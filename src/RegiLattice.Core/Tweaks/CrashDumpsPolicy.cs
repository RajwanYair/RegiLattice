// RegiLattice.Core — Tweaks/CrashDumpsPolicy.cs
// Sprint 352: Crash Dumps Policy tweaks (10 tweaks)
// Category: "Crash Dumps Policy" | Slug: crshmp
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CrashControl

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CrashDumpsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CrashControl";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "crshmp-disable-crash-report-telemetry",
            Label = "Disable Automatic Crash Report Transmission to Microsoft Telemetry",
            Category = "Crash Dumps Policy",
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
            Category = "Crash Dumps Policy",
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
            Category = "Crash Dumps Policy",
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
            Category = "Crash Dumps Policy",
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
            Category = "Crash Dumps Policy",
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
            Category = "Crash Dumps Policy",
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
            Category = "Crash Dumps Policy",
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
            Category = "Crash Dumps Policy",
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
            Category = "Crash Dumps Policy",
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
            Category = "Crash Dumps Policy",
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
