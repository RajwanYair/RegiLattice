# Contributing to RegiLattice

Thank you for considering a contribution! This document covers the tooling,
conventions, and workflow you need to get started.

## Prerequisites

- **Python 3.10+** (tested through 3.14)
- **Windows** (registry operations require `winreg`)
- Recommended editor: **VS Code** with the extensions listed in
  `.vscode/extensions.json`.

## Setup

```bash
# Clone and install in editable mode with dev dependencies
git clone <repo-url> && cd RegiLattice
pip install -e ".[dev]"
```

## Development Workflow

| Task              | Command                                          |
| ----------------- | ------------------------------------------------ |
| Lint              | `python -m ruff check regilattice/ tests/`       |
| Format            | `python -m ruff format regilattice/ tests/`      |
| Type-check (mypy) | `python -m mypy regilattice/`                    |
| Test              | `python -m pytest tests/ -v --tb=short`          |
| Test + coverage   | `python -m pytest tests/ --cov=regilattice`      |
| Run GUI           | `python -m regilattice --gui`                    |
| List tweaks       | `python -m regilattice --list`                   |

VS Code tasks (`Ctrl+Shift+B`) mirror these commands — see `.vscode/tasks.json`.

## Adding a New Tweak

1. Create or edit `regilattice/tweaks/<category>.py`.
2. Define registry key constants at module top: `_KEY = r"HKEY_..."`.
3. Implement the function triplet:

   ```python
   def _apply_my_tweak(*, require_admin: bool = True) -> None: ...
   def _remove_my_tweak(*, require_admin: bool = True) -> None: ...
   def _detect_my_tweak() -> bool: ...
   ```

4. Append a `TweakDef(...)` to the module-level `TWEAKS` list.
5. Ensure the `id` is **globally unique**, kebab-case, prefixed with the
   category slug (see `.github/copilot-instructions.md` for the full slug table).
6. Run the smoke tests: `python -m pytest tests/test_tweaks_smoke.py -x --tb=short`.
7. Run the linter: `python -m ruff check regilattice/ tests/`.

**No imports or registration needed** — the plugin loader auto-discovers every
`.py` in `tweaks/` that isn't `_`-prefixed.

## Code Style

- **Line length**: 150 characters (configured in `pyproject.toml`).
- **Formatter**: ruff format (runs on save in VS Code).
- **Linter**: ruff (sole linter — replaces flake8, isort, pyflakes, pycodestyle, and pylint).
- **Type-check**: mypy `--strict` and Pyright standard mode.
- **`require_admin`**: mandatory keyword-only parameter on all triplet functions,
  even when unused (ruff ARG002 is suppressed for this pattern).

## Commit Messages

Use concise, imperative-mood subject lines. Examples:

- `Add bluetooth-disable-handsfree tweak`
- `Fix detect_fn for explorer-show-extensions`
- `Update Privacy category descriptions`

## Testing

- All tweaks are auto-parametrized in `test_tweaks_smoke.py` — adding a tweak
  automatically generates tests for its triplet signatures, ID uniqueness, and
  `detect_fn` callability.
- Use `dry_session` fixture (from `conftest.py`) for tests that touch
  `RegistrySession` — it sets `_dry_run=True` so no real registry writes occur.

## Corporate Safety

Tweaks with `corp_safe=False` are blocked on corporate networks (detected via
AD, AAD, VPN, GPO, SCCM indicators). If your tweak only touches `HKCU` and
poses no enterprise risk, set `corp_safe=True`.

Thank you for your interest in contributing! This guide covers everything from
environment setup to submitting a PR.

---

## Quick Start (5 minutes)

1. **Fork & clone** the repository.
2. **Install** in editable mode:

   ```bash
   pip install -e ".[dev]"
   ```

3. **Copy the template** to create a new tweak module:

   ```bash
   cp regilattice/tweaks/_template.py regilattice/tweaks/myapp.py
   ```

4. **Edit** `myapp.py` -- define registry key paths, implement the
   `_apply_*` / `_remove_*` / `_detect_*` triplet, and register a `TweakDef`
   in the `TWEAKS` list.
5. **Validate** your work:

   ```bash
   python -m ruff check regilattice/ tests/
   python -m mypy regilattice/
   python -m pytest tests/test_tweaks_smoke.py -x --tb=short
   ```

6. **Commit** with a conventional message and open a PR.

---

## Environment Setup

### Python Executable

Use the non-WindowsApps Python. The Windows Store alias often fails silently.

```powershell
# Correct (adjust version):
%LOCALAPPDATA%\Python\bin\python.exe

# Or use py launcher:
py -3.10

# NEVER: C:\Users\<user>\AppData\Local\Microsoft\WindowsApps\python.exe
```

### Dev Dependencies

| Package | Purpose | Config |
|---------|---------|--------|
| `ruff` | Linting + formatting | `pyproject.toml [tool.ruff]` |
| `mypy` | Strict type checking | `pyproject.toml [tool.mypy]` |
| `pytest` | Test runner | `pyproject.toml [tool.pytest.ini_options]` |
| `pytest-cov` | Coverage reporting | `pyproject.toml [tool.coverage]` |

---

## Project Structure

```
regilattice/
├── tweaks/            # Plugin modules -- each exports TWEAKS: list[TweakDef]
│   ├── _template.py   # Contributor guide and example code
│   ├── performance.py # Example: 18 performance tweaks
│   └── ...            # 64 category modules, auto-discovered
├── cli.py             # argparse CLI
├── menu.py            # Interactive console menu
├── gui.py             # tkinter GUI (Catppuccin Mocha, ~1 432 lines)
├── gui_theme.py       # Theme constants (colours, fonts)
├── gui_tooltip.py     # Tooltip + description metadata parser
├── gui_widgets.py     # TweakRow + CategorySection
├── gui_dialogs.py     # Import/export/about dialogs
├── config.py          # ~/.regilattice.toml support
├── registry.py        # RegistrySession: winreg wrapper + backup + logging
├── corpguard.py       # Corporate network detection
├── elevation.py       # UAC elevation helpers
└── deps.py            # Lazy-import + auto-install
```

---

## Adding a New Tweak (Step-by-Step)

### 1. Choose or create the module file

Each category has its own file in `regilattice/tweaks/`. If your tweak fits an
existing category, add to that file. Otherwise create a new `.py` file -- the
loader discovers it automatically.

### 2. Define key-path constants at module top

```python
_MYAPP_KEY = r"HKEY_CURRENT_USER\Software\MyApp"
_MYAPP_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\MyApp"
```

Use raw strings (`r"..."`) for all registry paths.

### 3. Implement the apply / remove / detect triplet

```python
def _apply_no_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_MYAPP_POLICY], "MyAppTelemetry")
    SESSION.set_dword(_MYAPP_POLICY, "DisableTelemetry", 1)
    SESSION.log("MyApp: disabled telemetry")

def _remove_no_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MYAPP_POLICY, "DisableTelemetry")

def _detect_no_telemetry() -> bool:
    return SESSION.read_dword(_MYAPP_POLICY, "DisableTelemetry") == 1
```

**Rules:**
- `require_admin` must be keyword-only (`*,`) even if unused (ARG002 suppressed)
- Call `assert_admin()` first, then `SESSION.backup()`, then the mutation
- `_detect` returns `True` when the tweak is currently active
- `_remove` should restore the default state (delete value or set original default)

### 4. Export the `TWEAKS` list

```python
TWEAKS: list[TweakDef] = [
    TweakDef(
        id="myapp-no-telemetry",           # globally unique kebab-case
        label="Disable MyApp Telemetry",
        category="MyApp",
        apply_fn=_apply_no_telemetry,
        remove_fn=_remove_no_telemetry,
        detect_fn=_detect_no_telemetry,
        needs_admin=True,                   # False for HKCU-only tweaks
        corp_safe=False,                    # True if HKCU-only and no GPO risk
        registry_keys=[_MYAPP_POLICY],
        description="Prevents MyApp from sending usage data.",
        tags=["myapp", "telemetry", "privacy"],
    ),
]
```

### 5. Verify

```bash
python -m regilattice --list | findstr myapp
python -m pytest tests/test_tweaks_smoke.py -x --tb=short
python -m ruff check regilattice/ tests/
```

**No edits to `tweaks/__init__.py` needed** -- the loader auto-discovers it.

---

## Common Tweak Patterns

### Pattern A: Toggle a single DWORD

```python
def _apply_x(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY], "label")
    SESSION.set_dword(_KEY, "ValueName", 1)

def _remove_x(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY, "ValueName")

def _detect_x() -> bool:
    return SESSION.read_dword(_KEY, "ValueName") == 1
```

### Pattern B: Tweak = absence of a value

```python
def _apply_x(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY], "label")
    SESSION.delete_value(_KEY, "ValueName")

def _remove_x(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_KEY, "ValueName", default_value)

def _detect_x() -> bool:
    return SESSION.read_dword(_KEY, "ValueName") is None
```

### Pattern C: Disable a Windows service

```python
_SVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ServiceName"

def _apply_disable_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SVC], "disable-svc")
    SESSION.set_dword(_SVC, "Start", 4)   # 4 = Disabled

def _remove_disable_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SVC, "Start", 3)   # 3 = Manual

def _detect_disable_svc() -> bool:
    return SESSION.read_dword(_SVC, "Start") == 4
```

Service Start values: 0=Boot, 1=System, 2=Automatic, 3=Manual, 4=Disabled.

### Pattern D: Description metadata

Include `Default:` and optionally `Recommended:` as standalone sentences.
The GUI extracts these for tooltip hints and shows a teal "REC" badge.

```python
description="Disables X. Default: Enabled. Recommended: Disabled for privacy."
```

---

## Coding Standards

| Tool | Rules | Command |
|------|-------|---------|
| **ruff** | E, F, W, I, UP, B, SIM, RUF; line-length 150 | `ruff check regilattice/ tests/` |
| **mypy** | `--strict`, `python_version = "3.10"` | `mypy regilattice/` |
| **pytest** | ~13 700 tests | `pytest tests/ -x --tb=short` |

- `require_admin` kwarg must be present on apply/remove functions (ARG002 suppressed).
- Use `SESSION.backup()` before any registry mutation.
- Keep IDs globally unique, kebab-case: `{category_slug}-{descriptive-name}`.
- Line length: 150 chars (configured in pyproject.toml).
- Use plain ASCII hyphens `-` and quotes `"` (ruff flags Unicode confusables).

---

## Git Conventions

### Branching

| Branch | Purpose |
|--------|---------|
| `main` | Stable, always passes CI |
| `feat/*` | New features or tweak modules |
| `fix/*` | Bug fixes |

### Commit Messages

[Conventional Commits](https://www.conventionalcommits.org/) format:

| Prefix | Use |
|--------|-----|
| `feat:` | New feature or tweak |
| `fix:` | Bug fix |
| `docs:` | Documentation only |
| `test:` | Adding or updating tests |
| `refactor:` | Code change that is not a fix or feature |
| `chore:` | Maintenance (deps, CI config) |
| `ci:` | CI/CD pipeline changes |

---

## Pre-Commit Checklist

1. `python -m ruff check regilattice/ tests/`
2. `python -m mypy regilattice/`
3. `python -m pytest tests/ -x --tb=short`
4. Verify no duplicate tweak IDs: `python -m regilattice --list`
5. Line length <= 150 and ASCII-only strings

---

## Running Tests

```bash
# Full suite (~17 266 tests)
python -m pytest tests/ -v --tb=short

# Smoke tests only (fastest, covers all tweaks)
python -m pytest tests/test_tweaks_smoke.py -x --tb=short

# With coverage
python -m pytest tests/ --cov=regilattice --cov-report=term-missing

# Lint + type check
python -m ruff check regilattice/ tests/
python -m mypy regilattice/
```

Tests use `_dry_run=True` so no real registry writes happen. `detect_fn` tests
call the function but don't assert specific values (host-dependent).

---

## Key Files to Know

| File | Purpose |
|------|---------|
| `regilattice/tweaks/__init__.py` | Plugin loader, TweakDef, TweakExecutor, profiles, batch ops |
| `regilattice/tweaks/_template.py` | Detailed contributor guide with working examples |
| `regilattice/registry.py` | Registry read/write/backup/logging (SESSION) |
| `regilattice/corpguard.py` | Corporate network detection |
| `regilattice/config.py` | TOML config file support |
| `tests/conftest.py` | Test fixtures (dry_session, all_tweaks_list) |
| `tests/test_tweaks_smoke.py` | Auto-parametrized smoke tests for all tweaks |

---

## Need Help?

- Read `regilattice/tweaks/_template.py` for detailed patterns and troubleshooting.
- Look at existing tweak modules (e.g., `performance.py`, `privacy.py`) for real examples.
- See [.github/ARCHITECTURE.md](.github/ARCHITECTURE.md) for data flow and design decisions.
