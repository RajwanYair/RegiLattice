namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 638 — PolicyWindowsHello (Windows Hello for Business PIN policy)
// ─────────────────────────────────────────────────────────────────────────────

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 639 — PolicyEntraId (Microsoft Entra ID / Azure AD device policy)
// ─────────────────────────────────────────────────────────────────────────────

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 640 — PolicyKerberos (Kerberos authentication hardening policy)
// ─────────────────────────────────────────────────────────────────────────────

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 641 — PolicyAppInstaller (App Installer / WinGet MSIX policy)
// ─────────────────────────────────────────────────────────────────────────────

internal static partial class PolicyAuth
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _AzureAdConditionalAccessPolicy.Data,
            .. _AzureAdPrtSsoPolicy.Data,
            .. _AzureAdSsprPolicy.Data,
            .. _AzureAdTenantPolicy.Data,
            .. _BiometricAuthPolicy.Data,
            .. _Biometrics.Data,
            .. _BiometricsConfigPolicy.Data,
            .. _CredentialCachingPolicy.Data,
            .. _CredentialDelegationPolicy.Data,
            .. _CredentialManagerPolicy.Data,
            .. _CredentialRoamingPolicy.Data,
            .. _CredentialUiPolicy.Data,
            .. _EntraDeviceRegistrationPolicy.Data,
            .. _KerberoastMitigationPolicy.Data,
            .. _KerberosAdvanced.Data,
            .. _KerberosArmoringPolicy.Data,
            .. _KerberosDelegationPolicy.Data,
            .. _KerberosEncryptionPolicy.Data,
            .. _KerberosSecurityPolicy.Data,
            .. _LapsPolicy.Data,
            .. _LapsSecurity.Data,
            .. _LegacyAuthPolicy.Data,
            .. _LocalSecurityAuthorityPolicy.Data,
            .. _LogonCachePolicy.Data,
            .. _LogonGpoPolicy.Data,
            .. _LsaProtectionPolicy.Data,
            .. _PasswordlessSignInPolicy.Data,
            .. _SmartCardCredentialsPolicy.Data,
            .. _SmartCardCredProvPolicy.Data,
            .. _WebAuthnPolicy.Data,
            .. _WhfbPinPolicy.Data,
            .. _WindowsHelloAdvPolicy.Data,
            .. _WorkplaceJoinPolicy.Data,
        ];

    // ── AzureAdConditionalAccessPolicy ──
    private static class _AzureAdConditionalAccessPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AzureADAccount";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "aadca-require-mfa-device-compliance",
                    Label = "Require MFA and Device Compliance for AAD Sign-In",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Enforces that Azure AD sign-in sessions require multi-factor authentication and device compliance status checks before granting access to cloud resources.",
                    Tags = ["azure-ad", "mfa", "conditional-access", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "AAD sign-in requires MFA + compliance; non-compliant devices cannot access cloud apps.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowMFAForSignIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowMFAForSignIn")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowMFAForSignIn", 1)],
                },
                new TweakDef
                {
                    Id = "aadca-disable-legacy-auth",
                    Label = "Disable Legacy Authentication Protocols for AAD",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Blocks legacy authentication protocols (Basic Auth, NTLM over AAD) that cannot enforce MFA, preventing bypass of Conditional Access policies through legacy clients.",
                    Tags = ["azure-ad", "legacy-auth", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Legacy auth blocked; older mail clients and apps using Basic Auth to AAD will fail.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockLegacyAuthentication", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockLegacyAuthentication")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockLegacyAuthentication", 1)],
                },
                new TweakDef
                {
                    Id = "aadca-enforce-tap-policy",
                    Label = "Enforce Temporary Access Pass Policy for AAD",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Enforces the Temporary Access Pass (TAP) policy for Azure AD, ensuring one-time codes for account recovery are time-limited and comply with organisational policy.",
                    Tags = ["azure-ad", "tap", "recovery", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "TAP issuance and use governed by policy; emergency recovery codes follow compliance rules.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceTemporaryAccessPassPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceTemporaryAccessPassPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceTemporaryAccessPassPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "aadca-block-personal-accounts",
                    Label = "Block Personal Microsoft Accounts from AAD Sign-In",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Blocks personal (consumer) Microsoft accounts from being used for Azure AD sign-in on managed devices, preventing account mixing between personal and corporate identities.",
                    Tags = ["azure-ad", "personal-accounts", "account-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "MSA (personal) accounts blocked for AAD scenarios on this device.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowMicrosoftAccountSignIn", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowMicrosoftAccountSignIn")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowMicrosoftAccountSignIn", 0)],
                },
                new TweakDef
                {
                    Id = "aadca-restrict-tenant-access",
                    Label = "Restrict AAD Sign-In to Approved Tenants Only",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Restricts Azure AD authentication on this device to specific approved tenant IDs, preventing credential phishing attacks that redirect users to rogue AAD tenants.",
                    Tags = ["azure-ad", "tenant-restriction", "phishing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Only approved AAD tenants accessible; sign-in to unknown tenants is blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictAADSignInToApprovedTenants", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictAADSignInToApprovedTenants")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictAADSignInToApprovedTenants", 1)],
                },
                new TweakDef
                {
                    Id = "aadca-block-workplace-join",
                    Label = "Block Workplace Join for Non-Compliant Devices",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Blocks Azure AD Workplace Join (soft join) for devices that do not meet compliance requirements, preventing non-compliant devices from obtaining SSO tokens.",
                    Tags = ["azure-ad", "workplace-join", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Non-compliant devices cannot Workplace Join; SSO access to AAD resources requires MDM enrolment.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockAADWorkplaceJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockAADWorkplaceJoin")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockAADWorkplaceJoin", 1)],
                },
                new TweakDef
                {
                    Id = "aadca-disable-workplace-join-telemetry",
                    Label = "Disable Workplace Join Telemetry",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Disables telemetry collection during and after Azure AD Workplace Join operations, preventing registration events and diagnostic data from being sent to Microsoft.",
                    Tags = ["azure-ad", "workplace-join", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Workplace Join telemetry disabled; join process completes without sending diagnostic data.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableWorkplaceJoinTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableWorkplaceJoinTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableWorkplaceJoinTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "aadca-disable-wj-cert-prompt",
                    Label = "Disable Workplace Join Certificate Notification",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Disables the certificate-related notification prompts that appear during Workplace Join, preventing user interaction with certificate provisioning dialogs.",
                    Tags = ["azure-ad", "workplace-join", "certificate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Certificate notifications during WJ suppressed; provisioning is fully silent.",
                    ApplyOps = [RegOp.SetDword(Key2, "WorkplaceJoinCertNotificationEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "WorkplaceJoinCertNotificationEnabled")],
                    DetectOps = [RegOp.CheckDword(Key2, "WorkplaceJoinCertNotificationEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "aadca-block-guest-access",
                    Label = "Block Guest Account AAD Sign-In",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Prevents guest and B2B collaboration accounts from signing into Azure AD resources on this device, restricting access to direct organisational members only.",
                    Tags = ["azure-ad", "guest", "b2b", "access-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Guest/B2B accounts blocked; only licensed internal users can access AAD resources.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockGuestAADSignIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockGuestAADSignIn")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockGuestAADSignIn", 1)],
                },
                new TweakDef
                {
                    Id = "aadca-require-intune-compliance",
                    Label = "Require Intune Compliance for AAD Token Issuance",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Requires that the device passes Microsoft Intune compliance policy checks before AAD issues access tokens, blocking cloud resource access from unmanaged or non-compliant devices.",
                    Tags = ["azure-ad", "intune", "compliance", "conditional-access", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Intune compliance required for token issuance; non-enrolled devices lose cloud app access.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireIntuneCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireIntuneCompliance")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireIntuneCompliance", 1)],
                },
            ];
    }

    // ── AzureAdPrtSsoPolicy ──
    private static class _AzureAdPrtSsoPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudAuthentication";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "aadprt-enable-prt-sso",
                    Label = "Enable Primary Refresh Token SSO",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Enables Primary Refresh Token (PRT)-based Single Sign-On via the Web Account Manager (WAM) broker, allowing seamless SSO to Azure AD applications without password re-entry.",
                    Tags = ["azure-ad", "prt", "sso", "wam", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "PRT SSO enabled; users sign in once and access all AAD apps seamlessly.",
                    ApplyOps = [RegOp.SetDword(Key, "EnablePRTBasedSSO", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnablePRTBasedSSO")],
                    DetectOps = [RegOp.CheckDword(Key, "EnablePRTBasedSSO", 1)],
                },
                new TweakDef
                {
                    Id = "aadprt-enable-cae",
                    Label = "Enable Continuous Access Evaluation (CAE) for Tokens",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Enables Continuous Access Evaluation for AAD tokens, ensuring that risk events (user revocation, location change, password reset) immediately invalidate existing access tokens.",
                    Tags = ["azure-ad", "cae", "token-revocation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Active tokens revoked immediately on policy change; real-time security event response.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableCAEForAccessTokens", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableCAEForAccessTokens")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableCAEForAccessTokens", 1)],
                },
                new TweakDef
                {
                    Id = "aadprt-require-phishing-resistant-mfa",
                    Label = "Require Phishing-Resistant MFA for PRT Issuance",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Requires phishing-resistant MFA methods (FIDO2, Windows Hello, Certificate) for issuing Primary Refresh Tokens, blocking PRT issuance via SMS/email OTP which can be phished.",
                    Tags = ["azure-ad", "prt", "mfa", "phishing-resistant", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "SMS/email OTP cannot issue PRTs; only FIDO2/WHfB/certificate auth allowed.",
                    ApplyOps = [RegOp.SetDword(Key, "RequirePhishingResistantMFAForPRT", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequirePhishingResistantMFAForPRT")],
                    DetectOps = [RegOp.CheckDword(Key, "RequirePhishingResistantMFAForPRT", 1)],
                },
                new TweakDef
                {
                    Id = "aadprt-disable-roaming-creds",
                    Label = "Disable PRT Credential Roaming to Other Devices",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Prevents Primary Refresh Token-based credential material from roaming between devices, ensuring each device maintains its own device-bound authentication state.",
                    Tags = ["azure-ad", "prt", "credential-roaming", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PRT creds stay on originating device; no cross-device credential roaming.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePRTRoaming", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePRTRoaming")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePRTRoaming", 1)],
                },
                new TweakDef
                {
                    Id = "aadprt-set-token-cache-lifetime",
                    Label = "Set PRT Token Cache Lifetime to 4 Hours",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Limits the PRT-derived access token cache lifetime to 4 hours, requiring more frequent token refreshes and reducing the window for token theft exploitation.",
                    Tags = ["azure-ad", "prt", "token-cache", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Access tokens cached for 4 hours only; more frequent AAD calls on extended sessions.",
                    ApplyOps = [RegOp.SetDword(Key, "PRTTokenCacheLifetimeHours", 4)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PRTTokenCacheLifetimeHours")],
                    DetectOps = [RegOp.CheckDword(Key, "PRTTokenCacheLifetimeHours", 4)],
                },
                new TweakDef
                {
                    Id = "aadprt-enable-ip-bound-tokens",
                    Label = "Enable IP-Bound Token Binding for PRT SSO",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Enables IP address binding for PRT-derived access tokens, causing AAD to reject tokens presented from IP addresses different from those during token issuance.",
                    Tags = ["azure-ad", "prt", "ip-binding", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Tokens bound to origin IP; token replay from different IP fails. Mobile/VPN users may need exceptions.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableIPBoundTokenBinding", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableIPBoundTokenBinding")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableIPBoundTokenBinding", 1)],
                },
                new TweakDef
                {
                    Id = "aadprt-block-browser-prt-use",
                    Label = "Block Browser Access to PRT SSO Tokens",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Restricts browser extensions from accessing PRT SSO tokens through the WAM broker, preventing potentially malicious browser extensions from stealing SSO session material.",
                    Tags = ["azure-ad", "prt", "browser", "extensions", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Browser extension access to WAM PRT tokens blocked; SSO in browser requires direct AAD sign-in.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockBrowserPRTAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockBrowserPRTAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockBrowserPRTAccess", 1)],
                },
                new TweakDef
                {
                    Id = "aadprt-enable-wam-logging",
                    Label = "Enable WAM Broker Audit Logging",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Enables detailed audit logging for the Web Account Manager (WAM) token broker operations, providing a forensic trail of all SSO token issuance and refresh events.",
                    Tags = ["azure-ad", "wam", "logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WAM operations logged to event log; token abuse events become detectable.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableWAMAuditLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableWAMAuditLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableWAMAuditLogging", 1)],
                },
                new TweakDef
                {
                    Id = "aadprt-require-device-compliance-for-prt",
                    Label = "Require Device Compliance Status for PRT Issuance",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Blocks PRT issuance on devices that do not have a valid Intune compliance status, ensuring that only compliant devices can participate in PRT-based SSO.",
                    Tags = ["azure-ad", "prt", "compliance", "intune", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Non-compliant devices denied PRT; SSO blocked until compliance is restored.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireDeviceComplianceForPRT", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceComplianceForPRT")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireDeviceComplianceForPRT", 1)],
                },
                new TweakDef
                {
                    Id = "aadprt-block-prt-on-shared-device",
                    Label = "Block PRT SSO on Shared/Kiosk Devices",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Blocks Primary Refresh Token-based SSO on devices configured as shared or kiosk devices, preventing cross-user SSO token leakage on multi-user workstations.",
                    Tags = ["azure-ad", "prt", "shared-device", "kiosk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "PRT SSO disabled on shared/kiosk devices; each user must authenticate independently.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPRTOnSharedDevice", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPRTOnSharedDevice")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPRTOnSharedDevice", 1)],
                },
            ];
    }

    // ── AzureAdSsprPolicy ──
    private static class _AzureAdSsprPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SSPR";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "aadsspr-require-two-methods",
                    Label = "Require Two Methods for SSPR Authentication",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Configures Azure AD Self-Service Password Reset to require two authentication methods (e.g., email + phone, or authenticator app + security questions) before a password can be reset.",
                    Tags = ["azure-ad", "sspr", "mfa", "password-reset", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Two SSPR methods required; phishing a single factor is insufficient for account takeover via SSPR.",
                    ApplyOps = [RegOp.SetDword(Key, "NumberOfMethodsRequired", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NumberOfMethodsRequired")],
                    DetectOps = [RegOp.CheckDword(Key, "NumberOfMethodsRequired", 2)],
                },
                new TweakDef
                {
                    Id = "aadsspr-block-sms-method",
                    Label = "Block SMS as SSPR Authentication Method",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Disables SMS (one-time PIN via text message) as an allowed authentication method for SSPR, preventing SIM-swapping attacks from enabling account takeover via password reset.",
                    Tags = ["azure-ad", "sspr", "sms", "sim-swap", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "SMS SSPR blocked; users must use authenticator app, email, or FIDO2 for reset.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSMSMethod", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSMSMethod")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSMSMethod", 1)],
                },
                new TweakDef
                {
                    Id = "aadsspr-block-email-method",
                    Label = "Block External Email as SSPR Authentication Method",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Disables external email address (non-corporate) as an allowed authentication method for SSPR, forcing use of corporate email or authenticator apps.",
                    Tags = ["azure-ad", "sspr", "email", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "External email SSPR method blocked; reduces attack surface through personal email accounts.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableExternalEmailMethod", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableExternalEmailMethod")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableExternalEmailMethod", 1)],
                },
                new TweakDef
                {
                    Id = "aadsspr-require-security-questions-count",
                    Label = "Require Minimum 5 Security Questions for SSPR",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Requires users to answer at least 5 security questions for SSPR if the security questions method is enabled, increasing the difficulty of social engineering attacks.",
                    Tags = ["azure-ad", "sspr", "security-questions", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "5 security questions required for SSPR; harder to socially engineer an account reset.",
                    ApplyOps = [RegOp.SetDword(Key, "NumberOfSecurityQuestionsRequired", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NumberOfSecurityQuestionsRequired")],
                    DetectOps = [RegOp.CheckDword(Key, "NumberOfSecurityQuestionsRequired", 5)],
                },
                new TweakDef
                {
                    Id = "aadsspr-enforce-registration-at-logon",
                    Label = "Enforce SSPR Registration at Next Logon",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Forces users who have not registered SSPR methods to register at their next logon, ensuring all accounts have recovery methods configured before they need them.",
                    Tags = ["azure-ad", "sspr", "registration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Unregistered users prompted to register SSPR methods at logon; one-time interruption.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireRegistrationAtLogon", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireRegistrationAtLogon")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireRegistrationAtLogon", 1)],
                },
                new TweakDef
                {
                    Id = "aadsspr-set-reconfirm-interval",
                    Label = "Set SSPR Auth Method Reconfirmation to 180 Days",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Configures the SSPR method reconfirmation interval to 180 days, requiring users to verify their registered authentication methods are still valid every six months.",
                    Tags = ["azure-ad", "sspr", "reconfirmation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "SSPR methods verified every 6 months; stale phone numbers or emails are caught and updated.",
                    ApplyOps = [RegOp.SetDword(Key, "ReconfirmAuthMethodsIntervalDays", 180)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ReconfirmAuthMethodsIntervalDays")],
                    DetectOps = [RegOp.CheckDword(Key, "ReconfirmAuthMethodsIntervalDays", 180)],
                },
                new TweakDef
                {
                    Id = "aadsspr-require-authenticator-app",
                    Label = "Require Microsoft Authenticator App for SSPR",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Requires the Microsoft Authenticator app notification or code as an allowed (and preferred) SSPR method, providing stronger MFA-equivalent strength for password resets.",
                    Tags = ["azure-ad", "sspr", "authenticator", "mfa", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Authenticator app required for SSPR; users without the app need to register alternative strong methods.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAuthenticatorApp", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAuthenticatorApp")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAuthenticatorApp", 1)],
                },
                new TweakDef
                {
                    Id = "aadsspr-block-writeback-on-premises",
                    Label = "Block SSPR Password Writeback to On-Premises",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Disables SSPR Password Writeback which synchronises cloud reset passwords back to the on-premises Active Directory, preventing cloud-initiated account takeover from affecting on-prem.",
                    Tags = ["azure-ad", "sspr", "writeback", "on-premises", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Password Writeback disabled; cloud SSPR resets do not propagate to on-prem AD.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePasswordWriteback", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordWriteback")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePasswordWriteback", 1)],
                },
                new TweakDef
                {
                    Id = "aadsspr-notify-admin-on-reset",
                    Label = "Notify Admins on SSPR Use",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Enables administrator notification when a user uses SSPR to reset their password, creating an audit trail and alerting IT to potential account compromise events.",
                    Tags = ["azure-ad", "sspr", "notification", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "IT admins notified on each SSPR reset event; allows rapid response to suspicious resets.",
                    ApplyOps = [RegOp.SetDword(Key, "NotifyAdminsOnUserPasswordReset", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NotifyAdminsOnUserPasswordReset")],
                    DetectOps = [RegOp.CheckDword(Key, "NotifyAdminsOnUserPasswordReset", 1)],
                },
                new TweakDef
                {
                    Id = "aadsspr-limit-self-service-unlock",
                    Label = "Limit SSPR Self-Service Account Unlock Attempts",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Limits the number of self-service account unlock attempts via SSPR per hour to prevent brute-force enumeration of SSPR methods against locked accounts.",
                    Tags = ["azure-ad", "sspr", "account-unlock", "brute-force", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "SSPR unlock attempts rate-limited; brute-force account enumeration via unlock is prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxUnlockAttemptsPerHour", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxUnlockAttemptsPerHour")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxUnlockAttemptsPerHour", 3)],
                },
            ];
    }

    // ── AzureAdTenantPolicy ──
    private static class _AzureAdTenantPolicy
    {
        private const string AadKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AzureADAccount";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "aadtenant-block-email-signin",
                    Label = "Block Azure AD Email (MSA) Sign-In",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets AllowEmailSignIn=0 to prevent users from signing in with personal Microsoft accounts or email credentials instead of their enterprise Azure AD identity. Enforces corporate identity exclusivity.",
                    Tags = ["aad", "signin", "msa", "policy", "identity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks personal MSA/email sign-in; corporate Azure AD credentials unaffected.",
                    ApplyOps = [RegOp.SetDword(AadKey, "AllowEmailSignIn", 0)],
                    RemoveOps = [RegOp.DeleteValue(AadKey, "AllowEmailSignIn")],
                    DetectOps = [RegOp.CheckDword(AadKey, "AllowEmailSignIn", 0)],
                },
                new TweakDef
                {
                    Id = "aadtenant-block-non-enterprise-join",
                    Label = "Block Non-Enterprise Azure AD Device Join",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets BlockNonEnterpriseAADUserFromJoining=1. Prevents consumer or personal Azure AD accounts from joining this device, limiting device registration exclusively to managed enterprise tenants.",
                    Tags = ["aad", "join", "enterprise", "policy", "device"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks BYOD personal AAD joins; enterprise-managed joins proceed normally.",
                    ApplyOps = [RegOp.SetDword(AadKey, "BlockNonEnterpriseAADUserFromJoining", 1)],
                    RemoveOps = [RegOp.DeleteValue(AadKey, "BlockNonEnterpriseAADUserFromJoining")],
                    DetectOps = [RegOp.CheckDword(AadKey, "BlockNonEnterpriseAADUserFromJoining", 1)],
                },
                new TweakDef
                {
                    Id = "aadtenant-disable-consumer-apps",
                    Label = "Disable Consumer Azure AD App Enrollment",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets AllowWindowsConsumerApps=0. Prevents consumer-oriented Azure AD application enrollment on this device, blocking personal-use apps from registering with the user's Microsoft account identity.",
                    Tags = ["aad", "consumer", "apps", "policy", "enrollment"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks consumer app AAD enrollment; enterprise LOB app registration unaffected.",
                    ApplyOps = [RegOp.SetDword(AadKey, "AllowWindowsConsumerApps", 0)],
                    RemoveOps = [RegOp.DeleteValue(AadKey, "AllowWindowsConsumerApps")],
                    DetectOps = [RegOp.CheckDword(AadKey, "AllowWindowsConsumerApps", 0)],
                },
                new TweakDef
                {
                    Id = "aadtenant-enforce-tenant-restrictions",
                    Label = "Enforce Azure AD Tenant Restrictions",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets EnableTenantRestrictions=1. Activates tenant restriction policy, limiting which Azure AD tenants users can authenticate against. Works with network-level tenant restriction headers to block unauthorised tenant sign-ins.",
                    Tags = ["aad", "tenant", "restrictions", "policy", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "May block access to partner tenants not on the allowed list; coordinate with IT before applying.",
                    ApplyOps = [RegOp.SetDword(AadKey, "EnableTenantRestrictions", 1)],
                    RemoveOps = [RegOp.DeleteValue(AadKey, "EnableTenantRestrictions")],
                    DetectOps = [RegOp.CheckDword(AadKey, "EnableTenantRestrictions", 1)],
                },
                new TweakDef
                {
                    Id = "aadtenant-block-guest-accounts",
                    Label = "Block Azure AD Guest Account Sign-In",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets AllowGuestAccounts=0. Prevents guest or B2B invited user accounts from signing into this device, restricting access to member accounts belonging to the managed enterprise tenant only.",
                    Tags = ["aad", "guest", "b2b", "policy", "access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks guest/B2B sign-in to Windows; no impact on existing enterprise member accounts.",
                    ApplyOps = [RegOp.SetDword(AadKey, "AllowGuestAccounts", 0)],
                    RemoveOps = [RegOp.DeleteValue(AadKey, "AllowGuestAccounts")],
                    DetectOps = [RegOp.CheckDword(AadKey, "AllowGuestAccounts", 0)],
                },
                new TweakDef
                {
                    Id = "aadtenant-block-personal-accounts",
                    Label = "Block Personal Microsoft Account Links",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets BlockPersonalMicrosoftAccounts=1. Prevents users from adding or connecting personal Microsoft accounts to this device, ensuring only work/school accounts are provisioned.",
                    Tags = ["aad", "personal", "microsoft", "account", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks personal MSA account linking; existing enterprise account unaffected.",
                    ApplyOps = [RegOp.SetDword(AadKey, "BlockPersonalMicrosoftAccounts", 1)],
                    RemoveOps = [RegOp.DeleteValue(AadKey, "BlockPersonalMicrosoftAccounts")],
                    DetectOps = [RegOp.CheckDword(AadKey, "BlockPersonalMicrosoftAccounts", 1)],
                },
                new TweakDef
                {
                    Id = "aadtenant-require-privacy-consent",
                    Label = "Require Azure AD Privacy Consent",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets RequirePrivacyConsent=1. Forces display of the enterprise privacy consent dialog before Azure AD account setup, ensuring users acknowledge the organisation's data handling policy.",
                    Tags = ["aad", "privacy", "consent", "policy", "gdpr"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Adds a consent dialog to AAD provisioning flow; no impact on authenticated sessions.",
                    ApplyOps = [RegOp.SetDword(AadKey, "RequirePrivacyConsent", 1)],
                    RemoveOps = [RegOp.DeleteValue(AadKey, "RequirePrivacyConsent")],
                    DetectOps = [RegOp.CheckDword(AadKey, "RequirePrivacyConsent", 1)],
                },
                new TweakDef
                {
                    Id = "aadtenant-disable-shared-device-signin",
                    Label = "Disable Shared Azure AD Device Sign-In Mode",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets AllowSharedLocalAppData=0. Prevents Azure AD shared device mode from allowing local app data to persist between users, protecting data isolation on shared Windows kiosks and shared endpoints.",
                    Tags = ["aad", "shared", "kiosk", "policy", "isolation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enforces per-user app data isolation on shared AAD devices; no impact on dedicated devices.",
                    ApplyOps = [RegOp.SetDword(AadKey, "AllowSharedLocalAppData", 0)],
                    RemoveOps = [RegOp.DeleteValue(AadKey, "AllowSharedLocalAppData")],
                    DetectOps = [RegOp.CheckDword(AadKey, "AllowSharedLocalAppData", 0)],
                },
                new TweakDef
                {
                    Id = "aadtenant-block-home-edition-join",
                    Label = "Block AAD Join on Windows Home Editions",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets AllowAADPasswordReset=0. Disables self-service password reset sign-in from the Windows lock screen for Azure AD accounts, preventing unauthenticated SSPR attempts that could expose reset links.",
                    Tags = ["aad", "password", "reset", "policy", "lockscreen"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Removes SSPR link from lock screen; users can still reset via browser or IT helpdesk.",
                    ApplyOps = [RegOp.SetDword(AadKey, "AllowAADPasswordReset", 0)],
                    RemoveOps = [RegOp.DeleteValue(AadKey, "AllowAADPasswordReset")],
                    DetectOps = [RegOp.CheckDword(AadKey, "AllowAADPasswordReset", 0)],
                },
                new TweakDef
                {
                    Id = "aadtenant-disable-cloud-clipboard-aad",
                    Label = "Disable Azure AD Cloud Clipboard Sync",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets AllowCrossDeviceClipboard=0 in the Azure AD account policy scope. Prevents clipboard history from syncing to other Azure AD-joined or registered devices via the Microsoft cloud clipboard service.",
                    Tags = ["aad", "clipboard", "sync", "policy", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Disables cross-device clipboard roaming for AAD users; local clipboard history unaffected.",
                    ApplyOps = [RegOp.SetDword(AadKey, "AllowCrossDeviceClipboard", 0)],
                    RemoveOps = [RegOp.DeleteValue(AadKey, "AllowCrossDeviceClipboard")],
                    DetectOps = [RegOp.CheckDword(AadKey, "AllowCrossDeviceClipboard", 0)],
                },
            ];
    }

    // ── BiometricAuthPolicy ──
    private static class _BiometricAuthPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "biometric-disable-face-recognition",
                    Label = "Disable Windows Hello Face Recognition",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Disables Windows Hello Face Recognition, preventing camera-based biometric sign-in without disabling other biometric factors like fingerprint.",
                    Tags = ["biometrics", "windows-hello", "face", "camera", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Face recognition disabled; fingerprint and PIN sign-in still available.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableFaceRecognition", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableFaceRecognition")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableFaceRecognition", 1)],
                },
                new TweakDef
                {
                    Id = "biometric-disable-fingerprint",
                    Label = "Disable Windows Hello Fingerprint Sign-In",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Disables Windows Hello fingerprint sign-in without disabling other biometric or WHfB credential types, useful in environments where fingerprint readers present a shared contamination risk.",
                    Tags = ["biometrics", "windows-hello", "fingerprint", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Fingerprint sign-in disabled; face recognition and PIN still available.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableFingerprintSignIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableFingerprintSignIn")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableFingerprintSignIn", 1)],
                },
                new TweakDef
                {
                    Id = "biometric-require-anti-spoofing",
                    Label = "Require Anti-Spoofing for Face Recognition",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Requires that Windows Hello Face Recognition use enhanced anti-spoofing (infrared liveness detection), rejecting 2D photo attacks and blocking face sign-in from cameras without IR.",
                    Tags = ["biometrics", "windows-hello", "anti-spoofing", "face", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Anti-spoofing required; face sign-in only on cameras with IR liveness, defeating photo/mask attacks.",
                    ApplyOps = [RegOp.SetDword(Key, "FacialFeaturesUseEnhancedAntiSpoofing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "FacialFeaturesUseEnhancedAntiSpoofing")],
                    DetectOps = [RegOp.CheckDword(Key, "FacialFeaturesUseEnhancedAntiSpoofing", 1)],
                },
                new TweakDef
                {
                    Id = "biometric-block-domain-users-biometrics",
                    Label = "Block Biometric Sign-In for Domain Accounts",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Disables biometric sign-in for domain (Active Directory) accounts, requiring domain users to authenticate with WHfB PIN or password instead of biometrics for high-security domains.",
                    Tags = ["biometrics", "domain-account", "windows-hello", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Domain account biometric sign-in blocked; domain users must use PIN or password.",
                    ApplyOps = [RegOp.SetDword(Key, "DomainAccountsEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DomainAccountsEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "DomainAccountsEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "biometric-disable-biometrics-for-uac",
                    Label = "Disable Biometric Authentication for UAC Prompts",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Prevents biometrics (fingerprint or face) from being used to approve User Account Control elevation prompts, requiring a PIN or password for admin approval dialogs.",
                    Tags = ["biometrics", "uac", "elevation", "windows-hello", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Biometrics blocked for UAC; admin elevations require PIN or password approval.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableBiometricForUAC", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableBiometricForUAC")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableBiometricForUAC", 1)],
                },
                new TweakDef
                {
                    Id = "biometric-require-admin-enroll",
                    Label = "Require Admin Approval to Enroll Biometrics",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Requires administrator approval to enroll new biometric credentials on managed devices, preventing unauthorized enrollment of biometrics on shared or managed workstations.",
                    Tags = ["biometrics", "enrollment", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Biometric enrollment requires admin approval; self-service biometric setup restricted.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForBiometricEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForBiometricEnrollment")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForBiometricEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "biometric-log-biometric-auth-events",
                    Label = "Enable Audit Logging for All Biometric Authentication Events",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Enables Windows event log entries for every biometric authentication attempt (success, failure, lockout), providing an audit trail for biometric sign-in usage.",
                    Tags = ["biometrics", "audit-log", "windows-hello", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Biometric auth events logged; both success and failure attempts recorded in Security event log.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditBiometricAuthEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditBiometricAuthEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditBiometricAuthEvents", 1)],
                },
                new TweakDef
                {
                    Id = "biometric-disable-biometric-third-party",
                    Label = "Block Third-Party Biometric Device Drivers",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Prevents third-party biometric device drivers from registering with the Windows Biometric Framework, restricting biometric authentication to Microsoft-signed and WHQL-certified biometric hardware.",
                    Tags = ["biometrics", "third-party", "driver", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Third-party biometric drivers blocked; only WHQL-certified biometric hardware recognised.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyBiometricDrivers", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyBiometricDrivers")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyBiometricDrivers", 1)],
                },
                new TweakDef
                {
                    Id = "biometric-clear-biometrics-on-lock",
                    Label = "Clear Biometric Authentication Cache on Screen Lock",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Clears cached biometric authentication tokens when the screen locks, requiring a fresh biometric scan after every lock event rather than allowing replay of a recently cached biometric match.",
                    Tags = ["biometrics", "cache", "screen-lock", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Biometric cache cleared on lock; full biometric scan required after every screen lock.",
                    ApplyOps = [RegOp.SetDword(Key, "ClearBiometricCacheOnLock", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ClearBiometricCacheOnLock")],
                    DetectOps = [RegOp.CheckDword(Key, "ClearBiometricCacheOnLock", 1)],
                },
            ];
    }

    // ── Biometrics ──
    private static class _Biometrics
    {
        private const string Bio = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics";
        private const string BioCP = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics\Credential Provider";
        private const string BioFace = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics\FacialFeatures";
        private const string Whfb = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";
        private const string WhfbPin = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\PINComplexity";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "bio-disable-biometrics",
                Label = "Disable Windows Biometrics Service",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Disables the Windows Biometric Service (WbioSrvc) via group policy. "
                    + "Prevents fingerprint, face, and iris sensors from being used for "
                    + "authentication. Enabled=0 in the Biometrics policy key.",
                Tags = ["biometrics", "windows hello", "security", "sign-in"],
                RegistryKeys = [Bio],
                ApplyOps = [RegOp.SetDword(Bio, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Bio, "Enabled")],
                DetectOps = [RegOp.CheckDword(Bio, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "bio-disable-biometrics-domain",
                Label = "Disable Biometrics for Domain / AAD Users",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Blocks domain-joined and Azure AD joined accounts from using biometric "
                    + "sign-in. Does not affect local accounts. EnabledOnDomain=0.",
                Tags = ["biometrics", "domain", "aad", "security"],
                RegistryKeys = [Bio],
                ApplyOps = [RegOp.SetDword(Bio, "EnabledOnDomain", 0)],
                RemoveOps = [RegOp.DeleteValue(Bio, "EnabledOnDomain")],
                DetectOps = [RegOp.CheckDword(Bio, "EnabledOnDomain", 0)],
            },
            new TweakDef
            {
                Id = "bio-disable-biometric-sign-in",
                Label = "Disable Biometric Sign-In via Credential Provider",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Prevents biometric factors (fingerprint, face) from being offered on the "
                    + "Windows sign-in screen. Enabled=0 in the Credential Provider subkey.",
                Tags = ["biometrics", "sign-in", "credential provider", "security"],
                RegistryKeys = [BioCP],
                ApplyOps = [RegOp.SetDword(BioCP, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(BioCP, "Enabled")],
                DetectOps = [RegOp.CheckDword(BioCP, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "bio-enable-facial-anti-spoofing",
                Label = "Enable Windows Hello Facial Anti-Spoofing (ESS)",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Enables Enhanced Anti-Spoofing for Windows Hello facial recognition. "
                    + "Requires certified ISO 30107-3 PAD-compliant hardware. "
                    + "EnhancedAntiSpoofing=1. Recommended on devices that support it.",
                Tags = ["biometrics", "face recognition", "anti-spoofing", "security", "ess"],
                RegistryKeys = [BioFace],
                ApplyOps = [RegOp.SetDword(BioFace, "EnhancedAntiSpoofing", 1)],
                RemoveOps = [RegOp.DeleteValue(BioFace, "EnhancedAntiSpoofing")],
                DetectOps = [RegOp.CheckDword(BioFace, "EnhancedAntiSpoofing", 1)],
            },
            new TweakDef
            {
                Id = "bio-whfb-require-tpm",
                Label = "Require TPM for Windows Hello for Business Keys",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Forces Windows Hello for Business to store credential keys in the hardware "
                    + "TPM chip, blocking software-based key storage. RequireSecurityDevice=1. "
                    + "Devices without a TPM 2.0 chip will not be able to enrol.",
                Tags = ["windows hello", "tpm", "security", "whfb", "hardware"],
                RegistryKeys = [Whfb],
                ApplyOps = [RegOp.SetDword(Whfb, "RequireSecurityDevice", 1)],
                RemoveOps = [RegOp.DeleteValue(Whfb, "RequireSecurityDevice")],
                DetectOps = [RegOp.CheckDword(Whfb, "RequireSecurityDevice", 1)],
            },
            new TweakDef
            {
                Id = "bio-whfb-pin-min-length",
                Label = "Set Minimum Windows Hello PIN Length to 8",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Enforces a minimum of 8 characters for the Windows Hello PIN via the "
                    + "PINComplexity group policy. Increases brute-force resistance. "
                    + "MinimumPINLength=8.",
                Tags = ["windows hello", "pin", "complexity", "security", "whfb"],
                RegistryKeys = [WhfbPin],
                ApplyOps = [RegOp.SetDword(WhfbPin, "MinimumPINLength", 8)],
                RemoveOps = [RegOp.DeleteValue(WhfbPin, "MinimumPINLength")],
                DetectOps = [RegOp.CheckDword(WhfbPin, "MinimumPINLength", 8)],
            },
            new TweakDef
            {
                Id = "bio-whfb-pin-require-digits",
                Label = "Require Digits in Windows Hello PIN",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Requires at least one numeric digit in the Windows Hello PIN. " + "Prevents all-letter PINs. Digits=1 in PINComplexity.",
                Tags = ["windows hello", "pin", "complexity", "digits", "security"],
                RegistryKeys = [WhfbPin],
                ApplyOps = [RegOp.SetDword(WhfbPin, "Digits", 1)],
                RemoveOps = [RegOp.DeleteValue(WhfbPin, "Digits")],
                DetectOps = [RegOp.CheckDword(WhfbPin, "Digits", 1)],
            },
            new TweakDef
            {
                Id = "bio-whfb-pin-require-uppercase",
                Label = "Require Uppercase Letters in Windows Hello PIN",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description = "Requires at least one uppercase letter in the Windows Hello PIN. " + "UppercaseLetters=1 in PINComplexity.",
                Tags = ["windows hello", "pin", "complexity", "uppercase", "security"],
                RegistryKeys = [WhfbPin],
                ApplyOps = [RegOp.SetDword(WhfbPin, "UppercaseLetters", 1)],
                RemoveOps = [RegOp.DeleteValue(WhfbPin, "UppercaseLetters")],
                DetectOps = [RegOp.CheckDword(WhfbPin, "UppercaseLetters", 1)],
            },
            new TweakDef
            {
                Id = "bio-whfb-pin-require-lowercase",
                Label = "Require Lowercase Letters in Windows Hello PIN",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description = "Requires at least one lowercase letter in the Windows Hello PIN. " + "LowercaseLetters=1 in PINComplexity.",
                Tags = ["windows hello", "pin", "complexity", "lowercase", "security"],
                RegistryKeys = [WhfbPin],
                ApplyOps = [RegOp.SetDword(WhfbPin, "LowercaseLetters", 1)],
                RemoveOps = [RegOp.DeleteValue(WhfbPin, "LowercaseLetters")],
                DetectOps = [RegOp.CheckDword(WhfbPin, "LowercaseLetters", 1)],
            },
            new TweakDef
            {
                Id = "bio-whfb-pin-expiry",
                Label = "Set Windows Hello PIN Expiration to 90 Days",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Forces Windows Hello PINs to expire after 90 days, requiring users to "
                    + "create a new PIN on next sign-in. Expiration=90 in PINComplexity.",
                Tags = ["windows hello", "pin", "expiration", "rotation", "security"],
                RegistryKeys = [WhfbPin],
                ApplyOps = [RegOp.SetDword(WhfbPin, "Expiration", 90)],
                RemoveOps = [RegOp.DeleteValue(WhfbPin, "Expiration")],
                DetectOps = [RegOp.CheckDword(WhfbPin, "Expiration", 90)],
            },
        ];
    }

    // ── BiometricsConfigPolicy ──
    private static class _BiometricsConfigPolicy
    {
        private const string BioKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Biometrics";
        private const string BioDomain = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Biometrics\DomainAccounts";
        private const string BioFace = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Biometrics\FacialFeatures";
        private const string BioEnroll = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Biometrics\Enrollment";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "biopol-disable-biometrics",
                    Label = "Disable Windows Biometrics Service",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets Enabled=0 in the Biometrics policy key to disable the Windows biometric framework. Prevents all biometric sign-in methods including fingerprint and facial recognition.",
                    Tags = ["biometrics", "security", "policy", "sign-in"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Disables all Windows Hello biometric sign-in; users must use PIN or password.",
                    ApplyOps = [RegOp.SetDword(BioKey, "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(BioKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(BioKey, "Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "biopol-block-domain-biometric-logon",
                    Label = "Block Biometric Domain Logon",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets Enabled=0 under the DomainAccounts subkey to block domain-joined users from authenticating with biometrics. Useful in high-security enterprise environments.",
                    Tags = ["biometrics", "domain", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Domain users must use passwords or smart cards; no fingerprint/face logon.",
                    ApplyOps = [RegOp.SetDword(BioDomain, "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(BioDomain, "Enabled")],
                    DetectOps = [RegOp.CheckDword(BioDomain, "Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "biopol-disable-secondary-auth-factor",
                    Label = "Disable Biometric Secondary Authentication Factor",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets SecondaryAuthenticationFactor=0 to prevent biometrics from being used as a secondary authentication factor on top of primary credentials.",
                    Tags = ["biometrics", "mfa", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Removes biometric option from secondary authentication; users rely on password/PIN only.",
                    ApplyOps = [RegOp.SetDword(BioKey, "SecondaryAuthenticationFactor", 0)],
                    RemoveOps = [RegOp.DeleteValue(BioKey, "SecondaryAuthenticationFactor")],
                    DetectOps = [RegOp.CheckDword(BioKey, "SecondaryAuthenticationFactor", 0)],
                },
                new TweakDef
                {
                    Id = "biopol-disable-domain-secondary-auth",
                    Label = "Block Domain Biometric Secondary Authentication",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets SecondaryAuthenticationFactor=0 under DomainAccounts to prevent domain users from using biometrics as a secondary authentication factor.",
                    Tags = ["biometrics", "domain", "mfa", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Domain biometric MFA disabled; aligns with strict enterprise authentication policies.",
                    ApplyOps = [RegOp.SetDword(BioDomain, "SecondaryAuthenticationFactor", 0)],
                    RemoveOps = [RegOp.DeleteValue(BioDomain, "SecondaryAuthenticationFactor")],
                    DetectOps = [RegOp.CheckDword(BioDomain, "SecondaryAuthenticationFactor", 0)],
                },
                new TweakDef
                {
                    Id = "biopol-enforce-enhanced-anti-spoofing",
                    Label = "Enforce Enhanced Facial Anti-Spoofing",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets EnhancedAntiSpoofing=1 under FacialFeatures to require cameras with IR depth sensors for facial recognition, blocking photo or video spoofing attempts.",
                    Tags = ["biometrics", "face", "anti-spoofing", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enforces IR-based facial recognition; prevents photo/video spoofing attacks.",
                    ApplyOps = [RegOp.SetDword(BioFace, "EnhancedAntiSpoofing", 1)],
                    RemoveOps = [RegOp.DeleteValue(BioFace, "EnhancedAntiSpoofing")],
                    DetectOps = [RegOp.CheckDword(BioFace, "EnhancedAntiSpoofing", 1)],
                },
                new TweakDef
                {
                    Id = "biopol-disable-alternative-auth-factor",
                    Label = "Disable Alternative Authentication Factor via Biometrics",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets AlternativeAuthenticationFactor=0 to disable alternative biometric authentication factors such as vein pattern readers or external biometric devices.",
                    Tags = ["biometrics", "security", "policy", "authentication"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Blocks non-standard biometric auth factors; standard fingerprint/face are separately controlled.",
                    ApplyOps = [RegOp.SetDword(BioKey, "AlternativeAuthenticationFactor", 0)],
                    RemoveOps = [RegOp.DeleteValue(BioKey, "AlternativeAuthenticationFactor")],
                    DetectOps = [RegOp.CheckDword(BioKey, "AlternativeAuthenticationFactor", 0)],
                },
                new TweakDef
                {
                    Id = "biopol-block-biometric-enrollment",
                    Label = "Block New Biometric Credential Enrollment",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets BlockNewEnrollment=1 under the Enrollment subkey to prevent users from enrolling new biometric credentials (fingerprints or face). Existing credentials remain usable.",
                    Tags = ["biometrics", "enrollment", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "New biometric registrations are blocked; previously enrolled credentials still work.",
                    ApplyOps = [RegOp.SetDword(BioEnroll, "BlockNewEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(BioEnroll, "BlockNewEnrollment")],
                    DetectOps = [RegOp.CheckDword(BioEnroll, "BlockNewEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "biopol-disable-biometric-logon-ui",
                    Label = "Disable Biometric Sign-In UI",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets AllowLogon=0 to remove the biometric sign-in option from the Windows sign-in screen, ensuring only PIN, password, or smart card options are presented.",
                    Tags = ["biometrics", "logon", "ui", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Biometric option removed from sign-in screen; affects UI only, not underlying credentials.",
                    ApplyOps = [RegOp.SetDword(BioKey, "AllowLogon", 0)],
                    RemoveOps = [RegOp.DeleteValue(BioKey, "AllowLogon")],
                    DetectOps = [RegOp.CheckDword(BioKey, "AllowLogon", 0)],
                },
                new TweakDef
                {
                    Id = "biopol-disable-biometric-cred-provider",
                    Label = "Disable Biometric Credential Provider",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets DisableCredentialProviders=1 to disable the Windows biometric credential provider at the system level, removing biometric authentication from all credential prompts.",
                    Tags = ["biometrics", "credential", "provider", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "System-wide removal of biometric credential provider; may affect third-party apps relying on Windows Hello.",
                    ApplyOps = [RegOp.SetDword(BioKey, "DisableCredentialProviders", 1)],
                    RemoveOps = [RegOp.DeleteValue(BioKey, "DisableCredentialProviders")],
                    DetectOps = [RegOp.CheckDword(BioKey, "DisableCredentialProviders", 1)],
                },
                new TweakDef
                {
                    Id = "biopol-disable-face-id-enrollment",
                    Label = "Disable Windows Hello Face Recognition Enrollment",
                    Category = "User Account — Azure Ad Conditional Access",
                    Description =
                        "Sets Enabled=0 under the FacialFeatures key to disable facial recognition in Windows Hello entirely, while other biometric methods may remain available.",
                    Tags = ["biometrics", "face", "windows-hello", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Face recognition sign-in disabled; fingerprint biometrics may still function if enabled.",
                    ApplyOps = [RegOp.SetDword(BioFace, "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(BioFace, "Enabled")],
                    DetectOps = [RegOp.CheckDword(BioFace, "Enabled", 0)],
                },
            ];
    }

    // ── CredentialCachingPolicy ──
    private static class _CredentialCachingPolicy
    {
        private const string CredSSP = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\CredSSP\Parameters";

        private const string CredDel = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation";

        private const string WDigest = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest";

        private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "credcache-mitigate-credssp-oracle",
                Label = "Mitigate CredSSP oracle vulnerability (CVE-2018-0886)",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets CredSSP to 'Mitigated' to prevent credential forging via the encryption oracle. "
                    + "AllowEncryptionOracle=1. Values: 0=Vulnerable, 1=Mitigated, 2=Force Updated. "
                    + "Blocks connections to unpatched RDP servers (requires KB4103723 on remote).",
                Tags = ["credssp", "oracle", "rdp", "cve", "hardening"],
                ApplyOps = [RegOp.SetDword(CredSSP, "AllowEncryptionOracle", 1)],
                RemoveOps = [RegOp.DeleteValue(CredSSP, "AllowEncryptionOracle")],
                DetectOps = [RegOp.CheckDword(CredSSP, "AllowEncryptionOracle", 1)],
            },
            new TweakDef
            {
                Id = "credcache-restrict-rdp-admin-delegation",
                Label = "Restrict remote admin credential delegation",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Restricts remote administration credential delegation to prevent pass-the-hash attacks. "
                    + "RestrictedRemoteAdministration=1. Requires RDP clients to use RestrictedAdmin mode.",
                Tags = ["rdp", "delegation", "restricted-admin", "hardening"],
                ApplyOps = [RegOp.SetDword(CredDel, "RestrictedRemoteAdministration", 1)],
                RemoveOps = [RegOp.DeleteValue(CredDel, "RestrictedRemoteAdministration")],
                DetectOps = [RegOp.CheckDword(CredDel, "RestrictedRemoteAdministration", 1)],
            },
            new TweakDef
            {
                Id = "credcache-rdp-admin-type-protect",
                Label = "Require Protected Users or Restricted Admin for RDP",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Constrains remote RDP sessions to Protected Users group / Restricted Admin / Remote Credential Guard. "
                    + "RestrictedRemoteAdministrationType=3. Prevents clear-text credential exposure.",
                Tags = ["rdp", "protected-users", "delegation", "hardening"],
                ApplyOps = [RegOp.SetDword(CredDel, "RestrictedRemoteAdministrationType", 3)],
                RemoveOps = [RegOp.DeleteValue(CredDel, "RestrictedRemoteAdministrationType")],
                DetectOps = [RegOp.CheckDword(CredDel, "RestrictedRemoteAdministrationType", 3)],
            },
            new TweakDef
            {
                Id = "credcache-disable-wdigest",
                Label = "Disable WDigest plaintext credential caching",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows from storing credentials in plain text in memory for WDigest authentication. "
                    + "UseLogonCredential=0. Mitigates Mimikatz and similar credential-dumping attacks.",
                Tags = ["wdigest", "credentials", "lsass", "mimikatz", "hardening"],
                ApplyOps = [RegOp.SetDword(WDigest, "UseLogonCredential", 0)],
                RemoveOps = [RegOp.DeleteValue(WDigest, "UseLogonCredential")],
                DetectOps = [RegOp.CheckDword(WDigest, "UseLogonCredential", 0)],
            },
            new TweakDef
            {
                Id = "credcache-disable-domain-creds",
                Label = "Block storing network authentication credentials",
                Category = "User Account — Azure Ad Conditional Access",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows from storing credentials for network authentication to domain resources. "
                    + "DisableDomainCreds=1. Network passwords will not be saved in Credential Manager.",
                Tags = ["credentials", "domain", "storage", "hardening"],
                ApplyOps = [RegOp.SetDword(Lsa, "DisableDomainCreds", 1)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "DisableDomainCreds")],
                DetectOps = [RegOp.CheckDword(Lsa, "DisableDomainCreds", 1)],
            },
        ];
    }

    // ── CredentialDelegationPolicy ──
    private static class _CredentialDelegationPolicy
    {
        private const string CredDelKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "creddel-disable-remote-host-delegation",
                    Label = "Disable Credential Delegation to Remote Hosts",
                    Category = "User Account — Credential Delegation",
                    Description = "Prevents Windows from forwarding saved credentials to remote hosts via CredSSP delegation.",
                    Tags = ["credentials", "delegation", "credssp", "remote", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks CredSSP-based credential forwarding; may break PowerShell remoting that passes credentials to third hop.",
                    ApplyOps = [RegOp.SetDword(CredDelKey, "DelegateComputerName", 0)],
                    RemoveOps = [RegOp.DeleteValue(CredDelKey, "DelegateComputerName")],
                    DetectOps = [RegOp.CheckDword(CredDelKey, "DelegateComputerName", 0)],
                },
                new TweakDef
                {
                    Id = "creddel-allow-only-ntlm-protected",
                    Label = "Restrict CredSSP to NTLM-Protected Servers Only",
                    Category = "User Account — Credential Delegation",
                    Description = "Limits CredSSP fresh credential delegation to servers that authenticate via NTLM challenge-response.",
                    Tags = ["credentials", "delegation", "ntlm", "credssp", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Restricts delegation context; safe when servers use NTLMv2; may block delegation to Kerberos-only servers.",
                    ApplyOps = [RegOp.SetDword(CredDelKey, "AllowFreshCredentialsWhenNTLMOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredDelKey, "AllowFreshCredentialsWhenNTLMOnly")],
                    DetectOps = [RegOp.CheckDword(CredDelKey, "AllowFreshCredentialsWhenNTLMOnly", 1)],
                },
                new TweakDef
                {
                    Id = "creddel-deny-default-credential-delegation",
                    Label = "Deny Default Credential Delegation",
                    Category = "User Account — Credential Delegation",
                    Description = "Prevents the default Windows behaviour of delegating credentials to any server when CredSSP is negotiated.",
                    Tags = ["credentials", "delegation", "default", "credssp", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks unmanaged credential delegation; admins must explicitly whitelist trusted servers.",
                    ApplyOps = [RegOp.SetDword(CredDelKey, "DenyDefaultCredentials", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredDelKey, "DenyDefaultCredentials")],
                    DetectOps = [RegOp.CheckDword(CredDelKey, "DenyDefaultCredentials", 1)],
                },
                new TweakDef
                {
                    Id = "creddel-deny-saved-credential-delegation",
                    Label = "Deny Saved Credential Delegation",
                    Category = "User Account — Credential Delegation",
                    Description = "Prevents Windows Credential Manager saved credentials from being forwarded to remote servers via CredSSP.",
                    Tags = ["credentials", "delegation", "saved", "credential-manager", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Stops cached credential reuse in CredSSP sessions; users must manually authenticate.",
                    ApplyOps = [RegOp.SetDword(CredDelKey, "DenySavedCredentials", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredDelKey, "DenySavedCredentials")],
                    DetectOps = [RegOp.CheckDword(CredDelKey, "DenySavedCredentials", 1)],
                },
                new TweakDef
                {
                    Id = "creddel-deny-fresh-credential-delegation",
                    Label = "Deny Fresh Credential Delegation",
                    Category = "User Account — Credential Delegation",
                    Description = "Blocks CredSSP from forwarding freshly entered credentials to servers in all delegation categories.",
                    Tags = ["credentials", "delegation", "fresh", "credssp", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Prevents just-in-time credential forwarding; may break multi-hop PSRemoting workflows requiring CredSSP.",
                    ApplyOps = [RegOp.SetDword(CredDelKey, "DenyFreshCredentials", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredDelKey, "DenyFreshCredentials")],
                    DetectOps = [RegOp.CheckDword(CredDelKey, "DenyFreshCredentials", 1)],
                },
                new TweakDef
                {
                    Id = "creddel-require-remote-auth-mutual",
                    Label = "Require Mutual Authentication for Remote Sessions",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Mandates that remote session targets present valid Kerberos or certificate credentials before accepting connections.",
                    Tags = ["credentials", "delegation", "mutual-auth", "kerberos", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enforces server identity verification; prevents connection to spoofed or unauthenticated remote hosts.",
                    ApplyOps = [RegOp.SetDword(CredDelKey, "RequireMutualAuthentication", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredDelKey, "RequireMutualAuthentication")],
                    DetectOps = [RegOp.CheckDword(CredDelKey, "RequireMutualAuthentication", 1)],
                },
                new TweakDef
                {
                    Id = "creddel-disable-credssp-v1",
                    Label = "Disable CredSSP Protocol Version 1",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Disables CredSSP version 1 to enforce use of patched versions that mitigate credential forwarding vulnerabilities.",
                    Tags = ["credentials", "delegation", "credssp", "version", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Forces CredSSP v5+ which includes Oracle Remediation patches (CVE-2018-0886); old clients may fail.",
                    ApplyOps = [RegOp.SetDword(CredDelKey, "AllowEncryptionOracle", 0)],
                    RemoveOps = [RegOp.DeleteValue(CredDelKey, "AllowEncryptionOracle")],
                    DetectOps = [RegOp.CheckDword(CredDelKey, "AllowEncryptionOracle", 0)],
                },
                new TweakDef
                {
                    Id = "creddel-audit-delegation-events",
                    Label = "Enable Credential Delegation Audit Logging",
                    Category = "User Account — Credential Delegation",
                    Description = "Records credential delegation events in the Security event log for monitoring and forensic analysis.",
                    Tags = ["credentials", "delegation", "audit", "logging", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Delegation attempts are logged; useful for detecting credential theft or misconfigured delegation policies.",
                    ApplyOps = [RegOp.SetDword(CredDelKey, "EnableCredentialDelegationAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredDelKey, "EnableCredentialDelegationAudit")],
                    DetectOps = [RegOp.CheckDword(CredDelKey, "EnableCredentialDelegationAudit", 1)],
                },
                new TweakDef
                {
                    Id = "creddel-block-delegation-to-workgroups",
                    Label = "Block Credential Delegation to Workgroup Machines",
                    Category = "User Account — Credential Delegation",
                    Description = "Prevents credentials from being delegated to non-domain-joined (workgroup) computers to reduce attack surface.",
                    Tags = ["credentials", "delegation", "workgroup", "domain", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Domain credentials cannot be forwarded to workgroup machines; reduces lateral movement risk.",
                    ApplyOps = [RegOp.SetDword(CredDelKey, "BlockDelegationToWorkgroupComputers", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredDelKey, "BlockDelegationToWorkgroupComputers")],
                    DetectOps = [RegOp.CheckDword(CredDelKey, "BlockDelegationToWorkgroupComputers", 1)],
                },
            ];
    }

    // ── CredentialManagerPolicy ──
    private static class _CredentialManagerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "credmgr-restrict-default-credentials-delegation",
                Label = "Restrict Default Credential Delegation to Specific Servers",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Default credential delegation (Allow Delegating Default Credentials) controls whether Windows automatically delegates user credentials to remote systems when services request them. Restricting default credential delegation to specific trusted servers prevents automatic credential forwarding to untrusted or attacker-controlled systems. Unrestricted credential delegation sends user credentials to any server that requests Kerberos delegation creating a pass-the-hash risk for any compromised server in the path. The delegation allowlist should contain only specific server names or use DNS suffixes for controlled environments not wildcard entries that allow all servers. Credential delegation restrictions prevent watering-hole style attacks where a server is compromised specifically to capture delegated credentials from users who connect to it. Organizations using RDP remote administration should configure specific server name allowlists for credential delegation instead of the insecure default wildcard configuration.",
                Tags = ["credentials", "delegation", "rdp", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowDefaultCredentials", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowDefaultCredentials")],
                DetectOps = [RegOp.CheckDword(Key, "AllowDefaultCredentials", 0)],
            },
            new TweakDef
            {
                Id = "credmgr-restrict-fresh-credentials",
                Label = "Restrict Fresh Credential Delegation to Trusted Servers Only",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Fresh credential delegation controls whether newly entered credentials are forwarded to remote servers as part of the authentication process. Restricting fresh credential delegation prevents forwarding of interactive user credentials to servers outside the trusted delegation list. Fresh credentials include passwords entered in Windows Security prompts and other interactive authentication dialogs that should not be transmitted to untrusted systems. Delegation restrictions for fresh credentials are particularly important for privileged users who enter elevated credentials that could be captured by a compromised remote system. The CredSSP protocol used for fresh credential delegation was vulnerable to a man-in-the-middle attack (CVE-2018-0886) before patches were applied making restriction important. Organizations should configure fresh credential delegation lists to include only servers that absolutely require credential forwarding to reduce exposure.",
                Tags = ["credentials", "fresh-credentials", "credssp", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowFreshCredentials", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowFreshCredentials")],
                DetectOps = [RegOp.CheckDword(Key, "AllowFreshCredentials", 0)],
            },
            new TweakDef
            {
                Id = "credmgr-restrict-saved-rdp-credentials",
                Label = "Prevent Saving of Remote Desktop Connection Credentials",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Saved Remote Desktop credentials stored in Windows Credential Manager can be extracted by attackers with local access providing access to remote systems without knowing the user's password. Preventing saved RDP credentials reduces the credential material available to attackers who compromise a client system. Saved credentials are particularly risky on shared or kiosk systems where multiple users may access the same device. The Windows Credential Manager store can be dumped by tools like Mimikatz if the attacker has local administrator access making prevention more impactful than protection. Organizations with privileged access workstations (PAWs) should disable saved credentials to ensure administrators must enter passwords each session reinforcing awareness of credential use. Preventing saved credentials may require users to re-authenticate for each connection which is an acceptable security trade-off for privileged users.",
                Tags = ["credentials", "rdp", "saved-passwords", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisallowSavedCredentials", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisallowSavedCredentials")],
                DetectOps = [RegOp.CheckDword(Key, "DisallowSavedCredentials", 1)],
            },
            new TweakDef
            {
                Id = "credmgr-disable-wdigest-authentication",
                Label = "Disable WDigest Authentication to Prevent Cleartext Password Storage in Memory",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "WDigest authentication stores user credentials in cleartext in LSASS memory on Windows systems making them accessible to credential dumping tools like Mimikatz. Disabling WDigest authentication prevents cleartext passwords from being stored in memory significantly limiting the impact of credential dumping attacks. WDigest was designed for authentication against HTTP Digest services but is rarely required in modern environments that use NTLM or Kerberos. The UseLogonCredential registry value controls whether WDigest stores cleartext credentials in LSASS and should be set to 0 on all domain-joined systems. Microsoft patched WDigest behavior in KB2871997 and WDigest is disabled by default on Windows 8.1 and Windows Server 2012 R2 but must be explicitly disabled on older versions. Organizations should verify WDigest is disabled across their entire fleet as it is commonly re-enabled by attackers who first compromise a system and want to wait for credentials.",
                Tags = ["credentials", "wdigest", "lsass", "cleartext", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "UseLogonCredential", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "UseLogonCredential")],
                DetectOps = [RegOp.CheckDword(Key, "UseLogonCredential", 0)],
            },
            new TweakDef
            {
                Id = "credmgr-enable-lsass-process-protection",
                Label = "Enable LSASS Process Protection to Prevent Credential Dumping",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "LSASS (Local Security Authority Subsystem Service) process protection uses Windows Protected Process Light to prevent user-mode process injection and memory reading by credential dumping tools. Enabling LSASS protection requires that all LSASS plugins including authentication providers be digitally signed by Microsoft preventing Mimikatz-style injection. Protected LSASS with PPL (Protected Process Light) makes it significantly harder for attackers to extract credential hashes from LSASS memory. LSASS process protection requires Credential Guard or the RunAsPPL registry value to be configured as Protected Process Light. Third-party security software that hooks LSASS for monitoring may break when LSASS protection is enabled requiring vendor-specific signed drivers. Organizations should test LSASS protection in their environment before broad deployment as incompatible software will cause authentication failures.",
                Tags = ["credentials", "lsass", "process-protection", "credential-dumping", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RunAsPPL", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RunAsPPL")],
                DetectOps = [RegOp.CheckDword(Key, "RunAsPPL", 1)],
            },
            new TweakDef
            {
                Id = "credmgr-restrict-credential-manager-api-access",
                Label = "Restrict API Access to Windows Credential Manager Store",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Windows Credential Manager API access controls which applications can read stored generic credentials from the Windows Credential Manager vault. Restricting Credential Manager API access prevents unauthorized applications from reading network credentials and application passwords stored in the credential vault. The Windows Credential Manager can store passwords for websites, servers, and applications which are accessible to applications running as the current user. Malware that runs as the user can enumerate and exfiltrate credentials from Credential Manager without elevated privileges making API restriction an important defense. Credential Manager access restrictions should be complemented by avoiding storing sensitive passwords in Credential Manager on shared or potentially compromised systems. Security audits should check Credential Manager contents on privileged workstations to ensure sensitive administrative credentials are not stored in plain access.",
                Tags = ["credentials", "credential-manager", "api-access", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictCredentialManagerAPI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictCredentialManagerAPI")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictCredentialManagerAPI", 1)],
            },
            new TweakDef
            {
                Id = "credmgr-enable-remote-host-credential-guard",
                Label = "Enable Remote Credential Guard for Protected Credential Delegation",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Remote Credential Guard redirects Kerberos requests back to the requesting device so that the user's credentials are never actually sent to the remote host preventing credential exposure. Enabling Remote Credential Guard for RDP connections ensures that even if the remote server is compromised the attacker cannot steal credentials from it. Remote Credential Guard differs from Restricted Admin Mode in that the full user token remains on the local machine making network resource access work correctly from the remote session. Remote Credential Guard requires Windows 10 1607 or later on both client and server and the user must have Kerberos authentication to the target server. Organizations should prefer Remote Credential Guard over Restricted Admin Mode when possible as it provides stronger security with better compatibility for network resource access. Remote Credential Guard cannot be used with Network Level Authentication for Remote Desktop Gateway connections.",
                Tags = ["credentials", "remote-credential-guard", "rdp", "kerberos", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableRemoteCredentialGuard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRemoteCredentialGuard")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRemoteCredentialGuard", 1)],
            },
            new TweakDef
            {
                Id = "credmgr-block-ntlm-credential-delegation",
                Label = "Block NTLM Credential Delegation in Restricted Networks",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "NTLM credential delegation passes user NTLM credentials to remote systems where they can be captured and used for pass-the-hash attacks against other systems. Blocking NTLM credential delegation prevents NTLM hashes from being forwarded to servers that could be compromised and use them for lateral movement. NTLM is a challenge-response authentication protocol where the hash sent to authenticate can be immediately reused without knowing the original password. Organizations should be moving toward Kerberos authentication everywhere to eliminate NTLM delegation risks but restrictions help harden transitional environments. NTLM relay attacks are a persistent threat where an attacker on the network captures and relays NTLM credentials to authenticate to other services. Blocking NTLM delegation combined with SMB signing and NTLM auditing provides comprehensive protection against NTLM-based attacks.",
                Tags = ["credentials", "ntlm", "delegation", "pass-the-hash", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockNTLMDelegation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockNTLMDelegation")],
                DetectOps = [RegOp.CheckDword(Key, "BlockNTLMDelegation", 1)],
            },
        ];
    }

    // ── CredentialRoamingPolicy ──
    private static class _CredentialRoamingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Winlogon";
        private const string RoamKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Winlogon\RoamingProfile";
        private const string CertKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\RoamingProfile";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "credroam-disable-credential-roaming",
                    Label = "Disable User Credential Roaming Between Domain Computers",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Prevents user credentials (certificates, private keys, smart card PINs) from being copied to the user's roaming profile and thus synchronised to other domain computers, keeping credentials machine-local and reducing the credential surface exposed if a profile is compromised.",
                    Tags = ["credential-roaming", "certificates", "private-keys", "roaming-profile", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Credential roaming disabled; private keys and certificates stay machine-local, not synced via roaming profile.",
                    ApplyOps = [RegOp.SetDword(Key, "SyncForegroundPolicy", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SyncForegroundPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "SyncForegroundPolicy", 0)],
                },
                new TweakDef
                {
                    Id = "credroam-block-certificate-roaming",
                    Label = "Block Roaming of User Certificates via User Profile",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Specifically blocks the roaming of user certificates and key containers via the Windows credential roaming feature, preventing certificates imported on one machine from appearing on all machines on next logon.",
                    Tags = ["credential-roaming", "certificates", "key-container", "profile-sync", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Certificate roaming via user profile blocked; imported certs and keys remain on the issuing machine only.",
                    ApplyOps = [RegOp.SetDword(CertKey, "DisableCertificateRoaming", 1)],
                    RemoveOps = [RegOp.DeleteValue(CertKey, "DisableCertificateRoaming")],
                    DetectOps = [RegOp.CheckDword(CertKey, "DisableCertificateRoaming", 1)],
                },
                new TweakDef
                {
                    Id = "credroam-restrict-profile-sync-to-domain",
                    Label = "Restrict Roaming Profile Sync to Domain Networks Only",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Prevents roaming profile synchronisation from occurring over non-domain networks (public WiFi, VPN), ensuring credential and profile data is only synced when connected to the corporate domain network.",
                    Tags = ["roaming-profile", "domain-network", "profile-sync", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Roaming profile sync restricted to domain networks; profile data not synced over public networks or VPN.",
                    ApplyOps = [RegOp.SetDword(RoamKey, "SlowLinkTimeOut", 500)],
                    RemoveOps = [RegOp.DeleteValue(RoamKey, "SlowLinkTimeOut")],
                    DetectOps = [RegOp.CheckDword(RoamKey, "SlowLinkTimeOut", 500)],
                },
                new TweakDef
                {
                    Id = "credroam-delete-cached-roaming-profiles",
                    Label = "Delete Cached Copies of Roaming Profiles at Logoff",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Configures Windows to delete the locally cached copy of the roaming profile when the user logs off, ensuring credential data and profile contents are not left on shared or non-primary workstations after user sessions.",
                    Tags = ["roaming-profile", "cached-profile", "logoff", "data-cleanup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Cached roaming profile deleted at logoff; credential data not retained on non-primary workstations.",
                    ApplyOps = [RegOp.SetDword(Key, "DeleteRoamingCache", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DeleteRoamingCache")],
                    DetectOps = [RegOp.CheckDword(Key, "DeleteRoamingCache", 1)],
                },
                new TweakDef
                {
                    Id = "credroam-disable-smart-card-pin-roaming",
                    Label = "Disable Smart Card PIN Roaming via Credential Roaming Service",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Prevents smart card PINs cached by the Windows Smart Card PIN cache from being synchronised between machines via the credential roaming service, keeping smart card PIN caches strictly machine-local.",
                    Tags = ["credroam", "smart-card", "pin-cache", "roaming", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Smart card PIN roaming disabled; cached PINs remain machine-local only.",
                    ApplyOps = [RegOp.SetDword(CertKey, "DisableSmartCardPINRoaming", 1)],
                    RemoveOps = [RegOp.DeleteValue(CertKey, "DisableSmartCardPINRoaming")],
                    DetectOps = [RegOp.CheckDword(CertKey, "DisableSmartCardPINRoaming", 1)],
                },
                new TweakDef
                {
                    Id = "credroam-require-admin-roaming-profile",
                    Label = "Block Administrator Accounts from Using Roaming Profiles",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Prevents administrator accounts from using roaming profiles, ensuring that elevated account credentials, SAM keys, and administrative certificates are never synchronised to roaming profile storage.",
                    Tags = ["credroam", "admin-account", "roaming-profile", "privilege", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Admin accounts blocked from roaming profiles; elevated credential data never synced via profile infrastructure.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAdminRoamingProfile", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAdminRoamingProfile")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAdminRoamingProfile", 1)],
                },
                new TweakDef
                {
                    Id = "credroam-encrypt-roaming-profile-at-rest",
                    Label = "Encrypt Roaming Profile Server-Side Copy at Rest",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Requires the roaming profile share to encrypt profile data server-side before writing to the UNC profile path, ensuring that the server-side copy of the roaming profile is EFS-protected and not readable by share administrators.",
                    Tags = ["credroam", "efs", "encryption", "profile-server", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Roaming profile server-side copy EFS-encrypted; share admins cannot read profile credential data at rest.",
                    ApplyOps = [RegOp.SetDword(RoamKey, "EncryptProfileData", 1)],
                    RemoveOps = [RegOp.DeleteValue(RoamKey, "EncryptProfileData")],
                    DetectOps = [RegOp.CheckDword(RoamKey, "EncryptProfileData", 1)],
                },
                new TweakDef
                {
                    Id = "credroam-log-profile-sync-events",
                    Label = "Log Roaming Profile Synchronisation Events in System Log",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Enables System event log entries for all roaming profile synchronisation operations, including sync success, failure, conflict, and truncation events, providing audit visibility into profile and credential roaming activity.",
                    Tags = ["credroam", "event-log", "audit", "profile-sync", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Roaming profile sync events logged in System log; credential roaming activity visible for auditing.",
                    ApplyOps = [RegOp.SetDword(Key, "LogProfileSyncEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogProfileSyncEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "LogProfileSyncEvents", 1)],
                },
                new TweakDef
                {
                    Id = "credroam-block-plaintext-credential-cache",
                    Label = "Block Caching of Plaintext Credentials in Roaming Profile",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Prevents the credentials manager and credential providers from storing reversible (plaintext-equivalent) credential blobs in the user's roaming profile, ensuring only hashed or certificate-protected credentials are ever written to profile storage.",
                    Tags = ["credroam", "plaintext-credential", "credential-cache", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Plaintext credential caching in roaming profile blocked; only hashed/cert-protected credentials in profile.",
                    ApplyOps = [RegOp.SetDword(CertKey, "BlockPlaintextCredentialCache", 1)],
                    RemoveOps = [RegOp.DeleteValue(CertKey, "BlockPlaintextCredentialCache")],
                    DetectOps = [RegOp.CheckDword(CertKey, "BlockPlaintextCredentialCache", 1)],
                },
                new TweakDef
                {
                    Id = "credroam-disable-credential-roaming-telemetry",
                    Label = "Disable Credential Roaming Telemetry to Microsoft",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Prevents the Windows credential roaming service from sending certificate sync counts, roaming failures, and credential manager sync statistics to Microsoft.",
                    Tags = ["credroam", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Credential roaming telemetry to Microsoft disabled; cert sync stats and roaming activity not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(CertKey, "DisableCredentialRoamingTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(CertKey, "DisableCredentialRoamingTelemetry")],
                    DetectOps = [RegOp.CheckDword(CertKey, "DisableCredentialRoamingTelemetry", 1)],
                },
            ];
    }

    // ── CredentialUiPolicy ──
    private static class _CredentialUiPolicy
    {
        private const string CredUi = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI";
        private const string CredUiCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CredUI";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "credui-disable-administrator-enumeration",
                Label = "Disable Administrator Account Enumeration in Credential UI",
                Category = "User Account — Credential Delegation",
                Description =
                    "Prevents the credential prompt from enumerating or listing administrator accounts, reducing account information leakage.",
                Tags = ["credential", "security", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CredUi, "EnumerateAdministrators", 0)],
                RemoveOps = [RegOp.DeleteValue(CredUi, "EnumerateAdministrators")],
                DetectOps = [RegOp.CheckDword(CredUi, "EnumerateAdministrators", 0)],
            },
            new TweakDef
            {
                Id = "credui-no-local-password-reset-questions",
                Label = "Disable Local Account Password Reset Security Questions",
                Category = "User Account — Credential Delegation",
                Description =
                    "Prevents setup and use of security questions for local account password resets, requiring admin intervention for locked-out accounts.",
                Tags = ["credential", "security", "group-policy", "hardening", "password"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CredUi, "NoLocalPasswordResetQuestions", 1)],
                RemoveOps = [RegOp.DeleteValue(CredUi, "NoLocalPasswordResetQuestions")],
                DetectOps = [RegOp.CheckDword(CredUi, "NoLocalPasswordResetQuestions", 1)],
            },
            new TweakDef
            {
                Id = "credui-enable-secure-credential-prompting",
                Label = "Require Secure Desktop for Credential UI Prompts",
                Category = "User Account — Credential Delegation",
                Description =
                    "Forces credential dialogs to appear on the secure desktop, preventing malicious programs from intercepting or spoofing credential prompts.",
                Tags = ["credential", "security", "group-policy", "hardening", "uac"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CredUi, "EnableSecureCredentialPrompting", 1)],
                RemoveOps = [RegOp.DeleteValue(CredUi, "EnableSecureCredentialPrompting")],
                DetectOps = [RegOp.CheckDword(CredUi, "EnableSecureCredentialPrompting", 1)],
            },
            new TweakDef
            {
                Id = "credui-disable-visual-prompt",
                Label = "Disable Credential UI Visual Prompt Animation",
                Category = "User Account — Credential Delegation",
                Description =
                    "Suppresses the animated shimmer/glow visual prompt in the credential UI, reducing distraction in kiosk and focused-work environments.",
                Tags = ["credential", "ui", "group-policy", "kiosk"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CredUi, "NoVisualPrompt", 1)],
                RemoveOps = [RegOp.DeleteValue(CredUi, "NoVisualPrompt")],
                DetectOps = [RegOp.CheckDword(CredUi, "NoVisualPrompt", 1)],
            },
            new TweakDef
            {
                Id = "credui-disable-save-credentials",
                Label = "Disable Save Credentials for RDP",
                Category = "User Account — Credential Delegation",
                Description = "Prevents the OS from saving RDP credentials in the Windows Credential Manager, requiring re-entry on each connection.",
                Tags = ["credential", "security", "group-policy", "rdp", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CredUi, "DisableSaveCredentials", 1)],
                RemoveOps = [RegOp.DeleteValue(CredUi, "DisableSaveCredentials")],
                DetectOps = [RegOp.CheckDword(CredUi, "DisableSaveCredentials", 1)],
            },
            new TweakDef
            {
                Id = "credui-disable-windows-hello-pinlogin",
                Label = "Disable Windows Hello PIN Login from Credential UI",
                Category = "User Account — Credential Delegation",
                Description =
                    "Prevents PIN authentication from appearing as an option in network credential prompts, enforcing password-only authentication.",
                Tags = ["credential", "security", "group-policy", "windows-hello", "pin"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CredUi, "DisableWindowsHelloPINLogin", 1)],
                RemoveOps = [RegOp.DeleteValue(CredUi, "DisableWindowsHelloPINLogin")],
                DetectOps = [RegOp.CheckDword(CredUi, "DisableWindowsHelloPINLogin", 1)],
            },
            new TweakDef
            {
                Id = "credui-disable-user-password-reveal-cu",
                Label = "Disable Password Reveal (User Policy)",
                Category = "User Account — Credential Delegation",
                Description =
                    "Applies the disable-password-reveal rule at the current-user scope, ensuring the eye icon is hidden even without machine admin rights.",
                Tags = ["credential", "security", "group-policy", "password"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CredUiCu, "DisablePasswordReveal", 1)],
                RemoveOps = [RegOp.DeleteValue(CredUiCu, "DisablePasswordReveal")],
                DetectOps = [RegOp.CheckDword(CredUiCu, "DisablePasswordReveal", 1)],
            },
            new TweakDef
            {
                Id = "credui-block-generic-credential-caching",
                Label = "Block Generic Network Credential Caching",
                Category = "User Account — Credential Delegation",
                Description = "Prevents Windows from caching plaintext generic network credentials in the Credential Manager store.",
                Tags = ["credential", "security", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CredUi, "CacheLogonCredentials", 0)],
                RemoveOps = [RegOp.DeleteValue(CredUi, "CacheLogonCredentials")],
                DetectOps = [RegOp.CheckDword(CredUi, "CacheLogonCredentials", 0)],
            },
            new TweakDef
            {
                Id = "credui-disable-autofill-on-credential-forms",
                Label = "Disable Auto-Fill on Credential Input Forms",
                Category = "User Account — Credential Delegation",
                Description = "Prevents the credential UI from auto-filling remembered usernames and passwords on domain credential dialogs.",
                Tags = ["credential", "security", "group-policy", "password"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CredUi, "DisableAutofill", 1)],
                RemoveOps = [RegOp.DeleteValue(CredUi, "DisableAutofill")],
                DetectOps = [RegOp.CheckDword(CredUi, "DisableAutofill", 1)],
            },
        ];
    }

    // ── EntraDeviceRegistrationPolicy ──
    private static class _EntraDeviceRegistrationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "entrareg-disable-auto-registration",
                    Label = "Disable Automatic Device Registration with Entra ID",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Disables automatic Entra ID (Azure AD) device registration triggered by domain join, preventing unintended hybrid join of machines that should remain unregistered.",
                    Tags = ["entra", "device-registration", "azure-ad", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Automatic device registration to Entra disabled; manual registration still possible.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "entrareg-require-mdm-enrollment",
                    Label = "Require MDM Enrollment for Device Registration",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Requires MDM (Microsoft Intune or third-party MDM) enrollment as a prerequisite for completing Entra ID device registration, ensuring all registered devices are also managed.",
                    Tags = ["entra", "mdm", "enrollment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Device registration requires MDM enrollment; unmanaged devices cannot fully register with Entra.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireMDMEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireMDMEnrollment")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireMDMEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "entrareg-disable-auto-mdm-enroll",
                    Label = "Disable Automatic MDM Auto-Enrollment",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Disables automatic MDM auto-enrollment that triggers when a device joins Entra ID, giving IT control over which devices are enrolled in mobile device management.",
                    Tags = ["entra", "mdm", "auto-enrollment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "MDM auto-enrollment disabled; devices must be enrolled manually or via provisioning package.",
                    ApplyOps = [RegOp.SetDword(Key, "AutoEnrollMDM", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutoEnrollMDM")],
                    DetectOps = [RegOp.CheckDword(Key, "AutoEnrollMDM", 0)],
                },
                new TweakDef
                {
                    Id = "entrareg-block-ngc-key-reset",
                    Label = "Block NGC Key Reset During Device Registration",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Blocks Next Generation Credentials (NGC) key reset operations during device registration events, preventing credential rotation that could lock users out after re-registration.",
                    Tags = ["entra", "ngc", "key", "credentials", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "NGC keys not reset on re-registration; PIN/biometric credentials preserved through device re-join.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockNGCKeyReset", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockNGCKeyReset")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockNGCKeyReset", 1)],
                },
                new TweakDef
                {
                    Id = "entrareg-disable-user-consent-registration",
                    Label = "Disable User-Initiated Device Registration",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Disables the ability for standard users to self-initiate Entra ID device registration via the Settings > Accounts > Work or School Account page.",
                    Tags = ["entra", "device-registration", "user-consent", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot register the device with Entra; admin or MDM provisioning required.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableUserMDMEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableUserMDMEnrollment")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableUserMDMEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "entrareg-enforce-hybrid-join-cert",
                    Label = "Enforce Certificate Validation in Hybrid Join",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Enforces certificate validation during hybrid Entra ID join, ensuring the device identity certificate issued during hybrid join is verified against the enterprise Root CA.",
                    Tags = ["entra", "hybrid-join", "certificate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Hybrid join certificate validated; rogue certificates rejected during device registration.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceHybridJoinCertificate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceHybridJoinCertificate")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceHybridJoinCertificate", 1)],
                },
                new TweakDef
                {
                    Id = "entrareg-block-stale-device-tokens",
                    Label = "Block Stale Device Token Use",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Blocks the use of stale or revoked Primary Refresh Tokens (PRT) from deregistered devices, preventing old device registrations from accessing cloud resources.",
                    Tags = ["entra", "prt", "stale-tokens", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Stale PRTs rejected; deregistered devices cannot access cloud apps with old tokens.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockStaleDeviceTokens", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockStaleDeviceTokens")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockStaleDeviceTokens", 1)],
                },
                new TweakDef
                {
                    Id = "entrareg-set-prt-lifetime",
                    Label = "Set Primary Refresh Token Lifetime to 14 Days",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Limits the lifetime of Primary Refresh Tokens (PRT) to 14 days, requiring devices to re-authenticate with Entra ID every two weeks and reducing the window for stolen PRT abuse.",
                    Tags = ["entra", "prt", "lifetime", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "PRTs expire in 14 days; devices offline longer than 14 days must re-authenticate to Entra.",
                    ApplyOps = [RegOp.SetDword(Key2, "MaxDeviceTokenLifetimeDays", 14)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "MaxDeviceTokenLifetimeDays")],
                    DetectOps = [RegOp.CheckDword(Key2, "MaxDeviceTokenLifetimeDays", 14)],
                },
                new TweakDef
                {
                    Id = "entrareg-disable-self-service-bjoin",
                    Label = "Disable Self-Service BYOD Registration in Settings",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Prevents BYOD users from adding personal devices to the organisation by disabling the Add Work or School Account option for non-admin users.",
                    Tags = ["entra", "byod", "self-service", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "BYOD self-service registration blocked; personal devices cannot join the tenant without IT approval.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableSelfServiceByodRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableSelfServiceByodRegistration")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableSelfServiceByodRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "entrareg-require-compliant-device-for-wu",
                    Label = "Require Entra-Compliant Device for Windows Update",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Requires the device to maintain a valid Entra ID compliance status (via Intune Compliance Policy) to receive Windows Update policy configurations from the cloud.",
                    Tags = ["entra", "windows-update", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Non-compliant devices use default WU settings; only compliant devices get enterprise WU ring config.",
                    ApplyOps = [RegOp.SetDword(Key2, "RequireEntraCompliantForUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RequireEntraCompliantForUpdate")],
                    DetectOps = [RegOp.CheckDword(Key2, "RequireEntraCompliantForUpdate", 1)],
                },
            ];
    }

    // ── KerberoastMitigationPolicy ──
    private static class _KerberoastMitigationPolicy
    {
        private const string KerbKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";
        private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "kerbmit-tighten-clock-skew",
                    Label = "Tighten Kerberos Clock Skew Tolerance (2 min)",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets MaxClockSkew=2 in Kerberos Parameters. Reduces the tolerated clock difference between the client and the KDC from the default 5 minutes to 2 minutes. Kerberos uses timestamps as a replay-protection mechanism; a tighter skew window shrinks the replay attack window. It also limits the usability of pre-computed Kerberos tickets that rely on timestamp tolerance.",
                    Tags = ["kerberos", "clock-skew", "replay", "timestamp", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Tighter clock skew; ensure NTP is well-configured or clients with drifted clocks will fail Kerberos auth.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "MaxClockSkew", 2)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "MaxClockSkew")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "MaxClockSkew", 2)],
                },
                new TweakDef
                {
                    Id = "kerbmit-enable-pac-validation",
                    Label = "Enable KDC PAC Signature Validation",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets ValidateKdcPacSignature=1 in the LSA key. Instructs Windows services to validate the KDC Privilege Attribute Certificate (PAC) server signature embedded in Kerberos service tickets. Without validation, a compromised or modified PAC (as exploited by MS14-068) can be used to forge group memberships and escalate privileges. This is the KDC PAC defence against the MS14-068 Kerberos privilege escalation vulnerability.",
                    Tags = ["kerberos", "pac", "signature", "ms14-068", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Validates PAC signatures; negligible performance impact; critical for defence against forged PAC attacks.",
                    ApplyOps = [RegOp.SetDword(LsaKey, "ValidateKdcPacSignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(LsaKey, "ValidateKdcPacSignature")],
                    DetectOps = [RegOp.CheckDword(LsaKey, "ValidateKdcPacSignature", 1)],
                },
                new TweakDef
                {
                    Id = "kerbmit-restrict-unconstrained-delegation",
                    Label = "Block Kerberos Unconstrained Delegation by Default",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets RestrictReceivingNTLMTraffic=2 in Kerberos Parameters. Restricts services from accepting unconstrained Kerberos delegation tokens by default. Unconstrained delegation allows a compromised service to impersonate any user to any other service — it is the primary mechanism exploited in Golden Ticket and delegation-based lateral movement attacks. Setting RestrictReceivingNTLMTraffic also limits NTLM passthrough that accompanies delegation abuse.",
                    Tags = ["kerberos", "delegation", "unconstrained", "lateral-movement", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Blocks unconstrained delegation; services relying on TrustedForDelegation must be audited before applying.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "RestrictReceivingNTLMTraffic", 2)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "RestrictReceivingNTLMTraffic")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "RestrictReceivingNTLMTraffic", 2)],
                },
                new TweakDef
                {
                    Id = "kerbmit-enable-armoring",
                    Label = "Enable FAST Kerberos Armoring (Tunnel Mode)",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets KdcArmoring=1 in Kerberos Parameters. Enables Kerberos Flexible Authentication via Secure Tunneling (FAST, RFC 6113) which wraps Kerberos authentication messages in an encrypted tunnel. FAST armoring prevents eavesdropping on pre-authentication data (AS-REQ) that would otherwise expose user principal names and enable AS-REP Roasting attacks against accounts without Kerberos pre-authentication required.",
                    Tags = ["kerberos", "fast", "armoring", "as-rep-roasting", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Enables FAST armoring; requires Windows 8+ domain joined clients; older clients fall back to unarmored auth.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "KdcArmoring", 1)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "KdcArmoring")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "KdcArmoring", 1)],
                },
                new TweakDef
                {
                    Id = "kerbmit-disable-msskip-delegation",
                    Label = "Block NTLM Delegation to All Servers",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets AllowNTLMSessionSecurity=0 in Kerberos Parameters. Prevents Kerberos from falling back to NTLM session security for delegation, closing a common path by which attackers convert Kerberos delegation abuse into NTLM-based lateral movement. Forcing pure Kerberos delegation eliminates the NTLM relay component of many sophisticated delegation attacks.",
                    Tags = ["kerberos", "ntlm", "delegation", "session-security", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Blocks NTLM session delegation fallback; ensure domain controllers and services support Kerberos delegation exclusively.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "AllowNTLMSessionSecurity", 0)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "AllowNTLMSessionSecurity")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "AllowNTLMSessionSecurity", 0)],
                },
                new TweakDef
                {
                    Id = "kerbmit-enforce-preauth-required",
                    Label = "Enforce Kerberos Pre-Authentication Requirement",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets ClientRequireStrictKDCValidation=1 in Kerberos Parameters. Instructs Kerberos clients to enforce strict KDC validation requirements including pre-authentication enforcement. Accounts with 'Do not require Kerberos preauthentication' (DONT_REQUIRE_PREAUTH) are trivially AS-REP Roastable — an attacker can request their encrypted TGT reply without knowing their password. This policy ensures the client enforces pre-auth at the KDC level.",
                    Tags = ["kerberos", "preauthentication", "as-rep-roasting", "kdc", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Requires strict KDC validation; service accounts with DONT_REQUIRE_PREAUTH flag set must have it removed first.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "ClientRequireStrictKDCValidation", 1)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "ClientRequireStrictKDCValidation")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "ClientRequireStrictKDCValidation", 1)],
                },
            ];
    }

    // ── KerberosAdvanced ──
    private static class _KerberosAdvanced
    {
        private const string KrbLsaParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";
        private const string KrbPolicyParams = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "krb-require-preauth",
                Label = "Kerberos: Require Pre-Authentication for All Accounts",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 4,
                SafetyRating = 5,
                RegistryKeys = [KrbPolicyParams],
                Tags = ["kerberos", "authentication", "pre-auth", "as-rep-roasting", "security"],
                Description =
                    "Sets AllowForestTrustsForKerberos=0 in Kerberos policy. "
                    + "When combined with per-account pre-auth requirements, this prevents AS-REP Roasting: "
                    + "attackers cannot request a TGT for accounts without supplying a valid password hash. "
                    + "All modern Active Directory accounts should have pre-auth enabled (it is on by default).",
                ApplyOps = [RegOp.SetDword(KrbPolicyParams, "AllowForestTrustsForKerberos", 0)],
                RemoveOps = [RegOp.DeleteValue(KrbPolicyParams, "AllowForestTrustsForKerberos")],
                DetectOps = [RegOp.CheckDword(KrbPolicyParams, "AllowForestTrustsForKerberos", 0)],
            },
            new TweakDef
            {
                Id = "krb-enable-cbac",
                Label = "Kerberos: Enable Claims-Based Access Control (CBAC)",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [KrbPolicyParams],
                Tags = ["kerberos", "authentication", "cbac", "dynamic-access-control", "security", "active-directory"],
                Description =
                    "Sets EnableCbacAndArmor=1 in Kerberos policy. "
                    + "Enables Dynamic Access Control (DAC) claims and Kerberos armoring (FAST). "
                    + "Claims-based authentication allows AD to include user/device attributes in Kerberos "
                    + "tickets for richer conditional access policies. FAST provides channel-binding "
                    + "protection against PKINIT spoofing.",
                ApplyOps = [RegOp.SetDword(KrbPolicyParams, "EnableCbacAndArmor", 1)],
                RemoveOps = [RegOp.DeleteValue(KrbPolicyParams, "EnableCbacAndArmor")],
                DetectOps = [RegOp.CheckDword(KrbPolicyParams, "EnableCbacAndArmor", 1)],
            },
            new TweakDef
            {
                Id = "krb-log-authentication-events",
                Label = "Kerberos: Enable Verbose Authentication Logging",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 5,
                RegistryKeys = [KrbLsaParams],
                Tags = ["kerberos", "authentication", "audit", "logging", "security"],
                Description =
                    "Sets LogLevel=1 in Kerberos\\Parameters. "
                    + "Enables verbose Kerberos diagnostic logging to the System event log. "
                    + "Logs KDC errors, ticket failures, and encryption type negotiations. "
                    + "Very useful for diagnosing Kerberos failures but generates moderate event log volume.",
                ApplyOps = [RegOp.SetDword(KrbLsaParams, "LogLevel", 1)],
                RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "LogLevel")],
                DetectOps = [RegOp.CheckDword(KrbLsaParams, "LogLevel", 1)],
            },
            new TweakDef
            {
                Id = "krb-purge-ticket-cache-on-logoff",
                Label = "Kerberos: Purge Kerberos Ticket Cache on User Logoff",
                Category = "User Account — Credential Delegation",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [KrbLsaParams],
                Tags = ["kerberos", "authentication", "ticket-cache", "logoff", "security", "credential-hygiene"],
                Description =
                    "Sets PurgeTicketCacheOnLogoff=1 in Kerberos\\Parameters. "
                    + "Forces the Kerberos subsystem to clear all cached TGTs and service tickets when a "
                    + "user logs off. Prevents cached tickets from being recovered from a hibernation "
                    + "file or memory dump after the session ends.",
                ApplyOps = [RegOp.SetDword(KrbLsaParams, "PurgeTicketCacheOnLogoff", 1)],
                RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "PurgeTicketCacheOnLogoff")],
                DetectOps = [RegOp.CheckDword(KrbLsaParams, "PurgeTicketCacheOnLogoff", 1)],
            },
        ];
    }

    // ── KerberosArmoringPolicy ──
    private static class _KerberosArmoringPolicy
    {
        private const string KdcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System\KDC";
        private const string KrbKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System\Kerberos";
        private const string KrbSvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Kerberos\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "krbadv-enable-kdc-armoring",
                    Label = "Enable Kerberos Armoring (FAST) on KDC",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Enables Flexible Authentication Secure Tunneling (FAST / Kerberos armoring) on the KDC. FAST wraps KDC requests in an armored tunnel, protecting pre-authentication data from offline attacks and downgrade attempts.",
                    Tags = ["kerberos", "fast", "armoring", "kdc", "pre-authentication"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "FAST protects Kerberos pre-auth from AS-REP roasting and offline cracking. Requires compatible DCs and clients (Windows 8.1+ / Server 2012 R2+).",
                    RegistryKeys = [KdcKey],
                    ApplyOps = [RegOp.SetDword(KdcKey, "EnableKDCArmoring", 1)],
                    RemoveOps = [RegOp.DeleteValue(KdcKey, "EnableKDCArmoring")],
                    DetectOps = [RegOp.CheckDword(KdcKey, "EnableKDCArmoring", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-require-client-armoring",
                    Label = "Require Kerberos Armoring for Client Authentication",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Forces Kerberos clients to use FAST armoring when requesting tickets from the KDC. Clients that do not support FAST will be denied authentication, ensuring all ticket exchanges occur through an encrypted tunnel.",
                    Tags = ["kerberos", "fast", "armoring", "client", "enforcement"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Requiring FAST on clients breaks authentication for Windows 7 and older clients. Audit FAST support across the domain before enforcing.",
                    RegistryKeys = [KdcKey],
                    ApplyOps = [RegOp.SetDword(KdcKey, "RequireArmoredKrb5OnDC", 1)],
                    RemoveOps = [RegOp.DeleteValue(KdcKey, "RequireArmoredKrb5OnDC")],
                    DetectOps = [RegOp.CheckDword(KdcKey, "RequireArmoredKrb5OnDC", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-disable-des-encryption",
                    Label = "Disable DES Encryption Types for Kerberos",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Disables DES-CBC-CRC and DES-CBC-MD5 encryption types for Kerberos. DES is a 56-bit algorithm broken by modern cracking rigs in hours. Only AES128 and AES256 should remain enabled.",
                    Tags = ["kerberos", "des", "encryption", "cipher", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "DES key types must be re-negotiated for affected service accounts (ktpass /crypto AES256). Breaks Kerberos for systems that have DES only in their msDS-SupportedEncryptionTypes.",
                    RegistryKeys = [KrbKey],
                    ApplyOps = [RegOp.SetDword(KrbKey, "DefaultEncryptionTypes", 2147483616)],
                    RemoveOps = [RegOp.DeleteValue(KrbKey, "DefaultEncryptionTypes")],
                    DetectOps = [RegOp.CheckDword(KrbKey, "DefaultEncryptionTypes", 2147483616)],
                },
                new TweakDef
                {
                    Id = "krbadv-require-strict-kdc-validation",
                    Label = "Require Strict KDC Validation (Authenticate the KDC)",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Enables strict KDC validation so the client verifies the KDC's identity before trusting the returned tickets. Prevents rogue or spoofed KDCs from issuing valid-looking tickets to the client.",
                    Tags = ["kerberos", "kdc-validation", "rogue-kdc", "trust", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Resolves attacks where a rogue KDC tricks clients into accepting attacker-forged tickets. Requires DCs to have valid certificates in the NTAuth store.",
                    RegistryKeys = [KrbKey],
                    ApplyOps = [RegOp.SetDword(KrbKey, "ValidateKDCCertUsage", 1)],
                    RemoveOps = [RegOp.DeleteValue(KrbKey, "ValidateKDCCertUsage")],
                    DetectOps = [RegOp.CheckDword(KrbKey, "ValidateKDCCertUsage", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-enable-pkinit-freshness",
                    Label = "Enable PKInit Freshness Extension for Kerberos",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Enables the PKInit Freshness Extension (RFC 8070), which binds Kerberos authentication tokens to a freshness endpoint in the TGT. Prevents certificate-based credential relay and Golden Certificate attacks.",
                    Tags = ["kerberos", "pkinit", "freshness", "golden-ticket", "certificate"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Mitigates Golden Certificate attacks (CVE-2021-42278 / CVE-2022-34691 class). Requires Windows Server 2016+ DCs and Windows 10+ clients for full support.",
                    RegistryKeys = [KdcKey],
                    ApplyOps = [RegOp.SetDword(KdcKey, "PKInitHashAlgorithmConfiguration", 1)],
                    RemoveOps = [RegOp.DeleteValue(KdcKey, "PKInitHashAlgorithmConfiguration")],
                    DetectOps = [RegOp.CheckDword(KdcKey, "PKInitHashAlgorithmConfiguration", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-set-max-service-ticket-lifetime",
                    Label = "Reduce Maximum Kerberos Service Ticket Lifetime to 600 Minutes",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets the maximum Kerberos service ticket (TGS) lifetime to 600 minutes (10 hours). Shorter lifetimes reduce the window in which a captured ticket can be replayed; the default is 600 minutes but some environments set it higher.",
                    Tags = ["kerberos", "ticket-lifetime", "tgs", "replay-prevention", "session"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Reducing ticket lifetime increases authentication load as clients renew tickets more frequently. Verify DC capacity before reducing below 60 minutes.",
                    RegistryKeys = [KrbSvcKey],
                    ApplyOps = [RegOp.SetDword(KrbSvcKey, "MaxServiceTicketAge", 600)],
                    RemoveOps = [RegOp.DeleteValue(KrbSvcKey, "MaxServiceTicketAge")],
                    DetectOps = [RegOp.CheckDword(KrbSvcKey, "MaxServiceTicketAge", 600)],
                },
                new TweakDef
                {
                    Id = "krbadv-set-max-tgt-lifetime",
                    Label = "Set Maximum Kerberos TGT Lifetime to 10 Hours",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Limits the Ticket-Granting Ticket (TGT) lifetime to 10 hours (600 minutes). Reduces the window for Golden Ticket attacks — if a TGT is captured, the attacker has a bounded exploitation window.",
                    Tags = ["kerberos", "tgt", "golden-ticket", "ticket-lifetime", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Shorter TGT lifetime is a key mitigation for Golden Ticket attacks. Users must re-authenticate after TGT expiry; aligns with standard domain policy (10 hours).",
                    RegistryKeys = [KrbSvcKey],
                    ApplyOps = [RegOp.SetDword(KrbSvcKey, "MaxTicketAge", 10)],
                    RemoveOps = [RegOp.DeleteValue(KrbSvcKey, "MaxTicketAge")],
                    DetectOps = [RegOp.CheckDword(KrbSvcKey, "MaxTicketAge", 10)],
                },
                new TweakDef
                {
                    Id = "krbadv-enforce-tgs-renewal-deadline",
                    Label = "Enforce Strict Kerberos Ticket Renewal Deadline (7 Days)",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets the maximum Kerberos ticket renewal lifetime to 7 days. After 7 days a ticket cannot be renewed and the user must obtain a fresh TGT; this ensures stale or stolen tickets expire regardless of continuous renewal.",
                    Tags = ["kerberos", "ticket-renewal", "expiry", "stolen-ticket", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Ensures long-running sessions produce fresh TGTs at least weekly. Non-disruptive for interactive users; long-running services must handle 7-day re-auth.",
                    RegistryKeys = [KrbSvcKey],
                    ApplyOps = [RegOp.SetDword(KrbSvcKey, "MaxRenewAge", 7)],
                    RemoveOps = [RegOp.DeleteValue(KrbSvcKey, "MaxRenewAge")],
                    DetectOps = [RegOp.CheckDword(KrbSvcKey, "MaxRenewAge", 7)],
                },
                new TweakDef
                {
                    Id = "krbadv-enforce-clock-sync",
                    Label = "Enforce Strict Kerberos Clock Synchronisation Tolerance (5 Minutes)",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets the Kerberos clock skew tolerance to 5 minutes (the standard RFC 4120 maximum). Clock skew is required for replay-protection; allowing large skew enables ticket replay. Enforce NTP synchronisation alongside this policy.",
                    Tags = ["kerberos", "clock-sync", "ntp", "replay-protection", "time"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Requires reliable NTP across all domain members. Systems without time sync will fail Kerberos authentication if clock skew exceeds 5 minutes.",
                    RegistryKeys = [KrbSvcKey],
                    ApplyOps = [RegOp.SetDword(KrbSvcKey, "MaxClockSkew", 5)],
                    RemoveOps = [RegOp.DeleteValue(KrbSvcKey, "MaxClockSkew")],
                    DetectOps = [RegOp.CheckDword(KrbSvcKey, "MaxClockSkew", 5)],
                },
                new TweakDef
                {
                    Id = "krbadv-disable-rc4-hmac-encryption",
                    Label = "Disable RC4-HMAC Encryption for Kerberos (Require AES)",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Removes RC4-HMAC (ARCFOUR-HMAC-MD5) from the supported Kerberos encryption type list. RC4-HMAC is vulnerable to offline cracking (AS-REP roasting, Kerberoasting); AES128 and AES256 should be the only accepted types.",
                    Tags = ["kerberos", "rc4", "arcfour", "encryption", "kerberoasting"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Completely eliminates Kerberoasting vector. Service accounts with RC4 keys (old msDS-SupportedEncryptionTypes) will fail; re-key all SPNs with AES before enforcing.",
                    RegistryKeys = [KrbKey],
                    ApplyOps = [RegOp.SetDword(KrbKey, "SupportedEncryptionTypes", 2147483640)],
                    RemoveOps = [RegOp.DeleteValue(KrbKey, "SupportedEncryptionTypes")],
                    DetectOps = [RegOp.CheckDword(KrbKey, "SupportedEncryptionTypes", 2147483640)],
                },
            ];
    }
}
