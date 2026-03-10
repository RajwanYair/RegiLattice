"""Plugin-based tweak registry — auto-discovers TweakDef instances.

Every sub-module in this package exports a ``TWEAKS`` list of
:class:`TweakDef` instances.  The loader collects them all on import
so that the GUI, CLI, and menu can iterate over a single flat list.
"""

from __future__ import annotations

import concurrent.futures
import functools
import importlib
import json
import os
import pkgutil
import platform
import threading
import warnings
from collections import OrderedDict, deque
from collections.abc import Callable, Iterable
from dataclasses import dataclass, field
from enum import Enum
from pathlib import Path

__all__ = [
    "WIN10_21H2",
    "WIN11_21H2",
    "WIN11_22H2",
    "WIN11_23H2",
    "WIN11_24H2",
    "CategoryInfo",
    "ProfileDef",
    "TweakDef",
    "TweakResult",
    "all_category_info",
    "all_tweaks",
    "apply_all",
    "apply_profile",
    "apply_tweaks",
    "available_profiles",
    "categories",
    "categories_by_risk",
    "categories_by_scope",
    "category_counts",
    "category_info",
    "diff_snapshots",
    "filter_tweaks",
    "get_tweak",
    "load_snapshot",
    "profile_info",
    "reload_plugins",
    "remove_all",
    "remove_tweaks",
    "restore_snapshot",
    "save_snapshot",
    "search_tweaks",
    "status_map",
    "tweak_count_by_scope",
    "tweak_dependencies",
    "tweak_risk_level",
    "tweak_scope",
    "tweak_status",
    "tweaks_above_build",
    "tweaks_by_category",
    "tweaks_by_ids",
    "tweaks_by_scope",
    "tweaks_by_tag",
    "tweaks_excluded_by_profile",
    "tweaks_for_profile",
    "windows_build",
]

# ── Tweak result enum ────────────────────────────────────────────────────────


class TweakResult(str, Enum):
    """Typed result from applying, removing, or detecting a tweak."""

    APPLIED = "applied"
    REMOVED = "removed"
    NOT_APPLIED = "not applied"
    UNKNOWN = "unknown"
    UNCHANGED = "unchanged"
    SKIPPED_CORP = "skipped (corp)"
    SKIPPED_ADMIN = "skipped (admin)"
    SKIPPED_BUILD = "skipped (build)"
    ERROR = "error"


# ── Core descriptor ──────────────────────────────────────────────────────────


@dataclass(slots=True)
class TweakDef:
    """Declarative description of a single reversible registry tweak."""

    id: str
    label: str
    category: str
    apply_fn: Callable[..., None]
    remove_fn: Callable[..., None]
    detect_fn: Callable[[], bool] | None = None
    needs_admin: bool = True
    corp_safe: bool = False
    registry_keys: list[str] = field(default_factory=list)
    description: str = ""
    tags: list[str] = field(default_factory=list)
    depends_on: list[str] = field(default_factory=list)
    min_build: int = 0  # 0 = any Windows version, e.g. 22000 = Win 11 21H2+
    side_effects: str = ""  # describes what may break when applied
    source_url: str = ""  # KB article, docs URL, or community reference for this tweak


# ── Category metadata ────────────────────────────────────────────────────────


@dataclass(slots=True)
class CategoryInfo:
    """Metadata describing a tweak category for rich UI and documentation."""

    name: str
    icon: str = ""
    description: str = ""
    risk_level: str = "low"  # low | medium | high
    scope: str = "mixed"  # user | machine | mixed
    profiles: list[str] = field(default_factory=list)


# Auto-populated category metadata (derived from loaded tweaks)
_CATEGORY_INFO: dict[str, CategoryInfo] = {}


def _build_category_info() -> None:
    """Derive CategoryInfo for every discovered category.

    Must be called after both ``_load_plugins()`` and ``_PROFILES`` are defined.
    """
    _CATEGORY_INFO.clear()

    for name, cat_tweaks in _TWEAKS_BY_CAT.items():
        info = CategoryInfo(name=name)

        # Infer scope from tweaks in this category
        scopes: set[str] = set()
        for td in cat_tweaks:
            keys_upper = [k.upper() for k in td.registry_keys]
            has_hkcu = any(k.startswith(("HKCU\\", "HKEY_CURRENT_USER\\")) for k in keys_upper)
            has_hklm = any(k.startswith(("HKLM\\", "HKEY_LOCAL_MACHINE\\", "HKCR\\", "HKEY_CLASSES_ROOT\\")) for k in keys_upper)
            if has_hkcu:
                scopes.add("user")
            if has_hklm:
                scopes.add("machine")
        if scopes == {"user"}:
            info.scope = "user"
        elif scopes == {"machine"}:
            info.scope = "machine"
        else:
            info.scope = "mixed"

        # Infer risk from policy-key and admin ratios
        policy_ratio = sum(1 for t in cat_tweaks if any("\\policies\\" in k.lower() for k in t.registry_keys)) / max(len(cat_tweaks), 1)
        admin_ratio = sum(1 for t in cat_tweaks if t.needs_admin) / max(len(cat_tweaks), 1)
        if policy_ratio > 0.5 or admin_ratio > 0.8:
            info.risk_level = "high"
        elif admin_ratio > 0.4:
            info.risk_level = "medium"
        else:
            info.risk_level = "low"

        # Which profiles include this category?
        for pname, pdata in _PROFILES.items():
            if name in pdata.apply_categories:
                info.profiles.append(pname)

        _CATEGORY_INFO[name] = info


# ── Plugin loader ────────────────────────────────────────────────────────────

_ALL_TWEAKS: list[TweakDef] = []
_TWEAK_INDEX: dict[str, TweakDef] = {}  # O(1) lookup by id
_TWEAKS_BY_CAT: dict[str, list[TweakDef]] = {}  # O(1) category lookup

# Scope cache — avoids repeated upper() + startswith() per tweak row
_SCOPE_CACHE: dict[str, str] = {}
_SCOPE_LOCK = threading.Lock()

# Module-level frozenset — avoids re-creating a tuple inside a tight loop
_VALID_HIVE_PREFIXES: frozenset[str] = frozenset(
    {
        "HKEY_LOCAL_MACHINE",
        "HKEY_CURRENT_USER",
        "HKEY_CLASSES_ROOT",
        "HKEY_USERS",
        "HKLM",
        "HKCU",
        "HKCR",
        "HKU",
    }
)


def _load_plugins() -> None:
    """Import every sibling module and collect TWEAKS lists."""
    _ALL_TWEAKS.clear()
    _TWEAK_INDEX.clear()
    _TWEAKS_BY_CAT.clear()
    seen_ids: set[str] = set()
    assert __package__ is not None
    package = importlib.import_module(__package__)
    for _finder, name, _ispkg in pkgutil.iter_modules(package.__path__):
        if name.startswith("_"):
            continue  # skip _template and other private helpers
        try:
            mod = importlib.import_module(f"{__package__}.{name}")
        except Exception as exc:
            warnings.warn(f"Failed to load tweak module {name!r}: {exc}", stacklevel=2)
            continue
        tweaks_list: list[TweakDef] = getattr(mod, "TWEAKS", [])
        for td in tweaks_list:
            if td.id in seen_ids:
                raise ValueError(f"Duplicate TweakDef id: {td.id!r}")
            seen_ids.add(td.id)
            for key in td.registry_keys:
                if not any(key.startswith(p) for p in _VALID_HIVE_PREFIXES):
                    warnings.warn(
                        f"TweakDef {td.id!r}: invalid registry path {key!r}",
                        stacklevel=2,
                    )
            _ALL_TWEAKS.append(td)
            _TWEAK_INDEX[td.id] = td
    # Validate depends_on references
    for td in _ALL_TWEAKS:
        for dep in td.depends_on:
            if dep not in _TWEAK_INDEX:
                raise ValueError(f"TweakDef {td.id!r} depends_on unknown id: {dep!r}")
    # Sort by category (alphabetical), then by label within each category
    _ALL_TWEAKS.sort(key=lambda t: (t.category.lower(), t.label.lower()))
    # Build reverse category index for O(1) category lookups
    for td in _ALL_TWEAKS:
        _TWEAKS_BY_CAT.setdefault(td.category, []).append(td)
    # Pre-warm scope cache: one pass during import so GUI startup pays zero compute cost
    with _SCOPE_LOCK:
        for td in _ALL_TWEAKS:
            if td.id not in _SCOPE_CACHE:
                keys_upper = [k.upper() for k in td.registry_keys]
                has_user = any(k.startswith(("HKCU\\", "HKEY_CURRENT_USER\\")) for k in keys_upper)
                has_machine = any(
                    k.startswith(
                        (
                            "HKLM\\",
                            "HKEY_LOCAL_MACHINE\\",
                            "HKCR\\",
                            "HKEY_CLASSES_ROOT\\",
                        )
                    )
                    for k in keys_upper
                )
                if has_user and has_machine:
                    _SCOPE_CACHE[td.id] = "both"
                elif has_user:
                    _SCOPE_CACHE[td.id] = "user"
                elif not td.registry_keys:
                    _SCOPE_CACHE[td.id] = "machine" if td.needs_admin else "user"
                else:
                    _SCOPE_CACHE[td.id] = "machine"


_load_plugins()


def all_tweaks() -> list[TweakDef]:
    """Return every registered tweak (read-only view)."""
    return list(_ALL_TWEAKS)


def tweak_scope(td: TweakDef) -> str:
    """Return ``'user'``, ``'machine'``, or ``'both'`` based on registry keys.

    Results are cached by tweak id for fast GUI lookups.
    Thread-safe: multiple threads can compute scope concurrently.
    """
    cached = _SCOPE_CACHE.get(td.id)
    if cached is not None:
        return cached
    if not td.registry_keys:
        result = "machine" if td.needs_admin else "user"
    else:
        has_user = any(k.upper().startswith(("HKCU\\", "HKEY_CURRENT_USER\\")) for k in td.registry_keys)
        has_machine = any(k.upper().startswith(("HKLM\\", "HKEY_LOCAL_MACHINE\\", "HKCR\\", "HKEY_CLASSES_ROOT\\")) for k in td.registry_keys)
        if has_user and has_machine:
            result = "both"
        elif has_user:
            result = "user"
        else:
            result = "machine"
    # Harmless race: two threads computing the same idempotent value; use lock only for write
    with _SCOPE_LOCK:
        _SCOPE_CACHE[td.id] = result
    return result


def get_tweak(tweak_id: str) -> TweakDef | None:
    """Look up a tweak by id — O(1) dict lookup."""
    return _TWEAK_INDEX.get(tweak_id)


def reload_plugins() -> None:
    """Re-scan sub-modules (useful after hot-adding a plugin)."""
    with _SCOPE_LOCK:
        _SCOPE_CACHE.clear()
    with _SEARCH_LOCK:
        _SEARCH_INDEX.clear()
    _TAG_INDEX.clear()
    _load_plugins()
    _prewarm_indexes()
    _build_category_info()


def categories() -> list[str]:
    """Return sorted unique category names."""
    return list(OrderedDict.fromkeys(td.category for td in _ALL_TWEAKS))


def tweaks_by_category() -> dict[str, list[TweakDef]]:
    """Return tweaks grouped into ``{category: [TweakDef, ...]}``, sorted."""
    return dict(_TWEAKS_BY_CAT)


def tweaks_by_ids(ids: Iterable[str]) -> list[TweakDef]:
    """Return TweakDef objects for multiple IDs in one O(k) pass via _TWEAK_INDEX."""
    return [_TWEAK_INDEX[tid] for tid in ids if tid in _TWEAK_INDEX]


def tweaks_by_tag(tag: str) -> list[TweakDef]:
    """Return all tweaks that carry *tag* (case-insensitive, O(1) lookup)."""
    return list(_TAG_INDEX.get(tag.lower(), []))


def search_tweaks(query: str) -> list[TweakDef]:
    """Filter tweaks by a case-insensitive query matching id/label/category/tags.

    Uses pre-built search index for O(n) scanning with minimal allocations.
    Single-tag queries (no spaces) additionally use the O(1) tag index for
    an early-exit fast path when the query matches a known tag exactly.
    """
    q = query.lower().strip()
    if not q:
        return list(_ALL_TWEAKS)
    # Fast path: exact tag match
    if " " not in q and q in _TAG_INDEX:
        # Still verify the full search string (tag index is supplemental)
        tag_set = {td.id for td in _TAG_INDEX[q]}
        return [td for td in _ALL_TWEAKS if td.id in tag_set or q in _get_search_index(td)]
    return [td for td in _ALL_TWEAKS if q in _get_search_index(td)]


# Pre-built search index for fast full-text scanning
_SEARCH_INDEX: dict[str, str] = {}
_SEARCH_LOCK = threading.Lock()

# Tag index for O(1) tag-based filtering
_TAG_INDEX: dict[str, list[TweakDef]] = {}


def _get_search_index(td: TweakDef) -> str:
    """Return cached lowercased search string for a tweak."""
    cached = _SEARCH_INDEX.get(td.id)
    if cached is not None:
        return cached
    sep = "\0"
    result = sep.join([td.id, td.label, td.category, td.description, *td.tags]).lower()
    with _SEARCH_LOCK:
        _SEARCH_INDEX[td.id] = result
    return result


def _prewarm_indexes() -> None:
    """Eagerly populate _SEARCH_INDEX and _TAG_INDEX for all loaded tweaks.

    Called once after _load_plugins() so that the first search is instant
    rather than paying the O(n) deferred build cost at query time.
    """
    _TAG_INDEX.clear()
    with _SEARCH_LOCK:
        _SEARCH_INDEX.clear()
        for td in _ALL_TWEAKS:
            sep = "\0"
            _SEARCH_INDEX[td.id] = sep.join([td.id, td.label, td.category, td.description, *td.tags]).lower()
    for td in _ALL_TWEAKS:
        for tag in td.tags:
            _TAG_INDEX.setdefault(tag.lower(), []).append(td)


# ── Machine profiles ─────────────────────────────────────────────────────────


@dataclass(slots=True)
class ProfileDef:
    """Declarative description of a machine profile."""

    id: str
    description: str
    apply_categories: frozenset[str]
    skip_categories: frozenset[str] = field(default_factory=frozenset)


_PROFILES: dict[str, ProfileDef] = {}


def _register_profiles() -> None:
    """Populate the profile registry."""
    defs = [
        ProfileDef(
            id="business",
            description="Business workstation — productivity, security, cloud & workflow tweaks",
            apply_categories=frozenset(
                {
                    "Adobe",
                    "Backup & Recovery",
                    "Boot",
                    "Chrome",
                    "Clipboard & Drag-Drop",
                    "Cloud Storage",
                    "Communication",
                    "Context Menu",
                    "Cortana & Search",
                    "Crash & Diagnostics",
                    "Developer Tools",
                    "Display",
                    "Edge",
                    "Explorer",
                    "File System",
                    "Fonts",
                    "Indexing & Search",
                    "Lock Screen & Login",
                    "M365 Copilot",
                    "Maintenance",
                    "Network",
                    "Notifications",
                    "Office",
                    "OneDrive",
                    "Performance",
                    "Power",
                    "Printing",
                    "Privacy",
                    "Scheduled Tasks",
                    "Security",
                    "Services",
                    "Shell",
                    "Snap & Multitasking",
                    "Startup",
                    "System",
                    "Taskbar",
                    "Telemetry Advanced",
                    "Windows 11",
                    "Windows Update",
                }
            ),
            skip_categories=frozenset({"Gaming", "GPU / Graphics", "Virtualization", "WSL", "Scoop Tools"}),
        ),
        ProfileDef(
            id="gaming",
            description="Gaming rig — GPU, performance, low-latency & distraction-free tweaks",
            apply_categories=frozenset(
                {
                    "Audio",
                    "Bluetooth",
                    "Boot",
                    "Context Menu",
                    "Crash & Diagnostics",
                    "Display",
                    "DNS & Networking Advanced",
                    "Explorer",
                    "File System",
                    "Gaming",
                    "GPU / Graphics",
                    "Indexing & Search",
                    "Input",
                    "Multimedia",
                    "Network",
                    "Notifications",
                    "Performance",
                    "Power",
                    "Privacy",
                    "Scheduled Tasks",
                    "Screensaver & Lock",
                    "Services",
                    "Shell",
                    "Startup",
                    "Storage",
                    "System",
                    "Taskbar",
                    "Telemetry Advanced",
                    "USB & Peripherals",
                    "Widgets & News",
                    "Windows 11",
                }
            ),
            skip_categories=frozenset(
                {
                    "Office",
                    "Communication",
                    "OneDrive",
                    "Cloud Storage",
                    "LibreOffice",
                    "Printing",
                    "Backup & Recovery",
                    "M365 Copilot",
                    "RealVNC",
                }
            ),
        ),
        ProfileDef(
            id="privacy",
            description="Maximum privacy — disables telemetry, tracking, cloud & browser data collection",
            apply_categories=frozenset(
                {
                    "Adobe",
                    "AI / Copilot",
                    "Chrome",
                    "Cloud Storage",
                    "Communication",
                    "Cortana & Search",
                    "Crash & Diagnostics",
                    "Edge",
                    "Explorer",
                    "Firefox",
                    "Indexing & Search",
                    "Lock Screen & Login",
                    "M365 Copilot",
                    "Maintenance",
                    "Microsoft Store",
                    "Network",
                    "Notifications",
                    "Office",
                    "OneDrive",
                    "Performance",
                    "Privacy",
                    "Scheduled Tasks",
                    "Screensaver & Lock",
                    "Security",
                    "Services",
                    "Startup",
                    "System",
                    "Telemetry Advanced",
                    "Widgets & News",
                    "Windows 11",
                    "Windows Update",
                }
            ),
        ),
        ProfileDef(
            id="minimal",
            description="Minimal — lightweight essentials for fast, clean system operation",
            apply_categories=frozenset(
                {
                    "Boot",
                    "Context Menu",
                    "Crash & Diagnostics",
                    "Explorer",
                    "File System",
                    "Indexing & Search",
                    "Lock Screen & Login",
                    "Maintenance",
                    "Notifications",
                    "Performance",
                    "Power",
                    "Privacy",
                    "Scheduled Tasks",
                    "Security",
                    "Services",
                    "Shell",
                    "Startup",
                    "Storage",
                    "System",
                    "Telemetry Advanced",
                    "Widgets & News",
                    "Windows Update",
                }
            ),
        ),
        ProfileDef(
            id="server",
            description="Server — hardened, headless, optimized for uptime & remote management",
            apply_categories=frozenset(
                {
                    "Backup & Recovery",
                    "Boot",
                    "Context Menu",
                    "Crash & Diagnostics",
                    "DNS & Networking Advanced",
                    "Explorer",
                    "File System",
                    "Indexing & Search",
                    "Lock Screen & Login",
                    "Maintenance",
                    "Network",
                    "Notifications",
                    "Package Management",
                    "Performance",
                    "Power",
                    "Privacy",
                    "Remote Desktop",
                    "Scheduled Tasks",
                    "Security",
                    "Services",
                    "Shell",
                    "Startup",
                    "Storage",
                    "System",
                    "Telemetry Advanced",
                    "Virtualization",
                    "Windows Update",
                    "WSL",
                }
            ),
            skip_categories=frozenset(
                {
                    "Adobe",
                    "Audio",
                    "Chrome",
                    "Cloud Storage",
                    "Communication",
                    "Cortana & Search",
                    "Firefox",
                    "Fonts",
                    "Gaming",
                    "GPU / Graphics",
                    "LibreOffice",
                    "M365 Copilot",
                    "Multimedia",
                    "OneDrive",
                    "Scoop Tools",
                    "Widgets & News",
                }
            ),
        ),
    ]
    for p in defs:
        _PROFILES[p.id] = p


_register_profiles()

# Build category metadata now that both _ALL_TWEAKS and _PROFILES are ready
_build_category_info()

# Eagerly warm search + tag indexes so first search has zero lazy-build overhead
_prewarm_indexes()


def category_info(name: str) -> CategoryInfo | None:
    """Return metadata for a category or ``None``."""
    return _CATEGORY_INFO.get(name)


def all_category_info() -> dict[str, CategoryInfo]:
    """Return ``{name: CategoryInfo}`` for every known category."""
    return dict(_CATEGORY_INFO)


def categories_by_risk(level: str = "high") -> list[str]:
    """Return category names matching the given risk level."""
    return [ci.name for ci in _CATEGORY_INFO.values() if ci.risk_level == level]


def categories_by_scope(scope: str = "machine") -> list[str]:
    """Return category names matching the given scope (user/machine/mixed)."""
    return [ci.name for ci in _CATEGORY_INFO.values() if ci.scope == scope]


def available_profiles() -> list[str]:
    """Return known machine-profile names."""
    return list(_PROFILES)


def profile_info(name: str) -> ProfileDef | None:
    """Return metadata for a profile or ``None``."""
    return _PROFILES.get(name)


def tweaks_for_profile(name: str) -> list[TweakDef]:
    """Return tweaks that a profile would *apply* (= disable those features)."""
    prof = _PROFILES.get(name)
    if prof is None:
        raise ValueError(f"Unknown profile: {name!r}. Choose from {available_profiles()}")
    result: list[TweakDef] = []
    for cat in prof.apply_categories:
        result.extend(_TWEAKS_BY_CAT.get(cat, []))
    return result


def tweaks_excluded_by_profile(name: str) -> list[TweakDef]:
    """Return tweaks that a profile would *hide / skip*."""
    prof = _PROFILES.get(name)
    if prof is None:
        raise ValueError(f"Unknown profile: {name!r}. Choose from {available_profiles()}")
    result: list[TweakDef] = []
    for cat in prof.skip_categories:
        result.extend(_TWEAKS_BY_CAT.get(cat, []))
    return result


def apply_profile(
    name: str,
    *,
    force_corp: bool = False,
    require_admin: bool = True,
    progress_cb: Callable[[str, TweakResult], None] | None = None,
) -> dict[str, TweakResult]:
    """Apply every tweak associated with *name* profile."""
    targets = tweaks_for_profile(name)
    exe = TweakExecutor(force_corp=force_corp, require_admin=require_admin)
    return exe.run_batch(targets, "apply", progress_cb=progress_cb)


# ── Status helpers ───────────────────────────────────────────────────────────


def tweak_status(td: TweakDef) -> TweakResult:
    """Return the current status of a tweak as a :class:`TweakResult`."""
    if td.detect_fn is None:
        return TweakResult.UNKNOWN
    try:
        return TweakResult.APPLIED if td.detect_fn() else TweakResult.NOT_APPLIED
    except Exception:
        return TweakResult.UNKNOWN


def status_map(
    *,
    parallel: bool = False,
    max_workers: int = 0,
    progress_fn: Callable[[int, int], None] | None = None,
    ids: Iterable[str] | None = None,
) -> dict[str, TweakResult]:
    """Return ``{tweak_id: TweakResult}`` for registered tweaks.

    With ``parallel=True`` the detection runs in a thread-pool for faster GUI refresh.
    *progress_fn(done, total)* is called after each tweak completes (thread-safe).
    *max_workers* defaults to ``min(8, cpu_count)`` when 0.
    *ids* restricts evaluation to the requested subset — useful for incremental
    GUI refresh when only a few rows need updating.
    """
    from regilattice.registry import SESSION

    if max_workers <= 0:
        try:
            from regilattice.hwinfo import detect_hardware

            max_workers = detect_hardware().optimal_workers
        except Exception:
            max_workers = min(8, os.cpu_count() or 4)
    target_tweaks: list[TweakDef] = [_TWEAK_INDEX[i] for i in ids if i in _TWEAK_INDEX] if ids is not None else _ALL_TWEAKS
    total = len(target_tweaks)
    with SESSION.read_cache():
        if not parallel:
            results: dict[str, TweakResult] = {}
            for i, td in enumerate(target_tweaks):
                if td.detect_fn is None:
                    results[td.id] = TweakResult.UNKNOWN
                else:
                    results[td.id] = tweak_status(td)
                if progress_fn is not None:
                    progress_fn(i + 1, total)
            return results
        results = {}
        done = 0
        detect_tweaks: list[TweakDef] = []
        for td in target_tweaks:
            if td.detect_fn is None:
                results[td.id] = TweakResult.UNKNOWN
                done += 1
                if progress_fn is not None:
                    progress_fn(done, total)
            else:
                detect_tweaks.append(td)
        with concurrent.futures.ThreadPoolExecutor(max_workers=max_workers) as pool:
            futures = {pool.submit(tweak_status, td): td.id for td in detect_tweaks}
            for fut in concurrent.futures.as_completed(futures, timeout=120):
                tid = futures[fut]
                try:
                    results[tid] = fut.result(timeout=10)
                except Exception:
                    results[tid] = TweakResult.UNKNOWN
                done += 1
                if progress_fn is not None:
                    progress_fn(done, total)
        return results


# ── Snapshot / Undo ──────────────────────────────────────────────────────────


def save_snapshot(path: Path) -> None:
    """Persist the current status of all tweaks to *path* (JSON)."""
    data = {tid: res.value for tid, res in status_map().items()}
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(json.dumps(data, indent=2), encoding="utf-8")


def load_snapshot(path: Path) -> dict[str, str]:
    """Read a previously-saved snapshot."""
    return json.loads(path.read_text(encoding="utf-8"))  # type: ignore[no-any-return]


def restore_snapshot(
    path: Path,
    *,
    force_corp: bool = False,
    require_admin: bool = True,
) -> dict[str, TweakResult]:
    """Compare *path* against the live status and revert changed tweaks.

    Returns a dict of ``{tweak_id: TweakResult}``.
    """
    saved = load_snapshot(path)
    exe = TweakExecutor(force_corp=force_corp, require_admin=require_admin)
    results: dict[str, TweakResult] = {}
    for td in _ALL_TWEAKS:
        saved_state = saved.get(td.id)
        if saved_state is None:
            continue
        current = tweak_status(td)
        if current == saved_state:
            results[td.id] = TweakResult.UNCHANGED
            continue
        if saved_state == TweakResult.APPLIED:
            results[td.id] = exe.apply_one(td)
        else:
            results[td.id] = exe.remove_one(td)
    return results


def diff_snapshots(path_a: Path, path_b: Path) -> dict[str, tuple[str, str]]:
    """Compare two snapshot files and return a dict of changed tweaks.

    Returns ``{tweak_id: (state_a, state_b)}`` for tweaks that differ.
    """
    snap_a = load_snapshot(path_a)
    snap_b = load_snapshot(path_b)
    all_ids = set(snap_a) | set(snap_b)
    diffs: dict[str, tuple[str, str]] = {}
    for tid in sorted(all_ids):
        sa = snap_a.get(tid, "(absent)")
        sb = snap_b.get(tid, "(absent)")
        if sa != sb:
            diffs[tid] = (sa, sb)
    return diffs


# ── Dependency ordering ───────────────────────────────────────────────────────


def _topo_sort(tweaks: list[TweakDef]) -> list[TweakDef]:
    """Return *tweaks* in dependency-first order (Kahn's algorithm).

    Only tweaks **in the input list** participate; missing deps are silently
    skipped (they may have already been applied in a prior batch).
    Cycles raise :class:`ValueError`.
    """
    ids = {td.id for td in tweaks}
    by_id = {td.id: td for td in tweaks}
    # in-degree only for deps inside this batch
    in_deg: dict[str, int] = {td.id: 0 for td in tweaks}
    children: dict[str, list[str]] = {td.id: [] for td in tweaks}
    for td in tweaks:
        for dep in td.depends_on:
            if dep in ids:
                in_deg[td.id] += 1
                children[dep].append(td.id)
    queue = deque(tid for tid, deg in in_deg.items() if deg == 0)
    ordered: list[TweakDef] = []
    while queue:
        tid = queue.popleft()
        ordered.append(by_id[tid])
        for child in children[tid]:
            in_deg[child] -= 1
            if in_deg[child] == 0:
                queue.append(child)
    if len(ordered) != len(tweaks):
        raise ValueError("Cyclic dependency detected among tweaks")
    return ordered


# ── Windows build number constants ─────────────────────────────────────────

# Named build numbers for use in TweakDef.min_build.
# All w11-* tweaks should be at least WIN11_21H2 (22000).
WIN10_21H2: int = 19044  # Windows 10 21H2
WIN11_21H2: int = 22000  # Windows 11 21H2
WIN11_22H2: int = 22621  # Windows 11 22H2
WIN11_23H2: int = 22631  # Windows 11 23H2
WIN11_24H2: int = 26100  # Windows 11 24H2


# ── Windows build detection ──────────────────────────────────────────────────


@functools.lru_cache(maxsize=1)
def _windows_build() -> int:
    """Return the Windows build number (e.g. 22631 for Win 11 23H2).

    Returns 0 on non-Windows platforms or if detection fails.
    """
    try:
        return int(platform.version().split(".")[-1])
    except (ValueError, AttributeError):
        return 0


def windows_build() -> int:
    """Public accessor for the current Windows build number."""
    return _windows_build()


# ── Tweak executor ───────────────────────────────────────────────────────────


class TweakExecutor:
    """Centralised pipeline for applying / removing a single tweak.

    Corp-guard check, build-version gate, function call, and
    exception→TweakResult mapping happen here so that CLI, GUI, menu,
    and batch helpers all share one path.
    """

    __slots__ = ("force_corp", "require_admin")

    def __init__(self, *, force_corp: bool = False, require_admin: bool = True) -> None:
        self.force_corp = force_corp
        self.require_admin = require_admin

    # ── low-level helpers ────────────────────────────────────────────────

    def _is_blocked(self, td: TweakDef) -> bool:
        """Return True if *td* should be skipped due to corporate policy."""
        if self.force_corp or td.corp_safe:
            return False
        from regilattice.corpguard import is_corporate_network

        return is_corporate_network()

    def is_blocked(self, td: TweakDef) -> bool:
        """Public API: check if *td* would be blocked by corporate policy."""
        return self._is_blocked(td)

    def _is_build_blocked(self, td: TweakDef) -> bool:
        """Return True if the current Windows build is below *td.min_build*."""
        return td.min_build > 0 and _windows_build() < td.min_build

    def apply_one(self, td: TweakDef) -> TweakResult:
        """Apply a single tweak, returning a typed result.

        If the apply function raises, logs the error for diagnostics.
        """
        if self._is_blocked(td):
            return TweakResult.SKIPPED_CORP
        if self._is_build_blocked(td):
            return TweakResult.SKIPPED_BUILD
        try:
            td.apply_fn(require_admin=self.require_admin)
            return TweakResult.APPLIED
        except Exception:
            from regilattice.registry import SESSION

            SESSION.log(f"ERROR applying {td.id!r}")
            return TweakResult.ERROR

    def remove_one(self, td: TweakDef) -> TweakResult:
        """Remove (revert) a single tweak, returning a typed result.

        If the remove function raises, logs the error for diagnostics.
        """
        if self._is_blocked(td):
            return TweakResult.SKIPPED_CORP
        if self._is_build_blocked(td):
            return TweakResult.SKIPPED_BUILD
        try:
            td.remove_fn(require_admin=self.require_admin)
            return TweakResult.REMOVED
        except Exception:
            from regilattice.registry import SESSION

            SESSION.log(f"ERROR removing {td.id!r}")
            return TweakResult.ERROR

    # ── batch helpers ────────────────────────────────────────────────────

    def run_batch(
        self,
        tweaks: list[TweakDef],
        mode: str,
        *,
        parallel: bool = False,
        max_workers: int = 4,
        progress_cb: Callable[[str, TweakResult], None] | None = None,
    ) -> dict[str, TweakResult]:
        """Apply or remove a list of tweaks, optionally in parallel.

        *mode* must be ``"apply"`` or ``"remove"``.
        """
        fn = self.apply_one if mode == "apply" else self.remove_one
        results: dict[str, TweakResult] = {}
        ordered = _topo_sort(tweaks)

        def _do(td: TweakDef) -> tuple[str, TweakResult]:
            return td.id, fn(td)

        if parallel:
            with concurrent.futures.ThreadPoolExecutor(max_workers=max_workers) as pool:
                futures = {pool.submit(_do, td): td for td in ordered}
                for fut in concurrent.futures.as_completed(futures, timeout=300):
                    tid_key = futures[fut]
                    try:
                        tid, res = fut.result(timeout=30)
                    except Exception:
                        tid, res = tid_key.id, TweakResult.ERROR
                    results[tid] = res
                    if progress_cb:
                        progress_cb(tid, res)
        else:
            for td in ordered:
                tid, res = _do(td)
                results[tid] = res
                if progress_cb:
                    progress_cb(tid, res)
        return results


# ── Batch operations (convenience wrappers) ──────────────────────────────────


def _is_corp_blocked(td: TweakDef, *, force_corp: bool) -> bool:
    """Return True if *td* should be skipped due to corporate-network policy."""
    if force_corp or td.corp_safe:
        return False
    from regilattice.corpguard import is_corporate_network

    return is_corporate_network()


def apply_all(
    *,
    force_corp: bool = False,
    require_admin: bool = True,
    parallel: bool = False,
    max_workers: int = 4,
    progress_cb: Callable[[str, TweakResult], None] | None = None,
) -> dict[str, TweakResult]:
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
    exe = TweakExecutor(force_corp=force_corp, require_admin=require_admin)
    return exe.run_batch(
        list(_ALL_TWEAKS),
        "apply",
        parallel=parallel,
        max_workers=max_workers,
        progress_cb=progress_cb,
    )


def remove_all(
    *,
    force_corp: bool = False,
    require_admin: bool = True,
    parallel: bool = False,
    max_workers: int = 4,
    progress_cb: Callable[[str, TweakResult], None] | None = None,
) -> dict[str, TweakResult]:
    """Remove every registered tweak, respecting corp-safe flags."""
    exe = TweakExecutor(force_corp=force_corp, require_admin=require_admin)
    return exe.run_batch(
        list(_ALL_TWEAKS),
        "remove",
        parallel=parallel,
        max_workers=max_workers,
        progress_cb=progress_cb,
    )


def filter_tweaks(
    *,
    corp_safe: bool | None = None,
    needs_admin: bool | None = None,
    scope: str | None = None,
    category: str | None = None,
    min_build: int | None = None,
    tags: Iterable[str] | None = None,
    query: str | None = None,
) -> list[TweakDef]:
    """Return tweaks matching ALL supplied criteria (AND semantics).

    Parameters
    ----------
    corp_safe:
        If True, return only HKCU-safe tweaks; False returns only HKLM tweaks.
    needs_admin:
        Filter by the ``needs_admin`` flag.
    scope:
        ``'user'``, ``'machine'``, or ``'both'`` — matched via :func:`tweak_scope`.
    category:
        Exact category name match.
    min_build:
        Include only tweaks whose ``min_build`` is <= this value (i.e. compatible).
    tags:
        Tweaks that carry **all** of the supplied tags.
    query:
        Free-text search (delegated to :func:`search_tweaks`).
    """
    pool: list[TweakDef] = search_tweaks(query) if query else list(_ALL_TWEAKS)
    if corp_safe is not None:
        pool = [td for td in pool if td.corp_safe is corp_safe]
        if not pool:
            return pool
    if needs_admin is not None:
        pool = [td for td in pool if td.needs_admin is needs_admin]
        if not pool:
            return pool
    if scope is not None:
        pool = [td for td in pool if tweak_scope(td) == scope]
        if not pool:
            return pool
    if category is not None:
        pool = [td for td in pool if td.category == category]
        if not pool:
            return pool
    if min_build is not None:
        pool = [td for td in pool if td.min_build <= min_build]
        if not pool:
            return pool
    if tags is not None:
        tag_set = frozenset(t.lower() for t in tags)
        pool = [td for td in pool if tag_set.issubset(t.lower() for t in td.tags)]
    return pool


def tweak_dependencies(td: TweakDef, *, transitive: bool = True) -> list[TweakDef]:
    """Return the dependency chain for *td* in dependency-first (topological) order.

    Parameters
    ----------
    transitive:
        If True (default), recursively resolve the full dependency graph.
        If False, return only direct (one-hop) dependencies.
    """
    if not td.depends_on:
        return []
    if not transitive:
        return [_TWEAK_INDEX[dep] for dep in td.depends_on if dep in _TWEAK_INDEX]
    visited: set[str] = set()
    ordered: list[TweakDef] = []

    def _visit(node_id: str) -> None:
        if node_id in visited:
            return
        visited.add(node_id)
        node = _TWEAK_INDEX.get(node_id)
        if node is None:
            return
        for dep in node.depends_on:
            _visit(dep)
        ordered.append(node)

    for dep in td.depends_on:
        _visit(dep)
    return ordered


def apply_tweaks(
    ids: Iterable[str],
    *,
    force_corp: bool = False,
    require_admin: bool = True,
    parallel: bool = False,
    max_workers: int = 4,
    include_deps: bool = True,
    progress_cb: Callable[[str, TweakResult], None] | None = None,
) -> dict[str, TweakResult]:
    """Apply the tweaks identified by *ids*, resolving dependencies first.

    Parameters
    ----------
    include_deps:
        When True (default), prepend any unmet dependencies in topological order.
    """
    targets = tweaks_by_ids(ids)
    if include_deps:
        dep_ids: set[str] = set()
        for td in targets:
            for dep in tweak_dependencies(td):
                dep_ids.add(dep.id)
        extra = [_TWEAK_INDEX[d] for d in dep_ids if d not in {t.id for t in targets} and d in _TWEAK_INDEX]
        targets = extra + targets
    exe = TweakExecutor(force_corp=force_corp, require_admin=require_admin)
    return exe.run_batch(
        targets,
        "apply",
        parallel=parallel,
        max_workers=max_workers,
        progress_cb=progress_cb,
    )


def remove_tweaks(
    ids: Iterable[str],
    *,
    force_corp: bool = False,
    require_admin: bool = True,
    parallel: bool = False,
    max_workers: int = 4,
    progress_cb: Callable[[str, TweakResult], None] | None = None,
) -> dict[str, TweakResult]:
    """Remove the tweaks identified by *ids* (reverse of :func:`apply_tweaks`)."""
    targets = tweaks_by_ids(ids)
    exe = TweakExecutor(force_corp=force_corp, require_admin=require_admin)
    return exe.run_batch(
        targets,
        "remove",
        parallel=parallel,
        max_workers=max_workers,
        progress_cb=progress_cb,
    )


# ── Scope/build/risk helpers ─────────────────────────────────────────────────


def tweaks_by_scope(scope: str) -> list[TweakDef]:
    """Return all tweaks whose registry scope matches *scope* (user/machine/both).

    Equivalent to ``filter_tweaks(scope=scope)`` but uses the pre-warmed
    scope cache so the common GUI case (enumerate by scope) is O(n) with
    minimal per-entry overhead.
    """
    return [td for td in _ALL_TWEAKS if tweak_scope(td) == scope]


def tweaks_above_build(build: int) -> list[TweakDef]:
    """Return tweaks whose ``min_build`` is at most *build* (i.e. compatible).

    Tweaks with ``min_build == 0`` are always included (no version restriction).
    """
    return [td for td in _ALL_TWEAKS if td.min_build <= build]


def tweak_risk_level(td: TweakDef) -> str:
    """Return the risk level (``'low'``, ``'medium'``, or ``'high'``) for *td*.

    Derived from the :class:`CategoryInfo` for the tweak's category.
    Falls back to ``'low'`` for unknown categories.
    """
    info = _CATEGORY_INFO.get(td.category)
    return info.risk_level if info is not None else "low"


def tweak_count_by_scope() -> dict[str, int]:
    """Return ``{scope: count}`` for all registered tweaks.

    Uses the pre-warmed scope cache for O(n) performance.
    Useful for the GUI stats bar without re-computing scopes.
    """
    counts: dict[str, int] = {"user": 0, "machine": 0, "both": 0}
    for td in _ALL_TWEAKS:
        s = tweak_scope(td)
        counts[s] = counts.get(s, 0) + 1
    return counts


def category_counts() -> dict[str, int]:
    """Return ``{category: tweak_count}`` — O(n) dict comprehension over the
    pre-built category index for fast GUI stats bar rendering.
    """
    return {cat: len(ts) for cat, ts in _TWEAKS_BY_CAT.items()}
