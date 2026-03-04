"""Registry helpers for RegiLattice.

Wraps ``winreg`` (and ``reg.exe`` as a fallback) so every tweak can be
applied from Python with proper backup, logging, and error handling.
"""

from __future__ import annotations

import os
import platform
import re
import subprocess
from dataclasses import dataclass, field
from datetime import datetime
from pathlib import Path
from typing import Iterable, Optional, Tuple

if os.name == "nt":
    import winreg
else:  # pragma: no cover — guard for non-Windows CI
    winreg = None  # type: ignore[assignment]

# ── Root-hive mapping ────────────────────────────────────────────────────────

_ROOTS: dict[str, int | None] = {}

if winreg is not None:
    _ROOTS = {
        "HKEY_CLASSES_ROOT": winreg.HKEY_CLASSES_ROOT,
        "HKCR": winreg.HKEY_CLASSES_ROOT,
        "HKEY_CURRENT_USER": winreg.HKEY_CURRENT_USER,
        "HKCU": winreg.HKEY_CURRENT_USER,
        "HKEY_LOCAL_MACHINE": winreg.HKEY_LOCAL_MACHINE,
        "HKLM": winreg.HKEY_LOCAL_MACHINE,
    }


# ── Helpers ──────────────────────────────────────────────────────────────────


def _ensure_windows() -> None:
    if os.name != "nt":  # pragma: no cover — platform guard
        raise OSError("Registry tweaks require Windows.")


def _split_root(path: str) -> Tuple[int, str]:
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


@dataclass
class RegistrySession:
    """Encapsulates backup, log, and registry-write operations."""

    base_dir: Path
    _dry_run: bool = field(default=False, repr=False)

    # -- Logging --

    @property
    def log_path(self) -> Path:
        return self.base_dir / "RegiLattice.log"

    def log(self, message: str) -> None:
        ts = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        with self.log_path.open("a", encoding="utf-8") as fh:
            fh.write(f"{ts} : {message}\n")

    # -- Backup --

    def backup(self, keys: Iterable[str], label: str) -> Path:
        """Export registry keys via ``reg.exe``.

        Prefers *OneDrive/RegistryBackups*; falls back to *~/Documents*.
        """
        _ensure_windows()
        ts = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
        onedrive = os.environ.get("OneDrive", "")
        backup_root = (
            Path(onedrive) / "RegistryBackups"
            if onedrive and Path(onedrive).is_dir()
            else Path.home() / "Documents" / "RegistryBackups"
        )
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

    # -- Registry write primitives --

    def set_value(
        self,
        path: str,
        name: Optional[str],
        value: object,
        reg_type: int,
    ) -> None:
        """Create/open the key and set *name* to *value*."""
        if self._dry_run:
            self.log(f"[DRY-RUN] set_value {path} {name!r} = {value!r}")
            return
        root, subkey = _split_root(path)
        with winreg.CreateKey(root, subkey) as handle:  # type: ignore[arg-type]
            winreg.SetValueEx(handle, name or "", 0, reg_type, value)  # type: ignore[arg-type]

    def set_dword(self, path: str, name: str, value: int) -> None:
        self.set_value(path, name, value, winreg.REG_DWORD)  # type: ignore[union-attr]

    def set_string(self, path: str, name: Optional[str], value: str) -> None:
        self.set_value(path, name, value, winreg.REG_SZ)  # type: ignore[union-attr]

    # -- Registry delete primitives --

    def delete_tree(self, path: str) -> None:
        """Recursively delete a registry key and all its children."""
        if self._dry_run:
            self.log(f"[DRY-RUN] delete_tree {path}")
            return
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
            self.log(f"[DRY-RUN] delete_value {path} {name!r}")
            return
        root, subkey = _split_root(path)
        try:
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_SET_VALUE) as handle:  # type: ignore[arg-type]
                winreg.DeleteValue(handle, name)
        except FileNotFoundError:
            return
        except OSError:
            subprocess.run(["reg", "delete", path, "/v", name, "/f"], check=False)

    # -- Query helpers --

    def key_exists(self, path: str) -> bool:
        """Return True when the registry key exists."""
        _ensure_windows()
        try:
            root, subkey = _split_root(path)
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_READ):  # type: ignore[arg-type]
                return True
        except (FileNotFoundError, OSError):
            return False

    def read_dword(self, path: str, name: str) -> Optional[int]:
        """Return a DWORD value or None if missing."""
        _ensure_windows()
        try:
            root, subkey = _split_root(path)
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_READ) as handle:  # type: ignore[arg-type]
                val, typ = winreg.QueryValueEx(handle, name)
                return int(val) if typ == winreg.REG_DWORD else None  # type: ignore[union-attr]
        except (FileNotFoundError, OSError):
            return None

    def read_string(self, path: str, name: str) -> Optional[str]:
        """Return a REG_SZ value or None if missing."""
        _ensure_windows()
        try:
            root, subkey = _split_root(path)
            with winreg.OpenKey(root, subkey, 0, winreg.KEY_READ) as handle:  # type: ignore[arg-type]
                val, typ = winreg.QueryValueEx(handle, name)
                return str(val) if typ == winreg.REG_SZ else None  # type: ignore[union-attr]
        except (FileNotFoundError, OSError):
            return None


# ── Admin check ──────────────────────────────────────────────────────────────


class AdminRequirementError(PermissionError):
    """Raised when administrative rights are required."""


def assert_admin(required: bool = True) -> None:
    """Raise ``AdminRequirementError`` when elevation is needed but absent."""
    if not required:
        return
    _ensure_windows()
    import ctypes

    if not bool(ctypes.windll.shell32.IsUserAnAdmin()):  # type: ignore[attr-defined]
        raise AdminRequirementError(
            "Administrator privileges are required for this operation."
        )


# ── Module-level singleton ───────────────────────────────────────────────────

BASE_DIR = Path(__file__).resolve().parent.parent
SESSION = RegistrySession(base_dir=BASE_DIR)
