# Contributing to RegiLattice

Thank you for your interest in contributing!

## Getting Started

See the [Development Guide](docs/Development.md) for the full setup,
workflow, testing, and coding standards reference.

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

**Declarative pattern example** (preferred for simple registry tweaks):

```csharp
new TweakDef
{
    Id = "mycat-my-tweak",
    Label = "My Tweak Label",
    Category = "My Category",
    NeedsAdmin = false,
    CorpSafe = true,
    RegistryKeys = [Key],
    Description = "What this tweak does.",
    Tags = ["tag1", "tag2"],
    ApplyOps = [RegOp.SetDword(Key, "ValueName", 1)],
    RemoveOps = [RegOp.DeleteValue(Key, "ValueName")],
    DetectOps = [RegOp.CheckDword(Key, "ValueName", 1)],
}
```

## Commit Messages

Use [Conventional Commits](https://www.conventionalcommits.org/) format:
`type(scope): description` (e.g., `feat(tweaks): add bluetooth-disable-handsfree`).

## Corporate Safety

Tweaks with `CorpSafe = false` are blocked on corporate networks. If your tweak
only touches `HKCU` and poses no enterprise risk, set `CorpSafe = true`.
