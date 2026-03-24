#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class WcmConnectionPolicy
{
    private const string Wcm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wcmpol-disable-auto-connect",
            Label = "Disable WCM Auto-Connect to Non-Internet Networks",
            Category = "Windows Connection Manager Policy",
            Description = "Disables Windows Connection Manager automatic connection to networks when already connected to internet. Prevents unexpected Wi-Fi/mobile broadband connections that could create dual-homed exposure. Default: 0. Recommended: 1.",
            Tags = ["connection-manager", "network", "auto-connect", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Wcm],
            ApplyOps   = [RegOp.SetDword(Wcm, "fDisableAutoConnect", 1)],
            RemoveOps  = [RegOp.DeleteValue(Wcm, "fDisableAutoConnect")],
            DetectOps  = [RegOp.CheckDword(Wcm, "fDisableAutoConnect", 1)],
        },
        new TweakDef
        {
            Id = "wcmpol-minimize-connections",
            Label = "Minimize Simultaneous WCM Connections",
            Category = "Windows Connection Manager Policy",
            Description = "Instructs Windows Connection Manager to minimize the number of simultaneous connections to the internet, a domain, or a network. Prevents multi-homing unless required. Default: 0. Recommended: 3 (minimize, but allow manual overrides).",
            Tags = ["connection-manager", "network", "multi-home"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Wcm],
            ApplyOps   = [RegOp.SetDword(Wcm, "fMinimizeConnections", 3)],
            RemoveOps  = [RegOp.DeleteValue(Wcm, "fMinimizeConnections")],
            DetectOps  = [RegOp.CheckDword(Wcm, "fMinimizeConnections", 3)],
        },
        new TweakDef
        {
            Id = "wcmpol-block-non-domain",
            Label = "Block WCM Connections to Non-Domain Networks",
            Category = "Windows Connection Manager Policy",
            Description = "Blocks Windows Connection Manager from connecting to non-domain networks when the machine is domain-joined and internet is available via domain network. Reduces attack surface from untrusted Wi-Fi. Default: 0. Recommended: 1 for corporate.",
            Tags = ["connection-manager", "network", "domain", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Wcm],
            ApplyOps   = [RegOp.SetDword(Wcm, "fBlockNonDomain", 1)],
            RemoveOps  = [RegOp.DeleteValue(Wcm, "fBlockNonDomain")],
            DetectOps  = [RegOp.CheckDword(Wcm, "fBlockNonDomain", 1)],
        },
        new TweakDef
        {
            Id = "wcmpol-prefer-wired-network",
            Label = "Prefer Wired over Wireless in WCM",
            Category = "Windows Connection Manager Policy",
            Description = "Instructs Windows Connection Manager to prefer wired Ethernet connections over Wi-Fi when both are available. Improves stability and throughput without forcing disconnect from Wi-Fi. Default: 0. Recommended: 1.",
            Tags = ["connection-manager", "network", "wired", "wifi"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Wcm],
            ApplyOps   = [RegOp.SetDword(Wcm, "fPreferWiredNetwork", 1)],
            RemoveOps  = [RegOp.DeleteValue(Wcm, "fPreferWiredNetwork")],
            DetectOps  = [RegOp.CheckDword(Wcm, "fPreferWiredNetwork", 1)],
        },
        new TweakDef
        {
            Id = "wcmpol-soft-disconnect",
            Label = "Enable WCM Soft Disconnect on Wireless",
            Category = "Windows Connection Manager Policy",
            Description = "Enables soft-disconnect behavior in WCM: instead of immediately dropping a wireless connection, the system waits for applications to switch before disconnecting. Reduces connection-drop disruptions. Default: 0. Recommended: 1.",
            Tags = ["connection-manager", "network", "wifi", "disconnect"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Wcm],
            ApplyOps   = [RegOp.SetDword(Wcm, "fSoftDisconnectConnections", 1)],
            RemoveOps  = [RegOp.DeleteValue(Wcm, "fSoftDisconnectConnections")],
            DetectOps  = [RegOp.CheckDword(Wcm, "fSoftDisconnectConnections", 1)],
        },
        new TweakDef
        {
            Id = "wcmpol-disable-wlan-connectivity",
            Label = "Disable WLAN Connectivity via WCM Policy",
            Category = "Windows Connection Manager Policy",
            Description = "Disables WLAN (Wi-Fi) connections through the Windows Connection Manager policy. For wired-only or air-gapped workstations where wireless should be locked out at policy level. Default: 0. Recommended: 1 for restricted machines.",
            Tags = ["connection-manager", "wifi", "disable", "security", "wlan"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Wcm],
            ApplyOps   = [RegOp.SetDword(Wcm, "fDisableWlanConnectivity", 1)],
            RemoveOps  = [RegOp.DeleteValue(Wcm, "fDisableWlanConnectivity")],
            DetectOps  = [RegOp.CheckDword(Wcm, "fDisableWlanConnectivity", 1)],
        },
        new TweakDef
        {
            Id = "wcmpol-disable-wwan-connectivity",
            Label = "Disable WWAN/Mobile Broadband via WCM Policy",
            Category = "Windows Connection Manager Policy",
            Description = "Disables WWAN (mobile broadband/cellular) connections through the Windows Connection Manager policy. Prevents unexpected cellular data charges on enterprise devices. Default: 0. Recommended: 1 for non-mobile workstations.",
            Tags = ["connection-manager", "wwan", "mobile", "disable", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Wcm],
            ApplyOps   = [RegOp.SetDword(Wcm, "fDisableWwanConnectivity", 1)],
            RemoveOps  = [RegOp.DeleteValue(Wcm, "fDisableWwanConnectivity")],
            DetectOps  = [RegOp.CheckDword(Wcm, "fDisableWwanConnectivity", 1)],
        },
        new TweakDef
        {
            Id = "wcmpol-access-restrictions-on-reconnect",
            Label = "Apply WCM Access Restrictions on Reconnect",
            Category = "Windows Connection Manager Policy",
            Description = "Re-applies WCM connection-policy access restrictions when a managed network reconnects after being temporarily unavailable. Ensures policy enforcement is not bypassed by reconnection events. Default: 0. Recommended: 1.",
            Tags = ["connection-manager", "network", "policy", "reconnect"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Wcm],
            ApplyOps   = [RegOp.SetDword(Wcm, "fApplyAccessRestrictionsOnReconnect", 1)],
            RemoveOps  = [RegOp.DeleteValue(Wcm, "fApplyAccessRestrictionsOnReconnect")],
            DetectOps  = [RegOp.CheckDword(Wcm, "fApplyAccessRestrictionsOnReconnect", 1)],
        },
        new TweakDef
        {
            Id = "wcmpol-block-wifi-when-ethernet",
            Label = "Block Wi-Fi When Ethernet Connected via WCM",
            Category = "Windows Connection Manager Policy",
            Description = "Prevents Windows from maintaining active Wi-Fi connections when a wired Ethernet connection is available. Reduces dual-homed exposure and possible split-tunnel routing issues. Default: not set. Recommended: 1.",
            Tags = ["connection-manager", "wifi", "ethernet", "network", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Wcm],
            ApplyOps   = [RegOp.SetDword(Wcm, "fDisableConnectivityForEthernet", 1)],
            RemoveOps  = [RegOp.DeleteValue(Wcm, "fDisableConnectivityForEthernet")],
            DetectOps  = [RegOp.CheckDword(Wcm, "fDisableConnectivityForEthernet", 1)],
        },
        new TweakDef
        {
            Id = "wcmpol-no-local-policy-merge",
            Label = "Prevent Local WCM Policy Merge",
            Category = "Windows Connection Manager Policy",
            Description = "Prevents local administrator-configured WCM policies from being merged with domain Group Policy settings for WCM. Ensures only domain policy governs connection management. Default: 0. Recommended: 1 for managed environments.",
            Tags = ["connection-manager", "network", "group-policy", "management"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Wcm],
            ApplyOps   = [RegOp.SetDword(Wcm, "fBlockLocalPolicyMerge", 1)],
            RemoveOps  = [RegOp.DeleteValue(Wcm, "fBlockLocalPolicyMerge")],
            DetectOps  = [RegOp.CheckDword(Wcm, "fBlockLocalPolicyMerge", 1)],
        },
    ];
}
