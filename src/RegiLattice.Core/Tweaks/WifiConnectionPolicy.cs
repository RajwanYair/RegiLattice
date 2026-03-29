// RegiLattice.Core — Tweaks/WifiConnectionPolicy.cs
// Wi-Fi connection manager, profile sharing, open-network, and SSID policy — Sprint 499.
// Category: "Wi-Fi Connection Policy" | Slug: wificonn
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WifiConnectionPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wificonn-minimize-simultaneous-connections",
                Label = "Block Simultaneous Connections to Multiple Networks",
                Category = "Wi-Fi Connection Policy",
                Description =
                    "Prevents Windows from maintaining simultaneous connections to both a domain network and a non-domain network, closing a potential data exfiltration path where traffic could be bridged between isolation zones.",
                Tags = ["wi-fi", "multi-homed", "domain", "segmentation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Simultaneous domain and non-domain network connections blocked; only one network active at a time.",
                ApplyOps = [RegOp.SetDword(Key, "fMinimizeConnections", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "fMinimizeConnections")],
                DetectOps = [RegOp.CheckDword(Key, "fMinimizeConnections", 3)],
            },
            new TweakDef
            {
                Id = "wificonn-disable-softap",
                Label = "Disable Windows Wi-Fi SoftAP (Software Access Point)",
                Category = "Wi-Fi Connection Policy",
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
                Id = "wificonn-block-non-domain-when-on-domain",
                Label = "Block Non-Domain Wi-Fi When Domain Network is Connected",
                Category = "Wi-Fi Connection Policy",
                Description =
                    "Prevents users from connecting to any non-domain Wi-Fi network while a domain network connection is active, ensuring that domain-joined machines cannot escape corporate network monitoring by switching to a personal hotspot.",
                Tags = ["wi-fi", "domain", "non-domain", "corporate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Non-domain Wi-Fi blocked while domain network is active; machines stay on monitored corporate network.",
                ApplyOps = [RegOp.SetDword(Key, "fBlockNonDomain", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "fBlockNonDomain")],
                DetectOps = [RegOp.CheckDword(Key, "fBlockNonDomain", 1)],
            },
            new TweakDef
            {
                Id = "wificonn-disable-wifi-sense-open",
                Label = "Disable Wi-Fi Sense Connectivity to Open Suggested Hotspots",
                Category = "Wi-Fi Connection Policy",
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
                Category = "Wi-Fi Connection Policy",
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
                Category = "Wi-Fi Connection Policy",
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
                Category = "Wi-Fi Connection Policy",
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
                Category = "Wi-Fi Connection Policy",
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
                Category = "Wi-Fi Connection Policy",
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
                Category = "Wi-Fi Connection Policy",
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
