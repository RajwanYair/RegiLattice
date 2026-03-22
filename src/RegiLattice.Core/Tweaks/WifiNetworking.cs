// RegiLattice.Core — Tweaks/WifiNetworking.cs
// Wireless network adapter and Wi-Fi settings (Sprint 95).
// Slug "wifi" — HKLM WiFi, WLAN service, and radio power settings.
// Distinct from Network.cs (general) and NetworkInterface.cs (wired TCP/IP).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WifiNetworking
{
    private const string WifiPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy";

    private const string WifiService = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WlanSvc\Parameters";

    private const string WiFiSense = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config";

    private const string WiFiSensePolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\wifinetworkmanager";

    private const string WlanProfile = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList\DefaultMediaCost";

    private const string NdisTcp = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wifi-disable-wifi-sense",
            Label = "Disable Wi-Fi Sense (Auto-Join Shared Networks)",
            Category = "Wi-Fi Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["wifi", "wi-fi sense", "auto connect", "privacy", "security"],
            Description =
                "Disables Wi-Fi Sense which automatically connects to networks shared by "
                + "contacts via Outlook or Facebook. AutoConnectAllowedOEM=0. "
                + "Prevents connecting to unknown shared networks without user approval.",
            ApplyOps = [RegOp.SetDword(WiFiSense, "AutoConnectAllowedOEM", 0)],
            RemoveOps = [RegOp.SetDword(WiFiSense, "AutoConnectAllowedOEM", 1)],
            DetectOps = [RegOp.CheckDword(WiFiSense, "AutoConnectAllowedOEM", 0)],
        },
        new TweakDef
        {
            Id = "wifi-disable-wifi-sense-policy",
            Label = "Disable Wi-Fi Sense via Group Policy",
            Category = "Wi-Fi Networking",
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
            Id = "wifi-disable-auto-switch-network",
            Label = "Disable Automatic Network Switching When Wired",
            Category = "Wi-Fi Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["wifi", "auto switch", "wired", "network priority"],
            Description =
                "Prevents Windows from automatically switching between Wi-Fi and Ethernet "
                + "connections. fMinimizeConnections=1. Ensures the active connection "
                + "is used exclusively without background network switching.",
            ApplyOps = [RegOp.SetDword(WifiPolicy, "fMinimizeConnections", 1)],
            RemoveOps = [RegOp.DeleteValue(WifiPolicy, "fMinimizeConnections")],
            DetectOps = [RegOp.CheckDword(WifiPolicy, "fMinimizeConnections", 1)],
        },
        new TweakDef
        {
            Id = "wifi-disable-hotspot2-roaming",
            Label = "Disable Hotspot 2.0 / Passpoint Auto-Connect",
            Category = "Wi-Fi Networking",
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
            Category = "Wi-Fi Networking",
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
            Category = "Wi-Fi Networking",
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
            Category = "Wi-Fi Networking",
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
            Category = "Wi-Fi Networking",
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
            Category = "Wi-Fi Networking",
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
            Category = "Wi-Fi Networking",
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
