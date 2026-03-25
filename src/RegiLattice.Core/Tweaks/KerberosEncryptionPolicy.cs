#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 240 — Kerberos Encryption Policy (10 tweaks)
// Keys under HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters
// and HKLM\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters
internal static class KerberosEncryptionPolicy
{
    private const string KerbPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters";
    private const string KerbLsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "kerb-disable-des-encryption",
            Label = "Disable DES Encryption for Kerberos",
            Category = "Kerberos Encryption Policy",
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
            Category = "Kerberos Encryption Policy",
            Description = "Removes RC4-HMAC from Kerberos supported encryption types, forcing AES128/AES256 only.",
            Tags = ["kerberos", "rc4", "encryption", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "RC4 in Kerberos enables AS-REP roasting and other attacks. Removing it requires all principal accounts to have AES keys set.",
            ApplyOps = [RegOp.SetDword(KerbPolicyKey, "SupportedEncryptionTypes", 2147483616)],
            RemoveOps = [RegOp.DeleteValue(KerbPolicyKey, "SupportedEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(KerbPolicyKey, "SupportedEncryptionTypes", 2147483616)],
        },

        new TweakDef
        {
            Id = "kerb-require-aes256",
            Label = "Require AES256 for Kerberos",
            Category = "Kerberos Encryption Policy",
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
            Category = "Kerberos Encryption Policy",
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
            Category = "Kerberos Encryption Policy",
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
            Category = "Kerberos Encryption Policy",
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
            Category = "Kerberos Encryption Policy",
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
            Category = "Kerberos Encryption Policy",
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
            Category = "Kerberos Encryption Policy",
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
            Category = "Kerberos Encryption Policy",
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
