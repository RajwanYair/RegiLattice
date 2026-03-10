"""UAC elevation and privilege helpers for RegiLattice.

Provides utilities for:
- Checking if the current process is elevated (admin)
- Re-launching the current script with a UAC prompt
- Running an arbitrary callable in an elevated subprocess
"""

from __future__ import annotations

import ctypes
import os
import subprocess
import sys

__all__ = [
    "ensure_admin_or_elevate",
    "is_admin",
    "request_elevation",
    "run_elevated",
]

from .registry import SESSION

# ── Allowlist for elevated execution ────────────────────────────────────────
# Only Windows system utilities may be executed via run_elevated().
# This prevents accidental elevation of arbitrary programs and limits
# the attack surface if this function is called with untrusted input.

_ALLOWED_ELEVATED_EXECUTABLES: frozenset[str] = frozenset(
    {
        "reg",
        "reg.exe",
        "dism",
        "dism.exe",
        "icacls",
        "icacls.exe",
        "takeown",
        "takeown.exe",
        "netsh",
        "netsh.exe",
        "sc",
        "sc.exe",
        "schtasks",
        "schtasks.exe",
        "taskkill",
        "taskkill.exe",
        "wusa",
        "wusa.exe",
        "powershell",
        "powershell.exe",
        "pwsh",
        "pwsh.exe",
        "cmd",
        "cmd.exe",
        "wmic",
        "wmic.exe",
        "bcdedit",
        "bcdedit.exe",
        "fsutil",
        "fsutil.exe",
        "net",
        "net.exe",
        "netstat",
        "netstat.exe",
    }
)


def is_admin() -> bool:
    """Return True if the current process has administrator privileges."""
    if os.name != "nt":
        _getuid = getattr(os, "getuid", None)
        if _getuid is not None:
            return bool(_getuid() == 0)
        return False
    try:
        return bool(ctypes.windll.shell32.IsUserAnAdmin())
    except Exception:
        return False


def request_elevation(args: list[str] | None = None) -> int:
    """Re-launch the current Python script elevated via ShellExecuteW (UAC).

    Parameters
    ----------
    args:
        Command-line arguments to pass. Defaults to ``sys.argv``.

    Returns
    -------
    int
        The exit code of the elevated process, or 0 if ShellExecute succeeded.
    """
    if is_admin():
        return 0

    if os.name != "nt":
        SESSION.log("Elevation: not supported on non-Windows")
        return 1

    if args is None:
        args = sys.argv

    exe = sys.executable
    params = " ".join(f'"{a}"' for a in args)
    SESSION.log(f"Requesting UAC elevation: {exe} {params}")

    try:
        # ShellExecuteW returns >32 on success
        ret = ctypes.windll.shell32.ShellExecuteW(
            None,
            "runas",
            exe,
            params,
            None,
            1,  # SW_SHOWNORMAL
        )
        return 0 if ret > 32 else int(ret)
    except Exception as exc:
        SESSION.log(f"Elevation failed: {exc}")
        return 1


def run_elevated(command: list[str], *, timeout: int = 120) -> subprocess.CompletedProcess[str]:
    """Run a command list elevated via PowerShell Start-Process -Verb RunAs.

    This is useful for running a subprocess (e.g. ``reg.exe``, ``dism.exe``)
    with admin rights when the current process is not elevated.

    Raises
    ------
    ValueError
        If the executable (``command[0]``) is not in the allowed-commands
        security allowlist.
    """
    if not command:
        raise ValueError("command list must not be empty")

    exe_name = os.path.basename(command[0]).lower()
    if exe_name not in _ALLOWED_ELEVATED_EXECUTABLES:
        raise ValueError(
            f"run_elevated: '{command[0]}' is not in the allowed-executables list. "
            "Add it to _ALLOWED_ELEVATED_EXECUTABLES if it is a trusted system utility."
        )

    if is_admin():
        return subprocess.run(command, capture_output=True, text=True, timeout=timeout)

    # Wrap in PowerShell Start-Process -Verb RunAs -Wait

    ps_cmd = f"Start-Process -FilePath '{command[0]}' -ArgumentList '{' '.join(command[1:])}' -Verb RunAs -Wait -PassThru"
    return subprocess.run(
        ["powershell", "-NoProfile", "-Command", ps_cmd],
        capture_output=True,
        text=True,
        timeout=timeout,
    )


def ensure_admin_or_elevate() -> None:
    """If not admin, re-launch the current script elevated and exit.

    Call this at the very start of a script that requires admin. If elevation
    succeeds, the current (non-admin) process exits with code 0.
    """
    if is_admin():
        return
    code = request_elevation()
    sys.exit(code)
