"""Performance tweaks — startup delay, SvcHost split, NTFS last access."""

from __future__ import annotations

import subprocess
from typing import List

from turbotweak.registry import SESSION, assert_admin
from turbotweak.tweaks import TweakDef

# ── Performance (startup / responsiveness / network) ─────────────────────────

_PERF_SERIALIZE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Serialize"
)
_PERF_MULTIMEDIA = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Multimedia\SystemProfile"
)
_PERF_KEYS = [_PERF_SERIALIZE, _PERF_MULTIMEDIA]


def apply_performance(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-Performance")
    SESSION.backup(_PERF_KEYS, "Performance")
    SESSION.set_dword(_PERF_SERIALIZE, "StartupDelayInMSec", 0)
    SESSION.set_dword(_PERF_SERIALIZE, "WaitforIdleState", 0)
    SESSION.set_dword(_PERF_MULTIMEDIA, "SystemResponsiveness", 10)
    SESSION.set_dword(_PERF_MULTIMEDIA, "NetworkThrottlingIndex", 0xFFFFFFFF)
    SESSION.log("Completed Add-Performance")


def remove_performance(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-Performance")
    SESSION.backup(_PERF_KEYS, "Performance_Remove")
    SESSION.delete_value(_PERF_SERIALIZE, "StartupDelayInMSec")
    SESSION.delete_value(_PERF_SERIALIZE, "WaitforIdleState")
    SESSION.set_dword(_PERF_MULTIMEDIA, "SystemResponsiveness", 20)
    SESSION.set_dword(_PERF_MULTIMEDIA, "NetworkThrottlingIndex", 10)
    SESSION.log("Completed Remove-Performance")


def detect_performance() -> bool:
    return SESSION.read_dword(_PERF_SERIALIZE, "StartupDelayInMSec") == 0


# ── SvcHost Split Threshold ─────────────────────────────────────────────────

_SVCHOST_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"


def apply_svchost_split(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-SvcHostSplit")
    SESSION.backup([_SVCHOST_KEY], "SvcHostSplit")
    import ctypes

    class MEMORYSTATUSEX(ctypes.Structure):
        _fields_ = [
            ("dwLength", ctypes.c_ulong),
            ("dwMemoryLoad", ctypes.c_ulong),
            ("ullTotalPhys", ctypes.c_ulonglong),
            ("ullAvailPhys", ctypes.c_ulonglong),
            ("ullTotalPageFile", ctypes.c_ulonglong),
            ("ullAvailPageFile", ctypes.c_ulonglong),
            ("ullTotalVirtual", ctypes.c_ulonglong),
            ("ullAvailVirtual", ctypes.c_ulonglong),
            ("ullAvailExtendedVirtual", ctypes.c_ulonglong),
        ]

    mem = MEMORYSTATUSEX()
    mem.dwLength = ctypes.sizeof(MEMORYSTATUSEX)
    ctypes.windll.kernel32.GlobalMemoryStatusEx(  # type: ignore[attr-defined]
        ctypes.byref(mem)
    )
    ram_kb = int(mem.ullTotalPhys / 1024)
    SESSION.set_dword(_SVCHOST_KEY, "SvcHostSplitThresholdInKB", ram_kb)
    SESSION.log(f"Completed Add-SvcHostSplit: {ram_kb} KB")


def remove_svchost_split(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-SvcHostSplit")
    SESSION.backup([_SVCHOST_KEY], "SvcHostSplit_Remove")
    SESSION.set_dword(_SVCHOST_KEY, "SvcHostSplitThresholdInKB", 380000)
    SESSION.log("Completed Remove-SvcHostSplit")


def detect_svchost_split() -> bool:
    val = SESSION.read_dword(_SVCHOST_KEY, "SvcHostSplitThresholdInKB")
    return val is not None and val > 380000


# ── NTFS Last Access Timestamps ─────────────────────────────────────────────


def apply_disable_last_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableLastAccess")
    subprocess.run(
        ["fsutil", "behavior", "set", "disablelastaccess", "1"],
        check=True,
        capture_output=True,
        text=True,
    )
    SESSION.log("Completed Add-DisableLastAccess")


def remove_disable_last_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableLastAccess")
    subprocess.run(
        ["fsutil", "behavior", "set", "disablelastaccess", "2"],
        check=True,
        capture_output=True,
        text=True,
    )
    SESSION.log("Completed Remove-DisableLastAccess")


def detect_disable_last_access() -> bool:
    try:
        r = subprocess.run(
            ["fsutil", "behavior", "query", "disablelastaccess"],
            capture_output=True,
            text=True,
            timeout=5,
        )
        # Output like "DisableLastAccess = 1"
        return "= 1" in r.stdout
    except Exception:
        return False


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="performance",
        label="Performance Tweaks (Visual Effects)",
        category="Performance",
        apply_fn=apply_performance,
        remove_fn=remove_performance,
        detect_fn=detect_performance,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_PERF_KEYS,
    ),
    TweakDef(
        id="svchost-split",
        label="Optimize SvcHost Split (RAM-based)",
        category="Performance",
        apply_fn=apply_svchost_split,
        remove_fn=remove_svchost_split,
        detect_fn=detect_svchost_split,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SVCHOST_KEY],
    ),
    TweakDef(
        id="disable-last-access",
        label="Disable NTFS Last Access Timestamp",
        category="Performance",
        apply_fn=apply_disable_last_access,
        remove_fn=remove_disable_last_access,
        detect_fn=detect_disable_last_access,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[],
    ),
]
