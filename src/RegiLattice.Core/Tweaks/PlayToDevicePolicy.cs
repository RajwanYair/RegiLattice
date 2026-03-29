// RegiLattice.Core — Tweaks/PlayToDevicePolicy.cs
// DLNA / Play To receiver and WSD media device policy controls (Sprint 606).
// Category: "Play To Device Policy" | Slug: playtodev
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\PlayToReceiver
//      HKLM\SOFTWARE\Policies\Microsoft\Windows\WSD

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PlayToDevicePolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PlayToReceiver";
    private const string WsdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "playtodev-disable-play-to-receiver",
            Label = "Play To: Disable Windows Play To Receiver Feature",
            Category = "Play To Device Policy",
            Description = "Sets NotAllowPlayToReceiver=1 in PlayToReceiver machine policy. Disables the Windows 'Play To' receiver capability that allows other DLNA-compatible devices on the same network to push media content to this PC for playback. " +
                "'Play To' opens this device as a DLNA media renderer, listening for UPnP/DLNA control point commands from any device on the local network. On a corporate network, this allows any DLNA-capable device (including personal mobile phones) to push multimedia content to corporate workstations without authentication. Disabling the receiver prevents the device from accepting unsolicited media content pushes.",
            Tags = ["dlna", "play-to", "receiver", "media", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disables DLNA Play To receiver; workstation cannot accept media pushes from devices on local network.",
            ApplyOps = [RegOp.SetDword(Key, "NotAllowPlayToReceiver", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NotAllowPlayToReceiver")],
            DetectOps = [RegOp.CheckDword(Key, "NotAllowPlayToReceiver", 1)],
        },
        new TweakDef
        {
            Id = "playtodev-disable-play-to-sender",
            Label = "Play To: Disable Windows Play To Media Source Sending",
            Category = "Play To Device Policy",
            Description = "Sets DisablePlayTo=1 in PlayToReceiver machine policy. Disables the ability for users to use this PC as a 'Play To' source — sending media from Windows Media Player, Photos, or other DLNA-compatible applications to an external DLNA renderer. " +
                "Using this PC as a 'Play To' sender connects to UPnP devices on the network and pushes streaming data to them. On a corporate network, this could be used to stream sensitive video content from a corporate machine to a personal smart TV, Chromecast, or other unmanaged renderer. Disabling 'Play To' sender functionality prevents this data exfiltration vector.",
            Tags = ["dlna", "play-to", "sender", "media", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks DLNA media sending; screen/media content not streamable from this PC to external renderers.",
            ApplyOps = [RegOp.SetDword(Key, "DisablePlayTo", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePlayTo")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePlayTo", 1)],
        },
        new TweakDef
        {
            Id = "playtodev-block-wsd-device-discovery",
            Label = "Play To: Block WSD (Web Services on Devices) Network Discovery",
            Category = "Play To Device Policy",
            Description = "Sets DisableWSDDiscovery=1 in WSD machine policy. Prevents the Windows WSD (Web Services on Devices) stack from announcing this device to the local network and from probing for WSD-compatible peripherals — including networked printers, scanners, and media renderers. " +
                "WSD uses multicast UDP probes (WS-Discovery) that announce the device's presence to all LAN segments. These broadcasts leak device identity, OS version, and service capabilities to all network listeners. Disabling WSD discovery reduces the device's network footprint.",
            Tags = ["wsd", "discovery", "network", "multicast", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Blocks WSD/WS-Discovery multicast probes; device not discoverable via WSD protocol. May affect network printer discovery.",
            ApplyOps = [RegOp.SetDword(WsdKey, "DisableWSDDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableWSDDiscovery")],
            DetectOps = [RegOp.CheckDword(WsdKey, "DisableWSDDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "playtodev-block-wsd-printer-discovery",
            Label = "Play To: Block WSD-Based Printer Auto-Discovery",
            Category = "Play To Device Policy",
            Description = "Sets DisableWSDPrinting=1 in WSD machine policy. Prevents auto-discovery and installation of WSD-connected network printers. " +
                "WSD printer discovery installs printers from the local network automatically without user approval by default on domain-joined machines. On enterprise networks with centralised print server management, rogue WSD printers could intercept print jobs if employees accidentally redirect documents to an unauthorised WSD printer device near them. Disabling WSD printer discovery enforces exclusively server-managed print queue access.",
            Tags = ["wsd", "printer", "discovery", "print", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Blocks WSD printer auto-install; print queues must be added manually or via GPO print server.",
            ApplyOps = [RegOp.SetDword(WsdKey, "DisableWSDPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableWSDPrinting")],
            DetectOps = [RegOp.CheckDword(WsdKey, "DisableWSDPrinting", 1)],
        },
        new TweakDef
        {
            Id = "playtodev-block-wsd-scanner-discovery",
            Label = "Play To: Block WSD-Based Scanner Auto-Discovery",
            Category = "Play To Device Policy",
            Description = "Sets DisableWSDScanning=1 in WSD machine policy. Prevents automatic discovery and installation of network scanners that advertise themselves via WSD/WIA (Windows Image Acquisition). " +
                "WSD scanner discovery installs scanning drivers and opens WIA sessions to any WSD-compatible scanner found on the network. Unauthorised scanners on the network could be configured to receive forwarded scan-to-email jobs from misconfigured endpoints. Disabling WSD scanner discovery prevents unsolicited scanner driver installation and ensures scanning hardware is explicitly approved by IT.",
            Tags = ["wsd", "scanner", "discovery", "wia", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Blocks WSD scanner auto-discovery; scanners require manual or GPO-managed WIA configuration.",
            ApplyOps = [RegOp.SetDword(WsdKey, "DisableWSDScanning", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableWSDScanning")],
            DetectOps = [RegOp.CheckDword(WsdKey, "DisableWSDScanning", 1)],
        },
        new TweakDef
        {
            Id = "playtodev-disable-play-to-dmr-advertisement",
            Label = "Play To: Disable DLNA Digital Media Renderer Advertisement",
            Category = "Play To Device Policy",
            Description = "Sets NotAdvertisePlayToDevice=1 in PlayToReceiver machine policy. Prevents this Windows PC from advertising itself as a DLNA Digital Media Renderer (DMR) on the local network. " +
                "DMR advertisement broadcasts multicast UPnP SSDP announcements that include the device's name, model, IP address, and capabilities to all devices on the network. This advertisement allows any DLNA control point (phone, tablet, smart TV) to discover and push media to the PC. Suppressing the advertisement hides the device from DLNA discovery without fully disabling the network stack services.",
            Tags = ["dlna", "dmr", "advertisement", "ssdp", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Suppresses DLNA DMR advertisement; PC not visible to DLNA control points on the network.",
            ApplyOps = [RegOp.SetDword(Key, "NotAdvertisePlayToDevice", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NotAdvertisePlayToDevice")],
            DetectOps = [RegOp.CheckDword(Key, "NotAdvertisePlayToDevice", 1)],
        },
        new TweakDef
        {
            Id = "playtodev-disable-media-sharing-network-access",
            Label = "Play To: Disable Media Library Network Sharing",
            Category = "Play To Device Policy",
            Description = "Sets DisableMediaSharing=1 in PlayToReceiver machine policy. Prevents this PC from sharing its media library (pictures, videos, music) with other devices on the network via the Windows Media Player network sharing service. " +
                "Media library sharing exposes the contents of the user's Documents, Pictures, and Music folders to any UPnP/DLNA media renderer on the local network without per-file access controls. On corporate networks, user Documents folders frequently contain sensitive files that the file-sharing component includes in its media index.",
            Tags = ["media", "sharing", "library", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks media library network sharing; document folders not exposed via DLNA/UPnP media server.",
            ApplyOps = [RegOp.SetDword(Key, "DisableMediaSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMediaSharing")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMediaSharing", 1)],
        },
        new TweakDef
        {
            Id = "playtodev-restrict-play-to-local-subnet-only",
            Label = "Play To: Restrict Play To and DLNA to Local Subnet Only",
            Category = "Play To Device Policy",
            Description = "Sets AllowedNetworkScopes=1 in PlayToReceiver machine policy. Restricts the DLNA/Play To feature to the local subnet only, preventing cross-subnet media streaming and rendering. " +
                "Limiting Play To to the local subnet ensures that media streaming sessions cannot traverse network routers into other VLANs or the wide internet. This is the least-restrictive enterprise hardening option for organisations where DLNA is permitted for AV room systems on a dedicated VLAN but must not be accessible from corporate workstation VLANs.",
            Tags = ["dlna", "subnet", "scope", "network-segmentation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "DLNA scoped to local subnet only; cross-VLAN and internet-routed media streaming blocked.",
            ApplyOps = [RegOp.SetDword(Key, "AllowedNetworkScopes", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowedNetworkScopes")],
            DetectOps = [RegOp.CheckDword(Key, "AllowedNetworkScopes", 1)],
        },
        new TweakDef
        {
            Id = "playtodev-disable-device-play-auto-start",
            Label = "Play To: Disable Auto-Start of Play To Service at Logon",
            Category = "Play To Device Policy",
            Description = "Sets DisableAutoStart=1 in PlayToReceiver machine policy. Prevents the Windows Play To Receiver service from starting automatically at user logon. " +
                "The Play To receiver service starts in the background and maintains a network listener even when neither party has initiated a media session. This background process consumes memory, CPU, and network port capacity. Disabling auto-start ensures the service only runs when explicitly invoked, reducing the device's idle network service footprint.",
            Tags = ["dlna", "service", "auto-start", "startup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Play To service not auto-started; listener not running unless explicitly launched.",
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoStart", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoStart")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoStart", 1)],
        },
        new TweakDef
        {
            Id = "playtodev-disable-wsd-host-discovery",
            Label = "Play To: Disable WSD Function Discovery Host (FDHOST) Network Broadcast",
            Category = "Play To Device Policy",
            Description = "Sets DisableFunctionDiscoveryHostBroadcast=1 in WSD machine policy. Prevents the Function Discovery Host service from broadcasting WSD host announcements that advertise this machine's services (web services, UPnP capabilities) to other devices on the network. " +
                "Function Discovery is the mechanism Windows uses to populate the Network window in Explorer. Broadcasting the host's function discovery metadata exposes its installed services and capabilities to all LAN listeners. On hardened workstations, eliminating unnecessary network announcements reduces the device's identifiable surface in passive network reconnaissance.",
            Tags = ["wsd", "fdhost", "broadcast", "discovery", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Suppresses WSD function discovery host broadcasts; device less identifiable via passive LAN scanning.",
            ApplyOps = [RegOp.SetDword(WsdKey, "DisableFunctionDiscoveryHostBroadcast", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableFunctionDiscoveryHostBroadcast")],
            DetectOps = [RegOp.CheckDword(WsdKey, "DisableFunctionDiscoveryHostBroadcast", 1)],
        },
    ];
}
