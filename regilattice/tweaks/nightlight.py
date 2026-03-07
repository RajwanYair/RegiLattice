"""Night Light & HDR tweaks — Display colour temperature and HDR settings.

Covers: Night light scheduling, HDR playback, colour management,
auto-brightness, and display calibration policies.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_BLU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current"
    r"\default$windows.data.bluelightreduction.settings\windows.data.bluelightreduction.settings"
)
_BLU_STATE = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current"
    r"\default$windows.data.bluelightreduction.bluelightreductionstate\windows.data.bluelightreduction.bluelightreductionstate"
)
_HDR = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"
_DISPLAY_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display"
_COLOR_MGMT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM"
_GRAPHICS = r"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences"
_AUTOBRIGHTNESS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SensorSet"
_GAMMA_USER = r"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\ICM"
_WCG = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"
_HDR_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM"
_DISPLAY_ENHANCE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"


# ── Enable Night Light ──────────────────────────────────────────────────────


def _apply_enable_night_light(*, require_admin: bool = False) -> None:
    SESSION.log("Night Light: enable night light (blue light reduction)")
    SESSION.backup([_BLU_STATE], "NightLight")
    SESSION.set_dword(_BLU_STATE, "Data", 0x02)


def _remove_enable_night_light(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_BLU_STATE, "Data")


def _detect_enable_night_light() -> bool:
    val = SESSION.read_dword(_BLU_STATE, "Data")
    return val is not None and val != 0


# ── Enable HDR ──────────────────────────────────────────────────────────────


def _apply_enable_hdr(*, require_admin: bool = False) -> None:
    SESSION.log("Night Light: enable HDR video playback")
    SESSION.backup([_HDR], "HDR")
    SESSION.set_dword(_HDR, "EnableHDR", 1)


def _remove_enable_hdr(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_HDR, "EnableHDR", 0)


def _detect_enable_hdr() -> bool:
    return SESSION.read_dword(_HDR, "EnableHDR") == 1


# ── Enable HDR Auto-Brightness ──────────────────────────────────────────────


def _apply_hdr_auto_brightness(*, require_admin: bool = False) -> None:
    SESSION.log("Night Light: enable auto HDR brightness adjustment")
    SESSION.backup([_HDR], "HDRAutoBrightness")
    SESSION.set_dword(_HDR, "AutoHDR", 1)


def _remove_hdr_auto_brightness(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_HDR, "AutoHDR", 0)


def _detect_hdr_auto_brightness() -> bool:
    return SESSION.read_dword(_HDR, "AutoHDR") == 1


# ── Disable Content Adaptive Brightness (CABC) ──────────────────────────────


def _apply_disable_cabc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Night Light: disable Content Adaptive Brightness Control (CABC)")
    SESSION.backup([_DISPLAY_ENHANCE], "CABC")
    SESSION.set_dword(_DISPLAY_ENHANCE, "CABCEnabled", 0)


def _remove_disable_cabc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DISPLAY_ENHANCE, "CABCEnabled")


def _detect_disable_cabc() -> bool:
    return SESSION.read_dword(_DISPLAY_ENHANCE, "CABCEnabled") == 0


# ── Disable Adaptive Colour ─────────────────────────────────────────────────


def _apply_disable_adaptive_colour(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Night Light: disable adaptive colour adjustment")
    SESSION.backup([_COLOR_MGMT], "AdaptiveColour")
    SESSION.set_dword(_COLOR_MGMT, "AdaptiveColorEnabled", 0)


def _remove_disable_adaptive_colour(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_COLOR_MGMT, "AdaptiveColorEnabled")


def _detect_disable_adaptive_colour() -> bool:
    return SESSION.read_dword(_COLOR_MGMT, "AdaptiveColorEnabled") == 0


# ── Enable Wide Colour Gamut ────────────────────────────────────────────────


def _apply_enable_wcg(*, require_admin: bool = False) -> None:
    SESSION.log("Night Light: enable Wide Colour Gamut (WCG)")
    SESSION.backup([_WCG], "WCG")
    SESSION.set_dword(_WCG, "EnableWCG", 1)


def _remove_enable_wcg(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_WCG, "EnableWCG", 0)


def _detect_enable_wcg() -> bool:
    return SESSION.read_dword(_WCG, "EnableWCG") == 1


# ── Disable HDR Streaming ───────────────────────────────────────────────────


def _apply_disable_hdr_streaming(*, require_admin: bool = False) -> None:
    SESSION.log("Night Light: disable streaming HDR video")
    SESSION.backup([_HDR], "HDRStreaming")
    SESSION.set_dword(_HDR, "EnableHDRForStreamingVideo", 0)


def _remove_disable_hdr_streaming(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_HDR, "EnableHDRForStreamingVideo", 1)


def _detect_disable_hdr_streaming() -> bool:
    return SESSION.read_dword(_HDR, "EnableHDRForStreamingVideo") == 0


# ── Force Per-Process GPU Selection ──────────────────────────────────────────


def _apply_per_process_gpu(*, require_admin: bool = False) -> None:
    SESSION.log("Night Light: enable per-process GPU preference")
    SESSION.backup([_GRAPHICS], "PerProcessGPU")
    SESSION.set_string(_GRAPHICS, "DirectXUserGlobalSettings", "SwapEffectUpgradeEnable=1;")


def _remove_per_process_gpu(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_GRAPHICS, "DirectXUserGlobalSettings")


def _detect_per_process_gpu() -> bool:
    val = SESSION.read_string(_GRAPHICS, "DirectXUserGlobalSettings")
    return val is not None and "SwapEffectUpgradeEnable=1" in val


# ── Disable Display Policy ──────────────────────────────────────────────────


def _apply_disable_display_gp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Night Light: disable display settings modification via policy")
    SESSION.backup([_DISPLAY_POLICY], "DisplayPolicy")
    SESSION.set_dword(_DISPLAY_POLICY, "DisableDisplaySettings", 1)


def _remove_disable_display_gp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DISPLAY_POLICY, "DisableDisplaySettings")


def _detect_disable_display_gp() -> bool:
    return SESSION.read_dword(_DISPLAY_POLICY, "DisableDisplaySettings") == 1


# ── Disable HDR Auto-Toggle on Battery ──────────────────────────────────────


def _apply_disable_hdr_battery(*, require_admin: bool = False) -> None:
    SESSION.log("Night Light: keep HDR enabled on battery power")
    SESSION.backup([_HDR], "HDRBattery")
    SESSION.set_dword(_HDR, "DisableHDROnBattery", 0)


def _remove_disable_hdr_battery(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_HDR, "DisableHDROnBattery", 1)


def _detect_disable_hdr_battery() -> bool:
    return SESSION.read_dword(_HDR, "DisableHDROnBattery") == 0


# ── Set Default Colour Profile to sRGB ──────────────────────────────────────


def _apply_srgb_default(*, require_admin: bool = False) -> None:
    SESSION.log("Night Light: set default colour profile to sRGB IEC61966-2.1")
    SESSION.backup([_GAMMA_USER], "sRGB")
    SESSION.set_string(_GAMMA_USER, "ICMProfile", "sRGB IEC61966-2.1")


def _remove_srgb_default(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_GAMMA_USER, "ICMProfile")


def _detect_srgb_default() -> bool:
    val = SESSION.read_string(_GAMMA_USER, "ICMProfile")
    return val is not None and "sRGB" in val


# ── Disable DWM HDR Compositor ──────────────────────────────────────────────


def _apply_disable_dwm_hdr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Night Light: disable DWM HDR compositor mode via policy")
    SESSION.backup([_HDR_POLICY], "DWMHDR")
    SESSION.set_dword(_HDR_POLICY, "DisableHDR", 1)


def _remove_disable_dwm_hdr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_HDR_POLICY, "DisableHDR")


def _detect_disable_dwm_hdr() -> bool:
    return SESSION.read_dword(_HDR_POLICY, "DisableHDR") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="night-enable-night-light",
        label="Enable Night Light (Blue Light Filter)",
        category="Night Light & Display",
        apply_fn=_apply_enable_night_light,
        remove_fn=_remove_enable_night_light,
        detect_fn=_detect_enable_night_light,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_BLU_STATE],
        description="Enables Windows Night Light to reduce blue light emission. Schedule can be configured in Windows Settings.",
        tags=["night-light", "blue-light", "display", "eye-strain"],
    ),
    TweakDef(
        id="night-enable-hdr",
        label="Enable HDR Video Playback",
        category="Night Light & Display",
        apply_fn=_apply_enable_hdr,
        remove_fn=_remove_enable_hdr,
        detect_fn=_detect_enable_hdr,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_HDR],
        description="Enables HDR video playback on HDR-capable displays. Requires hardware support. Default: Disabled.",
        tags=["night-light", "hdr", "display", "video"],
    ),
    TweakDef(
        id="night-hdr-auto-brightness",
        label="Enable Auto HDR Brightness",
        category="Night Light & Display",
        apply_fn=_apply_hdr_auto_brightness,
        remove_fn=_remove_hdr_auto_brightness,
        detect_fn=_detect_hdr_auto_brightness,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_HDR],
        description="Enables automatic brightness adjustment for HDR content. Optimises SDR-to-HDR content mapping.",
        tags=["night-light", "hdr", "brightness", "auto"],
    ),
    TweakDef(
        id="night-disable-cabc",
        label="Disable Content Adaptive Brightness",
        category="Night Light & Display",
        apply_fn=_apply_disable_cabc,
        remove_fn=_remove_disable_cabc,
        detect_fn=_detect_disable_cabc,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DISPLAY_ENHANCE],
        description=(
            "Disables Content Adaptive Brightness Control (CABC) which adjusts screen brightness based on content. "
            "Can cause distracting brightness shifts. Recommended: Disabled for content creation."
        ),
        tags=["night-light", "brightness", "cabc", "display"],
    ),
    TweakDef(
        id="night-disable-adaptive-colour",
        label="Disable Adaptive Colour",
        category="Night Light & Display",
        apply_fn=_apply_disable_adaptive_colour,
        remove_fn=_remove_disable_adaptive_colour,
        detect_fn=_detect_disable_adaptive_colour,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_COLOR_MGMT],
        description="Disables the adaptive colour feature that shifts display colours based on ambient light. Provides consistent colour output.",
        tags=["night-light", "colour", "adaptive", "calibration"],
    ),
    TweakDef(
        id="night-enable-wcg",
        label="Enable Wide Colour Gamut (WCG)",
        category="Night Light & Display",
        apply_fn=_apply_enable_wcg,
        remove_fn=_remove_enable_wcg,
        detect_fn=_detect_enable_wcg,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_WCG],
        description="Enables Wide Colour Gamut support for richer colours on compatible displays. Default: Disabled.",
        tags=["night-light", "wcg", "colour", "gamut", "display"],
    ),
    TweakDef(
        id="night-disable-hdr-streaming",
        label="Disable HDR for Streaming Video",
        category="Night Light & Display",
        apply_fn=_apply_disable_hdr_streaming,
        remove_fn=_remove_disable_hdr_streaming,
        detect_fn=_detect_disable_hdr_streaming,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_HDR],
        description="Disables HDR playback for streaming video apps. Saves bandwidth and prevents colour issues on unsupported displays.",
        tags=["night-light", "hdr", "streaming", "video"],
    ),
    TweakDef(
        id="night-per-process-gpu",
        label="Enable Per-Process GPU Selection",
        category="Night Light & Display",
        apply_fn=_apply_per_process_gpu,
        remove_fn=_remove_per_process_gpu,
        detect_fn=_detect_per_process_gpu,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GRAPHICS],
        description="Enables DirectX swap chain upgrade for better GPU selection per application. May improve hybrid GPU laptops.",
        tags=["night-light", "gpu", "directx", "per-process"],
    ),
    TweakDef(
        id="night-disable-display-gp",
        label="Lock Display Settings (Policy)",
        category="Night Light & Display",
        apply_fn=_apply_disable_display_gp,
        remove_fn=_remove_disable_display_gp,
        detect_fn=_detect_disable_display_gp,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DISPLAY_POLICY],
        description="Prevents users from changing display settings via Group Policy. Useful for kiosk/shared machines.",
        tags=["night-light", "display", "policy", "lock"],
    ),
    TweakDef(
        id="night-keep-hdr-battery",
        label="Keep HDR on Battery Power",
        category="Night Light & Display",
        apply_fn=_apply_disable_hdr_battery,
        remove_fn=_remove_disable_hdr_battery,
        detect_fn=_detect_disable_hdr_battery,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_HDR],
        description="Prevents Windows from automatically disabling HDR when running on battery. May reduce battery life.",
        tags=["night-light", "hdr", "battery", "laptop"],
    ),
    TweakDef(
        id="night-srgb-default",
        label="Set Default Colour Profile to sRGB",
        category="Night Light & Display",
        apply_fn=_apply_srgb_default,
        remove_fn=_remove_srgb_default,
        detect_fn=_detect_srgb_default,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GAMMA_USER],
        description="Sets the default colour profile to standard sRGB IEC61966-2.1. Ensures consistent colour across applications.",
        tags=["night-light", "colour", "srgb", "profile", "calibration"],
    ),
    TweakDef(
        id="night-disable-dwm-hdr",
        label="Disable DWM HDR Compositor (Policy)",
        category="Night Light & Display",
        apply_fn=_apply_disable_dwm_hdr,
        remove_fn=_remove_disable_dwm_hdr,
        detect_fn=_detect_disable_dwm_hdr,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_HDR_POLICY],
        description="Disables the Desktop Window Manager HDR compositor via policy. Force SDR mode even on HDR displays.",
        tags=["night-light", "hdr", "dwm", "compositor", "policy"],
    ),
]
