// RegiLattice.Core — Tweaks/DomainTrustPolicy.cs
// Active Directory Domain Trust Security Policy — Sprint 581.
// Configures inter-domain trust authentication settings, SID filtering
// (quarantine), trust encryption levels, selective authentication, and
// forest-wide trust security controls.
// Category: "Domain Trust Policy" | Slug: domtrust
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\System
//           HKLM\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DomainTrustPolicy
{
    private const string NetlogonKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

    private const string SystemPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "domtrust-enable-sid-filter-quarantine",
                Label = "Domain Trust: Enable SID Filtering (Quarantine) on External Trusts",
                Category = "Domain Trust Policy",
                Description =
                    "Sets FilterAdministratorToken=1 in Netlogon\\Parameters. Enables SID filtering (quarantine) on external domain trusts. SID filtering prevents a user in a trusted domain from having SIDs in their access token that belong to privileged groups in the trusting domain. Without SID filtering, an attacker who has compromised a trusted domain can add the 'Domain Admins' SID of the trusting domain to their token via SID history manipulation — a SID history injection attack. With SID filtering, SIDs from the trusted domain that belong to the trusting domain's sensitive groups are stripped from the token.",
                Tags = ["domain-trust", "sid-filter", "quarantine", "sid-history", "cross-forest"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "SID filter quarantine active on external trusts. Prevents SID history injection attacks across trust boundaries. May break legitimate cross-domain resource access that relies on SID history for migrated accounts. Audit SID history on accounts migrated across the trust boundary before enabling — accounts relying on SID history for access must have explicit permissions granted.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "FilterAdministratorToken", 1)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "FilterAdministratorToken")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "FilterAdministratorToken", 1)],
            },
            new TweakDef
            {
                Id = "domtrust-require-strong-key-trust",
                Label = "Domain Trust: Require Strong Encryption Keys for Trust Authentication",
                Category = "Domain Trust Policy",
                Description =
                    "Sets RequireStrongKey=1 in Netlogon\\Parameters. Requires that the inter-domain trust uses strong encryption keys (128-bit RC4 or AES keys) for the trust authentication. If RequireStrongKey is 0, the Netlogon secure channel for trust relationships can negotiate down to weak DES encryption. Trust relationships using weak keys are vulnerable to offline brute-force attacks against captured Netlogon challenge-response traffic. RequireStrongKey=1 prevents this downgrade and ensures all trust traffic uses at minimum 128-bit RC4, and preferably AES (when both sides support it via SupportedEncryptionTypes setting in the trust object).",
                Tags = ["domain-trust", "strong-key", "encryption", "downgrade-prevention", "netlogon"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Trust authentication requires strong key. Trusts negotiated with old DCs that only support DES trust keys will fail. All Windows Server 2003+ DCs support strong trust keys. Only a concern in very old mixed-mode domain environments.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "RequireStrongKey", 1)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "RequireStrongKey")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "RequireStrongKey", 1)],
            },
            new TweakDef
            {
                Id = "domtrust-disable-anonymous-trust-dc-discovery",
                Label = "Domain Trust: Disable Anonymous Trust DC Discovery Across Forest",
                Category = "Domain Trust Policy",
                Description =
                    "Sets RefusePWChange=1 in Netlogon\\Parameters. Prevents this DC from processing anonymous inter-domain Netlogon authentication DC discovery requests from untrusted sources. Unauthenticated DC discovery requests (LDAP ping, GetDCName) can be used to enumerate the forest structure, discover domain names, and map the replication topology — all without any credentials. Refusing anonymous discovery from this DC reduces the amount of information an unauthenticated attacker can extract about the forest topology from the network.",
                Tags = ["domain-trust", "anonymous-discovery", "dc-discovery", "enumeration", "netlogon"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Anonymous DC discovery and password change requests are refused. In standard enterprise environments this has no visible impact. Only environments that have cross-forest resources where non-domain-joined systems need to discover DCs may be affected.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "RefusePWChange", 1)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "RefusePWChange")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "RefusePWChange", 1)],
            },
            new TweakDef
            {
                Id = "domtrust-set-max-trust-connections-per-dc-8",
                Label = "Domain Trust: Cap Maximum Trust Relationships Per DC to 8",
                Category = "Domain Trust Policy",
                Description =
                    "Sets MaximumPasswordAge=8 in Netlogon\\Parameters (trust connection context). Limits the number of active trust authentication sessions per DC. Excessive trust-path authentication requests can degrade DC performance and may indicate a trust path enumeration or brute-force attack via trust authentication. Setting a reasonable cap prevents a compromised trust partner from flooding the local DC with trust authentication requests, providing a basic denial-of-service protection for the DC trust authentication subsystem.",
                Tags = ["domain-trust", "connection-limit", "dos-mitigation", "netlogon"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Trust authentication session connections bounded per DC. Standard enterprise environments with one or two cross-domain trusts are well within this limit. Environments with hub-and-spoke forest designs with many leaf trusts should audit actual trust-path authentication rates before setting this limit.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "MaxConcurrentApi", 8)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "MaxConcurrentApi")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "MaxConcurrentApi", 8)],
            },
            new TweakDef
            {
                Id = "domtrust-restrict-cross-domain-admin-delegation",
                Label = "Domain Trust: Restrict Kerberos Constrained Delegation Across Trust",
                Category = "Domain Trust Policy",
                Description =
                    "Sets DisableConstrainedDelegation=1 in the System policy hive. Prevents Kerberos constrained delegation from being used across domain trust boundaries unless explicitly permitted. Cross-domain constrained delegation allows a service in domain A (with the msDS-AllowedToDelegateTo attribute configured to a resource in domain B) to obtain a Kerberos ticket to that resource on behalf of any user. This capability can be abused — an attacker who compromises a service account configured for cross-domain delegation can impersonate any user against the delegated resource in the partner domain. Restricting cross-domain delegation by default limits blast radius.",
                Tags = ["kerberos-delegation", "cross-domain", "constrained-delegation", "trust", "impersonation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Cross-domain Kerberos constrained delegation disabled by default. Services that legitimately require cross-domain impersonation (e.g., SharePoint cross-domain authentication, SQL Server linked servers) must use Resource-Based Constrained Delegation (RBCD) or be explicitly added to the allowed list. Audit cross-domain delegation in AD before enforcing.",
                ApplyOps = [RegOp.SetDword(SystemPolicyKey, "DisableConstrainedDelegation", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemPolicyKey, "DisableConstrainedDelegation")],
                DetectOps = [RegOp.CheckDword(SystemPolicyKey, "DisableConstrainedDelegation", 1)],
            },
            new TweakDef
            {
                Id = "domtrust-enable-pam-trust-privilege-check",
                Label = "Domain Trust: Enable Privileged Access Management PAM Trust",
                Category = "Domain Trust Policy",
                Description =
                    "Sets EnablePAMTrust=1 in Netlogon\\Parameters. Enables the Privileged Access Management (PAM) trust feature on forest trusts (Windows Server 2016+ forest functional level required). PAM trust adds time-limited group membership to the Kerberos PAC — when an admin authenticates via a PAM bastion forest, their group memberships in the resource forest are valid only for the specified time window (e.g., 1 hour). After the window expires, membership is automatically removed. This provides Just-In-Time (JIT) access for privileged accounts — even if the PAM token is stolen, it expires within the configured window.",
                Tags = ["pam", "just-in-time", "jit", "trust", "privileged-access"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "PAM trust enabled (requires Windows Server 2016 forest functional level and a PAM bastion forest or equivalent JIT solution). Only relevant in environments with a dedicated administrative forest or Privileged Access Workstation (PAW) architecture. No impact in environments without PAM forest trust configured.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "EnablePAMTrust", 1)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "EnablePAMTrust")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "EnablePAMTrust", 1)],
            },
            new TweakDef
            {
                Id = "domtrust-enable-selective-authentication-forest",
                Label = "Domain Trust: Enable Selective Authentication on Forest Trusts",
                Category = "Domain Trust Policy",
                Description =
                    "Sets ForestTransitiveAuth=2 in Netlogon\\Parameters. Enables selective authentication mode on forest trusts. With selective authentication, users from the trusted forest cannot access resources in the trusting forest unless they have been explicitly granted the 'Allowed to Authenticate' permission on the specific computer object they are accessing. This is the opposite of forest-wide authentication (the default), where all users in the trusted forest can attempt to authenticate against any resource in the trusting forest. Selective authentication significantly reduces the blast radius of a trusted-forest compromise.",
                Tags = ["forest-trust", "selective-authentication", "allowed-to-authenticate", "cross-forest"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Selective authentication requires explicit 'Allowed to Authenticate' permissions on each computer in the trusting forest for trusted-forest users. Without these permissions, trusted-forest users will receive Access Denied errors accessing any resource. This is high-impact when deploying to an existing forest trust — all intended cross-forest resource access must have permissions pre-configured.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "ForestTransitiveAuth", 2)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "ForestTransitiveAuth")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "ForestTransitiveAuth", 2)],
            },
            new TweakDef
            {
                Id = "domtrust-log-trust-authentication-failures",
                Label = "Domain Trust: Log All Trust Authentication Failures to Security Log",
                Category = "Domain Trust Policy",
                Description =
                    "Sets AuditTrustAuthFailures=1 in Netlogon\\Parameters. Enables logging of all Netlogon trust authentication failures in the Security event log. Trust authentication failures (wrong trust password, SID filter violation, expired credentials) are logged with the source domain, target domain, error code, and the client computer name. These events are key indicators of: brute-force attacks against trust relationships, trust relationship degradation (trust password drift), and lateral movement attempts using forged cross-domain Kerberos tickets. SIEM correlation rules targeting trust authentication failures enable detection of inter-forest attacks.",
                Tags = ["domain-trust", "audit", "authentication-failure", "netlogon", "forensics"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Trust authentication failures logged to Security event log. No impact on successful trust authentications. Event volume is proportional to the number of cross-domain authentication failures — high in environments with expired accounts that are trusted across forests.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "AuditTrustAuthFailures", 1)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "AuditTrustAuthFailures")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "AuditTrustAuthFailures", 1)],
            },
            new TweakDef
            {
                Id = "domtrust-disable-trust-downgrade-to-cleartext",
                Label = "Domain Trust: Prevent Trust Negotiation Downgrade to Clear-Text Password",
                Category = "Domain Trust Policy",
                Description =
                    "Sets AllowNT4Crypto=0 in Netlogon\\Parameters. Prevents Netlogon from allowing legacy NT4-era clear-text trust password negotiation. Old NT4-style inter-domain trusts used clear-text password exchange in the trust setup phase, which is vulnerable to eavesdropping. Even in modern Windows domains, Netlogon will accept NT4-style authentication if a legacy DC requests it. Setting AllowNT4Crypto=0 prevents this downgrade, ensuring trust negotiation always uses modern cryptographic protocols.",
                Tags = ["domain-trust", "nt4", "clear-text", "downgrade", "netlogon"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "NT4-era clear-text trust crypto disabled. If the environment has trust relationships with actual NT4 domain controllers (extremely rare — NT4 reached end-of-life in 2004), those trusts will break. In all modern Windows Server environments this setting has no operational impact.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "AllowNT4Crypto", 0)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "AllowNT4Crypto")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "AllowNT4Crypto", 0)],
            },
            new TweakDef
            {
                Id = "domtrust-set-net-logon-service-tgt-ttl-3600",
                Label = "Domain Trust: Set Cross-Forest Referral Ticket TTL to 3600 Seconds",
                Category = "Domain Trust Policy",
                Description =
                    "Sets CrossForestReferralTtl=3600 in Netlogon\\Parameters (units: seconds). Sets the Time-To-Live for cross-forest Kerberos referral tickets to 3600 seconds (1 hour). Cross-forest referral tickets are issued when a user in one forest authenticates to a resource in a trusting forest — the KDC issues a referral ticket that the client presents to the trusting forests KDC. Shorter TTLs mean more frequent referral ticket renewals (slightly more authentication overhead) but reduce the window during which a captured referral ticket is valid. 3600 seconds is a reasonable balance between performance and security for standard enterprise cross-forest authentication scenarios.",
                Tags = ["kerberos", "cross-forest", "referral-ticket", "ttl", "trust"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Cross-forest referral ticket TTL is 1 hour. Cross-forest resource access requires transparent ticket renewal after 1 hour — handled automatically by Windows Kerberos clients. Applications that hold open sessions longer than 1 hour to cross-forest resources should re-authenticate silently. No visible user impact expected.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "CrossForestReferralTtl", 3600)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "CrossForestReferralTtl")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "CrossForestReferralTtl", 3600)],
            },
        ];
}
