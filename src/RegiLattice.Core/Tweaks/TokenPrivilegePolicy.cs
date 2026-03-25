// RegiLattice.Core — Tweaks/TokenPrivilegePolicy.cs
// Sprint 359: Token Privilege Policy tweaks (10 tweaks)
// Category: "Token Privilege Policy" | Slug: tokpriv
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Privileges

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TokenPrivilegePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Privileges";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "tokpriv-restrict-debug-privilege-assignment",
            Label = "Restrict Assignment of Debug Programs Privilege to Authorized Accounts",
            Category = "Token Privilege Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "The SeDebugPrivilege token privilege allows processes that hold it to debug any other process on the system by attaching a debugger granting full read and write access to the target process's memory including LSASS where credentials are stored. Restricting assignment of debug privilege is critical because it is the primary privilege exploited by credential dumping tools such as Mimikatz that read LSASS memory to extract plaintext passwords and Kerberos tickets. By default only members of the local Administrators group hold debug privilege but privilege escalation vulnerabilities or misconfigured services can grant this privilege to unauthorized accounts. Organizations should audit and minimize the number of accounts that are assigned debug privilege reviewing each account's business justification. Developer accounts that require debug privilege for legitimate software development work should use separate privileged accounts rather than their standard user accounts. Changes to debug privilege assignments should trigger security alerts for immediate investigation.",
            Tags = ["debug-privilege", "se-debug", "credential-protection", "least-privilege", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictDebugPrivilegeAssignment", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictDebugPrivilegeAssignment")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictDebugPrivilegeAssignment", 1)],
        },
        new TweakDef
        {
            Id = "tokpriv-audit-privilege-use-events",
            Label = "Enable Audit Logging for Sensitive Privilege Use Security Events",
            Category = "Token Privilege Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Auditing sensitive privilege use events records use of security-sensitive Windows privileges to the security event log providing visibility into high-risk operations that could indicate privilege abuse or compromise. Sensitive privileges include SeDebugPrivilege SeTcbPrivilege SeLoadDriverPrivilege SeTakeOwnershipPrivilege and SeSecurityPrivilege each of which can be used to escalate control over system resources. Security operations teams should baseline normal patterns of privilege use and alert on anomalous privilege use that deviates from established patterns. Windows generates Event ID 4672 for special logon with sensitive privileges and Event ID 4673 for privilege use operations that should both be monitored. Over-monitoring of non-sensitive privilege use can generate excessive event volume so organizations should focus auditing on the specific high-risk privileges most likely to be exploited. Privilege use audit events should be correlated with identity events to identify privilege abuse patterns across multiple systems.",
            Tags = ["privilege-audit", "security-events", "monitoring", "privilege-use", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditPrivilegeUseEvents", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditPrivilegeUseEvents")],
            DetectOps = [RegOp.CheckDword(Key, "AuditPrivilegeUseEvents", 3)],
        },
        new TweakDef
        {
            Id = "tokpriv-restrict-take-ownership-privilege",
            Label = "Restrict SeTakeOwnershipPrivilege Assignment on Domain and Local Accounts",
            Category = "Token Privilege Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "SeTakeOwnershipPrivilege allows accounts that hold it to take ownership of any file object directory registry key or other securable object on the system regardless of the object's current discretionary access control list. Misuse of take ownership privilege can bypass file and registry ACL protections and allow unauthorized modification of system files security configuration files or credential stores. Ransomware and destructive malware can use take ownership privilege to override ACLs protecting backup data preventing recovery. Standard user accounts and service accounts should never be assigned take ownership privilege without written justification and security review. IT administrator accounts that have legitimate requirements to use take ownership in specific operational scenarios should be required to use separate privileged accounts and just-in-time access. Audit events for take ownership privilege use should be reviewed regularly to ensure that ownership changes are operation-appropriate.",
            Tags = ["take-ownership", "acl-bypass", "privilege-restriction", "least-privilege", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictTakeOwnershipPrivilege", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictTakeOwnershipPrivilege")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictTakeOwnershipPrivilege", 1)],
        },
        new TweakDef
        {
            Id = "tokpriv-block-load-driver-privilege-expansion",
            Label = "Block Unauthorized Expansion of Load Driver Privilege to Standard Accounts",
            Category = "Token Privilege Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "SeLoadDriverPrivilege enables accounts that hold it to load kernel mode device drivers which execute with the highest privilege level on Windows and can fully control the operating system with no security boundary blocking their access. Malicious drivers loaded by compromised accounts with load driver privilege can disable security software including antivirus and endpoint detection and response agents at the kernel level. Blocking expansion of load driver privilege prevents privilege escalation vectors that involve convincing applications with load driver privilege to load attacker-controlled drivers. Only the most privileged system accounts should hold load driver privilege and mechanisms like Windows Driver Signature Enforcement and Kernel Mode Code Signing Policy should complement privilege restriction. Organizations should verify that driver loading is controlled through enterprise driver deployment mechanisms rather than individual user-initiated loading operations. Driver load events should generate security alerts for security operations investigation.",
            Tags = ["driver-loading", "kernel-privilege", "privilege-escalation", "driver-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockLoadDriverPrivilegeExpansion", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockLoadDriverPrivilegeExpansion")],
            DetectOps = [RegOp.CheckDword(Key, "BlockLoadDriverPrivilegeExpansion", 1)],
        },
        new TweakDef
        {
            Id = "tokpriv-enforce-privilege-token-filtering",
            Label = "Enforce Privilege Token Filtering for Administrative Access on Domain Systems",
            Category = "Token Privilege Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Windows User Account Control token filtering splits administrator tokens into a limited user token and a full administrator token requiring elevation for operations that need the full administrator privileges. Enforcing privilege token filtering ensures that remote administrative access to systems uses split tokens by default limiting the damage that can be done if an administrator session is compromised. The UAC token filtering for remote access is controlled by the LocalAccountTokenFilterPolicy setting and should be configured to filter remote access tokens for all accounts. Local administrator accounts used for remote administration are particularly vulnerable to pass-the-hash attacks when they hold full administrative tokens for remote sessions. Token filtering for remote access should be complemented by restricting which accounts can perform remote administrative access using restricted admin mode or Windows Defender Remote Credential Guard. Domain accounts with local administrative rights should have token filtering applied consistently across all domain systems.",
            Tags = ["token-filtering", "uac", "remote-access", "pass-the-hash", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforcePrivilegeTokenFiltering", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforcePrivilegeTokenFiltering")],
            DetectOps = [RegOp.CheckDword(Key, "EnforcePrivilegeTokenFiltering", 1)],
        },
        new TweakDef
        {
            Id = "tokpriv-restrict-impersonate-client-privilege",
            Label = "Restrict SeImpersonatePrivilege to Service Accounts That Require Impersonation",
            Category = "Token Privilege Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "SeImpersonatePrivilege allows a service account to impersonate a client that has authenticated to the service presenting the client's security identity for access checks on other resources during the service operation. Misuse of impersonation privilege through named pipe impersonation attacks or token kidnapping can allow a low-privileged service to escalate to SYSTEM by impersonating a higher-privileged process that connects to a service endpoint. Impersonation privilege is required by many legitimate Windows services including IIS RPC services and database servers so blanket removal is not feasible but the list of accounts holding impersonation privilege should be minimized. Service accounts that do not service remote clients should not hold impersonation privilege and should be configured with SeImpersonatePrivilege explicitly removed. Named pipe impersonation attacks are mitigated by network access policies that restrict which accounts can create named pipes and network logon type restrictions. Security review of accounts holding impersonation privilege should verify each account's impersonation use case is valid and necessary.",
            Tags = ["impersonate-privilege", "privilege-escalation", "service-accounts", "impersonation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictImpersonateClientPrivilege", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictImpersonateClientPrivilege")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictImpersonateClientPrivilege", 1)],
        },
        new TweakDef
        {
            Id = "tokpriv-block-privilege-abuse-alerts",
            Label = "Enable Real-Time Alerts for Privilege Abuse and Excessive Privilege Operations",
            Category = "Token Privilege Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Real-time privilege abuse alerts notify security operations teams immediately when sensitive privileges are exercised in ways that deviate from established operational baselines providing early attack detection capability. Privilege abuse alerts should be configured for all operations involving debug privilege take ownership privilege and load driver privilege regardless of the identity of the account performing the operation. Time-of-day baseline analysis can identify privilege use that occurs outside normal business hours which may indicate account compromise or insider threat activity. Volume-based analysis should detect privilege escalation attacks where an attacker attempts many privilege operations in rapid succession. Privilege abuse alerts should be integrated with the security incident response process to ensure that alerts are triaged and investigated within defined service level objectives. False positive tuning for privilege abuse alerts is important to avoid alert fatigue which reduces the effectiveness of security monitoring programs.",
            Tags = ["privilege-abuse", "real-time-alerts", "security-monitoring", "incident-detection", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnablePrivilegeAbuseAlerts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnablePrivilegeAbuseAlerts")],
            DetectOps = [RegOp.CheckDword(Key, "EnablePrivilegeAbuseAlerts", 1)],
        },
        new TweakDef
        {
            Id = "tokpriv-enforce-time-limited-privilege-grants",
            Label = "Enforce Time-Limited Just-In-Time Privilege Elevation for Administrative Tasks",
            Category = "Token Privilege Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Time-limited privilege grants require that elevated privileges are explicitly requested for specific administrative tasks and automatically revoke the elevated privileges when the administrative session ends rather than having privileges permanently assigned to accounts. Just-in-time privilege access reduces the window of opportunity for attackers to exploit administrator accounts by limiting the duration during which elevated privileges are available. Permanent privilege assignments create long-lived attack opportunities while time-limited grants ensure that account compromise during non-administrative periods does not provide attacker access to administrative capabilities. JIT privilege systems should require multi-factor authentication for all privilege elevation requests to ensure that privilege is granted only to authenticated users with legitimate needs. Activity performed under JIT privilege sessions should be recorded for audit and investigation purposes. JIT privilege grant workflows should integrate with change management systems to ensure that privilege elevation requests are tied to approved work orders.",
            Tags = ["jit-privilege", "time-limited", "privilege-grants", "least-privilege", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceTimeLimitedPrivilegeGrants", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceTimeLimitedPrivilegeGrants")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceTimeLimitedPrivilegeGrants", 1)],
        },
        new TweakDef
        {
            Id = "tokpriv-restrict-backup-restore-privileges",
            Label = "Restrict SeBackupPrivilege and SeRestorePrivilege to Authorized Backup Accounts",
            Category = "Token Privilege Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "SeBackupPrivilege and SeRestorePrivilege allow accounts to bypass file and directory ACLs when reading or writing files as part of backup and restore operations making them powerful privileges that can be used to read any file regardless of access controls. SeBackupPrivilege is exploited by attackers to read the NTDS.dit Active Directory database file and the SYSTEM registry hive enabling full domain credential extraction even without direct access to LSASS memory. These privileges are assigned to backup service accounts and the Backup Operators group by default and the membership of Backup Operators should be reviewed to ensure only authorized backup service accounts are included. Backup accounts should be monitored for use of backup and restore privileges outside of scheduled backup windows as out-of-window use may indicate compromise. File access under backup and restore privileges should be logged with sufficient detail to reconstruct which files were accessed by the backup process. Segregation of backup privileges should be implemented where separate accounts perform backup and restore operations to limit the capabilities available to any single compromised account.",
            Tags = ["backup-privilege", "restore-privilege", "acl-bypass", "ntds-protection", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictBackupRestorePrivileges", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictBackupRestorePrivileges")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictBackupRestorePrivileges", 1)],
        },
        new TweakDef
        {
            Id = "tokpriv-block-assign-primary-token-privilege",
            Label = "Block Unauthorized Assignment of Primary Token Privilege to Service Accounts",
            Category = "Token Privilege Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "The SeAssignPrimaryTokenPrivilege allows a service to create child processes that run under a different security token enabling the calling process to run new processes with the token of a different user account without going through standard logon procedures. Unauthorized use of assign primary token privilege can be used to start processes with stolen or forged tokens allowing privilege escalation beyond the account level of the service that holds the privilege. This privilege is required by the Windows service account infrastructure and specific system services but should not be held by application service accounts. Service accounts that do not spawn child processes under different security contexts should have assign primary token privilege removed from their effective privilege set. New service deployments should be reviewed to verify that assign primary token privilege is not included in the account rights unless there is documented justification for needing this capability. Security testing of service accounts should verify that assign primary token privilege cannot be exploited for privilege escalation through token swapping techniques.",
            Tags = ["primary-token", "privilege-assignment", "service-security", "token-escalation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockAssignPrimaryTokenPrivilege", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockAssignPrimaryTokenPrivilege")],
            DetectOps = [RegOp.CheckDword(Key, "BlockAssignPrimaryTokenPrivilege", 1)],
        },
    ];
}
