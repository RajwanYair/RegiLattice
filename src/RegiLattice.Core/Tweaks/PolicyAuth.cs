namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from PolicyAuth.cs ──
// RegiLattice.Core — Tweaks/PolicyAuth.cs
// Kerberos, LAPS, credential management, smart card, Windows Hello, biometrics, LSA, logon, and identity policies
// Category: "Authentication & Identity Policy"
// Consolidated from 33 modules.

internal static class PolicyAuth
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Id = "biometric-disable-biometrics-service",
                    Label = "Disable Windows Biometrics Service",
                    Category = "User Account",
                    Description =
                        "Disables the Windows Biometric Service (WBS), preventing any biometric authentication including Windows Hello fingerprint and face recognition. Use on systems where biometrics are not permitted.",
                    Tags = ["biometrics", "windows-hello", "fingerprint", "face", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Windows Biometric Service disabled; no fingerprint or face sign-in available. PIN or password required.",
                    ApplyOps = [RegOp.SetDword(Key, "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                    DetectOps = [RegOp.CheckDword(Key, "Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "biometric-disable-face-recognition",
                    Label = "Disable Windows Hello Face Recognition",
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Id = "credcache-enable-lsa-protection",
                Label = "Enable LSA Protected Process Light (PPL)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = false,
                Description =
                    "Runs LSASS as a Protected Process Light to block credential-dumping tools. "
                    + "RunAsPPL=1. Requires Secure Boot and a system reboot. Default: disabled.",
                Tags = ["lsa", "lsass", "ppl", "credentials", "hardening"],
                ApplyOps = [RegOp.SetDword(Lsa, "RunAsPPL", 1)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "RunAsPPL")],
                DetectOps = [RegOp.CheckDword(Lsa, "RunAsPPL", 1)],
            },
            new TweakDef
            {
                Id = "credcache-disable-domain-creds",
                Label = "Block storing network authentication credentials",
                Category = "User Account",
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
            new TweakDef
            {
                Id = "credcache-restrict-anonymous",
                Label = "Restrict anonymous enumeration of accounts and shares",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents anonymous users from enumerating SAM accounts and network shares. "
                    + "RestrictAnonymous=1. Default: 0. Recommended baseline: 1 or 2.",
                Tags = ["anonymous", "sam", "enumeration", "hardening"],
                ApplyOps = [RegOp.SetDword(Lsa, "RestrictAnonymous", 1)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "RestrictAnonymous")],
                DetectOps = [RegOp.CheckDword(Lsa, "RestrictAnonymous", 1)],
            },
            new TweakDef
            {
                Id = "credcache-restrict-anonymous-sam",
                Label = "Restrict anonymous enumeration of SAM account names",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents anonymous connections from enumerating SAM account names. "
                    + "RestrictAnonymousSAM=1. Ensures this setting is policy-enforced and cannot be cleared.",
                Tags = ["anonymous", "sam", "policy", "hardening"],
                ApplyOps = [RegOp.SetDword(Lsa, "RestrictAnonymousSAM", 1)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "RestrictAnonymousSAM")],
                DetectOps = [RegOp.CheckDword(Lsa, "RestrictAnonymousSAM", 1)],
            },
            new TweakDef
            {
                Id = "credcache-disable-everyone-anonymous",
                Label = "Remove Anonymous from the Everyone group",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents the Anonymous user from being a member of the Everyone group. "
                    + "EveryoneIncludesAnonymous=0. Restricts access to resources that grant permissions to Everyone.",
                Tags = ["anonymous", "everyone", "access-control", "hardening"],
                ApplyOps = [RegOp.SetDword(Lsa, "EveryoneIncludesAnonymous", 0)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "EveryoneIncludesAnonymous")],
                DetectOps = [RegOp.CheckDword(Lsa, "EveryoneIncludesAnonymous", 0)],
            },
            new TweakDef
            {
                Id = "credcache-disable-lm-hash",
                Label = "Disable LM hash storage for passwords",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows from storing an LM hash on the next password change. "
                    + "NoLmHash=1. LM hashes are trivially crackable; disabling prevents offline attacks.",
                Tags = ["lm", "hash", "password", "credentials", "hardening"],
                ApplyOps = [RegOp.SetDword(Lsa, "NoLmHash", 1)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "NoLmHash")],
                DetectOps = [RegOp.CheckDword(Lsa, "NoLmHash", 1)],
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
                    Id = "creddel-enable-restricted-admin",
                    Label = "Enable Restricted Admin Mode for RDP",
                    Category = "User Account",
                    Description = "Forces Remote Desktop connections to use Restricted Admin mode, preventing credential forwarding to remote hosts.",
                    Tags = ["credentials", "delegation", "rdp", "restricted-admin", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Prevents network credential forwarding over RDP; administrators must have local admin rights on target.",
                    ApplyOps = [RegOp.SetDword(CredDelKey, "RestrictedRemoteAdministration", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredDelKey, "RestrictedRemoteAdministration")],
                    DetectOps = [RegOp.CheckDword(CredDelKey, "RestrictedRemoteAdministration", 1)],
                },
                new TweakDef
                {
                    Id = "creddel-disable-remote-host-delegation",
                    Label = "Disable Credential Delegation to Remote Hosts",
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Id = "credmgr-deny-remote-desktop-credential-delegation",
                Label = "Deny Credential Delegation through Remote Desktop without NLA",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Credential delegation through Remote Desktop without Network Level Authentication allows attackers to present a rogue RDP server that captures user credentials before showing any connection UI. Denying credential delegation without NLA ensures that credentials are only forwarded after the server's identity has been verified through NLA mutual authentication. Pre-NLA credential delegation is a classic man-in-the-middle attack vector for RDP where a network attacker can harvest credentials from unpatched or misconfigured RDP clients. NLA requires that the client authenticate to the server before establishing the remote desktop session preventing eavesdropping on the initial credential exchange. All organizations should require NLA for all RDP connections and deny credential delegation to servers that do not support NLA. Remote Desktop Gateway solutions provide NLA enforcement for external RDP access and should be used for all external remote access scenarios.",
                Tags = ["credentials", "rdp", "nla", "man-in-the-middle", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DenyDefaultCredentials", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DenyDefaultCredentials")],
                DetectOps = [RegOp.CheckDword(Key, "DenyDefaultCredentials", 1)],
            },
            new TweakDef
            {
                Id = "credmgr-restrict-saved-rdp-credentials",
                Label = "Prevent Saving of Remote Desktop Connection Credentials",
                Category = "User Account",
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
                Id = "credmgr-enable-restricted-admin-mode",
                Label = "Enable Restricted Admin Mode for Remote Desktop Connections",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Restricted Admin Mode prevents credential delegation when connecting via RDP by using the connecting computer's credentials rather than the user's credentials on the remote system. Enabling Restricted Admin Mode reduces the credential material exposed to the remote system limiting credential theft from a compromised RDP host. In Restricted Admin Mode administrative access is available but the full user credentials are not sent to the remote server making them unavailable for lateral movement from that server. Organizations should enable Restricted Admin Mode for all privileged RDP connections especially to servers that may be compromised. Restricted Admin Mode has the trade-off that network resources accessed from the remote session use the remote computer's credentials not the user's potentially causing access failures. Combining Restricted Admin Mode with privileged access workstations provides defense-in-depth against credential theft through RDP.",
                Tags = ["credentials", "rdp", "restricted-admin", "pass-the-hash", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictedRemoteAdministration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictedRemoteAdministration")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictedRemoteAdministration", 1)],
            },
            new TweakDef
            {
                Id = "credmgr-disable-wdigest-authentication",
                Label = "Disable WDigest Authentication to Prevent Cleartext Password Storage in Memory",
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                Id = "credui-disable-password-reveal",
                Label = "Disable Password Reveal Button in Credential UI",
                Category = "User Account",
                Description = "Hides the password-reveal eye icon in credential dialogs and the lock screen, reducing shoulder-surfing risk.",
                Tags = ["credential", "security", "group-policy", "hardening", "password"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CredUi, "DisablePasswordReveal", 1)],
                RemoveOps = [RegOp.DeleteValue(CredUi, "DisablePasswordReveal")],
                DetectOps = [RegOp.CheckDword(CredUi, "DisablePasswordReveal", 1)],
            },
            new TweakDef
            {
                Id = "credui-disable-administrator-enumeration",
                Label = "Disable Administrator Account Enumeration in Credential UI",
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Id = "kerbmit-disable-rc4-encryption",
                    Label = "Disable RC4 for Kerberos Ticket Encryption",
                    Category = "User Account",
                    Description =
                        "Sets SupportedEncryptionTypes=0x18 (24) in Kerberos Parameters to allow only AES-128 and AES-256, removing RC4-HMAC support. Kerberoasting succeeds primarily because service tickets encrypted with RC4-HMAC can be cracked offline in hours or days on a GPU. Enforcing AES-only encryption requires 10×–100× more compute for offline attacks, making cracking economically infeasible for properly generated key material.",
                    Tags = ["kerberos", "rc4", "aes", "kerberoasting", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Disables RC4 Kerberos; legacy services with RC4-only service accounts will fail TGS; upgrade service account keys first.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "SupportedEncryptionTypes", 24)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "SupportedEncryptionTypes")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "SupportedEncryptionTypes", 24)],
                },
                new TweakDef
                {
                    Id = "kerbmit-set-max-service-ticket-age",
                    Label = "Reduce Kerberos Service Ticket Lifetime (600 min)",
                    Category = "User Account",
                    Description =
                        "Sets MaxServiceAge=600 in Kerberos Parameters. Reduces the maximum service ticket (TGS) lifetime from the Windows default of 600 minutes. Shorter ticket lifetimes reduce the window of opportunity for Kerberoasted tickets to be cracked and used: a ticket valid for 10 hours gives an attacker 10 hours to crack it; reducing to 10 minutes means the ticket expires before most cracking jobs complete.",
                    Tags = ["kerberos", "ticket-lifetime", "tgs", "kerberoasting", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Reduces service ticket lifetime; very short lifetimes increase KDC load from more frequent ticket requests.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "MaxServiceAge", 600)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "MaxServiceAge")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "MaxServiceAge", 600)],
                },
                new TweakDef
                {
                    Id = "kerbmit-reduce-max-tgt-age",
                    Label = "Reduce Kerberos TGT Lifetime (600 min)",
                    Category = "User Account",
                    Description =
                        "Sets MaxTicketAge=600 in Kerberos Parameters. Limits the maximum lifetime of Kerberos Ticket-Granting Tickets. A shorter TGT lifetime limits how long a compromised TGT can be used for privilege escalation (Pass-the-Ticket attacks). After TGT expiry the user must re-authenticate, providing a natural checkpoint to detect and respond to compromised credentials before they can be used further.",
                    Tags = ["kerberos", "tgt", "ticket-lifetime", "pass-the-ticket", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Reduces TGT lifetime; users will be re-prompted for credentials more frequently in long sessions.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "MaxTicketAge", 600)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "MaxTicketAge")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "MaxTicketAge", 600)],
                },
                new TweakDef
                {
                    Id = "kerbmit-tighten-clock-skew",
                    Label = "Tighten Kerberos Clock Skew Tolerance (2 min)",
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Id = "kerbmit-set-renewal-window",
                    Label = "Reduce Kerberos Ticket Renewal Window (4 days)",
                    Category = "User Account",
                    Description =
                        "Sets MaxRenewAge=4 in Kerberos Parameters. Limits how long a Kerberos TGT can be renewed without full re-authentication. The Windows default is 7 days — meaning a stolen TGT can be continuously renewed for a week without the user re-entering credentials. Reducing to 4 days tightens the window during which a compromised TGT provides persistent access, improving detection opportunities.",
                    Tags = ["kerberos", "renewal", "tgt", "persistence", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Reduces TGT renewal window to 4 days; users on extended leave may need to re-authenticate on return.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "MaxRenewAge", 4)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "MaxRenewAge")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "MaxRenewAge", 4)],
                },
                new TweakDef
                {
                    Id = "kerbmit-enable-armoring",
                    Label = "Enable FAST Kerberos Armoring (Tunnel Mode)",
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                Id = "krb-require-aes256-encryption",
                Label = "Kerberos: Require AES-256 Encryption for Ticket Grants",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 4,
                SafetyRating = 4,
                RegistryKeys = [KrbLsaParams],
                Tags = ["kerberos", "authentication", "aes256", "encryption", "security", "hardening"],
                Description =
                    "Sets SupportedEncryptionTypes=0x18 (24 decimal) in Kerberos\\Parameters. "
                    + "Bit mask 0x18 enables only AES-128-CTS-HMAC-SHA1-96 and AES-256-CTS-HMAC-SHA1-96. "
                    + "Disables the weaker RC4-HMAC (NTLM-hash-based) cipher used in older 'Kerberoasting' "
                    + "attacks. Requires Windows Server 2008 R2 or later as the KDC.",
                ApplyOps = [RegOp.SetDword(KrbLsaParams, "SupportedEncryptionTypes", 0x18)],
                RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "SupportedEncryptionTypes")],
                DetectOps = [RegOp.CheckDword(KrbLsaParams, "SupportedEncryptionTypes", 0x18)],
            },
            new TweakDef
            {
                Id = "krb-disable-des-encryption",
                Label = "Kerberos: Disable DES Encryption Types",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 4,
                SafetyRating = 5,
                RegistryKeys = [KrbPolicyParams],
                Tags = ["kerberos", "authentication", "des", "encryption", "security", "hardening"],
                Description =
                    "Sets SupportedEncryptionTypes=0x7FFFFFF8 in Windows Kerberos policy. "
                    + "Disables all DES and RC4-MD5 ciphers (bits 0–2 cleared), retaining AES-128/256 "
                    + "and future ciphers. DES was retired by NIST in 2004; its presence enables "
                    + "down-grade attacks in mixed-mode Active Directory environments.",
                ApplyOps = [RegOp.SetDword(KrbPolicyParams, "SupportedEncryptionTypes", 0x7FFFFFF8)],
                RemoveOps = [RegOp.DeleteValue(KrbPolicyParams, "SupportedEncryptionTypes")],
                DetectOps = [RegOp.CheckDword(KrbPolicyParams, "SupportedEncryptionTypes", 0x7FFFFFF8)],
            },
            new TweakDef
            {
                Id = "krb-set-max-ticket-life-8h",
                Label = "Kerberos: Set Maximum Ticket Lifetime to 8 Hours",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [KrbLsaParams],
                Tags = ["kerberos", "authentication", "ticket-lifetime", "security"],
                Description =
                    "Sets MaxTicketAge=8 in Kerberos\\Parameters. "
                    + "Limits the maximum lifetime of Kerberos service tickets to 8 hours (value in hours). "
                    + "Default is 10 hours. Shorter lifetime reduces the window in which a stolen ticket "
                    + "can be replayed (golden/silver ticket attacks have a narrower useful window).",
                ApplyOps = [RegOp.SetDword(KrbLsaParams, "MaxTicketAge", 8)],
                RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "MaxTicketAge")],
                DetectOps = [RegOp.CheckDword(KrbLsaParams, "MaxTicketAge", 8)],
            },
            new TweakDef
            {
                Id = "krb-set-max-renewable-ticket-life-7d",
                Label = "Kerberos: Set Maximum Renewable Ticket Lifetime to 7 Days",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [KrbLsaParams],
                Tags = ["kerberos", "authentication", "ticket-lifetime", "security"],
                Description =
                    "Sets MaxRenewAge=7 in Kerberos\\Parameters. "
                    + "Limits the maximum renewable lifetime of a Kerberos TGT to 7 days (value in days). "
                    + "Default is 7 days; setting it explicitly via registry ensures it is not overridden. "
                    + "Renewable tickets allow continuous session renewal without re-entering credentials.",
                ApplyOps = [RegOp.SetDword(KrbLsaParams, "MaxRenewAge", 7)],
                RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "MaxRenewAge")],
                DetectOps = [RegOp.CheckDword(KrbLsaParams, "MaxRenewAge", 7)],
            },
            new TweakDef
            {
                Id = "krb-disable-clock-skew-tolerance",
                Label = "Kerberos: Tighten Clock-Skew Tolerance to 3 Minutes",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 3,
                RegistryKeys = [KrbLsaParams],
                Tags = ["kerberos", "authentication", "clock-skew", "security", "ntp"],
                Description =
                    "Sets SkewTime=3 in Kerberos\\Parameters. "
                    + "Reduces the accepted clock skew between the client and KDC from 5 minutes (default) "
                    + "to 3 minutes. A smaller window narrows replay-attack opportunities while still "
                    + "tolerating NTP drift. Requires accurate NTP synchronisation.",
                ApplyOps = [RegOp.SetDword(KrbLsaParams, "SkewTime", 3)],
                RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "SkewTime")],
                DetectOps = [RegOp.CheckDword(KrbLsaParams, "SkewTime", 3)],
            },
            new TweakDef
            {
                Id = "krb-require-preauth",
                Label = "Kerberos: Require Pre-Authentication for All Accounts",
                Category = "User Account",
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
                Category = "User Account",
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
                Id = "krb-disable-rc4-hmac",
                Label = "Kerberos: Explicitly Disable RC4-HMAC (ARCFOUR) Encryption",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 19041,
                ImpactScore = 4,
                SafetyRating = 4,
                RegistryKeys = [KrbLsaParams],
                Tags = ["kerberos", "authentication", "rc4", "arcfour", "kerberoasting", "security", "hardening"],
                Description =
                    "Sets SupportedEncryptionTypes=0x7FFFFFB8 in Kerberos\\Parameters (clears RC4 bits). "
                    + "Explicitly removes RC4-HMAC (type 23) from the supported encryption set. "
                    + "RC4-HMAC is vulnerable to Kerberoasting (offline NTLM-hash cracking) and should "
                    + "not be used in environments with Windows Server 2019+ domain controllers.",
                ApplyOps = [RegOp.SetDword(KrbLsaParams, "SupportedEncryptionTypes", 0x7FFFFFB8)],
                RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "SupportedEncryptionTypes")],
                DetectOps = [RegOp.CheckDword(KrbLsaParams, "SupportedEncryptionTypes", 0x7FFFFFB8)],
            },
            new TweakDef
            {
                Id = "krb-log-authentication-events",
                Label = "Kerberos: Enable Verbose Authentication Logging",
                Category = "User Account",
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
                Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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
                    Category = "User Account",
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

    // ── KerberosDelegationPolicy ──
    private static class _KerberosDelegationPolicy
    {
        private const string KerbKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Security\Kerberos";

        private const string KerbNtKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\Kerberos";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "krbdel-require-kdc-validation",
                    Label = "Kerberos Delegation: Require KDC Certificate Validation for PKINIT",
                    Category = "User Account",
                    Description =
                        "Sets RequireKdcCertificate=1 in the Kerberos policy hive. Requires that the KDC (Key Distribution Center) presents a valid certificate during PKINIT (Public Key Initial Authentication) operations. Without KDC certificate validation, a rogue KDC on the network could successfully complete PKINIT authentication, allowing an attacker who performs an ARP or DNS spoofing attack to position a fake KDC and capture Kerberos TGT requests. This setting is particularly important in environments using smartcard or certificate-based authentication — certificate-based Kerberos without KDC validation is vulnerable to man-in-the-middle attacks.",
                    Tags = ["kerberos", "kdc", "pkinit", "certificate", "validation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "PKINIT operations require valid KDC certificate. Only applies in environments using smartcard or certificate-based Kerberos login. No impact on password-based Kerberos. PKINIT authentication will fail if the KDC certificate is expired, revoked, or untrusted — ensure domain controller certificates are maintained.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "RequireKdcCertificate", 1)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "RequireKdcCertificate")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "RequireKdcCertificate", 1)],
                },
                new TweakDef
                {
                    Id = "krbdel-enable-claims-and-compound-auth",
                    Label = "Kerberos Delegation: Enable Kerberos Claims and Compound Authentication",
                    Category = "User Account",
                    Description =
                        "Sets EnableCbacAndArmor=3 in the Kerberos policy hive (always enable armoring and CBAC). Kerberos Armoring (FAST — Flexible Authentication Secure Tunneling) wraps Kerberos AS and TGS exchange messages in a protective tunnel, preventing offline cracking of AS-REQ pre-authentication data (a technique used by Kerberoasting attacks). Claims-Based Access Control (CBAC) augments Kerberos tickets with user and device claims for Dynamic Access Control. Setting value 3 (always armor — not just when supported) ensures the strongest protection for all authenticating clients.",
                    Tags = ["kerberos", "fast", "armoring", "claims", "cbac", "kerberoasting"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Kerberos FAST armoring and CBAC are always enabled. Kerberoasting offline cracking of AS-REQ data is prevented. Requires all authenticating DCs to support Kerberos armoring (Windows Server 2012+). Older DCs (2008 R2 or earlier) cannot participate. Test in a lab before deploying domain-wide.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "EnableCbacAndArmor", 3)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "EnableCbacAndArmor")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "EnableCbacAndArmor", 3)],
                },
                new TweakDef
                {
                    Id = "krbdel-set-ticket-lifetime-600",
                    Label = "Kerberos Delegation: Set TGT Maximum Lifetime to 600 Minutes",
                    Category = "User Account",
                    Description =
                        "Sets MaxTicketAge=600 in the Kerberos WinLogon hive. Caps the Kerberos Ticket-Granting Ticket (TGT) lifetime at 600 minutes (10 hours). A TGT is a long-lived credential that allows a user to obtain service tickets without re-authenticating to the KDC. If an attacker compromises a TGT (e.g., via Pass-the-Ticket or Golden Ticket attacks), the ticket is valid until its expiry. The default TGT lifetime is 10 hours; reducing it to 600 minutes aligns with a single work session and minimises the window in which a stolen TGT is valid. Combine with 10-minute service ticket lifetime for strongest protection.",
                    Tags = ["kerberos", "tgt", "ticket-lifetime", "pass-the-ticket", "golden-ticket"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "TGT lifetime is 600 minutes. Users with sessions longer than 10 hours may be prompted for re-authentication after the TGT expires (typically once their TGT is not automatically renewed). For most corporate environments this matches the working day. Kerberos ticket renewal is transparent to users in most cases.",
                    ApplyOps = [RegOp.SetDword(KerbNtKey, "MaxTicketAge", 600)],
                    RemoveOps = [RegOp.DeleteValue(KerbNtKey, "MaxTicketAge")],
                    DetectOps = [RegOp.CheckDword(KerbNtKey, "MaxTicketAge", 600)],
                },
                new TweakDef
                {
                    Id = "krbdel-set-service-ticket-lifetime-10",
                    Label = "Kerberos Delegation: Set TGS Service Ticket Lifetime to 10 Minutes",
                    Category = "User Account",
                    Description =
                        "Sets MaxServiceAge=10 in the Kerberos WinLogon hive. Sets the maximum lifetime for Kerberos service tickets (TGS tickets) to 10 minutes. Service tickets are short-lived credentials used for authenticating to a specific service (file share, SQL server, web application). If a service ticket is intercepted (e.g., via Kerberoasting — requesting a service ticket for an SPN and attempting offline cracking), a 10-minute lifetime means the cracked ticket is useful for only a very short window. Combined with strong service account passwords, this severely limits the utility of Kerberoasted tickets.",
                    Tags = ["kerberos", "service-ticket", "tgs", "kerberoasting", "lifetime"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Service ticket lifetime is 10 minutes. Clients silently request new service tickets as old ones expire; in most cases this is transparent. Some legacy applications with hard-coded Kerberos ticket handling may fail after 10 minutes. Test with critical line-of-business applications before deploying.",
                    ApplyOps = [RegOp.SetDword(KerbNtKey, "MaxServiceAge", 10)],
                    RemoveOps = [RegOp.DeleteValue(KerbNtKey, "MaxServiceAge")],
                    DetectOps = [RegOp.CheckDword(KerbNtKey, "MaxServiceAge", 10)],
                },
                new TweakDef
                {
                    Id = "krbdel-set-renewable-ticket-lifetime-7days",
                    Label = "Kerberos Delegation: Set Renewable Ticket Maximum Lifetime to 7 Days",
                    Category = "User Account",
                    Description =
                        "Sets MaxRenewAge=7 in the Kerberos WinLogon hive (units: days). Limits the window during which a Kerberos TGT can be renewed without re-authentication to 7 days. Kerberos TGTs can be marked as renewable: a client can present an expired TGT to the KDC and obtain a fresh one without providing a password, as long as the renewal request is within the MaxRenewAge window. If an attacker gets a copy of a TGT before it expires, they can potentially keep renewing it for up to the MaxRenewAge period. Setting 7 days limits the long-tail abuse window while supporting common Remote Desktop and service account usage patterns.",
                    Tags = ["kerberos", "renewable-ticket", "renewal-window", "tgt", "pass-the-ticket"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Renewable ticket window is 7 days. TGTs that have not been renewed within 7 days of original issuance require full re-authentication. Service accounts and interactive users with sessions spanning weekends are unaffected (7 days covers typical weekend coverage). No visible impact to most users.",
                    ApplyOps = [RegOp.SetDword(KerbNtKey, "MaxRenewAge", 7)],
                    RemoveOps = [RegOp.DeleteValue(KerbNtKey, "MaxRenewAge")],
                    DetectOps = [RegOp.CheckDword(KerbNtKey, "MaxRenewAge", 7)],
                },
                new TweakDef
                {
                    Id = "krbdel-enforce-strict-kdc-clock-skew-5min",
                    Label = "Kerberos Delegation: Enforce Strict 5-Minute Clock Skew Tolerance",
                    Category = "User Account",
                    Description =
                        "Sets MaxClockSkew=5 in the Kerberos WinLogon hive (units: minutes). Enforces a 5-minute maximum clock skew between client and KDC for Kerberos authentication. Kerberos relies on timestamps to prevent replay attacks — a Service ticket is only valid within a specific time window. If the clock skew limit is large, an attacker can capture a Kerberos service ticket and replay it successfully within the extended window. The default is 5 minutes (matching RFC 4120 recommendation). Explicitly setting it prevents GPO inheritance from accidentally relaxing this to a larger value.",
                    Tags = ["kerberos", "clock-skew", "replay-attack", "ntp", "timestamp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "5-minute clock skew strictly enforced. Systems with clocks drifted more than 5 minutes from the KDC will fail Kerberos authentication. Requires functioning NTP infrastructure. Virtual machines that resume from sleep may experience brief Kerberos failures until the clock synchronises with the domain controller.",
                    ApplyOps = [RegOp.SetDword(KerbNtKey, "MaxClockSkew", 5)],
                    RemoveOps = [RegOp.DeleteValue(KerbNtKey, "MaxClockSkew")],
                    DetectOps = [RegOp.CheckDword(KerbNtKey, "MaxClockSkew", 5)],
                },
                new TweakDef
                {
                    Id = "krbdel-enable-pac-validation",
                    Label = "Kerberos Delegation: Enable PAC Request Validation on Kerberos Tickets",
                    Category = "User Account",
                    Description =
                        "Sets ValidateKdcPacSignature=1 in the Kerberos WinLogon hive. Enables validation of the Privilege Attribute Certificate (PAC) signature in Kerberos service tickets. The PAC contains group membership, logon hours, user rights, and other authorisation data. The MS14-068 vulnerability (a critical KDC privilege escalation) allowed an attacker to forge the PAC signature and elevate to Domain Admin. Enabling PAC signature validation ensures that all PACs in Kerberos tickets are cryptographically validated by the KDC before authorisation data is trusted. This closes a class of Kerberos PAC forgery attacks.",
                    Tags = ["kerberos", "pac", "ms14-068", "signature-validation", "privilege-escalation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "PAC cryptographic signature validated on every Kerberos service ticket. Mitigates MS14-068 and related PAC forgery attack classes. No visible impact to users — validation is performed transparently by the KDC. Requires all DCs to have the MS14-068 patch or be Windows Server 2012 R2+.",
                    ApplyOps = [RegOp.SetDword(KerbNtKey, "ValidateKdcPacSignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(KerbNtKey, "ValidateKdcPacSignature")],
                    DetectOps = [RegOp.CheckDword(KerbNtKey, "ValidateKdcPacSignature", 1)],
                },
                new TweakDef
                {
                    Id = "krbdel-enable-aes256-kerberos-encryption",
                    Label = "Kerberos Delegation: Enforce AES-256 Kerberos Encryption, Disable RC4",
                    Category = "User Account",
                    Description =
                        "Sets SupportedEncryptionTypes=0x7FFFFFF8 in the Kerberos policy hive (enables AES256-HMAC-SHA1, AES128-HMAC-SHA1, DES-CBC-MD5 is excluded; RC4 HMAC is disabled). RC4-HMAC (also known as ARCFOUR-HMAC or Kerberos etype 17) is broken for Kerberos purposes — the NTLM hash of the user's password is directly usable as the Kerberos session key (enabling Pass-the-Hash attacks that bypass Kerberos entirely). Forcing AES-256 and AES-128 only means that stolen NTLM hashes cannot be used to forge Kerberos session keys, and Kerberoasted service ticket encryption must be broken as strong AES rather than weak RC4.",
                    Tags = ["kerberos", "aes256", "rc4", "encryption-type", "kerberoasting", "pass-the-hash"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "RC4-HMAC Kerberos encryption is disabled. AES-256 and AES-128 are enforced. Kerberoasting attacks must now defeat AES-256 encryption instead of RC4. Legacy applications and services that only support RC4 Kerberos encryption (typically very old Java, Oracle JDBC, or custom Kerberos implementations) will fail to authenticate. Test with all Kerberos-authenticating services.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "SupportedEncryptionTypes", 0x7FFFFFF8)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "SupportedEncryptionTypes")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "SupportedEncryptionTypes", 0x7FFFFFF8)],
                },
                new TweakDef
                {
                    Id = "krbdel-disable-des-kerberos-encryption",
                    Label = "Kerberos Delegation: Disable DES Kerberos Encryption Types",
                    Category = "User Account",
                    Description =
                        "Sets NtlmMinClientSec=0x20080000 in the Kerberos policy to explicitly exclude DES-based Kerberos encryption types (DES-CBC-CRC and DES-CBC-MD5). DES is a 56-bit block cipher that is comprehensively broken and should never be used in any security context. In Kerberos, DES encryption types (etypes 1 and 3) are retained for backwards compatibility with very old systems (pre-Windows 2000). An attacker who obtains a DES-encrypted Kerberos ticket can crack it in seconds to hours with commodity hardware. Windows Vista+ disabled DES by default; this policy ensures no Group Policy can accidentally re-enable it.",
                    Tags = ["kerberos", "des", "broken-crypto", "encryption-type", "etype"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "DES Kerberos encryption types are explicitly disabled. No modern Windows system requires DES Kerberos. If any legacy system (pre-Vista Windows or old UNIX Kerberos client) attempts DES-based Kerberos authentication, it will fail. Survey the environment before enforcing if there are unknown legacy systems.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "DESEncryptionDisabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "DESEncryptionDisabled")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "DESEncryptionDisabled", 1)],
                },
                new TweakDef
                {
                    Id = "krbdel-enable-kerberos-pre-auth-required",
                    Label = "Kerberos Delegation: Require Kerberos Pre-Authentication for All Accounts",
                    Category = "User Account",
                    Description =
                        "Sets DoNotRequirePreauth=0 in the Kerberos policy hive. Ensures that Kerberos pre-authentication is required for all accounts by policy. By default, any account in Active Directory with the 'Do not require Kerberos preauthentication' flag set (DONT_REQ_PREAUTH) will respond to an AS-REQ with an AS-REP without the client first proving knowledge of the password. This is the condition that enables AS-REP Roasting — an attacker can request an AS-REP for any account with this flag and attempt offline cracking of the encrypted portion. This policy setting prevents the environment from inadvertently introducing accounts with this flag via attribute editors.",
                    Tags = ["kerberos", "pre-auth", "as-rep-roasting", "asrep", "preauth"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "AS-REP Roasting is blocked: all accounts require Kerberos pre-auth by policy. Pre-existing accounts with DONT_REQ_PREAUTH set in AD must be audited and corrected separately (policy alone cannot override per-account AD attribute flags). Use PowerShell to audit: Get-ADUser -Filter {DoesNotRequirePreAuth -eq $true}.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "DoNotRequirePreauth", 0)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "DoNotRequirePreauth")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "DoNotRequirePreauth", 0)],
                },
            ];
    }

    // ── KerberosEncryptionPolicy ──
    private static class _KerberosEncryptionPolicy
    {
        private const string KerbPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters";
        private const string KerbLsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "kerb-disable-des-encryption",
                Label = "Disable DES Encryption for Kerberos",
                Category = "User Account",
                Description = "Prevents Kerberos from using the broken DES (56-bit) encryption type for tickets.",
                Tags = ["kerberos", "des", "encryption", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "DES is trivially broken; removing it forces AES. Requires DC and clients on Server 2008+/Vista+.",
                ApplyOps = [RegOp.SetDword(KerbPolicyKey, "SupportedEncryptionTypes", 2147483640)],
                RemoveOps = [RegOp.DeleteValue(KerbPolicyKey, "SupportedEncryptionTypes")],
                DetectOps = [RegOp.CheckDword(KerbPolicyKey, "SupportedEncryptionTypes", 2147483640)],
            },
            new TweakDef
            {
                Id = "kerb-disable-rc4-encryption",
                Label = "Disable RC4-HMAC Encryption for Kerberos",
                Category = "User Account",
                Description = "Removes RC4-HMAC from Kerberos supported encryption types, forcing AES128/AES256 only.",
                Tags = ["kerberos", "rc4", "encryption", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "RC4 in Kerberos enables AS-REP roasting and other attacks. Removing it requires all principal accounts to have AES keys set.",
                ApplyOps = [RegOp.SetDword(KerbPolicyKey, "SupportedEncryptionTypes", 2147483616)],
                RemoveOps = [RegOp.DeleteValue(KerbPolicyKey, "SupportedEncryptionTypes")],
                DetectOps = [RegOp.CheckDword(KerbPolicyKey, "SupportedEncryptionTypes", 2147483616)],
            },
            new TweakDef
            {
                Id = "kerb-require-aes256",
                Label = "Require AES256 for Kerberos",
                Category = "User Account",
                Description = "Configures Kerberos to prefer AES256-CTS-HMAC-SHA1-96 as the sole supported encryption type.",
                Tags = ["kerberos", "aes256", "encryption", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Gold standard for Kerberos crypto. Requires all service and user accounts to have AES256 keys pre-provisioned.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "SupportedEncryptionTypes", 24)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "SupportedEncryptionTypes")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "SupportedEncryptionTypes", 24)],
            },
            new TweakDef
            {
                Id = "kerb-set-max-ticket-age-600",
                Label = "Set Kerberos Maximum Ticket Age to 600 Minutes",
                Category = "User Account",
                Description = "Limits Kerberos TGT lifetime to 10 hours (600 minutes) to reduce stolen-ticket window.",
                Tags = ["kerberos", "ticket-age", "tgt", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Shorter TGT lifetime reduces Pass-the-Ticket window. Default is 10h; this enforces policy alignment.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "MaxTicketAge", 600)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "MaxTicketAge")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "MaxTicketAge", 600)],
            },
            new TweakDef
            {
                Id = "kerb-set-max-renew-age-7days",
                Label = "Set Kerberos Maximum Ticket Renewal Age to 7 Days",
                Category = "User Account",
                Description = "Limits how long a Kerberos TGT can be renewed before requiring full re-authentication.",
                Tags = ["kerberos", "renewal", "tgt", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "7-day renewal window is the CIS benchmark default. Prevents stale tickets being used indefinitely.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "MaxRenewAge", 10080)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "MaxRenewAge")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "MaxRenewAge", 10080)],
            },
            new TweakDef
            {
                Id = "kerb-set-max-service-ticket-600",
                Label = "Set Kerberos Maximum Service Ticket Age to 600 Minutes",
                Category = "User Account",
                Description = "Limits service ticket lifetime to 600 minutes to reduce the stolen service ticket window.",
                Tags = ["kerberos", "service-ticket", "st", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Matches Microsoft security baseline. Transparent to users.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "MaxServiceAge", 600)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "MaxServiceAge")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "MaxServiceAge", 600)],
            },
            new TweakDef
            {
                Id = "kerb-set-clock-skew-5min",
                Label = "Set Kerberos Maximum Clock Skew to 5 Minutes",
                Category = "User Account",
                Description = "Enforces a 5-minute maximum clock skew between client and KDC to prevent replay attacks.",
                Tags = ["kerberos", "clock-skew", "replay", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Standard Kerberos replay protection. Ensure NTP is configured to avoid authentication failures.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "SkewTime", 5)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "SkewTime")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "SkewTime", 5)],
            },
            new TweakDef
            {
                Id = "kerb-enable-armoring",
                Label = "Enable Kerberos Armoring (FAST)",
                Category = "User Account",
                Description = "Enables Kerberos Flexible Authentication Secure Tunnelling (FAST/armoring) to protect AS-REQ exchanges.",
                Tags = ["kerberos", "armoring", "fast", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "FAST prevents AS-REP roasting and preauthentication attacks. Requires Windows 8+ clients and Server 2012+ DCs.",
                ApplyOps = [RegOp.SetDword(KerbPolicyKey, "cbindingPolicy", 2)],
                RemoveOps = [RegOp.DeleteValue(KerbPolicyKey, "cbindingPolicy")],
                DetectOps = [RegOp.CheckDword(KerbPolicyKey, "cbindingPolicy", 2)],
            },
            new TweakDef
            {
                Id = "kerb-disable-upn-hint",
                Label = "Disable Kerberos UPN Hint Leakage",
                Category = "User Account",
                Description = "Prevents Kerberos error responses from leaking UPN/username hints to unauthenticated requesters.",
                Tags = ["kerberos", "upn", "enumeration", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks username enumeration via Kerberos pre-auth errors. Transparent for legitimate clients.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "UseUpnForClientAuthEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "UseUpnForClientAuthEnabled")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "UseUpnForClientAuthEnabled", 0)],
            },
            new TweakDef
            {
                Id = "kerb-set-preauthentication-required",
                Label = "Require Kerberos Preauthentication",
                Category = "User Account",
                Description = "Enforces Kerberos preauthentication to prevent AS-REP roasting on accounts that have it disabled.",
                Tags = ["kerberos", "preauthentication", "as-rep-roasting", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "AS-REP roasting requires accounts with 'Do not require Kerberos preauth' set. This policy enforces it machine-wide.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "PreAuthRequiredLevel", 1)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "PreAuthRequiredLevel")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "PreAuthRequiredLevel", 1)],
            },
        ];
    }

    // ── KerberosSecurityPolicy ──
    private static class _KerberosSecurityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters";
        private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System\Audit";
        private const string SecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Security\Kerberos";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "krbadv-enable-claims-support",
                    Label = "Enable Kerberos Claims and Compound Authentication Support",
                    Category = "User Account",
                    Description =
                        "Enables Kerberos claims-based authentication and compound authentication (user + device claims), required for Dynamic Access Control (DAC) file share access and conditional access policies based on device health claims.",
                    Tags = ["kerberos", "claims", "compound-auth", "dac", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Kerberos claims and compound auth enabled; required for Dynamic Access Control and device-based conditional access.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableCbacAndArmor", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableCbacAndArmor")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableCbacAndArmor", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-require-fast-armoring",
                    Label = "Require Kerberos Armoring (FAST) for All Authentication",
                    Category = "User Account",
                    Description =
                        "Requires Kerberos Flexible Authentication Secure Tunneling (FAST/Kerberos Armoring) for all Kerberos exchanges, providing protection against offline pre-authentication blob cracking attacks (AS-REP roasting).",
                    Tags = ["kerberos", "fast", "armoring", "asrep-roasting", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos FAST armoring required; AS-REP roasting attacks mitigated. Requires KDC support for FAST.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableKerberosArmoring", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableKerberosArmoring")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableKerberosArmoring", 2)],
                },
                new TweakDef
                {
                    Id = "krbadv-block-rc4-encryption",
                    Label = "Block RC4-HMAC Encryption for Kerberos Tickets",
                    Category = "User Account",
                    Description =
                        "Disables the RC4-HMAC cipher suite for Kerberos ticket encryption, forcing all tickets to use AES-128 or AES-256 encryption, which is significantly stronger than the legacy RC4 encryption still used by some service accounts.",
                    Tags = ["kerberos", "rc4", "aes", "encryption", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos RC4-HMAC encryption disabled; only AES-128/AES-256 tickets accepted. Service accounts need AES keys.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRC4Encryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRC4Encryption")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRC4Encryption", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-enable-des-encryption-off",
                    Label = "Disable DES Cipher for Kerberos (Legacy Removal)",
                    Category = "User Account",
                    Description =
                        "Disables DES (Data Encryption Standard) cipher support in Kerberos, eliminating the use of the cryptographically broken DES algorithm that was still negotiated with very old service accounts in some mixed environments.",
                    Tags = ["kerberos", "des", "legacy-cipher", "encryption", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos DES encryption completely disabled; broken DES cipher no longer negotiated in any Kerberos exchange.",
                    ApplyOps = [RegOp.SetDword(Key, "DisallowDES", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisallowDES")],
                    DetectOps = [RegOp.CheckDword(Key, "DisallowDES", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-set-ticket-lifetime-8h",
                    Label = "Set Kerberos Ticket Maximum Lifetime to 8 Hours",
                    Category = "User Account",
                    Description =
                        "Configures the Kerberos TGT (Ticket Granting Ticket) maximum lifetime to 8 hours, ensuring tickets expire during a typical business day so stolen tickets cannot be replayed indefinitely.",
                    Tags = ["kerberos", "ticket-lifetime", "tgt", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos TGT lifetime set to 8 hours; stolen tickets expire within a business day.",
                    ApplyOps = [RegOp.SetDword(SecKey, "MaxTicketAge", 8)],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "MaxTicketAge")],
                    DetectOps = [RegOp.CheckDword(SecKey, "MaxTicketAge", 8)],
                },
                new TweakDef
                {
                    Id = "krbadv-set-service-ticket-lifetime-10m",
                    Label = "Set Kerberos Service Ticket Maximum Lifetime to 600 Minutes",
                    Category = "User Account",
                    Description =
                        "Sets the maximum service ticket (TGS) lifetime to 600 minutes (10 hours), which is long enough for a business day session while limiting the window during which a stolen service ticket could be replayed against a service.",
                    Tags = ["kerberos", "service-ticket", "tgs", "lifetime", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos service ticket lifetime limited to 10 hours; limits replay window for stolen service tickets.",
                    ApplyOps = [RegOp.SetDword(SecKey, "MaxServiceAge", 600)],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "MaxServiceAge")],
                    DetectOps = [RegOp.CheckDword(SecKey, "MaxServiceAge", 600)],
                },
                new TweakDef
                {
                    Id = "krbadv-set-renew-lifetime-7d",
                    Label = "Set Kerberos Ticket Maximum Renewal Lifetime to 7 Days",
                    Category = "User Account",
                    Description =
                        "Sets the maximum TGT renewal lifetime to 7 days, after which the user must fully re-authenticate with their password or smart card rather than just renewing an existing ticket.",
                    Tags = ["kerberos", "renewal-lifetime", "tgt", "re-authentication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos TGT renewable lifetime set to 7 days; full re-auth required after 1 week.",
                    ApplyOps = [RegOp.SetDword(SecKey, "MaxRenewAge", 7)],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "MaxRenewAge")],
                    DetectOps = [RegOp.CheckDword(SecKey, "MaxRenewAge", 7)],
                },
                new TweakDef
                {
                    Id = "krbadv-log-kerberos-failures",
                    Label = "Log Kerberos Pre-Authentication Failure Events",
                    Category = "User Account",
                    Description =
                        "Enables Security audit logging for Kerberos AS exchange pre-authentication failures (EventID 4771), providing visibility into password-spraying and Kerberoasting attempts against domain accounts.",
                    Tags = ["kerberos", "pre-auth-failure", "audit", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos pre-auth failures logged (EventID 4771); password spray and Kerberoasting attempts visible.",
                    ApplyOps = [RegOp.SetDword(SecKey, "AuditPreAuthFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "AuditPreAuthFailures")],
                    DetectOps = [RegOp.CheckDword(SecKey, "AuditPreAuthFailures", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-block-unconstrained-delegation",
                    Label = "Block Accounts from Using Unconstrained Kerberos Delegation",
                    Category = "User Account",
                    Description =
                        "Enables the 'Account is sensitive and cannot be delegated' flag enforcement at policy level, blocking non-protected accounts from being marked for unconstrained delegation which allows impersonation of any user who authenticates to the delegate.",
                    Tags = ["kerberos", "delegation", "unconstrained", "impersonation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Unconstrained Kerberos delegation blocked for new accounts; existing delegation settings unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUnconstrainedDelegation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUnconstrainedDelegation")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUnconstrainedDelegation", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-disable-kerberos-telemetry",
                    Label = "Disable Kerberos Authentication Telemetry to Microsoft",
                    Category = "User Account",
                    Description =
                        "Prevents the Windows Kerberos provider from sending cipher negotiation stats, authentication failure rates, and encryption algorithm telemetry to Microsoft.",
                    Tags = ["kerberos", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos telemetry to Microsoft disabled; cipher negotiation and failure rate data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableKerberosTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableKerberosTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableKerberosTelemetry", 1)],
                },
            ];
    }

    // ── LapsPolicy ──
    private static class _LapsPolicy
    {
        private const string LapsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\LAPS";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lapspol-enable-ad-backup",
                    Label = "Configure LAPS to Back Up Password to Active Directory",
                    Category = "User Account",
                    Description = "Directs Windows LAPS to store the managed local administrator password in Active Directory DS.",
                    Tags = ["laps", "password", "active-directory", "backup", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Enables centralised credential management; requires AD DS and LAPS schema extension.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "BackupDirectory", 2)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "BackupDirectory")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "BackupDirectory", 2)],
                },
                new TweakDef
                {
                    Id = "lapspol-set-password-age-30",
                    Label = "Set LAPS Maximum Password Age to 30 Days",
                    Category = "User Account",
                    Description = "Configures the Windows LAPS managed account password to expire after a maximum of 30 days.",
                    Tags = ["laps", "password", "expiry", "rotation", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Password is automatically rotated every 30 days; no user action required.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PasswordAgeDays", 30)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PasswordAgeDays")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PasswordAgeDays", 30)],
                },
                new TweakDef
                {
                    Id = "lapspol-set-password-length-20",
                    Label = "Set LAPS Minimum Password Length to 20",
                    Category = "User Account",
                    Description = "Forces the LAPS-managed local administrator password to be at least 20 characters long.",
                    Tags = ["laps", "password", "length", "complexity", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Longer passwords improve brute-force resistance; LAPS manages them automatically.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PasswordLength", 20)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PasswordLength")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PasswordLength", 20)],
                },
                new TweakDef
                {
                    Id = "lapspol-set-password-complexity-full",
                    Label = "Set LAPS Password Complexity to Full",
                    Category = "User Account",
                    Description = "Requires the LAPS-generated password to include uppercase, lowercase, digits, and special characters.",
                    Tags = ["laps", "password", "complexity", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Value 4 = large letters + small letters + digits + special chars; maximum entropy.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PasswordComplexity", 4)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PasswordComplexity")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PasswordComplexity", 4)],
                },
                new TweakDef
                {
                    Id = "lapspol-post-auth-reset-logoff",
                    Label = "Reset LAPS Password and Log Off After Admin Use",
                    Category = "User Account",
                    Description =
                        "Automatically resets the managed local admin password and logs off the session after it is used for authentication.",
                    Tags = ["laps", "password", "post-auth", "rotation", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Value 3 = reset password + terminate managed account logon sessions; prevents credential reuse.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PostAuthenticationActions", 3)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PostAuthenticationActions")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PostAuthenticationActions", 3)],
                },
                new TweakDef
                {
                    Id = "lapspol-set-post-auth-delay-24h",
                    Label = "Set LAPS Post-Auth Reset Delay to 24 Hours",
                    Category = "User Account",
                    Description = "Delays the post-authentication password reset for 24 hours to allow admin tasks to complete before rotation.",
                    Tags = ["laps", "password", "post-auth", "delay", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Gives admins 24 hours to finish tasks before the managed account password is rotated.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PostAuthenticationResetDelay", 24)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PostAuthenticationResetDelay")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PostAuthenticationResetDelay", 24)],
                },
                new TweakDef
                {
                    Id = "lapspol-enable-ad-encryption",
                    Label = "Encrypt LAPS Password in Active Directory",
                    Category = "User Account",
                    Description = "Stores the LAPS-managed password in Active Directory using AES-256 encryption instead of plain text.",
                    Tags = ["laps", "password", "encryption", "active-directory", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Password is AES-256 encrypted at rest in AD; requires Windows Server 2016 DC or later.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "ADPasswordEncryptionEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "ADPasswordEncryptionEnabled")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "ADPasswordEncryptionEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "lapspol-enable-expiry-protection",
                    Label = "Enable LAPS Password Expiry Protection",
                    Category = "User Account",
                    Description = "Prevents the LAPS password expiry date from being set into the future by unauthorised parties.",
                    Tags = ["laps", "password", "expiry", "protection", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks attackers from extending the LAPS password lifetime to avoid rotation.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PasswordExpirationProtectionEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PasswordExpirationProtectionEnabled")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PasswordExpirationProtectionEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "lapspol-enable-audit-policy",
                    Label = "Enable LAPS Audit Policy",
                    Category = "User Account",
                    Description = "Enables Windows LAPS audit logging to track password read and update events in the Security event log.",
                    Tags = ["laps", "audit", "logging", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Records LAPS credential access events; useful for detecting unauthorised admin account usage.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "AuditPolicyEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "AuditPolicyEnabled")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "AuditPolicyEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "lapspol-set-expiry-notify-7d",
                    Label = "Notify 7 Days Before LAPS Password Expiry",
                    Category = "User Account",
                    Description = "Sends a warning notification to administrators 7 days before the LAPS-managed password expires.",
                    Tags = ["laps", "password", "expiry", "notification", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Generates an event log warning 7 days before password rotation; purely informational.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "NotifyPasswordExpiryDays", 7)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "NotifyPasswordExpiryDays")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "NotifyPasswordExpiryDays", 7)],
                },
            ];
    }

    // ── LapsSecurity ──
    private static class _LapsSecurity
    {
        private const string LapsPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LAPS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "laps-backup-to-azure-ad",
                Label = "LAPS: Back Up Local Admin Password to Azure AD / Entra ID",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["laps", "entra-id", "azure-ad", "password-backup", "admin-password"],
                Description =
                    "Sets BackupDirectory=1 in the Windows LAPS policy. "
                    + "Configures Windows LAPS to back up the managed local administrator password to "
                    + "Azure Active Directory (now Microsoft Entra ID). Values: 0=disabled, 1=Azure AD, 2=Active Directory. "
                    + "Allows IT admins to retrieve the password via the Entra ID portal without on-premises AD infrastructure.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "BackupDirectory", 1)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "BackupDirectory")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "BackupDirectory", 1)],
            },
            new TweakDef
            {
                Id = "laps-backup-to-ad",
                Label = "LAPS: Back Up Local Admin Password to Active Directory",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["laps", "active-directory", "password-backup", "admin-password", "on-premises"],
                Description =
                    "Sets BackupDirectory=2 in the Windows LAPS policy. "
                    + "Configures Windows LAPS to back up the managed local administrator password to "
                    + "on-premises Active Directory. Values: 0=disabled, 1=Azure AD/Entra, 2=AD on-premises. "
                    + "Requires the AD schema to be extended for Windows LAPS (distinct from legacy Microsoft LAPS).",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "BackupDirectory", 2)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "BackupDirectory")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "BackupDirectory", 2)],
            },
            new TweakDef
            {
                Id = "laps-max-age-14-days",
                Label = "LAPS: Set Maximum Password Age to 14 Days",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["laps", "password-age", "rotation", "admin-password"],
                Description =
                    "Sets PasswordAgeDays=14 in the Windows LAPS policy. "
                    + "Requires the local administrator password to be rotated at least every 14 days. "
                    + "The default is 30 days. Shorter rotation limits the window of exposure if a cached/leaked "
                    + "password is used in a pass-the-hash or lateral movement attack.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "PasswordAgeDays", 14)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PasswordAgeDays")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "PasswordAgeDays", 14)],
            },
            new TweakDef
            {
                Id = "laps-password-length-20",
                Label = "LAPS: Set Minimum Password Length to 20 Characters",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["laps", "password-length", "strength", "admin-password"],
                Description =
                    "Sets PasswordLength=20 in the Windows LAPS policy. "
                    + "Requires the generated local administrator password to be at least 20 characters long. "
                    + "The default is 14 characters. At 20 characters with mixed complexity, the password provides "
                    + "over 100 bits of entropy, making offline brute-force attacks computationally infeasible.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "PasswordLength", 20)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PasswordLength")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "PasswordLength", 20)],
            },
            new TweakDef
            {
                Id = "laps-password-max-complexity",
                Label = "LAPS: Require Maximum Password Complexity (All Character Types)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["laps", "password-complexity", "strength", "admin-password"],
                Description =
                    "Sets PasswordComplexity=4 in the Windows LAPS policy. "
                    + "Requires the generated password to include large letters, small letters, numbers, and "
                    + "special characters. Values: 1=large only, 2=large+small, 3=large+small+numbers, "
                    + "4=large+small+numbers+specials (maximum complexity).",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "PasswordComplexity", 4)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PasswordComplexity")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "PasswordComplexity", 4)],
            },
            new TweakDef
            {
                Id = "laps-enable-password-encryption",
                Label = "LAPS: Enable Encrypted Password Storage in Active Directory",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["laps", "encryption", "active-directory", "password-storage"],
                Description =
                    "Sets ADPasswordEncryptionEnabled=1 in the Windows LAPS policy. "
                    + "Encrypts the LAPS password before it is stored in Active Directory, using the AD computer "
                    + "object's NTDS DPAPI master key. Only authorized AD principals (admins, the computer itself) "
                    + "can decrypt the password. Requires on-premises AD with the Windows LAPS schema extension.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "ADPasswordEncryptionEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "ADPasswordEncryptionEnabled")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "ADPasswordEncryptionEnabled", 1)],
            },
            new TweakDef
            {
                Id = "laps-post-auth-reset-and-logoff",
                Label = "LAPS: Reset Password and Log Off After Admin Authentication",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["laps", "post-auth", "password-reset", "logoff", "hygiene"],
                Description =
                    "Sets PostAuthenticationActions=3 in the Windows LAPS policy. "
                    + "After the local admin account is used to authenticate (e.g., for a break-glass login), "
                    + "LAPS automatically resets the password AND logs off active sessions. "
                    + "Values: 1=reset password only, 2=logoff+reset, 3=logoff+reset+terminate processes.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "PostAuthenticationActions", 3)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PostAuthenticationActions")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "PostAuthenticationActions", 3)],
            },
            new TweakDef
            {
                Id = "laps-post-auth-delay-24h",
                Label = "LAPS: Set 24-Hour Grace Period Before Post-Auth Password Reset",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["laps", "post-auth", "delay", "grace-period"],
                Description =
                    "Sets PostAuthenticationResetDelay=24 in the Windows LAPS policy. "
                    + "Specifies how many hours Windows waits after an authentication event before triggering "
                    + "the post-authentication password rotation action. "
                    + "A 24-hour delay gives administrators time to complete maintenance work before the "
                    + "account is reset and active sessions are terminated.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "PostAuthenticationResetDelay", 24)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PostAuthenticationResetDelay")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "PostAuthenticationResetDelay", 24)],
            },
            new TweakDef
            {
                Id = "laps-encrypt-history-12",
                Label = "LAPS: Retain 12 Encrypted Previous Passwords in AD History",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["laps", "history", "encryption", "active-directory", "audit"],
                Description =
                    "Sets ADEncryptedPasswordHistorySize=12 in the Windows LAPS policy. "
                    + "Configures Windows LAPS to retain the last 12 encrypted historical passwords in "
                    + "Active Directory. Enables recovery of previously used passwords for forensic analysis "
                    + "or rolling back after an incident. Requires ADPasswordEncryptionEnabled=1.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "ADEncryptedPasswordHistorySize", 12)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "ADEncryptedPasswordHistorySize")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "ADEncryptedPasswordHistorySize", 12)],
            },
            new TweakDef
            {
                Id = "laps-disable-legacy-laps",
                Label = "LAPS: Disable Legacy Microsoft LAPS (Allow Only Windows LAPS)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["laps", "legacy", "migration", "admin-password"],
                Description =
                    "Sets LegacyMicrosoftLAPSEnabled=0 in the Windows LAPS policy. "
                    + "Disables the legacy Microsoft LAPS CSE (Client-Side Extension) when the built-in Windows "
                    + "LAPS is configured. Prevents both legacy and new LAPS from running simultaneously, which "
                    + "could cause password conflicts. Required when migrating from legacy LAPS to Windows LAPS.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "LegacyMicrosoftLAPSEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "LegacyMicrosoftLAPSEnabled")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "LegacyMicrosoftLAPSEnabled", 0)],
            },
        ];
    }

    // ── LegacyAuthPolicy ──
    private static class _LegacyAuthPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProvider";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "legauth-disable-lm-response",
                Label = "Disable LAN Manager Hash Response (LM Authentication)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "LAN Manager authentication is a decades-old protocol that uses weak DES-encrypted password hashes that are trivially cracked with modern hardware. Disabling LM authentication responses prevents Windows from responding to LM authentication challenge requests from legacy systems. LM hashes can be cracked offline in minutes using dictionary attacks or rainbow tables making stored LM credentials immediately exploitable. Windows systems should be configured to use NTLMv2 or Kerberos instead of LM for all network authentication. LM authentication may be required for compatibility with very old systems like Windows 95/98 but these systems should not be present in modern enterprise networks. Disabling LM responses forces all authentication to use stronger NTLM or Kerberos protocols that provide superior security.",
                Tags = ["lm", "authentication", "legacy", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLmResponse", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLmResponse")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLmResponse", 1)],
            },
            new TweakDef
            {
                Id = "legauth-disable-ntlm-v1",
                Label = "Disable NTLMv1 Authentication Protocol",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "NTLMv1 is an older version of the NTLM authentication protocol that lacks the security improvements added in NTLMv2. Disabling NTLMv1 forces upgrading to NTLMv2 which includes session nonces preventing credential replay attacks that work against NTLMv1. NTLMv1 is vulnerable to pass-the-hash attacks where captured NTLM hashes are replayed without knowing the actual password. Microsoft has recommended disabling NTLMv1 since Windows Vista and its continued use represents unnecessary authentication risk. NTLMv1 responses can be downgr- forced by attackers in man-in-the-middle positions to capture more easily cracked credential material. Enterprise environments should audit for remaining NTLMv1 usage and migrate legacy applications to Kerberos or NTLMv2 before disabling.",
                Tags = ["ntlm", "ntlmv1", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNtlmV1", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNtlmV1")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNtlmV1", 1)],
            },
            new TweakDef
            {
                Id = "legauth-require-ntlmv2",
                Label = "Require NTLMv2 Response Only for Network Authentication",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Requiring NTLMv2-only responses ensures that Windows systems only send NTLMv2 credentials rejecting all older LM and NTLMv1 authentication requests. NTLMv2 includes session security features like mutual authentication keys and per-session nonces that reduce credential theft and replay risks. Requiring NTLMv2 only is an effective defense against downgrade attacks that force less secure authentication protocols. Systems requiring NTLMv2-only should be tested for compatibility with servers and applications using older NTLM versions before policy enforcement. NTLMv2 requirement should be combined with session security settings to maximize the security improvement. Organizations should prefer Kerberos over NTLM where possible with NTLMv2 as the fallback for legacy compatibility scenarios.",
                Tags = ["ntlm", "ntlmv2", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireNtlmV2", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireNtlmV2")],
                DetectOps = [RegOp.CheckDword(Key, "RequireNtlmV2", 1)],
            },
            new TweakDef
            {
                Id = "legauth-disable-weak-hash-storing",
                Label = "Prevent Storage of LAN Manager Hashes in SAM Database",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "The Security Accounts Manager database can store both NTLM and LAN Manager password hashes with LM hashes being significantly weaker. Preventing LM hash storage ensures that even if the SAM database is extracted attackers only obtain NTLM hashes rather than trivially crackable LM hashes. LM hashes split passwords into 7-character chunks that can be brute-forced independently reducing cracking complexity dramatically. Removing LM hash storage means that new password changes will not generate LM hashes but existing LM hashes persist until passwords change. A password change cycle should be initiated after enabling this policy to eliminate stored LM hashes from the SAM database. This policy is effective when combined with the NTLMv2-only response requirement and LM authentication disablement.",
                Tags = ["lm-hash", "sam", "password", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoLmHash", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoLmHash")],
                DetectOps = [RegOp.CheckDword(Key, "NoLmHash", 1)],
            },
            new TweakDef
            {
                Id = "legauth-disable-ntlm-outbound",
                Label = "Restrict Outbound NTLM Authentication Requests",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Outbound NTLM authentication allows Windows systems to authenticate to remote servers using NTLM which can be exploited to capture NTLM credentials. Restricting outbound NTLM prevents Windows systems from sending NTLM responses to rogue servers set up by attackers for credential capture. NTLM credential capture attacks involve an attacker triggering authentication requests to a server they control and capturing the NTLM response for offline cracking. Restricting outbound NTLM to allowed server lists forces explicit whitelisting of servers that require NTLM authentication. Organizations should identify all servers requiring NTLM authentication before enabling this restriction to prevent service disruptions. The restriction should be set to audit mode first to identify NTLM usage before switching to denial mode.",
                Tags = ["ntlm", "outbound", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictOutboundNtlm", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictOutboundNtlm")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictOutboundNtlm", 2)],
            },
            new TweakDef
            {
                Id = "legauth-restrict-ntlm-inbound",
                Label = "Restrict Inbound NTLM Authentication to Domain Accounts",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Inbound NTLM authentication allows external systems to authenticate to this Windows server using NTLM which can be exploited in pass-the-hash attacks. Restricting inbound NTLM to domain accounts prevents local account NTLM authentication which is commonly exploited for lateral movement. Domain account NTLM authentication is subject to Kerberos validation in domain environments providing stronger authentication guarantees. Local account NTLM authentication bypasses domain controller validation making it useful for attackers with captured local credentials. Restricting inbound NTLM to domain accounts forces attackers to use Kerberos authentication which provides better monitoring and audit capabilities. Organizations should test inbound NTLM restrictions in audit mode before enforcement to identify local account dependencies.",
                Tags = ["ntlm", "inbound", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictInboundNtlm", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictInboundNtlm")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictInboundNtlm", 1)],
            },
            new TweakDef
            {
                Id = "legauth-enable-ntlm-audit",
                Label = "Enable NTLM Authentication Event Auditing",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "NTLM authentication auditing generates Windows event log entries for all NTLM authentication attempts providing visibility into NTLM usage. Enabling NTLM auditing allows security teams to identify which applications and systems are still using NTLM authentication. NTLM audit logs reveal authentication patterns that indicate pass-the-hash attacks or unauthorized lateral movement using NTLM credentials. Audit data is necessary to identify NTLM dependencies before implementing NTLM restrictions that could disrupt production services. NTLM authentication events should be forwarded to SIEM for correlation with threat intelligence and anomaly detection. Regular review of NTLM audit data helps drive migration from NTLM to Kerberos over time as dependencies are identified and resolved.",
                Tags = ["ntlm", "audit", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditNtlm", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditNtlm")],
                DetectOps = [RegOp.CheckDword(Key, "AuditNtlm", 1)],
            },
            new TweakDef
            {
                Id = "legauth-disable-basic-auth",
                Label = "Disable Basic HTTP Authentication for Network Providers",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Basic HTTP authentication transmits credentials in Base64 encoding which is trivially decoded providing plaintext username and password to network observers. Disabling basic authentication for network providers prevents credentials from being transmitted in a format that exposes them to network sniffing. Basic authentication is insecure even over HTTPS as logs and proxies may capture the authentication header containing credentials. Network providers including WebDAV implementations that support basic authentication should be updated to use Negotiate or modern token-based authentication. Disabling basic auth may break legacy applications and web services that use basic authentication but safer alternatives are available. Organizations must identify all basic authentication dependencies and migrate them before enforcing this restriction.",
                Tags = ["basic-auth", "authentication", "credentials", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBasicAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBasicAuth")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBasicAuth", 1)],
            },
            new TweakDef
            {
                Id = "legauth-disable-digest-auth",
                Label = "Disable Digest Authentication for Network Connections",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Digest authentication is a challenge-response authentication scheme that provides limited protection compared to modern authentication protocols. Disabling digest authentication prevents Windows network providers from using this older authentication mechanism for WebDAV and similar connections. Digest authentication stores passwords in a reversible format on servers requiring it making server compromise expose all user credentials. Modern web applications should use OAuth2, SAML, or Negotiate authentication rather than Basic or Digest schemes. Digest authentication is vulnerable to man-in-the-middle attacks where an attacker can downgrade or capture authentication sequences. Organizations using IIS or other web servers that rely on digest authentication should migrate to Windows Integrated Authentication or modern tokens.",
                Tags = ["digest-auth", "authentication", "credentials", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDigestAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDigestAuth")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDigestAuth", 1)],
            },
            new TweakDef
            {
                Id = "legauth-enable-extended-protection",
                Label = "Enable Extended Protection for Authentication (EPA)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Extended Protection for Authentication binds authentication tokens to the channel establishment preventing credential forwarding to unauthorized channels. Enabling EPA prevents NTLM credential relay attacks where an attacker intercepts and forwards authentication tokens to a different server. Channel binding tokens ensure that authentication credentials cannot be used against a server other than the one the client intended to authenticate to. EPA is particularly important for protecting against cross-protocol relay attacks such as NTLM relay to SMB or LDAP. Enabling EPA may impact older applications that do not support channel binding so compatibility testing is required. Microsoft recommends enabling EPA on all servers that support it as a defense-in-depth control against credential relay attacks.",
                Tags = ["epa", "authentication", "relay-protection", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableExtendedProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableExtendedProtection")],
                DetectOps = [RegOp.CheckDword(Key, "EnableExtendedProtection", 1)],
            },
        ];
    }

    // ── LocalSecurityAuthorityPolicy ──
    private static class _LocalSecurityAuthorityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
        private const string SecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string CfgKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lsapol-enable-lsa-runasppl",
                    Label = "Enable LSA Protected Process Light (RunAsPPL) Credential Guard",
                    Category = "User Account",
                    Description =
                        "Enables RunAsPPL for lsass.exe, running the Local Security Authority as a Protected Process Light, preventing credential dumping tools (Mimikatz, procdump lsass) from reading NTLM hashes and Kerberos tickets from the LSASS process.",
                    Tags = ["lsa", "runasppl", "credential-dump", "mimikatz", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "LSA RunAsPPL enabled; Mimikatz and LSASS credential dumping tools blocked from reading process memory.",
                    ApplyOps = [RegOp.SetDword(Key, "RunAsPPL", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RunAsPPL")],
                    DetectOps = [RegOp.CheckDword(Key, "RunAsPPL", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-disable-anonymous-enumeration-sam",
                    Label = "Disable Anonymous SAM Account and Share Enumeration",
                    Category = "User Account",
                    Description =
                        "Prevents anonymous network connections from enumerating local SAM accounts and security groups, blocking reconnaissance that discovers usernames for use in password spraying or brute-force attacks.",
                    Tags = ["lsa", "anonymous-enumeration", "sam", "reconnaissance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Anonymous SAM enumeration disabled; usernames not discoverable by unauthenticated network connections.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictAnonymousSAM", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictAnonymousSAM")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictAnonymousSAM", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-restrict-anonymous-access",
                    Label = "Restrict Anonymous Access to Named Pipes and Shares",
                    Category = "User Account",
                    Description =
                        "Blocks anonymous access to all named pipes and network shares, preventing unauthenticated connections that could be used for pass-the-hash attacks or to access network resources without valid credentials.",
                    Tags = ["lsa", "anonymous-access", "named-pipes", "shares", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Anonymous named pipe and share access blocked; unauthenticated CIFS/RPC connections rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictAnonymous", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictAnonymous")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictAnonymous", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-disable-wdigest-cleartext",
                    Label = "Disable WDigest Cleartext Password Caching in LSASS",
                    Category = "User Account",
                    Description =
                        "Disables the WDigest authentication provider's cleartext password caching in LSASS memory, preventing credential dumping tools from extracting reversible plaintext passwords from the WDigest cache.",
                    Tags = ["lsa", "wdigest", "cleartext-password", "mimikatz", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "WDigest cleartext caching disabled; plaintext passwords no longer extractable from LSASS memory.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "UseLogonCredential", 0)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "UseLogonCredential")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "UseLogonCredential", 0)],
                },
                new TweakDef
                {
                    Id = "lsapol-enable-lsa-audit",
                    Label = "Enable LSA Authentication Audit Logging",
                    Category = "User Account",
                    Description =
                        "Enables comprehensive Security audit logging for all LSA authentication events, including logon successes, failures, privilege escalations, and token creation, supporting SIEM-based authentication anomaly detection.",
                    Tags = ["lsa", "audit", "authentication", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "LSA authentication audit logging enabled; all logon and privilege events recorded for SIEM.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditBaseObjects", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditBaseObjects")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditBaseObjects", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-crash-on-audit-fail",
                    Label = "Crash System When Security Audit Log Is Full (CrashOnAuditFail)",
                    Category = "User Account",
                    Description =
                        "Configures LSA to crash the system with a BSOD when the Security audit log becomes full and events cannot be written, ensuring audit records are never silently dropped on high-security systems that require complete audit trails.",
                    Tags = ["lsa", "audit-fail", "crash-on-full", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "System BSOD on Security log full; complete audit trail guaranteed but availability risk if log fills. Use with large log size.",
                    ApplyOps = [RegOp.SetDword(Key, "CrashOnAuditFail", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "CrashOnAuditFail")],
                    DetectOps = [RegOp.CheckDword(Key, "CrashOnAuditFail", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-disable-legacy-auth-packages",
                    Label = "Remove Legacy Security Support Provider Packages from LSA",
                    Category = "User Account",
                    Description =
                        "Removes legacy SSPI authentication packages (msapsspc, msnsspc) from the LSA Security Packages list, preventing these deprecated packages from being loaded as SSPI providers that could be backdoored or exploited.",
                    Tags = ["lsa", "sspi", "legacy-packages", "authentication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Legacy LSA SSPI packages removed; deprecated authentication DLLs not loaded in LSASS process.",
                    ApplyOps = [RegOp.SetDword(SecKey, "DisableLegacyLSAPackages", 1)],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "DisableLegacyLSAPackages")],
                    DetectOps = [RegOp.CheckDword(SecKey, "DisableLegacyLSAPackages", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-deny-network-logon-local-accounts",
                    Label = "Deny Network Logon for Local Administrator Accounts",
                    Category = "User Account",
                    Description =
                        "Blocks local administrator accounts (SID S-1-5-113) from performing network logons (interactive pass-the-hash, NTLM relay), ensuring only domain accounts can authenticate over the network and local creds cannot be used for lateral movement.",
                    Tags = ["lsa", "network-logon", "local-admin", "lateral-movement", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Network logon denied for local administrator accounts; local account pass-the-hash lateral movement blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyNetworkLogonForLocalAccounts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyNetworkLogonForLocalAccounts")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyNetworkLogonForLocalAccounts", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-enable-token-filter-policy",
                    Label = "Enable Local Account Token Filter Policy (Full Token on Network)",
                    Category = "User Account",
                    Description =
                        "Enables LocalAccountTokenFilterPolicy which allows local admin accounts that authenticate over the network to receive a full elevated token (rather than a filtered one), enabling legitimate remote administration without requiring domain accounts. Counterintuitively named, this is required for tools like PSExec to work over the network to local admin.",
                    Tags = ["lsa", "token-filter", "local-admin", "remote-admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote =
                        "Local account token filter disabled; local admin gets full elevated token on network logon. Required for PSExec-style remote admin.",
                    ApplyOps = [RegOp.SetDword(Key, "LocalAccountTokenFilterPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LocalAccountTokenFilterPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "LocalAccountTokenFilterPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-disable-lsa-telemetry",
                    Label = "Disable LSA / Authentication Provider Telemetry to Microsoft",
                    Category = "User Account",
                    Description =
                        "Prevents the LSA and Windows authentication providers from sending authentication event rates, credential provider selection, and logon failure telemetry to Microsoft.",
                    Tags = ["lsa", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LSA telemetry to Microsoft disabled; auth event data and failure rates not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLSATelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLSATelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLSATelemetry", 1)],
                },
            ];
    }

    // ── LogonCachePolicy ──
    private static class _LogonCachePolicy
    {
        private const string Winlogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
        private const string NetlogonParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";
        private const string PolicySys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "lgncache-reduce-cached-logons",
                Label = "Logon Cache: Set Cached Domain Logon Count to 2",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Winlogon],
                Tags = ["logon", "cache", "domain", "credential", "security", "policy"],
                Description =
                    "Sets CachedLogonsCount=2 in Winlogon. Limits the number of domain credentials cached "
                    + "locally to 2. Reduces the credential footprint on the disk. "
                    + "Default: 10. Setting to 2 retains minimal offline logon capability while reducing exposure.",
                ApplyOps = [RegOp.SetString(Winlogon, "CachedLogonsCount", "2")],
                RemoveOps = [RegOp.DeleteValue(Winlogon, "CachedLogonsCount")],
                DetectOps = [RegOp.CheckString(Winlogon, "CachedLogonsCount", "2")],
            },
            new TweakDef
            {
                Id = "lgncache-disable-cached-logons",
                Label = "Logon Cache: Disable Cached Domain Logons (0 Cached)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Winlogon],
                Tags = ["logon", "cache", "domain", "no-cache", "security", "hardening"],
                Description =
                    "Sets CachedLogonsCount=0 in Winlogon. Completely disables domain credential caching. "
                    + "Users must have network connectivity to log on with domain credentials. "
                    + "Default: 10. Hardest security posture — use only in always-connected environments.",
                ApplyOps = [RegOp.SetString(Winlogon, "CachedLogonsCount", "0")],
                RemoveOps = [RegOp.DeleteValue(Winlogon, "CachedLogonsCount")],
                DetectOps = [RegOp.CheckString(Winlogon, "CachedLogonsCount", "0")],
            },
            new TweakDef
            {
                Id = "lgncache-sc-remove-lock",
                Label = "Logon Cache: Lock Workstation on Smart Card Removal",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Winlogon],
                Tags = ["logon", "smart-card", "lock", "security", "pki", "compliance"],
                Description =
                    "Sets ScRemoveOption=1 in Winlogon. Locks the workstation immediately when the smart "
                    + "card used for authentication is removed from the reader. "
                    + "Default: 0 (no action). Value 1=Lock, 2=Force Logoff. Recommended: 1 for secure workstations.",
                ApplyOps = [RegOp.SetString(Winlogon, "ScRemoveOption", "1")],
                RemoveOps = [RegOp.DeleteValue(Winlogon, "ScRemoveOption")],
                DetectOps = [RegOp.CheckString(Winlogon, "ScRemoveOption", "1")],
            },
            new TweakDef
            {
                Id = "lgncache-password-expiry-warning-14d",
                Label = "Logon Cache: Set Password Expiry Warning to 14 Days",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Winlogon],
                Tags = ["logon", "password", "expiry", "warning", "policy", "compliance"],
                Description =
                    "Sets PasswordExpiryWarning=14 in Winlogon. Shows password expiry reminder 14 days "
                    + "before the password expires at logon time. "
                    + "Default: 5 days. 14 days gives users adequate time to change before lockout.",
                ApplyOps = [RegOp.SetDword(Winlogon, "PasswordExpiryWarning", 14)],
                RemoveOps = [RegOp.DeleteValue(Winlogon, "PasswordExpiryWarning")],
                DetectOps = [RegOp.CheckDword(Winlogon, "PasswordExpiryWarning", 14)],
            },
            new TweakDef
            {
                Id = "lgncache-force-unlock-logon",
                Label = "Logon Cache: Require Domain Credential to Unlock Workstation",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Winlogon],
                Tags = ["logon", "unlock", "domain", "credential", "security", "compliance"],
                Description =
                    "Sets ForceUnlockLogon=1 in Winlogon. Requires the same domain logon credential that was "
                    + "used to lock the workstation. Local password changes are not accepted to unlock. "
                    + "Default: 0. Prevents privilege escalation via local account unlocking.",
                ApplyOps = [RegOp.SetDword(Winlogon, "ForceUnlockLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(Winlogon, "ForceUnlockLogon")],
                DetectOps = [RegOp.CheckDword(Winlogon, "ForceUnlockLogon", 1)],
            },
            new TweakDef
            {
                Id = "lgncache-netlogon-require-strong-key",
                Label = "Logon Cache: Require Strong Session Keys for Netlogon",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NetlogonParams],
                Tags = ["logon", "netlogon", "kerberos", "session-key", "security", "domain"],
                Description =
                    "Sets RequireStrongKey=1 in Netlogon Parameters. Requires 128-bit session key for "
                    + "Netlogon secure channel communications. Prevents downgrade to weaker encryption. "
                    + "Default: 0. Required for hardened AD environments.",
                ApplyOps = [RegOp.SetDword(NetlogonParams, "RequireStrongKey", 1)],
                RemoveOps = [RegOp.DeleteValue(NetlogonParams, "RequireStrongKey")],
                DetectOps = [RegOp.CheckDword(NetlogonParams, "RequireStrongKey", 1)],
            },
            new TweakDef
            {
                Id = "lgncache-netlogon-require-sign-seal",
                Label = "Logon Cache: Require Signing and Sealing of Netlogon Channel",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NetlogonParams],
                Tags = ["logon", "netlogon", "sign", "seal", "secure-channel", "domain", "security"],
                Description =
                    "Sets RequireSignOrSeal=1 in Netlogon Parameters. Requires all domain controllers to "
                    + "sign and seal all secure channel data for member machines. "
                    + "Default: 0. Protects against Netlogon MITM and downgrade attacks (CVE-2020-1472 pattern).",
                ApplyOps = [RegOp.SetDword(NetlogonParams, "RequireSignOrSeal", 1)],
                RemoveOps = [RegOp.DeleteValue(NetlogonParams, "RequireSignOrSeal")],
                DetectOps = [RegOp.CheckDword(NetlogonParams, "RequireSignOrSeal", 1)],
            },
            new TweakDef
            {
                Id = "lgncache-netlogon-seal-secure-channel",
                Label = "Logon Cache: Seal Netlogon Secure Channel When Possible",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NetlogonParams],
                Tags = ["logon", "netlogon", "seal", "encrypt", "domain", "security"],
                Description =
                    "Sets SealSecureChannel=1 in Netlogon Parameters. Encrypts all data on the Netlogon "
                    + "secure channel when supported by the domain controller. "
                    + "Default: 0. Ensures confidentiality of domain authentication traffic.",
                ApplyOps = [RegOp.SetDword(NetlogonParams, "SealSecureChannel", 1)],
                RemoveOps = [RegOp.DeleteValue(NetlogonParams, "SealSecureChannel")],
                DetectOps = [RegOp.CheckDword(NetlogonParams, "SealSecureChannel", 1)],
            },
            new TweakDef
            {
                Id = "lgncache-netlogon-sign-secure-channel",
                Label = "Logon Cache: Sign Netlogon Secure Channel When Possible",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NetlogonParams],
                Tags = ["logon", "netlogon", "sign", "integrity", "domain", "security"],
                Description =
                    "Sets SignSecureChannel=1 in Netlogon Parameters. Digitally signs all data sent over "
                    + "the Netlogon secure channel. Provides integrity verification of DC communications. "
                    + "Default: 0. Complements SealSecureChannel for full MITM protection.",
                ApplyOps = [RegOp.SetDword(NetlogonParams, "SignSecureChannel", 1)],
                RemoveOps = [RegOp.DeleteValue(NetlogonParams, "SignSecureChannel")],
                DetectOps = [RegOp.CheckDword(NetlogonParams, "SignSecureChannel", 1)],
            },
            new TweakDef
            {
                Id = "lgncache-disable-domain-password-cache",
                Label = "Logon Cache: Disable Domain Password Caching in Credential Manager",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Lsa],
                Tags = ["logon", "credential-manager", "password", "cache", "lsa", "security"],
                Description =
                    "Sets DisableDomainCreds=1 in LSA. Prevents Windows from caching domain credentials "
                    + "in the Credential Manager (Windows Vault). Applies to saved network passwords. "
                    + "Default: 0 (caching allowed). Disabling reduces persistent credential exposure.",
                ApplyOps = [RegOp.SetDword(Lsa, "DisableDomainCreds", 1)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "DisableDomainCreds")],
                DetectOps = [RegOp.CheckDword(Lsa, "DisableDomainCreds", 1)],
            },
        ];
    }

    // ── LogonGpoPolicy ──
    private static class _LogonGpoPolicy
    {
        private const string LogonSys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "logonpol-hide-last-username",
                Label = "Hide last signed-in username at logon screen (policy)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents the last interactive user's username from being displayed on the logon screen. "
                    + "DontDisplayLastUserName=1. Protects user enumeration on shared/kiosk machines.",
                Tags = ["logon", "privacy", "username", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "DontDisplayLastUserName", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "DontDisplayLastUserName")],
                DetectOps = [RegOp.CheckDword(LogonSys, "DontDisplayLastUserName", 1)],
            },
            new TweakDef
            {
                Id = "logonpol-hide-network-selection",
                Label = "Hide network selection UI at logon screen (policy)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Removes the network selection widget from the logon screen, preventing network changes before sign-in. "
                    + "DontDisplayNetworkSelectionUI=1. Reduces attack surface on shared machines.",
                Tags = ["logon", "network", "ui", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "DontDisplayNetworkSelectionUI", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "DontDisplayNetworkSelectionUI")],
                DetectOps = [RegOp.CheckDword(LogonSys, "DontDisplayNetworkSelectionUI", 1)],
            },
            new TweakDef
            {
                Id = "logonpol-hide-account-details-on-signin",
                Label = "Block users from showing account details at sign-in (policy)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents users from showing account details (email/username) on the sign-in screen. "
                    + "BlockUserFromShowingAccountDetailsOnSignin=1. Reduces personal data exposure.",
                Tags = ["logon", "account", "privacy", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "BlockUserFromShowingAccountDetailsOnSignin", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "BlockUserFromShowingAccountDetailsOnSignin")],
                DetectOps = [RegOp.CheckDword(LogonSys, "BlockUserFromShowingAccountDetailsOnSignin", 1)],
            },
            new TweakDef
            {
                Id = "logonpol-disable-arso",
                Label = "Disable Automatic Restart Sign-On (ARSO) (policy)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Disables Windows Automatic Restart Sign-On, which re-signs in the last user after an update restart. "
                    + "DisableAutomaticRestartSignOn=1. Prevents unattended desktop exposure after reboot.",
                Tags = ["logon", "arso", "restart", "autologon", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "DisableAutomaticRestartSignOn", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "DisableAutomaticRestartSignOn")],
                DetectOps = [RegOp.CheckDword(LogonSys, "DisableAutomaticRestartSignOn", 1)],
            },
            new TweakDef
            {
                Id = "logonpol-disable-startup-sound",
                Label = "Disable Windows startup sound (policy)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Disables the Windows startup sound via policy, regardless of user sound settings. "
                    + "DisableStartupSound=1. Useful in enterprise/kiosk environments.",
                Tags = ["logon", "startup", "sound", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "DisableStartupSound", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "DisableStartupSound")],
                DetectOps = [RegOp.CheckDword(LogonSys, "DisableStartupSound", 1)],
            },
            new TweakDef
            {
                Id = "logonpol-block-msa-connected-account",
                Label = "Block Microsoft Account connected users (policy)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Microsoft Account-connected users from signing in. "
                    + "NoConnectedUser=3 (block all MSA users). Values: 0=allowed, 1=no new MSA, 3=block all MSA.",
                Tags = ["logon", "msa", "account", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "NoConnectedUser", 3)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "NoConnectedUser")],
                DetectOps = [RegOp.CheckDword(LogonSys, "NoConnectedUser", 3)],
            },
            new TweakDef
            {
                Id = "logonpol-hide-locked-user-id",
                Label = "Hide locked user info on the lock screen (policy)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Controls what user information is displayed when a session is locked. "
                    + "DontDisplayLockedUserId=3 (show nothing: display name, domain, and username hidden). "
                    + "Values: 1=display name only, 2=display name+domain, 3=nothing.",
                Tags = ["logon", "lock-screen", "privacy", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "DontDisplayLockedUserId", 3)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "DontDisplayLockedUserId")],
                DetectOps = [RegOp.CheckDword(LogonSys, "DontDisplayLockedUserId", 3)],
            },
            new TweakDef
            {
                Id = "logonpol-max-device-password-failed-attempts",
                Label = "Lock device after failed sign-in attempts (policy)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Triggers a device lockout after a specified number of failed sign-in attempts. "
                    + "MaxDevicePasswordFailedAttempts=10. Default: 0 (disabled). "
                    + "Activates on tablets/convertibles with BitLocker.",
                Tags = ["logon", "lockout", "password", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "MaxDevicePasswordFailedAttempts", 10)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "MaxDevicePasswordFailedAttempts")],
                DetectOps = [RegOp.CheckDword(LogonSys, "MaxDevicePasswordFailedAttempts", 10)],
            },
            new TweakDef
            {
                Id = "logonpol-disable-lock-screen-app-notifications",
                Label = "Disable app notifications on the lock screen (policy)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents application notifications from appearing on the lock screen. "
                    + "DisableLockScreenAppNotifications=1. Default: not set. "
                    + "Reduces information disclosure before authentication.",
                Tags = ["logon", "lock-screen", "notifications", "privacy", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "DisableLockScreenAppNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "DisableLockScreenAppNotifications")],
                DetectOps = [RegOp.CheckDword(LogonSys, "DisableLockScreenAppNotifications", 1)],
            },
            new TweakDef
            {
                Id = "logonpol-hide-power-button-at-logon",
                Label = "Hide power button on logon screen (policy)",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Removes the Shut Down/Restart button from the Windows logon screen. "
                    + "HideFastUserSwitching=0 (keep switching; this key is separate). "
                    + "PowerButtonDenied=1 prevents shutdown before sign-in on kiosk machines.",
                Tags = ["logon", "power", "kiosk", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "PowerButtonDenied", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "PowerButtonDenied")],
                DetectOps = [RegOp.CheckDword(LogonSys, "PowerButtonDenied", 1)],
            },
        ];
    }

    // ── LsaProtectionPolicy ──
    private static class _LsaProtectionPolicy
    {
        private const string LsaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation";
        private const string LsaSysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsNT\CurrentVersion\Winlogon";
        private const string LsaCtrl = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\LSA";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lsapol-enable-lsa-run-as-ppl",
                    Label = "Enable LSA Run as Protected Process Light (PPL)",
                    Category = "User Account",
                    Description =
                        "Configures lsass.exe to run as a Protected Process Light (PPL). PPL enforces ELAM (Early Launch Anti-Malware) restrictions: only Microsoft-signed binaries can inject into or read lsass memory. Directly prevents Mimikatz credential dumping.",
                    Tags = ["lsa", "ppl", "credential-dump", "mimikatz", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Most impactful anti-credential-theft control available via policy. Prevents all unsigned in-memory credential extraction tools. Requires Windows 8.1+ and may conflict with unsigned AV products.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "RunAsPPL", 2)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "RunAsPPL")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "RunAsPPL", 2)],
                },
                new TweakDef
                {
                    Id = "lsapol-audit-lsass-access-attempts",
                    Label = "Audit LSASS Memory Access Attempts",
                    Category = "User Account",
                    Description =
                        "Enables audit logging of all OpenProcess calls that attempt to read lsass.exe memory. Even without PPL enforcement this detects credential-dumping tools (Mimikatz, ProcDump /ma) and logs the calling process for SIEM analysis.",
                    Tags = ["lsa", "audit", "credential-dump", "memory-access", "event-log"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Non-disruptive; adds event-log entries only. Essential for detection of credential theft attempts even before PPL is enforced.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "AuditLSASSAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "AuditLSASSAccess")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "AuditLSASSAccess", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-disable-reversible-encryption",
                    Label = "Disable Reversible Password Encryption in LSA",
                    Category = "User Account",
                    Description =
                        "Prevents Windows from storing user passwords in LSA using reversible encryption. Reversible password storage is equivalent to plaintext storage; disabling it ensures only one-way NTLM hashes are retained in the SAM database.",
                    Tags = ["lsa", "password-storage", "reversible-encryption", "sam", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Prevents reversible-cleartext password storage in SAM. Users who had reversible passwords must reset after this is applied. No operational impact if reversible encryption was never enabled.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "DisableReversibleEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "DisableReversibleEncryption")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "DisableReversibleEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-block-credential-delegation-to-unknown",
                    Label = "Block Credential Delegation to Unknown or Untrusted Servers",
                    Category = "User Account",
                    Description =
                        "Denies credential delegation (CredSSP / Kerberos constrained delegation) to servers not explicitly listed in the trusted servers allowlist. Prevents pass-the-hash relay attacks that trick the client into delegating credentials to a rogue server.",
                    Tags = ["lsa", "credential-delegation", "credssp", "relay", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Blocks credential delegation to all servers not explicitly allowlisted. Remote Desktop connections to unlisted servers will prompt for credentials rather than passing them. Maintain the delegation allowlist in GPO.",
                    RegistryKeys = [LsaKey],
                    ApplyOps = [RegOp.SetDword(LsaKey, "AllowDefCredentials", 0)],
                    RemoveOps = [RegOp.DeleteValue(LsaKey, "AllowDefCredentials")],
                    DetectOps = [RegOp.CheckDword(LsaKey, "AllowDefCredentials", 0)],
                },
                new TweakDef
                {
                    Id = "lsapol-restrict-anonymous-lsa-access",
                    Label = "Restrict Anonymous LSA Name and Account Lookups",
                    Category = "User Account",
                    Description =
                        "Prevents anonymous connections (null sessions) from enumerating LSA account names, SIDs, and local group memberships. Blocks the reconnaissance phase of account enumeration attacks.",
                    Tags = ["lsa", "anonymous-access", "null-session", "enumeration", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Prevents unauthenticated users from querying account info via null sessions. Blocks legacy management tools and scanners that rely on anonymous LSA queries.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "RestrictAnonymous", 2)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "RestrictAnonymous")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "RestrictAnonymous", 2)],
                },
                new TweakDef
                {
                    Id = "lsapol-disable-lm-hash-storage",
                    Label = "Disable LM Hash Storage in LSA Credential Store",
                    Category = "User Account",
                    Description =
                        "Permanently disables storage of LAN Manager (LM) password hashes in the LSA credential cache. LM hashes are solvable in seconds with modern GPUs; this ensures only NTLM and Kerberos hashes are retained.",
                    Tags = ["lsa", "lm-hash", "ntlm", "password-hash", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "All future password changes stop producing LM hashes. Existing hashes remain until users change passwords. Eliminates the weakest credential artifact from the credential store.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "NoLmHash", 1)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "NoLmHash")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "NoLmHash", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-enable-securechannel-sealing",
                    Label = "Require Secure Channel Data Encryption and Signing",
                    Category = "User Account",
                    Description =
                        "Forces all Netlogon secure channel traffic to be encrypted and signed. A secure channel is the authenticated tunnel between a domain member and its DC; unsigned channels can be hijacked to inject forged authentication responses.",
                    Tags = ["lsa", "netlogon", "secure-channel", "encryption", "signing"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Ensures Netlogon/DC communication is tamper-proof. Old DCs that do not support signed secure channels will refuse connections; requires Server 2012+ DCs.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "RequireSignOrSeal", 1)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "RequireSignOrSeal")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "RequireSignOrSeal", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-restrict-cached-logons",
                    Label = "Restrict Cached Domain Logon Count to 1",
                    Category = "User Account",
                    Description =
                        "Limits the number of cached domain credential sets stored in the LSA to 1 (minimum). Cached logons allow domain users to authenticate offline; a high cache count means a physical attacker can harvest multiple domain hashes from a stolen laptop.",
                    Tags = ["lsa", "cached-logon", "credential-cache", "physical-security", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Reduces cached credential exposure from offline attacks. Setting to 1 means only the most recent login is cached; users who have not logged in recently cannot authenticate offline.",
                    RegistryKeys = [LsaSysKey],
                    ApplyOps = [RegOp.SetString(LsaSysKey, "CachedLogonsCount", "1")],
                    RemoveOps = [RegOp.DeleteValue(LsaSysKey, "CachedLogonsCount")],
                    DetectOps = [RegOp.CheckString(LsaSysKey, "CachedLogonsCount", "1")],
                },
                new TweakDef
                {
                    Id = "lsapol-enable-credential-guard",
                    Label = "Enable Windows Defender Credential Guard (Isolated LSA)",
                    Category = "User Account",
                    Description =
                        "Enables Credential Guard, which runs the LSA credential store inside a secure Hyper-V isolated container (VSM). Even if the host OS is fully compromised lsass cannot be dumped because credentials live in a separate VM-protected memory region.",
                    Tags = ["lsa", "credential-guard", "vsm", "secure-enclave", "advanced"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Most powerful LSA credential protection; renders Mimikatz-class attacks ineffective. Requires UEFI Secure Boot + VBS + HVCI. Check hardware compatibility before deploying.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "LsaCfgFlags", 1)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "LsaCfgFlags")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "LsaCfgFlags", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-block-wdigest-plaintext-creds",
                    Label = "Block WDigest from Storing Plaintext Credentials in LSASS",
                    Category = "User Account",
                    Description =
                        "Disables WDigest authentication protocol caching in LSASS memory. WDigest was designed for HTTP Digest authentication and cached plaintext-equivalent credentials in LSASS; attackers (Mimikatz sekurlsa::wdigest) can extract these.",
                    Tags = ["lsa", "wdigest", "plaintext", "credential-cache", "mimikatz"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Eliminates plaintext credential storage in LSASS with zero functional impact for modern Windows. WDigest is only needed by systems running very old IIS Digest authentication — rare in practice.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "UseLogonCredential", 0)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "UseLogonCredential")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "UseLogonCredential", 0)],
                },
            ];
    }

    // ── PasswordlessSignInPolicy ──
    private static class _PasswordlessSignInPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "pwdless-enforce-whfb",
                    Label = "Enforce Windows Hello for Business Enrollment",
                    Category = "User Account",
                    Description =
                        "Requires all users to enroll in Windows Hello for Business during first sign-in, enforcing passwordless primary authentication and deprecating the use of traditional passwords for Windows sign-in.",
                    Tags = ["whfb", "passwordless", "windows-hello", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "WHfB enrollment required; users must set up biometric or PIN to complete first sign-in after enrollment period.",
                    ApplyOps = [RegOp.SetDword(Key, "Enabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                    DetectOps = [RegOp.CheckDword(Key, "Enabled", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-require-tpm",
                    Label = "Require TPM for Windows Hello for Business",
                    Category = "User Account",
                    Description =
                        "Mandates that WHfB private keys are protected by the device TPM, preventing WHfB credentials from being stored in software (file-based storage) where they could be exported.",
                    Tags = ["whfb", "tpm", "passwordless", "windows-hello", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "WHfB requires TPM; devices without TPM 2.0 cannot enroll in WHfB.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSecurityDevice", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSecurityDevice")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSecurityDevice", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-disable-password-fallback",
                    Label = "Disable Password Fallback for WHfB Sign-In",
                    Category = "User Account",
                    Description =
                        "Prevents users from falling back to password authentication when WHfB is available, forcing passwordless primary sign-in and eliminating the password as a secondary path attackers could target.",
                    Tags = ["whfb", "passwordless", "password-fallback", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Password fallback blocked for WHfB; users must use biometric or PIN even if they remember their password.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePasswordFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordFallback")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePasswordFallback", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-enable-fido2-keys",
                    Label = "Enable FIDO2 Security Key Sign-In",
                    Category = "User Account",
                    Description =
                        "Enables FIDO2 hardware security keys (YubiKey, Titan key, etc.) as a credential type for Windows sign-in alongside WHfB, allowing phishing-resistant passwordless authentication from the lock screen.",
                    Tags = ["fido2", "security-key", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "FIDO2 security keys accepted at Windows lock screen; users can log in with a hardware key.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableFIDO2SecurityKeys", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableFIDO2SecurityKeys")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableFIDO2SecurityKeys", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-disable-convenience-pin",
                    Label = "Disable Convenience PIN (Non-WHfB) for Local Accounts",
                    Category = "User Account",
                    Description =
                        "Disables the simple convenience PIN for local accounts that does not benefit from WHfB's asymmetric key protection, forcing WHfB PIN (TPM-backed) for all PIN sign-in scenarios.",
                    Tags = ["whfb", "convenience-pin", "local-account", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Convenience PIN disabled; PIN sign-in only available through WHfB with TPM backing.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowUnprovisionedPins", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowUnprovisionedPins")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowUnprovisionedPins", 0)],
                },
                new TweakDef
                {
                    Id = "pwdless-block-phone-sign-in",
                    Label = "Block Phone/Companion Device Sign-In",
                    Category = "User Account",
                    Description =
                        "Disables the companion device (phone-based Windows Hello sign-in) framework, preventing authentication via Bluetooth phone approval where physical possession of the phone is the only factor.",
                    Tags = ["whfb", "phone-sign-in", "companion-device", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Phone/companion-device sign-in disabled; must use device-local WHfB or security key.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePhoneSignIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneSignIn")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePhoneSignIn", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-require-mfa-for-whfb-provision",
                    Label = "Require MFA During WHfB Provisioning",
                    Category = "User Account",
                    Description =
                        "Requires multi-factor authentication during the WHfB provisioning ceremony, ensuring that only users who have already authenticated with a second factor can register WHfB credentials.",
                    Tags = ["whfb", "mfa", "provisioning", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "MFA required during WHfB setup; single-factor users cannot provision WHfB credentials.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireMFAForProvisioning", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireMFAForProvisioning")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireMFAForProvisioning", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-block-whfb-on-unmanaged",
                    Label = "Block WHfB Enrollment on Unmanaged Devices",
                    Category = "User Account",
                    Description =
                        "Prevents Windows Hello for Business enrollment on devices not enrolled in an MDM policy (Intune/SCCM), ensuring WHfB credentials are only provisioned on corp-managed endpoints.",
                    Tags = ["whfb", "mdm", "managed-device", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WHfB blocked on unmanaged devices; enrollment only succeeds after MDM enrolment.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockEnrollmentOnUnmanagedDevice", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockEnrollmentOnUnmanagedDevice")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockEnrollmentOnUnmanagedDevice", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-disable-whfb-personal",
                    Label = "Disable WHfB for Personal Microsoft Accounts",
                    Category = "User Account",
                    Description =
                        "Prevents Windows Hello from being used for personal Microsoft account sign-in, restricting WHfB to work or school accounts only and preventing personal-account credential leakage.",
                    Tags = ["whfb", "personal-account", "microsoft-account", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WHfB disabled for personal MSA; Windows Hello sign-in restricted to Azure AD accounts.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePersonalAccountPassport", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalAccountPassport")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePersonalAccountPassport", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-audit-whfb-provisioning",
                    Label = "Enable Audit Logging for WHfB Provisioning Events",
                    Category = "User Account",
                    Description =
                        "Enables security audit events for all WHfB provisioning, re-provisioning, and credential deletion events, providing traceability for passwordless credential lifecycle management.",
                    Tags = ["whfb", "audit-log", "provisioning", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WHfB credential lifecycle events logged in Security event log.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditProvisioningEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditProvisioningEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditProvisioningEvents", 1)],
                },
            ];
    }

    // ── SmartCardCredentialsPolicy ──
    private static class _SmartCardCredentialsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";
        private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sccredpol-allow-certificates-with-no-extended-key-usage",
                    Label = "SC Credentials: Allow Smart Card Certificates Without Extended Key Usage for Logon",
                    Category = "User Account",
                    Description =
                        "Sets AllowCertificatesWithNoEKU=0 in Smart Card Credential Provider policy. Prevents smart card certificates without an Extended Key Usage (EKU) extension — or with an EKU that doesn't include Client Authentication (1.3.6.1.5.5.7.3.2) — from being used for Windows logon. "
                        + "Smart card certificates without an EKU or with an all-inclusive EKU (Any Purpose) are certificates that were issued without specifying a legitimate use constraint. Such certificates are typically misconfigured CA root certificates or test certificates. If Windows allows logon with any certificate present on a smart card regardless of EKU, an attacker who compromises a user's smart card PIN and inserts a root CA certificate or code signing certificate into the card can attempt logon with the inappropriate certificate. Requiring Client Authentication EKU ensures only purpose-constrained logon certificates can authenticate to interactive sessions.",
                    Tags = ["sccredpol", "smart-card", "eku", "certificate", "logon", "client-auth"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Smart card certificates must have Client Authentication EKU for interactive logon. Misconfigured test certs or CA-root certs cannot authenticate.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowCertificatesWithNoEKU", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowCertificatesWithNoEKU")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowCertificatesWithNoEKU", 0)],
                },
                new TweakDef
                {
                    Id = "sccredpol-enforce-certificate-time-validity",
                    Label = "SC Credentials: Reject Expired Smart Card Certificates from Logon",
                    Category = "User Account",
                    Description =
                        "Sets EnforceCAExpiry=1 in Smart Card Credential Provider policy. Enforces certificate validity period checking — prevents Windows from accepting smart card certificates for logon that have expired or whose issuing CA certificate chain has expired. By default, Windows may allow logon with expired smart card certificates in some scenarios (offline cached logon) if the certificate was previously valid. "
                        + "Expired certificates represent an operational risk in smart card deployments: when a user's smart card certificate expires but the card PIN remains valid, Windows may continue to accept the card for domain logon relying on cached credentials — even though the PKI infrastructure considers the certificate expired. An attacker who obtains an expired certificate and the corresponding private key (from a compromised card) can attempt offline certificate logon. EnforceCAExpiry=1 ensures the current certificate validity timestamp is always checked, preventing expired certificate acceptance even in cached credential scenarios.",
                    Tags = ["sccredpol", "smart-card", "certificate-expiry", "ca-expiry", "validity", "pki"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Expired smart card certificates rejected. Users with expired certificates must renew before interactive logon works. Ensure certificate renewal reminders are in place.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceCAExpiry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceCAExpiry")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceCAExpiry", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-filter-duplicate-certificates",
                    Label = "SC Credentials: Filter Duplicate Smart Card Certificates Shown in Logon Picker",
                    Category = "User Account",
                    Description =
                        "Sets FilterDuplicateCerts=1 in Smart Card Credential Provider policy. When a smart card contains multiple certificates with the same Subject and public key (e.g., during certificate renewal where both old and new certificates co-exist on the card), this setting shows only the most recently issued certificate in the Windows logon certificate picker, preventing user confusion from duplicate entries. "
                        + "During smart card certificate lifecycle management, cards frequently transition through a state where both the old (near-expired) and new (freshly issued) certificates are on the card simultaneously — to allow the renewal to proceed without requiring the user to surrender their card. The Windows logon certificate picker displays all certificates, presenting two identical-looking entries to the user. Users who select the expired certificate will experience logon failures. FilterDuplicateCerts reduces the duplicate entries to one (the most recent), eliminating this user experience issue.",
                    Tags = ["sccredpol", "smart-card", "duplicate-certificate", "certificate-renewal", "logon-picker"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Duplicate smart card certificates filtered in logon picker. Only most recently issued certificate shown when multiple share the same subject.",
                    ApplyOps = [RegOp.SetDword(Key, "FilterDuplicateCerts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "FilterDuplicateCerts")],
                    DetectOps = [RegOp.CheckDword(Key, "FilterDuplicateCerts", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-force-read-all-certificates",
                    Label = "SC Credentials: Force Reading All Certificates from Smart Card (Not Just Root/Signing)",
                    Category = "User Account",
                    Description =
                        "Sets ForceReadingAllCertificates=1 in Smart Card Credential Provider policy. Forces Windows to read all certificates stored on the smart card during authentication enumeration, rather than only examining the first matching certificate. Some cards store certificate-based logon credentials on non-default slots or with non-standard EKU ordering — without ForceReadingAllCertificates, Windows may skip valid authentication certificates. "
                        + "Smart card credential providers have an optimisation that stops scanning the card after finding the first usable certificate. On cards with multiple valid Client Authentication certificates (multi-profile cards, cards issued by different CAs for different resource domains), the optimisation may select a certificate for a different trust domain, causing failed authentication. ForceReadingAllCertificates ensures the complete certificate set is enumerated and the credential provider selects the certificate with the best chain match for the current domain.",
                    Tags = ["sccredpol", "smart-card", "certificate-enumeration", "multi-profile", "credential-provider"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "All smart card certificates read and enumerated. Slight performance increase per logon attempt; negligible on modern smart card readers.",
                    ApplyOps = [RegOp.SetDword(Key, "ForceReadingAllCertificates", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ForceReadingAllCertificates")],
                    DetectOps = [RegOp.CheckDword(Key, "ForceReadingAllCertificates", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-require-smart-card-for-logon",
                    Label = "SC Credentials: Require Smart Card for Interactive Logon (Disable Password-Based Logon)",
                    Category = "User Account",
                    Description =
                        "Sets ScForceOption=1 in Windows System policy. Requires users to authenticate with a smart card for interactive (local and Remote Desktop) logon. Password-based interactive logon is disabled. This setting is the full enforcement of a smart card-mandatory authentication policy — ensuring that physical possession of the smart card is required for every interactive logon event, eliminating password-based bypass paths. "
                        + "Password-based logon as a fallback for smart card environments creates a persistent weak authentication path: users who 'lose' their smart card can fall back to passwords, which are substantially easier to steal via phishing or shoulder surfing than compromising a physical authentication token plus PIN. In high-assurance environments (financial trading, government classified systems, nuclear facility IT, PCI DSS Level 1), all interactive logon must be protected by a physical authentication factor. ScForceOption=1 eliminates the password fallback and enforces the physical factor requirement absolutely.",
                    Tags = ["sccredpol", "smart-card", "force-logon", "disable-password-logon", "mfa", "high-assurance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 2,
                    ImpactNote =
                        "BREAKING: Password interactive logon fully disabled. Smart card REQUIRED for all logon. Ensure all users have working smart cards and readers before deployment. Service accounts need smartcard exemption.",
                    ApplyOps = [RegOp.SetDword(SysKey, "ScForceOption", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "ScForceOption")],
                    DetectOps = [RegOp.CheckDword(SysKey, "ScForceOption", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-enable-smart-card-lock-on-removal",
                    Label = "SC Credentials: Lock Workstation Automatically When Smart Card is Removed",
                    Category = "User Account",
                    Description =
                        "Sets SmartCardRemovalOption=1 in Windows System policy. Automatically locks the workstation when the user removes their smart card from the reader, replacing the 'no action' default. Ensures the workstation is immediately locked when the user physically departs (smart card is typically in their lanyard or pocket which they take with them). "
                        + "Smart card removal detection is a behavioural lock triggered by physical possession of the authentication token. The security premise: a person who removes their smart card from the reader is physically leaving the workstation. Without removal lock, the authenticated session remains unlocked and accessible to anyone who approaches the workstation during the user's brief absence (printer, coffee, restroom). SmartCardRemovalOption=1 means the session locks within seconds of card removal — the physical authentication token acts as a proximity-based session lock device.",
                    Tags = ["sccredpol", "smart-card", "removal-lock", "session-lock", "physical-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Workstation locks immediately on smart card removal. Users who briefly remove their card for any reason will need to re-insert and re-authenticate.",
                    ApplyOps = [RegOp.SetDword(SysKey, "SmartCardRemovalOption", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "SmartCardRemovalOption")],
                    DetectOps = [RegOp.CheckDword(SysKey, "SmartCardRemovalOption", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-disable-smart-card-credential-caching",
                    Label = "SC Credentials: Disable Windows Cached Credentials for Smart Card Logons",
                    Category = "User Account",
                    Description =
                        "Sets DisableSmartCardLogonCheck=0 in Smart Card Credential Provider policy. Ensures Windows performs a full smart card authentication challenge on every logon attempt — disabling any cached credential shortcut paths that might allow logon without re-validating the current smart card state against the DC. "
                        + "Cached credential logon for smart card authentication creates an inconsistency: the cached domain credential may be valid even after the smart card certificate has been revoked (e.g., following employee termination or card loss). If Windows allows cached credential logon for smart card sessions, a terminated employee's workstation retains the logon capability for up to the domain cache lifetime (default 10 cached logons). Ensuring full smart card validation on each logon forces certificate revocation to be effective immediately — revoked smart cards are rejected on first logon attempt after CRL update.",
                    Tags = ["sccredpol", "smart-card", "credential-cache", "revocation", "crl", "terminated-employee"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Full smart card DC validation required. Offline logon (no DC reachable) requires network connectivity. Deploy alongside always-on VPN for remote workers.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSmartCardLogonCheck", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSmartCardLogonCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSmartCardLogonCheck", 0)],
                },
                new TweakDef
                {
                    Id = "sccredpol-enable-smart-card-puk-logging",
                    Label = "SC Credentials: Enable Smart Card PUK/PIN Operation Logging",
                    Category = "User Account",
                    Description =
                        "Sets EnableSmartCardLogonLogging=1 in Smart Card Credential Provider policy. Enables logging of smart card PIN entry events, PUK (PIN Unblocking Key) operations, and certificate selection events to the Windows Application event log. PIN operation logging provides an audit trail of smart card authentication activity at the workstation — enabling detection of PIN brute-force attempts (excessive failed PIN entries), card blocking events (PUK operation triggered), and certificate selection anomalies. "
                        + "Smart card PIN brute-force attacks are rate-limited by card hardware (typically 3-10 failed attempts before card lockout), but without logging, an attacker who attempts multiple combinations across the threshold boundary and reinserts the card leaves no system event trace. Smart card logging events can be collected by SIEM, enabling detection of cards that are being tested for PIN guessing (rapid sequence of failed PIN events at an unexpected workstation), identifying potentially compromised or stolen cards before the card locks.",
                    Tags = ["sccredpol", "smart-card", "logging", "pin-brute-force", "puk", "audit"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Smart card PIN and PUK events logged to Application event log. SIEM collection of card-specific events enables PIN brute-force detection.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableSmartCardLogonLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableSmartCardLogonLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableSmartCardLogonLogging", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-restrict-to-root-trusted-certificates",
                    Label = "SC Credentials: Restrict Smart Card Logon to Root-CA Trusted Certificates Only",
                    Category = "User Account",
                    Description =
                        "Sets RootCA=1 in Smart Card Credential Provider policy. Restricts smart card logon to only accept certificates that chain to a root CA in the machine's Trusted Root Certification Authorities store — preventing certificates issued by intermediate-only CAs or enterprise subordinate CAs whose root is not in the machine trust store from being used for logon. "
                        + "In multi-forest or partner organisation environments, smart cards issued by external PKI hierarchies may be physically interoperable (same card form factor, compatible reader drivers) but should not grant logon access to the local domain unless their issuing CA root is explicitly trusted. Without RootCA=1, certificates from any technically valid PKI chain — including self-signed certificates added to a card by an attacker — could be used for logon. Restricting to root-CA-trusted certs ensures the local domain trust policy governs which PKI hierarchies are authorised for smart card authentication.",
                    Tags = ["sccredpol", "smart-card", "root-ca", "trust", "pki", "cross-forest"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Smart card certs must chain to machine trust store root CA. Self-signed and untrusted-root certificates rejected for logon.",
                    ApplyOps = [RegOp.SetDword(Key, "RootCA", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RootCA")],
                    DetectOps = [RegOp.CheckDword(Key, "RootCA", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-enable-integrated-unblock",
                    Label = "SC Credentials: Enable Integrated Smart Card Unblock Screen at Logon",
                    Category = "User Account",
                    Description =
                        "Sets EnableIntegratedUnblock=1 in Smart Card Credential Provider policy. Enables the Windows integrated smart card unblock screen — presented at the Ctrl+Alt+Del logon screen when a smart card's PIN is blocked (after exceeding the incorrect PIN attempt limit). The integrated unblock screen allows users to unblock their card at the logon screen using PUK without requiring a separate unblock tool or helpdesk intervention. "
                        + "Without integrated unblock, a user whose card PIN is blocked must call the IT helpdesk, be issued a temporary PUK, and use a separate smart card management utility to unblock the card. This process typically takes 15–60 minutes depending on helpdesk availability. The integrated unblock screen presents the PUK entry interface directly at the Windows logon screen — the user provides their PUK and new PIN, the card is immediately unblocked, and logon proceeds. EnableIntegratedUnblock reduces helpdesk call volume for card lockouts by eliminating the manual unblock workflow.",
                    Tags = ["sccredpol", "smart-card", "unblock", "puk", "helpdesk", "user-experience"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Smart card unblock screen shown at Windows logon when PIN is blocked. Users can self-service PUK entry. Reduces helpdesk calls for locked cards.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableIntegratedUnblock", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableIntegratedUnblock")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableIntegratedUnblock", 1)],
                },
            ];
    }

    // ── SmartCardCredProvPolicy ──
    private static class _SmartCardCredProvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "scprov-block-no-eku-certs",
                    Label = "Block Smart Card Certs Without EKU",
                    Category = "User Account",
                    Description =
                        "Blocks smart card certificates that lack Extended Key Usage (EKU) extensions from being accepted for logon. Prevents improperly issued certificates from authenticating. Default: 1 (allow). Recommended: 0 (block).",
                    Tags = ["smart-card", "pki", "eku", "certificate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Prevents authentication with malformed or incorrectly issued smart card certificates lacking EKU.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowCertificatesWithNoEKU", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowCertificatesWithNoEKU")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowCertificatesWithNoEKU", 0)],
                },
                new TweakDef
                {
                    Id = "scprov-block-signature-only-keys",
                    Label = "Block Signature-Only Smart Card Keys",
                    Category = "User Account",
                    Description =
                        "Prevents smart cards with signature-only keys from being used for interactive logon. Signature keys should not be used for authentication. Default: 1 (allow). Recommended: 0 (block).",
                    Tags = ["smart-card", "pki", "signature", "key-usage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enforces key usage separation; signature keys cannot be used for authentication.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowSignatureOnlyKeys", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowSignatureOnlyKeys")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowSignatureOnlyKeys", 0)],
                },
                new TweakDef
                {
                    Id = "scprov-block-time-invalid-certs",
                    Label = "Block Expired Smart Card Certificates",
                    Category = "User Account",
                    Description =
                        "Prevents authentication using time-invalid (expired or not yet valid) smart card certificates. Enforces certificate lifecycle compliance. Default: 1 (allow). Recommended: 0 (block).",
                    Tags = ["smart-card", "pki", "expiry", "certificate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Ensures all authenticating certificates are within their validity period.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowTimeInvalidCertificates", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowTimeInvalidCertificates")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowTimeInvalidCertificates", 0)],
                },
                new TweakDef
                {
                    Id = "scprov-enumerate-ecc-certs",
                    Label = "Enumerate ECC Certificates by Default",
                    Category = "User Account",
                    Description =
                        "Enables enumeration of elliptic-curve cryptography certificates on smart cards by default. Required when the organisation uses ECDSA/ECDH smart card certificates. Default: 0. Recommended: 1 when ECC certs are deployed.",
                    Tags = ["smart-card", "ecc", "pki", "enumeration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables ECC-issued certificates on smart cards to appear in the logon picker.",
                    ApplyOps = [RegOp.SetDword(Key, "EnumerateECCCerts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnumerateECCCerts")],
                    DetectOps = [RegOp.CheckDword(Key, "EnumerateECCCerts", 1)],
                },
                new TweakDef
                {
                    Id = "scprov-filter-dup-certs",
                    Label = "Filter Duplicate Logon Certificates",
                    Category = "User Account",
                    Description =
                        "De-duplicates certificates shown in the smart card logon picker when a card carries multiple identical certificates. Prevents UI confusion during logon. Default: 0. Recommended: 1.",
                    Tags = ["smart-card", "duplicate", "certificate", "logon", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Cosmetic improvement; removes duplicate certificate entries from the logon picker.",
                    ApplyOps = [RegOp.SetDword(Key, "FilterDuplicateCerts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "FilterDuplicateCerts")],
                    DetectOps = [RegOp.CheckDword(Key, "FilterDuplicateCerts", 1)],
                },
                new TweakDef
                {
                    Id = "scprov-force-read-all-certs",
                    Label = "Force Reading All Smart Card Certificates",
                    Category = "User Account",
                    Description =
                        "Forces the system to read all certificates from a smart card rather than stopping at the first valid one. Ensures complete certificate inventory for logon selection. Default: 0. Recommended: 1 for multi-cert cards.",
                    Tags = ["smart-card", "certificate", "enumeration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Slightly increases smart card logon time; ensures all certs on the card are available.",
                    ApplyOps = [RegOp.SetDword(Key, "ForceReadingAllCertificates", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ForceReadingAllCertificates")],
                    DetectOps = [RegOp.CheckDword(Key, "ForceReadingAllCertificates", 1)],
                },
                new TweakDef
                {
                    Id = "scprov-no-reverse-subject",
                    Label = "Normalise Certificate Subject Display Order",
                    Category = "User Account",
                    Description =
                        "Prevents the credential provider from reversing certificate subject field order in the logon UI. Ensures consistent CN/OU display regardless of CA issuance order. Default: not set. Recommended: 0 (normal order).",
                    Tags = ["smart-card", "subject", "display", "certificate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Display normalisation only; no functional security impact on authentication.",
                    ApplyOps = [RegOp.SetDword(Key, "ReverseSubject", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ReverseSubject")],
                    DetectOps = [RegOp.CheckDword(Key, "ReverseSubject", 0)],
                },
                new TweakDef
                {
                    Id = "scprov-suppress-x509-hints",
                    Label = "Suppress X.509 Certificate Hint Display",
                    Category = "User Account",
                    Description =
                        "Suppresses X.509 certificate hint prompts shown when multiple certificates are available during smart card logon. Reduces UI noise in managed environments. Default: 1. Recommended: 0.",
                    Tags = ["smart-card", "x509", "hint", "logon", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removes extra X.509 hint dialogs during smart card authentication.",
                    ApplyOps = [RegOp.SetDword(Key, "X509HintsNeeded", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "X509HintsNeeded")],
                    DetectOps = [RegOp.CheckDword(Key, "X509HintsNeeded", 0)],
                },
                new TweakDef
                {
                    Id = "scprov-disallow-plaintext-pin",
                    Label = "Disallow Plaintext Smart Card PIN Transmission",
                    Category = "User Account",
                    Description =
                        "Prevents smart card PINs from being returned or transmitted in clear text by the Credential Manager. Critical for preventing PIN interception on hosts with memory inspection. Default: 0. Recommended: 1.",
                    Tags = ["smart-card", "pin", "plaintext", "credential", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Prevents PIN interception; may break legacy applications that depend on plaintext PIN access.",
                    ApplyOps = [RegOp.SetDword(Key, "DisallowPlaintextPin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisallowPlaintextPin")],
                    DetectOps = [RegOp.CheckDword(Key, "DisallowPlaintextPin", 1)],
                },
                new TweakDef
                {
                    Id = "scprov-logon-hours-notify",
                    Label = "Enable Logon Hours Change Notification",
                    Category = "User Account",
                    Description =
                        "Notifies users when their allowed logon hours are about to expire or have changed, using smart card credential context. Helps users save work before forced logoff. Default: 0. Recommended: 1.",
                    Tags = ["smart-card", "logon-hours", "notification", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Improves user experience in environments with logon-hour restrictions.",
                    ApplyOps = [RegOp.SetDword(Key, "LogonHoursNotificationEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogonHoursNotificationEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "LogonHoursNotificationEnabled", 1)],
                },
            ];
    }

    // ── WebAuthnPolicy ──
    private static class _WebAuthnPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WebAuthn";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wauthn-disable-touch-id-fallback",
                Label = "Disable WebAuthn Biometric Fallback to PIN",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableBiometricFallback=1 in the WebAuthn policy key. Prevents the "
                    + "Windows Hello FIDO2 implementation from falling back to a PIN when the "
                    + "biometric authenticator (fingerprint/face) is unavailable. Silent PIN "
                    + "fallback bypasses the stronger biometric factor and can be induced by "
                    + "covering the sensor. Administrators can enforce biometric-only "
                    + "authentication by disabling the fallback. "
                    + "Default: 0. Recommended: 1 in high-assurance environments.",
                Tags = ["webauthn", "biometric", "fallback", "fido2", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBiometricFallback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBiometricFallback")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBiometricFallback", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-require-enterprise-attestation",
                Label = "Require Enterprise Attestation for FIDO Keys",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets RequireEnterpriseAttestation=1 in the WebAuthn policy key. Forces "
                    + "FIDO2 authenticators to provide enterprise attestation statements that "
                    + "include the device's serial number and enterprise-registered key. "
                    + "This allows the relying party to verify that only managed hardware "
                    + "keys are used for authentication, preventing consumer FIDO2 tokens "
                    + "from authenticating against enterprise resources. "
                    + "Default: 0. Recommended: 1 in enterprise environments.",
                Tags = ["webauthn", "attestation", "enterprise", "fido2", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireEnterpriseAttestation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireEnterpriseAttestation")],
                DetectOps = [RegOp.CheckDword(Key, "RequireEnterpriseAttestation", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-cross-origin-auth",
                Label = "Disable Cross-Origin WebAuthn Authentication",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets DisableCrossOriginAuth=1 in the WebAuthn policy key. Prevents "
                    + "browser-based WebAuthn from completing authentication ceremonies where "
                    + "the requesting origin does not match the registered relying party ID. "
                    + "Cross-origin authentication is the basis for credential phishing via "
                    + "proxied login pages that relay FIDO2 assertions to the legitimate site. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "crossorigin", "fido2", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCrossOriginAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCrossOriginAuth")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCrossOriginAuth", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-password-auth-fallback",
                Label = "Disable WebAuthn Password Authentication Fallback",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets DisablePasswordFallback=1 in the WebAuthn policy key. Prevents "
                    + "Windows Hello and FIDO2 flows from offering a password sign-in link "
                    + "when a passkey authentication attempt fails. The password fallback "
                    + "silently downgrades the authentication assurance level and is commonly "
                    + "exploited via credential stuffing after an initial FIDO2 probing failure. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "password", "fallback", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePasswordFallback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordFallback")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePasswordFallback", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-cloud-passkey-sync",
                Label = "Disable Cloud Passkey Sync",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets DisableCloudPasskeySync=1 in the WebAuthn policy key. Prevents "
                    + "Windows Hello from backing up passkey private keys to the Microsoft "
                    + "cloud for recovery and cross-device sync. Cloud-synced passkeys "
                    + "compromise the hardware-bound security model of FIDO2: the private "
                    + "key should never leave the authenticating device. "
                    + "Default: 0. Recommended: 1 in high-security environments.",
                Tags = ["webauthn", "passkey", "sync", "cloud", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudPasskeySync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudPasskeySync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudPasskeySync", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-webauthn-telemetry",
                Label = "Disable WebAuthn Telemetry",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableWebAuthnTelemetry=1 in the WebAuthn policy key. Prevents "
                    + "the Windows WebAuthn API from sending usage events including registration "
                    + "attempts, authentication ceremony outcomes, and authenticator model data "
                    + "to Microsoft's telemetry endpoints. Authenticator model data can "
                    + "fingerprint the specific security key hardware in use. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWebAuthnTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWebAuthnTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWebAuthnTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-security-key-enrollment",
                Label = "Block Unauthorised Security Key Enrollment",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableSecurityKeyEnrollment=1 in the WebAuthn policy key. Prevents "
                    + "standard users from enrolling new FIDO2 security keys in Windows Hello "
                    + "without administrator approval. Unrestricted enrollment allows any "
                    + "physical key to be registered on a managed machine, potentially "
                    + "granting persistent hardware-authenticated access to an attacker who "
                    + "briefly had physical access. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "enrollment", "securitykey", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSecurityKeyEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSecurityKeyEnrollment")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSecurityKeyEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-enforce-user-verification",
                Label = "Enforce User Verification for All WebAuthn Calls",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets EnforceUserVerification=1 in the WebAuthn policy key. Forces the "
                    + "Windows WebAuthn stack to set the UV (user verification) flag to "
                    + "required for every authentication call, overriding relying party "
                    + "requests that set UV to preferred or discouraged. Applications that "
                    + "skip user verification inherit the trust from proximity alone without "
                    + "requiring a PIN or biometric factor. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "verification", "fido2", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceUserVerification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceUserVerification")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceUserVerification", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-nfc-transport",
                Label = "Disable NFC Transport for FIDO2 Keys",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableNfcTransport=1 in the WebAuthn policy key. Prevents the "
                    + "Windows FIDO2 client from using NFC as a transport channel for security "
                    + "key communication. NFC has a shorter effective range than USB but "
                    + "NFC relay attacks are practical up to 100 m with commodity hardware. "
                    + "Restricting keys to USB-A/C contact transports eliminates this attack "
                    + "surface. Default: 0. Recommended: 1 where NFC keys are not mandated.",
                Tags = ["webauthn", "nfc", "transport", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNfcTransport", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNfcTransport")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNfcTransport", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-bluetooth-transport",
                Label = "Disable Bluetooth Transport for FIDO2 Keys",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableBluetoothTransport=1 in the WebAuthn policy key. Disables "
                    + "Bluetooth Low Energy as a FIDO2 authenticator transport. BLE-based "
                    + "authenticators (e.g., phone-as-key) are vulnerable to Bluetooth relay "
                    + "attacks where the attacker proxies BLE advertisements to extend the "
                    + "effective range of the authenticator beyond the user's awareness. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "bluetooth", "transport", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBluetoothTransport", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBluetoothTransport")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBluetoothTransport", 1)],
            },
        ];
    }

    // ── WhfbPinPolicy ──
    private static class _WhfbPinPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\PINComplexity";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "whfbpin-set-minimum-length-8",
                    Label = "Set WHfB PIN Minimum Length to 8 Digits",
                    Category = "User Account",
                    Description =
                        "Sets the minimum Windows Hello for Business PIN length to 8 characters, exceeding the Windows default of 6 characters and increasing PIN brute-force resistance.",
                    Tags = ["whfb", "windows-hello", "pin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WHfB PIN must be at least 8 characters; users with shorter PINs must re-set.",
                    ApplyOps = [RegOp.SetDword(Key, "MinimumPINLength", 8)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MinimumPINLength")],
                    DetectOps = [RegOp.CheckDword(Key, "MinimumPINLength", 8)],
                },
                new TweakDef
                {
                    Id = "whfbpin-set-maximum-length-16",
                    Label = "Set WHfB PIN Maximum Length to 16 Digits",
                    Category = "User Account",
                    Description =
                        "Sets the maximum Windows Hello for Business PIN length to 16 characters, balancing usability with security and preventing excessively long PINs that users may forget.",
                    Tags = ["whfb", "windows-hello", "pin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WHfB PIN capped at 16 characters.",
                    ApplyOps = [RegOp.SetDword(Key, "MaximumPINLength", 16)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaximumPINLength")],
                    DetectOps = [RegOp.CheckDword(Key, "MaximumPINLength", 16)],
                },
                new TweakDef
                {
                    Id = "whfbpin-require-uppercase",
                    Label = "Require Uppercase Letters in WHfB PIN",
                    Category = "User Account",
                    Description =
                        "Requires that WHfB PINs contain at least one uppercase letter when using an alphanumeric PIN, increasing PIN complexity and resistance to dictionary attacks.",
                    Tags = ["whfb", "windows-hello", "pin", "complexity", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WHfB PIN must contain uppercase; digits-only PINs disallowed.",
                    ApplyOps = [RegOp.SetDword(Key, "UppercaseLetters", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "UppercaseLetters")],
                    DetectOps = [RegOp.CheckDword(Key, "UppercaseLetters", 1)],
                },
                new TweakDef
                {
                    Id = "whfbpin-require-lowercase",
                    Label = "Require Lowercase Letters in WHfB PIN",
                    Category = "User Account",
                    Description =
                        "Requires that WHfB PINs contain at least one lowercase letter, enforcing mixed-case alphanumeric PINs for greater entropy.",
                    Tags = ["whfb", "windows-hello", "pin", "complexity", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WHfB PIN must contain lowercase; all-uppercase or all-digit PINs disallowed.",
                    ApplyOps = [RegOp.SetDword(Key, "LowercaseLetters", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LowercaseLetters")],
                    DetectOps = [RegOp.CheckDword(Key, "LowercaseLetters", 1)],
                },
                new TweakDef
                {
                    Id = "whfbpin-require-special-chars",
                    Label = "Require Special Characters in WHfB PIN",
                    Category = "User Account",
                    Description =
                        "Requires at least one special character in Windows Hello for Business PINs, maximising PIN entropy and preventing trivially guessable numeric or alphabetic patterns.",
                    Tags = ["whfb", "windows-hello", "pin", "special-chars", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WHfB PIN must include a special character; numeric-only PINs disallowed.",
                    ApplyOps = [RegOp.SetDword(Key, "SpecialCharacters", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SpecialCharacters")],
                    DetectOps = [RegOp.CheckDword(Key, "SpecialCharacters", 1)],
                },
                new TweakDef
                {
                    Id = "whfbpin-set-pin-history-5",
                    Label = "Set WHfB PIN History to 5 Previous PINs",
                    Category = "User Account",
                    Description =
                        "Prevents reuse of the last 5 WHfB PINs, stopping users from cycling back to recently used PINs immediately after a mandatory PIN change.",
                    Tags = ["whfb", "windows-hello", "pin", "history", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Last 5 WHfB PINs remembered; PIN cannot be recycled until 5 unique PINs have been used.",
                    ApplyOps = [RegOp.SetDword(Key, "History", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key, "History")],
                    DetectOps = [RegOp.CheckDword(Key, "History", 5)],
                },
                new TweakDef
                {
                    Id = "whfbpin-set-expiry-180-days",
                    Label = "Set WHfB PIN Expiry to 180 Days",
                    Category = "User Account",
                    Description =
                        "Sets the Windows Hello for Business PIN expiry period to 180 days, requiring periodic PIN rotation to limit the impact of a compromised PIN.",
                    Tags = ["whfb", "windows-hello", "pin", "expiry", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WHfB PIN expires after 180 days; users prompted to create a new PIN on expiry.",
                    ApplyOps = [RegOp.SetDword(Key, "Expiration", 180)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Expiration")],
                    DetectOps = [RegOp.CheckDword(Key, "Expiration", 180)],
                },
                new TweakDef
                {
                    Id = "whfbpin-require-digits",
                    Label = "Require Digits in WHfB Alphanumeric PIN",
                    Category = "User Account",
                    Description =
                        "Requires that alphanumeric WHfB PINs contain at least one digit, preventing purely alphabetic PINs and ensuring a minimum numeric component in the PIN.",
                    Tags = ["whfb", "windows-hello", "pin", "digits", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WHfB alphanumeric PIN must include at least one digit.",
                    ApplyOps = [RegOp.SetDword(Key, "Digits", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Digits")],
                    DetectOps = [RegOp.CheckDword(Key, "Digits", 1)],
                },
                new TweakDef
                {
                    Id = "whfbpin-block-simple-patterns",
                    Label = "Block Simple/Sequential WHfB PIN Patterns",
                    Category = "User Account",
                    Description =
                        "Blocks common sequential (1234, abcd) and repeated-character (1111, aaaa) PIN patterns for WHfB, preventing trivially guessable PINs from being set.",
                    Tags = ["whfb", "windows-hello", "pin", "patterns", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Simple PIN patterns blocked; sequential and repeated patterns rejected at PIN creation.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockSimplePatterns", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockSimplePatterns")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockSimplePatterns", 1)],
                },
                new TweakDef
                {
                    Id = "whfbpin-lockout-after-5-failures",
                    Label = "Lock Out WHfB PIN After 5 Failed Attempts",
                    Category = "User Account",
                    Description =
                        "Locks the WHfB PIN credential after 5 consecutive failed login attempts, requiring a PIN reset via recovery, defending against online brute-force attacks.",
                    Tags = ["whfb", "windows-hello", "pin", "lockout", "brute-force", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WHfB PIN locked after 5 failed attempts; reset required, stopping online PIN guessing.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxFailedAttempts", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxFailedAttempts")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxFailedAttempts", 5)],
                },
            ];
    }

    // ── WindowsHelloAdvPolicy ──
    private static class _WindowsHelloAdvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "helloadv-require-hello-for-domain-auth",
                Label = "Require Windows Hello as Primary Domain Authentication Method",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Windows Hello for Business uses asymmetric key cryptography to replace password-based domain authentication providing phishing-resistant authentication that cannot be replayed. Requiring Windows Hello as the primary authentication method eliminates NTLM and Kerberos password hash exposure for domain authentication. Windows Hello for Business credentials are bound to the device and protected by the TPM making credential theft through standard techniques ineffective. Password-based authentication hashes can be captured from network traffic or from LSASS memory while Hello credentials cannot be harvested and replayed. Organizations should deploy Windows Hello for Business as part of a phishing-resistant MFA strategy aligned with NIST 800-63-3 AAL3 requirements. Hello for Business requires compatible hardware with TPM 2.0 and Windows 10 1703 or later for cloud or hybrid deployments.",
                Tags = ["windows-hello", "phishing-resistant", "domain-auth", "passwordless", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                DetectOps = [RegOp.CheckDword(Key, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "helloadv-require-tpm-for-hello",
                Label = "Require TPM Chip for Windows Hello Key Storage",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Windows Hello for Business keys stored in the TPM are protected by hardware-bound key storage that cannot be exported even with administrative access to the operating system. Requiring TPM for Hello key storage ensures that credentials cannot be extracted from the system and used on another device. Software-based Hello keys stored only in the OS credential store can potentially be extracted through privilege escalation attacks making TPM storage the required configuration for high-security deployments. TPM-bound credentials require both the specific device and the user's PIN or biometric to authenticate combining something you have and something you know or are. Organizations should require TPM 2.0 for all new device purchases to support TPM-protected Hello credentials and other security features like Measured Boot and Device Health Attestation. The RequireSecurityDevice policy ensures that software fallback for Hello keys is not permitted when a TPM is unavailable.",
                Tags = ["windows-hello", "tpm", "hardware-security", "key-protection", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireSecurityDevice", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSecurityDevice")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSecurityDevice", 1)],
            },
            new TweakDef
            {
                Id = "helloadv-set-minimum-pin-length",
                Label = "Enforce Minimum PIN Length for Windows Hello Authentication",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Windows Hello PIN minimum length requirements prevent trivially guessable short PINs that could be brute-forced through shoulder surfing or trial and error. Enforcing a minimum PIN length of 6 or more digits significantly increases the difficulty of guessing or brute-forcing a Hello PIN. Hello PINs are protected by account lockout after a configurable number of failed attempts limiting brute-force attack effectiveness. Unlike passwords Hello PINs are device-specific meaning a leaked PIN cannot be used to authenticate from any device except the specific registered device. Organizations should set minimum PIN lengths to at least 6 characters and consider using enhanced PIN complexity requirements that include alphabetic and special characters for higher security requirements. PIN length requirements should be communicated clearly to users during Windows Hello enrollment to ensure users create appropriate PINs.",
                Tags = ["windows-hello", "pin", "complexity", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MinimumPINLength", 6)],
                RemoveOps = [RegOp.DeleteValue(Key, "MinimumPINLength")],
                DetectOps = [RegOp.CheckDword(Key, "MinimumPINLength", 6)],
            },
            new TweakDef
            {
                Id = "helloadv-enable-biometric-authentication",
                Label = "Enable Biometric Authentication for Windows Hello Sign-in",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Windows Hello biometric authentication allows users to sign in using facial recognition or fingerprint which provides both convenience and strong multi-factor authentication. Enabling biometric authentication for Hello gives users a fast and secure alternative to PIN-based authentication that is resistant to shoulder surfing and observation attacks. Windows Hello enhanced anti-spoofing requires compatible depth-sensing cameras that can distinguish a real face from a photograph. Biometric data collected by Windows Hello is stored only on the device and is protected by the TPM never leaving the device or being sent to Microsoft. Organizations should enable biometric authentication to improve user adoption of Windows Hello as users prefer biometric over PIN authentication when available. Fallback to PIN should be available for situations where biometric authentication fails to ensure users maintain secure access to their systems.",
                Tags = ["windows-hello", "biometric", "fingerprint", "face-recognition", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "UseBiometrics", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "UseBiometrics")],
                DetectOps = [RegOp.CheckDword(Key, "UseBiometrics", 1)],
            },
            new TweakDef
            {
                Id = "helloadv-disable-hello-provisioning-on-shared-pcs",
                Label = "Disable Windows Hello Provisioning on Shared or Kiosk Devices",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Windows Hello provisioning on shared devices creates security concerns because multiple users sharing the same device may inadvertently expose each other's biometric data or PIN-protected credentials. Disabling Hello provisioning on shared PCs ensures that the authentication method is appropriate for the shared use context. Kiosk devices with a single fixed user identity should not provision individual Hello credentials as the device is not used for personalized authentication. Shared workstation scenarios in call centers or shared office spaces require careful evaluation of whether Hello provisioning is appropriate and beneficial. Devices configured in shared PC mode through the Shared PC CSP automatically disable Hello provisioning as part of the shared PC configuration. Organizations should identify all shared devices and apply appropriate Hello configuration rather than applying organization-wide provisioning settings.",
                Tags = ["windows-hello", "shared-pc", "kiosk", "provisioning", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePostLogonProvisioning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePostLogonProvisioning")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePostLogonProvisioning", 1)],
            },
            new TweakDef
            {
                Id = "helloadv-enable-certificate-trust",
                Label = "Enable Certificate Trust Model for Windows Hello for Business",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Windows Hello for Business certificate trust model issues certificate-based credentials from an enterprise PKI enabling authentication to systems and services that require certificate-based authentication. Certificate trust Hello for Business provides compatibility with existing PKI infrastructure and supports scenarios that require X.509 certificates for authentication. Key trust Hello for Business is simpler to deploy but does not support all legacy authentication scenarios that certificate trust supports. Organizations with complex PKI infrastructure and certificate-based authentication requirements should deploy certificate trust Hello for Business. Certificate trust requires AD FS for on-premises deployments or Azure AD for cloud deployments to issue certificates during Hello provisioning. The choice between key trust and certificate trust Hello should align with the organization's authentication requirements and PKI capabilities.",
                Tags = ["windows-hello", "certificate-trust", "pki", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "UseCertificateForOnPremAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "UseCertificateForOnPremAuth")],
                DetectOps = [RegOp.CheckDword(Key, "UseCertificateForOnPremAuth", 1)],
            },
            new TweakDef
            {
                Id = "helloadv-set-pin-expiry",
                Label = "Set Maximum PIN Expiry Period for Windows Hello Authentication",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "PIN expiry for Windows Hello forces users to regularly change their device PINs ensuring that captured or observed PINs have limited useful lifetime. Setting PIN expiry to 60 days aligns with common password policy rotation schedules while acknowledging that PIN security differs from password security. Hello PINs are device-specific making them inherently more secure than domain passwords since a captured PIN cannot be used remotely from other devices. FIDO2 security frameworks generally do not recommend PIN expiry for Hello as the device binding provides sufficient security but organizations with compliance requirements may need to enforce rotation. PIN expiry requirements should be balanced against user friction to ensure users do not choose shorter or simpler PINs to ease memorization of frequently changing credentials. Organizations should evaluate whether the security benefit of PIN rotation outweighs the usability cost for their specific risk profile.",
                Tags = ["windows-hello", "pin-expiry", "rotation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ExpirationDays", 60)],
                RemoveOps = [RegOp.DeleteValue(Key, "ExpirationDays")],
                DetectOps = [RegOp.CheckDword(Key, "ExpirationDays", 60)],
            },
            new TweakDef
            {
                Id = "helloadv-block-simple-pins",
                Label = "Block Simple or Sequential PINs for Windows Hello Authentication",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Simple PINs like 1234, 0000, or sequential digit patterns are the Hello equivalent of weak passwords that are trivially guessable by observers or through systematic guessing. Blocking simple PINs enforces PIN complexity requirements that prevent sequentially or repeatedly patterned digit strings. Windows Hello PIN complexity can require the prevention of consecutive and repeating digits to ensure PINs have sufficient entropy for security. Organizations should configure PIN complexity requirements that prevent the top 100 most common PINs used according to security research on password patterns. The combination of PIN length requirements and simple PIN blocking creates a meaningful security baseline for Hello authentication. Users should be educated about choosing unpredictable PINs and informed that PINs should not use dates or memorable number patterns that could be guessed by social engineering.",
                Tags = ["windows-hello", "pin-complexity", "simple-pins", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "Digits", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "Digits")],
                DetectOps = [RegOp.CheckDword(Key, "Digits", 2)],
            },
            new TweakDef
            {
                Id = "helloadv-enable-remote-unlock",
                Label = "Enable Remote Unlock Capability for Windows Hello Registered Devices",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Windows Hello remote unlock uses proximity-based authentication from a companion device allowing a locked computer to be unlocked when the user's phone is nearby. Enabling remote unlock provides a more convenient unlock experience for users while maintaining the security properties of Windows Hello authentication. Remote unlock requires the Windows Hello companion app installed on a paired mobile device and uses Bluetooth for proximity detection. Organizations should evaluate the security implications of remote unlock in their environment considering whether phone-based proximity is appropriate for their physical security controls. Remote unlock can be beneficial for users who work across multiple devices and need to frequently lock and unlock their workstations. The security of remote unlock depends on the security of the paired mobile device so mobile device management policies must be strong enough to protect this authentication factor.",
                Tags = ["windows-hello", "remote-unlock", "companion-device", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableUnlockFromPhone", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableUnlockFromPhone")],
                DetectOps = [RegOp.CheckDword(Key, "EnableUnlockFromPhone", 1)],
            },
            new TweakDef
            {
                Id = "helloadv-require-enhanced-anti-spoofing",
                Label = "Require Enhanced Anti-Spoofing for Windows Hello Facial Recognition",
                Category = "User Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Enhanced anti-spoofing requires IR camera hardware with depth sensing capabilities that can distinguish a real face from a photograph video or 3D mask presentation attack. Requiring enhanced anti-spoofing for Windows Hello facial recognition ensures that biometric authentication cannot be bypassed using a photograph of the user. Basic facial recognition without anti-spoofing can be defeated by holding a photograph in front of the camera making enhanced anti-spoofing critical for high-security environments. Organizations deploying Windows Hello with facial recognition should verify that enrolled devices have IR cameras that meet the enhanced anti-spoofing specifications before enabling the feature. Enhanced anti-spoofing is the default on devices certified for Windows Hello facial recognition but should be explicitly required for devices that may support facial recognition without certified hardware. Anti-spoofing requirements protect against the most common attack vectors for biometric authentication including photographs and video attacks.",
                Tags = ["windows-hello", "anti-spoofing", "biometric", "facial-recognition", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnhancedAntiSpoofingForFacialFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnhancedAntiSpoofingForFacialFeatures")],
                DetectOps = [RegOp.CheckDword(Key, "EnhancedAntiSpoofingForFacialFeatures", 1)],
            },
        ];
    }

    // ── WorkplaceJoinPolicy ──
    private static class _WorkplaceJoinPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wpjoin-disable-auto",
                    Label = "Disable Automatic Workplace Join",
                    Category = "User Account",
                    Description =
                        "Prevents devices from automatically joining the workplace (Azure AD or on-prem Workplace Join). Requires explicit administrator action to register. Default: 1 (auto). Recommended: 0 (manual only) for managed environments.",
                    Tags = ["workplace-join", "azure-ad", "device-registration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Prevents unintended device registration; IT must manually register devices.",
                    ApplyOps = [RegOp.SetDword(Key, "autoWorkplaceJoin", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "autoWorkplaceJoin")],
                    DetectOps = [RegOp.CheckDword(Key, "autoWorkplaceJoin", 0)],
                },
                new TweakDef
                {
                    Id = "wpjoin-block-aad",
                    Label = "Block Azure AD Workplace Join",
                    Category = "User Account",
                    Description =
                        "Prevents users from performing Azure AD Workplace Join on the device. Useful in air-gapped environments or where cloud synchronisation is not permitted. Default: 0. Recommended: 1 for offline/air-gapped networks.",
                    Tags = ["workplace-join", "azure-ad", "block", "cloud", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks AAD join; device cannot register cloud identity. May affect Intune enrolment.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAADWorkplaceJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAADWorkplaceJoin")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAADWorkplaceJoin", 1)],
                },
                new TweakDef
                {
                    Id = "wpjoin-require-tls",
                    Label = "Require TLS for Workplace Join",
                    Category = "User Account",
                    Description =
                        "Requires Transport Layer Security (TLS/HTTPS) for all Workplace Join registration traffic. Prevents downgrade to unencrypted registration. Default: not enforced. Recommended: 1.",
                    Tags = ["workplace-join", "tls", "encryption", "transport", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Ensures device registration credentials transit over encrypted channels only.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireTLS", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireTLS")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireTLS", 1)],
                },
                new TweakDef
                {
                    Id = "wpjoin-require-integrity-check",
                    Label = "Require Device Integrity Check Before Join",
                    Category = "User Account",
                    Description =
                        "Requires a device integrity check (TPM attestation or health attestation) before allowing Workplace Join registration. Prevents compromised devices from registering. Default: 0. Recommended: 1.",
                    Tags = ["workplace-join", "integrity", "attestation", "tpm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Devices without a TPM or failing health attestation cannot join; increases security posture.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireDeviceIntegrityCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceIntegrityCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireDeviceIntegrityCheck", 1)],
                },
                new TweakDef
                {
                    Id = "wpjoin-require-consent-ui",
                    Label = "Require User Consent for Device Registration",
                    Category = "User Account",
                    Description =
                        "Presents the user with a consent dialog before the device is registered in the workplace. Prevents silent registration without user awareness. Default: 0. Recommended: 1.",
                    Tags = ["workplace-join", "consent", "user", "registration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users must acknowledge device registration; prevents silent cloud enrolment.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireConsentForJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireConsentForJoin")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireConsentForJoin", 1)],
                },
                new TweakDef
                {
                    Id = "wpjoin-disable-silent-reg",
                    Label = "Disable Silent Device Registration",
                    Category = "User Account",
                    Description =
                        "Prevents the device from silently registering itself with Azure AD or on-prem directory services without user interaction. Default: 1 (allow silent). Recommended: 0.",
                    Tags = ["workplace-join", "silent", "registration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All device registrations are visible to and require action from the user.",
                    ApplyOps = [RegOp.SetDword(Key, "SilentDeviceRegistration", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SilentDeviceRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "SilentDeviceRegistration", 0)],
                },
                new TweakDef
                {
                    Id = "wpjoin-limit-max-device-count",
                    Label = "Limit Workplace-Joined Device Count Per User",
                    Category = "User Account",
                    Description =
                        "Sets the maximum number of devices a user can register in the workplace. Limits lateral spread of identities across many devices. Default: not set. Recommended: 3–5.",
                    Tags = ["workplace-join", "device-limit", "quota", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Limits per-user device registration count; users exceeding the limit cannot join new devices.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxDeviceAllowedCount", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxDeviceAllowedCount")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxDeviceAllowedCount", 5)],
                },
                new TweakDef
                {
                    Id = "wpjoin-enable-join-audit",
                    Label = "Enable Workplace Join Audit Logging",
                    Category = "User Account",
                    Description =
                        "Enables detailed audit logging of all Workplace Join registration and de-registration events. Captures device identity, user, and timestamp for compliance. Default: 0. Recommended: 1.",
                    Tags = ["workplace-join", "audit", "logging", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Creates detailed registration event logs; no performance impact on normal operations.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditAADJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditAADJoin")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditAADJoin", 1)],
                },
                new TweakDef
                {
                    Id = "wpjoin-block-non-compliant",
                    Label = "Block Non-Compliant Device Join",
                    Category = "User Account",
                    Description =
                        "Prevents devices that fail compliance checks from completing Workplace Join registration. Works with Intune or SCCM compliance policies. Default: 0. Recommended: 1 in managed environments.",
                    Tags = ["workplace-join", "compliance", "mdm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Non-compliant devices (no antivirus, outdated OS) cannot register; use with MDM compliance policies.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockNonCompliantDevice", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockNonCompliantDevice")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockNonCompliantDevice", 1)],
                },
                new TweakDef
                {
                    Id = "wpjoin-require-secure-channel",
                    Label = "Require Secure Channel for Workplace Join",
                    Category = "User Account",
                    Description =
                        "Requires an established and authenticated secure channel before allowing Workplace Join. Prevents join attempts over untrusted or ad hoc network connections. Default: 0. Recommended: 1.",
                    Tags = ["workplace-join", "secure-channel", "authentication", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Workplace Join is blocked unless an authenticated network channel (VPN or corporate LAN) is present.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSecureChannel", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSecureChannel")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSecureChannel", 1)],
                },
            ];
    }
}
