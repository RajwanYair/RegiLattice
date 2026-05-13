namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static partial class PolicyAppControl
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _AddRemoveProgramsPolicy.Data,
            .. _AppCompatGpoPolicy.Data,
            .. _AppCompatibilityPolicy.Data,
            .. _AppConsentStorePolicy.Data,
            .. _AppContainerPolicy.Data,
            .. _AppContainerSandboxPolicy.Data,
            .. _AppGuardPolicy.Data,
            .. _AppInstallerPolicy.Data,
            .. _ApplicationGuardPersistencePolicy.Data,
            .. _ApplicationRestartPolicy.Data,
            .. _AppLockerAdvancedPolicy.Data,
            .. _AppLockerPolicy.Data,
            .. _AppLockerWdac.Data,
            .. _AppPermissions.Data,
            .. _AppPrivacyPolicy.Data,
            .. _AppPrivacyPolicyAdv.Data,
            .. _AppReadinessPolicy.Data,
            .. _AppSiloAdvPolicy.Data,
            .. _AppSiloPolicy.Data,
            .. _AppVirtualization.Data,
            .. _AppxBundlePolicy.Data,
            .. _AppXPackagingPolicy.Data,
            .. _AppxPolicy.Data,
            .. _AppxProvisioningPolicy.Data,
            .. _CodeIntegrityAppPolicy.Data,
            .. _CodeSigningPolicy.Data,
            .. _MicrosoftStorePolicy.Data,
            .. _MsiInstallerPolicy.Data,
            .. _PackagedAppDebugPolicy.Data,
            .. _PushToInstallPolicy.Data,
            .. _SmartAppControlPolicy.Data,
            .. _SoftwareRestrictionAdvPolicy.Data,
            .. _WdacCodeIntegrity.Data,
            .. _WindowsDefenderApplicationControlPolicy.Data,
            .. _WindowsInstallerAdvPolicy.Data,
            .. _WindowsInstallerPolicy.Data,
            .. _WindowsScriptHostPolicy.Data,
            .. _WindowsStoreForBusinessPolicy.Data,
        ];

    // ── AddRemoveProgramsPolicy ──
    private static class _AddRemoveProgramsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AddRemovePrograms";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "arpp-disable-add-remove-programs",
                    Label = "Disable Add or Remove Programs Applet",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets NoAddRemovePrograms=1 to hide the Add or Remove Programs item from Control Panel entirely. "
                        + "Users cannot access the application management interface for installing, modifying, or removing software. "
                        + "Standard users are prevented from browsing the installed application list via this interface.",
                    Tags = ["add-remove-programs", "control-panel", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Hides Add/Remove Programs applet entirely; blocks user-initiated software management via Control Panel.",
                    ApplyOps = [RegOp.SetDword(Key, "NoAddRemovePrograms", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoAddRemovePrograms")],
                    DetectOps = [RegOp.CheckDword(Key, "NoAddRemovePrograms", 1)],
                },
                new TweakDef
                {
                    Id = "arpp-hide-add-new-programs",
                    Label = "Hide Add New Programs Tab",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets NoAddPage=1 to hide the 'Add New Programs' tab in the Add or Remove Programs applet. "
                        + "Users cannot browse or install programs from CD/DVD, floppy disk, or network-hosted installation packages "
                        + "via the Control Panel interface.",
                    Tags = ["add-remove-programs", "installation", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removes the Add New Programs tab; users cannot initiate software installs from media via this interface.",
                    ApplyOps = [RegOp.SetDword(Key, "NoAddPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoAddPage")],
                    DetectOps = [RegOp.CheckDword(Key, "NoAddPage", 1)],
                },
                new TweakDef
                {
                    Id = "arpp-hide-windows-components",
                    Label = "Hide Add/Remove Windows Components Page",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets NoWindowsSetupPage=1 to remove the 'Add/Remove Windows Components' page from the applet. "
                        + "Prevents users from adding or removing built-in Windows features and optional components such as "
                        + "IIS, Hyper-V, and legacy subsystem components through the Control Panel interface.",
                    Tags = ["add-remove-programs", "windows-features", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Hides optional Windows component management page; installed component state unchanged.",
                    ApplyOps = [RegOp.SetDword(Key, "NoWindowsSetupPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoWindowsSetupPage")],
                    DetectOps = [RegOp.CheckDword(Key, "NoWindowsSetupPage", 1)],
                },
                new TweakDef
                {
                    Id = "arpp-prevent-program-changes",
                    Label = "Prevent Changing Installed Programs",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets NoChangePage=1 to hide the Change Program button in Add or Remove Programs. "
                        + "Users are prevented from running application setup programs to modify or repair installed software. "
                        + "This prevents self-repair and configuration changes that could alter application behaviour.",
                    Tags = ["add-remove-programs", "modification", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Blocks in-place software modification; repair or reconfigure via Add/Remove Programs will not work.",
                    ApplyOps = [RegOp.SetDword(Key, "NoChangePage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoChangePage")],
                    DetectOps = [RegOp.CheckDword(Key, "NoChangePage", 1)],
                },
                new TweakDef
                {
                    Id = "arpp-block-remove-programs",
                    Label = "Block Remove Programs Page",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets NoRemovePage=1 to hide the Remove Programs section in Add or Remove Programs. "
                        + "Prevents standard users from uninstalling any software via the Control Panel interface, "
                        + "ensuring software removal requires elevated admin credentials and IT oversight.",
                    Tags = ["add-remove-programs", "uninstall", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Prevents software removal via Control Panel; uninstall must be performed by an administrator.",
                    ApplyOps = [RegOp.SetDword(Key, "NoRemovePage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoRemovePage")],
                    DetectOps = [RegOp.CheckDword(Key, "NoRemovePage", 1)],
                },
                new TweakDef
                {
                    Id = "arpp-hide-support-info",
                    Label = "Hide Software Support Information Links",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets NoSupportInfo=1 to remove the Support Information hyperlink from the Change or Remove Programs list. "
                        + "Prevents disclosure of vendor contact information and URLs embedded in installed software registry entries, "
                        + "reducing information available to users attempting to circumvent IT software management policies.",
                    Tags = ["add-remove-programs", "support", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Hides manufacturer support links in the applet list; cosmetic administrative control.",
                    ApplyOps = [RegOp.SetDword(Key, "NoSupportInfo", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoSupportInfo")],
                    DetectOps = [RegOp.CheckDword(Key, "NoSupportInfo", 1)],
                },
                new TweakDef
                {
                    Id = "arpp-block-add-from-network",
                    Label = "Block Adding Programs from Network Shares",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets NoAddFromNetwork=1 to hide the option to add programs from a network share or corporate distribution point "
                        + "within the Add or Remove Programs applet. Prevents users from browsing network paths and self-installing "
                        + "software packages without IT approval.",
                    Tags = ["add-remove-programs", "network", "installation", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Removes network-install option; admin/SCCM-pushed deployments are unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "NoAddFromNetwork", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoAddFromNetwork")],
                    DetectOps = [RegOp.CheckDword(Key, "NoAddFromNetwork", 1)],
                },
                new TweakDef
                {
                    Id = "arpp-hide-services-page",
                    Label = "Hide Add/Remove Services Tab",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets NoServices=1 to remove the 'Services' tab from the Add or Remove Programs applet, "
                        + "preventing users from enabling or disabling optional system services via this interface. "
                        + "Service state management is restricted to administrators using Services.msc or sc.exe.",
                    Tags = ["add-remove-programs", "services", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Hides the Services tab; service configuration state is not changed.",
                    ApplyOps = [RegOp.SetDword(Key, "NoServices", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoServices")],
                    DetectOps = [RegOp.CheckDword(Key, "NoServices", 1)],
                },
                new TweakDef
                {
                    Id = "arpp-hide-choose-programs",
                    Label = "Hide Set Program Access and Defaults Page",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets NoChooseProgramsPage=1 to hide the 'Set Program Access and Computer Defaults' page "
                        + "that allows users to configure default programs for file types and protocols. "
                        + "Complements DefaultBrowserPolicy and file-association GPO controls for consistent default-app enforcement.",
                    Tags = ["add-remove-programs", "defaults", "file-associations", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Removes the Set Program Access page; users cannot change default program assignments via this interface.",
                    ApplyOps = [RegOp.SetDword(Key, "NoChooseProgramsPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoChooseProgramsPage")],
                    DetectOps = [RegOp.CheckDword(Key, "NoChooseProgramsPage", 1)],
                },
                new TweakDef
                {
                    Id = "arpp-enforce-category-view",
                    Label = "Enforce Category View in Add/Remove Programs",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets DefaultCategory=1 to force the Add or Remove Programs applet to display the installed program list "
                        + "sorted by category as the default view. Overrides any per-user sort preference stored in the user profile, "
                        + "presenting a consistent categorised view for all users on the machine.",
                    Tags = ["add-remove-programs", "category", "ui", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Forces category-sorted view in Add/Remove Programs; cosmetic administrative preference only.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultCategory", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultCategory")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultCategory", 1)],
                },
            ];
    }

    // ── AppCompatGpoPolicy ──
    private static class _AppCompatGpoPolicy
    {
        private const string AppComp = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "accompat-disable-user-assistance-telemetry",
                Label = "Disable User Assistance Telemetry",
                Category = "Security — Add Remove Programs",
                Description = "Disables the User Assistance Telemetry component from sending application crash and help request data to Microsoft.",
                Tags = ["appcompat", "privacy", "telemetry", "group-policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(AppComp, "DisableUAT", 1)],
                RemoveOps = [RegOp.DeleteValue(AppComp, "DisableUAT")],
                DetectOps = [RegOp.CheckDword(AppComp, "DisableUAT", 1)],
            },
            new TweakDef
            {
                Id = "accompat-disable-wizard",
                Label = "Disable Program Compatibility Wizard",
                Category = "Security — Add Remove Programs",
                Description =
                    "Removes the Program Compatibility Wizard from the context menu and prevents users from running it to set compatibility modes.",
                Tags = ["appcompat", "group-policy", "debloat"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(AppComp, "DisableWizard", 1)],
                RemoveOps = [RegOp.DeleteValue(AppComp, "DisableWizard")],
                DetectOps = [RegOp.CheckDword(AppComp, "DisableWizard", 1)],
            },
            new TweakDef
            {
                Id = "accompat-prevent-access-16bit",
                Label = "Prevent Access to 16-bit Applications",
                Category = "Security — Add Remove Programs",
                Description = "Blocks execution of 16-bit applications by disabling the Windows on Windows (WOW) subsystem via Group Policy.",
                Tags = ["appcompat", "security", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(AppComp, "Prevent16BitMSDos", 1)],
                RemoveOps = [RegOp.DeleteValue(AppComp, "Prevent16BitMSDos")],
                DetectOps = [RegOp.CheckDword(AppComp, "Prevent16BitMSDos", 1)],
            },
            new TweakDef
            {
                Id = "accompat-turn-off-windows-error-reporting",
                Label = "Turn Off App Compatibility Windows Error Reporting",
                Category = "Security — Add Remove Programs",
                Description = "Suppresses Windows Error Reporting prompts generated by the Application Compatibility framework.",
                Tags = ["appcompat", "privacy", "telemetry", "group-policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(AppComp, "DisableWER", 1)],
                RemoveOps = [RegOp.DeleteValue(AppComp, "DisableWER")],
                DetectOps = [RegOp.CheckDword(AppComp, "DisableWER", 1)],
            },
        ];
    }

    // ── AppCompatibilityPolicy ──
    private static class _AppCompatibilityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ApplicationCompatibility";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "appcompat-disable-pca",
                    Label = "Disable Program Compatibility Assistant",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Disables the Program Compatibility Assistant (PCA) that monitors application launches and prompts users to run programs in a compatibility mode when failure is detected. PCA interactions can generate telemetry events and prompt users into changing application settings. Default: PCA enabled. Recommended: 1 on locked-down managed desktops.",
                    Tags = ["app-compat", "pca", "assistant", "compatibility", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "PCA prompts are suppressed; compatibility dialogs after application crashes or installation failures are hidden.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePCA", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePCA")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePCA", 1)],
                },
                new TweakDef
                {
                    Id = "appcompat-disable-engine",
                    Label = "Disable Application Compatibility Engine",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Turns off the Windows application compatibility shim engine. Shims intercept Win32 API calls and transparently modify behaviour for legacy applications. Disabling the engine prevents any shim from being applied and removes the attack surface of the shim infrastructure. Caution: may break some legacy applications. Default: engine enabled. Recommended: 1 where all deployed apps are tested on current Windows.",
                    Tags = ["app-compat", "shim", "engine", "legacy", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Compatibility shims are disabled; legacy application behaviour changes may surface. Test before deployment.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableEngine", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableEngine")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableEngine", 1)],
                },
                new TweakDef
                {
                    Id = "appcompat-disable-removal-program",
                    Label = "Disable Application Compatibility Removal Program",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Suppresses the prompt that appears when a program known to be incompatible with the current version of Windows is detected. The prompt normally says 'This program might not have installed correctly' and can lead to unintended re-installation attempts. Default: prompt enabled. Recommended: 1 on managed fleets.",
                    Tags = ["app-compat", "removal", "prompt", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote =
                        "Post-install compatibility prompts are suppressed; users are not prompted to reinstall known incompatible programs.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableInventory", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableInventory")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableInventory", 1)],
                },
                new TweakDef
                {
                    Id = "appcompat-disable-sdb-lookup-online",
                    Label = "Disable Online SDB Look-up for App Compatibility",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Prevents Windows from querying Microsoft's online SDB (Shim Data Base) to check whether a running application has a known compatibility fix that should be applied. Reduces outbound network calls and removes a subtle data channel where app hashes are sent. Default: online SDB lookup enabled. Recommended: 1.",
                    Tags = ["app-compat", "sdb", "shim", "online-lookup", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Windows does not query Microsoft's online shim database; no app fingerprints are sent to Microsoft.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableProgramCompatibilityWizard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableProgramCompatibilityWizard")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableProgramCompatibilityWizard", 1)],
                },
                new TweakDef
                {
                    Id = "appcompat-disable-telemetry",
                    Label = "Disable Application Compatibility Telemetry Upload",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Blocks Windows from uploading application compatibility telemetry events (crash data, failed launch events, installer outcomes) to Microsoft Watson servers. Reduces the PCA telemetry data channel. Default: telemetry uploaded. Recommended: 1.",
                    Tags = ["app-compat", "telemetry", "privacy", "watson", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Application compatibility telemetry is not uploaded to Microsoft; no app execution data is sent.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUAR", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUAR")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUAR", 1)],
                },
                new TweakDef
                {
                    Id = "appcompat-allow-only-approved-shims",
                    Label = "Allow Only IT-Approved Compatibility Shims",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Restricts SDB shim loading so that only shims from administrator-supplied SDB files can be applied. Prevents attackers from installing malicious custom shims (a known persistence technique) by blocking user-context SDB installs. Default: any SDB files can be installed. Recommended: 1.",
                    Tags = ["app-compat", "shim", "sdb", "security", "persistence", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only administrator-installed SDB shims are applied; user-context malicious shim installs are blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowUserShims", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowUserShims")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowUserShims", 0)],
                },
                new TweakDef
                {
                    Id = "appcompat-block-sdb-user-install",
                    Label = "Block Users from Installing SDB Files",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Prevents standard users from registering application compatibility database (.sdb) files in the registry. SDB-based persistence (Shim-Based Patch Injection) is a known attack technique. Restricting SDB installs to administrators significantly reduces this attack vector. Default: no restriction. Recommended: 1.",
                    Tags = ["app-compat", "sdb", "persistence", "user-restriction", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only admins can register SDB files; non-admin SDB shim persistence technique is blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUserSDBInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUserSDBInstall")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUserSDBInstall", 1)],
                },
                new TweakDef
                {
                    Id = "appcompat-disable-compatibility-chooser-ui",
                    Label = "Disable Compatibility Chooser UI in Right-Click Menu",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Removes the 'Troubleshoot Compatibility' option from the Explorer right-click context menu for executable files. Prevents users from launching the Program Compatibility Troubleshooter which could change per-user compatibility settings. Default: option shown. Recommended: 1 on managed desktops.",
                    Tags = ["app-compat", "context-menu", "ui", "troubleshooter", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote =
                        "'Troubleshoot Compatibility' is removed from Explorer context menus; compatibility settings cannot be changed by users.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCompatChooserUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCompatChooserUI")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCompatChooserUI", 1)],
                },
                new TweakDef
                {
                    Id = "appcompat-log-shim-events",
                    Label = "Log Application Compatibility Shim Events",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Enables logging of shim application events (which SDB was applied, which process, and which API was intercepted) to the Application Compatibility event log channel. Provides forensic visibility into shim activity for threat hunting. Default: shim events not logged. Recommended: 1.",
                    Tags = ["app-compat", "shim", "audit", "logging", "forensics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Shim loading events are written to the event log; suspicious shim-based persistence is detectable.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableCompatibilityLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableCompatibilityLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableCompatibilityLogging", 1)],
                },
                new TweakDef
                {
                    Id = "appcompat-disable-switches-per-process",
                    Label = "Disable Per-Process Compatibility Settings Override",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Prevents per-user compatibility settings stored in HKCU from overriding machine-wide app compatibility configuration. Ensures that even if a user manually sets a compatibility mode for an application, the system policy takes precedence. Default: per-user HKCU overrides allowed. Recommended: 1 on managed desktops.",
                    Tags = ["app-compat", "per-process", "override", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "User-level per-exe compatibility settings cannot override machine-scope policy; HKCU compatibility flags are ignored.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSwitchesPerProcess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSwitchesPerProcess")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSwitchesPerProcess", 1)],
                },
            ];
    }

    // ── AppConsentStorePolicy ──
    private static class _AppConsentStorePolicy
    {
        private const string AcsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppConsentStore";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "acspol-disable-consent-store",
                    Label = "Disable App Consent Store",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets Enable=0 to disable the Windows App Consent Store that tracks and manages per-app privacy consent decisions. Apps requiring user consent are denied automatically.",
                    Tags = ["consent", "privacy", "apps", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "App consent tracking disabled; may silently deny app access to calendar, contacts, etc.",
                    ApplyOps = [RegOp.SetDword(AcsKey, "Enable", 0)],
                    RemoveOps = [RegOp.DeleteValue(AcsKey, "Enable")],
                    DetectOps = [RegOp.CheckDword(AcsKey, "Enable", 0)],
                },
                new TweakDef
                {
                    Id = "acspol-restrict-app-consent-grants",
                    Label = "Restrict Automatic App Consent Grants",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets AllowConsentForApps=0 to prevent apps from receiving automatic consent grants. Every app consent request will require explicit user approval.",
                    Tags = ["consent", "privacy", "apps", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Auto-consent disabled; users must manually approve each app's permission request.",
                    ApplyOps = [RegOp.SetDword(AcsKey, "AllowConsentForApps", 0)],
                    RemoveOps = [RegOp.DeleteValue(AcsKey, "AllowConsentForApps")],
                    DetectOps = [RegOp.CheckDword(AcsKey, "AllowConsentForApps", 0)],
                },
                new TweakDef
                {
                    Id = "acspol-block-sensitive-consent",
                    Label = "Block Sensitive Information App Consent",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets AllowSensitiveConsentForApps=0 to block apps from requesting consent to access sensitive personal information categories such as health, financial, or communication data.",
                    Tags = ["consent", "sensitive", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Apps cannot request sensitive data consent; protects personal health/financial info.",
                    ApplyOps = [RegOp.SetDword(AcsKey, "AllowSensitiveConsentForApps", 0)],
                    RemoveOps = [RegOp.DeleteValue(AcsKey, "AllowSensitiveConsentForApps")],
                    DetectOps = [RegOp.CheckDword(AcsKey, "AllowSensitiveConsentForApps", 0)],
                },
                new TweakDef
                {
                    Id = "acspol-disable-consent-ux",
                    Label = "Disable App Consent User Interface",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets DisableConsentUx=1 to suppress the app consent dialog UI. Consent decisions are handled silently according to current policy without surfacing prompts to the user.",
                    Tags = ["consent", "ui", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Consent prompts hidden; apps receive policy-driven responses without user interaction.",
                    ApplyOps = [RegOp.SetDword(AcsKey, "DisableConsentUx", 1)],
                    RemoveOps = [RegOp.DeleteValue(AcsKey, "DisableConsentUx")],
                    DetectOps = [RegOp.CheckDword(AcsKey, "DisableConsentUx", 1)],
                },
                new TweakDef
                {
                    Id = "acspol-require-admin-consent-approval",
                    Label = "Require Administrator Consent Approval",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets RequireAdminApproval=1 so that all app consent requests must be explicitly approved by an administrator. Standard users cannot grant app permissions independently.",
                    Tags = ["consent", "admin", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot grant app permissions; admin approval required for each consent.",
                    ApplyOps = [RegOp.SetDword(AcsKey, "RequireAdminApproval", 1)],
                    RemoveOps = [RegOp.DeleteValue(AcsKey, "RequireAdminApproval")],
                    DetectOps = [RegOp.CheckDword(AcsKey, "RequireAdminApproval", 1)],
                },
                new TweakDef
                {
                    Id = "acspol-disable-consent-prompts",
                    Label = "Disable App Consent Prompts for Standard Users",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets DisableConsentPrompts=1 to prevent consent dialog prompts from appearing for standard users. All consent decisions are handled by Group Policy settings.",
                    Tags = ["consent", "prompts", "users", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Consent prompts removed for standard users; policy-defined defaults apply silently.",
                    ApplyOps = [RegOp.SetDword(AcsKey, "DisableConsentPrompts", 1)],
                    RemoveOps = [RegOp.DeleteValue(AcsKey, "DisableConsentPrompts")],
                    DetectOps = [RegOp.CheckDword(AcsKey, "DisableConsentPrompts", 1)],
                },
                new TweakDef
                {
                    Id = "acspol-block-third-party-app-consent",
                    Label = "Block Third-Party App Consent Requests",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets BlockThirdPartyConsent=1 to prevent sideloaded or third-party applications from requesting access to sensitive resources through the consent store.",
                    Tags = ["consent", "third-party", "sideload", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Third-party apps blocked from requesting consent; Store and enterprise apps are unaffected.",
                    ApplyOps = [RegOp.SetDword(AcsKey, "BlockThirdPartyConsent", 1)],
                    RemoveOps = [RegOp.DeleteValue(AcsKey, "BlockThirdPartyConsent")],
                    DetectOps = [RegOp.CheckDword(AcsKey, "BlockThirdPartyConsent", 1)],
                },
                new TweakDef
                {
                    Id = "acspol-disable-consent-history",
                    Label = "Disable App Consent Decision History",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets DisableConsentHistory=1 to prevent the consent store from recording a history of app consent decisions. Improves privacy by not persisting consent audit trails locally.",
                    Tags = ["consent", "history", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Consent decision history not stored locally; no audit trail of past app permission grants.",
                    ApplyOps = [RegOp.SetDword(AcsKey, "DisableConsentHistory", 1)],
                    RemoveOps = [RegOp.DeleteValue(AcsKey, "DisableConsentHistory")],
                    DetectOps = [RegOp.CheckDword(AcsKey, "DisableConsentHistory", 1)],
                },
                new TweakDef
                {
                    Id = "acspol-restrict-consent-data-collection",
                    Label = "Restrict Consent Data Collection by Apps",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets RestrictConsentDataCollection=1 to limit the types of data that applications can be granted consent to collect through the Windows consent store framework.",
                    Tags = ["consent", "data-collection", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Apps limited in the scope of data they can collect through consent grants.",
                    ApplyOps = [RegOp.SetDword(AcsKey, "RestrictConsentDataCollection", 1)],
                    RemoveOps = [RegOp.DeleteValue(AcsKey, "RestrictConsentDataCollection")],
                    DetectOps = [RegOp.CheckDword(AcsKey, "RestrictConsentDataCollection", 1)],
                },
                new TweakDef
                {
                    Id = "acspol-disable-consent-notifications",
                    Label = "Disable App Consent Change Notifications",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets DisableConsentNotifications=1 to suppress system notifications when apps are granted or denied consent to access resources. Reduces noise from consent-related toasts.",
                    Tags = ["consent", "notifications", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Consent-related toast notifications suppressed; consent decisions still take effect silently.",
                    ApplyOps = [RegOp.SetDword(AcsKey, "DisableConsentNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(AcsKey, "DisableConsentNotifications")],
                    DetectOps = [RegOp.CheckDword(AcsKey, "DisableConsentNotifications", 1)],
                },
            ];
    }

    // ── AppContainerPolicy ──
    private static class _AppContainerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppContainer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appcont-disable-loopback",
                Label = "Disable App Container Loopback",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "App Container loopback exemptions allow UWP apps running inside App Container sandboxes to make network connections to localhost and other loopback addresses. Disabling loopback access prevents sandboxed apps from communicating with local services or other processes via the loopback adapter. App Container isolation is designed to prevent sandboxed apps from accessing sensitive local resources and services. Loopback connections can bypass the intended network isolation of App Container by allowing communication with locally running privileged services. Restricting loopback access enforces stricter sandbox isolation for UWP applications. Applications requiring loopback access for legitimate development or proxy scenarios require explicit exemptions through approved mechanisms.",
                Tags = ["appcontainer", "sandbox", "loopback", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLoopback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLoopback")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLoopback", 1)],
            },
            new TweakDef
            {
                Id = "appcont-disable-capability-enumeration",
                Label = "Disable App Container Capability Enumeration",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "App Container capability enumeration exposes detailed information about the capabilities declared by sandboxed UWP applications. Disabling capability enumeration prevents the capability list from being queried through the shell and system APIs. Capability information can be used by malicious code to identify apps with elevated permissions or sensitive access grants. Limiting capability discovery reduces information exposure about the privilege levels of installed applications. Enterprise application assessments should use managed inventory tools rather than runtime capability enumeration. Disabling this feature has no functional impact on the operation of installed applications.",
                Tags = ["appcontainer", "capabilities", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCapabilityEnumeration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCapabilityEnumeration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCapabilityEnumeration", 1)],
            },
            new TweakDef
            {
                Id = "appcont-disable-network-access",
                Label = "Restrict App Container Network Access",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "App Container network access controls whether sandboxed UWP applications can make network connections to external resources. Restricting network access prevents App Container applications from communicating with external services without explicit network capability declarations. Unauthorized network access from sandboxed applications represents a data exfiltration risk for enterprise environments. UWP applications should declare required network capabilities at packaging time and have those capabilities reviewed before deployment. Blanket restriction of network access forces application review and approval before any sandbox network connectivity is permitted. Applications with legitimate network requirements should be approved through the enterprise application governance process.",
                Tags = ["appcontainer", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictNetworkAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictNetworkAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictNetworkAccess", 1)],
            },
            new TweakDef
            {
                Id = "appcont-disable-local-filesystem",
                Label = "Restrict App Container Local Filesystem Access",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "App Container filesystem access controls allow sandboxed UWP applications to request access to user libraries, documents, and other local filesystem locations. Restricting local filesystem access prevents sandboxed applications from reading or writing to sensitive filesystem locations beyond their designated isolated storage. Enterprise document stores and user data should not be accessible to sandboxed applications without explicit entitlement review. File access capabilities declared in the application manifest are subject to runtime user consent prompts which can be inadvertently approved. Policy-level restriction provides a mandatory control layer that operates below the user consent mechanism. Applications requiring filesystem access should be assessed for data handling practices before deployment.",
                Tags = ["appcontainer", "filesystem", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictLocalFilesystemAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictLocalFilesystemAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictLocalFilesystemAccess", 1)],
            },
            new TweakDef
            {
                Id = "appcont-disable-clipboard-access",
                Label = "Restrict App Container Clipboard Access",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "The clipboard is a shared resource used to transfer data between applications and often contains sensitive information copied from enterprise applications. App Container clipboard access allows sandboxed UWP applications to read from and write to the system clipboard. Restricting clipboard access prevents sandboxed applications from monitoring clipboard contents or injecting malicious clipboard data. Clipboard monitoring from sandboxed applications represents a low-privilege data exfiltration path in enterprise environments. DLP controls for clipboard data are more effective when combined with App Container clipboard restrictions. Applications requiring clipboard integration should be evaluated for data sensitivity and approved explicitly.",
                Tags = ["appcontainer", "clipboard", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictClipboardAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictClipboardAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictClipboardAccess", 1)],
            },
            new TweakDef
            {
                Id = "appcont-disable-usb-access",
                Label = "Restrict App Container USB Access",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "App Container USB access capability allows sandboxed UWP applications to communicate with USB-connected devices including HID devices and custom USB hardware. Restricting USB access prevents sandboxed applications from reading from or writing to USB devices without explicit approval. USB interfaces have been used as covert channels for data exfiltration and as attack surfaces against device firmware. Sandboxed applications rarely have legitimate reasons to communicate with arbitrary USB devices in enterprise deployments. USB device access for UWP applications should be reviewed against specific workflow requirements and granted selectively. Restricting this capability reduces the risk of malicious applications abusing USB access for reconnaissance or data theft.",
                Tags = ["appcontainer", "usb", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictUsbAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictUsbAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictUsbAccess", 1)],
            },
            new TweakDef
            {
                Id = "appcont-disable-com-access",
                Label = "Restrict App Container COM Server Access",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "COM server access from App Container allows sandboxed UWP applications to activate and communicate with out-of-process COM servers registered on the system. Restricting COM server access prevents sandboxed applications from leveraging COM to interact with higher-privileged processes and system services. COM objects exposed by system services or administrative applications may be accessible to sandboxed apps with insufficient access controls. The COM attack surface has historically been used for privilege escalation from sandboxed contexts. Restricting COM access from App Container strengthens the sandbox boundary against COM-based escape paths. Enterprise UWP applications with COM dependencies should be assessed and explicitly exempted if required.",
                Tags = ["appcontainer", "com", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictComAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictComAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictComAccess", 1)],
            },
            new TweakDef
            {
                Id = "appcont-disable-telemetry",
                Label = "Disable App Container Telemetry",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "App Container telemetry reports usage data about sandboxed application behavior, capability usage, and isolation events to Microsoft. This telemetry data helps improve the App Container security model and identify compatibility issues. Disabling this telemetry prevents information about installed sandboxed applications and their access patterns from being reported. Application behavior patterns represent sensitive operational information in enterprise environments. Telemetry should be evaluated under data governance policies before being permitted to transmit enterprise application usage data. App Container sandbox enforcement operates independently of this telemetry setting and is fully maintained.",
                Tags = ["appcontainer", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "appcont-disable-auto-launch",
                Label = "Disable App Container Automatic Launch",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "App Container automatic launch allows UWP applications to be launched automatically in response to system events, file associations, and protocol handlers. Disabling automatic launch prevents sandboxed applications from being invoked automatically without explicit user or administrator initiation. Automatic application launch triggered by file associations or protocol handlers can be exploited to launch attacker-controlled applications. Enterprise environment application launches should be controlled through managed deployment tools rather than automatic content-based triggers. Restricting automatic launch reduces the likelihood of sandboxed apps being invoked through social engineering attacks using malicious file types. Manual launch of approved applications continues to function normally.",
                Tags = ["appcontainer", "startup", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoLaunch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoLaunch")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoLaunch", 1)],
            },
            new TweakDef
            {
                Id = "appcont-enforce-app-isolation",
                Label = "Enforce App Container Strict Isolation",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "App Container strict isolation enforces the strongest available sandbox boundaries, blocking all capability requests that have not been explicitly approved by policy. Enabling strict isolation prevents any relaxation of the App Container security model through API compatibility shims or legacy exception paths. Default Windows App Container settings include several compatibility adjustments that slightly weaken the sandbox for application compatibility reasons. Strict isolation removes these compatibility relaxations to enforce the maximum intended sandbox boundary. Enterprise environments prioritizing security over UWP application compatibility should enable strict isolation. Applications that fail under strict isolation require developer remediation to use the App Container model correctly.",
                Tags = ["appcontainer", "isolation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceStrictIsolation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceStrictIsolation")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceStrictIsolation", 1)],
            },
        ];
    }

    // ── AppContainerSandboxPolicy ──
    private static class _AppContainerSandboxPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";
        private const string IsoKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppIsolation";
        private const string AppKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "appcon-deny-broadfileaccess",
                    Label = "Deny Broad File System Access Capability to UWP Apps",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Blocks UWP apps from exercising the broadFileSystemAccess capability that allows reading files outside the app's sandbox, preventing apps from reading arbitrary user files even if they declare the capability in their manifest.",
                    Tags = ["appcontainer", "broad-file-access", "capability", "sandbox", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "UWP broadFileSystemAccess capability denied; Store apps cannot read files outside their package sandbox.",
                    ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessFileSystem", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessFileSystem")],
                    DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessFileSystem", 2)],
                },
                new TweakDef
                {
                    Id = "appcon-enable-appcontainer-network-isolation",
                    Label = "Enable Network Isolation for AppContainer Processes",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Enforces strict network isolation for AppContainer processes, ensuring that UWP apps can only make network connections to endpoints declared in their manifest capabilities, blocking undeclared outbound connections.",
                    Tags = ["appcontainer", "network-isolation", "sandbox", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "AppContainer network isolation enforced; UWP apps can only connect to declared network endpoints.",
                    ApplyOps = [RegOp.SetDword(IsoKey, "EnforceNetworkIsolation", 1)],
                    RemoveOps = [RegOp.DeleteValue(IsoKey, "EnforceNetworkIsolation")],
                    DetectOps = [RegOp.CheckDword(IsoKey, "EnforceNetworkIsolation", 1)],
                },
                new TweakDef
                {
                    Id = "appcon-block-appcontainer-loopback",
                    Label = "Block AppContainer Loopback Exemption by Default",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Prevents UWP/AppContainer apps from being granted loopback network access exemptions that bypass AppContainer network isolation, ensuring all sandbox processes respect network isolation boundaries.",
                    Tags = ["appcontainer", "loopback", "network-isolation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Loopback exemptions blocked for AppContainer; sandbox apps cannot bypass network isolation via localhost.",
                    ApplyOps = [RegOp.SetDword(IsoKey, "BlockLoopbackExemption", 1)],
                    RemoveOps = [RegOp.DeleteValue(IsoKey, "BlockLoopbackExemption")],
                    DetectOps = [RegOp.CheckDword(IsoKey, "BlockLoopbackExemption", 1)],
                },
                new TweakDef
                {
                    Id = "appcon-disable-appcontainer-telemetry",
                    Label = "Disable AppContainer and AppPrivacy Telemetry to Microsoft",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Prevents AppContainer isolation components and app privacy capability grant telemetry from being sent to Microsoft, protecting information about app capability usage patterns from cloud disclosure.",
                    Tags = ["appcontainer", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AppContainer and app privacy telemetry to Microsoft disabled; capability grant stats not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(IsoKey, "DisableIsolationTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(IsoKey, "DisableIsolationTelemetry")],
                    DetectOps = [RegOp.CheckDword(IsoKey, "DisableIsolationTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "appcon-audit-capability-grants",
                    Label = "Audit AppContainer Capability Grant Events in Security Log",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Enables Security event log entries when a UWP application is granted access to a sensitive capability (location, microphone, camera, contacts, calendar), providing an audit trail of capability access grants.",
                    Tags = ["appcontainer", "capability", "audit", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AppContainer capability grant events logged in Security log; sensitive access grants auditable for compliance.",
                    ApplyOps = [RegOp.SetDword(IsoKey, "AuditCapabilityGrantEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(IsoKey, "AuditCapabilityGrantEvents")],
                    DetectOps = [RegOp.CheckDword(IsoKey, "AuditCapabilityGrantEvents", 1)],
                },
            ];
    }

    // ── AppGuardPolicy ──
    private static class _AppGuardPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appgrd-enable-managed-mode",
                Label = "Enable Microsoft Defender Application Guard Managed Mode",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Microsoft Defender Application Guard (MDAG) creates a hardware-isolated container for browsing untrusted websites preventing host system compromise. Enabling MDAG managed mode activates the Hyper-V container isolation for Microsoft Edge and protects the host from browser-based attacks. Managed mode uses enterprise-defined site lists to determine which sites are considered trusted and which must be opened in the isolated container. MDAG provides defense-in-depth for users who browse unknown external sites as container compromise does not affect the host operating system. The isolated container is discarded after each MDAG session preventing persistent malware from surviving browser session boundaries. MDAG requires hardware virtualization support and appropriate hardware which should be verified before enabling this policy.",
                Tags = ["app-guard", "edge", "isolation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowAppHVSI_ProviderSet", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowAppHVSI_ProviderSet")],
                DetectOps = [RegOp.CheckDword(Key, "AllowAppHVSI_ProviderSet", 1)],
            },
            new TweakDef
            {
                Id = "appgrd-disable-clipboard-host-to-container",
                Label = "Disable Clipboard from Host to Application Guard Container",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Clipboard sharing between the host and the Application Guard container allows data to flow between the trusted and untrusted environments. Disabling clipboard from host to container prevents accidental or intentional data leakage from the trusted host environment into isolated browsing sessions. Clipboard data copied in the host may contain sensitive credentials, personally identifiable information, or confidential documents. Disabling host-to-container clipboard sharing reduces the risk of data being captured by malicious content in the isolated container. Container-to-host clipboard may be separately controlled allowing users to copy content from isolated sessions back to the host. Organizations must balance user productivity with security when configuring clipboard sharing in Application Guard.",
                Tags = ["app-guard", "clipboard", "data-leakage", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AppHVSIClipboardSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIClipboardSettings")],
                DetectOps = [RegOp.CheckDword(Key, "AppHVSIClipboardSettings", 1)],
            },
            new TweakDef
            {
                Id = "appgrd-disable-clipboard-container-to-host",
                Label = "Disable Clipboard from Application Guard Container to Host",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Container-to-host clipboard sharing allows malicious content in the Application Guard container to inject data into the trusted host clipboard. Disabling container-to-host clipboard sharing prevents malicious code running in the isolated environment from passing data to the host system. Container-to-host clipboard attacks could inject malicious commands into the host clipboard that are executed when pasted into applications like PowerShell or terminals. Completely blocking container-to-host clipboard providing a complete bidirectional isolation between the browsing container and the host environment. Users who need to copy content from Application Guard sessions should use the approved content sharing mechanisms defined by IT policy. Setting clipboard to completely disabled for both directions provides maximum isolation at the cost of some user convenience.",
                Tags = ["app-guard", "clipboard", "container-escape", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AppHVSIClipboardFileType", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIClipboardFileType")],
                DetectOps = [RegOp.CheckDword(Key, "AppHVSIClipboardFileType", 0)],
            },
            new TweakDef
            {
                Id = "appgrd-disable-print-from-container",
                Label = "Disable Printing from Application Guard Container",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Printing from the Application Guard container sends document content from the isolated environment to physical printers potentially enabling data exfiltration. Disabling printing from Application Guard prevents untrusted web content from being printed to physical or virtual printers. Malicious websites in the container could initiate print jobs to capture screenshots or generate large volumes of print output. Disabling container printing ensures that print operations can only be initiated from trusted host applications preventing lateral data movement. Users who need to print from content viewed in Application Guard should be directed to copy content to the host environment through approved channels first. Print restrictions are particularly important in regulated environments where print activity must be controlled and audited.",
                Tags = ["app-guard", "printing", "data-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AppHVSIPrintingSettings", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIPrintingSettings")],
                DetectOps = [RegOp.CheckDword(Key, "AppHVSIPrintingSettings", 0)],
            },
            new TweakDef
            {
                Id = "appgrd-disable-container-persistence",
                Label = "Disable Application Guard Container Data Persistence",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Application Guard container persistence allows browser data like cookies, history, and cached content to be saved between sessions providing malicious code with persistence mechanisms. Disabling container persistence ensures that each Application Guard session starts fresh with no data from previous browsing sessions. Persistent container data including tracking cookies, session tokens, and malicious extensions would survive container restarts defeating isolation guarantees. Fresh container sessions on each launch ensure that any malicious code that executed in a previous session cannot influence subsequent sessions. Disabling persistence increases the security isolation guarantee at the cost of user convenience for sites requiring repeated authentication. Session persistence should only be enabled when users have a documented business need and appropriate security compensating controls are in place.",
                Tags = ["app-guard", "persistence", "isolation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AppHVSIContainerPersistence", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIContainerPersistence")],
                DetectOps = [RegOp.CheckDword(Key, "AppHVSIContainerPersistence", 0)],
            },
            new TweakDef
            {
                Id = "appgrd-enable-audit-logging",
                Label = "Enable Application Guard Usage Audit Logging",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Application Guard audit logging records session starts, container operations, and security policy violations related to MDAG usage. Enabling MDAG audit logging provides visibility into Application Guard usage patterns and potential security events within containers. Audit logs capture information about blocked network requests, copy-paste attempts, and other security-relevant container events. MDAG audit data helps identify users who are routinely browsing high-risk sites that trigger container usage indicating potential behavioral risks. Audit logs should be forwarded to SIEM and correlated with endpoint security events to detect potential container escape attempts. Regular review of MDAG audit data helps tune the trusted site lists and identify when container protections are most heavily used.",
                Tags = ["app-guard", "audit", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditApplicationGuard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditApplicationGuard")],
                DetectOps = [RegOp.CheckDword(Key, "AuditApplicationGuard", 1)],
            },
            new TweakDef
            {
                Id = "appgrd-block-enterprise-content-in-container",
                Label = "Block Access to Enterprise Sites inside Application Guard",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Blocking access to enterprise sites from within the Application Guard container prevents malicious code in the container from accessing internal corporate resources. Enterprise site access from within the container allows drive-by download attacks to potentially pivot to internal corporate applications using the user's session context. Blocking enterprise domains from container access ensures that untrusted browsing sessions cannot reach internal applications even with the user's credentials. Container-to-intranet blocking prevents Business Email Compromise attacks where users are tricked into clicking links that would access internal systems from an untrusted context. Enterprise site blocking rules should align with the Windows Information Protection trusted site list for consistent data protection. Blocking enterprise sites from the container does not affect normal host Edge browsing to trusted corporate resources.",
                Tags = ["app-guard", "enterprise-sites", "isolation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockNonEnterpriseContent", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockNonEnterpriseContent")],
                DetectOps = [RegOp.CheckDword(Key, "BlockNonEnterpriseContent", 0)],
            },
            new TweakDef
            {
                Id = "appgrd-disable-camera-microphone-in-container",
                Label = "Disable Camera and Microphone in Application Guard Container",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Camera and microphone access from the Application Guard container allows websites in the isolated environment to capture audio and video from the device. Disabling camera and microphone access in the Application Guard container prevents malicious websites from conducting surveillance of users. Rogue websites in the container could use camera or microphone to capture sensitive conversations or visual information present near the device. The isolation benefit of Application Guard is partially undermined if container content can access real hardware sensors. Camera and microphone should only be enabled in containers for specific documented use cases where untrusted browsing with media access is required. Container media restrictions protect against social engineering attacks that prompt users to allow camera access to untrusted websites.",
                Tags = ["app-guard", "camera", "microphone", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowCameraMicrophoneRedirection", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCameraMicrophoneRedirection")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCameraMicrophoneRedirection", 0)],
            },
            new TweakDef
            {
                Id = "appgrd-block-download-saving",
                Label = "Block Saving Downloaded Files from Application Guard",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Saving downloaded files from the Application Guard container to the host file system can introduce malware from untrusted sites to the trusted environment. Blocking downloads from Application Guard prevents malicious files downloaded in the isolated container from accessing the host file system. Drive-by download attacks in the container that deliver malware payloads are rendered ineffective when container downloads cannot reach the host. Files downloaded in the container remain contained within the isolated environment and are discarded when the container session ends. Users who need to save content from Application Guard browsing should be directed to approved transfer mechanisms like enterprise file sharing. Download blocking should be combined with container persistence disabled to ensure that malicious downloads cannot persist across container sessions.",
                Tags = ["app-guard", "downloads", "file-transfer", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SaveFilesToHost", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SaveFilesToHost")],
                DetectOps = [RegOp.CheckDword(Key, "SaveFilesToHost", 0)],
            },
            new TweakDef
            {
                Id = "appgrd-prevent-certificate-sharing",
                Label = "Prevent Certificate Sharing from Host to Application Guard",
                Category = "Security — Add Remove Programs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Certificate sharing from the host to the Application Guard container allows the container to use enterprise certificates for TLS authentication. Preventing certificate sharing ensures that enterprise identity certificates are not available to content running in the isolated container. Malicious content in the container with access to enterprise certificates could authenticate to enterprise services using the user's corporate identity. Certificate forwarding to the container effectively bridges the trust boundary between the untrusted browsing environment and enterprise identity. Enterprise certificates should remain scoped to the trusted host environment where access controls and monitoring provide appropriate oversight. Organizations that need enterprise certificate access in Application Guard should evaluate whether the use case justifies the reduced isolation boundary.",
                Tags = ["app-guard", "certificates", "identity", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AppHVSICertificateSharing", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AppHVSICertificateSharing")],
                DetectOps = [RegOp.CheckDword(Key, "AppHVSICertificateSharing", 0)],
            },
        ];
    }

    // ── AppInstallerPolicy ──
    private static class _AppInstallerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appins-disable-settings",
                Label = "Disable WinGet Settings Modification",
                Category = "Security — Add Remove Programs",
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
                Category = "Security — Add Remove Programs",
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
                Category = "Security — Add Remove Programs",
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
                Id = "appins-disable-additional-sources",
                Label = "Block Addition of Custom Package Sources",
                Category = "Security — Add Remove Programs",
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
                Category = "Security — Add Remove Programs",
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
                Category = "Security — Add Remove Programs",
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
                Category = "Security — Add Remove Programs",
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

    // ── ApplicationGuardPersistencePolicy ──
    private static class _ApplicationGuardPersistencePolicy
    {
        private const string WdagKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI";

        private const string OfficeWdagKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\AppHVSI";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wdagpe-disable-container-data-persistence",
                    Label = "WDAG Persistence: Disable Container Data Persistence Across Sessions",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets AllowPersistence=0 in AppHVSI policy. Disables WDAG container data persistence — ensuring that cookies, browser history, cached web content, and local storage inside the WDAG container are purged when the container is closed. This provides the strongest isolation: each session starts from a completely clean container image with no carry-over state from previous sessions. While this means users must re-authenticate to websites in each WDAG session, it prevents any session-to-session data leakage or attack artefact accumulation in the container.",
                    Tags = ["wdag", "persistence", "cookie-purge", "session-isolation", "clean-state"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "All container state is discarded on close. Users must log in to all WDAG-hosted websites on every new session. Acceptable for high-security environments. In mixed-security environments, consider enabling persistence with mandatory cleanup on compromise detection instead.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AllowPersistence", 0)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AllowPersistence")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AllowPersistence", 0)],
                },
                new TweakDef
                {
                    Id = "wdagpe-enable-office-application-guard",
                    Label = "WDAG Persistence: Enable Office Application Guard for Untrusted Documents",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets EnableOfficeApplicationGuard=1 in Office\\16.0\\Common\\AppHVSI policy. Enables Office Application Guard (OAG), which opens untrusted Office documents (Word, Excel, PowerPoint) received from the internet or marked as untrusted in a Hyper-V container. Malicious Office documents (weaponised macros, embedded OLE objects, exploit documents) open in the isolated container — if the document exploits a vulnerability in the Office parser, the exploit is contained. This is the most effective protection against socially-engineered office document attacks, which are the #1 initial access vector.",
                    Tags = ["office-application-guard", "word", "excel", "document-isolation", "hyperv"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Untrusted Office documents open in a Hyper-V container. Exploits in weaponised documents are contained. Requires WDAG-enabled hardware (Hyper-V, SLAT, 8GB+ RAM). Microsoft 365 Apps for Enterprise required. Documents from trusted locations (internal SharePoint, corporate file shares) are not affected and open normally.",
                    ApplyOps = [RegOp.SetDword(OfficeWdagKey, "EnableOfficeApplicationGuard", 1)],
                    RemoveOps = [RegOp.DeleteValue(OfficeWdagKey, "EnableOfficeApplicationGuard")],
                    DetectOps = [RegOp.CheckDword(OfficeWdagKey, "EnableOfficeApplicationGuard", 1)],
                },
                new TweakDef
                {
                    Id = "wdagpe-block-office-guard-macro-execution",
                    Label = "WDAG Persistence: Block Macro Execution in Office Application Guard",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets DisableMacrosInOfficeGuard=1 in Office\\16.0\\Common\\AppHVSI policy. Disables all VBA macro execution in documents opened inside the Office Application Guard container. Even within the isolated container, macros can perform network calls, attempt to communicate with the host, or interact with the container's file system. Blocking macros in the container provides defence-in-depth: if a document exploits a macro execution vulnerability, the macro cannot execute. Documents requiring macros should be opened outside the container only if they are trusted and have been scanned.",
                    Tags = ["office-guard", "macro", "vba", "disable", "container"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "VBA macros are blocked in Office Application Guard containers. Untrusted documents cannot execute macros even in the isolated container. Documents with required macros from trusted sources are not affected — they open outside the container. This is an additional safety layer on top of the container isolation.",
                    ApplyOps = [RegOp.SetDword(OfficeWdagKey, "DisableMacrosInOfficeGuard", 1)],
                    RemoveOps = [RegOp.DeleteValue(OfficeWdagKey, "DisableMacrosInOfficeGuard")],
                    DetectOps = [RegOp.CheckDword(OfficeWdagKey, "DisableMacrosInOfficeGuard", 1)],
                },
                new TweakDef
                {
                    Id = "wdagpe-restrict-guard-clipboard-to-host-in",
                    Label = "WDAG Persistence: Restrict Office Guard Clipboard to Host-to-Container Direction",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets OfficeguardClipboardSettings=2 in AppHVSI policy (host → container only). Configures clipboard behaviour for Office Application Guard containers to allow clipboard content from the host to be pasted into the container (enabling the user to paste text into a WDAG form) but blocking the container from exporting clipboard content to the host. This asymmetric clipboard policy prevents an exploit in the Office container from using the clipboard as a covert data exfiltration channel — a technique used by some document-borne malware.",
                    Tags = ["office-guard", "clipboard", "asymmetric", "exfiltration", "container"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Clipboard is restricted to host → container only in Office Guard sessions. Content from the WDAG Office document cannot be copied to the host clipboard. Users cannot copy-paste content from untrusted documents to host applications — they must save the document and use it as a trusted document after scanning.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "OfficeguardClipboardSettings", 2)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "OfficeguardClipboardSettings")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "OfficeguardClipboardSettings", 2)],
                },
                new TweakDef
                {
                    Id = "wdagpe-enforce-hardware-requirements",
                    Label = "WDAG Persistence: Enforce Hardware Requirement Check Before Starting WDAG",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets AppHVSIHardwareRequirementEnabled=1 in AppHVSI policy. Enables a pre-flight hardware compatibility check before WDAG containers are started. This check verifies that the CPU supports SLAT (for Hyper-V), VT-x/AMD-V is enabled in firmware, IOMMU is active (for DMA protection), and minimum RAM is available. If hardware requirements are not met, WDAG fails gracefully with a user-visible message rather than attempting to start a degraded container. Without this check, WDAG may start a container that appears functional but lacks proper isolation guarantees on incompatible hardware.",
                    Tags = ["wdag", "hardware-check", "slat", "vt-x", "iommu"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "WDAG performs hardware compatibility check before starting a container. Incompatible hardware (no SLAT, no IOMMU, insufficient RAM) fails the check and WDAG does not start. Users on incompatible hardware see an error message. Prevents degraded container isolation on unsupported hardware.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIHardwareRequirementEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIHardwareRequirementEnabled")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIHardwareRequirementEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "wdagpe-enable-container-threat-report",
                    Label = "WDAG Persistence: Enable WDAG Container Threat and Crash Reporting",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets AppHVSIThreatReportEnabled=1 in AppHVSI policy. Enables WDAG to send threat and container crash reports to the Windows Defender ATP (Defender for Endpoint) service when a container experiences anomalous crashes or attempted security boundary violations. These reports include container crash minidumps, the URL that was active when the crash occurred, and whether the crash pattern is consistent with known exploit signatures. Threat reports enable the SOC to detect when WDAG has actually stopped an exploit attempt — containers that crash unexpectedly are almost always indicators of an exploitation attempt.",
                    Tags = ["wdag", "threat-report", "crash", "mde", "exploit-detection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Container crashes and boundary violations are reported to Defender for Endpoint. Reports include crash minidumps and active URL. Requires Defender for Endpoint licence (MDE P2). In regulated environments, confirm the crash minidump data handling meets data residency requirements.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIThreatReportEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIThreatReportEnabled")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIThreatReportEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "wdagpe-block-extension-from-container",
                    Label = "WDAG Persistence: Block Browser Extension Usage in WDAG Container",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets AppHVSIExtensionBlock=1 in AppHVSI policy. Prevents browser extensions from loading within the WDAG Edge container. Browser extensions are a significant attack surface — a malicious extension installed in the host browser that also loads in the WDAG container could behave as a covert channel, passing data between the container and the internet or between the container and the host. Blocking extensions in the container ensures the WDAG isolation is not weakened by extension code that has access to both the container's DOM and the extension API.",
                    Tags = ["wdag", "extension-block", "browser-extension", "container", "covert-channel"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Browser extensions are disabled in the WDAG container. Extensions that users rely on (password managers, accessibility tools) are not available in WDAG sessions. Users who need extensions in the WDAG container must use trusted sites (which are outside the container).",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIExtensionBlock", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIExtensionBlock")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIExtensionBlock", 1)],
                },
                new TweakDef
                {
                    Id = "wdagpe-enable-automatic-container-update",
                    Label = "WDAG Persistence: Enable Automatic WDAG Container Image Update",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets AppHVSIAutoUpdateEnabled=1 in AppHVSI policy. Enables automatic updates of the WDAG base container image via Windows Update. The WDAG container image is essentially a minimal Windows installation. If the container image is not updated, it may accumulate known vulnerabilities within the container OS components — which, while isolated, could be leveraged to escape the isolation more easily. Automatic updates ensure that even if an attacker gains code execution within the container, the container itself is patched against known privilege escalation vulnerabilities.",
                    Tags = ["wdag", "auto-update", "container-image", "patch", "windows-update"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "WDAG container image is updated automatically via Windows Update. Container updates occur in the background without disrupting active sessions. Updates may require a WDAG service restart — active WDAG sessions may be disconnected when the container service restarts after an update.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIAutoUpdateEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIAutoUpdateEnabled")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIAutoUpdateEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "wdagpe-disable-container-proxy-bypass",
                    Label = "WDAG Persistence: Disable Proxy Bypass Inside the WDAG Container",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets AppHVSIProxyBypassEnabled=0 in AppHVSI policy. Prevents network traffic originating from within the WDAG container from bypassing the corporate proxy. Without this, a compromised website in the WDAG container that uses direct outbound connections (bypassing the proxy) can communicate with C2 infrastructure without appearing in proxy logs. Ensuring all container traffic goes through the corporate proxy enables DUT (Discover, Understand, Track) analysis of container network activity — even malicious connections from exploits are visible in proxy logs.",
                    Tags = ["wdag", "proxy", "bypass-prevention", "network-monitoring", "c2-detection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "WDAG container traffic cannot bypass the corporate proxy. All outbound connections from the container are routed via the proxy. Requires the proxy configuration to be accessible from the isolated container network. Enables proxy-based detection of malicious container traffic.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIProxyBypassEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIProxyBypassEnabled")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIProxyBypassEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "wdagpe-set-container-network-isolation-level-2",
                    Label = "WDAG Persistence: Set Container Network Isolation Level 2 (Restrict Host Communication)",
                    Category = "Security — Add Remove Programs",
                    Description =
                        "Sets AppHVSINetworkIsolationLevel=2 in AppHVSI policy. Sets the WDAG container network isolation level to 2 (restrictive: container can only reach the proxy and the trusted domain list; host-to-container direct communication is blocked). At level 1, the container can communicate to any internet address via the host proxy. At level 2, only the enterprise proxy endpoint and explicitly whitelisted external domains are reachable from the container network. This reduces the C2 communication surface — a compromised WDAG session can only reach domains the enterprise has explicitly permitted.",
                    Tags = ["wdag", "network-isolation", "level-2", "c2-restriction", "outbound"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Container outbound network is restricted to the proxy and whitelisted domains. Websites accessed through WDAG can only reach the internet via the approved proxy. Container network level 2 may break some websites that use direct connections or non-standard ports. Monitor proxy logs for blocked connection attempts.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSINetworkIsolationLevel", 2)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSINetworkIsolationLevel")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSINetworkIsolationLevel", 2)],
                },
            ];
    }

    // ── ApplicationRestartPolicy ──
    private static class _ApplicationRestartPolicy
    {
        private const string AeDebug = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug";
        private const string CrashCtl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";
        private const string WerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";
        private const string WerMain = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "apprstrt-disable-wer-queue",
                Label = "App Restart: Disable WER Problem Queue",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [WerPolicy],
                Tags = ["wer", "error-reporting", "queue", "privacy", "policy"],
                Description =
                    "Sets DontSendAdditionalData=1 in WER policy. Prevents WER from queuing and "
                    + "retrying transmission of error reports. Stops background upload attempts. "
                    + "Default: 0. Recommended alongside WER Disabled policy.",
                ApplyOps = [RegOp.SetDword(WerPolicy, "DontSendAdditionalData", 1)],
                RemoveOps = [RegOp.DeleteValue(WerPolicy, "DontSendAdditionalData")],
                DetectOps = [RegOp.CheckDword(WerPolicy, "DontSendAdditionalData", 1)],
            },
            new TweakDef
            {
                Id = "apprstrt-bypass-data-throttling",
                Label = "App Restart: Disable WER Data Throttling",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [WerPolicy],
                Tags = ["wer", "error-reporting", "throttle", "privacy", "policy"],
                Description =
                    "Sets BypassDataThrottling=1 in WER policy. Removes WER's bandwidth throttling "
                    + "which controls rate of error data transmission. "
                    + "Default: 0 (throttled). Used with WER Disabled to fully suppress crash telemetry.",
                ApplyOps = [RegOp.SetDword(WerPolicy, "BypassDataThrottling", 1)],
                RemoveOps = [RegOp.DeleteValue(WerPolicy, "BypassDataThrottling")],
                DetectOps = [RegOp.CheckDword(WerPolicy, "BypassDataThrottling", 1)],
            },
            new TweakDef
            {
                Id = "apprstrt-disable-wer-ui-consent",
                Label = "App Restart: Disable WER User Consent Prompts",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [WerPolicy],
                Tags = ["wer", "error-reporting", "consent", "ui", "privacy", "policy"],
                Description =
                    "Sets DefaultConsent=1 in WER policy. Silently suppresses all WER user-consent "
                    + "prompts ('Report this problem?'). No dialogs presented to users. "
                    + "Default: 4 (prompt). Value 1=Always Ask is overridden; combine with Disabled=1.",
                ApplyOps = [RegOp.SetDword(WerMain, "DefaultConsent", 1)],
                RemoveOps = [RegOp.DeleteValue(WerMain, "DefaultConsent")],
                DetectOps = [RegOp.CheckDword(WerMain, "DefaultConsent", 1)],
            },
            new TweakDef
            {
                Id = "apprstrt-wer-minimum-dump-size",
                Label = "App Restart: Reduce WER Dump Log Retention to 1 Entry",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [WerMain],
                Tags = ["wer", "error-reporting", "dump", "retention", "privacy", "policy"],
                Description =
                    "Sets MaxQueueSize=1 in WER. Limits the local WER crash queue to a single entry. "
                    + "Reduces the volume of crash report files sitting in AppData waiting for upload. "
                    + "Default: 50. Helps limit local crash data accumulation.",
                ApplyOps = [RegOp.SetDword(WerMain, "MaxQueueSize", 1)],
                RemoveOps = [RegOp.DeleteValue(WerMain, "MaxQueueSize")],
                DetectOps = [RegOp.CheckDword(WerMain, "MaxQueueSize", 1)],
            },
        ];
    }

    // ── AppLockerAdvancedPolicy ──
    private static class _AppLockerAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2";
        private const string ExeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Exe";
        private const string DllKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Dll";
        private const string MsiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Msi";
        private const string ScriptKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Script";
        private const string AppxKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Appx";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "alockadv-enable-applocker-audit-exe",
                    Label = "Enable AppLocker Audit Mode for Executables",
                    Category = "Security — Application Restart",
                    Description =
                        "Configures the AppLocker EXE rule collection to Audit mode, which logs every executable launch against rules (EventID 8004) without blocking it, enabling policy discovery before enforcement mode is activated.",
                    Tags = ["applocker", "audit-mode", "exe", "application-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "AppLocker EXE collection in Audit mode; all executable launches logged without blocking.",
                    ApplyOps = [RegOp.SetDword(ExeKey, "EnforcementMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExeKey, "EnforcementMode")],
                    DetectOps = [RegOp.CheckDword(ExeKey, "EnforcementMode", 1)],
                },
                new TweakDef
                {
                    Id = "alockadv-enable-applocker-dll-enforcement",
                    Label = "Enable AppLocker DLL Enforcement",
                    Category = "Security — Application Restart",
                    Description =
                        "Enables AppLocker DLL collection enforcement, which checks every DLL loaded by a process against AppLocker rules before allowing load, providing defence against DLL hijacking and side-loading attacks.",
                    Tags = ["applocker", "dll", "dll-enforcement", "application-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "AppLocker DLL enforcement active. All DLL loads checked — may impact performance significantly.",
                    ApplyOps = [RegOp.SetDword(DllKey, "EnforcementMode", 2)],
                    RemoveOps = [RegOp.DeleteValue(DllKey, "EnforcementMode")],
                    DetectOps = [RegOp.CheckDword(DllKey, "EnforcementMode", 2)],
                },
                new TweakDef
                {
                    Id = "alockadv-enable-applocker-script-enforcement",
                    Label = "Enable AppLocker Script Enforcement",
                    Category = "Security — Application Restart",
                    Description =
                        "Enables AppLocker Script collection enforcement for PowerShell (.ps1), batch (.cmd/.bat), VBScript (.vbs), and Windows Scripting Host files, blocking untrusted scripts from executing.",
                    Tags = ["applocker", "script", "powershell", "application-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "AppLocker script enforcement active; untrusted PS1/VBS/CMD scripts blocked from executing.",
                    ApplyOps = [RegOp.SetDword(ScriptKey, "EnforcementMode", 2)],
                    RemoveOps = [RegOp.DeleteValue(ScriptKey, "EnforcementMode")],
                    DetectOps = [RegOp.CheckDword(ScriptKey, "EnforcementMode", 2)],
                },
                new TweakDef
                {
                    Id = "alockadv-enable-applocker-appx-enforcement",
                    Label = "Enable AppLocker Packaged App (AppX) Enforcement",
                    Category = "Security — Application Restart",
                    Description =
                        "Enables AppLocker PackagedApp collection enforcement for MSIX/AppX Store-installed applications, blocking or auditing UWP apps that do not match configured publisher or package name rules.",
                    Tags = ["applocker", "appx", "msix", "store-apps", "application-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "AppLocker AppX enforcement active; unlisted Store apps blocked. Ensure allow rules cover required apps.",
                    ApplyOps = [RegOp.SetDword(AppxKey, "EnforcementMode", 2)],
                    RemoveOps = [RegOp.DeleteValue(AppxKey, "EnforcementMode")],
                    DetectOps = [RegOp.CheckDword(AppxKey, "EnforcementMode", 2)],
                },
                new TweakDef
                {
                    Id = "alockadv-enable-applocker-msi-enforcement",
                    Label = "Enable AppLocker Windows Installer (MSI) Enforcement",
                    Category = "Security — Application Restart",
                    Description =
                        "Enables AppLocker Windows Installer collection enforcement, blocking MSI and MSP installer execution that does not match publisher or path allow rules, preventing unauthorised software installation.",
                    Tags = ["applocker", "msi", "installer", "application-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "AppLocker MSI enforcement active; untrusted installers blocked. IT-managed MSIs must be allow-listed.",
                    ApplyOps = [RegOp.SetDword(MsiKey, "EnforcementMode", 2)],
                    RemoveOps = [RegOp.DeleteValue(MsiKey, "EnforcementMode")],
                    DetectOps = [RegOp.CheckDword(MsiKey, "EnforcementMode", 2)],
                },
                new TweakDef
                {
                    Id = "alockadv-enable-applocker-event-logging",
                    Label = "Enable AppLocker Policy Enforcement Event Logging",
                    Category = "Security — Application Restart",
                    Description =
                        "Configures the AppLocker event log to capture all rule enforcement (allowed, denied, audited) events in the Microsoft-Windows-AppLocker operational log for SOC and SIEM ingestion.",
                    Tags = ["applocker", "event-log", "audit", "application-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AppLocker enforcement events logged; all allow/deny decisions recorded in AppLocker operational log.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableAppLockerEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableAppLockerEventLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableAppLockerEventLogging", 1)],
                },
                new TweakDef
                {
                    Id = "alockadv-block-override-by-user",
                    Label = "Block Users from Overriding AppLocker Policy",
                    Category = "Security — Application Restart",
                    Description =
                        "Prevents standard users from modifying AppLocker configuration via local policy, ensuring application control rules can only be changed via domain GPO or local administrator action.",
                    Tags = ["applocker", "override", "standard-user", "application-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "AppLocker policy cannot be changed by standard users; changes require admin or domain GPO.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUserPolicyOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUserPolicyOverride")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUserPolicyOverride", 1)],
                },
                new TweakDef
                {
                    Id = "alockadv-allow-publisher-rules",
                    Label = "Allow Publisher-Based Rules as Default AppLocker Allow Strategy",
                    Category = "Security — Application Restart",
                    Description =
                        "Configures AppLocker to prefer publisher rules (signed certificate chains) over path rules, enabling software to be allowed based on an identified digital signature rather than a potentially spoofable file path.",
                    Tags = ["applocker", "publisher-rules", "digital-signature", "application-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Publisher-based allow rules preferred; signed software allowed by certificate chain rather than path.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultRuleStrategy", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultRuleStrategy")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultRuleStrategy", 2)],
                },
                new TweakDef
                {
                    Id = "alockadv-disable-applocker-telemetry",
                    Label = "Disable AppLocker Enforcement Telemetry to Microsoft",
                    Category = "Security — Application Restart",
                    Description =
                        "Prevents AppLocker from sending enforcement telemetry (blocked app names, hashes, publisher names) to Microsoft, protecting internal application inventory from cloud disclosure.",
                    Tags = ["applocker", "telemetry", "privacy", "microsoft", "application-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AppLocker telemetry to Microsoft disabled; blocked app names and hashes not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAppLockerTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAppLockerTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAppLockerTelemetry", 1)],
                },
            ];
    }

    // ── AppLockerPolicy ──
    private static class _AppLockerPolicy
    {
        private const string SrpBase = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2";
        private const string ExeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Exe";
        private const string MsiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Msi";
        private const string ScriptKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Script";
        private const string DllKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Dll";
        private const string AppxKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Appx";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "applocker-enable-appid-service",
                    Label = "Enable Application Identity Service for AppLocker",
                    Category = "Security — Application Restart",
                    Description =
                        "Configures the Application Identity (AppIDSvc) service to start automatically, which is required for AppLocker enforcement.",
                    Tags = ["applocker", "appid", "service", "policy", "application-control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "AppLocker will not enforce rules unless AppIDSvc is running; this tweak ensures it starts with Windows.",
                    ApplyOps = [RegOp.SetDword(SrpBase, "AppIdSvcStartType", 2)],
                    RemoveOps = [RegOp.DeleteValue(SrpBase, "AppIdSvcStartType")],
                    DetectOps = [RegOp.CheckDword(SrpBase, "AppIdSvcStartType", 2)],
                },
                new TweakDef
                {
                    Id = "applocker-enable-exe-auditing",
                    Label = "Enable AppLocker EXE Execution Auditing",
                    Category = "Security — Application Restart",
                    Description = "Enables event log auditing for all AppLocker EXE allow and deny events for visibility without enforcement.",
                    Tags = ["applocker", "exe", "audit", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All executable allow/deny events written to AppLocker event log; baseline for building allow-list rules.",
                    ApplyOps = [RegOp.SetDword(SrpBase, "EnableCollectionLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrpBase, "EnableCollectionLogging")],
                    DetectOps = [RegOp.CheckDword(SrpBase, "EnableCollectionLogging", 1)],
                },
                new TweakDef
                {
                    Id = "applocker-block-user-rule-creation",
                    Label = "Block Standard Users from Creating AppLocker Exceptions",
                    Category = "Security — Application Restart",
                    Description = "Prevents standard (non-administrator) users from creating AppLocker exception rules or publisher overrides.",
                    Tags = ["applocker", "user-rules", "policy", "application-control", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only administrators can add AppLocker exceptions; prevents users from bypassing application control policy.",
                    ApplyOps = [RegOp.SetDword(SrpBase, "UsersCanCreateExceptions", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrpBase, "UsersCanCreateExceptions")],
                    DetectOps = [RegOp.CheckDword(SrpBase, "UsersCanCreateExceptions", 0)],
                },
                new TweakDef
                {
                    Id = "applocker-enable-performance-logging",
                    Label = "Enable AppLocker Performance Event Logging",
                    Category = "Security — Application Restart",
                    Description = "Enables detailed performance telemetry logging for AppLocker rule evaluations to the event log.",
                    Tags = ["applocker", "performance", "logging", "policy", "diagnostics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Logs AppLocker evaluation metrics; minor event log overhead; useful for tuning rule sets.",
                    ApplyOps = [RegOp.SetDword(SrpBase, "EnablePerformanceLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrpBase, "EnablePerformanceLogging")],
                    DetectOps = [RegOp.CheckDword(SrpBase, "EnablePerformanceLogging", 1)],
                },
            ];
    }

    // ── AppLockerWdac ──
    private static class _AppLockerWdac
    {
        private const string CIPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";
        private const string AplPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2";
        private const string WdacPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Policy Manager";
        private const string DevGuardPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";
        private const string AplAppx = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Appx";
        private const string SiPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy";
        private const string HvciPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "apl-enable-wdac-event-logging",
                Label = "Enable WDAC / Code Integrity Operational Event Log",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["wdac", "code integrity", "security", "audit", "event log"],
                Description =
                    "Enables the Microsoft-Windows-CodeIntegrity/Operational event channel. "
                    + "Records every signature validation decision (pass/fail) made by the kernel Code Integrity module. "
                    + "Essential for monitoring WDAC policy effectiveness and investigating block events.",
                ApplyOps =
                [
                    RegOp.SetDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-CodeIntegrity/Operational",
                        "Enabled",
                        1
                    ),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-CodeIntegrity/Operational",
                        "Enabled"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-CodeIntegrity/Operational",
                        "Enabled",
                        1
                    ),
                ],
            },
            new TweakDef
            {
                Id = "apl-block-vulnerable-driver-list",
                Label = "Enable Microsoft Vulnerable Driver Blocklist",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["wdac", "security", "driver", "blocklist", "kernel"],
                Description =
                    "Activates the Microsoft-maintained list of drivers with known kernel vulnerabilities. "
                    + "Prevents BYOVD (Bring Your Own Vulnerable Driver) attacks where attackers load known-buggy drivers to escalate privileges.",
                ApplyOps = [RegOp.SetDword(CIPol, "VulnerableDriverBlocklistEnable", 1)],
                RemoveOps = [RegOp.DeleteValue(CIPol, "VulnerableDriverBlocklistEnable")],
                DetectOps = [RegOp.CheckDword(CIPol, "VulnerableDriverBlocklistEnable", 1)],
            },
            new TweakDef
            {
                Id = "apl-enable-smart-app-control-policy",
                Label = "Enable Smart App Control in Evaluate Mode",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = false,
                Tags = ["wdac", "smart app control", "security", "application control"],
                Description =
                    "Sets Smart App Control to evaluation mode (2) which silently evaluates unsigned apps without blocking. "
                    + "Allows learning phase before switching to enforcement. Only applicable on fresh Windows 11 installs.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy", "VerifiedAndReputablePolicyState", 2)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy", "VerifiedAndReputablePolicyState")],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy", "VerifiedAndReputablePolicyState", 2),
                ],
            },
            new TweakDef
            {
                Id = "apl-enable-hvci-strict",
                Label = "Enable Hypervisor-Protected Code Integrity (HVCI) Strict Mode",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["wdac", "hvci", "security", "hypervisor", "kernel"],
                Description =
                    "Enables HVCI (Memory Integrity) in strict mode. Kernel-mode code must be signed and validated "
                    + "by the hypervisor before execution, preventing unsigned kernel-mode rootkits and driver exploits.",
                ApplyOps =
                [
                    RegOp.SetDword(HvciPol, "EnableVirtualizationBasedSecurity", 1),
                    RegOp.SetDword(HvciPol, "HypervisorEnforcedCodeIntegrity", 2),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(HvciPol, "EnableVirtualizationBasedSecurity"),
                    RegOp.DeleteValue(HvciPol, "HypervisorEnforcedCodeIntegrity"),
                ],
                DetectOps = [RegOp.CheckDword(HvciPol, "HypervisorEnforcedCodeIntegrity", 2)],
            },
            new TweakDef
            {
                Id = "apl-enable-credential-guard",
                Label = "Enable Windows Credential Guard",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["credential guard", "security", "lsa", "pass-the-hash", "vbs"],
                Description =
                    "Enables Credential Guard which stores NTLM hashes and Kerberos tickets in a VBS-protected enclave, "
                    + "preventing pass-the-hash and pass-the-ticket attacks even if the OS kernel is compromised.",
                ApplyOps = [RegOp.SetDword(DevGuardPol, "EnableVirtualizationBasedSecurity", 1), RegOp.SetDword(DevGuardPol, "LsaCfgFlags", 1)],
                RemoveOps = [RegOp.DeleteValue(DevGuardPol, "EnableVirtualizationBasedSecurity"), RegOp.DeleteValue(DevGuardPol, "LsaCfgFlags")],
                DetectOps = [RegOp.CheckDword(DevGuardPol, "LsaCfgFlags", 1)],
            },
            new TweakDef
            {
                Id = "apl-block-ms-store-unsigned-apps",
                Label = "Block Unsigned Apps from Non-Store Sources (GPO)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["applocker", "security", "unsigned", "policy", "gpo"],
                Description =
                    "Uses a Group Policy path to enable SmartScreen enforcement mode, requiring apps from non-Store sources "
                    + "to pass reputation check before execution. Complementary to AppLocker rules.",
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 2),
                    RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "ShellSmartScreenLevel", "Block"),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "ShellSmartScreenLevel"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 2)],
            },
            new TweakDef
            {
                Id = "apl-disable-auto-play-allowlisting",
                Label = "Disable AutoPlay for Non-Listed Devices",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["applocker", "security", "autoplay", "usb", "policy"],
                Description =
                    "Prevents AutoPlay from running executables on removable media that aren't in the trusted device list. "
                    + "Stops USB-based malware from auto-executing when inserted.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoplayfornonVolume", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoplayfornonVolume")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoplayfornonVolume", 1)],
            },
            new TweakDef
            {
                Id = "apl-enable-lsa-protected-process",
                Label = "Enable LSA Protected Process Light (PPL)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["lsa", "security", "credential", "protected process", "anti-tamper"],
                Description =
                    "Runs the Local Security Authority (lsass.exe) as a Protected Process Light. "
                    + "Prevents unauthorized processes (including admin-level tools like Mimikatz) from reading memory of the credential store.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1)],
            },
            new TweakDef
            {
                Id = "apl-disable-office-macro-execution",
                Label = "Block Office Macro Execution from Internet-Origin Files",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["office", "macro", "security", "applocker", "wdac", "policy"],
                Description =
                    "Applies Group Policy to block VBA macros in Office documents downloaded from the internet. "
                    + "Closes one of the most common malware delivery vectors (phishing attachments with malicious macros).",
                ApplyOps =
                [
                    RegOp.SetDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\excel\security",
                        "blockcontentexecutionfrominternet",
                        1
                    ),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\excel\security",
                        "blockcontentexecutionfrominternet"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\excel\security",
                        "blockcontentexecutionfrominternet",
                        1
                    ),
                ],
            },
        ];
    }

    // ── AppPermissions ──
    private static class _AppPermissions
    {
        private const string AppPrivacy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "aperm-deny-camera-access",
                Label = "Block All Apps from Accessing Camera (GPO)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["camera", "privacy", "app permissions", "gpo"],
                Description =
                    "Enforces machine-wide policy denying all apps access to the camera. "
                    + "LetAppsAccessCamera=2. Overrides per-app user consent settings. "
                    + "Useful on shared or high-security PCs where camera access should be blocked.",
                ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessCamera", 2)],
                RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessCamera")],
                DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessCamera", 2)],
            },
            new TweakDef
            {
                Id = "aperm-deny-microphone-access",
                Label = "Block All Apps from Accessing Microphone (GPO)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["microphone", "privacy", "app permissions", "gpo"],
                Description =
                    "Enforces machine-wide policy denying all apps access to the microphone. "
                    + "LetAppsAccessMicrophone=2. Prevents unauthorized recording even if a "
                    + "user accidentally grants app permission.",
                ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessMicrophone", 2)],
                RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessMicrophone")],
                DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessMicrophone", 2)],
            },
            new TweakDef
            {
                Id = "aperm-deny-location-access",
                Label = "Block All Apps from Accessing Location (GPO)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["location", "gps", "privacy", "app permissions", "gpo"],
                Description =
                    "Enforces machine-wide policy denying all apps access to the location service. "
                    + "LetAppsAccessLocation=2. Prevents location-based tracking by any app "
                    + "regardless of per-app user consent.",
                ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessLocation", 2)],
                RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessLocation")],
                DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessLocation", 2)],
            },
            new TweakDef
            {
                Id = "aperm-deny-contacts-access",
                Label = "Block All Apps from Accessing Contacts (GPO)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["contacts", "privacy", "app permissions", "gpo"],
                Description =
                    "Enforces machine-wide policy denying all apps access to the contacts/address "
                    + "book. LetAppsAccessContacts=2. Prevents apps from harvesting contact data.",
                ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessContacts", 2)],
                RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessContacts")],
                DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessContacts", 2)],
            },
            new TweakDef
            {
                Id = "aperm-deny-calendar-access",
                Label = "Block All Apps from Accessing Calendar (GPO)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["calendar", "privacy", "app permissions", "gpo"],
                Description =
                    "Enforces machine-wide policy denying all apps access to the calendar. "
                    + "LetAppsAccessCalendar=2. Prevents scheduled-meeting and appointment "
                    + "data from being read by third-party apps.",
                ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessCalendar", 2)],
                RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessCalendar")],
                DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessCalendar", 2)],
            },
            new TweakDef
            {
                Id = "aperm-deny-call-history-access",
                Label = "Block All Apps from Accessing Call History (GPO)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["call history", "privacy", "app permissions", "phone", "gpo"],
                Description =
                    "Enforces machine-wide policy denying all apps access to call history. "
                    + "LetAppsAccessCallHistory=2. Prevents phone-linked apps from reading "
                    + "incoming/outgoing call logs.",
                ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessCallHistory", 2)],
                RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessCallHistory")],
                DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessCallHistory", 2)],
            },
            new TweakDef
            {
                Id = "aperm-deny-email-access",
                Label = "Block All Apps from Accessing Email (GPO)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["email", "privacy", "app permissions", "gpo"],
                Description =
                    "Enforces machine-wide policy denying all apps access to the user's email. "
                    + "LetAppsAccessEmail=2. Prevents third-party apps from reading email content "
                    + "via the Windows email capability.",
                ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessEmail", 2)],
                RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessEmail")],
                DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessEmail", 2)],
            },
            new TweakDef
            {
                Id = "aperm-deny-documents-access",
                Label = "Block All Apps from Accessing Documents Library (GPO)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["documents", "privacy", "app permissions", "gpo", "files"],
                Description =
                    "Enforces machine-wide policy denying all apps broad access to the Documents "
                    + "library. LetAppsAccessDocumentsLibrary=2. Note: apps may still access files "
                    + "via the file-picker dialog; this only blocks broad programmatic access.",
                ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessDocumentsLibrary", 2)],
                RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessDocumentsLibrary")],
                DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessDocumentsLibrary", 2)],
            },
            new TweakDef
            {
                Id = "aperm-deny-diagnostic-info",
                Label = "Block Apps from Accessing Diagnostic Information (GPO)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["diagnostics", "privacy", "app permissions", "gpo", "telemetry"],
                Description =
                    "Enforces machine-wide policy denying all apps access to diagnostic information "
                    + "(app list, battery status, usage data). LetAppsGetDiagnosticInfo=2. "
                    + "Reduces the amount of device metadata apps can harvest for fingerprinting.",
                ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsGetDiagnosticInfo", 2)],
                RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsGetDiagnosticInfo")],
                DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsGetDiagnosticInfo", 2)],
            },
            new TweakDef
            {
                Id = "aperm-deny-radio-access",
                Label = "Block All Apps from Controlling Radios (GPO)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["radio", "bluetooth", "wifi", "privacy", "app permissions", "gpo"],
                Description =
                    "Enforces machine-wide policy denying all apps the ability to turn on/off "
                    + "radios (Wi-Fi, Bluetooth, NFC). LetAppsAccessRadios=2. Prevents rogue apps "
                    + "from toggling wireless interfaces without user knowledge.",
                ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessRadios", 2)],
                RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessRadios")],
                DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessRadios", 2)],
            },
        ];
    }

    // ── AppPrivacyPolicy ──
    private static class _AppPrivacyPolicy
    {
        private const string Policy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appp-deny-notifications",
                Label = "Policy: Force-Deny All UWP Apps Notification Access",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Prevents all UWP apps from reading notification content across devices. "
                    + "LetAppsAccessNotifications=2. Toast/lock-screen notifications still fire;  "
                    + "cross-device notification mirroring is blocked.",
                Tags = ["notifications", "policy", "app privacy", "uwp", "privacy"],
                RegistryKeys = [Policy],
                ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessNotifications", 2)],
                RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessNotifications")],
                DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessNotifications", 2)],
            },
            new TweakDef
            {
                Id = "appp-deny-account-info",
                Label = "Policy: Force-Deny All UWP Apps Account Information Access",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Blocks all UWP apps from reading account information (name, picture, "
                    + "username) at the machine policy level. LetAppsAccessAccountInfo=2.",
                Tags = ["account info", "policy", "app privacy", "uwp", "privacy"],
                RegistryKeys = [Policy],
                ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessAccountInfo", 2)],
                RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessAccountInfo")],
                DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessAccountInfo", 2)],
            },
            new TweakDef
            {
                Id = "appp-deny-device-sync",
                Label = "Policy: Deny UWP Apps Near-Device Sync (Bluetooth/NFC)",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Prevents UWP apps from syncing with nearby devices via Bluetooth, "
                    + "NFC, or other proximity technologies. LetAppsSyncWithDevices=2.",
                Tags = ["sync", "bluetooth", "nfc", "policy", "app privacy"],
                RegistryKeys = [Policy],
                ApplyOps = [RegOp.SetDword(Policy, "LetAppsSyncWithDevices", 2)],
                RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsSyncWithDevices")],
                DetectOps = [RegOp.CheckDword(Policy, "LetAppsSyncWithDevices", 2)],
            },
            new TweakDef
            {
                Id = "appp-deny-phone-calls",
                Label = "Policy: Force-Deny All UWP Apps Phone Call Access",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description = "Blocks all UWP apps from accessing the phone/dialer at the machine level. " + "LetAppsAccessPhone=2.",
                Tags = ["phone", "calls", "policy", "app privacy", "uwp"],
                RegistryKeys = [Policy],
                ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessPhone", 2)],
                RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessPhone")],
                DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessPhone", 2)],
            },
            new TweakDef
            {
                Id = "appp-deny-tasks",
                Label = "Policy: Force-Deny All UWP Apps Task List Access",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Prevents all UWP apps from reading or writing to the system task list "
                    + "(Cortana reminders, to-do lists). LetAppsAccessTasks=2.",
                Tags = ["tasks", "to-do", "policy", "app privacy", "uwp"],
                RegistryKeys = [Policy],
                ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessTasks", 2)],
                RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessTasks")],
                DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessTasks", 2)],
            },
            new TweakDef
            {
                Id = "appp-deny-messaging",
                Label = "Policy: Force-Deny All UWP Apps SMS / Messaging Access",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description = "Blocks all UWP apps from sending or reading SMS and MMS messages at " + "the machine level. LetAppsAccessMessaging=2.",
                Tags = ["sms", "messaging", "policy", "app privacy", "uwp"],
                RegistryKeys = [Policy],
                ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessMessaging", 2)],
                RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessMessaging")],
                DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessMessaging", 2)],
            },
            new TweakDef
            {
                Id = "appp-deny-video-library",
                Label = "Policy: Force-Deny All UWP Apps Video Library Access",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description = "Machine policy denying all UWP apps access to the user's Video library " + "folder. LetAppsAccessVideoLibrary=2.",
                Tags = ["video", "library", "files", "policy", "app privacy", "uwp"],
                RegistryKeys = [Policy],
                ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessVideoLibrary", 2)],
                RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessVideoLibrary")],
                DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessVideoLibrary", 2)],
            },
        ];
    }

    // ── AppPrivacyPolicyAdv ──
    private static class _AppPrivacyPolicyAdv
    {
        private const string Policy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appprv2-deny-trusted-devices",
                Label = "App Privacy: Block all UWP apps from accessing trusted devices",
                Category = "Security — Application Restart",
                Description =
                    "Sets LetAppsAccessTrustedDevices=2 in AppPrivacy policy. Prevents all UWP apps from "
                    + "communicating with previously paired/trusted Bluetooth and USB devices.",
                Tags = ["privacy", "trusted-devices", "bluetooth", "usb", "app-privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessTrustedDevices", 2)],
                RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessTrustedDevices")],
                DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessTrustedDevices", 2)],
            },
            new TweakDef
            {
                Id = "appprv2-deny-gaze-input",
                Label = "App Privacy: Block all UWP apps from accessing gaze/eye-tracking input",
                Category = "Security — Application Restart",
                Description =
                    "Sets LetAppsAccessGazeInput=2 in AppPrivacy policy. Prevents all UWP apps from "
                    + "reading gaze or eye-tracking data from supported hardware at machine policy level.",
                Tags = ["privacy", "gaze", "eye-tracking", "input", "app-privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessGazeInput", 2)],
                RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessGazeInput")],
                DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessGazeInput", 2)],
            },
            new TweakDef
            {
                Id = "appprv2-deny-activate-with-voice",
                Label = "App Privacy: Block all UWP apps from background voice activation",
                Category = "Security — Application Restart",
                Description =
                    "Sets LetAppsActivateWithVoice=2 in AppPrivacy policy. Prevents all UWP apps from "
                    + "using wake-word / voice activation to start from a background or suspended state.",
                Tags = ["privacy", "voice", "activation", "background", "app-privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Policy, "LetAppsActivateWithVoice", 2)],
                RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsActivateWithVoice")],
                DetectOps = [RegOp.CheckDword(Policy, "LetAppsActivateWithVoice", 2)],
            },
            new TweakDef
            {
                Id = "appprv2-deny-activate-with-voice-above-lock",
                Label = "App Privacy: Block voice activation above the lock screen",
                Category = "Security — Application Restart",
                Description =
                    "Sets LetAppsActivateWithVoiceAboveLock=2 in AppPrivacy policy. Prevents all UWP apps "
                    + "from responding to wake-word voice commands when the device is locked.",
                Tags = ["privacy", "voice", "activation", "lock-screen", "app-privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Policy, "LetAppsActivateWithVoiceAboveLock", 2)],
                RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsActivateWithVoiceAboveLock")],
                DetectOps = [RegOp.CheckDword(Policy, "LetAppsActivateWithVoiceAboveLock", 2)],
            },
        ];
    }

    // ── AppReadinessPolicy ──
    private static class _AppReadinessPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppReadiness";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "apprdy-disable-service",
                Label = "Disable App Readiness Service",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The Windows App Readiness service prepares applications for first use after installation or user sign-in. Disabling this policy prevents the service from executing post-install configuration tasks during logon. This reduces CPU and I/O activity associated with first-run application setup, improving login responsiveness. The setting is most beneficial on enterprise systems with pre-configured application images. Consumer scenarios where users regularly install new Store applications may experience delayed first launches. No impact on application functionality once an application has completed its initial setup phase.",
                Tags = ["app-readiness", "startup", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAppReadiness", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAppReadiness")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAppReadiness", 1)],
            },
            new TweakDef
            {
                Id = "apprdy-disable-logging",
                Label = "Disable App Readiness Logging",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "App Readiness diagnostic logging captures detailed information about application preparation activities to event logs. Disabling this policy suppresses verbose logging from the App Readiness subsystem, reducing event log volume. Log data generated by this service is rarely consulted in enterprise environments with stable application stacks. Suppressing these events reduces noise in operational monitoring dashboards and SIEM pipelines. The diagnostic value of App Readiness logs is limited outside of application deployment troubleshooting scenarios. Removing this telemetry has no impact on application functionality or system stability.",
                Tags = ["app-readiness", "logging", "telemetry", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAppReadinessLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAppReadinessLogging")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAppReadinessLogging", 1)],
            },
            new TweakDef
            {
                Id = "apprdy-disable-prelaunch",
                Label = "Disable App Pre-Launch",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Windows pre-launches applications in the background to reduce perceived startup time for users. The pre-launch mechanism consumes memory and CPU resources before the user has actually requested the application. Disabling pre-launch ensures applications only start when explicitly requested by the user, freeing system resources for foreground tasks. On systems with limited RAM, pre-launched applications can cause memory pressure and paging activity. Enterprise workstations benefit from having resources dedicated to active business applications rather than speculative pre-launched apps. This setting is recommended for environments where resource predictability is more important than app launch latency.",
                Tags = ["app-readiness", "prelaunch", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrelaunch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrelaunch")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrelaunch", 1)],
            },
            new TweakDef
            {
                Id = "apprdy-disable-prefetch",
                Label = "Disable App Readiness Prefetch",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "App Readiness prefetch proactively loads application binaries and data into memory based on predicted usage patterns. This process runs during system idle periods and consumes I/O throughput that could otherwise be used by foreground workloads. Disabling policy-controlled prefetch gives administrators explicit control over memory residency of application components. On SSDs with low read latency, the benefit of prefetch is diminished and disabling it reduces unnecessary I/O wear. Servers and RDS hosts benefit from disabling prefetch to prevent memory bloat from unpredictable application load patterns. Applications continue to function normally with their standard operating system loader mechanisms.",
                Tags = ["app-readiness", "prefetch", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrefetch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrefetch")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrefetch", 1)],
            },
            new TweakDef
            {
                Id = "apprdy-disable-telemetry",
                Label = "Disable App Readiness Telemetry",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "App Readiness telemetry transmits usage metrics and diagnostic information about application preparation activities to Microsoft. This data includes timing information, failure rates, and application identifiers associated with the readiness process. Disabling this telemetry prevents application usage patterns from being transmitted outside the enterprise boundary. Privacy-sensitive organizations and regulated industries have compliance obligations to minimize telemetry data flows. The telemetry collection has no bearing on local application functionality or performance. Administrators can maintain equivalent diagnostic capability through internal logging and monitoring tools.",
                Tags = ["app-readiness", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAppReadinessTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAppReadinessTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAppReadinessTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "apprdy-disable-readiness-score",
                Label = "Disable App Readiness Score",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Windows computes application readiness scores to prioritize which applications should be prepared first during sign-in. The scoring algorithm consumes CPU cycles evaluating application state and historical usage patterns. Disabling the readiness score calculation reduces CPU overhead associated with sign-in processing. Enterprise environments with standardized application sets and predictable launch orders benefit from eliminating this adaptive algorithm. The scoring feature is primarily designed to optimize cold-start performance on consumer devices with diverse application ecosystems. Disabling it has no meaningful impact on application availability or usability in managed environments.",
                Tags = ["app-readiness", "startup", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableReadinessScore", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableReadinessScore")],
                DetectOps = [RegOp.CheckDword(Key, "DisableReadinessScore", 1)],
            },
            new TweakDef
            {
                Id = "apprdy-zero-max-wait",
                Label = "Set Max App Readiness Wait to Zero",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "The maximum wait time controls how long Windows delays sign-in completion while awaiting application readiness tasks. Setting this value to zero prevents any delay in sign-in completion due to App Readiness processing. This ensures that users can access their desktop immediately without waiting for speculative application preparation. Enterprise environments with fast SSD storage and pre-staged applications benefit most from eliminating this wait. Reducing sign-in latency improves user satisfaction and productivity on shared or hot-desk workstations. Application functionality is unaffected because apps load on demand when actually launched by the user.",
                Tags = ["app-readiness", "logon", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxWaitSeconds", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxWaitSeconds")],
                DetectOps = [RegOp.CheckDword(Key, "MaxWaitSeconds", 0)],
            },
            new TweakDef
            {
                Id = "apprdy-disable-first-signin-animation",
                Label = "Disable First Sign-In Animation",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Windows displays a first-run welcome animation when a user signs in for the first time, presenting tips about the operating system. This animation delays desktop availability by several seconds and is unnecessary in enterprise environments where users receive training separately. Disabling the first sign-in animation skips the welcome screen and brings users directly to their desktop. Provisioned systems configured for immediate productivity benefit from removing this consumer-oriented onboarding experience. MDM and Group Policy deployments typically disable this feature as a standard hardening step. No functionality is removed by suppressing this animation.",
                Tags = ["app-readiness", "logon", "oobe", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFirstSigninAnimation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFirstSigninAnimation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFirstSigninAnimation", 1)],
            },
            new TweakDef
            {
                Id = "apprdy-disable-registration-prompt",
                Label = "Disable App Registration Prompt",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Windows may prompt users to register applications during first sign-in or after updates to handle file type associations and protocol handlers. These prompts interrupt productivity workflows and generate confusion in enterprise environments with managed application policies. Disabling the app registration prompt prevents these dialogs from appearing during or after the sign-in sequence. Application associations are typically managed through Group Policy or MDM configurations in enterprise deployments. Administrators maintain full control over file type and protocol handler assignments without user prompts. This setting is a standard component of enterprise desktop imaging and provisioning workflows.",
                Tags = ["app-readiness", "applications", "oobe", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAppRegistrationPrompt", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAppRegistrationPrompt")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAppRegistrationPrompt", 1)],
            },
            new TweakDef
            {
                Id = "apprdy-disable-default-apps-choice",
                Label = "Disable Default Apps Choice Screen",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Windows presents a default apps choice screen to new users asking them to select preferred applications for common tasks such as web browsing and email. This screen is irrelevant in enterprise environments where default application associations are centrally managed. Disabling this policy prevents the default apps choice dialog from appearing at any point during the user session. Enterprise deployments benefit from deterministic application association policies without user intervention. Managed default associations ensure consistent user experiences and prevent unsanctioned applications from being set as defaults. Application association management remains fully available to administrators through centralized policy mechanisms.",
                Tags = ["app-readiness", "default-apps", "oobe", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDefaultAppsChoice", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDefaultAppsChoice")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDefaultAppsChoice", 1)],
            },
        ];
    }

    // ── AppSiloAdvPolicy ──
    private static class _AppSiloAdvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppSiloAdvanced";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appsilob-enforce-ipc-isolation-between-silos",
                Label = "Enforce Inter-Process Communication Isolation Between Application Silos",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Enforcing IPC isolation between application silos prevents applications running in separate silos from communicating through unnamed pipes shared memory and other inter-process communication channels that could be used to exchange unauthorized data or coordinate cross-silo attacks. App silos are designed to create isolation boundaries between applications and IPC channels that cross silo boundaries undermine the isolation guarantee. Applications with legitimate requirements to communicate across silo boundaries should use explicitly declared and audited IPC channels that are subject to appropriate authorization checks. Cross-silo IPC attempts that are blocked by isolation enforcement should generate audit events for security analysis. Silo-aware applications should be designed to use silo-isolated IPC mechanisms rather than system-wide named objects that are visible across all silos. Organizations should review application compatibility testing for IPC isolation policies before deployment to ensure that approved cross-silo communication patterns are correctly exempted.",
                Tags = ["app-silo", "ipc-isolation", "inter-process", "application-isolation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceIPCIsolationBetweenSilos", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceIPCIsolationBetweenSilos")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceIPCIsolationBetweenSilos", 1)],
            },
            new TweakDef
            {
                Id = "appsilob-restrict-silo-exit-control-flow",
                Label = "Restrict Control Flow Exits from Application Silo to Host Environment",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Restricting control flow exits from application silos prevents applications running in silos from executing code injection or DLL injection attacks that target processes running in the host environment outside the silo boundary. Control flow exit restrictions ensure that execution cannot jump from silo-isolated application code into host system code or other silo code through return-oriented programming or code injection techniques. Kernel-enforced control flow restrictions complement user-mode silo enforcement to prevent privilege escalation through exploiting the boundary between silo and host code execution contexts. Applications that require tight integration with host OS features should use approved well-defined API channels rather than direct code execution in host contexts. Control flow integrity enforcement within silos should be combined with host-to-silo boundary checks to create a bidirectional enforcement model. Security testing for silo control flow restrictions should include red team testing of known control flow substitution techniques to validate the effectiveness of enforcement.",
                Tags = ["app-silo", "control-flow", "code-injection", "silo-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSiloExitControlFlow", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSiloExitControlFlow")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSiloExitControlFlow", 1)],
            },
            new TweakDef
            {
                Id = "appsilob-enable-silo-resource-monitoring",
                Label = "Enable Advanced Resource Usage Monitoring for Application Silo Environments",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Advanced resource monitoring for application silos tracks CPU memory disk and network resource consumption at the silo level providing visibility into anomalous resource use that may indicate malicious activity within a silo. Silos that consume abnormally high resources may be running cryptomining workloads or performing intensive data processing operations not consistent with the silo's declared workload. Resource monitoring data for silos should be integrated with behavioral analytics to establish baselines and detect deviations that warrant investigation. Memory consumption anomalies within silos can indicate buffer overflow exploitation or excessive memory mapping that suggests vulnerability exploitation is in progress. CPU spike analysis within silo contexts can identify cryptomining or computation-intensive malware execution that falls below whole-system thresholds but exceeds per-silo baselines. Silo resource monitoring should be collected with sufficient granularity and retention to support incident investigation when anomalies are detected.",
                Tags = ["app-silo", "resource-monitoring", "behavioral-analytics", "anomaly-detection", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSiloResourceMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSiloResourceMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSiloResourceMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "appsilob-enforce-silo-network-namespace",
                Label = "Enforce Dedicated Network Namespace Isolation for Application Silos",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Enforcing dedicated network namespace isolation for application silos prevents applications within silos from directly accessing network resources visible to the host system or other silos without explicit authorization through defined network paths. Network namespace isolation ensures that silo applications cannot enumerate or access network resources outside their defined scope preventing lateral movement from a compromised silo to other network hosts. Silos with internet-facing components should be isolated in network namespaces that have controlled internet access policies preventing them from accessing internal network resources if compromised. Network traffic entering and exiting silo network namespaces should be inspected by network security controls implemented at the namespace boundary. Silo-specific firewall rules should ensure that network connections are limited to the endpoints required for the silo application's function. Monitoring of cross-namespace network traffic allows detection of silo breakout attempts that try to establish connections to unauthorized network destinations.",
                Tags = ["app-silo", "network-namespace", "network-isolation", "lateral-movement", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceSiloNetworkNamespace", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceSiloNetworkNamespace")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceSiloNetworkNamespace", 1)],
            },
            new TweakDef
            {
                Id = "appsilob-block-silo-kernel-object-access",
                Label = "Block Application Silo Access to Unauthorized Kernel Objects",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Blocking silo access to kernel objects outside the silo's authorized namespace prevents applications from reading or modifying kernel objects owned by the host or other silos which could be used to manipulate system behavior or extract sensitive information. Kernel objects including events mutexes semaphores and named pipes that cross the silo boundary create potential channels for silo breakout if their security descriptors are not appropriately restrictive. Access to kernel objects in the global namespace should be blocked for silo applications with only silo-local namespace objects accessible by default. Vulnerability classes like kernel object abuse become significantly more difficult to exploit when applications within silos cannot access the kernel objects that would normally be targets. Kernel object access control should be audited to detect silo applications attempting to access kernel objects outside their authorized scope. Exceptions for approved cross-silo kernel object access should be explicitly defined and reviewed as part of the silo security architecture.",
                Tags = ["app-silo", "kernel-objects", "namespace-isolation", "object-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockSiloKernelObjectAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockSiloKernelObjectAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockSiloKernelObjectAccess", 1)],
            },
            new TweakDef
            {
                Id = "appsilob-restrict-silo-registry-scope",
                Label = "Restrict Application Silo Registry Access to Silo-Scoped Registry Namespace",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Restricting silo registry access to the silo-scoped registry namespace prevents applications running in silos from reading or modifying host registry keys that control system configuration security settings and credentials. Registry key access from silos to host registry paths including HKLM system configuration keys and HKCU user preference keys should require explicit authorization. Malicious applications running in silos can use unrestricted registry access to read sensitive configuration data including stored credentials encryption keys and security policy settings. Silo registry namespace restrictions ensure that each silo has an isolated view of registry state preventing cross-silo information disclosure through shared registry key access. Changes to registry keys in the silo registry should not persist to the host registry unless an explicit registry redirection policy allows the specific key path. Audit events for silo registry access attempts outside the authorized scope should be reviewed to detect applications attempting to read host configuration data.",
                Tags = ["app-silo", "registry-isolation", "silo-namespace", "configuration-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSiloRegistryScope", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSiloRegistryScope")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSiloRegistryScope", 1)],
            },
            new TweakDef
            {
                Id = "appsilob-enforce-silo-security-event-logging",
                Label = "Enforce Security Event Logging for Application Silo Policy Violations",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enforcing security event logging for silo policy violations ensures that all attempts to violate silo isolation boundaries including unauthorized IPC attempts registry access requests and network namespace violations are captured in the security event log. Silo violation events provide intelligence about activities within silos that may indicate malware execution attempting to break out of containment or explore the host environment. Security event logging for silo violations should use a consistent event schema that includes silo identifier violating process identity violation type and targeted resource to support effective investigation. Silo security events should be forwarded to centralized SIEM alongside endpoint detection and response telemetry for correlation with broader security event patterns. High-volume silo violation events may indicate active exploitation attempts and should trigger elevated security monitoring responses. Retention of silo security events should match the organizational security audit log retention policy rather than defaulting to shorter operational log retention periods.",
                Tags = ["app-silo", "security-events", "audit-logging", "compliance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceSiloSecurityEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceSiloSecurityEventLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceSiloSecurityEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "appsilob-block-cross-silo-token-inheritance",
                Label = "Block Security Token Inheritance Across Application Silo Boundaries",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Blocking security token inheritance across silo boundaries prevents applications that spawn processes across silo boundaries from passing elevated or privileged security tokens to processes running in other silos that should have different security contexts. Cross-silo token inheritance creates privilege escalation paths where a process running in a restricted silo could gain the privileges of a token inherited from a higher-privileged process in another silo. Token inheritance blocking ensures that processes created in a silo always have tokens appropriate to that silo's security context rather than inheriting tokens from callers outside the silo. Process creation across silo boundaries should require explicit privilege assignment through the silo security policy rather than automatic token inheritance. Applications that legitimately need to create processes in other silos with specific security contexts should use the approved silo process creation API with explicit token specification. Security testing for cross-silo token inheritance should verify that token stripping is enforced consistently across all process creation mechanisms.",
                Tags = ["app-silo", "token-inheritance", "privilege-control", "cross-silo", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockCrossSiloTokenInheritance", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockCrossSiloTokenInheritance")],
                DetectOps = [RegOp.CheckDword(Key, "BlockCrossSiloTokenInheritance", 1)],
            },
            new TweakDef
            {
                Id = "appsilob-restrict-silo-debug-capabilities",
                Label = "Restrict Debugger Attachment Capabilities Within Application Silo Environments",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting debugger attachment within silo environments prevents applications running inside silos from attaching debuggers to other processes within the same silo which could be used for unauthorized memory inspection and code injection. Debugger access to silo processes should require explicit authorization through a defined diagnostic access policy rather than being available to all processes within the silo by default. Developer workflows that require debugging silo-hosted applications should use approved debugging infrastructure that authenticates the developer identity and logs debugging sessions. Attaching debuggers to silo processes without restriction allows any process within the silo to inspect and modify the memory of other silo processes which could extract sensitive data processed by those processes. Debug access restrictions should be enforced at the system call level to prevent circumvention through low-level debugging APIs. Production silo environments should have stricter debug access restrictions than development environments to protect sensitive workloads from unauthorized inspection.",
                Tags = ["app-silo", "debug-restriction", "memory-protection", "silo-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSiloDebugCapabilities", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSiloDebugCapabilities")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSiloDebugCapabilities", 1)],
            },
            new TweakDef
            {
                Id = "appsilob-enforce-silo-identity-isolation",
                Label = "Enforce Identity Isolation to Prevent Cross-Silo Identity Context Sharing",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Identity isolation enforcement ensures that authentication contexts credentials and identity tokens within one application silo cannot be accessed or impersonated by code running in other silos preventing cross-silo credential theft. Application silos that process authentication may cache tokens or credentials in memory structures that are accessible within the silo and identity isolation prevents these from being visible across silo boundaries. Credential isolation within silos is complementary to Windows Credential Guard but operates at finer granularity ensuring that even within an administrator security context cross-silo credential access is blocked. Authentication operations within silos should use silo-scoped credential stores rather than shared system credential stores to maintain isolation guarantees. Identity events from silo authentication operations should be logged distinctly from host authentication events to provide clear attribution of authentication activity to specific silo contexts. Security reviews should verify that identity isolation policies are correctly configured for all production silo deployments.",
                Tags = ["app-silo", "identity-isolation", "credential-protection", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceSiloIdentityIsolation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceSiloIdentityIsolation")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceSiloIdentityIsolation", 1)],
            },
        ];
    }

    // ── AppSiloPolicy ──
    private static class _AppSiloPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppSilo";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appsiloa-enable-silo-isolation",
                Label = "Enable App Silo Process Isolation for Privileged Process Containers",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "App Silo provides process isolation through Windows container primitives creating secure execution environments that limit the blast radius of application vulnerabilities. Enabling App Silo isolation restricts processes to their assigned containers preventing cross-process access to sensitive system resources and other application data. Application silos use Windows isolated namespaces and ACLs to prevent silo processes from accessing objects outside their container boundary. Process containment through silos limits the effectiveness of memory-based attacks like heap sprays and use-after-free vulnerabilities by preventing access to objects in other containers. Organizations should evaluate App Silo isolation for high-risk applications like browser components email clients and document processors that commonly execute untrusted content. Silo-isolated processes have reduced access to the system providing defense-in-depth against application exploitation.",
                Tags = ["app-silo", "isolation", "container", "process-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSiloIsolation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSiloIsolation")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSiloIsolation", 1)],
            },
            new TweakDef
            {
                Id = "appsiloa-restrict-silo-network-access",
                Label = "Restrict Silo Network Access to Allowlisted Endpoints",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "App Silo network access restriction limits the network endpoints that isolated processes can communicate with preventing data exfiltration through unauthorized network connections. Restricting silo network access to allowlisted endpoints contains the damage from application compromise by preventing the compromised process from reaching attacker-controlled infrastructure. Network isolation for application silos is implemented through Windows Filtering Platform rules that block silo processes from accessing unauthorized network destinations. Allowlist-based network access provides more precise control than broad network restrictions ensuring that legitimate application functionality is not impaired. Organizations should define network access allowlists based on the specific network endpoints each application requires for its legitimate business function. Monitoring for silo processes attempting to access blocked network destinations provides detection for application exploitation attempts.",
                Tags = ["app-silo", "network-isolation", "allowlist", "exfiltration-prevention", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSiloNetworkAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSiloNetworkAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSiloNetworkAccess", 1)],
            },
            new TweakDef
            {
                Id = "appsiloa-block-silo-registry-writes",
                Label = "Block Silo Processes from Writing to System Registry Hives",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Silo registry isolation prevents processes inside application silos from writing to system registry hives outside their container virtual registry namespace. Blocking silo registry writes prevents application exploits from achieving persistence through registry modifications or compromising system configuration. Registry virtualization within silos allows applications to read system registry keys while writes are redirected to a per-silo virtual registry that does not affect the system. Applications that legitimately need to write to system registry locations must have those specific requirements accommodated through policy exceptions. Silo registry isolation is particularly effective at preventing malware injected into silo processes from establishing persistence through standard registry-based persistence mechanisms. Monitoring for attempt to bypass silo registry restrictions provides detection for advanced attackers who attempt to escape the container.",
                Tags = ["app-silo", "registry-isolation", "persistence-prevention", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockSystemRegistryWrites", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockSystemRegistryWrites")],
                DetectOps = [RegOp.CheckDword(Key, "BlockSystemRegistryWrites", 1)],
            },
            new TweakDef
            {
                Id = "appsiloa-restrict-silo-filesystem-access",
                Label = "Restrict Silo Processes to Designated File System Namespaces",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "File system namespace isolation for application silos restricts silo processes to only accessing file system locations within their designated namespace preventing access to sensitive system files. Restricting file system access contains the impact of application compromise by preventing exploited processes from reading sensitive system data or user files outside their container. File system isolation uses Windows NTFS ACLs and namespace virtualization to provide silo processes with a view of only the file system locations they need. Applications that access user documents will have appropriate user namespace access while being blocked from accessing system directories and other users' files. File system isolation is particularly effective against path traversal and directory traversal attacks that attempt to access files outside the application's intended scope. Organizations should test file system restrictions with each silo-enabled application to verify that legitimate file access requirements are met.",
                Tags = ["app-silo", "filesystem-isolation", "namespace", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictFileSystemNamespace", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictFileSystemNamespace")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictFileSystemNamespace", 1)],
            },
            new TweakDef
            {
                Id = "appsiloa-enable-silo-audit-logging",
                Label = "Enable Audit Logging for App Silo Isolation Events",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "App Silo audit logging captures events related to silo creation termination and security boundary violations providing forensic data for incident response. Enabling silo audit logging creates visibility into application behavior within containers that can be analyzed to detect exploitation and containment violations. Silo boundary violation events indicate that a process attempted to access resources outside its container which is a strong indicator of application exploitation followed by sandbox escape attempts. Audit events from silo operations should be forwarded to SIEM with alerting on boundary violation events. Regular review of silo audit data helps identify applications that frequently attempt boundary violations which may indicate poorly configured silo policies or active attacks. Silo audit data combined with process execution monitoring provides comprehensive coverage for detecting application exploitation.",
                Tags = ["app-silo", "audit", "boundary-violations", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSiloAuditLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSiloAuditLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSiloAuditLogging", 1)],
            },
            new TweakDef
            {
                Id = "appsiloa-restrict-silo-token-privileges",
                Label = "Restrict Security Token Privileges for Silo Process Execution",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Token privilege restriction for silo processes removes elevated Windows privileges from application execution tokens limiting the capabilities available to exploit code running within the silo. Restricting token privileges reduces the severity of exploitation by preventing compromise code from leveraging privileges like SeDebugPrivilege or SeImpersonatePrivilege that enable lateral movement. Privilege reduction through token stripping is a well-established security technique that limits escalation paths even after an application vulnerability has been exploited. Organizations should remove all privileges not required for the specific application's legitimate function from the silo execution token. Token privilege restriction is complementary to process integrity levels and AppContainer restrictions providing multiple layers of privilege limitation. Applications requiring specific privileges for legitimate functions should have only those specific privileges allowed rather than running with full default token privileges.",
                Tags = ["app-silo", "token-privileges", "privilege-reduction", "exploitation-mitigation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictTokenPrivileges", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictTokenPrivileges")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictTokenPrivileges", 1)],
            },
            new TweakDef
            {
                Id = "appsiloa-enable-silo-integrity-monitoring",
                Label = "Enable File Integrity Monitoring for App Silo Container Contents",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "App Silo file integrity monitoring tracks changes to files within the silo container namespace detecting unauthorized modifications that may indicate persistence establishment by exploit code. Enabling silo integrity monitoring creates alerts when silo container files are modified outside the normal application update path. Exploit code that achieves code execution within a silo may attempt to persist by modifying application files or configuration within the container namespace. Integrity monitoring establishes a baseline of expected container file states and reports deviations that require investigation. Organizations should configure integrity monitoring baselines after initial application deployment and after each authorized application update. Integrity alerts for silo containers should have high priority in security monitoring as they indicate potential exploitation success requiring investigation.",
                Tags = ["app-silo", "integrity-monitoring", "persistence-detection", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableIntegrityMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIntegrityMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIntegrityMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "appsiloa-restrict-silo-object-access",
                Label = "Restrict Silo Access to Named Objects and Synchronization Primitives",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Named object restriction prevents silo processes from accessing Windows named objects like mutexes events and shared memory sections that belong to other processes outside the silo. Restricting silo named object access prevents inter-process communication channels that could be used for silo escape or cross-process exploitation. Named objects are commonly used for IPC between applications and malicious use of these channels is a container escape technique. Silo isolation of named objects requires that silo processes use their own namespace for named objects rather than the global namespace shared by all processes. Applications that use inter-process communication with components outside the silo need policy exceptions that allow the specific named objects required. Monitoring for silo access to named objects in the global namespace helps detect container escape attempts that use IPC channels.",
                Tags = ["app-silo", "named-objects", "ipc", "container-escape", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictNamedObjectAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictNamedObjectAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictNamedObjectAccess", 1)],
            },
            new TweakDef
            {
                Id = "appsiloa-set-silo-memory-limit",
                Label = "Set Memory Usage Limits for App Silo Process Containers",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Memory limits for application silos prevent individual containers from consuming excessive system memory which could cause denial of service by starving other processes or the OS kernel. Setting silo memory limits to appropriate values for the application's expected workload contains memory-based denial of service attacks within the container. Applications that contain memory leaks or run untrusted content that causes memory inflation can be contained to their limit preventing system-wide impact. Memory limit enforcement through Windows job objects is the mechanism used by silos to enforce per-container memory consumption. Organizations should set memory limits based on observed peak memory usage of each silo application with a reasonable headroom above normal peak. Memory limit violations that trigger container termination should be alerted on as they may indicate exploitation or memory-based denial of service within the container.",
                Tags = ["app-silo", "memory-limits", "denial-of-service", "resource-control", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableMemoryLimits", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMemoryLimits")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMemoryLimits", 1)],
            },
            new TweakDef
            {
                Id = "appsiloa-enable-silo-crash-reporting",
                Label = "Enable Crash Reporting for App Silo Security Boundary Failure Analysis",
                Category = "Security — Application Restart",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Silo crash reporting captures crash dumps and failure information when silo processes terminate abnormally enabling investigation of potential exploitation attempts. Enabling crash reporting for silos provides forensic data about the state of the process at the time of termination for analysis of exploitation attempts. Crash dumps from silo processes may contain exploitation artifacts like return-oriented programming chains or heap spray patterns that confirm an exploitation attempt occurred. Organizations should configure silo crash reporting to generate mini-dumps or full dumps depending on the sensitivity of the data processed in the silo. Crash dumps should not be transmitted to external services for silos that process sensitive data as the dump may contain the sensitive data. Regular analysis of silo crash patterns helps identify applications under active exploitation attack.",
                Tags = ["app-silo", "crash-reporting", "forensics", "exploitation-detection", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSiloCrashReporting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSiloCrashReporting")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSiloCrashReporting", 1)],
            },
        ];
    }
}
