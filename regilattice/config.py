"""User configuration via ``~/.regilattice.toml``.

Supports Python 3.11+ (``tomllib``) and 3.10 (``tomli`` fallback).

Example config::

    [general]
    force_corp = false
    max_workers = 8

    [backups]
    directory = "~/Documents/RegistryBackups"
    auto_backup = true
"""

from __future__ import annotations

import sys
from dataclasses import dataclass

__all__ = [
    "AppConfig",
    "load_config",
]
from pathlib import Path
from typing import Any

_tomllib: Any = None
if sys.version_info >= (3, 11):
    import tomllib

    _tomllib = tomllib
else:
    try:
        import tomli  # optional fallback for Python <3.11

        _tomllib = tomli
    except ModuleNotFoundError:
        pass

_DEFAULT_PATH = Path.home() / ".regilattice.toml"


@dataclass(slots=True)
class AppConfig:
    """Application-level settings loaded from TOML."""

    force_corp: bool = False
    max_workers: int = 8
    backup_dir: str = ""
    auto_backup: bool = True
    theme: str = "system"
    locale: str = "en"


def load_config(path: Path | None = None) -> AppConfig:
    """Read and return :class:`AppConfig` from *path* (or the default)."""
    path = path or _DEFAULT_PATH
    cfg = AppConfig()
    if not path.is_file():
        return cfg
    if _tomllib is None:
        return cfg  # no TOML parser available
    with path.open("rb") as fh:
        data: dict[str, Any] = _tomllib.load(fh)
    general = data.get("general", {})
    if isinstance(general, dict):
        if isinstance(general.get("force_corp"), bool):
            cfg.force_corp = general["force_corp"]
        if isinstance(general.get("max_workers"), int):
            cfg.max_workers = general["max_workers"]
        if isinstance(general.get("theme"), str):
            cfg.theme = general["theme"]
        if isinstance(general.get("locale"), str):
            cfg.locale = general["locale"]
    backups = data.get("backups", {})
    if isinstance(backups, dict):
        if isinstance(backups.get("directory"), str):
            cfg.backup_dir = backups["directory"]
        if isinstance(backups.get("auto_backup"), bool):
            cfg.auto_backup = backups["auto_backup"]
    return cfg
