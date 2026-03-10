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

| Location                     | Before (❌)                                            | After (✅)                                                             |
| ---------------------------- | ------------------------------------------------------ | ---------------------------------------------------------------------- |
| `tweaks/maintenance.py`      | `subprocess.run(cmd, shell=True)`                      | `subprocess.run(["cmd.exe", "/c", "wmic", ...])`                       |
| `gui_dialogs.py` (PS export) | `subprocess.run(f"powershell ... {path}", shell=True)` | `subprocess.run(["powershell.exe", "-File", str(path)])`               |
| `gui_dialogs.py` (Scoop)     | `subprocess.run(f"scoop {action} {pkg}", shell=True)`  | `subprocess.run(["powershell.exe", "-Command", "scoop", action, pkg])` |

---

## Multi-File Edits — Use `multi_replace_string_in_file`

When making independent edits across multiple files (or multiple non-overlapping
edits in one file), prefer `multi_replace_string_in_file` over sequential calls.
It is faster, atomic per-replacement, and produces a single summary.

---

## Test File Lint Pitfalls (ruff)

| Code     | Problem                                                     | Fix                                  |
| -------- | ----------------------------------------------------------- | ------------------------------------ |
| `F841`   | `with patch(...) as mock_list:` where `mock_list` is unused | Remove `as mock_list`                |
| `E501`   | Inline list comprehension longer than 150 chars             | Extract to `_helper()` function      |
| `RUF059` | `section, rows = fn()` where `rows` unused                  | Change to `section, _ = fn()`        |
| `I001`   | Unsorted imports                                            | Run `ruff --fix`                     |
| `RUF001` | En-dash or smart quote in string                            | Replace with plain ASCII `--` or `"` |

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
- Historical `aeger → RajwanYair` migration notes in `docs/Roadmap.md` are ✅ completed and cleaned (2026-03-09)

---

## hwinfo.py — Mockable ctypes Helper Pattern

Extract platform-specific `ctypes` calls into a named module-level function so tests can patch them without affecting the real implementation:

```python
# ✅ GOOD — named helper, fully patchable
def _ctypes_memory_mb() -> tuple[int, int] | None:
    with contextlib.suppress(Exception):
        import ctypes
        ...
    return None

# In test:
@patch("regilattice.hwinfo._ctypes_memory_mb", return_value=None)
def test_memory_cim_fallback(self, _mock_ctypes, _mock_cim): ...

# ❌ BAD — inline ctypes inside a cached function; test cannot mock it
@functools.lru_cache(maxsize=1)
def detect_memory():
    with contextlib.suppress(Exception):
        import ctypes
        ...  # unpatchable
```

---

## Tkinter Collapse/Expand — O(1) Container Hide Pattern

`_hide_rows()` must hide only the container frame, not unpack each individual row:

```python
# ✅ O(1) — hide container; child rows stay packed inside
def _hide_rows(self) -> None:
    self.content_frame.pack_forget()

# ❌ O(n_rows) — expensive per-row unpack on every collapse
def _hide_rows(self) -> None:
    for row in self.rows:
        row.unpack_row()
    self.content_frame.pack_forget()
```

This matters with 1 200+ rows — collapse/expand was noticeably slow before the fix.

---

## Status Messages — Skip 100% Progress, Set Ready in Completion Handler

The async status-update pattern: block the 100% "Detecting… 100%" flash (it would flicker before `_apply_statuses` resets it) and let the completion handler write the final Ready message:

```python
# In _on_progress:
if pct < 100:  # skip 100 % — _apply_statuses sets Ready status
    self._root.after(0, lambda p=pct: self._set_status(f"Detecting… {p} %", _WARN_YELLOW))

# At the end of _apply_statuses:
self._set_status(f"Ready  •  {total_tweaks} tweaks  •  {applied} applied / {default} default / {unknown} unknown")
```

Without the `< 100` guard the status bar would show "Detecting… 100%" momentarily before snapping to "Ready", which looks like a stuck counter.

---

## PowerShell Composite Scripts — Always Add `$ErrorActionPreference`

Every `_COMPOSITE_PS` style multi-cmdlet script must suppress non-fatal errors and protect unreliable cmdlets:

```powershell
# ✅ Add at the top of every composite PS script
$ErrorActionPreference = 'Continue'

# Wrap cmdlets that may be absent on some SKUs
try { $pd = Get-PhysicalDisk -ErrorAction Stop | ... } catch { $pd = @() }

# Protect potentially-null properties with explicit string cast
"$([string]($prop.NullableValue))"
```

---

## Tkinter Long Operations — Always Call `update_idletasks()` After Batch Loops

After iterating over all category sections (expand/collapse), call `update_idletasks()` once to flush pending geometry recalculations. Without it the window freezes until the event loop resumes:

```python
def _expand_all(self) -> None:
    for section in self._category_sections:
        if not section.expanded:
            section.toggle()
    self._root.update_idletasks()  # flush all pending geometry work at once
```

---

## Terminal Commands — ALWAYS PowerShell (Never Unix)

This workspace is on **Windows**. GitHub Copilot must NEVER emit Unix shell commands.
Use PowerShell equivalents for every terminal operation:

| ❌ NEVER use (Unix)   | ✅ Use instead (PowerShell)                     |
| --------------------- | ----------------------------------------------- |
| `tail -n N file`      | `Get-Content file \| Select-Object -Last N`     |
| `grep pattern file`   | `Select-String -Pattern 'pattern' file`         |
| `ls -la`              | `Get-ChildItem`                                 |
| `cat file`            | `Get-Content file`                              |
| `rm -rf dir`          | `Remove-Item -Recurse -Force dir`               |
| `touch file`          | `New-Item file -ItemType File`                  |
| `wc -l file`          | `(Get-Content file).Count`                      |
| `which cmd`           | `Get-Command cmd`                               |
| `export VAR=val`      | `$env:VAR = 'val'`                              |
| `&&` chaining         | `;` or `if ($LASTEXITCODE -eq 0) { ... }`       |

**Enforcement**: The full ban list is in `.github/copilot-instructions.md` under
"MANDATORY: Terminal Commands — PowerShell ONLY". That section is auto-loaded
every session — if a Unix command still appears, check that the file is saved
and the workspace has been reloaded.
