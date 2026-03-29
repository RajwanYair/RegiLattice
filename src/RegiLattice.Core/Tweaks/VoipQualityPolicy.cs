// RegiLattice.Core — Tweaks/VoipQualityPolicy.cs
// Teams-specific VoIP media port and DSCP quality-of-service policy (Sprint 595).
// Category: "VoIP QoS Policy" | Slug: voipqos
// Key: HKLM\SOFTWARE\Policies\Microsoft\Teams
// All tweaks are CorpSafe = true (Group Policy path).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class VoipQualityPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "voipqos-set-teams-audio-dscp-value",
            Label = "VoIP QoS: Mark Teams Audio RTP with DSCP EF (46)",
            Category = "VoIP QoS Policy",
            Description = "Sets AudioDscpValue=46 in Teams QoS policy. Instructs Teams to mark all real-time audio RTP packets with DSCP EF (Expedited Forwarding = 46, the highest priority class). " +
                "On enterprise networks with QoS-aware switches and routers, EF-marked packets receive the smallest queuing delay and lowest drop probability, which directly reduces jitter and one-way latency in Teams calls. " +
                "This setting is distinct from the generic Windows QoS multimedia scheduling rate and applies specifically to the Teams media engine RTP streams.",
            Tags = ["teams", "voip", "qos", "dscp", "audio", "rtp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Marks Teams audio RTP with EF DSCP 46; critical for low-latency voice on congested enterprise networks.",
            ApplyOps = [RegOp.SetDword(Key, "AudioDscpValue", 46)],
            RemoveOps = [RegOp.DeleteValue(Key, "AudioDscpValue")],
            DetectOps = [RegOp.CheckDword(Key, "AudioDscpValue", 46)],
        },
        new TweakDef
        {
            Id = "voipqos-set-teams-video-dscp-value",
            Label = "VoIP QoS: Mark Teams Video RTP with DSCP AF41 (34)",
            Category = "VoIP QoS Policy",
            Description = "Sets VideoDscpValue=34 in Teams QoS policy. Marks Teams video RTP packets with DSCP AF41 (Assured Forwarding 41 = 34). " +
                "AF41 is the IETF recommendation for interactive video conferencing traffic. It receives higher priority than best-effort but is de-prioritised below EF (audio). " +
                "Separating audio (EF) and video (AF41) ensures audio is never starved by high-bitrate video bursts during congestion.",
            Tags = ["teams", "voip", "qos", "dscp", "video", "rtp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Marks Teams video with AF41 DSCP 34; prevents video bursts from starving audio on saturated links.",
            ApplyOps = [RegOp.SetDword(Key, "VideoDscpValue", 34)],
            RemoveOps = [RegOp.DeleteValue(Key, "VideoDscpValue")],
            DetectOps = [RegOp.CheckDword(Key, "VideoDscpValue", 34)],
        },
        new TweakDef
        {
            Id = "voipqos-set-teams-appshar-dscp-value",
            Label = "VoIP QoS: Mark Teams App-Sharing with DSCP AF21 (18)",
            Category = "VoIP QoS Policy",
            Description = "Sets AppShareDscpValue=18 in Teams QoS policy. Marks Teams application-sharing and desktop-sharing RTP streams with DSCP AF21 (Assured Forwarding 21 = 18). " +
                "Screen share generates large and bursty traffic which should be deprioritised relative to live audio and video to prevent real-time media degredation during presentations.",
            Tags = ["teams", "voip", "qos", "dscp", "screenshare", "rtp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Marks app-sharing with AF21 DSCP 18; prevents screen share bursts from degrading audio/video quality.",
            ApplyOps = [RegOp.SetDword(Key, "AppShareDscpValue", 18)],
            RemoveOps = [RegOp.DeleteValue(Key, "AppShareDscpValue")],
            DetectOps = [RegOp.CheckDword(Key, "AppShareDscpValue", 18)],
        },
        new TweakDef
        {
            Id = "voipqos-enable-teams-audio-port-range",
            Label = "VoIP QoS: Enable Teams-Specific Audio UDP Port Range",
            Category = "VoIP QoS Policy",
            Description = "Sets AudioPortsEnabled=1 in Teams QoS policy. Enables the use of a dedicated UDP port range for Teams audio media. " +
                "Port-based QoS rules on network switches and firewalls can then classify and prioritise Teams audio traffic from these specific ports rather than relying solely on DSCP markings, which are sometimes stripped by ISPs.",
            Tags = ["teams", "voip", "qos", "ports", "udp", "audio"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables fixed port range for Teams audio; allows port-based QoS classification in addition to DSCP.",
            ApplyOps = [RegOp.SetDword(Key, "AudioPortsEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AudioPortsEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "AudioPortsEnabled", 1)],
        },
        new TweakDef
        {
            Id = "voipqos-set-teams-audio-port-start-50000",
            Label = "VoIP QoS: Set Teams Audio Port Range Start to 50000",
            Category = "VoIP QoS Policy",
            Description = "Sets AudioPortStart=50000 in Teams QoS policy. Configures the start of the UDP port range used by Teams audio media to port 50000. " +
                "This port base aligns with the Microsoft-recommended range for Teams voice and allows network administrators to create firewall ACLs and QoS policies targeting the well-known 50000–50019 range.",
            Tags = ["teams", "voip", "qos", "ports", "audio", "udp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Sets audio port range start to 50000 per Microsoft recommendation; enables precise firewall and QoS rules.",
            ApplyOps = [RegOp.SetDword(Key, "AudioPortStart", 50000)],
            RemoveOps = [RegOp.DeleteValue(Key, "AudioPortStart")],
            DetectOps = [RegOp.CheckDword(Key, "AudioPortStart", 50000)],
        },
        new TweakDef
        {
            Id = "voipqos-set-teams-audio-port-count-20",
            Label = "VoIP QoS: Set Teams Audio Port Count to 20",
            Category = "VoIP QoS Policy",
            Description = "Sets AudioPortCount=20 in Teams QoS policy. Allocates 20 consecutive UDP ports for Teams audio media starting from AudioPortStart. " +
                "A count of 20 provides enough ports for simultaneous call sessions on a single machine while keeping the range narrow enough for precise firewall and QoS ACL rules.",
            Tags = ["teams", "voip", "qos", "ports", "audio", "udp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Allocates 20 UDP ports for Teams audio; balances multi-session capacity with narrow QoS rule precision.",
            ApplyOps = [RegOp.SetDword(Key, "AudioPortCount", 20)],
            RemoveOps = [RegOp.DeleteValue(Key, "AudioPortCount")],
            DetectOps = [RegOp.CheckDword(Key, "AudioPortCount", 20)],
        },
        new TweakDef
        {
            Id = "voipqos-enable-teams-video-port-range",
            Label = "VoIP QoS: Enable Teams-Specific Video UDP Port Range",
            Category = "VoIP QoS Policy",
            Description = "Sets VideoPortsEnabled=1 in Teams QoS policy. Enables the use of a dedicated UDP port range for Teams video media streams. " +
                "Separating video on its own port range allows network equipment to apply different QoS policies to audio and video independently, which is important when network bandwidth needs to preferentially protect audio quality over video.",
            Tags = ["teams", "voip", "qos", "ports", "video", "udp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables dedicated video port range; allows separate QoS treatment of audio versus video streams.",
            ApplyOps = [RegOp.SetDword(Key, "VideoPortsEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "VideoPortsEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "VideoPortsEnabled", 1)],
        },
        new TweakDef
        {
            Id = "voipqos-set-teams-video-port-start-50020",
            Label = "VoIP QoS: Set Teams Video Port Range Start to 50020",
            Category = "VoIP QoS Policy",
            Description = "Sets VideoPortStart=50020 in Teams QoS policy. Sets the starting UDP port for Teams video media to 50020, immediately following the audio port range (50000–50019). " +
                "This layout allows a single contiguous firewall rule (50000–50039) to cover both audio and video, while still allowing separate DSCP markings to be applied per-range.",
            Tags = ["teams", "voip", "qos", "ports", "video", "udp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Sets video port start to 50020; aligns with audio range for manageable firewall rule design.",
            ApplyOps = [RegOp.SetDword(Key, "VideoPortStart", 50020)],
            RemoveOps = [RegOp.DeleteValue(Key, "VideoPortStart")],
            DetectOps = [RegOp.CheckDword(Key, "VideoPortStart", 50020)],
        },
        new TweakDef
        {
            Id = "voipqos-set-teams-video-port-count-20",
            Label = "VoIP QoS: Set Teams Video Port Count to 20",
            Category = "VoIP QoS Policy",
            Description = "Sets VideoPortCount=20 in Teams QoS policy. Allocates 20 UDP ports for Teams video media starting at VideoPortStart. " +
                "20 ports accommodates multiple simultaneous video sessions and gallery view scenarios without creating an overly broad firewall footprint.",
            Tags = ["teams", "voip", "qos", "ports", "video", "udp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Allocates 20 UDP ports for Teams video; supports gallery view with a narrow, manageable port range.",
            ApplyOps = [RegOp.SetDword(Key, "VideoPortCount", 20)],
            RemoveOps = [RegOp.DeleteValue(Key, "VideoPortCount")],
            DetectOps = [RegOp.CheckDword(Key, "VideoPortCount", 20)],
        },
        new TweakDef
        {
            Id = "voipqos-enable-teams-appshar-port-range",
            Label = "VoIP QoS: Enable Teams App-Sharing UDP Port Range",
            Category = "VoIP QoS Policy",
            Description = "Sets AppSharePortsEnabled=1 in Teams QoS policy. Enables a dedicated UDP port range for Teams application-sharing and desktop-sharing media streams. " +
                "Isolating app-sharing on its own port range allows network QoS policies to apply lower priority scheduling to screen share traffic while still guaranteeing audio and video delivery during congestion.",
            Tags = ["teams", "voip", "qos", "ports", "screenshare", "udp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables dedicated app-sharing port range; decouple screen share QoS from audio/video port policies.",
            ApplyOps = [RegOp.SetDword(Key, "AppSharePortsEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AppSharePortsEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "AppSharePortsEnabled", 1)],
        },
    ];
}
