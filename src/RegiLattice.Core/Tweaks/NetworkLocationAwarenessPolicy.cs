// RegiLattice.Core — Tweaks/NetworkLocationAwarenessPolicy.cs
// Network Location Awareness (NCSI/NCA) privacy and security hardening — Sprint 439.
// Disables active probing, Microsoft connectivity tests, hotspot detection, and IPv6
// checks; enforces corporate DNS probing, domain detection, and blocks location switching.
// Category: "Network Location Awareness Policy" | Slug: nlapol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkLocationAwarenessPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "nlapol-disable-active-probing",
                Label = "Disable NCSI Active Network Probing",
                Category = "Network Location Awareness Policy",
                Description =
                    "Sets NoActiveProbe=1 to disable the Network Connectivity Status Indicator active probe that contacts Microsoft servers (msftconnecttest.com). Stops phone-home traffic on every network connect.",
                Tags = ["ncsi", "probe", "privacy", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Stops NCSI phone-home probes; network status indicator may show 'No internet' on some scenarios.",
                ApplyOps = [RegOp.SetDword(Key, "NoActiveProbe", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoActiveProbe")],
                DetectOps = [RegOp.CheckDword(Key, "NoActiveProbe", 1)],
            },
            new TweakDef
            {
                Id = "nlapol-disable-ms-connectivity-test",
                Label = "Disable Microsoft Connectivity Test",
                Category = "Network Location Awareness Policy",
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
                Category = "Network Location Awareness Policy",
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
                Category = "Network Location Awareness Policy",
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
                Category = "Network Location Awareness Policy",
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
                Category = "Network Location Awareness Policy",
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
                Category = "Network Location Awareness Policy",
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
                Category = "Network Location Awareness Policy",
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
                Category = "Network Location Awareness Policy",
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
                Category = "Network Location Awareness Policy",
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
