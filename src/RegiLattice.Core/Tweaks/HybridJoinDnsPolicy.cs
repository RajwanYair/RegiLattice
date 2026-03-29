// RegiLattice.Core — Tweaks/HybridJoinDnsPolicy.cs
// Hybrid Azure AD Join DNS suffix configuration, SRV/CNAME resolution, and network detection — Sprint 461.
// Category: "Hybrid Join DNS Policy" | Slug: hjdns
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\NetworkIsolation
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudAuthentication\HybridJoin

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class HybridJoinDnsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkIsolation";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudAuthentication\HybridJoin";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "hjdns-enable-direct-hybrid-join",
                Label = "Enable Managed Domain Hybrid Join (No ADFS)",
                Category = "Hybrid Join DNS Policy",
                Description =
                    "Enables direct Hybrid Azure AD Join for managed domains without AD FS federation, allowing devices to register with AAD using username/password and SCP discovery.",
                Tags = ["hybrid-join", "azure-ad", "managed-domain", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Hybrid join enabled for managed domains; no AD FS redirect required.",
                ApplyOps = [RegOp.SetDword(Key2, "EnableDirectHybridJoin", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "EnableDirectHybridJoin")],
                DetectOps = [RegOp.CheckDword(Key2, "EnableDirectHybridJoin", 1)],
            },
            new TweakDef
            {
                Id = "hjdns-block-unregistered-domain-devices",
                Label = "Block Hybrid Join from Unregistered DNS Domains",
                Category = "Hybrid Join DNS Policy",
                Description =
                    "Prevents devices in DNS domains not listed in the Hybrid Join SCP from attempting to register with Azure AD, blocking rogue machines on unknown domains from joining.",
                Tags = ["hybrid-join", "dns", "domain", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Only devices in registered DNS domains can hybrid join; rogue domain machines blocked.",
                ApplyOps = [RegOp.SetDword(Key2, "BlockUnregisteredDomainJoin", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "BlockUnregisteredDomainJoin")],
                DetectOps = [RegOp.CheckDword(Key2, "BlockUnregisteredDomainJoin", 1)],
            },
            new TweakDef
            {
                Id = "hjdns-force-scp-lookup",
                Label = "Force Service Connection Point Lookup for Hybrid Join",
                Category = "Hybrid Join DNS Policy",
                Description =
                    "Forces Azure AD Hybrid Join to use Service Connection Point (SCP) in Active Directory for tenant discovery instead of the local registry, ensuring centrally managed tenant targeting.",
                Tags = ["hybrid-join", "scp", "active-directory", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "SCP-based tenant discovery enforced; client-side tenant overrides ignored.",
                ApplyOps = [RegOp.SetDword(Key2, "ForceSCPLookupForTenantDiscovery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "ForceSCPLookupForTenantDiscovery")],
                DetectOps = [RegOp.CheckDword(Key2, "ForceSCPLookupForTenantDiscovery", 1)],
            },
            new TweakDef
            {
                Id = "hjdns-disable-cloud-ap-tenant-override",
                Label = "Disable Cloud-AP Tenant Override for Hybrid Join",
                Category = "Hybrid Join DNS Policy",
                Description =
                    "Blocks user-level Cloud AP (Azure AD Authentication Plugin) tenant override that can redirect a device's hybrid join to a different AAD tenant ID.",
                Tags = ["hybrid-join", "cloud-ap", "tenant", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Tenant redirect via Cloud AP blocked; join target comes from SCP or policy only.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableCloudAPTenantOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableCloudAPTenantOverride")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableCloudAPTenantOverride", 1)],
            },
            new TweakDef
            {
                Id = "hjdns-isolate-enterprise-endpoints",
                Label = "Isolate Enterprise Network Endpoints for Cloud Authentication",
                Category = "Hybrid Join DNS Policy",
                Description =
                    "Configures Network Isolation policy to classify Microsoft cloud authentication endpoints as enterprise-owned, enabling Windows Information Protection to treat AAD traffic as internal.",
                Tags = ["hybrid-join", "network-isolation", "wip", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AAD endpoints classified as enterprise; WIP-enforced devices route auth traffic correctly.",
                ApplyOps = [RegOp.SetDword(Key, "EnterpriseCloudResources", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnterpriseCloudResources")],
                DetectOps = [RegOp.CheckDword(Key, "EnterpriseCloudResources", 1)],
            },
            new TweakDef
            {
                Id = "hjdns-block-non-domain-dns-fallback",
                Label = "Block Non-Domain DNS Fallback During Hybrid Join",
                Category = "Hybrid Join DNS Policy",
                Description =
                    "Prevents the hybrid join process from falling back to public DNS resolvers when the on-premises DNS server is unavailable, ensuring join only proceeds with trusted DNS resolution.",
                Tags = ["hybrid-join", "dns-fallback", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hybrid join aborts if domain DNS unreachable; prevents join to wrong tenant via public DNS.",
                ApplyOps = [RegOp.SetDword(Key2, "BlockPublicDNSFallback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "BlockPublicDNSFallback")],
                DetectOps = [RegOp.CheckDword(Key2, "BlockPublicDNSFallback", 1)],
            },
            new TweakDef
            {
                Id = "hjdns-require-line-of-sight",
                Label = "Require DC Line-of-Sight for Hybrid Join",
                Category = "Hybrid Join DNS Policy",
                Description =
                    "Requires line-of-sight to a domain controller (DC availability check) before allowing the hybrid join registration task to execute, preventing join failures when offline.",
                Tags = ["hybrid-join", "domain-controller", "offline", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Join task only runs with DC reachable; remote-only machines need internet direct join instead.",
                ApplyOps = [RegOp.SetDword(Key2, "RequireDCLineOfSight", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "RequireDCLineOfSight")],
                DetectOps = [RegOp.CheckDword(Key2, "RequireDCLineOfSight", 1)],
            },
            new TweakDef
            {
                Id = "hjdns-set-join-timeout",
                Label = "Set Hybrid Join Task Timeout to 90 Seconds",
                Category = "Hybrid Join DNS Policy",
                Description =
                    "Caps the Hybrid Azure AD Join registration task at 90 seconds, preventing long hangs at logon when the join endpoint is unreachable.",
                Tags = ["hybrid-join", "timeout", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Join task times out after 90 seconds; logon not blocked indefinitely if AAD is unreachable.",
                ApplyOps = [RegOp.SetDword(Key2, "RegistrationTaskTimeoutSeconds", 90)],
                RemoveOps = [RegOp.DeleteValue(Key2, "RegistrationTaskTimeoutSeconds")],
                DetectOps = [RegOp.CheckDword(Key2, "RegistrationTaskTimeoutSeconds", 90)],
            },
            new TweakDef
            {
                Id = "hjdns-disable-joined-device-local-admin",
                Label = "Disable Local Admin Add via AAD Device Join",
                Category = "Hybrid Join DNS Policy",
                Description =
                    "Prevents the automatic addition of the joining user as a local administrator when a device is hybrid-joined to Azure AD, maintaining least-privilege on joined devices.",
                Tags = ["hybrid-join", "local-admin", "least-privilege", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Joining user not added to local admins on hybrid join; standard user account maintained.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableAutoLocalAdminOnJoin", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableAutoLocalAdminOnJoin")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableAutoLocalAdminOnJoin", 1)],
            },
            new TweakDef
            {
                Id = "hjdns-enable-hybrid-join-audit",
                Label = "Enable Hybrid Join Operation Audit Logging",
                Category = "Hybrid Join DNS Policy",
                Description =
                    "Enables detailed audit event logging for Hybrid Azure AD Join operations, recording device registration attempts, successes, and failures to the Windows event log.",
                Tags = ["hybrid-join", "audit", "logging", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hybrid join events logged; failed or suspicious join attempts are detectable.",
                ApplyOps = [RegOp.SetDword(Key2, "EnableHybridJoinAuditLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "EnableHybridJoinAuditLogging")],
                DetectOps = [RegOp.CheckDword(Key2, "EnableHybridJoinAuditLogging", 1)],
            },
        ];
}
