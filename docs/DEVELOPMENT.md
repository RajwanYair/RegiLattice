# RegiLattice — Development Guide

> Local setup, workflow, testing, and contribution instructions.
> Last updated: 2026-03-08 · v1.0.1

---

## Prerequisites

| Requirement | Version | Notes |
|---|---|---|
| Python | 3.10 – 3.14 | Required. Windows is the primary platform. |
| Git | 2.40+ | For version control and pre-commit hooks. |
| Windows | 10/11 (build 19041+) | Required for registry operations. |
| VS Code | 1.85+ | Recommended editor with extensions in `.vscode/extensions.json`. |

---

## Quick Start (Windows)

```powershell
# 1. Clone the repository
git clone https://github.com/RajwanYair/RegiLattice.git
cd RegiLattice

# 2. Install in editable mode with all dev dependencies
python -m pip install -e ".[dev]"

# 3. Verify the installation
python -m regilattice --version
python -m regilattice --list | Select-Object -First 5

# 4. Run the full test suite
python -m pytest tests/ -v --tb=short
```

---

## Python Installation

The project requires a **non-alias** Python installation. On Windows, the
Windows Store alias (`python.exe` in `WindowsApps`) does not support all
registry operations.

**Recommended path:** `%LOCALAPPDATA%\Python\bin\python.exe`

To find your Python:

```powershell
where.exe python
python -c "import sys; print(sys.executable)"
```

---

## Dev Dependencies

Install everything needed for local development:

```powershell
python -m pip install -e ".[dev]"
```

The `[dev]` extras include:

| Package | Purpose |
|---|---|
| `pytest>=8` | Test runner |
| `pytest-cov>=7` | Code coverage |
| `pytest-mock` | Mock fixtures for unit tests |
| `hypothesis` | Property-based testing |
| `mypy>=1.5` | Static type checking (strict mode) |
| `ruff>=0.4` | Linter and formatter |

---

## Common Development Tasks

| Task | Command |
|---|---|
| Lint | `python -m ruff check regilattice/ tests/` |
| Format | `python -m ruff format regilattice/ tests/` |
| Type-check | `python -m mypy regilattice/` |
| Test | `python -m pytest tests/ -v --tb=short` |
| Test + coverage | `python -m pytest tests/ --cov=regilattice --cov-report=term-missing` |
| Run GUI (admin) | `python -m regilattice --gui` |
| List tweaks | `python -m regilattice --list` |
| Validate tweaks | `python -m regilattice --validate` |
| Show stats | `python -m regilattice --stats` |

VS Code tasks (`Ctrl+Shift+B`) provide IDE integration for all commands above.
See `.vscode/tasks.json`.

---

## Project Structure

```
regilattice/
├── __init__.py          # Package version (__version__)
├── __main__.py          # Entry: delegates to cli.main()
├── cli.py               # argparse CLI  (apply/remove/list/gui/profile/validate)
├── menu.py              # Interactive numbered console menu
├── gui.py               # Tkinter GUI main window
├── gui_widgets.py       # TweakRow + CategorySection widgets
├── gui_theme.py         # 4-theme engine (Catppuccin Mocha/Latte, Nord, Dracula)
├── gui_tooltip.py       # Tooltip widget + description metadata parser
├── gui_dialogs.py       # Import JSON, Export PS1, Scoop Manager, About dialog
├── registry.py          # RegistrySession — winreg wrapper + backup + logging
├── config.py            # User config via ~/.regilattice.toml (AppConfig)
├── corpguard.py         # Corporate network detection
├── elevation.py         # UAC elevation helpers
├── deps.py              # Lazy-import + auto-install for optional packages
├── analytics.py         # Local-only usage analytics
├── ratings.py           # Local tweak rating system
├── locale.py            # i18n string table for UI labels
├── hwinfo.py            # Hardware detection, adaptive config
├── marketplace.py       # Third-party plugin discovery & loading
└── tweaks/
    ├── __init__.py      # TweakDef dataclass, plugin loader, profiles, batch ops
    ├── _template.py     # Contributor guide (NOT loaded by plugin loader)
    └── *.py             # 69 category modules, each exports TWEAKS: list[TweakDef]
```

---

## Adding a New Tweak

1. **Create or edit** `regilattice/tweaks/<category>.py`.

2. **Define constants** at module top:

   ```python
   _KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\..."
   ```

3. **Implement the function triplet**:

   ```python
   def _apply_my_tweak(*, require_admin: bool = True) -> None:
       assert_admin(require_admin)
       SESSION.backup([_KEY], "my-tweak")
       SESSION.set_dword(_KEY, "SettingName", 1)

   def _remove_my_tweak(*, require_admin: bool = True) -> None:
       assert_admin(require_admin)
       SESSION.delete_value(_KEY, "SettingName")

   def _detect_my_tweak() -> bool:
       return SESSION.read_dword(_KEY, "SettingName") == 1
   ```

4. **Append a `TweakDef`** to the module-level `TWEAKS` list:

   ```python
   TweakDef(
       id="category-my-tweak",          # globally unique, kebab-case
       label="My Tweak Label",
       category="Category Name",
       apply_fn=_apply_my_tweak,
       remove_fn=_remove_my_tweak,
       detect_fn=_detect_my_tweak,
       needs_admin=True,
       corp_safe=False,
       registry_keys=[_KEY],
       description="What this tweak does. Default: 0. Recommended: 1.",
       tags=["performance", "keyword"],
   )
   ```

5. **Run smoke tests** and linter:

   ```powershell
   python -m pytest tests/test_tweaks_smoke.py -x --tb=short
   python -m ruff check regilattice/ tests/
   ```

**No imports or registration needed** — the plugin loader auto-discovers every
`.py` in `tweaks/` that is not `_`-prefixed.

---

## TweakDef Fields Reference

| Field | Type | Required | Description |
|---|---|---|---|
| `id` | `str` | ✅ | Globally unique kebab-case ID, prefixed with category slug |
| `label` | `str` | ✅ | Human-readable name shown in GUI/CLI |
| `category` | `str` | ✅ | UI grouping (must match a known category name) |
| `apply_fn` | `Callable` | ✅ | Function that applies the tweak |
| `remove_fn` | `Callable` | ✅ | Function that reverts the tweak |
| `detect_fn` | `Callable \| None` | — | Returns `True` when tweak is currently active |
| `needs_admin` | `bool` | — | `True` = requires UAC / HKLM access (default: `True`) |
| `corp_safe` | `bool` | — | `True` = HKCU-only, safe on corporate machines |
| `registry_keys` | `list[str]` | — | Registry paths touched (used for backup and tooltips) |
| `description` | `str` | — | Tooltip text; optionally contains `Default:` / `Recommended:` hints |
| `tags` | `list[str]` | — | Search keywords |
| `depends_on` | `list[str]` | — | IDs of tweaks this one depends on |
| `min_build` | `int` | — | Minimum Windows build number (0 = any) |
| `side_effects` | `str` | — | Free-text description of what may break when applied |
| `source_url` | `str` | — | KB article or documentation URL |

---

## GUI Performance Architecture

> Added in Sprint 7. Describes how the three main optimisations keep the GUI
> responsive across 1 200+ tweak rows and 69 categories.

### 1. Lazy Widget Build (startup)

**Problem:** Building all ~6 000 Tk widgets (1 200 rows × 5 widgets) at startup
was the primary cause of 3–8 second cold-start times.

**Solution:** `CategorySection` defers widget construction until the section is
first expanded.

```
startup
  ├── all 69 header frames created      (cheap — header only)
  └── row frames:  None × 1 200         (zero cost)

user expands "Explorer"
  └── _build_row_widgets() fires once
        ├── builds 41 × 5 = 205 widgets
        └── fires set_on_rows_built(callback)
              └── gui._wire_section_bindings() wires bindings + applies cached statuses
```

**Impact:** Cold-start widget count drops from ~6 000 to ~200 (headers only).
Each subsequent section expansion costs only: `rows × 5` widgets once.

### 2. Delta Status Updates (status refresh)

**Problem:** `_apply_statuses()` was reconfiguring all 1 200 rows on every
background status poll, generating thousands of redundant Tk IPC calls.

**Solution:** `_prev_statuses: dict[str, TweakResult]` caches the last propagated
result per tweak ID.  `_apply_statuses()` skips `widget.configure()` for any row
where the status is identical to the cached value.

```python
prev = self._prev_statuses
for row in self._tweak_rows:
    st = statuses.get(row.td.id, TweakResult.UNKNOWN)
    if prev.get(row.td.id) == st and row.frame is not None:
        continue              # ← delta skip: zero Tk IPC
    dot.configure(fg=colour)
    text_lbl.configure(text=text, fg=colour)
    btn.configure(text=btn_text, bg=btn_bg, fg=btn_fg)
```

**Special cases:**
- `row.frame is None` — row not yet built; skip configure (nothing to update).
- `_switch_theme()` calls `_prev_statuses.clear()` to force a full repaint on the
  next cycle (theme colours changed, so cached delta is stale).

### 3. Tooltip Singleton — `TooltipManager` (hover cost)

**Problem:** Each of the 1 200 rows had its own `Tooltip` object which created
and destroyed a `tk.Toplevel` on every `<Enter>` / `<Leave>` event.

**Solution:** One shared `tk.Toplevel` owned by `TooltipManager`.  All rows share
the same panel; `show()` repositions and relabels it, `hide()` withdraws (not destroys) it.

```
hover enters row 1        → TooltipManager.show("...", x, y)  ← deiconify
hover leaves row 1        → TooltipManager.hide()             ← withdraw
hover enters row 2        → TooltipManager.show("...", x, y)  ← deiconify
```

**Initialisation:** `TooltipManager.init(root)` must be called once after the
main window is created (`gui.py __init__` after `_build_ui()`).  The underlying
`Toplevel` is itself created lazily on the first `show()` call.

### 4. Theme Switch Optimisation

`_switch_theme()` skips `row.apply_theme()` for rows where `row.frame is None`
(not yet built).  `ttk.Style` propagates colour changes automatically to ttk
widgets, so only the custom `tk.Label` / `tk.Button` elements inside built rows
need a manual configure pass.

### Binding Architecture (deferred)

```
_finish_loading()
  ├── var.trace_add() for ALL rows    (BooleanVar works before widgets exist)
  └── _wire_section_bindings(s)       (only for already-expanded sections)

section.toggle()  [first expand]
  └── _build_row_widgets()
        └── fires set_on_rows_built callback
              └── gui._wire_section_bindings(section)
                    ├── Shift-click / click bindings on each cb (Checkbutton)
                    ├── Right-click binding on each frame
                    └── Applies _cached_statuses → _prev_statuses (delta-aware)
```

---

## Registry Session API

```python
from regilattice.registry import SESSION

SESSION.set_dword(path, name, value)        # REG_DWORD
SESSION.set_string(path, name, value)       # REG_SZ
SESSION.set_expand_string(path, name, val)  # REG_EXPAND_SZ
SESSION.set_multi_sz(path, name, values)    # REG_MULTI_SZ (list[str])
SESSION.set_binary(path, name, data)        # REG_BINARY
SESSION.set_qword(path, name, value)        # REG_QWORD (64-bit int)
SESSION.set_value(path, name, val, type)    # Any REG_* type
SESSION.read_dword(path, name)              # → int | None
SESSION.read_string(path, name)             # → str | None
SESSION.read_expand_string(path, name)      # → str | None
SESSION.read_multi_sz(path, name)           # → list[str] | None
SESSION.read_binary(path, name)             # → bytes | None
SESSION.read_qword(path, name)              # → int | None
SESSION.key_exists(path)                    # → bool
SESSION.delete_value(path, name)
SESSION.delete_tree(path)
SESSION.list_values(path)                   # → list[(name, value, type)]
SESSION.list_keys(path)                     # → list[str]
SESSION.backup(keys_list, label)
SESSION.log(message)
```

Registry paths use full hive names (`HKEY_LOCAL_MACHINE\...` or
`HKEY_CURRENT_USER\...`). Standard abbreviations `HKLM\...` / `HKCU\...` also work.

---

## Code Style

| Rule | Value |
|---|---|
| Line length | 150 characters |
| Formatter | `ruff format` (replaces black) |
| Linter | `ruff check` |
| Type-check | `mypy --strict` + Pyright standard mode |
| Imports | sorted by ruff isort rules |
| Docstrings | Google-style |
| `require_admin` | mandatory keyword-only arg on all triplet functions (even if unused) |

---

## Testing

### Running Tests

```powershell
# Full suite
python -m pytest tests/ -v --tb=short

# Specific test file
python -m pytest tests/test_tweaks_smoke.py -x --tb=short

# With coverage
python -m pytest tests/ --cov=regilattice --cov-report=term-missing

# Property-based tests
python -m pytest tests/test_property.py -v

# Benchmarks
python -m pytest tests/test_benchmarks.py -v
```

### Key Fixtures (`tests/conftest.py`)

| Fixture | Scope | Description |
|---|---|---|
| `dry_session` | function | `RegistrySession(dry_run=True)` — no real registry writes |
| `all_tweaks_list` | session | Pre-loaded list of all `TweakDef` instances |
| `tmp_analytics` | function | Isolated analytics store in `tmp_path` |
| `tmp_ratings` | function | Isolated ratings store in `tmp_path` |

### Coverage Target

The project targets **≥ 95 % coverage** on all critical paths. Run:

```powershell
python -m pytest tests/ --cov=regilattice --cov-report=html
# Open htmlcov/index.html in your browser
```

---

## WSL / Linux Notes

- Registry operations (`winreg`, `ctypes.windll`) are conditionally imported
  and silently skipped on non-Windows platforms.
- Tests that require Windows are guarded with
  `@pytest.mark.skipif(not is_windows(), reason="winreg unavailable")`.
- Core logic (TweakDef, profiles, search, analytics, ratings, locale) is
  fully testable on Linux/WSL.

---

## Pre-commit Hooks

```powershell
# Install hooks (once)
pip install pre-commit
pre-commit install

# Run all hooks on all files
pre-commit run --all-files
```

Hooks configured in `.pre-commit-config.yaml`:
- `ruff-check` — lint
- `ruff-format` — format
- `mypy` — type check

---

## Commit Message Convention

Use concise imperative-mood subject lines:

```
Add boot-disable-fast-startup tweak
Fix detect_fn for explorer-show-extensions
Update Privacy category descriptions
Bump version to 1.0.2
```

No ticket/issue references are required for routine changes.

---

## Release Checklist

1. Bump version in `regilattice/__init__.py`, `pyproject.toml`, `winget/*.yaml`
2. Update `CHANGELOG.md` (Keep-a-Changelog format)
3. Update `ROADMAP.md` velocity table
4. Run full test suite: `python -m pytest tests/ --tb=short`
5. Run linter: `python -m ruff check regilattice/ tests/`
6. Run type-check: `python -m mypy regilattice/`
7. Build package: `python -m hatch build`
8. Validate package: `python -m twine check dist/*`
9. Tag release: `git tag v1.0.x`
10. Push: `git push origin main --tags`
