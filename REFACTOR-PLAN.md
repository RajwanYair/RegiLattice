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

### 1.5 Extract TweakResult enum

Replace opaque `dict[str, str]` results with a proper enum:

```python
class TweakResult(Enum):
    APPLIED = "applied"
    REMOVED = "removed"
    SKIPPED_CORP = "skipped_corp"
    SKIPPED_ADMIN = "skipped_admin"
    ERROR = "error"
    UNKNOWN = "unknown"
```

Affected: `tweaks/__init__.py` (apply_all, remove_all, apply_profile, status_map), `gui.py`, `cli.py`, `menu.py`.

### 1.6 Extract theme constants to `gui_theme.py`

Move 20+ color constants, font definitions, and padding values to a dedicated module:

```python
# gui_theme.py
class CatppuccinMocha:
    BASE = "#1E1E2E"
    ACCENT = "#89B4FA"
    FG = "#CDD6F4"
    ...
```

### 1.7 Deduplicate corporate guard check

Extract `@corp_safe` decorator or `safe_execute()` helper:

```python
def safe_execute(fn: Callable, *, force: bool = False) -> TweakResult:
    try:
        assert_not_corporate(force=force)
    except CorporateNetworkError:
        return TweakResult.SKIPPED_CORP
    fn()
    return TweakResult.APPLIED
```

Affected: `cli.py`, `menu.py`, `gui.py` (3 locations -> 1 shared function).

---

## Phase 2 — Architecture Improvements

### 2.1 Decompose `RegiLatticeGUI` (1 432 lines -> ~5 modules)

Split the god class into:

| Module | Responsibility | Approx Lines |
|--------|---------------|-------------|
| `gui.py` | Main window shell, keyboard bindings | ~200 |
| `gui_theme.py` | Colors, fonts, padding, styles | ~80 |
| `gui_rows.py` | `_TweakRow`, `_CategorySection`, widget factories | ~300 |
| `gui_toolbar.py` | Search bar, filters, profile dropdown, action buttons | ~200 |
| `gui_dispatch.py` | Threaded execution, progress callbacks, result handling | ~200 |
| `gui_dialogs.py` | About, Scoop manager, Import JSON, Export PS1 | ~300 |
| `gui_tooltip.py` | `_Tooltip` class, tooltip text builder | ~100 |

### 2.2 Introduce MVC separation

```text
Model: tweaks/__init__.py (TweakDef, all_tweaks, status_map)
       + new TweakExecutor (apply/remove/detect pipeline)
Controller: gui_dispatch.py (threading, result routing)
View: gui.py + gui_rows.py + gui_toolbar.py (pure widget code)
```

### 2.3 Extract `TweakExecutor` class

Centralize tweak execution with corp guard, admin check, backup, logging:

```python
class TweakExecutor:
    def __init__(self, session: RegistrySession, *, force_corp: bool = False):
        self.session = session
        self.force_corp = force_corp

    def apply(self, td: TweakDef) -> TweakResult:
        if not self.force_corp and not td.corp_safe:
            if is_corporate_network():
                return TweakResult.SKIPPED_CORP
        self.session.backup(td.registry_keys, td.id)
        td.apply_fn(require_admin=td.needs_admin)
        return TweakResult.APPLIED
```

### 2.4 Thread safety in GUI

Replace direct callback invocation from worker threads with `root.after()`:

```python
def _thread_safe_cb(self, tid: str, res: TweakResult) -> None:
    self._root.after(0, lambda: self._update_row_status(tid, res))
```

Add cancellation token pattern for long-running batch operations.

### 2.5 Profile system as plugins

Move `_PROFILES` dict from `tweaks/__init__.py` to separate profile modules:

```text
regilattice/profiles/
    __init__.py   # auto-discovers profiles (same pattern as tweaks)
    business.py   # ProfileDef(id="business", categories=[...])
    gaming.py
    privacy.py
    minimal.py
    server.py
```

---

## Phase 3 — Enhanced Capabilities

### 3.1 Dry-run mode for CLI

Expose the existing `_dry_run=True` mechanism:

```bash
regilattice apply --dry-run perf-startup-delay
# Output: Would set HKLM\...\StartupDelay to 0 (currently: 10)
```

### 3.2 Undo/redo stack

Track tweak operations in a stack for quick undo:

```python
class UndoStack:
    def push(self, td: TweakDef, action: TweakResult) -> None: ...
    def undo(self) -> TweakDef | None: ...
    def redo(self) -> TweakDef | None: ...
```

### 3.3 Snapshot diff tool

```bash
regilattice snapshot diff baseline.json current.json
# Output: 12 tweaks changed, 5 applied, 7 removed
```

### 3.4 Tweak dependency graph

Add optional `depends_on` field to `TweakDef`:

```python
@dataclass(slots=True)
class TweakDef:
    ...
    depends_on: list[str] = field(default_factory=list)
```

### 3.5 Rollback on error

If `apply_fn` raises, auto-restore from the backup created at start:

```python
try:
    td.apply_fn(require_admin=td.needs_admin)
except Exception:
    self.session.restore_backup(td.id)
    return TweakResult.ERROR
```

### 3.6 Configuration file support

Support `~/.regilattice.toml` for user preferences:

```toml
[general]
force_corp = false
theme = "catppuccin-mocha"
max_workers = 8

[backups]
directory = "~/.regilattice/backups"
auto_backup = true
```

---

## Phase 4 — Testing & CI

### 4.1 Add integration tests

- CLI -> tweaks -> registry (dry-run) end-to-end tests
- GUI tests with tkinter in headless mode (`Xvfb` on Linux CI, or mock)

### 4.2 Add GUI unit tests

Test `_TweakRow`, `_CategorySection`, `_dispatch()` with mock tkinter root.

### 4.3 Pin dev dependencies

```toml
dev = ["pytest>=7,<9", "pytest-cov>=4,<6", "mypy>=1.5,<2", "ruff>=0.4,<1"]
```

### 4.4 Add CI/CD pipeline

```yaml
# .github/workflows/ci.yml
name: CI
on: [push, pull_request]
jobs:
  lint-and-test:
    runs-on: windows-latest
    strategy:
      matrix:
        python-version: ["3.10", "3.11", "3.12", "3.13", "3.14"]
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-python@v5
        with: { python-version: "${{ matrix.python-version }}" }
      - run: pip install -e ".[dev]"
      - run: ruff check regilattice/ tests/
      - run: mypy regilattice/
      - run: pytest tests/ -x --tb=short
```

### 4.5 Fix flaky `test_backup_creates_directory`

Isolate with mock subprocess and temp filesystem to avoid WinError 6.

---

## Phase 5 — Documentation & Distribution

### 5.1 Architecture diagram

Add Mermaid diagram to README showing data flow:

```text
Plugin modules -> tweaks/__init__.py -> TweakExecutor -> RegistrySession -> winreg
                                      |
                                cli/gui/menu
```

### 5.2 Contributing guide

Expand `_template.py` with 2-minute quickstart + video link.

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
