"""Java runtime registry tweaks.

Covers: auto-update, security prompts, web plugin.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_JAVA_UPDATE = r"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"
_JAVA_UPDATE32 = r"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy"
_JAVA_DEPLOY = r"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Plug-in"
_JAVA_WEB = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"
_JAVA_USAGE = r"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment"
_JAVA_DPI = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"
_JAVA_UPDATE_CHK = r"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"
_JAVA_JRE = r"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment"


# ── Disable Java Auto-Update ────────────────────────────────────────────────


def _apply_disable_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable auto-update")
    SESSION.backup([_JAVA_UPDATE, _JAVA_UPDATE32], "JavaUpdate")
    SESSION.set_dword(_JAVA_UPDATE, "EnableJavaUpdate", 0)
    SESSION.set_dword(_JAVA_UPDATE, "NotifyDownload", 0)
    SESSION.set_dword(_JAVA_UPDATE32, "EnableJavaUpdate", 0)
    SESSION.set_dword(_JAVA_UPDATE32, "NotifyDownload", 0)


def _remove_disable_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_JAVA_UPDATE, "EnableJavaUpdate", 1)
    SESSION.set_dword(_JAVA_UPDATE32, "EnableJavaUpdate", 1)


def _detect_disable_update() -> bool:
    return SESSION.read_dword(_JAVA_UPDATE, "EnableJavaUpdate") == 0


# ── Disable Java Web Plugin ─────────────────────────────────────────────────


def _apply_disable_web(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable browser web plugin")
    SESSION.backup([_JAVA_WEB], "JavaWebPlugin")
    SESSION.set_dword(_JAVA_WEB, "deployment.webjava.enabled", 0)


def _remove_disable_web(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_JAVA_WEB, "deployment.webjava.enabled", 1)


def _detect_disable_web() -> bool:
    return SESSION.read_dword(_JAVA_WEB, "deployment.webjava.enabled") == 0


# ── Disable Java Usage Tracking ───────────────────────────────────────────


def _apply_disable_tracking(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable usage tracking")
    SESSION.backup([_JAVA_USAGE], "JavaTracking")
    SESSION.set_string(_JAVA_USAGE, "usagetracker.track.last.timestamp", "0")
    SESSION.set_dword(_JAVA_WEB, "deployment.javaws.shortcut", 0)


def _remove_disable_tracking(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_JAVA_USAGE, "usagetracker.track.last.timestamp")
    SESSION.delete_value(_JAVA_WEB, "deployment.javaws.shortcut")


def _detect_disable_tracking() -> bool:
    return SESSION.read_string(_JAVA_USAGE, "usagetracker.track.last.timestamp") == "0"


# ── Java High DPI Scaling ─────────────────────────────────────────────────

_JAVA_EXE_PATH = r"C:\Program Files\Java\jre-1.8\bin\javaw.exe"


def _apply_java_dpi(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: enable System DPI-aware scaling")
    SESSION.backup([_JAVA_DPI], "JavaDPI")
    SESSION.set_string(_JAVA_DPI, _JAVA_EXE_PATH, "~ HIGHDPIAWARE")


def _remove_java_dpi(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_JAVA_DPI, _JAVA_EXE_PATH)


def _detect_java_dpi() -> bool:
    v = SESSION.read_string(_JAVA_DPI, _JAVA_EXE_PATH)
    return v is not None and "HIGHDPIAWARE" in v


# ── Disable Java Sponsor Offers ──────────────────────────────────────────────

_JAVA_SPONSOR = r"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"
_JAVA_SPONSOR32 = r"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy"


def _apply_disable_sponsor(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable sponsor offers during updates")
    SESSION.backup([_JAVA_SPONSOR, _JAVA_SPONSOR32], "JavaSponsor")
    SESSION.set_dword(_JAVA_SPONSOR, "EnableAutoUpdateCheck", 0)
    SESSION.set_dword(_JAVA_SPONSOR32, "EnableAutoUpdateCheck", 0)


def _remove_disable_sponsor(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_JAVA_SPONSOR, "EnableAutoUpdateCheck", 1)
    SESSION.set_dword(_JAVA_SPONSOR32, "EnableAutoUpdateCheck", 1)


def _detect_disable_sponsor() -> bool:
    return SESSION.read_dword(_JAVA_SPONSOR, "EnableAutoUpdateCheck") == 0


# ── Java Security Level → High ───────────────────────────────────────────────

_JAVA_SEC = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"


def _apply_java_security_high(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: set security level to HIGH")
    SESSION.backup([_JAVA_SEC], "JavaSecLevel")
    SESSION.set_string(_JAVA_SEC, "deployment.security.level", "VERY_HIGH")


def _remove_java_security_high(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_JAVA_SEC, "deployment.security.level")


def _detect_java_security_high() -> bool:
    return SESSION.read_string(_JAVA_SEC, "deployment.security.level") == "VERY_HIGH"


# ── Disable Java Error Reporting ──────────────────────────────────────────────

_JAVA_ERR = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"


def _apply_disable_error_reporting(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable error reporting")
    SESSION.backup([_JAVA_ERR], "JavaErrorReporting")
    SESSION.set_string(_JAVA_ERR, "deployment.javaws.showExceptions", "false")


def _remove_disable_error_reporting(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_JAVA_ERR, "deployment.javaws.showExceptions")


def _detect_disable_error_reporting() -> bool:
    return SESSION.read_string(_JAVA_ERR, "deployment.javaws.showExceptions") == "false"


# ── Disable Java Tip of the Day ──────────────────────────────────────────────


def _apply_disable_java_tip(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable Tip of the Day popups")
    SESSION.backup([_JAVA_WEB], "JavaTipOfDay")
    SESSION.set_string(_JAVA_WEB, "deployment.javaws.tip.day", "false")


def _remove_disable_java_tip(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_JAVA_WEB, "deployment.javaws.tip.day")


def _detect_disable_java_tip() -> bool:
    return SESSION.read_string(_JAVA_WEB, "deployment.javaws.tip.day") == "false"


# ── Disable Java Certificate Revocation Check ────────────────────────────────


def _apply_disable_cert_revoke(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable certificate revocation checks")
    SESSION.backup([_JAVA_WEB], "JavaCertRevoke")
    SESSION.set_string(_JAVA_WEB, "deployment.security.validation.ocsp", "false")


def _remove_disable_cert_revoke(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_JAVA_WEB, "deployment.security.validation.ocsp")


def _detect_disable_cert_revoke() -> bool:
    return SESSION.read_string(_JAVA_WEB, "deployment.security.validation.ocsp") == "false"


# ── Disable Java Auto-Update Check ──────────────────────────────────────────


def _apply_disable_update_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable automatic update check")
    SESSION.backup([_JAVA_UPDATE_CHK], "JavaUpdateCheck")
    SESSION.set_dword(_JAVA_UPDATE_CHK, "EnableAutoUpdateCheck", 0)


def _remove_disable_update_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_JAVA_UPDATE_CHK, "EnableAutoUpdateCheck", 1)


def _detect_disable_update_check() -> bool:
    return SESSION.read_dword(_JAVA_UPDATE_CHK, "EnableAutoUpdateCheck") == 0


# ── Java High Performance Graphics ──────────────────────────────────────────


def _apply_high_perf_graphics(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: enable hardware graphics acceleration")
    SESSION.backup([_JAVA_JRE], "JavaHWAccel")
    SESSION.set_dword(_JAVA_JRE, "JavaFXHardwareAcceleration", 1)


def _remove_high_perf_graphics(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_JAVA_JRE, "JavaFXHardwareAcceleration")


def _detect_high_perf_graphics() -> bool:
    return SESSION.read_dword(_JAVA_JRE, "JavaFXHardwareAcceleration") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-java-update",
        label="Disable Java Auto-Update",
        category="Java",
        apply_fn=_apply_disable_update,
        remove_fn=_remove_disable_update,
        detect_fn=_detect_disable_update,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_UPDATE, _JAVA_UPDATE32],
        description="Disables the Java automatic update scheduler (both 32-bit and 64-bit).",
        tags=["java", "update"],
    ),
    TweakDef(
        id="disable-java-web-plugin",
        label="Disable Java Web Plugin",
        category="Java",
        apply_fn=_apply_disable_web,
        remove_fn=_remove_disable_web,
        detect_fn=_detect_disable_web,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_WEB],
        description="Disables the Java browser web plugin for better security.",
        tags=["java", "security", "web"],
    ),
    TweakDef(
        id="disable-java-tracking",
        label="Disable Java Usage Tracking",
        category="Java",
        apply_fn=_apply_disable_tracking,
        remove_fn=_remove_disable_tracking,
        detect_fn=_detect_disable_tracking,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_USAGE, _JAVA_WEB],
        description="Disables Java's built-in usage tracking and telemetry reporting.",
        tags=["java", "telemetry", "privacy"],
    ),
    TweakDef(
        id="java-high-dpi",
        label="Java: Enable High DPI Scaling",
        category="Java",
        apply_fn=_apply_java_dpi,
        remove_fn=_remove_java_dpi,
        detect_fn=_detect_java_dpi,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_DPI],
        description="Marks the Java runtime as DPI-aware to fix blurry rendering on high-DPI displays.",
        tags=["java", "display", "dpi"],
    ),
    TweakDef(
        id="disable-java-sponsor",
        label="Disable Java Sponsor Offers",
        category="Java",
        apply_fn=_apply_disable_sponsor,
        remove_fn=_remove_disable_sponsor,
        detect_fn=_detect_disable_sponsor,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_SPONSOR, _JAVA_SPONSOR32],
        description="Prevents third-party sponsor offer pop-ups during Java installations and updates.",
        tags=["java", "sponsor", "bloat"],
    ),
    TweakDef(
        id="java-security-high",
        label="Java: Set Security Level to Very High",
        category="Java",
        apply_fn=_apply_java_security_high,
        remove_fn=_remove_java_security_high,
        detect_fn=_detect_java_security_high,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_SEC],
        description="Raises the Java security level to VERY_HIGH, blocking unsigned applets.",
        tags=["java", "security"],
    ),
    TweakDef(
        id="disable-java-error-reporting",
        label="Disable Java Error Reporting",
        category="Java",
        apply_fn=_apply_disable_error_reporting,
        remove_fn=_remove_disable_error_reporting,
        detect_fn=_detect_disable_error_reporting,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_ERR],
        description="Suppresses Java exception dialog pop-ups and error reporting.",
        tags=["java", "telemetry", "errors"],
    ),
    TweakDef(
        id="disable-java-tip-of-day",
        label="Disable Java Tip of the Day",
        category="Java",
        apply_fn=_apply_disable_java_tip,
        remove_fn=_remove_disable_java_tip,
        detect_fn=_detect_disable_java_tip,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_WEB],
        description="Disables the 'Tip of the Day' pop-up dialog in Java Control Panel.",
        tags=["java", "ui", "annoyance"],
    ),
    TweakDef(
        id="disable-java-cert-revoke",
        label="Disable Java Certificate Revocation Check",
        category="Java",
        apply_fn=_apply_disable_cert_revoke,
        remove_fn=_remove_disable_cert_revoke,
        detect_fn=_detect_disable_cert_revoke,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_JAVA_WEB],
        description="Disables OCSP certificate revocation checks for faster Java app startup (less secure).",
        tags=["java", "security", "performance"],
    ),
    TweakDef(
        id="java-disable-update-check",
        label="Disable Java Auto-Update Check",
        category="Java",
        apply_fn=_apply_disable_update_check,
        remove_fn=_remove_disable_update_check,
        detect_fn=_detect_disable_update_check,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_UPDATE_CHK],
        description=(
            "Disables Java's automatic update check at startup. Reduces background network traffic. "
            "Default: Enabled. Recommended: Disabled for managed environments."
        ),
        tags=["java", "update", "performance"],
    ),
    TweakDef(
        id="java-high-perf-graphics",
        label="Java High Performance Graphics",
        category="Java",
        apply_fn=_apply_high_perf_graphics,
        remove_fn=_remove_high_perf_graphics,
        detect_fn=_detect_high_perf_graphics,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_JRE],
        description=(
            "Enables hardware graphics acceleration for Java/JavaFX applications. "
            "Improves rendering performance. Default: Software. Recommended: Hardware."
        ),
        tags=["java", "graphics", "performance"],
    ),
]


# -- 12. Disable Java Auto-Update ────────────────────────────────────────────


def _apply_java_no_auto_upd(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_JAVA_UPDATE], "JavaDisableAutoUpdate")
    SESSION.set_dword(_JAVA_UPDATE, "EnableAutoUpdateCheck", 0)


def _remove_java_no_auto_upd(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_JAVA_UPDATE, "EnableAutoUpdateCheck", 1)


def _detect_java_no_auto_upd() -> bool:
    return SESSION.read_dword(_JAVA_UPDATE, "EnableAutoUpdateCheck") == 0


# -- 13. Disable Java Sponsor Offers ─────────────────────────────────────────


def _apply_java_no_sponsor_offers(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_JAVA_UPDATE], "JavaDisableSponsor")
    SESSION.set_dword(_JAVA_UPDATE, "EnableJavaUpdate", 0)


def _remove_java_no_sponsor_offers(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_JAVA_UPDATE, "EnableJavaUpdate", 1)


def _detect_java_no_sponsor_offers() -> bool:
    return SESSION.read_dword(_JAVA_UPDATE, "EnableJavaUpdate") == 0


TWEAKS += [
    TweakDef(
        id="java-disable-auto-update",
        label="Disable Java Auto-Update",
        category="Java",
        apply_fn=_apply_java_no_auto_upd,
        remove_fn=_remove_java_no_auto_upd,
        detect_fn=_detect_java_no_auto_upd,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_JAVA_UPDATE],
        description="Disables Java automatic update checks. Default: Enabled. Recommended: Disabled for managed environments.",
        tags=["java", "auto-update", "update"],
    ),
    TweakDef(
        id="java-disable-sponsor-offers",
        label="Disable Java Sponsor Offers",
        category="Java",
        apply_fn=_apply_java_no_sponsor_offers,
        remove_fn=_remove_java_no_sponsor_offers,
        detect_fn=_detect_java_no_sponsor_offers,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_JAVA_UPDATE],
        description="Disables sponsor/adware offers bundled with Java updates. Default: Enabled. Recommended: Disabled.",
        tags=["java", "sponsor", "adware", "offers"],
    ),
]


# -- 14. Disable Java Auto-Update Scheduler Notifications ─────────────────────


def _apply_java_disable_update_scheduler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable auto-update scheduler notifications")
    SESSION.backup([_JAVA_UPDATE], "JavaUpdateScheduler")
    SESSION.set_dword(_JAVA_UPDATE, "NotifyDownload", 0)
    SESSION.set_dword(_JAVA_UPDATE, "NotifyInstall", 0)


def _remove_java_disable_update_scheduler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_JAVA_UPDATE, "NotifyDownload", 1)
    SESSION.set_dword(_JAVA_UPDATE, "NotifyInstall", 1)


def _detect_java_disable_update_scheduler() -> bool:
    return (
        SESSION.read_dword(_JAVA_UPDATE, "NotifyDownload") == 0
        and SESSION.read_dword(_JAVA_UPDATE, "NotifyInstall") == 0
    )


# -- 15. Set Java Security Level to Very High ─────────────────────────────────


def _apply_java_security_veryhigh(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: set deployment security level to VERY_HIGH")
    SESSION.backup([_JAVA_WEB], "JavaSecVeryHigh")
    SESSION.set_string(_JAVA_WEB, "deployment.security.level", "VERY_HIGH")


def _remove_java_security_veryhigh(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_JAVA_WEB, "deployment.security.level", "HIGH")


def _detect_java_security_veryhigh() -> bool:
    return SESSION.read_string(_JAVA_WEB, "deployment.security.level") == "VERY_HIGH"


# -- 16. Disable Java Usage Tracking ──────────────────────────────────────────

_JAVA_ANALYTICS_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"


def _apply_java_disable_usage_tracking(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable usage tracking")
    SESSION.backup([_JAVA_ANALYTICS_KEY], "JavaUsageTracking")
    SESSION.set_string(_JAVA_ANALYTICS_KEY, "deployment.usagetracker.enabled", "false")


def _remove_java_disable_usage_tracking(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_JAVA_ANALYTICS_KEY, "deployment.usagetracker.enabled")


def _detect_java_disable_usage_tracking() -> bool:
    return SESSION.read_string(_JAVA_ANALYTICS_KEY, "deployment.usagetracker.enabled") == "false"


TWEAKS += [
    TweakDef(
        id="java-disable-update-scheduler",
        label="Disable Java Update Scheduler Notifications",
        category="Java",
        apply_fn=_apply_java_disable_update_scheduler,
        remove_fn=_remove_java_disable_update_scheduler,
        detect_fn=_detect_java_disable_update_scheduler,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_JAVA_UPDATE],
        description="Disables Java update scheduler download/install notifications. Default: Enabled. Recommended: Disabled.",
        tags=["java", "update", "scheduler", "notifications"],
    ),
    TweakDef(
        id="java-security-veryhigh",
        label="Set Java Security Level to Very High",
        category="Java",
        apply_fn=_apply_java_security_veryhigh,
        remove_fn=_remove_java_security_veryhigh,
        detect_fn=_detect_java_security_veryhigh,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_JAVA_WEB],
        description="Sets Java deployment security level to VERY_HIGH via policy. Default: HIGH. Recommended: VERY_HIGH.",
        tags=["java", "security", "deployment", "veryhigh"],
    ),
    TweakDef(
        id="java-disable-usage-tracking",
        label="Disable Java Usage Tracking",
        category="Java",
        apply_fn=_apply_java_disable_usage_tracking,
        remove_fn=_remove_java_disable_usage_tracking,
        detect_fn=_detect_java_disable_usage_tracking,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_JAVA_ANALYTICS_KEY],
        description="Disables Java usage tracker analytics. Default: Enabled. Recommended: Disabled for privacy.",
        tags=["java", "usage", "tracking", "analytics", "privacy"],
    ),
]
