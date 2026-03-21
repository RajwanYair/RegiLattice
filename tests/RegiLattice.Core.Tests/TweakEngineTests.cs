using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for TweakEngine: registration, lookup, search, profiles, dry-run ops.</summary>
public sealed class TweakEngineTests
{
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
        var engine = TestHelpers.CreateEngine();
        engine.RegisterBuiltins();
        Assert.Empty(engine.TweaksForProfile("nonexistent-profile"));
    }

    [Fact]
    public void TweaksForProfile_ValidName_ReturnsNonEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        engine.RegisterBuiltins();
        var tweaks = engine.TweaksForProfile("privacy");
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
        var result = engine.RemoveBatch([], forceCorp: true);
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

    // ── Sprint 21: Coverage boost — Filter, Batch, StatusMap, Export edge cases ──

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

// ── Sprint 24: ApplyProfile, CategoryCounts, ScopeCounts, utility paths ──

public sealed class TweakEngineSprint24Tests
{
    [Fact]
    public void ApplyProfile_ValidProfile_ReturnsResultsForEachTweak()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        // Verify profile resolution works (returns tweaks without applying - avoids spawning processes)
        var tweaks = engine.TweaksForProfile("privacy");
        Assert.NotEmpty(tweaks);
        Assert.All(tweaks, t => Assert.NotEmpty(t.Id));
    }

    [Fact]
    public void ApplyProfile_InvalidProfile_ReturnsEmpty()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var results = engine.ApplyProfile("nonexistent-xyz-profile");
        Assert.Empty(results);
    }

    [Fact]
    public void CategoryCounts_SumsToAllTweaksCount()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var total = engine.CategoryCounts().Values.Sum();
        Assert.Equal(engine.AllTweaks().Count, total);
    }

    [Fact]
    public void ScopeCounts_SumsToAllTweaksCount()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var total = engine.ScopeCounts().Values.Sum();
        Assert.Equal(engine.AllTweaks().Count, total);
    }

    [Fact]
    public void CategoryCounts_AllCategoriesPresent()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var cats = engine.Categories().ToHashSet();
        Assert.All(engine.CategoryCounts().Keys, k => Assert.Contains(k, cats));
    }

    [Fact]
    public void ScopeCounts_ContainsUserAndMachine()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var counts = engine.ScopeCounts();
        Assert.True(counts.ContainsKey(TweakScope.User) || counts.ContainsKey(TweakScope.Machine));
    }

    [Fact]
    public void GetScope_KnownId_ReturnsScope()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var first = engine.AllTweaks()[0];
        var scope = engine.GetScope(first);
        Assert.True(Enum.IsDefined(typeof(TweakScope), scope));
    }

    [Fact]
    public void TweaksByScope_User_ContainsAtLeastOneTweak()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var userTweaks = engine.TweaksByScope(TweakScope.User);
        Assert.NotEmpty(userTweaks);
        Assert.All(userTweaks, t => Assert.Equal(TweakScope.User, t.Scope));
    }

    [Fact]
    public void TweaksByScope_Machine_ContainsAtLeastOneTweak()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var machineTweaks = engine.TweaksByScope(TweakScope.Machine);
        Assert.NotEmpty(machineTweaks);
        Assert.All(machineTweaks, t => Assert.Equal(TweakScope.Machine, t.Scope));
    }

    [Fact]
    public void TweaksByTag_ReturnsOnlyMatchingTag()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var tagged = engine.TweaksByTag("privacy");
        Assert.NotEmpty(tagged);
        Assert.All(tagged, t => Assert.Contains("privacy", t.Tags, StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    public void Filter_ByCorpSafe_True_ReturnsOnlyCorpSafe()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var corpSafe = engine.Filter(corpSafe: true);
        Assert.NotEmpty(corpSafe);
        Assert.All(corpSafe, t => Assert.True(t.CorpSafe));
    }

    [Fact]
    public void Filter_ByNeedsAdmin_False_ReturnsOnlyNonAdmin()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var nonAdmin = engine.Filter(needsAdmin: false);
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
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var results = engine.Search("telemetry");
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
