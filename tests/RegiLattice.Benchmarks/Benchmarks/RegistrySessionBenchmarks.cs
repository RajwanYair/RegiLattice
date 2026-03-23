// tests/RegiLattice.Benchmarks/Benchmarks/RegistrySessionBenchmarks.cs
// Micro-benchmarks for RegistrySession: DryRun operation throughput,
// RegOp evaluation, and key-existence checks.

using BenchmarkDotNet.Attributes;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Benchmarks;

/// <summary>
/// Benchmarks for RegistrySession operations.
/// All write benchmarks use DryRun=true (no real registry modifications).
/// Read benchmarks target well-known HKCU keys present on all Windows machines.
/// </summary>
[MemoryDiagnoser]
public class RegistrySessionBenchmarks
{
    private RegistrySession _drySession = null!;
    private RegistrySession _liveSession = null!;
    private IReadOnlyList<RegOp> _dwordOps = null!;
    private IReadOnlyList<RegOp> _checkOps = null!;

    private const string WellKnownPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    [GlobalSetup]
    public void Setup()
    {
        _drySession = new RegistrySession(dryRun: true);
        _liveSession = new RegistrySession(dryRun: false);

        _dwordOps =
        [
            RegOp.SetDword(WellKnownPath, "BenchValue1", 1),
            RegOp.SetDword(WellKnownPath, "BenchValue2", 0),
            RegOp.SetDword(WellKnownPath, "BenchValue3", 1),
        ];

        _checkOps = [RegOp.CheckDword(WellKnownPath, "HideFileExt", 0), RegOp.CheckDword(WellKnownPath, "Hidden", 1)];
    }

    /// <summary>DryRun SetDword — measures path-parse + log overhead with no real I/O.</summary>
    [Benchmark(Baseline = true)]
    public void SetDword_DryRun() => _drySession.SetDword(WellKnownPath, "BenchTemp", 42);

    /// <summary>Evaluate a batch of DryRun write ops — simulate an Apply operation.</summary>
    [Benchmark]
    public void Execute_Batch_DryRun() => _drySession.Execute(_dwordOps);

    /// <summary>Evaluate a batch of registry-check ops (live read from HKCU).</summary>
    [Benchmark]
    public bool Evaluate_CheckOps() => _liveSession.Evaluate(_checkOps);

    /// <summary>Key-existence check — one registry open+close per call.</summary>
    [Benchmark]
    public bool KeyExists() => _liveSession.KeyExists(WellKnownPath);

    /// <summary>ReadDword from a well-known key (live I/O, measures registry roundtrip).</summary>
    [Benchmark]
    public int? ReadDword() => _liveSession.ReadDword(WellKnownPath, "HideFileExt");
}
