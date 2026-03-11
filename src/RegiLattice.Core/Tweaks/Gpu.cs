namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Gpu
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "gpu-disable-mpo",
            Label = "Disable Multi-Plane Overlay (MPO)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Multi-Plane Overlay which can cause black screens, flickering, or stuttering on some GPU/monitor combinations. Safe to disable if you experience display issues. Default: Enabled. Recommended: Disabled for troubleshooting.",
            Tags = ["gpu", "display", "fix", "mpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5)],
        },
        new TweakDef
        {
            Id = "gpu-tdr-timeout",
            Label = "Increase GPU TDR Timeout (10s)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Increases GPU Timeout Detection and Recovery delay from 2s to 10s. Prevents driver crash/reset during heavy GPU workloads like rendering, ML training, or compute shaders. Options: 2s (default) / 10s / 30s / 60s. Recommended: 10s.",
            Tags = ["gpu", "stability", "tdr", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 10),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDdiDelay", 10),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 2),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDdiDelay", 5),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 10)],
        },
        new TweakDef
        {
            Id = "gpu-disable-nvidia-telemetry",
            Label = "Disable NVIDIA Telemetry",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NVIDIA telemetry and usage data collection. Only applies if NVIDIA drivers are installed. Default: Enabled (opt-in). Recommended: Disabled.",
            Tags = ["gpu", "nvidia", "privacy", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client", @"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client", "OptInOrOutPreference", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID44231", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID64640", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID66610", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client", "OptInOrOutPreference"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID44231"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID64640"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID66610"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client", "OptInOrOutPreference", 0)],
        },
        new TweakDef
        {
            Id = "gpu-prefer-performance",
            Label = "GPU Global High Performance Mode",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets global DirectX GPU preference to high performance with swap effect upgrade enabled. Improves frame pacing. Default: System default. Recommended: High Performance.",
            Tags = ["gpu", "performance", "directx"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings", "SwapEffectUpgradeEnable=1;"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings"),
            ],
        },
        new TweakDef
        {
            Id = "gpu-disable-game-bar-overlay",
            Label = "Disable Game Bar Overlay for GPU",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Xbox Game Bar overlay (UseNexusForGameBarEnabled=0). Reduces GPU overhead and prevents accidental overlay activation. Default: 1 (Enabled). Recommended: 0 (Disabled).",
            Tags = ["gpu", "gaming", "overlay", "game-bar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\GameBar"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "UseNexusForGameBarEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "UseNexusForGameBarEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "UseNexusForGameBarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "gpu-nvidia-tdr-delay",
            Label = "Increase NVIDIA TDR Delay (8s)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the GPU TDR (Timeout Detection and Recovery) delay to 8 seconds. Prevents driver resets during heavy GPU workloads. Removal deletes the value, restoring the Windows default (2s). Default: 2s. Recommended: 8s.",
            Tags = ["gpu", "nvidia", "stability", "tdr"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 8),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 8)],
        },
        new TweakDef
        {
            Id = "gpu-disable-gpu-preemption",
            Label = "Disable GPU Preemption (Low Latency)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables GPU preemption (EnablePreemption=0) for lower render latency. May improve frame times in GPU-bound scenarios but can affect multi-tasking and system responsiveness. Default: Enabled. Removal deletes the value.",
            Tags = ["gpu", "latency", "gaming", "preemption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0)],
        },
        new TweakDef
        {
            Id = "gpu-force-dx12-ultimate",
            Label = "Force DirectX 12 Ultimate",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces DirectX 12 mode for all compatible applications. Enables advanced features like mesh shaders and raytracing. Default: Auto. Recommended: Enabled for DX12-capable GPUs.",
            Tags = ["gpu", "directx", "dx12", "performance", "raytracing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12", 1)],
        },
        new TweakDef
        {
            Id = "gpu-wddm-scheduler",
            Label = "Optimize WDDM GPU Scheduler",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Optimizes WDDM flip queue length to 2 frames for reduced input latency. Trades slight throughput for lower frame queue depth. Default: 3. Recommended: 2 for competitive gaming.",
            Tags = ["gpu", "wddm", "flip-queue", "latency", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueLength", 2),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueEnabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueLength"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueLength", 2)],
        },
        new TweakDef
        {
            Id = "gpu-disable-dwm-animations",
            Label = "Disable DWM Animations",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables DWM Aero Peek animations for reduced GPU overhead. Saves GPU cycles on compositing effects. Default: 1 (enabled). Recommended: Disabled for performance.",
            Tags = ["gpu", "dwm", "animations", "aero-peek", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAeroPeek", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "AnimationsShiftKey", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAeroPeek", 1),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "AnimationsShiftKey"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAeroPeek", 0)],
        },
        new TweakDef
        {
            Id = "gpu-increase-tdr-delay",
            Label = "Increase GPU TDR Timeout",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Increases GPU Timeout Detection and Recovery delay to 10 seconds. Prevents false TDR resets during heavy GPU workloads. Default: 2s. Recommended: 10s for compute/rendering.",
            Tags = ["gpu", "tdr", "timeout", "stability", "compute"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 10),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 10)],
        },
        new TweakDef
        {
            Id = "gpu-max-prerendered-frames",
            Label = "Set Max Pre-Rendered Frames to 1",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets the flip queue size to 1, limiting pre-rendered frames. Reduces input lag at the cost of slightly lower throughput. Default: 3 frames. Recommended: 1 for competitive gaming.",
            Tags = ["gpu", "pre-rendered", "frames", "input-lag", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize", 1)],
        },
        new TweakDef
        {
            Id = "gpu-enable-dx12-async",
            Label = "Enable DirectX 12 Async Compute",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables D3D12 asynchronous command buffer reuse for improved GPU throughput. Most beneficial in DirectX 12 games with async compute shaders. Default: Not set. Recommended: Enabled for gaming.",
            Tags = ["gpu", "directx12", "async", "compute", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE", 1)],
        },
        new TweakDef
        {
            Id = "gpu-disable-shader-cache",
            Label = "Disable DirectX Shader Disk Cache",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the DirectX on-disk shader cache. Reduces disk I/O; useful in scenarios where fresh shader compilation is preferred or disk space is constrained. Default: Enabled.",
            Tags = ["gpu", "shader", "cache", "disk", "directx"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ShaderCachePath", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ShaderCachePath"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ShaderCachePath", 0)],
        },
        new TweakDef
        {
            Id = "gpu-disable-hw-accelerated-scheduling",
            Label = "Disable Hardware-Accelerated GPU Scheduling",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables hardware-accelerated GPU scheduling (HAGS). Can fix stuttering on older GPUs. Default: varies.",
            Tags = ["gpu", "hags", "scheduling", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-delay-10",
            Label = "Increase GPU TDR Timeout to 10 Seconds",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Increases the Timeout Detection Recovery delay from 2s to 10s. Helps with long compute shaders or heavy rendering. Default: 2.",
            Tags = ["gpu", "tdr", "timeout", "crash", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 10)],
        },
        new TweakDef
        {
            Id = "gpu-force-high-performance-power",
            Label = "Force GPU High Performance Power Plan",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces the GPU to maximum performance power mode. Disables GPU power saving. Default: adaptive.",
            Tags = ["gpu", "power", "performance", "high-performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power", "DefaultD3ColdSupported", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power", "DefaultD3ColdSupported")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power", "DefaultD3ColdSupported", 0)],
        },
        new TweakDef
        {
            Id = "gpu-disable-preemption",
            Label = "Disable GPU Preemption",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables GPU preemption scheduling. May improve frame consistency but can cause hangs on some hardware. Default: enabled.",
            Tags = ["gpu", "preemption", "scheduling", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0)],
        },
    ];
}
