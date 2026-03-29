// RegiLattice.Core — Tweaks/IpsecRulePolicy.cs
// IPSec / IKE policy hardening via registry — Sprint 438.
// Disables default exemptions, enforces CRL checking, sets key lifetimes,
// requires PFS, mandates DH Group 2, enables AH integrity, and blocks null encryption.
// Category: "IPSec Rule Policy" | Slug: ipsecpol
// Registry: HKLM\SYSTEM\CurrentControlSet\Services\PolicyAgent\Oakley
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\IPSec\LocalPolicyModule

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class IpsecRulePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PolicyAgent\Oakley";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPSec\LocalPolicyModule";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ipsecpol-disable-default-exemptions",
                Label = "Disable IPSec Default Exemptions",
                Category = "IPSec Rule Policy",
                Description =
                    "Sets DisableDefaultExemptions=3 to remove built-in IKE, Kerberos, and multicast exemptions, ensuring all traffic is subject to IPSec filtering rules.",
                Tags = ["ipsec", "ike", "exemptions", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Removes default exemptions; may disrupt Kerberos until IPSec rules are configured.",
                ApplyOps = [RegOp.SetDword(Key, "DisableDefaultExemptions", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDefaultExemptions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDefaultExemptions", 3)],
            },
            new TweakDef
            {
                Id = "ipsecpol-strong-crl-check",
                Label = "Enable Strong CRL Checking for IPSec",
                Category = "IPSec Rule Policy",
                Description =
                    "Enables certificate revocation list (CRL) checking for certificates used in IPSec authentication, preventing revoked certificates from being accepted.",
                Tags = ["ipsec", "crl", "certificate", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "CRL checked per IKE negotiation; requires CRL availability at connection time.",
                ApplyOps = [RegOp.SetDword(Key, "EnableCRLCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableCRLCheck")],
                DetectOps = [RegOp.CheckDword(Key, "EnableCRLCheck", 1)],
            },
            new TweakDef
            {
                Id = "ipsecpol-ike-key-lifetime",
                Label = "Set IKE Main Mode Key Lifetime to 8 Hours",
                Category = "IPSec Rule Policy",
                Description =
                    "Sets the IKE main mode key lifetime to 480 minutes (8 hours). Regular renegotiation limits the window of exposure if a key is compromised.",
                Tags = ["ipsec", "ike", "key-lifetime", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Shorter lifetime improves key hygiene; increases IKE renegotiation frequency.",
                ApplyOps = [RegOp.SetDword(Key, "IKEKeyExpirationTime", 480)],
                RemoveOps = [RegOp.DeleteValue(Key, "IKEKeyExpirationTime")],
                DetectOps = [RegOp.CheckDword(Key, "IKEKeyExpirationTime", 480)],
            },
            new TweakDef
            {
                Id = "ipsecpol-session-key-lifetime",
                Label = "Set IPSec Session Key Lifetime to 15 Minutes",
                Category = "IPSec Rule Policy",
                Description =
                    "Sets the IPSec quick mode session key lifetime to 900 seconds (15 minutes), limiting the impact window of a compromised session key.",
                Tags = ["ipsec", "session-key", "lifetime", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "15-minute session key reduces exposed data per compromise; slight CPU overhead on busy links.",
                ApplyOps = [RegOp.SetDword(Key, "IKESessionKeyLifetime", 900)],
                RemoveOps = [RegOp.DeleteValue(Key, "IKESessionKeyLifetime")],
                DetectOps = [RegOp.CheckDword(Key, "IKESessionKeyLifetime", 900)],
            },
            new TweakDef
            {
                Id = "ipsecpol-enable-pfs",
                Label = "Enable Perfect Forward Secrecy for IPSec",
                Category = "IPSec Rule Policy",
                Description =
                    "Enables Perfect Forward Secrecy (PFS) so each session key is derived independently, preventing compromise of one key from exposing past or future sessions.",
                Tags = ["ipsec", "pfs", "encryption", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Adds computational overhead per session key negotiation; essential for high-security environments.",
                ApplyOps = [RegOp.SetDword(Key, "EnablePFS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePFS")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePFS", 1)],
            },
            new TweakDef
            {
                Id = "ipsecpol-require-dh-group2",
                Label = "Require Diffie-Hellman Group 2 for IKE",
                Category = "IPSec Rule Policy",
                Description = "Sets the minimum DH group to Group 2 (1024-bit MODP) for IKE negotiation, blocking the weaker Group 1 (768-bit).",
                Tags = ["ipsec", "dh", "diffie-hellman", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Requires DH Group 2+; incompatible with peers using obsolete Group 1.",
                ApplyOps = [RegOp.SetDword(Key, "DHGroup", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "DHGroup")],
                DetectOps = [RegOp.CheckDword(Key, "DHGroup", 2)],
            },
            new TweakDef
            {
                Id = "ipsecpol-enable-ah-integrity",
                Label = "Enable AH Integrity Checking for IPSec",
                Category = "IPSec Rule Policy",
                Description =
                    "Enables the AH (Authentication Header) integrity mechanism, ensuring packet headers are cryptographically verified during IPSec communication.",
                Tags = ["ipsec", "ah", "integrity", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "AH header authentication adds integrity; incompatible with NAT traversal.",
                ApplyOps = [RegOp.SetDword(Key2, "EnableAHIntegrity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "EnableAHIntegrity")],
                DetectOps = [RegOp.CheckDword(Key2, "EnableAHIntegrity", 1)],
            },
            new TweakDef
            {
                Id = "ipsecpol-block-null-encryption",
                Label = "Block Null Encryption in IPSec ESP",
                Category = "IPSec Rule Policy",
                Description =
                    "Disables null encryption in ESP (Encapsulating Security Payload), ensuring all IPSec-encrypted traffic uses a real cipher such as AES.",
                Tags = ["ipsec", "esp", "encryption", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "All ESP tunnels must use a real cipher; null-encryption tunnels are rejected.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableNullEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableNullEncryption")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableNullEncryption", 1)],
            },
            new TweakDef
            {
                Id = "ipsecpol-require-esp-encryption",
                Label = "Require ESP Encryption for All IPSec Tunnels",
                Category = "IPSec Rule Policy",
                Description = "Requires ESP with encryption for all IPSec connections, preventing integrity-only AH-only or unencrypted tunnels.",
                Tags = ["ipsec", "esp", "encryption", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Enforces encrypted ESP; AH-only tunnels are disallowed.",
                ApplyOps = [RegOp.SetDword(Key2, "RequireESPEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "RequireESPEncryption")],
                DetectOps = [RegOp.CheckDword(Key2, "RequireESPEncryption", 1)],
            },
            new TweakDef
            {
                Id = "ipsecpol-negotiation-poll-interval",
                Label = "Set IPSec Negotiation Poll Interval to 5 Minutes",
                Category = "IPSec Rule Policy",
                Description =
                    "Sets the IPSec policy negotiation polling interval to 300 seconds (5 minutes), controlling how frequently the service checks for policy changes.",
                Tags = ["ipsec", "negotiation", "policy", "interval"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Reduces policy-update latency; negligible performance impact.",
                ApplyOps = [RegOp.SetDword(Key2, "NegotiationPollInterval", 300)],
                RemoveOps = [RegOp.DeleteValue(Key2, "NegotiationPollInterval")],
                DetectOps = [RegOp.CheckDword(Key2, "NegotiationPollInterval", 300)],
            },
        ];
}
