---
name: release-notes
description: "Auto-draft CHANGELOG entries and release notes from git commits. Use when preparing a new release, summarising what changed since last tag, drafting the CHANGELOG section, or generating GitHub Release body text. Triggers on: 'draft changelog', 'release notes', 'what changed', 'CHANGELOG entry', 'generate release notes', 'summarise commits'."
argument-hint: "Optional: target version (e.g. 'v6.35.0') or commit range (e.g. 'v6.33.0..HEAD')"
---

# Release Notes — RegiLattice

## Objective

Generate a well-structured CHANGELOG entry for `docs/CHANGELOG.md` and a concise
GitHub Release body from the git commit log since the last tag.

## Step 1 — Identify the commit range

```powershell
# Find the most recent version tag
$LastTag = git.exe describe --tags --abbrev=0 2>$null
if (-not $LastTag) { $LastTag = 'v6.0.0' }

# List commits since last tag (one line each)
git.exe log "$LastTag..HEAD" --oneline
```

## Step 2 — Categorise commits by Conventional Commits type

Group by the type prefix (`feat`, `fix`, `perf`, `refactor`, `test`, `docs`, `chore`, `style`):

| Commit type | CHANGELOG section  |
|-------------|--------------------|
| `feat`      | ### Added          |
| `fix`       | ### Fixed          |
| `perf`      | ### Performance    |
| `refactor`  | ### Changed        |
| `test`      | ### Tests          |
| `docs`      | ### Documentation  |
| `chore`     | *(skip or Internal)* |
| `style`     | *(skip)*           |

## Step 3 — Draft the CHANGELOG entry

```markdown
## [X.Y.Z] — YYYY-MM-DD

### Added
- feat(tweaks): Sprint NNN — 50 new policy tweaks (PolicyBITS/PersonalizationPolicy/...)

### Fixed
- fix(engine): SnapshotManager.Save() no longer triggers live StatusMap on 7k+ tweaks

### Tests
- test(core): Split ExtendedCoverageTests monolith into 27 per-topic files

### Stats
- Tweaks: **X,XXX** | Categories: **NNN** | Modules: **NNN** | Tests: **N,NNN**
```

## Step 4 — Get current stats

```powershell
# Run the validate command to confirm stats
dotnet run --project src/RegiLattice.CLI -- --stats --no-color
```

Or use the counts script:
```powershell
.\scripts\Sync-CopilotInstructions.ps1 -DryRun
```

## Step 5 — Generate GitHub Release body

The GitHub Release body is a shortened version of the CHANGELOG entry:

```markdown
## What's New in vX.Y.Z

**[Two-sentence summary of main changes]**

### Highlights
- Bullet 1 (most impactful change)
- Bullet 2
- Bullet 3

### Stats
7,XXX tweaks · NNN categories · NNN modules · N,NNN tests · 0 failures

### Download
- [RegiLattice-vX.Y.Z-win-x64.exe](…) — portable GUI (no install)
- [RegiLatticeCLI-vX.Y.Z-win-x64.exe](…) — portable CLI (no install)
- [RegiLattice-vX.Y.Z-win-x64.msi](…) — MSI installer
```

## File Update Checklist (for each version bump)

The complete 28-file checklist is in `lessons-learned.instructions.md` (stats.svg section).
Minimum required for every version bump:

```
☐ docs/CHANGELOG.md   — prepend new ## [X.Y.Z] section
☐ Directory.Build.props — all 4 version properties in sync
☐ installer/Package.wxs — Version="X.Y.Z"
☐ README.md            — version badge + tweak/test counts
☐ docs/assets/stats.svg — plain integers (7718, not 7,718)
☐ .github/copilot-instructions.md — tweak/test count in Quick Facts table
```

Run `Sync-CopilotInstructions.ps1` to auto-update counts across all instruction files.

## Common Mistakes

- **Wrong CHANGELOG date**: always use `Get-Date -Format "yyyy-MM-dd"` — never use planned/future dates
- **Stale stats in body**: run `--stats` just before publishing the release to get the live numbers
- **Missing `vX.Y.Z` tag**: the tag push is what triggers `release.yml` — never defer it
