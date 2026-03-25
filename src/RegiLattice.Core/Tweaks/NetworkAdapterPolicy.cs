// RegiLattice.Core — Tweaks/NetworkAdapterPolicy.cs
// Sprint 315: Network Adapter Policy tweaks (10 tweaks)
// Category: "Network Adapter Policy" | Slug: netadp
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkAdapterConfiguration

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkAdapterPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkAdapterConfiguration";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netadp-disable-netbios-over-tcp",
            Label = "Disable NetBIOS Over TCP/IP",
            Category = "Network Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "NetBIOS over TCP/IP enables legacy NetBIOS name resolution and communication over modern TCP/IP networks. Disabling NetBIOS over TCP/IP prevents exploitation of legacy name resolution protocols vulnerable to poisoning attacks. LLMNR and NetBIOS name poisoning attacks allow adversaries to intercept credential hashes transmitted during name resolution. Responder and similar tools exploit NetBIOS to capture NTLMv2 credential hashes from enterprise endpoints for offline cracking. Modern Windows networks use DNS for name resolution and do not require NetBIOS except for backward compatibility with legacy systems. Disabling NetBIOS reduces the attack surface for credential harvesting through network protocol exploitation.",
            Tags = ["network", "netbios", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NetbiosOptions", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "NetbiosOptions")],
            DetectOps = [RegOp.CheckDword(Key, "NetbiosOptions", 2)],
        },
        new TweakDef
        {
            Id = "netadp-disable-link-local-multicast",
            Label = "Disable LLMNR Protocol",
            Category = "Network Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Link-Local Multicast Name Resolution is a fallback name resolution protocol used when DNS fails to resolve names on local networks. Disabling LLMNR prevents the endpoint from broadcasting name resolution requests that can be spoofed by adversaries on the same network segment. LLMNR poisoning captures NTLMv2 hashes when endpoints broadcast failed DNS name requests to the local network. Responder tools can answer LLMNR requests for any name and capture authentication attempts including corporate credentials. Modern enterprise networks with properly configured DNS should not require LLMNR for name resolution. Disabling LLMNR is widely recommended by security frameworks and incident response teams for credential protection.",
            Tags = ["network", "llmnr", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "netadp-disable-connection-sharing",
            Label = "Disable Internet Connection Sharing",
            Category = "Network Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Internet Connection Sharing enables a Windows endpoint to act as a NAT gateway providing internet access to devices connected through the endpoint. Disabling ICS prevents enterprise endpoints from acting as network bridges between different network interfaces. ICS on corporate endpoints creates unmonitored network paths that bypass enterprise network security controls. Devices sharing internet through an enterprise endpoint bypass NAC and network policy enforcement designed for the enterprise perimeter. ICS-enabled endpoints create complex network topologies that complicate network security monitoring and traffic analysis. Endpoint network functionality is unaffected by disabling ICS as corporate network access does not depend on endpoint-to-endpoint NAT.",
            Tags = ["network", "sharing", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableICS", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableICS")],
            DetectOps = [RegOp.CheckDword(Key, "DisableICS", 1)],
        },
        new TweakDef
        {
            Id = "netadp-disable-rss-offloading",
            Label = "Disable Receive Side Scaling Offloading",
            Category = "Network Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 4,
            Description =
                "Receive Side Scaling distributes network packet processing across multiple CPU cores using hardware offloading in network adapter firmware. Disabling RSS offloading forces network processing to occur entirely in the OS kernel stack rather than hardware acceleration. Hardware network offloading in NIC firmware can be exploited through specially crafted packets that trigger vulnerabilities in NIC firmware code. NIC firmware is often less frequently updated than OS kernel components creating longer vulnerability windows. In very high security environments the additional isolation from running network processing in kernel code rather than offload hardware may be preferred. Most enterprise environments benefit from keeping RSS enabled for performance but high-assurance environments may require kernel-only processing.",
            Tags = ["network", "rss", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableRss", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRss")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRss", 1)],
        },
        new TweakDef
        {
            Id = "netadp-disable-tcp-chimney",
            Label = "Disable TCP Chimney Offload",
            Category = "Network Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 4,
            Description =
                "TCP Chimney Offload moves TCP processing from the Windows network stack to network adapter hardware for improved throughput. Disabling TCP Chimney prevents network adapters from processing TCP connections in NIC firmware rather than the Windows kernel. TCP offload engine bugs have caused network instability issues and have been identified as potential security concerns in some adapter implementations. Offloaded TCP connections bypass some Windows network stack protections and monitoring capabilities that rely on kernel-level traffic processing. Network security monitoring tools that operate at the kernel level may not see offloaded TCP traffic. Environments using deep packet inspection or kernel-level network monitoring should evaluate whether TCP chimney offload conflicts with their monitoring requirements.",
            Tags = ["network", "tcp", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTCPChimney", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTCPChimney")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTCPChimney", 1)],
        },
        new TweakDef
        {
            Id = "netadp-restrict-winsock-access",
            Label = "Restrict Winsock Application Access",
            Category = "Network Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Winsock application programmable interfaces provide network access to applications and are a primary mechanism for application-level network communication. Restricting Winsock access from standard user programs prevents unauthorized network applications from initiating connections. Malware commonly uses Winsock APIs to establish command and control connections and exfiltrate data. Restricting Winsock to administrator-approved applications provides a network-level application allowlisting mechanism. Combined with application control policies Winsock restrictions can prevent unauthorized applications from accessing the network. Legitimate enterprise applications with required network access should be explicitly permitted rather than relying on blanket Winsock accessibility.",
            Tags = ["network", "winsock", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictWinsockAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictWinsockAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictWinsockAccess", 1)],
        },
        new TweakDef
        {
            Id = "netadp-disable-offload-checksum",
            Label = "Disable Network Checksum Offloading",
            Category = "Network Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 4,
            Description =
                "Checksum offloading allows network adapters to calculate TCP and IP checksums in hardware rather than in the Windows network stack. Disabling checksum offloading ensures that all packet integrity calculations are performed in the Windows kernel rather than NIC firmware. Checksum offloading can interfere with network packet capture and analysis tools that rely on correct checksums in captured traffic. Wireshark and similar monitoring tools often show checksum errors on systems with offloading enabled complicating network troubleshooting. In network monitoring environments disabling offloading simplifies packet analysis by ensuring checksums reflect actual packet content. This setting trades minor performance for improved network monitoring accuracy and reduced NIC firmware exposure.",
            Tags = ["network", "checksum", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableChecksumOffload", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableChecksumOffload")],
            DetectOps = [RegOp.CheckDword(Key, "DisableChecksumOffload", 1)],
        },
        new TweakDef
        {
            Id = "netadp-disable-auto-tuning",
            Label = "Disable Network Auto-Tuning",
            Category = "Network Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 4,
            Description =
                "Windows TCP Auto-Tuning dynamically adjusts TCP receive window sizes based on current network conditions for optimal throughput. Disabling auto-tuning sets a fixed TCP receive window size preventing dynamic adjustment of connection parameters. Automatically tuned TCP parameters can behave inconsistently and may produce unexpected traffic patterns that complicate network monitoring. Fixed window sizes produce predictable network behavior that is easier to analyze and troubleshoot. Some network monitoring environments and VPN solutions experience compatibility issues with auto-tuned TCP windows. Organizations experiencing network issues related to TCP auto-tuning behavior should evaluate disabling auto-tuning as a compatibility measure.",
            Tags = ["network", "tcp", "tuning", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoTuning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoTuning")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoTuning", 1)],
        },
        new TweakDef
        {
            Id = "netadp-enable-arp-protection",
            Label = "Enable ARP Spoofing Protection",
            Category = "Network Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "ARP spoofing attacks send fabricated ARP responses to associate the attacker's MAC address with legitimate IP addresses enabling man-in-the-middle attacks. Enabling ARP protection prevents the endpoint from updating its ARP cache with unsolicited or conflicting ARP replies. ARP poisoning is used to intercept traffic between a victim endpoint and its network gateway capturing unencrypted communications. ARP-based MitM attacks target credential theft in environments still using unencrypted protocols like HTTP or SMBv1. Enterprise segment security controls and endpoint ARP inspection together provide defense-in-depth against ARP poisoning. ARP protection ensures that the endpoint maintains an accurate ARP cache based on legitimate responses.",
            Tags = ["network", "arp", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableArpProtection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableArpProtection")],
            DetectOps = [RegOp.CheckDword(Key, "EnableArpProtection", 1)],
        },
        new TweakDef
        {
            Id = "netadp-disable-adapter-bridging",
            Label = "Disable Network Adapter Bridging",
            Category = "Network Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Network adapter bridging allows Windows to connect two different network adapters at layer 2 creating a bridge between separate network segments. Disabling adapter bridging prevents endpoints from creating layer 2 connections between corporate and non-corporate network segments. Bridged adapters allow traffic to flow between network segments that should remain isolated by network security controls. A bridge between a corporate wired network and an unmanaged wireless network bypasses segment isolation and monitoring. Network segmentation is a fundamental enterprise security control for limiting lateral movement and isolating sensitive systems. Endpoint bridging capabilities should be disabled unless there is a documented operational requirement with appropriate security review.",
            Tags = ["network", "bridging", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBridging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBridging")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBridging", 1)],
        },
    ];
}
