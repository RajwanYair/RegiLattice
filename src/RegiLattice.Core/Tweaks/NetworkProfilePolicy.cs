// RegiLattice.Core — Tweaks/NetworkProfilePolicy.cs
// Sprint 327: Network Profile Policy tweaks (10 tweaks)
// Category: "Network Profile Policy" | Slug: netprof
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivity

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkProfilePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivity";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netprof-block-auto-profile-change",
            Label = "Block Automatic Network Location Profile Changes",
            Category = "Network Profile Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Windows automatically changes network location profiles between Domain, Private, and Public based on domain connectivity detection. Blocking automatic profile changes prevents Windows from downgrading a network from Domain profile to Public profile when domain connectivity is temporarily unavailable. Automatic profile downgrades can trigger firewall rule changes that open ports normally restricted in Private or Public profiles. Malicious actors can cause profile downgrades by disrupting domain controller connectivity causing firewall rules to expand. Enterprise endpoints should maintain their configured profile regardless of transient connectivity issues. Stabilizing network profiles prevents unexpected firewall changes that could expose services not intended for the current network location.",
            Tags = ["network-profile", "firewall", "domain", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockAutoProfileChange", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockAutoProfileChange")],
            DetectOps = [RegOp.CheckDword(Key, "BlockAutoProfileChange", 1)],
        },
        new TweakDef
        {
            Id = "netprof-enforce-domain-profile",
            Label = "Enforce Domain Network Profile on Managed Endpoints",
            Category = "Network Profile Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Enforcing the Domain network profile ensures that managed endpoints use the most restrictive and appropriate firewall configuration for enterprise networks. Domain profile enforcement applies consistent firewall rules regardless of current network connectivity state or physical location. Enterprise endpoints should use Domain profile configuration which typically disables unnecessary services and restricts inbound connections. Users on VPN or remote connections may experience Domain profile rules applied through Network Access Protection or similar mechanisms. Domain profile enforcement prevents users from changing network profiles to Public or Private which have different firewall rule sets. Consistent profile enforcement ensures that endpoint security posture does not change based on network location.",
            Tags = ["network-profile", "domain", "firewall", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceDomainProfile", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceDomainProfile")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceDomainProfile", 1)],
        },
        new TweakDef
        {
            Id = "netprof-disable-ncsi-telemetry",
            Label = "Disable Network Connectivity Status Indicator Telemetry",
            Category = "Network Profile Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "The Network Connectivity Status Indicator performs periodic HTTP probes to Microsoft servers to determine internet connectivity status. Disabling NCSI telemetry stops the automatic connectivity checks to Microsoft-hosted probe endpoints that send device information. NCSI probes reveal information about network topology, device presence, and connectivity patterns to Microsoft cloud infrastructure. In air-gapped or restricted environments NCSI probes to external servers may violate network isolation requirements. Proxy auto-detection relies on NCSI for detection which can be reconfigured to use internal probe endpoints instead. Organizations with strict data residency or network traffic policies should configure NCSI to use internal probe hosts or disable external probing.",
            Tags = ["network-profile", "ncsi", "telemetry", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNCSITelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNCSITelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNCSITelemetry", 1)],
        },
        new TweakDef
        {
            Id = "netprof-set-internal-probe-host",
            Label = "Configure Internal NCSI Probe Host",
            Category = "Network Profile Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Configuring an internal NCSI probe host redirects Windows connectivity checks to an enterprise-hosted endpoint instead of Microsoft servers. An internal probe host allows NCSI connectivity determination without any traffic leaving the enterprise network boundary. Organizations with strict network egress controls can use an internal web server to respond to NCSI HTTP probes. The NCSI probe accesses http://[host]/ncsi.txt and checks the response content to determine connectivity status. Internal probe hosts should be highly available as NCSI determines whether the network location is considered connected. Configuring internal probes improves air-gapped environment support and ensures connectivity determination works without external access.",
            Tags = ["network-profile", "ncsi", "internal", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetString(Key, "DefaultInternetProbeHost", "")],
            RemoveOps = [RegOp.DeleteValue(Key, "DefaultInternetProbeHost")],
            DetectOps = [RegOp.CheckMissing(Key, "DefaultInternetProbeHost")],
        },
        new TweakDef
        {
            Id = "netprof-restrict-profile-user-change",
            Label = "Prevent Users from Changing Network Profile",
            Category = "Network Profile Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Preventing users from changing network profiles ensures that only administrators can modify the network location affecting firewall rule application. User-initiated profile changes from Domain to Public or Private could inadvertently apply incorrect firewall rules to managed endpoints. Users may change profiles to bypass firewall restrictions that apply in Domain profile mode. Restricting profile changes to administrators provides consistent network security posture enforcement across all enterprise endpoints. The network profile affects which Windows Firewall rule sets are active so profile changes have direct security consequences. Endpoint protection tools should monitor for unauthorized profile changes as a security configuration drift indicator.",
            Tags = ["network-profile", "user-restriction", "firewall", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictProfileUserChange", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictProfileUserChange")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictProfileUserChange", 1)],
        },
        new TweakDef
        {
            Id = "netprof-disable-passive-polling",
            Label = "Disable Passive Network Location Polling",
            Category = "Network Profile Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Passive network location polling continuously checks for changes in network connectivity status by monitoring DNS and domain controller availability. Disabling passive polling reduces network traffic generated by frequent connectivity checks on stable enterprise connections. Passive polling can generate significant domain controller traffic on large enterprises multiplied across thousands of endpoints. Enterprise environments with stable domain connectivity do not benefit from frequent location polling and the associated network overhead. Disabling passive polling reduces background noise in network monitoring tools that capture endpoint-to-domain-controller communications. Network profile stability policies combined with reduced polling frequency provide a cleaner network baseline for anomaly detection.",
            Tags = ["network-profile", "polling", "performance", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePassivePolling", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePassivePolling")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePassivePolling", 1)],
        },
        new TweakDef
        {
            Id = "netprof-block-multiple-active-profiles",
            Label = "Prevent Multiple Concurrent Active Network Profiles",
            Category = "Network Profile Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Multiple concurrent active network profiles can create inconsistent firewall rule application when different interfaces use different profiles. Blocking multiple active profiles prevents scenarios where one interface is Domain profile and another is Public profile simultaneously. Multi-homed endpoints with different profiles on different interfaces can expose services through the more permissive profile's firewall rules. Consistent single-profile enforcement ensures that firewall rules apply uniformly regardless of which interface receives traffic. Enterprise endpoints with both wired and wireless interfaces should maintain a consistent profile across all network interfaces. Network profile consistency monitoring helps identify multi-homed configurations that may create unintended security exposure.",
            Tags = ["network-profile", "multi-homed", "firewall", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockMultipleActiveProfiles", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockMultipleActiveProfiles")],
            DetectOps = [RegOp.CheckDword(Key, "BlockMultipleActiveProfiles", 1)],
        },
        new TweakDef
        {
            Id = "netprof-log-profile-changes",
            Label = "Enable Network Profile Change Event Logging",
            Category = "Network Profile Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Network profile change event logging records when the network location profile changes providing audit trail for security and compliance. Enabling profile change logging generates Windows events when profiles transition between Domain, Private, and Public. Profile change events correlated with user logon and network connection events help identify unauthorized profile manipulation. Security teams can monitor for unexpected profile changes that may indicate tampering with network security controls. Profile change events in the Windows Event Log provide timeline context for investigating Security incidents where profile changes preceded exposure. SIEM alerts on unexpected profile changes from Domain to Public can provide real-time detection of potential network security policy bypass.",
            Tags = ["network-profile", "logging", "audit", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableProfileChangeLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableProfileChangeLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableProfileChangeLogging", 1)],
        },
        new TweakDef
        {
            Id = "netprof-require-domain-auth-for-domain-profile",
            Label = "Require Domain Authentication for Domain Network Profile",
            Category = "Network Profile Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Requiring domain authentication for Domain network profile assignment prevents endpoints from claiming Domain profile status without actual domain connectivity. Domain profile assignment based on DNS suffix or network properties alone can be spoofed by attackers who configure a rogue network with matching DNS. Requiring domain authentication ensures the Domain profile is only assigned when the endpoint can authenticate with a domain controller. Without this requirement public networks with the same DNS suffix as an enterprise could trigger Domain profile and apply more permissive firewall rules. Domain authentication-based profile assignment provides a stronger identity anchor for network profile determination. This policy is particularly important for endpoints that roam between trusted and untrusted networks.",
            Tags = ["network-profile", "domain-auth", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireDomainAuthForDomainProfile", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireDomainAuthForDomainProfile")],
            DetectOps = [RegOp.CheckDword(Key, "RequireDomainAuthForDomainProfile", 1)],
        },
        new TweakDef
        {
            Id = "netprof-set-unidentified-networks-to-public",
            Label = "Set Unidentified Networks to Public Profile",
            Category = "Network Profile Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Unidentified networks that cannot be categorized as Domain or Private should default to the Public profile which applies the most restrictive firewall rules. Setting unidentified networks to Public prevents endpoints connected to unknown networks from using Domain or Private profile firewall rules. When an endpoint connects to an unknown network the Public profile blocks most inbound connections protecting the endpoint from network threats. Defaulting unidentified networks to Public is particularly important for laptops that may connect to hotel, conference, or public wireless networks. The Public profile's restrictive firewall rules provide default protection against network-level attacks while connected to unidentified networks. This fail-secure default ensures that unknown network environments do not inherit the more permissive Domain or Private profiles.",
            Tags = ["network-profile", "public-profile", "firewall", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SetUnidentifiedNetworksToPublic", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SetUnidentifiedNetworksToPublic")],
            DetectOps = [RegOp.CheckDword(Key, "SetUnidentifiedNetworksToPublic", 1)],
        },
    ];
}
