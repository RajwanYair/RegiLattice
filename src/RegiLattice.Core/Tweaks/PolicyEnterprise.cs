namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyEnterprise
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _AdfsFederationPolicy.Data,
            .. _ActiveDirectoryServicesPolicy.Data,
            .. _AdReplicationPolicy.Data,
            .. _AzureVirtualDesktopPolicy.Data,
            .. _CloudPcWindows365Policy.Data,
            .. _ConfigurationManagerPolicy.Data,
            .. _DeploymentServicesPolicy.Data,
            .. _DomainControllerHardeningPolicy.Data,
            .. _DomainIsolationPolicy.Data,
            .. _DomainTrustPolicy.Data,
            .. _EasMdmPolicy.Data,
            .. _EnterpriseDeviceManagementPolicy.Data,
            .. _EnterpriseResourceDeployPolicy.Data,
            .. _EnterpriseResourcePolicy.Data,
            .. _EnterpriseStateRoamingPolicy.Data,
            .. _GpoFolderRedirPolicy.Data,
            .. _GpoScriptsPolicy.Data,
            .. _GroupPolicySettingsPolicy.Data,
            .. _HotpatchUpdatePolicy.Data,
            .. _HybridJoinDnsPolicy.Data,
            .. _IntuneDeviceEventPolicy.Data,
            .. _MdmEnrollmentPolicy.Data,
            .. _MdmRegistrationPolicy.Data,
            .. _OobePolicy.Data,
            .. _RetailDemoPolicy.Data,
            .. _SharedPCPolicy.Data,
            .. _WindowsAutopilotPolicy.Data,
            .. _WindowsDeploymentServicesPolicy.Data,
            .. _WindowsFlightedFeaturesPolicy.Data,
            .. _WindowsFlightingPolicy.Data,
            .. _WindowsInsider.Data,
            .. _WindowsServicingPolicy.Data,
            .. _WindowsToGoPolicy.Data,
        ];

    // ── AdfsFederationPolicy ──
    private static class _AdfsFederationPolicy
    {
        private const string AdfsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ADFS";
        private const string AuthKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Authentication";
        private const string SvcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\adfssrv\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "adfspol-enable-extranet-lockout",
                    Label = "Enable ADFS Extranet Smart Lockout",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets EnableExtranetLockout=1 in the ADFS policy. Activates ADFS Extranet Smart Lockout (ESL) which tracks authentication attempts from extranet (external) IP addresses separately from intranet ones. Extranet lockout prevents password spray and brute-force attacks from the internet from locking out Active Directory accounts while still allowing internal users to authenticate normally.",
                    Tags = ["adfs", "extranet", "lockout", "brute-force", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Activates smart lockout for extranet auth; requires ADFS to be deployed with WAPProxy for effect.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "EnableExtranetLockout", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "EnableExtranetLockout")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "EnableExtranetLockout", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-set-extranet-lockout-threshold",
                    Label = "Set ADFS Extranet Lockout Threshold (5 attempts)",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets ExtranetLockoutThreshold=5 in the ADFS policy. Defines the number of failed authentication attempts from an extranet IP address before ADFS blocks further attempts from that IP. Five failed attempts is the CIS recommendation that balances security against accidental account lockout from mistyped passwords on shared IP networks (NAT, VPN exit nodes).",
                    Tags = ["adfs", "extranet", "lockout", "threshold", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks extranet IPs after 5 failures; corporate NAT exit nodes may need threshold adjustment.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "ExtranetLockoutThreshold", 5)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "ExtranetLockoutThreshold")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "ExtranetLockoutThreshold", 5)],
                },
                new TweakDef
                {
                    Id = "adfspol-disable-endpoint-wia-fallback",
                    Label = "Disable ADFS Windows Integrated Auth Fallback",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets DisableWIAFallback=1 in the ADFS policy. Prevents ADFS from falling back to Windows Integrated Authentication (Kerberos/NTLM from browser) when the primary authentication method fails. WIA fallback can expose NTLM credentials when users authenticate from non-domain-joined browsers, potentially enabling NTLM relay attacks. Disabling fallback forces explicit form-based or certificate authentication.",
                    Tags = ["adfs", "wia", "fallback", "ntlm", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Prevents WIA fallback; Intranet users who previously used WIA from non-domain browsers must use forms instead.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "DisableWIAFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "DisableWIAFallback")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "DisableWIAFallback", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-require-ssl-certificate-auth",
                    Label = "Require TLS Certificate Authentication for ADFS Service",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets RequireCertificateAuthentication=1 in the ADFS service Parameters key. Enforces mutual TLS certificate authentication for ADFS service account communication. When mutual TLS is required the ADFS service will reject connections from components (proxy servers, relying party trusts) that do not present a valid certificate, preventing impersonation of trusted federation endpoints.",
                    Tags = ["adfs", "tls", "certificate", "mutual-auth", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Requires mutual TLS from all ADFS-connecting components; ensure proxies and RPs have valid certificates.",
                    ApplyOps = [RegOp.SetDword(SvcKey, "RequireCertificateAuthentication", 1)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "RequireCertificateAuthentication")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "RequireCertificateAuthentication", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-enable-oauth-pkce",
                    Label = "Require PKCE for ADFS OAuth2 Authorization Code Flow",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets RequirePKCEForOAuth=1 in the ADFS policy. Enforces Proof Key for Code Exchange (PKCE, RFC 7636) for all OAuth 2.0 authorization code flow requests to ADFS. PKCE prevents authorization code interception attacks where an attacker intercepts the authorization code redirect and exchanges it for tokens. Required by RFC 9700 (OAuth 2.0 Security Best Current Practice) for all public and confidential clients.",
                    Tags = ["adfs", "oauth", "pkce", "authorization-code", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Requires PKCE; legacy OAuth clients that do not send a code_challenge will be rejected.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "RequirePKCEForOAuth", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "RequirePKCEForOAuth")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "RequirePKCEForOAuth", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-disable-device-auth-bypass",
                    Label = "Disable ADFS Device Authentication Bypass",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets DisableDeviceAuthenticationBypass=1 in the ADFS policy. Prevents ADFS from bypassing multi-factor authentication requirements based solely on device registration status. When disabled, a registered device alone is not sufficient to skip MFA — users must still satisfy the full authentication policy. This closes a gap where attackers who enroll a stolen device could bypass step-up authentication.",
                    Tags = ["adfs", "device-auth", "mfa", "bypass", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Device registration no longer bypasses MFA; compliant device policies may need adjustment for Conditional Access.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "DisableDeviceAuthenticationBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "DisableDeviceAuthenticationBypass")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "DisableDeviceAuthenticationBypass", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-set-token-replay-detection",
                    Label = "Enable ADFS Token Replay Detection",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets EnableTokenReplayDetection=1 in the ADFS policy. Activates the ADFS token replay detection cache which records recently used security tokens and rejects any attempt to present the same token a second time. Token replay attacks occur when an attacker intercepts a SAML assertion or JWT and submits it to gain access. Detection is critical for federated SSO scenarios where tokens flow through multiple network intermediaries.",
                    Tags = ["adfs", "token-replay", "detection", "saml", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Enables token replay cache; negligible performance impact on ADFS server under normal SSO load.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "EnableTokenReplayDetection", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "EnableTokenReplayDetection")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "EnableTokenReplayDetection", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-require-extended-protection",
                    Label = "Require Extended Protection for ADFS Authentication",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets EnableExtendedProtection=1 in the ADFS authentication policy. Enables Extended Protection for Authentication (EPA) which binds the Windows authentication handshake to the TLS channel. EPA prevents NTLM relay attacks where an attacker forwards authentication attempts to the ADFS endpoint from a man-in-the-middle position. Supported in all Windows versions since Windows 7 SP1.",
                    Tags = ["adfs", "extended-protection", "ntlm-relay", "authentication", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Requires EPA TLS channel binding; clients that do not support EPA (pre-Vista) will fail WIA authentication.",
                    ApplyOps = [RegOp.SetDword(AuthKey, "EnableExtendedProtection", 1)],
                    RemoveOps = [RegOp.DeleteValue(AuthKey, "EnableExtendedProtection")],
                    DetectOps = [RegOp.CheckDword(AuthKey, "EnableExtendedProtection", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-disable-prompt-login",
                    Label = "Disable ADFS Prompt=Login Re-Authentication Bypass",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets DisablePromptLoginHandling=1 in the ADFS policy. Prevents ADFS from honouring the OAuth/OIDC prompt=login parameter which forces a fresh login regardless of existing SSO session. While useful for applications needing fresh credentials, this parameter can be abused by attackers to force users into repeated phishing-susceptible login flows. Disabling allows ADFS to enforce its own session management instead.",
                    Tags = ["adfs", "oauth", "prompt-login", "session", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Ignores prompt=login; applications that need forced re-auth must use access token expiry instead.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "DisablePromptLoginHandling", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "DisablePromptLoginHandling")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "DisablePromptLoginHandling", 1)],
                },
                new TweakDef
                {
                    Id = "adfspol-enable-audit-events",
                    Label = "Enable ADFS Security Audit Events",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets AuditFlags=1 in the ADFS policy. Instructs ADFS to write security audit events to the Windows Security event log for all federation authentication requests, token issuances, and extranet lockout events. ADFS audit events (Event IDs 1200, 1201, 411, 412) are essential for detecting password spray attacks, compromised account usage, and abnormal token issuance patterns in a federated identity environment.",
                    Tags = ["adfs", "audit", "events", "security-log", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enables ADFS audit events in the Security log; increases log volume proportional to federation traffic.",
                    ApplyOps = [RegOp.SetDword(AdfsKey, "AuditFlags", 1)],
                    RemoveOps = [RegOp.DeleteValue(AdfsKey, "AuditFlags")],
                    DetectOps = [RegOp.CheckDword(AdfsKey, "AuditFlags", 1)],
                },
            ];
    }

    // ── ActiveDirectoryServicesPolicy ──
    private static class _ActiveDirectoryServicesPolicy
    {
        private const string AdPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        private const string NetlogonKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "addsvc-restrict-ntlm-outbound-to-trusted-servers",
                    Label = "AD Services: Restrict Outbound NTLM to Domain-Trusted Servers Only",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets RestrictSendingNTLMTraffic=2 in the System policy (value 2 = Deny All, value 1 = Audit, value 0 = Allow). Restricts outbound NTLM authentication to only servers in the trusted exception list. NTLM credentials can be captured by rogue SMB or HTTP servers (e.g., via LLMNR/NBT-NS poisoning with Responder) — any outbound NTLM challenge-response that reaches an attacker's server provides an NTLM hash that can be relayed or cracked. Denying outbound NTLM to non-trusted-listed servers prevents credential leakage via NTLM to attacker-controlled resources. Start with value 1 (Audit) to identify NTLM usage before enforcing value 2.",
                    Tags = ["ntlm", "outbound-restriction", "responder", "ntlm-relay", "credential-capture"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Outbound NTLM is denied for all servers not in the NTLM exception list. This is high-impact — applications and services that use NTLM for server connections (file shares, proxy auth, legacy web apps) will fail unless added to the exception list. Deploy first with value 1 (Audit) and examine NTLM audit events (Event ID 4011/4013) to map usage before enforcing value 2.",
                    ApplyOps = [RegOp.SetDword(AdPolicyKey, "RestrictSendingNTLMTraffic", 2)],
                    RemoveOps = [RegOp.DeleteValue(AdPolicyKey, "RestrictSendingNTLMTraffic")],
                    DetectOps = [RegOp.CheckDword(AdPolicyKey, "RestrictSendingNTLMTraffic", 2)],
                },
                new TweakDef
                {
                    Id = "addsvc-enable-ntlm-audit-logging",
                    Label = "AD Services: Enable NTLM Outbound Authentication Audit Logging",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets AuditReceivingNTLMTraffic=2 in the System policy (value 2 = Enable auditing for all NTLM authentication). Enables auditing of all outbound NTLM authentication requests from this client. Audited events appear in the Security event log (Event ID 8001/8002/8003) and include the destination server, the NTLM authentication type, and the caller process. This is essential for mapping NTLM usage before deploying NTLM restriction policies — it allows the security team to identify which applications, services, and users are using NTLM so equivalent Kerberos or modern authentication alternatives can be configured before NTLM is denied.",
                    Tags = ["ntlm", "audit", "event-log", "siem", "authentication-map"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "All outbound NTLM authentications are logged to the Security event log. Generates events for every NTLM use — in Active Directory environments this may include Windows file share browsing, SMB connections, etc. Integrate with SIEM. Event volume may be high in environments with heavy NTLM usage.",
                    ApplyOps = [RegOp.SetDword(AdPolicyKey, "AuditReceivingNTLMTraffic", 2)],
                    RemoveOps = [RegOp.DeleteValue(AdPolicyKey, "AuditReceivingNTLMTraffic")],
                    DetectOps = [RegOp.CheckDword(AdPolicyKey, "AuditReceivingNTLMTraffic", 2)],
                },
                new TweakDef
                {
                    Id = "addsvc-require-ldap-server-integrity",
                    Label = "AD Services: Require DC-Side LDAP Server Signing (Integrity Check)",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets LDAPServerIntegrity=2 in the Netlogon\\Parameters hive (value 2 = Require signing). Requires LDAP signing on LDAP connections from clients to domain controllers. This is the server-side complement to the LDAP client signing requirement (LDAPClientIntegrity). When both client and server require signing, LDAP relay attacks that attempt to intercept and modify LDAP traffic are blocked at both endpoints. Without the server-side requirement, an attacker could spoof a DC with an unsigned LDAP server even if client policy sends signed requests.",
                    Tags = ["ldap", "server-signing", "ldap-relay", "netlogon", "integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "DC-side LDAP signing required as well as client-side. All LDAP connections from this client to DCs must use signed sessions — both sides enforce it. LDAP clients that send unsigned LDAP requests will receive an LDAP_UNWILLING_TO_PERFORM error. Applies to all LDAP on port 389; LDAPS on 636 is unaffected (TLS provides equivalent protection).",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "LDAPServerIntegrity", 2)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "LDAPServerIntegrity")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "LDAPServerIntegrity", 2)],
                },
                new TweakDef
                {
                    Id = "addsvc-set-dc-locator-force-rediscovery-600",
                    Label = "AD Services: Set DC Locator Force Re-Discovery Period to 600 Seconds",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets ForceRediscoveryInterval=600 in Netlogon\\Parameters (units: seconds). Sets the minimum interval between forced DC re-discovery events to 600 seconds (10 minutes). DC locator caches the preferred domain controller for each domain to avoid repeated DC lookup traffic. If the preferred DC becomes unavailable (patched, restarted, or taken down), the client should re-discover a DC within a reasonable time. Setting 600 seconds ensures that clients do not hold stale DC references for longer than 10 minutes when a DC failure event occurs, reducing authentication outage windows during DC failover events.",
                    Tags = ["netlogon", "dc-locator", "failover", "rediscovery", "resilience"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "DC preference re-discovery forced every 600 seconds maximum. Clients will check for a better DC up to every 10 minutes. May generate additional Netlogon DC locator traffic in large environments with many workstations. Appropriate for standard enterprise environments with 2+ DCs per site.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "ForceRediscoveryInterval", 600)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "ForceRediscoveryInterval")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "ForceRediscoveryInterval", 600)],
                },
            ];
    }

    // ── AdReplicationPolicy ──
    private static class _AdReplicationPolicy
    {
        private const string NtdsPolicyKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NTDS\Parameters";

        private const string SystemPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "adrep-require-ntds-replication-sign-seal",
                    Label = "AD Replication: Require NTDS Replication Traffic Sign-and-Seal",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets ReplicationSignAndSeal=1 in NTDS\\Parameters. Requires that Active Directory replication traffic between domain controllers is both signed (integrity-protected) and sealed (encrypted). AD replication carries all directory changes — new accounts, password updates, group membership changes, and computer policy settings. If replication traffic is unprotected, an attacker who can perform a man-in-the-middle attack on DC-to-DC traffic can inject or modify replication data, potentially escalating privileges by injecting account changes. Sign-and-seal ensures all replication traffic is authenticated and encrypted.",
                    Tags = ["ntds", "replication", "sign-seal", "dc-to-dc", "encryption"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "NTDS replication traffic requires sign-and-seal. Additional CPU overhead for encryption on DCs; negligible for modern hardware. Old DCs (pre-Windows 2000) cannot participate in signed/sealed replication — only relevant for very old mixed-mode domains.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "ReplicationSignAndSeal", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "ReplicationSignAndSeal")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "ReplicationSignAndSeal", 1)],
                },
                new TweakDef
                {
                    Id = "adrep-set-tomb-stone-lifetime-180days",
                    Label = "AD Replication: Set Active Directory Tombstone Lifetime to 180 Days",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets TombstoneLifetime=180 in NTDS\\Parameters (units: days). Sets the AD tombstone lifetime to 180 days. When an object is deleted in AD, it becomes a tombstone — a marker that propagates the deletion to all DCs before the tombstone is permanently removed. If a DC is offline longer than the tombstone lifetime, it must be forcibly re-joined to the domain (a USN rollback scenario) or reinstalled. 60 days (the old default) is insufficient for quarterly disaster recovery testing cycles. 180 days ensures that DCs recovered from quarterly backup snapshots are still within the tombstone window and can be safely re-brought online without forced rejoin.",
                    Tags = ["ntds", "tombstone", "backup-recovery", "deleted-objects", "replication"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Tombstone lifetime extended to 180 days. Deleted AD objects remain as tombstones for 180 days before permanent removal. Domain controllers that have been offline for more than 180 days will require forced rejoin. Increases the size of the deleted-objects container in the AD database.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "TombstoneLifetime", 180)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "TombstoneLifetime")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "TombstoneLifetime", 180)],
                },
                new TweakDef
                {
                    Id = "adrep-enable-strict-replication-consistency",
                    Label = "AD Replication: Enable Strict Replication Consistency Mode",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets Strict Replication Consistency=1 in NTDS\\Parameters. Enables strict replication consistency, which causes NTDS to disable replication from a replication partner that has an out-of-date replication topology (i.e., has missed more than MaxConsistencyCheckPercent of updates). Without strict consistency, AD will attempt to 'loose' replicate with lagged partners even if that results in duplicate GUID conflicts or lingering objects. In lingering object scenarios (DCs that have been offline past the tombstone lifetime), strict mode prevents corrupted data from being silently re-introduced into the directory by an out-of-date DC.",
                    Tags = ["ntds", "replication-consistency", "lingering-objects", "strict-mode", "integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Strict replication consistency enforced. DCs with excessive replication lag will have replication blocked until the issue is resolved. May generate replication errors (Event ID 1388) in environments with intermittent DC connectivity. Monitor NTDS events after enabling — address any replication lag issues before enforcing.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "Strict Replication Consistency", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "Strict Replication Consistency")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "Strict Replication Consistency", 1)],
                },
                new TweakDef
                {
                    Id = "adrep-enable-dns-consistency-check",
                    Label = "AD Replication: Enable DNS Consistency Check During Promotion",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets DnsAvoidRegisterRecords=0 in NTDS\\Parameters. Ensures that all required DNS records for the domain controller are registered during or after promotion, and that the DNS consistency check is not bypassed. DC promotion attempts with unresolvable DNS names or misconfigured DNS zones that bypass the DNS check can result in DCs that are partially functional but not properly reachable by other DCs — leading to intermittent replication failures that are hard to diagnose. Ensuring DNS consistency is enforced catches DNS misconfigurations at promotion time rather than as production replication failures.",
                    Tags = ["ntds", "dns", "consistency-check", "promotion", "dc-registration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "DNS record registration for this DC is enabled (DnsAvoidRegisterRecords=0). DC registers all required DNS SRV and A records. In split-DNS environments, verify the DC can register records in both internal and external DNS zones as appropriate.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "DnsAvoidRegisterRecords", 0)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "DnsAvoidRegisterRecords")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "DnsAvoidRegisterRecords", 0)],
                },
                new TweakDef
                {
                    Id = "adrep-restrict-ad-single-object-recovery",
                    Label = "AD Replication: Enable AD Recycle Bin (Prevent Immediate Object Purge)",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets EnabledScopes=1 in NTDS\\Parameters. Enables the Active Directory Recycle Bin feature flag on this DC. The AD Recycle Bin preserves deleted objects (with all attributes including group memberships) for the deleted-object lifetime (default 180 days), making it possible to restore accidentally deleted user accounts, OUs, or groups without authoritative restore from backup. Without the Recycle Bin, deleted objects immediately lose most attributes and recovery requires authoritative NTDS restore or backup-based object recovery. This is a forest-level feature that must be enabled via PowerShell on the Schema Master (Enable-ADOptionalFeature) — this policy flag enables the local DC to participate.",
                    Tags = ["ntds", "recycle-bin", "object-recovery", "deleted-objects", "resilience"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Local DC participates in AD Recycle Bin. Requires the AD Recycle Bin optional feature to be enabled at the forest level first (Enable-ADOptionalFeature on Schema Master). Once enabled, cannot be reversed without forest recovery. Increases NTDS.dit database size due to full attribute retention for deleted objects.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "EnabledScopes", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "EnabledScopes")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "EnabledScopes", 1)],
                },
                new TweakDef
                {
                    Id = "adrep-set-max-replication-failures-5",
                    Label = "AD Replication: Alert on More Than 5 Consecutive Replication Failures",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets MaxConsistencyCheckPercent=5 in NTDS\\Parameters. Sets the threshold at which consecutive replication failures from a partner trigger a consistency check alert to 5 failures. By default, NTDS tolerates a high number of consecutive replication failures before logging a critical event or taking action. Setting a lower threshold ensures that replication health degradation is detected and reported early — critical for catching incidents where an attacker disrupts replication to prevent domain-wide propagation of security policy changes or account lockouts.",
                    Tags = ["ntds", "replication-failure", "alerting", "consistency", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Replication failure alerting triggers after 5 consecutive failures from a single replication partner. Event ID 1308 logged. In environments with intermittent network connectivity between sites, this threshold may generate false-positive events. Monitor and tune based on the expected replication reliability in your site-link topology.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "MaxConsistencyCheckPercent", 5)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "MaxConsistencyCheckPercent")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "MaxConsistencyCheckPercent", 5)],
                },
                new TweakDef
                {
                    Id = "adrep-enable-ad-audit-log-policy-access",
                    Label = "AD Replication: Enable Audit of AD DS Access Policy Operations",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets AuditPolicySubcategory=1 in NTDS\\Parameters. Enables auditing of Active Directory Service access subcategory events. These events include access to sensitive AD objects (krbtgt account reads, Domain Admins group modifications, Schema changes, replication metadata access), NTDS database file access, and NTDS parameter changes. Without this audit, an attacker who accesses sensitive AD objects (e.g., DCSync — requesting replication metadata from a DC to extract all password hashes) leaves no event log trail. With audit enabled, DCSync attempts generate replication audit events (EventID 4662, 4928) that can be detected by SIEM.",
                    Tags = ["ntds", "audit", "dcsync", "replication-access", "event-4662"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "AD DS access auditing enabled. DCSync-like replication requests (DS-Replication-Get-Changes-All) will generate Event ID 4662 in the Security log with GUID of the GUID accessed. SOC can detect DCSync attacks in real-time. May generate significant event log volume in large forests with active AD replication.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "AuditPolicySubcategory", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "AuditPolicySubcategory")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "AuditPolicySubcategory", 1)],
                },
                new TweakDef
                {
                    Id = "adrep-enable-ntds-encrypted-communication",
                    Label = "AD Replication: Enable NTDS RPC Encrypted Communication Channel",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets EncryptRpcCommunication=1 in NTDS\\Parameters. Enables RPC encryption for AD replication traffic between domain controllers. AD DS replication uses Microsoft RPC over TCP for inter-DC communication. Enabling RPC encryption ensures that the payload of replication packets (directory object changes, attribute updates, password hash data) is encrypted in transit between DCs. This is layered protection on top of sign-and-seal — even if sign-and-seal at the NTDS layer is bypassed, the RPC transport layer encryption provides an additional barrier.",
                    Tags = ["ntds", "rpc", "encryption", "replication", "transport-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "RPC encryption enabled for NTDS replication traffic. Additional CPU overhead from AES encryption of replication packets. On modern DC hardware (Xeon, EPYC) AES-NI instructions keep overhead below 1%. Verify no inter-DC firewall rules block the dynamic RPC port range (49152–65535) after enabling.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "EncryptRpcCommunication", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "EncryptRpcCommunication")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "EncryptRpcCommunication", 1)],
                },
                new TweakDef
                {
                    Id = "adrep-set-ad-query-policy-max-objects-10000",
                    Label = "AD Replication: Cap AD DS Directory Queries to 10000 Objects Per Operation",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets MaxTempTableSize=10000 in NTDS\\Parameters (units: objects). Limits the number of objects returned in a single AD query operation to 10,000. Unrestricted AD queries can consume significant DC CPU and memory — an attacker with LDAP read access who issues an unbounded subtree search against the entire domain partition can cause a DC denial-of-service by forcing it to process a millions-of-results query. Setting a per-query object cap ensures that even large LDAP clients must paginate, distributing the query load over time and preventing single-query DC saturation.",
                    Tags = ["ntds", "query-limit", "dos-mitigation", "ldap", "pagination"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "AD directory queries capped at 10,000 objects per operation. Clients requiring more than 10,000 results must use LDAP paged results control. Modern enterprise applications (Azure AD Connect, ADMT, Quest Migration Manager) handle paging natively. Custom scripts using unbounded LDAP searches must be updated.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "MaxTempTableSize", 10000)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "MaxTempTableSize")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "MaxTempTableSize", 10000)],
                },
                new TweakDef
                {
                    Id = "adrep-enable-rid-master-audit",
                    Label = "AD Replication: Enable RID (Relative Identifier) Pool Allocation Audit",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets AuditRidAllocation=1 in NTDS\\Parameters. Enables auditing of RID (Relative Identifier) pool allocation requests. Domain controllers request blocks of RIDs from the RID Master FSMO role to assign unique object SIDs when creating new AD objects. An unusually high rate of RID pool requests from a specific DC (e.g., thousands of allocations per day) may indicate automated object creation — a technique used by ransomware operators to create new privileged accounts en masse or by red teams performing domain object flooding. Auditing RID allocation enables detection of anomalous object creation bursts.",
                    Tags = ["ntds", "rid", "rid-master", "object-creation", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "RID pool allocation requests are audited. Generates Event ID 16657 in Directory Service log when a DC requests a new RID pool. Baseline normal RID allocation frequency for your environment (typically 1 pool every few months per active DC). Alert on abnormal frequency as potential ransomware or red-team indicator.",
                    ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "AuditRidAllocation", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "AuditRidAllocation")],
                    DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "AuditRidAllocation", 1)],
                },
            ];
    }

    // ── AzureVirtualDesktopPolicy ──
    private static class _AzureVirtualDesktopPolicy
    {
        private const string TsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

        private const string AvdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services\Azure Virtual Desktop";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "avd-enable-watermarking",
                    Label = "AVD: Enable Screen Watermarking for Session Hosts",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets EnableWatermarking=1 and WatermarkingHeightFactor/WidthFactor. Overlays a semi-transparent QR code on AVD session screens that encodes the user's UPN and session identifier. This watermark is user-invisible during normal work but visible in screenshots and screen captures. Watermarking is essential for data loss investigations and insider threat deterrence in environments handling sensitive or regulated data.",
                    Tags = ["avd", "watermarking", "dlp", "session", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Transparent watermark; no user impact. QR code is visible in screen captures which may affect screen-sharing in training.",
                    ApplyOps = [RegOp.SetDword(TsKey, "EnableWatermarking", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "EnableWatermarking")],
                    DetectOps = [RegOp.CheckDword(TsKey, "EnableWatermarking", 1)],
                },
                new TweakDef
                {
                    Id = "avd-disable-drive-redirect",
                    Label = "AVD: Disable Drive Redirection in Sessions",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets fDisableCdm=1. Prevents client-side drives (USB sticks, local hard drives, network shares) from being mounted in AVD sessions. Drive redirection is exploited for both data exfiltration (copying from session to external media) and malware delivery (running executables from a USB drive in the session). Removing drive redirection is a standard DLP and malware containment control in supervised VDI environments.",
                    Tags = ["avd", "drive-redirect", "usb", "dlp", "malware"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Removes session access to client drives. Users cannot access USB media or local files from within the AVD session.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisableCdm", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableCdm")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisableCdm", 1)],
                },
                new TweakDef
                {
                    Id = "avd-idle-disconnect-30min",
                    Label = "AVD: Disconnect Idle Sessions After 30 Minutes",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets MaxIdleTime=1800000 (30 minutes in milliseconds). Automatically disconnects AVD sessions that have been idle for 30 minutes. Idle sessions consume Azure compute costs and create an unattended-session security risk where unlocked sessions could be accessed by physical access to an unattended client. Auto-disconnect after 30 minutes is the standard enterprise baseline for cost and security management of AVD session hosts.",
                    Tags = ["avd", "idle", "session-management", "cost", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users with unsaved work may lose state if idle for 30 minutes. Pair with auto-save policies.",
                    ApplyOps = [RegOp.SetDword(TsKey, "MaxIdleTime", 1800000)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "MaxIdleTime")],
                    DetectOps = [RegOp.CheckDword(TsKey, "MaxIdleTime", 1800000)],
                },
                new TweakDef
                {
                    Id = "avd-enable-screen-capture-protection",
                    Label = "AVD: Enable Screen Capture Protection (DRM-Level)",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets fEnableScreenCaptureProtect=1. Enables AVD screen capture protection, which uses DRM-style OS hooks to prevent the AVD session content from being captured by screenshots, screen recording software, or GPU frame capture tools on the client side. The session content appears as a black region in any screen capture. Essential for protecting classified information displays, financial data, and healthcare PHI from accidental or intentional screen capture exfiltration.",
                    Tags = ["avd", "screen-capture", "dlp", "drm", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Session content is blackened in all screen captures on the client. Training recordings and accessibility tools that capture the screen will not see session content.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fEnableScreenCaptureProtect", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fEnableScreenCaptureProtect")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fEnableScreenCaptureProtect", 1)],
                },
                new TweakDef
                {
                    Id = "avd-enable-private-mode",
                    Label = "AVD: Enable Private Mode for Session Hosts",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets EnablePrivateMode=1. Activates AVD Private Mode which restricts session actions to reduce data leakage risk: disables local clipboard, file transfers, printing to local printers, and local drive access in a single policy. Private Mode is designed for shared/kiosk session hosts in sensitive environments where multiple users share the same session host profile. Equivalent to enabling fDisableClip + fDisableCdm + printer restrictions together.",
                    Tags = ["avd", "private-mode", "kiosk", "dlp", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Restricts all peripheral redirection. Not suitable for productivity use-cases requiring local file access or printing.",
                    ApplyOps = [RegOp.SetDword(TsKey, "EnablePrivateMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "EnablePrivateMode")],
                    DetectOps = [RegOp.CheckDword(TsKey, "EnablePrivateMode", 1)],
                },
                new TweakDef
                {
                    Id = "avd-set-rdp-security-layer",
                    Label = "AVD: Enforce TLS 1.2+ for RDP Transport Layer",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets SecurityLayer=2 (TLS). Forces the Remote Desktop Protocol transport layer to use SSL/TLS 1.2 or later for all connections to AVD session hosts. Value 0 = RDP legacy (cleartext-vulnerable), value 1 = negotiate (downgrade possible), value 2 = TLS required. In Azure, the network path is encrypted at the Azure backbone level; however, enforcing TLS at the RDP layer provides defence-in-depth and satisfies compliance requirements for encrypted-in-transit data.",
                    Tags = ["avd", "rdp", "tls", "encryption", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires TLS; breaks connections from very old RDP clients that cannot negotiate TLS. All modern clients support TLS.",
                    ApplyOps = [RegOp.SetDword(TsKey, "SecurityLayer", 2)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "SecurityLayer")],
                    DetectOps = [RegOp.CheckDword(TsKey, "SecurityLayer", 2)],
                },
                new TweakDef
                {
                    Id = "avd-require-nla",
                    Label = "AVD: Require Network Level Authentication for Sessions",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets UserAuthentication=1. Requires Network Level Authentication (NLA) before establishing an RDP session to the AVD host. NLA authenticates the user before allocating session resources, preventing unauthenticated users from reaching the Windows login screen and mounting DoS attacks by opening many half-authenticated sessions. AVD natively enforces Azure AD authentication; this setting adds an additional OS-level NLA gate.",
                    Tags = ["avd", "nla", "authentication", "rdp", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Requires NLA-capable RDP clients (all modern clients support NLA). Very old RDP clients may not connect.",
                    ApplyOps = [RegOp.SetDword(TsKey, "UserAuthentication", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "UserAuthentication")],
                    DetectOps = [RegOp.CheckDword(TsKey, "UserAuthentication", 1)],
                },
                new TweakDef
                {
                    Id = "avd-limit-session-connections",
                    Label = "AVD: Limit Users to a Single Active Session",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets fSingleSessionPerUser=1. Restricts each user to a single simultaneous AVD session across all host pool machines. Without this limit, a user can open multiple sessions (e.g., from multiple devices simultaneously), multiplying their compute cost and creating multiple unmanaged session states. Single-session enforcement reduces Azure compute costs proportionally to the number of multi-device users and simplifies session management.",
                    Tags = ["avd", "session-limit", "cost", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Single session per user. Prevents opening the same session from multiple client devices simultaneously.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fSingleSessionPerUser", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fSingleSessionPerUser")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fSingleSessionPerUser", 1)],
                },
                new TweakDef
                {
                    Id = "avd-enable-shortpath-udp",
                    Label = "AVD: Enable UDP ShortPath for Optimized Network Performance",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets fClientShortPathEndpointEnabled=1. Activates Azure Virtual Desktop UDP ShortPath, which establishes direct UDP-based transport between the AVD client and session host instead of routing all traffic through the Azure gateway TCP relay. UDP ShortPath reduces round-trip latency from 50–200 ms (TCP relay) to near-direct network latency, dramatically improving display responsiveness and Teams/audio quality in AVD sessions.",
                    Tags = ["avd", "shortpath", "udp", "latency", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Requires UDP 65330 outbound from client to be open on the firewall. Check network policy before enabling.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fClientShortPathEndpointEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fClientShortPathEndpointEnabled")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fClientShortPathEndpointEnabled", 1)],
                },
            ];
    }

    // ── CloudPcWindows365Policy ──
    private static class _CloudPcWindows365Policy
    {
        private const string CloudPcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\CloudPC";

        private const string TsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cloudpc-enable-udp-shortpath",
                    Label = "Cloud PC: Enable UDP ShortPath for Low-Latency Transport",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets fUdpRedirectorEnabled=1 under Terminal Services. Enables UDP-based RDP traffic for Windows 365 Cloud PCs, bypassing the TCP relay in Azure and creating a near-direct UDP path from the Windows 365 client to the Cloud PC. UDP ShortPath typically reduces RDP latency by 40–80 ms for geographically proximate users, significantly improving the responsiveness of interactive applications and video playback inside a Cloud PC session.",
                    Tags = ["cloudpc", "windows-365", "udp", "performance", "shortpath"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Requires UDP 3478/65330 outbound from client to Azure. Check firewall configuration before enabling.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fUdpRedirectorEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fUdpRedirectorEnabled")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fUdpRedirectorEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-enable-teams-optimization",
                    Label = "Cloud PC: Enable Teams AV Optimization (Media Redirection)",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets TeamsMeetingUnmuteOnEntry=0 and related Teams policy keys. Activates Teams audio/video media optimization for Windows 365 Cloud PCs, which redirects media processing from the Cloud PC CPU to the local client device. Without media optimization, Teams calls are processed server-side, consuming Cloud PC vCPU and causing high latency. With optimization, HD video calls run at near-native quality on the client while the Cloud PC CPU overhead drops by 70–90%.",
                    Tags = ["cloudpc", "teams", "media-redirect", "av", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires Teams client v1.4+ and Windows App/MSTSC v1.2.3004+. Older clients fall back to server-side processing without error.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fEnableTeamsHdxVideoOptimization", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fEnableTeamsHdxVideoOptimization")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fEnableTeamsHdxVideoOptimization", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-disable-printer-redirect",
                    Label = "Cloud PC: Disable Client Printer Redirection",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets fDisablePrnt=1. Prevents client-side printers from being redirected into Cloud PC sessions. Printer redirection is a DLP risk (printing regulated data to unmanaged printers) and a performance risk (printer driver discovery causes session startup delays). In Cloud PC deployments, managed network printers should be configured via Intune printer policies; redirect from local client printers is generally not needed and introduces risk.",
                    Tags = ["cloudpc", "printer", "redirect", "dlp", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users cannot print from a Cloud PC to their local/USB printer. Managed print servers via Intune are unaffected.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisablePrnt", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisablePrnt")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisablePrnt", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-set-display-depth-32bit",
                    Label = "Cloud PC: Set Remote Display to 32-Bit Color",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets MaxColorDepth=4 (32-bit). Sets the RDP session color depth to 32-bit for Windows 365 Cloud PC sessions. This is the maximum color depth supported by the RDP protocol. Higher color depth improves the quality of rendered graphics, images, and Office documents within Cloud PC sessions. Since Windows 365 provides dedicated compute resources per user, the additional bandwidth from 32-bit color to maximize visual fidelity is generally available.",
                    Tags = ["cloudpc", "display", "color-depth", "quality"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Higher color depth increases RDP bandwidth. Not recommended for poor network connections (<10 Mbps).",
                    ApplyOps = [RegOp.SetDword(TsKey, "MaxColorDepth", 4)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "MaxColorDepth")],
                    DetectOps = [RegOp.CheckDword(TsKey, "MaxColorDepth", 4)],
                },
                new TweakDef
                {
                    Id = "cloudpc-enable-gpu-redirect",
                    Label = "Cloud PC: Enable GPU RemoteFX Virtual GPU",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets AVC444ModePreferred=1. Enables AVC444 (H.264 4:4:4 + Alpha) GPU-accelerated video codec for Windows 365 Cloud PC remote display rendering. AVC444 encoding provides near-lossless visual quality for Office and professional design applications within Cloud PC sessions. Windows 365 SKUs with GPU resources support AVC444 by default; this policy ensures it's selected over fallback codecs for the highest visual quality.",
                    Tags = ["cloudpc", "gpu", "avc444", "gpu-redirect", "display"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AVC444 requires Windows 365 GPU-enabled SKU. On CPU-only SKUs this setting is ignored by the display subsystem.",
                    ApplyOps = [RegOp.SetDword(TsKey, "AVC444ModePreferred", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "AVC444ModePreferred")],
                    DetectOps = [RegOp.CheckDword(TsKey, "AVC444ModePreferred", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-lock-session-on-disconnect",
                    Label = "Cloud PC: Lock Screen Immediately on Session Disconnect",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets fPromptForPassword=1. Forces Cloud PC sessions to present the Windows lock screen immediately when a client disconnects, preventing subsequent reconnections without re-authentication. Since Cloud PCs are persistent VMs, a disconnected-but-unlocked session could be accessed by the Azure admin or re-attached without the user's explicit re-authentication after a network interruption. Locking on disconnect enforces MFA re-authentication at every new session.",
                    Tags = ["cloudpc", "lock-screen", "authentication", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires MFA re-authentication on every reconnect. Slightly increases session resume time for Teams and app continuity.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fPromptForPassword", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fPromptForPassword")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fPromptForPassword", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-session-time-limit-8h",
                    Label = "Cloud PC: Set Maximum Active Session Duration to 8 Hours",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets MaxConnectionTime=28800000 (8 hours in ms). Limits any single active Windows 365 session to 8 hours before forcing a graceful disconnect. Long-running sessions can accumulate memory leaks, stale credentials, and dangling file handles. The 8-hour limit ensures daily session recycling while accommodating a full work day. Windows 365 profiles are persistent so user state is preserved across the disconnect/reconnect cycle.",
                    Tags = ["cloudpc", "session-limit", "time-limit", "maintenance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Users are gracefully disconnected after 8 hours. Unsaved work may be lost if auto-save is not configured. Windows gives a warning before disconnect.",
                    ApplyOps = [RegOp.SetDword(TsKey, "MaxConnectionTime", 28800000)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "MaxConnectionTime")],
                    DetectOps = [RegOp.CheckDword(TsKey, "MaxConnectionTime", 28800000)],
                },
                new TweakDef
                {
                    Id = "cloudpc-disable-audio-record-redirect",
                    Label = "Cloud PC: Disable Microphone Redirection in Sessions",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets fDisableAudioCapture=1. Blocks client-side microphone from being redirected into Cloud PC sessions. Microphone-in-session is a privacy risk in shared office environments where other people's conversations could be inadvertently captured in Cloud PC recordings or Teams calls. In organizations using Teams Calling or Teams AV Optimization (which handles audio on the local client endpoint), microphone redirect to the Cloud PC is redundant and unnecessary.",
                    Tags = ["cloudpc", "microphone", "audio", "privacy", "redirect"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Blocks in-session microphone access. Users using Teams AV Optimization are unaffected as audio is processed locally.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisableAudioCapture", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableAudioCapture")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisableAudioCapture", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-enable-display-quality-max",
                    Label = "Cloud PC: Set Maximum Visual Quality Level for Display",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets VisualQuality=3 (medium-high). Sets the Cloud PC RDP display quality to the highest persistent level. Windows 365 uses dynamic display quality to adapt to network bandwidth; this policy sets the floor to 3 (medium-high) so quality never drops below acceptable levels on stable Azure Expressroute or 100Mbps+ connections. Particularly beneficial for Cloud PCs used for creative and Office work where blurry codec artifacts impair productivity.",
                    Tags = ["cloudpc", "display-quality", "rdp", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Higher baseline quality uses more bandwidth (~5–10 Mbps sustained). Not recommended for Cloud PCs accessed over mobile/4G connections.",
                    ApplyOps = [RegOp.SetDword(TsKey, "VisualQuality", 3)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "VisualQuality")],
                    DetectOps = [RegOp.CheckDword(TsKey, "VisualQuality", 3)],
                },
                new TweakDef
                {
                    Id = "cloudpc-disable-device-redirect",
                    Label = "Cloud PC: Disable PnP Device Redirection into Sessions",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets fDisablePNPRedir=1. Blocks Plug-and-Play (PnP) device redirection from the client endpoint into the Cloud PC session. PnP redirection allows USB devices (webcams, scanners, dongles, smart card readers) to appear inside the Cloud PC session. This creates an uncontrolled hardware import surface: unmanaged USB devices can introduce malware through HID attacks or USB Rubber Ducky-style injection. Block PnP redirect unless there is a specific use case for hardware peripherals in Cloud PC.",
                    Tags = ["cloudpc", "usb", "pnp", "device-redirect", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "USB/PnP devices are not available inside Cloud PC sessions. Smart card readers for local authentication are unaffected if using NLA.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisablePNPRedir", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisablePNPRedir")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisablePNPRedir", 1)],
                },
            ];
    }

    // ── ConfigurationManagerPolicy ──
    private static class _ConfigurationManagerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\ConfigurationManager";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "confmgr-require-code-signing-for-scripts",
                    Label = "ConfigMgr: Require Script Code Signing for All Client-Side Script Execution",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets RequireScriptCodeSigning=1 in ConfigurationManager policy. Requires that any script (PowerShell, VBScript, JScript) deployed through the Configuration Manager client for task sequences or application deployment must be digitally signed by a certificate trusted by the client's root store before execution. "
                        + "Configuration Manager script execution is a primary lateral movement vector in enterprise environments. A compromised management server or a rogue admin with deployment rights can push arbitrary scripts to all managed clients. Without code signing enforcement, any script pushed through ConfigMgr is executed verbatim. Requiring script code signing ensures only scripts signed by the enterprise PKI certificate authority are executed.",
                    Tags = ["configmgr", "sccm", "scripts", "code-signing", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Unsigned ConfigMgr scripts blocked; all deployment scripts must be signed by enterprise PKI before execution.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireScriptCodeSigning", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireScriptCodeSigning")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireScriptCodeSigning", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-enable-client-audit-logging",
                    Label = "ConfigMgr: Enable Comprehensive Audit Logging for Client Agent Operations",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets EnableClientAuditLogging=1 in ConfigurationManager policy. Enables detailed audit logging in the Configuration Manager client agent, causing all deployment operations (software installs, uninstalls, state machine transitions, inventory collection, policy downloads) to be recorded in the Security event log in addition to the standard ccmsetup.log files. "
                        + "The default ConfigMgr client logging writes verbose detail to log files under C:\\Windows\\CCM\\Logs\\ but does not generate Security event log entries auditable by a SIEM. With audit logging enabled, Security events are generated for every ConfigMgr operation, enabling correlation with Active Directory logon events, PowerShell execution events, and process creation events during incident investigations. This enables detection of ConfigMgr-based lateral movement.",
                    Tags = ["configmgr", "sccm", "audit-log", "security", "siem"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "ConfigMgr operations generate Security event log entries; SIEM can correlate ConfigMgr deployments with suspicious activities.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableClientAuditLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableClientAuditLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableClientAuditLogging", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-require-ssl-for-management-point",
                    Label = "ConfigMgr: Require HTTPS/PKI for All Client-to–Management Point Communication",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets RequireSSLForManagementPoint=1 in ConfigurationManager policy. Enforces that the ConfigMgr client uses HTTPS with PKI client certificates for all communication with the Management Point, Distribution Point, and other site roles, blocking fallback to HTTP. "
                        + "Configuration Manager in HTTP mode transmits deployment data, credentials used for network access accounts, and package download URLs in plaintext. A network attacker on the same segment as a ConfigMgr client can intercept policy downloads and inject malicious package locations. Enforcing HTTPS-only communication requires PKI infrastructure but prevents man-in-the-middle interception of ConfigMgr policy and deployment content.",
                    Tags = ["configmgr", "sccm", "https", "pki", "ssl", "management-point"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "ConfigMgr client requires HTTPS; HTTP communication with management point blocked. Requires PKI client certificates to be enrolled.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSSLForManagementPoint", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSSLForManagementPoint")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSSLForManagementPoint", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-disable-software-center-user-portal",
                    Label = "ConfigMgr: Disable Software Center User-Initiated Install Portal",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets DisableSoftwareCenterPortal=1 in ConfigurationManager policy. Disables the Software Center user-facing portal through which end users can browse 'Available' software and initiate their own optional application installs. Only 'Required' deployments that are pushed and mandatory remain active; the Software Center self-service catalog is removed from the user's Start menu. "
                        + "The Software Center self-service portal is appropriate for general enterprise endpoints where end users should be able to install productivity tools. In high-security or locked-down environments (healthcare workstations, kiosk terminals, PCI-scope machines), allowing users to install any software from the catalog — even admin-approved software — introduces unnecessary attack surface expansion. Application installs should be exclusively IT-admin-driven deployments.",
                    Tags = ["configmgr", "sccm", "software-center", "lockdown", "user-install"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Software Center self-service portal disabled; only mandatory/required ConfigMgr deployments are presented to users.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSoftwareCenterPortal", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSoftwareCenterPortal")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSoftwareCenterPortal", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-disable-client-auto-upgrade",
                    Label = "ConfigMgr: Disable Automatic ConfigMgr Client Agent Auto-Upgrade",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets DisableAutoUpgrade=1 in ConfigurationManager policy. Prevents the ConfigMgr client agent from automatically upgrading itself when the site server is running a newer version of the ConfigMgr client, requiring IT to explicitly push client upgrades through a managed deployment. "
                        + "The ConfigMgr client auto-upgrade mechanism upgrades the client agent on all managed endpoints automatically when the Primary Site server is upgraded. While convenient, this means that upgrading the site server triggers an automatic, uncontrolled rollout to thousands of endpoints simultaneously, with no staging, no pilot group, and no rollback capability. A buggy client version pushed by auto-upgrade to all endpoints can simultaneously disrupt the management channel for the entire estate.",
                    Tags = ["configmgr", "sccm", "client-upgrade", "rollout", "change-control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ConfigMgr client auto-upgrade disabled; client upgrades require explicit IT-managed deployment packages.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoUpgrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoUpgrade")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoUpgrade", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-require-admin-for-user-policy-execution",
                    Label = "ConfigMgr: Require Administrative Approval Before User-Targeted Policy Execution",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets RequireAdminApprovalForUserPolicy=1 in ConfigurationManager policy. Requires that user-targeted configuration baseline deployments (policies applied to users, not computers) receive explicit IT admin approval in the ConfigMgr console before the client agent executes them on the endpoint. "
                        + "In some ConfigMgr configurations, user-targeted configuration baselines can be deployed to security groups by less-privileged admins (Help Desk, Application Deployment staff) without requiring full ConfigMgr infrastructure admin privileges. If those baselines include scripts or registry modifications, a Help Desk operator with deployment rights could push policy changes to all users in their management scope. Requiring admin approval creates a second-factor approval gate for user-targeted policy execution.",
                    Tags = ["configmgr", "sccm", "user-policy", "admin-approval", "separation-of-duties"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "User-targeted ConfigMgr configuration baselines require admin approval before execution; prevents unauthorised user-policy deployment.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminApprovalForUserPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminApprovalForUserPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminApprovalForUserPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-cap-content-cache-size-5gb",
                    Label = "ConfigMgr: Cap Client Content Cache Size at 5 GB",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets MaxContentCacheSizeGB=5 in ConfigurationManager policy. Limits the ConfigMgr client content cache (the local disk cache where the client pre-downloads content from Distribution Points before installation) to a maximum of 5 GB, preventing the cache from consuming disk space beyond this limit. "
                        + "By default, the ConfigMgr client content cache can grow to 10% of total disk size. On large-disk endpoints (1 TB drives), this allows a 100 GB cache. In environments with thin-provisioned storage (VDI, laptop SSDs) or low-disk-space scenarios, an unbounded cache can fill available disk space, causing operating system failures or application performance issues. A 5 GB cap is sufficient for most enterprise software deployments while protecting disk space.",
                    Tags = ["configmgr", "sccm", "cache", "disk-space", "storage"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "ConfigMgr client content cache capped at 5 GB; disk consumption controlled for thin-provisioned storage environments.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxContentCacheSizeGB", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxContentCacheSizeGB")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxContentCacheSizeGB", 5)],
                },
                new TweakDef
                {
                    Id = "confmgr-disable-client-notification-feature",
                    Label = "ConfigMgr: Disable ConfigMgr Client Notification Channel",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets DisableClientNotification=1 in ConfigurationManager policy. Disables the ConfigMgr client notification channel — a push mechanism that allows the site server to send fast-path notifications to clients to immediately trigger a policy evaluation or initiate re-inventory without waiting for the standard polling interval. "
                        + "The client notification channel uses a persistent TCP connection from the ConfigMgr client to the Management Point. While this enables near-real-time policy deployment, it also means a compromised Management Point has an active connection to every managed client and can trigger immediate policy execution on all clients simultaneously. In environments where the threat model includes Management Point compromise, disabling the notification channel forces deployments to use the standard polling schedule which is easier to audit and rate-limit.",
                    Tags = ["configmgr", "sccm", "client-notification", "tcp", "management-point"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote =
                        "ConfigMgr push notifications disabled; policy deployment uses scheduled polling intervals instead of near-real-time push.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClientNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClientNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClientNotification", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-enable-tamper-protection",
                    Label = "ConfigMgr: Enable Tamper Protection for ConfigMgr Client Agent",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets EnableClientTamperProtection=1 in ConfigurationManager policy. Enables the ConfigMgr client tamper protection mechanism, which prevents standard users and non-admin processes from stopping or disabling the CCMExec service, deleting the CCM client registry keys, or uninstalling the ConfigMgr client agent. "
                        + "Attackers that gain code execution on an endpoint as a standard user or as a low-privilege process will attempt to disable security tools and management agents before proceeding with lateral movement or data exfiltration. The ConfigMgr client agent is a high-value target for disablement because it delivers security baselines, patches, and malware detection policies. Tamper protection prevents the CCMExec service from being stopped by non-admin processes.",
                    Tags = ["configmgr", "sccm", "tamper-protection", "service-protection", "ccmexec"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "ConfigMgr client tamper protection active; CCMExec service cannot be stopped by non-admin processes or scripts.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableClientTamperProtection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableClientTamperProtection")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableClientTamperProtection", 1)],
                },
                new TweakDef
                {
                    Id = "confmgr-block-network-access-account-caching",
                    Label = "ConfigMgr: Block Caching of Network Access Account Credentials on Client Disk",
                    Category = "System — Adfs Federation",
                    Description =
                        "Sets DisableNAACredentialCaching=1 in ConfigurationManager policy. Prevents the ConfigMgr client from caching the Network Access Account (NAA) credentials — the service account used to authenticate with Distribution Points — in the local DPAPI credential store on the client disk. "
                        + "The ConfigMgr Network Access Account is a domain service account whose credentials are distributed to all ConfigMgr-managed clients to allow content download from Distribution Points. By default, these credentials are cached on disk using DPAPI. On a compromised endpoint, an attacker can extract the NAA credentials using tools that decrypt DPAPI-protected data (accessible to SYSTEM-level processes) and then use those credentials to authenticate to internal servers as the NAA service account, often a domain user with broad read access.",
                    Tags = ["configmgr", "sccm", "naa", "credentials", "dpapi", "credential-theft"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "NAA credential caching disabled; ConfigMgr service account credentials are not stored on client disk after each policy download.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableNAACredentialCaching", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableNAACredentialCaching")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableNAACredentialCaching", 1)],
                },
            ];
    }

    // ── DeploymentServicesPolicy ──
    private static class _DeploymentServicesPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "depsvc-disable-multicast",
                Label = "Disable WDS Multicast Image Transfer",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Windows Deployment Services multicast transfers operating system images to multiple clients simultaneously using multicast UDP network traffic. Disabling multicast transmission prevents the generation of multicast network traffic from WDS servers that can impact other network devices. WDS multicast operates on well-known multicast addresses that require multicast routing infrastructure to function correctly. In environments without proper multicast routing, WDS multicast traffic can generate excessive broadcast traffic or fail to route correctly. Disabling multicast forces WDS to use unicast image transfers which are simpler to troubleshoot and less likely to cause network issues. Organizations that have properly configured multicast infrastructure may enable multicast for efficient simultaneous OS deployment to large numbers of clients.",
                Tags = ["wds", "multicast", "deployment", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMulticast", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMulticast")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMulticast", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-disable-pxe-response",
                Label = "Disable WDS PXE Boot Response",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "PXE boot allows network endpoints to boot from a WDS server and receive operating system images instead of booting from local storage. Disabling WDS PXE response prevents unauthorized network boot attempts from connecting to the WDS server. Unauthorized PXE boots could allow an attacker to boot a system into a WDS-provided environment bypassing local security controls. WDS PXE services should only respond to pre-authorized clients and systems requiring legitimate OS deployment. PXE boot in production environments represents an attack vector where adversaries can boot systems into controlled environments to capture credentials or bypass endpoint security. Organizations should restrict PXE responses to pre-authorized MAC addresses when WDS PXE is required for legitimate deployment.",
                Tags = ["wds", "pxe", "boot", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePxeResponse", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePxeResponse")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePxeResponse", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-require-user-authorization",
                Label = "Require User Authorization for WDS Network Boot",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "WDS network boot without user authorization can process deployment requests without verifying that the requesting system and user are authorized. Requiring user authorization for WDS deployments ensures that only authenticated and authorized users can initiate OS deployments through WDS. Unauthorized WDS deployments could replace a system's operating system with an attacker-controlled image bypassing all security controls. WDS authorization requirements prevent automated attackers from deploying compromised operating system images to corporate endpoints. User authorization should be combined with client device authentication to ensure both the user and the device are authorized for OS deployment. WDS authorization policies integrate with Active Directory to enforce group membership requirements before allowing OS deployment.",
                Tags = ["wds", "authorization", "deployment", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireUserAuthorization", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireUserAuthorization")],
                DetectOps = [RegOp.CheckDword(Key, "RequireUserAuthorization", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-enable-boot-logging",
                Label = "Enable WDS Boot Session Logging",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "WDS boot session logging records all client connections, image deployment attempts, and deployment outcomes for audit and troubleshooting purposes. Enabling boot logging ensures a complete record of all WDS deployment activity including successful and failed deployments. Boot session logs help identify unauthorized deployment attempts or unusual deployment patterns that may indicate security incidents. WDS deployment logs are valuable for capacity planning and identifying deployment infrastructure problems. Security monitoring of WDS logs should include alerts for deployments outside of authorized maintenance windows or from unauthorized source addresses. Boot logging should be forwarded to the SIEM to correlate deployment events with other security indicators.",
                Tags = ["wds", "logging", "audit", "deployment", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableBootLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableBootLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableBootLogging", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-disable-tftp-anonymous-access",
                Label = "Disable Anonymous TFTP Access to WDS Boot Files",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "WDS uses TFTP to serve boot files to PXE-booting clients and by default these files may be accessible via anonymous TFTP connections. Disabling anonymous TFTP access prevents unauthorized clients from downloading WDS boot files without authentication. Boot files themselves may not contain sensitive data but unrestricted TFTP access enables reconnaissance of the deployment infrastructure. TFTP file access should require client authentication to prevent mapping of the WDS boot file structure by attackers. Restricting TFTP access forces deployment clients to authenticate before receiving deployment configuration reducing unauthorized access risk. Organizations should evaluate whether anonymous TFTP is required for their deployment infrastructure or if authentication can be enforced.",
                Tags = ["wds", "tftp", "anonymous", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAnonymousTftp", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAnonymousTftp")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAnonymousTftp", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-restrict-image-groups",
                Label = "Restrict WDS Image Groups to Authorized Groups Only",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "WDS image groups organize OS images for deployment and access can be restricted to specific Active Directory security groups. Restricting image group access ensures that only authorized personnel and systems can deploy specific operating system images. Unrestricted image group access allows any authenticated user to initiate deployment of any available OS image including specialized system images. Role-based access to image groups ensures that desktop technicians access desktop images while server images are restricted to server administrators. Image group restrictions prevent employees from initiating unauthorized OS replacements on endpoints they manage. Image access auditing should be enabled alongside image group restrictions to log all access attempts.",
                Tags = ["wds", "image-groups", "access-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictImageGroupAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictImageGroupAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictImageGroupAccess", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-enable-driver-injection-restriction",
                Label = "Restrict WDS Driver Injection to Approved Drivers",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "WDS driver injection adds hardware-specific drivers to OS images during deployment providing out-of-box device compatibility. Restricting driver injection to approved driver packages prevents unapproved or potentially malicious drivers from being injected into deployment images. Driver injection without restrictions could allow an attacker who gains access to WDS infrastructure to inject malicious drivers into OS images. Approved driver packages should be tested and validated before adding to the WDS driver store for injection. Driver signing requirements should be enforced for all drivers added to the WDS driver store to prevent injection of unsigned drivers. WDS driver injection policies integrate with Windows Update for Business and Microsoft Update Catalog for validated driver sources.",
                Tags = ["wds", "drivers", "injection", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictDriverInjection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictDriverInjection")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictDriverInjection", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-disable-wds-client-logging",
                Label = "Disable WDS Client-Side Telemetry Logging",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "WDS client telemetry logging can transmit deployment event data from client systems back to Microsoft or deployment management systems. Disabling WDS client telemetry logging reduces the data transmitted about deployment infrastructure and client system configuration. Client telemetry during OS deployment includes hardware enumeration data, deployment timing, and setup configuration details. Reducing telemetry transmission during deployment limits the external visibility into enterprise deployment infrastructure and hardware inventory. WDS client telemetry data is most useful for troubleshooting deployment issues but may expose sensitive infrastructure details if transmitted externally. Organizations that rely on WDS telemetry for deployment health monitoring should route data to internal logging rather than external Microsoft services.",
                Tags = ["wds", "telemetry", "logging", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableClientTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClientTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClientTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-enforce-network-boot-security",
                Label = "Enforce Secure Network Boot Validation",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Secure network boot validation ensures that WDS boot images are cryptographically verified before execution preventing deployment of tampered images. Enforcing secure network boot validation prevents boot time attacks where an attacker replaces legitimate WDS boot images with malicious alternatives. Boot image integrity validation uses digital signatures to verify that images have not been modified since signing by the deployment administrator. Secure network boot is essential in environments where WDS infrastructure may be accessible to adversaries with lateral movement capabilities. Boot image signing should use code signing certificates with appropriate access controls to prevent unauthorized image signing. Secure network boot validation should be combined with Secure Boot on client systems to create an end-to-end boot integrity chain.",
                Tags = ["wds", "secure-boot", "validation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceSecureNetworkBoot", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceSecureNetworkBoot")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceSecureNetworkBoot", 1)],
            },
            new TweakDef
            {
                Id = "depsvc-limit-wds-server-accessibility",
                Label = "Restrict WDS Server Access to Deployment VLAN",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "WDS servers should only be accessible from deployment VLANs and networks used for OS deployment activities rather than from all corporate subnets. Restricting WDS server accessibility reduces the attack surface by limiting which network segments can reach the deployment infrastructure. Production VLANs should not have direct access to WDS servers unless those servers are actively used for production endpoint deployment. Deployment infrastructure accessible from all corporate subnets increases the risk of unauthorized deployment attempts or WDS infrastructure exploitation. Network ACLs and host-based firewall rules should restrict WDS port access to deployment VLANs and administrator management networks. WDS server accessibility restrictions should be documented and reviewed regularly as network topology changes.",
                Tags = ["wds", "network-restriction", "vlan", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LimitServerAccessibility", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LimitServerAccessibility")],
                DetectOps = [RegOp.CheckDword(Key, "LimitServerAccessibility", 1)],
            },
        ];
    }

    // ── DomainControllerHardeningPolicy ──
    private static class _DomainControllerHardeningPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";
        private const string SecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Netlogon";
        private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "dchrdn-restrict-null-session-pipes",
                    Label = "Restrict Null Session Named Pipe Access to Empty List",
                    Category = "System — Adfs Federation",
                    Description =
                        "Removes all entries from the NullSessionPipes registry value, ensuring no named pipes can be accessed via anonymous null session connections on this machine, closing a legacy attack vector for anonymous RPC enumeration.",
                    Tags = ["netlogon", "null-session", "named-pipes", "anonymous", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Null session named pipe list cleared; anonymous RPC pipe access completely blocked.",
                    ApplyOps = [RegOp.SetString(SecKey, "NullSessionPipes", "")],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "NullSessionPipes")],
                    DetectOps = [RegOp.CheckString(SecKey, "NullSessionPipes", "")],
                },
                new TweakDef
                {
                    Id = "dchrdn-log-netlogon-failures",
                    Label = "Log Netlogon Secure Channel Failure Events",
                    Category = "System — Adfs Federation",
                    Description =
                        "Enables detailed event log entries for Netlogon secure channel establishment failures, authentication denials, and secure channel seal/sign rejections, providing visibility into DC trust channel attacks.",
                    Tags = ["netlogon", "event-log", "audit", "secure-channel-failure", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Netlogon secure channel failure events logged; DC authentication and Zerologon attack attempts visible.",
                    ApplyOps = [RegOp.SetDword(Key, "DbFlag", 0x2080FFFF)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DbFlag")],
                    DetectOps = [RegOp.CheckDword(Key, "DbFlag", 0x2080FFFF)],
                },
                new TweakDef
                {
                    Id = "dchrdn-restrict-ntlm-in-domain",
                    Label = "Restrict Incoming NTLM Authentication in Domain Context",
                    Category = "System — Adfs Federation",
                    Description =
                        "Configures this domain member to block incoming NTLM authentication from domain accounts, requiring Kerberos for all intra-domain service authentication and preventing NTLM relay and pass-the-hash attacks between domain members.",
                    Tags = ["netlogon", "ntlm", "domain", "relay-attack", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Incoming NTLM from domain accounts blocked; intra-domain Kerberos required. NTLM relay via domain accounts mitigated.",
                    ApplyOps = [RegOp.SetDword(LsaKey, "RestrictReceivingNTLMTrafficInDomain", 2)],
                    RemoveOps = [RegOp.DeleteValue(LsaKey, "RestrictReceivingNTLMTrafficInDomain")],
                    DetectOps = [RegOp.CheckDword(LsaKey, "RestrictReceivingNTLMTrafficInDomain", 2)],
                },
                new TweakDef
                {
                    Id = "dchrdn-disable-netlogon-telemetry",
                    Label = "Disable Netlogon and Domain Services Telemetry to Microsoft",
                    Category = "System — Adfs Federation",
                    Description =
                        "Prevents the Netlogon service and domain authentication components from sending DC trust channel statistics, authentication success rates, and secure channel negotiation telemetry to Microsoft.",
                    Tags = ["netlogon", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Netlogon telemetry to Microsoft disabled; DC channel stats and domain auth data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableNetlogonTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableNetlogonTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableNetlogonTelemetry", 1)],
                },
            ];
    }

    // ── DomainIsolationPolicy ──
    private static class _DomainIsolationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPSec";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "domiso-enable-ipsec-policy",
                Label = "Enable IPsec Domain Isolation Policy",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "IPsec domain isolation restricts network communication so that domain-joined computers only accept connections from other authenticated domain members. Enabling domain isolation prevents non-domain-joined devices including compromised guest systems from establishing network connections with managed endpoints. Domain isolation is one of the most effective lateral movement prevention controls in Windows enterprise environments. All network traffic between domain-isolated endpoints is authenticated and optionally encrypted using IPsec transport mode. Implementing domain isolation requires IPsec firewall rules deployed through Group Policy that allow or require authentication. Domain isolation significantly reduces the blast radius of a single compromised endpoint by preventing lateral movement to other domain hosts.",
                Tags = ["ipsec", "domain-isolation", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableDomainIsolation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDomainIsolation")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDomainIsolation", 1)],
            },
            new TweakDef
            {
                Id = "domiso-require-auth-for-inbound",
                Label = "Require IPsec Authentication for Inbound Connections",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Requiring IPsec authentication for inbound connections ensures that all incoming network requests come from identifiable and domain-authenticated sources. Inbound authentication requirements prevent anonymous network connections and force all sources to be authenticated before services are accessible. IPsec authentication for inbound traffic uses Kerberos tickets from domain-joined computers providing cryptographic proof of identity. Non-domain resources that need access can be granted exemptions through connection security rule exceptions while keeping general isolation. Requiring inbound authentication effectively creates a software-defined perimeter that moves beyond network segmentation. This policy is foundational for server isolation scenarios where critical servers should only accept connections from specific authenticated hosts.",
                Tags = ["ipsec", "inbound", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireAuthForInbound", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAuthForInbound")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAuthForInbound", 1)],
            },
            new TweakDef
            {
                Id = "domiso-enable-ipsec-encryption",
                Label = "Enable IPsec Traffic Encryption",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "IPsec traffic encryption protects the confidentiality of data transmitted between domain-isolated endpoints beyond providing authentication. Encrypting IPsec traffic ensures that even if network traffic is intercepted the data content remains confidential. IPsec encryption complements authentication by preventing data-in-transit exposure for east-west traffic between domain systems. Enabling encryption in domain isolation scenarios requires negotiating encryption algorithms through IKE and maintaining security associations. AES-128 or AES-256 should be specified as the encryption algorithms in IPsec policy for modern compliance requirements. IPsec encryption for all domain traffic provides confidentiality protection that complements TLS-based application encryption.",
                Tags = ["ipsec", "encryption", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableIPsecEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIPsecEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIPsecEncryption", 1)],
            },
            new TweakDef
            {
                Id = "domiso-prefer-aes256",
                Label = "Prefer AES-256 for IPsec Encryption",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "AES-256 provides 256-bit symmetric encryption which exceeds NIST SP 800-57 recommendations for protection beyond 2030. Preferring AES-256 in IPsec policy ensures the strongest available symmetric encryption is negotiated for domain isolation traffic. Weaker algorithms like 3DES or AES-128 should only be used for compatibility when AES-256 is unavailable. AES-256 in IPsec may have slightly higher processing overhead than AES-128 but this is negligible on modern CPUs with AES-NI hardware acceleration. Setting AES-256 as the preferred algorithm ensures IKE negotiation selects the strongest option when both parties support it. Hardcoding preferred algorithms in IPsec policy prevents algorithm downgrade during negotiation to weaker but still technically supported options.",
                Tags = ["ipsec", "aes256", "encryption", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreferAES256", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreferAES256")],
                DetectOps = [RegOp.CheckDword(Key, "PreferAES256", 1)],
            },
            new TweakDef
            {
                Id = "domiso-enable-perfect-forward-secrecy",
                Label = "Enable Perfect Forward Secrecy for IPsec",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Perfect forward secrecy ensures that compromise of long-term keys does not allow decryption of previously recorded encrypted sessions. Enabling PFS for IPsec generates unique session keys for each IPsec security association using ephemeral Diffie-Hellman key exchange. Without PFS an attacker who records encrypted traffic can decrypt it after compromising the long-term keys used in the key exchange. PFS in IPsec requires renegotiation of master keys during IKE Phase 2 which adds some processing overhead. Diffie-Hellman Group 14 (2048-bit) or stronger should be specified for PFS to provide adequate security for the key exchange. PFS is an important property for long-lived secrets and provides cryptographic forward secrecy as part of defense-in-depth.",
                Tags = ["ipsec", "pfs", "forward-secrecy", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePerfectForwardSecrecy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePerfectForwardSecrecy")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePerfectForwardSecrecy", 1)],
            },
            new TweakDef
            {
                Id = "domiso-block-non-ipsec-fallback",
                Label = "Block Fallback to Unprotected Connections",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Connection fallback allows domain-isolated endpoints to fall back to unauthenticated connections when IPsec negotiation fails. Blocking fallback from IPsec-required connections ensures that traffic either uses IPsec protection or is not transmitted. Allowing fallback creates a gap in domain isolation where an attacker can force IPsec negotiation failure and access a target via plain traffic. Connection security rules should specify whether partial authentication fallback is allowed on a per-rule basis. Blocking fallback is appropriate for high-security servers while client endpoints may allow fallback for connections to non-domain resources. The trade-off between security (no fallback) and availability (fallback for resilience) must be evaluated for each deployment context.",
                Tags = ["ipsec", "fallback", "domain-isolation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockNonIPsecFallback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockNonIPsecFallback")],
                DetectOps = [RegOp.CheckDword(Key, "BlockNonIPsecFallback", 1)],
            },
            new TweakDef
            {
                Id = "domiso-enable-ike-v2",
                Label = "Require IKEv2 for IPsec Key Exchange",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "IKEv2 is the modern version of the Internet Key Exchange protocol providing improved security, reliability, and mobility compared to IKEv1. Requiring IKEv2 ensures that IPsec connections use the protocol version with MOBIKE support, traffic selectors, and improved negotiation. IKEv2 includes native dead peer detection and eliminates many of the mode negotiation vulnerabilities present in IKEv1 main and aggressive mode. IKEv2 with EAP authentication provides a strong mutual authentication mechanism suitable for remote access and domain isolation scenarios. Windows has supported IKEv2 since Windows 7 so requiring IKEv2 should not cause compatibility issues on modern enterprise endpoints. Requiring IKEv2 eliminates exposure to IKEv1 vulnerabilities including aggressive mode pre-shared key cracking and main mode identity disclosure.",
                Tags = ["ipsec", "ikev2", "key-exchange", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireIKEv2", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireIKEv2")],
                DetectOps = [RegOp.CheckDword(Key, "RequireIKEv2", 1)],
            },
            new TweakDef
            {
                Id = "domiso-log-ipsec-failures",
                Label = "Enable IPsec Negotiation Failure Logging",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "IPsec negotiation failure logging records events when IPsec key exchange fails providing visibility into domain isolation policy enforcement. Enabling failure logging helps diagnose misconfigured clients, rogue devices attempting connections, and policy configuration errors. Persistent IPsec negotiation failures from unexpected source addresses may indicate unauthorized devices attempting to communicate. IPsec failure events in Windows Firewall and Security Auditing logs include source/destination addresses, error codes, and protocol identifiers. SIEM correlation of IPsec failures with other security events enables detection of attempts to circumvent domain isolation. IPsec failure logging is essential during initial domain isolation deployment to identify endpoints that need policy updates.",
                Tags = ["ipsec", "logging", "failures", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LogIPsecFailures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogIPsecFailures")],
                DetectOps = [RegOp.CheckDword(Key, "LogIPsecFailures", 1)],
            },
            new TweakDef
            {
                Id = "domiso-exempt-icmp",
                Label = "Configure ICMP Exemption from IPsec",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "ICMP exemption from IPsec authentication requirements allows diagnostic network utilities like ping to function without IPsec negotiations. Configuring ICMP exemptions maintains diagnostic capability while requiring IPsec for all other traffic types. ICMP traffic does not carry sensitive data and exempting it simplifies troubleshooting without compromising the security of data-carrying connections. Exempted ICMP traffic still traverses the network in plaintext which is acceptable since ICMP carries diagnostic information not sensitive data. Overly strict IPsec policies that fail ICMP traffic complicate network troubleshooting and may cause connectivity issues with network monitoring tools. ICMP exemption should be combined with ICMP rate limiting and filtering to prevent ICMP-based denial of service attacks.",
                Tags = ["ipsec", "icmp", "exemption", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ExemptICMPFromIPSec", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ExemptICMPFromIPSec")],
                DetectOps = [RegOp.CheckDword(Key, "ExemptICMPFromIPSec", 1)],
            },
            new TweakDef
            {
                Id = "domiso-enable-ipsec-monitoring",
                Label = "Enable IPsec Security Association Monitoring",
                Category = "System — Adfs Federation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "IPsec security association monitoring tracks active IPsec connections and provides operational visibility into domain isolation health. Enabling SA monitoring exposes IPsec connection state for security operations teams to identify unexpected or problematic security associations. Live IPsec SA monitoring can detect sudden changes in protected connection counts that may indicate domain isolation failures or attacks. Security association data is available through Windows Firewall advanced monitoring and the Get-NetIPsecSA PowerShell cmdlet. Monitoring SA establishment rates can identify DoS attempts targeting IKE negotiation processes. IPsec monitoring data should be incorporated into security dashboards alongside firewall, endpoint, and network telemetry.",
                Tags = ["ipsec", "monitoring", "security-association", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSAMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSAMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSAMonitoring", 1)],
            },
        ];
    }

    // ── DomainTrustPolicy ──
    private static class _DomainTrustPolicy
    {
        private const string NetlogonKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

        private const string SystemPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "domtrust-enable-sid-filter-quarantine",
                    Label = "Domain Trust: Enable SID Filtering (Quarantine) on External Trusts",
                    Category = "System — Domain Trust",
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
                    Id = "domtrust-disable-anonymous-trust-dc-discovery",
                    Label = "Domain Trust: Disable Anonymous Trust DC Discovery Across Forest",
                    Category = "System — Domain Trust",
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
                    Category = "System — Domain Trust",
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
                    Category = "System — Domain Trust",
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
                    Category = "System — Domain Trust",
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
                    Category = "System — Domain Trust",
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
                    Category = "System — Domain Trust",
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
                    Id = "domtrust-set-net-logon-service-tgt-ttl-3600",
                    Label = "Domain Trust: Set Cross-Forest Referral Ticket TTL to 3600 Seconds",
                    Category = "System — Domain Trust",
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

    // ── EasMdmPolicy ──
    private static class _EasMdmPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EasMdm";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "easmdm-require-device-password",
                Label = "Exchange ActiveSync MDM Policy: Require Device Password",
                Category = "System — Domain Trust",
                Description =
                    "Enforces a device password requirement via Exchange ActiveSync MDM policy. "
                    + "When enabled, users must configure a PIN or password before the device can synchronise with an Exchange server. "
                    + "This aligns with corporate security baselines that mandate authentication on managed endpoints. "
                    + "Disabling removes the enforced password requirement imposed by EAS MDM.",
                Tags = ["eas", "mdm", "password", "exchange", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PasswordEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PasswordEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "PasswordEnabled", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enforces device password via EAS MDM; improves security posture on managed devices.",
            },
            new TweakDef
            {
                Id = "easmdm-min-password-length",
                Label = "Exchange ActiveSync MDM Policy: Set Minimum Password Length (8)",
                Category = "System — Domain Trust",
                Description =
                    "Sets the minimum device password length to 8 characters via Exchange ActiveSync MDM policy. "
                    + "Short passwords are vulnerable to brute-force attacks, especially on mobile and endpoint devices. "
                    + "A minimum of 8 characters is recommended by NIST SP 800-63B and aligns with most corporate security policies. "
                    + "Removing this policy reverts to the platform default (typically 4 characters).",
                Tags = ["eas", "mdm", "password", "length", "exchange"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MinDevicePasswordLength", 8)],
                RemoveOps = [RegOp.DeleteValue(Key, "MinDevicePasswordLength")],
                DetectOps = [RegOp.CheckDword(Key, "MinDevicePasswordLength", 8)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Forces minimum 8-character passwords on EAS-managed devices, reducing brute-force risk.",
            },
            new TweakDef
            {
                Id = "easmdm-max-failed-attempts",
                Label = "Exchange ActiveSync MDM Policy: Limit Max Failed Password Attempts (10)",
                Category = "System — Domain Trust",
                Description =
                    "Caps the number of consecutive failed password attempts to 10 before triggering a device lockout via Exchange ActiveSync MDM. "
                    + "Limiting failed attempts deters brute-force attacks against the device lock screen. "
                    + "After the threshold is reached, the device is locked and may require an administrator to unlock or initiate a remote wipe. "
                    + "Removing this policy restores the uncapped default.",
                Tags = ["eas", "mdm", "password", "failed-attempts", "lockout"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxDevicePasswordFailedAttempts", 10)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxDevicePasswordFailedAttempts")],
                DetectOps = [RegOp.CheckDword(Key, "MaxDevicePasswordFailedAttempts", 10)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Triggers lockout after 10 failed password attempts; protects against brute-force attacks.",
            },
            new TweakDef
            {
                Id = "easmdm-inactivity-lock-5min",
                Label = "Exchange ActiveSync MDM Policy: Lock Device After 5 Minutes Inactivity",
                Category = "System — Domain Trust",
                Description =
                    "Configures the Exchange ActiveSync MDM policy to lock the device screen after 5 minutes of inactivity. "
                    + "Auto-locking an idle device prevents unauthorised access when the device is left unattended. "
                    + "Five minutes is the industry-standard timeout recommended for corporate laptops and workstations. "
                    + "Removing this policy lifts the MDM-enforced inactivity timeout.",
                Tags = ["eas", "mdm", "lock", "inactivity", "screen-lock"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxInactivityTimeDeviceLock", 5)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxInactivityTimeDeviceLock")],
                DetectOps = [RegOp.CheckDword(Key, "MaxInactivityTimeDeviceLock", 5)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Auto-locks device after 5 minutes; reduces risk of unauthorised access to unattended endpoints.",
            },
            new TweakDef
            {
                Id = "easmdm-require-encryption",
                Label = "Exchange ActiveSync MDM Policy: Require Device Encryption",
                Category = "System — Domain Trust",
                Description =
                    "Requires full device storage encryption via Exchange ActiveSync MDM policy. "
                    + "Encryption ensures that data stored on the device cannot be read if the hardware is lost or stolen. "
                    + "This policy is mandatory for PCI-DSS, HIPAA, and most corporate data-protection frameworks. "
                    + "Removing this setting lifts the MDM encryption mandate.",
                Tags = ["eas", "mdm", "encryption", "bitlocker", "data-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireDeviceEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "RequireDeviceEncryption", 1)],
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Mandates full-disk encryption on EAS-managed devices; critical for data-at-rest protection.",
            },
            new TweakDef
            {
                Id = "easmdm-block-wifi",
                Label = "Exchange ActiveSync MDM Policy: Block Wi-Fi Connections",
                Category = "System — Domain Trust",
                Description =
                    "Disables Wi-Fi connectivity on the device via Exchange ActiveSync MDM policy. "
                    + "Blocking Wi-Fi forces the device to use wired or cellular connections, reducing exposure on potentially unsecured wireless networks. "
                    + "This is typically applied to high-security endpoints or kiosk devices where wireless connectivity is not permitted. "
                    + "Removing this policy restores MDM-controlled Wi-Fi access.",
                Tags = ["eas", "mdm", "wifi", "network", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowWiFi", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowWiFi")],
                DetectOps = [RegOp.CheckDword(Key, "AllowWiFi", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks Wi-Fi on managed devices; enforces wired/cellular-only network access.",
            },
            new TweakDef
            {
                Id = "easmdm-block-removable-storage",
                Label = "Exchange ActiveSync MDM Policy: Block Removable Storage",
                Category = "System — Domain Trust",
                Description =
                    "Prevents access to removable storage media (SD cards, USB drives) via Exchange ActiveSync MDM policy. "
                    + "Removable storage is a common vector for data exfiltration and introduction of malware. "
                    + "Blocking it on managed endpoints aligns with DLP (Data Loss Prevention) requirements in regulated industries. "
                    + "Removing this policy restores MDM-controlled removable storage access.",
                Tags = ["eas", "mdm", "storage", "dlp", "removable"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowRemovableStorage", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowRemovableStorage")],
                DetectOps = [RegOp.CheckDword(Key, "AllowRemovableStorage", 0)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks SD cards and USB drives on MDM-managed devices; reduces data exfiltration risk.",
            },
            new TweakDef
            {
                Id = "easmdm-block-camera",
                Label = "Exchange ActiveSync MDM Policy: Block Camera Use",
                Category = "System — Domain Trust",
                Description =
                    "Disables camera hardware on the device via Exchange ActiveSync MDM policy. "
                    + "Camera restrictions are commonly required in secure facilities, clean-room environments, or for devices that handle classified information. "
                    + "Enforcing this via MDM policy ensures compliance cannot be bypassed by the end user. "
                    + "Removing this policy restores camera availability on MDM-managed devices.",
                Tags = ["eas", "mdm", "camera", "privacy", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowCamera", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCamera")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCamera", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables camera on MDM-managed devices; required for secure-facility compliance.",
            },
            new TweakDef
            {
                Id = "easmdm-block-internet-sharing",
                Label = "Exchange ActiveSync MDM Policy: Block Internet Sharing / Hotspot",
                Category = "System — Domain Trust",
                Description =
                    "Blocks the ability to share the device's internet connection (hotspot/tethering) via Exchange ActiveSync MDM policy. "
                    + "Mobile hotspot can bypass corporate network monitoring and proxy controls, introducing compliance gaps. "
                    + "Prohibiting internet sharing on managed endpoints is a common corporate policy to maintain network visibility. "
                    + "Removing this policy lifts the MDM hotspot restriction.",
                Tags = ["eas", "mdm", "hotspot", "tethering", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowInternetSharing", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowInternetSharing")],
                DetectOps = [RegOp.CheckDword(Key, "AllowInternetSharing", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents hotspot/tethering on managed devices; maintains corporate network control.",
            },
            new TweakDef
            {
                Id = "easmdm-block-bluetooth",
                Label = "Exchange ActiveSync MDM Policy: Block Bluetooth",
                Category = "System — Domain Trust",
                Description =
                    "Disables Bluetooth connectivity on the device via Exchange ActiveSync MDM policy. "
                    + "Bluetooth can be exploited for proximity-based attacks (BlueSnarfing, BIAS) or used to exfiltrate data without leaving a network trace. "
                    + "Disabling Bluetooth is recommended for high-security endpoints where physical proximity attacks are a concern. "
                    + "Removing this policy restores MDM-controlled Bluetooth access (value 2 = allow, 0 = block).",
                Tags = ["eas", "mdm", "bluetooth", "wireless", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowBluetooth", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowBluetooth")],
                DetectOps = [RegOp.CheckDword(Key, "AllowBluetooth", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables Bluetooth on MDM-managed devices; reduces BlueSnarfing and proximity-based attack risk.",
            },
        ];
    }

    // ── EnterpriseDeviceManagementPolicy ──
    private static class _EnterpriseDeviceManagementPolicy
    {
        private const string ErmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager";

        private const string MdmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edm-enable-comanagement-with-sccm",
                    Label = "Enterprise Device Management: Enable Intune/SCCM Co-Management",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets EnableCoManagement=1 in MDM policy. Enables co-management of Windows 10/11 devices by both System Center Configuration Manager (SCCM/ConfigMgr) and Microsoft Intune simultaneously. Co-management allows gradual migration of workloads from SCCM to Intune — starting with compliance evaluation and conditional access in Intune while keeping software deployment in SCCM. Without this policy, organizations must choose one management plane. Co-management is the Microsoft-recommended path for organizations with existing SCCM infrastructure transitioning to cloud-modern management.",
                    Tags = ["co-management", "sccm", "configmgr", "intune", "cloud-management"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices are managed by both SCCM and Intune simultaneously. Workload authority (compliance, resource access, app deployment) is configurable per workload. Requires ConfigMgr 1710 or later and Intune subscription. Co-management authority conflicts are resolved by the workload slider settings in the SCCM console.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnableCoManagement", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableCoManagement")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnableCoManagement", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-remote-lock-on-compliance-breach",
                    Label = "Enterprise Device Management: Enable Remote Lock on Compliance Breach",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets EnableRemoteLockOnComplianceBreach=1 in EnterpriseResourceManager policy. Configures the MDM client to accept remote lock commands from the MDM authority when the device is marked non-compliant AND has not remediated within the grace period. Remote lock sets the device to the lock screen and requires the user to enter their PIN/password to regain access. This prevents a non-compliant device from being used while IT is investigating or while the device is remediating a compliance issue — ensuring that a known-non-compliant device is not being actively used to access corporate resources.",
                    Tags = ["remote-lock", "compliance", "non-compliant", "mdm", "incident-response"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "MDM authority can remotely lock a non-compliant device. The device requires the user's credentials to unlock. User may be temporarily unable to complete their work if locked during active use. Ensure a clear remediation process is communicated to users before deploying. Not the same as remote wipe — data is not affected.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "EnableRemoteLockOnComplianceBreach", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableRemoteLockOnComplianceBreach")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "EnableRemoteLockOnComplianceBreach", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-selective-wipe-on-unenroll",
                    Label = "Enterprise Device Management: Enable Selective Wipe of Corporate Data on Unenroll",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets EnableSelectiveWipeOnUnenroll=1 in EnterpriseResourceManager policy. Enables selective wipe of corporate data when a device unenrolls from MDM. Selective wipe removes only corporate-managed content: corporate email profiles, MDM-deployed certificates, VPN profiles, Wi-Fi profiles, and corporate app data — while preserving personal files, photos, and applications. This is the appropriate default for BYOD scenarios: when an employee leaves and disconnects their personal device from MDM, the corporate data is cleaned up without erasing the employee's personal content.",
                    Tags = ["selective-wipe", "unenrollment", "corporate-data", "byod", "data-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Unenrollment from MDM triggers removal of all MDM-deployed profiles, certificates, and managed app data. Personal files and apps are preserved. A corporate AAD-joined device unenrolling may lose domain join state. Not a full device wipe — ensure your users understand what is removed on unenrollment.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "EnableSelectiveWipeOnUnenroll", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableSelectiveWipeOnUnenroll")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "EnableSelectiveWipeOnUnenroll", 1)],
                },
                new TweakDef
                {
                    Id = "edm-require-approved-apps-only",
                    Label = "Enterprise Device Management: Restrict App Installation to MDM-Approved Apps Only",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets RequireApprovedAppsOnly=1 in EnterpriseResourceManager policy. Restricts app installation to apps that are deployed or approved by the MDM authority. Users are not permitted to install arbitrary apps from the Microsoft Store or third-party sources unless the MDM administrator has explicitly approved them in the app catalog. This policy is typically layered with AppLocker or Windows Defender Application Control. On its own, it provides an MDM-layer approval gate that blocks app installation from retail Store listings, reducing the attack surface from malicious store apps.",
                    Tags = ["approved-apps", "app-control", "mdm", "store", "whitelisting"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Only MDM-approved apps can be installed by users. Non-approved app installation attempts are blocked. Requires maintaining an approved app catalog in the MDM console. Users who need new apps must request IT approval. May disrupt productivity if the approval catalog is not kept up to date.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "RequireApprovedAppsOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "RequireApprovedAppsOnly")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "RequireApprovedAppsOnly", 1)],
                },
                new TweakDef
                {
                    Id = "edm-sync-device-inventory-every-4h",
                    Label = "Enterprise Device Management: Sync Device Inventory to MDM Every 4 Hours",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets InventorySyncIntervalHours=4 in EnterpriseResourceManager policy. Configures the MDM client to push a device inventory update (installed apps, hardware specs, disk space, OS version, installed patches) to the MDM authority every 4 hours. Accurate, fresh device inventory is essential for software license compliance, vulnerability management (detecting devices missing patches), and asset management. A staleinventory (updated less than once daily) may miss a device that has been reformatted, had apps removed, or had OS version changed — leading to false compliance reporting.",
                    Tags = ["inventory", "sync-interval", "asset-management", "vulnerability-mgmt", "mdm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Device inventory is uploaded to the MDM authority every 4 hours. Inventory includes installed apps, hardware, and OS state. Slightly increased MDM check-in frequency and bandwidth. Inventory sync data is typically 5–50 KB per cycle.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "InventorySyncIntervalHours", 4)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "InventorySyncIntervalHours")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "InventorySyncIntervalHours", 4)],
                },
                new TweakDef
                {
                    Id = "edm-block-factory-reset-by-user",
                    Label = "Enterprise Device Management: Prevent User-Initiated Factory Reset",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets BlockUserInitiatedFactoryReset=1 in EnterpriseResourceManager policy. Prevents standard users from performing a factory reset (Settings > System > Recovery > Reset this PC, or WinRE recovery). Factory reset bypasses MDM policies, removes all corporate data and certificates, and leaves the device unmanaged. An insider threat actor could use factory reset to wipe evidence before investigation. A regular user could accidentally factory reset, losing both personal and corporate data. IT-initiated remote wipe via the MDM console remains available for authorized operations.",
                    Tags = ["factory-reset", "protective", "insider-threat", "data-preservation", "mdm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Standard users cannot initiate factory reset. Local administrators can still reset via elevated permission flows. IT-initiated remote wipe from MDM console is not affected. Users who genuinely need to re-provision their device must contact IT.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "BlockUserInitiatedFactoryReset", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "BlockUserInitiatedFactoryReset")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "BlockUserInitiatedFactoryReset", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-mdm-certificate-renewal",
                    Label = "Enterprise Device Management: Enable Automatic MDM Certificate Renewal",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets EnableMdmCertificateRenewal=1 in MDM policy. Configures the MDM client to automatically renew the MDM enrollment certificate before it expires. The MDM enrollment certificate authenticates the device to the MDM service on every check-in. If this certificate expires without renewal, the device loses the ability to receive new policies, report compliance status, or accept remote management commands — even though it may still appear enrolled in the MDM console. Automatic renewal prevents this silent disconnection, which is especially important for devices in long-term storage or deployed in air-gapped environments.",
                    Tags = ["mdm", "certificate", "renewal", "enrollment", "expiry"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "MDM enrollment certificates are renewed automatically before expiry. Renewal occurs in the background without user interaction. Prevents devices from silently dropping off MDM management due to certificate expiry. Certificate validity periods are typically 1–2 years — renewal triggers at 80% of the validity period.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnableMdmCertificateRenewal", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableMdmCertificateRenewal")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnableMdmCertificateRenewal", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-managed-device-restrictions",
                    Label = "Enterprise Device Management: Enable MDM-Enforced Managed Device Restrictions",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets EnableManagedDeviceRestrictions=1 in EnterpriseResourceManager policy. Enables the enforcement layer for MDM-delivered device restrictions — settings like camera disable, screen capture restriction, clipboard policy, USB disable, and Bluetooth restriction — that are delivered as MDM CSP payloads. Without this flag, MDM restriction payloads are accepted but not enforced at the OS level. This is a master switch that must be enabled for MDM-pushed restrictions to take effect. Relevant for organizations deploying information protection policies that require disabling hardware capabilities on managed devices.",
                    Tags = ["mdm", "device-restrictions", "camera-disable", "clipboard", "usb"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "MDM-delivered device restrictions are enforced by the OS. Without this, restrictions are delivered but silently not applied. Restrictions that take effect depend on which CSP payloads the MDM administrator has configured — this policy enables the enforcement mechanism, not specific restrictions.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "EnableManagedDeviceRestrictions", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableManagedDeviceRestrictions")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "EnableManagedDeviceRestrictions", 1)],
                },
                new TweakDef
                {
                    Id = "edm-audit-mdm-policy-changes",
                    Label = "Enterprise Device Management: Audit All MDM Policy Application Events",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets AuditMdmPolicyChanges=1 in MDM policy. Enables audit events whenever an MDM policy is applied, updated, or removed on the device. Each audit event records the CSP path that was changed, the old and new values, the MDM authority that issued the change, and the result (success or error code). MDM policy audit events are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel. These events are essential for SIEM correlation: if a device's MDM policy is unexpectedly changed (indicating a rogue MDM push or configuration scope error), the audit trail makes detection possible.",
                    Tags = ["mdm", "audit", "policy-changes", "siem", "event-log"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "All MDM policy application events are logged with CSP path, values, and origin. Events written to DeviceManagement-Enterprise-Diagnostics-Provider channel. Slightly higher log volume on devices with frequent policy changes (Intune check-in + policy delta). Enables SIEM alerting on unexpected MDM policy modifications.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "AuditMdmPolicyChanges", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "AuditMdmPolicyChanges")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "AuditMdmPolicyChanges", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-encrypted-mdm-channel",
                    Label = "Enterprise Device Management: Enforce TLS 1.2+ for MDM Communication",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets RequireEncryptedMdmChannel=1 in MDM policy. Enforces that all MDM client communication (enrollment, check-in, policy delivery, command receipt) is conducted over TLS 1.2 or higher. MDM payloads include configuration settings, app assignments, certificate payloads, and VPN profiles — all of which are sensitive. An MDM session over TLS 1.0 can be downgrade-attacked using known vulnerabilities (BEAST, POODLE) to intercept policy payloads. Enforcing TLS 1.2+ on the MDM channel ensures that policy delivery is encrypted to modern standards.",
                    Tags = ["mdm", "tls", "encrypted-channel", "transport-security", "policy-delivery"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "MDM communication is restricted to TLS 1.2 or higher. MDM servers that only support TLS 1.0 or 1.1 will be unable to communicate with the client. All modern MDM services (Intune, SCCM cloud attachment) use TLS 1.2+. On-premises MDM servers must be updated if they are still on legacy TLS.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "RequireEncryptedMdmChannel", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "RequireEncryptedMdmChannel")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "RequireEncryptedMdmChannel", 1)],
                },
            ];
    }

    // ── EnterpriseResourceDeployPolicy ──
    private static class _EnterpriseResourceDeployPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "erdeploy-set-default-deploy-ring-broad",
                    Label = "Enterprise Deploy: Set Default Application Deployment Ring to 'Broad' Stable Channel",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets DefaultDeployRing=2 in EnterpriseResourceManager policy. Configures the default application deployment ring for this endpoint to the 'Broad' (stable) deployment ring, ensuring the device receives application updates only after full release validation has been completed across the Pilot and Early Majority rings. "
                        + "Enterprise application deployments using modern ring-based rollout (Intune or ConfigMgr ring filtering) gate updates through sequenced rings before broad deployment. Endpoints that are miscategorised as 'Pilot' receive updates intended for testing and may encounter pre-release application bugs. Explicitly setting the deployment ring to 'Broad' (ring 2) prevents endpoints from accidentally receiving early-ring deployments due to misconfigured ring assignment logic.",
                    Tags = ["enterprise-deploy", "deployment-ring", "app-update", "staging", "rollout"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Endpoint assigned to Broad (stable) deployment ring; receives application updates only after full validation.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultDeployRing", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultDeployRing")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultDeployRing", 2)],
                },
                new TweakDef
                {
                    Id = "erdeploy-require-admin-for-app-removal",
                    Label = "Enterprise Deploy: Require Administrator Approval to Remove Managed Applications",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets RequireAdminForAppRemoval=1 in EnterpriseResourceManager policy. Blocks standard users from uninstalling applications that were deployed by the enterprise (via Intune, ConfigMgr, or Group Policy Software Installation), requiring administrative credentials for removal even though the application was installed in user context. "
                        + "Required enterprise applications (endpoint detection and response agents, certificate management tools, identity protection software) must remain installed once deployed. A standard user who can uninstall enterprise-managed apps can remove security tooling from their device, creating a gap in protection that may persist until the next compliance check triggers a remediation deployment. Blocking user-initiated uninstall of managed apps prevents intentional or accidental removal of critical security tools.",
                    Tags = ["enterprise-deploy", "app-removal", "security-tools", "admin-required", "lockdown"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Managed application removal requires admin approval; users cannot uninstall security tools deployed by IT.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForAppRemoval", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForAppRemoval")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForAppRemoval", 1)],
                },
                new TweakDef
                {
                    Id = "erdeploy-block-user-initiated-install",
                    Label = "Enterprise Deploy: Block User-Initiated Application Installation Outside of Managed Channels",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets BlockUserInitiatedInstall=1 in EnterpriseResourceManager policy. Prevents users from initiating the installation of new applications through any mechanism other than IT-managed deployment channels (Intune, ConfigMgr, Software Center) — blocking double-click installer execution, Windows Installer (MSI) invocation, and MSIX/APPX package sideloading by standard users. "
                        + "The majority of enterprise malware infections arrive as LOB-disguised executables or malicious MSI packages that a user is socially engineered into running. If users can execute arbitrary installers, the application allowlist maintained by IT is bypassed — even if the endpoint has Microsoft Defender WDAC policy configured, a sufficiently permissive WDAC policy allows signed MSI files from any vendor. Blocking user-initiated installation removes the primary vector for user-driven software installation.",
                    Tags = ["enterprise-deploy", "user-install", "msi", "lockdown", "wdac", "applocker"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "User-initiated application installs blocked; all software installations require IT-managed deployment channel.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUserInitiatedInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUserInitiatedInstall")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUserInitiatedInstall", 1)],
                },
                new TweakDef
                {
                    Id = "erdeploy-enforce-maintenance-window",
                    Label = "Enterprise Deploy: Enforce Deployment Maintenance Window Compliance",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets EnforceMaintenanceWindow=1 in EnterpriseResourceManager policy. Restricts deployment execution by the enterprise resource manager to within the configured maintenance window schedule, preventing deployments from triggering application installs, updates, or reboots during business hours and confining disruptive deployments to the approved maintenance period. "
                        + "Without maintenance window enforcement, a deployment configured as 'Available as soon as possible' may start an application install or triggered reboot at any time, including during an end-user presentation or in the middle of a running workflow. Maintenance windows define agreed low-impact periods (after hours, weekends) for deployments. Enforcing the maintenance window prevents IT from accidentally or intentionally bypassing the agreed change window, which is often an ITIL or change management process requirement.",
                    Tags = ["enterprise-deploy", "maintenance-window", "deployment-schedule", "change-management", "itil"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Deployments confined to maintenance window; no installs or reboots triggered outside approved maintenance period.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceMaintenanceWindow", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceMaintenanceWindow")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceMaintenanceWindow", 1)],
                },
                new TweakDef
                {
                    Id = "erdeploy-cap-max-install-retries-3",
                    Label = "Enterprise Deploy: Cap Application Installation Retry Attempts at 3",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets MaxInstallRetries=3 in EnterpriseResourceManager policy. Limits the number of times the enterprise resource manager retries a failed application installation to 3 attempts before marking the deployment as failed and triggering an alert, rather than retrying indefinitely. "
                        + "A deployment that retries an application installation indefinitely will continually consume CPU, disk I/O, and network bandwidth on the endpoint for days or weeks. On endpoints with transient installation failures (antivirus blocking the installer, required service temporarily unavailable), unlimited retries create ongoing performance degradation. Capping retries at 3 ensures failed deployments are surfaced as failures in the management console rather than silently retrying without ever succeeding.",
                    Tags = ["enterprise-deploy", "install-retry", "deployment-failure", "performance", "alert"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Deployment install retries capped at 3; repeat failures surface as deployment failures rather than silent perpetual retry.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxInstallRetries", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxInstallRetries")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxInstallRetries", 3)],
                },
                new TweakDef
                {
                    Id = "erdeploy-enable-deployment-audit-log",
                    Label = "Enterprise Deploy: Enable Security Audit Log for All Deployment Operations",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets EnableDeploymentAuditLog=1 in EnterpriseResourceManager policy. Causes each application installation, update, and removal operation completed by the enterprise resource manager to generate a Security event log entry, recording the application name, version, deployment source, requesting authority, and outcome code. "
                        + "Application deployment audit logs are required in PCI-DSS, HIPAA, and SOC2 regulated environments where all software changes on in-scope endpoints must be tracked in a tamper-evident audit log. Without deployment audit logging, an attacker who compromises the management channel and installs a malicious application through the enterprise deployment infrastructure would have no on-device trace of the install (as the standard registry Uninstall key is easily manipulated). Security event log entries are tamper-resistant to local manipulation.",
                    Tags = ["enterprise-deploy", "audit-log", "deployment", "pci", "hipaa", "soc2"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All enterprise deployment operations generate Security event entries; compliance audit trail for software changes.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableDeploymentAuditLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableDeploymentAuditLog")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableDeploymentAuditLog", 1)],
                },
                new TweakDef
                {
                    Id = "erdeploy-disable-sideloaded-appx-packages",
                    Label = "Enterprise Deploy: Disable Sideloading of APPX Packages from Unmanaged Sources",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets DisableSideloadedApps=1 in EnterpriseResourceManager policy. Prevents installation of APPX/MSIX application packages from unsigned or unmanaged sources (USB drives, SharePoint file shares, developer sideloading) and restricts APPX installation to managed channels only (Microsoft Store for Business, Intune managed app, or enterprise signed MSIX bundles). "
                        + "MSIX sideloading is the primary vector for distributing trojanised or repackaged application packages disguised as legitimate enterprise tools. An attacker who sends a malicious MSIX package via email or file share (and the user's developer mode is enabled) can have arbitrary code run in a package context with the package's declared capabilities. Disabling sideloading from unmanaged sources blocks this vector without affecting Store and Intune-delivered MSIX packages.",
                    Tags = ["enterprise-deploy", "sideloading", "appx", "msix", "developer-mode", "trojan"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "APPX/MSIX sideloading from unmanaged sources blocked; only Store and IT-signed packages install.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSideloadedApps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSideloadedApps")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSideloadedApps", 1)],
                },
                new TweakDef
                {
                    Id = "erdeploy-require-signed-deployment-packages",
                    Label = "Enterprise Deploy: Require Cryptographic Signing for All Deployment Packages",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets RequireSignedPackages=1 in EnterpriseResourceManager policy. Requires that every application package deployed through the enterprise resource manager is digitally signed by a certificate in the enterprise trusted publisher store before the installation is allowed to proceed, blocking unsigned or improperly signed packages from executing. "
                        + "Unsigned deployment packages can be tampered with between the time they are created and the time they are deployed. An attacker who compromises a Distribution Point or content staging server can replace a legitimate installer package with a trojanised version. Without package signing verification, the deployment infrastructure distributes the malicious version to all targeted endpoints without any integrity check. Requiring signed packages ensures only packages that passed code signing (and therefore were authenticated at signing time) are installed.",
                    Tags = ["enterprise-deploy", "package-signing", "integrity", "distribution-point", "code-signing"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Unsigned deployment packages blocked; content integrity verified via code signing before installation.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSignedPackages", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedPackages")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSignedPackages", 1)],
                },
                new TweakDef
                {
                    Id = "erdeploy-block-store-install-during-maintenance",
                    Label = "Enterprise Deploy: Block Microsoft Store Application Updates During Active Deployment Window",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets BlockStoreInstallDuringDeployment=1 in EnterpriseResourceManager policy. Suspends automatic Microsoft Store application updates from downloading and installing during active enterprise deployment windows, preventing Store-initiated background installs from competing with enterprise deployment bandwidth and CPU allocations. "
                        + "Large enterprise deployments (OS feature updates, security patches for hundreds of applications) consume significant bandwidth from Distribution Points. If the Microsoft Store simultaneously triggers background app updates across the same endpoints during the deployment window, both processes compete for disk I/O, network bandwidth, and Windows Installer service locking. This can cause enterprise deployments to fail with 'service busy' errors or time out due to resource contention. Blocking Store updates during scheduled deployment windows eliminates this interference.",
                    Tags = ["enterprise-deploy", "store", "bandwidth", "contention", "deployment-window"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Store app updates paused during enterprise deployment windows; no resource contention with managed deployments.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockStoreInstallDuringDeployment", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockStoreInstallDuringDeployment")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockStoreInstallDuringDeployment", 1)],
                },
                new TweakDef
                {
                    Id = "erdeploy-enable-prerequisite-check-enforcement",
                    Label = "Enterprise Deploy: Enforce Prerequisite Dependency Checks Before Application Deployment",
                    Category = "System — Domain Trust",
                    Description =
                        "Sets EnforcePrerequisiteChecks=1 in EnterpriseResourceManager policy. Enforces that the installation of a dependent application is verified as successfully installed and functional before the enterprise resource manager proceeds with a higher-level application deployment that requires it as a prerequisite, rather than attempting the deployment and failing at runtime. "
                        + "Enterprise application deployments often have prerequisite chains: a LOB application may require a specific .NET runtime version, a specific redistributable, and a specific licence management service to be installed before it will work. Without prerequisite enforcement, all packages attempt installation in parallel, and the LOB application may fail (or partially install) because its prerequisites aren't available yet. Enforcing prerequisite checks runs the dependency chain in the correct order and stops the deployment if any prerequisite fails.",
                    Tags = ["enterprise-deploy", "prerequisites", "dependency", "deployment-order", "reliability"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Prerequisite dependency verification enforced; deployment packages install in correct dependency order.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforcePrerequisiteChecks", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforcePrerequisiteChecks")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforcePrerequisiteChecks", 1)],
                },
            ];
    }

    // ── EnterpriseResourcePolicy ──
    private static class _EnterpriseResourcePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "entres-enable-enterprise-resource-audit",
                Label = "Enable Audit Logging for Enterprise Resource Access Events",
                Category = "System — Domain Trust",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enterprise resource access auditing creates detailed logs of access to managed enterprise resources enabling compliance reporting and security monitoring for sensitive business assets. Audit events for enterprise resources include successful and failed access attempts the identity of the requester the time of access and the resources accessed. Comprehensive resource access logging is a requirement for many regulatory frameworks including PCI-DSS HIPAA and SOX that mandate audit trails for access to regulated data and systems. Organizations should forward enterprise resource audit events to a central security information and event management system for correlation and long-term retention. Audit logs should be protected from modification and deletion and retained for a period consistent with regulatory requirements for the organization. Regular review of enterprise resource audit data helps identify access patterns that deviate from expected behavior.",
                Tags = ["enterprise-resources", "audit", "compliance", "access-control", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableResourceAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableResourceAudit")],
                DetectOps = [RegOp.CheckDword(Key, "EnableResourceAudit", 1)],
            },
            new TweakDef
            {
                Id = "entres-enforce-resource-access-policies",
                Label = "Enforce Centralized Access Policies for Enterprise Resource Management",
                Category = "System — Domain Trust",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Centralized access policy enforcement ensures that enterprise resource access decisions are made by the central policy engine rather than individual system ACLs which can become inconsistent over time. Centralized access policies allow organizations to define access rules based on user attributes resource sensitivity and context ensuring consistent enforcement across all relevant systems. Policy-based access control allows rapid updating of access rules during security incidents such as revoking access for compromised accounts across all resources simultaneously. Dynamic access control policies can incorporate contextual factors like device health network location and time of day into access decisions providing more nuanced and secure access control. Organizations should test centralized access policies in audit mode before enforcing them to identify access configurations that require adjustment. The policy engine should be highly available and integrated with directory services to ensure that access decisions can be made reliably.",
                Tags = ["enterprise-resources", "centralized-access", "dynamic-access-control", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceCentralizedAccessPolicies", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceCentralizedAccessPolicies")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceCentralizedAccessPolicies", 1)],
            },
            new TweakDef
            {
                Id = "entres-restrict-resource-sharing-to-domain",
                Label = "Restrict Enterprise Resource Sharing to Domain-Authenticated Sessions",
                Category = "System — Domain Trust",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Restricting enterprise resource sharing to domain-authenticated sessions prevents non-domain accounts from accessing enterprise resources shared through the enterprise resource manager. Non-domain accounts include local accounts workgroup accounts and external accounts that have not been validated through the organizational authentication infrastructure. Domain authentication requirement ensures that resource access is tied to organizational identity management which controls account lifecycle and credentials. Resources shared through enterprise resource manager can include network shares printers work resources and device access that should be limited to known and managed identities. The domain restriction applies the organizational security policy and access controls to resource sharing preventing circumvention through local account access. Organizations should audit resource sharing configurations to verify that domain authentication is required for all sensitive enterprise resources.",
                Tags = ["enterprise-resources", "domain-restriction", "authentication", "access-control", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSharingToDomain", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSharingToDomain")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSharingToDomain", 1)],
            },
            new TweakDef
            {
                Id = "entres-enable-data-classification-integration",
                Label = "Enable Data Classification Integration with Enterprise Resource Policy",
                Category = "System — Domain Trust",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Data classification integration allows enterprise resource policy to make access decisions based on the sensitivity classification of the data resource ensuring that highly classified resources have appropriately strict access controls. Resource access policies that incorporate data classification labels can automatically apply stricter controls to sensitive or regulated data without requiring manual policy configuration for each resource. Classification-aware access control ensures that as data sensitivity increases the access controls around that data automatically tighten to apply appropriate protections. Integration with data loss prevention systems allows classification labels to also drive DLP policy enforcement decisions for enterprise resources. Organizations should establish a consistent data classification taxonomy that aligns with their regulatory obligations and risk management requirements. Classification labels should be applied consistently and the automated policy enforcement should be tested to verify that classification-based access decisions function as expected.",
                Tags = ["enterprise-resources", "data-classification", "access-control", "compliance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableDataClassificationIntegration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDataClassificationIntegration")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDataClassificationIntegration", 1)],
            },
            new TweakDef
            {
                Id = "entres-configure-resource-manager-logging",
                Label = "Configure Verbose Logging for Enterprise Resource Manager Operations",
                Category = "System — Domain Trust",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Verbose logging for enterprise resource manager captures detailed operational events including policy evaluation access decisions and configuration changes providing comprehensive visibility into resource management operations. Detailed logs enable troubleshooting of access issues where users are denied access to resources they should have access to or granted access to resources they should not. Resource manager operation logs should include policy evaluation results that explain why access was granted or denied based on the attributes evaluated. Organizations should forward resource manager logs to centralized log management for correlation with other security and operational telemetry. Log retention for resource manager operations should be at least 90 days to support incident investigation and compliance auditing. Verbose logging has a minor performance impact on resource access operations and the verbosity level should be validated against performance requirements.",
                Tags = ["enterprise-resources", "logging", "operational-visibility", "troubleshooting", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableVerboseLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableVerboseLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableVerboseLogging", 1)],
            },
            new TweakDef
            {
                Id = "entres-enforce-resource-expiration-policy",
                Label = "Enforce Expiration Policy for Temporary Enterprise Resource Access Grants",
                Category = "System — Domain Trust",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Resource access expiration policy ensures that temporary access grants automatically expire after their configured lifetime preventing indefinite accumulation of access permissions that are no longer required. Just-in-time access grants should expire automatically after the authorized time period without requiring manual intervention to remove the access. Organizations that grant temporary elevated access for specific tasks or time-limited projects benefit from automatic expiration to prevent those grants from remaining active after the justification expires. Expiration of temporary access is a key principle of zero standing privilege architectures where no accounts maintain persistent access to sensitive resources. The policy should configure appropriate default expiration periods for different types of resource access and should alert administrators when access is about to expire for review. Organizations should regularly audit active access grants to identify any that have excessive lifetimes or that should be expired.",
                Tags = ["enterprise-resources", "access-expiration", "just-in-time", "least-privilege", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceAccessExpirationPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceAccessExpirationPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceAccessExpirationPolicy", 1)],
            },
            new TweakDef
            {
                Id = "entres-block-cross-tenant-resource-access",
                Label = "Block Enterprise Resource Access from External Tenant Identities",
                Category = "System — Domain Trust",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Blocking cross-tenant resource access prevents external organizational identities from accessing enterprise resources without explicit authorization through configured cross-tenant access policies. External tenant identities from partner organizations guest accounts and contractor identities from different tenants should only access enterprise resources when explicitly granted through formal access review processes. Unrestricted cross-tenant access can allow resources to be reached by compromised accounts from partner organizations that have not implemented equivalent security controls. The policy enforces that any cross-tenant access must be explicitly configured rather than allowed by default based on federation trust relationships. Organizations that have legitimate cross-tenant collaboration requirements should configure specific cross-tenant access policies that grant the minimum required access to known and trusted partner identities. Regular review of cross-tenant access grants ensures that access is revoked when business relationships and collaboration requirements change.",
                Tags = ["enterprise-resources", "cross-tenant", "external-access", "identity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockCrossTenantResourceAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockCrossTenantResourceAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockCrossTenantResourceAccess", 1)],
            },
            new TweakDef
            {
                Id = "entres-enforce-resource-location-restriction",
                Label = "Restrict Enterprise Resource Access to Organizational Network Locations",
                Category = "System — Domain Trust",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Network location restrictions for enterprise resource access limit access to authorized network locations such as corporate network subnets and VPN connections preventing access from arbitrary internet locations. Location-based access restrictions add a context-aware control layer that complements identity-based controls by requiring that access come from known trusted infrastructure. Compromised credentials used from external locations are blocked by network location restrictions providing detection and blocking of credential theft that is used away from corporate infrastructure. Organizations should configure location-based restrictions to include both physical office networks and VPN connections that remote workers and administrators use. The policy should be implemented alongside conditional access policies to provide a consistent location-aware access control framework. Regular review of allowed network locations ensures that the list remains current as network infrastructure changes.",
                Tags = ["enterprise-resources", "network-location", "conditional-access", "credential-theft", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceNetworkLocationRestriction", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceNetworkLocationRestriction")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceNetworkLocationRestriction", 1)],
            },
            new TweakDef
            {
                Id = "entres-enable-resource-health-monitoring",
                Label = "Enable Health Monitoring for Enterprise Resource Availability and Integrity",
                Category = "System — Domain Trust",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enterprise resource health monitoring tracks the availability integrity and performance of managed resources providing early warning of resource degradation ahead of user-impacting failures. Resource health data helps distinguish between security incidents that cause resource degradation and operational causes allowing appropriate response procedures to be initiated quickly. Monitoring for unexpected resource configuration changes provides detection for insider threat and attacker activity that modifies resource settings to expand access or disrupt operations. Automated health checks should verify that resource access controls are intact that resources are accessible to authorized users and that resource configurations match their expected baselines. Health monitoring alerts should be integrated with incident management and change management processes to ensure timely and appropriate responses. Regular health monitoring reviews help identify systemic issues in resource management that require architectural changes.",
                Tags = ["enterprise-resources", "health-monitoring", "availability", "integrity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableResourceHealthMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableResourceHealthMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "EnableResourceHealthMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "entres-enforce-resource-naming-standards",
                Label = "Enforce Naming Standards for Enterprise Resource Registration and Discovery",
                Category = "System — Domain Trust",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Resource naming standards enforcement ensures that enterprise resources conform to organizational naming conventions that encode ownership classification and lifecycle information in the resource name. Consistent naming conventions enable automated policy application based on resource names including applying different security policies based on naming conventions that indicate the resource's environment or sensitivity. Naming standard enforcement prevents the creation of resources with ambiguous or misleading names that could cause misapplication of security policies. The naming convention should include indicators for environment production vs. test sensitivity classification owning team or department and creation date. Automated validation of resource names against the naming standard at registration time prevents non-compliant resources from being added to the enterprise resource inventory. Regular scanning of existing resources for naming standard compliance helps identify legacy resources that require remediation.",
                Tags = ["enterprise-resources", "naming-standards", "governance", "policy-automation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceResourceNamingStandards", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceResourceNamingStandards")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceResourceNamingStandards", 1)],
            },
        ];
    }

    // ── EnterpriseStateRoamingPolicy ──
    private static class _EnterpriseStateRoamingPolicy
    {
        private const string SyncKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "esroam-disable-password-sync",
                Label = "Enterprise State Roaming: Disable Password Settings Sync",
                Category = "System — Domain Trust",
                Description =
                    "Prevents Windows credential and password hashes from being synchronized through the Enterprise State Roaming channel to Azure AD cloud storage. Password roaming via ESR is distinct from Azure AD seamless SSO and may involve credential material being persisted in a cloud-accessible store. In high-security environments, all credential handling must be on-premises only.",
                Tags = ["state roaming", "sync", "password", "credentials", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisablePasswordSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisablePasswordSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisablePasswordSettingSync", 2)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents credential data from roaming via ESR; complements Azure AD SSPR policy.",
            },
            new TweakDef
            {
                Id = "esroam-disable-app-sync-setting",
                Label = "Enterprise State Roaming: Disable App Sync via ESR Channel",
                Category = "System — Domain Trust",
                Description =
                    "Disables the specific ESR sync provider for application data packages (UWP AppX app state, configuration blobs stored in the cloud). App sync allows UWP apps to restore their last-used state—including user-typed data—when the same account signs in on another device. For apps that handle sensitive data (forms, documents), roaming this state creates residual data in Azure.",
                Tags = ["state roaming", "sync", "app data", "uwp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableAppSyncSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableAppSyncSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableAppSyncSettingSync", 2)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops UWP app state from syncing across devices; each device retains independent app state.",
            },
            new TweakDef
            {
                Id = "esroam-block-user-override-app-sync",
                Label = "Enterprise State Roaming: Block User Override of App Settings Sync Policy",
                Category = "System — Domain Trust",
                Description =
                    "Prevents users from overriding the Group Policy that disables application settings synchronization. The Windows sync settings UI allows users to individually toggle sync categories; this policy forces DisableApplicationSettingSync to be admin-enforced and uneditable, ensuring corporate devices cannot roam application configuration data regardless of user preference.",
                Tags = ["state roaming", "sync", "app settings", "user override", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableApplicationSettingSyncUserOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableApplicationSettingSyncUserOverride")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableApplicationSettingSyncUserOverride", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents user from re-enabling app sync; admin policy is the only control.",
            },
            new TweakDef
            {
                Id = "esroam-block-user-override-start-layout",
                Label = "Enterprise State Roaming: Block User Override of Start Layout Sync Policy",
                Category = "System — Domain Trust",
                Description =
                    "Prevents users from re-enabling Start menu layout synchronization after it has been disabled by Group Policy. In environments with GPO-deployed Start menus, users should not be able to revert to a cloud-synced layout that was potentially configured on a personal device or a different organizational unit, as this undermines the standardized desktop configuration management.",
                Tags = ["state roaming", "sync", "start menu", "user override", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableStartLayoutSettingSyncUserOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableStartLayoutSettingSyncUserOverride")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableStartLayoutSettingSyncUserOverride", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents user from re-enabling Start layout sync; enforces the IT-deployed Start menu.",
            },
            new TweakDef
            {
                Id = "esroam-disable-device-account-sync",
                Label = "Enterprise State Roaming: Disable Device Account Settings Sync",
                Category = "System — Domain Trust",
                Description =
                    "Disables synchronization of device account settings (Microsoft account email app state, mail account configuration, calendar sync settings) through Enterprise State Roaming. On managed corporate devices where mail clients are configured centrally via MDM profiles or Exchange Autodiscover, preventing cloud-roaming of account settings avoids conflicts between centrally-pushed and user-synced configurations.",
                Tags = ["state roaming", "sync", "device account", "email", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableDeviceAccountSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableDeviceAccountSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableDeviceAccountSettingSync", 2)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Stops device account (email/calendar config) from syncing; MDM-managed profiles are unaffected.",
            },
        ];
    }

    // ── GpoFolderRedirPolicy ──
    private static class _GpoFolderRedirPolicy
    {
        private const string SystemKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string LogonKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Logon";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "folderredir-enable-cache-rename",
                Label = "GPO Folder Redirection: Enable Cache Rename on Redirect",
                Category = "System — Domain Trust",
                Description =
                    "Enables cache renaming when a redirected folder path changes. When a folder redirection target is updated via Group Policy (e.g., moving a redirected My Documents share from an old file server to a new one), Windows can seamlessly rename the local offline-files cache entry to match the new UNC path. Without this setting, the client cache may retain stale entries pointing to the old server, causing offline file sync conflicts.",
                Tags = ["folder redirection", "offline files", "gpo", "cache", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "FolderRedirectionEnableCacheRename", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "FolderRedirectionEnableCacheRename")],
                DetectOps = [RegOp.CheckDword(SystemKey, "FolderRedirectionEnableCacheRename", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Enables clean cache transitions when redirection targets change; reduces sync errors after file server migrations.",
            },
            new TweakDef
            {
                Id = "folderredir-disable-user-profile-roaming",
                Label = "GPO Folder Redirection: Disable User Profile Roaming Download",
                Category = "System — Domain Trust",
                Description =
                    "Prevents Windows from downloading a roaming user profile from the network during logon when the user profile server is unavailable. Without this policy, Windows waits for the roaming profile to download (up to the profile server timeout) before allowing login. Blocking this fallback download prevents slow logons when profile servers are down while ensuring that local cached profiles are used immediately.",
                Tags = ["folder redirection", "roaming profile", "logon", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "DisableRoamingProfileDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "DisableRoamingProfileDownload")],
                DetectOps = [RegOp.CheckDword(SystemKey, "DisableRoamingProfileDownload", 1)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Skips roaming profile download when server unavailable; users get cached profile with no network wait.",
            },
            new TweakDef
            {
                Id = "folderredir-disable-roaming-profile-quota-notification",
                Label = "GPO Folder Redirection: Disable Roaming Profile Quota Warning Notification",
                Category = "System — Domain Trust",
                Description =
                    "Suppresses the roaming profile quota warning balloon notification that appears in the notification area when a user's roaming profile approaches its storage quota. In enterprise environments where profile size is managed through other mechanisms (e.g., folder redirection, profile monitoring tools), these notifications create user confusion and help desk calls without providing actionable guidance for end users.",
                Tags = ["folder redirection", "roaming profile", "quota", "notification", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "DisableProfileQuotaNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "DisableProfileQuotaNotification")],
                DetectOps = [RegOp.CheckDword(SystemKey, "DisableProfileQuotaNotification", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses quota balloon notification; profile quota is still enforced by the file server.",
            },
            new TweakDef
            {
                Id = "folderredir-wait-for-policy-at-logon",
                Label = "GPO Folder Redirection: Wait for Group Policy at Logon (Background Sync Block)",
                Category = "System — Domain Trust",
                Description =
                    "Forces Windows to perform a synchronous (blocking) Group Policy application at logon rather than applying folder redirection policies in the background after the user is already logged in. Without this setting, users may briefly see their unredirected local Desktop and Documents folders before the redirection takes effect, resulting in files being saved to the wrong location. Synchronous policy application eliminates this window.",
                Tags = ["folder redirection", "group policy", "logon", "synchronous", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "WaitForPolicyToBeApplied", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "WaitForPolicyToBeApplied")],
                DetectOps = [RegOp.CheckDword(SystemKey, "WaitForPolicyToBeApplied", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Adds ~2–5 seconds to first logon on each new GPO change; prevents split-second folder location confusion.",
            },
            new TweakDef
            {
                Id = "folderredir-redirect-local-profile-to-network",
                Label = "GPO Folder Redirection: Grant User Exclusive Rights to Redirected Folder",
                Category = "System — Domain Trust",
                Description =
                    "Grants the user exclusive NTFS permissions on their redirected folder target when the folder redirection policy first creates it on the file server. Without this setting, the Administrators group retains access to all redirected folders, enabling administrators to read user-redirected documents. Granting exclusive user rights is a privacy and security best practice that ensures sensitive user data in redirected folders is only accessible to the owning account.",
                Tags = ["folder redirection", "permissions", "ntfs", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "GrantExclusiveRightsToFolderRedirection", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "GrantExclusiveRightsToFolderRedirection")],
                DetectOps = [RegOp.CheckDword(SystemKey, "GrantExclusiveRightsToFolderRedirection", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Grants user-only NTFS permissions on their redirected share; admins must use elevated credentials to access.",
            },
            new TweakDef
            {
                Id = "folderredir-use-localized-subfolder-names",
                Label = "GPO Folder Redirection: Use Localized Subfolder Names",
                Category = "System — Domain Trust",
                Description =
                    "Configures folder redirection to use the localized (OS language-specific) names for redirected subfolders on the file server rather than English names. In multi-language organizations where different users log on with different Windows UI languages, the names of redirected subfolder paths (e.g., Documents vs. Documenti vs. Dokumente) can vary unless this policy standardizes them to the localized folder names per user.",
                Tags = ["folder redirection", "localization", "subfolder names", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "UseLocalizedSubfolderNames", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "UseLocalizedSubfolderNames")],
                DetectOps = [RegOp.CheckDword(SystemKey, "UseLocalizedSubfolderNames", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Changes redirected subfolder names to match the user's Windows language; no data loss, only path naming change.",
            },
            new TweakDef
            {
                Id = "folderredir-move-contents-on-redirect",
                Label = "GPO Folder Redirection: Move Contents to Redirected Path",
                Category = "System — Domain Trust",
                Description =
                    "Instructs Windows to automatically move the contents of a folder from its original local location to the new UNC redirect target when folder redirection is first applied. Without this policy, existing local files stay in place and only new files go to the redirect target, leaving users with data split across two locations. Enabling content migration ensures a complete transition to the managed file server path.",
                Tags = ["folder redirection", "content migration", "data move", "gpo", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "MoveContentsExistingFolders", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "MoveContentsExistingFolders")],
                DetectOps = [RegOp.CheckDword(SystemKey, "MoveContentsExistingFolders", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Moves existing local files to the redirect target on first policy application; ensure the target share has sufficient space.",
            },
            new TweakDef
            {
                Id = "folderredir-disable-unc-path-hardening-bypass",
                Label = "GPO Folder Redirection: Block UNC Hardening Bypass for Redirected Paths",
                Category = "System — Domain Trust",
                Description =
                    "Prevents applications from bypassing UNC path hardening (SMB signing requirements) for redirected folder UNC targets. Windows allows some UNC access to bypass signing requirements for specific paths. This policy ensures that even though folder redirection targets are trusted by Windows, they are still subject to SMB signing requirements to prevent man-in-the-middle attacks on the file server connection carrying redirected folder traffic.",
                Tags = ["folder redirection", "unc hardening", "smb signing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "DisableUNCHardeningForRedirectedPaths", 0)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "DisableUNCHardeningForRedirectedPaths")],
                DetectOps = [RegOp.CheckDword(SystemKey, "DisableUNCHardeningForRedirectedPaths", 0)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Keeps UNC hardening active for redirect paths (sets to 0 = hardening NOT disabled); SMB signing is enforced.",
            },
            new TweakDef
            {
                Id = "folderredir-configure-profile-slow-link-detection",
                Label = "GPO Folder Redirection: Configure Slow-Link Detection Threshold",
                Category = "System — Domain Trust",
                Description =
                    "Configures the network bandwidth threshold below which Windows considers the connection to the roaming profile server as a 'slow link', triggering use of the local cached profile instead of downloading the full remote profile. Setting this to the Microsoft-recommended value of 500 kbps ensures that even on moderate WAN links, users get fast logons while good connections still get the full roaming/redirected experience.",
                Tags = ["folder redirection", "slow link", "profile", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [LogonKey],
                ApplyOps = [RegOp.SetDword(LogonKey, "SlowLinkDetect", 500)],
                RemoveOps = [RegOp.DeleteValue(LogonKey, "SlowLinkDetect")],
                DetectOps = [RegOp.CheckDword(LogonKey, "SlowLinkDetect", 500)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Sets 500 kbps as slow-link threshold; connections below this use cached profile for faster logon.",
            },
            new TweakDef
            {
                Id = "folderredir-enable-profile-migration-on-domain-join",
                Label = "GPO Folder Redirection: Enable Local Profile Migration on Domain Join",
                Category = "System — Domain Trust",
                Description =
                    "Permits migration of the local user profile to the roaming profile path when a user first logs in after a machine is joined to a domain. Without this policy, domain logons create a new empty profile and the user's existing local profile data (desktop files, AppData settings) is left behind in the local profile. Enabling migration ensures the first domain logon seamlessly carries over all existing local user data.",
                Tags = ["folder redirection", "profile migration", "domain join", "logon", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [LogonKey],
                ApplyOps = [RegOp.SetDword(LogonKey, "LocalProfileLoadTimeOut", 30)],
                RemoveOps = [RegOp.DeleteValue(LogonKey, "LocalProfileLoadTimeOut")],
                DetectOps = [RegOp.CheckDword(LogonKey, "LocalProfileLoadTimeOut", 30)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Sets a 30-second wait period for profile load before falling back to local cache on domain join.",
            },
        ];
    }

    // ── GpoScriptsPolicy ──
    private static class _GpoScriptsPolicy
    {
        private const string SystemKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "gposcripts-run-logon-script-sync",
                Label = "GPO Scripts: Run Logon Scripts Synchronously",
                Category = "System — Domain Trust",
                Description =
                    "Configures Windows to run all Group Policy logon scripts synchronously and display the desktop only after all logon scripts have completed. By default, Windows may display the desktop before all logon scripts finish, which can result in users opening applications before drive mappings, printer connections, or environment variables are established by logon scripts. Synchronous execution ensures scripts complete before the user session is accessible.",
                Tags = ["gpo scripts", "logon scripts", "synchronous", "startup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "RunLogonScriptSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "RunLogonScriptSync")],
                DetectOps = [RegOp.CheckDword(SystemKey, "RunLogonScriptSync", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Delays desktop display until all logon scripts complete; increases first-logon time by the duration of all scripts.",
            },
            new TweakDef
            {
                Id = "gposcripts-run-startup-script-sync",
                Label = "GPO Scripts: Run Startup Scripts Synchronously",
                Category = "System — Domain Trust",
                Description =
                    "Configures Windows to run all Computer Configuration startup scripts one at a time and sequentially before the logon prompt appears. Without this setting, startup scripts may run asynchronously in the background, meaning critical system initialization scripts (e.g., disk encryption unlock, certificate enrollment, MDM check-in) may not complete before a user logs in, potentially resulting in incomplete system state at logon.",
                Tags = ["gpo scripts", "startup scripts", "synchronous", "boot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "RunStartupScriptSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "RunStartupScriptSync")],
                DetectOps = [RegOp.CheckDword(SystemKey, "RunStartupScriptSync", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote =
                    "Ensures each startup script finishes before the next begins and before the logon screen appears; adds boot time equal to total startup script duration.",
            },
            new TweakDef
            {
                Id = "gposcripts-run-legacy-logon-hidden",
                Label = "GPO Scripts: Run Legacy Logon Scripts Visible but Silent",
                Category = "System — Domain Trust",
                Description =
                    "Forces legacy logon scripts (those defined in the user profile properties of Active Directory) to run visible to the user but without a separate CMD window. By default, legacy logon scripts (as distinct from Group Policy logon scripts) may flash console windows briefly. This policy suppresses the command prompt window while still allowing the script to run, providing a cleaner logon experience without confusing users with flashing black windows.",
                Tags = ["gpo scripts", "logon scripts", "legacy", "hidden window", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "RunLegacyLogonScriptHidden", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "RunLegacyLogonScriptHidden")],
                DetectOps = [RegOp.CheckDword(SystemKey, "RunLegacyLogonScriptHidden", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses a CMD window during legacy logon scripts; scripts still execute — purely a UX improvement.",
            },
            new TweakDef
            {
                Id = "gposcripts-set-max-script-wait-time",
                Label = "GPO Scripts: Set Maximum Script Runtime Timeout (10 minutes)",
                Category = "System — Domain Trust",
                Description =
                    "Sets the maximum time Windows will wait for a Group Policy script (startup, logon, logoff, or shutdown) to complete before forcibly terminating it. The default is 600 seconds (10 minutes). Scripts that exceed this timeout are terminated without completing. Setting this explicitly prevents runaway scripts from hanging the logon/logoff sequence indefinitely, which can leave the machine in an unresponsive state.",
                Tags = ["gpo scripts", "timeout", "max wait", "startup shutdown", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "MaxGPOScriptWait", 600)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "MaxGPOScriptWait")],
                DetectOps = [RegOp.CheckDword(SystemKey, "MaxGPOScriptWait", 600)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Explicitly sets the 600-second (10 min) script timeout; runaway scripts are forcibly terminated after this period.",
            },
            new TweakDef
            {
                Id = "gposcripts-hide-startup-scripts",
                Label = "GPO Scripts: Hide Startup Scripts (No Status Message)",
                Category = "System — Domain Trust",
                Description =
                    "Suppresses the 'Running startup scripts...' status message and progress screen that Windows displays during boot when Group Policy startup scripts are executing. In environments where startup scripts handle sensitive operations (certificate enrollment, TPM initialization commands, encrypted volume mounting), displaying the script status messages onscreen may expose the types of security operations to anyone observing the boot screen.",
                Tags = ["gpo scripts", "startup scripts", "hidden", "status message", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "HideStartupScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "HideStartupScripts")],
                DetectOps = [RegOp.CheckDword(SystemKey, "HideStartupScripts", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the startup script progress screen; scripts still run, but no progress message is shown at boot.",
            },
            new TweakDef
            {
                Id = "gposcripts-hide-logon-scripts",
                Label = "GPO Scripts: Hide Logon Scripts (No Progress Window)",
                Category = "System — Domain Trust",
                Description =
                    "Hides the 'Applying your personal settings...' and similar logon script progress messages that appear during user logon in verbose mode. While informative for administrators, these messages reveal that Group Policy logon scripts are running, potentially exposing script categories to end users. In secure environments, the logon process should be opaque — completing silently and presenting the desktop only when ready.",
                Tags = ["gpo scripts", "logon scripts", "hidden", "progress window", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "HideLogonScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "HideLogonScripts")],
                DetectOps = [RegOp.CheckDword(SystemKey, "HideLogonScripts", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses the logon script progress window; logon may appear to 'hang' briefly — this is expected behavior.",
            },
            new TweakDef
            {
                Id = "gposcripts-hide-logoff-scripts",
                Label = "GPO Scripts: Hide Logoff Scripts (No Logoff Window)",
                Category = "System — Domain Trust",
                Description =
                    "Suppresses the window that appears when Group Policy logoff scripts are executing at user sign-out. When logoff scripts clean up user sessions (removing temp credentials, wiping browser profiles, revoking certificates), showing the progress window to the user is unnecessary and can lead users to terminate the logoff early by pressing the power button, potentially leaving cleanup scripts incomplete.",
                Tags = ["gpo scripts", "logoff scripts", "hidden", "sign-out", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "HideLogoffScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "HideLogoffScripts")],
                DetectOps = [RegOp.CheckDword(SystemKey, "HideLogoffScripts", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides logoff script progress; users see the default 'Signing out...' overlay while scripts complete in the background.",
            },
            new TweakDef
            {
                Id = "gposcripts-hide-shutdown-scripts",
                Label = "GPO Scripts: Hide Shutdown Scripts (Silent System Shutdown)",
                Category = "System — Domain Trust",
                Description =
                    "Suppresses the shutdown script progress window that shows when Group Policy Computer Configuration shutdown scripts run during system power-down. Shutdown scripts commonly perform operations such as disk encryption key cleanup, network session teardown, and compliance logging. Hiding the progress window provides a cleaner shutdown experience and prevents disclosure of the shutdown script sequence to onlookers.",
                Tags = ["gpo scripts", "shutdown scripts", "hidden", "system shutdown", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "HideShutdownScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "HideShutdownScripts")],
                DetectOps = [RegOp.CheckDword(SystemKey, "HideShutdownScripts", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides shutdown script progress; the machine may appear to shut down more slowly as scripts run silently.",
            },
            new TweakDef
            {
                Id = "gposcripts-run-scripts-first-at-user-logon",
                Label = "GPO Scripts: Run User Logon Scripts Before Group Policy Logon Scripts",
                Category = "System — Domain Trust",
                Description =
                    "Forces user-level logon scripts (defined in profile properties) to run before the Group Policy client completes processing Computer and User Configuration logon scripts. In some deployment scenarios, user-specific scripts (which map personal drives or configure user-specific settings) must run before broader GPO changes are applied. This ordering ensures user context is established before group-level policies modify the environment.",
                Tags = ["gpo scripts", "logon scripts", "run order", "user scripts", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "RunScriptsFirstAtUserLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "RunScriptsFirstAtUserLogon")],
                DetectOps = [RegOp.CheckDword(SystemKey, "RunScriptsFirstAtUserLogon", 1)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote =
                    "Reorders script execution — user profile scripts run before GPO logon scripts; may expose environment to user scripts before GPO lockdown is applied.",
            },
            new TweakDef
            {
                Id = "gposcripts-set-max-noninteractive-runtime",
                Label = "GPO Scripts: Set Maximum Non-Interactive Script Runtime (5 minutes)",
                Category = "System — Domain Trust",
                Description =
                    "Sets the maximum time a non-interactive Group Policy script (startup, shutdown, logon, logoff scripts running in non-interactive mode) is allowed to run. Setting this to 300 seconds (5 minutes) provides a tighter timeout than the default 600 seconds. For background scripts that should complete quickly, this reduces the window during which a script error or infinite loop delays the logon or shutdown sequence.",
                Tags = ["gpo scripts", "timeout", "non-interactive", "runtime limit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "MaxNonInteractiveRunningTime", 300)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "MaxNonInteractiveRunningTime")],
                DetectOps = [RegOp.CheckDword(SystemKey, "MaxNonInteractiveRunningTime", 300)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Sets a 5-minute (300s) cap on non-interactive scripts; scripts that need more time must run interactively.",
            },
        ];
    }
}
