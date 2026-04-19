---
name: perf-profiling
description: "Profile and diagnose performance issues in RegiLattice: slow searches, UI lag, TweakEngine bottlenecks, WinForms rendering delays, BenchmarkDotNet micro-benchmarks. Use when asked to profile, measure, or improve performance. Triggers on: 'slow', 'performance', 'profile', 'benchmark', 'lag', 'freeze', 'latency', 'too slow', 'optimize speed', 'BenchmarkDotNet'."
argument-hint: "Describe the slow operation (e.g. 'search response time', 'StatusMap on 7k tweaks', 'MainForm startup')"
---

# Performance Profiling — RegiLattice

## Profiling Strategy (pick by symptom)

| Symptom | Tool | Target |
|---------|------|--------|
| Slow search (>50ms keystroke) | `Stopwatch` + unit test | `TweakEngine.Search()` |
| `StatusMap` hang (>5s) | `Stopwatch` + task cancellation | `TweakEngine.StatusMap(parallel: true)` |
| WinForms UI freeze | `Task.Run` offload audit | `MainForm` background tasks |
| Startup latency | `Stopwatch` + `RegisterBuiltins` trace | `TweakEngine.RegisterBuiltins()` |
| Memory growth | VS Diagnostic Tools / dotMemory | Static collection growth |
| Allocation hot-path | BenchmarkDotNet with `[MemoryDiagnoser]` | High-frequency methods |
| GDI handle exhaustion | Task Manager GDI column | Paint handlers |

## BenchmarkDotNet (existing `tests/RegiLattice.Benchmarks/`)

The project already has a benchmarks executable. Run it directly:

```powershell
# Build benchmarks in Release mode first
dotnet build tests/RegiLattice.Benchmarks/RegiLattice.Benchmarks.csproj -c Release

# Run all benchmarks (outputs to BenchmarkDotNet.Artifacts/ — gitignored)
dotnet run --project tests/RegiLattice.Benchmarks --configuration Release -- --runtimes net10.0
```

### Adding a new benchmark

```csharp
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net100)]
public sealed class TweakSearchBenchmark
{
    private TweakEngine _engine = null!;

    [GlobalSetup]
    public void Setup()
    {
        _engine = new TweakEngine();
        _engine.RegisterBuiltins();
    }

    [Benchmark]
    public IReadOnlyList<TweakDef> Search_Telemetry() =>
        _engine.Search("telemetry");

    [Benchmark]
    public IReadOnlyList<TweakDef> Search_TwoWords() =>
        _engine.Search("disable network");
}
```

### Benchmark placement rules

- Benchmarks live in `tests/RegiLattice.Benchmarks/Benchmarks/`
- Each benchmark class is `sealed`, uses `[GlobalSetup]` for one-time init
- `[IterationSetup]` for per-iteration warm state
- Output directory `BenchmarkDotNet.Artifacts/` is in `.gitignore`

## Stopwatch-based profiling in xUnit tests

For regression detection, add wall-clock assertions (with mandatory budget comment):

```csharp
[Fact]
public void StatusMap_CompletesWithinBudget()
{
    // Budget: 5s for a dry-run StatusMap over 7,718 tweaks.
    // Baseline: ~2.1s on dev machine (i7-12700K). Raise if tweaks exceed 10,000.
    var engine = new TweakEngine(new RegistrySession { DryRun = true });
    engine.RegisterBuiltins();

    var sw = Stopwatch.StartNew();
    _ = engine.StatusMap(parallel: true);
    sw.Stop();

    Assert.True(sw.Elapsed.TotalSeconds < 5,
        $"StatusMap took {sw.Elapsed.TotalSeconds:F2}s (budget: 5s)");
}
```

## Key Performance Hotspots

### `TweakEngine.Search(query)` — O(n) over all tweaks

Pre-built `_TWEAKS_SEARCH_PAIRS` avoids per-call delegate invocations. If search adds a
new synonym pass, verify it does not regress the 150ms budget test:

```csharp
// TweakEngineBuiltinsTests.cs — existing test
Assert.True(sw.ElapsedMilliseconds < 150,
    $"Search took {sw.ElapsedMilliseconds}ms (budget: 150ms)");
```

### `TweakEngine.StatusMap(parallel: true)` — parallel registry reads

Registry reads can stall on Intune/SCCM machines. The parallel version uses
`Task.WhenAll` with `CancellationToken`. Test coverage for the parallel path exists in
`TweakDetectSweepTests.cs`.

### `TweakEngine.RegisterBuiltins()` — 195 modules, 7,718 tweaks

Startup cost is one-time. If regression occurs:
1. Profile with `Stopwatch` in `TweakEngineBuiltinsTests.RegisterBuiltins_CompletesInUnder3s`
2. Check for accidental `IsApplicable` delegate that triggers WMI/P-Invoke at startup

### WinForms Paint handlers — GDI handle exhaustion

All locally created GDI objects (`StringFormat`, `Pen`, `SolidBrush`) MUST be inside
`using` blocks. See `lessons-learned.instructions.md` — GDI section.

### `SnapshotManager.Save()` — avoid live StatusMap in save path

Pass `cachedStatus` to `Save()` to skip redundant StatusMap. See lessons-learned:
"SnapshotManager.Save() Calls Live StatusMap()".

## Profiling Workflow

1. **Reproduce** — write a failing wall-clock test that demonstrates the regression
2. **Baseline** — measure current hot-path with `Stopwatch` or BenchmarkDotNet
3. **Isolate** — narrow to the specific method using selective benchmarks
4. **Fix** — apply the optimisation (cache, lazy init, parallel, pool, etc.)
5. **Verify** — wall-clock test now passes; add budget comment with new baseline
6. **Commit** with type `perf(scope): description`

## What NOT to Do

- Don't profile in Debug build — always use `-c Release` for meaningful numbers
- Don't hardcode budgets without a comment showing the tweak count at time of writing
- Don't suppress `[MemoryDiagnoser]` output — allocation counts matter
- Don't use `Thread.Sleep` as a timing tool — use `Stopwatch`
- Don't run BenchmarkDotNet inside xUnit — use `tests/RegiLattice.Benchmarks/` instead
