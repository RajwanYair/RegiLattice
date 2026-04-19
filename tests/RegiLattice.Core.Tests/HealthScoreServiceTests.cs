// tests/RegiLattice.Core.Tests/HealthScoreServiceTests.cs
// HealthScoreService bucket membership and score calculation tests.

using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>HealthScoreService tests.</summary>
[Collection("Builtins")]
public sealed class HealthScoreServiceTests
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

// ── merged from HealthScoreServicePreviewTests.cs ──────────────────────────────────
/// <summary>HealthScoreService.PreviewCategoryImpact tests.</summary>
[Collection("Builtins")]
public sealed class HealthScoreServicePreviewTests
{
    private readonly TweakEngine _engine;

    public HealthScoreServicePreviewTests(BuiltinsFixture fixture)
    {
        _engine = fixture.Engine;
    }

    private TweakEngine BuildEngine() => _engine;

    // ── Argument validation ───────────────────────────────────────────────

    [Fact]
    public void PreviewCategoryImpact_NullCategory_Throws()
    {
        var svc = new HealthScoreService(BuildEngine());
        var map = new Dictionary<string, TweakResult>();
        Assert.Throws<ArgumentNullException>(() => svc.PreviewCategoryImpact(null!, map));
    }

    [Fact]
    public void PreviewCategoryImpact_EmptyCategory_Throws()
    {
        var svc = new HealthScoreService(BuildEngine());
        var map = new Dictionary<string, TweakResult>();
        Assert.Throws<ArgumentException>(() => svc.PreviewCategoryImpact(string.Empty, map));
    }

    [Fact]
    public void PreviewCategoryImpact_NullStatusMap_Throws()
    {
        var svc = new HealthScoreService(BuildEngine());
        Assert.Throws<ArgumentNullException>(() => svc.PreviewCategoryImpact("Privacy", null!));
    }

    // ── Before/After semantics ────────────────────────────────────────────

    [Fact]
    public void PreviewCategoryImpact_AllAlreadyApplied_BeforeEqualsAfter()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);

        // Mark every tweak in Privacy as already applied.
        var byCategory = engine.TweaksByCategory();
        if (!byCategory.TryGetValue("Privacy", out var privacyTweaks) || privacyTweaks.Count == 0)
            return; // skip if category absent

        var map = privacyTweaks.ToDictionary(t => t.Id, _ => TweakResult.Applied, StringComparer.OrdinalIgnoreCase);

        var (before, after) = svc.PreviewCategoryImpact("Privacy", map);

        Assert.Equal(before.Privacy, after.Privacy);
        Assert.Equal(before.Performance, after.Performance);
        Assert.Equal(before.Security, after.Security);
        Assert.Equal(before.Stability, after.Stability);
        Assert.Equal(before.Overall, after.Overall);
    }

    [Fact]
    public void PreviewCategoryImpact_NoneApplied_AfterScoreGreaterOrEqual()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);

        var (before, after) = svc.PreviewCategoryImpact("Privacy", new Dictionary<string, TweakResult>());

        // Applying tweaks should only raise (or keep equal) the score.
        Assert.True(after.Privacy >= before.Privacy);
        Assert.True(after.Overall >= before.Overall);
    }

    [Fact]
    public void PreviewCategoryImpact_Privacy_AfterPrivacyScorePositiveOrEqual()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);

        var (before, after) = svc.PreviewCategoryImpact("Privacy", new Dictionary<string, TweakResult>());

        // After score must be ≥ before (applying tweaks never decreases score).
        Assert.True(after.Privacy >= before.Privacy, $"After={after.Privacy} should be >= Before={before.Privacy}");
    }

    [Fact]
    public void PreviewCategoryImpact_UnknownCategory_BothScoresEqual()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);

        var (before, after) = svc.PreviewCategoryImpact("ZZZ_NonExistentCategory_ZZZ", new Dictionary<string, TweakResult>());

        // An unknown category has no tweaks to simulate, so scores are identical.
        Assert.Equal(before.Privacy, after.Privacy);
        Assert.Equal(before.Overall, after.Overall);
    }

    // ── Score bounds ──────────────────────────────────────────────────────

    [Fact]
    public void PreviewCategoryImpact_Scores_AreInZeroToHundredRange()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);

        var (before, after) = svc.PreviewCategoryImpact("Performance", new Dictionary<string, TweakResult>());

        Assert.InRange(before.Performance, 0, 100);
        Assert.InRange(after.Performance, 0, 100);
        Assert.InRange(before.Overall, 0, 100);
        Assert.InRange(after.Overall, 0, 100);
    }

    // ── Selectively applied map does not regress ──────────────────────────

    [Fact]
    public void PreviewCategoryImpact_PartiallyApplied_AfterScoreAtLeastBefore()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);

        var byCategory = engine.TweaksByCategory();
        if (!byCategory.TryGetValue("Performance", out var perfTweaks) || perfTweaks.Count == 0)
            return; // skip if absent

        // Apply only the first half of performance tweaks.
        var halfApplied = perfTweaks.Take(perfTweaks.Count / 2).ToDictionary(t => t.Id, _ => TweakResult.Applied, StringComparer.OrdinalIgnoreCase);

        var (before, after) = svc.PreviewCategoryImpact("Performance", halfApplied);

        Assert.True(after.Overall >= before.Overall, $"After={after.Overall} should be >= Before={before.Overall}");
    }

    // ── Tuple structure ───────────────────────────────────────────────────

    [Fact]
    public void PreviewCategoryImpact_ReturnsTuple_WithBeforeAndAfter()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);

        var result = svc.PreviewCategoryImpact("Security", new Dictionary<string, TweakResult>());

        // Verify both components are well-typed HealthScore records.
        Assert.NotNull(result.Before);
        Assert.NotNull(result.After);
    }

    [Fact]
    public void PreviewCategoryImpact_Before_MatchesComputeOnSameMap()
    {
        var engine = BuildEngine();
        var svc = new HealthScoreService(engine);
        var map = new Dictionary<string, TweakResult>();

        var baseline = svc.Compute(map);
        var (before, _) = svc.PreviewCategoryImpact("Security", map);

        // The "Before" in the preview must equal a direct Compute() call on the same map.
        Assert.Equal(baseline.Privacy, before.Privacy);
        Assert.Equal(baseline.Performance, before.Performance);
        Assert.Equal(baseline.Security, before.Security);
        Assert.Equal(baseline.Stability, before.Stability);
        Assert.Equal(baseline.Overall, before.Overall);
    }
}
