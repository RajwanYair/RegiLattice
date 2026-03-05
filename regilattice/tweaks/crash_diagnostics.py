"""Crash & Diagnostics tweaks.

Covers crash dumps, error reporting, WER, diagnostic data, memory dumps,
BSOD auto-restart, and system diagnostic policies.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_CRASH = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"
_WER = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"
_WER_CU = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\Windows Error Reporting"
_WER_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"
_DIAG = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy"
_DIAG_SVC = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics"
_RECOVERY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"
_DUMP_PATH = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"
_PERF_TRACK = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}"
_APP_COMPAT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"

# ── 1. Disable automatic restart on BSOD ─────────────────────────────────────


def _apply_disable_auto_restart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH, "AutoReboot", 0)


def _remove_disable_auto_restart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH, "AutoReboot", 1)


def _detect_disable_auto_restart() -> bool:
    return SESSION.read_dword(_CRASH, "AutoReboot") == 0


# ── 2. Set crash dump to small (minidump) ────────────────────────────────────


def _apply_minidump(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH, "CrashDumpEnabled", 3)


def _remove_minidump(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH, "CrashDumpEnabled", 7)


def _detect_minidump() -> bool:
    return SESSION.read_dword(_CRASH, "CrashDumpEnabled") == 3


# ── 3. Disable crash dump entirely ───────────────────────────────────────────


def _apply_disable_crash_dump(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH, "CrashDumpEnabled", 0)


def _remove_disable_crash_dump(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH, "CrashDumpEnabled", 7)


def _detect_disable_crash_dump() -> bool:
    return SESSION.read_dword(_CRASH, "CrashDumpEnabled") == 0


# ── 4. Disable Windows Error Reporting ───────────────────────────────────────


def _apply_disable_wer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_WER, "Disabled", 1)


def _remove_disable_wer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_WER, "Disabled", 0)


def _detect_disable_wer() -> bool:
    return SESSION.read_dword(_WER, "Disabled") == 1


# ── 5. Disable WER (policy) ──────────────────────────────────────────────────


def _apply_disable_wer_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_WER_POLICY, "Disabled", 1)


def _remove_disable_wer_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WER_POLICY, "Disabled")


def _detect_disable_wer_policy() -> bool:
    return SESSION.read_dword(_WER_POLICY, "Disabled") == 1


# ── 6. Disable WER user-mode crash reporting ─────────────────────────────────


def _apply_disable_wer_user(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_WER_CU, "Disabled", 1)


def _remove_disable_wer_user(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_WER_CU, "Disabled", 0)


def _detect_disable_wer_user() -> bool:
    return SESSION.read_dword(_WER_CU, "Disabled") == 1


# ── 7. Disable automatic memory dump overwrite ───────────────────────────────


def _apply_disable_dump_overwrite(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH, "Overwrite", 0)


def _remove_disable_dump_overwrite(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH, "Overwrite", 1)


def _detect_disable_dump_overwrite() -> bool:
    return SESSION.read_dword(_CRASH, "Overwrite") == 0


# ── 8. Disable scripted diagnostics ──────────────────────────────────────────


def _apply_disable_scripted_diag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_DIAG, "EnableQueryRemoteServer", 0)
    SESSION.set_dword(_DIAG_SVC, "EnableDiagnostics", 0)


def _remove_disable_scripted_diag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DIAG, "EnableQueryRemoteServer")
    SESSION.delete_value(_DIAG_SVC, "EnableDiagnostics")


def _detect_disable_scripted_diag() -> bool:
    return SESSION.read_dword(_DIAG_SVC, "EnableDiagnostics") == 0


# ── 9. Disable performance tracking (WDI) ────────────────────────────────────


def _apply_disable_perf_track(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PERF_TRACK, "ScenarioExecutionEnabled", 0)


def _remove_disable_perf_track(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PERF_TRACK, "ScenarioExecutionEnabled")


def _detect_disable_perf_track() -> bool:
    return SESSION.read_dword(_PERF_TRACK, "ScenarioExecutionEnabled") == 0


# ── 10. Disable application compatibility engine ─────────────────────────────


def _apply_disable_app_compat(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_APP_COMPAT, "DisableEngine", 1)


def _remove_disable_app_compat(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_APP_COMPAT, "DisableEngine")


def _detect_disable_app_compat() -> bool:
    return SESSION.read_dword(_APP_COMPAT, "DisableEngine") == 1


# ── 11. Disable Program Compatibility Assistant ──────────────────────────────


def _apply_disable_pca(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_APP_COMPAT, "DisablePCA", 1)


def _remove_disable_pca(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_APP_COMPAT, "DisablePCA")


def _detect_disable_pca() -> bool:
    return SESSION.read_dword(_APP_COMPAT, "DisablePCA") == 1


# ── TWEAKS list ──────────────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="crash-disable-auto-restart",
        label="Disable Auto-Restart on BSOD",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_auto_restart,
        remove_fn=_remove_disable_auto_restart,
        detect_fn=_detect_disable_auto_restart,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CRASH],
        description="Stay on BSOD screen instead of auto-rebooting. Helps read stop codes. Default: auto-restart. Recommended: disabled.",
        tags=["bsod", "crash", "restart", "blue-screen"],
    ),
    TweakDef(
        id="crash-set-minidump",
        label="Set Crash Dump to Minidump",
        category="Crash & Diagnostics",
        apply_fn=_apply_minidump,
        remove_fn=_remove_minidump,
        detect_fn=_detect_minidump,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CRASH],
        description="Save only small minidumps on crash (saves disk). Default: automatic memory dump.",
        tags=["crash", "dump", "minidump", "disk"],
    ),
    TweakDef(
        id="crash-disable-crash-dump",
        label="Disable Crash Dump Entirely",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_crash_dump,
        remove_fn=_remove_disable_crash_dump,
        detect_fn=_detect_disable_crash_dump,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CRASH],
        description="Do not write any crash dump files. Saves disk but loses BSOD data. Default: automatic dump.",
        tags=["crash", "dump", "disable", "disk"],
    ),
    TweakDef(
        id="crash-disable-wer",
        label="Disable Windows Error Reporting",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_wer,
        remove_fn=_remove_disable_wer,
        detect_fn=_detect_disable_wer,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WER],
        description="Disable WER service from sending error reports to Microsoft. Default: enabled. Recommended: disabled for privacy.",
        tags=["wer", "error", "reporting", "privacy"],
    ),
    TweakDef(
        id="crash-disable-wer-policy",
        label="Disable WER (Policy)",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_wer_policy,
        remove_fn=_remove_disable_wer_policy,
        detect_fn=_detect_disable_wer_policy,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WER_POLICY],
        description="Machine-wide policy to disable Windows Error Reporting. Default: not set.",
        tags=["wer", "error", "policy", "privacy"],
    ),
    TweakDef(
        id="crash-disable-wer-user",
        label="Disable WER (User Level)",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_wer_user,
        remove_fn=_remove_disable_wer_user,
        detect_fn=_detect_disable_wer_user,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_WER_CU],
        description="Disable error reporting for current user only. Default: enabled.",
        tags=["wer", "error", "user", "privacy"],
    ),
    TweakDef(
        id="crash-disable-dump-overwrite",
        label="Disable Crash Dump Overwrite",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_dump_overwrite,
        remove_fn=_remove_disable_dump_overwrite,
        detect_fn=_detect_disable_dump_overwrite,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CRASH],
        description="Keep all crash dumps instead of overwriting with latest. Default: overwrite.",
        tags=["crash", "dump", "overwrite", "history"],
    ),
    TweakDef(
        id="crash-disable-scripted-diagnostics",
        label="Disable Scripted Diagnostics",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_scripted_diag,
        remove_fn=_remove_disable_scripted_diag,
        detect_fn=_detect_disable_scripted_diag,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DIAG, _DIAG_SVC],
        description="Disable automatic troubleshooters and scripted diagnostics. Default: enabled.",
        tags=["diagnostics", "troubleshooter", "script"],
    ),
    TweakDef(
        id="crash-disable-perf-tracking",
        label="Disable Performance Tracking (WDI)",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_perf_track,
        remove_fn=_remove_disable_perf_track,
        detect_fn=_detect_disable_perf_track,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_PERF_TRACK],
        description="Disable Windows Diagnostic Infrastructure performance tracking. Default: enabled.",
        tags=["wdi", "performance", "tracking", "diagnostics"],
    ),
    TweakDef(
        id="crash-disable-app-compat-engine",
        label="Disable App Compatibility Engine",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_app_compat,
        remove_fn=_remove_disable_app_compat,
        detect_fn=_detect_disable_app_compat,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_APP_COMPAT],
        description="Disable the application compatibility engine. Slight performance gain. Default: enabled.",
        tags=["compatibility", "app-compat", "engine", "performance"],
    ),
    TweakDef(
        id="crash-disable-pca",
        label="Disable Program Compatibility Assistant",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_pca,
        remove_fn=_remove_disable_pca,
        detect_fn=_detect_disable_pca,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_APP_COMPAT],
        description="Disable PCA popup suggesting compatibility settings. Default: enabled. Recommended: disabled if not needed.",
        tags=["pca", "compatibility", "assistant", "popup"],
    ),
]
