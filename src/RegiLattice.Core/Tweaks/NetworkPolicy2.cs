namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyNetworkExt
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

    // ── SharedFoldersSmbPolicy ──
    private static class _SharedFoldersSmbPolicy
    {
        private const string LanWs = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation";
        private const string LanSrv = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "smbshare-restrict-null-session-access",
                Label = "Restrict Null Session Access to Named Pipes and Shares",
                Category = "Network — Radius Auth",
                Description =
                    "Prevents anonymous (null session) connections from accessing named pipes and shares, blocking unauthenticated SMB enumeration.",
                Tags = ["smb", "network", "security", "hardening", "anonymous"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(LanSrv, "RestrictNullSessAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "RestrictNullSessAccess")],
                DetectOps = [RegOp.CheckDword(LanSrv, "RestrictNullSessAccess", 1)],
            },
            new TweakDef
            {
                Id = "smbshare-clear-null-session-pipes",
                Label = "Clear Null Session Named Pipes List",
                Category = "Network — Radius Auth",
                Description = "Removes all named pipes accessible via anonymous null sessions, reducing SMB attack surface.",
                Tags = ["smb", "network", "security", "hardening", "anonymous"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetMultiSz(LanSrv, "NullSessionPipes", [])],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "NullSessionPipes")],
                DetectOps = [RegOp.CheckString(LanSrv, "NullSessionPipes", "")],
            },
            new TweakDef
            {
                Id = "smbshare-clear-null-session-shares",
                Label = "Clear Null Session Shares List",
                Category = "Network — Radius Auth",
                Description = "Removes all shares accessible via anonymous null sessions, preventing unauthenticated share enumeration.",
                Tags = ["smb", "network", "security", "hardening", "anonymous"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetMultiSz(LanSrv, "NullSessionShares", [])],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "NullSessionShares")],
                DetectOps = [RegOp.CheckString(LanSrv, "NullSessionShares", "")],
            },
            new TweakDef
            {
                Id = "smbshare-enable-forced-logoff",
                Label = "Enable Forced Logoff When Logon Hours Expire",
                Category = "Network — Radius Auth",
                Description = "Forces a logoff when a user's permitted logon hours expire, ensuring access control policies are enforced.",
                Tags = ["smb", "network", "security", "group-policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(LanSrv, "EnableForcedLogOff", 1)],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "EnableForcedLogOff")],
                DetectOps = [RegOp.CheckDword(LanSrv, "EnableForcedLogOff", 1)],
            },
            new TweakDef
            {
                Id = "smbshare-disable-admin-shares",
                Label = "Disable Default Administrative SMB Shares",
                Category = "Network — Radius Auth",
                Description =
                    "Disables automatic creation of administrative shares (C$, ADMIN$, IPC$), reducing remote administrative access surface.",
                Tags = ["smb", "network", "security", "hardening"],
                NeedsAdmin = true,
                CorpSafe = false,
                ApplyOps = [RegOp.SetDword(LanSrv, "AutoShareWks", 0)],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "AutoShareWks")],
                DetectOps = [RegOp.CheckDword(LanSrv, "AutoShareWks", 0)],
            },
            new TweakDef
            {
                Id = "smbshare-set-smb-max-connections",
                Label = "Set Maximum Concurrent SMB Connections",
                Category = "Network — Radius Auth",
                Description =
                    "Limits the number of concurrent SMB connections to 16,777,216 (MaxMpxCt), preventing resource exhaustion from SMB floods.",
                Tags = ["smb", "network", "performance", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(LanSrv, "MaxMpxCt", 16777216)],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "MaxMpxCt")],
                DetectOps = [RegOp.CheckDword(LanSrv, "MaxMpxCt", 16777216)],
            },
        ];
    }

    // ── SmbEncryptionPolicy ──
    private static class _SmbEncryptionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SMB";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "smbenc-require-smb-encryption",
                Label = "Require SMB Encryption for All Connections",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "SMB encryption protects file sharing traffic from network interception preventing passive eavesdropping on file transfers and credentials. Requiring SMB encryption for all connections ensures that unencrypted SMB traffic is rejected by the server component. Credential capture via SMB relay attacks is one of the most common lateral movement techniques in Windows environments. SMB encryption was introduced in SMB3 and prevents passive capture of file data and authentication metadata. Requiring encryption may prevent connections from legacy clients using SMB1 or SMB2 which do not support encryption. Enterprise environments should ensure all clients support SMB3 encryption before enforcing this requirement to avoid connectivity disruptions.",
                Tags = ["smb", "encryption", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "RequireEncryption", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-disable-smb1",
                Label = "Disable SMB Version 1 Protocol",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "SMB version 1 is a critically vulnerable legacy protocol that enabled the EternalBlue exploit used in WannaCry, NotPetya, and other devastating ransomware campaigns. Disabling SMB1 removes the attack surface for known critical vulnerabilities including MS17-010 and related exploit chains. SMB1 does not support encryption, modern authentication, or other security features present in SMB2 and SMB3. Microsoft has recommended disabling SMB1 since 2014 and Windows 10 October 2018 Update removed SMB1 from default installations. Legacy devices requiring SMB1 such as old network-attached storage or printers should be replaced or isolated. Disabling SMB1 via policy is one of the highest-impact low-risk security improvements available for Windows network environments.",
                Tags = ["smb", "smb1", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSMB1", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSMB1")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSMB1", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-disable-smb-guest",
                Label = "Disable SMB Guest Authentication",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SMB guest authentication allows connections to file shares without credentials which creates an unauthenticated access path for malicious actors. Disabling SMB guest authentication requires all SMB connections to present valid credentials preventing anonymous file access. Guest authentication can be exploited in man-in-the-middle attacks where the attacker poses as a file server and accepts guest connections. Windows 10 1709 disabled guest authentication by default but older endpoints and custom configurations may still allow it. Guest authentication combined with credential harvesting tools can facilitate lateral movement in Windows networks. Disabling guest authentication is standard enterprise hardening and should be enforced through Group Policy on all Windows endpoints.",
                Tags = ["smb", "guest", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGuestAuthentication", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGuestAuthentication")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGuestAuthentication", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-enable-smb-signing",
                Label = "Require SMB Packet Signing",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "SMB packet signing provides message integrity protection ensuring that SMB packets have not been tampered with in transit. Requiring SMB signing prevents SMB relay attacks where an attacker captures and replays authenticated SMB connections. NTLM relay attacks including the infamous SMBRelay attack family are blocked when both client and server require SMB signing. SMB signing has been available since Windows 2000 and has minimal performance impact with modern hardware. Without SMB signing an attacker with network access can perform authenticated relay attacks using captures from legitimate users. SMB signing should be required on all domain-joined endpoints and domain controllers where compatible clients exist.",
                Tags = ["smb", "signing", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequirePacketSigning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequirePacketSigning")],
                DetectOps = [RegOp.CheckDword(Key, "RequirePacketSigning", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-block-ntlm-smb",
                Label = "Restrict NTLM Authentication over SMB",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "NTLM authentication over SMB is vulnerable to pass-the-hash attacks, credential relay, and offline password cracking of captured hashes. Restricting NTLM over SMB forces Kerberos authentication for domain-joined resources which provides stronger authentication guarantees. NTLM relay attacks can allow credential capture and reuse even when SMB signing is not required on the target server. Kerberos authentication over SMB prevents the majority of credential forwarding attacks by requiring valid Kerberos tickets. NTLM restriction may affect connections to workgroup resources, NAS devices, and non-domain-joined systems that only support NTLM. Organizations should audit NTLM dependency before restricting to avoid legitimate connectivity disruption.",
                Tags = ["smb", "ntlm", "kerberos", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictNTLMOverSMB", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictNTLMOverSMB")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictNTLMOverSMB", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-enable-secure-dialect",
                Label = "Enforce Minimum SMB Dialect Version",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "SMB dialect negotiation allows clients and servers to agree on the highest supported protocol version but a minimum version prevents downgrade attacks. Enforcing a minimum SMB dialect prevents attackers from forcing connections to use older vulnerable protocol versions. Protocol downgrade attacks can force SMB2 or SMB3 connections to fall back to SMB1 which lacks security features. Setting a minimum dialect of SMB 3.0 ensures that all connections use the version with encryption and pre-authentication integrity. SMB dialect enforcement may break compatibility with servers or network appliances running older protocol versions. Enterprise SMB infrastructure should be surveyed for SMB dialect support before enforcing a minimum version requirement.",
                Tags = ["smb", "dialect", "protocol", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceMinDialectVersion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceMinDialectVersion")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceMinDialectVersion", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-enable-pre-auth-integrity",
                Label = "Enable SMB3 Pre-Authentication Integrity",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SMB pre-authentication integrity provides cryptographic protection of the connection establishment process preventing man-in-the-middle injection during negotiation. Enabling pre-authentication integrity ensures that an attacker cannot tamper with the protocol negotiation phase to downgrade security settings. Pre-authentication integrity was introduced in SMB 3.1.1 and is required for full SMB encryption security guarantees. Without pre-authentication integrity an attacker positioned between client and server can modify the negotiation to disable encryption or signing. Pre-authentication integrity requires both client and server to support SMB 3.1.1 which is available on Windows 10 1607 and Server 2016 onwards. Enabling pre-authentication integrity is part of SMB hardening and complements encryption and signing requirements.",
                Tags = ["smb", "pre-auth", "integrity", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePreAuthIntegrity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePreAuthIntegrity")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePreAuthIntegrity", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-disable-admin-shares",
                Label = "Disable Default Administrative SMB Shares",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Administrative shares (C$ D$ ADMIN$) are default SMB shares that provide full drive access to administrators over the network. Disabling administrative shares removes these implicit file access paths that are frequently exploited for lateral movement in compromised environments. Ransomware and APT tools use administrative shares to copy files to remote systems and execute commands via PsExec and similar tools. Administrative shares are required for some legitimate IT management tools but modern endpoint management platforms do not rely on them. Disabling administrative shares forces legitimate remote management to use more controlled and monitored APIs instead of raw file access. Security teams should verify that no critical management tools depend on administrative shares before disabling them in production.",
                Tags = ["smb", "admin-shares", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAdminShares", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAdminShares")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAdminShares", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-restrict-anonymous-smb",
                Label = "Restrict Anonymous Access to SMB Named Pipes",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Anonymous access to SMB named pipes allows unauthenticated network enumeration of user accounts, share names, and other sensitive system information. Restricting anonymous named pipe access prevents attackers from gathering reconnaissance information without valid credentials. Named pipes such as LSARPC and SAMR exposed anonymously can be used to enumerate domain accounts, enumerate local groups, and gather other information. The RestrictAnonymous policy setting controls anonymous access to IPC$ and named pipes as distinct from regular file shares. Anonymous SMB enumeration is used by network scanners and exploitation frameworks to identify targets and gather credential targets. Restricting anonymous access should be part of standard Windows hardening alongside null session restrictions.",
                Tags = ["smb", "anonymous", "named-pipes", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictAnonymousAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictAnonymousAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictAnonymousAccess", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-audit-smb-connections",
                Label = "Enable SMB Connection Audit Logging",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "SMB connection audit logging records all SMB share access events providing visibility into file server connections and access patterns. Enabling SMB audit logging generates security events for SMB sessions including user account, source address, share name, and file operations. SMB audit logs are essential for detecting anomalous file access patterns, data exfiltration via SMB shares, and lateral movement activity. Security audit data from SMB servers should be collected and analyzed by SIEM systems for real-time threat detection. Object access auditing for file shares enables logging of specific file-level access events within SMB shares for detailed forensic capability. SMB connection auditing combined with user behavior analytics can detect compromised accounts accessing unusual resources.",
                Tags = ["smb", "audit", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditSMBConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSMBConnections")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSMBConnections", 1)],
            },
        ];
    }

    // ── SmbNetworking ──
    private static class _SmbNetworking
    {
        private const string LmWks = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters";

        private const string LmSrv = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters";

        private const string MrxSmb = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MrxSmb\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "smb-enable-large-mtu",
                Label = "Enable SMB Large MTU Support",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["smb", "network", "mtu", "performance"],
                Description =
                    "Enables SMB large MTU (Maximum Transmission Unit) negotiation. "
                    + "Improves transfer speed on jumbo-frame-capable networks (MTU ≥ 9000).",
                ApplyOps = [RegOp.SetDword(LmWks, "EnableLargeMTU", 1)],
                RemoveOps = [RegOp.DeleteValue(LmWks, "EnableLargeMTU")],
                DetectOps = [RegOp.CheckDword(LmWks, "EnableLargeMTU", 1)],
            },
            new TweakDef
            {
                Id = "smb-reduce-dormant-file-limit",
                Label = "Reduce SMB Dormant File Connection Limit",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["smb", "network", "connections", "memory"],
                Description =
                    "Reduces the number of dormant SMB connections kept open from 1023 to 64. "
                    + "Frees memory on workstations that connect to many different file servers.",
                ApplyOps = [RegOp.SetDword(LmWks, "DormantFileLimit", 64)],
                RemoveOps = [RegOp.DeleteValue(LmWks, "DormantFileLimit")],
                DetectOps = [RegOp.CheckDword(LmWks, "DormantFileLimit", 64)],
            },
            new TweakDef
            {
                Id = "smb-increase-server-max-work-items",
                Label = "Increase SMB Server Work Items",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["smb", "server", "performance", "workitems"],
                Description =
                    "Increases the maximum number of queued work items the SMB server "
                    + "processes to 2048 (from the default 128). Helps high-concurrency file servers.",
                ApplyOps = [RegOp.SetDword(LmSrv, "MaxWorkItems", 2048)],
                RemoveOps = [RegOp.DeleteValue(LmSrv, "MaxWorkItems")],
                DetectOps = [RegOp.CheckDword(LmSrv, "MaxWorkItems", 2048)],
            },
            new TweakDef
            {
                Id = "smb-increase-server-max-raw-work-items",
                Label = "Increase SMB Server Raw Work Buffer",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["smb", "server", "performance", "buffer"],
                Description =
                    "Increases the raw-mode SMB server buffer count to 512 (default 4). "
                    + "Improves large sequential read/write throughput on file servers.",
                ApplyOps = [RegOp.SetDword(LmSrv, "MaxRawWorkItems", 512)],
                RemoveOps = [RegOp.DeleteValue(LmSrv, "MaxRawWorkItems")],
                DetectOps = [RegOp.CheckDword(LmSrv, "MaxRawWorkItems", 512)],
            },
            new TweakDef
            {
                Id = "smb-enforce-smb-signing-client",
                Label = "Enforce SMB Signing on Client",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["smb", "signing", "security", "hardening", "client"],
                Description =
                    "Requires the SMB client to sign all outgoing SMB connections. "
                    + "Pairs with the server-side enforcement tweak for full MITM protection. "
                    + "Reboot or network reconnect required for effect.",
                ApplyOps = [RegOp.SetDword(LmWks, "RequireSecuritySignature", 1)],
                RemoveOps = [RegOp.SetDword(LmWks, "RequireSecuritySignature", 0)],
                DetectOps = [RegOp.CheckDword(LmWks, "RequireSecuritySignature", 1)],
            },
            new TweakDef
            {
                Id = "smb-increase-collection-count",
                Label = "Increase SMB Write-Ahead Collection Count",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["smb", "write", "buffer", "performance"],
                Description =
                    "Increases the SMB client write-ahead collection buffer count to 32 "
                    + "(from default 16). Improves sequential write performance to file servers "
                    + "by batching more data before flushing.",
                ApplyOps = [RegOp.SetDword(LmWks, "MaxCollectionCount", 32)],
                RemoveOps = [RegOp.DeleteValue(LmWks, "MaxCollectionCount")],
                DetectOps = [RegOp.CheckDword(LmWks, "MaxCollectionCount", 32)],
            },
        ];
    }

    // ── SmbServerHardeningPolicy ──
    private static class _SmbServerHardeningPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanManServer\Parameters";
        private const string SrvKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters";
        private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "smbsvr-disable-smb-compression",
                    Label = "Disable SMBv3 Compression to Prevent SMBleed Attacks",
                    Category = "Network — Radius Auth",
                    Description =
                        "Disables SMB compression on the server, mitigating SMBleed (CVE-2020-1206) and similar compression-path vulnerabilities that can allow unauthenticated reading of uninitialized kernel memory through SMB3 compressed data.",
                    Tags = ["smb", "compression", "smbleed", "cve-2020-1206", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SMBv3 compression disabled; SMBleed class vulnerabilities mitigated. Minor performance impact on compressed transfers.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCompression", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCompression")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCompression", 1)],
                },
                new TweakDef
                {
                    Id = "smbsvr-enable-smb-encryption",
                    Label = "Enable SMBv3 Encryption for All Shares (Enforce in Transit)",
                    Category = "Network — Radius Auth",
                    Description =
                        "Enables SMBv3 end-to-end encryption for all SMB connections to this server, ensuring file transfer content is AES-encrypted in transit and cannot be captured in plaintext on the network.",
                    Tags = ["smb", "encryption", "aes", "in-transit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "SMBv3 encryption enforced; file data is AES-encrypted in transit. Requires Windows 8/2012 or later clients.",
                    ApplyOps = [RegOp.SetDword(Key, "EncryptData", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EncryptData")],
                    DetectOps = [RegOp.CheckDword(Key, "EncryptData", 1)],
                },
                new TweakDef
                {
                    Id = "smbsvr-disable-guest-fallback",
                    Label = "Disable SMB Guest Authentication Fallback",
                    Category = "Network — Radius Auth",
                    Description =
                        "Prevents the SMB client from automatically falling back to anonymous guest authentication when the provided credentials are rejected, stopping silent elevation-of-failure-to-anonymous-access on misconfigured shares.",
                    Tags = ["smb", "guest", "anonymous", "fallback", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "SMB guest auth fallback disabled; authentication failures are hard failures, not silent anonymous access.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "EnableInsecureGuestLogons", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "EnableInsecureGuestLogons")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "EnableInsecureGuestLogons", 0)],
                },
                new TweakDef
                {
                    Id = "smbsvr-log-auth-failures",
                    Label = "Log SMB Authentication Failure Events in Security Log",
                    Category = "Network — Radius Auth",
                    Description =
                        "Enables Security event log audit entries for failed SMB authentication attempts, providing visibility into brute-force attacks and pass-the-hash attempts against network shares.",
                    Tags = ["smb", "auth-failure", "event-log", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "SMB auth failure events logged in Security log; brute-force and pass-the-hash attempts visible.",
                    ApplyOps = [RegOp.SetDword(PolKey, "LogAuthFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "LogAuthFailures")],
                    DetectOps = [RegOp.CheckDword(PolKey, "LogAuthFailures", 1)],
                },
                new TweakDef
                {
                    Id = "smbsvr-disable-smb-telemetry",
                    Label = "Disable SMB Server Telemetry Reporting to Microsoft",
                    Category = "Network — Radius Auth",
                    Description =
                        "Prevents the SMB server from sending connection statistics, negotiated cipher suites, session rates, and protocol version telemetry to Microsoft.",
                    Tags = ["smb", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "SMB telemetry to Microsoft disabled; session rates and cipher negotiation data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableSMBTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableSMBTelemetry")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableSMBTelemetry", 1)],
                },
            ];
    }

    // ── SmbServerPolicy ──
    private static class _SmbServerPolicy
    {
        private const string SmbSrv = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "smbsrv-disable-admin-share-server",
                Label = "Disable Hidden Admin Shares (Server Mode)",
                Category = "Network — Radius Auth",
                Description =
                    "Sets AutoShareServer=0 in LanmanServer parameters. Prevents Windows from automatically creating the hidden administrative shares (C$, D$, ADMIN$, IPC$) on server-class installations when the computer starts. Reduces the exposed SMB attack surface on file server roles.",
                Tags = ["smb", "admin-share", "server", "security", "hardening"],
                NeedsAdmin = true,
                CorpSafe = false,
                ApplyOps = [RegOp.SetDword(SmbSrv, "AutoShareServer", 0)],
                RemoveOps = [RegOp.DeleteValue(SmbSrv, "AutoShareServer")],
                DetectOps = [RegOp.CheckDword(SmbSrv, "AutoShareServer", 0)],
            },
            new TweakDef
            {
                Id = "smbsrv-enable-raw-mode",
                Label = "Enable SMB Raw Read/Write Mode",
                Category = "Network — Radius Auth",
                Description =
                    "Sets EnableRaw=1 in LanmanServer parameters. Ensures the SMB server permits raw-mode transfers (large single-command reads and writes without the overhead of a separate setup packet). Raw mode is the default; restoring it if previously disabled improves LAN performance for large file copies.",
                Tags = ["smb", "raw", "performance", "server", "tuning"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmbSrv, "EnableRaw", 1)],
                RemoveOps = [RegOp.DeleteValue(SmbSrv, "EnableRaw")],
                DetectOps = [RegOp.CheckDword(SmbSrv, "EnableRaw", 1)],
            },
            new TweakDef
            {
                Id = "smbsrv-set-size-req-buf",
                Label = "Set SMB Server Request Buffer Size to 4356",
                Category = "Network — Radius Auth",
                Description =
                    "Sets SizReqBuf=4356 in LanmanServer parameters. Configures the raw-mode read buffer size for the SMB server. 4356 bytes aligns the buffer to a common Ethernet MTU boundary (4 KB + SMB header overhead), which can reduce fragmented TCP segments for raw SMB operations on Gigabit networks.",
                Tags = ["smb", "buffer", "performance", "server", "tuning"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmbSrv, "SizReqBuf", 4356)],
                RemoveOps = [RegOp.DeleteValue(SmbSrv, "SizReqBuf")],
                DetectOps = [RegOp.CheckDword(SmbSrv, "SizReqBuf", 4356)],
            },
            new TweakDef
            {
                Id = "smbsrv-disk-space-threshold",
                Label = "Require 10% Free Disk Before SMB Writes",
                Category = "Network — Radius Auth",
                Description =
                    "Sets DiskSpaceThreshold=10 in LanmanServer parameters. Instructs the SMB server to return a disk-full error to clients when the volume hosting a share has less than 10% free space remaining, rather than waiting until the volume is completely full. Prevents total disk exhaustion which can corrupt open files.",
                Tags = ["smb", "disk", "threshold", "server", "reliability"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmbSrv, "DiskSpaceThreshold", 10)],
                RemoveOps = [RegOp.DeleteValue(SmbSrv, "DiskSpaceThreshold")],
                DetectOps = [RegOp.CheckDword(SmbSrv, "DiskSpaceThreshold", 10)],
            },
        ];
    }

    // ── SnmpPolicy ──
    private static class _SnmpPolicy
    {
        private const string SnmpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SNMP\Parameters";
        private const string AgentKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SNMP\Parameters\ValidCommunities";
        private const string MgrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SNMP\Parameters\PermittedManagers";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "snmppol-enable-auth-traps",
                Label = "SNMP Policy: Enable Authentication Failure Traps",
                Category = "Network — Radius Auth",
                Description =
                    "Sends SNMP authentication failure traps when unauthorized community string requests are received. Enables monitoring of unauthorized SNMP access attempts.",
                Tags = ["snmp", "auth", "traps", "monitoring", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Sends traps on unauthorized SNMP community string requests.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "EnableAuthenticationTraps", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "EnableAuthenticationTraps")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "EnableAuthenticationTraps", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-restrict-permitted-managers",
                Label = "SNMP Policy: Restrict Permitted Management Hosts",
                Category = "Network — Radius Auth",
                Description =
                    "Enforces GPO-defined list of permitted SNMP management hosts. The SNMP service only responds to requests from the hosts listed under PermittedManagers registry key.",
                Tags = ["snmp", "access-control", "managers", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Restricts SNMP responses to localhost only; blocks all remote management hosts.",
                RegistryKeys = [MgrKey],
                ApplyOps = [RegOp.SetString(MgrKey, "1", "localhost")],
                RemoveOps = [RegOp.DeleteValue(MgrKey, "1")],
                DetectOps = [RegOp.CheckString(MgrKey, "1", "localhost")],
            },
            new TweakDef
            {
                Id = "snmppol-disable-community-readonly",
                Label = "SNMP Policy: Remove Default Public Read-Only Community",
                Category = "Network — Radius Auth",
                Description =
                    "Removes the 'public' SNMP community string from the valid communities list. The default public community string is a well-known attack vector that enables SNMP enumeration.",
                Tags = ["snmp", "community", "public", "hardening", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Removes the default 'public' community string — a well-known SNMP attack vector.",
                RegistryKeys = [AgentKey],
                ApplyOps = [RegOp.SetDword(AgentKey, "public", 0)],
                RemoveOps = [RegOp.DeleteValue(AgentKey, "public")],
                DetectOps = [RegOp.CheckDword(AgentKey, "public", 0)],
            },
            new TweakDef
            {
                Id = "snmppol-set-community-read-only",
                Label = "SNMP Policy: Restrict Community String Permissions (Read-Only)",
                Category = "Network — Radius Auth",
                Description =
                    "Sets the SNMP community string type to Read Only (4) and removes Write/Create/Delete rights. Prevents SNMP-based configuration changes from network management stations.",
                Tags = ["snmp", "community", "read-only", "permissions", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Restricts community string to read-only; prevents SNMP SET operations.",
                RegistryKeys = [AgentKey],
                ApplyOps = [RegOp.SetDword(AgentKey, "private", 4)],
                RemoveOps = [RegOp.DeleteValue(AgentKey, "private")],
                DetectOps = [RegOp.CheckDword(AgentKey, "private", 4)],
            },
            new TweakDef
            {
                Id = "snmppol-disable-snmp-writeable",
                Label = "SNMP Policy: Disable SNMP Write Community Access",
                Category = "Network — Radius Auth",
                Description =
                    "Sets the community permissions to None (1) for the write community, disabling any SNMP SET operations. SNMP write access allows remote configuration changes to network devices.",
                Tags = ["snmp", "write", "set-operations", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables SNMP write (SET) operations entirely.",
                RegistryKeys = [AgentKey],
                ApplyOps = [RegOp.SetDword(AgentKey, "write", 1)],
                RemoveOps = [RegOp.DeleteValue(AgentKey, "write")],
                DetectOps = [RegOp.CheckDword(AgentKey, "write", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-enable-snmp-service-policy",
                Label = "SNMP Policy: Enable SNMP Service Policy Enforcement",
                Category = "Network — Radius Auth",
                Description =
                    "Enables GPO-based enforcement of SNMP service settings. When enabled, all SNMP service configuration is governed by Group Policy, overriding local service settings.",
                Tags = ["snmp", "gpo", "enforcement", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables GPO enforcement of all SNMP service settings.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "EnforceSNMPPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "EnforceSNMPPolicy")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "EnforceSNMPPolicy", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-disable-snmp-v1",
                Label = "SNMP Policy: Disable SNMPv1 Protocol",
                Category = "Network — Radius Auth",
                Description =
                    "Disables SNMPv1 through GPO policy. SNMPv1 transmits community strings in plain text and lacks encryption or authentication. Disabling it forces use of SNMPv2c or SNMPv3.",
                Tags = ["snmp", "v1", "legacy", "protocol", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Disables plaintext SNMPv1 protocol; forces SNMPv2c or SNMPv3.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "DisableSNMPv1", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "DisableSNMPv1")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "DisableSNMPv1", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-log-auth-failures",
                Label = "SNMP Policy: Log Authentication Failures to Event Log",
                Category = "Network — Radius Auth",
                Description =
                    "Configures the SNMP service to write authentication failure events to the Windows Security event log. Supports Security Information and Event Management (SIEM) integration.",
                Tags = ["snmp", "logging", "event-log", "auth", "siem", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Writes SNMP auth failures to Windows Security event log for SIEM.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "LogAuthFailures", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "LogAuthFailures")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "LogAuthFailures", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-block-snmp-from-internet",
                Label = "SNMP Policy: Block SNMP from Public Network Access",
                Category = "Network — Radius Auth",
                Description =
                    "Restricts SNMP to internal network connections only through GPO. Forces the SNMP service to discard any requests arriving from non-private network interfaces.",
                Tags = ["snmp", "firewall", "network", "internet", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Restricts SNMP to internal network interfaces only.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "BlockPublicNetworkAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "BlockPublicNetworkAccess")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "BlockPublicNetworkAccess", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-restrict-trap-receivers",
                Label = "SNMP Policy: Restrict SNMP Trap Receivers to Known Hosts",
                Category = "Network — Radius Auth",
                Description =
                    "Applies GPO-enforced filtering to SNMP trap destinations, limiting trap broadcasts to administrator-approved management systems. Reduces SNMP trap amplification risk.",
                Tags = ["snmp", "traps", "trap-receivers", "network", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Limits SNMP trap destinations to approved management systems only.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "RestrictTrapReceivers", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "RestrictTrapReceivers")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "RestrictTrapReceivers", 1)],
            },
        ];
    }

    // ── SshHardening ──
    private static class _SshHardening
    {
        private const string SshdConfig = @"C:\ProgramData\ssh\sshd_config";

        // Helper: apply a directive in sshd_config (add or replace).
        private static void SetSshdDirective(string directive, string value, bool dryRun)
        {
            if (dryRun)
                return;
            if (!System.IO.File.Exists(SshdConfig))
                return;
            string line = $"{directive} {value}";
            var lines = System.IO.File.ReadAllLines(SshdConfig);
            bool found = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string trimmed = lines[i].TrimStart();
                if (
                    trimmed.StartsWith(directive, System.StringComparison.OrdinalIgnoreCase)
                    && (trimmed.Length == directive.Length || trimmed[directive.Length] == ' ')
                )
                {
                    lines[i] = line;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                System.Array.Resize(ref lines, lines.Length + 1);
                lines[^1] = line;
            }
            System.IO.File.WriteAllLines(SshdConfig, lines);
        }

        // Helper: remove / comment out a directive.
        private static void RemoveSshdDirective(string directive, bool dryRun)
        {
            if (dryRun || !System.IO.File.Exists(SshdConfig))
                return;
            var lines = System.IO.File.ReadAllLines(SshdConfig);
            for (int i = 0; i < lines.Length; i++)
            {
                string trimmed = lines[i].TrimStart();
                if (
                    trimmed.StartsWith(directive, System.StringComparison.OrdinalIgnoreCase)
                    && (trimmed.Length == directive.Length || trimmed[directive.Length] == ' ')
                )
                {
                    lines[i] = "#" + lines[i];
                    break;
                }
            }
            System.IO.File.WriteAllLines(SshdConfig, lines);
        }

        // Helper: detect a directive is set to the expected value.
        private static bool DetectSshdDirective(string directive, string expectedValue)
        {
            if (!System.IO.File.Exists(SshdConfig))
                return false;
            foreach (string raw in System.IO.File.ReadAllLines(SshdConfig))
            {
                string trimmed = raw.TrimStart();
                if (trimmed.StartsWith("#", System.StringComparison.Ordinal))
                    continue;
                if (
                    trimmed.StartsWith(directive, System.StringComparison.OrdinalIgnoreCase)
                    && trimmed.Length > directive.Length
                    && trimmed[directive.Length] == ' '
                )
                {
                    string actual = trimmed[(directive.Length + 1)..].Trim();
                    return string.Equals(actual, expectedValue, System.StringComparison.OrdinalIgnoreCase);
                }
            }
            return false;
        }

        private static bool SshdConfigExists() => System.IO.File.Exists(SshdConfig);

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ssh-max-auth-tries-3",
                Label = "Limit SSH Authentication Attempts to 3",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets MaxAuthTries 3 in sshd_config. Limits failed authentication attempts "
                    + "per connection to 3 before disconnecting, reducing brute-force window. Default: 6.",
                Tags = ["ssh", "authentication", "brute-force", "security", "hardening"],
                ApplyAction = dry => SetSshdDirective("MaxAuthTries", "3", dry),
                RemoveAction = dry => RemoveSshdDirective("MaxAuthTries", dry),
                DetectAction = () => DetectSshdDirective("MaxAuthTries", "3"),
            },
            new TweakDef
            {
                Id = "ssh-login-grace-time-30",
                Label = "SSH Login Grace Time 30 Seconds",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets LoginGraceTime 30 in sshd_config. The server disconnects if a user "
                    + "has not authenticated within 30 seconds, preventing half-open connection exhaustion attacks. Default: 120.",
                Tags = ["ssh", "timeout", "security", "hardening", "dos"],
                ApplyAction = dry => SetSshdDirective("LoginGraceTime", "30", dry),
                RemoveAction = dry => RemoveSshdDirective("LoginGraceTime", dry),
                DetectAction = () => DetectSshdDirective("LoginGraceTime", "30"),
            },
            new TweakDef
            {
                Id = "ssh-permit-empty-passwords-no",
                Label = "Disallow SSH Empty Password Logins",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets PermitEmptyPasswords no in sshd_config. Prevents accounts with blank passwords "
                    + "from authenticating via SSH. Default: no (safe), but explicitly enforced here.",
                Tags = ["ssh", "password", "authentication", "security"],
                ApplyAction = dry => SetSshdDirective("PermitEmptyPasswords", "no", dry),
                RemoveAction = dry => RemoveSshdDirective("PermitEmptyPasswords", dry),
                DetectAction = () => DetectSshdDirective("PermitEmptyPasswords", "no"),
            },
            new TweakDef
            {
                Id = "ssh-disable-agent-forwarding",
                Label = "Disable SSH Agent Forwarding",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets AllowAgentForwarding no in sshd_config. Prevents forwarding of the SSH authentication "
                    + "agent from remote hosts, which could allow lateral movement if a relay host is compromised. Default: yes.",
                Tags = ["ssh", "agent", "forwarding", "security", "lateral-movement"],
                ApplyAction = dry => SetSshdDirective("AllowAgentForwarding", "no", dry),
                RemoveAction = dry => RemoveSshdDirective("AllowAgentForwarding", dry),
                DetectAction = () => DetectSshdDirective("AllowAgentForwarding", "no"),
            },
            new TweakDef
            {
                Id = "ssh-disable-tcp-forwarding",
                Label = "Disable SSH TCP Forwarding",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets AllowTcpForwarding no in sshd_config. Prevents SSH tunnelling of TCP connections "
                    + "through this host, blocking use of SSH as a proxy or pivot point. Default: yes.",
                Tags = ["ssh", "tcp", "tunnel", "forwarding", "security"],
                ApplyAction = dry => SetSshdDirective("AllowTcpForwarding", "no", dry),
                RemoveAction = dry => RemoveSshdDirective("AllowTcpForwarding", dry),
                DetectAction = () => DetectSshdDirective("AllowTcpForwarding", "no"),
            },
            new TweakDef
            {
                Id = "ssh-max-sessions-2",
                Label = "Limit SSH Concurrent Sessions to 2",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = false,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets MaxSessions 2 in sshd_config. Caps multiplexed sessions per connection to 2, "
                    + "limiting resource exhaustion attacks. May need increasing for automation workflows. Default: 10.",
                Tags = ["ssh", "sessions", "dos", "security", "resource"],
                ApplyAction = dry => SetSshdDirective("MaxSessions", "2", dry),
                RemoveAction = dry => RemoveSshdDirective("MaxSessions", dry),
                DetectAction = () => DetectSshdDirective("MaxSessions", "2"),
            },
            new TweakDef
            {
                Id = "ssh-strict-modes",
                Label = "Enable SSH StrictModes",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets StrictModes yes in sshd_config. Forces SSH to check file and directory permissions "
                    + "before accepting logins. Rejects login if home directory or authorized_keys are world-writable. Default: yes.",
                Tags = ["ssh", "permissions", "strictmodes", "security"],
                ApplyAction = dry => SetSshdDirective("StrictModes", "yes", dry),
                RemoveAction = dry => RemoveSshdDirective("StrictModes", dry),
                DetectAction = () => DetectSshdDirective("StrictModes", "yes"),
            },
            new TweakDef
            {
                Id = "ssh-disable-x11-forwarding",
                Label = "Disable SSH X11 Forwarding",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets X11Forwarding no in sshd_config. X11 forwarding is irrelevant on Windows "
                    + "and expands the attack surface by creating X11 proxy connections. Default: no on Windows.",
                Tags = ["ssh", "x11", "forwarding", "security", "attack-surface"],
                ApplyAction = dry => SetSshdDirective("X11Forwarding", "no", dry),
                RemoveAction = dry => RemoveSshdDirective("X11Forwarding", dry),
                DetectAction = () => DetectSshdDirective("X11Forwarding", "no"),
            },
            new TweakDef
            {
                Id = "ssh-set-strong-ciphers",
                Label = "Restrict SSH to Strong Ciphers",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets Ciphers to AES-256 in CTR/GCM modes only in sshd_config. "
                    + "Removes weak ciphers (3DES, RC4, AES-128-CBC) from the negotiation list. "
                    + "Default: broad cipher list. Recommended: AES-256-GCM and AES-256-CTR only.",
                Tags = ["ssh", "cipher", "encryption", "security", "hardening"],
                ApplyAction = dry => SetSshdDirective("Ciphers", "aes256-gcm@openssh.com,aes256-ctr", dry),
                RemoveAction = dry => RemoveSshdDirective("Ciphers", dry),
                DetectAction = () => DetectSshdDirective("Ciphers", "aes256-gcm@openssh.com,aes256-ctr"),
            },
            new TweakDef
            {
                Id = "ssh-set-strong-macs",
                Label = "Restrict SSH to Strong MAC Algorithms",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets MACs to HMAC-SHA2-512 and HMAC-SHA2-256 in sshd_config. "
                    + "Removes weak MACs (MD5, SHA1) from negotiation. "
                    + "Default: broad MAC list including SHA1. Recommended: SHA-256/SHA-512 only.",
                Tags = ["ssh", "mac", "hmac", "encryption", "security"],
                ApplyAction = dry => SetSshdDirective("MACs", "hmac-sha2-512,hmac-sha2-256", dry),
                RemoveAction = dry => RemoveSshdDirective("MACs", dry),
                DetectAction = () => DetectSshdDirective("MACs", "hmac-sha2-512,hmac-sha2-256"),
            },
        ];
    }

    // ── VoipQualityPolicy ──
    private static class _VoipQualityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "voipqos-set-teams-audio-dscp-value",
                    Label = "VoIP QoS: Mark Teams Audio RTP with DSCP EF (46)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AudioDscpValue=46 in Teams QoS policy. Instructs Teams to mark all real-time audio RTP packets with DSCP EF (Expedited Forwarding = 46, the highest priority class). "
                        + "On enterprise networks with QoS-aware switches and routers, EF-marked packets receive the smallest queuing delay and lowest drop probability, which directly reduces jitter and one-way latency in Teams calls. "
                        + "This setting is distinct from the generic Windows QoS multimedia scheduling rate and applies specifically to the Teams media engine RTP streams.",
                    Tags = ["teams", "voip", "qos", "dscp", "audio", "rtp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Marks Teams audio RTP with EF DSCP 46; critical for low-latency voice on congested enterprise networks.",
                    ApplyOps = [RegOp.SetDword(Key, "AudioDscpValue", 46)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AudioDscpValue")],
                    DetectOps = [RegOp.CheckDword(Key, "AudioDscpValue", 46)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-video-dscp-value",
                    Label = "VoIP QoS: Mark Teams Video RTP with DSCP AF41 (34)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets VideoDscpValue=34 in Teams QoS policy. Marks Teams video RTP packets with DSCP AF41 (Assured Forwarding 41 = 34). "
                        + "AF41 is the IETF recommendation for interactive video conferencing traffic. It receives higher priority than best-effort but is de-prioritised below EF (audio). "
                        + "Separating audio (EF) and video (AF41) ensures audio is never starved by high-bitrate video bursts during congestion.",
                    Tags = ["teams", "voip", "qos", "dscp", "video", "rtp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Marks Teams video with AF41 DSCP 34; prevents video bursts from starving audio on saturated links.",
                    ApplyOps = [RegOp.SetDword(Key, "VideoDscpValue", 34)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VideoDscpValue")],
                    DetectOps = [RegOp.CheckDword(Key, "VideoDscpValue", 34)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-appshar-dscp-value",
                    Label = "VoIP QoS: Mark Teams App-Sharing with DSCP AF21 (18)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AppShareDscpValue=18 in Teams QoS policy. Marks Teams application-sharing and desktop-sharing RTP streams with DSCP AF21 (Assured Forwarding 21 = 18). "
                        + "Screen share generates large and bursty traffic which should be deprioritised relative to live audio and video to prevent real-time media degredation during presentations.",
                    Tags = ["teams", "voip", "qos", "dscp", "screenshare", "rtp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Marks app-sharing with AF21 DSCP 18; prevents screen share bursts from degrading audio/video quality.",
                    ApplyOps = [RegOp.SetDword(Key, "AppShareDscpValue", 18)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AppShareDscpValue")],
                    DetectOps = [RegOp.CheckDword(Key, "AppShareDscpValue", 18)],
                },
                new TweakDef
                {
                    Id = "voipqos-enable-teams-audio-port-range",
                    Label = "VoIP QoS: Enable Teams-Specific Audio UDP Port Range",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AudioPortsEnabled=1 in Teams QoS policy. Enables the use of a dedicated UDP port range for Teams audio media. "
                        + "Port-based QoS rules on network switches and firewalls can then classify and prioritise Teams audio traffic from these specific ports rather than relying solely on DSCP markings, which are sometimes stripped by ISPs.",
                    Tags = ["teams", "voip", "qos", "ports", "udp", "audio"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables fixed port range for Teams audio; allows port-based QoS classification in addition to DSCP.",
                    ApplyOps = [RegOp.SetDword(Key, "AudioPortsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AudioPortsEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "AudioPortsEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-audio-port-start-50000",
                    Label = "VoIP QoS: Set Teams Audio Port Range Start to 50000",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AudioPortStart=50000 in Teams QoS policy. Configures the start of the UDP port range used by Teams audio media to port 50000. "
                        + "This port base aligns with the Microsoft-recommended range for Teams voice and allows network administrators to create firewall ACLs and QoS policies targeting the well-known 50000–50019 range.",
                    Tags = ["teams", "voip", "qos", "ports", "audio", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Sets audio port range start to 50000 per Microsoft recommendation; enables precise firewall and QoS rules.",
                    ApplyOps = [RegOp.SetDword(Key, "AudioPortStart", 50000)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AudioPortStart")],
                    DetectOps = [RegOp.CheckDword(Key, "AudioPortStart", 50000)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-audio-port-count-20",
                    Label = "VoIP QoS: Set Teams Audio Port Count to 20",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AudioPortCount=20 in Teams QoS policy. Allocates 20 consecutive UDP ports for Teams audio media starting from AudioPortStart. "
                        + "A count of 20 provides enough ports for simultaneous call sessions on a single machine while keeping the range narrow enough for precise firewall and QoS ACL rules.",
                    Tags = ["teams", "voip", "qos", "ports", "audio", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Allocates 20 UDP ports for Teams audio; balances multi-session capacity with narrow QoS rule precision.",
                    ApplyOps = [RegOp.SetDword(Key, "AudioPortCount", 20)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AudioPortCount")],
                    DetectOps = [RegOp.CheckDword(Key, "AudioPortCount", 20)],
                },
                new TweakDef
                {
                    Id = "voipqos-enable-teams-video-port-range",
                    Label = "VoIP QoS: Enable Teams-Specific Video UDP Port Range",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets VideoPortsEnabled=1 in Teams QoS policy. Enables the use of a dedicated UDP port range for Teams video media streams. "
                        + "Separating video on its own port range allows network equipment to apply different QoS policies to audio and video independently, which is important when network bandwidth needs to preferentially protect audio quality over video.",
                    Tags = ["teams", "voip", "qos", "ports", "video", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables dedicated video port range; allows separate QoS treatment of audio versus video streams.",
                    ApplyOps = [RegOp.SetDword(Key, "VideoPortsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VideoPortsEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "VideoPortsEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-video-port-start-50020",
                    Label = "VoIP QoS: Set Teams Video Port Range Start to 50020",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets VideoPortStart=50020 in Teams QoS policy. Sets the starting UDP port for Teams video media to 50020, immediately following the audio port range (50000–50019). "
                        + "This layout allows a single contiguous firewall rule (50000–50039) to cover both audio and video, while still allowing separate DSCP markings to be applied per-range.",
                    Tags = ["teams", "voip", "qos", "ports", "video", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Sets video port start to 50020; aligns with audio range for manageable firewall rule design.",
                    ApplyOps = [RegOp.SetDword(Key, "VideoPortStart", 50020)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VideoPortStart")],
                    DetectOps = [RegOp.CheckDword(Key, "VideoPortStart", 50020)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-video-port-count-20",
                    Label = "VoIP QoS: Set Teams Video Port Count to 20",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets VideoPortCount=20 in Teams QoS policy. Allocates 20 UDP ports for Teams video media starting at VideoPortStart. "
                        + "20 ports accommodates multiple simultaneous video sessions and gallery view scenarios without creating an overly broad firewall footprint.",
                    Tags = ["teams", "voip", "qos", "ports", "video", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Allocates 20 UDP ports for Teams video; supports gallery view with a narrow, manageable port range.",
                    ApplyOps = [RegOp.SetDword(Key, "VideoPortCount", 20)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VideoPortCount")],
                    DetectOps = [RegOp.CheckDword(Key, "VideoPortCount", 20)],
                },
                new TweakDef
                {
                    Id = "voipqos-enable-teams-appshar-port-range",
                    Label = "VoIP QoS: Enable Teams App-Sharing UDP Port Range",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AppSharePortsEnabled=1 in Teams QoS policy. Enables a dedicated UDP port range for Teams application-sharing and desktop-sharing media streams. "
                        + "Isolating app-sharing on its own port range allows network QoS policies to apply lower priority scheduling to screen share traffic while still guaranteeing audio and video delivery during congestion.",
                    Tags = ["teams", "voip", "qos", "ports", "screenshare", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables dedicated app-sharing port range; decouple screen share QoS from audio/video port policies.",
                    ApplyOps = [RegOp.SetDword(Key, "AppSharePortsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AppSharePortsEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "AppSharePortsEnabled", 1)],
                },
            ];
    }

    // ── VpnRemoteAccessPolicy ──
    private static class _VpnRemoteAccessPolicy
    {
        private const string RasKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess";
        private const string IkeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess\IKEv2";
        private const string ConnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess\Config";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "vpnras-require-strong-encryption",
                    Label = "Require Strong Encryption for VPN Connections",
                    Category = "Network — Voip Quality",
                    Description =
                        "Enforces maximum-strength encryption (MPPE 128-bit or AES-256) for all RRAS VPN connections. Rejects connections that negotiate weaker ciphers. Default: optional encryption.",
                    Tags = ["vpn", "encryption", "rras", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "VPN connections must use strong encryption; clients with weak cipher support will fail to connect.",
                    ApplyOps = [RegOp.SetDword(RasKey, "RequireStrongEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "RequireStrongEncryption")],
                    DetectOps = [RegOp.CheckDword(RasKey, "RequireStrongEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-disable-pptp-protocol",
                    Label = "Disable PPTP VPN Protocol",
                    Category = "Network — Voip Quality",
                    Description =
                        "Disables the insecure PPTP (Point-to-Point Tunneling Protocol) for VPN connections. PPTP is considered cryptographically broken. Default: enabled.",
                    Tags = ["vpn", "pptp", "security", "deprecated", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "PPTP connections blocked; legacy clients relying on PPTP must migrate to IKEv2/L2TP.",
                    ApplyOps = [RegOp.SetDword(RasKey, "DisablePPTP", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "DisablePPTP")],
                    DetectOps = [RegOp.CheckDword(RasKey, "DisablePPTP", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-enable-ikev2-preferred",
                    Label = "Set IKEv2 as Preferred VPN Protocol",
                    Category = "Network — Voip Quality",
                    Description =
                        "Configures RRAS to prefer IKEv2 (Internet Key Exchange v2) for VPN tunnel negotiation. IKEv2 supports MOBIKE for seamless roaming. Default: automatic protocol selection.",
                    Tags = ["vpn", "ikev2", "protocol", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "IKEv2 preferred for new connections; fallback to L2TP/SSTP if IKEv2 unavailable.",
                    ApplyOps = [RegOp.SetDword(IkeKey, "PreferIKEv2", 1)],
                    RemoveOps = [RegOp.DeleteValue(IkeKey, "PreferIKEv2")],
                    DetectOps = [RegOp.CheckDword(IkeKey, "PreferIKEv2", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-set-idle-timeout",
                    Label = "Set VPN Idle Disconnect Timeout to 30 Minutes",
                    Category = "Network — Voip Quality",
                    Description =
                        "Automatically disconnects inactive VPN sessions after 30 minutes of idle time. Frees up VPN server resources. Default: no idle timeout.",
                    Tags = ["vpn", "idle", "timeout", "rras", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Idle VPN sessions dropped after 30 minutes; users reconnect on next activity.",
                    ApplyOps = [RegOp.SetDword(ConnKey, "IdleDisconnectTimeout", 30)],
                    RemoveOps = [RegOp.DeleteValue(ConnKey, "IdleDisconnectTimeout")],
                    DetectOps = [RegOp.CheckDword(ConnKey, "IdleDisconnectTimeout", 30)],
                },
                new TweakDef
                {
                    Id = "vpnras-set-max-sessions",
                    Label = "Set Maximum Concurrent VPN Sessions to 100",
                    Category = "Network — Voip Quality",
                    Description =
                        "Limits the maximum number of concurrent VPN connections to the RRAS server to 100. Prevents resource exhaustion from excessive connections. Default: unlimited.",
                    Tags = ["vpn", "sessions", "limit", "rras", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Connection limit prevents server overload; users beyond 100 are queued or rejected.",
                    ApplyOps = [RegOp.SetDword(ConnKey, "MaxSessions", 100)],
                    RemoveOps = [RegOp.DeleteValue(ConnKey, "MaxSessions")],
                    DetectOps = [RegOp.CheckDword(ConnKey, "MaxSessions", 100)],
                },
                new TweakDef
                {
                    Id = "vpnras-enable-connection-logging",
                    Label = "Enable VPN Connection Audit Logging",
                    Category = "Network — Voip Quality",
                    Description =
                        "Enables audit logging for all VPN connection attempts (successful and failed). Logs are written to the Windows Security event log. Default: disabled.",
                    Tags = ["vpn", "logging", "audit", "security", "rras", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All VPN connection events logged for audit; slight increase in event log volume.",
                    ApplyOps = [RegOp.SetDword(RasKey, "EnableConnectionLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "EnableConnectionLogging")],
                    DetectOps = [RegOp.CheckDword(RasKey, "EnableConnectionLogging", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-disable-split-tunneling",
                    Label = "Disable VPN Split Tunneling",
                    Category = "Network — Voip Quality",
                    Description =
                        "Forces all client traffic through the VPN tunnel (full-tunnel mode). Prevents clients from accessing the internet directly while connected. Default: split tunneling allowed.",
                    Tags = ["vpn", "split-tunnel", "security", "rras", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "All traffic routed through VPN; increases VPN server bandwidth but eliminates bypass risk.",
                    ApplyOps = [RegOp.SetDword(RasKey, "DisableSplitTunnel", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "DisableSplitTunnel")],
                    DetectOps = [RegOp.CheckDword(RasKey, "DisableSplitTunnel", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-set-sa-lifetime",
                    Label = "Set IKEv2 SA Lifetime to 8 Hours",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets the IKEv2 security association (SA) lifetime to 8 hours (480 minutes). After expiry, the tunnel renegotiates keys. Default: 8 hours (may vary).",
                    Tags = ["vpn", "ikev2", "sa-lifetime", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "VPN tunnel renegotiates keys every 8 hours; brief reconnection on rekey.",
                    ApplyOps = [RegOp.SetDword(IkeKey, "SALifeTimeMinutes", 480)],
                    RemoveOps = [RegOp.DeleteValue(IkeKey, "SALifeTimeMinutes")],
                    DetectOps = [RegOp.CheckDword(IkeKey, "SALifeTimeMinutes", 480)],
                },
                new TweakDef
                {
                    Id = "vpnras-enable-nap-enforcement",
                    Label = "Enable Network Access Protection for VPN",
                    Category = "Network — Voip Quality",
                    Description =
                        "Enables NAP (Network Access Protection) health checks for VPN clients. Clients must meet health requirements (AV, firewall, updates) before being granted full access. Default: no NAP enforcement.",
                    Tags = ["vpn", "nap", "health-check", "security", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "VPN clients undergo health validation; non-compliant devices get restricted access.",
                    ApplyOps = [RegOp.SetDword(RasKey, "EnableNAP", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "EnableNAP")],
                    DetectOps = [RegOp.CheckDword(RasKey, "EnableNAP", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-disable-saved-credentials",
                    Label = "Prevent Saving VPN Credentials",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents users from saving VPN connection credentials. Users must enter credentials each time they connect. Reduces credential theft risk. Default: saving allowed.",
                    Tags = ["vpn", "credentials", "security", "credential-theft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Users re-enter VPN credentials each session; reduces risk of stored credential theft.",
                    ApplyOps = [RegOp.SetDword(RasKey, "DisableSavedCredentials", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "DisableSavedCredentials")],
                    DetectOps = [RegOp.CheckDword(RasKey, "DisableSavedCredentials", 1)],
                },
            ];
    }

    // ── WcmConnectionPolicy ──
    private static class _WcmConnectionPolicy
    {
        private const string Wcm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wcmpol-disable-auto-connect",
                Label = "Disable WCM Auto-Connect to Non-Internet Networks",
                Category = "Network — Voip Quality",
                Description =
                    "Disables Windows Connection Manager automatic connection to networks when already connected to internet. Prevents unexpected Wi-Fi/mobile broadband connections that could create dual-homed exposure. Default: 0. Recommended: 1.",
                Tags = ["connection-manager", "network", "auto-connect", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fDisableAutoConnect", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fDisableAutoConnect")],
                DetectOps = [RegOp.CheckDword(Wcm, "fDisableAutoConnect", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-minimize-connections",
                Label = "Minimize Simultaneous WCM Connections",
                Category = "Network — Voip Quality",
                Description =
                    "Instructs Windows Connection Manager to minimize the number of simultaneous connections to the internet, a domain, or a network. Prevents multi-homing unless required. Default: 0. Recommended: 3 (minimize, but allow manual overrides).",
                Tags = ["connection-manager", "network", "multi-home"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fMinimizeConnections", 3)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fMinimizeConnections")],
                DetectOps = [RegOp.CheckDword(Wcm, "fMinimizeConnections", 3)],
            },
            new TweakDef
            {
                Id = "wcmpol-prefer-wired-network",
                Label = "Prefer Wired over Wireless in WCM",
                Category = "Network — Voip Quality",
                Description =
                    "Instructs Windows Connection Manager to prefer wired Ethernet connections over Wi-Fi when both are available. Improves stability and throughput without forcing disconnect from Wi-Fi. Default: 0. Recommended: 1.",
                Tags = ["connection-manager", "network", "wired", "wifi"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fPreferWiredNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fPreferWiredNetwork")],
                DetectOps = [RegOp.CheckDword(Wcm, "fPreferWiredNetwork", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-soft-disconnect",
                Label = "Enable WCM Soft Disconnect on Wireless",
                Category = "Network — Voip Quality",
                Description =
                    "Enables soft-disconnect behavior in WCM: instead of immediately dropping a wireless connection, the system waits for applications to switch before disconnecting. Reduces connection-drop disruptions. Default: 0. Recommended: 1.",
                Tags = ["connection-manager", "network", "wifi", "disconnect"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fSoftDisconnectConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fSoftDisconnectConnections")],
                DetectOps = [RegOp.CheckDword(Wcm, "fSoftDisconnectConnections", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-disable-wlan-connectivity",
                Label = "Disable WLAN Connectivity via WCM Policy",
                Category = "Network — Voip Quality",
                Description =
                    "Disables WLAN (Wi-Fi) connections through the Windows Connection Manager policy. For wired-only or air-gapped workstations where wireless should be locked out at policy level. Default: 0. Recommended: 1 for restricted machines.",
                Tags = ["connection-manager", "wifi", "disable", "security", "wlan"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fDisableWlanConnectivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fDisableWlanConnectivity")],
                DetectOps = [RegOp.CheckDword(Wcm, "fDisableWlanConnectivity", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-disable-wwan-connectivity",
                Label = "Disable WWAN/Mobile Broadband via WCM Policy",
                Category = "Network — Voip Quality",
                Description =
                    "Disables WWAN (mobile broadband/cellular) connections through the Windows Connection Manager policy. Prevents unexpected cellular data charges on enterprise devices. Default: 0. Recommended: 1 for non-mobile workstations.",
                Tags = ["connection-manager", "wwan", "mobile", "disable", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fDisableWwanConnectivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fDisableWwanConnectivity")],
                DetectOps = [RegOp.CheckDword(Wcm, "fDisableWwanConnectivity", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-access-restrictions-on-reconnect",
                Label = "Apply WCM Access Restrictions on Reconnect",
                Category = "Network — Voip Quality",
                Description =
                    "Re-applies WCM connection-policy access restrictions when a managed network reconnects after being temporarily unavailable. Ensures policy enforcement is not bypassed by reconnection events. Default: 0. Recommended: 1.",
                Tags = ["connection-manager", "network", "policy", "reconnect"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fApplyAccessRestrictionsOnReconnect", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fApplyAccessRestrictionsOnReconnect")],
                DetectOps = [RegOp.CheckDword(Wcm, "fApplyAccessRestrictionsOnReconnect", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-block-wifi-when-ethernet",
                Label = "Block Wi-Fi When Ethernet Connected via WCM",
                Category = "Network — Voip Quality",
                Description =
                    "Prevents Windows from maintaining active Wi-Fi connections when a wired Ethernet connection is available. Reduces dual-homed exposure and possible split-tunnel routing issues. Default: not set. Recommended: 1.",
                Tags = ["connection-manager", "wifi", "ethernet", "network", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fDisableConnectivityForEthernet", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fDisableConnectivityForEthernet")],
                DetectOps = [RegOp.CheckDword(Wcm, "fDisableConnectivityForEthernet", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-no-local-policy-merge",
                Label = "Prevent Local WCM Policy Merge",
                Category = "Network — Voip Quality",
                Description =
                    "Prevents local administrator-configured WCM policies from being merged with domain Group Policy settings for WCM. Ensures only domain policy governs connection management. Default: 0. Recommended: 1 for managed environments.",
                Tags = ["connection-manager", "network", "group-policy", "management"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fBlockLocalPolicyMerge", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fBlockLocalPolicyMerge")],
                DetectOps = [RegOp.CheckDword(Wcm, "fBlockLocalPolicyMerge", 1)],
            },
        ];
    }

    // ── WcmWifiPolicy ──
    private static class _WcmWifiPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wcmpol-disable-soft-disconnect",
                    Label = "Disable WCM Soft-Disconnect from Wired",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents WCM from softly disconnecting from wired Ethernet when a preferred Wi-Fi connection becomes available. Keeps wired connections stable and avoids unexpected bandwidth switches. Default: soft-disconnect enabled. Recommended: 1 on workstations.",
                    Tags = ["wcm", "wifi", "wired", "disconnect", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Wired connections stay active; WCM will not automatically switch to Wi-Fi when a preferred Wi-Fi is detected.",
                    ApplyOps = [RegOp.SetDword(Key, "fSoftDisconnectConnections", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fSoftDisconnectConnections")],
                    DetectOps = [RegOp.CheckDword(Key, "fSoftDisconnectConnections", 0)],
                },
                new TweakDef
                {
                    Id = "wcmpol-disable-simultaneous-connections",
                    Label = "Disable Simultaneous Wired+Wi-Fi Connections",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents Windows from maintaining simultaneous wired and Wi-Fi connections. When both are active, WCM disconnects the lower-priority adapter. Reduces split-routing and unintended traffic leakage. Default: simultaneous allowed. Recommended: 1.",
                    Tags = ["wcm", "wifi", "wired", "simultaneous", "routing", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Only one connection type (wired or Wi-Fi) is active at a time; eliminates multi-homed routing confusion.",
                    ApplyOps = [RegOp.SetDword(Key, "fMinimizeConnections", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fMinimizeConnections")],
                    DetectOps = [RegOp.CheckDword(Key, "fMinimizeConnections", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-disable-wifi-hotspot-auto",
                    Label = "Disable Auto-Connect to Wi-Fi Hotspots",
                    Category = "Network — Voip Quality",
                    Description =
                        "Stops WCM from automatically connecting to Wi-Fi hotspots (e.g., paid hotspots, Wi-Fi Sense networks). Prevents unexpected connections to unvetted open networks. Default: auto-connect enabled. Recommended: 1.",
                    Tags = ["wcm", "wifi", "hotspot", "auto-connect", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Windows will not automatically join Wi-Fi hotspot networks; users must select manually.",
                    ApplyOps = [RegOp.SetDword(Key, "fBlockHotspotAutoConnect", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fBlockHotspotAutoConnect")],
                    DetectOps = [RegOp.CheckDword(Key, "fBlockHotspotAutoConnect", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-allow-manual-wifi-connect",
                    Label = "Allow Manual Wi-Fi Connection Despite Wired",
                    Category = "Network — Voip Quality",
                    Description =
                        "Permits users to manually connect to a Wi-Fi network even when an active wired Ethernet connection exists. Allows intentional dual-homing when needed (e.g., out-of-band management). Default: restricted. Recommended: 1 for power users.",
                    Tags = ["wcm", "wifi", "manual", "wired", "dual-home", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Users can manually join Wi-Fi while wired; WCM will not block the manual connection.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowManualConnectionWhileWiredConnected", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowManualConnectionWhileWiredConnected")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowManualConnectionWhileWiredConnected", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-preferred-wired-over-wifi",
                    Label = "Prefer Wired Connection Over Wi-Fi (Priority)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Configures WCM to always prefer wired Ethernet over Wi-Fi when both are available. Wired connections are prioritized in routing tables. Default: WCM balances based on cost and speed. Recommended: 1 for desktop workstations.",
                    Tags = ["wcm", "wired", "priority", "routing", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Wired adapter routes are preferred; Wi-Fi remains as fallback only.",
                    ApplyOps = [RegOp.SetDword(Key, "PreferWiredOverWireless", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreferWiredOverWireless")],
                    DetectOps = [RegOp.CheckDword(Key, "PreferWiredOverWireless", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-disable-cellular-as-fallback",
                    Label = "Disable Cellular as Wi-Fi Fallback",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents WCM from switching to a cellular data connection when Wi-Fi or wired connections become unavailable. Avoids unexpected mobile data consumption when tethered. Default: cellular allowed as fallback. Recommended: 1 on Wi-Fi-only policies.",
                    Tags = ["wcm", "cellular", "fallback", "mobile", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Cellular data is never used as a fallback; connectivity drops if Wi-Fi/wired fail.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCellularFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCellularFallback")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCellularFallback", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-block-non-domain-connections",
                    Label = "Block Non-Domain Network Connections on Domain Endpoints",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents domain-joined machines from connecting to non-domain (public/home) networks while connected to the corporate domain network. Stops traffic leakage to unmanaged networks. Default: not restricted. Recommended: 1 on domain endpoints.",
                    Tags = ["wcm", "domain", "network", "security", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Domain-joined machines cannot join public/home Wi-Fi while on the corporate network; strong defence against bridging attacks.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockNonDomainNetworks", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockNonDomainNetworks")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockNonDomainNetworks", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-auto-select-network-profile",
                    Label = "Disable Auto-Selection of Network Profile on Connect",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents WCM from automatically selecting the best network profile (Public/Private/Domain) upon connection. Requires users to explicitly choose the profile, reducing risk of miscategorising corporate networks as Public. Default: auto-select enabled.",
                    Tags = ["wcm", "network-profile", "public", "private", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Network profile is not auto-assigned; reduces risk of corporate network being set to Public with open file sharing.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoNetworkProfileSelection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoNetworkProfileSelection")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoNetworkProfileSelection", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-set-polling-interval-60s",
                    Label = "Set WCM Connection Polling Interval to 60 Seconds",
                    Category = "Network — Voip Quality",
                    Description =
                        "Adjusts the WCM service polling interval for connectivity changes to 60 seconds. Reduces WCM CPU wakeups on battery-powered laptops without significantly delaying reconnection. Default: ~5 seconds. Recommended: 60 for battery savings.",
                    Tags = ["wcm", "polling", "battery", "performance", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "WCM polls every 60s instead of ~5s; reduces wakeups and battery drain; reconnect after network switch takes up to 60s.",
                    ApplyOps = [RegOp.SetDword(Key, "ConnectionPollingIntervalSec", 60)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConnectionPollingIntervalSec")],
                    DetectOps = [RegOp.CheckDword(Key, "ConnectionPollingIntervalSec", 60)],
                },
                new TweakDef
                {
                    Id = "wcmpol-disable-managed-wifi-offload",
                    Label = "Disable WCM Managed Wi-Fi Offload",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents WCM from offloading Wi-Fi management to a cellular companion device or managed Wi-Fi radio. Keeps all connection decisions on the primary Windows networking stack. Default: offload allowed. Recommended: 1 on standard hardware.",
                    Tags = ["wcm", "wifi", "offload", "managed", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi management stays on Windows networking stack; no offloading to companion devices.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableManagedWifiOffload", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableManagedWifiOffload")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableManagedWifiOffload", 1)],
                },
            ];
    }

    // ── WebProxyAutoDiscoveryPolicy ──
    private static class _WebProxyAutoDiscoveryPolicy
    {
        private const string InetKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";

        private const string WpadKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Wpad";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wpad-disable-auto-detect",
                    Label = "WPAD: Disable Automatic Proxy Detection (WPAD Protocol)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AutoDetect=0 in WPAD policy. Disables Web Proxy Auto-Discovery Protocol (WPAD) which broadcasts DHCP/DNS queries to discover proxy configuration servers on the local network. WPAD is exploited in PoisonTap and similar attacks where an attacker's rogue DHCP or DNS server responds to WPAD queries, redirecting all HTTP/HTTPS traffic through an attacker-controlled proxy. Disabling WPAD and using explicit PAC file URLs or manual proxy configuration eliminates this attack surface.",
                    Tags = ["wpad", "proxy", "auto-detect", "security", "mitm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Disables WPAD. Proxy configuration must be supplied via PAC file URL, manual proxy settings, or Group Policy. Breaks environments relying on WPAD for zero-config proxy discovery.",
                    ApplyOps = [RegOp.SetDword(WpadKey, "WpadOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(WpadKey, "WpadOverride")],
                    DetectOps = [RegOp.CheckDword(WpadKey, "WpadOverride", 1)],
                },
                new TweakDef
                {
                    Id = "wpad-disable-pac-script-download-prompt",
                    Label = "WPAD: Suppress PAC File Download Confirmation Prompt",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets DisableProxyAutoConfigUrlRequest=0 in Internet Settings. Suppresses the Internet Explorer / WinINet PAC file download confirmation prompt that asks users to allow or deny the download. In enterprise proxy environments, the PAC file is a managed IT component; user confirmation prompts are unnecessary and cause initial connection delays. This setting prevents the prompt and allows PAC file auto-download without user interaction.",
                    Tags = ["wpad", "pac", "prompt", "enterprise", "ux"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "PAC file is downloaded silently. Security benefit of the prompt is marginal since PAC URL comes from Group Policy in managed environments.",
                    ApplyOps = [RegOp.SetDword(InetKey, "DisableProxyAutoConfigUrlRequest", 0)],
                    RemoveOps = [RegOp.DeleteValue(InetKey, "DisableProxyAutoConfigUrlRequest")],
                    DetectOps = [RegOp.CheckDword(InetKey, "DisableProxyAutoConfigUrlRequest", 0)],
                },
                new TweakDef
                {
                    Id = "wpad-enable-auto-configuration",
                    Label = "WPAD: Enable Automatic Configuration Script (PAC) Support",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AutoConfigUrl=1 in Internet Settings policy. Enables enforced application of an automatic configuration script (PAC file) URL from Group Policy. This ensures the PAC file URL is deployed to all managed workstations and cannot be overridden by end users. Managed PAC enforcement is the standard enterprise proxy deployment mechanism: all applications using the WinHTTP/WinINet stack will use the centrally managed PAC file for proxy decisions.",
                    Tags = ["wpad", "pac", "auto-config", "proxy", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enables PAC file enforcement. The PAC URL must be separately configured via the Proxy GPO. This setting only enables the PAC mechanism, not a specific URL.",
                    ApplyOps = [RegOp.SetDword(InetKey, "AutoConfigUrl", 1)],
                    RemoveOps = [RegOp.DeleteValue(InetKey, "AutoConfigUrl")],
                    DetectOps = [RegOp.CheckDword(InetKey, "AutoConfigUrl", 1)],
                },
                new TweakDef
                {
                    Id = "wpad-enable-winhttp-proxy",
                    Label = "WPAD: Enable WinHTTP Proxy Inheritance from IE Settings",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets EnableLegacyAutoProxyFeatures=1 in Internet Settings. Enables WinHTTP applications (background services, .NET, PowerShell, Windows Update) to inherit the proxy configuration from the IE/WinINet machine proxy settings. Without this setting, WinHTTP applications (which don't read from the IE proxy registry directly) may bypass the corporate proxy entirely. Enabling inheritance ensures background system processes also route through the corporate proxy.",
                    Tags = ["wpad", "winhttp", "proxy", "inheritance", "background"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "WinHTTP services will use the corporate proxy. Applications with hardcoded direct access (e.g., Windows Update might bypass proxy) are unaffected.",
                    ApplyOps = [RegOp.SetDword(InetKey, "EnableLegacyAutoProxyFeatures", 1)],
                    RemoveOps = [RegOp.DeleteValue(InetKey, "EnableLegacyAutoProxyFeatures")],
                    DetectOps = [RegOp.CheckDword(InetKey, "EnableLegacyAutoProxyFeatures", 1)],
                },
                new TweakDef
                {
                    Id = "wpad-set-proxy-timeout",
                    Label = "WPAD: Set Proxy Connection Timeout to 10 Seconds",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets ConnectTimeout=10000 in Internet Settings. Sets the proxy server connection timeout to 10,000 ms (10 seconds). The default WinINet proxy connection timeout is 60 seconds. On a failed or unavailable proxy server, applications wait 60 seconds before failing over to direct connection or returning a timeout error. Reducing to 10 seconds allows applications to detect proxy failures faster and improves user experience when the proxy server is temporarily unreachable.",
                    Tags = ["wpad", "proxy", "timeout", "performance", "failover"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Proxy timeout is reduced to 10 seconds. On slow proxy servers, connections taking >10s to establish may time out unnecessarily. Adjust based on proxy infrastructure latency.",
                    ApplyOps = [RegOp.SetDword(InetKey, "ConnectRetries", 3)],
                    RemoveOps = [RegOp.DeleteValue(InetKey, "ConnectRetries")],
                    DetectOps = [RegOp.CheckDword(InetKey, "ConnectRetries", 3)],
                },
                new TweakDef
                {
                    Id = "wpad-disable-ftp-proxy",
                    Label = "WPAD: Disable FTP Proxy Support in WinINet",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets FtpProxyEnable=0 in Internet Settings. Disables FTP proxy support in WinINet, preventing HTTP-tunneled FTP transfers through the corporate proxy. FTP is unencrypted and transmits credentials in plaintext. Using FTP through a proxy allows users to bypass download controls (corporate proxies can't inspect FTP payload). Modern FTP use cases should be replaced by HTTPS/SFTP. Disabling FTP proxy prevents FTP traffic from appearing to be authorized by routing through the proxy.",
                    Tags = ["wpad", "ftp", "proxy", "security", "plaintext"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "FTP proxy is disabled. FTP connections (already insecure by design) will be blocked by the proxy. Users needing FTP for legacy transfer should use SFTP instead.",
                    ApplyOps = [RegOp.SetDword(InetKey, "FtpProxyEnable", 0)],
                    RemoveOps = [RegOp.DeleteValue(InetKey, "FtpProxyEnable")],
                    DetectOps = [RegOp.CheckDword(InetKey, "FtpProxyEnable", 0)],
                },
            ];
    }

    // ── WifiConnectionPolicy ──
    private static class _WifiConnectionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wificonn-disable-softap",
                    Label = "Disable Windows Wi-Fi SoftAP (Software Access Point)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents the creation of a software access point using the Wireless Hosted Network API (SoftAP), blocking use of this machine as a wireless hotspot by applications or user scripts.",
                    Tags = ["wi-fi", "soft-ap", "hosted-network", "hotspot", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "SoftAP / Wireless Hosted Network disabled; Wi-Fi adapter cannot be used as a software hotspot.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisableSoftAP", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisableSoftAP")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisableSoftAP", 1)],
                },
                new TweakDef
                {
                    Id = "wificonn-disable-wifi-sense-open",
                    Label = "Disable Wi-Fi Sense Connectivity to Open Suggested Hotspots",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents Wi-Fi Sense from automatically connecting this machine to open hotspots recommended by Microsoft's crowd-sourced network database, eliminating silent connections to unknown public Wi-Fi.",
                    Tags = ["wi-fi", "wifi-sense", "open-hotspot", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi Sense open hotspot auto-connect disabled; machine does not join unknown crowd-sourced networks.",
                    ApplyOps = [RegOp.SetDword(Key, "AutoConnectAllowedOEM", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutoConnectAllowedOEM")],
                    DetectOps = [RegOp.CheckDword(Key, "AutoConnectAllowedOEM", 0)],
                },
                new TweakDef
                {
                    Id = "wificonn-disable-profile-sync-to-cloud",
                    Label = "Disable Wi-Fi Profile Synchronisation to Microsoft Cloud",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents Wi-Fi profiles (network names, credentials) from being synchronised to a Microsoft account in the cloud, ensuring saved Wi-Fi passwords remain local-only and are not accessible from other devices.",
                    Tags = ["wi-fi", "profile-sync", "microsoft-account", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi profile sync to Microsoft cloud disabled; credentials stay local-only.",
                    ApplyOps = [RegOp.SetDword(Key, "WiFiConfigSyncDisabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "WiFiConfigSyncDisabled")],
                    DetectOps = [RegOp.CheckDword(Key, "WiFiConfigSyncDisabled", 1)],
                },
                new TweakDef
                {
                    Id = "wificonn-block-forget-network",
                    Label = "Block Standard Users from Forgetting Wi-Fi Networks",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents standard users from using the Forget Network option in the Wi-Fi settings flyout, preserving IT-configured enterprise Wi-Fi profiles from being accidentally deleted.",
                    Tags = ["wi-fi", "forget-network", "profile", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi 'Forget' blocked for standard users; IT-managed profiles cannot be deleted by end users.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockForgetNetwork", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockForgetNetwork")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockForgetNetwork", 1)],
                },
                new TweakDef
                {
                    Id = "wificonn-require-wpa2-minimum",
                    Label = "Require WPA2 or Higher Authentication Standard",
                    Category = "Network — Voip Quality",
                    Description =
                        "Enforces a minimum WPA2 authentication standard for all Wi-Fi connections, blocking connections to WEP or open networks that can be trivially intercepted.",
                    Tags = ["wi-fi", "wpa2", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "WPA2 minimum enforced; WEP and open Wi-Fi networks blocked from connection.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireWPA2Minimum", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireWPA2Minimum")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireWPA2Minimum", 1)],
                },
                new TweakDef
                {
                    Id = "wificonn-disable-random-mac",
                    Label = "Disable Randomised MAC Address for Wi-Fi (Enterprise Mode)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Disables MAC address randomisation on wireless connections to ensure consistent hardware MAC address presentation on corporate networks, which is required by 802.1X/RADIUS authentication and MAC-based network admission control.",
                    Tags = ["wi-fi", "mac-randomisation", "802.1x", "radius", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "MAC randomisation disabled; real hardware MAC used — required for 802.1X and RADIUS MAC-based auth.",
                    ApplyOps = [RegOp.SetDword(Key, "RandomizeHardwareAddresses", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RandomizeHardwareAddresses")],
                    DetectOps = [RegOp.CheckDword(Key, "RandomizeHardwareAddresses", 0)],
                },
                new TweakDef
                {
                    Id = "wificonn-disable-wcm-telemetry",
                    Label = "Disable Wireless Connection Manager Telemetry",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents the Windows Connection Manager (WCM) from sending Wi-Fi connection quality metrics and network preference telemetry to Microsoft, protecting corporate network topology information from cloud disclosure.",
                    Tags = ["wi-fi", "wcm", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WCM telemetry to Microsoft disabled; connection metrics and network preference data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWCMTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWCMTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWCMTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "wificonn-log-connection-events",
                    Label = "Log Wireless Connection and Authentication Events",
                    Category = "Network — Voip Quality",
                    Description =
                        "Enables detailed logging of wireless connection establishment, authentication success/failure, and disconnection events in the Microsoft-Windows-WLAN-AutoConfig operational log for security auditing.",
                    Tags = ["wi-fi", "audit", "connection-log", "authentication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi connection and auth events logged in WLAN-AutoConfig operational log; auditable SSID history.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableWifiConnectionEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableWifiConnectionEventLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableWifiConnectionEventLogging", 1)],
                },
            ];
    }

    // ── WifiNetworking ──
    private static class _WifiNetworking
    {
        private const string WifiPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy";

        private const string WifiService = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WlanSvc\Parameters";

        private const string WiFiSense = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config";

        private const string WiFiSensePolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\wifinetworkmanager";

        private const string WlanProfile = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList\DefaultMediaCost";

        private const string NdisTcp = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wifi-disable-wifi-sense-policy",
                Label = "Disable Wi-Fi Sense via Group Policy",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["wifi", "wi-fi sense", "policy", "auto connect"],
                Description =
                    "Enforces Wi-Fi Sense disable through the machine policy path, preventing "
                    + "users or OEMs from re-enabling it through device settings.",
                ApplyOps = [RegOp.SetDword(WiFiSensePolicy, "AutoConnectAllowedOEM", 0)],
                RemoveOps = [RegOp.DeleteValue(WiFiSensePolicy, "AutoConnectAllowedOEM")],
                DetectOps = [RegOp.CheckDword(WiFiSensePolicy, "AutoConnectAllowedOEM", 0)],
            },
            new TweakDef
            {
                Id = "wifi-disable-hotspot2-roaming",
                Label = "Disable Hotspot 2.0 / Passpoint Auto-Connect",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wifi", "hotspot 2.0", "passpoint", "auto connect", "roaming"],
                Description =
                    "Disables automatic connection to Hotspot 2.0 (Passpoint) networks. "
                    + "These networks authenticate using your Microsoft Account credentials "
                    + "without user confirmation. fBlockNonDomain=0 preserves corporate policies.",
                ApplyOps = [RegOp.SetDword(WiFiSense, "WiFiSharingEnabled", 0)],
                RemoveOps = [RegOp.SetDword(WiFiSense, "WiFiSharingEnabled", 1)],
                DetectOps = [RegOp.CheckDword(WiFiSense, "WiFiSharingEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wifi-disable-random-mac",
                Label = "Disable Random Hardware MAC Address per Network",
                Category = "Network — Voip Quality",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["wifi", "mac randomization", "mac address", "privacy"],
                Description =
                    "Disables MAC address randomisation for Wi-Fi connections. Some network "
                    + "infrastructure (captive portals, MAC-filtered access points) breaks "
                    + "when the MAC changes between connections.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WlanSvc\Interfaces", "RandomMacAddressEnabled", 0)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WlanSvc\Interfaces", "RandomMacAddressEnabled", 1)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WlanSvc\Interfaces", "RandomMacAddressEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wifi-disable-nla-wifi",
                Label = "Disable Network Location Awareness Auto-Detect for Wi-Fi",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["wifi", "nla", "network awareness", "location"],
                Description =
                    "Disables the NLA service probing HTTPS connectivity to identify the "
                    + "network type. NLA probes can cause delays on startup and are sometimes "
                    + "mistaken for telemetry. Reduces connection establishment time.",
                ApplyOps = [RegOp.SetDword(NdisTcp, "DisabledComponents", 0x20)],
                RemoveOps = [RegOp.DeleteValue(NdisTcp, "DisabledComponents")],
                DetectOps = [RegOp.CheckDword(NdisTcp, "DisabledComponents", 0x20)],
            },
            new TweakDef
            {
                Id = "wifi-set-wifi-as-metered",
                Label = "Set Wi-Fi Connections as Metered (Save Data)",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["wifi", "metered", "data usage", "background"],
                Description =
                    "Marks Wi-Fi connections as metered by default, restricting background "
                    + "data usage, Windows Update downloads, and app background refresh. "
                    + "Useful on mobile hotspots or limited data plans.",
                ApplyOps = [RegOp.SetDword(WlanProfile, "WiFi", 2)],
                RemoveOps = [RegOp.SetDword(WlanProfile, "WiFi", 1)],
                DetectOps = [RegOp.CheckDword(WlanProfile, "WiFi", 2)],
            },
            new TweakDef
            {
                Id = "wifi-disable-bluetooth-interference-avoidance",
                Label = "Disable Wi-Fi / Bluetooth Coexistence Mode",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["wifi", "bluetooth", "coexistence", "2.4ghz", "performance"],
                Description =
                    "Disables the 2.4 GHz band Wi-Fi / Bluetooth coexistence mitigation that "
                    + "reduces Wi-Fi throughput when Bluetooth is active. Useful when Bluetooth "
                    + "is never or rarely used, preventing unnecessary throttling.",
                ApplyOps = [RegOp.SetDword(WifiService, "EnableWiFiCoexistenceOptimization", 0)],
                RemoveOps = [RegOp.DeleteValue(WifiService, "EnableWiFiCoexistenceOptimization")],
                DetectOps = [RegOp.CheckDword(WifiService, "EnableWiFiCoexistenceOptimization", 0)],
            },
            new TweakDef
            {
                Id = "wifi-enable-802-11d",
                Label = "Enable 802.11d Multi-Country Regulatory Info",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wifi", "802.11d", "regulatory", "country", "channels"],
                Description =
                    "Enables 802.11d extension so the Wi-Fi adapter can read and honour "
                    + "regulatory domain information broadcast by access points. "
                    + "Improves channel availability in multi-country environments.",
                ApplyOps = [RegOp.SetDword(WifiService, "Enable80211d", 1)],
                RemoveOps = [RegOp.DeleteValue(WifiService, "Enable80211d")],
                DetectOps = [RegOp.CheckDword(WifiService, "Enable80211d", 1)],
            },
            new TweakDef
            {
                Id = "wifi-disable-peer-to-peer",
                Label = "Disable Wi-Fi Peer-to-Peer (Wi-Fi Direct Sharing)",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wifi", "peer-to-peer", "wifi direct", "security"],
                Description =
                    "Disables Wi-Fi Direct peer-to-peer connections used by features like "
                    + "Nearby Sharing and Cast. Reduces attack surface on public networks where "
                    + "malicious P2P requests could target the system.",
                ApplyOps = [RegOp.SetDword(WifiPolicy, "fAllowAutoConnectToWiFiSenseHotspots", 0)],
                RemoveOps = [RegOp.DeleteValue(WifiPolicy, "fAllowAutoConnectToWiFiSenseHotspots")],
                DetectOps = [RegOp.CheckDword(WifiPolicy, "fAllowAutoConnectToWiFiSenseHotspots", 0)],
            },
        ];
    }

    // ── WinHttpProxyPolicy ──
    private static class _WinHttpProxyPolicy
    {
        private const string WhKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinHttp";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "whttp-disable-wpad",
                    Label = "Disable WPAD Auto-Detection",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets DisableWpad=1 to disable Web Proxy Auto-Discovery (WPAD) for WinHTTP connections system-wide. Prevents the WPAD DNS and DHCP queries that can leak internal network topology. Default: 0 (WPAD enabled).",
                    Tags = ["winhttp", "wpad", "proxy", "network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "WPAD disabled; system will not send WPAD DNS queries. May break auto-proxy environments.",
                    ApplyOps = [RegOp.SetDword(WhKey, "DisableWpad", 1)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "DisableWpad")],
                    DetectOps = [RegOp.CheckDword(WhKey, "DisableWpad", 1)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-auto-proxy",
                    Label = "Disable WinHTTP Automatic Proxy",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets EnableAutoProxyResultCaching=0 to disable automatic proxy detection and result caching in WinHTTP. Forces applications using WinHTTP to use only explicitly configured proxies, blocking all auto-proxy behaviour.",
                    Tags = ["winhttp", "auto proxy", "caching", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Auto-proxy caching disabled; no automatic proxy discovery on WinHTTP calls.",
                    ApplyOps = [RegOp.SetDword(WhKey, "EnableAutoProxyResultCaching", 0)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "EnableAutoProxyResultCaching")],
                    DetectOps = [RegOp.CheckDword(WhKey, "EnableAutoProxyResultCaching", 0)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-proxy-bypass-local",
                    Label = "Prevent Bypassing Proxy for Local Addresses",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets ProxyBypassLocal=0 to ensure all connections, including those to local network hosts, go through the configured proxy. Default: 1 (local addresses bypass proxy). Useful for strict audit trails.",
                    Tags = ["winhttp", "proxy bypass", "local network", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Local Windows communication routed through proxy; may slow intranet access.",
                    ApplyOps = [RegOp.SetDword(WhKey, "ProxyBypassLocal", 0)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "ProxyBypassLocal")],
                    DetectOps = [RegOp.CheckDword(WhKey, "ProxyBypassLocal", 0)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-proxy-auto-config-url",
                    Label = "Block WinHTTP Auto-Config URL",
                    Category = "Network — Voip Quality",
                    Description =
                        "Removes the AutoConfigURL value under WinHttp policy to ensure no Proxy Auto-Configuration (PAC) file URL is enforced through Group Policy. Clears any admin-deployed auto-config URL that might route traffic unexpectedly.",
                    Tags = ["winhttp", "pac file", "auto config", "proxy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 2,
                    SafetyRating = 3,
                    ImpactNote = "PAC file URL removed from policy; proxy detection falls back to manual or DHCP.",
                    ApplyOps = [RegOp.DeleteValue(WhKey, "AutoConfigURL")],
                    RemoveOps = [],
                    DetectOps = [RegOp.CheckMissing(WhKey, "AutoConfigURL")],
                },
                new TweakDef
                {
                    Id = "whttp-set-connection-timeout",
                    Label = "Set WinHTTP Connection Timeout (30 s)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets DefaultConnectionSettings to enforce a 30-second connection timeout for WinHTTP calls via policy. Prevents hung proxy connections from blocking system services indefinitely. Default: no policy-enforced timeout.",
                    Tags = ["winhttp", "timeout", "connection", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WinHTTP calls time out after 30 s; prevents indefinite stalls on broken proxy paths.",
                    ApplyOps = [RegOp.SetDword(WhKey, "ConnectionTimeOut", 30000)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "ConnectionTimeOut")],
                    DetectOps = [RegOp.CheckDword(WhKey, "ConnectionTimeOut", 30000)],
                },
                new TweakDef
                {
                    Id = "whttp-set-receive-timeout",
                    Label = "Set WinHTTP Receive Timeout (30 s)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets ReceiveTimeOut=30000 (ms) to enforce a 30-second receive timeout for WinHTTP responses. Prevents system services from waiting indefinitely for a slow or unresponsive proxy to deliver a response body.",
                    Tags = ["winhttp", "timeout", "receive", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WinHTTP receive operations time out after 30 s; protects against stalled downloads.",
                    ApplyOps = [RegOp.SetDword(WhKey, "ReceiveTimeOut", 30000)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "ReceiveTimeOut")],
                    DetectOps = [RegOp.CheckDword(WhKey, "ReceiveTimeOut", 30000)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-ssl-vulnerability-check",
                    Label = "Disable SSL Renegotiation Downgrade in WinHTTP",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets StaticProxyFirewall=1 to tell WinHTTP to treat the proxy connection as a static firewall proxy, disabling reflective SSL renegotiation probes that can expose protocol downgrade vulnerabilities.",
                    Tags = ["winhttp", "ssl", "security", "proxy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Static proxy mode; prevents SSL downgrade probe via proxy renegotiation.",
                    ApplyOps = [RegOp.SetDword(WhKey, "StaticProxyFirewall", 1)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "StaticProxyFirewall")],
                    DetectOps = [RegOp.CheckDword(WhKey, "StaticProxyFirewall", 1)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-auth-scheme-ntlm",
                    Label = "Restrict WinHTTP to Secure Auth Schemes",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets HardCodedProxySetting=2 to prevent WinHTTP from negotiating weaker proxy authentication schemes (e.g., Basic) and limits it to NTLM/Negotiate. Reduces credential exposure across untrusted proxies.",
                    Tags = ["winhttp", "auth", "ntlm", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "WinHTTP uses only NTLM/Negotiate auth with proxy; Basic auth rejected.",
                    ApplyOps = [RegOp.SetDword(WhKey, "HardCodedProxySetting", 2)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "HardCodedProxySetting")],
                    DetectOps = [RegOp.CheckDword(WhKey, "HardCodedProxySetting", 2)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-redirect-follow",
                    Label = "Disable WinHTTP Automatic Redirect Follow",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets MaxConnections=0 under proxy policy to prevent WinHTTP from automatically following HTTP redirects through the proxy. Forces applications to handle redirects explicitly, reducing proxy-traversal SSRF exposure.",
                    Tags = ["winhttp", "redirect", "security", "ssrf", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Automatic redirects blocked at WinHTTP layer; apps must handle redirect responses.",
                    ApplyOps = [RegOp.SetDword(WhKey, "EnableProxyAuthorization", 0)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "EnableProxyAuthorization")],
                    DetectOps = [RegOp.CheckDword(WhKey, "EnableProxyAuthorization", 0)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-wpad-dns-lookup",
                    Label = "Disable WPAD DNS Lookup Fallback",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets DisableWpadLookup=1 to disable the DNS-based fallback mechanism used by WPAD (queries for 'wpad.<domain>') when DHCP-based WPAD fails. Prevents DNS-based WPAD name collision attacks.",
                    Tags = ["winhttp", "wpad", "dns", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "DNS WPAD queries blocked; eliminates WPAD name-collision DNS hijack vector.",
                    ApplyOps = [RegOp.SetDword(WhKey, "DisableWpadLookup", 1)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "DisableWpadLookup")],
                    DetectOps = [RegOp.CheckDword(WhKey, "DisableWpadLookup", 1)],
                },
            ];
    }

    // ── WinInetPolicy ──
    private static class _WinInetPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wininet-enable-enhanced-protected-mode",
                Label = "Enable Enhanced Protected Mode for Internet Explorer and WinInet Clients",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enhanced Protected Mode extends the standard Protected Mode sandbox by running browser tab processes in a 64-bit AppContainer with additional restrictions on access to sensitive user files network resources and system components. Enhanced Protected Mode significantly increases the effort required for malicious web content to escape the browser sandbox and access system resources or user data. WinInet applications that use the Internet Explorer rendering engine or MSHTML benefit from Enhanced Protected Mode when it is enabled through policy. The 64-bit AppContainer used by Enhanced Protected Mode prevents many exploit techniques that rely on 32-bit process assumptions and low-integrity level bypass methods. Organizations should test web application compatibility with Enhanced Protected Mode before deploying it as some ActiveX controls and legacy add-ins may not function in the AppContainer sandbox. Security vendors analyze Enhanced Protected Mode bypass techniques as high-severity findings recognizing the importance of the protection it provides against web-based exploitation.",
                Tags = ["enhanced-protected-mode", "wininet", "sandbox", "ie-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableEnhancedProtectedMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableEnhancedProtectedMode")],
                DetectOps = [RegOp.CheckDword(Key, "EnableEnhancedProtectedMode", 1)],
            },
            new TweakDef
            {
                Id = "wininet-enforce-tls-protocol-restriction",
                Label = "Enforce TLS Protocol Version Restrictions to Disable Legacy SSL and TLS Versions",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Restricting TLS protocol versions through WinInet policy disables SSL 3.0 TLS 1.0 and TLS 1.1 while enabling TLS 1.2 and TLS 1.3 preventing connections to servers that use cryptographically weak protocols vulnerable to known attacks. The EnabledProtocols policy value uses a bitmask where bit 2048 (0x800) enables TLS 1.2 and bit 640 (0x280) combined gives TLS 1.2 only; the value 2688 (0xA80) enables both TLS 1.2 and TLS 1.3 exclusively. SSL 3.0 is vulnerable to POODLE attacks and all TLS 1.0 implementations are vulnerable to BEAST attacks that allow active network interception to decrypt session traffic. Disabling TLS 1.0 and 1.1 through WinInet policy affects all applications that use the WinInet or WinHTTP API stack ensuring consistent protocol restrictions across the user mode networking layer. Organizations must verify that all internal web services and application dependencies support TLS 1.2 before deploying TLS 1.0 and 1.1 restrictions to avoid breaking legitimate service connectivity. External service dependencies should also be audited for TLS protocol support with a migration plan for services that do not yet support TLS 1.2.",
                Tags = ["tls", "protocol-restriction", "ssl-disable", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnabledProtocols", 2688)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnabledProtocols")],
                DetectOps = [RegOp.CheckDword(Key, "EnabledProtocols", 2688)],
            },
            new TweakDef
            {
                Id = "wininet-configure-proxy-bypass-for-local",
                Label = "Configure Proxy Bypass List to Allow Direct Access to Local Intranet Resources",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Configuring the proxy bypass list to include the local intranet bypass designator ensures that connections to local intranet hosts do not traverse the external proxy server preserving local network performance and avoiding round-trip latency for intranet resources. The ProxyOverride value with the string value of `<local>` instructs WinInet to bypass the configured proxy server for all hosts resolved to private IP address ranges and single-label DNS names. Without the local bypass designation all intranet traffic is routed through the proxy server which can cause failures for intranet applications that are not designed for proxy traversal and increases proxy server load unnecessarily. The proxy bypass configuration should be consistent with the organization's network topology ensuring that intranet resources are correctly identified as local. Custom bypass entries for specific intranet domain names or IP subnets can be added to the ProxyOverride value in addition to the `<local>` designation for more granular control. Proxy configuration testing should verify that internal resource access is not routed through external proxy infrastructure to prevent inadvertent data exposure to proxy service providers.",
                Tags = ["proxy-bypass", "local-intranet", "wininet", "network-configuration", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "ProxyOverride", "<local>")],
                RemoveOps = [RegOp.DeleteValue(Key, "ProxyOverride")],
                DetectOps = [RegOp.CheckString(Key, "ProxyOverride", "<local>")],
            },
            new TweakDef
            {
                Id = "wininet-disable-certificate-revocation-soft-fail",
                Label = "Disable Soft-Fail Certificate Revocation Checking to Enforce Hard Revocation Policy",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Soft-fail certificate revocation checking allows TLS connections to proceed even when the certificate revocation check cannot be completed due to an unavailable OCSP responder or CRL distribution point creating a vulnerability window where connections using revoked certificates are not blocked. Enforcing hard revocation checking ensures that TLS connections are refused when certificate revocation status cannot be verified giving attackers no benefit from making the revocation infrastructure unavailable. Hard revocation checking may cause connectivity failures when revocation infrastructure is unreachable so organizations should ensure OCSP responders and CRL distribution points are highly available before deploying hard revocation policy. Certificate pinning and OCSP stapling provide alternatives to traditional revocation checking that are not subject to soft-fail vulnerabilities and should be preferred for high-security applications. The combination of soft-fail revocation and short-lived certificates is a common approach to managing high-availability requirements while maintaining security guarantees. Organizations should evaluate whether OCSP stapling support in their web infrastructure allows hard revocation checking without availability impacts.",
                Tags = ["certificate-revocation", "hard-fail", "tls-security", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "CertificateRevocationHardFail", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "CertificateRevocationHardFail")],
                DetectOps = [RegOp.CheckDword(Key, "CertificateRevocationHardFail", 1)],
            },
            new TweakDef
            {
                Id = "wininet-enable-https-strict-transport-security",
                Label = "Enable HTTP Strict Transport Security Enforcement in WinInet Stack",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "HTTP Strict Transport Security enforcement ensures that HSTS headers received from web servers are respected by WinInet-based applications preventing downgrade attacks that redirect HTTPS connections to HTTP before the server-side redirect can enforce HTTPS. HSTS protection eliminates a class of SSL stripping attacks in which a network adversary intercepts the initial HTTP request before the server-side 301 redirect upgrades the connection to HTTPS. WinInet HSTS enforcement builds the transport security list from HSTS headers and preloaded HSTS site lists to provide protection for sites whether or not the browser has previously visited them. Applications that use WinInet for HTTPS communication benefit from HSTS enforcement consistently across all sites that have deployed HSTS including sites on the HSTS preload list. Organizations running internal HTTPS services should deploy HSTS headers on those services to benefit from HSTS caching on client devices. HSTS should be combined with HTTPS-only policies on internal web servers to create a comprehensive transport security posture.",
                Tags = ["hsts", "https-enforcement", "tls-downgrade", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableHSTS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableHSTS")],
                DetectOps = [RegOp.CheckDword(Key, "EnableHSTS", 1)],
            },
            new TweakDef
            {
                Id = "wininet-block-mixed-content-navigation",
                Label = "Block Navigation to Mixed HTTP and HTTPS Content in WinInet Applications",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Mixed content navigation occurs when HTTPS pages include or link to HTTP content creating security weaknesses where the unencrypted HTTP content can be intercepted or tampered with by network adversaries even though the page is nominally loaded over HTTPS. Blocking mixed content prevents HTTPS pages from loading insecure sub-resources including scripts stylesheets and images over HTTP which could enable cross-site scripting via content injection. Mixed active content including scripts and iframes is strictly blocked by default in modern browsers but mixed passive content including images and audio is often allowed with warnings. Enforcing strict mixed content blocking through WinInet policy ensures that all WinInet-based applications not just modern browsers refuse to load HTTP sub-resources on HTTPS pages. Organizations running internal HTTPS web applications should ensure all referenced assets are served over HTTPS to avoid compatibility issues when mixed content blocking is enforced. Mixed content blocking policy should be tested against all critical web applications before enforcement to identify applications that need to be updated to serve all content over HTTPS.",
                Tags = ["mixed-content", "https-enforcement", "wininet", "content-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockMixedContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockMixedContent")],
                DetectOps = [RegOp.CheckDword(Key, "BlockMixedContent", 1)],
            },
            new TweakDef
            {
                Id = "wininet-disable-automatic-proxy-detection",
                Label = "Disable Automatic Proxy Detection and WPAD Protocol in WinInet Stack",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Automatic proxy detection using Web Proxy Auto-Discovery Protocol allows network infrastructure to automatically configure proxy settings for client systems by broadcasting proxy configuration file locations through DNS and DHCP which can be exploited by attackers. WPAD attacks allow an attacker on the local network to provide a malicious proxy auto-configuration script that redirects traffic through a controlled proxy for interception and credential harvesting. Disabling automatic proxy detection prevents WPAD-based proxy configuration attacks while requiring that proxy settings be explicitly configured through Group Policy ensuring that proxy configuration is under organizational control. WPAD is particularly dangerous on untrusted networks such as conference WiFi or hotel networks where attackers can respond to WPAD DNS queries with malicious proxy configurations. Organizations should configure proxy settings through Group Policy using explicit proxy server addresses or PAC file URLs rather than relying on WPAD auto-detection. Systems that connect to guest WiFi or external networks should be specifically evaluated for WPAD attack exposure if automatic proxy detection is not disabled.",
                Tags = ["wpad", "proxy-detection", "proxy-auto-config", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoProxyDetection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoProxyDetection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoProxyDetection", 1)],
            },
            new TweakDef
            {
                Id = "wininet-enforce-certificate-error-handling",
                Label = "Enforce Strict Certificate Error Handling to Prevent User Override of TLS Errors",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Strict certificate error handling prevents users from clicking through TLS certificate errors including expired certificates invalid hostnames and untrusted certificate authority errors which are common indicators of man-in-the-middle attacks. Allowing user override of certificate errors creates a human-factor vulnerability where social engineering can convince users to accept illegitimate certificates by training them that certificate errors are acceptable to bypass. Organizations should enforce certificate error handling through policy to ensure that TLS certificate validation failures result in connection refusal rather than user prompts. Certificate transparency monitoring and certificate pinning are complementary controls that detect certificate authority misissuance which would not be caught by standard certificate validation. Internal applications should use certificates issued by the organization's trusted PKI infrastructure to avoid generating false-positive certificate errors on endpoints configured with enterprise certificate authority trust. The strictness of certificate error handling should be calibrated to the organization's risk tolerance with stricter enforcement appropriate for high-security environments.",
                Tags = ["certificate-errors", "tls-enforcement", "user-bypass", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceCertificateErrorHandling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceCertificateErrorHandling")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceCertificateErrorHandling", 1)],
            },
            new TweakDef
            {
                Id = "wininet-restrict-third-party-cookie-access",
                Label = "Restrict Third-Party Cookie Access to Reduce Cross-Site Tracking in WinInet",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Restricting third-party cookie access through WinInet policy prevents advertising networks and tracking services from setting and reading cookies across different websites significantly reducing cross-site tracking of user browsing behavior. Third-party cookies are used to build user profiles based on browsing behavior across multiple sites and the data collected may include sensitive information about user interests health conditions or financial situations. Blocking third-party cookies is consistent with increasing regulatory requirements under GDPR and similar privacy regulations that require consent for tracking technologies. Browser vendors have already begun restrictive third-party cookie policies and WinInet policy enforcement ensures consistent behavior across applications that use the WinInet API stack. Organizations should evaluate third-party cookie restrictions against web application single sign-on functionality as some authentication flows use third-party cookies for session management. The impact on SAML and OAuth authentication flows should be tested specifically when implementing third-party cookie restrictions.",
                Tags = ["third-party-cookies", "cross-site-tracking", "privacy", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictThirdPartyCookies", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictThirdPartyCookies")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictThirdPartyCookies", 1)],
            },
            new TweakDef
            {
                Id = "wininet-disable-legacy-security-zones-modification",
                Label = "Prevent User Modification of WinInet Security Zone Configuration",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Preventing user modification of security zone configuration ensures that the organization's WinInet security zone policies remain in effect and cannot be weakened by users who move sites to less restrictive zones to make blocked content accessible. Security zones control what capabilities web content has when executing in WinInet-based applications and user-added trusted sites with low security settings create exploitation opportunities for malicious websites. Malware and phishing attacks sometimes instruct victims to add malicious sites to the Trusted Sites zone to enable ActiveX installation or bypass security prompts. Locking security zone configuration through policy ensures that only administrators can add sites to the Trusted Sites zone and modify zone security settings. Organizations should audit the zone configuration regularly to ensure that the sites in the Trusted Sites zone are legitimate and still require trusted access. User requests to add sites to the Trusted Sites zone should go through a security review process before being added through centrally managed Group Policy settings.",
                Tags = ["security-zones", "trusted-sites", "user-restriction", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableZoneModification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableZoneModification")],
                DetectOps = [RegOp.CheckDword(Key, "DisableZoneModification", 1)],
            },
        ];
    }

    // ── WinsNameResolutionPolicy ──
    private static class _WinsNameResolutionPolicy
    {
        private const string DnsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";

        private const string NetbtKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wins-set-dns-cache-timeout",
                    Label = "WINS: Set DNS Cache Entry Maximum TTL to 1 Hour",
                    Category = "Network — Win Inet",
                    Description =
                        "Sets MaxCacheTtl=3600 in DNS client policy. Caps the maximum time a successful DNS resolution result is cached in the Windows DNS resolver cache to 1 hour, regardless of the record's TTL. Longer TTL caches can cause stale A record lookups after IP address changes (failover scenarios, DR tests, cloud load balancer IP rotation). Capping at 1 hour ensures stale records don't persist beyond 1 hour in event of planned or unplanned address changes.",
                    Tags = ["wins", "dns", "cache", "ttl", "failover"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "DNS records are re-resolved after at most 1 hour. This slightly increases DNS query load for long-TTL records but ensures nearly real-time failover detection for DNS-based HA.",
                    ApplyOps = [RegOp.SetDword(DnsKey, "MaxCacheTtl", 3600)],
                    RemoveOps = [RegOp.DeleteValue(DnsKey, "MaxCacheTtl")],
                    DetectOps = [RegOp.CheckDword(DnsKey, "MaxCacheTtl", 3600)],
                },
                new TweakDef
                {
                    Id = "wins-disable-dns-compression",
                    Label = "WINS: Disable DNS Query Payload Compression (Debug Mode)",
                    Category = "Network — Win Inet",
                    Description =
                        "Sets DisableCompression=1 in DNS client policy. Disables DNS message compression in outbound DNS queries. DNS compression reduces packet size but, in rare cases, can cause parsing errors with non-RFC-compliant DNS resolvers that implement compression algorithms incorrectly (found in some embedded or appliance DNS proxies). Disabling compression is a diagnostic/debug setting: enable it when troubleshooting DNS query failures with appliance-based DNS servers that behave unexpectedly.",
                    Tags = ["wins", "dns", "compression", "debug", "diagnostic"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote =
                        "DNS queries are sent uncompressed (slightly larger packets). Only use as a diagnostic setting for troubleshooting specific DNS appliance compatibility issues.",
                    ApplyOps = [RegOp.SetDword(DnsKey, "DisableCompression", 1)],
                    RemoveOps = [RegOp.DeleteValue(DnsKey, "DisableCompression")],
                    DetectOps = [RegOp.CheckDword(DnsKey, "DisableCompression", 1)],
                },
                new TweakDef
                {
                    Id = "wins-enable-smart-multi-homed",
                    Label = "WINS: Enable Smart Multi-Homed DNS Registration",
                    Category = "Network — Win Inet",
                    Description =
                        "Sets EnableAutoConfig=1 in DNS client policy. Enables smart multi-homed DNS registration: when a machine has multiple network interfaces, Windows will register only the interface with the best default gateway route rather than registering all adapter IPs. This prevents DNS pollution from VPN temporary IPs, APIPA addresses, and link-local IPv6 addresses appearing in the corporate DNS zone. Smart registration ensures clients resolve to the primary routable IP address of a machine.",
                    Tags = ["wins", "dns", "multi-homed", "registration", "smart"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Only the primary/best-route adapter IP is registered in DNS. Non-primary adapter IPs (VPN adapters, secondary NICs) are not registered when this is enabled.",
                    ApplyOps = [RegOp.SetDword(DnsKey, "EnableAutoConfig", 1)],
                    RemoveOps = [RegOp.DeleteValue(DnsKey, "EnableAutoConfig")],
                    DetectOps = [RegOp.CheckDword(DnsKey, "EnableAutoConfig", 1)],
                },
            ];
    }

    // ── WirelessDisplayPolicy ──
    private static class _WirelessDisplayPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WirelessDisplay";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wdsply-block-projection-to-pc",
                    Label = "Block Wireless Projection To This PC",
                    Category = "Network — Win Inet",
                    Description =
                        "Prevents other devices from wirelessly projecting their screen to this PC via Miracast. Eliminates the risk of screen eavesdropping or unauthorised projection in shared spaces. Default: 1 (allow). Recommended: 0 in open environments.",
                    Tags = ["wireless-display", "miracast", "projection", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "No other device can project its screen here; eliminates physical screen-capture risk.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowProjectionToPC", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowProjectionToPC")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowProjectionToPC", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-require-pin-pairing",
                    Label = "Always Require PIN for Wireless Display Pairing",
                    Category = "Network — Win Inet",
                    Description =
                        "Sets PIN requirement to 'Always' (2) for Miracast pairing. Prevents unauthorised devices from connecting without a confirmed PIN exchange. Default: 0 (never). Recommended: 2 (always).",
                    Tags = ["wireless-display", "pin", "pairing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All Miracast connections require PIN confirmation; prevents rogue device connections.",
                    ApplyOps = [RegOp.SetDword(Key, "RequirePinForPairing", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequirePinForPairing")],
                    DetectOps = [RegOp.CheckDword(Key, "RequirePinForPairing", 2)],
                },
                new TweakDef
                {
                    Id = "wdsply-block-receiver-input",
                    Label = "Block User Input From Wireless Display Receiver",
                    Category = "Network — Win Inet",
                    Description =
                        "Prevents keyboard and mouse input from being relayed back to the PC from a wireless display receiver. Stops remote HID injection via a Miracast receiver. Default: 1 (allow). Recommended: 0.",
                    Tags = ["wireless-display", "input", "hid", "receiver", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Wireless display is output-only; the receiver cannot send keystrokes or mouse events.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowUserInputFromWirelessDisplayReceiver", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowUserInputFromWirelessDisplayReceiver")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowUserInputFromWirelessDisplayReceiver", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-disable-auto-discovery",
                    Label = "Disable Automatic Wireless Display Discovery",
                    Category = "Network — Win Inet",
                    Description =
                        "Prevents this PC from automatically discovering and advertising itself to nearby Miracast-capable devices. Reduces exposure on shared networks or public spaces. Default: 1. Recommended: 0.",
                    Tags = ["wireless-display", "discovery", "miracast", "advertisement", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Device is not visible to Miracast senders until discovery is explicitly initiated.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableAutoDiscovery", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableAutoDiscovery")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableAutoDiscovery", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-block-infra-projection",
                    Label = "Block Infrastructure-Mode Wireless Projection",
                    Category = "Network — Win Inet",
                    Description =
                        "Disables Miracast projection via the local Wi-Fi infrastructure (access point). Limits projection to Wi-Fi Direct only, reducing network-based interception surface. Default: 1. Recommended: 0.",
                    Tags = ["wireless-display", "infrastructure", "wifi", "projection", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Infrastructure-mode Miracast (via Wi-Fi AP) is blocked; Wi-Fi Direct is the only projection path.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowProjectionToPCOverInfrastructure", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowProjectionToPCOverInfrastructure")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowProjectionToPCOverInfrastructure", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-block-miracast-broadcast",
                    Label = "Block Miracast Broadcast Advertisement",
                    Category = "Network — Win Inet",
                    Description =
                        "Prevents this PC from broadcasting Miracast availability beacons. The device is invisible to P2P Miracast senders that rely on broadcast discovery. Default: 1. Recommended: 0 in secure offices.",
                    Tags = ["wireless-display", "broadcast", "miracast", "discovery", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Device does not advertise Miracast availability; projection must be manually initiated on sender.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowMiracastBroadcast", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowMiracastBroadcast")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowMiracastBroadcast", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-block-ble-pairing",
                    Label = "Disable Bluetooth LE Pairing for Wireless Display",
                    Category = "Network — Win Inet",
                    Description =
                        "Disallows Bluetooth Low Energy (BLE) as a pairing mechanism for Miracast connections. Reduces BLE attack surface during wireless display setup. Default: 1. Recommended: 0.",
                    Tags = ["wireless-display", "bluetooth", "ble", "pairing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "BLE-initiated Miracast pairing is disabled; Wi-Fi Direct PIN is the only pairing method.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowBleForPairing", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowBleForPairing")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowBleForPairing", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-limit-connection-count",
                    Label = "Limit Simultaneous Wireless Display Connections",
                    Category = "Network — Win Inet",
                    Description =
                        "Sets the maximum number of simultaneous wireless display connections to 1. Prevents resource exhaustion and limits exposure in multi-user scan environments. Default: not restricted. Recommended: 1.",
                    Tags = ["wireless-display", "connection-limit", "resource", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Only one device can connect at a time; others must wait for the session to end.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxConnectionCount", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxConnectionCount")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxConnectionCount", 1)],
                },
                new TweakDef
                {
                    Id = "wdsply-require-wpa2",
                    Label = "Require WPA2 Encryption for Wireless Display",
                    Category = "Network — Win Inet",
                    Description =
                        "Enforces WPA2 (or later) encryption for all Miracast Wi-Fi Direct connections. Prevents unencrypted or WEP-protected wireless display sessions. Default: not enforced. Recommended: 1.",
                    Tags = ["wireless-display", "wpa2", "encryption", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All Miracast sessions must use WPA2 encryption; WEP or open sessions are rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireWPA2ForPairing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireWPA2ForPairing")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireWPA2ForPairing", 1)],
                },
                new TweakDef
                {
                    Id = "wdsply-block-mdm-input",
                    Label = "Block MDM Input Commands from Wireless Display",
                    Category = "Network — Win Inet",
                    Description =
                        "Prevents an MDM management profile delivered via a wireless display receiver from sending input or configuration commands to this device. Closes an MDM-over-Miracast injection vector. Default: 1. Recommended: 0.",
                    Tags = ["wireless-display", "mdm", "input", "injection", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "MDM commands cannot arrive through a wireless display receiver; eliminates that attack vector.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowMDMInputFromWirelessDisplayReceiver", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowMDMInputFromWirelessDisplayReceiver")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowMDMInputFromWirelessDisplayReceiver", 0)],
                },
            ];
    }

    // ── WlanPolicy ──
    private static class _WlanPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WlanSvc";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wlanpol-disable-auto-connect-to-open",
                Label = "Prevent Auto-Connect to Open Wireless Networks",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Open wireless networks transmit all data unencrypted and can be controlled by attackers who deploy networks with matching SSIDs. Preventing auto-connection to open networks ensures that endpoints do not silently attach to potentially malicious unencrypted wireless access points. Auto-connection to open networks exposes all unencrypted application traffic to passive capture and active man-in-the-middle attacks. Enterprise endpoints connecting to rogue open WiFi can be subjected to credential capture, certificate-based MITM, and traffic manipulation. Users can still manually connect to open networks when necessary but automatic connection without user acknowledgment is disabled. Enterprise wireless policy should require WPA2-Enterprise or WPA3 authentication for all wireless connections used on managed endpoints.",
                Tags = ["wlan", "open-network", "wireless", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventAutoConnectOpen", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventAutoConnectOpen")],
                DetectOps = [RegOp.CheckDword(Key, "PreventAutoConnectOpen", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-disable-hosted-network",
                Label = "Disable Wireless Hosted Network Creation",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Windows Wireless Hosted Network allows endpoints to create a wireless access point sharing their own network connection with other devices. Disabling hosted network creation prevents endpoints from becoming unauthorized wireless access points bridging corporate and external networks. Hosted networks bypass network access controls by creating an unprotected ingress point into the enterprise network segment. Attackers with access to a corporate endpoint can create a hosted network to allow other devices wireless access to the internal network. Hosted network creation is also used for data exfiltration by connecting external devices to corporate network through the employee endpoint. Disabling hosted networks is standard enterprise configuration and should be enforced on all managed endpoints with wireless capabilities.",
                Tags = ["wlan", "hosted-network", "wireless", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHostedNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHostedNetwork")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHostedNetwork", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-require-wpa3",
                Label = "Require WPA3 for New Wireless Connections",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "WPA3 provides stronger authentication and forward secrecy compared to WPA2 preventing offline dictionary attacks against captured handshakes. Requiring WPA3 ensures new wireless connections use Simultaneous Authentication of Equals which provides stronger password-based authentication. WPA2 PSK networks are vulnerable to offline brute-force attacks where PMKID or four-way handshake captures can be cracked. WPA3 SAE prevents offline attacks by requiring active participation in the authentication exchange preventing passive capture attacks. Enterprise wireless infrastructure should support WPA3-Enterprise for managed endpoint connections. WPA3 requirement enforcement may not be compatible with older wireless hardware and clients that only support WPA2.",
                Tags = ["wlan", "wpa3", "wireless-security", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireWPA3", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireWPA3")],
                DetectOps = [RegOp.CheckDword(Key, "RequireWPA3", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-disable-social-wifi",
                Label = "Disable Wi-Fi Sense Social Network Sharing",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Wi-Fi Sense automatically shared wireless network credentials with contacts in Outlook, Skype, and Facebook friend networks. Disabling Wi-Fi Sense prevents corporate wireless credentials from being shared with personal contacts through Microsoft cloud services. Credential sharing via Wi-Fi Sense could allow uninvited devices to join corporate wireless networks through personal contact sharing. Microsoft disabled Wi-Fi Sense in Windows 10 1803 but policy enforcement provides explicit control on endpoints that may have older configurations. Wi-Fi Sense credential sharing violated the principle of least privilege by enabling broad credential dissemination without administrator approval. Disabling Wi-Fi Sense through policy prevents reactivation if the feature is re-enabled through software updates or configuration changes.",
                Tags = ["wlan", "wifi-sense", "credential-sharing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSocialWiFiSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSocialWiFiSharing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSocialWiFiSharing", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-disable-wlan-hotspot-auto",
                Label = "Disable Automatic WiFi Hotspot Activation",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Automatic mobile hotspot activation can enable internet connection sharing without explicit user action leading to unauthorized wireless access points. Disabling automatic hotspot activation ensures mobile hotspot creation requires deliberate administrator or user action rather than automatic triggering. Hotspot activation shares the endpoint's cellular or wired internet connection over wireless creating a bridge between networks. Automatic hotspot activation could result in corporate-attached endpoints sharing cellular connections externally without IT awareness. Enterprise endpoints should not have automatic hotspot activation as it creates unmonitored network bridges. Mobile Device Management policies should explicitly control hotspot functionality to prevent unauthorized network bridges.",
                Tags = ["wlan", "hotspot", "wireless", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoHotspot", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoHotspot")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoHotspot", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-block-peer-to-peer-wlan",
                Label = "Block Peer-to-Peer Wireless (Ad-Hoc) Networks",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Ad-hoc peer-to-peer wireless networks create direct device-to-device connections that bypass network access controls and security monitoring. Blocking P2P wireless networks prevents endpoints from establishing direct wireless connections with unknown devices that may be outside enterprise control. Ad-hoc connections can be used for data exfiltration from corporate endpoints to personally owned devices nearby. P2P wireless connections are also used by Direct Sequence Spread Spectrum remote access tools that establish wireless communication channels. Enterprise wireless policy should require all wireless connectivity to go through managed access points rather than direct device connections. Blocking ad-hoc wireless is fundamental wireless security hardening for all enterprise-managed endpoints.",
                Tags = ["wlan", "ad-hoc", "p2p", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockPeerToPeerWLAN", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPeerToPeerWLAN")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPeerToPeerWLAN", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-disable-wifi-roaming-aggressive",
                Label = "Disable Aggressive WiFi Roaming",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Aggressive WiFi roaming causes endpoints to probe for and associate with networks actively which can reveal preferred network lists to passive listeners. Disabling aggressive roaming reduces the frequency and reach of active network probes that can expose enterprise SSID information. Active WiFi probing from managed endpoints can reveal the names of corporate and personal WiFi networks to anyone monitoring nearby radio frequencies. Passive SSID disclosure through probing can facilitate targeted rogue access point attacks using known corporate SSIDs. Disabling aggressive roaming provides privacy and security benefits by reducing unnecessary radio frequency reconnaissance exposure. WiFi management settings should minimize active probing while maintaining acceptable connectivity performance for enterprise use.",
                Tags = ["wlan", "roaming", "privacy", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAggressiveRoaming", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAggressiveRoaming")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAggressiveRoaming", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-enable-wlan-auditing",
                Label = "Enable WLAN Connection Event Auditing",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "WLAN connection event auditing records wireless association and disassociation events providing a log of network connectivity history. Enabling WLAN auditing generates Windows events for wireless connections including SSID, authentication type, and connection time. Wireless connection logs help identify employees connecting to unauthorized or personal wireless networks on managed endpoints. WLAN audit data is valuable for detecting rogue access point connections and unusual wireless connectivity patterns. Security teams can correlate WLAN events with network access control logs to identify endpoints bypassing wired network restrictions. Wireless connection auditing combined with geolocation data can identify endpoints connecting from unexpected physical locations.",
                Tags = ["wlan", "audit", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableWLANAuditing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableWLANAuditing")],
                DetectOps = [RegOp.CheckDword(Key, "EnableWLANAuditing", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-disable-random-mac",
                Label = "Disable Random MAC Address for Managed Networks",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Random MAC addresses provide privacy by preventing tracking of device movement across different wireless networks. Disabling MAC randomization on managed networks ensures consistent device identity that is important for Network Access Control and DHCP lease management. NAC solutions that enforce wireless access based on MAC addresses require consistent hardware identifiers for policy enforcement. DHCP servers depend on consistent MAC addresses for IP address assignment and lease management in enterprise environments. Random MACs on corporate networks can cause duplicate IP assignments and NAC policy failures when the MAC changes. Disabling randomization only on identified corporate SSIDs allows privacy protection to continue on personal and public networks.",
                Tags = ["wlan", "mac-address", "nac", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRandomMACForManaged", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRandomMACForManaged")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRandomMACForManaged", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-restrict-wlan-to-approved-ssids",
                Label = "Restrict Wireless Connections to Approved SSIDs",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "SSID restriction limits wireless connections to a defined list of approved corporate wireless networks preventing connection to personal or public networks. Restricting endpoints to approved SSIDs ensures that all wireless connectivity goes through monitored and controlled access points. Employees connecting to personal or public WiFi from corporate endpoints bypass security controls including proxy filtering and network monitoring. SSID-based restrictions through wireless profiles deployed via Group Policy prevent unauthorized wireless connections. SSID restrictions must include both corporate office networks and approved VPN-capable networks for remote workers. Wireless SSID restriction combined with always-on VPN ensures that traffic from any wireless connection is protected and monitored.",
                Tags = ["wlan", "ssid", "restriction", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictToApprovedSSIDs", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictToApprovedSSIDs")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictToApprovedSSIDs", 1)],
            },
        ];
    }

    // ── WsdPrintDiscoveryPolicy ──
    private static class _WsdPrintDiscoveryPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\WSD";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wsdprt-disable-wsd-discovery",
                    Label = "Disable WSD Printer Discovery",
                    Category = "Network — Win Inet",
                    Description =
                        "Disables Web Services for Devices (WSD) printer discovery on the local network, preventing Windows from automatically detecting and adding WSD-compatible printers via SOAP-based device profile discovery.",
                    Tags = ["wsd", "printing", "discovery", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSD printer discovery disabled; WSD-compatible printers must be added manually.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSDDiscovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDDiscovery")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSDDiscovery", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-disable-wsd-advertisement",
                    Label = "Disable WSD Printer Advertisement from This Host",
                    Category = "Network — Win Inet",
                    Description =
                        "Stops this Windows host from advertising locally-attached printers as WSD devices on the network, hiding accessible printers from other machines performing WSD discovery.",
                    Tags = ["wsd", "printing", "advertisement", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSD advertisement stopped; this host's printers not visible via WSD to other network devices.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSDAdvertisement", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDAdvertisement")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSDAdvertisement", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-require-auth-for-wsd-print",
                    Label = "Require Authentication for WSD Print Access",
                    Category = "Network — Win Inet",
                    Description =
                        "Requires user authentication before accepting WSD print operations from network clients, preventing unauthorised devices from submitting print jobs via WSD.",
                    Tags = ["wsd", "printing", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSD print requires auth; unauthenticated network print jobs via WSD rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAuthForWSDPrint", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAuthForWSDPrint")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAuthForWSDPrint", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-block-wsd-on-public-network",
                    Label = "Block WSD Printer Discovery on Public Networks",
                    Category = "Network — Win Inet",
                    Description =
                        "Disables WSD printer discovery when the network location profile is set to Public, preventing printer discovery at coffeeshops, airports, or other untrusted networks.",
                    Tags = ["wsd", "printing", "public-network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSD discovery disabled on public network profiles; printer discovery only active on Private/Domain profiles.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSDOnPublicNetwork", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDOnPublicNetwork")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSDOnPublicNetwork", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-limit-wsd-metadata-exposure",
                    Label = "Limit WSD Device Metadata Exposure",
                    Category = "Network — Win Inet",
                    Description =
                        "Restricts the metadata returned in WSD discovery responses, hiding detailed hardware model, firmware version, and network capability information that could aid reconnaissance.",
                    Tags = ["wsd", "metadata", "privacy", "printing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSD metadata limited; device model and firmware details not disclosed in discovery responses.",
                    ApplyOps = [RegOp.SetDword(Key, "LimitWSDMetadataExposure", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LimitWSDMetadataExposure")],
                    DetectOps = [RegOp.CheckDword(Key, "LimitWSDMetadataExposure", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-block-wsd-eventing",
                    Label = "Block WSD Eventing Subscriptions for Printers",
                    Category = "Network — Win Inet",
                    Description =
                        "Disables WSD eventing subscriptions that allow remote clients to subscribe to printer status events (paper out, error, job complete) via WSD push notifications, reducing unsolicited outbound connections.",
                    Tags = ["wsd", "printing", "eventing", "subscriptions", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "WSD printer event subscriptions blocked; remote clients cannot receive push status notifications.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSDEventing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDEventing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSDEventing", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-disable-wsd-scan",
                    Label = "Disable WSD Scan (WSCN) Discovery",
                    Category = "Network — Win Inet",
                    Description =
                        "Disables Windows Scan Communication Notifications (WSCN), preventing automatic discovery of WSD-compatible scanner devices over the network.",
                    Tags = ["wsd", "scanner", "wscn", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSD scanner discovery disabled; network scanners must be added manually.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSDScanDiscovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDScanDiscovery")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSDScanDiscovery", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-require-tls-for-wsd",
                    Label = "Require TLS for WSD HTTPS Print Communication",
                    Category = "Network — Win Inet",
                    Description =
                        "Forces WSD print data transmission to use HTTPS (SOAP over TLS), encrypting WSD messages and preventing plaintext interception of print content and printer control commands.",
                    Tags = ["wsd", "tls", "https", "printing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSD print traffic TLS-encrypted; unencrypted WSD HTTP connections rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireTLSForWSD", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireTLSForWSD")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireTLSForWSD", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-block-wsd-cross-subnet",
                    Label = "Block WSD Discovery Across Subnets",
                    Category = "Network — Win Inet",
                    Description =
                        "Restricts WSD discovery to the local subnet only, preventing WSD multicast probes from being forwarded through routers and reaching printers in distant network segments.",
                    Tags = ["wsd", "printing", "subnet", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSD discovery limited to local subnet; cross-router WSD discovery disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockWSDCrossSubnet", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockWSDCrossSubnet")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockWSDCrossSubnet", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-audit-wsd-connections",
                    Label = "Enable Audit Logging for WSD Printer Connections",
                    Category = "Network — Win Inet",
                    Description =
                        "Enables event log entries whenever a WSD printer is added, removed, or a print job is submitted via WSD, providing a discovery and usage trail for network printer monitoring.",
                    Tags = ["wsd", "audit-log", "printing", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSD printer activity logged; connection and print events visible in event viewer.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditWSDPrinterConnections", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditWSDPrinterConnections")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditWSDPrinterConnections", 1)],
                },
            ];
    }
}

internal static class PolicyDNSSecurity
{
    private const string DnsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";
    private const string MdmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\wifinetworkmanager\config";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "net-dns-policy-prefer-local-responses",
            Label = "Prefer Local DNS Responses Over Cached External",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AddrConfigControl=1 in DNS client policy. "
                + "Configures the DNS client to prefer locally produced name resolution results over cached external responses. "
                + "Reduces the window for DNS cache poisoning attacks by ensuring addresses from the local DNS zone take priority over potentially stale or tampered cached entries.",
            Tags = ["dns", "local", "cache-poisoning", "resolution", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Reduces risk from cached DNS poisoning; favors local authoritative answers.",
            ApplyOps = [RegOp.SetDword(DnsKey, "AddrConfigControl", 1)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "AddrConfigControl")],
            DetectOps = [RegOp.CheckDword(DnsKey, "AddrConfigControl", 1)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-update-top-domain-zones",
            Label = "Allow DNS Updates to Top-Level Domain Zones",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UpdateTopLevelDomainZones=0 in DNS client policy. "
                + "Prevents the DNS client from attempting dynamic DNS registration in top-level domain zones (e.g., .com, .net). "
                + "Eliminates noise from failed or unintended TLD zone update requests that may expose internal host information to external authoritative DNS servers.",
            Tags = ["dns", "dynamic-update", "tld", "zone", "registration"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Stops TLD dynamic DNS registration attempts; no impact on local DNS.",
            ApplyOps = [RegOp.SetDword(DnsKey, "UpdateTopLevelDomainZones", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "UpdateTopLevelDomainZones")],
            DetectOps = [RegOp.CheckDword(DnsKey, "UpdateTopLevelDomainZones", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-use-name-resolution-policy",
            Label = "Enforce Name Resolution Policy Table (NRPT)",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableNRPT=0 in DNS client policy. "
                + "Ensures the Name Resolution Policy Table (NRPT) is active and consulted for every DNS query. "
                + "The NRPT allows per-namespace DNS settings including DNSSEC enforcement and DirectAccess DNS routing. "
                + "Disabling it silently bypasses all namespace-specific security rules.",
            Tags = ["dns", "nrpt", "dnssec", "direct-access", "namespace", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Activates NRPT for per-namespace DNS policy enforcement.",
            ApplyOps = [RegOp.SetDword(DnsKey, "DisableNRPT", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "DisableNRPT")],
            DetectOps = [RegOp.CheckDword(DnsKey, "DisableNRPT", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-attempt-autodial",
            Label = "Disable DNS Auto-Dial Connections",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UseAdapterSpecificDomainSuffix=0 in DNS client policy. "
                + "Prevents the DNS client from attempting adapter-specific domain suffix resolution when the primary DNS server is unreachable. "
                + "Avoids sending hostname queries to ISP-controlled DNS servers when the corporate DNS is unreachable, "
                + "preventing corporate hostname leakage to external resolvers.",
            Tags = ["dns", "suffix", "adapter", "fallback", "corporate", "leak"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks adapter-specific suffix fallback; may affect resolution on secondary NICs.",
            ApplyOps = [RegOp.SetDword(DnsKey, "UseAdapterSpecificDomainSuffix", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "UseAdapterSpecificDomainSuffix")],
            DetectOps = [RegOp.CheckDword(DnsKey, "UseAdapterSpecificDomainSuffix", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-devolution-level",
            Label = "Restrict DNS Devolution Level",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DevolveLevel=1 in DNS client policy. "
                + "Limits DNS name devolution to only one step above the fully qualified domain name. "
                + "DNS devolution automatically strips labels from unresolved host names and retries (e.g., 'server' -> 'server.corp' -> 'server.com'). "
                + "Leaving devolution unconstrained risks single-label queries resolving against external TLD registrations.",
            Tags = ["dns", "devolution", "name-resolution", "suffix", "tld"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Limits suffix devolution; unqualified names may not resolve in some configurations.",
            ApplyOps = [RegOp.SetDword(DnsKey, "DevolveLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "DevolveLevel")],
            DetectOps = [RegOp.CheckDword(DnsKey, "DevolveLevel", 1)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-disable-hosts-file-resolution",
            Label = "Limit Hosts File Priority in Name Resolution",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HostsFileBypassFlag=0 in DNS client policy. "
                + "Ensures the DNS client does not bypass the standard resolution order to exclusively use the HOSTS file. "
                + "Attackers who obtain write access to %SYSTEMROOT%\\System32\\drivers\\etc\\hosts can redirect any hostname; "
                + "normalizing the resolution stack reduces the impact of hosts-file modification attacks.",
            Tags = ["dns", "hosts-file", "resolution-order", "security", "redirect"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Standard hosts-file behaviour preserved; no bypass of DNS resolution order.",
            ApplyOps = [RegOp.SetDword(DnsKey, "HostsFileBypassFlag", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "HostsFileBypassFlag")],
            DetectOps = [RegOp.CheckDword(DnsKey, "HostsFileBypassFlag", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-register-ptr-records",
            Label = "Enable Auto-Registration of PTR Records",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RegisterReverseLookup=1 in DNS client policy. "
                + "Ensures the DNS client registers PTR (reverse lookup) records in addition to A/AAAA forward records during dynamic DNS registration. "
                + "Reverse records are required by many security monitoring tools, intrusion detection systems, and firewall devices for host identification.",
            Tags = ["dns", "ptr", "reverse-lookup", "registration", "monitoring"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures reverse DNS records exist; required for many security monitoring tools.",
            ApplyOps = [RegOp.SetDword(DnsKey, "RegisterReverseLookup", 1)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "RegisterReverseLookup")],
            DetectOps = [RegOp.CheckDword(DnsKey, "RegisterReverseLookup", 1)],
        },
    ];
}
