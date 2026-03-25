// RegiLattice.Core — Tweaks/NetCfgPolicy.cs
// Sprint 354: Network Configuration Policy tweaks (10 tweaks)
// Category: "Network Configuration Policy" | Slug: netcfg
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NCSI

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetCfgPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NCSI";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netcfg-disable-ncsi-active-probe",
            Label = "Disable Network Connectivity Status Indicator Active Internet Probe",
            Category = "Network Configuration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "The Network Connectivity Status Indicator performs active probes to Microsoft servers to determine internet connectivity status which generates regular outbound traffic to external Microsoft infrastructure. Disabling NCSI active probes stops the regular HTTP and DNS queries that NCSI sends to connectivity check endpoints preventing this traffic pattern from consuming network resources. Organizations that use network behavior analytics tools should be aware that NCSI traffic represents a baseline of normal traffic that should be filtered from anomaly detection. The NCSI probe traffic can reveal the presence of Windows systems on a network to passive security monitoring tools even in environments where active scanning is prohibited. Private network environments and air-gapped networks may generate security alerts when NCSI fails to connect to Microsoft servers. Organizations should evaluate whether disabling NCSI probes affects any software that relies on the NCSI network status indication for connectivity-dependent behavior.",
            Tags = ["ncsi", "active-probe", "internet-connectivity", "network-privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoActiveProbe", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoActiveProbe")],
            DetectOps = [RegOp.CheckDword(Key, "NoActiveProbe", 1)],
        },
        new TweakDef
        {
            Id = "netcfg-disable-ncsi-passive-polling",
            Label = "Disable NCSI Passive Network Polling for Network Status Detection",
            Category = "Network Configuration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "NCSI passive polling monitors network traffic to infer connectivity state without making active connections to connectivity check endpoints providing a less intrusive connectivity detection mechanism. Disabling passive polling stops NCSI from monitoring network traffic patterns to update its connectivity assessments between active probe cycles. Some network security tools flag the passive monitoring behavior of NCSI as anomalous traffic since it involves monitoring of network flows at the OS level. Organizations that conduct formal network security assessments should include NCSI behavior in their assessment scope to understand its traffic profile. Disabling both active probes and passive polling effectively turns off NCSI connectivity status reporting which may cause system tray indicators to show incorrect connectivity status. The impact of disabling NCSI should be evaluated for applications that use the Windows network connectivity notification API before applying this policy.",
            Tags = ["ncsi", "passive-polling", "network-monitoring", "connectivity", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePassivePolling", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePassivePolling")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePassivePolling", 1)],
        },
        new TweakDef
        {
            Id = "netcfg-configure-global-dns-suffix",
            Label = "Configure Global DNS Suffix for Network Name Resolution Policy",
            Category = "Network Configuration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Configuring the global DNS suffix search list through network configuration policy ensures consistent name resolution behavior across all domain members regardless of individual DNS client configuration. A controlled DNS suffix list prevents users from encountering DNS resolution issues caused by missing suffixes and ensures that organizational resource names are resolved through the correct DNS namespace. The global suffix configuration should list all organizational DNS domains in priority order to ensure efficient name resolution. DNS suffix policy should be aligned with the organizational network topology and should be updated when new DNS namespaces are added to the environment. Consistent DNS suffix configuration prevents split-brain DNS issues where resources are accessible by different names depending on client DNS configuration. Organizations should test DNS suffix changes in a test environment before deploying globally to prevent disruption to name resolution for critical services.",
            Tags = ["ncsi", "dns-suffix", "name-resolution", "network-config", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableGlobalDnsSuffixPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableGlobalDnsSuffixPolicy")],
            DetectOps = [RegOp.CheckDword(Key, "EnableGlobalDnsSuffixPolicy", 1)],
        },
        new TweakDef
        {
            Id = "netcfg-restrict-network-location-awareness",
            Label = "Restrict Network Location Awareness Profile Assignment",
            Category = "Network Configuration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Network Location Awareness determines the network profile public private or domain assigned to each network connection which affects firewall rules and network sharing settings. Restricting network profile assignment prevents networks from being incorrectly classified as private or domain which could apply less restrictive firewall rules than appropriate for untrusted networks. Public networks receive more restrictive firewall configuration while domain and private networks receive more permissive configuration. Users operating on untrusted networks like hotel WiFi or coffee shop hotspots may be prompted to change the network profile which if set to private or domain exposes more services through the firewall. Policy-based network location awareness override ensures that specific networks are consistently classified regardless of user choices. Organizations should configure their corporate network identifiers to ensure that corporate networks are consistently classified as domain networks.",
            Tags = ["ncsi", "network-location-awareness", "firewall", "network-profiles", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictNetworkLocationAwareness", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictNetworkLocationAwareness")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictNetworkLocationAwareness", 1)],
        },
        new TweakDef
        {
            Id = "netcfg-disable-network-connectivity-popup",
            Label = "Disable Network Connectivity Status Popup Notifications",
            Category = "Network Configuration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Disabling network connectivity status popups suppresses the Windows notification dialogs that appear when network connectivity status changes such as connecting to a new network or losing internet access. Network connectivity popups in environments where users are not expected to make network configuration decisions can be disruptive to user workflows. In kiosk and restricted desktop environments the connectivity popups may expose information about the network infrastructure that should not be visible to users. Organizations should disable connectivity popups for environments where IT manages all network configuration and users should not be involved in network status decisions. The popup suppression does not prevent connectivity status from being reported through the system tray or through programmatic queries to the network connectivity API. Network administrators should still be notified of connectivity changes through monitoring tools even when user-facing popups are disabled.",
            Tags = ["ncsi", "popup-notifications", "network-status", "user-interface", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableConnectivityPopup", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableConnectivityPopup")],
            DetectOps = [RegOp.CheckDword(Key, "DisableConnectivityPopup", 1)],
        },
        new TweakDef
        {
            Id = "netcfg-block-captive-portal-redirect",
            Label = "Block Captive Portal Redirect Detection and Notification",
            Category = "Network Configuration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Blocking captive portal redirect detection prevents Windows from automatically presenting captive portal authentication prompts when connecting to networks that require web-based authentication login. Captive portal detection involves probing external URLs which reveals the presence of enrolled Windows devices to network operators and monitoring systems. In enterprise environments devices should not be connecting to networks that use captive portals and the prompt itself may confuse users or provide a social engineering attack vector. Disabling captive portal redirection prevents the NCSI from following HTTP redirects to captive portal authentication pages which could be used by malicious hotspot operators to redirect to phishing pages. Organizations that legitimately need guest WiFi access through captive portals for visitors should provide this access through separate devices rather than managed enterprise endpoints. Blocking captive portal redirect also prevents managed networks from being accidentally classified as having limited internet access.",
            Tags = ["ncsi", "captive-portal", "network-security", "hotspot", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockCaptivePortalRedirect", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockCaptivePortalRedirect")],
            DetectOps = [RegOp.CheckDword(Key, "BlockCaptivePortalRedirect", 1)],
        },
        new TweakDef
        {
            Id = "netcfg-enable-interface-metric-policy",
            Label = "Enable Policy-Based Interface Metric Configuration",
            Category = "Network Configuration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Policy-based interface metric configuration allows administrators to define the routing preference of network interfaces through group policy ensuring consistent traffic routing behavior across all managed systems. Interface metrics determine the order in which network interfaces are used for routing with lower metric values preferred for outbound traffic. Controlled interface metrics ensure that traffic goes through the appropriate network interface including security appliances like network DLP and inspection systems when multiple network paths are available. Inconsistent interface metrics across the fleet can cause security traffic to bypass inspection systems when lower-priority interfaces are used instead of the primary managed interface. Organizations should define interface metrics that route traffic through all required security inspection points before reaching external destinations. Interface metric policy should be tested for each network topology including VPN-connected remote workers who have both VPN and direct internet interfaces.",
            Tags = ["ncsi", "interface-metrics", "routing", "network-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableInterfaceMetricPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableInterfaceMetricPolicy")],
            DetectOps = [RegOp.CheckDword(Key, "EnableInterfaceMetricPolicy", 1)],
        },
        new TweakDef
        {
            Id = "netcfg-restrict-apipa-assignment",
            Label = "Restrict Automatic Private IP Address Assignment on Link Failure",
            Category = "Network Configuration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Restricting Automatic Private IP Addressing prevents Windows from assigning a 169.254.x.x address when DHCP is unavailable which reduces the risk of systems accidentally joining link-local network segments that bypass normal network routing. APIPA addresses allow systems with failed DHCP to communicate with other APIPA-addressed systems on the same network segment which can create unmonitored peer-to-peer communication channels. In environments with strict network segmentation APIPA assignment can allow systems to communicate on segments they should not be able to reach if DHCP failure places unexpected systems on the same broadcast domain. Disabling APIPA ensures that systems without a valid DHCP lease lose network connectivity rather than falling back to an uncontrolled addressing scheme. Organizations should monitor for DHCP failures that would cause system unavailability when APIPA is disabled and ensure DHCP server high availability matches system uptime requirements. DHCP failure alerting should be configured to ensure that the absence of APIPA fallback does not result in extended system unavailability.",
            Tags = ["ncsi", "apipa", "dhcp", "network-addressing", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAPIPAAssignment", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAPIPAAssignment")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAPIPAAssignment", 1)],
        },
        new TweakDef
        {
            Id = "netcfg-block-wi-fi-sense-auto-connect",
            Label = "Block Wi-Fi Sense Automatic Connection to Shared Networks",
            Category = "Network Configuration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Wi-Fi Sense automatic connection sharing exposes corporate network credentials to contact networks by sharing WiFi credentials through Microsoft account contact sharing which extends beyond organizational control. Enterprise devices should not have their stored WiFi credentials shared with Microsoft contact networks as this distributes corporate network access credentials to uncontrolled parties. Blocking Wi-Fi Sense prevents automatic connection to networks that contacts have shared which can include untrusted consumer WiFi networks that could expose device traffic to unauthorized parties. On corporate networks Wi-Fi Sense could allow external parties who receive shared network credentials to connect to corporate guest networks or networks that should be restricted to managed devices. Organizations should disable Wi-Fi Sense on all managed enterprise devices to prevent inadvertent credential sharing and unexpected connections to external networks. WiFi network credential management in enterprise environments should be handled through 802.1X certificate-based authentication rather than shared pre-shared key credentials.",
            Tags = ["ncsi", "wi-fi-sense", "credential-sharing", "wireless-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockWiFiSenseAutoConnect", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockWiFiSenseAutoConnect")],
            DetectOps = [RegOp.CheckDword(Key, "BlockWiFiSenseAutoConnect", 1)],
        },
        new TweakDef
        {
            Id = "netcfg-enforce-secure-dns-configuration",
            Label = "Enforce Secure DNS Configuration for Network Name Resolution",
            Category = "Network Configuration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Enforcing secure DNS configuration ensures that DNS queries are sent only to organizationally managed DNS servers that provide DNS security extensions and protective filtering. Bypassing organizational DNS servers is a common technique for circumventing DNS-based security controls including malware domain blocking threat intelligence feeds and data loss prevention. Secure DNS configuration policy locks the DNS server configuration to approved organizational servers preventing users and applications from switching to alternative DNS providers. Organizations that have deployed DNS security features like RPZ zones threat feeds and DoH-based filtering rely on all clients using the configured DNS servers. The policy should be combined with network firewall rules that block DNS queries to non-approved DNS servers providing defense-in-depth against DNS bypass attempts. Regular auditing of DNS query patterns for queries to non-organizational DNS servers helps detect attempts to bypass DNS security controls.",
            Tags = ["ncsi", "dns-security", "dns-servers", "name-resolution", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceSecureDnsConfiguration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceSecureDnsConfiguration")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceSecureDnsConfiguration", 1)],
        },
    ];
}
