## Description

<!-- Brief description of what this PR does and why -->

## Type of Change

- [ ] New tweak(s)
- [ ] Bug fix (non-breaking)
- [ ] Feature (non-tweak, non-breaking)
- [ ] Breaking change (changes existing behaviour)
- [ ] Refactor / code quality
- [ ] Documentation
- [ ] CI / build improvement
- [ ] Performance improvement

## Changes Made

- Added/modified `src/RegiLattice.Core/Tweaks/xxx.cs`: ...
- ...

## Testing

- [ ] Built successfully: `dotnet build RegiLattice.sln -c Release -m:1` (**0 errors, 0 warnings**)
- [ ] All tests pass: `dotnet test RegiLattice.sln --settings tests/.runsettings` (**0 failures**)
- [ ] Validation clean: `dotnet run --project src/RegiLattice.CLI -- --validate` (**0 errors**)
- [ ] Tested on Windows 10 / 11 (build: _______)
- [ ] Dry-run tested: `--dry-run` flag produces expected output
- [ ] Apply + Remove cycles verified manually (tweak is fully reversible)

## Checklist

- [ ] New tweak IDs are globally unique kebab-case (`Select-String` scan confirms no collision)
- [ ] New tweaks have `ApplyOps`, `RemoveOps`, `DetectOps` (or custom delegates with `HasOperations = true`)
- [ ] `ImpactScore` (1–5) and `SafetyRating` (1–5) set explicitly on all new `TweakDef` entries
- [ ] `CorpSafe` and `NeedsAdmin` flags are set correctly
- [ ] All new classes are `sealed` unless inheritance is explicitly needed
- [ ] `#nullable enable` — no nullable warnings introduced
- [ ] `IReadOnlyList<T>` used for all public collection properties
- [ ] No duplicate registry ops (`PATH\ValueName`) across modules
- [ ] No Unicode confusables (en-dashes, smart quotes) — ASCII only in code
- [ ] Commit messages follow Conventional Commits format (`type(scope): description`)
- [ ] CHANGELOG.md updated if this is a user-facing change

## Performance Impact

- [ ] No performance impact expected
- [ ] Benchmarked — results: ___

## Screenshots (if applicable)

<!-- GUI changes, status badge behaviour, theme screenshots, etc. -->

## Related Issues

Closes #<!-- issue number -->
