namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from PolicyMisc.cs ──
// RegiLattice.Core — Tweaks/PolicyMisc.cs
// Remaining system policies: crash dumps, licensing, DotNet framework, active setup, peripheral policies, and unclassified registry controls
// Category: "System & Misc Policy"
// Consolidated from 29 modules.

internal static class PolicyMisc
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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

    // ── SystemRecoveryOptionsPolicy ──
    private static class _SystemRecoveryOptionsPolicy
    {
        private const string RecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SystemRecovery";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sysrecpol-disable-startup-repair",
                    Label = "Disable Automatic Startup Repair",
                    Category = "System",
                    Description =
                        "Sets DisableStartupRepair=1 to prevent Windows from automatically launching Startup Repair when repeated boot failures are detected. Useful for controlled boot environments.",
                    Tags = ["recovery", "startup-repair", "boot", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 2,
                    ImpactNote = "Automatic Startup Repair suppressed; boot failures require manual intervention to diagnose.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableStartupRepair", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableStartupRepair")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableStartupRepair", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-block-recovery-options-access",
                    Label = "Block Access to Recovery Options Menu",
                    Category = "System",
                    Description =
                        "Sets AllowAccessToRecoveryOptions=0 to prevent users from accessing the Windows Recovery Options menu (F8/Shift+F8 at boot). Enhances security by restricting boot-time intervention.",
                    Tags = ["recovery", "options", "boot", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 2,
                    ImpactNote = "Recovery options menu inaccessible; emergency access locked to administrator-controlled methods.",
                    ApplyOps = [RegOp.SetDword(RecKey, "AllowAccessToRecoveryOptions", 0)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "AllowAccessToRecoveryOptions")],
                    DetectOps = [RegOp.CheckDword(RecKey, "AllowAccessToRecoveryOptions", 0)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-disable-sr-from-recovery",
                    Label = "Disable System Restore from Recovery Environment",
                    Category = "System",
                    Description =
                        "Sets DisableSystemRestoreFromRecovery=1 to remove System Restore as an option within the Windows Recovery Environment (WinRE), preventing rollback during recovery sessions.",
                    Tags = ["recovery", "system-restore", "winre", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Users cannot use System Restore from WinRE; reduces risk of unauthorized config rollbacks.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableSystemRestoreFromRecovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableSystemRestoreFromRecovery")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableSystemRestoreFromRecovery", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-block-reset-this-pc",
                    Label = "Block Reset This PC Option",
                    Category = "System",
                    Description =
                        "Sets DisableResetPC=1 to remove the Reset This PC option from the recovery environment and Settings > Recovery. Prevents full system resets that could wipe enterprise configurations.",
                    Tags = ["recovery", "reset", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Reset This PC removed from Settings and WinRE; prevents unauthorized system wipes.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableResetPC", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableResetPC")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableResetPC", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-block-cmd-in-recovery",
                    Label = "Block Command Prompt in Recovery Environment",
                    Category = "System",
                    Description =
                        "Sets DisableCmdInRecovery=1 to remove the Command Prompt option from WinRE Advanced Options. Prevents low-level shell access that could be used to bypass OS security controls.",
                    Tags = ["recovery", "command-prompt", "winre", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WinRE Command Prompt disabled; prevents recovery-time bypass of Windows security features.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableCmdInRecovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableCmdInRecovery")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableCmdInRecovery", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-disable-recovery-ui",
                    Label = "Disable Recovery Environment User Interface",
                    Category = "System",
                    Description =
                        "Sets DisableRecoveryUI=1 to suppress the Windows Recovery Environment graphical interface. Recovery actions are restricted to command-line tools or domain-administered methods.",
                    Tags = ["recovery", "ui", "winre", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "WinRE graphical UI disabled; reduces attack surface during unattended or kiosk boot scenarios.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableRecoveryUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableRecoveryUI")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableRecoveryUI", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-block-advanced-recovery-tools",
                    Label = "Block Advanced Recovery Tools",
                    Category = "System",
                    Description =
                        "Sets BlockAdvancedTools=1 to hide Advanced Recovery Tools such as System Image Recovery, Startup Settings, and UEFI Firmware Settings from the WinRE options menu.",
                    Tags = ["recovery", "advanced-tools", "winre", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Advanced WinRE tools hidden; prevents unauthorized UEFI/firmware modifications from recovery.",
                    ApplyOps = [RegOp.SetDword(RecKey, "BlockAdvancedTools", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "BlockAdvancedTools")],
                    DetectOps = [RegOp.CheckDword(RecKey, "BlockAdvancedTools", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-disable-auto-recovery-boot",
                    Label = "Disable Automatic Recovery Boot Sequence",
                    Category = "System",
                    Description =
                        "Sets DisableAutoRecoveryBoot=1 to prevent Windows from automatically booting into the recovery environment after consecutive failed normal boots. Boots to error screen instead.",
                    Tags = ["recovery", "boot", "automatic", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 2,
                    ImpactNote = "Auto-recovery boot disabled; persistent boot failures require manual diagnostics access.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableAutoRecoveryBoot", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableAutoRecoveryBoot")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableAutoRecoveryBoot", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-hide-recovery-console",
                    Label = "Hide Recovery Console Menu Entry",
                    Category = "System",
                    Description =
                        "Sets HideRecoveryConsole=1 to remove the Recovery Console entry from the boot manager and WinRE menus. Prevents direct console access that bypasses normal Windows login.",
                    Tags = ["recovery", "console", "boot", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Recovery console option hidden from boot menu; reduces physical-access attack surface.",
                    ApplyOps = [RegOp.SetDword(RecKey, "HideRecoveryConsole", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "HideRecoveryConsole")],
                    DetectOps = [RegOp.CheckDword(RecKey, "HideRecoveryConsole", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-disable-memory-diagnostics",
                    Label = "Disable Memory Diagnostics in Recovery",
                    Category = "System",
                    Description =
                        "Sets DisableMemoryDiagnostics=1 to hide the Windows Memory Diagnostic option in WinRE. Prevents access to diagnostics tools that could be misused in shared-access environments.",
                    Tags = ["recovery", "memory", "diagnostics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Memory Diagnostics option hidden in WinRE; standard memory testing still available to admins.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableMemoryDiagnostics", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableMemoryDiagnostics")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableMemoryDiagnostics", 1)],
                },
            ];
    }

    // ── SystemRestoreGpoPolicy ──
    private static class _SystemRestoreGpoPolicy
    {
        private const string SrPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore";

        private const string SrSettings = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "srgpo-set-rp-session-interval",
                Label = "System Restore: Set restore-point creation interval to 1 day",
                Category = "System",
                Description =
                    "Sets RPSessionInterval=1 in SystemRestore settings. Limits automatic restore-point "
                    + "creation frequency to once per day rather than every session start, saving disk space.",
                Tags = ["system-restore", "interval", "schedule", "optimization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "RPSessionInterval", 1)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "RPSessionInterval")],
                DetectOps = [RegOp.CheckDword(SrSettings, "RPSessionInterval", 1)],
            },
            new TweakDef
            {
                Id = "srgpo-set-rp-global-interval",
                Label = "System Restore: Set global restore-point creation interval (24 hr)",
                Category = "System",
                Description =
                    "Sets RPGlobalInterval=1440 (minutes = 24 hours). Controls how often System Restore "
                    + "creates scheduled restore points, capping frequency to once per 24-hour period.",
                Tags = ["system-restore", "interval", "global", "schedule"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "RPGlobalInterval", 1440)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "RPGlobalInterval")],
                DetectOps = [RegOp.CheckDword(SrSettings, "RPGlobalInterval", 1440)],
            },
            new TweakDef
            {
                Id = "srgpo-disable-system-checkpoint",
                Label = "System Restore: Disable automatic system checkpoints",
                Category = "System",
                Description =
                    "Sets CreateSystemCheckPoints=0 in SystemRestore settings. Prevents Windows from "
                    + "automatically creating restore points during system events such as updates.",
                Tags = ["system-restore", "checkpoint", "automatic", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "CreateSystemCheckPoints", 0)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "CreateSystemCheckPoints")],
                DetectOps = [RegOp.CheckDword(SrSettings, "CreateSystemCheckPoints", 0)],
            },
            new TweakDef
            {
                Id = "srgpo-disable-scan-checkpoint",
                Label = "System Restore: Disable restore point creation before scan/cleanup",
                Category = "System",
                Description =
                    "Sets ScanInterval=0 in SystemRestore settings. Stops Windows Security (and legacy "
                    + "Defender) from automatically creating a restore point before each full scan.",
                Tags = ["system-restore", "scan", "checkpoint", "defender"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "ScanInterval", 0)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "ScanInterval")],
                DetectOps = [RegOp.CheckDword(SrSettings, "ScanInterval", 0)],
            },
            new TweakDef
            {
                Id = "srgpo-disable-optimistic-restore",
                Label = "System Restore: Disable optimistic restore support",
                Category = "System",
                Description =
                    "Sets OptimisticRestore=0 in SystemRestore settings. Disables the optimistic-restore "
                    + "code path that tries to recover the system without a full restore after certain failures.",
                Tags = ["system-restore", "recovery", "optimization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "OptimisticRestore", 0)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "OptimisticRestore")],
                DetectOps = [RegOp.CheckDword(SrSettings, "OptimisticRestore", 0)],
            },
            new TweakDef
            {
                Id = "srgpo-disable-batch-restore-points",
                Label = "System Restore: Disable batch software-install restore point creation",
                Category = "System",
                Description =
                    "Sets RestorePointCreationFrequency=0 in SystemRestore settings. Prevents batching of "
                    + "multiple restore-point creation requests within a single install session.",
                Tags = ["system-restore", "batch", "software-install", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "RestorePointCreationFrequency", 0)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "RestorePointCreationFrequency")],
                DetectOps = [RegOp.CheckDword(SrSettings, "RestorePointCreationFrequency", 0)],
            },
            new TweakDef
            {
                Id = "srgpo-disable-incremental-rps",
                Label = "System Restore: Disable incremental restore point diff storage",
                Category = "System",
                Description =
                    "Sets PreventIncrementalRestorations=1 in SystemRestore settings. Forces each restore "
                    + "point to be a full snapshot rather than an incremental delta, ensuring clean rollback.",
                Tags = ["system-restore", "incremental", "snapshot", "storage"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "PreventIncrementalRestorations", 1)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "PreventIncrementalRestorations")],
                DetectOps = [RegOp.CheckDword(SrSettings, "PreventIncrementalRestorations", 1)],
            },
        ];
    }

    // ── TimeSyncAdvPolicy ──
    private static class _TimeSyncAdvPolicy
    {
        private const string ParamKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Parameters";
        private const string CfgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Config";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "tsap-disable-nosync",
                    Label = "Prevent W32tm NoSync Mode",
                    Category = "System",
                    Description =
                        "Sets Type='NTP' (not 'NoSync') and effectively prevents the 'NoSync' policy from leaving the system unsynchronised. Ensures the Windows Time service always uses a time source rather than relying solely on the hardware clock.",
                    Tags = ["time sync", "nosync", "policy", "w32tm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Forces sync mode; system will always attempt time synchronisation.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "AnnounceFlags", 5)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "AnnounceFlags")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "AnnounceFlags", 5)],
                },
                new TweakDef
                {
                    Id = "tsap-set-polling-interval",
                    Label = "Set NTP Polling Interval (Every Hour)",
                    Category = "System",
                    Description =
                        "Sets MaxPollInterval=10 (2^10 = 1024 s ≈ 17 min) and MinPollInterval=6 (2^6 = 64 s) to keep the Windows Time service polling NTP servers more frequently. Default max: 15 (≈9 hours). Improves time accuracy.",
                    Tags = ["time sync", "polling", "interval", "ntp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "More frequent NTP polls; slight increase in outbound UDP 123 traffic.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "MaxPollInterval", 10)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxPollInterval")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "MaxPollInterval", 10)],
                },
                new TweakDef
                {
                    Id = "tsap-set-min-poll-interval",
                    Label = "Set NTP Minimum Polling Interval (64 s)",
                    Category = "System",
                    Description =
                        "Sets MinPollInterval=6 (2^6 = 64 seconds) as the minimum poll interval for the Windows Time service. Default: 10 (1024 s). Lowering this keeps clocks tighter on mobile devices that experience frequent network changes.",
                    Tags = ["time sync", "polling", "min interval", "ntp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Minimum 64-second poll interval; more responsive clock on unstable networks.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "MinPollInterval", 6)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "MinPollInterval")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "MinPollInterval", 6)],
                },
                new TweakDef
                {
                    Id = "tsap-enable-hyperv-timesync",
                    Label = "Enable Hyper-V Time Sync Provider",
                    Category = "System",
                    Description =
                        "Sets HyperVEnabled=1 in W32time Config to enable the Hyper-V time synchronisation provider when running inside a Hyper-V virtual machine. Improves clock accuracy for VMs that experience clock drift on pause/resume.",
                    Tags = ["time sync", "hyper-v", "vm", "virtual machine", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "VM clock synced from Hyper-V host; substantially reduces drift after VM pause/resume cycles.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "HyperVEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "HyperVEnabled")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "HyperVEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "tsap-set-large-phase-spike-threshold",
                    Label = "Increase Phase Spike Threshold",
                    Category = "System",
                    Description =
                        "Sets PhaseCorrectRate=7 and SpikeWatchPeriod=90 (seconds) via Config to widen the time-spike detection window. Reduces the number of legitimate time corrections that are incorrectly classified as spikes and discarded.",
                    Tags = ["time sync", "spike", "phase", "ntp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Fewer legitimate time corrections discarded as spikes on high-latency networks.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "SpikeWatchPeriod", 90)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "SpikeWatchPeriod")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "SpikeWatchPeriod", 90)],
                },
                new TweakDef
                {
                    Id = "tsap-set-event-log-flags",
                    Label = "Increase W32tm Event Log Verbosity",
                    Category = "System",
                    Description =
                        "Sets EventLogFlags=3 to enable both time-jump and time-source-change events in the W32tm event log. Default: 2 (time-jump only). Useful for auditing clock synchronisation events on sensitive systems.",
                    Tags = ["time sync", "event log", "audit", "w32tm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Both time-jump and source-change events logged; slight increase in event log volume.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "EventLogFlags", 3)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "EventLogFlags")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "EventLogFlags", 3)],
                },
            ];
    }

    // ── TimeServicePolicy ──
    private static class _TimeServicePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Parameters";
        private const string PrvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Providers\NtpClient";
        private const string CfgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Config";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "timepol-require-secure-time-provider",
                    Label = "Require Authenticated NTP Time Source for W32Time",
                    Category = "System",
                    Description =
                        "Configures Windows Time Service to use only authenticated NTP time sources (symmetric key mode or MS-SNTP), preventing time set via unauthenticated NTP which could be used to replay expired Kerberos tickets or HSTS bypass.",
                    Tags = ["w32time", "ntp", "authenticated", "kerberos", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Authenticated NTP required; unauthenticated time sources rejected. Prevents Kerberos ticket replay via time skew.",
                    ApplyOps = [RegOp.SetString(Key, "Type", "NT5DS")],
                    RemoveOps = [RegOp.DeleteValue(Key, "Type")],
                    DetectOps = [RegOp.CheckString(Key, "Type", "NT5DS")],
                },
                new TweakDef
                {
                    Id = "timepol-set-ntp-server-domain",
                    Label = "Set NTP Server to Domain Hierarchy (Domain Synchronisation)",
                    Category = "System",
                    Description =
                        "Configures Windows Time Service to synchronise time from the Active Directory domain hierarchy (PDC emulator chain), ensuring all domain-joined machines use a consistent, domain-controlled time source.",
                    Tags = ["w32time", "ntp", "domain", "active-directory", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Time synchronisation set to domain hierarchy; all machines use PDC emulator chain as time source.",
                    ApplyOps = [RegOp.SetString(PrvKey, "NtpServer", "")],
                    RemoveOps = [RegOp.DeleteValue(PrvKey, "NtpServer")],
                    DetectOps = [RegOp.CheckString(PrvKey, "NtpServer", "")],
                },
                new TweakDef
                {
                    Id = "timepol-log-time-jumps",
                    Label = "Log Large Time Synchronisation Jumps in System Log",
                    Category = "System",
                    Description =
                        "Enables System event log entries (EventID 35 — W32TM) when the clock is adjusted by more than 2 minutes due to a time synchronisation event, providing visibility into significant time changes for security auditing.",
                    Tags = ["w32time", "event-log", "audit", "time-jump", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Large time jump events logged in System log; significant NTP-driven clock changes visible for auditing.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "LogJumpEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "LogJumpEvents")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "LogJumpEvents", 1)],
                },
                new TweakDef
                {
                    Id = "timepol-set-poll-interval",
                    Label = "Set NTP Poll Interval to 3600 Seconds for Time Accuracy",
                    Category = "System",
                    Description =
                        "Sets the Windows Time Service NTP client poll interval to 3600 seconds (1 hour), balancing clock accuracy with network traffic, replacing the default variable 17-bit interval that can allow clocks to drift for many hours.",
                    Tags = ["w32time", "ntp", "poll-interval", "clock-accuracy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "NTP poll interval fixed at 1 hour; clock drift limited to less than 1 hour between synchronisations.",
                    ApplyOps = [RegOp.SetDword(PrvKey, "SpecialPollInterval", 3600)],
                    RemoveOps = [RegOp.DeleteValue(PrvKey, "SpecialPollInterval")],
                    DetectOps = [RegOp.CheckDword(PrvKey, "SpecialPollInterval", 3600)],
                },
                new TweakDef
                {
                    Id = "timepol-disable-w32time-telemetry",
                    Label = "Disable Windows Time Service Telemetry to Microsoft",
                    Category = "System",
                    Description =
                        "Prevents the Windows Time Service from sending time synchronisation success/failure rates, configured time source, and clock offset telemetry to Microsoft.",
                    Tags = ["w32time", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "W32TM telemetry to Microsoft disabled; time sync stats and configured source not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "timepol-block-time-provider-change",
                    Label = "Block Standard Users from Changing Time Synchronisation Provider",
                    Category = "System",
                    Description =
                        "Prevents standard users from changing the Windows Time Service provider configuration, ensuring time source and authentication settings can only be changed by administrators.",
                    Tags = ["w32time", "provider", "standard-user", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Time provider change blocked for standard users; NTP source and auth settings admin-only.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "BlockUserTimeProviderChange", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "BlockUserTimeProviderChange")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "BlockUserTimeProviderChange", 1)],
                },
                new TweakDef
                {
                    Id = "timepol-enable-hyperv-time-correction",
                    Label = "Enable Hyper-V Time Synchronisation Guest Correction",
                    Category = "System",
                    Description =
                        "Ensures that VMs running in Hyper-V synchronise their clocks from the Hyper-V host's time source rather than from an NTP server, preventing VM clock drift from causing Kerberos authentication failures in guest environments.",
                    Tags = ["w32time", "hyper-v", "time-sync", "vm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hyper-V VM time synchronisation from host enabled; VM clock maintained via VMBus, not NTP.",
                    ApplyOps = [RegOp.SetDword(PrvKey, "Enabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrvKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(PrvKey, "Enabled", 1)],
                },
                new TweakDef
                {
                    Id = "timepol-harden-stratum-1-sources",
                    Label = "Restrict Windows Time to Stratum-1 or Stratum-2 Sources Only",
                    Category = "System",
                    Description =
                        "Configures Windows Time Service to reject time sources below Stratum 2 quality, preventing synchronisation with inaccurate or potentially manipulated Stratum-8 or worse NTP sources.",
                    Tags = ["w32time", "ntp", "stratum", "accuracy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NTP sources limited to Stratum 1-2; inaccurate high-stratum sources rejected for time synchronisation.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "MaxAllowedPhaseOffset", 300)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxAllowedPhaseOffset")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "MaxAllowedPhaseOffset", 300)],
                },
            ];
    }

    // ── WindowsAnytimeUpgradePolicy ──
    private static class _WindowsAnytimeUpgradePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAnytimeUpgrade";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wanyu-disable-anytime-upgrade",
                    Label = "Disable Windows Anytime Upgrade",
                    Category = "System",
                    Description =
                        "Prevents users from launching Windows Anytime Upgrade to purchase and install a higher-edition license key. On managed corporate devices the OS edition is centrally managed; users should not be able to self-upgrade. Default: Anytime Upgrade accessible. Recommended: 1.",
                    Tags = ["anytime-upgrade", "edition", "upgrade", "store", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Windows Anytime Upgrade entry point is removed from the system; users cannot initiate an edition upgrade.",
                    ApplyOps = [RegOp.SetDword(Key, "Disabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Disabled")],
                    DetectOps = [RegOp.CheckDword(Key, "Disabled", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-disable-upgrade-via-store",
                    Label = "Disable OS Upgrade via Microsoft Store",
                    Category = "System",
                    Description =
                        "Prevents users from upgrading the operating system edition (e.g., Home → Pro, or Pro → Enterprise) via the Microsoft Store upgrade pathways. Keeps OS edition under IT control on managed devices. Default: Store-based edition upgrade permitted. Recommended: 1.",
                    Tags = ["anytime-upgrade", "edition", "store", "upgrade", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Microsoft Store OS edition upgrade path is blocked; edition remains as deployed by IT.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStoreUpgrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreUpgrade")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStoreUpgrade", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-block-key-entry-ui",
                    Label = "Block Product Key Entry for Edition Upgrade",
                    Category = "System",
                    Description =
                        "Removes the 'Change product key' button from Settings → Update & Security → Activation that would allow a user to enter a higher-edition key and trigger an in-place upgrade. Prevents unauthorized edition changes by typing a key. Default: key entry available. Recommended: 1.",
                    Tags = ["anytime-upgrade", "product-key", "activation", "edition", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Product key entry for in-place edition upgrade is removed from the Activation Settings page.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockKeyEntry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockKeyEntry")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockKeyEntry", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-log-upgrade-attempts",
                    Label = "Log Windows Anytime Upgrade Attempts",
                    Category = "System",
                    Description =
                        "Records an Application event log entry whenever a user attempts to initiate a Windows Anytime Upgrade, whether blocked by policy or not. Useful for detecting users who are trying to bypass edition controls. Default: attempts not logged. Recommended: 1 on monitored endpoints.",
                    Tags = ["anytime-upgrade", "audit", "logging", "edition", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Any attempt to start an edition upgrade is logged to the Application event log.",
                    ApplyOps = [RegOp.SetDword(Key, "LogUpgradeAttempts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogUpgradeAttempts")],
                    DetectOps = [RegOp.CheckDword(Key, "LogUpgradeAttempts", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-disable-upgrade-notification",
                    Label = "Suppress Windows Anytime Upgrade Notifications",
                    Category = "System",
                    Description =
                        "Suppresses promotional notifications and prompts that encourage users to purchase a higher Windows edition (e.g., 'Upgrade to Pro for these features'). Removes upsell nags from the UI without affecting the installed edition. Default: notifications displayed. Recommended: 1.",
                    Tags = ["anytime-upgrade", "notification", "ui", "edition", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Edition upgrade promotional notifications and upsell banners are suppressed.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUpgradeNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUpgradeNotifications")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUpgradeNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-prevent-downgrade",
                    Label = "Prevent Windows Edition Downgrade via Policy",
                    Category = "System",
                    Description =
                        "Prevents edition downgrades (e.g., Enterprise → Pro rollback) via key entry or the Activation Store. Protects against licence audit circumvention where a device could be temporarily downgraded. Default: downgrade via key entry possible. Recommended: 1.",
                    Tags = ["anytime-upgrade", "downgrade", "edition", "activation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Edition downgrade through Activation Settings is blocked; OS remains on the IT-deployed edition.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventEditionDowngrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventEditionDowngrade")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventEditionDowngrade", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-hide-activation-settings",
                    Label = "Hide Activation Settings Page",
                    Category = "System",
                    Description =
                        "Removes the Activation page from Windows Settings so users cannot view the activation status or attempt to change the product key. Useful on volume-licensed endpoints where individual activation management is not required and should not be user-accessible. Default: Activation page visible. Recommended: 1 on volume-licensed images.",
                    Tags = ["anytime-upgrade", "activation", "settings", "ui", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Settings → Activation page is hidden; users cannot view licensing state or change the product key.",
                    ApplyOps = [RegOp.SetDword(Key, "HideActivationPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "HideActivationPage")],
                    DetectOps = [RegOp.CheckDword(Key, "HideActivationPage", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-disable-phone-activation",
                    Label = "Disable Phone Activation Method",
                    Category = "System",
                    Description =
                        "Blocks the automated phone activation pathway that allows a user to activate a new edition by calling a Microsoft number and entering a confirmation code. Prevents out-of-band edition changes that bypasses online controls. Default: phone activation available. Recommended: 1.",
                    Tags = ["anytime-upgrade", "phone-activation", "edition", "activation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Phone activation path for edition upgrades is disabled; only online IT-managed activation is available.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePhoneActivation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneActivation")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePhoneActivation", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-lock-edition-to-deployed",
                    Label = "Lock OS Edition to IT-Deployed Edition",
                    Category = "System",
                    Description =
                        "Configures a policy lock that prevents the OS edition from changing in either direction (upgrade or downgrade) without explicit Group Policy update. Provides a strong enforcement control on managed devices where edition stability is a compliance requirement. Default: not locked. Recommended: 1 on standardised fleet deployments.",
                    Tags = ["anytime-upgrade", "edition", "lock", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "OS edition is locked to the IT-deployed value; neither upgrade nor downgrade is possible without GPO change.",
                    ApplyOps = [RegOp.SetDword(Key, "LockEdition", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LockEdition")],
                    DetectOps = [RegOp.CheckDword(Key, "LockEdition", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-disable-trial-edition-conversion",
                    Label = "Disable Trial Edition Conversion",
                    Category = "System",
                    Description =
                        "Prevents the OS from being converted from a trial (evaluation) edition to a retail edition via key entry. Ensures evaluation images are not accidentally or deliberately activated as production machines without proper licensing procedures. Default: trial conversion available. Recommended: 1 on production fleet.",
                    Tags = ["anytime-upgrade", "trial", "conversion", "edition", "activation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Trial-to-retail edition conversion is blocked; evaluation images cannot be activated without proper IT process.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTrialConversion", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTrialConversion")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTrialConversion", 1)],
                },
            ];
    }

    // ── WindowsBackupPolicy ──
    private static class _WindowsBackupPolicy
    {
        private const string BackupKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup";
        private const string ClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "backup-disable-backup",
                    Label = "Disable Windows Backup",
                    Category = "System",
                    Description = "Disables the Windows Backup feature and prevents users from initiating backups through the control panel.",
                    Tags = ["backup", "windows-backup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Windows Backup is disabled; use third-party or enterprise backup solutions instead.",
                    ApplyOps = [RegOp.SetDword(BackupKey, "DisableBackup", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupKey, "DisableBackup")],
                    DetectOps = [RegOp.CheckDword(BackupKey, "DisableBackup", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-restore",
                    Label = "Disable Windows Backup Restore",
                    Category = "System",
                    Description = "Prevents users from using the Windows Backup restore feature to recover files or system state.",
                    Tags = ["backup", "restore", "windows-backup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Restore via Windows Backup UI is blocked; enterprise recovery tools still function.",
                    ApplyOps = [RegOp.SetDword(BackupKey, "DisableRestore", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupKey, "DisableRestore")],
                    DetectOps = [RegOp.CheckDword(BackupKey, "DisableRestore", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-catalog-viewer",
                    Label = "Disable Windows Backup Catalog Viewer",
                    Category = "System",
                    Description = "Removes access to the Windows Backup catalog viewer preventing browsing of historical backup sets.",
                    Tags = ["backup", "catalog", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Catalog browser is hidden from users; backup files on disk are unaffected.",
                    ApplyOps = [RegOp.SetDword(BackupKey, "DisableCatalogViewer", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupKey, "DisableCatalogViewer")],
                    DetectOps = [RegOp.CheckDword(BackupKey, "DisableCatalogViewer", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-system-backup",
                    Label = "Disable Windows System Backup",
                    Category = "System",
                    Description = "Prevents users from creating system image or system files backups through the Windows Backup UI.",
                    Tags = ["backup", "system-backup", "image", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "System image creation is blocked; critical for environments using enterprise imaging solutions.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "NoBackupSysFiles", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "NoBackupSysFiles")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "NoBackupSysFiles", 1)],
                },
                new TweakDef
                {
                    Id = "backup-suppress-backup-progress-ui",
                    Label = "Suppress Windows Backup Progress Dialog",
                    Category = "System",
                    Description = "Hides the backup progress window and toast notifications that appear during Windows Backup operations.",
                    Tags = ["backup", "ui", "progress", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Silent backup mode; no visible progress indicator; check event logs to verify backup completion.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "NoProgressUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "NoProgressUI")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "NoProgressUI", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-online-backup",
                    Label = "Disable Online Backup Services Integration",
                    Category = "System",
                    Description = "Removes the online backup provider options from the Windows Backup configuration wizard.",
                    Tags = ["backup", "online", "cloud", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Cloud backup provider options are removed from the UI; local backup to drives still available.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "NoOnlineBackup", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "NoOnlineBackup")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "NoOnlineBackup", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-network-backup",
                    Label = "Disable Backup to Network Locations",
                    Category = "System",
                    Description = "Blocks Windows Backup from saving backup sets to network shares or mapped drives.",
                    Tags = ["backup", "network", "share", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Prevents backup data exfiltration to network shares; local drives only for Windows Backup.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "NoNetworkBackup", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "NoNetworkBackup")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "NoNetworkBackup", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-backup-over-metered",
                    Label = "Disable Windows Backup on Metered Connections",
                    Category = "System",
                    Description = "Prevents Windows Backup from running over metered (pay-per-use) network connections.",
                    Tags = ["backup", "metered", "network", "data-usage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Backup paused on metered connections; resumes automatically on unmetered networks.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "DisableBackupOnMeteredConnections", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "DisableBackupOnMeteredConnections")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "DisableBackupOnMeteredConnections", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-scheduled-backup",
                    Label = "Disable Scheduled Windows Backup",
                    Category = "System",
                    Description = "Prevents Windows from running scheduled background backups automatically on a configured schedule.",
                    Tags = ["backup", "scheduled", "task", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Automatic scheduled backups are disabled; manual backup invocation still works unless DisableBackup is also set.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "NoScheduledBackup", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "NoScheduledBackup")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "NoScheduledBackup", 1)],
                },
                new TweakDef
                {
                    Id = "backup-hide-control-panel-link",
                    Label = "Hide Windows Backup Control Panel Link",
                    Category = "System",
                    Description = "Removes the Windows Backup entry from the Control Panel and System & Security settings page.",
                    Tags = ["backup", "control-panel", "ui", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Backup settings UI is hidden; the underlying feature may still be invoked by command line or scripts.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "HideControlPanelLink", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "HideControlPanelLink")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "HideControlPanelLink", 1)],
                },
            ];
    }

    // ── WindowsConnectNowPolicy ──
    private static class _WindowsConnectNowPolicy
    {
        private const string RegKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\Registrars";
        private const string UiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\UI";
        private const string WcnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wcnpol-disable-execution-service",
                Label = "WCN Policy: Disable WCN Execution Service",
                Category = "System",
                Description =
                    "Prevents the WCN execution service from running through GPO. The WCN service manages network device discovery and configuration — disabling it reduces the attack surface on managed enterprise networks.",
                Tags = ["wcn", "service", "wireless", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents WCN execution service from running.",
                RegistryKeys = [RegKey],
                ApplyOps = [RegOp.SetDword(RegKey, "DisableWcnExecutionService", 1)],
                RemoveOps = [RegOp.DeleteValue(RegKey, "DisableWcnExecutionService")],
                DetectOps = [RegOp.CheckDword(RegKey, "DisableWcnExecutionService", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-flash-config",
                Label = "WCN Policy: Disable Flash Config Provisioning",
                Category = "System",
                Description =
                    "Disables the WCN Flash Config Registrar which allows device setup via USB-connected flash drives. Flash-based provisioning can be exploited to inject unauthorized wireless configurations.",
                Tags = ["wcn", "flash", "usb", "provisioning", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks USB flash drive-based wireless provisioning.",
                RegistryKeys = [RegKey],
                ApplyOps = [RegOp.SetDword(RegKey, "DisableFlashConfigRegistrar", 1)],
                RemoveOps = [RegOp.DeleteValue(RegKey, "DisableFlashConfigRegistrar")],
                DetectOps = [RegOp.CheckDword(RegKey, "DisableFlashConfigRegistrar", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-inband-80211",
                Label = "WCN Policy: Disable In-Band 802.11 Wireless Registrar",
                Category = "System",
                Description =
                    "Disables the WCN in-band 802.11 wireless registrar, which enables over-the-air device configuration. Prevents unauthorized wireless setup requests from being processed by managed devices.",
                Tags = ["wcn", "802.11", "wifi", "wireless", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables over-the-air 802.11 device configuration.",
                RegistryKeys = [RegKey],
                ApplyOps = [RegOp.SetDword(RegKey, "DisableInBand802DOT11Registrar", 1)],
                RemoveOps = [RegOp.DeleteValue(RegKey, "DisableInBand802DOT11Registrar")],
                DetectOps = [RegOp.CheckDword(RegKey, "DisableInBand802DOT11Registrar", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-upnp-registrar",
                Label = "WCN Policy: Disable UPnP-Based WCN Registrar",
                Category = "System",
                Description =
                    "Disables the WCN UPnP registrar. WCN over UPnP can expose wireless credentials and configuration data to other devices on the local network without authentication.",
                Tags = ["wcn", "upnp", "wireless", "network", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables WCN over UPnP; prevents unauthenticated credential exposure.",
                RegistryKeys = [RegKey],
                ApplyOps = [RegOp.SetDword(RegKey, "DisableUPnPRegistrar", 1)],
                RemoveOps = [RegOp.DeleteValue(RegKey, "DisableUPnPRegistrar")],
                DetectOps = [RegOp.CheckDword(RegKey, "DisableUPnPRegistrar", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-ui",
                Label = "WCN Policy: Disable WCN User Interface",
                Category = "System",
                Description =
                    "Hides the Windows Connect Now setup wizard from the Network and Sharing Center UI. Prevents end users from initiating WCN-based wireless device setup sessions on managed endpoints.",
                Tags = ["wcn", "ui", "wireless", "policy", "lockdown"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides WCN setup wizard from Network and Sharing Center.",
                RegistryKeys = [UiKey],
                ApplyOps = [RegOp.SetDword(UiKey, "DisableWcnUi", 1)],
                RemoveOps = [RegOp.DeleteValue(UiKey, "DisableWcnUi")],
                DetectOps = [RegOp.CheckDword(UiKey, "DisableWcnUi", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-auto-add",
                Label = "WCN Policy: Disable Automatic Device Add via WCN",
                Category = "System",
                Description =
                    "Prevents automatic device addition through WCN by disabling the auto-add registrar. Stops devices from self-enrolling into the network through the WCN protocol without admin intervention.",
                Tags = ["wcn", "auto-add", "device", "network", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents devices from self-enrolling into the network through WCN.",
                RegistryKeys = [RegKey],
                ApplyOps = [RegOp.SetDword(RegKey, "AllowAutoAddRegistrar", 0)],
                RemoveOps = [RegOp.DeleteValue(RegKey, "AllowAutoAddRegistrar")],
                DetectOps = [RegOp.CheckDword(RegKey, "AllowAutoAddRegistrar", 0)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-wcn-global",
                Label = "WCN Policy: Globally Disable Windows Connect Now",
                Category = "System",
                Description =
                    "Completely disables Windows Connect Now via the top-level GPO flag. Prevents any WCN-based operations including wireless device setup, UPnP registrar, and in-band 802.11 provisioning.",
                Tags = ["wcn", "disable", "wireless", "global", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Global WCN disable via top-level GPO flag.",
                RegistryKeys = [WcnKey],
                ApplyOps = [RegOp.SetDword(WcnKey, "DisableWCN", 1)],
                RemoveOps = [RegOp.DeleteValue(WcnKey, "DisableWCN")],
                DetectOps = [RegOp.CheckDword(WcnKey, "DisableWCN", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-pin-connect",
                Label = "WCN Policy: Disable PIN-Based WCN Device Connection",
                Category = "System",
                Description =
                    "Blocks PIN-based Windows Connect Now device pairing. WCN PIN-based setup is vulnerable to brute-force PIN enumeration attacks (similar to WPS vulnerabilities on routers).",
                Tags = ["wcn", "pin", "pairing", "wireless", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks PIN-based WCN pairing vulnerable to brute-force enumeration.",
                RegistryKeys = [WcnKey],
                ApplyOps = [RegOp.SetDword(WcnKey, "DisablePINConnect", 1)],
                RemoveOps = [RegOp.DeleteValue(WcnKey, "DisablePINConnect")],
                DetectOps = [RegOp.CheckDword(WcnKey, "DisablePINConnect", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-push-button-connect",
                Label = "WCN Policy: Disable Push Button WCN Connection",
                Category = "System",
                Description =
                    "Disables push-button connection method for Windows Connect Now. Physical push-button pairing can be exploited in unlocked or unattended environments to add unauthorized devices.",
                Tags = ["wcn", "push-button", "wps", "wireless", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables push-button WCN pairing on unattended devices.",
                RegistryKeys = [WcnKey],
                ApplyOps = [RegOp.SetDword(WcnKey, "DisablePushButtonConnect", 1)],
                RemoveOps = [RegOp.DeleteValue(WcnKey, "DisablePushButtonConnect")],
                DetectOps = [RegOp.CheckDword(WcnKey, "DisablePushButtonConnect", 1)],
            },
        ];
    }

    // ── WindowsLogonOptionsPolicy ──
    private static class _WindowsLogonOptionsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Winlogon";
        private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wlogon-disable-last-username-display",
                Label = "Windows Logon Options: Do Not Display Last Signed-In Username",
                Category = "System",
                Description =
                    "Prevents the logon screen from pre-filling or displaying the last signed-in user's username. "
                    + "Displaying the last username reduces the effort required for an attacker with physical access to attempt credential attacks. "
                    + "With this policy set, the username field is blank and the user must type their full UPN or samAccountName. "
                    + "Removing this policy restores pre-filled last-username display on the logon screen.",
                Tags = ["logon", "username", "screen", "privacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DontDisplayLastUserName", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DontDisplayLastUserName")],
                DetectOps = [RegOp.CheckDword(Key, "DontDisplayLastUserName", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clears pre-filled username on logon screen; reduces account enumeration risk.",
            },
            new TweakDef
            {
                Id = "wlogon-disable-last-user-account-logon-info",
                Label = "Windows Logon Options: Do Not Display Last Account Info at Logon",
                Category = "System",
                Description =
                    "Prevents the logon screen from displaying account information from the last successfully logged-on user. "
                    + "This includes not showing the account name, domain, and display picture associated with the previous session. "
                    + "Required by CIS Benchmark Level 1 for interactive logon hardening on domain or workgroup endpoints. "
                    + "Removing this policy restores last account display on the logon screen.",
                Tags = ["logon", "account-info", "cis", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DontDisplayLockedUserId", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "DontDisplayLockedUserId")],
                DetectOps = [RegOp.CheckDword(Key, "DontDisplayLockedUserId", 3)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides account info on locked screen; prevents account enumeration via UI.",
            },
            new TweakDef
            {
                Id = "wlogon-require-ctrl-alt-del",
                Label = "Windows Logon Options: Require Ctrl+Alt+Del Secure Attention Sequence",
                Category = "System",
                Description =
                    "Forces users to press Ctrl+Alt+Del before entering credentials on the logon screen. "
                    + "The Ctrl+Alt+Del Secure Attention Sequence (SAS) is a trusted OS-level signal that cannot be intercepted by malware. "
                    + "Disabling it allows fake logon screens created by trojans to capture credentials without triggering the SAS guard. "
                    + "Removing this policy makes Ctrl+Alt+Del optional (default consumer behavior).",
                Tags = ["logon", "ctrl-alt-del", "secure-attention", "credential", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCAD", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCAD")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCAD", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enforces SAS keystroke before logon; blocks fake credential capture screens.",
            },
            new TweakDef
            {
                Id = "wlogon-disable-password-reveal-button",
                Label = "Windows Logon Options: Disable Password Reveal Button",
                Category = "System",
                Description =
                    "Removes the password reveal (eye icon) button from password fields on the logon screen and credential dialogs. "
                    + "The reveal button is a usability feature but it creates shoulder-surfing risk in shared or open-plan environments. "
                    + "Disabling it prevents bystanders from using the button to glimpse passwords when the user unlocks the screen. "
                    + "Removing this policy restores the password reveal button.",
                Tags = ["logon", "password-reveal", "shoulder-surfing", "credential", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePasswordReveal", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordReveal")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePasswordReveal", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides password reveal button; reduces shoulder-surfing risk on shared workstations.",
            },
            new TweakDef
            {
                Id = "wlogon-set-legal-notice-caption",
                Label = "Windows Logon Options: Set Legal Notice Banner Caption",
                Category = "System",
                Description =
                    "Sets the caption text for the legal notice dialog shown before Windows logon. "
                    + "Displaying a legal notice at logon is a common compliance requirement that informs users the system is monitored and for authorized use only. "
                    + "The caption is the title bar text of the notice dialog (typically 'Authorized Use Only' or similar). "
                    + "Removing this policy clears the legal notice dialog if no text is configured.",
                Tags = ["logon", "legal-notice", "compliance", "banner", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "LegalNoticeCaption", "Authorized Access Only")],
                RemoveOps = [RegOp.DeleteValue(Key, "LegalNoticeCaption")],
                DetectOps = [RegOp.CheckString(Key, "LegalNoticeCaption", "Authorized Access Only")],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Displays legal notice caption on logon; satisfies compliance banner requirements.",
            },
            new TweakDef
            {
                Id = "wlogon-set-legal-notice-text",
                Label = "Windows Logon Options: Set Legal Notice Banner Body Text",
                Category = "System",
                Description =
                    "Sets the body text content of the legal notice dialog shown before Windows logon. "
                    + "Legal notice text should convey that the system is for authorized users only, activity is monitored, and unauthorized access is prohibited. "
                    + "Many compliance frameworks (PCI-DSS, HIPAA, NIST) require this logon warning. "
                    + "Removing this policy clears the notice body text; the dialog no longer appears if both caption and text are absent.",
                Tags = ["logon", "legal-notice", "compliance", "text", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps =
                [
                    RegOp.SetString(
                        Key,
                        "LegalNoticeText",
                        "This system is for authorized users only. All activity is monitored and logged. Unauthorized access is prohibited."
                    ),
                ],
                RemoveOps = [RegOp.DeleteValue(Key, "LegalNoticeText")],
                DetectOps =
                [
                    RegOp.CheckString(
                        Key,
                        "LegalNoticeText",
                        "This system is for authorized users only. All activity is monitored and logged. Unauthorized access is prohibited."
                    ),
                ],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Displays legal warning at logon; required by PCI-DSS, HIPAA, and NIST frameworks.",
            },
            new TweakDef
            {
                Id = "wlogon-disable-unlocking-from-non-domain-context",
                Label = "Windows Logon Options: Require Domain Logon to Unlock Machine",
                Category = "System",
                Description =
                    "Prevents users from unlocking a locked workstation using a local (non-domain) account. "
                    + "When enabled, only domain accounts can unlock the session, preventing an attacker from using a local account to bypass domain authentication. "
                    + "Best practice on domain-joined machines is to ensure the locked screen can only be cleared with domain credentials. "
                    + "Removing this policy allows local account unlocking of locked domain sessions.",
                Tags = ["logon", "unlock", "domain", "local-account", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ForceUnlockLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceUnlockLogon")],
                DetectOps = [RegOp.CheckDword(Key, "ForceUnlockLogon", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Requires domain credentials to unlock; prevents local-account session bypass.",
            },
            new TweakDef
            {
                Id = "wlogon-set-machine-inactivity-limit",
                Label = "Windows Logon Options: Set Machine Inactivity Limit (15 min)",
                Category = "System",
                Description =
                    "Configures a machine-scope inactivity timeout of 15 minutes after which the screen locks automatically. "
                    + "This policy is evaluated at the OS level and overrides user-configured screen saver delays. "
                    + "A 15-minute inactivity limit is the CIS Benchmark L1 recommendation for workstation endpoint hardening. "
                    + "Removing this policy removes the machine-scope inactivity timeout.",
                Tags = ["logon", "inactivity", "lock", "cis", "timeout", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "InactivityTimeoutSecs", 900)],
                RemoveOps = [RegOp.DeleteValue(Key, "InactivityTimeoutSecs")],
                DetectOps = [RegOp.CheckDword(Key, "InactivityTimeoutSecs", 900)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Forces screen lock after 15 min idle; prevents unattended access on shared workstations.",
            },
            new TweakDef
            {
                Id = "wlogon-disable-smart-card-removal-behavior-none",
                Label = "Windows Logon Options: Lock on Smart Card Removal",
                Category = "System",
                Description =
                    "Configures the system to lock the workstation when the smart card is removed from the reader. "
                    + "For environments using smart-card-based authentication (PIV, CAC), removing the card should immediately secure the session. "
                    + "Setting this to lock (value 1) prevents the workstation from remaining unlocked when the physical credential is withdrawn. "
                    + "Removing this policy reverts to the default behavior (no action on card removal).",
                Tags = ["logon", "smart-card", "physical-security", "lock", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ScRemoveOption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ScRemoveOption")],
                DetectOps = [RegOp.CheckDword(Key, "ScRemoveOption", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks workstation on smart card removal; prevents unattended session access.",
            },
        ];
    }

    // ── WindowsMailPolicy ──
    private static class _WindowsMailPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Mail";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "winmail-disable-manual-launch",
                Label = "Windows Mail Policy: Block Manual Launch of Windows Mail",
                Category = "System",
                Description =
                    "Prevents users from manually launching the Windows Mail application. Enterprise environments that route email exclusively through corporate clients (Outlook, web) should block the inbox Windows Mail app to reduce shadow IT risk.",
                Tags = ["mail", "windows-mail", "launch", "block", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ManualLaunchAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ManualLaunchAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "ManualLaunchAllowed", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents use of inbox Windows Mail on managed devices that require corporate mail clients.",
            },
            new TweakDef
            {
                Id = "winmail-disable-mail-import",
                Label = "Windows Mail Policy: Disable Import of External Mail Accounts",
                Category = "System",
                Description =
                    "Prevents Windows Mail from importing accounts, messages, or contacts from external mail clients. Disabling import reduces the risk of unauthorized data ingestion from non-corporate mail clients into the Windows Mail store.",
                Tags = ["mail", "windows-mail", "import", "accounts", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffMailImport", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffMailImport")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffMailImport", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents unauthorised account and message ingestion from non-corporate mail clients.",
            },
            new TweakDef
            {
                Id = "winmail-block-http-tracking-pixels",
                Label = "Windows Mail Policy: Block HTTP Remote Images (Anti-Tracking)",
                Category = "System",
                Description =
                    "Prevents Windows Mail from automatically loading HTTP images embedded in email messages. Remote images (1x1 tracking pixels) are widely used by marketers and threat actors to confirm email addresses are active and track recipient location.",
                Tags = ["mail", "windows-mail", "tracking", "images", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockHTTPImages", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockHTTPImages")],
                DetectOps = [RegOp.CheckDword(Key, "BlockHTTPImages", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks email tracking pixels; prevents confirmation of email activity by marketers and threat actors.",
            },
            new TweakDef
            {
                Id = "winmail-disable-featured-updates",
                Label = "Windows Mail Policy: Disable Featured Updates in Windows Mail",
                Category = "System",
                Description =
                    "Turns off the featured/promotional updates displayed within the Windows Mail application. In enterprise deployments, UI promotional messages are distractions that may redirect users to unsanctioned services.",
                Tags = ["mail", "windows-mail", "updates", "promotional", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffFeaturedUpdates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffFeaturedUpdates")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffFeaturedUpdates", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes promotional update banners from Windows Mail UI on managed devices.",
            },
            new TweakDef
            {
                Id = "winmail-disable-hotmail-contact-sync",
                Label = "Windows Mail Policy: Disable Hotmail/Live Contact Synchronisation",
                Category = "System",
                Description =
                    "Prevents Windows Mail from synchronising contacts with Microsoft Hotmail or Live accounts. On managed devices, contact sync to personal Microsoft accounts creates data exfiltration risk for confidential address book entries.",
                Tags = ["mail", "windows-mail", "hotmail", "contacts", "sync", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffHotmailContact", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffHotmailContact")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffHotmailContact", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents corporate address book data from syncing to personal Microsoft accounts.",
            },
            new TweakDef
            {
                Id = "winmail-force-plaintext-display",
                Label = "Windows Mail Policy: Force Plaintext Rendering for Email",
                Category = "System",
                Description =
                    "Forces Windows Mail to render incoming messages as plain text only. HTML email is the primary delivery vector for phishing attacks (hidden links, CSS tricks, JavaScript payloads). Plain text rendering neutralises the entire class of HTML-based email threats.",
                Tags = ["mail", "windows-mail", "plaintext", "html", "phishing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ForceHTMLMailAsPlainText", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceHTMLMailAsPlainText")],
                DetectOps = [RegOp.CheckDword(Key, "ForceHTMLMailAsPlainText", 1)],
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Neutralises HTML email threat vector including phishing links and JavaScript; breaks rich text formatting.",
            },
            new TweakDef
            {
                Id = "winmail-block-executable-attachments",
                Label = "Windows Mail Policy: Block Executable File Attachments",
                Category = "System",
                Description =
                    "Prevents Windows Mail from delivering or presenting executable file attachments (EXE, COM, BAT, PS1, etc.) to users. Executable email attachments are the most common initial access vector in enterprise phishing campaigns.",
                Tags = ["mail", "windows-mail", "attachments", "executable", "block", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockExecutableAttachments", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockExecutableAttachments")],
                DetectOps = [RegOp.CheckDword(Key, "BlockExecutableAttachments", 1)],
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Blocks executable email attachments — the primary phishing initial-access vector.",
            },
            new TweakDef
            {
                Id = "winmail-disable-shopping-links",
                Label = "Windows Mail Policy: Disable Shopping Promotional Links",
                Category = "System",
                Description =
                    "Disables shopping links and promotional offers embedded in Windows Mail. Enterprise mail clients should suppress commercial UI to prevent employee distraction and reduce the risk of clicking unsolicited purchase links.",
                Tags = ["mail", "windows-mail", "shopping", "promotional", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffShopping", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffShopping")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffShopping", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses commercial promotional links within Windows Mail.",
            },
            new TweakDef
            {
                Id = "winmail-disable-news-feed",
                Label = "Windows Mail Policy: Disable News Feed Integration",
                Category = "System",
                Description =
                    "Disables the integrated news feed widget within Windows Mail. News feed integration increases background network calls and may display content from external third-party news aggregators, which is inappropriate for managed enterprise environments.",
                Tags = ["mail", "windows-mail", "news", "feed", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffNewsFeed", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffNewsFeed")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffNewsFeed", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes third-party news feed integration from Windows Mail.",
            },
            new TweakDef
            {
                Id = "winmail-disable-calendar-integration",
                Label = "Windows Mail Policy: Disable Calendar Sync Integration",
                Category = "System",
                Description =
                    "Prevents Windows Mail from synchronising calendar data with Microsoft consumer accounts or Exchange integrations not managed by the enterprise. Blocks calendar data from being stored in the Windows Mail local store outside MDM supervision.",
                Tags = ["mail", "windows-mail", "calendar", "sync", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCalendarIntegration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCalendarIntegration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCalendarIntegration", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents calendar data from syncing to unmanaged Microsoft Account stores.",
            },
        ];
    }

    // ── WindowsMediaPlayerPolicy ──
    private static class _WindowsMediaPlayerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wmplay-disable-auto-codec-download",
                Label = "Windows Media Player: Disable Automatic Codec Download",
                Category = "System",
                Description =
                    "Prevents Windows Media Player from automatically downloading codecs from the Internet when a media file requires one. "
                    + "Automatic codec download can introduce unsigned or malicious codec software that runs in a privileged context. "
                    + "On managed endpoints codecs should be deployed via the software management tool, not pulled from Internet sources at runtime. "
                    + "Removing this policy re-enables automatic codec download when WMP encounters an unsupported format.",
                Tags = ["media-player", "codec", "download", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventCodecDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventCodecDownload")],
                DetectOps = [RegOp.CheckDword(Key, "PreventCodecDownload", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks runtime codec downloads; prevents unsigned codec execution from Internet sources.",
            },
            new TweakDef
            {
                Id = "wmplay-disable-auto-update-check",
                Label = "Windows Media Player: Disable Automatic Update Checking",
                Category = "System",
                Description =
                    "Prevents Windows Media Player from automatically checking for updates on the Internet. "
                    + "Automatic update checks for WMP can generate unexpected outbound traffic to Microsoft update servers. "
                    + "Updates should be managed through WSUS, SCCM, or Intune rather than individual application self-update mechanisms. "
                    + "Removing this policy re-enables WMP's automatic update check on application launch.",
                Tags = ["media-player", "auto-update", "bandwidth", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventAutoUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "PreventAutoUpdate", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables WMP self-update checks; consolidates media player updates through WSUS.",
            },
            new TweakDef
            {
                Id = "wmplay-disable-internet-streaming",
                Label = "Windows Media Player: Disable Internet Media Streaming",
                Category = "System",
                Description =
                    "Restricts Windows Media Player from streaming media content from Internet URLs. "
                    + "Allowing arbitrary Internet streaming can consume significant bandwidth and may result in access to unlicensed or inappropriate content. "
                    + "On corporate networks, media streaming should be restricted to internal or approved sources only. "
                    + "Removing this policy re-enables Internet-based media streaming in WMP.",
                Tags = ["media-player", "streaming", "internet", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventMediaSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventMediaSharing")],
                DetectOps = [RegOp.CheckDword(Key, "PreventMediaSharing", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents WMP Internet streaming; conserves bandwidth and blocks unapproved content.",
            },
            new TweakDef
            {
                Id = "wmplay-disable-digital-rights-management",
                Label = "Windows Media Player: Disable DRM License Acquisition from Internet",
                Category = "System",
                Description =
                    "Prevents Windows Media Player from automatically acquiring DRM (Digital Rights Management) licenses from the Internet. "
                    + "Automatic DRM license acquisition initiates outbound connections to third-party license servers without explicit user consent. "
                    + "On managed endpoints, DRM license acquisition should be user-confirmed or blocked entirely. "
                    + "Removing this policy re-enables automatic DRM license acquisition when protected media files are opened.",
                Tags = ["media-player", "drm", "licensing", "internet", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventDRMacquisition", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventDRMacquisition")],
                DetectOps = [RegOp.CheckDword(Key, "PreventDRMacquisition", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks DRM auto-acquisition; prevents silent outbound connections to license servers.",
            },
            new TweakDef
            {
                Id = "wmplay-disable-media-information-online",
                Label = "Windows Media Player: Disable Online Media Information Retrieval",
                Category = "System",
                Description =
                    "Prevents WMP from connecting to the Internet to retrieve album artwork, track information, and music metadata. "
                    + "Online metadata requests reveal what media files are being played to Microsoft or third-party data providers. "
                    + "This is a privacy risk on endpoints where users play internal audio recordings or video files. "
                    + "Removing this policy re-enables online media information lookup in WMP.",
                Tags = ["media-player", "metadata", "privacy", "internet", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMusicMetadata", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMusicMetadata")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMusicMetadata", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks online metadata lookup; prevents media file usage disclosure to third parties.",
            },
            new TweakDef
            {
                Id = "wmplay-disable-remote-skin-download",
                Label = "Windows Media Player: Disable Remote Skin and Visualizer Download",
                Category = "System",
                Description =
                    "Prevents Windows Media Player from downloading skins, visualizations, and plug-in content from the Internet. "
                    + "Remote skin and plug-in downloads represent an arbitrary code execution risk if the download source is compromised or spoofed. "
                    + "On managed endpoints, WMP customization content should come only from the software management catalog. "
                    + "Removing this policy re-enables remote skin and visualizer downloads from Microsoft.",
                Tags = ["media-player", "skin", "plugin", "download", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventRadioPresetsRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventRadioPresetsRetrieval")],
                DetectOps = [RegOp.CheckDword(Key, "PreventRadioPresetsRetrieval", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks WMP remote content downloads; prevents unofficial plugins and skins from executing.",
            },
        ];
    }

    // ── WindowsMediaPolicyAdv ──
    private static class _WindowsMediaPolicyAdv
    {
        private const string WmpLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer";
        private const string WmpCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\WindowsMediaPlayer";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wmply-no-screensaver",
                Label = "WMP: Disable screensaver activation during audio playback",
                Category = "System",
                Description =
                    "Sets AllowScreenSaver=0 in the Windows Media Player policy key. Prevents the "
                    + "screensaver from activating while WMP is playing audio, even when the screen is idle.",
                Tags = ["media", "wmp", "screensaver", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpLm, "AllowScreenSaver", 0)],
                RemoveOps = [RegOp.DeleteValue(WmpLm, "AllowScreenSaver")],
                DetectOps = [RegOp.CheckDword(WmpLm, "AllowScreenSaver", 0)],
            },
            new TweakDef
            {
                Id = "wmply-no-network-protocol-download",
                Label = "WMP: Prevent automatic network protocol download",
                Category = "System",
                Description =
                    "Sets PreventNetworkProtocolAutomaticDownload=1. Prevents Windows Media Player from "
                    + "automatically downloading streaming network protocol components.",
                Tags = ["media", "wmp", "network", "protocol", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpLm, "PreventNetworkProtocolAutomaticDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpLm, "PreventNetworkProtocolAutomaticDownload")],
                DetectOps = [RegOp.CheckDword(WmpLm, "PreventNetworkProtocolAutomaticDownload", 1)],
            },
            new TweakDef
            {
                Id = "wmply-user-no-cd-metadata",
                Label = "WMP (user): Prevent CD/DVD metadata retrieval per user",
                Category = "System",
                Description =
                    "Sets PreventCDDVDMetadataRetrieval=1 at the per-user policy scope (HKCU). Enforces "
                    + "no-internet-metadata policy for the current user regardless of machine policy.",
                Tags = ["media", "wmp", "metadata", "privacy", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpCu, "PreventCDDVDMetadataRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpCu, "PreventCDDVDMetadataRetrieval")],
                DetectOps = [RegOp.CheckDword(WmpCu, "PreventCDDVDMetadataRetrieval", 1)],
            },
            new TweakDef
            {
                Id = "wmply-user-no-music-metadata",
                Label = "WMP (user): Prevent music metadata retrieval per user",
                Category = "System",
                Description =
                    "Sets PreventMusicFileMetadataRetrieval=1 at the per-user policy scope (HKCU). "
                    + "Stops the current user's WMP session from downloading online music metadata.",
                Tags = ["media", "wmp", "metadata", "privacy", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpCu, "PreventMusicFileMetadataRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpCu, "PreventMusicFileMetadataRetrieval")],
                DetectOps = [RegOp.CheckDword(WmpCu, "PreventMusicFileMetadataRetrieval", 1)],
            },
            new TweakDef
            {
                Id = "wmply-user-no-radio-presets",
                Label = "WMP (user): Prevent internet radio presets per user",
                Category = "System",
                Description =
                    "Sets PreventRadioPresetsRetrieval=1 at the per-user policy scope (HKCU). Prevents "
                    + "the current user's WMP from fetching online radio station preset lists.",
                Tags = ["media", "wmp", "radio", "privacy", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpCu, "PreventRadioPresetsRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpCu, "PreventRadioPresetsRetrieval")],
                DetectOps = [RegOp.CheckDword(WmpCu, "PreventRadioPresetsRetrieval", 1)],
            },
        ];
    }

    // ── WindowsPerformancePolicy ──
    private static class _WindowsPerformancePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Performance";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wnperf-restrict-background-activity",
                Label = "Restrict Background Application Activity Through Performance Policy",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting background application activity through performance policy limits the CPU and I/O resources that background and suspended applications can consume improving foreground application responsiveness. Background activity restrictions ensure that non-interactive applications do not consume system resources at the expense of user-facing processes. Enterprise workstations running data analytics batch jobs or synchronization tasks in the background benefit from policies that prioritize interactive work. Resource limitations on background activity also constrain the impact of malware that attempts to use background execution contexts for long-running operations like encryption or data exfiltration. Performance policy restrictions apply to applications running in the background execution manager context which covers many Windows Store and background service applications. Organizations should test background activity restrictions to verify that necessary background operations like antivirus scanning and backup software continue to function.",
                Tags = ["performance", "background-activity", "resource-management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictBackgroundActivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictBackgroundActivity")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictBackgroundActivity", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-enable-cpu-priority-boost",
                Label = "Enable CPU Priority Boosting for Foreground Interactive Applications",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "CPU priority boosting for foreground applications gives interactive applications preferential CPU scheduling over background processes improving user experience for interactive computing tasks. Windows automatically boosts the scheduling priority of the foreground application to ensure responsive input handling but policy controls can extend this boost to all interactive applications. Priority boosting ensures that user-visible applications remain responsive even when background tasks are consuming significant CPU resources. Security tools like real-time antivirus engines use background priority to avoid impacting foreground performance and the foreground priority boost ensures that interactive work retains precedence. Organizations should verify that critical services that run in the background are not negatively impacted by foreground priority boosts reducing their ability to complete time-sensitive work. Performance monitoring should verify that the priority boost achieves the intended improvement in interactive responsiveness.",
                Tags = ["performance", "cpu-priority", "foreground-application", "scheduling", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableForegroundPriorityBoost", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableForegroundPriorityBoost")],
                DetectOps = [RegOp.CheckDword(Key, "EnableForegroundPriorityBoost", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-configure-memory-usage-policy",
                Label = "Configure System Memory Usage Policy for Balanced Performance",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Memory usage policy configuration controls how Windows manages physical memory allocation between application working sets page file usage and system cache to optimize performance for the primary system role. Server systems benefit from configuring memory policy to prioritize system cache and services while workstations benefit from policies that prioritize application working sets. Appropriate memory policy configuration reduces the frequency of hard page faults where data must be read from disk rather than satisfied from physical memory. Memory policy can be tuned to reduce paging activity on systems with adequate memory by configuring more aggressive working set retention. Organizations should evaluate the primary workload of each system type when setting memory policy to ensure the configuration matches the workload profile. Memory configuration changes should be validated through performance baseline comparison to verify improvement in the target metrics.",
                Tags = ["performance", "memory-management", "working-set", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableMemoryUsagePolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMemoryUsagePolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMemoryUsagePolicy", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-disable-animated-windows-effects",
                Label = "Disable Animated Window Effects for Improved System Performance",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Animated window effects including window minimize maximize fade and slide animations consume GPU resources and cause perceived latency in window operations that can be eliminated through policy without user impact on most enterprise workstations. Disabling animations through performance policy provides consistent visual performance settings across all managed workstations. On systems with limited GPU resources or dedicated GPU resources needed for business applications animation effects compete for GPU time with productive work. Disabling window animations is particularly beneficial for virtual desktop infrastructure environments where GPU resources are shared across many VM sessions. Policy-based animation control ensures consistent application of performance settings without relying on individual users to configure visual effects manually. The performance impact of disabling animations varies by hardware but is most significant on integrated graphics systems where CPU and GPU share memory bandwidth.",
                Tags = ["performance", "animations", "visual-effects", "gpu", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAnimatedEffects", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAnimatedEffects")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAnimatedEffects", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-configure-disk-io-scheduling",
                Label = "Configure Disk I/O Scheduling for Application vs System Service Balance",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Disk I/O scheduling policy controls how Windows prioritizes disk I/O requests between application I/O background I/O and system service I/O ensuring that critical I/O operations are completed with appropriate latency. I/O scheduling configuration affects how quickly applications can write logs read data files and access databases compared to background operations like disk defragmentation and search indexing. Systems that run intensive background disk I/O operations benefit from I/O scheduling policy that prevents background I/O from saturating disk bandwidth needed by foreground applications. NVMe SSD systems have lower scheduling overhead than traditional spinning disks but can still benefit from I/O prioritization for mixed workloads with both interactive and batch I/O. Organizations should profile disk I/O patterns across their fleet to identify systems where I/O contention is causing application performance degradation that policy can mitigate. I/O scheduling policy is particularly relevant for database servers file servers and storage-intensive workloads.",
                Tags = ["performance", "disk-io", "scheduling", "storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableDiskIOScheduling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDiskIOScheduling")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDiskIOScheduling", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-restrict-startup-program-execution",
                Label = "Restrict Startup Program Execution to Approved Application List",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Startup program execution restrictions prevent unauthorized applications from adding themselves to Windows startup locations and executing automatically at user login increasing both security and performance. Malware and potentially unwanted applications frequently add themselves to startup locations to maintain persistence and execute at each user login. Restricting startup execution to an approved list prevents both unauthorized persistence establishment and the performance degradation from accumulating startup programs over time. Startup program restrictions work best when combined with Software Restriction Policies or AppLocker to prevent programs from loading regardless of how they are invoked. Organizations should define the startup program allowlist based on applications that have legitimate business requirements for startup execution and review and trim this list periodically. User-controlled startup programs should be completely disabled for standard users who should not have the ability to modify which programs run automatically.",
                Tags = ["performance", "startup-programs", "persistence-prevention", "allowlist", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictStartupPrograms", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictStartupPrograms")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictStartupPrograms", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-configure-network-throttling",
                Label = "Configure Network Bandwidth Throttling for Background Update Operations",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Network bandwidth throttling for background update operations limits the bandwidth consumed by Windows Update delivery optimization and other background network consumers preventing them from saturating network connections during productive hours. Without throttling Windows Update delivery optimization and peer-to-peer update distribution can consume significant network bandwidth that impacts interactive work and business applications. Policy-based throttling configurations can restrict background network usage to specific percentages of available bandwidth and can vary restrictions based on time of day to allow unrestricted updates during off-hours. Delivery optimization settings should be configured to prioritize enterprise update caching servers over internet downloads where available reducing upstream bandwidth consumption. Organizations with thin WAN links benefit most from network throttling policies that prevent update traffic from impacting business-critical applications on constrained bandwidth connections. Background network usage should be monitored to verify that throttling is correctly limiting consumption and that updates are still completing within required timeframes.",
                Tags = ["performance", "network-throttling", "background-updates", "bandwidth", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableNetworkThrottling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableNetworkThrottling")],
                DetectOps = [RegOp.CheckDword(Key, "EnableNetworkThrottling", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-enable-prefetch-optimization",
                Label = "Enable Prefetch Optimization for Frequently Used Application Launch",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Prefetch optimization allows Windows to pre-load frequently used application data and code from disk into memory before the application is launched improving perceived application startup performance. Prefetch monitoring tracks which application data is accessed during startup and creates prefetch files that inform future startup pre-loading operations. The prefetch system improves application launch time by ensuring that frequently used executable pages and data files are in memory before they are needed. Prefetch optimization is most effective on spinning disk systems where sequential pre-reading is significantly faster than random access. On SSD systems the performance benefit is smaller but prefetch still provides improvement for large applications with slow-loading modules. Organizations should ensure that prefetch is enabled on production workstations and that the prefetch data directory has adequate storage space for the prefetch files accumulated by the applications in use.",
                Tags = ["performance", "prefetch", "application-launch", "startup", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePrefetchOptimization", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePrefetchOptimization")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePrefetchOptimization", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-configure-power-performance-balance",
                Label = "Configure Power and Performance Balance Policy for Enterprise Workloads",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Power and performance balance policy controls the tradeoff between computing performance and energy consumption ensuring that enterprise workstations deliver appropriate performance for business workloads. High-performance power plans increase processor performance states and disable power saving features that can reduce responsiveness for interactive and compute-intensive applications. Balanced power plans provide a reasonable compromise for most enterprise workloads adjusting performance states dynamically based on workload demand. Performance-critical applications like database servers engineering applications and real-time processing systems benefit from high-performance power plan configurations. Organizations should match power plan configuration to the workload profile of each system type rather than applying a uniform policy across all systems. Power consumption monitoring can verify that the configured power plan achieves the intended energy consumption profile for server hosting environments.",
                Tags = ["performance", "power-plan", "energy", "workload-tuning", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConfigurePowerPerformanceBalance", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigurePowerPerformanceBalance")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigurePowerPerformanceBalance", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-enable-performance-audit-logging",
                Label = "Enable Performance Audit Logging for System Resource Utilization",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Performance audit logging records system resource utilization events including CPU memory disk and network consumption providing data for capacity planning and performance anomaly detection. Performance data collected through audit logging enables historical analysis that identifies performance degradation trends before they become user-impacting problems. Security-relevant performance events such as sudden increases in CPU or memory consumption may indicate ongoing exploitation or malware execution. Performance audit data should be retained and analyzed alongside security event data to correlate performance anomalies with security incidents. Organizations should establish performance baselines for each system role to enable meaningful comparison of current performance against expected ranges. Performance audit logging data should be lightweight and targeted at high-value metrics that provide actionable insight without generating excessive log volume.",
                Tags = ["performance", "audit-logging", "resource-utilization", "baseline", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePerformanceAuditLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePerformanceAuditLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePerformanceAuditLogging", 1)],
            },
        ];
    }

    // ── WindowsReliabilityPolicy ──
    private static class _WindowsReliabilityPolicy
    {
        private const string RelKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Reliability";
        private const string WerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "relpol-disable-shutdown-tracker",
                Label = "Reliability Policy: Disable Shutdown Event Tracker",
                Category = "System",
                Description =
                    "Disables the Shutdown Event Tracker dialog that prompts users or admins for a reason when the system is shut down or restarted. Useful for desktops that do not require uptime tracking.",
                Tags = ["reliability", "shutdown", "event-tracker", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes the shutdown-reason prompt on desktops without uptime tracking.",
                RegistryKeys = [RelKey],
                ApplyOps = [RegOp.SetDword(RelKey, "ShutdownEventTrackerDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(RelKey, "ShutdownEventTrackerDisabled")],
                DetectOps = [RegOp.CheckDword(RelKey, "ShutdownEventTrackerDisabled", 1)],
            },
            new TweakDef
            {
                Id = "relpol-disable-rac-reporting",
                Label = "Reliability Policy: Disable RAC Problem Reporting to Microsoft",
                Category = "System",
                Description =
                    "Disables the Reliability Analysis Component (RAC) from forwarding problem report data to Microsoft. RAC gathers application crash data and forwards it to Problem Reports and Solutions (WER).",
                Tags = ["reliability", "rac", "wer", "reporting", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops RAC from forwarding application crash data to Microsoft via WER.",
                RegistryKeys = [RelKey],
                ApplyOps = [RegOp.SetDword(RelKey, "PCH_DoNotReport", 1)],
                RemoveOps = [RegOp.DeleteValue(RelKey, "PCH_DoNotReport")],
                DetectOps = [RegOp.CheckDword(RelKey, "PCH_DoNotReport", 1)],
            },
            new TweakDef
            {
                Id = "relpol-disable-archive",
                Label = "Reliability Policy: Disable Reliability Data Archive",
                Category = "System",
                Description =
                    "Disables the reliability history archive database written by the Reliability Analysis Component (RACAgent). Prevents creation and retention of Windows reliability scores and application failure records.",
                Tags = ["reliability", "archive", "rac", "history", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents creation of the reliability score database and failure history file.",
                RegistryKeys = [RelKey],
                ApplyOps = [RegOp.SetDword(RelKey, "DisableArchive", 1)],
                RemoveOps = [RegOp.DeleteValue(RelKey, "DisableArchive")],
                DetectOps = [RegOp.CheckDword(RelKey, "DisableArchive", 1)],
            },
            new TweakDef
            {
                Id = "relpol-limit-archive-count",
                Label = "Reliability Policy: Limit Reliability Archive Maximum Count",
                Category = "System",
                Description =
                    "Limits the number of reliability history records stored in the RAC database. Reducing the max archive count prevents unbounded growth of reliability data on low-disk-space endpoints.",
                Tags = ["reliability", "archive", "limit", "disk-space", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Caps reliability history records to prevent unbounded disk usage.",
                RegistryKeys = [RelKey],
                ApplyOps = [RegOp.SetDword(RelKey, "MaxArchiveCount", 10)],
                RemoveOps = [RegOp.DeleteValue(RelKey, "MaxArchiveCount")],
                DetectOps = [RegOp.CheckDword(RelKey, "MaxArchiveCount", 10)],
            },
            new TweakDef
            {
                Id = "relpol-disable-shutdown-reason-required",
                Label = "Reliability Policy: Disable Shutdown Reason Requirement",
                Category = "System",
                Description =
                    "Removes the requirement for users to provide an annotated reason when shutting down or restarting the system. Complements the Shutdown Event Tracker disable for unattended workstations.",
                Tags = ["reliability", "shutdown", "reason", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes the mandatory reason field from the shutdown/restart dialog.",
                RegistryKeys = [RelKey],
                ApplyOps = [RegOp.SetDword(RelKey, "ReasonRequired", 0)],
                RemoveOps = [RegOp.DeleteValue(RelKey, "ReasonRequired")],
                DetectOps = [RegOp.CheckDword(RelKey, "ReasonRequired", 0)],
            },
            new TweakDef
            {
                Id = "relpol-disable-shutdown-reason-display",
                Label = "Reliability Policy: Disable Shutdown Reason UI Display",
                Category = "System",
                Description =
                    "Disables the on-screen display of shutdown reason annotations set by the Shutdown Event Tracker. Reduces noise in end-user shutdown flows where reason data is collected only for IT audit purposes.",
                Tags = ["reliability", "shutdown", "reason", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides shutdown reason annotations from end-user shutdown flows.",
                RegistryKeys = [RelKey],
                ApplyOps = [RegOp.SetDword(RelKey, "ShutdownReasonOn", 0)],
                RemoveOps = [RegOp.DeleteValue(RelKey, "ShutdownReasonOn")],
                DetectOps = [RegOp.CheckDword(RelKey, "ShutdownReasonOn", 0)],
            },
            new TweakDef
            {
                Id = "relpol-disable-wer-ui-prompt",
                Label = "Reliability Policy: Disable WER User Prompt Dialog",
                Category = "System",
                Description =
                    "Suppresses the Windows Error Reporting prompt dialog when an application crashes. On headless or thin-client deployments, the WER dialog can block process termination and require remote intervention.",
                Tags = ["reliability", "wer", "dialog", "prompt", "headless", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Suppresses WER crash dialogs on headless/thin-client deployments.",
                RegistryKeys = [WerKey],
                ApplyOps = [RegOp.SetDword(WerKey, "DisableUI", 1)],
                RemoveOps = [RegOp.DeleteValue(WerKey, "DisableUI")],
                DetectOps = [RegOp.CheckDword(WerKey, "DisableUI", 1)],
            },
            new TweakDef
            {
                Id = "relpol-disable-wer-kernel-dump",
                Label = "Reliability Policy: Disable WER Kernel Fault/Dump Reporting",
                Category = "System",
                Description =
                    "Disables Windows Error Reporting capture of kernel-mode fault data (BSoD minidumps). Prevents automatic transmission of kernel dump data to Microsoft after BSODs on sensitive systems.",
                Tags = ["reliability", "wer", "kernel-dump", "bsod", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables kernel fault dump collection; protects sensitive kernel-space data.",
                RegistryKeys = [WerKey],
                ApplyOps = [RegOp.SetDword(WerKey, "DisableKernelFaultLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(WerKey, "DisableKernelFaultLogging")],
                DetectOps = [RegOp.CheckDword(WerKey, "DisableKernelFaultLogging", 1)],
            },
        ];
    }

    // ── WindowsTimeGpoPolicy ──
    private static class _WindowsTimeGpoPolicy
    {
        private const string W32Params = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Parameters";

        private const string W32Config = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Config";

        private const string NtpClient = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\TimeProviders\NtpClient";

        private const string NtpServer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\TimeProviders\NtpServer";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "timepol-ntp-server-pool",
                Label = "Configure NTP pool servers (policy)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets W32Time to synchronise from the NTP pool servers via GPO. "
                    + "NtpServer=0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9.",
                Tags = ["time", "ntp", "pool", "servers", "policy"],
                ApplyOps = [RegOp.SetString(W32Params, "NtpServer", "0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9")],
                RemoveOps = [RegOp.DeleteValue(W32Params, "NtpServer")],
                DetectOps =
                [
                    RegOp.CheckString(W32Params, "NtpServer", "0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9"),
                ],
            },
            new TweakDef
            {
                Id = "timepol-ntpclient-enable",
                Label = "Enable NTP client time provider (policy)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables the NTP client time provider via Group Policy so W32Time actively syncs from NTP servers. "
                    + "TimeProviders\\NtpClient Enabled=1.",
                Tags = ["time", "ntp", "client", "policy"],
                ApplyOps = [RegOp.SetDword(NtpClient, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(NtpClient, "Enabled")],
                DetectOps = [RegOp.CheckDword(NtpClient, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "timepol-ntpclient-poll-hourly",
                Label = "Set NTP client poll interval to 1 hour (policy)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets the NTP client to poll time servers every hour via policy. "
                    + "SpecialPollInterval=3600. Default: 604800 (1 week). More frequent syncs reduce clock drift.",
                Tags = ["time", "ntp", "poll", "interval", "policy"],
                ApplyOps = [RegOp.SetDword(NtpClient, "SpecialPollInterval", 3600)],
                RemoveOps = [RegOp.DeleteValue(NtpClient, "SpecialPollInterval")],
                DetectOps = [RegOp.CheckDword(NtpClient, "SpecialPollInterval", 3600)],
            },
            new TweakDef
            {
                Id = "timepol-ntpclient-eventlog",
                Label = "Log NTP time jumps and source changes (policy)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables NTP client event logging for time jumps and server source changes. "
                    + "EventLogFlags=3 (1=time jumps, 2=source changes, 3=both). Default: 0.",
                Tags = ["time", "ntp", "logging", "policy"],
                ApplyOps = [RegOp.SetDword(NtpClient, "EventLogFlags", 3)],
                RemoveOps = [RegOp.DeleteValue(NtpClient, "EventLogFlags")],
                DetectOps = [RegOp.CheckDword(NtpClient, "EventLogFlags", 3)],
            },
            new TweakDef
            {
                Id = "timepol-ntpserver-disable",
                Label = "Disable NTP server time provider on workstations (policy)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents this machine from acting as an NTP time server via Group Policy. "
                    + "TimeProviders\\NtpServer Enabled=0. Appropriate for workstations that should be clients only.",
                Tags = ["time", "ntp", "server", "disable", "policy"],
                ApplyOps = [RegOp.SetDword(NtpServer, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(NtpServer, "Enabled")],
                DetectOps = [RegOp.CheckDword(NtpServer, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "timepol-max-pos-correction",
                Label = "Cap maximum positive time correction at 2 hours (policy)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets the maximum positive time correction W32Time will accept via policy. "
                    + "MaxPosPhaseCorrection=7200 (2 hours). Prevents unexpectedly large clock forwards.",
                Tags = ["time", "phase", "correction", "policy"],
                ApplyOps = [RegOp.SetDword(W32Config, "MaxPosPhaseCorrection", 7200)],
                RemoveOps = [RegOp.DeleteValue(W32Config, "MaxPosPhaseCorrection")],
                DetectOps = [RegOp.CheckDword(W32Config, "MaxPosPhaseCorrection", 7200)],
            },
            new TweakDef
            {
                Id = "timepol-max-neg-correction",
                Label = "Cap maximum negative time correction at 2 hours (policy)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets the maximum negative time correction W32Time will accept via policy. "
                    + "MaxNegPhaseCorrection=7200 (2 hours). Prevents unexpectedly large clock rollbacks.",
                Tags = ["time", "phase", "correction", "policy"],
                ApplyOps = [RegOp.SetDword(W32Config, "MaxNegPhaseCorrection", 7200)],
                RemoveOps = [RegOp.DeleteValue(W32Config, "MaxNegPhaseCorrection")],
                DetectOps = [RegOp.CheckDword(W32Config, "MaxNegPhaseCorrection", 7200)],
            },
            new TweakDef
            {
                Id = "timepol-frequency-correct-rate",
                Label = "Set W32Time frequency correction rate (policy)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets how quickly W32Time corrects clock frequency drift via policy. "
                    + "FrequencyCorrectRate=4 (corrects up to 4 × 10ms per second). Default: 4.",
                Tags = ["time", "frequency", "correction", "policy"],
                ApplyOps = [RegOp.SetDword(W32Config, "FrequencyCorrectRate", 4)],
                RemoveOps = [RegOp.DeleteValue(W32Config, "FrequencyCorrectRate")],
                DetectOps = [RegOp.CheckDword(W32Config, "FrequencyCorrectRate", 4)],
            },
            new TweakDef
            {
                Id = "timepol-phase-correct-rate",
                Label = "Set W32Time phase (offset) correction rate (policy)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets how aggressively W32Time corrects phase (offset) errors via policy. "
                    + "PhaseCorrectRate=7 (most aggressive). Default: 1. Higher values resolve drift faster.",
                Tags = ["time", "phase", "offset", "policy"],
                ApplyOps = [RegOp.SetDword(W32Config, "PhaseCorrectRate", 7)],
                RemoveOps = [RegOp.DeleteValue(W32Config, "PhaseCorrectRate")],
                DetectOps = [RegOp.CheckDword(W32Config, "PhaseCorrectRate", 7)],
            },
        ];
    }

    // ── WindowsTimePolicy ──
    private static class _WindowsTimePolicy
    {
        private const string ParamKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Parameters";
        private const string CfgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Config";
        private const string NtpClient = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\TimeProviders\NtpClient";
        private const string NtpServer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\TimeProviders\NtpServer";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wtime-set-update-interval",
                Label = "Set Clock Update Interval to 30000 (30 Seconds)",
                Category = "System",
                Description =
                    "Sets UpdateInterval=30000 in the W32time Config policy key. "
                    + "Configures how often (in 100-nanosecond units, 30000 = approximately 3ms effective interval) "
                    + "the system clock is adjusted to converge on the NTP reference. "
                    + "Using the system default of 30000 via policy pins this to the recommended value. "
                    + "Default: absent. Recommended: 30000 to enforce clock discipline response rate.",
                Tags = ["ntp", "time", "update-interval", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Pins the clock update interval to the W32tm recommended rate of 30000.",
                ApplyOps = [RegOp.SetDword(CfgKey, "UpdateInterval", 30000)],
                RemoveOps = [RegOp.DeleteValue(CfgKey, "UpdateInterval")],
                DetectOps = [RegOp.CheckDword(CfgKey, "UpdateInterval", 30000)],
            },
        ];
    }

    // ── WinlogonPolicy ──
    private static class _WinlogonPolicy
    {
        private const string WlKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Winlogon";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wlpol-require-ctrl-alt-del",
                    Label = "Require Ctrl+Alt+Delete at Login",
                    Category = "System",
                    Description = "Enforces the secure attention sequence (Ctrl+Alt+Delete) before the Windows logon screen appears.",
                    Tags = ["winlogon", "ctrl-alt-del", "logon", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents login spoofing by requiring the hardware-intercepted SAS (Secure Attention Sequence).",
                    ApplyOps = [RegOp.SetDword(WlKey, "DisableCAD", 0)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "DisableCAD")],
                    DetectOps = [RegOp.CheckDword(WlKey, "DisableCAD", 0)],
                },
                new TweakDef
                {
                    Id = "wlpol-disable-autologon",
                    Label = "Disable Automatic Administrator Logon",
                    Category = "System",
                    Description = "Prevents Windows from automatically logging in with a saved administrator account and password at startup.",
                    Tags = ["winlogon", "autologon", "logon", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Disabling AutoAdminLogon forces manual login; critical for devices in shared or public environments.",
                    ApplyOps = [RegOp.SetDword(WlKey, "AutoAdminLogon", 0)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "AutoAdminLogon")],
                    DetectOps = [RegOp.CheckDword(WlKey, "AutoAdminLogon", 0)],
                },
                new TweakDef
                {
                    Id = "wlpol-lock-on-smartcard-removal",
                    Label = "Lock Workstation on Smart Card Removal",
                    Category = "System",
                    Description = "Automatically locks the workstation screen when the user removes their smart card from the reader.",
                    Tags = ["winlogon", "smart-card", "lock", "security", "mfa"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Value 1 = lock workstation; users must re-authenticate after removing their card.",
                    ApplyOps = [RegOp.SetDword(WlKey, "ScRemoveOption", 1)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "ScRemoveOption")],
                    DetectOps = [RegOp.CheckDword(WlKey, "ScRemoveOption", 1)],
                },
                new TweakDef
                {
                    Id = "wlpol-no-grace-period-after-screensaver",
                    Label = "No Grace Period After Screen Saver for Unlock",
                    Category = "System",
                    Description = "Requires immediate credential entry after the screen saver activates, with no grace period delay.",
                    Tags = ["winlogon", "screen-saver", "lock", "grace-period", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Zero-second grace period; users must enter password immediately after screen saver starts.",
                    ApplyOps = [RegOp.SetDword(WlKey, "ScreenSaverGracePeriod", 0)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "ScreenSaverGracePeriod")],
                    DetectOps = [RegOp.CheckDword(WlKey, "ScreenSaverGracePeriod", 0)],
                },
                new TweakDef
                {
                    Id = "wlpol-enable-force-unlock-logon",
                    Label = "Force Credential Re-Entry on Workstation Unlock",
                    Category = "System",
                    Description = "Requires full credential re-entry when unlocking a workstation, even if the same user locked it.",
                    Tags = ["winlogon", "unlock", "credentials", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents pass-through unlock with cached session; full authentication required on every unlock.",
                    ApplyOps = [RegOp.SetDword(WlKey, "ForceUnlockLogon", 1)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "ForceUnlockLogon")],
                    DetectOps = [RegOp.CheckDword(WlKey, "ForceUnlockLogon", 1)],
                },
                new TweakDef
                {
                    Id = "wlpol-block-software-sas",
                    Label = "Block Software-Generated Secure Attention Sequence",
                    Category = "System",
                    Description = "Prevents applications and services from programmatically generating the Ctrl+Alt+Delete SAS.",
                    Tags = ["winlogon", "sas", "security", "ctrl-alt-del"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Value 0 = only hardware can generate SAS; prevents malware from simulating the logon screen.",
                    ApplyOps = [RegOp.SetDword(WlKey, "SoftwareSASGeneration", 0)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "SoftwareSASGeneration")],
                    DetectOps = [RegOp.CheckDword(WlKey, "SoftwareSASGeneration", 0)],
                },
                new TweakDef
                {
                    Id = "wlpol-run-logon-scripts-sync",
                    Label = "Run Logon Scripts Synchronously",
                    Category = "System",
                    Description = "Waits for all logon scripts to complete before presenting the user desktop.",
                    Tags = ["winlogon", "logon-scripts", "gpo", "synchronous"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Desktop shown only after all scripts finish; may increase logon time on complex environments.",
                    ApplyOps = [RegOp.SetDword(WlKey, "RunLogonScriptSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "RunLogonScriptSync")],
                    DetectOps = [RegOp.CheckDword(WlKey, "RunLogonScriptSync", 1)],
                },
                new TweakDef
                {
                    Id = "wlpol-disable-boot-animation",
                    Label = "Disable Windows Boot Animation",
                    Category = "System",
                    Description = "Skips the animated Windows splash screen during boot to reduce boot time and remove branding.",
                    Tags = ["winlogon", "boot", "animation", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removes the spinning dots animation during Windows startup; marginal boot time improvement.",
                    ApplyOps = [RegOp.SetDword(WlKey, "EnableBootStatusPolicy", 0)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "EnableBootStatusPolicy")],
                    DetectOps = [RegOp.CheckDword(WlKey, "EnableBootStatusPolicy", 0)],
                },
                new TweakDef
                {
                    Id = "wlpol-hide-last-logon-user",
                    Label = "Hide Last Logged-On Username at Logon Screen",
                    Category = "System",
                    Description = "Clears the username field at the Windows logon screen so it does not display the last signed-in account.",
                    Tags = ["winlogon", "last-user", "privacy", "logon", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users must type their full username at each login; prevents username enumeration at the logon screen.",
                    ApplyOps = [RegOp.SetDword(WlKey, "DontDisplayLastUserName", 1)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "DontDisplayLastUserName")],
                    DetectOps = [RegOp.CheckDword(WlKey, "DontDisplayLastUserName", 1)],
                },
                new TweakDef
                {
                    Id = "wlpol-limit-cached-logons",
                    Label = "Limit Cached Domain Logon Credentials",
                    Category = "System",
                    Description = "Restricts how many domain credentials Windows caches locally for offline logon situations.",
                    Tags = ["winlogon", "cached-logon", "credentials", "domain", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Caches only 2 domain accounts locally; reduces credential exposure if disk is compromised. Set to 0 to disable caching entirely.",
                    ApplyOps = [RegOp.SetDword(WlKey, "CachedLogonsCount", 2)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "CachedLogonsCount")],
                    DetectOps = [RegOp.CheckDword(WlKey, "CachedLogonsCount", 2)],
                },
            ];
    }
}

internal static class PolicyWindowsFeeds
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsFeeds — Windows RSS Feeds/
    // Ticker controlled via Group Policy (distinct from the Dsh/News widgets path).

    private const string FeedsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsFeeds";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wsfeed-disable-windows-feeds",
            Label = "Disable Windows Feeds via Policy",
            Category = "Privacy",
            Description =
                "Sets DisableWindowsFeeds=1 in the WindowsFeeds Group Policy key. "
                + "Prevents the Windows Feeds (RSS/Atom) integration from running in the taskbar and File Explorer. "
                + "Eliminates background network polling for feed updates and removes the news ticker UI.",
            Tags = ["feeds", "rss", "news", "taskbar", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disables Windows Feed subscriptions and background RSS polling.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "DisableWindowsFeeds", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "DisableWindowsFeeds")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "DisableWindowsFeeds", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-background-sync",
            Label = "Disable Background Feed Synchronisation",
            Category = "Privacy",
            Description =
                "Sets BackgroundSyncEnabled=0 in the WindowsFeeds Group Policy key. "
                + "Prevents Windows from silently synchronising feed content in the background. "
                + "Reduces network traffic and CPU wakeups from feed polling tasks.",
            Tags = ["feeds", "sync", "background", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed content no longer syncs in the background; pages only update when manually opened.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "BackgroundSyncEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "BackgroundSyncEnabled")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "BackgroundSyncEnabled", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-prevent-feed-subscription",
            Label = "Prevent Users from Subscribing to Feeds",
            Category = "Privacy",
            Description =
                "Sets PreventSubscription=1 in the WindowsFeeds Group Policy key. "
                + "Blocks users from subscribing to new RSS/Atom feeds via Internet Explorer or Feed Discovery. "
                + "Useful in controlled environments where feed subscriptions must be centrally managed.",
            Tags = ["feeds", "subscription", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Users cannot add new RSS feed subscriptions in browsers or via auto-discovery.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "PreventSubscription", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "PreventSubscription")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "PreventSubscription", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-prevent-feed-discovery",
            Label = "Prevent Automatic Feed Discovery",
            Category = "Privacy",
            Description =
                "Sets PreventAutoDiscovery=1 in the WindowsFeeds Group Policy key. "
                + "Stops Internet Explorer and other browsers from automatically discovering available feeds "
                + "on visited web pages. Eliminates the feed icon in the toolbar and related network probes.",
            Tags = ["feeds", "discovery", "browser", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed discovery in browsers is disabled; no auto-detection of RSS/Atom links.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "PreventAutoDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "PreventAutoDiscovery")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "PreventAutoDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-unlocked-feeds",
            Label = "Lock Feed List to Prevent User Modifications",
            Category = "Privacy",
            Description =
                "Sets FeedListLocked=1 in the WindowsFeeds Group Policy key. "
                + "Prevents users from adding, removing, or modifying feed subscriptions. "
                + "Administrators retain full control over what feed sources are available systemwide.",
            Tags = ["feeds", "lockdown", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed list is read-only for standard users; only admins can change subscriptions.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "FeedListLocked", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "FeedListLocked")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "FeedListLocked", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-feed-download",
            Label = "Block Feed Content Download",
            Category = "Privacy",
            Description =
                "Sets AllowFeedDownload=0 in the WindowsFeeds Group Policy key. "
                + "Prevents Windows from downloading feed content to the local machine, stopping "
                + "offline reading caches and news-article pre-fetch from consuming bandwidth and storage.",
            Tags = ["feeds", "download", "bandwidth", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed articles are not pre-fetched; online access required to view feed content.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "AllowFeedDownload", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "AllowFeedDownload")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "AllowFeedDownload", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-third-party-feeds",
            Label = "Block Third-Party Feed Providers",
            Category = "Privacy",
            Description =
                "Sets AllowThirdPartyFeeds=0 in the WindowsFeeds Group Policy key. "
                + "Restricts feed aggregation to Windows-native sources only, preventing third-party "
                + "feed aggregators or browser extensions from registering as system-level feed providers.",
            Tags = ["feeds", "third-party", "policy", "enterprise", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Only Windows-native feed mechanisms are permitted; third-party aggregators are blocked.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "AllowThirdPartyFeeds", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "AllowThirdPartyFeeds")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "AllowThirdPartyFeeds", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-feed-reading-pane",
            Label = "Disable Feed Reading Pane",
            Category = "Privacy",
            Description =
                "Sets DisableReadingPane=1 in the WindowsFeeds Group Policy key. "
                + "Removes the feed reading pane from Internet Explorer and Windows RSS Platform view. "
                + "Reduces distraction and prevents previewing unapproved news content inside the browser.",
            Tags = ["feeds", "reading-pane", "browser", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Reading pane in feed viewer is hidden; feeds show in list-only mode.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "DisableReadingPane", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "DisableReadingPane")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "DisableReadingPane", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-enclosure-download",
            Label = "Block Feed Enclosure (Podcast) Auto-Download",
            Category = "Privacy",
            Description =
                "Sets AllowEnclosureDownload=0 in the WindowsFeeds Group Policy key. "
                + "Prevents Windows from automatically downloading podcast and media enclosures "
                + "attached to RSS feed items. Eliminates background large-file downloads triggered by feed updates.",
            Tags = ["feeds", "podcast", "enclosure", "download", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Podcast/media enclosures in RSS feeds are not auto-downloaded.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "AllowEnclosureDownload", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "AllowEnclosureDownload")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "AllowEnclosureDownload", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-restrict-feed-secure-only",
            Label = "Restrict Feeds to HTTPS Sources Only",
            Category = "Privacy",
            Description =
                "Sets SecureFeedsOnly=1 in the WindowsFeeds Group Policy key. "
                + "Enforces that only feeds served over HTTPS are accepted by the Windows RSS Platform. "
                + "Blocks plain HTTP feed URLs which could be subject to man-in-the-middle injection and content tampering.",
            Tags = ["feeds", "https", "security", "policy", "network"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "HTTP feed URLs are rejected; all feed sources must use HTTPS.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "SecureFeedsOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "SecureFeedsOnly")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "SecureFeedsOnly", 1)],
        },
    ];
}

// ── Sprint 658 — PolicyCompressedFolders ─────────────────────────────────────
internal static class PolicyCompressedFolders
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Explorer — controls
    // ZIP/compressed folder integration in File Explorer and shell.
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\CompressedFolders — dedicated key.

    private const string ZipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CompressedFolders";
    private const string ExplKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "zipfld-disable-compressed-folders",
            Label = "Disable ZIP Compressed Folder Support in Explorer",
            Category = "Storage",
            Description =
                "Sets DisableCompressedFolders=1 in the CompressedFolders Group Policy key. "
                + "Removes the native ZIP/compressed folder handler from File Explorer. "
                + "Users can no longer double-click a ZIP file to browse it as a folder within Explorer. "
                + "Useful when a third-party archiver (7-Zip, WinRAR) is the preferred tool on managed machines.",
            Tags = ["zip", "compressed", "explorer", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "ZIP files no longer open as virtual folders in Explorer; requires a third-party archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableCompressedFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableCompressedFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableCompressedFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-extract-all",
            Label = "Remove 'Extract All' Context-Menu Option",
            Category = "Storage",
            Description =
                "Sets DisableExtractAll=1 in the CompressedFolders Group Policy key. "
                + "Hides the 'Extract All' entry from the right-click context menu on ZIP files. "
                + "Combined with a managed archiver deployment, this enforces the corporate tool for archive extraction.",
            Tags = ["zip", "extract", "context-menu", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "'Extract All' is removed from ZIP context menus; users must use an installed archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableExtractAll", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableExtractAll")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableExtractAll", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-compress-selected-files",
            Label = "Remove 'Compress to ZIP' Context-Menu Option",
            Category = "Storage",
            Description =
                "Sets DisableNewCompressedFolder=1 in the CompressedFolders Group Policy key. "
                + "Removes the 'Compress to ZIP file' entry from the File Explorer shell context menu. "
                + "Prevents users from creating ZIP files directly from Explorer, directing archive operations to managed tools.",
            Tags = ["zip", "compress", "context-menu", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "ZIP creation from Explorer context menu is hidden; archiver tool required.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableNewCompressedFolder", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableNewCompressedFolder")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableNewCompressedFolder", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-block-network-archive-open",
            Label = "Block Opening Remote ZIP Files as Virtual Folders",
            Category = "Storage",
            Description =
                "Sets DisableNetworkCompressedFolders=1 in the CompressedFolders Group Policy key. "
                + "Prevents users from browsing ZIP archives located on network shares as virtual folders. "
                + "Reduces risk of data exfiltration via archive browsing of network resources and prevents "
                + "potential path-traversal attacks embedded in malicious remote ZIP files.",
            Tags = ["zip", "network", "security", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "ZIP files on network drives cannot be browsed as virtual folders in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableNetworkCompressedFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableNetworkCompressedFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableNetworkCompressedFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-cab-browsing",
            Label = "Disable CAB File Browsing in Explorer",
            Category = "Storage",
            Description =
                "Sets DisableCabFolders=1 in the CompressedFolders Group Policy key. "
                + "Prevents File Explorer from opening Microsoft Cabinet (.cab) files as virtual folders. "
                + "CAB files are used as installers and update containers — browsing them directly can "
                + "expose sensitive setup binaries. Forcing use of proper extraction tools adds an audit layer.",
            Tags = ["cab", "cabinet", "compressed", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = ".cab files no longer open as virtual folders; dedicated extraction required.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableCabFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableCabFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableCabFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-restrict-autorun-in-archive",
            Label = "Block AutoRun Execution Inside Archive Folders",
            Category = "Storage",
            Description =
                "Sets BlockArchiveAutoRun=1 in the CompressedFolders Group Policy key. "
                + "Prevents autorun.inf scripts embedded in ZIP/CAB archives from executing when the archive "
                + "is browsed as a virtual folder. Removes a potential initial-access vector for malware "
                + "distributed via weaponised archives delivered over email or USB.",
            Tags = ["zip", "autorun", "security", "malware", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "AutoRun scripts inside archives are blocked from executing within Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "BlockArchiveAutoRun", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "BlockArchiveAutoRun")],
            DetectOps = [RegOp.CheckDword(ZipKey, "BlockArchiveAutoRun", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-zip-sendto",
            Label = "Remove 'Send To Compressed Folder' from Right-Click",
            Category = "Storage",
            Description =
                "Sets DisableSendToCompressed=1 in the CompressedFolders Group Policy key. "
                + "Removes the 'Compressed (zipped) folder' destination from the Send To context menu entry. "
                + "Prevents casual in-place ZIP creation that bypasses DLP scanning on managed endpoints.",
            Tags = ["zip", "sendto", "context-menu", "dlp", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Send To > Compressed Folder is hidden; users must use an explicit archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableSendToCompressed", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableSendToCompressed")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableSendToCompressed", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-restrict-archive-max-size",
            Label = "Enforce Maximum Archive Size Limit",
            Category = "Storage",
            Description =
                "Sets MaxArchiveSizeMB=512 in the CompressedFolders Group Policy key. "
                + "Limits the maximum size of archives that Explorer will open as virtual folders to 512 MB. "
                + "Prevents ZIP-bomb denial-of-service attacks and runaway memory consumption when users "
                + "accidentally open decompression-ratio-maximised archives.",
            Tags = ["zip", "size-limit", "security", "dos", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Archives larger than 512 MB will not open as virtual folders in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "MaxArchiveSizeMB", 512)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "MaxArchiveSizeMB")],
            DetectOps = [RegOp.CheckDword(ZipKey, "MaxArchiveSizeMB", 512)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-archive-preview-handler",
            Label = "Disable Archive Preview Handler in Reading Pane",
            Category = "Storage",
            Description =
                "Sets DisableArchivePreviewHandler=1 in the CompressedFolders Group Policy key. "
                + "Prevents the Explorer Reading Pane from rendering a ZIP/CAB file preview when it is selected. "
                + "Preview rendering parses archive headers in-process; disabling it reduces attack surface for "
                + "vulnerabilities in the compressed-folders shell handler.",
            Tags = ["zip", "preview", "reading-pane", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Archive files show no preview in Explorer Reading Pane.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableArchivePreviewHandler", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableArchivePreviewHandler")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableArchivePreviewHandler", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-enforce-archive-scan-on-open",
            Label = "Enforce Antivirus Scan Before Opening Archive Content",
            Category = "Storage",
            Description =
                "Sets RequireScanBeforeArchiveOpen=1 in the CompressedFolders Group Policy key. "
                + "Forces Windows Defender or the registered antivirus to scan archive contents before "
                + "the virtual folder view is presented to the user. Prevents deferred-scan gaps where "
                + "malicious payloads inside archives reach the desktop before AV inspection completes.",
            Tags = ["zip", "antivirus", "scan", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Archive contents are AV-scanned before being displayed in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "RequireScanBeforeArchiveOpen", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "RequireScanBeforeArchiveOpen")],
            DetectOps = [RegOp.CheckDword(ZipKey, "RequireScanBeforeArchiveOpen", 1)],
        },
    ];
}

// ── Sprint 659 — PolicyWindowsChat ───────────────────────────────────────────
internal static class PolicyWindowsChat
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Chat — controls the Teams
    // consumer chat integration pinned to the Windows 11 taskbar.
    // Additional key: HKLM\SOFTWARE\Policies\Microsoft\Windows\Calling — voice calling integration.

    private const string ChatKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Chat";
    private const string CallingKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Calling";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wschat-set-chat-icon-hidden",
            Label = "Hide Chat Icon from Taskbar via Policy",
            Category = "Communication",
            Description =
                "Sets ChatIcon=2 in the Windows Chat Group Policy key. "
                + "Value 2 hides the Teams Chat / Meet Now icon from the Windows 11 taskbar. "
                + "Removes the consumer collaboration entry point on managed enterprise workstations "
                + "where the full Microsoft Teams Professional client is deployed instead.",
            Tags = ["chat", "teams", "taskbar", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Teams consumer chat icon is hidden from taskbar; Teams business client unaffected.",
            ApplyOps = [RegOp.SetDword(ChatKey, "ChatIcon", 2)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "ChatIcon")],
            DetectOps = [RegOp.CheckDword(ChatKey, "ChatIcon", 2)],
        },
        new TweakDef
        {
            Id = "wschat-disable-consumer-teams",
            Label = "Block Consumer Microsoft Teams Chat",
            Category = "Communication",
            Description =
                "Sets AllowTeamsChat=0 in the Windows Chat Group Policy key. "
                + "Blocks the consumer/personal Microsoft Teams integration built into Windows 11. "
                + "Prevents first-run wizard, account linking, and persistent notification badge for "
                + "Teams consumer on managed endpoints where personal accounts must not be used.",
            Tags = ["chat", "teams", "consumer", "policy", "enterprise", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Consumer Teams chat cannot be launched or configured from Windows 11.",
            ApplyOps = [RegOp.SetDword(ChatKey, "AllowTeamsChat", 0)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "AllowTeamsChat")],
            DetectOps = [RegOp.CheckDword(ChatKey, "AllowTeamsChat", 0)],
        },
        new TweakDef
        {
            Id = "wschat-disable-chat-notification-badge",
            Label = "Remove Chat Notification Badge on Taskbar",
            Category = "Communication",
            Description =
                "Sets HideChatBadge=1 in the Windows Chat Group Policy key. "
                + "Removes the unread-message badge overlaid on the Chat taskbar button. "
                + "Reduces distraction on workstations and prevents consumer-chat notifications "
                + "from drawing attention during corporate presentations or screen shares.",
            Tags = ["chat", "teams", "notification", "taskbar", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Chat notification badge (unread count) no longer shown on taskbar.",
            ApplyOps = [RegOp.SetDword(ChatKey, "HideChatBadge", 1)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "HideChatBadge")],
            DetectOps = [RegOp.CheckDword(ChatKey, "HideChatBadge", 1)],
        },
        new TweakDef
        {
            Id = "wschat-disable-first-launch-experience",
            Label = "Suppress Teams Chat First-Launch Welcome Screen",
            Category = "Communication",
            Description =
                "Sets SuppressFirstLaunchExperience=1 in the Windows Chat Group Policy key. "
                + "Skips the consumer Teams onboarding flow (sign-in prompt, terms screen, EULA) "
                + "when a user launches Chat for the first time. On managed machines the consumer "
                + "profile wizard interferes with standard provisioning and MDM enrollment flows.",
            Tags = ["chat", "teams", "onboarding", "first-run", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Consumer Teams first-run welcome flow is suppressed.",
            ApplyOps = [RegOp.SetDword(ChatKey, "SuppressFirstLaunchExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "SuppressFirstLaunchExperience")],
            DetectOps = [RegOp.CheckDword(ChatKey, "SuppressFirstLaunchExperience", 1)],
        },
        new TweakDef
        {
            Id = "wschat-disable-personal-account-linking",
            Label = "Block Personal Account Linking to Chat",
            Category = "Communication",
            Description =
                "Sets BlockPersonalAccountLinking=1 in the Windows Chat Group Policy key. "
                + "Prevents users from signing the built-in Chat with a personal Microsoft account. "
                + "Enforces the boundary between consumer and enterprise identity on shared and BYOD machines.",
            Tags = ["chat", "teams", "personal-account", "identity", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Personal Microsoft accounts cannot be linked to the Windows Chat taskbar widget.",
            ApplyOps = [RegOp.SetDword(ChatKey, "BlockPersonalAccountLinking", 1)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "BlockPersonalAccountLinking")],
            DetectOps = [RegOp.CheckDword(ChatKey, "BlockPersonalAccountLinking", 1)],
        },
        new TweakDef
        {
            Id = "wschat-disable-calling-integration",
            Label = "Disable Windows 11 Calling Integration",
            Category = "Communication",
            Description =
                "Sets AllowWindowsCalling=0 in the Calling Group Policy key. "
                + "Removes the Windows 11 calling integration that allows PSTN-linked mobile phone "
                + "numbers to be used directly from the Windows Start menu and taskbar call panel. "
                + "Reduces data-sharing with the linked mobile device on managed workstations.",
            Tags = ["calling", "phone", "integration", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Windows 11 taskbar Calling panel is disabled; desk phone/VOIP tools unaffected.",
            ApplyOps = [RegOp.SetDword(CallingKey, "AllowWindowsCalling", 0)],
            RemoveOps = [RegOp.DeleteValue(CallingKey, "AllowWindowsCalling")],
            DetectOps = [RegOp.CheckDword(CallingKey, "AllowWindowsCalling", 0)],
        },
        new TweakDef
        {
            Id = "wschat-disable-calling-auto-start",
            Label = "Prevent Windows Calling Service Auto-Start",
            Category = "Communication",
            Description =
                "Sets DisableCallingAutoStart=1 in the Calling Group Policy key. "
                + "Stops the Windows Calling background service from starting automatically at user login. "
                + "Reduces login latency and background network activity from the calling integration agent.",
            Tags = ["calling", "startup", "background", "policy", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Calling service does not auto-start; initiated only if user manually opens Calling.",
            ApplyOps = [RegOp.SetDword(CallingKey, "DisableCallingAutoStart", 1)],
            RemoveOps = [RegOp.DeleteValue(CallingKey, "DisableCallingAutoStart")],
            DetectOps = [RegOp.CheckDword(CallingKey, "DisableCallingAutoStart", 1)],
        },
        new TweakDef
        {
            Id = "wschat-disable-caller-id-lookup",
            Label = "Disable Caller ID Lookup via Microsoft Services",
            Category = "Communication",
            Description =
                "Sets AllowCallerIdLookup=0 in the Calling Group Policy key. "
                + "Prevents Windows from sending caller phone numbers to Microsoft Cloud to resolve "
                + "caller identity and display name before a call is answered. "
                + "Caller ID numbers remain local-only; no data leaves the device for name resolution.",
            Tags = ["calling", "caller-id", "privacy", "cloud", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Phone numbers from incoming calls are not sent to Microsoft for lookup.",
            ApplyOps = [RegOp.SetDword(CallingKey, "AllowCallerIdLookup", 0)],
            RemoveOps = [RegOp.DeleteValue(CallingKey, "AllowCallerIdLookup")],
            DetectOps = [RegOp.CheckDword(CallingKey, "AllowCallerIdLookup", 0)],
        },
        new TweakDef
        {
            Id = "wschat-disable-chat-history-sync",
            Label = "Block Cross-Device Chat History Sync",
            Category = "Communication",
            Description =
                "Sets DisableChatHistorySync=1 in the Windows Chat Group Policy key. "
                + "Prevents the consumer Teams chat history from synchronising across all devices linked "
                + "to the same personal Microsoft account. On shared terminals or kiosk devices, chat "
                + "history from other devices must not appear and could expose personal/confidential conversations.",
            Tags = ["chat", "history", "sync", "privacy", "cloud", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Chat history from other devices is not synced to this machine.",
            ApplyOps = [RegOp.SetDword(ChatKey, "DisableChatHistorySync", 1)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "DisableChatHistorySync")],
            DetectOps = [RegOp.CheckDword(ChatKey, "DisableChatHistorySync", 1)],
        },
        new TweakDef
        {
            Id = "wschat-restrict-chat-file-transfer",
            Label = "Block File Transfer via Consumer Chat",
            Category = "Communication",
            Description =
                "Sets DisableChatFileTransfer=1 in the Windows Chat Group Policy key. "
                + "Prevents users from sending or receiving files through the consumer Teams chat integration. "
                + "Removes an unmonitored side channel for data exfiltration that bypasses corporate DLP "
                + "policies configured on the enterprise Teams deployment.",
            Tags = ["chat", "file-transfer", "dlp", "security", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "File sharing in consumer chat is blocked; corporate Teams file sharing unaffected.",
            ApplyOps = [RegOp.SetDword(ChatKey, "DisableChatFileTransfer", 1)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "DisableChatFileTransfer")],
            DetectOps = [RegOp.CheckDword(ChatKey, "DisableChatFileTransfer", 1)],
        },
    ];
}

// ── Sprint 660 — PolicyTextInputExt ──────────────────────────────────────────
internal static class PolicyTextInputExt
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\TextInput — additional values beyond
    // the 5 already covered in existing modules (AllowHandwritingLMUpdate,
    // AllowInputDeviceUserInterface, AllowLinguisticDataCollection,
    // AllowTouchKeyboardAutoInvokeInDesktopMode, AllowVoiceTyping).

    private const string TiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TextInput";
    private const string ImeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IME";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "txtin2-disable-text-prediction",
            Label = "Disable Text Prediction for Physical Keyboards via Policy",
            Category = "Input",
            Description =
                "Sets AllowHardwareKeyboardTextSuggestions=0 in the TextInput Group Policy key. "
                + "Prevents Windows from showing inline word-completion suggestions on hardware (physical) keyboards. "
                + "Text prediction sends keystroke patterns to the language model; disabling preserves "
                + "the privacy of typed content on corporate devices.",
            Tags = ["text-input", "keyboard", "prediction", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Word suggestions no longer appear when typing on a physical keyboard.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowHardwareKeyboardTextSuggestions", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowHardwareKeyboardTextSuggestions")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowHardwareKeyboardTextSuggestions", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-user-settings-override",
            Label = "Lock Text Input Settings from User Override",
            Category = "Input",
            Description =
                "Sets AllowUserSettings=0 in the TextInput Group Policy key. "
                + "Prevents users from modifying text input settings (autocorrect, prediction thresholds, "
                + "handwriting personalisation) via Windows Settings. "
                + "All text-input behaviour is controlled exclusively by Group Policy on managed machines.",
            Tags = ["text-input", "settings", "lockdown", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Text Input settings page is read-only for standard users.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowUserSettings", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowUserSettings")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowUserSettings", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-autocorrect",
            Label = "Disable Hardware Keyboard Autocorrect via Policy",
            Category = "Input",
            Description =
                "Sets AllowKeyboardAutocorrect=0 in the TextInput Group Policy key. "
                + "Disables automatic spelling correction on hardware keyboard input. "
                + "Prevents autocorrect from silently changing intended technical terms, passwords, "
                + "or code identifiers in document editors and developer tools.",
            Tags = ["text-input", "autocorrect", "keyboard", "policy", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Keyboard autocorrect is disabled; all typed text is preserved verbatim.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowKeyboardAutocorrect", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowKeyboardAutocorrect")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowKeyboardAutocorrect", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-user-feedback-submission",
            Label = "Block Text Input User Feedback Telemetry",
            Category = "Input",
            Description =
                "Sets AllowUserFeedback=0 in the TextInput Group Policy key. "
                + "Prevents the Text Input (handwriting, touch keyboard, voice typing) subsystem from "
                + "prompting users for satisfaction ratings or submitting diagnostic feedback data "
                + "to Microsoft for language model improvement.",
            Tags = ["text-input", "telemetry", "feedback", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Text input feedback prompts are disabled; no telemetry submitted from input panel.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowUserFeedback", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowUserFeedback")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowUserFeedback", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ink-personalization-upload",
            Label = "Block Handwriting Personalisation Data Upload",
            Category = "Input",
            Description =
                "Sets AllowHandwritingPersonalizationUpload=0 in the TextInput Group Policy key. "
                + "Prevents handwriting recognition data (pen strokes and corrections) from being "
                + "transmitted to Microsoft servers for model personalisation. "
                + "Handwriting recognition continues to function using on-device models only.",
            Tags = ["text-input", "handwriting", "personalisation", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Ink/handwriting training data is not uploaded; on-device recognition continues.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowHandwritingPersonalizationUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowHandwritingPersonalizationUpload")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowHandwritingPersonalizationUpload", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ime-internet-access",
            Label = "Block IME Access to External Prediction Services",
            Category = "Input",
            Description =
                "Sets AllowIMENetworkAccess=0 in the TextInput Group Policy key. "
                + "Prevents Input Method Editors (IME) for CJK and other scripts from accessing the "
                + "internet for cloud-based candidate suggestions, emoji recommendations, and dictionary updates. "
                + "Eliminates keystroke exfiltration risk from cloud-connected IME prediction services.",
            Tags = ["text-input", "ime", "network", "privacy", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "IME prediction is limited to local/offline dictionaries; cloud suggestions disabled.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowIMENetworkAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowIMENetworkAccess")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowIMENetworkAccess", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-cloud-ime-candidates",
            Label = "Disable Cloud-Based IME Candidate Suggestions",
            Category = "Input",
            Description =
                "Sets AllowCloudCandidates=0 in the IME Group Policy key. "
                + "Prevents the Windows IME from fetching real-time cloud candidate improvements. "
                + "Cloud IME candidates require sending the current input context to Microsoft servers, "
                + "posing a risk of partial document or credential context disclosure in enterprise environments.",
            Tags = ["ime", "cloud", "candidates", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "IME uses only installed local candidate dictionaries; no cloud lookups.",
            ApplyOps = [RegOp.SetDword(ImeKey, "AllowCloudCandidates", 0)],
            RemoveOps = [RegOp.DeleteValue(ImeKey, "AllowCloudCandidates")],
            DetectOps = [RegOp.CheckDword(ImeKey, "AllowCloudCandidates", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ime-update",
            Label = "Block IME Automatic Dictionary Updates",
            Category = "Input",
            Description =
                "Sets AllowIMEUpdate=0 in the IME Group Policy key. "
                + "Prevents the Windows IME from automatically downloading dictionary and language model "
                + "updates from Microsoft Graph or Update servers. "
                + "Stabilises IME behaviour in controlled environments where software changes must be sanctioned.",
            Tags = ["ime", "update", "dictionary", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "IME dictionaries are frozen; language model updates require IT-approved deployment.",
            ApplyOps = [RegOp.SetDword(ImeKey, "AllowIMEUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(ImeKey, "AllowIMEUpdate")],
            DetectOps = [RegOp.CheckDword(ImeKey, "AllowIMEUpdate", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ime-telemetry",
            Label = "Disable IME Typing Telemetry",
            Category = "Input",
            Description =
                "Sets AllowIMETelemetry=0 in the IME Group Policy key. "
                + "Blocks the Windows IME from transmitting typing pattern, candidate selection, "
                + "and correction data to Microsoft for telemetry and model improvement. "
                + "Typing content is a high-sensitivity data stream; telemetry must be controlled in regulated sectors.",
            Tags = ["ime", "telemetry", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "IME typing telemetry is disabled; no input pattern data transmitted to Microsoft.",
            ApplyOps = [RegOp.SetDword(ImeKey, "AllowIMETelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(ImeKey, "AllowIMETelemetry")],
            DetectOps = [RegOp.CheckDword(ImeKey, "AllowIMETelemetry", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-touch-keyboard-auto-invoke",
            Label = "Prevent Touch Keyboard Auto-Invoke in Tablet Mode",
            Category = "Input",
            Description =
                "Sets AllowTouchKeyboardAutoInvoke=0 in the TextInput Group Policy key. "
                + "Disables the automatic appearance of the on-screen touch keyboard when a text field "
                + "gains focus in tablet mode. Users must manually invoke the touch keyboard, preventing "
                + "layout interference on large-screen kiosk and hybrid devices.",
            Tags = ["text-input", "touch-keyboard", "tablet", "kiosk", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Touch keyboard no longer pops up automatically when tapping text fields.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowTouchKeyboardAutoInvoke", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowTouchKeyboardAutoInvoke")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowTouchKeyboardAutoInvoke", 0)],
        },
    ];
}

// ── Sprint 661 — PolicySpeechInput ────────────────────────────────────────────
internal static class PolicySpeechInput
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\Speech — Speech Recognition / Voice Access.
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\SpeechModel — online speech model policies.

    private const string SpeechKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Speech";
    private const string ModelKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SpeechModel";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "spkinput-disable-online-speech-recognition",
            Label = "Disable Online Speech Recognition via Policy",
            Category = "Privacy",
            Description =
                "Sets AllowSpeechRecognition=0 in the Speech Group Policy key. "
                + "Prevents the cloud speech recognition service from being used for Windows speech features. "
                + "Voice data is only processed on-device; no audio is transmitted to Microsoft Speech servers. "
                + "Applies broadly to Cortana voice queries, Voice Typing, and Voice Access cloud enhancement.",
            Tags = ["speech", "recognition", "cloud", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Cloud speech recognition disabled; on-device speech processing continues.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechRecognition", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechRecognition")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechRecognition", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-activation",
            Label = "Block Always-On Voice Activation",
            Category = "Privacy",
            Description =
                "Sets AllowVoiceActivation=0 in the Speech Group Policy key. "
                + "Prevents applications from using the always-on voice listening hook (keyword detection). "
                + "Eliminates the continuous microphone monitoring required for wake words ('Hey Cortana', etc.), "
                + "removing a permanent audio capture pipeline from the endpoint.",
            Tags = ["speech", "voice-activation", "wake-word", "microphone", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Always-on wake word detection is disabled; microphone not continuously monitored.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceActivation", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceActivation")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceActivation", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-model-update",
            Label = "Block Automatic Speech Model Updates",
            Category = "Privacy",
            Description =
                "Sets AllowSpeechModelUpdate=0 in the SpeechModel Group Policy key. "
                + "Prevents Windows from automatically downloading and applying updated cloud or on-device "
                + "speech recognition model files. Stabilises speech behaviour in validated regulated "
                + "environments where untested model changes could affect accessibility tools.",
            Tags = ["speech", "model-update", "policy", "enterprise", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Speech model files are frozen; updates require IT-managed deployment.",
            ApplyOps = [RegOp.SetDword(ModelKey, "AllowSpeechModelUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(ModelKey, "AllowSpeechModelUpdate")],
            DetectOps = [RegOp.CheckDword(ModelKey, "AllowSpeechModelUpdate", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-telemetry",
            Label = "Disable Speech Input Telemetry",
            Category = "Privacy",
            Description =
                "Sets AllowSpeechTelemetry=0 in the Speech Group Policy key. "
                + "Blocks the Speech subsystem from sending diagnostic voice data, recognition accuracy "
                + "metrics, and corrected text snippets to Microsoft for model improvement. "
                + "Audio utterances and transcription corrections are classified as personal data under GDPR/HIPAA.",
            Tags = ["speech", "telemetry", "privacy", "compliance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Speech telemetry suppressed; no voice samples or transcription data transmitted.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechTelemetry")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-typing",
            Label = "Disable Voice Typing via Policy",
            Category = "Privacy",
            Description =
                "Sets AllowVoiceTyping=0 in the Speech Group Policy key. "
                + "Disables the Voice Typing feature (Win+H) systemwide via Group Policy. "
                + "Prevents users from dictating text into any application, stopping the microphone "
                + "activation path associated with dictation on shared and kiosk workstations.",
            Tags = ["speech", "voice-typing", "dictation", "microphone", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Voice Typing (Win+H) is disabled; microphone not used for dictation.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceTyping", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceTyping")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceTyping", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-cortana-voice",
            Label = "Disable Cortana Voice Interaction via Policy",
            Category = "Privacy",
            Description =
                "Sets AllowCortanaVoice=0 in the Speech Group Policy key. "
                + "Prevents Cortana from accepting voice input and responding to spoken queries. "
                + "Complements the Cortana keyboard disable by also closing the audio/microphone channel "
                + "used for Cortana's voice assistant functionality.",
            Tags = ["speech", "cortana", "voice", "microphone", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Cortana no longer accepts voice queries; keyboard interaction unaffected.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowCortanaVoice", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowCortanaVoice")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowCortanaVoice", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-personalization",
            Label = "Block Speech Personalisation Data Collection",
            Category = "Privacy",
            Description =
                "Sets AllowSpeechPersonalization=0 in the Speech Group Policy key. "
                + "Stops Windows from collecting contacts, calendar events, frequently typed words, "
                + "and app usage patterns to personalise speech recognition accuracy. "
                + "This dataset would stay on-device but its aggregation represents a privacy concern "
                + "in regulated environments where data minimisation principles apply.",
            Tags = ["speech", "personalisation", "privacy", "policy", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Speech personalisation disabled; recognition accuracy unchanged.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechPersonalization", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechPersonalization")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechPersonalization", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-access-start",
            Label = "Prevent Voice Access from Starting at Login",
            Category = "Privacy",
            Description =
                "Sets AllowVoiceAccessStartup=0 in the Speech Group Policy key. "
                + "Prevents the Windows Voice Access feature from automatically starting when a user logs "
                + "into Windows. Voice Access requires persistent microphone access; letting it auto-start "
                + "runs an unnecessary audio capture pipeline on workstations not requiring accessibility.",
            Tags = ["speech", "voice-access", "startup", "microphone", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Voice Access does not auto-start; users can still launch it manually.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceAccessStartup", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceAccessStartup")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceAccessStartup", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-restrict-online-speech-model",
            Label = "Block Online Speech Model Download",
            Category = "Privacy",
            Description =
                "Sets AllowOnlineSpeechModel=0 in the SpeechModel Group Policy key. "
                + "Prevents Windows from downloading an enhanced online speech recognition model "
                + "that improves accuracy beyond the locally installed model. "
                + "Disabling the download removes a background network data transfer and pins "
                + "speech processing to on-device models vetted by the organisation.",
            Tags = ["speech", "model", "download", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Online speech model is not downloaded; on-device model used exclusively.",
            ApplyOps = [RegOp.SetDword(ModelKey, "AllowOnlineSpeechModel", 0)],
            RemoveOps = [RegOp.DeleteValue(ModelKey, "AllowOnlineSpeechModel")],
            DetectOps = [RegOp.CheckDword(ModelKey, "AllowOnlineSpeechModel", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-access-across-lock",
            Label = "Disable Speech Recognition on Lock Screen",
            Category = "Privacy",
            Description =
                "Sets AllowSpeechOnLockScreen=0 in the Speech Group Policy key. "
                + "Prevents voice assistants and speech recognition from accepting voice input when "
                + "the workstation screen is locked. Eliminates the attack surface where an attacker "
                + "with physical access to a locked machine can issue voice commands to local assistants.",
            Tags = ["speech", "lock-screen", "security", "policy", "physical-security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Voice assistant cannot be invoked from locked screen; prevents audio-based attacks.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechOnLockScreen", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechOnLockScreen")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechOnLockScreen", 0)],
        },
    ];
}

internal static class PolicyAutoRun
{
    private const string ExplorerPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";
    private const string ExplorerCur = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "autoplay-policy-disable-autorun",
            Label = "Disable AutoRun via Policy (All Drives)",
            Category = "Security",
            Description =
                "Sets NoDriveTypeAutoRun=0xFF under the Windows\\CurrentVersion\\Policies\\Explorer key. "
                + "Disables AutoRun for all drive types (0xFF = all bits set), including optical drives, removable drives, "
                + "network drives, and fixed drives. "
                + "Prevents malware from exploiting the autorun.inf mechanism on any media.",
            Tags = ["autorun", "autoplay", "drives", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "AutoRun disabled on all drive types; eliminates autorun.inf exploitation vector.",
            ApplyOps = [RegOp.SetDword(ExplorerCur, "NoDriveTypeAutoRun", 0xFF)],
            RemoveOps = [RegOp.DeleteValue(ExplorerCur, "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(ExplorerCur, "NoDriveTypeAutoRun", 0xFF)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-prevent-mixed-content",
            Label = "Prevent AutoPlay for Mixed-Content Drives",
            Category = "Security",
            Description =
                "Sets HonorAutorunSetting=1 under the Windows\\Explorer Group Policy key. "
                + "Instructs Windows to honour the AutoRun setting from the device itself for mixed-content discs "
                + "(CD/DVD with both data and audio tracks). "
                + "Ensures that mixed-content media does not bypass AutoRun suppression policies.",
            Tags = ["autoplay", "cd", "dvd", "mixed-content", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Mixed-content media respects per-device AutoRun flags; no automatic execution on insert.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "HonorAutorunSetting", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "HonorAutorunSetting")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "HonorAutorunSetting", 1)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-block-set-default",
            Label = "Block Users from Changing AutoPlay Default",
            Category = "Security",
            Description =
                "Sets NoAutoplayBackpropagation=1 under the Windows\\Explorer Group Policy key. "
                + "Prevents the AutoPlay dialog from remembering and persisting new user-selected defaults. "
                + "Ensures that the centrally-configured AutoPlay policy is not overridden by individual user actions.",
            Tags = ["autoplay", "policy", "enterprise", "default"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "User-selected AutoPlay defaults are not saved; policy setting always takes precedence.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "NoAutoplayBackpropagation", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "NoAutoplayBackpropagation")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "NoAutoplayBackpropagation", 1)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-disable-shell-autoplay-handlers",
            Label = "Disable Shell AutoPlay Handlers for Removable Media",
            Category = "Security",
            Description =
                "Sets DisableAutoplayForRemovableMedia=1 under the Windows\\Explorer Group Policy key. "
                + "Suppresses all shell AutoPlay handler registrations for removable media when enforced via Group Policy. "
                + "Ensures that third-party software cannot add its own AutoPlay handler entries for USB drives.",
            Tags = ["autoplay", "shell", "handlers", "removable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Third-party AutoPlay handler registrations for removable media are blocked.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "DisableAutoplayForRemovableMedia", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "DisableAutoplayForRemovableMedia")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "DisableAutoplayForRemovableMedia", 1)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-turn-off-autoplay",
            Label = "Turn Off AutoPlay for All Media Types",
            Category = "Security",
            Description =
                "Sets DisableAutoplay=1 under the Windows\\Explorer Group Policy key — the master switch. "
                + "Completely disables the AutoPlay feature for ALL media and devices system-wide. "
                + "When this policy is applied, Windows will not process autorun.inf files or launch AutoPlay dialog handlers "
                + "regardless of device type.",
            Tags = ["autoplay", "disable", "master", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "AutoPlay completely disabled system-wide; no media or device will trigger automatic actions.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "DisableAutoplay", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "DisableAutoplay")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "DisableAutoplay", 1)],
        },
    ];
}

// ── Sprint 663 — PolicyWindowsStore ──────────────────────────────────────────
/// <summary>
/// Windows Store application deployment and installation policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\WindowsStore
/// These keys control whether users can access the Microsoft Store, install apps,
/// and how updates are distributed in managed environments.
/// </summary>
internal static class PolicyWindowsStore
{
    private const string StoreKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "storepl-policy-turn-off-store-notifications",
            Label = "Turn Off Store Notifications",
            Category = "System",
            Description =
                "Sets TurnOffStoreNotifications=1 under the WindowsStore Group Policy key. "
                + "Suppresses all notification toasts generated by the Windows Store, including app update availability, "
                + "promotional offers, and feature announcements. "
                + "Reduces notification noise in managed environments.",
            Tags = ["store", "notifications", "toasts", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Store-generated notification toasts are suppressed for all users.",
            ApplyOps = [RegOp.SetDword(StoreKey, "TurnOffStoreNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "TurnOffStoreNotifications")],
            DetectOps = [RegOp.CheckDword(StoreKey, "TurnOffStoreNotifications", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-disable-purchase",
            Label = "Disable Store App Purchases",
            Category = "System",
            Description =
                "Sets DisablePurchasePage=1 under the WindowsStore Group Policy key. "
                + "Prevents users from viewing or completing in-app purchases or paid app transactions in the Store. "
                + "Blocks the payment flow entirely, preventing accidental or unauthorised charges.",
            Tags = ["store", "purchase", "payment", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "In-app purchases and paid app transactions are blocked in the Store.",
            ApplyOps = [RegOp.SetDword(StoreKey, "DisablePurchasePage", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "DisablePurchasePage")],
            DetectOps = [RegOp.CheckDword(StoreKey, "DisablePurchasePage", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-disable-app-install",
            Label = "Block Users from Installing Apps",
            Category = "System",
            Description =
                "Sets BlockNonAdminUserInstall=1 under the WindowsStore Group Policy key. "
                + "Prevents non-administrator users from initiating app installations from any source via the Store. "
                + "Centralises app permission to administrators only.",
            Tags = ["store", "install", "non-admin", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Standard users cannot install apps; only administrators can approve and install.",
            ApplyOps = [RegOp.SetDword(StoreKey, "BlockNonAdminUserInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "BlockNonAdminUserInstall")],
            DetectOps = [RegOp.CheckDword(StoreKey, "BlockNonAdminUserInstall", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-disable-video-streaming",
            Label = "Disable Store Video Streaming",
            Category = "System",
            Description =
                "Sets DisableVideoPage=1 under the WindowsStore Group Policy key. "
                + "Hides the Video section of the Microsoft Store, preventing access to streaming video content purchasing. "
                + "Reduces bandwidth usage and removes non-productivity content from the Store in enterprise settings.",
            Tags = ["store", "video", "streaming", "policy", "bandwidth"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Store Video section hidden; streaming purchases not accessible.",
            ApplyOps = [RegOp.SetDword(StoreKey, "DisableVideoPage", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "DisableVideoPage")],
            DetectOps = [RegOp.CheckDword(StoreKey, "DisableVideoPage", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-disable-music-streaming",
            Label = "Disable Store Music Streaming",
            Category = "System",
            Description =
                "Sets DisableMusicPage=1 under the WindowsStore Group Policy key. "
                + "Hides the Music section of the Microsoft Store, removing access to Groove/Xbox Music purchasing. "
                + "Complements DisableVideoPage in locking down the Store to business-relevant apps only.",
            Tags = ["store", "music", "streaming", "policy", "bandwidth"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Store Music section hidden; streaming music purchases not accessible.",
            ApplyOps = [RegOp.SetDword(StoreKey, "DisableMusicPage", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "DisableMusicPage")],
            DetectOps = [RegOp.CheckDword(StoreKey, "DisableMusicPage", 1)],
        },
    ];
}

// ── Sprint 664 — PolicyLockScreen ────────────────────────────────────────────
/// <summary>
/// Lock Screen appearance and personalisation restriction policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization
/// These keys control whether the lock screen can be customised by users,
/// and which content is shown before authentication.
/// </summary>
internal static class PolicyLockScreen
{
    private const string PersonalKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight",
            Label = "Disable Windows Spotlight on Lock Screen",
            Category = "Security",
            Description =
                "Sets NoWindowsSpotlightOnLockScreen=1 under the Personalization Group Policy key. "
                + "Prevents Windows Spotlight from downloading and displaying rotating background images, "
                + "ads, and tips on the lock screen. "
                + "Eliminates background network traffic and removes Microsoft advertising from the pre-auth surface.",
            Tags = ["lockscreen", "spotlight", "policy", "privacy", "ads"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Windows Spotlight disabled on lock screen; no rotating images or advertising content.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightOnLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightOnLockScreen")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightOnLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-action-center",
            Label = "Disable Windows Spotlight in Action Centre",
            Category = "Security",
            Description =
                "Sets NoWindowsSpotlightOnActionCenter=1 under the Personalization Group Policy key. "
                + "Prevents Windows Spotlight suggestions and featured apps from appearing in the Action Centre panel. "
                + "Removes promotional content from the notification/action area.",
            Tags = ["lockscreen", "spotlight", "action-center", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Spotlight suggestions removed from Action Centre; notification area shows only real notifications.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightOnActionCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightOnActionCenter")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightOnActionCenter", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions on Lock Screen",
            Category = "Security",
            Description =
                "Sets NoThirdPartySuggestions=1 under the Personalization Group Policy key. "
                + "Prevents Windows from displaying app suggestions from third-party publishers on the lock screen. "
                + "Eliminates advertising and unsolicited install prompts from the sign-in surface.",
            Tags = ["lockscreen", "suggestions", "ads", "third-party", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Third-party app suggestions removed from lock screen; clean pre-auth experience.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoThirdPartySuggestions")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-windows-welcome",
            Label = "Disable Windows Welcome Experience Spotlight",
            Category = "Security",
            Description =
                "Sets NoWindowsSpotlightWindowsWelcomeExperience=1 under the Personalization Group Policy key. "
                + "Prevents the 'What's new in Windows' welcome experience from appearing after feature updates. "
                + "Stops an animated fullscreen takeover that introduces new features at the expense of user focus.",
            Tags = ["lockscreen", "spotlight", "welcome", "policy", "onboarding"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Post-update Windows Welcome Experience spotlight is suppressed.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightWindowsWelcomeExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightWindowsWelcomeExperience")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightWindowsWelcomeExperience", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-lock-screen-camera",
            Label = "Disable Camera Access from Lock Screen",
            Category = "Security",
            Description =
                "Sets NoCameraOnLockScreen=1 under the Personalization Group Policy key. "
                + "Prevents the Camera app from launching directly from the lock screen. "
                + "Eliminates a potential avenue for taking photos or accessing media without authenticating first.",
            Tags = ["lockscreen", "camera", "policy", "security", "access"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Camera cannot be launched from lock screen; full authentication required to access camera.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoCameraOnLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoCameraOnLockScreen")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoCameraOnLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-features",
            Label = "Turn Off All Spotlight Features",
            Category = "Security",
            Description =
                "Sets ConfigureWindowsSpotlight=2 under the Personalization Group Policy key. "
                + "Value 2 applies the most restrictive Spotlight policy: disabled entirely. "
                + "This replaces all per-feature Spotlight toggles with a single master-off switch for Group Policy compliance.",
            Tags = ["lockscreen", "spotlight", "master", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "All Windows Spotlight feature categories disabled via a single master policy value.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "ConfigureWindowsSpotlight", 2)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "ConfigureWindowsSpotlight")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "ConfigureWindowsSpotlight", 2)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-taskbar",
            Label = "Disable Spotlight Suggestions in Taskbar Search",
            Category = "Security",
            Description =
                "Sets NoWindowsSpotlightInSearch=1 under the Personalization Group Policy key. "
                + "Removes Microsoft-curated Spotlight content suggestions from appearing in the Windows Search bar on the taskbar. "
                + "Search results show only local and Bing content, not Spotlight-injected promotions.",
            Tags = ["lockscreen", "spotlight", "search", "taskbar", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Spotlight suggestions removed from taskbar Search; search shows only query results.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightInSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightInSearch")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightInSearch", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-settings",
            Label = "Disable Spotlight Tips in Settings",
            Category = "Security",
            Description =
                "Sets NoWindowsSpotlightInSettings=1 under the Personalization Group Policy key. "
                + "Removes the Windows Spotlight-powered tips and feature highlights that appear throughout the Settings app. "
                + "Reduces noise from Microsoft suggestions inside Settings pages.",
            Tags = ["lockscreen", "spotlight", "settings", "tips", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Spotlight tips and suggestions removed from all Settings pages.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightInSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightInSettings")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightInSettings", 1)],
        },
    ];
}

// ── Sprint 665 — PolicyRemoteAssistance ──────────────────────────────────────
/// <summary>
/// Remote Assistance security and behavioural Group Policy controls (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\RemoteAssistance
/// These keys govern Windows Remote Assistance (as distinct from RDP), including
/// solicitation controls, session duration, and ticket lifetime.
/// </summary>
internal static class PolicyRemoteAssistance
{
    private const string RemAssist = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";
    private const string RemAssistRA = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAssistance";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "remassist-policy-disable-offer-ra",
            Label = "Disable Offer Remote Assistance",
            Category = "Remote Desktop",
            Description =
                "Sets fAllowUnsolicited=0 under the Terminal Services Group Policy key. "
                + "Prevents helpers from offering unsolicited Remote Assistance sessions. "
                + "Disables the 'Offer RA' variant where a helper can push a connection without the user initiating a request.",
            Tags = ["remote-assistance", "offer", "unsolicited", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Unsolicited Remote Assistance offers blocked; only user-initiated requests allowed.",
            ApplyOps = [RegOp.SetDword(RemAssist, "fAllowUnsolicited", 0)],
            RemoveOps = [RegOp.DeleteValue(RemAssist, "fAllowUnsolicited")],
            DetectOps = [RegOp.CheckDword(RemAssist, "fAllowUnsolicited", 0)],
        },
        new TweakDef
        {
            Id = "remassist-policy-require-explicit-prompt",
            Label = "Require Explicit User Consent for RA Control",
            Category = "Remote Desktop",
            Description =
                "Sets fEnableFullControl=0 under the Terminal Services Group Policy key. "
                + "Restricts incoming Remote Assistance sessions to view-only mode; the user must grant explicit "
                + "permission before the helper can take mouse and keyboard control. "
                + "Prevents silent takeover of user sessions.",
            Tags = ["remote-assistance", "consent", "control", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "RA helper can view but not control the session until the user approves.",
            ApplyOps = [RegOp.SetDword(RemAssist, "fEnableFullControl", 0)],
            RemoveOps = [RegOp.DeleteValue(RemAssist, "fEnableFullControl")],
            DetectOps = [RegOp.CheckDword(RemAssist, "fEnableFullControl", 0)],
        },
        new TweakDef
        {
            Id = "remassist-policy-max-ticket-expiry-hours",
            Label = "Limit Remote Assistance Ticket Validity to 1 Hour",
            Category = "Remote Desktop",
            Description =
                "Sets MaxTicketExpiryUnits=1 and MaxTicketExpiry=1 under the RemoteAssistance Policy key. "
                + "Limits invitation ticket validity to 1 hour. "
                + "Shortens the window during which an RA invitation can be acted upon, reducing exposure.",
            Tags = ["remote-assistance", "ticket", "expiry", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "RA invitation tickets expire within 1 hour; stale invitations cannot be reused.",
            ApplyOps =
            [
                RegOp.SetDword(RemAssistRA, "MaxTicketExpiryUnits", 1),
                RegOp.SetDword(RemAssistRA, "MaxTicketExpiry", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(RemAssistRA, "MaxTicketExpiryUnits"),
                RegOp.DeleteValue(RemAssistRA, "MaxTicketExpiry"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(RemAssistRA, "MaxTicketExpiryUnits", 1),
                RegOp.CheckDword(RemAssistRA, "MaxTicketExpiry", 1),
            ],
        },
        new TweakDef
        {
            Id = "remassist-policy-require-bandwidth-limit",
            Label = "Set Remote Assistance Bandwidth Limit",
            Category = "Remote Desktop",
            Description =
                "Sets MaxAllowedBandwidth=2 under the RemoteAssistance Policy key. "
                + "Caps bandwidth consumed by a Remote Assistance session (value 2 = 2 Mbps maximum). "
                + "Prevents Remote Assistance from saturating network links during active sessions.",
            Tags = ["remote-assistance", "bandwidth", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Remote Assistance bandwidth capped at 2 Mbps; network impact during sessions is bounded.",
            ApplyOps = [RegOp.SetDword(RemAssistRA, "MaxAllowedBandwidth", 2)],
            RemoveOps = [RegOp.DeleteValue(RemAssistRA, "MaxAllowedBandwidth")],
            DetectOps = [RegOp.CheckDword(RemAssistRA, "MaxAllowedBandwidth", 2)],
        },
        new TweakDef
        {
            Id = "remassist-policy-disable-email-tickets",
            Label = "Disable Email Invitation Tickets for RA",
            Category = "Remote Desktop",
            Description =
                "Sets fAllowEmailInvitations=0 under the Terminal Services Group Policy key. "
                + "Prevents users from creating Remote Assistance invitation tickets delivered by email. "
                + "Email-based RA tickets can be forwarded, intercepted, or expire without notice; disabling this channel is a security best practice.",
            Tags = ["remote-assistance", "email", "invitation", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Email-based Remote Assistance invitations disabled; only Easy Connect or file-based invites allowed.",
            ApplyOps = [RegOp.SetDword(RemAssist, "fAllowEmailInvitations", 0)],
            RemoveOps = [RegOp.DeleteValue(RemAssist, "fAllowEmailInvitations")],
            DetectOps = [RegOp.CheckDword(RemAssist, "fAllowEmailInvitations", 0)],
        },
        new TweakDef
        {
            Id = "remassist-policy-disable-easy-connect",
            Label = "Disable Easy Connect Remote Assistance",
            Category = "Remote Desktop",
            Description =
                "Sets fAllowEasyConnect=0 under the RemoteAssistance Policy key. "
                + "Disables the 'Easy Connect' Remote Assistance method which uses the Peer Name Resolution Protocol "
                + "(PNRP) cloud service instead of a ticket file. "
                + "Easy Connect depends on external cloud infrastructure; disabling it constrains RA to local network or ticket methods.",
            Tags = ["remote-assistance", "easy-connect", "pnrp", "cloud", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Easy Connect cloud-based RA method disabled; ticket-file method remains available.",
            ApplyOps = [RegOp.SetDword(RemAssistRA, "fAllowEasyConnect", 0)],
            RemoveOps = [RegOp.DeleteValue(RemAssistRA, "fAllowEasyConnect")],
            DetectOps = [RegOp.CheckDword(RemAssistRA, "fAllowEasyConnect", 0)],
        },
        new TweakDef
        {
            Id = "remassist-policy-log-sessions",
            Label = "Enable Remote Assistance Session Logging",
            Category = "Remote Desktop",
            Description =
                "Sets EnableRASSessionAudit=1 under the RemoteAssistance Policy key. "
                + "Enables audit logging of Remote Assistance connection events. "
                + "Records who connected, when, and for how long — supports compliance and incident investigation.",
            Tags = ["remote-assistance", "logging", "audit", "policy", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "RA session events logged to the Windows event log for audit purposes.",
            ApplyOps = [RegOp.SetDword(RemAssistRA, "EnableRASSessionAudit", 1)],
            RemoveOps = [RegOp.DeleteValue(RemAssistRA, "EnableRASSessionAudit")],
            DetectOps = [RegOp.CheckDword(RemAssistRA, "EnableRASSessionAudit", 1)],
        },
        new TweakDef
        {
            Id = "remassist-policy-disable-file-transfer",
            Label = "Disable File Transfer During RA Sessions",
            Category = "Remote Desktop",
            Description =
                "Sets fDisableExclamation=1 under the Terminal Services Group Policy key. "
                + "Disables the file-transfer feature in Remote Assistance sessions. "
                + "Prevents the helper from sending or receiving files during a session, "
                + "blocking a common data exfiltration or dropper-delivery path.",
            Tags = ["remote-assistance", "file-transfer", "exfiltration", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "File transfer blocked during RA sessions; helpers cannot upload or download files.",
            ApplyOps = [RegOp.SetDword(RemAssist, "fDisableExclamation", 1)],
            RemoveOps = [RegOp.DeleteValue(RemAssist, "fDisableExclamation")],
            DetectOps = [RegOp.CheckDword(RemAssist, "fDisableExclamation", 1)],
        },
    ];
}

// ── Sprint 666 — PolicySmartCard ──────────────────────────────────────────────
/// <summary>
/// Smart Card authentication and credential provider Group Policy controls (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\System
/// These keys govern Smart Card logon behaviour, PIN policies, and forced card removal.
/// </summary>
internal static class PolicySmartCard
{
    private const string ScCredProv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";
    private const string WinSystem = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "smartcard-policy-force-logoff-on-removal",
            Label = "Force Logoff on Smart Card Removal",
            Category = "Security",
            Description =
                "Sets scremoveoption=2 under the SmartCardCredentialProvider Group Policy key. "
                + "Value 2 causes a full sign-out (rather than a lock) when the smart card is removed. "
                + "More aggressive than the lock option — suitable for high-security shared terminal environments.",
            Tags = ["smartcard", "logoff", "removal", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "User is immediately logged off when card is removed; unsaved work may be lost.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "scremoveoption", 2)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "scremoveoption")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "scremoveoption", 2)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-allow-integrated-unblock",
            Label = "Allow Integrated Unblock Screen for Smart Card PIN",
            Category = "Security",
            Description =
                "Sets AllowIntegratedUnblock=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Enables a built-in PIN unlock screen that appears on the credential provider for blocked smart cards, "
                + "removing the need for a separate help-desk intervention to reset a locked card.",
            Tags = ["smartcard", "pin", "unblock", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Users can self-service unblock a PIN-locked smart card via the Windows credential screen.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "AllowIntegratedUnblock", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "AllowIntegratedUnblock")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "AllowIntegratedUnblock", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-turn-on-virtual-card",
            Label = "Enable Virtual Smart Card Creation",
            Category = "Security",
            Description =
                "Sets AllowDomainPINLogon=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Allows BitLocker Network Unlock and domain accounts to authenticate with a PIN against a virtual TPM smart card. "
                + "Enables software-based smart card equivalent for devices without physical card readers.",
            Tags = ["smartcard", "virtual", "tpm", "pin", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Virtual smart card PIN logon enabled; TPM-backed credential usable without a physical card reader.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "AllowDomainPINLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "AllowDomainPINLogon")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "AllowDomainPINLogon", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-enable-certificate-propagation",
            Label = "Enable Smart Card Certificate Propagation",
            Category = "Security",
            Description =
                "Sets CertPropEnabled=1 under the Windows\\System Group Policy key. "
                + "Enables automatic propagation of smart card certificates from the card to the user's certificate store "
                + "when the card is inserted. "
                + "Required for applications that directly query the user certificate store rather than the card reader.",
            Tags = ["smartcard", "certificate", "propagation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Smart card certificates automatically appear in the user certificate store on card insert.",
            ApplyOps = [RegOp.SetDword(WinSystem, "CertPropEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WinSystem, "CertPropEnabled")],
            DetectOps = [RegOp.CheckDword(WinSystem, "CertPropEnabled", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-enable-cleanup-certificates",
            Label = "Clean Up Smart Card Certificates on Card Removal",
            Category = "Security",
            Description =
                "Sets CleanupCerts=1 under the Windows\\System Group Policy key. "
                + "Removes smart card certificates from the user store when the card is removed. "
                + "Works in conjunction with CertPropEnabled to maintain a consistent certificate state "
                + "reflecting only currently-inserted cards.",
            Tags = ["smartcard", "certificate", "cleanup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Propagated certificates removed from user store when smart card is removed.",
            ApplyOps = [RegOp.SetDword(WinSystem, "CleanupCerts", 1)],
            RemoveOps = [RegOp.DeleteValue(WinSystem, "CleanupCerts")],
            DetectOps = [RegOp.CheckDword(WinSystem, "CleanupCerts", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-turn-off-root-auto-update",
            Label = "Prevent Smart Card Root Certificate Auto-Update",
            Category = "Security",
            Description =
                "Sets RootCertificateAutoUpdate=0 under the SmartCardCredentialProvider key. "
                + "Prevents Windows from automatically downloading and updating root certificates from Windows Update "
                + "for smart card trust anchors. "
                + "Appropriate in air-gapped or strictly controlled environments where certificate trust is managed manually.",
            Tags = ["smartcard", "certificate", "root", "update", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Root certificate auto-update for smart cards disabled; manual CA trust management required.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "RootCertificateAutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "RootCertificateAutoUpdate")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "RootCertificateAutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-disable-pinpad-logon",
            Label = "Disable PIN Pad Bypass for Smart Card Logon",
            Category = "Security",
            Description =
                "Sets DisallowPINLessLogon=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Requires a PIN to be entered for every smart card logon, even if the card supports and is configured for PINless logon. "
                + "Ensures that PIN knowledge (something-you-know) combined with card possession (something-you-have) is always required.",
            Tags = ["smartcard", "pin", "logon", "policy", "mfa", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "PINless logon blocked; card + PIN is always required for true two-factor authentication.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "DisallowPINLessLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "DisallowPINLessLogon")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "DisallowPINLessLogon", 1)],
        },
    ];
}

internal static class PolicyEnterprise
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _AdfsFederationPolicy.Data,
            .. _ActiveDirectoryServicesPolicy.Data,
            .. _AdReplicationPolicy.Data,
            .. _AzureVirtualDesktopPolicy.Data,
            .. _CloudPcWindows365Policy.Data,
            .. _ConfigurationManagerPolicy.Data,
            .. _DeploymentServicesPolicy.Data,
            .. _DomainControllerHardeningPolicy.Data,
            .. _DomainIsolationPolicy.Data,
            .. _DomainTrustPolicy.Data,
            .. _EasMdmPolicy.Data,
            .. _EnterpriseDeviceManagementPolicy.Data,
            .. _EnterpriseResourceDeployPolicy.Data,
            .. _EnterpriseResourcePolicy.Data,
            .. _EnterpriseStateRoamingPolicy.Data,
            .. _GpoFolderRedirPolicy.Data,
            .. _GpoScriptsPolicy.Data,
            .. _GroupPolicySettingsPolicy.Data,
            .. _HotpatchUpdatePolicy.Data,
            .. _HybridJoinDnsPolicy.Data,
            .. _IntuneDeviceEventPolicy.Data,
            .. _MdmEnrollmentPolicy.Data,
            .. _MdmRegistrationPolicy.Data,
            .. _OobePolicy.Data,
            .. _RetailDemoPolicy.Data,
            .. _SharedPCPolicy.Data,
            .. _WindowsAutopilotPolicy.Data,
            .. _WindowsDeploymentServicesPolicy.Data,
            .. _WindowsFlightedFeaturesPolicy.Data,
            .. _WindowsFlightingPolicy.Data,
            .. _WindowsInsider.Data,
            .. _WindowsServicingPolicy.Data,
            .. _WindowsToGoPolicy.Data,
        ];

    // ── AdfsFederationPolicy ──
    private static class _AdfsFederationPolicy
    {
        private const string AdfsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ADFS";
        private const string AuthKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Authentication";
        private const string SvcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\adfssrv\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "adfspol-enable-extranet-lockout",
                    Label = "Enable ADFS Extranet Smart Lockout",
                    Category = "System",
                    Description =
                        "Sets EnableExtranetLockout=1 in the ADFS policy. Activates ADFS Extranet Smart Lockout (ESL) which tracks authentication attempts from extranet (external) IP addresses separately from intranet ones. Extranet lockout prevents password spray and brute-force attacks from the internet from locking out Active Directory accounts while still allowing internal users to authenticate normally.",
                    Tags = ["adfs", "extranet", "lockout", "brute-force", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Activates smart lockout for extranet auth; requires ADFS to be deployed with WAPProxy for effect.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "EnableExtranetLockout", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "EnableExtranetLockout")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "EnableExtranetLockout", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-set-extranet-lockout-threshold",
                    Label = "Set ADFS Extranet Lockout Threshold (5 attempts)",
                    Category = "System",
                    Description =
                        "Sets ExtranetLockoutThreshold=5 in the ADFS policy. Defines the number of failed authentication attempts from an extranet IP address before ADFS blocks further attempts from that IP. Five failed attempts is the CIS recommendation that balances security against accidental account lockout from mistyped passwords on shared IP networks (NAT, VPN exit nodes).",
                    Tags = ["adfs", "extranet", "lockout", "threshold", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks extranet IPs after 5 failures; corporate NAT exit nodes may need threshold adjustment.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "ExtranetLockoutThreshold", 5)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "ExtranetLockoutThreshold")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "ExtranetLockoutThreshold", 5)],
                },
                new TweakDef
                {
                    Id = "adfspol-disable-endpoint-wia-fallback",
                    Label = "Disable ADFS Windows Integrated Auth Fallback",
                    Category = "System",
                    Description =
                        "Sets DisableWIAFallback=1 in the ADFS policy. Prevents ADFS from falling back to Windows Integrated Authentication (Kerberos/NTLM from browser) when the primary authentication method fails. WIA fallback can expose NTLM credentials when users authenticate from non-domain-joined browsers, potentially enabling NTLM relay attacks. Disabling fallback forces explicit form-based or certificate authentication.",
                    Tags = ["adfs", "wia", "fallback", "ntlm", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Prevents WIA fallback; Intranet users who previously used WIA from non-domain browsers must use forms instead.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "DisableWIAFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "DisableWIAFallback")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "DisableWIAFallback", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-require-ssl-certificate-auth",
                    Label = "Require TLS Certificate Authentication for ADFS Service",
                    Category = "System",
                    Description =
                        "Sets RequireCertificateAuthentication=1 in the ADFS service Parameters key. Enforces mutual TLS certificate authentication for ADFS service account communication. When mutual TLS is required the ADFS service will reject connections from components (proxy servers, relying party trusts) that do not present a valid certificate, preventing impersonation of trusted federation endpoints.",
                    Tags = ["adfs", "tls", "certificate", "mutual-auth", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Requires mutual TLS from all ADFS-connecting components; ensure proxies and RPs have valid certificates.",
                    ApplyOps = [RegOp.SetDword(SvcKey, "RequireCertificateAuthentication", 1)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "RequireCertificateAuthentication")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "RequireCertificateAuthentication", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-enable-oauth-pkce",
                    Label = "Require PKCE for ADFS OAuth2 Authorization Code Flow",
                    Category = "System",
                    Description =
                        "Sets RequirePKCEForOAuth=1 in the ADFS policy. Enforces Proof Key for Code Exchange (PKCE, RFC 7636) for all OAuth 2.0 authorization code flow requests to ADFS. PKCE prevents authorization code interception attacks where an attacker intercepts the authorization code redirect and exchanges it for tokens. Required by RFC 9700 (OAuth 2.0 Security Best Current Practice) for all public and confidential clients.",
                    Tags = ["adfs", "oauth", "pkce", "authorization-code", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Requires PKCE; legacy OAuth clients that do not send a code_challenge will be rejected.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "RequirePKCEForOAuth", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "RequirePKCEForOAuth")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "RequirePKCEForOAuth", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-disable-device-auth-bypass",
                    Label = "Disable ADFS Device Authentication Bypass",
                    Category = "System",
                    Description =
                        "Sets DisableDeviceAuthenticationBypass=1 in the ADFS policy. Prevents ADFS from bypassing multi-factor authentication requirements based solely on device registration status. When disabled, a registered device alone is not sufficient to skip MFA — users must still satisfy the full authentication policy. This closes a gap where attackers who enroll a stolen device could bypass step-up authentication.",
                    Tags = ["adfs", "device-auth", "mfa", "bypass", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Device registration no longer bypasses MFA; compliant device policies may need adjustment for Conditional Access.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "DisableDeviceAuthenticationBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "DisableDeviceAuthenticationBypass")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "DisableDeviceAuthenticationBypass", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-set-token-replay-detection",
                    Label = "Enable ADFS Token Replay Detection",
                    Category = "System",
                    Description =
                        "Sets EnableTokenReplayDetection=1 in the ADFS policy. Activates the ADFS token replay detection cache which records recently used security tokens and rejects any attempt to present the same token a second time. Token replay attacks occur when an attacker intercepts a SAML assertion or JWT and submits it to gain access. Detection is critical for federated SSO scenarios where tokens flow through multiple network intermediaries.",
                    Tags = ["adfs", "token-replay", "detection", "saml", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Enables token replay cache; negligible performance impact on ADFS server under normal SSO load.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "EnableTokenReplayDetection", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "EnableTokenReplayDetection")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "EnableTokenReplayDetection", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-require-extended-protection",
                    Label = "Require Extended Protection for ADFS Authentication",
                    Category = "System",
                    Description =
                        "Sets EnableExtendedProtection=1 in the ADFS authentication policy. Enables Extended Protection for Authentication (EPA) which binds the Windows authentication handshake to the TLS channel. EPA prevents NTLM relay attacks where an attacker forwards authentication attempts to the ADFS endpoint from a man-in-the-middle position. Supported in all Windows versions since Windows 7 SP1.",
                    Tags = ["adfs", "extended-protection", "ntlm-relay", "authentication", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Requires EPA TLS channel binding; clients that do not support EPA (pre-Vista) will fail WIA authentication.",
                    ApplyOps = [RegOp.SetDword(AuthKey, "EnableExtendedProtection", 1)],
                    RemoveOps = [RegOp.DeleteValue(AuthKey, "EnableExtendedProtection")],
                    DetectOps = [RegOp.CheckDword(AuthKey, "EnableExtendedProtection", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-disable-prompt-login",
                    Label = "Disable ADFS Prompt=Login Re-Authentication Bypass",
                    Category = "System",
                    Description =
                        "Sets DisablePromptLoginHandling=1 in the ADFS policy. Prevents ADFS from honouring the OAuth/OIDC prompt=login parameter which forces a fresh login regardless of existing SSO session. While useful for applications needing fresh credentials, this parameter can be abused by attackers to force users into repeated phishing-susceptible login flows. Disabling allows ADFS to enforce its own session management instead.",
                    Tags = ["adfs", "oauth", "prompt-login", "session", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Ignores prompt=login; applications that need forced re-auth must use access token expiry instead.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "DisablePromptLoginHandling", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "DisablePromptLoginHandling")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "DisablePromptLoginHandling", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-enable-audit-events",
                    Label = "Enable ADFS Security Audit Events",
                    Category = "System",
                    Description =
                        "Sets AuditFlags=1 in the ADFS policy. Instructs ADFS to write security audit events to the Windows Security event log for all federation authentication requests, token issuances, and extranet lockout events. ADFS audit events (Event IDs 1200, 1201, 411, 412) are essential for detecting password spray attacks, compromised account usage, and abnormal token issuance patterns in a federated identity environment.",
                    Tags = ["adfs", "audit", "events", "security-log", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enables ADFS audit events in the Security log; increases log volume proportional to federation traffic.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "AuditFlags", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "AuditFlags")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "AuditFlags", 1)],
                },
            ];
    }

    // ── ActiveDirectoryServicesPolicy ──
    private static class _ActiveDirectoryServicesPolicy
    {
        private const string AdPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        private const string NetlogonKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "addsvc-restrict-ntlm-outbound-to-trusted-servers",
                    Label = "AD Services: Restrict Outbound NTLM to Domain-Trusted Servers Only",
                    Category = "System",
                    Description =
                        "Sets RestrictSendingNTLMTraffic=2 in the System policy (value 2 = Deny All, value 1 = Audit, value 0 = Allow). Restricts outbound NTLM authentication to only servers in the trusted exception list. NTLM credentials can be captured by rogue SMB or HTTP servers (e.g., via LLMNR/NBT-NS poisoning with Responder) — any outbound NTLM challenge-response that reaches an attacker's server provides an NTLM hash that can be relayed or cracked. Denying outbound NTLM to non-trusted-listed servers prevents credential leakage via NTLM to attacker-controlled resources. Start with value 1 (Audit) to identify NTLM usage before enforcing value 2.",
                    Tags = ["ntlm", "outbound-restriction", "responder", "ntlm-relay", "credential-capture"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Outbound NTLM is denied for all servers not in the NTLM exception list. This is high-impact — applications and services that use NTLM for server connections (file shares, proxy auth, legacy web apps) will fail unless added to the exception list. Deploy first with value 1 (Audit) and examine NTLM audit events (Event ID 4011/4013) to map usage before enforcing value 2.",
                    ApplyOps = [RegOp.SetDword(AdPolicyKey, "RestrictSendingNTLMTraffic", 2)],
                    RemoveOps = [RegOp.DeleteValue(AdPolicyKey, "RestrictSendingNTLMTraffic")],
                    DetectOps = [RegOp.CheckDword(AdPolicyKey, "RestrictSendingNTLMTraffic", 2)],
                },
                new TweakDef
                {
                    Id = "addsvc-enable-ntlm-audit-logging",
                    Label = "AD Services: Enable NTLM Outbound Authentication Audit Logging",
                    Category = "System",
                    Description =
                        "Sets AuditReceivingNTLMTraffic=2 in the System policy (value 2 = Enable auditing for all NTLM authentication). Enables auditing of all outbound NTLM authentication requests from this client. Audited events appear in the Security event log (Event ID 8001/8002/8003) and include the destination server, the NTLM authentication type, and the caller process. This is essential for mapping NTLM usage before deploying NTLM restriction policies — it allows the security team to identify which applications, services, and users are using NTLM so equivalent Kerberos or modern authentication alternatives can be configured before NTLM is denied.",
                    Tags = ["ntlm", "audit", "event-log", "siem", "authentication-map"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "All outbound NTLM authentications are logged to the Security event log. Generates events for every NTLM use — in Active Directory environments this may include Windows file share browsing, SMB connections, etc. Integrate with SIEM. Event volume may be high in environments with heavy NTLM usage.",
                    ApplyOps = [RegOp.SetDword(AdPolicyKey, "AuditReceivingNTLMTraffic", 2)],
                    RemoveOps = [RegOp.DeleteValue(AdPolicyKey, "AuditReceivingNTLMTraffic")],
                    DetectOps = [RegOp.CheckDword(AdPolicyKey, "AuditReceivingNTLMTraffic", 2)],
                },
                new TweakDef
                {
                    Id = "addsvc-require-ldap-server-integrity",
                    Label = "AD Services: Require DC-Side LDAP Server Signing (Integrity Check)",
                    Category = "System",
                    Description =
                        "Sets LDAPServerIntegrity=2 in the Netlogon\\Parameters hive (value 2 = Require signing). Requires LDAP signing on LDAP connections from clients to domain controllers. This is the server-side complement to the LDAP client signing requirement (LDAPClientIntegrity). When both client and server require signing, LDAP relay attacks that attempt to intercept and modify LDAP traffic are blocked at both endpoints. Without the server-side requirement, an attacker could spoof a DC with an unsigned LDAP server even if client policy sends signed requests.",
                    Tags = ["ldap", "server-signing", "ldap-relay", "netlogon", "integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "DC-side LDAP signing required as well as client-side. All LDAP connections from this client to DCs must use signed sessions — both sides enforce it. LDAP clients that send unsigned LDAP requests will receive an LDAP_UNWILLING_TO_PERFORM error. Applies to all LDAP on port 389; LDAPS on 636 is unaffected (TLS provides equivalent protection).",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "LDAPServerIntegrity", 2)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "LDAPServerIntegrity")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "LDAPServerIntegrity", 2)],
                },
                new TweakDef
                {
                    Id = "addsvc-set-dc-locator-force-rediscovery-600",
                    Label = "AD Services: Set DC Locator Force Re-Discovery Period to 600 Seconds",
                    Category = "System",
                    Description =
                        "Sets ForceRediscoveryInterval=600 in Netlogon\\Parameters (units: seconds). Sets the minimum interval between forced DC re-discovery events to 600 seconds (10 minutes). DC locator caches the preferred domain controller for each domain to avoid repeated DC lookup traffic. If the preferred DC becomes unavailable (patched, restarted, or taken down), the client should re-discover a DC within a reasonable time. Setting 600 seconds ensures that clients do not hold stale DC references for longer than 10 minutes when a DC failure event occurs, reducing authentication outage windows during DC failover events.",
                    Tags = ["netlogon", "dc-locator", "failover", "rediscovery", "resilience"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "DC preference re-discovery forced every 600 seconds maximum. Clients will check for a better DC up to every 10 minutes. May generate additional Netlogon DC locator traffic in large environments with many workstations. Appropriate for standard enterprise environments with 2+ DCs per site.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "ForceRediscoveryInterval", 600)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "ForceRediscoveryInterval")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "ForceRediscoveryInterval", 600)],
                },
            ];
    }

    // ── AdReplicationPolicy ──
    private static class _AdReplicationPolicy
    {
        private const string NtdsPolicyKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NTDS\Parameters";

        private const string SystemPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "adrep-require-ntds-replication-sign-seal",
                    Label = "AD Replication: Require NTDS Replication Traffic Sign-and-Seal",
                    Category = "System",
                    Description =
                        "Sets ReplicationSignAndSeal=1 in NTDS\\Parameters. Requires that Active Directory replication traffic between domain controllers is both signed (integrity-protected) and sealed (encrypted). AD replication carries all directory changes — new accounts, password updates, group membership changes, and computer policy settings. If replication traffic is unprotected, an attacker who can perform a man-in-the-middle attack on DC-to-DC traffic can inject or modify replication data, potentially escalating privileges by injecting account changes. Sign-and-seal ensures all replication traffic is authenticated and encrypted.",
                    Tags = ["ntds", "replication", "sign-seal", "dc-to-dc", "encryption"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "NTDS replication traffic requires sign-and-seal. Additional CPU overhead for encryption on DCs; negligible for modern hardware. Old DCs (pre-Windows 2000) cannot participate in signed/sealed replication — only relevant for very old mixed-mode domains.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "ReplicationSignAndSeal", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "ReplicationSignAndSeal")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "ReplicationSignAndSeal", 1)],
                },
                new TweakDef
                {
                    Id = "adrep-set-tomb-stone-lifetime-180days",
                    Label = "AD Replication: Set Active Directory Tombstone Lifetime to 180 Days",
                    Category = "System",
                    Description =
                        "Sets TombstoneLifetime=180 in NTDS\\Parameters (units: days). Sets the AD tombstone lifetime to 180 days. When an object is deleted in AD, it becomes a tombstone — a marker that propagates the deletion to all DCs before the tombstone is permanently removed. If a DC is offline longer than the tombstone lifetime, it must be forcibly re-joined to the domain (a USN rollback scenario) or reinstalled. 60 days (the old default) is insufficient for quarterly disaster recovery testing cycles. 180 days ensures that DCs recovered from quarterly backup snapshots are still within the tombstone window and can be safely re-brought online without forced rejoin.",
                    Tags = ["ntds", "tombstone", "backup-recovery", "deleted-objects", "replication"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Tombstone lifetime extended to 180 days. Deleted AD objects remain as tombstones for 180 days before permanent removal. Domain controllers that have been offline for more than 180 days will require forced rejoin. Increases the size of the deleted-objects container in the AD database.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "TombstoneLifetime", 180)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "TombstoneLifetime")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "TombstoneLifetime", 180)],
                },
                new TweakDef
                {
                    Id = "adrep-enable-strict-replication-consistency",
                    Label = "AD Replication: Enable Strict Replication Consistency Mode",
                    Category = "System",
                    Description =
                        "Sets Strict Replication Consistency=1 in NTDS\\Parameters. Enables strict replication consistency, which causes NTDS to disable replication from a replication partner that has an out-of-date replication topology (i.e., has missed more than MaxConsistencyCheckPercent of updates). Without strict consistency, AD will attempt to 'loose' replicate with lagged partners even if that results in duplicate GUID conflicts or lingering objects. In lingering object scenarios (DCs that have been offline past the tombstone lifetime), strict mode prevents corrupted data from being silently re-introduced into the directory by an out-of-date DC.",
                    Tags = ["ntds", "replication-consistency", "lingering-objects", "strict-mode", "integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Strict replication consistency enforced. DCs with excessive replication lag will have replication blocked until the issue is resolved. May generate replication errors (Event ID 1388) in environments with intermittent DC connectivity. Monitor NTDS events after enabling — address any replication lag issues before enforcing.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "Strict Replication Consistency", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "Strict Replication Consistency")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "Strict Replication Consistency", 1)],
                },
                new TweakDef
                {
                    Id = "adrep-enable-dns-consistency-check",
                    Label = "AD Replication: Enable DNS Consistency Check During Promotion",
                    Category = "System",
                    Description =
                        "Sets DnsAvoidRegisterRecords=0 in NTDS\\Parameters. Ensures that all required DNS records for the domain controller are registered during or after promotion, and that the DNS consistency check is not bypassed. DC promotion attempts with unresolvable DNS names or misconfigured DNS zones that bypass the DNS check can result in DCs that are partially functional but not properly reachable by other DCs — leading to intermittent replication failures that are hard to diagnose. Ensuring DNS consistency is enforced catches DNS misconfigurations at promotion time rather than as production replication failures.",
                    Tags = ["ntds", "dns", "consistency-check", "promotion", "dc-registration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "DNS record registration for this DC is enabled (DnsAvoidRegisterRecords=0). DC registers all required DNS SRV and A records. In split-DNS environments, verify the DC can register records in both internal and external DNS zones as appropriate.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "DnsAvoidRegisterRecords", 0)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "DnsAvoidRegisterRecords")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "DnsAvoidRegisterRecords", 0)],
                },
                new TweakDef
                {
                    Id = "adrep-restrict-ad-single-object-recovery",
                    Label = "AD Replication: Enable AD Recycle Bin (Prevent Immediate Object Purge)",
                    Category = "System",
                    Description =
                        "Sets EnabledScopes=1 in NTDS\\Parameters. Enables the Active Directory Recycle Bin feature flag on this DC. The AD Recycle Bin preserves deleted objects (with all attributes including group memberships) for the deleted-object lifetime (default 180 days), making it possible to restore accidentally deleted user accounts, OUs, or groups without authoritative restore from backup. Without the Recycle Bin, deleted objects immediately lose most attributes and recovery requires authoritative NTDS restore or backup-based object recovery. This is a forest-level feature that must be enabled via PowerShell on the Schema Master (Enable-ADOptionalFeature) — this policy flag enables the local DC to participate.",
                    Tags = ["ntds", "recycle-bin", "object-recovery", "deleted-objects", "resilience"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Local DC participates in AD Recycle Bin. Requires the AD Recycle Bin optional feature to be enabled at the forest level first (Enable-ADOptionalFeature on Schema Master). Once enabled, cannot be reversed without forest recovery. Increases NTDS.dit database size due to full attribute retention for deleted objects.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "EnabledScopes", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "EnabledScopes")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "EnabledScopes", 1)],
                },
                new TweakDef
                {
                    Id = "adrep-set-max-replication-failures-5",
                    Label = "AD Replication: Alert on More Than 5 Consecutive Replication Failures",
                    Category = "System",
                    Description =
                        "Sets MaxConsistencyCheckPercent=5 in NTDS\\Parameters. Sets the threshold at which consecutive replication failures from a partner trigger a consistency check alert to 5 failures. By default, NTDS tolerates a high number of consecutive replication failures before logging a critical event or taking action. Setting a lower threshold ensures that replication health degradation is detected and reported early — critical for catching incidents where an attacker disrupts replication to prevent domain-wide propagation of security policy changes or account lockouts.",
                    Tags = ["ntds", "replication-failure", "alerting", "consistency", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Replication failure alerting triggers after 5 consecutive failures from a single replication partner. Event ID 1308 logged. In environments with intermittent network connectivity between sites, this threshold may generate false-positive events. Monitor and tune based on the expected replication reliability in your site-link topology.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "MaxConsistencyCheckPercent", 5)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "MaxConsistencyCheckPercent")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "MaxConsistencyCheckPercent", 5)],
                },
                new TweakDef
                {
                    Id = "adrep-enable-ad-audit-log-policy-access",
                    Label = "AD Replication: Enable Audit of AD DS Access Policy Operations",
                    Category = "System",
                    Description =
                        "Sets AuditPolicySubcategory=1 in NTDS\\Parameters. Enables auditing of Active Directory Service access subcategory events. These events include access to sensitive AD objects (krbtgt account reads, Domain Admins group modifications, Schema changes, replication metadata access), NTDS database file access, and NTDS parameter changes. Without this audit, an attacker who accesses sensitive AD objects (e.g., DCSync — requesting replication metadata from a DC to extract all password hashes) leaves no event log trail. With audit enabled, DCSync attempts generate replication audit events (EventID 4662, 4928) that can be detected by SIEM.",
                    Tags = ["ntds", "audit", "dcsync", "replication-access", "event-4662"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "AD DS access auditing enabled. DCSync-like replication requests (DS-Replication-Get-Changes-All) will generate Event ID 4662 in the Security log with GUID of the GUID accessed. SOC can detect DCSync attacks in real-time. May generate significant event log volume in large forests with active AD replication.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "AuditPolicySubcategory", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "AuditPolicySubcategory")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "AuditPolicySubcategory", 1)],
                },
                new TweakDef
                {
                    Id = "adrep-enable-ntds-encrypted-communication",
                    Label = "AD Replication: Enable NTDS RPC Encrypted Communication Channel",
                    Category = "System",
                    Description =
                        "Sets EncryptRpcCommunication=1 in NTDS\\Parameters. Enables RPC encryption for AD replication traffic between domain controllers. AD DS replication uses Microsoft RPC over TCP for inter-DC communication. Enabling RPC encryption ensures that the payload of replication packets (directory object changes, attribute updates, password hash data) is encrypted in transit between DCs. This is layered protection on top of sign-and-seal — even if sign-and-seal at the NTDS layer is bypassed, the RPC transport layer encryption provides an additional barrier.",
                    Tags = ["ntds", "rpc", "encryption", "replication", "transport-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "RPC encryption enabled for NTDS replication traffic. Additional CPU overhead from AES encryption of replication packets. On modern DC hardware (Xeon, EPYC) AES-NI instructions keep overhead below 1%. Verify no inter-DC firewall rules block the dynamic RPC port range (49152–65535) after enabling.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "EncryptRpcCommunication", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "EncryptRpcCommunication")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "EncryptRpcCommunication", 1)],
                },
                new TweakDef
                {
                    Id = "adrep-set-ad-query-policy-max-objects-10000",
                    Label = "AD Replication: Cap AD DS Directory Queries to 10000 Objects Per Operation",
                    Category = "System",
                    Description =
                        "Sets MaxTempTableSize=10000 in NTDS\\Parameters (units: objects). Limits the number of objects returned in a single AD query operation to 10,000. Unrestricted AD queries can consume significant DC CPU and memory — an attacker with LDAP read access who issues an unbounded subtree search against the entire domain partition can cause a DC denial-of-service by forcing it to process a millions-of-results query. Setting a per-query object cap ensures that even large LDAP clients must paginate, distributing the query load over time and preventing single-query DC saturation.",
                    Tags = ["ntds", "query-limit", "dos-mitigation", "ldap", "pagination"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "AD directory queries capped at 10,000 objects per operation. Clients requiring more than 10,000 results must use LDAP paged results control. Modern enterprise applications (Azure AD Connect, ADMT, Quest Migration Manager) handle paging natively. Custom scripts using unbounded LDAP searches must be updated.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "MaxTempTableSize", 10000)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "MaxTempTableSize")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "MaxTempTableSize", 10000)],
                },
                new TweakDef
                {
                    Id = "adrep-enable-rid-master-audit",
                    Label = "AD Replication: Enable RID (Relative Identifier) Pool Allocation Audit",
                    Category = "System",
                    Description =
                        "Sets AuditRidAllocation=1 in NTDS\\Parameters. Enables auditing of RID (Relative Identifier) pool allocation requests. Domain controllers request blocks of RIDs from the RID Master FSMO role to assign unique object SIDs when creating new AD objects. An unusually high rate of RID pool requests from a specific DC (e.g., thousands of allocations per day) may indicate automated object creation — a technique used by ransomware operators to create new privileged accounts en masse or by red teams performing domain object flooding. Auditing RID allocation enables detection of anomalous object creation bursts.",
                    Tags = ["ntds", "rid", "rid-master", "object-creation", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "RID pool allocation requests are audited. Generates Event ID 16657 in Directory Service log when a DC requests a new RID pool. Baseline normal RID allocation frequency for your environment (typically 1 pool every few months per active DC). Alert on abnormal frequency as potential ransomware or red-team indicator.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "AuditRidAllocation", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "AuditRidAllocation")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "AuditRidAllocation", 1)],
                },
            ];
    }

    // ── AzureVirtualDesktopPolicy ──
    private static class _AzureVirtualDesktopPolicy
    {
        private const string TsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

        private const string AvdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services\Azure Virtual Desktop";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "avd-enable-watermarking",
                    Label = "AVD: Enable Screen Watermarking for Session Hosts",
                    Category = "System",
                    Description =
                        "Sets EnableWatermarking=1 and WatermarkingHeightFactor/WidthFactor. Overlays a semi-transparent QR code on AVD session screens that encodes the user's UPN and session identifier. This watermark is user-invisible during normal work but visible in screenshots and screen captures. Watermarking is essential for data loss investigations and insider threat deterrence in environments handling sensitive or regulated data.",
                    Tags = ["avd", "watermarking", "dlp", "session", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Transparent watermark; no user impact. QR code is visible in screen captures which may affect screen-sharing in training.",
                    ApplyOps = [RegOp.SetDword(TsKey, "EnableWatermarking", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "EnableWatermarking")],
                    DetectOps = [RegOp.CheckDword(TsKey, "EnableWatermarking", 1)],
                },
                new TweakDef
                {
                    Id = "avd-disable-drive-redirect",
                    Label = "AVD: Disable Drive Redirection in Sessions",
                    Category = "System",
                    Description =
                        "Sets fDisableCdm=1. Prevents client-side drives (USB sticks, local hard drives, network shares) from being mounted in AVD sessions. Drive redirection is exploited for both data exfiltration (copying from session to external media) and malware delivery (running executables from a USB drive in the session). Removing drive redirection is a standard DLP and malware containment control in supervised VDI environments.",
                    Tags = ["avd", "drive-redirect", "usb", "dlp", "malware"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Removes session access to client drives. Users cannot access USB media or local files from within the AVD session.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisableCdm", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableCdm")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisableCdm", 1)],
                },
                new TweakDef
                {
                    Id = "avd-idle-disconnect-30min",
                    Label = "AVD: Disconnect Idle Sessions After 30 Minutes",
                    Category = "System",
                    Description =
                        "Sets MaxIdleTime=1800000 (30 minutes in milliseconds). Automatically disconnects AVD sessions that have been idle for 30 minutes. Idle sessions consume Azure compute costs and create an unattended-session security risk where unlocked sessions could be accessed by physical access to an unattended client. Auto-disconnect after 30 minutes is the standard enterprise baseline for cost and security management of AVD session hosts.",
                    Tags = ["avd", "idle", "session-management", "cost", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users with unsaved work may lose state if idle for 30 minutes. Pair with auto-save policies.",
                    ApplyOps = [RegOp.SetDword(TsKey, "MaxIdleTime", 1800000)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "MaxIdleTime")],
                    DetectOps = [RegOp.CheckDword(TsKey, "MaxIdleTime", 1800000)],
                },
                new TweakDef
                {
                    Id = "avd-enable-screen-capture-protection",
                    Label = "AVD: Enable Screen Capture Protection (DRM-Level)",
                    Category = "System",
                    Description =
                        "Sets fEnableScreenCaptureProtect=1. Enables AVD screen capture protection, which uses DRM-style OS hooks to prevent the AVD session content from being captured by screenshots, screen recording software, or GPU frame capture tools on the client side. The session content appears as a black region in any screen capture. Essential for protecting classified information displays, financial data, and healthcare PHI from accidental or intentional screen capture exfiltration.",
                    Tags = ["avd", "screen-capture", "dlp", "drm", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Session content is blackened in all screen captures on the client. Training recordings and accessibility tools that capture the screen will not see session content.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fEnableScreenCaptureProtect", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fEnableScreenCaptureProtect")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fEnableScreenCaptureProtect", 1)],
                },
                new TweakDef
                {
                    Id = "avd-enable-private-mode",
                    Label = "AVD: Enable Private Mode for Session Hosts",
                    Category = "System",
                    Description =
                        "Sets EnablePrivateMode=1. Activates AVD Private Mode which restricts session actions to reduce data leakage risk: disables local clipboard, file transfers, printing to local printers, and local drive access in a single policy. Private Mode is designed for shared/kiosk session hosts in sensitive environments where multiple users share the same session host profile. Equivalent to enabling fDisableClip + fDisableCdm + printer restrictions together.",
                    Tags = ["avd", "private-mode", "kiosk", "dlp", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Restricts all peripheral redirection. Not suitable for productivity use-cases requiring local file access or printing.",
                    ApplyOps = [RegOp.SetDword(TsKey, "EnablePrivateMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "EnablePrivateMode")],
                    DetectOps = [RegOp.CheckDword(TsKey, "EnablePrivateMode", 1)],
                },
                new TweakDef
                {
                    Id = "avd-set-rdp-security-layer",
                    Label = "AVD: Enforce TLS 1.2+ for RDP Transport Layer",
                    Category = "System",
                    Description =
                        "Sets SecurityLayer=2 (TLS). Forces the Remote Desktop Protocol transport layer to use SSL/TLS 1.2 or later for all connections to AVD session hosts. Value 0 = RDP legacy (cleartext-vulnerable), value 1 = negotiate (downgrade possible), value 2 = TLS required. In Azure, the network path is encrypted at the Azure backbone level; however, enforcing TLS at the RDP layer provides defence-in-depth and satisfies compliance requirements for encrypted-in-transit data.",
                    Tags = ["avd", "rdp", "tls", "encryption", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires TLS; breaks connections from very old RDP clients that cannot negotiate TLS. All modern clients support TLS.",
                    ApplyOps = [RegOp.SetDword(TsKey, "SecurityLayer", 2)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "SecurityLayer")],
                    DetectOps = [RegOp.CheckDword(TsKey, "SecurityLayer", 2)],
                },
                new TweakDef
                {
                    Id = "avd-require-nla",
                    Label = "AVD: Require Network Level Authentication for Sessions",
                    Category = "System",
                    Description =
                        "Sets UserAuthentication=1. Requires Network Level Authentication (NLA) before establishing an RDP session to the AVD host. NLA authenticates the user before allocating session resources, preventing unauthenticated users from reaching the Windows login screen and mounting DoS attacks by opening many half-authenticated sessions. AVD natively enforces Azure AD authentication; this setting adds an additional OS-level NLA gate.",
                    Tags = ["avd", "nla", "authentication", "rdp", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Requires NLA-capable RDP clients (all modern clients support NLA). Very old RDP clients may not connect.",
                    ApplyOps = [RegOp.SetDword(TsKey, "UserAuthentication", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "UserAuthentication")],
                    DetectOps = [RegOp.CheckDword(TsKey, "UserAuthentication", 1)],
                },
                new TweakDef
                {
                    Id = "avd-limit-session-connections",
                    Label = "AVD: Limit Users to a Single Active Session",
                    Category = "System",
                    Description =
                        "Sets fSingleSessionPerUser=1. Restricts each user to a single simultaneous AVD session across all host pool machines. Without this limit, a user can open multiple sessions (e.g., from multiple devices simultaneously), multiplying their compute cost and creating multiple unmanaged session states. Single-session enforcement reduces Azure compute costs proportionally to the number of multi-device users and simplifies session management.",
                    Tags = ["avd", "session-limit", "cost", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Single session per user. Prevents opening the same session from multiple client devices simultaneously.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fSingleSessionPerUser", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fSingleSessionPerUser")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fSingleSessionPerUser", 1)],
                },
                new TweakDef
                {
                    Id = "avd-enable-shortpath-udp",
                    Label = "AVD: Enable UDP ShortPath for Optimized Network Performance",
                    Category = "System",
                    Description =
                        "Sets fClientShortPathEndpointEnabled=1. Activates Azure Virtual Desktop UDP ShortPath, which establishes direct UDP-based transport between the AVD client and session host instead of routing all traffic through the Azure gateway TCP relay. UDP ShortPath reduces round-trip latency from 50–200 ms (TCP relay) to near-direct network latency, dramatically improving display responsiveness and Teams/audio quality in AVD sessions.",
                    Tags = ["avd", "shortpath", "udp", "latency", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Requires UDP 65330 outbound from client to be open on the firewall. Check network policy before enabling.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fClientShortPathEndpointEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fClientShortPathEndpointEnabled")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fClientShortPathEndpointEnabled", 1)],
                },
            ];
    }

    // ── CloudPcWindows365Policy ──
    private static class _CloudPcWindows365Policy
    {
        private const string CloudPcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\CloudPC";

        private const string TsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cloudpc-enable-udp-shortpath",
                    Label = "Cloud PC: Enable UDP ShortPath for Low-Latency Transport",
                    Category = "System",
                    Description =
                        "Sets fUdpRedirectorEnabled=1 under Terminal Services. Enables UDP-based RDP traffic for Windows 365 Cloud PCs, bypassing the TCP relay in Azure and creating a near-direct UDP path from the Windows 365 client to the Cloud PC. UDP ShortPath typically reduces RDP latency by 40–80 ms for geographically proximate users, significantly improving the responsiveness of interactive applications and video playback inside a Cloud PC session.",
                    Tags = ["cloudpc", "windows-365", "udp", "performance", "shortpath"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Requires UDP 3478/65330 outbound from client to Azure. Check firewall configuration before enabling.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fUdpRedirectorEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fUdpRedirectorEnabled")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fUdpRedirectorEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-enable-teams-optimization",
                    Label = "Cloud PC: Enable Teams AV Optimization (Media Redirection)",
                    Category = "System",
                    Description =
                        "Sets TeamsMeetingUnmuteOnEntry=0 and related Teams policy keys. Activates Teams audio/video media optimization for Windows 365 Cloud PCs, which redirects media processing from the Cloud PC CPU to the local client device. Without media optimization, Teams calls are processed server-side, consuming Cloud PC vCPU and causing high latency. With optimization, HD video calls run at near-native quality on the client while the Cloud PC CPU overhead drops by 70–90%.",
                    Tags = ["cloudpc", "teams", "media-redirect", "av", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires Teams client v1.4+ and Windows App/MSTSC v1.2.3004+. Older clients fall back to server-side processing without error.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fEnableTeamsHdxVideoOptimization", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fEnableTeamsHdxVideoOptimization")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fEnableTeamsHdxVideoOptimization", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-disable-printer-redirect",
                    Label = "Cloud PC: Disable Client Printer Redirection",
                    Category = "System",
                    Description =
                        "Sets fDisablePrnt=1. Prevents client-side printers from being redirected into Cloud PC sessions. Printer redirection is a DLP risk (printing regulated data to unmanaged printers) and a performance risk (printer driver discovery causes session startup delays). In Cloud PC deployments, managed network printers should be configured via Intune printer policies; redirect from local client printers is generally not needed and introduces risk.",
                    Tags = ["cloudpc", "printer", "redirect", "dlp", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users cannot print from a Cloud PC to their local/USB printer. Managed print servers via Intune are unaffected.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisablePrnt", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisablePrnt")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisablePrnt", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-set-display-depth-32bit",
                    Label = "Cloud PC: Set Remote Display to 32-Bit Color",
                    Category = "System",
                    Description =
                        "Sets MaxColorDepth=4 (32-bit). Sets the RDP session color depth to 32-bit for Windows 365 Cloud PC sessions. This is the maximum color depth supported by the RDP protocol. Higher color depth improves the quality of rendered graphics, images, and Office documents within Cloud PC sessions. Since Windows 365 provides dedicated compute resources per user, the additional bandwidth from 32-bit color to maximize visual fidelity is generally available.",
                    Tags = ["cloudpc", "display", "color-depth", "quality"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Higher color depth increases RDP bandwidth. Not recommended for poor network connections (<10 Mbps).",
                    ApplyOps = [RegOp.SetDword(TsKey, "MaxColorDepth", 4)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "MaxColorDepth")],
                    DetectOps = [RegOp.CheckDword(TsKey, "MaxColorDepth", 4)],
                },
                new TweakDef
                {
                    Id = "cloudpc-enable-gpu-redirect",
                    Label = "Cloud PC: Enable GPU RemoteFX Virtual GPU",
                    Category = "System",
                    Description =
                        "Sets AVC444ModePreferred=1. Enables AVC444 (H.264 4:4:4 + Alpha) GPU-accelerated video codec for Windows 365 Cloud PC remote display rendering. AVC444 encoding provides near-lossless visual quality for Office and professional design applications within Cloud PC sessions. Windows 365 SKUs with GPU resources support AVC444 by default; this policy ensures it's selected over fallback codecs for the highest visual quality.",
                    Tags = ["cloudpc", "gpu", "avc444", "gpu-redirect", "display"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AVC444 requires Windows 365 GPU-enabled SKU. On CPU-only SKUs this setting is ignored by the display subsystem.",
                    ApplyOps = [RegOp.SetDword(TsKey, "AVC444ModePreferred", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "AVC444ModePreferred")],
                    DetectOps = [RegOp.CheckDword(TsKey, "AVC444ModePreferred", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-lock-session-on-disconnect",
                    Label = "Cloud PC: Lock Screen Immediately on Session Disconnect",
                    Category = "System",
                    Description =
                        "Sets fPromptForPassword=1. Forces Cloud PC sessions to present the Windows lock screen immediately when a client disconnects, preventing subsequent reconnections without re-authentication. Since Cloud PCs are persistent VMs, a disconnected-but-unlocked session could be accessed by the Azure admin or re-attached without the user's explicit re-authentication after a network interruption. Locking on disconnect enforces MFA re-authentication at every new session.",
                    Tags = ["cloudpc", "lock-screen", "authentication", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires MFA re-authentication on every reconnect. Slightly increases session resume time for Teams and app continuity.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fPromptForPassword", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fPromptForPassword")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fPromptForPassword", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-session-time-limit-8h",
                    Label = "Cloud PC: Set Maximum Active Session Duration to 8 Hours",
                    Category = "System",
                    Description =
                        "Sets MaxConnectionTime=28800000 (8 hours in ms). Limits any single active Windows 365 session to 8 hours before forcing a graceful disconnect. Long-running sessions can accumulate memory leaks, stale credentials, and dangling file handles. The 8-hour limit ensures daily session recycling while accommodating a full work day. Windows 365 profiles are persistent so user state is preserved across the disconnect/reconnect cycle.",
                    Tags = ["cloudpc", "session-limit", "time-limit", "maintenance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Users are gracefully disconnected after 8 hours. Unsaved work may be lost if auto-save is not configured. Windows gives a warning before disconnect.",
                    ApplyOps = [RegOp.SetDword(TsKey, "MaxConnectionTime", 28800000)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "MaxConnectionTime")],
                    DetectOps = [RegOp.CheckDword(TsKey, "MaxConnectionTime", 28800000)],
                },
                new TweakDef
                {
                    Id = "cloudpc-disable-audio-record-redirect",
                    Label = "Cloud PC: Disable Microphone Redirection in Sessions",
                    Category = "System",
                    Description =
                        "Sets fDisableAudioCapture=1. Blocks client-side microphone from being redirected into Cloud PC sessions. Microphone-in-session is a privacy risk in shared office environments where other people's conversations could be inadvertently captured in Cloud PC recordings or Teams calls. In organizations using Teams Calling or Teams AV Optimization (which handles audio on the local client endpoint), microphone redirect to the Cloud PC is redundant and unnecessary.",
                    Tags = ["cloudpc", "microphone", "audio", "privacy", "redirect"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Blocks in-session microphone access. Users using Teams AV Optimization are unaffected as audio is processed locally.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisableAudioCapture", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableAudioCapture")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisableAudioCapture", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-enable-display-quality-max",
                    Label = "Cloud PC: Set Maximum Visual Quality Level for Display",
                    Category = "System",
                    Description =
                        "Sets VisualQuality=3 (medium-high). Sets the Cloud PC RDP display quality to the highest persistent level. Windows 365 uses dynamic display quality to adapt to network bandwidth; this policy sets the floor to 3 (medium-high) so quality never drops below acceptable levels on stable Azure Expressroute or 100Mbps+ connections. Particularly beneficial for Cloud PCs used for creative and Office work where blurry codec artifacts impair productivity.",
                    Tags = ["cloudpc", "display-quality", "rdp", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Higher baseline quality uses more bandwidth (~5–10 Mbps sustained). Not recommended for Cloud PCs accessed over mobile/4G connections.",
                    ApplyOps = [RegOp.SetDword(TsKey, "VisualQuality", 3)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "VisualQuality")],
                    DetectOps = [RegOp.CheckDword(TsKey, "VisualQuality", 3)],
                },
                new TweakDef
                {
                    Id = "cloudpc-disable-device-redirect",
                    Label = "Cloud PC: Disable PnP Device Redirection into Sessions",
                    Category = "System",
                    Description =
                        "Sets fDisablePNPRedir=1. Blocks Plug-and-Play (PnP) device redirection from the client endpoint into the Cloud PC session. PnP redirection allows USB devices (webcams, scanners, dongles, smart card readers) to appear inside the Cloud PC session. This creates an uncontrolled hardware import surface: unmanaged USB devices can introduce malware through HID attacks or USB Rubber Ducky-style injection. Block PnP redirect unless there is a specific use case for hardware peripherals in Cloud PC.",
                    Tags = ["cloudpc", "usb", "pnp", "device-redirect", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "USB/PnP devices are not available inside Cloud PC sessions. Smart card readers for local authentication are unaffected if using NLA.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisablePNPRedir", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisablePNPRedir")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisablePNPRedir", 1)],
                },
            ];
    }

    // ── ConfigurationManagerPolicy ──
    private static class _ConfigurationManagerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\ConfigurationManager";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "confmgr-require-code-signing-for-scripts",
                    Label = "ConfigMgr: Require Script Code Signing for All Client-Side Script Execution",
                    Category = "System",
                    Description =
                        "Sets RequireScriptCodeSigning=1 in ConfigurationManager policy. Requires that any script (PowerShell, VBScript, JScript) deployed through the Configuration Manager client for task sequences or application deployment must be digitally signed by a certificate trusted by the client's root store before execution. "
                        + "Configuration Manager script execution is a primary lateral movement vector in enterprise environments. A compromised management server or a rogue admin with deployment rights can push arbitrary scripts to all managed clients. Without code signing enforcement, any script pushed through ConfigMgr is executed verbatim. Requiring script code signing ensures only scripts signed by the enterprise PKI certificate authority are executed.",
                    Tags = ["configmgr", "sccm", "scripts", "code-signing", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Unsigned ConfigMgr scripts blocked; all deployment scripts must be signed by enterprise PKI before execution.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireScriptCodeSigning", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireScriptCodeSigning")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireScriptCodeSigning", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-enable-client-audit-logging",
                    Label = "ConfigMgr: Enable Comprehensive Audit Logging for Client Agent Operations",
                    Category = "System",
                    Description =
                        "Sets EnableClientAuditLogging=1 in ConfigurationManager policy. Enables detailed audit logging in the Configuration Manager client agent, causing all deployment operations (software installs, uninstalls, state machine transitions, inventory collection, policy downloads) to be recorded in the Security event log in addition to the standard ccmsetup.log files. "
                        + "The default ConfigMgr client logging writes verbose detail to log files under C:\\Windows\\CCM\\Logs\\ but does not generate Security event log entries auditable by a SIEM. With audit logging enabled, Security events are generated for every ConfigMgr operation, enabling correlation with Active Directory logon events, PowerShell execution events, and process creation events during incident investigations. This enables detection of ConfigMgr-based lateral movement.",
                    Tags = ["configmgr", "sccm", "audit-log", "security", "siem"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "ConfigMgr operations generate Security event log entries; SIEM can correlate ConfigMgr deployments with suspicious activities.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableClientAuditLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableClientAuditLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableClientAuditLogging", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-require-ssl-for-management-point",
                    Label = "ConfigMgr: Require HTTPS/PKI for All Client-to–Management Point Communication",
                    Category = "System",
                    Description =
                        "Sets RequireSSLForManagementPoint=1 in ConfigurationManager policy. Enforces that the ConfigMgr client uses HTTPS with PKI client certificates for all communication with the Management Point, Distribution Point, and other site roles, blocking fallback to HTTP. "
                        + "Configuration Manager in HTTP mode transmits deployment data, credentials used for network access accounts, and package download URLs in plaintext. A network attacker on the same segment as a ConfigMgr client can intercept policy downloads and inject malicious package locations. Enforcing HTTPS-only communication requires PKI infrastructure but prevents man-in-the-middle interception of ConfigMgr policy and deployment content.",
                    Tags = ["configmgr", "sccm", "https", "pki", "ssl", "management-point"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "ConfigMgr client requires HTTPS; HTTP communication with management point blocked. Requires PKI client certificates to be enrolled.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSSLForManagementPoint", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSSLForManagementPoint")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSSLForManagementPoint", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-disable-software-center-user-portal",
                    Label = "ConfigMgr: Disable Software Center User-Initiated Install Portal",
                    Category = "System",
                    Description =
                        "Sets DisableSoftwareCenterPortal=1 in ConfigurationManager policy. Disables the Software Center user-facing portal through which end users can browse 'Available' software and initiate their own optional application installs. Only 'Required' deployments that are pushed and mandatory remain active; the Software Center self-service catalog is removed from the user's Start menu. "
                        + "The Software Center self-service portal is appropriate for general enterprise endpoints where end users should be able to install productivity tools. In high-security or locked-down environments (healthcare workstations, kiosk terminals, PCI-scope machines), allowing users to install any software from the catalog — even admin-approved software — introduces unnecessary attack surface expansion. Application installs should be exclusively IT-admin-driven deployments.",
                    Tags = ["configmgr", "sccm", "software-center", "lockdown", "user-install"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Software Center self-service portal disabled; only mandatory/required ConfigMgr deployments are presented to users.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSoftwareCenterPortal", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSoftwareCenterPortal")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSoftwareCenterPortal", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-disable-client-auto-upgrade",
                    Label = "ConfigMgr: Disable Automatic ConfigMgr Client Agent Auto-Upgrade",
                    Category = "System",
                    Description =
                        "Sets DisableAutoUpgrade=1 in ConfigurationManager policy. Prevents the ConfigMgr client agent from automatically upgrading itself when the site server is running a newer version of the ConfigMgr client, requiring IT to explicitly push client upgrades through a managed deployment. "
                        + "The ConfigMgr client auto-upgrade mechanism upgrades the client agent on all managed endpoints automatically when the Primary Site server is upgraded. While convenient, this means that upgrading the site server triggers an automatic, uncontrolled rollout to thousands of endpoints simultaneously, with no staging, no pilot group, and no rollback capability. A buggy client version pushed by auto-upgrade to all endpoints can simultaneously disrupt the management channel for the entire estate.",
                    Tags = ["configmgr", "sccm", "client-upgrade", "rollout", "change-control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ConfigMgr client auto-upgrade disabled; client upgrades require explicit IT-managed deployment packages.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoUpgrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoUpgrade")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoUpgrade", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-require-admin-for-user-policy-execution",
                    Label = "ConfigMgr: Require Administrative Approval Before User-Targeted Policy Execution",
                    Category = "System",
                    Description =
                        "Sets RequireAdminApprovalForUserPolicy=1 in ConfigurationManager policy. Requires that user-targeted configuration baseline deployments (policies applied to users, not computers) receive explicit IT admin approval in the ConfigMgr console before the client agent executes them on the endpoint. "
                        + "In some ConfigMgr configurations, user-targeted configuration baselines can be deployed to security groups by less-privileged admins (Help Desk, Application Deployment staff) without requiring full ConfigMgr infrastructure admin privileges. If those baselines include scripts or registry modifications, a Help Desk operator with deployment rights could push policy changes to all users in their management scope. Requiring admin approval creates a second-factor approval gate for user-targeted policy execution.",
                    Tags = ["configmgr", "sccm", "user-policy", "admin-approval", "separation-of-duties"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "User-targeted ConfigMgr configuration baselines require admin approval before execution; prevents unauthorised user-policy deployment.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminApprovalForUserPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminApprovalForUserPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminApprovalForUserPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-cap-content-cache-size-5gb",
                    Label = "ConfigMgr: Cap Client Content Cache Size at 5 GB",
                    Category = "System",
                    Description =
                        "Sets MaxContentCacheSizeGB=5 in ConfigurationManager policy. Limits the ConfigMgr client content cache (the local disk cache where the client pre-downloads content from Distribution Points before installation) to a maximum of 5 GB, preventing the cache from consuming disk space beyond this limit. "
                        + "By default, the ConfigMgr client content cache can grow to 10% of total disk size. On large-disk endpoints (1 TB drives), this allows a 100 GB cache. In environments with thin-provisioned storage (VDI, laptop SSDs) or low-disk-space scenarios, an unbounded cache can fill available disk space, causing operating system failures or application performance issues. A 5 GB cap is sufficient for most enterprise software deployments while protecting disk space.",
                    Tags = ["configmgr", "sccm", "cache", "disk-space", "storage"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "ConfigMgr client content cache capped at 5 GB; disk consumption controlled for thin-provisioned storage environments.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxContentCacheSizeGB", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxContentCacheSizeGB")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxContentCacheSizeGB", 5)],
                },
                new TweakDef
                {
                    Id = "confmgr-disable-client-notification-feature",
                    Label = "ConfigMgr: Disable ConfigMgr Client Notification Channel",
                    Category = "System",
                    Description =
                        "Sets DisableClientNotification=1 in ConfigurationManager policy. Disables the ConfigMgr client notification channel — a push mechanism that allows the site server to send fast-path notifications to clients to immediately trigger a policy evaluation or initiate re-inventory without waiting for the standard polling interval. "
                        + "The client notification channel uses a persistent TCP connection from the ConfigMgr client to the Management Point. While this enables near-real-time policy deployment, it also means a compromised Management Point has an active connection to every managed client and can trigger immediate policy execution on all clients simultaneously. In environments where the threat model includes Management Point compromise, disabling the notification channel forces deployments to use the standard polling schedule which is easier to audit and rate-limit.",
                    Tags = ["configmgr", "sccm", "client-notification", "tcp", "management-point"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote =
                        "ConfigMgr push notifications disabled; policy deployment uses scheduled polling intervals instead of near-real-time push.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClientNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClientNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClientNotification", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-enable-tamper-protection",
                    Label = "ConfigMgr: Enable Tamper Protection for ConfigMgr Client Agent",
                    Category = "System",
                    Description =
                        "Sets EnableClientTamperProtection=1 in ConfigurationManager policy. Enables the ConfigMgr client tamper protection mechanism, which prevents standard users and non-admin processes from stopping or disabling the CCMExec service, deleting the CCM client registry keys, or uninstalling the ConfigMgr client agent. "
                        + "Attackers that gain code execution on an endpoint as a standard user or as a low-privilege process will attempt to disable security tools and management agents before proceeding with lateral movement or data exfiltration. The ConfigMgr client agent is a high-value target for disablement because it delivers security baselines, patches, and malware detection policies. Tamper protection prevents the CCMExec service from being stopped by non-admin processes.",
                    Tags = ["configmgr", "sccm", "tamper-protection", "service-protection", "ccmexec"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "ConfigMgr client tamper protection active; CCMExec service cannot be stopped by non-admin processes or scripts.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableClientTamperProtection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableClientTamperProtection")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableClientTamperProtection", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-block-network-access-account-caching",
                    Label = "ConfigMgr: Block Caching of Network Access Account Credentials on Client Disk",
                    Category = "System",
                    Description =
                        "Sets DisableNAACredentialCaching=1 in ConfigurationManager policy. Prevents the ConfigMgr client from caching the Network Access Account (NAA) credentials — the service account used to authenticate with Distribution Points — in the local DPAPI credential store on the client disk. "
                        + "The ConfigMgr Network Access Account is a domain service account whose credentials are distributed to all ConfigMgr-managed clients to allow content download from Distribution Points. By default, these credentials are cached on disk using DPAPI. On a compromised endpoint, an attacker can extract the NAA credentials using tools that decrypt DPAPI-protected data (accessible to SYSTEM-level processes) and then use those credentials to authenticate to internal servers as the NAA service account, often a domain user with broad read access.",
                    Tags = ["configmgr", "sccm", "naa", "credentials", "dpapi", "credential-theft"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "NAA credential caching disabled; ConfigMgr service account credentials are not stored on client disk after each policy download.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableNAACredentialCaching", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableNAACredentialCaching")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableNAACredentialCaching", 1)],
                },
            ];
    }

    // ── DeploymentServicesPolicy ──
    private static class _DeploymentServicesPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "depsvc-disable-multicast",
                Label = "Disable WDS Multicast Image Transfer",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Windows Deployment Services multicast transfers operating system images to multiple clients simultaneously using multicast UDP network traffic. Disabling multicast transmission prevents the generation of multicast network traffic from WDS servers that can impact other network devices. WDS multicast operates on well-known multicast addresses that require multicast routing infrastructure to function correctly. In environments without proper multicast routing, WDS multicast traffic can generate excessive broadcast traffic or fail to route correctly. Disabling multicast forces WDS to use unicast image transfers which are simpler to troubleshoot and less likely to cause network issues. Organizations that have properly configured multicast infrastructure may enable multicast for efficient simultaneous OS deployment to large numbers of clients.",
                Tags = ["wds", "multicast", "deployment", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMulticast", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMulticast")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMulticast", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-disable-pxe-response",
                Label = "Disable WDS PXE Boot Response",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "PXE boot allows network endpoints to boot from a WDS server and receive operating system images instead of booting from local storage. Disabling WDS PXE response prevents unauthorized network boot attempts from connecting to the WDS server. Unauthorized PXE boots could allow an attacker to boot a system into a WDS-provided environment bypassing local security controls. WDS PXE services should only respond to pre-authorized clients and systems requiring legitimate OS deployment. PXE boot in production environments represents an attack vector where adversaries can boot systems into controlled environments to capture credentials or bypass endpoint security. Organizations should restrict PXE responses to pre-authorized MAC addresses when WDS PXE is required for legitimate deployment.",
                Tags = ["wds", "pxe", "boot", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePxeResponse", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePxeResponse")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePxeResponse", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-require-user-authorization",
                Label = "Require User Authorization for WDS Network Boot",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "WDS network boot without user authorization can process deployment requests without verifying that the requesting system and user are authorized. Requiring user authorization for WDS deployments ensures that only authenticated and authorized users can initiate OS deployments through WDS. Unauthorized WDS deployments could replace a system's operating system with an attacker-controlled image bypassing all security controls. WDS authorization requirements prevent automated attackers from deploying compromised operating system images to corporate endpoints. User authorization should be combined with client device authentication to ensure both the user and the device are authorized for OS deployment. WDS authorization policies integrate with Active Directory to enforce group membership requirements before allowing OS deployment.",
                Tags = ["wds", "authorization", "deployment", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireUserAuthorization", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireUserAuthorization")],
                DetectOps = [RegOp.CheckDword(Key, "RequireUserAuthorization", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-enable-boot-logging",
                Label = "Enable WDS Boot Session Logging",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "WDS boot session logging records all client connections, image deployment attempts, and deployment outcomes for audit and troubleshooting purposes. Enabling boot logging ensures a complete record of all WDS deployment activity including successful and failed deployments. Boot session logs help identify unauthorized deployment attempts or unusual deployment patterns that may indicate security incidents. WDS deployment logs are valuable for capacity planning and identifying deployment infrastructure problems. Security monitoring of WDS logs should include alerts for deployments outside of authorized maintenance windows or from unauthorized source addresses. Boot logging should be forwarded to the SIEM to correlate deployment events with other security indicators.",
                Tags = ["wds", "logging", "audit", "deployment", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableBootLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableBootLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableBootLogging", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-disable-tftp-anonymous-access",
                Label = "Disable Anonymous TFTP Access to WDS Boot Files",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "WDS uses TFTP to serve boot files to PXE-booting clients and by default these files may be accessible via anonymous TFTP connections. Disabling anonymous TFTP access prevents unauthorized clients from downloading WDS boot files without authentication. Boot files themselves may not contain sensitive data but unrestricted TFTP access enables reconnaissance of the deployment infrastructure. TFTP file access should require client authentication to prevent mapping of the WDS boot file structure by attackers. Restricting TFTP access forces deployment clients to authenticate before receiving deployment configuration reducing unauthorized access risk. Organizations should evaluate whether anonymous TFTP is required for their deployment infrastructure or if authentication can be enforced.",
                Tags = ["wds", "tftp", "anonymous", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAnonymousTftp", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAnonymousTftp")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAnonymousTftp", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-restrict-image-groups",
                Label = "Restrict WDS Image Groups to Authorized Groups Only",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "WDS image groups organize OS images for deployment and access can be restricted to specific Active Directory security groups. Restricting image group access ensures that only authorized personnel and systems can deploy specific operating system images. Unrestricted image group access allows any authenticated user to initiate deployment of any available OS image including specialized system images. Role-based access to image groups ensures that desktop technicians access desktop images while server images are restricted to server administrators. Image group restrictions prevent employees from initiating unauthorized OS replacements on endpoints they manage. Image access auditing should be enabled alongside image group restrictions to log all access attempts.",
                Tags = ["wds", "image-groups", "access-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictImageGroupAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictImageGroupAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictImageGroupAccess", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-enable-driver-injection-restriction",
                Label = "Restrict WDS Driver Injection to Approved Drivers",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "WDS driver injection adds hardware-specific drivers to OS images during deployment providing out-of-box device compatibility. Restricting driver injection to approved driver packages prevents unapproved or potentially malicious drivers from being injected into deployment images. Driver injection without restrictions could allow an attacker who gains access to WDS infrastructure to inject malicious drivers into OS images. Approved driver packages should be tested and validated before adding to the WDS driver store for injection. Driver signing requirements should be enforced for all drivers added to the WDS driver store to prevent injection of unsigned drivers. WDS driver injection policies integrate with Windows Update for Business and Microsoft Update Catalog for validated driver sources.",
                Tags = ["wds", "drivers", "injection", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictDriverInjection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictDriverInjection")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictDriverInjection", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-disable-wds-client-logging",
                Label = "Disable WDS Client-Side Telemetry Logging",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "WDS client telemetry logging can transmit deployment event data from client systems back to Microsoft or deployment management systems. Disabling WDS client telemetry logging reduces the data transmitted about deployment infrastructure and client system configuration. Client telemetry during OS deployment includes hardware enumeration data, deployment timing, and setup configuration details. Reducing telemetry transmission during deployment limits the external visibility into enterprise deployment infrastructure and hardware inventory. WDS client telemetry data is most useful for troubleshooting deployment issues but may expose sensitive infrastructure details if transmitted externally. Organizations that rely on WDS telemetry for deployment health monitoring should route data to internal logging rather than external Microsoft services.",
                Tags = ["wds", "telemetry", "logging", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableClientTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClientTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClientTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-enforce-network-boot-security",
                Label = "Enforce Secure Network Boot Validation",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Secure network boot validation ensures that WDS boot images are cryptographically verified before execution preventing deployment of tampered images. Enforcing secure network boot validation prevents boot time attacks where an attacker replaces legitimate WDS boot images with malicious alternatives. Boot image integrity validation uses digital signatures to verify that images have not been modified since signing by the deployment administrator. Secure network boot is essential in environments where WDS infrastructure may be accessible to adversaries with lateral movement capabilities. Boot image signing should use code signing certificates with appropriate access controls to prevent unauthorized image signing. Secure network boot validation should be combined with Secure Boot on client systems to create an end-to-end boot integrity chain.",
                Tags = ["wds", "secure-boot", "validation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceSecureNetworkBoot", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceSecureNetworkBoot")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceSecureNetworkBoot", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-limit-wds-server-accessibility",
                Label = "Restrict WDS Server Access to Deployment VLAN",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "WDS servers should only be accessible from deployment VLANs and networks used for OS deployment activities rather than from all corporate subnets. Restricting WDS server accessibility reduces the attack surface by limiting which network segments can reach the deployment infrastructure. Production VLANs should not have direct access to WDS servers unless those servers are actively used for production endpoint deployment. Deployment infrastructure accessible from all corporate subnets increases the risk of unauthorized deployment attempts or WDS infrastructure exploitation. Network ACLs and host-based firewall rules should restrict WDS port access to deployment VLANs and administrator management networks. WDS server accessibility restrictions should be documented and reviewed regularly as network topology changes.",
                Tags = ["wds", "network-restriction", "vlan", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LimitServerAccessibility", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LimitServerAccessibility")],
                DetectOps = [RegOp.CheckDword(Key, "LimitServerAccessibility", 1)],
            },
        ];
    }

    // ── DomainControllerHardeningPolicy ──
    private static class _DomainControllerHardeningPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";
        private const string SecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Netlogon";
        private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "dchrdn-restrict-null-session-pipes",
                    Label = "Restrict Null Session Named Pipe Access to Empty List",
                    Category = "System",
                    Description =
                        "Removes all entries from the NullSessionPipes registry value, ensuring no named pipes can be accessed via anonymous null session connections on this machine, closing a legacy attack vector for anonymous RPC enumeration.",
                    Tags = ["netlogon", "null-session", "named-pipes", "anonymous", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Null session named pipe list cleared; anonymous RPC pipe access completely blocked.",
                    ApplyOps = [RegOp.SetString(SecKey, "NullSessionPipes", "")],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "NullSessionPipes")],
                    DetectOps = [RegOp.CheckString(SecKey, "NullSessionPipes", "")],
                },
                new TweakDef
                {
                    Id = "dchrdn-log-netlogon-failures",
                    Label = "Log Netlogon Secure Channel Failure Events",
                    Category = "System",
                    Description =
                        "Enables detailed event log entries for Netlogon secure channel establishment failures, authentication denials, and secure channel seal/sign rejections, providing visibility into DC trust channel attacks.",
                    Tags = ["netlogon", "event-log", "audit", "secure-channel-failure", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Netlogon secure channel failure events logged; DC authentication and Zerologon attack attempts visible.",
                    ApplyOps = [RegOp.SetDword(Key, "DbFlag", 0x2080FFFF)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DbFlag")],
                    DetectOps = [RegOp.CheckDword(Key, "DbFlag", 0x2080FFFF)],
                },
                new TweakDef
                {
                    Id = "dchrdn-restrict-ntlm-in-domain",
                    Label = "Restrict Incoming NTLM Authentication in Domain Context",
                    Category = "System",
                    Description =
                        "Configures this domain member to block incoming NTLM authentication from domain accounts, requiring Kerberos for all intra-domain service authentication and preventing NTLM relay and pass-the-hash attacks between domain members.",
                    Tags = ["netlogon", "ntlm", "domain", "relay-attack", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Incoming NTLM from domain accounts blocked; intra-domain Kerberos required. NTLM relay via domain accounts mitigated.",
                    ApplyOps = [RegOp.SetDword(LsaKey, "RestrictReceivingNTLMTrafficInDomain", 2)],
                    RemoveOps = [RegOp.DeleteValue(LsaKey, "RestrictReceivingNTLMTrafficInDomain")],
                    DetectOps = [RegOp.CheckDword(LsaKey, "RestrictReceivingNTLMTrafficInDomain", 2)],
                },
                new TweakDef
                {
                    Id = "dchrdn-disable-netlogon-telemetry",
                    Label = "Disable Netlogon and Domain Services Telemetry to Microsoft",
                    Category = "System",
                    Description =
                        "Prevents the Netlogon service and domain authentication components from sending DC trust channel statistics, authentication success rates, and secure channel negotiation telemetry to Microsoft.",
                    Tags = ["netlogon", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Netlogon telemetry to Microsoft disabled; DC channel stats and domain auth data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableNetlogonTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableNetlogonTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableNetlogonTelemetry", 1)],
                },
            ];
    }

    // ── DomainIsolationPolicy ──
    private static class _DomainIsolationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPSec";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "domiso-enable-ipsec-policy",
                Label = "Enable IPsec Domain Isolation Policy",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "IPsec domain isolation restricts network communication so that domain-joined computers only accept connections from other authenticated domain members. Enabling domain isolation prevents non-domain-joined devices including compromised guest systems from establishing network connections with managed endpoints. Domain isolation is one of the most effective lateral movement prevention controls in Windows enterprise environments. All network traffic between domain-isolated endpoints is authenticated and optionally encrypted using IPsec transport mode. Implementing domain isolation requires IPsec firewall rules deployed through Group Policy that allow or require authentication. Domain isolation significantly reduces the blast radius of a single compromised endpoint by preventing lateral movement to other domain hosts.",
                Tags = ["ipsec", "domain-isolation", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableDomainIsolation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDomainIsolation")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDomainIsolation", 1)],
            },
            new TweakDef
            {
                Id = "domiso-require-auth-for-inbound",
                Label = "Require IPsec Authentication for Inbound Connections",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Requiring IPsec authentication for inbound connections ensures that all incoming network requests come from identifiable and domain-authenticated sources. Inbound authentication requirements prevent anonymous network connections and force all sources to be authenticated before services are accessible. IPsec authentication for inbound traffic uses Kerberos tickets from domain-joined computers providing cryptographic proof of identity. Non-domain resources that need access can be granted exemptions through connection security rule exceptions while keeping general isolation. Requiring inbound authentication effectively creates a software-defined perimeter that moves beyond network segmentation. This policy is foundational for server isolation scenarios where critical servers should only accept connections from specific authenticated hosts.",
                Tags = ["ipsec", "inbound", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireAuthForInbound", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAuthForInbound")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAuthForInbound", 1)],
            },
            new TweakDef
            {
                Id = "domiso-enable-ipsec-encryption",
                Label = "Enable IPsec Traffic Encryption",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "IPsec traffic encryption protects the confidentiality of data transmitted between domain-isolated endpoints beyond providing authentication. Encrypting IPsec traffic ensures that even if network traffic is intercepted the data content remains confidential. IPsec encryption complements authentication by preventing data-in-transit exposure for east-west traffic between domain systems. Enabling encryption in domain isolation scenarios requires negotiating encryption algorithms through IKE and maintaining security associations. AES-128 or AES-256 should be specified as the encryption algorithms in IPsec policy for modern compliance requirements. IPsec encryption for all domain traffic provides confidentiality protection that complements TLS-based application encryption.",
                Tags = ["ipsec", "encryption", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableIPsecEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIPsecEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIPsecEncryption", 1)],
            },
            new TweakDef
            {
                Id = "domiso-prefer-aes256",
                Label = "Prefer AES-256 for IPsec Encryption",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "AES-256 provides 256-bit symmetric encryption which exceeds NIST SP 800-57 recommendations for protection beyond 2030. Preferring AES-256 in IPsec policy ensures the strongest available symmetric encryption is negotiated for domain isolation traffic. Weaker algorithms like 3DES or AES-128 should only be used for compatibility when AES-256 is unavailable. AES-256 in IPsec may have slightly higher processing overhead than AES-128 but this is negligible on modern CPUs with AES-NI hardware acceleration. Setting AES-256 as the preferred algorithm ensures IKE negotiation selects the strongest option when both parties support it. Hardcoding preferred algorithms in IPsec policy prevents algorithm downgrade during negotiation to weaker but still technically supported options.",
                Tags = ["ipsec", "aes256", "encryption", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreferAES256", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreferAES256")],
                DetectOps = [RegOp.CheckDword(Key, "PreferAES256", 1)],
            },
            new TweakDef
            {
                Id = "domiso-enable-perfect-forward-secrecy",
                Label = "Enable Perfect Forward Secrecy for IPsec",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Perfect forward secrecy ensures that compromise of long-term keys does not allow decryption of previously recorded encrypted sessions. Enabling PFS for IPsec generates unique session keys for each IPsec security association using ephemeral Diffie-Hellman key exchange. Without PFS an attacker who records encrypted traffic can decrypt it after compromising the long-term keys used in the key exchange. PFS in IPsec requires renegotiation of master keys during IKE Phase 2 which adds some processing overhead. Diffie-Hellman Group 14 (2048-bit) or stronger should be specified for PFS to provide adequate security for the key exchange. PFS is an important property for long-lived secrets and provides cryptographic forward secrecy as part of defense-in-depth.",
                Tags = ["ipsec", "pfs", "forward-secrecy", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePerfectForwardSecrecy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePerfectForwardSecrecy")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePerfectForwardSecrecy", 1)],
            },
            new TweakDef
            {
                Id = "domiso-block-non-ipsec-fallback",
                Label = "Block Fallback to Unprotected Connections",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Connection fallback allows domain-isolated endpoints to fall back to unauthenticated connections when IPsec negotiation fails. Blocking fallback from IPsec-required connections ensures that traffic either uses IPsec protection or is not transmitted. Allowing fallback creates a gap in domain isolation where an attacker can force IPsec negotiation failure and access a target via plain traffic. Connection security rules should specify whether partial authentication fallback is allowed on a per-rule basis. Blocking fallback is appropriate for high-security servers while client endpoints may allow fallback for connections to non-domain resources. The trade-off between security (no fallback) and availability (fallback for resilience) must be evaluated for each deployment context.",
                Tags = ["ipsec", "fallback", "domain-isolation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockNonIPsecFallback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockNonIPsecFallback")],
                DetectOps = [RegOp.CheckDword(Key, "BlockNonIPsecFallback", 1)],
            },
            new TweakDef
            {
                Id = "domiso-enable-ike-v2",
                Label = "Require IKEv2 for IPsec Key Exchange",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "IKEv2 is the modern version of the Internet Key Exchange protocol providing improved security, reliability, and mobility compared to IKEv1. Requiring IKEv2 ensures that IPsec connections use the protocol version with MOBIKE support, traffic selectors, and improved negotiation. IKEv2 includes native dead peer detection and eliminates many of the mode negotiation vulnerabilities present in IKEv1 main and aggressive mode. IKEv2 with EAP authentication provides a strong mutual authentication mechanism suitable for remote access and domain isolation scenarios. Windows has supported IKEv2 since Windows 7 so requiring IKEv2 should not cause compatibility issues on modern enterprise endpoints. Requiring IKEv2 eliminates exposure to IKEv1 vulnerabilities including aggressive mode pre-shared key cracking and main mode identity disclosure.",
                Tags = ["ipsec", "ikev2", "key-exchange", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireIKEv2", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireIKEv2")],
                DetectOps = [RegOp.CheckDword(Key, "RequireIKEv2", 1)],
            },
            new TweakDef
            {
                Id = "domiso-log-ipsec-failures",
                Label = "Enable IPsec Negotiation Failure Logging",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "IPsec negotiation failure logging records events when IPsec key exchange fails providing visibility into domain isolation policy enforcement. Enabling failure logging helps diagnose misconfigured clients, rogue devices attempting connections, and policy configuration errors. Persistent IPsec negotiation failures from unexpected source addresses may indicate unauthorized devices attempting to communicate. IPsec failure events in Windows Firewall and Security Auditing logs include source/destination addresses, error codes, and protocol identifiers. SIEM correlation of IPsec failures with other security events enables detection of attempts to circumvent domain isolation. IPsec failure logging is essential during initial domain isolation deployment to identify endpoints that need policy updates.",
                Tags = ["ipsec", "logging", "failures", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LogIPsecFailures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogIPsecFailures")],
                DetectOps = [RegOp.CheckDword(Key, "LogIPsecFailures", 1)],
            },
            new TweakDef
            {
                Id = "domiso-exempt-icmp",
                Label = "Configure ICMP Exemption from IPsec",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "ICMP exemption from IPsec authentication requirements allows diagnostic network utilities like ping to function without IPsec negotiations. Configuring ICMP exemptions maintains diagnostic capability while requiring IPsec for all other traffic types. ICMP traffic does not carry sensitive data and exempting it simplifies troubleshooting without compromising the security of data-carrying connections. Exempted ICMP traffic still traverses the network in plaintext which is acceptable since ICMP carries diagnostic information not sensitive data. Overly strict IPsec policies that fail ICMP traffic complicate network troubleshooting and may cause connectivity issues with network monitoring tools. ICMP exemption should be combined with ICMP rate limiting and filtering to prevent ICMP-based denial of service attacks.",
                Tags = ["ipsec", "icmp", "exemption", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ExemptICMPFromIPSec", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ExemptICMPFromIPSec")],
                DetectOps = [RegOp.CheckDword(Key, "ExemptICMPFromIPSec", 1)],
            },
            new TweakDef
            {
                Id = "domiso-enable-ipsec-monitoring",
                Label = "Enable IPsec Security Association Monitoring",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "IPsec security association monitoring tracks active IPsec connections and provides operational visibility into domain isolation health. Enabling SA monitoring exposes IPsec connection state for security operations teams to identify unexpected or problematic security associations. Live IPsec SA monitoring can detect sudden changes in protected connection counts that may indicate domain isolation failures or attacks. Security association data is available through Windows Firewall advanced monitoring and the Get-NetIPsecSA PowerShell cmdlet. Monitoring SA establishment rates can identify DoS attempts targeting IKE negotiation processes. IPsec monitoring data should be incorporated into security dashboards alongside firewall, endpoint, and network telemetry.",
                Tags = ["ipsec", "monitoring", "security-association", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSAMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSAMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSAMonitoring", 1)],
            },
        ];
    }

    // ── DomainTrustPolicy ──
    private static class _DomainTrustPolicy
    {
        private const string NetlogonKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

        private const string SystemPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "domtrust-enable-sid-filter-quarantine",
                    Label = "Domain Trust: Enable SID Filtering (Quarantine) on External Trusts",
                    Category = "System",
                    Description =
                        "Sets FilterAdministratorToken=1 in Netlogon\\Parameters. Enables SID filtering (quarantine) on external domain trusts. SID filtering prevents a user in a trusted domain from having SIDs in their access token that belong to privileged groups in the trusting domain. Without SID filtering, an attacker who has compromised a trusted domain can add the 'Domain Admins' SID of the trusting domain to their token via SID history manipulation — a SID history injection attack. With SID filtering, SIDs from the trusted domain that belong to the trusting domain's sensitive groups are stripped from the token.",
                    Tags = ["domain-trust", "sid-filter", "quarantine", "sid-history", "cross-forest"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "SID filter quarantine active on external trusts. Prevents SID history injection attacks across trust boundaries. May break legitimate cross-domain resource access that relies on SID history for migrated accounts. Audit SID history on accounts migrated across the trust boundary before enabling — accounts relying on SID history for access must have explicit permissions granted.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "FilterAdministratorToken", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "FilterAdministratorToken")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "FilterAdministratorToken", 1)],
                },
                new TweakDef
                {
                    Id = "domtrust-disable-anonymous-trust-dc-discovery",
                    Label = "Domain Trust: Disable Anonymous Trust DC Discovery Across Forest",
                    Category = "System",
                    Description =
                        "Sets RefusePWChange=1 in Netlogon\\Parameters. Prevents this DC from processing anonymous inter-domain Netlogon authentication DC discovery requests from untrusted sources. Unauthenticated DC discovery requests (LDAP ping, GetDCName) can be used to enumerate the forest structure, discover domain names, and map the replication topology — all without any credentials. Refusing anonymous discovery from this DC reduces the amount of information an unauthenticated attacker can extract about the forest topology from the network.",
                    Tags = ["domain-trust", "anonymous-discovery", "dc-discovery", "enumeration", "netlogon"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Anonymous DC discovery and password change requests are refused. In standard enterprise environments this has no visible impact. Only environments that have cross-forest resources where non-domain-joined systems need to discover DCs may be affected.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "RefusePWChange", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "RefusePWChange")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "RefusePWChange", 1)],
                },
                new TweakDef
                {
                    Id = "domtrust-set-max-trust-connections-per-dc-8",
                    Label = "Domain Trust: Cap Maximum Trust Relationships Per DC to 8",
                    Category = "System",
                    Description =
                        "Sets MaximumPasswordAge=8 in Netlogon\\Parameters (trust connection context). Limits the number of active trust authentication sessions per DC. Excessive trust-path authentication requests can degrade DC performance and may indicate a trust path enumeration or brute-force attack via trust authentication. Setting a reasonable cap prevents a compromised trust partner from flooding the local DC with trust authentication requests, providing a basic denial-of-service protection for the DC trust authentication subsystem.",
                    Tags = ["domain-trust", "connection-limit", "dos-mitigation", "netlogon"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Trust authentication session connections bounded per DC. Standard enterprise environments with one or two cross-domain trusts are well within this limit. Environments with hub-and-spoke forest designs with many leaf trusts should audit actual trust-path authentication rates before setting this limit.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "MaxConcurrentApi", 8)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "MaxConcurrentApi")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "MaxConcurrentApi", 8)],
                },
                new TweakDef
                {
                    Id = "domtrust-restrict-cross-domain-admin-delegation",
                    Label = "Domain Trust: Restrict Kerberos Constrained Delegation Across Trust",
                    Category = "System",
                    Description =
                        "Sets DisableConstrainedDelegation=1 in the System policy hive. Prevents Kerberos constrained delegation from being used across domain trust boundaries unless explicitly permitted. Cross-domain constrained delegation allows a service in domain A (with the msDS-AllowedToDelegateTo attribute configured to a resource in domain B) to obtain a Kerberos ticket to that resource on behalf of any user. This capability can be abused — an attacker who compromises a service account configured for cross-domain delegation can impersonate any user against the delegated resource in the partner domain. Restricting cross-domain delegation by default limits blast radius.",
                    Tags = ["kerberos-delegation", "cross-domain", "constrained-delegation", "trust", "impersonation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Cross-domain Kerberos constrained delegation disabled by default. Services that legitimately require cross-domain impersonation (e.g., SharePoint cross-domain authentication, SQL Server linked servers) must use Resource-Based Constrained Delegation (RBCD) or be explicitly added to the allowed list. Audit cross-domain delegation in AD before enforcing.",
                    ApplyOps = [RegOp.SetDword(SystemPolicyKey, "DisableConstrainedDelegation", 1)],
                    RemoveOps = [RegOp.DeleteValue(SystemPolicyKey, "DisableConstrainedDelegation")],
                    DetectOps = [RegOp.CheckDword(SystemPolicyKey, "DisableConstrainedDelegation", 1)],
                },
                new TweakDef
                {
                    Id = "domtrust-enable-pam-trust-privilege-check",
                    Label = "Domain Trust: Enable Privileged Access Management PAM Trust",
                    Category = "System",
                    Description =
                        "Sets EnablePAMTrust=1 in Netlogon\\Parameters. Enables the Privileged Access Management (PAM) trust feature on forest trusts (Windows Server 2016+ forest functional level required). PAM trust adds time-limited group membership to the Kerberos PAC — when an admin authenticates via a PAM bastion forest, their group memberships in the resource forest are valid only for the specified time window (e.g., 1 hour). After the window expires, membership is automatically removed. This provides Just-In-Time (JIT) access for privileged accounts — even if the PAM token is stolen, it expires within the configured window.",
                    Tags = ["pam", "just-in-time", "jit", "trust", "privileged-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "PAM trust enabled (requires Windows Server 2016 forest functional level and a PAM bastion forest or equivalent JIT solution). Only relevant in environments with a dedicated administrative forest or Privileged Access Workstation (PAW) architecture. No impact in environments without PAM forest trust configured.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "EnablePAMTrust", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "EnablePAMTrust")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "EnablePAMTrust", 1)],
                },
                new TweakDef
                {
                    Id = "domtrust-enable-selective-authentication-forest",
                    Label = "Domain Trust: Enable Selective Authentication on Forest Trusts",
                    Category = "System",
                    Description =
                        "Sets ForestTransitiveAuth=2 in Netlogon\\Parameters. Enables selective authentication mode on forest trusts. With selective authentication, users from the trusted forest cannot access resources in the trusting forest unless they have been explicitly granted the 'Allowed to Authenticate' permission on the specific computer object they are accessing. This is the opposite of forest-wide authentication (the default), where all users in the trusted forest can attempt to authenticate against any resource in the trusting forest. Selective authentication significantly reduces the blast radius of a trusted-forest compromise.",
                    Tags = ["forest-trust", "selective-authentication", "allowed-to-authenticate", "cross-forest"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Selective authentication requires explicit 'Allowed to Authenticate' permissions on each computer in the trusting forest for trusted-forest users. Without these permissions, trusted-forest users will receive Access Denied errors accessing any resource. This is high-impact when deploying to an existing forest trust — all intended cross-forest resource access must have permissions pre-configured.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "ForestTransitiveAuth", 2)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "ForestTransitiveAuth")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "ForestTransitiveAuth", 2)],
                },
                new TweakDef
                {
                    Id = "domtrust-log-trust-authentication-failures",
                    Label = "Domain Trust: Log All Trust Authentication Failures to Security Log",
                    Category = "System",
                    Description =
                        "Sets AuditTrustAuthFailures=1 in Netlogon\\Parameters. Enables logging of all Netlogon trust authentication failures in the Security event log. Trust authentication failures (wrong trust password, SID filter violation, expired credentials) are logged with the source domain, target domain, error code, and the client computer name. These events are key indicators of: brute-force attacks against trust relationships, trust relationship degradation (trust password drift), and lateral movement attempts using forged cross-domain Kerberos tickets. SIEM correlation rules targeting trust authentication failures enable detection of inter-forest attacks.",
                    Tags = ["domain-trust", "audit", "authentication-failure", "netlogon", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Trust authentication failures logged to Security event log. No impact on successful trust authentications. Event volume is proportional to the number of cross-domain authentication failures — high in environments with expired accounts that are trusted across forests.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "AuditTrustAuthFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "AuditTrustAuthFailures")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "AuditTrustAuthFailures", 1)],
                },
                new TweakDef
                {
                    Id = "domtrust-set-net-logon-service-tgt-ttl-3600",
                    Label = "Domain Trust: Set Cross-Forest Referral Ticket TTL to 3600 Seconds",
                    Category = "System",
                    Description =
                        "Sets CrossForestReferralTtl=3600 in Netlogon\\Parameters (units: seconds). Sets the Time-To-Live for cross-forest Kerberos referral tickets to 3600 seconds (1 hour). Cross-forest referral tickets are issued when a user in one forest authenticates to a resource in a trusting forest — the KDC issues a referral ticket that the client presents to the trusting forests KDC. Shorter TTLs mean more frequent referral ticket renewals (slightly more authentication overhead) but reduce the window during which a captured referral ticket is valid. 3600 seconds is a reasonable balance between performance and security for standard enterprise cross-forest authentication scenarios.",
                    Tags = ["kerberos", "cross-forest", "referral-ticket", "ttl", "trust"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Cross-forest referral ticket TTL is 1 hour. Cross-forest resource access requires transparent ticket renewal after 1 hour — handled automatically by Windows Kerberos clients. Applications that hold open sessions longer than 1 hour to cross-forest resources should re-authenticate silently. No visible user impact expected.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "CrossForestReferralTtl", 3600)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "CrossForestReferralTtl")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "CrossForestReferralTtl", 3600)],
                },
            ];
    }

    // ── EasMdmPolicy ──
    private static class _EasMdmPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EasMdm";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "easmdm-require-device-password",
                Label = "Exchange ActiveSync MDM Policy: Require Device Password",
                Category = "System",
                Description =
                    "Enforces a device password requirement via Exchange ActiveSync MDM policy. "
                    + "When enabled, users must configure a PIN or password before the device can synchronise with an Exchange server. "
                    + "This aligns with corporate security baselines that mandate authentication on managed endpoints. "
                    + "Disabling removes the enforced password requirement imposed by EAS MDM.",
                Tags = ["eas", "mdm", "password", "exchange", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PasswordEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PasswordEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "PasswordEnabled", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enforces device password via EAS MDM; improves security posture on managed devices.",
            },
            new TweakDef
            {
                Id = "easmdm-min-password-length",
                Label = "Exchange ActiveSync MDM Policy: Set Minimum Password Length (8)",
                Category = "System",
                Description =
                    "Sets the minimum device password length to 8 characters via Exchange ActiveSync MDM policy. "
                    + "Short passwords are vulnerable to brute-force attacks, especially on mobile and endpoint devices. "
                    + "A minimum of 8 characters is recommended by NIST SP 800-63B and aligns with most corporate security policies. "
                    + "Removing this policy reverts to the platform default (typically 4 characters).",
                Tags = ["eas", "mdm", "password", "length", "exchange"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MinDevicePasswordLength", 8)],
                RemoveOps = [RegOp.DeleteValue(Key, "MinDevicePasswordLength")],
                DetectOps = [RegOp.CheckDword(Key, "MinDevicePasswordLength", 8)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Forces minimum 8-character passwords on EAS-managed devices, reducing brute-force risk.",
            },
            new TweakDef
            {
                Id = "easmdm-max-failed-attempts",
                Label = "Exchange ActiveSync MDM Policy: Limit Max Failed Password Attempts (10)",
                Category = "System",
                Description =
                    "Caps the number of consecutive failed password attempts to 10 before triggering a device lockout via Exchange ActiveSync MDM. "
                    + "Limiting failed attempts deters brute-force attacks against the device lock screen. "
                    + "After the threshold is reached, the device is locked and may require an administrator to unlock or initiate a remote wipe. "
                    + "Removing this policy restores the uncapped default.",
                Tags = ["eas", "mdm", "password", "failed-attempts", "lockout"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxDevicePasswordFailedAttempts", 10)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxDevicePasswordFailedAttempts")],
                DetectOps = [RegOp.CheckDword(Key, "MaxDevicePasswordFailedAttempts", 10)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Triggers lockout after 10 failed password attempts; protects against brute-force attacks.",
            },
            new TweakDef
            {
                Id = "easmdm-inactivity-lock-5min",
                Label = "Exchange ActiveSync MDM Policy: Lock Device After 5 Minutes Inactivity",
                Category = "System",
                Description =
                    "Configures the Exchange ActiveSync MDM policy to lock the device screen after 5 minutes of inactivity. "
                    + "Auto-locking an idle device prevents unauthorised access when the device is left unattended. "
                    + "Five minutes is the industry-standard timeout recommended for corporate laptops and workstations. "
                    + "Removing this policy lifts the MDM-enforced inactivity timeout.",
                Tags = ["eas", "mdm", "lock", "inactivity", "screen-lock"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxInactivityTimeDeviceLock", 5)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxInactivityTimeDeviceLock")],
                DetectOps = [RegOp.CheckDword(Key, "MaxInactivityTimeDeviceLock", 5)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Auto-locks device after 5 minutes; reduces risk of unauthorised access to unattended endpoints.",
            },
            new TweakDef
            {
                Id = "easmdm-require-encryption",
                Label = "Exchange ActiveSync MDM Policy: Require Device Encryption",
                Category = "System",
                Description =
                    "Requires full device storage encryption via Exchange ActiveSync MDM policy. "
                    + "Encryption ensures that data stored on the device cannot be read if the hardware is lost or stolen. "
                    + "This policy is mandatory for PCI-DSS, HIPAA, and most corporate data-protection frameworks. "
                    + "Removing this setting lifts the MDM encryption mandate.",
                Tags = ["eas", "mdm", "encryption", "bitlocker", "data-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireDeviceEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "RequireDeviceEncryption", 1)],
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Mandates full-disk encryption on EAS-managed devices; critical for data-at-rest protection.",
            },
            new TweakDef
            {
                Id = "easmdm-block-wifi",
                Label = "Exchange ActiveSync MDM Policy: Block Wi-Fi Connections",
                Category = "System",
                Description =
                    "Disables Wi-Fi connectivity on the device via Exchange ActiveSync MDM policy. "
                    + "Blocking Wi-Fi forces the device to use wired or cellular connections, reducing exposure on potentially unsecured wireless networks. "
                    + "This is typically applied to high-security endpoints or kiosk devices where wireless connectivity is not permitted. "
                    + "Removing this policy restores MDM-controlled Wi-Fi access.",
                Tags = ["eas", "mdm", "wifi", "network", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowWiFi", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowWiFi")],
                DetectOps = [RegOp.CheckDword(Key, "AllowWiFi", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks Wi-Fi on managed devices; enforces wired/cellular-only network access.",
            },
            new TweakDef
            {
                Id = "easmdm-block-removable-storage",
                Label = "Exchange ActiveSync MDM Policy: Block Removable Storage",
                Category = "System",
                Description =
                    "Prevents access to removable storage media (SD cards, USB drives) via Exchange ActiveSync MDM policy. "
                    + "Removable storage is a common vector for data exfiltration and introduction of malware. "
                    + "Blocking it on managed endpoints aligns with DLP (Data Loss Prevention) requirements in regulated industries. "
                    + "Removing this policy restores MDM-controlled removable storage access.",
                Tags = ["eas", "mdm", "storage", "dlp", "removable"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowRemovableStorage", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowRemovableStorage")],
                DetectOps = [RegOp.CheckDword(Key, "AllowRemovableStorage", 0)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks SD cards and USB drives on MDM-managed devices; reduces data exfiltration risk.",
            },
            new TweakDef
            {
                Id = "easmdm-block-camera",
                Label = "Exchange ActiveSync MDM Policy: Block Camera Use",
                Category = "System",
                Description =
                    "Disables camera hardware on the device via Exchange ActiveSync MDM policy. "
                    + "Camera restrictions are commonly required in secure facilities, clean-room environments, or for devices that handle classified information. "
                    + "Enforcing this via MDM policy ensures compliance cannot be bypassed by the end user. "
                    + "Removing this policy restores camera availability on MDM-managed devices.",
                Tags = ["eas", "mdm", "camera", "privacy", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowCamera", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCamera")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCamera", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables camera on MDM-managed devices; required for secure-facility compliance.",
            },
            new TweakDef
            {
                Id = "easmdm-block-internet-sharing",
                Label = "Exchange ActiveSync MDM Policy: Block Internet Sharing / Hotspot",
                Category = "System",
                Description =
                    "Blocks the ability to share the device's internet connection (hotspot/tethering) via Exchange ActiveSync MDM policy. "
                    + "Mobile hotspot can bypass corporate network monitoring and proxy controls, introducing compliance gaps. "
                    + "Prohibiting internet sharing on managed endpoints is a common corporate policy to maintain network visibility. "
                    + "Removing this policy lifts the MDM hotspot restriction.",
                Tags = ["eas", "mdm", "hotspot", "tethering", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowInternetSharing", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowInternetSharing")],
                DetectOps = [RegOp.CheckDword(Key, "AllowInternetSharing", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents hotspot/tethering on managed devices; maintains corporate network control.",
            },
            new TweakDef
            {
                Id = "easmdm-block-bluetooth",
                Label = "Exchange ActiveSync MDM Policy: Block Bluetooth",
                Category = "System",
                Description =
                    "Disables Bluetooth connectivity on the device via Exchange ActiveSync MDM policy. "
                    + "Bluetooth can be exploited for proximity-based attacks (BlueSnarfing, BIAS) or used to exfiltrate data without leaving a network trace. "
                    + "Disabling Bluetooth is recommended for high-security endpoints where physical proximity attacks are a concern. "
                    + "Removing this policy restores MDM-controlled Bluetooth access (value 2 = allow, 0 = block).",
                Tags = ["eas", "mdm", "bluetooth", "wireless", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowBluetooth", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowBluetooth")],
                DetectOps = [RegOp.CheckDword(Key, "AllowBluetooth", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables Bluetooth on MDM-managed devices; reduces BlueSnarfing and proximity-based attack risk.",
            },
        ];
    }

    // ── EnterpriseDeviceManagementPolicy ──
    private static class _EnterpriseDeviceManagementPolicy
    {
        private const string ErmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager";

        private const string MdmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edm-enable-comanagement-with-sccm",
                    Label = "Enterprise Device Management: Enable Intune/SCCM Co-Management",
                    Category = "System",
                    Description =
                        "Sets EnableCoManagement=1 in MDM policy. Enables co-management of Windows 10/11 devices by both System Center Configuration Manager (SCCM/ConfigMgr) and Microsoft Intune simultaneously. Co-management allows gradual migration of workloads from SCCM to Intune — starting with compliance evaluation and conditional access in Intune while keeping software deployment in SCCM. Without this policy, organizations must choose one management plane. Co-management is the Microsoft-recommended path for organizations with existing SCCM infrastructure transitioning to cloud-modern management.",
                    Tags = ["co-management", "sccm", "configmgr", "intune", "cloud-management"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices are managed by both SCCM and Intune simultaneously. Workload authority (compliance, resource access, app deployment) is configurable per workload. Requires ConfigMgr 1710 or later and Intune subscription. Co-management authority conflicts are resolved by the workload slider settings in the SCCM console.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnableCoManagement", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableCoManagement")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnableCoManagement", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-remote-lock-on-compliance-breach",
                    Label = "Enterprise Device Management: Enable Remote Lock on Compliance Breach",
                    Category = "System",
                    Description =
                        "Sets EnableRemoteLockOnComplianceBreach=1 in EnterpriseResourceManager policy. Configures the MDM client to accept remote lock commands from the MDM authority when the device is marked non-compliant AND has not remediated within the grace period. Remote lock sets the device to the lock screen and requires the user to enter their PIN/password to regain access. This prevents a non-compliant device from being used while IT is investigating or while the device is remediating a compliance issue — ensuring that a known-non-compliant device is not being actively used to access corporate resources.",
                    Tags = ["remote-lock", "compliance", "non-compliant", "mdm", "incident-response"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "MDM authority can remotely lock a non-compliant device. The device requires the user's credentials to unlock. User may be temporarily unable to complete their work if locked during active use. Ensure a clear remediation process is communicated to users before deploying. Not the same as remote wipe — data is not affected.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "EnableRemoteLockOnComplianceBreach", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableRemoteLockOnComplianceBreach")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "EnableRemoteLockOnComplianceBreach", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-selective-wipe-on-unenroll",
                    Label = "Enterprise Device Management: Enable Selective Wipe of Corporate Data on Unenroll",
                    Category = "System",
                    Description =
                        "Sets EnableSelectiveWipeOnUnenroll=1 in EnterpriseResourceManager policy. Enables selective wipe of corporate data when a device unenrolls from MDM. Selective wipe removes only corporate-managed content: corporate email profiles, MDM-deployed certificates, VPN profiles, Wi-Fi profiles, and corporate app data — while preserving personal files, photos, and applications. This is the appropriate default for BYOD scenarios: when an employee leaves and disconnects their personal device from MDM, the corporate data is cleaned up without erasing the employee's personal content.",
                    Tags = ["selective-wipe", "unenrollment", "corporate-data", "byod", "data-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Unenrollment from MDM triggers removal of all MDM-deployed profiles, certificates, and managed app data. Personal files and apps are preserved. A corporate AAD-joined device unenrolling may lose domain join state. Not a full device wipe — ensure your users understand what is removed on unenrollment.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "EnableSelectiveWipeOnUnenroll", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableSelectiveWipeOnUnenroll")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "EnableSelectiveWipeOnUnenroll", 1)],
                },
                new TweakDef
                {
                    Id = "edm-require-approved-apps-only",
                    Label = "Enterprise Device Management: Restrict App Installation to MDM-Approved Apps Only",
                    Category = "System",
                    Description =
                        "Sets RequireApprovedAppsOnly=1 in EnterpriseResourceManager policy. Restricts app installation to apps that are deployed or approved by the MDM authority. Users are not permitted to install arbitrary apps from the Microsoft Store or third-party sources unless the MDM administrator has explicitly approved them in the app catalog. This policy is typically layered with AppLocker or Windows Defender Application Control. On its own, it provides an MDM-layer approval gate that blocks app installation from retail Store listings, reducing the attack surface from malicious store apps.",
                    Tags = ["approved-apps", "app-control", "mdm", "store", "whitelisting"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Only MDM-approved apps can be installed by users. Non-approved app installation attempts are blocked. Requires maintaining an approved app catalog in the MDM console. Users who need new apps must request IT approval. May disrupt productivity if the approval catalog is not kept up to date.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "RequireApprovedAppsOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "RequireApprovedAppsOnly")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "RequireApprovedAppsOnly", 1)],
                },
                new TweakDef
                {
                    Id = "edm-sync-device-inventory-every-4h",
                    Label = "Enterprise Device Management: Sync Device Inventory to MDM Every 4 Hours",
                    Category = "System",
                    Description =
                        "Sets InventorySyncIntervalHours=4 in EnterpriseResourceManager policy. Configures the MDM client to push a device inventory update (installed apps, hardware specs, disk space, OS version, installed patches) to the MDM authority every 4 hours. Accurate, fresh device inventory is essential for software license compliance, vulnerability management (detecting devices missing patches), and asset management. A staleinventory (updated less than once daily) may miss a device that has been reformatted, had apps removed, or had OS version changed — leading to false compliance reporting.",
                    Tags = ["inventory", "sync-interval", "asset-management", "vulnerability-mgmt", "mdm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Device inventory is uploaded to the MDM authority every 4 hours. Inventory includes installed apps, hardware, and OS state. Slightly increased MDM check-in frequency and bandwidth. Inventory sync data is typically 5–50 KB per cycle.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "InventorySyncIntervalHours", 4)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "InventorySyncIntervalHours")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "InventorySyncIntervalHours", 4)],
                },
                new TweakDef
                {
                    Id = "edm-block-factory-reset-by-user",
                    Label = "Enterprise Device Management: Prevent User-Initiated Factory Reset",
                    Category = "System",
                    Description =
                        "Sets BlockUserInitiatedFactoryReset=1 in EnterpriseResourceManager policy. Prevents standard users from performing a factory reset (Settings > System > Recovery > Reset this PC, or WinRE recovery). Factory reset bypasses MDM policies, removes all corporate data and certificates, and leaves the device unmanaged. An insider threat actor could use factory reset to wipe evidence before investigation. A regular user could accidentally factory reset, losing both personal and corporate data. IT-initiated remote wipe via the MDM console remains available for authorized operations.",
                    Tags = ["factory-reset", "protective", "insider-threat", "data-preservation", "mdm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Standard users cannot initiate factory reset. Local administrators can still reset via elevated permission flows. IT-initiated remote wipe from MDM console is not affected. Users who genuinely need to re-provision their device must contact IT.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "BlockUserInitiatedFactoryReset", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "BlockUserInitiatedFactoryReset")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "BlockUserInitiatedFactoryReset", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-mdm-certificate-renewal",
                    Label = "Enterprise Device Management: Enable Automatic MDM Certificate Renewal",
                    Category = "System",
                    Description =
                        "Sets EnableMdmCertificateRenewal=1 in MDM policy. Configures the MDM client to automatically renew the MDM enrollment certificate before it expires. The MDM enrollment certificate authenticates the device to the MDM service on every check-in. If this certificate expires without renewal, the device loses the ability to receive new policies, report compliance status, or accept remote management commands — even though it may still appear enrolled in the MDM console. Automatic renewal prevents this silent disconnection, which is especially important for devices in long-term storage or deployed in air-gapped environments.",
                    Tags = ["mdm", "certificate", "renewal", "enrollment", "expiry"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "MDM enrollment certificates are renewed automatically before expiry. Renewal occurs in the background without user interaction. Prevents devices from silently dropping off MDM management due to certificate expiry. Certificate validity periods are typically 1–2 years — renewal triggers at 80% of the validity period.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnableMdmCertificateRenewal", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableMdmCertificateRenewal")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnableMdmCertificateRenewal", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-managed-device-restrictions",
                    Label = "Enterprise Device Management: Enable MDM-Enforced Managed Device Restrictions",
                    Category = "System",
                    Description =
                        "Sets EnableManagedDeviceRestrictions=1 in EnterpriseResourceManager policy. Enables the enforcement layer for MDM-delivered device restrictions — settings like camera disable, screen capture restriction, clipboard policy, USB disable, and Bluetooth restriction — that are delivered as MDM CSP payloads. Without this flag, MDM restriction payloads are accepted but not enforced at the OS level. This is a master switch that must be enabled for MDM-pushed restrictions to take effect. Relevant for organizations deploying information protection policies that require disabling hardware capabilities on managed devices.",
                    Tags = ["mdm", "device-restrictions", "camera-disable", "clipboard", "usb"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "MDM-delivered device restrictions are enforced by the OS. Without this, restrictions are delivered but silently not applied. Restrictions that take effect depend on which CSP payloads the MDM administrator has configured — this policy enables the enforcement mechanism, not specific restrictions.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "EnableManagedDeviceRestrictions", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableManagedDeviceRestrictions")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "EnableManagedDeviceRestrictions", 1)],
                },
                new TweakDef
                {
                    Id = "edm-audit-mdm-policy-changes",
                    Label = "Enterprise Device Management: Audit All MDM Policy Application Events",
                    Category = "System",
                    Description =
                        "Sets AuditMdmPolicyChanges=1 in MDM policy. Enables audit events whenever an MDM policy is applied, updated, or removed on the device. Each audit event records the CSP path that was changed, the old and new values, the MDM authority that issued the change, and the result (success or error code). MDM policy audit events are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel. These events are essential for SIEM correlation: if a device's MDM policy is unexpectedly changed (indicating a rogue MDM push or configuration scope error), the audit trail makes detection possible.",
                    Tags = ["mdm", "audit", "policy-changes", "siem", "event-log"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "All MDM policy application events are logged with CSP path, values, and origin. Events written to DeviceManagement-Enterprise-Diagnostics-Provider channel. Slightly higher log volume on devices with frequent policy changes (Intune check-in + policy delta). Enables SIEM alerting on unexpected MDM policy modifications.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "AuditMdmPolicyChanges", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "AuditMdmPolicyChanges")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "AuditMdmPolicyChanges", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-encrypted-mdm-channel",
                    Label = "Enterprise Device Management: Enforce TLS 1.2+ for MDM Communication",
                    Category = "System",
                    Description =
                        "Sets RequireEncryptedMdmChannel=1 in MDM policy. Enforces that all MDM client communication (enrollment, check-in, policy delivery, command receipt) is conducted over TLS 1.2 or higher. MDM payloads include configuration settings, app assignments, certificate payloads, and VPN profiles — all of which are sensitive. An MDM session over TLS 1.0 can be downgrade-attacked using known vulnerabilities (BEAST, POODLE) to intercept policy payloads. Enforcing TLS 1.2+ on the MDM channel ensures that policy delivery is encrypted to modern standards.",
                    Tags = ["mdm", "tls", "encrypted-channel", "transport-security", "policy-delivery"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "MDM communication is restricted to TLS 1.2 or higher. MDM servers that only support TLS 1.0 or 1.1 will be unable to communicate with the client. All modern MDM services (Intune, SCCM cloud attachment) use TLS 1.2+. On-premises MDM servers must be updated if they are still on legacy TLS.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "RequireEncryptedMdmChannel", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "RequireEncryptedMdmChannel")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "RequireEncryptedMdmChannel", 1)],
                },
            ];
    }

    // ── EnterpriseResourceDeployPolicy ──
    private static class _EnterpriseResourceDeployPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "erdeploy-set-default-deploy-ring-broad",
                    Label = "Enterprise Deploy: Set Default Application Deployment Ring to 'Broad' Stable Channel",
                    Category = "System",
                    Description =
                        "Sets DefaultDeployRing=2 in EnterpriseResourceManager policy. Configures the default application deployment ring for this endpoint to the 'Broad' (stable) deployment ring, ensuring the device receives application updates only after full release validation has been completed across the Pilot and Early Majority rings. "
                        + "Enterprise application deployments using modern ring-based rollout (Intune or ConfigMgr ring filtering) gate updates through sequenced rings before broad deployment. Endpoints that are miscategorised as 'Pilot' receive updates intended for testing and may encounter pre-release application bugs. Explicitly setting the deployment ring to 'Broad' (ring 2) prevents endpoints from accidentally receiving early-ring deployments due to misconfigured ring assignment logic.",
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
                    Category = "System",
                    Description =
                        "Sets RequireAdminForAppRemoval=1 in EnterpriseResourceManager policy. Blocks standard users from uninstalling applications that were deployed by the enterprise (via Intune, ConfigMgr, or Group Policy Software Installation), requiring administrative credentials for removal even though the application was installed in user context. "
                        + "Required enterprise applications (endpoint detection and response agents, certificate management tools, identity protection software) must remain installed once deployed. A standard user who can uninstall enterprise-managed apps can remove security tooling from their device, creating a gap in protection that may persist until the next compliance check triggers a remediation deployment. Blocking user-initiated uninstall of managed apps prevents intentional or accidental removal of critical security tools.",
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
                    Category = "System",
                    Description =
                        "Sets BlockUserInitiatedInstall=1 in EnterpriseResourceManager policy. Prevents users from initiating the installation of new applications through any mechanism other than IT-managed deployment channels (Intune, ConfigMgr, Software Center) — blocking double-click installer execution, Windows Installer (MSI) invocation, and MSIX/APPX package sideloading by standard users. "
                        + "The majority of enterprise malware infections arrive as LOB-disguised executables or malicious MSI packages that a user is socially engineered into running. If users can execute arbitrary installers, the application allowlist maintained by IT is bypassed — even if the endpoint has Microsoft Defender WDAC policy configured, a sufficiently permissive WDAC policy allows signed MSI files from any vendor. Blocking user-initiated installation removes the primary vector for user-driven software installation.",
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
                    Category = "System",
                    Description =
                        "Sets EnforceMaintenanceWindow=1 in EnterpriseResourceManager policy. Restricts deployment execution by the enterprise resource manager to within the configured maintenance window schedule, preventing deployments from triggering application installs, updates, or reboots during business hours and confining disruptive deployments to the approved maintenance period. "
                        + "Without maintenance window enforcement, a deployment configured as 'Available as soon as possible' may start an application install or triggered reboot at any time, including during an end-user presentation or in the middle of a running workflow. Maintenance windows define agreed low-impact periods (after hours, weekends) for deployments. Enforcing the maintenance window prevents IT from accidentally or intentionally bypassing the agreed change window, which is often an ITIL or change management process requirement.",
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
                    Category = "System",
                    Description =
                        "Sets MaxInstallRetries=3 in EnterpriseResourceManager policy. Limits the number of times the enterprise resource manager retries a failed application installation to 3 attempts before marking the deployment as failed and triggering an alert, rather than retrying indefinitely. "
                        + "A deployment that retries an application installation indefinitely will continually consume CPU, disk I/O, and network bandwidth on the endpoint for days or weeks. On endpoints with transient installation failures (antivirus blocking the installer, required service temporarily unavailable), unlimited retries create ongoing performance degradation. Capping retries at 3 ensures failed deployments are surfaced as failures in the management console rather than silently retrying without ever succeeding.",
                    Tags = ["enterprise-deploy", "install-retry", "deployment-failure", "performance", "alert"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Deployment install retries capped at 3; repeat failures surface as deployment failures rather than silent perpetual retry.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxInstallRetries", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxInstallRetries")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxInstallRetries", 3)],
                },
                new TweakDef
                {
                    Id = "erdeploy-enable-deployment-audit-log",
                    Label = "Enterprise Deploy: Enable Security Audit Log for All Deployment Operations",
                    Category = "System",
                    Description =
                        "Sets EnableDeploymentAuditLog=1 in EnterpriseResourceManager policy. Causes each application installation, update, and removal operation completed by the enterprise resource manager to generate a Security event log entry, recording the application name, version, deployment source, requesting authority, and outcome code. "
                        + "Application deployment audit logs are required in PCI-DSS, HIPAA, and SOC2 regulated environments where all software changes on in-scope endpoints must be tracked in a tamper-evident audit log. Without deployment audit logging, an attacker who compromises the management channel and installs a malicious application through the enterprise deployment infrastructure would have no on-device trace of the install (as the standard registry Uninstall key is easily manipulated). Security event log entries are tamper-resistant to local manipulation.",
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
                    Category = "System",
                    Description =
                        "Sets DisableSideloadedApps=1 in EnterpriseResourceManager policy. Prevents installation of APPX/MSIX application packages from unsigned or unmanaged sources (USB drives, SharePoint file shares, developer sideloading) and restricts APPX installation to managed channels only (Microsoft Store for Business, Intune managed app, or enterprise signed MSIX bundles). "
                        + "MSIX sideloading is the primary vector for distributing trojanised or repackaged application packages disguised as legitimate enterprise tools. An attacker who sends a malicious MSIX package via email or file share (and the user's developer mode is enabled) can have arbitrary code run in a package context with the package's declared capabilities. Disabling sideloading from unmanaged sources blocks this vector without affecting Store and Intune-delivered MSIX packages.",
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
                    Category = "System",
                    Description =
                        "Sets RequireSignedPackages=1 in EnterpriseResourceManager policy. Requires that every application package deployed through the enterprise resource manager is digitally signed by a certificate in the enterprise trusted publisher store before the installation is allowed to proceed, blocking unsigned or improperly signed packages from executing. "
                        + "Unsigned deployment packages can be tampered with between the time they are created and the time they are deployed. An attacker who compromises a Distribution Point or content staging server can replace a legitimate installer package with a trojanised version. Without package signing verification, the deployment infrastructure distributes the malicious version to all targeted endpoints without any integrity check. Requiring signed packages ensures only packages that passed code signing (and therefore were authenticated at signing time) are installed.",
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
                    Category = "System",
                    Description =
                        "Sets BlockStoreInstallDuringDeployment=1 in EnterpriseResourceManager policy. Suspends automatic Microsoft Store application updates from downloading and installing during active enterprise deployment windows, preventing Store-initiated background installs from competing with enterprise deployment bandwidth and CPU allocations. "
                        + "Large enterprise deployments (OS feature updates, security patches for hundreds of applications) consume significant bandwidth from Distribution Points. If the Microsoft Store simultaneously triggers background app updates across the same endpoints during the deployment window, both processes compete for disk I/O, network bandwidth, and Windows Installer service locking. This can cause enterprise deployments to fail with 'service busy' errors or time out due to resource contention. Blocking Store updates during scheduled deployment windows eliminates this interference.",
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
                    Category = "System",
                    Description =
                        "Sets EnforcePrerequisiteChecks=1 in EnterpriseResourceManager policy. Enforces that the installation of a dependent application is verified as successfully installed and functional before the enterprise resource manager proceeds with a higher-level application deployment that requires it as a prerequisite, rather than attempting the deployment and failing at runtime. "
                        + "Enterprise application deployments often have prerequisite chains: a LOB application may require a specific .NET runtime version, a specific redistributable, and a specific licence management service to be installed before it will work. Without prerequisite enforcement, all packages attempt installation in parallel, and the LOB application may fail (or partially install) because its prerequisites aren't available yet. Enforcing prerequisite checks runs the dependency chain in the correct order and stops the deployment if any prerequisite fails.",
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

    // ── EnterpriseResourcePolicy ──
    private static class _EnterpriseResourcePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "entres-enable-enterprise-resource-audit",
                Label = "Enable Audit Logging for Enterprise Resource Access Events",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enterprise resource access auditing creates detailed logs of access to managed enterprise resources enabling compliance reporting and security monitoring for sensitive business assets. Audit events for enterprise resources include successful and failed access attempts the identity of the requester the time of access and the resources accessed. Comprehensive resource access logging is a requirement for many regulatory frameworks including PCI-DSS HIPAA and SOX that mandate audit trails for access to regulated data and systems. Organizations should forward enterprise resource audit events to a central security information and event management system for correlation and long-term retention. Audit logs should be protected from modification and deletion and retained for a period consistent with regulatory requirements for the organization. Regular review of enterprise resource audit data helps identify access patterns that deviate from expected behavior.",
                Tags = ["enterprise-resources", "audit", "compliance", "access-control", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableResourceAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableResourceAudit")],
                DetectOps = [RegOp.CheckDword(Key, "EnableResourceAudit", 1)],
            },
            new TweakDef
            {
                Id = "entres-enforce-resource-access-policies",
                Label = "Enforce Centralized Access Policies for Enterprise Resource Management",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Centralized access policy enforcement ensures that enterprise resource access decisions are made by the central policy engine rather than individual system ACLs which can become inconsistent over time. Centralized access policies allow organizations to define access rules based on user attributes resource sensitivity and context ensuring consistent enforcement across all relevant systems. Policy-based access control allows rapid updating of access rules during security incidents such as revoking access for compromised accounts across all resources simultaneously. Dynamic access control policies can incorporate contextual factors like device health network location and time of day into access decisions providing more nuanced and secure access control. Organizations should test centralized access policies in audit mode before enforcing them to identify access configurations that require adjustment. The policy engine should be highly available and integrated with directory services to ensure that access decisions can be made reliably.",
                Tags = ["enterprise-resources", "centralized-access", "dynamic-access-control", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceCentralizedAccessPolicies", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceCentralizedAccessPolicies")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceCentralizedAccessPolicies", 1)],
            },
            new TweakDef
            {
                Id = "entres-restrict-resource-sharing-to-domain",
                Label = "Restrict Enterprise Resource Sharing to Domain-Authenticated Sessions",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Restricting enterprise resource sharing to domain-authenticated sessions prevents non-domain accounts from accessing enterprise resources shared through the enterprise resource manager. Non-domain accounts include local accounts workgroup accounts and external accounts that have not been validated through the organizational authentication infrastructure. Domain authentication requirement ensures that resource access is tied to organizational identity management which controls account lifecycle and credentials. Resources shared through enterprise resource manager can include network shares printers work resources and device access that should be limited to known and managed identities. The domain restriction applies the organizational security policy and access controls to resource sharing preventing circumvention through local account access. Organizations should audit resource sharing configurations to verify that domain authentication is required for all sensitive enterprise resources.",
                Tags = ["enterprise-resources", "domain-restriction", "authentication", "access-control", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSharingToDomain", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSharingToDomain")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSharingToDomain", 1)],
            },
            new TweakDef
            {
                Id = "entres-enable-data-classification-integration",
                Label = "Enable Data Classification Integration with Enterprise Resource Policy",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Data classification integration allows enterprise resource policy to make access decisions based on the sensitivity classification of the data resource ensuring that highly classified resources have appropriately strict access controls. Resource access policies that incorporate data classification labels can automatically apply stricter controls to sensitive or regulated data without requiring manual policy configuration for each resource. Classification-aware access control ensures that as data sensitivity increases the access controls around that data automatically tighten to apply appropriate protections. Integration with data loss prevention systems allows classification labels to also drive DLP policy enforcement decisions for enterprise resources. Organizations should establish a consistent data classification taxonomy that aligns with their regulatory obligations and risk management requirements. Classification labels should be applied consistently and the automated policy enforcement should be tested to verify that classification-based access decisions function as expected.",
                Tags = ["enterprise-resources", "data-classification", "access-control", "compliance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableDataClassificationIntegration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDataClassificationIntegration")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDataClassificationIntegration", 1)],
            },
            new TweakDef
            {
                Id = "entres-configure-resource-manager-logging",
                Label = "Configure Verbose Logging for Enterprise Resource Manager Operations",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Verbose logging for enterprise resource manager captures detailed operational events including policy evaluation access decisions and configuration changes providing comprehensive visibility into resource management operations. Detailed logs enable troubleshooting of access issues where users are denied access to resources they should have access to or granted access to resources they should not. Resource manager operation logs should include policy evaluation results that explain why access was granted or denied based on the attributes evaluated. Organizations should forward resource manager logs to centralized log management for correlation with other security and operational telemetry. Log retention for resource manager operations should be at least 90 days to support incident investigation and compliance auditing. Verbose logging has a minor performance impact on resource access operations and the verbosity level should be validated against performance requirements.",
                Tags = ["enterprise-resources", "logging", "operational-visibility", "troubleshooting", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableVerboseLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableVerboseLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableVerboseLogging", 1)],
            },
            new TweakDef
            {
                Id = "entres-enforce-resource-expiration-policy",
                Label = "Enforce Expiration Policy for Temporary Enterprise Resource Access Grants",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Resource access expiration policy ensures that temporary access grants automatically expire after their configured lifetime preventing indefinite accumulation of access permissions that are no longer required. Just-in-time access grants should expire automatically after the authorized time period without requiring manual intervention to remove the access. Organizations that grant temporary elevated access for specific tasks or time-limited projects benefit from automatic expiration to prevent those grants from remaining active after the justification expires. Expiration of temporary access is a key principle of zero standing privilege architectures where no accounts maintain persistent access to sensitive resources. The policy should configure appropriate default expiration periods for different types of resource access and should alert administrators when access is about to expire for review. Organizations should regularly audit active access grants to identify any that have excessive lifetimes or that should be expired.",
                Tags = ["enterprise-resources", "access-expiration", "just-in-time", "least-privilege", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceAccessExpirationPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceAccessExpirationPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceAccessExpirationPolicy", 1)],
            },
            new TweakDef
            {
                Id = "entres-block-cross-tenant-resource-access",
                Label = "Block Enterprise Resource Access from External Tenant Identities",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Blocking cross-tenant resource access prevents external organizational identities from accessing enterprise resources without explicit authorization through configured cross-tenant access policies. External tenant identities from partner organizations guest accounts and contractor identities from different tenants should only access enterprise resources when explicitly granted through formal access review processes. Unrestricted cross-tenant access can allow resources to be reached by compromised accounts from partner organizations that have not implemented equivalent security controls. The policy enforces that any cross-tenant access must be explicitly configured rather than allowed by default based on federation trust relationships. Organizations that have legitimate cross-tenant collaboration requirements should configure specific cross-tenant access policies that grant the minimum required access to known and trusted partner identities. Regular review of cross-tenant access grants ensures that access is revoked when business relationships and collaboration requirements change.",
                Tags = ["enterprise-resources", "cross-tenant", "external-access", "identity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockCrossTenantResourceAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockCrossTenantResourceAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockCrossTenantResourceAccess", 1)],
            },
            new TweakDef
            {
                Id = "entres-enforce-resource-location-restriction",
                Label = "Restrict Enterprise Resource Access to Organizational Network Locations",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Network location restrictions for enterprise resource access limit access to authorized network locations such as corporate network subnets and VPN connections preventing access from arbitrary internet locations. Location-based access restrictions add a context-aware control layer that complements identity-based controls by requiring that access come from known trusted infrastructure. Compromised credentials used from external locations are blocked by network location restrictions providing detection and blocking of credential theft that is used away from corporate infrastructure. Organizations should configure location-based restrictions to include both physical office networks and VPN connections that remote workers and administrators use. The policy should be implemented alongside conditional access policies to provide a consistent location-aware access control framework. Regular review of allowed network locations ensures that the list remains current as network infrastructure changes.",
                Tags = ["enterprise-resources", "network-location", "conditional-access", "credential-theft", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceNetworkLocationRestriction", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceNetworkLocationRestriction")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceNetworkLocationRestriction", 1)],
            },
            new TweakDef
            {
                Id = "entres-enable-resource-health-monitoring",
                Label = "Enable Health Monitoring for Enterprise Resource Availability and Integrity",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enterprise resource health monitoring tracks the availability integrity and performance of managed resources providing early warning of resource degradation ahead of user-impacting failures. Resource health data helps distinguish between security incidents that cause resource degradation and operational causes allowing appropriate response procedures to be initiated quickly. Monitoring for unexpected resource configuration changes provides detection for insider threat and attacker activity that modifies resource settings to expand access or disrupt operations. Automated health checks should verify that resource access controls are intact that resources are accessible to authorized users and that resource configurations match their expected baselines. Health monitoring alerts should be integrated with incident management and change management processes to ensure timely and appropriate responses. Regular health monitoring reviews help identify systemic issues in resource management that require architectural changes.",
                Tags = ["enterprise-resources", "health-monitoring", "availability", "integrity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableResourceHealthMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableResourceHealthMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "EnableResourceHealthMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "entres-enforce-resource-naming-standards",
                Label = "Enforce Naming Standards for Enterprise Resource Registration and Discovery",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Resource naming standards enforcement ensures that enterprise resources conform to organizational naming conventions that encode ownership classification and lifecycle information in the resource name. Consistent naming conventions enable automated policy application based on resource names including applying different security policies based on naming conventions that indicate the resource's environment or sensitivity. Naming standard enforcement prevents the creation of resources with ambiguous or misleading names that could cause misapplication of security policies. The naming convention should include indicators for environment production vs. test sensitivity classification owning team or department and creation date. Automated validation of resource names against the naming standard at registration time prevents non-compliant resources from being added to the enterprise resource inventory. Regular scanning of existing resources for naming standard compliance helps identify legacy resources that require remediation.",
                Tags = ["enterprise-resources", "naming-standards", "governance", "policy-automation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceResourceNamingStandards", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceResourceNamingStandards")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceResourceNamingStandards", 1)],
            },
        ];
    }

    // ── EnterpriseStateRoamingPolicy ──
    private static class _EnterpriseStateRoamingPolicy
    {
        private const string SyncKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "esroam-disable-password-sync",
                Label = "Enterprise State Roaming: Disable Password Settings Sync",
                Category = "System",
                Description =
                    "Prevents Windows credential and password hashes from being synchronized through the Enterprise State Roaming channel to Azure AD cloud storage. Password roaming via ESR is distinct from Azure AD seamless SSO and may involve credential material being persisted in a cloud-accessible store. In high-security environments, all credential handling must be on-premises only.",
                Tags = ["state roaming", "sync", "password", "credentials", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisablePasswordSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisablePasswordSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisablePasswordSettingSync", 2)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents credential data from roaming via ESR; complements Azure AD SSPR policy.",
            },
            new TweakDef
            {
                Id = "esroam-disable-app-sync-setting",
                Label = "Enterprise State Roaming: Disable App Sync via ESR Channel",
                Category = "System",
                Description =
                    "Disables the specific ESR sync provider for application data packages (UWP AppX app state, configuration blobs stored in the cloud). App sync allows UWP apps to restore their last-used state—including user-typed data—when the same account signs in on another device. For apps that handle sensitive data (forms, documents), roaming this state creates residual data in Azure.",
                Tags = ["state roaming", "sync", "app data", "uwp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableAppSyncSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableAppSyncSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableAppSyncSettingSync", 2)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops UWP app state from syncing across devices; each device retains independent app state.",
            },
            new TweakDef
            {
                Id = "esroam-block-user-override-app-sync",
                Label = "Enterprise State Roaming: Block User Override of App Settings Sync Policy",
                Category = "System",
                Description =
                    "Prevents users from overriding the Group Policy that disables application settings synchronization. The Windows sync settings UI allows users to individually toggle sync categories; this policy forces DisableApplicationSettingSync to be admin-enforced and uneditable, ensuring corporate devices cannot roam application configuration data regardless of user preference.",
                Tags = ["state roaming", "sync", "app settings", "user override", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableApplicationSettingSyncUserOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableApplicationSettingSyncUserOverride")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableApplicationSettingSyncUserOverride", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents user from re-enabling app sync; admin policy is the only control.",
            },
            new TweakDef
            {
                Id = "esroam-block-user-override-start-layout",
                Label = "Enterprise State Roaming: Block User Override of Start Layout Sync Policy",
                Category = "System",
                Description =
                    "Prevents users from re-enabling Start menu layout synchronization after it has been disabled by Group Policy. In environments with GPO-deployed Start menus, users should not be able to revert to a cloud-synced layout that was potentially configured on a personal device or a different organizational unit, as this undermines the standardized desktop configuration management.",
                Tags = ["state roaming", "sync", "start menu", "user override", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableStartLayoutSettingSyncUserOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableStartLayoutSettingSyncUserOverride")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableStartLayoutSettingSyncUserOverride", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents user from re-enabling Start layout sync; enforces the IT-deployed Start menu.",
            },
            new TweakDef
            {
                Id = "esroam-disable-device-account-sync",
                Label = "Enterprise State Roaming: Disable Device Account Settings Sync",
                Category = "System",
                Description =
                    "Disables synchronization of device account settings (Microsoft account email app state, mail account configuration, calendar sync settings) through Enterprise State Roaming. On managed corporate devices where mail clients are configured centrally via MDM profiles or Exchange Autodiscover, preventing cloud-roaming of account settings avoids conflicts between centrally-pushed and user-synced configurations.",
                Tags = ["state roaming", "sync", "device account", "email", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableDeviceAccountSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableDeviceAccountSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableDeviceAccountSettingSync", 2)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Stops device account (email/calendar config) from syncing; MDM-managed profiles are unaffected.",
            },
        ];
    }

    // ── GpoFolderRedirPolicy ──
    private static class _GpoFolderRedirPolicy
    {
        private const string SystemKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string LogonKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Logon";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "folderredir-enable-cache-rename",
                Label = "GPO Folder Redirection: Enable Cache Rename on Redirect",
                Category = "System",
                Description =
                    "Enables cache renaming when a redirected folder path changes. When a folder redirection target is updated via Group Policy (e.g., moving a redirected My Documents share from an old file server to a new one), Windows can seamlessly rename the local offline-files cache entry to match the new UNC path. Without this setting, the client cache may retain stale entries pointing to the old server, causing offline file sync conflicts.",
                Tags = ["folder redirection", "offline files", "gpo", "cache", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "FolderRedirectionEnableCacheRename", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "FolderRedirectionEnableCacheRename")],
                DetectOps = [RegOp.CheckDword(SystemKey, "FolderRedirectionEnableCacheRename", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Enables clean cache transitions when redirection targets change; reduces sync errors after file server migrations.",
            },
            new TweakDef
            {
                Id = "folderredir-disable-user-profile-roaming",
                Label = "GPO Folder Redirection: Disable User Profile Roaming Download",
                Category = "System",
                Description =
                    "Prevents Windows from downloading a roaming user profile from the network during logon when the user profile server is unavailable. Without this policy, Windows waits for the roaming profile to download (up to the profile server timeout) before allowing login. Blocking this fallback download prevents slow logons when profile servers are down while ensuring that local cached profiles are used immediately.",
                Tags = ["folder redirection", "roaming profile", "logon", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "DisableRoamingProfileDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "DisableRoamingProfileDownload")],
                DetectOps = [RegOp.CheckDword(SystemKey, "DisableRoamingProfileDownload", 1)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Skips roaming profile download when server unavailable; users get cached profile with no network wait.",
            },
            new TweakDef
            {
                Id = "folderredir-disable-roaming-profile-quota-notification",
                Label = "GPO Folder Redirection: Disable Roaming Profile Quota Warning Notification",
                Category = "System",
                Description =
                    "Suppresses the roaming profile quota warning balloon notification that appears in the notification area when a user's roaming profile approaches its storage quota. In enterprise environments where profile size is managed through other mechanisms (e.g., folder redirection, profile monitoring tools), these notifications create user confusion and help desk calls without providing actionable guidance for end users.",
                Tags = ["folder redirection", "roaming profile", "quota", "notification", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "DisableProfileQuotaNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "DisableProfileQuotaNotification")],
                DetectOps = [RegOp.CheckDword(SystemKey, "DisableProfileQuotaNotification", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses quota balloon notification; profile quota is still enforced by the file server.",
            },
            new TweakDef
            {
                Id = "folderredir-wait-for-policy-at-logon",
                Label = "GPO Folder Redirection: Wait for Group Policy at Logon (Background Sync Block)",
                Category = "System",
                Description =
                    "Forces Windows to perform a synchronous (blocking) Group Policy application at logon rather than applying folder redirection policies in the background after the user is already logged in. Without this setting, users may briefly see their unredirected local Desktop and Documents folders before the redirection takes effect, resulting in files being saved to the wrong location. Synchronous policy application eliminates this window.",
                Tags = ["folder redirection", "group policy", "logon", "synchronous", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "WaitForPolicyToBeApplied", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "WaitForPolicyToBeApplied")],
                DetectOps = [RegOp.CheckDword(SystemKey, "WaitForPolicyToBeApplied", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Adds ~2–5 seconds to first logon on each new GPO change; prevents split-second folder location confusion.",
            },
            new TweakDef
            {
                Id = "folderredir-redirect-local-profile-to-network",
                Label = "GPO Folder Redirection: Grant User Exclusive Rights to Redirected Folder",
                Category = "System",
                Description =
                    "Grants the user exclusive NTFS permissions on their redirected folder target when the folder redirection policy first creates it on the file server. Without this setting, the Administrators group retains access to all redirected folders, enabling administrators to read user-redirected documents. Granting exclusive user rights is a privacy and security best practice that ensures sensitive user data in redirected folders is only accessible to the owning account.",
                Tags = ["folder redirection", "permissions", "ntfs", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "GrantExclusiveRightsToFolderRedirection", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "GrantExclusiveRightsToFolderRedirection")],
                DetectOps = [RegOp.CheckDword(SystemKey, "GrantExclusiveRightsToFolderRedirection", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Grants user-only NTFS permissions on their redirected share; admins must use elevated credentials to access.",
            },
            new TweakDef
            {
                Id = "folderredir-use-localized-subfolder-names",
                Label = "GPO Folder Redirection: Use Localized Subfolder Names",
                Category = "System",
                Description =
                    "Configures folder redirection to use the localized (OS language-specific) names for redirected subfolders on the file server rather than English names. In multi-language organizations where different users log on with different Windows UI languages, the names of redirected subfolder paths (e.g., Documents vs. Documenti vs. Dokumente) can vary unless this policy standardizes them to the localized folder names per user.",
                Tags = ["folder redirection", "localization", "subfolder names", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "UseLocalizedSubfolderNames", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "UseLocalizedSubfolderNames")],
                DetectOps = [RegOp.CheckDword(SystemKey, "UseLocalizedSubfolderNames", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Changes redirected subfolder names to match the user's Windows language; no data loss, only path naming change.",
            },
            new TweakDef
            {
                Id = "folderredir-move-contents-on-redirect",
                Label = "GPO Folder Redirection: Move Contents to Redirected Path",
                Category = "System",
                Description =
                    "Instructs Windows to automatically move the contents of a folder from its original local location to the new UNC redirect target when folder redirection is first applied. Without this policy, existing local files stay in place and only new files go to the redirect target, leaving users with data split across two locations. Enabling content migration ensures a complete transition to the managed file server path.",
                Tags = ["folder redirection", "content migration", "data move", "gpo", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "MoveContentsExistingFolders", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "MoveContentsExistingFolders")],
                DetectOps = [RegOp.CheckDword(SystemKey, "MoveContentsExistingFolders", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Moves existing local files to the redirect target on first policy application; ensure the target share has sufficient space.",
            },
            new TweakDef
            {
                Id = "folderredir-disable-unc-path-hardening-bypass",
                Label = "GPO Folder Redirection: Block UNC Hardening Bypass for Redirected Paths",
                Category = "System",
                Description =
                    "Prevents applications from bypassing UNC path hardening (SMB signing requirements) for redirected folder UNC targets. Windows allows some UNC access to bypass signing requirements for specific paths. This policy ensures that even though folder redirection targets are trusted by Windows, they are still subject to SMB signing requirements to prevent man-in-the-middle attacks on the file server connection carrying redirected folder traffic.",
                Tags = ["folder redirection", "unc hardening", "smb signing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "DisableUNCHardeningForRedirectedPaths", 0)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "DisableUNCHardeningForRedirectedPaths")],
                DetectOps = [RegOp.CheckDword(SystemKey, "DisableUNCHardeningForRedirectedPaths", 0)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Keeps UNC hardening active for redirect paths (sets to 0 = hardening NOT disabled); SMB signing is enforced.",
            },
            new TweakDef
            {
                Id = "folderredir-configure-profile-slow-link-detection",
                Label = "GPO Folder Redirection: Configure Slow-Link Detection Threshold",
                Category = "System",
                Description =
                    "Configures the network bandwidth threshold below which Windows considers the connection to the roaming profile server as a 'slow link', triggering use of the local cached profile instead of downloading the full remote profile. Setting this to the Microsoft-recommended value of 500 kbps ensures that even on moderate WAN links, users get fast logons while good connections still get the full roaming/redirected experience.",
                Tags = ["folder redirection", "slow link", "profile", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [LogonKey],
                ApplyOps = [RegOp.SetDword(LogonKey, "SlowLinkDetect", 500)],
                RemoveOps = [RegOp.DeleteValue(LogonKey, "SlowLinkDetect")],
                DetectOps = [RegOp.CheckDword(LogonKey, "SlowLinkDetect", 500)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Sets 500 kbps as slow-link threshold; connections below this use cached profile for faster logon.",
            },
            new TweakDef
            {
                Id = "folderredir-enable-profile-migration-on-domain-join",
                Label = "GPO Folder Redirection: Enable Local Profile Migration on Domain Join",
                Category = "System",
                Description =
                    "Permits migration of the local user profile to the roaming profile path when a user first logs in after a machine is joined to a domain. Without this policy, domain logons create a new empty profile and the user's existing local profile data (desktop files, AppData settings) is left behind in the local profile. Enabling migration ensures the first domain logon seamlessly carries over all existing local user data.",
                Tags = ["folder redirection", "profile migration", "domain join", "logon", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [LogonKey],
                ApplyOps = [RegOp.SetDword(LogonKey, "LocalProfileLoadTimeOut", 30)],
                RemoveOps = [RegOp.DeleteValue(LogonKey, "LocalProfileLoadTimeOut")],
                DetectOps = [RegOp.CheckDword(LogonKey, "LocalProfileLoadTimeOut", 30)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Sets a 30-second wait period for profile load before falling back to local cache on domain join.",
            },
        ];
    }

    // ── GpoScriptsPolicy ──
    private static class _GpoScriptsPolicy
    {
        private const string SystemKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "gposcripts-run-logon-script-sync",
                Label = "GPO Scripts: Run Logon Scripts Synchronously",
                Category = "System",
                Description =
                    "Configures Windows to run all Group Policy logon scripts synchronously and display the desktop only after all logon scripts have completed. By default, Windows may display the desktop before all logon scripts finish, which can result in users opening applications before drive mappings, printer connections, or environment variables are established by logon scripts. Synchronous execution ensures scripts complete before the user session is accessible.",
                Tags = ["gpo scripts", "logon scripts", "synchronous", "startup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "RunLogonScriptSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "RunLogonScriptSync")],
                DetectOps = [RegOp.CheckDword(SystemKey, "RunLogonScriptSync", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Delays desktop display until all logon scripts complete; increases first-logon time by the duration of all scripts.",
            },
            new TweakDef
            {
                Id = "gposcripts-run-startup-script-sync",
                Label = "GPO Scripts: Run Startup Scripts Synchronously",
                Category = "System",
                Description =
                    "Configures Windows to run all Computer Configuration startup scripts one at a time and sequentially before the logon prompt appears. Without this setting, startup scripts may run asynchronously in the background, meaning critical system initialization scripts (e.g., disk encryption unlock, certificate enrollment, MDM check-in) may not complete before a user logs in, potentially resulting in incomplete system state at logon.",
                Tags = ["gpo scripts", "startup scripts", "synchronous", "boot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "RunStartupScriptSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "RunStartupScriptSync")],
                DetectOps = [RegOp.CheckDword(SystemKey, "RunStartupScriptSync", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote =
                    "Ensures each startup script finishes before the next begins and before the logon screen appears; adds boot time equal to total startup script duration.",
            },
            new TweakDef
            {
                Id = "gposcripts-run-legacy-logon-hidden",
                Label = "GPO Scripts: Run Legacy Logon Scripts Visible but Silent",
                Category = "System",
                Description =
                    "Forces legacy logon scripts (those defined in the user profile properties of Active Directory) to run visible to the user but without a separate CMD window. By default, legacy logon scripts (as distinct from Group Policy logon scripts) may flash console windows briefly. This policy suppresses the command prompt window while still allowing the script to run, providing a cleaner logon experience without confusing users with flashing black windows.",
                Tags = ["gpo scripts", "logon scripts", "legacy", "hidden window", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "RunLegacyLogonScriptHidden", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "RunLegacyLogonScriptHidden")],
                DetectOps = [RegOp.CheckDword(SystemKey, "RunLegacyLogonScriptHidden", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses a CMD window during legacy logon scripts; scripts still execute — purely a UX improvement.",
            },
            new TweakDef
            {
                Id = "gposcripts-set-max-script-wait-time",
                Label = "GPO Scripts: Set Maximum Script Runtime Timeout (10 minutes)",
                Category = "System",
                Description =
                    "Sets the maximum time Windows will wait for a Group Policy script (startup, logon, logoff, or shutdown) to complete before forcibly terminating it. The default is 600 seconds (10 minutes). Scripts that exceed this timeout are terminated without completing. Setting this explicitly prevents runaway scripts from hanging the logon/logoff sequence indefinitely, which can leave the machine in an unresponsive state.",
                Tags = ["gpo scripts", "timeout", "max wait", "startup shutdown", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "MaxGPOScriptWait", 600)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "MaxGPOScriptWait")],
                DetectOps = [RegOp.CheckDword(SystemKey, "MaxGPOScriptWait", 600)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Explicitly sets the 600-second (10 min) script timeout; runaway scripts are forcibly terminated after this period.",
            },
            new TweakDef
            {
                Id = "gposcripts-hide-startup-scripts",
                Label = "GPO Scripts: Hide Startup Scripts (No Status Message)",
                Category = "System",
                Description =
                    "Suppresses the 'Running startup scripts...' status message and progress screen that Windows displays during boot when Group Policy startup scripts are executing. In environments where startup scripts handle sensitive operations (certificate enrollment, TPM initialization commands, encrypted volume mounting), displaying the script status messages onscreen may expose the types of security operations to anyone observing the boot screen.",
                Tags = ["gpo scripts", "startup scripts", "hidden", "status message", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "HideStartupScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "HideStartupScripts")],
                DetectOps = [RegOp.CheckDword(SystemKey, "HideStartupScripts", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the startup script progress screen; scripts still run, but no progress message is shown at boot.",
            },
            new TweakDef
            {
                Id = "gposcripts-hide-logon-scripts",
                Label = "GPO Scripts: Hide Logon Scripts (No Progress Window)",
                Category = "System",
                Description =
                    "Hides the 'Applying your personal settings...' and similar logon script progress messages that appear during user logon in verbose mode. While informative for administrators, these messages reveal that Group Policy logon scripts are running, potentially exposing script categories to end users. In secure environments, the logon process should be opaque — completing silently and presenting the desktop only when ready.",
                Tags = ["gpo scripts", "logon scripts", "hidden", "progress window", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "HideLogonScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "HideLogonScripts")],
                DetectOps = [RegOp.CheckDword(SystemKey, "HideLogonScripts", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses the logon script progress window; logon may appear to 'hang' briefly — this is expected behavior.",
            },
            new TweakDef
            {
                Id = "gposcripts-hide-logoff-scripts",
                Label = "GPO Scripts: Hide Logoff Scripts (No Logoff Window)",
                Category = "System",
                Description =
                    "Suppresses the window that appears when Group Policy logoff scripts are executing at user sign-out. When logoff scripts clean up user sessions (removing temp credentials, wiping browser profiles, revoking certificates), showing the progress window to the user is unnecessary and can lead users to terminate the logoff early by pressing the power button, potentially leaving cleanup scripts incomplete.",
                Tags = ["gpo scripts", "logoff scripts", "hidden", "sign-out", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "HideLogoffScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "HideLogoffScripts")],
                DetectOps = [RegOp.CheckDword(SystemKey, "HideLogoffScripts", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides logoff script progress; users see the default 'Signing out...' overlay while scripts complete in the background.",
            },
            new TweakDef
            {
                Id = "gposcripts-hide-shutdown-scripts",
                Label = "GPO Scripts: Hide Shutdown Scripts (Silent System Shutdown)",
                Category = "System",
                Description =
                    "Suppresses the shutdown script progress window that shows when Group Policy Computer Configuration shutdown scripts run during system power-down. Shutdown scripts commonly perform operations such as disk encryption key cleanup, network session teardown, and compliance logging. Hiding the progress window provides a cleaner shutdown experience and prevents disclosure of the shutdown script sequence to onlookers.",
                Tags = ["gpo scripts", "shutdown scripts", "hidden", "system shutdown", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "HideShutdownScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "HideShutdownScripts")],
                DetectOps = [RegOp.CheckDword(SystemKey, "HideShutdownScripts", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides shutdown script progress; the machine may appear to shut down more slowly as scripts run silently.",
            },
            new TweakDef
            {
                Id = "gposcripts-run-scripts-first-at-user-logon",
                Label = "GPO Scripts: Run User Logon Scripts Before Group Policy Logon Scripts",
                Category = "System",
                Description =
                    "Forces user-level logon scripts (defined in profile properties) to run before the Group Policy client completes processing Computer and User Configuration logon scripts. In some deployment scenarios, user-specific scripts (which map personal drives or configure user-specific settings) must run before broader GPO changes are applied. This ordering ensures user context is established before group-level policies modify the environment.",
                Tags = ["gpo scripts", "logon scripts", "run order", "user scripts", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "RunScriptsFirstAtUserLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "RunScriptsFirstAtUserLogon")],
                DetectOps = [RegOp.CheckDword(SystemKey, "RunScriptsFirstAtUserLogon", 1)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote =
                    "Reorders script execution — user profile scripts run before GPO logon scripts; may expose environment to user scripts before GPO lockdown is applied.",
            },
            new TweakDef
            {
                Id = "gposcripts-set-max-noninteractive-runtime",
                Label = "GPO Scripts: Set Maximum Non-Interactive Script Runtime (5 minutes)",
                Category = "System",
                Description =
                    "Sets the maximum time a non-interactive Group Policy script (startup, shutdown, logon, logoff scripts running in non-interactive mode) is allowed to run. Setting this to 300 seconds (5 minutes) provides a tighter timeout than the default 600 seconds. For background scripts that should complete quickly, this reduces the window during which a script error or infinite loop delays the logon or shutdown sequence.",
                Tags = ["gpo scripts", "timeout", "non-interactive", "runtime limit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "MaxNonInteractiveRunningTime", 300)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "MaxNonInteractiveRunningTime")],
                DetectOps = [RegOp.CheckDword(SystemKey, "MaxNonInteractiveRunningTime", 300)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Sets a 5-minute (300s) cap on non-interactive scripts; scripts that need more time must run interactively.",
            },
        ];
    }

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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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

