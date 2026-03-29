// RegiLattice.Core — Tweaks/AccountManagementAuditPolicy.cs
// User and group account management audit policy controls for identity governance (Sprint 624).
// Category: "Account Management Audit Policy" | Slug: acmgmtaudit
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies\Account Management

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AccountManagementAuditPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Account Management";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "acmgmtaudit-audit-user-account-management",
            Label = "Account Mgmt Audit: Enable Success+Failure Auditing for All User Account Changes",
            Category = "Account Management Audit Policy",
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
            Category = "Account Management Audit Policy",
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
            Category = "Account Management Audit Policy",
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
            Category = "Account Management Audit Policy",
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
            Category = "Account Management Audit Policy",
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
            Category = "Account Management Audit Policy",
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
            Category = "Account Management Audit Policy",
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
            Category = "Account Management Audit Policy",
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
            Category = "Account Management Audit Policy",
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
            Category = "Account Management Audit Policy",
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
