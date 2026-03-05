"""Clipboard & Drag-Drop tweaks.

Covers clipboard history, cloud sync, drag-drop sensitivity,
clipboard redirector, and paste behavior.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_CLIPBOARD_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"
)
_CLIPBOARD_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Clipboard"
)
_DRAG_DROP = (
    r"HKEY_CURRENT_USER\Control Panel\Desktop"
)
_EXPLORER_ADV = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Advanced"
)
_TS_CLIENT = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft"
    r"\Windows NT\Terminal Services"
)
_CLOUD_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"
)


# ── Disable Clipboard History ────────────────────────────────────────────────


def _apply_disable_clipboard_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Clipboard: disable clipboard history via policy")
    SESSION.backup([_CLIPBOARD_POLICY], "ClipboardHistory")
    SESSION.set_dword(_CLIPBOARD_POLICY, "AllowClipboardHistory", 0)


def _remove_disable_clipboard_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLIPBOARD_POLICY, "AllowClipboardHistory")


def _detect_disable_clipboard_history() -> bool:
    return SESSION.read_dword(_CLIPBOARD_POLICY, "AllowClipboardHistory") == 0


# ── Enable Clipboard History (User Setting) ──────────────────────────────────


def _apply_enable_clipboard_history(*, require_admin: bool = False) -> None:
    SESSION.log("Clipboard: enable clipboard history (Win+V)")
    SESSION.backup([_CLIPBOARD_CU], "ClipboardHistoryUser")
    SESSION.set_dword(_CLIPBOARD_CU, "EnableClipboardHistory", 1)


def _remove_enable_clipboard_history(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CLIPBOARD_CU, "EnableClipboardHistory", 0)


def _detect_enable_clipboard_history() -> bool:
    return SESSION.read_dword(_CLIPBOARD_CU, "EnableClipboardHistory") == 1


# ── Disable Clipboard Cloud Sync ─────────────────────────────────────────────


def _apply_disable_cloud_clipboard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Clipboard: disable cross-device cloud clipboard sync")
    SESSION.backup([_CLOUD_POLICY], "CloudClipboard")
    SESSION.set_dword(_CLOUD_POLICY, "AllowCrossDeviceClipboard", 0)


def _remove_disable_cloud_clipboard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLOUD_POLICY, "AllowCrossDeviceClipboard")


def _detect_disable_cloud_clipboard() -> bool:
    return SESSION.read_dword(_CLOUD_POLICY, "AllowCrossDeviceClipboard") == 0


# ── Increase Drag-Drop Sensitivity (Pixels) ─────────────────────────────────


def _apply_increase_drag_threshold(*, require_admin: bool = False) -> None:
    SESSION.log("Clipboard: increase drag-drop threshold to 10 pixels")
    SESSION.backup([_DRAG_DROP], "DragThreshold")
    SESSION.set_string(_DRAG_DROP, "DragWidth", "10")
    SESSION.set_string(_DRAG_DROP, "DragHeight", "10")


def _remove_increase_drag_threshold(*, require_admin: bool = False) -> None:
    SESSION.set_string(_DRAG_DROP, "DragWidth", "4")
    SESSION.set_string(_DRAG_DROP, "DragHeight", "4")


def _detect_increase_drag_threshold() -> bool:
    return SESSION.read_string(_DRAG_DROP, "DragWidth") == "10"


# ── Decrease Drag-Drop Sensitivity (Easier Drag) ────────────────────────────


def _apply_decrease_drag_threshold(*, require_admin: bool = False) -> None:
    SESSION.log("Clipboard: decrease drag-drop threshold to 2 pixels")
    SESSION.backup([_DRAG_DROP], "DragThresholdLow")
    SESSION.set_string(_DRAG_DROP, "DragWidth", "2")
    SESSION.set_string(_DRAG_DROP, "DragHeight", "2")


def _remove_decrease_drag_threshold(*, require_admin: bool = False) -> None:
    SESSION.set_string(_DRAG_DROP, "DragWidth", "4")
    SESSION.set_string(_DRAG_DROP, "DragHeight", "4")


def _detect_decrease_drag_threshold() -> bool:
    return SESSION.read_string(_DRAG_DROP, "DragWidth") == "2"


# ── Disable RDP Clipboard Redirection ────────────────────────────────────────


def _apply_disable_rdp_clipboard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Clipboard: disable clipboard redirection in RDP sessions")
    SESSION.backup([_TS_CLIENT], "RdpClipboard")
    SESSION.set_dword(_TS_CLIENT, "fDisableClip", 1)


def _remove_disable_rdp_clipboard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TS_CLIENT, "fDisableClip")


def _detect_disable_rdp_clipboard() -> bool:
    return SESSION.read_dword(_TS_CLIENT, "fDisableClip") == 1


# ── Disable Clipboard Roaming (MSFT Account) ────────────────────────────────


def _apply_disable_clipboard_roaming(*, require_admin: bool = False) -> None:
    SESSION.log("Clipboard: disable clipboard roaming to Microsoft account")
    SESSION.backup([_CLIPBOARD_CU], "ClipboardRoaming")
    SESSION.set_dword(_CLIPBOARD_CU, "CloudClipboardAutomaticUpload", 0)


def _remove_disable_clipboard_roaming(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CLIPBOARD_CU, "CloudClipboardAutomaticUpload")


def _detect_disable_clipboard_roaming() -> bool:
    return SESSION.read_dword(_CLIPBOARD_CU, "CloudClipboardAutomaticUpload") == 0


# ── Set Drag-Drop Delay (milliseconds) ──────────────────────────────────────


def _apply_drag_delay(*, require_admin: bool = False) -> None:
    SESSION.log("Clipboard: set drag start delay to 0 ms (instant)")
    SESSION.backup([_DRAG_DROP], "DragDelay")
    SESSION.set_string(_DRAG_DROP, "DragDelay", "0")


def _remove_drag_delay(*, require_admin: bool = False) -> None:
    SESSION.set_string(_DRAG_DROP, "DragDelay", "200")  # Windows default


def _detect_drag_delay() -> bool:
    return SESSION.read_string(_DRAG_DROP, "DragDelay") == "0"


# ── Disable Clipboard Suggested Actions (Win11 22H2+) ───────────────────────

_SMART_CLIP = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"
)


def _apply_disable_suggested_actions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Clipboard: disable clipboard suggested actions")
    SESSION.backup([_SMART_CLIP], "ClipSuggestedActions")
    SESSION.set_dword(_SMART_CLIP, "EnableClipboardSuggestedActions", 0)


def _remove_disable_suggested_actions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SMART_CLIP, "EnableClipboardSuggestedActions")


def _detect_disable_suggested_actions() -> bool:
    return SESSION.read_dword(_SMART_CLIP, "EnableClipboardSuggestedActions") == 0


# ── Disable Auto-Suggest in Clipboard Panel ──────────────────────────────────


def _apply_disable_clipboard_suggest(*, require_admin: bool = False) -> None:
    SESSION.log("Clipboard: disable text suggestions in clipboard panel")
    SESSION.backup([_CLIPBOARD_CU], "ClipboardSuggest")
    SESSION.set_dword(_CLIPBOARD_CU, "EnableClipboardTextSuggestions", 0)


def _remove_disable_clipboard_suggest(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CLIPBOARD_CU, "EnableClipboardTextSuggestions")


def _detect_disable_clipboard_suggest() -> bool:
    return SESSION.read_dword(_CLIPBOARD_CU, "EnableClipboardTextSuggestions") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="clip-disable-history-policy",
        label="Disable Clipboard History (Policy)",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_disable_clipboard_history,
        remove_fn=_remove_disable_clipboard_history,
        detect_fn=_detect_disable_clipboard_history,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CLIPBOARD_POLICY],
        description=(
            "Disables clipboard history via Group Policy. "
            "Win+V clipboard panel will be unavailable. "
            "Default: not configured. Recommended: 0 (disabled) for privacy."
        ),
        tags=["clipboard", "history", "privacy", "policy"],
    ),
    TweakDef(
        id="clip-enable-history-user",
        label="Enable Clipboard History (Win+V)",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_enable_clipboard_history,
        remove_fn=_remove_enable_clipboard_history,
        detect_fn=_detect_enable_clipboard_history,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CLIPBOARD_CU],
        description=(
            "Enables clipboard history (Win+V) for the current user. "
            "Stores last 25 copied items. "
            "Default: 0 (off). Recommended: 1 (enabled)."
        ),
        tags=["clipboard", "history", "productivity"],
    ),
    TweakDef(
        id="clip-disable-cloud-sync",
        label="Disable Clipboard Cloud Sync",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_disable_cloud_clipboard,
        remove_fn=_remove_disable_cloud_clipboard,
        detect_fn=_detect_disable_cloud_clipboard,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CLOUD_POLICY],
        description=(
            "Disables cross-device clipboard sync via Microsoft account. "
            "Prevents clipboard data from leaving the device. "
            "Default: allowed. Recommended: 0 (disabled) for privacy."
        ),
        tags=["clipboard", "cloud", "sync", "privacy"],
    ),
    TweakDef(
        id="clip-increase-drag-threshold",
        label="Increase Drag-Drop Threshold (10 px)",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_increase_drag_threshold,
        remove_fn=_remove_increase_drag_threshold,
        detect_fn=_detect_increase_drag_threshold,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DRAG_DROP],
        description=(
            "Increases drag start threshold to 10 pixels. "
            "Prevents accidental drag on high-DPI screens. "
            "Default: 4 pixels. Recommended: 10."
        ),
        tags=["clipboard", "drag", "drop", "sensitivity", "dpi"],
    ),
    TweakDef(
        id="clip-decrease-drag-threshold",
        label="Decrease Drag-Drop Threshold (2 px)",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_decrease_drag_threshold,
        remove_fn=_remove_decrease_drag_threshold,
        detect_fn=_detect_decrease_drag_threshold,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DRAG_DROP],
        description=(
            "Decreases drag start threshold to 2 pixels for easier dragging. "
            "Default: 4 pixels. Recommended: 2 (for touchscreen/pen)."
        ),
        tags=["clipboard", "drag", "drop", "sensitivity", "touch"],
    ),
    TweakDef(
        id="clip-disable-rdp-clipboard",
        label="Disable RDP Clipboard Redirection",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_disable_rdp_clipboard,
        remove_fn=_remove_disable_rdp_clipboard,
        detect_fn=_detect_disable_rdp_clipboard,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TS_CLIENT],
        description=(
            "Disables clipboard sharing in Remote Desktop sessions. "
            "Prevents data leakage via copy/paste in RDP. "
            "Default: 0 (allowed). Recommended: 1 (disabled) for security."
        ),
        tags=["clipboard", "rdp", "security", "remote"],
    ),
    TweakDef(
        id="clip-disable-roaming",
        label="Disable Clipboard Roaming",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_disable_clipboard_roaming,
        remove_fn=_remove_disable_clipboard_roaming,
        detect_fn=_detect_disable_clipboard_roaming,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CLIPBOARD_CU],
        description=(
            "Disables automatic clipboard upload to Microsoft account "
            "for cross-device roaming. "
            "Default: enabled. Recommended: 0 (disabled) for privacy."
        ),
        tags=["clipboard", "roaming", "cloud", "privacy"],
    ),
    TweakDef(
        id="clip-instant-drag-delay",
        label="Set Instant Drag Delay (0 ms)",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_drag_delay,
        remove_fn=_remove_drag_delay,
        detect_fn=_detect_drag_delay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DRAG_DROP],
        description=(
            "Removes the 200 ms delay before a drag operation begins. "
            "Makes drag-and-drop feel more responsive. "
            "Default: 200 ms. Recommended: 0."
        ),
        tags=["clipboard", "drag", "delay", "responsiveness"],
    ),
    TweakDef(
        id="clip-disable-suggested-actions",
        label="Disable Clipboard Suggested Actions",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_disable_suggested_actions,
        remove_fn=_remove_disable_suggested_actions,
        detect_fn=_detect_disable_suggested_actions,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SMART_CLIP],
        description=(
            "Disables the suggested actions popup that appears when "
            "copying phone numbers/dates (Win11 22H2+). "
            "Default: enabled. Recommended: 0 (disabled)."
        ),
        tags=["clipboard", "suggestions", "popup", "win11"],
    ),
    TweakDef(
        id="clip-disable-text-suggestions",
        label="Disable Clipboard Text Suggestions",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_disable_clipboard_suggest,
        remove_fn=_remove_disable_clipboard_suggest,
        detect_fn=_detect_disable_clipboard_suggest,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CLIPBOARD_CU],
        description=(
            "Disables text prediction suggestions in the clipboard panel. "
            "Default: enabled. Recommended: 0 (disabled)."
        ),
        tags=["clipboard", "suggestions", "text", "panel"],
    ),
]


# -- Disable Cloud Clipboard Sync -------------------------------------------------


def _apply_disable_cloud_clipboard(*, require_admin: bool = True) -> None:
    SESSION.log("Clipboard: disable cloud clipboard sync")
    SESSION.backup([_CLIPBOARD_CU], "CloudClipboard")
    SESSION.set_dword(_CLIPBOARD_CU, "EnableClipboardHistory", 0)
    SESSION.set_dword(_CLIPBOARD_CU, "CloudClipboardAutomaticUpload", 0)


def _remove_disable_cloud_clipboard(*, require_admin: bool = True) -> None:
    SESSION.delete_value(_CLIPBOARD_CU, "EnableClipboardHistory")
    SESSION.delete_value(_CLIPBOARD_CU, "CloudClipboardAutomaticUpload")


def _detect_disable_cloud_clipboard() -> bool:
    return (
        SESSION.read_dword(_CLIPBOARD_CU, "EnableClipboardHistory") == 0
        and SESSION.read_dword(_CLIPBOARD_CU, "CloudClipboardAutomaticUpload") == 0
    )


# -- Disable Clipboard Roaming (Policy) -------------------------------------------


def _apply_disable_clipboard_roaming(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Clipboard: disable clipboard roaming via policy")
    SESSION.backup([_CLIPBOARD_POLICY], "ClipboardRoaming")
    SESSION.set_dword(_CLIPBOARD_POLICY, "AllowCrossDeviceClipboard", 0)


def _remove_disable_clipboard_roaming(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLIPBOARD_POLICY, "AllowCrossDeviceClipboard")


def _detect_disable_clipboard_roaming() -> bool:
    return SESSION.read_dword(_CLIPBOARD_POLICY, "AllowCrossDeviceClipboard") == 0


TWEAKS += [
    TweakDef(
        id="clip-disable-cloud-clipboard",
        label="Disable Cloud Clipboard Sync",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_disable_cloud_clipboard,
        remove_fn=_remove_disable_cloud_clipboard,
        detect_fn=_detect_disable_cloud_clipboard,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CLIPBOARD_CU],
        description=(
            "Disables cloud clipboard sync and automatic upload. "
            "Prevents clipboard data from being sent to Microsoft cloud services. "
            "Default: enabled. Recommended: disabled for privacy."
        ),
        tags=["clipboard", "cloud", "sync", "privacy"],
    ),
    TweakDef(
        id="clip-disable-clipboard-roaming",
        label="Disable Clipboard Roaming (Policy)",
        category="Clipboard & Drag-Drop",
        apply_fn=_apply_disable_clipboard_roaming,
        remove_fn=_remove_disable_clipboard_roaming,
        detect_fn=_detect_disable_clipboard_roaming,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CLIPBOARD_POLICY],
        description=(
            "Disables cross-device clipboard roaming via Group Policy. "
            "Prevents clipboard content from syncing across devices. "
            "Default: allowed. Recommended: disabled for security."
        ),
        tags=["clipboard", "roaming", "policy", "cross-device"],
    ),
]
