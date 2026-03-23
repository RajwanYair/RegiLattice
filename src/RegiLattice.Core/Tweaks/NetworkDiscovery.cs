// RegiLattice.Core — Tweaks/NetworkDiscovery.cs
// Network discovery protocol policies: LLTD, mDNS, SSDP, UPnP, FDResPub (Sprint 137).
// Slug "netdisc" — LLTD, Dnscache mDNS, NetBT, and related discovery service policies.
// Complements SmbNetworking.cs (SMB) and Hardening.cs (harden-disable-llmnr/NullSession).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkDiscovery
{
    private const string Lltd = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LLTD";
    private const string Dns = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters";
    private const string NetBt = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netdisc-disable-lltd-mapper",
            Label = "Disable LLTD Mapper I/O Driver",
            Category = "Network Discovery",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Disables the Link Layer Topology Discovery (LLTD) Mapper I/O driver, "
                + "preventing this machine from being mapped in the Windows Network Map. "
                + "EnableLLTDIO=0.",
            Tags = ["network discovery", "lltd", "map", "privacy", "security"],
            RegistryKeys = [Lltd],
            ApplyOps = [RegOp.SetDword(Lltd, "EnableLLTDIO", 0)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "EnableLLTDIO")],
            DetectOps = [RegOp.CheckDword(Lltd, "EnableLLTDIO", 0)],
        },
        new TweakDef
        {
            Id = "netdisc-disable-lltd-on-domain",
            Label = "Disable LLTD I/O on Domain Networks",
            Category = "Network Discovery",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description = "Prevents the LLTD Mapper I/O driver from running on domain-joined network " + "segments. AllowLLTDIOOnDomain=0.",
            Tags = ["network discovery", "lltd", "domain", "security"],
            RegistryKeys = [Lltd],
            ApplyOps = [RegOp.SetDword(Lltd, "AllowLLTDIOOnDomain", 0)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "AllowLLTDIOOnDomain")],
            DetectOps = [RegOp.CheckDword(Lltd, "AllowLLTDIOOnDomain", 0)],
        },
        new TweakDef
        {
            Id = "netdisc-disable-lltd-on-public",
            Label = "Disable LLTD I/O on Public Networks",
            Category = "Network Discovery",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description = "Prevents LLTD topology discovery on public / untrusted networks. " + "AllowLLTDIOOnPublicNet=0.",
            Tags = ["network discovery", "lltd", "public network", "security"],
            RegistryKeys = [Lltd],
            ApplyOps = [RegOp.SetDword(Lltd, "AllowLLTDIOOnPublicNet", 0)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "AllowLLTDIOOnPublicNet")],
            DetectOps = [RegOp.CheckDword(Lltd, "AllowLLTDIOOnPublicNet", 0)],
        },
        new TweakDef
        {
            Id = "netdisc-disable-lltd-responder",
            Label = "Disable LLTD Responder",
            Category = "Network Discovery",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Disables the LLTD Responder so this machine does not respond to network "
                + "topology discovery probes from other devices on the LAN. EnableRspndr=0.",
            Tags = ["network discovery", "lltd", "responder", "privacy"],
            RegistryKeys = [Lltd],
            ApplyOps = [RegOp.SetDword(Lltd, "EnableRspndr", 0)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "EnableRspndr")],
            DetectOps = [RegOp.CheckDword(Lltd, "EnableRspndr", 0)],
        },
        new TweakDef
        {
            Id = "netdisc-disable-lltd-responder-domain",
            Label = "Disable LLTD Responder on Domain Networks",
            Category = "Network Discovery",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description = "Prevents the LLTD Responder from answering topology probes on domain " + "network segments. AllowRspndrOnDomain=0.",
            Tags = ["network discovery", "lltd", "responder", "domain"],
            RegistryKeys = [Lltd],
            ApplyOps = [RegOp.SetDword(Lltd, "AllowRspndrOnDomain", 0)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "AllowRspndrOnDomain")],
            DetectOps = [RegOp.CheckDword(Lltd, "AllowRspndrOnDomain", 0)],
        },
        new TweakDef
        {
            Id = "netdisc-disable-mdns",
            Label = "Disable Multicast DNS (mDNS)",
            Category = "Network Discovery",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Disables the mDNS resolver in the Windows DNS Client. mDNS broadcasts "
                + "device names and can reveal information on local networks. EnableMDNS=0.",
            Tags = ["network discovery", "mdns", "multicast", "dns", "privacy"],
            RegistryKeys = [Dns],
            ApplyOps = [RegOp.SetDword(Dns, "EnableMDNS", 0)],
            RemoveOps = [RegOp.DeleteValue(Dns, "EnableMDNS")],
            DetectOps = [RegOp.CheckDword(Dns, "EnableMDNS", 0)],
        },
        new TweakDef
        {
            Id = "netdisc-netbios-pnode",
            Label = "Configure NetBIOS to P-Node (Unicast Only)",
            Category = "Network Discovery",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets NetBIOS name resolution to P-Node (Point-to-Point), which only uses "
                + "WINS/DNS unicast queries and does not broadcast name requests on the "
                + "local subnet. NodeType=2 reduces LAN broadcast traffic.",
            Tags = ["netbios", "broadcast", "wins", "name resolution", "network"],
            RegistryKeys = [NetBt],
            ApplyOps = [RegOp.SetDword(NetBt, "NodeType", 2)],
            RemoveOps = [RegOp.DeleteValue(NetBt, "NodeType")],
            DetectOps = [RegOp.CheckDword(NetBt, "NodeType", 2)],
        },
        new TweakDef
        {
            Id = "netdisc-disable-upnp-host-svc",
            Label = "Disable UPnP Host Service",
            Category = "Network Discovery",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Disables the UPnP Device Host service (upnphost), which allows the "
                + "machine to be discovered and controlled via Universal Plug and Play. "
                + "Start=4 (Disabled).",
            Tags = ["upnp", "network discovery", "service", "security"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 4)],
        },
        new TweakDef
        {
            Id = "netdisc-disable-ssdp-svc",
            Label = "Disable SSDP Discovery Service",
            Category = "Network Discovery",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Disables the SSDP Discovery service (SSDPSRV), which provides Simple "
                + "Service Discovery Protocol announcements. Prevents UPnP device enumeration. "
                + "Start=4 (Disabled).",
            Tags = ["ssdp", "upnp", "network discovery", "service"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SSDPSRV", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SSDPSRV", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SSDPSRV", "Start", 4)],
        },
        new TweakDef
        {
            Id = "netdisc-disable-fdrespub-svc",
            Label = "Disable Function Discovery Resource Publication",
            Category = "Network Discovery",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Disables the Function Discovery Resource Publication service (FDResPub), "
                + "which makes this computer discoverable on the network for file/printer "
                + "sharing. Start=4 (Disabled).",
            Tags = ["network discovery", "fdrespub", "sharing", "service"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 4)],
        },
    ];
}
