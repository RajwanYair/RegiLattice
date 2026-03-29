// RegiLattice.Core — Tweaks/LdapClientPolicy.cs
// LDAP Client Security Policy — Sprint 578.
// Configures LDAP client signing, channel binding, TLS enforcement,
// and LDAP session security for clients communicating with Active
// Directory domain controllers.
// Category: "LDAP Client Policy" | Slug: ldapclnt
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\LDAP
//           HKLM\SYSTEM\CurrentControlSet\Services\ldap

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LdapClientPolicy
{
    private const string LdapPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LDAP";

    private const string LdapSvcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ldap";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ldapclnt-require-ldap-signing",
                Label = "LDAP Client: Require LDAP Signing (Negotiate/Require)",
                Category = "LDAP Client Policy",
                Description =
                    "Sets LDAPClientIntegrity=2 in the LDAP policy hive (value 2 = Require signing; value 1 = Negotiate signing; value 0 = None). Requires LDAP clients to request LDAP packet signing on all LDAP connections to domain controllers. Without LDAP signing, the LDAP exchange is susceptible to LDAP relay attacks — an attacker who can perform a man-in-the-middle attack can modify LDAP query results without detection. LDAP signing is part of the security hardening recommended by Microsoft Security Advisory ADV190023 (LDAP channel binding and LDAP signing). Combined with LDAP channel binding, this closes a class of LDAP relay and NTLM relay attacks.",
                Tags = ["ldap", "signing", "integrity", "adv190023", "relay"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "LDAP clients require signing for all DC connections. Legacy LDAP applications that use anonymous LDAP binds or simple (plaintext) binds without signing will fail. Audit LDAP usage with DC diagnostic logging (Set 16 LDAP Interface Events to 2) before enforcing. Older UNIX/Linux LDAP clients may need PAM/NSS configuration updates.",
                ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LDAPClientIntegrity", 2)],
                RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LDAPClientIntegrity")],
                DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LDAPClientIntegrity", 2)],
            },
            new TweakDef
            {
                Id = "ldapclnt-require-ldap-channel-binding",
                Label = "LDAP Client: Require LDAP Channel Binding Token (CBT)",
                Category = "LDAP Client Policy",
                Description =
                    "Sets LdapEnforceChannelBinding=2 in the LDAP policy hive (value 2 = Always require channel binding; value 1 = Supported; value 0 = Never). LDAP Channel Binding Tokens (CBT) cryptographically bind an LDAP authentication exchange to the specific TLS channel it is using. This prevents LDAP relay attacks where an attacker intercepts an NTLM authentication for an LDAP-over-TLS connection and replays it over a different TLS connection (cross-channel relay). Combined with LDAP signing enforcement, this closes the NTLM relay to LDAP attack vector used by tools like Responder and ntlmrelayx.",
                Tags = ["ldap", "channel-binding", "cbt", "ntlm-relay", "tls"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "LDAP channel binding enforced on all connections. Applications that use LDAP-over-TLS (LDAPS on port 636) with NTLM authentication must support channel binding tokens. Older LDAP client libraries (OpenLDAP < 2.5, Python-ldap without CBT patches) will fail LDAPS authentication. Survey LDAP client versions before enforcing.",
                ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LdapEnforceChannelBinding", 2)],
                RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LdapEnforceChannelBinding")],
                DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LdapEnforceChannelBinding", 2)],
            },
            new TweakDef
            {
                Id = "ldapclnt-set-ldap-client-timeout-120s",
                Label = "LDAP Client: Set LDAP Search and Connection Timeout to 120 Seconds",
                Category = "LDAP Client Policy",
                Description =
                    "Sets LdapClientTimeout=120 in the LDAP policy hive (units: seconds). Sets the maximum number of seconds an LDAP client will wait for a search result before terminating the operation. Without a timeout, an LDAP client that connects to a slow or unresponsive domain controller can hold open connections indefinitely — an attacker who controls a fake DC can exploit this by responding very slowly to keep the LDAP connection open and drain client resources. Setting a bounded timeout ensures that LDAP operations fail gracefully when the DC is unresponsive, and the client can fall back to another DC.",
                Tags = ["ldap", "timeout", "connection-limit", "dc-failover", "dos-mitigation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "LDAP search and connection timeout is 120 seconds. Operations that require longer LDAP searches (large group enumeration, deep OU subtree searches) may time out in environments with extremely large directories. Monitor LDAP timeout events in application logs on clients running LDAP-intensive applications.",
                ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LdapClientTimeout", 120)],
                RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LdapClientTimeout")],
                DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LdapClientTimeout", 120)],
            },
            new TweakDef
            {
                Id = "ldapclnt-disable-ldap-anonymous-bind",
                Label = "LDAP Client: Disable Unauthenticated (Anonymous) LDAP Bind",
                Category = "LDAP Client Policy",
                Description =
                    "Sets DisableAnonymousBind=1 in the LDAP policy hive. Prevents LDAP clients from performing anonymous (unauthenticated) LDAP binds to Active Directory domain controllers. Anonymous LDAP binds historically allowed any system on the network to query AD for directory information (user accounts, group memberships, computer accounts) without authenticating. While Windows Server 2003 and later restrict anonymous LDAP read access by default at the DC level, client-side enforcement ensures that applications in the environment never attempt anonymous LDAP binds — a pattern that could succeed against non-standard LDAP servers or legacy DCs with weakened configuration.",
                Tags = ["ldap", "anonymous-bind", "authentication", "directory-enumeration"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Anonymous LDAP binds are blocked at the client level. Applications that use anonymous LDAP to query directory info without credentials will fail. Older monitoring tools and health check scripts that rely on anonymous LDAP must be updated to use service account credentials.",
                ApplyOps = [RegOp.SetDword(LdapPolicyKey, "DisableAnonymousBind", 1)],
                RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "DisableAnonymousBind")],
                DetectOps = [RegOp.CheckDword(LdapPolicyKey, "DisableAnonymousBind", 1)],
            },
            new TweakDef
            {
                Id = "ldapclnt-enforce-ldaps-port-636",
                Label = "LDAP Client: Enforce LDAP over TLS (LDAPS) on Port 636",
                Category = "LDAP Client Policy",
                Description =
                    "Sets UseLdapSsl=1 in the LDAP policy hive. Enforces the use of LDAP over TLS (LDAPS on port 636) for LDAP connections. Standard LDAP on port 389 transmits all directory queries and responses, including credentials, in plaintext. An attacker performing network traffic capture on the corporate network can extract LDAP bind credentials, observe group memberships, and construct detailed maps of Active Directory structure. LDAPS encrypts the entire LDAP session using TLS. Combined with LDAP signing and channel binding, LDAPS provides end-to-end protection for directory communications.",
                Tags = ["ldap", "ldaps", "tls", "port-636", "plaintext-prevention"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "LDAP connections use LDAPS (port 636) with TLS. Domain controllers must have valid LDAP server certificates installed. Certificate authority chain must be trusted by all LDAP clients. LDAP port 389 connections from this client will be rejected by LDAPS-only DCs. Ensure DCs have DC certificates from an internal PKI before enforcing.",
                ApplyOps = [RegOp.SetDword(LdapPolicyKey, "UseLdapSsl", 1)],
                RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "UseLdapSsl")],
                DetectOps = [RegOp.CheckDword(LdapPolicyKey, "UseLdapSsl", 1)],
            },
            new TweakDef
            {
                Id = "ldapclnt-set-max-ldap-connections-500",
                Label = "LDAP Client: Cap Maximum Concurrent LDAP Connections to 500",
                Category = "LDAP Client Policy",
                Description =
                    "Sets MaxConnections=500 in the LDAP service key. Limits the number of concurrent LDAP connections the client interface will maintain to 500. Unbounded LDAP connections on an LDAP client can lead to memory exhaustion if an application has a connection leak or if a malicious application attempts to open large numbers of LDAP connections to degrade other directory services consumers on the same host. This setting provides a reasonable upper bound for normal enterprise usage while preventing connection floods.",
                Tags = ["ldap", "connection-limit", "resource-bound", "dos-mitigation", "memory"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Maximum of 500 concurrent LDAP connections. In typical enterprise environments the actual number of concurrent LDAP connections is under 20. Applications that open many parallel LDAP connection contexts (directory synchronisation tools, identity management systems) should be tested to confirm they stay under 500.",
                ApplyOps = [RegOp.SetDword(LdapSvcKey, "MaxConnections", 500)],
                RemoveOps = [RegOp.DeleteValue(LdapSvcKey, "MaxConnections")],
                DetectOps = [RegOp.CheckDword(LdapSvcKey, "MaxConnections", 500)],
            },
            new TweakDef
            {
                Id = "ldapclnt-enable-referral-following-sasl",
                Label = "LDAP Client: Require SASL Authentication When Following LDAP Referrals",
                Category = "LDAP Client Policy",
                Description =
                    "Sets FollowReferralsWithSasl=1 in the LDAP policy hive. Requires that when an LDAP client follows an LDAP referral (a response from one DC that redirects the client to query a different DC or domain), the subsequent connection to the referred server uses SASL (GSSAPI/Kerberos) authentication rather than simple bind. An attacker who can serve a crafted LDAP referral can attempt to redirect the client to a malicious LDAP server — if the client then connects using simple bind (password), the credentials can be captured. SASL with Kerberos prevents this: the referral target must prove its identity via Kerberos before credentials are presented.",
                Tags = ["ldap", "referral", "sasl", "kerberos", "credential-capture"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "SASL required when following LDAP referrals. Applications that follow referrals using simple bind must switch to SASL/Kerberos for the referred connection. Modern .NET LDAP libraries and Windows LDAP APIs handle this transparently. Custom LDAP implementations may require code changes.",
                ApplyOps = [RegOp.SetDword(LdapPolicyKey, "FollowReferralsWithSasl", 1)],
                RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "FollowReferralsWithSasl")],
                DetectOps = [RegOp.CheckDword(LdapPolicyKey, "FollowReferralsWithSasl", 1)],
            },
            new TweakDef
            {
                Id = "ldapclnt-enable-ldap-diagnostic-logging",
                Label = "LDAP Client: Enable LDAP Client Diagnostic Event Logging",
                Category = "LDAP Client Policy",
                Description =
                    "Sets LdapDiagnosticsEnabled=1 in the LDAP policy hive. Enables LDAP client diagnostic logging to the Windows Application event log. When enabled, LDAP authentication failures, TLS handshake errors, channel binding failures, and referral-following events are logged with details including the DC hostname, error code, and the identity attempting authentication. This logging is essential for detecting LDAP attacks (repeated anonymous bind attempts, LDAP relay attempts where channel binding fails) and for diagnosing LDAP signing/channel binding compatibility issues during enforcement rollout.",
                Tags = ["ldap", "diagnostics", "logging", "event-log", "forensics"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "LDAP diagnostic events are logged to the Application event log. Generates event log volume proportional to the number of LDAP operations. In environments with high LDAP query rates, review the log volume impact. Events appear under source 'LDAP Client'. Integrate with SIEM for attack detection use cases.",
                ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LdapDiagnosticsEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LdapDiagnosticsEnabled")],
                DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LdapDiagnosticsEnabled", 1)],
            },
            new TweakDef
            {
                Id = "ldapclnt-block-ntlm-ldap-fallback",
                Label = "LDAP Client: Block NTLM Fallback in LDAP Authentication Negotiation",
                Category = "LDAP Client Policy",
                Description =
                    "Sets BlockNtlmLdapFallback=1 in the LDAP policy hive. Prevents the LDAP client from falling back to NTLM authentication when Kerberos authentication to a domain controller fails. An attacker who performs a downgrade attack (e.g., interfering with Kerberos SPN resolution) can force an LDAP client to use NTLM instead of Kerberos for authentication. NTLM is weaker and susceptible to relay attacks. Blocking NTLM fallback forces the client to fail visibly when Kerberos is unavailable rather than silently using the weaker protocol — making downgrade attacks immediately visible in logs.",
                Tags = ["ldap", "ntlm-fallback", "kerberos", "downgrade", "relay"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "NTLM fallback for LDAP is blocked. When Kerberos authentication to a DC fails (e.g., SPN resolution failure, KDC unreachable), the LDAP operation fails rather than retrying with NTLM. This may cause authentication failures during DC failover events. Monitor LDAP authentication failures in event logs after enforcing.",
                ApplyOps = [RegOp.SetDword(LdapPolicyKey, "BlockNtlmLdapFallback", 1)],
                RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "BlockNtlmLdapFallback")],
                DetectOps = [RegOp.CheckDword(LdapPolicyKey, "BlockNtlmLdapFallback", 1)],
            },
            new TweakDef
            {
                Id = "ldapclnt-enforce-ldap-query-size-limit-1000",
                Label = "LDAP Client: Enforce Maximum LDAP Query Result Size of 1000 Objects",
                Category = "LDAP Client Policy",
                Description =
                    "Sets MaxPageSize=1000 in the LDAP policy hive. Limits LDAP query results to a maximum of 1000 directory objects per page. Unbounded LDAP queries (queries without a size limit) can return tens of thousands of objects, consuming excessive DC memory and CPU, and enabling bulk directory enumeration by an attacker who has obtained LDAP query access. Setting a page size limit of 1000 ensures that applications must use paged results (LDAP paging control) to enumerate large sets of objects — and an attacker attempting to dump the entire directory in one query receives an error and must iterate, increasing the attack duration and detectability.",
                Tags = ["ldap", "query-size", "paging", "enumeration", "dos-mitigation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "LDAP query result size limited to 1000 objects per response. Applications that depend on unbounded LDAP queries (returning >1000 objects in one response) must be updated to use LDAP paged results control (LDAP_CONTROL_PAGEDRESULTS). Most modern LDAP libraries support paged results automatically.",
                ApplyOps = [RegOp.SetDword(LdapPolicyKey, "MaxPageSize", 1000)],
                RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "MaxPageSize")],
                DetectOps = [RegOp.CheckDword(LdapPolicyKey, "MaxPageSize", 1000)],
            },
        ];
}
