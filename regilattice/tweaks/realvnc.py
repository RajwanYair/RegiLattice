r"""RealVNC Server & Viewer registry tweaks.

Covers VNC Server security, connectivity, and Viewer default settings.
RealVNC stores its configuration under HKLM\SOFTWARE\RealVNC.
"""

from __future__ import annotations

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


# ── RealVNC Disable Auto-Update ──────────────────────────────────────────────


def _apply_disable_auto_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: disable automatic update checks")
    SESSION.backup([_VNC_SERVER], "VNCAutoUpdate")
    SESSION.set_dword(_VNC_SERVER, "AutoUpdate", 0)


def _remove_disable_auto_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_VNC_SERVER, "AutoUpdate", 1)


def _detect_disable_auto_update() -> bool:
    return SESSION.read_dword(_VNC_SERVER, "AutoUpdate") == 0


# ── RealVNC Optimize Encoding ────────────────────────────────────────────────


def _apply_optimize_encoding(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: set preferred encoding to ZRLE")
    SESSION.backup([_VNC_SERVER], "VNCEncoding")
    SESSION.set_string(_VNC_SERVER, "PreferredEncoding", "ZRLE")


def _remove_optimize_encoding(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VNC_SERVER, "PreferredEncoding")


def _detect_optimize_encoding() -> bool:
    return SESSION.read_string(_VNC_SERVER, "PreferredEncoding") == "ZRLE"


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
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
    TweakDef(
        id="realvnc-disable-auto-update",
        label="RealVNC Disable Auto-Update",
        category="RealVNC",
        apply_fn=_apply_disable_auto_update,
        remove_fn=_remove_disable_auto_update,
        detect_fn=_detect_disable_auto_update,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_VNC_SERVER],
        description=(
            "Disables RealVNC automatic update checks. Updates must be applied manually. "
            "Default: Enabled. Recommended: Disabled for managed deployments."
        ),
        tags=["realvnc", "vnc", "update"],
    ),
    TweakDef(
        id="realvnc-optimize-encoding",
        label="RealVNC Optimize Encoding",
        category="RealVNC",
        apply_fn=_apply_optimize_encoding,
        remove_fn=_remove_optimize_encoding,
        detect_fn=_detect_optimize_encoding,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_VNC_SERVER],
        description=(
            "Sets VNC encoding to ZRLE for best compression ratio over slow networks. "
            "Reduces bandwidth usage. Default: Auto. Recommended: ZRLE for WAN connections."
        ),
        tags=["realvnc", "vnc", "encoding", "performance", "network"],
    ),
]


# -- VNC Session Idle Timeout -----------------------------------------------------


def _apply_session_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: set idle session timeout to 30 minutes")
    SESSION.backup([_VNC_SERVER], "VNCSessionTimeout")
    SESSION.set_dword(_VNC_SERVER, "IdleTimeout", 1800)


def _remove_session_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VNC_SERVER, "IdleTimeout")


def _detect_session_timeout() -> bool:
    return SESSION.read_dword(_VNC_SERVER, "IdleTimeout") == 1800


# -- VNC Disable Clipboard Sharing ------------------------------------------------


def _apply_vnc_disable_clipboard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: disable clipboard sharing")
    SESSION.backup([_VNC_SERVER], "VNCDisableClipboard")
    SESSION.set_dword(_VNC_SERVER, "DisableClipboard", 1)


def _remove_vnc_disable_clipboard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VNC_SERVER, "DisableClipboard")


def _detect_vnc_disable_clipboard() -> bool:
    return SESSION.read_dword(_VNC_SERVER, "DisableClipboard") == 1


TWEAKS += [
    TweakDef(
        id="vnc-session-timeout",
        label="VNC: Set Idle Session Timeout (30 min)",
        category="RealVNC",
        apply_fn=_apply_session_timeout,
        remove_fn=_remove_session_timeout,
        detect_fn=_detect_session_timeout,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VNC_SERVER],
        description=(
            "Sets VNC idle session timeout to 30 minutes (1800 seconds). "
            "Automatically disconnects idle VNC sessions for security. "
            "Default: no timeout. Recommended: 1800."
        ),
        tags=["vnc", "timeout", "idle", "security"],
    ),
    TweakDef(
        id="vnc-disable-clipboard",
        label="VNC: Disable Clipboard Sharing (DWord)",
        category="RealVNC",
        apply_fn=_apply_vnc_disable_clipboard,
        remove_fn=_remove_vnc_disable_clipboard,
        detect_fn=_detect_vnc_disable_clipboard,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VNC_SERVER],
        description=(
            "Disables clipboard sharing between VNC server and clients via DWORD value. "
            "Prevents data leakage through clipboard transfer. "
            "Default: enabled. Recommended: disabled for DLP."
        ),
        tags=["vnc", "clipboard", "dlp", "security"],
    ),
]


# -- VNC: Enforce Encryption Always On (Policy) --------------------------------


def _apply_vnc_encryption_always(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: set encryption to AlwaysOn via policy")
    SESSION.backup([_VNC_POLICY], "VNCEncryptionAlways")
    SESSION.set_string(_VNC_POLICY, "Encryption", "AlwaysOn")
    SESSION.set_dword(_VNC_POLICY, "EncryptionForced", 1)


def _remove_vnc_encryption_always(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VNC_POLICY, "EncryptionForced")
    SESSION.set_string(_VNC_POLICY, "Encryption", "PreferOn")


def _detect_vnc_encryption_always() -> bool:
    return (
        SESSION.read_string(_VNC_POLICY, "Encryption") == "AlwaysOn"
        and SESSION.read_dword(_VNC_POLICY, "EncryptionForced") == 1
    )


# -- VNC: Disable File Transfer ------------------------------------------------


def _apply_vnc_disable_file_transfer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: disable file transfer")
    SESSION.backup([_VNC_SERVER, _VNC_POLICY], "VNCDisableFileTransfer")
    SESSION.set_dword(_VNC_SERVER, "EnableFileTransfer", 0)
    SESSION.set_dword(_VNC_POLICY, "EnableFileTransfer", 0)


def _remove_vnc_disable_file_transfer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_VNC_SERVER, "EnableFileTransfer", 1)
    SESSION.delete_value(_VNC_POLICY, "EnableFileTransfer")


def _detect_vnc_disable_file_transfer() -> bool:
    return SESSION.read_dword(_VNC_SERVER, "EnableFileTransfer") == 0


# -- VNC: Set Authentication to SystemAuth -------------------------------------


def _apply_vnc_auth_system(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RealVNC: set authentication to SystemAuth")
    SESSION.backup([_VNC_SERVER, _VNC_POLICY], "VNCAuthSystem")
    SESSION.set_string(_VNC_SERVER, "Authentication", "SystemAuth")
    SESSION.set_string(_VNC_POLICY, "Authentication", "SystemAuth")


def _remove_vnc_auth_system(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_VNC_SERVER, "Authentication", "VncAuth")
    SESSION.delete_value(_VNC_POLICY, "Authentication")


def _detect_vnc_auth_system() -> bool:
    return SESSION.read_string(_VNC_SERVER, "Authentication") == "SystemAuth"


TWEAKS += [
    TweakDef(
        id="vnc-encryption-always",
        label="VNC: Enforce Encryption Always On (Policy)",
        category="RealVNC",
        apply_fn=_apply_vnc_encryption_always,
        remove_fn=_remove_vnc_encryption_always,
        detect_fn=_detect_vnc_encryption_always,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VNC_POLICY],
        description=(
            "Forces VNC encryption to AlwaysOn via group policy key and sets EncryptionForced flag. "
            "Ensures connections are always encrypted regardless of server config. "
            "Default: PreferOn. Recommended: AlwaysOn."
        ),
        tags=["vnc", "encryption", "policy", "security"],
    ),
    TweakDef(
        id="vnc-disable-file-transfer",
        label="VNC: Disable File Transfer",
        category="RealVNC",
        apply_fn=_apply_vnc_disable_file_transfer,
        remove_fn=_remove_vnc_disable_file_transfer,
        detect_fn=_detect_vnc_disable_file_transfer,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VNC_SERVER, _VNC_POLICY],
        description=(
            "Disables file transfer capability in VNC sessions. "
            "Prevents users from transferring files via the VNC connection. "
            "Default: enabled. Recommended: disabled for DLP."
        ),
        tags=["vnc", "file-transfer", "dlp", "security"],
    ),
    TweakDef(
        id="vnc-auth-system",
        label="VNC: Set Authentication to SystemAuth",
        category="RealVNC",
        apply_fn=_apply_vnc_auth_system,
        remove_fn=_remove_vnc_auth_system,
        detect_fn=_detect_vnc_auth_system,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VNC_SERVER, _VNC_POLICY],
        description=(
            "Sets VNC authentication to SystemAuth (Windows credentials). "
            "Uses OS-level authentication instead of VNC-specific password. "
            "Default: VncAuth. Recommended: SystemAuth for enterprise."
        ),
        tags=["vnc", "auth", "system", "security"],
    ),
]
