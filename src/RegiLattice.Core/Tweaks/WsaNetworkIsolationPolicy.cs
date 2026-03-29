// RegiLattice.Core — Tweaks/WsaNetworkIsolationPolicy.cs
// WSA network isolation, firewall rules, and Android container network access controls — Sprint 469.
// Category: "WSA Network Isolation Policy" | Slug: wsanet
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Network

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WsaNetworkIsolationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Network";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wsanet-block-android-internet-access",
                Label = "Block Android Container Internet Access",
                Category = "WSA Network Isolation Policy",
                Description =
                    "Blocks all outbound internet access from the WSA Android container, allowing Android apps to run offline-only without connecting to internet services.",
                Tags = ["wsa", "android", "network", "isolation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Android container offline; all WSA Android app network calls fail. Only local localhost communication permitted.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidInternetAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidInternetAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidInternetAccess", 1)],
            },
            new TweakDef
            {
                Id = "wsanet-block-android-local-network",
                Label = "Block Android Container Local Network Access",
                Category = "WSA Network Isolation Policy",
                Description =
                    "Prevents the WSA Android container from accessing the local area network, stopping Android apps from scanning or communicating with LAN resources and IoT devices.",
                Tags = ["wsa", "android", "lan", "network", "isolation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WSA container LAN access blocked; Android apps cannot reach local network devices.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidLANAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidLANAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidLANAccess", 1)],
            },
            new TweakDef
            {
                Id = "wsanet-disable-android-vpn-client",
                Label = "Disable Android VPN Client within WSA",
                Category = "WSA Network Isolation Policy",
                Description =
                    "Disables the Android VPN API within WSA, preventing Android VPN apps from creating VPN tunnels that could route all Windows traffic through an Android-configured tunnel.",
                Tags = ["wsa", "android", "vpn", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Android VPN API disabled in WSA; Android VPN apps cannot create tunnels or intercept Windows traffic.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAndroidVPNClient", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidVPNClient")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAndroidVPNClient", 1)],
            },
            new TweakDef
            {
                Id = "wsanet-block-android-peer-to-peer",
                Label = "Block Android P2P Wi-Fi Direct in WSA",
                Category = "WSA Network Isolation Policy",
                Description =
                    "Blocks Android Wi-Fi Direct (P2P) in WSA, preventing Android apps from creating ad-hoc Wi-Fi connections to nearby devices that bypass enterprise network controls.",
                Tags = ["wsa", "android", "wifi-direct", "p2p", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Wi-Fi Direct blocked in WSA; Android apps cannot create direct peer connections.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidWifiDirect", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidWifiDirect")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidWifiDirect", 1)],
            },
            new TweakDef
            {
                Id = "wsanet-disable-android-hotspot",
                Label = "Disable Android Mobile Hotspot via WSA",
                Category = "WSA Network Isolation Policy",
                Description =
                    "Disables the ability for Android apps in WSA to activate a Wi-Fi hotspot, preventing an Android app from sharing the Windows internet connection without authorisation.",
                Tags = ["wsa", "android", "hotspot", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Android hotspot creation blocked in WSA; Android tethering apps have no effect.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAndroidHotspot", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidHotspot")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAndroidHotspot", 1)],
            },
            new TweakDef
            {
                Id = "wsanet-restrict-android-dns",
                Label = "Restrict Android Container to Enterprise DNS",
                Category = "WSA Network Isolation Policy",
                Description =
                    "Forces the WSA Android container to use the enterprise DNS servers configured for the Windows host, preventing Android apps from using public or hardcoded DNS resolvers (DNS-over-HTTPS bypass).",
                Tags = ["wsa", "android", "dns", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Android DNS forced to enterprise resolvers; hardcoded Android DNS-over-HTTPS bypassed.",
                ApplyOps = [RegOp.SetDword(Key, "ForceAndroidEnterpriseDNS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceAndroidEnterpriseDNS")],
                DetectOps = [RegOp.CheckDword(Key, "ForceAndroidEnterpriseDNS", 1)],
            },
            new TweakDef
            {
                Id = "wsanet-block-android-nfc",
                Label = "Block NFC Access for Android Apps in WSA",
                Category = "WSA Network Isolation Policy",
                Description =
                    "Blocks Android NFC APIs in WSA, preventing Android apps from accessing the Windows NFC stack if present, and stopping NFC-based contactless payment or data transfer by Android apps.",
                Tags = ["wsa", "android", "nfc", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "NFC access blocked for Android apps in WSA; contactless payment apps and NFC reader apps have no hardware access.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidNFCAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidNFCAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidNFCAccess", 1)],
            },
            new TweakDef
            {
                Id = "wsanet-disable-android-bluetooth",
                Label = "Disable Bluetooth Access for Android Apps in WSA",
                Category = "WSA Network Isolation Policy",
                Description =
                    "Disables Bluetooth access for Android applications in WSA, preventing Android apps from pairing with or communicating via Bluetooth peripherals using the Windows Bluetooth stack.",
                Tags = ["wsa", "android", "bluetooth", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Bluetooth blocked for Android apps in WSA; BT audio/file-transfer Android apps have no hardware access.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAndroidBluetooth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidBluetooth")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAndroidBluetooth", 1)],
            },
            new TweakDef
            {
                Id = "wsanet-block-android-background-data",
                Label = "Block Android Background Data Usage in WSA",
                Category = "WSA Network Isolation Policy",
                Description =
                    "Blocks Android apps in WSA from using network connectivity in the background (when the app screen is not visible), reducing data usage and preventing hidden data exfiltration.",
                Tags = ["wsa", "android", "background-data", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Background Android networking blocked; apps only access network when their window is active.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidBackgroundData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidBackgroundData")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidBackgroundData", 1)],
            },
            new TweakDef
            {
                Id = "wsanet-log-android-network-activity",
                Label = "Enable Audit Logging for Android Network Activity",
                Category = "WSA Network Isolation Policy",
                Description =
                    "Enables event logging for all network connections initiated by Android applications in WSA, providing visibility into Android app network behaviour for security monitoring.",
                Tags = ["wsa", "android", "network", "logging", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Android app network connections logged; outbound connections from WSA visible in event log.",
                ApplyOps = [RegOp.SetDword(Key, "EnableAndroidNetworkAuditLog", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableAndroidNetworkAuditLog")],
                DetectOps = [RegOp.CheckDword(Key, "EnableAndroidNetworkAuditLog", 1)],
            },
        ];
}
