"""Remote Desktop tweaks.

Covers RDP enable/disable, Network Level Authentication, encryption level,
keep-alive, persistent bitmap caching, port configuration, and shadow sessions.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_RDP = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server"
_RDP_TCP = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp"
_RDP_FW = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\StandardProfile\RemoteAdminSettings"
_TS_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"
_RDP_NLA = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp"

# ── 1. Enable Remote Desktop ─────────────────────────────────────────────────


def _apply_enable_rdp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP, "fDenyTSConnections", 0)


def _remove_enable_rdp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP, "fDenyTSConnections", 1)


def _detect_enable_rdp() -> bool:
    return SESSION.read_dword(_RDP, "fDenyTSConnections") == 0


# ── 2. Require NLA for RDP ───────────────────────────────────────────────────


def _apply_require_nla(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP_NLA, "UserAuthentication", 1)


def _remove_require_nla(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP_NLA, "UserAuthentication", 0)


def _detect_require_nla() -> bool:
    return SESSION.read_dword(_RDP_NLA, "UserAuthentication") == 1


# ── 3. Set RDP encryption to High ────────────────────────────────────────────


def _apply_rdp_high_encryption(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP_TCP, "MinEncryptionLevel", 3)


def _remove_rdp_high_encryption(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP_TCP, "MinEncryptionLevel", 1)


def _detect_rdp_high_encryption() -> bool:
    return SESSION.read_dword(_RDP_TCP, "MinEncryptionLevel") == 3


# ── 4. Enable RDP keep-alive ─────────────────────────────────────────────────


def _apply_rdp_keepalive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP_TCP, "KeepAliveEnable", 1)
    SESSION.set_dword(_RDP_TCP, "KeepAliveInterval", 1)


def _remove_rdp_keepalive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP_TCP, "KeepAliveEnable", 0)
    SESSION.delete_value(_RDP_TCP, "KeepAliveInterval")


def _detect_rdp_keepalive() -> bool:
    return SESSION.read_dword(_RDP_TCP, "KeepAliveEnable") == 1


# ── 5. Enable persistent bitmap caching ──────────────────────────────────────

_BITMAP = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"


def _apply_rdp_bitmap_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BITMAP, "AllowPersistentBitmapCaching", 1)


def _remove_rdp_bitmap_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BITMAP, "AllowPersistentBitmapCaching")


def _detect_rdp_bitmap_cache() -> bool:
    return SESSION.read_dword(_BITMAP, "AllowPersistentBitmapCaching") == 1


# ── 6. Change RDP port (3390 instead of 3389) ────────────────────────────────


def _apply_rdp_port_3390(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP_TCP, "PortNumber", 3390)


def _remove_rdp_port_3390(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP_TCP, "PortNumber", 3389)


def _detect_rdp_port_3390() -> bool:
    return SESSION.read_dword(_RDP_TCP, "PortNumber") == 3390


# ── 7. Disable RDP clipboard redirection ─────────────────────────────────────


def _apply_disable_rdp_clipboard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TS_POLICY, "fDisableClip", 1)


def _remove_disable_rdp_clipboard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TS_POLICY, "fDisableClip")


def _detect_disable_rdp_clipboard() -> bool:
    return SESSION.read_dword(_TS_POLICY, "fDisableClip") == 1


# ── 8. Disable RDP drive redirection ─────────────────────────────────────────


def _apply_disable_rdp_drives(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TS_POLICY, "fDisableCdm", 1)


def _remove_disable_rdp_drives(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TS_POLICY, "fDisableCdm")


def _detect_disable_rdp_drives() -> bool:
    return SESSION.read_dword(_TS_POLICY, "fDisableCdm") == 1


# ── 9. Disable RDP printer redirection ───────────────────────────────────────


def _apply_disable_rdp_printers(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TS_POLICY, "fDisableCpm", 1)


def _remove_disable_rdp_printers(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TS_POLICY, "fDisableCpm")


def _detect_disable_rdp_printers() -> bool:
    return SESSION.read_dword(_TS_POLICY, "fDisableCpm") == 1


# ── 10. Set RDP idle timeout (15 min) ────────────────────────────────────────


def _apply_rdp_idle_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TS_POLICY, "MaxIdleTime", 900000)


def _remove_rdp_idle_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TS_POLICY, "MaxIdleTime")


def _detect_rdp_idle_timeout() -> bool:
    return SESSION.read_dword(_TS_POLICY, "MaxIdleTime") == 900000


# ── 11. Disable Remote Assistance ────────────────────────────────────────────

_RA = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance"


def _apply_disable_remote_assist(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RA, "fAllowToGetHelp", 0)


def _remove_disable_remote_assist(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RA, "fAllowToGetHelp", 1)


def _detect_disable_remote_assist() -> bool:
    return SESSION.read_dword(_RA, "fAllowToGetHelp") == 0


# ── TWEAKS list ──────────────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="rdp-enable-remote-desktop",
        label="Enable Remote Desktop",
        category="Remote Desktop",
        apply_fn=_apply_enable_rdp,
        remove_fn=_remove_enable_rdp,
        detect_fn=_detect_enable_rdp,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_RDP],
        description="Allow incoming Remote Desktop connections. Default: disabled. Recommended: enable if needed.",
        tags=["rdp", "remote", "desktop", "connect"],
    ),
    TweakDef(
        id="rdp-require-nla",
        label="Require NLA for RDP",
        category="Remote Desktop",
        apply_fn=_apply_require_nla,
        remove_fn=_remove_require_nla,
        detect_fn=_detect_require_nla,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RDP_NLA],
        description="Require Network Level Authentication before RDP session. Default: enabled. Recommended: enabled for security.",
        tags=["rdp", "nla", "authentication", "security"],
    ),
    TweakDef(
        id="rdp-high-encryption",
        label="RDP High Encryption Level",
        category="Remote Desktop",
        apply_fn=_apply_rdp_high_encryption,
        remove_fn=_remove_rdp_high_encryption,
        detect_fn=_detect_rdp_high_encryption,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RDP_TCP],
        description="Set RDP minimum encryption to High (128-bit). Default: client-compatible. Recommended: high.",
        tags=["rdp", "encryption", "security"],
    ),
    TweakDef(
        id="rdp-enable-keepalive",
        label="Enable RDP Keep-Alive",
        category="Remote Desktop",
        apply_fn=_apply_rdp_keepalive,
        remove_fn=_remove_rdp_keepalive,
        detect_fn=_detect_rdp_keepalive,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RDP_TCP],
        description="Send keep-alive packets every minute to prevent RDP disconnects. Default: disabled.",
        tags=["rdp", "keepalive", "disconnect", "timeout"],
    ),
    TweakDef(
        id="rdp-bitmap-caching",
        label="Enable Persistent Bitmap Caching",
        category="Remote Desktop",
        apply_fn=_apply_rdp_bitmap_cache,
        remove_fn=_remove_rdp_bitmap_cache,
        detect_fn=_detect_rdp_bitmap_cache,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BITMAP],
        description="Cache bitmaps on disk for better RDP performance. Default: not set.",
        tags=["rdp", "bitmap", "cache", "performance"],
    ),
    TweakDef(
        id="rdp-change-port-3390",
        label="Change RDP Port to 3390",
        category="Remote Desktop",
        apply_fn=_apply_rdp_port_3390,
        remove_fn=_remove_rdp_port_3390,
        detect_fn=_detect_rdp_port_3390,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_RDP_TCP],
        description="Move RDP to port 3390 to reduce automated scanning. Default: 3389.",
        tags=["rdp", "port", "security", "scan"],
    ),
    TweakDef(
        id="rdp-disable-clipboard-redirect",
        label="Disable RDP Clipboard Redirection",
        category="Remote Desktop",
        apply_fn=_apply_disable_rdp_clipboard,
        remove_fn=_remove_disable_rdp_clipboard,
        detect_fn=_detect_disable_rdp_clipboard,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TS_POLICY],
        description="Block clipboard sharing between RDP host and client. DLP measure. Default: allowed.",
        tags=["rdp", "clipboard", "dlp", "security"],
    ),
    TweakDef(
        id="rdp-disable-drive-redirect",
        label="Disable RDP Drive Redirection",
        category="Remote Desktop",
        apply_fn=_apply_disable_rdp_drives,
        remove_fn=_remove_disable_rdp_drives,
        detect_fn=_detect_disable_rdp_drives,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TS_POLICY],
        description="Block drive mapping in RDP sessions. Security measure. Default: allowed.",
        tags=["rdp", "drive", "redirect", "security"],
    ),
    TweakDef(
        id="rdp-disable-printer-redirect",
        label="Disable RDP Printer Redirection",
        category="Remote Desktop",
        apply_fn=_apply_disable_rdp_printers,
        remove_fn=_remove_disable_rdp_printers,
        detect_fn=_detect_disable_rdp_printers,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TS_POLICY],
        description="Block printer redirection in RDP sessions. Default: allowed.",
        tags=["rdp", "printer", "redirect"],
    ),
    TweakDef(
        id="rdp-idle-timeout-15m",
        label="Set RDP Idle Timeout (15 min)",
        category="Remote Desktop",
        apply_fn=_apply_rdp_idle_timeout,
        remove_fn=_remove_rdp_idle_timeout,
        detect_fn=_detect_rdp_idle_timeout,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TS_POLICY],
        description="Disconnect idle RDP sessions after 15 minutes. Default: no timeout.",
        tags=["rdp", "idle", "timeout", "security"],
    ),
    TweakDef(
        id="rdp-disable-remote-assistance",
        label="Disable Remote Assistance",
        category="Remote Desktop",
        apply_fn=_apply_disable_remote_assist,
        remove_fn=_remove_disable_remote_assist,
        detect_fn=_detect_disable_remote_assist,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_RA],
        description="Disable offering and requesting Remote Assistance. Default: enabled. Recommended: disabled if unused.",
        tags=["remote", "assistance", "help", "security"],
    ),
]


# -- 12. Disable Remote Desktop ──────────────────────────────────────────────


def _apply_disable_rdp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_RDP], "DisableRDP")
    SESSION.set_dword(_RDP, "fDenyTSConnections", 1)


def _remove_disable_rdp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP, "fDenyTSConnections", 0)


def _detect_disable_rdp() -> bool:
    return SESSION.read_dword(_RDP, "fDenyTSConnections") == 1


# -- 13. Disable RDP Session Shadow ──────────────────────────────────────────


def _apply_disable_shadow(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_TS_POLICY], "DisableRDPShadow")
    SESSION.set_dword(_TS_POLICY, "Shadow", 0)


def _remove_disable_shadow(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TS_POLICY, "Shadow", 1)


def _detect_disable_shadow() -> bool:
    return SESSION.read_dword(_TS_POLICY, "Shadow") == 0


TWEAKS += [
    TweakDef(
        id="rdp-disable-rdp",
        label="Disable Remote Desktop",
        category="Remote Desktop",
        apply_fn=_apply_disable_rdp,
        remove_fn=_remove_disable_rdp,
        detect_fn=_detect_disable_rdp,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_RDP],
        description="Disables Remote Desktop connections entirely. Default: Depends on edition. Recommended: Disabled if unused.",
        tags=["rdp", "remote", "disable", "security"],
    ),
    TweakDef(
        id="rdp-disable-shadow",
        label="Disable RDP Session Shadowing",
        category="Remote Desktop",
        apply_fn=_apply_disable_shadow,
        remove_fn=_remove_disable_shadow,
        detect_fn=_detect_disable_shadow,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TS_POLICY],
        description="Disables remote session shadowing/observation via RDP. Default: Allowed. Recommended: Disabled for privacy.",
        tags=["rdp", "shadow", "observation", "security"],
    ),
]


# -- Set RDP Security Layer to SSL ---------------------------------------------


def _apply_rdp_security_ssl(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RDP: set security layer to SSL/TLS")
    SESSION.backup([_RDP_TCP], "RDPSecuritySSL")
    SESSION.set_dword(_RDP_TCP, "SecurityLayer", 2)


def _remove_rdp_security_ssl(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RDP_TCP, "SecurityLayer", 0)


def _detect_rdp_security_ssl() -> bool:
    return SESSION.read_dword(_RDP_TCP, "SecurityLayer") == 2


# -- Disable RDP Printer Redirection (Policy) ----------------------------------


def _apply_rdp_disable_printer_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RDP: disable printer redirection via policy")
    SESSION.backup([_TS_POLICY, _RDP_TCP], "RDPPrinterPolicy")
    SESSION.set_dword(_TS_POLICY, "fDisableCpm", 1)
    SESSION.set_dword(_RDP_TCP, "fDisableCpm", 1)


def _remove_rdp_disable_printer_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TS_POLICY, "fDisableCpm")
    SESSION.set_dword(_RDP_TCP, "fDisableCpm", 0)


def _detect_rdp_disable_printer_policy() -> bool:
    return SESSION.read_dword(_TS_POLICY, "fDisableCpm") == 1


# -- Set RDP Session Timeout (30 min) ------------------------------------------


def _apply_rdp_session_timeout_30m(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("RDP: set session timeout to 30 minutes")
    SESSION.backup([_TS_POLICY], "RDPSessionTimeout30m")
    SESSION.set_dword(_TS_POLICY, "MaxDisconnectionTime", 1_800_000)
    SESSION.set_dword(_TS_POLICY, "MaxIdleTime", 1_800_000)


def _remove_rdp_session_timeout_30m(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TS_POLICY, "MaxDisconnectionTime")
    SESSION.delete_value(_TS_POLICY, "MaxIdleTime")


def _detect_rdp_session_timeout_30m() -> bool:
    return SESSION.read_dword(_TS_POLICY, "MaxDisconnectionTime") == 1_800_000


TWEAKS += [
    TweakDef(
        id="rdp-security-layer-ssl",
        label="Set RDP Security Layer to SSL/TLS",
        category="Remote Desktop",
        apply_fn=_apply_rdp_security_ssl,
        remove_fn=_remove_rdp_security_ssl,
        detect_fn=_detect_rdp_security_ssl,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RDP_TCP],
        description=(
            "Sets the RDP security layer to SSL/TLS (level 2) for encrypted connections. "
            "Prevents legacy RDP security negotiation. Default: Negotiate. Recommended: SSL."
        ),
        tags=["rdp", "ssl", "tls", "security"],
    ),
    TweakDef(
        id="rdp-disable-printer-policy",
        label="Disable RDP Printer Redirection (Policy + WinStation)",
        category="Remote Desktop",
        apply_fn=_apply_rdp_disable_printer_policy,
        remove_fn=_remove_rdp_disable_printer_policy,
        detect_fn=_detect_rdp_disable_printer_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TS_POLICY, _RDP_TCP],
        description=(
            "Disables printer redirection in RDP via both policy and WinStation config. "
            "Blocks client printers from mapping to RDP sessions. Default: allowed. Recommended: disabled."
        ),
        tags=["rdp", "printer", "redirect", "policy"],
    ),
    TweakDef(
        id="rdp-session-timeout-30m",
        label="Set RDP Session Timeout (30 min)",
        category="Remote Desktop",
        apply_fn=_apply_rdp_session_timeout_30m,
        remove_fn=_remove_rdp_session_timeout_30m,
        detect_fn=_detect_rdp_session_timeout_30m,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TS_POLICY],
        description=(
            "Disconnects idle/disconnected RDP sessions after 30 minutes. "
            "Frees resources and improves security. Default: no timeout. Recommended: 30 min."
        ),
        tags=["rdp", "session", "timeout", "security"],
    ),
]

# ── Extra RDP hardening / comfort ───────────────────────────────────────────

_RDP_DESKTOP = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"


def _apply_rdp_disable_wallpaper(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_RDP_DESKTOP], "RDPWallpaper")
    SESSION.set_dword(_RDP_DESKTOP, "fDisableWallpaper", 1)


def _remove_rdp_disable_wallpaper(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_RDP_DESKTOP, "fDisableWallpaper")


def _detect_rdp_disable_wallpaper() -> bool:
    return SESSION.read_dword(_RDP_DESKTOP, "fDisableWallpaper") == 1


def _apply_rdp_enable_font_smoothing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_RDP_DESKTOP], "RDPFontSmoothing")
    SESSION.set_dword(_RDP_DESKTOP, "AllowFontAntiAlias", 1)


def _remove_rdp_enable_font_smoothing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_RDP_DESKTOP, "AllowFontAntiAlias")


def _detect_rdp_enable_font_smoothing() -> bool:
    return SESSION.read_dword(_RDP_DESKTOP, "AllowFontAntiAlias") == 1


def _apply_rdp_disable_audio_record(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_TS_POLICY], "RDPAudioRecord")
    SESSION.set_dword(_TS_POLICY, "fDisableCcm", 1)


def _remove_rdp_disable_audio_record(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TS_POLICY, "fDisableCcm")


def _detect_rdp_disable_audio_record() -> bool:
    return SESSION.read_dword(_TS_POLICY, "fDisableCcm") == 1


def _apply_rdp_enable_compression(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_RDP_TCP], "RDPCompression")
    SESSION.set_dword(_RDP_TCP, "CompressedData", 1)


def _remove_rdp_enable_compression(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_RDP_TCP, "CompressedData")


def _detect_rdp_enable_compression() -> bool:
    return SESSION.read_dword(_RDP_TCP, "CompressedData") == 1


def _apply_rdp_single_session(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_RDP], "RDPSingleSession")
    SESSION.set_dword(_RDP, "fSingleSessionPerUser", 1)


def _remove_rdp_single_session(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_RDP, "fSingleSessionPerUser")


def _detect_rdp_single_session() -> bool:
    return SESSION.read_dword(_RDP, "fSingleSessionPerUser") == 1


TWEAKS += [
    TweakDef(
        id="rdp-disable-wallpaper",
        label="Disable Desktop Wallpaper in RDP Sessions",
        category="Remote Desktop",
        apply_fn=_apply_rdp_disable_wallpaper,
        remove_fn=_remove_rdp_disable_wallpaper,
        detect_fn=_detect_rdp_disable_wallpaper,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RDP_DESKTOP],
        description=(
            "Disables desktop wallpaper rendering in RDP sessions to reduce bandwidth. "
            "Improves performance over slow connections. Default: Enabled. Recommended: Disabled."
        ),
        tags=["rdp", "wallpaper", "performance", "bandwidth"],
    ),
    TweakDef(
        id="rdp-enable-font-smoothing",
        label="Enable Font Anti-Aliasing in RDP Sessions",
        category="Remote Desktop",
        apply_fn=_apply_rdp_enable_font_smoothing,
        remove_fn=_remove_rdp_enable_font_smoothing,
        detect_fn=_detect_rdp_enable_font_smoothing,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RDP_DESKTOP],
        description=(
            "Allows ClearType / font anti-aliasing in Remote Desktop sessions. "
            "Improves text readability at cost of slight bandwidth increase. "
            "Default: Disabled. Recommended: Enabled for clarity."
        ),
        tags=["rdp", "font", "cleartype", "smoothing", "display"],
    ),
    TweakDef(
        id="rdp-disable-audio-record",
        label="Disable Audio Recording Redirection in RDP",
        category="Remote Desktop",
        apply_fn=_apply_rdp_disable_audio_record,
        remove_fn=_remove_rdp_disable_audio_record,
        detect_fn=_detect_rdp_disable_audio_record,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TS_POLICY],
        description=(
            "Disables audio capture/microphone redirection from the client to the RDP session. "
            "Reduces attack surface and bandwidth. Default: Enabled. Recommended: Disabled."
        ),
        tags=["rdp", "audio", "microphone", "record", "security"],
    ),
    TweakDef(
        id="rdp-enable-compression",
        label="Enable RDP Data Compression",
        category="Remote Desktop",
        apply_fn=_apply_rdp_enable_compression,
        remove_fn=_remove_rdp_enable_compression,
        detect_fn=_detect_rdp_enable_compression,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RDP_TCP],
        description=(
            "Enables compression of RDP session data to reduce bandwidth usage. "
            "Useful for connections over slower networks. Default: Disabled. Recommended: Enabled."
        ),
        tags=["rdp", "compression", "bandwidth", "performance"],
    ),
    TweakDef(
        id="rdp-single-session",
        label="Restrict to Single RDP Session Per User",
        category="Remote Desktop",
        apply_fn=_apply_rdp_single_session,
        remove_fn=_remove_rdp_single_session,
        detect_fn=_detect_rdp_single_session,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RDP],
        description=(
            "Limits each user to a single concurrent RDP session, reconnecting "
            "to an existing session rather than creating a new one. "
            "Default: Multiple sessions. Recommended: Single session."
        ),
        tags=["rdp", "session", "single", "reconnect"],
    ),
]
