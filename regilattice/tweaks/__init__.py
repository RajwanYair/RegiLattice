"""Plugin-based tweak registry — auto-discovers TweakDef instances.

Every sub-module in this package exports a ``TWEAKS`` list of
:class:`TweakDef` instances.  The loader collects them all on import
so that the GUI, CLI, and menu can iterate over a single flat list.
"""

from __future__ import annotations

import concurrent.futures
import importlib
import json
import pkgutil
from collections import OrderedDict
from collections.abc import Callable
from dataclasses import dataclass, field
from enum import Enum
from pathlib import Path

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

    for name in OrderedDict.fromkeys(td.category for td in _ALL_TWEAKS):
        info = CategoryInfo(name=name)

        # Infer scope from tweaks in this category
        scopes: set[str] = set()
        cat_tweaks = [t for t in _ALL_TWEAKS if t.category == name]
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


def _load_plugins() -> None:
    """Import every sibling module and collect TWEAKS lists."""
    _ALL_TWEAKS.clear()
    _TWEAK_INDEX.clear()
    seen_ids: set[str] = set()
    assert __package__ is not None
    package = importlib.import_module(__package__)
    for _finder, name, _ispkg in pkgutil.iter_modules(package.__path__):
        if name.startswith("_"):
            continue  # skip _template and other private helpers
        mod = importlib.import_module(f"{__package__}.{name}")
        tweaks_list: list[TweakDef] = getattr(mod, "TWEAKS", [])
        for td in tweaks_list:
            if td.id in seen_ids:
                raise ValueError(f"Duplicate TweakDef id: {td.id!r}")
            seen_ids.add(td.id)
            _ALL_TWEAKS.append(td)
            _TWEAK_INDEX[td.id] = td
    # Validate depends_on references
    for td in _ALL_TWEAKS:
        for dep in td.depends_on:
            if dep not in _TWEAK_INDEX:
                raise ValueError(f"TweakDef {td.id!r} depends_on unknown id: {dep!r}")
    # Sort by category (alphabetical), then by label within each category
    _ALL_TWEAKS.sort(key=lambda t: (t.category.lower(), t.label.lower()))


_load_plugins()


def all_tweaks() -> list[TweakDef]:
    """Return every registered tweak (read-only view)."""
    return list(_ALL_TWEAKS)


# Scope cache — avoids repeated upper() + startswith() per tweak row
_SCOPE_CACHE: dict[str, str] = {}


def tweak_scope(td: TweakDef) -> str:
    """Return ``'user'``, ``'machine'``, or ``'both'`` based on registry keys.

    Results are cached by tweak id for fast GUI lookups.
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
    _SCOPE_CACHE[td.id] = result
    return result


def get_tweak(tweak_id: str) -> TweakDef | None:
    """Look up a tweak by id — O(1) dict lookup."""
    return _TWEAK_INDEX.get(tweak_id)


def reload_plugins() -> None:
    """Re-scan sub-modules (useful after hot-adding a plugin)."""
    _SCOPE_CACHE.clear()
    _SEARCH_INDEX.clear()
    _load_plugins()
    _build_category_info()


def categories() -> list[str]:
    """Return sorted unique category names."""
    return list(OrderedDict.fromkeys(td.category for td in _ALL_TWEAKS))


def tweaks_by_category() -> dict[str, list[TweakDef]]:
    """Return tweaks grouped into ``{category: [TweakDef, ...]}``, sorted."""
    groups: dict[str, list[TweakDef]] = OrderedDict()
    for td in _ALL_TWEAKS:
        groups.setdefault(td.category, []).append(td)
    return groups


def search_tweaks(query: str) -> list[TweakDef]:
    """Filter tweaks by a case-insensitive query matching id/label/category/tags.

    Uses pre-built search index for O(n) scanning with minimal allocations.
    """
    q = query.lower()
    return [td for td in _ALL_TWEAKS if q in _get_search_index(td)]


# Pre-built search index for fast full-text scanning
_SEARCH_INDEX: dict[str, str] = {}


def _get_search_index(td: TweakDef) -> str:
    """Return cached lowercased search string for a tweak."""
    cached = _SEARCH_INDEX.get(td.id)
    if cached is not None:
        return cached
    sep = "\0"
    result = sep.join([td.id, td.label, td.category, td.description, *td.tags]).lower()
    _SEARCH_INDEX[td.id] = result
    return result


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
            description="Business workstation — productivity, security & cloud tweaks",
            apply_categories=frozenset({
                "Cloud Storage", "Office", "Communication", "OneDrive",
                "Security", "Network", "Privacy", "Printing",
                "Backup & Recovery", "M365 Copilot",
            }),
            skip_categories=frozenset({"Gaming", "GPU / Graphics", "Virtualization"}),
        ),
        ProfileDef(
            id="gaming",
            description="Gaming rig — GPU, performance & network tweaks",
            apply_categories=frozenset({
                "Gaming", "GPU / Graphics", "Performance", "Display",
                "Audio", "Network", "Power", "Services",
            }),
            skip_categories=frozenset({
                "Office", "Communication", "OneDrive", "Cloud Storage",
                "LibreOffice", "Printing", "Backup & Recovery",
            }),
        ),
        ProfileDef(
            id="privacy",
            description="Maximum privacy — disables telemetry, tracking & cloud",
            apply_categories=frozenset({
                "Privacy", "Cortana & Search", "AI / Copilot", "M365 Copilot",
                "Windows 11", "Cloud Storage", "Communication",
            }),
        ),
        ProfileDef(
            id="minimal",
            description="Minimal — lightweight essentials only (performance, startup, maintenance)",
            apply_categories=frozenset({
                "Performance", "Startup", "Maintenance", "Boot", "Power", "Services",
            }),
        ),
        ProfileDef(
            id="server",
            description="Server — virtualization, network hardening, services optimization",
            apply_categories=frozenset({
                "Network", "Power", "Services", "Security",
                "Virtualization", "Backup & Recovery",
            }),
            skip_categories=frozenset({
                "Gaming", "GPU / Graphics", "Communication",
                "Cloud Storage", "OneDrive",
            }),
        ),
    ]
    for p in defs:
        _PROFILES[p.id] = p


_register_profiles()

# Build category metadata now that both _ALL_TWEAKS and _PROFILES are ready
_build_category_info()


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
    return [td for td in _ALL_TWEAKS if td.category in prof.apply_categories]


def tweaks_excluded_by_profile(name: str) -> list[TweakDef]:
    """Return tweaks that a profile would *hide / skip*."""
    prof = _PROFILES.get(name)
    if prof is None:
        raise ValueError(f"Unknown profile: {name!r}. Choose from {available_profiles()}")
    return [td for td in _ALL_TWEAKS if td.category in prof.skip_categories]


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


def status_map(*, parallel: bool = False, max_workers: int = 8) -> dict[str, TweakResult]:
    """Return ``{tweak_id: TweakResult}`` for every registered tweak.

    With ``parallel=True`` the detection runs in a thread-pool for faster GUI refresh.
    """
    if not parallel:
        return {td.id: tweak_status(td) for td in _ALL_TWEAKS}
    results: dict[str, TweakResult] = {}
    with concurrent.futures.ThreadPoolExecutor(max_workers=max_workers) as pool:
        futures = {pool.submit(tweak_status, td): td.id for td in _ALL_TWEAKS}
        for fut in concurrent.futures.as_completed(futures):
            results[futures[fut]] = fut.result()
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
    queue = [tid for tid, deg in in_deg.items() if deg == 0]
    ordered: list[TweakDef] = []
    while queue:
        tid = queue.pop(0)
        ordered.append(by_id[tid])
        for child in children[tid]:
            in_deg[child] -= 1
            if in_deg[child] == 0:
                queue.append(child)
    if len(ordered) != len(tweaks):
        raise ValueError("Cyclic dependency detected among tweaks")
    return ordered


# ── Tweak executor ───────────────────────────────────────────────────────────


class TweakExecutor:
    """Centralised pipeline for applying / removing a single tweak.

    Corp-guard check, function call, and exception→TweakResult mapping
    happen here so that CLI, GUI, menu, and batch helpers all share one path.
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

    def apply_one(self, td: TweakDef) -> TweakResult:
        """Apply a single tweak, returning a typed result.

        If the apply function raises, logs the error for diagnostics.
        """
        if self._is_blocked(td):
            return TweakResult.SKIPPED_CORP
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
                for fut in concurrent.futures.as_completed(futures):
                    tid, res = fut.result()
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
    return exe.run_batch(list(_ALL_TWEAKS), "apply", parallel=parallel, max_workers=max_workers, progress_cb=progress_cb)


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
    return exe.run_batch(list(_ALL_TWEAKS), "remove", parallel=parallel, max_workers=max_workers, progress_cb=progress_cb)
