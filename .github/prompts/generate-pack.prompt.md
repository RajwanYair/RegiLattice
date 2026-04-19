---
mode: agent
description: "Generate a new .rlpack.json Tweak Pack from a list of tweak IDs or a category name, and wire it into packs/index.json with a matching test."
---

# Generate Tweak Pack

Create a new official RegiLattice Tweak Pack in the `packs/` directory.

## Pack Details

Pack name (slug, e.g. gaming-essentials):
`${input:Pack slug — kebab-case, e.g. gaming-essentials}`

Pack display name:
`${input:Display name, e.g. Gaming Essentials}`

Source (category name OR comma-separated tweak IDs):
`${input:Category name (e.g. Gaming) or tweak IDs (e.g. game-disable-mouse-accel,game-high-performance-mode)}`

## Steps

### 1. Discover candidate tweaks

If a **category** was given:
```powershell
dotnet run --project src/RegiLattice.CLI -- --list --no-color |
    Where-Object { $_ -match '\[Gaming\]' }
```

If **tweak IDs** were given, validate they exist:
```powershell
dotnet run --project src/RegiLattice.CLI -- --validate --no-color
```

### 2. Check for duplicates in existing packs

```powershell
Select-String -Path packs/*.rlpack.json -Pattern '"tweak-id"'
```

### 3. Create the pack file

Create `packs/{slug}.rlpack.json`:

```json
{
    "name": "{Display Name}",
    "version": "1.0.0",
    "author": "RegiLattice",
    "description": "One sentence describing what this pack optimises.",
    "tags": ["tag1", "tag2"],
    "tweaks": [
        "tweak-id-1",
        "tweak-id-2"
    ]
}
```

Rules:
- All IDs must exist in `TweakEngine.AllTweaks()` — pack loader rejects unknown IDs
- File encoding: UTF-8, no BOM
- Minimum 5 tweaks; maximum 100 tweaks per pack

### 4. Update `packs/index.json`

Add an entry to the `"packs"` array:

```json
{
    "id": "{slug}",
    "file": "{slug}.rlpack.json",
    "name": "{Display Name}",
    "description": "Same one-sentence description.",
    "category": "Gaming|Privacy|Security|Developer|Accessibility|Enterprise",
    "tweakCount": {N}
}
```

Also update `"updated": "YYYY-MM-DD"` at the top level.

### 5. Write a test

Add an `[InlineData]` entry to `OfficialPackTests` in
`tests/RegiLattice.Core.Tests/PluginTests.cs`:

```csharp
[InlineData("{slug}.rlpack.json")]
```

### 6. Validate

```powershell
dotnet test tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj `
    --filter "OfficialPack_LoadsWithoutError" `
    --settings tests/.runsettings `
    --logger "console;verbosity=minimal"
```

### 7. Commit

```powershell
git add -A
git commit -m "feat(packs): add {slug} pack ({N} tweaks)"
```

## Validation Checklist

- [ ] Pack file is valid JSON and loads without errors (`PackLoader.LoadFromJson`)
- [ ] `tweakCount` in `index.json` matches actual count in `.rlpack.json`
- [ ] All tweak IDs exist in `RegisterBuiltins()`
- [ ] No tweak ID appears in two official packs (run `Select-String`)
- [ ] `OfficialPack_LoadsWithoutError` test passes
- [ ] `packs/index.json` `"updated"` date is today
