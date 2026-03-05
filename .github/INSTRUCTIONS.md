# RegiLattice -- Instructions

> The canonical project reference is [copilot-instructions.md](copilot-instructions.md)
> (auto-loaded by GitHub Copilot). This file covers Git conventions and the
> pre-commit checklist only.

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

---

See also: [ARCHITECTURE.md](ARCHITECTURE.md), [CONTRIBUTING.md](CONTRIBUTING.md), [SKILLS.md](SKILLS.md)
