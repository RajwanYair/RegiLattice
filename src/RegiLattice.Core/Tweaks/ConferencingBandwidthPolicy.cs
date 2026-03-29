// RegiLattice.Core — Tweaks/ConferencingBandwidthPolicy.cs
// Teams video conferencing bandwidth and quality limits (Sprint 596).
// Category: "Conferencing Bandwidth Policy" | Slug: confbw
// Key: HKLM\SOFTWARE\Policies\Microsoft\Teams
// All tweaks are CorpSafe = true (Group Policy path).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ConferencingBandwidthPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "confbw-cap-max-video-resolution-720p",
            Label = "Conferencing BW: Cap Maximum Video Resolution at 720p",
            Category = "Conferencing Bandwidth Policy",
            Description = "Sets MaxVideoResolution=540 in Teams policy. Limits outbound camera video to 720p HD (1280x720) per participant rather than allowing uncapped 1080p Full HD. " +
                "On office networks with multiple concurrent video calls, permitting 1080p per user (3–5 Mbps) versus 720p (1–1.5 Mbps) can triple per-user bandwidth consumption. " +
                "Capping at 720p maintains good call quality while substantially reducing aggregate bandwidth demand across the org.",
            Tags = ["teams", "video", "resolution", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Caps outbound video at 720p; ~50-65% bandwidth saving per participant versus uncapped 1080p calls.",
            ApplyOps = [RegOp.SetDword(Key, "MaxVideoResolution", 540)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxVideoResolution")],
            DetectOps = [RegOp.CheckDword(Key, "MaxVideoResolution", 540)],
        },
        new TweakDef
        {
            Id = "confbw-set-max-call-bandwidth-1500kbps",
            Label = "Conferencing BW: Set Maximum Per-Call Bandwidth to 1500 Kbps",
            Category = "Conferencing Bandwidth Policy",
            Description = "Sets MaxCallBitsPerSecond=1500000 (1.5 Mbps) in Teams policy. Sets an absolute ceiling on the total bandwidth consumed by a single Teams audio+video call. " +
                "At 1.5 Mbps the call can sustain 720p video and high-fidelity audio with comfortable headroom. Without this cap, Teams adaptively scales to fill all available bandwidth including on uncongested gigabit networks, crowding out background file transfers and other services.",
            Tags = ["teams", "bandwidth", "cap", "qos", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Caps per-call total bandwidth at 1.5 Mbps; prevents single calls from consuming excessive capacity.",
            ApplyOps = [RegOp.SetDword(Key, "MaxCallBitsPerSecond", 1500000)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxCallBitsPerSecond")],
            DetectOps = [RegOp.CheckDword(Key, "MaxCallBitsPerSecond", 1500000)],
        },
        new TweakDef
        {
            Id = "confbw-set-content-share-bandwidth-500kbps",
            Label = "Conferencing BW: Set Maximum Content-Sharing Bandwidth to 500 Kbps",
            Category = "Conferencing Bandwidth Policy",
            Description = "Sets ContentSharingBitsPerSecond=500000 (500 Kbps) in Teams policy. Limits the bandwidth available for desktop and application sharing streams to 500 Kbps. " +
                "Screen share generates high-frequency updates on busy screens (IDEs, spreadsheets, PowerPoint animations) which can spike to 10+ Mbps without a cap. " +
                "500 Kbps delivers smooth sharing for most presentation use cases while preventing screen share from saturating call bandwidth.",
            Tags = ["teams", "screenshare", "bandwidth", "cap", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Caps content-sharing at 500 Kbps; prevents screen-share spikes from degrading meeting audio/video.",
            ApplyOps = [RegOp.SetDword(Key, "ContentSharingBitsPerSecond", 500000)],
            RemoveOps = [RegOp.DeleteValue(Key, "ContentSharingBitsPerSecond")],
            DetectOps = [RegOp.CheckDword(Key, "ContentSharingBitsPerSecond", 500000)],
        },
        new TweakDef
        {
            Id = "confbw-disable-hd-1080p-outbound-video",
            Label = "Conferencing BW: Disable 1080p Full HD Outbound Video",
            Category = "Conferencing Bandwidth Policy",
            Description = "Sets AllowHD1080p=0 in Teams policy. Explicitly disables sending 1080p video from the local camera during Teams meetings. " +
                "This is a secondary control that works alongside MaxVideoResolution. DisableHD1080p is evaluated at the Teams client layer, while MaxVideoResolution is evaluated by the media negotiation. " +
                "Setting both prevents 1080p video from being negotiated even on high-capacity links where the resolution cap alone may be overridden.",
            Tags = ["teams", "video", "1080p", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disables 1080p video at client layer; complementary control to MaxVideoResolution for dual enforcement.",
            ApplyOps = [RegOp.SetDword(Key, "AllowHD1080p", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowHD1080p")],
            DetectOps = [RegOp.CheckDword(Key, "AllowHD1080p", 0)],
        },
        new TweakDef
        {
            Id = "confbw-set-screen-share-max-framerate-15",
            Label = "Conferencing BW: Cap Screen-Share Frame Rate at 15 FPS",
            Category = "Conferencing Bandwidth Policy",
            Description = "Sets ScreenSharingFrameRate=15 in Teams policy. Reduces the maximum frame rate for Teams desktop and application sharing from the default (up to 30 FPS) to 15 FPS. " +
                "For typical presentation and document review use cases, 15 FPS is indistinguishable from 30 FPS. The bandwidth saving is proportional: halving frame rate nearly halves the constant stream bitrate for static content and substantially reduces peak rates during screen transitions.",
            Tags = ["teams", "screenshare", "framerate", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "15 FPS screen share; unnoticeable for presentations, ~40-50% bandwidth saving versus 30 FPS.",
            ApplyOps = [RegOp.SetDword(Key, "ScreenSharingFrameRate", 15)],
            RemoveOps = [RegOp.DeleteValue(Key, "ScreenSharingFrameRate")],
            DetectOps = [RegOp.CheckDword(Key, "ScreenSharingFrameRate", 15)],
        },
        new TweakDef
        {
            Id = "confbw-disable-together-mode-video",
            Label = "Conferencing BW: Disable Together Mode Video Layout",
            Category = "Conferencing Bandwidth Policy",
            Description = "Sets AllowTogetherMode=0 in Teams policy. Disables the Together Mode virtual background layout that places all participants in a shared scene. " +
                "Together Mode requires high-resolution video feeds from all participants and performs client-side compositing. On meetings with 10+ participants this doubles effective video bandwidth versus gallery view. " +
                "Disabling it is a straightforward bandwidth saving for large meetings on constrained networks.",
            Tags = ["teams", "video", "together-mode", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disables Together Mode; reduces video compositing overhead and bandwidth in large meetings.",
            ApplyOps = [RegOp.SetDword(Key, "AllowTogetherMode", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowTogetherMode")],
            DetectOps = [RegOp.CheckDword(Key, "AllowTogetherMode", 0)],
        },
        new TweakDef
        {
            Id = "confbw-disable-panorama-video",
            Label = "Conferencing BW: Disable Panoramic Room Video",
            Category = "Conferencing Bandwidth Policy",
            Description = "Sets AllowPanoramaVideo=0 in Teams policy. Disables the panoramic (wide-angle) room video mode available on Teams Rooms devices. " +
                "Panoramic video streams require significantly higher resolution and frame rates than standard participant video. For most remote participants, a standard camera view of the room is functionally equivalent. " +
                "Disabling panoramic saves 30–50% of per-room outbound bandwidth.",
            Tags = ["teams", "video", "panorama", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables panoramic room video; ~30-50% outbound bandwidth saving on Teams Rooms devices.",
            ApplyOps = [RegOp.SetDword(Key, "AllowPanoramaVideo", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowPanoramaVideo")],
            DetectOps = [RegOp.CheckDword(Key, "AllowPanoramaVideo", 0)],
        },
        new TweakDef
        {
            Id = "confbw-enable-adaptive-bitrate-control",
            Label = "Conferencing BW: Enable Adaptive Bitrate Control for Calls",
            Category = "Conferencing Bandwidth Policy",
            Description = "Sets EnableAdaptiveBitrateForCalling=1 in Teams policy. Enables the Teams adaptive bitrate algorithm to dynamically reduce video quality when packet loss or congestion is detected rather than maintaining maximum quality until the call breaks. " +
                "Without adaptive bitrate the media engine attempts to hold resolution fixed, which causes burst packet loss and call freezes. With it, video gracefully degrades to audio-only before dropping the call.",
            Tags = ["teams", "video", "adaptive-bitrate", "resilience", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enables adaptive bitrate; gracefully degrades video on congested links instead of call drops.",
            ApplyOps = [RegOp.SetDword(Key, "EnableAdaptiveBitrateForCalling", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableAdaptiveBitrateForCalling")],
            DetectOps = [RegOp.CheckDword(Key, "EnableAdaptiveBitrateForCalling", 1)],
        },
        new TweakDef
        {
            Id = "confbw-set-auto-degrade-threshold-50pct",
            Label = "Conferencing BW: Trigger Auto Quality Downgrade at 50% Bandwidth",
            Category = "Conferencing Bandwidth Policy",
            Description = "Sets AutoDegradeBandwidthThresholdPercent=50 in Teams policy. Configures the Teams media engine to start downgrading video resolution and frame rate once available bandwidth falls below 50% of the negotiated session maximum. " +
                "An earlier trigger (50% vs. default 75%) gives the adaptive algorithm more headroom to reduce bitrate before packetloss becomes perceptible, resulting in smoother degradation rather than abrupt quality drops.",
            Tags = ["teams", "video", "adaptive-bitrate", "degradation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Earlier 50% threshold for quality degradation; smoother congestion response versus default 75% trigger.",
            ApplyOps = [RegOp.SetDword(Key, "AutoDegradeBandwidthThresholdPercent", 50)],
            RemoveOps = [RegOp.DeleteValue(Key, "AutoDegradeBandwidthThresholdPercent")],
            DetectOps = [RegOp.CheckDword(Key, "AutoDegradeBandwidthThresholdPercent", 50)],
        },
        new TweakDef
        {
            Id = "confbw-disable-immersive-spaces",
            Label = "Conferencing BW: Disable Teams Immersive Spaces (3D Metaverse)",
            Category = "Conferencing Bandwidth Policy",
            Description = "Sets AllowImmersiveSpaces=0 in Teams policy. Disables the Teams Immersive Spaces feature which renders a 3D virtual meeting environment using the Mesh platform. " +
                "Immersive Spaces require GPU-accelerated 3D rendering and a dedicated high-bandwidth video stream that is typically 2–4× the bandwidth of a standard gallery view call. " +
                "Disabling this feature is appropriate for organisations where standard HD video meetings are the expected standard and 3D environments are unnecessary.",
            Tags = ["teams", "immersive", "mesh", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disables Teams 3D Immersive Spaces; saves 2-4× bandwidth versus standard video; GPU load reduced.",
            ApplyOps = [RegOp.SetDword(Key, "AllowImmersiveSpaces", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowImmersiveSpaces")],
            DetectOps = [RegOp.CheckDword(Key, "AllowImmersiveSpaces", 0)],
        },
    ];
}
