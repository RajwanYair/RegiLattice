// tests/RegiLattice.Core.Tests/HealthScoreServiceTests.cs
// Sprint 62 — HealthScoreService bucket membership and score calculation tests.

using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for Sprint 62: HealthScoreService.</summary>
public sealed class HealthScoreServiceTests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _engine;

    public HealthScoreServiceTests(BuiltinsFixture fixture)
    {
        _engine = fixture.Engine;
    }

    private TweakEngine BuildEngine() => _engine;

    // ── Compute with empty map ────────────────────────────────────────────

    [Fact]
    public void Compute_EmptyStatusMap_ReturnsZeroScores()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);
        var score = svc.Compute(new Dictionary<string, TweakResult>());

        Assert.Equal(0, score.Privacy);
        Assert.Equal(0, score.Performance);
        Assert.Equal(0, score.Security);
        Assert.Equal(0, score.Stability);
    }

    // ── Compute — higher applied count raises score ───────────────────────

    [Fact]
    public void Compute_AllPrivacyApplied_PrivacyScorePositive()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);
        var privacyTweaks = svc.PrivacyTweaks();

        var map = privacyTweaks.ToDictionary(t => t.Id, _ => TweakResult.Applied);
        var score = svc.Compute(map);

        Assert.True(score.Privacy > 0, $"Privacy score should be >0 when all {privacyTweaks.Count} privacy tweaks are applied.");
    }

    [Fact]
    public void Compute_AllPerformanceApplied_PerformanceScorePositive()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);
        var perfTweaks = svc.PerformanceTweaks();

        var map = perfTweaks.ToDictionary(t => t.Id, _ => TweakResult.Applied);
        var score = svc.Compute(map);

        Assert.True(score.Performance > 0, $"Performance score should be >0 when all {perfTweaks.Count} performance tweaks are applied.");
    }

    // ── Score bounds ──────────────────────────────────────────────────────

    [Fact]
    public void Compute_Scores_AreInZeroToHundredRange()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);
        var allIds = engine.AllTweaks().Select(t => t.Id).ToDictionary(id => id, _ => TweakResult.Applied);
        var score = svc.Compute(allIds);

        Assert.InRange(score.Privacy, 0, 100);
        Assert.InRange(score.Performance, 0, 100);
        Assert.InRange(score.Security, 0, 100);
        Assert.InRange(score.Stability, 0, 100);
        Assert.InRange(score.Overall, 0, 100);
    }

    // ── OverallLabel mapping ──────────────────────────────────────────────

    [Theory]
    [InlineData(90, "Excellent")]
    [InlineData(70, "Good")]
    [InlineData(50, "Fair")]
    [InlineData(25, "Needs Work")]
    [InlineData(5, "Poor")]
    public void OverallLabel_MapsCorrectly(int overall, string expectedLabel)
    {
        var score = new HealthScore(overall, overall, overall, overall, overall);
        Assert.Equal(expectedLabel, score.OverallLabel);
    }

    // ── Bucket helpers return non-empty lists ────────────────────────────

    [Fact]
    public void PrivacyTweaks_ReturnsNonEmptyList()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);
        Assert.NotEmpty(svc.PrivacyTweaks());
    }

    [Fact]
    public void PerformanceTweaks_ReturnsNonEmptyList()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);
        Assert.NotEmpty(svc.PerformanceTweaks());
    }

    [Fact]
    public void SecurityTweaks_ReturnsNonEmptyList()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);
        Assert.NotEmpty(svc.SecurityTweaks());
    }

    [Fact]
    public void StabilityTweaks_ReturnsNonEmptyList()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);
        Assert.NotEmpty(svc.StabilityTweaks());
    }

    // ── Bucket IDs are valid registered tweaks ────────────────────────────

    [Fact]
    public void PrivacyTweaks_AllIdsAreRegistered()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);
        var allIds = engine.AllTweaks().Select(t => t.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

        Assert.All(svc.PrivacyTweaks(), t => Assert.Contains(t.Id, allIds));
    }
}
