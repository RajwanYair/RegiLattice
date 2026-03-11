# API Reference

> Reference for the RegiLattice C# public API.
> Last verified: 2025-07-20 · v2.0.0

---

## RegiLattice.Core — Models

### `TweakDef`

`csharp
public sealed class TweakDef
{
    public required string Id { get; init; }
    public required string Label { get; init; }
    public required string Category { get; init; }
    public string Description { get; init; } = "";
    public IReadOnlyList<string> Tags { get; init; } = [];
    public bool NeedsAdmin { get; init; } = true;
    public bool CorpSafe { get; init; }
    public int MinBuild { get; init; }
    public IReadOnlyList<string> RegistryKeys { get; init; } = [];
    public IReadOnlyList<string> DependsOn { get; init; } = [];
    public string SideEffects { get; init; } = "";
    public string SourceUrl { get; init; } = "";

    // Declarative RegOp pattern (~95% of tweaks)
    public IReadOnlyList<RegOp> ApplyOps { get; init; } = [];
    public IReadOnlyList<RegOp> RemoveOps { get; init; } = [];
    public IReadOnlyList<RegOp> DetectOps { get; init; } = [];

    // Custom logic delegates (~5% of tweaks)
    public Action<bool>? ApplyAction { get; init; }
    public Action<bool>? RemoveAction { get; init; }
    public Func<bool>? DetectAction { get; init; }

    // Computed
    public TweakScope Scope => ComputeScope();
}
`

### `RegOp`

12 factory methods for declarative registry operations:

| Factory | Registry Type | Purpose |
|---------|--------------|---------|
| `RegOp.SetDword(path, name, value)` | REG_DWORD | Set 32-bit integer |
| `RegOp.SetString(path, name, value)` | REG_SZ | Set string value |
| `RegOp.SetExpandString(path, name, value)` | REG_EXPAND_SZ | Set expandable string |
| `RegOp.SetQword(path, name, value)` | REG_QWORD | Set 64-bit integer |
| `RegOp.SetBinary(path, name, bytes)` | REG_BINARY | Set binary data |
| `RegOp.SetMultiSz(path, name, strings)` | REG_MULTI_SZ | Set multi-string |
| `RegOp.DeleteValue(path, name)` | — | Delete a named value |
| `RegOp.DeleteTree(path)` | — | Delete entire key tree |
| `RegOp.CheckDword(path, name, expected)` | Detection | Check DWORD equals expected |
| `RegOp.CheckString(path, name, expected)` | Detection | Check string equals expected |
| `RegOp.CheckMissing(path, name)` | Detection | Check value does not exist |
| `RegOp.CheckKeyMissing(path)` | Detection | Check key does not exist |

### `TweakScope` (enum)

| Value | Meaning |
|-------|---------|
| `User` | HKCU only — no admin required |
| `Machine` | HKLM only — admin required |
| `Both` | Touches both hives |

### `TweakResult` (enum)

`Applied`, `Removed`, `NotApplied`, `Unknown`, `Unchanged`, `SkippedCorp`, `SkippedAdmin`, `SkippedBuild`, `Error`

### `ProfileDef`

`csharp
public sealed class ProfileDef
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyList<string> ApplyCategories { get; init; }
    public IReadOnlyList<string> SkipCategories { get; init; } = [];
}
`

---

## RegiLattice.Core — TweakEngine

Central tweak manager. All methods are on the `TweakEngine` class.

### Registration

| Method | Signature | Description |
|--------|-----------|-------------|
| `Register` | `(TweakDef td)` | Register a single tweak (throws on duplicate ID) |
| `RegisterBuiltins` | `()` | Register all 68 built-in tweak modules |
| `AllTweaks` | `() -> IReadOnlyList<TweakDef>` | All registered tweaks |
| `GetTweak` | `(string id) -> TweakDef?` | Look up by ID |

### Categories & Grouping

| Method | Signature | Description |
|--------|-----------|-------------|
| `Categories` | `() -> IReadOnlyList<string>` | Sorted category names |
| `TweaksByCategory` | `() -> IReadOnlyDictionary<string, IReadOnlyList<TweakDef>>` | Grouped by category |
| `TweaksByIds` | `(IEnumerable<string> ids) -> IReadOnlyList<TweakDef>` | Resolve IDs |
| `TweaksByTag` | `(string tag) -> IReadOnlyList<TweakDef>` | Filter by tag |
| `TweaksByScope` | `(TweakScope scope) -> IReadOnlyList<TweakDef>` | Filter by scope |
| `GetScope` | `(TweakDef td) -> TweakScope` | Compute scope for a tweak |

### Search & Filter

| Method | Signature | Description |
|--------|-----------|-------------|
| `Search` | `(string query) -> IReadOnlyList<TweakDef>` | Full-text search (ID, label, tags, description) |
| `Filter` | `(bool? corpSafe, bool? needsAdmin, TweakScope? scope, string? category, int? minBuild, string? query) -> IReadOnlyList<TweakDef>` | Multi-criterion filter |

### Status Detection

| Method | Signature | Description |
|--------|-----------|-------------|
| `DetectStatus` | `(TweakDef td) -> TweakResult` | Detect current state of a tweak |
| `StatusMap` | `(bool parallel, IEnumerable<string>? ids) -> IReadOnlyDictionary<string, TweakResult>` | Status of tweaks (optionally parallel) |

### Apply / Remove

| Method | Signature | Description |
|--------|-----------|-------------|
| `Apply` | `(TweakDef td) -> TweakResult` | Apply a single tweak |
| `Remove` | `(TweakDef td) -> TweakResult` | Remove/revert a single tweak |
| `ApplyBatch` | `(IEnumerable<string> ids) -> IReadOnlyDictionary<string, TweakResult>` | Batch apply by IDs |
| `RemoveBatch` | `(IEnumerable<string> ids) -> IReadOnlyDictionary<string, TweakResult>` | Batch remove by IDs |

### Profiles

| Method | Signature | Description |
|--------|-----------|-------------|
| `Profiles` | (static) `IReadOnlyList<ProfileDef>` | 5 built-in profiles |
| `GetProfile` | `(string name) -> ProfileDef?` | Look up profile by name |
| `TweaksForProfile` | `(string name) -> IReadOnlyList<TweakDef>` | Tweaks for a profile |
| `ApplyProfile` | `(string name) -> IReadOnlyDictionary<string, TweakResult>` | Apply all tweaks in a profile |

### Snapshots

| Method | Signature | Description |
|--------|-----------|-------------|
| `SaveSnapshot` | `(string path)` | Save current state to JSON |
| `LoadSnapshot` | `(string path) -> ...` | Load snapshot data |
| `RestoreSnapshot` | `(string path) -> IReadOnlyDictionary<string, TweakResult>` | Restore from snapshot |

### Statistics

| Method | Signature | Description |
|--------|-----------|-------------|
| `CategoryCounts` | `() -> IReadOnlyDictionary<string, int>` | Tweak count per category |
| `ScopeCounts` | `() -> IReadOnlyDictionary<TweakScope, int>` | Counts by scope |
| `ExportJson` | `(string path)` | Export all tweaks as JSON |
| `WindowsBuild` | `() -> int` | Current Windows build number |

---

## RegiLattice.Core — RegistrySession

Windows registry wrapper with backup, logging, and dry-run support.

### Write Operations

| Method | Parameters | Description |
|--------|-----------|-------------|
| `SetDword` | `(string path, string name, int value)` | REG_DWORD |
| `SetString` | `(string path, string name, string value)` | REG_SZ |
| `SetExpandString` | `(string path, string name, string value)` | REG_EXPAND_SZ |
| `SetQword` | `(string path, string name, long value)` | REG_QWORD |
| `SetBinary` | `(string path, string name, byte[] data)` | REG_BINARY |
| `SetMultiSz` | `(string path, string name, string[] values)` | REG_MULTI_SZ |
| `SetValue` | `(string path, string name, object value, RegistryValueKind kind)` | Any type |
| `DeleteValue` | `(string path, string name)` | Delete a single value |
| `DeleteTree` | `(string path)` | Delete an entire key tree |

### Read Operations

| Method | Returns | Description |
|--------|---------|-------------|
| `ReadDword` | `int?` | Read REG_DWORD |
| `ReadString` | `string?` | Read REG_SZ |
| `ReadQword` | `long?` | Read REG_QWORD |
| `ReadBinary` | `byte[]?` | Read REG_BINARY |
| `ReadMultiSz` | `string[]?` | Read REG_MULTI_SZ |
| `ReadValue` | `object?` | Read any type |

### Query Operations

| Method | Returns | Description |
|--------|---------|-------------|
| `KeyExists` | `bool` | Check if a registry key exists |
| `ValueExists` | `bool` | Check if a specific value exists |
| `ListSubKeys` | `string[]` | List child keys |
| `ListValueNames` | `string[]` | List value names in a key |

### Execution

| Method | Parameters | Description |
|--------|-----------|-------------|
| `Execute` | `(IReadOnlyList<RegOp> ops)` | Execute a list of write RegOps |
| `Evaluate` | `(IReadOnlyList<RegOp> ops) -> bool` | Evaluate detection RegOps (all must match) |

### Backup & Logging

| Method | Parameters | Description |
|--------|-----------|-------------|
| `Backup` | `(IEnumerable<string> keys, string label)` | JSON backup to `%LOCALAPPDATA%\RegiLattice\backups\` |
| `DryRun` | `bool` property | When true, no registry writes are performed |
| `DryOps` | `IReadOnlyList<RegOp>` | Operations that would have been executed in dry-run mode |
| `Log` | `(string message)` | Append to session log |
| `WriteLog` | `()` | Flush log to disk |

---

## RegiLattice.Core — Services

### CorporateGuard

| Method | Returns | Description |
|--------|---------|-------------|
| `IsCorporateNetwork` | `bool` | Detect corporate environment |
| `Status` | `CorporateStatus` | Detailed detection results |
| `IsGpoManaged` | `bool` | Check for Group Policy management |
| `ClearCache` | `void` | Reset cached detection results |

### Elevation

| Method | Returns | Description |
|--------|---------|-------------|
| `IsAdmin` | `bool` | Check if running as administrator |
| `RequestElevation` | `void` | Request UAC elevation |

### HardwareInfo

| Method | Returns | Description |
|--------|---------|-------------|
| `Detect` | `HardwareProfile` | Full hardware detection |
| `Summary` | `string` | Human-readable hardware summary |
| `SuggestProfile` | `string` | Recommended profile based on hardware |

### Analytics

| Method | Description |
|--------|-------------|
| `RecordApply(string id)` | Record a tweak application |
| `RecordRemove(string id)` | Record a tweak removal |
| `GetStats()` | Usage statistics |

### Ratings

| Method | Description |
|--------|-------------|
| `Rate(string id, int stars)` | Rate a tweak (1-5) |
| `GetRating(string id)` | Get rating for a tweak |
| `AllRatings()` | All ratings |
| `TopRated(int n)` | Top N rated tweaks |

### Locale

| Method | Description |
|--------|-------------|
| `Translate(string key)` | Look up i18n string |
| `SetLocale(string locale)` | Change current locale |
| `CurrentLocale()` | Get current locale name |

### AppConfig

| Method | Description |
|--------|-------------|
| `Load()` | Load configuration from disk |
| `ForceCorpGuard` | bool — override corporate guard |
| `Theme` | string — default theme name |
| `Locale` | string — default locale |
