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

1. All relevant tests pass (full `dotnet test` suite, 0 failures)
2. Release build succeeds with 0 errors, 0 warnings
3. No stale debug code, temp files, or `.tmp/` leftovers
4. CHANGELOG.md updated if features/fixes were added

```powershell
# End-of-session push flow
dotnet build RegiLattice.sln -c Release -m:1
dotnet test RegiLattice.sln --settings tests/.runsettings --blame-hang-timeout 60s --no-build -c Release --logger "console;verbosity=minimal"
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

Version follows **Semantic Versioning**: `MAJOR.MINOR.PATCH`

- `PATCH` bump: bug fixes, dead code removal, refactoring, docs trimming
- `MINOR` bump: new features, new tweaks, new dialogs/services
- `MAJOR` bump: breaking API changes or major architectural overhauls

When bumping:

1. Update `Directory.Build.props` — ALL four version properties must be kept in sync:
    ```xml
    <Version>X.Y.Z</Version>
    <AssemblyVersion>X.Y.Z.0</AssemblyVersion>
    <FileVersion>X.Y.Z.0</FileVersion>
    <InformationalVersion>X.Y.Z</InformationalVersion>
    ```
    No `Properties/AssemblyInfo.cs` exists — `Directory.Build.props` is the single source of truth.
2. Update `installer/Package.wxs` → `Version="X.Y.Z.0"`
3. Add `## [X.Y.Z] — YYYY-MM-DD` section to `docs/CHANGELOG.md`
4. Update `Readme.md` version badge and tweak/test counts if changed
5. Commit: `chore: bump version to vX.Y.Z`
6. Tag: `git tag vX.Y.Z`
7. Push at session end: `git push; git push --tags`

---

## Per-Sprint / Per-Iteration Commit Mandate

> **This is a standing rule that applies to every future session.**

Every sprint or named task phase MUST produce at least one commit **before moving to the next sprint.** Do not batch multiple sprints into one commit. The commit should clearly state:

- What sprint/phase it covers
- Total tweak count if tweaks were added
- Total test count if tests were added/changed

```
feat(tweaks): Sprint 48 — 50 new tweaks (Bluetooth/Printing/TouchPen/Speech/Storage)

Bluetooth.cs +10, Printing.cs +10, TouchPen.cs +10, Speech.cs +10, Storage.cs +10
5 dialog enhancements (+2 features each)
Total: 3046 tweaks, 1387 tests (0 failures)
```

---

## Quick Reference

```powershell
# Commit a logical phase / sprint
git add -A
git commit -m "feat(tweaks): add 10 network tweaks"

# Amend last commit (before push only)
git commit --amend --no-edit

# Build + test before committing
dotnet build RegiLattice.sln -c Debug -m:1
dotnet test RegiLattice.sln --settings tests/.runsettings --blame-hang-timeout 60s --no-build --logger "console;verbosity=minimal"

# End-of-session push (includes tags)
git push
git push --tags

# Bump and tag a release
# 1. Edit Directory.Build.props <Version> and installer/Package.wxs
git add -A
git commit -m "chore: bump version to vX.Y.Z"
git tag vX.Y.Z
git push; git push --tags
```
