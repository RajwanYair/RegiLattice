namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyNetwork
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                    Category = "Network — Adhoc Network",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                    Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Data Sense",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                    Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
}
