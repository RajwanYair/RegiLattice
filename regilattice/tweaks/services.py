"""Services optimization registry tweaks.

Covers: disabling optional services that consume resources silently —
Diagnostic Tracking, SysMain, WSearch, Windows Error Reporting,
Connected User Experiences, and Print Spooler (desktops without printers).
"""

from __future__ import annotations

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


# ── Disable Biometric Service ──────────────────────────────────────────────

_BIOMETRIC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc"


def _apply_disable_biometric(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Windows Biometric Service")
    SESSION.backup([_BIOMETRIC], "Biometric")
    SESSION.set_dword(_BIOMETRIC, "Start", 4)


def _remove_disable_biometric(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BIOMETRIC, "Start", 3)


def _detect_disable_biometric() -> bool:
    return SESSION.read_dword(_BIOMETRIC, "Start") == 4


# ── Disable Fax Service ─────────────────────────────────────────────────────

_FAX = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax"


def _apply_disable_fax(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Fax Service")
    SESSION.backup([_FAX], "Fax")
    SESSION.set_dword(_FAX, "Start", 4)


def _remove_disable_fax(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_FAX, "Start", 3)


def _detect_disable_fax() -> bool:
    return SESSION.read_dword(_FAX, "Start") == 4


# ── Disable Remote Registry ──────────────────────────────────────────────────

_REMREG = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry"


def _apply_disable_remote_registry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Remote Registry")
    SESSION.backup([_REMREG], "RemoteRegistry")
    SESSION.set_dword(_REMREG, "Start", 4)


def _remove_disable_remote_registry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_REMREG, "Start", 3)


def _detect_disable_remote_registry() -> bool:
    return SESSION.read_dword(_REMREG, "Start") == 4


# ── Disable Xbox Services ───────────────────────────────────────────────────

_XBOX_LIVE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblAuthManager"
_XBOX_SAVE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblGameSave"
_XBOX_NET = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc"


def _apply_disable_xbox_services(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Xbox Live services")
    SESSION.backup([_XBOX_LIVE, _XBOX_SAVE, _XBOX_NET], "XboxServices")
    SESSION.set_dword(_XBOX_LIVE, "Start", 4)
    SESSION.set_dword(_XBOX_SAVE, "Start", 4)
    SESSION.set_dword(_XBOX_NET, "Start", 4)


def _remove_disable_xbox_services(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_XBOX_LIVE, "Start", 3)
    SESSION.set_dword(_XBOX_SAVE, "Start", 3)
    SESSION.set_dword(_XBOX_NET, "Start", 3)


def _detect_disable_xbox_services() -> bool:
    return SESSION.read_dword(_XBOX_LIVE, "Start") == 4


# ── Disable Geolocation Service ──────────────────────────────────────────────

_GEOLOC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc"


def _apply_disable_geolocation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Geolocation Service")
    SESSION.backup([_GEOLOC], "Geolocation")
    SESSION.set_dword(_GEOLOC, "Start", 4)


def _remove_disable_geolocation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_GEOLOC, "Start", 3)


def _detect_disable_geolocation() -> bool:
    return SESSION.read_dword(_GEOLOC, "Start") == 4


# ── Disable Delivery Optimization Service ────────────────────────────────────

_DOSERVICE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc"


def _apply_disable_delivery_opt(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Delivery Optimization")
    SESSION.backup([_DOSERVICE], "DeliveryOptimization")
    SESSION.set_dword(_DOSERVICE, "Start", 4)


def _remove_disable_delivery_opt(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_DOSERVICE, "Start", 2)  # Auto


def _detect_disable_delivery_opt() -> bool:
    return SESSION.read_dword(_DOSERVICE, "Start") == 4


# ── Disable AllJoyn Router Service ───────────────────────────────────────────

_ALLJOYN = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AJRouter"


def _apply_disable_alljoyn(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable AllJoyn Router")
    SESSION.backup([_ALLJOYN], "AllJoyn")
    SESSION.set_dword(_ALLJOYN, "Start", 4)


def _remove_disable_alljoyn(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_ALLJOYN, "Start", 3)


def _detect_disable_alljoyn() -> bool:
    return SESSION.read_dword(_ALLJOYN, "Start") == 4


# ── Disable Windows Search Indexer ───────────────────────────────────────────


def _apply_disable_search_indexer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Windows Search indexer service")
    SESSION.backup([_SEARCH], "SearchIndexer")
    SESSION.set_dword(_SEARCH, "Start", 4)


def _remove_disable_search_indexer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH, "Start", 2)


def _detect_disable_search_indexer() -> bool:
    return SESSION.read_dword(_SEARCH, "Start") == 4


# ── Disable SysMain (Superfetch) ─────────────────────────────────────────────


def _apply_disable_sysmain_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable SysMain (Superfetch) preloading service")
    SESSION.backup([_SYSMAIN], "SysMainSvc")
    SESSION.set_dword(_SYSMAIN, "Start", 4)


def _remove_disable_sysmain_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SYSMAIN, "Start", 2)


def _detect_disable_sysmain_svc() -> bool:
    return SESSION.read_dword(_SYSMAIN, "Start") == 4


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
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
        description=("Disables the DiagTrack and dmwappushservice services that collect and send diagnostic data to Microsoft."),
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
        description=("Disables the Windows Search indexer service — helpful on low-resource machines or when using Everything search."),
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
        description=("Disables the Print Spooler service — reduces attack surface on machines that don't use local printers."),
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
    TweakDef(
        id="disable-wbiosrvc",
        label="Disable Biometric Service",
        category="Services",
        apply_fn=_apply_disable_biometric,
        remove_fn=_remove_disable_biometric,
        detect_fn=_detect_disable_biometric,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BIOMETRIC],
        description=("Disables Windows Biometric Service (WbioSrvc). Useful if fingerprint/face login is not used."),
        tags=["services", "biometric", "security"],
    ),
    TweakDef(
        id="disable-fax",
        label="Disable Fax Service",
        category="Services",
        apply_fn=_apply_disable_fax,
        remove_fn=_remove_disable_fax,
        detect_fn=_detect_disable_fax,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_FAX],
        description="Disables the legacy Windows Fax Service.",
        tags=["services", "legacy", "fax"],
    ),
    TweakDef(
        id="disable-remote-registry",
        label="Disable Remote Registry",
        category="Services",
        apply_fn=_apply_disable_remote_registry,
        remove_fn=_remove_disable_remote_registry,
        detect_fn=_detect_disable_remote_registry,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_REMREG],
        description=("Disables the Remote Registry service which allows remote access to the Windows registry. Security hardening measure."),
        tags=["services", "security", "remote"],
    ),
    TweakDef(
        id="disable-xbox-live-services",
        label="Disable Xbox Live Services",
        category="Services",
        apply_fn=_apply_disable_xbox_services,
        remove_fn=_remove_disable_xbox_services,
        detect_fn=_detect_disable_xbox_services,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_XBOX_LIVE, _XBOX_SAVE, _XBOX_NET],
        description="Disables Xbox Live Auth, Game Save, and Networking services.",
        tags=["services", "xbox", "gaming", "cleanup"],
    ),
    TweakDef(
        id="disable-geolocation-service",
        label="Disable Geolocation Service",
        category="Services",
        apply_fn=_apply_disable_geolocation,
        remove_fn=_remove_disable_geolocation,
        detect_fn=_detect_disable_geolocation,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GEOLOC],
        description="Disables the Windows Geolocation Service for privacy.",
        tags=["services", "privacy", "location"],
    ),
    TweakDef(
        id="disable-delivery-optimization-svc",
        label="Disable Delivery Optimization",
        category="Services",
        apply_fn=_apply_disable_delivery_opt,
        remove_fn=_remove_disable_delivery_opt,
        detect_fn=_detect_disable_delivery_opt,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DOSERVICE],
        description=("Disables the Delivery Optimization service which shares Windows Update data with other PCs on LAN and internet."),
        tags=["services", "update", "bandwidth", "privacy"],
    ),
    TweakDef(
        id="disable-alljoyn",
        label="Disable AllJoyn Router",
        category="Services",
        apply_fn=_apply_disable_alljoyn,
        remove_fn=_remove_disable_alljoyn,
        detect_fn=_detect_disable_alljoyn,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ALLJOYN],
        description="Disables the AllJoyn IoT router service — not needed by most users.",
        tags=["services", "iot", "cleanup"],
    ),
    TweakDef(
        id="svc-disable-search-indexer",
        label="Disable Windows Search Indexer",
        category="Services",
        apply_fn=_apply_disable_search_indexer,
        remove_fn=_remove_disable_search_indexer,
        detect_fn=_detect_disable_search_indexer,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SEARCH],
        description=(
            "Disables Windows Search indexing service. Significantly "
            "reduces disk I/O and CPU usage. Use Everything Search as "
            "alternative. Default: Automatic (2). "
            "Recommended: Disabled (4)."
        ),
        tags=["services", "search", "indexer", "performance", "io"],
    ),
    TweakDef(
        id="svc-disable-sysmain",
        label="Disable SysMain (Superfetch)",
        category="Services",
        apply_fn=_apply_disable_sysmain_svc,
        remove_fn=_remove_disable_sysmain_svc,
        detect_fn=_detect_disable_sysmain_svc,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SYSMAIN],
        description=(
            "Disables SysMain (formerly Superfetch) preloading service. "
            "Reduces disk I/O on SSDs where preloading provides minimal "
            "benefit. Default: Automatic (2). "
            "Recommended: Disabled (4) for SSDs."
        ),
        tags=["services", "sysmain", "superfetch", "performance", "ssd"],
    ),
]


# -- Disable Print Spooler (security hardening) --------------------------------


def _apply_svc_disable_print_spooler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disabling Print Spooler (security)")
    SESSION.backup([_SPOOLER], "SvcPrintSpooler")
    SESSION.set_dword(_SPOOLER, "Start", 4)
    SESSION.log("Services: Print Spooler disabled (security)")


def _remove_svc_disable_print_spooler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SPOOLER], "SvcPrintSpooler_Remove")
    SESSION.set_dword(_SPOOLER, "Start", 2)


def _detect_svc_disable_print_spooler() -> bool:
    return SESSION.read_dword(_SPOOLER, "Start") == 4


# -- Disable Fax Service (cleanup) ---------------------------------------------


def _apply_svc_disable_fax(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disabling Fax service (cleanup)")
    SESSION.backup([_FAX], "SvcFax")
    SESSION.set_dword(_FAX, "Start", 4)
    SESSION.log("Services: Fax service disabled (cleanup)")


def _remove_svc_disable_fax(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_FAX], "SvcFax_Remove")
    SESSION.set_dword(_FAX, "Start", 3)


def _detect_svc_disable_fax() -> bool:
    return SESSION.read_dword(_FAX, "Start") == 4


TWEAKS += [
    TweakDef(
        id="svc-disable-print-spooler",
        label="Disable Print Spooler (Security Hardening)",
        category="Services",
        apply_fn=_apply_svc_disable_print_spooler,
        remove_fn=_remove_svc_disable_print_spooler,
        detect_fn=_detect_svc_disable_print_spooler,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SPOOLER],
        description=("Disables Print Spooler to mitigate PrintNightmare vulnerabilities. Default: Automatic. Recommended: Disabled."),
        tags=["services", "spooler", "security", "printnightmare"],
    ),
    TweakDef(
        id="svc-disable-fax",
        label="Disable Fax Service (Cleanup)",
        category="Services",
        apply_fn=_apply_svc_disable_fax,
        remove_fn=_remove_svc_disable_fax,
        detect_fn=_detect_svc_disable_fax,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_FAX],
        description=("Disables the legacy Fax service to free resources. Default: Manual. Recommended: Disabled."),
        tags=["services", "fax", "legacy", "cleanup"],
    ),
]


# -- Disable Windows Biometric Service (WbioSrvc) -----------------------------


def _apply_svc_disable_wbiosrvc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Windows Biometric Service")
    SESSION.backup([_BIOMETRIC], "SvcBiometric")
    SESSION.set_dword(_BIOMETRIC, "Start", 4)


def _remove_svc_disable_wbiosrvc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BIOMETRIC, "Start", 3)


def _detect_svc_disable_wbiosrvc() -> bool:
    return SESSION.read_dword(_BIOMETRIC, "Start") == 4


# -- Disable Smart Card Service (SCardSvr) -------------------------------------

_SMARTCARD = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr"


def _apply_svc_disable_smartcard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Smart Card service")
    SESSION.backup([_SMARTCARD], "SvcSmartCard")
    SESSION.set_dword(_SMARTCARD, "Start", 4)


def _remove_svc_disable_smartcard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SMARTCARD, "Start", 3)


def _detect_svc_disable_smartcard() -> bool:
    return SESSION.read_dword(_SMARTCARD, "Start") == 4


# -- Disable Geolocation Service (lfsvc) ---------------------------------------


def _apply_svc_disable_geoloc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Geolocation service")
    SESSION.backup([_GEOLOC], "SvcGeolocation")
    SESSION.set_dword(_GEOLOC, "Start", 4)


def _remove_svc_disable_geoloc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_GEOLOC, "Start", 3)


def _detect_svc_disable_geoloc() -> bool:
    return SESSION.read_dword(_GEOLOC, "Start") == 4


TWEAKS += [
    TweakDef(
        id="svc-disable-biometric",
        label="Disable Windows Biometric Service",
        category="Services",
        apply_fn=_apply_svc_disable_wbiosrvc,
        remove_fn=_remove_svc_disable_wbiosrvc,
        detect_fn=_detect_svc_disable_wbiosrvc,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_BIOMETRIC],
        description=(
            "Disables Windows Biometric Service (WbioSrvc) used for fingerprint and face recognition. "
            "Frees resources if biometrics are unused. Default: Manual. Recommended: Disabled."
        ),
        tags=["services", "biometric", "wbiosrvc", "security"],
    ),
    TweakDef(
        id="svc-disable-smartcard",
        label="Disable Smart Card Service",
        category="Services",
        apply_fn=_apply_svc_disable_smartcard,
        remove_fn=_remove_svc_disable_smartcard,
        detect_fn=_detect_svc_disable_smartcard,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SMARTCARD],
        description=(
            "Disables the Smart Card service (SCardSvr) for smart-card readers. "
            "Safe to disable if no smart cards are used. Default: Manual. Recommended: Disabled."
        ),
        tags=["services", "smartcard", "scardsvr", "security"],
    ),
    TweakDef(
        id="svc-disable-geolocation",
        label="Disable Geolocation Service",
        category="Services",
        apply_fn=_apply_svc_disable_geoloc,
        remove_fn=_remove_svc_disable_geoloc,
        detect_fn=_detect_svc_disable_geoloc,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GEOLOC],
        description=(
            "Disables the Geolocation service (lfsvc) that tracks device location. "
            "Improves privacy. Default: Manual. Recommended: Disabled for desktops."
        ),
        tags=["services", "geolocation", "lfsvc", "privacy"],
    ),
]


# ══ Additional Service Tweaks ══════════════════════════════════════════

_CDPSVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CDPSvc"
_TRKWKS = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks"
_WALLET = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService"


def _apply_svc_disable_cdpsvc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Connected Devices Platform Service")
    SESSION.backup([_CDPSVC], "SvcCDPSvc")
    SESSION.set_dword(_CDPSVC, "Start", 4)


def _remove_svc_disable_cdpsvc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CDPSVC, "Start", 2)


def _detect_svc_disable_cdpsvc() -> bool:
    return SESSION.read_dword(_CDPSVC, "Start") == 4


def _apply_svc_disable_link_tracking(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Distributed Link Tracking Client")
    SESSION.backup([_TRKWKS], "SvcTrkWks")
    SESSION.set_dword(_TRKWKS, "Start", 4)


def _remove_svc_disable_link_tracking(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TRKWKS, "Start", 3)


def _detect_svc_disable_link_tracking() -> bool:
    return SESSION.read_dword(_TRKWKS, "Start") == 4


def _apply_svc_disable_wallet(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Services: disable Wallet Service")
    SESSION.backup([_WALLET], "SvcWallet")
    SESSION.set_dword(_WALLET, "Start", 4)


def _remove_svc_disable_wallet(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_WALLET, "Start", 3)


def _detect_svc_disable_wallet() -> bool:
    return SESSION.read_dword(_WALLET, "Start") == 4


TWEAKS += [
    TweakDef(
        id="svc-disable-cdpsvc",
        label="Disable Connected Devices Platform Service",
        category="Services",
        apply_fn=_apply_svc_disable_cdpsvc,
        remove_fn=_remove_svc_disable_cdpsvc,
        detect_fn=_detect_svc_disable_cdpsvc,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CDPSVC],
        description=(
            "Disables the Connected Devices Platform Service (CDPSvc) used for "
            "cross-device experiences. Frees resources if unused. "
            "Default: Automatic. Recommended: Disabled."
        ),
        tags=["services", "cdpsvc", "cross-device", "platform"],
    ),
    TweakDef(
        id="svc-disable-link-tracking",
        label="Disable Distributed Link Tracking Client",
        category="Services",
        apply_fn=_apply_svc_disable_link_tracking,
        remove_fn=_remove_svc_disable_link_tracking,
        detect_fn=_detect_svc_disable_link_tracking,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TRKWKS],
        description=(
            "Disables the Distributed Link Tracking Client (TrkWks) that "
            "maintains NTFS file links across networked computers. "
            "Default: Manual. Recommended: Disabled for standalone PCs."
        ),
        tags=["services", "link-tracking", "trkwks", "ntfs"],
    ),
    TweakDef(
        id="svc-disable-wallet",
        label="Disable Wallet Service",
        category="Services",
        apply_fn=_apply_svc_disable_wallet,
        remove_fn=_remove_svc_disable_wallet,
        detect_fn=_detect_svc_disable_wallet,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WALLET],
        description=(
            "Disables the Windows Wallet Service used for NFC-based payments. "
            "Safe to disable if contactless payments are unused. "
            "Default: Manual. Recommended: Disabled."
        ),
        tags=["services", "wallet", "nfc", "payment"],
    ),
]
