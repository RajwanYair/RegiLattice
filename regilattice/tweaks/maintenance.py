"""Maintenance tweaks — Registry Auto-Backup, Restore Point."""

from __future__ import annotations

import subprocess
from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Registry Auto-Backup ────────────────────────────────────────────────────

_REGBACK_KEY = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Configuration Manager"
)


def apply_regbackup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-RegistryBackup")
    SESSION.backup([_REGBACK_KEY], "RegAutoBackup")
    SESSION.set_dword(_REGBACK_KEY, "EnablePeriodicBackup", 1)
    SESSION.log("Completed Add-RegistryBackup")


def remove_regbackup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-RegistryBackup")
    SESSION.backup([_REGBACK_KEY], "RegAutoBackup_Remove")
    SESSION.set_dword(_REGBACK_KEY, "EnablePeriodicBackup", 0)
    SESSION.log("Completed Remove-RegistryBackup")


def detect_regbackup() -> bool:
    return SESSION.read_dword(_REGBACK_KEY, "EnablePeriodicBackup") == 1


# ── Disable Scheduled Defragmentation ──────────────────────────────────────

_DEFRAG_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction"
)


def _apply_disable_defrag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable scheduled defragmentation")
    SESSION.backup([_DEFRAG_KEY], "Defrag")
    SESSION.set_string(_DEFRAG_KEY, "Enable", "N")


def _remove_disable_defrag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_DEFRAG_KEY, "Enable", "Y")


def _detect_disable_defrag() -> bool:
    return SESSION.read_string(_DEFRAG_KEY, "Enable") == "N"


# ── Disable Windows Error Dumps ───────────────────────────────────────────

_CRASH_KEY = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"
)


def _apply_disable_dumps(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable crash memory dumps")
    SESSION.backup([_CRASH_KEY], "CrashDumps")
    SESSION.set_dword(_CRASH_KEY, "CrashDumpEnabled", 0)  # 0 = None
    SESSION.set_dword(_CRASH_KEY, "LogEvent", 0)


def _remove_disable_dumps(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH_KEY, "CrashDumpEnabled", 7)  # 7 = Automatic
    SESSION.set_dword(_CRASH_KEY, "LogEvent", 1)


def _detect_disable_dumps() -> bool:
    return SESSION.read_dword(_CRASH_KEY, "CrashDumpEnabled") == 0


# ── System Restore Point ────────────────────────────────────────────────────


def create_restore_point(*, require_admin: bool = True) -> None:
    """Create a Windows System Restore checkpoint via PowerShell."""
    assert_admin(require_admin)
    SESSION.log("Creating system restore point")
    cmd = (
        'powershell -NoProfile -Command "'
        "Checkpoint-Computer -Description 'RegiLattice' "
        "-RestorePointType 'MODIFY_SETTINGS'"
        '"'
    )
    subprocess.run(cmd, shell=True, check=False)
    SESSION.log("Restore point created")


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="registry-autobackup",
        label="Enable Registry Auto-Backup",
        category="Maintenance",
        apply_fn=apply_regbackup,
        remove_fn=remove_regbackup,
        detect_fn=detect_regbackup,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_REGBACK_KEY],
        description=(
            "Enables Windows nightly registry hive backup to "
            r"C:\Windows\System32\config\RegBack."
        ),
        tags=["maintenance", "backup", "registry"],
    ),
    TweakDef(
        id="disable-defrag-schedule",
        label="Disable Scheduled Defragmentation",
        category="Maintenance",
        apply_fn=_apply_disable_defrag,
        remove_fn=_remove_disable_defrag,
        detect_fn=_detect_disable_defrag,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DEFRAG_KEY],
        description=(
            "Disables the Windows automatic disk defragmentation schedule. "
            "Recommended for SSD-only systems."
        ),
        tags=["maintenance", "disk", "defrag", "ssd"],
    ),
    TweakDef(
        id="disable-crash-dumps",
        label="Disable Crash Memory Dumps",
        category="Maintenance",
        apply_fn=_apply_disable_dumps,
        remove_fn=_remove_disable_dumps,
        detect_fn=_detect_disable_dumps,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CRASH_KEY],
        description=(
            "Disables crash memory dump files to save disk space "
            "and avoid large MEMORY.DMP writes."
        ),
        tags=["maintenance", "disk", "cleanup", "crash"],
    ),
]
