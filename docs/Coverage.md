# RegiLattice — Coverage Report

> Per-module test coverage baseline. Last measured: 2026-03-09 · v1.0.0
> Command: `python -m pytest tests/ --cov=regilattice --cov-report=term-missing`

---

## Summary

| Scope | Coverage |
|---|---|
| **Core modules (non-tweak)** | ~88 % average |
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
| `cli.py` | 641 | 90 | **86 %** | `--gui`, admin-only, export edge cases |
| `config.py` | 50 | 5 | **90 %** | tomllib fallback path (Python < 3.11) |
| `corpguard.py` | 252 | 58 | **77 %** | VPN/SCCM detection paths (Windows-only) |
| `deps.py` | 74 | 0 | **100 %** | ✅ |
| `elevation.py` | 44 | 2 | **95 %** | Non-Windows uid path |
| `gui.py` | 1193 | 511 | **57 %** | 🔴 Priority — GUI test harness needed |
| `gui_dialogs.py` | 279 | 181 | **35 %** | 🔴 Dialog show/interact paths |
| `gui_theme.py` | 98 | 2 | **98 %** | ✅ |
| `gui_tooltip.py` | 159 | 15 | **91 %** | ✅ |
| `gui_widgets.py` | (see test_gui_widgets.py) | — | — | Covered via TweakRow/CategorySection tests |
| `hwinfo.py` | 280 | 9 | **97 %** | ✅ |
| `locale.py` | 28 | 0 | **100 %** | ✅ |
| `marketplace.py` | 82 | 4 | **95 %** | ✅ |
| `menu.py` | 167 | 11 | **93 %** | ✅ |
| `profiler.py` | — | — | — | Covered via test_benchmarks.py |
| `ratings.py` | 60 | 2 | **97 %** | ✅ |
| `registry.py` | 361 | 56 | **84 %** | Backup dir create / HKLM write edge cases |
| `tweaks/__init__.py` | 507 | 256 | **50 %** | Engine covered; platform-only paths skipped in CI |

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
| `gui.py` | 57 % | ≥ 80 % | 🔴 P1 |
| `gui_dialogs.py` | 35 % | ≥ 80 % | 🔴 P1 |
| `corpguard.py` | 77 % | ≥ 90 % | 🟡 P2 |
| `registry.py` | 84 % | ≥ 95 % | 🟡 P2 |
| `tweaks/__init__.py` | 50 % | ≥ 80 % | 🟡 P2 |
| `cli.py` | 86 % | ≥ 95 % | 🟡 P2 |
| Overall (non-tweak) | ~88 % | ≥ 95 % | P1 |

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
