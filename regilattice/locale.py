"""Lightweight locale / i18n support for RegiLattice.

Provides a thin string-table layer so every user-visible label in the
GUI and CLI lives in one place and can be overridden.

Usage::

    from regilattice.locale import t

    button_text = t("apply_all")          # "Apply All" (en)
    formatted   = t("tweaks_count", n=42) # "42 tweaks"
"""

from __future__ import annotations

import json
from pathlib import Path
from typing import Any

# ── Built-in English strings ────────────────────────────────────────────────

_EN: dict[str, str] = {
    # ── GUI buttons / labels ───────────────────────────────
    "apply_all": "Apply All",
    "remove_all": "Remove All",
    "apply_selected": "Apply Selected",
    "remove_selected": "Remove Selected",
    "invert_selection": "Invert Selection",
    "select_all": "Select All",
    "deselect_all": "Deselect All",
    "search_placeholder": "Search tweaks…",
    "status_applied": "APPLIED",
    "status_default": "DEFAULT",
    "status_unknown": "UNKNOWN",
    "status_error": "ERROR",
    "status_skipped": "SKIPPED",
    # ── GUI filters ────────────────────────────────────────
    "filter_all": "All",
    "filter_applied": "Applied",
    "filter_default": "Default",
    "filter_unknown": "Unknown",
    # ── GUI scope badges ───────────────────────────────────
    "scope_user": "USER",
    "scope_machine": "MACHINE",
    "scope_both": "BOTH",
    # ── GUI dialogs ────────────────────────────────────────
    "about_title": "About RegiLattice",
    "export_title": "Export PowerShell Script",
    "import_title": "Import Tweak Selection (JSON)",
    "scoop_title": "Scoop Tools Manager",
    "confirm_apply": "Apply {n} tweak(s)?",
    "confirm_remove": "Remove {n} tweak(s)?",
    # ── GUI summary bar ────────────────────────────────────
    "summary_applied": "Applied: {n}",
    "summary_default": "Default: {n}",
    "summary_unknown": "Unknown: {n}",
    "summary_recommended": "Recommended: {n}",
    "summary_gpo": "GPO: {n}",
    # ── CLI messages ───────────────────────────────────────
    "cli_applying": "Applying: {label}",
    "cli_removing": "Removing: {label}",
    "cli_done": "Done.",
    "cli_no_tweaks": "No tweaks matched.",
    "cli_profile_info": "Profile '{name}': {desc}",
    "cli_snapshot_saved": "Snapshot saved to {path}",
    # ── Misc ───────────────────────────────────────────────
    "tweaks_count": "{n} tweaks",
    "categories_count": "{n} categories",
    "corp_blocked": "Blocked by corporate policy",
    "admin_required": "Administrator privileges required",
    "build_skipped": "Requires Windows build {build}+",
}

# ── Active locale state ─────────────────────────────────────────────────────

_active: dict[str, str] = dict(_EN)
_state: dict[str, str] = {"locale": "en"}


def set_locale(name: str, overrides: dict[str, str] | None = None) -> None:
    """Switch locale.

    *name* is a locale tag (e.g. ``"en"``, ``"de"``).
    *overrides* is an optional dict of key → translated string that patches
    the built-in English table.  Missing keys fall back to English.
    """
    _state["locale"] = name
    _active.clear()
    _active.update(_EN)
    if overrides:
        _active.update(overrides)


def load_locale_file(path: str | Path) -> None:
    """Load a JSON locale file and apply it.

    The file must be a JSON object ``{ "locale": "xx", "strings": { ... } }``.
    """
    data = json.loads(Path(path).read_text(encoding="utf-8"))
    locale_name: str = data.get("locale", "custom")
    strings: dict[str, str] = data.get("strings", {})
    set_locale(locale_name, strings)


def current_locale() -> str:
    """Return the active locale tag."""
    return _state["locale"]


def available_keys() -> list[str]:
    """Return all known string keys (for tooling / validation)."""
    return sorted(_EN)


def t(key: str, **kwargs: Any) -> str:
    """Translate *key*, formatting with *kwargs* if provided.

    Returns the raw key if it is not in the table (safe fallback).
    """
    template = _active.get(key, key)
    if kwargs:
        return template.format(**kwargs)
    return template
