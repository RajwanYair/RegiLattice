// RegiLattice.Core — Tweaks/WcmWifiPolicy.cs
// Windows Connection Manager (WCM) Wi-Fi connection management GPO controls — Sprint 206.
// Controls automatic Wi-Fi switching, connection minimization, and adapter policies.
// Category: "Wireless Connection Manager Policy" | Slug: wcmpol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCM

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WcmWifiPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCM";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wcmpol-disable-soft-disconnect",
                Label = "Disable WCM Soft-Disconnect from Wired",
                Category = "Wireless Connection Manager Policy",
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
                Category = "Wireless Connection Manager Policy",
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
                Category = "Wireless Connection Manager Policy",
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
                Category = "Wireless Connection Manager Policy",
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
                Category = "Wireless Connection Manager Policy",
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
                Category = "Wireless Connection Manager Policy",
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
                Category = "Wireless Connection Manager Policy",
                Description =
                    "Prevents domain-joined machines from connecting to non-domain (public/home) networks while connected to the corporate domain network. Stops traffic leakage to unmanaged networks. Default: not restricted. Recommended: 1 on domain endpoints.",
                Tags = ["wcm", "domain", "network", "security", "corporate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Domain-joined machines cannot join public/home Wi-Fi while on the corporate network; strong defence against bridging attacks.",
                ApplyOps = [RegOp.SetDword(Key, "BlockNonDomainNetworks", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockNonDomainNetworks")],
                DetectOps = [RegOp.CheckDword(Key, "BlockNonDomainNetworks", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-auto-select-network-profile",
                Label = "Disable Auto-Selection of Network Profile on Connect",
                Category = "Wireless Connection Manager Policy",
                Description =
                    "Prevents WCM from automatically selecting the best network profile (Public/Private/Domain) upon connection. Requires users to explicitly choose the profile, reducing risk of miscategorising corporate networks as Public. Default: auto-select enabled.",
                Tags = ["wcm", "network-profile", "public", "private", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Network profile is not auto-assigned; reduces risk of corporate network being set to Public with open file sharing.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoNetworkProfileSelection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoNetworkProfileSelection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoNetworkProfileSelection", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-set-polling-interval-60s",
                Label = "Set WCM Connection Polling Interval to 60 Seconds",
                Category = "Wireless Connection Manager Policy",
                Description =
                    "Adjusts the WCM service polling interval for connectivity changes to 60 seconds. Reduces WCM CPU wakeups on battery-powered laptops without significantly delaying reconnection. Default: ~5 seconds. Recommended: 60 for battery savings.",
                Tags = ["wcm", "polling", "battery", "performance", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WCM polls every 60s instead of ~5s; reduces wakeups and battery drain; reconnect after network switch takes up to 60s.",
                ApplyOps = [RegOp.SetDword(Key, "ConnectionPollingIntervalSec", 60)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConnectionPollingIntervalSec")],
                DetectOps = [RegOp.CheckDword(Key, "ConnectionPollingIntervalSec", 60)],
            },
            new TweakDef
            {
                Id = "wcmpol-disable-managed-wifi-offload",
                Label = "Disable WCM Managed Wi-Fi Offload",
                Category = "Wireless Connection Manager Policy",
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
