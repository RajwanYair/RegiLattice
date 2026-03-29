// RegiLattice.Core — Tweaks/DohEnforcementPolicy.cs
// DNS-over-HTTPS (DoH) enforcement via Group Policy — Sprint 440.
// Enables and enforces DoH mode 3 (require), blocks plaintext DNS fallback,
// enables DNSSEC validation, disables LLMNR and NetBIOS name resolution.
// Category: "DoH Enforcement Policy" | Slug: dohpol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\DNSClient

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DohEnforcementPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DNSClient";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "dohpol-enable-doh-require",
                Label = "Enable DNS-over-HTTPS — Require Mode",
                Category = "DoH Enforcement Policy",
                Description =
                    "Sets EnableAutoDoh=3 to require DoH for all DNS queries. DNS resolution fails if a DoH resolver is unavailable, preventing plaintext DNS fallback.",
                Tags = ["doh", "dns", "encryption", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Enforces DoH; DNS resolution fails if DoH server is unreachable — configure DoH resolver first.",
                ApplyOps = [RegOp.SetDword(Key, "EnableAutoDoh", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableAutoDoh")],
                DetectOps = [RegOp.CheckDword(Key, "EnableAutoDoh", 3)],
            },
            new TweakDef
            {
                Id = "dohpol-doh-enforcement-mode",
                Label = "Set DoH Policy Enforcement Mode (Require=3)",
                Category = "DoH Enforcement Policy",
                Description =
                    "Sets DoHEnforcementMode=3 in the policy to ensure DoH is required across all network profiles, complementing EnableAutoDoh for comprehensive enforcement.",
                Tags = ["doh", "dns", "enforcement", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Sets policy enforcement mode to require for all profiles.",
                ApplyOps = [RegOp.SetDword(Key, "DoHEnforcementMode", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "DoHEnforcementMode")],
                DetectOps = [RegOp.CheckDword(Key, "DoHEnforcementMode", 3)],
            },
            new TweakDef
            {
                Id = "dohpol-block-plain-dns-fallback",
                Label = "Block Plaintext DNS Fallback",
                Category = "DoH Enforcement Policy",
                Description = "Prevents the DNS client from falling back to unencrypted UDP/TCP port-53 DNS queries when DoH is unavailable.",
                Tags = ["doh", "dns", "fallback", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "No plaintext DNS fallback; DNS fails if DoH resolver is down.",
                ApplyOps = [RegOp.SetDword(Key, "BlockPlainDNSFallback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPlainDNSFallback")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPlainDNSFallback", 1)],
            },
            new TweakDef
            {
                Id = "dohpol-enable-dnssec-validation",
                Label = "Enable DNSSEC Validation",
                Category = "DoH Enforcement Policy",
                Description =
                    "Enables DNSSEC (Domain Name System Security Extensions) validation to authenticate DNS responses and detect tampering.",
                Tags = ["doh", "dnssec", "dns", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "DNSSEC validates DNS responses; may break domains with misconfigured DNSSEC records.",
                ApplyOps = [RegOp.SetDword(Key, "EnableDNSSEC", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDNSSEC")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDNSSEC", 1)],
            },
            new TweakDef
            {
                Id = "dohpol-disable-llmnr",
                Label = "Disable LLMNR (Link-Local Multicast Name Resolution)",
                Category = "DoH Enforcement Policy",
                Description =
                    "Disables LLMNR to prevent mDNS-style name poisoning attacks (Responder tool) by eliminating the fallback multicast name resolution protocol.",
                Tags = ["doh", "llmnr", "dns", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Eliminates LLMNR; mitigates LLMNR-poisoning attacks (Responder). Local hostname resolution falls back to NetBIOS if not disabled.",
                ApplyOps = [RegOp.SetDword(Key, "EnableMulticast", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMulticast")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMulticast", 0)],
            },
            new TweakDef
            {
                Id = "dohpol-disable-netbios-name-resolution",
                Label = "Disable NetBIOS Name Resolution",
                Category = "DoH Enforcement Policy",
                Description =
                    "Disables NetBIOS name resolution over TCP/IP to prevent NBNS poisoning attacks. Clients must use DNS or DoH for name resolution.",
                Tags = ["doh", "netbios", "dns", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Disables NBNS; legacy applications relying on NetBIOS names may fail.",
                ApplyOps = [RegOp.SetDword(Key, "DisableNetBiosNameResolution", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNetBiosNameResolution")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNetBiosNameResolution", 1)],
            },
            new TweakDef
            {
                Id = "dohpol-enforce-doh-all-profiles",
                Label = "Enforce DoH Across All Network Profiles",
                Category = "DoH Enforcement Policy",
                Description =
                    "Enforces DoH on all network profiles (domain, private, public) rather than individual adapters, ensuring uniform encryption.",
                Tags = ["doh", "dns", "profiles", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Uniform DoH enforcement; no profile bypass allowed.",
                ApplyOps = [RegOp.SetDword(Key, "EnforceDoHAllProfiles", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceDoHAllProfiles")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceDoHAllProfiles", 1)],
            },
            new TweakDef
            {
                Id = "dohpol-require-signed-dns-responses",
                Label = "Require Signed DNS Responses",
                Category = "DoH Enforcement Policy",
                Description =
                    "Requires cryptographically signed DNS responses, rejecting unsigned answers to prevent DNS spoofing and cache poisoning attacks.",
                Tags = ["doh", "dns", "signing", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Unsigned DNS responses rejected; requires DNSSEC-signed zones for all resolved domains.",
                ApplyOps = [RegOp.SetDword(Key, "RequireSignedDNSResponses", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedDNSResponses")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSignedDNSResponses", 1)],
            },
            new TweakDef
            {
                Id = "dohpol-doh-query-timeout",
                Label = "Set DoH Query Timeout to 30 Seconds",
                Category = "DoH Enforcement Policy",
                Description =
                    "Sets the DoH query timeout to 30 seconds before the resolver fails, ensuring consistent behaviour across slow or degraded DoH server connections.",
                Tags = ["doh", "dns", "timeout", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "30-second DoH timeout; reduces DNS failures on degraded connections.",
                ApplyOps = [RegOp.SetDword(Key, "DoHQueryTimeout", 30)],
                RemoveOps = [RegOp.DeleteValue(Key, "DoHQueryTimeout")],
                DetectOps = [RegOp.CheckDword(Key, "DoHQueryTimeout", 30)],
            },
            new TweakDef
            {
                Id = "dohpol-disable-doh-cache",
                Label = "Disable DNS-over-HTTPS Cache",
                Category = "DoH Enforcement Policy",
                Description =
                    "Disables caching of DoH resolver responses in the DNS client cache, ensuring fresh lookups and preventing stale cached records from being served.",
                Tags = ["doh", "dns", "cache", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Every DoH lookup hits the resolver; may increase latency on high-traffic clients.",
                ApplyOps = [RegOp.SetDword(Key, "DisableDoHCache", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDoHCache")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDoHCache", 1)],
            },
        ];
}
