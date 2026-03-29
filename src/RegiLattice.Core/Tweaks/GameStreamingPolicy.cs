// RegiLattice.Core — Tweaks/GameStreamingPolicy.cs
// Xbox / Windows game streaming and remote play Group Policy controls (Sprint 603).
// Category: "Game Streaming Policy" | Slug: gstream
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR (streaming-specific values)
//      + HKLM\SOFTWARE\Policies\Microsoft\Windows\GameInput

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class GameStreamingPolicy
{
    private const string DvrKey   = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR";
    private const string InputKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameInput";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "gstream-block-xbox-remote-play",
            Label = "Game Stream: Block Xbox Remote Play From This Device",
            Category = "Game Streaming Policy",
            Description = "Sets AllowXboxRemotePlay=0 in GameDVR policy. Prevents users from streaming their Xbox console games to this Windows PC via the Xbox Remote Play feature. " +
                "Xbox Remote Play creates a persistent streaming session between the PC and an Xbox console over the internet. On enterprise devices, this introduces an unmanaged high-bandwidth consumer service into the corporate network and may allow the Xbox console (outside the corporate network perimeter) to stream audio/video captured by the device's microphone and camera back to it.",
            Tags = ["gaming", "streaming", "remote-play", "xbox", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks Xbox Remote Play streaming sessions; prevents unsanctioned remote device connectivity.",
            ApplyOps = [RegOp.SetDword(DvrKey, "AllowXboxRemotePlay", 0)],
            RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowXboxRemotePlay")],
            DetectOps = [RegOp.CheckDword(DvrKey, "AllowXboxRemotePlay", 0)],
        },
        new TweakDef
        {
            Id = "gstream-block-game-streaming-from-pc",
            Label = "Game Stream: Block Streaming PC Games to Other Devices",
            Category = "Game Streaming Policy",
            Description = "Sets AllowGameStreamingFromPC=0 in GameDVR policy. Prevents games running on this PC from being streamed to another device (e.g., to a browser, mobile device, or other PC) via Xbox-based or Miracast streaming. " +
                "PC game streaming renders GPU frames and encodes video of potentially sensitive content visible on the desktop, which is then transmitted across the network. Blocking streaming prevents screen content from being sent to uncontrolled endpoint devices.",
            Tags = ["gaming", "streaming", "screen-capture", "pc", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks PC game streaming; desktop video not transmitted to other devices via Xbox streaming.",
            ApplyOps = [RegOp.SetDword(DvrKey, "AllowGameStreamingFromPC", 0)],
            RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowGameStreamingFromPC")],
            DetectOps = [RegOp.CheckDword(DvrKey, "AllowGameStreamingFromPC", 0)],
        },
        new TweakDef
        {
            Id = "gstream-block-game-streaming-upload",
            Label = "Game Stream: Block Streaming Video Upload to Xbox Network",
            Category = "Game Streaming Policy",
            Description = "Sets AllowGameStreamingUpload=0 in GameDVR policy. Prevents captured game footage and streaming session recordings from being uploaded to Xbox Live's video hosting service. " +
                "Captured game clips that are uploaded to Xbox Live become publicly accessible (or accessible to the user's Xbox friends list). On managed devices, preventing video content capture and upload stops potential accidental disclosure of sensitive information visible on the screen (corporate applications, documents, chat windows) that appear in the game capture frame.",
            Tags = ["gaming", "streaming", "upload", "video", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks game video upload to Xbox Live; screen content not sent to external cloud.",
            ApplyOps = [RegOp.SetDword(DvrKey, "AllowGameStreamingUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowGameStreamingUpload")],
            DetectOps = [RegOp.CheckDword(DvrKey, "AllowGameStreamingUpload", 0)],
        },
        new TweakDef
        {
            Id = "gstream-block-game-input-service-telemetry",
            Label = "Game Stream: Block GameInput Service Telemetry Upload",
            Category = "Game Streaming Policy",
            Description = "Sets DisableTelemetry=1 in GameInput machine policy. Prevents the GameInput API service from sending telemetry about controller, keyboard, and mouse input events to Microsoft. " +
                "The GameInput service collects keystroke timing, button press frequency, and peripheral usage patterns from connected game input devices. On corporate workstations, this input telemetry includes potentially sensitive information about productivity application usage patterns that happen to be captured via the same keyboard or mouse.",
            Tags = ["gaming", "input", "telemetry", "keyboard", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks GameInput API telemetry; controller/keyboard usage data not sent to Microsoft.",
            ApplyOps = [RegOp.SetDword(InputKey, "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(InputKey, "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(InputKey, "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "gstream-block-game-input-cloud-config",
            Label = "Game Stream: Block GameInput Cloud Configuration Sync",
            Category = "Game Streaming Policy",
            Description = "Sets DisableCloudConfig=1 in GameInput machine policy. Prevents the GameInput service from downloading controller mapping configurations, button remapping profiles, and vibration settings from Microsoft's cloud game-input configuration service. " +
                "Cloud config sync for GameInput connects to an external endpoint at startup to check for updated controller profiles. This is an outbound network call to a Microsoft-controlled cloud endpoint that occurs automatically. On locked-down or air-gapped environments, any automatic cloud-sync mechanism should be disabled.",
            Tags = ["gaming", "input", "cloud-sync", "config", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks GameInput cloud config sync; controller profiles not auto-updated from Microsoft cloud.",
            ApplyOps = [RegOp.SetDword(InputKey, "DisableCloudConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(InputKey, "DisableCloudConfig")],
            DetectOps = [RegOp.CheckDword(InputKey, "DisableCloudConfig", 1)],
        },
        new TweakDef
        {
            Id = "gstream-disable-streaming-microphone-access",
            Label = "Game Stream: Disable Microphone Access for Game Streaming",
            Category = "Game Streaming Policy",
            Description = "Sets AllowMicrophoneAccess=0 in GameDVR policy. Prevents game streaming sessions and game capture from accessing the device's microphone. " +
                "Game streaming with microphone access enables audio capture of the room environment, which is a significant privacy and security concern on corporate devices. Disabling mic access for game streaming ensures that voice communication via Xbox streaming channels cannot intercept ambient conversations regardless of whether the user intentionally activates party chat.",
            Tags = ["gaming", "microphone", "privacy", "audio", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks microphone access for game streaming; ambient audio cannot be captured via Xbox streaming.",
            ApplyOps = [RegOp.SetDword(DvrKey, "AllowMicrophoneAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowMicrophoneAccess")],
            DetectOps = [RegOp.CheckDword(DvrKey, "AllowMicrophoneAccess", 0)],
        },
        new TweakDef
        {
            Id = "gstream-disable-streaming-camera-access",
            Label = "Game Stream: Disable Camera Access for Game Streaming",
            Category = "Game Streaming Policy",
            Description = "Sets AllowCameraAccess=0 in GameDVR policy. Prevents game streaming sessions from accessing the device's webcam or front-facing camera. " +
                "Game streaming with camera access enables live video capture of the user's physical environment. This creates a serious privacy risk on enterprise devices, where the room visible to the camera may contain whiteboards with sensitive information, other screens, or colleagues who have not consented to being recorded.",
            Tags = ["gaming", "camera", "webcam", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks webcam access for game streaming; no live video capture via Xbox streaming channels.",
            ApplyOps = [RegOp.SetDword(DvrKey, "AllowCameraAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowCameraAccess")],
            DetectOps = [RegOp.CheckDword(DvrKey, "AllowCameraAccess", 0)],
        },
        new TweakDef
        {
            Id = "gstream-limit-streaming-resolution-1080p",
            Label = "Game Stream: Limit Outbound Game Streaming Resolution to 1080p",
            Category = "Game Streaming Policy",
            Description = "Sets MaxStreamingResolution=2 in GameDVR policy. Caps outbound game streaming resolution at 1080p (1920×1080), preventing 4K or higher resolution game stream encoding. " +
                "4K game streaming at 60 fps can consume 40–80 Mbps of bandwidth, which severely degrades other users on shared corporate network links. Capping at 1080p limits peak streaming bandwidth to ~15–20 Mbps, reducing network impact for the rare scenario where streaming is permitted within a managed environment's acceptable-use policy.",
            Tags = ["gaming", "streaming", "resolution", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "1080p streaming cap; reduces peak streaming bandwidth from ~80 Mbps (4K) to ~20 Mbps.",
            ApplyOps = [RegOp.SetDword(DvrKey, "MaxStreamingResolution", 2)],
            RemoveOps = [RegOp.DeleteValue(DvrKey, "MaxStreamingResolution")],
            DetectOps = [RegOp.CheckDword(DvrKey, "MaxStreamingResolution", 2)],
        },
        new TweakDef
        {
            Id = "gstream-disable-stream-session-auto-reconnect",
            Label = "Game Stream: Disable Automatic Reconnection for Streaming Sessions",
            Category = "Game Streaming Policy",
            Description = "Sets AllowAutoReconnect=0 in GameDVR policy. Prevents game streaming sessions from automatically and silently reconnecting after a network interruption. " +
                "Auto-reconnect for streaming sessions can cause the device to establish a new streaming connection in the background (potentially after hours, if a session was left active) without the user's knowledge. This background reconnect may re-activate microphone and camera capture unexpectedly. Requiring manual reconnection ensures the user is aware of and actively initiating each streaming session.",
            Tags = ["gaming", "streaming", "reconnect", "background", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents background streaming reconnect; each session requires explicit user initiation.",
            ApplyOps = [RegOp.SetDword(DvrKey, "AllowAutoReconnect", 0)],
            RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowAutoReconnect")],
            DetectOps = [RegOp.CheckDword(DvrKey, "AllowAutoReconnect", 0)],
        },
        new TweakDef
        {
            Id = "gstream-disable-game-input-accessibility-overlay",
            Label = "Game Stream: Disable GameInput Accessibility Overlay in Streaming",
            Category = "Game Streaming Policy",
            Description = "Sets DisableAccessibilityOverlay=1 in GameInput machine policy. Prevents the GameInput accessibility overlay (on-screen virtual controller, input visualiser) from appearing during game streaming sessions. " +
                "The GameInput accessibility overlay injects screen content via a system-level overlay that is captured by game recording and streaming functions. On streaming sessions, this overlay appears in the video feed received by all viewers. On corporate devices, ensuring that no system-level overlays inject unexpected UI into game-streamed frames is part of content control.",
            Tags = ["gaming", "input", "overlay", "accessibility", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Hides GameInput accessibility overlay in streamed video; clean streaming output without injected overlays.",
            ApplyOps = [RegOp.SetDword(InputKey, "DisableAccessibilityOverlay", 1)],
            RemoveOps = [RegOp.DeleteValue(InputKey, "DisableAccessibilityOverlay")],
            DetectOps = [RegOp.CheckDword(InputKey, "DisableAccessibilityOverlay", 1)],
        },
    ];
}
