"""Plugin marketplace — discover, validate, and load third-party tweak modules.

This module provides the scaffolding for a future plugin ecosystem
where community members can publish additional tweak packs that integrate
seamlessly with the existing auto-discovery loader.

A plugin pack is a directory (or installed package) containing one or
more ``.py`` files, each exporting a ``TWEAKS: list[TweakDef]`` list.

Directory layout for a plugin pack::

    ~/.regilattice/plugins/
        my_tweaks/
            __init__.py        (optional)
            custom_privacy.py  # exports TWEAKS
            custom_network.py  # exports TWEAKS
            plugin.json        # metadata

``plugin.json`` schema::

    {
      "name": "my_tweaks",
      "version": "0.1.0",
      "author": "Alice",
      "description": "Extra privacy and network tweaks",
      "min_regilattice": "1.0.0"
    }

Usage::

    from regilattice.marketplace import discover_plugins, load_plugin, loaded_plugins

    plugins = discover_plugins()       # list of PluginMeta
    tweaks  = load_plugin(plugins[0])  # list[TweakDef]
"""

from __future__ import annotations

import importlib.util
import json
import sys
from dataclasses import dataclass, field
from pathlib import Path
from types import ModuleType

__all__ = [
    "PluginMeta",
    "discover_plugins",
    "load_plugin",
    "loaded_plugins",
    "plugins_dir",
    "unload_plugin",
]

from . import __version__
from .tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_PLUGINS_DIR = Path.home() / ".regilattice" / "plugins"

# ── Data classes ─────────────────────────────────────────────────────────────


@dataclass(slots=True)
class PluginMeta:
    """Metadata for a discovered plugin pack."""

    name: str
    version: str = "0.0.0"
    author: str = ""
    description: str = ""
    min_regilattice: str = "0.0.0"
    path: Path = field(default_factory=lambda: Path("."))


# ── Version helpers ──────────────────────────────────────────────────────────


def _parse_version(v: str) -> tuple[int, ...]:
    """Parse ``'1.2.3'`` or ``'1.2.3-dev'`` into ``(1, 2, 3)``."""
    parts: list[int] = []
    for segment in v.split("."):
        # Strip pre-release suffixes like "-dev", "-rc1"
        numeric = segment.split("-")[0]
        try:
            parts.append(int(numeric))
        except ValueError:
            break
    return tuple(parts) or (0,)


def _version_ok(required: str) -> bool:
    """Return *True* if the current RegiLattice version satisfies *required*."""
    return _parse_version(__version__) >= _parse_version(required)


# ── Discovery ────────────────────────────────────────────────────────────────

_loaded: dict[str, list[TweakDef]] = {}


def plugins_dir() -> Path:
    """Return the plugin directory path (creating it if absent)."""
    _PLUGINS_DIR.mkdir(parents=True, exist_ok=True)
    return _PLUGINS_DIR


def discover_plugins() -> list[PluginMeta]:
    """Scan ``~/.regilattice/plugins/`` for plugin packs."""
    result: list[PluginMeta] = []
    root = plugins_dir()
    for child in sorted(root.iterdir()):
        if not child.is_dir() or child.name.startswith(("_", ".")):
            continue
        meta_file = child / "plugin.json"
        if meta_file.exists():
            try:
                raw = json.loads(meta_file.read_text(encoding="utf-8"))
                result.append(
                    PluginMeta(
                        name=raw.get("name", child.name),
                        version=raw.get("version", "0.0.0"),
                        author=raw.get("author", ""),
                        description=raw.get("description", ""),
                        min_regilattice=raw.get("min_regilattice", "0.0.0"),
                        path=child,
                    )
                )
            except (json.JSONDecodeError, OSError):
                continue
        else:
            # Bare directory with .py files — treat as unnamed plugin
            py_files = list(child.glob("*.py"))
            if py_files:
                result.append(PluginMeta(name=child.name, path=child))
    return result


def load_plugin(meta: PluginMeta) -> list[TweakDef]:
    """Load all tweaks from a single plugin pack.

    Returns a list of :class:`TweakDef` instances.
    Raises :class:`RuntimeError` if the plugin requires a newer RegiLattice.
    """
    if not _version_ok(meta.min_regilattice):
        msg = f"Plugin {meta.name!r} requires RegiLattice >= {meta.min_regilattice} (have {__version__})"
        raise RuntimeError(msg)

    # Security: ensure plugin path is within the plugins directory (prevent path traversal)
    plugins_root = _PLUGINS_DIR.resolve()
    plugin_path = meta.path.resolve()
    try:
        plugin_path.relative_to(plugins_root)
    except ValueError:
        msg = f"Plugin path {str(meta.path)!r} is outside the plugins directory {str(_PLUGINS_DIR)!r}"
        raise RuntimeError(msg) from None

    tweaks: list[TweakDef] = []
    for py_file in sorted(meta.path.glob("*.py")):
        if py_file.name.startswith("_"):
            continue
        mod_name = f"regilattice_plugin_{meta.name}_{py_file.stem}"
        spec = importlib.util.spec_from_file_location(mod_name, py_file)
        if spec is None or spec.loader is None:
            continue
        mod: ModuleType = importlib.util.module_from_spec(spec)
        sys.modules[mod_name] = mod
        spec.loader.exec_module(mod)
        mod_tweaks = getattr(mod, "TWEAKS", None)
        if isinstance(mod_tweaks, list):
            tweaks.extend(mod_tweaks)

    _loaded[meta.name] = tweaks
    return tweaks


def loaded_plugins() -> dict[str, list[TweakDef]]:
    """Return all currently loaded plugin packs and their tweaks."""
    return dict(_loaded)


def unload_plugin(name: str) -> bool:
    """Remove a loaded plugin from the cache. Returns True if found."""
    return _loaded.pop(name, None) is not None
