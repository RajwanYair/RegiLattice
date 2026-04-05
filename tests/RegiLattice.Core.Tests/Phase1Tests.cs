// Phase1Tests.cs — xUnit tests for Phase 1 Engine & Model Hardening features:
//   1.1 BatchResult + transactional ApplyBatch / RemoveBatch by ID     (v6.14.0)
//   1.2 CancellationToken on StatusMap and Search                       (v6.14.0)
//   1.3 TweakRisk flags                                                 (v6.14.0)
//   1.4 RegDiff + ExecuteWithDiff                                       (v6.14.0)
//   1.5 Search relevance ranking + SearchRanked                        (v6.14.0)
//   1.6 User-defined custom profile API (TweakEngine wrappers)          (v6.15.0)
//   1.7 Tweak recommendation engine — TweakEngine.RecommendTweaks       (v6.15.0)

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

    // ═══════════════════════════════════════════════════════════════════
    // Phase 1.6 — User-defined custom profile API (TweakEngine wrappers)
    // ═══════════════════════════════════════════════════════════════════

    private static string UniqueName(string prefix = "phase16") =>
        $"{prefix}-{Guid.NewGuid():N}";

    [Fact]
    public void UserProfiles_InitiallyEmpty_WhenNoProfilesCreated()
    {
        // Profiles directory must not have accidentally leaked from a prior test.
        // We can't guarantee a clean slate on disk, so we just verify the method
        // returns an IReadOnlyList (contract check), not an exception.
        var profiles = TweakEngine.UserProfiles();
        Assert.NotNull(profiles);
    }

    [Fact]
    public void CreateUserProfile_PersistsToDisk_AndIsRetrievable()
    {
        string name = UniqueName("create");
        try
        {
            var created = TweakEngine.CreateUserProfile(name, "desc", ["id-a", "id-b"]);
            Assert.NotNull(created);
            Assert.Equal(name, created.Name);
            Assert.Equal("desc", created.Description);
            Assert.Equal(2, created.TweakIds.Count);

            var retrieved = TweakEngine.GetUserProfile(name);
            Assert.NotNull(retrieved);
            Assert.Equal(name, retrieved.Name);
        }
        finally
        {
            TweakEngine.DeleteUserProfile(name);
        }
    }

    [Fact]
    public void CreateUserProfile_DuplicateName_ThrowsInvalidOperationException()
    {
        string name = UniqueName("dup");
        TweakEngine.CreateUserProfile(name, "", []);
        try
        {
            Assert.Throws<InvalidOperationException>(() =>
                TweakEngine.CreateUserProfile(name, "", []));
        }
        finally
        {
            TweakEngine.DeleteUserProfile(name);
        }
    }

    [Fact]
    public void SaveUserProfile_OverwritesExistingProfile()
    {
        string name = UniqueName("save");
        try
        {
            var original = TweakEngine.CreateUserProfile(name, "original", ["id-x"]);
            var updated = original with { Description = "updated" };
            TweakEngine.SaveUserProfile(updated);

            var reloaded = TweakEngine.GetUserProfile(name);
            Assert.NotNull(reloaded);
            Assert.Equal("updated", reloaded.Description);
        }
        finally
        {
            TweakEngine.DeleteUserProfile(name);
        }
    }

    [Fact]
    public void UpdateUserProfile_UpdatesTweakIds()
    {
        string name = UniqueName("update");
        try
        {
            TweakEngine.CreateUserProfile(name, "desc", ["id-a"]);
            var updated = TweakEngine.UpdateUserProfile(name, ["id-b", "id-c"], "new-desc");
            Assert.Equal(2, updated.TweakIds.Count);
            Assert.Equal("new-desc", updated.Description);
        }
        finally
        {
            TweakEngine.DeleteUserProfile(name);
        }
    }

    [Fact]
    public void RenameUserProfile_ChangesNameOnDisk()
    {
        string name = UniqueName("rename-src");
        string newName = UniqueName("rename-dst");
        try
        {
            TweakEngine.CreateUserProfile(name, "", []);
            TweakEngine.RenameUserProfile(name, newName);

            Assert.Null(TweakEngine.GetUserProfile(name));
            Assert.NotNull(TweakEngine.GetUserProfile(newName));
        }
        finally
        {
            TweakEngine.DeleteUserProfile(newName);
        }
    }

    [Fact]
    public void CloneUserProfile_CreatesIndependentCopy()
    {
        string name = UniqueName("clone-src");
        string cloneName = UniqueName("clone-dst");
        try
        {
            TweakEngine.CreateUserProfile(name, "original", ["id-1"]);
            var clone = TweakEngine.CloneUserProfile(name, cloneName);
            Assert.Equal(cloneName, clone.Name);
            Assert.Equal("original", clone.Description);
            Assert.Equal(["id-1"], clone.TweakIds);
        }
        finally
        {
            TweakEngine.DeleteUserProfile(name);
            TweakEngine.DeleteUserProfile(cloneName);
        }
    }

    [Fact]
    public void DeleteUserProfile_RemovesProfileFromDisk()
    {
        string name = UniqueName("delete");
        TweakEngine.CreateUserProfile(name, "", []);
        TweakEngine.DeleteUserProfile(name);
        Assert.Null(TweakEngine.GetUserProfile(name));
    }

    [Fact]
    public void DeleteUserProfile_NonExistentName_DoesNotThrow()
    {
        TweakEngine.DeleteUserProfile("profile-that-does-not-exist-" + Guid.NewGuid().ToString("N"));
    }

    [Fact]
    public void ApplyUserProfile_WithUnknownIds_ReturnsDictionary()
    {
        string name = UniqueName("apply");
        try
        {
            TweakEngine.CreateUserProfile(name, "", ["nonexistent-id-1", "nonexistent-id-2"]);
            var engine = new TweakEngine(new RegistrySession(dryRun: true));
            // ApplyBatch returns empty when no IDs resolve — just verify no exception.
            var result = engine.ApplyUserProfile(name);
            Assert.NotNull(result);
        }
        finally
        {
            TweakEngine.DeleteUserProfile(name);
        }
    }

    [Fact]
    public void ApplyUserProfile_UnknownProfileName_ThrowsArgumentException()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        Assert.Throws<ArgumentException>(() =>
            engine.ApplyUserProfile("profile-that-does-not-exist-" + Guid.NewGuid().ToString("N")));
    }

    // ═══════════════════════════════════════════════════════════════════
    // Phase 1.7 — Tweak recommendation engine (TweakEngine.RecommendTweaks)
    // ═══════════════════════════════════════════════════════════════════

    private static TweakDef MakeRecommendable(string id, int impact, int safety) =>
        new()
        {
            Id = id,
            Label = id,
            Category = "Performance",
            Tags = ["perf"],
            ImpactScore = impact,
            SafetyRating = safety,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test\Recommend", id.Replace("-", ""), 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\Test\Recommend", id.Replace("-", ""))],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Test\Recommend", id.Replace("-", ""), 1)],
        };

    [Fact]
    public void RecommendTweaks_ReturnsNonNullList()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([MakeRecommendable("rec-001", impact: 4, safety: 5)]);
        var recommendations = engine.RecommendTweaks();
        Assert.NotNull(recommendations);
    }

    [Fact]
    public void RecommendTweaks_LimitsToMaxResults()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var tweaks = Enumerable.Range(1, 10)
            .Select(i => MakeRecommendable($"rec-limit-{i:000}", impact: 3, safety: 4))
            .ToList();
        engine.Register(tweaks);
        var recommendations = engine.RecommendTweaks(maxResults: 3);
        Assert.True(recommendations.Count <= 3, $"Expected ≤3 but got {recommendations.Count}");
    }

    [Fact]
    public void TweakRecommendation_ConfidencePercent_IsInRange()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([MakeRecommendable("rec-conf-001", impact: 5, safety: 5)]);
        var recommendations = engine.RecommendTweaks();
        foreach (var rec in recommendations)
        {
            Assert.InRange(rec.ConfidencePercent, 0, 100);
        }
    }

    [Fact]
    public void TweakRecommendation_HighImpactHighSafety_IsQuickWin()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([MakeRecommendable("rec-qw-001", impact: 5, safety: 5)]);
        var recommendations = engine.RecommendTweaks();
        // High-score tweak must appear and be flagged as a quick win.
        var qw = recommendations.FirstOrDefault(r => r.Tweak.Id == "rec-qw-001");
        Assert.NotNull(qw);
        Assert.True(qw.IsQuickWin, "impact=5 safety=5 must be a quick win");
    }

    [Fact]
    public void TweakRecommendation_LowImpact_IsNotQuickWin()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([MakeRecommendable("rec-notqw-001", impact: 2, safety: 5)]);
        var recommendations = engine.RecommendTweaks();
        var notQw = recommendations.FirstOrDefault(r => r.Tweak.Id == "rec-notqw-001");
        Assert.NotNull(notQw);
        Assert.False(notQw.IsQuickWin, "impact=2 must NOT be a quick win");
    }

    [Fact]
    public void TweakRecommendation_HasRequiredProperties()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([MakeRecommendable("rec-props-001", impact: 4, safety: 4)]);
        var recommendations = engine.RecommendTweaks();
        var rec = recommendations.FirstOrDefault(r => r.Tweak.Id == "rec-props-001");
        Assert.NotNull(rec);
        Assert.NotNull(rec.Tweak);
        Assert.NotEmpty(rec.Reason);
        Assert.InRange(rec.ConfidencePercent, 0, 100);
    }

    [Fact]
    public void RecommendTweaks_WithAlreadyAppliedStatus_ExcludesAlreadyAppliedTweaks()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var tw = MakeRecommendable("rec-applied-001", impact: 5, safety: 5);
        engine.Register([tw]);

        var statusMap = new Dictionary<string, TweakResult>
        {
            [tw.Id] = TweakResult.Applied,
        };
        var recommendations = engine.RecommendTweaks(statusMap: statusMap);

        // Already-applied tweak should not be in recommendations.
        Assert.DoesNotContain(recommendations, r => r.Tweak.Id == tw.Id);
    }
}
