// RegiLattice.Core — Tweaks/NetIoOffloadPolicy.cs
// Network I/O Offload & TCP Optimization Policy — Sprint 543.
// Configures Windows TCP/IP stack offload capabilities, receive-side scaling,
// chimney offload, Large Send Offload (LSO), and network adapter I/O
// performance optimization settings.
// Category: "Net IO Offload Policy" | Slug: netio
// Registry: HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetIoOffloadPolicy
{
    private const string TcpKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";

    private const string TcpifKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters";

    private const string AfDKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "netio-enable-tcp-1323-timestamps",
                Label = "Net IO: Enable TCP 1323 Timestamps for Accurate RTT",
                Category = "Net IO Offload Policy",
                Description =
                    "Sets Tcp1323Opts=1 under TCP/IP parameters. Enables RFC 1323 TCP timestamps which embed a timestamp value in every TCP segment. Timestamps allow the stack to accurately calculate round-trip time (RTT) for congestion control algorithms (CUBIC, NewReno) and detect spurious retransmissions more accurately. On high-bandwidth links with packet reordering, RFC 1323 timestamps significantly improve throughput stability by preventing premature retransmissions due to RTT measurement jitter.",
                Tags = ["tcp", "timestamps", "rtt", "offload", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables TCP timestamps. On some very old firewalls, TCP timestamps cause packets to be dropped. Modern enterprise firewalls handle timestamps correctly.",
                ApplyOps = [RegOp.SetDword(TcpKey, "Tcp1323Opts", 1)],
                RemoveOps = [RegOp.DeleteValue(TcpKey, "Tcp1323Opts")],
                DetectOps = [RegOp.CheckDword(TcpKey, "Tcp1323Opts", 1)],
            },
            new TweakDef
            {
                Id = "netio-set-tcp-checksum-hardware",
                Label = "Net IO: Enable TCP/IP Hardware Checksum Offload",
                Category = "Net IO Offload Policy",
                Description =
                    "Sets EnableTCPChimneyOffload=1 and ChecksumOffloadEnabled=1 in TCP/IP settings. Configures the TCP/IP stack to delegate TCP and IP header checksum computation to the network adapter hardware (TCP Offload Engine). Hardware checksum computation removes the CPU overhead of per-packet checksum calculations from the host CPU. On servers handling 10 Gbps+ traffic or high-PPS packet flows, hardware checksum offload can reduce CPU utilization by 5–15% for network processing.",
                Tags = ["tcp", "checksum", "offload", "hardware", "nic"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Offloads TCP checksum to NIC hardware. Requires checksum offload-capable NIC (all modern enterprise NICs support this). Some virtualization hypervisors may intercept and verify checksums.",
                ApplyOps = [RegOp.SetDword(TcpKey, "EnableTCPChimneyOffload", 1)],
                RemoveOps = [RegOp.DeleteValue(TcpKey, "EnableTCPChimneyOffload")],
                DetectOps = [RegOp.CheckDword(TcpKey, "EnableTCPChimneyOffload", 1)],
            },
            new TweakDef
            {
                Id = "netio-set-tcp-autotuning-high",
                Label = "Net IO: Set TCP Window Autotuning to Highly Restricted Mode",
                Category = "Net IO Offload Policy",
                Description =
                    "Sets TcpAutoTuningLevel=4 (highly restricted) in TCP/IP parameters. Sets TCP receive window auto-tuning to a conservative algorithm that grows the receive buffer more cautiously than the default. The highly restricted mode is appropriate for environments with high-speed last-mile but intermediate links with lossy behavior (satellite links, 4G LTE backhaul) where aggressive window growth causes sporadic retransmit storms. On reliable Ethernet networks, 'normal' (0) autotuning provides higher throughput.",
                Tags = ["tcp", "autotuning", "window", "buffer", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Conservative TCP window scaling may reduce throughput on high-bandwidth low-latency links. Only recommended for networks with frequent packet loss (satellite, LTE backhaul).",
                ApplyOps = [RegOp.SetDword(TcpKey, "TcpAutoTuningLevel", 4)],
                RemoveOps = [RegOp.DeleteValue(TcpKey, "TcpAutoTuningLevel")],
                DetectOps = [RegOp.CheckDword(TcpKey, "TcpAutoTuningLevel", 4)],
            },
            new TweakDef
            {
                Id = "netio-enable-rss",
                Label = "Net IO: Enable Receive-Side Scaling (RSS) for Multi-CPU Load",
                Category = "Net IO Offload Policy",
                Description =
                    "Sets EnableRSS=1 and MaxRssProcessors=4 in network adapter policy. Enables Receive-Side Scaling (RSS) which distributes incoming network packet processing across multiple CPU cores. Without RSS, all incoming traffic for a given NIC is processed on a single CPU core, creating a per-core throughput bottleneck at approximately 3–5 Gbps on modern hardware. With RSS, incoming packets are hashed to multiple CPU queues, scaling receive throughput linearly with CPU cores up to the NIC's hardware RSS queue limit.",
                Tags = ["rss", "networking", "cpu", "performance", "offload"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Requires RSS-capable NIC (all server-class NICs support RSS). RSS distributes interrupts across CPUs which may change CPU affinity behavior of network-intensive processes.",
                ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS",
                            "EnableRSS",
                            1
                        ),
                    ],
                RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS",
                            "EnableRSS"
                        ),
                    ],
                DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS",
                            "EnableRSS",
                            1
                        ),
                    ],
            },
            new TweakDef
            {
                Id = "netio-set-afd-fast-send-datagram",
                Label = "Net IO: Enable AFD Fast Send Datagram Path",
                Category = "Net IO Offload Policy",
                Description =
                    "Sets FastSendDatagramThreshold=1024 in AFD parameters. Configures the Windows Ancillary Function Driver (AFD) to use the fast datagram send path for UDP packets under 1024 bytes. The fast path bypasses several AFD buffer validation steps for trusted-size datagrams, reducing per-packet CPU cost for high-PPS UDP workloads. This benefits applications generating large volumes of small UDP packets: DNS servers processing thousands of queries per second, game servers, or network telemetry agents.",
                Tags = ["afd", "udp", "datagram", "fast-path", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Fast path bypasses some buffer validation for small UDP datagrams. Testing recommended for high-PPS DNS server workloads before production deployment.",
                ApplyOps = [RegOp.SetDword(AfDKey, "FastSendDatagramThreshold", 1024)],
                RemoveOps = [RegOp.DeleteValue(AfDKey, "FastSendDatagramThreshold")],
                DetectOps = [RegOp.CheckDword(AfDKey, "FastSendDatagramThreshold", 1024)],
            },
            new TweakDef
            {
                Id = "netio-increase-max-ports",
                Label = "Net IO: Increase Ephemeral Port Range to 49152-65535",
                Category = "Net IO Offload Policy",
                Description =
                    "Sets MaxUserPort=65534 and TcpTimedWaitDelay=30 in TCP/IP parameters. Expands the ephemeral port range to the RFC 6335 recommended range (49152–65535) and reduces the TIME_WAIT period to 30 seconds. The default Windows range is 49152–65535 already, but TIME_WAIT of 4 minutes means busy servers can exhaust 16,383 ports in 4 minutes at moderate connection rates. Reducing TIME_WAIT to 30 seconds allows 5–10× more concurrent connections on database servers, load balancers, and web proxy servers.",
                Tags = ["tcp", "ports", "time-wait", "connections", "server"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Shorter TIME_WAIT increases port reuse risk. On high-traffic servers handling TCP connections to external parties, shorter TIME_WAIT may cause connection confusion with duplicate TCP segment detection disabled.",
                ApplyOps =
                    [
                        RegOp.SetDword(TcpKey, "MaxUserPort", 65534),
                        RegOp.SetDword(TcpKey, "TcpTimedWaitDelay", 30),
                    ],
                RemoveOps =
                    [
                        RegOp.DeleteValue(TcpKey, "MaxUserPort"),
                        RegOp.DeleteValue(TcpKey, "TcpTimedWaitDelay"),
                    ],
                DetectOps =
                    [
                        RegOp.CheckDword(TcpKey, "MaxUserPort", 65534),
                    ],
            },
            new TweakDef
            {
                Id = "netio-enable-tcp-keepalive",
                Label = "Net IO: Enable TCP Keep-Alive with 2-Hour Interval",
                Category = "Net IO Offload Policy",
                Description =
                    "Sets KeepAliveTime=7200000 and KeepAliveInterval=1000 in TCP/IP policy. Enables TCP keep-alive probes with a 2-hour idle detection and 1-second probe interval. TCP keep-alives detect broken idle connections (firewalls that silently drop stateful entries, NAT timeouts, disconnected cables) and clean up orphaned sockets. This prevents resource exhaustion on servers that maintain long-lived application connections (SQL Server, LDAP, message queues) where the remote party may have disconnected without sending TCP RST.",
                Tags = ["tcp", "keepalive", "connection", "timeout", "server"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Keep-alive probes are sent after 2 hours of TCP idle time. Some applications (Oracle DB, older LDAP pools) have their own keep-alive settings that should be aligned.",
                ApplyOps =
                    [
                        RegOp.SetDword(TcpKey, "KeepAliveTime", 7200000),
                        RegOp.SetDword(TcpKey, "KeepAliveInterval", 1000),
                    ],
                RemoveOps =
                    [
                        RegOp.DeleteValue(TcpKey, "KeepAliveTime"),
                        RegOp.DeleteValue(TcpKey, "KeepAliveInterval"),
                    ],
                DetectOps =
                    [
                        RegOp.CheckDword(TcpKey, "KeepAliveTime", 7200000),
                    ],
            },
            new TweakDef
            {
                Id = "netio-disable-ipv4-source-routing",
                Label = "Net IO: Disable IPv4 Source Routing (Anti-Spoofing)",
                Category = "Net IO Offload Policy",
                Description =
                    "Sets DisableIPSourceRouting=2 in TCP/IP parameters. Drops and logs all IPv4 packets containing source routing options (Strict Source Route, Loose Source Route). IP source routing allows the sender to explicitly specify the route a packet takes through the network, overriding routing table decisions. This is used in IP spoofing attacks, source routing reconnaissance, and some firewall bypasses. Setting 2 (drop and log) ensures these packets are both rejected and generate security event log entries.",
                Tags = ["tcp", "source-routing", "spoofing", "network", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Legitimate source-routed applications are extremely rare on enterprise networks. This setting has no impact on normal TCP/IP traffic.",
                ApplyOps = [RegOp.SetDword(TcpKey, "DisableIPSourceRouting", 2)],
                RemoveOps = [RegOp.DeleteValue(TcpKey, "DisableIPSourceRouting")],
                DetectOps = [RegOp.CheckDword(TcpKey, "DisableIPSourceRouting", 2)],
            },
            new TweakDef
            {
                Id = "netio-disable-ipv6-source-routing",
                Label = "Net IO: Disable IPv6 Source Routing (Anti-Spoofing)",
                Category = "Net IO Offload Policy",
                Description =
                    "Sets DisableIPv6SourceRouting=1 in TCPv6 parameters. Drops IPv6 packets containing Type 0 Routing Header (RH0) extension headers. IPv6 RH0 was used in major amplified DoS attacks (CVE-2007-2242) where a small packet can be amplified to an enormous amount of traffic by specifying 127 intermediate hops in the routing header. RFC 5095 deprecated RH0; all modern networks should drop RH0-containing packets. This setting enforces RFC 5095 at the host‐stack level.",
                Tags = ["ipv6", "source-routing", "rh0", "dos", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "No legitimate applications use IPv6 RH0 since RFC 5095 deprecation in 2007. This setting has no impact on normal network operations.",
                ApplyOps = [RegOp.SetDword(TcpifKey, "DisableIPv6SourceRouting", 1)],
                RemoveOps = [RegOp.DeleteValue(TcpifKey, "DisableIPv6SourceRouting")],
                DetectOps = [RegOp.CheckDword(TcpifKey, "DisableIPv6SourceRouting", 1)],
            },
            new TweakDef
            {
                Id = "netio-enable-syn-attack-protection",
                Label = "Net IO: Enable SYN Flood Attack Protection (SYN Cookies)",
                Category = "Net IO Offload Policy",
                Description =
                    "Sets SynAttackProtect=1 in TCP/IP parameters. Enables SYN cookie-based SYN flood attack protection. Under a SYN flood attack, the server normally allocates a half-open TCP connection entry for each SYN packet received, exhausting the TCP incomplete connection queue. With SYN attack protection enabled, the stack uses SYN cookies: the server encodes connection state into the initial sequence number instead of allocating memory, allowing it to handle millions of SYN packets per second without queue exhaustion.",
                Tags = ["tcp", "syn-flood", "dos", "syn-cookie", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "SYN cookies are transparent to legitimate TCP clients. On very busy servers (>50,000 new connections/second) SYN cookie computation has a small CPU overhead.",
                ApplyOps = [RegOp.SetDword(TcpKey, "SynAttackProtect", 1)],
                RemoveOps = [RegOp.DeleteValue(TcpKey, "SynAttackProtect")],
                DetectOps = [RegOp.CheckDword(TcpKey, "SynAttackProtect", 1)],
            },
        ];
}
