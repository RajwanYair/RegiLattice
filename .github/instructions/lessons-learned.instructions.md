---
applyTo: "**/*.py,**/tests/**"
---

# Lessons Learned — RegiLattice Development

> Accumulated hard-won insights from iterative test coverage sprints and CI hardening.
> These rules are **as important as the coding standards** — they prevent recurring mistakes.
> Last updated: 2026-03-09 (v1.0.1)

---

## Tkinter Test Pitfalls

### ❌ Never use `event_generate` + `root.update()` in tests

```python
# ❌ BAD — causes pytest-timeout fatal thread interrupt (Windows error 0x80000003)
row.frame.event_generate("<Enter>")
root.update()

# ✅ GOOD — call the handler method directly
row._on_enter(None)   # signature: _on_enter(self, _: tk.Event[tk.Misc]) -> None
row._on_leave(None)   # parameter is never dereferenced in practice
```

**Why**: On Windows, `event_generate` during pytest creates a background thread to pump
messages. When the next `Tk()` root is constructed in the same process, the old pump
thread raises a fatal exception (code `0x80000003`), which pytest-timeout misinterprets
as a timeout, killing the entire test session.

### ✅ Always withdraw() and patch deferred loaders

```python
import tkinter as tk
from unittest.mock import patch

root = tk.Tk()
root.withdraw()   # ← critical — prevents actual window appearing in CI
with patch("regilattice.gui.RegiLatticeGUI._deferred_init"):
    gui = RegiLatticeGUI(root)
root.destroy()
```

### ✅ Use `@pytest.mark.timeout(10)` + `root.after` for GUI tests

```python
@pytest.mark.timeout(10)
def test_gui_closes_cleanly(root: tk.Tk) -> None:
    root.after(200, root.destroy)
    root.mainloop()
```

---

## Cache Isolation — Unique TweakDef IDs Per Test

`tweak_scope()` in `tweaks/__init__.py` uses `functools.lru_cache` keyed on `td.id`.
**Reusing the same `id` across tests causes stale cache hits and wrong assertions.**

```python
# ❌ BAD — two tests share id; second gets cached result from first
TweakDef(id="test-tweak", registry_keys=["HKEY_CURRENT_USER\\..."], ...)
TweakDef(id="test-tweak", registry_keys=["HKEY_LOCAL_MACHINE\\..."], ...)

# ✅ GOOD — unique id per test (or per test class)
TweakDef(id="scope-user-test-1", registry_keys=["HKEY_CURRENT_USER\\..."], ...)
TweakDef(id="scope-machine-test-1", registry_keys=["HKEY_LOCAL_MACHINE\\..."], ...)
```

**Rule**: Every `TweakDef` created in tests must have a **globally unique `id`** within the
test session. Use descriptive suffixes: `"<category>-<scenario>-test"`.

---

## Coverage: Use Real Enum Values, Not MagicMock

When testing branches that depend on enum comparisons, **always pass the real enum value**.

```python
# ❌ BAD — MagicMock() never equals TweakResult.UNKNOWN, so the [???] branch is never hit
patch("regilattice.menu.tweak_status", return_value=MagicMock())

# ✅ GOOD — real enum triggers the exact branch
from regilattice.tweaks import TweakResult
patch("regilattice.menu.tweak_status", return_value=TweakResult.UNKNOWN)
```

---

## Mock Preference — pytest-mock Over unittest.mock

```python
# ✅ Preferred — mocker fixture auto-resets, integrates with pytest output
def test_thing(mocker: pytest.MockerFixture) -> None:
    mocker.patch("regilattice.menu.tweak_status", return_value=TweakResult.APPLIED)

# Also OK — contextmanager form
with patch("regilattice.menu.tweak_status", return_value=TweakResult.APPLIED):
    ...
```

---

## Subprocess — Always Explicit Args, Never shell=True with Input

All three fixed vulnerabilities in recent security audit:

| Location | Before (❌) | After (✅) |
|---|---|---|
| `tweaks/maintenance.py` | `subprocess.run(cmd, shell=True)` | `subprocess.run(["cmd.exe", "/c", "wmic", ...])` |
| `gui_dialogs.py` (PS export) | `subprocess.run(f"powershell ... {path}", shell=True)` | `subprocess.run(["powershell.exe", "-File", str(path)])` |
| `gui_dialogs.py` (Scoop) | `subprocess.run(f"scoop {action} {pkg}", shell=True)` | `subprocess.run(["powershell.exe", "-Command", "scoop", action, pkg])` |

---

## Multi-File Edits — Use `multi_replace_string_in_file`

When making independent edits across multiple files (or multiple non-overlapping
edits in one file), prefer `multi_replace_string_in_file` over sequential calls.
It is faster, atomic per-replacement, and produces a single summary.

---

## Test File Lint Pitfalls (ruff)

| Code | Problem | Fix |
|---|---|---|
| `F841` | `with patch(...) as mock_list:` where `mock_list` is unused | Remove `as mock_list` |
| `E501` | Inline list comprehension longer than 150 chars | Extract to `_helper()` function |
| `RUF059` | `section, rows = fn()` where `rows` unused | Change to `section, _ = fn()` |
| `I001` | Unsorted imports | Run `ruff --fix` |
| `RUF001` | En-dash or smart quote in string | Replace with plain ASCII `--` or `"` |

---

## CI Best Practices Applied

- **pip cache**: key on `pyproject.toml` hash to invalidate on dependency changes
- **ruff format --check**: run after lint step (separate check, not auto-fix in CI)
- **codecov-action@v5**: use latest major; set `fail_ci_if_error: false`
- **pytest -x**: `--failfast` in CI to avoid drowning in error output on first failure
- **pre-commit hooks**: run `pytest smoke` as local pre-commit gate

---

## Version & Metadata

- `version` lives in `pyproject.toml` only — read at runtime via `importlib.metadata`
- Do **not** duplicate version strings in `__init__.py`; keep `__version__ = importlib.metadata.version("regilattice")`
- `authors` field: `[{ name = "YairRajwan" }]` — no email required in pyproject
- GitHub URLs: `https://github.com/RajwanYair/RegiLattice` — no `aeger` references

---

## GitHub Username is RajwanYair

All references to the GitHub account must use `RajwanYair` (not `aeger`):
- `CODEOWNERS`: `@RajwanYair` ✅ (verified 2026-03-09)
- `pyproject.toml` URLs: `https://github.com/RajwanYair/RegiLattice` ✅
- Historical `aeger → RajwanYair` migration notes in `docs/Roadmap.md` are ✅ completed items
