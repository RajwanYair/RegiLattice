// RegiLattice.Core — Tweaks/AdfsFederationPolicy.cs
// Active Directory Federation Services (ADFS) Security Policy — Sprint 435.
// Controls ADFS token lifetimes, extended protection, endpoint access, extranet
// lockout, claims provider trust hardening, and certificate requirements via
// Windows registry Group Policy paths.
// Category: "ADFS Federation Policy" | Slug: adfspol
// Registry paths:
//   HKLM\SOFTWARE\Policies\Microsoft\Windows\ADFS              — ADFS policy
//   HKLM\SOFTWARE\Policies\Microsoft\Windows\Authentication    — authentication
//   HKLM\SYSTEM\CurrentControlSet\Services\adfssrv\Parameters  — ADFS service

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AdfsFederationPolicy
{
    private const string AdfsKey   = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ADFS";
    private const string AuthKey   = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Authentication";
    private const string SvcKey    = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\adfssrv\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id          = "adfspol-enable-extranet-lockout",
                Label       = "Enable ADFS Extranet Smart Lockout",
                Category    = "ADFS Federation Policy",
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
                Category    = "ADFS Federation Policy",
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
                Category    = "ADFS Federation Policy",
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
                Category    = "ADFS Federation Policy",
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
                Category    = "ADFS Federation Policy",
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
                Category    = "ADFS Federation Policy",
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
                Category    = "ADFS Federation Policy",
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
                Category    = "ADFS Federation Policy",
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
                Category    = "ADFS Federation Policy",
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
                Category    = "ADFS Federation Policy",
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
