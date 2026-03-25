// RegiLattice.Core — Tweaks/TokenBrokerPolicy.cs
// Sprint 278: Token Broker Group Policy (10 tweaks)
// Category: "Token Broker Policy" | Slug: tokbrk
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TokenBroker

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TokenBrokerPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TokenBroker";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "tokbrk-disable-token-broker",
            Label = "Disable Web Account Token Broker",
            Category = "Token Broker Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 4,
            Description =
                "Sets DisableTokenBroker=1 in the TokenBroker policy key. Prevents the "
                + "Web Account Manager (WAM) token broker from brokering OAuth access "
                + "tokens between UWP and Win32 applications and Microsoft identity "
                + "endpoints. Token brokering silently reuses cached Microsoft account "
                + "credentials across applications without per-request user consent. "
                + "Disabling may break SSO workflows. Default: 0. Recommended: 1.",
            Tags = ["token", "broker", "wam", "oauth", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTokenBroker", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTokenBroker")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTokenBroker", 1)],
        },
        new TweakDef
        {
            Id = "tokbrk-disable-persistent-token-cache",
            Label = "Disable Persistent Token Cache",
            Category = "Token Broker Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets DisablePersistentTokenCache=1 in the TokenBroker policy key. "
                + "Prevents Web Account Manager from persisting OAuth refresh tokens to "
                + "the Windows Credential Locker across reboots. Without persistence "
                + "tokens expire on sign-out and cannot be reused after a cold start, "
                + "limiting the window for token theft via offline credential-store attacks. "
                + "Default: 0. Recommended: 1.",
            Tags = ["token", "cache", "credential", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePersistentTokenCache", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePersistentTokenCache")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePersistentTokenCache", 1)],
        },
        new TweakDef
        {
            Id = "tokbrk-disable-background-token-refresh",
            Label = "Disable Background Token Refresh",
            Category = "Token Broker Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets DisableBackgroundTokenRefresh=1 in the TokenBroker policy key. "
                + "Prevents WAM from silently refreshing expiring access tokens in the "
                + "background before the calling application requests them. Background "
                + "refresh tasks contact Azure AD or MSA endpoints at unpredictable "
                + "intervals, creating covert network egress that is invisible to manual "
                + "connection-monitoring tools. Default: 0. Recommended: 1.",
            Tags = ["token", "refresh", "background", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBackgroundTokenRefresh", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBackgroundTokenRefresh")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBackgroundTokenRefresh", 1)],
        },
        new TweakDef
        {
            Id = "tokbrk-disable-aad-token-sharing",
            Label = "Disable Azure AD Token Sharing Between Apps",
            Category = "Token Broker Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableAadTokenSharing=1 in the TokenBroker policy key. Blocks WAM "
                + "from sharing a single Azure AD access token issued to one application "
                + "with other requesting applications on the same machine. Token sharing "
                + "enables unexpected cross-application impersonation; each application "
                + "should acquire its own token with its own consent scope. "
                + "Default: 0. Recommended: 1.",
            Tags = ["aad", "token", "sharing", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAadTokenSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAadTokenSharing")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAadTokenSharing", 1)],
        },
        new TweakDef
        {
            Id = "tokbrk-disable-msa-token-sharing",
            Label = "Disable Microsoft Account Token Sharing",
            Category = "Token Broker Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableMsaTokenSharing=1 in the TokenBroker policy key. Prevents WAM "
                + "from sharing MSA (personal Microsoft account) access tokens between "
                + "installed applications. This stops first-party Microsoft apps with broad "
                + "scopes from silently delegating access to third-party applications that "
                + "have registered as WAM token consumers. "
                + "Default: 0. Recommended: 1.",
            Tags = ["msa", "token", "sharing", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMsaTokenSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMsaTokenSharing")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMsaTokenSharing", 1)],
        },
        new TweakDef
        {
            Id = "tokbrk-require-user-consent",
            Label = "Require Explicit User Consent for Token Grants",
            Category = "Token Broker Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets RequireUserConsentForTokenGrant=1 in the TokenBroker policy key. "
                + "Forces WAM to display a consent dialog each time an application requests "
                + "an access token, instead of silently granting from cache. Explicit "
                + "consent makes token acquisition visible to the user and prevents "
                + "applications from quietly accumulating access to cloud resources without "
                + "awareness. Default: 0. Recommended: 1.",
            Tags = ["token", "consent", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireUserConsentForTokenGrant", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireUserConsentForTokenGrant")],
            DetectOps = [RegOp.CheckDword(Key, "RequireUserConsentForTokenGrant", 1)],
        },
        new TweakDef
        {
            Id = "tokbrk-disable-token-telemetry",
            Label = "Disable Token Broker Telemetry",
            Category = "Token Broker Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableTokenBrokerTelemetry=1 in the TokenBroker policy key. Stops "
                + "WAM from emitting diagnostic events that log token-request attempts, "
                + "grant results, application identifiers, and API surface usage to Windows "
                + "telemetry ingestion. Token-request metadata can reveal which cloud "
                + "services applications are accessing without consent from the user. "
                + "Default: 0. Recommended: 1.",
            Tags = ["token", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTokenBrokerTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTokenBrokerTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTokenBrokerTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "tokbrk-disable-implicit-account-discovery",
            Label = "Disable Implicit Account Discovery",
            Category = "Token Broker Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableImplicitAccountDiscovery=1 in the TokenBroker policy key. "
                + "Prevents WAM from enumerating Microsoft accounts and Azure AD accounts "
                + "registered on the device to pre-populate sign-in flows in UWP apps. "
                + "Implicit discovery leaks the list of associated accounts to requesting "
                + "applications before any explicit user action. "
                + "Default: 0. Recommended: 1.",
            Tags = ["token", "account", "discovery", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableImplicitAccountDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableImplicitAccountDiscovery")],
            DetectOps = [RegOp.CheckDword(Key, "DisableImplicitAccountDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "tokbrk-limit-token-lifetime",
            Label = "Limit OAuth Token Lifetime",
            Category = "Token Broker Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets MaxTokenLifetimeMinutes=60 in the TokenBroker policy key. Caps the "
                + "maximum lifetime of access tokens cached by WAM to 60 minutes. Short "
                + "token lifetimes reduce the window during which a stolen token remains "
                + "usable and force more frequent re-authentication events that validate "
                + "the principal's current session state against the identity provider. "
                + "Default: not set (provider default, often 60–90 mins). Recommended: 60.",
            Tags = ["token", "lifetime", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxTokenLifetimeMinutes", 60)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxTokenLifetimeMinutes")],
            DetectOps = [RegOp.CheckDword(Key, "MaxTokenLifetimeMinutes", 60)],
        },
        new TweakDef
        {
            Id = "tokbrk-disable-enterprise-sso",
            Label = "Disable Enterprise SSO Token Propagation",
            Category = "Token Broker Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets DisableEnterpriseSso=1 in the TokenBroker policy key. Prevents WAM "
                + "from propagating a Kerberos or NTLM enterprise SSO token to non-enrolled "
                + "applications requesting Windows-integrated authentication. SSO propagation "
                + "to arbitrary applications can enable CSRF-style attacks where a malicious "
                + "application abuses enterprise credentials obtained via token forwarding. "
                + "Default: 0. Recommended: 1.",
            Tags = ["sso", "enterprise", "token", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableEnterpriseSso", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableEnterpriseSso")],
            DetectOps = [RegOp.CheckDword(Key, "DisableEnterpriseSso", 1)],
        },
    ];
}
