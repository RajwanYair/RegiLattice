// RegiLattice.Core — Tweaks/AiInferencePolicy.cs
// AI Inference Policy — Sprint 552.
// Configures Group Policy for AI inference on Windows: NPU/neural processing
// settings, AI model execution restrictions, inference sandbox controls,
// and hardware AI acceleration governance.
// Category: "AI Inference Policy" | Slug: aiinf
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\AI

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AiInferencePolicy
{
    private const string AiKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AI";

    private const string AiInfKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AI\Inference";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "aiinf-disable-cloud-ai-processing",
                Label = "AI Inference: Disable Cloud AI Processing for Local Tasks",
                Category = "AI Inference Policy",
                Description =
                    "Sets DisableCloudAI=1 in AI Inference policy. Prevents Windows AI platform from offloading inference workloads to Microsoft cloud AI services when local NPU/GPU resources are insufficient. Cloud inference sends data to remote servers for processing. For organisations with data sovereignty requirements or classification-level data restrictions, local-only inference ensures sensitive content (documents, images, voice) processed by AI features never leaves the device boundary.",
                Tags = ["ai", "inference", "cloud", "data-sovereignty", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "AI features fall back to local inference only. On devices without NPU/powerful GPU, some AI features may be slower or disabled. Required for classified/sensitive data environments.",
                ApplyOps = [RegOp.SetDword(AiInfKey, "DisableCloudAI", 1)],
                RemoveOps = [RegOp.DeleteValue(AiInfKey, "DisableCloudAI")],
                DetectOps = [RegOp.CheckDword(AiInfKey, "DisableCloudAI", 1)],
            },
            new TweakDef
            {
                Id = "aiinf-enable-model-integrity-check",
                Label = "AI Inference: Enable AI Model Signature Integrity Verification",
                Category = "AI Inference Policy",
                Description =
                    "Sets ModelIntegrityCheck=1 in AI Inference policy. Requires that AI model files (.onnx, .tflite, etc.) loaded by the Windows inference runtime have a valid Authenticode-compatible hash or signature before execution. Unsigned or hash-mismatched AI models are rejected. This prevents adversarial model substitution: an attacker who compromises the model store directory cannot inject a backdoored model that produces biased outputs or exfiltrates data through the model's inference calls.",
                Tags = ["ai", "inference", "model", "integrity", "signature"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "AI models must have valid signatures. Only Microsoft-signed or enterprise-signed models load. Custom or third-party unsigned models are blocked.",
                ApplyOps = [RegOp.SetDword(AiInfKey, "ModelIntegrityCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(AiInfKey, "ModelIntegrityCheck")],
                DetectOps = [RegOp.CheckDword(AiInfKey, "ModelIntegrityCheck", 1)],
            },
            new TweakDef
            {
                Id = "aiinf-disable-ai-feature-advertising",
                Label = "AI Inference: Disable AI Feature Discovery Advertising to Users",
                Category = "AI Inference Policy",
                Description =
                    "Sets DisableAIFeatureAdvertising=1 in AI policy. Suppresses in-product advertising, new feature banners, and onboarding prompts for AI-powered features across Windows and Microsoft 365 applications. In managed enterprise environments, feature rollout is controlled by IT through product update policies; individual per-user feature advertising can lead to adoption of unapproved AI tools, inconsistent compliance posture, and confusion about which AI features are enterprise-approved.",
                Tags = ["ai", "advertising", "notifications", "enterprise", "managed"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "AI feature ads and onboarding prompts are suppressed. Only affects promotional UI, not functionality. AI features that are enabled via policy continue to work.",
                ApplyOps = [RegOp.SetDword(AiKey, "DisableAIFeatureAdvertising", 1)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "DisableAIFeatureAdvertising")],
                DetectOps = [RegOp.CheckDword(AiKey, "DisableAIFeatureAdvertising", 1)],
            },
            new TweakDef
            {
                Id = "aiinf-restrict-third-party-models",
                Label = "AI Inference: Restrict Third-Party AI Model Loading",
                Category = "AI Inference Policy",
                Description =
                    "Sets AllowThirdPartyModels=0 in AI Inference policy. Prevents the Windows AI inference runtime from loading AI models from third-party sources or user-installed model packages. Only Microsoft-provided and enterprise-published AI models are permitted. Restricting to approved models prevents supply-chain attacks through AI model distribution channels: a compromised third-party AI model could exfiltrate data through the inference API or produce harmful content.",
                Tags = ["ai", "models", "third-party", "supply-chain", "restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Third-party AI models are blocked. Only Microsoft-provided and enterprise-published models load. Custom machine learning workflows using third-party models are blocked at the OS level.",
                ApplyOps = [RegOp.SetDword(AiInfKey, "AllowThirdPartyModels", 0)],
                RemoveOps = [RegOp.DeleteValue(AiInfKey, "AllowThirdPartyModels")],
                DetectOps = [RegOp.CheckDword(AiInfKey, "AllowThirdPartyModels", 0)],
            },
            new TweakDef
            {
                Id = "aiinf-enable-inference-audit-log",
                Label = "AI Inference: Enable Inference Operation Audit Logging",
                Category = "AI Inference Policy",
                Description =
                    "Sets EnableInferenceAudit=1 in AI Inference policy. Enables the Windows AI inference platform to emit audit events to the Windows Security Event Log when AI inference operations are performed: which model ran, input data class, output class, caller application, timestamp. This creates an audit trail for AI processing activity on the endpoint, supporting AI governance requirements: understanding what AI operations occurred, on what data, by which applications. Essential for regulated industries adopting AI features.",
                Tags = ["ai", "audit", "inference", "logging", "governance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI inference audit events are written to Event Log. Storage overhead depends on AI feature usage frequency. Essential for AI governance and compliance reporting.",
                ApplyOps = [RegOp.SetDword(AiInfKey, "EnableInferenceAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(AiInfKey, "EnableInferenceAudit")],
                DetectOps = [RegOp.CheckDword(AiInfKey, "EnableInferenceAudit", 1)],
            },
            new TweakDef
            {
                Id = "aiinf-disable-ai-telemetry",
                Label = "AI Inference: Disable AI Inference Platform Telemetry",
                Category = "AI Inference Policy",
                Description =
                    "Sets DisableAITelemetry=1 in AI Inference policy. Prevents the Windows AI inference runtime from transmitting performance telemetry, model usage statistics, and inference diagnostics to Microsoft's telemetry endpoints. The AI inference telemetry stream includes which models are run, frequency of inference calls, device capability benchmark data, and aggregate performance metrics. Disabling AI-specific telemetry complements the general telemetry restriction for environments with strict data transmission policies.",
                Tags = ["ai", "telemetry", "privacy", "inference", "data-collection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI inference telemetry is disabled. No model usage data is sent to Microsoft. Does not affect general Windows telemetry (controlled separately). AI features continue to function.",
                ApplyOps = [RegOp.SetDword(AiInfKey, "DisableAITelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(AiInfKey, "DisableAITelemetry")],
                DetectOps = [RegOp.CheckDword(AiInfKey, "DisableAITelemetry", 1)],
            },
            new TweakDef
            {
                Id = "aiinf-set-gpu-memory-limit",
                Label = "AI Inference: Limit AI Inference GPU Memory to 2 GB",
                Category = "AI Inference Policy",
                Description =
                    "Sets MaxGpuMemoryMB=2048 in AI Inference policy. Caps the amount of GPU video memory that the Windows AI inference runtime allocates for model loading and inference operations. Without a memory cap, a background AI feature (e.g., Windows Live Captions with a neural processing model) can consume the majority of GPU VRAM, degrading performance for foreground applications (particularly games, video editing, CAD). Setting a 2 GB limit ensures AI inference coexists with GPU-intensive workloads.",
                Tags = ["ai", "gpu", "memory", "performance", "inference"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI inference is limited to 2 GB GPU memory. Large models that require more VRAM fall back to CPU/NPU inference. On systems with 4+ GB VRAM, increase this limit if AI accuracy is affected.",
                ApplyOps = [RegOp.SetDword(AiInfKey, "MaxGpuMemoryMB", 2048)],
                RemoveOps = [RegOp.DeleteValue(AiInfKey, "MaxGpuMemoryMB")],
                DetectOps = [RegOp.CheckDword(AiInfKey, "MaxGpuMemoryMB", 2048)],
            },
            new TweakDef
            {
                Id = "aiinf-disable-ai-background-processing",
                Label = "AI Inference: Disable AI Background Inference Processing",
                Category = "AI Inference Policy",
                Description =
                    "Sets DisableBackgroundAI=1 in AI Inference policy. Prevents AI inference models from running in the background when the device is on battery or when no interactive user session is present. Background AI processing (e.g., pre-fetching inference results, updating cached embeddings for Windows Search) consumes NPU/GPU power and battery. Disabling background AI inference extends battery life and reduces thermal load on portable devices, at the cost of slightly higher first-run inference latency.",
                Tags = ["ai", "background", "battery", "inference", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Background AI inference is disabled. AI features are only processed when the user actively uses them. First-time invocation latency increases. Recommended for portable devices.",
                ApplyOps = [RegOp.SetDword(AiInfKey, "DisableBackgroundAI", 1)],
                RemoveOps = [RegOp.DeleteValue(AiInfKey, "DisableBackgroundAI")],
                DetectOps = [RegOp.CheckDword(AiInfKey, "DisableBackgroundAI", 1)],
            },
            new TweakDef
            {
                Id = "aiinf-enable-sandboxed-inference",
                Label = "AI Inference: Enable Sandboxed AI Inference Execution",
                Category = "AI Inference Policy",
                Description =
                    "Sets EnableInferenceSandbox=1 in AI Inference policy. Activates Windows AI inference sandbox mode, which runs inference operations in a restricted execution environment with limited filesystem and network access. Sandboxed inference prevents AI models from making outbound network calls, reading arbitrary user files, or writing to locations outside the designated model output directories. Provides defence-in-depth against AI model exfiltration attacks and prompt injection that attempts to leverage the inference runtime's host process capabilities.",
                Tags = ["ai", "sandbox", "inference", "isolation", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "AI inference runs in a sandboxed environment. Models with filesystem dependencies may fail (e.g., image-to-file save operations). Test AI features after enabling — sandboxing prevents models from accessing user documents.",
                ApplyOps = [RegOp.SetDword(AiInfKey, "EnableInferenceSandbox", 1)],
                RemoveOps = [RegOp.DeleteValue(AiInfKey, "EnableInferenceSandbox")],
                DetectOps = [RegOp.CheckDword(AiInfKey, "EnableInferenceSandbox", 1)],
            },
            new TweakDef
            {
                Id = "aiinf-disable-ai-personalisation",
                Label = "AI Inference: Disable AI Personalisation and User Context Collection",
                Category = "AI Inference Policy",
                Description =
                    "Sets DisableAIPersonalisation=1 in AI policy. Prevents AI features from accessing user history, documents, emails, and calendar data to 'personalise' AI responses and recommendations. AI personalisation builds a local semantic index of user activities to improve inference quality. In enterprise environments, this data collection must comply with privacy policies: employee communications and documents should not feed AI personalisation models without explicit consent. Disabling personalisation may reduce inference relevance but ensures no user context is indexed.",
                Tags = ["ai", "personalisation", "privacy", "user-data", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI personalisation is disabled. AI features respond without user-specific context — answers are more generic. Semantic search and AI-suggested completions are less precise.",
                ApplyOps = [RegOp.SetDword(AiKey, "DisableAIPersonalisation", 1)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "DisableAIPersonalisation")],
                DetectOps = [RegOp.CheckDword(AiKey, "DisableAIPersonalisation", 1)],
            },
        ];
}
