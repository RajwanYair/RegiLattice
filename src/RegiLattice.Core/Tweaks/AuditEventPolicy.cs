// RegiLattice.Core — Tweaks/AuditEventPolicy.cs
// Sprint 336: Audit Event Policy tweaks (10 tweaks)
// Category: "Audit Event Policy" | Slug: audevt
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Audit
// Audit values: 0=no audit, 1=success, 2=failure, 3=both

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AuditEventPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Audit";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "audevt-audit-logon-events",
            Label = "Enable Audit Policy for Logon Success and Failure Events",
            Category = "Audit Event Policy",
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
            Category = "Audit Event Policy",
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
            Category = "Audit Event Policy",
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
            Category = "Audit Event Policy",
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
            Category = "Audit Event Policy",
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
            Category = "Audit Event Policy",
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
            Category = "Audit Event Policy",
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
            Category = "Audit Event Policy",
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
            Category = "Audit Event Policy",
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
            Category = "Audit Event Policy",
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
