namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static partial class PolicyNetworkExt
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _NetworkAdapterPolicy.Data,
            .. _NetworkBridgePolicy.Data,
            .. _NetworkConnectionsPolicy.Data,
            .. _NetworkConnectStatusPolicy.Data,
            .. _NetworkDiagnosticsPolicy.Data,
            .. _NetworkDiscovery.Data,
            .. _NetworkHardenedPaths.Data,
            .. _NetworkInterface.Data,
            .. _NetworkListPolicy.Data,
            .. _NetworkLltdPolicy.Data,
            .. _NetworkLocationAwarenessPolicy.Data,
            .. _NetworkMonitoringPolicy.Data,
            .. _NetworkProfilePolicy.Data,
            .. _NetworkProjectionPolicy.Data,
            .. _NetworkQosPolicy.Data,
            .. _NfcPolicy.Data,
            .. _NicTeamingPolicy.Data,
            .. _NtpGpoPolicy.Data,
            .. _ProxyBypassPolicy.Data,
            .. _RadiusAuthPolicy.Data,
            .. _RemoteNetworkAccessPolicy.Data,
            .. _SharedFoldersSmbPolicy.Data,
            .. _SmbEncryptionPolicy.Data,
            .. _SmbNetworking.Data,
            .. _SmbServerHardeningPolicy.Data,
            .. _SmbServerPolicy.Data,
            .. _SnmpPolicy.Data,
            .. _SshHardening.Data,
            .. _VoipQualityPolicy.Data,
            .. _VpnRemoteAccessPolicy.Data,
            .. _WcmConnectionPolicy.Data,
            .. _WcmWifiPolicy.Data,
            .. _WebProxyAutoDiscoveryPolicy.Data,
            .. _WifiConnectionPolicy.Data,
            .. _WifiNetworking.Data,
            .. _WinHttpProxyPolicy.Data,
            .. _WinInetPolicy.Data,
            .. _WinsNameResolutionPolicy.Data,
            .. _WirelessDisplayPolicy.Data,
            .. _WlanPolicy.Data,
            .. _WsdPrintDiscoveryPolicy.Data,
        ];

    private static class _NetworkAdapterPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkAdapterConfiguration";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "netadp-disable-netbios-over-tcp",
                Label = "Disable NetBIOS Over TCP/IP",
                Category = "Network — Network Adapter",
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
                Category = "Network — Network Adapter",
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
                Category = "Network — Network Adapter",
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
                Category = "Network — Network Adapter",
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
                Category = "Network — Network Adapter",
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
                Category = "Network — Network Adapter",
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
                Category = "Network — Network Adapter",
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
                Category = "Network — Network Adapter",
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
                Category = "Network — Network Adapter",
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
                Category = "Network — Network Adapter",
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

    // ── NetworkBridgePolicy ──
    private static class _NetworkBridgePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "netbridge-prohibit-personal-hotspot",
                    Label = "Prohibit Windows Mobile Hotspot (Personal Hotspot)",
                    Category = "Network — Network Adapter",
                    Description =
                        "Disables the Windows Mobile Hotspot feature that allows the machine to share its internet connection via Wi-Fi, preventing uncontrolled wireless network egress via Personal Hotspot in enterprise environments.",
                    Tags = ["mobile-hotspot", "wi-fi", "sharing", "corporate-network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Mobile Hotspot prohibited; device cannot create a personal Wi-Fi hotspot from its WAN connection.",
                    ApplyOps = [RegOp.SetDword(Key, "NC_ShowSharedAccessHotspot", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NC_ShowSharedAccessHotspot")],
                    DetectOps = [RegOp.CheckDword(Key, "NC_ShowSharedAccessHotspot", 0)],
                },
                new TweakDef
                {
                    Id = "netbridge-disable-advanced-settings",
                    Label = "Block Standard Users from Accessing Network Advanced Settings",
                    Category = "Network — Network Adapter",
                    Description =
                        "Prevents non-administrator users from accessing the Advanced Settings of network connections that control adapter binding order, providers, and services, protecting network stack configuration from unauthorised changes.",
                    Tags = ["network-connections", "advanced-settings", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Network Advanced Settings blocked for standard users; binding order and network services protected.",
                    ApplyOps = [RegOp.SetDword(Key, "NC_AdvancedSettings", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NC_AdvancedSettings")],
                    DetectOps = [RegOp.CheckDword(Key, "NC_AdvancedSettings", 0)],
                },
                new TweakDef
                {
                    Id = "netbridge-disable-vpn-connect-disconnect",
                    Label = "Block Standard Users from Connecting or Disconnecting VPN Connections",
                    Category = "Network — Network Adapter",
                    Description =
                        "Prevents standard users from connecting to or disconnecting from VPN connections, ensuring that VPN access is always administrator-controlled and cannot be bypassed or circumvented by non-privileged users.",
                    Tags = ["vpn", "network-connections", "standard-user", "corporate-policy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "VPN connection and disconnection blocked for standard users; admin must manage VPN sessions.",
                    ApplyOps = [RegOp.SetDword(Key, "NC_LanConnect", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NC_LanConnect")],
                    DetectOps = [RegOp.CheckDword(Key, "NC_LanConnect", 0)],
                },
                new TweakDef
                {
                    Id = "netbridge-notify-on-connection-change",
                    Label = "Show Notification on Network Connection Status Change",
                    Category = "Network — Network Adapter",
                    Description =
                        "Enables system notifications to administrators when a network connection is connected, disconnected, or changed, providing real-time awareness of network topology changes on managed endpoints.",
                    Tags = ["network-connections", "notifications", "admin", "monitoring", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Network connection change notifications enabled; admins notified on connect/disconnect events.",
                    ApplyOps = [RegOp.SetDword(Key, "NC_StatsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NC_StatsEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "NC_StatsEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "netbridge-disable-network-map-topology",
                    Label = "Disable Automatic Network Topology Discovery Sharing",
                    Category = "Network — Network Adapter",
                    Description =
                        "Prevents Windows from sharing network topology information (responding to LLTD/WSD discovery requests) that reveals this machine's network connections and link layer topology to other network hosts.",
                    Tags = ["network-discovery", "topology", "lltd", "wsd", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Network topology discovery sharing disabled; machine does not reveal its network connections to discovery.",
                    ApplyOps = [RegOp.SetDword(Key, "NC_AllowTopologyDiscovery", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NC_AllowTopologyDiscovery")],
                    DetectOps = [RegOp.CheckDword(Key, "NC_AllowTopologyDiscovery", 0)],
                },
            ];
    }

    // ── NetworkConnectionsPolicy ──
    private static class _NetworkConnectionsPolicy
    {
        private const string Pol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "netconn-enable-admin-prohibits",
                Label = "Honour Admin-Prohibited Network Connection Actions",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["network", "connections", "policy", "admin", "security"],
                Description =
                    "Sets NC_EnableAdminProhibits=1 which activates all administrator prohibition policies "
                    + "in the Network Connections folder. This MUST be enabled before other NC_ restrictions "
                    + "take effect. Without it, all other netconn policies are silently ignored.",
                ApplyOps = [RegOp.SetDword(Pol, "NC_EnableAdminProhibits", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NC_EnableAdminProhibits")],
                DetectOps = [RegOp.CheckDword(Pol, "NC_EnableAdminProhibits", 1)],
            },
            new TweakDef
            {
                Id = "netconn-prevent-change-binding",
                Label = "Prevent Changing Network Component Binding Order",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["network", "binding", "policy", "security", "components"],
                Description =
                    "Prevents standard users from enabling, disabling, or reordering network protocol "
                    + "bindings (NC_ChangeBindingState=0). Stops users from disabling security protocol "
                    + "bindings like SMB signing or 802.1X authentication on network adapters.",
                ApplyOps = [RegOp.SetDword(Pol, "NC_ChangeBindingState", 0)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NC_ChangeBindingState")],
                DetectOps = [RegOp.CheckDword(Pol, "NC_ChangeBindingState", 0)],
            },
            new TweakDef
            {
                Id = "netconn-prevent-delete-all-user",
                Label = "Prevent Deleting All-User VPN/Dial-Up Connections",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["network", "vpn", "dialup", "connections", "policy"],
                Description =
                    "Prevents standard users from deleting network connections that are available to all "
                    + "users (NC_DeleteAllUserConnection=0). Protects corporate VPN profiles and dial-up "
                    + "connections created by IT from accidental or malicious removal.",
                ApplyOps = [RegOp.SetDword(Pol, "NC_DeleteAllUserConnection", 0)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NC_DeleteAllUserConnection")],
                DetectOps = [RegOp.CheckDword(Pol, "NC_DeleteAllUserConnection", 0)],
            },
            new TweakDef
            {
                Id = "netconn-prevent-ras-connect",
                Label = "Prevent Standard Users Connecting VPN/Dial-Up",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["network", "vpn", "dialup", "connect", "policy"],
                Description =
                    "Prevents standard users from connecting to dial-up or VPN connections (NC_RasConnect=0). "
                    + "Useful where VPN access should be controlled by IT policy, preventing users from "
                    + "connecting to unauthorised or personal VPN endpoints.",
                ApplyOps = [RegOp.SetDword(Pol, "NC_RasConnect", 0)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NC_RasConnect")],
                DetectOps = [RegOp.CheckDword(Pol, "NC_RasConnect", 0)],
            },
            new TweakDef
            {
                Id = "netconn-prevent-rename-connections",
                Label = "Prevent Users from Renaming Network Connections",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["network", "rename", "connections", "policy", "hardening"],
                Description =
                    "Prevents standard users from renaming network connections via the Network Connections "
                    + "folder (NC_RenameConnection=0). Stops confusion and social engineering attempts that "
                    + "rely on renaming connections to impersonate corporate VPN profiles or legitimate adapters.",
                ApplyOps = [RegOp.SetDword(Pol, "NC_RenameConnection", 0)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NC_RenameConnection")],
                DetectOps = [RegOp.CheckDword(Pol, "NC_RenameConnection", 0)],
            },
        ];
    }

    // ── NetworkConnectStatusPolicy ──
    private static class _NetworkConnectStatusPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectStatusIndicator";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ncsi-disable-active-probing",
                    Label = "Disable NCSI Active Probing (Privacy)",
                    Category = "Network — Network Adapter",
                    Description =
                        "Prevents Windows from sending HTTP probes to www.msftconnecttest.com to determine internet connectivity. Eliminates Microsoft telemetry connections from the network stack. Default: probing enabled. Recommended: 1 for privacy-focused environments.",
                    Tags = ["ncsi", "probing", "privacy", "telemetry", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Stop&Go: the system tray network icon may show 'No Internet' even with connectivity; captive portal detection disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "NoActiveProbe", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoActiveProbe")],
                    DetectOps = [RegOp.CheckDword(Key, "NoActiveProbe", 1)],
                },
                new TweakDef
                {
                    Id = "ncsi-disable-global-dns-probe",
                    Label = "Disable NCSI Global DNS Probe",
                    Category = "Network — Network Adapter",
                    Description =
                        "Suppresses the DNS lookup probe to dns.msftncsi.com that NCSI uses to verify connectivity. Reduces DNS traffic to Microsoft servers. Default: probe enabled. Recommended: 1 for hardened/air-gapped environments.",
                    Tags = ["ncsi", "dns", "probe", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "DNS probe to Microsoft suppressed; may cause 'No Internet' indication in system tray on valid connections.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePassivePolling", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePassivePolling")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePassivePolling", 1)],
                },
                new TweakDef
                {
                    Id = "ncsi-disable-captive-portal-detection",
                    Label = "Disable Captive Portal Browser Launch",
                    Category = "Network — Network Adapter",
                    Description =
                        "Prevents Windows from automatically launching a browser window when a captive portal (hotel/airport Wi-Fi) is detected. Reduces unsolicited network connections and popup browsing windows. Default: enabled. Recommended: 1 for corporate laptops.",
                    Tags = ["ncsi", "captive-portal", "browser", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Captive portal browser popup is suppressed; users must manually open a browser to log into hotel/airport Wi-Fi.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCaptivePortalDetection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCaptivePortalDetection")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCaptivePortalDetection", 1)],
                },
                new TweakDef
                {
                    Id = "ncsi-use-corporate-probe-host",
                    Label = "Use Corporate Custom Probe Host",
                    Category = "Network — Network Adapter",
                    Description =
                        "Enables NCSI to probe an internal corporate server instead of Microsoft's public endpoint. Allows NCSI to correctly report connectivity status on air-gapped or corporate networks. Default: not set. Recommended: 1 (then configure custom host separately).",
                    Tags = ["ncsi", "corporate", "intranet", "probe", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NCSI will use the custom probe host instead of Microsoft; set the probe host/path via companion registry values.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableCorporateProbe", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableCorporateProbe")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableCorporateProbe", 1)],
                },
                new TweakDef
                {
                    Id = "ncsi-disable-ipv6-probe",
                    Label = "Disable NCSI IPv6 Probe",
                    Category = "Network — Network Adapter",
                    Description =
                        "Prevents the NCSI IPv6 connectivity probe to ipv6.msftconnecttest.com. Reduces unsolicited outbound IPv6 traffic to Microsoft. Default: probe enabled. Recommended: 1 if IPv6 is not in use.",
                    Tags = ["ncsi", "ipv6", "probe", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "IPv6 NCSI probe suppressed; IPv6 connectivity indicator in system tray may be inaccurate.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableIPv6Probe", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableIPv6Probe")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableIPv6Probe", 1)],
                },
                new TweakDef
                {
                    Id = "ncsi-disable-internet-access-check",
                    Label = "Disable System-Wide Internet Access Check",
                    Category = "Network — Network Adapter",
                    Description =
                        "Suppresses the periodic system-level NCSI check that determines whether the machine has internet access. Useful on dedicated intranet-only systems. Default: check enabled. Recommended: 1 for air-gapped environments.",
                    Tags = ["ncsi", "internet-check", "intranet", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "No internet access check—apps relying on NCSI connectivity state may behave incorrectly.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableInternetAccessCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableInternetAccessCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableInternetAccessCheck", 1)],
                },
                new TweakDef
                {
                    Id = "ncsi-hide-network-icon-status",
                    Label = "Hide NCSI Status in System Tray Tooltip",
                    Category = "Network — Network Adapter",
                    Description =
                        "Removes the 'No Internet Access' or 'No connectivity' tooltip from the network system tray icon. Reduces user confusion on corporate networks that filter NCSI. Default: shown. Recommended: 1 on managed networks.",
                    Tags = ["ncsi", "tray", "notification", "network", "usability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Network tray icon tooltip no longer shows 'No Internet Access' on filtered corporate networks.",
                    ApplyOps = [RegOp.SetDword(Key, "HideNoInternetWarning", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "HideNoInternetWarning")],
                    DetectOps = [RegOp.CheckDword(Key, "HideNoInternetWarning", 1)],
                },
                new TweakDef
                {
                    Id = "ncsi-require-corporate-connectivity",
                    Label = "Require Corporate Network for NCSI 'Connected' Status",
                    Category = "Network — Network Adapter",
                    Description =
                        "Configures NCSI to only report 'Connected to Internet' when the device can also reach the corporate intranet probe host. Ensures the network indicator reflects both intranet and internet connectivity. Default: not configured.",
                    Tags = ["ncsi", "corporate", "connectivity", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "NCSI shows 'connected' only when both internet and corporate network are reachable.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireCorporateConnectivity", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireCorporateConnectivity")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireCorporateConnectivity", 1)],
                },
                new TweakDef
                {
                    Id = "ncsi-probe-retry-3",
                    Label = "Set NCSI Probe Retry Count to 3",
                    Category = "Network — Network Adapter",
                    Description =
                        "Limits the number of NCSI probe retries to 3 before declaring no connectivity. Reduces network congestion from repeated probing on slow or lossy links. Default: 5. Recommended: 3.",
                    Tags = ["ncsi", "probe", "retry", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "NCSI retries probes 3 times before showing 'No Internet'; faster failure detection on broken links.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxProbeRetryCount", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxProbeRetryCount")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxProbeRetryCount", 3)],
                },
                new TweakDef
                {
                    Id = "ncsi-log-probe-failures",
                    Label = "Enable NCSI Probe Failure Event Logging",
                    Category = "Network — Network Adapter",
                    Description =
                        "Enables audit logging of NCSI probe failures to the Windows Event Log. Useful for diagnosing connectivity issues on managed endpoints. Default: disabled. Recommended: 1 for monitored environments.",
                    Tags = ["ncsi", "logging", "audit", "network", "diagnostics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "NCSI probe failures are written to the Event Log; minor I/O overhead.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableProbeFailureLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableProbeFailureLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableProbeFailureLogging", 1)],
                },
            ];
    }

    // ── NetworkDiagnosticsPolicy ──
    private static class _NetworkDiagnosticsPolicy
    {
        private const string NetDiagKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkDiagnostics";
        private const string WdiWireless = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{EBC068D3-BD0A-4B60-9078-6B952B7B04D1}";
        private const string WdiNetConn = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{A7A5847A-7511-4E4E-90B1-45AD2A002F51}";
        private const string WdiNetPerf = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{4DC08CD6-E593-4B38-9ABC-9C25B15571C1}";
        private const string WdiNetCfg = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{C76A4930-2379-4C5F-B2B3-F671FDDF73E2}";
        private const string ScriptDiag = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ndiag-disable-helper-engine",
                Label = "Disable Network Diagnostics Helper Engine",
                Category = "Network — Network Adapter",
                Description =
                    "Sets DisableHelperEngine=1 in the NetworkDiagnostics policy key. "
                    + "Turns off the Windows Network Diagnostics helper engine entirely, preventing automated diagnosis "
                    + "of network connectivity issues and the 'Diagnose this problem' link in error dialogs. "
                    + "Default: absent (engine active). Recommended: 1 when users must escalate to IT for network issues.",
                Tags = ["network", "diagnostics", "policy", "restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes the 'Windows Network Diagnostics' auto-repair flow; network connectivity itself is unaffected.",
                ApplyOps = [RegOp.SetDword(NetDiagKey, "DisableHelperEngine", 1)],
                RemoveOps = [RegOp.DeleteValue(NetDiagKey, "DisableHelperEngine")],
                DetectOps = [RegOp.CheckDword(NetDiagKey, "DisableHelperEngine", 1)],
            },
            new TweakDef
            {
                Id = "ndiag-disable-wireless-scenario",
                Label = "Disable WDI Wireless Diagnostics Scenario",
                Category = "Network — Network Adapter",
                Description =
                    "Sets ScenarioExecutionEnabled=0 for the WDI Wireless Diagnostics scenario (GUID EBC068D3). "
                    + "Prevents the Windows Diagnostics Infrastructure from automatically running wireless troubleshooting "
                    + "steps when WLAN connectivity issues are detected. "
                    + "Default: absent (scenario active). Recommended: 0 in managed environments where WLAN is centrally controlled.",
                Tags = ["wireless", "wdi", "diagnostics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables automated wireless diagnostics; WLAN connectivity itself is unaffected.",
                ApplyOps = [RegOp.SetDword(WdiWireless, "ScenarioExecutionEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(WdiWireless, "ScenarioExecutionEnabled")],
                DetectOps = [RegOp.CheckDword(WdiWireless, "ScenarioExecutionEnabled", 0)],
            },
            new TweakDef
            {
                Id = "ndiag-disable-netconn-scenario",
                Label = "Disable WDI Network Connectivity Diagnostics Scenario",
                Category = "Network — Network Adapter",
                Description =
                    "Sets ScenarioExecutionEnabled=0 for the WDI Network Connectivity scenario (GUID A7A5847A). "
                    + "Prevents Windows from automatically running network connectivity troubleshooting steps. "
                    + "Default: absent (scenario active). Recommended: 0 on tightly managed networks.",
                Tags = ["network", "wdi", "diagnostics", "connectivity", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses automatic network connectivity diagnosis; manual 'Troubleshoot' actions still work.",
                ApplyOps = [RegOp.SetDword(WdiNetConn, "ScenarioExecutionEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(WdiNetConn, "ScenarioExecutionEnabled")],
                DetectOps = [RegOp.CheckDword(WdiNetConn, "ScenarioExecutionEnabled", 0)],
            },
            new TweakDef
            {
                Id = "ndiag-disable-netperf-scenario",
                Label = "Disable WDI Network Performance Monitoring Scenario",
                Category = "Network — Network Adapter",
                Description =
                    "Sets ScenarioExecutionEnabled=0 for the WDI Network Performance Monitoring scenario (GUID 4DC08CD6). "
                    + "Turns off the background network performance data collection component of the Windows Diagnostics Infrastructure. "
                    + "Default: absent (monitoring active). Recommended: 0 to reduce background network telemetry on managed systems.",
                Tags = ["network", "wdi", "performance", "monitoring", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables background WDI network performance monitoring; reduces diagnostic data collection.",
                ApplyOps = [RegOp.SetDword(WdiNetPerf, "ScenarioExecutionEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(WdiNetPerf, "ScenarioExecutionEnabled")],
                DetectOps = [RegOp.CheckDword(WdiNetPerf, "ScenarioExecutionEnabled", 0)],
            },
            new TweakDef
            {
                Id = "ndiag-disable-netcfg-scenario",
                Label = "Disable WDI Network Configuration Diagnostics Scenario",
                Category = "Network — Network Adapter",
                Description =
                    "Sets ScenarioExecutionEnabled=0 for the WDI Network Configuration scenario (GUID C76A4930). "
                    + "Prevents Windows from automatically running diagnostics when network adapter configuration issues are detected. "
                    + "Default: absent (scenario active). Recommended: 0 when NIC configuration is locked by policy.",
                Tags = ["network", "wdi", "configuration", "diagnostics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables the WDI network configuration diagnostic scenario; adapter configuration is unaffected.",
                ApplyOps = [RegOp.SetDword(WdiNetCfg, "ScenarioExecutionEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(WdiNetCfg, "ScenarioExecutionEnabled")],
                DetectOps = [RegOp.CheckDword(WdiNetCfg, "ScenarioExecutionEnabled", 0)],
            },
            new TweakDef
            {
                Id = "ndiag-validate-diag-helpers",
                Label = "Require Validation of Diagnostic Helper Modules",
                Category = "Network — Network Adapter",
                Description =
                    "Sets ValidateHelpers=1 in the ScriptedDiagnostics policy key. "
                    + "Requires that all diagnostic helper modules (*.dll) loaded by the scripted diagnostics engine be "
                    + "digitally signed and validated before execution, preventing unsigned diagnostic extensions. "
                    + "Default: absent (helpers not validated). Recommended: 1 for security on managed endpoints.",
                Tags = ["diagnostics", "scripted", "validation", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Enforces digital signature validation for all diagnostic helper DLLs; unsigned helpers are blocked.",
                ApplyOps = [RegOp.SetDword(ScriptDiag, "ValidateHelpers", 1)],
                RemoveOps = [RegOp.DeleteValue(ScriptDiag, "ValidateHelpers")],
                DetectOps = [RegOp.CheckDword(ScriptDiag, "ValidateHelpers", 1)],
            },
            new TweakDef
            {
                Id = "ndiag-no-remote-server-query",
                Label = "Block Scripted Diagnostics Remote Server Queries",
                Category = "Network — Network Adapter",
                Description =
                    "Sets DisableQueryRemoteServer=1 in the ScriptedDiagnostics policy key. "
                    + "Prevents the Windows Scripted Diagnostics service from querying Microsoft online servers "
                    + "for additional troubleshooting content or updated diagnostic packs. "
                    + "Default: absent (remote queries allowed). Recommended: 1 on air-gapped or privacy-sensitive environments.",
                Tags = ["diagnostics", "scripted", "privacy", "remote", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks diagnostic helper queries to Microsoft servers; local troubleshooters continue to work.",
                ApplyOps = [RegOp.SetDword(ScriptDiag, "DisableQueryRemoteServer", 1)],
                RemoveOps = [RegOp.DeleteValue(ScriptDiag, "DisableQueryRemoteServer")],
                DetectOps = [RegOp.CheckDword(ScriptDiag, "DisableQueryRemoteServer", 1)],
            },
            new TweakDef
            {
                Id = "ndiag-restrict-wireless-execution-level",
                Label = "Set WDI Wireless Diagnostics to View-Only",
                Category = "Network — Network Adapter",
                Description =
                    "Sets EnabledScenarioExecutionLevel=1 for the WDI Wireless Diagnostics scenario. "
                    + "Allows Windows to gather wireless diagnostic information and present results to the user, "
                    + "but prevents automatic repair actions from being taken without user confirmation. "
                    + "Default: absent (automatic repair). Recommended: 1 where users may view diagnostics but not auto-fix.",
                Tags = ["wireless", "wdi", "diagnostics", "restricted", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WDI wireless diagnostics shows results but does not auto-apply network fixes.",
                ApplyOps = [RegOp.SetDword(WdiWireless, "EnabledScenarioExecutionLevel", 1)],
                RemoveOps = [RegOp.DeleteValue(WdiWireless, "EnabledScenarioExecutionLevel")],
                DetectOps = [RegOp.CheckDword(WdiWireless, "EnabledScenarioExecutionLevel", 1)],
            },
            new TweakDef
            {
                Id = "ndiag-restrict-netconn-execution-level",
                Label = "Set WDI Network Connectivity Diagnostics to View-Only",
                Category = "Network — Network Adapter",
                Description =
                    "Sets EnabledScenarioExecutionLevel=1 for the WDI Network Connectivity scenario. "
                    + "Allows diagnosis of network problems but restricts the engine from automatically applying fixes. "
                    + "Default: absent (automatic repair). Recommended: 1 on networks where configuration changes require IT approval.",
                Tags = ["network", "wdi", "diagnostics", "restricted", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WDI network connectivity diagnostics shows results but does not auto-apply connection fixes.",
                ApplyOps = [RegOp.SetDword(WdiNetConn, "EnabledScenarioExecutionLevel", 1)],
                RemoveOps = [RegOp.DeleteValue(WdiNetConn, "EnabledScenarioExecutionLevel")],
                DetectOps = [RegOp.CheckDword(WdiNetConn, "EnabledScenarioExecutionLevel", 1)],
            },
        ];
    }

    // ── NetworkDiscovery ──
    private static class _NetworkDiscovery
    {
        private const string Lltd = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LLTD";
        private const string Dns = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters";
        private const string NetBt = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } = [];
    }

    // ── NetworkHardenedPaths ──
    private static class _NetworkHardenedPaths
    {
        private const string HardenedPaths = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProvider\HardenedPaths";
        private const string WebClient = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "nethpth-harden-sysvol",
                Label = "Network: Require Mutual Auth + Integrity for SYSVOL Shares",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [HardenedPaths],
                Tags = ["network", "unc", "sysvol", "smb", "hardening", "mitm"],
                Description =
                    @"Sets \\*\SYSVOL = ""RequireMutualAuthentication=1, RequireIntegrity=1"" in UNC Hardened Paths. "
                    + "Prevents SYSVOL share access over unauthenticated or integrity-unprotected channels, "
                    + "blocking pass-the-hash and relay attacks on domain share access. Default: not hardened.",
                ApplyOps = [RegOp.SetString(HardenedPaths, @"\\*\SYSVOL", "RequireMutualAuthentication=1, RequireIntegrity=1")],
                RemoveOps = [RegOp.DeleteValue(HardenedPaths, @"\\*\SYSVOL")],
                DetectOps = [RegOp.CheckString(HardenedPaths, @"\\*\SYSVOL", "RequireMutualAuthentication=1, RequireIntegrity=1")],
            },
            new TweakDef
            {
                Id = "nethpth-harden-netlogon",
                Label = "Network: Require Mutual Auth + Integrity for NETLOGON Shares",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [HardenedPaths],
                Tags = ["network", "unc", "netlogon", "smb", "hardening", "mitm"],
                Description =
                    @"Sets \\*\NETLOGON = ""RequireMutualAuthentication=1, RequireIntegrity=1"". "
                    + "Ensures NETLOGON share authentication against MITM attacks and relay attacks. "
                    + "Critical for domain-joined machines. Default: not hardened.",
                ApplyOps = [RegOp.SetString(HardenedPaths, @"\\*\NETLOGON", "RequireMutualAuthentication=1, RequireIntegrity=1")],
                RemoveOps = [RegOp.DeleteValue(HardenedPaths, @"\\*\NETLOGON")],
                DetectOps = [RegOp.CheckString(HardenedPaths, @"\\*\NETLOGON", "RequireMutualAuthentication=1, RequireIntegrity=1")],
            },
            new TweakDef
            {
                Id = "nethpth-harden-admin-shares",
                Label = "Network: Require Authentication for All Admin UNC Shares",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [HardenedPaths],
                Tags = ["network", "unc", "admin-share", "hardening", "lateral-movement"],
                Description =
                    @"Sets \\*\* (wildcard) = ""RequireAuthentication=1"" in UNC Hardened Paths. "
                    + "Requires SMB authentication on all UNC paths system-wide. "
                    + "Broad protection against unauthenticated lateral movement. Default: not enforced.",
                ApplyOps = [RegOp.SetString(HardenedPaths, @"\\*\*", "RequireAuthentication=1")],
                RemoveOps = [RegOp.DeleteValue(HardenedPaths, @"\\*\*")],
                DetectOps = [RegOp.CheckString(HardenedPaths, @"\\*\*", "RequireAuthentication=1")],
            },
            new TweakDef
            {
                Id = "nethpth-harden-ipc-integrity",
                Label = "Network: Require Integrity for IPC$ Shares",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [HardenedPaths],
                Tags = ["network", "unc", "ipc", "integrity", "smb-signing", "hardening"],
                Description =
                    @"Sets \\*\IPC$ = ""RequireMutualAuthentication=1, RequireIntegrity=1"" in UNC Hardened Paths. "
                    + "Full mutual authentication plus packet integrity (SMB signing) on IPC$ connections. "
                    + "Strongest IPC$ hardening. Supersedes nethpth-harden-ipc-share.",
                ApplyOps = [RegOp.SetString(HardenedPaths, @"\\*\IPC$", "RequireMutualAuthentication=1, RequireIntegrity=1")],
                RemoveOps = [RegOp.DeleteValue(HardenedPaths, @"\\*\IPC$")],
                DetectOps = [RegOp.CheckString(HardenedPaths, @"\\*\IPC$", "RequireMutualAuthentication=1, RequireIntegrity=1")],
            },
            new TweakDef
            {
                Id = "nethpth-disable-webdav-basic-auth",
                Label = "Network: Disable WebDAV Client Basic Authentication",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = false,
                RegistryKeys = [WebClient],
                Tags = ["webdav", "webclient", "basic-auth", "network", "security"],
                Description =
                    "Sets BasicAuthLevel=0 in WebClient service parameters. Prevents the WebDAV client from "
                    + "sending credentials as plaintext in HTTP Basic Authentication headers. "
                    + "Default: BasicAuthLevel=1 (disabled over HTTP, allowed over HTTPS). Set 0 to block entirely.",
                ApplyOps = [RegOp.SetDword(WebClient, "BasicAuthLevel", 0)],
                RemoveOps = [RegOp.DeleteValue(WebClient, "BasicAuthLevel")],
                DetectOps = [RegOp.CheckDword(WebClient, "BasicAuthLevel", 0)],
            },
            new TweakDef
            {
                Id = "nethpth-limit-webdav-file-size",
                Label = "Network: Cap WebDAV File Transfer Size at 50 MB",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = false,
                RegistryKeys = [WebClient],
                Tags = ["webdav", "webclient", "file-size", "network", "data-loss"],
                Description =
                    "Sets FileSizeLimitInBytes=52428800 (50 MB) in WebClient parameters. Limits the maximum file "
                    + "size the WebDAV client can upload or download in a single operation. "
                    + "Default: 47 MB (Windows default). Reduces risk of bulk data exfiltration via WebDAV.",
                ApplyOps = [RegOp.SetDword(WebClient, "FileSizeLimitInBytes", 52428800)],
                RemoveOps = [RegOp.DeleteValue(WebClient, "FileSizeLimitInBytes")],
                DetectOps = [RegOp.CheckDword(WebClient, "FileSizeLimitInBytes", 52428800)],
            },
            new TweakDef
            {
                Id = "nethpth-webdav-connection-timeout",
                Label = "Network: Reduce WebDAV Connection Timeout to 60 Seconds",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = false,
                RegistryKeys = [WebClient],
                Tags = ["webdav", "webclient", "timeout", "network", "performance"],
                Description =
                    "Sets ConnectionTimeout=60000 ms in WebClient parameters. Reduces blocking time when "
                    + "WebDAV connections to unavailable servers are attempted. "
                    + "Default: 60 000 ms. Lower values improve responsiveness when remote shares are offline.",
                ApplyOps = [RegOp.SetDword(WebClient, "ConnectionTimeout", 60000)],
                RemoveOps = [RegOp.DeleteValue(WebClient, "ConnectionTimeout")],
                DetectOps = [RegOp.CheckDword(WebClient, "ConnectionTimeout", 60000)],
            },
            new TweakDef
            {
                Id = "nethpth-webdav-send-timeout",
                Label = "Network: Reduce WebDAV Send Timeout to 30 Seconds",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = false,
                RegistryKeys = [WebClient],
                Tags = ["webdav", "webclient", "timeout", "network", "performance"],
                Description =
                    "Sets SendTimeout=30000 ms in WebClient parameters. Cuts time the client blocks waiting "
                    + "for a WebDAV server to confirm reception of a request. "
                    + "Default: 30 000 ms. Helps detect stalled upload sessions faster.",
                ApplyOps = [RegOp.SetDword(WebClient, "SendTimeout", 30000)],
                RemoveOps = [RegOp.DeleteValue(WebClient, "SendTimeout")],
                DetectOps = [RegOp.CheckDword(WebClient, "SendTimeout", 30000)],
            },
            new TweakDef
            {
                Id = "nethpth-webdav-receive-timeout",
                Label = "Network: Reduce WebDAV Receive Timeout to 60 Seconds",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = false,
                RegistryKeys = [WebClient],
                Tags = ["webdav", "webclient", "timeout", "network", "performance"],
                Description =
                    "Sets ReceiveTimeout=60000 ms in WebClient parameters. Limits how long the client waits "
                    + "for a server response after sending a WebDAV request. "
                    + "Default: 60 000 ms. Conservative value for typical enterprise network latency.",
                ApplyOps = [RegOp.SetDword(WebClient, "ReceiveTimeout", 60000)],
                RemoveOps = [RegOp.DeleteValue(WebClient, "ReceiveTimeout")],
                DetectOps = [RegOp.CheckDword(WebClient, "ReceiveTimeout", 60000)],
            },
        ];
    }

    // ── NetworkInterface ──
    private static class _NetworkInterface
    {
        private const string TcpIp = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";

        private const string TcpIpPerf = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces";

        private const string AfD = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters";

        private const string LanmanRedirector = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "nic-disable-tcp-chimney-offload",
                Label = "Disable TCP Chimney Offload",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["nic", "tcp", "chimney", "offload", "nic offload"],
                Description =
                    "Disables TCP chimney offload, which in practice causes CPU elevation "
                    + "issues on some NIC firmware. Keeps TCP processing in the Windows "
                    + "network stack where it is more predictable.",
                ApplyOps = [RegOp.SetDword(TcpIp, "EnableTCPChimney", 0)],
                RemoveOps = [RegOp.DeleteValue(TcpIp, "EnableTCPChimney")],
                DetectOps = [RegOp.CheckDword(TcpIp, "EnableTCPChimney", 0)],
            },
            new TweakDef
            {
                Id = "nic-enable-ctcp",
                Label = "Enable Compound TCP (CTCP) Congestion Control",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["nic", "ctcp", "congestion control", "tcp", "throughput"],
                Description =
                    "Enables Compound TCP congestion control algorithm, which increases the "
                    + "receive window more aggressively on high-bandwidth, high-latency links "
                    + "(e.g. intercontinental). Comparable to Linux's Cubic. 1 = enabled.",
                ApplyOps = [RegOp.SetDword(TcpIp, "EnableCTCP", 1)],
                RemoveOps = [RegOp.DeleteValue(TcpIp, "EnableCTCP")],
                DetectOps = [RegOp.CheckDword(TcpIp, "EnableCTCP", 1)],
            },
        ];
    }

    // ── NetworkListPolicy ──
    private static class _NetworkListPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkList";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "netlst-delete-all-user-files-on-exit",
                Label = "Delete Network Profile Files on Exit",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Network profile files contain connection history and metadata about previously connected networks stored on the device. Deleting these files on exit prevents accumulation of network history that could reveal location patterns and network infrastructure details. This policy is particularly relevant for mobile devices that connect to diverse networks during travel. Removing connection history on exit limits the information available to an attacker who gains physical or logical access to the device. Enterprise security policies often require that connection history be purged to prevent network topology exposure. Standard network connectivity remains fully functional as new connection profiles are created as needed.",
                Tags = ["network-list", "privacy", "cleanup", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DeleteAllUserFilesOnExit", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DeleteAllUserFilesOnExit")],
                DetectOps = [RegOp.CheckDword(Key, "DeleteAllUserFilesOnExit", 1)],
            },
            new TweakDef
            {
                Id = "netlst-disable-safety-ui",
                Label = "Disable Network Safety UI",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The network safety UI presents dialogs warning users about connecting to public or unsecured networks and requesting network location choices. Disabling this UI removes the location type selection dialog that appears when connecting to a new network. Enterprise networks are classified centrally through domain membership and network profile policies, making the interactive UI redundant. User-initiated network classification can result in incorrect security zone assignments that override intended enterprise policy. Removing the UI prevents users from inadvertently classifying enterprise networks as public or home networks. Network classification is managed deterministically through network location awareness policies and domain detection.",
                Tags = ["network-list", "ui", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNetworkSafetyUI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkSafetyUI")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNetworkSafetyUI", 1)],
            },
            new TweakDef
            {
                Id = "netlst-disable-connected-standby",
                Label = "Disable Connected Standby Network Mode",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Connected standby maintains network connectivity management during device sleep states, allowing the network list to update and applications to receive push notifications. Disabling connected standby in the network list policy prevents background network classification updates during sleep. This reduces battery consumption on laptop devices that connect to multiple networks across different work locations. Enterprise devices with corporate VPN requirements often do not need continuous network awareness while sleeping. Disabling this feature eliminates the power drain associated with maintaining the network enumeration stack during sleep. Standard network operations resume immediately upon device wake without any impact on connectivity.",
                Tags = ["network-list", "standby", "power", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableConnectedStandbyMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableConnectedStandbyMode")],
                DetectOps = [RegOp.CheckDword(Key, "DisableConnectedStandbyMode", 1)],
            },
            new TweakDef
            {
                Id = "netlst-disable-connection-assistant",
                Label = "Disable Network Connection Assistant",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The network connection assistant guides users through troubleshooting and configuring network connections when issues are detected. Disabling the connection assistant prevents users from using the wizard-based interface to modify network adapter configurations. Enterprise networking configurations are managed by network administrators and should not be modified by end users through interactive wizards. Allowing users to run the connection assistant can result in incorrect network configuration changes that require helpdesk intervention. Disabling this feature enforces network configuration control without removing administrators' ability to manage settings directly. Corporate network issues should be resolved through the helpdesk rather than user-initiated configuration changes.",
                Tags = ["network-list", "assistant", "configuration", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableConnectionAssistant", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableConnectionAssistant")],
                DetectOps = [RegOp.CheckDword(Key, "DisableConnectionAssistant", 1)],
            },
            new TweakDef
            {
                Id = "netlst-allow-network-icon",
                Label = "Allow Network Icon in System Tray",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The network icon in the system tray provides users with quick access to network status and connection management. Allowing the network icon to appear at its default value ensures users can easily identify connectivity issues and connection state. This policy sets DisableNetworkIcon to zero, confirming the network icon remains visible in the system tray. Removing the network icon impedes user ability to identify connectivity issues, leading to increased helpdesk calls. Enterprise environments benefit from users having basic network status visibility to self-diagnose simple connectivity problems. The network icon represents a balance between user access and administrative control over network configuration.",
                Tags = ["network-list", "ui", "system-tray", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNetworkIcon", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkIcon")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNetworkIcon", 0)],
            },
            new TweakDef
            {
                Id = "netlst-disable-telemetry",
                Label = "Disable Network List Telemetry",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Network list telemetry transmits information about discovered networks, connection events, and network profile changes to Microsoft. This data includes network identifiers such as SSIDs and BSSID information that can reveal location information. Disabling network list telemetry prevents network topology and location data from being transmitted outside the enterprise. Regulated industries require that network infrastructure details are not disclosed through telemetry channels. The network list continues to function identically regardless of telemetry status. Administrators requiring network connection analytics should implement dedicated network monitoring solutions.",
                Tags = ["network-list", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNetworkListTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkListTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNetworkListTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "netlst-disable-manual-roaming",
                Label = "Disable Manual Network Roaming",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "Manual roaming allows users to select which wireless network the device connects to when multiple networks with the same SSID are available in different locations. Disabling manual roaming prevents users from overriding the automatic network selection algorithm with manual access point selections. Enterprise wireless networks use 802.11r and RSSI-based roaming algorithms that should not be overridden by user selection. User-initiated manual AP selection can cause persistent connections to distant access points with poor signal, degrading performance. Disabling this feature ensures the enterprise wireless client uses optimal access points based on signal strength and policy. Centrally managed wireless infrastructure delivers better performance outcomes than user-directed manual selection.",
                Tags = ["network-list", "roaming", "wireless", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowManualRoaming", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowManualRoaming")],
                DetectOps = [RegOp.CheckDword(Key, "AllowManualRoaming", 0)],
            },
            new TweakDef
            {
                Id = "netlst-disable-social-network",
                Label = "Disable Social Network Integration",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Social network integration in the network list manager allows Windows to display social network status and share network information with connected social services. Disabling social network integration prevents the operating system from communicating with social network APIs through network list event callbacks. Enterprise workstations should not have social network integration active as it represents an uncontrolled data channel. Social network APIs can receive information about device connectivity state and network location through this integration. Disabling this feature eliminates a potential data exfiltration path through social network API calls. Standard network connectivity is completely unaffected by disabling social network integration.",
                Tags = ["network-list", "social-network", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSocialNetworkIntegration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSocialNetworkIntegration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSocialNetworkIntegration", 1)],
            },
            new TweakDef
            {
                Id = "netlst-disable-network-mapping",
                Label = "Disable Network Mapping",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Network mapping enumerates devices on the local network and displays them in the Network section of Windows Explorer using LLTD and network discovery protocols. Disabling network mapping prevents the device from actively probing the network for neighboring devices. Reducing device enumeration broadcasts lowers the network's attack surface by preventing device discovery by unauthenticated network participants. Enterprise devices on segmented corporate networks should not broadcast their presence to other network segments via network mapping. The LLTD protocol used for network mapping can reveal device types and capabilities to network observers. Disabling network mapping is recommended for all production enterprise endpoints as a defense-in-depth measure.",
                Tags = ["network-list", "discovery", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNetworkMapping", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkMapping")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNetworkMapping", 1)],
            },
            new TweakDef
            {
                Id = "netlst-disable-category-change",
                Label = "Disable Network Category Change",
                Category = "Network — Network Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Windows allows users to change the network location category between Private, Public, and Domain profiles through the network settings UI. Disabling network category change prevents end users from modifying the assigned network location type for any connected network. Enterprise network profiles should be assigned and locked by Group Policy based on domain membership and network identity detection. User-modified network categories can override firewall profiles and compliance policies leading to unintended security exposure. Incorrect network category assignments are a common cause of unexpected firewall behavior affecting enterprise applications. Locking network categories through policy ensures consistent firewall and security profile application across all endpoints.",
                Tags = ["network-list", "firewall", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNetworkCategoryChange", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkCategoryChange")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNetworkCategoryChange", 1)],
            },
        ];
    }

    // ── NetworkLltdPolicy ──
    private static class _NetworkLltdPolicy
    {
        private const string Lltd = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LLTD";
        private const string PeerNet = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Peernet";
        private const string PeerToPeer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerToPeer";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "netlltd-prohibit-lltdio-private",
                Label = "LLTD: Prohibit LLTD I/O driver on private networks",
                Category = "Network — Network Adapter",
                Description =
                    "Sets ProhibitLLTDIOOnPrivateNet=1. Prevents the LLTD Mapper I/O driver from operating "
                    + "on private network profiles, reducing topology exposure on home networks.",
                Tags = ["network", "lltd", "private", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Lltd, "ProhibitLLTDIOOnPrivateNet", 1)],
                RemoveOps = [RegOp.DeleteValue(Lltd, "ProhibitLLTDIOOnPrivateNet")],
                DetectOps = [RegOp.CheckDword(Lltd, "ProhibitLLTDIOOnPrivateNet", 1)],
            },
            new TweakDef
            {
                Id = "netlltd-prohibit-rspndr-private",
                Label = "LLTD: Prohibit LLTD Responder on private networks",
                Category = "Network — Network Adapter",
                Description =
                    "Sets ProhibitRspndrOnPrivateNet=1. Stops the LLTD Responder from operating on "
                    + "private network profiles, hiding this machine from home network topology maps.",
                Tags = ["network", "lltd", "responder", "private", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Lltd, "ProhibitRspndrOnPrivateNet", 1)],
                RemoveOps = [RegOp.DeleteValue(Lltd, "ProhibitRspndrOnPrivateNet")],
                DetectOps = [RegOp.CheckDword(Lltd, "ProhibitRspndrOnPrivateNet", 1)],
            },
            new TweakDef
            {
                Id = "netlltd-disable-peernet",
                Label = "Disable Windows People Near Me (Peernet) service",
                Category = "Network — Network Adapter",
                Description =
                    "Sets Disabled=1 in the Peernet policy key. Disables the People Near Me network "
                    + "service that discovers nearby contacts over the local network using Windows Collaboration.",
                Tags = ["network", "peernet", "people-near-me", "policy", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(PeerNet, "Disabled", 1)],
                RemoveOps = [RegOp.DeleteValue(PeerNet, "Disabled")],
                DetectOps = [RegOp.CheckDword(PeerNet, "Disabled", 1)],
            },
            new TweakDef
            {
                Id = "netlltd-disable-pnrp",
                Label = "Disable Peer Name Resolution Protocol (PNRP)",
                Category = "Network — Network Adapter",
                Description =
                    "Sets Disabled=1 in the PeerToPeer policy key. Disables PNRP, the peer-to-peer name "
                    + "resolution protocol used for Windows Meeting Space and legacy collaboration features.",
                Tags = ["network", "p2p", "pnrp", "policy", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(PeerToPeer, "Disabled", 1)],
                RemoveOps = [RegOp.DeleteValue(PeerToPeer, "Disabled")],
                DetectOps = [RegOp.CheckDword(PeerToPeer, "Disabled", 1)],
            },
        ];
    }

    // ── NetworkLocationAwarenessPolicy ──
    private static class _NetworkLocationAwarenessPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "nlapol-disable-ms-connectivity-test",
                    Label = "Disable Microsoft Connectivity Test",
                    Category = "Network — Network Adapter",
                    Description =
                        "Disables the Microsoft connectivity test that sends HTTP probes to msftconnecttest.com to determine internet reachability.",
                    Tags = ["ncsi", "connectivity", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Eliminates periodic HTTP calls to Microsoft; connectivity icon may be inaccurate.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMicrosoftConnectivityTest", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMicrosoftConnectivityTest")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMicrosoftConnectivityTest", 1)],
                },
                new TweakDef
                {
                    Id = "nlapol-disable-internet-connectivity-check",
                    Label = "Disable Internet Connectivity Checks",
                    Category = "Network — Network Adapter",
                    Description =
                        "Disables periodic internet connectivity checks performed by the Network Location Awareness service that can leak network topology information.",
                    Tags = ["ncsi", "internet", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Stops internet-check probes; may affect network-dependent features relying on NLA status.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableInternetConnectivityCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableInternetConnectivityCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableInternetConnectivityCheck", 1)],
                },
                new TweakDef
                {
                    Id = "nlapol-enable-corporate-dns-probe",
                    Label = "Enable Corporate DNS Probe for Network Detection",
                    Category = "Network — Network Adapter",
                    Description =
                        "Enables a corporate DNS probe to accurately detect when the machine is on the corporate network instead of relying on Microsoft cloud probes.",
                    Tags = ["ncsi", "dns", "corporate", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Improves corporate network detection accuracy using internal DNS.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableCorporateDNSProbe", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableCorporateDNSProbe")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableCorporateDNSProbe", 1)],
                },
                new TweakDef
                {
                    Id = "nlapol-disable-hotspot-detection",
                    Label = "Disable Wi-Fi Hotspot Detection",
                    Category = "Network — Network Adapter",
                    Description =
                        "Disables automatic hotspot (captive portal) detection that sends HTTP probes to detect whether a login portal intercepts connections.",
                    Tags = ["ncsi", "hotspot", "wifi", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Stops hotspot probe traffic; captive portal detection disabled on public Wi-Fi.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableHotspotDetection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableHotspotDetection")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableHotspotDetection", 1)],
                },
                new TweakDef
                {
                    Id = "nlapol-enable-nca",
                    Label = "Enable Network Connectivity Assistant (NCA)",
                    Category = "Network — Network Adapter",
                    Description =
                        "Enables the Network Connectivity Assistant service that provides DirectAccess connectivity status information to users.",
                    Tags = ["ncsi", "nca", "directaccess", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NCA provides DirectAccess status UI; no privacy impact when probes are disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableNCA", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableNCA")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableNCA", 1)],
                },
                new TweakDef
                {
                    Id = "nlapol-disable-ipv6-check",
                    Label = "Disable IPv6 Connectivity Check",
                    Category = "Network — Network Adapter",
                    Description =
                        "Disables the IPv6 connectivity check performed by NCSI that contacts Microsoft's IPv6 probe server, reducing telemetry on dual-stack networks.",
                    Tags = ["ncsi", "ipv6", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Stops IPv6 probe; IPv6 connectivity still works but indicator may be inaccurate.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableIPv6ConnectivityCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableIPv6ConnectivityCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableIPv6ConnectivityCheck", 1)],
                },
                new TweakDef
                {
                    Id = "nlapol-enforce-domain-detection",
                    Label = "Enforce Domain Network Detection",
                    Category = "Network — Network Adapter",
                    Description =
                        "Enforces domain network detection policy, ensuring that NLA correctly identifies domain presence based on DNS/DC availability.",
                    Tags = ["ncsi", "domain", "detection", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Improves domain-trust classification; requires configured internal DNS.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceDomainNetworkDetection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceDomainNetworkDetection")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceDomainNetworkDetection", 1)],
                },
                new TweakDef
                {
                    Id = "nlapol-block-location-switching",
                    Label = "Block Location-Based Network Profile Switching",
                    Category = "Network — Network Adapter",
                    Description =
                        "Blocks automatic network profile switching triggered by location or SSID changes, preventing accidental profile downgrades from domain to public.",
                    Tags = ["ncsi", "location", "network-profile", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Prevents accidental public-profile downgrade on domain machines.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockLocationBasedNetworkSwitching", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockLocationBasedNetworkSwitching")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockLocationBasedNetworkSwitching", 1)],
                },
                new TweakDef
                {
                    Id = "nlapol-passive-poll-disable",
                    Label = "Disable NCSI Passive Network Polling",
                    Category = "Network — Network Adapter",
                    Description =
                        "Disables background passive polling by the Network Connectivity Status Indicator service, reducing unnecessary network probes and telemetry.",
                    Tags = ["ncsi", "polling", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Reduces background network chatter; connectivity indicator updates less frequently.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePassivePolling", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePassivePolling")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePassivePolling", 1)],
                },
            ];
    }

    // ── NetworkMonitoringPolicy ──
    private static class _NetworkMonitoringPolicy
    {
        private const string NdfKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkDiagnostics";

        private const string DiagKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Diagnostics";

        private const string WdiKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Diagnostics\Networking";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "netmon-disable-ndf-online-repair",
                    Label = "Network Monitoring: Disable Network Diagnostic Online Auto-Repair",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Sets DontDisplayNetworkSelectionUI=1 in Network Diagnostics policy. Prevents the Windows Network Diagnostics Framework (NDF) from automatically connecting to Microsoft's online diagnostics service to retrieve updated diagnostic helpers and repair scripts. In enterprise environments, connectivity to external Microsoft diagnostic endpoints should be controlled centrally (via proxy allow-list), not triggered automatically by user-initiated troubleshooting dialogs. Online repair also leaks network configuration details to Microsoft.",
                    Tags = ["netmon", "ndf", "diagnostics", "online", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "NDF uses only local diagnostic helpers (no online repair retrieval). Administrators should ensure local NDF helpers are kept up-to-date via Windows Update.",
                    ApplyOps = [RegOp.SetDword(NdfKey, "DontDisplayNetworkSelectionUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(NdfKey, "DontDisplayNetworkSelectionUI")],
                    DetectOps = [RegOp.CheckDword(NdfKey, "DontDisplayNetworkSelectionUI", 1)],
                },
                new TweakDef
                {
                    Id = "netmon-enable-network-event-logging",
                    Label = "Network Monitoring: Enable Verbose Network Event Logging",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Sets NetworkEventLogging=1 in Network Diagnostics policy. Enables verbose network event logging which writes detailed network adapter state changes, DHCP lease events, IP address changes, and connectivity state transitions to the Windows event log (Source: Microsoft-Windows-NetworkProfile, Microsoft-Windows-NCSI). This log data is essential for correlating network problems with application failures and for SIEM-based network anomaly detection (unusual IP changes, frequent adapter resets).",
                    Tags = ["netmon", "event-log", "network", "dhcp", "diagnostics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Generates additional network event log entries. Ensure event log sizing is sufficient. Events are forwarded via WEF/WEC to SIEM for enterprise monitoring.",
                    ApplyOps = [RegOp.SetDword(NdfKey, "NetworkEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(NdfKey, "NetworkEventLogging")],
                    DetectOps = [RegOp.CheckDword(NdfKey, "NetworkEventLogging", 1)],
                },
                new TweakDef
                {
                    Id = "netmon-enable-ndis-trace",
                    Label = "Network Monitoring: Enable NDIS Driver Trace Collection",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Sets EnableNdisTrace=1 in Diagnostics policy. Enables Network Driver Interface Specification (NDIS) trace logging which captures driver-level packet events, miniport state changes, and power management events for network adapters. NDIS traces provide the lowest-level view of network adapter behavior, including driver errors and power state transitions that cause intermittent connectivity. These traces are collected by Windows Diagnostic Infrastructure (WDI) and submitted when network diagnostic scans are run.",
                    Tags = ["netmon", "ndis", "trace", "network-driver", "diagnostics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "NDIS trace logging is lightweight (ring buffer). Trace data is only written to disk when a diagnostic scan is triggered. No continuous disk I/O from this setting.",
                    ApplyOps = [RegOp.SetDword(DiagKey, "EnableNdisTrace", 1)],
                    RemoveOps = [RegOp.DeleteValue(DiagKey, "EnableNdisTrace")],
                    DetectOps = [RegOp.CheckDword(DiagKey, "EnableNdisTrace", 1)],
                },
                new TweakDef
                {
                    Id = "netmon-disable-autoplay-on-network",
                    Label = "Network Monitoring: Disable AutoPlay for Network-Mapped Drives",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Sets NoAutoPlayOnNetworkShares=1 in Network Monitoring/Diagnostics policy. Prevents Windows AutoPlay from executing autorun.inf on network-mapped drives. Network drive AutoPlay was the primary propagation vector for network worms that planted autorun.inf files on open shares. Even though AutoPlay on user machines may be disabled by other policies, explicitly blocking AutoPlay on network shares ensures that malicious autorun.inf placed on a file server by an attacker cannot trigger automatic execution on connecting clients.",
                    Tags = ["netmon", "autoplay", "network-share", "worm", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "AutoPlay behavior on USB and optical discs is controlled separately. This policy only affects network shares.",
                    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoPlayOnNetworkShares", 1)],
                    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoPlayOnNetworkShares")],
                    DetectOps =
                    [
                        RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoPlayOnNetworkShares", 1),
                    ],
                },
                new TweakDef
                {
                    Id = "netmon-enable-connectivity-probe",
                    Label = "Network Monitoring: Enable Corporate Connectivity Probe",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Sets EnableConnectivityProbing=1 in Network Diagnostics. Configures Windows to continuously probe an internal IT-managed connectivity check endpoint (corporate NCSI probe server) to track network connectivity quality. Connectivity probe failures generate event log events that allow SIEM and IT monitoring tools to detect network infrastructure failures, DNS outages, and proxy unavailability in real time across the managed endpoint fleet before users report issues.",
                    Tags = ["netmon", "probe", "connectivity", "siem", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires a corporate NCSI probe server to be configured (typically an internal web server). Generates HTTP probes every 30 seconds per adapter.",
                    ApplyOps = [RegOp.SetDword(NdfKey, "EnableConnectivityProbing", 1)],
                    RemoveOps = [RegOp.DeleteValue(NdfKey, "EnableConnectivityProbing")],
                    DetectOps = [RegOp.CheckDword(NdfKey, "EnableConnectivityProbing", 1)],
                },
                new TweakDef
                {
                    Id = "netmon-enable-pktmon",
                    Label = "Network Monitoring: Enable Packet Monitor (PktMon) Trace",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Sets PktMonEnabled=1 in Diagnostics policy. Enables access to the Windows Packet Monitor (pktmon) built-in network sniffer for diagnostic purposes. pktmon is a kernel-mode packet capture built into Windows Server 2019 and Windows 10 1809+. This policy enables the capture component for use by network administrators running 'pktmon start' diagnostics. Without this policy, pktmon requires administrator elevation which is already implied; this setting enables the functionality for diagnostic scripts.",
                    Tags = ["netmon", "pktmon", "packet-capture", "diagnostics", "admin"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enables pktmon diagnostic capture access. No continuous packet capture occurs; captures are initiated manually or by diagnostic scripts.",
                    ApplyOps = [RegOp.SetDword(DiagKey, "PktMonEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(DiagKey, "PktMonEnabled")],
                    DetectOps = [RegOp.CheckDword(DiagKey, "PktMonEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "netmon-enable-wdi-net-diagnostics",
                    Label = "Network Monitoring: Enable WDI Network Diagnostics Service",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Sets WdiNetDiagEnabled=1 in WDI Diagnostics settings. Enables the Windows Diagnostic Infrastructure (WDI) network diagnostics scenario which collects lightweight ambient network performance data when connectivity problems occur. WDI triggers automatic trace collection when network degradation is detected (packet loss >5%, latency spikes, DNS resolution delays) and saves diagnostic logs to %SystemRoot%\\diagnostics. These logs are critical for helpdesk troubleshooting remote endpoint network issues.",
                    Tags = ["netmon", "wdi", "diagnostics", "trace", "helpdesk"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "WDI diagnostics collect lightweight ambient traces. Trace collection is triggered by degradation events, not continuously. Trace files are local and require helpdesk access to collect.",
                    ApplyOps = [RegOp.SetDword(WdiKey, "WdiNetDiagEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdiKey, "WdiNetDiagEnabled")],
                    DetectOps = [RegOp.CheckDword(WdiKey, "WdiNetDiagEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "netmon-enable-smb-connection-audit",
                    Label = "Network Monitoring: Enable SMB Access Audit Logging",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Sets AuditSmb=1 in Network Monitoring policy. Enables auditing of SMB (Server Message Block) file access events, generating Windows Security event log entries for file share connections, access attempts, and share mount/unmount. SMB access audit is a core requirement for detecting lateral movement: attackers using pass-the-hash, pass-the-ticket, or network share enumeration tools (Impacket, CrackMapExec) generate distinctive SMB access patterns that appear in audit logs.",
                    Tags = ["netmon", "smb", "audit", "logging", "lateral-movement"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SMB access audit generates Security event log entries for every file share connection. Ensure event log capacity and SIEM egress are sized for the additional volume on file servers.",
                    ApplyOps = [RegOp.SetDword(NdfKey, "AuditSmb", 1)],
                    RemoveOps = [RegOp.DeleteValue(NdfKey, "AuditSmb")],
                    DetectOps = [RegOp.CheckDword(NdfKey, "AuditSmb", 1)],
                },
                new TweakDef
                {
                    Id = "netmon-set-connection-limit-per-host",
                    Label = "Network Monitoring: Limit Simultaneous Connections Per Server",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Sets WinHttpConnectionLimit=10 in WinHTTP settings. Limits the number of simultaneous HTTP connections per server to 10. The default limit of 64 per server allows aggressive web scrapers, update agents, and backup agents to open hundreds of simultaneous connections to a single server, potentially degrading server performance. A limit of 10 concurrent connections per client-server pair is sufficient for modern application workloads and prevents per-machine connection flooding.",
                    Tags = ["netmon", "connection-limit", "http", "performance", "server"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Simultaneous connections per destination are capped at 10. High-volume download agents (BITS, WSUS) use their own connection limits and may be unaffected.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings",
                            "MaxConnectionsPerServer",
                            10
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings",
                            "MaxConnectionsPerServer"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings",
                            "MaxConnectionsPerServer",
                            10
                        ),
                    ],
                },
            ];
    }

    // ── NetworkProfilePolicy ──
    private static class _NetworkProfilePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivity";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "netprof-block-auto-profile-change",
                Label = "Block Automatic Network Location Profile Changes",
                Category = "Network — Network Monitoring",
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
                Category = "Network — Network Monitoring",
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
                Category = "Network — Network Monitoring",
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
                Category = "Network — Network Monitoring",
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
                Category = "Network — Network Monitoring",
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
                Category = "Network — Network Monitoring",
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
                Category = "Network — Network Monitoring",
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
                Category = "Network — Network Monitoring",
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
                Category = "Network — Network Monitoring",
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
                Category = "Network — Network Monitoring",
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

    // ── NetworkProjectionPolicy ──
    private static class _NetworkProjectionPolicy
    {
        private const string ProjKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProjector";
        private const string ConnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect";
        private const string WdisKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WirelessDisplay";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "netproj-disable-network-projector",
                Label = "Disable Legacy Network Projector Connection",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets NoNetworkProjector=1 in the NetworkProjector policy key. "
                    + "Prevents users from connecting this machine to a legacy network projector via "
                    + "the 'Connect to a Network Projector' wizard (Windows 7/8 era feature). "
                    + "Network projectors exposed over LAN can be a lateral movement vector if reachable "
                    + "from an untrusted network segment. Disabling this closes the outbound connecting path. "
                    + "Default: absent (connection allowed). Recommended: 1 on endpoints without shared projectors.",
                Tags = ["network-projection", "projector", "wireless-display", "lan", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Legacy 'Connect to a Network Projector' wizard disabled.",
                ApplyOps = [RegOp.SetDword(ProjKey, "NoNetworkProjector", 1)],
                RemoveOps = [RegOp.DeleteValue(ProjKey, "NoNetworkProjector")],
                DetectOps = [RegOp.CheckDword(ProjKey, "NoNetworkProjector", 1)],
            },
            new TweakDef
            {
                Id = "netproj-require-pin-for-projection",
                Label = "Require PIN for 'Project to This PC'",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets RequirePinForPairing=2 in the Connect policy key (2=always require PIN). "
                    + "Requires a unique pairing PIN to be entered on the projecting device before it can "
                    + "establish a wireless display connection to this PC. Without a PIN requirement, "
                    + "any device on the same Wi-Fi can connect instantly without user consent. "
                    + "Values: 0=never, 1=first-time only, 2=always. "
                    + "Default: absent. Recommended: 2 in shared/open-office environments.",
                Tags = ["network-projection", "miracast", "pin", "pairing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "PIN required every time before a device can project wirelessly to this PC.",
                ApplyOps = [RegOp.SetDword(ConnKey, "RequirePinForPairing", 2)],
                RemoveOps = [RegOp.DeleteValue(ConnKey, "RequirePinForPairing")],
                DetectOps = [RegOp.CheckDword(ConnKey, "RequirePinForPairing", 2)],
            },
            new TweakDef
            {
                Id = "netproj-restrict-projection-to-secured-networks",
                Label = "Restrict Projection to Secured (Non-Open) Wi-Fi Networks",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets AllowProjectionToSecuredPCOnly=1 in the Connect policy key. "
                    + "Limits 'Project to This PC' to accept incoming Miracast connections only when the "
                    + "machine is connected to a password-protected (WPA/WPA2/WPA3) Wi-Fi network. "
                    + "Prevents accidental projection exposure when the machine is on an open conference "
                    + "room or hotel Wi-Fi where any third party could project content. "
                    + "Default: absent. Recommended: 1 for roaming/mobile employees.",
                Tags = ["network-projection", "miracast", "wifi-security", "wpa", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Wireless projection only accepted when on a password-protected Wi-Fi network.",
                ApplyOps = [RegOp.SetDword(ConnKey, "AllowProjectionToSecuredPCOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(ConnKey, "AllowProjectionToSecuredPCOnly")],
                DetectOps = [RegOp.CheckDword(ConnKey, "AllowProjectionToSecuredPCOnly", 1)],
            },
            new TweakDef
            {
                Id = "netproj-block-source-projection",
                Label = "Block This PC From Projecting to Other Devices",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets AllowProjectionFromPC=0 in the Connect policy key. "
                    + "Prevents the user from using 'Connect' or the Project button to send this PC's "
                    + "display to a Miracast dongle, smart TV, or wireless display adapter. "
                    + "While the risk is lower than the receive path, projecting to untrusted displays "
                    + "in a public setting can expose screen content. "
                    + "Default: absent (projection from PC allowed). Recommended: 0 in kiosk/locked environments.",
                Tags = ["network-projection", "miracast", "source", "wireless-display", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "This PC cannot project its display to other wireless display devices.",
                ApplyOps = [RegOp.SetDword(ConnKey, "AllowProjectionFromPC", 0)],
                RemoveOps = [RegOp.DeleteValue(ConnKey, "AllowProjectionFromPC")],
                DetectOps = [RegOp.CheckDword(ConnKey, "AllowProjectionFromPC", 0)],
            },
            new TweakDef
            {
                Id = "netproj-disable-wireless-display-infrastructure",
                Label = "Disable Wireless Display Infrastructure Mode",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets AllowWirelessDisplayInfrastructure=0 in the WirelessDisplay policy key. "
                    + "Disables the infrastructure-mode Miracast projection that routes wireless display "
                    + "traffic over a Wi-Fi router rather than a direct Wi-Fi Direct peer-to-peer link. "
                    + "Infrastructure-mode Miracast uses the corporate Wi-Fi, potentially traversing "
                    + "network segments and adding latency. Restricting to Wi-Fi Direct reduces the "
                    + "attack surface of projection traffic. Default: absent (infrastructure allowed).",
                Tags = ["network-projection", "miracast", "infrastructure", "wifi-direct", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Infrastructure-mode Miracast disabled; only Wi-Fi Direct peer-to-peer projection allowed.",
                ApplyOps = [RegOp.SetDword(WdisKey, "AllowWirelessDisplayInfrastructure", 0)],
                RemoveOps = [RegOp.DeleteValue(WdisKey, "AllowWirelessDisplayInfrastructure")],
                DetectOps = [RegOp.CheckDword(WdisKey, "AllowWirelessDisplayInfrastructure", 0)],
            },
            new TweakDef
            {
                Id = "netproj-disable-miracast-discovery-mcast",
                Label = "Disable Miracast Multicast Discovery Broadcast",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets DisableDeviceDiscovery=1 in the WirelessDisplay policy key. "
                    + "Prevents this machine from continuously broadcasting Miracast advertisement "
                    + "packets that announce its 'Project to This PC' capability on the local network. "
                    + "Passive Miracast discovery broadcasting can reveal machine presence and name "
                    + "on the network even when the machine would otherwise be silent. "
                    + "Default: absent (discovery broadcasts on). Recommended: 1 for stealth network posture.",
                Tags = ["network-projection", "miracast", "discovery", "multicast", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Miracast device presence broadcasts stopped; machine not discoverable via wireless display scanning.",
                ApplyOps = [RegOp.SetDword(WdisKey, "DisableDeviceDiscovery", 1)],
                RemoveOps = [RegOp.DeleteValue(WdisKey, "DisableDeviceDiscovery")],
                DetectOps = [RegOp.CheckDword(WdisKey, "DisableDeviceDiscovery", 1)],
            },
            new TweakDef
            {
                Id = "netproj-enforce-hdcp-for-wireless-display",
                Label = "Enforce HDCP Content Protection on Wireless Display",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets AllowProjectionToHDCP=1 in the WirelessDisplay policy key. "
                    + "Requires that the receiving wireless display device supports HDCP (High-bandwidth "
                    + "Digital Content Protection) before the PC will project to it. "
                    + "Prevents DRM-protected content (streaming video, presentations with ERM) from "
                    + "being cast to non-HDCP-compliant receiver dongles or TVs that might not encrypt "
                    + "the content link layer. "
                    + "Default: absent. Recommended: 1 on systems that display confidential or DRM content.",
                Tags = ["network-projection", "miracast", "hdcp", "drm", "content-protection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Wireless projection only to HDCP-capable receivers; non-compliant displays rejected.",
                ApplyOps = [RegOp.SetDword(WdisKey, "AllowProjectionToHDCP", 1)],
                RemoveOps = [RegOp.DeleteValue(WdisKey, "AllowProjectionToHDCP")],
                DetectOps = [RegOp.CheckDword(WdisKey, "AllowProjectionToHDCP", 1)],
            },
            new TweakDef
            {
                Id = "netproj-disable-projector-peer-trust",
                Label = "Disable Auto-Trust for Previously Projected Displays",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets AllowPreviouslyPairedDevice=0 in the Connect policy key. "
                    + "Prevents Windows from automatically accepting wireless display connections from "
                    + "devices that have previously been paired (trusted) with this PC. "
                    + "Previously paired devices can reconnect without PIN re-entry, which reduces friction "
                    + "but removes the consent step. If a paired device is stolen or compromised, "
                    + "this policy ensures it cannot silently reconnect. "
                    + "Default: absent (previous pairs trusted). Recommended: 0 in high-security environments.",
                Tags = ["network-projection", "miracast", "trust", "pairing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Previously paired Miracast devices not auto-trusted; PIN required every connection.",
                ApplyOps = [RegOp.SetDword(ConnKey, "AllowPreviouslyPairedDevice", 0)],
                RemoveOps = [RegOp.DeleteValue(ConnKey, "AllowPreviouslyPairedDevice")],
                DetectOps = [RegOp.CheckDword(ConnKey, "AllowPreviouslyPairedDevice", 0)],
            },
            new TweakDef
            {
                Id = "netproj-set-projection-screenlock-timeout",
                Label = "Set Wireless Display Auto-Lock Screen After Idle",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets ProjectionIdleTimeout=5 in the Connect policy key. "
                    + "Sets the number of minutes of idle time on a 'Project to This PC' session before "
                    + "the received display is automatically locked or disconnected. Without a timeout, "
                    + "a projecting device's session persists indefinitely even after the user walks away, "
                    + "allowing anyone at the receiving display to view projected content. "
                    + "Value in minutes; 5 minutes aligns with standard workstation lock policies. "
                    + "Default: absent (no idle timeout). Recommended: 5 for shared display environments.",
                Tags = ["network-projection", "miracast", "idle", "timeout", "screen-lock", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Wireless display session locks after 5 minutes of idle; unattended projection disconnected.",
                ApplyOps = [RegOp.SetDword(ConnKey, "ProjectionIdleTimeout", 5)],
                RemoveOps = [RegOp.DeleteValue(ConnKey, "ProjectionIdleTimeout")],
                DetectOps = [RegOp.CheckDword(ConnKey, "ProjectionIdleTimeout", 5)],
            },
        ];
    }

    // ── NetworkQosPolicy ──
    private static class _NetworkQosPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "nqos-disable-reservation",
                Label = "Disable Network QoS Bandwidth Reservation",
                Category = "Network — Network Monitoring",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows QoS allows applications to reserve a portion of available network bandwidth for guaranteed delivery of real-time traffic like video and voice. The QoS bandwidth reservation mechanism historically allowed applications to reserve up to 20 percent of available link bandwidth. Disabling QoS bandwidth reservation ensures all available network bandwidth remains available for use by all applications equally. In networks with sufficient bandwidth this reservation provides no practical benefit and may limit throughput for bulk data transfers. Enterprise network QoS should be enforced at the network infrastructure level using DSCP markings rather than per-host reservations. Disabling host-side QoS reservation has no impact on network-enforced traffic prioritization.",
                Tags = ["network", "qos", "bandwidth", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBestEffortReservation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBestEffortReservation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBestEffortReservation", 1)],
            },
            new TweakDef
            {
                Id = "nqos-disable-dscp-marking",
                Label = "Disable DSCP Marking Override",
                Category = "Network — Network Monitoring",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "DSCP marking allows Windows to tag outbound network packets with Differentiated Services Code Point values for network infrastructure-based traffic prioritization. Windows can override application-requested DSCP values through Group Policy QoS rules. Disabling DSCP marking override prevents Windows from altering the DSCP values set by applications and network infrastructure devices. In properly configured enterprise networks, DSCP values should be set by trusted network devices rather than clienthosts. Host-based DSCP marking can conflict with network-enforced QoS policies deployed across the enterprise. Disabling this override ensures that network infrastructure QoS policies take precedence over host-side marking attempts.",
                Tags = ["network", "qos", "dscp", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDscpMarkingOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDscpMarkingOverride")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDscpMarkingOverride", 1)],
            },
            new TweakDef
            {
                Id = "nqos-disable-throttling",
                Label = "Disable Network QoS Throttling",
                Category = "Network — Network Monitoring",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Windows QoS throttling limits outbound network traffic rates for applications that have been classified under specific QoS policies. Disabling QoS throttling removes artificially imposed rate limits on network applications that would otherwise be constrained. QoS throttling can inadvertently limit backup software, large file transfers, and other high-throughput legitimate workloads. Enterprise environments relying on network-level traffic shaping should not also apply host-side throttling that could conflict with infrastructure QoS. Removing throttling on trusted enterprise endpoints ensures full link utilization for bulk transfer operations. Network-level QoS enforced by switching and routing infrastructure remains unaffected by this setting.",
                Tags = ["network", "qos", "throttling", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableThrottling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableThrottling")],
                DetectOps = [RegOp.CheckDword(Key, "DisableThrottling", 1)],
            },
            new TweakDef
            {
                Id = "nqos-disable-policy-application",
                Label = "Disable User-Level QoS Policy Application",
                Category = "Network — Network Monitoring",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows QoS policies can be applied at both the computer level and user level, with user-level policies applied when specific users log on. Disabling user-level QoS policy application prevents per-user QoS configurations from being applied and enforced. User-level QoS policies are less predictable in shared environments because they vary based on which user is logged on. Computer-level QoS policies applied by Group Policy at the machine scope are unaffected and continue to be enforced. Consistent network behavior in shared computing environments is better achieved through machine-level QoS policies. Disabling user-level policy application simplifies QoS administration by eliminating user-specific network configuration variability.",
                Tags = ["network", "qos", "users", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableUserPolicyApplication", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUserPolicyApplication")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUserPolicyApplication", 1)],
            },
            new TweakDef
            {
                Id = "nqos-disable-pacer",
                Label = "Disable Network Packet Scheduler Pacer",
                Category = "Network — Network Monitoring",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "The Windows packet scheduler pacer component enforces QoS policies by spacing out packet transmission to achieve desired traffic rates. Disabling the packet scheduler pacer removes the inter-packet scheduling enforcement for QoS flows. The pacer component adds CPU overhead and latency for every outbound network packet when QoS is in use. Enterprise endpoints not relying on Windows host-based QoS can benefit from removing this scheduling overhead. Removing the pacer reduces network stack latency and may improve throughput for applications sensitive to jitter and scheduling overhead. Network QoS enforcement at the infrastructure level through switch and router configurations is unaffected.",
                Tags = ["network", "qos", "pacer", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePacer", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePacer")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePacer", 1)],
            },
            new TweakDef
            {
                Id = "nqos-disable-app-marking",
                Label = "Prevent Applications from Overriding QoS Settings",
                Category = "Network — Network Monitoring",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Applications using QoS APIs can request specific DSCP markings and bandwidth priorities for their network traffic. Preventing applications from overriding QoS settings ensures that application-requested markings do not supersede Group Policy-defined QoS rules. Applications may abuse QoS APIs to mark all traffic with high priority values, degrading the effectiveness of traffic prioritization. Enterprise QoS policies should take strict precedence over application-level QoS requests to maintain consistent network behavior. Policy-locked QoS settings ensure the network infrastructure receives consistent DSCP markings regardless of application behavior. Critical real-time applications that require specific QoS treatment should be configured in Group Policy rather than self-assigning priorities.",
                Tags = ["network", "qos", "applications", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventApplicationQosOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventApplicationQosOverride")],
                DetectOps = [RegOp.CheckDword(Key, "PreventApplicationQosOverride", 1)],
            },
            new TweakDef
            {
                Id = "nqos-disable-telemetry",
                Label = "Disable Network QoS Telemetry",
                Category = "Network — Network Monitoring",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Network QoS telemetry collects and reports data about network quality of service events and policy application statistics. This telemetry helps Microsoft understand QoS usage patterns and improve network stack performance in future releases. Disabling QoS telemetry prevents network traffic classification and policy usage data from being reported externally. Network quality metrics represent sensitive operational data that may reveal enterprise network architecture details. Telemetry reporting from QoS components should be evaluated against data governance requirements before being permitted. All QoS policy enforcement and traffic prioritization functions continue to operate normally without telemetry.",
                Tags = ["network", "qos", "telemetry", "privacy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableQosTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableQosTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableQosTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "nqos-disable-adaptive",
                Label = "Disable Adaptive QoS",
                Category = "Network — Network Monitoring",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Adaptive QoS dynamically adjusts traffic classification and bandwidth allocation based on observed network conditions. Disabling adaptive QoS prevents automatic changes to traffic prioritization based on runtime network quality measurements. Adaptive QoS adjustments can cause unpredictable network behavior changes during production hours in enterprise environments. Static QoS configurations are preferred for enterprise deployments where consistent and auditable network behavior is required. Dynamic priority adjustments may interfere with time-sensitive applications relying on predictable network response characteristics. Disabling adaptive QoS ensures QoS policy settings remain stable and consistent with the Group Policy-defined configuration.",
                Tags = ["network", "qos", "adaptive", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAdaptiveQos", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAdaptiveQos")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAdaptiveQos", 1)],
            },
            new TweakDef
            {
                Id = "nqos-disable-conformance",
                Label = "Disable QoS Traffic Conformance",
                Category = "Network — Network Monitoring",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "QoS traffic conformance controls track whether applications are transmitting within their declared QoS profiles and can take action against non-conforming flows. Disabling traffic conformance removes enforcement of declared traffic profiles and allows flows to transmit outside their QoS specifications. Conformance enforcement adds complexity and potential false positives for legitimate burst traffic patterns in enterprise applications. Enterprise applications with variable traffic patterns may trigger conformance violations unnecessarily. Traffic shaping and rate enforcement should be handled at the network infrastructure level where more accurate visibility is available. Disabling conformance reduces QoS subsystem CPU overhead without affecting traffic marking behavior.",
                Tags = ["network", "qos", "conformance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTrafficConformance", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTrafficConformance")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTrafficConformance", 1)],
            },
            new TweakDef
            {
                Id = "nqos-disable-flow-inspection",
                Label = "Disable QoS Flow Inspection",
                Category = "Network — Network Monitoring",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "QoS flow inspection examines network traffic characteristics to classify flows and apply appropriate QoS policies. Disabling flow inspection prevents the QoS subsystem from performing deep packet analysis for traffic classification. Flow inspection adds CPU and memory overhead proportional to the volume of classified network flows. Enterprise environments that do not rely on QoS flow-based classification for policy application can safely disable this component. Static QoS policy rules based on application name or destination port are unaffected and continue to operate without flow inspection. Disabling flow inspection reduces network stack processing overhead, particularly on high-throughput workloads.",
                Tags = ["network", "qos", "inspection", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFlowInspection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFlowInspection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFlowInspection", 1)],
            },
        ];
    }

    // ── NfcPolicy ──
    private static class _NfcPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NFC";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "nfcpol-disable-nfc-radio",
                    Label = "Disable NFC Radio",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Disables the NFC radio on devices that have one. Prevents unauthorised tap-to-transfer, tap-to-pay, and proximity-based pairing without physical control removal. Default: NFC enabled if hardware present. Recommended: 1 on corporate laptops without approved NFC use cases.",
                    Tags = ["nfc", "radio", "wireless", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "NFC radio is turned off; tap-to-pay, tap-to-connect, and NFC tag reading are all disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowNFC", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowNFC")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowNFC", 0)],
                },
                new TweakDef
                {
                    Id = "nfcpol-disable-tap-to-pay",
                    Label = "Disable NFC Tap-to-Pay",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Prevents Windows Wallet and third-party payment apps from using NFC for contactless payments. Removes financial transaction risk from NFC proximity attacks. Default: tap-to-pay enabled if NFC hardware present. Recommended: 1 on managed devices.",
                    Tags = ["nfc", "payment", "tap-to-pay", "wallet", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "NFC payment is blocked; tap-to-pay via Samsung Pay, Microsoft Wallet, or other NFC payment apps is disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowPayment", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowPayment")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowPayment", 0)],
                },
                new TweakDef
                {
                    Id = "nfcpol-disable-tap-to-connect",
                    Label = "Disable NFC Tap-to-Connect (Wi-Fi/BT Pairing)",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Prevents NFC from being used to automatically pair Bluetooth headsets, speakers, or configure Wi-Fi on another device via WPS. Reduces the attack surface of proximity-based device pairing. Default: tap-to-connect enabled. Recommended: 1.",
                    Tags = ["nfc", "tap-to-connect", "bluetooth", "wifi", "pairing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NFC-initiated Bluetooth pairing and Wi-Fi exchange are blocked; manual pairing through Settings is unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowHandshake", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowHandshake")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowHandshake", 0)],
                },
                new TweakDef
                {
                    Id = "nfcpol-disable-nfc-tag-reading",
                    Label = "Disable NFC Tag Reading",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Prevents Windows from reading data from passive NFC tags (such as NFC smart posters, RFID access badges used as open-URL triggers). Eliminates malicious-tag attack vectors. Default: tag reading enabled. Recommended: 1.",
                    Tags = ["nfc", "tag", "rfid", "read", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NFC passive tag reading is disabled; smart poster / rogue-tag attacks are blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowTagReading", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowTagReading")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowTagReading", 0)],
                },
                new TweakDef
                {
                    Id = "nfcpol-disable-secure-element",
                    Label = "Disable NFC Secure Element Access",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Blocks applications from accessing the NFC Secure Element (SE) — the tamper-resistant chip used for contactless payment credentials. Prevents SE-based payment credential theft. Default: SE access controlled per-app. Recommended: 1 unless approved payment apps are deployed.",
                    Tags = ["nfc", "secure-element", "payment", "credentials", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "No app can access the NFC Secure Element; payment credentials stored there are inaccessible.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowSecureElement", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowSecureElement")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowSecureElement", 0)],
                },
                new TweakDef
                {
                    Id = "nfcpol-block-nfc-in-enterprise",
                    Label = "Block All NFC in Enterprise Mode",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Master switch to disable all NFC functionality system-wide when the device is on a corporate/enterprise network. Provides a blanket NFC lockdown without needing individual element controls. Default: not restricted. Recommended: 1.",
                    Tags = ["nfc", "enterprise", "lockdown", "wireless", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "All NFC features (read, write, payment, pairing, SE) are locked down in enterprise environments.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowNFCInEnterprise", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowNFCInEnterprise")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowNFCInEnterprise", 0)],
                },
                new TweakDef
                {
                    Id = "nfcpol-disable-nfc-sharing",
                    Label = "Disable NFC Proximity Data Sharing",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Prevents Windows from sharing files, contacts, or links via NFC proximity transfer (similar to Android Beam). Stops inadvertent or malicious near-field data exfiltration. Default: sharing enabled on NFC hardware. Recommended: 1.",
                    Tags = ["nfc", "sharing", "proximity", "dlp", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NFC proximity data sharing is blocked; files and URLs cannot be transferred via NFC tap.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowSharing", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowSharing")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowSharing", 0)],
                },
                new TweakDef
                {
                    Id = "nfcpol-disable-nfc-host-card-emulation",
                    Label = "Disable NFC Host Card Emulation (HCE)",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Blocks Host Card Emulation — the mode that allows a device to emulate an NFC smart card (e.g., transit card, building access badge) in software. Prevents rogue HCE apps from cloning or spoofing credential cards. Default: HCE enabled on supported hardware. Recommended: 1.",
                    Tags = ["nfc", "hce", "card-emulation", "credentials", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "HCE is blocked; no app can emulate an NFC card, preventing badge cloning or transit card spoofing.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowHostCardEmulation", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowHostCardEmulation")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowHostCardEmulation", 0)],
                },
                new TweakDef
                {
                    Id = "nfcpol-log-nfc-activity",
                    Label = "Enable NFC Activity Audit Logging",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Records NFC tap events, tag reads, and connection establishments to the Security audit log. Provides a forensic trail of physical proximity events for DLP and incident investigations. Default: not logged. Recommended: 1 on monitored endpoints.",
                    Tags = ["nfc", "audit", "logging", "forensics", "dlp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "All NFC tap and connection events are written to the Security event log.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditNFCActivity", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditNFCActivity")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditNFCActivity", 1)],
                },
                new TweakDef
                {
                    Id = "nfcpol-disable-nfc-user-toggle",
                    Label = "Block Users from Toggling NFC in Settings",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Removes the NFC toggle from Settings → Network & Internet → Airplane Mode and NFC. Users cannot re-enable NFC regardless of the hardware switch state. Default: user toggle available. Recommended: 1 when NFC is disabled by policy.",
                    Tags = ["nfc", "settings", "user-restriction", "toggle", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "NFC Settings toggle is hidden/greyed out; policy setting is preserved regardless of user interaction.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUserNFCToggle", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUserNFCToggle")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUserNFCToggle", 1)],
                },
            ];
    }

    // ── NicTeamingPolicy ──
    private static class _NicTeamingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NICTeaming";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "nicteam-block-team-creation",
                    Label = "Block NIC Team Creation by Standard Users",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Prevents standard (non-administrator) users from creating new NIC teams (LBFO load-balancing / failover adapters), ensuring that network adapter bonding configurations are controlled exclusively by administrators.",
                    Tags = ["nic-teaming", "lbfo", "adapter", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NIC team creation blocked for standard users; LBFO adapter bonding requires admin rights.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowTeamCreationByNonAdmin", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowTeamCreationByNonAdmin")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowTeamCreationByNonAdmin", 0)],
                },
                new TweakDef
                {
                    Id = "nicteam-require-admin-for-deletion",
                    Label = "Require Admin to Delete NIC Teams",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Requires administrator privileges to delete NIC teams, preventing accidental or malicious destruction of load-balancing or failover network configurations by standard users or malicious scripts.",
                    Tags = ["nic-teaming", "lbfo", "deletion", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NIC team deletion requires admin rights; standard users cannot destroy bonded adapter configurations.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForTeamDeletion", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForTeamDeletion")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForTeamDeletion", 1)],
                },
                new TweakDef
                {
                    Id = "nicteam-set-teaming-mode-static",
                    Label = "Set Default NIC Teaming Mode to Static (Switch Independent)",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Sets the default NIC teaming mode to Switch Independent (no LACP negotiation), which does not require switch-side port aggregation configuration and works with any managed switch that allows multiple ports to the same host.",
                    Tags = ["nic-teaming", "lbfo", "teaming-mode", "switch-independent", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Default NIC teaming mode set to Switch Independent; LACP negotiation not required on the switch.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultTeamingMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultTeamingMode")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultTeamingMode", 0)],
                },
                new TweakDef
                {
                    Id = "nicteam-enable-team-health-logging",
                    Label = "Enable NIC Team Health Change Event Logging",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Enables event log entries for NIC team health state changes including member adapter failures, additions, and team-wide operational state changes for proactive failover monitoring.",
                    Tags = ["nic-teaming", "lbfo", "health", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NIC team health state changes logged; adapter failures and team state changes recorded in event log.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableTeamHealthEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableTeamHealthEventLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableTeamHealthEventLogging", 1)],
                },
                new TweakDef
                {
                    Id = "nicteam-disable-team-ui",
                    Label = "Disable NIC Teaming Configuration UI for Standard Users",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Removes the NIC Teaming page from Server Manager and Network Connections for non-administrator users, preventing accidental or unauthorised modification of NIC team configurations.",
                    Tags = ["nic-teaming", "lbfo", "ui", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "NIC Teaming UI hidden from standard users; only admins can access teaming configuration.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTeamingUIForNonAdmin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTeamingUIForNonAdmin")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTeamingUIForNonAdmin", 1)],
                },
                new TweakDef
                {
                    Id = "nicteam-set-load-balance-dynamic",
                    Label = "Set Default NIC Team Load Balancing Mode to Dynamic",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Sets the default NIC team load balancing algorithm to Dynamic mode, which distributes outbound traffic based on TCP/UDP flow measurements and periodically rebalances to prevent hot-spotting on a single team member.",
                    Tags = ["nic-teaming", "lbfo", "load-balancing", "dynamic", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NIC team default load balancing set to Dynamic; flow-aware rebalancing across team members.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultLBAlgorithm", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultLBAlgorithm")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultLBAlgorithm", 2)],
                },
                new TweakDef
                {
                    Id = "nicteam-set-standby-adapter-failover",
                    Label = "Set Default NIC Team Standby Adapter Mode for Failover",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Configures the default NIC team to use an active-standby topology where one adapter is always idle as a hot standby, ensuring seamless failover with no traffic disruption when the primary adapter fails.",
                    Tags = ["nic-teaming", "lbfo", "failover", "standby", "resilience", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NIC team standby mode enabled; one adapter held as hot standby for seamless failover on primary failure.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableStandbyAdapterFailover", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableStandbyAdapterFailover")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableStandbyAdapterFailover", 1)],
                },
                new TweakDef
                {
                    Id = "nicteam-block-team-membership-change",
                    Label = "Block Standard Users from Modifying NIC Team Membership",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Prevents standard users from adding adapters to or removing adapters from existing NIC teams, ensuring team membership changes can only be made by administrators.",
                    Tags = ["nic-teaming", "lbfo", "membership", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NIC team membership modification blocked for standard users; only admins add/remove team members.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockMembershipChangeByNonAdmin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockMembershipChangeByNonAdmin")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockMembershipChangeByNonAdmin", 1)],
                },
                new TweakDef
                {
                    Id = "nicteam-audit-team-cfg-changes",
                    Label = "Audit NIC Team Configuration Changes",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Enables Security event log entries for all NIC team configuration changes (create, delete, member add/remove, mode change), providing a change-management audit trail for network team availability configurations.",
                    Tags = ["nic-teaming", "lbfo", "audit", "configuration-change", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NIC team config changes audited; create/delete/membership events logged for change management.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditTeamConfigChanges", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditTeamConfigChanges")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditTeamConfigChanges", 1)],
                },
                new TweakDef
                {
                    Id = "nicteam-disable-nicteam-telemetry",
                    Label = "Disable NIC Teaming Telemetry Reporting to Microsoft",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Prevents the NIC Teaming subsystem from sending adapter bonding performance and health telemetry to Microsoft, protecting internal network adapter topology from cloud disclosure.",
                    Tags = ["nic-teaming", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "NIC Teaming telemetry to Microsoft disabled; adapter bonding topology not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableNICTeamingTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableNICTeamingTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableNICTeamingTelemetry", 1)],
                },
            ];
    }

    // ── NtpGpoPolicy ──
    private static class _NtpGpoPolicy
    {
        private const string NtpClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Client";
        private const string NtpParametersKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Parameters";
        private const string NtpConfigKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Config";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ntpgpo-enable-ntp-client",
                Label = "NTP Policy: Enable Windows NTP Client via Group Policy",
                Category = "Network — Network Monitoring",
                Description =
                    "Configures the Windows Time service (W32Time) client to retrieve time from an NTP server by enabling the NTP client via the Policies registry path. This policy controls the Windows Time service behaviour at the machine level. Accurate time synchronisation is a prerequisite for Kerberos authentication, SSL/TLS certificate validation, log correlation, and compliance auditing. Forcing the NTP client on ensures time sync cannot be accidentally disabled by local administrators.",
                Tags = ["ntp", "time sync", "w32time", "kerberos", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NtpClientKey],
                ApplyOps = [RegOp.SetDword(NtpClientKey, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(NtpClientKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(NtpClientKey, "Enabled", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Ensures Windows NTP client is active; required for domain Kerberos auth, certificate validation, and audit log consistency.",
            },
            new TweakDef
            {
                Id = "ntpgpo-set-cross-site-sync-flags",
                Label = "NTP Policy: Set NTP Cross-Site Synchronisation Flags",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets the CrossSiteSyncFlags value in the W32Time Client policy to 2, allowing the Windows Time service to synchronise from NTP time sources across AD sites. By default, Windows Time prefers NTP sources within the same AD site. Setting this flag allows cross-site sync as a fallback when no local NTP source is available, preventing time drift in remote sites with poor DC connectivity.",
                Tags = ["ntp", "time sync", "active directory", "cross site", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NtpClientKey],
                ApplyOps = [RegOp.SetDword(NtpClientKey, "CrossSiteSyncFlags", 2)],
                RemoveOps = [RegOp.DeleteValue(NtpClientKey, "CrossSiteSyncFlags")],
                DetectOps = [RegOp.CheckDword(NtpClientKey, "CrossSiteSyncFlags", 2)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Allows NTP sync from DCs in other AD sites; recommended for branch-office deployments with limited local DC bandwidth.",
            },
            new TweakDef
            {
                Id = "ntpgpo-set-special-poll-interval",
                Label = "NTP Policy: Set Special Poll Interval for NTP Synchronisation",
                Category = "Network — Network Monitoring",
                Description =
                    "Configures a specific NTP poll interval (in seconds) for the Windows Time service client operating in SpecialInterval mode. The default Windows Time poll interval can be too infrequent for high-security environments where clock drift is measured in seconds. A 900-second (15-minute) interval ensures that machines re-synchronise frequently enough to stay within the 5-minute Kerberos clock skew limit and to provide accurate timestamps for security event logs.",
                Tags = ["ntp", "time sync", "poll interval", "kerberos", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NtpClientKey],
                ApplyOps = [RegOp.SetDword(NtpClientKey, "SpecialPollInterval", 900)],
                RemoveOps = [RegOp.DeleteValue(NtpClientKey, "SpecialPollInterval")],
                DetectOps = [RegOp.CheckDword(NtpClientKey, "SpecialPollInterval", 900)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Sets NTP poll interval to 15 minutes; smaller intervals add network traffic but improve time accuracy.",
            },
            new TweakDef
            {
                Id = "ntpgpo-set-resolve-peer-backoff-min",
                Label = "NTP Policy: Set Minimum Peer Resolution Backoff Interval",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets the ResolvePeerBackOffMinutes value (in minutes) to 15, controlling the minimum time the Windows Time client waits before retrying a failed NTP peer resolution attempt. Short backoff intervals cause the W32Time service to hammer DNS and the NTP server with rapid retries after network outages. A 15-minute minimum backoff reduces NTP server load and prevents false-positive flapping alerts in network monitoring systems.",
                Tags = ["ntp", "time sync", "backoff", "network load", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NtpClientKey],
                ApplyOps = [RegOp.SetDword(NtpClientKey, "ResolvePeerBackOffMinutes", 15)],
                RemoveOps = [RegOp.DeleteValue(NtpClientKey, "ResolvePeerBackOffMinutes")],
                DetectOps = [RegOp.CheckDword(NtpClientKey, "ResolvePeerBackOffMinutes", 15)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Sets minimum NTP retry backoff to 15 minutes; reduces NTP/DNS chatter after transient network failures.",
            },
            new TweakDef
            {
                Id = "ntpgpo-set-resolve-peer-backoff-max",
                Label = "NTP Policy: Set Maximum Peer Resolution Backoff Retry Count",
                Category = "Network — Network Monitoring",
                Description =
                    "Sets the ResolvePeerBackOffMaxTimes value to 7, controlling the maximum number of exponential backoff retry doublings before the Windows Time client gives up and stops attempting to resolve an NTP peer. Without a cap, the exponential backoff can grow indefinitely, resulting in machines that stop trying to sync hours after a network issue clears. Limiting to 7 doublings (max backoff of 15 min × 2^7 ≈ 32 hours) balances persistence with eventual giving-up.",
                Tags = ["ntp", "time sync", "backoff retry", "network resilience", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NtpClientKey],
                ApplyOps = [RegOp.SetDword(NtpClientKey, "ResolvePeerBackOffMaxTimes", 7)],
                RemoveOps = [RegOp.DeleteValue(NtpClientKey, "ResolvePeerBackOffMaxTimes")],
                DetectOps = [RegOp.CheckDword(NtpClientKey, "ResolvePeerBackOffMaxTimes", 7)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Limits NTP peer retry doublings to 7; after ~32 hours without an NTP response, the service stops retrying until restarted.",
            },
        ];
    }

    // ── ProxyBypassPolicy ──
    private static class _ProxyBypassPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Control Panel";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "proxbyp-disable-autodetect",
                    Label = "Disable Auto-Detect Proxy Settings",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Disables automatic proxy detection (AutoDetect=0), preventing browsers and WinINET from trying to discover a proxy server automatically via WPAD or DHCP.",
                    Tags = ["proxy", "autodetect", "network", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Stops auto-detection; clients must use explicitly configured proxy settings.",
                    ApplyOps = [RegOp.SetDword(Key, "AutoDetect", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutoDetect")],
                    DetectOps = [RegOp.CheckDword(Key, "AutoDetect", 0)],
                },
                new TweakDef
                {
                    Id = "proxbyp-disable-autoconfig-url",
                    Label = "Disable Proxy Auto-Config URL",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Disables policy-driven auto-configuration URL (PAC file) processing by forcing ProxyAutoConfigUrl to an empty value, preventing unauthorized PAC file adoption.",
                    Tags = ["proxy", "pac", "autoconfig", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Disables PAC file; proxy must be explicitly set or controlled by separate policy.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoConfigUrl", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoConfigUrl")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoConfigUrl", 1)],
                },
                new TweakDef
                {
                    Id = "proxbyp-block-settings-change",
                    Label = "Block Proxy Settings Change by Users",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Locks the Connections settings page in Internet Properties so standard users cannot modify proxy settings (Connections=1 in IE Control Panel policy).",
                    Tags = ["proxy", "settings", "lockdown", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents user-side proxy bypass; admin privileges required to change proxy settings.",
                    ApplyOps = [RegOp.SetDword(Key2, "Connections", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "Connections")],
                    DetectOps = [RegOp.CheckDword(Key2, "Connections", 1)],
                },
                new TweakDef
                {
                    Id = "proxbyp-disable-wpad",
                    Label = "Disable WPAD (Web Proxy Auto-Discovery)",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Disables WPAD by setting DisableWpad=1, preventing the client from broadcasting WS-Discovery or DNS queries for a WPAD host, which can be spoofed by attackers.",
                    Tags = ["proxy", "wpad", "network", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Eliminates WPAD attack surface; explicit proxy configuration required.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWpad", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWpad")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWpad", 1)],
                },
                new TweakDef
                {
                    Id = "proxbyp-require-authenticated-proxy",
                    Label = "Require Authenticated Proxy",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Enforces proxy server authentication requirement, ensuring all WinINET proxy connections use valid enterprise credentials.",
                    Tags = ["proxy", "authentication", "network", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Authenticated proxy prevents anonymous internet access via the corporate proxy.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAuthenticatedProxy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAuthenticatedProxy")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAuthenticatedProxy", 1)],
                },
                new TweakDef
                {
                    Id = "proxbyp-block-direct-internet",
                    Label = "Block Direct Internet Access",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Blocks direct internet connections by policy, forcing all external traffic through the configured enterprise proxy server.",
                    Tags = ["proxy", "internet", "network", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Enforces proxy-only outbound; direct connections to internet IPs are blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockDirectInternetAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockDirectInternetAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockDirectInternetAccess", 1)],
                },
                new TweakDef
                {
                    Id = "proxbyp-disable-bypass-for-local",
                    Label = "Disable Proxy Bypass for Local Addresses",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Disables the default proxy bypass for local (intranet) addresses so that all traffic — including intranet — routes through the proxy for inspection.",
                    Tags = ["proxy", "local", "bypass", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Routes intranet traffic through proxy; may increase latency on local resources.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableBypassForLocal", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableBypassForLocal")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableBypassForLocal", 1)],
                },
                new TweakDef
                {
                    Id = "proxbyp-lock-proxy-settings",
                    Label = "Lock Proxy Settings from Changes",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Applies the proxy-settings lockdown policy so that users cannot view or change proxy configuration through the Internet Options dialog.",
                    Tags = ["proxy", "lockdown", "settings", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Internet Options Connections tab locked; admin-only access to proxy config.",
                    ApplyOps = [RegOp.SetDword(Key2, "Advanced", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "Advanced")],
                    DetectOps = [RegOp.CheckDword(Key2, "Advanced", 1)],
                },
                new TweakDef
                {
                    Id = "proxbyp-enforce-proxy-server",
                    Label = "Enforce Proxy Server Policy Setting",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Enables the ProxySettingsPerUser=0 policy to enforce machine-wide proxy settings rather than per-user, preventing individual users from substituting their own proxy.",
                    Tags = ["proxy", "server", "machine", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Machine-wide proxy enforced; per-user proxy overrides are blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "ProxySettingsPerUser", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ProxySettingsPerUser")],
                    DetectOps = [RegOp.CheckDword(Key, "ProxySettingsPerUser", 0)],
                },
                new TweakDef
                {
                    Id = "proxbyp-disable-vpn-split-tunneling",
                    Label = "Disable VPN Split Tunneling via Proxy Policy",
                    Category = "Network — Network Monitoring",
                    Description =
                        "Disables VPN split tunneling enforcement bypass by requiring all traffic to route through the proxy, eliminating the split-tunnel proxy-bypass vector.",
                    Tags = ["proxy", "vpn", "split-tunneling", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Forces all traffic through proxy; VPN split-tunnel internet bypass is eliminated.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSplitTunnelingBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSplitTunnelingBypass")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSplitTunnelingBypass", 1)],
                },
            ];
    }

    // ── RadiusAuthPolicy ──
    private static class _RadiusAuthPolicy
    {
        private const string NpsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NPS";

        private const string NetworkAccessKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkAccess";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "radius-require-server-cert-validation",
                    Label = "RADIUS: Require Server Certificate Validation for EAP-TLS/PEAP",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets ValidateServerCert=1 in NPS policy. Requires client supplicants to validate the NPS/RADIUS server's TLS certificate before completing the EAP-TLS or PEAP authentication handshake. Without server certificate validation, a rogue RADIUS server can impersonate the legitimate NPS server (evil twin attack) and harvest EAP credentials or perform man-in-the-middle authentication. Server certificate validation prevents this by verifying the RADIUS server's identity using the trusted PKI before committing credentials.",
                    Tags = ["radius", "nps", "eap", "certificate", "authentication"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "RADIUS server certificate must be trusted by client machines. Deploy NPS server certificate from an enterprise CA that clients trust. Without this setup, 802.1x authentication fails.",
                    ApplyOps = [RegOp.SetDword(NpsKey, "ValidateServerCert", 1)],
                    RemoveOps = [RegOp.DeleteValue(NpsKey, "ValidateServerCert")],
                    DetectOps = [RegOp.CheckDword(NpsKey, "ValidateServerCert", 1)],
                },
                new TweakDef
                {
                    Id = "radius-disable-legacy-eap-md5",
                    Label = "RADIUS: Disable Legacy EAP-MD5 Authentication Method",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets DisableEapMD5=1 in NPS policy. Removes EAP-MD5 from the list of accepted EAP authentication methods in the Windows Network Policy Server. EAP-MD5 is the oldest EAP method and is fundamentally insecure: it is vulnerable to dictionary attacks and offline brute force because the MD5 challenge-response is transmitted in the clear. RFC 9190 has deprecated EAP-MD5. Modern deployments should use EAP-TLS (certificate-based) or PEAP-MSCHAPv2 (password-based with TLS tunnel).",
                    Tags = ["radius", "nps", "eap", "md5", "authentication"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "EAP-MD5 clients cannot authenticate. Legacy devices that support only EAP-MD5 must be replaced or given alternative access. Most enterprise clients support EAP-TLS or PEAP.",
                    ApplyOps = [RegOp.SetDword(NpsKey, "DisableEapMD5", 1)],
                    RemoveOps = [RegOp.DeleteValue(NpsKey, "DisableEapMD5")],
                    DetectOps = [RegOp.CheckDword(NpsKey, "DisableEapMD5", 1)],
                },
                new TweakDef
                {
                    Id = "radius-enable-accounting-logging",
                    Label = "RADIUS: Enable NPS Accounting Log to Windows Event Log",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets AccountingLogging=1 in NPS policy. Enables the NPS to log RADIUS accounting records (Start, Stop, Interim-Update, Accounting-On/Off) to the Windows Security Event Log. Accounting logs record all network access sessions: who connected, for how long, from what endpoint, with what access policy applied. These logs are essential for security investigations (who was connected when an incident occurred?) and compliance (demonstrating network access is tracked and audited).",
                    Tags = ["radius", "nps", "accounting", "logging", "audit"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "RADIUS accounting events are written to the Security Event Log. Ensure sufficient log size retention is configured. Windows Event Log defaults may fill quickly in large environments.",
                    ApplyOps = [RegOp.SetDword(NpsKey, "AccountingLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(NpsKey, "AccountingLogging")],
                    DetectOps = [RegOp.CheckDword(NpsKey, "AccountingLogging", 1)],
                },
                new TweakDef
                {
                    Id = "radius-set-auth-retry-limit",
                    Label = "RADIUS: Limit Authentication Retry Attempts to 3 per Session",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets MaxAuthRetries=3 in NetworkAccess policy. Limits the number of consecutive EAP authentication retry attempts per network access session before the access request is rejected. Without a retry limit, an attacker can enumerate EAP authentication attempts indefinitely (automated brute-force). Setting the limit to 3 matches best practice from 802.1x implementations: a user who mistyped their PIN gets two retries, and the third failure terminates the connection, requiring physical re-insertion of the token or reconnection.",
                    Tags = ["radius", "authentication", "retry", "brute-force", "protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "After 3 failed authentication attempts the session is terminated. Users with multiple errors (e.g., wrong smart card PIN) must disconnect and reconnect. Adjust to 5 for environments with frequent PIN entry errors.",
                    ApplyOps = [RegOp.SetDword(NetworkAccessKey, "MaxAuthRetries", 3)],
                    RemoveOps = [RegOp.DeleteValue(NetworkAccessKey, "MaxAuthRetries")],
                    DetectOps = [RegOp.CheckDword(NetworkAccessKey, "MaxAuthRetries", 3)],
                },
                new TweakDef
                {
                    Id = "radius-set-eap-timeout-30s",
                    Label = "RADIUS: Set EAP Authentication Timeout to 30 Seconds",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets EapTimeout=30 in NPS policy. Configures the maximum duration the NPS allows for a single EAP authentication exchange. If the EAP conversation (from initial EAP Identity request to EAP Success/Failure) takes longer than 30 seconds, the NPS terminates the access request with an Access-Reject. Short timeouts prevent slow-response attacks and stale session accumulation from half-open EAP conversations. 30 seconds is sufficient for all current EAP methods including EAP-TLS on slow links.",
                    Tags = ["radius", "nps", "eap", "timeout", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Authentication exceeding 30 seconds is terminated. Smart card logon with OCSP/CRL check offline might briefly exceed this. Increase to 60 seconds if authentication latency is observed in slower environments.",
                    ApplyOps = [RegOp.SetDword(NpsKey, "EapTimeout", 30)],
                    RemoveOps = [RegOp.DeleteValue(NpsKey, "EapTimeout")],
                    DetectOps = [RegOp.CheckDword(NpsKey, "EapTimeout", 30)],
                },
                new TweakDef
                {
                    Id = "radius-enable-nps-audit-success",
                    Label = "RADIUS: Enable NPS Success Audit Events",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets AuditSuccessAuthentications=1 in NPS policy. Enables the logging of successful RADIUS Access-Accept events to the Windows Security Event Log (Event 6272: NPS granted access to a user). Success logging allows security operations teams to establish a baseline of acceptable network access patterns and detect anomalies (a user authenticating from an unknown location or at an unusual time). Without success logging, only failures are recorded, making it impossible to detect horizontal movement via legitimate credentials.",
                    Tags = ["radius", "nps", "audit", "success", "logging"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "RADIUS success events are written to Security Event Log. Event 6272 is generated for each successful 802.1x or VPN authentication. Volume will be high in large environments — ensure SIEM can handle the ingestion rate.",
                    ApplyOps = [RegOp.SetDword(NpsKey, "AuditSuccessAuthentications", 1)],
                    RemoveOps = [RegOp.DeleteValue(NpsKey, "AuditSuccessAuthentications")],
                    DetectOps = [RegOp.CheckDword(NpsKey, "AuditSuccessAuthentications", 1)],
                },
                new TweakDef
                {
                    Id = "radius-enable-nps-audit-failure",
                    Label = "RADIUS: Enable NPS Failure Audit Events",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets AuditFailedAuthentications=1 in NPS policy. Enables the logging of failed RADIUS Access-Reject events to the Windows Security Event Log (Event 6273: NPS denied access to a user, with the specific rejection reason code). Failure audit logging is essential for: detecting brute-force or credential stuffing attacks against network access, diagnosing 802.1x EAP failure reasons (certificate issues, policy mismatches, account disabled), and regulatory compliance that requires failed access to be logged.",
                    Tags = ["radius", "nps", "audit", "failure", "logging"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "RADIUS failure events are written to Security Event Log. Event 6273 is generated for each rejected 802.1x/VPN request. Essential for network access security monitoring.",
                    ApplyOps = [RegOp.SetDword(NpsKey, "AuditFailedAuthentications", 1)],
                    RemoveOps = [RegOp.DeleteValue(NpsKey, "AuditFailedAuthentications")],
                    DetectOps = [RegOp.CheckDword(NpsKey, "AuditFailedAuthentications", 1)],
                },
                new TweakDef
                {
                    Id = "radius-disable-pap-authentication",
                    Label = "RADIUS: Disable PAP (Password Authentication Protocol) on NPS",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets DisablePAP=1 in NPS policy. Removes PAP from the allowed RADIUS authentication protocols. PAP transmits the user password as cleartext (obfuscated only by MD5 XOR with the RADIUS shared secret) in the RADIUS Access-Request attribute (User-Password). An attacker with access to RADIUS traffic and knowledge of the shared secret can trivially recover user passwords from PAP requests. PAP is explicitly prohibited by PCI-DSS and NIST SP 800-162 for network authentication.",
                    Tags = ["radius", "pap", "authentication", "cleartext", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "PAP authentication is disabled. Clients that support only PAP (rare legacy devices) cannot authenticate. Verify all network clients support at least CHAP or EAP.",
                    ApplyOps = [RegOp.SetDword(NpsKey, "DisablePAP", 1)],
                    RemoveOps = [RegOp.DeleteValue(NpsKey, "DisablePAP")],
                    DetectOps = [RegOp.CheckDword(NpsKey, "DisablePAP", 1)],
                },
                new TweakDef
                {
                    Id = "radius-restrict-shared-secret-length",
                    Label = "RADIUS: Enforce Minimum 22-Character RADIUS Shared Secret",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets MinSharedSecretLength=22 in NPS policy. Enforces a minimum length for the RADIUS shared secret (the password shared between the NPS server and authenticating access points/NAS devices). The RADIUS shared secret is used as a key in the User-Password MD5 obfuscation and in the Message-Authenticator HMAC-MD5. Short shared secrets are vulnerable to offline dictionary and brute-force attacks on captured RADIUS traffic. NIST SP 800-162 recommends at least 22 random characters for RADIUS shared secrets.",
                    Tags = ["radius", "shared-secret", "password", "length", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Shared secrets shorter than 22 characters are rejected at NPS configuration. Existing access points with short shared secrets must be reconfigured. Minimum change required for existing deployments.",
                    ApplyOps = [RegOp.SetDword(NpsKey, "MinSharedSecretLength", 22)],
                    RemoveOps = [RegOp.DeleteValue(NpsKey, "MinSharedSecretLength")],
                    DetectOps = [RegOp.CheckDword(NpsKey, "MinSharedSecretLength", 22)],
                },
                new TweakDef
                {
                    Id = "radius-enable-proxy-state-attribute",
                    Label = "RADIUS: Enable Proxy-State Attribute Forwarding",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets EnableProxyState=1 in NPS policy. Enables the preservation and forwarding of the RADIUS 'Proxy-State' attribute (attribute 33) in proxied RADIUS requests. When an NPS server forwards authentication requests to another RADIUS server in a tiered proxy topology (e.g., NPS proxy → corporate NPS → Active Directory), the Proxy-State attribute allows the proxy chain to correlate responses to their originating requests. Without Proxy-State, high-volume RADIUS proxy deployments suffer mismatched request-response correlation.",
                    Tags = ["radius", "proxy", "nps", "forwarding", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Proxy-State forwarding is enabled. Only relevant in proxied RADIUS deployments. Required for correct NPS proxy chain configuration.",
                    ApplyOps = [RegOp.SetDword(NpsKey, "EnableProxyState", 1)],
                    RemoveOps = [RegOp.DeleteValue(NpsKey, "EnableProxyState")],
                    DetectOps = [RegOp.CheckDword(NpsKey, "EnableProxyState", 1)],
                },
            ];
    }

    // ── RemoteNetworkAccessPolicy ──
    private static class _RemoteNetworkAccessPolicy
    {
        private const string RasKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkAccessProtection";

        private const string RemAccKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Rpc";

        private const string RasMgrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "rnas-disable-nap-client",
                    Label = "Remote Access: Disable Network Access Protection (NAP) Client",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets EnableNap=0 in Network Access Protection policy. Disables the legacy Windows Network Access Protection (NAP) client which was deprecated in Windows Server 2012 R2 and removed from the enforcement role. NAP agents running on modern Windows clients generate event log warnings and consume background resources checking against a non-existent NAP infrastructure. On corporate networks without NAP servers, disabling the NAP client eliminates background health validation traffic and event log noise.",
                    Tags = ["remote-access", "nap", "legacy", "network", "cleanup"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Disables legacy NAP client. No impact on modern network access enforcement (Intune Compliance, Azure AD Conditional Access, NPS). Only affects deprecated Windows Server 2008-era NAP infrastructure.",
                    ApplyOps = [RegOp.SetDword(RasKey, "EnableNap", 0)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "EnableNap")],
                    DetectOps = [RegOp.CheckDword(RasKey, "EnableNap", 0)],
                },
                new TweakDef
                {
                    Id = "rnas-enable-remote-access-audit",
                    Label = "Remote Access: Enable Remote Access Service Audit Logging",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets EnableRemoteAccessAudit=1 in Remote Access policy. Enables comprehensive audit logging for Windows RAS/VPN connection events, including connection establishment, authentication success/failure, accounting start/stop, and session termination. These events are critical for SIEM correlation, compliance reporting (SOC 2, ISO 27001), and incident response timelines when investigating unauthorized remote access. Without this policy, VPN connection events may not appear in the Windows Security event log.",
                    Tags = ["remote-access", "vpn", "audit", "logging", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Generates VPN connection/disconnection audit events in the Security log. Ensure log capacity and SIEM forwarding are configured to handle the additional log volume.",
                    ApplyOps = [RegOp.SetDword(RasMgrKey, "EnableRemoteAccessAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasMgrKey, "EnableRemoteAccessAudit")],
                    DetectOps = [RegOp.CheckDword(RasMgrKey, "EnableRemoteAccessAudit", 1)],
                },
                new TweakDef
                {
                    Id = "rnas-disable-pap-auth",
                    Label = "Remote Access: Disable PAP (Plaintext Password) Authentication",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets AllowPap=0 in Remote Access policy. Disables Password Authentication Protocol (PAP) for VPN authentication. PAP transmits usernames and passwords in plaintext in the CHAP exchange, making them trivially interceptable on any network where the traffic can be captured (including TCP/IP networks without encryption). All modern VPN implementations use MSCHAPv2 or certificate-based authentication; PAP support is a legacy compatibility option that should be removed from all VPN endpoints.",
                    Tags = ["remote-access", "pap", "authentication", "plaintext", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Breaks VPN clients using PAP authentication. All modern VPN clients use MSCHAPv2 or EAP; PAP is only used by very old Cisco/legacy clients.",
                    ApplyOps = [RegOp.SetDword(RasMgrKey, "AllowPap", 0)],
                    RemoveOps = [RegOp.DeleteValue(RasMgrKey, "AllowPap")],
                    DetectOps = [RegOp.CheckDword(RasMgrKey, "AllowPap", 0)],
                },
                new TweakDef
                {
                    Id = "rnas-set-idle-hwm-timeout",
                    Label = "Remote Access: Set Remote Access Idle Timeout to 20 Minutes",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets IdleTimeoutMinutes=20 in Remote Access policy. Sets the VPN idle timeout: after 20 minutes of no user-initiated traffic through the VPN tunnel, the server terminates the connection. Idle VPN sessions hold server resources (IP allocations, NAT state, crypto session keys) indefinitely without this timeout. The 20-minute window accommodates brief work pauses while ensuring sessions from laptops left unattended in public locations are eventually cleaned up.",
                    Tags = ["remote-access", "vpn", "idle-timeout", "resource", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "VPN sessions are terminated after 20 minutes of idle traffic. AOVPN clients reconnect automatically; manual VPN users must reconnect.",
                    ApplyOps = [RegOp.SetDword(RasMgrKey, "IdleTimeoutMinutes", 20)],
                    RemoveOps = [RegOp.DeleteValue(RasMgrKey, "IdleTimeoutMinutes")],
                    DetectOps = [RegOp.CheckDword(RasMgrKey, "IdleTimeoutMinutes", 20)],
                },
                new TweakDef
                {
                    Id = "rnas-disable-legacy-protocols",
                    Label = "Remote Access: Disable Legacy VPN Protocols (PPTP, L2TP without IPsec)",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets AllowPptp=0 in Remote Access policy. Disables PPTP (Point-to-Point Tunneling Protocol) VPN connections. PPTP uses RC4 encryption which is broken: known plaintext attacks can decrypt PPTP streams in real time given sufficient traffic. Microsoft released MS-CHAPv2 as PPTP's authentication but MS-CHAPv2 dictionary attacks complete in hours on modern hardware. PPTP provides no meaningful security. IKEv2 or SSL VPN should replace PPTP in all enterprise environments.",
                    Tags = ["remote-access", "pptp", "legacy", "encryption", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Breaks PPTP VPN connections. Very old client operating systems (Windows XP, early Android) using PPTP will not connect. IKEv2/SSL clients are unaffected.",
                    ApplyOps = [RegOp.SetDword(RasMgrKey, "AllowPptp", 0)],
                    RemoveOps = [RegOp.DeleteValue(RasMgrKey, "AllowPptp")],
                    DetectOps = [RegOp.CheckDword(RasMgrKey, "AllowPptp", 0)],
                },
                new TweakDef
                {
                    Id = "rnas-enable-ikev2-mobility",
                    Label = "Remote Access: Enable IKEv2 Mobility (Network Roaming Support)",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets EnableIkev2Mobility=1 in Remote Access policy. Activates IKEv2 MOBIKE (RFC 4555) support for VPN sessions. MOBIKE allows an active IKEv2 VPN tunnel to survive a client IP address change (e.g., switching from Ethernet to Wi-Fi, or from office to home network) without tearing down and re-establishing the tunnel. With MOBIKE, users experience seamless network transitions with VPN reconnection times of under 1 second instead of the 5–15 seconds required for full IKEv2 re-establishment.",
                    Tags = ["remote-access", "ikev2", "mobility", "mobike", "roaming"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enables IKEv2 MOBIKE for seamless network transitions. Requires VPN server support for MOBIKE (Windows Server 2016+ RRAS does support it).",
                    ApplyOps = [RegOp.SetDword(RasMgrKey, "EnableIkev2Mobility", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasMgrKey, "EnableIkev2Mobility")],
                    DetectOps = [RegOp.CheckDword(RasMgrKey, "EnableIkev2Mobility", 1)],
                },
                new TweakDef
                {
                    Id = "rnas-disable-password-caching",
                    Label = "Remote Access: Disable VPN Credential Caching",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets DisablePasswordCaching=1 in Remote Access policy. Prevents Windows from caching VPN usernames and passwords in the Windows Credential Manager. Cached VPN credentials are stored in the LSA credential store and can be extracted by credential dumping tools (Mimikatz, Windows Credential Editor) running as SYSTEM. An attacker who compromises a device should not be able to harvest VPN credentials for lateral movement. Disabling caching forces VPN re-authentication on each session.",
                    Tags = ["remote-access", "credential-cache", "credential", "mimikatz", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Users must enter VPN credentials at each connection. Does not affect certificate-based or SSO (SAML/OIDC) VPN authentication where no password is stored.",
                    ApplyOps = [RegOp.SetDword(RasMgrKey, "DisablePasswordCaching", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasMgrKey, "DisablePasswordCaching")],
                    DetectOps = [RegOp.CheckDword(RasMgrKey, "DisablePasswordCaching", 1)],
                },
                new TweakDef
                {
                    Id = "rnas-enable-rras-accounting",
                    Label = "Remote Access: Enable RRAS RADIUS Accounting for Sessions",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets EnableAccounting=1 in Remote Access policy. Enables RADIUS accounting messages (Start, Stop, Interim) from Windows RRAS to a configured RADIUS server. RADIUS accounting provides a complete audit trail of VPN session duration, bytes transferred, client IP address, and authentication method for each VPN connection. This data is required for network access compliance reporting, ISP billing reconciliation, and post-incident forensic analysis of data volume transferred over VPN.",
                    Tags = ["remote-access", "radius", "accounting", "rras", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires a RADIUS server configured in Windows RRAS. If no RADIUS server is configured, this setting has no effect.",
                    ApplyOps = [RegOp.SetDword(RasMgrKey, "EnableAccounting", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasMgrKey, "EnableAccounting")],
                    DetectOps = [RegOp.CheckDword(RasMgrKey, "EnableAccounting", 1)],
                },
                new TweakDef
                {
                    Id = "rnas-limit-concurrent-connections",
                    Label = "Remote Access: Limit Concurrent Remote Access Connections to 100",
                    Category = "Network — Radius Auth",
                    Description =
                        "Sets MaxConcurrentConnections=100 in Remote Access policy. Sets a configured maximum for simultaneous VPN/remote access connections on the server. Without a limit, RRAS servers can be overwhelmed by connection floods (either from legitimate growth or from denial-of-service attacks). Setting 100 as the limit on a non-production or branch RRAS server prevents resource exhaustion. Adjust the value based on server capacity (a typical Windows Server 2022 VM supports 200–500 concurrent VPN sessions).",
                    Tags = ["remote-access", "concurrent", "limit", "dos", "capacity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "VPN connections above the configured limit are rejected. Adjust value based on server CPU/RAM capacity and expected peak concurrent user count.",
                    ApplyOps = [RegOp.SetDword(RasMgrKey, "MaxConcurrentConnections", 100)],
                    RemoveOps = [RegOp.DeleteValue(RasMgrKey, "MaxConcurrentConnections")],
                    DetectOps = [RegOp.CheckDword(RasMgrKey, "MaxConcurrentConnections", 100)],
                },
            ];
    }
}
