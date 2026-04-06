#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

internal static class GamingGPUPower
{
    private const string GfxPowerKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power";
    private const string NvKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\nvlddmkm\Global\NVTweak";
    private const string GfxKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers";
    private const string PwrSettingsKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00";
    private const string NvGlobalKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\nvlddmkm\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "gpupwr-prefer-high-perf",
            Label = "Set GPU Power Mode: Prefer Maximum Performance",
            Category = "GPU / Graphics",
            Description =
                "Sets the Windows GPU power preference to maximum performance, preventing the driver from downclocking the GPU during gaming workloads on power-limited systems.",
            Tags = ["gpu", "gaming", "power", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Keeps GPU clocks high during gaming; increases power draw on laptops.",
            ApplyOps = [RegOp.SetDword(GfxPowerKey, "PreferDiscreteGPU", 1)],
            RemoveOps = [RegOp.SetDword(GfxPowerKey, "PreferDiscreteGPU", 0)],
            DetectOps = [RegOp.CheckDword(GfxPowerKey, "PreferDiscreteGPU", 1)],
        },
        new TweakDef
        {
            Id = "gpupwr-enable-nvidia-persistence",
            Label = "Enable NVIDIA GPU Clock Persistence Mode",
            Category = "GPU / Graphics",
            Description =
                "Enables NVIDIA persistence mode so the GPU does not enter deep clock-down states between frames, eliminating re-ramp latency spikes at the start of each frame.",
            Tags = ["gpu", "nvidia", "gaming", "latency"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Eliminates GPU clock-ramp latency at frame boundaries; slightly increases idle power.",
            ApplyOps = [RegOp.SetDword(NvKey, "PerfLevelSrc", 0x2222)],
            RemoveOps = [RegOp.DeleteValue(NvKey, "PerfLevelSrc")],
            DetectOps = [RegOp.CheckDword(NvKey, "PerfLevelSrc", 0x2222)],
        },
        new TweakDef
        {
            Id = "gpupwr-tdr-level-recover",
            Label = "Set GPU TDR Recovery Level for Gaming",
            Category = "GPU / Graphics",
            Description =
                "Sets TdrLevel to 3 (recover from GPU timeout without reboot), reducing false driver resets in games that momentarily exceed the default 2-second GPU timeout.",
            Tags = ["gpu", "gaming", "tdr", "stability"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Reduces driver-reset events in GPU-intensive games at the cost of slightly delayed crash detection.",
            ApplyOps = [RegOp.SetDword(GfxKey, "TdrLevel", 3)],
            RemoveOps = [RegOp.SetDword(GfxKey, "TdrLevel", 3)],
            DetectOps = [RegOp.CheckDword(GfxKey, "TdrLevel", 3)],
        },
        new TweakDef
        {
            Id = "gpupwr-tdr-delay-extend",
            Label = "Extend GPU TDR Timeout to 8 Seconds",
            Category = "GPU / Graphics",
            Description =
                "Extends the GPU TDR (Timeout Detection and Recovery) timeout from 2 s to 8 s, preventing driver resets in games with long GPU compute stalls or shader compilation spikes.",
            Tags = ["gpu", "gaming", "tdr", "stability"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Prevents game-crashes from shader-compile stalls; set TdrLevel=3 first so recovery works correctly.",
            ApplyOps = [RegOp.SetDword(GfxKey, "TdrDelay", 8)],
            RemoveOps = [RegOp.SetDword(GfxKey, "TdrDelay", 2)],
            DetectOps = [RegOp.CheckDword(GfxKey, "TdrDelay", 8)],
        },
        new TweakDef
        {
            Id = "gpupwr-disable-hw-flip-queue-depth",
            Label = "Reduce Hardware Flip Queue Depth for Lower Latency",
            Category = "GPU / Graphics",
            Description =
                "Reduces the WDDM hardware flip queue depth from 3 to 1, preventing the GPU flip queue from pre-queuing frames and reducing input latency by 1–2 frames.",
            Tags = ["gpu", "gaming", "latency", "flip"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reduces input latency by reducing queued frames; may cause micro-stuttering on slower systems.",
            ApplyOps = [RegOp.SetDword(GfxKey, "HwFlipQueueMaxQueuedFlips", 1)],
            RemoveOps = [RegOp.SetDword(GfxKey, "HwFlipQueueMaxQueuedFlips", 3)],
            DetectOps = [RegOp.CheckDword(GfxKey, "HwFlipQueueMaxQueuedFlips", 1)],
        },
        new TweakDef
        {
            Id = "gpupwr-nvidia-no-power-limit",
            Label = "Disable NVIDIA Driver Power Thermal Limit Boost Block",
            Category = "GPU / Graphics",
            Description =
                "Disables the NVIDIA driver-enforced power throttle that can reduce GPU clock speeds when the card approaches its thermal or power limit during sustained gaming sessions.",
            Tags = ["gpu", "nvidia", "gaming", "power"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 2,
            ImpactNote = "May cause higher GPU temperatures; ensure adequate cooling before applying.",
            ApplyOps = [RegOp.SetDword(NvGlobalKey, "RmEnableGpuFirmware", 0)],
            RemoveOps = [RegOp.DeleteValue(NvGlobalKey, "RmEnableGpuFirmware")],
            DetectOps = [RegOp.CheckDword(NvGlobalKey, "RmEnableGpuFirmware", 0)],
        },
        new TweakDef
        {
            Id = "gpupwr-mhz-stabilise",
            Label = "Enable GPU DXGI Flip Frequency Stabilisation",
            Category = "GPU / Graphics",
            Description =
                "Enables DXGI flip frequency stabilisation in the graphics driver, smoothing out GPU clock frequency variation that causes micro-stuttering in frame-time graphs.",
            Tags = ["gpu", "gaming", "dxgi", "stuttering"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Reduces micro-stutter visible in frame-time analysis tools.",
            ApplyOps = [RegOp.SetDword(GfxKey, "DxgFlipStabilisationEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(GfxKey, "DxgFlipStabilisationEnabled")],
            DetectOps = [RegOp.CheckDword(GfxKey, "DxgFlipStabilisationEnabled", 1)],
        },
        new TweakDef
        {
            Id = "gpupwr-disable-compute-preemption-timeout",
            Label = "Extend GPU Compute Pre-emption Timeout for Games",
            Category = "GPU / Graphics",
            Description =
                "Extends the TDR pre-emption timeout for compute workloads, preventing false GPU resets when a game uses GPU compute (shader compilation, ray-tracing BVH rebuild) for longer than 2 s.",
            Tags = ["gpu", "gaming", "compute", "tdr"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            ImpactNote = "Prevents spurious TDR resets during GPU compute; set alongside TdrDelay for full coverage.",
            ApplyOps = [RegOp.SetDword(GfxKey, "TdrDdiDelay", 8)],
            RemoveOps = [RegOp.SetDword(GfxKey, "TdrDdiDelay", 2)],
            DetectOps = [RegOp.CheckDword(GfxKey, "TdrDdiDelay", 8)],
        },
        new TweakDef
        {
            Id = "gpupwr-enable-wddm-scheduler",
            Label = "Enable WDDM 3.0 Hardware-Accelerated GPU Scheduling",
            Category = "GPU / Graphics",
            Description =
                "Enables Hardware-Accelerated GPU Scheduling (HAGS) in WDDM 3.0, allowing the GPU to manage its own VRAM and scheduling to reduce CPU-GPU synchronisation overhead.",
            Tags = ["gpu", "gaming", "wddm", "scheduling"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Reduces CPU overhead and can lower GPU latency by 1–3 ms on WDDM 3.0 supported GPUs.",
            ApplyOps = [RegOp.SetDword(GfxKey, "HwSchMode", 2)],
            RemoveOps = [RegOp.SetDword(GfxKey, "HwSchMode", 1)],
            DetectOps = [RegOp.CheckDword(GfxKey, "HwSchMode", 2)],
        },
        new TweakDef
        {
            Id = "gpupwr-disable-dxgi-mpo-gaming",
            Label = "Disable Multi-Plane Overly on Single-Monitor Gaming Systems",
            Category = "GPU / Graphics",
            Description =
                "Disables DXGI Multi-Plane Overlay on single-display gaming systems where it can cause micro-stutter or incompatibility with certain anti-cheat engines.",
            Tags = ["gpu", "gaming", "mpo", "stutter"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Eliminates MPO-related micro-stutter on some GPU/driver combinations; re-enable if HDR output breaks.",
            ApplyOps = [RegOp.SetDword(GfxKey, "DisableOverlays", 1)],
            RemoveOps = [RegOp.DeleteValue(GfxKey, "DisableOverlays")],
            DetectOps = [RegOp.CheckDword(GfxKey, "DisableOverlays", 1)],
        },
    ];
}
