"""Google Chrome registry tweaks (policy-based)."""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_CHROME_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"
_CHROME_UPDATE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Update"
_CHROME_KEYS = [_CHROME_POLICY, _CHROME_UPDATE]


# ── Disable Chrome Background Apps ──────────────────────────────────────────


def apply_disable_chrome_bg(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableChromeBackground")
    SESSION.backup([_CHROME_POLICY], "ChromeBackground")
    SESSION.set_dword(_CHROME_POLICY, "BackgroundModeEnabled", 0)
    SESSION.log("Completed Add-DisableChromeBackground")


def remove_disable_chrome_bg(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableChromeBackground")
    SESSION.backup([_CHROME_POLICY], "ChromeBackground_Remove")
    SESSION.delete_value(_CHROME_POLICY, "BackgroundModeEnabled")
    SESSION.log("Completed Remove-DisableChromeBackground")


def detect_disable_chrome_bg() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "BackgroundModeEnabled") == 0


# ── Disable Chrome Telemetry / Metrics ───────────────────────────────────────


def apply_disable_chrome_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableChromeTelemetry")
    SESSION.backup([_CHROME_POLICY], "ChromeTelemetry")
    SESSION.set_dword(_CHROME_POLICY, "MetricsReportingEnabled", 0)
    SESSION.set_dword(_CHROME_POLICY, "SafeBrowsingExtendedReportingEnabled", 0)
    SESSION.set_dword(_CHROME_POLICY, "UrlKeyedAnonymizedDataCollectionEnabled", 0)
    SESSION.set_dword(_CHROME_POLICY, "SpellCheckServiceEnabled", 0)
    SESSION.set_dword(_CHROME_POLICY, "TranslateEnabled", 0)
    SESSION.log("Completed Add-DisableChromeTelemetry")


def remove_disable_chrome_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableChromeTelemetry")
    SESSION.backup([_CHROME_POLICY], "ChromeTelemetry_Remove")
    for val in (
        "MetricsReportingEnabled",
        "SafeBrowsingExtendedReportingEnabled",
        "UrlKeyedAnonymizedDataCollectionEnabled",
        "SpellCheckServiceEnabled",
        "TranslateEnabled",
    ):
        SESSION.delete_value(_CHROME_POLICY, val)
    SESSION.log("Completed Remove-DisableChromeTelemetry")


def detect_disable_chrome_telemetry() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "MetricsReportingEnabled") == 0


# ── Disable Chrome Auto-Update ──────────────────────────────────────────────


def apply_disable_chrome_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableChromeUpdate")
    SESSION.backup([_CHROME_UPDATE], "ChromeUpdate")
    # AutoUpdateCheckPeriodMinutes: 0=disabled
    SESSION.set_dword(_CHROME_UPDATE, "AutoUpdateCheckPeriodMinutes", 0)
    SESSION.set_dword(_CHROME_UPDATE, "UpdateDefault", 0)
    SESSION.log("Completed Add-DisableChromeUpdate")


def remove_disable_chrome_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableChromeUpdate")
    SESSION.backup([_CHROME_UPDATE], "ChromeUpdate_Remove")
    SESSION.delete_value(_CHROME_UPDATE, "AutoUpdateCheckPeriodMinutes")
    SESSION.delete_value(_CHROME_UPDATE, "UpdateDefault")
    SESSION.log("Completed Remove-DisableChromeUpdate")


def detect_disable_chrome_update() -> bool:
    return SESSION.read_dword(_CHROME_UPDATE, "AutoUpdateCheckPeriodMinutes") == 0


# ── Disable Chrome Hardware Acceleration ─────────────────────────────────────


def apply_disable_chrome_hwaccel(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableChromeHWAccel")
    SESSION.backup([_CHROME_POLICY], "ChromeHWAccel")
    SESSION.set_dword(_CHROME_POLICY, "HardwareAccelerationModeEnabled", 0)
    SESSION.log("Completed Add-DisableChromeHWAccel")


def remove_disable_chrome_hwaccel(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableChromeHWAccel")
    SESSION.backup([_CHROME_POLICY], "ChromeHWAccel_Remove")
    SESSION.delete_value(_CHROME_POLICY, "HardwareAccelerationModeEnabled")
    SESSION.log("Completed Remove-DisableChromeHWAccel")


def detect_disable_chrome_hwaccel() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "HardwareAccelerationModeEnabled") == 0


# ── Disable Chrome Browser Sign-In ────────────────────────────────────────


def apply_disable_chrome_signin(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable browser sign-in")
    SESSION.backup([_CHROME_POLICY], "ChromeSignIn")
    SESSION.set_dword(_CHROME_POLICY, "BrowserSignin", 0)  # 0=disabled
    SESSION.set_dword(_CHROME_POLICY, "SyncDisabled", 1)


def remove_disable_chrome_signin(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "BrowserSignin")
    SESSION.delete_value(_CHROME_POLICY, "SyncDisabled")


def detect_disable_chrome_signin() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "BrowserSignin") == 0


# ── Enable Chrome Secure DNS (DoH) ────────────────────────────────────────


def apply_chrome_secure_dns(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: enable DNS-over-HTTPS (Secure DNS)")
    SESSION.backup([_CHROME_POLICY], "ChromeSecureDNS")
    SESSION.set_string(_CHROME_POLICY, "DnsOverHttpsMode", "automatic")


def remove_chrome_secure_dns(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "DnsOverHttpsMode")


def detect_chrome_secure_dns() -> bool:
    return SESSION.read_string(_CHROME_POLICY, "DnsOverHttpsMode") == "automatic"


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-chrome-bg",
        label="Disable Chrome Background Apps",
        category="Chrome",
        apply_fn=apply_disable_chrome_bg,
        remove_fn=remove_disable_chrome_bg,
        detect_fn=detect_disable_chrome_bg,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Prevents Chrome from running in the background after the "
            "browser window is closed, saving memory and CPU."
        ),
        tags=["chrome", "browser", "background"],
    ),
    TweakDef(
        id="disable-chrome-telemetry",
        label="Disable Chrome Telemetry",
        category="Chrome",
        apply_fn=apply_disable_chrome_telemetry,
        remove_fn=remove_disable_chrome_telemetry,
        detect_fn=detect_disable_chrome_telemetry,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Disables Chrome metrics, spell-check cloud, translate, "
            "and extended safe-browsing reporting."
        ),
        tags=["chrome", "browser", "telemetry", "privacy"],
    ),
    TweakDef(
        id="disable-chrome-update",
        label="Disable Chrome Auto-Update",
        category="Chrome",
        apply_fn=apply_disable_chrome_update,
        remove_fn=remove_disable_chrome_update,
        detect_fn=detect_disable_chrome_update,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CHROME_UPDATE],
        description="Prevents Chrome from checking for or installing updates.",
        tags=["chrome", "browser", "update"],
    ),
    TweakDef(
        id="disable-chrome-hwaccel",
        label="Disable Chrome Hardware Acceleration",
        category="Chrome",
        apply_fn=apply_disable_chrome_hwaccel,
        remove_fn=remove_disable_chrome_hwaccel,
        detect_fn=detect_disable_chrome_hwaccel,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Forces Chrome to use software rendering instead of GPU, "
            "useful for troubleshooting display issues."
        ),
        tags=["chrome", "browser", "gpu"],
    ),
    TweakDef(
        id="disable-chrome-signin",
        label="Disable Chrome Sign-In & Sync",
        category="Chrome",
        apply_fn=apply_disable_chrome_signin,
        remove_fn=remove_disable_chrome_signin,
        detect_fn=detect_disable_chrome_signin,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CHROME_POLICY],
        description="Disables Chrome browser sign-in and sync via policy.",
        tags=["chrome", "browser", "privacy", "sync"],
    ),
    TweakDef(
        id="chrome-secure-dns",
        label="Enable Chrome Secure DNS (DoH)",
        category="Chrome",
        apply_fn=apply_chrome_secure_dns,
        remove_fn=remove_chrome_secure_dns,
        detect_fn=detect_chrome_secure_dns,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CHROME_POLICY],
        description="Enables DNS-over-HTTPS (automatic mode) in Chrome.",
        tags=["chrome", "browser", "dns", "security"],
    ),
]
