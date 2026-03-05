"""Google Chrome registry tweaks (policy-based)."""

from __future__ import annotations

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


# ── Disable Chrome Browser Sign-In Prompt (standalone) ─────────────────────


def apply_chrome_disable_signin(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable browser sign-in prompt")
    SESSION.backup([_CHROME_POLICY], "ChromeDisableSignin")
    SESSION.set_dword(_CHROME_POLICY, "BrowserSignin", 0)


def remove_chrome_disable_signin(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "BrowserSignin")


def detect_chrome_disable_signin() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "BrowserSignin") == 0


# ── Disable Chrome Sync ────────────────────────────────────────────────────


def apply_chrome_disable_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable sync")
    SESSION.backup([_CHROME_POLICY], "ChromeDisableSync")
    SESSION.set_dword(_CHROME_POLICY, "SyncDisabled", 1)


def remove_chrome_disable_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "SyncDisabled")


def detect_chrome_disable_sync() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "SyncDisabled") == 1


# ── Disable Chrome Cloud Spell Check ──────────────────────────────────────


def apply_chrome_disable_spell_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable cloud spell check service")
    SESSION.backup([_CHROME_POLICY], "ChromeDisableSpellCheck")
    SESSION.set_dword(_CHROME_POLICY, "SpellCheckServiceEnabled", 0)


def remove_chrome_disable_spell_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "SpellCheckServiceEnabled")


def detect_chrome_disable_spell_check() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "SpellCheckServiceEnabled") == 0


# ── Block Third-Party Cookies ──────────────────────────────────────────────


def apply_chrome_block_third_party_cookies(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: block third-party cookies")
    SESSION.backup([_CHROME_POLICY], "ChromeBlockThirdPartyCookies")
    SESSION.set_dword(_CHROME_POLICY, "BlockThirdPartyCookies", 1)


def remove_chrome_block_third_party_cookies(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "BlockThirdPartyCookies")


def detect_chrome_block_third_party_cookies() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "BlockThirdPartyCookies") == 1


# ── Disable Chrome Password Autofill ──────────────────────────────────────


def apply_chrome_disable_autofill_passwords(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable password manager / autofill")
    SESSION.backup([_CHROME_POLICY], "ChromeDisableAutofillPasswords")
    SESSION.set_dword(_CHROME_POLICY, "PasswordManagerEnabled", 0)


def remove_chrome_disable_autofill_passwords(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "PasswordManagerEnabled")


def detect_chrome_disable_autofill_passwords() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "PasswordManagerEnabled") == 0


# ── Disable Chrome Software Reporter ───────────────────────────────────────


def _apply_chrome_reporter(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable Software Reporter Tool")
    SESSION.backup([_CHROME_POLICY], "ChromeReporter")
    SESSION.set_dword(_CHROME_POLICY, "ChromeCleanupEnabled", 0)
    SESSION.set_dword(_CHROME_POLICY, "ChromeCleanupReportingEnabled", 0)


def _remove_chrome_reporter(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "ChromeCleanupEnabled")
    SESSION.delete_value(_CHROME_POLICY, "ChromeCleanupReportingEnabled")


def _detect_chrome_reporter() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "ChromeCleanupEnabled") == 0


# ── Disable Chrome Background Apps (Policy) ───────────────────────────────


def _apply_chrome_background(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable background apps")
    SESSION.backup([_CHROME_POLICY], "ChromeBackground")
    SESSION.set_dword(_CHROME_POLICY, "BackgroundModeEnabled", 0)


def _remove_chrome_background(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "BackgroundModeEnabled")


def _detect_chrome_background() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "BackgroundModeEnabled") == 0


# ── Disable Chrome Metrics Reporting ─────────────────────────────────────────


def _apply_chrome_disable_metrics(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable metrics reporting via policy")
    SESSION.backup([_CHROME_POLICY], "ChromeMetrics")
    SESSION.set_dword(_CHROME_POLICY, "MetricsReportingEnabled", 0)


def _remove_chrome_disable_metrics(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "MetricsReportingEnabled")


def _detect_chrome_disable_metrics() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "MetricsReportingEnabled") == 0


# ── Disable Chrome Default Browser Check ─────────────────────────────────────


def _apply_chrome_disable_default_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable default browser check prompt")
    SESSION.backup([_CHROME_POLICY], "ChromeDefaultBrowserCheck")
    SESSION.set_dword(_CHROME_POLICY, "DefaultBrowserSettingEnabled", 0)


def _remove_chrome_disable_default_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "DefaultBrowserSettingEnabled")


def _detect_chrome_disable_default_check() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "DefaultBrowserSettingEnabled") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
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
    TweakDef(
        id="chrome-disable-signin",
        label="Disable Chrome Browser Sign-In Prompt",
        category="Chrome",
        apply_fn=apply_chrome_disable_signin,
        remove_fn=remove_chrome_disable_signin,
        detect_fn=detect_chrome_disable_signin,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CHROME_POLICY],
        description="Disables the Chrome browser sign-in prompt via policy.",
        tags=["chrome", "browser", "signin", "privacy"],
    ),
    TweakDef(
        id="chrome-disable-sync",
        label="Disable Chrome Sync",
        category="Chrome",
        apply_fn=apply_chrome_disable_sync,
        remove_fn=remove_chrome_disable_sync,
        detect_fn=detect_chrome_disable_sync,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CHROME_POLICY],
        description="Disables Chrome profile sync via policy.",
        tags=["chrome", "browser", "sync", "privacy"],
    ),
    TweakDef(
        id="chrome-disable-spell-check-service",
        label="Disable Chrome Cloud Spell Check",
        category="Chrome",
        apply_fn=apply_chrome_disable_spell_check,
        remove_fn=remove_chrome_disable_spell_check,
        detect_fn=detect_chrome_disable_spell_check,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Disables the cloud-based spell-check service in Chrome, "
            "keeping only the local spell checker."
        ),
        tags=["chrome", "browser", "spellcheck", "privacy"],
    ),
    TweakDef(
        id="chrome-block-third-party-cookies",
        label="Block Third-Party Cookies",
        category="Chrome",
        apply_fn=apply_chrome_block_third_party_cookies,
        remove_fn=remove_chrome_block_third_party_cookies,
        detect_fn=detect_chrome_block_third_party_cookies,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CHROME_POLICY],
        description="Blocks third-party cookies in Chrome via policy.",
        tags=["chrome", "browser", "cookies", "privacy"],
    ),
    TweakDef(
        id="chrome-disable-autofill-passwords",
        label="Disable Chrome Password Autofill",
        category="Chrome",
        apply_fn=apply_chrome_disable_autofill_passwords,
        remove_fn=remove_chrome_disable_autofill_passwords,
        detect_fn=detect_chrome_disable_autofill_passwords,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Disables the built-in Chrome password manager and autofill "
            "for passwords via policy."
        ),
        tags=["chrome", "browser", "passwords", "autofill", "security"],
    ),
    TweakDef(
        id="chrome-disable-reporter",
        label="Disable Chrome Software Reporter",
        category="Chrome",
        apply_fn=_apply_chrome_reporter,
        remove_fn=_remove_chrome_reporter,
        detect_fn=_detect_chrome_reporter,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Disables Chrome Software Reporter Tool and cleanup reporting. "
            "Prevents high CPU usage from background scanning. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["chrome", "reporter", "cleanup", "performance"],
    ),
    TweakDef(
        id="chrome-disable-background",
        label="Disable Chrome Background Apps",
        category="Chrome",
        apply_fn=_apply_chrome_background,
        remove_fn=_remove_chrome_background,
        detect_fn=_detect_chrome_background,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Prevents Chrome from running in the background after closing. "
            "Frees memory and CPU. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["chrome", "background", "performance", "memory"],
    ),
    TweakDef(
        id="chrome-disable-metrics-reporting",
        label="Disable Chrome Metrics Reporting",
        category="Chrome",
        apply_fn=_apply_chrome_disable_metrics,
        remove_fn=_remove_chrome_disable_metrics,
        detect_fn=_detect_chrome_disable_metrics,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Disables Chrome metrics and usage reporting via enterprise "
            "policy. Prevents Chrome from sending usage statistics. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["chrome", "metrics", "telemetry", "privacy"],
    ),
    TweakDef(
        id="chrome-disable-default-browser-check",
        label="Disable Chrome Default Browser Check",
        category="Chrome",
        apply_fn=_apply_chrome_disable_default_check,
        remove_fn=_remove_chrome_disable_default_check,
        detect_fn=_detect_chrome_disable_default_check,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Prevents Chrome from prompting to set itself as the default "
            "browser on startup. Default: Enabled. "
            "Recommended: Disabled for managed environments."
        ),
        tags=["chrome", "default-browser", "prompt", "ux"],
    ),
]


# ── Disable Chrome Hardware Acceleration (Policy) ────────────────────────────


def _apply_chrome_hw_accel_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Chrome hardware acceleration via policy")
    SESSION.backup([_CHROME_POLICY], "ChromeHWAccel")
    SESSION.set_dword(_CHROME_POLICY, "HardwareAccelerationModeEnabled", 0)


def _remove_chrome_hw_accel_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "HardwareAccelerationModeEnabled")


def _detect_chrome_hw_accel_off() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "HardwareAccelerationModeEnabled") == 0


# ── Block Third-Party Cookies (Policy) ───────────────────────────────────────


def _apply_chrome_3p_cookies_block(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Block Chrome third-party cookies via policy")
    SESSION.backup([_CHROME_POLICY], "Chrome3pCookies")
    SESSION.set_dword(_CHROME_POLICY, "BlockThirdPartyCookies", 1)


def _remove_chrome_3p_cookies_block(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "BlockThirdPartyCookies")


def _detect_chrome_3p_cookies_block() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "BlockThirdPartyCookies") == 1


TWEAKS += [
    TweakDef(
        id="chrome-disable-hardware-accel-policy",
        label="Disable Chrome Hardware Acceleration (Policy)",
        category="Chrome",
        apply_fn=_apply_chrome_hw_accel_off,
        remove_fn=_remove_chrome_hw_accel_off,
        detect_fn=_detect_chrome_hw_accel_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Disables Chrome hardware acceleration via enterprise policy. "
            "Useful for troubleshooting GPU-related rendering issues. "
            "Default: Enabled. Recommended: Disabled if GPU issues occur."
        ),
        tags=["chrome", "hardware", "acceleration", "gpu", "policy"],
    ),
    TweakDef(
        id="chrome-enforce-3p-cookie-block",
        label="Block Chrome Third-Party Cookies (Policy)",
        category="Chrome",
        apply_fn=_apply_chrome_3p_cookies_block,
        remove_fn=_remove_chrome_3p_cookies_block,
        detect_fn=_detect_chrome_3p_cookies_block,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Blocks third-party cookies in Chrome via enterprise policy. "
            "Enhances privacy by preventing cross-site tracking. "
            "Default: Allowed. Recommended: Blocked for privacy."
        ),
        tags=["chrome", "cookies", "third-party", "privacy", "policy"],
    ),
]


# ── Disable Chrome Translate ────────────────────────────────────────────────


def _apply_chrome_translate_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable translate feature")
    SESSION.backup([_CHROME_POLICY], "ChromeTranslate")
    SESSION.set_dword(_CHROME_POLICY, "TranslateEnabled", 0)


def _remove_chrome_translate_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "TranslateEnabled")


def _detect_chrome_translate_off() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "TranslateEnabled") == 0


# ── Disable Chrome Media Recommendations ───────────────────────────────────


def _apply_chrome_media_rec_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable media recommendations")
    SESSION.backup([_CHROME_POLICY], "ChromeMediaRec")
    SESSION.set_dword(_CHROME_POLICY, "MediaRecommendationsEnabled", 0)


def _remove_chrome_media_rec_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "MediaRecommendationsEnabled")


def _detect_chrome_media_rec_off() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "MediaRecommendationsEnabled") == 0


# ── Disable Chrome Password Leak Detection ─────────────────────────────────


def _apply_chrome_leak_detect_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Chrome: disable password leak detection")
    SESSION.backup([_CHROME_POLICY], "ChromeLeakDetect")
    SESSION.set_dword(_CHROME_POLICY, "PasswordLeakDetectionEnabled", 0)


def _remove_chrome_leak_detect_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHROME_POLICY, "PasswordLeakDetectionEnabled")


def _detect_chrome_leak_detect_off() -> bool:
    return SESSION.read_dword(_CHROME_POLICY, "PasswordLeakDetectionEnabled") == 0


TWEAKS += [
    TweakDef(
        id="chrome-disable-translate",
        label="Disable Chrome Translate",
        category="Chrome",
        apply_fn=_apply_chrome_translate_off,
        remove_fn=_remove_chrome_translate_off,
        detect_fn=_detect_chrome_translate_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Disables the Chrome built-in page translation feature "
            "via enterprise policy. Prevents translate bar prompts. "
            "Default: Enabled. Recommended: Disabled if not needed."
        ),
        tags=["chrome", "translate", "language", "policy"],
    ),
    TweakDef(
        id="chrome-disable-media-recommendations",
        label="Disable Chrome Media Recommendations",
        category="Chrome",
        apply_fn=_apply_chrome_media_rec_off,
        remove_fn=_remove_chrome_media_rec_off,
        detect_fn=_detect_chrome_media_rec_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Disables personalized media recommendations on the Chrome "
            "New Tab page. Reduces data collection and distractions. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["chrome", "media", "recommendations", "privacy", "policy"],
    ),
    TweakDef(
        id="chrome-disable-leak-detection",
        label="Disable Chrome Password Leak Detection",
        category="Chrome",
        apply_fn=_apply_chrome_leak_detect_off,
        remove_fn=_remove_chrome_leak_detect_off,
        detect_fn=_detect_chrome_leak_detect_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CHROME_POLICY],
        description=(
            "Disables Chrome password leak detection that checks saved "
            "passwords against known data breaches. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["chrome", "password", "leak", "detection", "policy"],
    ),
]
