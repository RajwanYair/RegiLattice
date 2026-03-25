// RegiLattice.Core — Tweaks/ServiceAccountPolicy.cs
// Sprint 339: Service Account Policy tweaks (10 tweaks)
// Category: "Service Account Policy" | Slug: svcact
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ServiceAccounts

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ServiceAccountPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ServiceAccounts";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "svcact-enable-managed-service-accounts",
            Label = "Enable Managed Service Accounts for Windows Services",
            Category = "Service Account Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Managed Service Accounts (MSAs) are Active Directory accounts specifically designed for services with automatic password management eliminating manually managed service account passwords. Enabling managed service accounts ensures that service authentication uses automatically rotated passwords that cannot be guessed through credential stuffing attacks. Traditional service accounts use static passwords that are often set to never expire creating long-term credential material vulnerabilities if captured. MSAs integrate with Active Directory to automatically change their passwords on a regular schedule without requiring service restarts. Group Managed Service Accounts (gMSAs) extend MSAs to support multiple servers using the same service account reducing administrative overhead for clustered services. Organizations should migrate all services from domain accounts with static passwords to MSAs or gMSAs during their next infrastructure refresh.",
            Tags = ["service-accounts", "msa", "password-management", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableManagedServiceAccounts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableManagedServiceAccounts")],
            DetectOps = [RegOp.CheckDword(Key, "EnableManagedServiceAccounts", 1)],
        },
        new TweakDef
        {
            Id = "svcact-restrict-service-account-interactive",
            Label = "Prevent Service Accounts from Interactive Logon",
            Category = "Service Account Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Service accounts should only authenticate to services they run and should never be used for interactive desktop logon on any system. Preventing service account interactive logon ensures that service credentials cannot be used by attackers for lateral movement through terminal services or direct console access. Service accounts used for interactive logon are stronger attack targets as operators may set easily remembered passwords rather than complex automated passwords. Disabling interactive logon for service accounts is enforced through Deny logon locally and Deny logon through Remote Desktop Services user rights. Service account logon restrictions should be enforced across all systems in the domain not just the servers running the specific services. Monitoring for attempted interactive logon with service account credentials is a detection pattern for credential theft and misuse.",
            Tags = ["service-accounts", "interactive-logon", "lateral-movement", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DenyInteractiveLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DenyInteractiveLogon")],
            DetectOps = [RegOp.CheckDword(Key, "DenyInteractiveLogon", 1)],
        },
        new TweakDef
        {
            Id = "svcact-enforce-service-account-password-complexity",
            Label = "Enforce Strong Password Complexity for Service Accounts",
            Category = "Service Account Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Service account password complexity requirements ensure that manually managed service credentials meet minimum entropy requirements to resist brute-force and dictionary attacks. Enforcing strong passwords for service accounts applies password complexity rules specifically to accounts designated for service use. Service account passwords should be at least 25 characters with mixed character classes to resist offline cracking if the hash is compromised. Password complexity enforcement for service accounts should be higher than standard user requirements due to the elevated privileges service accounts typically hold. Organizations should automate service account password management through vaults like Azure Key Vault or CyberArk PAM rather than relying on manually managed static passwords. Service account password rotation policies should ensure rotation at least quarterly or more frequently for accounts with access to sensitive systems.",
            Tags = ["service-accounts", "password-complexity", "credentials", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforcePasswordComplexity", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforcePasswordComplexity")],
            DetectOps = [RegOp.CheckDword(Key, "EnforcePasswordComplexity", 1)],
        },
        new TweakDef
        {
            Id = "svcact-audit-service-account-usage",
            Label = "Enable Audit Logging for Service Account Authentication",
            Category = "Service Account Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Service account authentication auditing generates events for all authentication operations performed by service accounts for security monitoring and anomaly detection. Enabling service account audit logging creates visibility into normal service communication patterns that baseline detectors use to identify anomalies. Abnormal service account activity such as authentication to hosts not normally accessed by the service indicates potential credential theft and lateral movement. Service account credential stuffing attacks where the same service credentials are tried against multiple systems are detectable through authentication audit events. Service account audit data should be forwarded to SIEM with behavioral analytics to detect deviations from established normal authentication patterns. Regular review of service account audit data reduces the time to detect service account compromise which is a common attacker technique.",
            Tags = ["service-accounts", "audit", "authentication", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditServiceAccountUsage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditServiceAccountUsage")],
            DetectOps = [RegOp.CheckDword(Key, "AuditServiceAccountUsage", 1)],
        },
        new TweakDef
        {
            Id = "svcact-restrict-service-account-delegation",
            Label = "Restrict Unconstrained Kerberos Delegation for Service Accounts",
            Category = "Service Account Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Unconstrained Kerberos delegation allows a service to request Kerberos tickets on behalf of any principal and present them to any service creating a powerful privilege escalation path. Restricting unconstrained delegation forces service accounts to use constrained delegation specifying only the services they are authorized to access on behalf of users. Unconstrained delegation accounts are commonly targeted in Active Directory attacks because compromising them provides lateral movement to any service authenticated users access through them. Constrained delegation limits the blast radius of a service account compromise to only the specific services defined in its delegation configuration. Resource-Based Constrained Delegation is the most modern form and should be preferred over traditional constrained delegation for new service deployments. Organizations should audit all accounts with unconstrained delegation enabled and restrict them to constrained delegation as a priority remediation.",
            Tags = ["service-accounts", "kerberos", "delegation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictUnconstrainedDelegation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictUnconstrainedDelegation")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictUnconstrainedDelegation", 1)],
        },
        new TweakDef
        {
            Id = "svcact-enable-tiered-service-access",
            Label = "Enable Tiered Access for Service Accounts by Privilege Level",
            Category = "Service Account Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Tiered access control for service accounts restricts service account usage to specific privilege tiers preventing tier-0 service accounts from being used on tier-1 or tier-2 systems. Enabling tiered service account access implements Active Directory tiered administrative model principles for non-administrative service accounts. Service accounts with access to tier-0 systems like domain controllers should be different from service accounts used on tier-1 member servers. Tier-based separation prevents lateral movement from lower-tier compromised systems from directly reaching tier-0 infrastructure through service account credentials. Tiered service accounts should be enforced through security group policies that deny logon rights at inappropriate tiers. The Microsoft Enterprise Access Model documentation provides implementation guidance for tiered service account architecture.",
            Tags = ["service-accounts", "tiered-access", "privilege-isolation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableTieredServiceAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableTieredServiceAccess")],
            DetectOps = [RegOp.CheckDword(Key, "EnableTieredServiceAccess", 1)],
        },
        new TweakDef
        {
            Id = "svcact-prevent-spn-enumeration",
            Label = "Restrict Unauthenticated Service Principal Name Enumeration",
            Category = "Service Account Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Service Principal Name (SPN) enumeration allows attackers to discover service accounts with SPNs for Kerberoasting attacks requesting and offline cracking Kerberos service tickets. Restricting SPN enumeration to authenticated domain members reduces the ease of discovering Kerberoastable service accounts. While all authenticated users can query SPN data by default additional restrictions can limit SPN data visibility to authorized groups. Service accounts with weak passwords are vulnerable to Kerberoasting even with SPN visibility restrictions so password strength is complementary to this control. Organizations should implement long random passwords for all service accounts with SPNs as the primary Kerberoasting defense. Alerting on high-volume SPN queries from a single source helps detect Kerberoasting enumeration activities in the domain.",
            Tags = ["service-accounts", "spn", "kerberoasting", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSPNEnumeration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSPNEnumeration")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSPNEnumeration", 1)],
        },
        new TweakDef
        {
            Id = "svcact-disable-service-account-email-usage",
            Label = "Prevent Service Accounts from Accessing Email Mailboxes",
            Category = "Service Account Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Service accounts should not be associated with email mailboxes as phishing attacks against service accounts via email are a common compromise vector. Disabling email access for service accounts prevents attackers from using phishing or credential stuffing to access corporate email through service account credentials. Service accounts with email access have been used to exfiltrate sensitive data by attackers who compromise them through shared password reuse. Service accounts by definition are not human users and do not need email communication capabilities for their service functions. Organizations should disable Exchange Online licensing for service accounts and configure mailbox restrictions to prevent service account email access. Service accounts found accessing email in audit logs should be investigated immediately as this indicates potential credential compromise.",
            Tags = ["service-accounts", "email", "phishing-resistance", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableServiceAccountEmail", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableServiceAccountEmail")],
            DetectOps = [RegOp.CheckDword(Key, "DisableServiceAccountEmail", 1)],
        },
        new TweakDef
        {
            Id = "svcact-set-service-account-logon-hours",
            Label = "Restrict Service Account Logon to Business Hours Windows",
            Category = "Service Account Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Service account logon hour restrictions limit when interactive authentication with service credentials can occur reducing the window available for credential abuse. Restricting service account logon to operational hours creates anomaly detection opportunities when service accounts are used outside their normal hours. Service accounts running scheduled tasks may need logon hours that cover their operational schedule rather than full 24-hour access. Attackers who capture service credentials prefer to use them during off-hours when security monitoring coverage may be lower making logon hour restrictions an effective detective control. Service accounts running continuous services like databases require logon access at all hours but batch processing accounts can be restricted to business hours. Logon attempts outside the defined window generate authentication failure events that should be alerted on immediately.",
            Tags = ["service-accounts", "logon-hours", "access-control", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceLogonHours", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceLogonHours")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceLogonHours", 1)],
        },
        new TweakDef
        {
            Id = "svcact-enable-just-in-time-service-access",
            Label = "Enable Just-in-Time Access Elevation for Service Accounts",
            Category = "Service Account Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Just-in-time access elevation for service accounts grants elevated service permissions only when needed and revokes them after the task completes reducing standing privilege. Enabling JIT access for service privileges ensures that service accounts do not continuously hold elevated rights that could be abused if the account is compromised. Standing elevated privileges are the primary enabler of lateral movement where a single compromised account can immediately access critical resources. JIT access implementation requires a Privileged Access Management system like Azure AD PIM or CyberArk PAM to issue time-limited permission grants. Service account JIT access is more complex to implement than user JIT but provides the same significant security benefits for automated workloads. Organizations implementing JIT for service accounts should prioritize accounts with access to sensitive data or critical infrastructure.",
            Tags = ["service-accounts", "jit", "privileged-access", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableJITServiceAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableJITServiceAccess")],
            DetectOps = [RegOp.CheckDword(Key, "EnableJITServiceAccess", 1)],
        },
    ];
}
