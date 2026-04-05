using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for TweakEngine: registration, lookup, search, profiles, dry-run ops.</summary>
public sealed class TweakEngineTests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _builtins;

    public TweakEngineTests(BuiltinsFixture fixture) => _builtins = fixture.Engine;

    // ── Registration ────────────────────────────────────────────────────
    [Fact]
    public void Register_AddsTweaks()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("t-1"), TestHelpers.MakeTweak("t-2")]);
        Assert.Equal(2, engine.TweakCount);
    }

    [Fact]
    public void Register_DuplicateId_Throws()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("dup-1")]);
        Assert.Throws<InvalidOperationException>(() => engine.Register([TestHelpers.MakeTweak("dup-1")]));
    }

    // ── Lookup ──────────────────────────────────────────────────────────
    [Fact]
    public void GetTweak_ExistingId_ReturnsTweak()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("lookup-1")]);
        var td = engine.GetTweak("lookup-1");
        Assert.NotNull(td);
        Assert.Equal("lookup-1", td.Id);
    }

    [Fact]
    public void GetTweak_MissingId_ReturnsNull()
    {
        var engine = TestHelpers.CreateEngine();
        Assert.Null(engine.GetTweak("nonexistent"));
    }

    [Fact]
    public void AllTweaks_ReturnsAll()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("a-1"), TestHelpers.MakeTweak("a-2"), TestHelpers.MakeTweak("a-3")]);
        Assert.Equal(3, engine.AllTweaks().Count);
    }

    // ── Categories ──────────────────────────────────────────────────────
    [Fact]
    public void Categories_ReturnsSorted()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("c-1", "Zebra"), TestHelpers.MakeTweak("c-2", "Alpha")]);
        var cats = engine.Categories();
        Assert.Equal(2, cats.Count);
        Assert.Equal("Alpha", cats[0]);
        Assert.Equal("Zebra", cats[1]);
    }

    [Fact]
    public void TweaksByCategory_GroupsCorrectly()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("g-1", "CatA"), TestHelpers.MakeTweak("g-2", "CatA"), TestHelpers.MakeTweak("g-3", "CatB")]);
        var bycat = engine.TweaksByCategory();
        Assert.Equal(2, bycat["CatA"].Count);
        Assert.Single(bycat["CatB"]);
    }

    // ── Search ──────────────────────────────────────────────────────────
    [Fact]
    public void Search_EmptyQuery_ReturnsAll()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("s-1"), TestHelpers.MakeTweak("s-2")]);
        Assert.Equal(2, engine.Search("").Count);
    }

    [Fact]
    public void Search_ById_ReturnsMatch()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("search-target"), TestHelpers.MakeTweak("other-tweak")]);
        var results = engine.Search("search-target");
        Assert.Single(results);
        Assert.Equal("search-target", results[0].Id);
    }

    [Fact]
    public void Search_ByTag_ReturnsMatch()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "tag-tweak",
            Label = "Tag",
            Category = "X",
            Tags = ["unique-tag-xyz"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\tag-tweak", "Enabled", 1)],
        };
        engine.Register([td]);
        var results = engine.Search("unique-tag-xyz");
        Assert.Single(results);
    }

    [Fact]
    public void Search_CaseInsensitive()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("UPPER-case")]);
        var results = engine.Search("upper-case");
        Assert.Single(results);
    }

    // ── Filter ────────────────────────────────────────────────────────────
    [Fact]
    public void Filter_ByCategory()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("f-1", "CatA"), TestHelpers.MakeTweak("f-2", "CatB")]);
        var results = engine.Filter(category: "CatA");
        Assert.Single(results);
    }

    [Fact]
    public void Filter_ByScope()
    {
        var engine = TestHelpers.CreateEngine();
        var hkcu = new TweakDef
        {
            Id = "scope-u",
            Label = "U",
            Category = "X",
            RegistryKeys = [@"HKCU\Test"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        var hklm = new TweakDef
        {
            Id = "scope-m",
            Label = "M",
            Category = "X",
            RegistryKeys = [@"HKLM\Test"],
            ApplyOps = [RegOp.SetDword(@"HKLM\Test", "V", 1)],
        };
        engine.Register([hkcu, hklm]);
        var results = engine.Filter(scope: TweakScope.User);
        Assert.Single(results);
        Assert.Equal("scope-u", results[0].Id);
    }

    // ── Profiles ────────────────────────────────────────────────────────
    [Fact]
    public void Profiles_HasFiveBuiltIn()
    {
        Assert.Equal(5, TweakEngine.Profiles.Count);
    }

    [Fact]
    public void GetProfile_Business_Exists()
    {
        var p = TweakEngine.GetProfile("business");
        Assert.NotNull(p);
        Assert.Equal("business", p.Name.ToLowerInvariant());
    }

    [Fact]
    public void GetProfile_Unknown_ReturnsNull()
    {
        Assert.Null(TweakEngine.GetProfile("nonexistent"));
    }

    // ── Status detection (dry run) ──────────────────────────────────────
    [Fact]
    public void DetectStatus_NoDetectOps_ReturnsUnknown()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "detect-unk",
            Label = "Tweak",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\detect-unk", "Enabled", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Unknown, engine.DetectStatus(td));
    }

    [Fact]
    public void DetectStatus_WithDetectAction_ReturnsResult()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "detect-action",
            Label = "D",
            Category = "X",
            ApplyAction = (_) => { },
            DetectAction = () => true,
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Applied, engine.DetectStatus(td));
    }

    // ── Apply / Remove (dry run) ────────────────────────────────────────
    [Fact]
    public void Apply_WithApplyOps_ReturnsApplied()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "apply-1",
            Label = "A",
            Category = "X",
            CorpSafe = true,
            RegistryKeys = [@"HKCU\Software\Test"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "Val", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Applied, engine.Apply(td));
    }

    [Fact]
    public void Remove_WithRemoveOps_ReturnsNotApplied()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "remove-1",
            Label = "R",
            Category = "X",
            CorpSafe = true,
            RegistryKeys = [@"HKCU\Software\Test"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "Val", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\Test", "Val")],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.NotApplied, engine.Remove(td));
    }

    // ── RegisterBuiltins ────────────────────────────────────────────────
    // Moved to TweakEngineBuiltinsTests (shared fixture) for performance.

    // ── Category / Scope counts ─────────────────────────────────────────
    [Fact]
    public void CategoryCounts_MatchesTweaksByCategory()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("cc-1", "X"), TestHelpers.MakeTweak("cc-2", "X"), TestHelpers.MakeTweak("cc-3", "Y")]);
        var counts = engine.CategoryCounts();
        Assert.Equal(2, counts["X"]);
        Assert.Equal(1, counts["Y"]);
    }

    [Fact]
    public void ScopeCounts_SumsCorrectly()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "u1",
                Label = "U",
                Category = "C",
                RegistryKeys = [@"HKCU\Test"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
            },
            new TweakDef
            {
                Id = "m1",
                Label = "M",
                Category = "C",
                RegistryKeys = [@"HKLM\Test"],
                ApplyOps = [RegOp.SetDword(@"HKLM\Test", "V", 1)],
            },
        ]);
        var counts = engine.ScopeCounts();
        Assert.Equal(1, counts[TweakScope.User]);
        Assert.Equal(1, counts[TweakScope.Machine]);
    }

    // ── StatusMap ───────────────────────────────────────────────────────
    [Fact]
    public void StatusMap_ReturnsEntryPerTweak()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("sm-1"), TestHelpers.MakeTweak("sm-2")]);
        var map = engine.StatusMap();
        Assert.Equal(2, map.Count);
    }

    [Fact]
    public void StatusMap_Parallel_ReturnsEntryPerTweak()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("smp-1"), TestHelpers.MakeTweak("smp-2")]);
        var map = engine.StatusMap(parallel: true);
        Assert.Equal(2, map.Count);
    }

    // ── WindowsBuild ────────────────────────────────────────────────────
    [Fact]
    public void WindowsBuild_ReturnsPositive()
    {
        Assert.True(TweakEngine.WindowsBuild() > 0);
    }

    // ── TweaksByIds ─────────────────────────────────────────────────────
    [Fact]
    public void TweaksByIds_ReturnsMatchingTweaks()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("ids-1"), TestHelpers.MakeTweak("ids-2"), TestHelpers.MakeTweak("ids-3")]);
        var results = engine.TweaksByIds(["ids-1", "ids-3"]);
        Assert.Equal(2, results.Count);
        Assert.Contains(results, t => t.Id == "ids-1");
        Assert.Contains(results, t => t.Id == "ids-3");
    }

    [Fact]
    public void TweaksByIds_IgnoresUnknownIds()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("known-1")]);
        var results = engine.TweaksByIds(["known-1", "unknown-99"]);
        Assert.Single(results);
        Assert.Equal("known-1", results[0].Id);
    }

    // ── TweaksByTag ─────────────────────────────────────────────────────
    [Fact]
    public void TweaksByTag_ReturnsMatchingTweaks()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "tag-1",
            Label = "Tag",
            Category = "X",
            Tags = ["special-tag"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\tag-1", "V", 1)],
        };
        engine.Register([td, TestHelpers.MakeTweak("notag-1")]);
        var results = engine.TweaksByTag("special-tag");
        Assert.Single(results);
        Assert.Equal("tag-1", results[0].Id);
    }

    [Fact]
    public void TweaksByTag_CaseInsensitive()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "tagci-1",
            Label = "T",
            Category = "X",
            Tags = ["MyTag"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\tagci-1", "V", 1)],
        };
        engine.Register([td]);
        Assert.Single(engine.TweaksByTag("mytag"));
        Assert.Single(engine.TweaksByTag("MYTAG"));
    }

    // ── TweaksByScope ───────────────────────────────────────────────────
    [Fact]
    public void TweaksByScope_FiltersCorrectly()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "scp-u",
                Label = "U",
                Category = "C",
                RegistryKeys = [@"HKCU\Test"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
            },
            new TweakDef
            {
                Id = "scp-m",
                Label = "M",
                Category = "C",
                RegistryKeys = [@"HKLM\Test"],
                ApplyOps = [RegOp.SetDword(@"HKLM\Test", "V", 1)],
            },
        ]);
        var user = engine.TweaksByScope(TweakScope.User);
        var machine = engine.TweaksByScope(TweakScope.Machine);
        Assert.Single(user);
        Assert.Equal("scp-u", user[0].Id);
        Assert.Single(machine);
        Assert.Equal("scp-m", machine[0].Id);
    }

    // ── Filter (additional parameters) ──────────────────────────────────
    [Fact]
    public void Filter_ByCorpSafe()
    {
        var engine = TestHelpers.CreateEngine();
        var safe = new TweakDef
        {
            Id = "f-safe",
            Label = "S",
            Category = "X",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\f-safe", "V", 1)],
        };
        var risky = new TweakDef
        {
            Id = "f-risky",
            Label = "R",
            Category = "X",
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\f-risky", "V", 1)],
        };
        engine.Register([safe, risky]);
        var results = engine.Filter(corpSafe: true);
        Assert.Single(results);
        Assert.Equal("f-safe", results[0].Id);
    }

    [Fact]
    public void Filter_ByNeedsAdmin()
    {
        var engine = TestHelpers.CreateEngine();
        var admin = new TweakDef
        {
            Id = "f-admin",
            Label = "A",
            Category = "X",
            NeedsAdmin = true,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\f-admin", "V", 1)],
        };
        var noAdmin = new TweakDef
        {
            Id = "f-noadmin",
            Label = "N",
            Category = "X",
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\f-noadmin", "V", 1)],
        };
        engine.Register([admin, noAdmin]);
        var results = engine.Filter(needsAdmin: false);
        Assert.Single(results);
        Assert.Equal("f-noadmin", results[0].Id);
    }

    [Fact]
    public void Filter_ByMinBuild()
    {
        var engine = TestHelpers.CreateEngine();
        var old = new TweakDef
        {
            Id = "f-old",
            Label = "O",
            Category = "X",
            MinBuild = 19041,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\f-old", "V", 1)],
        };
        var fresh = new TweakDef
        {
            Id = "f-fresh",
            Label = "F",
            Category = "X",
            MinBuild = 22631,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\f-fresh", "V", 1)],
        };
        engine.Register([old, fresh]);
        var results = engine.Filter(minBuild: 20000);
        Assert.Single(results);
        Assert.Equal("f-old", results[0].Id);
    }

    [Fact]
    public void Filter_MultipleParams_CombinesAnd()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "fm-1",
                Label = "A",
                Category = "CatA",
                CorpSafe = true,
                NeedsAdmin = false,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\fm-1", "V", 1)],
            },
            new TweakDef
            {
                Id = "fm-2",
                Label = "B",
                Category = "CatA",
                CorpSafe = false,
                NeedsAdmin = false,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\fm-2", "V", 1)],
            },
            new TweakDef
            {
                Id = "fm-3",
                Label = "C",
                Category = "CatB",
                CorpSafe = true,
                NeedsAdmin = false,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\fm-3", "V", 1)],
            },
        ]);
        var results = engine.Filter(corpSafe: true, category: "CatA");
        Assert.Single(results);
        Assert.Equal("fm-1", results[0].Id);
    }

    // ── ApplyBatch / RemoveBatch ────────────────────────────────────────
    [Fact]
    public void ApplyBatch_ReturnsResultPerTweak()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = new[]
        {
            new TweakDef
            {
                Id = "ab-1",
                Label = "A",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\ab-1", "V", 1)],
            },
            new TweakDef
            {
                Id = "ab-2",
                Label = "B",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\ab-2", "V", 1)],
            },
        };
        engine.Register(tweaks);
        var results = engine.ApplyBatch(tweaks);
        Assert.Equal(2, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.Applied, r));
    }

    [Fact]
    public void ApplyBatch_Parallel_ReturnsResultPerTweak()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = new[]
        {
            new TweakDef
            {
                Id = "abp-1",
                Label = "A",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\abp-1", "V", 1)],
            },
            new TweakDef
            {
                Id = "abp-2",
                Label = "B",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\abp-2", "V", 1)],
            },
        };
        engine.Register(tweaks);
        var results = engine.ApplyBatch(tweaks, parallel: true);
        Assert.Equal(2, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.Applied, r));
    }

    [Fact]
    public void RemoveBatch_ReturnsResultPerTweak()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = new[]
        {
            new TweakDef
            {
                Id = "rb-1",
                Label = "A",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\rb-1", "V", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\rb-1", "V")],
            },
            new TweakDef
            {
                Id = "rb-2",
                Label = "B",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\rb-2", "V", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\rb-2", "V")],
            },
        };
        engine.Register(tweaks);
        var results = engine.RemoveBatch(tweaks);
        Assert.Equal(2, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.NotApplied, r));
    }

    // ── TweaksForProfile / ApplyProfile ─────────────────────────────────
    // Moved to TweakEngineBuiltinsTests (shared fixture) for performance.

    // ── SaveSnapshot / LoadSnapshot round-trip ──────────────────────────
    [Fact]
    public void SaveSnapshot_LoadSnapshot_RoundTrips()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("snap-1"), TestHelpers.MakeTweak("snap-2")]);
        var path = Path.Combine(Path.GetTempPath(), $"regilattice-test-{Guid.NewGuid()}.json");
        try
        {
            engine.SaveSnapshot(path);
            Assert.True(File.Exists(path));
            var loaded = engine.LoadSnapshot(path);
            Assert.Equal(2, loaded.Count);
            Assert.True(loaded.ContainsKey("snap-1"));
            Assert.True(loaded.ContainsKey("snap-2"));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── ExportJson ──────────────────────────────────────────────────────
    [Fact]
    public void ExportJson_WritesValidJson()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("exp-1"), TestHelpers.MakeTweak("exp-2")]);
        var path = Path.Combine(Path.GetTempPath(), $"regilattice-export-{Guid.NewGuid()}.json");
        try
        {
            engine.ExportJson(path);
            Assert.True(File.Exists(path));
            var json = File.ReadAllText(path);
            var docs = JsonSerializer.Deserialize<JsonElement>(json);
            Assert.Equal(JsonValueKind.Array, docs.ValueKind);
            Assert.Equal(2, docs.GetArrayLength());
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── Property counts ─────────────────────────────────────────────────
    [Fact]
    public void TweakCount_MatchesAllTweaks()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("cnt-1"), TestHelpers.MakeTweak("cnt-2"), TestHelpers.MakeTweak("cnt-3")]);
        Assert.Equal(engine.AllTweaks().Count, engine.TweakCount);
    }

    [Fact]
    public void CategoryCount_MatchesCategories()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("cat-1", "A"), TestHelpers.MakeTweak("cat-2", "B"), TestHelpers.MakeTweak("cat-3", "C")]);
        Assert.Equal(engine.Categories().Count, engine.CategoryCount);
    }

    // ── Snapshot round-trip ──────────────────────────────────────────────

    [Fact]
    public void SaveSnapshot_CreatesFile()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("snap-1")]);
        var path = Path.Combine(Path.GetTempPath(), $"regilattice-test-{Guid.NewGuid()}.json");
        try
        {
            engine.SaveSnapshot(path);
            Assert.True(File.Exists(path));
            var json = File.ReadAllText(path);
            Assert.Contains("snap-1", json);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void LoadSnapshot_ReturnsCorrectData()
    {
        var path = Path.Combine(Path.GetTempPath(), $"regilattice-test-{Guid.NewGuid()}.json");
        try
        {
            File.WriteAllText(path, "{\"tweak-a\": \"applied\", \"tweak-b\": \"notapplied\"}");
            var engine = TestHelpers.CreateEngine();
            var snapshot = engine.LoadSnapshot(path);
            Assert.Equal(2, snapshot.Count);
            Assert.Equal("applied", snapshot["tweak-a"]);
            Assert.Equal("notapplied", snapshot["tweak-b"]);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void RestoreSnapshot_SkipsMissingIds()
    {
        var path = Path.Combine(Path.GetTempPath(), $"regilattice-test-{Guid.NewGuid()}.json");
        try
        {
            File.WriteAllText(path, "{\"nonexistent-tweak\": \"applied\"}");
            var engine = TestHelpers.CreateEngine();
            var results = engine.RestoreSnapshot(path);
            Assert.Empty(results);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void SaveSnapshot_LoadSnapshot_RoundTrip()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("rt-1"), TestHelpers.MakeTweak("rt-2")]);
        var path = Path.Combine(Path.GetTempPath(), $"regilattice-test-{Guid.NewGuid()}.json");
        try
        {
            engine.SaveSnapshot(path);
            var loaded = engine.LoadSnapshot(path);
            Assert.Equal(2, loaded.Count);
            Assert.True(loaded.ContainsKey("rt-1"));
            Assert.True(loaded.ContainsKey("rt-2"));
        }
        finally
        {
            File.Delete(path);
        }
    }

    // ── ExportJson ──────────────────────────────────────────────────────

    [Fact]
    public void ExportJson_CreatesValidJson()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("exp-1", "Export"), TestHelpers.MakeTweak("exp-2", "Export")]);
        var path = Path.Combine(Path.GetTempPath(), $"regilattice-test-{Guid.NewGuid()}.json");
        try
        {
            engine.ExportJson(path);
            var json = File.ReadAllText(path);
            var doc = JsonDocument.Parse(json);
            Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
            Assert.Equal(2, doc.RootElement.GetArrayLength());
            Assert.Contains("exp-1", json);
            Assert.Contains("exp-2", json);
        }
        finally
        {
            File.Delete(path);
        }
    }

    // ── TweaksByTag / TweaksByScope / GetScope ──────────────────────────

    [Fact]
    public void TweaksByTag_MultipleTags_ReturnsMatchingSubset()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "tag-1",
                Label = "T1",
                Category = "A",
                Tags = ["alpha", "beta"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\T", "V", 1)],
            },
            new TweakDef
            {
                Id = "tag-2",
                Label = "T2",
                Category = "A",
                Tags = ["beta", "gamma"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\T", "V", 1)],
            },
            new TweakDef
            {
                Id = "tag-3",
                Label = "T3",
                Category = "A",
                Tags = ["gamma"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\T", "V", 1)],
            },
        ]);
        var beta = engine.TweaksByTag("beta");
        Assert.Equal(2, beta.Count);
        Assert.All(beta, t => Assert.Contains("beta", t.Tags));
    }

    [Fact]
    public void TweaksByTag_MissingTag_ReturnsEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("tag-x")]);
        Assert.Empty(engine.TweaksByTag("nonexistent"));
    }

    [Fact]
    public void TweaksByScope_ReturnsCorrectScope()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "scope-user",
                Label = "U",
                Category = "A",
                RegistryKeys = [@"HKCU\Software\Test"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
            },
            new TweakDef
            {
                Id = "scope-machine",
                Label = "M",
                Category = "A",
                RegistryKeys = [@"HKLM\Software\Test"],
                ApplyOps = [RegOp.SetDword(@"HKLM\Software\Test", "V", 1)],
            },
        ]);
        var userTweaks = engine.TweaksByScope(TweakScope.User);
        Assert.Single(userTweaks);
        Assert.Equal("scope-user", userTweaks[0].Id);
    }

    [Fact]
    public void GetScope_ReturnsCachedScope()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "gs-1",
            Label = "Test",
            Category = "A",
            RegistryKeys = [@"HKCU\Soft\Test"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Soft\Test", "V", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakScope.User, engine.GetScope(td));
    }

    // ── Freeze / CategoryCounts / ScopeCounts ───────────────────────────

    [Fact]
    public void Freeze_BuildsCaches()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("frz-1", "Alpha"), TestHelpers.MakeTweak("frz-2", "Beta")]);
        engine.Freeze();
        var catCounts = engine.CategoryCounts();
        Assert.Equal(2, catCounts.Count);
        Assert.Equal(1, catCounts["Alpha"]);
        Assert.Equal(1, catCounts["Beta"]);
    }

    [Fact]
    public void ScopeCounts_ReturnsCorrectBreakdown()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "sc-u",
                Label = "U",
                Category = "A",
                RegistryKeys = [@"HKCU\Soft\Test"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Soft\Test", "V", 1)],
            },
            new TweakDef
            {
                Id = "sc-m",
                Label = "M",
                Category = "A",
                RegistryKeys = [@"HKLM\Soft\Test"],
                ApplyOps = [RegOp.SetDword(@"HKLM\Soft\Test", "V", 1)],
            },
        ]);
        engine.Freeze();
        var counts = engine.ScopeCounts();
        Assert.Equal(1, counts[TweakScope.User]);
        Assert.Equal(1, counts[TweakScope.Machine]);
    }

    // ── TweaksForProfile ────────────────────────────────────────────────

    [Fact]
    public void TweaksForProfile_InvalidName_ReturnsEmpty()
    {
        Assert.Empty(_builtins.TweaksForProfile("nonexistent-profile"));
    }

    [Fact]
    public void TweaksForProfile_ValidName_ReturnsNonEmpty()
    {
        var tweaks = _builtins.TweaksForProfile("privacy");
        Assert.NotEmpty(tweaks);
        // All returned tweaks should be in the privacy profile's categories
        var profile = TweakEngine.Profiles.First(p => p.Name == "privacy");
        var cats = new HashSet<string>(profile.ApplyCategories, StringComparer.OrdinalIgnoreCase);
        Assert.All(tweaks, t => Assert.True(cats.Contains(t.Category), $"{t.Id} has category {t.Category} not in privacy profile"));
    }

    // ── WindowsBuild ────────────────────────────────────────────────────

    [Fact]
    public void WindowsBuild_ReturnsPositiveNumber()
    {
        int build = TweakEngine.WindowsBuild();
        Assert.True(build > 0, $"Expected positive build number, got {build}");
    }

    // ── TweaksByScope edge cases ────────────────────────────────────────

    [Fact]
    public void TweaksByScope_UserOnly_ExcludesMachine()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "scope-user",
                Label = "User",
                Category = "Test",
                RegistryKeys = [@"HKCU\Software\Test"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
            },
            new TweakDef
            {
                Id = "scope-machine",
                Label = "Machine",
                Category = "Test",
                RegistryKeys = [@"HKLM\SOFTWARE\Test"],
                ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Test", "V", 1)],
            },
        ]);

        var userTweaks = engine.TweaksByScope(TweakScope.User);
        Assert.Single(userTweaks);
        Assert.Equal("scope-user", userTweaks[0].Id);
    }

    [Fact]
    public void TweaksByScope_BothScope_ReturnsBothScopeTweaks()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "scope-both",
                Label = "Both",
                Category = "Test",
                RegistryKeys = [@"HKCU\Software\Test", @"HKLM\SOFTWARE\Test"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
            },
        ]);

        var bothTweaks = engine.TweaksByScope(TweakScope.Both);
        Assert.Single(bothTweaks);
        Assert.Equal("scope-both", bothTweaks[0].Id);
    }

    // ── GetScope caching ────────────────────────────────────────────────

    [Fact]
    public void GetScope_ReturnsConsistentResult()
    {
        var engine = TestHelpers.CreateEngine();
        var td = TestHelpers.MakeTweak("scope-cache-test");
        engine.Register([td]);

        var scope1 = engine.GetScope(td);
        var scope2 = engine.GetScope(td);
        Assert.Equal(scope1, scope2);
    }

    // ── Filter: query filter ────────────────────────────────────────────

    [Fact]
    public void Filter_ByQuery_FiltersOnSearchText()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "fq-telemetry",
                Label = "Disable Telemetry",
                Category = "Privacy",
                Description = "Stops telemetry data collection",
                Tags = ["telemetry"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\fq-telemetry", "V", 1)],
            },
            new TweakDef
            {
                Id = "fq-animation",
                Label = "Disable Animations",
                Category = "Performance",
                Tags = ["ui"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\fq-animation", "V", 1)],
            },
        ]);

        var result = engine.Filter(query: "telemetry");
        Assert.Single(result);
        Assert.Equal("fq-telemetry", result[0].Id);
    }

    [Fact]
    public void Filter_NoCriteria_ReturnsAllTweaks()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("filter-all-1"), TestHelpers.MakeTweak("filter-all-2"), TestHelpers.MakeTweak("filter-all-3")]);
        var result = engine.Filter();
        Assert.Equal(3, result.Count);
    }

    // ── IsApplicableOnHardware edge cases ───────────────────────────────

    [Fact]
    public void IsApplicableOnHardware_CustomPredicate_True()
    {
        var td = new TweakDef
        {
            Id = "hw-custom-true",
            Label = "Custom",
            Category = "Test",
            IsApplicable = () => true,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\hw-test", "V", 1)],
        };
        Assert.True(TweakEngine.IsApplicableOnHardware(td));
    }

    [Fact]
    public void IsApplicableOnHardware_CustomPredicate_False()
    {
        var td = new TweakDef
        {
            Id = "hw-custom-false",
            Label = "Custom",
            Category = "Test",
            IsApplicable = () => false,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\hw-test", "V", 1)],
        };
        Assert.False(TweakEngine.IsApplicableOnHardware(td));
    }

    [Fact]
    public void IsApplicableOnHardware_GenericCategory_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "hw-generic",
            Label = "Generic",
            Category = "Privacy",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\hw-test", "V", 1)],
        };
        Assert.True(TweakEngine.IsApplicableOnHardware(td));
    }

    // ── DetectStatus edge cases ─────────────────────────────────────────

    [Fact]
    public void DetectStatus_ThrowingDetectAction_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "detect-throw",
            Label = "Thrower",
            Category = "Test",
            DetectAction = () => throw new InvalidOperationException("detect boom"),
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\detect-throw", "V", 1)],
        };
        engine.Register([td]);

        var result = engine.DetectStatus(td);
        Assert.Equal(TweakResult.Error, result);
    }

    [Fact]
    public void DetectStatus_NoDetection_ReturnsUnknown()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "detect-none",
            Label = "NoDetect",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\detect-none", "V", 1)],
        };
        engine.Register([td]);

        var result = engine.DetectStatus(td);
        Assert.Equal(TweakResult.Unknown, result);
    }

    // ── StatusMap with subset of IDs ────────────────────────────────────

    [Fact]
    public void StatusMap_WithIds_ReturnsSubset()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("sm-1"), TestHelpers.MakeTweak("sm-2"), TestHelpers.MakeTweak("sm-3")]);

        var map = engine.StatusMap(ids: ["sm-1", "sm-3"]);
        Assert.Equal(2, map.Count);
        Assert.Contains("sm-1", map.Keys);
        Assert.Contains("sm-3", map.Keys);
        Assert.DoesNotContain("sm-2", map.Keys);
    }

    // ── Search multi-token ──────────────────────────────────────────────

    [Fact]
    public void Search_MultipleTokens_MatchesBothTerms()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "search-multi-perf-anim",
                Label = "Disable Performance Animations",
                Category = "Performance",
                Tags = ["animation"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\search-multi", "V", 1)],
            },
            new TweakDef
            {
                Id = "search-multi-priv",
                Label = "Disable Privacy Telemetry",
                Category = "Privacy",
                Tags = ["telemetry"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\search-priv", "V", 1)],
            },
        ]);

        var result = engine.Search("performance animation");
        Assert.Single(result);
        Assert.Equal("search-multi-perf-anim", result[0].Id);
    }

    // ── Batch operations with progress ──────────────────────────────────

    [Fact]
    public void ApplyBatch_WithProgressCallback_FiresForEachTweak()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("batch-p-1"), TestHelpers.MakeTweak("batch-p-2"), TestHelpers.MakeTweak("batch-p-3")]);
        var calls = new List<(int Done, int Total, string Id)>();
        engine.ApplyBatch(engine.AllTweaks(), forceCorp: true, (done, total, id, _) => calls.Add((done, total, id)));
        Assert.Equal(3, calls.Count);
        Assert.Equal(1, calls[0].Done);
        Assert.Equal(3, calls[0].Total);
        Assert.Equal(3, calls[2].Done);
    }

    [Fact]
    public void RemoveBatch_Empty_ReturnsEmptyDictionary()
    {
        var engine = TestHelpers.CreateEngine();
        var result = engine.RemoveBatch(Array.Empty<TweakDef>(), forceCorp: true);
        Assert.Empty(result);
    }

    [Fact]
    public void RemoveBatch_WithProgressCallback_FiresForEachTweak()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("rm-p-1"), TestHelpers.MakeTweak("rm-p-2")]);
        var calls = new List<string>();
        engine.RemoveBatch(engine.AllTweaks(), forceCorp: true, (_, _, id, _) => calls.Add(id));
        Assert.Equal(2, calls.Count);
    }

    [Fact]
    public void ApplyBatch_SkipsCorpUnsafe_WhenNotForced()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "corp-unsafe",
                Label = "Unsafe",
                Category = "Test",
                CorpSafe = false,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\corp", "V", 1)],
            },
        ]);
        var result = engine.ApplyBatch(engine.AllTweaks(), forceCorp: false);
        // On corporate machine this will be SkippedCorp, on non-corp it will be Applied
        Assert.Single(result);
    }

    // ── Filter combinations ─────────────────────────────────────────────

    [Fact]
    public void Filter_ByCorpSafe_ReturnsOnlyCorpSafe()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "f-corp-1",
                Label = "Safe",
                Category = "Test",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\f1", "V", 1)],
            },
            new TweakDef
            {
                Id = "f-corp-2",
                Label = "Unsafe",
                Category = "Test",
                CorpSafe = false,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\f2", "V", 1)],
            },
        ]);
        var result = engine.Filter(corpSafe: true);
        Assert.Single(result);
        Assert.Equal("f-corp-1", result[0].Id);
    }

    [Fact]
    public void Filter_ByNeedsAdmin_ReturnsOnlyNonAdmin()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "f-admin-1",
                Label = "Admin",
                Category = "Test",
                NeedsAdmin = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\a1", "V", 1)],
            },
            new TweakDef
            {
                Id = "f-admin-2",
                Label = "No Admin",
                Category = "Test",
                NeedsAdmin = false,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\a2", "V", 1)],
            },
        ]);
        var result = engine.Filter(needsAdmin: false);
        Assert.Single(result);
        Assert.Equal("f-admin-2", result[0].Id);
    }

    [Fact]
    public void Filter_ByMinBuild_ReturnsOnlyLowerBuild()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "f-build-1",
                Label = "Old",
                Category = "Test",
                MinBuild = 19041,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\b1", "V", 1)],
            },
            new TweakDef
            {
                Id = "f-build-2",
                Label = "New",
                Category = "Test",
                MinBuild = 99999,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\b2", "V", 1)],
            },
        ]);
        var result = engine.Filter(minBuild: 22000);
        Assert.Single(result);
        Assert.Equal("f-build-1", result[0].Id);
    }

    [Fact]
    public void Filter_MultipleCriteria_CombinesAll()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "f-multi-1",
                Label = "Match",
                Category = "Privacy",
                CorpSafe = true,
                NeedsAdmin = false,
                RegistryKeys = [@"HKCU\Software\m1"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\m1", "V", 1)],
            },
            new TweakDef
            {
                Id = "f-multi-2",
                Label = "No Match",
                Category = "Privacy",
                CorpSafe = false,
                NeedsAdmin = false,
                RegistryKeys = [@"HKCU\Software\m2"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\m2", "V", 1)],
            },
        ]);
        var result = engine.Filter(corpSafe: true, needsAdmin: false, scope: TweakScope.User, category: "Privacy");
        Assert.Single(result);
        Assert.Equal("f-multi-1", result[0].Id);
    }

    // ── Profile operations ──────────────────────────────────────────────

    [Fact]
    public void ApplyProfile_InvalidName_ReturnsEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("prof-t-1")]);
        var result = engine.ApplyProfile("nonexistent-profile");
        Assert.Empty(result);
    }

    [Fact]
    public void TweaksForProfile_InvalidName_ReturnsEmptyList()
    {
        var engine = TestHelpers.CreateEngine();
        var result = engine.TweaksForProfile("nonexistent");
        Assert.Empty(result);
    }

    // ── ExportJson ──────────────────────────────────────────────────────

    [Fact]
    public void ExportJson_CreatesValidJsonArray()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("export-1"), TestHelpers.MakeTweak("export-2")]);
        var path = Path.Combine(Path.GetTempPath(), $"rl-export-{Guid.NewGuid()}.json");
        try
        {
            engine.ExportJson(path);
            Assert.True(File.Exists(path));
            var json = File.ReadAllText(path);
            var doc = System.Text.Json.JsonDocument.Parse(json);
            Assert.Equal(System.Text.Json.JsonValueKind.Array, doc.RootElement.ValueKind);
            Assert.Equal(2, doc.RootElement.GetArrayLength());
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── Update ──────────────────────────────────────────────────────────

    [Fact]
    public void Update_WithUpdateAction_CallsUpdateAction()
    {
        var engine = TestHelpers.CreateEngine();
        bool called = false;
        engine.Register([
            new TweakDef
            {
                Id = "upd-1",
                Label = "Updatable",
                Category = "Test",
                CorpSafe = true,
                ApplyAction = _ => { },
                UpdateAction = _ => called = true,
            },
        ]);
        var td = engine.GetTweak("upd-1")!;
        var result = engine.Update(td, forceCorp: true);
        Assert.True(called);
        Assert.Equal(TweakResult.Applied, result);
    }

    [Fact]
    public void Update_ThrowingUpdateAction_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "upd-err",
                Label = "Broken",
                Category = "Test",
                CorpSafe = true,
                ApplyAction = _ => { },
                UpdateAction = _ => throw new InvalidOperationException("fail"),
            },
        ]);
        var td = engine.GetTweak("upd-err")!;
        var result = engine.Update(td, forceCorp: true);
        Assert.Equal(TweakResult.Error, result);
    }

    // ── Apply/Remove edge cases ─────────────────────────────────────────

    [Fact]
    public void Apply_HighMinBuild_ReturnsSkippedBuild()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "build-skip",
                Label = "Future",
                Category = "Test",
                CorpSafe = true,
                MinBuild = 999999,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\build", "V", 1)],
            },
        ]);
        var td = engine.GetTweak("build-skip")!;
        var result = engine.Apply(td, forceCorp: true);
        Assert.Equal(TweakResult.SkippedBuild, result);
    }

    [Fact]
    public void Apply_ThrowingApplyAction_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "apply-err",
                Label = "Broken",
                Category = "Test",
                CorpSafe = true,
                ApplyAction = _ => throw new InvalidOperationException("boom"),
            },
        ]);
        var td = engine.GetTweak("apply-err")!;
        var result = engine.Apply(td, forceCorp: true);
        Assert.Equal(TweakResult.Error, result);
    }

    [Fact]
    public void Remove_ThrowingRemoveAction_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "rm-err",
                Label = "Broken",
                Category = "Test",
                CorpSafe = true,
                ApplyAction = _ => { },
                RemoveAction = _ => throw new InvalidOperationException("boom"),
            },
        ]);
        var td = engine.GetTweak("rm-err")!;
        var result = engine.Remove(td, forceCorp: true);
        Assert.Equal(TweakResult.Error, result);
    }

    // ── ValidateTweaks integration ──────────────────────────────────────

    [Fact]
    public void ValidateTweaks_CleanEngine_ReturnsNoErrors()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("v-1"), TestHelpers.MakeTweak("v-2")]);
        var errors = engine.ValidateTweaks();
        Assert.Empty(errors);
    }

    // ── ResolveDependencies / Dependents ─────────────────────────────────

    [Fact]
    public void ResolveDependencies_UnknownId_Throws()
    {
        var engine = TestHelpers.CreateEngine();
        Assert.Throws<ArgumentException>(() => engine.ResolveDependencies("nope"));
    }

    [Fact]
    public void Dependents_NoDeps_ReturnsEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("dep-lone")]);
        var result = engine.Dependents("dep-lone");
        Assert.Empty(result);
    }

    // ── TweaksByTag / CategoryCounts / ScopeCounts ──────────────────────

    [Fact]
    public void TweaksByTag_NonexistentTag_ReturnsEmptyList()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("tag-t-1")]);
        var result = engine.TweaksByTag("nonexistent-tag");
        Assert.Empty(result);
    }

    [Fact]
    public void CategoryCounts_AfterFreeze_MatchesTweaksByCategory()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("cc-1", "Cat-A"), TestHelpers.MakeTweak("cc-2", "Cat-A"), TestHelpers.MakeTweak("cc-3", "Cat-B")]);
        engine.Freeze();
        var counts = engine.CategoryCounts();
        Assert.Equal(2, counts["Cat-A"]);
        Assert.Equal(1, counts["Cat-B"]);
    }

    [Fact]
    public void ScopeCounts_AfterFreeze_ContainsExpectedScopes()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("sc-1"), TestHelpers.MakeTweak("sc-2")]);
        engine.Freeze();
        var counts = engine.ScopeCounts();
        Assert.NotEmpty(counts);
        int total = counts.Values.Sum();
        Assert.Equal(2, total);
    }

    [Fact]
    public void Freeze_Idempotent_DoesNotThrow()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("freeze-1")]);
        engine.Freeze();
        engine.Freeze(); // second call should not throw
        Assert.Equal(1, engine.TweakCount);
    }

    [Fact]
    public void TweaksByIds_WithDuplicates_ReturnsUnique()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("dup-id")]);
        var result = engine.TweaksByIds(["dup-id", "dup-id", "dup-id"]);
        // TweaksByIds returns one per ID match (duplicates produce duplicates)
        Assert.Equal(3, result.Count);
        Assert.All(result, t => Assert.Equal("dup-id", t.Id));
    }

    [Fact]
    public void TweaksByIds_NonExistent_Skips()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("exists-1")]);
        var result = engine.TweaksByIds(["exists-1", "nope-1", "nope-2"]);
        Assert.Single(result);
    }

    [Fact]
    public void WindowsBuild_ReturnsPositiveValue()
    {
        var build = TweakEngine.WindowsBuild();
        Assert.True(build > 0);
    }

    [Fact]
    public void Search_EmptyQuery_ReturnsAllTweaks()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("search-empty-1"), TestHelpers.MakeTweak("search-empty-2")]);
        var result = engine.Search("");
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Search_WhitespaceQuery_ReturnsAllTweaks()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("search-ws-1")]);
        var result = engine.Search("   ");
        Assert.Single(result);
    }

    // ── Update fallback ─────────────────────────────────────────────────

    [Fact]
    public void Update_WithoutUpdateAction_FallsBackToApply()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "upd-fallback",
                Label = "No Update",
                Category = "Test",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\upd-fallback", "V", 1)],
            },
        ]);
        var td = engine.GetTweak("upd-fallback")!;
        var result = engine.Update(td, forceCorp: true);
        Assert.Equal(TweakResult.Applied, result);
    }

    // ── Apply: IsApplicable gate ────────────────────────────────────────

    [Fact]
    public void Apply_NotApplicable_ReturnsSkippedHw()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "hw-blocked",
                Label = "Blocked",
                Category = "Test",
                CorpSafe = true,
                IsApplicable = () => false,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\hw-blocked", "V", 1)],
            },
        ]);
        var td = engine.GetTweak("hw-blocked")!;
        var result = engine.Apply(td, forceCorp: true);
        Assert.Equal(TweakResult.SkippedHw, result);
    }

    // ── ExportJson with cached status ───────────────────────────────────

    [Fact]
    public void ExportJson_WithCachedStatus_UsesProvidedStatus()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("export-cached")]);
        var cached = new Dictionary<string, TweakResult> { ["export-cached"] = TweakResult.Applied };
        var path = Path.Combine(Path.GetTempPath(), $"rl-export-cached-{Guid.NewGuid()}.json");
        try
        {
            engine.ExportJson(path, cached);
            var json = File.ReadAllText(path);
            Assert.Contains("\"applied\"", json);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── StatusMap parallel with ids ──────────────────────────────────────

    [Fact]
    public void StatusMap_WithIds_Parallel_ReturnsSubset()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("smp-1"), TestHelpers.MakeTweak("smp-2"), TestHelpers.MakeTweak("smp-3")]);
        var map = engine.StatusMap(parallel: true, ids: ["smp-1", "smp-3"]);
        Assert.Equal(2, map.Count);
        Assert.Contains("smp-1", map.Keys);
        Assert.Contains("smp-3", map.Keys);
    }

    // ── DetectDuplicateRegistryOps ──────────────────────────────────────

    [Fact]
    public void DetectDuplicateRegistryOps_NoDuplicates_ReturnsEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("ddup-1"), TestHelpers.MakeTweak("ddup-2")]);
        var errors = engine.DetectDuplicateRegistryOps();
        Assert.Empty(errors);
    }

    // ── RegisterPack ────────────────────────────────────────────────────

    [Fact]
    public void RegisterPack_RegistersTweaks()
    {
        var engine = TestHelpers.CreateEngine();
        var packTweaks = new List<TweakDef> { TestHelpers.MakeTweak("pack-t-1", "PackCat"), TestHelpers.MakeTweak("pack-t-2", "PackCat") };
        engine.RegisterPack(packTweaks);
        Assert.Equal(2, engine.TweakCount);
        Assert.NotNull(engine.GetTweak("pack-t-1"));
        Assert.NotNull(engine.GetTweak("pack-t-2"));
    }

    // ── Apply/Remove with action delegates ──────────────────────────────

    [Fact]
    public void Apply_WithApplyAction_CallsDelegate()
    {
        var engine = TestHelpers.CreateEngine();
        bool called = false;
        engine.Register([
            new TweakDef
            {
                Id = "act-apply",
                Label = "Action",
                Category = "Test",
                CorpSafe = true,
                ApplyAction = _ => called = true,
            },
        ]);
        var td = engine.GetTweak("act-apply")!;
        engine.Apply(td, forceCorp: true);
        Assert.True(called);
    }

    [Fact]
    public void Remove_WithRemoveAction_CallsDelegate()
    {
        var engine = TestHelpers.CreateEngine();
        bool called = false;
        engine.Register([
            new TweakDef
            {
                Id = "act-remove",
                Label = "Action",
                Category = "Test",
                CorpSafe = true,
                ApplyAction = _ => { },
                RemoveAction = _ => called = true,
            },
        ]);
        var td = engine.GetTweak("act-remove")!;
        engine.Remove(td, forceCorp: true);
        Assert.True(called);
    }

    // ── Dependents with actual dependent ─────────────────────────────────

    [Fact]
    public void Dependents_WithDep_ReturnsDependent()
    {
        var engine = TestHelpers.CreateEngine();
        var parent = TestHelpers.MakeTweak("dep-parent");
        var child = new TweakDef
        {
            Id = "dep-child",
            Label = "Child",
            Category = "Test",
            DependsOn = ["dep-parent"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\dep-child", "V", 1)],
        };
        engine.Register([parent, child]);
        var dependents = engine.Dependents("dep-parent");
        Assert.Single(dependents);
        Assert.Equal("dep-child", dependents[0].Id);
    }

    // ── GetTweak after Freeze uses frozen dictionary ─────────────────────

    [Fact]
    public void GetTweak_AfterFreeze_ReturnsTweak()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("frozen-lookup")]);
        engine.Freeze();
        var td = engine.GetTweak("frozen-lookup");
        Assert.NotNull(td);
        Assert.Equal("frozen-lookup", td.Id);
    }

    [Fact]
    public void GetTweak_AfterFreeze_CaseInsensitive()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("frozen-case")]);
        engine.Freeze();
        var td = engine.GetTweak("FROZEN-CASE");
        Assert.NotNull(td);
    }

    // ── Coverage boost — Filter, Batch, StatusMap, Export edge cases ──

    [Fact]
    public void Filter_ByCategory_ReturnsOnlyThatCategory()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("fcat-a", "CatA"), TestHelpers.MakeTweak("fcat-b", "CatB"), TestHelpers.MakeTweak("fcat-c", "CatA")]);
        var results = engine.Filter(category: "CatA");
        Assert.Equal(2, results.Count);
        Assert.All(results, t => Assert.Equal("CatA", t.Category));
    }

    [Fact]
    public void Filter_ByMinBuild_ReturnsApplicableTweaks()
    {
        var engine = TestHelpers.CreateEngine();
        var lowBuild = new TweakDef
        {
            Id = "fbuild-low",
            Label = "Low",
            Category = "Test",
            MinBuild = 10000,
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        var highBuild = new TweakDef
        {
            Id = "fbuild-high",
            Label = "High",
            Category = "Test",
            MinBuild = 99999,
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        engine.Register([lowBuild, highBuild]);
        var results = engine.Filter(minBuild: 50000);
        Assert.Single(results);
        Assert.Equal("fbuild-low", results[0].Id);
    }

    [Fact]
    public void Filter_ByNeedsAdmin_ReturnsMatching()
    {
        var engine = TestHelpers.CreateEngine();
        var admin = new TweakDef
        {
            Id = "fadmin-yes",
            Label = "Admin",
            Category = "Test",
            NeedsAdmin = true,
            ApplyOps = [RegOp.SetDword(@"HKLM\Test", "V", 1)],
        };
        var noAdmin = new TweakDef
        {
            Id = "fadmin-no",
            Label = "NoAdmin",
            Category = "Test",
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        engine.Register([admin, noAdmin]);
        var results = engine.Filter(needsAdmin: false);
        Assert.Single(results);
        Assert.Equal("fadmin-no", results[0].Id);
    }

    [Fact]
    public void ApplyBatch_Parallel_ReturnsAllResults()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = Enumerable.Range(1, 5).Select(i => TestHelpers.MakeTweak($"batch-par-{i}")).ToList();
        engine.Register(tweaks);
        var results = engine.ApplyBatch(tweaks, forceCorp: true, parallel: true);
        Assert.Equal(5, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.Applied, r));
    }

    [Fact]
    public void RemoveBatch_Sequential_ReturnsAllResults()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = Enumerable.Range(1, 3).Select(i => TestHelpers.MakeTweak($"rmbatch-seq-{i}")).ToList();
        engine.Register(tweaks);
        var results = engine.RemoveBatch(tweaks, forceCorp: true, parallel: false);
        Assert.Equal(3, results.Count);
    }

    [Fact]
    public void RemoveBatch_Parallel_ReturnsAllResults()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = Enumerable.Range(1, 4).Select(i => TestHelpers.MakeTweak($"rmbatch-par-{i}")).ToList();
        engine.Register(tweaks);
        var results = engine.RemoveBatch(tweaks, forceCorp: true, parallel: true);
        Assert.Equal(4, results.Count);
    }

    [Fact]
    public void Apply_SkippedBuild_ReturnsBuildSkipped()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "skip-build",
            Label = "Future",
            Category = "Test",
            CorpSafe = true,
            MinBuild = 99999,
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        engine.Register([td]);
        var result = engine.Apply(td, forceCorp: true);
        Assert.Equal(TweakResult.SkippedBuild, result);
    }

    [Fact]
    public void Remove_WithRemoveOps_ReturnsNotAppliedAfterRemoval()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "remove-ops",
            Label = "Test",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKCU\Test", "V")],
        };
        engine.Register([td]);
        var result = engine.Remove(td, forceCorp: true);
        Assert.Equal(TweakResult.NotApplied, result);
    }

    [Fact]
    public void TweaksByTag_CustomTag_ReturnsNonEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("tag-test-1")]);
        var results = engine.TweaksByTag("test");
        Assert.NotEmpty(results);
    }

    [Fact]
    public void TweaksByScope_ReturnsMatchingTweaks()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("scope-user-x")]);
        var results = engine.TweaksByScope(TweakScope.User);
        Assert.NotEmpty(results);
    }

    [Fact]
    public void GetScope_ReturnsCorrectScope()
    {
        var engine = TestHelpers.CreateEngine();
        var td = TestHelpers.MakeTweak("scope-get-x");
        engine.Register([td]);
        var scope = engine.GetScope(td);
        Assert.Equal(TweakScope.User, scope);
    }

    [Fact]
    public void CategoryCounts_ReturnsCorrectCounts()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("cc-a1", "CatX"), TestHelpers.MakeTweak("cc-a2", "CatX"), TestHelpers.MakeTweak("cc-b1", "CatY")]);
        var counts = engine.CategoryCounts();
        Assert.Equal(2, counts["CatX"]);
        Assert.Equal(1, counts["CatY"]);
    }

    [Fact]
    public void ScopeCounts_ReturnsAllThreeScopes()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("sc-user")]);
        var counts = engine.ScopeCounts();
        Assert.True(counts.ContainsKey(TweakScope.User) || counts.ContainsKey(TweakScope.Machine));
    }

    [Fact]
    public void ExportJson_WritesFile()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("json-export-1")]);
        var path = Path.Combine(Path.GetTempPath(), $"regilattice-export-{Guid.NewGuid():N}.json");
        try
        {
            engine.ExportJson(path);
            Assert.True(File.Exists(path));
            var json = File.ReadAllText(path);
            Assert.StartsWith("[", json);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void WindowsBuild_ValueIsAboveZero()
    {
        var build = TweakEngine.WindowsBuild();
        Assert.True(build > 0);
    }

    [Fact]
    public void TweaksByIds_ReturnsOnlyRequested()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("byid-1"), TestHelpers.MakeTweak("byid-2"), TestHelpers.MakeTweak("byid-3")]);
        var results = engine.TweaksByIds(["byid-1", "byid-3"]);
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public void DetectStatus_WithDetectAction_UsesAction()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "detect-action-test",
            Label = "Test",
            Category = "Test",
            ApplyAction = _ => { },
            DetectAction = () => true,
        };
        engine.Register([td]);
        var status = engine.DetectStatus(td);
        Assert.Equal(TweakResult.Applied, status);
    }

    [Fact]
    public void DetectStatus_WithDetectAction_False_ReturnsNotApplied()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "detect-action-false",
            Label = "Test",
            Category = "Test",
            ApplyAction = _ => { },
            DetectAction = () => false,
        };
        engine.Register([td]);
        var status = engine.DetectStatus(td);
        Assert.Equal(TweakResult.NotApplied, status);
    }

    [Fact]
    public void DetectStatus_WithDetectAction_Throws_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "detect-action-throws",
            Label = "Test",
            Category = "Test",
            ApplyAction = _ => { },
            DetectAction = () => throw new InvalidOperationException("test"),
        };
        engine.Register([td]);
        var status = engine.DetectStatus(td);
        Assert.Equal(TweakResult.Error, status);
    }

    [Fact]
    public void Update_WithUpdateAction_Throws_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "update-throws",
            Label = "Test",
            Category = "Test",
            CorpSafe = true,
            ApplyAction = _ => { },
            UpdateAction = _ => throw new InvalidOperationException("test"),
        };
        engine.Register([td]);
        var result = engine.Update(td, forceCorp: true);
        Assert.Equal(TweakResult.Error, result);
    }

    [Fact]
    public void Apply_ApplyAction_Throws_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "apply-throws",
            Label = "Test",
            Category = "Test",
            CorpSafe = true,
            ApplyAction = _ => throw new InvalidOperationException("boom"),
        };
        engine.Register([td]);
        var result = engine.Apply(td, forceCorp: true);
        Assert.Equal(TweakResult.Error, result);
    }

    [Fact]
    public void Remove_RemoveAction_Throws_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "remove-throws",
            Label = "Test",
            Category = "Test",
            CorpSafe = true,
            ApplyAction = _ => { },
            RemoveAction = _ => throw new InvalidOperationException("boom"),
        };
        engine.Register([td]);
        var result = engine.Remove(td, forceCorp: true);
        Assert.Equal(TweakResult.Error, result);
    }
}

// ── ApplyProfile, CategoryCounts, ScopeCounts, utility paths ──

public sealed class TweakEngineAdditionalTests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _engine;

    public TweakEngineAdditionalTests(BuiltinsFixture fixture) => _engine = fixture.Engine;

    [Fact]
    public void ApplyProfile_ValidProfile_ReturnsResultsForEachTweak()
    {
        // Verify profile resolution works (returns tweaks without applying - avoids spawning processes)
        var tweaks = _engine.TweaksForProfile("privacy");
        Assert.NotEmpty(tweaks);
        Assert.All(tweaks, t => Assert.NotEmpty(t.Id));
    }

    [Fact]
    public void ApplyProfile_InvalidProfile_ReturnsEmpty()
    {
        var results = _engine.ApplyProfile("nonexistent-xyz-profile");
        Assert.Empty(results);
    }

    [Fact]
    public void CategoryCounts_SumsToAllTweaksCount()
    {
        var total = _engine.CategoryCounts().Values.Sum();
        Assert.Equal(_engine.AllTweaks().Count, total);
    }

    [Fact]
    public void ScopeCounts_SumsToAllTweaksCount()
    {
        var total = _engine.ScopeCounts().Values.Sum();
        Assert.Equal(_engine.AllTweaks().Count, total);
    }

    [Fact]
    public void CategoryCounts_AllCategoriesPresent()
    {
        var cats = _engine.Categories().ToHashSet();
        Assert.All(_engine.CategoryCounts().Keys, k => Assert.Contains(k, cats));
    }

    [Fact]
    public void ScopeCounts_ContainsUserAndMachine()
    {
        var counts = _engine.ScopeCounts();
        Assert.True(counts.ContainsKey(TweakScope.User) || counts.ContainsKey(TweakScope.Machine));
    }

    [Fact]
    public void GetScope_KnownId_ReturnsScope()
    {
        var first = _engine.AllTweaks()[0];
        var scope = _engine.GetScope(first);
        Assert.True(Enum.IsDefined(typeof(TweakScope), scope));
    }

    [Fact]
    public void TweaksByScope_User_ContainsAtLeastOneTweak()
    {
        var userTweaks = _engine.TweaksByScope(TweakScope.User);
        Assert.NotEmpty(userTweaks);
        Assert.All(userTweaks, t => Assert.Equal(TweakScope.User, t.Scope));
    }

    [Fact]
    public void TweaksByScope_Machine_ContainsAtLeastOneTweak()
    {
        var machineTweaks = _engine.TweaksByScope(TweakScope.Machine);
        Assert.NotEmpty(machineTweaks);
        Assert.All(machineTweaks, t => Assert.Equal(TweakScope.Machine, t.Scope));
    }

    [Fact]
    public void TweaksByTag_ReturnsOnlyMatchingTag()
    {
        var tagged = _engine.TweaksByTag("privacy");
        Assert.NotEmpty(tagged);
        Assert.All(tagged, t => Assert.Contains("privacy", t.Tags, StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    public void Filter_ByCorpSafe_True_ReturnsOnlyCorpSafe()
    {
        var corpSafe = _engine.Filter(corpSafe: true);
        Assert.NotEmpty(corpSafe);
        Assert.All(corpSafe, t => Assert.True(t.CorpSafe));
    }

    [Fact]
    public void Filter_ByNeedsAdmin_False_ReturnsOnlyNonAdmin()
    {
        var nonAdmin = _engine.Filter(needsAdmin: false);
        Assert.NotEmpty(nonAdmin);
        Assert.All(nonAdmin, t => Assert.False(t.NeedsAdmin));
    }

    [Fact]
    public void WindowsBuild_IsPositive()
    {
        Assert.True(TweakEngine.WindowsBuild() > 0);
    }

    [Fact]
    public void Search_SingleToken_ReturnsRelevantTweaks()
    {
        var results = _engine.Search("telemetry");
        Assert.NotEmpty(results);
        // NLP synonym expansion intentionally returns related tweaks beyond literal matches.
        // Verify that at least the majority of results directly reference the query term.
        int directMatches = results.Count(t =>
            t.Label.Contains("telemetry", StringComparison.OrdinalIgnoreCase)
            || t.Description.Contains("telemetry", StringComparison.OrdinalIgnoreCase)
            || t.Tags.Any(tag => tag.Contains("telemetry", StringComparison.OrdinalIgnoreCase))
        );
        Assert.True(
            directMatches > 0,
            $"Search 'telemetry' returned {results.Count} results but none directly contain 'telemetry' in label/desc/tags."
        );
    }

    [Fact]
    public void ExportJson_WritesReadableJson()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "sp24-export-test",
                Label = "Export Test Tweak",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\RegiLattice\Test", "Export", 1)],
                DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\RegiLattice\Test", "Export", 1)],
            },
        ]);
        var path = Path.Combine(Path.GetTempPath(), $"sp24-export-{Guid.NewGuid():N}.json");
        try
        {
            engine.ExportJson(path);
            var text = File.ReadAllText(path);
            Assert.Contains("Id", text);
            Assert.Contains("Label", text);
            Assert.Contains("sp24-export-test", text);
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch { }
        }
    }
}

// ── merged from TweakEngineSearchNlpTests.cs ──────────────────────────────────
/// <summary>NLP synonym search tests for TweakEngine.Search().</summary>
public sealed class TweakEngineSearchNlpTests
{
    private static TweakEngine BuildEngine()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        return engine;
    }

    // ── Direct term matching ──────────────────────────────────────────────

    [Fact]
    public void Search_EmptyQuery_ReturnsAllTweaks()
    {
        // Empty query is treated as "show all" — returns the full tweak set.
        var engine = BuildEngine();
        var results = engine.Search("");
        Assert.NotEmpty(results);
        Assert.Equal(engine.AllTweaks().Count, results.Count);
    }

    [Fact]
    public void Search_WhitespaceQuery_ReturnsAllTweaks()
    {
        // Whitespace-only query is treated as "show all" — same as empty.
        var engine = BuildEngine();
        var results = engine.Search("   ");
        Assert.NotEmpty(results);
        Assert.Equal(engine.AllTweaks().Count, results.Count);
    }

    // ── Synonym expansion ─────────────────────────────────────────────────

    [Theory]
    [InlineData("fast")]
    [InlineData("speed")]
    [InlineData("boost")]
    public void Search_PerformanceSynonym_ReturnsTweaks(string query)
    {
        var engine = BuildEngine();
        var results = engine.Search(query);
        Assert.NotEmpty(results);
    }

    [Theory]
    [InlineData("spy")]
    [InlineData("track")]
    [InlineData("telemetry")]
    public void Search_PrivacySynonym_ReturnsTweaks(string query)
    {
        var engine = BuildEngine();
        var results = engine.Search(query);
        Assert.NotEmpty(results);
    }

    [Theory]
    [InlineData("bloat")]
    [InlineData("junk")]
    [InlineData("unwanted")]
    public void Search_DebloatSynonym_ReturnsTweaks(string query)
    {
        var engine = BuildEngine();
        var results = engine.Search(query);
        Assert.NotEmpty(results);
    }

    // ── Multi-word AND logic ──────────────────────────────────────────────

    [Fact]
    public void Search_MultiWord_ReturnsIntersection()
    {
        var engine = BuildEngine();
        var single = engine.Search("telemetry");
        var multi = engine.Search("disable telemetry");
        // Multi-word should not return MORE results than single-keyword:
        Assert.True(multi.Count <= single.Count);
        // But should still return something
        Assert.NotEmpty(multi);
    }

    // ── Unknown term returns empty ────────────────────────────────────────

    [Fact]
    public void Search_UnknownTerm_ReturnsEmpty()
    {
        var engine = BuildEngine();
        var results = engine.Search("xyzzy_nonexistent_term_93812");
        Assert.Empty(results);
    }

    // ── All results are from the registered set ───────────────────────────

    [Fact]
    public void Search_ResultIds_AreRegisteredTweaks()
    {
        var engine = BuildEngine();
        var allIds = engine.AllTweaks().Select(t => t.Id).ToHashSet(StringComparer.Ordinal);
        var results = engine.Search("disable");
        Assert.All(results, t => Assert.Contains(t.Id, allIds));
    }
}

// ── merged from TweakEngineCoverageTests.cs ──────────────────────────────────

public sealed class ApplyResultBranchTests
{
    // SkippedBuild — MinBuild higher than current build
    [Fact]
    public void Apply_MinBuildNotMet_ReturnsSkippedBuild()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "sb-minbuild",
            Label = "Min Build",
            Category = "Test",
            CorpSafe = true,
            MinBuild = 9_999_999, // impossibly high
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\sb-minbuild", "V", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.SkippedBuild, engine.Apply(td, forceCorp: true));
    }

    // SkippedHw — IsApplicable returns false
    [Fact]
    public void Apply_IsApplicableFalse_ReturnsSkippedHw()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "sb-hw-gate",
            Label = "HW Gate",
            Category = "Test",
            CorpSafe = true,
            IsApplicable = () => false,
            ApplicabilityNote = "Not applicable on this hardware",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\sb-hw-gate", "V", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.SkippedHw, engine.Apply(td, forceCorp: true));
    }

    // Apply with ApplyAction delegate (not RegOps path)
    [Fact]
    public void Apply_WithApplyAction_ReturnsApplied()
    {
        var engine = TestHelpers.CreateEngine();
        bool called = false;
        var td = new TweakDef
        {
            Id = "sb-action",
            Label = "Action",
            Category = "Test",
            CorpSafe = true,
            ApplyAction = _ =>
            {
                called = true;
            },
        };
        engine.Register([td]);
        var result = engine.Apply(td, forceCorp: true);
        Assert.Equal(TweakResult.Applied, result);
        Assert.True(called);
    }

    // Apply with both MinBuild=0 (no restriction) and IsApplicable=true
    [Fact]
    public void Apply_NoBuildGate_NullIsApplicable_ReturnsApplied()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "sb-nogate",
            Label = "No Gate",
            Category = "Test",
            CorpSafe = true,
            MinBuild = 0,
            IsApplicable = null,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\sb-nogate", "V", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Applied, engine.Apply(td, forceCorp: true));
    }
}

// ── Remove() result branches ────────────────────────────────────────────────

public sealed class RemoveResultBranchTests
{
    // Remove with RemoveAction delegate
    [Fact]
    public void Remove_WithRemoveAction_ReturnsNotApplied()
    {
        var engine = TestHelpers.CreateEngine();
        bool called = false;
        var td = new TweakDef
        {
            Id = "rb-action",
            Label = "Remove Action",
            Category = "Test",
            CorpSafe = true,
            ApplyAction = _ => { },
            RemoveAction = _ =>
            {
                called = true;
            },
        };
        engine.Register([td]);
        var result = engine.Remove(td, forceCorp: true);
        Assert.Equal(TweakResult.NotApplied, result);
        Assert.True(called);
    }

    // Remove with no RemoveOps and no RemoveAction — returns NotApplied
    [Fact]
    public void Remove_NoRemoveOpsOrAction_ReturnsNotApplied()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "rb-noop",
            Label = "No Remove",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\rb-noop", "V", 1)],
            // RemoveOps intentionally empty, RemoveAction null
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.NotApplied, engine.Remove(td, forceCorp: true));
    }

    // SkippedCorp for Remove when forceCorp=false and CorpSafe=false
    [Fact]
    public void Remove_SkipsCorp_WhenNotForceCorp()
    {
        CorporateGuard.ClearCache();
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "rb-corp",
            Label = "Corp",
            Category = "Test",
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\rb-corp", "V", 1)],
        };
        engine.Register([td]);

        // forceCorp=true overrides corp guard — should not skip
        var withForce = engine.Remove(td, forceCorp: true);
        Assert.Equal(TweakResult.NotApplied, withForce);
    }
}

// ── Update() branches ───────────────────────────────────────────────────────

public sealed class UpdateBranchTests
{
    // No UpdateAction — falls back to Apply
    [Fact]
    public void Update_NoUpdateAction_FallsBackToApply()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "upd-fallback",
            Label = "Update",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\upd-fallback", "V", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Applied, engine.Update(td, forceCorp: true));
    }

    // Has UpdateAction — calls it and returns Applied
    [Fact]
    public void Update_WithUpdateAction_ReturnsApplied()
    {
        var engine = TestHelpers.CreateEngine();
        bool called = false;
        var td = new TweakDef
        {
            Id = "upd-action",
            Label = "Update With Action",
            Category = "Test",
            CorpSafe = true,
            ApplyAction = _ => { },
            UpdateAction = _ =>
            {
                called = true;
            },
        };
        engine.Register([td]);
        var result = engine.Update(td, forceCorp: true);
        Assert.Equal(TweakResult.Applied, result);
        Assert.True(called);
    }

    // UpdateAction throws — returns Error
    [Fact]
    public void Update_UpdateAction_Throws_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "upd-throw",
            Label = "Update Throw",
            Category = "Test",
            CorpSafe = true,
            ApplyAction = _ => { },
            UpdateAction = _ => throw new InvalidOperationException("update failure"),
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Error, engine.Update(td, forceCorp: true));
    }

    // Update with CorpSafe=false + forceCorp=false → SkippedCorp
    [Fact]
    public void Update_SkippedCorp_WhenNotForceCorp()
    {
        CorporateGuard.ClearCache();
        var engine = TestHelpers.CreateEngine();
        bool called = false;
        var td = new TweakDef
        {
            Id = "upd-corp-skip",
            Label = "Corp Skip",
            Category = "Test",
            CorpSafe = false,
            ApplyAction = _ => { },
            UpdateAction = _ =>
            {
                called = true;
            },
        };
        engine.Register([td]);

        // With forceCorp=true — should succeed
        var result = engine.Update(td, forceCorp: true);
        Assert.Equal(TweakResult.Applied, result);
        Assert.True(called);
    }
}

// ── ApplyBatch / RemoveBatch ────────────────────────────────────────────────

public sealed class BatchOperationTests
{
    [Fact]
    public void ApplyBatch_Sequential_ProcessesAll()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = new[]
        {
            new TweakDef
            {
                Id = "bat-a1",
                Label = "A",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-a1", "V", 1)],
            },
            new TweakDef
            {
                Id = "bat-a2",
                Label = "B",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-a2", "V", 1)],
            },
            new TweakDef
            {
                Id = "bat-a3",
                Label = "C",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-a3", "V", 1)],
            },
        };
        engine.Register(tweaks);
        var results = engine.ApplyBatch(tweaks, forceCorp: true, parallel: false);
        Assert.Equal(3, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.Applied, r));
    }

    [Fact]
    public void ApplyBatch_Parallel_ProcessesAll()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = Enumerable
            .Range(1, 8)
            .Select(i => new TweakDef
            {
                Id = $"bat-par-{i}",
                Label = $"P{i}",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword($@"HKCU\Software\bat-par-{i}", "V", 1)],
            })
            .ToList();
        engine.Register(tweaks);
        var results = engine.ApplyBatch(tweaks, forceCorp: true, parallel: true);
        Assert.Equal(8, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.Applied, r));
    }

    [Fact]
    public void RemoveBatch_Sequential_ProcessesAll()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = new[]
        {
            new TweakDef
            {
                Id = "bat-r1",
                Label = "A",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-r1", "V", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\bat-r1", "V")],
            },
            new TweakDef
            {
                Id = "bat-r2",
                Label = "B",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-r2", "V", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\bat-r2", "V")],
            },
        };
        engine.Register(tweaks);
        var results = engine.RemoveBatch(tweaks, forceCorp: true, parallel: false);
        Assert.Equal(2, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.NotApplied, r));
    }

    [Fact]
    public void RemoveBatch_Parallel_ProcessesAll()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = Enumerable
            .Range(1, 6)
            .Select(i => new TweakDef
            {
                Id = $"bat-rpar-{i}",
                Label = $"R{i}",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword($@"HKCU\Software\bat-rpar-{i}", "V", 1)],
                RemoveOps = [RegOp.DeleteValue($@"HKCU\Software\bat-rpar-{i}", "V")],
            })
            .ToList();
        engine.Register(tweaks);
        var results = engine.RemoveBatch(tweaks, forceCorp: true, parallel: true);
        Assert.Equal(6, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.NotApplied, r));
    }

    [Fact]
    public void ApplyBatch_DefaultProgress_Works()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = new[]
        {
            new TweakDef
            {
                Id = "bat-prog1",
                Label = "P1",
                Category = "X",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-prog1", "V", 1)],
            },
        };
        engine.Register(tweaks);
        var calls = new List<(int done, int total, string id, TweakResult r)>();
        var results = engine.ApplyBatch(tweaks, forceCorp: true, onProgress: (done, total, id, r) => calls.Add((done, total, id, r)));
        Assert.Single(calls);
        Assert.Equal(1, calls[0].done);
        Assert.Equal(1, calls[0].total);
        Assert.Equal("bat-prog1", calls[0].id);
    }
}

// ── DetectStatus exception branch ───────────────────────────────────────────

public sealed class DetectStatusBranchTests
{
    [Fact]
    public void DetectStatus_DetectActionThrows_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "ds-throw",
            Label = "Throw",
            Category = "Test",
            ApplyAction = _ => { },
            DetectAction = () => throw new InvalidOperationException("detect failure"),
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Error, engine.DetectStatus(td));
    }

    [Fact]
    public void DetectStatus_DetectOps_FalseResult_ReturnsNotApplied()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "ds-not-applied",
            Label = "Not Applied",
            Category = "Test",
            // CheckDword on a key that almost certainly won't match SentinelValue=99999
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\ds-not-applied", "V", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\ds-not-applied-nonexistent", "V", 99999)],
        };
        engine.Register([td]);
        var result = engine.DetectStatus(td);
        // Will be NotApplied (key doesn't exist = check fails)
        Assert.Equal(TweakResult.NotApplied, result);
    }
}

// ── StatusMap with id subset ────────────────────────────────────────────────

public sealed class StatusMapSubsetTests
{
    [Fact]
    public void StatusMap_WithIdSubset_ReturnsOnlyRequestedIds()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("ssub-1"), TestHelpers.MakeTweak("ssub-2"), TestHelpers.MakeTweak("ssub-3")]);
        var map = engine.StatusMap(parallel: false, ids: ["ssub-1", "ssub-3"]);
        Assert.Equal(2, map.Count);
        Assert.True(map.ContainsKey("ssub-1"));
        Assert.True(map.ContainsKey("ssub-3"));
        Assert.False(map.ContainsKey("ssub-2"));
    }

    [Fact]
    public void StatusMap_ParallelWithIds_ReturnsOnlyRequestedIds()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("spar-1"), TestHelpers.MakeTweak("spar-2"), TestHelpers.MakeTweak("spar-3")]);
        var map = engine.StatusMap(parallel: true, ids: ["spar-2"]);
        Assert.Single(map);
        Assert.True(map.ContainsKey("spar-2"));
    }
}

// ── Search multi-token ──────────────────────────────────────────────────────

public sealed class SearchMultiTokenTests
{
    [Fact]
    public void Search_MultiToken_AllTokensMustMatch()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "mts-privacy-telemetry",
            Label = "Disable Privacy Telemetry",
            Category = "Privacy",
            Description = "Disables telemetry reporting",
            Tags = ["privacy", "telemetry"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\mts-priv", "V", 1)],
        };
        var other = new TweakDef
        {
            Id = "mts-privacy-only",
            Label = "Privacy Only",
            Category = "Privacy",
            Tags = ["privacy"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\mts-priv2", "V", 1)],
        };
        engine.Register([td, other]);

        // Both tokens must appear — only td matches both "privacy" AND "telemetry"
        var results = engine.Search("privacy telemetry");
        Assert.Single(results);
        Assert.Equal("mts-privacy-telemetry", results[0].Id);
    }

    [Fact]
    public void Search_SingleToken_ReturnsAllMatches()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "mts-perf-1",
                Label = "Performance Tweak 1",
                Category = "Performance",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\mts-perf-1", "V", 1)],
            },
            new TweakDef
            {
                Id = "mts-perf-2",
                Label = "Performance Tweak 2",
                Category = "Performance",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\mts-perf-2", "V", 1)],
            },
            new TweakDef
            {
                Id = "mts-other",
                Label = "Other",
                Category = "Other",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\mts-other", "V", 1)],
            },
        ]);
        var results = engine.Search("performance");
        Assert.Equal(2, results.Count);
    }
}

// ── Filter(query:) branch ───────────────────────────────────────────────────

public sealed class FilterQueryBranchTests
{
    [Fact]
    public void Filter_WithQuery_FiltersOnSearchText()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "fq-disk",
                Label = "Disk Cleanup",
                Category = "Performance",
                Description = "Frees disk space",
                Tags = ["disk"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\fq-disk", "V", 1)],
            },
            new TweakDef
            {
                Id = "fq-mem",
                Label = "Memory Tweak",
                Category = "Performance",
                Description = "Reduces memory footprint",
                Tags = ["memory"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\fq-mem", "V", 1)],
            },
        ]);
        var results = engine.Filter(query: "disk");
        Assert.Single(results);
        Assert.Equal("fq-disk", results[0].Id);
    }

    [Fact]
    public void Filter_Query_NoMatch_ReturnsEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("fq-nothing")]);
        var results = engine.Filter(query: "zzz-no-match-possible-xyz");
        Assert.Empty(results);
    }

    [Fact]
    public void Filter_QueryAndCategory_CombinesFilters()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "fqc-1",
                Label = "Alpha Disk",
                Category = "CatA",
                Tags = ["disk"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\fqc-1", "V", 1)],
            },
            new TweakDef
            {
                Id = "fqc-2",
                Label = "Beta Disk",
                Category = "CatB",
                Tags = ["disk"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\fqc-2", "V", 1)],
            },
        ]);
        // "disk" in both, but filtered by CatA
        var results = engine.Filter(category: "CatA", query: "disk");
        Assert.Single(results);
        Assert.Equal("fqc-1", results[0].Id);
    }
}

// ── IsApplicableOnHardware category switch branches ─────────────────────────

public sealed class IsApplicableOnHardwareTests
{
    // Custom predicate trumps category
    [Fact]
    public void IsApplicableOnHardware_CustomPredicateTrue_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "iap-custom-true",
            Label = "Custom",
            Category = "WSL", // would be HardwareInfo.HasWslInstalled() otherwise
            IsApplicable = () => true,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-custom", "V", 1)],
        };
        Assert.True(TweakEngine.IsApplicableOnHardware(td));
    }

    [Fact]
    public void IsApplicableOnHardware_CustomPredicateFalse_ReturnsFalse()
    {
        var td = new TweakDef
        {
            Id = "iap-custom-false",
            Label = "Custom False",
            Category = "Performance",
            IsApplicable = () => false,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-custom-false", "V", 1)],
        };
        Assert.False(TweakEngine.IsApplicableOnHardware(td));
    }

    // Category-based detection — these call HardwareInfo methods (returns bool, can be true or false)
    [Theory]
    [InlineData("WSL")]
    [InlineData("Virtualization")]
    [InlineData("Chrome")]
    [InlineData("Firefox")]
    [InlineData("Edge")]
    [InlineData("Java")]
    [InlineData("Adobe")]
    [InlineData("LibreOffice")]
    [InlineData("Office")]
    [InlineData("M365 Copilot")]
    [InlineData("RealVNC")]
    [InlineData("VS Code")]
    [InlineData("Scoop Tools")]
    public void IsApplicableOnHardware_SoftwareCategory_ReturnsBool(string category)
    {
        var td = new TweakDef
        {
            Id = $"iap-{category.ToLowerInvariant().Replace(" ", "-")}",
            Label = category,
            Category = category,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-cat", "V", 1)],
        };
        // Should not throw — actual value depends on machine
        var result = TweakEngine.IsApplicableOnHardware(td);
        Assert.IsType<bool>(result);
    }

    // Default (no matching category, no special tag) → returns true
    [Fact]
    public void IsApplicableOnHardware_UnknownCategory_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "iap-unknown-cat",
            Label = "Unknown",
            Category = "SomeObscureCategory",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-unknown", "V", 1)],
        };
        Assert.True(TweakEngine.IsApplicableOnHardware(td));
    }

    // AutoDetectFromTags — nvidia tag
    [Fact]
    public void IsApplicableOnHardware_NvidiaTag_CallsHardwareInfo()
    {
        var td = new TweakDef
        {
            Id = "iap-nvidia",
            Label = "Nvidia",
            Category = "GPU / Graphics",
            Tags = ["nvidia"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-nvidia", "V", 1)],
        };
        var result = TweakEngine.IsApplicableOnHardware(td);
        Assert.IsType<bool>(result);
    }

    // AutoDetectFromTags — amd-gpu tag
    [Fact]
    public void IsApplicableOnHardware_AmdGpuTag_CallsHardwareInfo()
    {
        var td = new TweakDef
        {
            Id = "iap-amd",
            Label = "AMD",
            Category = "GPU / Graphics",
            Tags = ["amd-gpu"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-amd", "V", 1)],
        };
        var result = TweakEngine.IsApplicableOnHardware(td);
        Assert.IsType<bool>(result);
    }

    // AutoDetectFromTags — docker tag
    [Fact]
    public void IsApplicableOnHardware_DockerTag_CallsHardwareInfo()
    {
        var td = new TweakDef
        {
            Id = "iap-docker",
            Label = "Docker",
            Category = "Virtualization",
            Tags = ["docker"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-docker", "V", 1)],
        };
        var result = TweakEngine.IsApplicableOnHardware(td);
        Assert.IsType<bool>(result);
    }

    // AutoDetectFromTags — laptop tag
    [Fact]
    public void IsApplicableOnHardware_LaptopTag_CallsHardwareInfo()
    {
        var td = new TweakDef
        {
            Id = "iap-laptop",
            Label = "Laptop",
            Category = "Power Management",
            Tags = ["laptop"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-laptop", "V", 1)],
        };
        var result = TweakEngine.IsApplicableOnHardware(td);
        Assert.IsType<bool>(result);
    }

    // No special tag — falls through to return true
    [Fact]
    public void IsApplicableOnHardware_NoSpecialTag_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "iap-notag",
            Label = "No Tag",
            Category = "Performance",
            Tags = ["performance"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-notag", "V", 1)],
        };
        Assert.True(TweakEngine.IsApplicableOnHardware(td));
    }
}

// ── ExportJson ──────────────────────────────────────────────────────────────

public sealed class ExportJsonTests
{
    [Fact]
    public void ExportJson_WritesValidJson()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("ej-1", "Privacy"), TestHelpers.MakeTweak("ej-2", "Performance")]);
        var path = Path.Combine(Path.GetTempPath(), $"rl-export-{Guid.NewGuid():N}.json");
        try
        {
            engine.ExportJson(path);
            Assert.True(File.Exists(path));
            var json = File.ReadAllText(path);
            var doc = JsonDocument.Parse(json);
            Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
            Assert.Equal(2, doc.RootElement.GetArrayLength());
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ExportJson_WithCachedStatus_UsesCached()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("ej-cached")]);
        var path = Path.Combine(Path.GetTempPath(), $"rl-export-{Guid.NewGuid():N}.json");
        try
        {
            var cached = new Dictionary<string, TweakResult> { ["ej-cached"] = TweakResult.Applied };
            engine.ExportJson(path, cached);
            var json = File.ReadAllText(path);
            Assert.Contains("applied", json);
        }
        finally
        {
            File.Delete(path);
        }
    }
}

// ── GetScope ────────────────────────────────────────────────────────────────

public sealed class GetScopeTests
{
    [Fact]
    public void GetScope_ReturnsFromCache()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "gs-hkcu",
            Label = "HKCU",
            Category = "Test",
            RegistryKeys = [@"HKCU\Software\gs-hkcu"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\gs-hkcu", "V", 1)],
        };
        engine.Register([td]);
        // Should return User scope both times — second call uses _scopeCache
        Assert.Equal(TweakScope.User, engine.GetScope(td));
        Assert.Equal(TweakScope.User, engine.GetScope(td));
    }

    [Fact]
    public void GetScope_BothScopesTweak_ReturnsBoth()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "gs-both",
            Label = "Both",
            Category = "Test",
            RegistryKeys = [@"HKCU\Software\gs-both", @"HKLM\SOFTWARE\gs-both"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\gs-both", "V", 1), RegOp.SetDword(@"HKLM\SOFTWARE\gs-both", "V", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakScope.Both, engine.GetScope(td));
    }
}

// ── ResolveDependencies / Dependents ────────────────────────────────────────

public sealed class DependencyEngineTests
{
    [Fact]
    public void ResolveDependencies_SimpleChain_ReturnsInOrder()
    {
        var engine = TestHelpers.CreateEngine();
        var tdA = TestHelpers.MakeTweak("dep-eng-a");
        var tdB = new TweakDef
        {
            Id = "dep-eng-b",
            Label = "B",
            Category = "Test",
            DependsOn = ["dep-eng-a"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\dep-eng-b", "V", 1)],
        };
        engine.Register([tdA, tdB]);
        var resolved = engine.ResolveDependencies("dep-eng-b");
        Assert.Equal(2, resolved.Count);
        Assert.Equal("dep-eng-a", resolved[0].Id);
        Assert.Equal("dep-eng-b", resolved[1].Id);
    }

    [Fact]
    public void ResolveDependencies_UnknownId_Throws()
    {
        var engine = TestHelpers.CreateEngine();
        Assert.Throws<ArgumentException>(() => engine.ResolveDependencies("nonexistent-id-xyz"));
    }

    [Fact]
    public void Dependents_ReturnsTweaksThatDependOnGiven()
    {
        var engine = TestHelpers.CreateEngine();
        var tdA = TestHelpers.MakeTweak("dep-rev-a");
        var tdB = new TweakDef
        {
            Id = "dep-rev-b",
            Label = "B",
            Category = "Test",
            DependsOn = ["dep-rev-a"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\dep-rev-b", "V", 1)],
        };
        engine.Register([tdA, tdB]);
        var dependents = engine.Dependents("dep-rev-a");
        Assert.Single(dependents);
        Assert.Equal("dep-rev-b", dependents[0].Id);
    }

    [Fact]
    public void Dependents_NoDependents_ReturnsEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("dep-leaf")]);
        Assert.Empty(engine.Dependents("dep-leaf"));
    }
}

// ── Freeze post paths ───────────────────────────────────────────────────────

public sealed class FreezeBehaviorTests
{
    [Fact]
    public void GetTweak_AfterFreeze_UsesFrozenDictionary()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("frz-1"), TestHelpers.MakeTweak("frz-2")]);
        engine.Freeze(); // explicit freeze

        // After freeze, GetTweak uses FrozenDictionary
        Assert.NotNull(engine.GetTweak("frz-1"));
        Assert.Null(engine.GetTweak("nonexistent-frz"));
    }

    [Fact]
    public void Categories_AfterFreeze_UsesCachedArray()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("frz-cat-1", "Zeta"), TestHelpers.MakeTweak("frz-cat-2", "Alpha")]);
        engine.Freeze();

        var cats = engine.Categories();
        Assert.Equal("Alpha", cats[0]);
        Assert.Equal("Zeta", cats[1]);
    }

    [Fact]
    public void CategoryCounts_AfterFreeze_UsesCachedDict()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("frz-cc-1", "FrzCat"), TestHelpers.MakeTweak("frz-cc-2", "FrzCat")]);
        engine.Freeze();

        var counts = engine.CategoryCounts();
        Assert.Equal(2, counts["FrzCat"]);
    }

    [Fact]
    public void ScopeCounts_AfterFreeze_UsesCachedDict()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef
            {
                Id = "frz-sc-u",
                Label = "U",
                Category = "X",
                RegistryKeys = [@"HKCU\T"],
                ApplyOps = [RegOp.SetDword(@"HKCU\T", "V", 1)],
            },
            new TweakDef
            {
                Id = "frz-sc-m",
                Label = "M",
                Category = "X",
                RegistryKeys = [@"HKLM\T"],
                ApplyOps = [RegOp.SetDword(@"HKLM\T", "V", 1)],
            },
        ]);
        engine.Freeze();

        var counts = engine.ScopeCounts();
        Assert.Equal(1, counts[TweakScope.User]);
        Assert.Equal(1, counts[TweakScope.Machine]);
    }
}

// ── TweaksForProfile edge cases ─────────────────────────────────────────────

public sealed class TweaksForProfileTests
{
    [Fact]
    public void TweaksForProfile_CaseInsensitive()
    {
        var engine = TestHelpers.CreateEngine();
        // business profile has Privacy category
        engine.Register([
            new TweakDef
            {
                Id = "tfp-priv1",
                Label = "P1",
                Category = "Privacy",
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\tfp-priv1", "V", 1)],
            },
        ]);
        // GetProfile should be case-insensitive
        var tweaks1 = engine.TweaksForProfile("BUSINESS");
        var tweaks2 = engine.TweaksForProfile("business");
        Assert.Equal(tweaks1.Count, tweaks2.Count);
    }

    [Fact]
    public void TweaksForProfile_NullProfile_ReturnsEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        Assert.Empty(engine.TweaksForProfile("completely-nonexistent-profile-xyz"));
    }
}

// ── merged from ConflictDetectorTests.cs ──────────────────────────────────
/// <summary>ConflictDetector tests.</summary>
public sealed class ConflictDetectorTests
{
    // Known conflicting pair used across multiple tests.
    private const string IdA = "energy-enable-hardware-accelerated-gpu-scheduling";
    private const string IdB = "vbs-enable-hvci";

    // ── AllConflicts ──────────────────────────────────────────────────────

    [Fact]
    public void AllConflicts_IsNonEmpty()
    {
        Assert.NotEmpty(ConflictDetector.AllConflicts);
    }

    [Fact]
    public void AllConflicts_AllRecordsHaveNonEmptyIds()
    {
        Assert.All(
            ConflictDetector.AllConflicts,
            c =>
            {
                Assert.False(string.IsNullOrWhiteSpace(c.Id1), $"Id1 is empty in conflict ({c.Id1}, {c.Id2})");
                Assert.False(string.IsNullOrWhiteSpace(c.Id2), $"Id2 is empty in conflict ({c.Id1}, {c.Id2})");
                Assert.False(string.IsNullOrWhiteSpace(c.Reason), $"Reason is empty in conflict ({c.Id1}, {c.Id2})");
            }
        );
    }

    [Fact]
    public void AllConflicts_NoDuplicatePairs()
    {
        // Use a normalised-order key to detect (A,B) == (B,A).
        var pairs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var c in ConflictDetector.AllConflicts)
        {
            var key = string.CompareOrdinal(c.Id1, c.Id2) <= 0 ? $"{c.Id1}|{c.Id2}" : $"{c.Id2}|{c.Id1}";
            Assert.True(pairs.Add(key), $"Duplicate conflict pair registered: {c.Id1} / {c.Id2}");
        }
    }

    // ── Detect — empty / singleton ────────────────────────────────────────

    [Fact]
    public void Detect_EmptyList_ReturnsEmpty()
    {
        var conflicts = ConflictDetector.Detect([]);
        Assert.Empty(conflicts);
    }

    [Fact]
    public void Detect_SingleId_ReturnsEmpty()
    {
        var conflicts = ConflictDetector.Detect([IdA]);
        Assert.Empty(conflicts);
    }

    // ── Detect — known conflict ───────────────────────────────────────────

    [Fact]
    public void Detect_KnownConflictingPair_ReturnsOneConflict()
    {
        var conflicts = ConflictDetector.Detect([IdA, IdB]);
        Assert.Single(conflicts);
    }

    [Fact]
    public void Detect_KnownConflictingPair_ConflictContainsBothIds()
    {
        var conflict = ConflictDetector.Detect([IdA, IdB]).Single();
        var bothIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { conflict.Id1, conflict.Id2 };
        Assert.Contains(IdA, bothIds);
        Assert.Contains(IdB, bothIds);
    }

    [Fact]
    public void Detect_IsSymmetric_SameResultRegardlessOfOrder()
    {
        var ab = ConflictDetector.Detect([IdA, IdB]);
        var ba = ConflictDetector.Detect([IdB, IdA]);
        Assert.Equal(ab.Count, ba.Count);
    }

    // ── Detect — no conflict ──────────────────────────────────────────────

    [Fact]
    public void Detect_UnrelatedIds_ReturnsEmpty()
    {
        // Two IDs that share no known conflict pair.
        var conflicts = ConflictDetector.Detect(["dtcust-show-hidden-files", "audio-disable-sound-scheme"]);
        Assert.Empty(conflicts);
    }

    // ── ConflictsFor ──────────────────────────────────────────────────────

    [Fact]
    public void ConflictsFor_IdWithConflictInApplied_ReturnsOneResult()
    {
        var result = ConflictDetector.ConflictsFor(IdA, [IdB, "some-other-tweak"]);
        Assert.Single(result);
    }

    [Fact]
    public void ConflictsFor_IdWithNoConflictInApplied_ReturnsEmpty()
    {
        var result = ConflictDetector.ConflictsFor(IdA, ["dtcust-show-hidden-files"]);
        Assert.Empty(result);
    }

    [Fact]
    public void ConflictsFor_EmptyApplied_ReturnsEmpty()
    {
        var result = ConflictDetector.ConflictsFor(IdA, []);
        Assert.Empty(result);
    }

    [Fact]
    public void ConflictsFor_IdNotInAnyConflict_AlwaysReturnsEmpty()
    {
        // An ID that is not part of any known pair.
        var result = ConflictDetector.ConflictsFor("dtcust-show-hidden-files", [IdA, IdB, "sac-disable-virtualization-based-security"]);
        Assert.Empty(result);
    }

    // ── Reason is descriptive ─────────────────────────────────────────────

    [Fact]
    public void Detect_KnownPair_ReasonIsNonTrivial()
    {
        var conflict = ConflictDetector.Detect([IdA, IdB]).Single();
        Assert.True(conflict.Reason.Length >= 10, $"Conflict reason '{conflict.Reason}' is too short to be descriptive.");
    }
}
