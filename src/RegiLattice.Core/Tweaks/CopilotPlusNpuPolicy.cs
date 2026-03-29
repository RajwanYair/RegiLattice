// RegiLattice.Core — Tweaks/CopilotPlusNpuPolicy.cs
// Copilot+ PC NPU/AI inference, on-device model policy, and AI feature access controls — Sprint 483.
// Category: "Copilot Plus NPU Policy" | Slug: copnpu
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsAI\NPU

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CopilotPlusNpuPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI\NPU";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "copnpu-disable-npu-ai-inference",
                Label = "Disable NPU AI Inference for Copilot+ Features",
                Category = "Copilot Plus NPU Policy",
                Description =
                    "Prevents Windows from dispatching AI workloads to the Neural Processing Unit (NPU), disabling all NPU-accelerated Copilot+ features including live captions, image generation, and semantic search.",
                Tags = ["npu", "ai", "copilot-plus", "inference", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "NPU AI inference disabled; all Copilot+ AI features fall back to CPU or become unavailable.",
                ApplyOps = [RegOp.SetDword(Key, "DisableNPUInference", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNPUInference")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNPUInference", 1)],
            },
            new TweakDef
            {
                Id = "copnpu-disable-on-device-model-download",
                Label = "Block Automatic On-Device AI Model Downloads",
                Category = "Copilot Plus NPU Policy",
                Description =
                    "Prevents Windows from automatically downloading and installing on-device AI models to support Copilot+ features, stopping large background model downloads and maintaining control over on-device AI assets.",
                Tags = ["npu", "ai", "copilot-plus", "model-download", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Auto AI model downloads blocked; on-device AI features require manual model installation.",
                ApplyOps = [RegOp.SetDword(Key, "BlockModelAutoDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockModelAutoDownload")],
                DetectOps = [RegOp.CheckDword(Key, "BlockModelAutoDownload", 1)],
            },
            new TweakDef
            {
                Id = "copnpu-block-third-party-npu-apps",
                Label = "Block Third-Party Applications from Using NPU via Windows AI API",
                Category = "Copilot Plus NPU Policy",
                Description =
                    "Restricts NPU access via the Windows AI API to Microsoft-signed applications, preventing third-party apps from dispatching workloads to the NPU without IT approval.",
                Tags = ["npu", "ai", "third-party", "copilot-plus", "api", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Third-party NPU access blocked; only Microsoft-signed apps can use NPU via Windows AI API.",
                ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyNPUAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyNPUAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyNPUAccess", 1)],
            },
            new TweakDef
            {
                Id = "copnpu-limit-npu-power-budget",
                Label = "Limit NPU Sustained Power Budget",
                Category = "Copilot Plus NPU Policy",
                Description =
                    "Applies a sustained power cap to NPU AI workloads, limiting continuous NPU compute consumption to prevent the AI inference engine from monopolising power and thermal headroom on low-power form factors.",
                Tags = ["npu", "ai", "power", "copilot-plus", "thermal", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NPU power budget limited; AI inference slower but system remains responsive under sustained load.",
                ApplyOps = [RegOp.SetDword(Key, "LimitNPUPowerBudget", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LimitNPUPowerBudget")],
                DetectOps = [RegOp.CheckDword(Key, "LimitNPUPowerBudget", 1)],
            },
            new TweakDef
            {
                Id = "copnpu-disable-live-captions-ai",
                Label = "Disable AI-Powered Live Captions (NPU)",
                Category = "Copilot Plus NPU Policy",
                Description =
                    "Disables the NPU-accelerated live captions feature that transcribes all system audio in real time, preventing on-device audio transcription and the retention of audio content in the captions log.",
                Tags = ["npu", "ai", "live-captions", "audio", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI live captions disabled; real-time audio transcription feature unavailable.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLiveCaptionsAI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLiveCaptionsAI")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLiveCaptionsAI", 1)],
            },
            new TweakDef
            {
                Id = "copnpu-disable-ai-image-generation",
                Label = "Disable On-Device AI Image Generation (Cocreator)",
                Category = "Copilot Plus NPU Policy",
                Description =
                    "Disables the Cocreator on-device AI image generation feature in Paint and other apps that uses the NPU to generate images from text prompts, preventing local AI image synthesis.",
                Tags = ["npu", "ai", "image-generation", "cocreator", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI image generation disabled; Cocreator and on-device Stable Diffusion features unavailable.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAIImageGeneration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAIImageGeneration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAIImageGeneration", 1)],
            },
            new TweakDef
            {
                Id = "copnpu-disable-ai-super-resolution",
                Label = "Disable AI Super Resolution (Automatic SR)",
                Category = "Copilot Plus NPU Policy",
                Description =
                    "Disables the NPU-based AI Super Resolution feature that upscales games and video content using on-device neural inference, reducing constant NPU utilisation during media playback.",
                Tags = ["npu", "ai", "super-resolution", "video", "gaming", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "AI super resolution disabled; no NPU-accelerated video upscaling. GPU handles rendering at native resolution.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAISuperResolution", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAISuperResolution")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAISuperResolution", 1)],
            },
            new TweakDef
            {
                Id = "copnpu-disable-windows-studio-effects",
                Label = "Disable Windows Studio AI Effects (Background Blur, Eye Contact)",
                Category = "Copilot Plus NPU Policy",
                Description =
                    "Disables all Windows Studio Effects powered by the NPU, including background blur, portrait mode, voice focus, and eye contact correction for video calls, freeing NPU resources.",
                Tags = ["npu", "ai", "studio-effects", "camera", "video-call", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Windows Studio Effects disabled; no AI background blur, eye contact, or portrait mode in video calls.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWindowsStudioEffects", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsStudioEffects")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWindowsStudioEffects", 1)],
            },
            new TweakDef
            {
                Id = "copnpu-disable-ai-text-actions",
                Label = "Disable AI Text Actions (Smart Selection, Rewrite, Summary)",
                Category = "Copilot Plus NPU Policy",
                Description =
                    "Disables AI Text Actions powered by the NPU — including smart text selection, AI-powered rewrite suggestions, and text summarisation in apps — preventing on-device text analysis.",
                Tags = ["npu", "ai", "text-actions", "smart-selection", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI text actions disabled; smart selection, AI rewrite, and text summary features unavailable.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAITextActions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAITextActions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAITextActions", 1)],
            },
            new TweakDef
            {
                Id = "copnpu-audit-npu-workload-dispatch",
                Label = "Enable Audit Logging for NPU AI Workload Dispatches",
                Category = "Copilot Plus NPU Policy",
                Description =
                    "Enables Windows event logging for NPU AI workload dispatch events including which application requested NPU inference and the associated inference model name.",
                Tags = ["npu", "ai", "audit-log", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NPU workload dispatches logged; apps accessing the NPU are auditable.",
                ApplyOps = [RegOp.SetDword(Key, "AuditNPUWorkloadDispatch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditNPUWorkloadDispatch")],
                DetectOps = [RegOp.CheckDword(Key, "AuditNPUWorkloadDispatch", 1)],
            },
        ];
}
