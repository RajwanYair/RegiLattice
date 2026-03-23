// Network Map Discovery Policy — Sprint 142
// Slug "netlltd" — controls LLTD topology-discovery driver and responder,
// Windows People Near Me (Peernet), and Peer Name Resolution Protocol (PNRP).
//
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LLTD
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Peernet
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerToPeer
#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class NetworkLltdPolicy
{
    private const string Lltd = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LLTD";
    private const string PeerNet = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Peernet";
    private const string PeerToPeer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerToPeer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netlltd-disable-lltdio",
            Label = "LLTD: Disable Link Layer Topology Discovery I/O driver",
            Category = "Network Map Discovery Policy",
            Description =
                "Sets EnableLLTDIO=0 in the LLTD policy key. Prevents the LLTD Mapper I/O driver "
                + "from discovering the network topology used by the Network Map feature in router UIs.",
            Tags = ["network", "lltd", "discovery", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Lltd, "EnableLLTDIO", 0)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "EnableLLTDIO")],
            DetectOps = [RegOp.CheckDword(Lltd, "EnableLLTDIO", 0)],
        },
        new TweakDef
        {
            Id = "netlltd-prohibit-lltdio-private",
            Label = "LLTD: Prohibit LLTD I/O driver on private networks",
            Category = "Network Map Discovery Policy",
            Description =
                "Sets ProhibitLLTDIOOnPrivateNet=1. Prevents the LLTD Mapper I/O driver from operating "
                + "on private network profiles, reducing topology exposure on home networks.",
            Tags = ["network", "lltd", "private", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Lltd, "ProhibitLLTDIOOnPrivateNet", 1)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "ProhibitLLTDIOOnPrivateNet")],
            DetectOps = [RegOp.CheckDword(Lltd, "ProhibitLLTDIOOnPrivateNet", 1)],
        },
        new TweakDef
        {
            Id = "netlltd-no-lltdio-domain",
            Label = "LLTD: Disallow LLTD I/O driver on domain networks",
            Category = "Network Map Discovery Policy",
            Description =
                "Sets AllowLLTDIOOnDomain=0. Blocks the LLTD Mapper I/O driver on domain-joined "
                + "networks, preventing unsanctioned topology mapping in corporate environments.",
            Tags = ["network", "lltd", "domain", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Lltd, "AllowLLTDIOOnDomain", 0)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "AllowLLTDIOOnDomain")],
            DetectOps = [RegOp.CheckDword(Lltd, "AllowLLTDIOOnDomain", 0)],
        },
        new TweakDef
        {
            Id = "netlltd-no-lltdio-public",
            Label = "LLTD: Disallow LLTD I/O driver on public networks",
            Category = "Network Map Discovery Policy",
            Description =
                "Sets AllowLLTDIOOnPublicNet=0. Prevents the LLTD Mapper I/O driver from running on "
                + "public network profiles such as hotel or airport Wi-Fi, reducing attack surface.",
            Tags = ["network", "lltd", "public", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Lltd, "AllowLLTDIOOnPublicNet", 0)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "AllowLLTDIOOnPublicNet")],
            DetectOps = [RegOp.CheckDword(Lltd, "AllowLLTDIOOnPublicNet", 0)],
        },
        new TweakDef
        {
            Id = "netlltd-disable-rspndr",
            Label = "LLTD: Disable Link Layer Topology Discovery Responder",
            Category = "Network Map Discovery Policy",
            Description =
                "Sets EnableRspndr=0 in the LLTD policy key. Disables the LLTD Responder so this "
                + "machine does not appear in other devices' Network Map displays.",
            Tags = ["network", "lltd", "responder", "policy", "stealth"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Lltd, "EnableRspndr", 0)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "EnableRspndr")],
            DetectOps = [RegOp.CheckDword(Lltd, "EnableRspndr", 0)],
        },
        new TweakDef
        {
            Id = "netlltd-prohibit-rspndr-private",
            Label = "LLTD: Prohibit LLTD Responder on private networks",
            Category = "Network Map Discovery Policy",
            Description =
                "Sets ProhibitRspndrOnPrivateNet=1. Stops the LLTD Responder from operating on "
                + "private network profiles, hiding this machine from home network topology maps.",
            Tags = ["network", "lltd", "responder", "private", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Lltd, "ProhibitRspndrOnPrivateNet", 1)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "ProhibitRspndrOnPrivateNet")],
            DetectOps = [RegOp.CheckDword(Lltd, "ProhibitRspndrOnPrivateNet", 1)],
        },
        new TweakDef
        {
            Id = "netlltd-no-rspndr-domain",
            Label = "LLTD: Disallow LLTD Responder on domain networks",
            Category = "Network Map Discovery Policy",
            Description =
                "Sets AllowRspndrOnDomain=0. Prevents the LLTD Responder from operating on domain "
                + "networks, hiding this machine from corporate network map scans.",
            Tags = ["network", "lltd", "responder", "domain", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Lltd, "AllowRspndrOnDomain", 0)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "AllowRspndrOnDomain")],
            DetectOps = [RegOp.CheckDword(Lltd, "AllowRspndrOnDomain", 0)],
        },
        new TweakDef
        {
            Id = "netlltd-no-rspndr-public",
            Label = "LLTD: Disallow LLTD Responder on public networks",
            Category = "Network Map Discovery Policy",
            Description =
                "Sets AllowRspndrOnPublicNet=0. Prevents the LLTD Responder from running on public "
                + "network profiles, hiding this machine from network map scans on public Wi-Fi.",
            Tags = ["network", "lltd", "responder", "public", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Lltd, "AllowRspndrOnPublicNet", 0)],
            RemoveOps = [RegOp.DeleteValue(Lltd, "AllowRspndrOnPublicNet")],
            DetectOps = [RegOp.CheckDword(Lltd, "AllowRspndrOnPublicNet", 0)],
        },
        new TweakDef
        {
            Id = "netlltd-disable-peernet",
            Label = "Disable Windows People Near Me (Peernet) service",
            Category = "Network Map Discovery Policy",
            Description =
                "Sets Disabled=1 in the Peernet policy key. Disables the People Near Me network "
                + "service that discovers nearby contacts over the local network using Windows Collaboration.",
            Tags = ["network", "peernet", "people-near-me", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(PeerNet, "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(PeerNet, "Disabled")],
            DetectOps = [RegOp.CheckDword(PeerNet, "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "netlltd-disable-pnrp",
            Label = "Disable Peer Name Resolution Protocol (PNRP)",
            Category = "Network Map Discovery Policy",
            Description =
                "Sets Disabled=1 in the PeerToPeer policy key. Disables PNRP, the peer-to-peer name "
                + "resolution protocol used for Windows Meeting Space and legacy collaboration features.",
            Tags = ["network", "p2p", "pnrp", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(PeerToPeer, "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(PeerToPeer, "Disabled")],
            DetectOps = [RegOp.CheckDword(PeerToPeer, "Disabled", 1)],
        },
    ];
}
