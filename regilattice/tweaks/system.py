"""System tweaks — Long Paths, Reserved Storage, Remote Assistance, and more."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Long Paths ─────────────────────────────────────────────────────────────

_LONGPATH_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"
_RESERVED_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\ReserveManager"
)
_REMOTE_KEY = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Remote Assistance"
)


def apply_long_paths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-LongPaths")
    SESSION.backup([_LONGPATH_KEY], "LongPaths")
    SESSION.set_dword(_LONGPATH_KEY, "LongPathsEnabled", 1)
    SESSION.log("Completed Add-LongPaths")


def remove_long_paths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-LongPaths")
    SESSION.backup([_LONGPATH_KEY], "LongPaths_Remove")
    SESSION.set_dword(_LONGPATH_KEY, "LongPathsEnabled", 0)
    SESSION.log("Completed Remove-LongPaths")


def detect_long_paths() -> bool:
    return SESSION.read_dword(_LONGPATH_KEY, "LongPathsEnabled") == 1


# ── Disable Reserved Storage ─────────────────────────────────────────────


def _apply_disable_reserved(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable reserved storage (frees ~7 GB)")
    SESSION.backup([_RESERVED_KEY], "ReservedStorage")
    SESSION.set_dword(_RESERVED_KEY, "ShippedWithReserves", 0)


def _remove_disable_reserved(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RESERVED_KEY, "ShippedWithReserves", 1)


def _detect_disable_reserved() -> bool:
    return SESSION.read_dword(_RESERVED_KEY, "ShippedWithReserves") == 0


# ── Disable Remote Assistance ────────────────────────────────────────────


def _apply_disable_remote_assist(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable Remote Assistance")
    SESSION.backup([_REMOTE_KEY], "RemoteAssist")
    SESSION.set_dword(_REMOTE_KEY, "fAllowToGetHelp", 0)


def _remove_disable_remote_assist(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_REMOTE_KEY, "fAllowToGetHelp", 1)


def _detect_disable_remote_assist() -> bool:
    return SESSION.read_dword(_REMOTE_KEY, "fAllowToGetHelp") == 0


# ── High Timer Resolution (Performance) ──────────────────────────────────────────────────────

_TIMER_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"


def _apply_high_timer_resolution(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: enable global high timer resolution")
    SESSION.backup([_TIMER_KEY], "HighTimer")
    SESSION.set_dword(_TIMER_KEY, "GlobalTimerResolutionRequests", 1)


def _remove_high_timer_resolution(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TIMER_KEY, "GlobalTimerResolutionRequests")


def _detect_high_timer_resolution() -> bool:
    return SESSION.read_dword(_TIMER_KEY, "GlobalTimerResolutionRequests") == 1


# ── Disable UAC Dimming (Secure Desktop) ─────────────────────────────────────────────────────

_UAC_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Policies\System"
)


def _apply_disable_uac_dimming(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable UAC secure desktop dimming")
    SESSION.backup([_UAC_KEY], "UacDim")
    SESSION.set_dword(_UAC_KEY, "PromptOnSecureDesktop", 0)


def _remove_disable_uac_dimming(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_UAC_KEY, "PromptOnSecureDesktop", 1)


def _detect_disable_uac_dimming() -> bool:
    return SESSION.read_dword(_UAC_KEY, "PromptOnSecureDesktop") == 0


# ── Verbose Boot Messages ───────────────────────────────────────────────────

_BOOT_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"


def _apply_verbose_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: enable verbose boot/shutdown messages")
    SESSION.backup([_BOOT_KEY], "VerboseBoot")
    SESSION.set_dword(_BOOT_KEY, "VerboseStatus", 1)


def _remove_verbose_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BOOT_KEY, "VerboseStatus")


def _detect_verbose_boot() -> bool:
    return SESSION.read_dword(_BOOT_KEY, "VerboseStatus") == 1


# ── Disable Autoplay ─────────────────────────────────────────────────────

_AUTOPLAY_KEY = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers"


def _apply_disable_autoplay(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable AutoPlay for all media")
    SESSION.backup([_AUTOPLAY_KEY], "AutoPlay")
    SESSION.set_dword(_AUTOPLAY_KEY, "DisableAutoplay", 1)


def _remove_disable_autoplay(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_AUTOPLAY_KEY, "DisableAutoplay", 0)


def _detect_disable_autoplay() -> bool:
    return SESSION.read_dword(_AUTOPLAY_KEY, "DisableAutoplay") == 1


# ── Disable Activity History ─────────────────────────────────────────────

_ACTIVITY_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"


def _apply_disable_activity_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable Activity History")
    SESSION.backup([_ACTIVITY_KEY], "ActivityHistory")
    SESSION.set_dword(_ACTIVITY_KEY, "EnableActivityFeed", 0)
    SESSION.set_dword(_ACTIVITY_KEY, "PublishUserActivities", 0)
    SESSION.set_dword(_ACTIVITY_KEY, "UploadUserActivities", 0)


def _remove_disable_activity_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ACTIVITY_KEY, "EnableActivityFeed")
    SESSION.delete_value(_ACTIVITY_KEY, "PublishUserActivities")
    SESSION.delete_value(_ACTIVITY_KEY, "UploadUserActivities")


def _detect_disable_activity_history() -> bool:
    return SESSION.read_dword(_ACTIVITY_KEY, "EnableActivityFeed") == 0


# ── Disable Clipboard History ────────────────────────────────────────────

_CLIPBOARD_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"


def _apply_disable_clipboard_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable Clipboard History")
    SESSION.backup([_CLIPBOARD_KEY], "ClipboardHistory")
    SESSION.set_dword(_CLIPBOARD_KEY, "AllowClipboardHistory", 0)


def _remove_disable_clipboard_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLIPBOARD_KEY, "AllowClipboardHistory")


def _detect_disable_clipboard_history() -> bool:
    return SESSION.read_dword(_CLIPBOARD_KEY, "AllowClipboardHistory") == 0


# ── Disable Administrative Shares ────────────────────────────────────────

_LANMAN_KEY = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\LanmanServer\Parameters"
)


def _apply_disable_admin_shares(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable administrative shares (C$, ADMIN$)")
    SESSION.backup([_LANMAN_KEY], "AdminShares")
    SESSION.set_dword(_LANMAN_KEY, "AutoShareWks", 0)


def _remove_disable_admin_shares(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LANMAN_KEY, "AutoShareWks")


def _detect_disable_admin_shares() -> bool:
    return SESSION.read_dword(_LANMAN_KEY, "AutoShareWks") == 0


# ── Disable Windows Tips and Suggestions ─────────────────────────────────────

_TIPS_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\ContentDeliveryManager"
)


def _apply_disable_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable Windows tips and suggestions")
    SESSION.backup([_TIPS_KEY], "Tips")
    SESSION.set_dword(_TIPS_KEY, "SubscribedContent-338389Enabled", 0)
    SESSION.set_dword(_TIPS_KEY, "SoftLandingEnabled", 0)


def _remove_disable_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TIPS_KEY, "SubscribedContent-338389Enabled")
    SESSION.delete_value(_TIPS_KEY, "SoftLandingEnabled")


def _detect_disable_tips() -> bool:
    return SESSION.read_dword(_TIPS_KEY, "SubscribedContent-338389Enabled") == 0


# ── Disable Automatic Maintenance ────────────────────────────────────────────

_MAINTENANCE_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Schedule\Maintenance"
)


def _apply_disable_auto_maintenance(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable automatic maintenance")
    SESSION.backup([_MAINTENANCE_KEY], "AutoMaintenance")
    SESSION.set_dword(_MAINTENANCE_KEY, "MaintenanceDisabled", 1)


def _remove_disable_auto_maintenance(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MAINTENANCE_KEY, "MaintenanceDisabled")


def _detect_disable_auto_maintenance() -> bool:
    return SESSION.read_dword(_MAINTENANCE_KEY, "MaintenanceDisabled") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="enable-long-paths",
        label="Enable Win32 Long Paths",
        category="System",
        apply_fn=apply_long_paths,
        remove_fn=remove_long_paths,
        detect_fn=detect_long_paths,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_LONGPATH_KEY],
        description="Allows Win32 applications to use paths longer than 260 chars.",
        tags=["system", "filesystem", "long-paths"],
    ),
    TweakDef(
        id="disable-reserved-storage",
        label="Disable Reserved Storage (~7 GB)",
        category="System",
        apply_fn=_apply_disable_reserved,
        remove_fn=_remove_disable_reserved,
        detect_fn=_detect_disable_reserved,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_RESERVED_KEY],
        description="Disables Windows Reserved Storage which holds ~7 GB for updates.",
        tags=["system", "disk", "storage", "cleanup"],
    ),
    TweakDef(
        id="disable-remote-assistance",
        label="Disable Remote Assistance",
        category="System",
        apply_fn=_apply_disable_remote_assist,
        remove_fn=_remove_disable_remote_assist,
        detect_fn=_detect_disable_remote_assist,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_REMOTE_KEY],
        description="Disables Remote Assistance to reduce attack surface.",
        tags=["system", "security", "remote"],
    ),
    TweakDef(
        id="high-timer-resolution",
        label="Enable High Timer Resolution (Perf)",
        category="System",
        apply_fn=_apply_high_timer_resolution,
        remove_fn=_remove_high_timer_resolution,
        detect_fn=_detect_high_timer_resolution,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TIMER_KEY],
        description=(
            "Enables global high timer resolution (0.5ms) for improved "
            "scheduling accuracy. Benefits real-time audio, gaming, "
            "and latency-sensitive applications. "
            "Default: Disabled. Recommended: Enabled for gaming/audio."
        ),
        tags=["system", "performance", "timer", "latency"],
    ),
    TweakDef(
        id="disable-uac-dimming",
        label="Disable UAC Secure Desktop Dimming",
        category="System",
        apply_fn=_apply_disable_uac_dimming,
        remove_fn=_remove_disable_uac_dimming,
        detect_fn=_detect_disable_uac_dimming,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_UAC_KEY],
        description=(
            "Disables the dimmed secure desktop for UAC prompts. "
            "UAC still prompts but without screen dimming (faster). "
            "Default: Enabled. Recommended: Disabled for power users."
        ),
        tags=["system", "uac", "ux"],
    ),
    TweakDef(
        id="verbose-boot-status",
        label="Enable Verbose Boot Messages",
        category="System",
        apply_fn=_apply_verbose_boot,
        remove_fn=_remove_verbose_boot,
        detect_fn=_detect_verbose_boot,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BOOT_KEY],
        description=("Shows detailed status messages during Windows startup and shutdown instead of the generic loading screen."),
        tags=["system", "boot", "diagnostics"],
    ),
    TweakDef(
        id="disable-autoplay",
        label="Disable AutoPlay",
        category="System",
        apply_fn=_apply_disable_autoplay,
        remove_fn=_remove_disable_autoplay,
        detect_fn=_detect_disable_autoplay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_AUTOPLAY_KEY],
        description=("Disables AutoPlay for all removable media and devices. Security best practice to avoid malware auto-execution."),
        tags=["system", "security", "autoplay", "usb"],
    ),
    TweakDef(
        id="sys-disable-activity-history",
        label="Disable Activity History",
        category="System",
        apply_fn=_apply_disable_activity_history,
        remove_fn=_remove_disable_activity_history,
        detect_fn=_detect_disable_activity_history,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ACTIVITY_KEY],
        description=("Disables Activity History, preventing Windows from collecting and uploading activity data (timeline, app usage)."),
        tags=["system", "privacy", "activity-history", "timeline"],
    ),
    TweakDef(
        id="disable-clipboard-history",
        label="Disable Clipboard History",
        category="System",
        apply_fn=_apply_disable_clipboard_history,
        remove_fn=_remove_disable_clipboard_history,
        detect_fn=_detect_disable_clipboard_history,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CLIPBOARD_KEY],
        description=("Disables Windows Clipboard History (Win+V). Prevents clipboard content from being stored and synced."),
        tags=["system", "privacy", "clipboard"],
    ),
    TweakDef(
        id="disable-admin-shares",
        label="Disable Administrative Shares (C$, ADMIN$)",
        category="System",
        apply_fn=_apply_disable_admin_shares,
        remove_fn=_remove_disable_admin_shares,
        detect_fn=_detect_disable_admin_shares,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_LANMAN_KEY],
        description=("Disables default administrative shares (C$, ADMIN$). Reduces lateral-movement attack surface on workstations."),
        tags=["system", "security", "network", "shares"],
    ),
    TweakDef(
        id="system-disable-tips",
        label="Disable Windows Tips and Suggestions",
        category="System",
        apply_fn=_apply_disable_tips,
        remove_fn=_remove_disable_tips,
        detect_fn=_detect_disable_tips,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TIPS_KEY],
        description=(
            "Disables Windows tips, suggestions, and 'Get Even More Out of Windows' popups. "
            "Reduces distractions. Default: Enabled. Recommended: Disabled."
        ),
        tags=["system", "tips", "suggestions", "nag"],
    ),
    TweakDef(
        id="system-disable-auto-maintenance",
        label="Disable Automatic Maintenance",
        category="System",
        apply_fn=_apply_disable_auto_maintenance,
        remove_fn=_remove_disable_auto_maintenance,
        detect_fn=_detect_disable_auto_maintenance,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MAINTENANCE_KEY],
        description=(
            "Disables Windows automatic maintenance tasks (defrag, diagnostics, updates). "
            "Prevents unexpected disk and CPU usage. Default: Enabled. "
            "Recommended: Disabled for manual control."
        ),
        tags=["system", "maintenance", "performance", "scheduled"],
    ),
]


# -- Disable Fast Startup (HiberBoot) ------------------------------------------

_SESSION_POWER = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Power"
)


def _apply_sys_disable_fast_startup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disabling Fast Startup")
    SESSION.backup([_SESSION_POWER], "FastStartup")
    SESSION.set_dword(_SESSION_POWER, "HiberbootEnabled", 0)
    SESSION.log("System: Fast Startup disabled")


def _remove_sys_disable_fast_startup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SESSION_POWER], "FastStartup_Remove")
    SESSION.set_dword(_SESSION_POWER, "HiberbootEnabled", 1)


def _detect_sys_disable_fast_startup() -> bool:
    return SESSION.read_dword(_SESSION_POWER, "HiberbootEnabled") == 0


# -- Disable Windows Error Reporting -------------------------------------------

_WER_REPORT_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\Windows Error Reporting"
)


def _apply_sys_disable_error_reporting(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disabling Windows Error Reporting")
    SESSION.backup([_WER_REPORT_POLICY], "ErrorReporting")
    SESSION.set_dword(_WER_REPORT_POLICY, "Disabled", 1)
    SESSION.log("System: Windows Error Reporting disabled")


def _remove_sys_disable_error_reporting(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_WER_REPORT_POLICY], "ErrorReporting_Remove")
    SESSION.delete_value(_WER_REPORT_POLICY, "Disabled")


def _detect_sys_disable_error_reporting() -> bool:
    return SESSION.read_dword(_WER_REPORT_POLICY, "Disabled") == 1


TWEAKS += [
    TweakDef(
        id="sys-disable-fast-startup",
        label="Disable Fast Startup (HiberBoot)",
        category="System",
        apply_fn=_apply_sys_disable_fast_startup,
        remove_fn=_remove_sys_disable_fast_startup,
        detect_fn=_detect_sys_disable_fast_startup,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SESSION_POWER],
        description=("Disables Fast Startup (hybrid shutdown). Ensures clean boot every time. Default: Enabled. Recommended: Disabled."),
        tags=["system", "fast-startup", "hiberboot", "boot"],
    ),
    TweakDef(
        id="sys-disable-error-reporting",
        label="Disable Windows Error Reporting",
        category="System",
        apply_fn=_apply_sys_disable_error_reporting,
        remove_fn=_remove_sys_disable_error_reporting,
        detect_fn=_detect_sys_disable_error_reporting,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WER_REPORT_POLICY],
        description=("Disables Windows Error Reporting via Group Policy. Default: Enabled. Recommended: Disabled for privacy."),
        tags=["system", "error-reporting", "privacy", "wer"],
    ),
]


# ══ Additional System Tweaks (24H2+) ═════════════════════════════════════

_WINLOGON = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"
_MEMORY_MGMT = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"
_CRASH_CTRL = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"
_WPBT_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"


# -- Verbose Logon Messages ──────────────────────────────────────────────


def _apply_verbose_logon(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: enable verbose logon/logoff status messages")
    SESSION.backup([_WINLOGON], "VerboseLogon")
    SESSION.set_dword(
        r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
        "VerboseStatus",
        1,
    )


def _remove_verbose_logon(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(
        r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
        "VerboseStatus",
    )


def _detect_verbose_logon() -> bool:
    return (
        SESSION.read_dword(
            r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
            "VerboseStatus",
        )
        == 1
    )


# -- Detailed BSoD (Blue Screen of Death) ────────────────────────────────


def _apply_detailed_bsod(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: enable detailed BSoD with parameters")
    SESSION.backup([_CRASH_CTRL], "DetailedBSoD")
    SESSION.set_dword(_CRASH_CTRL, "DisplayParameters", 1)


def _remove_detailed_bsod(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CRASH_CTRL, "DisplayParameters")


def _detect_detailed_bsod() -> bool:
    return SESSION.read_dword(_CRASH_CTRL, "DisplayParameters") == 1


# -- Disable WPBT (Windows Platform Binary Table) ───────────────────────


def _apply_disable_wpbt(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable WPBT (vendor bloatware injection via UEFI)")
    SESSION.backup([_WPBT_KEY], "DisableWPBT")
    SESSION.set_dword(_WPBT_KEY, "DisableWpbtExecution", 1)


def _remove_disable_wpbt(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WPBT_KEY, "DisableWpbtExecution")


def _detect_disable_wpbt() -> bool:
    return SESSION.read_dword(_WPBT_KEY, "DisableWpbtExecution") == 1


# -- Disable Storage Sense ───────────────────────────────────────────────

_STORAGE_SENSE = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"


def _apply_disable_storage_sense(*, require_admin: bool = False) -> None:
    SESSION.log("System: disable Storage Sense automatic cleanup")
    SESSION.backup([_STORAGE_SENSE], "StorageSense")
    SESSION.set_dword(_STORAGE_SENSE, "01", 0)


def _remove_disable_storage_sense(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_STORAGE_SENSE, "01", 1)


def _detect_disable_storage_sense() -> bool:
    return SESSION.read_dword(_STORAGE_SENSE, "01") == 0


# -- Disable Automatic Reboot After Crash ────────────────────────────────


def _apply_disable_auto_reboot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable automatic reboot after crash")
    SESSION.backup([_CRASH_CTRL], "AutoReboot")
    SESSION.set_dword(_CRASH_CTRL, "AutoReboot", 0)


def _remove_disable_auto_reboot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH_CTRL, "AutoReboot", 1)


def _detect_disable_auto_reboot() -> bool:
    return SESSION.read_dword(_CRASH_CTRL, "AutoReboot") == 0


TWEAKS += [
    TweakDef(
        id="sys-verbose-logon",
        label="Enable Verbose Logon Messages",
        category="System",
        apply_fn=_apply_verbose_logon,
        remove_fn=_remove_verbose_logon,
        detect_fn=_detect_verbose_logon,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
        description=(
            "Shows detailed status messages during logon, logoff, startup, and "
            "shutdown (e.g., 'Applying Group Policy'). "
            "Default: disabled. Recommended: enabled for troubleshooting."
        ),
        tags=["system", "verbose", "logon", "boot", "diagnostic"],
    ),
    TweakDef(
        id="sys-detailed-bsod",
        label="Enable Detailed Blue Screen Info",
        category="System",
        apply_fn=_apply_detailed_bsod,
        remove_fn=_remove_detailed_bsod,
        detect_fn=_detect_detailed_bsod,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CRASH_CTRL],
        description=(
            "Shows technical parameters on BSoD screens instead of just the QR code "
            "and sad-face. Useful for diagnosing crash causes. "
            "Default: disabled. Recommended: enabled."
        ),
        tags=["system", "bsod", "crash", "diagnostic", "blue-screen"],
    ),
    TweakDef(
        id="sys-disable-wpbt",
        label="Disable WPBT (Vendor Bloatware Injection)",
        category="System",
        apply_fn=_apply_disable_wpbt,
        remove_fn=_remove_disable_wpbt,
        detect_fn=_detect_disable_wpbt,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WPBT_KEY],
        description=(
            "Disables the Windows Platform Binary Table which allows vendors "
            "to inject software via UEFI firmware (e.g., Lenovo, HP bloatware). "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["system", "wpbt", "uefi", "bloatware", "security"],
    ),
    TweakDef(
        id="sys-disable-storage-sense",
        label="Disable Storage Sense",
        category="System",
        apply_fn=_apply_disable_storage_sense,
        remove_fn=_remove_disable_storage_sense,
        detect_fn=_detect_disable_storage_sense,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_STORAGE_SENSE],
        description=(
            "Disables Storage Sense which automatically deletes temporary files, "
            "recycle bin contents, and Downloads folder items. "
            "Default: may be enabled. Recommended: disabled for control."
        ),
        tags=["system", "storage-sense", "cleanup", "disk"],
    ),
    TweakDef(
        id="sys-disable-auto-reboot",
        label="Disable Auto-Reboot After Crash",
        category="System",
        apply_fn=_apply_disable_auto_reboot,
        remove_fn=_remove_disable_auto_reboot,
        detect_fn=_detect_disable_auto_reboot,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CRASH_CTRL],
        description=(
            "Prevents Windows from automatically rebooting after a BSoD crash, "
            "giving time to read the error information. "
            "Default: auto-reboot. Recommended: disabled."
        ),
        tags=["system", "crash", "reboot", "bsod", "diagnostic"],
    ),
]


# -- Enable System Restore Frequency -------------------------------------------

_SYS_RESTORE_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"


def _apply_sys_restore_frequency(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: setting system restore frequency to unlimited")
    SESSION.backup([_SYS_RESTORE_KEY], "SysRestoreFreq")
    SESSION.set_dword(_SYS_RESTORE_KEY, "RPSessionInterval", 0)
    SESSION.log("System: system restore frequency set")


def _remove_sys_restore_frequency(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SYS_RESTORE_KEY], "SysRestoreFreq_Remove")
    SESSION.delete_value(_SYS_RESTORE_KEY, "RPSessionInterval")


def _detect_sys_restore_frequency() -> bool:
    return SESSION.read_dword(_SYS_RESTORE_KEY, "RPSessionInterval") == 0


# -- Disable Windows Tips -------------------------------------------------------

_CDM_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\ContentDeliveryManager"
)


def _apply_sys_disable_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disabling Windows Tips and Suggestions")
    SESSION.backup([_CDM_KEY], "SysDisableTips")
    SESSION.set_dword(_CDM_KEY, "SoftLandingEnabled", 0)
    SESSION.set_dword(_CDM_KEY, "SubscribedContent-338389Enabled", 0)
    SESSION.log("System: Windows Tips disabled")


def _remove_sys_disable_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CDM_KEY], "SysDisableTips_Remove")
    SESSION.set_dword(_CDM_KEY, "SoftLandingEnabled", 1)
    SESSION.set_dword(_CDM_KEY, "SubscribedContent-338389Enabled", 1)


def _detect_sys_disable_tips() -> bool:
    return SESSION.read_dword(_CDM_KEY, "SoftLandingEnabled") == 0


# -- Disable Power Throttling ---------------------------------------------------

_POWER_THROTTLE_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"


def _apply_sys_disable_power_throttling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disabling power throttling")
    SESSION.backup([_POWER_THROTTLE_KEY], "SysPowerThrottle")
    SESSION.set_dword(_POWER_THROTTLE_KEY, "PowerThrottlingOff", 1)
    SESSION.log("System: power throttling disabled")


def _remove_sys_disable_power_throttling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_POWER_THROTTLE_KEY], "SysPowerThrottle_Remove")
    SESSION.delete_value(_POWER_THROTTLE_KEY, "PowerThrottlingOff")


def _detect_sys_disable_power_throttling() -> bool:
    return SESSION.read_dword(_POWER_THROTTLE_KEY, "PowerThrottlingOff") == 1


TWEAKS += [
    TweakDef(
        id="sys-restore-frequency",
        label="Enable Unlimited System Restore Frequency",
        category="System",
        apply_fn=_apply_sys_restore_frequency,
        remove_fn=_remove_sys_restore_frequency,
        detect_fn=_detect_sys_restore_frequency,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SYS_RESTORE_KEY],
        description=(
            "Sets RPSessionInterval to 0, allowing unlimited system restore point creation frequency. "
            "Default: 1. Recommended: 0 for frequent snapshots."
        ),
        tags=["system", "restore", "backup", "frequency"],
    ),
    TweakDef(
        id="sys-disable-tips",
        label="Disable Windows Tips and Suggestions",
        category="System",
        apply_fn=_apply_sys_disable_tips,
        remove_fn=_remove_sys_disable_tips,
        detect_fn=_detect_sys_disable_tips,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM_KEY],
        description=("Disables Windows tips, tricks, and suggestions popups. Default: Enabled. Recommended: Disabled."),
        tags=["system", "tips", "suggestions", "notifications", "nag"],
    ),
    TweakDef(
        id="sys-disable-power-throttling",
        label="Disable Power Throttling",
        category="System",
        apply_fn=_apply_sys_disable_power_throttling,
        remove_fn=_remove_sys_disable_power_throttling,
        detect_fn=_detect_sys_disable_power_throttling,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_POWER_THROTTLE_KEY],
        description=(
            "Disables Windows power throttling which limits background app performance. Default: Enabled. Recommended: Disabled for desktops."
        ),
        tags=["system", "power", "throttling", "performance"],
    ),
]


# ══ Additional System Tweaks ═══════════════════════════════════════════

_POWER_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"
_FILESYSTEM_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"


def _apply_sys_disable_hibernation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable hibernation")
    SESSION.backup([_POWER_KEY], "SysHibernation")
    SESSION.set_dword(_POWER_KEY, "HibernateEnabled", 0)


def _remove_sys_disable_hibernation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_POWER_KEY, "HibernateEnabled", 1)


def _detect_sys_disable_hibernation() -> bool:
    return SESSION.read_dword(_POWER_KEY, "HibernateEnabled") == 0


def _apply_sys_disable_8dot3(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable 8.3 short filename creation")
    SESSION.backup([_FILESYSTEM_KEY], "Sys8dot3")
    SESSION.set_dword(_FILESYSTEM_KEY, "NtfsDisable8dot3NameCreation", 1)


def _remove_sys_disable_8dot3(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_FILESYSTEM_KEY, "NtfsDisable8dot3NameCreation", 0)


def _detect_sys_disable_8dot3() -> bool:
    return SESSION.read_dword(_FILESYSTEM_KEY, "NtfsDisable8dot3NameCreation") == 1


TWEAKS += [
    TweakDef(
        id="sys-disable-hibernation",
        label="Disable Hibernation",
        category="System",
        apply_fn=_apply_sys_disable_hibernation,
        remove_fn=_remove_sys_disable_hibernation,
        detect_fn=_detect_sys_disable_hibernation,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_POWER_KEY],
        description=(
            "Disables Windows hibernation, freeing disk space used by hiberfil.sys. Default: Enabled. Recommended: Disabled for desktops with SSDs."
        ),
        tags=["system", "hibernation", "power", "disk", "ssd"],
    ),
    TweakDef(
        id="sys-disable-8dot3",
        label="Disable 8.3 Short Filename Creation",
        category="System",
        apply_fn=_apply_sys_disable_8dot3,
        remove_fn=_remove_sys_disable_8dot3,
        detect_fn=_detect_sys_disable_8dot3,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_FILESYSTEM_KEY],
        description=(
            "Disables NTFS 8.3 short filename generation for improved file system "
            "performance. Default: Enabled. Recommended: Disabled on modern systems."
        ),
        tags=["system", "ntfs", "8dot3", "filesystem", "performance"],
    ),
]
