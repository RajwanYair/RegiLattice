// RegiLattice.Core — Tweaks/AzureAdPrtSsoPolicy.cs
// Azure AD Primary Refresh Token (PRT) SSO, token broker, WAM, and CAE controls — Sprint 459.
// Category: "Azure AD PRT SSO Policy" | Slug: aadprt
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudAuthentication

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AzureAdPrtSsoPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudAuthentication";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "aadprt-enable-prt-sso",
                Label = "Enable Primary Refresh Token SSO",
                Category = "Azure AD PRT SSO Policy",
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
                Category = "Azure AD PRT SSO Policy",
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
                Category = "Azure AD PRT SSO Policy",
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
                Category = "Azure AD PRT SSO Policy",
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
                Category = "Azure AD PRT SSO Policy",
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
                Category = "Azure AD PRT SSO Policy",
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
                Category = "Azure AD PRT SSO Policy",
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
                Category = "Azure AD PRT SSO Policy",
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
                Category = "Azure AD PRT SSO Policy",
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
                Category = "Azure AD PRT SSO Policy",
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
