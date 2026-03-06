"""Boot tweaks — Verbose boot, boot logo, timeout, UEFI, NumLock."""

from __future__ import annotations

import subprocess

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Policies\System"
)
_BCD = r"HKEY_LOCAL_MACHINE\BCD00000000\Objects"
_BOOTUX = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\BootControl"
)
_BOOT_CTL = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
_BOOT_SESSMGR = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"


# ── Functions ────────────────────────────────────────────────────────────────


def apply_verbose_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-VerboseBoot")
    SESSION.backup([_KEY], "VerboseBoot")
    SESSION.set_dword(_KEY, "verbosestatus", 1)
    SESSION.log("Completed Add-VerboseBoot")


def remove_verbose_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-VerboseBoot")
    SESSION.backup([_KEY], "VerboseBoot_Remove")
    SESSION.delete_value(_KEY, "verbosestatus")
    SESSION.log("Completed Remove-VerboseBoot")


def detect_verbose_boot() -> bool:
    return SESSION.read_dword(_KEY, "verbosestatus") == 1


# ── Disable Boot Logo ────────────────────────────────────────────────────────


def apply_disable_boot_logo(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: disable boot splash logo via bcdedit")
    subprocess.run(
        ["bcdedit", "/set", "quietboot", "off"],
        check=False,
        capture_output=True,
        text=True,
    )
    SESSION.log("Completed disable-boot-logo")


def remove_disable_boot_logo(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["bcdedit", "/set", "quietboot", "on"],
        check=False,
        capture_output=True,
        text=True,
    )


def detect_disable_boot_logo() -> bool:
    try:
        r = subprocess.run(
            ["bcdedit", "/enum", "{current}"],
            capture_output=True,
            text=True,
            timeout=5,
            check=False,
        )
        return "quietboot" in r.stdout.lower() and "off" in r.stdout.lower()
    except (OSError, subprocess.SubprocessError):
        return False


# ── Reduce Boot Timeout ──────────────────────────────────────────────────────


def apply_fast_boot_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: set OS selection timeout to 3 seconds")
    subprocess.run(
        ["bcdedit", "/timeout", "3"],
        check=False,
        capture_output=True,
        text=True,
    )


def remove_fast_boot_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["bcdedit", "/timeout", "30"],
        check=False,
        capture_output=True,
        text=True,
    )


def detect_fast_boot_timeout() -> bool:
    try:
        r = subprocess.run(
            ["bcdedit", "/enum", "{bootmgr}"],
            capture_output=True,
            text=True,
            timeout=5,
            check=False,
        )
        for line in r.stdout.splitlines():
            if "timeout" in line.lower():
                return "3" in line.split()[-1]
        return False
    except (OSError, subprocess.SubprocessError):
        return False


# ── Enable NumLock at Boot ───────────────────────────────────────────────────────────────────

_KEYBOARD_KEY = r"HKEY_USERS\.DEFAULT\Control Panel\Keyboard"


def _apply_numlock_on_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: enable NumLock at login screen")
    SESSION.backup([_KEYBOARD_KEY], "NumLockBoot")
    SESSION.set_string(_KEYBOARD_KEY, "InitialKeyboardIndicators", "2")


def _remove_numlock_on_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_KEYBOARD_KEY, "InitialKeyboardIndicators", "0")


def _detect_numlock_on_boot() -> bool:
    return SESSION.read_string(_KEYBOARD_KEY, "InitialKeyboardIndicators") == "2"


# ── Disable Boot-Time Driver Verification ────────────────────────────────────────────────────

_SESSMGR = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"


def _apply_disable_driver_verifier(*, require_admin: bool = True) -> None:
    """Disable Windows Driver Verifier at boot for faster startup (performance)."""
    assert_admin(require_admin)
    SESSION.log("Boot: disable Driver Verifier for faster boot")
    subprocess.run(["verifier", "/reset"], check=False, capture_output=True, text=True)


def _remove_disable_driver_verifier(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: re-enable standard Driver Verifier")
    subprocess.run(["verifier", "/standard", "/all"], check=False, capture_output=True, text=True)


def _detect_disable_driver_verifier() -> bool:
    try:
        r = subprocess.run(["verifier", "/querysettings"], capture_output=True, text=True, timeout=5, check=False)
        return "no driver" in r.stdout.lower() or r.returncode != 0
    except (OSError, subprocess.SubprocessError):
        return False


# ── Enable Boot Log (ntbtlog.txt) ────────────────────────────────────────────


def _apply_boot_log(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: enable boot log (ntbtlog.txt)")
    subprocess.run(
        ["bcdedit", "/set", "{current}", "bootlog", "Yes"],
        check=False,
        capture_output=True,
        text=True,
    )


def _remove_boot_log(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["bcdedit", "/set", "{current}", "bootlog", "No"],
        check=False,
        capture_output=True,
        text=True,
    )


def _detect_boot_log() -> bool:
    try:
        r = subprocess.run(
            ["bcdedit", "/enum", "{current}"],
            capture_output=True,
            text=True,
            timeout=5,
            check=False,
        )
        return "bootlog" in r.stdout.lower() and "yes" in r.stdout.lower()
    except (OSError, subprocess.SubprocessError):
        return False


# ── Disable Windows Recovery Environment ─────────────────────────────────────


def _apply_disable_winre(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: disable Windows Recovery Environment")
    subprocess.run(
        ["reagentc", "/disable"],
        check=False,
        capture_output=True,
        text=True,
    )


def _remove_disable_winre(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["reagentc", "/enable"],
        check=False,
        capture_output=True,
        text=True,
    )


def _detect_disable_winre() -> bool:
    try:
        r = subprocess.run(
            ["reagentc", "/info"],
            capture_output=True,
            text=True,
            timeout=5,
            check=False,
        )
        return "disabled" in r.stdout.lower()
    except (OSError, subprocess.SubprocessError):
        return False


# ── Disable Automatic Repair on Boot ─────────────────────────────────────────


def _apply_disable_auto_repair(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: disable automatic repair")
    subprocess.run(
        ["bcdedit", "/set", "{current}", "recoveryenabled", "No"],
        check=False,
        capture_output=True,
        text=True,
    )


def _remove_disable_auto_repair(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["bcdedit", "/set", "{current}", "recoveryenabled", "Yes"],
        check=False,
        capture_output=True,
        text=True,
    )


def _detect_disable_auto_repair() -> bool:
    try:
        r = subprocess.run(
            ["bcdedit", "/enum", "{current}"],
            capture_output=True,
            text=True,
            timeout=5,
            check=False,
        )
        for line in r.stdout.splitlines():
            if "recoveryenabled" in line.lower():
                return "no" in line.lower()
        return False
    except (OSError, subprocess.SubprocessError):
        return False


# ── Set Boot Policy to Ignore All Failures ────────────────────────────────────


def _apply_ignore_boot_failures(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: set boot failure policy to ignore")
    subprocess.run(
        ["bcdedit", "/set", "{current}", "bootstatuspolicy", "IgnoreAllFailures"],
        check=False,
        capture_output=True,
        text=True,
    )


def _remove_ignore_boot_failures(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["bcdedit", "/deletevalue", "{current}", "bootstatuspolicy"],
        check=False,
        capture_output=True,
        text=True,
    )


def _detect_ignore_boot_failures() -> bool:
    try:
        r = subprocess.run(
            ["bcdedit", "/enum", "{current}"],
            capture_output=True,
            text=True,
            timeout=5,
            check=False,
        )
        return "ignoreallfailures" in r.stdout.lower()
    except (OSError, subprocess.SubprocessError):
        return False


# ── Disable UEFI Secure Boot Check ───────────────────────────────────────────

_SECBOOT_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State"


def _apply_disable_secboot_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: disable Secure Boot state check notification")
    SESSION.backup([_SECBOOT_KEY], "SecBootCheck")
    SESSION.set_dword(_SECBOOT_KEY, "UEFISecureBootEnabled", 0)


def _remove_disable_secboot_check(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SECBOOT_KEY, "UEFISecureBootEnabled", 1)


def _detect_disable_secboot_check() -> bool:
    return SESSION.read_dword(_SECBOOT_KEY, "UEFISecureBootEnabled") == 0


# ── Verbose Status During Boot ────────────────────────────────────────────────


def _apply_verbose_status(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: enable verbose status messages instead of logo")
    SESSION.backup([_BOOT_CTL], "VerboseStatus")
    SESSION.set_dword(_BOOT_CTL, "VerboseStatus", 1)


def _remove_verbose_status(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BOOT_CTL, "VerboseStatus")


def _detect_verbose_status() -> bool:
    return SESSION.read_dword(_BOOT_CTL, "VerboseStatus") == 1


# ── Reduce ChkDsk Boot Timeout ───────────────────────────────────────────────


def _apply_chkdsk_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: reduce ChkDsk countdown from 10s to 3s")
    SESSION.backup([_BOOT_SESSMGR], "ChkDskTimeout")
    SESSION.set_dword(_BOOT_SESSMGR, "AutoChkTimeout", 3)


def _remove_chkdsk_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BOOT_SESSMGR, "AutoChkTimeout", 10)


def _detect_chkdsk_timeout() -> bool:
    return SESSION.read_dword(_BOOT_SESSMGR, "AutoChkTimeout") == 3


# ── Disable Boot Animation ────────────────────────────────────────────────

_BOOT_ANIM_PARAMS = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\BootAnimationParams"
)


def _apply_disable_boot_anim(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: disable boot animation/spinner")
    SESSION.backup([_BOOT_ANIM_PARAMS], "BootAnimation")
    SESSION.set_dword(_BOOT_ANIM_PARAMS, "Disabled", 1)


def _remove_disable_boot_anim(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BOOT_ANIM_PARAMS, "Disabled")


def _detect_disable_boot_anim() -> bool:
    return SESSION.read_dword(_BOOT_ANIM_PARAMS, "Disabled") == 1


# ── Disable Registry Flush Bandwidth Limit ───────────────────────────────

_CONFIG_MGR = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Configuration Manager"
)


def _apply_max_registry_flush(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: disable registry flush bandwidth limiting")
    SESSION.backup([_CONFIG_MGR], "RegistryFlush")
    SESSION.set_dword(_CONFIG_MGR, "MaximumBandwidthForRegistryFlush", 0)


def _remove_max_registry_flush(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CONFIG_MGR, "MaximumBandwidthForRegistryFlush")


def _detect_max_registry_flush() -> bool:
    return SESSION.read_dword(_CONFIG_MGR, "MaximumBandwidthForRegistryFlush") == 0


# ── Enable Fast Startup (Hiberboot) ───────────────────────────────────────────

_HIBERBOOT = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Power"
)


def _apply_enable_fast_startup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: enable Fast Startup (Hiberboot)")
    SESSION.backup([_HIBERBOOT], "FastStartup")
    SESSION.set_dword(_HIBERBOOT, "HiberbootEnabled", 1)


def _remove_enable_fast_startup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_HIBERBOOT, "HiberbootEnabled", 0)


def _detect_enable_fast_startup() -> bool:
    return SESSION.read_dword(_HIBERBOOT, "HiberbootEnabled") == 1


# ── Disable Boot Startup Sound ───────────────────────────────────────────────

_BOOT_SOUND = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Authentication\LogonUI\BootAnimation"
)


def _apply_disable_boot_startup_sound(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: disable Windows startup sound")
    SESSION.backup([_BOOT_SOUND], "BootStartupSound")
    SESSION.set_dword(_BOOT_SOUND, "DisableStartupSound", 1)


def _remove_disable_boot_startup_sound(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BOOT_SOUND, "DisableStartupSound")


def _detect_disable_boot_startup_sound() -> bool:
    return SESSION.read_dword(_BOOT_SOUND, "DisableStartupSound") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="boot-verbose-boot",
        label="Verbose Boot Messages",
        category="Boot",
        apply_fn=apply_verbose_boot,
        remove_fn=remove_verbose_boot,
        detect_fn=detect_verbose_boot,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY],
        description="Shows detailed service and driver status messages during boot.",
        tags=["boot", "debug", "diagnostic"],
    ),
    TweakDef(
        id="boot-disable-boot-logo",
        label="Disable Boot Splash Logo",
        category="Boot",
        apply_fn=apply_disable_boot_logo,
        remove_fn=remove_disable_boot_logo,
        detect_fn=detect_disable_boot_logo,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[],
        description="Disables the Windows boot splash animation for a text-mode boot.",
        tags=["boot", "performance", "splash"],
    ),
    TweakDef(
        id="boot-fast-boot-timeout",
        label="Reduce Boot Menu Timeout (3s)",
        category="Boot",
        apply_fn=apply_fast_boot_timeout,
        remove_fn=remove_fast_boot_timeout,
        detect_fn=detect_fast_boot_timeout,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[],
        description="Reduces the OS selection boot menu timeout from 30s to 3s.",
        tags=["boot", "performance", "timeout"],
    ),
    TweakDef(
        id="boot-numlock-on-boot",
        label="Enable NumLock at Boot",
        category="Boot",
        apply_fn=_apply_numlock_on_boot,
        remove_fn=_remove_numlock_on_boot,
        detect_fn=_detect_numlock_on_boot,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEYBOARD_KEY],
        description=("Turns on NumLock automatically at the login screen. Options: 0=Off, 2=On. Default: 0 (Off). Recommended: On."),
        tags=["boot", "keyboard", "numlock"],
    ),
    TweakDef(
        id="boot-disable-driver-verifier",
        label="Disable Boot Driver Verifier (Perf)",
        category="Boot",
        apply_fn=_apply_disable_driver_verifier,
        remove_fn=_remove_disable_driver_verifier,
        detect_fn=_detect_disable_driver_verifier,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[],
        description=(
            "Disables Windows Driver Verifier at boot for faster startup. "
            "Only relevant if Driver Verifier was previously enabled for debugging. "
            "Default: Disabled. Recommended: Keep disabled for production."
        ),
        tags=["boot", "performance", "driver"],
    ),
    TweakDef(
        id="boot-log",
        label="Enable Boot Log (ntbtlog.txt)",
        category="Boot",
        apply_fn=_apply_boot_log,
        remove_fn=_remove_boot_log,
        detect_fn=_detect_boot_log,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[],
        description=("Creates a text file (ntbtlog.txt) listing all drivers loaded during boot. Useful for diagnosing boot problems."),
        tags=["boot", "debug", "log"],
    ),
    TweakDef(
        id="boot-disable-winre",
        label="Disable Windows Recovery Environment",
        category="Boot",
        apply_fn=_apply_disable_winre,
        remove_fn=_remove_disable_winre,
        detect_fn=_detect_disable_winre,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[],
        description=(
            "Disables the Windows Recovery Environment (WinRE). Saves ~450 MB of reserved disk space but prevents automatic repair on failures."
        ),
        tags=["boot", "recovery", "disk"],
    ),
    TweakDef(
        id="boot-disable-auto-repair",
        label="Disable Automatic Boot Repair",
        category="Boot",
        apply_fn=_apply_disable_auto_repair,
        remove_fn=_remove_disable_auto_repair,
        detect_fn=_detect_disable_auto_repair,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[],
        description=("Prevents Windows from launching Automatic Repair after boot failures. Useful when the repair loop causes more harm than good."),
        tags=["boot", "repair", "recovery"],
    ),
    TweakDef(
        id="boot-ignore-boot-failures",
        label="Ignore All Boot Failures",
        category="Boot",
        apply_fn=_apply_ignore_boot_failures,
        remove_fn=_remove_ignore_boot_failures,
        detect_fn=_detect_ignore_boot_failures,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[],
        description=("Sets boot status policy to ignore all failures, suppressing the 'Windows did not start successfully' screen."),
        tags=["boot", "performance", "failure"],
    ),
    TweakDef(
        id="boot-disable-secboot-check",
        label="Suppress Secure Boot Status Check",
        category="Boot",
        apply_fn=_apply_disable_secboot_check,
        remove_fn=_remove_disable_secboot_check,
        detect_fn=_detect_disable_secboot_check,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SECBOOT_KEY],
        description=("Suppresses the Secure Boot status notification in Windows by setting UEFISecureBootEnabled to 0 in the registry."),
        tags=["boot", "security", "uefi"],
    ),
    TweakDef(
        id="boot-disable-logo",
        label="Disable Boot Logo",
        category="Boot",
        apply_fn=_apply_verbose_status,
        remove_fn=_remove_verbose_status,
        detect_fn=_detect_verbose_status,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BOOT_CTL],
        description=(
            "Shows verbose status messages during boot instead of the "
            "Windows logo animation. Useful for diagnosing slow boot. "
            "Default: Logo. Recommended: Verbose for troubleshooting."
        ),
        tags=["boot", "verbose", "diagnostics"],
    ),
    TweakDef(
        id="boot-menu-timeout",
        label="Reduce Boot Menu Timeout",
        category="Boot",
        apply_fn=_apply_chkdsk_timeout,
        remove_fn=_remove_chkdsk_timeout,
        detect_fn=_detect_chkdsk_timeout,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BOOT_SESSMGR],
        description=(
            "Reduces the automatic disk check (ChkDsk) countdown from "
            "10 to 3 seconds on boot. Speeds up boot when a disk check "
            "is pending. Default: 10s. Recommended: 3s."
        ),
        tags=["boot", "chkdsk", "timeout", "performance"],
    ),
    TweakDef(
        id="boot-disable-boot-anim",
        label="Disable Boot Animation/Spinner",
        category="Boot",
        apply_fn=_apply_disable_boot_anim,
        remove_fn=_remove_disable_boot_anim,
        detect_fn=_detect_disable_boot_anim,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_BOOT_ANIM_PARAMS],
        description=(
            "Disables the Windows boot animation/spinner for a faster "
            "perceived boot. The boot process skips the animated dots. "
            "Default: enabled. Recommended: disabled for faster boot."
        ),
        tags=["boot", "animation", "performance", "spinner"],
    ),
    TweakDef(
        id="boot-max-proc-count",
        label="Disable Registry Flush Bandwidth Limit",
        category="Boot",
        apply_fn=_apply_max_registry_flush,
        remove_fn=_remove_max_registry_flush,
        detect_fn=_detect_max_registry_flush,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CONFIG_MGR],
        description=(
            "Disables the bandwidth limit for registry hive flushing "
            "during boot and shutdown. Can speed up boot on systems "
            "with fast storage. Default: system-managed. "
            "Recommended: 0 (unlimited) on NVMe/SSD."
        ),
        tags=["boot", "registry", "flush", "performance", "ssd"],
    ),
    TweakDef(
        id="boot-enable-fast-startup",
        label="Enable Fast Startup (Hiberboot)",
        category="Boot",
        apply_fn=_apply_enable_fast_startup,
        remove_fn=_remove_enable_fast_startup,
        detect_fn=_detect_enable_fast_startup,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_HIBERBOOT],
        description=(
            "Enables Windows Fast Startup which uses a hybrid shutdown "
            "with hibernation to speed up boot time. "
            "Default: Usually enabled. Recommended: Enabled for fast boot."
        ),
        tags=["boot", "fast-startup", "hiberboot", "performance"],
    ),
    TweakDef(
        id="boot-disable-startup-sound",
        label="Disable Boot Startup Sound",
        category="Boot",
        apply_fn=_apply_disable_boot_startup_sound,
        remove_fn=_remove_disable_boot_startup_sound,
        detect_fn=_detect_disable_boot_startup_sound,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BOOT_SOUND],
        description=(
            "Disables the Windows startup sound that plays during boot. "
            "Useful for silent or headless environments. "
            "Default: Enabled. Recommended: Disabled for quiet boot."
        ),
        tags=["boot", "sound", "startup", "silent"],
    ),
]


# ── Disable Hibernation File ─────────────────────────────────────────────────

_POWER_CTL = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"
_PREFETCH_PARAMS = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Memory Management\PrefetchParameters"
)


def _apply_hiberfile_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable hibernation to free disk space")
    SESSION.backup([_POWER_CTL], "Hibernation")
    SESSION.set_dword(_POWER_CTL, "HibernateEnabled", 0)


def _remove_hiberfile_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_POWER_CTL, "HibernateEnabled", 1)


def _detect_hiberfile_off() -> bool:
    return SESSION.read_dword(_POWER_CTL, "HibernateEnabled") == 0


# ── Boot Prefetch Optimized ──────────────────────────────────────────────────


def _apply_prefetch_optimized(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Set boot prefetch to optimized mode")
    SESSION.backup([_PREFETCH_PARAMS], "BootPrefetch")
    SESSION.set_dword(_PREFETCH_PARAMS, "EnablePrefetcher", 3)


def _remove_prefetch_optimized(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PREFETCH_PARAMS, "EnablePrefetcher", 0)


def _detect_prefetch_optimized() -> bool:
    return SESSION.read_dword(_PREFETCH_PARAMS, "EnablePrefetcher") == 3


TWEAKS += [
    TweakDef(
        id="boot-disable-hiberfile",
        label="Disable Hibernation",
        category="Boot",
        apply_fn=_apply_hiberfile_off,
        remove_fn=_remove_hiberfile_off,
        detect_fn=_detect_hiberfile_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_POWER_CTL],
        description=(
            "Disables hibernation and removes the hiberfil.sys file to free disk space. Default: Enabled. Recommended: Disabled on desktops with SSD."
        ),
        tags=["boot", "hibernation", "disk-space", "performance"],
    ),
    TweakDef(
        id="boot-prefetch-optimized",
        label="Set Prefetch to Optimized Mode",
        category="Boot",
        apply_fn=_apply_prefetch_optimized,
        remove_fn=_remove_prefetch_optimized,
        detect_fn=_detect_prefetch_optimized,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_PREFETCH_PARAMS],
        description=(
            "Enables both boot and application prefetching for optimal "
            "performance. Value 3 = boot + app prefetch. "
            "Default: 3. Recommended: 3 for SSDs and HDDs."
        ),
        tags=["boot", "prefetch", "performance", "startup"],
    ),
]


_CRASH_CTL = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"
_MEM_MGMT = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Memory Management"
)


# ── Disable Auto-Reboot on BSOD ───────────────────────────────────────────


def _apply_disable_auto_reboot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: disable automatic reboot after BSOD")
    SESSION.backup([_CRASH_CTL], "AutoReboot")
    SESSION.set_dword(_CRASH_CTL, "AutoReboot", 0)


def _remove_disable_auto_reboot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CRASH_CTL], "AutoReboot_Remove")
    SESSION.set_dword(_CRASH_CTL, "AutoReboot", 1)


def _detect_disable_auto_reboot() -> bool:
    return SESSION.read_dword(_CRASH_CTL, "AutoReboot") == 0


# ── Show Full BSOD Parameters ──────────────────────────────────────────────


def _apply_full_bsod_info(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: show full BSOD crash parameters")
    SESSION.backup([_CRASH_CTL], "BsodDisplayParams")
    SESSION.set_dword(_CRASH_CTL, "DisplayParameters", 1)


def _remove_full_bsod_info(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CRASH_CTL, "DisplayParameters")


def _detect_full_bsod_info() -> bool:
    return SESSION.read_dword(_CRASH_CTL, "DisplayParameters") == 1


# ── Disable Paging Executive ───────────────────────────────────────────────


def _apply_disable_paging_exec(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: disable paging of kernel and drivers to disk")
    SESSION.backup([_MEM_MGMT], "PagingExecutive")
    SESSION.set_dword(_MEM_MGMT, "DisablePagingExecutive", 1)


def _remove_disable_paging_exec(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_MEM_MGMT], "PagingExecutive_Remove")
    SESSION.set_dword(_MEM_MGMT, "DisablePagingExecutive", 0)


def _detect_disable_paging_exec() -> bool:
    return SESSION.read_dword(_MEM_MGMT, "DisablePagingExecutive") == 1


TWEAKS += [
    TweakDef(
        id="boot-disable-auto-reboot",
        label="Disable Auto-Reboot on BSOD",
        category="Boot",
        apply_fn=_apply_disable_auto_reboot,
        remove_fn=_remove_disable_auto_reboot,
        detect_fn=_detect_disable_auto_reboot,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CRASH_CTL],
        description=(
            "Disables automatic reboot after a Blue Screen of Death. "
            "Allows reading the full BSOD error before the system restarts. "
            "Default: Enabled. Recommended: Disabled for debugging."
        ),
        tags=["boot", "bsod", "reboot", "crash", "debugging"],
    ),
    TweakDef(
        id="boot-full-bsod-info",
        label="Show Full BSOD Parameters",
        category="Boot",
        apply_fn=_apply_full_bsod_info,
        remove_fn=_remove_full_bsod_info,
        detect_fn=_detect_full_bsod_info,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CRASH_CTL],
        description=(
            "Shows detailed crash parameters on the Blue Screen of Death. "
            "Displays bug-check code and arguments for troubleshooting. "
            "Default: Hidden. Recommended: Shown for diagnostics."
        ),
        tags=["boot", "bsod", "parameters", "crash", "diagnostics"],
    ),
    TweakDef(
        id="boot-disable-paging-executive",
        label="Disable Paging Executive",
        category="Boot",
        apply_fn=_apply_disable_paging_exec,
        remove_fn=_remove_disable_paging_exec,
        detect_fn=_detect_disable_paging_exec,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_MEM_MGMT],
        description=(
            "Keeps kernel and drivers in physical RAM instead of paging "
            "to disk. Improves system responsiveness on machines with "
            "ample RAM. Default: Paging allowed. Recommended: Disabled."
        ),
        tags=["boot", "paging", "kernel", "memory", "performance"],
    ),
]
