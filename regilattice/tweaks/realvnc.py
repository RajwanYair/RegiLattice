"""RealVNC Server & Viewer registry tweaks.

Covers VNC Server security, connectivity, and Viewer default settings.
RealVNC stores its configuration under HKLM\SOFTWARE\RealVNC.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_VNC_SERVER = r"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"
_VNC_VIEWER = r"HKEY_CURRENT_USER\Software\RealVNC\vncviewer"
_VNC_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver"


# ── Enforce VNC Encryption ───────────────────────────────────────────────────


def _apply_encryption(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: enforce 'AlwaysOn' encryption")
    SESSION.backup([_VNC_SERVER, _VNC_POLICY], "VNCEncryption")
    SESSION.set_string(_VNC_SERVER, "Encryption", "AlwaysOn")
    SESSION.set_string(_VNC_POLICY, "Encryption", "AlwaysOn")


def _remove_encryption(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_VNC_SERVER, "Encryption", "PreferOn")
    SESSION.delete_value(_VNC_POLICY, "Encryption")


def _detect_encryption() -> bool:
    return SESSION.read_string(_VNC_SERVER, "Encryption") == "AlwaysOn"


# ── Set VNC Authentication to VncAuth + SystemAuth ───────────────────────────


def _apply_auth(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: set authentication to VncAuth+SystemAuth")
    SESSION.backup([_VNC_SERVER], "VNCAuth")
    SESSION.set_string(_VNC_SERVER, "Authentication", "VncAuth+SystemAuth")


def _remove_auth(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_VNC_SERVER, "Authentication", "SingleSignOn")


def _detect_auth() -> bool:
    val = SESSION.read_string(_VNC_SERVER, "Authentication")
    return val is not None and "VncAuth" in val


# ── Limit VNC Idle Timeout ──────────────────────────────────────────────────


def _apply_idle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: set idle timeout to 3600s")
    SESSION.backup([_VNC_SERVER], "VNCIdle")
    SESSION.set_dword(_VNC_SERVER, "IdleTimeout", 3600)


def _remove_idle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_VNC_SERVER, "IdleTimeout", 0)  # 0 = no timeout


def _detect_idle() -> bool:
    return SESSION.read_dword(_VNC_SERVER, "IdleTimeout") == 3600


# ── Disable VNC Server Tray Icon ─────────────────────────────────────────────


def _apply_no_tray(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: hide server tray icon")
    SESSION.backup([_VNC_SERVER], "VNCTray")
    SESSION.set_string(_VNC_SERVER, "ConnNotifyPolicy", "None")


def _remove_no_tray(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_VNC_SERVER, "ConnNotifyPolicy", "Notify")


def _detect_no_tray() -> bool:
    return SESSION.read_string(_VNC_SERVER, "ConnNotifyPolicy") == "None"


# ── VNC Viewer: Remember Connection ──────────────────────────────────────────


def _apply_viewer_remember(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC Viewer: enable recent connections list")
    SESSION.backup([_VNC_VIEWER], "VNCViewerRecent")
    SESSION.set_dword(_VNC_VIEWER, "RememberConnections", 1)
    SESSION.set_dword(_VNC_VIEWER, "MaxRecentConnections", 20)


def _remove_viewer_remember(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VNC_VIEWER, "RememberConnections")
    SESSION.delete_value(_VNC_VIEWER, "MaxRecentConnections")


def _detect_viewer_remember() -> bool:
    return SESSION.read_dword(_VNC_VIEWER, "RememberConnections") == 1


# ── VNC Viewer: Scaling Mode ────────────────────────────────────────────────


def _apply_viewer_scaling(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC Viewer: set scaling to 'FitWindow'")
    SESSION.backup([_VNC_VIEWER], "VNCViewerScaling")
    SESSION.set_string(_VNC_VIEWER, "Scaling", "FitWindow")


def _remove_viewer_scaling(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VNC_VIEWER, "Scaling")


def _detect_viewer_scaling() -> bool:
    return SESSION.read_string(_VNC_VIEWER, "Scaling") == "FitWindow"


# ── VNC Server: Blank Screen While Connected ─────────────────────────────


def _apply_blank_screen(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: blank local screen during remote session")
    SESSION.backup([_VNC_SERVER], "VNCBlank")
    SESSION.set_string(_VNC_SERVER, "BlankScreen", "WhenConnected")


def _remove_blank_screen(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_VNC_SERVER, "BlankScreen", "Never")


def _detect_blank_screen() -> bool:
    return SESSION.read_string(_VNC_SERVER, "BlankScreen") == "WhenConnected"


# ── VNC Server: Disable Clipboard Sharing ─────────────────────────────────


def _apply_no_clipboard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: disable clipboard sharing (DLP)")
    SESSION.backup([_VNC_SERVER, _VNC_POLICY], "VNCClipboard")
    SESSION.set_dword(_VNC_SERVER, "AcceptCutText", 0)
    SESSION.set_dword(_VNC_SERVER, "SendCutText", 0)
    SESSION.set_dword(_VNC_POLICY, "AcceptCutText", 0)
    SESSION.set_dword(_VNC_POLICY, "SendCutText", 0)


def _remove_no_clipboard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_VNC_SERVER, "AcceptCutText", 1)
    SESSION.set_dword(_VNC_SERVER, "SendCutText", 1)
    SESSION.delete_value(_VNC_POLICY, "AcceptCutText")
    SESSION.delete_value(_VNC_POLICY, "SendCutText")


def _detect_no_clipboard() -> bool:
    return SESSION.read_dword(_VNC_SERVER, "AcceptCutText") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="vnc-enforce-encryption",
        label="VNC: Enforce Encryption",
        category="RealVNC",
        apply_fn=_apply_encryption,
        remove_fn=_remove_encryption,
        detect_fn=_detect_encryption,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_VNC_SERVER, _VNC_POLICY],
        description="Forces VNC Server to use 'AlwaysOn' encryption for all connections.",
        tags=["vnc", "security", "encryption"],
    ),
    TweakDef(
        id="vnc-strong-auth",
        label="VNC: Strong Authentication",
        category="RealVNC",
        apply_fn=_apply_auth,
        remove_fn=_remove_auth,
        detect_fn=_detect_auth,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_VNC_SERVER],
        description="Sets VNC authentication to VncAuth + System authentication.",
        tags=["vnc", "security", "authentication"],
    ),
    TweakDef(
        id="vnc-idle-timeout",
        label="VNC: 1-Hour Idle Timeout",
        category="RealVNC",
        apply_fn=_apply_idle,
        remove_fn=_remove_idle,
        detect_fn=_detect_idle,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_VNC_SERVER],
        description="Disconnects idle VNC sessions after 1 hour (3600s).",
        tags=["vnc", "security", "timeout"],
    ),
    TweakDef(
        id="vnc-hide-tray",
        label="VNC: Hide Server Tray Icon",
        category="RealVNC",
        apply_fn=_apply_no_tray,
        remove_fn=_remove_no_tray,
        detect_fn=_detect_no_tray,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_VNC_SERVER],
        description="Hides the VNC Server tray icon and disables connection notifications.",
        tags=["vnc", "ui"],
    ),
    TweakDef(
        id="vnc-viewer-recent",
        label="VNC Viewer: Remember Connections",
        category="RealVNC",
        apply_fn=_apply_viewer_remember,
        remove_fn=_remove_viewer_remember,
        detect_fn=_detect_viewer_remember,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_VNC_VIEWER],
        description="Enables recent connections list (up to 20) in VNC Viewer.",
        tags=["vnc", "viewer", "ux"],
    ),
    TweakDef(
        id="vnc-viewer-fitwindow",
        label="VNC Viewer: Fit-to-Window Scaling",
        category="RealVNC",
        apply_fn=_apply_viewer_scaling,
        remove_fn=_remove_viewer_scaling,
        detect_fn=_detect_viewer_scaling,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_VNC_VIEWER],
        description="Sets the default VNC Viewer scaling mode to 'Fit Window'.",
        tags=["vnc", "viewer", "display"],
    ),
    TweakDef(
        id="vnc-blank-screen",
        label="VNC: Blank Screen When Connected",
        category="RealVNC",
        apply_fn=_apply_blank_screen,
        remove_fn=_remove_blank_screen,
        detect_fn=_detect_blank_screen,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_VNC_SERVER],
        description="Blanks the local monitor during an active VNC session for privacy.",
        tags=["vnc", "security", "privacy"],
    ),
    TweakDef(
        id="vnc-no-clipboard",
        label="VNC: Disable Clipboard Sharing",
        category="RealVNC",
        apply_fn=_apply_no_clipboard,
        remove_fn=_remove_no_clipboard,
        detect_fn=_detect_no_clipboard,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_VNC_SERVER, _VNC_POLICY],
        description="Disables clipboard sharing between VNC server and viewer (DLP).",
        tags=["vnc", "security", "clipboard", "dlp"],
    ),
]
