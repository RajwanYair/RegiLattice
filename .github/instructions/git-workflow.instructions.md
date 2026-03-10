---
applyTo: "**"
---

# Git Workflow — Commit & Push Strategy

## Core Principle

**Commit often within a session. Push only at the end of a chat session.**

This keeps the history granular for rollbacks while keeping the remote branch
clean and reviewable. Never push mid-session — incomplete work belongs in
local commits only.

---

## Within a GitHub Copilot Chat Session

### When to commit

| Trigger                                                   | Action       |
| --------------------------------------------------------- | ------------ |
| A discrete task/phase is complete (e.g., "add 10 tweaks") | `git commit` |
| A bug is fixed and tests pass                             | `git commit` |
| A refactor or module is updated                           | `git commit` |
| A config/docs file is updated                             | `git commit` |
| Before switching to a different concern                   | `git commit` |

Aim for **one commit per logical iteration or phase**, not per file.
A "session" typically produces 3–10 commits covering distinct chunks of work.

### When NOT to commit

- Mid-refactor (broken state, failing tests)
- Temporary debug code or scratch changes
- Incomplete features with no working baseline

### When to push

Only after the **full chat session ends** and you have confirmed:

1. All relevant tests pass (`pwsh scripts/test-changed.ps1` or full suite)
2. Ruff reports no errors (`python -m ruff check regilattice/ tests/`)
3. No stale debug code, temp files, or `.pyc` leftovers

```powershell
# End-of-session push flow
python -m ruff check regilattice/ tests/
python -m pytest tests/ -q --tb=short
git push
```

---

## Commit Message Format

Use **Conventional Commits** (`type(scope): description`):

```
<type>(<scope>): <short description>

<optional body: what and why>

<optional footer: BREAKING CHANGE or Closes #N>
```

### Types

| Type       | When                                             |
| ---------- | ------------------------------------------------ |
| `feat`     | New tweak, feature, or capability                |
| `fix`      | Bug fix                                          |
| `perf`     | Performance improvement without behaviour change |
| `refactor` | Code restructuring without behaviour change      |
| `test`     | Add or update tests                              |
| `docs`     | Documentation, comments, instructions            |
| `chore`    | Dependency updates, CI, tooling, build files     |
| `style`    | Formatting, lint fixes (no logic change)         |

### Scope examples

`tweaks`, `gui`, `engine`, `cli`, `config`, `tests`, `ci`, `deps`, `registry`

### Examples

```
feat(tweaks): add 10 WSL performance tweaks (wsl module)

Adds wsl-memory-reclaim, wsl-swap-disable, wsl-cpu-pinning, ...
Total: now 1500 tweaks.

perf(engine): eliminate O(n) categories() scan, pre-build search pairs

categories() was iterating 1490 tweaks each call; now returns
_TWEAKS_BY_CAT.keys() in O(k=69).
search_tweaks replaced per-element function calls with pre-built
_TWEAKS_SEARCH_PAIRS for 2× speedup.

fix(gui): skip redundant pack/forget calls with _packed state tracking

TweakRow.pack_row/unpack_row now no-op when state hasn't changed,
eliminating 1490 redundant Tk calls per incremental search keystroke.

docs(instructions): add git workflow commit strategy

chore(vscode): remove stale pylint.args, add Pylance analysis settings
```

---

## Branching

| Scenario                      | Branch                                   |
| ----------------------------- | ---------------------------------------- |
| Normal development            | `main` (direct, trusted single-dev repo) |
| Experimental redesign (risky) | `feature/<name>`                         |
| Hotfix from production        | `hotfix/<description>`                   |

No PR required for `main` — this is a single-developer project.
Use `git tag v<version>` on release commits for GitHub releases.

---

## Version Bumping

**Do not bump the version unless explicitly asked after manual review.**

When bumping:

1. Update `VERSION` file
2. Update `pyproject.toml` → `[project] version`
3. Update `regilattice/__init__.py` → `__version__`
4. Add `CHANGELOG.md` entry
5. Commit: `chore: bump version to v1.x.y`
6. Tag: `git tag v1.x.y`

---

## Quick Reference

```powershell
# Commit a logical phase
git add -A
git commit -m "feat(tweaks): add 10 network tweaks"

# Amend last commit (before push only)
git commit --amend --no-edit

# End-of-session push
git push

# Check what would be tested for current changes
pwsh scripts/test-changed.ps1 -WhatIf  # (just lists files, no run)

# Run only changed tests before committing
pwsh scripts/test-changed.ps1
```
