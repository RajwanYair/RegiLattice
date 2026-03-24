// RegiLattice.Core — Tweaks/NetworkConnectStatusPolicy.cs
// Network Connectivity Status Indicator (NCSI) machine-scope GPO controls — Sprint 204.
// Controls NCSI probing behaviour, captive portal detection, and network status display.
// Category: "Network Connectivity Status Policy" | Slug: ncsi
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectStatusIndicator

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkConnectStatusPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectStatusIndicator";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ncsi-disable-active-probing",
                Label = "Disable NCSI Active Probing (Privacy)",
                Category = "Network Connectivity Status Policy",
                Description =
                    "Prevents Windows from sending HTTP probes to www.msftconnecttest.com to determine internet connectivity. Eliminates Microsoft telemetry connections from the network stack. Default: probing enabled. Recommended: 1 for privacy-focused environments.",
                Tags = ["ncsi", "probing", "privacy", "telemetry", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Stop&Go: the system tray network icon may show 'No Internet' even with connectivity; captive portal detection disabled.",
                ApplyOps = [RegOp.SetDword(Key, "NoActiveProbe", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoActiveProbe")],
                DetectOps = [RegOp.CheckDword(Key, "NoActiveProbe", 1)],
            },
            new TweakDef
            {
                Id = "ncsi-disable-global-dns-probe",
                Label = "Disable NCSI Global DNS Probe",
                Category = "Network Connectivity Status Policy",
                Description =
                    "Suppresses the DNS lookup probe to dns.msftncsi.com that NCSI uses to verify connectivity. Reduces DNS traffic to Microsoft servers. Default: probe enabled. Recommended: 1 for hardened/air-gapped environments.",
                Tags = ["ncsi", "dns", "probe", "privacy", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "DNS probe to Microsoft suppressed; may cause 'No Internet' indication in system tray on valid connections.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePassivePolling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePassivePolling")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePassivePolling", 1)],
            },
            new TweakDef
            {
                Id = "ncsi-disable-captive-portal-detection",
                Label = "Disable Captive Portal Browser Launch",
                Category = "Network Connectivity Status Policy",
                Description =
                    "Prevents Windows from automatically launching a browser window when a captive portal (hotel/airport Wi-Fi) is detected. Reduces unsolicited network connections and popup browsing windows. Default: enabled. Recommended: 1 for corporate laptops.",
                Tags = ["ncsi", "captive-portal", "browser", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Captive portal browser popup is suppressed; users must manually open a browser to log into hotel/airport Wi-Fi.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCaptivePortalDetection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCaptivePortalDetection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCaptivePortalDetection", 1)],
            },
            new TweakDef
            {
                Id = "ncsi-use-corporate-probe-host",
                Label = "Use Corporate Custom Probe Host",
                Category = "Network Connectivity Status Policy",
                Description =
                    "Enables NCSI to probe an internal corporate server instead of Microsoft's public endpoint. Allows NCSI to correctly report connectivity status on air-gapped or corporate networks. Default: not set. Recommended: 1 (then configure custom host separately).",
                Tags = ["ncsi", "corporate", "intranet", "probe", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NCSI will use the custom probe host instead of Microsoft; set the probe host/path via companion registry values.",
                ApplyOps = [RegOp.SetDword(Key, "EnableCorporateProbe", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableCorporateProbe")],
                DetectOps = [RegOp.CheckDword(Key, "EnableCorporateProbe", 1)],
            },
            new TweakDef
            {
                Id = "ncsi-disable-ipv6-probe",
                Label = "Disable NCSI IPv6 Probe",
                Category = "Network Connectivity Status Policy",
                Description =
                    "Prevents the NCSI IPv6 connectivity probe to ipv6.msftconnecttest.com. Reduces unsolicited outbound IPv6 traffic to Microsoft. Default: probe enabled. Recommended: 1 if IPv6 is not in use.",
                Tags = ["ncsi", "ipv6", "probe", "privacy", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "IPv6 NCSI probe suppressed; IPv6 connectivity indicator in system tray may be inaccurate.",
                ApplyOps = [RegOp.SetDword(Key, "DisableIPv6Probe", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIPv6Probe")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIPv6Probe", 1)],
            },
            new TweakDef
            {
                Id = "ncsi-disable-internet-access-check",
                Label = "Disable System-Wide Internet Access Check",
                Category = "Network Connectivity Status Policy",
                Description =
                    "Suppresses the periodic system-level NCSI check that determines whether the machine has internet access. Useful on dedicated intranet-only systems. Default: check enabled. Recommended: 1 for air-gapped environments.",
                Tags = ["ncsi", "internet-check", "intranet", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "No internet access check—apps relying on NCSI connectivity state may behave incorrectly.",
                ApplyOps = [RegOp.SetDword(Key, "DisableInternetAccessCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInternetAccessCheck")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInternetAccessCheck", 1)],
            },
            new TweakDef
            {
                Id = "ncsi-hide-network-icon-status",
                Label = "Hide NCSI Status in System Tray Tooltip",
                Category = "Network Connectivity Status Policy",
                Description =
                    "Removes the 'No Internet Access' or 'No connectivity' tooltip from the network system tray icon. Reduces user confusion on corporate networks that filter NCSI. Default: shown. Recommended: 1 on managed networks.",
                Tags = ["ncsi", "tray", "notification", "network", "usability", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Network tray icon tooltip no longer shows 'No Internet Access' on filtered corporate networks.",
                ApplyOps = [RegOp.SetDword(Key, "HideNoInternetWarning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HideNoInternetWarning")],
                DetectOps = [RegOp.CheckDword(Key, "HideNoInternetWarning", 1)],
            },
            new TweakDef
            {
                Id = "ncsi-require-corporate-connectivity",
                Label = "Require Corporate Network for NCSI 'Connected' Status",
                Category = "Network Connectivity Status Policy",
                Description =
                    "Configures NCSI to only report 'Connected to Internet' when the device can also reach the corporate intranet probe host. Ensures the network indicator reflects both intranet and internet connectivity. Default: not configured.",
                Tags = ["ncsi", "corporate", "connectivity", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "NCSI shows 'connected' only when both internet and corporate network are reachable.",
                ApplyOps = [RegOp.SetDword(Key, "RequireCorporateConnectivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireCorporateConnectivity")],
                DetectOps = [RegOp.CheckDword(Key, "RequireCorporateConnectivity", 1)],
            },
            new TweakDef
            {
                Id = "ncsi-probe-retry-3",
                Label = "Set NCSI Probe Retry Count to 3",
                Category = "Network Connectivity Status Policy",
                Description =
                    "Limits the number of NCSI probe retries to 3 before declaring no connectivity. Reduces network congestion from repeated probing on slow or lossy links. Default: 5. Recommended: 3.",
                Tags = ["ncsi", "probe", "retry", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "NCSI retries probes 3 times before showing 'No Internet'; faster failure detection on broken links.",
                ApplyOps = [RegOp.SetDword(Key, "MaxProbeRetryCount", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxProbeRetryCount")],
                DetectOps = [RegOp.CheckDword(Key, "MaxProbeRetryCount", 3)],
            },
            new TweakDef
            {
                Id = "ncsi-log-probe-failures",
                Label = "Enable NCSI Probe Failure Event Logging",
                Category = "Network Connectivity Status Policy",
                Description =
                    "Enables audit logging of NCSI probe failures to the Windows Event Log. Useful for diagnosing connectivity issues on managed endpoints. Default: disabled. Recommended: 1 for monitored environments.",
                Tags = ["ncsi", "logging", "audit", "network", "diagnostics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "NCSI probe failures are written to the Event Log; minor I/O overhead.",
                ApplyOps = [RegOp.SetDword(Key, "EnableProbeFailureLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableProbeFailureLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableProbeFailureLogging", 1)],
            },
        ];
}
