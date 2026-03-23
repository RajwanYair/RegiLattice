// tests/RegiLattice.Benchmarks/Benchmarks/TweakEngineBenchmarks.cs
// Micro-benchmarks for TweakEngine hot paths: startup registration,
// search, filter, and category queries.

using BenchmarkDotNet.Attributes;
using RegiLattice.Core;
using RegiLattice.Core.Models;

namespace RegiLattice.Benchmarks;

/// <summary>
/// Benchmarks for TweakEngine core operations.
/// Run with: dotnet run --project tests/RegiLattice.Benchmarks -c Release
/// </summary>
[MemoryDiagnoser]
public class TweakEngineBenchmarks
{
    private TweakEngine _engine = null!;

    [GlobalSetup]
    public void Setup()
    {
        _engine = new TweakEngine();
        _engine.RegisterBuiltins();
    }

    /// <summary>Baseline: retrieve the full flat tweak list (should return same cached instance).</summary>
    [Benchmark(Baseline = true)]
    public IReadOnlyList<TweakDef> AllTweaks() => _engine.AllTweaks();

    /// <summary>Full-text search across IDs, labels, descriptions, and tags.</summary>
    [Benchmark]
    public IReadOnlyList<TweakDef> Search_Privacy() => _engine.Search("privacy");

    /// <summary>Multi-field search with a short term that matches many tweaks.</summary>
    [Benchmark]
    public IReadOnlyList<TweakDef> Search_Telemetry() => _engine.Search("telemetry");

    /// <summary>Filter by category — most common GUI operation.</summary>
    [Benchmark]
    public IReadOnlyList<TweakDef> Filter_ByCategory() => _engine.Filter(category: "Privacy");

    /// <summary>Filter by combined criteria (corp-safe + category + query).</summary>
    [Benchmark]
    public IReadOnlyList<TweakDef> Filter_Combined() => _engine.Filter(corpSafe: true, category: "Performance", query: "disable");

    /// <summary>Category grouping — used by the GUI category tree.</summary>
    [Benchmark]
    public IReadOnlyDictionary<string, List<TweakDef>> TweaksByCategory() => _engine.TweaksByCategory();

    /// <summary>Per-category counts dictionary — used by the GUI badge display.</summary>
    [Benchmark]
    public IReadOnlyDictionary<string, int> CategoryCounts() => _engine.CategoryCounts();

    /// <summary>Tag lookup — used by the tag cloud and CLI --show-tags.</summary>
    [Benchmark]
    public IReadOnlyList<TweakDef> TweaksByTag() => _engine.TweaksByTag("telemetry");
}
