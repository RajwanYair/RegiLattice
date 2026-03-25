// RegiLattice.Core — Tweaks/WlanPolicy.cs
// Sprint 328: WLAN Policy tweaks (10 tweaks)
// Category: "WLAN Policy" | Slug: wlanpol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WlanSvc

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WlanPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WlanSvc";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wlanpol-disable-auto-connect-to-open",
            Label = "Prevent Auto-Connect to Open Wireless Networks",
            Category = "WLAN Policy",
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
            Category = "WLAN Policy",
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
            Category = "WLAN Policy",
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
            Category = "WLAN Policy",
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
            Category = "WLAN Policy",
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
            Category = "WLAN Policy",
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
            Category = "WLAN Policy",
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
            Category = "WLAN Policy",
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
            Category = "WLAN Policy",
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
            Category = "WLAN Policy",
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
