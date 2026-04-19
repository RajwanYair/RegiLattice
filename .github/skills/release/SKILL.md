---
name: release
description: "Bump the version, update the changelog, tag, and publish a GitHub release for RegiLattice. Use when preparing a new release, updating version numbers, creating a CHANGELOG entry, or pushing a version tag to trigger the CI release workflow. Triggers on: 'bump version', 'release', 'publish', 'version', 'tag', 'changelog', 'MINOR bump', 'PATCH bump'."
argument-hint: "Version type: patch, minor, or major (e.g. 'minor bump for new features')"
---

# Release — RegiLattice

## Version Scheme

`MAJOR.MINOR.PATCH` — Semantic Versioning

| Bump    | When                                                          |
| ------- | ------------------------------------------------------------- |
| `PATCH` | Bug fixes, refactoring, docs, dead code removal, config fixes |
| `MINOR` | New tweaks, new features, new dialogs/services                |
| `MAJOR` | Breaking API changes, architectural overhauls                 |

## Files to Update (31-item checklist — see full details in `lessons-learned.instructions.md`)

> The canonical checklist with all 28 files + 3 external actions lives in
> `.github/instructions/lessons-learned.instructions.md` (section "stats.svg Uses
> Space-Separated Thousands"). Always reference that as the source of truth.

### Group A — Version properties (EVERY version bump)

| #   | File                    | Property / Pattern                                                          |
| --- | ----------------------- | --------------------------------------------------------------------------- |
| 1   | `Directory.Build.props` | `<Version>`, `<AssemblyVersion>`, `<FileVersion>`, `<InformationalVersion>` |
| 2   | `installer/Package.wxs` | `Version="X.Y.Z"` (inside `<Package ...>`)                                  |

### Group B — SVG graphics (when counts change)

| #   | File                                | What changes                                                                   |
| --- | ----------------------------------- | ------------------------------------------------------------------------------ |
| 3   | `docs/assets/stats.svg`             | Tweaks + categories + tests (space-separated thousands)                        |
| 4   | `docs/assets/banner.svg`            | Tweaks · categories · tests · themes · profiles                                |
| 5   | `docs/assets/features.svg`          | Per-category tweak count badges                                                |
| 6   | `docs/assets/architecture.svg`      | Stats badge + category pills: tweak count · category count · module file count |
| 7   | `docs/assets/how-it-works.svg`      | Tweaks count in Browse step                                                    |
| 8   | `docs/assets/project-structure.svg` | File count · tweak count · category count                                      |
| 9   | `docs/assets/solution-overview.svg` | File count · tweak count                                                       |

### Group C — Documentation & instruction files

| #   | File                                                   | What changes                                                                    |
| --- | ------------------------------------------------------ | ------------------------------------------------------------------------------- |
| 10  | `README.md`                                            | Version badge, test badge, download link, description, features, diagram counts |
| 11  | `CHANGELOG.md` (root stub)                             | Latest version entry summary                                                    |
| 12  | `docs/CHANGELOG.md`                                    | Prepend new `## [X.Y.Z]` section with Stats line                                |
| 13  | `docs/Development.md`                                  | Header "Last updated" date + version                                            |
| 14  | `docs/Roadmap.md`                                      | Baseline counts if changed                                                      |
| 15  | `.github/copilot-instructions.md`                      | Header, version table, tweak/category/module/test counts                        |
| 16  | `.github/instructions/workspace.instructions.md`       | Tweaks/module count in `Tweaks/` directory comment                              |
| 17  | `.github/instructions/lessons-learned.instructions.md` | Header date + version + counts                                                  |
| 18  | `.github/instructions/testing.instructions.md`         | Test project counts table (Core/CLI/GUI/Total)                                  |
| 19  | `.github/agents/regilattice.agent.md`                  | "Current state" line: tweak/category/module/test counts                         |

### Group D — Package registry manifests (version + description counts)

| #   | File                                               | What changes                                               |
| --- | -------------------------------------------------- | ---------------------------------------------------------- |
| 20  | `chocolatey/regilattice.nuspec`                    | `<version>`, `<summary>`, description counts               |
| 21  | `scoop/regilattice.json`                           | `version`, `url`, `hash`, description counts               |
| 22  | `winget/RegiLattice.RegiLattice.yaml`              | `PackageVersion`                                           |
| 23  | `winget/RegiLattice.RegiLattice.installer.yaml`    | `PackageVersion`, `InstallerUrl`                           |
| 24  | `winget/RegiLattice.RegiLattice.locale.en-US.yaml` | `PackageVersion`, `ShortDescription`, `Description` counts |
| 25  | `powershell/RegiLattice.psd1`                      | `ModuleVersion`                                            |

### Group E — Derived files (update AFTER release build)

| #   | File         | What changes               |
| --- | ------------ | -------------------------- |
| 26  | `Dockerfile` | `LABEL` description counts |

### Group F — External (post-push)

| #   | Action               | What changes                                       |
| --- | -------------------- | -------------------------------------------------- |
| 27  | GitHub About sidebar | `gh repo edit` — update tweak count in description |

## Step-by-Step Release Process

### 0. Create a release issue and branch (MANDATORY for version bumps)

**Current path**:

1. Create a release issue from `.github/ISSUE_TEMPLATE/release.yml`
2. Create the release branch locally: `git checkout -b release/vX.Y.Z`
3. Push the branch and open a draft PR to `main`

**Manual path** (for local-only prep):

```powershell
# Create branch
git checkout -b release/vX.Y.Z

# Create issue via gh CLI
gh issue create --title "Release vX.Y.Z" --label "release" --body "Release checklist for vX.Y.Z. See PR."

# After pushing changes:
gh pr create --title "chore: release vX.Y.Z" --body "Closes #NN" --base main
```

### 1. Pre-flight checks

```powershell
# Run all tests — must be 0 failures
dotnet test tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s
dotnet test tests/RegiLattice.CLI.Tests/RegiLattice.CLI.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s
dotnet test tests/RegiLattice.GUI.Tests/RegiLattice.GUI.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s

# Validate tweak integrity before tagging
dotnet run --project src/RegiLattice.CLI/RegiLattice.CLI.csproj -c Release --no-build -- --validate

# Release build — must be 0 errors, 0 warnings (build each project sequentially)
dotnet build src/RegiLattice.Core/RegiLattice.Core.csproj  -c Release
dotnet build src/RegiLattice.GUI/RegiLattice.GUI.csproj    -c Release
dotnet build src/RegiLattice.CLI/RegiLattice.CLI.csproj    -c Release
```

### 2. Bump version in all files

Update ALL properties simultaneously — never leave them out of sync:

```xml
<!-- Directory.Build.props -->
<Version>X.Y.Z</Version>
<AssemblyVersion>X.Y.Z.0</AssemblyVersion>
<FileVersion>X.Y.Z.0</FileVersion>
<InformationalVersion>X.Y.Z</InformationalVersion>
```

### 3. Add CHANGELOG entry in `docs/CHANGELOG.md`

```markdown
## [X.Y.Z] — YYYY-MM-DD

### Added

- ...

### Fixed

- ...

### Stats

- Tweaks: **N NNN**
- Tests: **N NNN** passing (NNN Core + NNN CLI + NNN GUI)
- Version bumped `X.Y.Z-1` → `X.Y.Z`
```

### 4. Update root `CHANGELOG.md` stub with the latest entry summary

### 5. Push to release branch and merge PR

```powershell
git add -A
git commit -m "chore: bump version to vX.Y.Z

- Directory.Build.props: all 4 version properties updated
- installer/Package.wxs: Version updated
- README.md: badge + download link updated
- CHANGELOG(s): vX.Y.Z entry added
Closes #NN
Total: N NNN tweaks, N NNN tests (0 failures)"

git push origin release/vX.Y.Z
```

Mark the draft PR as ready for review. Wait for CI to pass. Merge to `main`.

### 6. Tag and push (triggers GitHub Actions release workflow)

```powershell
git checkout main
git pull
git tag vX.Y.Z
git push --tags   # ← TRIGGERS release.yml
```

### 7. Post-release verification (MANDATORY — do not skip)

Wait 3–5 minutes then:

1. Open the GitHub Actions tab: `https://github.com/RajwanYair/RegiLattice/actions/workflows/release.yml`
2. Confirm the workflow for `vX.Y.Z` shows a green ✅ (not ❌ or ⏳)
3. Open `https://github.com/RajwanYair/RegiLattice/releases/latest`
4. Verify the release exists with all expected assets:
    - `RegiLattice-vX.Y.Z-win-x64.exe` (GUI portable)
    - `RegiLatticeCLI-vX.Y.Z-win-x64.exe` (CLI portable)
    - `SHA256SUMS.txt` (checksums)
    - `RegiLattice-vX.Y.Z-win-x64.msi` (optional)
    - `RegiLattice-vX.Y.Z-win-x64.msix` (optional)
5. Close the release issue (auto-closed if PR body had `Closes #NN`)

**Only after these checks pass** is the release complete. Do NOT skip this step — past releases
failed silently (v3.5.0 succeeded; v3.7.3 workflow failed because the MSI build error cascaded
and killed the release upload step entirely).

## GitHub Actions Release Workflow

The tag push triggers `.github/workflows/release.yml` which:

1. Builds GUI + CLI as self-contained `win-x64` single-file executables
2. _Optionally_ builds the WiX `.msi` installer (`continue-on-error: true`)
3. _Optionally_ builds an `.msix` package and Chocolatey package
4. Creates a GitHub Release with all matched artifacts

The MSI build is **non-blocking** — if WiX toolchain fails, the EXEs are still released.

## Recovering a Failed Release

If the workflow fails (or POST-RELEASE VERIFICATION shows no release at the expected tag):

```powershell
# 1. Fix the workflow/code causing the failure
# 2. Commit the fix
git add -A
git commit -m "fix(ci): fix release workflow issue"

# 3. Delete the broken tag locally and on remote
git tag -d vX.Y.Z
git push origin :refs/tags/vX.Y.Z

# 4. Re-create and push the tag — this re-triggers the workflow
git tag vX.Y.Z
git push --tags
```

Alternatively, use `workflow_dispatch` on GitHub Actions UI:

- Go to Actions → Release → "Run workflow"
- Select the `vX.Y.Z` tag as the ref
- Optionally enter the tag name in the `tag_name` input

## README Download Link — Correct Format

The README download link must use the **version number** in the link text, NOT a specific filename.
The actual uploaded filenames depend on the build and may not include the version:

```markdown
# ✅ CORRECT — version in text, resolves to actual release page

👉 **[Download RegiLattice vX.Y.Z](https://github.com/RajwanYair/RegiLattice/releases/latest)** (MSI installer + portable EXE)

# ❌ WRONG — hardcodes a specific filename that may not exist or may differ

👉 **[Download RegiLattice-X.Y.Z-win-x64.msi](https://github.com/.../releases/latest)**
```

## Pre-Push Sanity: Scrub Intel / Corporate Mentions

**Run this BEFORE every tag push.** Commit messages, CI logs, and terminal output sometimes
leak Intel-internal strings (proxy hostnames, corp network domains, OneDrive paths) into
tracked files. This step catches them before they appear in the public GitHub history.

### 1. Scan all tracked files for Intel / corporate strings

```powershell
# Strings to grep for (case-insensitive)
$patterns = @(
    'intel\.com',
    'intel corporation',
    'proxy.*intel',
    'intel.*proxy',
    'OneDrive.*Intel',
    'Intel.*OneDrive',
    '\bintel\b',         # standalone word; adjust if 'intel' appears legitimately (e.g. "intelligent")
    'http_proxy',
    'https_proxy',
    'no_proxy',
    'PROXY_HOST',
    'ProxyAddress',
    'ProxyServer',
    'UseProxy',
    'pac\.intel',
    'webproxy',
    'corporate.*proxy',
    'proxy.*corporate'
)

$root = "c:\Users\ryair\OneDrive - Intel Corporation\Documents\MyScripts\RegiLattice"
foreach ($pat in $patterns) {
    $hits = git -C $root grep -Iin $pat -- '*.cs' '*.md' '*.yml' '*.yaml' '*.json' '*.wxs' '*.props' '*.targets' '*.ps1' '*.psm1' '*.psd1' 2>$null
    if ($hits) { Write-Host "=== PATTERN: $pat ==="; $hits }
}
```

### 2. Fix any hits before committing

| Hit type                                                    | Action                                                                                                                                             |
| ----------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------- |
| Intel proxy URL in `.yml` / `.env` / config files           | Delete the line or replace with an empty string / placeholder                                                                                      |
| `http_proxy` / `https_proxy` env vars in workflow YAML      | Remove entirely — GitHub-hosted runners don't need them                                                                                            |
| `OneDrive - Intel Corporation` in a path embedded in source | Replace with a relative path or `%USERPROFILE%` placeholder                                                                                        |
| `intel.com` in a comment, doc, or URL                       | Replace with the public equivalent or remove                                                                                                       |
| `ProxyAddress` / `ProxyServer` registry key in a tweak      | This is a legitimate Windows registry tweak — keep it, but verify it uses the generic `HKCU\...\Internet Settings` path, not a hardcoded Intel URL |

### 3. Proxy-related registry tweaks are ALLOWED — with conditions

Tweaks that manage Windows proxy settings (e.g. `HKCU\Software\Microsoft\Windows\CurrentVersion\Internet Settings`)
are legitimate RegiLattice tweaks and must NOT be removed. Only remove hardcoded corporate values:

```csharp
// ✅ ALLOWED — generic proxy disable tweak, no Intel URL
RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyEnable", 0)

// ❌ REMOVE — hardcoded corporate proxy server
RegOp.SetString(@"HKCU\...\Internet Settings", "ProxyServer", "proxy.intel.com:911")
```

### 4. After cleanup, re-run the scan to confirm zero hits

```powershell
# Quick all-clear check
git -C $root grep -Iin 'intel\.com\|proxy.*intel\|intel.*proxy\|OneDrive.*Intel' -- '*.cs' '*.md' '*.yml' '*.json' '*.wxs' '*.props'
# Expected output: (empty)
```

Only proceed to `git tag` and `git push --tags` when the scan returns no hits.

---

## What NOT to Do

- **Don't bump version mid-session** — only bump when work is complete and tests pass
- **Never leave 4 version properties out of sync** — they all must match `X.Y.Z`
- **Don't push without tests passing** — 0 failures required
- **Don't forget `installer/Package.wxs`** — MSI silently embeds the old version otherwise
- **Don't skip post-release verification** — the workflow can fail silently; always check Actions tab
- **Don't hardcode a specific MSI filename** in README — it may not exist if WiX build fails
- **Don't push before running the Intel/proxy scrub** — corporate strings in public commits are irreversible without a history rewrite
