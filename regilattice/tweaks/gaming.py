"""Gaming tweaks — Game DVR / Game Bar."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

_GAMEDVR_CU = r"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR"
_GAMEBAR_CU = r"HKEY_CURRENT_USER\System\GameConfigStore"
_GAMEDVR_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR"
_GAMECONFIG_LM = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default"
    r"\ApplicationManagement\AllowGameDVR"
)
_GAMEMODE_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\GameBar"
)
_FULLSCREEN_CU = (
    r"HKEY_CURRENT_USER\System\GameConfigStore"
)

_KEYS = [_GAMEDVR_CU, _GAMEBAR_CU, _GAMEDVR_POLICY, _GAMECONFIG_LM]


def apply_disable_gamedvr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableGameDVR")
    SESSION.backup(_KEYS, "GameDVR")
    SESSION.set_dword(_GAMEDVR_CU, "AppCaptureEnabled", 0)
    SESSION.set_dword(_GAMEBAR_CU, "GameDVR_Enabled", 0)
    SESSION.set_dword(_GAMEDVR_POLICY, "AllowGameDVR", 0)
    SESSION.set_dword(_GAMECONFIG_LM, "value", 0)
    SESSION.log("Completed Add-DisableGameDVR")


def remove_disable_gamedvr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableGameDVR")
    SESSION.backup(_KEYS, "GameDVR_Remove")
    SESSION.delete_value(_GAMEDVR_CU, "AppCaptureEnabled")
    SESSION.set_dword(_GAMEBAR_CU, "GameDVR_Enabled", 1)
    SESSION.delete_value(_GAMEDVR_POLICY, "AllowGameDVR")
    SESSION.delete_value(_GAMECONFIG_LM, "value")
    SESSION.log("Completed Remove-DisableGameDVR")


def detect_disable_gamedvr() -> bool:
    return SESSION.read_dword(_GAMEBAR_CU, "GameDVR_Enabled") == 0


# ── Disable Game Mode ────────────────────────────────────────────────────────


def _apply_disable_game_mode(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Gaming: disable Windows Game Mode")
    SESSION.backup([_GAMEMODE_CU], "GameMode")
    SESSION.set_dword(_GAMEMODE_CU, "AllowAutoGameMode", 0)
    SESSION.set_dword(_GAMEMODE_CU, "AutoGameModeEnabled", 0)


def _remove_disable_game_mode(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GAMEMODE_CU, "AllowAutoGameMode")
    SESSION.set_dword(_GAMEMODE_CU, "AutoGameModeEnabled", 1)


def _detect_disable_game_mode() -> bool:
    return SESSION.read_dword(_GAMEMODE_CU, "AutoGameModeEnabled") == 0


# ── Disable Fullscreen Optimizations ───────────────────────────────────────


def _apply_disable_fso(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Gaming: disable fullscreen optimizations globally")
    SESSION.backup([_FULLSCREEN_CU], "FSO")
    SESSION.set_dword(_FULLSCREEN_CU, "GameDVR_FSEBehavior", 2)
    SESSION.set_dword(_FULLSCREEN_CU, "GameDVR_DXGIHonorFSEWindowsCompatible", 1)
    SESSION.set_dword(_FULLSCREEN_CU, "GameDVR_FSEBehaviorMode", 2)


def _remove_disable_fso(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FULLSCREEN_CU, "GameDVR_FSEBehavior")
    SESSION.delete_value(_FULLSCREEN_CU, "GameDVR_DXGIHonorFSEWindowsCompatible")
    SESSION.delete_value(_FULLSCREEN_CU, "GameDVR_FSEBehaviorMode")


def _detect_disable_fso() -> bool:
    return SESSION.read_dword(_FULLSCREEN_CU, "GameDVR_FSEBehavior") == 2


# ── Disable Xbox Services ───────────────────────────────────────────────────────────────────

_XBOX_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblAuthManager"
_XBOX_SAVE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblGameSave"


def _apply_disable_xbox_services(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Gaming: disable Xbox background services")
    SESSION.backup([_XBOX_KEY, _XBOX_SAVE], "XboxServices")
    SESSION.set_dword(_XBOX_KEY, "Start", 4)   # Disabled
    SESSION.set_dword(_XBOX_SAVE, "Start", 4)  # Disabled


def _remove_disable_xbox_services(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_XBOX_KEY, "Start", 3)   # Manual
    SESSION.set_dword(_XBOX_SAVE, "Start", 3)  # Manual


def _detect_disable_xbox_services() -> bool:
    return SESSION.read_dword(_XBOX_KEY, "Start") == 4


# ── HAGS + Optimizations for Games (Performance) ────────────────────────────────────────────

_GAME_PERF = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"
_GAMEBAR_TIPS = r"HKEY_CURRENT_USER\Software\Microsoft\GameBar"
_GAME_INPUT = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\GameInput"
_GPU_SCHED = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"
_XBGM = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\xbgm"
_NET_THROTTLE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"
_NAGLE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters"


def _apply_game_priority(*, require_admin: bool = True) -> None:
    """Set game processes to high priority scheduling."""
    assert_admin(require_admin)
    SESSION.log("Gaming: set game task priority to high")
    SESSION.backup([_GAME_PERF], "GamePriority")
    SESSION.set_dword(_GAME_PERF, "Priority", 6)          # High
    SESSION.set_dword(_GAME_PERF, "Scheduling Category", 2)  # High
    SESSION.set_dword(_GAME_PERF, "SFIO Priority", 3)     # High
    SESSION.set_string(_GAME_PERF, "GPU Priority", "8")


def _remove_game_priority(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_GAME_PERF, "Priority", 2)
    SESSION.set_dword(_GAME_PERF, "Scheduling Category", 0)
    SESSION.set_dword(_GAME_PERF, "SFIO Priority", 1)
    SESSION.set_string(_GAME_PERF, "GPU Priority", "8")


def _detect_game_priority() -> bool:
    return SESSION.read_dword(_GAME_PERF, "Priority") == 6


# ── Disable Game Bar Tips/Notifications ──────────────────────────────────────


def _apply_disable_game_bar_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Gaming: disable Game Bar tips/notifications")
    SESSION.backup([_GAMEBAR_TIPS], "GameBarTips")
    SESSION.set_dword(_GAMEBAR_TIPS, "ShowStartupPanel", 0)


def _remove_disable_game_bar_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_GAMEBAR_TIPS, "ShowStartupPanel", 1)


def _detect_disable_game_bar_tips() -> bool:
    return SESSION.read_dword(_GAMEBAR_TIPS, "ShowStartupPanel") == 0


# ── Disable Game Input Redirection ───────────────────────────────────────────


def _apply_disable_game_input_redirect(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Gaming: disable Game Input redirection")
    SESSION.backup([_GAME_INPUT], "GameInput")
    SESSION.set_dword(_GAME_INPUT, "Start", 4)


def _remove_disable_game_input_redirect(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_GAME_INPUT, "Start", 3)


def _detect_disable_game_input_redirect() -> bool:
    return SESSION.read_dword(_GAME_INPUT, "Start") == 4


# ── Enable Hardware-Accelerated GPU Scheduling ───────────────────────────────


def _apply_gpu_scheduling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Gaming: enable hardware-accelerated GPU scheduling")
    SESSION.backup([_GPU_SCHED], "GPUScheduling")
    SESSION.set_dword(_GPU_SCHED, "HwSchMode", 2)


def _remove_gpu_scheduling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_GPU_SCHED, "HwSchMode", 1)


def _detect_gpu_scheduling() -> bool:
    return SESSION.read_dword(_GPU_SCHED, "HwSchMode") == 2


# ── Disable Xbox Game Monitoring Service ─────────────────────────────────────


def _apply_disable_xbox_game_monitoring(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Gaming: disable Xbox Game Monitoring service")
    SESSION.backup([_XBGM], "XboxGameMonitoring")
    SESSION.set_dword(_XBGM, "Start", 4)


def _remove_disable_xbox_game_monitoring(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_XBGM, "Start", 3)


def _detect_disable_xbox_game_monitoring() -> bool:
    return SESSION.read_dword(_XBGM, "Start") == 4


# ── Disable Network Throttling Index ─────────────────────────────────────────


def _apply_network_throttling_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Gaming: disable network throttling index")
    SESSION.backup([_NET_THROTTLE], "NetworkThrottling")
    SESSION.set_dword(_NET_THROTTLE, "NetworkThrottlingIndex", 0xFFFFFFFF)


def _remove_network_throttling_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_NET_THROTTLE, "NetworkThrottlingIndex", 10)


def _detect_network_throttling_off() -> bool:
    return SESSION.read_dword(_NET_THROTTLE, "NetworkThrottlingIndex") == 0xFFFFFFFF


# ── Disable Nagle's Algorithm ────────────────────────────────────────────────


def _apply_disable_nagles_algorithm(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Gaming: disable Nagle's algorithm (low latency)")
    SESSION.backup([_NAGLE], "NaglesAlgorithm")
    SESSION.set_dword(_NAGLE, "TCPNoDelay", 1)


def _remove_disable_nagles_algorithm(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NAGLE, "TCPNoDelay")


def _detect_disable_nagles_algorithm() -> bool:
    return SESSION.read_dword(_NAGLE, "TCPNoDelay") == 1


# ── Disable Game DVR Background Recording ───────────────────────────────────


def _apply_disable_dvr_background(*, require_admin: bool = False) -> None:
    SESSION.log("Gaming: disable Game DVR background recording")
    SESSION.backup([_GAMEBAR_CU, _GAMEDVR_CU], "DVRBackground")
    SESSION.set_dword(_GAMEBAR_CU, "GameDVR_Enabled", 0)
    SESSION.set_dword(_GAMEDVR_CU, "AppCaptureEnabled", 0)


def _remove_disable_dvr_background(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_GAMEBAR_CU, "GameDVR_Enabled", 1)
    SESSION.set_dword(_GAMEDVR_CU, "AppCaptureEnabled", 1)


def _detect_disable_dvr_background() -> bool:
    return SESSION.read_dword(_GAMEBAR_CU, "GameDVR_Enabled") == 0


# ── Enable Game Mode Priority ───────────────────────────────────────────────


def _apply_game_mode_priority(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Gaming: set game threads to highest scheduling priority")
    SESSION.backup([_GAME_PERF], "GameModePriority")
    SESSION.set_dword(_GAME_PERF, "Priority", 6)
    SESSION.set_string(_GAME_PERF, "Scheduling Category", "High")


def _remove_game_mode_priority(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_GAME_PERF, "Priority", 2)
    SESSION.set_string(_GAME_PERF, "Scheduling Category", "Medium")


def _detect_game_mode_priority() -> bool:
    return SESSION.read_dword(_GAME_PERF, "Priority") == 6


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-gamedvr",
        label="Disable Game DVR / Game Bar",
        category="Gaming",
        apply_fn=apply_disable_gamedvr,
        remove_fn=remove_disable_gamedvr,
        detect_fn=detect_disable_gamedvr,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_KEYS,
        description=(
            "Disables Windows Game DVR, Game Bar overlay, and background "
            "recording for better gaming and benchmarking performance."
        ),
        tags=["gaming", "performance", "dvr"],
    ),
    TweakDef(
        id="disable-game-mode",
        label="Disable Windows Game Mode",
        category="Gaming",
        apply_fn=_apply_disable_game_mode,
        remove_fn=_remove_disable_game_mode,
        detect_fn=_detect_disable_game_mode,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GAMEMODE_CU],
        description="Disables Windows Game Mode which can cause stutter in some games.",
        tags=["gaming", "performance", "game-mode"],
    ),
    TweakDef(
        id="disable-fullscreen-optimizations",
        label="Disable Fullscreen Optimizations",
        category="Gaming",
        apply_fn=_apply_disable_fso,
        remove_fn=_remove_disable_fso,
        detect_fn=_detect_disable_fso,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_FULLSCREEN_CU],
        description=(
            "Disables Windows fullscreen optimizations (DX flip model) "
            "which can cause input lag in older games."
        ),
        tags=["gaming", "performance", "fullscreen"],
    ),
    TweakDef(
        id="disable-xbox-services",
        label="Disable Xbox Background Services",
        category="Gaming",
        apply_fn=_apply_disable_xbox_services,
        remove_fn=_remove_disable_xbox_services,
        detect_fn=_detect_disable_xbox_services,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_XBOX_KEY, _XBOX_SAVE],
        description=(
            "Disables Xbox Auth Manager and Xbox Game Save services. "
            "Frees resources if you don't use Xbox Live features. "
            "Options: 3=Manual, 4=Disabled. Default: Manual. Recommended: Disabled."
        ),
        tags=["gaming", "xbox", "services", "performance"],
    ),
    TweakDef(
        id="game-priority-high",
        label="Set Game Task Priority to High (Perf)",
        category="Gaming",
        apply_fn=_apply_game_priority,
        remove_fn=_remove_game_priority,
        detect_fn=_detect_game_priority,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GAME_PERF],
        description=(
            "Elevates scheduling priority for game processes. "
            "Improves frame times by giving games higher CPU/GPU priority. "
            "Default: Normal priority. Recommended: High for gaming PCs."
        ),
        tags=["gaming", "performance", "priority", "scheduling"],
    ),
    TweakDef(
        id="disable-game-bar-tips",
        label="Disable Game Bar Tips/Notifications",
        category="Gaming",
        apply_fn=_apply_disable_game_bar_tips,
        remove_fn=_remove_disable_game_bar_tips,
        detect_fn=_detect_disable_game_bar_tips,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GAMEBAR_TIPS],
        description="Disables the Game Bar startup tips and notification panels.",
        tags=["gaming", "gamebar", "notifications"],
    ),
    TweakDef(
        id="disable-game-input-redirect",
        label="Disable Game Input Redirection",
        category="Gaming",
        apply_fn=_apply_disable_game_input_redirect,
        remove_fn=_remove_disable_game_input_redirect,
        detect_fn=_detect_disable_game_input_redirect,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GAME_INPUT],
        description="Disables the GameInput service to prevent input redirection overhead.",
        tags=["gaming", "input", "services"],
    ),
    TweakDef(
        id="gpu-scheduling",
        label="Enable Hardware-Accelerated GPU Scheduling",
        category="Gaming",
        apply_fn=_apply_gpu_scheduling,
        remove_fn=_remove_gpu_scheduling,
        detect_fn=_detect_gpu_scheduling,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GPU_SCHED],
        description=(
            "Enables hardware-accelerated GPU scheduling (HAGS) for reduced "
            "latency and improved frame scheduling on supported GPUs."
        ),
        tags=["gaming", "performance", "gpu", "hags"],
    ),
    TweakDef(
        id="disable-xbox-game-monitoring",
        label="Disable Xbox Game Monitoring Service",
        category="Gaming",
        apply_fn=_apply_disable_xbox_game_monitoring,
        remove_fn=_remove_disable_xbox_game_monitoring,
        detect_fn=_detect_disable_xbox_game_monitoring,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_XBGM],
        description="Disables the Xbox Game Monitoring (xbgm) background service.",
        tags=["gaming", "xbox", "services", "performance"],
    ),
    TweakDef(
        id="network-throttling-off",
        label="Disable Network Throttling Index (Gaming)",
        category="Gaming",
        apply_fn=_apply_network_throttling_off,
        remove_fn=_remove_network_throttling_off,
        detect_fn=_detect_network_throttling_off,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NET_THROTTLE],
        description=(
            "Sets NetworkThrottlingIndex to 0xFFFFFFFF to disable network "
            "throttling, reducing latency for online gaming."
        ),
        tags=["gaming", "network", "performance", "latency"],
    ),
    TweakDef(
        id="disable-nagles-algorithm",
        label="Disable Nagle's Algorithm (Low Latency)",
        category="Gaming",
        apply_fn=_apply_disable_nagles_algorithm,
        remove_fn=_remove_disable_nagles_algorithm,
        detect_fn=_detect_disable_nagles_algorithm,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_NAGLE],
        description=(
            "Disables Nagle's algorithm via TCPNoDelay for lower network "
            "latency in multiplayer games."
        ),
        tags=["gaming", "network", "latency", "tcp"],
    ),
    TweakDef(
        id="gaming-disable-dvr-background",
        label="Disable Game DVR Background Recording",
        category="Gaming",
        apply_fn=_apply_disable_dvr_background,
        remove_fn=_remove_disable_dvr_background,
        detect_fn=_detect_disable_dvr_background,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GAMEBAR_CU, _GAMEDVR_CU],
        description=(
            "Disables Game DVR background recording. Frees GPU encoder "
            "and disk I/O resources. Default: Enabled. "
            "Recommended: Disabled for maximum FPS."
        ),
        tags=["gaming", "dvr", "recording", "performance"],
    ),
    TweakDef(
        id="gaming-mode-priority",
        label="Enable Game Mode Priority",
        category="Gaming",
        apply_fn=_apply_game_mode_priority,
        remove_fn=_remove_game_mode_priority,
        detect_fn=_detect_game_mode_priority,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GAME_PERF],
        description=(
            "Sets game threads to highest scheduling priority. Reduces "
            "input lag and frame time variance during gameplay. "
            "Default: Medium. Recommended: High."
        ),
        tags=["gaming", "priority", "performance", "latency"],
    ),
]
