// RegiLattice.Core — Tweaks/NetworkInterface.cs
// Network adapter tuning and TCP/IP stack settings (Sprint 88).
// Slug "nic" — HKLM + HKCU network adapter and TCP/IP optimisation keys.
// Distinct from Network.cs (general), NetworkOptimization.cs (Nagle, QoS), DnsNetworking.cs.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkInterface
{
    private const string TcpIp = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";

    private const string TcpIpPerf = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces";

    private const string AfD = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters";

    private const string LanmanRedirector = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "nic-increase-tcp-receive-window",
            Label = "Increase TCP Receive Window Size (256 KB)",
            Category = "Network Interface",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["nic", "tcp", "receive window", "throughput", "network"],
            Description =
                "Sets the TCP receive window to 262144 bytes (256 KB). Larger windows "
                + "improve throughput on high-bandwidth, high-latency links like fibre "
                + "or VPN tunnels by allowing more in-flight data.",
            ApplyOps = [RegOp.SetDword(TcpIp, "TcpWindowSize", 262144)],
            RemoveOps = [RegOp.DeleteValue(TcpIp, "TcpWindowSize")],
            DetectOps = [RegOp.CheckDword(TcpIp, "TcpWindowSize", 262144)],
        },
        new TweakDef
        {
            Id = "nic-enable-tcp-timestamps",
            Label = "Enable TCP Timestamps (RFC 1323)",
            Category = "Network Interface",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["nic", "tcp", "timestamps", "rfc1323", "rtt"],
            Description =
                "Enables TCP timestamps option (RFC 1323) so the kernel can accurately "
                + "measure RTT and detect lost packets without waiting for a retransmit "
                + "timeout. Improves congestion control on lossy connections.",
            ApplyOps = [RegOp.SetDword(TcpIp, "Tcp1323Opts", 3)],
            RemoveOps = [RegOp.SetDword(TcpIp, "Tcp1323Opts", 0)],
            DetectOps = [RegOp.CheckDword(TcpIp, "Tcp1323Opts", 3)],
        },
        new TweakDef
        {
            Id = "nic-increase-max-syn-retransmissions",
            Label = "Reduce TCP SYN Retransmissions (Faster Port Failure)",
            Category = "Network Interface",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["nic", "tcp", "syn", "retransmit", "latency"],
            Description =
                "Reduces maximum SYN retransmissions from the default 2 to 2, so failed "
                + "TCP connection attempts time out sooner (saves ~6 s per stuck connect). "
                + "Useful for desktops that frequently attempt to connect to unavailable hosts.",
            ApplyOps = [RegOp.SetDword(TcpIp, "TcpMaxConnectRetransmissions", 2)],
            RemoveOps = [RegOp.DeleteValue(TcpIp, "TcpMaxConnectRetransmissions")],
            DetectOps = [RegOp.CheckDword(TcpIp, "TcpMaxConnectRetransmissions", 2)],
        },
        new TweakDef
        {
            Id = "nic-reduce-tcp-timed-wait-delay",
            Label = "Reduce TCP TIME_WAIT Delay to 30 Seconds",
            Category = "Network Interface",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["nic", "tcp", "time wait", "port reuse", "server"],
            Description =
                "Sets TcpTimedWaitDelay to 30 seconds (down from default 60 s). "
                + "Enables faster port recycling for high-throughput servers or clients "
                + "that make large numbers of short TCP connections.",
            ApplyOps = [RegOp.SetDword(TcpIp, "TcpTimedWaitDelay", 30)],
            RemoveOps = [RegOp.DeleteValue(TcpIp, "TcpTimedWaitDelay")],
            DetectOps = [RegOp.CheckDword(TcpIp, "TcpTimedWaitDelay", 30)],
        },
        new TweakDef
        {
            Id = "nic-disable-tcp-chimney-offload",
            Label = "Disable TCP Chimney Offload",
            Category = "Network Interface",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["nic", "tcp", "chimney", "offload", "nic offload"],
            Description =
                "Disables TCP chimney offload, which in practice causes CPU elevation "
                + "issues on some NIC firmware. Keeps TCP processing in the Windows "
                + "network stack where it is more predictable.",
            ApplyOps = [RegOp.SetDword(TcpIp, "EnableTCPChimney", 0)],
            RemoveOps = [RegOp.DeleteValue(TcpIp, "EnableTCPChimney")],
            DetectOps = [RegOp.CheckDword(TcpIp, "EnableTCPChimney", 0)],
        },
        new TweakDef
        {
            Id = "nic-enable-rss",
            Label = "Enable Receive-Side Scaling (RSS)",
            Category = "Network Interface",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["nic", "rss", "receive side scaling", "multicore", "throughput"],
            Description =
                "Enables Receive-Side Scaling so incoming NIC interrupts are distributed "
                + "across multiple CPU cores. Significantly improves multi-stream network "
                + "throughput on multi-core systems with supported NICs.",
            ApplyOps = [RegOp.SetDword(TcpIp, "EnableRSS", 1)],
            RemoveOps = [RegOp.DeleteValue(TcpIp, "EnableRSS")],
            DetectOps = [RegOp.CheckDword(TcpIp, "EnableRSS", 1)],
        },
        new TweakDef
        {
            Id = "nic-increase-afd-send-backlog",
            Label = "Increase AFD Socket Send Backlog (High-Throughput Servers)",
            Category = "Network Interface",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["nic", "afd", "socket", "backlog", "server"],
            Description =
                "Increases the AFD socket send buffer maximum to allow apps to post "
                + "large writes without blocking. DefaultSendWindow=262144 (256 KB). "
                + "Benefits high-throughput servers and file transfer applications.",
            ApplyOps = [RegOp.SetDword(AfD, "DefaultSendWindow", 262144)],
            RemoveOps = [RegOp.DeleteValue(AfD, "DefaultSendWindow")],
            DetectOps = [RegOp.CheckDword(AfD, "DefaultSendWindow", 262144)],
        },
        new TweakDef
        {
            Id = "nic-increase-afd-receive-backlog",
            Label = "Increase AFD Socket Receive Backlog (High-Throughput Servers)",
            Category = "Network Interface",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["nic", "afd", "socket", "receive", "backlog"],
            Description =
                "Increases the AFD default receive window to 262144 bytes so the kernel "
                + "buffers more incoming data per socket. Reduces packet loss under burst "
                + "traffic from fast remote hosts.",
            ApplyOps = [RegOp.SetDword(AfD, "DefaultReceiveWindow", 262144)],
            RemoveOps = [RegOp.DeleteValue(AfD, "DefaultReceiveWindow")],
            DetectOps = [RegOp.CheckDword(AfD, "DefaultReceiveWindow", 262144)],
        },
        new TweakDef
        {
            Id = "nic-enable-ctcp",
            Label = "Enable Compound TCP (CTCP) Congestion Control",
            Category = "Network Interface",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["nic", "ctcp", "congestion control", "tcp", "throughput"],
            Description =
                "Enables Compound TCP congestion control algorithm, which increases the "
                + "receive window more aggressively on high-bandwidth, high-latency links "
                + "(e.g. intercontinental). Comparable to Linux's Cubic. 1 = enabled.",
            ApplyOps = [RegOp.SetDword(TcpIp, "EnableCTCP", 1)],
            RemoveOps = [RegOp.DeleteValue(TcpIp, "EnableCTCP")],
            DetectOps = [RegOp.CheckDword(TcpIp, "EnableCTCP", 1)],
        },
        new TweakDef
        {
            Id = "nic-reduce-max-port-range",
            Label = "Increase Dynamic Port Range (High-Connection Apps)",
            Category = "Network Interface",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["nic", "port range", "ephemeral", "connections"],
            Description =
                "Expands the ephemeral port pool by raising MaxUserPort to 65534 "
                + "(from default 5000). Prevents 'address already in use' errors on "
                + "systems that establish thousands of concurrent short-lived connections.",
            ApplyOps = [RegOp.SetDword(TcpIp, "MaxUserPort", 65534)],
            RemoveOps = [RegOp.DeleteValue(TcpIp, "MaxUserPort")],
            DetectOps = [RegOp.CheckDword(TcpIp, "MaxUserPort", 65534)],
        },
    ];
}
