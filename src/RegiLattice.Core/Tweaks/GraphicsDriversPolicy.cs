// RegiLattice.Core — Tweaks/GraphicsDriversPolicy.cs
// Sprint 283: Graphics Drivers Group Policy (10 tweaks)
// Category: "Graphics Drivers Policy" | Slug: gfxdrv
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GraphicsDrivers

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class GraphicsDriversPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GraphicsDrivers";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "gfxdrv-disable-dxgi-flip-model",
            Label = "Disable DXGI Flip Model Override",
            Category = "Graphics Drivers Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets DisableFlipModel=1 in the GraphicsDrivers policy key. Reverts DXGI "
                + "presentation from the optimised Flip Model (DXGI_SWAP_EFFECT_FLIP_*) "
                + "back to the legacy Blt Model for applications that do not explicitly "
                + "request a swap-effect. Flip Model reduces present latency and input  "
                + "lag for games; disabling it is useful only as a workaround for "
                + "display corruption bugs in specific driver versions. Default: 0.",
            Tags = ["graphics", "dxgi", "flip-model", "display", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFlipModel", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFlipModel")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFlipModel", 1)],
        },
        new TweakDef
        {
            Id = "gfxdrv-disable-mpo",
            Label = "Disable Multi-Plane Overlay",
            Category = "Graphics Drivers Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets DisableMultiplaneOverlay=1 in the GraphicsDrivers policy key. "
                + "Prevents the display engine from compositing independent window "
                + "planes (MPO) in the GPU hardware overlay rather than in software. "
                + "MPO is supposed to reduce GPU load and power; however, several AMD "
                + "and NVIDIA driver families produce screen flickering, black-flash, "
                + "or multi-monitor artefacts when MPO is enabled. Default: 0.",
            Tags = ["graphics", "mpo", "overlay", "display", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMultiplaneOverlay", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMultiplaneOverlay")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMultiplaneOverlay", 1)],
        },
        new TweakDef
        {
            Id = "gfxdrv-disable-variable-refresh",
            Label = "Disable Variable Refresh Rate (VRR/FreeSync/G-Sync) Policy",
            Category = "Graphics Drivers Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableVariableRefreshRate=1 in the GraphicsDrivers policy key. "
                + "Prevents VRR (FreeSync / G-Sync / Adaptive Sync) from being enabled "
                + "system-wide via the policy layer. VRR is beneficial for gaming but "
                + "can cause flicker artefacts on some panel firmwares during desktop "
                + "use and transitions between windowed and full-screen modes. "
                + "Default: 0. Recommended: 1 only on systems with affected displays.",
            Tags = ["graphics", "vrr", "freesync", "gsync", "display", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableVariableRefreshRate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableVariableRefreshRate")],
            DetectOps = [RegOp.CheckDword(Key, "DisableVariableRefreshRate", 1)],
        },
        new TweakDef
        {
            Id = "gfxdrv-disable-gpu-scheduler",
            Label = "Disable Hardware GPU Scheduler",
            Category = "Graphics Drivers Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 4,
            Description =
                "Sets HwSchMode=1 in the GraphicsDrivers policy key (1=disabled, 2=enabled). "
                + "Reverts GPU command scheduling from the Windows Hardware GPU Scheduler "
                + "(WDDM 2.7+) back to the legacy software scheduler. The hardware "
                + "scheduler reduces latency for games on modern GPUs; however, some "
                + "enterprise GPU drivers expose bugs in hardware scheduling that cause "
                + "intermittent TDR (Timeout Detection and Recovery) events. Default: 2.",
            Tags = ["graphics", "gpu-scheduler", "wddm", "tdr", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HwSchMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "HwSchMode")],
            DetectOps = [RegOp.CheckDword(Key, "HwSchMode", 1)],
        },
        new TweakDef
        {
            Id = "gfxdrv-disable-auto-hdr",
            Label = "Disable Auto HDR Policy Enforcement",
            Category = "Graphics Drivers Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableAutoHdr=1 in the GraphicsDrivers policy key. Prevents "
                + "Auto HDR from being applied to SDR games and applications at the "
                + "system-policy level even if a user enables it in Display Settings. "
                + "Auto HDR can degrade visual quality in games with hand-authored "
                + "per-material palette choices not designed for HDR range expansion. "
                + "Default: 0. Recommended: 1 for colour-critical design workstations.",
            Tags = ["graphics", "hdr", "auto-hdr", "display", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoHdr", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoHdr")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoHdr", 1)],
        },
        new TweakDef
        {
            Id = "gfxdrv-disable-dx12-resource-binding",
            Label = "Disable Experimental DX12 Resource Binding",
            Category = "Graphics Drivers Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 4,
            Description =
                "Sets DisableExperimentalResourceBinding=1 in the GraphicsDrivers policy "
                + "key. Opts out of experimental DX12 resource binding tier extensions "
                + "that the D3D runtime may advertise to applications on newer driver "
                + "versions before official specification alignment. Experimental "
                + "binding tiers can trigger spurious validation errors in debug layers "
                + "and unexpected behaviour in strictly conforming applications. Default: 0.",
            Tags = ["graphics", "dx12", "directx", "resource-binding", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableExperimentalResourceBinding", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableExperimentalResourceBinding")],
            DetectOps = [RegOp.CheckDword(Key, "DisableExperimentalResourceBinding", 1)],
        },
        new TweakDef
        {
            Id = "gfxdrv-disable-telemetry",
            Label = "Disable Graphics Driver Telemetry",
            Category = "Graphics Drivers Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableTelemetry=1 in the GraphicsDrivers policy key. Prevents "
                + "the WDDM kernel-mode driver framework from emitting graphics "
                + "performance and crash telemetry events to Microsoft's Watson and "
                + "CEIP collection pipelines. GPU model, driver version, render API "
                + "usage, and per-application frame-rate data are among the metrics "
                + "that these events capture. Default: 0. Recommended: 1.",
            Tags = ["graphics", "telemetry", "privacy", "wddm", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "gfxdrv-disable-preemption",
            Label = "Disable Fine-Grained GPU Preemption",
            Category = "Graphics Drivers Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets DisableFineGrainedPreemption=1 in the GraphicsDrivers policy key. "
                + "Reverts GPU command-list preemption from the fine-grained "
                + "(per-triangle / per-dispatch) level to the coarser DMA-packet "
                + "level supported by all WDDM 2.x GPU hardware. Fine-grained "
                + "preemption reduces OS responsiveness latency during GPU-intensive "
                + "workloads but can introduce micro-stutter on certain game workloads "
                + "with high preemption rates. Default: 0.",
            Tags = ["graphics", "preemption", "wddm", "gpu", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFineGrainedPreemption", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFineGrainedPreemption")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFineGrainedPreemption", 1)],
        },
        new TweakDef
        {
            Id = "gfxdrv-disable-d3d12-warp-updates",
            Label = "Disable D3D12 WARP Software Renderer Updates",
            Category = "Graphics Drivers Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableWarpUpdates=1 in the GraphicsDrivers policy key. Blocks "
                + "background delivery of updated WARP (Windows Advanced Rasterization "
                + "Platform) software-renderer binaries via Windows Update. WARP "
                + "updates are small and generally safe but represent an unplanned "
                + "background write that can interfere with lock-down software "
                + "inventory compliance checks. Default: 0.",
            Tags = ["graphics", "warp", "d3d12", "updates", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWarpUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWarpUpdates")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWarpUpdates", 1)],
        },
        new TweakDef
        {
            Id = "gfxdrv-disable-display-required",
            Label = "Disable Display Required Power Request Override",
            Category = "Graphics Drivers Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableDisplayRequired=1 in the GraphicsDrivers policy key. "
                + "Prevents the graphics subsystem from issuing a SYSTEM_REQUIRED or "
                + "DISPLAY_REQUIRED power request that keeps the display on even when "
                + "the power policy would otherwise dim or blank it. Some full-screen "
                + "applications and kiosk shells issue spurious power requests; this "
                + "policy ensures the system power governor retains control. Default: 0.",
            Tags = ["graphics", "power", "display", "sleep", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDisplayRequired", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDisplayRequired")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDisplayRequired", 1)],
        },
    ];
}
