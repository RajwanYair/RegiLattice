"""Adobe Reader / Acrobat registry tweaks.

Covers: auto-update, telemetry, protected mode, JavaScript.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

# Adobe Reader DC / Acrobat DC policy paths
_UPDATER = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"
_UPDATER2 = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Adobe Acrobat\DC\FeatureLockDown"
_ARM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Adobe\Adobe ARM\1.0\ARM"
_JS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"
_WELCOME = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen"
_PROTECTED = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"
_CLOUD = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud"
_CC_FILES = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXWelcome"
_GENUINE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\AdobeGenuineSoftware"
_CRASH = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess"
_HOME = r"HKEY_CURRENT_USER\Software\Adobe\CommonFiles\UsageCC"
_FONT_SYNC = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXDesktop"
_ARM_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\ARM\1\ARM"
_READER_GENERAL = r"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral"


# ── Disable Adobe Auto-Update ───────────────────────────────────────────────


def _apply_disable_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable auto-update (Reader + Acrobat)")
    SESSION.backup([_ARM, _UPDATER, _UPDATER2], "AdobeUpdate")
    SESSION.set_dword(_ARM, "iCheckReader", 0)
    SESSION.set_dword(_UPDATER, "bUpdater", 0)
    SESSION.set_dword(_UPDATER2, "bUpdater", 0)


def _remove_disable_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_ARM, "iCheckReader", 3)
    SESSION.delete_value(_UPDATER, "bUpdater")
    SESSION.delete_value(_UPDATER2, "bUpdater")


def _detect_disable_update() -> bool:
    return SESSION.read_dword(_ARM, "iCheckReader") == 0


# ── Disable Adobe Telemetry ─────────────────────────────────────────────────


def _apply_disable_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable usage data collection")
    SESSION.backup([_UPDATER], "AdobeTelemetry")
    SESSION.set_dword(_UPDATER, "bUsageMeasurement", 0)
    SESSION.set_dword(_UPDATER, "bAcroSuppressUpsell", 1)


def _remove_disable_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_UPDATER, "bUsageMeasurement")
    SESSION.delete_value(_UPDATER, "bAcroSuppressUpsell")


def _detect_disable_telemetry() -> bool:
    return SESSION.read_dword(_UPDATER, "bUsageMeasurement") == 0


# ── Disable Adobe JavaScript ────────────────────────────────────────────────


def _apply_disable_js(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable JavaScript in PDFs")
    SESSION.backup([_JS], "AdobeJS")
    SESSION.set_dword(_JS, "bDisableJavaScript", 1)


def _remove_disable_js(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_JS, "bDisableJavaScript", 0)


def _detect_disable_js() -> bool:
    return SESSION.read_dword(_JS, "bDisableJavaScript") == 1


# ── Disable Adobe Welcome Screen ────────────────────────────────────────────


def _apply_disable_welcome(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable welcome screen")
    SESSION.backup([_WELCOME], "AdobeWelcome")
    SESSION.set_dword(_WELCOME, "bShowWelcomeScreen", 0)


def _remove_disable_welcome(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WELCOME, "bShowWelcomeScreen")


def _detect_disable_welcome() -> bool:
    return SESSION.read_dword(_WELCOME, "bShowWelcomeScreen") == 0


# ── Enable Adobe Protected Mode (Sandbox) ─────────────────────────────────


def _apply_protected_mode(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: enable Protected Mode (sandbox)")
    SESSION.backup([_PROTECTED], "AdobeProtected")
    SESSION.set_dword(_PROTECTED, "bProtectedMode", 1)
    SESSION.set_dword(_PROTECTED, "iProtectedView", 2)  # 2 = All files


def _remove_protected_mode(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PROTECTED, "bProtectedMode")
    SESSION.delete_value(_PROTECTED, "iProtectedView")


def _detect_protected_mode() -> bool:
    return SESSION.read_dword(_PROTECTED, "bProtectedMode") == 1


# ── Disable Adobe Cloud Services ──────────────────────────────────────────


def _apply_disable_cloud(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable cloud storage integration")
    SESSION.backup([_CLOUD], "AdobeCloud")
    SESSION.set_dword(_CLOUD, "bDisableADCFileStore", 1)
    SESSION.set_dword(_CLOUD, "bUpdatesHidden", 1)


def _remove_disable_cloud(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLOUD, "bDisableADCFileStore")
    SESSION.delete_value(_CLOUD, "bUpdatesHidden")


def _detect_disable_cloud() -> bool:
    return SESSION.read_dword(_CLOUD, "bDisableADCFileStore") == 1


# ── Disable Adobe Creative Cloud File Sync ──────────────────────────────────


def _apply_disable_cc_files(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable Creative Cloud file sync")
    SESSION.backup([_CC_FILES], "AdobeCCFiles")
    SESSION.set_dword(_CC_FILES, "SyncDisabled", 1)


def _remove_disable_cc_files(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CC_FILES, "SyncDisabled")


def _detect_disable_cc_files() -> bool:
    return SESSION.read_dword(_CC_FILES, "SyncDisabled") == 1


# ── Disable Adobe Genuine Software Check ────────────────────────────────────


def _apply_disable_genuine_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable Genuine Software check")
    SESSION.backup([_GENUINE], "AdobeGenuine")
    SESSION.set_dword(_GENUINE, "AdobeGenuineEnabled", 0)


def _remove_disable_genuine_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GENUINE, "AdobeGenuineEnabled")


def _detect_disable_genuine_check() -> bool:
    return SESSION.read_dword(_GENUINE, "AdobeGenuineEnabled") == 0


# ── Disable Adobe Crash Reporter ────────────────────────────────────────────


def _apply_disable_crash_reporter(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable crash reporter")
    SESSION.backup([_CRASH], "AdobeCrash")
    SESSION.set_dword(_CRASH, "CrashReporting", 0)


def _remove_disable_crash_reporter(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CRASH, "CrashReporting")


def _detect_disable_crash_reporter() -> bool:
    return SESSION.read_dword(_CRASH, "CrashReporting") == 0


# ── Disable Adobe Home Screen on Launch ─────────────────────────────────────


def _apply_disable_home_screen() -> None:
    SESSION.log("Adobe: disable home screen on launch")
    SESSION.backup([_HOME], "AdobeHomeScreen")
    SESSION.set_dword(_HOME, "ShowHomeScreen", 0)


def _remove_disable_home_screen() -> None:
    SESSION.set_dword(_HOME, "ShowHomeScreen", 1)


def _detect_disable_home_screen() -> bool:
    return SESSION.read_dword(_HOME, "ShowHomeScreen") == 0


# ── Disable Adobe Font Sync ─────────────────────────────────────────────────


def _apply_disable_font_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable font sync")
    SESSION.backup([_FONT_SYNC], "AdobeFontSync")
    SESSION.set_dword(_FONT_SYNC, "DisableFontSync", 1)


def _remove_disable_font_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FONT_SYNC, "DisableFontSync")


def _detect_disable_font_sync() -> bool:
    return SESSION.read_dword(_FONT_SYNC, "DisableFontSync") == 1


# ── Disable Adobe Updater (Policy) ──────────────────────────────────────────


def _apply_disable_updater(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable Adobe Updater via ARM policy")
    SESSION.backup([_ARM_POLICY], "AdobeUpdater")
    SESSION.set_dword(_ARM_POLICY, "iCheckReader", 0)


def _remove_disable_updater(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ARM_POLICY, "iCheckReader")


def _detect_disable_updater() -> bool:
    return SESSION.read_dword(_ARM_POLICY, "iCheckReader") == 0


# ── Adobe Reduce Memory Usage ──────────────────────────────────────────────


def _apply_reduce_memory(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: enable instance reuse to reduce memory")
    SESSION.backup([_READER_GENERAL], "AdobeReduceMemory")
    SESSION.set_dword(_READER_GENERAL, "bReuseAcrobatInstance", 1)


def _remove_reduce_memory(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_READER_GENERAL, "bReuseAcrobatInstance")


def _detect_reduce_memory() -> bool:
    return SESSION.read_dword(_READER_GENERAL, "bReuseAcrobatInstance") == 1


# ── Disable Adobe Updater Service ────────────────────────────────────────────

_ADOBE_UPDATE_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\AdobeUpdate"


def _apply_disable_updater_service(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable Adobe Updater service via registry")
    SESSION.backup([_ADOBE_UPDATE_POLICY], "AdobeUpdaterService")
    SESSION.set_dword(_ADOBE_UPDATE_POLICY, "UpdaterEnabled", 0)


def _remove_disable_updater_service(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ADOBE_UPDATE_POLICY, "UpdaterEnabled")


def _detect_disable_updater_service() -> bool:
    return SESSION.read_dword(_ADOBE_UPDATE_POLICY, "UpdaterEnabled") == 0


# ── Disable Adobe Analytics ──────────────────────────────────────────────────

_ADOBE_ANALYTICS = r"HKEY_CURRENT_USER\Software\Adobe\CommonFiles\Usage"


def _apply_disable_adobe_analytics(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable analytics/telemetry opt-in")
    SESSION.backup([_ADOBE_ANALYTICS], "AdobeAnalytics")
    SESSION.set_dword(_ADOBE_ANALYTICS, "OptIn", 0)


def _remove_disable_adobe_analytics(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_ADOBE_ANALYTICS, "OptIn", 1)


def _detect_disable_adobe_analytics() -> bool:
    return SESSION.read_dword(_ADOBE_ANALYTICS, "OptIn") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="adobe-disable-adobe-update",
        label="Disable Adobe Auto-Update",
        category="Adobe",
        apply_fn=_apply_disable_update,
        remove_fn=_remove_disable_update,
        detect_fn=_detect_disable_update,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ARM, _UPDATER, _UPDATER2],
        description="Disables automatic updates for Adobe Reader and Acrobat DC.",
        tags=["adobe", "update"],
    ),
    TweakDef(
        id="adobe-disable-adobe-telemetry",
        label="Disable Adobe Telemetry",
        category="Adobe",
        apply_fn=_apply_disable_telemetry,
        remove_fn=_remove_disable_telemetry,
        detect_fn=_detect_disable_telemetry,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_UPDATER],
        description="Disables Adobe usage data collection and suppresses upsell prompts.",
        tags=["adobe", "telemetry", "privacy"],
    ),
    TweakDef(
        id="adobe-disable-adobe-javascript",
        label="Disable Adobe PDF JavaScript",
        category="Adobe",
        apply_fn=_apply_disable_js,
        remove_fn=_remove_disable_js,
        detect_fn=_detect_disable_js,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JS],
        description=("Disables JavaScript execution in PDF documents — major security hardening."),
        tags=["adobe", "security", "javascript"],
    ),
    TweakDef(
        id="adobe-disable-adobe-welcome",
        label="Disable Adobe Welcome Screen",
        category="Adobe",
        apply_fn=_apply_disable_welcome,
        remove_fn=_remove_disable_welcome,
        detect_fn=_detect_disable_welcome,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WELCOME],
        description="Suppresses the Adobe Reader welcome / start screen on launch.",
        tags=["adobe", "ux"],
    ),
    TweakDef(
        id="adobe-enable-adobe-protected-mode",
        label="Enable Adobe Protected Mode",
        category="Adobe",
        apply_fn=_apply_protected_mode,
        remove_fn=_remove_protected_mode,
        detect_fn=_detect_protected_mode,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PROTECTED],
        description="Enables Protected Mode sandbox and Protected View for all PDF files.",
        tags=["adobe", "security", "sandbox"],
    ),
    TweakDef(
        id="adobe-disable-adobe-cloud",
        label="Disable Adobe Cloud Services",
        category="Adobe",
        apply_fn=_apply_disable_cloud,
        remove_fn=_remove_disable_cloud,
        detect_fn=_detect_disable_cloud,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CLOUD],
        description="Disables Adobe Document Cloud file storage integration.",
        tags=["adobe", "cloud", "privacy"],
    ),
    TweakDef(
        id="adobe-disable-cc-files",
        label="Disable Adobe Creative Cloud File Sync",
        category="Adobe",
        apply_fn=_apply_disable_cc_files,
        remove_fn=_remove_disable_cc_files,
        detect_fn=_detect_disable_cc_files,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CC_FILES],
        description="Disables Adobe Creative Cloud file synchronization.",
        tags=["adobe", "cloud", "sync"],
    ),
    TweakDef(
        id="adobe-disable-genuine-check",
        label="Disable Adobe Genuine Software Check",
        category="Adobe",
        apply_fn=_apply_disable_genuine_check,
        remove_fn=_remove_disable_genuine_check,
        detect_fn=_detect_disable_genuine_check,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GENUINE],
        description="Disables the Adobe Genuine Software integrity check.",
        tags=["adobe", "genuine", "licensing"],
    ),
    TweakDef(
        id="adobe-disable-crash-reporter",
        label="Disable Adobe Crash Reporter",
        category="Adobe",
        apply_fn=_apply_disable_crash_reporter,
        remove_fn=_remove_disable_crash_reporter,
        detect_fn=_detect_disable_crash_reporter,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CRASH],
        description="Disables Adobe crash reporting.",
        tags=["adobe", "telemetry", "crash"],
    ),
    TweakDef(
        id="adobe-disable-home-screen",
        label="Disable Adobe Home Screen on Launch",
        category="Adobe",
        apply_fn=_apply_disable_home_screen,
        remove_fn=_remove_disable_home_screen,
        detect_fn=_detect_disable_home_screen,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_HOME],
        description="Disables the Adobe home screen shown on application launch.",
        tags=["adobe", "ux", "home"],
    ),
    TweakDef(
        id="adobe-disable-font-sync",
        label="Disable Adobe Font Sync",
        category="Adobe",
        apply_fn=_apply_disable_font_sync,
        remove_fn=_remove_disable_font_sync,
        detect_fn=_detect_disable_font_sync,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_FONT_SYNC],
        description="Disables Adobe Creative Cloud font synchronization.",
        tags=["adobe", "fonts", "sync"],
    ),
    TweakDef(
        id="adobe-disable-updater",
        label="Disable Adobe Updater",
        category="Adobe",
        apply_fn=_apply_disable_updater,
        remove_fn=_remove_disable_updater,
        detect_fn=_detect_disable_updater,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ARM_POLICY],
        description=(
            "Disables Adobe Acrobat Reader automatic updater. Updates must be "
            "applied manually. Default: Enabled. Recommended: Disabled for "
            "managed environments."
        ),
        tags=["adobe", "updater", "performance"],
    ),
    TweakDef(
        id="adobe-reduce-memory",
        label="Adobe Reduce Memory Usage",
        category="Adobe",
        apply_fn=_apply_reduce_memory,
        remove_fn=_remove_reduce_memory,
        detect_fn=_detect_reduce_memory,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_READER_GENERAL],
        description=(
            "Forces Adobe Reader to reuse existing instances instead of "
            "spawning new processes. Reduces memory footprint. "
            "Default: New instance. Recommended: Reuse."
        ),
        tags=["adobe", "memory", "performance"],
    ),
    TweakDef(
        id="adobe-disable-updater-service",
        label="Disable Adobe Updater Service",
        category="Adobe",
        apply_fn=_apply_disable_updater_service,
        remove_fn=_remove_disable_updater_service,
        detect_fn=_detect_disable_updater_service,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ADOBE_UPDATE_POLICY],
        description=("Disables the Adobe Updater service via registry policy. Prevents background update checks and downloads."),
        tags=["adobe", "updater", "service", "performance"],
    ),
    TweakDef(
        id="adobe-disable-analytics",
        label="Disable Adobe Analytics",
        category="Adobe",
        apply_fn=_apply_disable_adobe_analytics,
        remove_fn=_remove_disable_adobe_analytics,
        detect_fn=_detect_disable_adobe_analytics,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADOBE_ANALYTICS],
        description=("Disables Adobe analytics and telemetry data collection by opting out of usage tracking."),
        tags=["adobe", "analytics", "telemetry", "privacy"],
    ),
]


# ── Disable Adobe Creative Cloud Sync ────────────────────────────────────────

_CC_SYNC = r"HKEY_CURRENT_USER\Software\Adobe\CreativeCloud"
_ADOBE_APP_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\AdobeApp"


def _apply_cc_sync_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Adobe Creative Cloud sync")
    SESSION.backup([_CC_SYNC], "AdobeCCSync")
    SESSION.set_dword(_CC_SYNC, "SyncEnabled", 0)


def _remove_cc_sync_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CC_SYNC, "SyncEnabled")


def _detect_cc_sync_off() -> bool:
    return SESSION.read_dword(_CC_SYNC, "SyncEnabled") == 0


# ── Disable Adobe CEF Helper ────────────────────────────────────────────────


def _apply_cef_helper_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Adobe CEF Helper subprocess")
    SESSION.backup([_ADOBE_APP_POLICY], "AdobeCEFHelper")
    SESSION.set_dword(_ADOBE_APP_POLICY, "DisableCEFHelper", 1)


def _remove_cef_helper_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ADOBE_APP_POLICY, "DisableCEFHelper")


def _detect_cef_helper_off() -> bool:
    return SESSION.read_dword(_ADOBE_APP_POLICY, "DisableCEFHelper") == 1


TWEAKS += [
    TweakDef(
        id="adobe-disable-cloud-sync",
        label="Disable Adobe Creative Cloud Sync",
        category="Adobe",
        apply_fn=_apply_cc_sync_off,
        remove_fn=_remove_cc_sync_off,
        detect_fn=_detect_cc_sync_off,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CC_SYNC],
        description=(
            "Disables Adobe Creative Cloud file and settings sync. "
            "Reduces background network activity and cloud dependency. "
            "Default: Enabled. Recommended: Disabled on managed machines."
        ),
        tags=["adobe", "cloud", "sync", "creative-cloud"],
    ),
    TweakDef(
        id="adobe-disable-cef-subprocess",
        label="Disable Adobe CEF Helper",
        category="Adobe",
        apply_fn=_apply_cef_helper_off,
        remove_fn=_remove_cef_helper_off,
        detect_fn=_detect_cef_helper_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_ADOBE_APP_POLICY],
        description=(
            "Disables Adobe CEF (Chromium Embedded Framework) helper "
            "processes via policy. Reduces memory and CPU usage. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["adobe", "cef", "helper", "performance"],
    ),
]


# -- Disable Adobe Acrobat Cloud Services -------------------------------------


def _apply_disable_acrobat_cloud(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disabling Acrobat cloud file store")
    SESSION.backup([_CLOUD], "AcrobatCloud")
    SESSION.set_dword(_CLOUD, "bDisableADCFileStore", 1)


def _remove_disable_acrobat_cloud(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLOUD, "bDisableADCFileStore")


def _detect_disable_acrobat_cloud() -> bool:
    return SESSION.read_dword(_CLOUD, "bDisableADCFileStore") == 1


# -- Set PDF Default View to Single Page --------------------------------------


def _apply_pdf_single_page_view(*, require_admin: bool = True) -> None:
    SESSION.log("Adobe: setting default PDF view to single page")
    SESSION.backup([_READER_GENERAL], "PDFSinglePage")
    SESSION.set_dword(_READER_GENERAL, "iPageViewLayoutMode", 0)


def _remove_pdf_single_page_view(*, require_admin: bool = True) -> None:
    SESSION.delete_value(_READER_GENERAL, "iPageViewLayoutMode")


def _detect_pdf_single_page_view() -> bool:
    return SESSION.read_dword(_READER_GENERAL, "iPageViewLayoutMode") == 0


# -- Disable Adobe Whats New Screen -------------------------------------------


def _apply_disable_whats_new(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disabling What's New screen on launch")
    SESSION.backup([_WELCOME], "AdobeWhatsNew")
    SESSION.set_dword(_WELCOME, "bShowWhatsNew", 0)


def _remove_disable_whats_new(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WELCOME, "bShowWhatsNew")


def _detect_disable_whats_new() -> bool:
    return SESSION.read_dword(_WELCOME, "bShowWhatsNew") == 0


TWEAKS += [
    TweakDef(
        id="adobe-disable-acrobat-cloud",
        label="Disable Adobe Acrobat Cloud Services",
        category="Adobe",
        apply_fn=_apply_disable_acrobat_cloud,
        remove_fn=_remove_disable_acrobat_cloud,
        detect_fn=_detect_disable_acrobat_cloud,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CLOUD],
        description=(
            "Disables Adobe Document Cloud file store integration in "
            "Acrobat Reader. Prevents cloud save prompts. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["adobe", "acrobat", "cloud", "document-cloud"],
    ),
    TweakDef(
        id="adobe-pdf-single-page-view",
        label="Set PDF Default View to Single Page",
        category="Adobe",
        apply_fn=_apply_pdf_single_page_view,
        remove_fn=_remove_pdf_single_page_view,
        detect_fn=_detect_pdf_single_page_view,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_READER_GENERAL],
        description=(
            "Sets the default PDF page layout to single page view. "
            "Overrides continuous scroll as the default. "
            "Default: continuous. Recommended: single page."
        ),
        tags=["adobe", "pdf", "view", "layout", "single-page"],
    ),
    TweakDef(
        id="adobe-disable-welcome-screen",
        label="Disable Adobe What's New Screen",
        category="Adobe",
        apply_fn=_apply_disable_whats_new,
        remove_fn=_remove_disable_whats_new,
        detect_fn=_detect_disable_whats_new,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WELCOME],
        description=(
            "Disables the What's New promotional screen shown after "
            "Adobe Reader updates. Different from the start screen. "
            "Default: shown. Recommended: hidden."
        ),
        tags=["adobe", "welcome", "whats-new", "ux"],
    ),
]
