"""Mozilla Firefox registry tweaks (policy-based).

Firefox respects HKLM policies via its enterprise policy engine.
These correspond to ``policies.json`` entries but set via registry.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_FF_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"
_FF_DOH = rf"{_FF_POLICY}\DNSOverHTTPS"
_FF_KEYS = [_FF_POLICY]


# ── Disable Firefox Telemetry ───────────────────────────────────────────────


def apply_disable_ff_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableFirefoxTelemetry")
    SESSION.backup(_FF_KEYS, "FirefoxTelemetry")
    SESSION.set_dword(_FF_POLICY, "DisableTelemetry", 1)
    SESSION.set_dword(_FF_POLICY, "DisableFirefoxStudies", 1)
    SESSION.set_dword(_FF_POLICY, "DisableDefaultBrowserAgent", 1)
    SESSION.log("Completed Add-DisableFirefoxTelemetry")


def remove_disable_ff_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableFirefoxTelemetry")
    SESSION.backup(_FF_KEYS, "FirefoxTelemetry_Remove")
    SESSION.delete_value(_FF_POLICY, "DisableTelemetry")
    SESSION.delete_value(_FF_POLICY, "DisableFirefoxStudies")
    SESSION.delete_value(_FF_POLICY, "DisableDefaultBrowserAgent")
    SESSION.log("Completed Remove-DisableFirefoxTelemetry")


def detect_disable_ff_telemetry() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableTelemetry") == 1


# ── Disable Firefox Pocket ──────────────────────────────────────────────────


def apply_disable_ff_pocket(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableFirefoxPocket")
    SESSION.backup(_FF_KEYS, "FirefoxPocket")
    SESSION.set_dword(_FF_POLICY, "DisablePocket", 1)
    SESSION.log("Completed Add-DisableFirefoxPocket")


def remove_disable_ff_pocket(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableFirefoxPocket")
    SESSION.backup(_FF_KEYS, "FirefoxPocket_Remove")
    SESSION.delete_value(_FF_POLICY, "DisablePocket")
    SESSION.log("Completed Remove-DisableFirefoxPocket")


def detect_disable_ff_pocket() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisablePocket") == 1


# ── Disable Firefox Auto-Update ─────────────────────────────────────────────


def apply_disable_ff_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableFirefoxUpdate")
    SESSION.backup(_FF_KEYS, "FirefoxUpdate")
    SESSION.set_dword(_FF_POLICY, "DisableAppUpdate", 1)
    SESSION.log("Completed Add-DisableFirefoxUpdate")


def remove_disable_ff_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableFirefoxUpdate")
    SESSION.backup(_FF_KEYS, "FirefoxUpdate_Remove")
    SESSION.delete_value(_FF_POLICY, "DisableAppUpdate")
    SESSION.log("Completed Remove-DisableFirefoxUpdate")


def detect_disable_ff_update() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableAppUpdate") == 1


# ── Disable Firefox Crash Reporter ────────────────────────────────────────


def _apply_disable_ff_crash(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: disable crash reporter")
    SESSION.backup(_FF_KEYS, "FirefoxCrash")
    SESSION.set_dword(_FF_POLICY, "DisableCrashReporter", 1)


def _remove_disable_ff_crash(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DisableCrashReporter")


def _detect_disable_ff_crash() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableCrashReporter") == 1


# ── Disable Firefox Default Browser Check ─────────────────────────────────


def _apply_disable_ff_default_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: disable default browser check")
    SESSION.backup(_FF_KEYS, "FirefoxDefaultCheck")
    SESSION.set_dword(_FF_POLICY, "DontCheckDefaultBrowser", 1)


def _remove_disable_ff_default_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DontCheckDefaultBrowser")


def _detect_disable_ff_default_check() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DontCheckDefaultBrowser") == 1


# ── Disable Firefox Shield Studies ─────────────────────────────────────────


def _apply_disable_ff_studies(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: disable Shield studies")
    SESSION.backup(_FF_KEYS, "FirefoxStudies")
    SESSION.set_dword(_FF_POLICY, "DisableFirefoxStudies", 1)


def _remove_disable_ff_studies(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DisableFirefoxStudies")


def _detect_disable_ff_studies() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableFirefoxStudies") == 1


# ── Disable Firefox Feedback Prompts ───────────────────────────────────────


def _apply_disable_ff_feedback(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: disable feedback prompts")
    SESSION.backup(_FF_KEYS, "FirefoxFeedback")
    SESSION.set_dword(_FF_POLICY, "DisableFeedbackCommands", 1)


def _remove_disable_ff_feedback(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DisableFeedbackCommands")


def _detect_disable_ff_feedback() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableFeedbackCommands") == 1


# ── Disable Firefox Captive Portal Detection ──────────────────────────────


def _apply_disable_ff_captive_portal(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: disable captive portal detection")
    SESSION.backup(_FF_KEYS, "FirefoxCaptivePortal")
    SESSION.set_dword(_FF_POLICY, "CaptivePortal", 0)


def _remove_disable_ff_captive_portal(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "CaptivePortal")


def _detect_disable_ff_captive_portal() -> bool:
    return SESSION.read_dword(_FF_POLICY, "CaptivePortal") == 0


# ── Enable DNS-over-HTTPS ──────────────────────────────────────────────────


def _apply_ff_dns_over_https(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: enable DNS-over-HTTPS")
    SESSION.backup([_FF_DOH], "FirefoxDNSOverHTTPS")
    SESSION.set_dword(_FF_DOH, "Enabled", 1)
    SESSION.set_string(_FF_DOH, "ProviderURL", "https://mozilla.cloudflare-dns.com/dns-query")


def _remove_ff_dns_over_https(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_DOH, "Enabled")
    SESSION.delete_value(_FF_DOH, "ProviderURL")


def _detect_ff_dns_over_https() -> bool:
    return SESSION.read_dword(_FF_DOH, "Enabled") == 1


# ── Disable Extension Recommendations ─────────────────────────────────────


def _apply_disable_ff_ext_recommendations(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: disable extension recommendations")
    SESSION.backup(_FF_KEYS, "FirefoxExtRec")
    SESSION.set_dword(_FF_POLICY, "ExtensionRecommendations", 0)


def _remove_disable_ff_ext_recommendations(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "ExtensionRecommendations")


def _detect_disable_ff_ext_recommendations() -> bool:
    return SESSION.read_dword(_FF_POLICY, "ExtensionRecommendations") == 0


# ── Disable Password Reveal Button ────────────────────────────────────────


def _apply_disable_ff_password_reveal(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: disable password reveal button")
    SESSION.backup(_FF_KEYS, "FirefoxPasswordReveal")
    SESSION.set_dword(_FF_POLICY, "DisablePasswordReveal", 1)


def _remove_disable_ff_password_reveal(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DisablePasswordReveal")


def _detect_disable_ff_password_reveal() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisablePasswordReveal") == 1


# ── Disable Firefox Telemetry (Policy Only) ─────────────────────────────────


def _apply_ff_telemetry_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: disable telemetry via policy")
    SESSION.backup(_FF_KEYS, "FirefoxTelemetryPolicy")
    SESSION.set_dword(_FF_POLICY, "DisableTelemetry", 1)


def _remove_ff_telemetry_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DisableTelemetry")


def _detect_ff_telemetry_policy() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableTelemetry") == 1


# ── Disable Firefox Default Browser Check (Policy Only) ──────────────────────


def _apply_ff_default_check_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: disable default browser check via policy")
    SESSION.backup(_FF_KEYS, "FirefoxDefaultCheckPolicy")
    SESSION.set_dword(_FF_POLICY, "DontCheckDefaultBrowser", 1)


def _remove_ff_default_check_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DontCheckDefaultBrowser")


def _detect_ff_default_check_policy() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DontCheckDefaultBrowser") == 1


# ── Disable Firefox Form History ────────────────────────────────────────────


def _apply_ff_disable_form_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: disable form auto-fill history via policy")
    SESSION.backup(_FF_KEYS, "FirefoxFormHistory")
    SESSION.set_dword(_FF_POLICY, "DisableFormHistory", 1)


def _remove_ff_disable_form_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DisableFormHistory")


def _detect_ff_disable_form_history() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableFormHistory") == 1


# ── Disable Firefox Profile Import ──────────────────────────────────────────


def _apply_ff_disable_profile_import(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Firefox: disable profile import wizard via policy")
    SESSION.backup(_FF_KEYS, "FirefoxProfileImport")
    SESSION.set_dword(_FF_POLICY, "DisableProfileImport", 1)


def _remove_ff_disable_profile_import(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DisableProfileImport")


def _detect_ff_disable_profile_import() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableProfileImport") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-firefox-telemetry",
        label="Disable Firefox Telemetry & Studies",
        category="Firefox",
        apply_fn=apply_disable_ff_telemetry,
        remove_fn=remove_disable_ff_telemetry,
        detect_fn=detect_disable_ff_telemetry,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_FF_KEYS,
        description=(
            "Disables Firefox telemetry, Shield studies, and the "
            "Default Browser Agent background task."
        ),
        tags=["firefox", "browser", "telemetry", "privacy"],
    ),
    TweakDef(
        id="disable-firefox-pocket",
        label="Disable Firefox Pocket",
        category="Firefox",
        apply_fn=apply_disable_ff_pocket,
        remove_fn=remove_disable_ff_pocket,
        detect_fn=detect_disable_ff_pocket,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description="Disables the Pocket integration in Firefox new tab page.",
        tags=["firefox", "browser", "pocket"],
    ),
    TweakDef(
        id="disable-firefox-update",
        label="Disable Firefox Auto-Update",
        category="Firefox",
        apply_fn=apply_disable_ff_update,
        remove_fn=remove_disable_ff_update,
        detect_fn=detect_disable_ff_update,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_FF_KEYS,
        description="Prevents Firefox from auto-updating. Use in controlled environments.",
        tags=["firefox", "browser", "update"],
    ),
    TweakDef(
        id="disable-firefox-crash-reporter",
        label="Disable Firefox Crash Reporter",
        category="Firefox",
        apply_fn=_apply_disable_ff_crash,
        remove_fn=_remove_disable_ff_crash,
        detect_fn=_detect_disable_ff_crash,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description="Disables the Firefox crash reporter dialog after crashes.",
        tags=["firefox", "browser", "crash"],
    ),
    TweakDef(
        id="disable-firefox-default-check",
        label="Disable Firefox Default Browser Check",
        category="Firefox",
        apply_fn=_apply_disable_ff_default_check,
        remove_fn=_remove_disable_ff_default_check,
        detect_fn=_detect_disable_ff_default_check,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description="Stops Firefox from asking to be the default browser on startup.",
        tags=["firefox", "browser", "default"],
    ),
    TweakDef(
        id="firefox-disable-studies",
        label="Disable Firefox Shield Studies",
        category="Firefox",
        apply_fn=_apply_disable_ff_studies,
        remove_fn=_remove_disable_ff_studies,
        detect_fn=_detect_disable_ff_studies,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description="Disables Firefox Shield studies that deploy experimental features.",
        tags=["firefox", "browser", "studies", "privacy"],
    ),
    TweakDef(
        id="firefox-disable-feedback",
        label="Disable Firefox Feedback Prompts",
        category="Firefox",
        apply_fn=_apply_disable_ff_feedback,
        remove_fn=_remove_disable_ff_feedback,
        detect_fn=_detect_disable_ff_feedback,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description="Disables feedback prompts and commands in Firefox.",
        tags=["firefox", "browser", "feedback"],
    ),
    TweakDef(
        id="firefox-disable-captive-portal",
        label="Disable Firefox Captive Portal Detection",
        category="Firefox",
        apply_fn=_apply_disable_ff_captive_portal,
        remove_fn=_remove_disable_ff_captive_portal,
        detect_fn=_detect_disable_ff_captive_portal,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description="Disables Firefox captive portal detection network requests.",
        tags=["firefox", "browser", "captive-portal", "network"],
    ),
    TweakDef(
        id="firefox-dns-over-https",
        label="Enable DNS-over-HTTPS",
        category="Firefox",
        apply_fn=_apply_ff_dns_over_https,
        remove_fn=_remove_ff_dns_over_https,
        detect_fn=_detect_ff_dns_over_https,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_FF_DOH],
        description=(
            "Enables DNS-over-HTTPS in Firefox using the Cloudflare resolver."
        ),
        tags=["firefox", "browser", "dns", "privacy", "security"],
    ),
    TweakDef(
        id="firefox-disable-extension-recommendations",
        label="Disable Extension Recommendations",
        category="Firefox",
        apply_fn=_apply_disable_ff_ext_recommendations,
        remove_fn=_remove_disable_ff_ext_recommendations,
        detect_fn=_detect_disable_ff_ext_recommendations,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description="Disables extension recommendations in Firefox.",
        tags=["firefox", "browser", "extensions", "recommendations"],
    ),
    TweakDef(
        id="firefox-disable-password-reveal",
        label="Disable Password Reveal Button",
        category="Firefox",
        apply_fn=_apply_disable_ff_password_reveal,
        remove_fn=_remove_disable_ff_password_reveal,
        detect_fn=_detect_disable_ff_password_reveal,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description="Disables the password reveal button in Firefox login fields.",
        tags=["firefox", "browser", "password", "security"],
    ),
    TweakDef(
        id="firefox-disable-telemetry",
        label="Disable Firefox Telemetry",
        category="Firefox",
        apply_fn=_apply_ff_telemetry_policy,
        remove_fn=_remove_ff_telemetry_policy,
        detect_fn=_detect_ff_telemetry_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description=(
            "Disables Firefox telemetry data collection via enterprise policy. "
            "Reduces background network traffic. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["firefox", "telemetry", "privacy", "performance"],
    ),
    TweakDef(
        id="firefox-disable-default-check",
        label="Disable Firefox Default Browser Check",
        category="Firefox",
        apply_fn=_apply_ff_default_check_policy,
        remove_fn=_remove_ff_default_check_policy,
        detect_fn=_detect_ff_default_check_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description=(
            "Prevents Firefox from checking if it's the default browser on startup. "
            "Removes the nag dialog. "
            "Default: Check enabled. Recommended: Disabled."
        ),
        tags=["firefox", "default-browser", "nag"],
    ),
    TweakDef(
        id="firefox-disable-form-history",
        label="Disable Firefox Form History",
        category="Firefox",
        apply_fn=_apply_ff_disable_form_history,
        remove_fn=_remove_ff_disable_form_history,
        detect_fn=_detect_ff_disable_form_history,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description=(
            "Disables Firefox form auto-fill history via enterprise policy. "
            "Prevents saving of form data and search history. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["firefox", "form", "history", "privacy"],
    ),
    TweakDef(
        id="firefox-disable-profile-import",
        label="Disable Firefox Profile Import",
        category="Firefox",
        apply_fn=_apply_ff_disable_profile_import,
        remove_fn=_remove_ff_disable_profile_import,
        detect_fn=_detect_ff_disable_profile_import,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description=(
            "Disables the profile import wizard in Firefox. Prevents "
            "importing data from other browsers. "
            "Default: Enabled. Recommended: Disabled for managed environments."
        ),
        tags=["firefox", "import", "profile", "managed"],
    ),
]


# ── Disable Firefox Pocket ───────────────────────────────────────────────────


def _apply_ff_pocket_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Firefox Pocket integration")
    SESSION.backup(_FF_KEYS, "FirefoxPocket")
    SESSION.set_dword(_FF_POLICY, "DisablePocket", 1)


def _remove_ff_pocket_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DisablePocket")


def _detect_ff_pocket_off() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisablePocket") == 1


# ── Disable Firefox Auto-Update ──────────────────────────────────────────────


def _apply_ff_auto_update_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Firefox auto-update via policy")
    SESSION.backup(_FF_KEYS, "FirefoxAutoUpdate")
    SESSION.set_dword(_FF_POLICY, "DisableAppUpdate", 1)


def _remove_ff_auto_update_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DisableAppUpdate")


def _detect_ff_auto_update_off() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableAppUpdate") == 1


TWEAKS += [
    TweakDef(
        id="firefox-disable-pocket",
        label="Disable Firefox Pocket",
        category="Firefox",
        apply_fn=_apply_ff_pocket_off,
        remove_fn=_remove_ff_pocket_off,
        detect_fn=_detect_ff_pocket_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_FF_KEYS,
        description=(
            "Disables Firefox Pocket integration via enterprise policy. "
            "Removes the Pocket button and save-to-Pocket feature. "
            "Default: Enabled. Recommended: Disabled if not used."
        ),
        tags=["firefox", "pocket", "policy", "feature"],
    ),
    TweakDef(
        id="firefox-disable-auto-update",
        label="Disable Firefox Auto-Update",
        category="Firefox",
        apply_fn=_apply_ff_auto_update_off,
        remove_fn=_remove_ff_auto_update_off,
        detect_fn=_detect_ff_auto_update_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_FF_KEYS,
        description=(
            "Disables Firefox automatic updates via enterprise policy. "
            "Allows manual update control for managed environments. "
            "Default: Auto-update. Recommended: Disabled for managed setups."
        ),
        tags=["firefox", "update", "auto-update", "policy", "managed"],
    ),
]


# ── Disable Firefox Password Auto-Save ───────────────────────────────────────


def _apply_ff_password_autosave_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Firefox password auto-save via policy")
    SESSION.backup(_FF_KEYS, "FirefoxPasswordAutoSave")
    SESSION.set_dword(_FF_POLICY, "OfferToSaveLogins", 0)


def _remove_ff_password_autosave_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "OfferToSaveLogins")


def _detect_ff_password_autosave_off() -> bool:
    return SESSION.read_dword(_FF_POLICY, "OfferToSaveLogins") == 0


# ── Disable Firefox Screenshots ──────────────────────────────────────────────


def _apply_ff_screenshots_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Firefox Screenshots via policy")
    SESSION.backup(_FF_KEYS, "FirefoxScreenshots")
    SESSION.set_dword(_FF_POLICY, "DisableFirefoxScreenshots", 1)


def _remove_ff_screenshots_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DisableFirefoxScreenshots")


def _detect_ff_screenshots_off() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableFirefoxScreenshots") == 1


# ── Disable Firefox Safe Mode ────────────────────────────────────────────────


def _apply_ff_safe_mode_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Firefox safe mode access via policy")
    SESSION.backup(_FF_KEYS, "FirefoxSafeMode")
    SESSION.set_dword(_FF_POLICY, "DisableSafeMode", 1)


def _remove_ff_safe_mode_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FF_POLICY, "DisableSafeMode")


def _detect_ff_safe_mode_off() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableSafeMode") == 1


TWEAKS += [
    TweakDef(
        id="ff-disable-password-autosave",
        label="Disable Firefox Password Auto-Save",
        category="Firefox",
        apply_fn=_apply_ff_password_autosave_off,
        remove_fn=_remove_ff_password_autosave_off,
        detect_fn=_detect_ff_password_autosave_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_FF_KEYS,
        description=(
            "Disables the offer-to-save-logins prompt in Firefox via enterprise "
            "policy. Use when an external password manager is preferred. "
            "Default: Enabled. Recommended: Disabled with external manager."
        ),
        tags=["firefox", "passwords", "autosave", "policy", "security"],
    ),
    TweakDef(
        id="ff-disable-screenshots",
        label="Disable Firefox Screenshots",
        category="Firefox",
        apply_fn=_apply_ff_screenshots_off,
        remove_fn=_remove_ff_screenshots_off,
        detect_fn=_detect_ff_screenshots_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_FF_KEYS,
        description=(
            "Disables the built-in Firefox Screenshots feature via enterprise "
            "policy. Removes the screenshot button from the toolbar. "
            "Default: Enabled. Recommended: Disabled in managed environments."
        ),
        tags=["firefox", "screenshots", "policy", "feature"],
    ),
    TweakDef(
        id="ff-disable-safe-mode",
        label="Disable Firefox Safe Mode",
        category="Firefox",
        apply_fn=_apply_ff_safe_mode_off,
        remove_fn=_remove_ff_safe_mode_off,
        detect_fn=_detect_ff_safe_mode_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_FF_KEYS,
        description=(
            "Disables access to Firefox Safe Mode via enterprise policy. "
            "Prevents users from bypassing managed extensions and settings. "
            "Default: Enabled. Recommended: Disabled for locked-down deployments."
        ),
        tags=["firefox", "safe-mode", "policy", "managed", "security"],
    ),
]
