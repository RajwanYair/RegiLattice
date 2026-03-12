# RegiLattice — Plugin Marketplace Design

> **Status**: Plan only — not yet implemented.
> **Target**: v4.0.0+
> **Author**: RajwanYair
> **Created**: 2025-07-21

---

## Goals

1. Allow users to install community-authored **Tweak Packs** without recompiling
2. Allow publishing Tweak Packs to a central GitHub-hosted index
3. Keep the security model tight — no arbitrary code execution from untrusted sources
4. Minimal impact on the existing codebase (additive, not refactoring)

---

## Architecture Overview

```
┌────────────────────────────────────────────────────┐
│                  RegiLattice GUI / CLI              │
│                                                     │
│  ┌─────────────┐   ┌───────────┐   ┌────────────┐ │
│  │ TweakEngine  │◄──│ PluginMgr │──►│ IndexCache │ │
│  │ (built-in)   │   │           │   │ (local)    │ │
│  └─────────────┘   └─────┬─────┘   └────────────┘ │
│                           │                         │
└───────────────────────────┼─────────────────────────┘
                            │ HTTPS (read-only)
                            ▼
              ┌──────────────────────────┐
              │  GitHub Repository Index │
              │  RajwanYair/regilattice- │
              │  marketplace             │
              │  ├── index.json          │
              │  └── packs/              │
              │      ├── gaming-extra/   │
              │      ├── developer/      │
              │      └── privacy-pro/    │
              └──────────────────────────┘
```

### Key Design Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Pack format | JSON (declarative TweakDefs) | No .dll loading = no code execution risk |
| Distribution | GitHub repo + raw.githubusercontent | Free, versioned, no server infrastructure |
| Index | Single `index.json` at repo root | Simple, cacheable, no database needed |
| Install location | `%LOCALAPPDATA%\RegiLattice\packs\` | User-scoped, no admin for install |
| Verification | SHA-256 hash in index | Integrity check, not full signing (v1) |

---

## Tweak Pack Format

A Tweak Pack is a single `.json` file containing an array of declarative tweak definitions.
Only the **declarative RegOp pattern** is supported — no custom `ApplyAction` / `DetectAction`.

### `pack.json` Schema

```json
{
  "name": "gaming-extra",
  "displayName": "Gaming Extra Tweaks",
  "version": "1.0.0",
  "author": "RajwanYair",
  "description": "25 additional gaming performance tweaks for competitive FPS players.",
  "minRegiLatticeVersion": "3.1.0",
  "minWindowsBuild": 22621,
  "categories": ["Gaming Extra"],
  "tags": ["gaming", "fps", "performance", "latency"],
  "tweaks": [
    {
      "id": "gx-disable-game-bar-recording",
      "label": "Disable Game Bar Recording",
      "category": "Gaming Extra",
      "description": "Prevents Game Bar from auto-recording gameplay clips.",
      "tags": ["gaming", "performance"],
      "needsAdmin": true,
      "corpSafe": false,
      "minBuild": 22621,
      "applyOps": [
        { "kind": "SetDword", "path": "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\GameBar", "name": "AutoGameModeEnabled", "dwordValue": 0 }
      ],
      "removeOps": [
        { "kind": "DeleteValue", "path": "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\GameBar", "name": "AutoGameModeEnabled" }
      ],
      "detectOps": [
        { "kind": "CheckDword", "path": "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\GameBar", "name": "AutoGameModeEnabled", "dwordValue": 0 }
      ]
    }
  ]
}
```

### Supported RegOp Kinds in JSON

| Kind | Required Fields |
|------|-----------------|
| `SetDword` | `path`, `name`, `dwordValue` |
| `SetString` | `path`, `name`, `stringValue` |
| `SetExpandString` | `path`, `name`, `stringValue` |
| `SetQword` | `path`, `name`, `qwordValue` |
| `SetBinary` | `path`, `name`, `binaryValue` (base64) |
| `SetMultiSz` | `path`, `name`, `multiSzValue` (string[]) |
| `DeleteValue` | `path`, `name` |
| `DeleteTree` | `path` |
| `CheckDword` | `path`, `name`, `dwordValue` |
| `CheckString` | `path`, `name`, `stringValue` |
| `CheckMissing` | `path`, `name` |
| `CheckKeyMissing` | `path` |

---

## Index Repository Structure

```
RajwanYair/regilattice-marketplace/
├── index.json              # Master index of all packs
├── README.md               # Community guidelines
├── CONTRIBUTING.md          # How to submit a pack
├── schema/
│   ├── pack.schema.json    # JSON Schema for pack validation
│   └── index.schema.json   # JSON Schema for index
└── packs/
    ├── gaming-extra/
    │   ├── pack.json       # The pack definition
    │   └── README.md       # Pack documentation
    ├── developer/
    │   ├── pack.json
    │   └── README.md
    └── privacy-pro/
        ├── pack.json
        └── README.md
```

### `index.json` Schema

```json
{
  "version": 1,
  "lastUpdated": "2025-07-21T00:00:00Z",
  "packs": [
    {
      "name": "gaming-extra",
      "displayName": "Gaming Extra Tweaks",
      "version": "1.0.0",
      "author": "RajwanYair",
      "description": "25 additional gaming performance tweaks.",
      "tweakCount": 25,
      "categories": ["Gaming Extra"],
      "tags": ["gaming", "fps"],
      "sha256": "abc123...",
      "downloadUrl": "https://raw.githubusercontent.com/RajwanYair/regilattice-marketplace/main/packs/gaming-extra/pack.json",
      "minRegiLatticeVersion": "3.1.0"
    }
  ]
}
```

---

## Implementation Plan

### Phase 1 — Core Plugin Infrastructure (`RegiLattice.Core`)

**New files:**

| File | Purpose |
|------|---------|
| `src/RegiLattice.Core/Plugins/PackDef.cs` | Pack metadata model (sealed record) |
| `src/RegiLattice.Core/Plugins/PackLoader.cs` | JSON → TweakDef[] deserialiser + validator |
| `src/RegiLattice.Core/Plugins/PackIndex.cs` | Index model + fetch from GitHub |
| `src/RegiLattice.Core/Plugins/PackManager.cs` | Install / uninstall / list / update packs |

**Changes to existing:**

| File | Change |
|------|--------|
| `TweakEngine.cs` | Add `RegisterPack(PackDef)` method |
| `TweakDef.cs` | Add `PackSource` property (null = built-in) |

**Key classes:**

```csharp
// Models
public sealed record PackDef(
    string Name,
    string DisplayName,
    string Version,
    string Author,
    string Description,
    int TweakCount,
    IReadOnlyList<string> Categories,
    IReadOnlyList<string> Tags,
    string Sha256,
    string DownloadUrl,
    string MinRegiLatticeVersion);

public sealed record PackIndex(
    int Version,
    DateTime LastUpdated,
    IReadOnlyList<PackDef> Packs);

// Core manager
public sealed class PackManager
{
    private readonly string _packsDir;  // %LOCALAPPDATA%\RegiLattice\packs\

    public IReadOnlyList<PackDef> InstalledPacks();
    public async Task<PackIndex> FetchIndexAsync(CancellationToken ct);
    public async Task<IReadOnlyList<TweakDef>> InstallPackAsync(string name, CancellationToken ct);
    public bool UninstallPack(string name);
    public async Task<IReadOnlyList<TweakDef>> UpdatePackAsync(string name, CancellationToken ct);
}

// Loader
public static class PackLoader
{
    public static IReadOnlyList<TweakDef> LoadFromJson(string json, string packName);
    public static bool ValidatePackJson(string json, out IReadOnlyList<string> errors);
}
```

### Phase 2 — CLI Integration

**New CLI commands:**

| Command | Description |
|---------|-------------|
| `--marketplace list` | Show available packs from index |
| `--marketplace search <query>` | Search packs by name/tag |
| `--marketplace install <name>` | Download and install a pack |
| `--marketplace update <name>` | Update an installed pack |
| `--marketplace uninstall <name>` | Remove an installed pack |
| `--marketplace installed` | List locally installed packs |
| `--marketplace info <name>` | Show pack details |

### Phase 3 — GUI Integration

**New UI elements:**

| Element | Location | Description |
|---------|----------|-------------|
| Marketplace tab | Main sidebar | Browse/search/install packs |
| Pack detail panel | Marketplace tab | Name, author, version, tweak count, install button |
| Installed badge | Category tree | Show (plugin) badge next to pack categories |
| Update indicator | Status bar | "2 pack updates available" notification |

**New dialog: `MarketplaceDialog.cs`**

- Tabbed: Browse | Installed | Updates
- Browse: search bar + pack cards (name, author, stars, install button)
- Installed: list with uninstall/update buttons
- Updates: one-click "Update All"

### Phase 4 — Community & Governance

| Task | Description |
|------|-------------|
| JSON Schema validation | CI action validates all pack.json against schema |
| SHA-256 integrity | Client verifies hash before loading any pack |
| Author verification | GitHub account linked, display verified badge |
| Review process | PR-based submission, manual review before merge to index |
| Guidelines | Max 100 tweaks per pack, no HKLM writes without justification |
| Reporting | Flag mechanism for malicious packs |

---

## Security Model

### Threat vectors & mitigations

| Threat | Mitigation |
|--------|------------|
| Malicious registry writes | JSON-only — no .dll execution, no Process.Start |
| Pack tampering in transit | SHA-256 hash verification from index |
| Path traversal in registry paths | Validate all paths start with `HKEY_` prefix |
| Denial of service (huge pack) | Max 100 tweaks per pack, max 1MB file size |
| Index poisoning | HTTPS-only fetch, pinned to specific GitHub repo |
| Stale/broken tweaks | `DetectOps` required for all pack tweaks |

### Validation rules for pack tweaks

1. All IDs must be prefixed with pack name: `{pack}-{tweak-name}`
2. All registry paths must start with `HKEY_CURRENT_USER\` or `HKEY_LOCAL_MACHINE\`
3. `ApplyOps`, `RemoveOps`, and `DetectOps` must all be non-empty
4. No duplicate IDs (checked against built-in + all installed packs)
5. `Category` must match declared pack categories
6. `minBuild` must be reasonable (>= 19041 for Win10 2004+)

---

## File Layout After Implementation

```
src/RegiLattice.Core/
├── Plugins/
│   ├── PackDef.cs           # Pack metadata record
│   ├── PackIndex.cs         # Index model + deserialisation
│   ├── PackLoader.cs        # JSON → TweakDef[] + validation
│   └── PackManager.cs       # Install/uninstall/update/list
tests/RegiLattice.Core.Tests/
├── PackLoaderTests.cs       # Pack JSON parsing, validation, edge cases
├── PackManagerTests.cs      # Install/uninstall with temp dir
```

---

## Estimated Effort

| Phase | Effort | Dependencies |
|-------|--------|--------------|
| Phase 1 — Core infrastructure | 2-3 sessions | None |
| Phase 2 — CLI commands | 1 session | Phase 1 |
| Phase 3 — GUI dialog | 2 sessions | Phase 1 |
| Phase 4 — Community repo | 1 session | Phase 1 |
| **Total** | **6-7 sessions** | |

---

## Open Questions

1. **Versioning**: Should packs follow semver independently, or track RegiLattice version?
   - **Recommendation**: Independent semver with `minRegiLatticeVersion` constraint.

2. **Custom categories**: Can packs create new categories, or only add to existing?
   - **Recommendation**: Packs can create new categories (prefixed with pack name).

3. **Offline mode**: Should packs work fully offline once installed?
   - **Recommendation**: Yes — installed packs are fully self-contained JSON.

4. **Rating system**: Should packs have a rating/review system?
   - **Recommendation**: Defer to v4.1. Start with download count only.

5. **Auto-update**: Should packs auto-update, or require manual action?
   - **Recommendation**: Notify only. User must explicitly update.
