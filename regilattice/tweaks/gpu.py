"""GPU / Graphics registry tweaks — hardware scheduling, power, caching.

Covers: GPU hardware scheduling, shader cache, NVIDIA/AMD power management,
DirectX optimisations, and GPU-aware memory management.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Shared key paths ────────────────────────────────────────────────────────────────────────

_GPU_SCHED = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"
_DX_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"
_GRAPHICS_PERF = r"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences"
_DWMKEY = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"
_NV_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000"
_AMD_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000"
_GAME_CONFIG = r"HKEY_CURRENT_USER\System\GameConfigStore"
_GAME_BAR = r"HKEY_CURRENT_USER\Software\Microsoft\GameBar"
_DWM_LM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"


# ── Hardware-Accelerated GPU Scheduling (HwSchMode) ──────────────────────────────────────────


def _apply_hw_gpu_scheduling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: enable Hardware-Accelerated GPU Scheduling")
    SESSION.backup([_GPU_SCHED], "HwGpuScheduling")
    SESSION.set_dword(_GPU_SCHED, "HwSchMode", 2)  # 1=off, 2=on


def _remove_hw_gpu_scheduling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_GPU_SCHED], "HwGpuScheduling_Remove")
    SESSION.set_dword(_GPU_SCHED, "HwSchMode", 1)


def _detect_hw_gpu_scheduling() -> bool:
    return SESSION.read_dword(_GPU_SCHED, "HwSchMode") == 2


# ── Disable Variable Refresh Rate (VRR / MPO) ───────────────────────────────────────────────

_MPO_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"


def _apply_disable_mpo(*, require_admin: bool = True) -> None:
    """Disable Multi-Plane Overlay (MPO) — fixes black screen / flickering on some GPUs."""
    assert_admin(require_admin)
    SESSION.log("GPU: disable Multi-Plane Overlay (MPO)")
    SESSION.backup([_MPO_KEY], "DisableMPO")
    SESSION.set_dword(_MPO_KEY, "OverlayTestMode", 5)


def _remove_disable_mpo(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_MPO_KEY], "DisableMPO_Remove")
    SESSION.delete_value(_MPO_KEY, "OverlayTestMode")


def _detect_disable_mpo() -> bool:
    return SESSION.read_dword(_MPO_KEY, "OverlayTestMode") == 5


# ── Large GPU TDR Timeout ───────────────────────────────────────────────────────────────────


def _apply_gpu_tdr_timeout(*, require_admin: bool = True) -> None:
    """Increase GPU TDR (Timeout Detection and Recovery) delay from 2s to 10s.
    Prevents driver resets during heavy GPU loads (rendering, ML training)."""
    assert_admin(require_admin)
    SESSION.log("GPU: set TDR delay to 10 seconds")
    SESSION.backup([_GPU_SCHED], "GpuTdr")
    SESSION.set_dword(_GPU_SCHED, "TdrDelay", 10)
    SESSION.set_dword(_GPU_SCHED, "TdrDdiDelay", 10)


def _remove_gpu_tdr_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_GPU_SCHED], "GpuTdr_Remove")
    SESSION.set_dword(_GPU_SCHED, "TdrDelay", 2)
    SESSION.set_dword(_GPU_SCHED, "TdrDdiDelay", 5)


def _detect_gpu_tdr_timeout() -> bool:
    return SESSION.read_dword(_GPU_SCHED, "TdrDelay") == 10


# ── Disable NVIDIA Telemetry ────────────────────────────────────────────────────────────────

_NV_TELEM = r"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client"
_NV_TELEM2 = r"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS"


def _apply_disable_nvidia_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: disable NVIDIA telemetry")
    SESSION.backup([_NV_TELEM, _NV_TELEM2], "NvidiaTelemetry")
    SESSION.set_dword(_NV_TELEM, "OptInOrOutPreference", 0)
    SESSION.set_dword(_NV_TELEM2, "EnableRID44231", 0)
    SESSION.set_dword(_NV_TELEM2, "EnableRID64640", 0)
    SESSION.set_dword(_NV_TELEM2, "EnableRID66610", 0)


def _remove_disable_nvidia_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NV_TELEM, "OptInOrOutPreference")
    SESSION.delete_value(_NV_TELEM2, "EnableRID44231")
    SESSION.delete_value(_NV_TELEM2, "EnableRID64640")
    SESSION.delete_value(_NV_TELEM2, "EnableRID66610")


def _detect_disable_nvidia_telemetry() -> bool:
    return SESSION.read_dword(_NV_TELEM, "OptInOrOutPreference") == 0


# ── GPU Prefer Max Performance (Windows Graphics Settings) ───────────────────────────────────

_GPU_PREF = r"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences"


def _apply_gpu_prefer_performance(*, require_admin: bool = False) -> None:
    SESSION.log("GPU: set global GPU preference to high performance")
    SESSION.backup([_GPU_PREF], "GpuPerformance")
    SESSION.set_string(_GPU_PREF, "DirectXUserGlobalSettings", "SwapEffectUpgradeEnable=1;")


def _remove_gpu_prefer_performance(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_GPU_PREF, "DirectXUserGlobalSettings")


def _detect_gpu_prefer_performance() -> bool:
    val = SESSION.read_string(_GPU_PREF, "DirectXUserGlobalSettings")
    return val is not None and "SwapEffectUpgradeEnable=1" in val


# ── Disable Desktop Window Manager (DWM) Animations ─────────────────────────────────────────


def _apply_disable_dwm_animations(*, require_admin: bool = False) -> None:
    SESSION.log("GPU: disable DWM animations for snappier desktop")
    SESSION.backup([_DWMKEY], "DwmAnimations")
    SESSION.set_dword(_DWMKEY, "EnableAeroPeek", 0)
    SESSION.set_dword(_DWMKEY, "AnimationsShiftKey", 1)


def _remove_disable_dwm_animations(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_DWMKEY, "EnableAeroPeek", 1)
    SESSION.delete_value(_DWMKEY, "AnimationsShiftKey")


def _detect_disable_dwm_animations() -> bool:
    return SESSION.read_dword(_DWMKEY, "EnableAeroPeek") == 0


# ── Increase GPU VRAM Pre-Emption Granularity ───────────────────────────────────────────────

_GPU_PREEMPT = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler"


def _apply_gpu_preemption(*, require_admin: bool = True) -> None:
    """Set GPU preemption granularity to DMA buffer (reduces latency)."""
    assert_admin(require_admin)
    SESSION.log("GPU: set GPU preemption to DMA buffer level")
    SESSION.backup([_GPU_PREEMPT], "GpuPreemption")
    SESSION.set_dword(_GPU_PREEMPT, "EnablePreemption", 0)


def _remove_gpu_preemption(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_GPU_PREEMPT], "GpuPreemption_Remove")
    SESSION.delete_value(_GPU_PREEMPT, "EnablePreemption")


def _detect_gpu_preemption() -> bool:
    return SESSION.read_dword(_GPU_PREEMPT, "EnablePreemption") == 0


# ── Disable Fullscreen Optimizations Globally ───────────────────────────────────────────────


def _apply_disable_fse_global(*, require_admin: bool = False) -> None:
    SESSION.log("GPU: disable fullscreen optimizations globally")
    SESSION.backup([_GAME_CONFIG], "DisableFSEGlobal")
    SESSION.set_dword(_GAME_CONFIG, "GameDVR_FSEBehaviorMode", 2)


def _remove_disable_fse_global(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_GAME_CONFIG, "GameDVR_FSEBehaviorMode", 0)


def _detect_disable_fse_global() -> bool:
    return SESSION.read_dword(_GAME_CONFIG, "GameDVR_FSEBehaviorMode") == 2


# ── Disable Game Bar Overlay for GPU ─────────────────────────────────────────────────────────


def _apply_disable_game_bar_overlay(*, require_admin: bool = False) -> None:
    SESSION.log("GPU: disable Game Bar overlay")
    SESSION.backup([_GAME_BAR], "DisableGameBarOverlay")
    SESSION.set_dword(_GAME_BAR, "UseNexusForGameBarEnabled", 0)


def _remove_disable_game_bar_overlay(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_GAME_BAR, "UseNexusForGameBarEnabled", 1)


def _detect_disable_game_bar_overlay() -> bool:
    return SESSION.read_dword(_GAME_BAR, "UseNexusForGameBarEnabled") == 0


# ── Increase NVIDIA TDR Delay (8s) ──────────────────────────────────────────────────────────


def _apply_nvidia_tdr_delay(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: set NVIDIA TDR delay to 8 seconds")
    SESSION.backup([_GPU_SCHED], "NvidiaTdrDelay")
    SESSION.set_dword(_GPU_SCHED, "TdrDelay", 8)


def _remove_nvidia_tdr_delay(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GPU_SCHED, "TdrDelay")


def _detect_nvidia_tdr_delay() -> bool:
    return SESSION.read_dword(_GPU_SCHED, "TdrDelay") == 8


# ── Disable GPU Preemption (Low Latency) ─────────────────────────────────────────────────────


def _apply_disable_gpu_preemption(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: disable GPU preemption for low latency")
    SESSION.backup([_GPU_PREEMPT], "DisableGpuPreemption")
    SESSION.set_dword(_GPU_PREEMPT, "EnablePreemption", 0)


def _remove_disable_gpu_preemption(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GPU_PREEMPT, "EnablePreemption")


def _detect_disable_gpu_preemption() -> bool:
    return SESSION.read_dword(_GPU_PREEMPT, "EnablePreemption") == 0


# ── Disable Multi-Plane Overlay (Anti-Stutter) ──────────────────────────────────────────────


def _apply_mpo_disable(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: disable Multi-Plane Overlay (anti-stutter)")
    SESSION.backup([_DWM_LM], "MpoDisable")
    SESSION.set_dword(_DWM_LM, "OverlayTestMode", 5)


def _remove_mpo_disable(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DWM_LM, "OverlayTestMode")


def _detect_mpo_disable() -> bool:
    return SESSION.read_dword(_DWM_LM, "OverlayTestMode") == 5


# ── Force DirectX 12 Ultimate ────────────────────────────────────────────────────────────────


def _apply_force_dx12_ultimate(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: force DirectX 12 Ultimate mode")
    SESSION.backup([_DX_KEY], "ForceDx12Ultimate")
    SESSION.set_dword(_DX_KEY, "ForceD3D12", 1)


def _remove_force_dx12_ultimate(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_DX_KEY], "ForceDx12Ultimate_Remove")
    SESSION.delete_value(_DX_KEY, "ForceD3D12")


def _detect_force_dx12_ultimate() -> bool:
    return SESSION.read_dword(_DX_KEY, "ForceD3D12") == 1


# ── Disable Integrated GPU Power Saving (Platform Clock Constant TSC) ────────────────────────


def _apply_igpu_clock_tsc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: enable platform clock constant TSC")
    SESSION.backup([_GPU_SCHED], "IgpuClockTsc")
    SESSION.set_dword(_GPU_SCHED, "PlatformClockConstantTSC", 1)


def _remove_igpu_clock_tsc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_GPU_SCHED], "IgpuClockTsc_Remove")
    SESSION.delete_value(_GPU_SCHED, "PlatformClockConstantTSC")


def _detect_igpu_clock_tsc() -> bool:
    return SESSION.read_dword(_GPU_SCHED, "PlatformClockConstantTSC") == 1


# ── Optimize WDDM GPU Scheduler (Flip Queue) ────────────────────────────────────────────────


def _apply_wddm_scheduler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: optimise WDDM flip queue length to 2")
    SESSION.backup([_GPU_SCHED], "WddmScheduler")
    SESSION.set_dword(_GPU_SCHED, "HwFlipQueueLength", 2)
    SESSION.set_dword(_GPU_SCHED, "HwFlipQueueEnabled", 1)


def _remove_wddm_scheduler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_GPU_SCHED], "WddmScheduler_Remove")
    SESSION.delete_value(_GPU_SCHED, "HwFlipQueueLength")
    SESSION.delete_value(_GPU_SCHED, "HwFlipQueueEnabled")


def _detect_wddm_scheduler() -> bool:
    return SESSION.read_dword(_GPU_SCHED, "HwFlipQueueLength") == 2


# ── Plugin registration ─────────────────────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="gpu-hw-scheduling",
        label="Hardware-Accelerated GPU Scheduling",
        category="GPU / Graphics",
        apply_fn=_apply_hw_gpu_scheduling,
        remove_fn=_remove_hw_gpu_scheduling,
        detect_fn=_detect_hw_gpu_scheduling,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GPU_SCHED],
        description=(
            "Enables Hardware-Accelerated GPU Scheduling (HwSchMode=2). "
            "Reduces latency by letting the GPU manage its own memory scheduling. "
            "Requires Windows 10 2004+ and a supported GPU driver. "
            "Options: 1=Off, 2=On. Default: 1 (Off). Recommended: 2 (On)."
        ),
        tags=["gpu", "performance", "scheduling", "latency"],
    ),
    TweakDef(
        id="disable-mpo",
        label="Disable Multi-Plane Overlay (MPO)",
        category="GPU / Graphics",
        apply_fn=_apply_disable_mpo,
        remove_fn=_remove_disable_mpo,
        detect_fn=_detect_disable_mpo,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_MPO_KEY],
        description=(
            "Disables Multi-Plane Overlay which can cause black screens, "
            "flickering, or stuttering on some GPU/monitor combinations. "
            "Safe to disable if you experience display issues. "
            "Default: Enabled. Recommended: Disabled for troubleshooting."
        ),
        tags=["gpu", "display", "fix", "mpo"],
    ),
    TweakDef(
        id="gpu-tdr-timeout",
        label="Increase GPU TDR Timeout (10s)",
        category="GPU / Graphics",
        apply_fn=_apply_gpu_tdr_timeout,
        remove_fn=_remove_gpu_tdr_timeout,
        detect_fn=_detect_gpu_tdr_timeout,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GPU_SCHED],
        description=(
            "Increases GPU Timeout Detection and Recovery delay from 2s to 10s. "
            "Prevents driver crash/reset during heavy GPU workloads like rendering, "
            "ML training, or compute shaders. "
            "Options: 2s (default) / 10s / 30s / 60s. Recommended: 10s."
        ),
        tags=["gpu", "stability", "tdr", "rendering"],
    ),
    TweakDef(
        id="disable-nvidia-telemetry",
        label="Disable NVIDIA Telemetry",
        category="GPU / Graphics",
        apply_fn=_apply_disable_nvidia_telemetry,
        remove_fn=_remove_disable_nvidia_telemetry,
        detect_fn=_detect_disable_nvidia_telemetry,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NV_TELEM, _NV_TELEM2],
        description=(
            "Disables NVIDIA telemetry and usage data collection. "
            "Only applies if NVIDIA drivers are installed. "
            "Default: Enabled (opt-in). Recommended: Disabled."
        ),
        tags=["gpu", "nvidia", "privacy", "telemetry"],
    ),
    TweakDef(
        id="gpu-prefer-performance",
        label="GPU Global High Performance Mode",
        category="GPU / Graphics",
        apply_fn=_apply_gpu_prefer_performance,
        remove_fn=_remove_gpu_prefer_performance,
        detect_fn=_detect_gpu_prefer_performance,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GPU_PREF],
        description=(
            "Sets global DirectX GPU preference to high performance with "
            "swap effect upgrade enabled. Improves frame pacing. "
            "Default: System default. Recommended: High Performance."
        ),
        tags=["gpu", "performance", "directx"],
    ),
    TweakDef(
        id="disable-dwm-animations",
        label="Disable DWM Desktop Animations",
        category="GPU / Graphics",
        apply_fn=_apply_disable_dwm_animations,
        remove_fn=_remove_disable_dwm_animations,
        detect_fn=_detect_disable_dwm_animations,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DWMKEY],
        description=(
            "Disables Desktop Window Manager peek/flip animations for a "
            "snappier desktop experience. Reduces GPU compositor overhead. "
            "Default: Enabled. Recommended: Disabled for performance."
        ),
        tags=["gpu", "performance", "dwm", "animations"],
    ),
    TweakDef(
        id="gpu-preemption-disable",
        label="Disable GPU Preemption (Lower Latency)",
        category="GPU / Graphics",
        apply_fn=_apply_gpu_preemption,
        remove_fn=_remove_gpu_preemption,
        detect_fn=_detect_gpu_preemption,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GPU_PREEMPT],
        description=(
            "Disables GPU preemption to reduce render latency. "
            "Can improve frame times in games but may affect multi-tasking. "
            "Default: Enabled. Recommended: Disabled for gaming."
        ),
        tags=["gpu", "performance", "latency", "gaming"],
    ),
    TweakDef(
        id="disable-fullscreen-optimizations-global",
        label="Disable Fullscreen Optimizations Globally",
        category="GPU / Graphics",
        apply_fn=_apply_disable_fse_global,
        remove_fn=_remove_disable_fse_global,
        detect_fn=_detect_disable_fse_global,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GAME_CONFIG],
        description=(
            "Disables fullscreen optimizations globally via GameDVR_FSEBehaviorMode=2. "
            "Prevents Windows from applying borderless windowed mode to fullscreen apps. "
            "Can improve frame pacing and reduce input lag in games. "
            "Default: 0 (Enabled). Recommended: 2 (Disabled)."
        ),
        tags=["gpu", "gaming", "fullscreen", "performance"],
    ),
    TweakDef(
        id="disable-game-bar-overlay",
        label="Disable Game Bar Overlay for GPU",
        category="GPU / Graphics",
        apply_fn=_apply_disable_game_bar_overlay,
        remove_fn=_remove_disable_game_bar_overlay,
        detect_fn=_detect_disable_game_bar_overlay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GAME_BAR],
        description=(
            "Disables the Xbox Game Bar overlay (UseNexusForGameBarEnabled=0). "
            "Reduces GPU overhead and prevents accidental overlay activation. "
            "Default: 1 (Enabled). Recommended: 0 (Disabled)."
        ),
        tags=["gpu", "gaming", "overlay", "game-bar"],
    ),
    TweakDef(
        id="nvidia-tdr-delay",
        label="Increase NVIDIA TDR Delay (8s)",
        category="GPU / Graphics",
        apply_fn=_apply_nvidia_tdr_delay,
        remove_fn=_remove_nvidia_tdr_delay,
        detect_fn=_detect_nvidia_tdr_delay,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GPU_SCHED],
        description=(
            "Increases the GPU TDR (Timeout Detection and Recovery) delay to 8 seconds. "
            "Prevents driver resets during heavy GPU workloads. "
            "Removal deletes the value, restoring the Windows default (2s). "
            "Default: 2s. Recommended: 8s."
        ),
        tags=["gpu", "nvidia", "stability", "tdr"],
    ),
    TweakDef(
        id="disable-gpu-preemption",
        label="Disable GPU Preemption (Low Latency)",
        category="GPU / Graphics",
        apply_fn=_apply_disable_gpu_preemption,
        remove_fn=_remove_disable_gpu_preemption,
        detect_fn=_detect_disable_gpu_preemption,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GPU_PREEMPT],
        description=(
            "Disables GPU preemption (EnablePreemption=0) for lower render latency. "
            "May improve frame times in GPU-bound scenarios but can affect "
            "multi-tasking and system responsiveness. "
            "Default: Enabled. Removal deletes the value."
        ),
        tags=["gpu", "latency", "gaming", "preemption"],
    ),
    TweakDef(
        id="multiplane-overlay-disable",
        label="Disable Multi-Plane Overlay (Anti-Stutter)",
        category="GPU / Graphics",
        apply_fn=_apply_mpo_disable,
        remove_fn=_remove_mpo_disable,
        detect_fn=_detect_mpo_disable,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DWM_LM],
        description=(
            "Disables Multi-Plane Overlay via OverlayTestMode=5 under the HKLM DWM key. "
            "Fixes stuttering, flickering, and black screen issues on some hardware. "
            "Removal deletes the value. Default: Enabled. Recommended: Disabled."
        ),
        tags=["gpu", "display", "stutter", "mpo"],
    ),
    TweakDef(
        id="gpu-force-dx12-ultimate",
        label="Force DirectX 12 Ultimate",
        category="GPU / Graphics",
        apply_fn=_apply_force_dx12_ultimate,
        remove_fn=_remove_force_dx12_ultimate,
        detect_fn=_detect_force_dx12_ultimate,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DX_KEY],
        description=(
            "Forces DirectX 12 mode for all compatible applications. "
            "Enables advanced features like mesh shaders and raytracing. "
            "Default: Auto. Recommended: Enabled for DX12-capable GPUs."
        ),
        tags=["gpu", "directx", "dx12", "performance", "raytracing"],
    ),
    TweakDef(
        id="gpu-disable-igpu-powersave",
        label="Disable Integrated GPU Power Saving",
        category="GPU / Graphics",
        apply_fn=_apply_igpu_clock_tsc,
        remove_fn=_remove_igpu_clock_tsc,
        detect_fn=_detect_igpu_clock_tsc,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GPU_SCHED],
        description=(
            "Enables platform clock constant TSC for GPU scheduling. "
            "Provides more consistent GPU timer resolution, reducing frame time variance. "
            "Default: Not set. Recommended: Enabled."
        ),
        tags=["gpu", "clock", "tsc", "performance", "frame-time"],
    ),
    TweakDef(
        id="gpu-wddm-scheduler",
        label="Optimize WDDM GPU Scheduler",
        category="GPU / Graphics",
        apply_fn=_apply_wddm_scheduler,
        remove_fn=_remove_wddm_scheduler,
        detect_fn=_detect_wddm_scheduler,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GPU_SCHED],
        description=(
            "Optimizes WDDM flip queue length to 2 frames for reduced input latency. "
            "Trades slight throughput for lower frame queue depth. "
            "Default: 3. Recommended: 2 for competitive gaming."
        ),
        tags=["gpu", "wddm", "flip-queue", "latency", "gaming"],
    ),
]


# ── Disable DWM Animations ───────────────────────────────────────────────────


def _apply_disable_dwm_anim(*, require_admin: bool = False) -> None:
    SESSION.log("GPU: disable DWM Aero Peek animations")
    SESSION.backup([_DWMKEY], "DwmAnimations")
    SESSION.set_dword(_DWMKEY, "EnableAeroPeek", 0)


def _remove_disable_dwm_anim(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_DWMKEY, "EnableAeroPeek", 1)


def _detect_disable_dwm_anim() -> bool:
    return SESSION.read_dword(_DWMKEY, "EnableAeroPeek") == 0


# ── Increase GPU TDR Delay ───────────────────────────────────────────────────


def _apply_increase_tdr_delay(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: increase TDR timeout to 10 seconds")
    SESSION.backup([_GPU_SCHED], "TdrDelay")
    SESSION.set_dword(_GPU_SCHED, "TdrDelay", 10)


def _remove_increase_tdr_delay(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GPU_SCHED, "TdrDelay")


def _detect_increase_tdr_delay() -> bool:
    return SESSION.read_dword(_GPU_SCHED, "TdrDelay") == 10


TWEAKS += [
    TweakDef(
        id="gpu-disable-dwm-animations",
        label="Disable DWM Animations",
        category="GPU / Graphics",
        apply_fn=_apply_disable_dwm_anim,
        remove_fn=_remove_disable_dwm_anim,
        detect_fn=_detect_disable_dwm_anim,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DWMKEY],
        description=(
            "Disables DWM Aero Peek animations for reduced GPU overhead. "
            "Saves GPU cycles on compositing effects. "
            "Default: 1 (enabled). Recommended: Disabled for performance."
        ),
        tags=["gpu", "dwm", "animations", "aero-peek", "performance"],
    ),
    TweakDef(
        id="gpu-increase-tdr-delay",
        label="Increase GPU TDR Timeout",
        category="GPU / Graphics",
        apply_fn=_apply_increase_tdr_delay,
        remove_fn=_remove_increase_tdr_delay,
        detect_fn=_detect_increase_tdr_delay,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GPU_SCHED],
        description=(
            "Increases GPU Timeout Detection and Recovery delay to 10 seconds. "
            "Prevents false TDR resets during heavy GPU workloads. "
            "Default: 2s. Recommended: 10s for compute/rendering."
        ),
        tags=["gpu", "tdr", "timeout", "stability", "compute"],
    ),
]


# ── Disable GPU Power Throttling ─────────────────────────────────────────────────────────────


def _apply_disable_gpu_power_throttle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: disable GPU power throttling")
    SESSION.backup([_GPU_SCHED], "GpuPowerThrottle")
    SESSION.set_dword(_GPU_SCHED, "PowerThrottleQos", 0)


def _remove_disable_gpu_power_throttle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GPU_SCHED, "PowerThrottleQos")


def _detect_disable_gpu_power_throttle() -> bool:
    return SESSION.read_dword(_GPU_SCHED, "PowerThrottleQos") == 0


# ── Force Software Cursor ────────────────────────────────────────────────────────────────────


def _apply_force_sw_cursor(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: force software cursor rendering")
    SESSION.backup([_DWM_LM], "SwCursor")
    SESSION.set_dword(_DWM_LM, "UseSWCursor", 1)


def _remove_force_sw_cursor(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DWM_LM, "UseSWCursor")


def _detect_force_sw_cursor() -> bool:
    return SESSION.read_dword(_DWM_LM, "UseSWCursor") == 1


# ── Set Maximum Pre-Rendered Frames to 1 ─────────────────────────────────────────────────────


def _apply_max_prerendered_frames(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("GPU: set max pre-rendered frames to 1")
    SESSION.backup([_GPU_SCHED], "FlipQueueSize")
    SESSION.set_dword(_GPU_SCHED, "FlipQueueSize", 1)


def _remove_max_prerendered_frames(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GPU_SCHED, "FlipQueueSize")


def _detect_max_prerendered_frames() -> bool:
    return SESSION.read_dword(_GPU_SCHED, "FlipQueueSize") == 1


TWEAKS += [
    TweakDef(
        id="gpu-disable-power-throttle",
        label="Disable GPU Power Throttling",
        category="GPU / Graphics",
        apply_fn=_apply_disable_gpu_power_throttle,
        remove_fn=_remove_disable_gpu_power_throttle,
        detect_fn=_detect_disable_gpu_power_throttle,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GPU_SCHED],
        description=(
            "Disables GPU power throttling in the graphics driver scheduler. "
            "Prevents the GPU from downclocking during sustained workloads. "
            "Default: Enabled. Recommended: Disabled for compute workloads."
        ),
        tags=["gpu", "power", "throttle", "performance", "compute"],
    ),
    TweakDef(
        id="gpu-force-software-cursor",
        label="Force Software Cursor",
        category="GPU / Graphics",
        apply_fn=_apply_force_sw_cursor,
        remove_fn=_remove_force_sw_cursor,
        detect_fn=_detect_force_sw_cursor,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DWM_LM],
        description=(
            "Forces DWM to use software cursor rendering instead of hardware. "
            "Can reduce perceived input lag on some GPU/driver combinations. "
            "Default: Hardware cursor. Recommended: Software for low-latency."
        ),
        tags=["gpu", "cursor", "input-lag", "dwm", "latency"],
    ),
    TweakDef(
        id="gpu-max-prerendered-frames",
        label="Set Max Pre-Rendered Frames to 1",
        category="GPU / Graphics",
        apply_fn=_apply_max_prerendered_frames,
        remove_fn=_remove_max_prerendered_frames,
        detect_fn=_detect_max_prerendered_frames,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GPU_SCHED],
        description=(
            "Sets the flip queue size to 1, limiting pre-rendered frames. "
            "Reduces input lag at the cost of slightly lower throughput. "
            "Default: 3 frames. Recommended: 1 for competitive gaming."
        ),
        tags=["gpu", "pre-rendered", "frames", "input-lag", "gaming"],
    ),
]
