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

- Added/modified `src/RegiLattice.Core/Tweaks/xxx.cs`: ...
- ...

## Checklist

- [ ] `dotnet build RegiLattice.sln` succeeds with no warnings
- [ ] `dotnet test` passes (all 203+ tests green)
- [ ] New tweak IDs are globally unique kebab-case
- [ ] New tweaks have `ApplyOps`, `RemoveOps`, `DetectOps` (or custom delegates)
- [ ] `CorpSafe` and `NeedsAdmin` flags are set correctly
- [ ] All new classes are `sealed` unless inheritance is explicit
- [ ] `#nullable enable` — no nullable warnings
- [ ] `IReadOnlyList<T>` for all public collection properties
- [ ] No Unicode confusables (en-dashes, smart quotes) — ASCII only
- [ ] Commit messages follow Conventional Commits format

## Testing

How did you test this? (e.g., ran xUnit tests, tested on Windows 11 build XXXXX)

## Screenshots (if applicable)

GUI changes, status badge behaviour, etc.
