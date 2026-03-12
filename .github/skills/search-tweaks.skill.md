---
mode: agent
description: "Search for tweaks across all 72 categories by keyword, tag, scope, or registry path"
tools: ["read_file", "grep_search", "semantic_search"]
---

# Search Tweaks — RegiLattice Skill

You are an expert at finding tweaks in the RegiLattice registry tweak toolkit.

## Context

- RegiLattice has 1,981+ tweaks across 72 categories in `src/RegiLattice.Core/Tweaks/`
- Each tweak has: Id, Label, Category, Description, Tags, RegistryKeys, Scope
- Tweak IDs follow `{category_slug}-{descriptive-name}` naming

## Search Strategy

When the user asks to find a tweak:

1. **By keyword**: Search `src/RegiLattice.Core/Tweaks/*.cs` for the keyword in Label, Description, or Tags
2. **By registry path**: Search for the exact registry path across all modules
3. **By tag**: Search for the tag string in Tags arrays
4. **By scope**: Filter HKCU (User), HKLM (Machine), or Both
5. **By category**: Look in the specific category file

## Search Commands

```powershell
# Search all tweaks for a keyword
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern "keyword" -Context 2,2

# Search by registry path
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern "AllowTelemetry" -Context 5,5

# List all categories
Get-ChildItem "src\RegiLattice.Core\Tweaks\*.cs" | Select-Object -ExpandProperty BaseName
```

## Cross-Category Search

The `TweakEngine.Search(query)` method searches across ALL categories matching:
- Tweak ID
- Label
- Category name
- Description
- Tags

The `TweakEngine.Filter()` method supports multi-criteria filtering:
- `corpSafe` — corporate-safe tweaks only
- `needsAdmin` — admin-required tweaks
- `scope` — User, Machine, or Both
- `category` — specific category
- `minBuild` — minimum Windows build
- `query` — text search

## Output Format

Return results as a table:
| ID | Label | Category | Scope | Admin | Tags |
|----|-------|----------|-------|-------|------|

## Category Slugs

See `.github/copilot-instructions.md` for the full 72-category slug mapping.
