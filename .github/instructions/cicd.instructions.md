---
applyTo: "**/*.yml,**/*.yaml,.github/**"
---

# CI/CD and GitHub Actions Instructions

## ⛔ STANDING RULE: Release EXEs on Every Version Bump

> **This rule applies to EVERY session without exception.**

Every time a version is bumped (PATCH, MINOR, or MAJOR), the tag **must be pushed immediately**
after committing. The `git push --tags` triggers the `release.yml` GitHub Actions workflow which:

1. Builds a self-contained `RegiLattice-vX.Y.Z-win-x64.exe` (GUI)
2. Builds a self-contained `RegiLatticeCLI-vX.Y.Z-win-x64.exe` (CLI)
3. Builds the MSI installer via WiX when available
4. Builds an MSIX package when the runner toolchain supports it
5. Publishes the release assets plus `SHA256SUMS.txt` on a new GitHub Release

**No version bump may be committed without a tag push.**

```powershell
# Mandatory sequence every version bump:
dotnet build RegiLattice.sln -c Release -m:1        # must succeed, 0 errors
dotnet test RegiLattice.sln --no-build -c Release   # must be 0 failures
git add -A
git commit -m "chore: bump version to vX.Y.Z"
git tag vX.Y.Z
git push
git push --tags   # ← TRIGGERS release.yml → GUI EXE + CLI EXE + MSI to GitHub Releases
```

Skipping the tag push is a standing workflow violation. Every release must be published
to GitHub Releases as soon as the version is tagged.

---

## Workflow Design Principles

- Pin action versions to a full SHA or semver tag (`@v4`, not `@main`)
- Use `permissions: contents: read` (least privilege) as default
- Cache NuGet packages to speed up runs
- Run on both push to main and pull_request
- Windows-only project — run on `windows-latest`

## Canonical Action Versions (verified 2026-04-10)

> **Before adding or bumping any action, verify the version exists on the action's GitHub releases page.**
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

```powershell
# Verify a version exists before pinning:
gh release list --repo actions/upload-artifact --limit 5
```

## Workflow Ecosystem

| Workflow | Trigger | Purpose |
| -------- | ------- | ------- |
| `ci.yml` | `push`, `pull_request`, weekly schedule, manual dispatch | Build, test, vulnerability check, dependency review, pack validation, weekly mutation testing |
| `release.yml` | `v*` tag push, manual dispatch | Versioned release artifacts, GitHub Release, Chocolatey package |
| `weekly.yml` | Monday schedules, manual dispatch | CodeQL, stale issue/PR management, PSScriptAnalyzer |
| `smoke.yml` | release published | Cross-runner smoke test for the published CLI artifact |
| `pages.yml` | `push` to `main`, manual dispatch | GitHub Pages deployment |
| `packages.yml` | release published, manual dispatch | Publish GitHub Packages NuGet package and GHCR container |

## Standard .NET CI Workflow Pattern

```yaml
name: CI

on:
    push:
        branches: [main]
        paths-ignore:
            - 'docs/**'
            - '**.md'
            - '**.svg'
            - '**.txt'
            - '.github/instructions/**'
    pull_request:
        branches: [main]

permissions:
    contents: read

jobs:
    build-and-test:
        runs-on: windows-latest
        env:
            MSBUILDDISABLENODEREUSE: 1
            FORCE_JAVASCRIPT_ACTIONS_TO_NODE24: true

        steps:
            - uses: actions/checkout@v6

            - uses: actions/setup-dotnet@v5
              with:
                  dotnet-version: "10.0.x"

            - name: Cache NuGet
              uses: actions/cache@v5
              with:
                  path: ~/.nuget/packages
                  key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
                  restore-keys: ${{ runner.os }}-nuget-

            - name: Restore
              run: dotnet restore RegiLattice.sln

            - name: Build
              run: dotnet build RegiLattice.sln -c Release --no-restore --verbosity minimal

            # Run each project individually to avoid cross-assembly file-write races
            - name: Test (Core)
              run: dotnet test tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj -c Release --no-restore --settings tests/.runsettings --blame-hang-timeout 30s --collect:"XPlat Code Coverage" --logger "console;verbosity=minimal"

            - name: Test (CLI)
              run: dotnet test tests/RegiLattice.CLI.Tests/RegiLattice.CLI.Tests.csproj -c Release --no-restore --settings tests/.runsettings --blame-hang-timeout 30s --logger "console;verbosity=minimal"

            - name: Test (GUI)
              run: dotnet test tests/RegiLattice.GUI.Tests/RegiLattice.GUI.Tests.csproj -c Release --no-restore --settings tests/.runsettings --blame-hang-timeout 30s --logger "console;verbosity=minimal"

            - name: Check for vulnerable NuGet packages
              shell: pwsh
              run: |
                  $output = dotnet list RegiLattice.sln package --vulnerable --include-transitive 2>&1
                  Write-Host $output
                  if ($output -match 'has the following vulnerable packages') {
                      Write-Warning '::warning::Vulnerable NuGet packages detected — review Dependabot PRs'
                  }

            - name: Validate TweakDef integrity
              run: >-
                  dotnet run --project src/RegiLattice.CLI/RegiLattice.CLI.csproj
                  -c Release --no-build
                  -- --validate

            - name: Upload coverage artifact
              uses: actions/upload-artifact@v7
              with:
                  name: coverage-report
                  path: "**/coverage.cobertura.xml"

            - name: Upload coverage to Codecov
              uses: codecov/codecov-action@v6
              with:
                  token: ${{ secrets.CODECOV_TOKEN }}
                  files: "**/coverage.cobertura.xml"
                  fail_ci_if_error: false

    publish:
        needs: build-and-test
        if: github.ref == 'refs/heads/main' && github.event_name == 'push'
        runs-on: windows-latest
        env:
            MSBUILDDISABLENODEREUSE: 1
            FORCE_JAVASCRIPT_ACTIONS_TO_NODE24: true

        steps:
            - uses: actions/checkout@v6

            - uses: actions/setup-dotnet@v5
              with:
                  dotnet-version: "10.0.x"

            - name: Publish (self-contained)
              run: >-
                  dotnet publish src/RegiLattice.GUI/RegiLattice.GUI.csproj
                  -c Release -r win-x64 --self-contained true
                  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

            - name: Upload artifact
              uses: actions/upload-artifact@v7
              with:
                  name: RegiLattice-win-x64
                  path: src/RegiLattice.GUI/bin/Release/net10.0-windows/win-x64/publish/
```

## Commit Message Convention (Conventional Commits)

```
<type>(<scope>): <subject>

[optional body]

[optional footer(s)]
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

## NuGet Packages (keep up-to-date)

| Package                   | Current | Purpose                                    |
| ------------------------- | ------- | ------------------------------------------ |
| System.Management         | 10.0.5  | WMI queries (CorporateGuard, HardwareInfo) |
| Microsoft.NET.Test.Sdk    | 17.14.1 | Test host                                  |
| xunit                     | 2.9.3   | Test framework                             |
| xunit.runner.visualstudio | 2.8.2   | VS test adapter                            |
| coverlet.collector        | 6.0.4   | Code coverage                              |

## Release Workflow

1. Bump version in `.csproj` files + `CHANGELOG.md`
2. Commit: `chore: bump version to v2.x.y`
3. Tag: `git tag v2.x.y`
4. Push tag: `git push --tags`
5. GitHub Actions builds release artifact
6. Create GitHub Release with artifact attached

## Post-Release Verification (MANDATORY after every tag push)

> **This is a standing rule. Do NOT skip post-release verification.**

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
- `RegiLattice-vX.Y.Z-win-x64.msi` — optional MSI installer
- `RegiLattice-vX.Y.Z-win-x64.msix` — optional MSIX package

**If release.yml failed:**
```powershell
# Re-trigger by deleting and re-pushing the tag:
git tag -d vX.Y.Z
git push origin :refs/tags/vX.Y.Z
git tag vX.Y.Z
git push --tags
```

**If the failure needs tracking:** Open or update a `ci-failure` issue manually, then close it once the next run is green.
Use: `gh issue close <N> --repo RajwanYair/RegiLattice --comment "CI now green as of <commit>."`

---

## CI Best Practices

- **NuGet cache**: key on `.csproj` hash to invalidate on dependency changes
- **Build once, test without `--no-build`**: omit `--no-build` to let each test step do a fast incremental build — safer than relying on a prior Build step to produce all DLLs, especially for GUI.Tests which requires Windows SDK runtime packs
- **Per-project test runs — MANDATORY**: NEVER use `dotnet test RegiLattice.sln`. Always run each
  test project individually (Core → CLI → GUI). `dotnet test RegiLattice.sln` spawns Core.Tests and
  GUI.Tests concurrently as separate processes, both writing to the same `compliance-history.json`
  file, causing non-deterministic failures. See `tests/.runsettings` MaxCpuCount note.
- **`MSBUILDDISABLENODEREUSE: 1`**: Set at job level in every CI job — prevents MSBuild worker nodes from holding file locks across builds on the OneDrive-hosted workspace (MSB3492)
- **`FORCE_JAVASCRIPT_ACTIONS_TO_NODE24: true`**: Set at job level — ensures JS-based actions (checkout, cache, codecov) run on Node 24 without deprecation warnings
- **`paths-ignore` for docs-only changes**: Add to `on.push` to avoid a full build/test run when only `.md`, `.svg`, `.txt`, or instruction files changed
- **Codecov**: use `codecov-action@v6`; set `fail_ci_if_error: false`
- **Windows-only**: no matrix needed — single `windows-latest` runner
- **Self-contained publish**: `-r win-x64 --self-contained true -p:PublishSingleFile=true`
- **Stryker mutation testing**: runs from `src/RegiLattice.Core/` directory with `STRYKER_BUILD=1`; explicit `<TargetFramework>` required in Core and Core.Tests .csproj files for Buildalyzer compatibility; move to **weekly schedule** (cron) to avoid adding ~15 min per push
- **`dotnet build` verbosity**: NEVER use `-q`/`--verbosity quiet` — causes question-build aborts; use no flag or `--verbosity minimal`
- **Action version verification**: verify every action version exists before committing — a non-existent tag silently fails the step
