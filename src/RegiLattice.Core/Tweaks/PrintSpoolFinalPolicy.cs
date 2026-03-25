// RegiLattice.Core — Tweaks/PrintSpoolFinalPolicy.cs
// Sprint 363: Print Spool Final Policy tweaks (10 tweaks)
// Category: "Print Spool Final Policy" | Slug: splfinal
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\Cleanup

namespace RegiLattice.Core.Tweaks;
using RegiLattice.Core.Models;

internal static class PrintSpoolFinalPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\Cleanup";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "splfinal-enable-print-spooler-cleanup-on-idle",
            Label = "Enable Automatic Print Spooler Cleanup When Print Queue Is Idle",
            Category = "Print Spool Final Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description = "Enabling automatic print spooler cleanup when the print queue is idle removes completed print jobs and temporary spool files from the spooler directory ensuring that document content is not retained in the spool longer than necessary for the print operation. Print spool files contain document images in EMF or RAW format that may include sensitive content and should be removed promptly after the print job completes to minimize exposure. Automatic cleanup on idle conditions ensures that print spool data is cleared during normal operational periods without requiring administrative intervention for routine spool maintenance. Spool file cleanup reduces the attack surface on print servers by minimizing the window during which attackers can access spool files to recover document content. Organizations should verify that spool cleanup policies are applied consistently on all print servers and workstations with local print queues. Spool cleanup events should be logged to provide evidence that print data was disposed of appropriately for compliance reporting purposes.",
            Tags = ["print-spooler", "cleanup", "spool-files", "data-retention", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSpoolCleanupOnIdle", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSpoolCleanupOnIdle")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSpoolCleanupOnIdle", 1)],
        },
        new TweakDef
        {
            Id = "splfinal-enforce-immediate-spool-file-deletion",
            Label = "Enforce Immediate Deletion of Print Spool Files After Job Completion",
            Category = "Print Spool Final Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description = "Enforcing immediate deletion of print spool files upon job completion eliminates the retention window during which print spool data would otherwise be recoverable from the spool directory on print servers and workstations. Immediate spool deletion is a defense against forensic recovery of document content from print infrastructure that has been accessed by an attacker. Organizations handling sensitive information under regulatory requirements may need to implement immediate spool deletion to satisfy data minimization requirements for printed document data. Immediate deletion should be applied to all stages of the print spool including temporary intermediate files generated during EMF to device format conversion. The deletion operation should be verified to ensure files are actually removed rather than simply marked for deletion by the file system. Secure deletion using file overwrite operations rather than simple deletion should be considered for high-security environments where forensic recovery of spool data poses a significant risk.",
            Tags = ["print-spooler", "immediate-deletion", "spool-files", "secure-disposal", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceImmediateSpoolFileDeletion", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceImmediateSpoolFileDeletion")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceImmediateSpoolFileDeletion", 1)],
        },
        new TweakDef
        {
            Id = "splfinal-restrict-orphan-spool-file-retention",
            Label = "Restrict Retention of Orphaned Print Spool Files to Mandatory Cleanup Period",
            Category = "Print Spool Final Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description = "Orphaned print spool files resulting from failed or interrupted print jobs are retained in the spool directory indefinitely without automatic cleanup which creates unnecessary data accumulation and potential sensitive data exposure. Restricting orphaned spool file retention period to a maximum defined duration ensures that print data from failed jobs is automatically removed within a predictable timeframe. Long-term retention of orphaned spool files on print servers can accumulate large volumes of sensitive document data from all users who have sent print jobs to the server. Cleanup of orphaned spool files should be automated through the print spooler service rather than relying on manual administrator cleanup which may not occur regularly. The retention period for orphaned spool files should be set based on the sensitivity of the documents typically printed in the environment with shorter periods for environments processing sensitive regulated data. Cleanup operations for orphaned spool files should be logged to provide an audit trail of data disposal activities.",
            Tags = ["print-spooler", "orphaned-files", "cleanup", "spool-retention", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "OrphanedSpoolFileRetentionHours", 24)],
            RemoveOps = [RegOp.DeleteValue(Key, "OrphanedSpoolFileRetentionHours")],
            DetectOps = [RegOp.CheckDword(Key, "OrphanedSpoolFileRetentionHours", 24)],
        },
        new TweakDef
        {
            Id = "splfinal-enable-secure-spool-file-overwrite",
            Label = "Enable Secure Multi-Pass Overwrite for Print Spool File Deletion",
            Category = "Print Spool Final Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description = "Enabling secure overwrite for spool files replaces the content of spool files with random data before deletion ensuring that the document data is irrecoverable from the storage media through standard data recovery utilities. Simple deletion of spool files marks the file system entry as free but does not overwrite the underlying disk sectors leaving document content recoverable until those sectors are reused by other files. Organizations that process classified or highly sensitive documents using print infrastructure should implement secure overwrite for spool files to satisfy media sanitization requirements. The performance impact of secure overwrite operations on print servers is generally low because spool files are relatively small but the impact should be tested before deployment in high-volume print environments. Secure overwrite should be applied to all temporary files generated during the print rendering process including intermediate format conversion files that may contain partial document images. Compliance documentation for sensitive data handling programs should reference secure spool file deletion as a control contributing to data disposal assurance.",
            Tags = ["print-spooler", "secure-overwrite", "data-sanitization", "spool-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSecureSpoolFileOverwrite", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSecureSpoolFileOverwrite")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSecureSpoolFileOverwrite", 1)],
        },
        new TweakDef
        {
            Id = "splfinal-audit-spool-directory-access",
            Label = "Enable Audit Logging for Print Spool Directory File System Access Events",
            Category = "Print Spool Final Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description = "Enabling audit logging for print spool directory access events records all reads writes and deletions of files in the print spool directory providing visibility into unauthorized access to spool data by processes other than the print spooler service. Unauthorized access to the print spool directory by non-spooler processes may indicate malware attempting to read document content from spool files or an attacker harvesting document data. Access to spool directory files should be restricted to the Print Spooler service and local SYSTEM account with all other access attempts generating security audit events. Spool directory access audit events should be reviewed for access by unusual processes or user identities that do not have legitimate access needs. Security audit rules for the spool directory should be configured at the object access audit level to capture both successful and failed access attempts. Spool directory access audit data should be forwarded to SIEM for correlation with other endpoint security events to identify malicious access patterns.",
            Tags = ["print-spooler", "spool-directory", "audit-logging", "file-access", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditSpoolDirectoryAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditSpoolDirectoryAccess")],
            DetectOps = [RegOp.CheckDword(Key, "AuditSpoolDirectoryAccess", 1)],
        },
        new TweakDef
        {
            Id = "splfinal-restrict-spool-directory-permissions",
            Label = "Restrict File System Permissions on Print Spool Directory to Minimum Required Access",
            Category = "Print Spool Final Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description = "Restricting file system permissions on the print spool directory ensures that only the Print Spooler service account and local administrators have access to spool files preventing unauthorized reading or modification of print job data. Default Windows configurations allow the Network Service account and some user accounts to read from the spool directory which is broader access than required for normal printing operations. Tightening spool directory ACLs to SYSTEM and Print Spooler service only requires careful testing to ensure that the print spooler functionality is not broken and that legitimate access patterns are maintained. The Windows default spool directory path is %SYSTEMROOT%\\System32\\spool\\PRINTERS which should have restrictive ACLs preventing standard user access. Spool directory permission changes should be performed with care and tested thoroughly before production deployment as misconfigured permissions can prevent printing from functioning. Periodic review of spool directory permissions should verify that ACLs have not been relaxed by software installation or administrative changes.",
            Tags = ["print-spooler", "directory-permissions", "acl", "least-privilege", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSpoolDirectoryPermissions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSpoolDirectoryPermissions")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSpoolDirectoryPermissions", 1)],
        },
        new TweakDef
        {
            Id = "splfinal-block-spool-file-access-by-network",
            Label = "Block Remote Network Access to Print Spooler Spool File Directory",
            Category = "Print Spool Final Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description = "Blocking remote network access to the print spool directory ensures that network shares and remote file access protocols cannot be used to read or enumerate print spool contents from remote systems without the authorization required for spooler management operations. The PrintNightmare vulnerability family demonstrated that access to the spool directory from remote network connections can be exploited for privilege escalation and remote code execution. Blocking network access to the spool directory at the file system level provides defense in depth complementing the print spooler service access controls. Network firewall rules should also block remote access to the print spooler service on port 445 from systems that are not authorized print clients or print administrators. The printer driver path within the spool directory is particularly sensitive as it can be used to load arbitrary DLLs if network access is permitted. Vulnerability assessments should specifically test for network access to the spool directory as part of print infrastructure security evaluations.",
            Tags = ["print-spooler", "network-access", "printnightmare", "remote-access", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockNetworkSpoolFileAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockNetworkSpoolFileAccess")],
            DetectOps = [RegOp.CheckDword(Key, "BlockNetworkSpoolFileAccess", 1)],
        },
        new TweakDef
        {
            Id = "splfinal-enable-spool-service-hardening",
            Label = "Enable Additional Security Hardening for Print Spooler Service Operation",
            Category = "Print Spool Final Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description = "Print spooler service hardening applies additional security restrictions to the spooler process including restricting which DLLs can be loaded controlling network communication capabilities and applying attack surface reduction rules specifically targeting the print spooler attack surface. The print spooler service has historically been a common target for privilege escalation exploit chains and running the spooler with hardened configuration significantly reduces the effectiveness of known exploit techniques. Spooler hardening includes disabling the ability for the Print Spooler to accept remote connections when the system is not intended to serve as a print server which eliminates the network attack surface. Applications on workstations that do not require serving print jobs to other computers should run the print spooler in local-only mode to prevent remote exploitation. Print server configurations that require the remote print spooler functionality should apply spooler hardening in ways that are compatible with the remote printing use case. Microsoft security updates for the print spooler should be applied promptly due to the elevated risk associated with known spooler vulnerabilities.",
            Tags = ["print-spooler", "service-hardening", "attack-surface", "exploit-mitigation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSpoolServiceHardening", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSpoolServiceHardening")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSpoolServiceHardening", 1)],
        },
        new TweakDef
        {
            Id = "splfinal-configure-spool-file-encryption",
            Label = "Configure Encryption for Print Spool Files on Disk at Rest",
            Category = "Print Spool Final Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description = "Configuring encryption for print spool files on disk ensures that document content written to the spool directory during print operations is protected against unauthorized access by processes that can access the file system but are not authorized to access print data. Spool file encryption can be implemented through EFS Encrypting File System applied to the spool directory or through volume-level BitLocker encryption that covers the system drive where the spool directory resides. EFS applied specifically to the spool directory provides per-file encryption with the Print Spooler service as the authorized accessor while BitLocker provides volume-level protection relevant to physical media attacks. Organizations processing highly sensitive documents should evaluate spool file encryption as a control that complements access control restrictions on the spool directory. Encryption key management for spool file encryption should integrate with organizational key management practices to ensure keys are recoverable in the event of system failure. Performance testing should validate that spool file encryption does not introduce unacceptable latency in the print workflow for high-volume print environments.",
            Tags = ["print-spooler", "spool-encryption", "data-at-rest", "efs", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSpoolFileEncryption", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSpoolFileEncryption")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSpoolFileEncryption", 1)],
        },
        new TweakDef
        {
            Id = "splfinal-disable-persistently-cached-print-jobs",
            Label = "Disable Persistent Caching of Print Jobs in Print Spool for Offline Recovery",
            Category = "Print Spool Final Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description = "Disabling persistent caching of print jobs prevents the print spooler from retaining print job data across system restarts for the purpose of re-submitting jobs that were queued when a printer was offline. Persistent print job caching means that document content can remain in the spool for extended periods including across security-relevant system events such as user logoff or system hibernation. Users who submit print jobs intending them to be printed will have a poor experience if persistent caching is disabled when the target printer is unavailable but the security benefit justifies the workflow impact in high-security environments. Organizations with strict data handling requirements for sensitive document categories should disable persistent print job caching to ensure document data does not accumulate in the spool across operational sessions. Alternative print management approaches including print management software that provides controlled job resubmission with appropriate authentication can address legitimate offline printing requirements. User communication about the impact of disabling persistent print caching should be provided before the policy is deployed.",
            Tags = ["print-spooler", "persistent-cache", "data-minimization", "offline-printing", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePersistentlyCachedPrintJobs", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePersistentlyCachedPrintJobs")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePersistentlyCachedPrintJobs", 1)],
        },
    ];
}
