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
}
