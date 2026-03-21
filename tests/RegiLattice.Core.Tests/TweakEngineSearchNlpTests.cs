// tests/RegiLattice.Core.Tests/TweakEngineSearchNlpTests.cs
// Sprint 58 — NLP synonym search expansion tests.

using RegiLattice.Core;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for Sprint 58: NLP synonym search in TweakEngine.Search().</summary>
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
