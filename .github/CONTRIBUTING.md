# Contributing to RegiLattice

Thank you for your interest in contributing! This guide will help you get
started quickly.

---

## Quick Start

```bash
# Clone and install in editable mode with dev dependencies
git clone <repo-url>
cd RegiLattice
pip install -e ".[dev]"

# Verify everything works
python -m pytest tests/ -x --tb=short
python -m ruff check regilattice/ tests/
python -m regilattice --list
```

## Adding a New Tweak

This is the most common contribution. The plugin architecture makes it simple:

### 1. Find or create the module

Each category has its own file (`regilattice/tweaks/<category>.py`).
If your tweak fits an existing category, add to that file. Otherwise create
a new `.py` file -- the loader discovers it automatically.

### 2. Follow the triplet pattern

```python
"""My category tweaks."""
from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Example"


def _apply_example(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY], "example")
    SESSION.set_dword(_KEY, "Disabled", 1)


def _remove_example(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY, "Disabled")


def _detect_example() -> bool:
    return SESSION.read_dword(_KEY, "Disabled") == 1


TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-example",              # globally unique kebab-case
        label="Disable Example Feature",
        category="My Category",
        apply_fn=_apply_example,
        remove_fn=_remove_example,
        detect_fn=_detect_example,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY],
        description="Disables the example feature via registry policy.",
        tags=["example", "policy"],
    ),
]
```

### 3. Verify

```bash
python -m regilattice --list | findstr example
python -m pytest tests/test_tweaks_smoke.py -x --tb=short
python -m ruff check regilattice/ tests/
```

The smoke tests auto-discover your new tweak and validate its signatures,
ID uniqueness, and detect function.

**No edits to `tweaks/__init__.py` needed.**

### 4. Submit a PR

Use the PR template and ensure the checklist is complete.

---

## Code Style & Commit Messages

See the [project conventions](copilot-instructions.md#common-pitfalls) and
[Git conventions](INSTRUCTIONS.md#git-conventions) for full details.

## Testing

```bash
# Full suite (8 000+ tests)
python -m pytest tests/ -v --tb=short

# Smoke tests only (fastest, covers all tweaks)
python -m pytest tests/test_tweaks_smoke.py -x --tb=short

# With coverage
python -m pytest tests/ --cov=regilattice --cov-report=term-missing

# Lint + type check
python -m ruff check regilattice/ tests/
python -m mypy regilattice/
```

## Key Files to Know

| File | What it does |
|---|---|
| `regilattice/tweaks/__init__.py` | Plugin loader, TweakDef, profiles, batch ops |
| `regilattice/tweaks/_template.py` | Detailed contributor guide with patterns |
| `regilattice/registry.py` | Registry read/write/backup/logging |
| `regilattice/corpguard.py` | Corporate network detection |
| `tests/conftest.py` | Test fixtures (dry_session, all_tweaks_list) |
| `tests/test_tweaks_smoke.py` | Auto-parametrized smoke tests |

## Common Pitfalls

- **Duplicate IDs** -- The loader raises `ValueError` at import. Each ID must be unique
  across ALL 63+ modules. Run `python -m regilattice --list` to check.
- **Unicode characters** -- Use plain ASCII hyphens `-` and quotes `"`. Ruff flags
  en-dashes and smart quotes as confusables (RUF001/RUF003).
- **`require_admin` kwarg** -- Must be present as `*, require_admin: bool = True` on
  every apply/remove function, even if unused. ARG002 is suppressed for this.
- **Testing on CI** -- Tests use `_dry_run=True` so no real registry writes happen.
  `detect_fn` tests call the function but don't assert specific values (host-dependent).

---

## Project Structure

See [ARCHITECTURE.md](.github/ARCHITECTURE.md) for data flow diagrams and
design decisions. See [instructions.md](.github/instructions.md) for the
complete project reference.
