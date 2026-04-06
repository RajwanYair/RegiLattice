// RegiLattice.Core.Tests — E2EWorkflowTests.cs
// Phase 4.1 — End-to-end workflow tests covering the full tweak lifecycle.
// Phase 4.6 — Concurrent safety tests for StatusMap and ApplyBatch.
//
// All tests run in DryRun=true mode — no registry writes occur.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

// ── Phase 4.1 — E2E Workflow Tests ──────────────────────────────────────────

/// <summary>
/// End-to-end workflow tests.  Each scenario exercises a complete user workflow
/// from engine creation through apply / detect / verify / revert using DryRun.
/// </summary>
[Collection("E2E-Sequential")]
public sealed class E2EWorkflowTests : IDisposable
{
    private readonly TweakEngine _engine;
    private readonly string _tmpDir;

    public E2EWorkflowTests()
    {
        _engine = new TweakEngine(new RegistrySession(dryRun: true));
        _engine.RegisterBuiltins();
        CorporateGuard.StubCorporate = false;
        _tmpDir = Path.Combine(Path.GetTempPath(), $"RL-E2E-{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tmpDir);
    }

    public void Dispose()
    {
        CorporateGuard.StubCorporate = null;
        if (Directory.Exists(_tmpDir))
            Directory.Delete(_tmpDir, recursive: true);
    }

    // ── Scenario 1 — Full apply → detect → remove lifecycle ─────────────────

    [Fact]
    public void Scenario1_ApplyDetectRemove_FullLifecycle_DryRun()
    {
        // Arrange — pick a known privacy tweak with Registry ops
        var tweak = _engine.Search("telemetry")
            .First(t => t.Kind is TweakKind.Registry or TweakKind.GroupPolicy);

        // Act — Apply
        var applyResult = _engine.Apply(tweak, requireAdmin: false, forceCorp: false);

        // Assert — In DryRun the result is Applied or NotApplied (detection uses real registry)
        Assert.True(
            applyResult is TweakResult.Applied or TweakResult.NotApplied or TweakResult.Unknown,
            $"Apply returned unexpected result: {applyResult}"
        );

        // Act — Detect
        var detected = _engine.DetectStatus(tweak);
        Assert.True(
            detected is TweakResult.Applied or TweakResult.NotApplied or TweakResult.Unknown,
            $"DetectStatus returned unexpected result: {detected}"
        );

        // Act — Remove
        var removeResult = _engine.Remove(tweak, requireAdmin: false, forceCorp: false);
        Assert.True(
            removeResult is TweakResult.NotApplied or TweakResult.Applied or TweakResult.Unknown,
            $"Remove returned unexpected result: {removeResult}"
        );
    }

    // ── Scenario 2 — Profile → batch apply workflow ──────────────────────────

    [Fact]
    public void Scenario2_ApplyProfile_PrivacyProfile_ReturnsBatchResults()
    {
        var results = _engine.ApplyProfile("privacy", forceCorp: false, parallel: false);

        Assert.NotNull(results);
        Assert.NotEmpty(results);
        Assert.All(results.Values, r =>
            Assert.True(Enum.IsDefined(r), $"Result {r} is not a valid TweakResult"));
    }

    // ── Scenario 3 — Apply batch then remove all ─────────────────────────────

    [Fact]
    public void Scenario3_ApplyBatchThenRemoveAll_CountsMatch()
    {
        // Use a small subset to keep the test fast
        var tweaks = _engine.AllTweaks()
            .Where(t => t.Category == 
            .Where(t => t.Category == "Performance")
            .Take(10)
            .ToList();

        Assert.True(tweaks.Count > 0, "Expected at least one Performance tweak");

        var applyResults = _engine.ApplyBatch(tweaks, forceCorp: false, parallel: false);
        Assert.Equal(tweaks.Count, applyResults.Count);

        var removeResults = _engine.RemoveBatch(tweaks, forceCorp: false, parallel: false);
        Assert.Equal(tweaks.Count, removeResults.Count);
    }

    // ── Scenario 4 — DryRun validation — registry session captures ops ───────

    [Fact]
    public void Scenario4_DryRun_RegistrySession_CapturesOpsWithoutWriting()
    {
        // Create a fresh engine with an inspectable RegistrySession
        var session = new RegistrySession(dryRun: true);
        var engine = new TweakEngine(session);

        var tweak = new TweakDef
        {
            Id = "e2e-dryrun-test",
            Label = "DryRun Test",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(
                @"HKEY_CURRENT_USER\SOFTWARE\RegiLattice\E2ETest",
                "DryValue", 42)],
            RemoveOps = [RegOp.DeleteValue(
                @"HKEY_CURRENT_USER\SOFTWARE\RegiLattice\E2ETest",
                "DryValue")],
            DetectOps = [RegOp.CheckDword(
                @"HKEY_CURRENT_USER\SOFTWARE\RegiLattice\E2ETest",
                "DryValue", 42)],
        };[tweak]);

        engine.Apply(tweak, requireAdmin: false, forceCorp: false);

        // DryRun must record the op (DryOps is an int count)
        Assert.True(session.DryOps > 0, "Expected DryRun to capture at least one ope at least one op");
    }

    // ── Scenario 5 — Snapshot create / restore round-trip ────────────────────

    [Fact]
    public void Scenario5_SnapshotRoundTrip_SaveAndRestore_FileCreated()
    {
        var snapshotPath = Path.Combine(_tmpDir, "snapshot.json");
        _engine.SaveSnapshot(snapshotPath);

        Assert.True(File.Exists(snapshotPath), "Snapshot file was not created");
        Assert.True(new FileInfo(snapshotPath).Length > 0, "Snapshot file is empty");

        // Load back
        var loaded = _engine.LoadSnapshot(snapshotPath);
        Assert.NotNull(loaded);
        Assert.NotEmpty(loaded);
    }

    // ── Scenario 6 — Export JSON → file created and valid ────────────────────

    [Fact]
    public void Scenario6_ExportJson_FileSaved_ContainsTweaks()
    {
        var exportPath = Path.Combine(_tmpDir, "export.json");

        // Pass a pre-built stub status so ExportJson skips live DetectStatus registry
        // reads (which are very slow on Intune filter-driver machines — ~7k registry reads).
        var stubStatus = _engine.AllTweaks()
            .ToDictionary(t => t.Id, _ => TweakResult.Unknown);
        _engine.ExportJson(exportPath, stubStatus);

        Assert.True(File.Exists(exportPath));
        var content = File.ReadAllText(exportPath);
        Assert.Contains("\"id\"", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("\"category\"", content, StringComparison.OrdinalIgnoreCase);
    }

    // ── Scenario 7 — Dependency chain resolution end-to-end ──────────────────

    [Fact]
    public void Scenario7_DependencyChain_ResolveAndDependents_Consistent()
    {
        // Find a tweak that has DependsOn
        var dependent = _engine.AllTweaks().FirstOrDefault(t => t.DependsOn.Count > 0);
        if (dependent is null)
        {
            // No dependencies in this build — still verify API runs cleanly
            var anyId = _engine.AllTweaks().First().Id;
            var chain = _engine.ResolveDependencies(anyId);
            Assert.NotNull(chain);
            return;
        }

        var resolved = _engine.ResolveDependencies(dependent.Id);
        Assert.NotNull(resolved);
        Assert.Contains(resolved, t => t.Id == dependent.Id);

        // All DependsOn IDs should be in the resolved chain
        foreach (var depId in dependent.DependsOn)
            Assert.Contains(resolved, t => t.Id == depI t.Id == depId);
    }

    // ── Scenario 8 — CorporateGuard blocks unsafe tweaks when enabled ─────────

    [Fact]
    public void Scenario8_CorporateGuard_BlocksUnsafeTweaks_WhenActive()
    {
        CorporateGuard.StubCorporate = true;
        try
        {
            var corpSession = new RegistrySession(dryRun: true);
            var corpEngine = new TweakEngine(corpSession);

            var unsafeTweak = new TweakDef
            {
                Id = "e2e-corp-unsafe",
                Label = "Corp Unsafe Tweak",
                Category = "Security",
                CorpSafe = false,
                NeedsAdmin = true,
                ApplyOps = [RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\RegiLattice\CorpTest",
                    "Value", 1)],
                RemoveOps = [RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\RegiLattice\CorpTest",
                    "Value")],
                DetectOps = [RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\RegiLattice\CorpTest",
                    "Value", 1)],
            };
            corpEngine.Register([unsafeTweak]k]);

            var result = corpEngine.Apply(unsafeTweak, requireAdmin: false, forceCorp: false);
            Assert.Equal(TweakResult.SkippedCorp, result);
        }
        finally
        {
            CorporateGuard.StubCorporate = false;
        }
    }

    // ── Scenario 9 — ValidateTweaks returns 0 issues on full builtins ─────────

    [Fact]
    public void Scenario9_ValidateTweaks_AllBuiltins_ZeroErrors()
    {
        var errors = _engine.ValidateTweaks();
        Assert.Empty(errors);
    }

    // ── Scenario 10 — Engine loads all 146+ modules without exception ─────────

    [Fact]
    public void Scenario10_RegisterBuiltins_LoadsAllModules_NoException()
    {
        var freshEngine = new TweakEngine(new RegistrySession(dryRun: true));
        var ex = Record.Exception(freshEngine.RegisterBuiltins);
        Assert.Null(ex);

        var all = freshEngine.AllTweaks();
        Assert.True(all.Count >= 7000,
            $"Expected ≥7,000 tweaks after RegisterBuiltins; got {all.Count}");
        Assert.True(freshEngine.Categories().Count >= 100,
            $"Expected ≥100 categories; got {freshEngine.Categories().Count}");
    }
}

// ── Phase 4.6 — Concurrent Safety Tests ─────────────────────────────────────

/// <summary>
/// Concurrent safety tests verifying that StatusMap and ApplyBatch are
/// thread-safe under simultaneous access.  All run with DryRun=true.
/// </summary>
[Collection("E2E-Sequential")]
public sealed class ConcurrentSafetyTests : IDisposable
{
    private readonly TweakEngine _engine;

    public ConcurrentSafetyTests()
    {
        _engine = new TweakEngine(new RegistrySession(dryRun: true));
        _engine.RegisterBuiltins();
        CorporateGuard.StubCorporate = false;
    }

    public void Dispose() => CorporateGuard.StubCorporate = null;

    // ── 10 concurrent StatusMap calls — no deadlock, results consistent ───────

    [Fact]
    public async Task Concurrent_StatusMap_10Parallel_NoDeadlock()
    {
        const int concurrency = 10;

        // Use a small fixed id list to limit wall-clock time
        var ids = _engine.AllTweaks()
            .Where(t => t.Kind is TweakKind.Registry or TweakKind.GroupPolicy)
            .Take(20)
            .Select(t => t.Id)
            .ToList();

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        var tasks = Enumerable.Range(0, concurrency).Select(_ =>
            Task.Run(() => _engine.StatusMap(parallel: false, ids: ids), cts.Token)
        ).ToArray();

        var results = await Task.WhenAll(tasks);

        Assert.Equal(concurrency, results.Length);
        foreach (var result in results)
        {
            Assert.NotNull(result);
            Assert.Equal(ids.Count, r
            Assert.NotNull(result);
            Assert.Equal(ids.Count, result.Count);
        }
    }

    // ── 5 concurrent ApplyBatch with DryRun — no corruption ──────────────────
async Task Concurrent_ApplyBatch_5Parallel_NoCorruption()
    {
        const int concurrency = 5;

        var tweaks = _engine.AllTweaks()
            .Where(t => t.Category == "Privacy")
            .Take(10)
            .ToList();

        Assert.True(tweaks.Count > 0);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        var tasks = Enumerable.Range(0, concurrency).Select(_ =>
            Task.Run(() => _engine.ApplyBatch(tweaks, forceCorp: false, parallel: false), cts.Token)
        ).ToArray();

        var results = await Task.WhenAll(tasks);

        Assert.Equal(concurrency, results.Length);
        foreach (var result in results)
        {
            Assert.NotNull(result);
            Assert.Equal(tweaks.Count, result.Count);
            Assert.All(rrrency, results.Length);
        foreach (var result in results)
        {
            Assert.NotNull(result);
            Assert.Equal(tweaks.Count, result.Count);
            Assert.All(result.Values, r => Assert.True(Enum.IsDefined(r)));
        }
    }async Task Concurrent_MixedReadAndApply_NoException()
    {
        var tweaks = _engine.AllTweaks()
            .Where(t => t.Category == "Performance")
            .Take(5)
            .ToList();

        var readTasks = Enumerable.Range(0, 5).Select(_ =>
            Task.Run(() =>
            {
                var allTweaks = _engine.AllTweaks();
                var cats = _engine.Categories();
                return _engine.Search("cpu");
            })
        );

        var applyTasks = Enumerable.Range(0, 3).Select(_ =>
            Task.Run(() => _engine.ApplyBatch(tweaks, forceCorp: false, parallel: false))
        );

        await Task.When

        var applyTasks = Enumerable.Range(0, 3).Select(_ =>
            Task.Run(() => _engine.ApplyBatch(tweaks, forceCorp: false, parallel: false))
        );

        await Task.WhenAll([.. readTasks, .. applyTasks]);
    }
}

// ── Collection definition — prevents test classes from running in parallel ───

[CollectionDefinition("E2E-Sequential")]
public sealed class E2ESequentialCollection : ICollectionFixture<BuiltinsFixture> { }
