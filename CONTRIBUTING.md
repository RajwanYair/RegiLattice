# Contributing to RegiLattice

For the full contribution guide, see **[docs/CONTRIBUTING.md](docs/CONTRIBUTING.md)**.

## Quick Reference

| Task | Command |
|------|---------|
| Build (Debug) | `dotnet build RegiLattice.sln -c Debug` |
| Build (Release) | `dotnet build RegiLattice.sln -c Release` |
| Test | `dotnet test RegiLattice.sln` |
| Run GUI | `dotnet run --project src/RegiLattice.GUI` |
| Run CLI | `dotnet run --project src/RegiLattice.CLI -- --list` |

## Adding a New Tweak

1. Create or edit a module in `src/RegiLattice.Core/Tweaks/`.
2. Define tweaks using the declarative `RegOp` pattern (preferred) or `Action`/`Func` delegates.
3. Export them as `public static List<TweakDef> Tweaks { get; }`.
4. Register the module in `TweakEngine.RegisterBuiltins()`.
5. Run `dotnet test` to verify.

## Commit Messages

Use [Conventional Commits](https://www.conventionalcommits.org/) format:
`type(scope): description` (e.g., `feat(tweaks): add bluetooth-disable-handsfree`).
