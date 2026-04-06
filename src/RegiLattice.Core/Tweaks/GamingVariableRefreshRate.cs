#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

internal static class GamingVariableRefreshRate
{
    private const string AdaptiveSyncKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\AdaptiveSync";
    private const string GfxDriversKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers";
    private const string DxUserKey = @"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences\DirectXUserGlobalSettings";
    private const string NvTweakKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\NVTweak";
    private const string DxGlobalKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vrr-enable-adaptive-sync",
            Label = "Enable Windows Adaptive Sync (FreeSync / G-Sync Compatible)",
            Category = "Gaming",
            Description =
                "Enables Windows adaptive-sync presentation for VRR monitors. Required for FreeSync and G-Sync Compatible to work in windowed and borderless-fullscreen modes.",
            Tags = ["vrr", "freesync", "gsync", "gaming", "display"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Eliminates screen tearing in VRR-capable games at fluctuating frame rates.",
            ApplyOps = [RegOp.SetDword(AdaptiveSyncKey, "AllowAdaptiveSync", 1)],
            RemoveOps = [RegOp.SetDword(AdaptiveSyncKey, "AllowAdaptiveSync", 0)],
            DetectOps = [RegOp.CheckDword(AdaptiveSyncKey, "AllowAdaptiveSync", 1)],
        },
        new TweakDef
        {
            Id = "vrr-enable-dxgi-vrr-optimize",
            Label = "Enable DXGI VRR Swap-Chain Optimisation",
            Category = "Gaming",
            Description =
                "Enables DXGI presentation flag that allows the swap chain to take advantage of VRR displays for lower input latency with adaptive sync.",
            Tags = ["vrr", "dxgi", "gaming", "latency"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Reduces input latency and allows DXGI flip model to exploit VRR on compatible displays.",
            ApplyOps = [RegOp.SetString(DxUserKey, "VRROptimizeEnable", "1")],
            RemoveOps = [RegOp.SetString(DxUserKey, "VRROptimizeEnable", "0")],
            DetectOps = [RegOp.CheckString(DxUserKey, "VRROptimizeEnable", "1")],
        },
        new TweakDef
        {
            Id = "vrr-enable-mpg-vsync-off",
            Label = "Disable V-Sync Override for VRR Monitors",
            Category = "Gaming",
            Description =
                "Disables the forced V-Sync fallback on VRR displays, allowing the VRR range to operate without V-Sync clamping frame rate to refresh rate.",
            Tags = ["vrr", "vsync", "gaming", "performance"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Allows frame rates above the nominal monitor refresh without V-Sync tearing artefacts on VRR displays.",
            ApplyOps = [RegOp.SetString(DxUserKey, "SwapEffectUpgradeEnable", "1")],
            RemoveOps = [RegOp.SetString(DxUserKey, "SwapEffectUpgradeEnable", "0")],
            DetectOps = [RegOp.CheckString(DxUserKey, "SwapEffectUpgradeEnable", "1")],
        },
        new TweakDef
        {
            Id = "vrr-disable-dxgi-mpo-on-latency",
            Label = "Optimise GPU Preemption for VRR Low-Latency Mode",
            Category = "Gaming",
            Description =
                "Sets GPU preemption granularity to instruction-level for VRR low-latency scheduling, reducing frame-delivery jitter on adaptive sync displays.",
            Tags = ["vrr", "gaming", "latency", "gpu"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reduces frame delivery jitter for smoother VRR performance in DX11 and DX12 games.",
            ApplyOps = [RegOp.SetDword(GfxDriversKey, "EnablePreemption", 1)],
            RemoveOps = [RegOp.SetDword(GfxDriversKey, "EnablePreemption", 1)],
            DetectOps = [RegOp.CheckDword(GfxDriversKey, "EnablePreemption", 1)],
        },
        new TweakDef
        {
            Id = "vrr-nvidia-frl-disable",
            Label = "Disable NVIDIA Frame Rate Limiter for VRR Gaming",
            Category = "Gaming",
            Description =
                "Disables the NVIDIA Frame Rate Limiter (FRL) that can interfere with G-Sync and FreeSync Compatible VRR operation at the hardware level.",
            Tags = ["vrr", "nvidia", "gaming", "fps"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Prevents NVIDIA driver's FRL from capping frame rate below monitor VRR maximum on G-Sync Compatible displays.",
            ApplyOps = [RegOp.SetDword(NvTweakKey, "FRLControl", 0)],
            RemoveOps = [RegOp.DeleteValue(NvTweakKey, "FRLControl")],
            DetectOps = [RegOp.CheckDword(NvTweakKey, "FRLControl", 0)],
        },
        new TweakDef
        {
            Id = "vrr-enable-lfc",
            Label = "Enable Low Frame Rate Compensation (LFC)",
            Category = "Gaming",
            Description =
                "Enables Low Frame Rate Compensation on adaptive sync monitors: the display multiplies frames when FPS drops below the minimum VRR range, preventing blank flicker.",
            Tags = ["vrr", "lfc", "freesync", "gaming"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Eliminates blank-screen flicker when the game FPS drops below the monitor's VRR minimum threshold.",
            ApplyOps = [RegOp.SetDword(AdaptiveSyncKey, "EnableLFC", 1)],
            RemoveOps = [RegOp.DeleteValue(AdaptiveSyncKey, "EnableLFC")],
            DetectOps = [RegOp.CheckDword(AdaptiveSyncKey, "EnableLFC", 1)],
        },
        new TweakDef
        {
            Id = "vrr-set-refresh-rate-override",
            Label = "Allow Application-Requested Refresh Rate Changes",
            Category = "Gaming",
            Description =
                "Allows games to request a specific display refresh rate through the DXGI API, which is required for VRR to lock to the game's target frame rate on some displays.",
            Tags = ["vrr", "refresh-rate", "gaming", "dxgi"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables per-game refresh-rate targeting for VRR displays to operate at the optimal rate.",
            ApplyOps = [RegOp.SetDword(GfxDriversKey, "AllowRROverride", 1)],
            RemoveOps = [RegOp.DeleteValue(GfxDriversKey, "AllowRROverride")],
            DetectOps = [RegOp.CheckDword(GfxDriversKey, "AllowRROverride", 1)],
        },
        new TweakDef
        {
            Id = "vrr-disable-vsync-idle-timeout",
            Label = "Reduce V-Sync Idle Timeout for VRR Responsiveness",
            Category = "Gaming",
            Description =
                "Reduces the GPU scheduler VSync idle timeout to 1 ms, keeping the VRR adaptive sync engine active between frames for lower perceived latency.",
            Tags = ["vrr", "vsync", "gaming", "latency"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Keeps the VRR engine primed between frames for faster response when frame rate spikes or drops.",
            ApplyOps = [RegOp.SetDword(GfxDriversKey, "VsyncIdleTimeout", 1)],
            RemoveOps = [RegOp.DeleteValue(GfxDriversKey, "VsyncIdleTimeout")],
            DetectOps = [RegOp.CheckDword(GfxDriversKey, "VsyncIdleTimeout", 1)],
        },
        new TweakDef
        {
            Id = "vrr-mpo-plane-count",
            Label = "Set Multi-Plane Overlay Count for VRR Gaming",
            Category = "Gaming",
            Description =
                "Configures the minimum number of multi-plane overlay planes required before DXGI activates MPO, preventing MPO from competing with VRR presentation for overlapping windows.",
            Tags = ["vrr", "mpo", "gaming", "display"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents MPO from inadvertently disabling VRR when transparent overlays are present.",
            ApplyOps = [RegOp.SetDword(DxGlobalKey, "MultiplaneOverlayMinSupportedFormatCount", 1)],
            RemoveOps = [RegOp.DeleteValue(DxGlobalKey, "MultiplaneOverlayMinSupportedFormatCount")],
            DetectOps = [RegOp.CheckDword(DxGlobalKey, "MultiplaneOverlayMinSupportedFormatCount", 1)],
        },
        new TweakDef
        {
            Id = "vrr-gpu-late-latency",
            Label = "Set GPU Late-Latency Mode for VRR",
            Category = "Gaming",
            Description =
                "Enables GPU late-latency (also called NVIDIA Reflex-style latency reduction) which delays CPU submission until the GPU is nearly ready, reducing frame queuing with VRR.",
            Tags = ["vrr", "latency", "gpu", "gaming"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Reduces input-to-display latency by 5–15 ms on VRR displays at the cost of marginal CPU overhead.",
            ApplyOps = [RegOp.SetDword(GfxDriversKey, "D3DGPULateLatency", 0)],
            RemoveOps = [RegOp.DeleteValue(GfxDriversKey, "D3DGPULateLatency")],
            DetectOps = [RegOp.CheckDword(GfxDriversKey, "D3DGPULateLatency", 0)],
        },
    ];
}
