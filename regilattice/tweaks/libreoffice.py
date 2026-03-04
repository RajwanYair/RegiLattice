"""LibreOffice / Apache OpenOffice registry tweaks.

LibreOffice stores its settings primarily in user profile XML files,
but on Windows some installer-level preferences are in the registry.
These tweaks cover common registry-accessible settings.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_LO_UPDATE = (
    r"HKEY_CURRENT_USER\Software\LibreOffice\UNO\InstallPath"
)
_LO_SHELL = r"HKEY_LOCAL_MACHINE\SOFTWARE\LibreOffice\LibreOffice"
_OO_SHELL = r"HKEY_LOCAL_MACHINE\SOFTWARE\OpenOffice"

# Installer-level auto-update (Windows MSI property):
_LO_MAINTENANCE = r"HKEY_LOCAL_MACHINE\SOFTWARE\LibreOffice\MaintenanceService"
_OO_MAINTENANCE = r"HKEY_LOCAL_MACHINE\SOFTWARE\OpenOffice.org\MaintenanceService"

# File association keys for default-format tweaking
_LO_CAPS = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\LibreOffice\Capabilities"
)

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


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
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
]
