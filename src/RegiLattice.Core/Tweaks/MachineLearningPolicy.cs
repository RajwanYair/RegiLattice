// RegiLattice.Core — Tweaks/MachineLearningPolicy.cs
// Machine Learning Platform Policy — Sprint 556.
// Configures Group Policy for Windows Machine Learning (WinML) platform and
// ONNX runtime: model build cache, GPU DirectML access, WinML API governance,
// and experimental model feature restrictions.
// Category: "Machine Learning Policy" | Slug: mlpol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\MachineLearning

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MachineLearningPolicy
{
    private const string MlKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\MachineLearning";

    private const string OnnxKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\MachineLearning\ONNX";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "mlpol-enable-winml-telemetry-logging",
                Label = "Machine Learning: Enable WinML Runtime Telemetry for Diagnostics",
                Category = "Machine Learning Policy",
                Description =
                    "Sets WinMLTelemetryEnabled=1 in MachineLearning policy. Enables structured diagnostic telemetry from the Windows Machine Learning runtime to a local Event Log channel. This diagnostic telemetry includes WinML API call traces, model loading times, inference session metrics, and error traces. Unlike cloud telemetry (controlled by the Telemetry policy), this writes diagnostic data locally and is used by IT support teams to diagnose WinML-based application failures without requiring remote access to Microsoft's telemetry backend.",
                Tags = ["machine-learning", "winml", "telemetry", "diagnostics", "logging"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WinML diagnostic events are written to local Event Log. Does not send data externally. Required for diagnosing WinML application failures in enterprise support scenarios.",
                ApplyOps = [RegOp.SetDword(MlKey, "WinMLTelemetryEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(MlKey, "WinMLTelemetryEnabled")],
                DetectOps = [RegOp.CheckDword(MlKey, "WinMLTelemetryEnabled", 1)],
            },
            new TweakDef
            {
                Id = "mlpol-enable-onnx-quantisation",
                Label = "Machine Learning: Enable ONNX Model Quantisation for Performance",
                Category = "Machine Learning Policy",
                Description =
                    "Sets ONNXQuantisationEnabled=1 in ONNX policy. Allows the ONNX runtime to load INT8/INT4 quantised model variants when available for a given model. Quantised models are 2–4× smaller and run 2–4× faster on CPU/NPU compared to FP32 models, with minimal accuracy loss for most inference tasks. Enabling quantisation makes AI features significantly more responsive on devices without a discrete GPU, and extends battery life for inference on portable devices. Quantised models must be pre-compiled and provided by the model developer.",
                Tags = ["machine-learning", "onnx", "quantisation", "performance", "ai"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "ONNX quantised model variants are used when available. Output quality may marginally differ from FP32 models in rare cases. Significantly improves inference performance on CPU/NPU.",
                ApplyOps = [RegOp.SetDword(OnnxKey, "ONNXQuantisationEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(OnnxKey, "ONNXQuantisationEnabled")],
                DetectOps = [RegOp.CheckDword(OnnxKey, "ONNXQuantisationEnabled", 1)],
            },
            new TweakDef
            {
                Id = "mlpol-disable-directml-access",
                Label = "Machine Learning: Disable DirectML GPU Acceleration for Third-Party Apps",
                Category = "Machine Learning Policy",
                Description =
                    "Sets DisableDirectML=1 in MachineLearning policy. Prevents third-party applications from using the DirectML API to run GPU-accelerated machine learning inference on the device's GPU. DirectML-based inference can drive GPU utilisation to 100% in background applications. Disabling DirectML for third-party apps prevents stealth cryptocurrency mining, background data processing, or model training disguised as inference workloads. Microsoft system components continue to use DirectML (Windows AI features).",
                Tags = ["machine-learning", "directml", "gpu", "security", "third-party"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Third-party DirectML GPU acceleration is blocked. Third-party AI apps fall back to CPU inference. Microsoft system components are exempt. GPU remains available for display and rendering tasks.",
                ApplyOps = [RegOp.SetDword(MlKey, "DisableDirectML", 1)],
                RemoveOps = [RegOp.DeleteValue(MlKey, "DisableDirectML")],
                DetectOps = [RegOp.CheckDword(MlKey, "DisableDirectML", 1)],
            },
            new TweakDef
            {
                Id = "mlpol-disable-experimental-models",
                Label = "Machine Learning: Disable Experimental AI Model Features",
                Category = "Machine Learning Policy",
                Description =
                    "Sets DisableExperimentalModels=1 in MachineLearning policy. Prevents Windows from loading and running experimental or preview AI model variants that are enabled via A/B feature flag experiments from Microsoft. Experimental models have not completed safety evaluation and may produce unexpected outputs or exhibit unintentional behaviours. Enterprise environments require predictable AI behaviour; disabling experimental model features ensures only release-grade models are used for all AI inference tasks.",
                Tags = ["machine-learning", "experimental", "models", "enterprise", "stability"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Experimental model variants are blocked. Only release-grade AI models are loaded. AI feature behaviour is predictable and consistent across all managed devices.",
                ApplyOps = [RegOp.SetDword(MlKey, "DisableExperimentalModels", 1)],
                RemoveOps = [RegOp.DeleteValue(MlKey, "DisableExperimentalModels")],
                DetectOps = [RegOp.CheckDword(MlKey, "DisableExperimentalModels", 1)],
            },
            new TweakDef
            {
                Id = "mlpol-set-onnx-cache-size",
                Label = "Machine Learning: Set ONNX Model Build Cache Size to 512 MB",
                Category = "Machine Learning Policy",
                Description =
                    "Sets ONNXCacheSizeMB=512 in ONNX policy. Configures the ONNX runtime model build cache (compiled model storage) to a maximum of 512 MB. The ONNX runtime compiles ML models to hardware-optimised executables on first load and caches the compiled artefact to avoid recompilation on subsequent loads. Without a size limit, the cache grows unboundedly across model versions and updates. 512 MB accommodates ~10–20 compiled model variants while preventing the ONNX cache from consuming multi-GB of disk space on shared systems.",
                Tags = ["machine-learning", "onnx", "cache", "disk", "storage"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "ONNX model cache is limited to 512 MB. Least-recently-used model compilations are evicted when the limit is reached, causing re-compilation latency on first run. Increase if many distinct AI apps are deployed.",
                ApplyOps = [RegOp.SetDword(OnnxKey, "ONNXCacheSizeMB", 512)],
                RemoveOps = [RegOp.DeleteValue(OnnxKey, "ONNXCacheSizeMB")],
                DetectOps = [RegOp.CheckDword(OnnxKey, "ONNXCacheSizeMB", 512)],
            },
            new TweakDef
            {
                Id = "mlpol-enable-winml-app-isolation",
                Label = "Machine Learning: Enable WinML App Isolation for Model Access",
                Category = "Machine Learning Policy",
                Description =
                    "Sets WinMLAppIsolation=1 in MachineLearning policy. Enables process isolation between WinML-enabled applications and the model store. Each application that uses the WinML API receives a virtualised view of the model storage; applications cannot enumerate models installed by other applications. Isolation prevents cross-application AI model theft, model enumeration attacks (discovering what AI capabilities are available), and malicious applications from poisoning models used by trusted applications.",
                Tags = ["machine-learning", "winml", "isolation", "security", "app"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WinML applications are isolated from each other's model stores. Applications can only access models they installed or that are in the system model store. No UX impact for well-designed applications.",
                ApplyOps = [RegOp.SetDword(MlKey, "WinMLAppIsolation", 1)],
                RemoveOps = [RegOp.DeleteValue(MlKey, "WinMLAppIsolation")],
                DetectOps = [RegOp.CheckDword(MlKey, "WinMLAppIsolation", 1)],
            },
            new TweakDef
            {
                Id = "mlpol-disable-model-marketplace-access",
                Label = "Machine Learning: Disable User Access to AI Model Marketplace",
                Category = "Machine Learning Policy",
                Description =
                    "Sets DisableAIModelMarketplace=1 in MachineLearning policy. Blocks user access to the Windows AI model marketplace (where users can download and install community AI models for use with WinML applications). Community models have not been vetted by the organisation's AI safety review process and may contain model backdoors, copyright-infringing training data, or inappropriate content. IT controls model deployment through the enterprise model catalogue.",
                Tags = ["machine-learning", "marketplace", "model", "restriction", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI model marketplace is inaccessible to users. Models can only be deployed by IT through enterprise channels. Users cannot install community AI models.",
                ApplyOps = [RegOp.SetDword(MlKey, "DisableAIModelMarketplace", 1)],
                RemoveOps = [RegOp.DeleteValue(MlKey, "DisableAIModelMarketplace")],
                DetectOps = [RegOp.CheckDword(MlKey, "DisableAIModelMarketplace", 1)],
            },
            new TweakDef
            {
                Id = "mlpol-enable-federated-learning-block",
                Label = "Machine Learning: Block Federated Learning Data Participation",
                Category = "Machine Learning Policy",
                Description =
                    "Sets BlockFederatedLearning=1 in MachineLearning policy. Prevents Windows AI features from participating in federated learning schemes where anonymised gradients derived from user data are sent to Microsoft to improve global AI models. While federated learning is designed to preserve user privacy (only aggregated gradients, not raw data, are shared), the mathematical underpinnings of gradient inversion attacks have demonstrated that gradients can reveal aspects of training data. Blocking participation prevents any local user data derivative from leaving the device.",
                Tags = ["machine-learning", "federated-learning", "privacy", "data-sharing", "gradient"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Federated learning participation is blocked. Local AI models are not used to improve Microsoft's global models. AI features continue to use the latest centrally-trained models deployed through Windows Update.",
                ApplyOps = [RegOp.SetDword(MlKey, "BlockFederatedLearning", 1)],
                RemoveOps = [RegOp.DeleteValue(MlKey, "BlockFederatedLearning")],
                DetectOps = [RegOp.CheckDword(MlKey, "BlockFederatedLearning", 1)],
            },
            new TweakDef
            {
                Id = "mlpol-set-inference-thread-count",
                Label = "Machine Learning: Limit AI Inference CPU Thread Count to 4",
                Category = "Machine Learning Policy",
                Description =
                    "Sets MaxInferenceThreads=4 in MachineLearning policy. Caps the number of CPU threads the WinML inference engine can use for model computation. Without a thread cap, large model inference can consume all available CPU cores, causing CPU contention and degraded performance for foreground work. Limiting inference to 4 threads ensures the inference engine running in background applications shares CPU fairly with interactive work. Appropriate for shared desktops and thin clients where AI background tasks should not compete with the user's primary workload.",
                Tags = ["machine-learning", "inference", "cpu-threads", "performance", "throttle"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI inference is limited to 4 CPU threads. Background AI tasks run slower on high-core-count machines. Increase to 8 for AI workstation class devices.",
                ApplyOps = [RegOp.SetDword(MlKey, "MaxInferenceThreads", 4)],
                RemoveOps = [RegOp.DeleteValue(MlKey, "MaxInferenceThreads")],
                DetectOps = [RegOp.CheckDword(MlKey, "MaxInferenceThreads", 4)],
            },
            new TweakDef
            {
                Id = "mlpol-enable-model-access-audit",
                Label = "Machine Learning: Enable Audit Log for AI Model Access Events",
                Category = "Machine Learning Policy",
                Description =
                    "Sets EnableModelAccessAudit=1 in MachineLearning policy. Writes an Event Log audit entry each time an application loads an AI model from the Windows model store: the model name and version, the requesting application (PID, image path), the access type (load, execute, update), and the timestamp. This provides a complete inventory of AI model usage for security forensics, compliance auditing, and anomaly detection (e.g., unexpected application loading a sensitive AI model, or a model being loaded outside business hours).",
                Tags = ["machine-learning", "model-access", "audit", "security", "logging"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI model access events are logged per-application per-load. Log volume is proportional to AI feature usage frequency. Essential for AI governance and security operations.",
                ApplyOps = [RegOp.SetDword(MlKey, "EnableModelAccessAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(MlKey, "EnableModelAccessAudit")],
                DetectOps = [RegOp.CheckDword(MlKey, "EnableModelAccessAudit", 1)],
            },
        ];
}
