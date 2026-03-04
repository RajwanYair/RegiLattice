"""Plugin-based tweak registry — auto-discovers TweakDef instances.

Every sub-module in this package exports a ``TWEAKS`` list of
:class:`TweakDef` instances.  The loader collects them all on import
so that the GUI, CLI, and menu can iterate over a single flat list.
"""

from __future__ import annotations

import importlib
import json
import pkgutil
from collections import OrderedDict
from concurrent.futures import ThreadPoolExecutor, as_completed
from dataclasses import dataclass, field
from pathlib import Path
from typing import Callable, Dict, List, Optional

# ── Core descriptor ──────────────────────────────────────────────────────────


@dataclass
class TweakDef:
    """Declarative description of a single reversible registry tweak."""

    id: str
    label: str
    category: str
    apply_fn: Callable[..., None]
    remove_fn: Callable[..., None]
    detect_fn: Optional[Callable[[], bool]] = None
    needs_admin: bool = True
    corp_safe: bool = False
    registry_keys: list[str] = field(default_factory=list)
    description: str = ""
    tags: list[str] = field(default_factory=list)


# ── Plugin loader ────────────────────────────────────────────────────────────

_ALL_TWEAKS: List[TweakDef] = []


def _load_plugins() -> None:
    """Import every sibling module and collect TWEAKS lists."""
    _ALL_TWEAKS.clear()
    seen_ids: set[str] = set()
    package = importlib.import_module(__package__)  # type: ignore[arg-type]
    for _finder, name, _ispkg in pkgutil.iter_modules(package.__path__):
        mod = importlib.import_module(f"{__package__}.{name}")
        tweaks_list: list[TweakDef] = getattr(mod, "TWEAKS", [])
        for td in tweaks_list:
            if td.id in seen_ids:
                raise ValueError(f"Duplicate TweakDef id: {td.id!r}")
            seen_ids.add(td.id)
            _ALL_TWEAKS.append(td)
    # Sort by category (alphabetical), then by label within each category
    _ALL_TWEAKS.sort(key=lambda t: (t.category.lower(), t.label.lower()))


_load_plugins()


def all_tweaks() -> List[TweakDef]:
    """Return every registered tweak (read-only view)."""
    return list(_ALL_TWEAKS)


def get_tweak(tweak_id: str) -> TweakDef | None:
    """Look up a tweak by id."""
    for td in _ALL_TWEAKS:
        if td.id == tweak_id:
            return td
    return None


def reload_plugins() -> None:
    """Re-scan sub-modules (useful after hot-adding a plugin)."""
    _load_plugins()


def categories() -> List[str]:
    """Return sorted unique category names."""
    return list(OrderedDict.fromkeys(td.category for td in _ALL_TWEAKS))


def tweaks_by_category() -> Dict[str, List[TweakDef]]:
    """Return tweaks grouped into ``{category: [TweakDef, ...]}``, sorted."""
    groups: Dict[str, List[TweakDef]] = OrderedDict()
    for td in _ALL_TWEAKS:
        groups.setdefault(td.category, []).append(td)
    return groups


def search_tweaks(query: str) -> List[TweakDef]:
    """Filter tweaks by a case-insensitive query matching id/label/category/tags."""
    q = query.lower()
    results: List[TweakDef] = []
    for td in _ALL_TWEAKS:
        fields = [td.id, td.label, td.category, td.description] + td.tags
        if any(q in f.lower() for f in fields):
            results.append(td)
    return results


# ── Status helpers ───────────────────────────────────────────────────────────


def tweak_status(td: TweakDef) -> str:
    """Return ``'applied'``, ``'not applied'``, or ``'unknown'``."""
    if td.detect_fn is None:
        return "unknown"
    try:
        return "applied" if td.detect_fn() else "not applied"
    except Exception:
        return "unknown"


def status_map() -> Dict[str, str]:
    """Return ``{tweak_id: status}`` for every registered tweak."""
    return {td.id: tweak_status(td) for td in _ALL_TWEAKS}


# ── Snapshot / Undo ──────────────────────────────────────────────────────────


def save_snapshot(path: Path) -> None:
    """Persist the current status of all tweaks to *path* (JSON)."""
    data = status_map()
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(json.dumps(data, indent=2), encoding="utf-8")


def load_snapshot(path: Path) -> Dict[str, str]:
    """Read a previously-saved snapshot."""
    return json.loads(path.read_text(encoding="utf-8"))  # type: ignore[no-any-return]


def restore_snapshot(
    path: Path,
    *,
    force_corp: bool = False,
    require_admin: bool = True,
) -> Dict[str, str]:
    """Compare *path* against the live status and revert changed tweaks.

    Returns a dict of ``{tweak_id: action}`` where action is
    ``'applied'``, ``'removed'``, or ``'skipped'``.
    """
    saved = load_snapshot(path)
    results: Dict[str, str] = {}
    for td in _ALL_TWEAKS:
        saved_state = saved.get(td.id)
        if saved_state is None:
            continue
        current = tweak_status(td)
        if current == saved_state:
            results[td.id] = "unchanged"
            continue
        if not force_corp and not td.corp_safe:
            from regilattice.corpguard import is_corporate_network

            if is_corporate_network():
                results[td.id] = "skipped (corp)"
                continue
        try:
            if saved_state == "applied":
                td.apply_fn(require_admin=require_admin)
                results[td.id] = "applied"
            else:
                td.remove_fn(require_admin=require_admin)
                results[td.id] = "removed"
        except Exception:
            results[td.id] = "error"
    return results


# ── Batch operations ─────────────────────────────────────────────────────────


def apply_all(
    *,
    force_corp: bool = False,
    require_admin: bool = True,
    parallel: bool = False,
    max_workers: int = 4,
    progress_cb: Optional[Callable[[str, str], None]] = None,
) -> Dict[str, str]:
    """Apply every registered tweak, respecting corp-safe flags.

    Parameters
    ----------
    parallel:
        If True, apply tweaks concurrently (non-admin tweaks only when
        ``require_admin`` is True and process is not elevated).
    max_workers:
        Thread count for parallel mode.
    progress_cb:
        Optional callback ``(tweak_id, result)`` invoked after each tweak.
    """
    results: Dict[str, str] = {}

    def _do(td: TweakDef) -> tuple[str, str]:
        if not force_corp and not td.corp_safe:
            from regilattice.corpguard import is_corporate_network

            if is_corporate_network():
                return td.id, "skipped (corp)"
        try:
            td.apply_fn(require_admin=require_admin)
            return td.id, "applied"
        except Exception as exc:
            return td.id, f"error: {exc}"

    if parallel:
        with ThreadPoolExecutor(max_workers=max_workers) as pool:
            futures = {pool.submit(_do, td): td for td in _ALL_TWEAKS}
            for fut in as_completed(futures):
                tid, res = fut.result()
                results[tid] = res
                if progress_cb:
                    progress_cb(tid, res)
    else:
        for td in _ALL_TWEAKS:
            tid, res = _do(td)
            results[tid] = res
            if progress_cb:
                progress_cb(tid, res)
    return results


def remove_all(
    *,
    force_corp: bool = False,
    require_admin: bool = True,
    parallel: bool = False,
    max_workers: int = 4,
    progress_cb: Optional[Callable[[str, str], None]] = None,
) -> Dict[str, str]:
    """Remove every registered tweak, respecting corp-safe flags."""
    results: Dict[str, str] = {}

    def _do(td: TweakDef) -> tuple[str, str]:
        if not force_corp and not td.corp_safe:
            from regilattice.corpguard import is_corporate_network

            if is_corporate_network():
                return td.id, "skipped (corp)"
        try:
            td.remove_fn(require_admin=require_admin)
            return td.id, "removed"
        except Exception as exc:
            return td.id, f"error: {exc}"

    if parallel:
        with ThreadPoolExecutor(max_workers=max_workers) as pool:
            futures = {pool.submit(_do, td): td for td in _ALL_TWEAKS}
            for fut in as_completed(futures):
                tid, res = fut.result()
                results[tid] = res
                if progress_cb:
                    progress_cb(tid, res)
    else:
        for td in _ALL_TWEAKS:
            tid, res = _do(td)
            results[tid] = res
            if progress_cb:
                progress_cb(tid, res)
    return results
