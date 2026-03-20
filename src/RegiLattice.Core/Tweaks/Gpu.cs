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
            Description =
                "Disables Multi-Plane Overlay which can cause black screens, flickering, or stuttering on some GPU/monitor combinations. Safe to disable if you experience display issues. Default: Enabled. Recommended: Disabled for troubleshooting.",
            Tags = ["gpu", "display", "fix", "mpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5)],
        },
        new TweakDef
        {
            Id = "gpu-tdr-timeout",
            Label = "Increase GPU TDR Timeout (10s)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Increases GPU Timeout Detection and Recovery delay from 2s to 10s. Prevents driver crash/reset during heavy GPU workloads like rendering, ML training, or compute shaders. Options: 2s (default) / 10s / 30s / 60s. Recommended: 10s.",
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
            Description =
                "Disables NVIDIA telemetry and usage data collection. Only applies if NVIDIA drivers are installed. Default: Enabled (opt-in). Recommended: Disabled.",
            Tags = ["gpu", "nvidia", "privacy", "telemetry"],
            IsApplicable = HardwareInfo.HasNvidiaGpu,
            ApplicabilityNote = "Requires NVIDIA GPU",
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS",
            ],
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
            Description =
                "Sets global DirectX GPU preference to high performance with swap effect upgrade enabled. Improves frame pacing. Default: System default. Recommended: High Performance.",
            Tags = ["gpu", "performance", "directx"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences",
                    "DirectXUserGlobalSettings",
                    "SwapEffectUpgradeEnable=1;"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences",
                    "DirectXUserGlobalSettings",
                    "SwapEffectUpgradeEnable=1;"
                ),
            ],
        },
        new TweakDef
        {
            Id = "gpu-disable-game-bar-overlay",
            Label = "Disable Game Bar Overlay for GPU",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Xbox Game Bar overlay (UseNexusForGameBarEnabled=0). Reduces GPU overhead and prevents accidental overlay activation. Default: 1 (Enabled). Recommended: 0 (Disabled).",
            Tags = ["gpu", "gaming", "overlay", "game-bar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\GameBar"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "UseNexusForGameBarEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "UseNexusForGameBarEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "UseNexusForGameBarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "gpu-nvidia-tdr-delay",
            Label = "Increase NVIDIA TDR Delay (8s)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the GPU TDR (Timeout Detection and Recovery) delay to 8 seconds. Prevents driver resets during heavy GPU workloads. Removal deletes the value, restoring the Windows default (2s). Default: 2s. Recommended: 8s.",
            Tags = ["gpu", "nvidia", "stability", "tdr"],
            IsApplicable = HardwareInfo.HasNvidiaGpu,
            ApplicabilityNote = "Requires NVIDIA GPU",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 8)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 8)],
        },
        new TweakDef
        {
            Id = "gpu-disable-gpu-preemption",
            Label = "Disable GPU Preemption (Low Latency)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables GPU preemption (EnablePreemption=0) for lower render latency. May improve frame times in GPU-bound scenarios but can affect multi-tasking and system responsiveness. Default: Enabled. Removal deletes the value.",
            Tags = ["gpu", "latency", "gaming", "preemption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0)],
        },
        new TweakDef
        {
            Id = "gpu-force-dx12-ultimate",
            Label = "Force DirectX 12 Ultimate",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces DirectX 12 mode for all compatible applications. Enables advanced features like mesh shaders and raytracing. Default: Auto. Recommended: Enabled for DX12-capable GPUs.",
            Tags = ["gpu", "directx", "dx12", "performance", "raytracing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12", 1)],
        },
        new TweakDef
        {
            Id = "gpu-wddm-scheduler",
            Label = "Optimize WDDM GPU Scheduler",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Optimizes WDDM flip queue length to 2 frames for reduced input latency. Trades slight throughput for lower frame queue depth. Default: 3. Recommended: 2 for competitive gaming.",
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
            Description =
                "Disables DWM Aero Peek animations for reduced GPU overhead. Saves GPU cycles on compositing effects. Default: 1 (enabled). Recommended: Disabled for performance.",
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
            Description =
                "Increases GPU Timeout Detection and Recovery delay to 10 seconds. Prevents false TDR resets during heavy GPU workloads. Default: 2s. Recommended: 10s for compute/rendering.",
            Tags = ["gpu", "tdr", "timeout", "stability", "compute"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 10)],
        },
        new TweakDef
        {
            Id = "gpu-max-prerendered-frames",
            Label = "Set Max Pre-Rendered Frames to 1",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the flip queue size to 1, limiting pre-rendered frames. Reduces input lag at the cost of slightly lower throughput. Default: 3 frames. Recommended: 1 for competitive gaming.",
            Tags = ["gpu", "pre-rendered", "frames", "input-lag", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize", 1)],
        },
        new TweakDef
        {
            Id = "gpu-enable-dx12-async",
            Label = "Enable DirectX 12 Async Compute",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables D3D12 asynchronous command buffer reuse for improved GPU throughput. Most beneficial in DirectX 12 games with async compute shaders. Default: Not set. Recommended: Enabled for gaming.",
            Tags = ["gpu", "directx12", "async", "compute", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE", 1)],
        },
        new TweakDef
        {
            Id = "gpu-disable-shader-cache",
            Label = "Disable DirectX Shader Disk Cache",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the DirectX on-disk shader cache. Reduces disk I/O; useful in scenarios where fresh shader compilation is preferred or disk space is constrained. Default: Enabled.",
            Tags = ["gpu", "shader", "cache", "disk", "directx"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ShaderCachePath", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ShaderCachePath")],
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
            Description =
                "Increases the Timeout Detection Recovery delay from 2s to 10s. Helps with long compute shaders or heavy rendering. Default: 2.",
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
        new TweakDef
        {
            Id = "gpu-disable-fullscreen-optimizations-global",
            Label = "Disable Fullscreen Optimisations (Global)",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables fullscreen optimisations globally for all applications. Forces true exclusive fullscreen mode. Default: enabled.",
            Tags = ["gpu", "fullscreen", "optimizations", "global"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode", 2)],
        },
        new TweakDef
        {
            Id = "gpu-disable-igpu-powersave",
            Label = "Disable iGPU Power Saving",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Intel integrated GPU power-saving features. Forces maximum iGPU performance. Default: power saving enabled.",
            Tags = ["gpu", "igpu", "intel", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "PlatformSupportMiracast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "PlatformSupportMiracast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "PlatformSupportMiracast", 0)],
        },
        new TweakDef
        {
            Id = "gpu-disable-power-throttle",
            Label = "Disable GPU Power Throttling",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables GPU power throttling. Forces the GPU to operate at full power. May increase power consumption. Default: enabled.",
            Tags = ["gpu", "throttle", "power", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1)],
        },
        new TweakDef
        {
            Id = "gpu-force-software-cursor",
            Label = "Force Software Cursor",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces software cursor rendering instead of hardware cursor. May fix cursor corruption issues on some GPUs. Default: hardware cursor.",
            Tags = ["gpu", "cursor", "software", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "EnableSWCursor", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "EnableSWCursor")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "EnableSWCursor", 1)],
        },
        new TweakDef
        {
            Id = "gpu-hw-scheduling",
            Label = "Enable Hardware-Accelerated GPU Scheduling",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            Description = "Enables Hardware-Accelerated GPU Scheduling (WDDM 2.7+). Reduces GPU scheduling latency and CPU overhead. Default: off.",
            Tags = ["gpu", "hw-scheduling", "wddm", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2)],
        },
        new TweakDef
        {
            Id = "gpu-increase-priority",
            Label = "Increase GPU Thread Priority",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the GPU thread priority to 8 in the MMCSS Games profile. Ensures GPU workloads get higher scheduling priority. Default: not set.",
            Tags = ["gpu", "priority", "mmcss", "games"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GPU Priority",
                    8
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GPU Priority"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GPU Priority",
                    8
                ),
            ],
        },
        new TweakDef
        {
            Id = "gpu-multiplane-overlay-disable",
            Label = "Disable Multiplane Overlay (MPO)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Multiplane Overlay. Can fix stuttering, flickering, and black screens on some GPU/driver combinations. Default: enabled.",
            Tags = ["gpu", "mpo", "overlay", "fix"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5)],
        },
        new TweakDef
        {
            Id = "gpu-preemption-disable",
            Label = "Disable GPU Compute Preemption",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables GPU compute preemption at the DWM level. May improve GPGPU compute performance but can cause display hangs. Default: enabled.",
            Tags = ["gpu", "compute", "preemption", "dwm"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "CompositionPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "CompositionPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "CompositionPolicy", 0)],
        },
        new TweakDef
        {
            Id = "gpu-wddm3-miracast",
            Label = "Disable Miracast (WDDM)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Miracast wireless display support at the driver level. Frees GPU resources used for Miracast. Default: enabled.",
            Tags = ["gpu", "miracast", "wddm", "wireless-display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect", "AllowProjectionToPC", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect", "AllowProjectionToPC")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect", "AllowProjectionToPC", 0)],
        },
        new TweakDef
        {
            Id = "gpu-tasks-gpu-priority",
            Label = "Boost GPU Priority for Games Profile",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets GpuPriority=8 in the Multimedia SystemProfile Tasks\\Games key. Requests highest GPU scheduling priority for game processes via MMCSS. Default: 1 or 2.",
            Tags = ["gpu", "priority", "mmcss", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GpuPriority",
                    8
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GpuPriority"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GpuPriority",
                    8
                ),
            ],
        },
        new TweakDef
        {
            Id = "gpu-hw-sched-policy",
            Label = "Set WDDM Scheduler Policy to Batch",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SchedulerPolicy=2 (batch mode) in GraphicsDrivers. Batches GPU work submissions to reduce context-switch overhead. Default: preemptive.",
            Tags = ["gpu", "wddm", "scheduler", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "SchedulerPolicy", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "SchedulerPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "SchedulerPolicy", 2)],
        },
        new TweakDef
        {
            Id = "gpu-threading-optimization",
            Label = "Enable GPU Driver Threading Optimization",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ThreadedOptimizationFlags=1 in GraphicsDrivers to enable driver threading optimizations. Allows the GPU driver to use multiple CPU threads for command processing. Default: off.",
            Tags = ["gpu", "threading", "driver", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "ThreadedOptimizationFlags", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "ThreadedOptimizationFlags")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "ThreadedOptimizationFlags", 1)],
        },
        new TweakDef
        {
            Id = "gpu-hw-accel-enable",
            Label = "Enable WPF Hardware Acceleration",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableHWAcceleration=0 in Avalon.Graphics to ensure WPF/XAML apps use GPU hardware acceleration. Default: 0 (enabled).",
            Tags = ["gpu", "hwaccel", "wpf", "avalon"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Avalon.Graphics"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Avalon.Graphics", "DisableHWAcceleration", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Avalon.Graphics", "DisableHWAcceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Avalon.Graphics", "DisableHWAcceleration", 0)],
        },
        new TweakDef
        {
            Id = "gpu-display-preemption-off",
            Label = "Disable GPU Display Preemption",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnablePreemption=0 in GraphicsDrivers Scheduler to disable GPU task preemption. Can reduce frame-time variance in latency-sensitive workloads. Default: enabled.",
            Tags = ["gpu", "preemption", "scheduler", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0)],
        },
        new TweakDef
        {
            Id = "gpu-idle-power-engine-timeout",
            Label = "Set GPU Engine Timeout for Idle Power",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrEngineTimeout=13 seconds in GraphicsDrivers. Controls how long the GPU engine can be unresponsive before reset recovery. Default: 2 seconds.",
            Tags = ["gpu", "tdr", "timeout", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrEngineTimeout", 13)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrEngineTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrEngineTimeout", 13)],
        },
        new TweakDef
        {
            Id = "gpu-hdr-auto-color",
            Label = "Enable DWM Auto Color Management",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets AutoColorManagement=1 in DWM registry key to enable automatic HDR/color management for connected displays that support it. Default: 0.",
            Tags = ["gpu", "hdr", "color", "dwm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "AutoColorManagement", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "AutoColorManagement")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "AutoColorManagement", 1)],
        },
        new TweakDef
        {
            Id = "gpu-tdr-limit-extend",
            Label = "Extend GPU TDR Limit Count to 10",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLimitCount=10 in GraphicsDrivers. Allows up to 10 TDR recoveries in 60 seconds before crashing. Useful for overclocked or compute workloads. Default: 5.",
            Tags = ["gpu", "tdr", "stability", "overclock"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitCount", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitCount")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitCount", 10)],
        },
        new TweakDef
        {
            Id = "gpu-multi-adapter-alt",
            Label = "Set Multi-GPU Alternate Frame Rendering",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UseNoPlatformUpdateMode=1 in GraphicsDrivers to hint drivers to avoid platform-specific update mode that may conflict with multi-GPU setups. Default: 0.",
            Tags = ["gpu", "multi-gpu", "adapter", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "UseNoPlatformUpdateMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "UseNoPlatformUpdateMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "UseNoPlatformUpdateMode", 1)],
        },
        new TweakDef
        {
            Id = "gpu-opengl-flip-interval",
            Label = "Set DirectDraw Flip Interval to 0",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets FlipInterval=0 in DirectDraw settings to allow immediate buffer flips without vertical sync wait. Reduces display-pipeline latency for OpenGL/DDraw apps. Default: 1.",
            Tags = ["gpu", "opengl", "directdraw", "vsync"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw", "FlipInterval", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw", "FlipInterval")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw", "FlipInterval", 0)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-level-recover",
            Label = "Set GPU TDR Level to Recover (No Bugcheck)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLevel=3 so Windows recovers the GPU engine after a Timeout Detection & Recovery (TDR) event without triggering a bugcheck. Improves stability for overclocked or demanding GPU workloads. Default: 3 on most systems.",
            Tags = ["gpu", "tdr", "stability", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLevel", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLevel", 3)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-debugging-off",
            Label = "Disable GPU TDR Crash Dump Generation",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables TDR debug crash dump generation by setting TdrDebugging=0. Prevents large crash dumps when the GPU recovers from a timeout, reducing disk I/O overhead during recovery. Default: 0.",
            Tags = ["gpu", "tdr", "dump", "debugging"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDebugging", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDebugging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDebugging", 0)],
        },
        new TweakDef
        {
            Id = "gpu-enable-vrr-optimize",
            Label = "Enable Windows 11 VRR Optimisation",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22000,
            Description =
                "Enables the Variable Refresh Rate (VRR) optimisation in DWM on Windows 11. Allows the desktop compositor to leverage VRR/FreeSync/G-Sync for smoother UI rendering. Default: off (requires supported display).",
            Tags = ["gpu", "vrr", "freesync", "gsync", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "VrrOptimizeEnable", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "VrrOptimizeEnable", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "VrrOptimizeEnable", 1)],
        },
        new TweakDef
        {
            Id = "gpu-disable-aero-peek",
            Label = "Disable Aero Peek (Taskbar Transparency Effect)",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Aero Peek, which shows a live transparent preview of windows when hovering over taskbar thumbnails. Reduces DWM compositing load. Default: enabled.",
            Tags = ["gpu", "aero", "peek", "dwm", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "EnableAeroPeek", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "EnableAeroPeek", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "EnableAeroPeek", 0)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-ddi-delay",
            Label = "Set GPU TDR DDI Delay to 0 (Immediate)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrDdiDelay=0 so the DDI watchdog immediately detects when a DDI call exceeds the allowed time. Allows faster GPU error detection with less latency on recovery. Default: not set (uses kernel default).",
            Tags = ["gpu", "tdr", "ddi", "watchdog"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDdiDelay", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDdiDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDdiDelay", 0)],
        },
        new TweakDef
        {
            Id = "gpu-enable-hw-flip-queue",
            Label = "Enable GPU Hardware Flip Queue",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            Description =
                "Enables the DirectX Graphics Kernel hardware flip queue via DxgkrnlEnableHwFlipQueue=1. Moves present queue management to hardware, reducing CPU involvement and frame delivery latency. Default: system-managed.",
            Tags = ["gpu", "flip-queue", "latency", "dx", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "DxgkrnlEnableHwFlipQueue", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "DxgkrnlEnableHwFlipQueue")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "DxgkrnlEnableHwFlipQueue", 1)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-limit-value",
            Label = "Increase GPU TDR Limit Count (Stability)",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLimitValue=60 to allow up to 60 GPU timeouts within the TdrLimitTime window before triggering a full system bugcheck. More tolerant under heavy GPU load or overclocking. Default: 5.",
            Tags = ["gpu", "tdr", "limit", "stability", "overclocking"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitValue", 60)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitValue")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitValue", 60)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-limit-time",
            Label = "Extend GPU TDR Limit Time Window",
            Category = "GPU / Graphics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLimitTime=60 to extend the TDR limit counting window to 60 seconds. Combined with a higher TdrLimitValue this prevents bugchecks on systems that have occasional GPU hangs under load. Default: 60 (may vary).",
            Tags = ["gpu", "tdr", "limit", "time", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitTime", 60)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitTime", 60)],
        },
        new TweakDef
        {
            Id = "gpu-disable-smooth-fonts",
            Label = "Disable Font Smoothing (GPU Rendering)",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows font smoothing (anti-aliasing). Reduces GPU compositing work for text rendering. Useful on high-DPI displays where sub-pixel rendering is less needed. Default: enabled.",
            Tags = ["gpu", "font", "smoothing", "rendering", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0")],
        },
        new TweakDef
        {
            Id = "gpu-disable-cleartype",
            Label = "Switch Font Smoothing from ClearType to Standard",
            Category = "GPU / Graphics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Switches font smoothing from ClearType (sub-pixel, type 2) to standard anti-aliasing (grayscale, type 1). Reduces LCD sub-pixel rendering overhead in DWM. Default: ClearType (type 2) on Windows.",
            Tags = ["gpu", "cleartype", "font", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", 1)],
        },
    ];
}
