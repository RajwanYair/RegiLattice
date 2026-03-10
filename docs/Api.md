# API Reference

> Auto-generated reference for the `regilattice` public API.

## Core Modules

### `regilattice.tweaks`

The primary engine for tweak management.

#### `TweakDef`

```python
@dataclass(slots=True)
class TweakDef:
    id: str                                    # unique kebab-case identifier
    label: str                                 # human-readable name
    category: str                              # UI grouping
    apply_fn: Callable[..., None]              # apply the tweak
    remove_fn: Callable[..., None]             # revert the tweak
    detect_fn: Callable[[], bool] | None       # True = currently active
    needs_admin: bool = True                   # requires UAC elevation
    corp_safe: bool = False                    # safe on corporate networks
    registry_keys: list[str]                   # registry paths touched
    description: str = ""                      # tooltip / help text
    tags: list[str]                            # search keywords
    depends_on: list[str]                      # IDs of prerequisite tweaks
    min_build: int = 0                         # minimum Windows build number (0 = any)
```

#### `TweakResult`

```python
class TweakResult(str, Enum):
    APPLIED = "applied"
    REMOVED = "removed"
    NOT_APPLIED = "not applied"
    UNKNOWN = "unknown"
    UNCHANGED = "unchanged"
    SKIPPED_CORP = "skipped (corp)"
    SKIPPED_ADMIN = "skipped (admin)"
    SKIPPED_BUILD = "skipped (build)"
    ERROR = "error"
```

#### Functions

| Function | Signature | Description |
|----------|-----------|-------------|
| `all_tweaks()` | `→ list[TweakDef]` | All loaded tweaks, sorted by category then label |
| `get_tweak(id)` | `→ TweakDef \| None` | Look up a single tweak by ID |
| `categories()` | `→ list[str]` | All category names (sorted) |
| `tweaks_by_category()` | `→ dict[str, list[TweakDef]]` | Tweaks grouped by category |
| `search_tweaks(query)` | `→ list[TweakDef]` | Search by ID, label, tags, description |
| `tweak_status(td)` | `→ TweakResult` | Detect current state of a tweak |
| `status_map()` | `→ dict[str, TweakResult]` | Status of all tweaks (parallel detection) |
| `apply_all(...)` | `→ dict[str, TweakResult]` | Apply every tweak |
| `remove_all(...)` | `→ dict[str, TweakResult]` | Remove every tweak |
| `apply_profile(name)` | `→ dict[str, TweakResult]` | Apply a named profile |
| `available_profiles()` | `→ list[str]` | List profile names |
| `profile_info(name)` | `→ ProfileDef` | Profile metadata |
| `tweaks_for_profile(name)` | `→ list[TweakDef]` | Tweaks in a profile |
| `save_snapshot(path)` | `→ None` | Save current tweak state to JSON |
| `restore_snapshot(path)` | `→ dict[str, TweakResult]` | Restore from snapshot |
| `diff_snapshots(a, b)` | `→ dict[str, tuple[str, str]]` | Compare two snapshots |
| `windows_build()` | `→ int` | Current Windows build number |
| `tweak_scope(td)` | `→ str` | "user", "machine", or "both" |
| `filter_tweaks(...)` | `→ list[TweakDef]` | Composable multi-criterion filter (corp_safe, needs_admin, scope, category, min_build, tags, query) |
| `tweak_dependencies(td, *, transitive)` | `→ list[TweakDef]` | Dependency chain in topological order |
| `apply_tweaks(ids, *, include_deps, ...)` | `→ dict[str, TweakResult]` | Batch apply by ID list with auto dep resolution |
| `remove_tweaks(ids, ...)` | `→ dict[str, TweakResult]` | Batch remove by ID list |
| `tweaks_by_ids(ids)` | `→ list[TweakDef]` | Resolve IDs to TweakDef objects (unknown IDs silently skipped) |
| `tweaks_by_tag(tag)` | `→ list[TweakDef]` | All tweaks carrying the given tag (case-insensitive) |
| `tweaks_by_scope(scope)` | `→ list[TweakDef]` | All tweaks matching `"user"`, `"machine"`, or `"both"` |
| `tweaks_above_build(build)` | `→ list[TweakDef]` | Tweaks with `min_build <= build` |
| `tweak_risk_level(td)` | `→ str` | `"low"`, `"medium"`, or `"high"` risk classification |
| `tweak_count_by_scope()` | `→ dict[str, int]` | Counts per scope key (`user/machine/both`) |
| `category_counts()` | `→ dict[str, int]` | Tweak count per category name |

---

### `regilattice.registry`

Low-level Windows registry wrapper.

#### `RegistrySession`

```python
SESSION = RegistrySession()  # singleton

SESSION.set_dword(path, name, value)       # REG_DWORD
SESSION.set_string(path, name, value)      # REG_SZ
SESSION.set_binary(path, name, data)       # REG_BINARY
SESSION.set_qword(path, name, value)       # REG_QWORD (64-bit int)
SESSION.set_expand_string(path, name, v)   # REG_EXPAND_SZ
SESSION.set_multi_sz(path, name, values)   # REG_MULTI_SZ (list[str])
SESSION.set_value(path, name, val, type)   # any type
SESSION.read_dword(path, name) → int|None
SESSION.read_string(path, name) → str|None
SESSION.read_binary(path, name) → bytes|None
SESSION.read_qword(path, name) → int|None
SESSION.read_expand_string(path, name) → str|None
SESSION.read_multi_sz(path, name) → list[str]|None
SESSION.list_values(path) → list[tuple[str, object, int]]
SESSION.list_keys(path) → list[str]
SESSION.key_exists(path) → bool
SESSION.delete_value(path, name)
SESSION.delete_tree(path)
SESSION.backup(keys_list, label)
SESSION.log(message)
```

#### Helpers

| Function | Description |
|----------|-------------|
| `assert_admin(require_admin)` | Raises `AdminRequirementError` if not elevated |
| `is_windows()` | `→ bool` — True on Windows |
| `platform_summary()` | `→ str` — OS version string |

---

### `regilattice.cli`

Command-line interface entry point.

```bash
python -m regilattice                                  # interactive menu
python -m regilattice --list                           # list all tweaks
python -m regilattice --list --category Explorer       # filter by category
python -m regilattice --list --output json             # machine-readable JSON
python -m regilattice apply <id>                       # apply a tweak
python -m regilattice remove <id>                      # remove a tweak
python -m regilattice --gui                            # launch GUI
python -m regilattice --profile gaming                 # apply profile
python -m regilattice --validate                       # check TweakDef integrity
python -m regilattice --stats                          # scope/admin/corp breakdown
python -m regilattice --categories                     # list category names
python -m regilattice --list-categories --output json  # categories as JSON
python -m regilattice --snapshot before.json           # save state
python -m regilattice --restore before.json            # restore state
python -m regilattice --snapshot-diff a.json b.json    # compare snapshots
python -m regilattice --check                          # audit current state
python -m regilattice --diff gaming                    # delta vs profile
python -m regilattice --dry-run --list                 # dry-run mode
python -m regilattice --search "telemetry"             # search tweaks
python -m regilattice --search "telemetry" --output json  # search as JSON
python -m regilattice --list --scope user                  # filter by registry scope
python -m regilattice --list --scope machine               # machine-scope tweaks only
python -m regilattice --list --min-build 22621             # Win 11 22H2+ tweaks only
python -m regilattice --list --corp-safe                   # HKCU-only tweaks
python -m regilattice --list --needs-admin                 # admin-required tweaks
python -m regilattice --export-json out.json           # export as JSON
python -m regilattice --import-json in.json            # import selection
python -m regilattice --export-reg out.reg             # export registry
```

---

### `regilattice.config`

User configuration via `~/.regilattice.toml`.

```python
config = load_config()
config.force_corp      # bool — bypass corporate check
config.max_workers     # int — thread pool size
config.backup_dir      # Path — backup directory
config.auto_backup     # bool — automatic backups
config.theme           # str — UI theme ("system" | "mocha" | "latte" | "nord" | "dracula")
config.locale          # str — UI language tag (default "en")
```

---

### `regilattice.corpguard`

Corporate network detection.

| Function | Description |
|----------|-------------|
| `is_corporate_network()` | `→ bool` — True if corp environment detected |
| `assert_not_corporate(force_corp=False)` | Raises `CorporateNetworkError` if corp |
| `corp_guard_status()` | `→ dict` — Detailed detection results || `corp_guard_reasons()` | `→ list[str]` — Copy of reasons list from last detection |
| `reset_corp_cache()` | Clear cached detection result (useful in tests/hot-reload) |

---

### `regilattice.elevation`

UAC elevation helpers.

| Function | Description |
|----------|-------------|
| `is_admin()` | `→ bool` — True if running elevated |
| `request_elevation()` | Re-launch as admin via UAC |
| `run_elevated(cmd)` | Run a command with elevation |
| `ensure_admin_or_elevate()` | Elevate if not already admin |

---

### `regilattice.analytics`

Local-only usage analytics (no data sent anywhere).

| Function | Description |
|----------|-------------|
| `record_apply(tweak_id)` | Record a successful apply |
| `record_remove(tweak_id)` | Record a successful remove |
| `record_error()` | Record a generic error |
| `record_error_for(tweak_id)` | Record a per-tweak error (increments both global and per-ID counters) |
| `record_session()` | Record session start |
| `get_stats()` | `→ AnalyticsData` |
| `error_stats()` | `→ dict[str, int]` — per-tweak error counts |
| `top_tweaks(n=10)` | `→ list[tuple[str, int]]` — most applied |
| `reset()` | Clear all analytics |

### `regilattice.ratings`

Local tweak rating system (1–5 stars + optional notes).

| Function | Description |
|----------|-----------|
| `rate_tweak(tweak_id, stars, note="")` | Set rating for a tweak (1–5 stars) |
| `get_rating(tweak_id)` | `→ RatingEntry \| None` — retrieve stored rating |
| `all_ratings()` | `→ dict[str, RatingEntry]` — all rated tweaks |
| `remove_rating(tweak_id)` | Delete rating for a tweak |
| `top_rated(n=10)` | `→ list[tuple[str, int]]` — highest-rated tweaks |
| `average_rating()` | `→ float \| None` — mean stars across all rated tweaks |
| `rated_count()` | `→ int` — number of tweaks that have been rated |

---

| Module | Key Exports |
|--------|-------------|
| `gui.py` | `RegiLatticeGUI`, `launch()` |
| `gui_widgets.py` | `TweakRow`, `CategorySection` |
| `gui_theme.py` | `set_theme()`, `available_themes()`, `current_theme()` |
| `gui_tooltip.py` | `Tooltip`, `build_tooltip_text()`, `has_recommendation()` |
| `gui_dialogs.py` | `import_json_selection()`, `export_powershell()`, `show_about()` |

## Adding a New Tweak

1. Create/edit `regilattice/tweaks/<category>.py`
2. Implement `_apply_X`, `_remove_X`, `_detect_X` triplet
3. Append `TweakDef(...)` to module-level `TWEAKS` list
4. Run `pytest tests/test_tweaks_smoke.py -x`
5. No imports or registration needed — auto-discovered
