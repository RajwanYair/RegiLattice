"""Printing tweaks — spooler, drivers, defaults, and optimization.

Covers: print spooler performance, driver isolation, default printer,
Point-and-Print restrictions, and PDF/XPS printer management.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_SPOOLER = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"
_PRINTERS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"
_POINTPRINT = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT"
    r"\Printers\PointAndPrint"
)
_PRINTING_CU = r"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Windows"
_XPS_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\XPS"
_PROVIDERS = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers"


# ── Disable Print Spooler Auto-Start ────────────────────────────────────────


def _apply_disable_spooler_autostart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Printing: configure print spooler to manual start")
    _svc = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"
    SESSION.backup([_svc], "PrintSpooler")
    SESSION.set_dword(_svc, "Start", 3)  # 3 = Manual


def _remove_disable_spooler_autostart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    _svc = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"
    SESSION.set_dword(_svc, "Start", 2)  # 2 = Automatic


def _detect_disable_spooler_autostart() -> bool:
    _svc = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"
    return SESSION.read_dword(_svc, "Start") == 3


# ── Enable Print Driver Isolation ────────────────────────────────────────────


def _apply_driver_isolation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Printing: enable print driver isolation for stability")
    SESSION.backup([_SPOOLER], "DriverIsolation")
    SESSION.set_dword(_SPOOLER, "PrintDriverIsolationGroups", 1)
    SESSION.set_dword(_SPOOLER, "PrintDriverIsolationOverrideCompat", 1)


def _remove_driver_isolation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SPOOLER, "PrintDriverIsolationGroups")
    SESSION.delete_value(_SPOOLER, "PrintDriverIsolationOverrideCompat")


def _detect_driver_isolation() -> bool:
    return SESSION.read_dword(_SPOOLER, "PrintDriverIsolationGroups") == 1


# ── Disable Point-and-Print Restrictions ─────────────────────────────────────


def _apply_disable_pointandprint(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Printing: disable Point-and-Print driver install restrictions")
    SESSION.backup([_POINTPRINT], "PointAndPrint")
    SESSION.set_dword(_POINTPRINT, "RestrictDriverInstallationToAdministrators", 1)
    SESSION.set_dword(_POINTPRINT, "NoWarningNoElevationOnInstall", 0)


def _remove_disable_pointandprint(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_POINTPRINT, "RestrictDriverInstallationToAdministrators")
    SESSION.delete_value(_POINTPRINT, "NoWarningNoElevationOnInstall")


def _detect_disable_pointandprint() -> bool:
    return SESSION.read_dword(_POINTPRINT, "RestrictDriverInstallationToAdministrators") == 1


# ── Disable Windows Default Printer Management ──────────────────────────────


def _apply_disable_default_printer_mgmt(*, require_admin: bool = False) -> None:
    SESSION.log("Printing: disable Windows managing the default printer")
    _key = r"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Windows"
    SESSION.backup([_key], "DefaultPrinterMgmt")
    SESSION.set_dword(_key, "LegacyDefaultPrinterMode", 1)


def _remove_disable_default_printer_mgmt(*, require_admin: bool = False) -> None:
    _key = r"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Windows"
    SESSION.set_dword(_key, "LegacyDefaultPrinterMode", 0)


def _detect_disable_default_printer_mgmt() -> bool:
    _key = r"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Windows"
    return SESSION.read_dword(_key, "LegacyDefaultPrinterMode") == 1


# ── Disable Microsoft XPS Document Writer ────────────────────────────────────


def _apply_disable_xps_writer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Printing: disable Microsoft XPS Document Writer")
    SESSION.backup([_PRINTERS], "XPSWriter")
    SESSION.set_dword(_PRINTERS, "DisableXPSDocumentWriter", 1)


def _remove_disable_xps_writer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PRINTERS, "DisableXPSDocumentWriter")


def _detect_disable_xps_writer() -> bool:
    return SESSION.read_dword(_PRINTERS, "DisableXPSDocumentWriter") == 1


# ── Disable Network Printer Publishing ───────────────────────────────────────


def _apply_disable_printer_publishing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Printing: disable network printer publishing in AD")
    SESSION.backup([_PRINTERS], "PrinterPublishing")
    SESSION.set_dword(_PRINTERS, "DisableServerPrinterPublishing", 1)


def _remove_disable_printer_publishing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PRINTERS, "DisableServerPrinterPublishing")


def _detect_disable_printer_publishing() -> bool:
    return SESSION.read_dword(_PRINTERS, "DisableServerPrinterPublishing") == 1


# ── Disable Internet Printing (IPP) ─────────────────────────────────────────


def _apply_disable_internet_printing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Printing: disable Internet Printing Client (IPP)")
    SESSION.backup([_PRINTERS], "InternetPrinting")
    SESSION.set_dword(_PRINTERS, "DisableHTTPPrinting", 1)
    SESSION.set_dword(_PRINTERS, "DisableWebPnPDownload", 1)


def _remove_disable_internet_printing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PRINTERS, "DisableHTTPPrinting")
    SESSION.delete_value(_PRINTERS, "DisableWebPnPDownload")


def _detect_disable_internet_printing() -> bool:
    return SESSION.read_dword(_PRINTERS, "DisableHTTPPrinting") == 1


# ── Disable Print Queue Logging ──────────────────────────────────────────────


def _apply_disable_queue_logging(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Printing: disable print queue event logging")
    SESSION.backup([_PROVIDERS], "PrintQueueLogging")
    SESSION.set_dword(_PROVIDERS, "EventLog", 0)


def _remove_disable_queue_logging(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PROVIDERS, "EventLog", 1)


def _detect_disable_queue_logging() -> bool:
    return SESSION.read_dword(_PROVIDERS, "EventLog") == 0


# ── Limit Print Spooler Memory ───────────────────────────────────────────────


def _apply_limit_spooler_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Printing: set print spooler to normal priority")
    SESSION.backup([_SPOOLER], "SpoolerPriority")
    SESSION.set_dword(_SPOOLER, "SpoolerPriority", 0)


def _remove_limit_spooler_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SPOOLER, "SpoolerPriority")


def _detect_limit_spooler_memory() -> bool:
    return SESSION.read_dword(_SPOOLER, "SpoolerPriority") == 0


# ── Disable Remote Printing ──────────────────────────────────────────────────


def _apply_disable_remote(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Printing: disable remote printing connections")
    SESSION.backup([_PRINTERS], "RemotePrinting")
    SESSION.set_dword(_PRINTERS, "DisableRemotePrinting", 1)


def _remove_disable_remote(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PRINTERS, "DisableRemotePrinting")


def _detect_disable_remote() -> bool:
    return SESSION.read_dword(_PRINTERS, "DisableRemotePrinting") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="printing-disable-spooler-autostart",
        label="Set Print Spooler to Manual Start",
        category="Printing",
        apply_fn=_apply_disable_spooler_autostart,
        remove_fn=_remove_disable_spooler_autostart,
        detect_fn=_detect_disable_spooler_autostart,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"],
        description=(
            "Sets the Print Spooler service to Manual start. Reduces attack "
            "surface (PrintNightmare) and improves boot time if no printer is used."
        ),
        tags=["printing", "spooler", "security", "performance"],
    ),
    TweakDef(
        id="printing-driver-isolation",
        label="Enable Print Driver Isolation",
        category="Printing",
        apply_fn=_apply_driver_isolation,
        remove_fn=_remove_driver_isolation,
        detect_fn=_detect_driver_isolation,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SPOOLER],
        description=(
            "Runs third-party print drivers in an isolated process. "
            "Prevents a buggy driver from crashing the spooler service."
        ),
        tags=["printing", "driver", "stability", "isolation"],
    ),
    TweakDef(
        id="printing-disable-pointandprint",
        label="Restrict Point-and-Print Drivers",
        category="Printing",
        apply_fn=_apply_disable_pointandprint,
        remove_fn=_remove_disable_pointandprint,
        detect_fn=_detect_disable_pointandprint,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_POINTPRINT],
        description=(
            "Requires admin approval for Point-and-Print driver installs. "
            "Mitigates PrintNightmare and similar spooler vulnerabilities."
        ),
        tags=["printing", "security", "pointandprint", "policy"],
    ),
    TweakDef(
        id="printing-disable-default-mgmt",
        label="Disable Windows Default Printer Management",
        category="Printing",
        apply_fn=_apply_disable_default_printer_mgmt,
        remove_fn=_remove_disable_default_printer_mgmt,
        detect_fn=_detect_disable_default_printer_mgmt,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PRINTING_CU],
        description=(
            "Stops Windows from automatically setting the default printer "
            "to the last one used. Keeps your chosen default printer fixed."
        ),
        tags=["printing", "default", "management"],
    ),
    TweakDef(
        id="printing-disable-xps-writer",
        label="Disable XPS Document Writer",
        category="Printing",
        apply_fn=_apply_disable_xps_writer,
        remove_fn=_remove_disable_xps_writer,
        detect_fn=_detect_disable_xps_writer,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PRINTERS],
        description=(
            "Removes the Microsoft XPS Document Writer virtual printer "
            "from the printers list. Reduces clutter if you never use XPS."
        ),
        tags=["printing", "xps", "cleanup"],
    ),
    TweakDef(
        id="printing-disable-publishing",
        label="Disable Network Printer Publishing",
        category="Printing",
        apply_fn=_apply_disable_printer_publishing,
        remove_fn=_remove_disable_printer_publishing,
        detect_fn=_detect_disable_printer_publishing,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PRINTERS],
        description=(
            "Prevents shared printers from being published in Active Directory. "
            "Reduces AD clutter on enterprise networks."
        ),
        tags=["printing", "network", "activedirectory", "enterprise"],
    ),
    TweakDef(
        id="printing-disable-internet-printing",
        label="Disable Internet Printing (IPP)",
        category="Printing",
        apply_fn=_apply_disable_internet_printing,
        remove_fn=_remove_disable_internet_printing,
        detect_fn=_detect_disable_internet_printing,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PRINTERS],
        description=(
            "Disables Internet Printing Protocol and Web PnP driver downloads. "
            "Reduces attack surface from internet-facing print services."
        ),
        tags=["printing", "internet", "security", "ipp"],
    ),
    TweakDef(
        id="printing-disable-queue-logging",
        label="Disable Print Queue Logging",
        category="Printing",
        apply_fn=_apply_disable_queue_logging,
        remove_fn=_remove_disable_queue_logging,
        detect_fn=_detect_disable_queue_logging,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PROVIDERS],
        description=(
            "Disables event logging for print queue events. Reduces disk I/O "
            "from print job tracking. Default: 1 (Enabled). Recommended: 0 (Disabled)."
        ),
        tags=["printing", "logging", "performance"],
    ),
    TweakDef(
        id="printing-limit-spooler-memory",
        label="Limit Print Spooler Memory",
        category="Printing",
        apply_fn=_apply_limit_spooler_memory,
        remove_fn=_remove_limit_spooler_memory,
        detect_fn=_detect_limit_spooler_memory,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SPOOLER],
        description=(
            "Sets Print Spooler to normal priority. Prevents spooler from consuming "
            "excessive CPU during large print jobs. Default: Above Normal. Recommended: Normal."
        ),
        tags=["printing", "memory", "performance", "priority"],
    ),
    TweakDef(
        id="printing-disable-remote",
        label="Disable Remote Printing",
        category="Printing",
        apply_fn=_apply_disable_remote,
        remove_fn=_remove_disable_remote,
        detect_fn=_detect_disable_remote,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PRINTERS],
        description=(
            "Blocks incoming remote print connections. Reduces attack surface by preventing "
            "remote users from sending print jobs to this machine. Default: Enabled. "
            "Recommended: Disabled for security."
        ),
        tags=["printing", "remote", "security"],
    ),
]


_SPOOLER_SVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"


# -- Disable Print Spooler Service ------------------------------------------------


def _apply_disable_print_spooler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Printing: disable Print Spooler service")
    SESSION.backup([_SPOOLER_SVC], "PrintSpooler")
    SESSION.set_dword(_SPOOLER_SVC, "Start", 4)


def _remove_disable_print_spooler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SPOOLER_SVC, "Start", 2)


def _detect_disable_print_spooler() -> bool:
    return SESSION.read_dword(_SPOOLER_SVC, "Start") == 4


# -- Restrict Printer Driver Installation -----------------------------------------


def _apply_restrict_driver_install(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Printing: restrict printer driver installation to admins")
    SESSION.backup([_PRINTERS], "RestrictDriverInstall")
    SESSION.set_dword(_PRINTERS, "RestrictDriverInstallationToAdministrators", 1)


def _remove_restrict_driver_install(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PRINTERS, "RestrictDriverInstallationToAdministrators")


def _detect_restrict_driver_install() -> bool:
    return SESSION.read_dword(_PRINTERS, "RestrictDriverInstallationToAdministrators") == 1


TWEAKS += [
    TweakDef(
        id="printing-disable-print-spooler",
        label="Disable Print Spooler Service",
        category="Printing",
        apply_fn=_apply_disable_print_spooler,
        remove_fn=_remove_disable_print_spooler,
        detect_fn=_detect_disable_print_spooler,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SPOOLER_SVC],
        description=(
            "Disables the Print Spooler service entirely (Start=4). "
            "Reduces attack surface on machines that do not use printers. "
            "Default: Automatic (2). Recommended: Disabled (4) if no printing needed."
        ),
        tags=["printing", "spooler", "service", "security"],
    ),
    TweakDef(
        id="printing-restrict-driver-install",
        label="Restrict Printer Driver Installation",
        category="Printing",
        apply_fn=_apply_restrict_driver_install,
        remove_fn=_remove_restrict_driver_install,
        detect_fn=_detect_restrict_driver_install,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_PRINTERS],
        description=(
            "Restricts printer driver installation to administrators only. "
            "Mitigates PrintNightmare-class vulnerabilities. "
            "Default: not restricted. Recommended: restricted."
        ),
        tags=["printing", "driver", "security", "restriction"],
    ),
]
