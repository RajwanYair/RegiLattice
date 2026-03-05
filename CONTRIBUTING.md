# Contributing to RegiLattice

Thank you for your interest in contributing! This guide will get you from zero to a merged tweak in minutes.

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
4. **Edit** `myapp.py` — define your registry key paths, implement the `_apply_*` / `_remove_*` / `_detect_*` triplet, and register a `TweakDef` in the `TWEAKS` list.
5. **Validate** your work:
   ```bash
   python -m ruff check regilattice/ tests/
   python -m mypy regilattice/
   python -m pytest tests/ -x --tb=short
   ```
6. **Commit** with a conventional commit message and open a PR.

## Project Structure

```
regilattice/
├── tweaks/            # Plugin modules — each exports TWEAKS: list[TweakDef]
│   ├── _template.py   # Contributor guide and example code
│   ├── performance.py # Example: 18 performance tweaks
│   └── ...            # 64 category modules, auto-discovered
├── cli.py             # argparse CLI
├── gui.py             # tkinter GUI (Catppuccin Mocha theme)
├── registry.py        # RegistrySession: winreg wrapper + backup + logging
├── config.py          # ~/.regilattice.toml support
└── corpguard.py       # Corporate network detection
```

## The Tweak Triplet Pattern

Every tweak implements three functions:

```python
def _apply_my_tweak(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY], "my-tweak")
    SESSION.set_dword(_KEY, "ValueName", 0)
    SESSION.log("Applied my-tweak")

def _remove_my_tweak(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY], "my-tweak-remove")
    SESSION.set_dword(_KEY, "ValueName", 1)
    SESSION.log("Removed my-tweak")

def _detect_my_tweak() -> bool:
    return SESSION.read_dword(_KEY, "ValueName") == 0
```

## TweakDef Registration

```python
TWEAKS: list[TweakDef] = [
    TweakDef(
        id="myapp-disable-telemetry",       # globally unique kebab-case
        label="Disable MyApp Telemetry",
        category="MyApp",                    # creates new category if needed
        apply_fn=_apply_my_tweak,
        remove_fn=_remove_my_tweak,
        detect_fn=_detect_my_tweak,
        needs_admin=True,                    # True for HKLM keys
        corp_safe=False,                     # True if HKCU-only and safe on corp
        registry_keys=[_KEY],
        description="Disables telemetry data collection in MyApp.",
        tags=["myapp", "telemetry", "privacy"],
    ),
]
```

## Coding Standards

| Tool | Config | Command |
|------|--------|---------|
| **Linter** | ruff (E, F, W, I, UP, B, SIM, RUF; line-length 150) | `ruff check regilattice/ tests/` |
| **Type checker** | mypy --strict (Python 3.10) | `mypy regilattice/` |
| **Tests** | pytest (~13 800 tests) | `pytest tests/ -x --tb=short` |

- `require_admin` keyword argument must be present on apply/remove functions (ARG002 is suppressed).
- Use `SESSION.backup()` before any registry mutation.
- Keep IDs globally unique (kebab-case: `<module>-<verb>-<noun>`).

## Commit Convention

Use [Conventional Commits](https://www.conventionalcommits.org/):

- `feat:` — new tweak, feature, or capability
- `fix:` — bug fix
- `refactor:` — code restructuring without behavior change
- `docs:` — documentation only
- `chore:` — tooling, CI, dependencies
- `ci:` — CI/CD changes

## Need Help?

- Read `regilattice/tweaks/_template.py` for detailed patterns and troubleshooting.
- Check `REFACTOR-PLAN.md` for the project roadmap.
- Look at existing tweak modules (e.g., `performance.py`, `privacy.py`) for real examples.
