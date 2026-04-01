// RegiLattice.Core — Tweaks/PolicyEnterprise.cs
// Azure AD, ADFS, Intune/MDM, Autopilot, deployment, flighting, insider builds, enterprise GPO, and managed environment policies
// Category: "Enterprise Management Policy"
// Consolidated from 33 modules.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyEnterprise
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
                    Id          = "adfspol-enable-extranet-lockout",
                    Label       = "Enable ADFS Extranet Smart Lockout",
                    Category = "System",
                    Description = "Sets EnableExtranetLockout=1 in the ADFS policy. Activates ADFS Extranet Smart Lockout (ESL) which tracks authentication attempts from extranet (external) IP addresses separately from intranet ones. Extranet lockout prevents password spray and brute-force attacks from the internet from locking out Active Directory accounts while still allowing internal users to authenticate normally.",
                    Tags        = ["adfs", "extranet", "lockout", "brute-force", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote  = "Activates smart lockout for extranet auth; requires ADFS to be deployed with WAPProxy for effect.",
                    ApplyOps    = [RegOp.SetDword(AdfsKey, "EnableExtranetLockout", 1)],
                    RemoveOps   = [RegOp.DeleteValue(AdfsKey, "EnableExtranetLockout")],
                    DetectOps   = [RegOp.CheckDword(AdfsKey, "EnableExtranetLockout", 1)],
                },
                new TweakDef
                {
                    Id          = "adfspol-set-extranet-lockout-threshold",
                    Label       = "Set ADFS Extranet Lockout Threshold (5 attempts)",
                    Category = "System",
                    Description = "Sets ExtranetLockoutThreshold=5 in the ADFS policy. Defines the number of failed authentication attempts from an extranet IP address before ADFS blocks further attempts from that IP. Five failed attempts is the CIS recommendation that balances security against accidental account lockout from mistyped passwords on shared IP networks (NAT, VPN exit nodes).",
                    Tags        = ["adfs", "extranet", "lockout", "threshold", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote  = "Blocks extranet IPs after 5 failures; corporate NAT exit nodes may need threshold adjustment.",
                    ApplyOps    = [RegOp.SetDword(AdfsKey, "ExtranetLockoutThreshold", 5)],
                    RemoveOps   = [RegOp.DeleteValue(AdfsKey, "ExtranetLockoutThreshold")],
                    DetectOps   = [RegOp.CheckDword(AdfsKey, "ExtranetLockoutThreshold", 5)],
                },
                new TweakDef
                {
                    Id          = "adfspol-disable-endpoint-wia-fallback",
                    Label       = "Disable ADFS Windows Integrated Auth Fallback",
                    Category = "System",
                    Description = "Sets DisableWIAFallback=1 in the ADFS policy. Prevents ADFS from falling back to Windows Integrated Authentication (Kerberos/NTLM from browser) when the primary authentication method fails. WIA fallback can expose NTLM credentials when users authenticate from non-domain-joined browsers, potentially enabling NTLM relay attacks. Disabling fallback forces explicit form-based or certificate authentication.",
                    Tags        = ["adfs", "wia", "fallback", "ntlm", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote  = "Prevents WIA fallback; Intranet users who previously used WIA from non-domain browsers must use forms instead.",
                    ApplyOps    = [RegOp.SetDword(AdfsKey, "DisableWIAFallback", 1)],
                    RemoveOps   = [RegOp.DeleteValue(AdfsKey, "DisableWIAFallback")],
                    DetectOps   = [RegOp.CheckDword(AdfsKey, "DisableWIAFallback", 1)],
                },
                new TweakDef
                {
                    Id          = "adfspol-require-ssl-certificate-auth",
                    Label       = "Require TLS Certificate Authentication for ADFS Service",
                    Category = "System",
                    Description = "Sets RequireCertificateAuthentication=1 in the ADFS service Parameters key. Enforces mutual TLS certificate authentication for ADFS service account communication. When mutual TLS is required the ADFS service will reject connections from components (proxy servers, relying party trusts) that do not present a valid certificate, preventing impersonation of trusted federation endpoints.",
                    Tags        = ["adfs", "tls", "certificate", "mutual-auth", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote  = "Requires mutual TLS from all ADFS-connecting components; ensure proxies and RPs have valid certificates.",
                    ApplyOps    = [RegOp.SetDword(SvcKey, "RequireCertificateAuthentication", 1)],
                    RemoveOps   = [RegOp.DeleteValue(SvcKey, "RequireCertificateAuthentication")],
                    DetectOps   = [RegOp.CheckDword(SvcKey, "RequireCertificateAuthentication", 1)],
                },
                new TweakDef
                {
                    Id          = "adfspol-enable-oauth-pkce",
                    Label       = "Require PKCE for ADFS OAuth2 Authorization Code Flow",
                    Category = "System",
                    Description = "Sets RequirePKCEForOAuth=1 in the ADFS policy. Enforces Proof Key for Code Exchange (PKCE, RFC 7636) for all OAuth 2.0 authorization code flow requests to ADFS. PKCE prevents authorization code interception attacks where an attacker intercepts the authorization code redirect and exchanges it for tokens. Required by RFC 9700 (OAuth 2.0 Security Best Current Practice) for all public and confidential clients.",
                    Tags        = ["adfs", "oauth", "pkce", "authorization-code", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote  = "Requires PKCE; legacy OAuth clients that do not send a code_challenge will be rejected.",
                    ApplyOps    = [RegOp.SetDword(AdfsKey, "RequirePKCEForOAuth", 1)],
                    RemoveOps   = [RegOp.DeleteValue(AdfsKey, "RequirePKCEForOAuth")],
                    DetectOps   = [RegOp.CheckDword(AdfsKey, "RequirePKCEForOAuth", 1)],
                },
                new TweakDef
                {
                    Id          = "adfspol-disable-device-auth-bypass",
                    Label       = "Disable ADFS Device Authentication Bypass",
                    Category = "System",
                    Description = "Sets DisableDeviceAuthenticationBypass=1 in the ADFS policy. Prevents ADFS from bypassing multi-factor authentication requirements based solely on device registration status. When disabled, a registered device alone is not sufficient to skip MFA — users must still satisfy the full authentication policy. This closes a gap where attackers who enroll a stolen device could bypass step-up authentication.",
                    Tags        = ["adfs", "device-auth", "mfa", "bypass", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote  = "Device registration no longer bypasses MFA; compliant device policies may need adjustment for Conditional Access.",
                    ApplyOps    = [RegOp.SetDword(AdfsKey, "DisableDeviceAuthenticationBypass", 1)],
                    RemoveOps   = [RegOp.DeleteValue(AdfsKey, "DisableDeviceAuthenticationBypass")],
                    DetectOps   = [RegOp.CheckDword(AdfsKey, "DisableDeviceAuthenticationBypass", 1)],
                },
                new TweakDef
                {
                    Id          = "adfspol-set-token-replay-detection",
                    Label       = "Enable ADFS Token Replay Detection",
                    Category = "System",
                    Description = "Sets EnableTokenReplayDetection=1 in the ADFS policy. Activates the ADFS token replay detection cache which records recently used security tokens and rejects any attempt to present the same token a second time. Token replay attacks occur when an attacker intercepts a SAML assertion or JWT and submits it to gain access. Detection is critical for federated SSO scenarios where tokens flow through multiple network intermediaries.",
                    Tags        = ["adfs", "token-replay", "detection", "saml", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote  = "Enables token replay cache; negligible performance impact on ADFS server under normal SSO load.",
                    ApplyOps    = [RegOp.SetDword(AdfsKey, "EnableTokenReplayDetection", 1)],
                    RemoveOps   = [RegOp.DeleteValue(AdfsKey, "EnableTokenReplayDetection")],
                    DetectOps   = [RegOp.CheckDword(AdfsKey, "EnableTokenReplayDetection", 1)],
                },
                new TweakDef
                {
                    Id          = "adfspol-require-extended-protection",
                    Label       = "Require Extended Protection for ADFS Authentication",
                    Category = "System",
                    Description = "Sets EnableExtendedProtection=1 in the ADFS authentication policy. Enables Extended Protection for Authentication (EPA) which binds the Windows authentication handshake to the TLS channel. EPA prevents NTLM relay attacks where an attacker forwards authentication attempts to the ADFS endpoint from a man-in-the-middle position. Supported in all Windows versions since Windows 7 SP1.",
                    Tags        = ["adfs", "extended-protection", "ntlm-relay", "authentication", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote  = "Requires EPA TLS channel binding; clients that do not support EPA (pre-Vista) will fail WIA authentication.",
                    ApplyOps    = [RegOp.SetDword(AuthKey, "EnableExtendedProtection", 1)],
                    RemoveOps   = [RegOp.DeleteValue(AuthKey, "EnableExtendedProtection")],
                    DetectOps   = [RegOp.CheckDword(AuthKey, "EnableExtendedProtection", 1)],
                },
                new TweakDef
                {
                    Id          = "adfspol-disable-prompt-login",
                    Label       = "Disable ADFS Prompt=Login Re-Authentication Bypass",
                    Category = "System",
                    Description = "Sets DisablePromptLoginHandling=1 in the ADFS policy. Prevents ADFS from honouring the OAuth/OIDC prompt=login parameter which forces a fresh login regardless of existing SSO session. While useful for applications needing fresh credentials, this parameter can be abused by attackers to force users into repeated phishing-susceptible login flows. Disabling allows ADFS to enforce its own session management instead.",
                    Tags        = ["adfs", "oauth", "prompt-login", "session", "hardening"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote  = "Ignores prompt=login; applications that need forced re-auth must use access token expiry instead.",
                    ApplyOps    = [RegOp.SetDword(AdfsKey, "DisablePromptLoginHandling", 1)],
                    RemoveOps   = [RegOp.DeleteValue(AdfsKey, "DisablePromptLoginHandling")],
                    DetectOps   = [RegOp.CheckDword(AdfsKey, "DisablePromptLoginHandling", 1)],
                },
                new TweakDef
                {
                    Id          = "adfspol-enable-audit-events",
                    Label       = "Enable ADFS Security Audit Events",
                    Category = "System",
                    Description = "Sets AuditFlags=1 in the ADFS policy. Instructs ADFS to write security audit events to the Windows Security event log for all federation authentication requests, token issuances, and extranet lockout events. ADFS audit events (Event IDs 1200, 1201, 411, 412) are essential for detecting password spray attacks, compromised account usage, and abnormal token issuance patterns in a federated identity environment.",
                    Tags        = ["adfs", "audit", "events", "security-log", "compliance"],
                    NeedsAdmin  = true,
                    CorpSafe    = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote  = "Enables ADFS audit events in the Security log; increases log volume proportional to federation traffic.",
                    ApplyOps    = [RegOp.SetDword(AdfsKey, "AuditFlags", 1)],
                    RemoveOps   = [RegOp.DeleteValue(AdfsKey, "AuditFlags")],
                    DetectOps   = [RegOp.CheckDword(AdfsKey, "AuditFlags", 1)],
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
                    Id = "addsvc-require-strong-dc-channel",
                    Label = "AD Services: Require Sign-and-Seal (Strong) Secure Channel to DC",
                    Category = "System",
                    Description =
                        "Sets RequireSignOrSeal=1 in Netlogon\\Parameters. Requires all Netlogon secure channel traffic between this workstation and domain controllers to be both signed and sealed (encrypted). The Netlogon secure channel carries authentication traffic, machine account password changes, and group policy downloads. If unsigned and unencrypted, the secure channel is susceptible to the Zerologon vulnerability (CVE-2020-1472) and earlier Netlogon protocol attacks that allow privilege escalation to Domain Admin. Requiring sign-and-seal ensures all Netlogon traffic is integrity-protected and encrypted.",
                    Tags = ["netlogon", "secure-channel", "zerologon", "cve-2020-1472", "sign-seal"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Netlogon secure channel requires sign-and-seal. Mitigates Zerologon and Netlogon protocol downgrade attacks. No user-visible impact — all modern Windows DCs support sign-and-seal. May block authentication if older (pre-Windows 2000) DCs exist in the domain.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "RequireSignOrSeal", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "RequireSignOrSeal")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "RequireSignOrSeal", 1)],
                },
                new TweakDef
                {
                    Id = "addsvc-enable-secure-channel-sealing",
                    Label = "AD Services: Enable Netlogon Secure Channel Encryption (Seal)",
                    Category = "System",
                    Description =
                        "Sets SealSecureChannel=1 in Netlogon\\Parameters. Enables encryption (sealing) of the Netlogon secure channel in addition to signing. While RequireSignOrSeal=1 ensures integrity, this setting specifically ensures confidentiality — the channel content is encrypted and cannot be captured by network eavesdropping. Together, signing and sealing provide authenticated-and-encrypted communication between clients and domain controllers for all Netlogon protocol messages, including machine account password refresh operations.",
                    Tags = ["netlogon", "seal", "encryption", "secure-channel", "confidentiality"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Netlogon channel encryption enabled. Works in conjunction with signing (RequireSignOrSeal). No visible impact to users or applications — encryption is handled transparently by the Netlogon service. Requires DCs to support secure channel encryption, which all Windows Server 2000+ DCs support.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "SealSecureChannel", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "SealSecureChannel")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "SealSecureChannel", 1)],
                },
                new TweakDef
                {
                    Id = "addsvc-set-machine-password-age-30days",
                    Label = "AD Services: Set Machine Account Password Rotation Interval to 30 Days",
                    Category = "System",
                    Description =
                        "Sets MaximumPasswordAge=30 in Netlogon\\Parameters (units: days). Sets the maximum age of the machine account password to 30 days, after which Netlogon automatically requests a new password from the DC. Machine account passwords authenticate the workstation to the domain (used in Netlogon secure channel setup and Kerberos S4U2Proxy). An attacker who compromises a machine account password can perform Pass-the-Hash or Silver Ticket attacks using the machine account's Kerberos hash. Frequent rotation limits the attacker's window of opportunity. Default is 30 days; explicitly setting this prevents GPO drift to longer values.",
                    Tags = ["machine-account", "password-rotation", "netlogon", "silver-ticket", "s4u"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Machine account password rotated every 30 days automatically by Netlogon. This is the default behaviour; explicitly setting it prevents accidental policy drift. No user-visible impact. Machine account password changes are transparent. Disable rotation only for specific reasons (domain join cloning scenarios) via DisablePasswordChange.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "MaximumPasswordAge", 30)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "MaximumPasswordAge")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "MaximumPasswordAge", 30)],
                },
                new TweakDef
                {
                    Id = "addsvc-enable-machine-account-password-change",
                    Label = "AD Services: Enable Automatic Machine Account Password Rotation",
                    Category = "System",
                    Description =
                        "Sets DisablePasswordChange=0 in Netlogon\\Parameters. Explicitly enables automatic machine account password changes (sets the DisablePasswordChange flag to 0 = do NOT disable). Automatic machine account password rotation is a security feature — some organisations disable it to prevent 'secure channel' credential staleness in certain edge cases (e.g., VDI golden image re-deployment). Disabling rotation means the machine's Active Directory password stays static indefinitely, making it a persistent credential that is more valuable to attackers and never expires. Explicitly enabling rotation is a defence-in-depth measure.",
                    Tags = ["machine-account", "password-rotation", "disable-prevention", "netlogon"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Machine account automatic password change enabled (DisablePasswordChange=0). This is the default Windows behaviour. If your environment uses VDI clones or domain-joined VM templates, ensure the VDI solution handles machine account conflicts via SysPrep or equivalent before enforcing.",
                    ApplyOps = [RegOp.SetDword(NetlogonKey, "DisablePasswordChange", 0)],
                    RemoveOps = [RegOp.DeleteValue(NetlogonKey, "DisablePasswordChange")],
                    DetectOps = [RegOp.CheckDword(NetlogonKey, "DisablePasswordChange", 0)],
                },
                new TweakDef
                {
                    Id = "addsvc-restrict-ntlm-outbound-to-trusted-servers",
                    Label = "AD Services: Restrict Outbound NTLM to Domain-Trusted Servers Only",
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Id = "avd-disable-clipboard-redirect",
                    Label = "AVD: Disable Clipboard Redirection in Sessions",
                    Category = "System",
                    Description =
                        "Sets fDisableClip=1. Blocks bidirectional clipboard redirection between the AVD session and the client device. Clipboard is a primary data exfiltration vector in VDI environments: users copy sensitive data from the session and paste it outside the controlled environment. Disabling clipboard redirection is a key DLP control for finance, healthcare, and legal VDI deployments. Some AVD workflows may require clipboard for productivity; evaluate per use-case.",
                    Tags = ["avd", "clipboard", "dlp", "data-exfiltration", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Prevents copy/paste between session and client. Significant productivity impact for workflows requiring copy of data from session.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisableClip", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableClip")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisableClip", 1)],
                },
                new TweakDef
                {
                    Id = "avd-disable-drive-redirect",
                    Label = "AVD: Disable Drive Redirection in Sessions",
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
                    Description =
                        "Sets EnablePrivateMode=1. Activates AVD Private Mode which restricts session actions to reduce data leakage risk: disables local clipboard, file transfers, printing to local printers, and local drive access in a single policy. Private Mode is designed for shared/kiosk session hosts in sensitive environments where multiple users share the same session host profile. Equivalent to enabling fDisableClip + fDisableCdm + printer restrictions together.",
                    Tags = ["avd", "private-mode", "kiosk", "dlp", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Restricts all peripheral redirection. Not suitable for productivity use-cases requiring local file access or printing.",
                    ApplyOps = [RegOp.SetDword(TsKey, "EnablePrivateMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "EnablePrivateMode")],
                    DetectOps = [RegOp.CheckDword(TsKey, "EnablePrivateMode", 1)],
                },
                new TweakDef
                {
                    Id = "avd-set-rdp-security-layer",
                    Label = "AVD: Enforce TLS 1.2+ for RDP Transport Layer",
                    Category = "System",
                    Description =
                        "Sets SecurityLayer=2 (TLS). Forces the Remote Desktop Protocol transport layer to use SSL/TLS 1.2 or later for all connections to AVD session hosts. Value 0 = RDP legacy (cleartext-vulnerable), value 1 = negotiate (downgrade possible), value 2 = TLS required. In Azure, the network path is encrypted at the Azure backbone level; however, enforcing TLS at the RDP layer provides defence-in-depth and satisfies compliance requirements for encrypted-in-transit data.",
                    Tags = ["avd", "rdp", "tls", "encryption", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Requires TLS; breaks connections from very old RDP clients that cannot negotiate TLS. All modern clients support TLS.",
                    ApplyOps = [RegOp.SetDword(TsKey, "SecurityLayer", 2)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "SecurityLayer")],
                    DetectOps = [RegOp.CheckDword(TsKey, "SecurityLayer", 2)],
                },
                new TweakDef
                {
                    Id = "avd-require-nla",
                    Label = "AVD: Require Network Level Authentication for Sessions",
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
        private const string CloudPcKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\CloudPC";

        private const string TsKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cloudpc-enable-udp-shortpath",
                    Label = "Cloud PC: Enable UDP ShortPath for Low-Latency Transport",
                    Category = "System",
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
                    Category = "System",
                    Description =
                        "Sets TeamsMeetingUnmuteOnEntry=0 and related Teams policy keys. Activates Teams audio/video media optimization for Windows 365 Cloud PCs, which redirects media processing from the Cloud PC CPU to the local client device. Without media optimization, Teams calls are processed server-side, consuming Cloud PC vCPU and causing high latency. With optimization, HD video calls run at near-native quality on the client while the Cloud PC CPU overhead drops by 70–90%.",
                    Tags = ["cloudpc", "teams", "media-redirect", "av", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Requires Teams client v1.4+ and Windows App/MSTSC v1.2.3004+. Older clients fall back to server-side processing without error.",
                    ApplyOps =
                        [
                            RegOp.SetDword(TsKey, "fEnableTeamsHdxVideoOptimization", 1),
                        ],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fEnableTeamsHdxVideoOptimization")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fEnableTeamsHdxVideoOptimization", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-disable-printer-redirect",
                    Label = "Cloud PC: Disable Client Printer Redirection",
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
                    Description =
                        "Sets fPromptForPassword=1. Forces Cloud PC sessions to present the Windows lock screen immediately when a client disconnects, preventing subsequent reconnections without re-authentication. Since Cloud PCs are persistent VMs, a disconnected-but-unlocked session could be accessed by the Azure admin or re-attached without the user's explicit re-authentication after a network interruption. Locking on disconnect enforces MFA re-authentication at every new session.",
                    Tags = ["cloudpc", "lock-screen", "authentication", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Requires MFA re-authentication on every reconnect. Slightly increases session resume time for Teams and app continuity.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fPromptForPassword", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fPromptForPassword")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fPromptForPassword", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-session-time-limit-8h",
                    Label = "Cloud PC: Set Maximum Active Session Duration to 8 Hours",
                    Category = "System",
                    Description =
                        "Sets MaxConnectionTime=28800000 (8 hours in ms). Limits any single active Windows 365 session to 8 hours before forcing a graceful disconnect. Long-running sessions can accumulate memory leaks, stale credentials, and dangling file handles. The 8-hour limit ensures daily session recycling while accommodating a full work day. Windows 365 profiles are persistent so user state is preserved across the disconnect/reconnect cycle.",
                    Tags = ["cloudpc", "session-limit", "time-limit", "maintenance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Users are gracefully disconnected after 8 hours. Unsaved work may be lost if auto-save is not configured. Windows gives a warning before disconnect.",
                    ApplyOps = [RegOp.SetDword(TsKey, "MaxConnectionTime", 28800000)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "MaxConnectionTime")],
                    DetectOps = [RegOp.CheckDword(TsKey, "MaxConnectionTime", 28800000)],
                },
                new TweakDef
                {
                    Id = "cloudpc-disable-audio-record-redirect",
                    Label = "Cloud PC: Disable Microphone Redirection in Sessions",
                    Category = "System",
                    Description =
                        "Sets fDisableAudioCapture=1. Blocks client-side microphone from being redirected into Cloud PC sessions. Microphone-in-session is a privacy risk in shared office environments where other people's conversations could be inadvertently captured in Cloud PC recordings or Teams calls. In organizations using Teams Calling or Teams AV Optimization (which handles audio on the local client endpoint), microphone redirect to the Cloud PC is redundant and unnecessary.",
                    Tags = ["cloudpc", "microphone", "audio", "privacy", "redirect"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks in-session microphone access. Users using Teams AV Optimization are unaffected as audio is processed locally.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisableAudioCapture", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableAudioCapture")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisableAudioCapture", 1)],
                },
                new TweakDef
                {
                    Id = "cloudpc-enable-display-quality-max",
                    Label = "Cloud PC: Set Maximum Visual Quality Level for Display",
                    Category = "System",
                    Description =
                        "Sets VisualQuality=3 (medium-high). Sets the Cloud PC RDP display quality to the highest persistent level. Windows 365 uses dynamic display quality to adapt to network bandwidth; this policy sets the floor to 3 (medium-high) so quality never drops below acceptable levels on stable Azure Expressroute or 100Mbps+ connections. Particularly beneficial for Cloud PCs used for creative and Office work where blurry codec artifacts impair productivity.",
                    Tags = ["cloudpc", "display-quality", "rdp", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Higher baseline quality uses more bandwidth (~5–10 Mbps sustained). Not recommended for Cloud PCs accessed over mobile/4G connections.",
                    ApplyOps = [RegOp.SetDword(TsKey, "VisualQuality", 3)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "VisualQuality")],
                    DetectOps = [RegOp.CheckDword(TsKey, "VisualQuality", 3)],
                },
                new TweakDef
                {
                    Id = "cloudpc-disable-device-redirect",
                    Label = "Cloud PC: Disable PnP Device Redirection into Sessions",
                    Category = "System",
                    Description =
                        "Sets fDisablePNPRedir=1. Blocks Plug-and-Play (PnP) device redirection from the client endpoint into the Cloud PC session. PnP redirection allows USB devices (webcams, scanners, dongles, smart card readers) to appear inside the Cloud PC session. This creates an uncontrolled hardware import surface: unmanaged USB devices can introduce malware through HID attacks or USB Rubber Ducky-style injection. Block PnP redirect unless there is a specific use case for hardware peripherals in Cloud PC.",
                    Tags = ["cloudpc", "usb", "pnp", "device-redirect", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "USB/PnP devices are not available inside Cloud PC sessions. Smart card readers for local authentication are unaffected if using NLA.",
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
                Category = "System",
                Description = "Sets RequireScriptCodeSigning=1 in ConfigurationManager policy. Requires that any script (PowerShell, VBScript, JScript) deployed through the Configuration Manager client for task sequences or application deployment must be digitally signed by a certificate trusted by the client's root store before execution. " +
                    "Configuration Manager script execution is a primary lateral movement vector in enterprise environments. A compromised management server or a rogue admin with deployment rights can push arbitrary scripts to all managed clients. Without code signing enforcement, any script pushed through ConfigMgr is executed verbatim. Requiring script code signing ensures only scripts signed by the enterprise PKI certificate authority are executed.",
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
                Category = "System",
                Description = "Sets EnableClientAuditLogging=1 in ConfigurationManager policy. Enables detailed audit logging in the Configuration Manager client agent, causing all deployment operations (software installs, uninstalls, state machine transitions, inventory collection, policy downloads) to be recorded in the Security event log in addition to the standard ccmsetup.log files. " +
                    "The default ConfigMgr client logging writes verbose detail to log files under C:\\Windows\\CCM\\Logs\\ but does not generate Security event log entries auditable by a SIEM. With audit logging enabled, Security events are generated for every ConfigMgr operation, enabling correlation with Active Directory logon events, PowerShell execution events, and process creation events during incident investigations. This enables detection of ConfigMgr-based lateral movement.",
                Tags = ["configmgr", "sccm", "audit-log", "security", "siem"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "ConfigMgr operations generate Security event log entries; SIEM can correlate ConfigMgr deployments with suspicious activities.",
                ApplyOps = [RegOp.SetDword(Key, "EnableClientAuditLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableClientAuditLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableClientAuditLogging", 1)],
            },
            new TweakDef
            {
                Id = "confmgr-require-ssl-for-management-point",
                Label = "ConfigMgr: Require HTTPS/PKI for All Client-to–Management Point Communication",
                Category = "System",
                Description = "Sets RequireSSLForManagementPoint=1 in ConfigurationManager policy. Enforces that the ConfigMgr client uses HTTPS with PKI client certificates for all communication with the Management Point, Distribution Point, and other site roles, blocking fallback to HTTP. " +
                    "Configuration Manager in HTTP mode transmits deployment data, credentials used for network access accounts, and package download URLs in plaintext. A network attacker on the same segment as a ConfigMgr client can intercept policy downloads and inject malicious package locations. Enforcing HTTPS-only communication requires PKI infrastructure but prevents man-in-the-middle interception of ConfigMgr policy and deployment content.",
                Tags = ["configmgr", "sccm", "https", "pki", "ssl", "management-point"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "ConfigMgr client requires HTTPS; HTTP communication with management point blocked. Requires PKI client certificates to be enrolled.",
                ApplyOps = [RegOp.SetDword(Key, "RequireSSLForManagementPoint", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSSLForManagementPoint")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSSLForManagementPoint", 1)],
            },
            new TweakDef
            {
                Id = "confmgr-disable-software-center-user-portal",
                Label = "ConfigMgr: Disable Software Center User-Initiated Install Portal",
                Category = "System",
                Description = "Sets DisableSoftwareCenterPortal=1 in ConfigurationManager policy. Disables the Software Center user-facing portal through which end users can browse 'Available' software and initiate their own optional application installs. Only 'Required' deployments that are pushed and mandatory remain active; the Software Center self-service catalog is removed from the user's Start menu. " +
                    "The Software Center self-service portal is appropriate for general enterprise endpoints where end users should be able to install productivity tools. In high-security or locked-down environments (healthcare workstations, kiosk terminals, PCI-scope machines), allowing users to install any software from the catalog — even admin-approved software — introduces unnecessary attack surface expansion. Application installs should be exclusively IT-admin-driven deployments.",
                Tags = ["configmgr", "sccm", "software-center", "lockdown", "user-install"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Software Center self-service portal disabled; only mandatory/required ConfigMgr deployments are presented to users.",
                ApplyOps = [RegOp.SetDword(Key, "DisableSoftwareCenterPortal", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSoftwareCenterPortal")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSoftwareCenterPortal", 1)],
            },
            new TweakDef
            {
                Id = "confmgr-disable-client-auto-upgrade",
                Label = "ConfigMgr: Disable Automatic ConfigMgr Client Agent Auto-Upgrade",
                Category = "System",
                Description = "Sets DisableAutoUpgrade=1 in ConfigurationManager policy. Prevents the ConfigMgr client agent from automatically upgrading itself when the site server is running a newer version of the ConfigMgr client, requiring IT to explicitly push client upgrades through a managed deployment. " +
                    "The ConfigMgr client auto-upgrade mechanism upgrades the client agent on all managed endpoints automatically when the Primary Site server is upgraded. While convenient, this means that upgrading the site server triggers an automatic, uncontrolled rollout to thousands of endpoints simultaneously, with no staging, no pilot group, and no rollback capability. A buggy client version pushed by auto-upgrade to all endpoints can simultaneously disrupt the management channel for the entire estate.",
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
                Category = "System",
                Description = "Sets RequireAdminApprovalForUserPolicy=1 in ConfigurationManager policy. Requires that user-targeted configuration baseline deployments (policies applied to users, not computers) receive explicit IT admin approval in the ConfigMgr console before the client agent executes them on the endpoint. " +
                    "In some ConfigMgr configurations, user-targeted configuration baselines can be deployed to security groups by less-privileged admins (Help Desk, Application Deployment staff) without requiring full ConfigMgr infrastructure admin privileges. If those baselines include scripts or registry modifications, a Help Desk operator with deployment rights could push policy changes to all users in their management scope. Requiring admin approval creates a second-factor approval gate for user-targeted policy execution.",
                Tags = ["configmgr", "sccm", "user-policy", "admin-approval", "separation-of-duties"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "User-targeted ConfigMgr configuration baselines require admin approval before execution; prevents unauthorised user-policy deployment.",
                ApplyOps = [RegOp.SetDword(Key, "RequireAdminApprovalForUserPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminApprovalForUserPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAdminApprovalForUserPolicy", 1)],
            },
            new TweakDef
            {
                Id = "confmgr-cap-content-cache-size-5gb",
                Label = "ConfigMgr: Cap Client Content Cache Size at 5 GB",
                Category = "System",
                Description = "Sets MaxContentCacheSizeGB=5 in ConfigurationManager policy. Limits the ConfigMgr client content cache (the local disk cache where the client pre-downloads content from Distribution Points before installation) to a maximum of 5 GB, preventing the cache from consuming disk space beyond this limit. " +
                    "By default, the ConfigMgr client content cache can grow to 10% of total disk size. On large-disk endpoints (1 TB drives), this allows a 100 GB cache. In environments with thin-provisioned storage (VDI, laptop SSDs) or low-disk-space scenarios, an unbounded cache can fill available disk space, causing operating system failures or application performance issues. A 5 GB cap is sufficient for most enterprise software deployments while protecting disk space.",
                Tags = ["configmgr", "sccm", "cache", "disk-space", "storage"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "ConfigMgr client content cache capped at 5 GB; disk consumption controlled for thin-provisioned storage environments.",
                ApplyOps = [RegOp.SetDword(Key, "MaxContentCacheSizeGB", 5)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxContentCacheSizeGB")],
                DetectOps = [RegOp.CheckDword(Key, "MaxContentCacheSizeGB", 5)],
            },
            new TweakDef
            {
                Id = "confmgr-disable-client-notification-feature",
                Label = "ConfigMgr: Disable ConfigMgr Client Notification Channel",
                Category = "System",
                Description = "Sets DisableClientNotification=1 in ConfigurationManager policy. Disables the ConfigMgr client notification channel — a push mechanism that allows the site server to send fast-path notifications to clients to immediately trigger a policy evaluation or initiate re-inventory without waiting for the standard polling interval. " +
                    "The client notification channel uses a persistent TCP connection from the ConfigMgr client to the Management Point. While this enables near-real-time policy deployment, it also means a compromised Management Point has an active connection to every managed client and can trigger immediate policy execution on all clients simultaneously. In environments where the threat model includes Management Point compromise, disabling the notification channel forces deployments to use the standard polling schedule which is easier to audit and rate-limit.",
                Tags = ["configmgr", "sccm", "client-notification", "tcp", "management-point"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "ConfigMgr push notifications disabled; policy deployment uses scheduled polling intervals instead of near-real-time push.",
                ApplyOps = [RegOp.SetDword(Key, "DisableClientNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClientNotification")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClientNotification", 1)],
            },
            new TweakDef
            {
                Id = "confmgr-enable-tamper-protection",
                Label = "ConfigMgr: Enable Tamper Protection for ConfigMgr Client Agent",
                Category = "System",
                Description = "Sets EnableClientTamperProtection=1 in ConfigurationManager policy. Enables the ConfigMgr client tamper protection mechanism, which prevents standard users and non-admin processes from stopping or disabling the CCMExec service, deleting the CCM client registry keys, or uninstalling the ConfigMgr client agent. " +
                    "Attackers that gain code execution on an endpoint as a standard user or as a low-privilege process will attempt to disable security tools and management agents before proceeding with lateral movement or data exfiltration. The ConfigMgr client agent is a high-value target for disablement because it delivers security baselines, patches, and malware detection policies. Tamper protection prevents the CCMExec service from being stopped by non-admin processes.",
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
                Category = "System",
                Description = "Sets DisableNAACredentialCaching=1 in ConfigurationManager policy. Prevents the ConfigMgr client from caching the Network Access Account (NAA) credentials — the service account used to authenticate with Distribution Points — in the local DPAPI credential store on the client disk. " +
                    "The ConfigMgr Network Access Account is a domain service account whose credentials are distributed to all ConfigMgr-managed clients to allow content download from Distribution Points. By default, these credentials are cached on disk using DPAPI. On a compromised endpoint, an attacker can extract the NAA credentials using tools that decrypt DPAPI-protected data (accessible to SYSTEM-level processes) and then use those credentials to authenticate to internal servers as the NAA service account, often a domain user with broad read access.",
                Tags = ["configmgr", "sccm", "naa", "credentials", "dpapi", "credential-theft"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "NAA credential caching disabled; ConfigMgr service account credentials are not stored on client disk after each policy download.",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Id           = "dchrdn-require-secure-channel-signing",
                Label        = "Require Signing on All Netlogon Secure Channel Connections",
                Category = "System",
                Description  = "Configures Netlogon to require cryptographic signing on all secure channel connections from this machine to its domain controller, protecting against Zerologon (CVE-2020-1472) and secure channel downgrade attacks.",
                Tags         = ["netlogon", "secure-channel", "zerologon", "cve-2020-1472", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 5,
                SafetyRating = 5,
                ImpactNote   = "Netlogon secure channel signing required; Zerologon class attacks against this machine's DC connection blocked.",
                ApplyOps     = [RegOp.SetDword(Key, "RequireSignOrSeal", 1)],
                RemoveOps    = [RegOp.DeleteValue(Key, "RequireSignOrSeal")],
                DetectOps    = [RegOp.CheckDword(Key, "RequireSignOrSeal", 1)],
            },
            new TweakDef
            {
                Id           = "dchrdn-require-secure-channel-sealing",
                Label        = "Require Sealing (Encryption) on Netlogon Secure Channel",
                Category = "System",
                Description  = "Configures the Netlogon secure channel to use full encryption (sealing) in addition to signing, ensuring the contents of secure channel messages cannot be intercepted and read by network observers.",
                Tags         = ["netlogon", "sealing", "encryption", "secure-channel", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 5,
                SafetyRating = 5,
                ImpactNote   = "Netlogon secure channel sealing enabled; DC communication encrypted end-to-end, not just signed.",
                ApplyOps     = [RegOp.SetDword(Key, "SealSecureChannel", 1)],
                RemoveOps    = [RegOp.DeleteValue(Key, "SealSecureChannel")],
                DetectOps    = [RegOp.CheckDword(Key, "SealSecureChannel", 1)],
            },
            new TweakDef
            {
                Id           = "dchrdn-sign-secure-channel",
                Label        = "Enable Netlogon Secure Channel Cryptographic Signatures",
                Category = "System",
                Description  = "Enables Netlogon secure channel signing at the Windows Security Support Provider level, ensuring all Netlogon RPC traffic includes a HMAC-based message authentication code protecting against modification.",
                Tags         = ["netlogon", "signing", "hmac", "rpc", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 5,
                SafetyRating = 5,
                ImpactNote   = "Netlogon RPC channel signing enabled; HMAC integrity protection active on all DC communication.",
                ApplyOps     = [RegOp.SetDword(Key, "SignSecureChannel", 1)],
                RemoveOps    = [RegOp.DeleteValue(Key, "SignSecureChannel")],
                DetectOps    = [RegOp.CheckDword(Key, "SignSecureChannel", 1)],
            },
            new TweakDef
            {
                Id           = "dchrdn-set-max-machine-account-age-30d",
                Label        = "Set Machine Account Password Maximum Age to 30 Days",
                Category = "System",
                Description  = "Sets the maximum age of the machine account Kerberos trust password to 30 days, ensuring machine account credentials are regularly rotated and limiting the window during which a stolen machine account password can be misused.",
                Tags         = ["netlogon", "machine-account", "password-age", "rotation", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 3,
                SafetyRating = 5,
                ImpactNote   = "Machine account password rotation cycle set to 30 days; stolen machine credentials expire in 30 days maximum.",
                ApplyOps     = [RegOp.SetDword(Key, "MaximumPasswordAge", 30)],
                RemoveOps    = [RegOp.DeleteValue(Key, "MaximumPasswordAge")],
                DetectOps    = [RegOp.CheckDword(Key, "MaximumPasswordAge", 30)],
            },
            new TweakDef
            {
                Id           = "dchrdn-disable-machine-account-pwd-change",
                Label        = "Enable Machine Account Password Rotation (Prevent Disabling)",
                Category = "System",
                Description  = "Ensures the machine account password rotation feature is not disabled, counteracting malware or misconfiguration that sets DisablePasswordChange=1 to prevent the machine account from rotating its domain password.",
                Tags         = ["netlogon", "machine-account", "password-change", "security", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 4,
                SafetyRating = 5,
                ImpactNote   = "Machine account password change enabled (DisablePasswordChange=0); regular DC trust rotations enforced.",
                ApplyOps     = [RegOp.SetDword(Key, "DisablePasswordChange", 0)],
                RemoveOps    = [RegOp.DeleteValue(Key, "DisablePasswordChange")],
                DetectOps    = [RegOp.CheckDword(Key, "DisablePasswordChange", 0)],
            },
            new TweakDef
            {
                Id           = "dchrdn-enable-strong-key",
                Label        = "Enable Strong Session Key for Netlogon Secure Channel",
                Category = "System",
                Description  = "Forces the use of AES-256 strong session keys for Netlogon secure channel encryption rather than the legacy DES-based 64-bit session keys, significantly increasing the strength of DC trust channel encryption.",
                Tags         = ["netlogon", "strong-key", "aes", "session-key", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 5,
                SafetyRating = 5,
                ImpactNote   = "Netlogon AES-256 strong session keys required; legacy 64-bit DES session keys rejected for DC channel.",
                ApplyOps     = [RegOp.SetDword(Key, "RequireStrongKey", 1)],
                RemoveOps    = [RegOp.DeleteValue(Key, "RequireStrongKey")],
                DetectOps    = [RegOp.CheckDword(Key, "RequireStrongKey", 1)],
            },
            new TweakDef
            {
                Id           = "dchrdn-restrict-null-session-pipes",
                Label        = "Restrict Null Session Named Pipe Access to Empty List",
                Category = "System",
                Description  = "Removes all entries from the NullSessionPipes registry value, ensuring no named pipes can be accessed via anonymous null session connections on this machine, closing a legacy attack vector for anonymous RPC enumeration.",
                Tags         = ["netlogon", "null-session", "named-pipes", "anonymous", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 4,
                SafetyRating = 5,
                ImpactNote   = "Null session named pipe list cleared; anonymous RPC pipe access completely blocked.",
                ApplyOps     = [RegOp.SetString(SecKey, "NullSessionPipes", "")],
                RemoveOps    = [RegOp.DeleteValue(SecKey, "NullSessionPipes")],
                DetectOps    = [RegOp.CheckString(SecKey, "NullSessionPipes", "")],
            },
            new TweakDef
            {
                Id           = "dchrdn-log-netlogon-failures",
                Label        = "Log Netlogon Secure Channel Failure Events",
                Category = "System",
                Description  = "Enables detailed event log entries for Netlogon secure channel establishment failures, authentication denials, and secure channel seal/sign rejections, providing visibility into DC trust channel attacks.",
                Tags         = ["netlogon", "event-log", "audit", "secure-channel-failure", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 4,
                SafetyRating = 5,
                ImpactNote   = "Netlogon secure channel failure events logged; DC authentication and Zerologon attack attempts visible.",
                ApplyOps     = [RegOp.SetDword(Key, "DbFlag", 0x2080FFFF)],
                RemoveOps    = [RegOp.DeleteValue(Key, "DbFlag")],
                DetectOps    = [RegOp.CheckDword(Key, "DbFlag", 0x2080FFFF)],
            },
            new TweakDef
            {
                Id           = "dchrdn-restrict-ntlm-in-domain",
                Label        = "Restrict Incoming NTLM Authentication in Domain Context",
                Category = "System",
                Description  = "Configures this domain member to block incoming NTLM authentication from domain accounts, requiring Kerberos for all intra-domain service authentication and preventing NTLM relay and pass-the-hash attacks between domain members.",
                Tags         = ["netlogon", "ntlm", "domain", "relay-attack", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 5,
                SafetyRating = 5,
                ImpactNote   = "Incoming NTLM from domain accounts blocked; intra-domain Kerberos required. NTLM relay via domain accounts mitigated.",
                ApplyOps     = [RegOp.SetDword(LsaKey, "RestrictReceivingNTLMTrafficInDomain", 2)],
                RemoveOps    = [RegOp.DeleteValue(LsaKey, "RestrictReceivingNTLMTrafficInDomain")],
                DetectOps    = [RegOp.CheckDword(LsaKey, "RestrictReceivingNTLMTrafficInDomain", 2)],
            },
            new TweakDef
            {
                Id           = "dchrdn-disable-netlogon-telemetry",
                Label        = "Disable Netlogon and Domain Services Telemetry to Microsoft",
                Category = "System",
                Description  = "Prevents the Netlogon service and domain authentication components from sending DC trust channel statistics, authentication success rates, and secure channel negotiation telemetry to Microsoft.",
                Tags         = ["netlogon", "telemetry", "privacy", "microsoft", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 2,
                SafetyRating = 5,
                ImpactNote   = "Netlogon telemetry to Microsoft disabled; DC channel stats and domain auth data not sent to cloud.",
                ApplyOps     = [RegOp.SetDword(Key, "DisableNetlogonTelemetry", 1)],
                RemoveOps    = [RegOp.DeleteValue(Key, "DisableNetlogonTelemetry")],
                DetectOps    = [RegOp.CheckDword(Key, "DisableNetlogonTelemetry", 1)],
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                    Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
        private const string ErmKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager";

        private const string MdmKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edm-enable-comanagement-with-sccm",
                    Label = "Enterprise Device Management: Enable Intune/SCCM Co-Management",
                    Category = "System",
                    Description =
                        "Sets EnableCoManagement=1 in MDM policy. Enables co-management of Windows 10/11 devices by both System Center Configuration Manager (SCCM/ConfigMgr) and Microsoft Intune simultaneously. Co-management allows gradual migration of workloads from SCCM to Intune — starting with compliance evaluation and conditional access in Intune while keeping software deployment in SCCM. Without this policy, organizations must choose one management plane. Co-management is the Microsoft-recommended path for organizations with existing SCCM infrastructure transitioning to cloud-modern management.",
                    Tags = ["co-management", "sccm", "configmgr", "intune", "cloud-management"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Devices are managed by both SCCM and Intune simultaneously. Workload authority (compliance, resource access, app deployment) is configurable per workload. Requires ConfigMgr 1710 or later and Intune subscription. Co-management authority conflicts are resolved by the workload slider settings in the SCCM console.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnableCoManagement", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableCoManagement")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnableCoManagement", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-remote-lock-on-compliance-breach",
                    Label = "Enterprise Device Management: Enable Remote Lock on Compliance Breach",
                    Category = "System",
                    Description =
                        "Sets EnableRemoteLockOnComplianceBreach=1 in EnterpriseResourceManager policy. Configures the MDM client to accept remote lock commands from the MDM authority when the device is marked non-compliant AND has not remediated within the grace period. Remote lock sets the device to the lock screen and requires the user to enter their PIN/password to regain access. This prevents a non-compliant device from being used while IT is investigating or while the device is remediating a compliance issue — ensuring that a known-non-compliant device is not being actively used to access corporate resources.",
                    Tags = ["remote-lock", "compliance", "non-compliant", "mdm", "incident-response"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "MDM authority can remotely lock a non-compliant device. The device requires the user's credentials to unlock. User may be temporarily unable to complete their work if locked during active use. Ensure a clear remediation process is communicated to users before deploying. Not the same as remote wipe — data is not affected.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "EnableRemoteLockOnComplianceBreach", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableRemoteLockOnComplianceBreach")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "EnableRemoteLockOnComplianceBreach", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-selective-wipe-on-unenroll",
                    Label = "Enterprise Device Management: Enable Selective Wipe of Corporate Data on Unenroll",
                    Category = "System",
                    Description =
                        "Sets EnableSelectiveWipeOnUnenroll=1 in EnterpriseResourceManager policy. Enables selective wipe of corporate data when a device unenrolls from MDM. Selective wipe removes only corporate-managed content: corporate email profiles, MDM-deployed certificates, VPN profiles, Wi-Fi profiles, and corporate app data — while preserving personal files, photos, and applications. This is the appropriate default for BYOD scenarios: when an employee leaves and disconnects their personal device from MDM, the corporate data is cleaned up without erasing the employee's personal content.",
                    Tags = ["selective-wipe", "unenrollment", "corporate-data", "byod", "data-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Unenrollment from MDM triggers removal of all MDM-deployed profiles, certificates, and managed app data. Personal files and apps are preserved. A corporate AAD-joined device unenrolling may lose domain join state. Not a full device wipe — ensure your users understand what is removed on unenrollment.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "EnableSelectiveWipeOnUnenroll", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableSelectiveWipeOnUnenroll")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "EnableSelectiveWipeOnUnenroll", 1)],
                },
                new TweakDef
                {
                    Id = "edm-require-approved-apps-only",
                    Label = "Enterprise Device Management: Restrict App Installation to MDM-Approved Apps Only",
                    Category = "System",
                    Description =
                        "Sets RequireApprovedAppsOnly=1 in EnterpriseResourceManager policy. Restricts app installation to apps that are deployed or approved by the MDM authority. Users are not permitted to install arbitrary apps from the Microsoft Store or third-party sources unless the MDM administrator has explicitly approved them in the app catalog. This policy is typically layered with AppLocker or Windows Defender Application Control. On its own, it provides an MDM-layer approval gate that blocks app installation from retail Store listings, reducing the attack surface from malicious store apps.",
                    Tags = ["approved-apps", "app-control", "mdm", "store", "whitelisting"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Only MDM-approved apps can be installed by users. Non-approved app installation attempts are blocked. Requires maintaining an approved app catalog in the MDM console. Users who need new apps must request IT approval. May disrupt productivity if the approval catalog is not kept up to date.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "RequireApprovedAppsOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "RequireApprovedAppsOnly")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "RequireApprovedAppsOnly", 1)],
                },
                new TweakDef
                {
                    Id = "edm-sync-device-inventory-every-4h",
                    Label = "Enterprise Device Management: Sync Device Inventory to MDM Every 4 Hours",
                    Category = "System",
                    Description =
                        "Sets InventorySyncIntervalHours=4 in EnterpriseResourceManager policy. Configures the MDM client to push a device inventory update (installed apps, hardware specs, disk space, OS version, installed patches) to the MDM authority every 4 hours. Accurate, fresh device inventory is essential for software license compliance, vulnerability management (detecting devices missing patches), and asset management. A staleinventory (updated less than once daily) may miss a device that has been reformatted, had apps removed, or had OS version changed — leading to false compliance reporting.",
                    Tags = ["inventory", "sync-interval", "asset-management", "vulnerability-mgmt", "mdm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Device inventory is uploaded to the MDM authority every 4 hours. Inventory includes installed apps, hardware, and OS state. Slightly increased MDM check-in frequency and bandwidth. Inventory sync data is typically 5–50 KB per cycle.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "InventorySyncIntervalHours", 4)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "InventorySyncIntervalHours")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "InventorySyncIntervalHours", 4)],
                },
                new TweakDef
                {
                    Id = "edm-block-factory-reset-by-user",
                    Label = "Enterprise Device Management: Prevent User-Initiated Factory Reset",
                    Category = "System",
                    Description =
                        "Sets BlockUserInitiatedFactoryReset=1 in EnterpriseResourceManager policy. Prevents standard users from performing a factory reset (Settings > System > Recovery > Reset this PC, or WinRE recovery). Factory reset bypasses MDM policies, removes all corporate data and certificates, and leaves the device unmanaged. An insider threat actor could use factory reset to wipe evidence before investigation. A regular user could accidentally factory reset, losing both personal and corporate data. IT-initiated remote wipe via the MDM console remains available for authorized operations.",
                    Tags = ["factory-reset", "protective", "insider-threat", "data-preservation", "mdm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Standard users cannot initiate factory reset. Local administrators can still reset via elevated permission flows. IT-initiated remote wipe from MDM console is not affected. Users who genuinely need to re-provision their device must contact IT.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "BlockUserInitiatedFactoryReset", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "BlockUserInitiatedFactoryReset")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "BlockUserInitiatedFactoryReset", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-mdm-certificate-renewal",
                    Label = "Enterprise Device Management: Enable Automatic MDM Certificate Renewal",
                    Category = "System",
                    Description =
                        "Sets EnableMdmCertificateRenewal=1 in MDM policy. Configures the MDM client to automatically renew the MDM enrollment certificate before it expires. The MDM enrollment certificate authenticates the device to the MDM service on every check-in. If this certificate expires without renewal, the device loses the ability to receive new policies, report compliance status, or accept remote management commands — even though it may still appear enrolled in the MDM console. Automatic renewal prevents this silent disconnection, which is especially important for devices in long-term storage or deployed in air-gapped environments.",
                    Tags = ["mdm", "certificate", "renewal", "enrollment", "expiry"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "MDM enrollment certificates are renewed automatically before expiry. Renewal occurs in the background without user interaction. Prevents devices from silently dropping off MDM management due to certificate expiry. Certificate validity periods are typically 1–2 years — renewal triggers at 80% of the validity period.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnableMdmCertificateRenewal", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableMdmCertificateRenewal")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnableMdmCertificateRenewal", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-managed-device-restrictions",
                    Label = "Enterprise Device Management: Enable MDM-Enforced Managed Device Restrictions",
                    Category = "System",
                    Description =
                        "Sets EnableManagedDeviceRestrictions=1 in EnterpriseResourceManager policy. Enables the enforcement layer for MDM-delivered device restrictions — settings like camera disable, screen capture restriction, clipboard policy, USB disable, and Bluetooth restriction — that are delivered as MDM CSP payloads. Without this flag, MDM restriction payloads are accepted but not enforced at the OS level. This is a master switch that must be enabled for MDM-pushed restrictions to take effect. Relevant for organizations deploying information protection policies that require disabling hardware capabilities on managed devices.",
                    Tags = ["mdm", "device-restrictions", "camera-disable", "clipboard", "usb"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "MDM-delivered device restrictions are enforced by the OS. Without this, restrictions are delivered but silently not applied. Restrictions that take effect depend on which CSP payloads the MDM administrator has configured — this policy enables the enforcement mechanism, not specific restrictions.",
                    ApplyOps = [RegOp.SetDword(ErmKey, "EnableManagedDeviceRestrictions", 1)],
                    RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableManagedDeviceRestrictions")],
                    DetectOps = [RegOp.CheckDword(ErmKey, "EnableManagedDeviceRestrictions", 1)],
                },
                new TweakDef
                {
                    Id = "edm-audit-mdm-policy-changes",
                    Label = "Enterprise Device Management: Audit All MDM Policy Application Events",
                    Category = "System",
                    Description =
                        "Sets AuditMdmPolicyChanges=1 in MDM policy. Enables audit events whenever an MDM policy is applied, updated, or removed on the device. Each audit event records the CSP path that was changed, the old and new values, the MDM authority that issued the change, and the result (success or error code). MDM policy audit events are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel. These events are essential for SIEM correlation: if a device's MDM policy is unexpectedly changed (indicating a rogue MDM push or configuration scope error), the audit trail makes detection possible.",
                    Tags = ["mdm", "audit", "policy-changes", "siem", "event-log"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All MDM policy application events are logged with CSP path, values, and origin. Events written to DeviceManagement-Enterprise-Diagnostics-Provider channel. Slightly higher log volume on devices with frequent policy changes (Intune check-in + policy delta). Enables SIEM alerting on unexpected MDM policy modifications.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "AuditMdmPolicyChanges", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "AuditMdmPolicyChanges")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "AuditMdmPolicyChanges", 1)],
                },
                new TweakDef
                {
                    Id = "edm-enable-encrypted-mdm-channel",
                    Label = "Enterprise Device Management: Enforce TLS 1.2+ for MDM Communication",
                    Category = "System",
                    Description =
                        "Sets RequireEncryptedMdmChannel=1 in MDM policy. Enforces that all MDM client communication (enrollment, check-in, policy delivery, command receipt) is conducted over TLS 1.2 or higher. MDM payloads include configuration settings, app assignments, certificate payloads, and VPN profiles — all of which are sensitive. An MDM session over TLS 1.0 can be downgrade-attacked using known vulnerabilities (BEAST, POODLE) to intercept policy payloads. Enforcing TLS 1.2+ on the MDM channel ensures that policy delivery is encrypted to modern standards.",
                    Tags = ["mdm", "tls", "encrypted-channel", "transport-security", "policy-delivery"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "MDM communication is restricted to TLS 1.2 or higher. MDM servers that only support TLS 1.0 or 1.1 will be unable to communicate with the client. All modern MDM services (Intune, SCCM cloud attachment) use TLS 1.2+. On-premises MDM servers must be updated if they are still on legacy TLS.",
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
                Category = "System",
                Description = "Sets DefaultDeployRing=2 in EnterpriseResourceManager policy. Configures the default application deployment ring for this endpoint to the 'Broad' (stable) deployment ring, ensuring the device receives application updates only after full release validation has been completed across the Pilot and Early Majority rings. " +
                    "Enterprise application deployments using modern ring-based rollout (Intune or ConfigMgr ring filtering) gate updates through sequenced rings before broad deployment. Endpoints that are miscategorised as 'Pilot' receive updates intended for testing and may encounter pre-release application bugs. Explicitly setting the deployment ring to 'Broad' (ring 2) prevents endpoints from accidentally receiving early-ring deployments due to misconfigured ring assignment logic.",
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
                Category = "System",
                Description = "Sets RequireAdminForAppRemoval=1 in EnterpriseResourceManager policy. Blocks standard users from uninstalling applications that were deployed by the enterprise (via Intune, ConfigMgr, or Group Policy Software Installation), requiring administrative credentials for removal even though the application was installed in user context. " +
                    "Required enterprise applications (endpoint detection and response agents, certificate management tools, identity protection software) must remain installed once deployed. A standard user who can uninstall enterprise-managed apps can remove security tooling from their device, creating a gap in protection that may persist until the next compliance check triggers a remediation deployment. Blocking user-initiated uninstall of managed apps prevents intentional or accidental removal of critical security tools.",
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
                Category = "System",
                Description = "Sets BlockUserInitiatedInstall=1 in EnterpriseResourceManager policy. Prevents users from initiating the installation of new applications through any mechanism other than IT-managed deployment channels (Intune, ConfigMgr, Software Center) — blocking double-click installer execution, Windows Installer (MSI) invocation, and MSIX/APPX package sideloading by standard users. " +
                    "The majority of enterprise malware infections arrive as LOB-disguised executables or malicious MSI packages that a user is socially engineered into running. If users can execute arbitrary installers, the application allowlist maintained by IT is bypassed — even if the endpoint has Microsoft Defender WDAC policy configured, a sufficiently permissive WDAC policy allows signed MSI files from any vendor. Blocking user-initiated installation removes the primary vector for user-driven software installation.",
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
                Category = "System",
                Description = "Sets EnforceMaintenanceWindow=1 in EnterpriseResourceManager policy. Restricts deployment execution by the enterprise resource manager to within the configured maintenance window schedule, preventing deployments from triggering application installs, updates, or reboots during business hours and confining disruptive deployments to the approved maintenance period. " +
                    "Without maintenance window enforcement, a deployment configured as 'Available as soon as possible' may start an application install or triggered reboot at any time, including during an end-user presentation or in the middle of a running workflow. Maintenance windows define agreed low-impact periods (after hours, weekends) for deployments. Enforcing the maintenance window prevents IT from accidentally or intentionally bypassing the agreed change window, which is often an ITIL or change management process requirement.",
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
                Category = "System",
                Description = "Sets MaxInstallRetries=3 in EnterpriseResourceManager policy. Limits the number of times the enterprise resource manager retries a failed application installation to 3 attempts before marking the deployment as failed and triggering an alert, rather than retrying indefinitely. " +
                    "A deployment that retries an application installation indefinitely will continually consume CPU, disk I/O, and network bandwidth on the endpoint for days or weeks. On endpoints with transient installation failures (antivirus blocking the installer, required service temporarily unavailable), unlimited retries create ongoing performance degradation. Capping retries at 3 ensures failed deployments are surfaced as failures in the management console rather than silently retrying without ever succeeding.",
                Tags = ["enterprise-deploy", "install-retry", "deployment-failure", "performance", "alert"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Deployment install retries capped at 3; repeat failures surface as deployment failures rather than silent perpetual retry.",
                ApplyOps = [RegOp.SetDword(Key, "MaxInstallRetries", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxInstallRetries")],
                DetectOps = [RegOp.CheckDword(Key, "MaxInstallRetries", 3)],
            },
            new TweakDef
            {
                Id = "erdeploy-enable-deployment-audit-log",
                Label = "Enterprise Deploy: Enable Security Audit Log for All Deployment Operations",
                Category = "System",
                Description = "Sets EnableDeploymentAuditLog=1 in EnterpriseResourceManager policy. Causes each application installation, update, and removal operation completed by the enterprise resource manager to generate a Security event log entry, recording the application name, version, deployment source, requesting authority, and outcome code. " +
                    "Application deployment audit logs are required in PCI-DSS, HIPAA, and SOC2 regulated environments where all software changes on in-scope endpoints must be tracked in a tamper-evident audit log. Without deployment audit logging, an attacker who compromises the management channel and installs a malicious application through the enterprise deployment infrastructure would have no on-device trace of the install (as the standard registry Uninstall key is easily manipulated). Security event log entries are tamper-resistant to local manipulation.",
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
                Category = "System",
                Description = "Sets DisableSideloadedApps=1 in EnterpriseResourceManager policy. Prevents installation of APPX/MSIX application packages from unsigned or unmanaged sources (USB drives, SharePoint file shares, developer sideloading) and restricts APPX installation to managed channels only (Microsoft Store for Business, Intune managed app, or enterprise signed MSIX bundles). " +
                    "MSIX sideloading is the primary vector for distributing trojanised or repackaged application packages disguised as legitimate enterprise tools. An attacker who sends a malicious MSIX package via email or file share (and the user's developer mode is enabled) can have arbitrary code run in a package context with the package's declared capabilities. Disabling sideloading from unmanaged sources blocks this vector without affecting Store and Intune-delivered MSIX packages.",
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
                Category = "System",
                Description = "Sets RequireSignedPackages=1 in EnterpriseResourceManager policy. Requires that every application package deployed through the enterprise resource manager is digitally signed by a certificate in the enterprise trusted publisher store before the installation is allowed to proceed, blocking unsigned or improperly signed packages from executing. " +
                    "Unsigned deployment packages can be tampered with between the time they are created and the time they are deployed. An attacker who compromises a Distribution Point or content staging server can replace a legitimate installer package with a trojanised version. Without package signing verification, the deployment infrastructure distributes the malicious version to all targeted endpoints without any integrity check. Requiring signed packages ensures only packages that passed code signing (and therefore were authenticated at signing time) are installed.",
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
                Category = "System",
                Description = "Sets BlockStoreInstallDuringDeployment=1 in EnterpriseResourceManager policy. Suspends automatic Microsoft Store application updates from downloading and installing during active enterprise deployment windows, preventing Store-initiated background installs from competing with enterprise deployment bandwidth and CPU allocations. " +
                    "Large enterprise deployments (OS feature updates, security patches for hundreds of applications) consume significant bandwidth from Distribution Points. If the Microsoft Store simultaneously triggers background app updates across the same endpoints during the deployment window, both processes compete for disk I/O, network bandwidth, and Windows Installer service locking. This can cause enterprise deployments to fail with 'service busy' errors or time out due to resource contention. Blocking Store updates during scheduled deployment windows eliminates this interference.",
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
                Category = "System",
                Description = "Sets EnforcePrerequisiteChecks=1 in EnterpriseResourceManager policy. Enforces that the installation of a dependent application is verified as successfully installed and functional before the enterprise resource manager proceeds with a higher-level application deployment that requires it as a prerequisite, rather than attempting the deployment and failing at runtime. " +
                    "Enterprise application deployments often have prerequisite chains: a LOB application may require a specific .NET runtime version, a specific redistributable, and a specific licence management service to be installed before it will work. Without prerequisite enforcement, all packages attempt installation in parallel, and the LOB application may fail (or partially install) because its prerequisites aren't available yet. Enforcing prerequisite checks runs the dependency chain in the correct order and stops the deployment if any prerequisite fails.",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Category = "System",
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
                Id = "esroam-disable-app-setting-sync",
                Label = "Enterprise State Roaming: Disable Application Settings Sync",
                Category = "System",
                Description = "Prevents Windows from syncing application settings (e.g., browser preferences, UWP app configuration) to Azure AD / Microsoft account cloud storage via Enterprise State Roaming. Application settings often contain business-critical customizations; roaming them to cloud storage creates data residency concerns and can cause settings to propagate to personal devices using the same account.",
                Tags = ["state roaming", "sync", "app settings", "azure ad", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableApplicationSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableApplicationSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableApplicationSettingSync", 2)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables app settings sync to cloud; settings remain device-local only.",
            },
            new TweakDef
            {
                Id = "esroam-disable-start-layout-sync",
                Label = "Enterprise State Roaming: Disable Start Menu Layout Sync",
                Category = "System",
                Description = "Disables synchronization of the Windows Start layout (Start screen tile arrangement, pinned apps, size configuration) across devices enrolled in Enterprise State Roaming. In managed environments where Start menus are deployed via Group Policy or provisioning packages, cloud roaming of Start layout can overwrite the IT-managed layout, creating inconsistency across machines.",
                Tags = ["state roaming", "sync", "start menu", "layout", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableStartLayoutSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableStartLayoutSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableStartLayoutSettingSync", 2)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Stops Start menu layout from roaming; each device keeps its own IT-deployed or user-configured layout.",
            },
            new TweakDef
            {
                Id = "esroam-disable-desktop-theme-sync",
                Label = "Enterprise State Roaming: Disable Desktop Theme Sync",
                Category = "System",
                Description = "Prevents Windows desktop themes (wallpaper, accent color, visual effects, window transparency) from synchronizing across devices. While cosmetic, theme sync can override corporate branding standards (desktop wallpapers, accent colors mandated by IT) when the same account logs in on different managed computers or when personal account themes overwrite work account settings.",
                Tags = ["state roaming", "sync", "theme", "desktop", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableDesktopThemeSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableDesktopThemeSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableDesktopThemeSettingSync", 2)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents theme roaming; no functional impact — wallpaper and accent color stay per-device.",
            },
            new TweakDef
            {
                Id = "esroam-disable-browser-setting-sync",
                Label = "Enterprise State Roaming: Disable Web Browser Settings Sync",
                Category = "System",
                Description = "Stops the Windows Settings Sync provider from roaming browser-related settings (favorites, history sync pointers) through the ESR (Enterprise State Roaming) channel. Note: this controls the Windows-level sync channel, not the browser's own sync channel. Browser favorites should be managed via browser-specific policies (see Edge/Chrome policy modules).",
                Tags = ["state roaming", "sync", "browser", "favorites", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableWebBrowserSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableWebBrowserSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableWebBrowserSettingSync", 2)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks ESR-level browser sync; browser-native sync (Edge profile sync) requires a separate policy.",
            },
            new TweakDef
            {
                Id = "esroam-disable-password-sync",
                Label = "Enterprise State Roaming: Disable Password Settings Sync",
                Category = "System",
                Description = "Prevents Windows credential and password hashes from being synchronized through the Enterprise State Roaming channel to Azure AD cloud storage. Password roaming via ESR is distinct from Azure AD seamless SSO and may involve credential material being persisted in a cloud-accessible store. In high-security environments, all credential handling must be on-premises only.",
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
                Category = "System",
                Description = "Disables the specific ESR sync provider for application data packages (UWP AppX app state, configuration blobs stored in the cloud). App sync allows UWP apps to restore their last-used state—including user-typed data—when the same account signs in on another device. For apps that handle sensitive data (forms, documents), roaming this state creates residual data in Azure.",
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
                Id = "esroam-block-user-override-setting-sync",
                Label = "Enterprise State Roaming: Block User Override of Settings Sync Policy",
                Category = "System",
                Description = "Prevents users from re-enabling settings synchronization that has been disabled by Group Policy. Without this policy, a standard user can navigate to Windows Settings → Accounts → Sync your settings and re-enable options that the admin has turned off. This policy ensures sync restrictions are permanent and cannot be overridden by end users.",
                Tags = ["state roaming", "sync", "user override", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SyncKey],
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableSettingSyncUserOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableSettingSyncUserOverride")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableSettingSyncUserOverride", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Locks the sync policy; users cannot re-enable disabled sync categories from Settings.",
            },
            new TweakDef
            {
                Id = "esroam-block-user-override-app-sync",
                Label = "Enterprise State Roaming: Block User Override of App Settings Sync Policy",
                Category = "System",
                Description = "Prevents users from overriding the Group Policy that disables application settings synchronization. The Windows sync settings UI allows users to individually toggle sync categories; this policy forces DisableApplicationSettingSync to be admin-enforced and uneditable, ensuring corporate devices cannot roam application configuration data regardless of user preference.",
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
                Category = "System",
                Description = "Prevents users from re-enabling Start menu layout synchronization after it has been disabled by Group Policy. In environments with GPO-deployed Start menus, users should not be able to revert to a cloud-synced layout that was potentially configured on a personal device or a different organizational unit, as this undermines the standardized desktop configuration management.",
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
                Category = "System",
                Description = "Disables synchronization of device account settings (Microsoft account email app state, mail account configuration, calendar sync settings) through Enterprise State Roaming. On managed corporate devices where mail clients are configured centrally via MDM profiles or Exchange Autodiscover, preventing cloud-roaming of account settings avoids conflicts between centrally-pushed and user-synced configurations.",
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
                Category = "System",
                Description = "Enables cache renaming when a redirected folder path changes. When a folder redirection target is updated via Group Policy (e.g., moving a redirected My Documents share from an old file server to a new one), Windows can seamlessly rename the local offline-files cache entry to match the new UNC path. Without this setting, the client cache may retain stale entries pointing to the old server, causing offline file sync conflicts.",
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
                Category = "System",
                Description = "Prevents Windows from downloading a roaming user profile from the network during logon when the user profile server is unavailable. Without this policy, Windows waits for the roaming profile to download (up to the profile server timeout) before allowing login. Blocking this fallback download prevents slow logons when profile servers are down while ensuring that local cached profiles are used immediately.",
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
                Category = "System",
                Description = "Suppresses the roaming profile quota warning balloon notification that appears in the notification area when a user's roaming profile approaches its storage quota. In enterprise environments where profile size is managed through other mechanisms (e.g., folder redirection, profile monitoring tools), these notifications create user confusion and help desk calls without providing actionable guidance for end users.",
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
                Category = "System",
                Description = "Forces Windows to perform a synchronous (blocking) Group Policy application at logon rather than applying folder redirection policies in the background after the user is already logged in. Without this setting, users may briefly see their unredirected local Desktop and Documents folders before the redirection takes effect, resulting in files being saved to the wrong location. Synchronous policy application eliminates this window.",
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
                Category = "System",
                Description = "Grants the user exclusive NTFS permissions on their redirected folder target when the folder redirection policy first creates it on the file server. Without this setting, the Administrators group retains access to all redirected folders, enabling administrators to read user-redirected documents. Granting exclusive user rights is a privacy and security best practice that ensures sensitive user data in redirected folders is only accessible to the owning account.",
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
                Category = "System",
                Description = "Configures folder redirection to use the localized (OS language-specific) names for redirected subfolders on the file server rather than English names. In multi-language organizations where different users log on with different Windows UI languages, the names of redirected subfolder paths (e.g., Documents vs. Documenti vs. Dokumente) can vary unless this policy standardizes them to the localized folder names per user.",
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
                Category = "System",
                Description = "Instructs Windows to automatically move the contents of a folder from its original local location to the new UNC redirect target when folder redirection is first applied. Without this policy, existing local files stay in place and only new files go to the redirect target, leaving users with data split across two locations. Enabling content migration ensures a complete transition to the managed file server path.",
                Tags = ["folder redirection", "content migration", "data move", "gpo", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "MoveContentsExistingFolders", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "MoveContentsExistingFolders")],
                DetectOps = [RegOp.CheckDword(SystemKey, "MoveContentsExistingFolders", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Moves existing local files to the redirect target on first policy application; ensure the target share has sufficient space.",
            },
            new TweakDef
            {
                Id = "folderredir-disable-unc-path-hardening-bypass",
                Label = "GPO Folder Redirection: Block UNC Hardening Bypass for Redirected Paths",
                Category = "System",
                Description = "Prevents applications from bypassing UNC path hardening (SMB signing requirements) for redirected folder UNC targets. Windows allows some UNC access to bypass signing requirements for specific paths. This policy ensures that even though folder redirection targets are trusted by Windows, they are still subject to SMB signing requirements to prevent man-in-the-middle attacks on the file server connection carrying redirected folder traffic.",
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
                Category = "System",
                Description = "Configures the network bandwidth threshold below which Windows considers the connection to the roaming profile server as a 'slow link', triggering use of the local cached profile instead of downloading the full remote profile. Setting this to the Microsoft-recommended value of 500 kbps ensures that even on moderate WAN links, users get fast logons while good connections still get the full roaming/redirected experience.",
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
                Category = "System",
                Description = "Permits migration of the local user profile to the roaming profile path when a user first logs in after a machine is joined to a domain. Without this policy, domain logons create a new empty profile and the user's existing local profile data (desktop files, AppData settings) is left behind in the local profile. Enabling migration ensures the first domain logon seamlessly carries over all existing local user data.",
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
                Category = "System",
                Description = "Configures Windows to run all Group Policy logon scripts synchronously and display the desktop only after all logon scripts have completed. By default, Windows may display the desktop before all logon scripts finish, which can result in users opening applications before drive mappings, printer connections, or environment variables are established by logon scripts. Synchronous execution ensures scripts complete before the user session is accessible.",
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
                Category = "System",
                Description = "Configures Windows to run all Computer Configuration startup scripts one at a time and sequentially before the logon prompt appears. Without this setting, startup scripts may run asynchronously in the background, meaning critical system initialization scripts (e.g., disk encryption unlock, certificate enrollment, MDM check-in) may not complete before a user logs in, potentially resulting in incomplete system state at logon.",
                Tags = ["gpo scripts", "startup scripts", "synchronous", "boot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "RunStartupScriptSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "RunStartupScriptSync")],
                DetectOps = [RegOp.CheckDword(SystemKey, "RunStartupScriptSync", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Ensures each startup script finishes before the next begins and before the logon screen appears; adds boot time equal to total startup script duration.",
            },
            new TweakDef
            {
                Id = "gposcripts-run-legacy-logon-hidden",
                Label = "GPO Scripts: Run Legacy Logon Scripts Visible but Silent",
                Category = "System",
                Description = "Forces legacy logon scripts (those defined in the user profile properties of Active Directory) to run visible to the user but without a separate CMD window. By default, legacy logon scripts (as distinct from Group Policy logon scripts) may flash console windows briefly. This policy suppresses the command prompt window while still allowing the script to run, providing a cleaner logon experience without confusing users with flashing black windows.",
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
                Category = "System",
                Description = "Sets the maximum time Windows will wait for a Group Policy script (startup, logon, logoff, or shutdown) to complete before forcibly terminating it. The default is 600 seconds (10 minutes). Scripts that exceed this timeout are terminated without completing. Setting this explicitly prevents runaway scripts from hanging the logon/logoff sequence indefinitely, which can leave the machine in an unresponsive state.",
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
                Category = "System",
                Description = "Suppresses the 'Running startup scripts...' status message and progress screen that Windows displays during boot when Group Policy startup scripts are executing. In environments where startup scripts handle sensitive operations (certificate enrollment, TPM initialization commands, encrypted volume mounting), displaying the script status messages onscreen may expose the types of security operations to anyone observing the boot screen.",
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
                Category = "System",
                Description = "Hides the 'Applying your personal settings...' and similar logon script progress messages that appear during user logon in verbose mode. While informative for administrators, these messages reveal that Group Policy logon scripts are running, potentially exposing script categories to end users. In secure environments, the logon process should be opaque — completing silently and presenting the desktop only when ready.",
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
                Category = "System",
                Description = "Suppresses the window that appears when Group Policy logoff scripts are executing at user sign-out. When logoff scripts clean up user sessions (removing temp credentials, wiping browser profiles, revoking certificates), showing the progress window to the user is unnecessary and can lead users to terminate the logoff early by pressing the power button, potentially leaving cleanup scripts incomplete.",
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
                Category = "System",
                Description = "Suppresses the shutdown script progress window that shows when Group Policy Computer Configuration shutdown scripts run during system power-down. Shutdown scripts commonly perform operations such as disk encryption key cleanup, network session teardown, and compliance logging. Hiding the progress window provides a cleaner shutdown experience and prevents disclosure of the shutdown script sequence to onlookers.",
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
                Category = "System",
                Description = "Forces user-level logon scripts (defined in profile properties) to run before the Group Policy client completes processing Computer and User Configuration logon scripts. In some deployment scenarios, user-specific scripts (which map personal drives or configure user-specific settings) must run before broader GPO changes are applied. This ordering ensures user context is established before group-level policies modify the environment.",
                Tags = ["gpo scripts", "logon scripts", "run order", "user scripts", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "RunScriptsFirstAtUserLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "RunScriptsFirstAtUserLogon")],
                DetectOps = [RegOp.CheckDword(SystemKey, "RunScriptsFirstAtUserLogon", 1)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Reorders script execution — user profile scripts run before GPO logon scripts; may expose environment to user scripts before GPO lockdown is applied.",
            },
            new TweakDef
            {
                Id = "gposcripts-set-max-noninteractive-runtime",
                Label = "GPO Scripts: Set Maximum Non-Interactive Script Runtime (5 minutes)",
                Category = "System",
                Description = "Sets the maximum time a non-interactive Group Policy script (startup, shutdown, logon, logoff scripts running in non-interactive mode) is allowed to run. Setting this to 300 seconds (5 minutes) provides a tighter timeout than the default 600 seconds. For background scripts that should complete quickly, this reduces the window during which a script error or infinite loop delays the logon or shutdown sequence.",
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

    // ── GroupPolicySettingsPolicy ──
    private static class _GroupPolicySettingsPolicy
    {
        private const string Key =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy";

        // Well-known Group Policy Object GUID for User Configuration CSE
        private const string UserCseKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "gppol-disable-slow-link-detection",
                    Label = "Disable Slow-Link GP Processing Skip",
                    Category = "System",
                    Description =
                        "By default, some Group Policy CSEs (such as Software Installation) are skipped when a slow network link is detected. Disabling this exception ensures all policies are fully applied even over slow connections. Default: slow-link skip enabled. Recommended: 1 on all managed endpoints.",
                    Tags = ["group-policy", "slow-link", "processing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Group Policy fully applies on all network connections including slow links; no CSEs are skipped.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoSlowLink",
                            0
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoSlowLink"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoSlowLink",
                            0
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "gppol-force-reprocess-changed",
                    Label = "Force Reprocessing of Changed GP Objects",
                    Category = "System",
                    Description =
                        "Instructs the Group Policy client to reapply all policy settings at each background refresh cycle, even if no GPO has changed since the last refresh. Ensures policy drift (caused by local changes) is corrected at the next refresh. Default: only changed GPOs are reprocessed. Recommended: 1.",
                    Tags = ["group-policy", "refresh", "reprocess", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All policy settings are re-applied at every background refresh cycle; local configuration drift is corrected automatically.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoBackgroundPolicy",
                            0
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoBackgroundPolicy"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                            "NoBackgroundPolicy",
                            0
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "gppol-set-refresh-interval-30min",
                    Label = "Set GP Background Refresh Interval to 30 Minutes",
                    Category = "System",
                    Description =
                        "Reduces the background Group Policy refresh interval from the default 90 minutes (+30-minute random offset) to 30 minutes. Faster refresh means policy changes reach devices sooner and local configuration drift is corrected more quickly. Default: 90 minutes. Recommended: 30 for dynamic policy environments.",
                    Tags = ["group-policy", "refresh-interval", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Group Policy background refresh fires every 30 minutes; policy changes propagate to devices within half an hour.",
                    ApplyOps = [RegOp.SetDword(Key, "GroupPolicyRefreshTime", 30)],
                    RemoveOps = [RegOp.DeleteValue(Key, "GroupPolicyRefreshTime")],
                    DetectOps = [RegOp.CheckDword(Key, "GroupPolicyRefreshTime", 30)],
                },
                new TweakDef
                {
                    Id = "gppol-set-refresh-offset-0",
                    Label = "Set GP Refresh Random Offset to 0",
                    Category = "System",
                    Description =
                        "Removes the random time offset added to each policy refresh interval. The default offset spreads refresh load across the interval; setting it to 0 makes refreshes predictable and easier to correlate with compliance scan windows. Default: 30-minute random offset. Recommended: 0 in controlled networks.",
                    Tags = ["group-policy", "refresh-interval", "offset", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "GP refresh fires at exactly the configured interval with no random delay; refresh times are deterministic.",
                    ApplyOps = [RegOp.SetDword(Key, "GroupPolicyRefreshTimeDC", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "GroupPolicyRefreshTimeDC")],
                    DetectOps = [RegOp.CheckDword(Key, "GroupPolicyRefreshTimeDC", 0)],
                },
                new TweakDef
                {
                    Id = "gppol-enable-verbose-logging",
                    Label = "Enable Verbose GP Processing Logging",
                    Category = "System",
                    Description =
                        "Writes detailed diagnostic information about each Group Policy processing cycle to the Group Policy Operational event log. Enables troubleshooting of GPO application failures and security audit of policy application. Default: limited operational logging. Recommended: 1 on managed endpoints.",
                    Tags = ["group-policy", "logging", "audit", "diagnostics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Detailed GP processing events written to Microsoft-Windows-GroupPolicy/Operational log.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableVerboseLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableVerboseLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableVerboseLogging", 1)],
                },
                new TweakDef
                {
                    Id = "gppol-disable-user-gpo-override",
                    Label = "Prevent Users from Overriding Group Policy Settings",
                    Category = "System",
                    Description =
                        "Blocks users from modifying registry keys that are managed by Group Policy, even when those keys are under HKCU. Without this, a technically savvy user could temporarily override a GPO setting by writing directly to HKCU. Default: HKCU writes allowed. Recommended: 1 on high-security desktops.",
                    Tags = ["group-policy", "user-override", "security", "hkcu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users cannot bypass HKCU-targeted GP settings by writing conflicting registry values.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUserGPOOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUserGPOOverride")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUserGPOOverride", 1)],
                },
                new TweakDef
                {
                    Id = "gppol-apply-during-logon",
                    Label = "Apply Group Policy Synchronously at Logon",
                    Category = "System",
                    Description =
                        "Forces Group Policy to be applied synchronously at logon — the desktop does not appear until all policies have been processed. Prevents users from interacting with the desktop before security policies (such as drive mappings, logon scripts, and folder redirection) are applied. Default: async logon on workstations. Recommended: 1.",
                    Tags = ["group-policy", "logon", "synchronous", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Logon is slightly slower but all policies are guaranteed applied before the desktop appears.",
                    ApplyOps = [RegOp.SetDword(Key, "SynchronousMachineGroupPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SynchronousMachineGroupPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "SynchronousMachineGroupPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "gppol-log-rsop-data",
                    Label = "Enable RSoP (Resultant Set of Policy) Logging",
                    Category = "System",
                    Description =
                        "Enables collection and logging of Resultant Set of Policy data, which records exactly which policies are applied to each user and computer. Required for the 'Logging' mode of Group Policy Modeling and for compliance audits that verify policy coverage. Default: RSoP logging enabled but may be disabled by some hardening guides. Recommended: 1.",
                    Tags = ["group-policy", "rsop", "logging", "audit", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "RSoP data is collected; IT can run Group Policy Results wizard to verify policy application.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLocalGPOs", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLocalGPOs")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLocalGPOs", 0)],
                },
                new TweakDef
                {
                    Id = "gppol-block-local-gpo",
                    Label = "Disable Local Group Policy Objects for Domain Members",
                    Category = "System",
                    Description =
                        "Prevents Local GPOs (lgpo.exe modifications, Local Security Policy) from being applied on domain-joined machines. When domain GPOs manage all settings, local GPOs can introduce conflicts or be used to circumvent domain policy. Default: local GPOs applied. Recommended: 1 on all domain-joined machines.",
                    Tags = ["group-policy", "local-gpo", "domain", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Local Group Policy Objects are ignored; only domain-delivered GPOs apply.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLocalGPO", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLocalGPO")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLocalGPO", 1)],
                },
                new TweakDef
                {
                    Id = "gppol-require-secure-channel",
                    Label = "Require Secure Channel for GP Download",
                    Category = "System",
                    Description =
                        "Forces Windows to use a signed and encrypted secure channel (Kerberos) when downloading GPOs from the domain controller. Prevents man-in-the-middle attacks that inject malicious policy settings during transport. Default: secure channel used but not strictly enforced for all GPOs. Recommended: 1.",
                    Tags = ["group-policy", "secure-channel", "kerberos", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "GP downloads are authenticated and encrypted; rogue policy injection during GPO download is blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSecureChannel", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSecureChannel")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSecureChannel", 1)],
                },
            ];

    }

    // ── HotpatchUpdatePolicy ──
    private static class _HotpatchUpdatePolicy
    {
        private const string HotpatchKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\Hotpatch";
        private const string WuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "hotpatch-enable-hotpatch-updates",
                Label = "Enable Windows Hotpatch (Live Kernel Patching)",
                Category = "System",
                Description = "Enables Windows Hotpatch, which installs security patches directly into running kernel and system process memory without requiring a reboot. Dramatically reduces downtime for critical servers and VMs while keeping them current.",
                Tags = ["hotpatch", "live-patching", "kernel", "windows-update", "reboot-less"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Enables in-memory patching for monthly quality updates without reboots. Requires Windows 11 24H2+ or Azure Edition VMs. Baseline updates still require occasional reboots quarterly.",
                RegistryKeys = [HotpatchKey],
                ApplyOps  = [RegOp.SetDword(HotpatchKey, "EnableHotPatch", 1)],
                RemoveOps = [RegOp.DeleteValue(HotpatchKey, "EnableHotPatch")],
                DetectOps = [RegOp.CheckDword(HotpatchKey, "EnableHotPatch", 1)],
            },
            new TweakDef
            {
                Id = "hotpatch-disable-hotpatch-updates",
                Label = "Disable Windows Hotpatch Updates",
                Category = "System",
                Description = "Administratively disables the Hotpatch update channel, reverting the device to the traditional monthly Update Tuesday update cycle that installs patches via a reboot. Suitable for environments that require deterministic full-restart update cycles.",
                Tags = ["hotpatch", "disable", "windows-update", "patching", "control"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Reverts to standard reboot-required patching; ensures full restart cycle occurs each month. No security risk from disabling Hotpatch as long as devices are patched via regular WU channel.",
                RegistryKeys = [HotpatchKey],
                ApplyOps  = [RegOp.SetDword(HotpatchKey, "EnableHotPatch", 0)],
                RemoveOps = [RegOp.DeleteValue(HotpatchKey, "EnableHotPatch")],
                DetectOps = [RegOp.CheckDword(HotpatchKey, "EnableHotPatch", 0)],
            },
            new TweakDef
            {
                Id = "hotpatch-require-code-integrity",
                Label = "Require Code Integrity Validation for Hotpatch Modules",
                Category = "System",
                Description = "Enforces Authenticode signature verification for every Hotpatch module before it is loaded into kernel memory. Prevents unsigned or tampered patches from being applied even if a threat actor gains WU delivery access.",
                Tags = ["hotpatch", "code-integrity", "signature", "authenticode", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Strong defence-in-depth: only Microsoft-signed hotpatch binaries can be applied. Has no impact on legitimate Microsoft patches; all Microsoft hotpatches are signed.",
                RegistryKeys = [HotpatchKey],
                ApplyOps  = [RegOp.SetDword(HotpatchKey, "RequireCodeIntegrity", 1)],
                RemoveOps = [RegOp.DeleteValue(HotpatchKey, "RequireCodeIntegrity")],
                DetectOps = [RegOp.CheckDword(HotpatchKey, "RequireCodeIntegrity", 1)],
            },
            new TweakDef
            {
                Id = "hotpatch-block-rollback",
                Label = "Block Hotpatch Rollback to Unpatched State",
                Category = "System",
                Description = "Prevents administrators and automated tools from rolling back applied hotpatch modules to a pre-patched kernel state. Ensures regulatory compliance environments maintain a continuous patched state.",
                Tags = ["hotpatch", "rollback", "compliance", "integrity", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocking rollback ensures continuous kernel-level protection but may complicate incident response if a hotpatch introduces a regression. Test thoroughly before enforcing.",
                RegistryKeys = [HotpatchKey],
                ApplyOps  = [RegOp.SetDword(HotpatchKey, "BlockHotpatchRollback", 1)],
                RemoveOps = [RegOp.DeleteValue(HotpatchKey, "BlockHotpatchRollback")],
                DetectOps = [RegOp.CheckDword(HotpatchKey, "BlockHotpatchRollback", 1)],
            },
            new TweakDef
            {
                Id = "hotpatch-audit-patch-events",
                Label = "Enable Hotpatch Apply and Fail Event Auditing",
                Category = "System",
                Description = "Enables detailed event logging for every Hotpatch application attempt, whether successful or failed. Events include the patch identifier, timestamp, module hash, and failure reason code for SIEM ingestion.",
                Tags = ["hotpatch", "audit", "event-log", "siem", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Non-disruptive; only adds event log entries. Essential for organisations with change-management and patch-tracking compliance requirements.",
                RegistryKeys = [HotpatchKey],
                ApplyOps  = [RegOp.SetDword(HotpatchKey, "EnableHotpatchAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(HotpatchKey, "EnableHotpatchAudit")],
                DetectOps = [RegOp.CheckDword(HotpatchKey, "EnableHotpatchAudit", 1)],
            },
            new TweakDef
            {
                Id = "hotpatch-limit-max-deferred-reboots",
                Label = "Limit Maximum Reboots Deferred by Hotpatch to 2 Baseline Periods",
                Category = "System",
                Description = "Caps the number of consecutive Update Tuesday cycles that Hotpatch can defer a baseline (reboot-required) update. After the configured number of hotpatch-only cycles, a baseline restart is mandated to consolidate all patches.",
                Tags = ["hotpatch", "baseline-reboot", "deferred-restart", "patch-cycle", "control"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents indefinite deferral of reboot-required baselines. Allows 2 hotpatch months before a mandatory restart, balancing uptime and update discipline.",
                RegistryKeys = [HotpatchKey],
                ApplyOps  = [RegOp.SetDword(HotpatchKey, "MaxDeferredBaselineRestarts", 2)],
                RemoveOps = [RegOp.DeleteValue(HotpatchKey, "MaxDeferredBaselineRestarts")],
                DetectOps = [RegOp.CheckDword(HotpatchKey, "MaxDeferredBaselineRestarts", 2)],
            },
            new TweakDef
            {
                Id = "hotpatch-schedule-baseline-restart",
                Label = "Schedule Mandatory Baseline Restart Outside Business Hours",
                Category = "System",
                Description = "Configures hotpatch baseline restarts to occur outside defined active hours (default: 2:00 AM), avoiding interruption of user sessions. When a baseline reboot is required, it is deferred to the next maintenance window.",
                Tags = ["hotpatch", "baseline-reboot", "active-hours", "maintenance-window", "scheduling"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Configures restart timing to 2 AM UTC; pairs with the WU active hours policy to keep machines updated without disrupting users.",
                RegistryKeys = [HotpatchKey],
                ApplyOps  = [RegOp.SetDword(HotpatchKey, "ScheduleBaselineRestartHour", 2)],
                RemoveOps = [RegOp.DeleteValue(HotpatchKey, "ScheduleBaselineRestartHour")],
                DetectOps = [RegOp.CheckDword(HotpatchKey, "ScheduleBaselineRestartHour", 2)],
            },
            new TweakDef
            {
                Id = "hotpatch-disable-telemetry-upload",
                Label = "Disable Hotpatch Telemetry Upload to Microsoft",
                Category = "System",
                Description = "Prevents the Hotpatch subsystem from uploading patch application telemetry, timing data, and failure diagnostics to Microsoft. Retains telemetry locally in the event log only for internal analysis.",
                Tags = ["hotpatch", "telemetry", "privacy", "diagnostic-data", "cloud"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Reduces data shared with Microsoft about kernel patching events. Does not affect hotpatch functionality or reliability; purely a data outflow control.",
                RegistryKeys = [HotpatchKey],
                ApplyOps  = [RegOp.SetDword(HotpatchKey, "DisableHotpatchTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(HotpatchKey, "DisableHotpatchTelemetry")],
                DetectOps = [RegOp.CheckDword(HotpatchKey, "DisableHotpatchTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "hotpatch-exclude-driver-updates",
                Label = "Exclude Driver Updates from Hotpatch Delivery Channel",
                Category = "System",
                Description = "Restricts the Hotpatch delivery channel to security patches only, excluding optional and driver updates. Driver changes often require a full reboot for hardware initialisation; delivering them via Hotpatch risks incomplete initialisation.",
                Tags = ["hotpatch", "driver-updates", "exclusion", "windows-update", "stability"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents unexpected driver-level changes during a hotpatch cycle; driver updates fall through to the next reboot-requiring WU pass.",
                RegistryKeys = [HotpatchKey],
                ApplyOps  = [RegOp.SetDword(HotpatchKey, "ExcludeDriversFromHotpatch", 1)],
                RemoveOps = [RegOp.DeleteValue(HotpatchKey, "ExcludeDriversFromHotpatch")],
                DetectOps = [RegOp.CheckDword(HotpatchKey, "ExcludeDriversFromHotpatch", 1)],
            },
            new TweakDef
            {
                Id = "hotpatch-require-managed-device-enrollment",
                Label = "Require Managed Device Enrollment for Hotpatch Activation",
                Category = "System",
                Description = "Permits Hotpatch activation only on devices enrolled in a compatible MDM solution (Intune, MEM). Unmanaged devices fall back to the standard WU reboot channel. Ensures compliance-tracking for reboot-free patch deployments.",
                Tags = ["hotpatch", "mdm", "intune", "device-enrollment", "compliance"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Ties Hotpatch enrollment to MDM compliance posture; non-enrolled devices are not eligible. Useful for enterprise environments tracking update compliance via Intune.",
                RegistryKeys = [WuKey],
                ApplyOps  = [RegOp.SetDword(WuKey, "RequireManagedDeviceForHotpatch", 1)],
                RemoveOps = [RegOp.DeleteValue(WuKey, "RequireManagedDeviceForHotpatch")],
                DetectOps = [RegOp.CheckDword(WuKey, "RequireManagedDeviceForHotpatch", 1)],
            },
        ];

    }

    // ── HybridJoinDnsPolicy ──
    private static class _HybridJoinDnsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkIsolation";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudAuthentication\HybridJoin";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "hjdns-enable-direct-hybrid-join",
                    Label = "Enable Managed Domain Hybrid Join (No ADFS)",
                    Category = "System",
                    Description =
                        "Enables direct Hybrid Azure AD Join for managed domains without AD FS federation, allowing devices to register with AAD using username/password and SCP discovery.",
                    Tags = ["hybrid-join", "azure-ad", "managed-domain", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Hybrid join enabled for managed domains; no AD FS redirect required.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableDirectHybridJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableDirectHybridJoin")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableDirectHybridJoin", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-block-unregistered-domain-devices",
                    Label = "Block Hybrid Join from Unregistered DNS Domains",
                    Category = "System",
                    Description =
                        "Prevents devices in DNS domains not listed in the Hybrid Join SCP from attempting to register with Azure AD, blocking rogue machines on unknown domains from joining.",
                    Tags = ["hybrid-join", "dns", "domain", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only devices in registered DNS domains can hybrid join; rogue domain machines blocked.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockUnregisteredDomainJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockUnregisteredDomainJoin")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockUnregisteredDomainJoin", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-force-scp-lookup",
                    Label = "Force Service Connection Point Lookup for Hybrid Join",
                    Category = "System",
                    Description =
                        "Forces Azure AD Hybrid Join to use Service Connection Point (SCP) in Active Directory for tenant discovery instead of the local registry, ensuring centrally managed tenant targeting.",
                    Tags = ["hybrid-join", "scp", "active-directory", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "SCP-based tenant discovery enforced; client-side tenant overrides ignored.",
                    ApplyOps = [RegOp.SetDword(Key2, "ForceSCPLookupForTenantDiscovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "ForceSCPLookupForTenantDiscovery")],
                    DetectOps = [RegOp.CheckDword(Key2, "ForceSCPLookupForTenantDiscovery", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-disable-cloud-ap-tenant-override",
                    Label = "Disable Cloud-AP Tenant Override for Hybrid Join",
                    Category = "System",
                    Description =
                        "Blocks user-level Cloud AP (Azure AD Authentication Plugin) tenant override that can redirect a device's hybrid join to a different AAD tenant ID.",
                    Tags = ["hybrid-join", "cloud-ap", "tenant", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Tenant redirect via Cloud AP blocked; join target comes from SCP or policy only.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableCloudAPTenantOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableCloudAPTenantOverride")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableCloudAPTenantOverride", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-isolate-enterprise-endpoints",
                    Label = "Isolate Enterprise Network Endpoints for Cloud Authentication",
                    Category = "System",
                    Description =
                        "Configures Network Isolation policy to classify Microsoft cloud authentication endpoints as enterprise-owned, enabling Windows Information Protection to treat AAD traffic as internal.",
                    Tags = ["hybrid-join", "network-isolation", "wip", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AAD endpoints classified as enterprise; WIP-enforced devices route auth traffic correctly.",
                    ApplyOps = [RegOp.SetDword(Key, "EnterpriseCloudResources", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnterpriseCloudResources")],
                    DetectOps = [RegOp.CheckDword(Key, "EnterpriseCloudResources", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-block-non-domain-dns-fallback",
                    Label = "Block Non-Domain DNS Fallback During Hybrid Join",
                    Category = "System",
                    Description =
                        "Prevents the hybrid join process from falling back to public DNS resolvers when the on-premises DNS server is unavailable, ensuring join only proceeds with trusted DNS resolution.",
                    Tags = ["hybrid-join", "dns-fallback", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hybrid join aborts if domain DNS unreachable; prevents join to wrong tenant via public DNS.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockPublicDNSFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockPublicDNSFallback")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockPublicDNSFallback", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-require-line-of-sight",
                    Label = "Require DC Line-of-Sight for Hybrid Join",
                    Category = "System",
                    Description =
                        "Requires line-of-sight to a domain controller (DC availability check) before allowing the hybrid join registration task to execute, preventing join failures when offline.",
                    Tags = ["hybrid-join", "domain-controller", "offline", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Join task only runs with DC reachable; remote-only machines need internet direct join instead.",
                    ApplyOps = [RegOp.SetDword(Key2, "RequireDCLineOfSight", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RequireDCLineOfSight")],
                    DetectOps = [RegOp.CheckDword(Key2, "RequireDCLineOfSight", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-set-join-timeout",
                    Label = "Set Hybrid Join Task Timeout to 90 Seconds",
                    Category = "System",
                    Description =
                        "Caps the Hybrid Azure AD Join registration task at 90 seconds, preventing long hangs at logon when the join endpoint is unreachable.",
                    Tags = ["hybrid-join", "timeout", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Join task times out after 90 seconds; logon not blocked indefinitely if AAD is unreachable.",
                    ApplyOps = [RegOp.SetDword(Key2, "RegistrationTaskTimeoutSeconds", 90)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RegistrationTaskTimeoutSeconds")],
                    DetectOps = [RegOp.CheckDword(Key2, "RegistrationTaskTimeoutSeconds", 90)],
                },
                new TweakDef
                {
                    Id = "hjdns-disable-joined-device-local-admin",
                    Label = "Disable Local Admin Add via AAD Device Join",
                    Category = "System",
                    Description =
                        "Prevents the automatic addition of the joining user as a local administrator when a device is hybrid-joined to Azure AD, maintaining least-privilege on joined devices.",
                    Tags = ["hybrid-join", "local-admin", "least-privilege", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Joining user not added to local admins on hybrid join; standard user account maintained.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableAutoLocalAdminOnJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableAutoLocalAdminOnJoin")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableAutoLocalAdminOnJoin", 1)],
                },
                new TweakDef
                {
                    Id = "hjdns-enable-hybrid-join-audit",
                    Label = "Enable Hybrid Join Operation Audit Logging",
                    Category = "System",
                    Description =
                        "Enables detailed audit event logging for Hybrid Azure AD Join operations, recording device registration attempts, successes, and failures to the Windows event log.",
                    Tags = ["hybrid-join", "audit", "logging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hybrid join events logged; failed or suspicious join attempts are detectable.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableHybridJoinAuditLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableHybridJoinAuditLogging")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableHybridJoinAuditLogging", 1)],
                },
            ];

    }

    // ── IntuneDeviceEventPolicy ──
    private static class _IntuneDeviceEventPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection\MDM";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "intuneev-enable-device-health-reporting",
                Label = "Intune: Enable Intune Device Health Reporting for Compliance Assessment",
                Category = "System",
                Description = "Sets EnableDeviceHealthReporting=1 in the MDM data collection policy. Enables the Intune client health reporting service which sends device health attestation data — TPM status, Secure Boot state, BitLocker encryption status, ELAM driver state, UEFI firmware version — to the Intune service for compliance policy evaluation. " +
                    "Intune's device compliance policies can gate conditional access (blocking Microsoft 365, SharePoint, or other Entra ID protected resources) based on device health. For health-based conditional access to function, the device must send health attestation reports. Disabling health reporting (or leaving it unconfigured) causes compliance status to show as 'Unknown', which depending on conditional access policy settings may either block all access or allow access by default for unknown-state devices.",
                Tags = ["intune", "mdm", "health-reporting", "compliance", "tpm", "conditional-access"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Intune device health reports sent to service; compliance-based conditional access evaluates correct device health state.",
                ApplyOps = [RegOp.SetDword(Key, "EnableDeviceHealthReporting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDeviceHealthReporting")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDeviceHealthReporting", 1)],
            },
            new TweakDef
            {
                Id = "intuneev-disable-mdm-diagnostic-telemetry-upload",
                Label = "Intune: Disable Voluntary MDM Diagnostic Data Upload to Microsoft",
                Category = "System",
                Description = "Sets DisableMDMDiagnosticsTelemetry=1 in the MDM data collection policy. Stops the Intune MDM client from uploading optional diagnostic data about MDM client performance, error rates, and command processing latency to Microsoft's MDM service telemetry pipeline, separate from Windows diagnostic data. " +
                    "The MDM client telemetry pipeline transmits information about policy processing durations, enrollment command error codes, and sync performance metrics. While this data is used by Microsoft for service improvement and does not contain policy payload content, it reveals information about the organisation's governance structure: how many MDM commands are failing, which policy types are erroring, and whether device compliance is degrading. Disabling this prevents that metadata from leaving the organisation.",
                Tags = ["intune", "mdm", "telemetry", "diagnostic-data", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "MDM client performance telemetry upload stopped; MDM client metadata stays within the organisation.",
                ApplyOps = [RegOp.SetDword(Key, "DisableMDMDiagnosticsTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMDMDiagnosticsTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMDMDiagnosticsTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "intuneev-require-enrollment-certificate",
                Label = "Intune: Require PKI Certificate for MDM Enrollment Authentication",
                Category = "System",
                Description = "Sets RequireMDMEnrollmentCertificate=1 in the MDM data collection policy. Configures the MDM client to use a PKI client certificate issued by the internal CA for Intune enrollment authentication, rather than Microsoft Entra ID token-only authentication, providing a hardware-bound credential (certificate stored in TPM) alongside the Entra token. " +
                    "Token-based MDM enrollment (Entra ID access token only) is subject to token theft attacks — an attacker who steals an Entra ID access token from a device could initiate MDM enrollment of a hostile device. PKI certificate-based enrollment requires the certificate private key (ideally TPM-bound) in addition to the Entra token, making stolen tokens insufficient to enrol a new device because the certificate is non-exportable from the TPM.",
                Tags = ["intune", "mdm", "enrollment", "certificate", "pki", "tpm"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "PKI certificate required for MDM enrollment; token theft alone insufficient to enrol a hostile device.",
                ApplyOps = [RegOp.SetDword(Key, "RequireMDMEnrollmentCertificate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireMDMEnrollmentCertificate")],
                DetectOps = [RegOp.CheckDword(Key, "RequireMDMEnrollmentCertificate", 1)],
            },
            new TweakDef
            {
                Id = "intuneev-enable-mdm-event-audit-log",
                Label = "Intune: Enable MDM Client Audit Logging for Every Policy Command",
                Category = "System",
                Description = "Sets EnableMDMEventAuditLog=1 in the MDM data collection policy. Enables detailed audit logging in the Windows MDM stack, causing every OMA-DM command received from the Intune service (CSP write, CSP delete, configuration profile apply, compliance check result) to generate an audit event in the Security event log. " +
                    "MDM policy delivery happens silently in the background. Without audit logging, there is no on-device record of which policies were applied, when they were applied, which settings were changed, and who authorised the change. This creates a gap in the device's audit trail — changes made via MDM bypass the traditional registry audit trail. With MDM audit logging enabled, all MDM-delivered policy changes generate Security events auditable by SIEM alongside other registry change events.",
                Tags = ["intune", "mdm", "audit-log", "oma-dm", "csp", "siem"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Every MDM OMA-DM policy command generates a Security event; MDM changes included in SIEM correlation.",
                ApplyOps = [RegOp.SetDword(Key, "EnableMDMEventAuditLog", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMDMEventAuditLog")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMDMEventAuditLog", 1)],
            },
            new TweakDef
            {
                Id = "intuneev-block-mdm-unenrollment",
                Label = "Intune: Block User-Initiated MDM Unenrollment from Settings",
                Category = "System",
                Description = "Sets BlockMDMUnenrollment=1 in the MDM data collection policy. Prevents users from manually removing the MDM enrollment from Settings > Accounts > Access work or school, blocking self-service unenrollment that would remove all MDM-delivered policies, compliance baselines, and enterprise configuration from the device. " +
                    "A user who unenrols their device from MDM removes all Intune-delivered policies, certificates, and compliance configurations in a single action. This gives users the ability to escape enterprise security enforcement by removing device management. The device continues to function normally but is no longer managed, no longer receives security patches via Intune, no longer reports compliance, and potentially still has access to enterprise resources if conditional access doesn't immediately detect the unenrollment.",
                Tags = ["intune", "mdm", "unenrollment", "lockout", "compliance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "User-initiated MDM unenrollment blocked; enterprise management cannot be removed from Settings without admin action.",
                ApplyOps = [RegOp.SetDword(Key, "BlockMDMUnenrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockMDMUnenrollment")],
                DetectOps = [RegOp.CheckDword(Key, "BlockMDMUnenrollment", 1)],
            },
            new TweakDef
            {
                Id = "intuneev-enforce-compliance-check-daily",
                Label = "Intune: Enforce Daily MDM Compliance Check-In Regardless of Network",
                Category = "System",
                Description = "Sets EnforceComplianceCheckCadenceHours=24 in the MDM data collection policy. Forces the Intune MDM client to attempt a compliance status check-in to the Intune service at least once every 24 hours, even if the last successful sync was within the standard 8-hour interval, ensuring compliance policy is always evaluated at least daily. " +
                    "MDM sync frequency is typically driven by the Intune service push schedule. Devices that are frequently off the corporate network (remote workers using cellular connections) may go days between Intune syncs if they are not on Wi-Fi and data usage policies are aggressive. A device not syncing for multiple days may have outdated compliance status, allowing it to retain conditional access even after a compliance change (e.g., BitLocker requirement added) that it cannot meet.",
                Tags = ["intune", "mdm", "compliance", "check-in", "cadence", "remote"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "MDM compliance check-in enforced at least daily; compliance status reflects current policy even for remote workers.",
                ApplyOps = [RegOp.SetDword(Key, "EnforceComplianceCheckCadenceHours", 24)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceComplianceCheckCadenceHours")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceComplianceCheckCadenceHours", 24)],
            },
            new TweakDef
            {
                Id = "intuneev-require-signed-mdm-commands",
                Label = "Intune: Require Cryptographic Signing of All OMA-DM Commands",
                Category = "System",
                Description = "Sets RequireSignedMDMCommands=1 in the MDM data collection policy. Requires that all OMA-DM commands received from the MDM server are cryptographically signed with the Intune service certificate, and rejects unsigned or incorrectly signed OMA-DM payloads, protecting against rogue MDM server injection. " +
                    "OMA-DM is the protocol that carries MDM policy commands from the Intune service to the client. Without command signing enforcement, an attacker who achieves a man-in-the-middle position between the endpoint and the Intune service endpoint could inject arbitrary OMA-DM commands (which translate to registry writes, file downloads, and application installs). Requiring signed commands ensures only the authentic Intune service can deliver policy changes.",
                Tags = ["intune", "mdm", "oma-dm", "signing", "mitm", "command-integrity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Unsigned OMA-DM commands rejected; MDM policy injection via man-in-the-middle blocked.",
                ApplyOps = [RegOp.SetDword(Key, "RequireSignedMDMCommands", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedMDMCommands")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSignedMDMCommands", 1)],
            },
            new TweakDef
            {
                Id = "intuneev-enable-mdm-config-lockdown",
                Label = "Intune: Enable MDM Config Lock to Re-Enforce Settings Changed Out-of-Band",
                Category = "System",
                Description = "Sets EnableMDMConfigLockdown=1 in the MDM data collection policy. Enables the MDM config lock feature, which continuously monitors settings delivered by Intune compliance or configuration profiles and automatically reverts any changes made to those settings through other means (GPO that conflicts with MDM, manual registry edits, third-party tools). " +
                    "MDM config lock prevents MDM-delivered settings from being overridden by competing configuration mechanisms. Without config lock, other Group Policy settings delivered via domain join, local GPOs applied by elevated users, or malicious registry edits can override MDM-delivered security baselines. Config lock creates a continuous enforcement loop that re-applies MDM settings whenever they deviate from the expected values, functioning as a security posture self-healing mechanism.",
                Tags = ["intune", "mdm", "config-lock", "drift", "enforcement", "security-baseline"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "MDM config lockdown active; out-of-band registry/GPO changes that conflict with Intune profiles are automatically reverted.",
                ApplyOps = [RegOp.SetDword(Key, "EnableMDMConfigLockdown", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMDMConfigLockdown")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMDMConfigLockdown", 1)],
            },
            new TweakDef
            {
                Id = "intuneev-disable-mdm-agent-auto-update-from-store",
                Label = "Intune: Block MDM Agent Auto-Update from Microsoft Store",
                Category = "System",
                Description = "Sets DisableMDMAgentAutoUpdate=1 in the MDM data collection policy. Prevents the Intune Company Portal and MDM management agent components from auto-updating from the Microsoft Store, requiring IT to control agent updates through managed deployment paths (MDM app profiles, SCCM, or Intune Win32 app) rather than consumer Store delivery. " +
                    "MDM agent updates delivered through the Microsoft Store follow the Store's release schedule independently of IT's testing and validation calendar. A Store-delivered agent update may change MDM enrollment flow, compliance evaluation behaviour, or Company Portal UI in ways that weren't tested by IT's change management process. Blocking auto-update from Store and using managed deployment paths ensures IT controls when MDM agent updates reach production endpoints.",
                Tags = ["intune", "mdm", "company-portal", "auto-update", "store", "change-control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "MDM agent Store updates blocked; Intune Company Portal and agent updates require IT-managed deployment.",
                ApplyOps = [RegOp.SetDword(Key, "DisableMDMAgentAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMDMAgentAutoUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMDMAgentAutoUpdate", 1)],
            },
            new TweakDef
            {
                Id = "intuneev-enable-remote-wipe-audit-log",
                Label = "Intune: Enable Audit Logging for Remote Wipe Commands Received from MDM Server",
                Category = "System",
                Description = "Sets EnableRemoteWipeAuditLog=1 in the MDM data collection policy. Generates a Security event log entry (and application event log warning) the moment the Intune service delivers a remote wipe command to the client, recording the timestamp and wipe type (quick wipe vs full wipe) before the wipe execution begins. " +
                    "Remote wipe is the nuclear security action available through MDM — it erases all device data. Without an audit log entry before execution, there is no on-device evidence that a wipe was initiated via MDM (distinguishable from a local factory reset). In scenarios where a remote wipe was accidental (wrong device targeted in the Intune console) or unauthorised (admin credential compromise), forensic investigation of what happened requires an event record. A pre-wipe audit log event can be captured by a SIEM before the device is erased.",
                Tags = ["intune", "mdm", "remote-wipe", "audit-log", "forensics", "siem"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Security event written when remote wipe command received; SIEM captures wipe initiation before erasure completes.",
                ApplyOps = [RegOp.SetDword(Key, "EnableRemoteWipeAuditLog", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRemoteWipeAuditLog")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRemoteWipeAuditLog", 1)],
            },
        ];

    }

    // ── MdmEnrollmentPolicy ──
    private static class _MdmEnrollmentPolicy
    {
        private const string MdmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";
        private const string WpjKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";
        private const string HelloKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "mdmpol-disable-auto-enroll",
                Label = "Disable Automatic MDM Enrollment on Azure AD Join",
                Category = "System",
                Description =
                    "Prevents the device from automatically enrolling into Mobile Device Management (MDM/Intune) when joined to Azure Active Directory. Requires explicit manual enrollment.",
                Tags = ["mdm", "intune", "azure-ad", "enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Prevents automatic Intune enrollment on Azure AD join; deploy deliberately.",
                RegistryKeys = [MdmKey],
                ApplyOps = [RegOp.SetDword(MdmKey, "AutoEnrollMDM", 0)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "AutoEnrollMDM")],
                DetectOps = [RegOp.CheckDword(MdmKey, "AutoEnrollMDM", 0)],
            },
            new TweakDef
            {
                Id = "mdmpol-disable-user-registration",
                Label = "Disable User-Initiated MDM Registration",
                Category = "System",
                Description =
                    "Prevents users from manually registering the device with a Mobile Device Management server. Only administrators can initiate MDM enrollment.",
                Tags = ["mdm", "enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks self-service MDM registration by users.",
                RegistryKeys = [MdmKey],
                ApplyOps = [RegOp.SetDword(MdmKey, "EnableRegistration", 0)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableRegistration")],
                DetectOps = [RegOp.CheckDword(MdmKey, "EnableRegistration", 0)],
            },
            new TweakDef
            {
                Id = "mdmpol-block-aad-workplace-join",
                Label = "Block Azure AD Workplace Join",
                Category = "System",
                Description =
                    "Prevents the device from being registered with Azure Active Directory as a workplace-joined device. Blocks self-service Azure AD registration from Settings.",
                Tags = ["azure-ad", "workplace-join", "mdm", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Prevents self-service Azure AD registration; may impact BYOD scenarios.",
                RegistryKeys = [WpjKey],
                ApplyOps = [RegOp.SetDword(WpjKey, "BlockAADWorkplaceJoin", 1)],
                RemoveOps = [RegOp.DeleteValue(WpjKey, "BlockAADWorkplaceJoin")],
                DetectOps = [RegOp.CheckDword(WpjKey, "BlockAADWorkplaceJoin", 1)],
            },
            new TweakDef
            {
                Id = "mdmpol-disable-auto-workplace-join",
                Label = "Disable Automatic Workplace Registration",
                Category = "System",
                Description =
                    "Prevents the device from automatically registering with a workplace (Azure AD/Entra ID) during user sign-in. Requires explicit admin-driven join workflow.",
                Tags = ["azure-ad", "workplace-join", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents automatic Azure AD/Entra ID device registration during sign-in.",
                RegistryKeys = [WpjKey],
                ApplyOps = [RegOp.SetDword(WpjKey, "autoWorkplaceJoin", 0)],
                RemoveOps = [RegOp.DeleteValue(WpjKey, "autoWorkplaceJoin")],
                DetectOps = [RegOp.CheckDword(WpjKey, "autoWorkplaceJoin", 0)],
            },
            new TweakDef
            {
                Id = "mdmpol-disable-hello-for-business",
                Label = "Disable Windows Hello for Business",
                Category = "System",
                Description =
                    "Disables Windows Hello for Business (WHFB) enterprise credential provisioning. Users cannot set up WHFB biometrics or PIN backed by PKI infrastructure.",
                Tags = ["windows-hello", "hello-for-business", "credential", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Disables enterprise WHfB provisioning; users cannot set up PKI-backed credentials.",
                RegistryKeys = [HelloKey],
                ApplyOps = [RegOp.SetDword(HelloKey, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(HelloKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(HelloKey, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "mdmpol-require-hello-tpm",
                Label = "Require TPM for Windows Hello for Business",
                Category = "System",
                Description =
                    "Requires a Trusted Platform Module (TPM) chip for Windows Hello for Business provisioning. Prevents software-only (less secure) TPM emulation from being used.",
                Tags = ["windows-hello", "tpm", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Ensures WHfB credentials are TPM-protected, not software-emulated.",
                RegistryKeys = [HelloKey],
                ApplyOps = [RegOp.SetDword(HelloKey, "RequireSecurityDevice", 1)],
                RemoveOps = [RegOp.DeleteValue(HelloKey, "RequireSecurityDevice")],
                DetectOps = [RegOp.CheckDword(HelloKey, "RequireSecurityDevice", 1)],
            },
            new TweakDef
            {
                Id = "mdmpol-disable-hello-pin-recovery",
                Label = "Disable Windows Hello PIN Recovery Service",
                Category = "System",
                Description =
                    "Disables the cloud-based PIN recovery service for Windows Hello. PINs cannot be reset via Microsoft account cloud backup. Keeps credentials fully local.",
                Tags = ["windows-hello", "pin", "recovery", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Keeps PIN credentials fully local; disables cloud recovery backup.",
                RegistryKeys = [HelloKey],
                ApplyOps = [RegOp.SetDword(HelloKey, "EnablePinRecovery", 0)],
                RemoveOps = [RegOp.DeleteValue(HelloKey, "EnablePinRecovery")],
                DetectOps = [RegOp.CheckDword(HelloKey, "EnablePinRecovery", 0)],
            },
            new TweakDef
            {
                Id = "mdmpol-disable-hello-remote",
                Label = "Disable Remote Windows Hello (Phone Sign-In)",
                Category = "System",
                Description =
                    "Disables the Remote Windows Hello feature that allows using a phone or paired device as a sign-in credential for the PC. Available since Windows 10 1607.",
                Tags = ["windows-hello", "remote", "phone", "credential", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables using a paired phone as a PC sign-in credential.",
                RegistryKeys = [$@"{HelloKey}\Remote"],
                ApplyOps = [RegOp.SetDword($@"{HelloKey}\Remote", "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue($@"{HelloKey}\Remote", "Enabled")],
                DetectOps = [RegOp.CheckDword($@"{HelloKey}\Remote", "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "mdmpol-disable-hello-biometrics",
                Label = "Disable Biometrics for Windows Hello",
                Category = "System",
                Description =
                    "Disables the use of biometrics (fingerprint, face recognition) for Windows Hello authentication. PIN remains available as the fallback credential.",
                Tags = ["windows-hello", "biometrics", "fingerprint", "face-id", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Forces PIN-only authentication; no fingerprint or face ID.",
                RegistryKeys = [$@"{HelloKey}\Biometrics"],
                ApplyOps = [RegOp.SetDword($@"{HelloKey}\Biometrics", "UseBiometrics", 0)],
                RemoveOps = [RegOp.DeleteValue($@"{HelloKey}\Biometrics", "UseBiometrics")],
                DetectOps = [RegOp.CheckDword($@"{HelloKey}\Biometrics", "UseBiometrics", 0)],
            },
            new TweakDef
            {
                Id = "mdmpol-disable-dynamic-lock",
                Label = "Disable Dynamic Lock (Phone Proximity Lock)",
                Category = "System",
                Description =
                    "Disables Dynamic Lock, which automatically locks the PC when a paired Bluetooth phone moves out of range. Prevents unintended automatic locking in enterprise environments.",
                Tags = ["dynamic-lock", "bluetooth", "lock", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables Bluetooth proximity-based automatic PC locking.",
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock", 0)],
            },
        ];

    }

    // ── MdmRegistrationPolicy ──
    private static class _MdmRegistrationPolicy
    {
        private const string MdmKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

        private const string EnrollKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDMEnrollment";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "mdmreg-enable-aad-auto-enrollment",
                    Label = "MDM Registration: Enable Auto-Enrollment for Azure AD Joined Devices",
                    Category = "System",
                    Description =
                        "Sets AutoEnrollMDM=1 in MDM policy. Enables automatic MDM enrollment for devices that join Azure AD (Azure AD Join or Azure AD Hybrid Join). When a device joins Azure AD, the enrollment process automatically provisions the device with an MDM enrolment token and registers it with the configured MDM authority (typically Microsoft Intune). Without this policy, AAD Joined devices are registered in Azure AD but not MDM-managed — group policy, compliance checks, and app deployments via Intune will not work. Auto-enrollment is the standard corporate device onboarding mechanism.",
                    Tags = ["mdm", "auto-enrollment", "azure-ad", "intune", "device-management"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "AAD-joined devices automatically enroll in MDM upon sign-in. The MDM authority URL and scope are read from the tenant's MDM discovery service. Requires an Intune license (or other MDM) assigned to the user. Devices already AAD-joined will not retroactively enroll — only newly joining devices are affected.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "AutoEnrollMDM", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "AutoEnrollMDM")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "AutoEnrollMDM", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-require-reenrollment-on-rename",
                    Label = "MDM Registration: Require Re-Enrollment after Device Rename",
                    Category = "System",
                    Description =
                        "Sets RequireReenrollmentOnRename=1 in MDM policy. Forces the device to re-enroll in MDM when the device name changes. Device renaming is sometimes used as a pivot technique during lateral movement: an attacker renames a managed device to match an expected device name to pass name-based access controls. Forcing re-enrollment on rename ensures the MDM service receives a new enrollment token for the renamed device, which updates the device record in the MDM database and triggers compliance re-evaluation. Any conditional access policies that check the MDM enrollment record are therefore aware of the identity change.",
                    Tags = ["mdm", "re-enrollment", "device-rename", "identity", "conditional-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Device renaming triggers MDM re-enrollment. Re-enrollment is transparent — it occurs in the background without disrupting the user session. Useful in environments where device names are used as identifiers in network access rules or SIEM queries.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "RequireReenrollmentOnRename", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "RequireReenrollmentOnRename")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "RequireReenrollmentOnRename", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-disable-user-unenrollment",
                    Label = "MDM Registration: Prevent Users from Manually Unenrolling Device from MDM",
                    Category = "System",
                    Description =
                        "Sets DisallowUserMdmUnenrollment=1 in MDM policy. Prevents standard users (non-administrators) from unenrolling the device from MDM management through the Settings app. Without this policy, any user with access to Settings > Accounts > Access work or school can disconnect the device from MDM management, effectively removing it from IT control, compliance enforcement, and conditional access scope. While admins can still unenroll via MDM push commands, preventing user-initiated unenrollment ensures the device remains managed.",
                    Tags = ["mdm", "unenrollment", "user-restriction", "settings", "tamper-prevention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot disconnect the device from MDM using Settings. Local administrators and MDM push-initiated unenrollment still work. The Settings UI option to disconnect is grayed out or removed for non-admins.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "DisallowUserMdmUnenrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "DisallowUserMdmUnenrollment")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "DisallowUserMdmUnenrollment", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-use-enterprise-enrollment-only",
                    Label = "MDM Registration: Restrict MDM Enrollment to Enterprise Tenants Only",
                    Category = "System",
                    Description =
                        "Sets EnterpriseEnrollmentOnly=1 in MDM policy. Restricts MDM enrollment so that only corporate tenants (as determined by the MDM authority in the Group Policy or the domain's MDM discovery service) can claim management of the device. Without this policy, a device can be enrolled by any MDM provider, including personal Intune accounts. This is relevant in bring-your-own-device (BYOD) scenarios where an employee might accidentally enroll their managed corporate device with their personal Microsoft 365 account's MDM, causing policy conflicts.",
                    Tags = ["mdm", "enrollment", "enterprise-only", "byod", "tenant-restriction"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only the corporate MDM authority (set by Group Policy or Windows AutoPilot) can enroll the device. Personal Microsoft account MDM enrollment is rejected. Prevents accidental dual-enrollment or policy conflicts from personal MDM tenants.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnterpriseEnrollmentOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnterpriseEnrollmentOnly")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnterpriseEnrollmentOnly", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-enable-diagnostic-auto-upload",
                    Label = "MDM Registration: Enable Automatic Diagnostic Log Upload to MDM",
                    Category = "System",
                    Description =
                        "Sets EnableDiagnosticUpload=1 in MDM policy. Enables the MDM client to automatically upload MDM diagnostic logs to the MDM server when requested via a remote log collection push from the MDM authority. Without this, IT admins must physically access the device or use complex manual collection procedures to retrieve MDM diagnostic files. With this enabled, an MDM admin can trigger log collection from the Intune console without user interaction — essential for diagnosing enrollment failures, policy application errors, or app deployment problems on devices that are not physically accessible.",
                    Tags = ["mdm", "diagnostics", "log-upload", "remote-collection", "intune"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "MDM diagnostic logs are uploaded to the MDM server on remote request. Logs include MDM client logs, Event Log snapshots, and enrollment logs. Only the MDM server can initiate collection — users cannot trigger it. Small bandwidth overhead during collection.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnableDiagnosticUpload", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableDiagnosticUpload")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnableDiagnosticUpload", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-set-enrollment-check-in-interval-4h",
                    Label = "MDM Registration: Set MDM Check-In Interval to 4 Hours",
                    Category = "System",
                    Description =
                        "Sets EnrollmentCheckInIntervalHours=4 in MDM policy. Sets the frequency at which the MDM client checks in with the MDM server to receive new policies, app assignments, compliance commands, and configuration updates. The default check-in interval is 8 hours. A 4-hour interval reduces the lag between MDM policy changes (such as blocking USB, pushing a security update requirement, or revering a credential) and their application on devices. In incident response scenarios, the ability to push a policy change and have it take effect within 4 hours rather than 8 hours is a meaningful response time improvement.",
                    Tags = ["mdm", "check-in", "policy-apply", "interval", "response-time"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "MDM client checks in with the MDM server every 4 hours. Reduces policy propagation lag from 8h to 4h. Slightly higher MDM service traffic — negligible for typical enterprise deployments.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnrollmentCheckInIntervalHours", 4)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnrollmentCheckInIntervalHours")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnrollmentCheckInIntervalHours", 4)],
                },
                new TweakDef
                {
                    Id = "mdmreg-enable-conditional-access-notification",
                    Label = "MDM Registration: Enable MDM Enrollment Notification for Conditional Access",
                    Category = "System",
                    Description =
                        "Sets NotifyConditionalAccessOnEnrollment=1 in MDM policy. Configures the MDM client to push an enrollment state notification to the Azure AD conditional access service whenever the device's MDM enrollment status changes (enrolled, unenrolled, compliance state changed). Without this notification push, conditional access relies on polling of the Intune device inventory, which has a delay. The push notification significantly reduces the time between an enrollment state change and the conditional access enforcement update — important for scenarios like immediately restoring access after successful compliance remediation.",
                    Tags = ["mdm", "conditional-access", "enrollment-notification", "aad", "response-time"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enrollment state changes trigger an immediate push notification to AAD conditional access. Reduces the delay between compliance remediation and access restoration. Requires AAD and Intune integration — has no effect on on-premises MDM without AAD integration.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "NotifyConditionalAccessOnEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "NotifyConditionalAccessOnEnrollment")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "NotifyConditionalAccessOnEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-block-guest-from-enrollment",
                    Label = "MDM Registration: Block Guest Accounts from MDM Enrollment",
                    Category = "System",
                    Description =
                        "Sets BlockGuestAccountEnrollment=1 in MDM policy. Prevents Guest accounts from triggering MDM enrollment or accessing MDM-managed resources. Guest accounts by definition have no AAD identity and should not enroll in MDM. In some configurations, a device with an active Guest session can inadvertently trigger MDM enrollment flows with an empty principal, creating orphaned device records in the MDM tenant. Blocking guest account enrollment eliminates this edge case and prevents Guest-session processes from interacting with the MDM client.",
                    Tags = ["mdm", "guest-account", "enrollment-block", "identity", "orphaned-device"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Guest accounts cannot initiate or complete MDM enrollment. Prevents orphaned MDM device records from Guest-triggered enrollment flows. No impact on standard user or administrator enrollment processes.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "BlockGuestAccountEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "BlockGuestAccountEnrollment")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "BlockGuestAccountEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-enable-silent-enrollment",
                    Label = "MDM Registration: Enable Silent (No User Prompt) MDM Enrollment",
                    Category = "System",
                    Description =
                        "Sets EnableSilentEnrollment=1 in MDM policy. Configures MDM enrollment to complete silently without displaying user-facing dialogs, progress indicators, or consent prompts. Silent enrollment is used in corporate provisioning scenarios (Autopilot, bulk enrolment) where the device is pre-configured by IT before delivery to the user. Without silent enrollment, the MDM client shows enrollment progress dialogs that may alarm users who are not expecting them. Silent enrollment also reduces the risk of users cancelling the enrollment process mid-flow, which can leave the device in a partially-enrolled state.",
                    Tags = ["mdm", "silent-enrollment", "autopilot", "provisioning", "user-experience"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "MDM enrollment completes without user-visible dialogs or prompts. Used in Autopilot and bulk enrollment scenarios. Best combined with Enrollment Status Page (ESP) for user transparency during provisioning.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "EnableSilentEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableSilentEnrollment")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "EnableSilentEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "mdmreg-enable-enrollment-retry-on-failure",
                    Label = "MDM Registration: Enable Automatic Retry on MDM Enrollment Failure",
                    Category = "System",
                    Description =
                        "Sets EnableEnrollmentRetryOnFailure=1 in EnrollmentSecurity policy. Enables the MDM client to automatically retry enrollment if the initial enrollment attempt fails due to network connectivity issues, MDM service transient errors, or AAD token acquisition failures. Without retry logic, a single transient failure during Autopilot provisioning (e.g., the device starts enrollment before DNS is fully resolving, or the MDM service returns HTTP 503 during a brief outage) results in a permanently unenrolled device that requires manual remediation. Automatic retry ensures transient failures are recovered without IT intervention.",
                    Tags = ["mdm", "enrollment-retry", "resilience", "autopilot", "transient-failure"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Failed MDM enrollment attempts are automatically retried with exponential backoff. Significantly reduces Autopilot and bulk-enrollment failures due to transient connectivity or service errors. Retry schedule is governed by the MDM client's built-in backoff policy.",
                    ApplyOps = [RegOp.SetDword(EnrollKey, "EnableEnrollmentRetryOnFailure", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnrollKey, "EnableEnrollmentRetryOnFailure")],
                    DetectOps = [RegOp.CheckDword(EnrollKey, "EnableEnrollmentRetryOnFailure", 1)],
                },
            ];

    }

    // ── OobePolicy ──
    private static class _OobePolicy
    {
        private const string OobeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OOBE";
        private const string SetupKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Setup";
        private const string ShellLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Shell";
        private const string ShellCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Shell";
        private const string SrvMgrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Server Manager";
        private const string SystemPolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "oobe-disable-privacy-experience",
                Label = "Disable OOBE Privacy Experience",
                Category = "System",
                Description =
                    "Sets DisablePrivacyExperience=1 in the Windows OOBE policy key. "
                    + "Prevents the full-screen privacy settings wizard from appearing on first sign-in for new user accounts "
                    + "(covers Diagnostic Data, Inking, Location, and related consent screens). "
                    + "Default: absent (privacy wizard shown). Recommended: 1 on domain-joined or company-provisioned devices.",
                Tags = ["oobe", "privacy", "first-run", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Skips the OOBE privacy consent wizard on first sign-in; privacy settings remain at system defaults.",
                ApplyOps = [RegOp.SetDword(OobeKey, "DisablePrivacyExperience", 1)],
                RemoveOps = [RegOp.DeleteValue(OobeKey, "DisablePrivacyExperience")],
                DetectOps = [RegOp.CheckDword(OobeKey, "DisablePrivacyExperience", 1)],
            },
            new TweakDef
            {
                Id = "oobe-skip-user-oobe",
                Label = "Skip User OOBE Page",
                Category = "System",
                Description =
                    "Sets SkipUserOOBE=1 in the Windows OOBE policy key. "
                    + "Suppresses the user portion of the Out-of-Box Experience wizard, skipping personalization and "
                    + "account setup prompts at first logon for new local users. "
                    + "Default: absent (user OOBE shown). Recommended: 1 on pre-provisioned enterprise desktops.",
                Tags = ["oobe", "first-run", "setup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Skips user-facing OOBE setup prompts; account is still created with system defaults.",
                ApplyOps = [RegOp.SetDword(OobeKey, "SkipUserOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(OobeKey, "SkipUserOOBE")],
                DetectOps = [RegOp.CheckDword(OobeKey, "SkipUserOOBE", 1)],
            },
            new TweakDef
            {
                Id = "oobe-skip-machine-oobe",
                Label = "Skip Machine OOBE Page",
                Category = "System",
                Description =
                    "Sets SkipMachineOOBE=1 in the Windows OOBE policy key. "
                    + "Suppresses the machine-level portion of the OOBE wizard during initial Windows setup, "
                    + "skipping device configuration prompts such as region and language when a response answer file is in use. "
                    + "Default: absent (machine OOBE shown). Recommended: 1 in MDT/WDS/Autopilot deployments.",
                Tags = ["oobe", "setup", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Skips machine-level OOBE prompts during Windows setup; used mainly in imaging/provisioning scenarios.",
                ApplyOps = [RegOp.SetDword(OobeKey, "SkipMachineOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(OobeKey, "SkipMachineOOBE")],
                DetectOps = [RegOp.CheckDword(OobeKey, "SkipMachineOOBE", 1)],
            },
            new TweakDef
            {
                Id = "oobe-no-network-connections-wizard",
                Label = "Disable OOBE Network Connections Wizard",
                Category = "System",
                Description =
                    "Sets DisableNetworkConnectionsWizard=1 in the Windows OOBE policy key. "
                    + "Suppresses the network connection setup wizard that appears during the OOBE phase, "
                    + "useful when network configuration is handled by MDM or answer files. "
                    + "Default: absent (network wizard shown). Recommended: 1 in managed deployment scenarios.",
                Tags = ["oobe", "network", "wizard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Skips the OOBE network setup wizard; network connectivity is handled by provisioning tools.",
                ApplyOps = [RegOp.SetDword(OobeKey, "DisableNetworkConnectionsWizard", 1)],
                RemoveOps = [RegOp.DeleteValue(OobeKey, "DisableNetworkConnectionsWizard")],
                DetectOps = [RegOp.CheckDword(OobeKey, "DisableNetworkConnectionsWizard", 1)],
            },
            new TweakDef
            {
                Id = "oobe-no-first-logon-animation",
                Label = "Disable First Logon Animation",
                Category = "System",
                Description =
                    "Sets ShowFirstLogonAnimation=0 in the Windows Setup policy key. "
                    + "Disables the full-screen 'Hi' and 'Getting Windows ready' animation sequence shown to new users on first sign-in, "
                    + "reducing the wait time at initial logon. "
                    + "Default: absent (animation shown). Recommended: 0 on corporate desktops for faster first-logon.",
                Tags = ["oobe", "animation", "first-logon", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Skips the first-logon animation; new users reach the desktop faster on initial sign-in.",
                ApplyOps = [RegOp.SetDword(SetupKey, "ShowFirstLogonAnimation", 0)],
                RemoveOps = [RegOp.DeleteValue(SetupKey, "ShowFirstLogonAnimation")],
                DetectOps = [RegOp.CheckDword(SetupKey, "ShowFirstLogonAnimation", 0)],
            },
            new TweakDef
            {
                Id = "oobe-no-welcome-screen-lm",
                Label = "Disable Welcome Screen (Machine)",
                Category = "System",
                Description =
                    "Sets NoWelcomeScreen=1 in the machine-scoped Windows Shell policy key. "
                    + "Suppresses the Windows Welcome Center / Did You Know tips overlay that could appear post-setup. "
                    + "Default: absent. Recommended: 1 on managed enterprise desktops to skip unneeded first-run UI.",
                Tags = ["oobe", "welcome", "shell", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Suppresses the Welcome Center overlay for all users; no functional impact after initial run.",
                ApplyOps = [RegOp.SetDword(ShellLm, "NoWelcomeScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(ShellLm, "NoWelcomeScreen")],
                DetectOps = [RegOp.CheckDword(ShellLm, "NoWelcomeScreen", 1)],
            },
            new TweakDef
            {
                Id = "oobe-no-welcome-screen-user",
                Label = "Disable Welcome Screen (Current User)",
                Category = "System",
                Description =
                    "Sets NoWelcomeScreen=1 in the per-user Windows Shell policy key. "
                    + "Hides the Welcome Center / Getting Started experience for the current user. "
                    + "Default: absent. Recommended: 1 for individual user profiles on managed systems.",
                Tags = ["oobe", "welcome", "shell", "user", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Suppresses the Welcome Center for this user only; no functional impact.",
                ApplyOps = [RegOp.SetDword(ShellCu, "NoWelcomeScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(ShellCu, "NoWelcomeScreen")],
                DetectOps = [RegOp.CheckDword(ShellCu, "NoWelcomeScreen", 1)],
            },
            new TweakDef
            {
                Id = "oobe-no-server-manager-at-logon",
                Label = "Disable Server Manager Auto-Open at Logon",
                Category = "System",
                Description =
                    "Sets DoNotOpenServerManagerAtLogon=1 in the Server Manager policy key. "
                    + "Prevents Windows Server Manager from automatically opening at every administrator logon. "
                    + "Applies to Windows Server editions; setting is ignored on Windows Client. "
                    + "Default: absent (Server Manager opens at logon). Recommended: 1 on production servers where automatic windows interfere with operations.",
                Tags = ["oobe", "server-manager", "server", "logon", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Server Manager no longer opens automatically at logon; it can still be launched manually.",
                ApplyOps = [RegOp.SetDword(SrvMgrKey, "DoNotOpenServerManagerAtLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(SrvMgrKey, "DoNotOpenServerManagerAtLogon")],
                DetectOps = [RegOp.CheckDword(SrvMgrKey, "DoNotOpenServerManagerAtLogon", 1)],
            },
            new TweakDef
            {
                Id = "oobe-disable-balloon-tips",
                Label = "Disable System Tray Balloon Tips",
                Category = "System",
                Description =
                    "Sets EnableBalloonTips=0 in the machine-side System policy key. "
                    + "Suppresses all Action Center / notification area balloon notifications and first-run tip balloons "
                    + "that appear after the initial desktop load. "
                    + "Default: absent (balloon tips enabled). Recommended: 0 on corporate desktops to reduce user interruptions.",
                Tags = ["oobe", "balloon", "notifications", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses first-run balloon tips and system tray notifications; Action Center itself is unaffected.",
                ApplyOps = [RegOp.SetDword(ShellLm, "EnableBalloonTips", 0)],
                RemoveOps = [RegOp.DeleteValue(ShellLm, "EnableBalloonTips")],
                DetectOps = [RegOp.CheckDword(ShellLm, "EnableBalloonTips", 0)],
            },
            new TweakDef
            {
                Id = "oobe-disable-upgrade-ui",
                Label = "Disable Windows Upgrade Prompt UI",
                Category = "System",
                Description =
                    "Sets DisableUXFirstRunAnimation=1 in the Windows Setup policy key. "
                    + "Suppresses the upgrade experience UX animations and first-run prompts that may appear "
                    + "after a major Windows feature update is applied to an existing account. "
                    + "Default: absent. Recommended: 1 on managed devices receiving OS updates via WSUS / Autopilot.",
                Tags = ["oobe", "upgrade", "animation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Suppresses post-upgrade UX first-run animations; system functionality is unaffected.",
                ApplyOps = [RegOp.SetDword(SetupKey, "DisableUXFirstRunAnimation", 1)],
                RemoveOps = [RegOp.DeleteValue(SetupKey, "DisableUXFirstRunAnimation")],
                DetectOps = [RegOp.CheckDword(SetupKey, "DisableUXFirstRunAnimation", 1)],
            },
        ];

    }

    // ── RetailDemoPolicy ──
    private static class _RetailDemoPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RetailDemo";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "rdemo-disable-retail-demo",
                Label = "Disable Retail Demo Mode",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets NoRetailDemo=1 in the RetailDemo policy key. Prevents Windows from "
                    + "entering retail demo mode, which runs a continuous promotional demonstration "
                    + "experience on display units in retail stores. Demo mode overrides normal user "
                    + "experience settings and launches curated content. Default: 0 (demo mode "
                    + "allowed by OEM configuration). Recommended: 1 on corporate and personal "
                    + "devices to block any inadvertent demo-mode activation.",
                Tags = ["retail-demo", "kiosk", "policy", "display"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRetailDemo", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRetailDemo")],
                DetectOps = [RegOp.CheckDword(Key, "NoRetailDemo", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-attract-loop",
                Label = "Disable Retail Demo Attract Loop",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoAttractLoop=1 in the RetailDemo policy key. Stops the idle attract-loop "
                    + "video that plays on unattended retail display machines to showcase Windows "
                    + "features and invite customer interaction. On non-retail devices this loop "
                    + "would trigger after inactivity timeout and play promotional video full-screen. "
                    + "Default: 0. Recommended: 1 on any device that is not a retail display unit.",
                Tags = ["retail-demo", "attract", "video", "idle", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoAttractLoop", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoAttractLoop")],
                DetectOps = [RegOp.CheckDword(Key, "NoAttractLoop", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-auto-signin",
                Label = "Disable Retail Demo Auto Sign-In",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets NoAutoSignIn=1 in the RetailDemo policy key. Prevents a demo account from "
                    + "automatically signing in on boot, which is a behaviour specific to retail "
                    + "store display units running in unattended self-service mode. Automatic demo "
                    + "sign-in bypasses normal login prompts and loads a preconfigured experience "
                    + "account. Default: 0. Recommended: 1 on devices requiring secure authentication.",
                Tags = ["retail-demo", "sign-in", "auto-login", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoAutoSignIn", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoAutoSignIn")],
                DetectOps = [RegOp.CheckDword(Key, "NoAutoSignIn", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-demo-apps",
                Label = "Disable Retail Demo App Provisioning",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoRetailDemoApps=1 in the RetailDemo policy key. Prevents provisioning of "
                    + "Microsoft-curated demo applications installed on retail display machines to "
                    + "showcase Windows and Microsoft 365 features. These apps are silently installed "
                    + "from the Store without user consent on retail-configured devices. "
                    + "Default: 0. Recommended: 1 on enterprise and personal devices.",
                Tags = ["retail-demo", "apps", "store", "provisioning", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRetailDemoApps", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRetailDemoApps")],
                DetectOps = [RegOp.CheckDword(Key, "NoRetailDemoApps", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-demo-content",
                Label = "Disable Retail Demo Content Delivery",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoRetailDemoContent=1 in the RetailDemo policy key. Prevents Windows from "
                    + "downloading and staging promotional content packages from Microsoft CDN for "
                    + "use in retail demo scenarios. These background downloads consume network "
                    + "bandwidth and disk space without user awareness on non-retail devices. "
                    + "Default: 0. Recommended: 1 on metered or managed connections.",
                Tags = ["retail-demo", "content", "cdn", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRetailDemoContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRetailDemoContent")],
                DetectOps = [RegOp.CheckDword(Key, "NoRetailDemoContent", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-experience-provider",
                Label = "Disable Retail Demo Device Experience Provider",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoDeviceExperienceProvider=1 in the RetailDemo policy key. Blocks the "
                    + "Device Experience Provider component used by OEM retail configurations to "
                    + "display branded hardware demonstrations and guided tours during initial setup "
                    + "in stores. This provider can launch demo walkthroughs without user action. "
                    + "Default: 0. Recommended: 1 on post-purchase devices.",
                Tags = ["retail-demo", "experience", "oem", "provider", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoDeviceExperienceProvider", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoDeviceExperienceProvider")],
                DetectOps = [RegOp.CheckDword(Key, "NoDeviceExperienceProvider", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-demo-banner",
                Label = "Disable Retail Demo Info Banner",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets NoDemoBanner=1 in the RetailDemo policy key. Hides the informational "
                    + "banner shown in retail demo mode that prompts customers to purchase the "
                    + "device or explore Windows features being demonstrated on the floor model. "
                    + "This UI element is irrelevant and distracting on owned devices. "
                    + "Default: 0. Recommended: 1 on non-retail hardware.",
                Tags = ["retail-demo", "banner", "ui", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoDemoBanner", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoDemoBanner")],
                DetectOps = [RegOp.CheckDword(Key, "NoDemoBanner", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-oobe-demo",
                Label = "Disable Retail Demo OOBE Flow",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoRetailOOBE=1 in the RetailDemo policy key. Prevents the out-of-box "
                    + "experience from branching into retail demo setup mode during initial device "
                    + "configuration. The retail OOBE path creates a temporary guest demo account "
                    + "and loads promotional assets instead of standard setup. "
                    + "Default: 0 for OEM-configured retail images. Recommended: 1 everywhere else.",
                Tags = ["retail-demo", "oobe", "setup", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRetailOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRetailOOBE")],
                DetectOps = [RegOp.CheckDword(Key, "NoRetailOOBE", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-cleanup-revert",
                Label = "Disable Retail Demo Cleanup Revert Task",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoCleanupRevert=1 in the RetailDemo policy key. Blocks the scheduled "
                    + "retail demo cleanup task that runs after business hours to wipe user "
                    + "interactions and restore the machine to factory demo defaults. On non-retail "
                    + "devices this task would destructively remove user customisations and data "
                    + "overnight. Default: 0. Recommended: 1 on all personal and managed devices.",
                Tags = ["retail-demo", "cleanup", "scheduled-task", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoCleanupRevert", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoCleanupRevert")],
                DetectOps = [RegOp.CheckDword(Key, "NoCleanupRevert", 1)],
            },
            new TweakDef
            {
                Id = "rdemo-disable-demo-telemetry",
                Label = "Disable Retail Demo Interaction Telemetry",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoRetailTelemetry=1 in the RetailDemo policy key. Stops transmission of "
                    + "retail demo interaction analytics (which buttons were pressed, time spent on "
                    + "demo scenes, feature engagement) to Microsoft. This retail-specific telemetry "
                    + "stream is separate from the standard Windows diagnostic data pipeline and "
                    + "continues even when diagnostic level is set to Basic. "
                    + "Default: 0. Recommended: 1 for privacy.",
                Tags = ["retail-demo", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRetailTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRetailTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "NoRetailTelemetry", 1)],
            },
        ];

    }

    // ── SharedPCPolicy ──
    private static class _SharedPCPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SharedPC";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "shpc-disable-shared-pc-mode",
                Label = "Disable Shared PC Mode",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Shared PC mode optimizes Windows for use by multiple users on a single device, enabling account deletion and profile cleanup between sessions. Disabling Shared PC mode ensures a standard single-user workstation configuration is enforced on dedicated corporate devices. Shared PC mode behaviors including account deletion and profile compression are inappropriate for dedicated workstations where user data persistence is required. This policy is relevant for environments that may inadvertently inherit shared PC settings from imported operating system images. Dedicated workstations should operate in standard single-user mode to preserve user profile data and application settings between sessions. All standard user profile and session management behaviors remain active when Shared PC mode is disabled.",
                Tags = ["shared-pc", "kiosk", "profiles", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSharedPCMode", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSharedPCMode")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSharedPCMode", 0)],
            },
            new TweakDef
            {
                Id = "shpc-zero-disk-deletion-level",
                Label = "Disable Disk Level Deletion",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Shared PC disk level deletion determines how aggressively user profiles and cached data are removed when disk space falls below configured thresholds. Setting disk level deletion to zero disables automatic account and profile deletion based on disk pressure. Dedicated workstations with persistent user profiles require that data not be deleted without explicit administrative action. Automatic deletion of user profiles in shared PC mode can cause data loss if users inadvertently leave unsynchronized files on the local device. Enterprises managing user data retention policies should handle profile lifecycle through MDM or Group Policy rather than automatic deletion thresholds. This setting preserves user data integrity on devices transitioned out of shared mode.",
                Tags = ["shared-pc", "disk", "profiles", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DiskLevelDeletion", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DiskLevelDeletion")],
                DetectOps = [RegOp.CheckDword(Key, "DiskLevelDeletion", 0)],
            },
            new TweakDef
            {
                Id = "shpc-zero-disk-caching-level",
                Label = "Disable Disk Level Caching",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Shared PC disk level caching controls the threshold at which the shared PC cleanup service begins compressing or removing cached profile data. Setting disk level caching to zero disables threshold-triggered caching operations managed by the Shared PC service. Dedicated workstations do not require disk caching thresholds as profile cleanup is managed through standard operating system mechanisms. Shared PC caching behaviors can unexpectedly compress user profile directories, causing application state loss. Disabling this threshold prevents the Shared PC cache management service from interfering with normal profile operations. Standard Windows disk management and profile management policies govern storage usage when shared PC caching is off.",
                Tags = ["shared-pc", "caching", "profiles", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DiskLevelCaching", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DiskLevelCaching")],
                DetectOps = [RegOp.CheckDword(Key, "DiskLevelCaching", 0)],
            },
            new TweakDef
            {
                Id = "shpc-zero-inactive-threshold",
                Label = "Disable Inactive User Threshold",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The inactive threshold in Shared PC mode determines how many days of inactivity trigger automatic account deletion during maintenance windows. Setting this to zero disables time-based account deletion entirely in the Shared PC policy framework. Dedicated workstations maintain persistent user profiles and should not automatically delete accounts based on inactivity. User account lifecycle management on dedicated devices is handled through Active Directory and HR-driven deprovisioning processes. Automatic account deletion without enterprise coordination can violate data governance and audit requirements. This setting ensures user accounts persist until explicitly deprovisioned through proper IT workflows.",
                Tags = ["shared-pc", "accounts", "profiles", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "InactiveThreshold", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "InactiveThreshold")],
                DetectOps = [RegOp.CheckDword(Key, "InactiveThreshold", 0)],
            },
            new TweakDef
            {
                Id = "shpc-zero-max-page-file-mb",
                Label = "Disable Shared PC Max Page File Size",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Shared PC mode can impose a maximum page file size restriction to conserve disk space when multiple user profiles share limited storage. Setting this to zero removes the Shared PC imposed page file size ceiling. Dedicated workstations managed by separate page file policies should not have an additional Shared PC constraint overriding the configured page file size. Conflicting page file size policies can result in insufficient virtual memory for workloads that exceed the Shared PC imposed ceiling. Dedicated workstation page file sizing should be governed by the PageFile policy settings or system defaults exclusively. Removing the Shared PC page file restriction ensures only the authoritative page file policy applies.",
                Tags = ["shared-pc", "pagefile", "memory", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxPageFileSizeMB", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxPageFileSizeMB")],
                DetectOps = [RegOp.CheckDword(Key, "MaxPageFileSizeMB", 0)],
            },
            new TweakDef
            {
                Id = "shpc-delete-guest-on-logoff",
                Label = "Delete Guest Account on Logoff",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Guest accounts created for temporary access accumulate profile data and application state that persists across sessions if not cleaned up. Enabling deletion of the guest account on logoff ensures no data from guest sessions persists on the device after the guest logs out. This policy is a security best practice for devices used in public or semi-public environments where guest access is permitted. Residual guest profile data could contain sensitive information browsed or downloaded during the guest session. Devices in public access areas such as lobbies, libraries, and conference rooms benefit most from automatic guest cleanup. Combining this setting with other Shared PC policies creates a comprehensive ephemeral session environment.",
                Tags = ["shared-pc", "guest", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DeleteGuestAccountOnLogoff", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DeleteGuestAccountOnLogoff")],
                DetectOps = [RegOp.CheckDword(Key, "DeleteGuestAccountOnLogoff", 1)],
            },
            new TweakDef
            {
                Id = "shpc-restrict-local-storage",
                Label = "Restrict Local Storage Access",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Restricting local storage in Shared PC environments prevents users from saving large amounts of data to the local disk, preserving space for system operations. This policy is particularly important for shared devices where multiple user profiles compete for limited storage capacity. Combining local storage restriction with cloud synchronization policies directs user data to centrally managed repositories. Shared devices in classroom or kiosk configurations benefit from storage restrictions that prevent individual users from consuming the entire disk. Users on restricted devices still have access to their cloud-synchronized documents and files through mobile clients. The restriction does not prevent normal application usage, only limits the growth of user-created local content.",
                Tags = ["shared-pc", "storage", "kiosk", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictLocalStorage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictLocalStorage")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictLocalStorage", 1)],
            },
            new TweakDef
            {
                Id = "shpc-disable-enabled-flag",
                Label = "Disable Shared PC Enabled Flag",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "The Enabled flag in Shared PC policy acts as a master switch that activates all other Shared PC behaviors including account management and cleanup. Setting this flag to zero disables the entire Shared PC policy framework on the device. Dedicated workstations should have Shared PC mode fully disabled to prevent any unintended overlap with shared device behaviors. Disabling the Enabled flag supersedes other individual Shared PC policy settings. This setting is appropriate as a cleanup measure when migrating devices from shared configurations to dedicated single-user deployments. Standard enterprise workstation management takes over all session and account management when Shared PC is fully disabled.",
                Tags = ["shared-pc", "kiosk", "accounts", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                DetectOps = [RegOp.CheckDword(Key, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "shpc-clear-kiosk-aumid",
                Label = "Clear Kiosk Mode Application ID",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The Kiosk Mode AUMID specifies the application that runs in full-screen kiosk mode when Shared PC kiosk configuration is active. Clearing this value removes any configured kiosk application assignment from the Shared PC policy. Dedicated workstations operating in standard desktop mode should not have a kiosk AUMID configured. An accidentally inherited kiosk AUMID can cause unexpected single-application lockout behavior if Shared PC mode is re-enabled. Clearing this value ensures devices transitioned from kiosk to standard configuration do not retain kiosk application assignments. Standard multi-application desktop behavior is preserved when no AUMID is configured.",
                Tags = ["shared-pc", "kiosk", "applications", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "KioskModeAUMID", "")],
                RemoveOps = [RegOp.DeleteValue(Key, "KioskModeAUMID")],
                DetectOps = [RegOp.CheckString(Key, "KioskModeAUMID", "")],
            },
            new TweakDef
            {
                Id = "shpc-require-signin-on-resume",
                Label = "Require Sign-In on Resume",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Requiring sign-in on resume from sleep or hibernation ensures that unauthorized users cannot access a shared device left unattended in a locked state. This policy enforces password prompt on wake for all users on shared devices, preventing unauthorized session hijacking. Shared public access devices are particularly vulnerable to unauthorized access between legitimate user sessions. Combined with short screen lock timeouts, this setting provides a strong access control baseline for multi-user environments. Sign-in on resume also ensures any screen content from the previous session is cleared before the new user can view the display. This security measure is aligned with CIS benchmark recommendations for shared computing environments.",
                Tags = ["shared-pc", "security", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SignInOnResume", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SignInOnResume")],
                DetectOps = [RegOp.CheckDword(Key, "SignInOnResume", 1)],
            },
        ];

    }

    // ── WindowsAutopilotPolicy ──
    private static class _WindowsAutopilotPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Autopilot";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "wpautopilot-block-oobe-cortana",
                Label = "Autopilot: Suppress Cortana Voice Assistant During OOBE Provisioning",
                Category = "System",
                Description = "Sets DisableCortanaInOOBE=1 in Autopilot policy. Prevents Cortana's voice-guided OOBE assistant from launching during the Windows Out-Of-Box Experience on Autopilot-provisioned devices, eliminating unexpected voice output and microphone access during unattended provisioning. " +
                    "During self-deploying Autopilot provisioning, the device may go through OOBE phases unattended. Cortana's voice interface launching during an unattended provisioning session can trigger unexpected audio output (speakers active) and request microphone access, which is unnecessary and potentially alarming in secure staging environments. Suppressing Cortana during OOBE ensures silent, predictable provisioning.",
                Tags = ["autopilot", "oobe", "cortana", "provisioning", "silent"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Cortana suppressed during OOBE; Autopilot provisioning completes silently without voice prompts.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCortanaInOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCortanaInOOBE")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCortanaInOOBE", 1)],
            },
            new TweakDef
            {
                Id = "wpautopilot-require-tpm-attestation",
                Label = "Autopilot: Require TPM Attestation Before Autopilot Pre-Provisioning Completes",
                Category = "System",
                Description = "Sets RequireTPMAttestation=1 in Autopilot policy. Requires that the device's TPM chip successfully completes attestation with the Microsoft Attestation Service before Autopilot White Glove pre-provisioning is allowed to complete, ensuring only machines with healthy TPM chips receive the provisioning credential blob. " +
                    "Autopilot White Glove pre-provisioning downloads and installs applications and policies during the Technician Phase. If TPM attestation is not required, a device with a non-functional or tampered TPM can still be fully provisioned and shipped to an end user with an enterprise credential blob. Requiring TPM attestation ensures only hardware with a verified, healthy TPM is provisioned, supporting BitLocker and Windows Hello for Business.",
                Tags = ["autopilot", "tpm", "attestation", "white-glove", "hardware-security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "TPM attestation required; Autopilot White Glove fails for devices with non-functional or tampered TPM.",
                ApplyOps = [RegOp.SetDword(Key, "RequireTPMAttestation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireTPMAttestation")],
                DetectOps = [RegOp.CheckDword(Key, "RequireTPMAttestation", 1)],
            },
            new TweakDef
            {
                Id = "wpautopilot-block-language-selection-in-oobe",
                Label = "Autopilot: Skip Language and Region Selection in OOBE (Silent Provisioning)",
                Category = "System",
                Description = "Sets SkipLanguageAndRegion=1 in Autopilot policy. Skips the language selection, keyboard layout, and region selection screens during OOBE, using the locale settings pre-configured in the Autopilot deployment profile instead of prompting the user or technician during provisioning. " +
                    "Self-deploying Autopilot profiles target unattended provisioning. Any OOBE screen that blocks at a user input prompt (language, region) halts the provisioning workflow until answered. In staging environments where devices are provisioned in bulk on racks, unexpected OOBE prompts that require per-device interaction break the automation, requiring manual intervention on each device.",
                Tags = ["autopilot", "oobe", "language", "silent", "unattended"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Language/region OOBE screens skipped; Autopilot provisioning uses profile locale settings without prompt.",
                ApplyOps = [RegOp.SetDword(Key, "SkipLanguageAndRegion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SkipLanguageAndRegion")],
                DetectOps = [RegOp.CheckDword(Key, "SkipLanguageAndRegion", 1)],
            },
            new TweakDef
            {
                Id = "wpautopilot-disable-privacy-settings-screen",
                Label = "Autopilot: Skip Privacy Settings Screen in OOBE",
                Category = "System",
                Description = "Sets DisablePrivacySettingsInOOBE=1 in Autopilot policy. Suppresses the privacy settings configuration screen that appears during OOBE, where Windows presents toggles for diagnostic data, location, speech recognition, and ink/typing personalisation, using enterprise policy defaults instead. " +
                    "The OOBE privacy settings screen presents users and technicians with a series of toggle choices that may override enterprise Group Policy settings if the user makes incorrect selections during provisioning. By skipping this screen and applying privacy settings via Group Policy or Intune configuration profiles, the enterprise ensures the device always meets its defined privacy configuration baseline from first boot.",
                Tags = ["autopilot", "oobe", "privacy", "provisioning", "baseline"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "OOBE privacy settings screen skipped; enterprise policy controls privacy toggles rather than OOBE user selection.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePrivacySettingsInOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivacySettingsInOOBE")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrivacySettingsInOOBE", 1)],
            },
            new TweakDef
            {
                Id = "wpautopilot-enable-secure-diagnostics-upload",
                Label = "Autopilot: Enable Secure Diagnostic Log Upload on Provisioning Failure",
                Category = "System",
                Description = "Sets EnableDiagnosticsUploadOnFailure=1 in Autopilot policy. Enables automatic upload of diagnostic logs to the Microsoft Intune service when Autopilot provisioning fails, allowing IT admins to review failure details in the Intune admin center without physical access to the device. " +
                    "Autopilot provisioning failures in the field (enrolled device failing to complete provisioning at an employee's desk) are difficult to diagnose without the detailed log files stored on the device. Without automatic log upload, IT must either collect logs manually (requiring physical access or remote PowerShell) or rely on the user to capture and submit logs. Enabling automatic upload on failure provides actionable failure diagnostics in the admin portal.",
                Tags = ["autopilot", "diagnostics", "failure", "logging", "intune"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Autopilot failure logs uploaded automatically to Intune; no physical access needed for provisioning failure diagnostics.",
                ApplyOps = [RegOp.SetDword(Key, "EnableDiagnosticsUploadOnFailure", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDiagnosticsUploadOnFailure")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDiagnosticsUploadOnFailure", 1)],
            },
            new TweakDef
            {
                Id = "wpautopilot-block-manual-hardware-hash-upload",
                Label = "Autopilot: Block Manual Hardware Hash Upload by Non-Administrators",
                Category = "System",
                Description = "Sets DisableManualHardwareHashUpload=1 in Autopilot policy. Prevents standard users from manually running scripts or PowerShell commands that collect the device's hardware hash and upload it to the Autopilot service, restricting hardware hash registration to OEM upload and IT admin-initiated processes. " +
                    "Hardware hash registration is the authoritative step that associates a physical device with an Autopilot deployment profile. If standard users can run scripts to upload hardware hashes of arbitrary devices (including virtual machines running on personal hardware), they may register personal devices into the enterprise Autopilot service, bootstrapping them with enterprise policies, certificates, and credentials.",
                Tags = ["autopilot", "hardware-hash", "registration", "unauthorised", "admin"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Manual hardware hash upload blocked for standard users; only OEM/IT admin can register devices.",
                ApplyOps = [RegOp.SetDword(Key, "DisableManualHardwareHashUpload", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableManualHardwareHashUpload")],
                DetectOps = [RegOp.CheckDword(Key, "DisableManualHardwareHashUpload", 1)],
            },
            new TweakDef
            {
                Id = "wpautopilot-enable-provisioning-audit-log",
                Label = "Autopilot: Enable Security Audit Log for Autopilot Provisioning Events",
                Category = "System",
                Description = "Sets EnableProvisioningAuditLog=1 in Autopilot policy. Causes a Security event log entry to be written at each stage of the Autopilot provisioning workflow (device registration, Entra ID join, MDM enrollment, application installation) including the result and any error codes. " +
                    "Without provisioning audit logging, there is no on-device Security event record of what happened during Autopilot provisioning — only the results visible in the Intune admin portal. Having on-device event log entries for each provisioning stage enables post-incident forensics if a device's provisioning state is questioned (e.g., whether a specific application or configuration was applied correctly during the initial setup).",
                Tags = ["autopilot", "audit", "provisioning", "event-log", "forensics"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Security event log entries written at each Autopilot provisioning stage; on-device provisioning history available.",
                ApplyOps = [RegOp.SetDword(Key, "EnableProvisioningAuditLog", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableProvisioningAuditLog")],
                DetectOps = [RegOp.CheckDword(Key, "EnableProvisioningAuditLog", 1)],
            },
            new TweakDef
            {
                Id = "wpautopilot-require-enrolled-device-for-provisioning",
                Label = "Autopilot: Require Device Pre-Registration Before OOBE Autopilot Profile Download",
                Category = "System",
                Description = "Sets RequirePreRegistration=1 in Autopilot policy. Enforces that the device must be pre-registered in the Autopilot service (via hardware hash) before the OOBE Autopilot profile download proceeds, blocking provisioning of devices that have not been explicitly registered by IT. " +
                    "Without pre-registration enforcement, an unregistered device going through OOBE on the same network as a registered device might accidentally receive an Autopilot profile due to subnet-based profile assignment misconfiguration. Requiring explicit pre-registration ensures that Autopilot profiles are only applied to known, IT-registered hardware and not to devices that are accidentally discoverable.",
                Tags = ["autopilot", "pre-registration", "oobe", "hardware", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Autopilot provisioning blocked for non-registered hardware; only pre-enrolled devices receive provisioning profiles.",
                ApplyOps = [RegOp.SetDword(Key, "RequirePreRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequirePreRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "RequirePreRegistration", 1)],
            },
            new TweakDef
            {
                Id = "wpautopilot-block-oobe-skip-button",
                Label = "Autopilot: Remove OOBE Skip/Cancel Button to Prevent Provisioning Abandonment",
                Category = "System",
                Description = "Sets DisableSkipButtonInOOBE=1 in Autopilot policy. Removes the 'Skip' and 'Cancel' buttons from Autopilot OOBE screens that would allow a user or technician to abort the provisioning workflow before it completes, ensuring devices are always fully provisioned before being usable. " +
                    "OOBE Skip buttons allow a technician or user to abandon Autopilot provisioning mid-way through, leaving the device in a partially configured state with some apps installed and others not, MDM enrollment incomplete, and security baselines potentially unapplied. A partially provisioned device may appear to work normally while critical security configurations are absent.",
                Tags = ["autopilot", "oobe", "skip", "provisioning", "incomplete"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "OOBE skip/cancel buttons removed; Autopilot provisioning must complete before device becomes usable.",
                ApplyOps = [RegOp.SetDword(Key, "DisableSkipButtonInOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSkipButtonInOOBE")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSkipButtonInOOBE", 1)],
            },
            new TweakDef
            {
                Id = "wpautopilot-set-provisioning-timeout-90min",
                Label = "Autopilot: Set Autopilot Enrollment Status Page Timeout to 90 Minutes",
                Category = "System",
                Description = "Sets EnrollmentStatusPageTimeout=90 in Autopilot policy. Sets the Autopilot Enrollment Status Page (ESP) timeout — the maximum time the ESP will wait for app and policy installation to complete before triggering an error — to 90 minutes. " +
                    "The default ESP timeout is 60 minutes. In enterprise environments with large required application sets or slow network segments (branch office with limited bandwidth), the app installation phase can exceed 60 minutes especially for large apps delivered via Intune Win32 app deployment (LOB apps with 500 MB+ installers). An ESP timeout before provisioning completes leaves the device in an error state, triggering a factory reset. A 90-minute timeout accommodates larger app sets.",
                Tags = ["autopilot", "esp", "timeout", "provisioning", "apps"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "ESP timeout extended to 90 minutes; large application packages have more time to complete installation during provisioning.",
                ApplyOps = [RegOp.SetDword(Key, "EnrollmentStatusPageTimeout", 90)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnrollmentStatusPageTimeout")],
                DetectOps = [RegOp.CheckDword(Key, "EnrollmentStatusPageTimeout", 90)],
            },
        ];

    }

    // ── WindowsDeploymentServicesPolicy ──
    private static class _WindowsDeploymentServicesPolicy
    {
        private const string SvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS\Server";
        private const string PxeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS\PXE";
        private const string TransKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS\Transport";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id           = "wds-require-admin-approval",
                Label        = "Require Admin Approval for PXE Boot Clients",
                Category = "System",
                Description  = "Requires administrator approval before unknown PXE clients can boot from WDS. Prevents unauthorised devices from imaging. Default: auto-approve.",
                Tags         = ["wds", "pxe", "security", "approval", "deployment", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 4,
                SafetyRating = 4,
                ImpactNote   = "Unknown PXE clients are held pending admin approval; known-device imaging unaffected.",
                ApplyOps     = [RegOp.SetDword(PxeKey, "RequireAdminApproval", 1)],
                RemoveOps    = [RegOp.DeleteValue(PxeKey, "RequireAdminApproval")],
                DetectOps    = [RegOp.CheckDword(PxeKey, "RequireAdminApproval", 1)],
            },
            new TweakDef
            {
                Id           = "wds-disable-unknown-pxe",
                Label        = "Block Unknown Clients from PXE Boot",
                Category = "System",
                Description  = "Prevents unknown (non-pre-staged) computers from performing PXE boot via WDS. Only pre-staged/known devices can image. Default: allow all.",
                Tags         = ["wds", "pxe", "security", "unknown-clients", "deployment", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 5,
                SafetyRating = 3,
                ImpactNote   = "Only pre-staged devices can PXE boot; new devices must be pre-staged in AD first.",
                ApplyOps     = [RegOp.SetDword(PxeKey, "AllowUnknownClients", 0)],
                RemoveOps    = [RegOp.DeleteValue(PxeKey, "AllowUnknownClients")],
                DetectOps    = [RegOp.CheckDword(PxeKey, "AllowUnknownClients", 0)],
            },
            new TweakDef
            {
                Id           = "wds-enable-pxe-prompt",
                Label        = "Enable PXE Boot Key Press Prompt",
                Category = "System",
                Description  = "Requires the user to press a key (e.g., F12) to initiate PXE boot. Prevents automatic network boot on every startup. Default: may auto-boot.",
                Tags         = ["wds", "pxe", "prompt", "boot", "deployment", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 3,
                SafetyRating = 5,
                ImpactNote   = "Users must press a key to PXE boot; prevents accidental reimaging.",
                ApplyOps     = [RegOp.SetDword(PxeKey, "PxePromptPolicy", 1)],
                RemoveOps    = [RegOp.DeleteValue(PxeKey, "PxePromptPolicy")],
                DetectOps    = [RegOp.CheckDword(PxeKey, "PxePromptPolicy", 1)],
            },
            new TweakDef
            {
                Id           = "wds-set-pxe-timeout",
                Label        = "Set PXE Prompt Timeout to 10 Seconds",
                Category = "System",
                Description  = "Sets the PXE boot key-press prompt timeout to 10 seconds. After timeout, the device continues to local disk boot. Default: varies by BIOS.",
                Tags         = ["wds", "pxe", "timeout", "boot", "deployment", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 2,
                SafetyRating = 5,
                ImpactNote   = "10-second window for PXE boot; device falls through to local boot on timeout.",
                ApplyOps     = [RegOp.SetDword(PxeKey, "PxePromptTimeout", 10)],
                RemoveOps    = [RegOp.DeleteValue(PxeKey, "PxePromptTimeout")],
                DetectOps    = [RegOp.CheckDword(PxeKey, "PxePromptTimeout", 10)],
            },
            new TweakDef
            {
                Id           = "wds-enable-logging",
                Label        = "Enable WDS Deployment Event Logging",
                Category = "System",
                Description  = "Enables detailed event logging for WDS deployment operations. Provides audit trail of which devices were imaged and when. Default: minimal logging.",
                Tags         = ["wds", "logging", "audit", "deployment", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 3,
                SafetyRating = 5,
                ImpactNote   = "Detailed WDS deployment events written to event log; slight disk overhead.",
                ApplyOps     = [RegOp.SetDword(SvcKey, "EnableLogging", 1)],
                RemoveOps    = [RegOp.DeleteValue(SvcKey, "EnableLogging")],
                DetectOps    = [RegOp.CheckDword(SvcKey, "EnableLogging", 1)],
            },
            new TweakDef
            {
                Id           = "wds-set-multicast-transfer-mode",
                Label        = "Set WDS Multicast Transfer to Auto Mode",
                Category = "System",
                Description  = "Configures multicast image transfers to auto-select between multicast and unicast based on network conditions. Default: multicast only.",
                Tags         = ["wds", "multicast", "network", "deployment", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 3,
                SafetyRating = 4,
                ImpactNote   = "WDS auto-selects best transfer mode; improves reliability on networks without multicast support.",
                ApplyOps     = [RegOp.SetDword(TransKey, "TransferMode", 1)],
                RemoveOps    = [RegOp.DeleteValue(TransKey, "TransferMode")],
                DetectOps    = [RegOp.CheckDword(TransKey, "TransferMode", 1)],
            },
            new TweakDef
            {
                Id           = "wds-set-multicast-session-threshold",
                Label        = "Set Multicast Session Client Threshold to 10",
                Category = "System",
                Description  = "Sets the minimum number of clients before a multicast session starts. Prevents starting a multicast session for only 1–2 clients. Default: 1.",
                Tags         = ["wds", "multicast", "threshold", "deployment", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 2,
                SafetyRating = 5,
                ImpactNote   = "Multicast waits for 10 clients before starting; single clients use unicast fallback.",
                ApplyOps     = [RegOp.SetDword(TransKey, "MulticastSessionThreshold", 10)],
                RemoveOps    = [RegOp.DeleteValue(TransKey, "MulticastSessionThreshold")],
                DetectOps    = [RegOp.CheckDword(TransKey, "MulticastSessionThreshold", 10)],
            },
            new TweakDef
            {
                Id           = "wds-enable-tftp-window-size",
                Label        = "Set WDS TFTP Block Size to 16384",
                Category = "System",
                Description  = "Increases the TFTP block size used by WDS PXE boot to 16384 bytes. Improves image download speed on modern networks. Default: 1456 (standard TFTP block).",
                Tags         = ["wds", "tftp", "performance", "pxe", "deployment", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 3,
                SafetyRating = 4,
                ImpactNote   = "Faster PXE image downloads; may fail on networks with low MTU or NAT.",
                ApplyOps     = [RegOp.SetDword(TransKey, "TftpBlockSize", 16384)],
                RemoveOps    = [RegOp.DeleteValue(TransKey, "TftpBlockSize")],
                DetectOps    = [RegOp.CheckDword(TransKey, "TftpBlockSize", 16384)],
            },
            new TweakDef
            {
                Id           = "wds-disable-dhcp-option-60",
                Label        = "Disable DHCP Option 60 (PXEClient Class ID)",
                Category = "System",
                Description  = "Prevents WDS from adding DHCP Option 60 (PXEClient class identifier) to DHCP responses. Use when WDS is co-located with DHCP to avoid conflicts. Default: enabled.",
                Tags         = ["wds", "dhcp", "pxe", "network", "deployment", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 3,
                SafetyRating = 3,
                ImpactNote   = "Prevents DHCP conflict when WDS and DHCP share the same server; PXE may need DHCP Option 66/67 instead.",
                ApplyOps     = [RegOp.SetDword(PxeKey, "UseDhcpPorts", 0)],
                RemoveOps    = [RegOp.DeleteValue(PxeKey, "UseDhcpPorts")],
                DetectOps    = [RegOp.CheckDword(PxeKey, "UseDhcpPorts", 0)],
            },
            new TweakDef
            {
                Id           = "wds-restrict-naming-policy",
                Label        = "Enforce WDS Computer Naming Policy",
                Category = "System",
                Description  = "Enforces a server-defined computer naming policy for imaged devices. Prevents users from choosing arbitrary computer names during imaging. Default: user-chosen.",
                Tags         = ["wds", "naming", "policy", "deployment", "standardisation"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 3,
                SafetyRating = 5,
                ImpactNote   = "Imaged computers get server-defined names; ensures naming convention compliance.",
                ApplyOps     = [RegOp.SetDword(SvcKey, "NamingPolicy", 1)],
                RemoveOps    = [RegOp.DeleteValue(SvcKey, "NamingPolicy")],
                DetectOps    = [RegOp.CheckDword(SvcKey, "NamingPolicy", 1)],
            },
        ];

    }

    // ── WindowsFlightedFeaturesPolicy ──
    private static class _WindowsFlightedFeaturesPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FlightedFeatures";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "flight-disable-feature-trials",
                Label = "Windows Flighted Features: Disable Feature Trials",
                Category = "System",
                Description =
                    "Prevents Windows from enrolling this device in feature trials via the flighting (A/B testing) mechanism. "
                    + "Feature trials push experimental or partially-ready features to a subset of devices without user opt-in. "
                    + "Disabling trials ensures only fully-released, validated features are active on enterprise endpoints. "
                    + "Removing this policy re-enables Microsoft's ability to push feature trial updates.",
                Tags = ["flighting", "feature-trial", "stability", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnabledFlightedFeatures", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnabledFlightedFeatures")],
                DetectOps = [RegOp.CheckDword(Key, "EnabledFlightedFeatures", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents experimental feature roll-outs; improves endpoint stability and predictability.",
            },
            new TweakDef
            {
                Id = "flight-block-preview-builds",
                Label = "Windows Flighted Features: Block Preview Build Features",
                Category = "System",
                Description =
                    "Prevents preview-ring feature flags from being activated on production endpoints via the flighting registry policy. "
                    + "Preview builds may include unstable code paths, driver compatibility issues, or features not yet hardened for enterprise use. "
                    + "Blocking preview feature activation is a standard practice in SOE (Standard Operating Environment) management. "
                    + "Removing this policy allows Microsoft flighting to selectively enable preview features on this device.",
                Tags = ["flighting", "preview", "windows-update", "stability", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePreviewFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePreviewFeatures")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePreviewFeatures", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks preview feature activation; reduces risk of unstable code on production machines.",
            },
            new TweakDef
            {
                Id = "flight-set-branch-readiness-semi-annual",
                Label = "Windows Flighted Features: Set Branch Readiness to Semi-Annual Channel",
                Category = "System",
                Description =
                    "Configures the Windows flighting branch readiness level to the Semi-Annual Channel (production ring). "
                    + "The branch readiness level controls which update ring the device belongs to — insider, beta, or release. "
                    + "Semi-Annual Channel (value 32) ensures only fully validated updates are offered. "
                    + "Removing this policy allows Windows Update to assign the device to its default ring.",
                Tags = ["flighting", "branch", "update-ring", "semi-annual", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BranchReadinessLevel", 32)],
                RemoveOps = [RegOp.DeleteValue(Key, "BranchReadinessLevel")],
                DetectOps = [RegOp.CheckDword(Key, "BranchReadinessLevel", 32)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks device to Semi-Annual Channel; prevents insider/beta feature ring enrollment.",
            },
            new TweakDef
            {
                Id = "flight-disable-diagnostic-data-upload",
                Label = "Windows Flighted Features: Disable Diagnostic Data Upload for Flights",
                Category = "System",
                Description =
                    "Disables the upload of diagnostic data specifically associated with flighted (experimental) feature usage. "
                    + "When a feature trial is active, Windows collects enhanced telemetry to evaluate the trial's effectiveness. "
                    + "This policy stops that additional telemetry while still permitting baseline diagnostic data. "
                    + "Removing this policy re-enables flight-specific enhanced diagnostic data collection.",
                Tags = ["flighting", "telemetry", "privacy", "diagnostic-data", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFlightDiagnosticData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFlightDiagnosticData")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFlightDiagnosticData", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Reduces telemetry associated with feature trials; improves privacy on managed endpoints.",
            },
            new TweakDef
            {
                Id = "flight-disable-experimentation",
                Label = "Windows Flighted Features: Disable A/B Experimentation",
                Category = "System",
                Description =
                    "Prevents Windows from applying A/B experimentation overrides via the flighting system. "
                    + "A/B experimentation can silently change UI layouts, default settings, or feature availability without the user's knowledge. "
                    + "On managed endpoints, unpredictable behaviour changes caused by experiments can interfere with helpdesk scripts and SOE policies. "
                    + "Removing this policy re-allows A/B experiments to be applied to this device.",
                Tags = ["flighting", "experimentation", "ab-test", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableExperimentation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableExperimentation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableExperimentation", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents silent A/B experiments; ensures consistent, predictable Windows behaviour.",
            },
            new TweakDef
            {
                Id = "flight-set-target-release-version",
                Label = "Windows Flighted Features: Set Target Release Version (24H2)",
                Category = "System",
                Description =
                    "Pins the device to Windows 11 24H2 as the target feature update version via the flighting policy. "
                    + "Pinning the target release prevents automatic upgrade to newer feature releases before IT validation is complete. "
                    + "This is critical in environments with validated SOE images and application compatibility dependencies. "
                    + "Removing this policy allows Windows Update to offer the next feature release when available.",
                Tags = ["flighting", "target-release", "version-pin", "feature-update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TargetReleaseVersion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TargetReleaseVersion")],
                DetectOps = [RegOp.CheckDword(Key, "TargetReleaseVersion", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Pins device to a target release; prevents unvetted feature update upgrades.",
            },
            new TweakDef
            {
                Id = "flight-disable-insider-content",
                Label = "Windows Flighted Features: Disable Insider Tip Content",
                Category = "System",
                Description =
                    "Blocks Windows Insider tip and promotional content pushed via the flighting infrastructure. "
                    + "Insider tips are shown in Start, Tips app, and Settings to encourage enrollment in the Insider Program. "
                    + "On enterprise endpoints this content is irrelevant and can distract users from productivity. "
                    + "Removing this policy re-enables Insider tip content delivery via the flighting system.",
                Tags = ["flighting", "insider", "tips", "content", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInsiderContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInsiderContent")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInsiderContent", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses Insider Program promotional content; cleaner enterprise desktop experience.",
            },
            new TweakDef
            {
                Id = "flight-disable-rollback-on-failure",
                Label = "Windows Flighted Features: Disable Automatic Rollback on Flight Failure",
                Category = "System",
                Description =
                    "Controls whether Windows automatically rolls back a failed flight update without administrator approval. "
                    + "Automatic rollback can interfere with change-management processes in enterprise environments where all changes must be audited. "
                    + "Disabling automatic rollback requires IT to explicitly approve any reversion action. "
                    + "Removing this policy re-enables Windows automatic rollback on flight failure.",
                Tags = ["flighting", "rollback", "change-management", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRollbackOnFailure", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRollbackOnFailure")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRollbackOnFailure", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents automatic silent rollback; keeps change-management audit trail intact.",
            },
            new TweakDef
            {
                Id = "flight-disable-feature-notifications",
                Label = "Windows Flighted Features: Disable Feature Notification Banners",
                Category = "System",
                Description =
                    "Suppresses notification banners introduced as part of flight updates — new feature announcements, upgrade prompts, and welcome screens. "
                    + "Flight-related notifications interrupt workflows and are inappropriate in a managed enterprise environment. "
                    + "This policy blocks those banners from appearing regardless of which features are active. "
                    + "Removing this policy re-enables flight-driven notification banners.",
                Tags = ["flighting", "notifications", "banners", "ux", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFeatureNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFeatureNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFeatureNotifications", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Stops flight-driven notification banners; reduces user interruptions on managed desktops.",
            },
            new TweakDef
            {
                Id = "flight-enforce-production-ring",
                Label = "Windows Flighted Features: Enforce Production Ring Only",
                Category = "System",
                Description =
                    "Forces the flighting infrastructure to treat this device as production-ring only, blocking all early-access feature assignments. "
                    + "In combination with BranchReadinessLevel, this ensures the device cannot be reclassified by Microsoft's backend assignment logic. "
                    + "Enforcing production ring is mandatory for PCI-DSS and SOX environments where any change to production systems requires prior approval. "
                    + "Removing this policy allows the backend to reclassify the device into any ring.",
                Tags = ["flighting", "production-ring", "compliance", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceProductionRing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceProductionRing")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceProductionRing", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks device to production ring permanently; critical for compliance-controlled endpoints.",
            },
        ];

    }

    // ── WindowsFlightingPolicy ──
    private static class _WindowsFlightingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "flight-disable-insider-preview",
                Label = "Disable Windows Insider Preview Enrollment",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Windows Insider Preview allows users to enroll their devices to receive pre-release Windows builds that are not yet generally available. Disabling Insider Preview enrollment prevents users from opting their devices into receiving unstable pre-release Windows builds. Preview builds may contain unfixed security vulnerabilities, missing patches, or experimental changes not appropriate for production environments. Insider builds do not receive the same security testing as general availability releases creating potential exposure to undisclosed vulnerabilities. Enterprise devices should run only tested and approved Windows builds deployed through managed update processes. Preventing insider enrollment ensures that enterprise endpoints remain on tested stable builds with complete security patch coverage.",
                Tags = ["flighting", "insider", "updates", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableConfigFlighting", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableConfigFlighting")],
                DetectOps = [RegOp.CheckDword(Key, "EnableConfigFlighting", 0)],
            },
            new TweakDef
            {
                Id = "flight-disable-preview-builds",
                Label = "Block Preview Build Installation",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Beyond enrollment control, Windows Update can be blocked from offering feature preview builds to enrolled users and devices. Blocking preview build installation provides an additional layer of protection ensuring that preview builds cannot be installed even if enrollment somehow occurs. Preview builds installed on enterprise devices create unsupported configurations that may not receive all security patches. IT change management processes require that OS upgrades be tested and validated before enterprise deployment. Preview builds can change system behavior, remove features, or alter security defaults in ways not accounted for by enterprise security baselines. Blocking preview installations ensures enterprise devices remain on the approved and tested configuration.",
                Tags = ["flighting", "insider", "updates", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowBuildPreview", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowBuildPreview")],
                DetectOps = [RegOp.CheckDword(Key, "AllowBuildPreview", 0)],
            },
            new TweakDef
            {
                Id = "flight-disable-config-flighting",
                Label = "Disable Windows Configuration Flighting",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Configuration flighting extends beyond build previews to include experimental feature toggles and configuration changes delivered through Microsoft's flighting infrastructure. Disabling configuration flighting prevents Microsoft from remotely toggling experimental Windows features on enterprise endpoints without IT awareness or approval. Configuration changes delivered through flighting can alter security settings, enable or disable features, or change system behavior. Enterprise security baselines assume specific feature configurations and flighting changes can invalidate baseline assumptions. IT must maintain awareness of all configuration changes on managed endpoints to ensure security policy compliance. Disabling configuration flighting ensures that the Windows configuration remains consistent with the IT-tested and approved baseline.",
                Tags = ["flighting", "configuration", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableConfigFlightingForFlights", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableConfigFlightingForFlights")],
                DetectOps = [RegOp.CheckDword(Key, "EnableConfigFlightingForFlights", 0)],
            },
            new TweakDef
            {
                Id = "flight-disable-telemetry-for-flighting",
                Label = "Disable Flighting Telemetry Uploads",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Flighting telemetry collects usage and diagnostic data from enrolled devices to help Microsoft evaluate preview feature quality and performance. Disabling flighting telemetry prevents upload of diagnostic and usage data associated with preview feature experiments. Flighting telemetry may include details about enterprise software usage, hardware configuration, and user behavior with experimental features. Sending enterprise endpoint telemetry to external parties without approval may violate enterprise data governance policies. Even on non-enrolled devices some flighting infrastructure may attempt to collect diagnostic data. Disabling flighting telemetry ensures that no preview-associated diagnostic data is transmitted from endpoints.",
                Tags = ["flighting", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFlightingTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFlightingTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFlightingTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "flight-disable-feature-rollout",
                Label = "Disable Gradual Feature Rollout",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Gradual feature rollouts deliver new features to a percentage of endpoints before full general availability. Disabling gradual feature rollout prevents selected endpoints from receiving new features ahead of the general release schedule. Endpoints receiving features early may have different behavior from other endpoints complicating support and security assessment. Early feature deployments may not have received complete security review and may expose new attack surfaces before hardening guidance is available. Enterprise environments benefit from predictable feature delivery through managed update processes rather than random selection for early rollout. Disabling gradual rollouts ensures consistent behavior across all enterprise endpoints at all times.",
                Tags = ["flighting", "features", "updates", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGradualRollout", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGradualRollout")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGradualRollout", 1)],
            },
            new TweakDef
            {
                Id = "flight-disable-experimental-features",
                Label = "Disable Experimental Feature Flags",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Experimental feature flags are toggles that can enable incomplete or tentative features that are not yet ready for general release. Disabling experimental feature flags prevents activation of features that may have security vulnerabilities, instabilities, or incomplete implementations. Experimental features may have bypassed the complete security review process that production features undergo before general availability. Enabling experimental flags can expose endpoints to attack vectors not present in released features without corresponding security guidance. Enterprise endpoints should only run features that have completed the full development, testing, and security review lifecycle. Experimental features can be evaluated in isolated sandbox environments by development and security teams without risk to production endpoints.",
                Tags = ["flighting", "experimental", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableExperimentalFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableExperimentalFeatures")],
                DetectOps = [RegOp.CheckDword(Key, "DisableExperimentalFeatures", 1)],
            },
            new TweakDef
            {
                Id = "flight-disable-a-b-testing",
                Label = "Disable A/B Feature Testing Participation",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Microsoft uses A/B testing to evaluate different user interface designs and feature implementations on randomly selected endpoints. Disabling A/B testing participation prevents endpoints from being selected to receive alternative interface designs or feature variants. A/B test subjects may receive features with different defaults or behaviors that deviate from the enterprise-approved baseline configuration. Security assessments and user training are developed for consistent interface implementations and A/B variants complicate these processes. Product feature changes affecting enterprise workflows should be introduced through IT-managed deployment cycles not random selection. Opting out of A/B testing ensures all enterprise endpoints receive the same consistent default Windows experience.",
                Tags = ["flighting", "ab-testing", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableABTesting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableABTesting")],
                DetectOps = [RegOp.CheckDword(Key, "DisableABTesting", 1)],
            },
            new TweakDef
            {
                Id = "flight-set-insider-ring",
                Label = "Set Windows Insider Ring to None",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Windows Insider has multiple rings from Dev Channel receiving the most experimental builds to Release Preview receiving near-release builds. Setting the insider ring to None ensures the endpoint is not associated with any insider channel and receives only generally available updates. Device assignment to any insider ring makes the endpoint eligible for pre-release builds regardless of other enrollment settings. Enterprise endpoints should not be affiliated with any insider ring to ensure they only receive production-quality builds. Insider ring assignments should be cleared to confirm no residual enrollment state persists from previous configurations. Setting the ring to None combined with disabling enrollment provides defense-in-depth against accidental preview build delivery.",
                Tags = ["flighting", "insider", "updates", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BranchReadinessLevel", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "BranchReadinessLevel")],
                DetectOps = [RegOp.CheckDword(Key, "BranchReadinessLevel", 0)],
            },
            new TweakDef
            {
                Id = "flight-disable-insider-program-settings",
                Label = "Disable Insider Program Settings Access",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The Insider Program section in Windows Settings provides users with the interface to enroll in or change insider program membership. Disabling access to Insider Program settings removes the user-accessible configuration page that controls insider enrollment. Hiding the settings page prevents accidental or deliberate enrollment by users who are unaware of enterprise policy against insider participation. Settings access removal is a complementary control to the enrollment block policy providing defense-in-depth. Users attempting to enroll through the settings page will receive a policy-blocked message rather than the enrollment interface. Administrative access to insider settings remains available for authorized IT change processes.",
                Tags = ["flighting", "insider", "ui", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInsiderProgramSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInsiderProgramSettings")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInsiderProgramSettings", 1)],
            },
            new TweakDef
            {
                Id = "flight-disable-optional-feature-updates",
                Label = "Disable Optional Preview Feature Updates",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows Update includes optional feature updates that users can choose to install before they become mandatory on a future update schedule. Disabling optional feature updates prevents users from installing new Windows features that have not been tested and approved by IT. Optional feature updates may include security-relevant changes that alter system behavior without IT awareness. Features received through optional updates may not be covered by enterprise security baselines creating undefined risk. Enterprise feature deployment should proceed through IT-managed testing and approval processes with appropriate scheduling. Preventing optional update installation ensures IT maintains control over the timing and content of feature changes on managed endpoints.",
                Tags = ["flighting", "updates", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOptionalFeatureUpdates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOptionalFeatureUpdates")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOptionalFeatureUpdates", 1)],
            },
        ];

    }

    // ── WindowsInsider ──
    private static class _WindowsInsider
    {
        private const string PreviewBuildsPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds";

        private const string DataCollection = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

        private const string CloudContent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

        private const string SelfHostApplicability = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsSelfHost\Applicability";

        private const string FeedbackRules = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Siuf\Rules";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "insider-block-preview-builds",
                Label = "Block Windows Insider Preview Build Enrollment (GPO)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["insider", "preview", "flighting", "windows update", "policy"],
                Description =
                    "Prevents users from enrolling this device in the Windows Insider Program "
                    + "via Group Policy. AllowBuildPreview=0. The 'Windows Insider Program' page "
                    + "in Settings is greyed out and the device receives only stable Windows builds.",
                ApplyOps = [RegOp.SetDword(PreviewBuildsPolicy, "AllowBuildPreview", 0)],
                RemoveOps = [RegOp.DeleteValue(PreviewBuildsPolicy, "AllowBuildPreview")],
                DetectOps = [RegOp.CheckDword(PreviewBuildsPolicy, "AllowBuildPreview", 0)],
            },
            new TweakDef
            {
                Id = "insider-disable-config-flighting",
                Label = "Disable Configuration Flighting (A/B Feature Tests)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["insider", "flighting", "a/b testing", "telemetry", "privacy"],
                Description =
                    "Disables Windows configuration flighting — the mechanism Microsoft uses to "
                    + "remotely enable or disable features on specific devices as part of A/B tests. "
                    + "EnableConfigFlighting=0 prevents undocumented feature changes pushed by Microsoft.",
                ApplyOps = [RegOp.SetDword(PreviewBuildsPolicy, "EnableConfigFlighting", 0)],
                RemoveOps = [RegOp.DeleteValue(PreviewBuildsPolicy, "EnableConfigFlighting")],
                DetectOps = [RegOp.CheckDword(PreviewBuildsPolicy, "EnableConfigFlighting", 0)],
            },
            new TweakDef
            {
                Id = "insider-disable-experimentation",
                Label = "Disable Windows Experimentation (A/B Feature Trials)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["insider", "experimentation", "a/b testing", "telemetry"],
                Description =
                    "Prevents Windows from participating in Microsoft's experimentation framework "
                    + "used to validate new features before official release. "
                    + "EnableExperimentation=0 ensures a fully stable, non-experimental Windows build.",
                ApplyOps = [RegOp.SetDword(PreviewBuildsPolicy, "EnableExperimentation", 0)],
                RemoveOps = [RegOp.DeleteValue(PreviewBuildsPolicy, "EnableExperimentation")],
                DetectOps = [RegOp.CheckDword(PreviewBuildsPolicy, "EnableExperimentation", 0)],
            },
            new TweakDef
            {
                Id = "insider-disable-feedback-notifications",
                Label = "Disable Windows Feedback Notification Popups",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["insider", "feedback", "notification", "privacy"],
                Description =
                    "Suppresses the periodic 'How is Windows working for you?' feedback popups. "
                    + "DoNotShowFeedbackNotifications=1. These prompts collect usage data and "
                    + "interrupt the user — disabling them keeps the UI clean.",
                ApplyOps = [RegOp.SetDword(DataCollection, "DoNotShowFeedbackNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(DataCollection, "DoNotShowFeedbackNotifications")],
                DetectOps = [RegOp.CheckDword(DataCollection, "DoNotShowFeedbackNotifications", 1)],
            },
            new TweakDef
            {
                Id = "insider-set-retail-ring",
                Label = "Set Device to Retail (Non-Insider) Ring",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["insider", "ring", "retail", "preview builds"],
                Description =
                    "Sets the Windows Update ring to Retail via the WindowsSelfHost applicability "
                    + "registry. EnablePreviewBuilds=0 ensures this device uses stable production "
                    + "builds only, exiting any Insider ring it may have been enrolled in.",
                ApplyOps = [RegOp.SetDword(SelfHostApplicability, "EnablePreviewBuilds", 0)],
                RemoveOps = [RegOp.DeleteValue(SelfHostApplicability, "EnablePreviewBuilds")],
                DetectOps = [RegOp.CheckDword(SelfHostApplicability, "EnablePreviewBuilds", 0)],
            },
            new TweakDef
            {
                Id = "insider-disable-feedback-frequency",
                Label = "Stop Windows Feedback Frequency Prompts",
                Category = "System",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["insider", "feedback", "siuf", "privacy"],
                Description =
                    "Sets the Windows SIUF (System-Initiated User Feedback) period count to 0, "
                    + "silencing all Windows 'Rate your experience' prompts. "
                    + "NumberOfSIUFInPeriod=0 in the user feedback rules key.",
                ApplyOps = [RegOp.SetDword(FeedbackRules, "NumberOfSIUFInPeriod", 0)],
                RemoveOps = [RegOp.DeleteValue(FeedbackRules, "NumberOfSIUFInPeriod")],
                DetectOps = [RegOp.CheckDword(FeedbackRules, "NumberOfSIUFInPeriod", 0)],
            },
            new TweakDef
            {
                Id = "insider-disable-consumer-features",
                Label = "Disable Windows Consumer (Non-Enterprise) Features",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["insider", "consumer features", "cloud content", "privacy"],
                Description =
                    "Disables Windows consumer-only experiences: auto pre-install of promoted apps, "
                    + "Spotlight suggestions on the Start menu, and per-user app recommendations. "
                    + "DisableWindowsConsumerFeatures=1 in Cloud Content policy.",
                ApplyOps = [RegOp.SetDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableWindowsConsumerFeatures")],
                DetectOps = [RegOp.CheckDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
            },
            new TweakDef
            {
                Id = "insider-disable-soft-landing",
                Label = "Disable Soft Landing Tips (New Feature Suggestions)",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["insider", "soft landing", "cloud content", "tips", "privacy"],
                Description =
                    "Blocks 'soft landing' — the mechanism Windows uses to show first-run tips, "
                    + "feature highlight cards, and What's New overlays after major updates. "
                    + "DisableSoftLanding=1. Keeps post-update UI identical to pre-update.",
                ApplyOps = [RegOp.SetDword(CloudContent, "DisableSoftLanding", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableSoftLanding")],
                DetectOps = [RegOp.CheckDword(CloudContent, "DisableSoftLanding", 1)],
            },
            new TweakDef
            {
                Id = "insider-disable-cloud-optimized-content",
                Label = "Disable Cloud-Optimized Content Delivery",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["insider", "cloud content", "content delivery", "privacy"],
                Description =
                    "Disables cloud-optimized content that Microsoft delivers to the lock screen, "
                    + "Start menu, and desktop (e.g., personalized ads, app promotions). "
                    + "DisableCloudOptimizedContent=1 in Cloud Content policy.",
                ApplyOps = [RegOp.SetDword(CloudContent, "DisableCloudOptimizedContent", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableCloudOptimizedContent")],
                DetectOps = [RegOp.CheckDword(CloudContent, "DisableCloudOptimizedContent", 1)],
            },
            new TweakDef
            {
                Id = "insider-disable-cloud-content-experience",
                Label = "Disable Cloud Content for Windows Suggestions",
                Category = "System",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["insider", "cloud content", "suggestions", "windows tips", "privacy"],
                Description =
                    "Disables cloud-delivered content shown in the Windows welcome experience, "
                    + "Settings highlights, and the first-run screen after major updates. "
                    + "DisableTailoredExperiencesWithDiagnosticData=1 prevents Clippy-style "
                    + "personalized suggestions based on diagnostics.",
                ApplyOps = [RegOp.SetDword(DataCollection, "DisableTailoredExperiencesWithDiagnosticData", 1)],
                RemoveOps = [RegOp.DeleteValue(DataCollection, "DisableTailoredExperiencesWithDiagnosticData")],
                DetectOps = [RegOp.CheckDword(DataCollection, "DisableTailoredExperiencesWithDiagnosticData", 1)],
            },
        ];

    }

    // ── WindowsServicingPolicy ──
    private static class _WindowsServicingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "winsvc-set-target-ga-release-channel",
                Label = "Servicing: Set Windows Update for Business Channel to GA Release Channel",
                Category = "System",
                Description = "Sets TargetReleaseVersionInfo=\"GA\" in WindowsUpdate policy. Configures Windows Update for Business to target the General Availability (GA) channel, ensuring the endpoint only receives fully released Windows 11/10 builds rather than Beta channel, Release Preview builds, or Insider Preview builds, providing the most stable update experience. " +
                    "Without an explicit channel configuration, a Windows endpoint may be enrolled in a Windows Insider Program channel from a previous administrator action and continue receiving pre-release builds. Pre-release builds are not covered by the standard Microsoft support lifecycle and may contain known stability regressions. Locking the endpoint to the GA channel ensures only fully supported, production-validated Windows builds are ever installed.",
                Tags = ["windows-servicing", "release-channel", "ga", "insider", "update"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Windows Update locked to GA channel; pre-release Insider and beta builds cannot be installed.",
                ApplyOps = [RegOp.SetString(Key, "TargetReleaseVersionInfo", "GA")],
                RemoveOps = [RegOp.DeleteValue(Key, "TargetReleaseVersionInfo")],
                DetectOps = [RegOp.CheckString(Key, "TargetReleaseVersionInfo", "GA")],
            },
            new TweakDef
            {
                Id = "winsvc-defer-feature-updates-90-days",
                Label = "Servicing: Defer Windows Feature Updates for 90 Days from GA Release",
                Category = "System",
                Description = "Sets DeferFeatureUpdatesPeriodInDays=90 in WindowsUpdate policy. Delays the installation of Windows Feature Updates (major annual or semi-annual releases introducing new OS capabilities) by 90 days from the date they are first made publicly available, giving Microsoft time to issue compatibility fixes and giving IT time to complete validation and application compatibility testing. " +
                    "New Windows Feature Updates (e.g., Windows 11 version upgrades) introduce significant changes to the OS, including driver model changes, security changes, and UI modifications. Enterprises that immediately deploy new feature updates (0-day) routinely encounter application compatibility regressions, driver failures for specialised hardware, and Group Policy setting changes that require updated ADMX templates. A 90-day deferral provides buffer for Microsoft to release hotfixes and for enterprise IT to complete testing.",
                Tags = ["windows-servicing", "feature-update", "deferral", "compatibility", "testing"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Feature updates deferred 90 days; Microsoft and IT have time to address compatibility issues before enterprise deployment.",
                ApplyOps = [RegOp.SetDword(Key, "DeferFeatureUpdatesPeriodInDays", 90)],
                RemoveOps = [RegOp.DeleteValue(Key, "DeferFeatureUpdatesPeriodInDays")],
                DetectOps = [RegOp.CheckDword(Key, "DeferFeatureUpdatesPeriodInDays", 90)],
            },
            new TweakDef
            {
                Id = "winsvc-defer-quality-updates-7-days",
                Label = "Servicing: Defer Windows Quality Updates for 7 Days to Allow Reliability Monitoring",
                Category = "System",
                Description = "Sets DeferQualityUpdatesPeriodInDays=7 in WindowsUpdate policy. Delays the installation of Windows Quality Updates (monthly Patch Tuesday cumulative updates containing security fixes, reliability improvements, and bug fixes) by 7 days from their initial release to allow time for early-adopter reports to surface critical issues before enterprise-wide deployment. " +
                    "Monthly Patch Tuesday cumulative updates occasionally introduce regressions — caused by a security fix that changes underlying API behaviour or a reliability fix interacting unexpectedly with specific application configurations. In prior years, Patch Tuesday updates have introduced BSoDs for specific driver configurations, performance regressions in SMB file server workloads, and print spooler failures. A 7-day deferral allows Microsoft, the community, and independent testing labs to publish regression reports before the update reaches production endpoints.",
                Tags = ["windows-servicing", "quality-update", "patch-tuesday", "deferral", "regression"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Quality updates deferred 7 days; regression reports communicated before production deployment.",
                ApplyOps = [RegOp.SetDword(Key, "DeferQualityUpdatesPeriodInDays", 7)],
                RemoveOps = [RegOp.DeleteValue(Key, "DeferQualityUpdatesPeriodInDays")],
                DetectOps = [RegOp.CheckDword(Key, "DeferQualityUpdatesPeriodInDays", 7)],
            },
            new TweakDef
            {
                Id = "winsvc-disable-dual-scan",
                Label = "Servicing: Disable WUfB Dual-Scan (WSUS + Windows Update Cloud Simultaneously)",
                Category = "System",
                Description = "Sets DisableDualScan=1 in WindowsUpdate policy. Prevents Windows Update for Business from simultaneously scanning both the corporate WSUS server and the Windows Update cloud service for updates, restricting update source to the configured primary source only (typically WSUS). Without this setting, endpoints configured with both WSUS and WUfB policies may accidentally install cloud-sourced updates that haven't been approved in WSUS. " +
                    "WSUS environments use update approval workflows to prevent unapproved patches from installing. Windows Update for Business cloud scanning bypasses WSUS approval workflows — an update that is DECLINED in WSUS may still install if the endpoint simultaneously scans and finds the update approved in the Windows Update cloud service. Dual scan effectively breaks WSUS update governance by allowing cloud updates to supersede WSUS-declined updates.",
                Tags = ["windows-servicing", "dual-scan", "wsus", "wufb", "update-governance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Dual-scan disabled; updates only sourced from configured primary (WSUS/WUfB); cloud updates cannot bypass WSUS approval.",
                ApplyOps = [RegOp.SetDword(Key, "DisableDualScan", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDualScan")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDualScan", 1)],
            },
            new TweakDef
            {
                Id = "winsvc-block-preview-builds",
                Label = "Servicing: Block Windows Preview Builds and Insider Preview Enrollment",
                Category = "System",
                Description = "Sets ManagePreviewBuilds=1 in WindowsUpdate policy. Prevents Windows from accessing Insider Preview builds, blocks the Windows Insider Program from enrolling the device, and hides the 'Windows Insider Program' section from Settings > Windows Update, making it impossible for users or administrators to opt into Insider Preview channels that would replace the production OS with a pre-release build. " +
                    "Windows Insider Program enrolment replaces the production Windows build with a pre-release build that may have known critical vulnerabilities (disclosed during the Insider period), removed security features under development, or APIs with breaking changes from the production build. On enterprise endpoints, any path that allows downgrading from a supported production build to an unsupported pre-release build bypasses the enterprise's patching SLA and software support commitments.",
                Tags = ["windows-servicing", "insider-preview", "preview-builds", "insider-program", "lockdown"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Windows Insider Program blocked; device cannot be enrolled in preview channels or receive pre-release builds.",
                ApplyOps = [RegOp.SetDword(Key, "ManagePreviewBuilds", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ManagePreviewBuilds")],
                DetectOps = [RegOp.CheckDword(Key, "ManagePreviewBuilds", 1)],
            },
            new TweakDef
            {
                Id = "winsvc-exclude-drivers-from-quality-updates",
                Label = "Servicing: Exclude Driver Updates from Monthly Quality Update Package",
                Category = "System",
                Description = "Sets ExcludeWUDriversInQualityUpdate=1 in WindowsUpdate policy. Prevents Windows Update for Business from installing driver updates as part of the monthly cumulative quality update package, requiring that driver updates are sourced and approved separately through the driver management pipeline rather than being bundled into the OS quality update. " +
                    "Driver updates bundled into Windows quality updates have been a source of hardware compatibility regressions, particularly for specialised peripherals, storage controllers, and graphics subsystems. A mandatory driver update included in a cumulative update may replace a tested, stable OEM driver with a Microsoft-provided inbox driver that behaves differently for specific hardware configurations. Excluding drivers from quality updates allows IT to validate and approve driver updates independently on a slower cadence.",
                Tags = ["windows-servicing", "drivers", "quality-update", "regression", "driver-management"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Driver updates excluded from quality update packages; drivers validated and deployed on separate IT-controlled schedule.",
                ApplyOps = [RegOp.SetDword(Key, "ExcludeWUDriversInQualityUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ExcludeWUDriversInQualityUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "ExcludeWUDriversInQualityUpdate", 1)],
            },
            new TweakDef
            {
                Id = "winsvc-block-optional-content-updates",
                Label = "Servicing: Block Optional Windows Content Updates (Media Features, Language Packs)",
                Category = "System",
                Description = "Sets AllowOptionalContent=0 in WindowsUpdate policy. Prevents Windows Update from automatically downloading and installing optional content updates — including optional feature updates, language experience packs, optional cumulative update components, and regional supplemental content packs — without explicit IT administrator approval for each optional package. " +
                    "Optional content includes media feature packs, additional language support, and supplemental features that Microsoft offers but does not install by default. While largely benign, optional content can consume hundreds of MB of disk space per package and is not required for enterprise operation. In disk-constrained environments (VDI thin clients, 128 GB endpoint SSDs) or bandwidth-constrained environments (WAN-connected branch offices), automatic download of optional content packages creates unnecessary overhead without enterprise benefit.",
                Tags = ["windows-servicing", "optional-content", "language-packs", "disk-space", "bandwidth"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Optional Windows content updates blocked; language packs and optional features not auto-downloaded.",
                ApplyOps = [RegOp.SetDword(Key, "AllowOptionalContent", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowOptionalContent")],
                DetectOps = [RegOp.CheckDword(Key, "AllowOptionalContent", 0)],
            },
            new TweakDef
            {
                Id = "winsvc-set-readiness-level-general-availability",
                Label = "Servicing: Set Branch Readiness Level to General Availability Channel",
                Category = "System",
                Description = "Sets BranchReadinessLevel=16 in WindowsUpdate policy. Sets the Windows Update for Business readiness level (deployment ring) to General Availability Channel (value 16), directing the endpoint to receive feature updates only after they have been on the General Availability channel for the configured deferral period, rather than from the Beta or Release Preview channels. " +
                    "BranchReadinessLevel determines which update channel feeds feature update availability. A value of 2 selects the Release Preview channel; 16 selects General Availability. Enterprises that configure WUfB without explicitly setting the readiness level may receive updates from the Release Preview channel, which contains builds that are near-final but may still have issues resolved between Release Preview and GA. Explicit GA targeting closes this gap.",
                Tags = ["windows-servicing", "branch-readiness", "ga-channel", "feature-update", "wufb"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WUfB readiness level set to GA Channel (16); only fully released feature updates are eligible for deployment.",
                ApplyOps = [RegOp.SetDword(Key, "BranchReadinessLevel", 16)],
                RemoveOps = [RegOp.DeleteValue(Key, "BranchReadinessLevel")],
                DetectOps = [RegOp.CheckDword(Key, "BranchReadinessLevel", 16)],
            },
            new TweakDef
            {
                Id = "winsvc-enable-safe-os-update-rollback",
                Label = "Servicing: Enable SafeOS Update Rollback on Feature Update Failure Detection",
                Category = "System",
                Description = "Sets EnableSafeOSUpdateRollback=1 in WindowsUpdate policy. Enables the Windows Safe OS rollback mechanism for failed feature updates. When a feature update installation fails (BSoD during upgrade, driver incompatibility detected, boot loop), Windows automatically rolls back to the previous working build rather than leaving the endpoint in an unbootable or partially-upgraded state. " +
                    "Feature update installation failures can leave an endpoint in a state where it has partially installed the new version but cannot boot successfully. Without SafeOS rollback enabled, the endpoint may enter a boot repair loop, requiring IT to perform manual recovery (recovery console, reimaging). With SafeOS rollback, Windows detects the boot failure and automatically recovers to the last known good state, minimising end-user downtime and IT support demand for failed feature update deployments.",
                Tags = ["windows-servicing", "rollback", "feature-update", "safeos", "recovery"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "SafeOS rollback enabled; failed feature updates auto-revert to previous working build without manual IT intervention.",
                ApplyOps = [RegOp.SetDword(Key, "EnableSafeOSUpdateRollback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSafeOSUpdateRollback")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSafeOSUpdateRollback", 1)],
            },
            new TweakDef
            {
                Id = "winsvc-enable-compliance-deadline-enforcement",
                Label = "Servicing: Enable Compliance Deadline Enforcement to Prevent Indefinite Update Deferral",
                Category = "System",
                Description = "Sets EnableComplianceDeadlineEnforcement=1 in WindowsUpdate policy. Enables the WUfB compliance deadline mechanism, which automatically enforces update installation (overriding user-controlled active hours and post-deadline deferral settings) when a security update has been available beyond the configured deadline period, ensuring security patches cannot be deferred indefinitely by end-users. " +
                    "Windows Update for Business user deadline controls allow end-users to dismiss and defer reboot prompts after updates are downloaded. In environments without compliance deadline enforcement, a user who repeatedly dismisses reboot prompts can delay security patch installation for weeks or months. The compliance deadline enforcement mechanism ensures that regardless of user behaviour, a security update that has been downloaded for more than the configured deadline period (typically 3–7 days) will install on the next restart.",
                Tags = ["windows-servicing", "compliance-deadline", "security-patch", "forced-reboot", "sla"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Compliance deadline enforcement active; security updates cannot be deferred indefinitely by end-users; SLA enforced.",
                ApplyOps = [RegOp.SetDword(Key, "EnableComplianceDeadlineEnforcement", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableComplianceDeadlineEnforcement")],
                DetectOps = [RegOp.CheckDword(Key, "EnableComplianceDeadlineEnforcement", 1)],
            },
        ];

    }

    // ── WindowsToGoPolicy ──
    private static class _WindowsToGoPolicy
    {
        private const string WtgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PortableOperatingSystem";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wtg-disable-sleep",
                Label = "Disable Sleep States for Windows To Go",
                Category = "System",
                Description = "Sets EnableSleep=0 in the PortableOperatingSystem policy key. "
                    + "Prevents Windows To Go workspaces from entering S1-S3 sleep states while running "
                    + "from a USB drive. Sleep states on WTG disks can corrupt the workspace if the USB "
                    + "connection is interrupted during wake-up. Applying this ensures the system either "
                    + "stays awake or shuts down completely, never entering an intermediate sleep state "
                    + "when running from a WTG workspace. Default: absent (sleep allowed).",
                Tags = ["windows-to-go", "sleep", "usb", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Sleep states disabled for WTG workspaces; prevents USB-to-sleep corruption scenarios.",
                ApplyOps  = [RegOp.SetDword(WtgKey, "EnableSleep", 0)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "EnableSleep")],
                DetectOps = [RegOp.CheckDword(WtgKey, "EnableSleep", 0)],
            },
            new TweakDef
            {
                Id = "wtg-disable-hibernation",
                Label = "Disable Hibernation for Windows To Go",
                Category = "System",
                Description = "Sets EnableHibernation=0 in the PortableOperatingSystem policy key. "
                    + "Prevents Windows To Go workspaces from using the hibernate (S4) power state. "
                    + "Hibernation on a WTG USB workspace saves RAM to the hiberfil.sys on the USB disk, "
                    + "but wake-up can fail if the USB drive is moved or the system firmware changes. "
                    + "Disabling hibernate avoids this by requiring a full shutdown instead. "
                    + "Default: absent (hibernation allowed).",
                Tags = ["windows-to-go", "hibernation", "hibernate", "usb", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hibernation disabled for WTG workspaces; prevents hiberfil corruption on USB devices.",
                ApplyOps  = [RegOp.SetDword(WtgKey, "EnableHibernation", 0)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "EnableHibernation")],
                DetectOps = [RegOp.CheckDword(WtgKey, "EnableHibernation", 0)],
            },
            new TweakDef
            {
                Id = "wtg-block-workspace-creation",
                Label = "Block Windows To Go Workspace Creation",
                Category = "System",
                Description = "Sets NoWorkspaceCreation=1 in the PortableOperatingSystem policy key. "
                    + "Prevents users from using the Windows To Go Workspace Creator wizard to create "
                    + "new WTG workspaces from this machine. Ensures WTG environments are only created "
                    + "by IT administrators and not by standard users who may inadvertently copy "
                    + "sensitive corporate data to an unmanaged USB drive. "
                    + "Default: absent (creation allowed). Recommended: 1 on managed corporate endpoints.",
                Tags = ["windows-to-go", "workspace-creation", "usb", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WTG workspace creation wizard blocked; only IT-created workspaces can be used.",
                ApplyOps  = [RegOp.SetDword(WtgKey, "NoWorkspaceCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "NoWorkspaceCreation")],
                DetectOps = [RegOp.CheckDword(WtgKey, "NoWorkspaceCreation", 1)],
            },
            new TweakDef
            {
                Id = "wtg-block-boot-from-external",
                Label = "Block Booting From External WTG Media",
                Category = "System",
                Description = "Sets BlockBootFromExternalMedia=1 in the PortableOperatingSystem policy key. "
                    + "Prevents this machine from booting a Windows To Go workspace from external USB media. "
                    + "Ensures the machine always boots its internal Windows installation and cannot be "
                    + "redirected by an inserted WTG USB drive. Protects against using WTG to bypass local "
                    + "security controls or Intune/Group Policy enrollment. "
                    + "Default: absent (booting from external media allowed).",
                Tags = ["windows-to-go", "boot", "usb", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WTG boot from external USB media blocked; internal Windows always boots instead.",
                ApplyOps  = [RegOp.SetDword(WtgKey, "BlockBootFromExternalMedia", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "BlockBootFromExternalMedia")],
                DetectOps = [RegOp.CheckDword(WtgKey, "BlockBootFromExternalMedia", 1)],
            },
            new TweakDef
            {
                Id = "wtg-disable-host-offline-folders",
                Label = "Disable Host Offline Folders in Windows To Go",
                Category = "System",
                Description = "Sets NoOfflineFolders=1 in the PortableOperatingSystem policy key. "
                    + "Prevents the WTG workspace from accessing the host machine's Offline Files cache. "
                    + "Ensures that when a user boots into a WTG workspace, they cannot read or write "
                    + "the Offline Files data of the host machine, preventing data leakage from the "
                    + "host's cached network files into the WTG environment. "
                    + "Default: absent (offline folder access allowed). Recommended: 1 on shared/kiosk desktops.",
                Tags = ["windows-to-go", "offline-folders", "data-leakage", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Offline Folders cache on host not accessible from WTG workspace.",
                ApplyOps  = [RegOp.SetDword(WtgKey, "NoOfflineFolders", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "NoOfflineFolders")],
                DetectOps = [RegOp.CheckDword(WtgKey, "NoOfflineFolders", 1)],
            },
            new TweakDef
            {
                Id = "wtg-disable-retail-demo",
                Label = "Disable Retail Demo Mode for Windows To Go",
                Category = "System",
                Description = "Sets DisableRetailDemo=1 in the PortableOperatingSystem policy key. "
                    + "Suppresses the Retail Demo Experience (RDX) from being shown or launched when "
                    + "a WTG workspace boots on a retail display or demo machine. Prevents WTG workspaces "
                    + "from being used as a kiosk demo mode and ensures productive enterprise use only. "
                    + "Default: absent. Recommended: 1 on all non-retail WTG deployments.",
                Tags = ["windows-to-go", "retail-demo", "kiosk", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Retail demo mode suppressed in WTG workspaces.",
                ApplyOps  = [RegOp.SetDword(WtgKey, "DisableRetailDemo", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "DisableRetailDemo")],
                DetectOps = [RegOp.CheckDword(WtgKey, "DisableRetailDemo", 1)],
            },
            new TweakDef
            {
                Id = "wtg-disable-sync-on-metered",
                Label = "Disable Sync Provider on Metered Connection for WTG",
                Category = "System",
                Description = "Sets DisableSyncProviderOnMetered=1 in the PortableOperatingSystem policy key. "
                    + "Prevents the WTG workspace from contacting cloud sync providers (OneDrive, Dropbox, etc.) "
                    + "when the device is on a metered network connection. Reduces data usage costs for WTG "
                    + "workspaces roaming over mobile broadband or tethered hotspots. "
                    + "Default: absent (sync allowed on metered). Recommended: 1 to protect data budgets.",
                Tags = ["windows-to-go", "sync", "metered", "onedrive", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud sync providers blocked on metered connections in WTG workspaces.",
                ApplyOps  = [RegOp.SetDword(WtgKey, "DisableSyncProviderOnMetered", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "DisableSyncProviderOnMetered")],
                DetectOps = [RegOp.CheckDword(WtgKey, "DisableSyncProviderOnMetered", 1)],
            },
            new TweakDef
            {
                Id = "wtg-block-cross-hardware-deploy",
                Label = "Block Cross-Hardware WTG Deployment",
                Category = "System",
                Description = "Sets NoCrossHardwareDeploy=1 in the PortableOperatingSystem policy key. "
                    + "Prevents the WTG workspace from being moved to a different hardware platform once it "
                    + "has been provisioned. Cross-hardware WTG deployment can cause driver conflicts, "
                    + "DHCP/MAC-address confusion, or break hardware-specific licensing tied to the original "
                    + "provisioning machine. Restricting this ensures workspace integrity. "
                    + "Default: absent (cross-hardware allowed). Recommended: 1 in managed enterprise WTG.",
                Tags = ["windows-to-go", "hardware", "deploy", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "WTG workspace cannot be re-provisioned on different hardware.",
                ApplyOps  = [RegOp.SetDword(WtgKey, "NoCrossHardwareDeploy", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "NoCrossHardwareDeploy")],
                DetectOps = [RegOp.CheckDword(WtgKey, "NoCrossHardwareDeploy", 1)],
            },
            new TweakDef
            {
                Id = "wtg-enforce-secure-boot",
                Label = "Enforce Secure Boot for Windows To Go Workspaces",
                Category = "System",
                Description = "Sets RequireSecureBoot=1 in the PortableOperatingSystem policy key. "
                    + "Requires that the host machine's Secure Boot setting be enabled before a WTG "
                    + "workspace will boot. Prevents WTG from being used as an attack vector on machines "
                    + "where Secure Boot has been disabled, ensuring the WTG kernel and boot files are "
                    + "signed and unmodified. "
                    + "Default: absent (Secure Boot not required). Recommended: 1 for security-hardened environments.",
                Tags = ["windows-to-go", "secure-boot", "uefi", "security", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "WTG workspace only boots on machines with Secure Boot enabled.",
                ApplyOps  = [RegOp.SetDword(WtgKey, "RequireSecureBoot", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "RequireSecureBoot")],
                DetectOps = [RegOp.CheckDword(WtgKey, "RequireSecureBoot", 1)],
            },
            new TweakDef
            {
                Id = "wtg-disable-automatic-update",
                Label = "Disable Automatic Windows Update in WTG Workspace",
                Category = "System",
                Description = "Sets NoAutoUpdate=1 in the PortableOperatingSystem policy key. "
                    + "Prevents the WTG workspace from automatically downloading and installing Windows updates "
                    + "while running on the road. Updates in a WTG workspace use the host machine's internet "
                    + "connection and can run out of USB drive space or interrupt productivity. "
                    + "Updates should be applied via WSUS or a scheduled service window instead. "
                    + "Default: absent (automatic updates allowed). Recommended: 1 on managed WTG deployments.",
                Tags = ["windows-to-go", "windows-update", "automatic", "portable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Automatic Windows Update disabled in WTG workspaces; updates must be pushed manually.",
                ApplyOps  = [RegOp.SetDword(WtgKey, "NoAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(WtgKey, "NoAutoUpdate")],
                DetectOps = [RegOp.CheckDword(WtgKey, "NoAutoUpdate", 1)],
            },
        ];

    }

}
