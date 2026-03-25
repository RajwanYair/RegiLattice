// RegiLattice.Core — Tweaks/VideoCapturePolicy.cs
// Sprint 271: Video Capture Group Policy (10 tweaks)
// Category: "Video Capture Policy" | Slug: vcap
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VideoCapture

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class VideoCapturePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VideoCapture";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vcap-disable-video-capture",
            Label = "Disable Video Capture Device Access",
            Category = "Video Capture Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Sets DisableVideoCapture=1 in the VideoCapture policy key. Blocks all "
                + "application-level access to video capture hardware (webcams, capture cards, "
                + "virtual cameras). Applications requesting the Camera device class are denied "
                + "at the policy layer before the privacy permission prompt. Stronger than "
                + "per-app toggles; applies universally. Default: 0. Recommended: 1 on "
                + "kiosk, conference room, or regulated-data machines.",
            Tags = ["video-capture", "camera", "webcam", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableVideoCapture", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableVideoCapture")],
            DetectOps = [RegOp.CheckDword(Key, "DisableVideoCapture", 1)],
        },
        new TweakDef
        {
            Id = "vcap-disable-screen-capture",
            Label = "Disable Screen Capture via Policy",
            Category = "Video Capture Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Sets DisableScreenCapture=1 in the VideoCapture policy key. Prevents "
                + "applications from using screen-capture APIs (Desktop Duplication API, "
                + "Graphics.CaptureItem) to record the screen contents. Blocks tools such "
                + "as OBS, Teams recording, and screenshot utilities at the policy layer. "
                + "Default: 0. Recommended: 1 on machines handling sensitive classified "
                + "or commercially confidential data.",
            Tags = ["video-capture", "screen-capture", "screenshot", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableScreenCapture", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableScreenCapture")],
            DetectOps = [RegOp.CheckDword(Key, "DisableScreenCapture", 1)],
        },
        new TweakDef
        {
            Id = "vcap-disable-broadcast",
            Label = "Disable Live Broadcast Capture",
            Category = "Video Capture Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets DisableBroadcast=1 in the VideoCapture policy key. Blocks applications "
                + "from using Windows broadcast APIs to stream game or desktop content to "
                + "external platforms (Twitch, YouTube, Beam). These broadcast sessions can "
                + "inadvertently expose corporate data if a game running on a managed device "
                + "captures an adjacent application window. "
                + "Default: 0. Recommended: 1 on corporate gaming or shared workstations.",
            Tags = ["video-capture", "broadcast", "streaming", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBroadcast", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBroadcast")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBroadcast", 1)],
        },
        new TweakDef
        {
            Id = "vcap-disable-game-capture",
            Label = "Disable Game DVR-style Video Capture",
            Category = "Video Capture Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets DisableGameCapture=1 in the VideoCapture policy key. Prevents the "
                + "GameDVR / Xbox Game Bar capture subsystem from recording gameplay video "
                + "clips and screenshots via the VideoCapture pipeline. Frees GPU encoder "
                + "headroom reserved for background capture. This policy targets the capture "
                + "backend, supplementing the GameDVR GP setting which only hides the UI. "
                + "Default: 0. Recommended: 1 on non-gaming workstations.",
            Tags = ["video-capture", "game-dvr", "xbox", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableGameCapture", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGameCapture")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGameCapture", 1)],
        },
        new TweakDef
        {
            Id = "vcap-disable-audio-capture",
            Label = "Disable Audio Capture with Video",
            Category = "Video Capture Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets DisableAudioCapture=1 in the VideoCapture policy key. Prevents "
                + "applications from pairing microphone or system-audio capture with video "
                + "capture sessions. Without audio capture, recording tools can still "
                + "capture video but produce silent clips. Reduces the exposure surface "
                + "for audio-based eavesdropping via legitimate recording applications. "
                + "Default: 0. Recommended: 1 on open-plan or regulated office seats.",
            Tags = ["video-capture", "audio", "microphone", "recording", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAudioCapture", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAudioCapture")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAudioCapture", 1)],
        },
        new TweakDef
        {
            Id = "vcap-require-admin-for-capture",
            Label = "Require Admin Rights for Video Capture",
            Category = "Video Capture Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets RequireAdminForCapture=1 in the VideoCapture policy key. Elevates "
                + "the required privilege level for video capture operations so that only "
                + "processes running with administrative rights can activate capture devices. "
                + "Standard user applications, including browser-based conferencing tools, "
                + "cannot access capture without elevation. Effective on shared machines. "
                + "Default: 0. Recommended: 1 on high-security shared workstations.",
            Tags = ["video-capture", "admin", "privilege", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireAdminForCapture", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForCapture")],
            DetectOps = [RegOp.CheckDword(Key, "RequireAdminForCapture", 1)],
        },
        new TweakDef
        {
            Id = "vcap-disable-camera-telemetry",
            Label = "Disable Camera Capture Telemetry",
            Category = "Video Capture Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableCaptureTelemetry=1 in the VideoCapture policy key. Stops "
                + "Windows from sending camera and capture device usage events to Microsoft. "
                + "These events include which applications activated the camera, session "
                + "duration, and device identifiers. The data informs Windows quality "
                + "improvements but may be unwanted on privacy-sensitive deployments. "
                + "Default: 0. Recommended: 1.",
            Tags = ["video-capture", "telemetry", "camera", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCaptureTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCaptureTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCaptureTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "vcap-disable-virtual-camera",
            Label = "Disable Virtual Camera Device Access",
            Category = "Video Capture Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets DisableVirtualCamera=1 in the VideoCapture policy key. Blocks "
                + "applications from accessing virtual camera devices installed by software "
                + "such as OBS Virtual Camera, NDI Tools, or ManyCam. Virtual cameras can "
                + "function as a transparency layer that bypasses physical camera policies "
                + "by injecting pre-recorded or manipulated video into conferencing tools. "
                + "Default: 0. Recommended: 1 on compliance-requiring conferencing setups.",
            Tags = ["video-capture", "virtual-camera", "obs", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableVirtualCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableVirtualCamera")],
            DetectOps = [RegOp.CheckDword(Key, "DisableVirtualCamera", 1)],
        },
        new TweakDef
        {
            Id = "vcap-disable-media-capture-api",
            Label = "Disable MediaCapture API for UWP Apps",
            Category = "Video Capture Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets DisableMediaCaptureAPI=1 in the VideoCapture policy key. Prevents "
                + "UWP applications from using the Windows.Media.Capture.MediaCapture API "
                + "to access camera and microphone hardware. Most modern Microsoft Store "
                + "conferencing and imaging apps use this API. Blocking it forces those apps "
                + "to request fallback devices or fail gracefully. "
                + "Default: 0. Recommended: 1 on locked-down app environments.",
            Tags = ["video-capture", "uwp", "media-capture", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMediaCaptureAPI", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMediaCaptureAPI")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMediaCaptureAPI", 1)],
        },
        new TweakDef
        {
            Id = "vcap-disable-background-capture",
            Label = "Disable Background Video Capture",
            Category = "Video Capture Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets DisableBackgroundCapture=1 in the VideoCapture policy key. Prevents "
                + "applications that have been sent to the background from continuing to "
                + "hold an active video capture session. Normally a minimised app retains "
                + "the camera even when the user switches away. This policy releases the "
                + "device when the capturing application loses focus, ensuring the camera "
                + "indicator light extinguishes. Default: 0. Recommended: 1.",
            Tags = ["video-capture", "background", "camera", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBackgroundCapture", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBackgroundCapture")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBackgroundCapture", 1)],
        },
    ];
}
