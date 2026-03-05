"""LibreOffice / Apache OpenOffice registry tweaks.

LibreOffice stores its settings primarily in user profile XML files,
but on Windows some installer-level preferences are in the registry.
These tweaks cover common registry-accessible settings.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

# Installer-level auto-update (Windows MSI property):
_LO_MAINTENANCE = r"HKEY_LOCAL_MACHINE\SOFTWARE\LibreOffice\MaintenanceService"
_OO_MAINTENANCE = r"HKEY_LOCAL_MACHINE\SOFTWARE\OpenOffice.org\MaintenanceService"

# UNO misc settings (HKCU):
_LO_UNO_MISC = r"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"

# ── Disable LibreOffice Auto-Update ─────────────────────────────────────────


def _apply_disable_autoupdate(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: disable auto-update service")
    for key in (_LO_MAINTENANCE, _OO_MAINTENANCE):
        SESSION.backup([key], "LOAutoUpdate")
        SESSION.set_dword(key, "Enable", 0)


def _remove_disable_autoupdate(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    for key in (_LO_MAINTENANCE, _OO_MAINTENANCE):
        SESSION.set_dword(key, "Enable", 1)


def _detect_disable_autoupdate() -> bool:
    return SESSION.read_dword(_LO_MAINTENANCE, "Enable") == 0


# ── Disable LibreOffice Crash Reporter ───────────────────────────────────────

_LO_CRASH = (
    r"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport"
)


def _apply_disable_crash(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: disable crash reporter")
    SESSION.backup([_LO_CRASH], "LOCrashReporter")
    SESSION.set_dword(_LO_CRASH, "Enable", 0)
    SESSION.set_dword(_LO_CRASH, "AutoSubmit", 0)


def _remove_disable_crash(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LO_CRASH, "Enable")
    SESSION.delete_value(_LO_CRASH, "AutoSubmit")


def _detect_disable_crash() -> bool:
    return SESSION.read_dword(_LO_CRASH, "Enable") == 0


# ── Set Default Save Format to OOXML (docx/xlsx/pptx) ───────────────────────

_LO_DEFAULT_FMT = (
    r"HKEY_CURRENT_USER\Software\LibreOffice\DefaultFormat"
)


def _apply_default_ooxml(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: default save format → OOXML")
    SESSION.backup([_LO_DEFAULT_FMT], "LODefaultFormat")
    # Stored as human-readable hint; actual config is in registrymodifications.xcu
    SESSION.set_string(_LO_DEFAULT_FMT, "Writer", "MS Word 2007 XML")
    SESSION.set_string(_LO_DEFAULT_FMT, "Calc", "Calc MS Excel 2007 XML")
    SESSION.set_string(_LO_DEFAULT_FMT, "Impress", "Impress MS PowerPoint 2007 XML")


def _remove_default_ooxml(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LO_DEFAULT_FMT, "Writer")
    SESSION.delete_value(_LO_DEFAULT_FMT, "Calc")
    SESSION.delete_value(_LO_DEFAULT_FMT, "Impress")


def _detect_default_ooxml() -> bool:
    return SESSION.read_string(_LO_DEFAULT_FMT, "Writer") == "MS Word 2007 XML"


# ── Register LibreOffice as Default Handler ──────────────────────────────────

_ASSOC_KEY = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts"
_DOC_EXTS = (".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", ".ods", ".odp")
_LO_RECOVERY = r"HKEY_CURRENT_USER\Software\LibreOffice\Recovery"
_LO_START = r"HKEY_CURRENT_USER\Software\LibreOffice\StartCenter"


def _apply_lo_default_handler(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: set as default handler for common document types")
    for ext in _DOC_EXTS:
        choice_key = rf"{_ASSOC_KEY}\{ext}\UserChoice"
        SESSION.backup([choice_key], f"LOHandler_{ext}")
        # ProgId for LibreOffice
        SESSION.set_string(choice_key, "ProgId", f"LibreOffice{ext.lstrip('.')}")


def _remove_lo_default_handler(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ext in _DOC_EXTS:
        choice_key = rf"{_ASSOC_KEY}\{ext}\UserChoice"
        SESSION.delete_value(choice_key, "ProgId")


def _detect_lo_default_handler() -> bool:
    choice = rf"{_ASSOC_KEY}\.odt\UserChoice"
    val = SESSION.read_string(choice, "ProgId")
    return val is not None and "LibreOffice" in val


# ── Disable LibreOffice Recovery Mode ──────────────────────────────────────


def _apply_disable_recovery(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: disable crash recovery dialogs")
    SESSION.backup([_LO_RECOVERY], "LORecovery")
    SESSION.set_dword(_LO_RECOVERY, "AutoSaveEnabled", 0)


def _remove_disable_recovery(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LO_RECOVERY, "AutoSaveEnabled")


def _detect_disable_recovery() -> bool:
    return SESSION.read_dword(_LO_RECOVERY, "AutoSaveEnabled") == 0


# ── Disable LibreOffice Start Center ──────────────────────────────────────


def _apply_disable_startcenter(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: disable Start Center on launch")
    SESSION.backup([_LO_START], "LOStartCenter")
    SESSION.set_dword(_LO_START, "ShowStartCenter", 0)


def _remove_disable_startcenter(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LO_START, "ShowStartCenter")


def _detect_disable_startcenter() -> bool:
    return SESSION.read_dword(_LO_START, "ShowStartCenter") == 0


# ── Disable LibreOffice Crash Reporting (UNO) ─────────────────────────────────


def _apply_disable_crash_reporting(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: disable crash reporting (UNO)")
    SESSION.backup([_LO_UNO_MISC], "LOCrashReporting")
    SESSION.set_string(_LO_UNO_MISC, "CrashReport", "false")


def _remove_disable_crash_reporting(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_LO_UNO_MISC, "CrashReport", "true")


def _detect_disable_crash_reporting() -> bool:
    return SESSION.read_string(_LO_UNO_MISC, "CrashReport") == "false"


# ── Disable LibreOffice Online Update Check ──────────────────────────────────


def _apply_disable_online_update(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: disable online update check")
    SESSION.backup([_LO_UNO_MISC], "LOOnlineUpdate")
    SESSION.set_string(_LO_UNO_MISC, "AutoCheckEnabled", "false")


def _remove_disable_online_update(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_LO_UNO_MISC, "AutoCheckEnabled", "true")


def _detect_disable_online_update() -> bool:
    return SESSION.read_string(_LO_UNO_MISC, "AutoCheckEnabled") == "false"


# ── Disable Start Center Recent News ─────────────────────────────────────────


def _apply_disable_startcenter_news(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: disable Start Center recent news")
    SESSION.backup([_LO_UNO_MISC], "LOStartCenterNews")
    SESSION.set_string(_LO_UNO_MISC, "StartCenterInfoEnabled", "false")


def _remove_disable_startcenter_news(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_LO_UNO_MISC, "StartCenterInfoEnabled", "true")


def _detect_disable_startcenter_news() -> bool:
    return SESSION.read_string(_LO_UNO_MISC, "StartCenterInfoEnabled") == "false"


# ── Disable LibreOffice Macro Execution ──────────────────────────────────────


def _apply_disable_macros(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: disable macro execution (security level 3)")
    SESSION.backup([_LO_UNO_MISC], "LOMacros")
    SESSION.set_string(_LO_UNO_MISC, "MacroSecurityLevel", "3")


def _remove_disable_macros(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_LO_UNO_MISC, "MacroSecurityLevel", "1")


def _detect_disable_macros() -> bool:
    return SESSION.read_string(_LO_UNO_MISC, "MacroSecurityLevel") == "3"


# ── Disable LibreOffice Send Feedback ────────────────────────────────────────


def _apply_disable_send_feedback(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: disable send feedback")
    SESSION.backup([_LO_UNO_MISC], "LOSendFeedback")
    SESSION.set_string(_LO_UNO_MISC, "SendFeedback", "false")


def _remove_disable_send_feedback(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_LO_UNO_MISC, "SendFeedback", "true")


def _detect_disable_send_feedback() -> bool:
    return SESSION.read_string(_LO_UNO_MISC, "SendFeedback") == "false"


# ── Disable LibreOffice Java Runtime ─────────────────────────────────────────


def _apply_disable_java(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: disable Java runtime")
    SESSION.backup([_LO_UNO_MISC], "LOJava")
    SESSION.set_string(_LO_UNO_MISC, "JavaEnabled", "false")


def _remove_disable_java(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_LO_UNO_MISC, "JavaEnabled", "true")


def _detect_disable_java() -> bool:
    return SESSION.read_string(_LO_UNO_MISC, "JavaEnabled") == "false"


# ── Reduce LibreOffice Auto-Save Interval ───────────────────────────────────


def _apply_autosave_interval(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: set auto-save interval to 3 minutes")
    SESSION.backup([_LO_UNO_MISC], "LOAutoSaveInterval")
    SESSION.set_string(_LO_UNO_MISC, "AutoSaveTimeIntervall", "3")


def _remove_autosave_interval(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_LO_UNO_MISC, "AutoSaveTimeIntervall", "10")


def _detect_autosave_interval() -> bool:
    return SESSION.read_string(_LO_UNO_MISC, "AutoSaveTimeIntervall") == "3"


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-libreoffice-autoupdate",
        label="Disable LibreOffice Auto-Update",
        category="LibreOffice",
        apply_fn=_apply_disable_autoupdate,
        remove_fn=_remove_disable_autoupdate,
        detect_fn=_detect_disable_autoupdate,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LO_MAINTENANCE, _OO_MAINTENANCE],
        description=(
            "Disables the LibreOffice / OpenOffice maintenance service "
            "auto-update mechanism."
        ),
        tags=["libreoffice", "openoffice", "update"],
    ),
    TweakDef(
        id="disable-libreoffice-crash-reporter",
        label="Disable LibreOffice Crash Reporter",
        category="LibreOffice",
        apply_fn=_apply_disable_crash,
        remove_fn=_remove_disable_crash,
        detect_fn=_detect_disable_crash,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LO_CRASH],
        description="Disables the LibreOffice crash reporter and auto-submit.",
        tags=["libreoffice", "telemetry", "privacy"],
    ),
    TweakDef(
        id="libreoffice-default-ooxml",
        label="Default Save as OOXML (docx/xlsx)",
        category="LibreOffice",
        apply_fn=_apply_default_ooxml,
        remove_fn=_remove_default_ooxml,
        detect_fn=_detect_default_ooxml,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LO_DEFAULT_FMT],
        description=(
            "Sets LibreOffice default save format to Microsoft OOXML "
            "(docx, xlsx, pptx) for better interoperability."
        ),
        tags=["libreoffice", "format", "compatibility"],
    ),
    TweakDef(
        id="libreoffice-default-handler",
        label="Set LibreOffice as Default Handler",
        category="LibreOffice",
        apply_fn=_apply_lo_default_handler,
        remove_fn=_remove_lo_default_handler,
        detect_fn=_detect_lo_default_handler,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ASSOC_KEY],
        description=(
            "Registers LibreOffice as the default handler for common "
            "document formats (.doc, .docx, .xls, .xlsx, .ppt, .odt, etc.)."
        ),
        tags=["libreoffice", "file-association", "default"],
    ),
    TweakDef(
        id="disable-libreoffice-recovery",
        label="Disable LibreOffice Recovery",
        category="LibreOffice",
        apply_fn=_apply_disable_recovery,
        remove_fn=_remove_disable_recovery,
        detect_fn=_detect_disable_recovery,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LO_RECOVERY],
        description="Disables LibreOffice crash recovery and auto-save dialogs.",
        tags=["libreoffice", "recovery", "ux"],
    ),
    TweakDef(
        id="disable-libreoffice-startcenter",
        label="Disable LibreOffice Start Center",
        category="LibreOffice",
        apply_fn=_apply_disable_startcenter,
        remove_fn=_remove_disable_startcenter,
        detect_fn=_detect_disable_startcenter,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LO_START],
        description="Opens LibreOffice directly to a new document instead of the Start Center.",
        tags=["libreoffice", "startcenter", "ux"],
    ),
    TweakDef(
        id="libreoffice-disable-crash-reporting",
        label="Disable LibreOffice Crash Reporting",
        category="LibreOffice",
        apply_fn=_apply_disable_crash_reporting,
        remove_fn=_remove_disable_crash_reporting,
        detect_fn=_detect_disable_crash_reporting,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LO_UNO_MISC],
        description="Disables LibreOffice crash reporting via UNO Misc setting.",
        tags=["libreoffice", "telemetry", "privacy", "crash"],
    ),
    TweakDef(
        id="libreoffice-disable-online-update",
        label="Disable LibreOffice Online Update Check",
        category="LibreOffice",
        apply_fn=_apply_disable_online_update,
        remove_fn=_remove_disable_online_update,
        detect_fn=_detect_disable_online_update,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LO_UNO_MISC],
        description="Disables the LibreOffice online update check.",
        tags=["libreoffice", "update", "privacy"],
    ),
    TweakDef(
        id="libreoffice-disable-startcenter-news",
        label="Disable Start Center Recent News",
        category="LibreOffice",
        apply_fn=_apply_disable_startcenter_news,
        remove_fn=_remove_disable_startcenter_news,
        detect_fn=_detect_disable_startcenter_news,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LO_UNO_MISC],
        description="Disables the recent news feed on the LibreOffice Start Center.",
        tags=["libreoffice", "startcenter", "ux", "privacy"],
    ),
    TweakDef(
        id="libreoffice-disable-macros",
        label="Disable LibreOffice Macro Execution",
        category="LibreOffice",
        apply_fn=_apply_disable_macros,
        remove_fn=_remove_disable_macros,
        detect_fn=_detect_disable_macros,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LO_UNO_MISC],
        description=(
            "Sets LibreOffice macro security level to Very High (3), "
            "effectively disabling macro execution."
        ),
        tags=["libreoffice", "macros", "security"],
    ),
    TweakDef(
        id="libreoffice-disable-send-feedback",
        label="Disable LibreOffice Send Feedback",
        category="LibreOffice",
        apply_fn=_apply_disable_send_feedback,
        remove_fn=_remove_disable_send_feedback,
        detect_fn=_detect_disable_send_feedback,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LO_UNO_MISC],
        description="Disables the LibreOffice send feedback feature.",
        tags=["libreoffice", "telemetry", "privacy", "feedback"],
    ),
    TweakDef(
        id="libre-disable-java",
        label="Disable LibreOffice Java Runtime",
        category="LibreOffice",
        apply_fn=_apply_disable_java,
        remove_fn=_remove_disable_java,
        detect_fn=_detect_disable_java,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LO_UNO_MISC],
        description=(
            "Disables Java runtime in LibreOffice. Reduces memory usage "
            "and startup time. Some wizards/macros may require Java. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["libreoffice", "java", "performance", "memory"],
    ),
    TweakDef(
        id="libre-autosave-interval",
        label="Reduce LibreOffice Auto-Save Interval",
        category="LibreOffice",
        apply_fn=_apply_autosave_interval,
        remove_fn=_remove_autosave_interval,
        detect_fn=_detect_autosave_interval,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LO_UNO_MISC],
        description=(
            "Sets LibreOffice auto-save interval to 3 minutes for better "
            "crash recovery. Default: 10 minutes. Recommended: 3 minutes."
        ),
        tags=["libreoffice", "autosave", "recovery"],
    ),
]


# ── Disable LibreOffice Recovery (Policy) ────────────────────────────────────

_LO_RECOVERY_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice"
    r"\org.openoffice.Office.Recovery\Recovery"
)


def _apply_disable_lo_recovery(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: disable document recovery via policy")
    SESSION.backup([_LO_RECOVERY_POLICY], "LORecovery")
    SESSION.set_string(_LO_RECOVERY_POLICY, "AutoSaveEnabled", "false")


def _remove_disable_lo_recovery(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_LO_RECOVERY_POLICY, "AutoSaveEnabled", "true")


def _detect_disable_lo_recovery() -> bool:
    return SESSION.read_string(_LO_RECOVERY_POLICY, "AutoSaveEnabled") == "false"


# ── Disable LibreOffice Macro Execution (Policy) ─────────────────────────────

_LO_MACRO_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice"
    r"\org.openoffice.Office.Common\Security\Scripting"
)


def _apply_disable_lo_macros(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LibreOffice: set macro security to Very High (3)")
    SESSION.backup([_LO_MACRO_POLICY], "LOMacroSec")
    SESSION.set_dword(_LO_MACRO_POLICY, "MacroSecurityLevel", 3)


def _remove_disable_lo_macros(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_LO_MACRO_POLICY, "MacroSecurityLevel", 1)


def _detect_disable_lo_macros() -> bool:
    return SESSION.read_dword(_LO_MACRO_POLICY, "MacroSecurityLevel") == 3


TWEAKS += [
    TweakDef(
        id="libreoffice-disable-recovery",
        label="Disable LibreOffice Document Recovery",
        category="LibreOffice",
        apply_fn=_apply_disable_lo_recovery,
        remove_fn=_remove_disable_lo_recovery,
        detect_fn=_detect_disable_lo_recovery,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_LO_RECOVERY_POLICY],
        description=(
            "Disables LibreOffice automatic document recovery via Group Policy. "
            "Prevents crash recovery prompts on startup. "
            "Default: Enabled. Recommended: Disabled for managed environments."
        ),
        tags=["libreoffice", "recovery", "autosave", "policy"],
    ),
    TweakDef(
        id="libreoffice-disable-macro-exec",
        label="Disable LibreOffice Macro Execution",
        category="LibreOffice",
        apply_fn=_apply_disable_lo_macros,
        remove_fn=_remove_disable_lo_macros,
        detect_fn=_detect_disable_lo_macros,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_LO_MACRO_POLICY],
        description=(
            "Sets LibreOffice macro security level to Very High (3) via policy. "
            "Only trusted signed macros will execute. "
            "Default: 1 (Medium). Recommended: 3 (Very High) for security."
        ),
        tags=["libreoffice", "macros", "security", "policy"],
    ),
]
