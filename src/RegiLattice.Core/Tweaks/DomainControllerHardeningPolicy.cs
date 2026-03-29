// RegiLattice.Core — Tweaks/DomainControllerHardeningPolicy.cs
// Domain controller hardening, Netlogon secure channel, machine account password, and DC-specific restrictions — Sprint 527.
// Category: "Domain Controller Hardening Policy" | Slug: dchrdn
// Registry: HKLM\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DomainControllerHardeningPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";
    private const string SecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Netlogon";
    private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "dchrdn-require-secure-channel-signing",
            Label        = "Require Signing on All Netlogon Secure Channel Connections",
            Category     = "Domain Controller Hardening Policy",
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
            Category     = "Domain Controller Hardening Policy",
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
            Category     = "Domain Controller Hardening Policy",
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
            Category     = "Domain Controller Hardening Policy",
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
            Category     = "Domain Controller Hardening Policy",
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
            Category     = "Domain Controller Hardening Policy",
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
            Category     = "Domain Controller Hardening Policy",
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
            Category     = "Domain Controller Hardening Policy",
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
            Category     = "Domain Controller Hardening Policy",
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
            Category     = "Domain Controller Hardening Policy",
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
