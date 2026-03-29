// RegiLattice.Core — Tweaks/DirectXRenderingPolicy.cs
// Direct3D, DirectX feature levels, DXGI, and GPU resource management policy — Sprint 511.
// Category: "DirectX Rendering Policy" | Slug: d3dpol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Direct3D

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DirectXRenderingPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Direct3D";
    private const string DxKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DirectX";
    private const string DgiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DXGI";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "d3dpol-disable-d3d-debug-layer",
            Label        = "Disable Direct3D Debug Layer in Production",
            Category     = "DirectX Rendering Policy",
            Description  = "Disables the Direct3D Debug Layer (D3D11_CREATE_DEVICE_DEBUG / DX12 debug flag) in production environments, preventing verbose GPU validation from activating when debug runtimes are installed on production machines.",
            Tags         = ["direct3d", "debug-layer", "gpu", "production", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "D3D debug layer disabled; no GPU validation overhead even if debug SDK is installed on the machine.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableDebugLayer", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableDebugLayer")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableDebugLayer", 1)],
        },
        new TweakDef
        {
            Id           = "d3dpol-disable-d3d-warp-fallback",
            Label        = "Disable Direct3D WARP Software Renderer Fallback",
            Category     = "DirectX Rendering Policy",
            Description  = "Prevents applications from using the WARP (Windows Advanced Rasterisation Platform) CPU-based software renderer as a fallback when hardware Direct3D is unavailable, ensuring all D3D rendering uses physical GPU hardware.",
            Tags         = ["direct3d", "warp", "software-renderer", "gpu", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "WARP software renderer blocked; D3D apps fail without GPU rather than running at 1/100 speed on CPU.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableWARPFallback", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableWARPFallback")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableWARPFallback", 1)],
        },
        new TweakDef
        {
            Id           = "d3dpol-set-feature-level-minimum-11",
            Label        = "Require Minimum Direct3D Feature Level 11.0",
            Category     = "DirectX Rendering Policy",
            Description  = "Sets a minimum required Direct3D feature level of 11.0, preventing applications from requesting feature levels below D3D11 and ensuring all GPU workloads use modern shader models and resource bindings.",
            Tags         = ["direct3d", "feature-level", "d3d11", "gpu", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "D3D minimum feature level set to 11.0; apps cannot fall back to D3D9/D3D10 mode.",
            ApplyOps     = [RegOp.SetDword(Key, "MinimumFeatureLevel", 0xB000)],
            RemoveOps    = [RegOp.DeleteValue(Key, "MinimumFeatureLevel")],
            DetectOps    = [RegOp.CheckDword(Key, "MinimumFeatureLevel", 0xB000)],
        },
        new TweakDef
        {
            Id           = "d3dpol-disable-dxgi-fullscreen-opt",
            Label        = "Disable DXGI Fullscreen Optimisations App-Wide",
            Category     = "DirectX Rendering Policy",
            Description  = "Disables DXGI Fullscreen Optimisations at system policy level, preventing Windows from overriding fullscreen exclusive mode with a windowed swap chain, which can cause frame timing inconsistencies in precision rendering.",
            Tags         = ["dxgi", "fullscreen-optimisations", "exclusive-mode", "direct3d", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "DXGI fullscreen optimisations disabled; true exclusive fullscreen used for all DXGI apps system-wide.",
            ApplyOps     = [RegOp.SetDword(DgiKey, "DisableFullscreenOptimizations", 1)],
            RemoveOps    = [RegOp.DeleteValue(DgiKey, "DisableFullscreenOptimizations")],
            DetectOps    = [RegOp.CheckDword(DgiKey, "DisableFullscreenOptimizations", 1)],
        },
        new TweakDef
        {
            Id           = "d3dpol-enable-auto-hdr",
            Label        = "Enable Direct3D Auto HDR for SDR Application Upscaling",
            Category     = "DirectX Rendering Policy",
            Description  = "Enables Windows Auto HDR which algorithmically expands the luminance range of SDR Direct3D 11 and 12 applications for HDR display output, improving visual quality of DX applications without source code changes.",
            Tags         = ["direct3d", "auto-hdr", "hdr", "display", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Auto HDR enabled; D3D SDR apps upscaled to HDR range on compatible HDR monitors.",
            ApplyOps     = [RegOp.SetDword(Key, "AutoHDREnabled", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "AutoHDREnabled")],
            DetectOps    = [RegOp.CheckDword(Key, "AutoHDREnabled", 1)],
        },
        new TweakDef
        {
            Id           = "d3dpol-disable-d3d-telemetry",
            Label        = "Disable Direct3D Telemetry Reporting to Microsoft",
            Category     = "DirectX Rendering Policy",
            Description  = "Prevents the Direct3D runtime from sending application GPU usage, feature level, and performance telemetry to Microsoft, protecting information about GPU workload characteristics from cloud disclosure.",
            Tags         = ["direct3d", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "D3D telemetry to Microsoft disabled; GPU app usage and feature level stats not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableD3DTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableD3DTelemetry")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableD3DTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "d3dpol-enable-dx12-ultimate",
            Label        = "Enable DirectX 12 Ultimate Feature Set Policy",
            Category     = "DirectX Rendering Policy",
            Description  = "Configures the system to prefer the DirectX 12 Ultimate feature set (Shader Model 6.6, Mesh Shaders, Sampler Feedback, DirectX Raytracing 1.1) when available, enabling applications to use the highest GPU capability tier.",
            Tags         = ["direct3d", "dx12-ultimate", "raytracing", "mesh-shader", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "DX12 Ultimate feature set preferred; apps can advertise SM6.6 / RT 1.1 / Mesh Shaders on compatible GPUs.",
            ApplyOps     = [RegOp.SetDword(Key, "PreferDX12Ultimate", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "PreferDX12Ultimate")],
            DetectOps    = [RegOp.CheckDword(Key, "PreferDX12Ultimate", 1)],
        },
        new TweakDef
        {
            Id           = "d3dpol-restrict-gpu-access-sandboxed",
            Label        = "Restrict Direct3D GPU Access in Sandboxed AppContainer Processes",
            Category     = "DirectX Rendering Policy",
            Description  = "Configures reduced-privilege Direct3D access for AppContainer (UWP sandbox) processes, preventing sandboxed applications from accessing full GPU command queue capabilities that could be used for side-channel attacks.",
            Tags         = ["direct3d", "appcontainer", "sandbox", "gpu-access", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Sandboxed GPU access restricted; AppContainer apps have limited GPU command queue capabilities.",
            ApplyOps     = [RegOp.SetDword(Key, "RestrictGPUAccessInSandbox", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RestrictGPUAccessInSandbox")],
            DetectOps    = [RegOp.CheckDword(Key, "RestrictGPUAccessInSandbox", 1)],
        },
        new TweakDef
        {
            Id           = "d3dpol-log-d3d-device-removed",
            Label        = "Log Direct3D Device Removed Events for Diagnostics",
            Category     = "DirectX Rendering Policy",
            Description  = "Enables Application event log entries for DXGI_ERROR_DEVICE_REMOVED and DXGI_ERROR_DEVICE_HUNG events generated by Direct3D, providing diagnostic information about GPU hardware failures, driver crashes, and TDR events.",
            Tags         = ["direct3d", "device-removed", "event-log", "diagnostics", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "D3D device removed events logged; GPU failure reasons visible in Application event log for diagnostics.",
            ApplyOps     = [RegOp.SetDword(Key, "LogDeviceRemovedEvents", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LogDeviceRemovedEvents")],
            DetectOps    = [RegOp.CheckDword(Key, "LogDeviceRemovedEvents", 1)],
        },
        new TweakDef
        {
            Id           = "d3dpol-disable-overlay-planes",
            Label        = "Disable DirectX Hardware Overlay Planes",
            Category     = "DirectX Rendering Policy",
            Description  = "Disables DXGI hardware overlay planes that allow applications to render directly into GPU overlay surfaces, preventing overlay plane usage that bypasses DWM compositing and can lead to display corruption on multi-monitor setups.",
            Tags         = ["direct3d", "dxgi", "overlay-planes", "display", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Hardware overlay planes disabled; all rendering goes through DWM compositor. Reduces display corruption risk.",
            ApplyOps     = [RegOp.SetDword(DgiKey, "DisableHWOverlayPlanes", 1)],
            RemoveOps    = [RegOp.DeleteValue(DgiKey, "DisableHWOverlayPlanes")],
            DetectOps    = [RegOp.CheckDword(DgiKey, "DisableHWOverlayPlanes", 1)],
        },
    ];
}
