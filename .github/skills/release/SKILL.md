---
name: release
description: "Bump the version, update the changelog, tag, and publish a GitHub release for RegiLattice. Use when preparing a new release, updating version numbers, creating a CHANGELOG entry, or pushing a version tag to trigger the CI release workflow. Triggers on: 'bump version', 'release', 'publish', 'version', 'tag', 'changelog', 'MINOR bump', 'PATCH bump'."
argument-hint: "Version type: patch, minor, or major (e.g. 'minor bump for new features')"
---

# Release — RegiLattice

## Version Scheme

`MAJOR.MINOR.PATCH` — Semantic Versioning

| Bump | When |
|------|------|
| `PATCH` | Bug fixes, refactoring, docs, dead code removal, config fixes |
| `MINOR` | New tweaks, new features, new dialogs/services |
| `MAJOR` | Breaking API changes, architectural overhauls |

## Files to Update (all 4 version properties must stay in sync)

| File | Property / Pattern |
|------|--------------------|
| `Directory.Build.props` | `<Version>`, `<AssemblyVersion>`, `<FileVersion>`, `<InformationalVersion>` |
| `installer/Package.wxs` | `Version="X.Y.Z"` (inside `<Package ...>`) |
| `README.md` | `version-X.Y.Z` badge + download link |
| `docs/CHANGELOG.md` | New `## [X.Y.Z] — YYYY-MM-DD` section |
| `CHANGELOG.md` (root stub) | Same latest entry for GitHub community health |
| `.github/copilot-instructions.md` | `Version` and `Tests` rows in Quick Facts table |

## Step-by-Step Release Process

### 1. Pre-flight checks
```powershell
# Run all tests — must be 0 failures
dotnet test RegiLattice.sln --settings tests/.runsettings --blame-hang-timeout 60s

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

### 5. Commit
```powershell
git add -A
git commit -m "chore: bump version to vX.Y.Z

- Directory.Build.props: all 4 version properties updated
- installer/Package.wxs: Version updated
- README.md: badge + download link updated
- CHANGELOG(s): vX.Y.Z entry added
Total: N NNN tweaks, N NNN tests (0 failures)"
```

### 6. Tag and push (triggers GitHub Actions release workflow)
```powershell
git tag vX.Y.Z
git push
git push --tags
```

## GitHub Actions Release Workflow

The tag push triggers `.github/workflows/release.yml` which:
1. Builds GUI + CLI as self-contained `win-x64` single-file executables
2. Builds the WiX `.msi` installer
3. Creates a GitHub Release with all 3 artifacts attached
4. Release URL: `https://github.com/RajwanYair/RegiLattice/releases`

## What NOT to Do

- **Don't bump version mid-session** — only bump when work is complete and tests pass
- **Never leave 4 version properties out of sync** — they all must match `X.Y.Z`
- **Don't push without tests passing** — 0 failures required
- **Don't forget `installer/Package.wxs`** — MSI silently embeds the old version otherwise
