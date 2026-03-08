"""Smart dependency management for RegiLattice.

Provides utilities to lazily import optional packages and auto-install
them when missing, with cascading fallback strategies:

1. User-space pip install (``--user``)
2. System-wide pip install (requires admin)
3. ``ensurepip`` bootstrap + retry
4. Graceful degradation with a stub that raises on use

Usage::

    from regilattice.deps import lazy_import
    requests = lazy_import("requests")        # auto-installs if missing
    rich = lazy_import("rich", pip_name="rich[all]")
"""

from __future__ import annotations

import importlib
import logging
import subprocess
import sys
from types import ModuleType

__all__ = [
    "install_package",
    "lazy_import",
    "require",
]

log = logging.getLogger(__name__)


# ── Pip installer helpers ────────────────────────────────────────────────────


def _pip_install(package: str, *, user: bool = False) -> bool:
    """Run ``pip install`` and return True on success."""
    cmd = [sys.executable, "-m", "pip", "install"]
    if user:
        cmd.append("--user")
    cmd.append(package)
    try:
        result = subprocess.run(
            cmd,
            capture_output=True,
            text=True,
            timeout=120,
        )
        if result.returncode == 0:
            log.info("Installed %s via pip%s", package, " (--user)" if user else "")
            return True
        log.debug("pip install failed (rc=%d): %s", result.returncode, result.stderr[:200])
    except (subprocess.TimeoutExpired, FileNotFoundError, OSError) as exc:
        log.debug("pip install error: %s", exc)
    return False


def _ensure_pip() -> bool:
    """Bootstrap pip via ``ensurepip`` if it is missing."""
    try:
        importlib.import_module("pip")
        return True
    except ImportError:
        pass
    try:
        import ensurepip

        ensurepip.bootstrap(upgrade=True)
        log.info("Bootstrapped pip via ensurepip")
        return True
    except Exception as exc:
        log.debug("ensurepip failed: %s", exc)
    return False


def install_package(pip_name: str) -> bool:
    """Try to install *pip_name* using cascading strategies.

    1. User-space install (``pip install --user <pkg>``)
    2. System-wide install (``pip install <pkg>``)
    3. Ensure pip exists, then retry user-space
    """
    # Strategy 1: user-space
    if _pip_install(pip_name, user=True):
        return True

    # Strategy 2: system-wide (needs admin on some systems)
    if _pip_install(pip_name, user=False):
        return True

    # Strategy 3: bootstrap pip then retry
    if _ensure_pip():
        if _pip_install(pip_name, user=True):
            return True
        if _pip_install(pip_name, user=False):
            return True

    log.warning("Could not install %s with any strategy", pip_name)
    return False


# ── Lazy import ──────────────────────────────────────────────────────────────


class _MissingSentinel:
    """Placeholder for a package that couldn't be imported or installed."""

    def __init__(self, name: str) -> None:
        self._name = name

    def __getattr__(self, item: str) -> None:
        raise ImportError(f"Optional dependency '{self._name}' is not available. Install it manually: pip install {self._name}")


def lazy_import(
    module_name: str,
    *,
    pip_name: str | None = None,
    auto_install: bool = True,
) -> ModuleType:
    """Import *module_name*, auto-installing via pip if missing.

    Parameters
    ----------
    module_name:
        The importable Python module name (e.g. ``"requests"``).
    pip_name:
        The pip package name if different from *module_name*
        (e.g. ``"Pillow"`` for ``import PIL``).
    auto_install:
        If True (default), attempt to install the package when not found.

    Returns
    -------
    The imported module, or a sentinel that raises ``ImportError`` on use
    if installation failed.
    """
    try:
        return importlib.import_module(module_name)
    except ImportError:
        pass

    if auto_install:
        target = pip_name or module_name
        if install_package(target):
            try:
                return importlib.import_module(module_name)
            except ImportError:
                pass

    # Return a sentinel object so that downstream code can still reference
    # the name — it will raise a clear error when actually used.
    return _MissingSentinel(module_name)  # type: ignore[return-value]


def require(*packages: str) -> None:
    """Ensure all listed packages are importable, installing if needed.

    Raises ``ImportError`` if any package cannot be installed.

    Usage::

        from regilattice.deps import require
        require("rich", "requests")
    """
    for pkg in packages:
        try:
            importlib.import_module(pkg)
        except ImportError:
            if not install_package(pkg):
                raise ImportError(
                    f"Required package '{pkg}' is not available and could not be installed automatically. Run: pip install {pkg}"
                ) from None
