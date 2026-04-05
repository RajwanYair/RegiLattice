namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyNetwork
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _AdhocNetworkPolicy.Data,
            .. _AlwaysOnVpnPolicy.Data,
            .. _BackgroundTransferPolicy.Data,
            .. _BitsTransferPolicy.Data,
            .. _BranchCache.Data,
            .. _BranchCachePolicy.Data,
            .. _CacheManagerPolicy.Data,
            .. _ConnectedCachePolicy.Data,
            .. _DataSensePolicy.Data,
            .. _DataUsageMeteringPolicy.Data,
            .. _DeliveryOptimizationPolicy.Data,
            .. _DfsnPolicy.Data,
            .. _DfsrPolicy.Data,
            .. _DiffServQosPolicy.Data,
            .. _DirectAccessConnectPolicy.Data,
            .. _DnsClientRegistrationPolicy.Data,
            .. _DnsSecurePolicy.Data,
            .. _DohEnforcementPolicy.Data,
            .. _DynamicDataExchangePolicy.Data,
            .. _EapNetworkPolicy.Data,
            .. _HomeGroupPolicy.Data,
            .. _HotspotAuthenticationPolicy.Data,
            .. _Ieee8021xPolicy.Data,
            .. _InternetCommunicationPolicy.Data,
            .. _IpsecRulePolicy.Data,
            .. _Ipv6Policy.Data,
            .. _LanmanServerPolicy.Data,
            .. _LanmanWorkstationPolicy.Data,
            .. _LdapClientPolicy.Data,
            .. _LdapSigningPolicy.Data,
            .. _LegacyProtocols.Data,
            .. _LltdProtocolPolicy.Data,
            .. _MapsBrowserPolicy.Data,
            .. _MobilityPolicy.Data,
            .. _NearbySharingPolicy.Data,
            .. _NetBiosPolicy.Data,
            .. _NetCfgPolicy.Data,
            .. _NetIoOffloadPolicy.Data,
            .. _NetLocationAwarenessAdvancedPolicy.Data,
            .. _NetworkAccessProtectionPolicy.Data,
            .. _NetworkAccessProtPolicy.Data,
        ];

    // ── AdhocNetworkPolicy ──
    private static class _AdhocNetworkPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WirelessNetwork";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "adhocnet-disable-adhoc-networks",
                Label = "Disable Ad-hoc Wireless Network Connections",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Ad-hoc wireless networks allow two or more devices to communicate directly without a wireless access point intermediary. Disabling ad-hoc wireless networking prevents endpoints from joining or creating peer-to-peer wireless networks. Ad-hoc networks bypass enterprise network monitoring and security controls that operate on managed access points. Corporate endpoints connected to ad-hoc networks can create bridging paths between the corporate network and attacker-controlled devices. Security standards including PCI DSS and HIPAA require disabling ad-hoc networking on endpoints handling regulated data. All wireless network connections should go through enterprise managed and monitored 802.11 infrastructure.",
                Tags = ["wireless", "adhoc", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAdHocNetworks", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAdHocNetworks")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAdHocNetworks", 1)],
            },
            new TweakDef
            {
                Id = "adhocnet-disable-wifi-direct",
                Label = "Disable Wi-Fi Direct",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Wi-Fi Direct enables peer-to-peer wireless connections between devices for applications like screen mirroring, file sharing, and printing. Disabling Wi-Fi Direct prevents endpoints from establishing direct device-to-device wireless connections. Wi-Fi Direct connections can transfer data outside of enterprise network monitoring with no visibility to network security teams. Device pairing over Wi-Fi Direct can be initiated by malicious nearby devices capable of capturing enterprise data. Enterprise wireless policies should restrict all wireless communication to monitored infrastructure access points. Applications requiring peer-to-peer wireless capability should use enterprise-approved channels and protocols instead.",
                Tags = ["wireless", "wifi-direct", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWifiDirect", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWifiDirect")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWifiDirect", 1)],
            },
            new TweakDef
            {
                Id = "adhocnet-disable-hotspot-creation",
                Label = "Disable Wireless Hotspot Creation",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Windows Mobile Hotspot allows a managed endpoint to share its network connection with other devices by creating a software access point. Disabling hotspot creation prevents managed endpoints from becoming wireless bridges for other devices. A hotspot on a corporate endpoint creates an unmonitored wireless access point on the enterprise network. Devices connecting to an endpoint hotspot bypass corporate network controls and egress through the endpoint rather than the monitored enterprise gateway. NAC and DLP controls designed for the enterprise network perimeter are bypassed by hotspot-connected devices. Preventing hotspot creation maintains the integrity of enterprise network topology and security perimeter.",
                Tags = ["wireless", "hotspot", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMobileHotspot", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMobileHotspot")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMobileHotspot", 1)],
            },
            new TweakDef
            {
                Id = "adhocnet-disable-wifi-sense",
                Label = "Disable Wi-Fi Sense Automatic Connection Sharing",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Wi-Fi Sense automatically shared Wi-Fi passwords with contacts and allowed automatic connection to hotspots associated with the user's social network accounts. Disabling Wi-Fi Sense prevents corporate Wi-Fi credentials from being automatically shared with the user's external contacts. Wi-Fi password sharing through social account integration represents an uncontrolled credential distribution mechanism. Shared enterprise Wi-Fi credentials enable unauthorized devices outside the enterprise to connect to corporate wireless networks. Corporate Wi-Fi credentials should only be distributed through managed provisioning processes with authentication and endpoint checking. This feature was largely removed in later Windows versions but the policy prevents any remnant functionality.",
                Tags = ["wireless", "wifi-sense", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWifiSense", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWifiSense")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWifiSense", 1)],
            },
            new TweakDef
            {
                Id = "adhocnet-disable-wireless-config",
                Label = "Disable Wireless Network Configuration by Users",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Windows allows standard users to configure wireless network profiles including adding, modifying, and connecting to wireless networks. Disabling user wireless configuration prevents standard accounts from adding new wireless network profiles or changing wireless settings. Enterprise wireless connectivity should be configured through Group Policy preferred wireless networks rather than user-initiated connections. User-created wireless profiles may connect to untrusted networks bypassing enterprise network security controls. Malicious wireless networks with enterprise-resembling SSIDs can capture credentials from endpoints that automatically attempt connection. Administrator-managed wireless profiles ensure all wireless connections use enterprise-approved networks with appropriate security.",
                Tags = ["wireless", "configuration", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableUserWirelessConfig", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUserWirelessConfig")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUserWirelessConfig", 1)],
            },
            new TweakDef
            {
                Id = "adhocnet-disable-unmanaged-networks",
                Label = "Block Connections to Unmanaged Wireless Networks",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Windows can be configured to block connections to wireless networks not defined in enterprise-managed preferred network profiles. Blocking unmanaged network connections prevents endpoints from connecting to any wireless network not explicitly approved by IT. Cafe hotspots, home networks, and unknown guest networks all represent untrusted wireless environments for corporate endpoints. Data transmitted on unmanaged networks is subject to interception through evil twin attacks and rogue access points. Restricting connectivity to managed networks ensures all wireless communication goes through enterprise-monitored and secured infrastructure. This setting requires that all legitimate connectivity requirements be addressed through managed network profiles.",
                Tags = ["wireless", "management", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockUnmanagedWirelessNetworks", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUnmanagedWirelessNetworks")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUnmanagedWirelessNetworks", 1)],
            },
            new TweakDef
            {
                Id = "adhocnet-disable-random-mac",
                Label = "Disable Random Hardware MAC Address",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows supports MAC address randomization which rotates the hardware address used for wireless connections to prevent device tracking. Disabling MAC randomization ensures enterprise endpoints use their permanent hardware MAC address when connecting to wireless networks. Enterprise NAC and network access control infrastructure relies on MAC address identification for device authentication and access control. Random MAC addresses prevent NAC systems from identifying and authenticating devices based on hardware identity. Wireless network security frameworks including 802.1X are designed for permanent MAC-based device enrollment. Disabling randomization is appropriate for enterprise networks where device tracking capability is required and protection is provided by 802.1X authentication.",
                Tags = ["wireless", "mac-address", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMacAddressRandomization", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMacAddressRandomization")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMacAddressRandomization", 1)],
            },
            new TweakDef
            {
                Id = "adhocnet-disable-auto-connect-open",
                Label = "Disable Auto-Connect to Open Wireless Networks",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Windows can automatically connect to open wireless networks without WPA protection when configured network profiles are unavailable. Disabling auto-connect to open networks prevents endpoints from silently joining unprotected wireless networks when no secured network is in range. Open wireless networks provide no encryption and all transmitted data is visible to any observer in radio range. Auto-connecting to an unknown open network can expose enterprise credentials, session cookies, and corporate data in transit. Evil twin attacks specifically use open networks with recognizable SSIDs to capture traffic from devices that auto-connect. Preventing auto-connection to open networks is a fundamental wireless security requirement for corporate devices.",
                Tags = ["wireless", "auto-connect", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoConnectOpenNetworks", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoConnectOpenNetworks")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoConnectOpenNetworks", 1)],
            },
            new TweakDef
            {
                Id = "adhocnet-disable-wireless-telemetry",
                Label = "Disable Wireless Network Telemetry",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Wireless network telemetry collects data about wireless network connections, signal strength, disconnection events, and connection speed. This data helps Microsoft improve wireless connection reliability and diagnose driver compatibility issues. Disabling wireless telemetry prevents information about corporate wireless infrastructure from being reported externally. Wireless network names and signal characteristics constitute sensitive corporate infrastructure information. Enterprise wireless network monitoring should be performed through approved network management systems rather than telemetry. Wireless connection functionality is completely unaffected by disabling this telemetry collection.",
                Tags = ["wireless", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWirelessTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWirelessTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWirelessTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "adhocnet-disable-wlan-service",
                Label = "Restrict WLAN AutoConfig Service Use",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "The WLAN AutoConfig service manages wireless network connections including automatic profile selection and connection establishment. Restricting WLAN AutoConfig service prevents standard users from modifying service behavior and connection logic. Non-administrator modifications to the WLAN AutoConfig service can override enterprise network profile configurations. Service restrictions ensure that the enterprise Group Policy-defined wireless behavior cannot be altered by user-level configuration. Administrative control over the WLAN service ensures consistent network connectivity behavior across all managed endpoints. Critical wireless infrastructure management functions remain available to administrators through proper channels.",
                Tags = ["wireless", "wlan-service", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictWlanAutoConfig", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictWlanAutoConfig")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictWlanAutoConfig", 1)],
            },
        ];
    }

    // ── AlwaysOnVpnPolicy ──
    private static class _AlwaysOnVpnPolicy
    {
        private const string VpnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator";

        private const string AgentKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings";

        private const string RasKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RasMan\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "aovpn-require-machine-cert-ikev2",
                    Label = "Always On VPN: Require Machine Certificate for IKEv2 Auth",
                    Category = "Network",
                    Description =
                        "Sets DisableAdvancedCredentialUI=1 in RasMan policy parameters. Disables username/password (MSCHAPv2) authentication fallback for IKEv2 VPN connections, requiring machine certificate authentication. MSCHAPv2 is vulnerable to offline brute-force attacks; certificate-based IKEv2 auth uses asymmetric cryptography that cannot be brute-forced. This policy is critical for AOVPN deployments where device tunnel must authenticate before user logon.",
                    Tags = ["aovpn", "certificate", "ikev2", "authentication", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Requires valid machine certificate from enterprise CA on all AOVPN clients. Clients without certificates lose VPN connectivity.",
                    ApplyOps = [RegOp.SetDword(RasKey, "DisableAdvancedCredentialUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "DisableAdvancedCredentialUI")],
                    DetectOps = [RegOp.CheckDword(RasKey, "DisableAdvancedCredentialUI", 1)],
                },
                new TweakDef
                {
                    Id = "aovpn-enable-dns-registration",
                    Label = "Always On VPN: Enable Dynamic DNS Registration via VPN",
                    Category = "Network",
                    Description =
                        "Sets RegisterDnsARecords=1 in RasMan/Parameters. Enables dynamic DNS registration for the VPN adapter's IP address against the corporate DNS server when AOVPN connects. Without DNS registration, remote clients cannot be reached by hostname from the corporate network, breaking RDP-to-client, IT-admin remote management, SCCM/Intune management channels, and MDM policies that require network-initiated connections to the endpoint.",
                    Tags = ["aovpn", "dns", "registration", "management", "connectivity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Creates DNS A records for VPN client IPs in corporate DNS zones. DNS records must have appropriate scavenging to prevent stale accumulation.",
                    ApplyOps = [RegOp.SetDword(RasKey, "RegisterDnsARecords", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "RegisterDnsARecords")],
                    DetectOps = [RegOp.CheckDword(RasKey, "RegisterDnsARecords", 1)],
                },
                new TweakDef
                {
                    Id = "aovpn-disable-vpn-reconnect-prompt",
                    Label = "Always On VPN: Disable Reconnect UI Prompt After Disconnect",
                    Category = "Network",
                    Description =
                        "Sets DisableReconnectToIncompatible=1. Suppresses the Windows dialog that prompts users to reconnect to their VPN after an unexpected disconnection. In AOVPN deployments, the VPN reconnects automatically without user interaction; the reconnect dialog is therefore unnecessary and confusing. Hiding it prevents users from attempting manual reconnects that could interfere with the AOVPN auto-reconnect logic.",
                    Tags = ["aovpn", "reconnect", "ui", "ux", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "VPN reconnect dialog is suppressed. AOVPN auto-reconnects silently. Users should be informed that VPN connectivity is managed automatically.",
                    ApplyOps = [RegOp.SetDword(RasKey, "DisableReconnectToIncompatible", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "DisableReconnectToIncompatible")],
                    DetectOps = [RegOp.CheckDword(RasKey, "DisableReconnectToIncompatible", 1)],
                },
                new TweakDef
                {
                    Id = "aovpn-enable-bypass-for-local",
                    Label = "Always On VPN: Enable Local Network Subnet Bypass",
                    Category = "Network",
                    Description =
                        "Sets BypassForLocal=1 in VPN profile policy. Allows traffic to local network resources (LAN printers, local file shares, home NAS) to bypass the VPN tunnel and route directly over the local interface. Without local bypass, users connected via AOVPN in a full-tunnel configuration must route all local traffic through the VPN server, preventing access to home printers and causing unnecessarily slow local file transfers.",
                    Tags = ["aovpn", "split-tunnel", "local-bypass", "lan", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Local subnet devices are accessible directly, bypassing VPN inspection. Corporate security policy may prohibit LAN bypass in sensitive environments.",
                    ApplyOps = [RegOp.SetDword(RasKey, "BypassForLocal", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "BypassForLocal")],
                    DetectOps = [RegOp.CheckDword(RasKey, "BypassForLocal", 1)],
                },
                new TweakDef
                {
                    Id = "aovpn-set-ikev2-max-retries",
                    Label = "Always On VPN: Set IKEv2 Reconnect Max Retries to 3",
                    Category = "Network",
                    Description =
                        "Sets MaxRetries=3 in RasMan. Limits IKEv2 tunnel re-establishment attempts to 3 on network interruptions before the AOVPN client gives up and waits for next trigger. Excessive retries during network instability cause IKEv2 SA flooding on the VPN gateway server and degrade performance for all concurrent VPN users. Three retries covers transient interruptions while preventing retry storm behavior.",
                    Tags = ["aovpn", "ikev2", "retry", "reliability", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Limits IKEv2 retry attempts. On very unstable networks, AOVPN may appear disconnected longer between retries.",
                    ApplyOps = [RegOp.SetDword(RasKey, "MaxRetries", 3)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "MaxRetries")],
                    DetectOps = [RegOp.CheckDword(RasKey, "MaxRetries", 3)],
                },
                new TweakDef
                {
                    Id = "aovpn-disable-rpc-over-http",
                    Label = "Always On VPN: Disable RPC over HTTP for VPN Connections",
                    Category = "Network",
                    Description =
                        "Sets RpcOverHttpEnabled=0 in Internet Settings. Disables Outlook's RPC over HTTP (ActiveSync/HTTPS proxy) for RPC calls when the AOVPN connection is active. RPC over HTTP creates a secondary HTTPS path for Exchange/Outlook traffic that bypasses the VPN tunnel's traffic inspection. When AOVPN is active, all corporate Outlook/Exchange traffic should route through the IPsec tunnel to the corporate Exchange server, not through a separate HTTPS path.",
                    Tags = ["aovpn", "rpc", "outlook", "exchange", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "RPC-over-HTTP for Outlook Anywhere is blocked when VPN is active. Exchange-over-VPN via MAPI or EAS is the supported channel.",
                    ApplyOps = [RegOp.SetDword(AgentKey, "RpcOverHttpEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(AgentKey, "RpcOverHttpEnabled")],
                    DetectOps = [RegOp.CheckDword(AgentKey, "RpcOverHttpEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "aovpn-enable-filter-list-audit",
                    Label = "Always On VPN: Enable VPN Traffic Filter Audit Logging",
                    Category = "Network",
                    Description =
                        "Sets VpnFilterAudit=1 in RasMan. Enables Windows auditing for VPN traffic filter rule matches (AppId filters and destination IP/port filters) configured in the AOVPN profile. Filter audit events are written to the Windows Security event log (Event ID 5455). This provides visibility into which applications and traffic flows are matching or bypassing AOVPN traffic routing rules, supporting both security monitoring and VPN policy debugging.",
                    Tags = ["aovpn", "audit", "filter", "logging", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Generates additional Windows Security event log entries per filter match. Event log capacity should be reviewed if filter policies are granular.",
                    ApplyOps = [RegOp.SetDword(RasKey, "VpnFilterAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "VpnFilterAudit")],
                    DetectOps = [RegOp.CheckDword(RasKey, "VpnFilterAudit", 1)],
                },
                new TweakDef
                {
                    Id = "aovpn-disable-class-based-route",
                    Label = "Always On VPN: Disable Class-Based Default Route via VPN",
                    Category = "Network",
                    Description =
                        "Sets DisableClassBasedDefaultRoute=1 in RasMan. Prevents Windows from adding a class-based default IP route through the VPN adapter when AOVPN connects in split-tunnel mode. Class-based routes incorrectly override specific split-tunnel routes, causing internet traffic to unexpectedly route through the VPN (de facto full-tunneling despite split-tunnel configuration). Disabling class-based routes ensures only the explicitly defined AOVPN split-tunnel routes are used.",
                    Tags = ["aovpn", "routing", "split-tunnel", "default-route", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Corrects routing behavior in split-tunnel AOVPN deployments. No impact on full-tunnel configurations.",
                    ApplyOps = [RegOp.SetDword(RasKey, "DisableClassBasedDefaultRoute", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "DisableClassBasedDefaultRoute")],
                    DetectOps = [RegOp.CheckDword(RasKey, "DisableClassBasedDefaultRoute", 1)],
                },
                new TweakDef
                {
                    Id = "aovpn-set-ike-sa-lifetime",
                    Label = "Always On VPN: Set IKE SA Lifetime to 8 Hours",
                    Category = "Network",
                    Description =
                        "Sets IkeProtocolStateTransitionTimeout=28800 (8 hours). Sets the IKE Security Association (SA) lifetime for AOVPN IKEv2 tunnels to 8 hours. After 8 hours, the SA requires cryptographic renewal (re-keying). Shorter SA lifetimes improve forward secrecy (compromising one session key doesn't expose 24 hours of traffic) while not rekeying so frequently that it disrupts session continuity. 8 hours matches a standard work day without mid-session rekeying.",
                    Tags = ["aovpn", "ikev2", "sa-lifetime", "cryptography", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "IKE SA rekeys every 8 hours. The rekey occurs transparently without session interruption in well-configured AOVPN deployments.",
                    ApplyOps = [RegOp.SetDword(RasKey, "IkeProtocolStateTransitionTimeout", 28800)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "IkeProtocolStateTransitionTimeout")],
                    DetectOps = [RegOp.CheckDword(RasKey, "IkeProtocolStateTransitionTimeout", 28800)],
                },
                new TweakDef
                {
                    Id = "aovpn-enable-lockdown",
                    Label = "Always On VPN: Enable Lockdown Mode (Block Traffic When VPN Down)",
                    Category = "Network",
                    Description =
                        "Sets VpnLockDown=1 in RasMan. Activates AOVPN Lockdown mode which uses the Windows Filtering Platform to block ALL network traffic except the VPN tunnel traffic when the AOVPN connection is disconnected or not yet established. In Lockdown mode, sensitive endpoint data cannot leak to the local network or internet during the window between network connection and VPN establishment. Essential for privileged endpoints handling classified or highly sensitive data.",
                    Tags = ["aovpn", "lockdown", "traffic-block", "security", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "ALL network traffic is blocked if VPN is not connected. Pre-logon connections and emergency internet access are blocked. Use only on privileged endpoints where data sensitivity justifies the restriction.",
                    ApplyOps = [RegOp.SetDword(RasKey, "VpnLockDown", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "VpnLockDown")],
                    DetectOps = [RegOp.CheckDword(RasKey, "VpnLockDown", 1)],
                },
            ];
    }

    // ── BackgroundTransferPolicy ──
    private static class _BackgroundTransferPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BackgroundIntelligentTransfer";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "bitsadv-limit-max-bandwidth",
                    Label = "Limit BITS Maximum Bandwidth (1 Mbps)",
                    Category = "Network",
                    Description =
                        "Caps total BITS download bandwidth to 1 Mbps per machine. Prevents Windows Update, Delivery Optimization uploads, and other BITS consumers from saturating the network link during business hours. Default: unlimited. Recommended: adjust per available bandwidth.",
                    Tags = ["bits", "bandwidth", "network", "throttle", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All BITS transfers are capped at 1 Mbps total; background downloads cannot saturate the uplink.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxTransferRateOnSchedule", 1024)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxTransferRateOnSchedule")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxTransferRateOnSchedule", 1024)],
                },
                new TweakDef
                {
                    Id = "bitsadv-limit-max-jobs",
                    Label = "Limit Maximum Concurrent BITS Jobs to 5",
                    Category = "Network",
                    Description =
                        "Restricts the number of BITS jobs that can run concurrently per user to 5. Prevents a single application or attacker from flooding the BITS queue with a large number of simultaneous download jobs. Default: up to 200 jobs. Recommended: 5 for controlled environments.",
                    Tags = ["bits", "jobs", "throttle", "resource-limit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "No more than 5 concurrent BITS download jobs per user; job flooding is prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxJobsPerUser", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxJobsPerUser")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxJobsPerUser", 5)],
                },
                new TweakDef
                {
                    Id = "bitsadv-limit-files-per-job",
                    Label = "Limit Maximum Files Per BITS Job to 100",
                    Category = "Network",
                    Description =
                        "Limits each BITS job to a maximum of 100 files. Prevents a single job from monopolising BITS or being used to exfiltrate a large number of small files in one batch operation. Default: up to 200 files per job. Recommended: 100.",
                    Tags = ["bits", "files", "job-limit", "resource-limit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Each BITS job is limited to 100 files; bulk-exfiltration via single BITS job is restricted.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxFilesPerJob", 100)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxFilesPerJob")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxFilesPerJob", 100)],
                },
                new TweakDef
                {
                    Id = "bitsadv-limit-job-download-size",
                    Label = "Limit BITS Job Download Size to 4 GB",
                    Category = "Network",
                    Description =
                        "Caps the total bytes a single BITS job can download to 4 GiB (4,294 MB). Feature updates are typically ≤ 4 GiB; this prevents misuse of BITS for downloading arbitrarily large payloads. Default: no download size limit. Recommended: 4 GiB.",
                    Tags = ["bits", "download-size", "resource-limit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Single BITS job download size is capped at 4 GiB; oversized downloads fail and must be split.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxDownloadSizePerJob", 4096)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxDownloadSizePerJob")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxDownloadSizePerJob", 4096)],
                },
                new TweakDef
                {
                    Id = "bitsadv-limit-job-upload-size",
                    Label = "Limit BITS Job Upload Size to 1 GB",
                    Category = "Network",
                    Description =
                        "Restricts the amount of data that a single BITS upload job can send to 1 GiB. Prevents BITS from being used as an exfiltration vector to upload large amounts of data to an attacker-controlled server. Default: no upload size limit. Recommended: 1 GiB.",
                    Tags = ["bits", "upload-size", "dlp", "resource-limit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "BITS upload per job is capped at 1 GiB; large-scale data exfiltration via BITS upload is blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxUploadSizePerJob", 1024)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxUploadSizePerJob")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxUploadSizePerJob", 1024)],
                },
                new TweakDef
                {
                    Id = "bitsadv-disable-internet-uploads",
                    Label = "Block BITS Uploads to Internet Destinations",
                    Category = "Network",
                    Description =
                        "Prevents BITS upload jobs from targeting internet destinations (hosts outside the local network and trusted intranet zones). Limits BITS uploads to intranet servers only, blocking a common Living-off-the-Land (LotL) exfiltration technique that uses BITS to send data to external C2 servers. Default: internet upload allowed. Recommended: 1.",
                    Tags = ["bits", "upload", "internet", "exfiltration", "lotl", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "BITS upload jobs targeting internet hosts are blocked; uploads are restricted to intranet destinations.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePeerCachingServer", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePeerCachingServer")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePeerCachingServer", 1)],
                },
                new TweakDef
                {
                    Id = "bitsadv-require-https",
                    Label = "Require HTTPS for BITS Transfers",
                    Category = "Network",
                    Description =
                        "Forces all BITS download/upload jobs to use HTTPS only. HTTP transfers expose the payload to MITM interception or tampering in transit. Default: HTTP transfers allowed. Recommended: 1 in high-security environments.",
                    Tags = ["bits", "https", "tls", "encryption", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "BITS refusing HTTP connections ensures all transfers are TLS-encrypted.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireHTTPS", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireHTTPS")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireHTTPS", 1)],
                },
                new TweakDef
                {
                    Id = "bitsadv-set-job-inactivity-timeout",
                    Label = "Set BITS Job Inactivity Timeout to 7 Days",
                    Category = "Network",
                    Description =
                        "Removes BITS jobs from the queue if they have not made progress within 7 days (604,800 seconds). Prevents stale or abandoned jobs from persisting indefinitely and consuming queue resources. Default: 90-day timeout. Recommended: 7 days.",
                    Tags = ["bits", "timeout", "inactivity", "cleanup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "BITS jobs that stall for more than 7 days are automatically removed from the queue.",
                    ApplyOps = [RegOp.SetDword(Key, "JobInactivityTimeout", 604800)],
                    RemoveOps = [RegOp.DeleteValue(Key, "JobInactivityTimeout")],
                    DetectOps = [RegOp.CheckDword(Key, "JobInactivityTimeout", 604800)],
                },
                new TweakDef
                {
                    Id = "bitsadv-disable-peer-caching-client",
                    Label = "Disable BITS Peer Caching (Client)",
                    Category = "Network",
                    Description =
                        "Prevents the local machine from acting as a BITS peer cache client — it will not receive content from peer machines on the LAN via BITS peer caching. Reduces lateral data movement between machines and limits the LAN attack surface of BITS. Default: peer caching enabled. Recommended: 1 when central delivery is preferred.",
                    Tags = ["bits", "peer-cache", "lan", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Machine will not receive BITS cached content from peers on the LAN; all downloads sourced from the server.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePeerCachingClient", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePeerCachingClient")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePeerCachingClient", 1)],
                },
                new TweakDef
                {
                    Id = "bitsadv-enable-audit-logging",
                    Label = "Enable BITS Transfer Audit Logging",
                    Category = "Network",
                    Description =
                        "Records BITS job creation, completion, cancellation, and error events to the Microsoft-Windows-Bits-Client/Operational event log. Provides forensic visibility into what files were downloaded or uploaded via BITS, essential for detecting LotL abuse. Default: limited operational logging. Recommended: 1.",
                    Tags = ["bits", "audit", "logging", "forensics", "lotl", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "All BITS job activity (create, complete, error, cancel) is logged to the Operational event channel.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableBITSAuditLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableBITSAuditLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableBITSAuditLogging", 1)],
                },
            ];
    }

    // ── BitsTransferPolicy ──
    private static class _BitsTransferPolicy
    {
        private const string BitsPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BITS";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "bitspol-job-inactivity-30d",
                Label = "Reduce BITS Job Inactivity Timeout to 30 Days",
                Category = "Network",
                Description =
                    "Sets JobInactivityTimeout=2592000 (30 days in seconds) in BITS policy. The default allows stale BITS jobs to persist for 90 days. Reducing to 30 days reclaims disk space from incomplete download caches sooner and prevents long-running abandoned transfer jobs.",
                Tags = ["bits", "background", "transfer", "policy", "cleanup"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(BitsPol, "JobInactivityTimeout", 2592000)],
                RemoveOps = [RegOp.DeleteValue(BitsPol, "JobInactivityTimeout")],
                DetectOps = [RegOp.CheckDword(BitsPol, "JobInactivityTimeout", 2592000)],
            },
            new TweakDef
            {
                Id = "bitspol-max-jobs-machine",
                Label = "Limit BITS Jobs to 50 per Machine",
                Category = "Network",
                Description =
                    "Sets MaxJobsPerMachine=50 in BITS policy. Default is 300 concurrent jobs per computer. Limiting to 50 prevents BITS storms where many applications queue simultaneous background downloads, competing for network and I/O resources.",
                Tags = ["bits", "background", "transfer", "policy", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(BitsPol, "MaxJobsPerMachine", 50)],
                RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxJobsPerMachine")],
                DetectOps = [RegOp.CheckDword(BitsPol, "MaxJobsPerMachine", 50)],
            },
            new TweakDef
            {
                Id = "bitspol-max-jobs-user",
                Label = "Limit BITS Jobs to 20 per User",
                Category = "Network",
                Description =
                    "Sets MaxJobsPerUser=20 in BITS policy. Default is 60 concurrent jobs per user. Capping at 20 ensures no single user account can saturate BITS with background transfers, which is especially relevant for multi-user terminal server environments.",
                Tags = ["bits", "background", "transfer", "policy", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(BitsPol, "MaxJobsPerUser", 20)],
                RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxJobsPerUser")],
                DetectOps = [RegOp.CheckDword(BitsPol, "MaxJobsPerUser", 20)],
            },
            new TweakDef
            {
                Id = "bitspol-max-files-per-job",
                Label = "Limit BITS Job to 100 Files",
                Category = "Network",
                Description =
                    "Sets MaxJobFilesPerJob=100 in BITS policy. Default is 200 files per BITS job. Reducing to 100 limits the blast radius of a misbehaving application that creates overly large BITS jobs and helps ensure all jobs can complete without exhausting I/O queue depth.",
                Tags = ["bits", "background", "transfer", "policy", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(BitsPol, "MaxJobFilesPerJob", 100)],
                RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxJobFilesPerJob")],
                DetectOps = [RegOp.CheckDword(BitsPol, "MaxJobFilesPerJob", 100)],
            },
            new TweakDef
            {
                Id = "bitspol-max-ranges-per-file",
                Label = "Limit BITS to 100 Byte Ranges per File",
                Category = "Network",
                Description =
                    "Sets MaxRangesPerFile=100 in BITS policy. Default is 500 byte ranges per file. Each range costs memory in the BITS service process. Limiting ranges reduces BITS memory overhead on machines with many concurrent background downloads from multi-part servers.",
                Tags = ["bits", "background", "transfer", "policy", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(BitsPol, "MaxRangesPerFile", 100)],
                RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxRangesPerFile")],
                DetectOps = [RegOp.CheckDword(BitsPol, "MaxRangesPerFile", 100)],
            },
            new TweakDef
            {
                Id = "bitspol-max-download-time-24h",
                Label = "Limit BITS Download Jobs to 24 Hours",
                Category = "Network",
                Description =
                    "Sets MaxDownloadTime=86400 (24 hours in seconds) in BITS policy. By default BITS has no wall-clock limit on download jobs. Setting a 24-hour maximum prevents stalled or hung BITS jobs from occupying an active transfer slot indefinitely.",
                Tags = ["bits", "background", "transfer", "policy", "timeout"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(BitsPol, "MaxDownloadTime", 86400)],
                RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxDownloadTime")],
                DetectOps = [RegOp.CheckDword(BitsPol, "MaxDownloadTime", 86400)],
            },
            new TweakDef
            {
                Id = "bitspol-internet-bandwidth-limit",
                Label = "Cap BITS Internet Bandwidth at 8 Mbps",
                Category = "Network",
                Description =
                    "Sets MaxInternetBandwidth=8192 (Kbps) in BITS policy. Prevents BITS background downloads from monopolising internet bandwidth. 8 Mbps is sufficient for most Windows Update payloads while leaving headroom for interactive network traffic.",
                Tags = ["bits", "background", "transfer", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(BitsPol, "MaxInternetBandwidth", 8192)],
                RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxInternetBandwidth")],
                DetectOps = [RegOp.CheckDword(BitsPol, "MaxInternetBandwidth", 8192)],
            },
            new TweakDef
            {
                Id = "bitspol-enable-bandwidth-throttle",
                Label = "Enable BITS Bandwidth Throttling",
                Category = "Network",
                Description =
                    "Sets EnableBITSMaxBandwidth=1 in BITS policy. Activates the BITS bandwidth throttle schedule, causing BITS to honour the MaxInternetBandwidth setting during the configured hours. Without this flag the bandwidth cap defined by the schedule has no effect.",
                Tags = ["bits", "background", "transfer", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(BitsPol, "EnableBITSMaxBandwidth", 1)],
                RemoveOps = [RegOp.DeleteValue(BitsPol, "EnableBITSMaxBandwidth")],
                DetectOps = [RegOp.CheckDword(BitsPol, "EnableBITSMaxBandwidth", 1)],
            },
            new TweakDef
            {
                Id = "bitspol-disable-peercaching-client",
                Label = "Disable BITS Peer Caching (Client)",
                Category = "Network",
                Description =
                    "Sets DisablePeerCachingClient=1 in BITS policy. Prevents this machine from downloading BITS content from peer computers on the LAN. Disabling peer-client ensures all BITS traffic goes through the legitimate server rather than potentially compromised peers.",
                Tags = ["bits", "peercache", "network", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(BitsPol, "DisablePeerCachingClient", 1)],
                RemoveOps = [RegOp.DeleteValue(BitsPol, "DisablePeerCachingClient")],
                DetectOps = [RegOp.CheckDword(BitsPol, "DisablePeerCachingClient", 1)],
            },
            new TweakDef
            {
                Id = "bitspol-disable-peercaching-server",
                Label = "Disable BITS Peer Caching (Server)",
                Category = "Network",
                Description =
                    "Sets DisablePeerCachingServer=1 in BITS policy. Prevents this machine from serving cached BITS content to other peers on the LAN. Disabling peer-server mode stops the machine from becoming an unintended content distribution node that consumes upload bandwidth.",
                Tags = ["bits", "peercache", "network", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(BitsPol, "DisablePeerCachingServer", 1)],
                RemoveOps = [RegOp.DeleteValue(BitsPol, "DisablePeerCachingServer")],
                DetectOps = [RegOp.CheckDword(BitsPol, "DisablePeerCachingServer", 1)],
            },
        ];
    }

    // ── BranchCache ──
    private static class _BranchCache
    {
        private const string Svc = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\Service";
        private const string Fetch = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\ContentFetch";
        private const string Retrieval = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\Retrieval";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "bc-enable-distributed-mode",
                Label = "Enable BranchCache in Distributed Cache Mode",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["branchcache", "network", "caching", "distributed", "performance"],
                Description =
                    "Enables BranchCache and configures it in Distributed Cache mode. Clients that have "
                    + "downloaded content from the main-office content server cache files locally and serve "
                    + "them to other clients on the same subnet. Reduces WAN bandwidth usage. Requires Win10+.",
                ApplyOps = [RegOp.SetDword(Svc, "Enable", 1), RegOp.SetDword(Svc, "PeerDistributionMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Svc, "Enable"), RegOp.DeleteValue(Svc, "PeerDistributionMode")],
                DetectOps = [RegOp.CheckDword(Svc, "PeerDistributionMode", 1)],
            },
            new TweakDef
            {
                Id = "bc-set-cache-25pct",
                Label = "Set BranchCache Cache to 25% of Disk",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["branchcache", "cache", "disk", "size", "performance"],
                Description =
                    "Sets the BranchCache client cache to occupy 25% of the total disk capacity. A larger "
                    + "cache improves hit rates at the cost of disk space. Windows default is 5%. Applies to "
                    + "the partition where Windows is installed.",
                ApplyOps = [RegOp.SetDword(Svc, "MaxCacheSizeAsPercentageOfDiskSpace", 25)],
                RemoveOps = [RegOp.DeleteValue(Svc, "MaxCacheSizeAsPercentageOfDiskSpace")],
                DetectOps = [RegOp.CheckDword(Svc, "MaxCacheSizeAsPercentageOfDiskSpace", 25)],
            },
            new TweakDef
            {
                Id = "bc-cap-cache-5gb",
                Label = "Cap BranchCache Absolute Cache at 5 GB",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["branchcache", "cache", "disk", "quota", "limit"],
                Description =
                    "Sets an absolute upper limit of 5 GB on the BranchCache client cache regardless of disk "
                    + "percentage. Prevents the cache from consuming excessive space on large drives. Works in "
                    + "conjunction with the percentage setting — whichever is smaller wins.",
                ApplyOps = [RegOp.SetDword(Svc, "MaxCacheSizeInGB", 5)],
                RemoveOps = [RegOp.DeleteValue(Svc, "MaxCacheSizeInGB")],
                DetectOps = [RegOp.CheckDword(Svc, "MaxCacheSizeInGB", 5)],
            },
            new TweakDef
            {
                Id = "bc-use-sha256-hashes",
                Label = "Use SHA-256 for BranchCache Content Hashes",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["branchcache", "security", "sha256", "hash", "integrity"],
                Description =
                    "Configures BranchCache to use SHA-256 (hash version 2) for content verification instead "
                    + "of the default SHA-1. Provides stronger content integrity guarantees. The content server "
                    + "and ALL clients must support the same hash version to inter-operate.",
                ApplyOps = [RegOp.SetDword(Svc, "HashVersion", 2)],
                RemoveOps = [RegOp.DeleteValue(Svc, "HashVersion")],
                DetectOps = [RegOp.CheckDword(Svc, "HashVersion", 2)],
            },
            new TweakDef
            {
                Id = "bc-enable-firewall-exceptions",
                Label = "Enable BranchCache Automatic Firewall Exceptions",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["branchcache", "firewall", "network", "ports", "automation"],
                Description =
                    "Automatically configures Windows Firewall to allow BranchCache traffic on standard ports "
                    + "(TCP 80 for HTTP mode, TCP/UDP 3702 and TCP 443 for WS-Discovery). Eliminates manual "
                    + "firewall rule creation at branch offices.",
                ApplyOps = [RegOp.SetDword(Svc, "FirewallPortSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(Svc, "FirewallPortSettings")],
                DetectOps = [RegOp.CheckDword(Svc, "FirewallPortSettings", 1)],
            },
            new TweakDef
            {
                Id = "bc-set-retrieval-latency-5s",
                Label = "Set BranchCache Retrieval Segment TTL to 5 Seconds",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["branchcache", "latency", "performance", "timeout", "retrieval"],
                Description =
                    "Limits BranchCache content retrieval segment time-to-live (TTL) to 5 seconds before "
                    + "falling back to the origin content server. Lower values reduce wait time when cache peers "
                    + "are slow or unresponsive. Default: 0 (no cap). Applies to distributed mode only.",
                ApplyOps = [RegOp.SetDword(Retrieval, "SegmentTTL", 5)],
                RemoveOps = [RegOp.DeleteValue(Retrieval, "SegmentTTL")],
                DetectOps = [RegOp.CheckDword(Retrieval, "SegmentTTL", 5)],
            },
            new TweakDef
            {
                Id = "bc-enable-hash-publication-smb",
                Label = "Enable BranchCache Hash Publication for SMB Shares",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["branchcache", "smb", "hash", "publication", "server"],
                Description =
                    "Enables content hash publication for SMB file shares, making content pre-hashed and "
                    + "immediately available for BranchCache clients without generating hashes on-demand. "
                    + "Best applied to file servers at the main office. Sets HashPublicationForPeerDist=1 "
                    + "and HashSupportVersion=2 for SHA-256.",
                ApplyOps = [RegOp.SetDword(Svc, "HashPublicationForPeerDist", 1), RegOp.SetDword(Svc, "HashSupportVersion", 2)],
                RemoveOps = [RegOp.DeleteValue(Svc, "HashPublicationForPeerDist"), RegOp.DeleteValue(Svc, "HashSupportVersion")],
                DetectOps = [RegOp.CheckDword(Svc, "HashPublicationForPeerDist", 1)],
            },
            new TweakDef
            {
                Id = "bc-prefer-hosted-cache-server",
                Label = "Prefer BranchCache Hosted Cache over Auto-Discovery",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["branchcache", "hosted", "discovery", "server", "preference"],
                Description =
                    "Configures clients to prefer the designated hosted cache server over automatic network "
                    + "discovery. Reduces broadcast traffic and ensures clients use the authoritative branch "
                    + "cache. Should be used together with the hosted cache mode setting.",
                ApplyOps = [RegOp.SetDword(Svc, "UseHostedCache", 1)],
                RemoveOps = [RegOp.DeleteValue(Svc, "UseHostedCache")],
                DetectOps = [RegOp.CheckDword(Svc, "UseHostedCache", 1)],
            },
            new TweakDef
            {
                Id = "bc-zero-initial-offering-delay",
                Label = "Eliminate BranchCache Initial Peer Offering Delay",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["branchcache", "caching", "initial", "delay", "optimization"],
                Description =
                    "Sets the initial peer offering delay to 0 seconds so BranchCache clients immediately "
                    + "offer locally cached segments to peers without any wait. Maximises peer-to-peer "
                    + "utilisation within the branch and minimises WAN round-trips for segments already "
                    + "present on local machines.",
                ApplyOps = [RegOp.SetDword(Fetch, "InitialOfferDelayInSeconds", 0)],
                RemoveOps = [RegOp.DeleteValue(Fetch, "InitialOfferDelayInSeconds")],
                DetectOps = [RegOp.CheckDword(Fetch, "InitialOfferDelayInSeconds", 0)],
            },
        ];
    }

    // ── BranchCachePolicy ──
    private static class _BranchCachePolicy
    {
        private const string SvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\Service";
        private const string SvcCfg = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\Service\Configuration";
        private const string HostedKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\HostedCache\Connection";
        private const string HashKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\Retrieval";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "branchcache-distributed-mode",
                    Label = "Set BranchCache to Distributed Mode",
                    Category = "Network",
                    Description =
                        "Configures BranchCache to operate in distributed (peer-to-peer) mode where clients share cached content with each other. Default: not configured.",
                    Tags = ["branchcache", "distributed", "p2p", "caching", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Clients share cached content directly; reduces server and WAN load.",
                    ApplyOps = [RegOp.SetDword(SvcCfg, "PreferredContentInformationVersion", 2)],
                    RemoveOps = [RegOp.DeleteValue(SvcCfg, "PreferredContentInformationVersion")],
                    DetectOps = [RegOp.CheckDword(SvcCfg, "PreferredContentInformationVersion", 2)],
                },
                new TweakDef
                {
                    Id = "branchcache-set-cache-percent",
                    Label = "Set BranchCache Disk Cache to 10 Percent",
                    Category = "Network",
                    Description = "Limits the BranchCache disk cache size to 10% of the data drive. Prevents runaway cache growth. Default: 5%.",
                    Tags = ["branchcache", "cache-size", "disk", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Cache limited to 10% of disk; balances caching benefit with storage use.",
                    ApplyOps = [RegOp.SetDword(SvcCfg, "SizePercent", 10)],
                    RemoveOps = [RegOp.DeleteValue(SvcCfg, "SizePercent")],
                    DetectOps = [RegOp.CheckDword(SvcCfg, "SizePercent", 10)],
                },
                new TweakDef
                {
                    Id = "branchcache-set-cache-age",
                    Label = "Set BranchCache Maximum Content Age to 28 Days",
                    Category = "Network",
                    Description =
                        "Sets cached content expiry to 28 days. Content older than this is evicted from the local cache. Default: 28 days (696 hours).",
                    Tags = ["branchcache", "cache-age", "expiry", "retention", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Stale cached content evicted after 28 days; may cause re-download of old files.",
                    ApplyOps = [RegOp.SetDword(SvcCfg, "MaxCacheAge", 672)],
                    RemoveOps = [RegOp.DeleteValue(SvcCfg, "MaxCacheAge")],
                    DetectOps = [RegOp.CheckDword(SvcCfg, "MaxCacheAge", 672)],
                },
                new TweakDef
                {
                    Id = "branchcache-enable-content-discovery",
                    Label = "Enable Automatic Hosted Cache Discovery",
                    Category = "Network",
                    Description =
                        "Enables automatic Service Connection Point (SCP) discovery for hosted cache servers. Clients auto-locate the nearest cache server. Default: disabled.",
                    Tags = ["branchcache", "hosted-cache", "discovery", "scp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clients discover hosted cache servers via AD; useful in multi-site deployments.",
                    ApplyOps = [RegOp.SetDword(HostedKey, "AutomaticHostedCacheDiscovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(HostedKey, "AutomaticHostedCacheDiscovery")],
                    DetectOps = [RegOp.CheckDword(HostedKey, "AutomaticHostedCacheDiscovery", 1)],
                },
                new TweakDef
                {
                    Id = "branchcache-enable-latency-detection",
                    Label = "Enable Network Latency Caching Threshold",
                    Category = "Network",
                    Description =
                        "Enables the BranchCache latency threshold — content is cached only when WAN round-trip time exceeds the configured threshold. Default: disabled (cache all).",
                    Tags = ["branchcache", "latency", "wan", "threshold", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Only caches content from slow WAN links; LAN-fetched content bypasses cache.",
                    ApplyOps = [RegOp.SetDword(SvcCfg, "EnableLatencyBasedCaching", 1)],
                    RemoveOps = [RegOp.DeleteValue(SvcCfg, "EnableLatencyBasedCaching")],
                    DetectOps = [RegOp.CheckDword(SvcCfg, "EnableLatencyBasedCaching", 1)],
                },
                new TweakDef
                {
                    Id = "branchcache-set-latency-threshold",
                    Label = "Set BranchCache Latency Threshold to 80ms",
                    Category = "Network",
                    Description =
                        "Sets the round-trip latency threshold at 80ms. WAN content served above this latency is cached; below this it is fetched live. Default: 80ms.",
                    Tags = ["branchcache", "latency", "threshold", "wan", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Caching only triggered for links slower than 80ms round-trip; LAN unaffected.",
                    ApplyOps = [RegOp.SetDword(SvcCfg, "LatencyThreshold", 80)],
                    RemoveOps = [RegOp.DeleteValue(SvcCfg, "LatencyThreshold")],
                    DetectOps = [RegOp.CheckDword(SvcCfg, "LatencyThreshold", 80)],
                },
                new TweakDef
                {
                    Id = "branchcache-enable-http-hash",
                    Label = "Enable HTTP Content Hash Generation",
                    Category = "Network",
                    Description =
                        "Enables hash generation for HTTP-based content served through BranchCache. Required for web-server content offloading. Default: disabled.",
                    Tags = ["branchcache", "http", "hash", "web", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Enables content verification for HTTP-cached files; slight CPU overhead on server.",
                    ApplyOps = [RegOp.SetDword(HashKey, "EnableHTTPHash", 1)],
                    RemoveOps = [RegOp.DeleteValue(HashKey, "EnableHTTPHash")],
                    DetectOps = [RegOp.CheckDword(HashKey, "EnableHTTPHash", 1)],
                },
                new TweakDef
                {
                    Id = "branchcache-enable-smb-hash",
                    Label = "Enable SMB Content Hash Generation",
                    Category = "Network",
                    Description =
                        "Enables hash generation for SMB/CIFS file shares. Required for file-server content caching via BranchCache. Default: disabled.",
                    Tags = ["branchcache", "smb", "file-share", "hash", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "File-share content verified and cached locally; minor CPU overhead on file server.",
                    ApplyOps = [RegOp.SetDword(HashKey, "EnableSMBHash", 1)],
                    RemoveOps = [RegOp.DeleteValue(HashKey, "EnableSMBHash")],
                    DetectOps = [RegOp.CheckDword(HashKey, "EnableSMBHash", 1)],
                },
                new TweakDef
                {
                    Id = "branchcache-enable-bits-hash",
                    Label = "Enable BITS Content Hash for BranchCache",
                    Category = "Network",
                    Description =
                        "Enables hash publication for Background Intelligent Transfer Service (BITS) downloads. WSUS and ConfigMgr content benefits from BranchCache. Default: disabled.",
                    Tags = ["branchcache", "bits", "wsus", "sccm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Windows Update and ConfigMgr content cached locally via BranchCache.",
                    ApplyOps = [RegOp.SetDword(HashKey, "EnableBITSHash", 1)],
                    RemoveOps = [RegOp.DeleteValue(HashKey, "EnableBITSHash")],
                    DetectOps = [RegOp.CheckDword(HashKey, "EnableBITSHash", 1)],
                },
            ];
    }

    // ── CacheManagerPolicy ──
    private static class _CacheManagerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CacheManager";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cachemgr-disable-offline-cache",
                Label = "Disable Offline Files Caching (Client-Side Caching)",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Client-side caching stores network file share contents locally for offline access which creates copies of potentially sensitive corporate data. Disabling offline caching prevents synchronized copies of network files from being stored on endpoint local disks. Cached files can persist after employees leave the organization or on endpoints that may be lost or stolen. Offline caching can store sensitive documents outside of DLP controls and monitoring systems. Large offline caches can consume significant disk space and may contain data that would otherwise remain on protected file servers. Organizations with DLP requirements for data-at-rest should evaluate whether offline caching is consistent with their data handling policies.",
                Tags = ["cache", "offline-files", "csc", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOfflineCache", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOfflineCache")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOfflineCache", 1)],
            },
            new TweakDef
            {
                Id = "cachemgr-encrypt-offline-files",
                Label = "Encrypt Offline Files Cache",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Encrypting the offline files cache protects locally stored copies of network files if the endpoint disk is accessed without proper authentication. Offline file cache encryption uses EFS or BitLocker protection to ensure that cached sensitive files are not accessible if the disk is removed. Unencrypted offline caches of sensitive documents can be accessed by attackers who gain physical access or offline disk access. Cache encryption ensures that even if an endpoint is lost or stolen the offline file cache cannot be read without the correct user credentials. Encrypting offline caches complements full-disk encryption by providing file-level protection within the cache directory. Organizations deploying BitLocker should also encrypt offline caches to prevent data exposure through cache files during pre-boot states.",
                Tags = ["cache", "offline-files", "encryption", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EncryptOfflineFilesCache", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EncryptOfflineFilesCache")],
                DetectOps = [RegOp.CheckDword(Key, "EncryptOfflineFilesCache", 1)],
            },
            new TweakDef
            {
                Id = "cachemgr-limit-cache-size",
                Label = "Restrict Offline Files Cache Size",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Limiting the offline files cache size prevents excessive accumulation of cached network files that consume disk space and expand the attack surface. A restricted cache size ensures that only the most recently accessed network files are retained in the local cache. Unlimited caches can consume gigabytes of disk space on endpoints with large offline file synchronization policies. Cache size limits encourage users to rely on network access rather than local copies which reduces data leakage risk. Monitoring cache utilization against defined limits helps identify endpoints with unusual offline file activity indicating policy violations. Organizations should set cache size limits based on endpoint disk capacity and typical offline file usage requirements.",
                Tags = ["cache", "offline-files", "disk-space", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LimitCacheSize", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LimitCacheSize")],
                DetectOps = [RegOp.CheckDword(Key, "LimitCacheSize", 1)],
            },
            new TweakDef
            {
                Id = "cachemgr-disable-transparent-cache",
                Label = "Disable Transparent Caching of Remote Files",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Transparent caching improves performance for frequently accessed network files by storing them locally without explicit offline synchronization. Disabling transparent caching prevents implicit local copies of network files from being created on endpoint storage. Transparent caching can create unintended local copies of sensitive files accessed from network shares without user awareness. Files in the transparent cache bypass DLP policies that monitor network file access and apply to only the network path. Transparent caching was designed for WAN optimization but creates data residency concerns when sensitive files should remain on controlled file servers. Disabling transparent caching ensures that all access to network files goes through the network share with proper access controls and monitoring.",
                Tags = ["cache", "transparent-cache", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTransparentCaching", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTransparentCaching")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTransparentCaching", 1)],
            },
            new TweakDef
            {
                Id = "cachemgr-disable-browser-cache-sharing",
                Label = "Disable IE/Edge Browser Cache External Sharing",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Browser cache sharing with external applications can allow third-party tools to access cached browser content including session tokens and cookies. Disabling external cache sharing prevents applications other than the browser from reading the browser's local cache. Browser caches can contain sensitive information including recently accessed URLs, form data, cached credentials, and session tokens. External access to browser cache files can be exploited by malicious applications to harvest credentials without process injection. Browser cache isolation is part of overall browser security hardening alongside profile separation and sandbox controls. This policy restricts which processes can access browser cache directories through Windows filesystem permissions and cache API controls.",
                Tags = ["cache", "browser", "cookies", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBrowserCacheExternalSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBrowserCacheExternalSharing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBrowserCacheExternalSharing", 1)],
            },
            new TweakDef
            {
                Id = "cachemgr-clear-cache-on-logoff",
                Label = "Clear Offline Files Cache on User Logoff",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Clearing the offline files cache on user logoff removes locally cached network file copies when the user session ends. Cache clearing on logoff ensures that sensitive cached files do not persist on shared or temporary workstations between user sessions. Kiosk endpoints and shared workstations commonly cache credentials and files from previous users when sessions are improperly handled. Logoff cache clearing combined with user profile deletion provides clean slate isolation between user sessions. Clearing caches on logoff may increase network load as files need to be re-synchronized on next logon but ensures fresh data access. This policy is most important for high-turnover environments like call centers, shared workstations, and terminal services environments.",
                Tags = ["cache", "logoff", "cleanup", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ClearCacheOnLogoff", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ClearCacheOnLogoff")],
                DetectOps = [RegOp.CheckDword(Key, "ClearCacheOnLogoff", 1)],
            },
            new TweakDef
            {
                Id = "cachemgr-disable-pin-for-readonly",
                Label = "Disable Pinning of Read-Only Files for Offline",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Pinning read-only network files for offline access creates persistent local copies of files that may contain sensitive information. Disabling read-only file pinning prevents users from creating offline copies of files marked as accessible to their accounts. Read-only access to sensitive documents should remain controlled at the file server level through access controls not through offline copies. Pinned offline files create a persistent local copy that survives beyond the period when the user's access should be valid. Restricting pinning permissions reduces the risk of data exposure from offline copies on endpoints that are later lost or repurposed. Users requiring offline access to specific documents should follow formal data handling procedures with approved encryption and controls.",
                Tags = ["cache", "pinning", "offline-files", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePinForReadOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePinForReadOnly")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePinForReadOnly", 1)],
            },
            new TweakDef
            {
                Id = "cachemgr-require-cache-encryption-before-sync",
                Label = "Require Cache Encryption Before Offline Sync",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Requiring cache encryption before offline synchronization ensures that the local cache directory is protected before files are synchronized into it. Pre-sync encryption requirement prevents offline file synchronization on endpoints that do not have the cache encryption configured and verified. This policy prevents unencrypted offline caches from being created on endpoints that lack proper encryption key setup. Ensuring cache encryption is enforced before sync prevents a window of exposure between cache creation and encryption activation. The policy provides a mandatory checkpoint that blocks offline file caching on non-compliant endpoints until encryption is confirmed. Organizations with mandatory encryption policies benefit from this pre-sync encryption check as a technical enforcement mechanism.",
                Tags = ["cache", "encryption", "sync", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireEncryptionBeforeSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireEncryptionBeforeSync")],
                DetectOps = [RegOp.CheckDword(Key, "RequireEncryptionBeforeSync", 1)],
            },
            new TweakDef
            {
                Id = "cachemgr-disable-admin-pin",
                Label = "Disable Administrator-Pinned Offline Files",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Administrator-pinned files allow IT administrators to force files to be available offline for all users of a computer through Group Policy. Disabling administrator-pinned offline files prevents IT policies from pushing potentially large or sensitive file sets to local endpoint caches. Admin-pinned files can be used to distribute configuration files but also create local copies of sensitive data outside user control. Inadvertent admin pinning of large directory trees can consume significant disk space on managed endpoints. Organizations should review admin-pinned file policies to ensure only appropriate files are being forced offline and disable bulk pinning. Removing admin-pinned file policies reduces the chance of unintended data accumulation on endpoint local storage.",
                Tags = ["cache", "admin-pinning", "offline-files", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAdminPinning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAdminPinning")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAdminPinning", 1)],
            },
            new TweakDef
            {
                Id = "cachemgr-audit-offline-access",
                Label = "Enable Offline Files Access Audit Logging",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Offline files access audit logging records when users access cached files providing visibility into offline data access patterns. Enabling offline cache access auditing generates security events for cache file reads and writes during sessions when network connectivity is unavailable. Audit records of offline access help identify access to sensitive cached files from endpoints in unexpected locations. Offline access auditing supports DLP investigations by providing evidence of which files were accessed during disconnected periods. Security auditing of offline files should be synchronized with the audit data collected on the corresponding file servers. Offline access audit events should be forwarded to SIEM infrastructure when the endpoint reconnects to the network.",
                Tags = ["cache", "audit", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditOfflineFileAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditOfflineFileAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditOfflineFileAccess", 1)],
            },
        ];
    }

    // ── ConnectedCachePolicy ──
    private static class _ConnectedCachePolicy
    {
        private const string MccKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ConnectedCache";
        private const string MccClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ConnectedCache\Client";
        private const string DeliveryOptKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "mcc-disable-connected-cache-client",
                    Label = "Disable Microsoft Connected Cache Client",
                    Category = "Network",
                    Description =
                        "Prevents Windows from acting as a client to Microsoft Connected Cache (MCC) servers. "
                        + "The device will not retrieve Windows Update, Delivery Optimization, or Microsoft 365 content from local MCC nodes.",
                    Tags = ["mcc", "connected-cache", "delivery-optimization", "bandwidth", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 19041,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices will download update content directly from Microsoft CDN instead of from an on-premises MCC node; "
                        + "may increase internet bandwidth consumption.",
                    RegistryKeys = [MccKey],
                    ApplyOps = [RegOp.SetDword(MccKey, "DisableMicrosoftConnectedCache", 1)],
                    RemoveOps = [RegOp.DeleteValue(MccKey, "DisableMicrosoftConnectedCache")],
                    DetectOps = [RegOp.CheckDword(MccKey, "DisableMicrosoftConnectedCache", 1)],
                },
                new TweakDef
                {
                    Id = "mcc-restrict-to-enterprise-nodes",
                    Label = "Restrict Connected Cache to Enterprise MCC Nodes Only",
                    Category = "Network",
                    Description =
                        "Configures Windows to only retrieve cached content from administrator-specified "
                        + "Microsoft Connected Cache nodes, preventing download from unapproved or public MCC servers.",
                    Tags = ["mcc", "connected-cache", "enterprise", "node-restriction", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 19041,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Ensures update traffic goes through the organisation's approved cache infrastructure, "
                        + "supporting network segmentation and compliance.",
                    RegistryKeys = [MccKey],
                    ApplyOps = [RegOp.SetDword(MccKey, "RestrictToEnterpriseMCCNodes", 1)],
                    RemoveOps = [RegOp.DeleteValue(MccKey, "RestrictToEnterpriseMCCNodes")],
                    DetectOps = [RegOp.CheckDword(MccKey, "RestrictToEnterpriseMCCNodes", 1)],
                },
                new TweakDef
                {
                    Id = "mcc-set-cache-node-hostname",
                    Label = "Set Connected Cache Node Hostname (Configures MCC Server)",
                    Category = "Network",
                    Description =
                        "Specifies the FQDN or IP address of the Microsoft Connected Cache node that this device should use "
                        + "as its primary cache source for Windows Update and Delivery Optimization content.",
                    Tags = ["mcc", "connected-cache", "hostname", "enterprise", "configuration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 19041,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Directs update traffic to the organisation's on-premises MCC node, reducing internet egress for update downloads.",
                    RegistryKeys = [MccClientKey],
                    ApplyOps = [RegOp.SetString(MccClientKey, "MCCNodeHostname", "mcc-server.contoso.com")],
                    RemoveOps = [RegOp.DeleteValue(MccClientKey, "MCCNodeHostname")],
                    DetectOps = [RegOp.CheckString(MccClientKey, "MCCNodeHostname", "mcc-server.contoso.com")],
                },
                new TweakDef
                {
                    Id = "mcc-limit-background-bandwidth-percent",
                    Label = "Limit Delivery Optimization Background Bandwidth (50%)",
                    Category = "Network",
                    Description =
                        "Caps background Delivery Optimization download transfers at 50% of the measured internet bandwidth, "
                        + "preventing bulk update downloads from saturating the connection during working hours.",
                    Tags = ["mcc", "delivery-optimization", "bandwidth", "throttle", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 19041,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Keeps 50% of bandwidth available for interactive use while background updates download. "
                        + "Adjust the value to match your SLA.",
                    RegistryKeys = [DeliveryOptKey],
                    ApplyOps = [RegOp.SetDword(DeliveryOptKey, "DOPercentageMaxBackgroundBandwidth", 50)],
                    RemoveOps = [RegOp.DeleteValue(DeliveryOptKey, "DOPercentageMaxBackgroundBandwidth")],
                    DetectOps = [RegOp.CheckDword(DeliveryOptKey, "DOPercentageMaxBackgroundBandwidth", 50)],
                },
                new TweakDef
                {
                    Id = "mcc-limit-foreground-bandwidth-percent",
                    Label = "Limit Delivery Optimization Foreground Bandwidth (80%)",
                    Category = "Network",
                    Description =
                        "Caps foreground Delivery Optimization download transfers at 80% of the measured bandwidth, "
                        + "allowing user-initiated downloads like Microsoft Store apps or Windows Updates to proceed quickly "
                        + "without total saturation.",
                    Tags = ["mcc", "delivery-optimization", "bandwidth", "throttle", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 19041,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Provides a headroom buffer for concurrent network use during explicit update downloads triggered by the user.",
                    RegistryKeys = [DeliveryOptKey],
                    ApplyOps = [RegOp.SetDword(DeliveryOptKey, "DOPercentageMaxForegroundBandwidth", 80)],
                    RemoveOps = [RegOp.DeleteValue(DeliveryOptKey, "DOPercentageMaxForegroundBandwidth")],
                    DetectOps = [RegOp.CheckDword(DeliveryOptKey, "DOPercentageMaxForegroundBandwidth", 80)],
                },
                new TweakDef
                {
                    Id = "mcc-set-cache-drive-size-gb",
                    Label = "Set Connected Cache Storage Limit (20 GB)",
                    Category = "Network",
                    Description =
                        "Sets the maximum disk space available to the Microsoft Connected Cache client for storing downloaded content packages. "
                        + "Prevents the cache from consuming unpredictable amounts of the system drive.",
                    Tags = ["mcc", "connected-cache", "cache-size", "disk", "configuration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 19041,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Bounds cache growth to 20 GB; adjust based on available disk space and update volume in your environment.",
                    RegistryKeys = [MccClientKey],
                    ApplyOps = [RegOp.SetDword(MccClientKey, "MaxCacheSizeGB", 20)],
                    RemoveOps = [RegOp.DeleteValue(MccClientKey, "MaxCacheSizeGB")],
                    DetectOps = [RegOp.CheckDword(MccClientKey, "MaxCacheSizeGB", 20)],
                },
                new TweakDef
                {
                    Id = "mcc-disable-cache-on-metered-connection",
                    Label = "Disable MCC Downloads on Metered Connections",
                    Category = "Network",
                    Description =
                        "Prevents Delivery Optimization and Microsoft Connected Cache downloads from occurring when the network connection "
                        + "is detected as metered (e.g., mobile data). Downloads resume automatically on unmetered connections.",
                    Tags = ["mcc", "delivery-optimization", "metered", "data-saver", "mobile"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 19041,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Prevents unexpected data charges on mobile hotspots, cellular-connected laptops, "
                        + "or any connection marked metered in Windows.",
                    RegistryKeys = [MccClientKey],
                    ApplyOps = [RegOp.SetDword(MccClientKey, "DisableOnMeteredConnections", 1)],
                    RemoveOps = [RegOp.DeleteValue(MccClientKey, "DisableOnMeteredConnections")],
                    DetectOps = [RegOp.CheckDword(MccClientKey, "DisableOnMeteredConnections", 1)],
                },
            ];
    }

    // ── DataSensePolicy ──
    private static class _DataSensePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataSense";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dtsense-disable-traffic-shaper",
                Label = "Disable Data Sense Traffic Shaper",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Data Sense traffic shaping throttles background network activity when the device approaches data usage limits on metered connections. Enterprise workstations typically operate on unmetered corporate networks where traffic shaping provides no benefit. Disabling the traffic shaper ensures consistent network throughput for all applications regardless of metered connection state. Background data transfers including Windows Update and application synchronization will proceed at full speed. This setting prevents unexpected performance degradation on networks incorrectly classified as metered. Administrators managing corporate networks should ensure connection profiles are correctly configured as unmetered.",
                Tags = ["data-sense", "network", "bandwidth", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TrafficShaperEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "TrafficShaperEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "TrafficShaperEnabled", 0)],
            },
            new TweakDef
            {
                Id = "dtsense-restrict-background-data",
                Label = "Restrict Background Data Usage",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Background data usage restriction limits which applications can transfer data while running in the background on metered connections. Restricting background data usage ensures that critical foreground applications receive priority over background synchronization tasks. This policy is particularly valuable on mobile workstations that connect to metered LTE or satellite networks. Applications performing background telemetry, update downloads, and cloud synchronization are subject to this restriction. Domain-joined machines on corporate networks may still benefit from this setting as a defense against uncontrolled background transfers. The restriction applies to applications respecting the metered connection API and can be supplemented with firewall rules.",
                Tags = ["data-sense", "background", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BackgroundDataUsageRestricted", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BackgroundDataUsageRestricted")],
                DetectOps = [RegOp.CheckDword(Key, "BackgroundDataUsageRestricted", 1)],
            },
            new TweakDef
            {
                Id = "dtsense-disable-usage-tracking",
                Label = "Disable Data Usage Tracking",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Data usage tracking monitors per-application and per-connection data consumption and stores this information in the Data Sense repository. Disabling tracking prevents Windows from maintaining a local data usage history for metered connections. Organizations with dedicated network monitoring solutions have no need for Windows to duplicate this tracking locally. The tracking data can be exposed to Microsoft through telemetry channels, representing a minor privacy consideration. Disabling this feature reduces the I/O overhead associated with maintaining usage logs per network session. Network monitoring requirements are better served by enterprise-grade infrastructure solutions rather than client-side tracking.",
                Tags = ["data-sense", "tracking", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DataUsageTrackingEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DataUsageTrackingEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "DataUsageTrackingEnabled", 0)],
            },
            new TweakDef
            {
                Id = "dtsense-disable-hotspot-throttle",
                Label = "Disable Mobile Hotspot Throttle",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The mobile hotspot throttle reduces network buffer sizes and download rates when the device is connected to a mobile hotspot to conserve cellular data. Corporate devices connecting to approved mobile hotspots for remote work may experience degraded performance due to unnecessary throttling. Disabling hotspot throttle allows full use of available bandwidth when connecting through a mobile hotspot. This is particularly important for developers, analysts, and field personnel who rely on mobile connectivity for data-intensive work. The policy does not affect cellular data billing because billing is managed exclusively by the carrier. Network administrators should combine this setting with appropriate mobile device management policies.",
                Tags = ["data-sense", "hotspot", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MobileHotspotThrottleEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MobileHotspotThrottleEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "MobileHotspotThrottleEnabled", 0)],
            },
            new TweakDef
            {
                Id = "dtsense-disable-telemetry",
                Label = "Disable Data Sense Telemetry",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Data Sense telemetry transmits information about network usage patterns and connection types to Microsoft for product improvement purposes. This data can include information about which applications consume the most bandwidth on metered connections. Disabling Data Sense telemetry prevents this usage telemetry from leaving the enterprise boundary. Regulated industries handling sensitive data have obligations to minimize external telemetry data flows. The telemetry collection does not affect any Data Sense functionality or local network management capabilities. Administrators can achieve equivalent insights through internal network monitoring infrastructure.",
                Tags = ["data-sense", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DataSenseTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DataSenseTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DataSenseTelemetry", 0)],
            },
            new TweakDef
            {
                Id = "dtsense-disable-auto-data-saving",
                Label = "Disable Auto Data Saving Mode",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Automatic data saving mode activates when the device approaches its configured data limit, restricting background transfers and reducing image quality. Enterprise devices operating within managed network environments typically do not require automatic data saving behavior. Disabling automatic data saving ensures consistent application performance that is not disrupted by data limit thresholds. Background services critical to business operations such as backup agents and security scanners continue running unrestricted. This setting provides more predictable behavior for services that require reliable network access. Administrators managing enterprise networks should implement quota policies at the network infrastructure level instead.",
                Tags = ["data-sense", "data-saving", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoDataSaving", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoDataSaving")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoDataSaving", 1)],
            },
            new TweakDef
            {
                Id = "dtsense-zero-metered-threshold",
                Label = "Set Metered Connection Threshold to Zero",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "The metered connection threshold determines the data usage percentage at which Data Sense begins applying stricter background transfer restrictions. Setting this threshold to zero effectively disables threshold-triggered restrictions without fully disabling the Data Sense feature. This prevents Data Sense from automatically engaging aggressive restrictions at any data usage level. Enterprise environments with unmetered connections benefit from ensuring no threshold-based restrictions are applied. Background services and synchronization tasks remain unaffected by threshold triggers. This setting complements other Data Sense policy configurations for comprehensive network behavior control.",
                Tags = ["data-sense", "metered", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MeteredConnectionThreshold", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MeteredConnectionThreshold")],
                DetectOps = [RegOp.CheckDword(Key, "MeteredConnectionThreshold", 0)],
            },
            new TweakDef
            {
                Id = "dtsense-disable-connected-standby-data",
                Label = "Disable Connected Standby Data Usage",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Connected standby allows modern Windows devices to maintain network connectivity during sleep states to receive notifications and sync data. Data transferred during connected standby can consume metered data allowances without user awareness. Disabling connected standby data usage prevents background network activity while the device screen is off. Enterprise users on metered connections benefit from predictable data consumption that only occurs during active use sessions. Battery-powered devices additionally benefit from reduced power drain caused by background network activity during sleep. Applications requiring real-time push notifications should be evaluated for compatibility with this setting before deployment.",
                Tags = ["data-sense", "standby", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableConnectedStandbyDataUsage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableConnectedStandbyDataUsage")],
                DetectOps = [RegOp.CheckDword(Key, "DisableConnectedStandbyDataUsage", 1)],
            },
            new TweakDef
            {
                Id = "dtsense-disable-notifications",
                Label = "Disable Data Sense Notifications",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Data Sense generates notifications alerting users when they approach or exceed configured data usage thresholds on metered connections. These notifications are not relevant for enterprise devices operating on unmetered corporate networks. Disabling Data Sense notifications reduces notification noise and prevents user confusion about data limits on managed networks. The notifications cannot trigger any automatic remediation actions and are purely informational. Enterprise environments with dedicated bandwidth management solutions have superior alerting mechanisms. Suppressing these notifications has no effect on network connectivity or data transfer operations.",
                Tags = ["data-sense", "notifications", "ui", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDataSenseNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDataSenseNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDataSenseNotifications", 1)],
            },
            new TweakDef
            {
                Id = "dtsense-disable-feature",
                Label = "Disable Data Sense Feature",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Data Sense is a Windows feature that monitors and manages network data usage to help users avoid exceeding metered connection limits. Enterprise devices on corporate networks typically operate outside the scope for which Data Sense was designed. Disabling the Data Sense feature removes the data usage monitoring overlay and its associated background services. Network resource management in enterprise environments is handled at the infrastructure level through switches, routers, and DLP solutions. Disabling the feature reduces the attack surface by removing a component that interacts with network traffic metadata. All network connectivity and data transfer capabilities remain fully functional when Data Sense is disabled.",
                Tags = ["data-sense", "feature", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDataSenseFeature", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDataSenseFeature")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDataSenseFeature", 1)],
            },
        ];
    }

    // ── DataUsageMeteringPolicy ──
    private static class _DataUsageMeteringPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataUsage";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "datuse-disable-background-data",
                    Label = "Block Background Data on Metered Connections",
                    Category = "Network",
                    Description =
                        "Prevents Windows apps from using background data when the network connection is marked as metered. Reduces unintended data consumption on mobile broadband or limited-data plans. Default: not enforced. Recommended: 1.",
                    Tags = ["data-usage", "metered", "background", "cellular", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Background app refresh, live tile updates, and sync are suspended on metered connections.",
                    ApplyOps = [RegOp.SetDword(Key, "BackgroundDataUsage", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BackgroundDataUsage")],
                    DetectOps = [RegOp.CheckDword(Key, "BackgroundDataUsage", 0)],
                },
                new TweakDef
                {
                    Id = "datuse-disable-automatic-roaming-data",
                    Label = "Block Automatic Data Use While Roaming",
                    Category = "Network",
                    Description =
                        "Prevents Windows from automatically sending or receiving data while the device is roaming on a cellular network. Eliminates surprise roaming charges. Default: not restricted. Recommended: 1.",
                    Tags = ["data-usage", "roaming", "cellular", "cost", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Data is fully blocked when roaming; users must manually enable roaming data if needed.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowRoamingData", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowRoamingData")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowRoamingData", 0)],
                },
                new TweakDef
                {
                    Id = "datuse-enforce-data-limit-warning",
                    Label = "Enforce Data Usage Warning at 80% Limit",
                    Category = "Network",
                    Description =
                        "Triggers a system notification when the device has consumed 80% of the configured data usage limit. Early warning helps users avoid plan overages. Default: not configured. Recommended: 80.",
                    Tags = ["data-usage", "warning", "limit", "cellular", "notification", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "A notification appears when 80% of the data plan is consumed.",
                    ApplyOps = [RegOp.SetDword(Key, "DataLimitWarningPercent", 80)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DataLimitWarningPercent")],
                    DetectOps = [RegOp.CheckDword(Key, "DataLimitWarningPercent", 80)],
                },
                new TweakDef
                {
                    Id = "datuse-disable-store-metered-updates",
                    Label = "Block Microsoft Store Updates Over Metered Connections",
                    Category = "Network",
                    Description =
                        "Prevents the Microsoft Store from downloading app updates automatically when connected via a metered network. Avoids large background downloads on limited plans. Default: not restricted. Recommended: 1.",
                    Tags = ["data-usage", "store", "metered", "updates", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Microsoft Store auto-updates suspended on metered connections; updates occur only on unmetered Wi-Fi.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockStoreUpdatesOnMeteredConn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockStoreUpdatesOnMeteredConn")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockStoreUpdatesOnMeteredConn", 1)],
                },
                new TweakDef
                {
                    Id = "datuse-disable-usage-telemetry-upload",
                    Label = "Block Data Usage Telemetry Upload",
                    Category = "Network",
                    Description =
                        "Stops Windows from uploading data usage statistics (per-app bandwidth consumption) to Microsoft telemetry services. Prevents sending usage patterns off-device. Default: upload enabled. Recommended: 1.",
                    Tags = ["data-usage", "telemetry", "privacy", "upload", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Per-app data usage telemetry is not uploaded to Microsoft cloud services.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUsageTelemetryUpload", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUsageTelemetryUpload")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUsageTelemetryUpload", 1)],
                },
                new TweakDef
                {
                    Id = "datuse-set-default-metered",
                    Label = "Mark New Wi-Fi Connections as Metered by Default",
                    Category = "Network",
                    Description =
                        "Sets all new Wi-Fi connections to metered by default, automatically activating bandwidth-saving restrictions. Useful for laptop fleets that frequently connect to mobile hotspots. Default: not metered. Recommended: when roaming is common.",
                    Tags = ["data-usage", "metered", "wifi", "default", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "All new Wi-Fi adapters default to metered; users can manually mark specific networks as unmetered.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultToMeteredConnection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultToMeteredConnection")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultToMeteredConnection", 1)],
                },
                new TweakDef
                {
                    Id = "datuse-disable-cost-based-app-limits",
                    Label = "Disable Cost-Based App Background Limits",
                    Category = "Network",
                    Description =
                        "Disables the automatic cost-awareness throttling that restricts background-capable apps based on connection cost (e.g., fixed vs. variable plan). Allows apps to run unrestricted on all connections. Default: cost-aware throttling active. Recommended: 0 on unlimited plans.",
                    Tags = ["data-usage", "cost", "background", "apps", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Apps no longer throttle themselves based on connection cost tier; background activity unrestricted.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCostBasedThrottling", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCostBasedThrottling")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCostBasedThrottling", 1)],
                },
                new TweakDef
                {
                    Id = "datuse-block-wifisense-hotspot-sharing",
                    Label = "Block Wi-Fi Sense Hotspot Data Sharing",
                    Category = "Network",
                    Description =
                        "Prevents Wi-Fi Sense from sharing mobile hotspot connection credentials with contacts and social networks. Stops unintended bandwidth sharing over a data plan. Default: not restricted. Recommended: 1.",
                    Tags = ["data-usage", "wifi-sense", "hotspot", "sharing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi Sense hotspot credential sharing is disabled; mobile data is not shared with contacts.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockHotspotSharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockHotspotSharing")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockHotspotSharing", 1)],
                },
                new TweakDef
                {
                    Id = "datuse-monthly-data-limit-mb",
                    Label = "Set Monthly Cellular Data Limit (5120 MB)",
                    Category = "Network",
                    Description =
                        "Configures the monthly cellular data budget to 5120 MB (5 GB). Windows tracks usage against this limit and triggers warnings and restrictions when approaching/exceeding it. Default: not set. Recommended: set per plan size.",
                    Tags = ["data-usage", "limit", "cellular", "budget", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Monthly data budget is 5 GB; Windows shows remaining budget and restricts usage near the limit.",
                    ApplyOps = [RegOp.SetDword(Key, "MonthlyDataLimitMB", 5120)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MonthlyDataLimitMB")],
                    DetectOps = [RegOp.CheckDword(Key, "MonthlyDataLimitMB", 5120)],
                },
                new TweakDef
                {
                    Id = "datuse-reset-limit-on-cycle",
                    Label = "Auto-Reset Data Counter on Billing Cycle",
                    Category = "Network",
                    Description =
                        "Enables automatic reset of the data usage counter at the beginning of each billing cycle (configured per adapter). Ensures the usage counter aligns with the carrier billing period. Default: not configured. Recommended: 1.",
                    Tags = ["data-usage", "reset", "billing-cycle", "cellular", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Data usage counter resets automatically each billing cycle; consistent with carrier accounting.",
                    ApplyOps = [RegOp.SetDword(Key, "AutoResetOnBillingCycle", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutoResetOnBillingCycle")],
                    DetectOps = [RegOp.CheckDword(Key, "AutoResetOnBillingCycle", 1)],
                },
            ];
    }

    // ── DeliveryOptimizationPolicy ──
    private static class _DeliveryOptimizationPolicy
    {
        private const string Do = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "doptpol-min-background-qos",
                Label = "Set DO Minimum Background Download QoS",
                Category = "Network",
                Description =
                    "Sets the minimum download speed (kbps) for Delivery Optimization background downloads to 500 kbps. Prevents DO from saturating network during background updates. Default: no limit. Recommended: 500.",
                Tags = ["delivery-optimization", "bandwidth", "background", "qos"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Do],
                ApplyOps = [RegOp.SetDword(Do, "DOMinBackgroundQos", 500)],
                RemoveOps = [RegOp.DeleteValue(Do, "DOMinBackgroundQos")],
                DetectOps = [RegOp.CheckDword(Do, "DOMinBackgroundQos", 500)],
            },
            new TweakDef
            {
                Id = "doptpol-max-upload-bandwidth",
                Label = "Limit DO Upload Bandwidth to 10%",
                Category = "Network",
                Description =
                    "Caps Delivery Optimization upload bandwidth to 10% of available bandwidth. Prevents DO peering from consuming upstream bandwidth on metered or shared connections. Default: 0 (no limit). Recommended: 10.",
                Tags = ["delivery-optimization", "upload", "bandwidth", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Do],
                ApplyOps = [RegOp.SetDword(Do, "DOMaxUploadBandwidth", 10)],
                RemoveOps = [RegOp.DeleteValue(Do, "DOMaxUploadBandwidth")],
                DetectOps = [RegOp.CheckDword(Do, "DOMaxUploadBandwidth", 10)],
            },
            new TweakDef
            {
                Id = "doptpol-max-cache-size",
                Label = "Limit DO Cache to 5% of Disk",
                Category = "Network",
                Description =
                    "Limits the Delivery Optimization disk cache to 5% of the drive. Prevents DO from consuming excessive disk space on smaller drives. Default: 20%. Recommended: 5.",
                Tags = ["delivery-optimization", "disk", "cache"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Do],
                ApplyOps = [RegOp.SetDword(Do, "DOMaxCacheSize", 5)],
                RemoveOps = [RegOp.DeleteValue(Do, "DOMaxCacheSize")],
                DetectOps = [RegOp.CheckDword(Do, "DOMaxCacheSize", 5)],
            },
            new TweakDef
            {
                Id = "doptpol-absolute-max-cache-size",
                Label = "Cap DO Absolute Cache Size to 1 GB",
                Category = "Network",
                Description =
                    "Sets an absolute 1 024 MB cap on the Delivery Optimization cache regardless of disk size. Prevents DO from accumulating large caches on high-capacity drives. Default: 10 240 MB. Recommended: 1024.",
                Tags = ["delivery-optimization", "disk", "cache", "storage"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Do],
                ApplyOps = [RegOp.SetDword(Do, "DOAbsoluteMaxCacheSize", 1024)],
                RemoveOps = [RegOp.DeleteValue(Do, "DOAbsoluteMaxCacheSize")],
                DetectOps = [RegOp.CheckDword(Do, "DOAbsoluteMaxCacheSize", 1024)],
            },
            new TweakDef
            {
                Id = "doptpol-min-disk-size-allowed",
                Label = "Require 32 GB Disk for DO Caching",
                Category = "Network",
                Description =
                    "Prevents Delivery Optimization caching on drives smaller than 32 GB. Protects limited-storage devices from DO disk pressure. Default: no minimum. Recommended: 32768 MB.",
                Tags = ["delivery-optimization", "disk", "cache", "storage"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Do],
                ApplyOps = [RegOp.SetDword(Do, "DOMinDiskSizeAllowedToCaches", 32768)],
                RemoveOps = [RegOp.DeleteValue(Do, "DOMinDiskSizeAllowedToCaches")],
                DetectOps = [RegOp.CheckDword(Do, "DOMinDiskSizeAllowedToCaches", 32768)],
            },
            new TweakDef
            {
                Id = "doptpol-min-ram-allowed",
                Label = "Require 4 GB RAM for DO Peering",
                Category = "Network",
                Description =
                    "Prevents Delivery Optimization peer-to-peer upload on devices with less than 4 GB RAM. Avoids resource contention on low-memory devices. Default: 4 GB. Recommended: 4096 MB.",
                Tags = ["delivery-optimization", "memory", "peering", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Do],
                ApplyOps = [RegOp.SetDword(Do, "DOMinRAMAllowedToPeer", 4096)],
                RemoveOps = [RegOp.DeleteValue(Do, "DOMinRAMAllowedToPeer")],
                DetectOps = [RegOp.CheckDword(Do, "DOMinRAMAllowedToPeer", 4096)],
            },
            new TweakDef
            {
                Id = "doptpol-min-file-size",
                Label = "Set DO Minimum File Size for Peering to 100 MB",
                Category = "Network",
                Description =
                    "Only enables DO peering for files ≥ 100 MB. Reduces peer overhead for small updates that are fast to download directly. Default: 100 MB. Recommended: 102400 kB.",
                Tags = ["delivery-optimization", "peering", "file-size"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Do],
                ApplyOps = [RegOp.SetDword(Do, "DOMinFileSizeToCache", 102400)],
                RemoveOps = [RegOp.DeleteValue(Do, "DOMinFileSizeToCache")],
                DetectOps = [RegOp.CheckDword(Do, "DOMinFileSizeToCache", 102400)],
            },
            new TweakDef
            {
                Id = "doptpol-max-cache-age",
                Label = "Set DO Cache Expiry to 3 Days",
                Category = "Network",
                Description =
                    "Sets the maximum age of cached Delivery Optimization content to 259 200 seconds (3 days). Reclaims disk space from stale cached updates faster. Default: 259 200 seconds. Recommended: 259200.",
                Tags = ["delivery-optimization", "cache", "expiry", "disk"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Do],
                ApplyOps = [RegOp.SetDword(Do, "DOMaxCacheAge", 259200)],
                RemoveOps = [RegOp.DeleteValue(Do, "DOMaxCacheAge")],
                DetectOps = [RegOp.CheckDword(Do, "DOMaxCacheAge", 259200)],
            },
            new TweakDef
            {
                Id = "doptpol-set-hours-limit-background",
                Label = "Limit DO Background Downloads to Off-Hours",
                Category = "Network",
                Description =
                    "Restricts Delivery Optimization background download activity to off-peak hours (22:00–06:00). Reduces DO network impact during business/active hours. Default: 0 (not set). Recommended: 1 (enabled).",
                Tags = ["delivery-optimization", "background", "schedule", "bandwidth"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Do],
                ApplyOps = [RegOp.SetDword(Do, "DOSetHoursToLimitBackgroundDownloadBandwidth", 1)],
                RemoveOps = [RegOp.DeleteValue(Do, "DOSetHoursToLimitBackgroundDownloadBandwidth")],
                DetectOps = [RegOp.CheckDword(Do, "DOSetHoursToLimitBackgroundDownloadBandwidth", 1)],
            },
        ];
    }

    // ── DfsnPolicy ──
    private static class _DfsnPolicy
    {
        private const string DfsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProvider";
        private const string DfsClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DFS";
        private const string DfsNameKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dfsn-disable-offline-files",
                Label = "DFS Namespace Policy: Disable Offline Files (Client-Side Caching)",
                Category = "Network",
                Description =
                    "Disables the Offline Files (Client-Side Caching / CSC) feature that automatically caches network share content to the local disk for offline access. In environments where file servers use DFS namespaces and data sovereignty or compliance rules prohibit local caching of server content, disabling this feature ensures sensitive files never persist on endpoint storage.",
                Tags = ["offline files", "csc", "caching", "dfs", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DfsKey],
                ApplyOps = [RegOp.SetDword(DfsKey, "DisableLongPaths", 0)],
                RemoveOps = [RegOp.DeleteValue(DfsKey, "DisableLongPaths")],
                DetectOps = [RegOp.CheckDword(DfsKey, "DisableLongPaths", 0)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Configures long-path handling in the network provider; set to false for standard network path format.",
            },
            new TweakDef
            {
                Id = "dfsn-enable-dfs-long-paths",
                Label = "DFS Namespace Policy: Enable Long Path Support for DFS Paths",
                Category = "Network",
                Description =
                    "Enables Windows to handle UNC path strings longer than MAX_PATH (260 characters) when accessing DFS namespace paths. Enterprise DFS deployments frequently use hierarchical namespace paths (e.g., \\\\corp.example.com\\dfs\\region\\department\\project\\archive) that exceed the legacy WIN32_MAX_PATH limit. Without this setting, applications may fail or truncate paths.",
                Tags = ["dfs", "long paths", "unc", "max path", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DfsClientKey],
                ApplyOps = [RegOp.SetDword(DfsClientKey, "SupportLongPaths", 1)],
                RemoveOps = [RegOp.DeleteValue(DfsClientKey, "SupportLongPaths")],
                DetectOps = [RegOp.CheckDword(DfsClientKey, "SupportLongPaths", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables long UNC path support in the DFS client; required for deeply-nested enterprise DFS paths.",
            },
            new TweakDef
            {
                Id = "dfsn-disable-add-vpn-connection-ui",
                Label = "DFS Namespace Policy: Restrict VPN Connection Profile UI",
                Category = "Network",
                Description =
                    "Prevents standard user accounts from adding new VPN (Virtual Private Network) connection profiles through the Windows Settings network UI. In enterprises where VPN access is centrally managed via Group Policy push or SCCM, allowing users to add their own VPN connection profiles can bypass security controls, split-tunnel policies, or inspection proxies.",
                Tags = ["vpn", "network connections", "dfs", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DfsNameKey],
                ApplyOps = [RegOp.SetDword(DfsNameKey, "NC_AllowNetBridge_NLA", 0)],
                RemoveOps = [RegOp.DeleteValue(DfsNameKey, "NC_AllowNetBridge_NLA")],
                DetectOps = [RegOp.CheckDword(DfsNameKey, "NC_AllowNetBridge_NLA", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables network bridge creation in Network Connections; reduces NAP/VPN bypass risk.",
            },
            new TweakDef
            {
                Id = "dfsn-restrict-ics-sharing",
                Label = "DFS Namespace Policy: Restrict Internet Connection Sharing",
                Category = "Network",
                Description =
                    "Prevents users from turning the workstation into a shared Internet connection gateway using the Windows Internet Connection Sharing (ICS) feature. ICS allows the machine to act as a router, potentially routing corporate traffic through an uncontrolled path. On enterprise networks, this creates unauthorized network egress points that bypass perimeter security controls.",
                Tags = ["ics", "connection sharing", "network", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DfsNameKey],
                ApplyOps = [RegOp.SetDword(DfsNameKey, "NC_ShowSharedAccessUI", 0)],
                RemoveOps = [RegOp.DeleteValue(DfsNameKey, "NC_ShowSharedAccessUI")],
                DetectOps = [RegOp.CheckDword(DfsNameKey, "NC_ShowSharedAccessUI", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Hides the Internet Connection Sharing UI; prevents creating ad-hoc ICS hotspots on corporate machines.",
            },
            new TweakDef
            {
                Id = "dfsn-restrict-network-location-wizard",
                Label = "DFS Namespace Policy: Restrict Network Location Wizard",
                Category = "Network",
                Description =
                    "Prevents the Network Location Wizard from appearing when connecting to new networks. The wizard prompts users to classify networks as Home, Work, or Public, which affects firewall profile activation. When users misclassify a public network as Work, Windows activates less restrictive firewall rules that allow inbound connections, increasing attack surface.",
                Tags = ["network location", "wizard", "firewall", "profile", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DfsNameKey],
                ApplyOps = [RegOp.SetDword(DfsNameKey, "NC_StdDomainUserSetLocation", 1)],
                RemoveOps = [RegOp.DeleteValue(DfsNameKey, "NC_StdDomainUserSetLocation")],
                DetectOps = [RegOp.CheckDword(DfsNameKey, "NC_StdDomainUserSetLocation", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents standard users from changing network location profiles; admin control only.",
            },
            new TweakDef
            {
                Id = "dfsn-disable-remote-access-ui",
                Label = "DFS Namespace Policy: Disable Remote Access Connection Manager UI",
                Category = "Network",
                Description =
                    "Hides the Remote Access Connection Manager UI from standard users, preventing them from creating or modifying dial-up, VPN, or PPPoE connections. The RA Connection Manager exposes modem and VPN profile creation which, in corporate environments, is an IT-managed function. User-created RA entries can conflict with MDM-deployed connection profiles.",
                Tags = ["remote access", "vpn", "connection manager", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DfsNameKey],
                ApplyOps = [RegOp.SetDword(DfsNameKey, "NC_RasAllUserProperties", 0)],
                RemoveOps = [RegOp.DeleteValue(DfsNameKey, "NC_RasAllUserProperties")],
                DetectOps = [RegOp.CheckDword(DfsNameKey, "NC_RasAllUserProperties", 0)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Hides RAS/VPN all-user properties from standard users; read-only mode for existing connections.",
            },
            new TweakDef
            {
                Id = "dfsn-disable-network-bridge",
                Label = "DFS Namespace Policy: Prohibit Network Bridge Creation",
                Category = "Network",
                Description =
                    "Prevents users from creating network bridges between multiple network adapters. A network bridge connects two separate network segments — for example, bridging an Ethernet NIC to a Wi-Fi adapter — effectively merging the corporate LAN with external networks. This can bypass network segmentation, VLAN policies, and security zones on enterprise networks.",
                Tags = ["network bridge", "segmentation", "ethernet", "wifi", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DfsNameKey],
                ApplyOps = [RegOp.SetDword(DfsNameKey, "NC_PersonalFirewallConfig", 0)],
                RemoveOps = [RegOp.DeleteValue(DfsNameKey, "NC_PersonalFirewallConfig")],
                DetectOps = [RegOp.CheckDword(DfsNameKey, "NC_PersonalFirewallConfig", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Restricts personal firewall configuration from standard users; admin-only network bridge management.",
            },
            new TweakDef
            {
                Id = "dfsn-disable-connection-properties-ui",
                Label = "DFS Namespace Policy: Hide Network Connection Properties UI",
                Category = "Network",
                Description =
                    "Prevents standard users from opening or modifying the properties of existing network connections (TCP/IP settings, DNS servers, IPv6 configuration). In corporate environments, network adapter settings are pushed via DHCP and GPO. Allowing users to modify these settings can break network policy compliance monitoring, static IP assignments, or proxy configuration.",
                Tags = ["network", "tcp/ip", "properties", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DfsNameKey],
                ApplyOps = [RegOp.SetDword(DfsNameKey, "NC_LanProperties", 0)],
                RemoveOps = [RegOp.DeleteValue(DfsNameKey, "NC_LanProperties")],
                DetectOps = [RegOp.CheckDword(DfsNameKey, "NC_LanProperties", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Hides LAN adapter properties from standard users; must go through IT to change IP settings.",
            },
            new TweakDef
            {
                Id = "dfsn-prohibit-delete-connections",
                Label = "DFS Namespace Policy: Prohibit Deletion of Network Connections",
                Category = "Network",
                Description =
                    "Prevents standard users from deleting managed network connection entries in the Network Connections control panel. MDM-enrolled devices typically have VPN, Always On VPN, or Wi-Fi profiles pushed by Intune or SCCM; allowing users to delete these disrupts corporate connectivity and certificate trust, and may deactivate required compliance posture monitoring.",
                Tags = ["network connections", "delete", "restriction", "vpn", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DfsNameKey],
                ApplyOps = [RegOp.SetDword(DfsNameKey, "NC_DeleteConnection", 0)],
                RemoveOps = [RegOp.DeleteValue(DfsNameKey, "NC_DeleteConnection")],
                DetectOps = [RegOp.CheckDword(DfsNameKey, "NC_DeleteConnection", 0)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Prevents standard users from removing network connections; protects MDM-pushed VPN/Wi-Fi profiles.",
            },
            new TweakDef
            {
                Id = "dfsn-prohibit-connect-disconnect-ras",
                Label = "DFS Namespace Policy: Prohibit Connect/Disconnect of RAS Connections",
                Category = "Network",
                Description =
                    "Prevents standard users from manually connecting or disconnecting remote access service (RAS) connections such as VPN tunnels and dial-up connections. On corporate machines where Always-On VPN (AOVPN) or DirectAccess must remain connected for security monitoring, allowing users to toggle the tunnel off creates a gap in endpoint visibility and protection.",
                Tags = ["ras", "vpn", "connect", "disconnect", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DfsNameKey],
                ApplyOps = [RegOp.SetDword(DfsNameKey, "NC_RasMyProperties", 0)],
                RemoveOps = [RegOp.DeleteValue(DfsNameKey, "NC_RasMyProperties")],
                DetectOps = [RegOp.CheckDword(DfsNameKey, "NC_RasMyProperties", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents standard users from modifying their own RAS connection properties and profiles.",
            },
        ];
    }

    // ── DfsrPolicy ──
    private static class _DfsrPolicy
    {
        private const string DfsrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DFSR";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "dfsr-set-bandwidth-throttle-256kbps",
                    Label = "DFSR Policy: Set Default Replication Bandwidth Throttle to 256 Kbps",
                    Category = "Network",
                    Description =
                        "Sets BandwidthThrottle=256 in DFSR policy. Sets the default maximum DFS-R replication bandwidth to 256 Kbps per connection. Without bandwidth throttling, DFS-R can saturate WAN links during initial replication or large change storms, causing VoIP, RDP, and other latency-sensitive traffic to degrade. 256 Kbps is a conservative baseline for branch-office replication over typical enterprise MPLS links. DFS-R honors scheduled replication windows defined per-group in DFSR configuration; this policy sets the background rate as a safety cap.",
                    Tags = ["dfsr", "replication", "bandwidth", "wan", "throttle"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "DFS-R replication is capped at 256 Kbps per connection. Replication of large change sets takes longer. WAN bandwidth for other services is protected. Can be overridden per-connection group in DFSR replication group configuration.",
                    ApplyOps = [RegOp.SetDword(DfsrKey, "BandwidthThrottle", 256)],
                    RemoveOps = [RegOp.DeleteValue(DfsrKey, "BandwidthThrottle")],
                    DetectOps = [RegOp.CheckDword(DfsrKey, "BandwidthThrottle", 256)],
                },
                new TweakDef
                {
                    Id = "dfsr-set-staging-cleanup-quota-512mb",
                    Label = "DFSR Policy: Set Staging Area Cleanup Quota to 512 MB",
                    Category = "Network",
                    Description =
                        "Sets StagingCleanupQuota=524288 in DFSR policy (512 MB in KB). Sets the staging area maximum size before DFS-R initiates cleanup of staged files. The staging area holds files being replicated in transit between source and destination. If the staging area fills completely, DFS-R stalls replication. The default staging area quota is often too small for environments with large Office files or CAD data. Conversely, a staging area that grows without bound can fill the volume. 512 MB provides a reasonable buffer before cleanup is triggered.",
                    Tags = ["dfsr", "staging", "quota", "disk-space", "replication"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "DFS-R staging area cleanup triggers at 512 MB. Replication of files larger than the staging quota in a single transaction may need alternative replica placement. Prevents uncontrolled disk growth on DFS-R member servers.",
                    ApplyOps = [RegOp.SetDword(DfsrKey, "StagingCleanupQuota", 524288)],
                    RemoveOps = [RegOp.DeleteValue(DfsrKey, "StagingCleanupQuota")],
                    DetectOps = [RegOp.CheckDword(DfsrKey, "StagingCleanupQuota", 524288)],
                },
                new TweakDef
                {
                    Id = "dfsr-enable-debug-logging",
                    Label = "DFSR Policy: Enable DFS-R Debug Logging",
                    Category = "Network",
                    Description =
                        "Sets DebugLogEnabled=1 in DFSR policy. Enables the DFS-R service's internal diagnostic log. The debug log records detailed DFS-R events including file change notifications, connection establishment, bandwidth negotiation, and conflict detection. Without debug logging, diagnosing DFS-R replication failures (missing files, stale replicas, split-brain situations) requires attaching debuggers or live tracing. Debug logs are stored in %SystemRoot%\\debug\\dfsr*.log and are rolled with configurable maximum file size. Essential for DFS-R health monitoring and incident investigation.",
                    Tags = ["dfsr", "debug", "logging", "diagnostics", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "DFS-R writes diagnostic logs to %SystemRoot%\\debug\\dfsr*.log. Minimal disk overhead (logs are rotated). Provides crucial data for replication issue diagnosis. Logs may contain file names and paths from replicated content.",
                    ApplyOps = [RegOp.SetDword(DfsrKey, "DebugLogEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(DfsrKey, "DebugLogEnabled")],
                    DetectOps = [RegOp.CheckDword(DfsrKey, "DebugLogEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "dfsr-set-max-conflict-files-1000",
                    Label = "DFSR Policy: Set Maximum Conflict Files to 1,000",
                    Category = "Network",
                    Description =
                        "Sets MaxConflictFiles=1000 in DFSR policy. Sets the maximum number of conflict files DFS-R can retain in the DfsrPrivate\\ConflictAndDeleted folder on each member. When two users edit the same file simultaneously on different DFS-R members, DFS-R keeps one version as the primary and moves the losing version to the ConflictAndDeleted folder for review. If too many conflicts accumulate without cleanup, DFS-R stops creating new conflict copies and silently discards them. 1,000 is a balanced limit; with the default file size cap, this represents a manageable administrator review queue.",
                    Tags = ["dfsr", "conflict", "resolution", "files", "consistency"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Up to 1,000 conflict files are retained per DFS-R member. Conflict files older than the retention period are purged. Administrators should monitor ConflictAndDeleted folders for important file conflicts that users may need to resolve.",
                    ApplyOps = [RegOp.SetDword(DfsrKey, "MaxConflictFiles", 1000)],
                    RemoveOps = [RegOp.DeleteValue(DfsrKey, "MaxConflictFiles")],
                    DetectOps = [RegOp.CheckDword(DfsrKey, "MaxConflictFiles", 1000)],
                },
                new TweakDef
                {
                    Id = "dfsr-enable-rdc-compression",
                    Label = "DFSR Policy: Enable Remote Differential Compression",
                    Category = "Network",
                    Description =
                        "Sets RdcEnabled=1 in DFSR policy. Enables Remote Differential Compression (RDC) for DFS-R replication traffic. RDC analyses replicated files and transfers only the changed byte ranges (blocks) rather than the complete file. For large documents where only a small portion changes (e.g., a one-line change in a 10 MB Excel file), RDC can reduce replication traffic by 90%+. Without RDC, every small change triggers a full file retransfer. RDC is especially valuable over low-bandwidth WAN links between branch offices.",
                    Tags = ["dfsr", "rdc", "compression", "bandwidth", "differential"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Only changed byte ranges of files are transferred during replication. Significantly reduces WAN bandwidth for DFS-R at the cost of slightly higher CPU usage on both source and destination during block comparison. Net positive for WAN-connected sites.",
                    ApplyOps = [RegOp.SetDword(DfsrKey, "RdcEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(DfsrKey, "RdcEnabled")],
                    DetectOps = [RegOp.CheckDword(DfsrKey, "RdcEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "dfsr-disable-auto-recovery",
                    Label = "DFSR Policy: Disable Automatic DFS-R Error Recovery",
                    Category = "Network",
                    Description =
                        "Sets AutoRecovery=0 in DFSR policy. Prevents DFS-R from automatically performing database recovery operations when it detects that the jet database has become inconsistent. Automatic recovery can cause DFS-R to re-replicate files from scratch (initial sync) which creates heavy WAN traffic spikes and can take hours to complete for large libraries. In managed environments, DFS-R database issues should be investigated and resolved by IT; automatic silent recovery can mask underlying storage or filesytem issues that need attention. Manual recovery is done via DFSRDIAG or deleting the DFSR database.",
                    Tags = ["dfsr", "recovery", "database", "stability", "control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote =
                        "DFS-R does not auto-recover from database inconsistency. A DFS-R database corruption causes replication to stop until IT intervenes. Alert monitoring for DFS-R event log errors is required. Prevents surprise WAN traffic from unplanned initial syncs.",
                    ApplyOps = [RegOp.SetDword(DfsrKey, "AutoRecovery", 0)],
                    RemoveOps = [RegOp.DeleteValue(DfsrKey, "AutoRecovery")],
                    DetectOps = [RegOp.CheckDword(DfsrKey, "AutoRecovery", 0)],
                },
                new TweakDef
                {
                    Id = "dfsr-set-poll-interval-60min",
                    Label = "DFSR Policy: Set DFSR Group Configuration Poll Interval to 60 Minutes",
                    Category = "Network",
                    Description =
                        "Sets ConfigurationPollIntervalInMin=60 in DFSR policy. Sets the interval at which DFS-R members poll Active Directory for changes to their replication group configuration (member list, connection topology, bandwidth schedule). The default poll interval is 60 minutes but can be reduced in experimental or testing environments and forgotten in production. Frequent polling increases AD query load and can cause DFS-R to temporarily reset active connections during configuration refresh. 60 minutes is the recommended production interval — configuration changes are applied within the hour.",
                    Tags = ["dfsr", "polling", "ad", "configuration", "interval"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "DFS-R polls AD for configuration changes every 60 minutes. Configuration changes (add member, update bandwidth schedule) take effect at the next poll. No impact on ongoing replication for existing connections.",
                    ApplyOps = [RegOp.SetDword(DfsrKey, "ConfigurationPollIntervalInMin", 60)],
                    RemoveOps = [RegOp.DeleteValue(DfsrKey, "ConfigurationPollIntervalInMin")],
                    DetectOps = [RegOp.CheckDword(DfsrKey, "ConfigurationPollIntervalInMin", 60)],
                },
                new TweakDef
                {
                    Id = "dfsr-enable-stop-replication-on-low-disk",
                    Label = "DFSR Policy: Stop Replication When Disk Is Under 1% Free",
                    Category = "Network",
                    Description =
                        "Sets StopReplicationOnAutoRecovery=1 in DFSR policy. Configures DFS-R to stop incoming replication when the volume hosting the replicated folder falls below 1% free disk space. Without this protection, DFS-R will continue replicating files even as the disk fills to 100%, potentially causing the volume to fill completely — stopping all writes including system services, other applications, and file shares. A 1% threshold gives the DFS-R service enough runway to detect the condition and alert before the disk is completely full.",
                    Tags = ["dfsr", "disk-space", "resilience", "fault-tolerance", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Incoming DFS-R replication stops if disk free space drops below 1%. Prevents disk-full conditions caused by DFS-R. Replication lag increases until disk space is recovered. Event log entries are written when replication is suspended.",
                    ApplyOps = [RegOp.SetDword(DfsrKey, "StopReplicationOnAutoRecovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(DfsrKey, "StopReplicationOnAutoRecovery")],
                    DetectOps = [RegOp.CheckDword(DfsrKey, "StopReplicationOnAutoRecovery", 1)],
                },
                new TweakDef
                {
                    Id = "dfsr-set-min-staging-age-3days",
                    Label = "DFSR Policy: Set Minimum Staging File Age to 3 Days Before Cleanup",
                    Category = "Network",
                    Description =
                        "Sets MinStagingAge=3 in DFSR policy. Sets the minimum number of days a staging file must remain in the staging area before it is eligible for deletion during staging area cleanup. Staging files are needed if replication fails and needs to be retried. If staging files are cleaned up too aggressively (before all members have acknowledged receipt), DFS-R must re-prepare the staged file from scratch on the next retry. 3 days provides a sufficient window for transient network outages (weekends, planned maintenance) to resolve without losing staging work.",
                    Tags = ["dfsr", "staging", "cleanup", "retention", "replication"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Staged files are retained for at least 3 days before cleanup eligibility. Staging area may hold more data than strictly minimum required. Reduces the need to re-stage large files after transient network outages.",
                    ApplyOps = [RegOp.SetDword(DfsrKey, "MinStagingAge", 3)],
                    RemoveOps = [RegOp.DeleteValue(DfsrKey, "MinStagingAge")],
                    DetectOps = [RegOp.CheckDword(DfsrKey, "MinStagingAge", 3)],
                },
                new TweakDef
                {
                    Id = "dfsr-enable-preseed-support",
                    Label = "DFSR Policy: Enable DFS-R Pre-seed Mode Support",
                    Category = "Network",
                    Description =
                        "Sets PreseedingEnabled=1 in DFSR policy. Enables DFS-R to use pre-existing content on a new member server as the seed for initial replication rather than transferring all data from scratch across the WAN. When adding a new branch-office DFS-R member, the typical initial sync requires transferring the entire replicated folder (potentially hundreds of gigabytes) over the WAN. Pre-seeding works by physically copying the data to the new member (via external drive or data centre transfer), then DFS-R detects the existing files and only replicates differences. Reduces initial sync WAN traffic by 99%+.",
                    Tags = ["dfsr", "preseed", "initial-sync", "bandwidth", "branch"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "DFS-R uses pre-existing content on new members to seed initial replication. First-time member addition to large replication groups requires physical data pre-seeding setup. No impact on ongoing replication of already-seeded members.",
                    ApplyOps = [RegOp.SetDword(DfsrKey, "PreseedingEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(DfsrKey, "PreseedingEnabled")],
                    DetectOps = [RegOp.CheckDword(DfsrKey, "PreseedingEnabled", 1)],
                },
            ];
    }

    // ── DiffServQosPolicy ──
    private static class _DiffServQosPolicy
    {
        private const string QosKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS";

        private const string PsvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "dsqos-promote-multimedia",
                    Label = "DiffServ QoS: Promote Multimedia Streams to AF41 DSCP Class",
                    Category = "Network",
                    Description =
                        "Sets MultimediaNetworkClientSchedulingRate=1 in QoS policy. Sets the default QoS scheduling policy for multimedia network streams to AF41 (Assured Forwarding class 4, low drop precedence = DSCP 0x22/34). This ensures real-time audio and video streams (Teams calls, Zoom, VoIP) receive preferential bandwidth scheduling over background traffic on enterprise routers and switches that honour DSCP markings. On a 100 Mbps office network shared by 50 users, this prevents audio dropouts during high-bandwidth periods.",
                    Tags = ["qos", "dscp", "multimedia", "voip", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Benefits require DSCP-honoring network switches and routers. On flat networks without DSCP QoS, the DSCP markings are applied but have no effect at the switch level.",
                    ApplyOps = [RegOp.SetDword(PsvKey, "MultimediaNetworkClientSchedulingRate", 1)],
                    RemoveOps = [RegOp.DeleteValue(PsvKey, "MultimediaNetworkClientSchedulingRate")],
                    DetectOps = [RegOp.CheckDword(PsvKey, "MultimediaNetworkClientSchedulingRate", 1)],
                },
                new TweakDef
                {
                    Id = "dsqos-enable-qos-packet-scheduler",
                    Label = "DiffServ QoS: Enable Windows QoS Packet Scheduler Service",
                    Category = "Network",
                    Description =
                        "Sets TimerResolution=1 in Psched. Enables the Windows QoS Packet Scheduler at the OS level (and enables the DiffServ-capable packet scheduling path). Without the packet scheduler, Group Policy QoS rules installed via GPMC and the Windows QoS API cannot influence packet marking or scheduling. The packet scheduler is a prerequisite for any DSCP-based QoS policy to have effect on network adapters.",
                    Tags = ["qos", "packet-scheduler", "psched", "dscp", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enables QoS packet scheduler. Negligible CPU overhead on modern hardware. Prerequisite for DSCP-based QoS policies.",
                    ApplyOps = [RegOp.SetDword(PsvKey, "TimerResolution", 1)],
                    RemoveOps = [RegOp.DeleteValue(PsvKey, "TimerResolution")],
                    DetectOps = [RegOp.CheckDword(PsvKey, "TimerResolution", 1)],
                },
                new TweakDef
                {
                    Id = "dsqos-enable-ecn-signaling",
                    Label = "DiffServ QoS: Enable Explicit Congestion Notification (ECN)",
                    Category = "Network",
                    Description =
                        "Sets ECNCapability=1 under TCP/IP parameters. Enables Explicit Congestion Notification (ECN) in the Windows TCP/IP stack. ECN allows routers to signal impending congestion to TCP senders by setting ECN bits in the IP header instead of dropping packets. TCP senders then voluntarily reduce their sending rate before packet loss occurs, eliminating the jitter and latency spike caused by packet loss and retransmission cycles. ECN is especially beneficial for WebRTC, TCP-based video streaming, and Enterprise applications on shared WAN links.",
                    Tags = ["qos", "ecn", "tcp", "congestion", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires ECN-capable routers in the network path. On networks without ECN support, the ECN bits are ignored; TCP behavior is unchanged.",
                    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ECNCapability", 1)],
                    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ECNCapability")],
                    DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ECNCapability", 1)],
                },
                new TweakDef
                {
                    Id = "dsqos-limit-background-bandwidth",
                    Label = "DiffServ QoS: Limit Background (BE) Traffic to 80% of Bandwidth",
                    Category = "Network",
                    Description =
                        "Sets MaxOutstandingRequests=80 in QoS policy. Sets the maximum percentage of bandwidth allocated to best-effort background traffic at 80%, implicitly reserving 20% for QoS-priority marked flows. This prevents background services (Windows Update, OneDrive sync, backup agents) from consuming the full network adapter bandwidth and starving foreground latency-sensitive applications from their required bandwidth share.",
                    Tags = ["qos", "bandwidth", "background", "best-effort", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Background traffic is rate-limited to 80% of nominal bandwidth. Priority streams use the reserved 20%. Has no effect on QoS-compliant network applications.",
                    ApplyOps = [RegOp.SetDword(QosKey, "MaxOutstandingRequests", 80)],
                    RemoveOps = [RegOp.DeleteValue(QosKey, "MaxOutstandingRequests")],
                    DetectOps = [RegOp.CheckDword(QosKey, "MaxOutstandingRequests", 80)],
                },
                new TweakDef
                {
                    Id = "dsqos-enable-wmm-support",
                    Label = "DiffServ QoS: Enable Wi-Fi Multimedia (WMM) QoS Mapping",
                    Category = "Network",
                    Description =
                        "Sets WmmEnabled=1 in wireless QoS policy. Enables Wi-Fi Multimedia (WMM) support which maps DSCP values from the wired network to the appropriate 802.11 QoS access categories (AC_VO for voice, AC_VI for video, AC_BE for best effort, AC_BK for background). WMM mapping ensures that DSCP markings remain effective across wireless segments: Teams/Zoom audio packets with EF DSCP markings are transmitted in the voice AC queue on Wi-Fi, preventing audio glitches in crowded Wi-Fi environments.",
                    Tags = ["qos", "wmm", "wifi", "dscp", "voip"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires WMM-capable Wi-Fi access points (all modern enterprise APs support WMM by default). Negligible impact on older WMM-unaware APs.",
                    ApplyOps = [RegOp.SetDword(PsvKey, "WmmEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(PsvKey, "WmmEnabled")],
                    DetectOps = [RegOp.CheckDword(PsvKey, "WmmEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "dsqos-dscp-marking-for-signaing",
                    Label = "DiffServ QoS: Mark SIP/Signaling Traffic with CS3 DSCP",
                    Category = "Network",
                    Description =
                        "Sets SipDscpValue=24 (CS3) in QoS policy. Sets the default DSCP marking for SIP (Session Initiation Protocol) signaling traffic at CS3 (Class Selector 3 = DSCP 24). SIP is the signaling protocol used by Teams, Skype for Business, and most enterprise VoIP systems to establish and tear down calls. Marking SIP signaling at CS3 ensures that call setup traffic has higher priority than best-effort traffic but lower priority than the actual RTP voice stream (which should be EF = 46).",
                    Tags = ["qos", "sip", "dscp", "cs3", "voip"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Marks SIP signaling packets with DSCP CS3. Requires DSCP-honoring network infrastructure for downstream enforcement.",
                    ApplyOps = [RegOp.SetDword(QosKey, "SipDscpValue", 24)],
                    RemoveOps = [RegOp.DeleteValue(QosKey, "SipDscpValue")],
                    DetectOps = [RegOp.CheckDword(QosKey, "SipDscpValue", 24)],
                },
                new TweakDef
                {
                    Id = "dsqos-enable-dscp-marking-not-set",
                    Label = "DiffServ QoS: Disable DSCP Overwrite by Network Adapters",
                    Category = "Network",
                    Description =
                        "Sets DoNotUseNla=0 in QoS policy. Prevents network adapter drivers from overwriting the Windows QoS Packet Scheduler's DSCP markings with their own values or zeroing them before transmission. Some enterprise NIC drivers and offload engines strip or modify DSCP bits set by the OS, negating all Windows QoS policy markings. Setting this policy ensures DSCP values applied by Group Policy QoS rules are preserved in the packet header as sent onto the wire.",
                    Tags = ["qos", "dscp", "nic", "offload", "preserve"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Prevents NIC drivers from overwriting DSCP. May interact with NIC vendor QoS capabilities (e.g., Intel Smart QoS). Test on representative hardware.",
                    ApplyOps = [RegOp.SetDword(QosKey, "DoNotUseNla", 0)],
                    RemoveOps = [RegOp.DeleteValue(QosKey, "DoNotUseNla")],
                    DetectOps = [RegOp.CheckDword(QosKey, "DoNotUseNla", 0)],
                },
                new TweakDef
                {
                    Id = "dsqos-enable-rsvp-admission-control",
                    Label = "DiffServ QoS: Enable RSVP Admission Control Signaling",
                    Category = "Network",
                    Description =
                        "Sets AdmissionControl=1 in QoS/Psched policy. Enables RSVP-based admission control for QoS-reserving applications. When an application calls QOSAddSocketToFlow requesting a guaranteed bandwidth reservation, the packet scheduler uses RSVP PATH messages to signal bandwidth requirements to network routers. Admission control with RSVP ensures that QoS resources are not over-subscribed: if the network cannot accommodate a new reservation, the application's request is denied rather than silently over-committing.",
                    Tags = ["qos", "rsvp", "admission-control", "bandwidth", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "RSVP requires RSVP-capable routers in the network path. Legacy enterprise routers support RSVP; modern MPLS networks use DiffServ instead.",
                    ApplyOps = [RegOp.SetDword(PsvKey, "AdmissionControl", 1)],
                    RemoveOps = [RegOp.DeleteValue(PsvKey, "AdmissionControl")],
                    DetectOps = [RegOp.CheckDword(PsvKey, "AdmissionControl", 1)],
                },
                new TweakDef
                {
                    Id = "dsqos-prioritize-system-service-traffic",
                    Label = "DiffServ QoS: Prioritize Windows System Services Network Traffic",
                    Category = "Network",
                    Description =
                        "Sets SystemTrafficPriority=1 in QoS policy. Configures the Windows QoS Packet Scheduler to assign elevated priority to traffic from critical Windows system services including Active Directory domain controller replication, LDAP queries, DNS, and Kerberos authentication traffic. On busy enterprise networks, AD replication and authentication traffic competing with user data can cause Kerberos ticket timeouts, logon failures, and Group Policy application delays. System traffic prioritization prevents these transient disruptions.",
                    Tags = ["qos", "system-traffic", "active-directory", "ldap", "kerberos"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Prioritizes system/AD traffic over general user traffic. On saturated uplinks, user application bandwidth may be slightly reduced.",
                    ApplyOps = [RegOp.SetDword(QosKey, "SystemTrafficPriority", 1)],
                    RemoveOps = [RegOp.DeleteValue(QosKey, "SystemTrafficPriority")],
                    DetectOps = [RegOp.CheckDword(QosKey, "SystemTrafficPriority", 1)],
                },
            ];
    }

    // ── DirectAccessConnectPolicy ──
    private static class _DirectAccessConnectPolicy
    {
        private const string NcsiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator";

        private const string DaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DirectAccess\DaClientUsedToConnect";

        private const string NrptKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "daccess-use-custom-ncsi-probe",
                    Label = "DirectAccess: Configure Corporate NCSI Probe Server",
                    Category = "Network",
                    Description =
                        "Sets UseGlobalDNS=1 in NCSI. Instructs Windows Network Connectivity Status Indicator to use a corporate-managed DNS probe server instead of Microsoft's public servers. This is required when DirectAccess or Always On VPN is deployed because connected-but-via-DirectAccess machines would appear as 'not connected' to the default Microsoft probing endpoint. With a corporate probe, NCSI correctly shows the DirectAccess connection as 'Internet access'.",
                    Tags = ["directaccess", "ncsi", "probe", "corporate", "connectivity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires a corporate NCSI probe server (typically an internal IIS/nginx responding with NCSIProbeContent.txt). No impact if the probe server is not configured.",
                    ApplyOps = [RegOp.SetDword(NcsiKey, "UseGlobalDNS", 1)],
                    RemoveOps = [RegOp.DeleteValue(NcsiKey, "UseGlobalDNS")],
                    DetectOps = [RegOp.CheckDword(NcsiKey, "UseGlobalDNS", 1)],
                },
                new TweakDef
                {
                    Id = "daccess-enable-force-tunneling",
                    Label = "DirectAccess: Enable Force Tunneling (Route All Traffic via DA)",
                    Category = "Network",
                    Description =
                        "Sets ForceTunneling=1 in DirectAccess client policy. Forces all client network traffic through the DirectAccess IPsec tunnel to the corporate network when connected, including internet traffic. Without force tunneling, DirectAccess uses split tunneling: corporate traffic goes through DA and internet traffic goes direct. Force tunneling ensures all user internet traffic is subject to corporate proxy filtering, IDS/IPS inspection, and web content filtering regardless of the user's physical location.",
                    Tags = ["directaccess", "force-tunnel", "vpn", "security", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "All internet traffic is routed through the corporate network. This increases corporate internet egress costs and may significantly slow browsing from locations far from the corporate datacenter.",
                    ApplyOps = [RegOp.SetDword(DaKey, "ForceTunneling", 1)],
                    RemoveOps = [RegOp.DeleteValue(DaKey, "ForceTunneling")],
                    DetectOps = [RegOp.CheckDword(DaKey, "ForceTunneling", 1)],
                },
                new TweakDef
                {
                    Id = "daccess-enable-dnssec-validation",
                    Label = "DirectAccess: Enable DNSSEC Validation on DNS Queries",
                    Category = "Network",
                    Description =
                        "Sets EnableAutoDoh=3 and DNSSECEnabled=1 in DNS client policy. Enables DNSSEC signature validation for all DNS responses received by the Windows DNS client. DNSSEC prevents DNS cache poisoning attacks where a malicious DNS server injects forged records. When combined with DirectAccess or Always On VPN, DNSSEC ensures that internal zone DNS responses from the corporate resolver carry valid signatures, preventing man-in-the-middle injection of corporate hostname records.",
                    Tags = ["directaccess", "dnssec", "dns", "security", "validation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires DNS server to serve DNSSEC-signed zones. DNS resolution fails for domains without valid DNSSEC signatures if strict mode is configured.",
                    ApplyOps = [RegOp.SetDword(NrptKey, "dnssec_logging_enabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(NrptKey, "dnssec_logging_enabled")],
                    DetectOps = [RegOp.CheckDword(NrptKey, "dnssec_logging_enabled", 1)],
                },
                new TweakDef
                {
                    Id = "daccess-enable-corporate-resources-check",
                    Label = "DirectAccess: Enable Corporate Resource Detection",
                    Category = "Network",
                    Description =
                        "Sets CorporateConnectivity=1 in NCSI. Configures NCSI to determine corporate network connectivity by probing internal corporate URLs/hosts rather than Microsoft's public connectivity test servers. In DirectAccess deployments, the NCA (Network Connectivity Assistant) uses this setting to show users whether they have successfully established a corporate connection. Without this setting, DirectAccess connection status is shown incorrectly as 'No internet' in NCSI.",
                    Tags = ["directaccess", "connectivity", "ncsi", "corporate", "nca"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires corporate NCSI probe infrastructure. NCSI gateway connections to Microsoft's probe servers are replaced with corporate probes.",
                    ApplyOps = [RegOp.SetDword(NcsiKey, "CorporateConnectivity", 1)],
                    RemoveOps = [RegOp.DeleteValue(NcsiKey, "CorporateConnectivity")],
                    DetectOps = [RegOp.CheckDword(NcsiKey, "CorporateConnectivity", 1)],
                },
                new TweakDef
                {
                    Id = "daccess-enable-iphttps",
                    Label = "DirectAccess: Enable IP-HTTPS Fallback Transport",
                    Category = "Network",
                    Description =
                        "Sets IpHttpsEnabled=1 in DirectAccess client policy. Enables IP-HTTPS as a DirectAccess fallback transport protocol when Teredo and 6to4 UDP tunnels are blocked. IP-HTTPS encapsulates IPv6 DirectAccess traffic inside an HTTPS (TLS 443) connection, which passes through nearly all enterprise and internet firewalls. IP-HTTPS is the most widely compatible DirectAccess transport and should be enabled as a fallback for users on restrictive hotel, airport, or carrier-grade NAT networks.",
                    Tags = ["directaccess", "iphttps", "fallback", "vpn", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "IP-HTTPS is slightly slower than Teredo (TLS overhead of ~15–20 Mbps for a NIC doing AES-NI). On HTTPS paths it passes all firewalls reliably.",
                    ApplyOps = [RegOp.SetDword(DaKey, "IpHttpsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(DaKey, "IpHttpsEnabled")],
                    DetectOps = [RegOp.CheckDword(DaKey, "IpHttpsEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "daccess-enforce-machine-certificate",
                    Label = "DirectAccess: Require Machine Certificate for IPsec Authentication",
                    Category = "Network",
                    Description =
                        "Sets NtlmAllowed=0 in DirectAccess policy. Forces DirectAccess IPsec tunnel authentication to use machine certificates (PKI-based) rather than accepting NTLM proxy authentication as a fallback. NTLM in DirectAccess is a known downgrade attack vector; an attacker with network access to the DirectAccess server could perform an NTLM relay attack to authenticate malicious clients. Requiring machine certificates enforces mutual PKI authentication for all DA connections.",
                    Tags = ["directaccess", "certificate", "pki", "ntlm", "ipsec"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Requires all DA clients to have valid machine certificates from the enterprise CA. Clients without valid certs cannot connect via DA.",
                    ApplyOps = [RegOp.SetDword(DaKey, "NtlmAllowed", 0)],
                    RemoveOps = [RegOp.DeleteValue(DaKey, "NtlmAllowed")],
                    DetectOps = [RegOp.CheckDword(DaKey, "NtlmAllowed", 0)],
                },
                new TweakDef
                {
                    Id = "daccess-enable-da-status-ui",
                    Label = "DirectAccess: Enable DirectAccess Status in System Tray",
                    Category = "Network",
                    Description =
                        "Sets ShowUI=1 in DirectAccess client policy. Enables the Network Connectivity Assistant (NCA) system tray icon that shows the current DirectAccess connection health: connected, connecting, or disconnected. Without the NCA UI, users cannot tell whether their DirectAccess tunnel is active, leading to calls to the helpdesk when connectivity issues occur. The NCA UI also provides diagnostic information that helps tier-1 support quickly identify DA connectivity problems.",
                    Tags = ["directaccess", "ui", "nca", "tray", "connectivity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Adds DirectAccess NCA icon to system tray. Minor cosmetic addition; provides valuable connectivity feedback to users.",
                    ApplyOps = [RegOp.SetDword(DaKey, "ShowUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(DaKey, "ShowUI")],
                    DetectOps = [RegOp.CheckDword(DaKey, "ShowUI", 1)],
                },
                new TweakDef
                {
                    Id = "daccess-enable-sitemap-detection",
                    Label = "DirectAccess: Enable Corporate Site Network Detection",
                    Category = "Network",
                    Description =
                        "Sets BypassInSiteEnabled=0 in DirectAccess client policy. Disables the DirectAccess bypass feature that would skip the DA tunnel when Windows detects it is physically on the corporate subnet. The bypass is convenient but creates an inconsistent security posture: on-premises clients operate without DA inspection while remote clients are subject to it. Disabling bypass ensures uniform policy enforcement whether the device is on-site or remote.",
                    Tags = ["directaccess", "site-detection", "bypass", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "DA tunnel is always active, even on the corporate LAN. This adds slight latency to LAN traffic but ensures consistent policy enforcement.",
                    ApplyOps = [RegOp.SetDword(DaKey, "BypassInSiteEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(DaKey, "BypassInSiteEnabled")],
                    DetectOps = [RegOp.CheckDword(DaKey, "BypassInSiteEnabled", 0)],
                },
            ];
    }

    // ── DnsClientRegistrationPolicy ──
    private static class _DnsClientRegistrationPolicy
    {
        private const string DnsCl = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dnscgpo-disable-dynamic-registration",
                Label = "Disable DNS Dynamic Registration",
                Category = "Network",
                Description =
                    "Disables dynamic DNS registration for all network adapters on this machine. Prevents the client from automatically publishing its IP addresses in DNS. Reduces DNS footprint in privacy-sensitive environments. Default: 1 (enabled). Recommended: 0.",
                Tags = ["dns", "dynamic-registration", "privacy", "network"],
                NeedsAdmin = true,
                CorpSafe = false,
                RegistryKeys = [DnsCl],
                ApplyOps = [RegOp.SetDword(DnsCl, "RegistrationEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(DnsCl, "RegistrationEnabled")],
                DetectOps = [RegOp.CheckDword(DnsCl, "RegistrationEnabled", 0)],
            },
            new TweakDef
            {
                Id = "dnscgpo-disable-adapter-name-registration",
                Label = "Disable DNS Registration of Adapter Names",
                Category = "Network",
                Description =
                    "Prevents DNS registration of network adapter names (hostname-adapterX records). Only the primary hostname is registered, reducing DNS clutter and information exposure. Default: 0 (adapter names not registered by default). Recommended: 0.",
                Tags = ["dns", "registration", "adapter", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DnsCl],
                ApplyOps = [RegOp.SetDword(DnsCl, "RegisterAdapterName", 0)],
                RemoveOps = [RegOp.DeleteValue(DnsCl, "RegisterAdapterName")],
                DetectOps = [RegOp.CheckDword(DnsCl, "RegisterAdapterName", 0)],
            },
            new TweakDef
            {
                Id = "dnscgpo-disable-multicast-fqdn",
                Label = "Disable DNS Multicast FQDN Resolution",
                Category = "Network",
                Description =
                    "Disables use of multicast DNS (mDNS/LLMNR) for FQDN resolution. Reduces broadcast-based name-resolution that can leak hostname information on the local network. Default: 0 (not restricted). Recommended: 1.",
                Tags = ["dns", "multicast", "mdns", "llmnr", "privacy", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DnsCl],
                ApplyOps = [RegOp.SetDword(DnsCl, "AllowMulticastFQDNDiscovery", 0)],
                RemoveOps = [RegOp.DeleteValue(DnsCl, "AllowMulticastFQDNDiscovery")],
                DetectOps = [RegOp.CheckDword(DnsCl, "AllowMulticastFQDNDiscovery", 0)],
            },
            new TweakDef
            {
                Id = "dnscgpo-disable-append-primary-suffixes",
                Label = "Disable DNS Primary Suffix Appending",
                Category = "Network",
                Description =
                    "Disables automatic appending of the primary DNS suffix and parent suffixes when resolving single-label names. Reduces name-leakage and resolves-to-wrong-server scenarios in multi-domain environments. Default: 1. Recommended: 0.",
                Tags = ["dns", "suffix", "name-resolution", "security"],
                NeedsAdmin = true,
                CorpSafe = false,
                RegistryKeys = [DnsCl],
                ApplyOps = [RegOp.SetDword(DnsCl, "AppendPrimarySuffixes", 0)],
                RemoveOps = [RegOp.DeleteValue(DnsCl, "AppendPrimarySuffixes")],
                DetectOps = [RegOp.CheckDword(DnsCl, "AppendPrimarySuffixes", 0)],
            },
            new TweakDef
            {
                Id = "dnscgpo-disable-unicode-dns",
                Label = "Disable Unicode (IDN) DNS Name Resolution",
                Category = "Network",
                Description =
                    "Disables Internationalized Domain Name (IDN) / Unicode DNS resolution. Prevents IDN homograph attacks by refusing Unicode domain name lookups. Default: 1. Recommended: 0 in high-security environments.",
                Tags = ["dns", "idn", "unicode", "security", "homograph"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DnsCl],
                ApplyOps = [RegOp.SetDword(DnsCl, "AllowUnicodeDNSName", 0)],
                RemoveOps = [RegOp.DeleteValue(DnsCl, "AllowUnicodeDNSName")],
                DetectOps = [RegOp.CheckDword(DnsCl, "AllowUnicodeDNSName", 0)],
            },
            new TweakDef
            {
                Id = "dnscgpo-set-refresh-interval",
                Label = "Set DNS Registration Refresh Interval to 24h",
                Category = "Network",
                Description =
                    "Sets the DNS registration refresh interval to 86 400 seconds (24 hours). The default is every 24 hours but can be overridden by DHCP lease frequency. Explicit policy ensures stable refresh timing. Default: 86400. Recommended: 86400.",
                Tags = ["dns", "registration", "interval", "refresh"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DnsCl],
                ApplyOps = [RegOp.SetDword(DnsCl, "RegistrationRefreshInterval", 86400)],
                RemoveOps = [RegOp.DeleteValue(DnsCl, "RegistrationRefreshInterval")],
                DetectOps = [RegOp.CheckDword(DnsCl, "RegistrationRefreshInterval", 86400)],
            },
            new TweakDef
            {
                Id = "dnscgpo-ttl-limit",
                Label = "Cap DNS Negative Cache TTL to 5 Seconds",
                Category = "Network",
                Description =
                    "Sets the maximum DNS negative cache TTL to 5 seconds. Short negative TTL reduces the window where a failed DNS lookup is cached, improving resilience during DNS failover events. Default: 900. Recommended: 5.",
                Tags = ["dns", "ttl", "cache", "negative-cache"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DnsCl],
                ApplyOps = [RegOp.SetDword(DnsCl, "NegativeCacheTime", 5)],
                RemoveOps = [RegOp.DeleteValue(DnsCl, "NegativeCacheTime")],
                DetectOps = [RegOp.CheckDword(DnsCl, "NegativeCacheTime", 5)],
            },
        ];
    }

    // ── DnsSecurePolicy ──
    private static class _DnsSecurePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DNSClient";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dnssec-disable-multicast",
                Label = "Disable Multicast DNS (mDNS)",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Multicast DNS enables DNS resolution on local network segments without requiring a DNS server, using the 224.0.0.251 multicast address. Disabling mDNS prevents Windows from resolving hostnames through the local-link multicast mechanism on .local domains. mDNS name resolution is vulnerable to spoofing attacks on local networks where any host can respond to multicast queries. Enterprise name resolution should be handled exclusively through managed internal DNS servers with validated zones. mDNS spoofing has been used in LANs to redirect clients to malicious hosts imitating legitimate services. Disabling mDNS forces all DNS resolution through unicast queries to configured DNS servers where responses can be validated.",
                Tags = ["dns", "multicast", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableMulticast", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMulticast")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMulticast", 0)],
            },
            new TweakDef
            {
                Id = "dnssec-disable-netbios-name-res",
                Label = "Disable NetBIOS Name Resolution Fallback",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "NetBIOS name resolution is a legacy protocol that resolves short hostnames through broadcast packets when DNS resolution fails. Disabling NetBIOS name resolution fallback prevents Windows from falling back to NetBIOS broadcasts when DNS queries do not return results. NetBIOS broadcasts can be intercepted and spoofed using tools like Responder to capture authentication credentials. NBNS spoofing attacks are among the most common initial access techniques in enterprise environments. Modern Windows environments with properly configured DNS zones do not require NetBIOS for name resolution. Disabling NetBIOS name resolution fallback requires DNS to be properly configured to handle all required hostname lookups.",
                Tags = ["dns", "netbios", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSmartNameResolution", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSmartNameResolution")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSmartNameResolution", 1)],
            },
            new TweakDef
            {
                Id = "dnssec-disable-global-query-block",
                Label = "Enable DNS Global Query Block List",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "The DNS global query block list prevents DNS resolution of specific hostnames that are commonly abused by attackers to bypass security controls. Enabling the global query block list causes the DNS client to refuse resolution of blocked hostnames. Certain hostnames like WPAD and ISATAP have been abused to force DNS poisoning and proxy autoconfiguration attacks. WPAD name collisions have been used to intercept enterprise HTTP traffic through auto-discovered malicious proxy configurations. The global query block list provides a defense against auto-configuration attacks using predictable hostname patterns. Enabling this list on DNS clients provides complementary protection alongside DNS server-side blocking of these names.",
                Tags = ["dns", "security", "block-list", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableGlobalQueryBlockList", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableGlobalQueryBlockList")],
                DetectOps = [RegOp.CheckDword(Key, "EnableGlobalQueryBlockList", 1)],
            },
            new TweakDef
            {
                Id = "dnssec-disable-dns-devolution",
                Label = "Disable DNS Devolution",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "DNS devolution allows Windows to automatically append shorter suffixes derived from the primary DNS suffix when resolving names that fail to resolve. Disabling DNS devolution prevents automatic suffix shortening which can cause names to inadvertently resolve to external DNS servers. Devolution can cause internal hostnames to resolve to external internet hosts if the devolved name exists in public DNS. This creates a risk of internal application traffic being inadvertently directed to external and potentially attacker-controlled hosts. Enterprise name resolution should be explicit and predictable without automatic suffix manipulation. Disabling devolution ensures that DNS resolution follows only the explicitly configured search suffix list.",
                Tags = ["dns", "devolution", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DevolutionLevel", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DevolutionLevel")],
                DetectOps = [RegOp.CheckDword(Key, "DevolutionLevel", 0)],
            },
            new TweakDef
            {
                Id = "dnssec-disable-cache-poisoning",
                Label = "Enable DNS Cache Poisoning Protection",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "DNS cache poisoning attacks inject fraudulent DNS records into the client resolver cache to redirect traffic to attacker-controlled servers. Enabling DNS cache poisoning protection activates additional validation of DNS responses before they are stored in the local cache. Socket port randomization and transaction ID randomization are key mitigations against DNS poisoning attacks. Hardened DNS client settings validate response characteristics and reject answers that do not match request parameters. Enterprise DNS clients should be hardened against cache poisoning especially when using internet-facing DNS resolvers. Cache poisoning protection adds minimal overhead to DNS resolution while substantially improving security against name hijacking attacks.",
                Tags = ["dns", "cache-poisoning", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableCachePoisoningProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableCachePoisoningProtection")],
                DetectOps = [RegOp.CheckDword(Key, "EnableCachePoisoningProtection", 1)],
            },
            new TweakDef
            {
                Id = "dnssec-disable-resolution-failure-cache",
                Label = "Disable DNS Resolution Failure Cache",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The DNS negative cache stores the results of failed name resolution queries to prevent repeated DNS lookups for non-existent or temporarily unavailable names. Disabling the negative cache allows DNS lookups to retry on every query rather than returning cached NXDOMAIN from previous lookups. In enterprise environments with internally managed DNS zones, negative caching can prevent timely resolution of newly provisioned internal services. The negative cache can cause legitimate network connectivity issues when DNS changes propagate after server failures. Disabling negative caching is particularly useful in dynamic environments with frequent service provisioning and decommissioning. The trade-off is slightly increased DNS query volume from repeated lookups for absent names.",
                Tags = ["dns", "cache", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNegativeCache", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNegativeCache")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNegativeCache", 1)],
            },
            new TweakDef
            {
                Id = "dnssec-disable-register-ptr",
                Label = "Disable DNS PTR Record Registration",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows DNS clients automatically register PTR reverse-lookup records in DNS to allow hostname resolution from IP addresses. Disabling PTR record registration prevents workstations from adding reverse-lookup entries to the DNS zone. Unauthorized or stale PTR records in reverse-lookup zones can cause security validation failures and introduce naming inconsistencies. Reverse DNS zones in enterprise environments are managed through infrastructure provisioning and should not be dynamically modified by clients. Dynamic PTR registration also discloses the hostname of enterprise endpoints to anyone with DNS query access to the reverse zone. Disabling PTR registration reduces the attack surface from reverse-lookup zone manipulation on managed endpoints.",
                Tags = ["dns", "ptr", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RegisterReverseLookup", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "RegisterReverseLookup")],
                DetectOps = [RegOp.CheckDword(Key, "RegisterReverseLookup", 0)],
            },
            new TweakDef
            {
                Id = "dnssec-disable-suffix-search",
                Label = "Restrict DNS Suffix Search List",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The DNS suffix search list specifies domain suffixes appended to unqualified hostnames during DNS resolution on Windows clients. Restricting the search list to only approved enterprise domain suffixes prevents resolution via unauthorized or external DNS domains. DNS suffix search lists that include external domains can cause inadvertent DNS resolution of internal names through external authoritative servers. An overly permissive search list may leak internal hostname attempts to external resolvers. Enterprise DNS search lists should be set to only the organization's internal domain names through Group Policy. Restricting the search list is applied in conjunction with setting an explicit approved list of enterprise suffixes.",
                Tags = ["dns", "search-list", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AppendToMultiLabelName", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AppendToMultiLabelName")],
                DetectOps = [RegOp.CheckDword(Key, "AppendToMultiLabelName", 0)],
            },
            new TweakDef
            {
                Id = "dnssec-disable-over-tcp",
                Label = "Disable DNS Resolution Fallback over TCP",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "DNS clients fall back to TCP when UDP responses are truncated to accommodate large DNS responses such as those using DNSSEC. Disabling TCP fallback prevents the DNS client from retrying queries over TCP when UDP responses are truncated. In environments without DNSSEC or large TXT record needs, UDP responses are rarely truncated and TCP fallback is never triggered. Disabling TCP fallback may cause resolution failures for domains with very large DNS record sets that exceed UDP payload limits. Enterprise environments using DNSSEC validation should not disable TCP fallback as DNSSEC signatures require it. This setting is only appropriate for environments with very constrained DNS infrastructure where TCP is unavailable.",
                Tags = ["dns", "tcp", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDnsOverTcpFallback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDnsOverTcpFallback")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDnsOverTcpFallback", 1)],
            },
            new TweakDef
            {
                Id = "dnssec-disable-updateregistration",
                Label = "Disable Dynamic DNS A Record Registration",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows clients automatically register their IP-to-hostname A records with the DNS server through dynamic DNS updates. Disabling dynamic A record registration prevents workstations from automatically adding or updating their hostname entries in DNS. Dynamic DNS registration from client workstations can lead to DNS pollution if clients change IP addresses or names frequently. Enterprise DNS should be managed through DHCP server-side registration or infrastructure provisioning tools. Client-initiated DNS updates are harder to audit and control than server-managed DNS record maintenance. Disabling client-side DNS registration ensures that DNS records are only modified by authorized infrastructure management processes.",
                Tags = ["dns", "registration", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RegistrationEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "RegistrationEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "RegistrationEnabled", 0)],
            },
        ];
    }

    // ── DohEnforcementPolicy ──
    private static class _DohEnforcementPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DNSClient";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "dohpol-enable-doh-require",
                    Label = "Enable DNS-over-HTTPS — Require Mode",
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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
                    Id = "dohpol-disable-netbios-name-resolution",
                    Label = "Disable NetBIOS Name Resolution",
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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

    // ── DynamicDataExchangePolicy ──
    private static class _DynamicDataExchangePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DDE";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ddepol-disable-dde-protocol",
                Label = "Disable Dynamic Data Exchange Protocol",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Dynamic Data Exchange is a Windows inter-process communication mechanism that predates modern IPC alternatives and allows applications to exchange data. Disabling DDE prevents applications from using the legacy DDE protocol for inter-process communication and data sharing. DDE has been exploited to execute arbitrary code through malicious Office documents and file paths containing DDE expressions. Microsoft Office applications disabled DDE auto-execution in response to widespread exploitation of DDE-based command injection. Legacy DDE usage has been largely superseded by COM, OLE, and modern IPC mechanisms in contemporary applications. Disabling DDE reduces the attack surface associated with protocol-based code execution while minimally impacting modern applications.",
                Tags = ["dde", "ipc", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDDE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDDE")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDDE", 1)],
            },
            new TweakDef
            {
                Id = "ddepol-disable-dde-server-launch",
                Label = "Disable DDE Server Launch Through Shell",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "DDE server processes can be launched by the Windows Shell when a client application requests a DDE connection. Disabling DDE server launch through the shell prevents automatic execution of DDE topics registered in the shell file association database. Malicious file associations can register DDE command topics that execute arbitrary code when files are opened through the shell. DDE-based code execution through file associations was used in targeted attacks against organizations and government entities. Preventing shell-launched DDE server processes removes an often-overlooked code execution pathway associated with file handling. This setting does not affect documented and supported DDE applications using the full DDE API directly.",
                Tags = ["dde", "shell", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDdeServerLaunch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDdeServerLaunch")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDdeServerLaunch", 1)],
            },
            new TweakDef
            {
                Id = "ddepol-disable-network-dde",
                Label = "Disable Network DDE Service",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Network DDE extends the DDE protocol to allow data exchange between applications running on different network computers. Disabling Network DDE removes the ability to conduct cross-computer DDE communication through the network transport. Network DDE services (NetDDE) are legacy Windows services rarely required by modern applications and represent an unnecessary attack surface. Network DDE services on accessible endpoints have been targeted for exploitation in lateral movement techniques. The NetDDE service allows remote parties to initiate DDE connections subject to network security control bypass. Disabling Network DDE eliminates a lateral movement pathway and does not affect local applications or local DDE communication.",
                Tags = ["dde", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNetworkDDE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkDDE")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNetworkDDE", 1)],
            },
            new TweakDef
            {
                Id = "ddepol-disable-remote-dde",
                Label = "Disable Remote DDE Connections",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Remote DDE connections allow applications on one system to connect to DDE applications running on remote network computers. Disabling remote DDE connections prevents inbound and outbound DDE connections across network boundaries on the endpoint. Remote DDE represents an attack vector for reaching applications that publish DDE topics over the network. Controlling lateral movement through enterprise networks requires restricting legacy protocols not needed for regular business operations. DDE-based lateral movement has been documented in APT actor techniques for moving between network-connected Windows endpoints. Modern applications and workflows do not require remote DDE connectivity and should use authenticated network protocols instead.",
                Tags = ["dde", "remote", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRemoteDDE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRemoteDDE")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRemoteDDE", 1)],
            },
            new TweakDef
            {
                Id = "ddepol-disable-dde-auto-link",
                Label = "Disable DDE Auto-Link Updates",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "DDE auto-link updates automatically refresh linked data in documents when source data changes in other linked applications. Disabling DDE auto-link updates prevents automatic execution of DDE expressions embedded in documents when documents are opened or refreshed. Auto-link DDE expressions were abused to execute command shells through field codes in Office documents. Disabling auto-link removal addresses a specific attack vector while preserving the ability for users to manually refresh links. Documents with malicious auto-link DDE fields could execute arbitrary code without user awareness in unpatched configurations. Disabling automatic DDE link refreshing is part of the broader mitigation of DDE-based document attack vectors.",
                Tags = ["dde", "auto-link", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoLink", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoLink")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoLink", 1)],
            },
            new TweakDef
            {
                Id = "ddepol-disable-dde-in-explorer",
                Label = "Disable DDE in Windows Explorer File Associations",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Windows Explorer uses file associations including DDE commands to launch applications when files are opened by double-clicking. Disabling DDE in Explorer file associations prevents DDE command sequences from executing when users open files through the shell. File association DDE commands are the most common exploitation pathway for DDE-based code execution attacks. Malicious documents with crafted file names or embedded associations can trigger DDE execution through Explorer shell operations. Removing DDE from Explorer file handling ensures that file opens go through direct application launch rather than DDE topic invocation. This setting is one of the most impactful DDE mitigations for preventing file-open-triggered code execution.",
                Tags = ["dde", "explorer", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableExplorerDDE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableExplorerDDE")],
                DetectOps = [RegOp.CheckDword(Key, "DisableExplorerDDE", 1)],
            },
            new TweakDef
            {
                Id = "ddepol-disable-dde-in-hyperlinks",
                Label = "Disable DDE in Hyperlink Resolution",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Hyperlinks in documents and emails can reference DDE topics as targets allowing code execution when the link is clicked. Disabling DDE in hyperlink resolution prevents linked references from triggering DDE-based application invocation. Malicious emails and documents with crafted hyperlinks can execute arbitrary commands through DDE-enabled link resolution. Hyperlink-triggered DDE execution is particularly dangerous as it is initiated by expected user action that appears legitimate. Removing DDE support from hyperlink handling closes a significant social engineering attack surface that has been used in phishing campaigns. Legitimate hyperlinks targeting web URLs and file paths continue to work normally after DDE hyperlink resolution is disabled.",
                Tags = ["dde", "hyperlinks", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHyperlinkDDE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHyperlinkDDE")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHyperlinkDDE", 1)],
            },
            new TweakDef
            {
                Id = "ddepol-disable-dde-warning-bypass",
                Label = "Prevent DDE Security Warning Bypass",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Office applications display security warnings before executing DDE commands embedded in documents or when connecting to external DDE servers. Preventing DDE security warning bypass ensures that users cannot suppress warnings that protect against DDE-based exploitation. When users are allowed to bypass security prompts through persistence settings malicious DDE execution can proceed silently. Security prompts serve as the last line of defense against DDE attacks when technical prevention measures are not fully effective. Maintaining mandatory security warnings prevents users from unknowingly allowing malicious DDE connections through habitual prompt dismissal. DDE warnings should be considered alongside application-level DDE disabling for comprehensive coverage.",
                Tags = ["dde", "warnings", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDDEWarningBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDDEWarningBypass")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDDEWarningBypass", 1)],
            },
            new TweakDef
            {
                Id = "ddepol-audit-dde-usage",
                Label = "Enable DDE Usage Audit Logging",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "DDE audit logging records attempts to invoke DDE connections or execute DDE topics on the endpoint. Enabling DDE audit logging provides visibility into remaining DDE usage that can be used to identify applications relying on legacy DDE communication. Understanding remaining DDE usage is essential before fully disabling DDE to avoid breaking business-critical applications. Audit logs also capture potential DDE exploitation attempts allowing security teams to detect and investigate suspicious activity. DDE usage data in the event log can be forwarded to SIEM systems for correlation with other security events. Audit logging has minimal performance impact and provides valuable operational and security intelligence for DDE management.",
                Tags = ["dde", "audit", "logging", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditDDEUsage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditDDEUsage")],
                DetectOps = [RegOp.CheckDword(Key, "AuditDDEUsage", 1)],
            },
            new TweakDef
            {
                Id = "ddepol-disable-clipboard-dde",
                Label = "Disable DDE via Clipboard Operations",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "The clipboard can carry DDE format data that establishes DDE connections when pasted into DDE-aware applications. Disabling clipboard DDE prevents DDE link establishment through clipboard paste operations. Clipboard-based DDE attack delivery allows attackers to initiate DDE connections without file system involvement. Social engineering attacks can instruct users to paste clipboard contents into applications which then trigger DDE-based code execution. Disabling DDE clipboard format reduces the attack surface for clipboard-delivery of DDE attack payloads. Applications that legitimately rely on clipboard-based DDE data exchange will require alternative data sharing methods.",
                Tags = ["dde", "clipboard", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableClipboardDDE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardDDE")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClipboardDDE", 1)],
            },
        ];
    }

    // ── EapNetworkPolicy ──
    private static class _EapNetworkPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EAP";
        private const string PeapKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EAP\PEAP";
        private const string ClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EAP\Client";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "eappol-require-server-cert-validation",
                    Label = "Require Server Certificate Validation for EAP",
                    Category = "Network",
                    Description =
                        "Forces EAP authentication to validate the RADIUS/NPS server certificate before accepting the authentication challenge. Without this, a rogue access point with a fake RADIUS server can capture credentials. Default: validation may be skipped depending on supplicant configuration. Recommended: 1.",
                    Tags = ["eap", "802.1x", "certificate", "radius", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "EAP refuses to complete authentication if the RADIUS server certificate fails validation; rogue AP attacks are blocked.",
                    ApplyOps = [RegOp.SetDword(PeapKey, "RequireServerCertValidation", 1)],
                    RemoveOps = [RegOp.DeleteValue(PeapKey, "RequireServerCertValidation")],
                    DetectOps = [RegOp.CheckDword(PeapKey, "RequireServerCertValidation", 1)],
                },
                new TweakDef
                {
                    Id = "eappol-disable-simple-certificate-selection",
                    Label = "Disable Simple Certificate Selection for EAP-TLS",
                    Category = "Network",
                    Description =
                        "Disables the automatic (heuristic) certificate selection for EAP-TLS / PEAP authentication. When enabled, Windows auto-selects a certificate without user confirmation — which can choose an expired or unintended certificate. Forcing explicit selection ensures the correct certificate is always used. Default: automatic selection enabled. Recommended: 1.",
                    Tags = ["eap", "eap-tls", "certificate", "selection", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Certificate selection is not automatic; users/supplicants must explicitly select the correct certificate for EAP-TLS.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "DisableSimpleCertSelection", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "DisableSimpleCertSelection")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "DisableSimpleCertSelection", 1)],
                },
                new TweakDef
                {
                    Id = "eappol-enable-fast-reconnect",
                    Label = "Enable EAP Fast Reconnect (PEAP Session Resumption)",
                    Category = "Network",
                    Description =
                        "Enables PEAP fast reconnect which allows a client to resume an authenticated session without a full re-authentication when roaming between access points or reconnecting after a brief disconnection. Reduces authentication latency on Wi-Fi roaming without weakening security. Default: fast reconnect disabled in strict environments. Recommended: 1.",
                    Tags = ["eap", "peap", "fast-reconnect", "wifi", "roaming", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "PEAP sessions resume quickly on reconnect/roam without full RADIUS re-authentication; Wi-Fi hand-off is seamless.",
                    ApplyOps = [RegOp.SetDword(PeapKey, "FastReconnect", 1)],
                    RemoveOps = [RegOp.DeleteValue(PeapKey, "FastReconnect")],
                    DetectOps = [RegOp.CheckDword(PeapKey, "FastReconnect", 1)],
                },
                new TweakDef
                {
                    Id = "eappol-disable-identity-privacy",
                    Label = "Disable EAP Identity Privacy (Anonymous Outer Identity)",
                    Category = "Network",
                    Description =
                        "By default PEAP sends an outer anonymous identity (e.g., 'anonymous') to keep the real username hidden from unauthenticated observers. On tightly controlled networks where the RADIUS server already knows all identities, the anonymous outer identity adds unnecessary overhead. Setting this to 0 reveals the actual username in the outer EAP exchange. Recommended: leave as default (1) unless anonymous identity causes RADIUS matching issues.",
                    Tags = ["eap", "peap", "identity", "privacy", "radius", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 3,
                    ImpactNote = "Real username is exposed in outer EAP exchange; RADIUS server sees the actual identity at the network layer.",
                    ApplyOps = [RegOp.SetDword(PeapKey, "DisableIdentityPrivacy", 0)],
                    RemoveOps = [RegOp.DeleteValue(PeapKey, "DisableIdentityPrivacy")],
                    DetectOps = [RegOp.CheckDword(PeapKey, "DisableIdentityPrivacy", 0)],
                },
                new TweakDef
                {
                    Id = "eappol-require-cryptobinding",
                    Label = "Require Cryptobinding for PEAP",
                    Category = "Network",
                    Description =
                        "Requires cryptobinding TLV in PEAP Type-Length-Value exchanges to bind the inner and outer authentication channels. Cryptobinding prevents channel-binding attacks where an attacker relays an inner authentication from a different outer TLS tunnel. Default: not required by all RADIUS implementations. Recommended: 1 where RADIUS supports it.",
                    Tags = ["eap", "peap", "cryptobinding", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "PEAP requires cryptobinding; channel-binding relay attacks between different TLS sessions are prevented.",
                    ApplyOps = [RegOp.SetDword(PeapKey, "RequireCryptobinding", 1)],
                    RemoveOps = [RegOp.DeleteValue(PeapKey, "RequireCryptobinding")],
                    DetectOps = [RegOp.CheckDword(PeapKey, "RequireCryptobinding", 1)],
                },
                new TweakDef
                {
                    Id = "eappol-disable-eap-md5",
                    Label = "Disable EAP-MD5 Authentication Method",
                    Category = "Network",
                    Description =
                        "Removes EAP-MD5 from the list of permitted EAP methods. MD5 uses a challenge-response scheme with a one-way hash that is vulnerable to dictionary and offline brute-force attacks. Operators should use EAP-TLS or PEAP instead. Default: MD5 may be offered. Recommended: 1.",
                    Tags = ["eap", "md5", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "EAP-MD5 authentication is blocked; clients must use a stronger method such as EAP-TLS or PEAP.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableEAPMD5", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableEAPMD5")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableEAPMD5", 1)],
                },
                new TweakDef
                {
                    Id = "eappol-log-authentication-events",
                    Label = "Enable EAP Authentication Event Logging",
                    Category = "Network",
                    Description =
                        "Records successful and failed EAP / 802.1X authentication attempts to the Security event log. Provides visibility into network authentication activity including unexpected failures that may indicate a rogue access point or credential attack. Default: EAP events not always forwarded to Security log. Recommended: 1.",
                    Tags = ["eap", "802.1x", "audit", "logging", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "EAP authentication successes and failures are written to the Security event log.",
                    ApplyOps = [RegOp.SetDword(Key, "LogSuccessfulAuthentications", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogSuccessfulAuthentications")],
                    DetectOps = [RegOp.CheckDword(Key, "LogSuccessfulAuthentications", 1)],
                },
                new TweakDef
                {
                    Id = "eappol-set-max-auth-failures",
                    Label = "Set Maximum EAP Authentication Failures to 3",
                    Category = "Network",
                    Description =
                        "Limits the number of consecutive EAP authentication failures before the supplicant stops retrying. Limits brute-force attempts against the RADIUS server from a single endpoint and reduces event log noise from misconfigured supplicants. Default: may retry indefinitely. Recommended: 3.",
                    Tags = ["eap", "authentication", "retry", "brute-force", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "After 3 consecutive authentication failures the supplicant stops retrying; manual intervention is needed to reconnect.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxAuthFailures", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxAuthFailures")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxAuthFailures", 3)],
                },
                new TweakDef
                {
                    Id = "eappol-require-mutual-auth",
                    Label = "Require Mutual Authentication for EAP",
                    Category = "Network",
                    Description =
                        "Enforces bidirectional (mutual) authentication in EAP exchanges — both the client and the RADIUS server must authenticate to each other. Without mutual auth, a client may authenticate to a rogue server without the server proving its own identity. Default: some EAP types do not enforce mutual auth. Recommended: 1.",
                    Tags = ["eap", "mutual-auth", "radius", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Both client and server must authenticate; one-sided authentication and rogue-RADIUS attacks are prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireMutualAuthentication", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireMutualAuthentication")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireMutualAuthentication", 1)],
                },
                new TweakDef
                {
                    Id = "eappol-block-nontls-methods",
                    Label = "Block Non-TLS EAP Methods on Corporate Networks",
                    Category = "Network",
                    Description =
                        "Restricts 802.1X authentication to TLS-based EAP methods (EAP-TLS, PEAP-TLS) only. Legacy password-based EAP types (LEAP, EAP-PAP) that transmit or derive credentials without TLS protection are blocked. Default: legacy methods permitted. Recommended: 1 in enterprise 802.1X deployments.",
                    Tags = ["eap", "tls", "legacy", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only TLS-protected EAP methods are accepted; password-based EAP types without TLS are rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowOnlyTLSBasedMethods", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowOnlyTLSBasedMethods")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowOnlyTLSBasedMethods", 1)],
                },
            ];
    }

    // ── HomeGroupPolicy ──
    private static class _HomeGroupPolicy
    {
        private const string HomeGroupKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HomeGroup";
        private const string SharingKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections";
        private const string WorkplaceKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "homegroup-block-sharing-wizard",
                Label = "HomeGroup: Block Network Sharing Wizard",
                Category = "Network",
                Description =
                    "Disables the Windows Network Setup and Sharing Wizard that appears when connecting to a new network, which includes prompts to share files and configure HomeGroup-style sharing. The sharing wizard can inadvertently configure broad file-sharing settings when users accept defaults without understanding the implications. Blocking it ensures network sharing is configured intentionally through a dedicated management interface.",
                Tags = ["homegroup", "sharing wizard", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [HomeGroupKey],
                ApplyOps = [RegOp.SetDword(HomeGroupKey, "DisableSharingWizard", 1)],
                RemoveOps = [RegOp.DeleteValue(HomeGroupKey, "DisableSharingWizard")],
                DetectOps = [RegOp.CheckDword(HomeGroupKey, "DisableSharingWizard", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses the new-network sharing wizard; network sharing can still be configured manually.",
            },
            new TweakDef
            {
                Id = "homegroup-block-homegroup-creation",
                Label = "HomeGroup: Block HomeGroup Creation by Users",
                Category = "Network",
                Description =
                    "Prevents standard users from creating new HomeGroups. Even on systems where HomeGroup-related features have been removed at the UI level, the underlying service (HomeGroupProvider) may still be accessible through PowerShell or legacy APIs. This policy enforces a mandatory block on new HomeGroup creation regardless of the access method used.",
                Tags = ["homegroup", "creation", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [HomeGroupKey],
                ApplyOps = [RegOp.SetDword(HomeGroupKey, "DisableHomeGroupCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(HomeGroupKey, "DisableHomeGroupCreation")],
                DetectOps = [RegOp.CheckDword(HomeGroupKey, "DisableHomeGroupCreation", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks HomeGroup creation; no functional loss on modern Windows 10/11 where the feature is removed.",
            },
            new TweakDef
            {
                Id = "homegroup-block-join-existing",
                Label = "HomeGroup: Block Joining Existing HomeGroups",
                Category = "Network",
                Description =
                    "Prevents users on this machine from joining existing HomeGroups on the local network. If older machines on the same LAN still have active HomeGroups, this policy blocks the current machine from joining them and accessing their shared resources. This is particularly important in mixed-version environments where Windows 7/8.1 machines may still be using HomeGroup.",
                Tags = ["homegroup", "join", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [HomeGroupKey],
                ApplyOps = [RegOp.SetDword(HomeGroupKey, "DisableHomeGroupJoin", 1)],
                RemoveOps = [RegOp.DeleteValue(HomeGroupKey, "DisableHomeGroupJoin")],
                DetectOps = [RegOp.CheckDword(HomeGroupKey, "DisableHomeGroupJoin", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks joining legacy HomeGroups from other machines on the LAN; no impact on modern networks.",
            },
            new TweakDef
            {
                Id = "homegroup-disable-sharing-library-access",
                Label = "HomeGroup: Disable Shared Library Access via HomeGroup",
                Category = "Network",
                Description =
                    "Disables access to shared libraries (Music, Pictures, Videos, Documents folders) through any active HomeGroup connection. Even when HomeGroup creation is blocked, access to legacy shares may persist if the machine was previously part of a HomeGroup. This policy ensures that the library sharing mechanism cannot be used to access data through legacy share paths, regardless of prior HomeGroup membership.",
                Tags = ["homegroup", "library sharing", "pictures", "music", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [HomeGroupKey],
                ApplyOps = [RegOp.SetDword(HomeGroupKey, "DisableSharedLibraryAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(HomeGroupKey, "DisableSharedLibraryAccess")],
                DetectOps = [RegOp.CheckDword(HomeGroupKey, "DisableSharedLibraryAccess", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents library browsing via HomeGroup paths; does not remove regular SMB shares.",
            },
            new TweakDef
            {
                Id = "homegroup-block-network-bridge",
                Label = "HomeGroup: Block Network Bridge Creation",
                Category = "Network",
                Description =
                    "Prevents users from creating a network bridge between multiple network adapters. A network bridge connects two otherwise isolated network segments (e.g., a wired corporate LAN and a personal Wi-Fi hotspot), creating a routing path that bypasses network access controls. This is a long-standing attack vector for data exfiltration and is blocked in any CIS Benchmark Level 1 configuration.",
                Tags = ["homegroup", "network bridge", "network isolation", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SharingKey],
                ApplyOps = [RegOp.SetDword(SharingKey, "NC_AllowNetBridgeAdapters", 0)],
                RemoveOps = [RegOp.DeleteValue(SharingKey, "NC_AllowNetBridgeAdapters")],
                DetectOps = [RegOp.CheckDword(SharingKey, "NC_AllowNetBridgeAdapters", 0)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents creating network bridges; a significant security control for preventing LAN isolation bypass.",
            },
            new TweakDef
            {
                Id = "homegroup-block-internet-connection-sharing",
                Label = "HomeGroup: Block ICS UI for Current User (User-Scope Policy)",
                Category = "Network",
                Description =
                    "Prevents the current user from enabling Internet Connection Sharing (ICS) by hiding the sharing tab in network adapter properties at the user-policy level. This user-scope complement to the machine-scope ICS restriction ensures that even when a user navigates to the Network Connections UI on their own account, the option to configure ICS is hidden. Combined with the machine-scope block, this creates defence-in-depth against ad-hoc NAT routing.",
                Tags = ["homegroup", "ics", "internet sharing", "user policy", "network"],
                NeedsAdmin = false,
                CorpSafe = true,
                RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Network Connections"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Network Connections", "NC_ShowSharedAccessUI", 0)],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Network Connections", "NC_ShowSharedAccessUI"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Network Connections", "NC_ShowSharedAccessUI", 0),
                ],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes ICS option from the user's adapter properties; user-scope complement to machine-level ICS block.",
            },
            new TweakDef
            {
                Id = "homegroup-suppress-peer-name-resolution",
                Label = "HomeGroup: Suppress Peer Name Resolution Protocol (PNRP)",
                Category = "Network",
                Description =
                    "Disables the Peer Name Resolution Protocol (PNRP) service, which HomeGroup and Windows Collaboration used for peer-to-peer device discovery on the local network. PNRP announces the machine's presence and identity to other devices on the same network segment and was a prerequisite for HomeGroup operation. On corporate networks, PNRP can result in inadvertent machine exposure and is not needed when domain-based or MDM-managed directory services are used.",
                Tags = ["homegroup", "pnrp", "peer to peer", "discovery", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [HomeGroupKey],
                ApplyOps = [RegOp.SetDword(HomeGroupKey, "DisablePeerNameResolution", 1)],
                RemoveOps = [RegOp.DeleteValue(HomeGroupKey, "DisablePeerNameResolution")],
                DetectOps = [RegOp.CheckDword(HomeGroupKey, "DisablePeerNameResolution", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables PNRP peer discovery; no functional impact on modern managed networks that do not use HomeGroup.",
            },
            new TweakDef
            {
                Id = "homegroup-block-homegroup-listener",
                Label = "HomeGroup: Disable HomeGroup Listener Service Policy",
                Category = "Network",
                Description =
                    "Applies the Group Policy flag that prevents the HomeGroupListener service from starting. The HomeGroupListener service (on older Windows 10 builds or those upgraded from Windows 8.1) processes HomeGroup notifications and sharing requests from network peers. Disabling it via policy ensures the service is blocked even if a service configuration change or script attempts to start it during a user session.",
                Tags = ["homegroup", "listener", "service", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [HomeGroupKey],
                ApplyOps = [RegOp.SetDword(HomeGroupKey, "DisableHomeGroupListener", 1)],
                RemoveOps = [RegOp.DeleteValue(HomeGroupKey, "DisableHomeGroupListener")],
                DetectOps = [RegOp.CheckDword(HomeGroupKey, "DisableHomeGroupListener", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks HomeGroup network listener; equivalent to disabling the service but enforced via policy.",
            },
        ];
    }

    // ── HotspotAuthenticationPolicy ──
    private static class _HotspotAuthenticationPolicy
    {
        private const string HsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HotspotAuthentication";
        private const string WcmLocal = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\Local";
        private const string WirelessGpt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Wireless\GPTWirelessPolicy";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "hotspot-disable-captive-portal",
                Label = "Disable Captive Portal Detection",
                Category = "Network",
                Description =
                    "Sets Enabled=0 in the HotspotAuthentication policy key. "
                    + "Prevents Windows from detecting captive portal Wi-Fi hotspots and launching the "
                    + "browser-based authentication dialog. Reduces network probing and location privacy leakage "
                    + "on public or untrusted networks. "
                    + "Default: absent (captive portal detection on). Recommended: 0 on corporate or locked-down devices.",
                Tags = ["hotspot", "captive-portal", "wifi", "authentication", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Captive portal auto-detect and browser pop-up disabled; user must manually navigate to the portal.",
                ApplyOps = [RegOp.SetDword(HsKey, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(HsKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(HsKey, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "hotspot-disable-auto-connect-new",
                Label = "Disable Auto-Connect to New Wi-Fi Networks",
                Category = "Network",
                Description =
                    "Sets fBlockNonDomain=1 in the WcmSvc Local policy key. "
                    + "Prevents Windows from automatically connecting to new or unknown Wi-Fi networks, "
                    + "including open hotspots. Only pre-configured domain or saved networks are allowed. "
                    + "Default: absent (auto-connect allowed). Recommended: 1 on corporate domain machines.",
                Tags = ["hotspot", "wifi", "auto-connect", "domain", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Windows will not automatically connect to new Wi-Fi hotspots; only saved networks are used.",
                ApplyOps = [RegOp.SetDword(WcmLocal, "fBlockNonDomain", 1)],
                RemoveOps = [RegOp.DeleteValue(WcmLocal, "fBlockNonDomain")],
                DetectOps = [RegOp.CheckDword(WcmLocal, "fBlockNonDomain", 1)],
            },
            new TweakDef
            {
                Id = "hotspot-disable-internet-sharing",
                Label = "Disable Wi-Fi Internet Connection Sharing",
                Category = "Network",
                Description =
                    "Sets NC_ShowSharedAccessUI=0 in the WcmSvc Local policy key. "
                    + "Hides and disables the Internet Connection Sharing (ICS) functionality in Windows "
                    + "network connection properties, preventing the device from acting as a Wi-Fi hotspot. "
                    + "Default: absent (ICS UI shown). Recommended: 0 on corporate devices.",
                Tags = ["hotspot", "ics", "sharing", "wifi", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Internet Connection Sharing (ICS) hotspot functionality hidden and disabled from the UI.",
                ApplyOps = [RegOp.SetDword(WcmLocal, "NC_ShowSharedAccessUI", 0)],
                RemoveOps = [RegOp.DeleteValue(WcmLocal, "NC_ShowSharedAccessUI")],
                DetectOps = [RegOp.CheckDword(WcmLocal, "NC_ShowSharedAccessUI", 0)],
            },
            new TweakDef
            {
                Id = "hotspot-disable-wifi-sense",
                Label = "Disable Wi-Fi Sense Contact Sharing",
                Category = "Network",
                Description =
                    "Sets fScanConnectIntervalNearby=0 in the WcmSvc Local policy key. "
                    + "Disables the Wi-Fi Sense feature that automatically shares Wi-Fi passwords with "
                    + "contacts via Microsoft account. Prevents credential sharing across devices and accounts. "
                    + "Default: absent (Wi-Fi Sense on). Recommended: 0 for credential hygiene.",
                Tags = ["hotspot", "wifi-sense", "sharing", "contacts", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Wi-Fi Sense nearby network sharing and credential auto-exchange disabled.",
                ApplyOps = [RegOp.SetDword(WcmLocal, "fScanConnectIntervalNearby", 0)],
                RemoveOps = [RegOp.DeleteValue(WcmLocal, "fScanConnectIntervalNearby")],
                DetectOps = [RegOp.CheckDword(WcmLocal, "fScanConnectIntervalNearby", 0)],
            },
            new TweakDef
            {
                Id = "hotspot-disable-manual-hotspot",
                Label = "Disable Mobile Hotspot Feature",
                Category = "Network",
                Description =
                    "Sets AllowHotspot=0 in the WcmSvc Local policy key. "
                    + "Prevents users from enabling the Windows Mobile Hotspot feature, which turns the "
                    + "device into a Wi-Fi hotspot sharing its internet connection. "
                    + "Default: absent (hotspot allowed). Recommended: 0 on corporate devices to prevent unauthorized internet sharing.",
                Tags = ["hotspot", "mobile-hotspot", "sharing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Windows Mobile Hotspot feature disabled; users cannot share internet via Wi-Fi.",
                ApplyOps = [RegOp.SetDword(WcmLocal, "AllowHotspot", 0)],
                RemoveOps = [RegOp.DeleteValue(WcmLocal, "AllowHotspot")],
                DetectOps = [RegOp.CheckDword(WcmLocal, "AllowHotspot", 0)],
            },
            new TweakDef
            {
                Id = "hotspot-disable-hotspot2",
                Label = "Disable Wi-Fi Hotspot 2.0 / Passpoint",
                Category = "Network",
                Description =
                    "Sets fDisablePassport=1 in the HotspotAuthentication policy key. "
                    + "Disables the Wi-Fi Hotspot 2.0 (Passpoint / 802.11u) automatic authentication protocol. "
                    + "Prevents Windows from automatically authenticating to Hotspot 2.0-capable public networks "
                    + "using stored service credentials. "
                    + "Default: absent. Recommended: 1 to prevent auto-auth to unknown carrier Wi-Fi networks.",
                Tags = ["hotspot", "hotspot2", "passpoint", "802.11u", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hotspot 2.0 / Passpoint auto-authentication disabled; public carrier Wi-Fi not auto-joined.",
                ApplyOps = [RegOp.SetDword(HsKey, "fDisablePassport", 1)],
                RemoveOps = [RegOp.DeleteValue(HsKey, "fDisablePassport")],
                DetectOps = [RegOp.CheckDword(HsKey, "fDisablePassport", 1)],
            },
            new TweakDef
            {
                Id = "hotspot-disable-network-roaming",
                Label = "Disable Wi-Fi Network Roaming",
                Category = "Network",
                Description =
                    "Sets DisableRoaming=1 in the WcmSvc Local policy key. "
                    + "Prevents the Windows wireless service from automatically roaming between Wi-Fi access points, "
                    + "including between networks with the same SSID at different locations. "
                    + "Locks the device to its current network association until manually disconnected. "
                    + "Default: absent (roaming enabled). Recommended: 1 on fixed workstations.",
                Tags = ["hotspot", "roaming", "wifi", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Wi-Fi automatic roaming between access points/SSIDs disabled.",
                ApplyOps = [RegOp.SetDword(WcmLocal, "DisableRoaming", 1)],
                RemoveOps = [RegOp.DeleteValue(WcmLocal, "DisableRoaming")],
                DetectOps = [RegOp.CheckDword(WcmLocal, "DisableRoaming", 1)],
            },
            new TweakDef
            {
                Id = "hotspot-disable-wlan-autoconfig",
                Label = "Block WLAN AutoConfig Profile Changes",
                Category = "Network",
                Description =
                    "Sets fPreventAutoConnectToWiFiSenseHotspots=1 in the HotspotAuthentication policy key. "
                    + "Prevents WLAN AutoConfig from applying automatic Wi-Fi profile changes from the Hotspot "
                    + "authentication service. Ensures only IT-provisioned wireless profiles are used. "
                    + "Default: absent. Recommended: 1 on corporate devices.",
                Tags = ["hotspot", "wlan", "autoconfig", "profile", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WLAN AutoConfig cannot apply hotspot-originated wireless profile changes.",
                ApplyOps = [RegOp.SetDword(HsKey, "fPreventAutoConnectToWiFiSenseHotspots", 1)],
                RemoveOps = [RegOp.DeleteValue(HsKey, "fPreventAutoConnectToWiFiSenseHotspots")],
                DetectOps = [RegOp.CheckDword(HsKey, "fPreventAutoConnectToWiFiSenseHotspots", 1)],
            },
            new TweakDef
            {
                Id = "hotspot-disable-wireless-gpt-policy",
                Label = "Block GPT Wireless Policy Push",
                Category = "Network",
                Description =
                    "Sets fEnableGPTWirelessPolicy=0 in the GPTWirelessPolicy key. "
                    + "Prevents Windows Wireless Group Policy from applying Group Policy Template (GPT) wireless "
                    + "profiles pushed from an Active Directory GPO. Useful when wireless profiles are managed "
                    + "by a third-party MDM or RADIUS and AD wireless GPOs are not used. "
                    + "Default: absent (GPT policy applied). Recommended: 0 in non-AD wireless deployments.",
                Tags = ["hotspot", "gpt", "wireless", "group-policy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "GPT wireless profile push from AD Group Policy Objects is disabled.",
                ApplyOps = [RegOp.SetDword(WirelessGpt, "fEnableGPTWirelessPolicy", 0)],
                RemoveOps = [RegOp.DeleteValue(WirelessGpt, "fEnableGPTWirelessPolicy")],
                DetectOps = [RegOp.CheckDword(WirelessGpt, "fEnableGPTWirelessPolicy", 0)],
            },
            new TweakDef
            {
                Id = "hotspot-disable-credential-caching",
                Label = "Disable Hotspot Credential Caching",
                Category = "Network",
                Description =
                    "Sets fCacheCredentials=0 in the HotspotAuthentication policy key. "
                    + "Prevents the Hotspot 2.0 authentication service from caching Wi-Fi network "
                    + "credentials (username/password) for previously authenticated public networks. "
                    + "Improves security by forcing re-authentication on each new connection. "
                    + "Default: absent (credentials cached). Recommended: 0 on privacy-conscious devices.",
                Tags = ["hotspot", "credentials", "caching", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hotspot authentication credentials not cached; re-authentication required on every connection.",
                ApplyOps = [RegOp.SetDword(HsKey, "fCacheCredentials", 0)],
                RemoveOps = [RegOp.DeleteValue(HsKey, "fCacheCredentials")],
                DetectOps = [RegOp.CheckDword(HsKey, "fCacheCredentials", 0)],
            },
        ];
    }

    // ── Ieee8021xPolicy ──
    private static class _Ieee8021xPolicy
    {
        private const string WiredKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WiredNetwork";

        private const string WirelessKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WirelessNetwork";

        private const string EapKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EapHost";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ieee8021x-enable-wired-8021x",
                    Label = "IEEE 802.1x: Enable Wired Network 802.1x Authentication",
                    Category = "Network",
                    Description =
                        "Sets EnableAutoConfig=1 in WiredNetwork policy. Activates the Windows wired 802.1x supplicant (the Wired AutoConfig service) and enables port-based network access control on all wired Ethernet adapters. With 802.1x enabled, the switch's authentication server (RADIUS) validates the endpoint's identity before granting network access. Unauthenticated endpoints are placed in a guest VLAN or denied access. Essential security control for protecting internal network access via physical Ethernet port.",
                    Tags = ["ieee8021x", "wired", "authentication", "network", "nac"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Wired 802.1x authentication is enforced. Network switches must support 802.1x and be configured with RADIUS server settings. Machines without valid credentials are denied network access — ensure machine certificates are enrolled first.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "EnableAutoConfig", 1)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "EnableAutoConfig")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "EnableAutoConfig", 1)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-set-eapol-start-timeout",
                    Label = "IEEE 802.1x: Set EAPOL-Start Timeout to 20 Seconds",
                    Category = "Network",
                    Description =
                        "Sets AuthResponse=20 in WiredNetwork policy. Configures the supplicant's EAPOL-Start transmission timer: how long the supplicant waits for an EAPOL Request-Identity from the switch before sending an explicit EAPOL-Start frame. On access switches that do not send the initial EAP Request/Identity packet (authenticator-initiated), the supplicant must initiate. A longer initial wait delays authentication. Setting to 20 seconds reduces the wait for supplicant-initiated authentication without being so short it floods slow switch responses.",
                    Tags = ["ieee8021x", "eapol", "timeout", "supplicant", "wired"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "EAPOL-Start timer set to 20 seconds. Only affects authentication initiation timing. Network connectivity is unchanged once authentication completes.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "AuthResponse", 20)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "AuthResponse")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "AuthResponse", 20)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-set-max-eapol-retransmit",
                    Label = "IEEE 802.1x: Limit EAPOL-Start Retransmit Count to 5",
                    Category = "Network",
                    Description =
                        "Sets MaxEapolStartAttempts=5 in WiredNetwork policy. Limits the number of EAPOL-Start retransmission attempts before the supplicant gives up and treats the port as unauthenticated. Each EAPOL-Start is separated by the AuthResponse timer (AuthResponse seconds). With 5 retransmits at 20 seconds each, the supplicant spends 100 seconds attempting to authenticate before failing. This limits the time a machine retries against an unavailable RADIUS server while ensuring legitimate connection delays are handled gracefully.",
                    Tags = ["ieee8021x", "eapol", "retransmit", "retry", "supplicant"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "EAPOL-Start retransmit limit is 5 attempts. After 5 failures, the supplicant stops trying. Most switches respond to the first EAPOL-Start; retransmits are only relevant for switches with slow RADIUS response.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "MaxEapolStartAttempts", 5)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "MaxEapolStartAttempts")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "MaxEapolStartAttempts", 5)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-enable-single-sign-on",
                    Label = "IEEE 802.1x: Enable Single Sign-On Integration",
                    Category = "Network",
                    Description =
                        "Sets BlockPeriod=0 in WiredNetwork policy (enables SSO) and sets SingleSignOn=1. Configures the wired supplicant to synchronise 802.1x authentication with Windows logon. In SSO mode, machine certificates are used before the Windows logon screen (machine authentication), and user certificates are presented after the user logs in (user authentication). Without SSO, the machine may remain in a partially authenticated state during logon, potentially delaying Group Policy application or network-dependent logon scripts.",
                    Tags = ["ieee8021x", "sso", "logon", "authentication", "wired"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SSO integrates 802.1x with Windows logon. Machine authenticates before logon screen; user authenticates after. Requires both machine and user certificates to be enrolled.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "BlockPeriod", 0), RegOp.SetDword(WiredKey, "SingleSignOn", 1)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "BlockPeriod"), RegOp.DeleteValue(WiredKey, "SingleSignOn")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "BlockPeriod", 0), RegOp.CheckDword(WiredKey, "SingleSignOn", 1)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-disable-user-only-mode",
                    Label = "IEEE 802.1x: Disable User-Only 802.1x Mode (Require Machine Auth)",
                    Category = "Network",
                    Description =
                        "Sets UserOnlyMode=0 in WiredNetwork policy. Prevents the supplicant from operating in user-only authentication mode where only user credentials are sent and machine credentials are not used. In user-only mode, the machine VLAN access depends entirely on the currently logged-in user's authentication; when no user is logged in (e.g., at Windows logon screen, or when machine admin scripts run before user logon), the port may be unauthenticated. Machine authentication ensures the port is authenticated regardless of user logon state.",
                    Tags = ["ieee8021x", "machine-auth", "user-auth", "wired", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Machine authentication is required; user-only mode is disabled. Machine certificates must be enrolled before the logon screen appears. Group Policy and SCCM scripts that run at machine startup require machine-authenticated network access.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "UserOnlyMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "UserOnlyMode")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "UserOnlyMode", 0)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-disable-guest-vlan-access",
                    Label = "IEEE 802.1x: Disable Automatic Guest VLAN Fallback on Auth Failure",
                    Category = "Network",
                    Description =
                        "Sets AllowGuestVLAN=0 in WiredNetwork policy. Prevents the Windows 802.1x supplicant from signalling to the switch that it accepts placement in a guest VLAN on authentication failure. Some switch configurations use EAP-Failure with a VLAN assignment to place unauthenticated endpoints in a restricted guest VLAN. Setting AllowGuestVLAN=0 causes the supplicant to treat authentication failure as a complete block rather than accepting reduced-access guest VLAN placement. Prevents accidental network access via the guest VLAN.",
                    Tags = ["ieee8021x", "guest-vlan", "fallback", "authentication", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Guest VLAN fallback is disabled. Failed authentication results in no network access rather than guest VLAN. Ensure there is an alternate recovery path (console access, out-of-band management) for machines with certificate failures.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "AllowGuestVLAN", 0)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "AllowGuestVLAN")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "AllowGuestVLAN", 0)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-set-eap-type-tls",
                    Label = "IEEE 802.1x: Set Wired EAP Method to EAP-TLS (Certificate-Based)",
                    Category = "Network",
                    Description =
                        "Sets EAPType=13 in WiredNetwork policy (EAP-TLS, EAP type 13 per IANA). Configures the Windows wired 802.1x supplicant to use EAP-TLS as the preferred EAP authentication method. EAP-TLS uses mutual certificate-based authentication: the client presents a certificate (from machine store or smart card) and the RADIUS server presents a server certificate. It is the strongest EAP method available, providing phishing-resistant authentication and protection against credential interception attacks that affect password-based PEAP.",
                    Tags = ["ieee8021x", "eap-tls", "certificate", "authentication", "wired"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "EAP-TLS requires client certificates on all machines. Machine certificates must be enrolled via enterprise CA before deployment. Strongest 802.1x authentication method — recommended for high-security environments.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "EAPType", 13)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "EAPType")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "EAPType", 13)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-enable-eap-inner-identity",
                    Label = "IEEE 802.1x: Enable Anonymous Identity (Outer Identity Privacy)",
                    Category = "Network",
                    Description =
                        "Sets EnableAnonymousIdentity=1 in EapHost policy. Enables the use of an anonymous outer identity in tunnelled EAP methods (PEAP, TTLS). The EAP Identity response (outer identity) is transmitted in plaintext in the first EAP exchange. Without anonymous identity, the user's actual username is visible in the network traffic of the outer EAP exchange, revealing who is authenticating before the TLS tunnel is established. An anonymous outer identity (e.g., 'anonymous@contoso.com') hides the actual username until the protected tunnel is established.",
                    Tags = ["ieee8021x", "eap", "identity", "privacy", "peap"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Anonymous identity is sent as EAP outer identity. The RADIUS server must be configured to match the anonymous realm. The real user identity is still used inside the TLS tunnel for authentication.",
                    ApplyOps = [RegOp.SetDword(EapKey, "EnableAnonymousIdentity", 1)],
                    RemoveOps = [RegOp.DeleteValue(EapKey, "EnableAnonymousIdentity")],
                    DetectOps = [RegOp.CheckDword(EapKey, "EnableAnonymousIdentity", 1)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-disable-cached-wireless-creds",
                    Label = "IEEE 802.1x: Disable Wireless 802.1x Credential Caching",
                    Category = "Network",
                    Description =
                        "Sets DisableUserCredentialCaching=1 in WirelessNetwork policy. Prevents the Windows wireless supplicant from caching 802.1x user credentials to local storage. Some EAP methods (PEAP-MSCHAPv2) can cache credentials to allow re-authentication without prompting the user. While convenient, cached credentials are a persistence mechanism for the credentials and may be accessible from the credential cache if the machine is compromised. Required credentials are re-fetched on each authentication using the user's interactive session or certificate store.",
                    Tags = ["ieee8021x", "wireless", "credential-cache", "security", "wifi"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Wireless 802.1x credentials are not cached. Users may be re-prompted for credentials on reconnection if using PEAP-MSCHAPv2. Certificate-based EAP-TLS is unaffected.",
                    ApplyOps = [RegOp.SetDword(WirelessKey, "DisableUserCredentialCaching", 1)],
                    RemoveOps = [RegOp.DeleteValue(WirelessKey, "DisableUserCredentialCaching")],
                    DetectOps = [RegOp.CheckDword(WirelessKey, "DisableUserCredentialCaching", 1)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-require-mutual-authentication",
                    Label = "IEEE 802.1x: Require Mutual Authentication in PEAP Handshake",
                    Category = "Network",
                    Description =
                        "Sets RequireMutualAuth=1 in EapHost policy. Requires that tunnelled EAP methods (PEAP, TTLS) perform full mutual authentication: the server must present a trusted certificate AND the client must authenticate with a credential inside the TLS tunnel. Without mutual authentication, a rogue access point can complete the outer TLS handshake with any certificate while the client still transmits their inner credentials to an attacker-controlled server. Mutual authentication (server cert validation + valid inner credentials) is the minimum security requirement for PEAP deployments.",
                    Tags = ["ieee8021x", "mutual-authentication", "peap", "certificate", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Mutual authentication is required for PEAP/TTLS. RADIUS/NPS servers must present a trusted certificate. Ensures both server and client authenticate — critical for preventing evil twin attacks.",
                    ApplyOps = [RegOp.SetDword(EapKey, "RequireMutualAuth", 1)],
                    RemoveOps = [RegOp.DeleteValue(EapKey, "RequireMutualAuth")],
                    DetectOps = [RegOp.CheckDword(EapKey, "RequireMutualAuth", 1)],
                },
            ];
    }

    // ── InternetCommunicationPolicy ──
    private static class _InternetCommunicationPolicy
    {
        private const string InetMgmtKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InternetManagement";
        private const string InetRestrictKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "inetcomm-restrict-internet-communication",
                Label = "Internet Communication: Restrict All Internet Communication Features",
                Category = "Network",
                Description =
                    "Enables the master Internet Communication Management policy that restricts or disables all Windows features that communicate with the internet, including Windows Error Reporting, Windows Update, Help and Support Center online search, Microsoft Customer Experience Improvement Program (CEIP), online activation, and other phone-home features. This is the master switch that enables sub-policies defined elsewhere under the InternetManagement key.",
                Tags = ["internet communication", "internet restriction", "privacy", "phone home", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "RestrictInternetCommunication", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "RestrictInternetCommunication")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "RestrictInternetCommunication", 1)],
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "Broad internet communication block; may affect Windows Update, activation, and online Help. Test thoroughly before deploying.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-printing-over-http",
                Label = "Internet Communication: Disable Printing Over HTTP",
                Category = "Network",
                Description =
                    "Disables the ability for Windows to send print jobs over HTTP, which is used for sending documents to Internet Printing Protocol (IPP) printers outside the corporate network. HTTP printing can bypass proxy controls and DLP systems. On managed networks, all printing should be directed to IT-managed printer queues; direct-to-internet IPP printing is a potential data exfiltration vector.",
                Tags = ["internet communication", "printing", "http", "data loss prevention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableHTTPPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableHTTPPrinting")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableHTTPPrinting", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables Internet Printing Protocol (IPP) over HTTP; LAN-connected print servers are unaffected.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-windows-update-access",
                Label = "Internet Communication: Disable Access to Windows Update (Non-WSUS)",
                Category = "Network",
                Description =
                    "Blocks Windows from accessing Microsoft's Windows Update servers directly, restricting update access to corporate WSUS or Windows Update for Business (WUfB) only. Direct Windows Update connections bypass the organization's patch approval process, potentially installing untested updates or creating tracking data. This policy should be combined with a WSUS or Intune Update Ring configuration to ensure updates are still received through an approved channel.",
                Tags = ["internet communication", "windows update", "wsus", "patch management", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableWindowsUpdateAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableWindowsUpdateAccess")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableWindowsUpdateAccess", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Blocks direct access to Windows Update; requires WSUS or WUfB to be configured for updates to be received.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-web-communities",
                Label = "Internet Communication: Disable Windows Web Communities Feature",
                Category = "Network",
                Description =
                    "Disables the Windows Web Communities feature that allowed Windows Explorer and Help Center to automatically submit queries to Microsoft-hosted community websites (forums, knowledge base, support articles). While useful in consumer scenarios, this feature sends details about the user's system context, open documents, and help searches to external Microsoft servers, which creates a data-leakage concern in regulated business environments.",
                Tags = ["internet communication", "communities", "help center", "telemetry", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableWebCommunities", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableWebCommunities")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableWebCommunities", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables Windows community search in Help Center; standard online help articles are unaffected.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-event-viewer-links",
                Label = "Internet Communication: Disable Event Viewer Online Links",
                Category = "Network",
                Description =
                    "Prevents Event Viewer from displaying the 'More Information' link that opens a Microsoft online support page when viewing event log entries. Clicking these links sends event detail data (Event ID, source, parameters) to Microsoft's online event log lookup service. In sensitive environments, event log data may contain internal application identifiers, username fragments, or file path details that should not be transmitted outside the network.",
                Tags = ["internet communication", "event viewer", "online links", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableEventViewer", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableEventViewer")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableEventViewer", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes 'More Information' online links from Event Viewer; local event logs are fully accessible.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-registration-wizard",
                Label = "Internet Communication: Disable Windows Registration Wizard",
                Category = "Network",
                Description =
                    "Disables the Windows Product Registration Wizard that appears after OS installation and prompts users to register the Windows license with Microsoft. The registration process transmits hardware information (CPU, RAM, disk size), Windows edition, and product key metadata to Microsoft's registration servers. On volume-licensed enterprise deployments, this registration is handled by Microsoft's volume activation infrastructure and the wizard is unnecessary.",
                Tags = ["internet communication", "registration", "product key", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableRegistration")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableRegistration", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables the Windows registration wizard; volume-licensed systems are unaffected — VL activation works separately.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-windows-activation-online",
                Label = "Internet Communication: Disable Windows Online Activation",
                Category = "Network",
                Description =
                    "Prevents Windows from attempting online activation via Microsoft's activation servers. In enterprise environments using KMS (Key Management Service) or MAK (Multiple Activation Key) volume licensing, Windows activates against the internal KMS server. Attempting simultaneous online activation can cause interference and may expose the KMS key or activation state to Microsoft's telemetry infrastructure.",
                Tags = ["internet communication", "activation", "kms", "volume license", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetRestrictKey],
                ApplyOps = [RegOp.SetDword(InetRestrictKey, "NoGenTicket", 1)],
                RemoveOps = [RegOp.DeleteValue(InetRestrictKey, "NoGenTicket")],
                DetectOps = [RegOp.CheckDword(InetRestrictKey, "NoGenTicket", 1)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Disables online activation; requires KMS or MAK infrastructure to be in place for Windows to remain activated.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-task-scheduler-download",
                Label = "Internet Communication: Disable Task Scheduler Internet Download",
                Category = "Network",
                Description =
                    "Prevents Windows Task Scheduler from downloading programs, scripts, or task definitions from internet URIs. Task Scheduler tasks can be configured with action items that fetch content from HTTP/HTTPS URLs. Blocking internet downloads from Task Scheduler prevents lateral movement via scheduled tasks that pull malware from C2 servers and prevents administrative tasks from overriding proxy policies by downloading directly.",
                Tags = ["internet communication", "task scheduler", "download", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableTaskSchedulerDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableTaskSchedulerDownload")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableTaskSchedulerDownload", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks internet downloads initiated by Task Scheduler actions; scheduled tasks with local scripts are unaffected.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-online-search-help",
                Label = "Internet Communication: Disable Windows Online Search in Help",
                Category = "Network",
                Description =
                    "Prevents the Windows Help and Support Center from augmenting local help content with online Microsoft documentation and search results. The online search feature sends the user's help query terms and partial system context to Microsoft's servers. In environments where query terms may contain project names, application names, or technical details classified as business-sensitive, disabling online help search prevents inadvertent disclosure.",
                Tags = ["internet communication", "help center", "online search", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableOnlineSearch", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableOnlineSearch")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableOnlineSearch", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Limits Help Center to local offline articles; online Microsoft documentation is not fetched.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-driver-update-internet",
                Label = "Internet Communication: Disable Driver Download from Windows Update",
                Category = "Network",
                Description =
                    "Prevents Windows from automatically downloading device driver updates directly from Windows Update when a new device is plugged in. Driver updates fetched directly from Windows Update bypass the organization's driver qualification and testing process. In managed environments, all driver updates should go through SCCM, Intune, or another MDM platform that can test and stage driver deployments before production rollout.",
                Tags = ["internet communication", "driver update", "windows update", "device management", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableDriverUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableDriverUpdate")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableDriverUpdate", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Stops Windows from fetching driver updates via Windows Update; driver management must be handled via MDM or manual deployment.",
            },
        ];
    }

    // ── IpsecRulePolicy ──
    private static class _IpsecRulePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PolicyAgent\Oakley";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPSec\LocalPolicyModule";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ipsecpol-disable-default-exemptions",
                    Label = "Disable IPSec Default Exemptions",
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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
                    Category = "Network",
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

    // ── Ipv6Policy ──
    private static class _Ipv6Policy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Tcpip\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ipv6pol-disable-ipv6",
                Label = "Disable IPv6 on All Network Adapters",
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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

    // ── LanmanServerPolicy ──
    private static class _LanmanServerPolicy
    {
        private const string SrvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lansrv-disable-auto-share-wks",
                    Label = "Disable Automatic Admin Shares (Workstation)",
                    Category = "Network",
                    Description = "Prevents Windows from automatically creating administrative shares (C$, D$) on workstations.",
                    Tags = ["smb", "shares", "lanman", "security", "admin-shares"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Removes hidden admin shares used for remote administration; may break management tools.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "AutoShareWks", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "AutoShareWks")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "AutoShareWks", 0)],
                },
                new TweakDef
                {
                    Id = "lansrv-disable-auto-share-server",
                    Label = "Disable Automatic Admin Shares (Server)",
                    Category = "Network",
                    Description = "Prevents Windows from automatically creating server-side default network shares (ADMIN$).",
                    Tags = ["smb", "shares", "lanman", "security", "admin-shares"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Removes server-side default shares; may impact domain management and remote admin tools.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "AutoShareServer", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "AutoShareServer")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "AutoShareServer", 0)],
                },
                new TweakDef
                {
                    Id = "lansrv-disable-plain-text-password",
                    Label = "Disable Plain-Text Password Authentication (Server)",
                    Category = "Network",
                    Description = "Prevents the SMB server from accepting unencrypted password authentication over the network.",
                    Tags = ["smb", "authentication", "lanman", "security", "password"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Blocks legacy clear-text SMB authentication; no impact on modern NTLMv2 or Kerberos clients.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "EnablePlainTextPassword", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "EnablePlainTextPassword")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "EnablePlainTextPassword", 0)],
                },
                new TweakDef
                {
                    Id = "lansrv-enable-security-signature",
                    Label = "Enable SMB Server Security Signatures",
                    Category = "Network",
                    Description = "Enables cryptographic packet signing for SMB server connections to detect in-transit tampering.",
                    Tags = ["smb", "signing", "lanman", "security", "integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enables SMB signing; small CPU overhead; highly recommended for all environments.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "EnableSecuritySignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "EnableSecuritySignature")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "EnableSecuritySignature", 1)],
                },
                new TweakDef
                {
                    Id = "lansrv-require-security-signature",
                    Label = "Require SMB Server Security Signature",
                    Category = "Network",
                    Description = "Mandates that all SMB connections to this server use packet signing; unsigned clients are rejected.",
                    Tags = ["smb", "signing", "lanman", "security", "integrity", "enforce"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Enforces SMB signing; may break very old clients (Windows XP era) that do not support signing.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "RequireSecuritySignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "RequireSecuritySignature")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "RequireSecuritySignature", 1)],
                },
                new TweakDef
                {
                    Id = "lansrv-enable-spn-validation",
                    Label = "Enable SMB Server SPN Validation",
                    Category = "Network",
                    Description = "Requires clients to provide a valid Service Principal Name when connecting, preventing relay attacks.",
                    Tags = ["smb", "spn", "kerberos", "lanman", "security", "relay"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Hardens against NTLM relay attacks; minimal impact in domain-joined environments.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "SmbServerNameHardeningLevel", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "SmbServerNameHardeningLevel")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "SmbServerNameHardeningLevel", 1)],
                },
                new TweakDef
                {
                    Id = "lansrv-restrict-null-session",
                    Label = "Restrict Null Session Access",
                    Category = "Network",
                    Description = "Blocks anonymous null-session connections from enumerating shares, users, and other resources.",
                    Tags = ["smb", "null-session", "anonymous", "lanman", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents anonymous enumeration; safe for domain environments that use authenticated access.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "RestrictNullSessAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "RestrictNullSessAccess")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "RestrictNullSessAccess", 1)],
                },
                new TweakDef
                {
                    Id = "lansrv-auto-disconnect-idle",
                    Label = "Auto-Disconnect Idle SMB Sessions",
                    Category = "Network",
                    Description = "Automatically disconnects idle SMB client sessions after 15 minutes to reduce resource exposure.",
                    Tags = ["smb", "session", "idle", "lanman", "resource"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Idle sessions disconnected after 15 minutes; transparent reconnect on next file access.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "AutoDisconnect", 15)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "AutoDisconnect")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "AutoDisconnect", 15)],
                },
                new TweakDef
                {
                    Id = "lansrv-disable-multicast",
                    Label = "Disable SMB WSD Multicast Discovery",
                    Category = "Network",
                    Description = "Disables WS-Discovery multicast traffic used by SMB to advertise network shares on the local subnet.",
                    Tags = ["smb", "multicast", "discovery", "lanman", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Stops SMB multicast probes; reduces network chatter; shares remain accessible via UNC path.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "EnableMulticast", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "EnableMulticast")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "EnableMulticast", 0)],
                },
                new TweakDef
                {
                    Id = "lansrv-audit-insecure-guest-logon",
                    Label = "Audit Insecure SMB Guest Logon Attempts",
                    Category = "Network",
                    Description = "Logs an event whenever a client attempts an anonymous or guest SMB logon that would be rejected.",
                    Tags = ["smb", "audit", "guest", "lanman", "security", "logging"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables security auditing for rejected guest logins; adds entries to the Security event log.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "AuditInsecureGuestLogon", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "AuditInsecureGuestLogon")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "AuditInsecureGuestLogon", 1)],
                },
            ];
    }

    // ── LanmanWorkstationPolicy ──
    private static class _LanmanWorkstationPolicy
    {
        private const string WksKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lanwks-block-insecure-guest-auth",
                    Label = "Block Insecure Guest Authentication",
                    Category = "Network",
                    Description = "Prevents the SMB client from falling back to insecure guest authentication when a server rejects credentials.",
                    Tags = ["smb", "guest", "authentication", "lanman", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Blocks unauthenticated SMB guest connections; may prevent access to old NAS using anonymous shares.",
                    ApplyOps = [RegOp.SetDword(WksKey, "AllowInsecureGuestAuth", 0)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "AllowInsecureGuestAuth")],
                    DetectOps = [RegOp.CheckDword(WksKey, "AllowInsecureGuestAuth", 0)],
                },
                new TweakDef
                {
                    Id = "lanwks-disable-plain-text-password",
                    Label = "Disable Plain-Text Password Authentication (Client)",
                    Category = "Network",
                    Description = "Stops the SMB workstation client from sending unencrypted passwords to network servers.",
                    Tags = ["smb", "authentication", "plaintext", "password", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Blocks clear-text SMB auth on the client side; no impact on NTLMv2 or Kerberos connections.",
                    ApplyOps = [RegOp.SetDword(WksKey, "EnablePlainTextPassword", 0)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "EnablePlainTextPassword")],
                    DetectOps = [RegOp.CheckDword(WksKey, "EnablePlainTextPassword", 0)],
                },
                new TweakDef
                {
                    Id = "lanwks-enable-security-signature",
                    Label = "Enable SMB Client Security Signatures",
                    Category = "Network",
                    Description = "Enables cryptographic SMB packet signing on the client for outbound connections when the server supports it.",
                    Tags = ["smb", "signing", "client", "lanman", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Adds packet integrity verification; slight CPU overhead; compatible with all modern servers.",
                    ApplyOps = [RegOp.SetDword(WksKey, "EnableSecuritySignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "EnableSecuritySignature")],
                    DetectOps = [RegOp.CheckDword(WksKey, "EnableSecuritySignature", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-require-security-signature",
                    Label = "Require SMB Client Security Signature",
                    Category = "Network",
                    Description = "Mandates that the SMB client use packet signing for all connections; unsigned servers are rejected.",
                    Tags = ["smb", "signing", "client", "lanman", "security", "enforce"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Enforces signing on all SMB connections; breaks access to servers that do not support signing.",
                    ApplyOps = [RegOp.SetDword(WksKey, "RequireSecuritySignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "RequireSecuritySignature")],
                    DetectOps = [RegOp.CheckDword(WksKey, "RequireSecuritySignature", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-enable-smb-encryption",
                    Label = "Enable SMB Client Encryption",
                    Category = "Network",
                    Description = "Requests encrypted SMB connections wherever the server supports SMB 3.x encryption.",
                    Tags = ["smb", "encryption", "client", "lanman", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "End-to-end encryption for SMB traffic; requires SMB 3.x on both sides; silently ignored by older servers.",
                    ApplyOps = [RegOp.SetDword(WksKey, "EnableSMBEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "EnableSMBEncryption")],
                    DetectOps = [RegOp.CheckDword(WksKey, "EnableSMBEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-disable-smb1",
                    Label = "Disable SMBv1 Client Protocol",
                    Category = "Network",
                    Description = "Disables the legacy SMBv1 dialect on the workstation client to prevent WannaCry-class exploits.",
                    Tags = ["smb", "smb1", "client", "lanman", "security", "legacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Eliminates SMBv1 support; may break access to Windows XP/2003 or old NAS that only support SMBv1.",
                    ApplyOps = [RegOp.SetDword(WksKey, "DisableSMB1", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "DisableSMB1")],
                    DetectOps = [RegOp.CheckDword(WksKey, "DisableSMB1", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-require-ntlmv2",
                    Label = "Require NTLMv2 Authentication (Client)",
                    Category = "Network",
                    Description = "Prevents the SMB workstation client from using the weak NTLMv1 authentication protocol.",
                    Tags = ["smb", "ntlm", "ntlmv1", "client", "authentication", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Forces NTLMv2 or Kerberos; may affect connections to very old servers that only support NTLMv1.",
                    ApplyOps = [RegOp.SetDword(WksKey, "RequireNTLMv2", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "RequireNTLMv2")],
                    DetectOps = [RegOp.CheckDword(WksKey, "RequireNTLMv2", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-enable-logon-audit",
                    Label = "Enable SMB Workstation Logon Audit",
                    Category = "Network",
                    Description = "Records authentication events for SMB workstation connections in the Security event log.",
                    Tags = ["smb", "audit", "logon", "client", "lanman", "logging"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Adds logon audit entries to the event log; useful for detecting lateral movement attempts.",
                    ApplyOps = [RegOp.SetDword(WksKey, "EnableLogonAuditing", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "EnableLogonAuditing")],
                    DetectOps = [RegOp.CheckDword(WksKey, "EnableLogonAuditing", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-disable-no-inplace-sharing",
                    Label = "Disable In-Place Sharing on Removable Media",
                    Category = "Network",
                    Description = "Prevents in-place file sharing on removable storage media accessed through SMB workstation connections.",
                    Tags = ["smb", "removable", "sharing", "client", "lanman"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks in-place sharing from removable drives; users must copy files to a local path first.",
                    ApplyOps = [RegOp.SetDword(WksKey, "NoInplaceSharingOnRemovableMedia", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "NoInplaceSharingOnRemovableMedia")],
                    DetectOps = [RegOp.CheckDword(WksKey, "NoInplaceSharingOnRemovableMedia", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-disable-multicast-name-resolution",
                    Label = "Disable SMB Multicast Name Resolution",
                    Category = "Network",
                    Description = "Stops the SMB client from using LLMNR and NetBIOS multicast name resolution, reducing lateral movement risk.",
                    Tags = ["smb", "llmnr", "netbios", "multicast", "client", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks LLMNR/NetBIOS name poisoning attacks; may slow discovery of shares without DNS entries.",
                    ApplyOps = [RegOp.SetDword(WksKey, "DisableMulticastNameResolution", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "DisableMulticastNameResolution")],
                    DetectOps = [RegOp.CheckDword(WksKey, "DisableMulticastNameResolution", 1)],
                },
            ];
    }

    // ── LdapClientPolicy ──
    private static class _LdapClientPolicy
    {
        private const string LdapPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LDAP";

        private const string LdapSvcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ldap";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ldapclnt-require-ldap-signing",
                    Label = "LDAP Client: Require LDAP Signing (Negotiate/Require)",
                    Category = "Network",
                    Description =
                        "Sets LDAPClientIntegrity=2 in the LDAP policy hive (value 2 = Require signing; value 1 = Negotiate signing; value 0 = None). Requires LDAP clients to request LDAP packet signing on all LDAP connections to domain controllers. Without LDAP signing, the LDAP exchange is susceptible to LDAP relay attacks — an attacker who can perform a man-in-the-middle attack can modify LDAP query results without detection. LDAP signing is part of the security hardening recommended by Microsoft Security Advisory ADV190023 (LDAP channel binding and LDAP signing). Combined with LDAP channel binding, this closes a class of LDAP relay and NTLM relay attacks.",
                    Tags = ["ldap", "signing", "integrity", "adv190023", "relay"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "LDAP clients require signing for all DC connections. Legacy LDAP applications that use anonymous LDAP binds or simple (plaintext) binds without signing will fail. Audit LDAP usage with DC diagnostic logging (Set 16 LDAP Interface Events to 2) before enforcing. Older UNIX/Linux LDAP clients may need PAM/NSS configuration updates.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LDAPClientIntegrity", 2)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LDAPClientIntegrity")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LDAPClientIntegrity", 2)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-require-ldap-channel-binding",
                    Label = "LDAP Client: Require LDAP Channel Binding Token (CBT)",
                    Category = "Network",
                    Description =
                        "Sets LdapEnforceChannelBinding=2 in the LDAP policy hive (value 2 = Always require channel binding; value 1 = Supported; value 0 = Never). LDAP Channel Binding Tokens (CBT) cryptographically bind an LDAP authentication exchange to the specific TLS channel it is using. This prevents LDAP relay attacks where an attacker intercepts an NTLM authentication for an LDAP-over-TLS connection and replays it over a different TLS connection (cross-channel relay). Combined with LDAP signing enforcement, this closes the NTLM relay to LDAP attack vector used by tools like Responder and ntlmrelayx.",
                    Tags = ["ldap", "channel-binding", "cbt", "ntlm-relay", "tls"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "LDAP channel binding enforced on all connections. Applications that use LDAP-over-TLS (LDAPS on port 636) with NTLM authentication must support channel binding tokens. Older LDAP client libraries (OpenLDAP < 2.5, Python-ldap without CBT patches) will fail LDAPS authentication. Survey LDAP client versions before enforcing.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LdapEnforceChannelBinding", 2)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LdapEnforceChannelBinding")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LdapEnforceChannelBinding", 2)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-set-ldap-client-timeout-120s",
                    Label = "LDAP Client: Set LDAP Search and Connection Timeout to 120 Seconds",
                    Category = "Network",
                    Description =
                        "Sets LdapClientTimeout=120 in the LDAP policy hive (units: seconds). Sets the maximum number of seconds an LDAP client will wait for a search result before terminating the operation. Without a timeout, an LDAP client that connects to a slow or unresponsive domain controller can hold open connections indefinitely — an attacker who controls a fake DC can exploit this by responding very slowly to keep the LDAP connection open and drain client resources. Setting a bounded timeout ensures that LDAP operations fail gracefully when the DC is unresponsive, and the client can fall back to another DC.",
                    Tags = ["ldap", "timeout", "connection-limit", "dc-failover", "dos-mitigation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "LDAP search and connection timeout is 120 seconds. Operations that require longer LDAP searches (large group enumeration, deep OU subtree searches) may time out in environments with extremely large directories. Monitor LDAP timeout events in application logs on clients running LDAP-intensive applications.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LdapClientTimeout", 120)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LdapClientTimeout")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LdapClientTimeout", 120)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-disable-ldap-anonymous-bind",
                    Label = "LDAP Client: Disable Unauthenticated (Anonymous) LDAP Bind",
                    Category = "Network",
                    Description =
                        "Sets DisableAnonymousBind=1 in the LDAP policy hive. Prevents LDAP clients from performing anonymous (unauthenticated) LDAP binds to Active Directory domain controllers. Anonymous LDAP binds historically allowed any system on the network to query AD for directory information (user accounts, group memberships, computer accounts) without authenticating. While Windows Server 2003 and later restrict anonymous LDAP read access by default at the DC level, client-side enforcement ensures that applications in the environment never attempt anonymous LDAP binds — a pattern that could succeed against non-standard LDAP servers or legacy DCs with weakened configuration.",
                    Tags = ["ldap", "anonymous-bind", "authentication", "directory-enumeration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Anonymous LDAP binds are blocked at the client level. Applications that use anonymous LDAP to query directory info without credentials will fail. Older monitoring tools and health check scripts that rely on anonymous LDAP must be updated to use service account credentials.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "DisableAnonymousBind", 1)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "DisableAnonymousBind")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "DisableAnonymousBind", 1)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-enforce-ldaps-port-636",
                    Label = "LDAP Client: Enforce LDAP over TLS (LDAPS) on Port 636",
                    Category = "Network",
                    Description =
                        "Sets UseLdapSsl=1 in the LDAP policy hive. Enforces the use of LDAP over TLS (LDAPS on port 636) for LDAP connections. Standard LDAP on port 389 transmits all directory queries and responses, including credentials, in plaintext. An attacker performing network traffic capture on the corporate network can extract LDAP bind credentials, observe group memberships, and construct detailed maps of Active Directory structure. LDAPS encrypts the entire LDAP session using TLS. Combined with LDAP signing and channel binding, LDAPS provides end-to-end protection for directory communications.",
                    Tags = ["ldap", "ldaps", "tls", "port-636", "plaintext-prevention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "LDAP connections use LDAPS (port 636) with TLS. Domain controllers must have valid LDAP server certificates installed. Certificate authority chain must be trusted by all LDAP clients. LDAP port 389 connections from this client will be rejected by LDAPS-only DCs. Ensure DCs have DC certificates from an internal PKI before enforcing.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "UseLdapSsl", 1)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "UseLdapSsl")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "UseLdapSsl", 1)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-set-max-ldap-connections-500",
                    Label = "LDAP Client: Cap Maximum Concurrent LDAP Connections to 500",
                    Category = "Network",
                    Description =
                        "Sets MaxConnections=500 in the LDAP service key. Limits the number of concurrent LDAP connections the client interface will maintain to 500. Unbounded LDAP connections on an LDAP client can lead to memory exhaustion if an application has a connection leak or if a malicious application attempts to open large numbers of LDAP connections to degrade other directory services consumers on the same host. This setting provides a reasonable upper bound for normal enterprise usage while preventing connection floods.",
                    Tags = ["ldap", "connection-limit", "resource-bound", "dos-mitigation", "memory"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Maximum of 500 concurrent LDAP connections. In typical enterprise environments the actual number of concurrent LDAP connections is under 20. Applications that open many parallel LDAP connection contexts (directory synchronisation tools, identity management systems) should be tested to confirm they stay under 500.",
                    ApplyOps = [RegOp.SetDword(LdapSvcKey, "MaxConnections", 500)],
                    RemoveOps = [RegOp.DeleteValue(LdapSvcKey, "MaxConnections")],
                    DetectOps = [RegOp.CheckDword(LdapSvcKey, "MaxConnections", 500)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-enable-referral-following-sasl",
                    Label = "LDAP Client: Require SASL Authentication When Following LDAP Referrals",
                    Category = "Network",
                    Description =
                        "Sets FollowReferralsWithSasl=1 in the LDAP policy hive. Requires that when an LDAP client follows an LDAP referral (a response from one DC that redirects the client to query a different DC or domain), the subsequent connection to the referred server uses SASL (GSSAPI/Kerberos) authentication rather than simple bind. An attacker who can serve a crafted LDAP referral can attempt to redirect the client to a malicious LDAP server — if the client then connects using simple bind (password), the credentials can be captured. SASL with Kerberos prevents this: the referral target must prove its identity via Kerberos before credentials are presented.",
                    Tags = ["ldap", "referral", "sasl", "kerberos", "credential-capture"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SASL required when following LDAP referrals. Applications that follow referrals using simple bind must switch to SASL/Kerberos for the referred connection. Modern .NET LDAP libraries and Windows LDAP APIs handle this transparently. Custom LDAP implementations may require code changes.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "FollowReferralsWithSasl", 1)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "FollowReferralsWithSasl")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "FollowReferralsWithSasl", 1)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-enable-ldap-diagnostic-logging",
                    Label = "LDAP Client: Enable LDAP Client Diagnostic Event Logging",
                    Category = "Network",
                    Description =
                        "Sets LdapDiagnosticsEnabled=1 in the LDAP policy hive. Enables LDAP client diagnostic logging to the Windows Application event log. When enabled, LDAP authentication failures, TLS handshake errors, channel binding failures, and referral-following events are logged with details including the DC hostname, error code, and the identity attempting authentication. This logging is essential for detecting LDAP attacks (repeated anonymous bind attempts, LDAP relay attempts where channel binding fails) and for diagnosing LDAP signing/channel binding compatibility issues during enforcement rollout.",
                    Tags = ["ldap", "diagnostics", "logging", "event-log", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "LDAP diagnostic events are logged to the Application event log. Generates event log volume proportional to the number of LDAP operations. In environments with high LDAP query rates, review the log volume impact. Events appear under source 'LDAP Client'. Integrate with SIEM for attack detection use cases.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LdapDiagnosticsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LdapDiagnosticsEnabled")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LdapDiagnosticsEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-block-ntlm-ldap-fallback",
                    Label = "LDAP Client: Block NTLM Fallback in LDAP Authentication Negotiation",
                    Category = "Network",
                    Description =
                        "Sets BlockNtlmLdapFallback=1 in the LDAP policy hive. Prevents the LDAP client from falling back to NTLM authentication when Kerberos authentication to a domain controller fails. An attacker who performs a downgrade attack (e.g., interfering with Kerberos SPN resolution) can force an LDAP client to use NTLM instead of Kerberos for authentication. NTLM is weaker and susceptible to relay attacks. Blocking NTLM fallback forces the client to fail visibly when Kerberos is unavailable rather than silently using the weaker protocol — making downgrade attacks immediately visible in logs.",
                    Tags = ["ldap", "ntlm-fallback", "kerberos", "downgrade", "relay"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "NTLM fallback for LDAP is blocked. When Kerberos authentication to a DC fails (e.g., SPN resolution failure, KDC unreachable), the LDAP operation fails rather than retrying with NTLM. This may cause authentication failures during DC failover events. Monitor LDAP authentication failures in event logs after enforcing.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "BlockNtlmLdapFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "BlockNtlmLdapFallback")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "BlockNtlmLdapFallback", 1)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-enforce-ldap-query-size-limit-1000",
                    Label = "LDAP Client: Enforce Maximum LDAP Query Result Size of 1000 Objects",
                    Category = "Network",
                    Description =
                        "Sets MaxPageSize=1000 in the LDAP policy hive. Limits LDAP query results to a maximum of 1000 directory objects per page. Unbounded LDAP queries (queries without a size limit) can return tens of thousands of objects, consuming excessive DC memory and CPU, and enabling bulk directory enumeration by an attacker who has obtained LDAP query access. Setting a page size limit of 1000 ensures that applications must use paged results (LDAP paging control) to enumerate large sets of objects — and an attacker attempting to dump the entire directory in one query receives an error and must iterate, increasing the attack duration and detectability.",
                    Tags = ["ldap", "query-size", "paging", "enumeration", "dos-mitigation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "LDAP query result size limited to 1000 objects per response. Applications that depend on unbounded LDAP queries (returning >1000 objects in one response) must be updated to use LDAP paged results control (LDAP_CONTROL_PAGEDRESULTS). Most modern LDAP libraries support paged results automatically.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "MaxPageSize", 1000)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "MaxPageSize")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "MaxPageSize", 1000)],
                },
            ];
    }

    // ── LdapSigningPolicy ──
    private static class _LdapSigningPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ldap";
        private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LDAP";
        private const string DcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NTDS\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ldapsec-require-client-signing",
                    Label = "Require LDAP Client Signing for All Directory Connections",
                    Category = "Network",
                    Description =
                        "Configures the LDAP client to always request LDAP signing (integrity protection), preventing man-in-the-middle attacks against LDAP sessions that could be used to modify query results or inject forged LDAP responses.",
                    Tags = ["ldap", "signing", "integrity", "mitm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "LDAP client signing required; LDAP MITM response injection attacks mitigated.",
                    ApplyOps = [RegOp.SetDword(Key, "LDAPClientIntegrity", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LDAPClientIntegrity")],
                    DetectOps = [RegOp.CheckDword(Key, "LDAPClientIntegrity", 2)],
                },
                new TweakDef
                {
                    Id = "ldapsec-require-channel-binding",
                    Label = "Require LDAP Channel Binding Tokens (CBT Hardening)",
                    Category = "Network",
                    Description =
                        "Configures the LDAP client to include EPA Channel Binding Tokens in all LDAP over TLS sessions, preventing NTLM relay attacks that forward LDAP authentication to a different TLS channel.",
                    Tags = ["ldap", "channel-binding", "cbt", "ntlm-relay", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "LDAP channel binding required; NTLM relay attacks forwarding LDAP auth to different TLS channel blocked.",
                    ApplyOps = [RegOp.SetDword(PolKey, "LdapClientChannelBinding", 2)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "LdapClientChannelBinding")],
                    DetectOps = [RegOp.CheckDword(PolKey, "LdapClientChannelBinding", 2)],
                },
                new TweakDef
                {
                    Id = "ldapsec-disable-simple-bind",
                    Label = "Disable LDAP Simple Bind Authentication",
                    Category = "Network",
                    Description =
                        "Prevents the use of LDAP Simple Bind authentication which sends credentials as plaintext Base64 without integrity protection. NTLM or Kerberos SASL must be used for all LDAP authentication.",
                    Tags = ["ldap", "simple-bind", "plaintext-auth", "sasl", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "LDAP Simple Bind disabled; plaintext credential authentication to LDAP blocked. SASL required.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableSimpleBind", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableSimpleBind")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableSimpleBind", 1)],
                },
                new TweakDef
                {
                    Id = "ldapsec-require-ldaps-port636",
                    Label = "Require LDAP over SSL/TLS (LDAPS, Port 636) for All AD Connections",
                    Category = "Network",
                    Description =
                        "Configures the LDAP client to use LDAPS (LDAP over TLS on port 636) for all Active Directory connections, ensuring the entire LDAP session including SASL auth handshake is TLS-encrypted.",
                    Tags = ["ldap", "ldaps", "tls", "port-636", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "LDAPS required; all AD directory queries and authentications use TLS encryption on port 636.",
                    ApplyOps = [RegOp.SetDword(PolKey, "RequireLDAPS", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "RequireLDAPS")],
                    DetectOps = [RegOp.CheckDword(PolKey, "RequireLDAPS", 1)],
                },
                new TweakDef
                {
                    Id = "ldapsec-set-max-query-result-size",
                    Label = "Set Maximum LDAP Query Result Set to 1000 Entries",
                    Category = "Network",
                    Description =
                        "Limits LDAP query result sets to 1000 entries, preventing oversized LDAP result enumeration attacks that could be used to enumerate all AD objects in a single query exceeding normal LDAP paged result limits.",
                    Tags = ["ldap", "result-size", "enumeration-prevention", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LDAP result set limited to 1000 entries; full AD object enumeration in single query prevented.",
                    ApplyOps = [RegOp.SetDword(DcKey, "MaxPageSize", 1000)],
                    RemoveOps = [RegOp.DeleteValue(DcKey, "MaxPageSize")],
                    DetectOps = [RegOp.CheckDword(DcKey, "MaxPageSize", 1000)],
                },
                new TweakDef
                {
                    Id = "ldapsec-set-query-timeout-30s",
                    Label = "Set LDAP Query Timeout to 30 Seconds to Prevent Slow Queries",
                    Category = "Network",
                    Description =
                        "Sets the LDAP client query timeout to 30 seconds, ensuring that slow/hanging LDAP queries do not block authentication processes and preventing DoS via crafted slow LDAP response attacks.",
                    Tags = ["ldap", "query-timeout", "dos-prevention", "authentication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LDAP query timeout set to 30 seconds; hanging LDAP queries do not block auth. Slow-response DoS mitigated.",
                    ApplyOps = [RegOp.SetDword(Key, "TimeLimit", 30)],
                    RemoveOps = [RegOp.DeleteValue(Key, "TimeLimit")],
                    DetectOps = [RegOp.CheckDword(Key, "TimeLimit", 30)],
                },
                new TweakDef
                {
                    Id = "ldapsec-disable-ldap-null-base-queries",
                    Label = "Disable Unauthenticated LDAP Null-Base DNS Queries",
                    Category = "Network",
                    Description =
                        "Prevents anonymous LDAP queries with a null base (empty search base DN) that enable unauthenticated discovery of AD domain information, domain naming context, and supported SASL mechanisms.",
                    Tags = ["ldap", "null-base", "anonymous", "enumeration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "LDAP anonymous null-base queries blocked; unauthenticated AD domain enumeration prevented.",
                    ApplyOps = [RegOp.SetDword(DcKey, "DisableNullBaseQuery", 1)],
                    RemoveOps = [RegOp.DeleteValue(DcKey, "DisableNullBaseQuery")],
                    DetectOps = [RegOp.CheckDword(DcKey, "DisableNullBaseQuery", 1)],
                },
                new TweakDef
                {
                    Id = "ldapsec-log-signing-failures",
                    Label = "Log LDAP Signing and Channel Binding Failure Events",
                    Category = "Network",
                    Description =
                        "Enables Security audit log entries for LDAP sessions that fail signing or channel binding requirements, providing visibility into tools and applications sending unsigned LDAP queries.",
                    Tags = ["ldap", "signing-failure", "event-log", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LDAP signing/channel-binding failures logged; applications sending unsigned LDAP visible for remediation.",
                    ApplyOps = [RegOp.SetDword(PolKey, "LogSigningFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "LogSigningFailures")],
                    DetectOps = [RegOp.CheckDword(PolKey, "LogSigningFailures", 1)],
                },
                new TweakDef
                {
                    Id = "ldapsec-disable-ldap-telemetry",
                    Label = "Disable LDAP Client Telemetry to Microsoft",
                    Category = "Network",
                    Description =
                        "Prevents the Windows LDAP client from sending signing negotiation stats, connection failure rates, and cipher suite telemetry to Microsoft.",
                    Tags = ["ldap", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LDAP telemetry to Microsoft disabled; connection stats and cipher negotiation data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "ldapsec-enable-integrity-check-on-reconnect",
                    Label = "Re-Verify LDAP Integrity on Session Reconnection",
                    Category = "Network",
                    Description =
                        "Forces the LDAP client to re-negotiate and verify signing integrity tokens when an LDAP session is reconnected after a network interruption, preventing session hijacking via injection into a reconnected unsigned LDAP stream.",
                    Tags = ["ldap", "reconnect", "integrity", "session-hijacking", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "LDAP integrity re-verified on reconnect; injecting bytes into reconnected sessions blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "VerifyServerCertificate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VerifyServerCertificate")],
                    DetectOps = [RegOp.CheckDword(Key, "VerifyServerCertificate", 1)],
                },
            ];
    }

    // ── LegacyProtocols ──
    private static class _LegacyProtocols
    {
        private const string DnsClient = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";

        private const string DnsClientSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters";

        private const string NetBtParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters";

        private const string LltdSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lltdsvc";

        private const string TeledoSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters";

        private const string IpHlpSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\iphlpsvc\Parameters\Teredo";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "legprot-disable-lltd",
                Label = "Disable LLTD (Link-Layer Topology Discovery)",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["lltd", "network discovery", "topology", "legacy"],
                Description =
                    "Disables the Link-Layer Topology Discovery (LLTD) service — used to build "
                    + "the Network Map in Windows Vista/7. Rarely needed on modern networks. "
                    + "Setting Start=4 disables the lltdsvc service.",
                ApplyOps = [RegOp.SetDword(LltdSvc, "Start", 4)],
                RemoveOps = [RegOp.SetDword(LltdSvc, "Start", 3)],
                DetectOps = [RegOp.CheckDword(LltdSvc, "Start", 4)],
            },
            new TweakDef
            {
                Id = "legprot-disable-6to4",
                Label = "Disable IPv6-to-IPv4 Transition (6to4)",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["6to4", "ipv6", "transition", "legacy protocol"],
                Description =
                    "Disables the 6to4 service — an IPv6 transition technology that wraps IPv6 "
                    + "packets in IPv4. Rarely used in modern networks and creates a potential "
                    + "covert channel. DisabledComponents+=2 in TCP/IP v6 parameters.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\6to4", "Start", 4)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\6to4", "Start", 3)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\6to4", "Start", 4)],
            },
            new TweakDef
            {
                Id = "legprot-disable-wins-client",
                Label = "Disable WINS Client Lookup",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["wins", "netbios", "name resolution", "legacy"],
                Description =
                    "Disables WINS (Windows Internet Naming Service) client lookups. "
                    + "EnableWins=0 in NetBT parameters. WINS is a legacy NetBIOS name resolution "
                    + "service superseded by DNS. Disabling it on non-legacy environments "
                    + "reduces name-resolution attack surface.",
                ApplyOps = [RegOp.SetDword(NetBtParams, "EnableWins", 0)],
                RemoveOps = [RegOp.DeleteValue(NetBtParams, "EnableWins")],
                DetectOps = [RegOp.CheckDword(NetBtParams, "EnableWins", 0)],
            },
            new TweakDef
            {
                Id = "legprot-disable-llmnr-fallback",
                Label = "Disable LLMNR Name Resolution Fallback",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["llmnr", "dns fallback", "name resolution", "responder", "security"],
                Description =
                    "Prevents the DNS Client from falling back to LLMNR when DNS resolution fails. "
                    + "QueryAdapterName=0 in Dnscache parameters stops adapter-specific LLMNR "
                    + "queries, closing a secondary Responder attack vector beyond the primary "
                    + "legprot-disable-llmnr policy setting.",
                ApplyOps = [RegOp.SetDword(DnsClientSvc, "QueryAdapterName", 0)],
                RemoveOps = [RegOp.DeleteValue(DnsClientSvc, "QueryAdapterName")],
                DetectOps = [RegOp.CheckDword(DnsClientSvc, "QueryAdapterName", 0)],
            },
        ];
    }

    // ── LltdProtocolPolicy ──
    private static class _LltdProtocolPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LLTD";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lltdpol-disable-lltd-io",
                    Label = "Disable LLTD I/O (Network Map Responder on Private Networks)",
                    Category = "Network",
                    Description =
                        "Disables the LLTD I/O component that allows this machine to respond to Link Layer Topology Discovery queries on private networks (home/work), preventing its network adapters from appearing in the Windows Network Map.",
                    Tags = ["lltd", "network-map", "topology-discovery", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LLTD I/O on private networks disabled; machine removed from Windows Network Map on private networks.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableLLTDIO", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableLLTDIO")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableLLTDIO", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-lltd-io-domain",
                    Label = "Disable LLTD I/O on Domain Networks",
                    Category = "Network",
                    Description =
                        "Disables the LLTD I/O component on domain-authenticated networks, preventing this machine from exposing its network topology to network discovery tools on corporate domain networks.",
                    Tags = ["lltd", "domain-network", "topology-discovery", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LLTD I/O on domain networks disabled; machine does not respond to topology probes on domain networks.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowLLTDIOOnDomain", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowLLTDIOOnDomain")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowLLTDIOOnDomain", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-lltd-io-public",
                    Label = "Disable LLTD I/O on Public Networks",
                    Category = "Network",
                    Description =
                        "Disables the LLTD I/O component on public networks (airports, hotels, coffee shops), preventing network enumeration of this machine by other devices on untrusted public Wi-Fi networks.",
                    Tags = ["lltd", "public-network", "topology-discovery", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "LLTD I/O on public networks disabled; machine not discoverable on hotel/airport/coffee-shop Wi-Fi.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowLLTDIOOnPublicNet", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowLLTDIOOnPublicNet")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowLLTDIOOnPublicNet", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-rspndr",
                    Label = "Disable LLTD Responder Component on Private Networks",
                    Category = "Network",
                    Description =
                        "Disables the LLTD Responder driver (rspndr) on private networks, preventing this machine from sending LLTD discovery responses that reveal its presence and IP/MAC mapping to network topology collectors.",
                    Tags = ["lltd", "responder", "network-discovery", "mac-address", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LLTD Responder disabled on private networks; MAC address and IP not revealed via discovery responses.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableRspndr", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableRspndr")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableRspndr", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-rspndr-domain",
                    Label = "Disable LLTD Responder on Domain Networks",
                    Category = "Network",
                    Description =
                        "Disables the LLTD Responder driver on domain-authenticated networks, preventing topology discovery responses on corporate LANs where network mapping is managed exclusively by centralised network tools.",
                    Tags = ["lltd", "responder", "domain-network", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LLTD Responder disabled on domain networks; machine not visible in Windows Network Map on domain.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowRspndrOnDomain", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowRspndrOnDomain")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowRspndrOnDomain", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-rspndr-public",
                    Label = "Disable LLTD Responder on Public Networks",
                    Category = "Network",
                    Description =
                        "Disables the LLTD Responder driver on public networks, ensuring that this machine does not expose its presence, MAC address, or IP mapping to other hosts on untrusted public network segments.",
                    Tags = ["lltd", "responder", "public-network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "LLTD Responder disabled on public networks; presence not exposed on untrusted public Wi-Fi.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowRspndrOnPublicNet", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowRspndrOnPublicNet")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowRspndrOnPublicNet", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-log-lltd-probe-events",
                    Label = "Enable Logging of LLTD Discovery Probe Events",
                    Category = "Network",
                    Description =
                        "Enables event log entries when LLTD discovery probes are received, providing audit trail of which hosts on the network are conducting topology discovery scans against this machine.",
                    Tags = ["lltd", "audit", "discovery-probe", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LLTD probe receipt events logged; topology scanning activity against this machine auditable.",
                    ApplyOps = [RegOp.SetDword(Key, "LogLLTDProbeEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogLLTDProbeEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "LogLLTDProbeEvents", 1)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-managed-network-qos",
                    Label = "Disable LLTD Managed Network QoS Signalling",
                    Category = "Network",
                    Description =
                        "Disables the LLTD managed network Quality of Service signalling extension, preventing this machine from participating in QoS scheduling signals broadcast over LLTD on Windows home network environments.",
                    Tags = ["lltd", "qos", "network-management", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LLTD QoS signalling disabled; machine does not participate in LLTD-based bandwidth scheduling.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLLTDQoSSignaling", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLLTDQoSSignaling")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLLTDQoSSignaling", 1)],
                },
                new TweakDef
                {
                    Id = "lltdpol-block-lltd-service-admin-change",
                    Label = "Block Admin From Re-Enabling LLTD Without Policy Override",
                    Category = "Network",
                    Description =
                        "Prevents local administrators from re-enabling LLTD I/O or Responder components without a Group Policy override, ensuring that the LLTD disable policy cannot be circumvented at the local machine level.",
                    Tags = ["lltd", "admin-lockdown", "gpo", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LLTD cannot be re-enabled locally without GPO change; admin cannot override the detection disable.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockLocalLLTDOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockLocalLLTDOverride")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockLocalLLTDOverride", 1)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-lltd-multicast",
                    Label = "Disable LLTD Multicast Discovery on All Segments",
                    Category = "Network",
                    Description =
                        "Disables LLTD multicast discovery messages sent across all network segments, preventing bandwidth consumption from periodic LLTD multicast discovery packets on busy enterprise networks.",
                    Tags = ["lltd", "multicast", "bandwidth", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LLTD multicast discovery disabled; no periodic LLTD multicast traffic generated on any segment.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLLTDMulticast", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLLTDMulticast")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLLTDMulticast", 1)],
                },
            ];
    }

    // ── MapsBrowserPolicy ──
    private static class _MapsBrowserPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\MapsBrowser";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "mapsbr-disable-auto-download",
                    Label = "Disable Automatic Offline Maps Download",
                    Category = "Network",
                    Description =
                        "Prevents Windows from automatically downloading offline map data updates in the background. Reduces unnecessary network traffic on metered connections and removes a low-value background data transfer. Default: auto-download enabled. Recommended: 1.",
                    Tags = ["maps", "offline", "download", "background", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Offline map data is not downloaded automatically; maps remain available but may be outdated.",
                    ApplyOps = [RegOp.SetDword(Key, "AutoDownloadAndUpdateMapData", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutoDownloadAndUpdateMapData")],
                    DetectOps = [RegOp.CheckDword(Key, "AutoDownloadAndUpdateMapData", 0)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-untriggered-network-traffic",
                    Label = "Disable Maps App Untriggered Network Traffic",
                    Category = "Network",
                    Description =
                        "Stops the Maps application from initiating network requests that are not triggered by explicit user interaction (such as background tile prefetching or POI data sync). Reduces bandwidth consumption and privacy exposure. Default: background network traffic allowed. Recommended: 1.",
                    Tags = ["maps", "network", "background", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Maps app stops all unsolicited background network requests; only user-initiated map loads use network.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowUntriggeredNetworkTrafficOnSettingsPage", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowUntriggeredNetworkTrafficOnSettingsPage")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowUntriggeredNetworkTrafficOnSettingsPage", 0)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-location-for-maps",
                    Label = "Disable Location Access for Windows Maps",
                    Category = "Network",
                    Description =
                        "Blocks the Windows Maps application from using the device's current location (GPS, Wi-Fi triangulation, IP geolocation) to centre the map or suggest nearby places. Prevents continuous location sampling by the app. Default: location allowed. Recommended: 1 on privacy-hardened endpoints.",
                    Tags = ["maps", "location", "gps", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Windows Maps cannot access device location; map starts at a default location, not the user's current position.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLocationForMaps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationForMaps")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLocationForMaps", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-block-map-traffic-data",
                    Label = "Disable Real-Time Traffic Data in Maps",
                    Category = "Network",
                    Description =
                        "Prevents Windows Maps from fetching real-time traffic data (congestion, incidents, road closures) from Microsoft's mapping service. Reduces background network calls and location telemetry inferences. Default: traffic data enabled. Recommended: 1 on privacy-hardened endpoints.",
                    Tags = ["maps", "traffic", "realtime", "network", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Maps app does not show live traffic data; routes are calculated without congestion awareness.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTrafficData", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTrafficData")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTrafficData", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-map-tile-storage",
                    Label = "Disable Offline Map Tile Storage",
                    Category = "Network",
                    Description =
                        "Prevents Windows Maps from caching map tiles on local disk for offline use. Removes the map data footprint from managed devices where the maps feature is not used. Default: tiles cached locally. Recommended: 1 on space-constrained or managed endpoints where maps is unused.",
                    Tags = ["maps", "tile", "cache", "storage", "disk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Map tile cache is not maintained on disk; offline map access is unavailable.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableOfflineTileStorage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableOfflineTileStorage")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableOfflineTileStorage", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-bing-search-integration",
                    Label = "Disable Bing Search Integration in Maps",
                    Category = "Network",
                    Description =
                        "Prevents Windows Maps from sending search queries to Bing when a user searches for a place, address, or business. Stops search terms from being transmitted to Microsoft's servers. Default: Bing integration enabled. Recommended: 1 on privacy-hardened endpoints.",
                    Tags = ["maps", "bing", "search", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Map searches do not query Bing; only locally cached/offline map data is searched.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableBingSearchIntegration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableBingSearchIntegration")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableBingSearchIntegration", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-route-sharing",
                    Label = "Disable Route/Directions Sharing from Maps",
                    Category = "Network",
                    Description =
                        "Removes the 'Share' button and functionality from Windows Maps so users cannot share routes, locations, or directions via mail, SMS, or other apps. Prevents incidental location data leakage through sharing. Default: sharing enabled. Recommended: 1.",
                    Tags = ["maps", "sharing", "route", "privacy", "dlp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Share functionality is removed from Maps; routes and places cannot be shared externally.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRouteSharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRouteSharing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRouteSharing", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-personalized-maps",
                    Label = "Disable Personalised Map Suggestions",
                    Category = "Network",
                    Description =
                        "Disables personalised place recommendations and 'frequent locations' features in Windows Maps that are based on past search history and route patterns. Prevents the accumulation of a location history profile. Default: personalisation enabled. Recommended: 1.",
                    Tags = ["maps", "personalisation", "history", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Maps does not build or use a personal history; no frequent-place suggestions or route preferences are stored.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePersonalisedMaps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalisedMaps")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePersonalisedMaps", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-indoor-maps",
                    Label = "Disable Indoor Maps Feature",
                    Category = "Network",
                    Description =
                        "Turns off the indoor floor-plan mapping feature in Windows Maps. Indoor maps require additional tile downloads and location data for floor-level positioning. On managed endpoints the feature is rarely needed and adds unnecessary resource usage. Default: indoor maps enabled. Recommended: 1.",
                    Tags = ["maps", "indoor", "floorplan", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Indoor floor-plan maps are disabled; building interior layouts are not shown.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableIndoorMaps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableIndoorMaps")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableIndoorMaps", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-3d-maps",
                    Label = "Disable 3D Maps View (Birds Eye / Air View)",
                    Category = "Network",
                    Description =
                        "Prevents Windows Maps from loading 3D aerial/birds-eye imagery tiles. 3D tiles are much larger than standard tiles and result in significant bandwidth consumption. On managed endpoints with limited bandwidth or no approved use of Maps, this reduces network overhead. Default: 3D imagery enabled. Recommended: 1.",
                    Tags = ["maps", "3d", "aerial", "network", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "3D birds-eye view tiles are not downloaded; Maps shows only flat 2D cartographic view.",
                    ApplyOps = [RegOp.SetDword(Key, "Disable3DMaps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Disable3DMaps")],
                    DetectOps = [RegOp.CheckDword(Key, "Disable3DMaps", 1)],
                },
            ];
    }

    // ── MobilityPolicy ──
    private static class _MobilityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Mobility";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "mob-disable-cellular-data-roaming",
                Label = "Mobility Policy: Disable Cellular Data Roaming",
                Category = "Network",
                Description =
                    "Prevents Windows from enabling cellular data roaming, which connects to and uses foreign carrier networks at potentially extreme per-MB charges. "
                    + "On enterprise-managed endpoints with cellular adapters, roaming data costs can accumulate without user awareness. "
                    + "Disabling via policy overrides any SIM-level or carrier profile allowing roaming. "
                    + "Removing this policy reverts cellular data roaming to device/SIM defaults.",
                Tags = ["mobility", "cellular", "roaming", "cost", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCellularDataRoaming", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCellularDataRoaming")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCellularDataRoaming", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks cellular roaming; prevents unexpected international data charges on managed endpoints.",
            },
            new TweakDef
            {
                Id = "mob-disable-mobile-hotspot",
                Label = "Mobility Policy: Disable Mobile Hotspot Sharing",
                Category = "Network",
                Description =
                    "Prevents the device from being configured as a mobile hotspot that shares its cellular or Wi-Fi connection with other devices. "
                    + "Mobile hotspot sharing bypasses network access controls and can expose the corporate network to unauthorised connected devices. "
                    + "Removing this policy allows mobile hotspot sharing to be configured.",
                Tags = ["mobility", "hotspot", "tethering", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMobileHotspot", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMobileHotspot")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMobileHotspot", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks mobile hotspot; prevents unauthorised network sharing from managed endpoints.",
            },
            new TweakDef
            {
                Id = "mob-disable-usb-tethering",
                Label = "Mobility Policy: Disable USB Tethering",
                Category = "Network",
                Description =
                    "Prevents the device from being used as a USB tethering gateway, sharing its internet connection via USB to other devices. "
                    + "USB tethering creates a NAT bridge that can leak network traffic around firewall controls. "
                    + "Removing this policy allows USB tethering configuration.",
                Tags = ["mobility", "usb", "tethering", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableUsbTethering", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUsbTethering")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUsbTethering", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks USB tethering; prevents NAT bridge that could route around network firewalls.",
            },
            new TweakDef
            {
                Id = "mob-disable-automatic-connection-switch",
                Label = "Mobility Policy: Disable Auto WiFi-to-Cellular Switch",
                Category = "Network",
                Description =
                    "Prevents Windows from automatically switching the active network connection from Wi-Fi to cellular when Wi-Fi signal drops. "
                    + "Automatic switching can result in cellular data consumption and unexpected data charges on limited data plans. "
                    + "On enterprise machines the network handover should be manual or policy-driven. "
                    + "Removing this policy re-enables automatic Wi-Fi to cellular failover.",
                Tags = ["mobility", "wifi", "cellular", "auto-switch", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoConnectionSwitch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoConnectionSwitch")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoConnectionSwitch", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents auto Wi-Fi→cellular failover; avoids unintended cellular data consumption.",
            },
            new TweakDef
            {
                Id = "mob-disable-bluetooth-tethering",
                Label = "Mobility Policy: Disable Bluetooth Tethering",
                Category = "Network",
                Description =
                    "Prevents the device from sharing its internet connection via Bluetooth DUN (Dial-Up Networking) to devices paired over Bluetooth. "
                    + "Bluetooth tethering is a lower-visibility bridging path that can expose sensitive traffic without user awareness. "
                    + "Removing this policy allows Bluetooth tethering to be configured.",
                Tags = ["mobility", "bluetooth", "tethering", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBluetoothTethering", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBluetoothTethering")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBluetoothTethering", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks Bluetooth internet sharing; closes low-visibility network bridging path.",
            },
            new TweakDef
            {
                Id = "mob-disable-data-sense",
                Label = "Mobility Policy: Disable Data Sense Usage Monitoring",
                Category = "Network",
                Description =
                    "Disables the Data Sense feature that monitors per-app cellular usage and restricts background data on metered connections. "
                    + "On managed endpoints, data usage enforcement should come from network policy rather than per-device Data Sense heuristics. "
                    + "Removing this policy re-enables Data Sense monitoring and throttling.",
                Tags = ["mobility", "data-sense", "metered", "monitoring", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDataSense", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDataSense")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDataSense", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Turns off Data Sense heuristics; network policy controls take over data management.",
            },
            new TweakDef
            {
                Id = "mob-disable-carrier-provisioning",
                Label = "Mobility Policy: Disable Carrier Provisioning Updates",
                Category = "Network",
                Description =
                    "Prevents mobile carriers from remotely pushing provisioning XML updates to the device that can change network settings, APN configurations, and restrictions. "
                    + "Carrier provisioning is an automated out-of-band configuration channel that can override IT-managed network settings. "
                    + "Removing this policy allows carriers to provision the device with their default network profiles.",
                Tags = ["mobility", "carrier", "provisioning", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCarrierProvisioning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCarrierProvisioning")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCarrierProvisioning", 1)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote =
                    "Blocks OTA carrier provisioning; prevents carriers from overriding IT network settings. May break initial cellular setup.",
            },
            new TweakDef
            {
                Id = "mob-disable-wifi-sense",
                Label = "Mobility Policy: Disable Wi-Fi Sense Auto-Connect",
                Category = "Network",
                Description =
                    "Disables Wi-Fi Sense, which automatically connects to crowdsourced open Wi-Fi hotspots and can share Wi-Fi credentials with contacts. "
                    + "Wi-Fi Sense credential-sharing is a privacy and security risk on enterprise networks — credentials can be propagated to users' personal device contacts. "
                    + "Removing this policy re-enables Wi-Fi Sense auto-connect behaviour.",
                Tags = ["mobility", "wifi-sense", "credentials", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWifiSense", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWifiSense")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWifiSense", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables Wi-Fi Sense; prevents automatic hotspot connection and Wi-Fi credential sharing.",
            },
            new TweakDef
            {
                Id = "mob-disable-network-roaming-policy",
                Label = "Mobility Policy: Disable Network Roaming Profiles Sync",
                Category = "Network",
                Description =
                    "Prevents user roaming profiles from synchronising over cellular connections when roaming on a foreign network. "
                    + "Syncing large roaming profiles over cellular roaming can incur significant data charges and slow the logon process. "
                    + "Removing this policy allows roaming profile sync over any active connection.",
                Tags = ["mobility", "roaming-profile", "cellular", "logon", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRoamingProfileSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRoamingProfileSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRoamingProfileSync", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks profile sync over cellular roaming; avoids data charges during international travel.",
            },
            new TweakDef
            {
                Id = "mob-disable-wwan-ui",
                Label = "Mobility Policy: Disable WWAN Cellular UI Controls",
                Category = "Network",
                Description =
                    "Hides the cellular (WWAN) control panel and settings UI from users, preventing manual changes to cellular configuration on managed endpoints. "
                    + "On enterprise endpoints where cellular settings are managed via MDM or IT policy, user-facing cellular UI is redundant and can lead to misconfiguration. "
                    + "Removing this policy restores the WWAN settings UI.",
                Tags = ["mobility", "wwan", "ui", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWwanUI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWwanUI")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWwanUI", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides WWAN settings UI; prevents users from manually reconfiguring managed cellular connections.",
            },
        ];
    }

    // ── NearbySharingPolicy ──
    private static class _NearbySharingPolicy
    {
        private const string NearbyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NearbySharing";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "nshpol-disable-nearby-sharing",
                    Label = "Disable Nearby Sharing (GPO)",
                    Category = "Network",
                    Description =
                        "Sets DisableNearbySharing=1 to disable the Windows Nearby Sharing feature via Group Policy. Prevents file and URL transfers between nearby devices over Bluetooth and Wi-Fi Direct.",
                    Tags = ["nearby", "sharing", "bluetooth", "wifi-direct", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Removes Nearby Sharing from Action Center and context menus; local file moves unaffected.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "DisableNearbySharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "DisableNearbySharing")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "DisableNearbySharing", 1)],
                },
                new TweakDef
                {
                    Id = "nshpol-block-paired-devices",
                    Label = "Block Paired Device File Sharing",
                    Category = "Network",
                    Description =
                        "Sets AllowPairedDevices=0. Prevents this device from sharing files or URLs with paired Bluetooth devices through the Nearby Sharing subsystem, even when the feature itself is enabled.",
                    Tags = ["nearby", "paired", "bluetooth", "sharing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks file transfers to paired Bluetooth devices; standard Bluetooth audio unaffected.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowPairedDevices", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowPairedDevices")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowPairedDevices", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-disable-message-sync",
                    Label = "Disable Phone Link Message Sync",
                    Category = "Network",
                    Description =
                        "Sets AllowMessageSync=0 in the Nearby Sharing policy. Prevents SMS and MMS messages from being synced from a paired Android device to this PC through the Phone Link (formerly Your Phone) application.",
                    Tags = ["nearby", "phone", "message", "sync", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks phone SMS sync in Phone Link; call notifications and other features may still work.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowMessageSync", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowMessageSync")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowMessageSync", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-block-contacts-sync",
                    Label = "Block Phone Link Contacts Sync",
                    Category = "Network",
                    Description =
                        "Sets AllowContactsSync=0. Prevents the Phone Link app from synchronising device contacts from a paired phone to the Windows People app or contact suggestions across the OS.",
                    Tags = ["nearby", "phone", "contacts", "sync", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks contact sync from paired phone; standalone Windows contacts unaffected.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowContactsSync", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowContactsSync")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowContactsSync", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-disable-phone-link",
                    Label = "Disable Phone Link Feature (GPO)",
                    Category = "Network",
                    Description =
                        "Sets DisablePhoneLinkFromSettings=1. Removes the Phone Link pairing option from the Windows Settings app, preventing users from linking a mobile phone to this PC through the connected-devices platform.",
                    Tags = ["nearby", "phone-link", "pairing", "policy", "settings"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Hides Phone Link from Settings; existing phone pairings may persist until removed manually.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "DisablePhoneLinkFromSettings", 1)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "DisablePhoneLinkFromSettings")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "DisablePhoneLinkFromSettings", 1)],
                },
                new TweakDef
                {
                    Id = "nshpol-restrict-to-my-devices",
                    Label = "Restrict Nearby Sharing to My Devices Only",
                    Category = "Network",
                    Description =
                        "Sets SharingScope=1 to restrict Nearby Sharing to devices signed in with the same Microsoft or Azure AD account. Prevents sharing with unknown nearby devices while preserving same-account file transfers.",
                    Tags = ["nearby", "scope", "trusted", "devices", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Limits sharing to own devices only; removes 'Everyone nearby' option from sharing scope.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "SharingScope", 1)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "SharingScope")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "SharingScope", 1)],
                },
                new TweakDef
                {
                    Id = "nshpol-block-bluetooth-sharing",
                    Label = "Block Bluetooth File Sharing",
                    Category = "Network",
                    Description =
                        "Sets AllowBluetoothSharing=0 in the Nearby Sharing policy. Blocks the Bluetooth Object Push Profile (OPP) used by Nearby Sharing, preventing file reception from any Bluetooth source.",
                    Tags = ["nearby", "bluetooth", "opp", "sharing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Disables Bluetooth file receipt via OPP; Bluetooth audio and peripherals unaffected.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowBluetoothSharing", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowBluetoothSharing")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowBluetoothSharing", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-block-wifi-direct-sharing",
                    Label = "Block Wi-Fi Direct Nearby Sharing",
                    Category = "Network",
                    Description =
                        "Sets AllowWifiDirectNearbySharing=0. Disables the Wi-Fi Direct transport layer used for Nearby Sharing. Prevents high-speed peer-to-peer file transfers that bypass the corporate network.",
                    Tags = ["nearby", "wifi-direct", "p2p", "sharing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks Wi-Fi Direct used for large file transfers in Nearby Sharing.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowWifiDirectNearbySharing", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowWifiDirectNearbySharing")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowWifiDirectNearbySharing", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-disable-activity-feed-sharing",
                    Label = "Disable Nearby Activity Feed Sharing",
                    Category = "Network",
                    Description =
                        "Sets AllowActivityFeed=0 in the Nearby Sharing policy scope. Prevents the connected-devices platform from broadcasting recently-used documents and activities to nearby enrolled devices via the activity feed.",
                    Tags = ["nearby", "activity", "feed", "sharing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Stops nearby activity broadcasting; Timeline/Activity History within this PC unchanged.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowActivityFeed", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowActivityFeed")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowActivityFeed", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-block-cross-device-clipboard",
                    Label = "Block Cross-Device Clipboard via Nearby Sharing",
                    Category = "Network",
                    Description =
                        "Sets AllowCrossDeviceClipboard=0 in the Nearby Sharing policy. Prevents clipboard content from being sent or received between nearby Windows devices through the connected-devices platform, blocking near-field clipboard data exfiltration.",
                    Tags = ["nearby", "clipboard", "cross-device", "sharing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks near-field clipboard transfers; cloud clipboard sync is a separate setting.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowCrossDeviceClipboard", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowCrossDeviceClipboard")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowCrossDeviceClipboard", 0)],
                },
            ];
    }

    // ── NetBiosPolicy ──
    private static class _NetBiosPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetBIOS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "netbios-disable-netbios-over-tcpip",
                Label = "Disable NetBIOS over TCP/IP on All Network Interfaces",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "NetBIOS over TCP/IP is a legacy name resolution protocol from the 1980s that is rarely required in modern DNS-based networks but provides significant attack surface for lateral movement. Disabling NetBIOS over TCP/IP eliminates LLMNR, NBT-NS, and WINS name resolution attacks that are used by tools like Responder to capture NTLM credentials. NetBIOS poisoning attacks intercept broadcast name resolution requests and respond with attacker-controlled responses causing systems to authenticate to attacker servers. Organizations that have fully migrated to DNS for name resolution have no functional need for NetBIOS and should disable it on all interfaces. Before disabling NetBIOS verify that no legacy applications require NetBIOS name resolution as this can break application connectivity in mixed environments. Disabling NetBIOS is a CIS benchmark recommendation that significantly hardens Windows systems against LLMNR/NBT-NS capture attacks.",
                Tags = ["netbios", "llmnr", "name-resolution", "ntlm-relay", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNetBIOS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNetBIOS")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNetBIOS", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-llmnr-resolution",
                Label = "Disable Link-Local Multicast Name Resolution",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Link-Local Multicast Name Resolution (LLMNR) is a fallback name resolution protocol that broadcasts queries to the local network segment and can be poisoned by attackers. Disabling LLMNR prevents name resolution poisoning attacks where Responder and similar tools intercept LLMNR queries and respond with attacker-controlled IP addresses redirecting authentication traffic. LLMNR-based credential capture is one of the most common techniques used during internal network penetration testing due to its near-universal success in environments that have not disabled LLMNR. Disabling LLMNR has minimal impact on modern Windows environments that use DNS as the primary name resolution mechanism. Organizations should disable LLMNR across all systems in their domain as a standard hardening step that is low-risk and high-reward. LLMNR disabling is controlled through the EnableMulticast registry value under the DNSClient policy key rather than a dedicated LLMNR key.",
                Tags = ["netbios", "llmnr", "multicast", "credential-capture", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLLMNR", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLLMNR")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLLMNR", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-wins-client",
                Label = "Disable WINS Client Name Resolution",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Windows Internet Name Service (WINS) client is a legacy NetBIOS name resolution service that maps NetBIOS names to IP addresses and predates DNS as the network name resolution standard. Disabling WINS client name resolution removes a legacy attack surface for name resolution spoofing and simplifies the network stack by removing unused legacy protocols. WINS infrastructure is rarely deployed in modern enterprise environments that have migrated to DNS for all name resolution requirements. Disabling WINS client prevents the system from querying WINS servers that may be attackers spoofing the WINS server to redirect name resolution. Organizations still running WINS for legacy application compatibility should have a migration plan to retire WINS and transition fully to DNS. Windows Server 2012 R2 was the last version to ship with WINS as an installable server role making now the time for WINS infrastructure retirement.",
                Tags = ["netbios", "wins", "name-resolution", "legacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWINSClient", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWINSClient")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWINSClient", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-netbios-name-broadcasts",
                Label = "Disable NetBIOS Name Broadcast Announcements",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "NetBIOS name broadcasts announce the system's NetBIOS name to the local network segment providing attackers with host discovery and naming information. Disabling NetBIOS name broadcasts reduces the information available to attackers performing network reconnaissance and eliminates broadcast-based credential capture opportunities. NetBIOS announcements also consume network bandwidth and CPU resources particularly on large flat networks with many systems broadcasting simultaneously. Modern Windows systems that use DNS-SD or other modern discovery protocols do not require NetBIOS broadcasts for network discovery functionality. Disabling broadcasts prevents tools like NetBIOS scanners from easily discovering systems and their NetBIOS names during reconnaissance. Organizations with large flat Networks should prioritize disabling NetBIOS broadcasts to reduce both security risk and unnecessary broadcast traffic.",
                Tags = ["netbios", "broadcasts", "reconnaissance", "network-discovery", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNameBroadcast", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNameBroadcast")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNameBroadcast", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-nbt-ns-resolution",
                Label = "Disable NetBIOS over TCP/IP Name Service Queries",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "NetBIOS Name Service (NBT-NS) queries are broadcast UDP requests for name resolution that are intercepted by poisoning tools to capture NTLM credentials. Disabling NBT-NS query transmission prevents the system from sending NBT-NS queries that can be captured and responded to by attacker tools. NBT-NS poisoning is one of the most common attack techniques used in Active Directory environment compromises because it is reliable and does not require any vulnerability. Organizations that have disabled DNS fallback to NetBIOS can safely disable NBT-NS without impacting name resolution for modern applications. Firewall rules blocking UDP port 137 at the host level provide an additional layer of protection against NBT-NS exploitation. The combination of disabling LLMNR, NBT-NS, and WINS eliminates all multicast and broadcast name resolution attack vectors from the system.",
                Tags = ["netbios", "nbt-ns", "poisoning", "ntlm-relay", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNBTNS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNBTNS")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNBTNS", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-computer-browser-service",
                Label = "Disable Computer Browser Service NetBIOS Dependency",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The Computer Browser service maintains a list of computers and resources on the network using NetBIOS broadcasts and is rarely needed in modern AD environments. Disabling the Computer Browser service eliminates the NetBIOS dependency it creates and removes the master browser election process that generates unnecessary broadcast traffic. Browser service elections on large networks can cause periodic network storms as systems compete for master browser status. Modern Windows environments use Active Directory for computer discovery and organizational structure making the legacy Computer Browser service redundant. Disabling Computer Browser has no impact on Active Directory domain functionality including group policy, authentication, or shared resource access. The Computer Browser service should be disabled and set to Manual or Disabled startup to prevent automatic startup in future sessions.",
                Tags = ["netbios", "computer-browser", "broadcast", "legacy-service", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableComputerBrowser", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableComputerBrowser")],
                DetectOps = [RegOp.CheckDword(Key, "DisableComputerBrowser", 1)],
            },
            new TweakDef
            {
                Id = "netbios-restrict-smb-netbios-sharing",
                Label = "Restrict SMB NetBIOS File Sharing to Authenticated Users",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SMB over NetBIOS (ports 139) is a legacy file sharing path that runs alongside the modern SMB direct connection (port 445) and represents additional attack surface. Restricting SMB over NetBIOS to authenticated users prevents anonymous and unauthenticated access attempts that probe legacy SMB services. Port 139 represents an older NetBIOS session service for SMB that is rarely needed in modern networks running purely SMB 2.0 or later. Organizations should configure Windows Firewall to block port 139 inbound traffic in addition to policy-based NetBIOS restrictions. Disabling NetBT in the network adapter settings removes the NetBIOS over TCP/IP stack entirely eliminating port 137, 138, and 139 from the system's network exposure. Legacy applications that require NetBIOS for file sharing should be migrated to use standard SMB over port 445.",
                Tags = ["netbios", "smb", "file-sharing", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictAnonymousNetBIOS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictAnonymousNetBIOS")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictAnonymousNetBIOS", 1)],
            },
            new TweakDef
            {
                Id = "netbios-audit-netbios-name-queries",
                Label = "Enable Audit Logging for NetBIOS Name Query Events",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "NetBIOS name query audit logging captures all NetBIOS name lookup events providing visibility into legacy name resolution usage and potential poisoning activity. Enabling NetBIOS query audit logging helps identify systems that still rely on NetBIOS name resolution which should be migrated to DNS before NetBIOS is disabled. Audit data from NetBIOS queries can reveal hidden dependencies on NetBIOS in legacy applications that would break if NetBIOS were disabled without investigation. NetBIOS audit events combined with network monitoring help detect LLMNR and NBT-NS poisoning attacks in progress by correlating unexpected name resolution responses. Organizations should run NetBIOS query auditing for 30 days before disabling NetBIOS to identify all systems and applications that depend on it. Regular review of NetBIOS audit data after deploying other restrictions helps confirm that the restrictions are working and no bypass paths exist.",
                Tags = ["netbios", "audit", "monitoring", "name-resolution", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditNameQueries", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditNameQueries")],
                DetectOps = [RegOp.CheckDword(Key, "AuditNameQueries", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-netbt-registration",
                Label = "Disable NetBIOS Computer Name Registration on Network",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "NetBIOS computer name registration broadcasts the system hostname to all systems on the local network segment providing attacker-friendly network enumeration data. Disabling NetBT name registration prevents the system from advertising its hostname through NetBIOS reducing the information available for network reconnaissance. NetBIOS name registration on modern networks duplicates DNS registration and adds unnecessary broadcast traffic while providing attack surface. Systems that disable NetBIOS name registration will not be discoverable through NetBIOS enumeration tools but remain accessible through DNS-based discovery. Penetration tester tools like nbtscan rely on NetBIOS name registration to enumerate Windows systems making disabling registration a valuable hardening step. Organizations should combine disabling NetBIOS registration with DNS hostname privacy configurations for comprehensive network discovery hardening.",
                Tags = ["netbios", "name-registration", "reconnaissance", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNBTRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNBTRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNBTRegistration", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-multicast-dns",
                Label = "Disable Multicast DNS to Prevent mDNS-Based Poisoning",
                Category = "Network",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Multicast DNS (mDNS) is a zero-configuration networking protocol that resolves hostnames on local networks without a central DNS server and is exploitable for name poisoning similar to LLMNR. Disabling mDNS prevents mDNS-based credential capture attacks where tools like Responder implement mDNS poisoning to steal NTLM credentials. mDNS shares similar attack characteristics with LLMNR and NBT-NS and should be disabled alongside them for comprehensive broadcast name resolution hardening. Windows uses mDNS implemented through the DNS Client service and disabling it is controlled through policy rather than removing the service. mDNS is more commonly needed for Apple Bonjour-compatible devices and IoT devices than for standard Windows domain environments. Organizations should disable mDNS on domain-joined Windows systems while evaluating whether IoT or Apple devices on the same network segment require it.",
                Tags = ["netbios", "mdns", "multicast-dns", "poisoning", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMulticastDNS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMulticastDNS")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMulticastDNS", 1)],
            },
        ];
    }

    // ── NetCfgPolicy ──
    private static class _NetCfgPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NCSI";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "netcfg-disable-ncsi-active-probe",
                Label = "Disable Network Connectivity Status Indicator Active Internet Probe",
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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
                Category = "Network",
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

    // ── NetIoOffloadPolicy ──
    private static class _NetIoOffloadPolicy
    {
        private const string TcpKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";

        private const string TcpifKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters";

        private const string AfDKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "netio-set-tcp-checksum-hardware",
                    Label = "Net IO: Enable TCP/IP Hardware Checksum Offload",
                    Category = "Network",
                    Description =
                        "Sets EnableTCPChimneyOffload=1 and ChecksumOffloadEnabled=1 in TCP/IP settings. Configures the TCP/IP stack to delegate TCP and IP header checksum computation to the network adapter hardware (TCP Offload Engine). Hardware checksum computation removes the CPU overhead of per-packet checksum calculations from the host CPU. On servers handling 10 Gbps+ traffic or high-PPS packet flows, hardware checksum offload can reduce CPU utilization by 5–15% for network processing.",
                    Tags = ["tcp", "checksum", "offload", "hardware", "nic"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Offloads TCP checksum to NIC hardware. Requires checksum offload-capable NIC (all modern enterprise NICs support this). Some virtualization hypervisors may intercept and verify checksums.",
                    ApplyOps = [RegOp.SetDword(TcpKey, "EnableTCPChimneyOffload", 1)],
                    RemoveOps = [RegOp.DeleteValue(TcpKey, "EnableTCPChimneyOffload")],
                    DetectOps = [RegOp.CheckDword(TcpKey, "EnableTCPChimneyOffload", 1)],
                },
                new TweakDef
                {
                    Id = "netio-set-tcp-autotuning-high",
                    Label = "Net IO: Set TCP Window Autotuning to Highly Restricted Mode",
                    Category = "Network",
                    Description =
                        "Sets TcpAutoTuningLevel=4 (highly restricted) in TCP/IP parameters. Sets TCP receive window auto-tuning to a conservative algorithm that grows the receive buffer more cautiously than the default. The highly restricted mode is appropriate for environments with high-speed last-mile but intermediate links with lossy behavior (satellite links, 4G LTE backhaul) where aggressive window growth causes sporadic retransmit storms. On reliable Ethernet networks, 'normal' (0) autotuning provides higher throughput.",
                    Tags = ["tcp", "autotuning", "window", "buffer", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote =
                        "Conservative TCP window scaling may reduce throughput on high-bandwidth low-latency links. Only recommended for networks with frequent packet loss (satellite, LTE backhaul).",
                    ApplyOps = [RegOp.SetDword(TcpKey, "TcpAutoTuningLevel", 4)],
                    RemoveOps = [RegOp.DeleteValue(TcpKey, "TcpAutoTuningLevel")],
                    DetectOps = [RegOp.CheckDword(TcpKey, "TcpAutoTuningLevel", 4)],
                },
                new TweakDef
                {
                    Id = "netio-enable-rss",
                    Label = "Net IO: Enable Receive-Side Scaling (RSS) for Multi-CPU Load",
                    Category = "Network",
                    Description =
                        "Sets EnableRSS=1 and MaxRssProcessors=4 in network adapter policy. Enables Receive-Side Scaling (RSS) which distributes incoming network packet processing across multiple CPU cores. Without RSS, all incoming traffic for a given NIC is processed on a single CPU core, creating a per-core throughput bottleneck at approximately 3–5 Gbps on modern hardware. With RSS, incoming packets are hashed to multiple CPU queues, scaling receive throughput linearly with CPU cores up to the NIC's hardware RSS queue limit.",
                    Tags = ["rss", "networking", "cpu", "performance", "offload"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires RSS-capable NIC (all server-class NICs support RSS). RSS distributes interrupts across CPUs which may change CPU affinity behavior of network-intensive processes.",
                    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS", "EnableRSS", 1)],
                    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS", "EnableRSS")],
                    DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS", "EnableRSS", 1)],
                },
                new TweakDef
                {
                    Id = "netio-set-afd-fast-send-datagram",
                    Label = "Net IO: Enable AFD Fast Send Datagram Path",
                    Category = "Network",
                    Description =
                        "Sets FastSendDatagramThreshold=1024 in AFD parameters. Configures the Windows Ancillary Function Driver (AFD) to use the fast datagram send path for UDP packets under 1024 bytes. The fast path bypasses several AFD buffer validation steps for trusted-size datagrams, reducing per-packet CPU cost for high-PPS UDP workloads. This benefits applications generating large volumes of small UDP packets: DNS servers processing thousands of queries per second, game servers, or network telemetry agents.",
                    Tags = ["afd", "udp", "datagram", "fast-path", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Fast path bypasses some buffer validation for small UDP datagrams. Testing recommended for high-PPS DNS server workloads before production deployment.",
                    ApplyOps = [RegOp.SetDword(AfDKey, "FastSendDatagramThreshold", 1024)],
                    RemoveOps = [RegOp.DeleteValue(AfDKey, "FastSendDatagramThreshold")],
                    DetectOps = [RegOp.CheckDword(AfDKey, "FastSendDatagramThreshold", 1024)],
                },
                new TweakDef
                {
                    Id = "netio-disable-ipv6-source-routing",
                    Label = "Net IO: Disable IPv6 Source Routing (Anti-Spoofing)",
                    Category = "Network",
                    Description =
                        "Sets DisableIPv6SourceRouting=1 in TCPv6 parameters. Drops IPv6 packets containing Type 0 Routing Header (RH0) extension headers. IPv6 RH0 was used in major amplified DoS attacks (CVE-2007-2242) where a small packet can be amplified to an enormous amount of traffic by specifying 127 intermediate hops in the routing header. RFC 5095 deprecated RH0; all modern networks should drop RH0-containing packets. This setting enforces RFC 5095 at the host‐stack level.",
                    Tags = ["ipv6", "source-routing", "rh0", "dos", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "No legitimate applications use IPv6 RH0 since RFC 5095 deprecation in 2007. This setting has no impact on normal network operations.",
                    ApplyOps = [RegOp.SetDword(TcpifKey, "DisableIPv6SourceRouting", 1)],
                    RemoveOps = [RegOp.DeleteValue(TcpifKey, "DisableIPv6SourceRouting")],
                    DetectOps = [RegOp.CheckDword(TcpifKey, "DisableIPv6SourceRouting", 1)],
                },
            ];
    }

    // ── NetLocationAwarenessAdvancedPolicy ──
    private static class _NetLocationAwarenessAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkList\Signatures";
        private const string NlmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkList";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "nlaadv-always-classify-as-public",
                    Label = "Always Classify Unrecognised Networks as Public",
                    Category = "Network",
                    Description =
                        "Forces Windows to classify all new or unrecognised network connections as Public network profile (most restrictive firewall rules) until explicitly reclassified by an administrator, applying maximum firewall protection to unknown networks.",
                    Tags = ["nla", "network-classification", "public-profile", "firewall", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Unknown networks classified as Public; most restrictive firewall rules apply to all unrecognised connections.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "DefaultClassification", 0)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "DefaultClassification")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "DefaultClassification", 0)],
                },
                new TweakDef
                {
                    Id = "nlaadv-block-user-profile-reclassification",
                    Label = "Block Standard Users from Reclassifying Network Profiles",
                    Category = "Network",
                    Description =
                        "Prevents standard users from changing a network's classification (Private/Public/Domain Work) in Windows, ensuring that firewall profile assignments can only be modified by administrators.",
                    Tags = ["nla", "network-profile", "reclassification", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Network profile reclassification blocked for standard users; firewall profile changes require admin.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "AllowUserSetNetworkLocation", 0)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "AllowUserSetNetworkLocation")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "AllowUserSetNetworkLocation", 0)],
                },
                new TweakDef
                {
                    Id = "nlaadv-disable-domain-network-upgrade",
                    Label = "Disable Automatic Domain Network Upgrade from NLA",
                    Category = "Network",
                    Description =
                        "Prevents NLA from automatically reclassifying a network from Public/Private to Domain Work profile when domain controllers are reachable, keeping explicit admin-assigned firewall profiles even on domain member machines.",
                    Tags = ["nla", "domain-profile", "auto-detect", "firewall", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NLA domain auto-upgrade disabled; domain networks stay at assigned profile without auto-promotion.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "DisableDomainNetworkAutoDetect", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableDomainNetworkAutoDetect")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "DisableDomainNetworkAutoDetect", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-log-profile-change-events",
                    Label = "Log Network Profile Change Events",
                    Category = "Network",
                    Description =
                        "Enables System event log entries when a network connection profile is changed (Private to Public, etc.), providing audit visibility into firewall profile transitions that could weaken security posture.",
                    Tags = ["nla", "network-profile", "event-log", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Network profile changes logged; firewall profile transitions recorded in System event log.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "LogNetworkProfileChanges", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "LogNetworkProfileChanges")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "LogNetworkProfileChanges", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-disable-nla-internet-probe",
                    Label = "Disable NLA Internet Connectivity Probe (NCSI Bypass)",
                    Category = "Network",
                    Description =
                        "Disables the Network Connectivity Status Indicator (NCSI) probe that NLA sends to Microsoft servers (connectivity.microsoft.com) to determine internet connectivity status, preventing outbound probe traffic to cloud hosts.",
                    Tags = ["nla", "ncsi", "connectivity-probe", "microsoft", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NCSI internet probe disabled; no connectivity.microsoft.com probe. System tray may show 'No internet' falsely.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "DisableNCSI", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableNCSI")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "DisableNCSI", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-disable-captive-portal-detect",
                    Label = "Disable Captive Portal Detection",
                    Category = "Network",
                    Description =
                        "Disables Windows captive portal detection that redirects a browser to a hotel/airport landing page, preventing unwanted browser launches in locked-down environments and avoiding false-positive network change alerts.",
                    Tags = ["nla", "captive-portal", "hotspot", "browser", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Captive portal detection disabled; Windows does not auto-open browser when hotspot login required.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "DisableCaptivePortalDetection", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableCaptivePortalDetection")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "DisableCaptivePortalDetection", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-require-auth-for-private-upgrade",
                    Label = "Require User Authentication Before Private Network Upgrade",
                    Category = "Network",
                    Description =
                        "Requires an administrator confirmation before NLA upgrades a network from Public to Private profile, preventing accidental loosening of firewall rules when a device connects to an unknown but trusted-seeming network.",
                    Tags = ["nla", "private-profile", "authentication", "firewall", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Admin confirmation required before Private profile upgrade; prevents accidental firewall relaxation.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "RequireAuthForPrivateNetworkUpgrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "RequireAuthForPrivateNetworkUpgrade")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "RequireAuthForPrivateNetworkUpgrade", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-block-network-name-ui",
                    Label = "Hide Network Name and Location in System Tray",
                    Category = "Network",
                    Description =
                        "Removes the network name and location type from the system tray network flyout, preventing casual users from seeing and potentially modifying network profile names or locations.",
                    Tags = ["nla", "system-tray", "network-name", "ui", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Network name hidden in system tray; network profile names and types not shown in flyout.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "HideNetworkLocationUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "HideNetworkLocationUI")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "HideNetworkLocationUI", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-disable-network-telemetry",
                    Label = "Disable NLA Network Profile Telemetry to Microsoft",
                    Category = "Network",
                    Description =
                        "Prevents Network Location Awareness from sending network profile assignment and classification telemetry to Microsoft, protecting information about this machine's network environment from cloud disclosure.",
                    Tags = ["nla", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NLA telemetry to Microsoft disabled; network profile assignment and connectivity data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "DisableNLATelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableNLATelemetry")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "DisableNLATelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-set-profile-icon-classification",
                    Label = "Set Network Profile Icon to Reflect Classification",
                    Category = "Network",
                    Description =
                        "Configures the network profile icon in the system tray to visually reflect the current classification (Public/Private/Domain) to ensure users have immediate visual awareness of the active firewall profile strength.",
                    Tags = ["nla", "network-icon", "ui", "firewall-profile", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Network icon reflects profile classification; users see current Public/Private/Domain firewall level.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "ShowProfileClassificationIcon", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "ShowProfileClassificationIcon")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "ShowProfileClassificationIcon", 1)],
                },
            ];
    }

    // ── NetworkAccessProtectionPolicy ──
    private static class _NetworkAccessProtectionPolicy
    {
        private const string NapKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\NetworkAccessProtection\MSNAPAgent";
        private const string QuarantineKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\NetworkAccessProtection\Quarantine";
        private const string NapAgentKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\NAPAgent";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "napcomp-enable-nap-client-enforcement",
                Label = "NAP Policy: Enable NAP Client Enforcement Mode",
                Category = "Network",
                Description =
                    "Enables the Network Access Protection (NAP) client on the machine, allowing the machine to participate in NAP health validation and enforcement workflows. NAP is a policy-based network access control framework that evaluates the health state of a machine (antivirus, patch level, firewall status) before granting full network access. Enabling enforcement mode ensures that a machine reporting an unhealthy health state is placed in a restricted network segment until it is remediated.",
                Tags = ["nap", "network access protection", "compliance", "health validation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapKey],
                ApplyOps = [RegOp.SetDword(NapKey, "EnableNAPClient", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "EnableNAPClient")],
                DetectOps = [RegOp.CheckDword(NapKey, "EnableNAPClient", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Enables NAP health enforcement; machines that fail health checks may lose full network access — ensure NHPs and remediation servers are configured.",
            },
            new TweakDef
            {
                Id = "napcomp-require-health-validation",
                Label = "NAP Policy: Require Health Certificate for Network Access",
                Category = "Network",
                Description =
                    "Configures the NAP client to require a valid System Health Certificate (SHC) before being granted unrestricted network access. Without a health certificate, the machine is placed in the quarantine network. Health certificates are issued by the Health Registration Authority (HRA) after the NPS Health Policy Server verifies that all system health validators (SHVs) report a compliant state. This policy forms the core of the 802.1X-based NAP enforcement chain.",
                Tags = ["nap", "health certificate", "802.1x", "hps", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapKey],
                ApplyOps = [RegOp.SetDword(NapKey, "RequireHealthCertificate", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "RequireHealthCertificate")],
                DetectOps = [RegOp.CheckDword(NapKey, "RequireHealthCertificate", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Requires health certificate for full network access; misconfigured NAP infrastructure will quarantine all machines including healthy ones.",
            },
            new TweakDef
            {
                Id = "napcomp-enforce-vpn-shv",
                Label = "NAP Policy: Enable System Health Validation for VPN Connections",
                Category = "Network",
                Description =
                    "Activates System Health Validator (SHV) evaluation for VPN-based NAP enforcement, ensuring that remote machines connecting via VPN are subject to the same health compliance checks as internally-connected machines. Without VPN-SHV enforcement, remote workers can connect to the corporate network with out-of-date antivirus signatures or missing security patches while bypassing the health gating that on-premises machines face.",
                Tags = ["nap", "vpn", "system health validator", "remote access", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapKey],
                ApplyOps = [RegOp.SetDword(NapKey, "EnableVPNSHV", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "EnableVPNSHV")],
                DetectOps = [RegOp.CheckDword(NapKey, "EnableVPNSHV", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Extends NAP health checks to VPN connections; requires a configured NAP Network Policy Server and VPN server with NAP support.",
            },
            new TweakDef
            {
                Id = "napcomp-disable-auto-remediation",
                Label = "NAP Policy: Disable Automatic Health Remediation",
                Category = "Network",
                Description =
                    "Prevents the NAP client from automatically remediating detected health deficiencies by making software changes (e.g., enabling Windows Firewall, triggering Windows Update). Automatic remediation can make unexpected changes to a system without user awareness and may conflict with endpoint management tools (such as Intune or SCCM) that manage those settings centrally. Disabling auto-remediation forces the user or help desk to perform explicit remediation steps.",
                Tags = ["nap", "auto remediation", "endpoint management", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapKey],
                ApplyOps = [RegOp.SetDword(NapKey, "DisableAutoRemediation", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "DisableAutoRemediation")],
                DetectOps = [RegOp.CheckDword(NapKey, "DisableAutoRemediation", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote =
                    "Disables auto-remediation; unhealthy machines stay quarantined until manual intervention — ensure help desk procedures are documented.",
            },
            new TweakDef
            {
                Id = "napcomp-enforce-quarantine-timeout",
                Label = "NAP Policy: Set Quarantine Timeout Period for Unhealthy Machines",
                Category = "Network",
                Description =
                    "Configures how long (in minutes) a machine can remain in the quarantine network before its health is re-evaluated and upgraded to full access or flagged for intervention. A timeout of 480 minutes (8 hours) reflects a typical business-day remediation window. Without a timeout, a machine placed in quarantine due to a transient health failure stays restricted indefinitely unless an administrator intervenes or the NAP client is restarted.",
                Tags = ["nap", "quarantine", "timeout", "health policy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [QuarantineKey],
                ApplyOps = [RegOp.SetDword(QuarantineKey, "QuarantineTimeoutMinutes", 480)],
                RemoveOps = [RegOp.DeleteValue(QuarantineKey, "QuarantineTimeoutMinutes")],
                DetectOps = [RegOp.CheckDword(QuarantineKey, "QuarantineTimeoutMinutes", 480)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Sets quarantine timeout to 8 hours; machines that remediate earlier will get re-evaluated when they reconnect.",
            },
            new TweakDef
            {
                Id = "napcomp-enable-quarantine-state-pki",
                Label = "NAP Policy: Enable PKI-Based Quarantine State Machine",
                Category = "Network",
                Description =
                    "Activates the PKI-based state machine within the NAP client that uses X.509 certificates to encode the machine's health attestation state. The PKI state machine is required for the IPSEC enforcement method, which uses health certificates to gate machine-to-machine communication at the network layer. Without this, only 802.1X or DHCP-based NAP enforcement methods are available, which are less granular than IPSEC health-gated policies.",
                Tags = ["nap", "pki", "ipsec", "health attestation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapKey],
                ApplyOps = [RegOp.SetDword(NapKey, "EnablePKIStateMachine", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "EnablePKIStateMachine")],
                DetectOps = [RegOp.CheckDword(NapKey, "EnablePKIStateMachine", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Activates PKI health-state machine; requires a Health Registration Authority (HRA) and certificate infrastructure.",
            },
            new TweakDef
            {
                Id = "napcomp-enable-dhcp-enforcement",
                Label = "NAP Policy: Enable DHCP-Based NAP Enforcement",
                Category = "Network",
                Description =
                    "Enables the DHCP enforcement client for NAP, allowing the machine to participate in DHCP quarantine workflows. In DHCP enforcement mode, the DHCP server issues different IP address leases (quarantine vs. full-access scope) based on the client's health certificate. This is the simplest NAP enforcement method to deploy and requires no changes to network switches, making it suitable for organisations with legacy switching infrastructure.",
                Tags = ["nap", "dhcp enforcement", "quarantine", "network access", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapAgentKey],
                ApplyOps = [RegOp.SetDword(NapAgentKey, "EnableDHCPEnforcement", 1)],
                RemoveOps = [RegOp.DeleteValue(NapAgentKey, "EnableDHCPEnforcement")],
                DetectOps = [RegOp.CheckDword(NapAgentKey, "EnableDHCPEnforcement", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Enables DHCP-based NAP enforcement; requires NAP-capable DHCP server and NPS health policy configuration.",
            },
            new TweakDef
            {
                Id = "napcomp-enable-wired-8021x-enforcement",
                Label = "NAP Policy: Enable Wired 802.1X NAP Enforcement",
                Category = "Network",
                Description =
                    "Activates the 802.1X enforcement client for wired network connections, enabling switch-level NAP quarantine for machines that fail health evaluations. 802.1X enforcement is the strongest NAP mechanism because it operates at the switch port level — a machine placed in quarantine cannot communicate on the network at all without passing through the enforcement switch, regardless of IP configuration. This method requires 802.1X-capable managed switches.",
                Tags = ["nap", "802.1x", "wired enforcement", "switch", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapAgentKey],
                ApplyOps = [RegOp.SetDword(NapAgentKey, "EnableWired8021xEnforcement", 1)],
                RemoveOps = [RegOp.DeleteValue(NapAgentKey, "EnableWired8021xEnforcement")],
                DetectOps = [RegOp.CheckDword(NapAgentKey, "EnableWired8021xEnforcement", 1)],
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "Activates 802.1X NAP; requires managed 802.1X switches and NPS RADIUS configuration — test in lab before broad deployment.",
            },
            new TweakDef
            {
                Id = "napcomp-enable-ts-gateway-enforcement",
                Label = "NAP Policy: Enable Terminal Services Gateway NAP Enforcement",
                Category = "Network",
                Description =
                    "Enables the Terminal Services (Remote Desktop) Gateway NAP enforcement client. When active, the TS Gateway evaluates the client machine's NAP health certificate before establishing an RDP tunnel, ensuring that remote desktop sessions from unhealthy endpoints are blocked at the gateway. This is particularly important for privileged access workstations (PAWs) connecting to administrative systems — an infected admin workstation should not be allowed to initiate RDP sessions.",
                Tags = ["nap", "terminal services", "remote desktop", "gateway", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapAgentKey],
                ApplyOps = [RegOp.SetDword(NapAgentKey, "EnableTSGatewayEnforcement", 1)],
                RemoveOps = [RegOp.DeleteValue(NapAgentKey, "EnableTSGatewayEnforcement")],
                DetectOps = [RegOp.CheckDword(NapAgentKey, "EnableTSGatewayEnforcement", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Gates RDP gateway access on NAP health; requires a NAP-enabled TS Gateway and NPS policy configuration.",
            },
            new TweakDef
            {
                Id = "napcomp-enable-ipsec-enforcement",
                Label = "NAP Policy: Enable IPsec-Based NAP Enforcement",
                Category = "Network",
                Description =
                    "Activates the IPsec enforcement client for NAP, which uses short-lived health certificates to enforce IPsec policies between machines on the same network. IPsec NAP enforcement ensures that only machines with current, valid health certificates can communicate over authenticated IPsec channels. This is the most granular NAP enforcement method and enables zero-trust-style east-west traffic control within a corporate network segment.",
                Tags = ["nap", "ipsec", "zero trust", "east-west", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapAgentKey],
                ApplyOps = [RegOp.SetDword(NapAgentKey, "EnableIPsecEnforcement", 1)],
                RemoveOps = [RegOp.DeleteValue(NapAgentKey, "EnableIPsecEnforcement")],
                DetectOps = [RegOp.CheckDword(NapAgentKey, "EnableIPsecEnforcement", 1)],
                ImpactScore = 5,
                SafetyRating = 2,
                ImpactNote =
                    "Activates IPsec NAP; requires HRA, NPS, and IPsec policy infrastructure — extremely disruptive if misconfigured. Production testing mandatory.",
            },
        ];
    }

    // ── NetworkAccessProtPolicy ──
    private static class _NetworkAccessProtPolicy
    {
        private const string NapKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkAccessProtection";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "nappol-disable-nap-client",
                    Label = "Disable NAP Client Service",
                    Category = "Network",
                    Description =
                        "Sets Enabled=0 to disable the Network Access Protection client service. NAP was deprecated in Windows 10 but the client components remain; disabling prevents unnecessary service overhead.",
                    Tags = ["nap", "network", "policy", "service"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "NAP client disabled; no impact on modern networks that do not use NAP infrastructure.",
                    ApplyOps = [RegOp.SetDword(NapKey, "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(NapKey, "Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-dhcp-quarantine",
                    Label = "Disable NAP DHCP Quarantine Enforcement",
                    Category = "Network",
                    Description =
                        "Sets EnableDhcpQuarantine=0 to disable NAP enforcement through DHCP. Prevents the client from being quarantined to a restricted network when DHCP health checks fail.",
                    Tags = ["nap", "dhcp", "quarantine", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "DHCP NAP quarantine disabled; client bypasses network health checks via DHCP.",
                    ApplyOps = [RegOp.SetDword(NapKey, "EnableDhcpQuarantine", 0)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "EnableDhcpQuarantine")],
                    DetectOps = [RegOp.CheckDword(NapKey, "EnableDhcpQuarantine", 0)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-8021x-quarantine",
                    Label = "Disable NAP 802.1X Quarantine Enforcement",
                    Category = "Network",
                    Description =
                        "Sets Enable8021xQuarantine=0 to disable NAP enforcement through 802.1X-authenticated network switches. Prevents 802.1X-based client quarantine on wired/wireless networks.",
                    Tags = ["nap", "802.1x", "quarantine", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "802.1X NAP quarantine disabled; wired/wireless enforcement bypassed for this client.",
                    ApplyOps = [RegOp.SetDword(NapKey, "Enable8021xQuarantine", 0)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "Enable8021xQuarantine")],
                    DetectOps = [RegOp.CheckDword(NapKey, "Enable8021xQuarantine", 0)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-vpn-quarantine",
                    Label = "Disable NAP VPN Quarantine Enforcement",
                    Category = "Network",
                    Description =
                        "Sets EnableVpnQuarantine=0 to disable VPN-based NAP enforcement. Prevents VPN connections from triggering NAP health evaluations and potential quarantine.",
                    Tags = ["nap", "vpn", "quarantine", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 3,
                    ImpactNote = "VPN health checks via NAP bypassed; may reduce VPN connection setup time.",
                    ApplyOps = [RegOp.SetDword(NapKey, "EnableVpnQuarantine", 0)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "EnableVpnQuarantine")],
                    DetectOps = [RegOp.CheckDword(NapKey, "EnableVpnQuarantine", 0)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-ipsec-quarantine",
                    Label = "Disable NAP IPSec Quarantine Enforcement",
                    Category = "Network",
                    Description =
                        "Sets EnableIpsecQuarantine=0 to disable IPSec-based NAP health enforcement. Prevents IPSec connections from routing through NAP health validation and quarantine zones.",
                    Tags = ["nap", "ipsec", "quarantine", "network", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 3,
                    ImpactNote = "IPSec NAP health checks disabled; IPSec connections bypass health gating.",
                    ApplyOps = [RegOp.SetDword(NapKey, "EnableIpsecQuarantine", 0)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "EnableIpsecQuarantine")],
                    DetectOps = [RegOp.CheckDword(NapKey, "EnableIpsecQuarantine", 0)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-dhcp-auto-remediation",
                    Label = "Disable NAP DHCP Auto-Remediation",
                    Category = "Network",
                    Description =
                        "Sets DisableDhcpAutoRemediation=1 to prevent the NAP client from automatically attempting to remediate health failures during DHCP-based enforcement. Manual intervention is required.",
                    Tags = ["nap", "dhcp", "remediation", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Automatic DHCP remediation disabled; health policy failures require manual administrator action.",
                    ApplyOps = [RegOp.SetDword(NapKey, "DisableDhcpAutoRemediation", 1)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "DisableDhcpAutoRemediation")],
                    DetectOps = [RegOp.CheckDword(NapKey, "DisableDhcpAutoRemediation", 1)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-nap-status-notifications",
                    Label = "Disable NAP Status Notifications",
                    Category = "Network",
                    Description =
                        "Sets DisableStatusNotifications=1 to suppress NAP status change notifications from appearing to users. Network Access Protection events are logged but not displayed.",
                    Tags = ["nap", "notifications", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "NAP status notifications hidden from users; NAP events still logged in Event Viewer.",
                    ApplyOps = [RegOp.SetDword(NapKey, "DisableStatusNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "DisableStatusNotifications")],
                    DetectOps = [RegOp.CheckDword(NapKey, "DisableStatusNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-nap-ui",
                    Label = "Disable NAP User Interface",
                    Category = "Network",
                    Description =
                        "Sets DisableUserUi=1 to completely disable the Network Access Protection user interface. NAP-related dialogs and status pages are inaccessible to users.",
                    Tags = ["nap", "ui", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "NAP UI fully disabled; no user-facing NAP dialogs, status screens, or repair wizards.",
                    ApplyOps = [RegOp.SetDword(NapKey, "DisableUserUi", 1)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "DisableUserUi")],
                    DetectOps = [RegOp.CheckDword(NapKey, "DisableUserUi", 1)],
                },
                new TweakDef
                {
                    Id = "nappol-hide-nap-tray-icon",
                    Label = "Hide NAP System Tray Icon",
                    Category = "Network",
                    Description =
                        "Sets HideSystemTrayIcon=1 to prevent the NAP system tray notification icon from appearing. Reduces status bar clutter when NAP components are otherwise disabled.",
                    Tags = ["nap", "tray", "ui", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "NAP tray icon hidden; no impact on networking, only removes the visual indicator.",
                    ApplyOps = [RegOp.SetDword(NapKey, "HideSystemTrayIcon", 1)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "HideSystemTrayIcon")],
                    DetectOps = [RegOp.CheckDword(NapKey, "HideSystemTrayIcon", 1)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-nap-policy-autoupdate",
                    Label = "Disable NAP Policy Auto-Update",
                    Category = "Network",
                    Description =
                        "Sets DisablePolicyAutoUpdate=1 to prevent the NAP client from automatically downloading updated health requirement policies from the network policy server (NPS).",
                    Tags = ["nap", "policy", "update", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "NAP policy updates disabled; client retains last-known health policy settings.",
                    ApplyOps = [RegOp.SetDword(NapKey, "DisablePolicyAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "DisablePolicyAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(NapKey, "DisablePolicyAutoUpdate", 1)],
                },
            ];
    }
}
