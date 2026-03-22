// tests/RegiLattice.Core.Tests/HealthScoreServicePreviewTests.cs
// Sprint 71 — HealthScoreService.PreviewCategoryImpact tests.

#nullable enable

using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for Sprint 71: HealthScoreService.PreviewCategoryImpact.</summary>
public sealed class HealthScoreServicePreviewTests : IClassFixture<BuiltinsFixture>
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
