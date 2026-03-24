---
applyTo: "**/*.yml,**/*.yaml,.github/**"
---

# CI/CD and GitHub Actions Instructions

## ⛔ STANDING RULE: Release EXEs on Every Version Bump

> **This rule applies to EVERY session without exception.**

Every time a version is bumped (PATCH, MINOR, or MAJOR), the tag **must be pushed immediately**
after committing. The `git push --tags` triggers the `release.yml` GitHub Actions workflow which:

1. Builds a self-contained `RegiLattice.exe` (win-x64, single file, GUI)
2. Builds a self-contained `RegiLatticeCLI.exe` (win-x64, single file, CLI)
3. Builds the MSI installer via WiX
4. Publishes all three as assets on a new GitHub Release

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

## Standard .NET CI Workflow Pattern

```yaml
name: CI

on:
    push:
        branches: [main]
    pull_request:
        branches: [main]

permissions:
    contents: read

jobs:
    build-and-test:
        runs-on: windows-latest

        steps:
            - uses: actions/checkout@v4

            - uses: actions/setup-dotnet@v4
              with:
                  dotnet-version: "10.0.x"

            - name: Cache NuGet
              uses: actions/cache@v4
              with:
                  path: ~/.nuget/packages
                  key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
                  restore-keys: ${{ runner.os }}-nuget-

            - name: Restore
              run: dotnet restore RegiLattice.sln

            - name: Build
              run: dotnet build RegiLattice.sln -c Release --no-restore

            - name: Test with coverage
              run: dotnet test RegiLattice.sln -c Release --no-build --collect:"XPlat Code Coverage" --logger "console;verbosity=normal"

            - name: Upload coverage artifact
              uses: actions/upload-artifact@v4
              with:
                  name: coverage-report
                  path: "**/coverage.cobertura.xml"

            - name: Upload coverage to Codecov
              uses: codecov/codecov-action@v5
              with:
                  token: ${{ secrets.CODECOV_TOKEN }}
                  files: "**/coverage.cobertura.xml"
                  fail_ci_if_error: false

    publish:
        needs: build-and-test
        if: github.ref == 'refs/heads/main' && github.event_name == 'push'
        runs-on: windows-latest

        steps:
            - uses: actions/checkout@v4

            - uses: actions/setup-dotnet@v4
              with:
                  dotnet-version: "10.0.x"

            - name: Publish (self-contained)
              run: >-
                  dotnet publish src/RegiLattice.GUI/RegiLattice.GUI.csproj
                  -c Release -r win-x64 --self-contained true
                  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

            - name: Upload artifact
              uses: actions/upload-artifact@v4
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
| System.Management         | 9.0.3   | WMI queries (CorporateGuard, HardwareInfo) |
| Microsoft.NET.Test.Sdk    | 17.11.1 | Test host                                  |
| xunit                     | 2.9.2   | Test framework                             |
| xunit.runner.visualstudio | 2.8.2   | VS test adapter                            |
| coverlet.collector        | 6.0.2   | Code coverage                              |

## Release Workflow

1. Bump version in `.csproj` files + `CHANGELOG.md`
2. Commit: `chore: bump version to v2.x.y`
3. Tag: `git tag v2.x.y`
4. Push tag: `git push --tags`
5. GitHub Actions builds release artifact
6. Create GitHub Release with artifact attached

## CI Best Practices

- **NuGet cache**: key on `.csproj` hash to invalidate on dependency changes
- **Build once, test from build**: use `--no-build` in test step after `dotnet build`
- **Codecov**: use `codecov-action@v5`; set `fail_ci_if_error: false`
- **Windows-only**: no matrix needed — single `windows-latest` runner
- **Self-contained publish**: `-r win-x64 --self-contained true -p:PublishSingleFile=true`
