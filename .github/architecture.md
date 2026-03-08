# RegiLattice -- Architecture

> Deep-dive into data flow, dependency graph, and design decisions.
> Last verified: 2026-03-08 (v1.0.1-dev, 1 292 tweaks, 69 categories, ~17 511 tests).

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
    (1 292 tweaks)         (5 profiles)
         |
    tweaks/*.py               <-- 69 modules, auto-discovered
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
  gui.py  ----------------+---> gui_widgets.py (TweakRow, CategorySection)
                           +---> gui_theme.py (set_theme, colour constants)
                           +---> gui_tooltip.py (Tooltip, build_tooltip_text)
                           +---> gui_dialogs.py (import/export/scoop/about)
                           +---> tweaks/__init__.py
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
RegiLatticeGUI (gui.py)
  |
  +-- _root: tk.Tk (main window, DWM dark title bar on Win11)
  |
  +-- Toolbar
  |     +-- Search entry (StringVar, traces to _filter_rows)
  |     +-- Status filter dropdown (All/Applied/Default/Unknown)
  |     +-- Scope filter dropdown (All/User Only/Machine Only/Both)
  |     +-- Profile selector dropdown (Business/Gaming/Privacy/Minimal/Server)
  |     +-- Theme selector dropdown (Catppuccin Mocha/Latte, Nord, Dracula)
  |     +-- Force checkbox (bypass corp guard)
  |     +-- Selection counter ("N selected")
  |
  +-- Legend bar
  |     +-- Colour key: Applied / Default / Unknown / Corp Blocked / GPO
  |     +-- Keyboard shortcut hints
  |
  +-- Scrollable frame (canvas + scrollbar)
  |     +-- CategorySection (gui_widgets.py, collapsible)
  |           +-- Header: arrow + title + count + risk/scope/profile badges + Enable All / Disable All
  |           +-- TweakRow instances (gui_widgets.py):
  |                 +-- Status dot (colour) + status text (APPLIED/DEFAULT/UNKNOWN)
  |                 +-- Checkbox (batch selection)
  |                 +-- Toggle button (individual enable/disable)
  |                 +-- Badges: SCOPE, ADMIN, CORP, GPO, REC
  |                 +-- Tooltip (gui_tooltip.py: description, status, default/rec, tags, keys)
  |
  +-- Action bar (row 1: Apply Selected / Remove Selected)
  +-- Action bar (row 2: Save Snapshot / Restore Snapshot / Restore Point / Export PS1)
  +-- Action bar (row 3: Import JSON / Scoop Manager / Toggle Log / About)
  |
  +-- Summary stats bar: Applied / Default / Unknown / Recommended / GPO / Blocked
  +-- Progress bar + status label
  +-- Log viewer panel (hidden by default)
```

**GUI modules:**
- `gui.py` — Main window, deferred init, batch loading, threading dispatch
- `gui_widgets.py` — `TweakRow` (status, checkbox, toggle, badges, tooltip) + `CategorySection` (collapsible header, count badge, batch buttons)
- `gui_theme.py` — 4-theme support: Catppuccin Mocha/Latte, Nord, Dracula. `set_theme()` updates all module-level constants at runtime.
- `gui_tooltip.py` — `Tooltip` (follow-cursor Toplevel), `parse_description_metadata()` (LRU-cached), `has_recommendation()`, `build_tooltip_text()`
- `gui_dialogs.py` — `import_json_selection()`, `export_powershell()`, `open_scoop_manager()`, `show_about()`

**Threading model:**
- Deferred init: window appears instantly, corp check runs in background thread
- Category rows loaded in batches of 4 via `after()` scheduling
- All tweak operations (apply/remove/scan) run in daemon threads
- UI updates posted via `self._root.after(0, callback, ...)` (main thread only)
- Status refresh uses `status_map(parallel=True, max_workers=8)` for bulk detection
- Progress reported via callback functions passed to batch operations

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
    +-- ~13 tests x 1 228 tweaks = ~16 000 parametrized tests
  |
  test_tweaks_init.py
    +-- Plugin loader: module count, tweak count
    +-- Categories: sorted, non-empty
    +-- Profiles: all 5 profiles valid, apply/skip categories exist
    +-- Search: query matching across fields
    +-- Batch: apply_all/remove_all with dry_run
  |
  test_cli.py          # argparse, --list, --profile, apply/remove dispatch
  test_config.py       # AppConfig loading/defaults
  test_corpguard.py    # mocked probes, force override
  test_deps.py         # lazy_import, install_package, require
  test_elevation.py    # is_admin(), request_elevation(), run_elevated()
  test_gui_dialogs.py  # PS1 export, JSON import, about dialog
  test_gui_theme.py    # theme switching, colour attribute validation
  test_gui_tooltip.py  # tooltip text building, metadata parsing
  test_gui_widgets.py  # tweak scope classification
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
| **Catppuccin Mocha + 3 themes** | Modern dark theme with switchable alternatives (Latte, Nord, Dracula) |
| **Parallel status detection** | Thread-pool `status_map()` for fast GUI refresh across 1 228 tweaks |
| **`lru_cache` tooltip parsing** | Avoids re-parsing description metadata on every tooltip render |
| **Recommendation badges** | Visual indicator for tweaks with `Recommended:` in description |
| **Deferred loading** | Window appears instantly; categories loaded in batches of 4; corp check async |
| **Threaded GUI ops** | Prevents UI freeze during long batch operations |
| **Backup before write** | Every mutation is preceded by `reg.exe` export; enables manual rollback |
| **frozenset profiles** | Immutable category sets; hashable for caching |
