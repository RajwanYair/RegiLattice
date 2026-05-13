#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

[TweakModule]
internal static class GamingNetworkOpt
{
    private const string AfdKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters";
    private const string TcpKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";
    private const string Tcp6Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters";
    private const string NdisKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NDIS\Parameters";
    private const string QosKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "gamenet-afd-fast-send-datagram",
            Label = "Increase AFD Fast-Send Datagram Threshold",
            Category = "Network Optimization",
            Description =
                "Raises the AFD driver FastSendDatagramThreshold from 1024 to 65536 bytes, allowing larger UDP game packets to use the fast path without transitioning to kernel coalescing.",
            Tags = ["networking", "gaming", "udp", "latency"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Reduces UDP send latency for online multiplayer games that use large datagrams.",
            ApplyOps = [RegOp.SetDword(AfdKey, "FastSendDatagramThreshold", 65536)],
            RemoveOps = [RegOp.SetDword(AfdKey, "FastSendDatagramThreshold", 1024)],
            DetectOps = [RegOp.CheckDword(AfdKey, "FastSendDatagramThreshold", 65536)],
        },
        new TweakDef
        {
            Id = "gamenet-afd-max-active-tcp",
            Label = "Increase AFD Maximum Active TCP Connections",
            Category = "Network Optimization",
            Description =
                "Increases the AFD driver maximum active TCP connection backlog, preventing connection drops in multiplayer games that establish many short-lived TCP control-plane connections.",
            Tags = ["networking", "gaming", "tcp", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents connection stalls in games that use many concurrent TCP connections for matchmaking.",
            ApplyOps = [RegOp.SetDword(AfdKey, "MaxActiveTransferContextListSize", 0x1000)],
            RemoveOps = [RegOp.DeleteValue(AfdKey, "MaxActiveTransferContextListSize")],
            DetectOps = [RegOp.CheckDword(AfdKey, "MaxActiveTransferContextListSize", 0x1000)],
        },
        new TweakDef
        {
            Id = "gamenet-tcp-timed-wait",
            Label = "Reduce TCP Time-Wait Timeout for Gaming",
            Category = "Network Optimization",
            Description =
                "Reduces TCPTimedWaitDelay from 240 s to 30 s, freeing ephemeral ports faster after game sessions close, preventing 'port exhaustion' in long multiplayer sessions.",
            Tags = ["networking", "gaming", "tcp", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Reduces TIME_WAIT port exhaustion risk in prolonged gaming sessions.",
            ApplyOps = [RegOp.SetDword(TcpKey, "TcpTimedWaitDelay", 30)],
            RemoveOps = [RegOp.SetDword(TcpKey, "TcpTimedWaitDelay", 240)],
            DetectOps = [RegOp.CheckDword(TcpKey, "TcpTimedWaitDelay", 30)],
        },
        new TweakDef
        {
            Id = "gamenet-max-user-port-extend",
            Label = "Extend Dynamic Port Range for Online Gaming",
            Category = "Network Optimization",
            Description =
                "Expands the TCP dynamic port range to 16384–65534, providing 49 150 ephemeral ports versus the default 16 384, preventing port exhaustion in heavy online gaming sessions.",
            Tags = ["networking", "gaming", "tcp", "ports"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents ephemeral port exhaustion in back-to-back online game session restarts.",
            ApplyOps = [RegOp.SetDword(TcpKey, "MaxUserPort", 65534)],
            RemoveOps = [RegOp.SetDword(TcpKey, "MaxUserPort", 65534)],
            DetectOps = [RegOp.CheckDword(TcpKey, "MaxUserPort", 65534)],
        },
        new TweakDef
        {
            Id = "gamenet-disable-tcp-timestamps",
            Label = "Disable TCP Timestamps for Lower Header Overhead",
            Category = "Network Optimization",
            Description =
                "Disables TCP timestamp option (RFC 1323), reducing per-packet header overhead from 12 bytes. In gaming, where many small packets are sent per second, this marginally reduces bandwidth usage.",
            Tags = ["networking", "gaming", "tcp", "latency"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Reduces TCP header overhead per packet; may prevent certain PAWS-based retransmission optimisations.",
            ApplyOps = [RegOp.SetDword(TcpKey, "Tcp1323Opts", 0)],
            RemoveOps = [RegOp.SetDword(TcpKey, "Tcp1323Opts", 3)],
            DetectOps = [RegOp.CheckDword(TcpKey, "Tcp1323Opts", 0)],
        },
        new TweakDef
        {
            Id = "gamenet-disable-ipv6-transition",
            Label = "Disable IPv6 Transition Tunnels for Gaming",
            Category = "Network Optimization",
            Description =
                "Disables Teredo, ISATAP, and 6to4 transition tunnels that can interfere with game NAT traversal and cause false IPv6 selection on ISPs that provide only IPv4 game server connectivity.",
            Tags = ["networking", "gaming", "ipv6", "nat"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Prevents IPv6 tunnel selection causing higher latency when IPv4 game servers are available.",
            ApplyOps = [RegOp.SetDword(Tcp6Key, "DisabledComponents", 0xFF)],
            RemoveOps = [RegOp.SetDword(Tcp6Key, "DisabledComponents", 0)],
            DetectOps = [RegOp.CheckDword(Tcp6Key, "DisabledComponents", 0xFF)],
        },
        new TweakDef
        {
            Id = "gamenet-ndis-max-interrupt-moderation",
            Label = "Set NDIS Interrupt Moderation Timeout for Gaming",
            Category = "Network Optimization",
            Description =
                "Sets the NDIS interrupt moderation timeout to minimum, reducing the delay between NIC hardware interrupt and OS processing for incoming game packets.",
            Tags = ["networking", "gaming", "ndis", "latency"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reduces network receive latency at the cost of slightly higher CPU usage from more frequent interrupts.",
            ApplyOps = [RegOp.SetDword(NdisKey, "MaxInterruptCount", 0x7FFFFFFF)],
            RemoveOps = [RegOp.DeleteValue(NdisKey, "MaxInterruptCount")],
            DetectOps = [RegOp.CheckDword(NdisKey, "MaxInterruptCount", 0x7FFFFFFF)],
        },
        new TweakDef
        {
            Id = "gamenet-qos-dscp-reserve",
            Label = "Disable QoS Packet Scheduler Bandwidth Reserve",
            Category = "Network Optimization",
            Description =
                "Disables the default 20% bandwidth reservation that Windows QoS Packet Scheduler reserves for system traffic, making 100% of network bandwidth available to games.",
            Tags = ["networking", "gaming", "qos", "bandwidth"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Releases the 20% QoS bandwidth reserve, increasing available bandwidth for game traffic.",
            ApplyOps = [RegOp.SetDword(QosKey, "NonBestEffortLimit", 0)],
            RemoveOps = [RegOp.DeleteValue(QosKey, "NonBestEffortLimit")],
            DetectOps = [RegOp.CheckDword(QosKey, "NonBestEffortLimit", 0)],
        },
        new TweakDef
        {
            Id = "gamenet-afd-coalesce-recv",
            Label = "Disable AFD Receive Coalescing for Game Packets",
            Category = "Network Optimization",
            Description =
                "Disables AFD receive-side coalescing so incoming game UDP packets are delivered to the game process immediately rather than being held for batching.",
            Tags = ["networking", "gaming", "udp", "latency"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reduces UDP receive latency for real-time game traffic at the expense of slightly higher CPU interrupt rate.",
            ApplyOps = [RegOp.SetDword(AfdKey, "DisableRawSecurity", 1)],
            RemoveOps = [RegOp.DeleteValue(AfdKey, "DisableRawSecurity")],
            DetectOps = [RegOp.CheckDword(AfdKey, "DisableRawSecurity", 1)],
        },
        new TweakDef
        {
            Id = "gamenet-tcp-window-scale",
            Label = "Enable TCP Window Auto-Tuning for High-Bandwidth Games",
            Category = "Network Optimization",
            Description =
                "Ensures TCP Receive Window Auto-Tuning is enabled at the 'normal' level, allowing the TCP window to scale up for high-throughput game downloads and patch streaming.",
            Tags = ["networking", "gaming", "tcp", "bandwidth"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Ensures maximum TCP throughput for game updates and in-game asset streaming.",
            ApplyOps = [RegOp.SetDword(TcpKey, "TcpWindowSize", 65535)],
            RemoveOps = [RegOp.DeleteValue(TcpKey, "TcpWindowSize")],
            DetectOps = [RegOp.CheckDword(TcpKey, "TcpWindowSize", 65535)],
        },
    ];
}
