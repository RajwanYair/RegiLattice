// RegiLattice.Core — Tweaks/TeamsMeetingAudioPolicy.cs
// Microsoft Teams Meeting Audio and Media Policy — Sprint 592.
// Configures Teams meeting audio quality settings, media encryption,
// audio codec selection, noise suppression, and meeting recording policy.
// Category: "Teams Meeting Audio Policy" | Slug: tmsaud
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Office\16.0\Teams
//           HKLM\SOFTWARE\Policies\Microsoft\Teams

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TeamsMeetingAudioPolicy
{
    private const string TeamsOfficeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Teams";

    private const string TeamsPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "tmsaud-disable-bypass-local-media-optimization",
                Label = "Teams Audio: Enable Local Media Optimization (Bypass Direct Routing Media Server)",
                Category = "Teams Meeting Audio Policy",
                Description =
                    "Sets DisableLocalMediaOptimization=0 in the Teams policy key. Enables Local Media Optimization (LMO) for Teams Phone Direct Routing — when enabled, Teams routes media (audio/video) directly between the SBC (Session Border Controller) and client endpoints that are on the same network, bypassing the Teams media relay server in Microsoft Azure. This dramatically reduces latency for on-premises Teams Phone calls by keeping media traffic local instead of routing via Azure data centres thousands of miles away. LMO is the primary quality improvement for enterprises using Teams Phone with on-premises SBC infrastructure.",
                Tags = ["teams", "local-media-optimization", "direct-routing", "sbc", "latency"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Local Media Optimization enabled for Teams Direct Routing. Requires SBC configuration to support LMO (SBC vendor firmware update and topology configuration). Reduces media latency for on-premises SBC-based PSTN calls. Requires proper NAT/firewall configuration — the SBC must be reachable directly from the Teams client network on media ports.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableLocalMediaOptimization", 0)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableLocalMediaOptimization")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableLocalMediaOptimization", 0)],
            },
            new TweakDef
            {
                Id = "tmsaud-require-e2e-media-encryption",
                Label = "Teams Audio: Require End-to-End Media Encryption for All Teams Calls",
                Category = "Teams Meeting Audio Policy",
                Description =
                    "Sets RequireE2EEncryption=1 in the Teams policy key. Requires end-to-end encrypted (E2EE) audio and video for all Teams one-on-one calls. Standard Teams calls use SRTP encryption in transit (client-to-Microsoft-server), but with E2EE the encryption is applied client-to-client and the Teams server cannot decrypt the media streams. E2EE prevents a man-in-the-middle attack at the Microsoft server layer from intercepting meeting audio. E2EE calls do not support recording, transcription, PSTN access, or conference room devices — it is designed specifically for sensitive bilateral conversations.",
                Tags = ["teams", "e2e-encryption", "srtp", "media-encryption", "end-to-end"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "E2EE required for Teams calls. E2EE calls cannot use: cloud recording, live transcription, PSTN PSTN transfer, breakout rooms, or conference room (Teams Rooms) devices. Participants on older Teams clients that do not support E2EE will receive a notification that they cannot join. Only use for designated high-sensitivity teams — not as a universal policy.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "RequireE2EEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "RequireE2EEncryption")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "RequireE2EEncryption", 1)],
            },
            new TweakDef
            {
                Id = "tmsaud-enable-noise-suppression",
                Label = "Teams Audio: Enable Deep Learning-Based Noise Suppression (AI audio)",
                Category = "Teams Meeting Audio Policy",
                Description =
                    "Sets DisableNoiseSuppression=0 in the Teams policy key. Enables the Teams AI-powered noise suppression feature that uses a deep neural network (DNN) to filter background sounds during calls and meetings. The DNN model identifies non-speech audio patterns (keyboard typing, HVAC noise, office background chatter, train/plane noise) and removes them in real time before transmitting audio to other participants. Noise suppression significantly improves call quality in open-plan offices, home environments, and noisy locations.",
                Tags = ["teams", "noise-suppression", "ai-audio", "dnn", "call-quality"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Teams AI noise suppression enabled. Uses CPU cycles for real-time neural network inference (3–8% CPU on modern hardware). On low-end hardware (2-core i5, <8 GB RAM), noise suppression may cause audio processing delays. Teams can be configured per-user to use lighter suppression intensity if CPU impact is a concern.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableNoiseSuppression", 0)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableNoiseSuppression")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableNoiseSuppression", 0)],
            },
            new TweakDef
            {
                Id = "tmsaud-enable-high-fidelity-audio",
                Label = "Teams Audio: Enable High Fidelity Music Mode (48 kHz Stereo Audio Codec)",
                Category = "Teams Meeting Audio Policy",
                Description =
                    "Sets DisableHighFidelityAudio=0 in the Teams policy key. Enables High Fidelity Audio mode — Teams uses a 48 kHz stereo audio codec (OPUS stereo at ~128 kbps) instead of the default 16 kHz mono speech codec. High Fidelity Audio is critical for Teams meetings that include music playback (instrument demos, music education, virtual concerts, media production reviews) — standard speech-optimised codecs process frequencies up to 8 kHz, which makes music sound muffled. High Fidelity mode passes the full audible range to remote participants.",
                Tags = ["teams", "high-fidelity-audio", "music-mode", "opus", "48khz"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "48 kHz stereo codec enabled. Uses ~128 kbps per participant audio bandwidth (compared to ~20 kbps in standard mode). Noise suppression and echo cancellation are disabled in High Fidelity mode — not intended for regular voice meetings. This mode is only useful for music education, media review, or broadcast-style meetings.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableHighFidelityAudio", 0)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableHighFidelityAudio")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableHighFidelityAudio", 0)],
            },
            new TweakDef
            {
                Id = "tmsaud-restrict-meeting-recording-auto-retention",
                Label = "Teams Audio: Restrict Automatic Meeting Recording Retention to 60 Days",
                Category = "Teams Meeting Audio Policy",
                Description =
                    "Sets MeetingRecordingExpirationDays=60 in the Teams policy key. Sets the automatic expiration period for Teams meeting recordings to 60 days. Without an expiration policy, meeting recordings are retained indefinitely in OneDrive/SharePoint — accumulating storage at 300–500 MB per hour of recording. Recordings of regular team meetings rarely need retention beyond 60 days. Sensitive recordings, compliance recordings, and training recordings can be manually marked to retain beyond the default period. 60 days balances storage cost against ad-hoc lookback access requirements.",
                Tags = ["teams", "meeting-recording", "retention", "expiration", "storage"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Meeting recordings auto-expire after 60 days. Users receive a notification 14 days before expiry to save recordings they want to keep permanently. Recordings used for compliance, eDiscovery, or HR purposes should be manually tagged for longer retention or moved to an archived location before the 60-day limit.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "MeetingRecordingExpirationDays", 60)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "MeetingRecordingExpirationDays")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "MeetingRecordingExpirationDays", 60)],
            },
            new TweakDef
            {
                Id = "tmsaud-disable-third-party-audio-device-telemetry",
                Label = "Teams Audio: Disable Third-Party Audio Device Telemetry in Teams",
                Category = "Teams Meeting Audio Policy",
                Description =
                    "Sets DisableDeviceTelemetry=1 in the Teams policy key. Prevents Teams from transmitting audio device quality telemetry (audio hardware model, driver version, audio quality metrics, audio device firmware) to Microsoft's Teams Quality Analytics platform. While device telemetry is used by Microsoft to improve Teams audio quality diagnostics, in privacy-sensitive environments the audio hardware inventory data may be subject to data governance controls. Disabling telemetry prevents the Teams client from transmitting hardware details to third-party analytics services.",
                Tags = ["teams", "telemetry", "audio-device", "privacy", "analytics"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Audio device telemetry not transmitted to Microsoft Teams analytics. Microsoft admin centre audio quality diagnostics will have limited data for this organisation. Required Teams call quality logs (per-call QoS data) are separate from optional telemetry and are not affected.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableDeviceTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableDeviceTelemetry")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableDeviceTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "tmsaud-enable-call-quality-reporting",
                Label = "Teams Audio: Enable per-Call Quality Diagnostics Reporting to Teams Admin Centre",
                Category = "Teams Meeting Audio Policy",
                Description =
                    "Sets DisableCallQualityReporting=0 in the Teams policy key. Enables per-call quality diagnostics reporting to the Teams admin centre (Call Quality Dashboard — CQD). CQD receives per-call statistics including audio quality metrics (jitter, packet loss, round-trip time), stream quality scores, network path information, and device performance data. The CQD dashboard allows IT to identify poor call quality by building type, user group, network segment, or device model — essential for diagnosing systematic Teams audio quality problems in the enterprise network.",
                Tags = ["teams", "call-quality", "cqd", "jitter", "packet-loss"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Per-call quality metrics reported to Teams Admin Centre CQD. Call quality data includes network path, quality scores, and device identifiers — no meeting content. IT admins with Teams admin centre access can view CQD reports. CQD data is retained for 28 days by default in Microsoft 365.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableCallQualityReporting", 0)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableCallQualityReporting")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableCallQualityReporting", 0)],
            },
            new TweakDef
            {
                Id = "tmsaud-block-third-party-meeting-audio-apps",
                Label = "Teams Audio: Block Third-Party Audio App Integration in Teams Meetings",
                Category = "Teams Meeting Audio Policy",
                Description =
                    "Sets BlockThirdPartyAudioApps=1 in the Teams policy key. Prevents third-party audio applications (Krisp, RTX Voice, NVIDIA RTX Voice, Dolby Voice) from being registered or used as audio processing filters within Teams meetings. Third-party audio apps integrate with Teams via the Windows AudioGraph API or virtual audio device drivers to process mic/speaker audio. While often beneficial for noise suppression, these apps have system-level access to all audio data — in high-security environments, audio filtering apps from third-party vendors may not meet data handling requirements.",
                Tags = ["teams", "third-party-audio", "audio-filter", "krisp", "audio-app"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Third-party audio filter apps blocked in Teams. Users who rely on Krisp or NVIDIA RTX Voice for noise suppression must use Teams built-in noise suppression instead. Some users with hearing accessibility needs who use audio enhancement apps may be impacted — evaluate accessibility implications before deploying.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "BlockThirdPartyAudioApps", 1)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "BlockThirdPartyAudioApps")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "BlockThirdPartyAudioApps", 1)],
            },
            new TweakDef
            {
                Id = "tmsaud-set-dynamic-emergency-calling",
                Label = "Teams Audio: Enable Dynamic Emergency Calling (E911 Location Services)",
                Category = "Teams Meeting Audio Policy",
                Description =
                    "Sets EnableDynamicEmergencyCalling=1 in the Teams policy key. Enables dynamic emergency calling for Teams Phone — when a user dials 911 (or equivalent country emergency service number), Teams automatically determines the user's physical location based on network topology (IP subnet, wireless BSSID, chassis ID from LLDP) and sends it to the emergency service. Without dynamic emergency calling, 911 callers' locations may be registered to the main corporate headquarters address regardless of which office they are in, causing emergency responders to be dispatched to the wrong location.",
                Tags = ["teams", "e911", "emergency-calling", "location", "dynamic-e911"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "Dynamic E911 location enabled for Teams Phone. Requires Teams Phone network configuration in the Teams admin centre (Location Information Service — LIS) with subnet, wireless AP, and switch port mappings to physical addresses. Incomplete LIS configuration results in fallback to organisation address. This is a legal requirement in many jurisdictions for multi-site organisations using Teams Phone.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "EnableDynamicEmergencyCalling", 1)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "EnableDynamicEmergencyCalling")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "EnableDynamicEmergencyCalling", 1)],
            },
            new TweakDef
            {
                Id = "tmsaud-enable-compliance-recording",
                Label = "Teams Audio: Enable Compliance Recording Policy Flag (Regulatory Recording Prerequisite)",
                Category = "Teams Meeting Audio Policy",
                Description =
                    "Sets EnableComplianceRecording=1 in the Teams policy key. Sets the policy flag marking that this organisation uses Microsoft Teams Compliance Recording (a Teams certified compliance recording solution). Compliance Recording differs from regular meeting recording — it captures all calls and meetings automatically, is tamper-proof, and is retained according to the compliance policy rather than user action. In regulated industries (financial services, healthcare, legal), all communications must be recorded for regulatory compliance. Setting this flag enables the compliance recording infrastructure in the Teams client.",
                Tags = ["teams", "compliance-recording", "regulatory", "financial-services", "tamper-proof"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Compliance recording enabled. Requires a Teams certified compliance recording solution (e.g., Verint, NICE, Nuance, Microsoft Purview Communication Compliance). Users are notified that calls are recorded. Setting this flag alone does not start recording — a compliance recording bot must be configured in Teams Calling Policy in the admin centre.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "EnableComplianceRecording", 1)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "EnableComplianceRecording")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "EnableComplianceRecording", 1)],
            },
        ];
}
