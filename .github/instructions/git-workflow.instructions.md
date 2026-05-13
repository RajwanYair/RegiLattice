---
applyTo: "**"
---

# Git Workflow — Commit & Push Strategy

## ⛔ STANDING RULE: GitHub Release on Every Version Bump

> **This rule is unconditional and applies to every session, every sprint, every change.**

Every time a version is bumped (PATCH, MINOR, or MAJOR), you **MUST**:

1. Commit the version bump
2. Create a git tag matching the version (`vX.Y.Z`)
3. Push the commit AND the tag immediately

The tag push triggers the GitHub Actions `release.yml` workflow which automatically:
- Builds self-contained `RegiLattice.GUI.exe` (win-x64, single file)
- Builds self-contained `RegiLatticeCLI.exe` (win-x64, single file)
- Builds the MSI installer via WiX
- Publishes all artifacts as a new GitHub Release

**No version bump may be committed, tagged, or pushed without the complete tag push sequence.**
Deferring the `git push --tags` is a workflow violation. Release artifacts must be published simultaneously with the version tag.

```powershell
# ✅ MANDATORY — must run exactly this sequence on every version bump
git add -A
git commit -m "chore: bump version to vX.Y.Z"
git tag vX.Y.Z
git push
git push --tags   # ← TRIGGERS release.yml → EXEs + MSI → GitHub Releases
```

---

## Core Principle

**Commit often within a session. Push on every version bump.**

Every time a version is bumped (PATCH, MINOR, or MAJOR), that commit **must be tagged
and pushed immediately** — this triggers the GitHub Actions `release.yml` workflow
which publishes self-contained EXEs and MSI to GitHub Releases.

Non-version-bump work (docs, config, intermediate sprints without a version change)
stays in local commits until the next version bump. Never push non-versioned
incremental work mid-session; only push when a versioned release is ready.

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

**Push immediately on every version bump** — the tag push triggers the GitHub Actions
`release.yml` workflow that publishes EXEs + MSI to GitHub Releases. This is a
**standing rule** — every release must be published as soon as the version is tagged.

```powershell
# Every version bump must follow this exact sequence:
git add -A
git commit -m "feat(tweaks): Sprint NNN — N tweaks, vX.Y.Z\n\nTotal: NNNN tweaks, NNNN tests"
git tag vX.Y.Z
git push
git push --tags   # ← triggers release.yml → publishes EXEs to GitHub Releases
```

For non-version-bump work, push criteria (at end of session):

1. All relevant tests pass (full `dotnet test` suite, 0 failures)
2. Release build succeeds with 0 errors, 0 warnings
3. No stale debug code, temp files, or `.tmp/` leftovers
4. CHANGELOG.md updated if features/fixes were added

```powershell
# End-of-session push flow
dotnet build RegiLattice.sln -c Release -m:1
dotnet test tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s -c Release --logger "console;verbosity=minimal"
dotnet test tests/RegiLattice.CLI.Tests/RegiLattice.CLI.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s -c Release --logger "console;verbosity=minimal"
dotnet test tests/RegiLattice.GUI.Tests/RegiLattice.GUI.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s -c Release --logger "console;verbosity=minimal"
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
| Version bump / release        | `release/vX.Y.Z` → PR to `main`         |
| Experimental redesign (risky) | `feature/<name>` → PR to `main`          |
| Hotfix from production        | `hotfix/<description>` → PR to `main`    |

Direct commits to `main` are allowed for small, low-risk changes (docs, config tweaks,
single-file fixes). For **version bumps** and **multi-file features**, use a branch + PR
so CI validates the change before it lands on `main`.

---

## GitHub Issues & Pull Requests in the Release Flow

> **This is a standing rule. Every version bump MUST be tracked by a GitHub Issue
> and merged via a Pull Request.**

### Issue-Driven Development

| Event                         | Action                                                |
| ----------------------------- | ----------------------------------------------------- |
| Bug discovered                | Create or find a **Bug Report** issue                 |
| Feature planned               | Create a **Feature Request** issue                    |
| New tweaks needed             | Create a **New Tweak Proposal** issue                 |
| Version bump planned          | Create a **Release Checklist** issue from `.github/ISSUE_TEMPLATE/release.yml` |
| CI/Release fails              | Inspect the failed workflow run and track follow-up in a `ci-failure` issue if needed |

Every commit that addresses an issue MUST reference it in the footer:

```
fix(engine): handle null TweakDef in Search

Closes #42
```

### Release via Pull Request (MANDATORY for version bumps)

**Current path**:

1. Create a release issue from `.github/ISSUE_TEMPLATE/release.yml`
2. Check out the release branch locally: `git checkout -b release/vX.Y.Z`
3. Update all version files (follow the checklist in the release issue)
4. Push changes to the release branch and open a draft PR to `main`
5. Mark the PR as ready → CI runs automatically
6. Merge the PR to `main` (squash or merge commit)
7. Tag and push immediately after merge:
   ```powershell
   git checkout main
   git pull
   git tag vX.Y.Z
   git push --tags   # ← TRIGGERS release.yml
   ```
8. Verify the release, then close the release issue

**Manual path** (for urgent hotfixes):

```powershell
git checkout -b release/vX.Y.Z
# ... make version file changes ...
git add -A
git commit -m "chore: bump version to vX.Y.Z"
git push -u origin release/vX.Y.Z
# Create PR via gh CLI:
gh pr create --title "chore: release vX.Y.Z" --body "Closes #NN" --base main
# After CI passes and PR is merged:
git checkout main; git pull
git tag vX.Y.Z
git push --tags
```

### Linking Issues to PRs

- **In PR body**: Use `Closes #N` to auto-close issues when the PR merges
- **In commit messages**: Use `Closes #N` or `Fixes #N` in the footer
- **Multiple issues**: List each on a separate line: `Closes #42\nCloses #43`
- **Related but not closing**: Use `Related to #N` or `Ref #N`

### When a PR is NOT Required

Small, low-risk changes may be committed directly to `main`:

- Documentation-only changes (`.md`, `.svg`, comments)
- Config file updates (`.vscode/`, `.github/instructions/`)
- Single-file formatting or style fixes
- Instruction file updates

Even for direct commits, reference the related issue if one exists.

---

## Version Bumping

**Do not bump the version unless explicitly asked after manual review.**

Version follows **Semantic Versioning**: `MAJOR.MINOR.PATCH`

- `PATCH` bump: bug fixes, dead code removal, refactoring, docs trimming
- `MINOR` bump: new features, new tweaks, new dialogs/services
- `MAJOR` bump: breaking API changes or major architectural overhauls

When bumping:

0. **Create a release issue** — create it from the Release Checklist template, then create the release branch and draft PR manually.
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
4. Update `Readme.md` version badge, test badge, tweak/test counts, and Mermaid diagram counts if changed
5. Update all count-bearing metadata files — **see the full 28-file checklist** in `lessons-learned.instructions.md` (section "SVG Count Update Checklist"). Counts tracked across files: **tweaks · categories · modules · tests · themes · profiles · pkg-managers**
6. **Update all SVG graphics** — every version bump where counts change MUST update the SVG files in `docs/assets/`:
    - `stats.svg` — tweaks + categories + tests (plain digits: `7718`, never space-separated)
    - `banner.svg` — tweaks, categories, tests, themes, profiles
    - `architecture.svg` — stats badge + category pills: tweak count · category count · module file count
    - `how-it-works.svg` — tweaks count in Browse step
    - `features.svg` — per-category tweak counts (if categories changed)
    - `project-structure.svg` — file count · tweak count · category count
    - `solution-overview.svg` — file count · tweak count
    ```powershell
    # Quick bulk-replace for SVGs (run after counts change):
    (Get-Content "docs\assets\stats.svg") -replace 'OLD_TWEAK_COUNT', 'NEW_TWEAK_COUNT' | Set-Content "docs\assets\stats.svg"
    ```
7. Update package registry manifests: `powershell/RegiLattice.psd1`, `Dockerfile` — version + description counts
8. Update GitHub About sidebar: `gh repo edit RajwanYair/RegiLattice --description "... N,NNN tweaks ..."`
9. Commit: `chore: bump version to vX.Y.Z`
10. Tag: `git tag vX.Y.Z`
11. Push: `git push; git push --tags`  ← **TRIGGERS release.yml → EXE + CLI + MSI published to GitHub Releases**

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
dotnet test tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s --logger "console;verbosity=minimal"
dotnet test tests/RegiLattice.CLI.Tests/RegiLattice.CLI.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s --logger "console;verbosity=minimal"
dotnet test tests/RegiLattice.GUI.Tests/RegiLattice.GUI.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s --logger "console;verbosity=minimal"

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

---

## GitHub Actions — CI/CD Reference

### Canonical Action Versions (verified 2026-04-10)

> **Before pinning any action, verify the version exists on the action's GitHub releases page.**
> Pinning a non-existent version silently fails the CI step.

| Action | Stable Version |
|--------|---------------|
| `actions/checkout` | `@v6` |
| `actions/setup-dotnet` | `@v5` |
| `actions/cache` | `@v5` |
| `actions/upload-artifact` | `@v7` |
| `codecov/codecov-action` | `@v6` |
| `github/codeql-action/init` | `@v4` |
| `github/codeql-action/analyze` | `@v4` |
| `github/codeql-action/upload-sarif` | `@v4` |
| `actions/dependency-review-action` | `@v4` |
| `actions/labeler` | `@v6` |
| `actions/github-script` | `@v8` |
| `actions/stale` | `@v10` |

### Workflow Ecosystem

| Workflow | Trigger | Purpose |
| -------- | ------- | ------- |
| `ci.yml` | `push`, `pull_request`, weekly schedule, manual dispatch | Build, test, vulnerability check, dependency review, pack validation, weekly mutation testing |
| `release.yml` | `v*` tag push, manual dispatch | Versioned release artifacts, GitHub Release, Chocolatey package |
| `weekly.yml` | Monday schedules, manual dispatch | CodeQL, stale issue/PR management, PSScriptAnalyzer |

### Post-Release Verification (MANDATORY after every tag push)

After every `git push --tags`, wait 3–5 minutes then run:

```powershell
# 1. Check all workflow runs for the tag — all must show 'success'
gh run list --repo RajwanYair/RegiLattice --limit 5

# 2. Spot-check the release workflow job conclusions
$runId = (gh run list --repo RajwanYair/RegiLattice --workflow release.yml --limit 1 --json databaseId | ConvertFrom-Json)[0].databaseId
gh run view $runId --repo RajwanYair/RegiLattice --json jobs | ConvertFrom-Json | Select-Object -ExpandProperty jobs | Select-Object name,conclusion

# 3. Verify the release exists with all expected artifacts
gh release view vX.Y.Z --repo RajwanYair/RegiLattice
```

**Expected artifacts on every release:**
- `RegiLattice-vX.Y.Z-win-x64.exe` — GUI portable (self-contained win-x64)
- `RegiLatticeCLI-vX.Y.Z-win-x64.exe` — CLI portable (self-contained win-x64)
- `SHA256SUMS.txt` — checksums file

**If release.yml failed** — re-trigger by deleting and re-pushing the tag:
```powershell
git tag -d vX.Y.Z; git push origin :refs/tags/vX.Y.Z; git tag vX.Y.Z; git push --tags
```

### CI Best Practices

- **`MSBUILDDISABLENODEREUSE: 1`** at job level — prevents MSBuild node file-lock (MSB3492)
- **`FORCE_JAVASCRIPT_ACTIONS_TO_NODE24: true`** at job level — avoids Node 16/20 deprecation warnings
- **Per-project test runs** — NEVER use `dotnet test RegiLattice.sln`; run Core → CLI → GUI individually to avoid cross-assembly file-write races
- **No `--no-build` for GUI.Tests** — always build before testing; Windows SDK runtime packs require the build copy step
- **`paths-ignore` for docs-only changes** — skip CI on `.md`, `.svg`, `.txt`, instruction file changes
- **No `-q`/`--verbosity quiet`** — causes question-build aborts; use `--verbosity minimal` or no flag
- **Stryker mutation testing** — move to weekly cron schedule; adds ~15 min per push otherwise
