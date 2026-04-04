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
                    Id = "clipadv-disable-clipboard-history",
                    Label = "Disable Clipboard History Feature",
                    Category = "System",
                    Description =
                        "Sets AllowClipboardHistory=0 to disable the Windows clipboard history feature (Win+V), preventing clipboard contents from being stored in history.",
                    Tags = ["clipboard", "history", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Disables clipboard history; Win+V no longer shows recent clipboard items.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowClipboardHistory", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowClipboardHistory")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowClipboardHistory", 0)],
                },
                new TweakDef
                {
                    Id = "clipadv-disable-cross-device-sync",
                    Label = "Disable Cross-Device Clipboard Sync",
                    Category = "System",
                    Description =
                        "Sets AllowCrossDeviceClipboard=0 to prevent clipboard content from syncing between devices linked to the same Microsoft account via the cloud.",
                    Tags = ["clipboard", "sync", "cloud", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard contents stay on-device; no cross-device sync via Microsoft account.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowCrossDeviceClipboard", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowCrossDeviceClipboard")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowCrossDeviceClipboard", 0)],
                },
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
                    Id = "cdump-disable-crash-dump",
                    Label = "Disable Crash Dump Generation",
                    Category = "System",
                    Description =
                        "Sets CrashDumpEnabled=0 to disable creation of any memory dump file when Windows encounters a stop error (BSOD). Frees disk space on constrained devices and prevents large dump files from accumulating. Default: 7 (auto).",
                    Tags = ["crash dump", "bsod", "memory", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 3,
                    SafetyRating = 2,
                    ImpactNote = "Disables all crash dumps; BSODs produce no diagnostic artifacts. Reduces disk use but hampers debugging.",
                    ApplyOps = [RegOp.SetDword(CcKey, "CrashDumpEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(CcKey, "CrashDumpEnabled")],
                    DetectOps = [RegOp.CheckDword(CcKey, "CrashDumpEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "cdump-set-mini-dump",
                    Label = "Set Crash Dump to Minidump Only",
                    Category = "System",
                    Description =
                        "Sets CrashDumpEnabled=3 to configure Windows to write only a small minidump (~256 KB) on stop errors instead of a full or kernel memory dump. Balances debuggability with disk usage. Default: 7 (auto).",
                    Tags = ["crash dump", "minidump", "bsod", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "256 KB minidump on BSOD; sufficient for driver crash analysis with low disk overhead.",
                    ApplyOps = [RegOp.SetDword(CcKey, "CrashDumpEnabled", 3)],
                    RemoveOps = [RegOp.DeleteValue(CcKey, "CrashDumpEnabled")],
                    DetectOps = [RegOp.CheckDword(CcKey, "CrashDumpEnabled", 3)],
                },
                new TweakDef
                {
                    Id = "cdump-disable-auto-reboot",
                    Label = "Disable Automatic Reboot on BSOD",
                    Category = "System",
                    Description =
                        "Sets AutoReboot=0 to prevent Windows from automatically restarting immediately after a stop error. The system halts at the blue screen allowing the error code to be read. Useful for physical machines and servers.",
                    Tags = ["crash dump", "auto reboot", "bsod", "server", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "System stays at BSOD screen until manually rebooted; stop code visible for diagnosis.",
                    ApplyOps = [RegOp.SetDword(CcKey, "AutoReboot", 0)],
                    RemoveOps = [RegOp.DeleteValue(CcKey, "AutoReboot")],
                    DetectOps = [RegOp.CheckDword(CcKey, "AutoReboot", 0)],
                },
                new TweakDef
                {
                    Id = "cdump-disable-log-event",
                    Label = "Disable BSOD Event Log Entry",
                    Category = "System",
                    Description =
                        "Sets LogEvent=0 to prevent Windows from writing an event log entry to the System log when a stop error occurs. Default: 1 (log enabled). Reduces event log verbosity on systems with frequent crash-recovery cycles.",
                    Tags = ["crash dump", "event log", "bsod", "logging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 2,
                    SafetyRating = 3,
                    ImpactNote = "No event log entry on BSOD; reduces auditability of stop errors.",
                    ApplyOps = [RegOp.SetDword(CcKey, "LogEvent", 0)],
                    RemoveOps = [RegOp.DeleteValue(CcKey, "LogEvent")],
                    DetectOps = [RegOp.CheckDword(CcKey, "LogEvent", 0)],
                },
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
                    Id = "cdump-overwrite-existing-dump",
                    Label = "Overwrite Existing Crash Dump",
                    Category = "System",
                    Description =
                        "Sets Overwrite=1 so that each new crash dump overwrites the previous dump file rather than keeping multiple copies. Prevents disk space exhaustion on managed devices that experience occasional stop errors.",
                    Tags = ["crash dump", "overwrite", "disk", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "New BSOD dump replaces old one; only most recent crash is retained on disk.",
                    ApplyOps = [RegOp.SetDword(CcKey, "Overwrite", 1)],
                    RemoveOps = [RegOp.DeleteValue(CcKey, "Overwrite")],
                    DetectOps = [RegOp.CheckDword(CcKey, "Overwrite", 1)],
                },
                new TweakDef
                {
                    Id = "cdump-disable-filter-pages",
                    Label = "Disable Crash Dump Page Filtering",
                    Category = "System",
                    Description =
                        "Sets FilterPages=0 to disable the Windows crash dump page-filtering feature that removes unnecessary memory pages before writing the dump file. Produces more complete dumps at the cost of larger file size. Default: 1.",
                    Tags = ["crash dump", "filter", "memory", "debugging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Full unfiltered memory dumps; larger files but more useful for deep kernel analysis.",
                    ApplyOps = [RegOp.SetDword(CcKey, "FilterPages", 0)],
                    RemoveOps = [RegOp.DeleteValue(CcKey, "FilterPages")],
                    DetectOps = [RegOp.CheckDword(CcKey, "FilterPages", 0)],
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
                    Id = "wmpol-prevent-codec-download",
                    Label = "WMP: Prevent Automatic Codec Download from the Internet",
                    Category = "System",
                    Description =
                        "Sets PreventCodecDownload=1 in Windows Media Player policy. Prevents Windows Media Player from automatically downloading codecs from the internet to play media files that use unknown or missing codecs. Automatic codec download has historically been a malware delivery vector: specially crafted media files embedded codec 'requirements' that redirected to malicious codec installer EXEs from attacker-controlled servers rather than legitimate codec repositories. "
                        + "The drive-by codec attack vector was prevalent in the Windows XP/Vista era: opening a video file triggers WMP's codec detection, which displays a dialog offering to download from a URL embedded in the media file's codec detection field — which can point to any server. Modern enterprise security policies require that all software (including codecs) be installed through approved channels (SCCM, Intune). Blocking automatic codec download ensures users cannot inadvertently install unapproved software via a malicious media file.",
                    Tags = ["wmpol", "windows-media-player", "codec", "download", "malware", "drive-by"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "WMP cannot auto-download codecs. Users will see 'codec missing' error for unsupported formats. Enterprise codec deployments via SCCM/Intune unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventCodecDownload", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventCodecDownload")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventCodecDownload", 1)],
                },
                new TweakDef
                {
                    Id = "wmpol-disable-auto-update",
                    Label = "WMP: Disable Windows Media Player Automatic Online Update Check",
                    Category = "System",
                    Description =
                        "Sets DisableAutoUpdate=1 in Windows Media Player policy. Prevents Windows Media Player from automatically checking for updates and new functionality online. WMP update checks contact Microsoft servers on every WMP launch, contributing to outbound telemetry traffic and potentially introducing version changes to a controlled software baseline. "
                        + "On enterprise-managed systems where application updates are managed by WSUS or SCCM, unsolicited update checks by individual applications create unpredictable patching timelines. WMP updates have in the past introduced new codec support, UI changes, and DRM policy updates that required revalidation by enterprise compatibility teams. Disabling auto-update ensures WMP version state is controlled exclusively by IT-managed patching processes.",
                    Tags = ["wmpol", "windows-media-player", "auto-update", "baseline", "wsus"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "WMP update checks disabled. Updates delivered via WSUS/Windows Update instead of direct WMP check. No functional impact on media playback.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "wmpol-disable-network-settings",
                    Label = "WMP: Prevent User Modification of Network Streaming Settings",
                    Category = "System",
                    Description =
                        "Sets DisableNetworkSettings=1 in Windows Media Player policy. Prevents users from modifying Windows Media Player network settings (streaming protocol selection, proxy configuration, bandwidth usage). On corporate networks, streaming settings should be configured by IT to ensure WMP uses approved proxy settings and consumption limits, preventing users from configuring direct internet streaming paths that bypass proxy inspection. "
                        + "WMP network settings include the ability to configure RTSP and HTTP streaming protocol preferences and proxy exclusion lists. A user who configures WMP to bypass the corporate proxy for streaming sources creates an uninspected traffic path for internet-sourced audio/video streams. Corporate DLP and web filtering policies rely on all internet traffic flowing through the approved proxy for content inspection. Locking WMP network settings prevents direct-to-internet streaming paths.",
                    Tags = ["wmpol", "windows-media-player", "network-settings", "proxy", "streaming", "dlp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "WMP network settings page locked. Streaming uses system proxy settings configured by IT. Users cannot configure alternative streaming protocol paths.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableNetworkSettings", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkSettings")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableNetworkSettings", 1)],
                },
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
                Id = "srgpo-disable-sr-policy",
                Label = "System Restore: Disable System Restore via Group Policy",
                Category = "System",
                Description =
                    "Sets DisableSR=1 in the SystemRestore policy key. Turns off System Restore for all "
                    + "drives and prevents automatic restore point creation. Frees disk space used by VSC.",
                Tags = ["system-restore", "vss", "rollback", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrPol, "DisableSR", 1)],
                RemoveOps = [RegOp.DeleteValue(SrPol, "DisableSR")],
                DetectOps = [RegOp.CheckDword(SrPol, "DisableSR", 1)],
            },
            new TweakDef
            {
                Id = "srgpo-disable-config-policy",
                Label = "System Restore: Lock out System Restore configuration UI",
                Category = "System",
                Description =
                    "Sets DisableConfig=1 in the SystemRestore policy key. Hides the 'Configure' button "
                    + "on the System Protection tab, preventing users from enabling or adjusting SR settings.",
                Tags = ["system-restore", "lockout", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrPol, "DisableConfig", 1)],
                RemoveOps = [RegOp.DeleteValue(SrPol, "DisableConfig")],
                DetectOps = [RegOp.CheckDword(SrPol, "DisableConfig", 1)],
            },
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
                Id = "srgpo-set-max-disk-usage",
                Label = "System Restore: Cap restore-point disk usage at 5 %",
                Category = "System",
                Description =
                    "Sets DiskPercent=5 in SystemRestore settings. Limits the maximum disk space that "
                    + "System Restore shadow copies may consume to 5 % of the system drive.",
                Tags = ["system-restore", "disk-space", "storage", "optimization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "DiskPercent", 5)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "DiskPercent")],
                DetectOps = [RegOp.CheckDword(SrSettings, "DiskPercent", 5)],
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
                    Id = "tsap-set-ntp-type",
                    Label = "Set W32tm Sync Type to NTP",
                    Category = "System",
                    Description =
                        "Sets the W32tm Type value to 'NTP' via policy, instructing the Windows Time service to use NTP time sources only (not domain hierarchy NT5DS). Required on stand-alone machines or workgroup environments.",
                    Tags = ["time sync", "ntp", "w32tm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "W32tm uses NTP sources; bypasses domain hierarchy sync which may cause skew in AD.",
                    ApplyOps = [RegOp.SetString(ParamKey, "Type", "NTP")],
                    RemoveOps = [RegOp.DeleteValue(ParamKey, "Type")],
                    DetectOps = [RegOp.CheckString(ParamKey, "Type", "NTP")],
                },
                new TweakDef
                {
                    Id = "tsap-set-ntp-server",
                    Label = "Set NTP Server to pool.ntp.org",
                    Category = "System",
                    Description =
                        "Sets NtpServer='pool.ntp.org,0x9' as the time source for the Windows Time service via Group Policy. Use '0x9' flags (poll + step). Replaces time.windows.com for environments that prefer public NTP pools.",
                    Tags = ["time sync", "ntp", "ntp server", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "System syncs to pool.ntp.org; suitable for non-domain SOHO and development machines.",
                    ApplyOps = [RegOp.SetString(ParamKey, "NtpServer", "pool.ntp.org,0x9")],
                    RemoveOps = [RegOp.DeleteValue(ParamKey, "NtpServer")],
                    DetectOps = [RegOp.CheckString(ParamKey, "NtpServer", "pool.ntp.org,0x9")],
                },
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
                    Id = "tsap-set-max-pos-phase-correction",
                    Label = "Limit Max Positive Phase Correction (1 h)",
                    Category = "System",
                    Description =
                        "Sets MaxPosPhaseCorrection=3600 (seconds) to cap how far forward the system clock can be stepped in a single sync. Default: 0x7FFFFFFF (unlimited). Prevents accidental large forward clock jumps on domain or NTP misconfiguration.",
                    Tags = ["time sync", "phase correction", "clock jump", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clock cannot jump forward more than 1 hour in one step; protects certificate/Kerberos validity.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxPosPhaseCorrection")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
                },
                new TweakDef
                {
                    Id = "tsap-set-max-neg-phase-correction",
                    Label = "Limit Max Negative Phase Correction (1 h)",
                    Category = "System",
                    Description =
                        "Sets MaxNegPhaseCorrection=3600 (seconds) to cap how far backward the system clock can be stepped. Default: 0x7FFFFFFF (unlimited). Prevents Kerberos ticket invalidation and certificate errors caused by large backward clock jumps.",
                    Tags = ["time sync", "phase correction", "clock", "kerberos", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clock cannot jump back more than 1 hour; protects authentication tokens.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxNegPhaseCorrection")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
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
                    Id = "timepol-set-max-pos-phase-correction",
                    Label = "Set Maximum Positive Time Correction to 3600 Seconds",
                    Category = "System",
                    Description =
                        "Limits the maximum positive time jump that W32TM will accept in a single synchronisation to 3600 seconds (1 hour), preventing an attacker from jumping the system clock forward to expire Kerberos tickets or bypass time-based security checks.",
                    Tags = ["w32time", "time-correction", "max-jump", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Maximum positive NTP clock adjustment limited to 1 hour; large forward time jumps blocked.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxPosPhaseCorrection")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
                },
                new TweakDef
                {
                    Id = "timepol-set-max-neg-phase-correction",
                    Label = "Set Maximum Negative Time Correction to 3600 Seconds",
                    Category = "System",
                    Description =
                        "Limits the maximum negative time jump (clock backward adjustment) to 3600 seconds (1 hour), preventing attacks that move the clock backward to re-validate already-expired certificates or Kerberos tickets.",
                    Tags = ["w32time", "time-correction", "max-jump-backward", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Maximum negative NTP clock adjustment limited to 1 hour; backward time jumps to replay tickets blocked.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxNegPhaseCorrection")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
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
                Id = "wcnpol-disable-registrars",
                Label = "WCN Policy: Disable WCN Registrars",
                Category = "System",
                Description =
                    "Disables all Windows Connect Now (WCN) registrar functionality via Group Policy. WCN allows wireless device configuration over the network — a potential attack vector on corporate Wi-Fi.",
                Tags = ["wcn", "wireless", "wifi", "registrar", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables all WCN registrar functionality; closes wireless config attack vector.",
                RegistryKeys = [RegKey],
                ApplyOps = [RegOp.SetDword(RegKey, "EnableRegistrars", 0)],
                RemoveOps = [RegOp.DeleteValue(RegKey, "EnableRegistrars")],
                DetectOps = [RegOp.CheckDword(RegKey, "EnableRegistrars", 0)],
            },
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
                Id = "wlogon-disable-fast-user-switching",
                Label = "Windows Logon Options: Disable Fast User Switching",
                Category = "System",
                Description =
                    "Prevents multiple user sessions from being simultaneously active via fast user switching. "
                    + "Fast user switching allows a second user to log on without the first user logging off, leaving their session unlocked in memory. "
                    + "This increases attack surface and can violate compliance policies requiring single-session workstations. "
                    + "Removing this policy re-enables fast user switching.",
                Tags = ["logon", "fast-user-switching", "session", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysKey],
                ApplyOps = [RegOp.SetDword(SysKey, "HideFastUserSwitching", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "HideFastUserSwitching")],
                DetectOps = [RegOp.CheckDword(SysKey, "HideFastUserSwitching", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables fast user switching; enforces single-user session workstation compliance.",
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
                Id = "wmplay-disable-network-settings-change",
                Label = "Windows Media Player: Disable Network Settings Changes",
                Category = "System",
                Description =
                    "Prevents WMP users from modifying network configuration in the Windows Media Player settings dialog. "
                    + "Network settings in WMP include proxy configuration, streaming protocol selection, and bandwidth limits. "
                    + "On managed endpoints these settings should be centrally controlled to ensure network traffic complies with organizational policies. "
                    + "Removing this policy re-enables user ability to change WMP network settings.",
                Tags = ["media-player", "network", "settings", "lockdown", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNetworkSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkSettings")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNetworkSettings", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents WMP network config changes; keeps media streaming under organizational control.",
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
                Id = "wmplay-disable-library-sharing",
                Label = "Windows Media Player: Disable Media Library Sharing",
                Category = "System",
                Description =
                    "Prevents Windows Media Player from sharing its media library on the local network through the Windows Media Network Sharing service. "
                    + "WMP library sharing exposes media file metadata and content over the network, which may include corporate training materials, meeting recordings, or other sensitive files. "
                    + "Disabling library sharing reduces lateral movement attack surface from media sharing protocols. "
                    + "Removing this policy allows WMP library sharing to be enabled by users.",
                Tags = ["media-player", "library", "sharing", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventLibrarySharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventLibrarySharing")],
                DetectOps = [RegOp.CheckDword(Key, "PreventLibrarySharing", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks WMP library sharing; reduces network attack surface from media sharing protocols.",
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
                Id = "wmplay-disable-usage-reporting",
                Label = "Windows Media Player: Disable Usage Reporting",
                Category = "System",
                Description =
                    "Prevents Windows Media Player from sending usage reports and playback data to Microsoft. "
                    + "WMP usage reporting transmits data about media formats played, codecs used, and playback errors. "
                    + "On enterprise endpoints, this constitutes unnecessary telemetry that should be disabled in line with data minimization policies. "
                    + "Removing this policy re-enables WMP usage reporting to Microsoft.",
                Tags = ["media-player", "telemetry", "usage-reporting", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventCDDVDMetadataRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventCDDVDMetadataRetrieval")],
                DetectOps = [RegOp.CheckDword(Key, "PreventCDDVDMetadataRetrieval", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables WMP telemetry and CD/DVD metadata requests; reduces media usage reporting.",
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
            new TweakDef
            {
                Id = "wmplay-hide-privacy-tab",
                Label = "Windows Media Player: Hide Privacy Tab in Options",
                Category = "System",
                Description =
                    "Removes the Privacy tab from the Windows Media Player Options dialog. "
                    + "The Privacy tab allows users to modify privacy settings including DRM, usage reporting, and online content lookups. "
                    + "When privacy settings are centrally locked via GPO, hiding the tab prevents users from attempting to change managed settings. "
                    + "Removing this policy restores the Privacy tab in WMP Options.",
                Tags = ["media-player", "privacy", "options-tab", "lockdown", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HidePrivacyTab", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HidePrivacyTab")],
                DetectOps = [RegOp.CheckDword(Key, "HidePrivacyTab", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides WMP Privacy tab; prevents users from modifying centrally managed privacy settings.",
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
                Id = "wmply-no-cd-metadata",
                Label = "WMP: Prevent CD/DVD metadata retrieval from the internet",
                Category = "System",
                Description =
                    "Sets PreventCDDVDMetadataRetrieval=1. Stops Windows Media Player from contacting "
                    + "online databases to retrieve CD/DVD album art, track names, and other metadata.",
                Tags = ["media", "wmp", "metadata", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpLm, "PreventCDDVDMetadataRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpLm, "PreventCDDVDMetadataRetrieval")],
                DetectOps = [RegOp.CheckDword(WmpLm, "PreventCDDVDMetadataRetrieval", 1)],
            },
            new TweakDef
            {
                Id = "wmply-no-music-metadata",
                Label = "WMP: Prevent music file metadata retrieval from the internet",
                Category = "System",
                Description =
                    "Sets PreventMusicFileMetadataRetrieval=1. Blocks WMP from downloading online "
                    + "metadata for music files such as album art, artist info, and lyrics.",
                Tags = ["media", "wmp", "metadata", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpLm, "PreventMusicFileMetadataRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpLm, "PreventMusicFileMetadataRetrieval")],
                DetectOps = [RegOp.CheckDword(WmpLm, "PreventMusicFileMetadataRetrieval", 1)],
            },
            new TweakDef
            {
                Id = "wmply-no-radio-presets",
                Label = "WMP: Prevent internet radio preset retrieval",
                Category = "System",
                Description =
                    "Sets PreventRadioPresetsRetrieval=1. Prevents Windows Media Player from downloading "
                    + "internet radio station presets from online services.",
                Tags = ["media", "wmp", "radio", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpLm, "PreventRadioPresetsRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpLm, "PreventRadioPresetsRetrieval")],
                DetectOps = [RegOp.CheckDword(WmpLm, "PreventRadioPresetsRetrieval", 1)],
            },
            new TweakDef
            {
                Id = "wmply-no-auto-update",
                Label = "WMP: Disable automatic Windows Media Player updates",
                Category = "System",
                Description =
                    "Sets DisableAutoUpdate=1. Prevents Windows Media Player from automatically checking "
                    + "for and downloading software updates in the background.",
                Tags = ["media", "wmp", "update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpLm, "DisableAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpLm, "DisableAutoUpdate")],
                DetectOps = [RegOp.CheckDword(WmpLm, "DisableAutoUpdate", 1)],
            },
            new TweakDef
            {
                Id = "wmply-no-codec-download",
                Label = "WMP: Prevent automatic codec download",
                Category = "System",
                Description =
                    "Sets PreventCodecDownload=1. Stops Windows Media Player from automatically "
                    + "downloading codecs needed to play unsupported media formats.",
                Tags = ["media", "wmp", "codec", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpLm, "PreventCodecDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpLm, "PreventCodecDownload")],
                DetectOps = [RegOp.CheckDword(WmpLm, "PreventCodecDownload", 1)],
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
                Id = "relpol-block-wer-auto-upload",
                Label = "Reliability Policy: Block WER Auto-Upload of Crash Dumps",
                Category = "System",
                Description =
                    "Prevents Windows Error Reporting from automatically uploading minidumps and full memory dumps to Microsoft. Crash dumps can contain sensitive application data, PII, or credentials from process memory.",
                Tags = ["reliability", "wer", "crash-dump", "upload", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents automatic upload of crash dumps; protects PII and credentials in memory.",
                RegistryKeys = [WerKey],
                ApplyOps = [RegOp.SetDword(WerKey, "LoggingDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WerKey, "LoggingDisabled")],
                DetectOps = [RegOp.CheckDword(WerKey, "LoggingDisabled", 1)],
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
                Id = "relpol-limit-wer-queue-count",
                Label = "Reliability Policy: Limit WER Report Queue Size",
                Category = "System",
                Description =
                    "Limits the maximum number of error reports held in the WER queue to a small number. On heavily-used endpoints, an unbounded WER queue can consume significant disk space.",
                Tags = ["reliability", "wer", "queue", "limit", "disk-space", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Caps the WER queue to prevent unbounded disk usage on busy endpoints.",
                RegistryKeys = [WerKey],
                ApplyOps = [RegOp.SetDword(WerKey, "MaxQueueCount", 5)],
                RemoveOps = [RegOp.DeleteValue(WerKey, "MaxQueueCount")],
                DetectOps = [RegOp.CheckDword(WerKey, "MaxQueueCount", 5)],
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
                Id = "timepol-type-ntp",
                Label = "Enforce W32Time sync type NTP (policy)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Configures W32Time to use NTP as the synchronisation source via Group Policy. "
                    + "Type=NTP. Standalone workstations default to NT5DS on domain or NTP when standalone.",
                Tags = ["time", "ntp", "w32time", "policy"],
                ApplyOps = [RegOp.SetString(W32Params, "Type", "NTP")],
                RemoveOps = [RegOp.DeleteValue(W32Params, "Type")],
                DetectOps = [RegOp.CheckString(W32Params, "Type", "NTP")],
            },
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
                Id = "wtime-set-ntp-server",
                Label = "Configure NTP Server to time.windows.com",
                Category = "System",
                Description =
                    "Sets the NtpServer value in the W32time Parameters policy key to 'time.windows.com,0x9'. "
                    + "Configures Windows Time Service to sync from Microsoft's public NTP server using the NT5DS+NTP type. "
                    + "Type=NTP is required for non-domain workstations; domain-joined machines default to NT5DS hierarchy. "
                    + "Default: 'time.windows.com,0x9' (NTP). Recommended: enforce via policy to prevent drift.",
                Tags = ["ntp", "time", "sync", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Forces NTP sync from time.windows.com; system clock stays accurate on non-domain machines.",
                ApplyOps = [RegOp.SetString(ParamKey, "NtpServer", "time.windows.com,0x9")],
                RemoveOps = [RegOp.DeleteValue(ParamKey, "NtpServer")],
                DetectOps = [RegOp.CheckString(ParamKey, "NtpServer", "time.windows.com,0x9")],
            },
            new TweakDef
            {
                Id = "wtime-force-ntp-type",
                Label = "Force Time Sync Type to NTP",
                Category = "System",
                Description =
                    "Sets Type=NTP in the W32time Parameters policy key. "
                    + "Forces the Windows Time Service to use NTP (Network Time Protocol) as the sync source "
                    + "rather than the domain hierarchy (NT5DS) or no sync (NoSync). "
                    + "Essential for workgroup machines to maintain accurate time against external NTP servers. "
                    + "Default: NT5DS on domain, NTP on standalone. Recommended: NTP for all non-domain machines.",
                Tags = ["ntp", "time", "type", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Time service uses NTP for synchronisation; do not set on domain machines (breaks DC hierarchy).",
                ApplyOps = [RegOp.SetString(ParamKey, "Type", "NTP")],
                RemoveOps = [RegOp.DeleteValue(ParamKey, "Type")],
                DetectOps = [RegOp.CheckString(ParamKey, "Type", "NTP")],
            },
            new TweakDef
            {
                Id = "wtime-enable-ntp-client",
                Label = "Enable NTP Client Provider",
                Category = "System",
                Description =
                    "Sets Enabled=1 in the W32time NtpClient TimeProvider policy key. "
                    + "Ensures the built-in NTP client provider is active and allowed to gather time samples "
                    + "from configured NTP servers. Without this, the W32tm service cannot get NTP time. "
                    + "Default: absent (enabled by default). Recommended: 1 to explicitly enforce via policy.",
                Tags = ["ntp", "client", "time", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "NTP client provider explicitly enabled by policy; system will actively sync from NTP servers.",
                ApplyOps = [RegOp.SetDword(NtpClient, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(NtpClient, "Enabled")],
                DetectOps = [RegOp.CheckDword(NtpClient, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "wtime-disable-ntp-server",
                Label = "Disable NTP Server Provider",
                Category = "System",
                Description =
                    "Sets Enabled=0 in the W32time NtpServer TimeProvider policy key. "
                    + "Disables this machine from acting as an NTP server for other clients on the network. "
                    + "Workstations should never serve NTP time to peers; only dedicated time servers or DCs should. "
                    + "Default: absent (NTP server role disabled on non-DC machines). Recommended: 0 on workstations.",
                Tags = ["ntp", "server", "time", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "This machine will not serve NTP time to other clients; inbound NTP queries are ignored.",
                ApplyOps = [RegOp.SetDword(NtpServer, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(NtpServer, "Enabled")],
                DetectOps = [RegOp.CheckDword(NtpServer, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "wtime-set-poll-interval-6",
                Label = "Set NTP Poll Interval to 64 Seconds (Accurate)",
                Category = "System",
                Description =
                    "Sets SpecialPollInterval=64 in the W32time NtpClient policy key. "
                    + "Configures the NTP client to query the time server every 64 seconds (2^6), "
                    + "providing fast correction for machines where tight time accuracy is required "
                    + "(e.g., Kerberos authentication, certificate validity checks). "
                    + "Default: absent (OS default 604800 = 1 week for workstations). "
                    + "Recommended: 64 on highly accurate or compliance-sensitive deployments.",
                Tags = ["ntp", "poll-interval", "accuracy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "NTP polled every 64 seconds; increases NTP traffic but keeps clock within ±1 second.",
                ApplyOps = [RegOp.SetDword(NtpClient, "SpecialPollInterval", 64)],
                RemoveOps = [RegOp.DeleteValue(NtpClient, "SpecialPollInterval")],
                DetectOps = [RegOp.CheckDword(NtpClient, "SpecialPollInterval", 64)],
            },
            new TweakDef
            {
                Id = "wtime-set-max-pos-phase-correction",
                Label = "Set Maximum Positive Phase Correction to 3600s",
                Category = "System",
                Description =
                    "Sets MaxPosPhaseCorrection=3600 in the W32time Config policy key. "
                    + "Limits how far the clock can jump forward in a single correction to 3600 seconds (1 hour). "
                    + "Prevents time-jump attacks where an attacker injects a far-future timestamp. "
                    + "Default: absent (OS default 48 hours). Recommended: 3600 for security-hardened environments.",
                Tags = ["ntp", "time", "security", "phase-correction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Clock cannot jump forward more than 1 hour in a single NTP correction; protects against time injection attacks.",
                ApplyOps = [RegOp.SetDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
                RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxPosPhaseCorrection")],
                DetectOps = [RegOp.CheckDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
            },
            new TweakDef
            {
                Id = "wtime-set-max-neg-phase-correction",
                Label = "Set Maximum Negative Phase Correction to 3600s",
                Category = "System",
                Description =
                    "Sets MaxNegPhaseCorrection=3600 in the W32time Config policy key. "
                    + "Limits how far the clock can jump backward in a single correction to 3600 seconds (1 hour). "
                    + "Prevents time-rollback attacks that could revalidate expired certificates or bypass time-based access controls. "
                    + "Default: absent (OS default 48 hours). Recommended: 3600 for certificate-sensitive environments.",
                Tags = ["ntp", "time", "security", "phase-correction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Clock cannot jump backward more than 1 hour in a single NTP correction; protects against rollback attacks.",
                ApplyOps = [RegOp.SetDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
                RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxNegPhaseCorrection")],
                DetectOps = [RegOp.CheckDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
            },
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
            new TweakDef
            {
                Id = "wtime-set-phase-correction-rate",
                Label = "Set NTP Phase Correction Rate to 1 (Fast)",
                Category = "System",
                Description =
                    "Sets FrequencyCorrectRate=4 in the W32time Config policy key. "
                    + "Controls how aggressively W32tm corrects the local oscillator frequency to match the NTP reference. "
                    + "Value 4 (the OS default) represents a balanced correction rate suitable for most workstations. "
                    + "Setting explicitly via policy prevents drift caused by third-party tools resetting this to slower values. "
                    + "Default: absent. Recommended: 4.",
                Tags = ["ntp", "time", "frequency", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Frequency correction rate pinned to 4 (OS default); prevents third-party tools from changing it.",
                ApplyOps = [RegOp.SetDword(CfgKey, "FrequencyCorrectRate", 4)],
                RemoveOps = [RegOp.DeleteValue(CfgKey, "FrequencyCorrectRate")],
                DetectOps = [RegOp.CheckDword(CfgKey, "FrequencyCorrectRate", 4)],
            },
            new TweakDef
            {
                Id = "wtime-set-spike-watchdog",
                Label = "Enable NTP Spike Watchdog Protection",
                Category = "System",
                Description =
                    "Sets SpikeWatchPeriod=900 in the W32time Config policy key (900 seconds = 15 minutes). "
                    + "Sets the window during which W32tm detects and ignores suspicious time spike samples "
                    + "from NTP servers — large, sudden deviations that may indicate NTP spoofing or misconfigured servers. "
                    + "Default: absent (W32tm uses a shorter default window). "
                    + "Recommended: 900 to extend spike detection for high-value machines.",
                Tags = ["ntp", "time", "security", "spike", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Suspicious NTP spikes are ignored for 15 minutes before re-evaluating; hardens against NTP injection.",
                ApplyOps = [RegOp.SetDword(CfgKey, "SpikeWatchPeriod", 900)],
                RemoveOps = [RegOp.DeleteValue(CfgKey, "SpikeWatchPeriod")],
                DetectOps = [RegOp.CheckDword(CfgKey, "SpikeWatchPeriod", 900)],
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
