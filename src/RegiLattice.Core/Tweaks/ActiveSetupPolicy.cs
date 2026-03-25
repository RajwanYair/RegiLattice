// RegiLattice.Core — Tweaks/ActiveSetupPolicy.cs
// Sprint 345: Active Setup Policy tweaks (10 tweaks)
// Category: "Active Setup Policy" | Slug: actsetup
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ActiveSetup

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ActiveSetupPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ActiveSetup";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "actsetup-disable-active-setup-execution",
            Label = "Disable Active Setup Execution for Non-Administrative Users",
            Category = "Active Setup Policy",
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
            Category = "Active Setup Policy",
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
            Category = "Active Setup Policy",
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
            Category = "Active Setup Policy",
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
            Category = "Active Setup Policy",
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
            Category = "Active Setup Policy",
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
            Category = "Active Setup Policy",
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
            Category = "Active Setup Policy",
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
            Category = "Active Setup Policy",
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
            Category = "Active Setup Policy",
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
