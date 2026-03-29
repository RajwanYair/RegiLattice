// RegiLattice.Core — Tweaks/NetworkBridgePolicy.cs
// Windows network bridge creation and bridging restriction policy — Sprint 497.
// Category: "Network Bridge Policy" | Slug: netbridge
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Network Connections

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkBridgePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "netbridge-prohibit-bridge-installation",
                Label = "Prohibit Installation of Network Bridges",
                Category = "Network Bridge Policy",
                Description =
                    "Prevents users from creating network bridge connections that combine multiple network adapters into a single bridged segment, closing the route by which a user could bypass network segmentation controls.",
                Tags = ["network-bridge", "segmentation", "security", "adapter", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Network bridge creation prohibited; users cannot bridge two network adapters to bypass segmentation.",
                ApplyOps = [RegOp.SetDword(Key, "NC_AllowNetBridge_NLA", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "NC_AllowNetBridge_NLA")],
                DetectOps = [RegOp.CheckDword(Key, "NC_AllowNetBridge_NLA", 0)],
            },
            new TweakDef
            {
                Id = "netbridge-prohibit-ics",
                Label = "Prohibit Internet Connection Sharing",
                Category = "Network Bridge Policy",
                Description =
                    "Disables Windows Internet Connection Sharing (ICS) on all connections, preventing a machine from acting as an ad-hoc NAT gateway and creating an unmonitored network egress path for other devices.",
                Tags = ["ics", "internet-sharing", "nat", "network-policy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "ICS prohibited; this machine cannot share its internet connection with other devices.",
                ApplyOps = [RegOp.SetDword(Key, "NC_ShowSharedAccessUI", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "NC_ShowSharedAccessUI")],
                DetectOps = [RegOp.CheckDword(Key, "NC_ShowSharedAccessUI", 0)],
            },
            new TweakDef
            {
                Id = "netbridge-prohibit-personal-hotspot",
                Label = "Prohibit Windows Mobile Hotspot (Personal Hotspot)",
                Category = "Network Bridge Policy",
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
                Id = "netbridge-block-add-remove-connections",
                Label = "Block Standard Users from Adding or Removing Network Connections",
                Category = "Network Bridge Policy",
                Description =
                    "Prevents non-administrator users from creating new network connections or deleting existing ones, ensuring that network adapter configuration is managed exclusively by IT administrators.",
                Tags = ["network-connections", "standard-user", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Standard users blocked from adding or deleting network connections; admin required.",
                ApplyOps = [RegOp.SetDword(Key, "NC_AddRemoveComponents", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "NC_AddRemoveComponents")],
                DetectOps = [RegOp.CheckDword(Key, "NC_AddRemoveComponents", 0)],
            },
            new TweakDef
            {
                Id = "netbridge-disable-advanced-settings",
                Label = "Block Standard Users from Accessing Network Advanced Settings",
                Category = "Network Bridge Policy",
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
                Category = "Network Bridge Policy",
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
                Id = "netbridge-block-ras-connection-manager",
                Label = "Block Access to the Remote Access Connection Manager",
                Category = "Network Bridge Policy",
                Description =
                    "Prevents non-administrator users from using the Remote Access Connection Manager to create, modify, or delete remote access (dial-up, VPN) connection entries.",
                Tags = ["ras", "remote-access", "vpn", "dial-up", "standard-user", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Remote Access Connection Manager access blocked for standard users; VPN entries protected.",
                ApplyOps = [RegOp.SetDword(Key, "NC_RasMyProperties", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "NC_RasMyProperties")],
                DetectOps = [RegOp.CheckDword(Key, "NC_RasMyProperties", 0)],
            },
            new TweakDef
            {
                Id = "netbridge-hide-lan-component-properties",
                Label = "Hide Properties of LAN Network Components from Standard Users",
                Category = "Network Bridge Policy",
                Description =
                    "Removes the ability for standard users to view or modify the properties of LAN adapter components (TCP/IP, DNS, WINS bindings) from the Network Connections folder, preventing unauthorised IP configuration changes.",
                Tags = ["lan", "network-properties", "tcp-ip", "standard-user", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "LAN adapter component properties hidden for standard users; IP/DNS configuration protected.",
                ApplyOps = [RegOp.SetDword(Key, "NC_LanProperties", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "NC_LanProperties")],
                DetectOps = [RegOp.CheckDword(Key, "NC_LanProperties", 0)],
            },
            new TweakDef
            {
                Id = "netbridge-notify-on-connection-change",
                Label = "Show Notification on Network Connection Status Change",
                Category = "Network Bridge Policy",
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
                Category = "Network Bridge Policy",
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
