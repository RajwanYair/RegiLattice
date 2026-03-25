#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 260 — Network Projection Policy (10 tweaks)
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\NetworkProjector
//      HKLM\SOFTWARE\Policies\Microsoft\Windows\Connect  (Miracast / Project to This PC)
// Controls legacy "Connect to a Network Projector" and Miracast wireless display projection.
// Slug: "netproj-"
internal static class NetworkProjectionPolicy
{
    private const string ProjKey    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProjector";
    private const string ConnKey    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect";
    private const string WdisKey    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WirelessDisplay";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netproj-disable-network-projector",
            Label = "Disable Legacy Network Projector Connection",
            Category = "Network Projection Policy",
            Description = "Sets NoNetworkProjector=1 in the NetworkProjector policy key. "
                + "Prevents users from connecting this machine to a legacy network projector via "
                + "the 'Connect to a Network Projector' wizard (Windows 7/8 era feature). "
                + "Network projectors exposed over LAN can be a lateral movement vector if reachable "
                + "from an untrusted network segment. Disabling this closes the outbound connecting path. "
                + "Default: absent (connection allowed). Recommended: 1 on endpoints without shared projectors.",
            Tags = ["network-projection", "projector", "wireless-display", "lan", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Legacy 'Connect to a Network Projector' wizard disabled.",
            ApplyOps  = [RegOp.SetDword(ProjKey, "NoNetworkProjector", 1)],
            RemoveOps = [RegOp.DeleteValue(ProjKey, "NoNetworkProjector")],
            DetectOps = [RegOp.CheckDword(ProjKey, "NoNetworkProjector", 1)],
        },
        new TweakDef
        {
            Id = "netproj-disable-project-to-this-pc",
            Label = "Disable 'Project to This PC' (Miracast Receiver)",
            Category = "Network Projection Policy",
            Description = "Sets AllowProjectionToPC=0 in the Connect policy key. "
                + "Prevents this machine from acting as a Miracast receiver ('Project to This PC'). "
                + "When enabled, the PC accepts incoming wireless display connections from phones, "
                + "tablets, and other PCs on the same Wi-Fi network. Disabling this removes the "
                + "reception capability entirely, preventing unauthorised display mirroring onto "
                + "this machine. Default: absent (projection receiver allowed).",
            Tags = ["network-projection", "miracast", "wireless-display", "receiver", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Miracast 'Project to This PC' receiver mode disabled; no incoming wireless display connections.",
            ApplyOps  = [RegOp.SetDword(ConnKey, "AllowProjectionToPC", 0)],
            RemoveOps = [RegOp.DeleteValue(ConnKey, "AllowProjectionToPC")],
            DetectOps = [RegOp.CheckDword(ConnKey, "AllowProjectionToPC", 0)],
        },
        new TweakDef
        {
            Id = "netproj-require-pin-for-projection",
            Label = "Require PIN for 'Project to This PC'",
            Category = "Network Projection Policy",
            Description = "Sets RequirePinForPairing=2 in the Connect policy key (2=always require PIN). "
                + "Requires a unique pairing PIN to be entered on the projecting device before it can "
                + "establish a wireless display connection to this PC. Without a PIN requirement, "
                + "any device on the same Wi-Fi can connect instantly without user consent. "
                + "Values: 0=never, 1=first-time only, 2=always. "
                + "Default: absent. Recommended: 2 in shared/open-office environments.",
            Tags = ["network-projection", "miracast", "pin", "pairing", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "PIN required every time before a device can project wirelessly to this PC.",
            ApplyOps  = [RegOp.SetDword(ConnKey, "RequirePinForPairing", 2)],
            RemoveOps = [RegOp.DeleteValue(ConnKey, "RequirePinForPairing")],
            DetectOps = [RegOp.CheckDword(ConnKey, "RequirePinForPairing", 2)],
        },
        new TweakDef
        {
            Id = "netproj-restrict-projection-to-secured-networks",
            Label = "Restrict Projection to Secured (Non-Open) Wi-Fi Networks",
            Category = "Network Projection Policy",
            Description = "Sets AllowProjectionToSecuredPCOnly=1 in the Connect policy key. "
                + "Limits 'Project to This PC' to accept incoming Miracast connections only when the "
                + "machine is connected to a password-protected (WPA/WPA2/WPA3) Wi-Fi network. "
                + "Prevents accidental projection exposure when the machine is on an open conference "
                + "room or hotel Wi-Fi where any third party could project content. "
                + "Default: absent. Recommended: 1 for roaming/mobile employees.",
            Tags = ["network-projection", "miracast", "wifi-security", "wpa", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Wireless projection only accepted when on a password-protected Wi-Fi network.",
            ApplyOps  = [RegOp.SetDword(ConnKey, "AllowProjectionToSecuredPCOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(ConnKey, "AllowProjectionToSecuredPCOnly")],
            DetectOps = [RegOp.CheckDword(ConnKey, "AllowProjectionToSecuredPCOnly", 1)],
        },
        new TweakDef
        {
            Id = "netproj-block-source-projection",
            Label = "Block This PC From Projecting to Other Devices",
            Category = "Network Projection Policy",
            Description = "Sets AllowProjectionFromPC=0 in the Connect policy key. "
                + "Prevents the user from using 'Connect' or the Project button to send this PC's "
                + "display to a Miracast dongle, smart TV, or wireless display adapter. "
                + "While the risk is lower than the receive path, projecting to untrusted displays "
                + "in a public setting can expose screen content. "
                + "Default: absent (projection from PC allowed). Recommended: 0 in kiosk/locked environments.",
            Tags = ["network-projection", "miracast", "source", "wireless-display", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "This PC cannot project its display to other wireless display devices.",
            ApplyOps  = [RegOp.SetDword(ConnKey, "AllowProjectionFromPC", 0)],
            RemoveOps = [RegOp.DeleteValue(ConnKey, "AllowProjectionFromPC")],
            DetectOps = [RegOp.CheckDword(ConnKey, "AllowProjectionFromPC", 0)],
        },
        new TweakDef
        {
            Id = "netproj-disable-wireless-display-infrastructure",
            Label = "Disable Wireless Display Infrastructure Mode",
            Category = "Network Projection Policy",
            Description = "Sets AllowWirelessDisplayInfrastructure=0 in the WirelessDisplay policy key. "
                + "Disables the infrastructure-mode Miracast projection that routes wireless display "
                + "traffic over a Wi-Fi router rather than a direct Wi-Fi Direct peer-to-peer link. "
                + "Infrastructure-mode Miracast uses the corporate Wi-Fi, potentially traversing "
                + "network segments and adding latency. Restricting to Wi-Fi Direct reduces the "
                + "attack surface of projection traffic. Default: absent (infrastructure allowed).",
            Tags = ["network-projection", "miracast", "infrastructure", "wifi-direct", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Infrastructure-mode Miracast disabled; only Wi-Fi Direct peer-to-peer projection allowed.",
            ApplyOps  = [RegOp.SetDword(WdisKey, "AllowWirelessDisplayInfrastructure", 0)],
            RemoveOps = [RegOp.DeleteValue(WdisKey, "AllowWirelessDisplayInfrastructure")],
            DetectOps = [RegOp.CheckDword(WdisKey, "AllowWirelessDisplayInfrastructure", 0)],
        },
        new TweakDef
        {
            Id = "netproj-disable-miracast-discovery-mcast",
            Label = "Disable Miracast Multicast Discovery Broadcast",
            Category = "Network Projection Policy",
            Description = "Sets DisableDeviceDiscovery=1 in the WirelessDisplay policy key. "
                + "Prevents this machine from continuously broadcasting Miracast advertisement "
                + "packets that announce its 'Project to This PC' capability on the local network. "
                + "Passive Miracast discovery broadcasting can reveal machine presence and name "
                + "on the network even when the machine would otherwise be silent. "
                + "Default: absent (discovery broadcasts on). Recommended: 1 for stealth network posture.",
            Tags = ["network-projection", "miracast", "discovery", "multicast", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Miracast device presence broadcasts stopped; machine not discoverable via wireless display scanning.",
            ApplyOps  = [RegOp.SetDword(WdisKey, "DisableDeviceDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(WdisKey, "DisableDeviceDiscovery")],
            DetectOps = [RegOp.CheckDword(WdisKey, "DisableDeviceDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "netproj-enforce-hdcp-for-wireless-display",
            Label = "Enforce HDCP Content Protection on Wireless Display",
            Category = "Network Projection Policy",
            Description = "Sets AllowProjectionToHDCP=1 in the WirelessDisplay policy key. "
                + "Requires that the receiving wireless display device supports HDCP (High-bandwidth "
                + "Digital Content Protection) before the PC will project to it. "
                + "Prevents DRM-protected content (streaming video, presentations with ERM) from "
                + "being cast to non-HDCP-compliant receiver dongles or TVs that might not encrypt "
                + "the content link layer. "
                + "Default: absent. Recommended: 1 on systems that display confidential or DRM content.",
            Tags = ["network-projection", "miracast", "hdcp", "drm", "content-protection", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Wireless projection only to HDCP-capable receivers; non-compliant displays rejected.",
            ApplyOps  = [RegOp.SetDword(WdisKey, "AllowProjectionToHDCP", 1)],
            RemoveOps = [RegOp.DeleteValue(WdisKey, "AllowProjectionToHDCP")],
            DetectOps = [RegOp.CheckDword(WdisKey, "AllowProjectionToHDCP", 1)],
        },
        new TweakDef
        {
            Id = "netproj-disable-projector-peer-trust",
            Label = "Disable Auto-Trust for Previously Projected Displays",
            Category = "Network Projection Policy",
            Description = "Sets AllowPreviouslyPairedDevice=0 in the Connect policy key. "
                + "Prevents Windows from automatically accepting wireless display connections from "
                + "devices that have previously been paired (trusted) with this PC. "
                + "Previously paired devices can reconnect without PIN re-entry, which reduces friction "
                + "but removes the consent step. If a paired device is stolen or compromised, "
                + "this policy ensures it cannot silently reconnect. "
                + "Default: absent (previous pairs trusted). Recommended: 0 in high-security environments.",
            Tags = ["network-projection", "miracast", "trust", "pairing", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Previously paired Miracast devices not auto-trusted; PIN required every connection.",
            ApplyOps  = [RegOp.SetDword(ConnKey, "AllowPreviouslyPairedDevice", 0)],
            RemoveOps = [RegOp.DeleteValue(ConnKey, "AllowPreviouslyPairedDevice")],
            DetectOps = [RegOp.CheckDword(ConnKey, "AllowPreviouslyPairedDevice", 0)],
        },
        new TweakDef
        {
            Id = "netproj-set-projection-screenlock-timeout",
            Label = "Set Wireless Display Auto-Lock Screen After Idle",
            Category = "Network Projection Policy",
            Description = "Sets ProjectionIdleTimeout=5 in the Connect policy key. "
                + "Sets the number of minutes of idle time on a 'Project to This PC' session before "
                + "the received display is automatically locked or disconnected. Without a timeout, "
                + "a projecting device's session persists indefinitely even after the user walks away, "
                + "allowing anyone at the receiving display to view projected content. "
                + "Value in minutes; 5 minutes aligns with standard workstation lock policies. "
                + "Default: absent (no idle timeout). Recommended: 5 for shared display environments.",
            Tags = ["network-projection", "miracast", "idle", "timeout", "screen-lock", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Wireless display session locks after 5 minutes of idle; unattended projection disconnected.",
            ApplyOps  = [RegOp.SetDword(ConnKey, "ProjectionIdleTimeout", 5)],
            RemoveOps = [RegOp.DeleteValue(ConnKey, "ProjectionIdleTimeout")],
            DetectOps = [RegOp.CheckDword(ConnKey, "ProjectionIdleTimeout", 5)],
        },
    ];
}
