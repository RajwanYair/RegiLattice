// Phase1Tests.cs — xUnit tests for v6.14.0 Phase 1 Engine & Model Hardening features:
//   1.3 TweakRisk flags
//   1.5 Search relevance ranking + SearchRanked
//   1.2 CancellationToken on StatusMap and Search
//   1.1 BatchResult + transactional ApplyBatch / RemoveBatch by ID
//   1.4 RegDiff + ExecuteWithDiff

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

public sealed class Phase1Tests
{
    // ── Helpers ──────────────────────────────────────────────────────────

    private static TweakDef MakeHklm(string id, bool deleteTree = false) =>
        new()
        {
            Id = id,
            Label = id,
            Category = "Test",
            ApplyOps = deleteTree
                ? [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test")]
                : [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "V", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "V")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "V", 1)],
        };

    private static TweakDef MakeHkcu(string id) =>
        new()
        {
            Id = id,
            Label = id,
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Test", "V")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };

    private static TweakDef MakeWithCategory(string id, string category) =>
        new()
        {
            Id = id,
            Label = id,
            Category = category,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\Test", "V")],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Test", "V", 1)],
        };

    // ═══════════════════════════════════════════════════════════════════
    // Phase 1.3 — TweakRisk flags
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public void TweakRisk_DefaultsToNone()
    {
        var td = MakeHklm("risk-default");
        Assert.Equal(TweakRisk.None, td.RiskFlags);
    }

    [Fact]
    public void TweakRisk_EffectiveRiskFlags_AutoDetectsModifiesHKLM()
    {
        var td = MakeHklm("risk-hklm");
        Assert.True(td.EffectiveRiskFlags.HasFlag(TweakRisk.ModifiesHKLM));
    }

    [Fact]
    public void TweakRisk_EffectiveRiskFlags_AutoDetectsModifiesHKCU()
    {
        var td = MakeHkcu("risk-hkcu");
        Assert.True(td.EffectiveRiskFlags.HasFlag(TweakRisk.ModifiesHKCU));
    }

    [Fact]
    public void TweakRisk_EffectiveRiskFlags_AutoDetectsDeletesKey()
    {
        var td = MakeHklm("risk-delete", deleteTree: true);
        Assert.True(td.EffectiveRiskFlags.HasFlag(TweakRisk.DeletesKey));
    }

    [Fact]
    public void TweakRisk_MixedHives_HasBothFlags()
    {
        var td = new TweakDef
        {
            Id = "risk-both",
            Label = "Both",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Test", "V", 1), RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
            RemoveOps = [],
        };
        Assert.True(td.EffectiveRiskFlags.HasFlag(TweakRisk.ModifiesHKLM));
        Assert.True(td.EffectiveRiskFlags.HasFlag(TweakRisk.ModifiesHKCU));
    }

    [Fact]
    public void TweakRisk_ExplicitFlagPreserved()
    {
        var td = new TweakDef
        {
            Id = "risk-explicit",
            Label = "Explicit",
            Category = "Test",
            RiskFlags = TweakRisk.RequiresReboot,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
            RemoveOps = [],
        };
        Assert.True(td.EffectiveRiskFlags.HasFlag(TweakRisk.RequiresReboot));
        Assert.True(td.EffectiveRiskFlags.HasFlag(TweakRisk.ModifiesHKCU)); // auto-detected + explicit
    }

    [Fact]
    public void TweakRisk_ServicesCategory_SetsAffectsService()
    {
        var td = MakeWithCategory("risk-svc", "Services");
        Assert.True(td.EffectiveRiskFlags.HasFlag(TweakRisk.AffectsService));
    }

    [Fact]
    public void TweakRisk_NetworkCategory_SetsAffectsNetwork()
    {
        var td = MakeWithCategory("risk-net", "Network");
        Assert.True(td.EffectiveRiskFlags.HasFlag(TweakRisk.AffectsNetwork));
    }

    [Fact]
    public void TweakRisk_SecurityCategory_SetsAffectsSecurity()
    {
        var td = MakeWithCategory("risk-sec", "Security");
        Assert.True(td.EffectiveRiskFlags.HasFlag(TweakRisk.AffectsSecurity));
    }

    [Fact]
    public void TweakRisk_NoOps_NoCategory_RemainsNone()
    {
        var td = new TweakDef
        {
            Id = "risk-none",
            Label = "None",
            Category = "Test",
            ApplyOps = [],
            ApplyAction = _ => { }, // has operation but no ops
        };
        Assert.Equal(TweakRisk.None, td.EffectiveRiskFlags);
    }

    [Fact]
    public void TweakRisk_FlagsEnum_BitwiseOrWorks()
    {
        TweakRisk flags = TweakRisk.ModifiesHKLM | TweakRisk.DeletesKey;
        Assert.True(flags.HasFlag(TweakRisk.ModifiesHKLM));
        Assert.True(flags.HasFlag(TweakRisk.DeletesKey));
        Assert.False(flags.HasFlag(TweakRisk.RequiresReboot));
    }

    // ═══════════════════════════════════════════════════════════════════
    // Phase 1.5 — Search relevance ranking
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public void Search_ReturnsResultsOrderedByRelevance_IdMatchFirst()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "ranking-exact-test",
                Label = "Unrelated Label",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
                RemoveOps = [],
            },
            new TweakDef
            {
                Id = "ranking-other",
                Label = "Ranking Exact Test Something",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test2", "V", 1)],
                RemoveOps = [],
            },
        ]);

        var results = engine.Search("ranking-exact-test");

        Assert.NotEmpty(results);
        // Exact ID match should come first
        Assert.Equal("ranking-exact-test", results[0].Id);
    }

    [Fact]
    public void SearchRanked_ReturnsScoresDescending()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "ranked-id-match",
                Label = "Telemetry Exact Match",
                Category = "Privacy",
                Tags = ["telemetry"],
                Description = "Disable telemetry data collection",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
                RemoveOps = [],
            },
            new TweakDef
            {
                Id = "ranked-other",
                Label = "Other",
                Category = "Test",
                Description = "Contains the word telemetry somewhere",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test2", "V", 1)],
                RemoveOps = [],
            },
        ]);

        var results = engine.SearchRanked("telemetry");

        Assert.NotEmpty(results);
        // Scores should be in descending order
        for (int i = 0; i < results.Count - 1; i++)
            Assert.True(
                results[i].Score >= results[i + 1].Score,
                $"Score at {i} ({results[i].Score}) should be >= score at {i + 1} ({results[i + 1].Score})"
            );
    }

    [Fact]
    public void SearchRanked_EmptyQuery_ReturnsAllWithZeroScore()
    {
        var engine = TestHelpers.CreateEngine();
        var results = engine.SearchRanked("");
        Assert.Equal(engine.AllTweaks().Count, results.Count);
        Assert.All(results, r => Assert.Equal(0, r.Score));
    }

    [Fact]
    public void SearchRanked_NoMatch_ReturnsEmpty()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "srn-1",
                Label = "Alpha",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
                RemoveOps = [],
            },
        ]);
        var results = engine.SearchRanked("zzz-not-found");
        Assert.Empty(results);
    }

    [Fact]
    public void Search_IdExactMatch_ScoresHigherThanLabelMatch()
    {
        var engine = new TweakEngine();
        engine.Register([
            // Tweak whose label contains "gamma" but ID doesn't
            new TweakDef
            {
                Id = "alpha-beta",
                Label = "gamma delta optimization",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\A", "V", 1)],
                RemoveOps = [],
            },
            // Tweak whose ID exactly = "gamma"
            new TweakDef
            {
                Id = "gamma",
                Label = "Something Else Entirely",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\B", "V", 1)],
                RemoveOps = [],
            },
        ]);

        var results = engine.Search("gamma");

        // Exact ID match should be first
        Assert.Equal("gamma", results[0].Id);
    }

    // ═══════════════════════════════════════════════════════════════════
    // Phase 1.2 — CancellationToken support
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public void Search_AlreadyCancelledToken_ThrowsOperationCancelled()
    {
        var engine = TestHelpers.CreateEngine();
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        Assert.Throws<OperationCanceledException>(() => engine.Search("telemetry", cts.Token));
    }

    [Fact]
    public void SearchRanked_AlreadyCancelledToken_ThrowsOperationCancelled()
    {
        var engine = TestHelpers.CreateEngine();
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        Assert.Throws<OperationCanceledException>(() => engine.SearchRanked("privacy", cts.Token));
    }

    [Fact]
    public void StatusMap_AlreadyCancelledToken_ThrowsOperationCancelled()
    {
        var engine = TestHelpers.CreateEngine();
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        Assert.Throws<OperationCanceledException>(() => engine.StatusMap(parallel: false, ids: null, ct: cts.Token));
    }

    [Fact]
    public void StatusMap_NotCancelledToken_ReturnsNormally()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "ct-sm-1",
                Label = "CT SM 1",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\CT", "V", 1)],
                RemoveOps = [],
                DetectOps = [RegOp.CheckDword(@"HKCU\Software\CT", "V", 1)],
            },
        ]);
        using var cts = new CancellationTokenSource();
        var map = engine.StatusMap(parallel: false, ids: null, ct: cts.Token);
        Assert.Single(map);
    }

    [Fact]
    public void ApplyBatch_AlreadyCancelledToken_WasCancelledTrue()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "ct-ab-1",
                Label = "CT AB 1",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\CT", "V", 1)],
                RemoveOps = [],
            },
        ]);
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var result = engine.ApplyBatch(["ct-ab-1"], ct: cts.Token);
        Assert.True(result.WasCancelled);
        Assert.Empty(result.Results);
    }

    // ═══════════════════════════════════════════════════════════════════
    // Phase 1.1 — BatchResult and transactional apply
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public void ApplyBatch_ByIds_ReturnsBatchResult()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "br-1",
                Label = "BR 1",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\BR", "V", 1)],
                RemoveOps = [],
                DetectOps = [RegOp.CheckDword(@"HKCU\Software\BR", "V", 1)],
            },
            new TweakDef
            {
                Id = "br-2",
                Label = "BR 2",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\BR", "V2", 1)],
                RemoveOps = [],
                DetectOps = [RegOp.CheckDword(@"HKCU\Software\BR", "V2", 1)],
            },
        ]);

        var result = engine.ApplyBatch(["br-1", "br-2"]);

        Assert.NotNull(result);
        Assert.Equal(2, result.Results.Count);
        Assert.False(result.RolledBack);
        Assert.False(result.WasCancelled);
    }

    [Fact]
    public void RemoveBatch_ByIds_ReturnsBatchResult()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "rbr-1",
                Label = "RBR 1",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\RBR", "V", 1)],
                RemoveOps = [],
                DetectOps = [RegOp.CheckDword(@"HKCU\Software\RBR", "V", 1)],
            },
        ]);

        var result = engine.RemoveBatch(["rbr-1"]);

        Assert.NotNull(result);
        Assert.Single(result.Results);
        Assert.False(result.RolledBack);
    }

    [Fact]
    public void BatchResult_SuccessCount_ExcludesErrors()
    {
        // A DryRun engine will return Unknown (not Error) for detect-only tweaks,
        // so simulate using a tweak with only ApplyAction that always fails.
        // Use forceCorp: true to bypass corporate guard on Intel machines.
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "brc-ok",
                Label = "OK",
                Category = "Test",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\BRC", "V", 1)],
                RemoveOps = [],
            },
            new TweakDef
            {
                Id = "brc-err",
                Label = "Err",
                Category = "Test",
                CorpSafe = true,
                ApplyAction = _ => throw new InvalidOperationException("test error"),
                ApplyOps = [],
                RemoveOps = [],
            },
        ]);

        var result = engine.ApplyBatch(["brc-ok", "brc-err"], forceCorp: true);

        Assert.Equal(2, result.Results.Count);
        Assert.Equal(1, result.FailureCount);
    }

    [Fact]
    public void ApplyBatch_OnProgress_IsCalled()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "prog-1",
                Label = "P1",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Prog", "V", 1)],
                RemoveOps = [],
            },
            new TweakDef
            {
                Id = "prog-2",
                Label = "P2",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Prog", "V2", 1)],
                RemoveOps = [],
            },
        ]);

        var calls = new List<(int Done, int Total, string Id)>();
        engine.ApplyBatch(["prog-1", "prog-2"], onProgress: (done, total, id) => calls.Add((done, total, id)));

        Assert.Equal(2, calls.Count);
        Assert.Equal(1, calls[0].Done);
        Assert.Equal(2, calls[0].Total);
        Assert.Equal(2, calls[1].Done);
    }

    [Fact]
    public void ApplyBatch_EmptyIds_ReturnsEmptyBatchResult()
    {
        var engine = TestHelpers.CreateEngine();
        var result = engine.ApplyBatch(Array.Empty<string>());
        Assert.Empty(result.Results);
        Assert.False(result.RolledBack);
        Assert.False(result.HasErrors);
    }

    [Fact]
    public void BatchResult_RolledBack_IsFalseOnSuccess()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "rb-ok-1",
                Label = "RB OK 1",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\RB", "V", 1)],
                RemoveOps = [],
            },
        ]);

        var result = engine.ApplyBatch(["rb-ok-1"], transactional: true);

        Assert.False(result.RolledBack);
    }

    // ═══════════════════════════════════════════════════════════════════
    // Phase 1.4 — RegDiff + ExecuteWithDiff
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public void ExecuteWithDiff_DryRun_SetValue_ReturnsExpectedDiff()
    {
        var session = new RegistrySession(dryRun: true);
        var ops = new[] { RegOp.SetDword(@"HKCU\Software\DiffTest", "MyValue", 42) };

        var (diffs, success) = session.ExecuteWithDiff(ops);

        Assert.True(success);
        Assert.Single(diffs);
        Assert.Equal(@"HKCU\Software\DiffTest", diffs[0].Path);
        Assert.Equal("MyValue", diffs[0].ValueName);
        Assert.Equal(42, diffs[0].After);
    }

    [Fact]
    public void ExecuteWithDiff_DryRun_DeleteValue_ShowsNullAfter()
    {
        var session = new RegistrySession(dryRun: true);
        var ops = new[] { RegOp.DeleteValue(@"HKCU\Software\DiffTest", "MyValue") };

        var (diffs, success) = session.ExecuteWithDiff(ops);

        Assert.True(success);
        Assert.Single(diffs);
        Assert.Null(diffs[0].After);
        Assert.Equal("MyValue", diffs[0].ValueName);
    }

    [Fact]
    public void ExecuteWithDiff_DryRun_DeleteTree_ReturnsKeyDiff()
    {
        var session = new RegistrySession(dryRun: true);
        var ops = new[] { RegOp.DeleteTree(@"HKCU\Software\DiffTest") };

        var (diffs, success) = session.ExecuteWithDiff(ops);

        Assert.True(success);
        Assert.Single(diffs);
        Assert.Equal("(key)", diffs[0].ValueName);
        Assert.Null(diffs[0].After);
    }

    [Fact]
    public void ExecuteWithDiff_EmptyOps_ReturnsTrueWithEmptyDiffs()
    {
        var session = new RegistrySession(dryRun: true);

        var (diffs, success) = session.ExecuteWithDiff([]);

        Assert.True(success);
        Assert.Empty(diffs);
    }

    [Fact]
    public void RegDiff_Changed_TrueWhenValuesAreDifferent()
    {
        var diff = new RegDiff
        {
            Path = @"HKCU\Test",
            ValueName = "V",
            Before = 1,
            After = 2,
        };
        Assert.True(diff.Changed);
    }

    [Fact]
    public void RegDiff_Changed_FalseWhenValuesAreEqual()
    {
        var diff = new RegDiff
        {
            Path = @"HKCU\Test",
            ValueName = "V",
            Before = 42,
            After = 42,
        };
        Assert.False(diff.Changed);
    }

    [Fact]
    public void RegDiff_Changed_TrueWhenBeforeNullAfterNotNull()
    {
        var diff = new RegDiff
        {
            Path = @"HKCU\Test",
            ValueName = "V",
            Before = null,
            After = 1,
        };
        Assert.True(diff.Changed);
    }

    [Fact]
    public void RegDiff_Properties_AreAccessible()
    {
        var diff = new RegDiff
        {
            Path = @"HKLM\Software\Test",
            ValueName = "Foo",
            Before = "old",
            After = "new",
        };
        Assert.Equal(@"HKLM\Software\Test", diff.Path);
        Assert.Equal("Foo", diff.ValueName);
        Assert.Equal("old", diff.Before);
        Assert.Equal("new", diff.After);
        Assert.True(diff.Changed);
        Assert.Contains("→", diff.ToString());
    }
}
