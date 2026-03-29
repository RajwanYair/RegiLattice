// RegiLattice.Core — Tweaks/KerberoastMitigationPolicy.cs
// Kerberoasting Attack Mitigation and Kerberos Hardening — Sprint 436.
// Controls Kerberos protocol parameters to mitigate Kerberoasting (offline cracking of
// service ticket hashes), ticket lifetime limits, encryption enforcement, delegation
// restrictions, and PAC validation to harden Kerberos-based authentication.
// Category: "Kerberoast Mitigation" | Slug: kerbmit
// Registry paths:
//   HKLM\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters — Kerberos configuration
//   HKLM\SYSTEM\CurrentControlSet\Control\Lsa                      — LSA security

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class KerberoastMitigationPolicy
{
    private const string KerbKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";
    private const string LsaKey  = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id          = "kerbmit-disable-rc4-encryption",
                Label       = "Disable RC4 for Kerberos Ticket Encryption",
                Category    = "Kerberoast Mitigation",
                Description = "Sets SupportedEncryptionTypes=0x18 (24) in Kerberos Parameters to allow only AES-128 and AES-256, removing RC4-HMAC support. Kerberoasting succeeds primarily because service tickets encrypted with RC4-HMAC can be cracked offline in hours or days on a GPU. Enforcing AES-only encryption requires 10×–100× more compute for offline attacks, making cracking economically infeasible for properly generated key material.",
                Tags        = ["kerberos", "rc4", "aes", "kerberoasting", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote  = "Disables RC4 Kerberos; legacy services with RC4-only service accounts will fail TGS; upgrade service account keys first.",
                ApplyOps    = [RegOp.SetDword(KerbKey, "SupportedEncryptionTypes", 24)],
                RemoveOps   = [RegOp.DeleteValue(KerbKey, "SupportedEncryptionTypes")],
                DetectOps   = [RegOp.CheckDword(KerbKey, "SupportedEncryptionTypes", 24)],
            },
            new TweakDef
            {
                Id          = "kerbmit-set-max-service-ticket-age",
                Label       = "Reduce Kerberos Service Ticket Lifetime (600 min)",
                Category    = "Kerberoast Mitigation",
                Description = "Sets MaxServiceAge=600 in Kerberos Parameters. Reduces the maximum service ticket (TGS) lifetime from the Windows default of 600 minutes. Shorter ticket lifetimes reduce the window of opportunity for Kerberoasted tickets to be cracked and used: a ticket valid for 10 hours gives an attacker 10 hours to crack it; reducing to 10 minutes means the ticket expires before most cracking jobs complete.",
                Tags        = ["kerberos", "ticket-lifetime", "tgs", "kerberoasting", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote  = "Reduces service ticket lifetime; very short lifetimes increase KDC load from more frequent ticket requests.",
                ApplyOps    = [RegOp.SetDword(KerbKey, "MaxServiceAge", 600)],
                RemoveOps   = [RegOp.DeleteValue(KerbKey, "MaxServiceAge")],
                DetectOps   = [RegOp.CheckDword(KerbKey, "MaxServiceAge", 600)],
            },
            new TweakDef
            {
                Id          = "kerbmit-reduce-max-tgt-age",
                Label       = "Reduce Kerberos TGT Lifetime (600 min)",
                Category    = "Kerberoast Mitigation",
                Description = "Sets MaxTicketAge=600 in Kerberos Parameters. Limits the maximum lifetime of Kerberos Ticket-Granting Tickets. A shorter TGT lifetime limits how long a compromised TGT can be used for privilege escalation (Pass-the-Ticket attacks). After TGT expiry the user must re-authenticate, providing a natural checkpoint to detect and respond to compromised credentials before they can be used further.",
                Tags        = ["kerberos", "tgt", "ticket-lifetime", "pass-the-ticket", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote  = "Reduces TGT lifetime; users will be re-prompted for credentials more frequently in long sessions.",
                ApplyOps    = [RegOp.SetDword(KerbKey, "MaxTicketAge", 600)],
                RemoveOps   = [RegOp.DeleteValue(KerbKey, "MaxTicketAge")],
                DetectOps   = [RegOp.CheckDword(KerbKey, "MaxTicketAge", 600)],
            },
            new TweakDef
            {
                Id          = "kerbmit-tighten-clock-skew",
                Label       = "Tighten Kerberos Clock Skew Tolerance (2 min)",
                Category    = "Kerberoast Mitigation",
                Description = "Sets MaxClockSkew=2 in Kerberos Parameters. Reduces the tolerated clock difference between the client and the KDC from the default 5 minutes to 2 minutes. Kerberos uses timestamps as a replay-protection mechanism; a tighter skew window shrinks the replay attack window. It also limits the usability of pre-computed Kerberos tickets that rely on timestamp tolerance.",
                Tags        = ["kerberos", "clock-skew", "replay", "timestamp", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote  = "Tighter clock skew; ensure NTP is well-configured or clients with drifted clocks will fail Kerberos auth.",
                ApplyOps    = [RegOp.SetDword(KerbKey, "MaxClockSkew", 2)],
                RemoveOps   = [RegOp.DeleteValue(KerbKey, "MaxClockSkew")],
                DetectOps   = [RegOp.CheckDword(KerbKey, "MaxClockSkew", 2)],
            },
            new TweakDef
            {
                Id          = "kerbmit-enable-pac-validation",
                Label       = "Enable KDC PAC Signature Validation",
                Category    = "Kerberoast Mitigation",
                Description = "Sets ValidateKdcPacSignature=1 in the LSA key. Instructs Windows services to validate the KDC Privilege Attribute Certificate (PAC) server signature embedded in Kerberos service tickets. Without validation, a compromised or modified PAC (as exploited by MS14-068) can be used to forge group memberships and escalate privileges. This is the KDC PAC defence against the MS14-068 Kerberos privilege escalation vulnerability.",
                Tags        = ["kerberos", "pac", "signature", "ms14-068", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote  = "Validates PAC signatures; negligible performance impact; critical for defence against forged PAC attacks.",
                ApplyOps    = [RegOp.SetDword(LsaKey, "ValidateKdcPacSignature", 1)],
                RemoveOps   = [RegOp.DeleteValue(LsaKey, "ValidateKdcPacSignature")],
                DetectOps   = [RegOp.CheckDword(LsaKey, "ValidateKdcPacSignature", 1)],
            },
            new TweakDef
            {
                Id          = "kerbmit-restrict-unconstrained-delegation",
                Label       = "Block Kerberos Unconstrained Delegation by Default",
                Category    = "Kerberoast Mitigation",
                Description = "Sets RestrictReceivingNTLMTraffic=2 in Kerberos Parameters. Restricts services from accepting unconstrained Kerberos delegation tokens by default. Unconstrained delegation allows a compromised service to impersonate any user to any other service — it is the primary mechanism exploited in Golden Ticket and delegation-based lateral movement attacks. Setting RestrictReceivingNTLMTraffic also limits NTLM passthrough that accompanies delegation abuse.",
                Tags        = ["kerberos", "delegation", "unconstrained", "lateral-movement", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote  = "Blocks unconstrained delegation; services relying on TrustedForDelegation must be audited before applying.",
                ApplyOps    = [RegOp.SetDword(KerbKey, "RestrictReceivingNTLMTraffic", 2)],
                RemoveOps   = [RegOp.DeleteValue(KerbKey, "RestrictReceivingNTLMTraffic")],
                DetectOps   = [RegOp.CheckDword(KerbKey, "RestrictReceivingNTLMTraffic", 2)],
            },
            new TweakDef
            {
                Id          = "kerbmit-set-renewal-window",
                Label       = "Reduce Kerberos Ticket Renewal Window (4 days)",
                Category    = "Kerberoast Mitigation",
                Description = "Sets MaxRenewAge=4 in Kerberos Parameters. Limits how long a Kerberos TGT can be renewed without full re-authentication. The Windows default is 7 days — meaning a stolen TGT can be continuously renewed for a week without the user re-entering credentials. Reducing to 4 days tightens the window during which a compromised TGT provides persistent access, improving detection opportunities.",
                Tags        = ["kerberos", "renewal", "tgt", "persistence", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote  = "Reduces TGT renewal window to 4 days; users on extended leave may need to re-authenticate on return.",
                ApplyOps    = [RegOp.SetDword(KerbKey, "MaxRenewAge", 4)],
                RemoveOps   = [RegOp.DeleteValue(KerbKey, "MaxRenewAge")],
                DetectOps   = [RegOp.CheckDword(KerbKey, "MaxRenewAge", 4)],
            },
            new TweakDef
            {
                Id          = "kerbmit-enable-armoring",
                Label       = "Enable FAST Kerberos Armoring (Tunnel Mode)",
                Category    = "Kerberoast Mitigation",
                Description = "Sets KdcArmoring=1 in Kerberos Parameters. Enables Kerberos Flexible Authentication via Secure Tunneling (FAST, RFC 6113) which wraps Kerberos authentication messages in an encrypted tunnel. FAST armoring prevents eavesdropping on pre-authentication data (AS-REQ) that would otherwise expose user principal names and enable AS-REP Roasting attacks against accounts without Kerberos pre-authentication required.",
                Tags        = ["kerberos", "fast", "armoring", "as-rep-roasting", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote  = "Enables FAST armoring; requires Windows 8+ domain joined clients; older clients fall back to unarmored auth.",
                ApplyOps    = [RegOp.SetDword(KerbKey, "KdcArmoring", 1)],
                RemoveOps   = [RegOp.DeleteValue(KerbKey, "KdcArmoring")],
                DetectOps   = [RegOp.CheckDword(KerbKey, "KdcArmoring", 1)],
            },
            new TweakDef
            {
                Id          = "kerbmit-disable-msskip-delegation",
                Label       = "Block NTLM Delegation to All Servers",
                Category    = "Kerberoast Mitigation",
                Description = "Sets AllowNTLMSessionSecurity=0 in Kerberos Parameters. Prevents Kerberos from falling back to NTLM session security for delegation, closing a common path by which attackers convert Kerberos delegation abuse into NTLM-based lateral movement. Forcing pure Kerberos delegation eliminates the NTLM relay component of many sophisticated delegation attacks.",
                Tags        = ["kerberos", "ntlm", "delegation", "session-security", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote  = "Blocks NTLM session delegation fallback; ensure domain controllers and services support Kerberos delegation exclusively.",
                ApplyOps    = [RegOp.SetDword(KerbKey, "AllowNTLMSessionSecurity", 0)],
                RemoveOps   = [RegOp.DeleteValue(KerbKey, "AllowNTLMSessionSecurity")],
                DetectOps   = [RegOp.CheckDword(KerbKey, "AllowNTLMSessionSecurity", 0)],
            },
            new TweakDef
            {
                Id          = "kerbmit-enforce-preauth-required",
                Label       = "Enforce Kerberos Pre-Authentication Requirement",
                Category    = "Kerberoast Mitigation",
                Description = "Sets ClientRequireStrictKDCValidation=1 in Kerberos Parameters. Instructs Kerberos clients to enforce strict KDC validation requirements including pre-authentication enforcement. Accounts with 'Do not require Kerberos preauthentication' (DONT_REQUIRE_PREAUTH) are trivially AS-REP Roastable — an attacker can request their encrypted TGT reply without knowing their password. This policy ensures the client enforces pre-auth at the KDC level.",
                Tags        = ["kerberos", "preauthentication", "as-rep-roasting", "kdc", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote  = "Requires strict KDC validation; service accounts with DONT_REQUIRE_PREAUTH flag set must have it removed first.",
                ApplyOps    = [RegOp.SetDword(KerbKey, "ClientRequireStrictKDCValidation", 1)],
                RemoveOps   = [RegOp.DeleteValue(KerbKey, "ClientRequireStrictKDCValidation")],
                DetectOps   = [RegOp.CheckDword(KerbKey, "ClientRequireStrictKDCValidation", 1)],
            },
        ];
}
