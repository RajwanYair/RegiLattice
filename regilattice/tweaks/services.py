"""Services optimization registry tweaks.

Covers: disabling optional services that consume resources silently —
Diagnostic Tracking, SysMain, WSearch, Windows Error Reporting,
Connected User Experiences, and Print Spooler (desktops without printers).
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths (service Start types: 2=Auto, 3=Manual, 4=Disabled) ────────────

_DIAGTRACK = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack"
_DMWAPPUSH = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\dmwappushservice"
_SEARCH = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch"
_WER = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc"
_WER_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\Windows Error Reporting"
)
_SPOOLER = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"
_SYSMAIN = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain"
_CUXE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc"


# ── Disable Diagnostic Tracking Service ──────────────────────────────────────


def _apply_disable_diagtrack(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable DiagTrack (Diagnostic Tracking)")
    SESSION.backup([_DIAGTRACK, _DMWAPPUSH], "DiagTrack")
    SESSION.set_dword(_DIAGTRACK, "Start", 4)
    SESSION.set_dword(_DMWAPPUSH, "Start", 4)


def _remove_disable_diagtrack(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_DIAGTRACK, "Start", 2)  # Auto
    SESSION.set_dword(_DMWAPPUSH, "Start", 3)  # Manual


def _detect_disable_diagtrack() -> bool:
    return SESSION.read_dword(_DIAGTRACK, "Start") == 4


# ── Disable Windows Search Indexer Service ───────────────────────────────────


def _apply_disable_wsearch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Windows Search indexer")
    SESSION.backup([_SEARCH], "WSearch")
    SESSION.set_dword(_SEARCH, "Start", 4)


def _remove_disable_wsearch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH, "Start", 2)


def _detect_disable_wsearch() -> bool:
    return SESSION.read_dword(_SEARCH, "Start") == 4


# ── Disable Windows Error Reporting ──────────────────────────────────────────


def _apply_disable_wer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Windows Error Reporting")
    SESSION.backup([_WER, _WER_POLICY], "WER")
    SESSION.set_dword(_WER, "Start", 4)
    SESSION.set_dword(_WER_POLICY, "Disabled", 1)


def _remove_disable_wer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_WER, "Start", 3)  # Manual
    SESSION.delete_value(_WER_POLICY, "Disabled")


def _detect_disable_wer() -> bool:
    return SESSION.read_dword(_WER, "Start") == 4


# ── Disable Print Spooler ───────────────────────────────────────────────────


def _apply_disable_spooler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Print Spooler")
    SESSION.backup([_SPOOLER], "PrintSpooler")
    SESSION.set_dword(_SPOOLER, "Start", 4)


def _remove_disable_spooler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SPOOLER, "Start", 2)


def _detect_disable_spooler() -> bool:
    return SESSION.read_dword(_SPOOLER, "Start") == 4


# ── Disable SysMain (Superfetch) Service ───────────────────────────────────


def _apply_disable_sysmain(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable SysMain (Superfetch)")
    SESSION.backup([_SYSMAIN], "SysMain")
    SESSION.set_dword(_SYSMAIN, "Start", 4)


def _remove_disable_sysmain(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SYSMAIN, "Start", 2)  # Automatic


def _detect_disable_sysmain() -> bool:
    return SESSION.read_dword(_SYSMAIN, "Start") == 4


# ── Disable Diagnostic Service ────────────────────────────────────────────


def _apply_disable_diagsvc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Diagnostic Service (DiagSvc)")
    SESSION.backup([_CUXE], "DiagSvc")
    SESSION.set_dword(_CUXE, "Start", 4)


def _remove_disable_diagsvc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CUXE, "Start", 3)  # Manual


def _detect_disable_diagsvc() -> bool:
    return SESSION.read_dword(_CUXE, "Start") == 4


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-diagtrack-service",
        label="Disable DiagTrack Service Startup",
        category="Services",
        apply_fn=_apply_disable_diagtrack,
        remove_fn=_remove_disable_diagtrack,
        detect_fn=_detect_disable_diagtrack,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DIAGTRACK, _DMWAPPUSH],
        description=(
            "Disables the DiagTrack and dmwappushservice services that "
            "collect and send diagnostic data to Microsoft."
        ),
        tags=["services", "telemetry", "privacy"],
    ),
    TweakDef(
        id="disable-wsearch",
        label="Disable Windows Search Indexer",
        category="Services",
        apply_fn=_apply_disable_wsearch,
        remove_fn=_remove_disable_wsearch,
        detect_fn=_detect_disable_wsearch,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SEARCH],
        description=(
            "Disables the Windows Search indexer service — helpful on "
            "low-resource machines or when using Everything search."
        ),
        tags=["services", "performance", "disk"],
    ),
    TweakDef(
        id="disable-wer",
        label="Disable Windows Error Reporting",
        category="Services",
        apply_fn=_apply_disable_wer,
        remove_fn=_remove_disable_wer,
        detect_fn=_detect_disable_wer,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WER, _WER_POLICY],
        description="Disables Windows Error Reporting service and crash dump uploads.",
        tags=["services", "telemetry", "privacy"],
    ),
    TweakDef(
        id="disable-print-spooler",
        label="Disable Print Spooler Service",
        category="Services",
        apply_fn=_apply_disable_spooler,
        remove_fn=_remove_disable_spooler,
        detect_fn=_detect_disable_spooler,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SPOOLER],
        description=(
            "Disables the Print Spooler service — reduces attack surface "
            "on machines that don't use local printers."
        ),
        tags=["services", "security", "printer"],
    ),
    TweakDef(
        id="disable-sysmain-service",
        label="Disable SysMain (Superfetch)",
        category="Services",
        apply_fn=_apply_disable_sysmain,
        remove_fn=_remove_disable_sysmain,
        detect_fn=_detect_disable_sysmain,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SYSMAIN],
        description="Disables the SysMain (Superfetch) service — beneficial on SSD systems.",
        tags=["services", "performance", "ssd"],
    ),
    TweakDef(
        id="disable-diagsvc",
        label="Disable Diagnostic Service",
        category="Services",
        apply_fn=_apply_disable_diagsvc,
        remove_fn=_remove_disable_diagsvc,
        detect_fn=_detect_disable_diagsvc,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CUXE],
        description="Disables the Diagnostic Service (DiagSvc) that runs troubleshooters.",
        tags=["services", "telemetry", "diagnostics"],
    ),
]
