// RegiLattice.Core — Tweaks/NetworkMonitoringPolicy.cs
// Network Monitoring & Diagnostics Policy — Sprint 546.
// Configures Group Policy for Windows network diagnostic and monitoring features
// including Network Diagnostics Framework (NDF), network tracing, helper
// diagnostics, and event logging for network performance monitoring.
// Category: "Network Monitoring Policy" | Slug: netmon
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\NetworkDiagnostics

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkMonitoringPolicy
{
    private const string NdfKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkDiagnostics";

    private const string DiagKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Diagnostics";

    private const string WdiKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Diagnostics\Networking";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "netmon-disable-ndf-online-repair",
                Label = "Network Monitoring: Disable Network Diagnostic Online Auto-Repair",
                Category = "Network Monitoring Policy",
                Description =
                    "Sets DontDisplayNetworkSelectionUI=1 in Network Diagnostics policy. Prevents the Windows Network Diagnostics Framework (NDF) from automatically connecting to Microsoft's online diagnostics service to retrieve updated diagnostic helpers and repair scripts. In enterprise environments, connectivity to external Microsoft diagnostic endpoints should be controlled centrally (via proxy allow-list), not triggered automatically by user-initiated troubleshooting dialogs. Online repair also leaks network configuration details to Microsoft.",
                Tags = ["netmon", "ndf", "diagnostics", "online", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NDF uses only local diagnostic helpers (no online repair retrieval). Administrators should ensure local NDF helpers are kept up-to-date via Windows Update.",
                ApplyOps = [RegOp.SetDword(NdfKey, "DontDisplayNetworkSelectionUI", 1)],
                RemoveOps = [RegOp.DeleteValue(NdfKey, "DontDisplayNetworkSelectionUI")],
                DetectOps = [RegOp.CheckDword(NdfKey, "DontDisplayNetworkSelectionUI", 1)],
            },
            new TweakDef
            {
                Id = "netmon-enable-network-event-logging",
                Label = "Network Monitoring: Enable Verbose Network Event Logging",
                Category = "Network Monitoring Policy",
                Description =
                    "Sets NetworkEventLogging=1 in Network Diagnostics policy. Enables verbose network event logging which writes detailed network adapter state changes, DHCP lease events, IP address changes, and connectivity state transitions to the Windows event log (Source: Microsoft-Windows-NetworkProfile, Microsoft-Windows-NCSI). This log data is essential for correlating network problems with application failures and for SIEM-based network anomaly detection (unusual IP changes, frequent adapter resets).",
                Tags = ["netmon", "event-log", "network", "dhcp", "diagnostics"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Generates additional network event log entries. Ensure event log sizing is sufficient. Events are forwarded via WEF/WEC to SIEM for enterprise monitoring.",
                ApplyOps = [RegOp.SetDword(NdfKey, "NetworkEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(NdfKey, "NetworkEventLogging")],
                DetectOps = [RegOp.CheckDword(NdfKey, "NetworkEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "netmon-enable-ndis-trace",
                Label = "Network Monitoring: Enable NDIS Driver Trace Collection",
                Category = "Network Monitoring Policy",
                Description =
                    "Sets EnableNdisTrace=1 in Diagnostics policy. Enables Network Driver Interface Specification (NDIS) trace logging which captures driver-level packet events, miniport state changes, and power management events for network adapters. NDIS traces provide the lowest-level view of network adapter behavior, including driver errors and power state transitions that cause intermittent connectivity. These traces are collected by Windows Diagnostic Infrastructure (WDI) and submitted when network diagnostic scans are run.",
                Tags = ["netmon", "ndis", "trace", "network-driver", "diagnostics"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "NDIS trace logging is lightweight (ring buffer). Trace data is only written to disk when a diagnostic scan is triggered. No continuous disk I/O from this setting.",
                ApplyOps = [RegOp.SetDword(DiagKey, "EnableNdisTrace", 1)],
                RemoveOps = [RegOp.DeleteValue(DiagKey, "EnableNdisTrace")],
                DetectOps = [RegOp.CheckDword(DiagKey, "EnableNdisTrace", 1)],
            },
            new TweakDef
            {
                Id = "netmon-disable-autoplay-on-network",
                Label = "Network Monitoring: Disable AutoPlay for Network-Mapped Drives",
                Category = "Network Monitoring Policy",
                Description =
                    "Sets NoAutoPlayOnNetworkShares=1 in Network Monitoring/Diagnostics policy. Prevents Windows AutoPlay from executing autorun.inf on network-mapped drives. Network drive AutoPlay was the primary propagation vector for network worms that planted autorun.inf files on open shares. Even though AutoPlay on user machines may be disabled by other policies, explicitly blocking AutoPlay on network shares ensures that malicious autorun.inf placed on a file server by an attacker cannot trigger automatic execution on connecting clients.",
                Tags = ["netmon", "autoplay", "network-share", "worm", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "AutoPlay behavior on USB and optical discs is controlled separately. This policy only affects network shares.",
                ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer",
                            "NoAutoPlayOnNetworkShares",
                            1
                        ),
                    ],
                RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer",
                            "NoAutoPlayOnNetworkShares"
                        ),
                    ],
                DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer",
                            "NoAutoPlayOnNetworkShares",
                            1
                        ),
                    ],
            },
            new TweakDef
            {
                Id = "netmon-enable-connectivity-probe",
                Label = "Network Monitoring: Enable Corporate Connectivity Probe",
                Category = "Network Monitoring Policy",
                Description =
                    "Sets EnableConnectivityProbing=1 in Network Diagnostics. Configures Windows to continuously probe an internal IT-managed connectivity check endpoint (corporate NCSI probe server) to track network connectivity quality. Connectivity probe failures generate event log events that allow SIEM and IT monitoring tools to detect network infrastructure failures, DNS outages, and proxy unavailability in real time across the managed endpoint fleet before users report issues.",
                Tags = ["netmon", "probe", "connectivity", "siem", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Requires a corporate NCSI probe server to be configured (typically an internal web server). Generates HTTP probes every 30 seconds per adapter.",
                ApplyOps = [RegOp.SetDword(NdfKey, "EnableConnectivityProbing", 1)],
                RemoveOps = [RegOp.DeleteValue(NdfKey, "EnableConnectivityProbing")],
                DetectOps = [RegOp.CheckDword(NdfKey, "EnableConnectivityProbing", 1)],
            },
            new TweakDef
            {
                Id = "netmon-enable-pktmon",
                Label = "Network Monitoring: Enable Packet Monitor (PktMon) Trace",
                Category = "Network Monitoring Policy",
                Description =
                    "Sets PktMonEnabled=1 in Diagnostics policy. Enables access to the Windows Packet Monitor (pktmon) built-in network sniffer for diagnostic purposes. pktmon is a kernel-mode packet capture built into Windows Server 2019 and Windows 10 1809+. This policy enables the capture component for use by network administrators running 'pktmon start' diagnostics. Without this policy, pktmon requires administrator elevation which is already implied; this setting enables the functionality for diagnostic scripts.",
                Tags = ["netmon", "pktmon", "packet-capture", "diagnostics", "admin"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Enables pktmon diagnostic capture access. No continuous packet capture occurs; captures are initiated manually or by diagnostic scripts.",
                ApplyOps = [RegOp.SetDword(DiagKey, "PktMonEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(DiagKey, "PktMonEnabled")],
                DetectOps = [RegOp.CheckDword(DiagKey, "PktMonEnabled", 1)],
            },
            new TweakDef
            {
                Id = "netmon-disable-network-location-wizard",
                Label = "Network Monitoring: Disable Network Location Wizard Popup",
                Category = "Network Monitoring Policy",
                Description =
                    "Sets NC_ShowSharedAccessUI=0 and DomainNetworkFirewallProfile=1 in network policy. Suppresses the 'Set Network Location' wizard that prompts users to classify a new network as Public, Private, or Domain. In a corporate environment, network location should be set automatically via domain detection (domain-joined machines that can reach the DC use the Domain profile). User-facing prompts to classify networks can result in corporate network infrastructure being classified as Public (overly restrictive) or Private (insufficiently restrictive).",
                Tags = ["netmon", "network-location", "wizard", "firewall-profile", "ui"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Network classification prompt is suppressed. Domain-joined machines auto-detect domain networks. Non-domain-joined VMs should have network location set via script.",
                ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections",
                            "NC_ShowSharedAccessUI",
                            0
                        ),
                    ],
                RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections",
                            "NC_ShowSharedAccessUI"
                        ),
                    ],
                DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections",
                            "NC_ShowSharedAccessUI",
                            0
                        ),
                    ],
            },
            new TweakDef
            {
                Id = "netmon-enable-wdi-net-diagnostics",
                Label = "Network Monitoring: Enable WDI Network Diagnostics Service",
                Category = "Network Monitoring Policy",
                Description =
                    "Sets WdiNetDiagEnabled=1 in WDI Diagnostics settings. Enables the Windows Diagnostic Infrastructure (WDI) network diagnostics scenario which collects lightweight ambient network performance data when connectivity problems occur. WDI triggers automatic trace collection when network degradation is detected (packet loss >5%, latency spikes, DNS resolution delays) and saves diagnostic logs to %SystemRoot%\\diagnostics. These logs are critical for helpdesk troubleshooting remote endpoint network issues.",
                Tags = ["netmon", "wdi", "diagnostics", "trace", "helpdesk"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WDI diagnostics collect lightweight ambient traces. Trace collection is triggered by degradation events, not continuously. Trace files are local and require helpdesk access to collect.",
                ApplyOps = [RegOp.SetDword(WdiKey, "WdiNetDiagEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WdiKey, "WdiNetDiagEnabled")],
                DetectOps = [RegOp.CheckDword(WdiKey, "WdiNetDiagEnabled", 1)],
            },
            new TweakDef
            {
                Id = "netmon-enable-smb-connection-audit",
                Label = "Network Monitoring: Enable SMB Access Audit Logging",
                Category = "Network Monitoring Policy",
                Description =
                    "Sets AuditSmb=1 in Network Monitoring policy. Enables auditing of SMB (Server Message Block) file access events, generating Windows Security event log entries for file share connections, access attempts, and share mount/unmount. SMB access audit is a core requirement for detecting lateral movement: attackers using pass-the-hash, pass-the-ticket, or network share enumeration tools (Impacket, CrackMapExec) generate distinctive SMB access patterns that appear in audit logs.",
                Tags = ["netmon", "smb", "audit", "logging", "lateral-movement"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "SMB access audit generates Security event log entries for every file share connection. Ensure event log capacity and SIEM egress are sized for the additional volume on file servers.",
                ApplyOps = [RegOp.SetDword(NdfKey, "AuditSmb", 1)],
                RemoveOps = [RegOp.DeleteValue(NdfKey, "AuditSmb")],
                DetectOps = [RegOp.CheckDword(NdfKey, "AuditSmb", 1)],
            },
            new TweakDef
            {
                Id = "netmon-set-connection-limit-per-host",
                Label = "Network Monitoring: Limit Simultaneous Connections Per Server",
                Category = "Network Monitoring Policy",
                Description =
                    "Sets WinHttpConnectionLimit=10 in WinHTTP settings. Limits the number of simultaneous HTTP connections per server to 10. The default limit of 64 per server allows aggressive web scrapers, update agents, and backup agents to open hundreds of simultaneous connections to a single server, potentially degrading server performance. A limit of 10 concurrent connections per client-server pair is sufficient for modern application workloads and prevents per-machine connection flooding.",
                Tags = ["netmon", "connection-limit", "http", "performance", "server"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Simultaneous connections per destination are capped at 10. High-volume download agents (BITS, WSUS) use their own connection limits and may be unaffected.",
                ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings",
                            "MaxConnectionsPerServer",
                            10
                        ),
                    ],
                RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings",
                            "MaxConnectionsPerServer"
                        ),
                    ],
                DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings",
                            "MaxConnectionsPerServer",
                            10
                        ),
                    ],
            },
        ];
}
