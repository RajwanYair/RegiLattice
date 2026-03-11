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


_SIUF = r"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"
_WIN_CTRL = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Windows"


# -- Disable Feedback Notifications -----------------------------------------------


def _apply_disable_feedback_notifications(*, require_admin: bool = True) -> None:
    SESSION.log("Crash & Diagnostics: disable feedback request notifications")
    SESSION.backup([_SIUF], "FeedbackNotifications")
    SESSION.set_dword(_SIUF, "NumberOfSIUFInPeriod", 0)


def _remove_disable_feedback_notifications(*, require_admin: bool = True) -> None:
    SESSION.delete_value(_SIUF, "NumberOfSIUFInPeriod")


def _detect_disable_feedback_notifications() -> bool:
    return SESSION.read_dword(_SIUF, "NumberOfSIUFInPeriod") == 0


# -- Disable Error Dialog Boxes ---------------------------------------------------


def _apply_disable_error_dialog(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Crash & Diagnostics: disable automatic error dialog boxes")
    SESSION.backup([_WIN_CTRL], "ErrorDialog")
    SESSION.set_dword(_WIN_CTRL, "ErrorMode", 2)


def _remove_disable_error_dialog(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_WIN_CTRL, "ErrorMode", 0)


def _detect_disable_error_dialog() -> bool:
    return SESSION.read_dword(_WIN_CTRL, "ErrorMode") == 2


TWEAKS += [
    TweakDef(
        id="crash-disable-feedback-notifications",
        label="Disable Feedback Request Notifications",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_feedback_notifications,
        remove_fn=_remove_disable_feedback_notifications,
        detect_fn=_detect_disable_feedback_notifications,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SIUF],
        description=(
            "Disables Windows feedback request notification prompts. "
            "Sets NumberOfSIUFInPeriod to 0 to suppress all feedback popups. "
            "Default: periodic. Recommended: disabled."
        ),
        tags=["crash", "feedback", "notifications", "privacy"],
    ),
    TweakDef(
        id="crash-disable-error-dialog",
        label="Disable Automatic Error Dialog Boxes",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_error_dialog,
        remove_fn=_remove_disable_error_dialog,
        detect_fn=_detect_disable_error_dialog,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WIN_CTRL],
        description=(
            "Disables automatic error dialog boxes (ErrorMode=2). "
            "Suppresses critical-error-handler message boxes for background services. "
            "Default: 0 (show all). Recommended: 2 for servers."
        ),
        tags=["crash", "error", "dialog", "server"],
    ),
]


# ── Enable Full Memory Dumps ─────────────────────────────────────────────────


def _apply_full_memory_dump(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Crash: set CrashDumpEnabled to full memory dump (1)")
    SESSION.backup([_CRASH], "FullMemoryDump")
    SESSION.set_dword(_CRASH, "CrashDumpEnabled", 1)


def _remove_full_memory_dump(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH, "CrashDumpEnabled", 7)  # 7 = Automatic (default)


def _detect_full_memory_dump() -> bool:
    return SESSION.read_dword(_CRASH, "CrashDumpEnabled") == 1


# ── Disable WER Queue Reporting ──────────────────────────────────────────────


def _apply_disable_wer_queue(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Crash: disable WER queue-based reporting")
    SESSION.backup([_WER], "WerQueueOff")
    SESSION.set_dword(_WER, "DisableQueue", 1)


def _remove_disable_wer_queue(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WER, "DisableQueue")


def _detect_disable_wer_queue() -> bool:
    return SESSION.read_dword(_WER, "DisableQueue") == 1


# ── Set Crash Dump to Kernel Only ────────────────────────────────────────────


def _apply_kernel_dump_only(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Crash: set crash dump to kernel-only mode (2)")
    SESSION.backup([_CRASH], "KernelDumpOnly")
    SESSION.set_dword(_CRASH, "CrashDumpEnabled", 2)


def _remove_kernel_dump_only(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH, "CrashDumpEnabled", 7)  # 7 = Automatic (default)


def _detect_kernel_dump_only() -> bool:
    return SESSION.read_dword(_CRASH, "CrashDumpEnabled") == 2


TWEAKS += [
    TweakDef(
        id="crash-enable-full-memory-dump",
        label="Enable Full Memory Dumps",
        category="Crash & Diagnostics",
        apply_fn=_apply_full_memory_dump,
        remove_fn=_remove_full_memory_dump,
        detect_fn=_detect_full_memory_dump,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CRASH],
        description=(
            "Sets CrashDumpEnabled to 1 (Complete memory dump) for "
            "full debugging information on BSOD. Requires sufficient page file. "
            "Default: 7 (Automatic). Recommended: 1 for debugging."
        ),
        tags=["crash", "dump", "memory", "debugging", "bsod"],
    ),
    TweakDef(
        id="crash-disable-wer-queue",
        label="Disable WER Queue Reporting",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_wer_queue,
        remove_fn=_remove_disable_wer_queue,
        detect_fn=_detect_disable_wer_queue,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WER],
        description=(
            "Disables Windows Error Reporting queue-based report collection. "
            "Stops WER from queuing reports for later submission. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["crash", "wer", "queue", "reporting", "privacy"],
    ),
    TweakDef(
        id="crash-set-kernel-dump-only",
        label="Set Crash Dump to Kernel Only",
        category="Crash & Diagnostics",
        apply_fn=_apply_kernel_dump_only,
        remove_fn=_remove_kernel_dump_only,
        detect_fn=_detect_kernel_dump_only,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CRASH],
        description=(
            "Sets CrashDumpEnabled to 2 (Kernel memory dump). Captures only "
            "kernel-mode memory, saving disk space while retaining key info. "
            "Default: 7 (Automatic). Recommended: 2 for production."
        ),
        tags=["crash", "dump", "kernel", "production", "disk"],
    ),
]


# ══ Additional Crash & Diagnostics Tweaks ====================================

_AEDEBUG_CRASH = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug"
_WER_CONSENT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent"


# -- Disable JIT Debugger Auto-Attach -----------------------------------------


def _apply_disable_jit_debugger(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Crash: disable automatic JIT debugger attachment on crash")
    SESSION.backup([_AEDEBUG_CRASH], "JITDebuggerAuto")
    SESSION.set_string(_AEDEBUG_CRASH, "Auto", "0")


def _remove_disable_jit_debugger(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_AEDEBUG_CRASH, "Auto", "1")


def _detect_disable_jit_debugger() -> bool:
    return SESSION.read_string(_AEDEBUG_CRASH, "Auto") == "0"


# -- Don't Send Additional Crash Data -----------------------------------------


def _apply_wer_no_additional_data(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Crash: configure WER to not send additional data")
    SESSION.backup([_WER], "WERNoAdditional")
    SESSION.set_dword(_WER, "DontSendAdditionalData", 1)


def _remove_wer_no_additional_data(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WER, "DontSendAdditionalData")


def _detect_wer_no_additional_data() -> bool:
    return SESSION.read_dword(_WER, "DontSendAdditionalData") == 1


# -- Disable WER Archive (stop storing reports on disk) -----------------------


def _apply_wer_disable_archive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Crash: disable WER report archive (stop storing crash reports)")
    SESSION.backup([_WER], "WERArchive")
    SESSION.set_dword(_WER, "DisableArchive", 1)


def _remove_wer_disable_archive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WER, "DisableArchive")


def _detect_wer_disable_archive() -> bool:
    return SESSION.read_dword(_WER, "DisableArchive") == 1


# -- Set WER Consent to Parameters Only ---------------------------------------


def _apply_wer_min_consent(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Crash: set WER consent to send parameters only (minimize data sharing)")
    SESSION.backup([_WER_CONSENT], "WERConsent")
    SESSION.set_dword(_WER_CONSENT, "DefaultConsent", 2)


def _remove_wer_min_consent(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WER_CONSENT, "DefaultConsent")


def _detect_wer_min_consent() -> bool:
    return SESSION.read_dword(_WER_CONSENT, "DefaultConsent") == 2


# -- Suppress WER UI Popup for Current User -----------------------------------


def _apply_suppress_crash_ui(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Crash: suppress Windows Error Reporting UI popup for current user")
    SESSION.backup([_WER_CU], "WERNoUI")
    SESSION.set_dword(_WER_CU, "DontShowUI", 1)


def _remove_suppress_crash_ui(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WER_CU, "DontShowUI")


def _detect_suppress_crash_ui() -> bool:
    return SESSION.read_dword(_WER_CU, "DontShowUI") == 1


TWEAKS += [
    TweakDef(
        id="crash-disable-jit-debugger",
        label="Disable Auto-Attach of JIT Debugger",
        category="Crash & Diagnostics",
        apply_fn=_apply_disable_jit_debugger,
        remove_fn=_remove_disable_jit_debugger,
        detect_fn=_detect_disable_jit_debugger,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_AEDEBUG_CRASH],
        description=(
            "Prevents Windows from automatically launching a JIT debugger when "
            "an application crashes. Suppresses the 'attach debugger?' dialog. "
            "Default: 1 (auto-attach). Recommended: 0 on production machines."
        ),
        tags=["crash", "debugger", "jit", "aedebug", "dev"],
    ),
    TweakDef(
        id="crash-wer-no-additional-data",
        label="WER: Don't Send Additional Crash Data",
        category="Crash & Diagnostics",
        apply_fn=_apply_wer_no_additional_data,
        remove_fn=_remove_wer_no_additional_data,
        detect_fn=_detect_wer_no_additional_data,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WER],
        description=(
            "Instructs Windows Error Reporting to not include supplemental data "
            "(heap dumps, user-mode state) when submitting crash reports. "
            "Default: sends all data. Recommended: disabled for privacy."
        ),
        tags=["crash", "wer", "privacy", "data", "telemetry"],
    ),
    TweakDef(
        id="crash-wer-disable-archive",
        label="WER: Disable Local Report Archive",
        category="Crash & Diagnostics",
        apply_fn=_apply_wer_disable_archive,
        remove_fn=_remove_wer_disable_archive,
        detect_fn=_detect_wer_disable_archive,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WER],
        description=(
            "Stops Windows Error Reporting from maintaining a local archive of "
            r"submitted crash reports in ProgramData\Microsoft\Windows\WER\ReportArchive. "
            "Default: archive enabled. Recommended: disabled for disk space."
        ),
        tags=["crash", "wer", "archive", "disk", "privacy"],
    ),
    TweakDef(
        id="crash-wer-min-consent",
        label="WER: Send Parameters Only (Minimal Consent)",
        category="Crash & Diagnostics",
        apply_fn=_apply_wer_min_consent,
        remove_fn=_remove_wer_min_consent,
        detect_fn=_detect_wer_min_consent,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WER_CONSENT],
        description=(
            "Sets WER DefaultConsent=2 (parameters only), limiting transmitted "
            "crash data to exception codes and fault module, not executable images. "
            "Default: varies (1=always ask, 4=send all). Recommended: 2 for privacy."
        ),
        tags=["crash", "wer", "consent", "privacy", "telemetry"],
    ),
    TweakDef(
        id="crash-suppress-wer-ui",
        label="Suppress WER Crash Popup for Current User",
        category="Crash & Diagnostics",
        apply_fn=_apply_suppress_crash_ui,
        remove_fn=_remove_suppress_crash_ui,
        detect_fn=_detect_suppress_crash_ui,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_WER_CU],
        description=(
            "Hides the Windows Error Reporting dialog that appears after an "
            "application crash, silently logging the report instead. "
            "Default: show dialog. Recommended: suppress on developer workstations."
        ),
        tags=["crash", "wer", "ui", "popup", "dialog"],
    ),
]
