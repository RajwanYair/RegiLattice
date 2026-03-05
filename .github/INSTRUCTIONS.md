# RegiLattice -- Instructions

> The canonical project reference is [copilot-instructions.md](copilot-instructions.md)
> (auto-loaded by GitHub Copilot). This file covers environment setup,
> tool configuration, known pitfalls, and the pre-commit checklist.

---

## Environment Setup

### Python Executable

Use the **non-WindowsApps** Python. The Windows Store alias often fails
silently or picks the wrong version.

```powershell
# Correct path (adjust version as needed):
C:\Users\<user>\AppData\Local\Python\bin\python.exe

# Or use the py launcher:
py -3.10

# NEVER use the WindowsApps alias:
#   C:\Users\<user>\AppData\Local\Microsoft\WindowsApps\python.exe
```

The `.vscode/settings.json` sets `python.defaultInterpreterPath` to the
correct location. If `python` commands fail in the terminal with
"Python was not found; run without arguments to install from the Microsoft Store",
the Windows Store alias is intercepting — use the full path or `py` launcher.

### Install (editable + dev deps)

```powershell
& "C:\Users\<user>\AppData\Local\Python\bin\python.exe" -m pip install -e ".[dev]"
```

This installs: `pytest`, `pytest-cov`, `mypy`, `ruff`.

### Dev Dependencies

| Package | Purpose | Config |
|---------|---------|--------|
| `ruff` | Linting + formatting | `pyproject.toml [tool.ruff]` |
| `mypy` | Strict type checking | `pyproject.toml [tool.mypy]` |
| `pytest` | Test runner | `pyproject.toml [tool.pytest.ini_options]` |
| `pytest-cov` | Coverage reporting | `pyproject.toml [tool.coverage]` |

---

## Type Checking: mypy vs Pylance (Pyright)

This project validates with **both** mypy (CLI) and Pylance (VS Code).
They use different type stubs and have different strictness, causing occasional
conflicts.

### Known Divergences

| Issue | mypy | Pylance | Resolution |
|-------|------|---------|------------|
| `tk.Event` type parameters | Accepts bare `tk.Event` | Requires `tk.Event[tk.Misc]` | Use `tk.Event[tk.Misc]` everywhere |
| `concurrent.futures` exports | Finds `ThreadPoolExecutor` | May not find it (stub gap for 3.14) | Set `pythonVersion = "3.10"` in Pyright config |
| `iconbitmap()` typed | Untyped call error `[no-untyped-call]` | No error (different stubs) | Keep `# type: ignore[no-untyped-call]` + suppress `reportUnnecessaryTypeIgnoreComment` |
| `from __future__ import annotations` | Defers all annotations | Same, but ruff UP037 removes unnecessary quotes | Don't string-quote annotations when `__future__` import is present |

### Configuration

- **mypy**: `pyproject.toml [tool.mypy]` — `strict = true`, `python_version = "3.10"`
- **Pyright**: `pyproject.toml [tool.pyright]` — `pythonVersion = "3.10"`, `typeCheckingMode = "standard"`
- **VS Code**: `.vscode/settings.json` — `python.analysis.pythonVersion = "3.10"`, diagnostic severity overrides

### After changing settings

Reload the VS Code window (`Ctrl+Shift+P` → "Developer: Reload Window")
to force Pylance to re-analyze with the new configuration.

---

## Linting: ruff

### Rules enabled

`E, F, W, I, UP, B, SIM, RUF` — line length 150.

### Suppressed rules

| Rule | Reason |
|------|--------|
| `ARG002` | `require_admin` kwarg is part of the TweakDef API contract; must be present even if unused |

### Common ruff pitfalls

- **UP037**: Don't string-quote type annotations when `from __future__ import annotations` is present. ruff will flag and autofix.
- **I001**: Import blocks must be sorted (stdlib → third-party → local). Use `ruff check --fix` to auto-sort.
- **RUF001/RUF003**: Unicode confusables — use plain ASCII hyphens `-` and quotes `"`.

### Auto-fix

```powershell
python -m ruff check --fix regilattice/ tests/
python -m ruff format regilattice/ tests/
```

---

## Quick Reference

For architecture, API, stats, and tweak counts see
[copilot-instructions.md](copilot-instructions.md) for current stats.

### Git Conventions

[Conventional Commits](https://www.conventionalcommits.org/) format:

| Prefix | Use |
|---|---|
| `feat:` | New feature or tweak |
| `fix:` | Bug fix |
| `docs:` | Documentation only |
| `test:` | Adding or updating tests |
| `refactor:` | Code change that is not a fix or feature |
| `chore:` | Maintenance (deps, CI config) |
| `ci:` | CI/CD pipeline changes |
| `assets:` | Non-code assets |

One logical change per commit.

### Branch Strategy

| Branch | Purpose |
|---|---|
| `main` | Stable, always passes CI |
| `feat/*` | New features / new tweak modules |
| `fix/*` | Bug fixes |

### Pre-commit Checklist

1. `python -m ruff check regilattice/ tests/`
2. `python -m mypy regilattice/`
3. `python -m pytest tests/ -x --tb=short`
4. Verify no duplicate tweak IDs: `python -m regilattice --list`
5. Confirm line length <= 150 and ASCII-only strings
6. Check VS Code Problems panel is clean (reload window if needed)

---

See also: [ARCHITECTURE.md](ARCHITECTURE.md), [CONTRIBUTING.md](CONTRIBUTING.md), [SKILLS.md](SKILLS.md)
