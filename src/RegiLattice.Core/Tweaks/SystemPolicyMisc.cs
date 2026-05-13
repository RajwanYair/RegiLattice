namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static partial class PolicyMisc
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _ActiveSetupPolicy.Data,
            .. _ActiveXInstallerServicePolicy.Data,
            .. _ClipboardHistoryAdvancedPolicy.Data,
            .. _ClipboardHistoryPolicy.Data,
            .. _ClipboardSensitivityPolicy.Data,
            .. _CrashDumpPolicy.Data,
            .. _CrashDumpsPolicy.Data,
            .. _DotNetFrameworkPolicy.Data,
            .. _LicensingPolicy.Data,
            .. _MediaFoundationPolicy.Data,
            .. _MediaPlayerAdvPolicy.Data,
            .. _MsdtcPolicy.Data,
            .. _RestartManagerPolicy.Data,
            .. _SystemRecoveryOptionsPolicy.Data,
            .. _SystemRestoreGpoPolicy.Data,
            .. _TimeSyncAdvPolicy.Data,
            .. _TimeServicePolicy.Data,
            .. _WindowsAnytimeUpgradePolicy.Data,
            .. _WindowsBackupPolicy.Data,
            .. _WindowsConnectNowPolicy.Data,
            .. _WindowsLogonOptionsPolicy.Data,
            .. _WindowsMailPolicy.Data,
            .. _WindowsMediaPlayerPolicy.Data,
            .. _WindowsMediaPolicyAdv.Data,
            .. _WindowsPerformancePolicy.Data,
            .. _WindowsReliabilityPolicy.Data,
            .. _WindowsTimeGpoPolicy.Data,
            .. _WindowsTimePolicy.Data,
            .. _WinlogonPolicy.Data,
        ];

    // ── ActiveSetupPolicy ──
    private static class _ActiveSetupPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ActiveSetup";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "actsetup-disable-active-setup-execution",
                Label = "Disable Active Setup Execution for Non-Administrative Users",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Active Setup is a Windows mechanism that runs initialization commands once per user profile and can be abused for persistence by attackers who create malicious Active Setup entries. Disabling Active Setup execution for standard users prevents this persistence mechanism from running commands in the user context at each logon. Malware commonly uses the Active Setup registry key to persist across reboots by creating entries that execute malicious code on each user login without requiring administrative privileges. Active Setup entries are stored under HKLM\\SOFTWARE\\Microsoft\\Active Setup\\InstalledComponents and run once for each user based on version comparison. Organizations should audit Active Setup entries regularly to ensure no unauthorized entries have been added by malware or unauthorized software. Restricting Active Setup execution reduces the attack surface for persistence mechanisms that target the HKLM user initialization path.",
                Tags = ["active-setup", "persistence", "startup", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableActiveSetup", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableActiveSetup")],
                DetectOps = [RegOp.CheckDword(Key, "DisableActiveSetup", 1)],
            },
            new TweakDef
            {
                Id = "actsetup-audit-active-setup-changes",
                Label = "Enable Auditing for Active Setup Registry Modifications",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Active Setup registry key auditing captures changes to the InstalledComponents key that is used for per-user initialization enabling detection of persistence through this mechanism. Enabling registry auditing on Active Setup keys provides forensic evidence when malware adds persistence entries through the Active Setup mechanism. Many malware samples use Active Setup as a less-monitored persistence path compared to the more well-known Run keys and scheduled tasks. Registry audit events for the Active Setup key should be forwarded to SIEM with alerting on any new entry creation or modification. Baseline the expected Active Setup entries in your environment to enable anomaly detection when unexpected entries appear. Legitimate software does use Active Setup for initialization so a baseline of expected entries is needed to reduce false positives for alerting.",
                Tags = ["active-setup", "audit", "persistence-detection", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditActiveSetupChanges", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditActiveSetupChanges")],
                DetectOps = [RegOp.CheckDword(Key, "AuditActiveSetupChanges", 1)],
            },
            new TweakDef
            {
                Id = "actsetup-restrict-active-setup-to-signed",
                Label = "Restrict Active Setup to Digitally Signed Components Only",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Restricting Active Setup execution to digitally signed components prevents unsigned malware from using Active Setup as a persistence mechanism by requiring valid code signatures. Enforcing code signing for Active Setup components reduces the risk that attacker-added Active Setup entries will execute since most malware is not signed with trusted certificates. Code signing requirements for Active Setup align with Windows SmartScreen and application control policies that restrict execution to trusted signed binaries. Organizations implementing application control through AppLocker or Windows Defender Application Control should include Active Setup restrictions as part of the overall execution control strategy. Monitoring for Active Setup components with unsigned or untrusted signatures provides detection of attempted Active Setup abuse even when the restrictions prevent execution. Legacy Active Setup components from older software may lack valid signatures and should be reviewed to determine if they are still needed.",
                Tags = ["active-setup", "code-signing", "execution-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireSignedComponents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedComponents")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSignedComponents", 1)],
            },
            new TweakDef
            {
                Id = "actsetup-limit-active-setup-user-context",
                Label = "Limit Active Setup Execution to System Context Only",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Active Setup entries that run in user context can be abused by malware to execute malicious code with user privileges during each logon providing reliable persistence. Limiting Active Setup to system context execution ensures that only system-level initialization occurs through this mechanism rather than user-context code. User-context Active Setup entries are the most common abuse path because they execute without requiring administrative privileges on the system. Restricting user-context Active Setup does not affect most legitimate initialization tasks that run in system context as part of Windows component initialization. Organizations should review all existing user-context Active Setup entries and evaluate whether they serve a legitimate purpose before restricting user-context execution. Legacy applications that use user-context Active Setup for initialization will fail to initialize their components if user-context execution is restricted.",
                Tags = ["active-setup", "user-context", "execution-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LimitToSystemContext", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LimitToSystemContext")],
                DetectOps = [RegOp.CheckDword(Key, "LimitToSystemContext", 1)],
            },
            new TweakDef
            {
                Id = "actsetup-disable-iexplore-active-setup",
                Label = "Disable Internet Explorer Active Setup Initialization Components",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Internet Explorer Active Setup initialization components run at first logon to configure IE settings for each user profile but IE has been deprecated and retired in Windows 11. Disabling IE Active Setup components removes unnecessary initialization overhead for systems where Internet Explorer is not used or has been removed. IE Active Setup entries may create IE-related registry keys and settings that are unnecessary on systems where IE has been replaced by Microsoft Edge. Removing IE Active Setup components cleans up the user profile initialization process and reduces the time required for first logon on systems where IE initialization is not required. Systems running Windows 11 where IE was removed should audit remaining IE Active Setup entries and disable those that serve no functional purpose. Removing IE Active Setup entries should be tested to ensure that legacy applications depending on IE initialization are not broken.",
                Tags = ["active-setup", "internet-explorer", "ie-deprecation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableIEActiveSetup", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIEActiveSetup")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIEActiveSetup", 1)],
            },
            new TweakDef
            {
                Id = "actsetup-block-remote-active-setup-triggers",
                Label = "Block Remotely Triggered Active Setup Execution",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Active Setup entries can potentially be triggered from remote sessions including Remote Desktop which creates a remote code execution path if malicious entries have been added. Blocking remote triggering of Active Setup prevents remotely established sessions from activating malicious persistence entries that an attacker added after compromising the system. Active Setup entries added by malware executing in a first logon scenario can persist across remote sessions if remote activation is not restricted. Remote session Active Setup restrictions complement other Active Setup security controls by reducing the execution paths available for Active Setup-based persistence. Organizations with heavy Remote Desktop usage should evaluate the security implications of Active Setup in remote sessions during their security assessment. Monitoring for Active Setup execution in remote desktop sessions (RDP) is a valuable detection signal for Active Setup abuse.",
                Tags = ["active-setup", "remote-execution", "rdp", "persistence", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockRemoteTriggers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockRemoteTriggers")],
                DetectOps = [RegOp.CheckDword(Key, "BlockRemoteTriggers", 1)],
            },
            new TweakDef
            {
                Id = "actsetup-whitelist-active-setup-guids",
                Label = "Enforce Allowlist for Permitted Active Setup Component GUIDs",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Active Setup component GUID allowlisting restricts which component identifiers can trigger execution preventing unknown or attacker-created GUIDs from executing. Enforcing an allowlist for permitted GUIDs ensures that only specifically approved Active Setup components can run during user profile initialization. GUID allowlisting for Active Setup is a granular control that requires maintaining a list of all legitimate component GUIDs used by the organization's software. Organizations implementing GUID allowlisting should inventory all Active Setup GUIDs across their fleet before applying restrictions to avoid blocking legitimate initialization. The GUID allowlist approach provides stronger protection than code signing alone because it combines both the identity and the identity of the executing component. Maintaining the GUID allowlist requires a software management process to add new GUIDs when deploying software that uses Active Setup for initialization.",
                Tags = ["active-setup", "allowlist", "guid", "execution-control", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableComponentAllowlist", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableComponentAllowlist")],
                DetectOps = [RegOp.CheckDword(Key, "EnableComponentAllowlist", 1)],
            },
            new TweakDef
            {
                Id = "actsetup-log-active-setup-execution",
                Label = "Enable Execution Logging for All Active Setup Component Runs",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Active Setup execution logging creates a record of every Active Setup component that runs during user profile initialization providing forensic visibility into initialization actions. Enabling execution logging for Active Setup helps administrators identify which components are running for each user providing data for optimization and security auditing. Active Setup execution logs can be compared against a known-good baseline to detect new or modified components that may indicate compromise. Organizations should retain Active Setup execution logs for at least 90 days as part of their security monitoring strategy for detecting persistence mechanisms. Execution logging should capture the full command line of each component to detect components that were modified to include malicious arguments. Cross-referencing Active Setup execution logs with process execution monitoring from EDR solutions provides comprehensive coverage for this persistence technique.",
                Tags = ["active-setup", "logging", "execution-monitoring", "forensics", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableExecutionLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableExecutionLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableExecutionLogging", 1)],
            },
            new TweakDef
            {
                Id = "actsetup-prevent-active-setup-version-spoofing",
                Label = "Prevent Active Setup Component Version Number Spoofing",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Active Setup execution is controlled by version number comparison where components run when the installed version is higher than the per-user stored version allowing version number manipulation to force re-execution. Preventing version number spoofing blocks attackers from modifying the version number of malicious Active Setup components to force them to repeatedly execute at each user logon. Version spoofing attacks modify the version string of an Active Setup component to always be higher than the user-stored version causing perpetual re-execution of the component. Integrity monitoring of the Active Setup component version numbers provides detection for version manipulation attempts. Active Setup version numbers should be monotonically increasing and any decrease should be treated as suspicious and investigated. Organizations should implement file integrity monitoring for the Active Setup registry key to detect version number modifications.",
                Tags = ["active-setup", "version-spoofing", "persistence", "integrity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventVersionSpoofing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventVersionSpoofing")],
                DetectOps = [RegOp.CheckDword(Key, "PreventVersionSpoofing", 1)],
            },
            new TweakDef
            {
                Id = "actsetup-enforce-active-setup-user-isolation",
                Label = "Enforce User Profile Isolation for Active Setup Component Execution",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Active Setup user profile isolation ensures that each user's initialization state is tracked independently and that one user's Active Setup configuration cannot affect another user's profile. Enforcing user profile isolation for Active Setup prevents a compromised user profile from manipulating Active Setup state for other users on the same system. Shared workstations where multiple users log in sequentially have higher risk for cross-user Active Setup state manipulation if isolation is not enforced. User isolation for Active Setup complements mandatory profile and AppContainer confinement to create boundaries between users sharing the same physical hardware. Active Setup isolation should be combined with user profile security controls like ACLs on user profile directories to prevent unauthorized access. Organizations with high-security shared workstation scenarios should evaluate Active Setup isolation as part of the overall user isolation strategy.",
                Tags = ["active-setup", "user-isolation", "profile-security", "shared-workstation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceUserIsolation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceUserIsolation")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceUserIsolation", 1)],
            },
        ];
    }

    // ── ActiveXInstallerServicePolicy ──
    private static class _ActiveXInstallerServicePolicy
    {
        private const string AxKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AxInstaller";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "axinst-disable-activex-install",
                Label = "Disable ActiveX Installer Service",
                Category = "System — Active Setup",
                Description =
                    "Sets DoNotRunAxInstaller=1 in the AxInstaller policy key. "
                    + "Prevents the ActiveX Installer Service from installing or updating ActiveX controls on this machine. "
                    + "Recommended on all modern systems where legacy ActiveX content is not required, "
                    + "reducing the attack surface from malicious or out-of-date ActiveX controls. "
                    + "Default: absent (service runs). Recommended: 1 for all non-IE-enterprise deployments.",
                Tags = ["activex", "installer", "legacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "ActiveX Installer Service disabled; ActiveX controls cannot be installed or updated machine-wide.",
                ApplyOps = [RegOp.SetDword(AxKey, "DoNotRunAxInstaller", 1)],
                RemoveOps = [RegOp.DeleteValue(AxKey, "DoNotRunAxInstaller")],
                DetectOps = [RegOp.CheckDword(AxKey, "DoNotRunAxInstaller", 1)],
            },
            new TweakDef
            {
                Id = "axinst-require-admin-approval",
                Label = "Require Admin Approval for ActiveX Install",
                Category = "System — Active Setup",
                Description =
                    "Sets RequireApproval=1 in the AxInstaller policy key. "
                    + "Forces the ActiveX Installer Service to require administrator approval "
                    + "before installing any ActiveX control, even for controls from trusted zones. "
                    + "Prevents silent ActiveX installation by non-admin users in enterprise environments. "
                    + "Default: absent (controls install silently from trusted zones). Recommended: 1.",
                Tags = ["activex", "approval", "admin", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ActiveX installs require explicit admin approval even from trusted sites.",
                ApplyOps = [RegOp.SetDword(AxKey, "RequireApproval", 1)],
                RemoveOps = [RegOp.DeleteValue(AxKey, "RequireApproval")],
                DetectOps = [RegOp.CheckDword(AxKey, "RequireApproval", 1)],
            },
            new TweakDef
            {
                Id = "axinst-disable-trusted-zone-only",
                Label = "Block ActiveX Install from Untrusted Zones",
                Category = "System — Active Setup",
                Description =
                    "Sets DisableActiveXInstallFromUntrustedZones=1 in the AxInstaller policy key. "
                    + "Prevents the ActiveX Installer Service from processing install requests "
                    + "for controls that originate from untrusted or restricted security zones. "
                    + "Controls from the Internet zone and restricted sites are blocked. "
                    + "Default: absent (all zones allowed). Recommended: 1.",
                Tags = ["activex", "zones", "untrusted", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ActiveX installs from untrusted or Internet zones blocked by the installer service.",
                ApplyOps = [RegOp.SetDword(AxKey, "DisableActiveXInstallFromUntrustedZones", 1)],
                RemoveOps = [RegOp.DeleteValue(AxKey, "DisableActiveXInstallFromUntrustedZones")],
                DetectOps = [RegOp.CheckDword(AxKey, "DisableActiveXInstallFromUntrustedZones", 1)],
            },
            new TweakDef
            {
                Id = "axinst-log-successful-installs",
                Label = "Enable ActiveX Install Success Logging",
                Category = "System — Active Setup",
                Description =
                    "Sets LoggingEnabled=1 in the AxInstaller policy key. "
                    + "Instructs the ActiveX Installer Service to write a log entry to the Windows Event Log "
                    + "for every successfully installed ActiveX control, including source URL and CLSID. "
                    + "Supports compliance auditing of legacy ActiveX deployments. "
                    + "Default: absent (no success logging). Recommended: 1 in audit-aware environments.",
                Tags = ["activex", "logging", "audit", "event-log", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "ActiveX successful install events logged to the Application event log.",
                ApplyOps = [RegOp.SetDword(AxKey, "LoggingEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(AxKey, "LoggingEnabled")],
                DetectOps = [RegOp.CheckDword(AxKey, "LoggingEnabled", 1)],
            },
            new TweakDef
            {
                Id = "axinst-log-failed-installs",
                Label = "Enable ActiveX Install Failure Logging",
                Category = "System — Active Setup",
                Description =
                    "Sets ErrorLoggingEnabled=1 in the AxInstaller policy key. "
                    + "Instructs the ActiveX Installer Service to write error events to the Windows Event Log "
                    + "for failed ActiveX control installations, including the error code and control CLSID. "
                    + "Default: absent (errors silently discarded). Recommended: 1 to track blocked or failing installs.",
                Tags = ["activex", "logging", "errors", "event-log", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "ActiveX install failure and error events logged to the Application event log.",
                ApplyOps = [RegOp.SetDword(AxKey, "ErrorLoggingEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(AxKey, "ErrorLoggingEnabled")],
                DetectOps = [RegOp.CheckDword(AxKey, "ErrorLoggingEnabled", 1)],
            },
            new TweakDef
            {
                Id = "axinst-disable-activex-update",
                Label = "Disable Automatic ActiveX Control Updates",
                Category = "System — Active Setup",
                Description =
                    "Sets DisableAxUpdate=1 in the AxInstaller policy key. "
                    + "Prevents the ActiveX Installer Service from automatically updating existing ActiveX controls "
                    + "to newer versions when a web page requests an update. Helps maintain a known-good control state "
                    + "in locked-down enterprise environments. "
                    + "Default: absent (updates allowed). Recommended: 1 when ActiveX control versions are change-managed.",
                Tags = ["activex", "update", "version-lock", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "ActiveX controls not automatically updated; only initial installs processed by the service.",
                ApplyOps = [RegOp.SetDword(AxKey, "DisableAxUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(AxKey, "DisableAxUpdate")],
                DetectOps = [RegOp.CheckDword(AxKey, "DisableAxUpdate", 1)],
            },
            new TweakDef
            {
                Id = "axinst-block-per-user-install",
                Label = "Block Per-User ActiveX Control Installation",
                Category = "System — Active Setup",
                Description =
                    "Sets BlockPerUserInstall=1 in the AxInstaller policy key. "
                    + "Prevents the ActiveX Installer Service from installing controls in per-user profile locations, "
                    + "forcing all ActiveX installations to the machine-wide registry or Program Files. "
                    + "Prevents users from silently deploying controls into their own profile. "
                    + "Default: absent (per-user installs allowed). Recommended: 1 in enterprise environments.",
                Tags = ["activex", "per-user", "profile", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Per-user ActiveX control installations blocked; only machine-wide installs by admins are permitted.",
                ApplyOps = [RegOp.SetDword(AxKey, "BlockPerUserInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(AxKey, "BlockPerUserInstall")],
                DetectOps = [RegOp.CheckDword(AxKey, "BlockPerUserInstall", 1)],
            },
            new TweakDef
            {
                Id = "axinst-disable-silent-install",
                Label = "Disable Silent ActiveX Control Installation",
                Category = "System — Active Setup",
                Description =
                    "Sets DisableSilentInstall=1 in the AxInstaller policy key. "
                    + "Prevents the ActiveX Installer Service from installing controls without displaying "
                    + "a visible installation prompt or User Account Control elevation dialog. "
                    + "Ensures all ActiveX activity is visible to the user or admin. "
                    + "Default: absent (silent install possible). Recommended: 1 for user awareness.",
                Tags = ["activex", "silent", "uac", "prompt", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "ActiveX controls always install with a visible prompt; silent background installs blocked.",
                ApplyOps = [RegOp.SetDword(AxKey, "DisableSilentInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(AxKey, "DisableSilentInstall")],
                DetectOps = [RegOp.CheckDword(AxKey, "DisableSilentInstall", 1)],
            },
            new TweakDef
            {
                Id = "axinst-restrict-download-cache",
                Label = "Restrict ActiveX Download Cache Size",
                Category = "System — Active Setup",
                Description =
                    "Sets MaxCachedDownloadSize=0 in the AxInstaller policy key. "
                    + "Limits the ActiveX Installer Service download cache to zero, preventing caching of "
                    + "downloaded ActiveX control installer packages. Each install always re-downloads the package. "
                    + "Prevents malicious packages from being cached and re-used across sessions. "
                    + "Default: absent (default cache size). Recommended: 0 on high-security machines.",
                Tags = ["activex", "cache", "download", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "ActiveX installer download cache disabled; no packages cached on disk.",
                ApplyOps = [RegOp.SetDword(AxKey, "MaxCachedDownloadSize", 0)],
                RemoveOps = [RegOp.DeleteValue(AxKey, "MaxCachedDownloadSize")],
                DetectOps = [RegOp.CheckDword(AxKey, "MaxCachedDownloadSize", 0)],
            },
            new TweakDef
            {
                Id = "axinst-block-ocx-download",
                Label = "Block ActiveX OCX Download from Internet",
                Category = "System — Active Setup",
                Description =
                    "Sets BlockOcxDownload=1 in the AxInstaller policy key. "
                    + "Prevents the ActiveX Installer Service from downloading .ocx files (OLE Control eXtensions) "
                    + "from any internet-based source, including trusted sites. "
                    + "Forces all ActiveX control files to be sourced from local paths or intranet shares. "
                    + "Default: absent (internet downloads allowed). Recommended: 1 in air-gapped or enterprise environments.",
                Tags = ["activex", "ocx", "download", "internet", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ActiveX .ocx file downloads from the internet completely blocked by the installer service.",
                ApplyOps = [RegOp.SetDword(AxKey, "BlockOcxDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(AxKey, "BlockOcxDownload")],
                DetectOps = [RegOp.CheckDword(AxKey, "BlockOcxDownload", 1)],
            },
        ];
    }

    // ── ClipboardHistoryAdvancedPolicy ──
    private static class _ClipboardHistoryAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "clipadv-disable-history-across-sessions",
                    Label = "Disable Clipboard History Across Sessions",
                    Category = "System — Active Setup",
                    Description = "Disables clipboard history persistence across logon sessions so clipboard items are cleared when a user logs off.",
                    Tags = ["clipboard", "session", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard cleared on logoff; sensitive data does not persist between sessions.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClipboardHistoryAcrossSessions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardHistoryAcrossSessions")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClipboardHistoryAcrossSessions", 1)],
                },
                new TweakDef
                {
                    Id = "clipadv-block-app-clipboard-access",
                    Label = "Block Clipboard Access from Apps",
                    Category = "System — Active Setup",
                    Description =
                        "Blocks background application access to clipboard contents unless the application is in the foreground, preventing silent clipboard exfiltration.",
                    Tags = ["clipboard", "apps", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Background apps blocked from reading clipboard; may break clipboard managers.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockClipboardAccessApps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockClipboardAccessApps")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockClipboardAccessApps", 1)],
                },
                new TweakDef
                {
                    Id = "clipadv-disable-history-logging",
                    Label = "Disable Clipboard History Logging",
                    Category = "System — Active Setup",
                    Description =
                        "Disables event logging of clipboard history operations, preventing clipboard contents from appearing in diagnostic logs.",
                    Tags = ["clipboard", "logging", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard history operations not logged; reduces diagnostic data.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClipboardHistoryLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardHistoryLog")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClipboardHistoryLog", 1)],
                },
                new TweakDef
                {
                    Id = "clipadv-restrict-to-current-user",
                    Label = "Restrict Clipboard History to Current User Only",
                    Category = "System — Active Setup",
                    Description =
                        "Restricts clipboard history storage so that entries are isolated to the current user's session and cannot be accessed by other users on the same machine.",
                    Tags = ["clipboard", "user", "isolation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard history is per-user only; no shared clipboard history between accounts.",
                    ApplyOps = [RegOp.SetDword(Key, "ClipboardCurrentUserOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ClipboardCurrentUserOnly")],
                    DetectOps = [RegOp.CheckDword(Key, "ClipboardCurrentUserOnly", 1)],
                },
                new TweakDef
                {
                    Id = "clipadv-disable-rich-text-clipboard",
                    Label = "Disable Rich Text Clipboard Format",
                    Category = "System — Active Setup",
                    Description =
                        "Disables the rich text (RTF) clipboard format, forcing text copies to plain text and reducing the metadata stored in clipboard entries.",
                    Tags = ["clipboard", "rich-text", "format", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "RTF formatting stripped on copy; pasted text is plain. May affect Word/Office workflows.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRichTextClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRichTextClipboard")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRichTextClipboard", 1)],
                },
                new TweakDef
                {
                    Id = "clipadv-block-bg-app-clipboard",
                    Label = "Block Clipboard API for Background Apps",
                    Category = "System — Active Setup",
                    Description =
                        "Prevents background applications from using the clipboard API for reads or writes, limiting clipboard exposure to foreground processes only.",
                    Tags = ["clipboard", "background", "api", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Background clipboard API blocked; clipboard managers and automation tools may break.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockClipboardBgApps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockClipboardBgApps")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockClipboardBgApps", 1)],
                },
                new TweakDef
                {
                    Id = "clipadv-max-item-count-25",
                    Label = "Set Clipboard History Max Item Count to 25",
                    Category = "System — Active Setup",
                    Description = "Caps the clipboard history list at 25 entries to limit on-disk footprint of potentially sensitive copied data.",
                    Tags = ["clipboard", "limit", "history", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Older clipboard items purged after 25; reduces sensitive data retention.",
                    ApplyOps = [RegOp.SetDword(Key, "ClipboardMaxItemCount", 25)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ClipboardMaxItemCount")],
                    DetectOps = [RegOp.CheckDword(Key, "ClipboardMaxItemCount", 25)],
                },
                new TweakDef
                {
                    Id = "clipadv-disable-lock-screen-clipboard",
                    Label = "Disable Clipboard History on Lock Screen",
                    Category = "System — Active Setup",
                    Description =
                        "Disables access to clipboard history from the lock screen, preventing unauthenticated users from viewing previously copied content.",
                    Tags = ["clipboard", "lock-screen", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard history inaccessible on lock screen; prevents physical access leakage.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLockScreenClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLockScreenClipboard")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLockScreenClipboard", 1)],
                },
            ];
    }

    // ── ClipboardHistoryPolicy ──
    private static class _ClipboardHistoryPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ClipboardHistory";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "clphist-disable-clipboard-history",
                Label = "Disable Clipboard History",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableClipboardHistory=1 in the ClipboardHistory policy key. Prevents "
                    + "Windows from storing a multi-item clipboard history accessible via Win+V. "
                    + "Clipboard history retains copied text, images, and HTML fragments in memory "
                    + "across application boundaries. Disabling it limits the clipboard to one "
                    + "item at a time, reducing the surface area for data leakage. "
                    + "Default: 0. Recommended: 1 on shared or high-security machines.",
                Tags = ["clipboard", "history", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableClipboardHistory", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardHistory")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClipboardHistory", 1)],
            },
            new TweakDef
            {
                Id = "clphist-disable-cloud-sync",
                Label = "Disable Clipboard Cloud Sync",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets DisableClipboardSync=1 in the ClipboardHistory policy key. Stops "
                    + "clipboard content from being synchronised to Microsoft's cloud and "
                    + "distributed to other devices signed in with the same Microsoft account. "
                    + "Cloud sync can exfiltrate copied passwords, keys, and PII to additional "
                    + "devices the user owns or shares. Default: 0. Recommended: 1 always.",
                Tags = ["clipboard", "cloud", "sync", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableClipboardSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClipboardSync", 1)],
            },
            new TweakDef
            {
                Id = "clphist-clear-on-logoff",
                Label = "Clear Clipboard History on Logoff",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets ClearClipboardOnLogoff=1 in the ClipboardHistory policy key. Purges "
                    + "the entire stored clipboard history when the user logs off. Without this "
                    + "policy the history persists across sessions, meaning a subsequent user "
                    + "on a shared machine can inspect copied content from the previous session "
                    + "via Win+V. Default: 0. Recommended: 1 on shared workstations.",
                Tags = ["clipboard", "logoff", "clear", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ClearClipboardOnLogoff", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ClearClipboardOnLogoff")],
                DetectOps = [RegOp.CheckDword(Key, "ClearClipboardOnLogoff", 1)],
            },
            new TweakDef
            {
                Id = "clphist-disable-enterprise-sync",
                Label = "Disable Clipboard Enterprise Roaming",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableEnterpriseSync=1 in the ClipboardHistory policy key. Prevents "
                    + "clipboard history from roaming across enterprise devices enrolled in the "
                    + "same Azure AD tenant via the enterprise clipboard sync service. Roaming "
                    + "clipboard in an enterprise context can propagate sensitive data from a "
                    + "secure workstation to a less-secure shared device. "
                    + "Default: 0. Recommended: 1 in regulated industries.",
                Tags = ["clipboard", "enterprise", "sync", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableEnterpriseSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableEnterpriseSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableEnterpriseSync", 1)],
            },
            new TweakDef
            {
                Id = "clphist-disable-pin-items",
                Label = "Disable Clipboard Pin Persistent Items",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisablePinItems=1 in the ClipboardHistory policy key. Prevents users "
                    + "from pinning clipboard items, blocking indefinite retention of specific "
                    + "copied fragments in the history viewer. Pinned items are never evicted "
                    + "by the normal rotation algorithm, meaning sensitive data could persist "
                    + "across many sessions. Default: 0. Recommended: 1.",
                Tags = ["clipboard", "pin", "retention", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePinItems", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePinItems")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePinItems", 1)],
            },
            new TweakDef
            {
                Id = "clphist-disable-image-data",
                Label = "Disable Clipboard Image Data Retention",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableImageData=1 in the ClipboardHistory policy key. Prevents "
                    + "Windows from storing bitmap and image data in the clipboard history. "
                    + "Image clipboard entries can be large and may contain screenshots of "
                    + "confidential documents. Text-only clipboard history is significantly "
                    + "smaller and less sensitive. Default: 0. Recommended: 1.",
                Tags = ["clipboard", "image", "screenshot", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableImageData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableImageData")],
                DetectOps = [RegOp.CheckDword(Key, "DisableImageData", 1)],
            },
            new TweakDef
            {
                Id = "clphist-disable-html-data",
                Label = "Disable Clipboard HTML Fragment Retention",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableHtmlData=1 in the ClipboardHistory policy key. Prevents the "
                    + "history from storing HTML-format clipboard entries produced by browsers "
                    + "and rich-text editors. HTML clipboard data can include embedded form "
                    + "field values, session tokens, and styling metadata that goes beyond the "
                    + "visible copied text. Default: 0. Recommended: 1.",
                Tags = ["clipboard", "html", "browser", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHtmlData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHtmlData")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHtmlData", 1)],
            },
            new TweakDef
            {
                Id = "clphist-disable-thumbnail-preview",
                Label = "Disable Clipboard History Thumbnail Preview",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets DisableThumbnailPreview=1 in the ClipboardHistory policy key. Removes "
                    + "the visual thumbnail preview shown in the Win+V clipboard picker. Thumbnail "
                    + "previews generate on-demand renders of previously copied images and "
                    + "documents, caching them for rapid display. Disabling removes the cache "
                    + "and reduces memory consumption. Default: 0. Recommended: 1.",
                Tags = ["clipboard", "thumbnail", "preview", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableThumbnailPreview", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableThumbnailPreview")],
                DetectOps = [RegOp.CheckDword(Key, "DisableThumbnailPreview", 1)],
            },
            new TweakDef
            {
                Id = "clphist-limit-history-size",
                Label = "Limit Clipboard History Size",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets MaxHistorySize=10 in the ClipboardHistory policy key. Caps the number "
                    + "of items retained in clipboard history to 10 entries (default system "
                    + "maximum is 25). A smaller history window reduces the amount of data "
                    + "available to an attacker who briefly accesses the machine and reviews "
                    + "history via Win+V. Default: not set (25 items). Recommended: 10.",
                Tags = ["clipboard", "history", "limit", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxHistorySize", 10)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxHistorySize")],
                DetectOps = [RegOp.CheckDword(Key, "MaxHistorySize", 10)],
            },
            new TweakDef
            {
                Id = "clphist-disable-telemetry",
                Label = "Disable Clipboard History Telemetry",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableClipboardTelemetry=1 in the ClipboardHistory policy key. "
                    + "Prevents Windows from reporting clipboard history usage analytics "
                    + "(feature engagement, copy-paste frequency, sync events) to Microsoft's "
                    + "telemetry pipeline. These signals inform product improvements but transmit "
                    + "behavioural metadata outside of normal diagnostic data consent. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["clipboard", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableClipboardTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClipboardTelemetry", 1)],
            },
        ];
    }

    // ── ClipboardSensitivityPolicy ──
    private static class _ClipboardSensitivityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "clipsens-block-sensitive-data-dlp",
                    Label = "Block Sensitive Data in Clipboard (DLP)",
                    Category = "System — Active Setup",
                    Description =
                        "Enables DLP-style clipboard content blocking that prevents sensitive data (PII, credentials, financial info) from being copied to the clipboard by monitored apps.",
                    Tags = ["clipboard", "dlp", "sensitive", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Sensitive-data clipboard block; may interrupt legitimate copy operations for data classes.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClipboardDLP", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardDLP")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClipboardDLP", 1)],
                },
                new TweakDef
                {
                    Id = "clipsens-disable-diagnostic-monitoring",
                    Label = "Disable Clipboard Monitoring by Diagnostic Services",
                    Category = "System — Active Setup",
                    Description =
                        "Disables clipboard monitoring by Windows diagnostic data services, preventing clipboard usage data (not content) from being collected as diagnostic telemetry.",
                    Tags = ["clipboard", "monitoring", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Diagnostic clipboard monitoring stopped; reduces telemetry without functional impact.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClipboardMonitoringDiag", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardMonitoringDiag")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClipboardMonitoringDiag", 1)],
                },
                new TweakDef
                {
                    Id = "clipsens-block-pii-in-history",
                    Label = "Block PII from Clipboard History",
                    Category = "System — Active Setup",
                    Description =
                        "Prevents personally identifiable information from being stored in clipboard history entries, stripping or blocking PII items before they enter the history store.",
                    Tags = ["clipboard", "pii", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "PII-matching clipboard items excluded from history; requires content scanning.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPIIInClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPIIInClipboard")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPIIInClipboard", 1)],
                },
                new TweakDef
                {
                    Id = "clipsens-disable-usage-analytics",
                    Label = "Disable Clipboard Usage Analytics",
                    Category = "System — Active Setup",
                    Description =
                        "Disables collection of clipboard usage analytics (copy/paste frequency, format types, app usage) sent to Microsoft for product improvement.",
                    Tags = ["clipboard", "analytics", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard analytics not collected; no functional impact.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClipboardMetrics", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardMetrics")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClipboardMetrics", 1)],
                },
                new TweakDef
                {
                    Id = "clipsens-restrict-secure-desktops",
                    Label = "Restrict Clipboard to Secure Desktops Only",
                    Category = "System — Active Setup",
                    Description =
                        "Restricts clipboard operations to secure desktop contexts only, preventing clipboard data from crossing the security boundary between secure and non-secure desktops.",
                    Tags = ["clipboard", "secure-desktop", "isolation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Clipboard may not cross secure/non-secure desktop boundary.",
                    ApplyOps = [RegOp.SetDword(Key, "SecureDesktopClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SecureDesktopClipboard")],
                    DetectOps = [RegOp.CheckDword(Key, "SecureDesktopClipboard", 1)],
                },
                new TweakDef
                {
                    Id = "clipsens-block-bluetooth-share",
                    Label = "Block Clipboard Sharing via Bluetooth",
                    Category = "System — Active Setup",
                    Description =
                        "Blocks clipboard content from being shared over Bluetooth connections (e.g., via Swift Pair or Nearby Sharing with Bluetooth transport).",
                    Tags = ["clipboard", "bluetooth", "sharing", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard not shared via Bluetooth; nearby Bluetooth devices cannot receive clipboard data.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardBluetoothShare", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardBluetoothShare")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardBluetoothShare", 1)],
                },
                new TweakDef
                {
                    Id = "clipsens-disable-kiosk-clipboard",
                    Label = "Disable Clipboard Access in Kiosk Mode",
                    Category = "System — Active Setup",
                    Description =
                        "Disables clipboard access in Kiosk (Assigned Access) mode, preventing kiosk users from copying data from the kiosk session to other applications.",
                    Tags = ["clipboard", "kiosk", "assigned-access", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Kiosk clipboard blocked; kiosk users cannot copy data out of the session.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableKioskModeClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableKioskModeClipboard")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableKioskModeClipboard", 1)],
                },
                new TweakDef
                {
                    Id = "clipsens-prevent-password-paste",
                    Label = "Prevent Password Paste from Clipboard Manager",
                    Category = "System — Active Setup",
                    Description =
                        "Blocks clipboard managers from injecting stored passwords into password fields via paste, requiring direct typing or approved password manager integration.",
                    Tags = ["clipboard", "password", "paste", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Password paste from clipboard managers blocked; users must type or use approved password manager.",
                    ApplyOps = [RegOp.SetDword(Key2, "PreventPasswordPasteFromClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "PreventPasswordPasteFromClipboard")],
                    DetectOps = [RegOp.CheckDword(Key2, "PreventPasswordPasteFromClipboard", 1)],
                },
                new TweakDef
                {
                    Id = "clipsens-max-data-size-64kb",
                    Label = "Restrict Clipboard Max Data Size to 64 KB",
                    Category = "System — Active Setup",
                    Description =
                        "Caps the maximum size of a single clipboard entry at 64 KB (65536 bytes), limiting the volume of bulk data that can be exfiltrated in a single clipboard operation.",
                    Tags = ["clipboard", "size-limit", "dlp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Large clipboard copies (e.g., image, file list) truncated or blocked above 64 KB.",
                    ApplyOps = [RegOp.SetDword(Key2, "ClipboardMaxDataSizeKB", 64)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "ClipboardMaxDataSizeKB")],
                    DetectOps = [RegOp.CheckDword(Key2, "ClipboardMaxDataSizeKB", 64)],
                },
                new TweakDef
                {
                    Id = "clipsens-disable-encryption-bypass",
                    Label = "Disable Clipboard Encryption Bypass",
                    Category = "System — Active Setup",
                    Description =
                        "Disables clipboard encryption bypass mechanisms that allow certain privileged processes to read encrypted clipboard contents without proper decryption.",
                    Tags = ["clipboard", "encryption", "bypass", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Encryption bypass paths blocked; all clipboard access goes through standard decryption.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardEncryptionBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardEncryptionBypass")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardEncryptionBypass", 1)],
                },
            ];
    }

    // ── CrashDumpPolicy ──
    private static class _CrashDumpPolicy
    {
        private const string CcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cdump-disable-send-alert",
                    Label = "Disable BSOD Admin Alert",
                    Category = "System — Active Setup",
                    Description =
                        "Sets SendAlert=0 to prevent Windows from sending a network alert to the designated administrator message server when a stop error occurs. Default: 1 in domain environments. Relevant for workgroup machines.",
                    Tags = ["crash dump", "alert", "network", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "No network alert sent on BSOD; appropriate for standalone or home-network machines.",
                    ApplyOps = [RegOp.SetDword(CcKey, "SendAlert", 0)],
                    RemoveOps = [RegOp.DeleteValue(CcKey, "SendAlert")],
                    DetectOps = [RegOp.CheckDword(CcKey, "SendAlert", 0)],
                },
                new TweakDef
                {
                    Id = "cdump-disable-storage-telemetry",
                    Label = "Disable Crash Dump Telemetry Collection",
                    Category = "System — Active Setup",
                    Description =
                        "Sets StorageTelemetryEnabled=0 to prevent WER from uploading crash dump telemetry to Microsoft when connected. Combines with WER upload policies for comprehensive crash data privacy.",
                    Tags = ["crash dump", "telemetry", "wer", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Crash dump data not uploaded for telemetry analysis; local dumps still created if enabled.",
                    ApplyOps = [RegOp.SetDword(CcKey, "StorageTelemetryEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(CcKey, "StorageTelemetryEnabled")],
                    DetectOps = [RegOp.CheckDword(CcKey, "StorageTelemetryEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "cdump-disable-dump-log-file",
                    Label = "Disable Crash Dump Log File",
                    Category = "System — Active Setup",
                    Description =
                        "Sets EnableLogFile=0 to prevent the crash dump subsystem from writing the memory.dmp log header file alongside the dump. Default: 1. Reduces extra disk writes and keeps the dump directory clean.",
                    Tags = ["crash dump", "log file", "disk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "No supplemental dump log file written; core dump file (if enabled) still created.",
                    ApplyOps = [RegOp.SetDword(CcKey, "EnableLogFile", 0)],
                    RemoveOps = [RegOp.DeleteValue(CcKey, "EnableLogFile")],
                    DetectOps = [RegOp.CheckDword(CcKey, "EnableLogFile", 0)],
                },
                new TweakDef
                {
                    Id = "cdump-disable-dedicated-dump-file",
                    Label = "Disable Dedicated Dump File",
                    Category = "System — Active Setup",
                    Description =
                        "Sets DisableDedicatedDumpFile=1 to prevent Windows from reserving a dedicated page-file-adjacent dump file (used on devices where the page file is too small for a full dump). Default: 0 (dedicated file used when needed).",
                    Tags = ["crash dump", "dedicated file", "pagefile", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "No reserved dump file; on low-pagefile devices the dump may fail to be captured.",
                    ApplyOps = [RegOp.SetDword(CcKey, "DisableDedicatedDumpFile", 1)],
                    RemoveOps = [RegOp.DeleteValue(CcKey, "DisableDedicatedDumpFile")],
                    DetectOps = [RegOp.CheckDword(CcKey, "DisableDedicatedDumpFile", 1)],
                },
            ];
    }

    // ── CrashDumpsPolicy ──
    private static class _CrashDumpsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CrashControl";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "crshmp-disable-crash-report-telemetry",
                Label = "Disable Automatic Crash Report Transmission to Microsoft Telemetry",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Disabling automatic crash report transmission prevents Windows Error Reporting from sending crash dump data to Microsoft's telemetry infrastructure. Crash dumps may contain sensitive data from process memory including encryption keys authentication tokens database connection strings and user data that was in memory at the time of the crash. In enterprise environments crash reports may contain proprietary business logic application code and sensitive application data from any application that was running at the time of the crash. Organizations should evaluate whether the diagnostic benefit of automatic crash reporting outweighs the data exposure risk before enabling automatic cloud transmission. An alternative approach is to configure crash dumps to be stored locally and reviewed by an internal security team before any submission to external parties. Organizations operating in data-sensitive industries should treat crash dump data with the same sensitivity level as the data processed by the crashed application.",
                Tags = ["crash-dumps", "telemetry", "data-protection", "error-reporting", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCrashReportTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCrashReportTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCrashReportTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "crshmp-restrict-minidump-directory",
                Label = "Restrict Mini Dump Directory to Secure Administrative Location",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting the mini dump directory to a secure administrator-only location prevents unauthorized users from accessing crash dump files that may contain sensitive process memory data. By default crash dumps are written to locations accessible to local administrators and in some configurations to standard users. Moving crash dump files to a location with strict ACLs ensures that only authorized personnel can access the dump files for analysis. The directory should have audit logging configured so that all access to crash dump files is recorded for security monitoring. Organizations that have incident response procedures for application crashes should ensure that the dump directory is included in their investigation toolchain. Restricting access to dump files is particularly important when applications that process sensitive regulated data can generate crash dumps.",
                Tags = ["crash-dumps", "dump-directory", "access-control", "secure-storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "MinidumpDir", "")],
                RemoveOps = [RegOp.DeleteValue(Key, "MinidumpDir")],
                DetectOps = [RegOp.CheckMissing(Key, "MinidumpDir")],
            },
            new TweakDef
            {
                Id = "crshmp-configure-dump-type-kernel",
                Label = "Configure Crash Dump Type to Kernel Memory Dump for System Analysis",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Configuring the crash dump type to kernel memory dump captures only kernel mode memory at the time of system crash providing sufficient diagnostic information while reducing the volume of user-mode memory captured. Complete memory dumps capture all physical memory including all user-mode application data which maximizes data exposure risk but may be required for certain deep diagnostic scenarios. Kernel memory dumps contain the kernel code stack and data structures enabling diagnosis of kernel-mode crashes and blue screen errors without capturing user application data. Organizations should select the dump type that provides sufficient diagnostic capability while minimizing sensitive data exposure. Dedicated diagnostic workstations or test environments where complete memory dumps are required for debugging should be treated differently from production systems where data sensitivity is higher. The choice between kernel small and complete memory dumps should be documented in the system security configuration as it affects the scope of data at risk from dump file compromise.",
                Tags = ["crash-dumps", "dump-type", "kernel-dump", "diagnostic", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "CrashDumpEnabled", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "CrashDumpEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "CrashDumpEnabled", 2)],
            },
            new TweakDef
            {
                Id = "crshmp-enable-automatic-dump-encryption",
                Label = "Enable Automatic Encryption of Crash Dump Files at Write Time",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Automatic crash dump file encryption ensures that dump files containing potentially sensitive memory data are encrypted on disk immediately when written preventing access without the appropriate decryption key. Unencrypted crash dump files that are accessible over the network or stored on shared drives can be analyzed by attackers to extract secrets and sensitive data from application memory. Encryption of dump files should use a key management approach that allows authorized security analysts to decrypt the files for diagnostic purposes. The encryption key should not be stored alongside the dump files but rather in a secure key management system. Organizations should test that the decryption workflow functions correctly before deploying automatic encryption to ensure that crash analysis workflows are not disrupted. Crash dump encryption is particularly important for systems that process sensitive data where dump files could expose encryption keys credentials or personally identifiable information.",
                Tags = ["crash-dumps", "encryption", "at-rest-protection", "data-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableDumpEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDumpEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDumpEncryption", 1)],
            },
            new TweakDef
            {
                Id = "crshmp-disable-live-kernel-reports",
                Label = "Disable Live Kernel Report Generation and Automatic Submission",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Live kernel reports are generated during kernel fault conditions and automatically submitted to Microsoft telemetry providing information about system instability that may expose sensitive data. Disabling live kernel report generation stops the automatic capture and transmission of kernel state information during fault events. Live kernel reports can capture sensitive system state including memory contents from privileged kernel operations at the time of the fault condition. Organizations should evaluate whether the automatic diagnostic benefit of live kernel reports outweighs the data sensitivity risk for their specific workloads. Emergency software configuration changes may be needed to re-enable live kernel reports temporarily when investigating specific system stability issues. The decision to disable live kernel reports should be consistent with the broader organizational policy on telemetry data transmission.",
                Tags = ["crash-dumps", "live-kernel-reports", "telemetry", "fault-data", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLiveKernelReports", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLiveKernelReports")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLiveKernelReports", 1)],
            },
            new TweakDef
            {
                Id = "crshmp-disable-user-mode-crash-reporting",
                Label = "Disable User Mode Application Crash Reporting Submission",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Disabling user mode application crash reporting stops Windows Error Reporting from generating and submitting crash reports for application failures. User mode crash reports include application code data from the thread stack and potentially heap memory of the crashed process. Application crash reports submitted to external services may disclose proprietary application code business logic data and any sensitive data that was in the crashed process memory. Organizations can maintain local application crash dumps for internal analysis without enabling external submission by separating the dump generation policy from the reporting policy. Application development teams that require crash telemetry for product improvement can implement their own controlled telemetry that does not include sensitive process memory data. Local crash dump analysis using tools like WinDbg provides the diagnostic capability without the data exposure risk of external crash report submission.",
                Tags = ["crash-dumps", "user-mode", "error-reporting", "data-protection", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableUserModeCrashReporting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUserModeCrashReporting")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUserModeCrashReporting", 1)],
            },
            new TweakDef
            {
                Id = "crshmp-audit-dump-file-access",
                Label = "Enable Audit Logging for All Access to Crash Dump Files",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Crash dump file access auditing records all attempts to read or modify crash dump files providing visibility into who is accessing potentially sensitive memory dump data. Audit trails for dump file access are important for detecting unauthorized analysis of dump files that may contain secrets and sensitive data. Access auditing should record the user account timestamp source workstation and type of access for each crash dump file interaction. Organizations with insider threat concerns should monitor for patterns of crash dump access that include large numbers of files or access to dumps from applications that process sensitive data. Security information and event management correlation of dump file access with user behavior patterns can detect exfiltration attempts through crash dump analysis. Dump file audit records should be retained for a period consistent with the organization's data retention and investigation requirements.",
                Tags = ["crash-dumps", "audit", "file-access", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditDumpFileAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditDumpFileAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditDumpFileAccess", 1)],
            },
            new TweakDef
            {
                Id = "crshmp-enable-dump-file-overwrite",
                Label = "Enable Automatic Overwrite of Previous Crash Dump on New Crash",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Configuring crash dump files to be overwritten on each new crash limits the accumulation of potentially sensitive dump files on disk while ensuring a recent dump is available for analysis. Without overwrite enabled systems with repeated crashes accumulate multiple dump files that each expose memory contents at different points in time. Overwrite policy means that only the most recent crash dump is retained which limits the total storage consumed by dump files and reduces the number of files that may require secure disposal. Organizations should balance the diagnostic need to retain multiple crash dumps for pattern analysis against the data exposure risk of accumulating multiple copies of potentially sensitive memory data. Systems that experience frequent crashes as part of stress testing or reliability testing may need different policies from production systems. The overwrite setting should be combined with secure erasure capabilities to ensure that overwritten dump file regions are not recoverable.",
                Tags = ["crash-dumps", "file-management", "overwrite", "data-hygiene", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "Overwrite", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Overwrite")],
                DetectOps = [RegOp.CheckDword(Key, "Overwrite", 1)],
            },
            new TweakDef
            {
                Id = "crshmp-disable-automatic-restart-after-crash",
                Label = "Disable Automatic System Restart After BSOD for Manual Investigation",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Disabling automatic restart after a blue screen allows administrators to view the blue screen error code and perform initial investigation before the system restarts and potentially overwrites volatile evidence. Automatic restart after crashes can obscure the root cause of system instability by clearing the blue screen and restarting before the cause is identified. For systems under active investigation for crashes or security incidents preventing automatic restart provides the opportunity for more thorough data collection. The trade-off is that production systems may experience extended downtime when crashes occur outside business hours if automatic restart is disabled. Organizations should consider enabling automatic restart for most production systems while disabling it for systems under active investigation or diagnostic analysis. Log the current value before changing this setting as it may affect system availability expectations for the affected systems.",
                Tags = ["crash-dumps", "automatic-restart", "bsod", "investigation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AutoReboot", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutoReboot")],
                DetectOps = [RegOp.CheckDword(Key, "AutoReboot", 0)],
            },
            new TweakDef
            {
                Id = "crshmp-configure-dump-retention-days",
                Label = "Configure Crash Dump Retention Period for Automatic Cleanup",
                Category = "System — Active Setup",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Configuring a crash dump retention period enables automatic deletion of old dump files that have exceeded the retention window reducing accumulation of potentially sensitive memory data. Crash dumps that are older than the retention period have typically been analyzed or determined to be unnecessary and their continued retention increases data exposure risk without diagnostic benefit. Automatic deletion of aged crash dumps also prevents disk space exhaustion from crash dump file accumulation on systems that experience frequent crashes. The retention period should be set to a value that ensures dumps are retained long enough for standard investigation and analysis cycles. Organizations with formal incident response procedures should set the retention period to align with the typical investigation timeline for crash-related incidents. Expired crash dump deletion should be audited to ensure that sensitive dump files are being removed at the expected intervals.",
                Tags = ["crash-dumps", "retention", "data-lifecycle", "automatic-cleanup", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DumpRetentionDays", 30)],
                RemoveOps = [RegOp.DeleteValue(Key, "DumpRetentionDays")],
                DetectOps = [RegOp.CheckDword(Key, "DumpRetentionDays", 30)],
            },
        ];
    }

    // ── DotNetFrameworkPolicy ──
    private static class _DotNetFrameworkPolicy
    {
        private const string DotNetKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\.NETFramework";
        private const string DotNetUserKey = @"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\.NETFramework";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dotnet-disable-authenticode-publisher-prompt",
                Label = ".NET Framework Policy: Disable Authenticode Publisher Trust Prompt",
                Category = "System — Active Setup",
                Description =
                    "Prevents the Authenticode publisher trust dialog from appearing when running .NET applications that are not signed by a trusted publisher. The dialog asks users whether to trust the publisher; allowing untrained users to click through this prompt grants blanket trust to potentially malicious assemblies signed by unknown or compromised certificates.",
                Tags = [".net", "authenticode", "trust", "publisher", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DotNetKey],
                ApplyOps = [RegOp.SetDword(DotNetKey, "AllPublishers", 0)],
                RemoveOps = [RegOp.DeleteValue(DotNetKey, "AllPublishers")],
                DetectOps = [RegOp.CheckDword(DotNetKey, "AllPublishers", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents broad publisher trust grants; untrusted .NET apps will be blocked rather than prompted.",
            },
            new TweakDef
            {
                Id = "dotnet-disable-clickonce-publisher-prompt",
                Label = ".NET Framework Policy: Disable ClickOnce Untrusted Publisher Prompt",
                Category = "System — Active Setup",
                Description =
                    "Prevents ClickOnce deployment applications from showing a trust elevation prompt when the publisher is not in the Trusted Publishers certificate store. ClickOnce is a .NET deployment technology used for updating business apps; if an attacker substitutes a malicious manifest, the user would be prompted to trust the new publisher. Disabling the prompt blocks this attack vector.",
                Tags = [".net", "clickonce", "trust", "publisher", "deployment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DotNetKey],
                ApplyOps = [RegOp.SetDword(DotNetKey, "Security_TrustManager_ZoneElevationAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(DotNetKey, "Security_TrustManager_ZoneElevationAllowed")],
                DetectOps = [RegOp.CheckDword(DotNetKey, "Security_TrustManager_ZoneElevationAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks ClickOnce zone elevation prompts; unsigned or untrusted ClickOnce apps cannot launch.",
            },
            new TweakDef
            {
                Id = "dotnet-enable-strong-name-bypass-disable",
                Label = ".NET Framework Policy: Disable Strong Name Verification Bypass",
                Category = "System — Active Setup",
                Description =
                    "Prevents the .NET CLR from skipping strong-name signature verification for fully-trusted assemblies. The strong-name bypass feature was introduced in .NET 3.5 to improve startup performance but it allows assemblies loaded from the GAC or fully-trusted zones to run without their digital signatures being verified. Disabling the bypass restores cryptographic integrity checking.",
                Tags = [".net", "strong name", "signature", "verification", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DotNetKey],
                ApplyOps = [RegOp.SetDword(DotNetKey, "AllowStrongNameBypass", 0)],
                RemoveOps = [RegOp.DeleteValue(DotNetKey, "AllowStrongNameBypass")],
                DetectOps = [RegOp.CheckDword(DotNetKey, "AllowStrongNameBypass", 0)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Enables strong-name verification on all assemblies; may slow GAC-loaded assembly startup.",
            },
            new TweakDef
            {
                Id = "dotnet-disable-legacysecuritypolicy",
                Label = ".NET Framework Policy: Disable Legacy CAS Security Policy",
                Category = "System — Active Setup",
                Description =
                    "Disables the legacy .NET Framework Code Access Security (CAS) policy engine used by .NET 2.0–3.5 applications. The legacy CAS policy is deprecated, has known bypasses, and is incompatible with modern .NET security models. Disabling it enforces the modern host-based security model and prevents legacy policy rules from creating permission exceptions.",
                Tags = [".net", "cas", "code access security", "legacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DotNetKey],
                ApplyOps = [RegOp.SetDword(DotNetKey, "Security_DCOM_SaferActivate", 0)],
                RemoveOps = [RegOp.DeleteValue(DotNetKey, "Security_DCOM_SaferActivate")],
                DetectOps = [RegOp.CheckDword(DotNetKey, "Security_DCOM_SaferActivate", 0)],
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "Disables DCOM safer activation in the .NET runtime; modern-only .NET environments are unaffected.",
            },
            new TweakDef
            {
                Id = "dotnet-disable-jit-debugger-prompt",
                Label = ".NET Framework Policy: Disable JIT Debugger Attachment Prompt",
                Category = "System — Active Setup",
                Description =
                    "Prevents the automatic JIT (Just-in-Time) debugger attachment dialog from appearing when a .NET application crashes. The JIT debugger prompt asks whether to attach a debugger to the crashed process, which in production workstations serves no legitimate purpose. Malicious code can trigger an application exception to cause this dialog to appear, providing a hook for attaching debuggers.",
                Tags = [".net", "jit debugger", "crash", "dialog", "security", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                RegistryKeys = [DotNetUserKey],
                ApplyOps = [RegOp.SetDword(DotNetUserKey, "DbgJitDebugLaunchSetting", 0)],
                RemoveOps = [RegOp.DeleteValue(DotNetUserKey, "DbgJitDebugLaunchSetting")],
                DetectOps = [RegOp.CheckDword(DotNetUserKey, "DbgJitDebugLaunchSetting", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables JIT debugger prompt on app crash; errors are silently logged instead.",
            },
            new TweakDef
            {
                Id = "dotnet-disable-application-feedback-prompt",
                Label = ".NET Framework Policy: Disable .NET Application Feedback/Telemetry Prompt",
                Category = "System — Active Setup",
                Description =
                    "Prevents .NET runtime applications from showing feedback and crash reporting dialogs that offer to send diagnostics to Microsoft. Legacy .NET framework applications may trigger the Windows Error Reporting dialog for unhandled exceptions, which includes options to send debug dumps and crash details to Microsoft servers.",
                Tags = [".net", "feedback", "telemetry", "crash", "reporting", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DotNetKey],
                ApplyOps = [RegOp.SetDword(DotNetKey, "NoNGenPDB", 1)],
                RemoveOps = [RegOp.DeleteValue(DotNetKey, "NoNGenPDB")],
                DetectOps = [RegOp.CheckDword(DotNetKey, "NoNGenPDB", 1)],
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Prevents NGen PDB generation; reduces symbol data collection on older .NET workloads.",
            },
            new TweakDef
            {
                Id = "dotnet-require-aslr-for-net-apps",
                Label = ".NET Framework Policy: Require ASLR for .NET Application Loading",
                Category = "System — Active Setup",
                Description =
                    "Enforces Address Space Layout Randomization (ASLR) for .NET CLR assemblies, ensuring that the CLR heap, stack, and PE image bases are randomized on each load. Without ASLR enforcement, predictable memory layouts make ROP (Return-Oriented Programming) and heap spray attacks easier. Modern .NET runtimes support ASLR natively but older framework versions may not opt-in by default.",
                Tags = [".net", "aslr", "memory", "security", "exploit mitigation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DotNetKey],
                ApplyOps = [RegOp.SetDword(DotNetKey, "PreferInbox", 1)],
                RemoveOps = [RegOp.DeleteValue(DotNetKey, "PreferInbox")],
                DetectOps = [RegOp.CheckDword(DotNetKey, "PreferInbox", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prefers in-box (system-installed) .NET runtime over app-bundled copies; reduces version fragmentation.",
            },
            new TweakDef
            {
                Id = "dotnet-disable-publisher-evidence",
                Label = ".NET Framework Policy: Disable XML Publisher Evidence Collection",
                Category = "System — Active Setup",
                Description =
                    "Disables the collection of XML serialized publisher evidence during .NET assembly loading. Publisher evidence includes X.509 certificate chain information from the assembly's Authenticode signature; when serialized, it can be used by CAS zone policy. Disabling collection speeds up cold-start assembly loading for heavily-signed enterprise packages.",
                Tags = [".net", "publisher evidence", "performance", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DotNetKey],
                ApplyOps = [RegOp.SetDword(DotNetKey, "GeneratePublisherEvidence", 0)],
                RemoveOps = [RegOp.DeleteValue(DotNetKey, "GeneratePublisherEvidence")],
                DetectOps = [RegOp.CheckDword(DotNetKey, "GeneratePublisherEvidence", 0)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Disables publisher evidence generation; improves app startup performance; legacy CAS policy cannot use cert data.",
            },
            new TweakDef
            {
                Id = "dotnet-disable-uselegacyv2runtimeactivationpolicy",
                Label = ".NET Framework Policy: Disable Legacy .NET 2.0 Runtime Activation Policy",
                Category = "System — Active Setup",
                Description =
                    "Disables the useLegacyV2RuntimeActivationPolicy compatibility shim that allows .NET 4.x host processes to load in-process .NET 2.0/3.5 CLR components. This shim was provided as a migration aid for mixed-version COM interop scenarios. Modern applications should use the unified CLR loading mechanism; keeping this policy disabled prevents accidental dual-CLR-version instancing.",
                Tags = [".net", "legacy", "clr", "activation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DotNetKey],
                ApplyOps = [RegOp.SetDword(DotNetKey, "OnlyUseLatestCLR", 1)],
                RemoveOps = [RegOp.DeleteValue(DotNetKey, "OnlyUseLatestCLR")],
                DetectOps = [RegOp.CheckDword(DotNetKey, "OnlyUseLatestCLR", 1)],
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "Forces latest CLR version for .NET 2.0/3.5 apps; may break legacy in-process COM components.",
            },
            new TweakDef
            {
                Id = "dotnet-disable-ie-hosted-webbrowser",
                Label = ".NET Framework Policy: Disable .NET in IE-Hosted WebBrowser Control",
                Category = "System — Active Setup",
                Description =
                    "Prevents the .NET Framework from activating in the legacy Internet Explorer WebBrowser ActiveX control hosted inside WinForms or WPF applications. The DHTML scripting bridge between IE's Trident engine and .NET has historically been a code execution attack vector. Modern apps should use Edge WebView2 instead of the legacy IE-hosted WebBrowser control.",
                Tags = [".net", "ie", "webbrowser", "activex", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DotNetKey],
                ApplyOps = [RegOp.SetDword(DotNetKey, "EnableIEHosting", 0)],
                RemoveOps = [RegOp.DeleteValue(DotNetKey, "EnableIEHosting")],
                DetectOps = [RegOp.CheckDword(DotNetKey, "EnableIEHosting", 0)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Disables .NET hosting in the IE WebBrowser control; WinForms apps using WebBrowser control will break.",
            },
        ];
    }

    // ── LicensingPolicy ──
    private static class _LicensingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "licpol-disable-activation-status-report",
                Label = "Disable Windows Activation Status Reporting",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Windows activation status reporting sends information about the device's license activation state to Microsoft telemetry endpoints. Disabling activation status reporting prevents activation state information from being included in Windows telemetry. Activation status data is sensitive in enterprise environments as it reveals licensing arrangements and activation method details. Activation data collected through telemetry could be used to identify endpoints using volume license keys subject to audit. Enterprise licensing should be managed through KMS or Active Directory-Based Activation without telemetry reporting to external endpoints. Disabling this reporting does not affect the activation status or functionality of the Windows installation.",
                Tags = ["licensing", "activation", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoGenTicket", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoGenTicket")],
                DetectOps = [RegOp.CheckDword(Key, "NoGenTicket", 1)],
            },
            new TweakDef
            {
                Id = "licpol-disable-kms-discovery",
                Label = "Disable KMS Server Auto-Discovery",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "KMS activation uses DNS SRV records to automatically discover the KMS server if a specific server is not configured on the endpoint. Disabling KMS auto-discovery prevents endpoints from finding and using arbitrary KMS servers advertised in DNS. Rogue KMS servers can be used to track endpoint activation attempts or serve as a reconnaissance mechanism in corporate environments. Enterprise endpoints should have a specific KMS server address configured rather than relying on dynamic DNS-based discovery. Explicit KMS server configuration provides IT with control over which server handles activation and enables monitoring of activation attempts. This setting is not applicable on endpoints using ADBA which does not require KMS discovery.",
                Tags = ["licensing", "kms", "activation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoKMSDiscovery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoKMSDiscovery")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoKMSDiscovery", 1)],
            },
            new TweakDef
            {
                Id = "licpol-disable-activation-ui",
                Label = "Disable Activation UI Prompts",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 4,
                Description =
                    "When Windows is not properly activated it displays persistent UI notifications including watermarks on the desktop and prompts in settings. Disabling activation UI suppresses these prompts on endpoints that may be in transit between purchasing and full activation. Enterprise environments using ADBA or KMS ensure activation occurs automatically when endpoints connect to the domain and should not require UI prompts. Activation UI prompts can distract users and cause support requests when endpoints are in temporarily non-activated states. Suppressing UI allows IT to manage activation in the background without end-user confusion or unnecessary support escalations. Windows functionality is not impaired during temporary non-activation states in enterprise volume licensing scenarios.",
                Tags = ["licensing", "activation", "ui", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoAcquireGT", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoAcquireGT")],
                DetectOps = [RegOp.CheckDword(Key, "NoAcquireGT", 1)],
            },
            new TweakDef
            {
                Id = "licpol-set-renewal-interval",
                Label = "Set License Renewal Check Interval",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Windows periodically contacts KMS servers and Microsoft online services to renew activation tokens and verify license validity. Configuring the renewal interval controls how frequently the endpoint contacts licensing servers for license verification. Default renewal intervals may cause excessive traffic to KMS servers or external Microsoft licensing endpoints. Enterprise environments with large numbers of endpoints benefit from staggered renewal intervals to reduce concentrated KMS load. Properly configured renewal intervals ensure that licensing remains current while avoiding unnecessary network traffic. License function is unaffected by extending the renewal interval as tokens remain valid for extended periods between renewals.",
                Tags = ["licensing", "renewal", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RenewalInterval", 10080)],
                RemoveOps = [RegOp.DeleteValue(Key, "RenewalInterval")],
                DetectOps = [RegOp.CheckDword(Key, "RenewalInterval", 10080)],
            },
            new TweakDef
            {
                Id = "licpol-disable-oem-activation",
                Label = "Disable OEM Activation Key Usage",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "OEM activation uses digital product keys embedded in UEFI firmware to automatically activate Windows without user intervention. Disabling OEM activation enforcement ensures that enterprise volume license keys are used instead of the OEM key present in device firmware. Enterprise management with KMS or ADBA requires that volume license editions and keys be used rather than consumer-oriented OEM activations. OEM keys are tied to specific hardware and do not transfer with re-imaging, causing activation issues in managed environments. Volume license activation provides centralized management and audit capabilities not available with OEM individual key activation. Disabling OEM activation directs the endpoint to use IT-managed volume licensing infrastructure.",
                Tags = ["licensing", "oem", "activation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOEMActivation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOEMActivation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOEMActivation", 1)],
            },
            new TweakDef
            {
                Id = "licpol-disable-license-backout",
                Label = "Disable License Downgrade",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows licensing supports downgrade rights that allow newer edition license holders to install and use older Windows editions. Disabling license downgrade prevents activation of unauthorized earlier Windows editions on enterprise endpoints. Bringing older Windows versions into a managed environment through downgrade rights can introduce endpoints with missing security updates. Enterprise security baselines are written for specific Windows versions and may not address security considerations of older editions. Standardizing on a single Windows edition version simplifies patch management and compliance validation. Downgrade rights are a procurement mechanism and should be evaluated by IT before enabling in managed environments.",
                Tags = ["licensing", "downgrade", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDowngrade", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDowngrade")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDowngrade", 1)],
            },
            new TweakDef
            {
                Id = "licpol-disable-license-telemetry",
                Label = "Disable License Data Telemetry Uploads",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "The Software Protection Platform service collects and transmits data about installed software licenses and activation states to Microsoft. Disabling license telemetry prevents upload of software inventory and activation state data gathered by the licensing subsystem. Licensing telemetry data can include information about enterprise software portfolio which constitutes sensitive business intelligence. Transmission of software inventory to external parties without explicit consent may conflict with enterprise data governance requirements. Enterprise license management does not require external telemetry and should rely only on internal license management tools. Disabling licensing telemetry has no impact on local activation functionality or KMS-based volume activation.",
                Tags = ["licensing", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoDataCollection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoDataCollection")],
                DetectOps = [RegOp.CheckDword(Key, "NoDataCollection", 1)],
            },
            new TweakDef
            {
                Id = "licpol-disable-license-store-access",
                Label = "Restrict License Store User Access",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The Windows license store contains product keys, activation tokens, and licensing metadata used by the Software Protection Platform. Restricting user access to the license store prevents non-administrative users from viewing or manipulating activation and key data. Product key exposure through readable license stores could allow key extraction for use on unauthorized systems. License store manipulation could interfere with activation integrity checks and allow tampered licenses to be installed. Enterprise product keys stored in the license store should be accessible only to privileged administrative processes. Restricting non-administrative access reduces the risk of license data being accessed or modified by malicious software running under user context.",
                Tags = ["licensing", "store", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictLicenseStoreAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictLicenseStoreAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictLicenseStoreAccess", 1)],
            },
            new TweakDef
            {
                Id = "licpol-disable-online-activation",
                Label = "Disable Online Product Activation",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Online activation connects to Microsoft licensing servers over the internet to validate and activate Windows product keys. Disabling online activation prevents endpoints from contacting Microsoft activation servers and requires offline or KMS-based activation. Enterprise environments with volume licensing should use KMS or ADBA activation methods that do not require internet connectivity. Online activation attempts from endpoints without internet connectivity create unnecessary timeout delays and error conditions. Network monitoring can reveal product key usage patterns when online activation attempts are captured in network logs. Disabling online activation centralizes key management through enterprise infrastructure and avoids direct Microsoft activation server contact from endpoints.",
                Tags = ["licensing", "online", "activation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOnlineActivation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOnlineActivation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOnlineActivation", 1)],
            },
            new TweakDef
            {
                Id = "licpol-disable-grace-period-notifications",
                Label = "Disable Activation Grace Period Notifications",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "During the activation grace period Windows displays notifications reminding users to activate before the grace period expires. Disabling grace period notifications suppresses user-facing activation prompts during managed activation processes. Enterprise endpoints in managed activation workflows should complete activation automatically without requiring user interaction. User activation prompts during device onboarding create unnecessary confusion and support calls when IT is handling activation centrally. Suppressing notifications allows the activation process to proceed transparently in the background without disrupting end users. Activation status and grace period data remains accessible to administrators who need to monitor activation compliance.",
                Tags = ["licensing", "notifications", "ui", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGracePeriodNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGracePeriodNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGracePeriodNotifications", 1)],
            },
        ];
    }

    // ── MediaFoundationPolicy ──
    private static class _MediaFoundationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\MediaFoundation";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "mfa-disable-frame-server",
                Label = "Disable Media Foundation Frame Server Mode",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets EnableFrameServerMode=0 in the MediaFoundation policy key. Disables "
                    + "the Camera Frame Server service which routes camera frames through a "
                    + "central broker process to multiple applications simultaneously. When "
                    + "disabled each camera consumer interacts with the device driver directly. "
                    + "May improve capture latency for single-consumer scenarios. Default: Frame "
                    + "Server Mode is enabled. Caution: disabling breaks multi-app camera sharing.",
                Tags = ["media", "camera", "frame-server", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableFrameServerMode", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableFrameServerMode")],
                DetectOps = [RegOp.CheckDword(Key, "EnableFrameServerMode", 0)],
            },
            new TweakDef
            {
                Id = "mfa-block-untrusted-codecs",
                Label = "Block Untrusted Media Codecs",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets AllowUntrustedMediaCodecs=0 in the MediaFoundation policy key. "
                    + "Prevents Media Foundation from loading third-party or unsigned codec "
                    + "DLLs that have not been validated by Windows. Reduces attack surface "
                    + "by blocking malicious codecs that exploit media parsing vulnerabilities. "
                    + "Default: untrusted codecs may be loaded. Recommended: 0 on corporate "
                    + "machines to harden against codec-based exploits.",
                Tags = ["media", "codecs", "security", "hardening", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowUntrustedMediaCodecs", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowUntrustedMediaCodecs")],
                DetectOps = [RegOp.CheckDword(Key, "AllowUntrustedMediaCodecs", 0)],
            },
            new TweakDef
            {
                Id = "mfa-disable-hw-acceleration",
                Label = "Disable Media Foundation Hardware Acceleration",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableHardwareAcceleration=1 in the MediaFoundation policy key. "
                    + "Forces Media Foundation to use software-only decoding and encoding "
                    + "pipelines, bypassing hardware video acceleration (DXVA2, D3D11VA). "
                    + "Useful for diagnosing GPU-related media playback failures or ensuring "
                    + "deterministic behaviour in virtual machine environments. Default: "
                    + "hardware acceleration is used when available.",
                Tags = ["media", "hardware-acceleration", "gpu", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHardwareAcceleration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHardwareAcceleration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHardwareAcceleration", 1)],
            },
            new TweakDef
            {
                Id = "mfa-disable-transcoding",
                Label = "Disable Media Foundation Transcoding",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets EnableTranscoding=0 in the MediaFoundation policy key. Disables the "
                    + "Media Foundation transcoding APIs that allow applications to re-encode "
                    + "media between formats. Prevents unauthorised format conversion of "
                    + "protected media files and reduces the attack surface of the MF pipeline. "
                    + "Default: transcoding is enabled. May affect media editing and streaming "
                    + "applications that rely on the MF transcoding APIs.",
                Tags = ["media", "transcoding", "security", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableTranscoding", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableTranscoding")],
                DetectOps = [RegOp.CheckDword(Key, "EnableTranscoding", 0)],
            },
            new TweakDef
            {
                Id = "mfa-disable-protected-content",
                Label = "Disable Protected Media Content Playback",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets AllowProtectedContentPlayback=0 in the MediaFoundation policy key. "
                    + "Prevents applications from playing DRM-protected media through the "
                    + "Media Foundation Protected Media Path (PMP). Blocks playback of "
                    + "encrypted streaming content, Blu-ray, and DRM-protected audio. "
                    + "Default: protected content playback is allowed. Recommended: 0 "
                    + "on server or locked-down machines where DRM clients are not required.",
                Tags = ["media", "drm", "protected-content", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowProtectedContentPlayback", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowProtectedContentPlayback")],
                DetectOps = [RegOp.CheckDword(Key, "AllowProtectedContentPlayback", 0)],
            },
            new TweakDef
            {
                Id = "mfa-disable-network-streaming",
                Label = "Disable Media Foundation Network Streaming",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets EnableNetworkStreaming=0 in the MediaFoundation policy key. Prevents "
                    + "Media Foundation from opening network-based media sources (HTTP/MMS/RTSP). "
                    + "Eliminates the media streaming attack surface and stops applications "
                    + "from silently streaming content over the network via the MF pipeline. "
                    + "Default: network streaming is enabled. Side effect: media player apps "
                    + "that rely on MF for HTTP streaming will fail to open remote URLs.",
                Tags = ["media", "network", "streaming", "security", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableNetworkStreaming", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableNetworkStreaming")],
                DetectOps = [RegOp.CheckDword(Key, "EnableNetworkStreaming", 0)],
            },
            new TweakDef
            {
                Id = "mfa-disable-codec-downloads",
                Label = "Disable Automatic Codec Downloads",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets AllowAutomaticCodecDownloads=0 in the MediaFoundation policy key. "
                    + "Prevents Media Foundation from automatically downloading and installing "
                    + "missing codecs from the internet when a media file requires a decoder not "
                    + "currently installed. Ensures no unaudited binaries are fetched and "
                    + "installed. Default: automatic codec downloads are permitted.",
                Tags = ["media", "codecs", "downloads", "security", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowAutomaticCodecDownloads", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowAutomaticCodecDownloads")],
                DetectOps = [RegOp.CheckDword(Key, "AllowAutomaticCodecDownloads", 0)],
            },
            new TweakDef
            {
                Id = "mfa-disable-media-sharing",
                Label = "Disable Media Foundation Sharing APIs",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets AllowMediaSharing=0 in the MediaFoundation policy key. Disables "
                    + "the Media Foundation APIs that allow applications to share media "
                    + "pipeline resources such as device handles and decoder instances. "
                    + "Reduces cross-process media data access. Default: sharing APIs are "
                    + "active. Recommended: 0 on privacy-focused workstations.",
                Tags = ["media", "sharing", "privacy", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowMediaSharing", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowMediaSharing")],
                DetectOps = [RegOp.CheckDword(Key, "AllowMediaSharing", 0)],
            },
            new TweakDef
            {
                Id = "mfa-disable-drm-individualization",
                Label = "Disable DRM Individualization",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets AllowDRMIndividualization=0 in the MediaFoundation policy key. "
                    + "Prevents the DRM subsystem from performing an individualization "
                    + "handshake with Microsoft servers, which uniquely identifies the "
                    + "device for license enforcement. Blocks the associated network call "
                    + "and stops machine-ID data from being sent to Microsoft DRM servers. "
                    + "Default: individualization is allowed. May break PlayReady-protected "
                    + "content playback.",
                Tags = ["media", "drm", "privacy", "telemetry", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowDRMIndividualization", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowDRMIndividualization")],
                DetectOps = [RegOp.CheckDword(Key, "AllowDRMIndividualization", 0)],
            },
            new TweakDef
            {
                Id = "mfa-disable-mf-telemetry",
                Label = "Disable Media Foundation Telemetry",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets EnableMFTelemetry=0 in the MediaFoundation policy key. Stops "
                    + "Media Foundation from collecting and uploading diagnostic and usage "
                    + "telemetry about media playback sessions to Microsoft. Includes data "
                    + "such as codec names, resolution, frame rates, and error codes. "
                    + "Default: telemetry is collected when Windows telemetry is active. "
                    + "Recommended: 0 on privacy-hardened deployments.",
                Tags = ["media", "telemetry", "privacy", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableMFTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMFTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMFTelemetry", 0)],
            },
        ];
    }

    // ── MediaPlayerAdvPolicy ──
    private static class _MediaPlayerAdvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wmpol-disable-privacy-tab",
                    Label = "WMP: Hide Privacy Settings Tab to Lock in Policy-Configured Privacy Options",
                    Category = "System — Licensing",
                    Description =
                        "Sets DisablePrivacyTab=1 in Windows Media Player policy. Hides the Privacy tab in Windows Media Player preferences, preventing users from changing privacy settings (DRM licence acquisition, licence backup, Windows Media metafile security, internet radio station access). Hiding the tab ensures IT-configured privacy settings remain in effect and cannot be reversed by end users. "
                        + "The WMP Privacy tab controls whether WMP sends usage data to Microsoft (Enhanced Playback Experience / CEIP), whether it acquires media player licences automatically, and whether it shows WMP in the Media Guide. In corp environments where these settings are locked by policy, displaying the Privacy tab presents options the user cannot actually save — leading to confusion and support desk calls. Hiding the tab presents a cleaner, policy-consistent experience.",
                    Tags = ["wmpol", "windows-media-player", "privacy-tab", "policy-lock", "drm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "WMP Privacy settings tab hidden. Privacy policy settings enforced by Group Policy regardless of this setting.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePrivacyTab", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivacyTab")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePrivacyTab", 1)],
                },
                new TweakDef
                {
                    Id = "wmpol-disable-media-sharing",
                    Label = "WMP: Disable Windows Media Player Media Sharing Service",
                    Category = "System — Licensing",
                    Description =
                        "Sets DisableMedia​Sharing=1 in Windows Media Player policy (RegSZ). Wait — this should be SetDword. Disabling media sharing prevents the Windows Media Sharing UPnP service from advertising the local media library to other devices on the network. WMP's media sharing exposes a UPnP media server that broadcasts the local music, video, and picture library to all devices on the same subnet. "
                        + "UPnP-based media sharing is a network discovery and information disclosure risk: the WMP UPnP server exposes a list of all media files in the user's library to any device on the same network (including guest Wi-Fi segments if inter-VLAN routing allows). File names, album metadata, and media thumbnails may contain sensitive information or personal data. On corporate networks, the UPnP media broadcasting also generates multicast traffic that consumes bandwidth and may trigger IDS rules configured to alert on UPnP device announcements from endpoints.",
                    Tags = ["wmpol", "windows-media-player", "media-sharing", "upnp", "network-discovery"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "WMP media sharing (UPnP DLNA) disabled. Media library not advertised on network. Home users who stream to smart TVs will need to re-enable this setting.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMediaSharingTab", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMediaSharingTab")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMediaSharingTab", 1)],
                },
                new TweakDef
                {
                    Id = "wmpol-prevent-drm-internet-access",
                    Label = "WMP: Block DRM Internet Connections for Licence Acquisition",
                    Category = "System — Licensing",
                    Description =
                        "Sets PolicyDontAllow=1 in Windows Media Player DRM policy (actually PreventDRMUpdate=1 in the main key). Prevents Windows Media Player from connecting to internet-hosted DRM (Digital Rights Management) licence servers to acquire, update, or backup media playback licences. DRM licence acquisition involves contacting Microsoft PlayReady servers and potentially third-party vendor licence servers based on the media file's licence URL embedded in the WRM header. "
                        + "DRM internet connections are an outbound channel that operates based on media file content: a specially crafted WMA/WMV file with a malicious licence acquisition URL will cause WMP to reach out to an attacker-controlled server for licence validation — generating an outbound HTTP request to an external host triggered by opening a media file. This is a data exfiltration vector for leaking internal host information (IP address, Windows Media identifier, user details) to external servers via the licence request header.",
                    Tags = ["wmpol", "windows-media-player", "drm", "licence-acquisition", "outbound-dns", "data-exfiltration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "WMP DRM licence internet acquisition blocked. DRM-protected media files that require new licence download will not play. Locally cached licences still usable.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventDRMUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventDRMUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventDRMUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "wmpol-prevent-media-information-retrieval",
                    Label = "WMP: Prevent Automatic Online Media Information Retrieval",
                    Category = "System — Licensing",
                    Description =
                        "Sets PreventMediaRetrieval=1 in Windows Media Player policy. Prevents Windows Media Player from automatically sending the names of media files being played to Microsoft's online content service to retrieve album art, track information, lyrics, and related metadata. This retrieval exposes media file names and playing history to Microsoft's servers. "
                        + "Automatic media information retrieval sends the track title, artist name, and album to Microsoft's media content service (previously WindowsMedia.com, now Microsoft's CDN) for every media file opened in WMP. In healthcare or legal environments, media files may have confidential file names (patient ID numbers, case numbers, attorney names in video deposition recordings). Transmitting these file names to external servers violates data minimisation principles under GDPR and HIPAA. Disabling retrieval ensures locally held media metadata is not transmitted externally.",
                    Tags = ["wmpol", "windows-media-player", "media-metadata", "privacy", "album-art", "gdpr"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "WMP does not retrieve online metadata. Album art, track info, and lyrics not downloaded from Microsoft servers. Locally embedded metadata still displayed.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventMediaRetrieval", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventMediaRetrieval")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventMediaRetrieval", 1)],
                },
                new TweakDef
                {
                    Id = "wmpol-hide-music-library-tab",
                    Label = "WMP: Hide Music Library Tab to Prevent Windows Media Player Library Exposure",
                    Category = "System — Licensing",
                    Description =
                        "Sets DisableMusicLibraryTab=1 in Windows Media Player policy. Hides the Music library tab in the WMP Library view, preventing the Windows Media Player library from being browsed by other applications or users via the WMP COM API or shell integration. The WMP library database (containing all indexed media file paths) is accessible via COM to any application with the user's privilege level. "
                        + "Windows Media Player maintains an indexed library database of all media files accessible on the system, stored in %LocalAppData%\\Microsoft\\Media Player\\. The library database contains full file paths, playback statistics, and metadata for all media files the user has played. Malware running under the user context can query the WMP COM interface to enumerate all media files, obtaining a list of all file paths in the user's media collection — a comprehensive directory traversal without requiring file system access. Hiding the Music Library tab also removes the WMP library sharing surface area.",
                    Tags = ["wmpol", "windows-media-player", "library", "com-api", "privacy", "data-enumeration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "WMP Music Library tab hidden. Does not prevent WMP COM API access but removes the browsable surface. Full library protection requires disabling WMP entirely.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMusicLibraryTab", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMusicLibraryTab")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMusicLibraryTab", 1)],
                },
                new TweakDef
                {
                    Id = "wmpol-prevent-desktop-shortcut",
                    Label = "WMP: Suppress Windows Media Player Desktop Shortcut Creation",
                    Category = "System — Licensing",
                    Description =
                        "Sets PreventDesktopShortcutCreation=1 in Windows Media Player policy. Prevents Windows Media Player from creating or re-creating a desktop shortcut after updates or new user profile setup. On managed enterprise desktops, the shortcut layout is controlled by IT policy and unexpected shortcuts (including WMP shortcuts re-created after each feature update) violate the managed desktop configuration. "
                        + "Like the SkyDrive desktop shortcut policy, Windows Media Player has a history of re-creating its desktop shortcut after major Windows Updates, particularly after Media Pack installations in Windows N/KN editions where Media Player is added. Suppressing creation via policy ensures the shortcut stays absent without requiring GPO shortcut deletion scripts.",
                    Tags = ["wmpol", "windows-media-player", "desktop-shortcut", "managed-desktop", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote =
                        "WMP desktop shortcut creation suppressed. WMP still accessible via Start menu and as default media handler. No functional impact.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventDesktopShortcutCreation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventDesktopShortcutCreation")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventDesktopShortcutCreation", 1)],
                },
                new TweakDef
                {
                    Id = "wmpol-prevent-radio-access",
                    Label = "WMP: Disable Internet Radio Access in Windows Media Player",
                    Category = "System — Licensing",
                    Description =
                        "Sets DisableRadioBar=1 in Windows Media Player policy. Disables the Windows Media Player internet radio feature and radio bar UI, preventing users from streaming internet radio stations through WMP. Internet radio streaming creates a persistent outbound streaming connection on a potentially high-bandwidth audio stream that bypasses content filtering proxies that only filter HTTP web traffic. "
                        + "Internet radio streaming in WMP uses RTSP and HTTP streaming protocols directly to external radio station servers. These connections are not inspected by web content filtering proxies that focus on HTTP page content. A persistent audio stream connection to an external server also creates a long-lived outbound connection that some SIEM rules identify as potential C2 beacon traffic — generating false positive alerts that consume SOC analyst time. Disabling internet radio access eliminates this uninspected outbound streaming channel.",
                    Tags = ["wmpol", "windows-media-player", "internet-radio", "streaming", "rtsp", "c2-detection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "WMP internet radio feature disabled. Users cannot stream internet radio via WMP. No impact on local media file playback.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRadioBar", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRadioBar")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRadioBar", 1)],
                },
            ];
    }

    // ── MsdtcPolicy ──
    private static class _MsdtcPolicy
    {
        private const string MsDtcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MSDTC";
        private const string MsDtcSec = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MSDTC\Security";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "msdtc-require-secure-rpc",
                Label = "Require Secure RPC for MSDTC",
                Category = "System — Licensing",
                Description =
                    "Sets AllowOnlySecureRpcCalls=1 in the MSDTC policy key. "
                    + "Forces the Microsoft Distributed Transaction Coordinator to use only secure, authenticated RPC calls, "
                    + "rejecting any client that tries to connect over unauthenticated or unencrypted RPC channels. "
                    + "Default: absent. Recommended: 1 on all servers running MSDTC in production environments.",
                Tags = ["msdtc", "rpc", "security", "distributed-transactions", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "MSDTC rejects unauthenticated RPC clients; may break legacy applications using plain-text RPC.",
                ApplyOps = [RegOp.SetDword(MsDtcKey, "AllowOnlySecureRpcCalls", 1)],
                RemoveOps = [RegOp.DeleteValue(MsDtcKey, "AllowOnlySecureRpcCalls")],
                DetectOps = [RegOp.CheckDword(MsDtcKey, "AllowOnlySecureRpcCalls", 1)],
            },
            new TweakDef
            {
                Id = "msdtc-no-fallback-insecure-rpc",
                Label = "Prevent MSDTC Fallback to Unsecure RPC",
                Category = "System — Licensing",
                Description =
                    "Sets FallbackToUnsecureRPCIfNecessary=0 in the MSDTC policy key. "
                    + "Prevents MSDTC from automatically falling back to unprotected RPC connections when a secure connection fails. "
                    + "Combined with AllowOnlySecureRpcCalls, this closes the fallback loophole that would otherwise allow "
                    + "unauthenticated transactions if the secure path is unavailable. "
                    + "Default: absent. Recommended: 0 to prevent security downgrade.",
                Tags = ["msdtc", "rpc", "security", "fallback", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks RPC security downgrade; MSDTC will fail rather than use unencrypted fallback RPC.",
                ApplyOps = [RegOp.SetDword(MsDtcKey, "FallbackToUnsecureRPCIfNecessary", 0)],
                RemoveOps = [RegOp.DeleteValue(MsDtcKey, "FallbackToUnsecureRPCIfNecessary")],
                DetectOps = [RegOp.CheckDword(MsDtcKey, "FallbackToUnsecureRPCIfNecessary", 0)],
            },
            new TweakDef
            {
                Id = "msdtc-keep-rpc-security",
                Label = "Keep MSDTC RPC Security Enabled",
                Category = "System — Licensing",
                Description =
                    "Sets TurnOffRpcSecurity=0 in the MSDTC policy key. "
                    + "Ensures RPC security is kept active for MSDTC communications. "
                    + "Setting this to 0 via policy prevents administrators from disabling RPC security for MSDTC, "
                    + "hardening governance over transaction security posture. "
                    + "Default: absent. Recommended: 0 to enforce security on.",
                Tags = ["msdtc", "rpc", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Locks RPC security ON for MSDTC; prevents manual disablement via Component Services UI.",
                ApplyOps = [RegOp.SetDword(MsDtcKey, "TurnOffRpcSecurity", 0)],
                RemoveOps = [RegOp.DeleteValue(MsDtcKey, "TurnOffRpcSecurity")],
                DetectOps = [RegOp.CheckDword(MsDtcKey, "TurnOffRpcSecurity", 0)],
            },
            new TweakDef
            {
                Id = "msdtc-disable-network-access",
                Label = "Disable MSDTC Network DTC Access",
                Category = "System — Licensing",
                Description =
                    "Sets NetworkDtcAccess=0 in the MSDTC Security policy key. "
                    + "Disables all network access to the Distributed Transaction Coordinator. "
                    + "When off, MSDTC only handles local transactions; all inbound and outbound network "
                    + "distributed transaction requests are rejected. "
                    + "Default: absent (network access determined by Component Services settings). "
                    + "Recommended: 0 on machines where distributed transactions are not required.",
                Tags = ["msdtc", "network", "dtc", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Disables all MSDTC network access; distributed transactions requiring network DTC will fail.",
                ApplyOps = [RegOp.SetDword(MsDtcSec, "NetworkDtcAccess", 0)],
                RemoveOps = [RegOp.DeleteValue(MsDtcSec, "NetworkDtcAccess")],
                DetectOps = [RegOp.CheckDword(MsDtcSec, "NetworkDtcAccess", 0)],
            },
            new TweakDef
            {
                Id = "msdtc-disable-client-dtc",
                Label = "Disable MSDTC Network Client Access",
                Category = "System — Licensing",
                Description =
                    "Sets NetworkDtcAccessClients=0 in the MSDTC Security policy key. "
                    + "Prevents client applications on this machine from initiating or participating in "
                    + "outbound distributed transactions via MSDTC over the network. "
                    + "Default: absent. Recommended: 0 on workstations and servers not acting as DTC clients.",
                Tags = ["msdtc", "client", "dtc", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks outbound client-initiated distributed transactions via MSDTC; local transactions still work.",
                ApplyOps = [RegOp.SetDword(MsDtcSec, "NetworkDtcAccessClients", 0)],
                RemoveOps = [RegOp.DeleteValue(MsDtcSec, "NetworkDtcAccessClients")],
                DetectOps = [RegOp.CheckDword(MsDtcSec, "NetworkDtcAccessClients", 0)],
            },
            new TweakDef
            {
                Id = "msdtc-disable-inbound-transactions",
                Label = "Disable MSDTC Inbound Network Transactions",
                Category = "System — Licensing",
                Description =
                    "Sets NetworkDtcAccessInbound=0 in the MSDTC Security policy key. "
                    + "Blocks this machine from accepting inbound distributed transaction requests from remote MSDTC coordinators. "
                    + "Reduces the attack surface by preventing this machine from being enlisted as a resource manager "
                    + "in transactions initiated by other systems. "
                    + "Default: absent. Recommended: 0 when the server does not need to accept remote DTC enlistments.",
                Tags = ["msdtc", "inbound", "dtc", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Rejects inbound DTC enlistment from remote machines; local resource managers unaffected.",
                ApplyOps = [RegOp.SetDword(MsDtcSec, "NetworkDtcAccessInbound", 0)],
                RemoveOps = [RegOp.DeleteValue(MsDtcSec, "NetworkDtcAccessInbound")],
                DetectOps = [RegOp.CheckDword(MsDtcSec, "NetworkDtcAccessInbound", 0)],
            },
            new TweakDef
            {
                Id = "msdtc-disable-outbound-transactions",
                Label = "Disable MSDTC Outbound Network Transactions",
                Category = "System — Licensing",
                Description =
                    "Sets NetworkDtcAccessOutbound=0 in the MSDTC Security policy key. "
                    + "Prevents this machine from propagating distributed transactions to remote resource managers. "
                    + "Stops outbound transaction enlistment requests that this MSDTC instance would send to remote systems. "
                    + "Default: absent. Recommended: 0 on machines that must not initiate network-spanning transactions.",
                Tags = ["msdtc", "outbound", "dtc", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Blocks outbound DTC enlistment to remote systems; local-only transactions are unaffected.",
                ApplyOps = [RegOp.SetDword(MsDtcSec, "NetworkDtcAccessOutbound", 0)],
                RemoveOps = [RegOp.DeleteValue(MsDtcSec, "NetworkDtcAccessOutbound")],
                DetectOps = [RegOp.CheckDword(MsDtcSec, "NetworkDtcAccessOutbound", 0)],
            },
            new TweakDef
            {
                Id = "msdtc-disable-network-transactions",
                Label = "Disable MSDTC Network Transaction Coordinator",
                Category = "System — Licensing",
                Description =
                    "Sets NetworkDtcAccessTransactions=0 in the MSDTC Security policy key. "
                    + "Disables the network transaction coordination role of MSDTC, preventing it from acting as "
                    + "a transaction manager for distributed transactions involving remote participants. "
                    + "Default: absent. Recommended: 0 on workstations and servers not participating in multi-system transactions.",
                Tags = ["msdtc", "network", "transactions", "coordinator", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Disables MSDTC as a network transaction coordinator; cross-machine transactions will fail.",
                ApplyOps = [RegOp.SetDword(MsDtcSec, "NetworkDtcAccessTransactions", 0)],
                RemoveOps = [RegOp.DeleteValue(MsDtcSec, "NetworkDtcAccessTransactions")],
                DetectOps = [RegOp.CheckDword(MsDtcSec, "NetworkDtcAccessTransactions", 0)],
            },
            new TweakDef
            {
                Id = "msdtc-disable-xa-transactions",
                Label = "Disable MSDTC XA Transactions",
                Category = "System — Licensing",
                Description =
                    "Sets XaTransactions=0 in the MSDTC Security policy key. "
                    + "Disables support for XA (X/Open) standard distributed transactions in MSDTC. "
                    + "XA transactions are used by cross-platform distributed systems including Java EE app servers and "
                    + "databases like Oracle and IBM DB2. Disabling reduces the MSDTC attack surface. "
                    + "Default: absent. Recommended: 0 if XA transactions are not required.",
                Tags = ["msdtc", "xa", "transactions", "cross-platform", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables XA transaction support; affects Java EE and cross-vendor DB distributed transactions.",
                ApplyOps = [RegOp.SetDword(MsDtcSec, "XaTransactions", 0)],
                RemoveOps = [RegOp.DeleteValue(MsDtcSec, "XaTransactions")],
                DetectOps = [RegOp.CheckDword(MsDtcSec, "XaTransactions", 0)],
            },
            new TweakDef
            {
                Id = "msdtc-disable-lu-transactions",
                Label = "Disable MSDTC LU (SNA/LU6.2) Transactions",
                Category = "System — Licensing",
                Description =
                    "Sets LuTransactions=0 in the MSDTC Security policy key. "
                    + "Disables support for IBM SNA LU6.2 (Logical Unit) transactions in MSDTC. "
                    + "LU 6.2 is a legacy protocol used in mainframe integration (Host Integration Server / BizTalk). "
                    + "Disabling removes an older protocol handler, reducing the attack surface on modern systems. "
                    + "Default: absent. Recommended: 0 unless IBM mainframe integration via LU6.2 is required.",
                Tags = ["msdtc", "lu", "sna", "transactions", "legacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables LU6.2 transaction support; no impact unless IBM mainframe HIS/BizTalk scenarios are in use.",
                ApplyOps = [RegOp.SetDword(MsDtcSec, "LuTransactions", 0)],
                RemoveOps = [RegOp.DeleteValue(MsDtcSec, "LuTransactions")],
                DetectOps = [RegOp.CheckDword(MsDtcSec, "LuTransactions", 0)],
            },
        ];
    }

    // ── RestartManagerPolicy ──
    private static class _RestartManagerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RestartManager";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "rstmgr-disable-restart-manager",
                Label = "Disable Restart Manager",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Restart Manager coordinates with applications during software installation to minimize system restarts by restarting only affected processes. Disabling Restart Manager forces software installers to require full system reboots rather than selective process restarts. This policy is appropriate for environments where installers cannot be trusted to coordinate correctly through the Restart Manager API. Some security tools prefer full reboots to partial process restarts to ensure complete state refresh after security updates. Installers in enterprise environments are typically tested and certified to work correctly with or without Restart Manager. Administrators should evaluate installer behavior before broadly disabling this feature across a fleet.",
                Tags = ["restart-manager", "installation", "reboot", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRestartManager", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRestartManager")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRestartManager", 1)],
            },
            new TweakDef
            {
                Id = "rstmgr-disable-app-relaunch",
                Label = "Disable Application Relaunch After Restart",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Restart Manager can relaunch applications that were running before a system restart, attempting to restore the pre-restart state. Disabling app relaunch prevents applications from being automatically started by Restart Manager after the system returns from a reboot. Enterprise environments with managed startup sequences controlled through logon scripts and Group Policy prefer deterministic application startup. Unexpected application relaunch can interfere with security tools, time-sensitive workflows, and session initialization scripts. This policy ensures that the post-restart application state is controlled exclusively by the configured startup infrastructure. Users can still manually restart their applications after reboot without any functional limitation.",
                Tags = ["restart-manager", "relaunch", "startup", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAppRelaunch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAppRelaunch")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAppRelaunch", 1)],
            },
            new TweakDef
            {
                Id = "rstmgr-allow-mitigations",
                Label = "Allow Restart Manager Mitigations",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Restart Manager mitigations provide fallback behaviors when applications cannot be restarted gracefully during software installation. Keeping mitigations enabled at their default safe state ensures Restart Manager can handle edge cases in application shutdown sequences. This policy sets DisableMitigations to zero, preserving the protective fallback behaviors built into the Restart Manager framework. Applications that do not respond to shutdown requests during installation benefit from these mitigation strategies. Disabling mitigations can cause installer hangs when applications refuse to terminate during the shutdown phase. The default mitigation behavior represents the most resilient configuration for diverse application environments.",
                Tags = ["restart-manager", "mitigations", "installation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMitigations", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMitigations")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMitigations", 0)],
            },
            new TweakDef
            {
                Id = "rstmgr-disable-telemetry",
                Label = "Disable Restart Manager Telemetry",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Restart Manager telemetry collects data about shutdown sequences, application restart success rates, and installation coordination outcomes. This information is transmitted to Microsoft to improve installer compatibility and Restart Manager behavior in future Windows versions. Disabling this telemetry prevents installation process metadata from leaving the enterprise environment. Organizations with data egress monitoring policies benefit from eliminating telemetry streams from system components. Restart Manager continues to function identically regardless of whether telemetry is enabled or disabled. Administrative audit requirements can be met through event log monitoring rather than external telemetry.",
                Tags = ["restart-manager", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRestartManagerTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRestartManagerTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRestartManagerTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "rstmgr-disable-session-registration",
                Label = "Disable Restart Manager Session Registration",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Restart Manager session registration allows applications and services to register themselves for coordination during installation shutdowns. Disabling session registration prevents applications from enrolling in the Restart Manager coordination protocol. This is appropriate for environments where all software is deployed through enterprise deployment tools that manage their own process termination. Applications running in containerized or isolated environments do not benefit from cross-process Restart Manager coordination. Disabling registration simplifies the installation pipeline by removing dependencies on the Restart Manager inter-process communication channel. Standard application functionality is not affected since session registration is only relevant during software installation.",
                Tags = ["restart-manager", "registration", "installation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSessionRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSessionRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSessionRegistration", 1)],
            },
            new TweakDef
            {
                Id = "rstmgr-set-shutdown-timeout",
                Label = "Set Service Shutdown Timeout 30 Seconds",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The shutdown timeout determines how long Restart Manager waits for services to respond to shutdown requests during software installation. Configuring a 30-second timeout ensures that the installation process does not wait indefinitely for unresponsive services. Services that fail to shut down within 30 seconds are forcibly terminated, allowing the installation to proceed without hanging. This timeout value balances giving legitimate services adequate time to shut down gracefully against preventing indefinite installation stalls. Environments with large enterprise services that require extended shutdown sequences may need to increase this timeout. The value should be coordinated with observed shut-down times for the largest services running on the target systems.",
                Tags = ["restart-manager", "timeout", "installation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ShutdownTimeout", 30)],
                RemoveOps = [RegOp.DeleteValue(Key, "ShutdownTimeout")],
                DetectOps = [RegOp.CheckDword(Key, "ShutdownTimeout", 30)],
            },
            new TweakDef
            {
                Id = "rstmgr-disable-reboot-notification",
                Label = "Disable Reboot Notification",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Restart Manager generates user-facing notifications when a pending reboot is required after software installation. Disabling reboot notifications suppresses these prompts, delegating reboot scheduling to enterprise patch management tools. Environments using SCCM, Intune, or similar platforms manage reboot windows through maintenance windows and compliance policies. Unsuppressed reboot notifications can prompt users to restart at inopportune times, interrupting active work sessions. Enterprise maintenance window policies ensure reboots occur during off-hours without disrupting productivity. Disabling these notifications requires that the enterprise patch management platform reliably handles all reboot coordination.",
                Tags = ["restart-manager", "notifications", "reboot", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRebootNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRebootNotification")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRebootNotification", 1)],
            },
            new TweakDef
            {
                Id = "rstmgr-disable-service-restart",
                Label = "Disable Service Restart by Restart Manager",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Restart Manager can automatically restart Windows services after shutting them down to facilitate software installation without a full system reboot. Disabling service restart by Restart Manager prevents services from being bounced automatically during installation sequences. Enterprise services such as security agents, database services, and monitoring agents may have complex restart dependencies that Restart Manager cannot honor. Forcibly restarting services can cause data loss or transaction failures if the service has in-flight operations at shutdown time. Organizations with strict service availability requirements prefer to control all service restarts through documented runbook procedures. This setting is appropriate for environments where unplanned service restarts pose operational or data integrity risks.",
                Tags = ["restart-manager", "services", "installation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableServiceRestart", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableServiceRestart")],
                DetectOps = [RegOp.CheckDword(Key, "DisableServiceRestart", 1)],
            },
            new TweakDef
            {
                Id = "rstmgr-zero-max-service-shutdown-wait",
                Label = "Set Max Service Shutdown Wait to Zero",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "The maximum service shutdown wait time specifies how long Restart Manager waits for each individual service to complete its shutdown before force-terminating it. Setting this to zero removes a policy-defined maximum wait, reverting to operating system default timeout behavior for service shutdown. This prevents a policy that forces overly aggressive service termination from conflicting with services requiring longer graceful shutdown sequences. Services with extensive state serialization or active network connections may require several seconds to shut down cleanly without data corruption. Removing the policy-imposed maximum ensures the default operating system timeout governs service shutdown behavior. This setting is appropriate when the default Windows service control manager timeout is preferred over a policy override.",
                Tags = ["restart-manager", "services", "timeout", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxServiceShutdownWaitSeconds", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxServiceShutdownWaitSeconds")],
                DetectOps = [RegOp.CheckDword(Key, "MaxServiceShutdownWaitSeconds", 0)],
            },
            new TweakDef
            {
                Id = "rstmgr-allow-graceful-shutdown",
                Label = "Allow Graceful Shutdown Behavior",
                Category = "System — Licensing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Graceful shutdown behavior in Restart Manager allows services and applications to receive proper shutdown notifications and complete in-progress operations before termination. Keeping this behavior enabled at its default value ensures applications can safely persist state and close open file handles during installation sequences. This policy sets DisableGracefulShutdown to zero, preserving the safe default of allowing orderly application and service termination. Ungraceful termination can leave files in inconsistent states, corrupt databases, and cause post-installation errors that require manual cleanup. Graceful shutdown is particularly important for applications maintaining write transactions at the time of shutdown. This critical safety mechanism should remain enabled in all enterprise configurations.",
                Tags = ["restart-manager", "shutdown", "stability", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGracefulShutdown", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGracefulShutdown")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGracefulShutdown", 0)],
            },
        ];
    }
}
