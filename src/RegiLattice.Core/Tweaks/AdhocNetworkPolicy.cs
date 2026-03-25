// RegiLattice.Core — Tweaks/AdhocNetworkPolicy.cs
// Sprint 307: Ad-hoc Network Policy tweaks (10 tweaks)
// Category: "Adhoc Network Policy" | Slug: adhocnet
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WirelessNetwork

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AdhocNetworkPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WirelessNetwork";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "adhocnet-disable-adhoc-networks",
            Label = "Disable Ad-hoc Wireless Network Connections",
            Category = "Adhoc Network Policy",
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
            Category = "Adhoc Network Policy",
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
            Category = "Adhoc Network Policy",
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
            Category = "Adhoc Network Policy",
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
            Category = "Adhoc Network Policy",
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
            Category = "Adhoc Network Policy",
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
            Category = "Adhoc Network Policy",
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
            Category = "Adhoc Network Policy",
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
            Category = "Adhoc Network Policy",
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
            Category = "Adhoc Network Policy",
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
