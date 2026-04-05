namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyEnterprise
{
    // ── GroupPolicySettingsPolicy ──
    private static class _GroupPolicySettingsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy";

        // Well-known Group Policy Object GUID for User Configuration CSE
        private const string UserCseKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "gppol-disable-slow-link-detection",
                    Label = "Disable Slow-Link GP Processing Skip",
                    Category = "System — Group Policy Settings",
                    Description =
                        "By default, some Group Policy CSEs (such as Software Installation) are skipped when a slow network link is detected. Disabling this exception ensures all policies are fully applied even over slow connections. Default: slow-link skip enabled. Recommended: 1 on all managed endpoints.",
                    Tags = ["group-policy", "slow-link", "processing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Group Policy fully applies on all network connections including slow links; no CSEs are skipped.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoSlowLink",
                            0
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoSlowLink"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoSlowLink",
                            0
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "gppol-force-reprocess-changed",
                    Label = "Force Reprocessing of Changed GP Objects",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Instructs the Group Policy client to reapply all policy settings at each background refresh cycle, even if no GPO has changed since the last refresh. Ensures policy drift (caused by local changes) is corrected at the next refresh. Default: only changed GPOs are reprocessed. Recommended: 1.",
                    Tags = ["group-policy", "refresh", "reprocess", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "All policy settings are re-applied at every background refresh cycle; local configuration drift is corrected automatically.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoBackgroundPolicy",
                            0
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoBackgroundPolicy"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoBackgroundPolicy",
                            0
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "gppol-set-refresh-interval-30min",
                    Label = "Set GP Background Refresh Interval to 30 Minutes",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Reduces the background Group Policy refresh interval from the default 90 minutes (+30-minute random offset) to 30 minutes. Faster refresh means policy changes reach devices sooner and local configuration drift is corrected more quickly. Default: 90 minutes. Recommended: 30 for dynamic policy environments.",
                    Tags = ["group-policy", "refresh-interval", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Group Policy background refresh fires every 30 minutes; policy changes propagate to devices within half an hour.",
                    ApplyOps = [RegOp.SetDword(Key, "GroupPolicyRefreshTime", 30)],
                    RemoveOps = [RegOp.DeleteValue(Key, "GroupPolicyRefreshTime")],
                    DetectOps = [RegOp.CheckDword(Key, "GroupPolicyRefreshTime", 30)],
                },
                new TweakDef
                {
                    Id = "gppol-set-refresh-offset-0",
                    Label = "Set GP Refresh Random Offset to 0",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Removes the random time offset added to each policy refresh interval. The default offset spreads refresh load across the interval; setting it to 0 makes refreshes predictable and easier to correlate with compliance scan windows. Default: 30-minute random offset. Recommended: 0 in controlled networks.",
                    Tags = ["group-policy", "refresh-interval", "offset", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "GP refresh fires at exactly the configured interval with no random delay; refresh times are deterministic.",
                    ApplyOps = [RegOp.SetDword(Key, "GroupPolicyRefreshTimeDC", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "GroupPolicyRefreshTimeDC")],
                    DetectOps = [RegOp.CheckDword(Key, "GroupPolicyRefreshTimeDC", 0)],
                },
                new TweakDef
                {
                    Id = "gppol-enable-verbose-logging",
                    Label = "Enable Verbose GP Processing Logging",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Writes detailed diagnostic information about each Group Policy processing cycle to the Group Policy Operational event log. Enables troubleshooting of GPO application failures and security audit of policy application. Default: limited operational logging. Recommended: 1 on managed endpoints.",
                    Tags = ["group-policy", "logging", "audit", "diagnostics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Detailed GP processing events written to Microsoft-Windows-GroupPolicy/Operational log.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableVerboseLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableVerboseLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableVerboseLogging", 1)],
                },
                new TweakDef
                {
                    Id = "gppol-disable-user-gpo-override",
                    Label = "Prevent Users from Overriding Group Policy Settings",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Blocks users from modifying registry keys that are managed by Group Policy, even when those keys are under HKCU. Without this, a technically savvy user could temporarily override a GPO setting by writing directly to HKCU. Default: HKCU writes allowed. Recommended: 1 on high-security desktops.",
                    Tags = ["group-policy", "user-override", "security", "hkcu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users cannot bypass HKCU-targeted GP settings by writing conflicting registry values.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUserGPOOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUserGPOOverride")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUserGPOOverride", 1)],
                },
                new TweakDef
                {
                    Id = "gppol-apply-during-logon",
                    Label = "Apply Group Policy Synchronously at Logon",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Forces Group Policy to be applied synchronously at logon — the desktop does not appear until all policies have been processed. Prevents users from interacting with the desktop before security policies (such as drive mappings, logon scripts, and folder redirection) are applied. Default: async logon on workstations. Recommended: 1.",
                    Tags = ["group-policy", "logon", "synchronous", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Logon is slightly slower but all policies are guaranteed applied before the desktop appears.",
                    ApplyOps = [RegOp.SetDword(Key, "SynchronousMachineGroupPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SynchronousMachineGroupPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "SynchronousMachineGroupPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "gppol-log-rsop-data",
                    Label = "Enable RSoP (Resultant Set of Policy) Logging",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Enables collection and logging of Resultant Set of Policy data, which records exactly which policies are applied to each user and computer. Required for the 'Logging' mode of Group Policy Modeling and for compliance audits that verify policy coverage. Default: RSoP logging enabled but may be disabled by some hardening guides. Recommended: 1.",
                    Tags = ["group-policy", "rsop", "logging", "audit", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "RSoP data is collected; IT can run Group Policy Results wizard to verify policy application.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLocalGPOs", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLocalGPOs")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLocalGPOs", 0)],
                },
                new TweakDef
                {
                    Id = "gppol-block-local-gpo",
                    Label = "Disable Local Group Policy Objects for Domain Members",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Prevents Local GPOs (lgpo.exe modifications, Local Security Policy) from being applied on domain-joined machines. When domain GPOs manage all settings, local GPOs can introduce conflicts or be used to circumvent domain policy. Default: local GPOs applied. Recommended: 1 on all domain-joined machines.",
                    Tags = ["group-policy", "local-gpo", "domain", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Local Group Policy Objects are ignored; only domain-delivered GPOs apply.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLocalGPO", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLocalGPO")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLocalGPO", 1)],
                },
                new TweakDef
                {
                    Id = "gppol-require-secure-channel",
                    Label = "Require Secure Channel for GP Download",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Forces Windows to use a signed and encrypted secure channel (Kerberos) when downloading GPOs from the domain controller. Prevents man-in-the-middle attacks that inject malicious policy settings during transport. Default: secure channel used but not strictly enforced for all GPOs. Recommended: 1.",
                    Tags = ["group-policy", "secure-channel", "kerberos", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "GP downloads are authenticated and encrypted; rogue policy injection during GPO download is blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSecureChannel", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSecureChannel")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSecureChannel", 1)],
                },
            ];
    }

    // ── HotpatchUpdatePolicy ──
    private static class _HotpatchUpdatePolicy
    {
        private const string HotpatchKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\Hotpatch";
        private const string WuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "hotpatch-disable-hotpatch-updates",
                    Label = "Disable Windows Hotpatch Updates",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Administratively disables the Hotpatch update channel, reverting the device to the traditional monthly Update Tuesday update cycle that installs patches via a reboot. Suitable for environments that require deterministic full-restart update cycles.",
                    Tags = ["hotpatch", "disable", "windows-update", "patching", "control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Reverts to standard reboot-required patching; ensures full restart cycle occurs each month. No security risk from disabling Hotpatch as long as devices are patched via regular WU channel.",
                    RegistryKeys = [HotpatchKey],
                    ApplyOps = [RegOp.SetDword(HotpatchKey, "EnableHotPatch", 0)],
                    RemoveOps = [RegOp.DeleteValue(HotpatchKey, "EnableHotPatch")],
                    DetectOps = [RegOp.CheckDword(HotpatchKey, "EnableHotPatch", 0)],
                },
                new TweakDef
                {
                    Id = "hotpatch-require-code-integrity",
                    Label = "Require Code Integrity Validation for Hotpatch Modules",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Enforces Authenticode signature verification for every Hotpatch module before it is loaded into kernel memory. Prevents unsigned or tampered patches from being applied even if a threat actor gains WU delivery access.",
                    Tags = ["hotpatch", "code-integrity", "signature", "authenticode", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Strong defence-in-depth: only Microsoft-signed hotpatch binaries can be applied. Has no impact on legitimate Microsoft patches; all Microsoft hotpatches are signed.",
                    RegistryKeys = [HotpatchKey],
                    ApplyOps = [RegOp.SetDword(HotpatchKey, "RequireCodeIntegrity", 1)],
                    RemoveOps = [RegOp.DeleteValue(HotpatchKey, "RequireCodeIntegrity")],
                    DetectOps = [RegOp.CheckDword(HotpatchKey, "RequireCodeIntegrity", 1)],
                },
                new TweakDef
                {
                    Id = "hotpatch-block-rollback",
                    Label = "Block Hotpatch Rollback to Unpatched State",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Prevents administrators and automated tools from rolling back applied hotpatch modules to a pre-patched kernel state. Ensures regulatory compliance environments maintain a continuous patched state.",
                    Tags = ["hotpatch", "rollback", "compliance", "integrity", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Blocking rollback ensures continuous kernel-level protection but may complicate incident response if a hotpatch introduces a regression. Test thoroughly before enforcing.",
                    RegistryKeys = [HotpatchKey],
                    ApplyOps = [RegOp.SetDword(HotpatchKey, "BlockHotpatchRollback", 1)],
                    RemoveOps = [RegOp.DeleteValue(HotpatchKey, "BlockHotpatchRollback")],
                    DetectOps = [RegOp.CheckDword(HotpatchKey, "BlockHotpatchRollback", 1)],
                },
                new TweakDef
                {
                    Id = "hotpatch-audit-patch-events",
                    Label = "Enable Hotpatch Apply and Fail Event Auditing",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Enables detailed event logging for every Hotpatch application attempt, whether successful or failed. Events include the patch identifier, timestamp, module hash, and failure reason code for SIEM ingestion.",
                    Tags = ["hotpatch", "audit", "event-log", "siem", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Non-disruptive; only adds event log entries. Essential for organisations with change-management and patch-tracking compliance requirements.",
                    RegistryKeys = [HotpatchKey],
                    ApplyOps = [RegOp.SetDword(HotpatchKey, "EnableHotpatchAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(HotpatchKey, "EnableHotpatchAudit")],
                    DetectOps = [RegOp.CheckDword(HotpatchKey, "EnableHotpatchAudit", 1)],
                },
                new TweakDef
                {
                    Id = "hotpatch-limit-max-deferred-reboots",
                    Label = "Limit Maximum Reboots Deferred by Hotpatch to 2 Baseline Periods",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Caps the number of consecutive Update Tuesday cycles that Hotpatch can defer a baseline (reboot-required) update. After the configured number of hotpatch-only cycles, a baseline restart is mandated to consolidate all patches.",
                    Tags = ["hotpatch", "baseline-reboot", "deferred-restart", "patch-cycle", "control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Prevents indefinite deferral of reboot-required baselines. Allows 2 hotpatch months before a mandatory restart, balancing uptime and update discipline.",
                    RegistryKeys = [HotpatchKey],
                    ApplyOps = [RegOp.SetDword(HotpatchKey, "MaxDeferredBaselineRestarts", 2)],
                    RemoveOps = [RegOp.DeleteValue(HotpatchKey, "MaxDeferredBaselineRestarts")],
                    DetectOps = [RegOp.CheckDword(HotpatchKey, "MaxDeferredBaselineRestarts", 2)],
                },
                new TweakDef
                {
                    Id = "hotpatch-schedule-baseline-restart",
                    Label = "Schedule Mandatory Baseline Restart Outside Business Hours",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Configures hotpatch baseline restarts to occur outside defined active hours (default: 2:00 AM), avoiding interruption of user sessions. When a baseline reboot is required, it is deferred to the next maintenance window.",
                    Tags = ["hotpatch", "baseline-reboot", "active-hours", "maintenance-window", "scheduling"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Configures restart timing to 2 AM UTC; pairs with the WU active hours policy to keep machines updated without disrupting users.",
                    RegistryKeys = [HotpatchKey],
                    ApplyOps = [RegOp.SetDword(HotpatchKey, "ScheduleBaselineRestartHour", 2)],
                    RemoveOps = [RegOp.DeleteValue(HotpatchKey, "ScheduleBaselineRestartHour")],
                    DetectOps = [RegOp.CheckDword(HotpatchKey, "ScheduleBaselineRestartHour", 2)],
                },
                new TweakDef
                {
                    Id = "hotpatch-disable-telemetry-upload",
                    Label = "Disable Hotpatch Telemetry Upload to Microsoft",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Prevents the Hotpatch subsystem from uploading patch application telemetry, timing data, and failure diagnostics to Microsoft. Retains telemetry locally in the event log only for internal analysis.",
                    Tags = ["hotpatch", "telemetry", "privacy", "diagnostic-data", "cloud"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Reduces data shared with Microsoft about kernel patching events. Does not affect hotpatch functionality or reliability; purely a data outflow control.",
                    RegistryKeys = [HotpatchKey],
                    ApplyOps = [RegOp.SetDword(HotpatchKey, "DisableHotpatchTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(HotpatchKey, "DisableHotpatchTelemetry")],
                    DetectOps = [RegOp.CheckDword(HotpatchKey, "DisableHotpatchTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "hotpatch-exclude-driver-updates",
                    Label = "Exclude Driver Updates from Hotpatch Delivery Channel",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Restricts the Hotpatch delivery channel to security patches only, excluding optional and driver updates. Driver changes often require a full reboot for hardware initialisation; delivering them via Hotpatch risks incomplete initialisation.",
                    Tags = ["hotpatch", "driver-updates", "exclusion", "windows-update", "stability"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Prevents unexpected driver-level changes during a hotpatch cycle; driver updates fall through to the next reboot-requiring WU pass.",
                    RegistryKeys = [HotpatchKey],
                    ApplyOps = [RegOp.SetDword(HotpatchKey, "ExcludeDriversFromHotpatch", 1)],
                    RemoveOps = [RegOp.DeleteValue(HotpatchKey, "ExcludeDriversFromHotpatch")],
                    DetectOps = [RegOp.CheckDword(HotpatchKey, "ExcludeDriversFromHotpatch", 1)],
                },
                new TweakDef
                {
                    Id = "hotpatch-require-managed-device-enrollment",
                    Label = "Require Managed Device Enrollment for Hotpatch Activation",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Permits Hotpatch activation only on devices enrolled in a compatible MDM solution (Intune, MEM). Unmanaged devices fall back to the standard WU reboot channel. Ensures compliance-tracking for reboot-free patch deployments.",
                    Tags = ["hotpatch", "mdm", "intune", "device-enrollment", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Ties Hotpatch enrollment to MDM compliance posture; non-enrolled devices are not eligible. Useful for enterprise environments tracking update compliance via Intune.",
                    RegistryKeys = [WuKey],
                    ApplyOps = [RegOp.SetDword(WuKey, "RequireManagedDeviceForHotpatch", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "RequireManagedDeviceForHotpatch")],
                    DetectOps = [RegOp.CheckDword(WuKey, "RequireManagedDeviceForHotpatch", 1)],
                },
            ];
    }

    // ── HybridJoinDnsPolicy ──
    private static class _HybridJoinDnsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkIsolation";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudAuthentication\HybridJoin";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "hjdns-enable-direct-hybrid-join",
                    Label = "Enable Managed Domain Hybrid Join (No ADFS)",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Enables direct Hybrid Azure AD Join for managed domains without AD FS federation, allowing devices to register with AAD using username/password and SCP discovery.",
                    Tags = ["hybrid-join", "azure-ad", "managed-domain", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Hybrid join enabled for managed domains; no AD FS redirect required.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableDirectHybridJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableDirectHybridJoin")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableDirectHybridJoin", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-block-unregistered-domain-devices",
                    Label = "Block Hybrid Join from Unregistered DNS Domains",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Prevents devices in DNS domains not listed in the Hybrid Join SCP from attempting to register with Azure AD, blocking rogue machines on unknown domains from joining.",
                    Tags = ["hybrid-join", "dns", "domain", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only devices in registered DNS domains can hybrid join; rogue domain machines blocked.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockUnregisteredDomainJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockUnregisteredDomainJoin")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockUnregisteredDomainJoin", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-force-scp-lookup",
                    Label = "Force Service Connection Point Lookup for Hybrid Join",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Forces Azure AD Hybrid Join to use Service Connection Point (SCP) in Active Directory for tenant discovery instead of the local registry, ensuring centrally managed tenant targeting.",
                    Tags = ["hybrid-join", "scp", "active-directory", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "SCP-based tenant discovery enforced; client-side tenant overrides ignored.",
                    ApplyOps = [RegOp.SetDword(Key2, "ForceSCPLookupForTenantDiscovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "ForceSCPLookupForTenantDiscovery")],
                    DetectOps = [RegOp.CheckDword(Key2, "ForceSCPLookupForTenantDiscovery", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-disable-cloud-ap-tenant-override",
                    Label = "Disable Cloud-AP Tenant Override for Hybrid Join",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Blocks user-level Cloud AP (Azure AD Authentication Plugin) tenant override that can redirect a device's hybrid join to a different AAD tenant ID.",
                    Tags = ["hybrid-join", "cloud-ap", "tenant", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Tenant redirect via Cloud AP blocked; join target comes from SCP or policy only.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableCloudAPTenantOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableCloudAPTenantOverride")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableCloudAPTenantOverride", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-isolate-enterprise-endpoints",
                    Label = "Isolate Enterprise Network Endpoints for Cloud Authentication",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Configures Network Isolation policy to classify Microsoft cloud authentication endpoints as enterprise-owned, enabling Windows Information Protection to treat AAD traffic as internal.",
                    Tags = ["hybrid-join", "network-isolation", "wip", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AAD endpoints classified as enterprise; WIP-enforced devices route auth traffic correctly.",
                    ApplyOps = [RegOp.SetDword(Key, "EnterpriseCloudResources", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnterpriseCloudResources")],
                    DetectOps = [RegOp.CheckDword(Key, "EnterpriseCloudResources", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-block-non-domain-dns-fallback",
                    Label = "Block Non-Domain DNS Fallback During Hybrid Join",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Prevents the hybrid join process from falling back to public DNS resolvers when the on-premises DNS server is unavailable, ensuring join only proceeds with trusted DNS resolution.",
                    Tags = ["hybrid-join", "dns-fallback", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hybrid join aborts if domain DNS unreachable; prevents join to wrong tenant via public DNS.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockPublicDNSFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockPublicDNSFallback")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockPublicDNSFallback", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-require-line-of-sight",
                    Label = "Require DC Line-of-Sight for Hybrid Join",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Requires line-of-sight to a domain controller (DC availability check) before allowing the hybrid join registration task to execute, preventing join failures when offline.",
                    Tags = ["hybrid-join", "domain-controller", "offline", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Join task only runs with DC reachable; remote-only machines need internet direct join instead.",
                    ApplyOps = [RegOp.SetDword(Key2, "RequireDCLineOfSight", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RequireDCLineOfSight")],
                    DetectOps = [RegOp.CheckDword(Key2, "RequireDCLineOfSight", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-set-join-timeout",
                    Label = "Set Hybrid Join Task Timeout to 90 Seconds",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Caps the Hybrid Azure AD Join registration task at 90 seconds, preventing long hangs at logon when the join endpoint is unreachable.",
                    Tags = ["hybrid-join", "timeout", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Join task times out after 90 seconds; logon not blocked indefinitely if AAD is unreachable.",
                    ApplyOps = [RegOp.SetDword(Key2, "RegistrationTaskTimeoutSeconds", 90)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RegistrationTaskTimeoutSeconds")],
                    DetectOps = [RegOp.CheckDword(Key2, "RegistrationTaskTimeoutSeconds", 90)],
                },
                new TweakDef
                {
                    Id = "hjdns-disable-joined-device-local-admin",
                    Label = "Disable Local Admin Add via AAD Device Join",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Prevents the automatic addition of the joining user as a local administrator when a device is hybrid-joined to Azure AD, maintaining least-privilege on joined devices.",
                    Tags = ["hybrid-join", "local-admin", "least-privilege", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Joining user not added to local admins on hybrid join; standard user account maintained.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableAutoLocalAdminOnJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableAutoLocalAdminOnJoin")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableAutoLocalAdminOnJoin", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-enable-hybrid-join-audit",
                    Label = "Enable Hybrid Join Operation Audit Logging",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Enables detailed audit event logging for Hybrid Azure AD Join operations, recording device registration attempts, successes, and failures to the Windows event log.",
                    Tags = ["hybrid-join", "audit", "logging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hybrid join events logged; failed or suspicious join attempts are detectable.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableHybridJoinAuditLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableHybridJoinAuditLogging")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableHybridJoinAuditLogging", 1)],
                },
            ];
    }

    // ── IntuneDeviceEventPolicy ──
    private static class _IntuneDeviceEventPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection\MDM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "intuneev-enable-device-health-reporting",
                    Label = "Intune: Enable Intune Device Health Reporting for Compliance Assessment",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets EnableDeviceHealthReporting=1 in the MDM data collection policy. Enables the Intune client health reporting service which sends device health attestation data — TPM status, Secure Boot state, BitLocker encryption status, ELAM driver state, UEFI firmware version — to the Intune service for compliance policy evaluation. "
                        + "Intune's device compliance policies can gate conditional access (blocking Microsoft 365, SharePoint, or other Entra ID protected resources) based on device health. For health-based conditional access to function, the device must send health attestation reports. Disabling health reporting (or leaving it unconfigured) causes compliance status to show as 'Unknown', which depending on conditional access policy settings may either block all access or allow access by default for unknown-state devices.",
                    Tags = ["intune", "mdm", "health-reporting", "compliance", "tpm", "conditional-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Intune device health reports sent to service; compliance-based conditional access evaluates correct device health state.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableDeviceHealthReporting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableDeviceHealthReporting")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableDeviceHealthReporting", 1)],
                },
                new TweakDef
                {
                    Id = "intuneev-disable-mdm-diagnostic-telemetry-upload",
                    Label = "Intune: Disable Voluntary MDM Diagnostic Data Upload to Microsoft",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets DisableMDMDiagnosticsTelemetry=1 in the MDM data collection policy. Stops the Intune MDM client from uploading optional diagnostic data about MDM client performance, error rates, and command processing latency to Microsoft's MDM service telemetry pipeline, separate from Windows diagnostic data. "
                        + "The MDM client telemetry pipeline transmits information about policy processing durations, enrollment command error codes, and sync performance metrics. While this data is used by Microsoft for service improvement and does not contain policy payload content, it reveals information about the organisation's governance structure: how many MDM commands are failing, which policy types are erroring, and whether device compliance is degrading. Disabling this prevents that metadata from leaving the organisation.",
                    Tags = ["intune", "mdm", "telemetry", "diagnostic-data", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "MDM client performance telemetry upload stopped; MDM client metadata stays within the organisation.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMDMDiagnosticsTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMDMDiagnosticsTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMDMDiagnosticsTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "intuneev-require-enrollment-certificate",
                    Label = "Intune: Require PKI Certificate for MDM Enrollment Authentication",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets RequireMDMEnrollmentCertificate=1 in the MDM data collection policy. Configures the MDM client to use a PKI client certificate issued by the internal CA for Intune enrollment authentication, rather than Microsoft Entra ID token-only authentication, providing a hardware-bound credential (certificate stored in TPM) alongside the Entra token. "
                        + "Token-based MDM enrollment (Entra ID access token only) is subject to token theft attacks — an attacker who steals an Entra ID access token from a device could initiate MDM enrollment of a hostile device. PKI certificate-based enrollment requires the certificate private key (ideally TPM-bound) in addition to the Entra token, making stolen tokens insufficient to enrol a new device because the certificate is non-exportable from the TPM.",
                    Tags = ["intune", "mdm", "enrollment", "certificate", "pki", "tpm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "PKI certificate required for MDM enrollment; token theft alone insufficient to enrol a hostile device.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireMDMEnrollmentCertificate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireMDMEnrollmentCertificate")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireMDMEnrollmentCertificate", 1)],
                },
                new TweakDef
                {
                    Id = "intuneev-enable-mdm-event-audit-log",
                    Label = "Intune: Enable MDM Client Audit Logging for Every Policy Command",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets EnableMDMEventAuditLog=1 in the MDM data collection policy. Enables detailed audit logging in the Windows MDM stack, causing every OMA-DM command received from the Intune service (CSP write, CSP delete, configuration profile apply, compliance check result) to generate an audit event in the Security event log. "
                        + "MDM policy delivery happens silently in the background. Without audit logging, there is no on-device record of which policies were applied, when they were applied, which settings were changed, and who authorised the change. This creates a gap in the device's audit trail — changes made via MDM bypass the traditional registry audit trail. With MDM audit logging enabled, all MDM-delivered policy changes generate Security events auditable by SIEM alongside other registry change events.",
                    Tags = ["intune", "mdm", "audit-log", "oma-dm", "csp", "siem"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Every MDM OMA-DM policy command generates a Security event; MDM changes included in SIEM correlation.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableMDMEventAuditLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableMDMEventAuditLog")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableMDMEventAuditLog", 1)],
                },
                new TweakDef
                {
                    Id = "intuneev-block-mdm-unenrollment",
                    Label = "Intune: Block User-Initiated MDM Unenrollment from Settings",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets BlockMDMUnenrollment=1 in the MDM data collection policy. Prevents users from manually removing the MDM enrollment from Settings > Accounts > Access work or school, blocking self-service unenrollment that would remove all MDM-delivered policies, compliance baselines, and enterprise configuration from the device. "
                        + "A user who unenrols their device from MDM removes all Intune-delivered policies, certificates, and compliance configurations in a single action. This gives users the ability to escape enterprise security enforcement by removing device management. The device continues to function normally but is no longer managed, no longer receives security patches via Intune, no longer reports compliance, and potentially still has access to enterprise resources if conditional access doesn't immediately detect the unenrollment.",
                    Tags = ["intune", "mdm", "unenrollment", "lockout", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "User-initiated MDM unenrollment blocked; enterprise management cannot be removed from Settings without admin action.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockMDMUnenrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockMDMUnenrollment")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockMDMUnenrollment", 1)],
                },
                new TweakDef
                {
                    Id = "intuneev-enforce-compliance-check-daily",
                    Label = "Intune: Enforce Daily MDM Compliance Check-In Regardless of Network",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets EnforceComplianceCheckCadenceHours=24 in the MDM data collection policy. Forces the Intune MDM client to attempt a compliance status check-in to the Intune service at least once every 24 hours, even if the last successful sync was within the standard 8-hour interval, ensuring compliance policy is always evaluated at least daily. "
                        + "MDM sync frequency is typically driven by the Intune service push schedule. Devices that are frequently off the corporate network (remote workers using cellular connections) may go days between Intune syncs if they are not on Wi-Fi and data usage policies are aggressive. A device not syncing for multiple days may have outdated compliance status, allowing it to retain conditional access even after a compliance change (e.g., BitLocker requirement added) that it cannot meet.",
                    Tags = ["intune", "mdm", "compliance", "check-in", "cadence", "remote"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "MDM compliance check-in enforced at least daily; compliance status reflects current policy even for remote workers.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceComplianceCheckCadenceHours", 24)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceComplianceCheckCadenceHours")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceComplianceCheckCadenceHours", 24)],
                },
                new TweakDef
                {
                    Id = "intuneev-require-signed-mdm-commands",
                    Label = "Intune: Require Cryptographic Signing of All OMA-DM Commands",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets RequireSignedMDMCommands=1 in the MDM data collection policy. Requires that all OMA-DM commands received from the MDM server are cryptographically signed with the Intune service certificate, and rejects unsigned or incorrectly signed OMA-DM payloads, protecting against rogue MDM server injection. "
                        + "OMA-DM is the protocol that carries MDM policy commands from the Intune service to the client. Without command signing enforcement, an attacker who achieves a man-in-the-middle position between the endpoint and the Intune service endpoint could inject arbitrary OMA-DM commands (which translate to registry writes, file downloads, and application installs). Requiring signed commands ensures only the authentic Intune service can deliver policy changes.",
                    Tags = ["intune", "mdm", "oma-dm", "signing", "mitm", "command-integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Unsigned OMA-DM commands rejected; MDM policy injection via man-in-the-middle blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSignedMDMCommands", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedMDMCommands")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSignedMDMCommands", 1)],
                },
                new TweakDef
                {
                    Id = "intuneev-enable-mdm-config-lockdown",
                    Label = "Intune: Enable MDM Config Lock to Re-Enforce Settings Changed Out-of-Band",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets EnableMDMConfigLockdown=1 in the MDM data collection policy. Enables the MDM config lock feature, which continuously monitors settings delivered by Intune compliance or configuration profiles and automatically reverts any changes made to those settings through other means (GPO that conflicts with MDM, manual registry edits, third-party tools). "
                        + "MDM config lock prevents MDM-delivered settings from being overridden by competing configuration mechanisms. Without config lock, other Group Policy settings delivered via domain join, local GPOs applied by elevated users, or malicious registry edits can override MDM-delivered security baselines. Config lock creates a continuous enforcement loop that re-applies MDM settings whenever they deviate from the expected values, functioning as a security posture self-healing mechanism.",
                    Tags = ["intune", "mdm", "config-lock", "drift", "enforcement", "security-baseline"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "MDM config lockdown active; out-of-band registry/GPO changes that conflict with Intune profiles are automatically reverted.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableMDMConfigLockdown", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableMDMConfigLockdown")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableMDMConfigLockdown", 1)],
                },
                new TweakDef
                {
                    Id = "intuneev-disable-mdm-agent-auto-update-from-store",
                    Label = "Intune: Block MDM Agent Auto-Update from Microsoft Store",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets DisableMDMAgentAutoUpdate=1 in the MDM data collection policy. Prevents the Intune Company Portal and MDM management agent components from auto-updating from the Microsoft Store, requiring IT to control agent updates through managed deployment paths (MDM app profiles, SCCM, or Intune Win32 app) rather than consumer Store delivery. "
                        + "MDM agent updates delivered through the Microsoft Store follow the Store's release schedule independently of IT's testing and validation calendar. A Store-delivered agent update may change MDM enrollment flow, compliance evaluation behaviour, or Company Portal UI in ways that weren't tested by IT's change management process. Blocking auto-update from Store and using managed deployment paths ensures IT controls when MDM agent updates reach production endpoints.",
                    Tags = ["intune", "mdm", "company-portal", "auto-update", "store", "change-control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "MDM agent Store updates blocked; Intune Company Portal and agent updates require IT-managed deployment.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMDMAgentAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMDMAgentAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMDMAgentAutoUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "intuneev-enable-remote-wipe-audit-log",
                    Label = "Intune: Enable Audit Logging for Remote Wipe Commands Received from MDM Server",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets EnableRemoteWipeAuditLog=1 in the MDM data collection policy. Generates a Security event log entry (and application event log warning) the moment the Intune service delivers a remote wipe command to the client, recording the timestamp and wipe type (quick wipe vs full wipe) before the wipe execution begins. "
                        + "Remote wipe is the nuclear security action available through MDM — it erases all device data. Without an audit log entry before execution, there is no on-device evidence that a wipe was initiated via MDM (distinguishable from a local factory reset). In scenarios where a remote wipe was accidental (wrong device targeted in the Intune console) or unauthorised (admin credential compromise), forensic investigation of what happened requires an event record. A pre-wipe audit log event can be captured by a SIEM before the device is erased.",
                    Tags = ["intune", "mdm", "remote-wipe", "audit-log", "forensics", "siem"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Security event written when remote wipe command received; SIEM captures wipe initiation before erasure completes.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableRemoteWipeAuditLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableRemoteWipeAuditLog")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableRemoteWipeAuditLog", 1)],
                },
            ];
    }

    // ── MdmEnrollmentPolicy ──
    private static class _MdmEnrollmentPolicy
    {
        private const string MdmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";
        private const string WpjKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";
        private const string HelloKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "mdmpol-disable-auto-enroll",
                Label = "Disable Automatic MDM Enrollment on Azure AD Join",
                Category = "System — Group Policy Settings",
                Description =
                    "Prevents the device from automatically enrolling into Mobile Device Management (MDM/Intune) when joined to Azure Active Directory. Requires explicit manual enrollment.",
                Tags = ["mdm", "intune", "azure-ad", "enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Prevents automatic Intune enrollment on Azure AD join; deploy deliberately.",
                RegistryKeys = [MdmKey],
                ApplyOps = [RegOp.SetDword(MdmKey, "AutoEnrollMDM", 0)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "AutoEnrollMDM")],
                DetectOps = [RegOp.CheckDword(MdmKey, "AutoEnrollMDM", 0)],
            },
            new TweakDef
            {
                Id = "mdmpol-disable-user-registration",
                Label = "Disable User-Initiated MDM Registration",
                Category = "System — Group Policy Settings",
                Description =
                    "Prevents users from manually registering the device with a Mobile Device Management server. Only administrators can initiate MDM enrollment.",
                Tags = ["mdm", "enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks self-service MDM registration by users.",
                RegistryKeys = [MdmKey],
                ApplyOps = [RegOp.SetDword(MdmKey, "EnableRegistration", 0)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableRegistration")],
                DetectOps = [RegOp.CheckDword(MdmKey, "EnableRegistration", 0)],
            },
            new TweakDef
            {
                Id = "mdmpol-disable-hello-pin-recovery",
                Label = "Disable Windows Hello PIN Recovery Service",
                Category = "System — Group Policy Settings",
                Description =
                    "Disables the cloud-based PIN recovery service for Windows Hello. PINs cannot be reset via Microsoft account cloud backup. Keeps credentials fully local.",
                Tags = ["windows-hello", "pin", "recovery", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Keeps PIN credentials fully local; disables cloud recovery backup.",
                RegistryKeys = [HelloKey],
                ApplyOps = [RegOp.SetDword(HelloKey, "EnablePinRecovery", 0)],
                RemoveOps = [RegOp.DeleteValue(HelloKey, "EnablePinRecovery")],
                DetectOps = [RegOp.CheckDword(HelloKey, "EnablePinRecovery", 0)],
            },
            new TweakDef
            {
                Id = "mdmpol-disable-hello-biometrics",
                Label = "Disable Biometrics for Windows Hello",
                Category = "System — Group Policy Settings",
                Description =
                    "Disables the use of biometrics (fingerprint, face recognition) for Windows Hello authentication. PIN remains available as the fallback credential.",
                Tags = ["windows-hello", "biometrics", "fingerprint", "face-id", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Forces PIN-only authentication; no fingerprint or face ID.",
                RegistryKeys = [$@"{HelloKey}\Biometrics"],
                ApplyOps = [RegOp.SetDword($@"{HelloKey}\Biometrics", "UseBiometrics", 0)],
                RemoveOps = [RegOp.DeleteValue($@"{HelloKey}\Biometrics", "UseBiometrics")],
                DetectOps = [RegOp.CheckDword($@"{HelloKey}\Biometrics", "UseBiometrics", 0)],
            },
            new TweakDef
            {
                Id = "mdmpol-disable-dynamic-lock",
                Label = "Disable Dynamic Lock (Phone Proximity Lock)",
                Category = "System — Group Policy Settings",
                Description =
                    "Disables Dynamic Lock, which automatically locks the PC when a paired Bluetooth phone moves out of range. Prevents unintended automatic locking in enterprise environments.",
                Tags = ["dynamic-lock", "bluetooth", "lock", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables Bluetooth proximity-based automatic PC locking.",
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock", 0)],
            },
        ];
    }

    // ── MdmRegistrationPolicy ──
    private static class _MdmRegistrationPolicy
    {
        private const string MdmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

        private const string EnrollKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDMEnrollment";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "mdmreg-require-reenrollment-on-rename",
                    Label = "MDM Registration: Require Re-Enrollment after Device Rename",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets RequireReenrollmentOnRename=1 in MDM policy. Forces the device to re-enroll in MDM when the device name changes. Device renaming is sometimes used as a pivot technique during lateral movement: an attacker renames a managed device to match an expected device name to pass name-based access controls. Forcing re-enrollment on rename ensures the MDM service receives a new enrollment token for the renamed device, which updates the device record in the MDM database and triggers compliance re-evaluation. Any conditional access policies that check the MDM enrollment record are therefore aware of the identity change.",
                    Tags = ["mdm", "re-enrollment", "device-rename", "identity", "conditional-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Device renaming triggers MDM re-enrollment. Re-enrollment is transparent — it occurs in the background without disrupting the user session. Useful in environments where device names are used as identifiers in network access rules or SIEM queries.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "RequireReenrollmentOnRename", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "RequireReenrollmentOnRename")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "RequireReenrollmentOnRename", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-disable-user-unenrollment",
                    Label = "MDM Registration: Prevent Users from Manually Unenrolling Device from MDM",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets DisallowUserMdmUnenrollment=1 in MDM policy. Prevents standard users (non-administrators) from unenrolling the device from MDM management through the Settings app. Without this policy, any user with access to Settings > Accounts > Access work or school can disconnect the device from MDM management, effectively removing it from IT control, compliance enforcement, and conditional access scope. While admins can still unenroll via MDM push commands, preventing user-initiated unenrollment ensures the device remains managed.",
                    Tags = ["mdm", "unenrollment", "user-restriction", "settings", "tamper-prevention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Standard users cannot disconnect the device from MDM using Settings. Local administrators and MDM push-initiated unenrollment still work. The Settings UI option to disconnect is grayed out or removed for non-admins.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "DisallowUserMdmUnenrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "DisallowUserMdmUnenrollment")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "DisallowUserMdmUnenrollment", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-use-enterprise-enrollment-only",
                    Label = "MDM Registration: Restrict MDM Enrollment to Enterprise Tenants Only",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets EnterpriseEnrollmentOnly=1 in MDM policy. Restricts MDM enrollment so that only corporate tenants (as determined by the MDM authority in the Group Policy or the domain's MDM discovery service) can claim management of the device. Without this policy, a device can be enrolled by any MDM provider, including personal Intune accounts. This is relevant in bring-your-own-device (BYOD) scenarios where an employee might accidentally enroll their managed corporate device with their personal Microsoft 365 account's MDM, causing policy conflicts.",
                    Tags = ["mdm", "enrollment", "enterprise-only", "byod", "tenant-restriction"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Only the corporate MDM authority (set by Group Policy or Windows AutoPilot) can enroll the device. Personal Microsoft account MDM enrollment is rejected. Prevents accidental dual-enrollment or policy conflicts from personal MDM tenants.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnterpriseEnrollmentOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnterpriseEnrollmentOnly")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnterpriseEnrollmentOnly", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-enable-diagnostic-auto-upload",
                    Label = "MDM Registration: Enable Automatic Diagnostic Log Upload to MDM",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets EnableDiagnosticUpload=1 in MDM policy. Enables the MDM client to automatically upload MDM diagnostic logs to the MDM server when requested via a remote log collection push from the MDM authority. Without this, IT admins must physically access the device or use complex manual collection procedures to retrieve MDM diagnostic files. With this enabled, an MDM admin can trigger log collection from the Intune console without user interaction — essential for diagnosing enrollment failures, policy application errors, or app deployment problems on devices that are not physically accessible.",
                    Tags = ["mdm", "diagnostics", "log-upload", "remote-collection", "intune"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "MDM diagnostic logs are uploaded to the MDM server on remote request. Logs include MDM client logs, Event Log snapshots, and enrollment logs. Only the MDM server can initiate collection — users cannot trigger it. Small bandwidth overhead during collection.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnableDiagnosticUpload", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableDiagnosticUpload")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnableDiagnosticUpload", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-set-enrollment-check-in-interval-4h",
                    Label = "MDM Registration: Set MDM Check-In Interval to 4 Hours",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets EnrollmentCheckInIntervalHours=4 in MDM policy. Sets the frequency at which the MDM client checks in with the MDM server to receive new policies, app assignments, compliance commands, and configuration updates. The default check-in interval is 8 hours. A 4-hour interval reduces the lag between MDM policy changes (such as blocking USB, pushing a security update requirement, or revering a credential) and their application on devices. In incident response scenarios, the ability to push a policy change and have it take effect within 4 hours rather than 8 hours is a meaningful response time improvement.",
                    Tags = ["mdm", "check-in", "policy-apply", "interval", "response-time"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "MDM client checks in with the MDM server every 4 hours. Reduces policy propagation lag from 8h to 4h. Slightly higher MDM service traffic — negligible for typical enterprise deployments.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnrollmentCheckInIntervalHours", 4)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnrollmentCheckInIntervalHours")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnrollmentCheckInIntervalHours", 4)],
                },
                new TweakDef
                {
                    Id = "mdmreg-enable-conditional-access-notification",
                    Label = "MDM Registration: Enable MDM Enrollment Notification for Conditional Access",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets NotifyConditionalAccessOnEnrollment=1 in MDM policy. Configures the MDM client to push an enrollment state notification to the Azure AD conditional access service whenever the device's MDM enrollment status changes (enrolled, unenrolled, compliance state changed). Without this notification push, conditional access relies on polling of the Intune device inventory, which has a delay. The push notification significantly reduces the time between an enrollment state change and the conditional access enforcement update — important for scenarios like immediately restoring access after successful compliance remediation.",
                    Tags = ["mdm", "conditional-access", "enrollment-notification", "aad", "response-time"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enrollment state changes trigger an immediate push notification to AAD conditional access. Reduces the delay between compliance remediation and access restoration. Requires AAD and Intune integration — has no effect on on-premises MDM without AAD integration.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "NotifyConditionalAccessOnEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "NotifyConditionalAccessOnEnrollment")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "NotifyConditionalAccessOnEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-block-guest-from-enrollment",
                    Label = "MDM Registration: Block Guest Accounts from MDM Enrollment",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets BlockGuestAccountEnrollment=1 in MDM policy. Prevents Guest accounts from triggering MDM enrollment or accessing MDM-managed resources. Guest accounts by definition have no AAD identity and should not enroll in MDM. In some configurations, a device with an active Guest session can inadvertently trigger MDM enrollment flows with an empty principal, creating orphaned device records in the MDM tenant. Blocking guest account enrollment eliminates this edge case and prevents Guest-session processes from interacting with the MDM client.",
                    Tags = ["mdm", "guest-account", "enrollment-block", "identity", "orphaned-device"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Guest accounts cannot initiate or complete MDM enrollment. Prevents orphaned MDM device records from Guest-triggered enrollment flows. No impact on standard user or administrator enrollment processes.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "BlockGuestAccountEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "BlockGuestAccountEnrollment")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "BlockGuestAccountEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-enable-silent-enrollment",
                    Label = "MDM Registration: Enable Silent (No User Prompt) MDM Enrollment",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets EnableSilentEnrollment=1 in MDM policy. Configures MDM enrollment to complete silently without displaying user-facing dialogs, progress indicators, or consent prompts. Silent enrollment is used in corporate provisioning scenarios (Autopilot, bulk enrolment) where the device is pre-configured by IT before delivery to the user. Without silent enrollment, the MDM client shows enrollment progress dialogs that may alarm users who are not expecting them. Silent enrollment also reduces the risk of users cancelling the enrollment process mid-flow, which can leave the device in a partially-enrolled state.",
                    Tags = ["mdm", "silent-enrollment", "autopilot", "provisioning", "user-experience"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "MDM enrollment completes without user-visible dialogs or prompts. Used in Autopilot and bulk enrollment scenarios. Best combined with Enrollment Status Page (ESP) for user transparency during provisioning.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnableSilentEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableSilentEnrollment")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnableSilentEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-enable-enrollment-retry-on-failure",
                    Label = "MDM Registration: Enable Automatic Retry on MDM Enrollment Failure",
                    Category = "System — Group Policy Settings",
                    Description =
                        "Sets EnableEnrollmentRetryOnFailure=1 in EnrollmentSecurity policy. Enables the MDM client to automatically retry enrollment if the initial enrollment attempt fails due to network connectivity issues, MDM service transient errors, or AAD token acquisition failures. Without retry logic, a single transient failure during Autopilot provisioning (e.g., the device starts enrollment before DNS is fully resolving, or the MDM service returns HTTP 503 during a brief outage) results in a permanently unenrolled device that requires manual remediation. Automatic retry ensures transient failures are recovered without IT intervention.",
                    Tags = ["mdm", "enrollment-retry", "resilience", "autopilot", "transient-failure"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Failed MDM enrollment attempts are automatically retried with exponential backoff. Significantly reduces Autopilot and bulk-enrollment failures due to transient connectivity or service errors. Retry schedule is governed by the MDM client's built-in backoff policy.",
                    ApplyOps = [RegOp.SetDword(EnrollKey, "EnableEnrollmentRetryOnFailure", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnrollKey, "EnableEnrollmentRetryOnFailure")],
                    DetectOps = [RegOp.CheckDword(EnrollKey, "EnableEnrollmentRetryOnFailure", 1)],
                },
            ];
    }

    // ── OobePolicy ──
    private static class _OobePolicy
    {
        private const string OobeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OOBE";
        private const string SetupKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Setup";
        private const string ShellLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Shell";
        private const string ShellCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Shell";
        private const string SrvMgrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Server Manager";
        private const string SystemPolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "oobe-no-network-connections-wizard",
                Label = "Disable OOBE Network Connections Wizard",
                Category = "System — Group Policy Settings",
                Description =
                    "Sets DisableNetworkConnectionsWizard=1 in the Windows OOBE policy key. "
                    + "Suppresses the network connection setup wizard that appears during the OOBE phase, "
                    + "useful when network configuration is handled by MDM or answer files. "
                    + "Default: absent (network wizard shown). Recommended: 1 in managed deployment scenarios.",
                Tags = ["oobe", "network", "wizard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Skips the OOBE network setup wizard; network connectivity is handled by provisioning tools.",
                ApplyOps = [RegOp.SetDword(OobeKey, "DisableNetworkConnectionsWizard", 1)],
                RemoveOps = [RegOp.DeleteValue(OobeKey, "DisableNetworkConnectionsWizard")],
                DetectOps = [RegOp.CheckDword(OobeKey, "DisableNetworkConnectionsWizard", 1)],
            },
            new TweakDef
            {
                Id = "oobe-no-first-logon-animation",
                Label = "Disable First Logon Animation",
                Category = "System — Group Policy Settings",
                Description =
                    "Sets ShowFirstLogonAnimation=0 in the Windows Setup policy key. "
                    + "Disables the full-screen 'Hi' and 'Getting Windows ready' animation sequence shown to new users on first sign-in, "
                    + "reducing the wait time at initial logon. "
                    + "Default: absent (animation shown). Recommended: 0 on corporate desktops for faster first-logon.",
                Tags = ["oobe", "animation", "first-logon", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Skips the first-logon animation; new users reach the desktop faster on initial sign-in.",
                ApplyOps = [RegOp.SetDword(SetupKey, "ShowFirstLogonAnimation", 0)],
                RemoveOps = [RegOp.DeleteValue(SetupKey, "ShowFirstLogonAnimation")],
                DetectOps = [RegOp.CheckDword(SetupKey, "ShowFirstLogonAnimation", 0)],
            },
            new TweakDef
            {
                Id = "oobe-no-welcome-screen-lm",
                Label = "Disable Welcome Screen (Machine)",
                Category = "System — Group Policy Settings",
                Description =
                    "Sets NoWelcomeScreen=1 in the machine-scoped Windows Shell policy key. "
                    + "Suppresses the Windows Welcome Center / Did You Know tips overlay that could appear post-setup. "
                    + "Default: absent. Recommended: 1 on managed enterprise desktops to skip unneeded first-run UI.",
                Tags = ["oobe", "welcome", "shell", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Suppresses the Welcome Center overlay for all users; no functional impact after initial run.",
                ApplyOps = [RegOp.SetDword(ShellLm, "NoWelcomeScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(ShellLm, "NoWelcomeScreen")],
                DetectOps = [RegOp.CheckDword(ShellLm, "NoWelcomeScreen", 1)],
            },
            new TweakDef
            {
                Id = "oobe-no-welcome-screen-user",
                Label = "Disable Welcome Screen (Current User)",
                Category = "System — Group Policy Settings",
                Description =
                    "Sets NoWelcomeScreen=1 in the per-user Windows Shell policy key. "
                    + "Hides the Welcome Center / Getting Started experience for the current user. "
                    + "Default: absent. Recommended: 1 for individual user profiles on managed systems.",
                Tags = ["oobe", "welcome", "shell", "user", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Suppresses the Welcome Center for this user only; no functional impact.",
                ApplyOps = [RegOp.SetDword(ShellCu, "NoWelcomeScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(ShellCu, "NoWelcomeScreen")],
                DetectOps = [RegOp.CheckDword(ShellCu, "NoWelcomeScreen", 1)],
            },
            new TweakDef
            {
                Id = "oobe-no-server-manager-at-logon",
                Label = "Disable Server Manager Auto-Open at Logon",
                Category = "System — Group Policy Settings",
                Description =
                    "Sets DoNotOpenServerManagerAtLogon=1 in the Server Manager policy key. "
                    + "Prevents Windows Server Manager from automatically opening at every administrator logon. "
                    + "Applies to Windows Server editions; setting is ignored on Windows Client. "
                    + "Default: absent (Server Manager opens at logon). Recommended: 1 on production servers where automatic windows interfere with operations.",
                Tags = ["oobe", "server-manager", "server", "logon", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Server Manager no longer opens automatically at logon; it can still be launched manually.",
                ApplyOps = [RegOp.SetDword(SrvMgrKey, "DoNotOpenServerManagerAtLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(SrvMgrKey, "DoNotOpenServerManagerAtLogon")],
                DetectOps = [RegOp.CheckDword(SrvMgrKey, "DoNotOpenServerManagerAtLogon", 1)],
            },
            new TweakDef
            {
                Id = "oobe-disable-balloon-tips",
                Label = "Disable System Tray Balloon Tips",
                Category = "System — Group Policy Settings",
                Description =
                    "Sets EnableBalloonTips=0 in the machine-side System policy key. "
                    + "Suppresses all Action Center / notification area balloon notifications and first-run tip balloons "
                    + "that appear after the initial desktop load. "
                    + "Default: absent (balloon tips enabled). Recommended: 0 on corporate desktops to reduce user interruptions.",
                Tags = ["oobe", "balloon", "notifications", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses first-run balloon tips and system tray notifications; Action Center itself is unaffected.",
                ApplyOps = [RegOp.SetDword(ShellLm, "EnableBalloonTips", 0)],
                RemoveOps = [RegOp.DeleteValue(ShellLm, "EnableBalloonTips")],
                DetectOps = [RegOp.CheckDword(ShellLm, "EnableBalloonTips", 0)],
            },
            new TweakDef
            {
                Id = "oobe-disable-upgrade-ui",
                Label = "Disable Windows Upgrade Prompt UI",
                Category = "System — Group Policy Settings",
                Description =
                    "Sets DisableUXFirstRunAnimation=1 in the Windows Setup policy key. "
                    + "Suppresses the upgrade experience UX animations and first-run prompts that may appear "
                    + "after a major Windows feature update is applied to an existing account. "
                    + "Default: absent. Recommended: 1 on managed devices receiving OS updates via WSUS / Autopilot.",
                Tags = ["oobe", "upgrade", "animation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Suppresses post-upgrade UX first-run animations; system functionality is unaffected.",
                ApplyOps = [RegOp.SetDword(SetupKey, "DisableUXFirstRunAnimation", 1)],
                RemoveOps = [RegOp.DeleteValue(SetupKey, "DisableUXFirstRunAnimation")],
                DetectOps = [RegOp.CheckDword(SetupKey, "DisableUXFirstRunAnimation", 1)],
            },
        ];
    }

    // ── RetailDemoPolicy ──
    private static class _RetailDemoPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RetailDemo";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "rdemo-disable-retail-demo",
                Label = "Disable Retail Demo Mode",
                Category = "System — Group Policy Settings",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets NoRetailDemo=1 in the RetailDemo policy key. Prevents Windows from "
                    + "entering retail demo mode, which runs a continuous promotional demonstration "
                    + "experience on display units in retail stores. Demo mode overrides normal user "
                    + "experience settings and launches curated content. Default: 0 (demo mode "
                    + "allowed by OEM configuration). Recommended: 1 on corporate and personal "
                    + "devices to block any inadvertent demo-mode activation.",
                Tags = ["retail-demo", "kiosk", "policy", "display"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRetailDemo", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRetailDemo")],
                DetectOps = [RegOp.CheckDword(Key, "NoRetailDemo", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-attract-loop",
                Label = "Disable Retail Demo Attract Loop",
                Category = "System — Group Policy Settings",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoAttractLoop=1 in the RetailDemo policy key. Stops the idle attract-loop "
                    + "video that plays on unattended retail display machines to showcase Windows "
                    + "features and invite customer interaction. On non-retail devices this loop "
                    + "would trigger after inactivity timeout and play promotional video full-screen. "
                    + "Default: 0. Recommended: 1 on any device that is not a retail display unit.",
                Tags = ["retail-demo", "attract", "video", "idle", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoAttractLoop", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoAttractLoop")],
                DetectOps = [RegOp.CheckDword(Key, "NoAttractLoop", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-auto-signin",
                Label = "Disable Retail Demo Auto Sign-In",
                Category = "System — Group Policy Settings",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets NoAutoSignIn=1 in the RetailDemo policy key. Prevents a demo account from "
                    + "automatically signing in on boot, which is a behaviour specific to retail "
                    + "store display units running in unattended self-service mode. Automatic demo "
                    + "sign-in bypasses normal login prompts and loads a preconfigured experience "
                    + "account. Default: 0. Recommended: 1 on devices requiring secure authentication.",
                Tags = ["retail-demo", "sign-in", "auto-login", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoAutoSignIn", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoAutoSignIn")],
                DetectOps = [RegOp.CheckDword(Key, "NoAutoSignIn", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-demo-apps",
                Label = "Disable Retail Demo App Provisioning",
                Category = "System — Group Policy Settings",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoRetailDemoApps=1 in the RetailDemo policy key. Prevents provisioning of "
                    + "Microsoft-curated demo applications installed on retail display machines to "
                    + "showcase Windows and Microsoft 365 features. These apps are silently installed "
                    + "from the Store without user consent on retail-configured devices. "
                    + "Default: 0. Recommended: 1 on enterprise and personal devices.",
                Tags = ["retail-demo", "apps", "store", "provisioning", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRetailDemoApps", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRetailDemoApps")],
                DetectOps = [RegOp.CheckDword(Key, "NoRetailDemoApps", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-demo-content",
                Label = "Disable Retail Demo Content Delivery",
                Category = "System — Group Policy Settings",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoRetailDemoContent=1 in the RetailDemo policy key. Prevents Windows from "
                    + "downloading and staging promotional content packages from Microsoft CDN for "
                    + "use in retail demo scenarios. These background downloads consume network "
                    + "bandwidth and disk space without user awareness on non-retail devices. "
                    + "Default: 0. Recommended: 1 on metered or managed connections.",
                Tags = ["retail-demo", "content", "cdn", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRetailDemoContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRetailDemoContent")],
                DetectOps = [RegOp.CheckDword(Key, "NoRetailDemoContent", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-experience-provider",
                Label = "Disable Retail Demo Device Experience Provider",
                Category = "System — Group Policy Settings",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoDeviceExperienceProvider=1 in the RetailDemo policy key. Blocks the "
                    + "Device Experience Provider component used by OEM retail configurations to "
                    + "display branded hardware demonstrations and guided tours during initial setup "
                    + "in stores. This provider can launch demo walkthroughs without user action. "
                    + "Default: 0. Recommended: 1 on post-purchase devices.",
                Tags = ["retail-demo", "experience", "oem", "provider", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoDeviceExperienceProvider", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoDeviceExperienceProvider")],
                DetectOps = [RegOp.CheckDword(Key, "NoDeviceExperienceProvider", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-demo-banner",
                Label = "Disable Retail Demo Info Banner",
                Category = "System — Group Policy Settings",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets NoDemoBanner=1 in the RetailDemo policy key. Hides the informational "
                    + "banner shown in retail demo mode that prompts customers to purchase the "
                    + "device or explore Windows features being demonstrated on the floor model. "
                    + "This UI element is irrelevant and distracting on owned devices. "
                    + "Default: 0. Recommended: 1 on non-retail hardware.",
                Tags = ["retail-demo", "banner", "ui", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoDemoBanner", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoDemoBanner")],
                DetectOps = [RegOp.CheckDword(Key, "NoDemoBanner", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-oobe-demo",
                Label = "Disable Retail Demo OOBE Flow",
                Category = "System — Group Policy Settings",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoRetailOOBE=1 in the RetailDemo policy key. Prevents the out-of-box "
                    + "experience from branching into retail demo setup mode during initial device "
                    + "configuration. The retail OOBE path creates a temporary guest demo account "
                    + "and loads promotional assets instead of standard setup. "
                    + "Default: 0 for OEM-configured retail images. Recommended: 1 everywhere else.",
                Tags = ["retail-demo", "oobe", "setup", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRetailOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRetailOOBE")],
                DetectOps = [RegOp.CheckDword(Key, "NoRetailOOBE", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-cleanup-revert",
                Label = "Disable Retail Demo Cleanup Revert Task",
                Category = "System — Group Policy Settings",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoCleanupRevert=1 in the RetailDemo policy key. Blocks the scheduled "
                    + "retail demo cleanup task that runs after business hours to wipe user "
                    + "interactions and restore the machine to factory demo defaults. On non-retail "
                    + "devices this task would destructively remove user customisations and data "
                    + "overnight. Default: 0. Recommended: 1 on all personal and managed devices.",
                Tags = ["retail-demo", "cleanup", "scheduled-task", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoCleanupRevert", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoCleanupRevert")],
                DetectOps = [RegOp.CheckDword(Key, "NoCleanupRevert", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-demo-telemetry",
                Label = "Disable Retail Demo Interaction Telemetry",
                Category = "System — Group Policy Settings",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoRetailTelemetry=1 in the RetailDemo policy key. Stops transmission of "
                    + "retail demo interaction analytics (which buttons were pressed, time spent on "
                    + "demo scenes, feature engagement) to Microsoft. This retail-specific telemetry "
                    + "stream is separate from the standard Windows diagnostic data pipeline and "
                    + "continues even when diagnostic level is set to Basic. "
                    + "Default: 0. Recommended: 1 for privacy.",
                Tags = ["retail-demo", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRetailTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRetailTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "NoRetailTelemetry", 1)],
            },
        ];
    }

    // ── SharedPCPolicy ──
    private static class _SharedPCPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SharedPC";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "shpc-disable-shared-pc-mode",
                Label = "Disable Shared PC Mode",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Shared PC mode optimizes Windows for use by multiple users on a single device, enabling account deletion and profile cleanup between sessions. Disabling Shared PC mode ensures a standard single-user workstation configuration is enforced on dedicated corporate devices. Shared PC mode behaviors including account deletion and profile compression are inappropriate for dedicated workstations where user data persistence is required. This policy is relevant for environments that may inadvertently inherit shared PC settings from imported operating system images. Dedicated workstations should operate in standard single-user mode to preserve user profile data and application settings between sessions. All standard user profile and session management behaviors remain active when Shared PC mode is disabled.",
                Tags = ["shared-pc", "kiosk", "profiles", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSharedPCMode", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSharedPCMode")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSharedPCMode", 0)],
            },
            new TweakDef
            {
                Id = "shpc-zero-disk-deletion-level",
                Label = "Disable Disk Level Deletion",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Shared PC disk level deletion determines how aggressively user profiles and cached data are removed when disk space falls below configured thresholds. Setting disk level deletion to zero disables automatic account and profile deletion based on disk pressure. Dedicated workstations with persistent user profiles require that data not be deleted without explicit administrative action. Automatic deletion of user profiles in shared PC mode can cause data loss if users inadvertently leave unsynchronized files on the local device. Enterprises managing user data retention policies should handle profile lifecycle through MDM or Group Policy rather than automatic deletion thresholds. This setting preserves user data integrity on devices transitioned out of shared mode.",
                Tags = ["shared-pc", "disk", "profiles", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DiskLevelDeletion", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DiskLevelDeletion")],
                DetectOps = [RegOp.CheckDword(Key, "DiskLevelDeletion", 0)],
            },
            new TweakDef
            {
                Id = "shpc-zero-disk-caching-level",
                Label = "Disable Disk Level Caching",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Shared PC disk level caching controls the threshold at which the shared PC cleanup service begins compressing or removing cached profile data. Setting disk level caching to zero disables threshold-triggered caching operations managed by the Shared PC service. Dedicated workstations do not require disk caching thresholds as profile cleanup is managed through standard operating system mechanisms. Shared PC caching behaviors can unexpectedly compress user profile directories, causing application state loss. Disabling this threshold prevents the Shared PC cache management service from interfering with normal profile operations. Standard Windows disk management and profile management policies govern storage usage when shared PC caching is off.",
                Tags = ["shared-pc", "caching", "profiles", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DiskLevelCaching", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DiskLevelCaching")],
                DetectOps = [RegOp.CheckDword(Key, "DiskLevelCaching", 0)],
            },
            new TweakDef
            {
                Id = "shpc-zero-inactive-threshold",
                Label = "Disable Inactive User Threshold",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The inactive threshold in Shared PC mode determines how many days of inactivity trigger automatic account deletion during maintenance windows. Setting this to zero disables time-based account deletion entirely in the Shared PC policy framework. Dedicated workstations maintain persistent user profiles and should not automatically delete accounts based on inactivity. User account lifecycle management on dedicated devices is handled through Active Directory and HR-driven deprovisioning processes. Automatic account deletion without enterprise coordination can violate data governance and audit requirements. This setting ensures user accounts persist until explicitly deprovisioned through proper IT workflows.",
                Tags = ["shared-pc", "accounts", "profiles", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "InactiveThreshold", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "InactiveThreshold")],
                DetectOps = [RegOp.CheckDword(Key, "InactiveThreshold", 0)],
            },
            new TweakDef
            {
                Id = "shpc-zero-max-page-file-mb",
                Label = "Disable Shared PC Max Page File Size",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Shared PC mode can impose a maximum page file size restriction to conserve disk space when multiple user profiles share limited storage. Setting this to zero removes the Shared PC imposed page file size ceiling. Dedicated workstations managed by separate page file policies should not have an additional Shared PC constraint overriding the configured page file size. Conflicting page file size policies can result in insufficient virtual memory for workloads that exceed the Shared PC imposed ceiling. Dedicated workstation page file sizing should be governed by the PageFile policy settings or system defaults exclusively. Removing the Shared PC page file restriction ensures only the authoritative page file policy applies.",
                Tags = ["shared-pc", "pagefile", "memory", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxPageFileSizeMB", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxPageFileSizeMB")],
                DetectOps = [RegOp.CheckDword(Key, "MaxPageFileSizeMB", 0)],
            },
            new TweakDef
            {
                Id = "shpc-delete-guest-on-logoff",
                Label = "Delete Guest Account on Logoff",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Guest accounts created for temporary access accumulate profile data and application state that persists across sessions if not cleaned up. Enabling deletion of the guest account on logoff ensures no data from guest sessions persists on the device after the guest logs out. This policy is a security best practice for devices used in public or semi-public environments where guest access is permitted. Residual guest profile data could contain sensitive information browsed or downloaded during the guest session. Devices in public access areas such as lobbies, libraries, and conference rooms benefit most from automatic guest cleanup. Combining this setting with other Shared PC policies creates a comprehensive ephemeral session environment.",
                Tags = ["shared-pc", "guest", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DeleteGuestAccountOnLogoff", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DeleteGuestAccountOnLogoff")],
                DetectOps = [RegOp.CheckDword(Key, "DeleteGuestAccountOnLogoff", 1)],
            },
            new TweakDef
            {
                Id = "shpc-restrict-local-storage",
                Label = "Restrict Local Storage Access",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Restricting local storage in Shared PC environments prevents users from saving large amounts of data to the local disk, preserving space for system operations. This policy is particularly important for shared devices where multiple user profiles compete for limited storage capacity. Combining local storage restriction with cloud synchronization policies directs user data to centrally managed repositories. Shared devices in classroom or kiosk configurations benefit from storage restrictions that prevent individual users from consuming the entire disk. Users on restricted devices still have access to their cloud-synchronized documents and files through mobile clients. The restriction does not prevent normal application usage, only limits the growth of user-created local content.",
                Tags = ["shared-pc", "storage", "kiosk", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictLocalStorage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictLocalStorage")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictLocalStorage", 1)],
            },
            new TweakDef
            {
                Id = "shpc-disable-enabled-flag",
                Label = "Disable Shared PC Enabled Flag",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "The Enabled flag in Shared PC policy acts as a master switch that activates all other Shared PC behaviors including account management and cleanup. Setting this flag to zero disables the entire Shared PC policy framework on the device. Dedicated workstations should have Shared PC mode fully disabled to prevent any unintended overlap with shared device behaviors. Disabling the Enabled flag supersedes other individual Shared PC policy settings. This setting is appropriate as a cleanup measure when migrating devices from shared configurations to dedicated single-user deployments. Standard enterprise workstation management takes over all session and account management when Shared PC is fully disabled.",
                Tags = ["shared-pc", "kiosk", "accounts", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                DetectOps = [RegOp.CheckDword(Key, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "shpc-clear-kiosk-aumid",
                Label = "Clear Kiosk Mode Application ID",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The Kiosk Mode AUMID specifies the application that runs in full-screen kiosk mode when Shared PC kiosk configuration is active. Clearing this value removes any configured kiosk application assignment from the Shared PC policy. Dedicated workstations operating in standard desktop mode should not have a kiosk AUMID configured. An accidentally inherited kiosk AUMID can cause unexpected single-application lockout behavior if Shared PC mode is re-enabled. Clearing this value ensures devices transitioned from kiosk to standard configuration do not retain kiosk application assignments. Standard multi-application desktop behavior is preserved when no AUMID is configured.",
                Tags = ["shared-pc", "kiosk", "applications", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "KioskModeAUMID", "")],
                RemoveOps = [RegOp.DeleteValue(Key, "KioskModeAUMID")],
                DetectOps = [RegOp.CheckString(Key, "KioskModeAUMID", "")],
            },
            new TweakDef
            {
                Id = "shpc-require-signin-on-resume",
                Label = "Require Sign-In on Resume",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Requiring sign-in on resume from sleep or hibernation ensures that unauthorized users cannot access a shared device left unattended in a locked state. This policy enforces password prompt on wake for all users on shared devices, preventing unauthorized session hijacking. Shared public access devices are particularly vulnerable to unauthorized access between legitimate user sessions. Combined with short screen lock timeouts, this setting provides a strong access control baseline for multi-user environments. Sign-in on resume also ensures any screen content from the previous session is cleared before the new user can view the display. This security measure is aligned with CIS benchmark recommendations for shared computing environments.",
                Tags = ["shared-pc", "security", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SignInOnResume", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SignInOnResume")],
                DetectOps = [RegOp.CheckDword(Key, "SignInOnResume", 1)],
            },
        ];
    }

    // ── WindowsAutopilotPolicy ──
    private static class _WindowsAutopilotPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Autopilot";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wpautopilot-block-oobe-cortana",
                    Label = "Autopilot: Suppress Cortana Voice Assistant During OOBE Provisioning",
                    Category = "System — Shared P C",
                    Description =
                        "Sets DisableCortanaInOOBE=1 in Autopilot policy. Prevents Cortana's voice-guided OOBE assistant from launching during the Windows Out-Of-Box Experience on Autopilot-provisioned devices, eliminating unexpected voice output and microphone access during unattended provisioning. "
                        + "During self-deploying Autopilot provisioning, the device may go through OOBE phases unattended. Cortana's voice interface launching during an unattended provisioning session can trigger unexpected audio output (speakers active) and request microphone access, which is unnecessary and potentially alarming in secure staging environments. Suppressing Cortana during OOBE ensures silent, predictable provisioning.",
                    Tags = ["autopilot", "oobe", "cortana", "provisioning", "silent"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Cortana suppressed during OOBE; Autopilot provisioning completes silently without voice prompts.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCortanaInOOBE", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCortanaInOOBE")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCortanaInOOBE", 1)],
                },
                new TweakDef
                {
                    Id = "wpautopilot-require-tpm-attestation",
                    Label = "Autopilot: Require TPM Attestation Before Autopilot Pre-Provisioning Completes",
                    Category = "System — Shared P C",
                    Description =
                        "Sets RequireTPMAttestation=1 in Autopilot policy. Requires that the device's TPM chip successfully completes attestation with the Microsoft Attestation Service before Autopilot White Glove pre-provisioning is allowed to complete, ensuring only machines with healthy TPM chips receive the provisioning credential blob. "
                        + "Autopilot White Glove pre-provisioning downloads and installs applications and policies during the Technician Phase. If TPM attestation is not required, a device with a non-functional or tampered TPM can still be fully provisioned and shipped to an end user with an enterprise credential blob. Requiring TPM attestation ensures only hardware with a verified, healthy TPM is provisioned, supporting BitLocker and Windows Hello for Business.",
                    Tags = ["autopilot", "tpm", "attestation", "white-glove", "hardware-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "TPM attestation required; Autopilot White Glove fails for devices with non-functional or tampered TPM.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireTPMAttestation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireTPMAttestation")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireTPMAttestation", 1)],
                },
                new TweakDef
                {
                    Id = "wpautopilot-block-language-selection-in-oobe",
                    Label = "Autopilot: Skip Language and Region Selection in OOBE (Silent Provisioning)",
                    Category = "System — Shared P C",
                    Description =
                        "Sets SkipLanguageAndRegion=1 in Autopilot policy. Skips the language selection, keyboard layout, and region selection screens during OOBE, using the locale settings pre-configured in the Autopilot deployment profile instead of prompting the user or technician during provisioning. "
                        + "Self-deploying Autopilot profiles target unattended provisioning. Any OOBE screen that blocks at a user input prompt (language, region) halts the provisioning workflow until answered. In staging environments where devices are provisioned in bulk on racks, unexpected OOBE prompts that require per-device interaction break the automation, requiring manual intervention on each device.",
                    Tags = ["autopilot", "oobe", "language", "silent", "unattended"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Language/region OOBE screens skipped; Autopilot provisioning uses profile locale settings without prompt.",
                    ApplyOps = [RegOp.SetDword(Key, "SkipLanguageAndRegion", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SkipLanguageAndRegion")],
                    DetectOps = [RegOp.CheckDword(Key, "SkipLanguageAndRegion", 1)],
                },
                new TweakDef
                {
                    Id = "wpautopilot-disable-privacy-settings-screen",
                    Label = "Autopilot: Skip Privacy Settings Screen in OOBE",
                    Category = "System — Shared P C",
                    Description =
                        "Sets DisablePrivacySettingsInOOBE=1 in Autopilot policy. Suppresses the privacy settings configuration screen that appears during OOBE, where Windows presents toggles for diagnostic data, location, speech recognition, and ink/typing personalisation, using enterprise policy defaults instead. "
                        + "The OOBE privacy settings screen presents users and technicians with a series of toggle choices that may override enterprise Group Policy settings if the user makes incorrect selections during provisioning. By skipping this screen and applying privacy settings via Group Policy or Intune configuration profiles, the enterprise ensures the device always meets its defined privacy configuration baseline from first boot.",
                    Tags = ["autopilot", "oobe", "privacy", "provisioning", "baseline"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "OOBE privacy settings screen skipped; enterprise policy controls privacy toggles rather than OOBE user selection.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePrivacySettingsInOOBE", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivacySettingsInOOBE")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePrivacySettingsInOOBE", 1)],
                },
                new TweakDef
                {
                    Id = "wpautopilot-enable-secure-diagnostics-upload",
                    Label = "Autopilot: Enable Secure Diagnostic Log Upload on Provisioning Failure",
                    Category = "System — Shared P C",
                    Description =
                        "Sets EnableDiagnosticsUploadOnFailure=1 in Autopilot policy. Enables automatic upload of diagnostic logs to the Microsoft Intune service when Autopilot provisioning fails, allowing IT admins to review failure details in the Intune admin center without physical access to the device. "
                        + "Autopilot provisioning failures in the field (enrolled device failing to complete provisioning at an employee's desk) are difficult to diagnose without the detailed log files stored on the device. Without automatic log upload, IT must either collect logs manually (requiring physical access or remote PowerShell) or rely on the user to capture and submit logs. Enabling automatic upload on failure provides actionable failure diagnostics in the admin portal.",
                    Tags = ["autopilot", "diagnostics", "failure", "logging", "intune"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Autopilot failure logs uploaded automatically to Intune; no physical access needed for provisioning failure diagnostics.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableDiagnosticsUploadOnFailure", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableDiagnosticsUploadOnFailure")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableDiagnosticsUploadOnFailure", 1)],
                },
                new TweakDef
                {
                    Id = "wpautopilot-block-manual-hardware-hash-upload",
                    Label = "Autopilot: Block Manual Hardware Hash Upload by Non-Administrators",
                    Category = "System — Shared P C",
                    Description =
                        "Sets DisableManualHardwareHashUpload=1 in Autopilot policy. Prevents standard users from manually running scripts or PowerShell commands that collect the device's hardware hash and upload it to the Autopilot service, restricting hardware hash registration to OEM upload and IT admin-initiated processes. "
                        + "Hardware hash registration is the authoritative step that associates a physical device with an Autopilot deployment profile. If standard users can run scripts to upload hardware hashes of arbitrary devices (including virtual machines running on personal hardware), they may register personal devices into the enterprise Autopilot service, bootstrapping them with enterprise policies, certificates, and credentials.",
                    Tags = ["autopilot", "hardware-hash", "registration", "unauthorised", "admin"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Manual hardware hash upload blocked for standard users; only OEM/IT admin can register devices.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableManualHardwareHashUpload", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableManualHardwareHashUpload")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableManualHardwareHashUpload", 1)],
                },
                new TweakDef
                {
                    Id = "wpautopilot-enable-provisioning-audit-log",
                    Label = "Autopilot: Enable Security Audit Log for Autopilot Provisioning Events",
                    Category = "System — Shared P C",
                    Description =
                        "Sets EnableProvisioningAuditLog=1 in Autopilot policy. Causes a Security event log entry to be written at each stage of the Autopilot provisioning workflow (device registration, Entra ID join, MDM enrollment, application installation) including the result and any error codes. "
                        + "Without provisioning audit logging, there is no on-device Security event record of what happened during Autopilot provisioning — only the results visible in the Intune admin portal. Having on-device event log entries for each provisioning stage enables post-incident forensics if a device's provisioning state is questioned (e.g., whether a specific application or configuration was applied correctly during the initial setup).",
                    Tags = ["autopilot", "audit", "provisioning", "event-log", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Security event log entries written at each Autopilot provisioning stage; on-device provisioning history available.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableProvisioningAuditLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableProvisioningAuditLog")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableProvisioningAuditLog", 1)],
                },
                new TweakDef
                {
                    Id = "wpautopilot-require-enrolled-device-for-provisioning",
                    Label = "Autopilot: Require Device Pre-Registration Before OOBE Autopilot Profile Download",
                    Category = "System — Shared P C",
                    Description =
                        "Sets RequirePreRegistration=1 in Autopilot policy. Enforces that the device must be pre-registered in the Autopilot service (via hardware hash) before the OOBE Autopilot profile download proceeds, blocking provisioning of devices that have not been explicitly registered by IT. "
                        + "Without pre-registration enforcement, an unregistered device going through OOBE on the same network as a registered device might accidentally receive an Autopilot profile due to subnet-based profile assignment misconfiguration. Requiring explicit pre-registration ensures that Autopilot profiles are only applied to known, IT-registered hardware and not to devices that are accidentally discoverable.",
                    Tags = ["autopilot", "pre-registration", "oobe", "hardware", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Autopilot provisioning blocked for non-registered hardware; only pre-enrolled devices receive provisioning profiles.",
                    ApplyOps = [RegOp.SetDword(Key, "RequirePreRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequirePreRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "RequirePreRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "wpautopilot-block-oobe-skip-button",
                    Label = "Autopilot: Remove OOBE Skip/Cancel Button to Prevent Provisioning Abandonment",
                    Category = "System — Shared P C",
                    Description =
                        "Sets DisableSkipButtonInOOBE=1 in Autopilot policy. Removes the 'Skip' and 'Cancel' buttons from Autopilot OOBE screens that would allow a user or technician to abort the provisioning workflow before it completes, ensuring devices are always fully provisioned before being usable. "
                        + "OOBE Skip buttons allow a technician or user to abandon Autopilot provisioning mid-way through, leaving the device in a partially configured state with some apps installed and others not, MDM enrollment incomplete, and security baselines potentially unapplied. A partially provisioned device may appear to work normally while critical security configurations are absent.",
                    Tags = ["autopilot", "oobe", "skip", "provisioning", "incomplete"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "OOBE skip/cancel buttons removed; Autopilot provisioning must complete before device becomes usable.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSkipButtonInOOBE", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSkipButtonInOOBE")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSkipButtonInOOBE", 1)],
                },
                new TweakDef
                {
                    Id = "wpautopilot-set-provisioning-timeout-90min",
                    Label = "Autopilot: Set Autopilot Enrollment Status Page Timeout to 90 Minutes",
                    Category = "System — Shared P C",
                    Description =
                        "Sets EnrollmentStatusPageTimeout=90 in Autopilot policy. Sets the Autopilot Enrollment Status Page (ESP) timeout — the maximum time the ESP will wait for app and policy installation to complete before triggering an error — to 90 minutes. "
                        + "The default ESP timeout is 60 minutes. In enterprise environments with large required application sets or slow network segments (branch office with limited bandwidth), the app installation phase can exceed 60 minutes especially for large apps delivered via Intune Win32 app deployment (LOB apps with 500 MB+ installers). An ESP timeout before provisioning completes leaves the device in an error state, triggering a factory reset. A 90-minute timeout accommodates larger app sets.",
                    Tags = ["autopilot", "esp", "timeout", "provisioning", "apps"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "ESP timeout extended to 90 minutes; large application packages have more time to complete installation during provisioning.",
                    ApplyOps = [RegOp.SetDword(Key, "EnrollmentStatusPageTimeout", 90)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnrollmentStatusPageTimeout")],
                    DetectOps = [RegOp.CheckDword(Key, "EnrollmentStatusPageTimeout", 90)],
                },
            ];
    }

    // ── WindowsDeploymentServicesPolicy ──
    private static class _WindowsDeploymentServicesPolicy
    {
        private const string SvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS\Server";
        private const string PxeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS\PXE";
        private const string TransKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS\Transport";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wds-require-admin-approval",
                    Label = "Require Admin Approval for PXE Boot Clients",
                    Category = "System — Shared P C",
                    Description =
                        "Requires administrator approval before unknown PXE clients can boot from WDS. Prevents unauthorised devices from imaging. Default: auto-approve.",
                    Tags = ["wds", "pxe", "security", "approval", "deployment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Unknown PXE clients are held pending admin approval; known-device imaging unaffected.",
                    ApplyOps = [RegOp.SetDword(PxeKey, "RequireAdminApproval", 1)],
                    RemoveOps = [RegOp.DeleteValue(PxeKey, "RequireAdminApproval")],
                    DetectOps = [RegOp.CheckDword(PxeKey, "RequireAdminApproval", 1)],
                },
                new TweakDef
                {
                    Id = "wds-disable-unknown-pxe",
                    Label = "Block Unknown Clients from PXE Boot",
                    Category = "System — Shared P C",
                    Description =
                        "Prevents unknown (non-pre-staged) computers from performing PXE boot via WDS. Only pre-staged/known devices can image. Default: allow all.",
                    Tags = ["wds", "pxe", "security", "unknown-clients", "deployment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "Only pre-staged devices can PXE boot; new devices must be pre-staged in AD first.",
                    ApplyOps = [RegOp.SetDword(PxeKey, "AllowUnknownClients", 0)],
                    RemoveOps = [RegOp.DeleteValue(PxeKey, "AllowUnknownClients")],
                    DetectOps = [RegOp.CheckDword(PxeKey, "AllowUnknownClients", 0)],
                },
                new TweakDef
                {
                    Id = "wds-enable-pxe-prompt",
                    Label = "Enable PXE Boot Key Press Prompt",
                    Category = "System — Shared P C",
                    Description =
                        "Requires the user to press a key (e.g., F12) to initiate PXE boot. Prevents automatic network boot on every startup. Default: may auto-boot.",
                    Tags = ["wds", "pxe", "prompt", "boot", "deployment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users must press a key to PXE boot; prevents accidental reimaging.",
                    ApplyOps = [RegOp.SetDword(PxeKey, "PxePromptPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(PxeKey, "PxePromptPolicy")],
                    DetectOps = [RegOp.CheckDword(PxeKey, "PxePromptPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "wds-set-pxe-timeout",
                    Label = "Set PXE Prompt Timeout to 10 Seconds",
                    Category = "System — Shared P C",
                    Description =
                        "Sets the PXE boot key-press prompt timeout to 10 seconds. After timeout, the device continues to local disk boot. Default: varies by BIOS.",
                    Tags = ["wds", "pxe", "timeout", "boot", "deployment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "10-second window for PXE boot; device falls through to local boot on timeout.",
                    ApplyOps = [RegOp.SetDword(PxeKey, "PxePromptTimeout", 10)],
                    RemoveOps = [RegOp.DeleteValue(PxeKey, "PxePromptTimeout")],
                    DetectOps = [RegOp.CheckDword(PxeKey, "PxePromptTimeout", 10)],
                },
                new TweakDef
                {
                    Id = "wds-enable-logging",
                    Label = "Enable WDS Deployment Event Logging",
                    Category = "System — Shared P C",
                    Description =
                        "Enables detailed event logging for WDS deployment operations. Provides audit trail of which devices were imaged and when. Default: minimal logging.",
                    Tags = ["wds", "logging", "audit", "deployment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Detailed WDS deployment events written to event log; slight disk overhead.",
                    ApplyOps = [RegOp.SetDword(SvcKey, "EnableLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "EnableLogging")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "EnableLogging", 1)],
                },
                new TweakDef
                {
                    Id = "wds-set-multicast-transfer-mode",
                    Label = "Set WDS Multicast Transfer to Auto Mode",
                    Category = "System — Shared P C",
                    Description =
                        "Configures multicast image transfers to auto-select between multicast and unicast based on network conditions. Default: multicast only.",
                    Tags = ["wds", "multicast", "network", "deployment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "WDS auto-selects best transfer mode; improves reliability on networks without multicast support.",
                    ApplyOps = [RegOp.SetDword(TransKey, "TransferMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(TransKey, "TransferMode")],
                    DetectOps = [RegOp.CheckDword(TransKey, "TransferMode", 1)],
                },
                new TweakDef
                {
                    Id = "wds-set-multicast-session-threshold",
                    Label = "Set Multicast Session Client Threshold to 10",
                    Category = "System — Shared P C",
                    Description =
                        "Sets the minimum number of clients before a multicast session starts. Prevents starting a multicast session for only 1–2 clients. Default: 1.",
                    Tags = ["wds", "multicast", "threshold", "deployment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Multicast waits for 10 clients before starting; single clients use unicast fallback.",
                    ApplyOps = [RegOp.SetDword(TransKey, "MulticastSessionThreshold", 10)],
                    RemoveOps = [RegOp.DeleteValue(TransKey, "MulticastSessionThreshold")],
                    DetectOps = [RegOp.CheckDword(TransKey, "MulticastSessionThreshold", 10)],
                },
                new TweakDef
                {
                    Id = "wds-enable-tftp-window-size",
                    Label = "Set WDS TFTP Block Size to 16384",
                    Category = "System — Shared P C",
                    Description =
                        "Increases the TFTP block size used by WDS PXE boot to 16384 bytes. Improves image download speed on modern networks. Default: 1456 (standard TFTP block).",
                    Tags = ["wds", "tftp", "performance", "pxe", "deployment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Faster PXE image downloads; may fail on networks with low MTU or NAT.",
                    ApplyOps = [RegOp.SetDword(TransKey, "TftpBlockSize", 16384)],
                    RemoveOps = [RegOp.DeleteValue(TransKey, "TftpBlockSize")],
                    DetectOps = [RegOp.CheckDword(TransKey, "TftpBlockSize", 16384)],
                },
                new TweakDef
                {
                    Id = "wds-disable-dhcp-option-60",
                    Label = "Disable DHCP Option 60 (PXEClient Class ID)",
                    Category = "System — Shared P C",
                    Description =
                        "Prevents WDS from adding DHCP Option 60 (PXEClient class identifier) to DHCP responses. Use when WDS is co-located with DHCP to avoid conflicts. Default: enabled.",
                    Tags = ["wds", "dhcp", "pxe", "network", "deployment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Prevents DHCP conflict when WDS and DHCP share the same server; PXE may need DHCP Option 66/67 instead.",
                    ApplyOps = [RegOp.SetDword(PxeKey, "UseDhcpPorts", 0)],
                    RemoveOps = [RegOp.DeleteValue(PxeKey, "UseDhcpPorts")],
                    DetectOps = [RegOp.CheckDword(PxeKey, "UseDhcpPorts", 0)],
                },
                new TweakDef
                {
                    Id = "wds-restrict-naming-policy",
                    Label = "Enforce WDS Computer Naming Policy",
                    Category = "System — Shared P C",
                    Description =
                        "Enforces a server-defined computer naming policy for imaged devices. Prevents users from choosing arbitrary computer names during imaging. Default: user-chosen.",
                    Tags = ["wds", "naming", "policy", "deployment", "standardisation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Imaged computers get server-defined names; ensures naming convention compliance.",
                    ApplyOps = [RegOp.SetDword(SvcKey, "NamingPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "NamingPolicy")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "NamingPolicy", 1)],
                },
            ];
    }

    // ── WindowsFlightedFeaturesPolicy ──
    private static class _WindowsFlightedFeaturesPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FlightedFeatures";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "flight-disable-feature-trials",
                Label = "Windows Flighted Features: Disable Feature Trials",
                Category = "System — Shared P C",
                Description =
                    "Prevents Windows from enrolling this device in feature trials via the flighting (A/B testing) mechanism. "
                    + "Feature trials push experimental or partially-ready features to a subset of devices without user opt-in. "
                    + "Disabling trials ensures only fully-released, validated features are active on enterprise endpoints. "
                    + "Removing this policy re-enables Microsoft's ability to push feature trial updates.",
                Tags = ["flighting", "feature-trial", "stability", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnabledFlightedFeatures", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnabledFlightedFeatures")],
                DetectOps = [RegOp.CheckDword(Key, "EnabledFlightedFeatures", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents experimental feature roll-outs; improves endpoint stability and predictability.",
            },
            new TweakDef
            {
                Id = "flight-block-preview-builds",
                Label = "Windows Flighted Features: Block Preview Build Features",
                Category = "System — Shared P C",
                Description =
                    "Prevents preview-ring feature flags from being activated on production endpoints via the flighting registry policy. "
                    + "Preview builds may include unstable code paths, driver compatibility issues, or features not yet hardened for enterprise use. "
                    + "Blocking preview feature activation is a standard practice in SOE (Standard Operating Environment) management. "
                    + "Removing this policy allows Microsoft flighting to selectively enable preview features on this device.",
                Tags = ["flighting", "preview", "windows-update", "stability", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePreviewFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePreviewFeatures")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePreviewFeatures", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks preview feature activation; reduces risk of unstable code on production machines.",
            },
            new TweakDef
            {
                Id = "flight-set-branch-readiness-semi-annual",
                Label = "Windows Flighted Features: Set Branch Readiness to Semi-Annual Channel",
                Category = "System — Shared P C",
                Description =
                    "Configures the Windows flighting branch readiness level to the Semi-Annual Channel (production ring). "
                    + "The branch readiness level controls which update ring the device belongs to — insider, beta, or release. "
                    + "Semi-Annual Channel (value 32) ensures only fully validated updates are offered. "
                    + "Removing this policy allows Windows Update to assign the device to its default ring.",
                Tags = ["flighting", "branch", "update-ring", "semi-annual", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BranchReadinessLevel", 32)],
                RemoveOps = [RegOp.DeleteValue(Key, "BranchReadinessLevel")],
                DetectOps = [RegOp.CheckDword(Key, "BranchReadinessLevel", 32)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks device to Semi-Annual Channel; prevents insider/beta feature ring enrollment.",
            },
            new TweakDef
            {
                Id = "flight-disable-diagnostic-data-upload",
                Label = "Windows Flighted Features: Disable Diagnostic Data Upload for Flights",
                Category = "System — Shared P C",
                Description =
                    "Disables the upload of diagnostic data specifically associated with flighted (experimental) feature usage. "
                    + "When a feature trial is active, Windows collects enhanced telemetry to evaluate the trial's effectiveness. "
                    + "This policy stops that additional telemetry while still permitting baseline diagnostic data. "
                    + "Removing this policy re-enables flight-specific enhanced diagnostic data collection.",
                Tags = ["flighting", "telemetry", "privacy", "diagnostic-data", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFlightDiagnosticData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFlightDiagnosticData")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFlightDiagnosticData", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Reduces telemetry associated with feature trials; improves privacy on managed endpoints.",
            },
            new TweakDef
            {
                Id = "flight-disable-experimentation",
                Label = "Windows Flighted Features: Disable A/B Experimentation",
                Category = "System — Shared P C",
                Description =
                    "Prevents Windows from applying A/B experimentation overrides via the flighting system. "
                    + "A/B experimentation can silently change UI layouts, default settings, or feature availability without the user's knowledge. "
                    + "On managed endpoints, unpredictable behaviour changes caused by experiments can interfere with helpdesk scripts and SOE policies. "
                    + "Removing this policy re-allows A/B experiments to be applied to this device.",
                Tags = ["flighting", "experimentation", "ab-test", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableExperimentation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableExperimentation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableExperimentation", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents silent A/B experiments; ensures consistent, predictable Windows behaviour.",
            },
            new TweakDef
            {
                Id = "flight-set-target-release-version",
                Label = "Windows Flighted Features: Set Target Release Version (24H2)",
                Category = "System — Shared P C",
                Description =
                    "Pins the device to Windows 11 24H2 as the target feature update version via the flighting policy. "
                    + "Pinning the target release prevents automatic upgrade to newer feature releases before IT validation is complete. "
                    + "This is critical in environments with validated SOE images and application compatibility dependencies. "
                    + "Removing this policy allows Windows Update to offer the next feature release when available.",
                Tags = ["flighting", "target-release", "version-pin", "feature-update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TargetReleaseVersion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TargetReleaseVersion")],
                DetectOps = [RegOp.CheckDword(Key, "TargetReleaseVersion", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Pins device to a target release; prevents unvetted feature update upgrades.",
            },
            new TweakDef
            {
                Id = "flight-disable-insider-content",
                Label = "Windows Flighted Features: Disable Insider Tip Content",
                Category = "System — Shared P C",
                Description =
                    "Blocks Windows Insider tip and promotional content pushed via the flighting infrastructure. "
                    + "Insider tips are shown in Start, Tips app, and Settings to encourage enrollment in the Insider Program. "
                    + "On enterprise endpoints this content is irrelevant and can distract users from productivity. "
                    + "Removing this policy re-enables Insider tip content delivery via the flighting system.",
                Tags = ["flighting", "insider", "tips", "content", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInsiderContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInsiderContent")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInsiderContent", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses Insider Program promotional content; cleaner enterprise desktop experience.",
            },
            new TweakDef
            {
                Id = "flight-disable-rollback-on-failure",
                Label = "Windows Flighted Features: Disable Automatic Rollback on Flight Failure",
                Category = "System — Shared P C",
                Description =
                    "Controls whether Windows automatically rolls back a failed flight update without administrator approval. "
                    + "Automatic rollback can interfere with change-management processes in enterprise environments where all changes must be audited. "
                    + "Disabling automatic rollback requires IT to explicitly approve any reversion action. "
                    + "Removing this policy re-enables Windows automatic rollback on flight failure.",
                Tags = ["flighting", "rollback", "change-management", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRollbackOnFailure", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRollbackOnFailure")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRollbackOnFailure", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents automatic silent rollback; keeps change-management audit trail intact.",
            },
            new TweakDef
            {
                Id = "flight-disable-feature-notifications",
                Label = "Windows Flighted Features: Disable Feature Notification Banners",
                Category = "System — Shared P C",
                Description =
                    "Suppresses notification banners introduced as part of flight updates — new feature announcements, upgrade prompts, and welcome screens. "
                    + "Flight-related notifications interrupt workflows and are inappropriate in a managed enterprise environment. "
                    + "This policy blocks those banners from appearing regardless of which features are active. "
                    + "Removing this policy re-enables flight-driven notification banners.",
                Tags = ["flighting", "notifications", "banners", "ux", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFeatureNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFeatureNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFeatureNotifications", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Stops flight-driven notification banners; reduces user interruptions on managed desktops.",
            },
            new TweakDef
            {
                Id = "flight-enforce-production-ring",
                Label = "Windows Flighted Features: Enforce Production Ring Only",
                Category = "System — Shared P C",
                Description =
                    "Forces the flighting infrastructure to treat this device as production-ring only, blocking all early-access feature assignments. "
                    + "In combination with BranchReadinessLevel, this ensures the device cannot be reclassified by Microsoft's backend assignment logic. "
                    + "Enforcing production ring is mandatory for PCI-DSS and SOX environments where any change to production systems requires prior approval. "
                    + "Removing this policy allows the backend to reclassify the device into any ring.",
                Tags = ["flighting", "production-ring", "compliance", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceProductionRing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceProductionRing")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceProductionRing", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks device to production ring permanently; critical for compliance-controlled endpoints.",
            },
        ];
    }

    // ── WindowsFlightingPolicy ──
    private static class _WindowsFlightingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "flight-disable-insider-preview",
                Label = "Disable Windows Insider Preview Enrollment",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Windows Insider Preview allows users to enroll their devices to receive pre-release Windows builds that are not yet generally available. Disabling Insider Preview enrollment prevents users from opting their devices into receiving unstable pre-release Windows builds. Preview builds may contain unfixed security vulnerabilities, missing patches, or experimental changes not appropriate for production environments. Insider builds do not receive the same security testing as general availability releases creating potential exposure to undisclosed vulnerabilities. Enterprise devices should run only tested and approved Windows builds deployed through managed update processes. Preventing insider enrollment ensures that enterprise endpoints remain on tested stable builds with complete security patch coverage.",
                Tags = ["flighting", "insider", "updates", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableConfigFlighting", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableConfigFlighting")],
                DetectOps = [RegOp.CheckDword(Key, "EnableConfigFlighting", 0)],
            },
            new TweakDef
            {
                Id = "flight-disable-preview-builds",
                Label = "Block Preview Build Installation",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Beyond enrollment control, Windows Update can be blocked from offering feature preview builds to enrolled users and devices. Blocking preview build installation provides an additional layer of protection ensuring that preview builds cannot be installed even if enrollment somehow occurs. Preview builds installed on enterprise devices create unsupported configurations that may not receive all security patches. IT change management processes require that OS upgrades be tested and validated before enterprise deployment. Preview builds can change system behavior, remove features, or alter security defaults in ways not accounted for by enterprise security baselines. Blocking preview installations ensures enterprise devices remain on the approved and tested configuration.",
                Tags = ["flighting", "insider", "updates", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowBuildPreview", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowBuildPreview")],
                DetectOps = [RegOp.CheckDword(Key, "AllowBuildPreview", 0)],
            },
            new TweakDef
            {
                Id = "flight-disable-config-flighting",
                Label = "Disable Windows Configuration Flighting",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Configuration flighting extends beyond build previews to include experimental feature toggles and configuration changes delivered through Microsoft's flighting infrastructure. Disabling configuration flighting prevents Microsoft from remotely toggling experimental Windows features on enterprise endpoints without IT awareness or approval. Configuration changes delivered through flighting can alter security settings, enable or disable features, or change system behavior. Enterprise security baselines assume specific feature configurations and flighting changes can invalidate baseline assumptions. IT must maintain awareness of all configuration changes on managed endpoints to ensure security policy compliance. Disabling configuration flighting ensures that the Windows configuration remains consistent with the IT-tested and approved baseline.",
                Tags = ["flighting", "configuration", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableConfigFlightingForFlights", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableConfigFlightingForFlights")],
                DetectOps = [RegOp.CheckDword(Key, "EnableConfigFlightingForFlights", 0)],
            },
            new TweakDef
            {
                Id = "flight-disable-telemetry-for-flighting",
                Label = "Disable Flighting Telemetry Uploads",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Flighting telemetry collects usage and diagnostic data from enrolled devices to help Microsoft evaluate preview feature quality and performance. Disabling flighting telemetry prevents upload of diagnostic and usage data associated with preview feature experiments. Flighting telemetry may include details about enterprise software usage, hardware configuration, and user behavior with experimental features. Sending enterprise endpoint telemetry to external parties without approval may violate enterprise data governance policies. Even on non-enrolled devices some flighting infrastructure may attempt to collect diagnostic data. Disabling flighting telemetry ensures that no preview-associated diagnostic data is transmitted from endpoints.",
                Tags = ["flighting", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFlightingTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFlightingTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFlightingTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "flight-disable-feature-rollout",
                Label = "Disable Gradual Feature Rollout",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Gradual feature rollouts deliver new features to a percentage of endpoints before full general availability. Disabling gradual feature rollout prevents selected endpoints from receiving new features ahead of the general release schedule. Endpoints receiving features early may have different behavior from other endpoints complicating support and security assessment. Early feature deployments may not have received complete security review and may expose new attack surfaces before hardening guidance is available. Enterprise environments benefit from predictable feature delivery through managed update processes rather than random selection for early rollout. Disabling gradual rollouts ensures consistent behavior across all enterprise endpoints at all times.",
                Tags = ["flighting", "features", "updates", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGradualRollout", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGradualRollout")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGradualRollout", 1)],
            },
            new TweakDef
            {
                Id = "flight-disable-experimental-features",
                Label = "Disable Experimental Feature Flags",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Experimental feature flags are toggles that can enable incomplete or tentative features that are not yet ready for general release. Disabling experimental feature flags prevents activation of features that may have security vulnerabilities, instabilities, or incomplete implementations. Experimental features may have bypassed the complete security review process that production features undergo before general availability. Enabling experimental flags can expose endpoints to attack vectors not present in released features without corresponding security guidance. Enterprise endpoints should only run features that have completed the full development, testing, and security review lifecycle. Experimental features can be evaluated in isolated sandbox environments by development and security teams without risk to production endpoints.",
                Tags = ["flighting", "experimental", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableExperimentalFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableExperimentalFeatures")],
                DetectOps = [RegOp.CheckDword(Key, "DisableExperimentalFeatures", 1)],
            },
            new TweakDef
            {
                Id = "flight-disable-a-b-testing",
                Label = "Disable A/B Feature Testing Participation",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Microsoft uses A/B testing to evaluate different user interface designs and feature implementations on randomly selected endpoints. Disabling A/B testing participation prevents endpoints from being selected to receive alternative interface designs or feature variants. A/B test subjects may receive features with different defaults or behaviors that deviate from the enterprise-approved baseline configuration. Security assessments and user training are developed for consistent interface implementations and A/B variants complicate these processes. Product feature changes affecting enterprise workflows should be introduced through IT-managed deployment cycles not random selection. Opting out of A/B testing ensures all enterprise endpoints receive the same consistent default Windows experience.",
                Tags = ["flighting", "ab-testing", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableABTesting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableABTesting")],
                DetectOps = [RegOp.CheckDword(Key, "DisableABTesting", 1)],
            },
            new TweakDef
            {
                Id = "flight-set-insider-ring",
                Label = "Set Windows Insider Ring to None",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Windows Insider has multiple rings from Dev Channel receiving the most experimental builds to Release Preview receiving near-release builds. Setting the insider ring to None ensures the endpoint is not associated with any insider channel and receives only generally available updates. Device assignment to any insider ring makes the endpoint eligible for pre-release builds regardless of other enrollment settings. Enterprise endpoints should not be affiliated with any insider ring to ensure they only receive production-quality builds. Insider ring assignments should be cleared to confirm no residual enrollment state persists from previous configurations. Setting the ring to None combined with disabling enrollment provides defense-in-depth against accidental preview build delivery.",
                Tags = ["flighting", "insider", "updates", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BranchReadinessLevel", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "BranchReadinessLevel")],
                DetectOps = [RegOp.CheckDword(Key, "BranchReadinessLevel", 0)],
            },
            new TweakDef
            {
                Id = "flight-disable-insider-program-settings",
                Label = "Disable Insider Program Settings Access",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The Insider Program section in Windows Settings provides users with the interface to enroll in or change insider program membership. Disabling access to Insider Program settings removes the user-accessible configuration page that controls insider enrollment. Hiding the settings page prevents accidental or deliberate enrollment by users who are unaware of enterprise policy against insider participation. Settings access removal is a complementary control to the enrollment block policy providing defense-in-depth. Users attempting to enroll through the settings page will receive a policy-blocked message rather than the enrollment interface. Administrative access to insider settings remains available for authorized IT change processes.",
                Tags = ["flighting", "insider", "ui", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInsiderProgramSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInsiderProgramSettings")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInsiderProgramSettings", 1)],
            },
            new TweakDef
            {
                Id = "flight-disable-optional-feature-updates",
                Label = "Disable Optional Preview Feature Updates",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows Update includes optional feature updates that users can choose to install before they become mandatory on a future update schedule. Disabling optional feature updates prevents users from installing new Windows features that have not been tested and approved by IT. Optional feature updates may include security-relevant changes that alter system behavior without IT awareness. Features received through optional updates may not be covered by enterprise security baselines creating undefined risk. Enterprise feature deployment should proceed through IT-managed testing and approval processes with appropriate scheduling. Preventing optional update installation ensures IT maintains control over the timing and content of feature changes on managed endpoints.",
                Tags = ["flighting", "updates", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOptionalFeatureUpdates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOptionalFeatureUpdates")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOptionalFeatureUpdates", 1)],
            },
        ];
    }

    // ── WindowsInsider ──
    private static class _WindowsInsider
    {
        private const string PreviewBuildsPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds";

        private const string DataCollection = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

        private const string CloudContent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

        private const string SelfHostApplicability = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsSelfHost\Applicability";

        private const string FeedbackRules = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Siuf\Rules";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "insider-disable-experimentation",
                Label = "Disable Windows Experimentation (A/B Feature Trials)",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["insider", "experimentation", "a/b testing", "telemetry"],
                Description =
                    "Prevents Windows from participating in Microsoft's experimentation framework "
                    + "used to validate new features before official release. "
                    + "EnableExperimentation=0 ensures a fully stable, non-experimental Windows build.",
                ApplyOps = [RegOp.SetDword(PreviewBuildsPolicy, "EnableExperimentation", 0)],
                RemoveOps = [RegOp.DeleteValue(PreviewBuildsPolicy, "EnableExperimentation")],
                DetectOps = [RegOp.CheckDword(PreviewBuildsPolicy, "EnableExperimentation", 0)],
            },
            new TweakDef
            {
                Id = "insider-set-retail-ring",
                Label = "Set Device to Retail (Non-Insider) Ring",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["insider", "ring", "retail", "preview builds"],
                Description =
                    "Sets the Windows Update ring to Retail via the WindowsSelfHost applicability "
                    + "registry. EnablePreviewBuilds=0 ensures this device uses stable production "
                    + "builds only, exiting any Insider ring it may have been enrolled in.",
                ApplyOps = [RegOp.SetDword(SelfHostApplicability, "EnablePreviewBuilds", 0)],
                RemoveOps = [RegOp.DeleteValue(SelfHostApplicability, "EnablePreviewBuilds")],
                DetectOps = [RegOp.CheckDword(SelfHostApplicability, "EnablePreviewBuilds", 0)],
            },
            new TweakDef
            {
                Id = "insider-disable-cloud-content-experience",
                Label = "Disable Cloud Content for Windows Suggestions",
                Category = "System — Shared P C",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["insider", "cloud content", "suggestions", "windows tips", "privacy"],
                Description =
                    "Disables cloud-delivered content shown in the Windows welcome experience, "
                    + "Settings highlights, and the first-run screen after major updates. "
                    + "DisableTailoredExperiencesWithDiagnosticData=1 prevents Clippy-style "
                    + "personalized suggestions based on diagnostics.",
                ApplyOps = [RegOp.SetDword(DataCollection, "DisableTailoredExperiencesWithDiagnosticData", 1)],
                RemoveOps = [RegOp.DeleteValue(DataCollection, "DisableTailoredExperiencesWithDiagnosticData")],
                DetectOps = [RegOp.CheckDword(DataCollection, "DisableTailoredExperiencesWithDiagnosticData", 1)],
            },
        ];
    }

    // ── WindowsServicingPolicy ──
    private static class _WindowsServicingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "winsvc-set-target-ga-release-channel",
                    Label = "Servicing: Set Windows Update for Business Channel to GA Release Channel",
                    Category = "System — Shared P C",
                    Description =
                        "Sets TargetReleaseVersionInfo=\"GA\" in WindowsUpdate policy. Configures Windows Update for Business to target the General Availability (GA) channel, ensuring the endpoint only receives fully released Windows 11/10 builds rather than Beta channel, Release Preview builds, or Insider Preview builds, providing the most stable update experience. "
                        + "Without an explicit channel configuration, a Windows endpoint may be enrolled in a Windows Insider Program channel from a previous administrator action and continue receiving pre-release builds. Pre-release builds are not covered by the standard Microsoft support lifecycle and may contain known stability regressions. Locking the endpoint to the GA channel ensures only fully supported, production-validated Windows builds are ever installed.",
                    Tags = ["windows-servicing", "release-channel", "ga", "insider", "update"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Windows Update locked to GA channel; pre-release Insider and beta builds cannot be installed.",
                    ApplyOps = [RegOp.SetString(Key, "TargetReleaseVersionInfo", "GA")],
                    RemoveOps = [RegOp.DeleteValue(Key, "TargetReleaseVersionInfo")],
                    DetectOps = [RegOp.CheckString(Key, "TargetReleaseVersionInfo", "GA")],
                },
                new TweakDef
                {
                    Id = "winsvc-defer-feature-updates-90-days",
                    Label = "Servicing: Defer Windows Feature Updates for 90 Days from GA Release",
                    Category = "System — Shared P C",
                    Description =
                        "Sets DeferFeatureUpdatesPeriodInDays=90 in WindowsUpdate policy. Delays the installation of Windows Feature Updates (major annual or semi-annual releases introducing new OS capabilities) by 90 days from the date they are first made publicly available, giving Microsoft time to issue compatibility fixes and giving IT time to complete validation and application compatibility testing. "
                        + "New Windows Feature Updates (e.g., Windows 11 version upgrades) introduce significant changes to the OS, including driver model changes, security changes, and UI modifications. Enterprises that immediately deploy new feature updates (0-day) routinely encounter application compatibility regressions, driver failures for specialised hardware, and Group Policy setting changes that require updated ADMX templates. A 90-day deferral provides buffer for Microsoft to release hotfixes and for enterprise IT to complete testing.",
                    Tags = ["windows-servicing", "feature-update", "deferral", "compatibility", "testing"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Feature updates deferred 90 days; Microsoft and IT have time to address compatibility issues before enterprise deployment.",
                    ApplyOps = [RegOp.SetDword(Key, "DeferFeatureUpdatesPeriodInDays", 90)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DeferFeatureUpdatesPeriodInDays")],
                    DetectOps = [RegOp.CheckDword(Key, "DeferFeatureUpdatesPeriodInDays", 90)],
                },
                new TweakDef
                {
                    Id = "winsvc-defer-quality-updates-7-days",
                    Label = "Servicing: Defer Windows Quality Updates for 7 Days to Allow Reliability Monitoring",
                    Category = "System — Shared P C",
                    Description =
                        "Sets DeferQualityUpdatesPeriodInDays=7 in WindowsUpdate policy. Delays the installation of Windows Quality Updates (monthly Patch Tuesday cumulative updates containing security fixes, reliability improvements, and bug fixes) by 7 days from their initial release to allow time for early-adopter reports to surface critical issues before enterprise-wide deployment. "
                        + "Monthly Patch Tuesday cumulative updates occasionally introduce regressions — caused by a security fix that changes underlying API behaviour or a reliability fix interacting unexpectedly with specific application configurations. In prior years, Patch Tuesday updates have introduced BSoDs for specific driver configurations, performance regressions in SMB file server workloads, and print spooler failures. A 7-day deferral allows Microsoft, the community, and independent testing labs to publish regression reports before the update reaches production endpoints.",
                    Tags = ["windows-servicing", "quality-update", "patch-tuesday", "deferral", "regression"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Quality updates deferred 7 days; regression reports communicated before production deployment.",
                    ApplyOps = [RegOp.SetDword(Key, "DeferQualityUpdatesPeriodInDays", 7)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DeferQualityUpdatesPeriodInDays")],
                    DetectOps = [RegOp.CheckDword(Key, "DeferQualityUpdatesPeriodInDays", 7)],
                },
                new TweakDef
                {
                    Id = "winsvc-disable-dual-scan",
                    Label = "Servicing: Disable WUfB Dual-Scan (WSUS + Windows Update Cloud Simultaneously)",
                    Category = "System — Shared P C",
                    Description =
                        "Sets DisableDualScan=1 in WindowsUpdate policy. Prevents Windows Update for Business from simultaneously scanning both the corporate WSUS server and the Windows Update cloud service for updates, restricting update source to the configured primary source only (typically WSUS). Without this setting, endpoints configured with both WSUS and WUfB policies may accidentally install cloud-sourced updates that haven't been approved in WSUS. "
                        + "WSUS environments use update approval workflows to prevent unapproved patches from installing. Windows Update for Business cloud scanning bypasses WSUS approval workflows — an update that is DECLINED in WSUS may still install if the endpoint simultaneously scans and finds the update approved in the Windows Update cloud service. Dual scan effectively breaks WSUS update governance by allowing cloud updates to supersede WSUS-declined updates.",
                    Tags = ["windows-servicing", "dual-scan", "wsus", "wufb", "update-governance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Dual-scan disabled; updates only sourced from configured primary (WSUS/WUfB); cloud updates cannot bypass WSUS approval.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDualScan", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDualScan")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDualScan", 1)],
                },
                new TweakDef
                {
                    Id = "winsvc-block-preview-builds",
                    Label = "Servicing: Block Windows Preview Builds and Insider Preview Enrollment",
                    Category = "System — Shared P C",
                    Description =
                        "Sets ManagePreviewBuilds=1 in WindowsUpdate policy. Prevents Windows from accessing Insider Preview builds, blocks the Windows Insider Program from enrolling the device, and hides the 'Windows Insider Program' section from Settings > Windows Update, making it impossible for users or administrators to opt into Insider Preview channels that would replace the production OS with a pre-release build. "
                        + "Windows Insider Program enrolment replaces the production Windows build with a pre-release build that may have known critical vulnerabilities (disclosed during the Insider period), removed security features under development, or APIs with breaking changes from the production build. On enterprise endpoints, any path that allows downgrading from a supported production build to an unsupported pre-release build bypasses the enterprise's patching SLA and software support commitments.",
                    Tags = ["windows-servicing", "insider-preview", "preview-builds", "insider-program", "lockdown"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Windows Insider Program blocked; device cannot be enrolled in preview channels or receive pre-release builds.",
                    ApplyOps = [RegOp.SetDword(Key, "ManagePreviewBuilds", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ManagePreviewBuilds")],
                    DetectOps = [RegOp.CheckDword(Key, "ManagePreviewBuilds", 1)],
                },
                new TweakDef
                {
                    Id = "winsvc-exclude-drivers-from-quality-updates",
                    Label = "Servicing: Exclude Driver Updates from Monthly Quality Update Package",
                    Category = "System — Shared P C",
                    Description =
                        "Sets ExcludeWUDriversInQualityUpdate=1 in WindowsUpdate policy. Prevents Windows Update for Business from installing driver updates as part of the monthly cumulative quality update package, requiring that driver updates are sourced and approved separately through the driver management pipeline rather than being bundled into the OS quality update. "
                        + "Driver updates bundled into Windows quality updates have been a source of hardware compatibility regressions, particularly for specialised peripherals, storage controllers, and graphics subsystems. A mandatory driver update included in a cumulative update may replace a tested, stable OEM driver with a Microsoft-provided inbox driver that behaves differently for specific hardware configurations. Excluding drivers from quality updates allows IT to validate and approve driver updates independently on a slower cadence.",
                    Tags = ["windows-servicing", "drivers", "quality-update", "regression", "driver-management"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Driver updates excluded from quality update packages; drivers validated and deployed on separate IT-controlled schedule.",
                    ApplyOps = [RegOp.SetDword(Key, "ExcludeWUDriversInQualityUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ExcludeWUDriversInQualityUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "ExcludeWUDriversInQualityUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "winsvc-block-optional-content-updates",
                    Label = "Servicing: Block Optional Windows Content Updates (Media Features, Language Packs)",
                    Category = "System — Shared P C",
                    Description =
                        "Sets AllowOptionalContent=0 in WindowsUpdate policy. Prevents Windows Update from automatically downloading and installing optional content updates — including optional feature updates, language experience packs, optional cumulative update components, and regional supplemental content packs — without explicit IT administrator approval for each optional package. "
                        + "Optional content includes media feature packs, additional language support, and supplemental features that Microsoft offers but does not install by default. While largely benign, optional content can consume hundreds of MB of disk space per package and is not required for enterprise operation. In disk-constrained environments (VDI thin clients, 128 GB endpoint SSDs) or bandwidth-constrained environments (WAN-connected branch offices), automatic download of optional content packages creates unnecessary overhead without enterprise benefit.",
                    Tags = ["windows-servicing", "optional-content", "language-packs", "disk-space", "bandwidth"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Optional Windows content updates blocked; language packs and optional features not auto-downloaded.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowOptionalContent", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowOptionalContent")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowOptionalContent", 0)],
                },
                new TweakDef
                {
                    Id = "winsvc-set-readiness-level-general-availability",
                    Label = "Servicing: Set Branch Readiness Level to General Availability Channel",
                    Category = "System — Shared P C",
                    Description =
                        "Sets BranchReadinessLevel=16 in WindowsUpdate policy. Sets the Windows Update for Business readiness level (deployment ring) to General Availability Channel (value 16), directing the endpoint to receive feature updates only after they have been on the General Availability channel for the configured deferral period, rather than from the Beta or Release Preview channels. "
                        + "BranchReadinessLevel determines which update channel feeds feature update availability. A value of 2 selects the Release Preview channel; 16 selects General Availability. Enterprises that configure WUfB without explicitly setting the readiness level may receive updates from the Release Preview channel, which contains builds that are near-final but may still have issues resolved between Release Preview and GA. Explicit GA targeting closes this gap.",
                    Tags = ["windows-servicing", "branch-readiness", "ga-channel", "feature-update", "wufb"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WUfB readiness level set to GA Channel (16); only fully released feature updates are eligible for deployment.",
                    ApplyOps = [RegOp.SetDword(Key, "BranchReadinessLevel", 16)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BranchReadinessLevel")],
                    DetectOps = [RegOp.CheckDword(Key, "BranchReadinessLevel", 16)],
                },
                new TweakDef
                {
                    Id = "winsvc-enable-safe-os-update-rollback",
                    Label = "Servicing: Enable SafeOS Update Rollback on Feature Update Failure Detection",
                    Category = "System — Shared P C",
                    Description =
                        "Sets EnableSafeOSUpdateRollback=1 in WindowsUpdate policy. Enables the Windows Safe OS rollback mechanism for failed feature updates. When a feature update installation fails (BSoD during upgrade, driver incompatibility detected, boot loop), Windows automatically rolls back to the previous working build rather than leaving the endpoint in an unbootable or partially-upgraded state. "
                        + "Feature update installation failures can leave an endpoint in a state where it has partially installed the new version but cannot boot successfully. Without SafeOS rollback enabled, the endpoint may enter a boot repair loop, requiring IT to perform manual recovery (recovery console, reimaging). With SafeOS rollback, Windows detects the boot failure and automatically recovers to the last known good state, minimising end-user downtime and IT support demand for failed feature update deployments.",
                    Tags = ["windows-servicing", "rollback", "feature-update", "safeos", "recovery"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SafeOS rollback enabled; failed feature updates auto-revert to previous working build without manual IT intervention.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableSafeOSUpdateRollback", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableSafeOSUpdateRollback")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableSafeOSUpdateRollback", 1)],
                },
                new TweakDef
                {
                    Id = "winsvc-enable-compliance-deadline-enforcement",
                    Label = "Servicing: Enable Compliance Deadline Enforcement to Prevent Indefinite Update Deferral",
                    Category = "System — Shared P C",
                    Description =
                        "Sets EnableComplianceDeadlineEnforcement=1 in WindowsUpdate policy. Enables the WUfB compliance deadline mechanism, which automatically enforces update installation (overriding user-controlled active hours and post-deadline deferral settings) when a security update has been available beyond the configured deadline period, ensuring security patches cannot be deferred indefinitely by end-users. "
                        + "Windows Update for Business user deadline controls allow end-users to dismiss and defer reboot prompts after updates are downloaded. In environments without compliance deadline enforcement, a user who repeatedly dismisses reboot prompts can delay security patch installation for weeks or months. The compliance deadline enforcement mechanism ensures that regardless of user behaviour, a security update that has been downloaded for more than the configured deadline period (typically 3–7 days) will install on the next restart.",
                    Tags = ["windows-servicing", "compliance-deadline", "security-patch", "forced-reboot", "sla"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Compliance deadline enforcement active; security updates cannot be deferred indefinitely by end-users; SLA enforced.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableComplianceDeadlineEnforcement", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableComplianceDeadlineEnforcement")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableComplianceDeadlineEnforcement", 1)],
                },
            ];
    }

    // ── WindowsToGoPolicy ──
    private static class _WindowsToGoPolicy
    {
        private const string WtgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PortableOperatingSystem";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wtg-disable-sleep",
                Label = "Disable Sleep States for Windows To Go",
                Category = "System — Shared P C",
                Description =
                    "Sets EnableSleep=0 in the PortableOperatingSystem policy key. "
                    + "Prevents Windows To Go workspaces from entering S1-S3 sleep states while running "
                    + "from a USB drive. Sleep states on WTG disks can corrupt the workspace if the USB "
                    + "connection is interrupted during wake-up. Applying this ensures the system either "
                    + "stays awake or shuts down completely, never entering an intermediate sleep state "
                    + "when running from a WTG workspace. Default: absent (sleep allowed).",
                Tags = ["windows-to-go", "sleep", "usb", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Sleep states disabled for WTG workspaces; prevents USB-to-sleep corruption scenarios.",
                ApplyOps = [RegOp.SetDword(WtgKey, "EnableSleep", 0)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "EnableSleep")],
                DetectOps = [RegOp.CheckDword(WtgKey, "EnableSleep", 0)],
            },
            new TweakDef
            {
                Id = "wtg-disable-hibernation",
                Label = "Disable Hibernation for Windows To Go",
                Category = "System — Shared P C",
                Description =
                    "Sets EnableHibernation=0 in the PortableOperatingSystem policy key. "
                    + "Prevents Windows To Go workspaces from using the hibernate (S4) power state. "
                    + "Hibernation on a WTG USB workspace saves RAM to the hiberfil.sys on the USB disk, "
                    + "but wake-up can fail if the USB drive is moved or the system firmware changes. "
                    + "Disabling hibernate avoids this by requiring a full shutdown instead. "
                    + "Default: absent (hibernation allowed).",
                Tags = ["windows-to-go", "hibernation", "hibernate", "usb", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hibernation disabled for WTG workspaces; prevents hiberfil corruption on USB devices.",
                ApplyOps = [RegOp.SetDword(WtgKey, "EnableHibernation", 0)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "EnableHibernation")],
                DetectOps = [RegOp.CheckDword(WtgKey, "EnableHibernation", 0)],
            },
            new TweakDef
            {
                Id = "wtg-block-workspace-creation",
                Label = "Block Windows To Go Workspace Creation",
                Category = "System — Shared P C",
                Description =
                    "Sets NoWorkspaceCreation=1 in the PortableOperatingSystem policy key. "
                    + "Prevents users from using the Windows To Go Workspace Creator wizard to create "
                    + "new WTG workspaces from this machine. Ensures WTG environments are only created "
                    + "by IT administrators and not by standard users who may inadvertently copy "
                    + "sensitive corporate data to an unmanaged USB drive. "
                    + "Default: absent (creation allowed). Recommended: 1 on managed corporate endpoints.",
                Tags = ["windows-to-go", "workspace-creation", "usb", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WTG workspace creation wizard blocked; only IT-created workspaces can be used.",
                ApplyOps = [RegOp.SetDword(WtgKey, "NoWorkspaceCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "NoWorkspaceCreation")],
                DetectOps = [RegOp.CheckDword(WtgKey, "NoWorkspaceCreation", 1)],
            },
            new TweakDef
            {
                Id = "wtg-block-boot-from-external",
                Label = "Block Booting From External WTG Media",
                Category = "System — Shared P C",
                Description =
                    "Sets BlockBootFromExternalMedia=1 in the PortableOperatingSystem policy key. "
                    + "Prevents this machine from booting a Windows To Go workspace from external USB media. "
                    + "Ensures the machine always boots its internal Windows installation and cannot be "
                    + "redirected by an inserted WTG USB drive. Protects against using WTG to bypass local "
                    + "security controls or Intune/Group Policy enrollment. "
                    + "Default: absent (booting from external media allowed).",
                Tags = ["windows-to-go", "boot", "usb", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WTG boot from external USB media blocked; internal Windows always boots instead.",
                ApplyOps = [RegOp.SetDword(WtgKey, "BlockBootFromExternalMedia", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "BlockBootFromExternalMedia")],
                DetectOps = [RegOp.CheckDword(WtgKey, "BlockBootFromExternalMedia", 1)],
            },
            new TweakDef
            {
                Id = "wtg-disable-host-offline-folders",
                Label = "Disable Host Offline Folders in Windows To Go",
                Category = "System — Shared P C",
                Description =
                    "Sets NoOfflineFolders=1 in the PortableOperatingSystem policy key. "
                    + "Prevents the WTG workspace from accessing the host machine's Offline Files cache. "
                    + "Ensures that when a user boots into a WTG workspace, they cannot read or write "
                    + "the Offline Files data of the host machine, preventing data leakage from the "
                    + "host's cached network files into the WTG environment. "
                    + "Default: absent (offline folder access allowed). Recommended: 1 on shared/kiosk desktops.",
                Tags = ["windows-to-go", "offline-folders", "data-leakage", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Offline Folders cache on host not accessible from WTG workspace.",
                ApplyOps = [RegOp.SetDword(WtgKey, "NoOfflineFolders", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "NoOfflineFolders")],
                DetectOps = [RegOp.CheckDword(WtgKey, "NoOfflineFolders", 1)],
            },
            new TweakDef
            {
                Id = "wtg-disable-retail-demo",
                Label = "Disable Retail Demo Mode for Windows To Go",
                Category = "System — Shared P C",
                Description =
                    "Sets DisableRetailDemo=1 in the PortableOperatingSystem policy key. "
                    + "Suppresses the Retail Demo Experience (RDX) from being shown or launched when "
                    + "a WTG workspace boots on a retail display or demo machine. Prevents WTG workspaces "
                    + "from being used as a kiosk demo mode and ensures productive enterprise use only. "
                    + "Default: absent. Recommended: 1 on all non-retail WTG deployments.",
                Tags = ["windows-to-go", "retail-demo", "kiosk", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Retail demo mode suppressed in WTG workspaces.",
                ApplyOps = [RegOp.SetDword(WtgKey, "DisableRetailDemo", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "DisableRetailDemo")],
                DetectOps = [RegOp.CheckDword(WtgKey, "DisableRetailDemo", 1)],
            },
            new TweakDef
            {
                Id = "wtg-disable-sync-on-metered",
                Label = "Disable Sync Provider on Metered Connection for WTG",
                Category = "System — Shared P C",
                Description =
                    "Sets DisableSyncProviderOnMetered=1 in the PortableOperatingSystem policy key. "
                    + "Prevents the WTG workspace from contacting cloud sync providers (OneDrive, Dropbox, etc.) "
                    + "when the device is on a metered network connection. Reduces data usage costs for WTG "
                    + "workspaces roaming over mobile broadband or tethered hotspots. "
                    + "Default: absent (sync allowed on metered). Recommended: 1 to protect data budgets.",
                Tags = ["windows-to-go", "sync", "metered", "onedrive", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud sync providers blocked on metered connections in WTG workspaces.",
                ApplyOps = [RegOp.SetDword(WtgKey, "DisableSyncProviderOnMetered", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "DisableSyncProviderOnMetered")],
                DetectOps = [RegOp.CheckDword(WtgKey, "DisableSyncProviderOnMetered", 1)],
            },
            new TweakDef
            {
                Id = "wtg-block-cross-hardware-deploy",
                Label = "Block Cross-Hardware WTG Deployment",
                Category = "System — Shared P C",
                Description =
                    "Sets NoCrossHardwareDeploy=1 in the PortableOperatingSystem policy key. "
                    + "Prevents the WTG workspace from being moved to a different hardware platform once it "
                    + "has been provisioned. Cross-hardware WTG deployment can cause driver conflicts, "
                    + "DHCP/MAC-address confusion, or break hardware-specific licensing tied to the original "
                    + "provisioning machine. Restricting this ensures workspace integrity. "
                    + "Default: absent (cross-hardware allowed). Recommended: 1 in managed enterprise WTG.",
                Tags = ["windows-to-go", "hardware", "deploy", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "WTG workspace cannot be re-provisioned on different hardware.",
                ApplyOps = [RegOp.SetDword(WtgKey, "NoCrossHardwareDeploy", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "NoCrossHardwareDeploy")],
                DetectOps = [RegOp.CheckDword(WtgKey, "NoCrossHardwareDeploy", 1)],
            },
            new TweakDef
            {
                Id = "wtg-enforce-secure-boot",
                Label = "Enforce Secure Boot for Windows To Go Workspaces",
                Category = "System — Shared P C",
                Description =
                    "Sets RequireSecureBoot=1 in the PortableOperatingSystem policy key. "
                    + "Requires that the host machine's Secure Boot setting be enabled before a WTG "
                    + "workspace will boot. Prevents WTG from being used as an attack vector on machines "
                    + "where Secure Boot has been disabled, ensuring the WTG kernel and boot files are "
                    + "signed and unmodified. "
                    + "Default: absent (Secure Boot not required). Recommended: 1 for security-hardened environments.",
                Tags = ["windows-to-go", "secure-boot", "uefi", "security", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "WTG workspace only boots on machines with Secure Boot enabled.",
                ApplyOps = [RegOp.SetDword(WtgKey, "RequireSecureBoot", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "RequireSecureBoot")],
                DetectOps = [RegOp.CheckDword(WtgKey, "RequireSecureBoot", 1)],
            },
            new TweakDef
            {
                Id = "wtg-disable-automatic-update",
                Label = "Disable Automatic Windows Update in WTG Workspace",
                Category = "System — Shared P C",
                Description =
                    "Sets NoAutoUpdate=1 in the PortableOperatingSystem policy key. "
                    + "Prevents the WTG workspace from automatically downloading and installing Windows updates "
                    + "while running on the road. Updates in a WTG workspace use the host machine's internet "
                    + "connection and can run out of USB drive space or interrupt productivity. "
                    + "Updates should be applied via WSUS or a scheduled service window instead. "
                    + "Default: absent (automatic updates allowed). Recommended: 1 on managed WTG deployments.",
                Tags = ["windows-to-go", "windows-update", "automatic", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Automatic Windows Update disabled in WTG workspaces; updates must be pushed manually.",
                ApplyOps = [RegOp.SetDword(WtgKey, "NoAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "NoAutoUpdate")],
                DetectOps = [RegOp.CheckDword(WtgKey, "NoAutoUpdate", 1)],
            },
        ];
    }
}
