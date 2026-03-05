# RegiLattice — Refactor Roadmap

> Full refactoring plan for best-in-class code clarity and enhanced capabilities.
> Generated 2025-03-05. Status: v2.0.0, 971+ tweaks, 64 categories, ~12 548 tests all passing.

---

## Phase 1 — Code Quality (Quick Wins)

### 1.1 Remove duplicate WiFi Sense functions in `network.py` ✅ DONE

- Duplicate `_apply_disable_wifi_sense` / `_remove_disable_wifi_sense` / `_detect_disable_wifi_sense` at lines ~941-953 removed.
- Duplicate `TweakDef(id="net-disable-wifi-sense")` removed (kept `id="disable-wifi-sense"`).

### 1.2 Fix all mypy errors ✅ DONE

- Removed 8 unused `# type: ignore[type-arg]` in `gui.py` (no longer needed in Python 3.10+).
- Removed unused `# type: ignore[attr-defined]` on `concurrent.futures` import.
- Added `# type: ignore[no-untyped-call]` for `iconbitmap()` (tkinter stub limitation).

### 1.3 Remove .bat launcher ✅ DONE

- `Launch-RegiLattice.bat` deleted. `Launch-RegiLattice.ps1` is the sole launcher.

### 1.4 Fix Pylance/Pyright `tk.Event` type parameter warnings ✅ DONE

- Changed all bare `tk.Event` annotations to `tk.Event[tk.Misc]` in `gui.py`.
- Pylance `standard` type-checking mode requires explicit generic parameters; mypy does not.

### 1.5 Extract TweakResult enum ✅ DONE

- Introduced `TweakResult(str, Enum)` with values: APPLIED, REMOVED, NOT_APPLIED, UNKNOWN, UNCHANGED, SKIPPED_CORP, SKIPPED_ADMIN, ERROR.
- Updated `apply_all`, `remove_all`, `apply_profile`, `status_map`, `restore_snapshot` in `tweaks/__init__.py`.
- Updated `cli.py`, `menu.py`, `gui.py` to use TweakResult comparisons.
- Updated all test mocks and assertions in `test_tweaks_init.py` and `test_cli.py`.

### 1.6 Extract theme constants to `gui_theme.py` ✅ DONE

- Created `regilattice/gui_theme.py` with all Catppuccin Mocha colour palette, status indicators, and font constants.
- Updated `gui.py` to import from `gui_theme` module and alias all constants for backward compatibility.

### 1.7 Deduplicate corporate guard check ✅ DONE

- Added `_is_corp_blocked(td, *, force_corp)` helper in `tweaks/__init__.py`.
- Replaced 4 inline corp-guard checks in `apply_profile`, `apply_all`, `remove_all`, `restore_snapshot` with the centralized helper.
- Lazy import of `corpguard.is_corporate_network` inside the helper.

---

## Phase 2 — Architecture Improvements

### 2.1 Decompose `RegiLatticeGUI` (1 432 lines -> ~5 modules) ✅ DONE

Extracted ~750 lines from gui.py (1850→1050) into three focused modules:
- `gui_tooltip.py`: Tooltip widget + `parse_description_metadata` + `build_tooltip_text`
- `gui_widgets.py`: `TweakRow` + `CategorySection` classes
- `gui_dialogs.py`: `import_json_selection`, `export_powershell`, `open_scoop_manager`, `show_about`

All dialog methods in gui.py now delegate to gui_dialogs. Proper `Callable`/`Sequence`
typing eliminates circular import issues.

### 2.2 Introduce MVC separation

```text
Model: tweaks/__init__.py (TweakDef, all_tweaks, status_map)
       + new TweakExecutor (apply/remove/detect pipeline)
Controller: gui_dispatch.py (threading, result routing)
View: gui.py + gui_rows.py + gui_toolbar.py (pure widget code)
```

### 2.3 Extract `TweakExecutor` class ✅ DONE

- Created `TweakExecutor` class in `tweaks/__init__.py` with `_is_blocked()`, `apply_one()`, `remove_one()`, `run_batch()` methods.
- Refactored `apply_all`, `remove_all`, `apply_profile`, `restore_snapshot` to delegate to `TweakExecutor`.
- Backward-compatible `_is_corp_blocked` standalone function kept.

### 2.4 Thread safety in GUI ✅ DONE

- Added `_running` flag and `_cancel = threading.Event()` for batch operation control.
- Stored button references as instance variables; `_set_running()` toggles button state.
- `_dispatch()` checks `_running` flag — if already running, signals cancellation.
- `_run_tweaks()` checks `_cancel.is_set()` each iteration with for/else pattern.

### 2.5 ProfileDef dataclass ✅ DONE

- Created `ProfileDef` dataclass (slots=True) with id, description, apply_categories, skip_categories.
- Replaced `_PROFILES: dict[str, dict[str, object]]` with `_PROFILES: dict[str, ProfileDef]`.
- Updated `profile_info()` return type to `ProfileDef | None`, eliminating all `type: ignore[assignment]` casts.
- Note: Kept profiles in `tweaks/__init__.py` (not separate plugin modules) — pragmatic choice for only 5 profiles.

---

## Phase 3 — Enhanced Capabilities

### 3.1 Dry-run mode for CLI ✅ DONE

- Added `--dry-run` argument to CLI parser.
- Handler sets `SESSION._dry_run = True` before executing commands.
- All registry mutations log `[DRY-RUN]` prefix without touching the registry.

### 3.2 Undo/redo stack

Track tweak operations in a stack for quick undo:

```python
class UndoStack:
    def push(self, td: TweakDef, action: TweakResult) -> None: ...
    def undo(self) -> TweakDef | None: ...
    def redo(self) -> TweakDef | None: ...
```

### 3.3 Snapshot diff tool ✅ DONE

- Added `--snapshot-diff FILE_A FILE_B` CLI argument.
- Implemented `diff_snapshots(path_a, path_b)` in `tweaks/__init__.py`.
- Prints a formatted table of changed tweak IDs with old→new state.

### 3.4 Tweak dependency graph ✅ DONE

- Added `depends_on: list[str]` field to `TweakDef` dataclass.
- Dependency validation in `_load_plugins()` after module loading.
- Implemented `_topo_sort()` using Kahn's algorithm (with `collections.deque`).
- `run_batch()` topologically orders tweaks before executing.
- Added `test_depends_on_is_list` smoke test and `TestTopoSort` with 5 unit tests.

### 3.5 Rollback on error ✅ DONE

- Added `restore_backup()` method to `RegistrySession` (imports `.reg` files via `reg import`).
- `TweakExecutor.apply_one()` and `remove_one()` catch exceptions and log via `SESSION.log()`.
- Returns `TweakResult.ERROR` on failure with full diagnostic logging.

### 3.6 Configuration file support ✅ DONE

- Created `regilattice/config.py` with `AppConfig` dataclass and `load_config()` function.
- Supports `tomllib` (3.11+) with `tomli` fallback (3.10), graceful degradation if neither available.
- Added `--config` CLI flag; config values used as defaults (CLI flags override).

---

## Phase 4 — Testing & CI

### 4.1 Add integration tests

- CLI -> tweaks -> registry (dry-run) end-to-end tests
- GUI tests with tkinter in headless mode (`Xvfb` on Linux CI, or mock)

### 4.2 Add GUI unit tests

Test `_TweakRow`, `_CategorySection`, `_dispatch()` with mock tkinter root.

### 4.3 Pin dev dependencies ✅ DONE

- Pinned: `pytest>=8,<9`, `pytest-cov>=7,<8`, `mypy>=1.5,<2`, `ruff>=0.4,<1`.

### 4.4 Add CI/CD pipeline ✅ DONE

- Created `.github/workflows/ci.yml` with matrix: Python 3.10-3.13, windows-latest.
- Steps: checkout, setup-python, pip install, ruff check, mypy, pytest.

### 4.5 Fix flaky `test_backup_creates_directory` ✅ DONE

- Fixed by mocking `subprocess.run` — no longer triggers WinError 6.
- All 13,793 tests now pass without skip filters.

---

## Phase 5 — Documentation & Distribution

### 5.1 Architecture diagram ✅ DONE

- Added Mermaid flowchart to README.md showing full data flow:
  Interfaces (CLI/Menu/GUI) → Core → Plugin Modules → Windows Registry.

### 5.2 Contributing guide ✅ DONE

- Created `CONTRIBUTING.md` with quickstart, project structure, tweak triplet pattern,
  TweakDef registration, coding standards, and commit conventions.

### 5.3 PyInstaller / standalone exe

Add `regilattice.spec` for building standalone Windows executable.

### 5.4 Scoop / Chocolatey package

Define manifests for Windows package managers.

---

## Execution Order (Recommended)

```text
Phase 1.5 -> 1.6 -> 1.7       (quick wins)
Phase 2.1 -> 2.3 -> 2.2       (GUI decomposition first, then MVC)
Phase 2.4 -> 2.5              (thread safety, profiles)
Phase 4.4 -> 4.3 -> 4.1       (CI first for safety net)
Phase 3.1 -> 3.5 -> 3.2       (dry-run, rollback, undo)
Phase 5.1 -> 5.3              (docs, packaging)
Phase 3.3 -> 3.4 -> 3.6       (advanced features last)
```

---

## Phase 6 — Performance Optimizations ✅ DONE

- Built `_TWEAKS_BY_CAT` reverse category index — eliminates ~62K iterations at startup.
- Profile filtering uses `_TWEAKS_BY_CAT` (O(categories) instead of O(all_tweaks)).
- `tweaks_by_category()` returns cached dict copy instead of rebuilding.
- `_topo_sort()` uses `collections.deque` for O(1) popleft (was O(n) `list.pop(0)`).
- Cached `has_recommendation()` results per tweak id (`_REC_CACHE`).
- GUI passes pre-computed `statuses` dict to `CategorySection.update_count()`.
- GUI `_filter_rows()` uses `_cached_statuses` instead of calling `tweak_status()` per row.
- GUI caches `rec_count` and `gpo_count` (static per session — computed once).
