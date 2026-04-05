namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from PolicyAudit.cs ──
// RegiLattice.Core — Tweaks/PolicyAudit.cs
// Security auditing, event log management, ETW sessions, event forwarding, process creation, and access audit policies
// Category: "Security Audit Policy"
// Consolidated from 20 modules.

internal static partial class PolicyAudit
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
        private const string Key =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Account Management";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "acmgmtaudit-audit-user-account-management",
                    Label = "Account Mgmt Audit: Enable Success+Failure Auditing for All User Account Changes",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditUserAccountManagement=3 (Success+Failure) in the Advanced Audit Policy Account Management category. Generates Security events 4720 (created), 4722 (enabled), 4723 (pwd change attempt), 4724 (pwd reset), 4725 (disabled), 4726 (deleted), 4738 (changed), 4740 (locked out), 4765/4766 (SID history) for all local and domain user account lifecycle operations. Provides complete user identity lifecycle audit trail. "
                        + "User account management events are the foundational identity audit record. Security events 4720/4726 (account create/delete) are mandatory for SOC monitoring because they record rogue account creation — a common persistence technique. Without user account management auditing enabled, a threat actor can create a new backdoor local administrator account and there is no Security event log record of the account creation. All identity governance and SoD (Separation of Duties) compliance requirements depend on this audit subcategory being active.",
                    Tags = ["account-mgmt-audit", "user-account", "account-creation", "4720", "persistence", "backdoor-account"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Full user account lifecycle audited; rogue account creation generates Event 4720 — foundational SOC monitoring signal.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditUserAccountManagement", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditUserAccountManagement")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditUserAccountManagement", 3)],
                },
                new TweakDef
                {
                    Id = "acmgmtaudit-audit-security-group-management",
                    Label = "Account Mgmt Audit: Enable Auditing for All Security Group Membership Changes",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditSecurityGroupManagement=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events 4727 (global group created), 4728 (member added), 4729 (member removed), 4730 (global group deleted), 4731 (local group created), 4732 (local group member added), 4733 (local group member removed), 4734 (local group deleted), 4735 (local group changed) for all security group membership operations. "
                        + "Group membership changes are the primary privilege escalation audit signal in Active Directory environments. Adding a compromised account to Domain Admins, Backup Operators, or any privileged security group generates Event 4728/4732. SOC SIEM rules that alert on additions to predefined sensitive security groups (Domain Admins, Enterprise Admins, Schema Admins, Protected Users) depend entirely on this audit subcategory being active across all domain controllers and endpoints.",
                    Tags = ["account-mgmt-audit", "security-group", "group-membership", "4728", "privilege-escalation", "domain-admins"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Security group membership changes audited; additions to privileged groups (Domain Admins) generate immediate SIEM detection events.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditSecurityGroupManagement", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditSecurityGroupManagement")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditSecurityGroupManagement", 3)],
                },
                new TweakDef
                {
                    Id = "acmgmtaudit-audit-distribution-group-management",
                    Label = "Account Mgmt Audit: Enable Auditing for Distribution Group Membership Changes",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditDistributionGroupManagement=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events for distribution group lifecycle operations (4744–4758: create, change, delete, member add, member remove for global and universal distribution groups). Provides identity governance visibility for non-security-enabled groups that may have access to sensitive email distribution lists or SharePoint groups. "
                        + "Distribution groups do not have security principals and cannot directly grant file system access, but they control email distribution reach and SharePoint group membership when used as SharePoint audience targeting groups. An attacker who adds a compromised account to a 'Finance-All' distribution group gains full visibility of financial email communications including budgets, deals, and sensitive financial data delivered through that distribution list. Auditing distribution group changes enables detection of email list infiltration.",
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
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditComputerAccountManagement=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events 4741 (computer account created), 4742 (computer account changed), 4743 (computer account deleted) for computer object lifecycle operations in Active Directory. Provides detection for rogue computer account creation used for Kerberos silver ticket persistence. "
                        + "Computer account creation in Active Directory is a high-value attack technique. By default, any domain user can create up to 10 computer objects in any container they have permissions over (ms-DS-MachineAccountQuota). An attacker with a foothold in the domain can create new computer accounts (RBCD, resource-based constrained delegation attacks), configure a service principal name, and use Kerberos delegation to obtain elevated Kerberos tickets. Computer account creation events detect this persistence technique immediately.",
                    Tags = ["account-mgmt-audit", "computer-account", "4741", "rbcd", "kerberos", "silver-ticket"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Computer account creation (Event 4741) audited; RBCD/Kerberos silver ticket attack via rogue computer account detectable.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditComputerAccountManagement", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditComputerAccountManagement")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditComputerAccountManagement", 3)],
                },
                new TweakDef
                {
                    Id = "acmgmtaudit-audit-other-account-management-events",
                    Label = "Account Mgmt Audit: Enable Other Account Management Events (Password Hash Sync, PKI)",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditOtherAccountManagementEvents=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events 4782 (password hash was accessed), 4793 (password policy API called), 4798 (user's local group membership enumerated), 4799 (security-enabled local group members enumerated) — capturing credential database access and reconnaissance activities that fall outside the standard account management event types. "
                        + "Events 4798 and 4799 (local group membership enumeration) are particularly significant — they are generated when a script or tool enumerates the members of the local Administrators group on an endpoint. Ransomware operators and red teams consistently enumerate local admin group membership across all endpoints immediately after initial compromise to identify which machines have Domain Admins logged in or have shared local admin passwords. These events provide direct detection of the reconnaissance phase of a ransomware campaign.",
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
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditApplicationGroupManagement=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events for application group lifecycle operations (4783–4792: create, change, delete, member add/remove for non-Universal, non-Security application groups used by network access protection and application-specific group policies). "
                        + "Application groups include Windows Authorization Manager (AzMan) application groups, which are used by LOB applications to define role-based access control within the application independent of Active Directory security groups. If an attacker gains write access to an AzMan policy store, they can add themselves to application-level admin roles without modifying Active Directory groups. Auditing application group changes detects this application-level privilege escalation vector.",
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
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditAccountLockout=3 (Success+Failure) in the Advanced Audit Policy Logon/Logoff category. Generates Security event 4625 Failure, 4770, and 4771 for failed logon attempts and event 4740 (account locked out) when an account's failed logon threshold is exceeded, providing brute force password spray attack detection across all endpoints and authentication services. "
                        + "Password spray attacks target a single password against an entire user list to avoid triggering per-account lockout thresholds (one attempt per account does not trigger lockout). Account lockout audit enables detection of spray patterns by correlating event 4740 (account locked out) across multiple accounts in a short time window — multiple lockouts in minutes with the same originating IP address is a high-fidelity indicator of a password spray attack. SOC SIEM rules for password spray are entirely dependent on this audit subcategory.",
                    Tags = ["account-mgmt-audit", "account-lockout", "4740", "password-spray", "brute-force", "siem"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Account lockout events (4740) generated; password spray attacks create correlated lockout pattern detectable by SIEM.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditAccountLockout", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditAccountLockout")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditAccountLockout", 3)],
                },
                new TweakDef
                {
                    Id = "acmgmtaudit-audit-user-right-assignment",
                    Label = "Account Mgmt Audit: Enable Auditing for User Right Assignment Changes",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditPolicyChange=3 (Success+Failure) in the Advanced Audit Policy Audit Policy Change category. Generates Security events 4703 (token privilege enabled/disabled), 4704 (user right assigned), 4705 (user right removed) when any user right (SeDebugPrivilege, SeTcbPrivilege, SeImpersonatePrivilege, etc.) is granted to or removed from any security principal. Detects direct user right manipulation. "
                        + "Granting SeDebugPrivilege or SeImpersonatePrivilege directly to a non-administrator security principal is an authoritative persistence technique — it gives the principal the same privilege as a local administrator for a specific action without adding them to the Administrators group. This bypasses monitoring rules that only watch for Administrators group membership changes. Auditing user right assignment changes detects this out-of-band privilege grant pathway.",
                    Tags = ["account-mgmt-audit", "user-rights", "4704", "sedebug", "seimpersonate", "privilege-grant"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "User right assignment changes audited; direct SeDebugPrivilege/SeImpersonatePrivilege grants generate detection events.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditPolicyChange", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditPolicyChange")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditPolicyChange", 3)],
                },
                new TweakDef
                {
                    Id = "acmgmtaudit-audit-credential-validation-failures",
                    Label = "Account Mgmt Audit: Enable Credential Validation Failure Auditing for Auth Attack Detection",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditCredentialValidation=3 (Success+Failure) in the Advanced Audit Policy Account Logon category. Generates Security events 4776 (NTLM authentication attempt — success/failure) and 4772/4776 failure events for failed NTLM credential validation attempts against the local SAM, enabling detection of NTLM hash relay attacks, local brute force, and pass-the-hash authentication re-use attempts against local account hashes. "
                        + "NTLM authentication failure events (4776 Failure) are the primary detection signal for NTLM relay attacks — when an attacker captures an NTLM challenge-response and relays it to a different server, the relay attempt generates authentication failure events with the source workstation name visible. Pass-the-Hash attempts against local accounts (using a harvested NTLM hash to authenticate to SMB) also generate 4776 Failure events from unexpected source machines. These events feed the 'NTLM authentication anomaly' SIEM detection rules.",
                    Tags = ["account-mgmt-audit", "credential-validation", "ntlm", "4776", "pass-the-hash", "relay-attack"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "NTLM authentication failures audited (Event 4776); NTLM relay and pass-the-hash attacks generate detectable event patterns.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditCredentialValidation", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditCredentialValidation")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditCredentialValidation", 3)],
                },
                new TweakDef
                {
                    Id = "acmgmtaudit-enable-kerberos-service-ticket-audit",
                    Label = "Account Mgmt Audit: Enable Kerberos Service Ticket Auditing for Ticket Attack Detection",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditKerberosServiceTicket=3 (Success+Failure) in the Advanced Audit Policy Account Logon category. Generates Security events 4769 (Kerberos service ticket request — success), 4770 (Kerberos service ticket renew), and 4771 (Kerberos pre-authentication failure) for all Kerberos ticket-granting service (TGS) requests, enabling detection of Kerberoasting attacks that request service tickets for all SPNs to offline crack their RC4-encrypted password hashes. "
                        + "Kerberoasting is one of the most common Active Directory attack techniques: any domain user can request a TGS for any service principal name, and if the service account's domain password is RC4-encrypted in the ticket (etype 0x17), the ticket can be taken offline for brute force password cracking without triggering any lockout. Auditing Kerberos TGS requests generates Event 4769 for each SPN ticket request — a Kerberoasting scan (requesting TGS for all SPNs in rapid succession) creates a distinctive volume and timing pattern detectable by SIEM.",
                    Tags = ["account-mgmt-audit", "kerberos", "4769", "kerberoasting", "tgs", "service-ticket"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Kerberos TGS requests audited (Event 4769); Kerberoasting SPN scan generates distinctive volume pattern detectable by SIEM.",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
        private const string Key =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\DS Access";
        private const string DetailKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Detailed Tracking";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "dsaudit-audit-directory-service-access",
                    Label = "DS Audit: Enable Directory Service Object Access Auditing (LDAP Reads to Sensitive AD Objects)",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditDirectoryServiceAccess=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates Security event 4661 for SACL-triggered access to Active Directory objects on domain controllers — user objects, group objects, GPO links, schema attributes, and AdminSDHolder-protected objects — providing on-DC audit records of all access to sensitive AD data. "
                        + "Active Directory is the crown jewel of the enterprise identity infrastructure. Without directory service access auditing, an attacker who performs an LDAP dump of all user objects (including password hint attributes, lastLogon, adminCount, userAccountControl enumeration) leaves no Security event log trace on the domain controller. With SACL-protected sensitive AD objects (all adminCount=1 objects, GPO objects, schema), directory service access events generate on every LDAP read, enabling DCSync detection and AD reconnaissance identification.",
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
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditDirectoryServiceChanges=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates Security events 5136 (attribute modified), 5137 (object created), 5138 (object restored from tombstone), 5139 (object moved), 5141 (object deleted) for all changes to Active Directory objects, providing a granular changelog of AD modifications. "
                        + "Event 5136 is the AD schema-level modification record — it captures every attribute write to every AD object (user, group, computer, GPO, schema). Without this auditing subcategory enabled on domain controllers, the SOC has no event log record of Group Policy Object (GPO) modifications, AdminSDHolder ACL changes, Service Principal Name (SPN) additions (Kerberoasting target creation), or Domain Trust modifications (trust injection). SOC SIEM rules for GPO modification, persistence SPN addition, and trust injection all depend on Event 5136.",
                    Tags = ["ds-audit", "directory-service-changes", "event-5136", "gpo", "spn", "trust-injection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "AD attribute changes logged (Event 5136); GPO modifications, SPN additions, and trust changes generate SOC detection events.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditDirectoryServiceChanges", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditDirectoryServiceChanges")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditDirectoryServiceChanges", 3)],
                },
                new TweakDef
                {
                    Id = "dsaudit-audit-directory-service-replication",
                    Label = "DS Audit: Enable Directory Service Replication Auditing for DCSync Detection",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditDirectoryServiceReplication=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates Security events 4928 (source naming context established — replication initiated), 4929 (source naming context removed), 4930 (source naming context modified), 4931 (destination naming context modified) for AD replication operations. Enables detection of DCSync attacks performed by non-DC machines invoking DS-Replication-Get-Changes privileges. "
                        + "DCSync (Mimikatz's lsadump::dcsync) mimics the behaviour of a domain controller requesting replication from another DC to obtain all account password hashes without requiring local access to the DC. The attack uses DS-Replication-Get-Changes-All privileges. Replication audit events (4928) are generated on the target DC when the replication request arrives. A 4928 event from a client workstation (not a domain controller) is a high-fidelity DCSync detection signal.",
                    Tags = ["ds-audit", "replication", "event-4928", "dcsync", "ds-replication", "mimikatz"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "DS replication events audited; DCSync attack from non-DC machines generates Event 4928 — high-fidelity detection signal.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditDirectoryServiceReplication", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditDirectoryServiceReplication")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditDirectoryServiceReplication", 3)],
                },
                new TweakDef
                {
                    Id = "dsaudit-audit-detailed-replication",
                    Label = "DS Audit: Enable Detailed Directory Service Replication Auditing",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditDetailedDirectoryServiceReplication=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates verbose Security events 4932/4933 (synchronization of a naming context has begun/ended) and 4934/4935/4937 (attribute of AD object replicated/failed/lingering object removed) for each object-level attribute synchronisation step during AD replication, providing attribute-granular replication change records. "
                        + "Detailed replication auditing provides the object-level granularity missing from standard replication auditing. When a naming context replication session (Event 4928) encompasses thousands of object changes, the standard events identify that replication occurred but not which specific objects or attributes were synchronised. Detailed replication events (4932/4934) identify the specific objects replicated in each session, enabling investigation of which specific accounts were targeted in a DCSync attack session.",
                    Tags = ["ds-audit", "detailed-replication", "event-4932", "naming-context", "dcsync-detail"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Detailed replication events generated; specific objects and attributes synchronised during DCSync sessions are identifiable.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditDetailedDirectoryServiceReplication", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditDetailedDirectoryServiceReplication")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditDetailedDirectoryServiceReplication", 3)],
                },
                new TweakDef
                {
                    Id = "dsaudit-enable-dpapi-activity-audit",
                    Label = "DS Audit: Enable DPAPI Activity Auditing for Master Key Access Monitoring",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditDPAPIActivity=3 (Success+Failure) in Advanced Audit Policy Detailed Tracking category. Generates Security events 4692 (DPAPI backup key was requested), 4693 (DPAPI data was decrypted), 4694 (DPAPI data was encrypted), 4695 (DPAPI data was decrypted in unprotected state) for all DPAPI encryption and decryption operations. Enables detection of DPAPI master key harvesting attacks. "
                        + "DPAPI master key backup operations (Event 4692) are generated when a new DPAPI master key is created and its backup is sent to the domain controller for recovery purposes. In DPAPI masterkey harvesting attacks (used by NanoDump, SharpDPAPI), an attacker requests the DPAPI backup key from the domain controller to decrypt all locally cached DPAPI blobs across the enterprise. Event 4692 from an unexpected non-system principal is a binary indicator of DPAPI master key interception.",
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
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditRPCEvents=3 (Success+Failure) in Advanced Audit Policy Detailed Tracking category. Generates Security event 5712 (RPC connection attempt) for remote procedure call connections with caller identity, target interface UUID, and endpoint information — enabling detection of RPC-based lateral movement techniques that use Windows RPC interfaces (MS-SAMR, MS-LSAD, MS-DRSR, MS-RPRN) to access remote system resources. "
                        + "Remote Printer Spooler (MS-RPRN) exploitation (PrintNightmare) and RPC-based DCSync (MS-DRSR interface calls) are primary RPC-based attack techniques. Without RPC event auditing, there is no Security event log record of specific Windows RPC interface calls made to an endpoint. RPC event audit enables detection of PrintNightmare exploitation (unexpected MS-RPRN calls from non-print-server machines) and RPC-based credential access attempts targeting SAMR and LSAD interfaces.",
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
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditCentralAccessPolicyStaging=1 (Success) in Advanced Audit Policy DS Access category. Generates Security event 4818 (proposed Central Access Policy does not grant the same access permissions as the current Central Access Policy) when a proposed Dynamic Access Control policy being tested in staging mode would grant different access than the currently active policy, identifying files that would change access before the policy is deployed. "
                        + "Central Access Policy staging is the Windows DAC mechanism for safely testing new classification policies before deploying them to production. Without staging audit events, IT cannot determine the blast radius of a new DAC policy change — which files would gain new access grants, which would lose existing access. Event 4818 provides a non-destructive preview showing exactly which resources would receive different access treatment under the proposed policy vs the current policy.",
                    Tags = ["ds-audit", "central-access-policy", "dac", "staging", "event-4818", "policy-testing"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "DAC staging audit events (4818) generated; policy impact assessment before deployment identifies access changes without risk.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditCentralAccessPolicyStaging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditCentralAccessPolicyStaging")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditCentralAccessPolicyStaging", 1)],
                },
                new TweakDef
                {
                    Id = "dsaudit-enable-certificate-services-audit",
                    Label = "DS Audit: Enable Active Directory Certificate Services Audit for CA Operation Monitoring",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditCertificationServices=3 (Success+Failure) in Advanced Audit Policy Object Access category. Generates Security events 4870/4871/4872/4873/4874/4875/4876/4877 for all Active Directory Certificate Services operations — certificate requests (approved, denied, pending), certificate revocations, certificate template modifications, and CA role service start/stop. Critical for detecting AD CS-based privilege escalation (ESC1–ESC8 attacks). "
                        + "AD Certificate Services attacks (ESC1–ESC8, as catalogued by SpecterOps) enable low-privilege users to obtain certificates that can be used for domain admin authentication or persistent machine authentication bypass. Without CS audit events, a user who requests and receives a certificate through a misconfigured template (ESC1: SANs allowed by requester) generates no Security alert. Certificate request events (4886: certificate requested, 4887: certificate issued) record the subject, certificate template, and requester — enabling detection of privilege-elevating certificate requests.",
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
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditFilteringPlatformConnection=3 (Success+Failure) in Advanced Audit Policy Object Access category. Generates Security events 5031 (WFP application blocked), 5150/5151 (WFP packet blocked/dropped), 5156/5157 (WFP connection allowed/blocked by application) for Windows Filtering Platform (Windows Firewall) connection decisions, providing process-to-network socket binding records without requiring Sysmon Event ID 3. "
                        + "WFP connection allowed/blocked events (5156/5157) provide the same process-to-network binding information as Sysmon Event 3 but natively through Windows Security event log. Organisations that cannot deploy Sysmon can achieve equivalent network visibility using WFP auditing. Event 5156 records the process making the connection, the destination IP/port, and the protocol — enabling detection of command-and-control beaconing, lateral movement SMB connections, and data exfiltration to external IP ranges.",
                    Tags = ["ds-audit", "wfp", "windows-firewall", "event-5156", "c2-detection", "network-profiling"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "WFP connection events (5156) generated natively; C2 beaconing and lateral movement network connections logged without Sysmon.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditFilteringPlatformConnection", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditFilteringPlatformConnection")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditFilteringPlatformConnection", 3)],
                },
                new TweakDef
                {
                    Id = "dsaudit-audit-handle-manipulation",
                    Label = "DS Audit: Enable Handle Manipulation Auditing for LSASS Memory Access Detection",
                    Category = "Security — Account Management Audit",
                    Description =
                        "Sets AuditHandleManipulation=3 (Success+Failure) in Advanced Audit Policy Object Access category. Generates Security event 4658 (handle to object closed) and event 4690 (attempt to duplicate handle to object) that complement the SACL-based object access events — specifically Event 4690 which records attempts to duplicate an open handle to a sensitive object (such as an LSASS process handle) to a different process. "
                        + "Process handle duplication is an advanced LSASS dump technique used to avoid the more detectable direct process access calls. Tools like x64dump and some variants of Cobalt Strike's in-memory credential extraction duplicate an existing handle to the LSASS process (owned by csrss.exe or another trusted process) rather than opening a new handle from a suspicious process. Event 4690 captures this handle duplication attempt, providing detection for handle-based LSASS access that bypasses protection based solely on process open calls.",
                    Tags = ["ds-audit", "handle-manipulation", "event-4690", "lsass", "handle-duplication", "credential-theft"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Handle duplication (Event 4690) audited; LSASS credential dump via handle duplication technique generates detection events.",
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
                Id = "werpol-disable-crash-dialog",
                Label = "WER: Suppress Crash Report Dialog",
                Category = "Security — Account Management Audit",
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
                Id = "werpol-disable-logging",
                Label = "WER: Disable Error Report Logging to Event Log",
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Account Management Audit",
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
                    Category = "Security — Event Log Channel",
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
                    Category = "Security — Event Log Channel",
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
                    Category = "Security — Event Log Channel",
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
                    Category = "Security — Event Log Channel",
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
                    Category = "Security — Event Log Channel",
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
                    Category = "Security — Event Log Channel",
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
                    Category = "Security — Event Log Channel",
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
                    Category = "Security — Event Log Channel",
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
                    Category = "Security — Event Log Channel",
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
                    Category = "Security — Event Log Channel",
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
                Id = "evtgpo-setup-size-64mb",
                Label = "Set Setup Event Log Size to 64 MB (GPO)",
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
                Id = "evtgpo-system-overwrite",
                Label = "Overwrite System Event Log When Full (GPO)",
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
}
