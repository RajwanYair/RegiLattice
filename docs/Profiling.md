# RegiLattice — Profiling Guide

> Performance measurement and optimization for the C# codebase.
> Last updated: 2025-07-20 · v3.0.0

---

## Table of Contents

1. [Performance Overview](#1-performance-overview)
2. [Built-in Diagnostics](#2-built-in-diagnostics)
3. [.NET Profiling Tools](#3-net-profiling-tools)
4. [BenchmarkDotNet](#4-benchmarkdotnet)
5. [Common Bottlenecks](#5-common-bottlenecks)
6. [WinForms Performance](#6-winforms-performance)

---

## 1. Performance Overview

| Root Cause | Symptoms | Diagnostic Tool |
|---|---|---|
| **CPU-bound** | High CPU, UI freezes during computation | dotTrace, PerfView, BenchmarkDotNet |
| **I/O-bound** | Freeze waiting for registry/disk | PerfView, `Stopwatch` around I/O calls |
| **Rendering stall** | UI feels heavy on scroll/resize | WinForms DoubleBuffered, profile `OnPaint` |
| **Allocation pressure** | GC pauses, memory growth | dotMemory, `GC.GetTotalMemory()` |

> **Golden rule** — The UI thread must never block for more than ~16 ms
> (one frame at 60 fps). Long operations should run via `Task.Run()` or `BackgroundWorker`.

---

## 2. Built-in Diagnostics

### Stopwatch Timing

```csharp
var sw = System.Diagnostics.Stopwatch.StartNew();
var statuses = engine.StatusMap(parallel: true);
sw.Stop();
Console.WriteLine($"StatusMap took {sw.ElapsedMilliseconds} ms");
```

### DryRun Mode

`RegistrySession.DryRun = true` prevents any registry writes. Use for safe
profiling of apply/remove operations without side effects.

---

## 3. .NET Profiling Tools

### PerfView (Free, Microsoft)

```powershell
# Download PerfView from https://github.com/microsoft/perfview/releases
# Profile the CLI:
PerfView /GCCollectOnly collect dotnet run --project src/RegiLattice.CLI -- --list
```

### dotTrace (JetBrains)

- Attach to running WinForms process
- Timeline view shows UI thread stalls
- Hotspot analysis for CPU-bound code

### Visual Studio Profiler

```powershell
# From Developer PowerShell:
dotnet build -c Release
# Debug > Performance Profiler > CPU Usage / .NET Object Allocation
```

---

## 4. BenchmarkDotNet

Add benchmarks to a dedicated project:

```csharp
[MemoryDiagnoser]
public class EngineBenchmarks
{
    private TweakEngine _engine = null!;

    [GlobalSetup]
    public void Setup()
    {
        _engine = new TweakEngine();
        _engine.RegisterBuiltins();
    }

    [Benchmark]
    public IReadOnlyList<string> Categories() => _engine.Categories();

    [Benchmark]
    public IReadOnlyList<TweakDef> Search() => _engine.Search("privacy");

    [Benchmark]
    public IReadOnlyDictionary<string, int> CategoryCounts() => _engine.CategoryCounts();
}
```

Run with:

```powershell
dotnet run --project benchmarks/ -c Release
```

---

## 5. Common Bottlenecks

| Bottleneck | Cause | Fix |
|---|---|---|
| Slow `RegisterBuiltins()` | Large number of tweak modules | Pre-compile, lazy-load modules |
| Slow `StatusMap()` | Sequential registry reads | Use `parallel: true` parameter |
| UI freeze on apply/remove | Blocking UI thread | Use `Task.Run()` + `Invoke()` for UI updates |
| Theme switch lag | Repainting many controls | Double-buffered rendering (already enabled) |
| Memory growth | Large tweak lists retained | Use `IReadOnlyList<T>`, avoid copying |

---

## 6. WinForms Performance

### Double Buffering

Already enabled in `MainForm`:

```csharp
SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
```

### Reducing Control Count

- Use collapsible sections (category panels) to reduce visible control count
- Lazy-create controls for collapsed sections
- Use `SuspendLayout()`/`ResumeLayout()` during batch updates

### Background Operations

```csharp
// Never block the UI thread for registry operations
await Task.Run(() => engine.ApplyBatch(ids));
// Update UI on the UI thread
Invoke(() => RefreshStatusBadges());
```
