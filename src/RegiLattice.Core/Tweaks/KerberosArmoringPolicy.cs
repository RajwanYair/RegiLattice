// RegiLattice.Core — Tweaks/KerberosArmoringPolicy.cs
// Kerberos armoring and advanced Kerberos security Group Policy controls — Sprint 374.
// Category: "Kerberos Hardening Policy" | Slug: krbadv
// Registry paths: HKLM\SOFTWARE\Policies\Microsoft\Windows\System\KDC (KDC side)
//                 HKLM\SOFTWARE\Policies\Microsoft\Windows\System\Kerberos (client side)
// MinBuild: 9600 (Windows 8.1 / Server 2012 R2+)
namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class KerberosArmoringPolicy
{
    private const string KdcKey     = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System\KDC";
    private const string KrbKey     = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System\Kerberos";
    private const string KrbSvcKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Kerberos\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "krbadv-enable-kdc-armoring",
            Label = "Enable Kerberos Armoring (FAST) on KDC",
            Category = "Kerberos Hardening Policy",
            Description = "Enables Flexible Authentication Secure Tunneling (FAST / Kerberos armoring) on the KDC. FAST wraps KDC requests in an armored tunnel, protecting pre-authentication data from offline attacks and downgrade attempts.",
            Tags = ["kerberos", "fast", "armoring", "kdc", "pre-authentication"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "FAST protects Kerberos pre-auth from AS-REP roasting and offline cracking. Requires compatible DCs and clients (Windows 8.1+ / Server 2012 R2+).",
            RegistryKeys = [KdcKey],
            ApplyOps  = [RegOp.SetDword(KdcKey, "EnableKDCArmoring", 1)],
            RemoveOps = [RegOp.DeleteValue(KdcKey, "EnableKDCArmoring")],
            DetectOps = [RegOp.CheckDword(KdcKey, "EnableKDCArmoring", 1)],
        },
        new TweakDef
        {
            Id = "krbadv-require-client-armoring",
            Label = "Require Kerberos Armoring for Client Authentication",
            Category = "Kerberos Hardening Policy",
            Description = "Forces Kerberos clients to use FAST armoring when requesting tickets from the KDC. Clients that do not support FAST will be denied authentication, ensuring all ticket exchanges occur through an encrypted tunnel.",
            Tags = ["kerberos", "fast", "armoring", "client", "enforcement"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "Requiring FAST on clients breaks authentication for Windows 7 and older clients. Audit FAST support across the domain before enforcing.",
            RegistryKeys = [KdcKey],
            ApplyOps  = [RegOp.SetDword(KdcKey, "RequireArmoredKrb5OnDC", 1)],
            RemoveOps = [RegOp.DeleteValue(KdcKey, "RequireArmoredKrb5OnDC")],
            DetectOps = [RegOp.CheckDword(KdcKey, "RequireArmoredKrb5OnDC", 1)],
        },
        new TweakDef
        {
            Id = "krbadv-disable-des-encryption",
            Label = "Disable DES Encryption Types for Kerberos",
            Category = "Kerberos Hardening Policy",
            Description = "Disables DES-CBC-CRC and DES-CBC-MD5 encryption types for Kerberos. DES is a 56-bit algorithm broken by modern cracking rigs in hours. Only AES128 and AES256 should remain enabled.",
            Tags = ["kerberos", "des", "encryption", "cipher", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "DES key types must be re-negotiated for affected service accounts (ktpass /crypto AES256). Breaks Kerberos for systems that have DES only in their msDS-SupportedEncryptionTypes.",
            RegistryKeys = [KrbKey],
            ApplyOps  = [RegOp.SetDword(KrbKey, "DefaultEncryptionTypes", 2147483616)],
            RemoveOps = [RegOp.DeleteValue(KrbKey, "DefaultEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(KrbKey, "DefaultEncryptionTypes", 2147483616)],
        },
        new TweakDef
        {
            Id = "krbadv-require-strict-kdc-validation",
            Label = "Require Strict KDC Validation (Authenticate the KDC)",
            Category = "Kerberos Hardening Policy",
            Description = "Enables strict KDC validation so the client verifies the KDC's identity before trusting the returned tickets. Prevents rogue or spoofed KDCs from issuing valid-looking tickets to the client.",
            Tags = ["kerberos", "kdc-validation", "rogue-kdc", "trust", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Resolves attacks where a rogue KDC tricks clients into accepting attacker-forged tickets. Requires DCs to have valid certificates in the NTAuth store.",
            RegistryKeys = [KrbKey],
            ApplyOps  = [RegOp.SetDword(KrbKey, "ValidateKDCCertUsage", 1)],
            RemoveOps = [RegOp.DeleteValue(KrbKey, "ValidateKDCCertUsage")],
            DetectOps = [RegOp.CheckDword(KrbKey, "ValidateKDCCertUsage", 1)],
        },
        new TweakDef
        {
            Id = "krbadv-enable-pkinit-freshness",
            Label = "Enable PKInit Freshness Extension for Kerberos",
            Category = "Kerberos Hardening Policy",
            Description = "Enables the PKInit Freshness Extension (RFC 8070), which binds Kerberos authentication tokens to a freshness endpoint in the TGT. Prevents certificate-based credential relay and Golden Certificate attacks.",
            Tags = ["kerberos", "pkinit", "freshness", "golden-ticket", "certificate"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Mitigates Golden Certificate attacks (CVE-2021-42278 / CVE-2022-34691 class). Requires Windows Server 2016+ DCs and Windows 10+ clients for full support.",
            RegistryKeys = [KdcKey],
            ApplyOps  = [RegOp.SetDword(KdcKey, "PKInitHashAlgorithmConfiguration", 1)],
            RemoveOps = [RegOp.DeleteValue(KdcKey, "PKInitHashAlgorithmConfiguration")],
            DetectOps = [RegOp.CheckDword(KdcKey, "PKInitHashAlgorithmConfiguration", 1)],
        },
        new TweakDef
        {
            Id = "krbadv-set-max-service-ticket-lifetime",
            Label = "Reduce Maximum Kerberos Service Ticket Lifetime to 600 Minutes",
            Category = "Kerberos Hardening Policy",
            Description = "Sets the maximum Kerberos service ticket (TGS) lifetime to 600 minutes (10 hours). Shorter lifetimes reduce the window in which a captured ticket can be replayed; the default is 600 minutes but some environments set it higher.",
            Tags = ["kerberos", "ticket-lifetime", "tgs", "replay-prevention", "session"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reducing ticket lifetime increases authentication load as clients renew tickets more frequently. Verify DC capacity before reducing below 60 minutes.",
            RegistryKeys = [KrbSvcKey],
            ApplyOps  = [RegOp.SetDword(KrbSvcKey, "MaxServiceTicketAge", 600)],
            RemoveOps = [RegOp.DeleteValue(KrbSvcKey, "MaxServiceTicketAge")],
            DetectOps = [RegOp.CheckDword(KrbSvcKey, "MaxServiceTicketAge", 600)],
        },
        new TweakDef
        {
            Id = "krbadv-set-max-tgt-lifetime",
            Label = "Set Maximum Kerberos TGT Lifetime to 10 Hours",
            Category = "Kerberos Hardening Policy",
            Description = "Limits the Ticket-Granting Ticket (TGT) lifetime to 10 hours (600 minutes). Reduces the window for Golden Ticket attacks — if a TGT is captured, the attacker has a bounded exploitation window.",
            Tags = ["kerberos", "tgt", "golden-ticket", "ticket-lifetime", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Shorter TGT lifetime is a key mitigation for Golden Ticket attacks. Users must re-authenticate after TGT expiry; aligns with standard domain policy (10 hours).",
            RegistryKeys = [KrbSvcKey],
            ApplyOps  = [RegOp.SetDword(KrbSvcKey, "MaxTicketAge", 10)],
            RemoveOps = [RegOp.DeleteValue(KrbSvcKey, "MaxTicketAge")],
            DetectOps = [RegOp.CheckDword(KrbSvcKey, "MaxTicketAge", 10)],
        },
        new TweakDef
        {
            Id = "krbadv-enforce-tgs-renewal-deadline",
            Label = "Enforce Strict Kerberos Ticket Renewal Deadline (7 Days)",
            Category = "Kerberos Hardening Policy",
            Description = "Sets the maximum Kerberos ticket renewal lifetime to 7 days. After 7 days a ticket cannot be renewed and the user must obtain a fresh TGT; this ensures stale or stolen tickets expire regardless of continuous renewal.",
            Tags = ["kerberos", "ticket-renewal", "expiry", "stolen-ticket", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures long-running sessions produce fresh TGTs at least weekly. Non-disruptive for interactive users; long-running services must handle 7-day re-auth.",
            RegistryKeys = [KrbSvcKey],
            ApplyOps  = [RegOp.SetDword(KrbSvcKey, "MaxRenewAge", 7)],
            RemoveOps = [RegOp.DeleteValue(KrbSvcKey, "MaxRenewAge")],
            DetectOps = [RegOp.CheckDword(KrbSvcKey, "MaxRenewAge", 7)],
        },
        new TweakDef
        {
            Id = "krbadv-enforce-clock-sync",
            Label = "Enforce Strict Kerberos Clock Synchronisation Tolerance (5 Minutes)",
            Category = "Kerberos Hardening Policy",
            Description = "Sets the Kerberos clock skew tolerance to 5 minutes (the standard RFC 4120 maximum). Clock skew is required for replay-protection; allowing large skew enables ticket replay. Enforce NTP synchronisation alongside this policy.",
            Tags = ["kerberos", "clock-sync", "ntp", "replay-protection", "time"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Requires reliable NTP across all domain members. Systems without time sync will fail Kerberos authentication if clock skew exceeds 5 minutes.",
            RegistryKeys = [KrbSvcKey],
            ApplyOps  = [RegOp.SetDword(KrbSvcKey, "MaxClockSkew", 5)],
            RemoveOps = [RegOp.DeleteValue(KrbSvcKey, "MaxClockSkew")],
            DetectOps = [RegOp.CheckDword(KrbSvcKey, "MaxClockSkew", 5)],
        },
        new TweakDef
        {
            Id = "krbadv-disable-rc4-hmac-encryption",
            Label = "Disable RC4-HMAC Encryption for Kerberos (Require AES)",
            Category = "Kerberos Hardening Policy",
            Description = "Removes RC4-HMAC (ARCFOUR-HMAC-MD5) from the supported Kerberos encryption type list. RC4-HMAC is vulnerable to offline cracking (AS-REP roasting, Kerberoasting); AES128 and AES256 should be the only accepted types.",
            Tags = ["kerberos", "rc4", "arcfour", "encryption", "kerberoasting"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "Completely eliminates Kerberoasting vector. Service accounts with RC4 keys (old msDS-SupportedEncryptionTypes) will fail; re-key all SPNs with AES before enforcing.",
            RegistryKeys = [KrbKey],
            ApplyOps  = [RegOp.SetDword(KrbKey, "SupportedEncryptionTypes", 2147483640)],
            RemoveOps = [RegOp.DeleteValue(KrbKey, "SupportedEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(KrbKey, "SupportedEncryptionTypes", 2147483640)],
        },
    ];
}
