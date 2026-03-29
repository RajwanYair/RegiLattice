// RegiLattice.Core — Tweaks/GpuComputePolicy.cs
// GPU compute, GPGPU, DirectML, CUDA, and Windows ML policy — Sprint 512.
// Category: "GPU Compute Policy" | Slug: gpucmp
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\GPU

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class GpuComputePolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GPU";
    private const string MlKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinML";
    private const string DmlKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DirectML";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "gpucmp-disable-winml-gpu",
            Label        = "Disable Windows ML GPU Inference",
            Category     = "GPU Compute Policy",
            Description  = "Prevents Windows Machine Learning (WinML) from executing inference operations on the GPU, forcing model evaluation to the CPU, which can reduce power consumption and GPU memory pressure on non-AI workstation deployments.",
            Tags         = ["windows-ml", "gpu-inference", "ai", "compute", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WinML GPU inference disabled; all ML model inference runs on CPU. AI apps may be significantly slower.",
            ApplyOps     = [RegOp.SetDword(MlKey, "DisableGPUInference", 1)],
            RemoveOps    = [RegOp.DeleteValue(MlKey, "DisableGPUInference")],
            DetectOps    = [RegOp.CheckDword(MlKey, "DisableGPUInference", 1)],
        },
        new TweakDef
        {
            Id           = "gpucmp-limit-winml-vram",
            Label        = "Limit Windows ML VRAM Usage to 2 GB",
            Category     = "GPU Compute Policy",
            Description  = "Caps the amount of GPU VRAM that Windows ML inference sessions can allocate to 2048 MB, preventing WinML workloads from consuming all available GPU memory and degrading rendering performance of foreground applications.",
            Tags         = ["windows-ml", "vram", "memory-limit", "gpu", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WinML VRAM capped at 2 GB; large ML models that exceed limit fall back to system RAM or fail.",
            ApplyOps     = [RegOp.SetDword(MlKey, "MaxVRAMMB", 2048)],
            RemoveOps    = [RegOp.DeleteValue(MlKey, "MaxVRAMMB")],
            DetectOps    = [RegOp.CheckDword(MlKey, "MaxVRAMMB", 2048)],
        },
        new TweakDef
        {
            Id           = "gpucmp-disable-directml-third-party",
            Label        = "Block Third-Party DirectML Operator Packages",
            Category     = "GPU Compute Policy",
            Description  = "Prevents third-party applications from loading external DirectML operator packages outside of the Windows SDK, reducing the attack surface from unsigned or malicious ML operator DLLs loaded into application GPU compute contexts.",
            Tags         = ["directml", "operator-packages", "third-party", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Third-party DirectML operator packages blocked; only SDK-bundled operators loaded in GPU compute pipelines.",
            ApplyOps     = [RegOp.SetDword(DmlKey, "BlockThirdPartyOperators", 1)],
            RemoveOps    = [RegOp.DeleteValue(DmlKey, "BlockThirdPartyOperators")],
            DetectOps    = [RegOp.CheckDword(DmlKey, "BlockThirdPartyOperators", 1)],
        },
        new TweakDef
        {
            Id           = "gpucmp-disable-winml-telemetry",
            Label        = "Disable Windows ML Telemetry Reporting to Microsoft",
            Category     = "GPU Compute Policy",
            Description  = "Prevents Windows ML from sending model inference statistics, GPU capability, and API usage telemetry to Microsoft, protecting information about AI workload characteristics from cloud disclosure.",
            Tags         = ["windows-ml", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WinML telemetry to Microsoft disabled; inference stats and GPU model data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(MlKey, "DisableWinMLTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(MlKey, "DisableWinMLTelemetry")],
            DetectOps    = [RegOp.CheckDword(MlKey, "DisableWinMLTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "gpucmp-set-gpu-compute-app-priority",
            Label        = "Set Foreground App GPU Compute Priority to High",
            Category     = "GPU Compute Policy",
            Description  = "Configures the GPU compute scheduler to give foreground applications higher compute queue priority than background GPU processes, ensuring interactive AI and graphics applications are not starved by background ML training jobs.",
            Tags         = ["gpu", "compute-priority", "scheduler", "foreground", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Foreground app GPU compute priority elevated; background GPU jobs deprioritised for better interactivity.",
            ApplyOps     = [RegOp.SetDword(Key, "ForegroundComputePriority", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "ForegroundComputePriority")],
            DetectOps    = [RegOp.CheckDword(Key, "ForegroundComputePriority", 2)],
        },
        new TweakDef
        {
            Id           = "gpucmp-disable-cuda-in-wsl",
            Label        = "Disable CUDA GPU Passthrough into WSL2",
            Category     = "GPU Compute Policy",
            Description  = "Disables CUDA and DirectX GPU passthrough into WSL2 virtual machines, preventing WSL2 Linux processes from accessing GPU compute resources and potential GPU-level privilege escalation from Linux guest to Windows host.",
            Tags         = ["gpu", "wsl2", "cuda", "gpu-passthrough", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "GPU passthrough to WSL2 disabled; CUDA and DirectX not accessible from Linux WSL2 processes.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableWSLGPUPassthrough", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableWSLGPUPassthrough")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableWSLGPUPassthrough", 1)],
        },
        new TweakDef
        {
            Id           = "gpucmp-restrict-gpu-access-untrusted",
            Label        = "Restrict GPU Compute Access for Untrusted Applications",
            Category     = "GPU Compute Policy",
            Description  = "Enables GPU access filtering for untrusted (non-publisher-verified, non-Store) applications, preventing low-reputation software from accessing GPU compute queues that could be used for crypto-mining without user consent.",
            Tags         = ["gpu", "compute-access", "untrusted-apps", "cryptomining", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "GPU compute access restricted for untrusted apps; crypto-mining by unsigned background apps blocked.",
            ApplyOps     = [RegOp.SetDword(Key, "RestrictComputeForUntrustedApps", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RestrictComputeForUntrustedApps")],
            DetectOps    = [RegOp.CheckDword(Key, "RestrictComputeForUntrustedApps", 1)],
        },
        new TweakDef
        {
            Id           = "gpucmp-enable-compute-audit",
            Label        = "Enable GPU Compute Session Audit Logging",
            Category     = "GPU Compute Policy",
            Description  = "Enables audit logging of GPU compute session creation and destruction events, recording which processes open compute contexts on the GPU for security monitoring of GPU resource usage patterns.",
            Tags         = ["gpu", "compute", "audit", "event-log", "session", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "GPU compute session creation logged; process names and context types recorded for security monitoring.",
            ApplyOps     = [RegOp.SetDword(Key, "EnableComputeSessionAudit", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "EnableComputeSessionAudit")],
            DetectOps    = [RegOp.CheckDword(Key, "EnableComputeSessionAudit", 1)],
        },
        new TweakDef
        {
            Id           = "gpucmp-disable-npn-compute",
            Label        = "Disable NPU Compute Offload for IntelAI / Microsoft NPU",
            Category     = "GPU Compute Policy",
            Description  = "Prevents applications from using the NPU (Neural Processing Unit) for compute offload on Copilot+ and Intel AI Boost hardware, ensuring AI workloads run on the GPU or CPU where execution can be monitored and controlled.",
            Tags         = ["npu", "compute-offload", "ai", "copilot-plus", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "NPU compute offload disabled; AI workloads route to GPU/CPU instead of NPU on Copilot+ hardware.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableNPUComputeOffload", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableNPUComputeOffload")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableNPUComputeOffload", 1)],
        },
        new TweakDef
        {
            Id           = "gpucmp-set-vram-reservation",
            Label        = "Reserve GPU VRAM Headroom for System Compositor",
            Category     = "GPU Compute Policy",
            Description  = "Reserves a guaranteed amount of GPU VRAM for the DWM compositor and system UI rendering, preventing GPU compute and ML workloads from exhausting VRAM and causing desktop compositing failures.",
            Tags         = ["gpu", "vram", "reservation", "compositor", "stability", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "VRAM explicitly reserved for system compositor; compute workloads cannot starve DWM of display memory.",
            ApplyOps     = [RegOp.SetDword(Key, "CompositorVRAMReserveMB", 256)],
            RemoveOps    = [RegOp.DeleteValue(Key, "CompositorVRAMReserveMB")],
            DetectOps    = [RegOp.CheckDword(Key, "CompositorVRAMReserveMB", 256)],
        },
    ];
}
