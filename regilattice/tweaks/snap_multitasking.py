"""Snap & Multitasking tweaks.

Covers Snap Assist/Layouts, Alt+Tab behaviour, virtual desktop edge swipe,
Aero Shake, title bar window shake, and multitasking animations.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_SNAP = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"
_SNAP_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI"
_MULTITASK = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView"
_DWMKEY = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"
_DWM_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM"
_ALTTAB = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"
_VD = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"

# ── 1. Disable Snap Assist ───────────────────────────────────────────────────


def _apply_disable_snap_assist(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "SnapAssist", 0)


def _remove_disable_snap_assist(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "SnapAssist", 1)


def _detect_disable_snap_assist() -> bool:
    return SESSION.read_dword(_SNAP, "SnapAssist") == 0


# ── 2. Disable Snap Layouts (Win11 hover) ────────────────────────────────────


def _apply_disable_snap_layouts(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "EnableSnapAssistFlyout", 0)


def _remove_disable_snap_layouts(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "EnableSnapAssistFlyout", 1)


def _detect_disable_snap_layouts() -> bool:
    return SESSION.read_dword(_SNAP, "EnableSnapAssistFlyout") == 0


# ── 3. Disable Snap Groups (Alt+Tab grouping) ────────────────────────────────


def _apply_disable_snap_groups(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "EnableTaskGroups", 0)


def _remove_disable_snap_groups(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "EnableTaskGroups", 1)


def _detect_disable_snap_groups() -> bool:
    return SESSION.read_dword(_SNAP, "EnableTaskGroups") == 0


# ── 4. Alt+Tab show only open windows (no Edge tabs) ─────────────────────────


def _apply_alttab_windows_only(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_MULTITASK, "Enabled", 0)


def _remove_alttab_windows_only(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_MULTITASK, "Enabled")


def _detect_alttab_windows_only() -> bool:
    return SESSION.read_dword(_MULTITASK, "Enabled") == 0


# ── 5. Disable Aero Shake (title bar shake to minimize) ──────────────────────


def _apply_disable_aero_shake(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "DisallowShaking", 1)


def _remove_disable_aero_shake(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "DisallowShaking", 0)


def _detect_disable_aero_shake() -> bool:
    return SESSION.read_dword(_SNAP, "DisallowShaking") == 1


# ── 6. Disable window animations ─────────────────────────────────────────────

_ANIM_KEY = r"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"


def _apply_disable_window_anim(*, require_admin: bool = False) -> None:
    SESSION.set_string(_ANIM_KEY, "MinAnimate", "0")


def _remove_disable_window_anim(*, require_admin: bool = False) -> None:
    SESSION.set_string(_ANIM_KEY, "MinAnimate", "1")


def _detect_disable_window_anim() -> bool:
    return SESSION.read_string(_ANIM_KEY, "MinAnimate") == "0"


# ── 7. Disable virtual desktop edge swipe ────────────────────────────────────


def _apply_disable_vd_swipe(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SNAP_POLICY, "AllowEdgeSwipe", 0)


def _remove_disable_vd_swipe(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SNAP_POLICY, "AllowEdgeSwipe")


def _detect_disable_vd_swipe() -> bool:
    return SESSION.read_dword(_SNAP_POLICY, "AllowEdgeSwipe") == 0


# ── 8. Show virtual desktops on all monitors ─────────────────────────────────


def _apply_vd_all_monitors(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_VD, "VirtualDesktopTaskbarFilter", 0)


def _remove_vd_all_monitors(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_VD, "VirtualDesktopTaskbarFilter", 1)


def _detect_vd_all_monitors() -> bool:
    return SESSION.read_dword(_VD, "VirtualDesktopTaskbarFilter") == 0


# ── 9. Disable auto-arrange windows when docked ──────────────────────────────


def _apply_disable_auto_arrange(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "JointResize", 0)


def _remove_disable_auto_arrange(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "JointResize", 1)


def _detect_disable_auto_arrange() -> bool:
    return SESSION.read_dword(_SNAP, "JointResize") == 0


# ── 10. Disable snap fill available space ─────────────────────────────────────


def _apply_disable_snap_fill(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "SnapFill", 0)


def _remove_disable_snap_fill(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "SnapFill", 1)


def _detect_disable_snap_fill() -> bool:
    return SESSION.read_dword(_SNAP, "SnapFill") == 0


# ── 11. Disable Show suggestions in Snapped windows ──────────────────────────


def _apply_disable_snap_suggest(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "SnapAssist", 0)
    SESSION.set_dword(_SNAP, "DITest", 0)


def _remove_disable_snap_suggest(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "DITest", 1)


def _detect_disable_snap_suggest() -> bool:
    return SESSION.read_dword(_SNAP, "DITest") == 0


# ── 12. Disable DWM animations policy ────────────────────────────────────────


def _apply_disable_dwm_anim_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_DWM_POLICY, "DisallowAnimations", 1)


def _remove_disable_dwm_anim_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DWM_POLICY, "DisallowAnimations")


def _detect_disable_dwm_anim_policy() -> bool:
    return SESSION.read_dword(_DWM_POLICY, "DisallowAnimations") == 1


# ── TWEAKS list ──────────────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="snap-disable-snap-assist",
        label="Disable Snap Assist",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_snap_assist,
        remove_fn=_remove_disable_snap_assist,
        detect_fn=_detect_disable_snap_assist,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SNAP],
        description="Disable showing suggestions when snapping windows. Default: enabled. Recommended: personal preference.",
        tags=["snap", "assist", "window", "suggestion"],
    ),
    TweakDef(
        id="snap-disable-snap-layouts",
        label="Disable Snap Layouts Flyout",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_snap_layouts,
        remove_fn=_remove_disable_snap_layouts,
        detect_fn=_detect_disable_snap_layouts,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SNAP],
        description="Disable the hover-over maximize button Snap Layouts flyout (Win11). Default: enabled.",
        tags=["snap", "layouts", "flyout", "maximize", "win11"],
    ),
    TweakDef(
        id="snap-disable-snap-groups",
        label="Disable Snap Groups in Alt+Tab",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_snap_groups,
        remove_fn=_remove_disable_snap_groups,
        detect_fn=_detect_disable_snap_groups,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SNAP],
        description="Disable grouping Snap windows in Alt+Tab and taskbar. Default: enabled.",
        tags=["snap", "groups", "alt-tab", "taskbar"],
    ),
    TweakDef(
        id="snap-alttab-windows-only",
        label="Alt+Tab: Open Windows Only",
        category="Snap & Multitasking",
        apply_fn=_apply_alttab_windows_only,
        remove_fn=_remove_alttab_windows_only,
        detect_fn=_detect_alttab_windows_only,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_MULTITASK],
        description="Show only open windows in Alt+Tab, not browser tabs. Default: includes Edge tabs.",
        tags=["alt-tab", "tabs", "edge", "windows"],
    ),
    TweakDef(
        id="snap-disable-aero-shake",
        label="Disable Aero Shake",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_aero_shake,
        remove_fn=_remove_disable_aero_shake,
        detect_fn=_detect_disable_aero_shake,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SNAP],
        description="Disable shaking a window title bar to minimise all others. Default: enabled.",
        tags=["aero", "shake", "minimize"],
    ),
    TweakDef(
        id="snap-disable-window-animations",
        label="Disable Window Min/Max Animations",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_window_anim,
        remove_fn=_remove_disable_window_anim,
        detect_fn=_detect_disable_window_anim,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ANIM_KEY],
        description="Disable minimize/maximize window animation for snappier feel. Default: enabled.",
        tags=["animation", "minimize", "maximize", "performance"],
    ),
    TweakDef(
        id="snap-disable-vd-edge-swipe",
        label="Disable Virtual Desktop Edge Swipe",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_vd_swipe,
        remove_fn=_remove_disable_vd_swipe,
        detect_fn=_detect_disable_vd_swipe,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SNAP_POLICY],
        description="Disable edge swipe to switch virtual desktops (policy). Default: enabled.",
        tags=["virtual-desktop", "edge", "swipe", "gesture"],
    ),
    TweakDef(
        id="snap-vd-all-monitors",
        label="Show Desktops on All Monitors",
        category="Snap & Multitasking",
        apply_fn=_apply_vd_all_monitors,
        remove_fn=_remove_vd_all_monitors,
        detect_fn=_detect_vd_all_monitors,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_VD],
        description="Show virtual desktop windows on all monitors in taskbar. Default: current monitor only.",
        tags=["virtual-desktop", "monitor", "taskbar"],
    ),
    TweakDef(
        id="snap-disable-auto-arrange",
        label="Disable Auto-Arrange on Dock",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_auto_arrange,
        remove_fn=_remove_disable_auto_arrange,
        detect_fn=_detect_disable_auto_arrange,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SNAP],
        description="Disable automatic window rearrangement when docking/undocking. Default: enabled.",
        tags=["dock", "arrange", "resize"],
    ),
    TweakDef(
        id="snap-disable-snap-fill",
        label="Disable Snap Fill Available Space",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_snap_fill,
        remove_fn=_remove_disable_snap_fill,
        detect_fn=_detect_disable_snap_fill,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SNAP],
        description="Disable automatically filling available space when snapping a window. Default: enabled.",
        tags=["snap", "fill", "space", "resize"],
    ),
    TweakDef(
        id="snap-disable-snap-suggestions",
        label="Disable Snap Window Suggestions",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_snap_suggest,
        remove_fn=_remove_disable_snap_suggest,
        detect_fn=_detect_disable_snap_suggest,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SNAP],
        description="Disable AI/suggested windows when snapping. Default: enabled.",
        tags=["snap", "suggestion", "ai"],
    ),
    TweakDef(
        id="snap-disable-dwm-anim-policy",
        label="Disable DWM Animations (Policy)",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_dwm_anim_policy,
        remove_fn=_remove_disable_dwm_anim_policy,
        detect_fn=_detect_disable_dwm_anim_policy,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DWM_POLICY],
        description="Machine-wide policy to disable Desktop Window Manager animations. Default: enabled.",
        tags=["dwm", "animation", "policy"],
    ),
]


# -- 13. Disable Edge Swipe Navigation ───────────────────────────────────────


def _apply_disable_edge_swipe_nav(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SNAP_POLICY], "EdgeSwipeNav")
    SESSION.set_dword(_SNAP_POLICY, "AllowEdgeSwipe", 0)


def _remove_disable_edge_swipe_nav(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SNAP_POLICY, "AllowEdgeSwipe", 1)


def _detect_disable_edge_swipe_nav() -> bool:
    return SESSION.read_dword(_SNAP_POLICY, "AllowEdgeSwipe") == 0


# -- 14. Disable Desktop Peek ───────────────────────────────────────────────


def _apply_disable_peek(*, require_admin: bool = False) -> None:
    SESSION.backup([_SNAP], "DesktopPeek")
    SESSION.set_dword(_SNAP, "DisablePreviewDesktop", 1)


def _remove_disable_peek(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SNAP, "DisablePreviewDesktop", 0)


def _detect_disable_peek() -> bool:
    return SESSION.read_dword(_SNAP, "DisablePreviewDesktop") == 1


TWEAKS += [
    TweakDef(
        id="snap-disable-edge-swipe-nav",
        label="Disable Edge Swipe Navigation",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_edge_swipe_nav,
        remove_fn=_remove_disable_edge_swipe_nav,
        detect_fn=_detect_disable_edge_swipe_nav,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SNAP_POLICY],
        description="Disables edge swipe navigation gestures on touchscreens. Default: Enabled. Recommended: Disabled on desktops.",
        tags=["snap", "edge", "swipe", "gesture", "touch"],
    ),
    TweakDef(
        id="snap-disable-desktop-peek",
        label="Disable Desktop Peek",
        category="Snap & Multitasking",
        apply_fn=_apply_disable_peek,
        remove_fn=_remove_disable_peek,
        detect_fn=_detect_disable_peek,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SNAP],
        description="Disables Aero Peek / desktop preview when hovering over the Show Desktop button. Default: Enabled.",
        tags=["snap", "peek", "desktop", "aero"],
    ),
]
