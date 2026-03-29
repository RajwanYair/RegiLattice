// RegiLattice.Core — Tweaks/KerberosSecurityPolicy.cs
// Kerberos authentication security, constrained delegation, ticket lifetime, and hardening policy — Sprint 522.
// Category: "Kerberos Security Policy" | Slug: krbadv
// Registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class KerberosSecurityPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters";
    private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System\Audit";
    private const string SecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Security\Kerberos";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "krbadv-enable-claims-support",
            Label        = "Enable Kerberos Claims and Compound Authentication Support",
            Category     = "Kerberos Security Policy",
            Description  = "Enables Kerberos claims-based authentication and compound authentication (user + device claims), required for Dynamic Access Control (DAC) file share access and conditional access policies based on device health claims.",
            Tags         = ["kerberos", "claims", "compound-auth", "dac", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Kerberos claims and compound auth enabled; required for Dynamic Access Control and device-based conditional access.",
            ApplyOps     = [RegOp.SetDword(Key, "EnableCbacAndArmor", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "EnableCbacAndArmor")],
            DetectOps    = [RegOp.CheckDword(Key, "EnableCbacAndArmor", 1)],
        },
        new TweakDef
        {
            Id           = "krbadv-require-fast-armoring",
            Label        = "Require Kerberos Armoring (FAST) for All Authentication",
            Category     = "Kerberos Security Policy",
            Description  = "Requires Kerberos Flexible Authentication Secure Tunneling (FAST/Kerberos Armoring) for all Kerberos exchanges, providing protection against offline pre-authentication blob cracking attacks (AS-REP roasting).",
            Tags         = ["kerberos", "fast", "armoring", "asrep-roasting", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Kerberos FAST armoring required; AS-REP roasting attacks mitigated. Requires KDC support for FAST.",
            ApplyOps     = [RegOp.SetDword(Key, "EnableKerberosArmoring", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "EnableKerberosArmoring")],
            DetectOps    = [RegOp.CheckDword(Key, "EnableKerberosArmoring", 2)],
        },
        new TweakDef
        {
            Id           = "krbadv-block-rc4-encryption",
            Label        = "Block RC4-HMAC Encryption for Kerberos Tickets",
            Category     = "Kerberos Security Policy",
            Description  = "Disables the RC4-HMAC cipher suite for Kerberos ticket encryption, forcing all tickets to use AES-128 or AES-256 encryption, which is significantly stronger than the legacy RC4 encryption still used by some service accounts.",
            Tags         = ["kerberos", "rc4", "aes", "encryption", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Kerberos RC4-HMAC encryption disabled; only AES-128/AES-256 tickets accepted. Service accounts need AES keys.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableRC4Encryption", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableRC4Encryption")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableRC4Encryption", 1)],
        },
        new TweakDef
        {
            Id           = "krbadv-enable-des-encryption-off",
            Label        = "Disable DES Cipher for Kerberos (Legacy Removal)",
            Category     = "Kerberos Security Policy",
            Description  = "Disables DES (Data Encryption Standard) cipher support in Kerberos, eliminating the use of the cryptographically broken DES algorithm that was still negotiated with very old service accounts in some mixed environments.",
            Tags         = ["kerberos", "des", "legacy-cipher", "encryption", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Kerberos DES encryption completely disabled; broken DES cipher no longer negotiated in any Kerberos exchange.",
            ApplyOps     = [RegOp.SetDword(Key, "DisallowDES", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisallowDES")],
            DetectOps    = [RegOp.CheckDword(Key, "DisallowDES", 1)],
        },
        new TweakDef
        {
            Id           = "krbadv-set-ticket-lifetime-8h",
            Label        = "Set Kerberos Ticket Maximum Lifetime to 8 Hours",
            Category     = "Kerberos Security Policy",
            Description  = "Configures the Kerberos TGT (Ticket Granting Ticket) maximum lifetime to 8 hours, ensuring tickets expire during a typical business day so stolen tickets cannot be replayed indefinitely.",
            Tags         = ["kerberos", "ticket-lifetime", "tgt", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Kerberos TGT lifetime set to 8 hours; stolen tickets expire within a business day.",
            ApplyOps     = [RegOp.SetDword(SecKey, "MaxTicketAge", 8)],
            RemoveOps    = [RegOp.DeleteValue(SecKey, "MaxTicketAge")],
            DetectOps    = [RegOp.CheckDword(SecKey, "MaxTicketAge", 8)],
        },
        new TweakDef
        {
            Id           = "krbadv-set-service-ticket-lifetime-10m",
            Label        = "Set Kerberos Service Ticket Maximum Lifetime to 600 Minutes",
            Category     = "Kerberos Security Policy",
            Description  = "Sets the maximum service ticket (TGS) lifetime to 600 minutes (10 hours), which is long enough for a business day session while limiting the window during which a stolen service ticket could be replayed against a service.",
            Tags         = ["kerberos", "service-ticket", "tgs", "lifetime", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Kerberos service ticket lifetime limited to 10 hours; limits replay window for stolen service tickets.",
            ApplyOps     = [RegOp.SetDword(SecKey, "MaxServiceAge", 600)],
            RemoveOps    = [RegOp.DeleteValue(SecKey, "MaxServiceAge")],
            DetectOps    = [RegOp.CheckDword(SecKey, "MaxServiceAge", 600)],
        },
        new TweakDef
        {
            Id           = "krbadv-set-renew-lifetime-7d",
            Label        = "Set Kerberos Ticket Maximum Renewal Lifetime to 7 Days",
            Category     = "Kerberos Security Policy",
            Description  = "Sets the maximum TGT renewal lifetime to 7 days, after which the user must fully re-authenticate with their password or smart card rather than just renewing an existing ticket.",
            Tags         = ["kerberos", "renewal-lifetime", "tgt", "re-authentication", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Kerberos TGT renewable lifetime set to 7 days; full re-auth required after 1 week.",
            ApplyOps     = [RegOp.SetDword(SecKey, "MaxRenewAge", 7)],
            RemoveOps    = [RegOp.DeleteValue(SecKey, "MaxRenewAge")],
            DetectOps    = [RegOp.CheckDword(SecKey, "MaxRenewAge", 7)],
        },
        new TweakDef
        {
            Id           = "krbadv-log-kerberos-failures",
            Label        = "Log Kerberos Pre-Authentication Failure Events",
            Category     = "Kerberos Security Policy",
            Description  = "Enables Security audit logging for Kerberos AS exchange pre-authentication failures (EventID 4771), providing visibility into password-spraying and Kerberoasting attempts against domain accounts.",
            Tags         = ["kerberos", "pre-auth-failure", "audit", "event-log", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Kerberos pre-auth failures logged (EventID 4771); password spray and Kerberoasting attempts visible.",
            ApplyOps     = [RegOp.SetDword(SecKey, "AuditPreAuthFailures", 1)],
            RemoveOps    = [RegOp.DeleteValue(SecKey, "AuditPreAuthFailures")],
            DetectOps    = [RegOp.CheckDword(SecKey, "AuditPreAuthFailures", 1)],
        },
        new TweakDef
        {
            Id           = "krbadv-block-unconstrained-delegation",
            Label        = "Block Accounts from Using Unconstrained Kerberos Delegation",
            Category     = "Kerberos Security Policy",
            Description  = "Enables the 'Account is sensitive and cannot be delegated' flag enforcement at policy level, blocking non-protected accounts from being marked for unconstrained delegation which allows impersonation of any user who authenticates to the delegate.",
            Tags         = ["kerberos", "delegation", "unconstrained", "impersonation", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Unconstrained Kerberos delegation blocked for new accounts; existing delegation settings unaffected.",
            ApplyOps     = [RegOp.SetDword(Key, "BlockUnconstrainedDelegation", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "BlockUnconstrainedDelegation")],
            DetectOps    = [RegOp.CheckDword(Key, "BlockUnconstrainedDelegation", 1)],
        },
        new TweakDef
        {
            Id           = "krbadv-disable-kerberos-telemetry",
            Label        = "Disable Kerberos Authentication Telemetry to Microsoft",
            Category     = "Kerberos Security Policy",
            Description  = "Prevents the Windows Kerberos provider from sending cipher negotiation stats, authentication failure rates, and encryption algorithm telemetry to Microsoft.",
            Tags         = ["kerberos", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Kerberos telemetry to Microsoft disabled; cipher negotiation and failure rate data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableKerberosTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableKerberosTelemetry")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableKerberosTelemetry", 1)],
        },
    ];
}
