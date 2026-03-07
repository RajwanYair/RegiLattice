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

---

### `regilattice.registry`

Low-level Windows registry wrapper.

#### `RegistrySession`

```python
SESSION = RegistrySession()  # singleton

SESSION.set_dword(path, name, value)       # REG_DWORD
SESSION.set_string(path, name, value)      # REG_SZ
SESSION.set_value(path, name, val, type)   # any type
SESSION.read_dword(path, name) → int|None
SESSION.read_string(path, name) → str|None
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
python -m regilattice                         # interactive menu
python -m regilattice --list                  # list all tweaks
python -m regilattice apply <id>              # apply a tweak
python -m regilattice remove <id>             # remove a tweak
python -m regilattice --gui                   # launch GUI
python -m regilattice --profile gaming        # apply profile
python -m regilattice --snapshot before.json  # save state
python -m regilattice --restore before.json   # restore state
python -m regilattice --snapshot-diff a.json b.json           # compare
python -m regilattice --snapshot-diff a.json b.json --html r.html  # HTML report
python -m regilattice --check                 # audit current state
python -m regilattice --diff gaming           # delta vs profile
python -m regilattice --dry-run --list        # dry-run mode
python -m regilattice --search "telemetry"    # search tweaks
python -m regilattice --export-json out.json  # export as JSON
python -m regilattice --import-json in.json   # import selection
python -m regilattice --export-reg out.reg    # export registry
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
```

---

### `regilattice.corpguard`

Corporate network detection.

| Function | Description |
|----------|-------------|
| `is_corporate_network()` | `→ bool` — True if corp environment detected |
| `assert_not_corporate(force=False)` | Raises `CorporateNetworkError` if corp |
| `corp_guard_status()` | `→ dict` — Detailed detection results |

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
| `record_error()` | Record an error |
| `record_session()` | Record session start |
| `get_stats()` | `→ AnalyticsData` |
| `top_tweaks(n=10)` | `→ list[tuple[str, int]]` — most applied |
| `reset()` | Clear all analytics |

---

## GUI Modules

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
