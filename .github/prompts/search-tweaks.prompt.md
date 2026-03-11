---
mode: agent
description: "Search for tweaks across all 72 categories by keyword, registry path, tag, or scope"
---

# Search Tweaks

Search for registry tweaks across all categories in the RegiLattice engine.

## Input

Search query: `${input:query}`
Scope filter (optional): `${input:scope:all}` (all / user / machine / both)
Category filter (optional): `${input:category}`

## How to Search

1. Search across `src/RegiLattice.Core/Tweaks/*.cs` files for the query
2. Match against: Id, Label, Description, Tags, RegistryKeys
3. Apply scope filter if specified
4. Group results by category

## Output Format

| # | ID | Label | Category | Scope | Admin | Description |
|---|-----|-------|----------|-------|-------|-------------|
| 1 | priv-disable-telemetry | Disable Telemetry | Privacy | MACHINE | Yes | ... |

## CLI Equivalent

```powershell
dotnet run --project src/RegiLattice.CLI -- --search "${input:query}"
```
