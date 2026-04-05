namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyFirewallProfiles
{
    private const string Domain = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile";
    private const string Private = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile";
    private const string Public = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fw-policy-domain-allow-outbound",
            Label = "Allow Outbound by Default (Domain Profile)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultOutboundAction=0 (Allow) under the Domain profile Group Policy path. "
                + "Permits outbound connections by default when on a domain network, while still logging them. "
                + "Allows legitimate outbound traffic without requiring per-application outbound rules for normal domain operations.",
            Tags = ["firewall", "domain", "outbound", "allow", "policy"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Maintains normal outbound connectivity on domain networks.",
            ApplyOps = [RegOp.SetDword(Domain, "DefaultOutboundAction", 0)],
            RemoveOps = [RegOp.DeleteValue(Domain, "DefaultOutboundAction")],
            DetectOps = [RegOp.CheckDword(Domain, "DefaultOutboundAction", 0)],
        },
        new TweakDef
        {
            Id = "fw-policy-public-allow-outbound",
            Label = "Allow Outbound by Default (Public Profile)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultOutboundAction=0 (Allow) under the Public profile Group Policy path. "
                + "Permits outbound connections on public networks so users can browse the web and access cloud services normally. "
                + "Paired with strict inbound blocking to balance security and usability on untrusted networks.",
            Tags = ["firewall", "public", "outbound", "allow", "policy"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Preserves outbound connectivity on public networks.",
            ApplyOps = [RegOp.SetDword(Public, "DefaultOutboundAction", 0)],
            RemoveOps = [RegOp.DeleteValue(Public, "DefaultOutboundAction")],
            DetectOps = [RegOp.CheckDword(Public, "DefaultOutboundAction", 0)],
        },
    ];
}

/// <summary>
/// Sprint 648 — Netlogon secure channel and domain authentication policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\NetLogon and
///           HKLM\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters
/// Controls domain controller secure channel signing, sealing,
/// NT4 crypto restrictions, and DNS-only domain joining.
/// </summary>
internal static class PolicyNetLogon
{
    private const string GpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\NetLogon";
    private const string SvcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sec-netlogon-dns-only-domain-join",
            Label = "Restrict Domain Join to DNS Registration",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowDNSOnlyJoin=1 in Netlogon parameters. "
                + "Prevents the domain join process from using WINS/NetBIOS name resolution to locate domain controllers. "
                + "Forces domain join operations to rely on DNS only, eliminating NetBIOS-based DC discovery that is vulnerable to spoofing.",
            Tags = ["netlogon", "domain-join", "dns", "netbios", "wins", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "DNS-only DC discovery; WINS-based environments may need DNS records updated.",
            ApplyOps = [RegOp.SetDword(SvcKey, "AllowDNSOnlyJoin", 1)],
            RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowDNSOnlyJoin")],
            DetectOps = [RegOp.CheckDword(SvcKey, "AllowDNSOnlyJoin", 1)],
        },
        new TweakDef
        {
            Id = "sec-netlogon-avoid-pdc-on-wan",
            Label = "Avoid PDC Emulator on WAN for Authentication",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AvoidPdcOnWan=1 in Netlogon parameters. "
                + "Instructs the Netlogon service not to contact the PDC Emulator across slow WAN links during user authentication. "
                + "Reduces authentication delays at remote branch office sites where WAN latency to the PDC Emulator would cause login hangs.",
            Tags = ["netlogon", "pdc", "wan", "performance", "branch-office", "ad"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Improves login speed at WAN-connected sites; no security downside.",
            ApplyOps = [RegOp.SetDword(SvcKey, "AvoidPdcOnWan", 1)],
            RemoveOps = [RegOp.DeleteValue(SvcKey, "AvoidPdcOnWan")],
            DetectOps = [RegOp.CheckDword(SvcKey, "AvoidPdcOnWan", 1)],
        },
    ];
}

/// <summary>
/// Sprint 649 — Reliability Monitor data collection and WER reporting policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting
///           HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Reliability
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting
/// Controls whether Reliability Monitor gathers crash data, uploads it,
/// and exposes it through the Windows Error Reporting UI.
/// </summary>
/// <summary>
/// Sprint 650 — DNS client security and multicast name resolution policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\wifinetworkmanager\config
/// Controls LLMNR multicast, DNS-over-HTTPS enforcement, smart name resolution,
/// and related DNS/name resolution security policies.
/// </summary>
/// <summary>
/// Sprint 651 — Windows SmartScreen and application reputation enforcement (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\System (EnableSmartScreen)
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\WTDS\Components (Enhanced Phishing)
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\PowerShell (SmartScreen for scripts)
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\Safer (Software Restriction)
/// Controls Windows SmartScreen filters, enhanced phishing protection, and
/// application reputation checks for downloaded files.
/// </summary>
internal static class PolicySmartScreenWin
{
    private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string WtdsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WTDS\Components";
    private const string SaferKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Safer\CodeIdentifiers";
    private const string AppRepKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-capture",
            Label = "Enable Enhanced Phishing Protection — Capture Check",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CaptureThreatWindow=1 in WTDS Components policy. "
                + "Activates Enhanced Phishing Protection's threat capture mechanism, which screenshots and checks credential entry pages. "
                + "Detects credential harvesting phishing sites in real time, even when embedded in enterprise applications or documents.",
            Tags = ["smartscreen", "phishing", "wtds", "credential", "enhanced"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Captures phishing attempts at credential entry; slight performance overhead.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "CaptureThreatWindow", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "CaptureThreatWindow")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "CaptureThreatWindow", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-notify-malicious",
            Label = "Enhanced Phishing Protection — Notify on Malicious Site",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NotifyMalicious=1 in WTDS Components policy. "
                + "Configures Enhanced Phishing Protection to display a warning notification when a user visits a detected phishing or malicious site. "
                + "Alerts users in real time rather than silently blocking traffic, allowing them to understand why access was interrupted.",
            Tags = ["smartscreen", "phishing", "notification", "wtds", "warning"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Shows phishing warnings in Windows; requires Microsoft Defender support.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyMalicious", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyMalicious")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyMalicious", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-notify-password-reuse",
            Label = "Enhanced Phishing Protection — Warn on Password Reuse",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NotifyPasswordReuse=1 in WTDS Components policy. "
                + "Enables the Enhanced Phishing Protection warning that fires when a user types their Windows account password into a non-Windows credential form. "
                + "Detects password reuse attacks where users enter their corporate password on a personal or untrusted site.",
            Tags = ["smartscreen", "phishing", "password-reuse", "wtds", "credential"],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Warns when Windows password is reused on other sites; zero performance impact.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyPasswordReuse", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyPasswordReuse")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyPasswordReuse", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-unsafe-app",
            Label = "Enhanced Phishing Protection — Warn on Unsafe App Password Entry",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NotifyUnsafeApp=1 in WTDS Components policy. "
                + "Triggers Enhanced Phishing Protection warnings when the Windows account password is entered in an application flagged as potentially unsafe. "
                + "Extends phishing detection beyond browser sessions to desktop applications that prompt for credentials.",
            Tags = ["smartscreen", "phishing", "unsafe-app", "wtds", "desktop-app"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Monitors desktop app credential prompts for password misuse.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyUnsafeApp", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyUnsafeApp")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyUnsafeApp", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-safer-log-policy",
            Label = "Enable Software Restriction Policy Event Logging",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LogFileName=%WINDIR%\\system32\\spp.log in Software Restriction Policy code identifiers. "
                + "Enables SRP to write a detailed log of all application execution events with their restriction disposition to the specified log file. "
                + "Provides an audit trail for compliance and incident investigation.",
            Tags = ["srp", "software-restriction", "safer", "logging", "audit"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Logs all SRP execution decisions to a file for audit review.",
            ApplyOps = [RegOp.SetExpandString(SaferKey, "LogFileName", @"%WINDIR%\system32\spp.log")],
            RemoveOps = [RegOp.DeleteValue(SaferKey, "LogFileName")],
            DetectOps = [RegOp.CheckString(SaferKey, "LogFileName", @"%WINDIR%\system32\spp.log")],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-mrt-disable-auto-download",
            Label = "Disable Automatic MRT Download via Windows Update",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontOfferThroughWUAU=1 in MRT policy. "
                + "Prevents Windows Update from automatically downloading and running the Microsoft Malicious Software Removal Tool (MRT/MSRT). "
                + "In enterprise environments, MRT deployment should be managed through SCCM/Intune or WSUS rather than automatic Windows Update push, "
                + "to control scan timing and avoid unexpected CPU/disk load during business hours.",
            Tags = ["mrt", "msrt", "windows-update", "malware-removal", "enterprise"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Prevents auto MRT push via WU; enterprise should deploy MRT through managed channels.",
            ApplyOps = [RegOp.SetDword(AppRepKey, "DontOfferThroughWUAU", 1)],
            RemoveOps = [RegOp.DeleteValue(AppRepKey, "DontOfferThroughWUAU")],
            DetectOps = [RegOp.CheckDword(AppRepKey, "DontOfferThroughWUAU", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-mrt-disable-infection-report",
            Label = "Disable MRT Infection Report Upload to Microsoft",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontReportInfectionInformation=1 in MRT policy. "
                + "Prevents the Malicious Software Removal Tool from sending infection report telemetry to Microsoft after removing malware. "
                + "The infection report includes information about the malware found, the machine configuration, and the removal status. "
                + "In air-gapped or high-security environments, preventing this upload limits external data transmission.",
            Tags = ["mrt", "infection-report", "telemetry", "upload", "privacy", "air-gap"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Stops MRT infection report uploads; no impact on malware removal capability.",
            ApplyOps = [RegOp.SetDword(AppRepKey, "DontReportInfectionInformation", 1)],
            RemoveOps = [RegOp.DeleteValue(AppRepKey, "DontReportInfectionInformation")],
            DetectOps = [RegOp.CheckDword(AppRepKey, "DontReportInfectionInformation", 1)],
        },
    ];
}

internal static class PolicyAppControl
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                    Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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

