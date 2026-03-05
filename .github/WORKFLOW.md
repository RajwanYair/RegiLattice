# regilattice -- workflow

Development workflow, plugin creation guide, and pre-commit checklist.
Last verified: 2026-03-05 (578 tweaks, 43 categories, ~5 927 tests).

---

## Branch strategy

| Branch | Purpose |
|---|---|
| `main` | Stable, production-ready code |
| `feat/*` | New tweaks or features |
| `fix/*` | Bug fixes |

## Commit convention

[Conventional Commits](https://www.conventionalcommits.org/):

```
feat: add WSL memory-limit optimisation tweak
feat(tweaks): add 6 new network tweaks (LLMNR, WPAD, ECN, SMBv1, DNS cache)
fix: corp guard false positive on personal VPN
docs: update README with new tweak table
test: add detect_fn unit tests for privacy tweaks
refactor: extract plugin loader into tweaks/__init__
ci: add Python ruff+pytest workflow
chore: bump version to 2.1.0
assets: add logo.png
```

---

## Adding a new tweak (step-by-step)

### 1. Choose or create the module file

Each category has its own file in `regilattice/tweaks/`. If your tweak fits an
existing category, add to that file. Otherwise, create a new `.py` file:

```python
# regilattice/tweaks/myapp.py
"""MyApp registry tweaks."""
from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef
```

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
    SESSION.log("MyApp: disable telemetry")
    SESSION.backup([_MYAPP_POLICY], "MyAppTelemetry")
    SESSION.set_dword(_MYAPP_POLICY, "DisableTelemetry", 1)

def _remove_no_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MYAPP_POLICY, "DisableTelemetry")

def _detect_no_telemetry() -> bool:
    return SESSION.read_dword(_MYAPP_POLICY, "DisableTelemetry") == 1
```

**Rules:**
- `require_admin` must be keyword-only (`*,`) even if unused (ARG002 is suppressed)
- Call `assert_admin()` first, then `SESSION.backup()`, then the mutation
- `_detect` returns `True` when the tweak is currently active
- `_remove` should restore the default state (delete value or set original default)

### 4. Export `TWEAKS` list

```python
TWEAKS: list[TweakDef] = [
    TweakDef(
        id="myapp-no-telemetry",          # MUST be globally unique kebab-case
        label="Disable MyApp Telemetry",
        category="MyApp",
        apply_fn=_apply_no_telemetry,
        remove_fn=_remove_no_telemetry,
        detect_fn=_detect_no_telemetry,
        needs_admin=True,                  # False for HKCU-only tweaks
        corp_safe=False,                   # True if HKCU-only and no GPO risk
        registry_keys=[_MYAPP_POLICY],     # informational
        description="Prevents MyApp from sending usage data.",
        tags=["myapp", "telemetry", "privacy"],
    ),
]
```

### 5. Verify

```bash
# Check it loads without duplicate IDs
python -m regilattice --list | findstr myapp

# Run smoke tests (auto-discovers your new tweak)
python -m pytest tests/test_tweaks_smoke.py -v -k myapp

# Run full test suite
python -m pytest tests/ -x --tb=short

# Lint
python -m ruff check regilattice/ tests/
```

**No edits to `tweaks/__init__.py` needed** -- the loader finds it automatically.

---

## Common tweak patterns

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

### Pattern B: Delete a value (tweak = absence of value)

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

### Pattern C: Multiple registry values

```python
def _apply_x(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY1, _KEY2], "label")
    SESSION.set_dword(_KEY1, "A", 0)
    SESSION.set_dword(_KEY2, "B", 1)
    SESSION.set_string(_KEY2, "C", "disabled")
```

### Pattern D: Disable a Windows service

```python
_SVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ServiceName"

def _apply_disable_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SVC], "disable-svc")
    SESSION.set_dword(_SVC, "Start", 4)  # 4 = Disabled

def _remove_disable_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SVC, "Start", 3)  # 3 = Manual

def _detect_disable_svc() -> bool:
    return SESSION.read_dword(_SVC, "Start") == 4
```

Service Start values: 0=Boot, 1=System, 2=Automatic, 3=Manual, 4=Disabled.

---

## Coding conventions

| Rule | Example |
|---|---|
| Private functions prefixed `_` | `_apply_no_telemetry` |
| Apply / remove accept keyword-only `require_admin` | `*, require_admin: bool = True` |
| Call `assert_admin()` at the top | gates UAC elevation |
| Call `SESSION.backup()` before first write | enables rollback |
| Call `SESSION.log()` for audit trail | appends to log file |
| `needs_admin=False` for HKCU-only tweaks | no UAC prompt |
| IDs are kebab-case and globally unique | `"myapp-no-telemetry"` |
| Line length: 150 chars | configured in pyproject.toml |
| Use `from __future__ import annotations` | every .py file |
| Use plain ASCII (no en-dashes, smart quotes) | ruff RUF001/RUF003 |

## TweakDef field reference

| Field | Type | Required | Default | Purpose |
|---|---|---|---|---|
| `id` | `str` | yes | -- | Unique kebab-case slug. Used by CLI, snapshots, tests. |
| `label` | `str` | yes | -- | Human-readable title for GUI / menu. |
| `category` | `str` | yes | -- | Category heading for UI grouping. |
| `apply_fn` | `Callable` | yes | -- | Applies the tweak (registry writes). |
| `remove_fn` | `Callable` | yes | -- | Reverts the tweak (restore defaults). |
| `detect_fn` | `Callable` | no | `None` | Returns `True` when tweak is active. |
| `needs_admin` | `bool` | no | `True` | Requires elevation for HKLM writes. |
| `corp_safe` | `bool` | no | `False` | Safe to apply on corporate networks. |
| `registry_keys` | `list[str]` | no | `[]` | Paths shown in tooltips, used for backup. |
| `description` | `str` | no | `""` | Tooltip / `--list` help text. |
| `tags` | `list[str]` | no | `[]` | Extra search keywords. |

---

## Running tests

```bash
# Install dev dependencies
pip install -e ".[dev]"

# Full test suite (~5 927 tests)
python -m pytest tests/ -v --tb=short

# Smoke tests only (fast, covers all tweaks)
python -m pytest tests/test_tweaks_smoke.py -x --tb=short

# With coverage
python -m pytest tests/ -v --tb=short --cov=regilattice --cov-report=term-missing

# Lint
python -m ruff check regilattice/ tests/

# Type check
python -m mypy regilattice/
```

## Pre-commit checklist

- [ ] `ruff check regilattice/ tests/` passes (no new warnings)
- [ ] `python -m pytest tests/ -x --tb=short` passes (all tests green)
- [ ] `mypy --strict regilattice/` passes (or no regressions)
- [ ] New tweak has all 3 functions: apply, remove, detect
- [ ] New tweak ID is globally unique kebab-case
- [ ] `detect_fn` returns correct status
- [ ] `corp_safe` flag set correctly (True for HKCU-only)
- [ ] `needs_admin` flag set correctly
- [ ] `registry_keys` lists all touched paths
- [ ] Commit uses conventional format (`feat:`, `fix:`, etc.)

## Release process

1. Bump version in `pyproject.toml` (`version = "X.Y.Z"`).
2. Update category/tweak counts in README and `.github/instructions.md`.
3. Run full test suite + lint.
4. Tag: `git tag vX.Y.Z && git push --tags`.
5. Build: `python -m build` (generates wheel + sdist).
