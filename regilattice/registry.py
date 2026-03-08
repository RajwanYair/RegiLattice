"""Registry helpers for RegiLattice.

Wraps ``winreg`` (and ``reg.exe`` as a fallback) so every tweak can be
applied from Python with proper backup, logging, and error handling.
"""

from __future__ import annotations

import os
import platform
import re
import subprocess
import threading
import time
from collections.abc import Iterable
from dataclasses import dataclass, field
from datetime import datetime
from pathlib import Path

if os.name == "nt":
    import winreg
else:  # pragma: no cover — guard for non-Windows CI
    winreg = None  # type: ignore[assignment]

__all__ = [
    "BASE_DIR",
    "SESSION",
    "AdminRequirementError",
    "RegistrySession",
    "assert_admin",
    "is_windows",
    "platform_summary",
]

# ── Root-hive mapping ────────────────────────────────────────────────────────

_ROOTS: dict[str, int | None] = {}

# Pre-sorted prefix list: longest prefix first so the first match is always correct
_PREFIX_LIST: list[tuple[str, int]] = []  # (prefix_upper, root_handle) — built at import

if winreg is not None:
    _ROOTS = {
        "HKEY_CLASSES_ROOT": winreg.HKEY_CLASSES_ROOT,
        "HKCR": winreg.HKEY_CLASSES_ROOT,
        "HKEY_CURRENT_USER": winreg.HKEY_CURRENT_USER,
        "HKCU": winreg.HKEY_CURRENT_USER,
        "HKEY_LOCAL_MACHINE": winreg.HKEY_LOCAL_MACHINE,
        "HKLM": winreg.HKEY_LOCAL_MACHINE,
    }
    # Sort longest prefix first (“HKEY_CLASSES_ROOT” before “HKCR”) for unambiguous matching
    _PREFIX_LIST = sorted(
        ((p.upper(), r) for p, r in _ROOTS.items() if r is not None),
        key=lambda t: -len(t[0]),
    )


# ── Helpers ──────────────────────────────────────────────────────────────────


def _ensure_windows() -> None:
    if os.name != "nt":  # pragma: no cover — platform guard
        raise OSError("Registry tweaks require Windows.")


# Transient WinError codes that merit a single retry
_TRANSIENT_WINERRORS = {5, 6}  # ERROR_ACCESS_DENIED, ERROR_INVALID_HANDLE


def _retry_on_transient(fn: object, *args: object, **kwargs: object) -> object:
    """Call *fn* once; on transient OSError, wait 0.2 s and retry once."""
    try:
        return fn(*args, **kwargs)  # type: ignore[operator]
    except OSError as exc:
        if getattr(exc, "winerror", None) not in _TRANSIENT_WINERRORS:
            raise
        time.sleep(0.2)
        return fn(*args, **kwargs)  # type: ignore[operator]


def _split_root(path: str) -> tuple[int, str]:
    """Split a registry path into (root-handle, subkey)."""
    _ensure_windows()
    for prefix, root in _ROOTS.items():
        if path.upper().startswith(prefix.upper() + "\\") and root is not None:
            return root, path[len(prefix) + 1 :]
    raise ValueError(f"Unsupported registry path: {path}")


def is_windows() -> bool:
    return os.name == "nt"


def platform_summary() -> str:
    return f"{platform.system()} {platform.release()} ({platform.version()})"


# ── Session ──────────────────────────────────────────────────────────────────


class _ReadCacheContext:
    """Context manager that enables/disables the read cache on a RegistrySession."""

    __slots__ = ("_session",)

    def __init__(self, session: RegistrySession) -> None:
        self._session = session

    def __enter__(self) -> _ReadCacheContext:
        self._session.enable_read_cache()
        return self

    def __exit__(self, *exc: object) -> None:
        self._session.disable_read_cache()


@dataclass
class RegistrySession:
    """Encapsulates backup, log, and registry-write operations."""

    base_dir: Path
    _dry_run: bool = field(default=False, repr=False)
    _dry_ops: int = field(default=0, repr=False)
    _read_cache_enabled: bool = field(default=False, repr=False)
    _read_cache: dict[tuple[str, str, str], object] = field(default_factory=dict, repr=False)
    _read_cache_lock: threading.Lock = field(default_factory=threading.Lock, repr=False)

    def __post_init__(self) -> None:
        self._log_path = self.base_dir / "RegiLattice.log"

    # -- Read cache --

    def enable_read_cache(self) -> None:
        """Enable per-session read cache (use during detect cycles)."""
        with self._read_cache_lock:
            self._read_cache_enabled = True
            self._read_cache.clear()

    def disable_read_cache(self) -> None:
        """Disable and clear the read cache."""
        with self._read_cache_lock:
            self._read_cache_enabled = False
            self._read_cache.clear()

    def read_cache(self) -> _ReadCacheContext:
        """Context manager for scoped read cache.

        Usage::

            with SESSION.read_cache():
                # all reads within this block are cached
                val = SESSION.read_dword(path, name)
        """
        return _ReadCacheContext(self)

    def _invalidate_cache_for(self, path: str, name: str) -> None:
        """Remove cached entries for a specific path/name after a write."""
        if not self._read_cache_enabled:
            return
        with self._read_cache_lock:
            for suffix in ("dword", "string", "exists"):
                self._read_cache.pop((path, name, suffix), None)
            # Also invalidate key_exists for the path
            self._read_cache.pop((path, "", "exists"), None)

    # -- Logging --

    @property
    def log_path(self) -> Path:
        return self._log_path

    @property
    def dry_run(self) -> bool:
        """Whether the session is in dry-run mode (no actual registry writes)."""
        return self._dry_run

    @dry_run.setter
    def dry_run(self, value: bool) -> None:
        self._dry_run = value

    @property
    def dry_ops(self) -> int:
        """Number of registry operations skipped in dry-run mode."""
        return self._dry_ops

    def log(self, message: str) -> None:
        ts = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        with self._log_path.open("a", encoding="utf-8") as fh:
            fh.write(f"{ts} : {message}\n")

    # -- Backup --

    def backup(self, keys: Iterable[str], label: str) -> Path:
        """Export registry keys via ``reg.exe``.

        Prefers *OneDrive/RegistryBackups*; falls back to *~/Documents*.
        """
        if self._dry_run:
            self._dry_ops += 1
            self.log(f"[DRY-RUN] backup {label}")
            return self.base_dir / f"backup_{label}"
        _ensure_windows()
        ts = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
        onedrive = os.environ.get("ONEDRIVE", "")
        backup_root = Path(onedrive) / "RegistryBackups" if onedrive and Path(onedrive).is_dir() else Path.home() / "Documents" / "RegistryBackups"
        backup_path = backup_root / f"{label}_{ts}"
        backup_path.mkdir(parents=True, exist_ok=True)

        for key in keys:
            safe = re.sub(r'[\\:*{}<>|"]', "_", key)
            reg_file = backup_path / f"{safe}.reg"
            try:
                subprocess.run(
                    ["reg", "export", key, str(reg_file), "/y"],
                    check=True,
                    capture_output=True,
                    text=True,
                )
            except subprocess.CalledProcessError as exc:
                self.log(f"Backup warning for {key}: {exc.stderr.strip()}")

        self.log(f"Backup saved at: {backup_path}")
        return backup_path

    def restore_backup(self, backup_path: Path) -> bool:
        """Import all ``.reg`` files from *backup_path*.

        Returns ``True`` if at least one file was successfully imported.
        """
        if self._dry_run:
            self._dry_ops += 1
            self.log(f"[DRY-RUN] restore_backup {backup_path}")
            return True
        _ensure_windows()
        imported = False
        for reg_file in sorted(backup_path.glob("*.reg")):
            try:
                subprocess.run(
                    ["reg", "import", str(reg_file)],
                    check=True,
                    capture_output=True,
                    text=True,
                )
                imported = True
            except subprocess.CalledProcessError as exc:
                self.log(f"Restore warning for {reg_file.name}: {exc.stderr.strip()}")
        if imported:
            self.log(f"Restored backup from: {backup_path}")
        return imported

    # -- Registry write primitives --

    def set_value(
        self,
        path: str,
        name: str | None,
        value: object,
        reg_type: int,
    ) -> None:
        """Create/open the key and set *name* to *value*."""
        if self._dry_run:
            self._dry_ops += 1
            self.log(f"[DRY-RUN] set_value {path} {name!r} = {value!r}")
            return
        # Invalidate read cache for this path/name
        self._invalidate_cache_for(path, name or "")
        root, subkey = _split_root(path)

        def _do_write() -> None:
            with winreg.CreateKey(root, subkey) as handle:
                winreg.SetValueEx(handle, name or "", 0, reg_type, value)  # type: ignore[call-overload]

        _retry_on_transient(_do_write)

    def set_dword(self, path: str, name: str, value: int) -> None:
        self.set_value(path, name, value, winreg.REG_DWORD)

    def set_string(self, path: str, name: str | None, value: str) -> None:
        self.set_value(path, name, value, winreg.REG_SZ)

    # -- Registry delete primitives --

    def delete_tree(self, path: str) -> None:
        """Recursively delete a registry key and all its children."""
        if self._dry_run:
            self._dry_ops += 1
            self.log(f"[DRY-RUN] delete_tree {path}")
            return
        # Clear all cache entries under this path
        if self._read_cache_enabled:
            to_remove = [k for k in self._read_cache if k[0].startswith(path)]
            for k in to_remove:
                del self._read_cache[k]
        root, subkey = _split_root(path)
        self._delete_subkey_tree(root, subkey)

    @staticmethod
    def _delete_subkey_tree(root: int, subkey: str) -> None:
        """Walk children depth-first, then remove the parent."""
        assert winreg is not None
        try:
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_READ) as handle:
                children: list[str] = []
                idx = 0
                while True:
                    try:
                        children.append(winreg.EnumKey(handle, idx))
                        idx += 1
                    except OSError:
                        break
            for child in children:
                RegistrySession._delete_subkey_tree(root, f"{subkey}\\{child}")
            winreg.DeleteKey(root, subkey)
        except FileNotFoundError:
            pass
        except OSError:
            # Last resort — reg.exe for locked keys
            subprocess.run(["reg", "delete", f"{root}\\{subkey}", "/f"], check=False)

    def delete_value(self, path: str, name: str) -> None:
        """Delete a single value from a key."""
        if self._dry_run:
            self._dry_ops += 1
            self.log(f"[DRY-RUN] delete_value {path} {name!r}")
            return
        self._invalidate_cache_for(path, name)
        root, subkey = _split_root(path)

        def _do_delete() -> None:
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_SET_VALUE) as handle:
                winreg.DeleteValue(handle, name)

        try:
            _retry_on_transient(_do_delete)
        except FileNotFoundError:
            return
        except OSError:
            subprocess.run(["reg", "delete", path, "/v", name, "/f"], check=False)

    # -- Query helpers --

    def key_exists(self, path: str) -> bool:
        """Return True when the registry key exists."""
        _ensure_windows()
        cache_key = (path, "", "exists")
        with self._read_cache_lock:
            if self._read_cache_enabled and cache_key in self._read_cache:
                return self._read_cache[cache_key]  # type: ignore[return-value]
        try:
            root, subkey = _split_root(path)
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_READ):
                result = True
        except (FileNotFoundError, OSError):
            result = False
        with self._read_cache_lock:
            if self._read_cache_enabled:
                self._read_cache[cache_key] = result
        return result

    def read_dword(self, path: str, name: str) -> int | None:
        """Return a DWORD value or None if missing."""
        _ensure_windows()
        cache_key = (path, name, "dword")
        with self._read_cache_lock:
            if self._read_cache_enabled and cache_key in self._read_cache:
                return self._read_cache[cache_key]  # type: ignore[return-value]
        try:
            root, subkey = _split_root(path)
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_READ) as handle:
                val, typ = winreg.QueryValueEx(handle, name)
                result = int(val) if typ == winreg.REG_DWORD else None
        except (FileNotFoundError, OSError):
            result = None
        with self._read_cache_lock:
            if self._read_cache_enabled:
                self._read_cache[cache_key] = result
        return result

    def read_string(self, path: str, name: str) -> str | None:
        """Return a REG_SZ value or None if missing."""
        _ensure_windows()
        cache_key = (path, name, "string")
        with self._read_cache_lock:
            if self._read_cache_enabled and cache_key in self._read_cache:
                return self._read_cache[cache_key]  # type: ignore[return-value]
        try:
            root, subkey = _split_root(path)
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_READ) as handle:
                val, typ = winreg.QueryValueEx(handle, name)
                result = str(val) if typ == winreg.REG_SZ else None
        except (FileNotFoundError, OSError):
            result = None
        with self._read_cache_lock:
            if self._read_cache_enabled:
                self._read_cache[cache_key] = result
        return result

    def read_binary(self, path: str, name: str) -> bytes | None:
        """Return a REG_BINARY value as bytes, or None if missing."""
        _ensure_windows()
        cache_key = (path, name, "binary")
        with self._read_cache_lock:
            if self._read_cache_enabled and cache_key in self._read_cache:
                return self._read_cache[cache_key]  # type: ignore[return-value]
        try:
            root, subkey = _split_root(path)
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_READ) as handle:
                val, typ = winreg.QueryValueEx(handle, name)
                result = bytes(val) if typ == winreg.REG_BINARY else None
        except (FileNotFoundError, OSError):
            result = None
        with self._read_cache_lock:
            if self._read_cache_enabled:
                self._read_cache[cache_key] = result
        return result

    def read_qword(self, path: str, name: str) -> int | None:
        """Return a REG_QWORD (64-bit int) value, or None if missing."""
        _ensure_windows()
        cache_key = (path, name, "qword")
        with self._read_cache_lock:
            if self._read_cache_enabled and cache_key in self._read_cache:
                return self._read_cache[cache_key]  # type: ignore[return-value]
        try:
            root, subkey = _split_root(path)
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_READ) as handle:
                val, typ = winreg.QueryValueEx(handle, name)
                result = int(val) if typ == winreg.REG_QWORD else None
        except (FileNotFoundError, OSError):
            result = None
        with self._read_cache_lock:
            if self._read_cache_enabled:
                self._read_cache[cache_key] = result
        return result

    def set_binary(self, path: str, name: str, data: bytes) -> None:
        """Write a REG_BINARY value."""
        self.set_value(path, name, data, winreg.REG_BINARY)

    def set_qword(self, path: str, name: str, value: int) -> None:
        """Write a REG_QWORD (64-bit integer) value."""
        self.set_value(path, name, value, winreg.REG_QWORD)

    def list_values(self, path: str) -> list[tuple[str, object, int]]:
        """Enumerate all (name, value, type) triples in a key.

        Returns an empty list when the key does not exist.
        """
        _ensure_windows()
        try:
            root, subkey = _split_root(path)
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_READ) as handle:
                result: list[tuple[str, object, int]] = []
                idx = 0
                while True:
                    try:
                        name, val, typ = winreg.EnumValue(handle, idx)
                        result.append((name, val, typ))
                        idx += 1
                    except OSError:
                        break
                return result
        except (FileNotFoundError, OSError, ValueError):
            return []

    def list_keys(self, path: str) -> list[str]:
        """Enumerate child key names immediately under *path*.

        Returns an empty list when the key does not exist.
        """
        _ensure_windows()
        try:
            root, subkey = _split_root(path)
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_READ) as handle:
                result: list[str] = []
                idx = 0
                while True:
                    try:
                        result.append(winreg.EnumKey(handle, idx))
                        idx += 1
                    except OSError:
                        break
                return result
        except (FileNotFoundError, OSError, ValueError):
            return []


# ── Admin check ──────────────────────────────────────────────────────────────


class AdminRequirementError(PermissionError):
    """Raised when administrative rights are required."""


def assert_admin(required: bool = True) -> None:
    """Raise ``AdminRequirementError`` when elevation is needed but absent."""
    if not required:
        return
    _ensure_windows()
    import ctypes

    if not bool(ctypes.windll.shell32.IsUserAnAdmin()):
        raise AdminRequirementError("Administrator privileges are required for this operation.")


# ── Module-level singleton ───────────────────────────────────────────────────

BASE_DIR = Path(__file__).resolve().parent.parent
SESSION = RegistrySession(base_dir=BASE_DIR)
