# Contributing to RegiLattice

Thank you for your interest in contributing!

## Getting Started

See the [Development Guide](docs/Development.md) for the full setup,
workflow, testing, and coding standards reference.

## Quick Reference

| Task | Command |
|------|---------|
| Install (dev) | `pip install -e ".[dev]"` |
| Lint | `python -m ruff check regilattice/ tests/` |
| Type-check | `python -m mypy regilattice/` |
| Test | `python -m pytest tests/ -v --tb=short` |
| Test (changed only) | `pwsh scripts/test-changed.ps1` |

## Adding a New Tweak

1. Create or edit a module in `regilattice/tweaks/`.
2. Implement the `_apply_*` / `_remove_*` / `_detect_*` triplet.
3. Append a `TweakDef(...)` to the module `TWEAKS` list.
4. Run `python -m pytest tests/test_tweaks_smoke.py -x --tb=short`.

See [`regilattice/tweaks/_template.py`](regilattice/tweaks/_template.py) for
a detailed contributor guide with working examples and common patterns.

## Commit Messages

Use [Conventional Commits](https://www.conventionalcommits.org/) format:
`type(scope): description` (e.g., `feat(tweaks): add bluetooth-disable-handsfree`).

## Corporate Safety

Tweaks with `corp_safe=False` are blocked on corporate networks. If your tweak
only touches `HKCU` and poses no enterprise risk, set `corp_safe=True`.
