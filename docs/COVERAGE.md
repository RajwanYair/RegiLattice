# RegiLattice — Coverage Report

> Per-module test coverage baseline. Last measured: 2026-03-08 · v1.0.1
> Command: `python -m pytest tests/ --cov=regilattice --cov-report=term-missing`

---

## Summary

| Scope | Coverage |
|---|---|
| **Core modules (non-tweak)** | ~81 % average |
| **Tweak modules (apply/remove/detect fns)** | ~49 % average (intentionally limited — registry calls are not exercised in CI) |
| **Overall total** | ~51 % |

The low overall percentage is dominated by the 69 tweak category modules, where
the `_apply_*` / `_remove_*` / `_detect_*` functions touch real registry keys
and are intentionally **not** invoked in automated tests. All tweak signatures,
IDs, and `detect_fn` callability are verified via `test_tweaks_smoke.py`.

---

## Core Module Coverage

| Module | Statements | Missed | Coverage | Notes |
|---|---|---|---|---|
| `__init__.py` | 5 | 0 | **100 %** | ✅ |
| `analytics.py` | 61 | 0 | **100 %** | ✅ |
| `config.py` | 50 | 5 | **90 %** | tomllib fallback path (Python < 3.11) |
| `corpguard.py` | 252 | 62 | **75 %** | VPN / SCCM detection paths |
| `deps.py` | 73 | 0 | **100 %** | ✅ |
| `elevation.py` | 44 | 2 | **95 %** | Non-Windows uid path |
| `gui.py` | 1096 | 955 | **13 %** | 🔴 Highest priority — needs GUI test harness |
| `gui_dialogs.py` | 171 | 76 | **56 %** | Dialog show/interact paths |
| `gui_theme.py` | 97 | 2 | **98 %** | ✅ |
| `gui_tooltip.py` | 97 | 25 | **74 %** | `Tooltip.show()` visual paths |
| `gui_widgets.py` | 241 | 138 | **43 %** | 🔴 Widget event handlers |
| `hwinfo.py` | 280 | 5 | **98 %** | ✅ |
| `locale.py` | 28 | 0 | **100 %** | ✅ |
| `marketplace.py` | 75 | 4 | **95 %** | ✅ |
| `menu.py` | 166 | 11 | **93 %** | ✅ |
| `ratings.py` | 60 | 2 | **97 %** | ✅ |
| `registry.py` | 361 | 56 | **84 %** | Backup dir / edge cases |
| `cli.py` | 572 | 77 | **87 %** | `--gui`, export edge cases |
| `tweaks/__init__.py` | ~800 | ~80 | **~90 %** | ✅ Core engine well-covered |

---

## Tweak Module Coverage (average ~49 %)

The low coverage in tweak modules is **by design**:

- `_apply_*` and `_remove_*` functions call `winreg.SetValueEx` / `winreg.DeleteValue`,
  which require a real Windows registry and admin rights.
- These are tested in integration tests on a CI runner with registry access.
- `detect_fn` callability is validated in `test_tweaks_smoke.py`.

| Module | Coverage | Notes |
|---|---|---|
| `tweaks/accessibility.py` | ~51 % | Apply/remove paths uncovered |
| `tweaks/explorer.py` | ~42 % | Largest module (504 stmts) |
| `tweaks/shell.py` | ~35 % | Complex multi-step tweaks |
| `tweaks/win11.py` | ~43 % | Large module (376 stmts) |
| `tweaks/wsl.py` | ~44 % | WSL integration paths |
| *(all others)* | 40–63 % | Pattern consistent across categories |

---

## Coverage Targets

| Target | Current | Goal | Priority |
|---|---|---|---|
| `gui.py` | 13 % | ≥ 80 % | 🔴 P1 (Sprint 4) |
| `gui_widgets.py` | 43 % | ≥ 80 % | 🔴 P1 (Sprint 4) |
| `gui_dialogs.py` | 56 % | ≥ 80 % | 🟡 P2 (Sprint 4) |
| `corpguard.py` | 75 % | ≥ 90 % | 🟡 P2 (Sprint 4) |
| `registry.py` | 84 % | ≥ 95 % | 🟡 P2 (Sprint 4) |
| `gui_tooltip.py` | 74 % | ≥ 90 % | 🟡 P2 (Sprint 4) |
| Overall (non-tweak) | ~81 % | ≥ 95 % | P1 |

---

## Running Coverage Locally

```powershell
# Full report with missing lines
python -m pytest tests/ --cov=regilattice --cov-report=term-missing

# HTML report (open htmlcov/index.html)
python -m pytest tests/ --cov=regilattice --cov-report=html

# Coverage for a specific module only
python -m pytest tests/test_registry.py --cov=regilattice.registry --cov-report=term-missing
```

---

## Improving Coverage

### GUI modules (`gui.py`, `gui_widgets.py`, `gui_dialogs.py`)

Use `unittest.mock.patch` with `tkinter` mocked:

```python
import sys
from unittest.mock import MagicMock, patch

# Mock the entire tkinter module before importing gui
sys.modules["tkinter"] = MagicMock()
sys.modules["tkinter.ttk"] = MagicMock()
sys.modules["tkinter.messagebox"] = MagicMock()

from regilattice import gui
```

### Registry modules (apply/remove/detect)

Use the `dry_session` pytest fixture from `conftest.py`:

```python
def test_apply_tweak(dry_session, monkeypatch):
    from regilattice.registry import SESSION
    monkeypatch.setattr("regilattice.tweaks.explorer.SESSION", dry_session)
    from regilattice.tweaks.explorer import _apply_show_extensions
    _apply_show_extensions(require_admin=False)
    assert dry_session._write_log  # verify a write was recorded
```
