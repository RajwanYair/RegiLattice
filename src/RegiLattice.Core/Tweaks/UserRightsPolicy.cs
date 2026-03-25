// RegiLattice.Core — Tweaks/UserRightsPolicy.cs
// Sprint 337: User Rights Policy tweaks (10 tweaks)
// Category: "User Rights Policy" | Slug: usrrts
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PrivilegeRights

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class UserRightsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PrivilegeRights";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "usrrts-restrict-debug-privilege",
            Label = "Restrict SeDebugPrivilege to Administrators Only",
            Category = "User Rights Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "SeDebugPrivilege allows a process to read and write to any other process's memory regardless of the target process's security descriptor. Restricting debug privilege to only administrators prevents standard users and service accounts from accessing kernel and other privileged process memory. Credential-stealing malware including Mimikatz requires SeDebugPrivilege to read LSASS memory and extract authentication credentials. Standard user accounts should never have debug privilege as there is no legitimate operational reason for non-administrators to debug system processes. Service accounts used for application workloads should be audited to ensure SeDebugPrivilege has not been inadvertently granted to them. Restricting debug privilege is one of the most effective controls against credential theft from LSASS complementing Credential Guard.",
            Tags = ["privilege", "debug", "credentials", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSeDebugPrivilege", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeDebugPrivilege")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSeDebugPrivilege", 1)],
        },
        new TweakDef
        {
            Id = "usrrts-restrict-backup-privilege",
            Label = "Restrict SeBackupPrivilege to Backup Operators Only",
            Category = "User Rights Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "SeBackupPrivilege allows bypassing file and directory permissions to read any file on the system for backup purposes including the SAM database and NTDS.dit. Restricting backup privilege to designated backup operator accounts prevents standard users from using backup APIs to extract protected files. Unauthorized use of SeBackupPrivilege is a documented technique for reading the NTDS.dit Active Directory database without triggering normal access control auditing. Service accounts and applications that require file backup capabilities should be granted backup privilege through membership in the Backup Operators group rather than individually. Monitoring backup privilege use events in the security event log helps identify unauthorized access to protected files through backup APIs. Backup privilege restriction is especially important for domain controllers where NTDS.dit contains all domain password hashes.",
            Tags = ["privilege", "backup", "file-access", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSeBackupPrivilege", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeBackupPrivilege")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSeBackupPrivilege", 1)],
        },
        new TweakDef
        {
            Id = "usrrts-restrict-restore-privilege",
            Label = "Restrict SeRestorePrivilege to Backup Operators Only",
            Category = "User Rights Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "SeRestorePrivilege allows overwriting any file or directory on the system bypassing normal access controls which could be used to replace critical system files. Restricting restore privilege to designated accounts prevents unauthorized users from using restore APIs to overwrite system binaries or security configuration files. Malicious use of restore privilege can replace legitimate Windows components with trojanized versions achieving persistent system compromise. System file replacement through restore privilege does not require disabling Windows File Protection if done through the backup APIs directly. Restore privilege is necessary for legitimate backup solutions and disaster recovery but should be restricted to dedicated service accounts. Monitoring restore privilege use events helps detect unauthorized file replacement operations that may indicate an active attack.",
            Tags = ["privilege", "restore", "file-integrity", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSeRestorePrivilege", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeRestorePrivilege")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSeRestorePrivilege", 1)],
        },
        new TweakDef
        {
            Id = "usrrts-restrict-load-driver-privilege",
            Label = "Restrict SeLoadDriverPrivilege to Administrators Only",
            Category = "User Rights Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "SeLoadDriverPrivilege allows loading and unloading device drivers into kernel memory providing complete system control to the account possessing it. Restricting driver load privilege to administrators ensures that only authorized accounts can introduce new code into kernel address space. Malicious drivers loaded through SeLoadDriverPrivilege can bypass antivirus, hide processes and files, and provide undetectable persistent access. Vulnerability exploitation has historically sometimes escalated from user to kernel via SeLoadDriverPrivilege held by non-admin accounts. Non-administrative accounts in Windows generally should not have driver load privilege unless there is a specific documented requirement. Driver load privilege audit events should be monitored with alerting for any driver loading by unexpected accounts or outside of authorized change windows.",
            Tags = ["privilege", "drivers", "kernel", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSeLoadDriverPrivilege", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeLoadDriverPrivilege")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSeLoadDriverPrivilege", 1)],
        },
        new TweakDef
        {
            Id = "usrrts-restrict-take-ownership",
            Label = "Restrict SeTakeOwnershipPrivilege to Administrators Only",
            Category = "User Rights Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "SeTakeOwnershipPrivilege allows taking ownership of any file, directory, or other object overriding access control lists regardless of the current owner's settings. Restricting take ownership privilege prevents unauthorized users from claiming ownership of files they do not have permission to access. Take ownership attacks allow gaining access to protected files like ntds.dit or private keys by claiming ownership and then modifying ACLs to grant access. Administrative accounts legitimately need this privilege for maintenance operations but standard and service accounts generally should not. Take ownership events should be audited as legitimate use is rare and unauthorized use is a strong indicator of attempted privilege abuse. Restricting take ownership privilege reduces the risk of data access through ACL manipulation attacks.",
            Tags = ["privilege", "ownership", "acl", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSeTakeOwnership", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeTakeOwnership")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSeTakeOwnership", 1)],
        },
        new TweakDef
        {
            Id = "usrrts-restrict-impersonate-privilege",
            Label = "Restrict SeImpersonatePrivilege to Service Accounts Only",
            Category = "User Rights Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "SeImpersonatePrivilege allows a process to impersonate another user's security token which can be used for token theft attacks to escalate privileges. Restricting impersonate privilege to service accounts and administrative groups prevents standard users from using impersonation for privilege escalation. Juicy Potato and similar token impersonation attacks exploit SeImpersonatePrivilege to escalate from service account to SYSTEM level access. Web application service accounts that hold impersonate privilege are common attack targets for token impersonation after exploiting web application vulnerabilities. Service accounts that require impersonation should be granted it explicitly through security group membership rather than directly and usage should be monitored. Restricting impersonate privilege is particularly important for IIS application pool accounts and other internet-facing service accounts.",
            Tags = ["privilege", "impersonation", "token-theft", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSeImpersonatePrivilege", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeImpersonatePrivilege")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSeImpersonatePrivilege", 1)],
        },
        new TweakDef
        {
            Id = "usrrts-remove-network-logon-guests",
            Label = "Deny Network Logon Rights to Guest Accounts",
            Category = "User Rights Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Network logon rights for guest accounts allow unauthenticated or guest-authenticated users to access network resources on systems where guests are allowed. Explicitly denying network logon to guest accounts prevents null-session attacks and anonymous network access to shared system resources. Guest account network access has been used to enumerate system information without authentication providing attackers a foothold for further attacks. Denying network logon rights to guests should be combined with disabling the Guest account itself for defense-in-depth. Even disabled guest accounts should have network logon rights denied to prevent accidental re-enabling from creating an access pathway. Legacy Windows networks sometimes relied on guest access for file sharing compatibility but modern environments should not have guest network access enabled.",
            Tags = ["privilege", "guest", "network-logon", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DenyNetworkLogonGuests", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DenyNetworkLogonGuests")],
            DetectOps = [RegOp.CheckDword(Key, "DenyNetworkLogonGuests", 1)],
        },
        new TweakDef
        {
            Id = "usrrts-restrict-shutdown-privilege",
            Label = "Restrict Remote Shutdown Privilege to Administrators",
            Category = "User Rights Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Remote shutdown privilege allows a user to shut down or restart a system remotely which can be used as a denial-of-service attack against production systems. Restricting remote shutdown privilege to administrators prevents standard users and service accounts from remotely restarting critical systems. Unauthorized remote shutdown capability could be exploited to disrupt services interrupt availability and trigger incidents that distract security teams from the primary attack. Production servers and domain controllers should only allow authorized IT administrators to perform remote shutdown operations. Remote shutdown events should be audited and alerts generated for shutdowns outside of authorized maintenance windows. Restricting shutdown privilege is a defense-in-depth control that limits the impact an attacker can cause with hijacked user credentials.",
            Tags = ["privilege", "shutdown", "availability", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictRemoteShutdown", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictRemoteShutdown")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictRemoteShutdown", 1)],
        },
        new TweakDef
        {
            Id = "usrrts-restrict-create-global-objects",
            Label = "Restrict SeCreateGlobalPrivilege to Trusted Service Accounts",
            Category = "User Rights Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "SeCreateGlobalPrivilege allows creating global Windows objects accessible from any user session which can be used for privilege escalation through global object manipulation. Restricting global object creation privilege to trusted service accounts prevents standard users from creating objects that can interfere with other user sessions. Global object namespace attacks can be used by malware to create objects with predictable names that privileged processes will open allowing exploitation. Applications that require creating global objects should be specifically evaluated and granted this privilege through dedicated service accounts. Global object creation restriction is particularly relevant for applications that accept data from untrusted sources and have privileged service components. Auditing global object creation events helps identify unauthorized use of this privilege that may indicate an active attack.",
            Tags = ["privilege", "global-objects", "privilege-escalation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSeCreateGlobal", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeCreateGlobal")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSeCreateGlobal", 1)],
        },
        new TweakDef
        {
            Id = "usrrts-restrict-act-as-os-privilege",
            Label = "Restrict SeTcbPrivilege (Act as Part of OS) to System Accounts",
            Category = "User Rights Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "SeTcbPrivilege (Trusted Computing Base privilege) grants a process the ability to act as part of the operating system itself with the highest possible level of system access. Restricting SeTcbPrivilege to only SYSTEM and LocalService accounts prevents any user or service from obtaining OS-level capabilities that bypass all security controls. Accounts holding SeTcbPrivilege can create security tokens for any user and impersonate any principal including SYSTEM without restriction. Any process running under an account with SeTcbPrivilege has complete and unrestricted access to the entire system making credential theft of such accounts catastrophic. No administrative user account should hold SeTcbPrivilege as even administrators should operate without full OS-level access. Monitoring for accounts added to the holders of SeTcbPrivilege should generate immediate security alerts.",
            Tags = ["privilege", "tcb", "act-as-os", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSeTcbPrivilege", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeTcbPrivilege")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSeTcbPrivilege", 1)],
        },
    ];
}
