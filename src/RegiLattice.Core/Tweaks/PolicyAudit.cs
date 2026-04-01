// RegiLattice.Core — Tweaks/PolicyAudit.cs
// Security auditing, event log management, ETW sessions, event forwarding, process creation, and access audit policies
// Category: "Security Audit Policy"
// Consolidated from 20 modules.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyAudit
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _AccountManagementAuditPolicy.Data,
            .. _AuditEventPolicy.Data,
            .. _AuditPolicyAdvancedPolicy.Data,
            .. _DiagnosticDataViewerPolicy.Data,
            .. _DsObjectAccessAuditPolicy.Data,
            .. _ErrorReportingPolicy.Data,
            .. _EtwSessionPolicy.Data,
            .. _EventForwardingPolicy.Data,
            .. _EventLogChannelPolicy.Data,
            .. _EventLogGpoPolicy.Data,
            .. _EventSubscriptionPolicy.Data,
            .. _EventTracingPolicy.Data,
            .. _LogonEventsAuditPolicy.Data,
            .. _ObjectAccessPolicy.Data,
            .. _PrintAuditPolicy.Data,
            .. _PrivilegeUseAuditPolicy.Data,
            .. _ProcessCreationAuditPolicy.Data,
            .. _SecurityAuditPolicy.Data,
            .. _SqlServerAuditPolicy.Data,
            .. _WefSubscriptionPolicy.Data,
        ];

    // ── AccountManagementAuditPolicy ──
    private static class _AccountManagementAuditPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Account Management";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "acmgmtaudit-audit-user-account-management",
                Label = "Account Mgmt Audit: Enable Success+Failure Auditing for All User Account Changes",
                Category = "Security",
                Description = "Sets AuditUserAccountManagement=3 (Success+Failure) in the Advanced Audit Policy Account Management category. Generates Security events 4720 (created), 4722 (enabled), 4723 (pwd change attempt), 4724 (pwd reset), 4725 (disabled), 4726 (deleted), 4738 (changed), 4740 (locked out), 4765/4766 (SID history) for all local and domain user account lifecycle operations. Provides complete user identity lifecycle audit trail. " +
                    "User account management events are the foundational identity audit record. Security events 4720/4726 (account create/delete) are mandatory for SOC monitoring because they record rogue account creation — a common persistence technique. Without user account management auditing enabled, a threat actor can create a new backdoor local administrator account and there is no Security event log record of the account creation. All identity governance and SoD (Separation of Duties) compliance requirements depend on this audit subcategory being active.",
                Tags = ["account-mgmt-audit", "user-account", "account-creation", "4720", "persistence", "backdoor-account"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Full user account lifecycle audited; rogue account creation generates Event 4720 — foundational SOC monitoring signal.",
                ApplyOps = [RegOp.SetDword(Key, "AuditUserAccountManagement", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditUserAccountManagement")],
                DetectOps = [RegOp.CheckDword(Key, "AuditUserAccountManagement", 3)],
            },
            new TweakDef
            {
                Id = "acmgmtaudit-audit-security-group-management",
                Label = "Account Mgmt Audit: Enable Auditing for All Security Group Membership Changes",
                Category = "Security",
                Description = "Sets AuditSecurityGroupManagement=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events 4727 (global group created), 4728 (member added), 4729 (member removed), 4730 (global group deleted), 4731 (local group created), 4732 (local group member added), 4733 (local group member removed), 4734 (local group deleted), 4735 (local group changed) for all security group membership operations. " +
                    "Group membership changes are the primary privilege escalation audit signal in Active Directory environments. Adding a compromised account to Domain Admins, Backup Operators, or any privileged security group generates Event 4728/4732. SOC SIEM rules that alert on additions to predefined sensitive security groups (Domain Admins, Enterprise Admins, Schema Admins, Protected Users) depend entirely on this audit subcategory being active across all domain controllers and endpoints.",
                Tags = ["account-mgmt-audit", "security-group", "group-membership", "4728", "privilege-escalation", "domain-admins"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Security group membership changes audited; additions to privileged groups (Domain Admins) generate immediate SIEM detection events.",
                ApplyOps = [RegOp.SetDword(Key, "AuditSecurityGroupManagement", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSecurityGroupManagement")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSecurityGroupManagement", 3)],
            },
            new TweakDef
            {
                Id = "acmgmtaudit-audit-distribution-group-management",
                Label = "Account Mgmt Audit: Enable Auditing for Distribution Group Membership Changes",
                Category = "Security",
                Description = "Sets AuditDistributionGroupManagement=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events for distribution group lifecycle operations (4744–4758: create, change, delete, member add, member remove for global and universal distribution groups). Provides identity governance visibility for non-security-enabled groups that may have access to sensitive email distribution lists or SharePoint groups. " +
                    "Distribution groups do not have security principals and cannot directly grant file system access, but they control email distribution reach and SharePoint group membership when used as SharePoint audience targeting groups. An attacker who adds a compromised account to a 'Finance-All' distribution group gains full visibility of financial email communications including budgets, deals, and sensitive financial data delivered through that distribution list. Auditing distribution group changes enables detection of email list infiltration.",
                Tags = ["account-mgmt-audit", "distribution-group", "email-list", "sharepoint", "insider-threat"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Distribution group changes audited; unauthorised additions to sensitive email lists generate detection events.",
                ApplyOps = [RegOp.SetDword(Key, "AuditDistributionGroupManagement", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditDistributionGroupManagement")],
                DetectOps = [RegOp.CheckDword(Key, "AuditDistributionGroupManagement", 3)],
            },
            new TweakDef
            {
                Id = "acmgmtaudit-audit-computer-account-management",
                Label = "Account Mgmt Audit: Enable Auditing for Computer Account Creation and Deletion",
                Category = "Security",
                Description = "Sets AuditComputerAccountManagement=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events 4741 (computer account created), 4742 (computer account changed), 4743 (computer account deleted) for computer object lifecycle operations in Active Directory. Provides detection for rogue computer account creation used for Kerberos silver ticket persistence. " +
                    "Computer account creation in Active Directory is a high-value attack technique. By default, any domain user can create up to 10 computer objects in any container they have permissions over (ms-DS-MachineAccountQuota). An attacker with a foothold in the domain can create new computer accounts (RBCD, resource-based constrained delegation attacks), configure a service principal name, and use Kerberos delegation to obtain elevated Kerberos tickets. Computer account creation events detect this persistence technique immediately.",
                Tags = ["account-mgmt-audit", "computer-account", "4741", "rbcd", "kerberos", "silver-ticket"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Computer account creation (Event 4741) audited; RBCD/Kerberos silver ticket attack via rogue computer account detectable.",
                ApplyOps = [RegOp.SetDword(Key, "AuditComputerAccountManagement", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditComputerAccountManagement")],
                DetectOps = [RegOp.CheckDword(Key, "AuditComputerAccountManagement", 3)],
            },
            new TweakDef
            {
                Id = "acmgmtaudit-audit-other-account-management-events",
                Label = "Account Mgmt Audit: Enable Other Account Management Events (Password Hash Sync, PKI)",
                Category = "Security",
                Description = "Sets AuditOtherAccountManagementEvents=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events 4782 (password hash was accessed), 4793 (password policy API called), 4798 (user's local group membership enumerated), 4799 (security-enabled local group members enumerated) — capturing credential database access and reconnaissance activities that fall outside the standard account management event types. " +
                    "Events 4798 and 4799 (local group membership enumeration) are particularly significant — they are generated when a script or tool enumerates the members of the local Administrators group on an endpoint. Ransomware operators and red teams consistently enumerate local admin group membership across all endpoints immediately after initial compromise to identify which machines have Domain Admins logged in or have shared local admin passwords. These events provide direct detection of the reconnaissance phase of a ransomware campaign.",
                Tags = ["account-mgmt-audit", "4798", "4799", "local-group-enumeration", "ransomware", "recon"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Local group membership enumeration (4798/4799) audited; ransomware reconnaissance phase detectable in real time.",
                ApplyOps = [RegOp.SetDword(Key, "AuditOtherAccountManagementEvents", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditOtherAccountManagementEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditOtherAccountManagementEvents", 3)],
            },
            new TweakDef
            {
                Id = "acmgmtaudit-audit-application-group-management",
                Label = "Account Mgmt Audit: Enable Application Group Management Auditing",
                Category = "Security",
                Description = "Sets AuditApplicationGroupManagement=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events for application group lifecycle operations (4783–4792: create, change, delete, member add/remove for non-Universal, non-Security application groups used by network access protection and application-specific group policies). " +
                    "Application groups include Windows Authorization Manager (AzMan) application groups, which are used by LOB applications to define role-based access control within the application independent of Active Directory security groups. If an attacker gains write access to an AzMan policy store, they can add themselves to application-level admin roles without modifying Active Directory groups. Auditing application group changes detects this application-level privilege escalation vector.",
                Tags = ["account-mgmt-audit", "application-group", "azman", "rbac", "app-privilege-escalation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Application group changes audited; AzMan policy store privilege escalation generates detection events.",
                ApplyOps = [RegOp.SetDword(Key, "AuditApplicationGroupManagement", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditApplicationGroupManagement")],
                DetectOps = [RegOp.CheckDword(Key, "AuditApplicationGroupManagement", 3)],
            },
            new TweakDef
            {
                Id = "acmgmtaudit-enable-account-lockout-audit",
                Label = "Account Mgmt Audit: Enable Account Lockout Event Auditing for Brute Force Detection",
                Category = "Security",
                Description = "Sets AuditAccountLockout=3 (Success+Failure) in the Advanced Audit Policy Logon/Logoff category. Generates Security event 4625 Failure, 4770, and 4771 for failed logon attempts and event 4740 (account locked out) when an account's failed logon threshold is exceeded, providing brute force password spray attack detection across all endpoints and authentication services. " +
                    "Password spray attacks target a single password against an entire user list to avoid triggering per-account lockout thresholds (one attempt per account does not trigger lockout). Account lockout audit enables detection of spray patterns by correlating event 4740 (account locked out) across multiple accounts in a short time window — multiple lockouts in minutes with the same originating IP address is a high-fidelity indicator of a password spray attack. SOC SIEM rules for password spray are entirely dependent on this audit subcategory.",
                Tags = ["account-mgmt-audit", "account-lockout", "4740", "password-spray", "brute-force", "siem"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Account lockout events (4740) generated; password spray attacks create correlated lockout pattern detectable by SIEM.",
                ApplyOps = [RegOp.SetDword(Key, "AuditAccountLockout", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditAccountLockout")],
                DetectOps = [RegOp.CheckDword(Key, "AuditAccountLockout", 3)],
            },
            new TweakDef
            {
                Id = "acmgmtaudit-audit-user-right-assignment",
                Label = "Account Mgmt Audit: Enable Auditing for User Right Assignment Changes",
                Category = "Security",
                Description = "Sets AuditPolicyChange=3 (Success+Failure) in the Advanced Audit Policy Audit Policy Change category. Generates Security events 4703 (token privilege enabled/disabled), 4704 (user right assigned), 4705 (user right removed) when any user right (SeDebugPrivilege, SeTcbPrivilege, SeImpersonatePrivilege, etc.) is granted to or removed from any security principal. Detects direct user right manipulation. " +
                    "Granting SeDebugPrivilege or SeImpersonatePrivilege directly to a non-administrator security principal is an authoritative persistence technique — it gives the principal the same privilege as a local administrator for a specific action without adding them to the Administrators group. This bypasses monitoring rules that only watch for Administrators group membership changes. Auditing user right assignment changes detects this out-of-band privilege grant pathway.",
                Tags = ["account-mgmt-audit", "user-rights", "4704", "sedebug", "seimpersonate", "privilege-grant"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "User right assignment changes audited; direct SeDebugPrivilege/SeImpersonatePrivilege grants generate detection events.",
                ApplyOps = [RegOp.SetDword(Key, "AuditPolicyChange", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditPolicyChange")],
                DetectOps = [RegOp.CheckDword(Key, "AuditPolicyChange", 3)],
            },
            new TweakDef
            {
                Id = "acmgmtaudit-audit-credential-validation-failures",
                Label = "Account Mgmt Audit: Enable Credential Validation Failure Auditing for Auth Attack Detection",
                Category = "Security",
                Description = "Sets AuditCredentialValidation=3 (Success+Failure) in the Advanced Audit Policy Account Logon category. Generates Security events 4776 (NTLM authentication attempt — success/failure) and 4772/4776 failure events for failed NTLM credential validation attempts against the local SAM, enabling detection of NTLM hash relay attacks, local brute force, and pass-the-hash authentication re-use attempts against local account hashes. " +
                    "NTLM authentication failure events (4776 Failure) are the primary detection signal for NTLM relay attacks — when an attacker captures an NTLM challenge-response and relays it to a different server, the relay attempt generates authentication failure events with the source workstation name visible. Pass-the-Hash attempts against local accounts (using a harvested NTLM hash to authenticate to SMB) also generate 4776 Failure events from unexpected source machines. These events feed the 'NTLM authentication anomaly' SIEM detection rules.",
                Tags = ["account-mgmt-audit", "credential-validation", "ntlm", "4776", "pass-the-hash", "relay-attack"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "NTLM authentication failures audited (Event 4776); NTLM relay and pass-the-hash attacks generate detectable event patterns.",
                ApplyOps = [RegOp.SetDword(Key, "AuditCredentialValidation", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditCredentialValidation")],
                DetectOps = [RegOp.CheckDword(Key, "AuditCredentialValidation", 3)],
            },
            new TweakDef
            {
                Id = "acmgmtaudit-enable-kerberos-service-ticket-audit",
                Label = "Account Mgmt Audit: Enable Kerberos Service Ticket Auditing for Ticket Attack Detection",
                Category = "Security",
                Description = "Sets AuditKerberosServiceTicket=3 (Success+Failure) in the Advanced Audit Policy Account Logon category. Generates Security events 4769 (Kerberos service ticket request — success), 4770 (Kerberos service ticket renew), and 4771 (Kerberos pre-authentication failure) for all Kerberos ticket-granting service (TGS) requests, enabling detection of Kerberoasting attacks that request service tickets for all SPNs to offline crack their RC4-encrypted password hashes. " +
                    "Kerberoasting is one of the most common Active Directory attack techniques: any domain user can request a TGS for any service principal name, and if the service account's domain password is RC4-encrypted in the ticket (etype 0x17), the ticket can be taken offline for brute force password cracking without triggering any lockout. Auditing Kerberos TGS requests generates Event 4769 for each SPN ticket request — a Kerberoasting scan (requesting TGS for all SPNs in rapid succession) creates a distinctive volume and timing pattern detectable by SIEM.",
                Tags = ["account-mgmt-audit", "kerberos", "4769", "kerberoasting", "tgs", "service-ticket"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Kerberos TGS requests audited (Event 4769); Kerberoasting SPN scan generates distinctive volume pattern detectable by SIEM.",
                ApplyOps = [RegOp.SetDword(Key, "AuditKerberosServiceTicket", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditKerberosServiceTicket")],
                DetectOps = [RegOp.CheckDword(Key, "AuditKerberosServiceTicket", 3)],
            },
        ];

    }

    // ── AuditEventPolicy ──
    private static class _AuditEventPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Audit";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "audevt-audit-logon-events",
                Label = "Enable Audit Policy for Logon Success and Failure Events",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Logon event auditing generates security log entries for every interactive, network, and service logon attempt including both successful and failed authentications. Enabling logon audit success and failure events provides the foundation for detecting brute force attacks, unauthorized access, and suspicious authentication patterns. Failed logon events reveal password spray attacks where an attacker attempts common passwords against multiple accounts. Successful logons from unexpected locations or times indicate potential account compromise that requires immediate investigation. Logon events should be forwarded to SIEM in real time for correlation with threat intelligence and behavioral analytics. The volume of logon events on domain controllers is high so appropriate log size and retention policies must be configured.",
                Tags = ["audit", "logon", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TokenRightAuditLogon", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "TokenRightAuditLogon")],
                DetectOps = [RegOp.CheckDword(Key, "TokenRightAuditLogon", 3)],
            },
            new TweakDef
            {
                Id = "audevt-audit-account-management",
                Label = "Enable Audit Policy for Account Management Changes",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Account management auditing generates events for user account creation, modification, deletion, password changes, and group membership changes. Enabling account management audit success and failure events creates visibility into identity lifecycle operations for security monitoring and compliance. Unauthorized account creation is a common technique used by attackers to establish persistent access after initial compromise. Privilege escalation through unauthorized group membership changes is detectable through account management audit events. Account management events should be reviewed for out-of-band changes that occur outside of approved IT service management processes. SIEM correlation of account management events with expected change management tickets helps identify unauthorized identity changes.",
                Tags = ["audit", "account-management", "identity", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditAccountManagement", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditAccountManagement")],
                DetectOps = [RegOp.CheckDword(Key, "AuditAccountManagement", 3)],
            },
            new TweakDef
            {
                Id = "audevt-audit-privilege-use",
                Label = "Enable Audit Policy for Sensitive Privilege Use Events",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Privilege use auditing records events when sensitive Windows privileges like SeDebugPrivilege, SeTakeOwnershipPrivilege, or SeLoadDriverPrivilege are exercised. Enabling privilege use audit for sensitive privileges detects processes that abuse elevated rights for post-exploitation activities. SeDebugPrivilege allows reading any process's memory which is commonly used by credential-stealing malware to access LSASS. Unexpected use of sensitive privileges by non-standard processes or non-administrative users indicates potential privilege abuse or malware activity. Privilege use events generate significant volume especially SeSecurityPrivilege so selective auditing of the most sensitive privileges is recommended. SIEM alerts for privilege use by unexpected processes or outside of expected maintenance windows help identify suspicious activity.",
                Tags = ["audit", "privileges", "sensitive-rights", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditPrivilegeUse", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditPrivilegeUse")],
                DetectOps = [RegOp.CheckDword(Key, "AuditPrivilegeUse", 2)],
            },
            new TweakDef
            {
                Id = "audevt-audit-policy-change",
                Label = "Enable Audit Policy for Security Policy Changes",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Security policy change auditing generates events when audit policies, trust policies, or security configuration changes are made on a system. Enabling policy change audit success and failure events provides visibility into security posture modifications that could weaken the system's defenses. Attackers with administrator access often disable audit logging as a first step to cover their tracks making policy change auditing critical. Policy change events should be monitored in real time with immediate alerts for audit log clearing or audit policy changes. Group Policy change events help detect when unauthorized changes have been pushed through GPO or when GPO objects have been modified. Policy change auditing should be protected with tamper-evident log forwarding to ensure audit records persist even if local logs are cleared.",
                Tags = ["audit", "policy-change", "security-config", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditPolicyChange", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditPolicyChange")],
                DetectOps = [RegOp.CheckDword(Key, "AuditPolicyChange", 3)],
            },
            new TweakDef
            {
                Id = "audevt-audit-object-access",
                Label = "Enable Audit Policy for File and Registry Object Access",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Object access auditing generates events when Windows objects including files, registry keys, and other resources with configured SACLs are accessed. Enabling object access auditing with failure events reveals unauthorized access attempts to protected resources like sensitive files and critical registry keys. Object access audit success events support data loss prevention monitoring by recording access to sensitive document repositories. Object access auditing must be combined with configuring SACLs on specific objects to generate access events ensuring only relevant objects generate audit traffic. High-value data stores should have SACLs configured to log all access attempts while lower-sensitivity resources can log only failures. Object access audit logs should be reviewed regularly to identify unusual access patterns or inappropriate data access.",
                Tags = ["audit", "object-access", "file-access", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditObjectAccess", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditObjectAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditObjectAccess", 2)],
            },
            new TweakDef
            {
                Id = "audevt-audit-process-creation",
                Label = "Enable Audit Policy for Process Creation Events",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Process creation auditing generates event 4688 for each new process started including the full command line when command line auditing is also enabled. Enabling process creation auditing with command line content capture is one of the most valuable audit policies for detecting malware execution and attacker activity. Command line data in process creation events reveals the full command used to launch malicious tools including PowerShell scripts, lateral movement tools, and credential dumpers. Process creation events provide the execution history necessary to reconstruct attacker activities during incident response investigations. Process creation auditing data should be forwarded to SIEM and analyzed with behavioral analytics to detect suspicious execution patterns. The combination of process creation audit and PowerShell ScriptBlock logging provides near-complete visibility into malicious script execution.",
                Tags = ["audit", "process-creation", "execution", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditProcessCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditProcessCreation")],
                DetectOps = [RegOp.CheckDword(Key, "AuditProcessCreation", 1)],
            },
            new TweakDef
            {
                Id = "audevt-audit-logon-special",
                Label = "Enable Audit Policy for Special Logon Events",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Special logon auditing generates events for logons using administrator-equivalent accounts or accounts assigned sensitive privileges like SeTcbPrivilege. Enabling special logon auditing provides visibility into privileged session establishment that represents a higher risk than normal user logons. Special logons to servers by accounts with domain administrator or service account privileges should be scrutinized for legitimacy. Special logon events help identify service accounts being used interactively which may indicate credential theft or inappropriate account use. Privileged access workstations and jump servers should have special logon auditing enabled and the events closely monitored. Special logon events correlating with suspicious activity from the same source IP or user account indicate potential credential compromise.",
                Tags = ["audit", "special-logon", "privileged-access", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditSpecialLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSpecialLogon")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSpecialLogon", 1)],
            },
            new TweakDef
            {
                Id = "audevt-audit-directory-service",
                Label = "Enable Audit Policy for Active Directory Service Changes",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Directory service audit events record changes to Active Directory objects including user account modifications, group changes, and computer object updates. Enabling directory service change auditing on domain controllers provides visibility into all Active Directory modifications. Unauthorized modifications to Active Directory such as adding users to privileged groups or changing password policies are detectable through directory service auditing. DCSync attacks where an attacker requests directory replication to extract password hashes generate directory service events. Directory service auditing data is essential for Active Directory security monitoring and is required for most enterprise SIEM detections against identity attacks. Directory service events should be collected with real-time forwarding to SIEM for immediate detection of suspicious AD modifications.",
                Tags = ["audit", "active-directory", "directory-service", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditDSAccess", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditDSAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditDSAccess", 3)],
            },
            new TweakDef
            {
                Id = "audevt-audit-ipsec-extended",
                Label = "Enable Extended Audit Policy for IPSec Events",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "IPSec audit events record the establishment and failure of IPSec security associations providing visibility into network security policy enforcement. Enabling IPSec audit events helps monitor compliance with network isolation and encryption policies that rely on IPSec for enforcement. IPSec failure events indicate hosts that are failing to establish required security associations which may indicate configuration issues or active attacks. Dropped connections due to IPSec policy failures are recorded and can be analyzed to identify misconfigured endpoints or unauthorized devices attempting to bypass network isolation. IPSec audit data helps validate that domain isolation policies are being correctly enforced across all in-scope subnets. IPSec extended mode events also capture IKE negotiation details useful for troubleshooting VPN and site-to-site encrypted network issues.",
                Tags = ["audit", "ipsec", "network-security", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditIPsec", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditIPsec")],
                DetectOps = [RegOp.CheckDword(Key, "AuditIPsec", 3)],
            },
            new TweakDef
            {
                Id = "audevt-audit-security-system-extension",
                Label = "Enable Audit Policy for Security System Extension Loading",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Security system extension auditing generates events when authentication packages, security packages, notify packages, or security system services are installed or loaded. Enabling security system extension auditing detects malicious authentication packages and notification packages that are common kernel-level persistence mechanisms. Attackers install malicious authentication packages to intercept credentials or implement backdoor authentication bypassing normal Windows security. Security system extension events on domain controllers should be closely monitored as malicious authentication packages there affect the entire domain. Authentication package loading events should be verified against the known-good list of authorized authentication extensions for each system. Security system extension audit is a valuable detection control for sophisticated attacks that modify the Windows security subsystem.",
                Tags = ["audit", "security-extension", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditSecuritySystemExtension", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSecuritySystemExtension")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSecuritySystemExtension", 1)],
            },
        ];

    }

    // ── AuditPolicyAdvancedPolicy ──
    private static class _AuditPolicyAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Audit";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "auditadv-force-subcategory-policy",
                    Label = "Force Audit Policy Subcategory Settings Over Category",
                    Category = "Security",
                    Description =
                        "Forces Windows to use advanced audit policy subcategory settings (configured via auditpol.exe or Group Policy Advanced Audit) rather than the basic per-category settings from the local security policy, enabling fine-grained audit control.",
                    Tags = ["audit", "audit-policy", "subcategory", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Advanced audit subcategory settings take precedence over basic category settings; fine-grained audit enabled.",
                    ApplyOps = [RegOp.SetDword(Key, "SCENoApplyLegacyAuditPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SCENoApplyLegacyAuditPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "SCENoApplyLegacyAuditPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "auditadv-enable-pnp-activity-audit",
                    Label = "Enable Plug and Play Activity Audit",
                    Category = "Security",
                    Description =
                        "Enables auditing of Plug and Play device connections and disconnections, generating Security event 6416 for each new external device plugged in, supporting exfiltration investigations via USB/Thunderbolt devices.",
                    Tags = ["audit", "pnp", "usb", "device-connection", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "PnP device audit enabled; every external device connection logged as Security event 6416.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditPNPActivity", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditPNPActivity")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditPNPActivity", 1)],
                },
                new TweakDef
                {
                    Id = "auditadv-enable-removable-storage-audit",
                    Label = "Enable Removable Storage Object Access Audit",
                    Category = "Security",
                    Description =
                        "Enables auditing of read and write access to removable storage devices, generating Security event 4663 entries for file access on USB drives, SD cards, and other removable media.",
                    Tags = ["audit", "removable-storage", "file-access", "usb", "dlp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Removable storage access audited; file reads and writes to USB/SD logged as Security event 4663.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditRemovableStorage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditRemovableStorage")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditRemovableStorage", 1)],
                },
                new TweakDef
                {
                    Id = "auditadv-enable-token-right-adjusted-audit",
                    Label = "Enable Token Right Adjustment Audit",
                    Category = "Security",
                    Description =
                        "Enables auditing of privilege adjustments (token right changes) such as SeDebugPrivilege, SeLoadDriverPrivilege activations, generating Security event 4703 to track privilege escalation attempts.",
                    Tags = ["audit", "privilege", "token-rights", "escalation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Token right adjustment audited; privilege escalation attempts logged as Security event 4703.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditTokenRightAdjusted", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditTokenRightAdjusted")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditTokenRightAdjusted", 1)],
                },
                new TweakDef
                {
                    Id = "auditadv-enable-user-account-management-audit",
                    Label = "Enable User Account Management Success and Failure Audit",
                    Category = "Security",
                    Description =
                        "Enables both success and failure auditing of user account management operations (account creation, modification, deletion, password reset, enable/disable) generating Security events 4720-4767 for compliance.",
                    Tags = ["audit", "user-accounts", "account-management", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "User account management audited (success+failure); account lifecycle events logged for compliance.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditUserAccountManagement", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditUserAccountManagement")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditUserAccountManagement", 3)],
                },
                new TweakDef
                {
                    Id = "auditadv-enable-sensitive-privilege-use-audit",
                    Label = "Enable Sensitive Privilege Use Audit",
                    Category = "Security",
                    Description =
                        "Enables auditing of sensitive privilege use (e.g., acting as part of OS, taking ownership, restoring files), generating Security event 4673/4674 entries to detect abuse of powerful administrative rights.",
                    Tags = ["audit", "privilege-use", "sensitive-privileges", "admin-abuse", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Sensitive privilege use audited; high-value privilege activations logged as Security events 4673/4674.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditSensitivePrivilegeUse", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditSensitivePrivilegeUse")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditSensitivePrivilegeUse", 1)],
                },
                new TweakDef
                {
                    Id = "auditadv-enable-ipsec-driver-audit",
                    Label = "Enable IPsec Driver Audit",
                    Category = "Security",
                    Description =
                        "Enables auditing of IPsec driver events including filter match, connection establishment, and connection drop events, supporting network security posture monitoring and VPN tunnel activity auditing.",
                    Tags = ["audit", "ipsec", "vpn", "network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "IPsec driver events audited; filter matches and tunnel events logged for network security monitoring.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditIPsecDriver", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditIPsecDriver")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditIPsecDriver", 1)],
                },
                new TweakDef
                {
                    Id = "auditadv-enable-wfp-audit",
                    Label = "Enable Windows Filtering Platform (WFP) Audit",
                    Category = "Security",
                    Description =
                        "Enables auditing of Windows Filtering Platform connection permit and drop events, generating Security events 5031, 5152-5158 to support network activity analysis and firewall rule effectiveness reviews.",
                    Tags = ["audit", "wfp", "firewall", "network", "connection", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WFP connections audited; firewall permit and drop decisions logged as Security events 5152-5158.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditFilteringPlatform", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditFilteringPlatform")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditFilteringPlatform", 1)],
                },
                new TweakDef
                {
                    Id = "auditadv-enable-registry-audit",
                    Label = "Enable Registry Object Access Audit",
                    Category = "Security",
                    Description =
                        "Enables auditing of registry key access and modifications when an object SACL is present, supporting post-incident forensics by recording which processes accessed security-sensitive registry keys.",
                    Tags = ["audit", "registry", "object-access", "forensics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Registry object access audited; registry key reads/writes logged when SACL is set on the key.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditRegistryAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditRegistryAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditRegistryAccess", 1)],
                },
                new TweakDef
                {
                    Id = "auditadv-disable-audit-policy-change-by-user",
                    Label = "Block Audit Policy Changes by Non-Admin Users",
                    Category = "Security",
                    Description =
                        "Prevents non-administrator users from modifying audit policy settings via auditpol.exe or the Security Policy snap-in, ensuring the audit configuration cannot be weakened by standard users or compromised service accounts.",
                    Tags = ["audit", "audit-policy", "tamper-protection", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Audit policy changes blocked for non-admins; security audit configuration is tamper-resistant.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUserAuditPolicyChange", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUserAuditPolicyChange")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUserAuditPolicyChange", 1)],
                },
            ];

    }

    // ── DiagnosticDataViewerPolicy ──
    private static class _DiagnosticDataViewerPolicy
    {
        private const string DataCol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "diagdvr-disable-viewer",
                Label = "Telemetry: Disable the Diagnostic Data Viewer app",
                Category = "Security",
                Description =
                    "Sets DisableDiagnosticDataViewer=1. Prevents end users from opening the Diagnostic "
                    + "Data Viewer app to inspect telemetry sent to Microsoft, reducing data-disclosure risk.",
                Tags = ["telemetry", "diagnostic", "viewer", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(DataCol, "DisableDiagnosticDataViewer", 1)],
                RemoveOps = [RegOp.DeleteValue(DataCol, "DisableDiagnosticDataViewer")],
                DetectOps = [RegOp.CheckDword(DataCol, "DisableDiagnosticDataViewer", 1)],
            },
            new TweakDef
            {
                Id = "diagdvr-disable-device-health-attestation",
                Label = "Telemetry: Disable Device Health Attestation service reporting",
                Category = "Security",
                Description =
                    "Sets AllowDeviceHealthAttestationService=0. Prevents Windows from uploading "
                    + "boot-state measurements to the Microsoft Device Health Attestation cloud service.",
                Tags = ["telemetry", "health-attestation", "tpm", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(DataCol, "AllowDeviceHealthAttestationService", 0)],
                RemoveOps = [RegOp.DeleteValue(DataCol, "AllowDeviceHealthAttestationService")],
                DetectOps = [RegOp.CheckDword(DataCol, "AllowDeviceHealthAttestationService", 0)],
            },
            new TweakDef
            {
                Id = "diagdvr-limit-diagnostic-log-collection",
                Label = "Telemetry: Limit diagnostic log collection for Windows Update",
                Category = "Security",
                Description =
                    "Sets LimitDiagnosticLogCollection=1. Restricts the volume of diagnostic logs "
                    + "collected from the device and uploaded during Windows Update servicing operations.",
                Tags = ["telemetry", "diagnostic", "logs", "windows-update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(DataCol, "LimitDiagnosticLogCollection", 1)],
                RemoveOps = [RegOp.DeleteValue(DataCol, "LimitDiagnosticLogCollection")],
                DetectOps = [RegOp.CheckDword(DataCol, "LimitDiagnosticLogCollection", 1)],
            },
            new TweakDef
            {
                Id = "diagdvr-disable-enterprise-auth-proxy",
                Label = "Telemetry: Disable enterprise auth-proxy for telemetry uploads",
                Category = "Security",
                Description =
                    "Sets DisableEnterpriseAuthProxy=1. Prevents the Connected User Experiences service "
                    + "from using Authenticated Proxy to send telemetry, forcing direct connection only.",
                Tags = ["telemetry", "proxy", "enterprise", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(DataCol, "DisableEnterpriseAuthProxy", 1)],
                RemoveOps = [RegOp.DeleteValue(DataCol, "DisableEnterpriseAuthProxy")],
                DetectOps = [RegOp.CheckDword(DataCol, "DisableEnterpriseAuthProxy", 1)],
            },
            new TweakDef
            {
                Id = "diagdvr-disable-onesettings-auditing",
                Label = "Telemetry: Disable OneSettings diagnostic auditing",
                Category = "Security",
                Description =
                    "Sets EnableOneSettingsAuditing=0. Prevents Windows from recording a local audit log "
                    + "of each OneSettings configuration payload fetched from Microsoft cloud endpoints.",
                Tags = ["telemetry", "onesettings", "audit", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(DataCol, "EnableOneSettingsAuditing", 0)],
                RemoveOps = [RegOp.DeleteValue(DataCol, "EnableOneSettingsAuditing")],
                DetectOps = [RegOp.CheckDword(DataCol, "EnableOneSettingsAuditing", 0)],
            },
            new TweakDef
            {
                Id = "diagdvr-disable-update-compliance-processing",
                Label = "Telemetry: Disable Update Compliance telemetry processing",
                Category = "Security",
                Description =
                    "Sets AllowUpdateComplianceProcessing=0. Prevents the device from sending telemetry "
                    + "to the Windows Update Compliance cloud analytics workspace.",
                Tags = ["telemetry", "update-compliance", "analytics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(DataCol, "AllowUpdateComplianceProcessing", 0)],
                RemoveOps = [RegOp.DeleteValue(DataCol, "AllowUpdateComplianceProcessing")],
                DetectOps = [RegOp.CheckDword(DataCol, "AllowUpdateComplianceProcessing", 0)],
            },
            new TweakDef
            {
                Id = "diagdvr-disable-wufb-cloud-processing",
                Label = "Telemetry: Disable Windows Update for Business cloud processing",
                Category = "Security",
                Description =
                    "Sets AllowWUfBCloudProcessing=0. Prevents the device from sending telemetry to the "
                    + "Windows Update for Business cloud processing pipeline.",
                Tags = ["telemetry", "wufb", "cloud", "windows-update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(DataCol, "AllowWUfBCloudProcessing", 0)],
                RemoveOps = [RegOp.DeleteValue(DataCol, "AllowWUfBCloudProcessing")],
                DetectOps = [RegOp.CheckDword(DataCol, "AllowWUfBCloudProcessing", 0)],
            },
            new TweakDef
            {
                Id = "diagdvr-disable-desktop-analytics",
                Label = "Telemetry: Disable Desktop Analytics/Endpoint Analytics telemetry",
                Category = "Security",
                Description =
                    "Sets AllowDesktopAnalyticsProcessing=0. Stops the device from contributing "
                    + "telemetry to Microsoft Desktop Analytics and Endpoint Analytics workloads.",
                Tags = ["telemetry", "desktop-analytics", "intune", "analytics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(DataCol, "AllowDesktopAnalyticsProcessing", 0)],
                RemoveOps = [RegOp.DeleteValue(DataCol, "AllowDesktopAnalyticsProcessing")],
                DetectOps = [RegOp.CheckDword(DataCol, "AllowDesktopAnalyticsProcessing", 0)],
            },
            new TweakDef
            {
                Id = "diagdvr-disable-commercial-data-pipeline",
                Label = "Telemetry: Disable commercial data pipeline telemetry upload",
                Category = "Security",
                Description =
                    "Sets AllowCommercialDataPipeline=0. Prevents Windows from routing diagnostic data "
                    + "through the commercial telemetry pipeline used by enterprise monitoring solutions.",
                Tags = ["telemetry", "commercial", "pipeline", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(DataCol, "AllowCommercialDataPipeline", 0)],
                RemoveOps = [RegOp.DeleteValue(DataCol, "AllowCommercialDataPipeline")],
                DetectOps = [RegOp.CheckDword(DataCol, "AllowCommercialDataPipeline", 0)],
            },
            new TweakDef
            {
                Id = "diagdvr-limit-enhanced-diagnostic-data",
                Label = "Telemetry: Limit enhanced diagnostic data for Windows Analytics",
                Category = "Security",
                Description =
                    "Sets LimitEnhancedDiagnosticDataWindowsAnalytics=0. When telemetry is set to Enhanced, "
                    + "this policy further limits the enhanced-tier subset sent to Windows Analytics.",
                Tags = ["telemetry", "enhanced", "analytics", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(DataCol, "LimitEnhancedDiagnosticDataWindowsAnalytics", 0)],
                RemoveOps = [RegOp.DeleteValue(DataCol, "LimitEnhancedDiagnosticDataWindowsAnalytics")],
                DetectOps = [RegOp.CheckDword(DataCol, "LimitEnhancedDiagnosticDataWindowsAnalytics", 0)],
            },
        ];

    }

    // ── DsObjectAccessAuditPolicy ──
    private static class _DsObjectAccessAuditPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\DS Access";
        private const string DetailKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Detailed Tracking";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "dsaudit-audit-directory-service-access",
                Label = "DS Audit: Enable Directory Service Object Access Auditing (LDAP Reads to Sensitive AD Objects)",
                Category = "Security",
                Description = "Sets AuditDirectoryServiceAccess=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates Security event 4661 for SACL-triggered access to Active Directory objects on domain controllers — user objects, group objects, GPO links, schema attributes, and AdminSDHolder-protected objects — providing on-DC audit records of all access to sensitive AD data. " +
                    "Active Directory is the crown jewel of the enterprise identity infrastructure. Without directory service access auditing, an attacker who performs an LDAP dump of all user objects (including password hint attributes, lastLogon, adminCount, userAccountControl enumeration) leaves no Security event log trace on the domain controller. With SACL-protected sensitive AD objects (all adminCount=1 objects, GPO objects, schema), directory service access events generate on every LDAP read, enabling DCSync detection and AD reconnaissance identification.",
                Tags = ["ds-audit", "directory-service", "active-directory", "ldap", "dcsync", "sacl"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "AD sensitive object SACL access events generated; DCSync attack (drsuapi replication) generates detection events.",
                ApplyOps = [RegOp.SetDword(Key, "AuditDirectoryServiceAccess", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditDirectoryServiceAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditDirectoryServiceAccess", 3)],
            },
            new TweakDef
            {
                Id = "dsaudit-audit-directory-service-changes",
                Label = "DS Audit: Enable Directory Service Object Modification Auditing (AD Object Changes)",
                Category = "Security",
                Description = "Sets AuditDirectoryServiceChanges=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates Security events 5136 (attribute modified), 5137 (object created), 5138 (object restored from tombstone), 5139 (object moved), 5141 (object deleted) for all changes to Active Directory objects, providing a granular changelog of AD modifications. " +
                    "Event 5136 is the AD schema-level modification record — it captures every attribute write to every AD object (user, group, computer, GPO, schema). Without this auditing subcategory enabled on domain controllers, the SOC has no event log record of Group Policy Object (GPO) modifications, AdminSDHolder ACL changes, Service Principal Name (SPN) additions (Kerberoasting target creation), or Domain Trust modifications (trust injection). SOC SIEM rules for GPO modification, persistence SPN addition, and trust injection all depend on Event 5136.",
                Tags = ["ds-audit", "directory-service-changes", "event-5136", "gpo", "spn", "trust-injection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "AD attribute changes logged (Event 5136); GPO modifications, SPN additions, and trust changes generate SOC detection events.",
                ApplyOps = [RegOp.SetDword(Key, "AuditDirectoryServiceChanges", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditDirectoryServiceChanges")],
                DetectOps = [RegOp.CheckDword(Key, "AuditDirectoryServiceChanges", 3)],
            },
            new TweakDef
            {
                Id = "dsaudit-audit-directory-service-replication",
                Label = "DS Audit: Enable Directory Service Replication Auditing for DCSync Detection",
                Category = "Security",
                Description = "Sets AuditDirectoryServiceReplication=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates Security events 4928 (source naming context established — replication initiated), 4929 (source naming context removed), 4930 (source naming context modified), 4931 (destination naming context modified) for AD replication operations. Enables detection of DCSync attacks performed by non-DC machines invoking DS-Replication-Get-Changes privileges. " +
                    "DCSync (Mimikatz's lsadump::dcsync) mimics the behaviour of a domain controller requesting replication from another DC to obtain all account password hashes without requiring local access to the DC. The attack uses DS-Replication-Get-Changes-All privileges. Replication audit events (4928) are generated on the target DC when the replication request arrives. A 4928 event from a client workstation (not a domain controller) is a high-fidelity DCSync detection signal.",
                Tags = ["ds-audit", "replication", "event-4928", "dcsync", "ds-replication", "mimikatz"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "DS replication events audited; DCSync attack from non-DC machines generates Event 4928 — high-fidelity detection signal.",
                ApplyOps = [RegOp.SetDword(Key, "AuditDirectoryServiceReplication", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditDirectoryServiceReplication")],
                DetectOps = [RegOp.CheckDword(Key, "AuditDirectoryServiceReplication", 3)],
            },
            new TweakDef
            {
                Id = "dsaudit-audit-detailed-replication",
                Label = "DS Audit: Enable Detailed Directory Service Replication Auditing",
                Category = "Security",
                Description = "Sets AuditDetailedDirectoryServiceReplication=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates verbose Security events 4932/4933 (synchronization of a naming context has begun/ended) and 4934/4935/4937 (attribute of AD object replicated/failed/lingering object removed) for each object-level attribute synchronisation step during AD replication, providing attribute-granular replication change records. " +
                    "Detailed replication auditing provides the object-level granularity missing from standard replication auditing. When a naming context replication session (Event 4928) encompasses thousands of object changes, the standard events identify that replication occurred but not which specific objects or attributes were synchronised. Detailed replication events (4932/4934) identify the specific objects replicated in each session, enabling investigation of which specific accounts were targeted in a DCSync attack session.",
                Tags = ["ds-audit", "detailed-replication", "event-4932", "naming-context", "dcsync-detail"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Detailed replication events generated; specific objects and attributes synchronised during DCSync sessions are identifiable.",
                ApplyOps = [RegOp.SetDword(Key, "AuditDetailedDirectoryServiceReplication", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditDetailedDirectoryServiceReplication")],
                DetectOps = [RegOp.CheckDword(Key, "AuditDetailedDirectoryServiceReplication", 3)],
            },
            new TweakDef
            {
                Id = "dsaudit-enable-dpapi-activity-audit",
                Label = "DS Audit: Enable DPAPI Activity Auditing for Master Key Access Monitoring",
                Category = "Security",
                Description = "Sets AuditDPAPIActivity=3 (Success+Failure) in Advanced Audit Policy Detailed Tracking category. Generates Security events 4692 (DPAPI backup key was requested), 4693 (DPAPI data was decrypted), 4694 (DPAPI data was encrypted), 4695 (DPAPI data was decrypted in unprotected state) for all DPAPI encryption and decryption operations. Enables detection of DPAPI master key harvesting attacks. " +
                    "DPAPI master key backup operations (Event 4692) are generated when a new DPAPI master key is created and its backup is sent to the domain controller for recovery purposes. In DPAPI masterkey harvesting attacks (used by NanoDump, SharpDPAPI), an attacker requests the DPAPI backup key from the domain controller to decrypt all locally cached DPAPI blobs across the enterprise. Event 4692 from an unexpected non-system principal is a binary indicator of DPAPI master key interception.",
                Tags = ["ds-audit", "dpapi", "event-4692", "master-key", "credential-decryption", "sharpdpapi"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "DPAPI master key access events (4692) generated; DPAPI backup key harvesting attack immediately detectable.",
                ApplyOps = [RegOp.SetDword(DetailKey, "AuditDPAPIActivity", 3)],
                RemoveOps = [RegOp.DeleteValue(DetailKey, "AuditDPAPIActivity")],
                DetectOps = [RegOp.CheckDword(DetailKey, "AuditDPAPIActivity", 3)],
            },
            new TweakDef
            {
                Id = "dsaudit-enable-rpc-events-audit",
                Label = "DS Audit: Enable RPC Events Auditing for Remote Service Call Monitoring",
                Category = "Security",
                Description = "Sets AuditRPCEvents=3 (Success+Failure) in Advanced Audit Policy Detailed Tracking category. Generates Security event 5712 (RPC connection attempt) for remote procedure call connections with caller identity, target interface UUID, and endpoint information — enabling detection of RPC-based lateral movement techniques that use Windows RPC interfaces (MS-SAMR, MS-LSAD, MS-DRSR, MS-RPRN) to access remote system resources. " +
                    "Remote Printer Spooler (MS-RPRN) exploitation (PrintNightmare) and RPC-based DCSync (MS-DRSR interface calls) are primary RPC-based attack techniques. Without RPC event auditing, there is no Security event log record of specific Windows RPC interface calls made to an endpoint. RPC event audit enables detection of PrintNightmare exploitation (unexpected MS-RPRN calls from non-print-server machines) and RPC-based credential access attempts targeting SAMR and LSAD interfaces.",
                Tags = ["ds-audit", "rpc", "event-5712", "printnightmare", "ms-rprn", "samr", "lsad"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "RPC connection events (5712) generated; PrintNightmare MS-RPRN and SAMR/LSAD-based credential access detectable.",
                ApplyOps = [RegOp.SetDword(DetailKey, "AuditRPCEvents", 3)],
                RemoveOps = [RegOp.DeleteValue(DetailKey, "AuditRPCEvents")],
                DetectOps = [RegOp.CheckDword(DetailKey, "AuditRPCEvents", 3)],
            },
            new TweakDef
            {
                Id = "dsaudit-enable-central-access-policy-staging",
                Label = "DS Audit: Enable Central Access Policy Staging Audit for DAC Rule Pre-Deployment Testing",
                Category = "Security",
                Description = "Sets AuditCentralAccessPolicyStaging=1 (Success) in Advanced Audit Policy DS Access category. Generates Security event 4818 (proposed Central Access Policy does not grant the same access permissions as the current Central Access Policy) when a proposed Dynamic Access Control policy being tested in staging mode would grant different access than the currently active policy, identifying files that would change access before the policy is deployed. " +
                    "Central Access Policy staging is the Windows DAC mechanism for safely testing new classification policies before deploying them to production. Without staging audit events, IT cannot determine the blast radius of a new DAC policy change — which files would gain new access grants, which would lose existing access. Event 4818 provides a non-destructive preview showing exactly which resources would receive different access treatment under the proposed policy vs the current policy.",
                Tags = ["ds-audit", "central-access-policy", "dac", "staging", "event-4818", "policy-testing"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "DAC staging audit events (4818) generated; policy impact assessment before deployment identifies access changes without risk.",
                ApplyOps = [RegOp.SetDword(Key, "AuditCentralAccessPolicyStaging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditCentralAccessPolicyStaging")],
                DetectOps = [RegOp.CheckDword(Key, "AuditCentralAccessPolicyStaging", 1)],
            },
            new TweakDef
            {
                Id = "dsaudit-enable-certificate-services-audit",
                Label = "DS Audit: Enable Active Directory Certificate Services Audit for CA Operation Monitoring",
                Category = "Security",
                Description = "Sets AuditCertificationServices=3 (Success+Failure) in Advanced Audit Policy Object Access category. Generates Security events 4870/4871/4872/4873/4874/4875/4876/4877 for all Active Directory Certificate Services operations — certificate requests (approved, denied, pending), certificate revocations, certificate template modifications, and CA role service start/stop. Critical for detecting AD CS-based privilege escalation (ESC1–ESC8 attacks). " +
                    "AD Certificate Services attacks (ESC1–ESC8, as catalogued by SpecterOps) enable low-privilege users to obtain certificates that can be used for domain admin authentication or persistent machine authentication bypass. Without CS audit events, a user who requests and receives a certificate through a misconfigured template (ESC1: SANs allowed by requester) generates no Security alert. Certificate request events (4886: certificate requested, 4887: certificate issued) record the subject, certificate template, and requester — enabling detection of privilege-elevating certificate requests.",
                Tags = ["ds-audit", "ad-cs", "certificate-services", "event-4887", "esc1", "privilege-escalation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "AD CS operations audited; ESC1-ESC8 certificate template abuse and rogue CA operations generate Security events.",
                ApplyOps = [RegOp.SetDword(Key, "AuditCertificationServices", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditCertificationServices")],
                DetectOps = [RegOp.CheckDword(Key, "AuditCertificationServices", 3)],
            },
            new TweakDef
            {
                Id = "dsaudit-audit-filtering-platform-connection",
                Label = "DS Audit: Enable Windows Filtering Platform Connection Auditing for Network Profiling",
                Category = "Security",
                Description = "Sets AuditFilteringPlatformConnection=3 (Success+Failure) in Advanced Audit Policy Object Access category. Generates Security events 5031 (WFP application blocked), 5150/5151 (WFP packet blocked/dropped), 5156/5157 (WFP connection allowed/blocked by application) for Windows Filtering Platform (Windows Firewall) connection decisions, providing process-to-network socket binding records without requiring Sysmon Event ID 3. " +
                    "WFP connection allowed/blocked events (5156/5157) provide the same process-to-network binding information as Sysmon Event 3 but natively through Windows Security event log. Organisations that cannot deploy Sysmon can achieve equivalent network visibility using WFP auditing. Event 5156 records the process making the connection, the destination IP/port, and the protocol — enabling detection of command-and-control beaconing, lateral movement SMB connections, and data exfiltration to external IP ranges.",
                Tags = ["ds-audit", "wfp", "windows-firewall", "event-5156", "c2-detection", "network-profiling"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "WFP connection events (5156) generated natively; C2 beaconing and lateral movement network connections logged without Sysmon.",
                ApplyOps = [RegOp.SetDword(Key, "AuditFilteringPlatformConnection", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditFilteringPlatformConnection")],
                DetectOps = [RegOp.CheckDword(Key, "AuditFilteringPlatformConnection", 3)],
            },
            new TweakDef
            {
                Id = "dsaudit-audit-handle-manipulation",
                Label = "DS Audit: Enable Handle Manipulation Auditing for LSASS Memory Access Detection",
                Category = "Security",
                Description = "Sets AuditHandleManipulation=3 (Success+Failure) in Advanced Audit Policy Object Access category. Generates Security event 4658 (handle to object closed) and event 4690 (attempt to duplicate handle to object) that complement the SACL-based object access events — specifically Event 4690 which records attempts to duplicate an open handle to a sensitive object (such as an LSASS process handle) to a different process. " +
                    "Process handle duplication is an advanced LSASS dump technique used to avoid the more detectable direct process access calls. Tools like x64dump and some variants of Cobalt Strike's in-memory credential extraction duplicate an existing handle to the LSASS process (owned by csrss.exe or another trusted process) rather than opening a new handle from a suspicious process. Event 4690 captures this handle duplication attempt, providing detection for handle-based LSASS access that bypasses protection based solely on process open calls.",
                Tags = ["ds-audit", "handle-manipulation", "event-4690", "lsass", "handle-duplication", "credential-theft"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Handle duplication (Event 4690) audited; LSASS credential dump via handle duplication technique generates detection events.",
                ApplyOps = [RegOp.SetDword(Key, "AuditHandleManipulation", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditHandleManipulation")],
                DetectOps = [RegOp.CheckDword(Key, "AuditHandleManipulation", 3)],
            },
        ];

    }

    // ── ErrorReportingPolicy ──
    private static class _ErrorReportingPolicy
    {
        private const string WerPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";
        private const string WerConsent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting\Consent";
        private const string WerQueue = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting\ExcludedApplications";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "werpol-disable-wer",
                Label = "Disable Windows Error Reporting via Group Policy",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Tags = ["wer", "error reporting", "crash", "privacy", "group policy"],
                Description =
                    "Disables Windows Error Reporting (WER) system-wide via Group Policy. "
                    + "Disabled = 1. No crash reports are collected, stored, or sent to Microsoft. "
                    + "Default: enabled. Recommended for air-gapped or high-privacy deployments. "
                    + "Note: disabling WER also prevents WER-based crash analysis in Event Viewer.",
                ApplyOps = [RegOp.SetDword(WerPol, "Disabled", 1)],
                RemoveOps = [RegOp.SetDword(WerPol, "Disabled", 0)],
                DetectOps = [RegOp.CheckDword(WerPol, "Disabled", 1)],
            },
            new TweakDef
            {
                Id = "werpol-disable-internet-send",
                Label = "WER: Block Sending Error Reports to Microsoft",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["wer", "error reporting", "telemetry", "privacy", "group policy"],
                Description =
                    "Prevents Windows Error Reporting from sending crash data over the internet to Microsoft. "
                    + "DontSendAdditionalData = 1. Crash data is still collected locally but not transmitted. "
                    + "Preferred privacy setting that retains local crash diagnostics while stopping uploads.",
                ApplyOps = [RegOp.SetDword(WerPol, "DontSendAdditionalData", 1)],
                RemoveOps = [RegOp.SetDword(WerPol, "DontSendAdditionalData", 0)],
                DetectOps = [RegOp.CheckDword(WerPol, "DontSendAdditionalData", 1)],
            },
            new TweakDef
            {
                Id = "werpol-disable-crash-dialog",
                Label = "WER: Suppress Crash Report Dialog",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wer", "error reporting", "crash", "ui", "kiosk", "group policy"],
                Description =
                    "Suppresses the 'Windows has stopped working' crash dialog box shown to users. "
                    + "DontShowUI = 1. Errors are still logged but users see no dialog. "
                    + "Recommended for kiosk deployments and unattended servers to avoid hanging on UI prompts.",
                ApplyOps = [RegOp.SetDword(WerPol, "DontShowUI", 1)],
                RemoveOps = [RegOp.SetDword(WerPol, "DontShowUI", 0)],
                DetectOps = [RegOp.CheckDword(WerPol, "DontShowUI", 1)],
            },
            new TweakDef
            {
                Id = "werpol-bypass-throttling",
                Label = "WER: Bypass Error Report Throttling",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wer", "error reporting", "crash", "diagnostics", "group policy"],
                Description =
                    "Disables WER's built-in throttling that limits how many reports can be sent. "
                    + "BypassDataThrottling = 1. Ensures all crash reports are captured in corporate "
                    + "WER server deployments where local rate-limiting would mask incident scope. "
                    + "Default: throttling enabled. Use in conjunction with a corporate WER server.",
                ApplyOps = [RegOp.SetDword(WerPol, "BypassDataThrottling", 1)],
                RemoveOps = [RegOp.SetDword(WerPol, "BypassDataThrottling", 0)],
                DetectOps = [RegOp.CheckDword(WerPol, "BypassDataThrottling", 1)],
            },
            new TweakDef
            {
                Id = "werpol-disable-logging",
                Label = "WER: Disable Error Report Logging to Event Log",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Tags = ["wer", "error reporting", "event log", "privacy", "group policy"],
                Description =
                    "Prevents WER from writing crash report summaries to the Windows Application event log. "
                    + "LoggingDisabled = 1. Reduces noise in event logs on systems with frequent non-critical "
                    + "application crashes. Default: logging enabled.",
                ApplyOps = [RegOp.SetDword(WerPol, "LoggingDisabled", 1)],
                RemoveOps = [RegOp.SetDword(WerPol, "LoggingDisabled", 0)],
                DetectOps = [RegOp.CheckDword(WerPol, "LoggingDisabled", 1)],
            },
            new TweakDef
            {
                Id = "werpol-auto-approve-reports",
                Label = "WER: Auto-Approve All Error Report Submissions",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["wer", "error reporting", "consent", "group policy", "enterprise"],
                Description =
                    "Configures WER consent to automatically send all error reports without prompting users. "
                    + "DefaultConsent = 4 (send all data). Used in enterprise environments where crash data "
                    + "is routed to an internal WER server. Default: prompt user (1). "
                    + "Levels: 1=prompt, 2=basic params, 3=params+heap, 4=all data.",
                ApplyOps = [RegOp.SetDword(WerConsent, "DefaultConsent", 4)],
                RemoveOps = [RegOp.SetDword(WerConsent, "DefaultConsent", 1)],
                DetectOps = [RegOp.CheckDword(WerConsent, "DefaultConsent", 4)],
            },
            new TweakDef
            {
                Id = "werpol-disable-heap-dumps",
                Label = "WER: Disable Heap Memory Dump Collection",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["wer", "error reporting", "memory dump", "privacy", "security", "group policy"],
                Description =
                    "Prevents WER from collecting heap memory dumps alongside crash reports. "
                    + "LocalDumps\\DumpType = 0 (no dump). Heap dumps can contain sensitive data "
                    + "including passwords, tokens, or PII present in application memory at crash time. "
                    + "Default: dumps enabled. Recommended for privacy-sensitive deployments.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 0)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 2)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 0)],
            },
            new TweakDef
            {
                Id = "werpol-disable-queue-reporting",
                Label = "WER: Disable Queued Error Report Sending",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wer", "error reporting", "queue", "privacy", "group policy"],
                Description =
                    "Disables WER's queuing mechanism that stores crash reports and sends them later "
                    + "when connectivity is available. MaxQueueSizePercentage = 0. "
                    + "Prevents accumulation of potentially sensitive crash data in %LOCALAPPDATA%\\Microsoft\\Windows\\WER\\. "
                    + "Default: up to 10% of available disk quota used for queue.",
                ApplyOps = [RegOp.SetDword(WerPol, "MaxQueueSizePercentage", 0)],
                RemoveOps = [RegOp.DeleteValue(WerPol, "MaxQueueSizePercentage")],
                DetectOps = [RegOp.CheckDword(WerPol, "MaxQueueSizePercentage", 0)],
            },
            new TweakDef
            {
                Id = "werpol-disable-unplanned-shutdown-reports",
                Label = "WER: Suppress Unplanned OS Shutdown Reports",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wer", "error reporting", "shutdown", "privacy", "group policy"],
                Description =
                    "Prevents WER from generating and sending a report after unplanned OS shutdowns "
                    + "(power loss, hard resets). DisableArchive = 1 blocks archiving of these events. "
                    + "Reduces telemetry from power-sensitive environments such as laptops in unreliable "
                    + "power conditions. Default: reports sent on next boot.",
                ApplyOps = [RegOp.SetDword(WerPol, "DisableArchive", 1)],
                RemoveOps = [RegOp.SetDword(WerPol, "DisableArchive", 0)],
                DetectOps = [RegOp.CheckDword(WerPol, "DisableArchive", 1)],
            },
            new TweakDef
            {
                Id = "werpol-purge-report-archive",
                Label = "WER: Set Maximum Archive Store to Zero Days",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wer", "error reporting", "archive", "privacy", "cleanup", "group policy"],
                Description =
                    "Sets the WER archive retention period to 0 days, causing crash reports in "
                    + "%ProgramData%\\Microsoft\\Windows\\WER\\ReportArchive to be purged immediately. "
                    + "MaxArchiveCount = 1. Prevents long-term storage of crash dumps that may "
                    + "contain sensitive application memory. Default: reports kept for 1 year.",
                ApplyOps = [RegOp.SetDword(WerPol, "MaxArchiveCount", 1)],
                RemoveOps = [RegOp.DeleteValue(WerPol, "MaxArchiveCount")],
                DetectOps = [RegOp.CheckDword(WerPol, "MaxArchiveCount", 1)],
            },
        ];

    }

    // ── EtwSessionPolicy ──
    private static class _EtwSessionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ETW";
        private const string EvtSysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventSystem";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "etwses-disable-auto-logger-startup",
                    Label = "Disable ETW Auto-Logger Sessions at Startup",
                    Category = "Security",
                    Description =
                        "Prevents ETW auto-logger trace sessions from starting automatically at system boot, reducing the number of persistent trace sessions that consume memory and logging bandwidth during normal operation.",
                    Tags = ["etw", "auto-logger", "startup", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ETW auto-logger startup sessions disabled; fewer background trace sessions at boot.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoLoggerAtStartup", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoLoggerAtStartup")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoLoggerAtStartup", 1)],
                },
                new TweakDef
                {
                    Id = "etwses-block-user-trace-sessions",
                    Label = "Block Standard Users from Creating ETW Trace Sessions",
                    Category = "Security",
                    Description =
                        "Prevents standard (non-administrator) user accounts from creating new ETW trace sessions via StartTrace API, restricting diagnostic trace collection to administrator-initiated sessions only.",
                    Tags = ["etw", "trace-session", "standard-user", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ETW trace session creation restricted to admins; standard users cannot start new trace sessions.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUserTraceSessions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUserTraceSessions")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUserTraceSessions", 1)],
                },
                new TweakDef
                {
                    Id = "etwses-disable-wpp-tracing",
                    Label = "Disable WPP Software Tracing Buffer Logging",
                    Category = "Security",
                    Description =
                        "Disables Windows Pre-Processing (WPP) software tracing buffer logging, stopping WPP-instrumented drivers and services from maintaining in-memory circular trace buffers that consume non-paged pool memory.",
                    Tags = ["etw", "wpp", "software-tracing", "memory", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WPP trace buffer logging disabled; WPP-instrumented component tracing stopped, freeing pool memory.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWPPTracing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWPPTracing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWPPTracing", 1)],
                },
                new TweakDef
                {
                    Id = "etwses-set-max-trace-sessions-8",
                    Label = "Limit Maximum Concurrent ETW Trace Sessions to 8",
                    Category = "Security",
                    Description =
                        "Sets the maximum number of concurrent ETW trace sessions to 8, reducing resource usage from trace session handle tables and preventing excessive trace session proliferation from misconfigured applications.",
                    Tags = ["etw", "max-sessions", "resource-limit", "tracing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Maximum concurrent ETW trace sessions limited to 8; fewer trace sessions reduces per-session overhead.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxTraceSessionCount", 8)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxTraceSessionCount")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxTraceSessionCount", 8)],
                },
                new TweakDef
                {
                    Id = "etwses-block-third-party-providers",
                    Label = "Block Third-Party ETW Provider Registration",
                    Category = "Security",
                    Description =
                        "Prevents third-party applications from registering new ETW providers in the system namespace, restricting ETW instrumentation to Microsoft-signed components and reducing the attack surface for provider injection.",
                    Tags = ["etw", "provider-registration", "third-party", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Third-party ETW provider registration blocked; only Microsoft-signed ETW providers allowed.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyProviderRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyProviderRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyProviderRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "etwses-disable-diagnostic-sessions",
                    Label = "Disable Automatic Diagnostic ETW Session Startup",
                    Category = "Security",
                    Description =
                        "Disables automatic startup of Windows diagnostic ETW sessions (DiagTrack, WdiContextLog, AppModel) that run at boot to support telemetry and diagnostics, reducing process creation overhead and memory footprint.",
                    Tags = ["etw", "diagnostic-sessions", "telemetry", "startup-performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Automatic diagnostic ETW sessions disabled at startup; DiagTrack/WdiContextLog no longer auto-started.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDiagnosticETWSessions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDiagnosticETWSessions")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDiagnosticETWSessions", 1)],
                },
                new TweakDef
                {
                    Id = "etwses-enable-session-audit",
                    Label = "Enable ETW Trace Session Creation and Deletion Audit",
                    Category = "Security",
                    Description =
                        "Enables audit log entries when ETW trace sessions are created or deleted, providing visibility into which processes are setting up system-level event tracing that could be used for monitoring or exfiltration.",
                    Tags = ["etw", "audit", "trace-session", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ETW trace session creation/deletion events audited; trace session activity is logged for review.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditTraceSessionActivity", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditTraceSessionActivity")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditTraceSessionActivity", 1)],
                },
                new TweakDef
                {
                    Id = "etwses-disable-telemetry-reporting",
                    Label = "Disable ETW Telemetry Reporting to Microsoft",
                    Category = "Security",
                    Description =
                        "Prevents the ETW infrastructure from sending trace session statistics and provider usage telemetry to Microsoft, keeping internal diagnostic trace topology and provider utilisation patterns from cloud disclosure.",
                    Tags = ["etw", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ETW telemetry to Microsoft disabled; trace session statistics not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableETWTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableETWTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableETWTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "etwses-disable-com-event-system",
                    Label = "Disable COM+ Event System ETW Tracing",
                    Category = "Security",
                    Description =
                        "Disables the COM+ Event System event tracing provider, stopping background COM subscription events from being generated and reducing ETW trace volume on systems where COM+ subscriptions are unused.",
                    Tags = ["etw", "com+", "event-system", "tracing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "COM+ Event System ETW provider disabled; COM subscription events no longer traced.",
                    ApplyOps = [RegOp.SetDword(EvtSysKey, "DisableEventSystem", 1)],
                    RemoveOps = [RegOp.DeleteValue(EvtSysKey, "DisableEventSystem")],
                    DetectOps = [RegOp.CheckDword(EvtSysKey, "DisableEventSystem", 1)],
                },
                new TweakDef
                {
                    Id = "etwses-disable-kernel-logger",
                    Label = "Disable ETW NT Kernel Logger Trace Session",
                    Category = "Security",
                    Description =
                        "Disables the ETW NT Kernel Logger trace session that captures system-wide kernel events (process, thread, I/O, network), reducing the background monitoring overhead on production systems not undergoing active diagnostics.",
                    Tags = ["etw", "kernel-logger", "performance", "tracing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ETW NT Kernel Logger disabled; system-wide kernel event tracing stopped. Impacts some diagnostic tools.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableNTKernelLogger", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableNTKernelLogger")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableNTKernelLogger", 1)],
                },
            ];

    }

    // ── EventForwardingPolicy ──
    private static class _EventForwardingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventForwarding";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "evtfwd-enable-subscription-manager",
                    Label = "Enable WEF Subscription Manager",
                    Category = "Security",
                    Description =
                        "Activates the Windows Event Forwarding subscription manager, allowing this source computer to forward events to a configured collector. Required for WEF operation. Default: 0. Recommended: 1 when WEF is deployed.",
                    Tags = ["wef", "event-forwarding", "subscription", "siem", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enables centralized event forwarding to a SIEM or log collector.",
                    ApplyOps = [RegOp.SetDword(Key, "SubscriptionManagerEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SubscriptionManagerEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "SubscriptionManagerEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "evtfwd-require-encryption",
                    Label = "Require Encrypted Event Forwarding Channel",
                    Category = "Security",
                    Description =
                        "Prevents event forwarding over unencrypted channels. All WEF traffic must use HTTPS or Kerberos-authenticated transport. Default: not enforced. Recommended: 1 for any production WEF deployment.",
                    Tags = ["wef", "event-forwarding", "encryption", "https", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "No event data leaves the host in plaintext; WEF over HTTP is rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowUnencryptedForwarding", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowUnencryptedForwarding")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowUnencryptedForwarding", 0)],
                },
                new TweakDef
                {
                    Id = "evtfwd-require-kerberos-auth",
                    Label = "Require Kerberos Authentication for WEF",
                    Category = "Security",
                    Description =
                        "Enforces Kerberos mutual authentication for all Windows Event Forwarding connections. Prevents relaying to an untrusted or spoofed collector endpoint. Default: 0. Recommended: 1 in domain environments.",
                    Tags = ["wef", "event-forwarding", "kerberos", "authentication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only domain-authenticated collectors accepted; prevents event data exfiltration via rogue collectors.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireKerberosAuthentication", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireKerberosAuthentication")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireKerberosAuthentication", 1)],
                },
                new TweakDef
                {
                    Id = "evtfwd-limit-max-forward-rate",
                    Label = "Limit Maximum Event Forwarding Rate",
                    Category = "Security",
                    Description =
                        "Caps the maximum rate at which events are forwarded to the collector at 1000 events per second. Prevents event flooding from overwhelming the collector during high-activity periods. Default: unlimited. Recommended: 1000.",
                    Tags = ["wef", "event-forwarding", "rate-limit", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "High-activity hosts may drop events above the cap; increase limit on noisy source computers.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxForwardingRate", 1000)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxForwardingRate")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxForwardingRate", 1000)],
                },
                new TweakDef
                {
                    Id = "evtfwd-set-retry-interval",
                    Label = "Set WEF Connection Retry Interval",
                    Category = "Security",
                    Description =
                        "Configures the interval (in seconds) between connection retry attempts when the WEF collector is unreachable. Lower values detect recovery faster; higher values reduce network noise. Default: 300. Recommended: 60.",
                    Tags = ["wef", "event-forwarding", "retry", "availability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Events may be delayed up to one retry interval duration if the collector is temporarily unavailable.",
                    ApplyOps = [RegOp.SetDword(Key, "RetryInterval", 60)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RetryInterval")],
                    DetectOps = [RegOp.CheckDword(Key, "RetryInterval", 60)],
                },
                new TweakDef
                {
                    Id = "evtfwd-set-heartbeat-interval",
                    Label = "Set WEF Collector Heartbeat Interval",
                    Category = "Security",
                    Description =
                        "Sets the heartbeat keep-alive interval (seconds) for WEF collector connections. Ensures the subscription stays active and the collector knows the source is alive. Default: not set. Recommended: 3600 (1 hour).",
                    Tags = ["wef", "event-forwarding", "heartbeat", "keepalive", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Inactive WEF subscriptions persist; collector receives periodic health signals from source.",
                    ApplyOps = [RegOp.SetDword(Key, "HeartbeatInterval", 3600)],
                    RemoveOps = [RegOp.DeleteValue(Key, "HeartbeatInterval")],
                    DetectOps = [RegOp.CheckDword(Key, "HeartbeatInterval", 3600)],
                },
                new TweakDef
                {
                    Id = "evtfwd-set-connection-timeout",
                    Label = "Set WEF Connection Timeout",
                    Category = "Security",
                    Description =
                        "Sets the connection timeout (in seconds) for WEF collector connections. After this period without a response, the connection is dropped and retried. Default: 30. Recommended: 60.",
                    Tags = ["wef", "event-forwarding", "timeout", "connection", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Slow collector responses up to 60 seconds are tolerated before reconnection.",
                    ApplyOps = [RegOp.SetDword(Key, "ConnectionTimeout", 60)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConnectionTimeout")],
                    DetectOps = [RegOp.CheckDword(Key, "ConnectionTimeout", 60)],
                },
                new TweakDef
                {
                    Id = "evtfwd-limit-max-queue-size",
                    Label = "Limit WEF Local Event Queue Size",
                    Category = "Security",
                    Description =
                        "Caps the local event queue (held while the collector is unreachable) to 1024 MB. Prevents unbounded disk growth during extended collector outages. Default: unlimited. Recommended: 1024.",
                    Tags = ["wef", "event-forwarding", "queue", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Events beyond the queue limit are dropped; increase limit on systems with strict audit requirements.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxQueueSizeMB", 1024)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxQueueSizeMB")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxQueueSizeMB", 1024)],
                },
                new TweakDef
                {
                    Id = "evtfwd-use-minimize-bandwidth",
                    Label = "Use Bandwidth-Minimising WEF Delivery Mode",
                    Category = "Security",
                    Description =
                        "Switches WEF delivery optimisation to minimise bandwidth consumption (batch mode). Events are grouped and sent less frequently but more efficiently. Default: 0 (normal). Recommended: 1 on constrained WAN links.",
                    Tags = ["wef", "event-forwarding", "bandwidth", "delivery", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Event delivery may be delayed; latency vs bandwidth trade-off. Not suitable for real-time detection.",
                    ApplyOps = [RegOp.SetDword(Key, "DeliveryOptimizationMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DeliveryOptimizationMode")],
                    DetectOps = [RegOp.CheckDword(Key, "DeliveryOptimizationMode", 1)],
                },
                new TweakDef
                {
                    Id = "evtfwd-enable-event-consolidation",
                    Label = "Enable WEF Event Consolidation at Source",
                    Category = "Security",
                    Description =
                        "Enables duplicate event consolidation on the source computer before forwarding. Repeated identical events within the batch window are sent once with a count. Reduces collector load. Default: 0. Recommended: 1.",
                    Tags = ["wef", "event-forwarding", "consolidation", "deduplication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Repeated events are collapsed; collector sees one entry with event count rather than flood of identical events.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableEventConsolidation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableEventConsolidation")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableEventConsolidation", 1)],
                },
            ];

    }

    // ── EventLogChannelPolicy ──
    private static class _EventLogChannelPolicy
    {
        private const string AppKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Application";
        private const string SecurityKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security";
        private const string SystemKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "evtchan-application-log-size-64mb",
                    Label = "Set Application Event Log Maximum Size to 64 MB",
                    Category = "Security",
                    Description =
                        "Sets the Application event log channel maximum file size to 64 MB (65536 KB), providing a larger rolling buffer for application-generated events before older records are overwritten.",
                    Tags = ["event-log", "application-log", "log-size", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Application event log maximum size set to 64 MB; larger event history before oldest overwritten.",
                    ApplyOps = [RegOp.SetDword(AppKey, "MaxSize", 65536)],
                    RemoveOps = [RegOp.DeleteValue(AppKey, "MaxSize")],
                    DetectOps = [RegOp.CheckDword(AppKey, "MaxSize", 65536)],
                },
                new TweakDef
                {
                    Id = "evtchan-security-log-size-256mb",
                    Label = "Set Security Event Log Maximum Size to 256 MB",
                    Category = "Security",
                    Description =
                        "Sets the Security event log channel maximum file size to 256 MB (262144 KB), providing substantial rolling buffer capacity for high-volume security audit events such as logon/logoff and object access.",
                    Tags = ["event-log", "security-log", "log-size", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Security event log maximum size set to 256 MB; extended audit event history before overwrite.",
                    ApplyOps = [RegOp.SetDword(SecurityKey, "MaxSize", 262144)],
                    RemoveOps = [RegOp.DeleteValue(SecurityKey, "MaxSize")],
                    DetectOps = [RegOp.CheckDword(SecurityKey, "MaxSize", 262144)],
                },
                new TweakDef
                {
                    Id = "evtchan-system-log-size-64mb",
                    Label = "Set System Event Log Maximum Size to 64 MB",
                    Category = "Security",
                    Description =
                        "Sets the System event log channel maximum file size to 64 MB (65536 KB), ensuring system-level driver, service, and hardware events are retained longer before overwrite during high-event-rate conditions.",
                    Tags = ["event-log", "system-log", "log-size", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "System event log maximum size set to 64 MB; system events retained longer before overwrite.",
                    ApplyOps = [RegOp.SetDword(SystemKey, "MaxSize", 65536)],
                    RemoveOps = [RegOp.DeleteValue(SystemKey, "MaxSize")],
                    DetectOps = [RegOp.CheckDword(SystemKey, "MaxSize", 65536)],
                },
                new TweakDef
                {
                    Id = "evtchan-security-log-retain-never-overwrite",
                    Label = "Set Security Event Log to Never Overwrite Old Events",
                    Category = "Security",
                    Description =
                        "Configures the Security event log to stop logging new events when the log is full rather than overwriting the oldest events, ensuring regulatory audit trails are never silently discarded.",
                    Tags = ["event-log", "security-log", "overwrite", "audit-trail", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Security log set to never-overwrite; oldest audits preserved when log fills. May halt logging if full.",
                    ApplyOps = [RegOp.SetString(SecurityKey, "Retention", "true")],
                    RemoveOps = [RegOp.DeleteValue(SecurityKey, "Retention")],
                    DetectOps = [RegOp.CheckString(SecurityKey, "Retention", "true")],
                },
                new TweakDef
                {
                    Id = "evtchan-restrict-security-log-guest",
                    Label = "Restrict Guest Account Security Event Log Access",
                    Category = "Security",
                    Description =
                        "Prevents the Guest account from reading the Security event log, ensuring that sensitive audit data (logon events, privilege use) cannot be accessed by unauthenticated or minimally-privileged guest sessions.",
                    Tags = ["event-log", "security-log", "guest", "access-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Security log access blocked for Guest account; anonymous and guest users cannot read audit trail.",
                    ApplyOps = [RegOp.SetDword(SecurityKey, "RestrictGuestAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(SecurityKey, "RestrictGuestAccess")],
                    DetectOps = [RegOp.CheckDword(SecurityKey, "RestrictGuestAccess", 1)],
                },
                new TweakDef
                {
                    Id = "evtchan-restrict-application-log-guest",
                    Label = "Restrict Guest Account Application Event Log Access",
                    Category = "Security",
                    Description =
                        "Prevents the Guest account from reading Application event log entries, protecting potentially sensitive application error messages and stack traces from unauthenticated access.",
                    Tags = ["event-log", "application-log", "guest", "access-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Application log access blocked for Guest account; application errors/stack traces hidden from guests.",
                    ApplyOps = [RegOp.SetDword(AppKey, "RestrictGuestAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(AppKey, "RestrictGuestAccess")],
                    DetectOps = [RegOp.CheckDword(AppKey, "RestrictGuestAccess", 1)],
                },
                new TweakDef
                {
                    Id = "evtchan-restrict-system-log-guest",
                    Label = "Restrict Guest Account System Event Log Access",
                    Category = "Security",
                    Description =
                        "Prevents the Guest account from reading System event log entries, hiding driver failures, service start/stop events, and hardware error messages from unauthenticated guest sessions.",
                    Tags = ["event-log", "system-log", "guest", "access-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "System log access blocked for Guest account; driver and hardware events hidden from guest users.",
                    ApplyOps = [RegOp.SetDword(SystemKey, "RestrictGuestAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(SystemKey, "RestrictGuestAccess")],
                    DetectOps = [RegOp.CheckDword(SystemKey, "RestrictGuestAccess", 1)],
                },
                new TweakDef
                {
                    Id = "evtchan-application-log-overwrite-oldest",
                    Label = "Set Application Event Log to Overwrite Events Older Than 30 Days",
                    Category = "Security",
                    Description =
                        "Configures the Application event log to overwrite events older than 30 days when the log fills up, ensuring at least 30 days of application event history while preventing the log from permanently growing.",
                    Tags = ["event-log", "application-log", "overwrite", "retention", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Application log overwrites events older than 30 days; 30-day minimum retention maintained.",
                    ApplyOps = [RegOp.SetDword(AppKey, "AutoBackupLogFiles", 0)],
                    RemoveOps = [RegOp.DeleteValue(AppKey, "AutoBackupLogFiles")],
                    DetectOps = [RegOp.CheckDword(AppKey, "AutoBackupLogFiles", 0)],
                },
                new TweakDef
                {
                    Id = "evtchan-security-log-auto-backup",
                    Label = "Enable Automatic Security Event Log Backup on Full",
                    Category = "Security",
                    Description =
                        "Enables automatic backup of the Security event log to a .evtx archive file when the log reaches capacity, preserving the full audit history before the log is cleared and begins collecting new events.",
                    Tags = ["event-log", "security-log", "auto-backup", "archive", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Security log auto-backup on full; full .evtx archive saved before log cleared. Longer audit history preserved.",
                    ApplyOps = [RegOp.SetDword(SecurityKey, "AutoBackupLogFiles", 1)],
                    RemoveOps = [RegOp.DeleteValue(SecurityKey, "AutoBackupLogFiles")],
                    DetectOps = [RegOp.CheckDword(SecurityKey, "AutoBackupLogFiles", 1)],
                },
                new TweakDef
                {
                    Id = "evtchan-disable-event-log-registry-edit",
                    Label = "Disable Direct Registry Editing of Event Log Channel Settings",
                    Category = "Security",
                    Description =
                        "Prevents users and scripts from making direct registry edits to event log channel keys (MaxSize, Retention, etc.) outside of Group Policy, ensuring that event log configuration cannot be tampered with by non-admin processes.",
                    Tags = ["event-log", "registry", "tamper-protection", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Event log channel registry values locked down; tamper via direct registry edit blocked for non-admins.",
                    ApplyOps = [RegOp.SetDword(AppKey, "DisableDirectRegistryEdit", 1)],
                    RemoveOps = [RegOp.DeleteValue(AppKey, "DisableDirectRegistryEdit")],
                    DetectOps = [RegOp.CheckDword(AppKey, "DisableDirectRegistryEdit", 1)],
                },
            ];

    }

    // ── EventLogGpoPolicy ──
    private static class _EventLogGpoPolicy
    {
        private const string GpoEvt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog";
        private const string App = GpoEvt + @"\Application";
        private const string Sec = GpoEvt + @"\Security";
        private const string Sys = GpoEvt + @"\System";
        private const string Setup = GpoEvt + @"\Setup";
        private const string Forwarded = GpoEvt + @"\ForwardedEvents";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "evtgpo-application-size-128mb",
                Label = "Set Application Event Log Size to 128 MB (GPO)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets the Application event log maximum size to 128 MB (131072 KB) via GPO. "
                    + "Enforces the size policy even if administrators change the local setting. "
                    + "Uses the GPO path Policies\\Windows\\EventLog\\Application\\MaxSize.",
                Tags = ["event log", "application log", "size", "gpo", "audit"],
                RegistryKeys = [App],
                ApplyOps = [RegOp.SetDword(App, "MaxSize", 131072)],
                RemoveOps = [RegOp.DeleteValue(App, "MaxSize")],
                DetectOps = [RegOp.CheckDword(App, "MaxSize", 131072)],
            },
            new TweakDef
            {
                Id = "evtgpo-security-size-1gb",
                Label = "Set Security Event Log Size to 1 GB (GPO)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets the Security event log maximum size to 1 GB (1048576 KB) via GPO. "
                    + "Recommended for environments with verbose audit policies to avoid overwriting "
                    + "forensic evidence. Policies\\Windows\\EventLog\\Security\\MaxSize.",
                Tags = ["event log", "security log", "audit", "forensics", "gpo"],
                RegistryKeys = [Sec],
                ApplyOps = [RegOp.SetDword(Sec, "MaxSize", 1048576)],
                RemoveOps = [RegOp.DeleteValue(Sec, "MaxSize")],
                DetectOps = [RegOp.CheckDword(Sec, "MaxSize", 1048576)],
            },
            new TweakDef
            {
                Id = "evtgpo-system-size-128mb",
                Label = "Set System Event Log Size to 128 MB (GPO)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets the System event log maximum size to 128 MB (131072 KB) via GPO. "
                    + "Provides adequate retention for driver, service, and system error events "
                    + "on busy servers. Policies\\Windows\\EventLog\\System\\MaxSize.",
                Tags = ["event log", "system log", "size", "gpo"],
                RegistryKeys = [Sys],
                ApplyOps = [RegOp.SetDword(Sys, "MaxSize", 131072)],
                RemoveOps = [RegOp.DeleteValue(Sys, "MaxSize")],
                DetectOps = [RegOp.CheckDword(Sys, "MaxSize", 131072)],
            },
            new TweakDef
            {
                Id = "evtgpo-setup-size-64mb",
                Label = "Set Setup Event Log Size to 64 MB (GPO)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets the Setup event log maximum size to 64 MB (65536 KB) via GPO. "
                    + "Retains Windows feature/update installation history needed for "
                    + "troubleshooting failed updates. Policies\\Windows\\EventLog\\Setup\\MaxSize.",
                Tags = ["event log", "setup log", "windows update", "gpo"],
                RegistryKeys = [Setup],
                ApplyOps = [RegOp.SetDword(Setup, "MaxSize", 65536)],
                RemoveOps = [RegOp.DeleteValue(Setup, "MaxSize")],
                DetectOps = [RegOp.CheckDword(Setup, "MaxSize", 65536)],
            },
            new TweakDef
            {
                Id = "evtgpo-forwarded-size-256mb",
                Label = "Set Forwarded Events Log Size to 256 MB (GPO)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets the ForwardedEvents log maximum size to 256 MB (262144 KB) via GPO. "
                    + "Important for systems acting as WEF (Windows Event Forwarding) subscribers "
                    + "that collect events from many remote machines.",
                Tags = ["event log", "forwarded events", "wef", "gpo", "siem"],
                RegistryKeys = [Forwarded],
                ApplyOps = [RegOp.SetDword(Forwarded, "MaxSize", 262144)],
                RemoveOps = [RegOp.DeleteValue(Forwarded, "MaxSize")],
                DetectOps = [RegOp.CheckDword(Forwarded, "MaxSize", 262144)],
            },
            new TweakDef
            {
                Id = "evtgpo-application-overwrite",
                Label = "Overwrite Application Event Log When Full (GPO)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets Application event log Retention=0 via GPO, configuring the channel "
                    + "to overwrite the oldest events instead of stopping to accept new ones "
                    + "when the log is full. Prevents event logging failures.",
                Tags = ["event log", "application log", "retention", "overwrite", "gpo"],
                RegistryKeys = [App],
                ApplyOps = [RegOp.SetDword(App, "Retention", 0)],
                RemoveOps = [RegOp.DeleteValue(App, "Retention")],
                DetectOps = [RegOp.CheckDword(App, "Retention", 0)],
            },
            new TweakDef
            {
                Id = "evtgpo-security-overwrite",
                Label = "Overwrite Security Event Log When Full (GPO)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets Security event log Retention=0 via GPO. On systems with CrashOnAuditFail "
                    + "disabled, this ensures the security log continues to operate without "
                    + "blocking the system when full.",
                Tags = ["event log", "security log", "retention", "overwrite", "gpo"],
                RegistryKeys = [Sec],
                ApplyOps = [RegOp.SetDword(Sec, "Retention", 0)],
                RemoveOps = [RegOp.DeleteValue(Sec, "Retention")],
                DetectOps = [RegOp.CheckDword(Sec, "Retention", 0)],
            },
            new TweakDef
            {
                Id = "evtgpo-system-overwrite",
                Label = "Overwrite System Event Log When Full (GPO)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets System event log Retention=0 via GPO. Allows the System log to "
                    + "continuously accept new events by overwriting old ones, preventing "
                    + "driver/service events from being dropped.",
                Tags = ["event log", "system log", "retention", "overwrite", "gpo"],
                RegistryKeys = [Sys],
                ApplyOps = [RegOp.SetDword(Sys, "Retention", 0)],
                RemoveOps = [RegOp.DeleteValue(Sys, "Retention")],
                DetectOps = [RegOp.CheckDword(Sys, "Retention", 0)],
            },
            new TweakDef
            {
                Id = "evtgpo-setup-overwrite",
                Label = "Overwrite Setup Event Log When Full (GPO)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets Setup event log Retention=0 via GPO, allowing the Setup log to "
                    + "overwrite old installation/upgrade records when it reaches capacity, "
                    + "keeping cumulative update history available.",
                Tags = ["event log", "setup log", "retention", "overwrite", "gpo"],
                RegistryKeys = [Setup],
                ApplyOps = [RegOp.SetDword(Setup, "Retention", 0)],
                RemoveOps = [RegOp.DeleteValue(Setup, "Retention")],
                DetectOps = [RegOp.CheckDword(Setup, "Retention", 0)],
            },
            new TweakDef
            {
                Id = "evtgpo-forwarded-overwrite",
                Label = "Overwrite Forwarded Events Log When Full (GPO)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets ForwardedEvents log Retention=0 via GPO. Ensures the Windows Event "
                    + "Forwarding collector continues to receive forwarded events even when the "
                    + "subscribed log is at capacity.",
                Tags = ["event log", "forwarded events", "wef", "retention", "overwrite", "gpo"],
                RegistryKeys = [Forwarded],
                ApplyOps = [RegOp.SetDword(Forwarded, "Retention", 0)],
                RemoveOps = [RegOp.DeleteValue(Forwarded, "Retention")],
                DetectOps = [RegOp.CheckDword(Forwarded, "Retention", 0)],
            },
        ];

    }

    // ── EventSubscriptionPolicy ──
    private static class _EventSubscriptionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventCollector";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wecpol-enable-event-collector-service",
                    Label = "Enable Windows Event Collector Service",
                    Category = "Security",
                    Description =
                        "Enables the Windows Event Collector service which accepts WinRM-based event forwarding subscriptions, allowing this machine to act as a centralised log collection point for multiple source machines.",
                    Tags = ["event-collector", "wec", "winrm", "siem", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Windows Event Collector service enabled; this machine accepts WEF subscriptions as a collector.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableEventCollector", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableEventCollector")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableEventCollector", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-require-https-on-collector",
                    Label = "Require HTTPS on Windows Event Collector Subscriptions",
                    Category = "Security",
                    Description =
                        "Forces all incoming event forwarding connections to the Windows Event Collector to use HTTPS, blocking plain HTTP or unencrypted WinRM connections from source machines.",
                    Tags = ["event-collector", "https", "encryption", "wec", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Event Collector requires HTTPS; plain HTTP forwarding connections rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireHTTPS", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireHTTPS")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireHTTPS", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-limit-subscription-concurrency-100",
                    Label = "Limit Event Collector Concurrent Source Connections to 100",
                    Category = "Security",
                    Description =
                        "Sets the maximum number of concurrent source machine connections to the Windows Event Collector to 100, preventing resource exhaustion on the collector from too many simultaneous forwarding sessions.",
                    Tags = ["event-collector", "concurrency", "resource-limit", "wec", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Event Collector limited to 100 concurrent source connections; excess source machines rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxConcurrentForwardingConnections", 100)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxConcurrentForwardingConnections")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxConcurrentForwardingConnections", 100)],
                },
                new TweakDef
                {
                    Id = "wecpol-log-subscription-setup-failures",
                    Label = "Log Event Collector Subscription Setup Failures",
                    Category = "Security",
                    Description =
                        "Enables detailed event log entries when Windows Event Collector subscription setup fails, providing diagnostics for misconfigurations such as authentication failures, network issues, and XPath query errors.",
                    Tags = ["event-collector", "diagnostics", "subscription-failure", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Subscription setup failure events logged; WEC configuration errors are diagnostic and visible.",
                    ApplyOps = [RegOp.SetDword(Key, "LogSubscriptionSetupFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogSubscriptionSetupFailures")],
                    DetectOps = [RegOp.CheckDword(Key, "LogSubscriptionSetupFailures", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-disable-legacy-event-subscription",
                    Label = "Disable Legacy Event Pull Subscription (Source-Initiated Only)",
                    Category = "Security",
                    Description =
                        "Disables collector-initiated (legacy pull) subscriptions, allowing only source-initiated (push) subscriptions where source machines connect to the collector, which works across NAT and firewall boundaries.",
                    Tags = ["event-collector", "pull-subscription", "source-initiated", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Collector-initiated pull subscriptions disabled; only source-initiated push subscriptions allowed.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCollectorInitiatedSubscriptions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCollectorInitiatedSubscriptions")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCollectorInitiatedSubscriptions", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-audit-subscription-activity",
                    Label = "Audit All Event Collector Subscription Activity",
                    Category = "Security",
                    Description =
                        "Enables detailed auditing of all Windows Event Collector subscription activities (created, modified, deleted, connected, disconnected) to the local Security event log for compliance and change tracking.",
                    Tags = ["event-collector", "audit", "subscription-activity", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All WEC subscription activities audited; subscription changes and connection events logged.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditSubscriptionActivity", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditSubscriptionActivity")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditSubscriptionActivity", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-set-heartbeat-interval-3600",
                    Label = "Set Event Collector Heartbeat Interval to 3600 Seconds",
                    Category = "Security",
                    Description =
                        "Sets the Windows Event Collector heartbeat interval to 3600 seconds (one hour), reducing the frequency of heartbeat network traffic between source machines and the collector on stable networks.",
                    Tags = ["event-collector", "heartbeat", "network", "interval", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WEC heartbeat interval set to 1 hour; less heartbeat traffic on stable networks.",
                    ApplyOps = [RegOp.SetDword(Key, "HeartbeatIntervalSeconds", 3600)],
                    RemoveOps = [RegOp.DeleteValue(Key, "HeartbeatIntervalSeconds")],
                    DetectOps = [RegOp.CheckDword(Key, "HeartbeatIntervalSeconds", 3600)],
                },
                new TweakDef
                {
                    Id = "wecpol-restrict-subscription-management-to-admin",
                    Label = "Restrict Event Collector Subscription Management to Administrators",
                    Category = "Security",
                    Description =
                        "Requires administrator privileges to create, modify, or delete Windows Event Collector subscriptions, preventing standard users or service accounts from altering the event collection pipeline.",
                    Tags = ["event-collector", "admin", "subscription-management", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Subscription management restricted to admins; standard users cannot create or modify WEC subscriptions.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForSubscriptions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForSubscriptions")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForSubscriptions", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-set-max-event-buffer-1mb",
                    Label = "Set Event Collector Internal Buffer to 1 MB Per Subscription",
                    Category = "Security",
                    Description =
                        "Sets the internal memory buffer used per Windows Event Collector subscription to 1 MB, providing sufficient queuing capacity for burst event delivery while limiting per-subscription memory consumption.",
                    Tags = ["event-collector", "buffer", "memory", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WEC per-subscription buffer set to 1 MB; moderate burst tolerance without excessive memory use.",
                    ApplyOps = [RegOp.SetDword(Key, "SubscriptionBufferSizeKB", 1024)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SubscriptionBufferSizeKB")],
                    DetectOps = [RegOp.CheckDword(Key, "SubscriptionBufferSizeKB", 1024)],
                },
                new TweakDef
                {
                    Id = "wecpol-disable-collector-telemetry",
                    Label = "Disable Windows Event Collector Telemetry to Microsoft",
                    Category = "Security",
                    Description =
                        "Prevents the Windows Event Collector service from sending diagnostic and telemetry data about subscription health and performance to Microsoft, protecting internal event collection architecture from cloud disclosure.",
                    Tags = ["event-collector", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WEC telemetry to Microsoft disabled; no subscription health stats sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCollectorTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCollectorTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCollectorTelemetry", 1)],
                },
            ];

    }

    // ── EventTracingPolicy ──
    private static class _EventTracingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventTracing";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "evttrc-disable-etw-telemetry",
                Label = "Disable ETW Telemetry Data Collection",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Event Tracing for Windows collects detailed system telemetry including application performance data, error information, and system events. Disabling ETW telemetry data collection reduces the amount of diagnostic data written to trace log files and uploaded externally. ETW trace data collected from enterprise endpoints can contain sensitive operational information not appropriate for external transmission. Large ETW trace files can consume significant disk space and system resources on high-activity endpoints. Disabling unnecessary telemetry ETW sessions reduces system resource usage without impacting essential Windows operations. Security-critical ETW providers should be maintained while discretionary telemetry providers are disabled.",
                Tags = ["event-tracing", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryETW", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryETW")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryETW", 1)],
            },
            new TweakDef
            {
                Id = "evttrc-restrict-etw-provider-registration",
                Label = "Restrict ETW Provider Registration to Admins",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "ETW providers are software components that write event data to trace sessions and can be registered by any running application. Restricting ETW provider registration to administrators prevents standard user applications from registering custom ETW providers. Malicious software can register ETW providers to intercept and monitor events from security-sensitive ETW sessions. An attacker-registered ETW provider can receive events from protected sessions if improperly isolated. Administrative registration requirements ensure that ETW providers are vetted before being allowed to participate in the tracing infrastructure. This restriction reduces the risk of unauthorized event interception through rogue provider registration.",
                Tags = ["event-tracing", "registration", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictProviderRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictProviderRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictProviderRegistration", 1)],
            },
            new TweakDef
            {
                Id = "evttrc-disable-process-trace-access",
                Label = "Disable Process-Wide ETW Trace Access",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "ETW trace access allows processes to consume events written by other processes and system components through trace listeners. Disabling process-wide trace access prevents standard user processes from reading events written by other applications and security components. Malicious processes with trace access can monitor security software activity, credential operations, and authentication events. Reading security-relevant ETW events can reveal information useful for evading detection and bypassing security controls. Trace consumption should be restricted to authorized security monitoring and diagnostic tools with appropriate permissions. This setting reduces information available to malicious processes for detection evasion and security control bypass.",
                Tags = ["event-tracing", "access", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictProcessTraceAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictProcessTraceAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictProcessTraceAccess", 1)],
            },
            new TweakDef
            {
                Id = "evttrc-set-trace-buffer-size",
                Label = "Set Maximum ETW Trace Buffer Size",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "ETW trace sessions use memory buffers to temporarily hold events before writing to disk or consuming by listeners. Setting the maximum trace buffer size limits the memory that individual ETW sessions can consume on endpoints. Unbounded ETW buffer allocation can allow denial-of-service conditions where ETW sessions consume large amounts of system memory. High buffer limits on endpoints with many active trace sessions can significantly impact available memory for operating system and application use. Reasonable buffer limits ensure that ETW tracing provides diagnostics value without causing memory pressure. Buffer size limits should be set based on the number of concurrent trace sessions and available system memory.",
                Tags = ["event-tracing", "buffer", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxTraceBufferSize", 32)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxTraceBufferSize")],
                DetectOps = [RegOp.CheckDword(Key, "MaxTraceBufferSize", 32)],
            },
            new TweakDef
            {
                Id = "evttrc-disable-live-etw-consumption",
                Label = "Disable Unauthorized Live ETW Event Consumption",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Live ETW event consumption allows processes to read events in real time from active trace sessions as they are generated. Disabling unauthorized live consumption prevents non-privileged processes from subscribing to and receiving live ETW event streams. Live event streams from security-relevant ETW providers can reveal real-time authentication activity and security control states. Attackers with live ETW access can monitor the effect of their actions in real time to evade detection and optimize attack timing. Restricting live consumption to authorized monitoring processes reduces information disclosure risk from the ETW subsystem. Administrative and security monitoring tools should connect to ETW sessions through controlled interfaces rather than open consumption.",
                Tags = ["event-tracing", "consumption", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictLiveConsumption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictLiveConsumption")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictLiveConsumption", 1)],
            },
            new TweakDef
            {
                Id = "evttrc-enable-etw-audit-policy",
                Label = "Enable ETW Security Audit Logging",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "ETW security audit logging records significant ETW infrastructure events including session creation, provider registration, and access attempts. Enabling ETW audit logging provides visibility into ETW usage patterns that can indicate surveillance or data exfiltration through tracing. Monitoring ETW infrastructure events supports detection of malicious use of the tracing subsystem for reconnaissance. Security operations centers can correlate ETW audit events with other security indicators to identify suspicious monitoring activity. ETW audit events are written to the Windows Security event log and can be forwarded to SIEM infrastructure. Audit logging has minimal performance overhead and provides valuable data for both security monitoring and forensic investigation.",
                Tags = ["event-tracing", "audit", "logging", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditETWSecurity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditETWSecurity")],
                DetectOps = [RegOp.CheckDword(Key, "AuditETWSecurity", 1)],
            },
            new TweakDef
            {
                Id = "evttrc-disable-circular-buffer-overwrite",
                Label = "Disable ETW Circular Buffer Overwrite",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "ETW circular buffer mode overwrites the oldest events when the buffer fills up rather than halting event collection. Disabling circular buffer overwrite prevents critical security events from being silently lost when the buffer reaches capacity. In circular buffer mode an attacker who generates high volumes of noise events can cause important security events to be overwritten. Security investigation depends on having complete event records and overwritten events cannot be recovered for forensic analysis. Enterprise security event logging should use sequential or expanding buffers that retain all events rather than overwriting old ones. Log management infrastructure should be sized appropriately to handle event volumes without resorting to circular overwrite.",
                Tags = ["event-tracing", "buffer", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCircularBufferOverwrite", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCircularBufferOverwrite")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCircularBufferOverwrite", 1)],
            },
            new TweakDef
            {
                Id = "evttrc-restrict-etw-logfile-access",
                Label = "Restrict ETW Log File Access Permissions",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "ETW log files written to disk can contain sensitive operational data about system and application activity during the trace session. Restricting ETW log file access permissions ensures that only authorized users and processes can read trace log files. Standard users with access to ETW log files can extract operational data about system activities including cryptographic operations and credential access. Log file access restrictions complement ETW session access controls to protect sensitive trace data at rest. ETW log files should be protected with the same access controls applied to other sensitive operational data. Access auditing should be enabled on ETW log directories to detect unauthorized read attempts.",
                Tags = ["event-tracing", "files", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictLogFileAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictLogFileAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictLogFileAccess", 1)],
            },
            new TweakDef
            {
                Id = "evttrc-disable-process-trace-auto-logger",
                Label = "Disable Unauthorized AutoLogger Sessions",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "AutoLogger sessions start automatically at system boot before any user authentication and collect events from the earliest stages of system startup. Disabling unauthorized AutoLogger sessions prevents malicious or unnecessary persistent trace sessions from running throughout system operation. AutoLogger sessions are created through registry keys and a malicious AutoLogger can monitor security-sensitive startup events. Persistent trace sessions consume memory and processing resources throughout the system lifetime even when not needed. Unauthorized AutoLogger sessions can be used for persistence by malicious software that registers a trace session during infection. Managing AutoLogger registrations ensures that only approved diagnostic sessions run during system startup.",
                Tags = ["event-tracing", "autologger", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictAutoLoggerCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictAutoLoggerCreation")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictAutoLoggerCreation", 1)],
            },
            new TweakDef
            {
                Id = "evttrc-set-event-log-file-size",
                Label = "Set Event Log Maximum File Size",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Event log file size limits determine the maximum amount of event data that is retained before the oldest events are overwritten or the log becomes full. Setting appropriate file size limits ensures that sufficient event history is retained for security investigations. Small event log file sizes cause frequent overwriting that can prevent investigation of incidents that occurred in the past. Event log size recommendations from NIST and CIS benchmarks specify minimum file sizes for different log types. The Security event log should be large enough to retain at minimum 7 days of events for common incident investigation timeframes. Log file size settings should be coordinated with centralized log forwarding to ensure events are captured before local overwrite.",
                Tags = ["event-tracing", "log-size", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxEventLogFileSize", 65536)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxEventLogFileSize")],
                DetectOps = [RegOp.CheckDword(Key, "MaxEventLogFileSize", 65536)],
            },
        ];

    }

    // ── LogonEventsAuditPolicy ──
    private static class _LogonEventsAuditPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Logon-Logoff";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "logonaudit-audit-logon-success-failure",
                Label = "Logon Audit: Enable Success+Failure Auditing for All Interactive and Network Logon Events",
                Category = "Security",
                Description = "Sets AuditLogon=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events 4624 (successful logon) with logon type, source IP, and authentication protocol, and 4625 (failed logon) with error code, source IP, and account name for every interactive (Type 2), network (Type 3), service (Type 5), batch (Type 4), and remote desktop (Type 10) logon and logon failure. " +
                    "Event 4624 and 4625 are the most fundamental SOC monitoring events — all lateral movement paths (SMB, RDP, WinRM, PsExec, WMI) generate logon events on the destination endpoint. Without logon auditing, there is no on-endpoint record of who authenticated, from where, and using what mechanism. The combination of 4624 (successful network logon) from an unexpected IP with 4648 (explicit credential use) from the same timeframe is a high-fidelity indicator for pass-the-hash lateral movement.",
                Tags = ["logon-audit", "event-4624", "event-4625", "lateral-movement", "rdp", "smb"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "All logon success and failure events generated; lateral movement via SMB/RDP/WMI leaves on-endpoint Event 4624 traces.",
                ApplyOps = [RegOp.SetDword(Key, "AuditLogon", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditLogon")],
                DetectOps = [RegOp.CheckDword(Key, "AuditLogon", 3)],
            },
            new TweakDef
            {
                Id = "logonaudit-audit-logoff-events",
                Label = "Logon Audit: Enable Logoff Event Auditing to Calculate Session Duration",
                Category = "Security",
                Description = "Sets AuditLogoff=1 (Success) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4634 (account logoff) when an interactive or network session ends, enabling SIEM correlation to calculate session duration by pairing each 4624 logon event with its 4634 logoff counterpart. Session duration is an important context signal for anomalous access detection. " +
                    "Session duration analysis enables detection of anomalous access patterns. A network logon (4624 Type 3) that lasts 0.3 seconds followed by a logoff (4634) is consistent with automated tool access (PsExec command execution, SMB enumeration). A session from an external IP lasting 4 hours at 2 AM is anomalous for a finance analyst's account. Without logoff events, session duration calculations are impossible and the analyst must infer session end from other activity gaps in the log.",
                Tags = ["logon-audit", "event-4634", "session-duration", "anomaly-detection", "siem"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Logoff events generated (4634); session duration calculable; anomalous session patterns detectable via logon/logoff correlation.",
                ApplyOps = [RegOp.SetDword(Key, "AuditLogoff", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditLogoff")],
                DetectOps = [RegOp.CheckDword(Key, "AuditLogoff", 1)],
            },
            new TweakDef
            {
                Id = "logonaudit-audit-account-lockout-logon",
                Label = "Logon Audit: Enable Account Lockout Event Auditing at Logon (4740 on Destination)",
                Category = "Security",
                Description = "Sets AuditAccountLockout=1 (Success) in Advanced Audit Policy Logon/Logoff category (logon-side complement to the Account Management lockout setting). Generates Security event 4625 subtype failure events on the endpoint where a locked-out account attempts logon in addition to the domain controller-generated 4740. Provides per-endpoint lockout event rather than only DC-centric events. " +
                    "Domain controller-generated lockout events (4740) identify that an account locked out but report only the last DC that processed the lockout, not all the individual endpoints generating failed logon attempts that accumulated to the lockout threshold. Endpoint-generated 4625 Failure / Sub-status 0xC0000234 (account locked out at logon time) events pinpoint exactly which endpoints are producing the lockout-triggering authentication failures, enabling source system identification for spray attack forensics.",
                Tags = ["logon-audit", "account-lockout", "4740", "4625", "spray-attack", "source-identification"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Per-endpoint lockout attempt events generated; spray attack source endpoints identifiable without relying only on DC events.",
                ApplyOps = [RegOp.SetDword(Key, "AuditAccountLockout", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditAccountLockout")],
                DetectOps = [RegOp.CheckDword(Key, "AuditAccountLockout", 1)],
            },
            new TweakDef
            {
                Id = "logonaudit-audit-network-policy-server",
                Label = "Logon Audit: Enable Network Policy Server Radius/NPS Authentication Auditing",
                Category = "Security",
                Description = "Sets AuditNetworkPolicyServer=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events 6272 (NPS granted access), 6273 (NPS denied access), 6274 (NPS discarded request), 6275 (NPS discarded accounting request), 6276 (NPS quarantined client), 6277/6278 (NPS granted probation/revoked access) for RADIUS network access control decisions made by the local NPS role. " +
                    "Network Policy Server (NPS/RADIUS) is the authentication gateway for 802.1X network access control (wired and wireless NAC), VPN authentication, and DirectAccess. NPS audit events record every network access authentication decision — including which machine certificates or user credentials were validated, which NPS policy matched, and whether access was granted or denied. A compromised certificate used to authenticate to the corporate wireless network generates NPS event 6272 with the certificate thumbprint, enabling certificate abuse detection.",
                Tags = ["logon-audit", "nps", "radius", "802.1x", "vpn", "nac"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "NPS/RADIUS authentication decisions audited; network access control events provide NAC bypass and certificate abuse detection.",
                ApplyOps = [RegOp.SetDword(Key, "AuditNetworkPolicyServer", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditNetworkPolicyServer")],
                DetectOps = [RegOp.CheckDword(Key, "AuditNetworkPolicyServer", 3)],
            },
            new TweakDef
            {
                Id = "logonaudit-audit-other-logon-logoff-events",
                Label = "Logon Audit: Enable 'Other Logon/Logoff Events' for Session Reconnection Tracking",
                Category = "Security",
                Description = "Sets AuditOtherLogonLogoffEvents=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events 4649 (replay attack detected), 4778 (session reconnected to Window Station), 4779 (session disconnected from Window Station), 4800 (workstation locked), 4801 (workstation unlocked), 4802/4803 (screensaver invoked/dismissed), 5378 (credential delegation requested), 5632/5633 (wireless/wired 802.1X authentication). " +
                    "Events 4778/4779 (RDP/Terminal Services session reconnect and disconnect) are critical for RDP lateral movement forensics. Each reconnect event records the source IP, session ID, and account name separately from the initial logon event. Without other logon/logoff events, an attacker who uses RDP shadowing or session hijacking (connecting to an existing session without creating a new logon event) may not generate additional 4624 events. The 4778 reconnect event captures this post-logon session reuse.",
                Tags = ["logon-audit", "other-logon", "4778", "4779", "rdp-session", "session-hijacking"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "RDP session reconnect/disconnect events (4778/4779) audited; RDP session hijacking and shadowing generate detectable events.",
                ApplyOps = [RegOp.SetDword(Key, "AuditOtherLogonLogoffEvents", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditOtherLogonLogoffEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditOtherLogonLogoffEvents", 3)],
            },
            new TweakDef
            {
                Id = "logonaudit-audit-explicit-credential-use",
                Label = "Logon Audit: Enable Explicit Credential Use Auditing (RunAs, Over-Pass-the-Hash, WinRM)",
                Category = "Security",
                Description = "Sets AuditExplicitCredentialUse=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4648 (logon using explicit credentials) when a process uses a different set of credentials to create a new logon session — covering RunAs executions, WMI remote command execution using explicit credentials, WinRM with credential parameters, and Over-Pass-the-Hash (explicit logon using an injected NTLM hash). " +
                    "Event 4648 is a direct detection signal for Over-Pass-the-Hash and Overpass-the-Hash attacks. When Mimikatz performs an OverPTH (inject NTLM hash into a new logon session using explicit credential logon), Windows generates a 4648 event on the source machine. The combination of 4648 from Machine-A with 4624 Type 3 from Machine-B to Machine-A within the same second is a high-fidelity indicator of pass-the-hash lateral movement initiation from Machine-A.",
                Tags = ["logon-audit", "explicit-credentials", "event-4648", "overpass-the-hash", "winrm", "runas"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Explicit credential use events (4648) generated; Over-Pass-the-Hash and RunAs credential abuse directly detectable.",
                ApplyOps = [RegOp.SetDword(Key, "AuditExplicitCredentialUse", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditExplicitCredentialUse")],
                DetectOps = [RegOp.CheckDword(Key, "AuditExplicitCredentialUse", 3)],
            },
            new TweakDef
            {
                Id = "logonaudit-audit-special-logon-sensitive-groups",
                Label = "Logon Audit: Enable Special Logon Auditing for Privileged Group Member Authentication",
                Category = "Security",
                Description = "Sets AuditSpecialLogon=1 (Success) in Advanced Audit Policy Logon/Logoff category (logon-side complement to the Account Management special logon setting). Generates Security event 4964 whenever a user whose account is a member of the Special Groups list (typically Domain Admins, Enterprise Admins) authenticates interactively or via the network, providing privileged account authentication monitoring without the noise of universal 4624 auditing. " +
                    "Privileged account authentication monitoring serves as a low-effort approximation of Privileged Access Workstation (PAW) compliance enforcement. If Domain Admins should only authenticate from designated admin workstations, Event 4964 events where the source computer name is not in the approved PAW list indicate a policy violation — an admin authenticated from a regular user workstation. This SIEM rule requires only two data sources: the 4964 event and the approved PAW machine list.",
                Tags = ["logon-audit", "event-4964", "domain-admins", "paw", "privileged-access", "compliance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Privileged group member logons generate Event 4964; admin authentication from non-PAW workstations detectable by SIEM.",
                ApplyOps = [RegOp.SetDword(Key, "AuditSpecialLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSpecialLogon")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSpecialLogon", 1)],
            },
            new TweakDef
            {
                Id = "logonaudit-audit-group-membership-at-logon",
                Label = "Logon Audit: Enable Group Membership Enumeration at Logon for Privilege Visibility",
                Category = "Security",
                Description = "Sets AuditGroupMembership=1 (Success) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4627 which lists the full group membership of the logon token at logon time, complementing Event 4624 with the list of all security groups the logging-on user is a member of at the moment of logon. Enables detection of SID injection and Kerberos golden ticket attacks using extra group SIDs. " +
                    "Kerberos golden tickets can be crafted with extra group SIDs added to the PAC (Privileged Account Certificate) that were not in the account's actual group membership. When such a ticket is used for authentication, Windows generates a 4627 event showing the effective group membership of the logon token. By comparing 4627 group membership against the account's actual AD group membership, anomalous extra SIDs (e.g., Domain Admins SID for a non-admin account) are immediately visible as golden ticket indicators.",
                Tags = ["logon-audit", "group-membership", "event-4627", "golden-ticket", "pac", "kerberos"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Logon-time group membership logged (4627); Kerberos golden ticket with extra group SIDs detectable via 4627/AD membership comparison.",
                ApplyOps = [RegOp.SetDword(Key, "AuditGroupMembership", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditGroupMembership")],
                DetectOps = [RegOp.CheckDword(Key, "AuditGroupMembership", 1)],
            },
            new TweakDef
            {
                Id = "logonaudit-audit-ipsec-extended-mode",
                Label = "Logon Audit: Enable IPSec Extended Mode Auditing for Network Authentication Failures",
                Category = "Security",
                Description = "Sets AuditIPSecExtendedMode=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events for IPSec IKEv2 extended mode negotiation (4978/4979/4980/4983/4984), recording Kerberos, certificate, or preshared-key authentication exchanges, useful in environments using IPSec machine authentication for network segmentation enforcement via Windows Firewall with Advanced Security rules. " +
                    "IPSec extended mode authentication provides machine-level authentication for encrypted connections between Windows endpoints in isolated network segments. Failure events from IPSec extended mode indicate endpoints attempting cross-segment communication that is blocked by IPSec policy — a potential indicator of lateral movement attempts that a compromised endpoint's attacker is trying to reach an isolated server segment. Extended mode failures highlight network segmentation policy violations in real time.",
                Tags = ["logon-audit", "ipsec", "ike", "extended-mode", "network-segmentation", "firewall"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "IPSec extended mode authentication events audited; cross-segment communication failures generate events indicating lateral movement.",
                ApplyOps = [RegOp.SetDword(Key, "AuditIPSecExtendedMode", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditIPSecExtendedMode")],
                DetectOps = [RegOp.CheckDword(Key, "AuditIPSecExtendedMode", 3)],
            },
            new TweakDef
            {
                Id = "logonaudit-audit-user-device-claims",
                Label = "Logon Audit: Enable User and Device Claims Auditing for Dynamic Access Control",
                Category = "Security",
                Description = "Sets AuditUserDeviceClaims=1 (Success) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4626 at logon time, which records the user and device claims embedded in the Kerberos authentication token when Dynamic Access Control (DAC) is used — providing visibility into the claims used for conditional access decisions in DAC-protected file server and classification label systems. " +
                    "Dynamic Access Control uses Kerberos claims (user department, device compliance state, classification clearance level) to make file access decisions on Windows Server file shares. A user whose Kerberos token contains an incorrect department claim (e.g., claim was modified at token issue time by a Kerberos token forgery attack) could gain access to files classified for a different department. Event 4626 records the actual claims present at logon time, enabling post-incident review of whether inappropriate access was gated on correct claim values.",
                Tags = ["logon-audit", "claims", "dynamic-access-control", "event-4626", "kerberos", "dac"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "User/device Kerberos claims logged at logon (4626); Dynamic Access Control claim-based access decisions auditable.",
                ApplyOps = [RegOp.SetDword(Key, "AuditUserDeviceClaims", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditUserDeviceClaims")],
                DetectOps = [RegOp.CheckDword(Key, "AuditUserDeviceClaims", 1)],
            },
        ];

    }

    // ── ObjectAccessPolicy ──
    private static class _ObjectAccessPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ObjectAccess";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "objacs-enable-file-system-auditing",
                Label = "Enable File System Object Access Auditing",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "File system object access auditing records access to files and directories that have SACL entries configured for auditing. Enabling file system auditing generates security events for file access operations including read, write, create, and delete when the object's SACL requests auditing. File access auditing is essential for detecting unauthorized access to sensitive files and directories in enterprise environments. Security Event Log events 4663 and 4656 record file access with details about the user, process, file path, and access type. File system auditing log data supports DLP investigations, insider threat detection, and forensic analysis after security incidents. Organizations should configure SACLs on sensitive directories and enable this policy to ensure audit events are generated.",
                Tags = ["object-access", "file-system", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableFileSystemAuditing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableFileSystemAuditing")],
                DetectOps = [RegOp.CheckDword(Key, "EnableFileSystemAuditing", 1)],
            },
            new TweakDef
            {
                Id = "objacs-enable-registry-auditing",
                Label = "Enable Registry Object Access Auditing",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Registry object access auditing records access to registry keys that have SACL entries configured requesting audit events. Enabling registry auditing generates security events for registry read and write operations on monitored keys. Registry modification auditing is critical for detecting persistence mechanisms that write to run keys, service configurations, and authentication providers. Security Event Log events 4663 and 4657 record registry access with account, key path, and operation type information. Registry auditing of sensitive keys like HKLM\\SYSTEM\\CurrentControlSet\\Services provides early warning of service-based persistence. Organizations should configure SACLs on high-value registry paths and ensure this policy is enabled for audit event generation.",
                Tags = ["object-access", "registry", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableRegistryAuditing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRegistryAuditing")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRegistryAuditing", 1)],
            },
            new TweakDef
            {
                Id = "objacs-enable-kernel-object-auditing",
                Label = "Enable Kernel Object Access Auditing",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Kernel object auditing records access to kernel objects such as mutexes, semaphores, and event objects that have SACL-based audit entries. Enabling kernel object auditing provides visibility into inter-process synchronization and communication through kernel objects. Malware commonly uses named kernel objects for synchronization and coordination between malicious processes in multi-stage attacks. Security events for kernel object access help identify attacker-created synchronization primitives used for process coordination. Kernel object auditing is lower volume than file system auditing but provides targeted visibility into process behavior. High-value kernel objects like named mutexes known to be used by specific malware families should be configured with SACLs.",
                Tags = ["object-access", "kernel", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableKernelObjectAuditing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableKernelObjectAuditing")],
                DetectOps = [RegOp.CheckDword(Key, "EnableKernelObjectAuditing", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-sam-access",
                Label = "Enable SAM Database Object Access Auditing",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SAM database object access auditing records attempts to access local account credentials stored in the Security Account Manager database. Enabling SAM access auditing generates security events when processes attempt to open the SAM database for credential access. SAM database access is a common credential harvesting technique used by tools like Mimikatz and similar password dumping utilities. Security Event Log event 4661 records SAM object access with the requesting account and process identifier for forensic analysis. SAM access auditing helps detect credential dumping activity even when it occurs through APIs rather than raw disk access. Detecting SAM access events should be correlated with other artifacts like LSASS process access and unusual administrative tool execution.",
                Tags = ["object-access", "sam", "credentials", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditSAMAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSAMAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSAMAccess", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-lsass-access",
                Label = "Enable LSASS Process Access Auditing",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "LSASS process object auditing records attempts by other processes to open handles to the LSASS process with credential-reading access rights. Enabling LSASS access auditing generates security events when processes attempt to read memory from the Local Security Authority Server Service. Credential dumping tools including Mimikatz, Procdump, and comsvcs.dll extraction all require opening LSASS with PROCESS_VM_READ permissions. Security Event Log event 4656 and 10 from Sysmon can detect LSASS credential access attempts from unauthorized processes. LSASS access detection is one of the most important detections for credential-based lateral movement in enterprise environments. Detecting LSASS access should trigger immediate investigation as legitimate software rarely accesses LSASS process memory.",
                Tags = ["object-access", "lsass", "credentials", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditLSASSAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditLSASSAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditLSASSAccess", 1)],
            },
            new TweakDef
            {
                Id = "objacs-enable-detailed-file-share-audit",
                Label = "Enable Detailed File Share Access Auditing",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Detailed file share auditing records individual file-level access within network shares rather than just share connection events. Enabling detailed file share auditing generates security events with specific file paths, access types, and requestor identities for all share file access. Standard file share auditing only records share connections but detailed auditing provides visibility into which specific files are accessed. Detailed file share audit events are more voluminous than connection-level events and may require additional log infrastructure capacity. Security Event Log event 5145 records detailed file share access with object name, access mask, and account information. Detailed file share auditing is valuable for DLP scenarios and post-incident investigation of data access patterns.",
                Tags = ["object-access", "file-share", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableDetailedFileShareAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDetailedFileShareAudit")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDetailedFileShareAudit", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-removable-storage",
                Label = "Enable Removable Storage Access Auditing",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Removable storage object access auditing records all file access and write operations to USB drives, external hard drives, and other removable media. Enabling removable storage auditing generates security events when users read from or write to removable storage devices. Data exfiltration via USB is a persistent insider threat vector and removable storage auditing provides the evidence chain needed for investigation. Security Event Log event 4663 with object type Removable Storage records the file path, access type, and user for each removable storage operation. Removable storage audit events should be correlated with USB device connection events to identify devices connected for purpose of data exfiltration. Removable storage auditing is most valuable in combination with removable storage access restrictions to detect circumvention attempts.",
                Tags = ["object-access", "removable-storage", "usb", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditRemovableStorageAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditRemovableStorageAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditRemovableStorageAccess", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-cert-services",
                Label = "Enable Certification Authority Object Auditing",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Certificate Authority object access auditing records access to CA database objects and certificate management operations. Enabling CA object auditing generates security events for certificate issuance, revocation, template access, and CA configuration changes. Unauthorized certificate issuance from enterprise CAs is a serious threat enabling creation of forged authentication certificates. Security Event Log events 4874, 4875, and related CA events record certificate operations with requestor identity and certificate details. CA object auditing is essential for detecting certificate-based attacks including unauthorized administrator certificate issuance for authentication bypass. CA audit events should be aggregated with other PKI infrastructure events for comprehensive monitoring.",
                Tags = ["object-access", "pki", "certificates", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditCertificationServicesAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditCertificationServicesAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditCertificationServicesAccess", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-object-handle-manipulation",
                Label = "Enable Object Handle Manipulation Auditing",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Object handle manipulation auditing records when handles to auditable objects are created or closed providing a complete access lifecycle view. Enabling handle manipulation auditing generates security events for handle creation and close operations that bracket actual object access. Handle auditing provides context for other object access events by establishing when access windows opened and closed. Security Event Log event 4659 records object deletion after handle closure providing tracking for file deletion operations. Handle lifecycle auditing is used in detailed forensic analysis to reconstruct object access timelines. Handle manipulation events on critical objects like SAM, LSASS, and sensitive files provide complementary evidence for access investigations.",
                Tags = ["object-access", "handles", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditHandleManipulation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditHandleManipulation")],
                DetectOps = [RegOp.CheckDword(Key, "AuditHandleManipulation", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-central-access-policy",
                Label = "Enable Central Access Policy Staging Auditing",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Central Access Policy staging auditing records what central access policy would have done when applied to object access requests before policies are enforced. Enabling staging audit mode generates security events showing how new central access policies would affect access without blocking current users. CAP staging allows administrators to test Dynamic Access Control policies and identify unexpected effects before enforcement. Security Event Log events in staging mode identify which policy expressions matched and what access decisions would result. Staging audit data enables policy refinement to remove overly restrictive rules that would block legitimate access. Central access policy staging is essential for large enterprise DAC deployments where policy errors could affect many users.",
                Tags = ["object-access", "central-access", "dac", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditCentralAccessPolicyStaging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditCentralAccessPolicyStaging")],
                DetectOps = [RegOp.CheckDword(Key, "AuditCentralAccessPolicyStaging", 1)],
            },
        ];

    }

    // ── PrintAuditPolicy ──
    private static class _PrintAuditPolicy
    {
        private const string AudKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\AuditPrint";

        private const string PrtKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "prtaud-enable-print-job-auditing",
                    Label = "Print Audit: Enable Print Job Audit Events",
                    Category = "Security",
                    Description =
                        "Sets AuditPrintJobs=1 in AuditPrint policy. Enables security audit events for every print job processed by the Windows print spooler. When enabled, the Windows Security event log receives Event ID 4624 (document print event) for each job including: user name, computer name, printer name, document name, job ID, number of pages, and bytes printed. This provides a complete record of document print activity — essential for data loss prevention auditing (detecting mass printing of PII), compliance (HIPAA, SOX printed document requirements), and forensic investigation.",
                    Tags = ["print-audit", "security-log", "dlp", "print-jobs", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Every print job generates a security audit event. Security event log volume increases — ensure the event log size is sufficient and logs are forwarded to a SIEM. Document names in the log may contain sensitive information from the job metadata.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditPrintJobs", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditPrintJobs")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditPrintJobs", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-enable-printer-config-auditing",
                    Label = "Print Audit: Enable Printer Configuration Change Auditing",
                    Category = "Security",
                    Description =
                        "Sets AuditPrinterConfiguration=1 in AuditPrint policy. Enables audit events when printer configuration changes are made: printer added, printer deleted, default printer changed, printer properties modified, printer sharing enabled or disabled. Unauthorised printer configuration changes can be used by attackers to redirect print jobs (malicious printer substitution attack) or to create new printer shares for lateral movement. Configuration change auditing creates an immutable log of every printer infrastructure modification for forensic review.",
                    Tags = ["print-audit", "configuration", "printer-add", "security", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Printer configuration changes generate audit events. SIEM rules for suspicious printer configuration changes (printers added/modified by non-admin accounts) detect potential print spooler abuse. Minimal event volume in stable environments.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditPrinterConfiguration", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditPrinterConfiguration")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditPrinterConfiguration", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-enable-driver-install-auditing",
                    Label = "Print Audit: Enable Printer Driver Installation Auditing",
                    Category = "Security",
                    Description =
                        "Sets AuditDriverInstall=1 in AuditPrint policy. Enables audit events for printer driver installation and removal operations. Printer driver installations are a critical security event path — PrintNightmare and related exploits specifically used driver installation as the code execution vector. Auditing every driver install event provides a detection opportunity: SIEM rules can alert on driver installations by non-IT accounts, installations of unexpected driver names, or driver installs that occur at unusual times. Complements the restriction policies that require admin rights for driver installation.",
                    Tags = ["print-audit", "driver-install", "printnightmare", "security", "detection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Printer driver installations and removals generate audit events. Alerts on unexpected driver installs are a high-fidelity PrintNightmare indicator. Negligible event volume in controlled environments.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditDriverInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditDriverInstall")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditDriverInstall", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-enable-print-server-connections",
                    Label = "Print Audit: Enable Audit for Print Server Connection Events",
                    Category = "Security",
                    Description =
                        "Sets AuditServerConnections=1 in AuditPrint policy. Enables audit events when clients connect to and disconnect from the print server's spooler service via RPC. Each connection event records the client machine name, user account, and connection timestamp. Print server connection auditing is particularly valuable for detecting exploitation of print spooler RPC vulnerabilities: an attacker scanning for PrintNightmare-vulnerable servers will generate connection events before any exploit payload is sent. The connection pattern (connection from unusual machines, outside business hours) is detectable.",
                    Tags = ["print-audit", "server-connections", "rpc", "security", "detection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Print server RPC connection and disconnection events are logged. In environments with many print clients, this generates high event volume. Consider applying to high-value print servers only and forwarding to central SIEM for analysis.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditServerConnections", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditServerConnections")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditServerConnections", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-set-print-log-max-7days",
                    Label = "Print Audit: Retain Print Audit Log for 7 Days",
                    Category = "Security",
                    Description =
                        "Sets AuditLogRetentionDays=7 in AuditPrint policy. Sets the minimum retention period for print audit log entries to 7 days. Print audit log retention of at least 7 days satisfies most operational investigation requirements: typical incident detection occurs within 24-48 hours, and 7 days provides sufficient lookback to correlate print events with the full timeline of an incident. Retaining logs beyond 30 days without SIEM export strains local storage on print servers. This policy sets the minimum — logs should be forwarded to a SIEM for long-term retention independently.",
                    Tags = ["print-audit", "log-retention", "compliance", "siem", "investigation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Print audit logs are retained locally for at minimum 7 days. SIEM forwarding is recommended for longer retention. Local disk space consumption is proportional to job volume.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditLogRetentionDays", 7)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditLogRetentionDays")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditLogRetentionDays", 7)],
                },
                new TweakDef
                {
                    Id = "prtaud-disable-direct-printing-bypass",
                    Label = "Print Audit: Disable Direct Printing Bypass (Enforce Spooler Path)",
                    Category = "Security",
                    Description =
                        "Sets DisableDirectPrinting=1 in Printers policy. Prevents applications from sending print jobs directly to printer hardware ports, bypassing the Windows print spooler. Applications that print directly to a port (WriteFile to LPT1:, socket to port 9100, or direct Win32 printer I/O) bypass the entire print audit chain — no job events, no audit log, no DLP scanning. Enforcing the spooler path ensures all print output is intercepted, logged, and subject to print quota policies. Required for complete print audit coverage.",
                    Tags = ["direct-printing", "spooler-bypass", "dlp", "audit", "control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Applications that bypass the spooler with direct port I/O (legacy manufacturing, point-of-sale, label printers) may stop printing. Test with all applications that use non-standard printing methods before deploying. Standard Windows GDI/WDM/XPS printing paths are unaffected.",
                    ApplyOps = [RegOp.SetDword(PrtKey, "DisableDirectPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableDirectPrinting")],
                    DetectOps = [RegOp.CheckDword(PrtKey, "DisableDirectPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-enable-page-count-tracking",
                    Label = "Print Audit: Enable Per-User Print Page Count Tracking",
                    Category = "Security",
                    Description =
                        "Sets EnablePageTracking=1 in AuditPrint policy. Enables per-user print page count tracking in the Windows print spooler. Page count data is accumulated in the print quota subsystem and can be consumed by print accounting software, print management consoles, and quota enforcement systems. Without page tracking, print accountability is based on job counts rather than page volumes — a user printing 500-page documents daily appears identical to one printing 10 single-page emails. Page tracking is prerequisite to enforcing any meaningful print volume policy.",
                    Tags = ["print-audit", "page-tracking", "quota", "accounting", "usage"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Per-user and per-printer page count data is tracked. Negligible overhead. Data is accessible via Print Management console and print accounting APIs. Does not enforce quotas by itself — pair with a print quota enforcement solution.",
                    ApplyOps = [RegOp.SetDword(AudKey, "EnablePageTracking", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "EnablePageTracking")],
                    DetectOps = [RegOp.CheckDword(AudKey, "EnablePageTracking", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-restrict-color-printing",
                    Label = "Print Audit: Restrict Colour Printing to Authorised Users",
                    Category = "Security",
                    Description =
                        "Sets RestrictColorPrinting=1 in AuditPrint policy. Restricts colour printing capability on managed printers to users who are members of an authorised colour printing security group. All other users are limited to monochrome (black and white) output. Colour printing costs are typically 5-10× higher than monochrome per page. Unrestricted colour printing is a significant operational cost driver in large organisations. Restricting colour printing to users with a business need (design, marketing, executive) provides measurable cost reduction without impacting most users.",
                    Tags = ["print-audit", "colour-printing", "cost-control", "restriction", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Colour printing is restricted to authorised users. Unauthorised users print in monochrome regardless of printer capability. Colour authorisation group must be configured in print server properties. Significant toner cost reduction in large deployments.",
                    ApplyOps = [RegOp.SetDword(AudKey, "RestrictColorPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "RestrictColorPrinting")],
                    DetectOps = [RegOp.CheckDword(AudKey, "RestrictColorPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-enable-secure-print-release",
                    Label = "Print Audit: Enable Secure Print Release (Hold-and-Release)",
                    Category = "Security",
                    Description =
                        "Sets EnableSecurePrint=1 in AuditPrint policy. Enables print job hold-and-release (secure print) mode: jobs are queued on the print server but not released to the physical printer until the user authenticates at the printer panel (PIN, smart card, or badge). Documents are not printed and left unattended on the printer tray — a significant physical security and confidentiality control. Sensitive documents printed to shared office printers routinely sit uncollected for minutes to hours. Secure print release eliminates physical information disclosure.",
                    Tags = ["print-audit", "secure-print", "hold-release", "physical-security", "confidentiality"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Print jobs are held until the submitter authenticates at the printer. Requires printer hardware that supports hold-and-release (most enterprise MFPs). Users must approach the printer to release jobs. Uncollected jobs expire after the configured timeout.",
                    ApplyOps = [RegOp.SetDword(AudKey, "EnableSecurePrint", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "EnableSecurePrint")],
                    DetectOps = [RegOp.CheckDword(AudKey, "EnableSecurePrint", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-log-deleted-print-jobs",
                    Label = "Print Audit: Log Deleted and Cancelled Print Jobs",
                    Category = "Security",
                    Description =
                        "Sets AuditDeletedJobs=1 in AuditPrint policy. Enables audit events when print jobs are deleted or cancelled from the print queue. Print job deletion events capture the who (user account that cancelled), what (document name, printer, job ID), and when (timestamp). Deletions by accounts that did not submit the job indicate queue manipulation — an administrator (or attacker with elevated privileges) deleting another user's print job. This is relevant in secure print environments where deleted-before-release events indicate tampering with the print queue.",
                    Tags = ["print-audit", "deleted-jobs", "queue-manipulation", "security", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Cancelled and deleted print jobs generate audit events. SIEM correlation of the submitter vs. the deleting account detects queue manipulation. Negligible event volume in normal environments.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditDeletedJobs", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditDeletedJobs")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditDeletedJobs", 1)],
                },
            ];

    }

    // ── PrivilegeUseAuditPolicy ──
    private static class _PrivilegeUseAuditPolicy
    {
        private const string PrivKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Privilege Use";
        private const string AclKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Object Access";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "privaudit-audit-sensitive-privilege-use",
                Label = "Privilege Audit: Enable Auditing of Sensitive Privilege Use (SeDebug, SeTcb, SeBackup)",
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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
                Category = "Security",
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

    // ── ProcessCreationAuditPolicy ──
    private static class _ProcessCreationAuditPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "pcaudit-enable-cmdline-in-process-creation-events",
                Label = "Process Audit: Enable Full Command Line in Process Creation Security Events",
                Category = "Security",
                Description = "Sets ProcessCreationIncludeCmdLine_Enabled=1 in the Windows System policy. Enables Windows Security event 4688 (Process Creation) to include the full command-line argument string of the spawned process in the event, rather than only the process executable path. This allows SIEM systems to detect living-off-the-land attacks, fileless malware, and suspicious PowerShell invocations by analysing the full arguments of every process created. " +
                    "Process creation event 4688 without command-line inclusion only shows the executable path (e.g., powershell.exe), not the arguments (-EncodedCommand, -ExecutionPolicy Bypass, -WindowStyle Hidden). Without arguments visible, encoded PowerShell commands, Mimikatz execution via living-off-the-land binaries (LOLBins), and command injection attacks are almost entirely opaque in the Security event log. Command-line auditing is the foundational enabling control for advanced threat detection.",
                Tags = ["process-audit", "cmdline", "process-creation", "event-4688", "siem", "lolbins"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Full command lines visible in Event 4688; SIEM can detect encoded/obfuscated PowerShell, LOLBins, and injection attacks.",
                ApplyOps = [RegOp.SetDword(Key, "ProcessCreationIncludeCmdLine_Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ProcessCreationIncludeCmdLine_Enabled")],
                DetectOps = [RegOp.CheckDword(Key, "ProcessCreationIncludeCmdLine_Enabled", 1)],
            },
            new TweakDef
            {
                Id = "pcaudit-enable-wmi-activity-auditing",
                Label = "Process Audit: Enable WMI Activity Audit Log for Process-Level WMI Operations",
                Category = "Security",
                Description = "Sets EnableWMIActivityAudit=1 in the Windows System policy. Enables the Microsoft-Windows-WMI-Activity/Operational event log channel, causing WMI query execution, WMI provider invocations, and WMI subscription modifications to be logged. WMI is a primary lateral movement and persistence technique used by threat actors to execute code remotely without spawning a child process visible in process creation audit logs. " +
                    "WMI-based attacks (used in APT28, Carbanak, and most enterprise-targeted ransomware operators) execute payload code through the WMI provider host (WmiPrvSE.exe) as a child of svchost.exe, bypassing process creation rules that watch for powershell.exe or cmd.exe. WMI activity logging provides a parallel audit trail for WMI-executed commands that cannot be correlated from process creation events alone, enabling detection of WMI-based fileless lateral movement.",
                Tags = ["process-audit", "wmi", "lateral-movement", "wmiprvse", "apt", "fileless"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "WMI operations logged in Activity event channel; WMI-based lateral movement and persistence detectable by EDR/SIEM.",
                ApplyOps = [RegOp.SetDword(Key, "EnableWMIActivityAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableWMIActivityAudit")],
                DetectOps = [RegOp.CheckDword(Key, "EnableWMIActivityAudit", 1)],
            },
            new TweakDef
            {
                Id = "pcaudit-enable-psh-module-logging",
                Label = "Process Audit: Enable PowerShell Module Logging for All Script Block Execution",
                Category = "Security",
                Description = "Sets EnableModuleLogging=1 in the Windows System policy. Enables PowerShell module logging, which records the full content of every PowerShell pipeline execution (all commands, scripts, and functions invoked) to the PowerShell event log (Microsoft-Windows-PowerShell/Operational, Event ID 4103), providing complete visibility into what code PowerShell executes even when scripts are obfuscated. " +
                    "PowerShell is the most commonly abused administrative tool for post-exploitation activities. Module logging captures the deobfuscated execution of AMSI-aware scripts — when a malicious actor uses encoded base64 commands or string manipulation to evade static detection, PowerShell must decode the payload before execution. Module logging captures the post-decode execution pipeline, revealing the actual malicious commands regardless of the obfuscation layering.",
                Tags = ["process-audit", "powershell", "module-logging", "obfuscation", "amsi", "script-block"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "PowerShell module logging active; all PowerShell execution including decoded obfuscated commands visible in event log.",
                ApplyOps = [RegOp.SetDword(Key, "EnableModuleLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableModuleLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableModuleLogging", 1)],
            },
            new TweakDef
            {
                Id = "pcaudit-enable-psh-script-block-logging",
                Label = "Process Audit: Enable PowerShell Script Block Logging with Obfuscation Auto-Logging",
                Category = "Security",
                Description = "Sets EnableScriptBlockLogging=1 in the Windows System policy. Enables PowerShell Script Block logging (Event ID 4104), which captures every script block (function body, scriptblock literal, and processed script pipeline) executed by PowerShell into the event log. When combined with AMSI integration, suspicious script block content is automatically promoted to 'suspicious script block' events (4104 with level Warning) without requiring rule tuning. " +
                    "Script block logging is stronger than module logging because it operates at a lower level (the PowerShell engine's block compilation step) and captures the content of scripts before they are executed, even when the script is loaded from memory or piped from another command. Script block logging is complementary to AMSI — AMSI inspects content before execution for malware signatures; script block logging captures all execution for post-incident investigation.",
                Tags = ["process-audit", "powershell", "script-block-logging", "event-4104", "amsi", "memory-only"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Script block logging active (Event 4104); all PowerShell script content including memory-only scripts captured.",
                ApplyOps = [RegOp.SetDword(Key, "EnableScriptBlockLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableScriptBlockLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableScriptBlockLogging", 1)],
            },
            new TweakDef
            {
                Id = "pcaudit-enable-psh-transcription",
                Label = "Process Audit: Enable PowerShell Transcription to Centralised Audit Share",
                Category = "Security",
                Description = "Sets EnableTranscripting=1 in the Windows System policy. Enables PowerShell transcription, which writes a text transcript of every PowerShell session (all input commands and output) to a log file. When combined with a centralised transcript output directory (network share or DFS path), all PowerShell session activity from all endpoints is written to a central searchable store. " +
                    "PowerShell transcripts capture information that neither script block logging nor module logging captures: the full interactive session flow including the output returned by commands (e.g., the contents of Get-ChildItem output, netstat results captured by commands, or credentials visible in command output). While transcripts are more verbose than event log entries, they provide a continuous narrative of a PowerShell session that is invaluable for incident reconstruction when reconstructing what a threat actor did during a dwell-time period.",
                Tags = ["process-audit", "powershell", "transcription", "session-log", "incident-response"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "PowerShell transcription enabled; all PS session input and output logged to transcript file.",
                ApplyOps = [RegOp.SetDword(Key, "EnableTranscripting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableTranscripting")],
                DetectOps = [RegOp.CheckDword(Key, "EnableTranscripting", 1)],
            },
            new TweakDef
            {
                Id = "pcaudit-enable-protected-event-logging",
                Label = "Process Audit: Enable Protected Event Logging for PowerShell Encrypted Log Entries",
                Category = "Security",
                Description = "Sets EnableProtectedEventLogging=1 in the Windows System policy. Enables Protected Event Logging, which encrypts the content of sensitive PowerShell script block log entries (Event 4104) using a specified asymmetric public key certificate, so that the log content can only be read by the private key holder on the log analysis server, protecting sensitive command content (passwords, tokens) in the event log from local plaintext exposure. " +
                    "Standard PowerShell script block logging writes command content in plaintext to the event log. If an administrative PowerShell script processes credentials, API keys, or sensitive data, those values appear in the local Security event log in cleartext. Any process with read access to the Security event log (including some malware) can harvest these credentials from the log. Protected Event Logging encrypts sensitive entries, allowing detection while protecting the content from local extraction.",
                Tags = ["process-audit", "powershell", "protected-event-logging", "encryption", "credentials", "log-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "PowerShell event log entries encrypted with PKI certificate; sensitive commands protected from local plaintext access.",
                ApplyOps = [RegOp.SetDword(Key, "EnableProtectedEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableProtectedEventLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableProtectedEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "pcaudit-enable-security-audit-process-termination",
                Label = "Process Audit: Enable Security Audit Events for Process Termination (Event 4689)",
                Category = "Security",
                Description = "Sets AuditProcessTermination=1 in the Windows System policy. Enables Security event log event 4689 (A process has exited), which records the process name, PID, user account, and exit code when any process terminates. When correlated with Event 4688 (process creation), this enables calculation of exact process lifetimes, detection of very-short-lived suspicious processes, and analysis of process trees during incident investigation. " +
                    "Process termination audit enables detection of living-off-the-land binary (LOLBin) usage where a legitimately signed binary (e.g., certutil.exe, regsvr32.exe) is spawned, executes a malicious payload, and exits in milliseconds. Without process termination events, the SIEM only has the creation event and no end marker, making it impossible to calculate process lifetime or determine what happened between creation and exit. Short-lifetime processes (sub-second) that accomplish significant work are high-fidelity attack indicators.",
                Tags = ["process-audit", "process-termination", "event-4689", "lolbins", "process-lifetime", "siem"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Process termination Events 4689 generated; process lifetimes calculable; short-lived LOLBin execution detectable.",
                ApplyOps = [RegOp.SetDword(Key, "AuditProcessTermination", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditProcessTermination")],
                DetectOps = [RegOp.CheckDword(Key, "AuditProcessTermination", 1)],
            },
            new TweakDef
            {
                Id = "pcaudit-enable-pnp-activity-audit",
                Label = "Process Audit: Enable Plug-and-Play Device Connection/Disconnection Audit Events",
                Category = "Security",
                Description = "Sets AuditPNPActivity=1 in the Windows System policy. Enables Security event log events 6416/6419/6420/6421/6423/6424 (Plug and Play activity) that record when new hardware devices are connected or disconnected from the system, including USB drives, network adapters, Bluetooth dongles, and other peripherals — recording the device ID, device type, and connecting user account. " +
                    "USB removable storage is a primary exfiltration vector and a common way to deliver malware (BadUSB, autorun malware). Without PnP audit events, there is no Security event log record of which USB devices were connected, to which endpoints, by which user, at what time. PnP audit events provide DLP and insider threat detection capability — a user who copies data to a USB drive that was connected to their endpoint for 3 minutes generates a complete audit trail of the connection without requiring endpoint DLP software.",
                Tags = ["process-audit", "pnp", "usb", "device-connection", "exfiltration", "baduusb"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "USB/PnP device connections generate Security events; device connection history auditable for exfiltration detection.",
                ApplyOps = [RegOp.SetDword(Key, "AuditPNPActivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditPNPActivity")],
                DetectOps = [RegOp.CheckDword(Key, "AuditPNPActivity", 1)],
            },
            new TweakDef
            {
                Id = "pcaudit-enable-network-connection-events-sysmon-style",
                Label = "Process Audit: Enable Network Connection Events in Windows Event Log Without Sysmon",
                Category = "Security",
                Description = "Sets AuditNetworkConnectionEvents=1 in the Windows System policy. Enables network connection audit events in the Security event log, recording each TCP/UDP connection attempt with the originating process ID, source address/port, and destination address/port, providing network process binding visibility without requiring Sysmon or third-party endpoint agents. " +
                    "Network connection logging is standard in Sysmon Event ID 3, but many enterprises cannot deploy Sysmon due to policy or operational constraints. Windows Security event log network connection auditing (when configured) provides a subset of the same visibility natively. Detecting beaconing to C2 infrastructure requires correlation of process creation events with the network connections those processes make. Without network connection events, process creation auditing alone cannot establish which external hosts a suspicious process contacted.",
                Tags = ["process-audit", "network-connections", "c2-detection", "beaconing", "sysmon-alternative"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Network connection events logged natively; C2 beaconing detectable without Sysmon deployment.",
                ApplyOps = [RegOp.SetDword(Key, "AuditNetworkConnectionEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditNetworkConnectionEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditNetworkConnectionEvents", 1)],
            },
            new TweakDef
            {
                Id = "pcaudit-set-min-security-event-log-size-512mb",
                Label = "Process Audit: Set Minimum Security Event Log Size to 512 MB",
                Category = "Security",
                Description = "Sets SecurityEventLogMinSizeMB=512 in the Windows System policy. Enforces a minimum Security event log file size of 512 MB, ensuring that the on-device event log buffer is large enough to sustain at least 30 days of security audit event retention without log rotation truncating investigative evidence before it can be forwarded to a SIEM. " +
                    "The default Windows Security event log size is 20 MB. With process creation auditing, command-line auditing, and PnP auditing all enabled, a busy endpoint can generate several MB of Security events per hour. A 20 MB log buffer retains as little as a few hours of events. On endpoints without a SIEM agent forwarding events in real time, a 20 MB log means that events from an overnight incident may have been overwritten before the morning IT team investigates. A 512 MB buffer provides several weeks of local retention.",
                Tags = ["process-audit", "event-log", "retention", "siem", "forensics"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Security event log minimum size 512 MB; multi-week local retention for environments without real-time SIEM forwarding.",
                ApplyOps = [RegOp.SetDword(Key, "SecurityEventLogMinSizeMB", 512)],
                RemoveOps = [RegOp.DeleteValue(Key, "SecurityEventLogMinSizeMB")],
                DetectOps = [RegOp.CheckDword(Key, "SecurityEventLogMinSizeMB", 512)],
            },
        ];

    }

    // ── SecurityAuditPolicy ──
    private static class _SecurityAuditPolicy
    {
        private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

        private const string LsaPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

        private const string AuditPolicy = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Audit";

        private const string KerberosParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";

        private const string NetLogon = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "audit-enable-verbose-audit-policy",
                Label = "Enable Verbose Security Audit Policy Subcategory",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["audit", "security", "logging", "policy"],
                Description =
                    "Enables subcategory-level audit policy (Win Vista+ feature) to override "
                    + "the coarser category-level audit settings. Required for detailed event "
                    + "log entries in Security Event Log.",
                ApplyOps = [RegOp.SetDword(LsaPolicy, "SCENoApplyLegacyAuditPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(LsaPolicy, "SCENoApplyLegacyAuditPolicy")],
                DetectOps = [RegOp.CheckDword(LsaPolicy, "SCENoApplyLegacyAuditPolicy", 1)],
            },
            new TweakDef
            {
                Id = "audit-require-sign-seal-dc",
                Label = "Require Secure Channel Signing and Sealing",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["audit", "netlogon", "signing", "security", "domain"],
                Description =
                    "Requires that all Netlogon secure channel traffic be signed and sealed. "
                    + "Prevents Netlogon relay attacks (Zerologon CVE-2020-1472). "
                    + "Set to 1 to require both signing and sealing.",
                ApplyOps = [RegOp.SetDword(NetLogon, "RequireSignOrSeal", 1)],
                RemoveOps = [RegOp.DeleteValue(NetLogon, "RequireSignOrSeal")],
                DetectOps = [RegOp.CheckDword(NetLogon, "RequireSignOrSeal", 1)],
            },
            new TweakDef
            {
                Id = "audit-disable-ntlm-v1",
                Label = "Disable NTLMv1 Authentication (Require NTLMv2)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["audit", "ntlm", "authentication", "security", "hardening"],
                Description =
                    "Sets LmCompatibilityLevel to 5 — send NTLMv2 only, refuse LM and NTLMv1. "
                    + "Prevents pass-the-hash attacks using weak NTLMv1 hashes. "
                    + "May break legacy apps/devices that only support NTLMv1.",
                ApplyOps = [RegOp.SetDword(Lsa, "LmCompatibilityLevel", 5)],
                RemoveOps = [RegOp.SetDword(Lsa, "LmCompatibilityLevel", 3)],
                DetectOps = [RegOp.CheckDword(Lsa, "LmCompatibilityLevel", 5)],
            },
            new TweakDef
            {
                Id = "audit-disable-lm-hash-storage",
                Label = "Disable LAN Manager Hash Storage",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["audit", "lm hash", "password", "security", "hardening"],
                Description =
                    "Prevents Windows from storing LAN Manager password hashes in the SAM "
                    + "database. LM hashes are cryptographically weak and can be cracked quickly. "
                    + "Required for PCI DSS and CIS Windows 11 compliance.",
                ApplyOps = [RegOp.SetDword(Lsa, "NoLMHash", 1)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "NoLMHash")],
                DetectOps = [RegOp.CheckDword(Lsa, "NoLMHash", 1)],
            },
            new TweakDef
            {
                Id = "audit-restrict-anonymous-access",
                Label = "Restrict Anonymous SAM/LSA Access",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["audit", "anonymous", "sam", "lsa", "security"],
                Description =
                    "Disables anonymous access to lists of SAM accounts and LSA policy "
                    + "information via null sessions. Prevents unauthenticated enumeration "
                    + "of user accounts over the network.",
                ApplyOps = [RegOp.SetDword(Lsa, "RestrictAnonymousSAM", 1)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "RestrictAnonymousSAM")],
                DetectOps = [RegOp.CheckDword(Lsa, "RestrictAnonymousSAM", 1)],
            },
            new TweakDef
            {
                Id = "audit-restrict-anonymous-full",
                Label = "Fully Restrict Anonymous Network Access",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["audit", "anonymous", "restricted", "network", "security"],
                Description =
                    "Sets RestrictAnonymous to 1, preventing null-session connections from "
                    + "enumerating shares and users. Blocks anonymous pipe/-share access. "
                    + "Value 2 would be full isolation (may break some workgroup scenarios).",
                ApplyOps = [RegOp.SetDword(Lsa, "RestrictAnonymous", 1)],
                RemoveOps = [RegOp.SetDword(Lsa, "RestrictAnonymous", 0)],
                DetectOps = [RegOp.CheckDword(Lsa, "RestrictAnonymous", 1)],
            },
            new TweakDef
            {
                Id = "audit-force-audit-policy-on-logon",
                Label = "Force Audit Policy Update on Every Logon",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["audit", "policy", "logon", "consistency"],
                Description =
                    "Forces Windows to re-apply the audit policy from the Security database "
                    + "at every user logon. Ensures audit settings are always current even "
                    + "if domain GPO has been applied between reboots.",
                ApplyOps = [RegOp.SetDword(Lsa, "ForceGuest", 0)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "ForceGuest")],
                DetectOps = [RegOp.CheckDword(Lsa, "ForceGuest", 0)],
            },
            new TweakDef
            {
                Id = "audit-enable-crash-on-audit-fail",
                Label = "Enable System Halt on Security Audit Failure",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 5,
                SafetyRating = 2,
                Tags = ["audit", "crash", "halt", "security", "compliance"],
                Description =
                    "Causes Windows to halt (BSOD) if the security event log cannot accept "
                    + "new events. Value 2 = halt. Used in high-security environments where "
                    + "audit trail continuity is mandatory (e.g. FISMA-High). "
                    + "WARNING: system will freeze if log is full.",
                ApplyOps = [RegOp.SetDword(Lsa, "CrashOnAuditFail", 2)],
                RemoveOps = [RegOp.SetDword(Lsa, "CrashOnAuditFail", 0)],
                DetectOps = [RegOp.CheckDword(Lsa, "CrashOnAuditFail", 2)],
            },
            new TweakDef
            {
                Id = "audit-disable-anonymous-token-enumeration",
                Label = "Disable Anonymous Account Token Enumeration",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["audit", "anonymous", "token", "enumeration", "security"],
                Description =
                    "Prevents anonymous connections from enumerating account names and groups "
                    + "via the SAM protocol. Reduces attack surface for password spraying and "
                    + "user enumeration over the network.",
                ApplyOps = [RegOp.SetDword(Lsa, "EveryoneIncludesAnonymous", 0)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "EveryoneIncludesAnonymous")],
                DetectOps = [RegOp.CheckDword(Lsa, "EveryoneIncludesAnonymous", 0)],
            },
        ];

    }

    // ── SqlServerAuditPolicy ──
    private static class _SqlServerAuditPolicy
    {
        private const string InstanceKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSSQLServer\MSSQLServer";
        private const string NetLibKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSSQLServer\MSSQLServer\SuperSocketNetLib";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id          = "sqlaup-enable-full-login-audit",
                    Label       = "Enable Full SQL Server Login Audit (Success + Failure)",
                    Category = "Security",
                    Description = "Sets AuditLevel=3 in the MSSQLServer instance key. Controls the level of SQL Server login auditing: 0=none, 1=success only, 2=failure only, 3=both success and failure. Full auditing (level 3) records every authentication attempt to the SQL error log, enabling detection of brute-force attacks and unauthorised access. Required by most security compliance frameworks (CIS SQL Server Benchmark, STIG).",
                    Tags        = ["sql-server", "audit", "login", "compliance", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote  = "Logs every SQL login attempt; increases SQL error log size on high-connection-rate servers.",
                    ApplyOps    = [RegOp.SetDword(InstanceKey, "AuditLevel", 3)],
                    RemoveOps   = [RegOp.DeleteValue(InstanceKey, "AuditLevel")],
                    DetectOps   = [RegOp.CheckDword(InstanceKey, "AuditLevel", 3)],
                },
                new TweakDef
                {
                    Id          = "sqlaup-enforce-windows-auth-only",
                    Label       = "Enforce Windows Authentication Only for SQL Server",
                    Category = "Security",
                    Description = "Sets LoginMode=1 in the MSSQLServer instance key. Restricts SQL Server to Windows Authentication (Integrated Security) mode only, disabling SQL Server login accounts (LoginMode=2 enables mixed mode). Windows Authentication uses Kerberos or NTLM, benefits from Active Directory password policies, is audited by Windows Security event logs, and eliminates the risk of weak SQL-only passwords.",
                    Tags        = ["sql-server", "authentication", "windows-auth", "security", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote  = "Disables SQL login accounts; applications using SQL usernames/passwords must be migrated to Windows Auth first.",
                    ApplyOps    = [RegOp.SetDword(InstanceKey, "LoginMode", 1)],
                    RemoveOps   = [RegOp.DeleteValue(InstanceKey, "LoginMode")],
                    DetectOps   = [RegOp.CheckDword(InstanceKey, "LoginMode", 1)],
                },
                new TweakDef
                {
                    Id          = "sqlaup-disable-named-pipes",
                    Label       = "Disable SQL Server Named Pipe Protocol",
                    Category = "Security",
                    Description = "Sets NpEnabled=0 in the SQL Server SuperSocketNetLib key. Disables the Named Pipes network protocol for SQL Server connections. Named Pipes traverses SMB and can expose the SQL Server service through Windows file-sharing ports (445/TCP). Disabling Named Pipes forces all connections through TCP/IP which can be precisely port-filtered by a firewall.",
                    Tags        = ["sql-server", "network", "named-pipes", "protocol", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote  = "Drops Named Pipes support; local applications using np: connection strings must switch to tcp:.",
                    ApplyOps    = [RegOp.SetDword(NetLibKey, "NpEnabled", 0)],
                    RemoveOps   = [RegOp.DeleteValue(NetLibKey, "NpEnabled")],
                    DetectOps   = [RegOp.CheckDword(NetLibKey, "NpEnabled", 0)],
                },
                new TweakDef
                {
                    Id          = "sqlaup-disable-shared-memory",
                    Label       = "Disable SQL Server Shared Memory Protocol",
                    Category = "Security",
                    Description = "Sets SmEnabled=0 in the SQL Server SuperSocketNetLib key. Disables the Shared Memory protocol that allows local processes to connect to SQL Server via memory-mapped communication. While convenient, Shared Memory connections bypass network-layer access controls entirely. Disabling it forces all connections (even local) through explicit TCP/IP, ensuring firewall rules and port-level controls apply uniformly.",
                    Tags        = ["sql-server", "network", "shared-memory", "protocol", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote  = "Drops Shared Memory; local automated tools and T-SQL jobs using shared memory connections must use TCP instead.",
                    ApplyOps    = [RegOp.SetDword(NetLibKey, "SmEnabled", 0)],
                    RemoveOps   = [RegOp.DeleteValue(NetLibKey, "SmEnabled")],
                    DetectOps   = [RegOp.CheckDword(NetLibKey, "SmEnabled", 0)],
                },
                new TweakDef
                {
                    Id          = "sqlaup-enable-tcp-protocol",
                    Label       = "Ensure SQL Server TCP/IP Protocol Is Enabled",
                    Category = "Security",
                    Description = "Sets TcpEnabled=1 in the SQL Server SuperSocketNetLib key. Guarantees the TCP/IP network protocol is active for SQL Server, which is the only protocol that can be properly firewalled and port-filtered. Combined with disabling Named Pipes and Shared Memory, this ensures all SQL Server traffic traverses TCP so network access controls are consistently applied.",
                    Tags        = ["sql-server", "network", "tcp", "protocol", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote  = "Ensures TCP is enabled; no business impact if TCP was already active (the common default).",
                    ApplyOps    = [RegOp.SetDword(NetLibKey, "TcpEnabled", 1)],
                    RemoveOps   = [RegOp.DeleteValue(NetLibKey, "TcpEnabled")],
                    DetectOps   = [RegOp.CheckDword(NetLibKey, "TcpEnabled", 1)],
                },
                new TweakDef
                {
                    Id          = "sqlaup-hide-sql-instance",
                    Label       = "Hide SQL Server Instance from Network Browsers",
                    Category = "Security",
                    Description = "Sets HideInstance=1 in the MSSQLServer key. Instructs SQL Server Browser to not return the instance name in response to network enumeration requests. When hidden, clients must supply the explicit server name and port; they cannot discover it through SQL Server Browser UDP broadcasts. This reduces the attack surface by preventing automated scanners from locating the SQL instance via port 1434 UDP enumeration.",
                    Tags        = ["sql-server", "browser", "discovery", "network", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote  = "Hides instance from SQL Browser; connection strings must specify host\\instance explicitly.",
                    ApplyOps    = [RegOp.SetDword(InstanceKey, "HideInstance", 1)],
                    RemoveOps   = [RegOp.DeleteValue(InstanceKey, "HideInstance")],
                    DetectOps   = [RegOp.CheckDword(InstanceKey, "HideInstance", 1)],
                },
                new TweakDef
                {
                    Id          = "sqlaup-disable-xp-cmdshell-flag",
                    Label       = "Record xp_cmdshell Disabled State in Registry",
                    Category = "Security",
                    Description = "Sets XPCmdShellEnabled=0 in the MSSQLServer key. This registry flag indicates that the xp_cmdshell extended stored procedure (which executes OS shell commands from T-SQL) must remain disabled. While the authoritative control is sp_configure inside SQL Server, recording the intended state in the registry allows compliance scanning tools that audit registry keys to verify xp_cmdshell is disabled without querying the SQL instance directly.",
                    Tags        = ["sql-server", "xp-cmdshell", "compliance", "policy", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote  = "Registry flag only; xp_cmdshell must also be disabled via sp_configure inside SQL Server for full protection.",
                    ApplyOps    = [RegOp.SetDword(InstanceKey, "XPCmdShellEnabled", 0)],
                    RemoveOps   = [RegOp.DeleteValue(InstanceKey, "XPCmdShellEnabled")],
                    DetectOps   = [RegOp.CheckDword(InstanceKey, "XPCmdShellEnabled", 0)],
                },
                new TweakDef
                {
                    Id          = "sqlaup-enable-error-reporting",
                    Label       = "Enable SQL Server Error Log Verbosity",
                    Category = "Security",
                    Description = "Sets NumErrorLogs=10 in the MSSQLServer key. Controls how many SQL Server error log files are retained in rotation. Increasing from the default (6) to 10 prevents aggressive error log cycling that could make forensic investigation of incidents difficult. Retaining more log cycles ensures a longer audit trail is available when a security incident is discovered days or weeks after it occurred.",
                    Tags        = ["sql-server", "error-log", "audit", "retention", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote  = "Retains 10 rotated error log files instead of 6; negligible additional disk usage.",
                    ApplyOps    = [RegOp.SetDword(InstanceKey, "NumErrorLogs", 10)],
                    RemoveOps   = [RegOp.DeleteValue(InstanceKey, "NumErrorLogs")],
                    DetectOps   = [RegOp.CheckDword(InstanceKey, "NumErrorLogs", 10)],
                },
                new TweakDef
                {
                    Id          = "sqlaup-disable-olap-remote-connect",
                    Label       = "Disable SQL Server OLAP Remote Connections Flag",
                    Category = "Security",
                    Description = "Sets AllowRemoteConnections=0 in the SQL Server SuperSocketNetLib key. Disables incoming remote connections through the OLAP/Analysis Services network library path. When SQL Server Analysis Services is not deployed or when OLAP connectivity should be restricted to the local machine, disabling remote connections through this protocol handler reduces the network-exposed attack surface of the SQL Server installation.",
                    Tags        = ["sql-server", "olap", "remote", "network", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote  = "Blocks OLAP remote connections; Analysis Services remote clients must connect via explicit TCP/IP instead.",
                    ApplyOps    = [RegOp.SetDword(NetLibKey, "AllowRemoteConnections", 0)],
                    RemoveOps   = [RegOp.DeleteValue(NetLibKey, "AllowRemoteConnections")],
                    DetectOps   = [RegOp.CheckDword(NetLibKey, "AllowRemoteConnections", 0)],
                },
                new TweakDef
                {
                    Id          = "sqlaup-enable-sql-server-encryption",
                    Label       = "Enable SQL Server Force Encryption Flag",
                    Category = "Security",
                    Description = "Sets ForceEncryption=1 in the SQL Server SuperSocketNetLib key. Instructs SQL Server to require encrypted connections (TLS/SSL) for all client connections. Without forced encryption, clients may connect without TLS, transmitting queries and data in plaintext across the network. This registry flag mirrors the Force Encryption option in SQL Server Configuration Manager and should be set alongside a valid server certificate.",
                    Tags        = ["sql-server", "encryption", "tls", "network", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote  = "Forces TLS on all connections; client connection strings must trust the SQL Server certificate or connections will fail.",
                    ApplyOps    = [RegOp.SetDword(NetLibKey, "ForceEncryption", 1)],
                    RemoveOps   = [RegOp.DeleteValue(NetLibKey, "ForceEncryption")],
                    DetectOps   = [RegOp.CheckDword(NetLibKey, "ForceEncryption", 1)],
                },
            ];

    }

    // ── WefSubscriptionPolicy ──
    private static class _WefSubscriptionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wefsubpol-enable-event-forwarding",
                    Label = "Enable Windows Event Forwarding Subscription Service",
                    Category = "Security",
                    Description =
                        "Enables the Windows Event Collector service subscription mechanism allowing this machine to forward events to a WEF collector server via WinRM, centralising log collection in a SIEM-compatible pipeline.",
                    Tags = ["wef", "event-forwarding", "winrm", "siem", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Event forwarding enabled; logs forwarded to WEF collector. Requires WinRM and collector configured.",
                    ApplyOps = [RegOp.SetDword(Key, "SubscriptionManagerEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SubscriptionManagerEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "SubscriptionManagerEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-use-https-transport",
                    Label = "Require HTTPS Transport for Event Forwarding",
                    Category = "Security",
                    Description =
                        "Configures Windows Event Forwarding to use HTTPS (encrypted) transport instead of plain HTTP, ensuring that event data in transit between sources and the collector cannot be intercepted.",
                    Tags = ["wef", "https", "encryption", "transport", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WEF transport set to HTTPS; event forwarding encrypted. HTTP forwarding blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "ForceHTTPS", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ForceHTTPS")],
                    DetectOps = [RegOp.CheckDword(Key, "ForceHTTPS", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-require-kerberos-auth",
                    Label = "Require Kerberos Authentication for Event Forwarding",
                    Category = "Security",
                    Description =
                        "Requires Kerberos authentication for Windows Event Forwarding connections, ensuring only domain-joined machines with valid Kerberos tickets can forward events.",
                    Tags = ["wef", "kerberos", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WEF requires Kerberos auth; only domain-joined machines with valid tickets can forward events.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireKerberosAuth", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireKerberosAuth")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireKerberosAuth", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-set-max-batch-50",
                    Label = "Set Event Forwarding Maximum Batch Size to 50",
                    Category = "Security",
                    Description =
                        "Limits the maximum number of events in a single Windows Event Forwarding delivery batch to 50, reducing peak network bandwidth bursts while ensuring timely delivery of security events.",
                    Tags = ["wef", "batch-size", "network", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WEF batch size limited to 50; smaller burst bandwidth, slightly more delivery requests.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxEventBatchSize", 50)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxEventBatchSize")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxEventBatchSize", 50)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-push-delivery-mode",
                    Label = "Set Event Forwarding Delivery Mode to Push",
                    Category = "Security",
                    Description =
                        "Configures Windows Event Forwarding to use push delivery mode where the source machine initiates delivery, enabling more timely forwarding of security events versus poll-based pull mode.",
                    Tags = ["wef", "delivery-mode", "push", "timeliness", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WEF delivery mode set to push; events forwarded immediately rather than awaiting collector poll.",
                    ApplyOps = [RegOp.SetDword(Key, "DeliveryMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DeliveryMode")],
                    DetectOps = [RegOp.CheckDword(Key, "DeliveryMode", 0)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-block-untrusted-collectors",
                    Label = "Block Event Forwarding to Untrusted Collector Servers",
                    Category = "Security",
                    Description =
                        "Prevents event forwarding connections to collector servers whose certificates are not trusted by the local certificate store, blocking forwarding hijacks to rogue collection endpoints.",
                    Tags = ["wef", "trusted-collector", "certificate", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Event forwarding restricted to certificate-trusted collectors; rogue WEF endpoints blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUntrustedCollectors", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUntrustedCollectors")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUntrustedCollectors", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-retain-on-failure",
                    Label = "Retain Local Events When Forwarding Fails",
                    Category = "Security",
                    Description =
                        "Configures the event forwarding pipeline to retain events in the local event log when the collector is unreachable, ensuring no security event loss during network outages.",
                    Tags = ["wef", "retention", "resilience", "no-drop", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Events retained locally on WEF failure; no event loss when collector is unreachable.",
                    ApplyOps = [RegOp.SetDword(Key, "RetainLocalOnForwardingFailure", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RetainLocalOnForwardingFailure")],
                    DetectOps = [RegOp.CheckDword(Key, "RetainLocalOnForwardingFailure", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-log-forwarding-failures",
                    Label = "Log Event Forwarding Connection Failures",
                    Category = "Security",
                    Description =
                        "Enables event log recording of Windows Event Forwarding connection failures, making pipeline outages visible in the local System event log for diagnostics.",
                    Tags = ["wef", "failure-logging", "connectivity", "diagnostics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WEF connection failure events logged locally; forwarding outages are detectable via event log.",
                    ApplyOps = [RegOp.SetDword(Key, "LogForwardingFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogForwardingFailures")],
                    DetectOps = [RegOp.CheckDword(Key, "LogForwardingFailures", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-disable-unauthenticated",
                    Label = "Disable Unauthenticated Windows Event Forwarding",
                    Category = "Security",
                    Description =
                        "Blocks event forwarding sessions that do not supply authentication credentials, preventing anonymous forwarding connections that could be used to exfiltrate logs to external endpoints.",
                    Tags = ["wef", "authentication", "security", "anonymous", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Anonymous WEF forwarding disabled; all forwarding sessions must supply credentials.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUnauthenticatedForwarding", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUnauthenticatedForwarding")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUnauthenticatedForwarding", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-disable-health-telemetry",
                    Label = "Disable Event Forwarding Health Telemetry to Microsoft",
                    Category = "Security",
                    Description =
                        "Prevents the Windows Event Forwarding service from sending health and reliability telemetry about the forwarding pipeline to Microsoft, keeping internal event collection topology out of cloud telemetry.",
                    Tags = ["wef", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WEF health telemetry to Microsoft disabled; forwarding topology not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableForwardingHealthTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableForwardingHealthTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableForwardingHealthTelemetry", 1)],
                },
            ];

    }

}
