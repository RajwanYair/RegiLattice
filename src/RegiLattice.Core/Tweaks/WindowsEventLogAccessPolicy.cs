// WindowsEventLogAccessPolicy.cs — Event log channel size and access control policies
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\EventLog\*
// Slug: evtacc
// Category: Windows Event Log Access Policy

namespace RegiLattice.Core.Tweaks;

using System.Collections.Generic;
using RegiLattice.Core.Models;

internal static class WindowsEventLogAccessPolicy
{
    private const string AppLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Application";
    private const string SecLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security";
    private const string SysLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\System";
    private const string PsLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Windows PowerShell";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "evtacc-set-security-log-size-100mb",
            Label = "Event Log Access: Set Security Log Maximum Size to 100 MB",
            Category = "Windows Event Log Access Policy",
            Description =
                "Sets the maximum size of the Windows Security event log to 100 MB (102,400 KB). "
                + "Security logs capture authentication, privilege use, object access, and policy changes. "
                + "A 100 MB log retains significantly more history than the default 20 MB, reducing the risk of log overwrite during high-event periods. "
                + "Removing this policy reverts the security log size to its configured or default value.",
            Tags = ["event-log", "security-log", "log-size", "audit", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SecLog],
            ApplyOps = [RegOp.SetDword(SecLog, "MaxSize", 102400)],
            RemoveOps = [RegOp.DeleteValue(SecLog, "MaxSize")],
            DetectOps = [RegOp.CheckDword(SecLog, "MaxSize", 102400)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Expands security log to 100 MB; retains more audit history before log overwrite.",
        },
        new TweakDef
        {
            Id = "evtacc-set-system-log-size-50mb",
            Label = "Event Log Access: Set System Log Maximum Size to 50 MB",
            Category = "Windows Event Log Access Policy",
            Description =
                "Sets the maximum size of the Windows System event log to 50 MB (51,200 KB). "
                + "System logs record OS-level events including driver failures, service status changes, and hardware errors. "
                + "Increasing the log size from the default 20 MB ensures system events are retained longer for post-incident analysis. "
                + "Removing this policy reverts the system log to its configured or default maximum size.",
            Tags = ["event-log", "system-log", "log-size", "audit", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SysLog],
            ApplyOps = [RegOp.SetDword(SysLog, "MaxSize", 51200)],
            RemoveOps = [RegOp.DeleteValue(SysLog, "MaxSize")],
            DetectOps = [RegOp.CheckDword(SysLog, "MaxSize", 51200)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Expands system log to 50 MB; preserves more OS event history for diagnostics.",
        },
        new TweakDef
        {
            Id = "evtacc-set-application-log-size-50mb",
            Label = "Event Log Access: Set Application Log Maximum Size to 50 MB",
            Category = "Windows Event Log Access Policy",
            Description =
                "Sets the maximum size of the Windows Application event log to 50 MB (51,200 KB). "
                + "Application logs capture events from user-mode applications, COM+ components, and .NET runtime. "
                + "A larger log buffer ensures application errors are not overwritten before they can be captured by monitoring agents. "
                + "Removing this policy reverts the application log to its configured or default maximum size.",
            Tags = ["event-log", "application-log", "log-size", "audit", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [AppLog],
            ApplyOps = [RegOp.SetDword(AppLog, "MaxSize", 51200)],
            RemoveOps = [RegOp.DeleteValue(AppLog, "MaxSize")],
            DetectOps = [RegOp.CheckDword(AppLog, "MaxSize", 51200)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Expands application log to 50 MB; reduces risk of losing application error events.",
        },
        new TweakDef
        {
            Id = "evtacc-set-powershell-log-size-50mb",
            Label = "Event Log Access: Set PowerShell Log Maximum Size to 50 MB",
            Category = "Windows Event Log Access Policy",
            Description =
                "Sets the maximum size of the Windows PowerShell event log to 50 MB (51,200 KB). "
                + "PowerShell logs are critical for detecting malicious script execution, living-off-the-land attacks, and lateral movement. "
                + "The default PowerShell log size is too small to retain a meaningful window of script block and operational events. "
                + "Removing this policy reverts the PowerShell log to its configured or default maximum size.",
            Tags = ["event-log", "powershell-log", "log-size", "threat-detection", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [PsLog],
            ApplyOps = [RegOp.SetDword(PsLog, "MaxSize", 51200)],
            RemoveOps = [RegOp.DeleteValue(PsLog, "MaxSize")],
            DetectOps = [RegOp.CheckDword(PsLog, "MaxSize", 51200)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Expands PowerShell log to 50 MB; retains more script event history for threat detection.",
        },
        new TweakDef
        {
            Id = "evtacc-security-log-retain-7days",
            Label = "Event Log Access: Retain Security Log for 7 Days Before Overwriting",
            Category = "Windows Event Log Access Policy",
            Description =
                "Configures the security event log retention policy to retain events for a minimum of 7 days before overwriting. "
                + "This ensures that security events are available for forensic analysis for at least one week after they occur. "
                + "The retention policy value of 7 days is the CIS Benchmark recommendation for workstation security logging. "
                + "Removing this policy reverts the security log retention to its default (overwrite as needed).",
            Tags = ["event-log", "security-log", "retention", "forensics", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SecLog],
            ApplyOps = [RegOp.SetDword(SecLog, "Retention", 604800)],
            RemoveOps = [RegOp.DeleteValue(SecLog, "Retention")],
            DetectOps = [RegOp.CheckDword(SecLog, "Retention", 604800)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Retains security events for 7 days; supports forensic analysis windows for incidents.",
        },
        new TweakDef
        {
            Id = "evtacc-security-log-autobackup",
            Label = "Event Log Access: Auto-Backup Security Log When Full",
            Category = "Windows Event Log Access Policy",
            Description =
                "Enables automatic backup archiving of the security event log when it reaches capacity. "
                + "When enabled, Windows saves a timestamped archive copy of the full log before creating space for new events. "
                + "Auto-backup prevents indefinite log overwrite and creates an audit trail archive without requiring a SIEM agent. "
                + "Removing this policy disables automatic backup of the security log on overflow.",
            Tags = ["event-log", "security-log", "backup", "archive", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SecLog],
            ApplyOps = [RegOp.SetDword(SecLog, "AutoBackupLogFiles", 1)],
            RemoveOps = [RegOp.DeleteValue(SecLog, "AutoBackupLogFiles")],
            DetectOps = [RegOp.CheckDword(SecLog, "AutoBackupLogFiles", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Archives security log on overflow; prevents event loss without a SIEM agent.",
        },
        new TweakDef
        {
            Id = "evtacc-restrict-guest-access-security-log",
            Label = "Event Log Access: Restrict Guest Access to Security Log",
            Category = "Windows Event Log Access Policy",
            Description =
                "Prevents guest accounts from reading the security event log. "
                + "The security log contains sensitive information about authentication attempts and privilege usage that should not be accessible to unauthenticated or guest-level users. "
                + "This policy denies the 'Guests' group read access to the security log channel. "
                + "Removing this policy allows guest accounts to read security log contents.",
            Tags = ["event-log", "security-log", "access-control", "guest", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SecLog],
            ApplyOps = [RegOp.SetDword(SecLog, "RestrictGuestAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(SecLog, "RestrictGuestAccess")],
            DetectOps = [RegOp.CheckDword(SecLog, "RestrictGuestAccess", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Denies guest account access to security log; prevents log content leakage.",
        },
        new TweakDef
        {
            Id = "evtacc-restrict-guest-access-system-log",
            Label = "Event Log Access: Restrict Guest Access to System Log",
            Category = "Windows Event Log Access Policy",
            Description =
                "Prevents guest accounts from reading the system event log. "
                + "System logs contain details about driver loads, service failures, and hardware events that can aid an attacker in reconnaissance. "
                + "Restricting guest access limits information available to unauthenticated users who gain temporary access to the machine. "
                + "Removing this policy allows guest accounts to read system log contents.",
            Tags = ["event-log", "system-log", "access-control", "guest", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SysLog],
            ApplyOps = [RegOp.SetDword(SysLog, "RestrictGuestAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(SysLog, "RestrictGuestAccess")],
            DetectOps = [RegOp.CheckDword(SysLog, "RestrictGuestAccess", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Denies guest account access to system log; limits reconnaissance information.",
        },
        new TweakDef
        {
            Id = "evtacc-restrict-guest-access-application-log",
            Label = "Event Log Access: Restrict Guest Access to Application Log",
            Category = "Windows Event Log Access Policy",
            Description =
                "Prevents guest accounts from reading the application event log. "
                + "Application log events can expose internal application behavior, error messages, and stack traces useful to an attacker. "
                + "Restricting guest access follows the principle of least privilege for event log visibility. "
                + "Removing this policy allows guest accounts to read application log contents.",
            Tags = ["event-log", "application-log", "access-control", "guest", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [AppLog],
            ApplyOps = [RegOp.SetDword(AppLog, "RestrictGuestAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(AppLog, "RestrictGuestAccess")],
            DetectOps = [RegOp.CheckDword(AppLog, "RestrictGuestAccess", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Denies guest access to application log; limits application information exposure.",
        },
        new TweakDef
        {
            Id = "evtacc-system-log-autobackup",
            Label = "Event Log Access: Auto-Backup System Log When Full",
            Category = "Windows Event Log Access Policy",
            Description =
                "Enables automatic backup archiving of the system event log when it reaches capacity. "
                + "System events relating to hardware failures, driver crashes, or service terminations should be preserved. "
                + "Auto-backup ensures system events are not lost when the log fills during a high-activity period such as a malware incident or hardware degradation. "
                + "Removing this policy disables automatic backup of the system log on overflow.",
            Tags = ["event-log", "system-log", "backup", "archive", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SysLog],
            ApplyOps = [RegOp.SetDword(SysLog, "AutoBackupLogFiles", 1)],
            RemoveOps = [RegOp.DeleteValue(SysLog, "AutoBackupLogFiles")],
            DetectOps = [RegOp.CheckDword(SysLog, "AutoBackupLogFiles", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Archives system log on overflow; ensures hardware/service events are not lost.",
        },
    ];
}
