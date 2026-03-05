## Description

Brief description of what this PR does.

## Type of Change

- [ ] New tweak(s)
- [ ] Bug fix
- [ ] Feature (non-tweak)
- [ ] Refactor / code quality
- [ ] Documentation
- [ ] CI / build

## Changes

- Added/modified `regilattice/tweaks/xxx.py`: ...
- ...

## Checklist

- [ ] `python -m ruff check regilattice/ tests/` passes
- [ ] `python -m pytest tests/ -x --tb=short` passes
- [ ] `python -m mypy regilattice/` passes (or no regressions)
- [ ] New tweak IDs are globally unique kebab-case
- [ ] New tweaks have all three functions: apply, remove, detect
- [ ] `corp_safe` and `needs_admin` flags are set correctly
- [ ] No Unicode confusables (en-dashes, smart quotes) -- ASCII only
- [ ] Commit messages follow Conventional Commits format

## Testing

How did you test this? (e.g., ran smoke tests, tested on Windows 11 build XXXXX)

## Screenshots (if applicable)

GUI changes, status badge behaviour, etc.
