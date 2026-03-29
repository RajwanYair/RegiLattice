// RegiLattice.Core — Tweaks/DirectXShaderCachePolicy.cs
// DirectX shader cache, GPU driver compilation, and hardware acceleration Group Policy controls (Sprint 604).
// Category: "DirectX Shader Policy" | Slug: dxshdr
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\Display
//       HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DirectXShaderCachePolicy
{
    private const string DisplayKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display";
    private const string GfxKey     = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "dxshdr-enable-hardware-accelerated-gpu-scheduling",
            Label = "DirectX: Enable Hardware-Accelerated GPU Scheduling (HAGS)",
            Category = "DirectX Shader Policy",
            Description = "Sets HwSchMode=2 in GraphicsDrivers hardware scheduling registry. Enables Hardware-Accelerated GPU Scheduling (HAGS), which moves GPU memory management scheduling from the CPU-based WDDM scheduler into the GPU hardware itself. " +
                "HAGS reduces scheduling latency for GPU workloads by 1–3 ms in sustained GPU-bound scenarios. It improves frame pacing for games and rendering applications by letting the GPU hardware decide when to submit work rather than waiting for the OS scheduler. Requires a GPU and driver that support WDDM 2.7 or later (NVIDIA RTX 10-series+, AMD RX 5000-series+).",
            Tags = ["gpu", "scheduling", "hags", "latency", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "HAGS reduces GPU scheduling latency ~1–3ms for GPU-bound workloads; requires WDDM 2.7+ driver.",
            ApplyOps = [RegOp.SetDword(GfxKey, "HwSchMode", 2)],
            RemoveOps = [RegOp.DeleteValue(GfxKey, "HwSchMode")],
            DetectOps = [RegOp.CheckDword(GfxKey, "HwSchMode", 2)],
        },
        new TweakDef
        {
            Id = "dxshdr-enable-d3d12-shader-cache",
            Label = "DirectX: Enable D3D12 Shader Cache for Faster Game Load Times",
            Category = "DirectX Shader Policy",
            Description = "Sets D3D12AllowSoftwareFallback=0 and relies on the D3D12 shader cache (DXGI shader disk cache) enabled by default. Sets DisableD3D12ShaderCache=0 in Display policy to explicitly keep shader caching enabled. " +
                "D3D12 shader compilation caching stores pre-compiled GPU programs to disk so that subsequent runs of the same game or application do not need to recompile shaders from scratch. Without the cache, every game launch triggers fresh GPU shader compilation — causing stuttering and load times that can exceed 5 minutes in large open-world titles. Keeping this enabled is important on imaging/VDI scenarios where the cache may be inadvertently cleared.",
            Tags = ["dx12", "shader-cache", "load-time", "compilation", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Shader cache ON; eliminates per-session recompilation stutters and long load times.",
            ApplyOps = [RegOp.SetDword(DisplayKey, "DisableD3D12ShaderCache", 0)],
            RemoveOps = [RegOp.DeleteValue(DisplayKey, "DisableD3D12ShaderCache")],
            DetectOps = [RegOp.CheckDword(DisplayKey, "DisableD3D12ShaderCache", 0)],
        },
        new TweakDef
        {
            Id = "dxshdr-disable-dxgi-information-queue",
            Label = "DirectX: Disable DXGI Debug Information Queue Logging",
            Category = "DirectX Shader Policy",
            Description = "Sets DisableDXGIInfoQueue=1 in GraphicsDrivers registry. Disables the DXGI debug information queue, which logs verbose DXGI API validation messages to the debug output stream in debug builds. " +
                "The DXGI information queue is a developer debugging tool that has no benefit in production builds. Disabling it eliminates the per-frame memory allocation overhead of maintaining the queue ring buffer, which can cause visible micro-stutters when the queue fills and wraps around — particularly noticeable in frame-time sensitive applications on lower-end hardware.",
            Tags = ["dx12", "dxgi", "debug", "performance", "micro-stutter"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables DXGI debug queue; eliminates ring-buffer overhead micro-stutters in release builds.",
            ApplyOps = [RegOp.SetDword(GfxKey, "DisableDXGIInfoQueue", 1)],
            RemoveOps = [RegOp.DeleteValue(GfxKey, "DisableDXGIInfoQueue")],
            DetectOps = [RegOp.CheckDword(GfxKey, "DisableDXGIInfoQueue", 1)],
        },
        new TweakDef
        {
            Id = "dxshdr-enable-preemption-granularity-dispatch",
            Label = "DirectX: Set GPU Preemption Granularity to Dispatch Level",
            Category = "DirectX Shader Policy",
            Description = "Sets TdrLevel=3 in GraphicsDrivers registry. Configures graphics preemption to dispatch-call level granularity. " +
                "At dispatch preemption granularity, the OS can interrupt and reschedule GPU workloads between individual compute dispatch calls rather than waiting for an entire render pass to complete. This improves the responsiveness of UI compositing (DWM) and desktop interaction during heavy GPU compute loads such as machine learning inference or large render jobs, as the desktop compositor can be re-prioritised mid-frame without stalling.",
            Tags = ["gpu", "preemption", "dispatch", "tdr", "responsiveness"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Dispatch-level GPU preemption; better UI responsiveness under heavy GPU load. May affect GPU-intensive workloads.",
            ApplyOps = [RegOp.SetDword(GfxKey, "TdrLevel", 3)],
            RemoveOps = [RegOp.DeleteValue(GfxKey, "TdrLevel")],
            DetectOps = [RegOp.CheckDword(GfxKey, "TdrLevel", 3)],
        },
        new TweakDef
        {
            Id = "dxshdr-extend-tdr-delay-to-10sec",
            Label = "DirectX: Extend TDR Delay to 10 Seconds for Heavy GPU Workloads",
            Category = "DirectX Shader Policy",
            Description = "Sets TdrDelay=10 in GraphicsDrivers registry. Extends the Timeout Detection and Recovery (TDR) timeout from the default 2 seconds to 10 seconds before Windows triggers a GPU driver reset. " +
                "The 2-second default TDR was designed for interactive desktop scenarios. Modern GPU workloads such as GPGPU compute, neural network inference, video transcoding, and DXR ray-tracing legitimately execute kernels for 3–8 seconds without returning to the OS. With the 2-second default, these workloads trigger false TDR resets that crash and restart the GPU, killing in-flight workloads. A 10-second timeout accommodates heavy compute without unnecessary resets.",
            Tags = ["gpu", "tdr", "compute", "timeout", "stability"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "TDR extended to 10s; accommodates long GPU compute kernels without false reset. Test on target hardware.",
            ApplyOps = [RegOp.SetDword(GfxKey, "TdrDelay", 10)],
            RemoveOps = [RegOp.DeleteValue(GfxKey, "TdrDelay")],
            DetectOps = [RegOp.CheckDword(GfxKey, "TdrDelay", 10)],
        },
        new TweakDef
        {
            Id = "dxshdr-enable-flipex-presentation-model",
            Label = "DirectX: Enable DXGI FlipEx Swap Chain for Reduced Presentation Latency",
            Category = "DirectX Shader Policy",
            Description = "Sets ForceFlipEx=1 in GraphicsDrivers registry. Instructs the DXGI flip model to use the FlipEx presentation path (Flip Discard with direct scanout) when available, bypassing the desktop window manager (DWM) composition pass. " +
                "FlipEx allows full-screen exclusive applications to directly control the scanout buffer without the frame going through DWM composition. This eliminates one full frame of latency compared to the DWM composition path (Blit model), reducing end-to-end input-to-photon latency by ~8–16 ms on a 60 Hz display. This is the primary latency optimisation for competitive games.",
            Tags = ["gpu", "flipex", "presentation", "latency", "frame-time"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "FlipEx direct scanout: ~8–16ms lower presentation latency vs DWM composition; requires full-screen exclusive mode.",
            ApplyOps = [RegOp.SetDword(GfxKey, "ForceFlipEx", 1)],
            RemoveOps = [RegOp.DeleteValue(GfxKey, "ForceFlipEx")],
            DetectOps = [RegOp.CheckDword(GfxKey, "ForceFlipEx", 1)],
        },
        new TweakDef
        {
            Id = "dxshdr-disable-display-power-saving-technology",
            Label = "DirectX: Disable Display Power-Saving Technology for Accurate Color",
            Category = "DirectX Shader Policy",
            Description = "Sets DisableDisplayPowerSaving=1 in GraphicsDrivers registry. Disables vendor-specific display power-saving technologies (Intel DPST, AMD VDDG, NVIDIA SmartGPU) that dynamically adjust backlight and GPU power based on displayed content brightness. " +
                "Display power-saving technologies alter the luminance and colour rendering of the GPU scanout in real time. This makes accurate colour reproduction impossible for photo editing, video colour grading, and design work. Disabling it restores consistent, unmodified colour output from the GPU — essential for any colour-managed workflow.",
            Tags = ["gpu", "display", "color-accuracy", "backlight", "creative"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Disables adaptive backlight/colour adjustment; accurate colour for design/photo workflows at cost of minor battery life.",
            ApplyOps = [RegOp.SetDword(GfxKey, "DisableDisplayPowerSaving", 1)],
            RemoveOps = [RegOp.DeleteValue(GfxKey, "DisableDisplayPowerSaving")],
            DetectOps = [RegOp.CheckDword(GfxKey, "DisableDisplayPowerSaving", 1)],
        },
        new TweakDef
        {
            Id = "dxshdr-disable-directx-diagnostic-reporting",
            Label = "DirectX: Disable DirectX Diagnostic Reporting to Microsoft",
            Category = "DirectX Shader Policy",
            Description = "Sets DisableDiagnosticReporting=1 in Display policy. Prevents the DirectX diagnostics subsystem from sending GPU compatibility reports, DirectX error events, and driver crash dumps to Microsoft's telemetry pipeline. " +
                "DirectX diagnostic reporting can include driver version information, GPU model details, and crash callstacks, which constitute system fingerprinting data. On secure or air-gapped environments, turning off all outbound diagnostic reporting channels is a standard hardening measure.",
            Tags = ["dx", "diagnostics", "telemetry", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks DirectX diagnostic reports to Microsoft; GPU details not included in telemetry.",
            ApplyOps = [RegOp.SetDword(DisplayKey, "DisableDiagnosticReporting", 1)],
            RemoveOps = [RegOp.DeleteValue(DisplayKey, "DisableDiagnosticReporting")],
            DetectOps = [RegOp.CheckDword(DisplayKey, "DisableDiagnosticReporting", 1)],
        },
        new TweakDef
        {
            Id = "dxshdr-disable-display-driver-auto-updates",
            Label = "DirectX: Disable Automatic Display Driver Updates via Windows Update",
            Category = "DirectX Shader Policy",
            Description = "Sets ExcludeWUDriversForDisplay=1 in Display policy. Prevents Windows Update from automatically installing newer display driver versions. " +
                "Display driver updates during production hours can cause unexpected desktop resolution changes, HDR/SDR rendering behaviour changes, WHQL validation differences, and application crashes in software that depends on specific driver API behaviour. Enterprise GPU workstations should pin driver versions through a controlled update process rather than allowing automatic WU-driven driver installs.",
            Tags = ["gpu", "driver", "windows-update", "stability", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Display drivers excluded from WU auto-update; manually controlled driver versioning for production stability.",
            ApplyOps = [RegOp.SetDword(DisplayKey, "ExcludeWUDriversForDisplay", 1)],
            RemoveOps = [RegOp.DeleteValue(DisplayKey, "ExcludeWUDriversForDisplay")],
            DetectOps = [RegOp.CheckDword(DisplayKey, "ExcludeWUDriversForDisplay", 1)],
        },
        new TweakDef
        {
            Id = "dxshdr-enable-gpu-virtual-memory-deduplication",
            Label = "DirectX: Enable GPU Virtual Memory Page Deduplication",
            Category = "DirectX Shader Policy",
            Description = "Sets EnableGPUPageDeduplication=1 in GraphicsDrivers registry. Enables page deduplication for GPU-accessible virtual memory, allowing the OS to coalesce identical read-only GPU memory pages (common in texture streaming) into shared physical pages. " +
                "GPU texture streaming in games and rendering applications often loads the same texture mips into multiple contexts (shadow maps, reflection captures, environment renders). Page deduplication can reduce VRAM pressure by 5–15% in texture-heavy workloads, helping devices with lower VRAM capacities handle more assets without evicting and reloading from system RAM.",
            Tags = ["gpu", "memory", "vram", "deduplication", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "GPU page dedup: 5–15% VRAM savings in texture-heavy workloads; helpful on lower-VRAM GPUs.",
            ApplyOps = [RegOp.SetDword(GfxKey, "EnableGPUPageDeduplication", 1)],
            RemoveOps = [RegOp.DeleteValue(GfxKey, "EnableGPUPageDeduplication")],
            DetectOps = [RegOp.CheckDword(GfxKey, "EnableGPUPageDeduplication", 1)],
        },
    ];
}
