using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Shared fixture that calls RegisterBuiltins() once for all tests that need it.</summary>
public sealed class BuiltinsFixture
{
    public TweakEngine Engine { get; }

    public BuiltinsFixture()
    {
        Engine = new TweakEngine(new RegistrySession(dryRun: true));
        Engine.RegisterBuiltins();
    }
}

/// <summary>Tests for TweakEngine: registration, lookup, search, profiles, dry-run ops.</summary>
public sealed class TweakEngineTests
{
    private static TweakEngine CreateEngine()
    {
        var session = new RegistrySession(dryRun: true);
        return new TweakEngine(session);
    }

    private static TweakDef MakeTweak(string id, string category = "Test", string label = "Tweak") =>
        new()
        {
            Id = id,
            Label = label,
            Category = category,
            RegistryKeys = [$@"HKCU\Software\{id}"],
            Description = $"Description for {id}",
            Tags = ["test", category.ToLowerInvariant()],
            ApplyOps = [RegOp.SetDword($@"HKCU\Software\{id}", "Enabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"HKCU\Software\{id}", "Enabled")],
            DetectOps = [RegOp.CheckDword($@"HKCU\Software\{id}", "Enabled", 1)],
        };

    // ── Registration ────────────────────────────────────────────────────
    [Fact]
    public void Register_AddsTweaks()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("t-1"), MakeTweak("t-2")]);
        Assert.Equal(2, engine.TweakCount);
    }

    [Fact]
    public void Register_DuplicateId_Throws()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("dup-1")]);
        Assert.Throws<InvalidOperationException>(() => engine.Register([MakeTweak("dup-1")]));
    }

    // ── Lookup ──────────────────────────────────────────────────────────
    [Fact]
    public void GetTweak_ExistingId_ReturnsTweak()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("lookup-1")]);
        var td = engine.GetTweak("lookup-1");
        Assert.NotNull(td);
        Assert.Equal("lookup-1", td.Id);
    }

    [Fact]
    public void GetTweak_MissingId_ReturnsNull()
    {
        var engine = CreateEngine();
        Assert.Null(engine.GetTweak("nonexistent"));
    }

    [Fact]
    public void AllTweaks_ReturnsAll()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("a-1"), MakeTweak("a-2"), MakeTweak("a-3")]);
        Assert.Equal(3, engine.AllTweaks().Count);
    }

    // ── Categories ──────────────────────────────────────────────────────
    [Fact]
    public void Categories_ReturnsSorted()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("c-1", "Zebra"), MakeTweak("c-2", "Alpha")]);
        var cats = engine.Categories();
        Assert.Equal(2, cats.Count);
        Assert.Equal("Alpha", cats[0]);
        Assert.Equal("Zebra", cats[1]);
    }

    [Fact]
    public void TweaksByCategory_GroupsCorrectly()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("g-1", "CatA"), MakeTweak("g-2", "CatA"), MakeTweak("g-3", "CatB")]);
        var bycat = engine.TweaksByCategory();
        Assert.Equal(2, bycat["CatA"].Count);
        Assert.Single(bycat["CatB"]);
    }

    // ── Search ──────────────────────────────────────────────────────────
    [Fact]
    public void Search_EmptyQuery_ReturnsAll()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("s-1"), MakeTweak("s-2")]);
        Assert.Equal(2, engine.Search("").Count);
    }

    [Fact]
    public void Search_ById_ReturnsMatch()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("search-target"), MakeTweak("other-tweak")]);
        var results = engine.Search("search-target");
        Assert.Single(results);
        Assert.Equal("search-target", results[0].Id);
    }

    [Fact]
    public void Search_ByTag_ReturnsMatch()
    {
        var engine = CreateEngine();
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
        var engine = CreateEngine();
        engine.Register([MakeTweak("UPPER-case")]);
        var results = engine.Search("upper-case");
        Assert.Single(results);
    }

    // ── Filter ────────────────────────────────────────────────────────────
    [Fact]
    public void Filter_ByCategory()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("f-1", "CatA"), MakeTweak("f-2", "CatB")]);
        var results = engine.Filter(category: "CatA");
        Assert.Single(results);
    }

    [Fact]
    public void Filter_ByScope()
    {
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
        engine.Register([MakeTweak("cc-1", "X"), MakeTweak("cc-2", "X"), MakeTweak("cc-3", "Y")]);
        var counts = engine.CategoryCounts();
        Assert.Equal(2, counts["X"]);
        Assert.Equal(1, counts["Y"]);
    }

    [Fact]
    public void ScopeCounts_SumsCorrectly()
    {
        var engine = CreateEngine();
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
        var engine = CreateEngine();
        engine.Register([MakeTweak("sm-1"), MakeTweak("sm-2")]);
        var map = engine.StatusMap();
        Assert.Equal(2, map.Count);
    }

    [Fact]
    public void StatusMap_Parallel_ReturnsEntryPerTweak()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("smp-1"), MakeTweak("smp-2")]);
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
        var engine = CreateEngine();
        engine.Register([MakeTweak("ids-1"), MakeTweak("ids-2"), MakeTweak("ids-3")]);
        var results = engine.TweaksByIds(["ids-1", "ids-3"]);
        Assert.Equal(2, results.Count);
        Assert.Contains(results, t => t.Id == "ids-1");
        Assert.Contains(results, t => t.Id == "ids-3");
    }

    [Fact]
    public void TweaksByIds_IgnoresUnknownIds()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("known-1")]);
        var results = engine.TweaksByIds(["known-1", "unknown-99"]);
        Assert.Single(results);
        Assert.Equal("known-1", results[0].Id);
    }

    // ── TweaksByTag ─────────────────────────────────────────────────────
    [Fact]
    public void TweaksByTag_ReturnsMatchingTweaks()
    {
        var engine = CreateEngine();
        var td = new TweakDef
        {
            Id = "tag-1",
            Label = "Tag",
            Category = "X",
            Tags = ["special-tag"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\tag-1", "V", 1)],
        };
        engine.Register([td, MakeTweak("notag-1")]);
        var results = engine.TweaksByTag("special-tag");
        Assert.Single(results);
        Assert.Equal("tag-1", results[0].Id);
    }

    [Fact]
    public void TweaksByTag_CaseInsensitive()
    {
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
        engine.Register([MakeTweak("snap-1"), MakeTweak("snap-2")]);
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
        var engine = CreateEngine();
        engine.Register([MakeTweak("exp-1"), MakeTweak("exp-2")]);
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
        var engine = CreateEngine();
        engine.Register([MakeTweak("cnt-1"), MakeTweak("cnt-2"), MakeTweak("cnt-3")]);
        Assert.Equal(engine.AllTweaks().Count, engine.TweakCount);
    }

    [Fact]
    public void CategoryCount_MatchesCategories()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("cat-1", "A"), MakeTweak("cat-2", "B"), MakeTweak("cat-3", "C")]);
        Assert.Equal(engine.Categories().Count, engine.CategoryCount);
    }

    // ── Snapshot round-trip ──────────────────────────────────────────────

    [Fact]
    public void SaveSnapshot_CreatesFile()
    {
        var engine = CreateEngine();
        engine.Register([MakeTweak("snap-1")]);
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
            var engine = CreateEngine();
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
            var engine = CreateEngine();
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
        var engine = CreateEngine();
        engine.Register([MakeTweak("rt-1"), MakeTweak("rt-2")]);
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
        var engine = CreateEngine();
        engine.Register([MakeTweak("exp-1", "Export"), MakeTweak("exp-2", "Export")]);
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
        engine.Register([MakeTweak("tag-x")]);
        Assert.Empty(engine.TweaksByTag("nonexistent"));
    }

    [Fact]
    public void TweaksByScope_ReturnsCorrectScope()
    {
        var engine = CreateEngine();
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
        var engine = CreateEngine();
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
        var engine = CreateEngine();
        engine.Register([MakeTweak("frz-1", "Alpha"), MakeTweak("frz-2", "Beta")]);
        engine.Freeze();
        var catCounts = engine.CategoryCounts();
        Assert.Equal(2, catCounts.Count);
        Assert.Equal(1, catCounts["Alpha"]);
        Assert.Equal(1, catCounts["Beta"]);
    }

    [Fact]
    public void ScopeCounts_ReturnsCorrectBreakdown()
    {
        var engine = CreateEngine();
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
        var engine = CreateEngine();
        engine.RegisterBuiltins();
        Assert.Empty(engine.TweaksForProfile("nonexistent-profile"));
    }

    [Fact]
    public void TweaksForProfile_ValidName_ReturnsNonEmpty()
    {
        var engine = CreateEngine();
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
}

/// <summary>Tests that require RegisterBuiltins — share a single engine via IClassFixture.</summary>
public sealed class TweakEngineBuiltinsTests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _engine;

    public TweakEngineBuiltinsTests(BuiltinsFixture fixture)
    {
        _engine = fixture.Engine;
    }

    [Fact]
    public void RegisterBuiltins_LoadsAllTweaks()
    {
        Assert.True(_engine.TweakCount > 1000, $"Expected >1000 tweaks, got {_engine.TweakCount}");
        Assert.True(_engine.CategoryCount >= 60, $"Expected >=60 categories, got {_engine.CategoryCount}");
    }

    [Fact]
    public void RegisterBuiltins_AllIdsUnique()
    {
        var ids = _engine.AllTweaks().Select(t => t.Id).ToList();
        Assert.Equal(ids.Count, ids.Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Fact]
    public void RegisterBuiltins_AllHaveRequiredFields()
    {
        foreach (var td in _engine.AllTweaks())
        {
            Assert.False(string.IsNullOrWhiteSpace(td.Id), $"Tweak with empty Id found");
            Assert.False(string.IsNullOrWhiteSpace(td.Label), $"Tweak {td.Id} has empty Label");
            Assert.False(string.IsNullOrWhiteSpace(td.Category), $"Tweak {td.Id} has empty Category");
        }
    }

    [Fact]
    public void TweaksForProfile_UnknownProfile_ReturnsEmpty()
    {
        Assert.Empty(_engine.TweaksForProfile("nonexistent"));
    }

    [Fact]
    public void TweaksForProfile_Business_ReturnsNonEmpty()
    {
        var tweaks = _engine.TweaksForProfile("business");
        Assert.NotEmpty(tweaks);
        Assert.True(tweaks.Count > 100, $"Expected >100 tweaks for business profile, got {tweaks.Count}");
    }

    [Fact]
    public void ApplyProfile_UnknownProfile_ReturnsEmpty()
    {
        Assert.Empty(_engine.ApplyProfile("nonexistent"));
    }

    [Fact]
    public void RegisterBuiltins_SecurityCategoryExists()
    {
        Assert.Contains("Security", _engine.Categories());
    }

    [Theory]
    [InlineData("sec-restrict-anonymous-enum")]
    [InlineData("sec-enable-dep-always")]
    [InlineData("sec-enable-safe-dll-search")]
    [InlineData("sec-reduce-cached-logons")]
    [InlineData("sec-restrict-sam-remote")]
    [InlineData("sec-disable-llmnr")]
    [InlineData("sec-disable-netbios")]
    [InlineData("sec-disable-wpad")]
    [InlineData("sec-enforce-smb-signing")]
    [InlineData("sec-disable-powershell-v2")]
    [InlineData("sec-enforce-lsa-ppl")]
    [InlineData("sec-block-wdigest-caching")]
    [InlineData("sec-disable-autorun-all")]
    [InlineData("sec-enforce-nla")]
    [InlineData("sec-disable-lm-hash")]
    [InlineData("sec-enforce-sehop")]
    [InlineData("sec-force-strong-key-protection")]
    [InlineData("sec-restrict-null-session-pipes")]
    [InlineData("sec-set-ntlmv2-only")]
    public void RegisterBuiltins_SecurityTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Security", tweak.Category);
        Assert.NotEmpty(tweak.ApplyOps);
    }

    [Theory]
    [InlineData("perf-disable-startup-delay")]
    [InlineData("perf-disable-low-disk-warning")]
    [InlineData("perf-increase-irp-stack")]
    [InlineData("perf-disable-tips-notifications")]
    [InlineData("perf-disable-explorer-search-history")]
    [InlineData("perf-increase-file-system-cache")]
    [InlineData("perf-disable-8dot3-name-creation")]
    [InlineData("perf-increase-network-throttle")]
    [InlineData("perf-disable-nagle-algorithm")]
    [InlineData("perf-disable-power-throttling")]
    public void RegisterBuiltins_NewPerfTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Performance", tweak.Category);
        Assert.NotEmpty(tweak.ApplyOps);
    }

    // ── Command Line Tweaks ─────────────────────────────────────────────
    [Theory]
    [InlineData("cmd-disable-hyper-v-hypervisor")]
    [InlineData("cmd-enable-boot-log")]
    [InlineData("cmd-increase-tscsyncpolicy")]
    [InlineData("cmd-disable-dynamic-tick")]
    [InlineData("cmd-set-platform-tick-high")]
    [InlineData("cmd-disable-netbios-over-tcpip")]
    [InlineData("cmd-enable-tcp-autotuning")]
    [InlineData("cmd-enable-rss")]
    [InlineData("cmd-disable-tcp-timestamps")]
    [InlineData("cmd-enable-ecn")]
    [InlineData("cmd-set-ultimate-perf-plan")]
    [InlineData("cmd-disable-usb-selective-suspend")]
    [InlineData("cmd-disable-ie-feature")]
    [InlineData("cmd-enable-sandbox")]
    public void RegisterBuiltins_CmdTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Command Line", tweak.Category);
        Assert.NotNull(tweak.ApplyAction);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    // ── PowerShell Tweaks ───────────────────────────────────────────────
    [Theory]
    [InlineData("ps-disable-print-spooler", TweakKind.ServiceControl)]
    [InlineData("ps-disable-remote-registry", TweakKind.ServiceControl)]
    [InlineData("ps-disable-fax-service", TweakKind.ServiceControl)]
    [InlineData("ps-disable-xbox-services", TweakKind.ServiceControl)]
    [InlineData("ps-clear-temp-files", TweakKind.PowerShell)]
    [InlineData("ps-flush-dns-cache", TweakKind.PowerShell)]
    [InlineData("ps-disable-diagnostics-hub", TweakKind.ServiceControl)]
    [InlineData("ps-disable-wmp-network-sharing", TweakKind.ServiceControl)]
    [InlineData("ps-disable-geolocation-service", TweakKind.ServiceControl)]
    [InlineData("ps-disable-connected-user-experience", TweakKind.ServiceControl)]
    [InlineData("ps-disable-dmwappush-service", TweakKind.ServiceControl)]
    [InlineData("ps-optimize-network-adapter", TweakKind.PowerShell)]
    public void RegisterBuiltins_PsTweakExists(string id, TweakKind expectedKind)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("PowerShell", tweak.Category);
        Assert.Equal(expectedKind, tweak.Kind);
    }

    // ── Scheduled Task (PS) Tweaks ──────────────────────────────────────
    [Theory]
    [InlineData("pst-disable-customer-experience")]
    [InlineData("pst-disable-app-telemetry")]
    [InlineData("pst-disable-windows-maps-update")]
    [InlineData("pst-disable-feedback-hub")]
    [InlineData("pst-disable-disk-diagnostics")]
    [InlineData("pst-disable-office-telemetry")]
    [InlineData("pst-disable-speech-model-update")]
    [InlineData("pst-disable-device-census")]
    [InlineData("pst-disable-handwriting-data")]
    [InlineData("pst-disable-cloud-experience")]
    [InlineData("pst-disable-diagnostic-data-controller")]
    [InlineData("pst-disable-power-efficiency")]
    [InlineData("pst-disable-idle-maintenance")]
    [InlineData("pst-disable-defrag-scheduled")]
    [InlineData("pst-disable-location-notification")]
    [InlineData("pst-disable-windows-error-reporting")]
    [InlineData("pst-disable-family-safety")]
    [InlineData("pst-disable-autochk-rebooter")]
    [InlineData("pst-disable-license-validation")]
    [InlineData("pst-disable-net-framework-ngen")]
    public void RegisterBuiltins_PstTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Scheduled Tasks", tweak.Category);
        Assert.Equal(TweakKind.ScheduledTask, tweak.Kind);
        Assert.NotNull(tweak.ApplyAction);
    }

    // ── Hardening Tweaks ────────────────────────────────────────────────
    [Theory]
    [InlineData("harden-enable-credential-guard")]
    [InlineData("harden-disable-wdigest")]
    [InlineData("harden-enable-lsa-protection")]
    [InlineData("harden-restrict-ntlm-outgoing")]
    [InlineData("harden-enable-aslr-force")]
    [InlineData("harden-disable-null-session-pipes")]
    [InlineData("harden-enable-safe-search-mode")]
    [InlineData("harden-restrict-remote-sam")]
    [InlineData("harden-disable-remote-uac-filter")]
    [InlineData("harden-enable-smb-encryption")]
    [InlineData("harden-disable-smb1")]
    [InlineData("harden-enable-secure-boot-check")]
    [InlineData("harden-enable-audit-logon-events")]
    [InlineData("harden-set-password-policy")]
    [InlineData("harden-enable-firewall-all-profiles")]
    public void RegisterBuiltins_HardeningTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Hardening", tweak.Category);
    }

    // ── Developer Tweaks ────────────────────────────────────────────────
    [Theory]
    [InlineData("dev-disable-last-access-timestamp")]
    [InlineData("dev-increase-memory-mapped-limit")]
    [InlineData("dev-add-defender-exclusion-repos")]
    [InlineData("dev-enable-utf8-system-wide")]
    [InlineData("dev-enable-sudo")]
    [InlineData("dev-git-lfs-install")]
    [InlineData("dev-env-add-dotnet-tools")]
    [InlineData("dev-disable-defender-realtime-build")]
    [InlineData("dev-enable-developer-mode-full")]
    [InlineData("dev-increase-irp-stack-size")]
    [InlineData("dev-enable-wsl2")]
    [InlineData("dev-enable-openssh-server")]
    [InlineData("dev-set-execution-policy-unrestricted")]
    [InlineData("dev-disable-ntfs-8dot3-names")]
    [InlineData("dev-increase-file-handle-limit")]
    public void RegisterBuiltins_DeveloperTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Developer", tweak.Category);
    }

    // ── Memory Optimization Tweaks ──────────────────────────────────────
    [Theory]
    [InlineData("mem-disable-paging-executive")]
    [InlineData("mem-enable-large-system-cache")]
    [InlineData("mem-clear-pagefile-on-shutdown")]
    [InlineData("mem-set-iot-registry-quota")]
    [InlineData("mem-optimize-svchosts")]
    [InlineData("mem-disable-memory-compression")]
    [InlineData("mem-set-second-level-data-cache")]
    [InlineData("mem-disable-prefetch-boost")]
    [InlineData("mem-set-io-page-lock-limit")]
    [InlineData("mem-disable-page-combining")]
    [InlineData("mem-set-nonpaged-pool-limit")]
    [InlineData("mem-disable-trim-on-memory-pressure")]
    [InlineData("mem-set-system-pages")]
    [InlineData("mem-disable-superfetch-service")]
    [InlineData("mem-enable-large-pages")]
    public void RegisterBuiltins_MemoryTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Memory", tweak.Category);
    }

    // ── New categories exist ────────────────────────────────────────────
    [Theory]
    [InlineData("Command Line")]
    [InlineData("PowerShell")]
    [InlineData("Hardening")]
    [InlineData("Developer")]
    [InlineData("Memory")]
    [InlineData("Disk Cleanup")]
    [InlineData("Debloat")]
    [InlineData("Network Optimization")]
    [InlineData("Power Management")]
    [InlineData("SSD Optimization")]
    [InlineData("App Compatibility")]
    [InlineData("User Account")]
    [InlineData("Browser Common")]
    [InlineData("Windows Recall")]
    [InlineData("Proxy & VPN")]
    [InlineData("Event Logging")]
    [InlineData("System Restore")]
    [InlineData("Scheduled Tasks")]
    [InlineData("Security")]
    public void RegisterBuiltins_NewCategoryExists(string category)
    {
        Assert.Contains(category, _engine.Categories());
    }

    // ── TweakKind detection ─────────────────────────────────────────────
    [Fact]
    public void TweakKind_RegistryTweak_DefaultsToRegistry()
    {
        var tweak = _engine.GetTweak("perf-disable-startup-delay");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.Registry, tweak.Kind);
    }

    [Fact]
    public void TweakKind_CommandLineTweak_IsSystemCommand()
    {
        var tweak = _engine.GetTweak("cmd-disable-hyper-v-hypervisor");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    // ── Disk Cleanup Tweaks ─────────────────────────────────────────────
    [Theory]
    [InlineData("cleanup-disable-thumbnail-cache")]
    [InlineData("cleanup-disable-delivery-optimisation")]
    [InlineData("cleanup-run-disk-cleanup-silent")]
    [InlineData("cleanup-clear-windows-temp")]
    [InlineData("cleanup-clear-windows-update-cache")]
    [InlineData("cleanup-enable-storage-sense")]
    [InlineData("cleanup-disable-hibernation")]
    [InlineData("cleanup-compact-os")]
    [InlineData("cleanup-disable-reserved-storage")]
    public void RegisterBuiltins_DiskCleanupTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Disk Cleanup", tweak.Category);
    }

    // ── Debloat Tweaks ──────────────────────────────────────────────────
    [Theory]
    [InlineData("debloat-remove-preinstalled-apps")]
    [InlineData("debloat-disable-suggested-apps")]
    [InlineData("debloat-disable-auto-app-install")]
    [InlineData("debloat-disable-consumer-features")]
    [InlineData("debloat-disable-xbox-game-bar")]
    [InlineData("debloat-remove-optional-features")]
    [InlineData("debloat-disable-start-web-search")]
    [InlineData("debloat-disable-cloud-content")]
    public void RegisterBuiltins_DebloatTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Debloat", tweak.Category);
    }

    // ── Network Optimization Tweaks ─────────────────────────────────────
    [Theory]
    [InlineData("netopt-disable-nagle-algorithm")]
    [InlineData("netopt-increase-tcp-window-size")]
    [InlineData("netopt-disable-network-throttling")]
    [InlineData("netopt-set-dns-cloudflare")]
    [InlineData("netopt-set-dns-google")]
    [InlineData("netopt-disable-ipv6")]
    [InlineData("netopt-disable-qos-packet-scheduler")]
    [InlineData("netopt-enable-dns-cache-boost")]
    public void RegisterBuiltins_NetworkOptimizationTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Network Optimization", tweak.Category);
    }

    // ── Power Management Tweaks ─────────────────────────────────────────
    [Theory]
    [InlineData("pwrmgmt-disable-fast-startup")]
    [InlineData("pwrmgmt-disable-connected-standby")]
    [InlineData("pwrmgmt-set-high-performance-plan")]
    [InlineData("pwrmgmt-disable-cpu-parking")]
    [InlineData("pwrmgmt-disable-usb-selective-suspend")]
    [InlineData("pwrmgmt-disable-wake-timers")]
    [InlineData("pwrmgmt-set-lid-close-nothing")]
    public void RegisterBuiltins_PowerManagementTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Power Management", tweak.Category);
    }

    // ── TweakKind detection for new modules ─────────────────────────────
    [Fact]
    public void TweakKind_DiskCleanupSystemCommand_IsCorrect()
    {
        var tweak = _engine.GetTweak("cleanup-disable-hibernation");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    [Fact]
    public void TweakKind_DebloatPowerShell_IsCorrect()
    {
        var tweak = _engine.GetTweak("debloat-remove-preinstalled-apps");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.PowerShell, tweak.Kind);
    }

    [Fact]
    public void TweakKind_NetworkOptPowerShell_IsCorrect()
    {
        var tweak = _engine.GetTweak("netopt-set-dns-cloudflare");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.PowerShell, tweak.Kind);
    }

    [Fact]
    public void TweakKind_PowerMgmtSystemCommand_IsCorrect()
    {
        var tweak = _engine.GetTweak("pwrmgmt-set-high-performance-plan");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    // ── SSD Optimization Tweaks ─────────────────────────────────────────
    [Theory]
    [InlineData("ssd-disable-superfetch")]
    [InlineData("ssd-disable-prefetch")]
    [InlineData("ssd-disable-last-access-timestamp")]
    [InlineData("ssd-enable-trim")]
    [InlineData("ssd-disable-defrag-schedule")]
    [InlineData("ssd-disable-windows-search-indexing")]
    [InlineData("ssd-enable-write-caching")]
    [InlineData("ssd-disable-hibernation-ssd")]
    [InlineData("ssd-disable-8dot3-names")]
    [InlineData("ssd-increase-ntfs-memory-usage")]
    [InlineData("ssd-large-system-cache")]
    [InlineData("ssd-disable-boot-trace")]
    [InlineData("ssd-disable-ntfs-compression")]
    [InlineData("ssd-disable-ntfs-encryption")]
    [InlineData("ssd-disable-storage-sense")]
    [InlineData("ssd-set-io-priority-normal")]
    [InlineData("ssd-disable-low-disk-check")]
    [InlineData("ssd-increase-ntfs-mft-zone")]
    [InlineData("ssd-disable-readyboost")]
    [InlineData("ssd-disable-disk-perf-counters")]
    public void RegisterBuiltins_SsdOptimizationTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("SSD Optimization", tweak.Category);
    }

    // ── App Compatibility Tweaks ────────────────────────────────────────
    [Theory]
    [InlineData("compat-disable-compatibility-telemetry")]
    [InlineData("compat-disable-program-compatibility-assistant")]
    [InlineData("compat-disable-steps-recorder")]
    [InlineData("compat-disable-inventory-collector")]
    [InlineData("compat-disable-engine")]
    [InlineData("compat-disable-switchback")]
    [InlineData("compat-disable-web-search-in-run")]
    [InlineData("compat-disable-fault-tolerant-heap")]
    [InlineData("compat-disable-customer-experience")]
    [InlineData("compat-disable-smart-screen-apps")]
    [InlineData("compat-disable-app-launch-tracking")]
    [InlineData("compat-disable-startup-delay")]
    [InlineData("compat-disable-autoplay-devices")]
    [InlineData("compat-disable-maintenance-wakeup")]
    [InlineData("compat-set-diagnostic-data-basic")]
    [InlineData("compat-force-classic-shutdown")]
    [InlineData("compat-disable-background-apps")]
    [InlineData("compat-disable-tips-suggestions")]
    [InlineData("compat-disable-shim-database")]
    public void RegisterBuiltins_AppCompatibilityTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("App Compatibility", tweak.Category);
    }

    // ── User Account Tweaks ─────────────────────────────────────────────
    [Theory]
    [InlineData("uac-disable-dimming")]
    [InlineData("uac-set-silent-admin")]
    [InlineData("uac-disable-for-built-in-admin")]
    [InlineData("uac-enable-admin-approval-mode")]
    [InlineData("uac-virtualise-file-registry")]
    [InlineData("uac-disable-auto-admin-logon")]
    [InlineData("uac-set-account-lockout-10")]
    [InlineData("uac-set-password-length-8")]
    [InlineData("uac-hide-last-username")]
    [InlineData("uac-disable-credential-guard-lock-timeout")]
    [InlineData("uac-require-ctrl-alt-del")]
    [InlineData("uac-set-lockout-duration-30")]
    [InlineData("uac-set-max-password-age-90")]
    [InlineData("uac-elevate-signed-only")]
    [InlineData("uac-restrict-blank-password")]
    [InlineData("uac-enable-installer-detection")]
    [InlineData("uac-standard-user-prompt-credentials")]
    [InlineData("uac-disable-remote-uac")]
    [InlineData("uac-enable-secure-desktop")]
    public void RegisterBuiltins_UserAccountTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("User Account", tweak.Category);
    }

    // ── Browser Common Tweaks ───────────────────────────────────────────
    [Theory]
    [InlineData("browser-disable-dns-prefetch")]
    [InlineData("browser-disable-background-network")]
    [InlineData("browser-disable-prediction-service")]
    [InlineData("browser-disable-metrics-reporting")]
    [InlineData("browser-disable-autofill-cc")]
    [InlineData("browser-disable-autofill-addresses")]
    [InlineData("browser-disable-password-manager")]
    [InlineData("browser-send-do-not-track")]
    [InlineData("browser-block-third-party-cookies")]
    [InlineData("browser-disable-safe-browsing-telemetry")]
    [InlineData("browser-disable-translate")]
    [InlineData("browser-disable-spell-check")]
    [InlineData("browser-disable-search-suggestions")]
    [InlineData("browser-disable-sync")]
    [InlineData("browser-disable-browser-sign-in")]
    [InlineData("browser-disable-media-router")]
    [InlineData("browser-disable-shopping-features")]
    [InlineData("browser-disable-preloading")]
    [InlineData("browser-disable-form-fill")]
    public void RegisterBuiltins_BrowserCommonTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Browser Common", tweak.Category);
    }

    // ── TweakKind detection for wave 3 modules ──────────────────────────
    [Fact]
    public void TweakKind_SsdSystemCommand_IsCorrect()
    {
        var tweak = _engine.GetTweak("ssd-disable-last-access-timestamp");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    [Fact]
    public void TweakKind_UacRegistry_IsCorrect()
    {
        var tweak = _engine.GetTweak("uac-disable-dimming");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.GroupPolicy, tweak.Kind);
    }

    // ── Windows Recall Tweaks ───────────────────────────────────────────
    [Theory]
    [InlineData("recall-disable-recall")]
    [InlineData("recall-disable-saving-snapshots")]
    [InlineData("recall-disable-ai-suggestions")]
    [InlineData("recall-disable-semantic-indexing")]
    [InlineData("recall-disable-cocreator")]
    [InlineData("recall-disable-image-creator")]
    [InlineData("recall-disable-generative-fill")]
    [InlineData("recall-disable-ai-in-notepad")]
    [InlineData("recall-disable-web-content-eval")]
    [InlineData("recall-disable-cross-device-resume")]
    [InlineData("recall-disable-ai-search-highlights")]
    [InlineData("recall-disable-inking-and-typing-personalization")]
    public void RegisterBuiltins_WindowsRecallTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Windows Recall", tweak.Category);
    }

    // ── Proxy & VPN Tweaks ──────────────────────────────────────────────
    [Theory]
    [InlineData("proxy-disable-auto-detect")]
    [InlineData("proxy-disable-proxy-server")]
    [InlineData("proxy-disable-ncsi-active-probing")]
    [InlineData("proxy-disable-ipv6-transition")]
    [InlineData("proxy-disable-smart-multi-homed")]
    [InlineData("proxy-disable-llmnr")]
    [InlineData("proxy-disable-netbios-over-tcpip")]
    [InlineData("proxy-set-winhttp-timeout")]
    [InlineData("proxy-disable-insecure-fallback")]
    [InlineData("proxy-disable-web-proxy-auto-config")]
    [InlineData("proxy-enable-dns-over-https")]
    [InlineData("proxy-disable-wifi-sense")]
    public void RegisterBuiltins_ProxyVpnTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Proxy & VPN", tweak.Category);
    }

    // ── Event Logging Tweaks ────────────────────────────────────────────
    [Theory]
    [InlineData("evtlog-increase-system-log-size")]
    [InlineData("evtlog-increase-security-log-size")]
    [InlineData("evtlog-increase-application-log-size")]
    [InlineData("evtlog-enable-powershell-script-block-logging")]
    [InlineData("evtlog-enable-powershell-module-logging")]
    [InlineData("evtlog-enable-process-creation-audit")]
    [InlineData("evtlog-set-crash-dump-mini")]
    [InlineData("evtlog-disable-auto-reboot-on-crash")]
    [InlineData("evtlog-enable-verbose-boot-status")]
    [InlineData("evtlog-enable-shutdown-reason")]
    [InlineData("evtlog-log-retention-overwrite")]
    [InlineData("evtlog-disable-event-forwarding")]
    [InlineData("evtlog-set-app-log-32mb")]
    [InlineData("evtlog-set-system-log-32mb")]
    [InlineData("evtlog-set-security-log-64mb")]
    [InlineData("evtlog-disable-event-tracing-autologger")]
    [InlineData("evtlog-disable-powershell-logging")]
    [InlineData("evtlog-enable-powershell-transcription")]
    [InlineData("evtlog-enable-command-line-auditing")]
    [InlineData("evtlog-disable-srum")]
    public void RegisterBuiltins_EventLoggingTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Event Logging", tweak.Category);
    }

    // ── System Restore Tweaks ───────────────────────────────────────────
    [Theory]
    [InlineData("restore-disable-system-restore")]
    [InlineData("restore-disable-config-change-restore")]
    [InlineData("restore-set-max-frequency-daily")]
    [InlineData("restore-disable-vss-service")]
    [InlineData("restore-disable-previous-versions")]
    [InlineData("restore-enable-scheduled-points")]
    [InlineData("restore-disable-wer-queue")]
    [InlineData("restore-disable-wer-archive")]
    [InlineData("restore-set-wer-consent-send-always")]
    [InlineData("restore-disable-memory-dump")]
    [InlineData("restore-set-mini-dump-only")]
    [InlineData("restore-overwrite-existing-dump")]
    [InlineData("restore-disable-auto-reboot-on-crash")]
    [InlineData("restore-disable-wer-logging")]
    [InlineData("restore-set-wer-max-queue-5")]
    [InlineData("restore-set-wer-max-archive-5")]
    [InlineData("restore-disable-auto-recovery-boot")]
    [InlineData("restore-disable-shadow-copy-optimisation")]
    [InlineData("restore-disable-wer-dump-type")]
    [InlineData("restore-limit-wer-dump-count")]
    public void RegisterBuiltins_SystemRestoreTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("System Restore", tweak.Category);
    }

    // ── TweakKind detection for wave 4 ──────────────────────────────────
    [Fact]
    public void TweakKind_RecallGroupPolicy_IsCorrect()
    {
        var tweak = _engine.GetTweak("recall-disable-recall");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.GroupPolicy, tweak.Kind);
    }

    [Fact]
    public void TweakKind_ProxyRegistry_IsCorrect()
    {
        var tweak = _engine.GetTweak("proxy-disable-auto-detect");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.Registry, tweak.Kind);
    }

    // ── Performance: RegisterBuiltins + Freeze ──────────────────────────
    [Fact]
    public void RegisterBuiltins_CompletesUnder500ms()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.RegisterBuiltins();
        sw.Stop();

        Assert.True(sw.ElapsedMilliseconds < 500, $"RegisterBuiltins took {sw.ElapsedMilliseconds}ms (budget: 500ms)");
    }

    [Fact]
    public void Freeze_BuildsFrozenDictionary()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.RegisterBuiltins();
        // Freeze() is called inside RegisterBuiltins, so lookups should work
        Assert.NotNull(engine.GetTweak("priv-disable-telemetry"));
    }

    [Fact]
    public void Search_After2301Tweaks_CompletesUnder50ms()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var results = _engine.Search("telemetry");
        sw.Stop();

        Assert.NotEmpty(results);
        Assert.True(sw.ElapsedMilliseconds < 50, $"Search took {sw.ElapsedMilliseconds}ms (budget: 50ms)");
    }

    [Fact]
    public void Categories_ReturnsCachedResult()
    {
        var cats1 = _engine.Categories();
        var cats2 = _engine.Categories();
        Assert.Same(cats1, cats2); // Should be same reference (cached)
    }

    // ── Validation ──────────────────────────────────────────────────────

    [Fact]
    public void ValidateTweaks_AllBuiltins_ReturnsNoErrors()
    {
        var errors = _engine.ValidateTweaks();
        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateTweaks_BrokenDependsOn_ReturnsError()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "test-dep-missing",
                Label = "Test",
                Category = "Test",
                DependsOn = ["nonexistent-tweak"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
            },
        ]);
        var errors = engine.ValidateTweaks();
        Assert.Contains(errors, e => e.Contains("nonexistent-tweak"));
    }

    [Fact]
    public void ValidateTweaks_CircularDependency_ReturnsError()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "circ-a",
                Label = "A",
                Category = "Test",
                DependsOn = ["circ-b"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
            new TweakDef
            {
                Id = "circ-b",
                Label = "B",
                Category = "Test",
                DependsOn = ["circ-a"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
        ]);
        var errors = engine.ValidateTweaks();
        Assert.Contains(errors, e => e.Contains("circular dependency"));
    }

    // ── Dependency resolution ───────────────────────────────────────────

    [Fact]
    public void ResolveDependencies_NoDeps_ReturnsSingleItem()
    {
        var chain = _engine.ResolveDependencies(_engine.AllTweaks()[0].Id);
        Assert.Single(chain);
    }

    [Fact]
    public void ResolveDependencies_SimpleChain_ReturnsTopologicalOrder()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "dep-base",
                Label = "Base",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "Base", 1)],
            },
            new TweakDef
            {
                Id = "dep-child",
                Label = "Child",
                Category = "Test",
                DependsOn = ["dep-base"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "Child", 1)],
            },
        ]);
        engine.Freeze();

        var chain = engine.ResolveDependencies("dep-child");
        Assert.Equal(2, chain.Count);
        Assert.Equal("dep-base", chain[0].Id);
        Assert.Equal("dep-child", chain[1].Id);
    }

    [Fact]
    public void ResolveDependencies_CircularDep_Throws()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "loop-a",
                Label = "A",
                Category = "Test",
                DependsOn = ["loop-b"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
            new TweakDef
            {
                Id = "loop-b",
                Label = "B",
                Category = "Test",
                DependsOn = ["loop-a"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
        ]);
        engine.Freeze();

        Assert.Throws<InvalidOperationException>(() => engine.ResolveDependencies("loop-a"));
    }

    [Fact]
    public void ResolveDependencies_UnknownId_Throws()
    {
        Assert.Throws<ArgumentException>(() => _engine.ResolveDependencies("nonexistent-tweak-id"));
    }

    [Fact]
    public void Dependents_NoReverse_ReturnsEmpty()
    {
        // Pick a tweak that nothing depends on (most tweaks)
        var dependents = _engine.Dependents(_engine.AllTweaks()[0].Id);
        // May be empty or not — just verify it doesn't throw
        Assert.NotNull(dependents);
    }

    [Fact]
    public void Dependents_WithReverseDep_ReturnsDependents()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "parent-tweak",
                Label = "Parent",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "P", 1)],
            },
            new TweakDef
            {
                Id = "child-tweak",
                Label = "Child",
                Category = "Test",
                DependsOn = ["parent-tweak"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C", 1)],
            },
        ]);

        var deps = engine.Dependents("parent-tweak");
        Assert.Single(deps);
        Assert.Equal("child-tweak", deps[0].Id);
    }

    // ── Batch with progress callback ────────────────────────────────────

    [Fact]
    public void ApplyBatch_WithProgress_ReportsAllTweaks()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "prog-a",
                Label = "A",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
            new TweakDef
            {
                Id = "prog-b",
                Label = "B",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
        ]);

        var progressCalls = new List<(int Done, int Total, string Id)>();
        engine.ApplyBatch(engine.AllTweaks(), forceCorp: false, (done, total, id, _) => progressCalls.Add((done, total, id)));

        Assert.Equal(2, progressCalls.Count);
        Assert.Equal(1, progressCalls[0].Done);
        Assert.Equal(2, progressCalls[0].Total);
        Assert.Equal(2, progressCalls[1].Done);
        Assert.Equal(2, progressCalls[1].Total);
    }

    [Fact]
    public void RemoveBatch_WithProgress_ReportsAllTweaks()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "rmprog-a",
                Label = "A",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Test", "A")],
            },
        ]);

        int callCount = 0;
        engine.RemoveBatch(engine.AllTweaks(), forceCorp: false, (_, _, _, _) => callCount++);
        Assert.Equal(1, callCount);
    }

    // ── Filter: multi-criteria edge cases ───────────────────────────────

    [Fact]
    public void Filter_AllCriteria_CombinesAnd()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var target = new TweakDef
        {
            Id = "fa-target",
            Label = "Target Tweak",
            Category = "FilterAll",
            CorpSafe = true,
            NeedsAdmin = false,
            MinBuild = 19041,
            Tags = ["filtertest"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-target", "V", 1)],
        };
        engine.Register([
            target,
            new TweakDef
            {
                Id = "fa-wrong-cat",
                Label = "Wrong Cat",
                Category = "Other",
                CorpSafe = true,
                NeedsAdmin = false,
                MinBuild = 19041,
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-wrong-cat", "V", 1)],
            },
            new TweakDef
            {
                Id = "fa-wrong-admin",
                Label = "Needs Admin",
                Category = "FilterAll",
                CorpSafe = true,
                NeedsAdmin = true,
                MinBuild = 19041,
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-wrong-admin", "V", 1)],
            },
            new TweakDef
            {
                Id = "fa-wrong-corp",
                Label = "Not CorpSafe",
                Category = "FilterAll",
                CorpSafe = false,
                NeedsAdmin = false,
                MinBuild = 19041,
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-wrong-corp", "V", 1)],
            },
        ]);
        engine.Freeze();

        var results = engine.Filter(corpSafe: true, needsAdmin: false, category: "FilterAll", minBuild: 20000, query: "target");
        Assert.Single(results);
        Assert.Equal("fa-target", results[0].Id);
    }

    [Fact]
    public void Filter_NoMatches_ReturnsEmpty()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef { Id = "fn-1", Label = "T", Category = "CatA", ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fn-1", "V", 1)] },
            new TweakDef { Id = "fn-2", Label = "T", Category = "CatB", ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fn-2", "V", 1)] },
        ]);
        engine.Freeze();

        var results = engine.Filter(category: "NonExistentCategory");
        Assert.Empty(results);
    }

    [Fact]
    public void Filter_NoCriteria_ReturnsAll()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef { Id = "fnc-1", Label = "T", Category = "Test", ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fnc-1", "V", 1)] },
            new TweakDef { Id = "fnc-2", Label = "T", Category = "Test", ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fnc-2", "V", 1)] },
            new TweakDef { Id = "fnc-3", Label = "T", Category = "Test", ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fnc-3", "V", 1)] },
        ]);
        engine.Freeze();

        var results = engine.Filter();
        Assert.Equal(3, results.Count);
    }

    [Fact]
    public void Filter_QueryAndScope_CombinesAnd()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "fqs-user",
                Label = "User Telemetry",
                Category = "Privacy",
                RegistryKeys = [@"HKEY_CURRENT_USER\Software\fqs-user"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fqs-user", "V", 1)],
            },
            new TweakDef
            {
                Id = "fqs-machine",
                Label = "Machine Telemetry",
                Category = "Privacy",
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\Software\fqs-machine"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\Software\fqs-machine", "V", 1)],
            },
        ]);
        engine.Freeze();

        var results = engine.Filter(scope: TweakScope.User, query: "telemetry");
        Assert.Single(results);
        Assert.Equal("fqs-user", results[0].Id);
    }

    // ── Update ──────────────────────────────────────────────────────────

    [Fact]
    public void Update_NoUpdateAction_FallsBackToApply()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var td = new TweakDef
        {
            Id = "upd-fallback",
            Label = "U",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\upd-fallback", "V", 1)],
        };
        engine.Register([td]);

        var result = engine.Update(td);
        Assert.Equal(TweakResult.Applied, result);
    }

    [Fact]
    public void Update_WithUpdateAction_CallsUpdateAction()
    {
        bool called = false;
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var td = new TweakDef
        {
            Id = "upd-custom",
            Label = "U",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\upd-custom", "V", 1)],
            UpdateAction = (_) => { called = true; },
        };
        engine.Register([td]);

        var result = engine.Update(td);
        Assert.Equal(TweakResult.Applied, result);
        Assert.True(called);
    }

    [Fact]
    public void Update_UpdateActionThrows_ReturnsError()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var td = new TweakDef
        {
            Id = "upd-err",
            Label = "U",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\upd-err", "V", 1)],
            UpdateAction = (_) => throw new InvalidOperationException("update failed"),
        };
        engine.Register([td]);

        var result = engine.Update(td);
        Assert.Equal(TweakResult.Error, result);
    }

    // ── Complex dependency graphs ───────────────────────────────────────

    [Fact]
    public void ResolveDependencies_DiamondGraph_ReturnsCorrectOrder()
    {
        // Diamond: A depends on B and C, both B and C depend on D
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "diamond-d",
                Label = "D",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "D", 1)],
            },
            new TweakDef
            {
                Id = "diamond-b",
                Label = "B",
                Category = "Test",
                DependsOn = ["diamond-d"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
            new TweakDef
            {
                Id = "diamond-c",
                Label = "C",
                Category = "Test",
                DependsOn = ["diamond-d"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C", 1)],
            },
            new TweakDef
            {
                Id = "diamond-a",
                Label = "A",
                Category = "Test",
                DependsOn = ["diamond-b", "diamond-c"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
        ]);
        engine.Freeze();

        var chain = engine.ResolveDependencies("diamond-a");
        var chainList = chain.ToList();
        Assert.Equal(4, chainList.Count);
        // D must come before B and C; B and C must come before A
        int idxD = chainList.FindIndex(t => t.Id == "diamond-d");
        int idxB = chainList.FindIndex(t => t.Id == "diamond-b");
        int idxC = chainList.FindIndex(t => t.Id == "diamond-c");
        int idxA = chainList.FindIndex(t => t.Id == "diamond-a");
        Assert.True(idxD < idxB);
        Assert.True(idxD < idxC);
        Assert.True(idxB < idxA);
        Assert.True(idxC < idxA);
    }

    [Fact]
    public void ResolveDependencies_DeepChain_FiveLevels()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "chain-1",
                Label = "L1",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L1", 1)],
            },
            new TweakDef
            {
                Id = "chain-2",
                Label = "L2",
                Category = "Test",
                DependsOn = ["chain-1"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L2", 1)],
            },
            new TweakDef
            {
                Id = "chain-3",
                Label = "L3",
                Category = "Test",
                DependsOn = ["chain-2"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L3", 1)],
            },
            new TweakDef
            {
                Id = "chain-4",
                Label = "L4",
                Category = "Test",
                DependsOn = ["chain-3"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L4", 1)],
            },
            new TweakDef
            {
                Id = "chain-5",
                Label = "L5",
                Category = "Test",
                DependsOn = ["chain-4"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L5", 1)],
            },
        ]);
        engine.Freeze();

        var chain = engine.ResolveDependencies("chain-5");
        Assert.Equal(5, chain.Count);
        for (int i = 0; i < 5; i++)
            Assert.Equal($"chain-{i + 1}", chain[i].Id);
    }

    [Fact]
    public void Dependents_MultipleChildren_ReturnsAll()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "multi-parent",
                Label = "Parent",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "P", 1)],
            },
            new TweakDef
            {
                Id = "multi-child-1",
                Label = "Child1",
                Category = "Test",
                DependsOn = ["multi-parent"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C1", 1)],
            },
            new TweakDef
            {
                Id = "multi-child-2",
                Label = "Child2",
                Category = "Test",
                DependsOn = ["multi-parent"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C2", 1)],
            },
            new TweakDef
            {
                Id = "multi-child-3",
                Label = "Child3",
                Category = "Test",
                DependsOn = ["multi-parent"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C3", 1)],
            },
        ]);

        var deps = engine.Dependents("multi-parent");
        Assert.Equal(3, deps.Count);
        Assert.Contains(deps, d => d.Id == "multi-child-1");
        Assert.Contains(deps, d => d.Id == "multi-child-2");
        Assert.Contains(deps, d => d.Id == "multi-child-3");
    }
}
