# RegiLattice -- Architecture

> Deep-dive into data flow, dependency graph, and design decisions.
> Last verified: 2026-03-05 (v2.0.0, 578 tweaks, 43 categories, ~5 927 tests).

---

## 1  High-Level Data Flow

```
                   User
                    |
         +----------+-----------+
         |          |           |
       CLI       Menu        GUI
    (cli.py)   (menu.py)   (gui.py)
         |          |           |
         +----------+-----------+
                    |
            tweaks/__init__.py        <-- plugin loader, profiles, batch ops
                    |
         +----------+-----------+
         |                      |
    TweakDef list          _PROFILES dict
    (578 tweaks)           (5 profiles)
         |
    tweaks/*.py               <-- 43 modules, auto-discovered
         |
    +----+----+
    |         |
registry.py  corpguard.py
    |              |
  winreg      ctypes / subprocess
  reg.exe     dsregcmd, Get-NetAdapter
```

## 2  Module Dependency Graph

```
regilattice/
  __init__.py             # standalone (importlib.metadata)
  __main__.py  ----------> cli.py
  cli.py  ----------------+---> tweaks/__init__.py
                           +---> registry.py
                           +---> corpguard.py
                           +---> menu.py
  menu.py  ---------------+---> tweaks/__init__.py
                           +---> registry.py
                           +---> corpguard.py
                           +---> tweaks/maintenance.py (create_restore_point)
  gui.py  ----------------+---> tweaks/__init__.py
                           +---> registry.py
                           +---> corpguard.py
                           +---> tweaks/maintenance.py (create_restore_point)
  elevation.py  ----------+---> registry.py
  deps.py                  # standalone (subprocess, importlib)
  corpguard.py  ----------+---> registry.py
  registry.py              # standalone (winreg, subprocess, pathlib)
  tweaks/__init__.py  -----+---> (auto-imports all tweaks/*.py modules)
  tweaks/*.py  ------------+---> registry.py (SESSION, assert_admin)
                            +---> tweaks/__init__.py (TweakDef only)
```

**Key rule:** No circular imports. Tweak modules import only from
`registry.py` (for SESSION/assert_admin) and `tweaks/__init__.py` (for TweakDef).

## 3  Plugin Loader Sequence

```
Application starts
  |
  v
tweaks/__init__.py is imported
  |
  v
_load_plugins() runs automatically at module scope
  |
  v
pkgutil.iter_modules(tweaks.__path__)
  |
  v
For each .py file (skip _-prefixed):
  |
  +---> importlib.import_module("regilattice.tweaks.<name>")
  |       |
  |       v
  |     Module executes, defining TWEAKS: list[TweakDef]
  |
  +---> Collect getattr(mod, "TWEAKS", [])
  |
  +---> Check for duplicate IDs (ValueError if found)
  |
  +---> Append to _ALL_TWEAKS, index in _TWEAK_INDEX
  |
  v
Sort _ALL_TWEAKS by (category.lower(), label.lower())
  |
  v
Ready -- all_tweaks() returns the sorted list
```

## 4  Tweak Execution Flow

### Apply

```
User picks "apply <id>"
  |
  v
get_tweak(id) -> TweakDef
  |
  v
assert_not_corporate(force=flag)   # raises CorporateNetworkError
  |
  v
td.apply_fn(require_admin=True)
  |
  v
  _apply_<name>()
    |
    +---> assert_admin(require_admin)   # raises AdminRequirementError
    +---> SESSION.backup([keys], label) # reg.exe export to .reg file
    +---> SESSION.set_dword(...)        # winreg write
    +---> SESSION.log(message)          # append to RegiLattice.log
```

### Detect

```
GUI refresh / status_map()
  |
  v
For each TweakDef:
  |
  v
td.detect_fn()  (if not None)
  |
  v
Returns True (applied), False (default), or None triggers "unknown"
  |
  v
tweak_status(td) -> "applied" | "not applied" | "unknown"
```

## 5  GUI Architecture

```
RegiLatticeGUI (gui.py, ~1 192 lines)
  |
  +-- _root: tk.Tk (main window)
  |
  +-- Toolbar
  |     +-- Search entry (StringVar, traces to _filter_rows)
  |     +-- Status filter dropdown (All/Applied/Default/Unknown)
  |     +-- Profile selector dropdown (Business/Gaming/Privacy/Minimal/Server)
  |     +-- Force checkbox (bypass corp guard)
  |     +-- Selection counter ("N selected")
  |
  +-- Legend bar
  |     +-- Colour key: Applied / Default / Unknown / Corp Blocked
  |     +-- Keyboard shortcut hints
  |
  +-- Scrollable frame (canvas + scrollbar)
  |     +-- _CategorySection (collapsible, per category)
  |           +-- Header: arrow + title + count badge + Enable All / Disable All
  |           +-- _TweakRow instances:
  |                 +-- Status dot (colour) + status text (APPLIED/DEFAULT/UNKNOWN)
  |                 +-- Checkbox (batch selection)
  |                 +-- Toggle button (individual enable/disable)
  |                 +-- Badges: ADMIN, CORP, REC (recommendation)
  |                 +-- _Tooltip (hover: description, status, default/rec, tags, keys)
  |
  +-- Action bar (row 1: Apply Selected / Remove Selected)
  +-- Action bar (row 2: Save Snapshot / Restore Snapshot / Restore Point / Export PS1)
  |
  +-- Summary stats bar: Applied / Default / Unknown / Recommended / Blocked
  +-- Progress bar + status label
```

**Key GUI classes:**
- `_Tooltip` - hover tooltip following mouse with `update_text()` support
- `_TweakRow` - single tweak row widget with status dot, text, checkbox, toggle, badges
- `_CategorySection` - collapsible section with header, count badge, batch buttons
- `RegiLatticeGUI` - main application window

**GUI helper functions:**
- `_parse_description_metadata(description)` - extracts `Default:` / `Recommended:` hints (cached via `lru_cache`)
- `_has_recommendation(td)` - checks if description contains recommendation
- `_build_tooltip_text(td, status)` - builds rich tooltip with all metadata

**Threading model:**
- All tweak operations (apply/remove/scan) run in daemon threads
- UI updates posted via `self._root.after(0, callback, ...)` (main thread only)
- Status refresh uses `status_map(parallel=True, max_workers=8)` for bulk detection
- Progress reported via callback functions passed to batch operations

**Theme constants (Catppuccin Mocha):**

| Variable | Hex | Name | Usage |
|---|---|---|---|
| `_BG` | `#1E1E2E` | Base | Main background |
| `_BG_SURFACE` | `#24273A` | Surface0 | Category headers |
| `_CARD_BG` | `#313244` | Surface1 | Tweak row background |
| `_CARD_HOVER` | `#45475A` | Surface2 | Hover state |
| `_FG` | `#CDD6F4` | Text | Primary text |
| `_FG_DIM` | `#6C7086` | Overlay0 | Secondary text |
| `_ACCENT` | `#89B4FA` | Blue | Links, active items |
| `_OK_GREEN` | `#A6E3A1` | Green | Applied status |
| `_WARN_YELLOW` | `#F9E2AF` | Yellow | Unknown status |
| `_ERR_RED` | `#F38BA8` | Red | Error / Corp blocked |
| `_PURPLE` | `#CBA6F7` | Mauve | Restore buttons |
| `_TEAL` | `#94E2D5` | Teal | Recommendation badges |
| `_STATUS_DEFAULT` | `#89DCEB` | Sky | Default (not applied) |

## 6  Profile System

```python
_PROFILES: dict[str, dict[str, object]] = {
    "business": {
        "description": "...",
        "apply_categories": frozenset({...}),   # categories to apply
        "skip_categories": frozenset({...}),    # categories to hide
    },
    ...
}
```

`apply_profile(name)` iterates `tweaks_for_profile(name)` and calls each
`TweakDef.apply_fn()`, respecting corp-safe flags. Returns
`{tweak_id: "applied" | "skipped (corp)" | "error: ..."}`.

## 7  Corporate Guard Decision Tree

```
is_corporate_network()
  |
  +---> _is_domain_joined()          AD domain membership
  |       True? -> return True
  |
  +---> _is_azure_ad_joined()        dsregcmd /status
  |       True? -> return True
  |
  +---> _has_vpn_adapter()           Get-NetAdapter vs 13 vendor keywords
  |       True? -> return True
  |
  +---> _has_gpo_indicators()        HKLM\..\Policies registry check
  |       True? -> return True
  |
  +---> _is_managed_device()         SCCM/Intune enrollment
  |       True? -> return True
  |
  +---> return False                  Not corporate
```

When corporate is detected and a tweak has `corp_safe=False`:
- CLI: prints error, exits 6
- GUI: grays out the tweak
- Override: `--force` flag (CLI) or force checkbox (GUI)

## 8  Registry Session Internals

```
RegistrySession (registry.py)
  |
  +-- _dry_run: bool          # True in tests (no actual reg writes)
  +-- base_dir: Path          # backup directory root
  +-- log_path: Path          # RegiLattice.log location
  |
  +-- _split_root(path)       # "HKLM\...\Key" -> (HKEY_LOCAL_MACHINE, "...\Key")
  +-- set_dword()             # winreg.CreateKeyEx + SetValueEx(REG_DWORD)
  +-- set_string()            # winreg.CreateKeyEx + SetValueEx(REG_SZ)
  +-- read_dword()            # winreg.OpenKeyEx + QueryValueEx -> int|None
  +-- read_string()           # winreg.OpenKeyEx + QueryValueEx -> str|None
  +-- key_exists()            # winreg.OpenKeyEx try/except
  +-- delete_value()          # winreg.OpenKeyEx + DeleteValue
  +-- delete_tree()           # recursive key deletion
  +-- backup()                # subprocess.run(["reg", "export", ...])
  +-- log()                   # append timestamped message to log file
```

Path parsing: `_ROOTS` maps `HKEY_LOCAL_MACHINE`, `HKLM`, `HKEY_CURRENT_USER`, `HKCU`,
`HKEY_CLASSES_ROOT`, `HKCR` to winreg handles.

## 9  Test Architecture

```
tests/
  conftest.py
    +-- dry_session fixture      # RegistrySession(_dry_run=True, base_dir=tmp_path)
    +-- all_tweaks_list fixture  # session-scoped, cached list of all TweakDef
  |
  test_tweaks_smoke.py
    +-- Auto-parametrized over all_tweaks_list
    +-- Tests: apply_fn signature, remove_fn signature, detect_fn callable
    +-- Tests: ID format (kebab-case), ID uniqueness, required fields
    +-- ~10 tests x 578 tweaks = ~5 780 parametrized tests
  |
  test_tweaks_init.py
    +-- Plugin loader: module count, tweak count
    +-- Categories: sorted, non-empty
    +-- Profiles: all 5 profiles valid, apply/skip categories exist
    +-- Search: query matching across fields
    +-- Batch: apply_all/remove_all with dry_run
  |
  test_cli.py          # argparse, --list, --profile, apply/remove dispatch
  test_corpguard.py    # mocked probes, force override
  test_deps.py         # lazy_import, fallback stubs
  test_elevation.py    # is_admin(), request_elevation()
  test_menu.py         # Menu class, banner, selection
  test_registry.py     # set/read/delete, backup, logging, path parsing
```

## 10  Design Decisions

| Decision | Rationale |
|---|---|
| **Plugin auto-discovery** | Zero-friction contributor experience; no central registration file to conflict on |
| **stdlib-only runtime** | Installs anywhere without pip; works in restricted environments |
| **TweakDef dataclass** | Single source of truth; enables parametrized testing, GUI rendering, CLI listing |
| **`require_admin` kwarg** | Uniform API contract; allows testing with `require_admin=False` in CI |
| **`_dry_run` mode** | Tests can validate tweak logic without touching the real registry |
| **Corporate guard** | Prevents accidental damage on managed machines; legal/compliance safety |
| **Catppuccin Mocha** | Modern dark theme with excellent contrast ratios; consistent palette |
| **Parallel status detection** | Thread-pool `status_map()` for fast GUI refresh across 578 tweaks |
| **`lru_cache` tooltip parsing** | Avoids re-parsing description metadata on every tooltip render |
| **Recommendation badges** | Visual indicator for tweaks with `Recommended:` in description |
| **Threaded GUI ops** | Prevents UI freeze during long batch operations |
| **Backup before write** | Every mutation is preceded by `reg.exe` export; enables manual rollback |
| **frozenset profiles** | Immutable category sets; hashable for caching |
