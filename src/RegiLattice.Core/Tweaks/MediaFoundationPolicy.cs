// RegiLattice.Core — Tweaks/MediaFoundationPolicy.cs
// Sprint 264: Media Foundation Group Policy (10 tweaks)
// Category: "Media Foundation Policy" | Slug: mfa
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\MediaFoundation

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MediaFoundationPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\MediaFoundation";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "mfa-disable-frame-server",
            Label = "Disable Media Foundation Frame Server Mode",
            Category = "Media Foundation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Sets EnableFrameServerMode=0 in the MediaFoundation policy key. Disables "
                + "the Camera Frame Server service which routes camera frames through a "
                + "central broker process to multiple applications simultaneously. When "
                + "disabled each camera consumer interacts with the device driver directly. "
                + "May improve capture latency for single-consumer scenarios. Default: Frame "
                + "Server Mode is enabled. Caution: disabling breaks multi-app camera sharing.",
            Tags = ["media", "camera", "frame-server", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableFrameServerMode", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableFrameServerMode")],
            DetectOps = [RegOp.CheckDword(Key, "EnableFrameServerMode", 0)],
        },
        new TweakDef
        {
            Id = "mfa-block-untrusted-codecs",
            Label = "Block Untrusted Media Codecs",
            Category = "Media Foundation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Sets AllowUntrustedMediaCodecs=0 in the MediaFoundation policy key. "
                + "Prevents Media Foundation from loading third-party or unsigned codec "
                + "DLLs that have not been validated by Windows. Reduces attack surface "
                + "by blocking malicious codecs that exploit media parsing vulnerabilities. "
                + "Default: untrusted codecs may be loaded. Recommended: 0 on corporate "
                + "machines to harden against codec-based exploits.",
            Tags = ["media", "codecs", "security", "hardening", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowUntrustedMediaCodecs", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowUntrustedMediaCodecs")],
            DetectOps = [RegOp.CheckDword(Key, "AllowUntrustedMediaCodecs", 0)],
        },
        new TweakDef
        {
            Id = "mfa-disable-hw-acceleration",
            Label = "Disable Media Foundation Hardware Acceleration",
            Category = "Media Foundation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets DisableHardwareAcceleration=1 in the MediaFoundation policy key. "
                + "Forces Media Foundation to use software-only decoding and encoding "
                + "pipelines, bypassing hardware video acceleration (DXVA2, D3D11VA). "
                + "Useful for diagnosing GPU-related media playback failures or ensuring "
                + "deterministic behaviour in virtual machine environments. Default: "
                + "hardware acceleration is used when available.",
            Tags = ["media", "hardware-acceleration", "gpu", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableHardwareAcceleration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableHardwareAcceleration")],
            DetectOps = [RegOp.CheckDword(Key, "DisableHardwareAcceleration", 1)],
        },
        new TweakDef
        {
            Id = "mfa-disable-transcoding",
            Label = "Disable Media Foundation Transcoding",
            Category = "Media Foundation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets EnableTranscoding=0 in the MediaFoundation policy key. Disables the "
                + "Media Foundation transcoding APIs that allow applications to re-encode "
                + "media between formats. Prevents unauthorised format conversion of "
                + "protected media files and reduces the attack surface of the MF pipeline. "
                + "Default: transcoding is enabled. May affect media editing and streaming "
                + "applications that rely on the MF transcoding APIs.",
            Tags = ["media", "transcoding", "security", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableTranscoding", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableTranscoding")],
            DetectOps = [RegOp.CheckDword(Key, "EnableTranscoding", 0)],
        },
        new TweakDef
        {
            Id = "mfa-disable-protected-content",
            Label = "Disable Protected Media Content Playback",
            Category = "Media Foundation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets AllowProtectedContentPlayback=0 in the MediaFoundation policy key. "
                + "Prevents applications from playing DRM-protected media through the "
                + "Media Foundation Protected Media Path (PMP). Blocks playback of "
                + "encrypted streaming content, Blu-ray, and DRM-protected audio. "
                + "Default: protected content playback is allowed. Recommended: 0 "
                + "on server or locked-down machines where DRM clients are not required.",
            Tags = ["media", "drm", "protected-content", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowProtectedContentPlayback", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowProtectedContentPlayback")],
            DetectOps = [RegOp.CheckDword(Key, "AllowProtectedContentPlayback", 0)],
        },
        new TweakDef
        {
            Id = "mfa-disable-network-streaming",
            Label = "Disable Media Foundation Network Streaming",
            Category = "Media Foundation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets EnableNetworkStreaming=0 in the MediaFoundation policy key. Prevents "
                + "Media Foundation from opening network-based media sources (HTTP/MMS/RTSP). "
                + "Eliminates the media streaming attack surface and stops applications "
                + "from silently streaming content over the network via the MF pipeline. "
                + "Default: network streaming is enabled. Side effect: media player apps "
                + "that rely on MF for HTTP streaming will fail to open remote URLs.",
            Tags = ["media", "network", "streaming", "security", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableNetworkStreaming", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableNetworkStreaming")],
            DetectOps = [RegOp.CheckDword(Key, "EnableNetworkStreaming", 0)],
        },
        new TweakDef
        {
            Id = "mfa-disable-codec-downloads",
            Label = "Disable Automatic Codec Downloads",
            Category = "Media Foundation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets AllowAutomaticCodecDownloads=0 in the MediaFoundation policy key. "
                + "Prevents Media Foundation from automatically downloading and installing "
                + "missing codecs from the internet when a media file requires a decoder not "
                + "currently installed. Ensures no unaudited binaries are fetched and "
                + "installed. Default: automatic codec downloads are permitted.",
            Tags = ["media", "codecs", "downloads", "security", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowAutomaticCodecDownloads", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowAutomaticCodecDownloads")],
            DetectOps = [RegOp.CheckDword(Key, "AllowAutomaticCodecDownloads", 0)],
        },
        new TweakDef
        {
            Id = "mfa-disable-media-sharing",
            Label = "Disable Media Foundation Sharing APIs",
            Category = "Media Foundation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets AllowMediaSharing=0 in the MediaFoundation policy key. Disables "
                + "the Media Foundation APIs that allow applications to share media "
                + "pipeline resources such as device handles and decoder instances. "
                + "Reduces cross-process media data access. Default: sharing APIs are "
                + "active. Recommended: 0 on privacy-focused workstations.",
            Tags = ["media", "sharing", "privacy", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowMediaSharing", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowMediaSharing")],
            DetectOps = [RegOp.CheckDword(Key, "AllowMediaSharing", 0)],
        },
        new TweakDef
        {
            Id = "mfa-disable-drm-individualization",
            Label = "Disable DRM Individualization",
            Category = "Media Foundation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets AllowDRMIndividualization=0 in the MediaFoundation policy key. "
                + "Prevents the DRM subsystem from performing an individualization "
                + "handshake with Microsoft servers, which uniquely identifies the "
                + "device for license enforcement. Blocks the associated network call "
                + "and stops machine-ID data from being sent to Microsoft DRM servers. "
                + "Default: individualization is allowed. May break PlayReady-protected "
                + "content playback.",
            Tags = ["media", "drm", "privacy", "telemetry", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowDRMIndividualization", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowDRMIndividualization")],
            DetectOps = [RegOp.CheckDword(Key, "AllowDRMIndividualization", 0)],
        },
        new TweakDef
        {
            Id = "mfa-disable-mf-telemetry",
            Label = "Disable Media Foundation Telemetry",
            Category = "Media Foundation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets EnableMFTelemetry=0 in the MediaFoundation policy key. Stops "
                + "Media Foundation from collecting and uploading diagnostic and usage "
                + "telemetry about media playback sessions to Microsoft. Includes data "
                + "such as codec names, resolution, frame rates, and error codes. "
                + "Default: telemetry is collected when Windows telemetry is active. "
                + "Recommended: 0 on privacy-hardened deployments.",
            Tags = ["media", "telemetry", "privacy", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableMFTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMFTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMFTelemetry", 0)],
        },
    ];
}
