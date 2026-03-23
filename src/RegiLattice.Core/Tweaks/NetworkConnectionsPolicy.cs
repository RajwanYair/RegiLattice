// RegiLattice.Core — Tweaks/NetworkConnectionsPolicy.cs
// Network Connections Group Policy restrictions (Sprint 129, T8.2).
// Slug "netconn" — HKLM\SOFTWARE\Policies\Microsoft\Windows\Network Connections.
// Controls whether standard users can create, delete, configure, or share network connections.
// Distinct from Firewall.cs (packet rules), Network.cs (adapter settings), SmbNetworking.cs.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkConnectionsPolicy
{
    private const string Pol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netconn-enable-admin-prohibits",
            Label = "Honour Admin-Prohibited Network Connection Actions",
            Category = "Network Connections Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["network", "connections", "policy", "admin", "security"],
            Description =
                "Sets NC_EnableAdminProhibits=1 which activates all administrator prohibition policies "
                + "in the Network Connections folder. This MUST be enabled before other NC_ restrictions "
                + "take effect. Without it, all other netconn policies are silently ignored.",
            ApplyOps = [RegOp.SetDword(Pol, "NC_EnableAdminProhibits", 1)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NC_EnableAdminProhibits")],
            DetectOps = [RegOp.CheckDword(Pol, "NC_EnableAdminProhibits", 1)],
        },
        new TweakDef
        {
            Id = "netconn-prevent-bridge",
            Label = "Prevent Users from Creating Network Bridges",
            Category = "Network Connections Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["network", "bridge", "security", "policy", "connections"],
            Description =
                "Prohibits standard users from creating network bridges (NC_AllowNetBridge_NLA=0). "
                + "Network bridges can bypass firewall segmentation and create unexpected routing paths. "
                + "Essential in corporate environments where VLANs provide network isolation.",
            ApplyOps = [RegOp.SetDword(Pol, "NC_AllowNetBridge_NLA", 0)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NC_AllowNetBridge_NLA")],
            DetectOps = [RegOp.CheckDword(Pol, "NC_AllowNetBridge_NLA", 0)],
        },
        new TweakDef
        {
            Id = "netconn-prevent-add-remove-components",
            Label = "Prevent Adding or Removing Network Components",
            Category = "Network Connections Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["network", "components", "policy", "security", "install"],
            Description =
                "Prevents standard users from installing or uninstalling network protocols, clients, "
                + "and services via the adapter Properties dialog (NC_AddRemoveComponents=0). Stops "
                + "installation of rogue protocol drivers or removal of security-critical components.",
            ApplyOps = [RegOp.SetDword(Pol, "NC_AddRemoveComponents", 0)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NC_AddRemoveComponents")],
            DetectOps = [RegOp.CheckDword(Pol, "NC_AddRemoveComponents", 0)],
        },
        new TweakDef
        {
            Id = "netconn-prevent-change-binding",
            Label = "Prevent Changing Network Component Binding Order",
            Category = "Network Connections Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["network", "binding", "policy", "security", "components"],
            Description =
                "Prevents standard users from enabling, disabling, or reordering network protocol "
                + "bindings (NC_ChangeBindingState=0). Stops users from disabling security protocol "
                + "bindings like SMB signing or 802.1X authentication on network adapters.",
            ApplyOps = [RegOp.SetDword(Pol, "NC_ChangeBindingState", 0)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NC_ChangeBindingState")],
            DetectOps = [RegOp.CheckDword(Pol, "NC_ChangeBindingState", 0)],
        },
        new TweakDef
        {
            Id = "netconn-prevent-delete-all-user",
            Label = "Prevent Deleting All-User VPN/Dial-Up Connections",
            Category = "Network Connections Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["network", "vpn", "dialup", "connections", "policy"],
            Description =
                "Prevents standard users from deleting network connections that are available to all "
                + "users (NC_DeleteAllUserConnection=0). Protects corporate VPN profiles and dial-up "
                + "connections created by IT from accidental or malicious removal.",
            ApplyOps = [RegOp.SetDword(Pol, "NC_DeleteAllUserConnection", 0)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NC_DeleteAllUserConnection")],
            DetectOps = [RegOp.CheckDword(Pol, "NC_DeleteAllUserConnection", 0)],
        },
        new TweakDef
        {
            Id = "netconn-prevent-lan-access-properties",
            Label = "Prevent Accessing LAN Adapter Properties",
            Category = "Network Connections Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["network", "lan", "properties", "adapter", "policy"],
            Description =
                "Prevents standard users from opening the Properties dialog for LAN (Ethernet) "
                + "connections (NC_LanProperties=0). Stops modification of IP address/DNS settings, "
                + "protocol binding, and adapter configuration on corporate workstations.",
            ApplyOps = [RegOp.SetDword(Pol, "NC_LanProperties", 0)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NC_LanProperties")],
            DetectOps = [RegOp.CheckDword(Pol, "NC_LanProperties", 0)],
        },
        new TweakDef
        {
            Id = "netconn-prevent-ras-connect",
            Label = "Prevent Standard Users Connecting VPN/Dial-Up",
            Category = "Network Connections Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["network", "vpn", "dialup", "connect", "policy"],
            Description =
                "Prevents standard users from connecting to dial-up or VPN connections (NC_RasConnect=0). "
                + "Useful where VPN access should be controlled by IT policy, preventing users from "
                + "connecting to unauthorised or personal VPN endpoints.",
            ApplyOps = [RegOp.SetDword(Pol, "NC_RasConnect", 0)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NC_RasConnect")],
            DetectOps = [RegOp.CheckDword(Pol, "NC_RasConnect", 0)],
        },
        new TweakDef
        {
            Id = "netconn-prevent-ras-all-user-properties",
            Label = "Prevent Viewing All-User VPN Connection Properties",
            Category = "Network Connections Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["network", "vpn", "properties", "all-user", "policy"],
            Description =
                "Prevents standard users from viewing or editing the properties of VPN/dial-up "
                + "connections created for all users (NC_RasAllUserProperties=0). Hides server "
                + "addresses, credentials, and routing configuration from non-admin users.",
            ApplyOps = [RegOp.SetDword(Pol, "NC_RasAllUserProperties", 0)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NC_RasAllUserProperties")],
            DetectOps = [RegOp.CheckDword(Pol, "NC_RasAllUserProperties", 0)],
        },
        new TweakDef
        {
            Id = "netconn-prohibit-internet-connection-sharing",
            Label = "Prohibit Internet Connection Sharing (ICS)",
            Category = "Network Connections Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["network", "ics", "sharing", "hotspot", "policy"],
            Description =
                "Prohibits enabling Internet Connection Sharing via the Network Connections folder "
                + "(NC_ShowSharedAccessUI=0). Prevents workstations from acting as unauthorised NAT "
                + "gateways which could bypass network security controls and create rogue access points.",
            ApplyOps = [RegOp.SetDword(Pol, "NC_ShowSharedAccessUI", 0)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NC_ShowSharedAccessUI")],
            DetectOps = [RegOp.CheckDword(Pol, "NC_ShowSharedAccessUI", 0)],
        },
        new TweakDef
        {
            Id = "netconn-prevent-rename-connections",
            Label = "Prevent Users from Renaming Network Connections",
            Category = "Network Connections Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["network", "rename", "connections", "policy", "hardening"],
            Description =
                "Prevents standard users from renaming network connections via the Network Connections "
                + "folder (NC_RenameConnection=0). Stops confusion and social engineering attempts that "
                + "rely on renaming connections to impersonate corporate VPN profiles or legitimate adapters.",
            ApplyOps = [RegOp.SetDword(Pol, "NC_RenameConnection", 0)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NC_RenameConnection")],
            DetectOps = [RegOp.CheckDword(Pol, "NC_RenameConnection", 0)],
        },
    ];
}
