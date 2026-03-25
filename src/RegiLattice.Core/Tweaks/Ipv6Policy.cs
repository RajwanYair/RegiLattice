// RegiLattice.Core — Tweaks/Ipv6Policy.cs
// Sprint 316: IPv6 Policy tweaks (10 tweaks)
// Category: "IPv6 Policy" | Slug: ipv6pol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Tcpip\Parameters

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Ipv6Policy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Tcpip\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ipv6pol-disable-ipv6",
            Label = "Disable IPv6 on All Network Adapters",
            Category = "IPv6 Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "IPv6 provides the next generation internet protocol addressing and communication capabilities alongside or replacing IPv4. Disabling IPv6 on all interfaces prevents the endpoint from using IPv6 for any network communication and removes associated attack surfaces. IPv6 tunneling protocols including Teredo and 6to4 can bypass IPv4-based network security controls by encapsulating IPv6 within IPv4. Networks not prepared to monitor and filter IPv6 traffic have security gaps when endpoints use IPv6 active alongside IPv4. Enterprise environments that do not operate IPv6 network infrastructure should disable IPv6 to prevent protocol confusion and bypass risks. Organizations actively deploying IPv6 should not apply this tweak and should instead configure IPv6 security controls appropriately.",
            Tags = ["ipv6", "network", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableIPv6", 0xFF)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableIPv6")],
            DetectOps = [RegOp.CheckDword(Key, "DisableIPv6", 0xFF)],
        },
        new TweakDef
        {
            Id = "ipv6pol-disable-teredo",
            Label = "Disable Teredo IPv6 Tunnel",
            Category = "IPv6 Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Teredo is an IPv6 tunneling protocol that encapsulates IPv6 traffic inside UDP packets enabling IPv6 connectivity through IPv4 NAT devices. Disabling Teredo prevents endpoints from establishing IPv6 tunnels that can bypass IPv4-based network security controls. Teredo tunnels traverse NAT and firewall devices that are configured for IPv4 only creating unmonitored network paths. Malware uses Teredo to establish IPv6-based command and control connections that bypass IPv4-only security monitoring. Enterprise networks should use native IPv6 or approved tunnel mechanisms rather than automatic tunnel adapters like Teredo. Disabling Teredo does not affect native IPv6 connectivity or explicitly configured enterprise IPv6 tunnels.",
            Tags = ["ipv6", "teredo", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTeredo", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTeredo")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTeredo", 1)],
        },
        new TweakDef
        {
            Id = "ipv6pol-disable-6to4",
            Label = "Disable 6to4 IPv6 Tunnel Adapter",
            Category = "IPv6 Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "6to4 is an automatic IPv6 tunneling mechanism that encapsulates IPv6 packets inside IPv4 packets using protocol 41. Disabling 6to4 prevents automatic IPv6 tunnel creation through the 6to4 relay infrastructure operated by third parties. 6to4 relay servers on the internet are not controlled by enterprise network administrators and represent unmonitored egress paths. Traffic through 6to4 relays bypasses enterprise security controls that operate on the IPv4 network layer. The 6to4 mechanism has known security weaknesses including inability to verify the identity of relay servers used for the tunnel. Disabling 6to4 is recommended for enterprise networks that do not require automatic IPv6 tunnel establishment.",
            Tags = ["ipv6", "6to4", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "Disable6to4", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "Disable6to4")],
            DetectOps = [RegOp.CheckDword(Key, "Disable6to4", 1)],
        },
        new TweakDef
        {
            Id = "ipv6pol-disable-isatap",
            Label = "Disable ISATAP IPv6 Tunnel",
            Category = "IPv6 Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Intra-Site Automatic Tunnel Addressing Protocol enables IPv6 communication within IPv4 intranets by creating automatic virtual IPv6 adapters. Disabling ISATAP prevents automatic creation of IPv6 tunnels within the enterprise intranet using the ISATAP mechanism. ISATAP tunnels are typically managed by enterprise IT but the automatic discovery and tunnel creation aspects reduce IT control. Legacy ISATAP deployments represent a transition technology that should be replaced by native IPv6 as part of enterprise IPv6 planning. Endpoints with ISATAP enabled have additional network interfaces that must be independently secured and monitored. Disabling ISATAP simplifies network adapter management and reduces the number of active network interfaces requiring security consideration.",
            Tags = ["ipv6", "isatap", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableISATAP", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableISATAP")],
            DetectOps = [RegOp.CheckDword(Key, "DisableISATAP", 1)],
        },
        new TweakDef
        {
            Id = "ipv6pol-prefer-ipv4",
            Label = "Prefer IPv4 over IPv6 by Default",
            Category = "IPv6 Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Windows preferentially uses IPv6 for network connections when both IPv4 and IPv6 addresses are available for a destination. Setting IPv4 preference ensures that endpoint connections use the enterprise-monitored IPv4 network rather than IPv6 when both are available. Enterprise network security monitoring is often more mature for IPv4 than IPv6 and IPv4 preference ensures traffic goes through monitored paths. IPv6-only destinations remain reachable through direct IPv6 connections while dual-stack destinations prefer IPv4 paths. This is a transitional setting appropriate for environments where IPv6 security monitoring lags IPv4 capability. When IPv6 monitoring is fully operational this preference setting can be removed to allow natural protocol selection.",
            Tags = ["ipv6", "preference", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreferIpv4OverIpv6", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreferIpv4OverIpv6")],
            DetectOps = [RegOp.CheckDword(Key, "PreferIpv4OverIpv6", 1)],
        },
        new TweakDef
        {
            Id = "ipv6pol-disable-dhcpv6",
            Label = "Disable DHCPv6 Client",
            Category = "IPv6 Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "DHCPv6 provides automatic IPv6 address configuration for endpoints on networks with DHCPv6 servers. Disabling DHCPv6 prevents the endpoint from receiving IPv6 address assignments from DHCPv6 servers on the network. Rogue DHCPv6 servers can be set up to respond faster than legitimate servers and provide malicious IPv6 gateway and DNS configurations. DHCP starvation and rogue server attacks can redirect endpoint traffic through attacker-controlled infrastructure. Enterprises that do not deploy DHCPv6 infrastructure have no need for DHCPv6 client functionality on endpoints. Static IPv6 addressing or SLAAC can be used for legitimate IPv6 needs without DHCPv6 client-side exposure.",
            Tags = ["ipv6", "dhcpv6", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDHCPv6", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDHCPv6")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDHCPv6", 1)],
        },
        new TweakDef
        {
            Id = "ipv6pol-disable-ipv6-multicast",
            Label = "Disable IPv6 Multicast Listener Discovery",
            Category = "IPv6 Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "IPv6 Multicast Listener Discovery allows endpoints to announce multicast group subscriptions to routers and other endpoints. Disabling IPv6 MLD prevents the endpoint from participating in IPv6 multicast groups and announcing multicast subscriptions. IPv6 multicast subscriptions are broadcast to the network segment and can reveal information about applications and services running on the endpoint. Multicast-based discovery protocols expose service information that can aid network reconnaissance. Endpoints that do not require IPv6 multicast functionality have no need for MLD participation and can reduce network protocol exposure. This setting reduces the IPv6 protocol footprint on networks where multicast is not required.",
            Tags = ["ipv6", "multicast", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableIPv6Multicast", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableIPv6Multicast")],
            DetectOps = [RegOp.CheckDword(Key, "DisableIPv6Multicast", 1)],
        },
        new TweakDef
        {
            Id = "ipv6pol-disable-privacy-extensions",
            Label = "Disable IPv6 Privacy Extensions",
            Category = "IPv6 Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "IPv6 privacy extensions generate random IPv6 addresses that change periodically to prevent tracking of endpoints by external observers. Disabling IPv6 privacy extensions causes the endpoint to use its permanent EUI-64 based IPv6 address derived from the MAC address. In enterprise environments endpoint tracking through IPv6 addresses may be required for network security monitoring and incident response. Security tools that map IPv6 addresses to endpoints require consistent addresses that do not rotate to function correctly. Privacy extensions interfere with network access control systems that enforce policy based on endpoint IPv6 address identity. Disabling privacy extensions maintains consistent IPv6 address associations needed for enterprise monitoring and access control.",
            Tags = ["ipv6", "privacy", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrivacyExtensions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivacyExtensions")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrivacyExtensions", 1)],
        },
        new TweakDef
        {
            Id = "ipv6pol-restrict-ipv6-scope",
            Label = "Restrict IPv6 to Site-Local Scope",
            Category = "IPv6 Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "IPv6 addresses can have global scope allowing them to be routed to the internet or site-local scope limiting them to the internal network. Restricting the endpoint to site-local scope IPv6 addresses prevents direct routed internet communication using the IPv6 global prefix. Global IPv6 addresses on enterprise endpoints bypass NAT-based perimeter security that restricts which internal systems have direct internet accessibility. Enterprises that provision globally routable IPv6 addresses on all endpoints provide attackers a direct path to each endpoint from the internet. Site-local IPv6 restricts internet-initiable connections requiring explicit routing policy to allow inbound IPv6 connections. This setting is appropriate for enterprise edges where inbound connection control is a security requirement.",
            Tags = ["ipv6", "scope", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictGlobalIPv6", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictGlobalIPv6")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictGlobalIPv6", 1)],
        },
        new TweakDef
        {
            Id = "ipv6pol-disable-ipv6-transition-mechs",
            Label = "Disable IPv6 Transition Technologies",
            Category = "IPv6 Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "IPv6 transition technologies including Teredo 6to4 ISATAP and other tunneling mechanisms allow IPv6 over IPv4 infrastructure. Disabling all transition technologies with a single setting prevents any automatically configured IPv6 tunnel from activating. Individual disabling of each transition mechanism provides defense-in-depth but this comprehensive setting ensures complete coverage. Transition mechanisms are often used by attackers to bypass firewall controls that only filter IPv4 traffic. Enterprise networks should deploy native IPv6 through controlled infrastructure rather than automatic tunneling mechanisms. A comprehensive disable of transition technologies eliminates the risk of overlooking any specific mechanism while planning IPv6 deployment.",
            Tags = ["ipv6", "transition", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTransitionTechnologies", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTransitionTechnologies")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTransitionTechnologies", 1)],
        },
    ];
}
