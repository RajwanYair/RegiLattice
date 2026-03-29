// RegiLattice.Core — Tweaks/PrivilegeUseAuditPolicy.cs
// Advanced privilege use and sensitive privilege audit policy controls (Sprint 623).
// Category: "Privilege Use Audit Policy" | Slug: privaudit
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies\Privilege Use

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrivilegeUseAuditPolicy
{
    private const string PrivKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Privilege Use";
    private const string AclKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Object Access";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "privaudit-audit-sensitive-privilege-use",
            Label = "Privilege Audit: Enable Auditing of Sensitive Privilege Use (SeDebug, SeTcb, SeBackup)",
            Category = "Privilege Use Audit Policy",
            Description = "Sets 'Audit Sensitive Privilege Use'=3 (Success+Failure) in the Advanced Audit Policy. Generates Security event 4673/4674 whenever a process invokes a sensitive privilege — SeDebugPrivilege (used by Mimikatz for LSASS dump), SeTcbPrivilege (act as operating system), SeBackupPrivilege (bypass file ACLs for backup), SeRestorePrivilege, SeTakeOwnershipPrivilege — providing direct detection signal for privilege-abuse attack techniques. " +
                "SeDebugPrivilege invocation is a binary trigger for LSASS credential dumping — every major credential harvesting tool (Mimikatz, ProcDump LSASS, Task Manager LSASS dump) requires SeDebugPrivilege to access LSASS memory. Auditing sensitive privilege use generates Security event 4673 the instant any process invokes SeDebugPrivilege, providing near-real-time detection of credential theft attempts through SIEM correlation — typically one of the highest-fidelity, lowest-noise detection rules in an enterprise SIEM.",
            Tags = ["privilege-audit", "sensitive-privilege", "sedebug", "mimikatz", "lsass", "credential-theft"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Sensitive privilege use events generated; SeDebugPrivilege (Mimikatz/LSASS dump) detection in near-real-time.",
            ApplyOps = [RegOp.SetDword(PrivKey, "AuditSensitivePrivilegeUse", 3)],
            RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditSensitivePrivilegeUse")],
            DetectOps = [RegOp.CheckDword(PrivKey, "AuditSensitivePrivilegeUse", 3)],
        },
        new TweakDef
        {
            Id = "privaudit-audit-nonsensitive-privilege-use",
            Label = "Privilege Audit: Enable Auditing of Non-Sensitive Privilege Use (SeShutdown, SeLoad)",
            Category = "Privilege Use Audit Policy",
            Description = "Sets 'Audit Non-Sensitive Privilege Use'=1 (Success) in the Advanced Audit Policy. Generates Security event 4673/4674 for non-sensitive privilege invocations (SeShutdownPrivilege, SeUndockPrivilege, SeLoadDriverPrivilege, SeSystemtimePrivilege, SeTimeZonePrivilege, SeChangeNotifyPrivilege). Non-sensitive privilege events complement sensitive privilege events to provide a complete picture of privilege hierarchy escalation. " +
                "SeLoadDriverPrivilege invocation is the second critical attack signal — attackers who load a signed-but-vulnerable driver as a vector for privilege escalation (BYOVD, Bring Your Own Vulnerable Driver) must invoke SeLoadDriverPrivilege to install the driver. Auditing this privilege provides detection for BYOVD attacks (used by Lazarus Group, BlackMatter ransomware) before the vulnerable driver is loaded and exploited.",
            Tags = ["privilege-audit", "nonsensitive-privilege", "seloaddriver", "byovd", "driver-exploit"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Non-sensitive privilege invocations audited; SeLoadDriverPrivilege (BYOVD attack vector) detectable.",
            ApplyOps = [RegOp.SetDword(PrivKey, "AuditNonSensitivePrivilegeUse", 1)],
            RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditNonSensitivePrivilegeUse")],
            DetectOps = [RegOp.CheckDword(PrivKey, "AuditNonSensitivePrivilegeUse", 1)],
        },
        new TweakDef
        {
            Id = "privaudit-audit-other-privilege-use-events",
            Label = "Privilege Audit: Enable 'Other Privilege Use Events' for Complete Privilege Coverage",
            Category = "Privilege Use Audit Policy",
            Description = "Sets 'Audit Other Privilege Use Events'=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events for miscellaneous privilege use scenarios not captured by Sensitive or Non-Sensitive subcategories, including encrypted data recovery, user right assignments via Direct Access, and scheduled task privilege overrides. Completes the privilege use audit coverage across all three subcategories. " +
                "The 'Other Privilege Use Events' subcategory captures edge-case privilege invocations that don't neatly fit the Sensitive/Non-Sensitive taxonomy — including cross-domain encrypted data access (EFS recovery) and some legacy DCOM privilege transitions. While individually lower-signal than SeDebugPrivilege events, collectively these events fill gaps in the privilege audit trail that sophisticated threat actors may attempt to exploit by routing privilege escalation through lesser-audited paths.",
            Tags = ["privilege-audit", "other-privilege", "efs", "dcom", "complete-coverage"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Other privilege use events audited; complete privilege audit coverage across all three subcategories.",
            ApplyOps = [RegOp.SetDword(PrivKey, "AuditOtherPrivilegeUseEvents", 3)],
            RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditOtherPrivilegeUseEvents")],
            DetectOps = [RegOp.CheckDword(PrivKey, "AuditOtherPrivilegeUseEvents", 3)],
        },
        new TweakDef
        {
            Id = "privaudit-audit-file-system-failures",
            Label = "Privilege Audit: Enable File System Access Failure Auditing for ACL Bypass Detection",
            Category = "Privilege Use Audit Policy",
            Description = "Sets 'Audit File System Failures'=2 (Failure) in the Advanced Audit Policy Object Access category. Generates Security event 4656/4663 (Failure) whenever a process is denied access to a file or folder due to DACL permissions, recording the file path, access type requested, requesting process, and user account — providing detection for access scanning and ACL enumeration attacks. " +
                "Access failure events are high-signal early warning indicators for insider threat and lateral movement reconnaissance. A compromised account scanning the file system for accessible data will generate hundreds of access failure events as it attempts to read protected files and directories above its permission level. A volume spike in Event 4656 Failure events from a single user account is a reliable indicator of data access scanning or Shadow IT application attempting to read sensitive data repositories.",
            Tags = ["privilege-audit", "file-system", "access-failure", "acl", "insider-threat", "scanning"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "File system access failure events generated; ACL bypass attempts and access scanning produce high-fidelity detection events.",
            ApplyOps = [RegOp.SetDword(AclKey, "AuditFileSystem", 2)],
            RemoveOps = [RegOp.DeleteValue(AclKey, "AuditFileSystem")],
            DetectOps = [RegOp.CheckDword(AclKey, "AuditFileSystem", 2)],
        },
        new TweakDef
        {
            Id = "privaudit-audit-registry-object-access",
            Label = "Privilege Audit: Enable Sensitive Registry Key Access Auditing",
            Category = "Privilege Use Audit Policy",
            Description = "Sets 'Audit Registry Object Access'=3 (Success+Failure) in the Advanced Audit Policy Object Access category. Generates Security events 4656/4663 for registry key access operations on SACL-protected registry keys (keys with an assigned Security Audit ACL), enabling detection of access to AutoRun keys, service configuration keys, and other persistence mechanism registry locations. " +
                "Registry-based persistence (Run keys, Services, COM hijacking targets) are the most common dwell-time persistence mechanisms. Auditing access to SACL-protected registry keys (HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Run, HKLM\\SYSTEM\\CurrentControlSet\\Services, HKLM\\SOFTWARE\\Classes\\CLSID) detects both initial persistence registration (write access) and the periodic re-invocation of persistence (read access at logon). When SACL-protected keys are configured on high-value locations, SIEM rules can alert on unexpected write access creating new persistence entries.",
            Tags = ["privilege-audit", "registry", "sacl", "persistence", "run-keys", "com-hijacking"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "SACL-protected registry keys generate access events; persistence mechanism modifications detectable via event correlation.",
            ApplyOps = [RegOp.SetDword(AclKey, "AuditRegistry", 3)],
            RemoveOps = [RegOp.DeleteValue(AclKey, "AuditRegistry")],
            DetectOps = [RegOp.CheckDword(AclKey, "AuditRegistry", 3)],
        },
        new TweakDef
        {
            Id = "privaudit-audit-removable-storage-access",
            Label = "Privilege Audit: Enable Removable Storage Access Audit Events for USB DLP",
            Category = "Privilege Use Audit Policy",
            Description = "Sets 'Audit Removable Storage'=3 (Success+Failure) in the Advanced Audit Policy Object Access category. Generates Security event 4663 for all read and write operations to removable storage devices (USB drives, SD cards, DVD writers), recording the file name, operation type, and user account for every file accessed on removable media — enabling DLP monitoring without a dedicated DLP agent. " +
                "Removable storage audit provides per-file visibility of data access on USB drives. Where standard PnP audit (plug/unplug events) only shows that a device was connected, removable storage audit shows exactly which files were copied to or read from the device. This enables insider threat scenarios to be reconstructed precisely — ACME employee connected USB drive X at 14:32, copied 47 files totalling 2.3 GB from the SharePoint mapped drive, disconnected at 14:35 — from on-device event log evidence alone.",
            Tags = ["privilege-audit", "removable-storage", "usb", "dlp", "insider-threat", "data-exfiltration"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Per-file removable storage access audited; USB data exfiltration reconstructable at file level from Security event log.",
            ApplyOps = [RegOp.SetDword(AclKey, "AuditRemovableStorage", 3)],
            RemoveOps = [RegOp.DeleteValue(AclKey, "AuditRemovableStorage")],
            DetectOps = [RegOp.CheckDword(AclKey, "AuditRemovableStorage", 3)],
        },
        new TweakDef
        {
            Id = "privaudit-audit-token-right-adjustment",
            Label = "Privilege Audit: Enable Token Privilege Adjustment Auditing for UAC Bypass Detection",
            Category = "Privilege Use Audit Policy",
            Description = "Sets AuditTokenPrivilegeAdjustment=3 (Success+Failure) in the Windows System policy privilege section. Generates Security event 4703 (Token privilege adjustment) when a process enables or disables a privilege in its own access token, providing detection for UAC bypass techniques that involve enabling disabled privileges in a standard user token to perform privileged operations without triggering a UAC prompt. " +
                "Many UAC bypass techniques (mockdirs, fodhelper, eventvwr, DLL UAC auto-elevations) work by enabling privileges that are present but disabled in the current token (e.g., SeImpersonatePrivilege, SeAssignPrimaryTokenPrivilege) through techniques that avoid the standard UAC elevation flow. Token privilege adjustment events (4703) generated when these operations occur provide a direct detection signal for UAC bypass patterns — especially in combination with process creation events showing the bypassed elevated process that spawns immediately after the token adjustment.",
            Tags = ["privilege-audit", "token-adjustment", "uac-bypass", "event-4703", "impersonation"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Token privilege adjustments generate Event 4703; UAC bypass techniques involving token privilege enabling detectable.",
            ApplyOps = [RegOp.SetDword(PrivKey, "AuditTokenPrivilegeAdjustment", 3)],
            RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditTokenPrivilegeAdjustment")],
            DetectOps = [RegOp.CheckDword(PrivKey, "AuditTokenPrivilegeAdjustment", 3)],
        },
        new TweakDef
        {
            Id = "privaudit-audit-special-logon",
            Label = "Privilege Audit: Enable Special Logon Auditing (Admin Equivalent or Special Group Logons)",
            Category = "Privilege Use Audit Policy",
            Description = "Sets AuditSpecialLogon=1 (Success) in the Advanced Audit Policy Logon/Logoff category. Generates Security event 4964 (Special groups assigned to new logon) when an Entra ID or domain user whose account is a member of a Special Groups audit list logs on, providing targeted monitoring for high-privilege accounts without the event volume of full logon auditing for all users. " +
                "Special Logon auditing enables selective privileged account monitoring. By configuring the Special Groups list to include Domain Admins, Enterprise Admins, Backup Operators, and other critical security groups, the enterprise gets immediate Security event notification every time any member of those groups authenticates to any endpoint in the domain — without generating Event 4624 for every employee logon. This powers 'privileged account logon monitoring' SIEM rules with precise scope and minimal noise.",
            Tags = ["privilege-audit", "special-logon", "event-4964", "admin-monitoring", "privileged-accounts"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Special group logons generate Event 4964; privileged account authentication to any endpoint monitored in real time.",
            ApplyOps = [RegOp.SetDword(PrivKey, "AuditSpecialLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditSpecialLogon")],
            DetectOps = [RegOp.CheckDword(PrivKey, "AuditSpecialLogon", 1)],
        },
        new TweakDef
        {
            Id = "privaudit-audit-sam-sam-access",
            Label = "Privilege Audit: Enable SAM Database Access Auditing for Credential Database Protection",
            Category = "Privilege Use Audit Policy",
            Description = "Sets AuditSAMAccess=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events when the Security Account Manager (SAM) database is accessed, providing detection for credential dumping techniques that target the local SAM database (offline dump of SYSTEM and SAM hive, volume shadow copy SAM extraction, or SecretsDump against local accounts). " +
                "The SAM database contains the NTLM password hashes for all local Windows user accounts. SAM database access is a common post-exploitation step — after gaining SYSTEM privileges, threat actors extract SAM to harvest local account hashes for Pass-the-Hash attacks or for offline cracking. Auditing SAM access generates Security events whenever the SAM hive is opened with access beyond normal system operations, providing detection signals for credential harvesting operations against local accounts.",
            Tags = ["privilege-audit", "sam", "credential-dumping", "ntlm", "pass-the-hash", "secretsdump"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "SAM database access audited; credential dumping attempts targeting local account hashes generate Security events.",
            ApplyOps = [RegOp.SetDword(PrivKey, "AuditSAMAccess", 3)],
            RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditSAMAccess")],
            DetectOps = [RegOp.CheckDword(PrivKey, "AuditSAMAccess", 3)],
        },
        new TweakDef
        {
            Id = "privaudit-audit-lsa-secrets-access",
            Label = "Privilege Audit: Enable LSA Secrets Access Auditing for Service Credential Protection",
            Category = "Privilege Use Audit Policy",
            Description = "Sets AuditLSASecretsAccess=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events when the Local Security Authority (LSA) secrets store is accessed, detecting attempts to harvest service account credentials and DPAPI master keys stored in the LSA secrets store by tools such as Mimikatz's lsadump::secrets command or reg.exe SYSTEM hive extraction. " +
                "LSA secrets contain auto-logon account passwords, service account passwords for Windows services configured to run as domain accounts, DPAPI master key encryption keys, and cached domain credentials (DCC2 hashes). These are higher-value credentials than local SAM hashes because service account credentials are often over-provisioned domain accounts with access to multiple servers. Auditing LSA secrets access detects the critical early step of service account credential harvesting that enables subsequent lateral movement.",
            Tags = ["privilege-audit", "lsa-secrets", "service-credentials", "dpapi", "mimikatz", "lateral-movement"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "LSA secrets access audited; service account credential harvesting (Mimikatz lsadump::secrets) generates detection events.",
            ApplyOps = [RegOp.SetDword(PrivKey, "AuditLSASecretsAccess", 3)],
            RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditLSASecretsAccess")],
            DetectOps = [RegOp.CheckDword(PrivKey, "AuditLSASecretsAccess", 3)],
        },
    ];
}
